using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Interface;
using StackExchange.Redis;

namespace API.Service
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;

        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
            
        }
		public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
		{
				if (response == null)
				{
						return;
				}

				var options = new JsonSerializerOptions
				{
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};

				var serializedResponse = JsonSerializer.Serialize(response,options);
				
				await _database.StringSetAsync(cacheKey,serializedResponse, timeToLive);
		}

		public async Task<string> GetCacheResponseAsync(string cacheKey)
		{
				var cacheResponse = await _database.StringGetAsync(cacheKey);
				
				if(cacheResponse.IsNullOrEmpty)
				{
						return null;
				}
		
				return cacheResponse;
		}

        public async Task<object> RemoveDataAsync(string cacheKey)
        {
            var exists = await _database.KeyExistsAsync(cacheKey);

            if(exists)
            {
                Console.WriteLine( await _database.StringGetAsync(cacheKey) + "before deleteCacheKey%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
                await _database.KeyDeleteAsync(cacheKey);
                Console.WriteLine( await _database.StringGetAsync(cacheKey) + "after deleteCacheKey%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

				return await _database.StringGetAsync(cacheKey);

            }

            return false;
        }
    }
}