using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.PaymentDTO
{
    public class StripeSettings
    {
        public string SecretKey { get; set; } = null!;
        public string SuccessUrl { get; set; } = null!;
        public string CancelUrl { get; set; } = null!;
        public string WebhookSecret { get; set; } = null!;
    }
}
