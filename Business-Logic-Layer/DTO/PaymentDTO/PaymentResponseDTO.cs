using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.PaymentDTO
{
    public class PaymentResponseDTO
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public string CheckoutUrl { get; set; } = null!;
    }
}
