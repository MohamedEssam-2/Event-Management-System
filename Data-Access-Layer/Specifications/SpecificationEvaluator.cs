using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Interface;
using Microsoft.EntityFrameworkCore;


namespace Data_Access_Layer.Specifications
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, Tkey>(IQueryable<TEntity> EntryPoint, ISpecification<TEntity, Tkey> specification)
            where TEntity : BaseEntity<Tkey>
        {
            var query = EntryPoint;
            if (specification is not null && specification.Include_Expressions.Any())
            {
                foreach (var includeExpression in specification.Include_Expressions)
                {
                    query = query.Include(includeExpression);
                }
            }
            return query;
        }
    }
}
