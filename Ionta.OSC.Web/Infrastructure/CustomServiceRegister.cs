using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ionta.OSC.ToolKit.Services;
using Ionta.OSC.ToolKit.ServiceProvider;
using System.Reflection;

namespace OpenServiceCreator.Infrastructure
{
    public class CustomServiceRegister : ICustomServiceRegister
    {
        private readonly IServiceCollection _services;
        private readonly IAssemblyManager _assemblyManager;
        public CustomServiceRegister(IServiceCollection services, IAssemblyManager assemblyManager)
        {
            _services = services;
            _assemblyManager = assemblyManager;
            assemblyManager.OnChange += RegisterNewService;
        }

        private void RegisterNewService(Assembly[] assemblies)
        {
            foreach(Assembly assembly in assemblies)
            {
                var services = assembly.GetTypes()
                    .Where(a => a.GetCustomAttribute(typeof(ServiceAttribute)) != null);

                foreach(var service in services)
                {
                    var attributeInfo = (ServiceAttribute)service.GetCustomAttribute(typeof(ServiceAttribute));

                    switch (attributeInfo.Type)
                    {
                        case ServiceType.Scoped:
                            _services.AddScoped(service, attributeInfo.Inteface);
                            break;
                        case ServiceType.Singelton:
                            _services.AddSingleton(service, attributeInfo.Inteface);
                            break;
                        /*
                        case ServiceType.Transiten:
                            _services.AddTransient(service, attributeInfo.Inteface);
                            break;
                        */
                    }
                }
            }
        }
    }
}
