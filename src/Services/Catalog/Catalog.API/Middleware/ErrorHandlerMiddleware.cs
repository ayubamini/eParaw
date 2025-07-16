using Catalog.Application.Common.Exceptions;
using Catalog.Domain.Common;
using System.Net;
using System.Text.Json;


namespace Catalog.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
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
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new ErrorResponse();

                switch (error)
                {
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = e.Message;
                        responseModel.Errors = e.Errors;
                        break;
                    case NotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.Message = e.Message;
                        break;
                    case DomainException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = e.Message;
                        break;
                    default:
                        _logger.LogError(error, "An unhandled exception occurred");
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = "An error occurred while processing your request.";
                        break;
                }

                var jsonResponse = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await response.WriteAsync(jsonResponse);
            }
        }

        private class ErrorResponse
        {
            public string Message { get; set; }
            public IDictionary<string, string[]> Errors { get; set; }
        }
    }

}
