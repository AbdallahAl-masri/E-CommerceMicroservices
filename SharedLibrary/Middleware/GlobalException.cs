using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Logs;
using System.Text.Json;


namespace SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string message = "Sorry, internal server error occurred. Try again";
            int statusCode = StatusCodes.Status500InternalServerError;
            string title = "Error";

            try
            {
                await next(context);

                //if Response is 429 TooManyRequests
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    message = "Too many requests are made";
                    statusCode = StatusCodes.Status429TooManyRequests;
                    title = "Warning";
                    await ModifyHeader(context, message, statusCode, title);
                }

                //if Response is 401 Unauthorized
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    message = "Unauthorized access";
                    statusCode = StatusCodes.Status401Unauthorized;
                    title = "Alert";
                    await ModifyHeader(context, message, statusCode, title);
                }

                //if Response is 403 Forbidden
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    message = "Forbidden access";
                    statusCode = StatusCodes.Status403Forbidden;
                    title = "Out of Access";
                    await ModifyHeader(context, message, statusCode, title);
                }


            }
            catch (Exception ex)
            {
                // Log the exception
                LogException.LogExceptions(ex);

                // if Exeption is of type TaskCanceledException or OperationCanceledException
                if (ex is TaskCanceledException || ex is OperationCanceledException)
                {
                    message = "Request Timedout... try again";
                    statusCode = StatusCodes.Status408RequestTimeout;
                    title = "Out of Time";
                }

                //if none of the above then Internal Server Error (default)
                await ModifyHeader(context, message, statusCode, title);
            }
        }

        private static async Task ModifyHeader(HttpContext context, string message, int statusCode, string title)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = message
            }), CancellationToken.None);
            return;
        }
    }
}
