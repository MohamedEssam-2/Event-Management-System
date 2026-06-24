using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repository.Interface
{
  
        public interface IGenericRepo<TEntity,TKey> where TEntity : BaseEntity<TKey>
        {
          public Task<List<TEntity>> GetAll();
          public Task<TEntity?> GetById(TKey id);
          public Task Create(TEntity entity);
          public void Update(TEntity entity);
          public void Delete(TEntity entity);
        }
    
}
