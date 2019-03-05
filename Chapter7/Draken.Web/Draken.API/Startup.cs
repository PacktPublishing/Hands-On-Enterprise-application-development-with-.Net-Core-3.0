using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draken.API.Infrastructure;
using Draken.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Draken.API
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
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    assembly => assembly.MigrationsAssembly(typeof(Contact).Assembly.FullName));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Security:Jwt:Issuer"],
                        ValidAudience = Configuration["Security:Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Security:Jwt:Key"]))
                    };
                });

            services.AddMvc()
                .AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Draken.API",
                    Version = "v1",
                    Description = "Open API documentation of Draken.API",
                    Contact = new OpenApiContact()
                    {
                        Name = "Anuraj",
                        Url = new Uri("https://twitter.com/anuraj")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri("https://anuraj.mit-license.org/")
                    }
                });

                var bearerScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                };

                c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.AddSecurityDefinition("Bearer", bearerScheme);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseAuthentication();
            app.UseRouting(routes =>
            {
                routes.MapApplication();
            });

            app.UseAuthorization();
        }
    }
}
