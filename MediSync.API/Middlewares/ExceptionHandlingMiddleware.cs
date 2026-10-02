using Azure;
using System.Net;
using System.Text.Json;

namespace MediSync.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;
        public ExceptionHandlingMiddleware( RequestDelegate _next , ILogger<ExceptionHandlingMiddleware>_logger   )
        {
            next = _next;
            logger = _logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch ( Exception e )
            {
                logger.LogError(e, "حدث خطا غير متوقع:{Message}", e.Message); 
                await HandleExceptionAsync(context,e);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";
            var statusCode = e switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                ArgumentException or InvalidOperationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError

            };
            context.Response.StatusCode = (int)statusCode;
            var response = new
            {
                statusCode = context.Response.StatusCode,
                Message = e.Message,
                detailed = statusCode == HttpStatusCode.InternalServerError ? "Error" : null
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response))    ;
        }
    }
}
