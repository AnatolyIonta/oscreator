using Ionta.OSC.ToolKit.ServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ionta.OSC.Core.ServiceTools
{
    public interface IServiceManager
    {
        public IServiceCollection GlobalCollection { get; }
        public Action<IServiceCollection> ConfigurePrivateContainer { get; set; }
        public object? GetService(string serviceName, Assembly assembly);
        public object? GetService<T>(Assembly assembly);
        public object? GetService(Type serviceType, Assembly assembly);
        public void RegisterGlobalService<I, S>(ServiceType type, bool isBuild);
        public void RegisterGlobalService(Type service, Type _interface, ServiceType type, bool isBuild);
        public void GlobalServiceBuild();
        public Microsoft.Extensions.DependencyInjection.ServiceProvider GetGlobalServiceProvider();
    }
}
