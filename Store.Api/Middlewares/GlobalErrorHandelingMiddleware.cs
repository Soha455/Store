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

                // 1.Set Status Code
                // 2.Set Content Type
                // 3.Set body (Response Object)
                // 4.Return Response 

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new ErrorDetails()
                { 
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);

            }
        }

    }
}

//  Logging Exception means save it in DB for technical support or display on console