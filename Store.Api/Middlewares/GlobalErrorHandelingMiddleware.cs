using Domain.Exceptions;
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
                await _next.Invoke(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)   // Empty body & statuscode=404 so notfound endpoint  request & response without exception
                {
                    await HandlingNotFoundEndPointAsync(context);
                }
            }
            catch(Exception ex)
            {
                // Log Exception
                _logger.LogError(ex, ex.Message);
                await HandlingErrorAsync(context, ex);

            }
        }

        private static async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {
            // 1.Set Status Code
            // 2.Set Content Type
            // 3.Set body (Response Object)
            // 4.Return Response 

            //context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/json";

            var response = new ErrorDetails()
            {
                ErrorMessage = ex.Message
            };


            response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandlingNotFoundEndPointAsync(HttpContext context)
        {
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"End Point {context.Request.Path} is not Found !!"
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

//  Logging Exception means save it in DB for technical support or display on console