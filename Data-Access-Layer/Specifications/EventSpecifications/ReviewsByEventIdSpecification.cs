using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class ReviewsByEventIdSpecification:BaseSpecification<Review,int>
    {
        public ReviewsByEventIdSpecification(int eventId):base(e=>e.EventId == eventId)
        {
            AddInclude(r => r.User);
            AddInclude(r => r.Event);
        }
    }
}
