using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class ActivityDetailCacheAttribute: Attribute,IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        public ActivityDetailCacheAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
            
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userAccessor = context.HttpContext.RequestServices.GetRequiredService<IUserAccessor>();

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cacheResponse = await cacheService.GetCacheResponseAsync(cacheKey);

            if(!string.IsNullOrEmpty(cacheResponse))
            {
                Console.WriteLine(cacheResponse+"Found CacheResponse&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };

                context.Result = contentResult;
                return;
            }

            var executedContext = await next();
            Console.WriteLine(cacheResponse+"not Found CacheResponse&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");

            if(executedContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
                Console.WriteLine("Create new CacheResponse&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");

            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {

                var keyBuilder = new StringBuilder();


                keyBuilder.Append($"{request.Path}");
   
                foreach(var (key,value) in request.Query.OrderBy(x => x.Key))
                {
                    keyBuilder.Append($"|{key}-{value}");
                }
                
                return keyBuilder.ToString();
        }
    }
}