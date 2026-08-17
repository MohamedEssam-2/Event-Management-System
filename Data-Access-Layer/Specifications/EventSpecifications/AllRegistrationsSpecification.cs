using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class AllRegistrationsSpecification :BaseSpecification<Registration,int>
    {
        public AllRegistrationsSpecification():base(r => true)
        {
            AddInclude(r => r.Event);
            AddInclude(r => r.User);
        }
    }
}
