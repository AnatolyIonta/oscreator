using AssemblyLoader.Loader;
using Ionta.OSC.ToolKit.ServiceProvider;
using Ionta.OSC.ToolKit.Services;
using Ionta.OSC.ToolKit.Store;
using Ionta.StoreLoader;
using Ionta.StoreLoader.Migration;
using Ionta.ServiceTools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenServiceCreator.Infrastructure;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ionta.OSC.ToolKit.Auth;
using Ionta.OSC.Storage;
using Ionta.OSC.App.Services.HashingPasswordService;
using Ionta.OSC.App.Services.Auth;
using Ionta.OSC.App;
using Microsoft.AspNetCore.Http;
using MediatR;

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
            services.AddOptions();
            services.AddSingleton<IAssemblyManager, AssemblyManager>();
            services.AddSingleton<IServiceProvider, Ionta.ServiceTools.ServiceProvider>();
            services.AddSingleton<IMigrationGenerator, MigrationGenerator>();
            services.AddScoped<IHashingPasswordService, HashingPasswordService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddMediatR(typeof(Startup));
            services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(IOscStorage).Assembly, typeof(IHttpContextAccessor).Assembly, typeof(AuthOptions).Assembly);


            services.AddScoped(servicesProvider =>
            {
                var options = new DbContextOptionsBuilder<DataStore>()
                    .UseNpgsql(GetDatabaseConnectionString(Configuration));
                
                var assemblyLoader = servicesProvider.GetService<IAssemblyManager>();
                return (IDataStore)(new DataStore(options.Options, assemblyLoader));
            });

            services.AddScoped(servicesProvider =>
            {
                var options = new DbContextOptionsBuilder<OscStorage>()
                    .UseNpgsql(GetOscDatabaseConnectionString(Configuration));

                var hashingPassword = servicesProvider.GetService<IHashingPasswordService>();
                return (IOscStorage)(new OscStorage(options.Options, hashingPassword));
            });

            var authOptionsCofiguration = Configuration.GetSection("Auth").Get<AuthOptions>();
            services.AddSingleton(authOptionsCofiguration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                {
                    var authOptions = authOptionsCofiguration;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issure,
                        ValidateAudience = false,
                        ValidateActor = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                }
                );

        }

        private static string GetDatabaseConnectionString(IConfiguration configuration)
        {
            return
                $"Host={configuration["DB_GLOBAL_HOST"]};Port={configuration["DB_GLOBAL_PORT"]};Database={configuration["DB_GLOBAL_NAME"]};Username={configuration["DB_GLOBAL_USER"]};Password={configuration["DB_GLOBAL_PASSWORD"]}";
        }

        private static string GetOscDatabaseConnectionString(IConfiguration configuration)
        {
            return
                $"Host={configuration["DB_HOST"]};Port={configuration["DB_PORT"]};Database={configuration["DB_NAME"]};Username={configuration["DB_USER"]};Password={configuration["DB_PASSWORD"]}";
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAssemblyManager assemblyManager, IServiceProvider serviceProvider)
        {
            serviceProvider.AddScoped(()=>
            {
                var options = new DbContextOptionsBuilder<DataStore>()
                    .UseNpgsql(GetDatabaseConnectionString(Configuration));

                return (IDataStore)(new DataStore(options.Options, assemblyManager));
            });

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

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseMiddleware<CustomControllerMiddleware>(assemblyManager);


            assemblyManager.InitAssembly(Assembly.GetAssembly(GetType()));

        }
    }
}