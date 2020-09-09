using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .ConfigureApplicationPartManager(apm => 
                {
                    var pluginsPath = Path.Combine(WebHostEnvironment.ContentRootPath, "Plugins");
                    var assemblyFiles = Directory.GetFiles(pluginsPath, "*WebPart.dll", SearchOption.AllDirectories);

                    foreach (var assemblyFile in assemblyFiles) 
                    {
                        var a = Assembly.LoadFrom(assemblyFile);
                        apm.ApplicationParts.Add(new AssemblyPart(a));
                        var relatedAssemblies = RelatedAssemblyAttribute.GetRelatedAssemblies(a, throwOnError: true);
                        foreach (ApplicationPart view in relatedAssemblies.SelectMany(s => CompiledRazorAssemblyApplicationPartFactory.GetDefaultApplicationParts(s)))
                        {
                            apm.ApplicationParts.Add(view);
                        }
                    }
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
