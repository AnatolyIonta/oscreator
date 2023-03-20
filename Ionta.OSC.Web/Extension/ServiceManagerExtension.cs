using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.Core.Auth;
using Ionta.OSC.Core.ServiceTools;
using Ionta.OSC.Core.Store;
using Ionta.OSC.ToolKit.Auth;
using Ionta.OSC.ToolKit.Store;

using Microsoft.EntityFrameworkCore;

namespace Ionta.OSC.Web.Extension
{
    public static class ServiceManagerExtension
    {
        public static IServiceManager Init(this IServiceManager serviceManager, IAssemblyManager assemblyManager, string conectStringGlobal, IConfiguration configuration)
        {
            serviceManager.GlobalCollection.AddScoped((serviceProvider) =>
            {
                var options = new DbContextOptionsBuilder<DataStore>()
                    .UseNpgsql(conectStringGlobal);

                return (IDataStore)(new DataStore(options.Options, assemblyManager, configuration));
            });

            serviceManager.GlobalCollection.AddSingleton(serviceProvider => (IServiceProvider)serviceManager);

            serviceManager.ConfigurePrivateContainer = (collection) =>
            {
                collection.AddScoped((serviceProvider) =>
                {
                    var options = new DbContextOptionsBuilder<DataStore>()
                        .UseNpgsql(conectStringGlobal);

                    return (IDataStore)(new DataStore(options.Options, assemblyManager, configuration));
                });
                collection.AddSingleton(serviceProvider => (IServiceProvider)serviceManager);
                collection.AddSingleton(serviceProvider => (IAuthenticationService)new AuthenticationService(configuration["Secret"]));
                collection.AddHttpClient();
            };

            serviceManager.GlobalServiceBuild();

            return serviceManager;
        }
    }
}
