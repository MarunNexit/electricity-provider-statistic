using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace electricity_provider_server_api.Middleware
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class UnauthorizedAccessAppException : Exception
    {
        public UnauthorizedAccessAppException(string message) : base(message) { }
    }

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not Found");
                await WriteProblemDetailsResponse(context, HttpStatusCode.NotFound, ex.Message, "https://httpstatuses.com/404");
            }
            catch (UnauthorizedAccessAppException ex)
            {
                _logger.LogWarning(ex, "Unauthorized");
                await WriteProblemDetailsResponse(context, HttpStatusCode.Forbidden, ex.Message, "https://httpstatuses.com/403");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation Failed");
                await WriteProblemDetailsResponse(context, HttpStatusCode.BadRequest, ex.Message, "https://httpstatuses.com/400");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                await WriteProblemDetailsResponse(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.", "https://httpstatuses.com/500");
            }
        }

        private async Task WriteProblemDetailsResponse(HttpContext context, HttpStatusCode statusCode, string detail, string type)
        {
            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = Enum.GetName(typeof(HttpStatusCode), statusCode),
                Detail = detail,
                Type = type,
                Instance = context.Request.Path
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, options));
        }
    }

}
