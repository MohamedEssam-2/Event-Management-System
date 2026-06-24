using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Database;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;

namespace Data_Access_Layer.Repository.Implementation
{
    public class UnitOfWork(EventContext _context) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];
        public IGenericRepo<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var RepoName = typeof(TEntity).Name;
            if (_repositories.ContainsKey(RepoName))
            {
                return (IGenericRepo<TEntity, TKey>)_repositories[RepoName];
            }
            else
            {
                var repoInstance = new GenricRepo<TEntity, TKey>(_context);
                _repositories.Add(RepoName, repoInstance);
                return repoInstance;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
