using System;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.Profiles;
using AchieveIt.BusinessLogic.Services;
using AchieveIt.DataAccess;
using AchieveIt.DataAccess.UnitOfWork;
using Kirpichyov.FriendlyJwt.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace AchieveIt.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddAutoMapper(
                typeof(AchieveIt.BusinessLogic.Profiles.UserProfile), 
                typeof(AchieveIt.API.Profiles.UserProfile)
            );
            services.AddControllersWithViews();

            services.AddControllers()
                // FriendlyJwt authorization services registration below
                .AddFriendlyJwtAuthentication(configuration =>
                {
                    configuration.Audience = "https://localhost:5001";
                    configuration.Issuer = "https://localhost:5001";
                    configuration.Secret = "SecretYGPV8XC6bPJhQCUBV2LtDSharp";
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "AchieveIt.API", Version = "v1"});
            });
            
            services.AddFriendlyJwt();
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));
            
            services.AddDbContext<DatabaseContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(Configuration.GetConnectionString("Default"), serverVersion)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AchieveIt.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}