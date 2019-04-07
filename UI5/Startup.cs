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

            services.AddMvc(options => 
                options.EnableEndpointRouting = false
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

  
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

     
 
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ui5")),
                RequestPath = "/ui5"
            });

            app.UseMvc(routes =>
            {
                routes.Select().Expand().Filter().OrderBy().Count();
                routes.MapODataServiceRoute("odata", "odata", Models.OData.GetEdmModel(), new DefaultODataBatchHandler());

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }


    }
}
