using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class CategoryWithEventSpecifications : BaseSpecification<Category, int>
    {
        //Get by id
        public CategoryWithEventSpecifications(int id) : base(c => c.Id == id)
        {
            AddInclude(e => e.Events);
        }

        public CategoryWithEventSpecifications(string Search):base(C => string.IsNullOrWhiteSpace(Search) || C.Name.ToLower().Contains(Search.ToLower()))
        {
            
        }
    }
}
