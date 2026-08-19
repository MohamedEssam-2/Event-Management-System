using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Enum;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
      public class UserEventPendingOrderSpecification : BaseSpecification<Order, int>
    {
        public UserEventPendingOrderSpecification(string userId, int eventId): base(o =>o.UserId == userId && o.EventId == eventId &&o.Status == OrderStatus.Pending)
        {

        }
    }
 
}
