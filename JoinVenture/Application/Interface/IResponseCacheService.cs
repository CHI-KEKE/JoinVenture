using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IResponseCacheService
    {
		Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);

		Task<string> GetCacheResponseAsync(string cacheKey);

        Task<object> RemoveDataAsync(string cacheKey);
    }
}