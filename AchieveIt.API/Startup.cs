using System;
using AchieveIt.API.Extensions;
using AchieveIt.API.Validators;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.Services;
using AchieveIt.DataAccess;
using AchieveIt.DataAccess.UnitOfWork;
using AchieveIt.Shared.Options;
using Azure.Storage.Blobs;
using FluentValidation;
using FluentValidation.AspNetCore;
using Kirpichyov.FriendlyJwt.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IHomeworkService, HomeworkService>();
            services.AddScoped<IHomeworkAttachmentService, HomeworkAttachmentService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IForumService, ForumService>();
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

            services.AddOptions<BlobStorageOptions>()
                .Bind(Configuration.GetSection(BlobStorageOptions.SectionName));
            
            services.AddControllers()
                .AddFriendlyJwtAuthentication(configuration =>
                {
                    var jwtOptions = new JwtOptions();
                    Configuration.Bind(JwtOptions.JwtSectionName, jwtOptions);

                    configuration.Audience = jwtOptions.Audience;
                    configuration.Issuer = jwtOptions.Issuer;
                    configuration.Secret = jwtOptions.Secret;
                })
                
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<ExceptionMiddlewareExtensions>();
                })
                
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<StudentValidator>();
                });
            ValidatorOptions.Global.LanguageManager.Enabled = false;

            var blobOptions = new BlobStorageOptions();
            Configuration.Bind(BlobStorageOptions.SectionName, blobOptions);
            services.AddSingleton(x => new BlobServiceClient(blobOptions.ConnectionString));
            services.AddSingleton<IBlobService, BlobService>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "AchieveIt.API", Version = "v1"});
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
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

            app.UseSerilogRequestLogging();

            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}