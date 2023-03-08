using Ionta.OSC.App;
using Ionta.OSC.App.Services.AssemblyInitializer;
using Ionta.OSC.App.Services.Auth;
using Ionta.OSC.ToolKit.Auth;
using MediatR;
using System.Reflection;
using Ionta.OSC.Web.Infrastructure;
using Ionta.OSC.ToolKit.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ionta.OSC.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ionta.OSC.App.Services;

using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.Core.ServiceTools;
using Ionta.OSC.Core.Store;
using Ionta.OSC.Core.Store.Migration;
using Ionta.OSC.Core.Auth;
using Microsoft.Extensions.DependencyInjection;
using Ionta.OSC.Core.CustomControllers.ControllerHandler;
using Ionta.OSC.Core.CustomControllers.ControllerLoaderService;
using Ionta.OSC.Web.Extension;
using Ionta.OSC.Core.AssembliesInformation;
using Ionta.OSC.Core.Assemblys.V2;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

builder.Services.AddControllersWithViews();
services.AddOptions();
services.AddSingleton<IAssemblyManager, AssemblyManagerV2>();
services.AddSingleton<IAssemblyStore, AssembliesStore>();
services.AddSingleton<IServiceManager, ServiceManager>();
services.AddSingleton<IMigrationGenerator, MigrationGenerator>();
services.AddMemoryCache();
services.AddCustomControllers();
services.AddScoped<IAssemblyInitializer, AssemblyInitializer>();                                                                                    
services.AddScoped<IAssembliesInfo, AssembliesInfo>();                                                                                    
//services.AddTransient<IHashingPasswordService, HashingPasswordService>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped(servicesProvider =>
{
    var options = new DbContextOptionsBuilder<DataStore>()
        .UseNpgsql(GetDatabaseConnectionString(servicesProvider.GetService<IConfiguration>()));

    var assemblyLoader = servicesProvider.GetService<IAssemblyManager>();
    return (IDataStore)(new DataStore(options.Options, assemblyLoader, builder.Configuration));
});

var conectionString = GetOscDatabaseConnectionString(builder.Configuration);
services.AddDbContextPool<IOscStorage, OscStorage>(options =>
{
    options.UseNpgsql(conectionString);
}, 16);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(IOscStorage).Assembly, typeof(IHttpContextAccessor).Assembly,
                typeof(AuthOptions).Assembly, typeof(IMigrationGenerator).Assembly);

var authOptionsCofiguration = builder.Configuration.GetSection("Auth").Get<AuthOptions>();

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
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<IUserProvider, UserProvider>();
services.AddSingleton(serviceProvider => (IAuthenticationService)new AuthenticationService(builder.Configuration["Secret"]));

var app = builder.Build();

app.UseAuthentication();

app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

var assemblyManager = app.Services.GetService<IAssemblyManager>();
var serviceManager = app.Services.GetService<IServiceManager>();
var Configuration = app.Services.GetService<IConfiguration>();

app.UseMiddleware<CustomAuthenticationMiddleware>();
app.UseMiddleware<Ionta.OSC.Web.Infrastructure.V2.CustomControllerMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var store = scope.ServiceProvider.GetService<IOscStorage>();
    store.ApplyMigrations();
}

serviceManager.GlobalCollection.AddScoped((serviceProvider) =>
{
    var options = new DbContextOptionsBuilder<DataStore>()
        .UseNpgsql(GetDatabaseConnectionString(Configuration));

    return (IDataStore)(new DataStore(options.Options, assemblyManager, builder.Configuration));
});

serviceManager.GlobalCollection.AddSingleton(serviceProvider => (IServiceProvider)serviceManager);

serviceManager.ConfigurePrivateContainer = (collection) =>
{
    collection.AddScoped((serviceProvider) =>
    {
        var options = new DbContextOptionsBuilder<DataStore>()
            .UseNpgsql(GetDatabaseConnectionString(Configuration));

        return (IDataStore)(new DataStore(options.Options, assemblyManager, builder.Configuration));
    });
    collection.AddSingleton(serviceProvider => (IServiceProvider)serviceManager);
    collection.AddSingleton(serviceProvider => (IAuthenticationService)new AuthenticationService(Configuration["Secret"]));
    collection.AddHttpClient();
};

serviceManager.GlobalServiceBuild();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetService<IAssemblyInitializer>();
    initializer.Initialize();
}

app.Run();



static string GetDatabaseConnectionString(IConfiguration configuration)
{
    return
        $"Host={configuration["DB_GLOBAL_HOST"]};Port={configuration["DB_GLOBAL_PORT"]};Database={configuration["DB_GLOBAL_NAME"]};Username={configuration["DB_GLOBAL_USER"]};Password={configuration["DB_GLOBAL_PASSWORD"]}";
}

static string GetOscDatabaseConnectionString(IConfiguration configuration)
{
    return
        $"Host={configuration["DB_HOST"]};Port={configuration["DB_PORT"]};Database={configuration["DB_NAME"]};Username={configuration["DB_USER"]};Password={configuration["DB_PASSWORD"]}";
}