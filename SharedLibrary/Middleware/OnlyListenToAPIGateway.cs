using Microsoft.AspNetCore.Http;

namespace SharedLibrary.Middleware
{
    public class OnlyListenToAPIGateway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Extract the ApiGateway header from the request
            var signHeader = context.Request.Headers["ApiGateway"];

            // null means the request is not from the API Gateway // 503 Service Unavailable
            if (signHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry, service is unavailable");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
