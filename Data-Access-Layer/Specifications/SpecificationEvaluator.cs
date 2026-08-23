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
            if (specification.Criteria is not null)
            {
                query = query.Where(specification.Criteria);
            }
            if (specification is not null && specification.Include_Expressions.Any())
            {

                foreach (var includeExpression in specification.Include_Expressions)
                {
                    query = query.Include(includeExpression);
                }
            }
            if(specification!.OrderBy is not null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            if (specification.OrderByDescending is not null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            if (specification.IsPaginated)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }
            return query;
        }
    }
}
