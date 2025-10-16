using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LakeCalendarApi.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("x-api-key", out var extractedApiKey)) 
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key was not provided"
                };

                return;
            }
            var appsettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKeys = appsettings.GetSection("ApiKeys").Get<List<string>>();
            
            if (!apiKeys!.Contains(extractedApiKey!))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key is not valid"
                };

                return;
            }
            await next();
        }

        


    }
}
