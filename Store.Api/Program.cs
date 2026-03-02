
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;
using Persistence.Data;
using Services;
using ServicesAbstractions;
using Shared.ErrorModels;
using Store.Api.Extenstions;
using Store.Api.Middlewares;
using static System.Runtime.InteropServices.JavaScript.JSType;



// BCZ There are two AssemblyReference classes in presistence and Services
using AssemblyMapping = Services.AssemblyReference;

namespace Store.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Before Build configure services
            // Add services to the container.

            builder.Services.RegisterAllServices(builder.Configuration);

            var app = builder.Build();
            
            // After Build configure Middlewares
            // Configure the HTTP request pipeline.

            await app.CofigureMiddlewares();

            app.Run();
        }
    }
}
