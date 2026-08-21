using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Enum;

namespace Data_Access_Layer.Models
{
    public class Payment:BaseEntity<int>
    {
        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public string? StripeSessionId { get; set; }

        public string? StripePaymentIntentId { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; } = null!;
    }
}
