using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repository.Interface
{
    public interface IUnitOfWork
    {
        public IGenericRepo<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
        Task<int> SaveChangesAsync();
    }
}
