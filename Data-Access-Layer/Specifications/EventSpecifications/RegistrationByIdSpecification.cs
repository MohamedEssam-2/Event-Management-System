using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class RegistrationByIdSpecification : BaseSpecification<Registration, int>
    {
        public RegistrationByIdSpecification(int registrationId): base(r => r.Id == registrationId)
        {
            AddInclude(e => e.User);
            AddInclude(e => e.Event);
        }
    }
}
