using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using UI5.Controllers;
using UI5.Data;
using static UI5.Models.OData;

namespace UI5
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(opt => opt.UseInMemoryDatabase("Test"));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MyDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });


            services.AddOData();
            services.AddDirectoryBrowser();

            services.AddMvc(options => 
                options.EnableEndpointRouting = false
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

  
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseDirectoryBrowser(new DirectoryBrowserOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ui5")),
                    RequestPath = "/ui5"
                });

            }
            //DefaultFilesOptions options = new DefaultFilesOptions();
            //options.DefaultFileNames.Clear();
            //options.DefaultFileNames.Add("Index.html");
            //app.UseDefaultFiles(options);


            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseAuthentication();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".properties"] = "application/text";

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ui5")),
                RequestPath = "/ui5",
                ContentTypeProvider = provider
            });

            app.UseODataBatching();

            app.Use((context, next) =>
            {
                context.Response.Headers["OData-Version"] = "4.0";
                return next.Invoke();
            });

            app.UseMvc(routes =>
            {
                routes.Select().Expand().Filter().OrderBy().MaxTop(null).Count();
                routes.MapODataServiceRoute("odata", "odata", Models.OData.GetEdmModel(), new DefaultODataBatchHandler());
                routes.MapRoute( name: "default",  template: "{controller=Home}/{action=Index}/{id?}");
            });

            Helper.Methods.CreateRolesAsync(serviceProvider).Wait();

        }


    }
}
