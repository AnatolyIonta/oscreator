using Ionta.OSC.Core.CustomControllers.ControllerHandler;
using Ionta.OSC.Core.CustomControllers.ControllerLoaderService;

namespace Ionta.OSC.Web.Extension
{
    public static class CustomControllerExtension
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddSingleton<IControllerHandler, ControllerHandler>();
            services.AddSingleton<IControllerLoaderService, ControllerLoaderService>();
            return services;
        }
    }
}
