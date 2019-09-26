using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SystemPlus.Collections.Generic;
using SystemPlus.Web;
using SystemPlus.Web.Email;
using SystemPlus.Web.Logging;
using SystemPlus.Web.ReWriters;
using WebsiteTemplate.Core;
using WebsiteTemplate.Core.Emailing;
using WebsiteTemplate.Data;
using WebsiteTemplate.Models;
using WebsiteTemplate.Services;

namespace WebsiteTemplate
{
    public class Startup
    {
        #region Fields

        Database database;

        #endregion

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            EmailTools.Replacements.AddRange(UrlMaker.GetTagReplacements());

            database = new Database(Configuration["ConnectionStrings:DefaultConnection"]);

            IEmailSender emailSender = new Office365Emailer(Configuration["Email:Address"], Configuration["Email:Password"], "Admin");
            Emailer emailer = new Emailer(emailSender, "");
            
            // add objects to initialise constructors with
            services.AddSingleton(typeof(Database), database);
            services.AddSingleton(typeof(Emailer), emailer);
            services.AddSingleton(typeof(IEmailSender), emailSender);
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
            });

            // Require https sitewide
            services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });
            
            services.AddIdentity<User, Role>(o =>
            {
                o.User.RequireUniqueEmail = true;

                o.Lockout.MaxFailedAccessAttempts = 5;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                o.Lockout.AllowedForNewUsers = false;

                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 6;
                o.Password.RequiredUniqueChars = 1;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
            })
            .AddUserStore<MyUserStore>()
            .AddRoleStore<MyRoleStore>()
            .AddDefaultTokenProviders();

            services.AddAuthentication()
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            })
            .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.ClientId = Configuration["Authentication:Facebook:ClientId"];
                facebookOptions.ClientSecret = Configuration["Authentication:Facebook:ClientSecret"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanAdmin", policy => policy.RequireClaim("IsAdmin"));
            });

            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddRazorPages();

            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            app.UseStatusCodePagesWithReExecute("/Home/Status/{0}");
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection().UseWwwRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    Microsoft.AspNetCore.Http.Headers.ResponseHeaders headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue()
                    {
                        MaxAge = TimeSpan.FromDays(3),
                    };
                }
            });

            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // map routes with area and no controller
                endpoints.MapControllerRoute(name: "AreaRouteNoController",
                    pattern: "{area:exists}/{action}",
                    defaults: new { controller = "Home", action = "Index" });

                // map routes with area
                endpoints.MapControllerRoute(name: "AreaRoute",
                    pattern: "{area:exists}/{controller}/{action}",
                    defaults: new { controller = "Home", action = "Index" });

                // map routes with no controller
                endpoints.MapControllerRoute(name: "RouteNoController",
                   pattern: "{action}",
                    new { controller = "Home", action = "Index" });

                // default
                endpoints.MapControllerRoute(name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    new { controller = "Home", action = "Index" });

                endpoints.MapRazorPages();
            });
        }
    }
}
