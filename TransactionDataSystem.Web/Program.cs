
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TransactionDataSystem.Infrastructure.Data;
using TransactionDataSystem.Infrastructure.UnitOfWork;
using TransactionDataSystem.Services.Interfaces;
using TransactionDataSystem.Services.Mapping;
using TransactionDataSystem.Services.Services;
using TransactionDataSystem.Web.Extensions;

namespace TransactionDataSystem.Web
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Add PostgreSQL database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add repositories and unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IReportingService, ReportingService>();

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TransactionDataSystem API", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TransactionDataSystem API v1"));
            }

            // Add global exception handling middleware
            app.UseGlobalExceptionHandler();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Apply migrations
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    dbContext.Database.Migrate();
                }
            }

            app.Run();

        }
    }

    public partial class Program { }
}
