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
    public class UpcomingEventsSpecification : BaseSpecification<Event, int>
    {
        public UpcomingEventsSpecification() : base(e => e.Status == EventStatus.Scheduled /*&&e.Date > DateTime.UtcNow*/)
        {
            AddInclude(e => e.Category);
            AddOrderBy(e => e.Date);
        }
    }
}
