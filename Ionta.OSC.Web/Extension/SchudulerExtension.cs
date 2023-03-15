using Ionta.OSC.App.Scheduler;
using Ionta.OSC.App.Services.Scheduler;

namespace Ionta.OSC.Web.Extension
{
    public static class SchudulerExtension
    {
        public static IServiceCollection AddSchuduler(this IServiceCollection services)
        {
            services.AddSingleton<IScheduler, Scheduler>();
            services.AddHostedService<Worker>();

            return services;
        }
    }
}
