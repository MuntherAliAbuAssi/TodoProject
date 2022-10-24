using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOProject.Core.Managers;
using TODOProject.Core.Managers.interfaces;
using TODOProject.Core.Mapper;
using TODOProject.DbModel.Models;
using TODOProject.Extenstions;

namespace ToDoProject
{
    public class Startup
    {
        private MapperConfiguration _mapperConfigration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _mapperConfigration = new MapperConfiguration(a =>
            {
                a.AddProfile(new Mapping());
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<ICommonManager, CommonManager>();
            services.AddTransient<IToDoManager, ToDoManager>();

            services.AddDbContext<ToDoContext>();

            services.AddSingleton(ms => _mapperConfigration.CreateMapper());

            services.AddLogging();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoProject", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Please insert Bearer JWT token, Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
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
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters 
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true, 
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = Configuration["Jwt:Issuer"],
                                ValidAudience = Configuration["Jwt:Issuer"],
                                ClockSkew = TimeSpan.Zero,
                                IssuerSigningKey = new SymmetricSecurityKey(
                                                       Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                            };
                        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoProject v1"));
            }

            Log.Logger = new LoggerConfiguration()
                         .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Minute)
                         .CreateLogger();

            app.ConfigureExceptionHandler(Log.Logger, env);

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
