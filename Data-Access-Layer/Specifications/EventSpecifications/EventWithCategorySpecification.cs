using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class EventWithCategorySpecification  :BaseSpecification<Event,int>
    {

        public EventWithCategorySpecification(int id ):base(e=>e.Id==id)
        {
            AddInclude(e => e.Category);
        }
        //Get all 
        public EventWithCategorySpecification(string Search): base(E=>string.IsNullOrWhiteSpace(Search) ||E.Name.ToLower().Contains(Search.ToLower()))
        {
            AddInclude(e => e.Category);
  
        }

       
    }
}
