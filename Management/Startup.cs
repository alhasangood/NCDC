using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Models;
using Management.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.MailKitSmtp;
using Management.Settings;

namespace Management
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddDbContext<NCDCContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("NCDC"), o => o.CommandTimeout(180));
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddAuthentication(o =>
            {
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
        .AddCookie(options =>
        {
            options.AccessDeniedPath = new PathString("/");
            options.LoginPath = new PathString("/");
            options.Cookie.Name = "NCDC";

        });
            services.AddSession(o => { });

            services.AddControllers(config =>
            {
                config.Filters.Add(new AuthorizeFilter());
                config.Filters.Add(typeof(ValidationFilter));
            })
                    .AddFluentValidation(c =>
                    {
                        c.ValidatorOptions.CascadeMode = CascadeMode.Stop;
                        c.RegisterValidatorsFromAssemblyContaining<Startup>();
                    });

         
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Portal", Version = "v1" });
            });

            var mailSettings = Configuration.GetSection("Email").Get<SmtpClientOptions>();
            services.AddFluentEmail(mailSettings.User, Configuration["Settings:Nccp"].ToString()).AddMailKitSender(mailSettings);

            services.Configure<SecuritySettings>(Configuration.GetSection("Security"));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var logger = new LoggerConfiguration()
                       .ReadFrom.Configuration(Configuration.GetSection("Logging"))
                       .Enrich.FromLogContext();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portal v1"));
            }

            loggerFactory.AddSerilog();

            Serilog.Log.Logger = logger.CreateLogger();

            app.UseHttpsRedirection();

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}



