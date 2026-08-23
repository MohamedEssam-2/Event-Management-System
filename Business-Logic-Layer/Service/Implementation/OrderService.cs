using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.OrderDTO;
using Business_Logic_Layer.DTO.PaginationDTO;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Enum;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Data_Access_Layer.Specifications.EventSpecifications;

namespace Business_Logic_Layer.Service.Implementation
{
    public class OrderService(IUnitOfWork _unitOfWork,ICurrentUserService _currentUser ,IMapper _mapper) : IOrderService
    {
        public async Task<ServiceResponse<int>> CreateOrder(int EventId)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var eventResult = await _unitOfWork.GetRepository<Event, int>().GetById(EventId);
            if (eventResult == null)
            {
                throw new EventNotFoundException(EventId);
            }
            if(eventResult.IsDeleted)
            {
                throw new EventNotFoundException(EventId);
            }
            if(eventResult.Status == EventStatus.Canceled)
            {
                throw new BadRequestException("Cant Order Event is canceled . ");
            }
            if (eventResult.Status == EventStatus.Completed)
            {
                throw new BadRequestException("Can't order a completed event.");
            }
            if (eventResult.Price <= 0)
            {
                throw new BadRequestException("Cant Order Event Price is Free.");
            }
            if (eventResult.Date <= DateTime.UtcNow)
            {
                throw new BadRequestException("Can't order an event that has already started or ended."
                );
            }
            var registrationSpec =new UserEventRegistrationSpecification(userId, EventId);
            var registration =await _unitOfWork.GetRepository<Registration, int>().CountAsync(registrationSpec);
            if (registration > 0)
            {
                throw new BadRequestException("You are already registered for this event.");
            }
            var existingRegistration = new EventRegistrationsSpecification(EventId);
            var registrations =await _unitOfWork.GetRepository<Registration, int>().CountAsync(existingRegistration);

            if (eventResult.MaxAttendees.HasValue && registrations >= eventResult.MaxAttendees.Value)
            {
                throw new BadRequestException("Event has reached its maximum number of attendees.");
            }
            var pendingOrderSpec =new UserEventPendingOrderSpecification(userId, EventId);
            var pendingOrder =await _unitOfWork.GetRepository<Order, int>().CountAsync(pendingOrderSpec);
            if (pendingOrder > 0)
            {
                throw new BadRequestException("You already have a pending order for this event.");
            }
            var order = new Order
            {
                UserId = userId,
                EventId = EventId,
                Amount = eventResult.Price,
                Status = OrderStatus.Pending,
                //OrderDate = DateTime.UtcNow,
                CreatedBy = _currentUser.FullName,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<Order, int>().Create(order);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<int>
            {
                Data = order.Id,
                Message = "Order created successfully.",
                Success = true
            };
        }



        public async Task<ServiceResponse<List<ReadOrderDTO>>> GetMyOrders()
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var Spec = new UserOrdersSpecification(userId);
            var Myorders = await _unitOfWork.GetRepository<Order, int>().GetAll(Spec);
            var orderDto= _mapper.Map<List<ReadOrderDTO>>(Myorders);
            return new ServiceResponse<List<ReadOrderDTO>>
            {
                Success = true,
                Message = "Orders retrieved successfully.",
                Data = orderDto,
            };


        }

        public async Task<ServiceResponse<bool>> DeleteOrder(int OrderId)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
          
            var order=await _unitOfWork.GetRepository<Order,int>().GetById(OrderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with Id = {OrderId} is not found");
            }
            if (order.UserId != userId)
            {
                throw new UnauthorizedException("You are not authorized to delete this order.");
            }
            if (order.Status == OrderStatus.Paid)
            {
                throw new BadRequestException("Cannot delete a paid order.");
            }
            if (order.Status == OrderStatus.Canceled)
            {
                throw new BadRequestException("Order is already canceled.");
            }
            order.DeletedBy = _currentUser.FullName;
            order.DeletedDate = DateTime.UtcNow;
            _unitOfWork.GetRepository<Order, int>().Delete(order);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Success = true,
                Message = "Order deleted successfully.",
                Data = true
            };


        }

        public async Task<ServiceResponse<ReadOrderDTO>> GetOrderById(int OrderId)
        {
            var userId = _currentUser.UserId;

            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var spec = new OrdersByIdSpecification(OrderId);
            var order =await _unitOfWork.GetRepository<Order, int>().GetById(spec);
            if (order == null)
            {
                throw new NotFoundException($"Order with Id = {OrderId} is not found");
            }

            if (order.UserId != userId)
            {
                throw new UnauthorizedException(
                    "You are not authorized to view this order."
                );
            }
            var orderDto = _mapper.Map<ReadOrderDTO>(order);
            return new ServiceResponse<ReadOrderDTO>
            {
                Success = true,
                Message = "Order retrieved successfully.",
                Data = orderDto
            };
        }

        public async Task<ServiceResponse<PagedResultDTO<ReadOrderDTO>>> GetAllOrders(int PageIndex, int PageSize, string? sortBy)
        {
            var spec = new AllOrdersSpecification(PageIndex,PageSize,sortBy!);
            var orders = await _unitOfWork.GetRepository<Order, int>().GetAll(spec);
            var orderDTOs = _mapper.Map<List<ReadOrderDTO>>(orders);
            var totalOrders = await _unitOfWork.GetRepository<Order, int>().CountAsync(spec);
            var result = new PagedResultDTO<ReadOrderDTO>
            {
                TotalCount = totalOrders,
                PageIndex = PageIndex,
                PageSize = PageSize,
                Data = orderDTOs
            };
            return new ServiceResponse<PagedResultDTO<ReadOrderDTO>>
            {
                Success = true,
                Message = "Orders retrieved successfully.",
                Data = result
            };

        }

        public async Task<ServiceResponse<PagedResultDTO<ReadOrderDTO>>> GetOrdersByEventId(int EventId, int PageIndex, int PageSize, string? sortBy)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var eventResult =await _unitOfWork.GetRepository<Event, int>().GetById(EventId);
            if (eventResult == null)
            {
                throw new EventNotFoundException(EventId);
            }
            if(eventResult.IsDeleted)
            {
                throw new EventNotFoundException(EventId);
            }
            if (eventResult.Status == EventStatus.Canceled)
            {
                throw new BadRequestException("Event is canceled.");
            }
            var spec = new OrdersByEventIdSpecification(EventId, PageIndex, PageSize, sortBy);
            var orders = await _unitOfWork.GetRepository<Order, int>().GetAll(spec);
            var eventOrderDTOs = _mapper.Map<List<ReadOrderDTO>>(orders);
            var totalOrders = await _unitOfWork.GetRepository<Order, int>().CountAsync(spec);
            var result = new PagedResultDTO<ReadOrderDTO>
            {
                TotalCount = totalOrders,
                PageIndex = PageIndex,
                PageSize = PageSize,
                Data = eventOrderDTOs
            };
            return new ServiceResponse<PagedResultDTO<ReadOrderDTO>>
            {
                Success = true,
                Message = "Orders retrieved successfully.",
                Data = result
            };

        }

        public async Task<ServiceResponse<ReadOrderDTO>> CancelOrder(int OrderId)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var spec = new OrdersByIdSpecification(OrderId);
            var order = await _unitOfWork.GetRepository<Order, int>().GetById(spec);
            if (order == null)
            {
                throw new NotFoundException($"Order with Id = {OrderId} is not found");
            }
            if(order.IsDeleted)
            {
                throw new NotFoundException($"Order with Id = {OrderId} is not found");
            }
            if (order.UserId != userId)
            {
                throw new UnauthorizedException("You are not authorized to cancel this order.");
            }
            if (order.Status == OrderStatus.Paid)
            {
                throw new BadRequestException("Cannot cancel a paid order.");
            }
            if (order.Status == OrderStatus.Canceled)
            {
                throw new BadRequestException("Order is already canceled.");
            }
                order.Status = OrderStatus.Canceled;
                order.UpdatedBy = _currentUser.FullName;
                order.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.GetRepository<Order, int>().Update(order);
                await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<ReadOrderDTO>
            {
                Success = true,
                Message = "Order canceled successfully.",
                Data = _mapper.Map<ReadOrderDTO>(order)
            };

        }
    }
}
