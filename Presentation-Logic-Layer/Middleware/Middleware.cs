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
            }
        }
    }
}
