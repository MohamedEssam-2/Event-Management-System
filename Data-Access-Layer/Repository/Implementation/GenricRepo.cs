using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Database;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Data_Access_Layer.Specifications;
using Data_Access_Layer.Specifications.Interface;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Data_Access_Layer.Repository.Implementation
{
    public class GenricRepo<TEntity, TKey>(EventContext _context) : IGenericRepo<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public async Task<TEntity> Create(TEntity entity)
        {
             await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            _context.Set<TEntity>().Update(entity);
        }

        public async Task<TEntity?> GetById(TKey id)
        {
          return await _context.Set<TEntity>().FindAsync(id);
        }
        public async Task<TEntity?> GetById(ISpecification<TEntity, TKey> specification)
        {
            return await SpecificationEvaluator.CreateQuery(_context.Set<TEntity>(), specification).FirstOrDefaultAsync();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll(ISpecification<TEntity, TKey> spec)
        {
            var query = SpecificationEvaluator.CreateQuery(_context.Set<TEntity>(), spec);
            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity, TKey> specification)
        {
            IQueryable<TEntity> Query = _context.Set<TEntity>();
            if(specification.Criteria is not null)
            {
                Query = Query.Where(specification.Criteria);
            }
            return await Query.CountAsync();
        }
    }
}
