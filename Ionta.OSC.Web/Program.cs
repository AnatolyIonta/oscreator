using AssemblyLoader.Loader;
using Ionta.OSC.App;
using Ionta.OSC.App.Services.AssemblyInitializer;
using Ionta.OSC.App.Services.Auth;
using Ionta.OSC.App.Services.HashingPasswordService;
using Ionta.OSC.ToolKit.Auth;
using Ionta.OSC.ToolKit.Services;
using Ionta.ServiceTools.V2;
using Ionta.StoreLoader.Migration;
using MediatR;
using System.Reflection;
using Ionta.OSC.Web.Infrastructure;
using Ionta.OSC.ToolKit.Store;
using Ionta.StoreLoader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ionta.OSC.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.

builder.Services.AddControllersWithViews();
services.AddOptions();
services.AddSingleton<IAssemblyManager, AssemblyManager>();
services.AddSingleton<IServiceManager, ServiceManager>();
services.AddSingleton<IMigrationGenerator, MigrationGenerator>();
services.AddScoped<IAssemblyInitializer, AssemblyInitializer>();
services.AddScoped<IHashingPasswordService, HashingPasswordService>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped(servicesProvider =>
{
    var options = new DbContextOptionsBuilder<DataStore>()
        .UseNpgsql(GetDatabaseConnectionString(servicesProvider.GetService<IConfiguration>()));

    var assemblyLoader = servicesProvider.GetService<IAssemblyManager>();
    return (IDataStore)(new DataStore(options.Options, assemblyLoader));
});

services.AddScoped(servicesProvider =>
{
    var options = new DbContextOptionsBuilder<OscStorage>()
        .UseNpgsql(GetOscDatabaseConnectionString(servicesProvider.GetService<IConfiguration>()));

    var hashingPassword = servicesProvider.GetService<IHashingPasswordService>();
    return (IOscStorage)(new OscStorage(options.Options, hashingPassword));
});

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

var assemblyManager = app.Services.GetService<IAssemblyManager>();
var serviceManager = app.Services.GetService<IServiceManager>();
var Configuration = app.Services.GetService<IConfiguration>();

app.UseMiddleware<CustomControllerMiddleware>(assemblyManager);


serviceManager.GlobalCollection.AddScoped((serviceProvider) =>
{
    var options = new DbContextOptionsBuilder<DataStore>()
        .UseNpgsql(GetDatabaseConnectionString(Configuration));

    return (IDataStore)(new DataStore(options.Options, assemblyManager));
});

serviceManager.GlobalCollection.AddSingleton(serviceProvider => (IServiceProvider)serviceManager);

serviceManager.ConfigurePrivateContainer = (collection) =>
{
    collection.AddScoped((serviceProvider) =>
    {
        var options = new DbContextOptionsBuilder<DataStore>()
            .UseNpgsql(GetDatabaseConnectionString(Configuration));

        return (IDataStore)(new DataStore(options.Options, assemblyManager));
    });
    collection.AddSingleton(serviceProvider => (IServiceProvider)serviceManager);
};

serviceManager.GlobalServiceBuild();

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