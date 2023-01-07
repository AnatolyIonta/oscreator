using Ionta.OSC.ToolKit.ServiceProvider;
using Ionta.OSC.ToolKit.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IServiceProvider = Ionta.OSC.ToolKit.ServiceProvider.IServiceProvider;

namespace Ionta.ServiceTools.V2
{
    public class ServiceManager : IServiceManager, IServiceProvider
    {
        public Dictionary<Assembly, Microsoft.Extensions.DependencyInjection.ServiceProvider> PrivateContainers { get; set; }
        public IServiceCollection GlobalCollection { get; set; }
        public Microsoft.Extensions.DependencyInjection.ServiceProvider GlobalContainer { get; set; }
        public Action<IServiceCollection> ConfigurePrivateContainer { get; set; }

        private readonly IAssemblyManager _assemblyManager;

        public ServiceManager(IAssemblyManager assemblyManager) 
        {
            GlobalCollection = new ServiceCollection();
            PrivateContainers = new Dictionary<Assembly, Microsoft.Extensions.DependencyInjection.ServiceProvider> { };
            _assemblyManager = assemblyManager;
            assemblyManager.OnChange += OnChange;
            assemblyManager.OnUnloading += OnUnloading;
        }

        public object? GetService(string serviceName, Assembly assembly)
        {
            var serviceType = assembly.GetType(serviceName);
            return GetService(serviceType, assembly);

        }

        public object? GetService<T>(Assembly assembly)
        {
            return GetService(typeof(T), assembly);
        }

        public object? GetService(Type serviceType, Assembly assembly)
        {
            object result = GlobalContainer.GetService(serviceType);
            if (result != null) return result;

            if (PrivateContainers.TryGetValue(assembly, out var provider))
            {
                result = provider.GetService(serviceType);
            }

            return result;
        }

        public void RegisterGlobalService<I,S>(ServiceType type, bool isBuild)
        {
            RegisterGlobalService(typeof(S), typeof(I), type, isBuild);
        }

        public void RegisterGlobalService(Type service, Type _interface, ServiceType type, bool isBuild)
        {
            switch (type)
            {
                case ServiceType.Scoped:
                    GlobalCollection.AddScoped(_interface ?? service, service);
                    break;
                case ServiceType.Singelton:
                    GlobalCollection.AddSingleton(_interface ?? service, service);
                    break;
                case ServiceType.Transiten:
                    GlobalCollection.AddTransient(_interface ?? service, service);
                    break;
            }
            if(isBuild) GlobalContainer = GlobalCollection.BuildServiceProvider();
        }

        public void GlobalServiceBuild()
        {
            GlobalContainer = GlobalCollection.BuildServiceProvider();
        }

        public Microsoft.Extensions.DependencyInjection.ServiceProvider GetGlobalServiceProvider()
        {
            return GlobalContainer;
        }

        private void OnUnloading(Assembly[] assemblies)
        {
            foreach(Assembly assembly in assemblies)
            {
                var servicesGlobal = assembly.GetTypes()
                    .Where(a => a.GetCustomAttribute(typeof(ServiceAttribute)) != null && a.GetCustomAttribute(typeof(GlobalServiceAttribute)) != null);

                
                foreach (var service in servicesGlobal)
                {
                    var attribute = (ServiceAttribute)service.GetCustomAttribute(typeof(ServiceAttribute));
                    ServiceLifetime lifetime = ServiceLifetime.Transient;
                    switch (attribute.Type)
                    {
                        case ServiceType.Transiten:
                            lifetime = ServiceLifetime.Transient;
                            break;
                        case ServiceType.Singelton:
                            lifetime = ServiceLifetime.Singleton;
                            break;
                        case ServiceType.Scoped:
                            lifetime = ServiceLifetime.Scoped;
                            break;
                    }
                    
                    GlobalCollection.Remove(new ServiceDescriptor(service, attribute.Inteface ?? service, lifetime));
                }
                GlobalServiceBuild();

                PrivateContainers.Remove(assembly);
            }
        }

        private void OnChange(Assembly[] assemblies)
        {
            foreach(Assembly assembly in assemblies)
            {
                var servicesPrivate = assembly.GetTypes()
                    .Where(a => a.GetCustomAttribute(typeof(ServiceAttribute)) != null && a.GetCustomAttribute(typeof(GlobalServiceAttribute)) == null);

                var servicesGlobal = assembly.GetTypes()
                    .Where(a => a.GetCustomAttribute(typeof(ServiceAttribute)) != null && a.GetCustomAttribute(typeof(GlobalServiceAttribute)) != null);

                var serviceCollection = new ServiceCollection();

                foreach (var service in servicesGlobal)
                {
                    var attributeInfo = (ServiceAttribute)service.GetCustomAttribute(typeof(ServiceAttribute));

                    RegisterGlobalService(service, attributeInfo.Inteface, attributeInfo.Type, false);
                }

                GlobalServiceBuild();

                foreach (var service in servicesPrivate)
                {
                    var attributeInfo = (ServiceAttribute)service.GetCustomAttribute(typeof(ServiceAttribute));

                    ConfigurePrivateContainer(serviceCollection);

                    switch (attributeInfo.Type)
                    {
                        case ServiceType.Singelton:
                            serviceCollection.AddSingleton(attributeInfo.Inteface ?? service, service);
                            break;
                        case ServiceType.Scoped:
                            serviceCollection.AddScoped(attributeInfo.Inteface ?? service, service);
                            break;
                        case ServiceType.Transiten:
                            serviceCollection.AddTransient(attributeInfo.Inteface ?? service, service);
                            break;
                    }
                }
                PrivateContainers.Add(assembly, serviceCollection.BuildServiceProvider());
            }
        }

        public ServiceWrapper GetService(string serviceName)
        {
            var x = _assemblyManager.GetAssemblies()[0].GetTypes();
            var types = _assemblyManager.GetAssemblies().Select(e => e.GetTypes().FirstOrDefault(t => t.Name == serviceName));
            var serviceType = types.First(e => e != null);

            return GetService(serviceType);
        }

        public ServiceWrapper GetService(Type serviceType)
        {
            GlobalServiceBuild();
            var service = GlobalContainer.GetService(serviceType);
            if (service == null) return null;
            return new ServiceWrapper(service);
        }
    }
}
