using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class AllOrdersSpecification : BaseSpecification<Order, int>
    {
        public AllOrdersSpecification(int PageIndex, int PageSize,string? sortBy) : base(o => true)
        {
            AddInclude(o => o.Event);
            AddInclude(o => o.User);
            switch (sortBy)
            {

                case "dateAsc":
                    AddOrderBy(e => e.CreatedAt!);
                    break;
                case "dateDesc":
                    AddOrderByDescending(e => e.CreatedAt!);
                    break;
                case "priceAsc":
                    AddOrderBy(e => e.Amount);
                    break;
                case "priceDesc":
                    AddOrderByDescending(e => e.Amount);
                    break;
                default:
                    AddOrderBy(e => e.Id);
                    break;
            }

            ApplyPagination(PageSize, PageIndex);
        }
    }
}
