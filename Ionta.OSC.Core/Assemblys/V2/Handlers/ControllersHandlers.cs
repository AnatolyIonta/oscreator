using Ionta.OSC.ToolKit.Auth;
using Ionta.OSC.ToolKit.Controllers;
using System.Reflection;

namespace Ionta.OSC.Core.Assemblys.V2.Handlers
{
    public class ControllersHandlers : IGetTypeHandler<ControllerInfo>
    {
        public Type Type => typeof(ControllerInfo);

        public IEnumerable<ControllerInfo> Handle(Assembly assembly)
        {
                var controllers = assembly.GetTypes()
                    .Where(a => a.GetCustomAttribute(typeof(ControllerAttribute)) != null);

                foreach (var controller in controllers)
                {
                    var methods = controller
                        .GetMethods()
                        .Where(m => m.GetCustomAttribute(typeof(PostAttribute)) != null || m.GetCustomAttribute(typeof(GetAttribute)) != null);
                    yield return new ControllerInfo()
                    {
                        Handlers = methods.Select(m => {
                            var attribute = m.GetCustomAttribute(typeof(PostAttribute)) ?? m.GetCustomAttribute(typeof(GetAttribute));
                            return new HandlerInfo()
                            {
                                Handler = m,
                                Path = ((IMethodAttribute)attribute).Path,
                                Method = ((IMethodAttribute)attribute).Method
                            };
                        }
                        ),
                        Path = ((ControllerAttribute)controller.GetCustomAttribute(typeof(ControllerAttribute)))?.Prefix,
                        Type = controller,
                        Authorize = controller.GetCustomAttribute(typeof(AuthorizeAttribute)) != null
                    };

                }
        }
    }
}
