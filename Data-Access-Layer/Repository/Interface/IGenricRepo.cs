using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Interface;

namespace Data_Access_Layer.Repository.Interface
{
  
        public interface IGenericRepo<TEntity,TKey> where TEntity : BaseEntity<TKey>
        {
        public Task<IEnumerable<TEntity>> GetAll(ISpecification<TEntity,TKey>spec);
        public Task<List<TEntity>> GetAll();
          public Task<TEntity?> GetById(TKey id);
        public Task<TEntity?> GetById(ISpecification<TEntity, TKey> specification);
        public Task<TEntity> Create(TEntity entity);
          public void Update(TEntity entity);
          public void Delete(TEntity entity);
        }
    
}
