using Ionta.OSC.ToolKit.Controllers;
using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.Core.ServiceTools;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text;
using MediatR;
using Microsoft.Net.Http.Headers;
using Ionta.OSC.ToolKit.Auth;

namespace Ionta.OSC.Web.Infrastructure
{
    public class CustomControllerMiddleware
    {
        //private List<ControllerInfo> _info
        private readonly IAssemblyManager _manager;
        private readonly IServiceManager _services;
        private readonly RequestDelegate _next;
        private readonly IAuthenticationService _authentication;

        public CustomControllerMiddleware(RequestDelegate next, IAssemblyManager infoManager, IServiceManager services, IAuthenticationService authentication)
        {
            _manager = infoManager;
            //LoadControllers(infoManager.GetAssemblies());
            //infoManager.OnChange += LoadControllers;
            //infoManager.OnUnloading += OnUnloading;
            _next = next;
            _services = services;
            _authentication = authentication;
        }

        private List<ControllerInfo> GetControllers()
        {
            return _manager.GetControllers(_manager.GetAssemblies()).ToList();
        }

       
        //private void OnUnloading(Assembly[] obj)
        //{
        //    var controllers = _manager.GetControllers(_manager.GetAssemblies());

        //    /*
        //    foreach(var controller in controllers)
        //    {
        //        var target = _info.FirstOrDefault(i => i.Type == controller.Type);
        //        if(target != null)
        //        {
        //            _info.Remove(target);
        //        }
        //    }
        //    */
        //}

        //public void LoadControllers(Assembly[] assemblies)
        //{
        //    _info = _info.Union(_manager.GetControllers(assemblies)).ToList();
        //}

        public async Task InvokeAsync(HttpContext context)
        {
            var _info = GetControllers();
            foreach (var controller in _info)
            {
                if (context.Request.Path.Value == null) await SendResult(context, "ok");
                if (context.Request.Path.Value.StartsWith("/" + controller.Path))
                {
                    if (controller.Authorize) {
                        var accessToken = context.Request.Headers[HeaderNames.Authorization];
                        if (!_authentication.ValidateToken(accessToken.ToString().Replace("Bearer ","")))
                        {
                            context.Response.StatusCode = 403;
                            return;
                        }
                    }
                    foreach (var handler in controller.Handlers)
                    {
                        if (context.Request.Path.Value == $"/{controller.Path}/{handler.Path}")
                        {
                            object[] parametrs = null;

                            if (context.Request.Method.ToLower() != handler.Method.ToString().ToLower()) throw new Exception("Методы не совпадают");

                            if (context.Request.Method.ToLower() == "post")
                            {
                                var parameters = await GetParametrJson(context, handler.Handler);
                                parametrs = parameters is null ? new object[] { } : new[] { parameters };
                            }
                            else if (context.Request.Method.ToLower() == "get")
                            {
                                parametrs = GetParametersUrl(context, handler.Handler).ToArray();
                            }

                            var executeInfo = new ExecuteInfo()
                            {
                                Handler = handler.Handler,
                                Controller = controller,
                                Services = GetServices(controller),
                                Parameter = parametrs
                            };
                            await Execute(context, executeInfo);

                            return;
                        }
                    }
                }
            }

            await _next.Invoke(context);
        }

        private async Task Execute(HttpContext context, ExecuteInfo info)
        {
            var constructorInfo = info.Controller.Type.GetConstructors().First();
            var instance = constructorInfo.Invoke(info.Services);
            var method = info.Handler;

            var parametrs = info.Parameter;
            dynamic methodResult = method.Invoke(instance, parametrs);

            if (IsAsyncMethod(method))
            {
                var res = await methodResult;
                await SendResult(context, res);
            }
            else
            {
                await SendResult(context, methodResult);
            }
        }

        private object[] GetServices(ControllerInfo controller)
        {
            var constructorInfo = controller.Type.GetConstructors().First();

            var services = constructorInfo.GetParameters()
                .Select(p => _services.GetService(p.ParameterType, controller.Type.Assembly))
                .ToArray();

            return services;
        }

        private async Task<object> GetParametrJson(HttpContext context, MethodInfo handler)
        {
            var parametrs = handler.GetParameters();

            if (parametrs.Length == 0) return null;
            if (parametrs.Length > 1) throw new Exception("Контроллер не может иметь больше 1 параметра!");

            var parameterType = handler.GetParameters()[0].ParameterType;

            var json = await GetJson(context.Request.Body);
            var parameter = JsonSerializer.Deserialize(json, parameterType);

            return parameter;
        }

        private IEnumerable<object> GetParametersUrl(HttpContext context, MethodInfo handler)
        {
            var parametrs = handler.GetParameters();

            if (parametrs.Length > 0)
            {
                var query = context.Request.Query;

                foreach (var parametr in parametrs)
                {
                    object result = query.First(q => q.Key.ToLower() == parametr.Name.ToLower()).Value.ToString();
                    if (parametr.ParameterType != typeof(string))
                    {
                        result = Convert.ChangeType(result, parametr.ParameterType);
                    }
                    yield return result;
                }
            }
        }

        private static bool IsAsyncMethod(MethodInfo method)
        {
            Type attType = typeof(AsyncStateMachineAttribute);

            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);

            return (attrib != null);
        }

        private async Task SendResult(HttpContext context, object result)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(new JsonResult(result) { StatusCode = 200 });
        }

        private async Task<string> GetJson(System.IO.Stream input)
        {
            using (StreamReader reader
                   = new StreamReader(input, Encoding.UTF8, true, 1024, true))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
    class ExecuteInfo
    {
        public MethodInfo Handler { get; set; }
        public object[] Services { get; set; }
        public ControllerInfo Controller { get; set; }
        public object[] Parameter { get; set; }
    }
}

