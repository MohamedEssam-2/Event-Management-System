using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Interface;

namespace Data_Access_Layer.Specifications.Base
{
    public abstract class BaseSpecification<TEntity, Tkey> : ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public ICollection<Expression<Func<TEntity, object>>> Include_Expressions { get; } = [];
        public void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Include_Expressions.Add(includeExpression);
        }
    }
}
