
using Business_Logic_Layer.DTO.ErrorDTO;
using Business_Logic_Layer.Exceptions;

namespace Presentation_Logic_Layer.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate _next,ILogger<ExceptionHandlingMiddleware> _logger,IWebHostEnvironment _environment)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException)when (context.RequestAborted.IsCancellationRequested)
            {
                _logger.LogDebug("Request was cancelled by the client. TraceId: {TraceId}",
                    context.TraceIdentifier);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;

                _logger.LogError(ex,"Unhandled exception occurred. TraceId: {TraceId}, Path: {Path}",traceId,
                    context.Request.Path);

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Cannot write error response because the response has already started. TraceId: {TraceId}",
                        traceId);

                    throw;
                }

                await HandleExceptionAsync(context, ex, traceId);
            }
        }

        private Task HandleExceptionAsync(HttpContext context,Exception ex,string traceId)
        {
            var statusCode = ex is AppException appException
                ? appException.StatusCode
                : StatusCodes.Status500InternalServerError;

            var errorMessage = ex is AppException
                ? ex.Message
                : _environment.IsDevelopment()
                    ? ex.Message
                    : "An unexpected error occurred.";

            var response = new ErrorToReturn
            {
                StatusCode = statusCode,
                ErrorMessage = errorMessage,
                TraceId = traceId
            };

            context.Response.Clear();
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
