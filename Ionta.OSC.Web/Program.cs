using Ionta.OSC.App;
using Ionta.OSC.App.Services.AssemblyInitializer;
using Ionta.OSC.Web.Infrastructure;
using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.Core.ServiceTools;
using Ionta.OSC.Web.Extension;

using MediatR;

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
services.AddMemoryCache();
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

services.AddSingleton<IServiceManager, ServiceManager>();

services.AddAssembliesSystem();
services.AddCustomControllers();
services.AddStore(
    GetOscDatabaseConnectionString(builder.Configuration), 
    GetDatabaseConnectionString(builder.Configuration), 
    builder.Configuration);
services.AddAuthenticationJWT(builder.Configuration);
services.AddSchuduler();

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

services.AddLogging();


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

var assemblyManager = app.Services.GetRequiredService<IAssemblyManager>();
var serviceManager = app.Services.GetRequiredService<IServiceManager>();
var Configuration = app.Services.GetRequiredService<IConfiguration>();

app.UseMiddleware<CustomAuthenticationMiddleware>();
app.UseMiddleware<Ionta.OSC.Web.Infrastructure.V2.CustomControllerMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var store = scope.ServiceProvider.GetService<IOscStorage>();
    store.ApplyMigrations();
}

serviceManager.Init(
    assemblyManager,
    GetDatabaseConnectionString(builder.Configuration),
    builder.Configuration);

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