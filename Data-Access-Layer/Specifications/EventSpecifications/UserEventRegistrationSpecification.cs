using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class UserEventRegistrationSpecification : BaseSpecification<Registration, int>
    {
        public UserEventRegistrationSpecification(string userid,int eventid):base(r => r.UserId == userid && r.EventId == eventid)
        {
            
        }
    }
}
