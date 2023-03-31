using Ionta.OSC.Core.CustomControllers.Methods;
using Ionta.OSC.Core.Exeption;
using Ionta.OSC.Core.ServiceTools;
using Ionta.OSC.ToolKit.Controllers;

using System.Reflection;

namespace Ionta.OSC.Core.CustomControllers.ControllerHandler
{
    public class ControllerHandler : IControllerHandler
    {
        private IServiceManager _services { get; init; }
        public ControllerHandler(IServiceManager services) { _services = services; }
        public async Task<ExecuteInfo?> ExecuteController(RequestInfo request, ControllerInfo controller)
        {
            foreach (var handler in controller.Handlers)
            {
                if (request.Path.ToLower() == $"/{controller.Path.ToLower()}/{handler.Path.ToLower()}")
                {
                    object[] parametrs = await ExecuteMethod(request, handler.Handler);

                    if (request.Method.ToLower() != handler.Method.ToString().ToLower()) throw new MethodsNotMatch();

                    var executeInfo = new ExecuteInfo()
                    {
                        Handler = handler.Handler,
                        Controller = controller,
                        Services = GetServices(controller),
                        Parameter = parametrs
                    };

                    return executeInfo;
                }
            }
            return null;
        }

        private async Task<object[]> ExecuteMethod(RequestInfo request, MethodInfo methodInfo)
        {
            var methods = this.GetType().Assembly.GetTypes().Where(e => e.GetInterface(nameof(IMethod)) != null);
            foreach (var method in methods)
            {
                var constructor = method.GetConstructor(Type.EmptyTypes);
                var obj = (IMethod)constructor.Invoke(null);
                if(obj.Name.ToLower() == request.Method.ToLower())
                {
                   return await obj.GetParametrs(request, methodInfo);
                }
            }
            throw new MethodNotFound();
        }

        private object[] GetServices(ControllerInfo controller)
        {
            var constructorInfo = controller.Type.GetConstructors().First();

            var parametrs = constructorInfo.GetParameters().ToArray();
            var services = parametrs
                .Select(p => _services.GetService(p.ParameterType, controller.Type.Assembly))
                .ToArray();

            return services;
        }
    }
}
