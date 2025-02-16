namespace APIGateway.Middleware
{
    public class AttachSignatureToRequest(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers["ApiGateway"] = "Signed";
            await next(context);
        }
    }
}
