using Shared.ErrorModels;

namespace Store.Api.Middlewares
{
    public class GlobalErrorHandelingMiddleware
    { 
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandelingMiddleware> _logger;

        public GlobalErrorHandelingMiddleware(RequestDelegate next , ILogger<GlobalErrorHandelingMiddleware> logger)
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
            catch(Exception ex) 
            {
                // Log Exception
                _logger.LogError(ex,ex.Message);

                // 1.Set Response Status Code
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // 2.Set Response Content Type
                context.Response.ContentType = "application/json";

                // 3.Set Response Object (body)
                var response = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };

                // 4.Return Response 
                await context.Response.WriteAsJsonAsync(response);

            }
        }

    }
}

//  Logging Exception means save it in DB for technical support or display on console