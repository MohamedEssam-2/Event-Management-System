using System.Text;
using Business_Logic_Layer.Service.Implementation;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation_Logic_Layer.Attributes
{
    public class CacheRedisAttribute(int duration=10) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();
            var key = GenerateKey(context.HttpContext);

            Console.WriteLine($"CACHE KEY: {key}");

            var result = await cacheService.GetAsync(key);

            Console.WriteLine($"CACHE RESULT: {result}");
            //Cache Hit
            if (result != null)
            {
                context.Result = new ContentResult()
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            //Cache Miss
            var resultContext = await next.Invoke();
            if (resultContext.Result is OkObjectResult okObject)
            {
                await cacheService.SetAsync(key, okObject.Value, TimeSpan.FromMinutes(duration));
            }

        }
        private string GenerateKey(HttpContext httpContext)
        {
            var key = new StringBuilder();
            key.Append($"{httpContext.Request.Path}");
            foreach (var item in httpContext.Request.Query.OrderBy(x => x.Key))
            {
                key.Append($"|{item.Key}-{item.Value}");
            }
            return key.ToString();

        }
    }
}
