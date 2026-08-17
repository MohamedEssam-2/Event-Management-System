using System.Linq.Expressions;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Interface;

namespace Data_Access_Layer.Specifications.Base
{
    public abstract class BaseSpecification<TEntity, Tkey> : ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; }

        protected BaseSpecification(Expression<Func<TEntity,bool>> criteriaExpression )
        {
            Criteria = criteriaExpression;
        }

        public ICollection<Expression<Func<TEntity, object>>> Include_Expressions { get; } = [];

        public void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Include_Expressions.Add(includeExpression);
        }


    
        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

        public void AddOrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            OrderBy = orderBy;
        }
        public void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
        }



        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPaginated { get; private set; }

 
        public void ApplyPagination(int PageSize, int PageIndex)
        {
            IsPaginated = true;
            Take = PageSize;
            Skip = (PageIndex - 1) * PageSize;
        }
    }
}
