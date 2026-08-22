using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class PaymentByOrderSpecification :BaseSpecification<Payment, int>
    {
        public PaymentByOrderSpecification(int orderId): base(p => p.OrderId == orderId)
        {
        }
    }
}
