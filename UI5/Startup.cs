using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData.Routing.Conventions;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using UI5.Controllers;
using UI5.Data;
using UI5.Helper;
using static UI5.Models.OData;

namespace UI5
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

            var tokenValidationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenProviderOptions:Key").Value)),
                ValidateIssuer = true,
                ValidIssuer = Configuration.GetSection("TokenProviderOptions:Issuer").Value,
                ValidateAudience = true,
                ValidAudience = Configuration.GetSection("TokenProviderOptions:Issuer").Value,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;
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
            services.AddODataQueryFilter();

            //services.AddDirectoryBrowser();
            services.AddCors(options =>
            {
                options.AddPolicy("UI5Policy",
                builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin()
                           .AllowCredentials()
                           .WithExposedHeaders("OData-Version");
                });
            });
            services.AddMvc( 
                options => options.EnableEndpointRouting = false
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices(); 
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

            app.UseHsts();
            app.UseHttpsRedirection();

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

            app.UseCors("UI5Policy");

            app.Use((context, next) =>
            {
                context.Response.Headers["OData-Version"] = "4.0";
                return next.Invoke();
            });

            app.UseODataBatching();

            app.UseMvc(routes =>
            {
                routes.Select().Expand().Filter().OrderBy().MaxTop(null).Count();
                routes.MapODataServiceRoute("odata", "odata", ODataBuilder.GetModel(), new DefaultODataBatchHandler());
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                routes.EnableDependencyInjection();
            });

            Helper.Methods.CreateRolesAsync(serviceProvider).Wait();
        }


    }
}
