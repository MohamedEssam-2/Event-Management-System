using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;

namespace Data_Access_Layer.Specifications.Interface
{
    public interface ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public ICollection<Expression<Func<TEntity, object>>> Include_Expressions { get; }
        public Expression<Func<TEntity, bool>>? Criteria { get; }
        public Expression<Func<TEntity, object>>? OrderBy { get;  }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; }
        public int Skip { get; }
        public int Take { get; }
        public bool IsPaginated { get; }

    }
}
