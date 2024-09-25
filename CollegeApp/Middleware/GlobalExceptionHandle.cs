using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace CollegeApp.Infrastructure
{
    public class GlobalExceptionHandle : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandle> _logger;
        public GlobalExceptionHandle(ILogger<GlobalExceptionHandle> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
            _logger.LogError(exception, $"Exception {exception.Message}, TraceId : {traceId}");

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string title = "Internal Server Error";
            string detail = exception.Message;
            string typeUrl = "https://docs.microsoft.com/en-us/rest/api/storageservices/overview#500-internal-server-error";

            if (exception is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;
                title = "Bad Request";
                typeUrl = "https://docs.microsoft.com/en-us/rest/api/storageservices/overview#400-bad-request";
            }
            else if (exception is AuthenticationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                title = "Unauthorized";
                typeUrl = "https://docs.microsoft.com/en-us/rest/api/storageservices/overview#401-unauthorized";
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Forbidden;
                title = "Forbidden";
                typeUrl = "https://docs.microsoft.com/en-us/rest/api/storageservices/overview#403-forbidden";
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                title = "Resource Not Found";
                typeUrl = "https://docs.microsoft.com/en-us/rest/api/storageservices/overview#404-not-found";
            }
            else if (exception is NotImplementedException)
            {
                statusCode = HttpStatusCode.ServiceUnavailable;
                title = "Service Unavailable";
                typeUrl = "https://docs.microsoft.com/en-us/rest/api/storageservices/overview#503-service-unavailable";
            }

            var details = new ProblemDetails
            {
                Type = typeUrl,
                Title = title,
                Status = (int)statusCode,
                Detail = detail,
                Instance = httpContext.Request.Path,
                Extensions = { { "traceId", traceId} }
            };

            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)statusCode;

            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;

        }
    }
}
