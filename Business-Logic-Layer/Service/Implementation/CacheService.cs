using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Repository.Implementation;
using Data_Access_Layer.Repository.Interface;

namespace Business_Logic_Layer.Service.Implementation
{
    public class CacheService(ICacheRepository _repository) : ICacheService
    {
        public async Task<string?> GetAsync(string key)
        {
            var cacheValue = await _repository.GetAsync(key);
            return cacheValue;
        }

        public Task SetAsync(string key, object cacheValue, TimeSpan duration)
        {
            var valueToReturn = JsonSerializer.Serialize(cacheValue, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return _repository.SetAsync(key, valueToReturn, duration);
        }
    }
}
