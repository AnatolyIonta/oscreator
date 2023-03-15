using Ionta.OSC.App.Services.AssemblyInitializer;
using Ionta.OSC.Core.AssembliesInformation;
using Ionta.OSC.Core.Assemblys.V2;
using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.Core.Store.Migration;
using Ionta.OSC.Core.Store;
using Ionta.OSC.App.Services.Auth;
using Ionta.OSC.App;
using Ionta.OSC.Storage;
using Ionta.OSC.ToolKit.Store;
using Microsoft.EntityFrameworkCore;

namespace Ionta.OSC.Web.Extension
{
    public static class StoreExtension
    {
        public static IServiceCollection AddStore(this IServiceCollection services, string conectStringOSC, string conectStringGlobal, IConfiguration configuration)
        {
            services.AddSingleton<IMigrationGenerator, MigrationGenerator>();
            services.AddScoped(servicesProvider =>
            {
                var options = new DbContextOptionsBuilder<DataStore>()
                    .UseNpgsql(conectStringGlobal);

                var assemblyLoader = servicesProvider.GetService<IAssemblyManager>();
                return (IDataStore)(new DataStore(options.Options, assemblyLoader, configuration));
            });

            var conectionString = conectStringOSC;
            services.AddDbContextPool<IOscStorage, OscStorage>(options =>
            {
                options.UseNpgsql(conectionString);
            }, 16);

            return services;
        }
    }
}
