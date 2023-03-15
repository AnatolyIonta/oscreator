using Ionta.OSC.App.Services.AssemblyInitializer;
using Ionta.OSC.Core.AssembliesInformation;
using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.Core.Assemblys.V2;

namespace Ionta.OSC.Web.Extension
{
    public static class AssembliExtension
    {
        public static IServiceCollection AddAssembliesSystem(this IServiceCollection services)
        {
            services.AddSingleton<IAssemblyManager, AssemblyManagerV2>();
            services.AddSingleton<IAssemblyStore, AssembliesStore>();
            services.AddScoped<IAssemblyInitializer, AssemblyInitializer>();
            services.AddScoped<IAssembliesInfo, AssembliesInfo>();

            return services;
        }
    }
}
