using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class UserEventReviewSpecification : BaseSpecification<Review,int>
    {
        public UserEventReviewSpecification(string userId,int EventId):base(ue => ue.UserId == userId && ue.EventId == EventId)
        {
            
        }
    }
}
