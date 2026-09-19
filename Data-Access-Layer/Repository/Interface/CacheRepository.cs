using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Repository.Implementation;
using StackExchange.Redis;

namespace Data_Access_Layer.Repository.Interface
{
    public class CacheRepository(IConnectionMultiplexer _connection ) : ICacheRepository
    {
        public readonly IDatabase _database = _connection.GetDatabase();
        public async Task<string?> GetAsync(string key)
        {
            var res= await _database.StringGetAsync(key);
            return res.IsNullOrEmpty ? null : res.ToString();
        }

        public async Task SetAsync(string key, string value, TimeSpan duration )
        {
            var result = await _database.StringSetAsync(key, value, duration);
            var test = await _database.StringGetAsync(key);
        }

        public async Task RemoveAsync(string key)
        {
            var result=await _database.KeyDeleteAsync(key);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            var endpoints = _connection.GetEndPoints().First();
            var server = _connection.GetServer(endpoints);
            var keys = server.Keys(pattern: $"{prefix}*");
            foreach (var key in keys)
            {
                await _database.KeyDeleteAsync(key);
            }
        }
    }
}
