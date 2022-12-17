using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ionta.OSC.ToolKit.ServiceProvider;
using Ionta.OSC.ToolKit.Services;

namespace Ionta.ServiceTools
{
    class ServiceInfo
    {
        public ServiceType Type { get; set; }
        public Type Service { get; set; }
        public object Generator { get; set; }
    }
    public class ServiceProvider : Ionta.OSC.ToolKit.ServiceProvider.IServiceProvider
    {
        private Dictionary<Type, ServiceInfo> ServiceCollection = new();
        private Dictionary<Type, object> SingletoneService = new();

        public ServiceProvider(IAssemblyManager assemblyManager)
        {
            RegisterNewService(assemblyManager.GetAssemblies());
            assemblyManager.OnChange += RegisterNewService;
            assemblyManager.OnUnloading += OnUnloading;
        }

        private void OnUnloading(Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                var services = assembly.GetTypes()
                    .Where(a => a.GetCustomAttribute(typeof(ServiceAttribute)) != null);

                foreach (var service in services)
                {
                    ServiceCollection.Remove(service);
                }
            }
        }

        public void AddScoped<I, T>()
        {
            AddService(ServiceType.Scoped, typeof(T), typeof(I));
        }

        public void AddScoped<T>()
        {
            AddService(ServiceType.Scoped, typeof(T));
        }

        public void AddScoped<T>(Func<T> func)
        {
            AddService<T>(ServiceType.Scoped, func);
        }

        public void AddSingeltone<I, T>()
        {
            AddService(ServiceType.Singelton, typeof(T), typeof(I));
        }
        public void AddSingeltone<T>(Func<T> func)
        {
            AddService<T>(ServiceType.Singelton, func);
        }

        public void AddSingeltone<T>()
        {
            AddService(ServiceType.Singelton, typeof(T));
        }

        public object? GetService(string serviceName)
        {
            return GetService(ServiceCollection.Keys.First(s => s.Name == serviceName));
        }

        public object? GetService(Type serviceType)
        {
            ServiceInfo info;
            if (ServiceCollection.TryGetValue(serviceType, out info))
            {
                switch (info.Type)
                {
                    case ServiceType.Singelton:
                        if (SingletoneService.TryGetValue(serviceType, out object service))
                        {
                            return service;
                        }
                        else
                        {
                            var serviceInstace = info.Generator == null 
                                ? CreateServiceInstace(info.Service)
                                : ((Func<object>)info.Generator).Invoke();
                            SingletoneService.Add(serviceType, serviceInstace);

                            return serviceInstace;
                        }
                    case ServiceType.Scoped:
                        return info.Generator == null
                                ? CreateServiceInstace(info.Service)
                                : ((Func<object>)info.Generator).Invoke();
                    default:
                        return null;
                }
            }
            else return null;
        }

        private void AddService(ServiceType type, Type service, Type _interface = null)
        {
            var serviceInfo = new ServiceInfo() { Type = type, Service = service };
            ServiceCollection.Add(_interface ?? service, serviceInfo);
        }

        private void AddService<T>(ServiceType type, object generator)
        {
            var serviceInfo = new ServiceInfo() { Type = type, Generator = generator};
            ServiceCollection.Add(typeof(T), serviceInfo);
        }

        private object CreateServiceInstace(Type service)
        {
            var serviceConstructor = service.GetConstructors().First();
            var services = serviceConstructor.GetParameters()
                .Select(p => GetService(p.ParameterType))
                .ToArray();
            var serviceInstace = serviceConstructor.Invoke(services);

            return serviceInstace;
        }

        private void RegisterNewService(Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                var services = assembly.GetTypes()
                    .Where(a => a.GetCustomAttribute(typeof(ServiceAttribute)) != null);

                foreach (var service in services)
                {
                    var attributeInfo = (ServiceAttribute)service.GetCustomAttribute(typeof(ServiceAttribute));

                    var serviceInfo = new ServiceInfo() { Type = attributeInfo.Type, Service = service };
                    ServiceCollection.Add(attributeInfo.Inteface ?? service, serviceInfo);
                }
            }
        }
    }
}
