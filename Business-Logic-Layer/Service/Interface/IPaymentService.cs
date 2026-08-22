using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.PaymentDTO;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IPaymentService
    {
        public Task<ServiceResponse<PaymentResponseDTO>> CreateCheckoutSession(int orderId);
        public Task HandleWebhookAsync(string json, string stripeSignature);
    }       
}
