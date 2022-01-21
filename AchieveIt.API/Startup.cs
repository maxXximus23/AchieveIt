using System;
using AchieveIt.API.Extensions;
using AchieveIt.API.Validators;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.Services;
using AchieveIt.DataAccess;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using AchieveIt.Shared.Options;
using FluentValidation;
using FluentValidation.AspNetCore;
using Kirpichyov.FriendlyJwt.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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

            services.AddOptions<JwtOptions>()
                .Bind(Configuration.GetSection(JwtOptions.JwtSectionName));
            
            services.AddOptions<RefreshTokenOptions>()
                .Bind(Configuration.GetSection(RefreshTokenOptions.RefreshTokenSectionName));
            
            services.AddControllers()
                .AddFriendlyJwtAuthentication(configuration =>
                {
                    var jwtOptions = new JwtOptions();
                    Configuration.Bind(JwtOptions.JwtSectionName, jwtOptions);

                    configuration.Audience = jwtOptions.Audience;
                    configuration.Issuer = jwtOptions.Issuer;
                    configuration.Secret = jwtOptions.Secret;
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<StudentValidator>());
            ValidatorOptions.Global.LanguageManager.Enabled = false;

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

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}