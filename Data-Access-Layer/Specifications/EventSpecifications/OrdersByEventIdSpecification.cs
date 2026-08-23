using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class OrdersByEventIdSpecification:BaseSpecification<Order, int>
    {
        public OrdersByEventIdSpecification(int eventId ,int PageIndex , int PageSize , string? sortBy):base(o =>o.EventId==eventId)
        {
            AddInclude(u=> u.User);
            AddInclude(u => u.Event);
            switch(sortBy)
            {
                case "priceAsc":
                    AddOrderBy(o => o.Amount);
                    break;
                case "priceDesc":
                    AddOrderByDescending(o => o.Amount);
                    break;
                case "dateAsc":
                    AddOrderBy(o => o.CreatedAt!);
                    break;
                case "dateDesc":
                    AddOrderByDescending(o => o.CreatedAt!);
                    break;
                default:
                    AddOrderByDescending(o => o.Id);
                    break;
            }
            ApplyPagination(PageSize, PageIndex);
        }
    }
}
