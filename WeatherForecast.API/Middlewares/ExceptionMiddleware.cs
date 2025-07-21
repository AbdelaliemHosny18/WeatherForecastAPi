using System.Net;
using System.Text.Json;
using WeatherForecast.Domain.Exceptions;

namespace WeatherForecast.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                UserAlreadyExistsException => (int)HttpStatusCode.Conflict,            // 409
                InvalidCredentialsException => (int)HttpStatusCode.Unauthorized,       // 401
                NotFoundException => (int)HttpStatusCode.NotFound,                     // 404
                BadRequestException => (int)HttpStatusCode.BadRequest,                 // 400
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,            // 401
                _ => (int)HttpStatusCode.InternalServerError                           // 500
            };

            var result = JsonSerializer.Serialize(new
            {
                status = statusCode,
                message = exception.Message
            });

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
