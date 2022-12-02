using AssemblyLoader.Loader;
using Ionta.OSC.ToolKit.Services;
using Ionta.OSC.ToolKit.Store;
using Ionta.StoreLoader;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenServiceCreator.Infrastructure;

namespace OpenServiceCreator
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
            services.AddControllersWithViews();
            services.AddSingleton<IAssemblyManager, AssemblyManager>();
            services.AddScoped(servicesProvider =>
            {
                var options = new DbContextOptionsBuilder<DataStore>()
                    .UseNpgsql(GetDatabaseConnectionString(Configuration));
                
                var assemblyLoader = servicesProvider.GetService<IAssemblyManager>();
                return (IDataStore)(new DataStore(options.Options, assemblyLoader));
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
        }
        
        private static string GetDatabaseConnectionString(IConfiguration configuration)
        {
            return
                $"Host={configuration["DB_HOST"]};Port={configuration["DB_PORT"]};Database={configuration["DB_NAME"]};Username={configuration["DB_USER"]};Password={configuration["DB_PASSWORD"]}";
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAssemblyManager assemblyManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseMiddleware<CustomControllerMiddleware>(assemblyManager);

          
            /*
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
            */
            
        }
    }
}