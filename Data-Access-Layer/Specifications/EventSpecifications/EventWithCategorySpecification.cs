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
        public EventWithCategorySpecification(string Search, int PageIndex, int PageSize, string sortBy) : base(E=>string.IsNullOrWhiteSpace(Search) ||E.Name.ToLower().Contains(Search.ToLower()))
        {
            AddInclude(e => e.Category);
            switch(sortBy)
            {
                case "nameAsc":
                    AddOrderBy(e => e.Name);
                    break;
                case "nameDesc":
                    AddOrderByDescending(e => e.Name);
                    break;
                case "dateAsc":
                    AddOrderBy(e => e.Date);
                    break;
                case "dateDesc":
                    AddOrderByDescending(e => e.Date);
                    break;
                case "priceAsc":
                    AddOrderBy(e => e.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(e => e.Price);
                    break;
                default:
                    AddOrderBy(e => e.Id);
                    break;
            }

            ApplyPagination(PageSize, PageIndex);
        }

       
    }
}
