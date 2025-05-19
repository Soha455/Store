
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services;
using ServicesAbstractions;

// BCZ There are two AssemblyReference classes in presistence and Services
using AssemblyMapping = Services.AssemblyReference;

namespace Store.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDbInitializer,DbInitializer>();    // Allow DI for DbInitializer
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddAutoMapper(typeof(AssemblyMapping).Assembly);

            var app = builder.Build();

            #region Data Seeding

            using var Scope = app.Services.CreateScope();
            var dbInitalizer = Scope.ServiceProvider.GetRequiredService<IDbInitializer>();  // Ask CLR to create object from IDbInitializer not from its constructor
            await dbInitalizer.InitializeAsync();

            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();         // For Rendering Static Files images,Videos,CSS,JavaScript,HTML 

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
