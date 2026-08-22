using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.PaymentDTO;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Enum;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Data_Access_Layer.Specifications.EventSpecifications;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;

namespace Business_Logic_Layer.Service.Implementation
{
    public class PaymentService(IUnitOfWork _unitOfWork, ICurrentUserService _currentUser, IOptions<StripeSettings> _stripeSettings) : IPaymentService
    {
        public async Task<ServiceResponse<PaymentResponseDTO>> CreateCheckoutSession(int orderId)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var order = await _unitOfWork.GetRepository<Order, int>().GetById(orderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with Id = {orderId} is not found.");
            }
            if (order.UserId != userId)
            {
                throw new UnauthorizedException("You are not authorized to pay for this order.");
            }
            if (order.Status != OrderStatus.Pending)
            {
                throw new BadRequestException("Only pending orders can be paid.");
            }
            var eventEntity = await _unitOfWork.GetRepository<Data_Access_Layer.Models.Event, int>().GetById(order.EventId);
            if (eventEntity == null)
            {
                throw new EventNotFoundException(order.EventId);
            }
            var existingPayment =await _unitOfWork.GetRepository<Payment, int>().GetById(new PaymentByOrderSpecification(orderId));
            if (existingPayment != null)
            {
                throw new BadRequestException("A payment already exists for this order.");
            }
            StripeConfiguration.ApiKey = _stripeSettings.Value.SecretKey;
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl =_stripeSettings.Value.SuccessUrl,
                CancelUrl = _stripeSettings.Value.CancelUrl,
                Metadata = new Dictionary<string, string>
        {
                {
                  "OrderId",order.Id.ToString()
                }
        },

                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                Quantity = 1,
                PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "USD",
                        UnitAmount =(long)(order.Amount * 100),
                        ProductData =new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = eventEntity.Name
                            }
                    }
            }
        }
            };

            var sessionService = new Stripe.Checkout.SessionService();
            var session =await sessionService.CreateAsync(options);

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.Amount,
                Status = PaymentStatus.Pending,
                StripeSessionId = session.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.FullName
            };
            await _unitOfWork.GetRepository<Payment, int>().Create(payment);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<PaymentResponseDTO>
            {
                Success = true,
                Message ="Checkout session created successfully.",
                Data = new PaymentResponseDTO
                {
                    PaymentId = payment.Id,
                    OrderId = order.Id,
                    CheckoutUrl = session.Url!
                }
            };
        }

        public async Task HandleWebhookAsync(string json, string stripeSignature)
        {
            var stripeEvent =EventUtility.ConstructEvent(json,stripeSignature,_stripeSettings.Value.WebhookSecret);
            if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
            {
                var session =stripeEvent.Data.Object as Stripe.Checkout.Session;
                if (session == null)
                {
                    throw new BadRequestException("Invalid Stripe session.");
                }
                if (!session.Metadata.TryGetValue("OrderId",out var orderIdValue))
                {
                    throw new BadRequestException("OrderId was not found in Stripe metadata.");
                }
                var orderId =int.Parse(orderIdValue);
                var payment =await _unitOfWork.GetRepository<Payment, int>().GetById(new PaymentByOrderSpecification(orderId));
                if (payment == null)
                {
                    throw new NotFoundException($"Payment for Order {orderId} was not found.");
                }
                if (payment.Status == PaymentStatus.Paid)
                {
                    return;
                }
                var order =await _unitOfWork.GetRepository<Order, int>().GetById(orderId);
                if (order == null)
                {
                    throw new NotFoundException($"Order with Id {orderId} is not found.");
                }
                payment.Status =PaymentStatus.Paid;
                payment.StripePaymentIntentId =session.PaymentIntentId;
                payment.UpdatedAt =DateTime.UtcNow;
                order.Status =OrderStatus.Paid;
                order.UpdatedAt =DateTime.UtcNow;

                var existingRegistration =await _unitOfWork.GetRepository<Registration, int>().CountAsync(new UserEventRegistrationSpecification(order.UserId,order.EventId));
                if (existingRegistration == 0)
                {
                    var registration = new Registration
                    {
                        UserId = order.UserId,
                        EventId = order.EventId,
                        RegistrationDate =DateTime.UtcNow,
                        CreatedAt =DateTime.UtcNow,
                        CreatedBy ="Stripe Webhook"
                    };
                    await _unitOfWork.GetRepository<Registration, int>().Create(registration);
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
