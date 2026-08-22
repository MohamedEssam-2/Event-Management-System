using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class OrderWithUserAndEventSpecification:BaseSpecification<Order,int>
    {
        public OrderWithUserAndEventSpecification(int orderId)
       : base(o => o.Id == orderId)
        {
            AddInclude(o => o.User);
            AddInclude(o => o.Event);
        }
    }
}
