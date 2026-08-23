using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class AllRegistrationsSpecification :BaseSpecification<Registration,int>
    {
        public AllRegistrationsSpecification(int PageIndex, int PageSize , string? sortBy) :base(r => true)
        {
            AddInclude(r => r.Event);
            AddInclude(r => r.User);
            switch (sortBy)
            {

                case "dateAsc":
                    AddOrderBy(e => e.CreatedAt!);
                    break;
                case "dateDesc":
                    AddOrderByDescending(e => e.CreatedAt!);
                    break;

                default:
                    AddOrderBy(e => e.Id);
                    break;
            }
            ApplyPagination(PageSize, PageIndex);
        }
    }
}
