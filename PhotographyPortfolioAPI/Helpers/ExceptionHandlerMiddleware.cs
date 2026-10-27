using PhotographyPortfolioAPI.Contracts.Common;
using System.Net;
using System.Text.Json;

namespace PhotographyPortfolioAPI.Helpers
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try { await _next(context); }
            catch (Exception ex) { await HandleExceptionAsync(context, ex); }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = exception switch
            {
                UnauthorizedAccessException => (401, ApiResponseData<object>.Error(exception.Message, 401)),
                InvalidOperationException => (400, ApiResponseData<object>.Error(exception.Message, 400)),
                ArgumentException => (400, ApiResponseData<object>.Error(exception.Message, 400)),
                KeyNotFoundException => (404, ApiResponseData<object>.Error(exception.Message, 404)),
                _ => (500, ApiResponseData<object>.Error(exception.Message, 500))
            };
            context.Response.StatusCode = response.Item1;
            return context.Response.WriteAsync(JsonSerializer.Serialize(response.Item2));
        }
    }
}
