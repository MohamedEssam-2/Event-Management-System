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
    public class ReminderDueRegistrationsSpecification : BaseSpecification<Registration, int>
    {
        public ReminderDueRegistrationsSpecification(DateTime nowUtc, double hoursBefore, int batchSize)
            : base(r => !r.IsDeleted
                     && r.ReminderSentAt == null
                     && !r.Event.IsDeleted
                     && r.Event.Status == EventStatus.Scheduled
                     && r.Event.Date > nowUtc
                     && r.Event.Date <= nowUtc.AddHours(hoursBefore)
                     && (r.RegistrationDate == null
                         || r.RegistrationDate <= r.Event.Date.AddHours(-hoursBefore)))
        {
            AddInclude(r => r.Event);
            AddInclude(r => r.User);
            AddOrderBy(r => r.Event.Date);
            ApplyPagination(batchSize, 1);
        }
    }
}
