using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;
using Data_Access_Layer.Specifications.Interface;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class MyEventsSpecification : BaseSpecification<Event,int>
    {
        public MyEventsSpecification(string userId):base(e=>e.OrganizerId==userId)
        {
            AddInclude(e => e.Category);
        }
    }
}
