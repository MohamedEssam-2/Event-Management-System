using Business_Logic_Layer.DTO.ErrorDTO;
using Business_Logic_Layer.Exceptions;
using System.Text.Json;

namespace Presentation_Logic_Layer.Middleware
{
    public class Middleware(RequestDelegate _next,ILogger<Middleware> _logger)
    {
        public async Task InvokeAsync(HttpContext _httpContext)
        {
            try
            {
                await _next.Invoke(_httpContext);
            }
            catch(Exception ex )
            { 
                _logger.LogError(ex,ex.Message);
                await HandleExceptionAsync(_httpContext, ex);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var statusCode = ex is AppException appException
                ? appException.StatusCode
                : StatusCodes.Status500InternalServerError;

            context.Response.StatusCode = statusCode;

            var response = new ErrorToReturn
            {
                StatusCode = statusCode,
                ErrorMessage = ex.Message
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
