using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class ReistrationByEventId : BaseSpecification<Registration, int>
    {
        public ReistrationByEventId(int eventId , int PageIndex ,int PageSize)
          : base(r => r.EventId == eventId && !r.IsDeleted )
        {
            AddInclude(u => u.User);
            AddInclude(u => u.Event);
            ApplyPagination(PageSize, PageIndex);
        }

    }
}
