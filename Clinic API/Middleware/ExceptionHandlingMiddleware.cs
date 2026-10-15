using Clinic_BusinessLayer.Exceptions;
using System.Net;
using System.Text.Json;

namespace Clinic_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Application exception: {ErrorCode}", ex.ErrorCode);
                await HandleExceptionAsync(context, ex.StatusCode, ex.ErrorCode, ex.Message, null);
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage);
                _logger.LogWarning(ex, "Validation error");
                await HandleExceptionAsync(context, (int)HttpStatusCode.BadRequest,
                    "VALIDATION_ERROR", "Validation failed", errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                var message = _env.IsDevelopment()
                    ? ex.Message
                    : "Something went wrong. Please try again later.";

                IEnumerable<string>? details = _env.IsDevelopment()
                    ? new[] { ex.StackTrace ?? string.Empty }
                    : null;

                await HandleExceptionAsync(context, (int)HttpStatusCode.InternalServerError,
                    "INTERNAL_ERROR", message, details);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            int statusCode,
            string errorCode,
            string message,
            IEnumerable<string>? details)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                error = errorCode,
                message,
                details,
                traceId = context.TraceIdentifier,
                timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}