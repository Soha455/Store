using Services;
using Persistence;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;
using Domain.Contracts;
using Store.Api.Middlewares;

namespace Store.Api.Extenstions
{
    public static class Extenstions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddBuiltInServices();

            services.AddSwaggerServices();

            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices();

            services.AddConfigureServices();

            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
           
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        private static IServiceCollection AddConfigureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                                              .Select(m => new ValidtionError()
                                              {
                                                  Field = m.Key,
                                                  Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                                              });

                    var response = new ValidationeErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });


            return services;
        }



        public static async Task<WebApplication> CofigureMiddlewares(this WebApplication app)
        {
            await app.InitializeDatabaseAsync();

            app.UseGlobalErrorHandeling();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();         // For Rendering Static Files images,Videos,CSS,JavaScript,HTML 

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var dbInitalizer = Scope.ServiceProvider.GetRequiredService<IDbInitializer>();  // Ask CLR to create object from IDbInitializer not from its constructor
            await dbInitalizer.InitializeAsync();

            return app;
        }

        private static WebApplication UseGlobalErrorHandeling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandelingMiddleware>();         // Configuring the Errors Middleware 

            return app;
        }


    }
}
