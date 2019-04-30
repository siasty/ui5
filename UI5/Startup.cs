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
            services.AddOData();

            services.AddDirectoryBrowser();

            services.AddMvc(options => 
                options.EnableEndpointRouting = false
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

  
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("Index.html");
            app.UseDefaultFiles(options);


            app.UseStaticFiles();
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
                context.Response.Headers["OData-MaxVersion"] = "4.0";
                return next.Invoke();
            });

            app.UseMvc(routes =>
            {

                routes.Select().Expand().Filter().OrderBy().MaxTop(null).Count();
                routes.MapODataServiceRoute("odata", "odata", GetEdmModel(), new DefaultODataBatchHandler());
            });

        }


    }
}
