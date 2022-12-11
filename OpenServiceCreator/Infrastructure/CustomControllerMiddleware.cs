using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AssemblyLoader.Loader;
using Ionta.ServiceTools;
using Ionta.OSC.ToolKit.Controllers;
using Ionta.OSC.ToolKit.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IServiceProvider = Ionta.OSC.ToolKit.ServiceProvider.IServiceProvider;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

namespace OpenServiceCreator.Infrastructure
{
    public class CustomControllerMiddleware
    {
        private IEnumerable<ControllerInfo> _info = new List<ControllerInfo>();
        private readonly IAssemblyManager _manager;
        private readonly IServiceProvider _services;
        private readonly RequestDelegate _next;
        
        public CustomControllerMiddleware(RequestDelegate next, IAssemblyManager infoManager, IServiceProvider services)
        {
            _manager = infoManager;
            LoadControllers(infoManager.GetAssemblies());
            infoManager.OnChange += LoadControllers;
            _next = next;
            _services = services;
        }
        public void LoadControllers(Assembly[] assemblies)
        {
            _info = _info.Union(_manager.GetControllers(assemblies));
        }
        public async Task InvokeAsync(HttpContext context)
        {
            foreach (var controller in _info)
            {
                if(context.Request.Path.Value == null) await SendResult(context,"ok");
                if (context.Request.Path.Value.StartsWith("/"+controller.Path))
                {
                    foreach (var handler in controller.Handlers)
                    {
                        if (context.Request.Path.Value == $"/{controller.Path}/{handler.Path}")
                        {
                            var constructorInfo = controller.Type.GetConstructors().First();

                            var services = constructorInfo.GetParameters()
                                .Select(p => _services.GetService(p.ParameterType))
                                .ToArray();

                            var instance = constructorInfo.Invoke(services);

                            var parameterType = handler.Handler.GetParameters()[0].ParameterType;

                            var json = await GetJson(context.Request.Body);

                            var parameter = JsonSerializer.Deserialize(json, parameterType);

                            var method = handler.Handler;

                            dynamic methodResult = handler.Handler.Invoke(instance, new[] { parameter });

                            
                            if (IsAsyncMethod(method))
                            {
                                var res = await methodResult;
                                await SendResult(context, res);
                            }
                            else
                            {
                                await SendResult(context, methodResult);
                            }

                            //SendResult(context, methodResult);
                            return;
                        }
                    }
                }
            }
            
            await _next.Invoke(context);
        }

        private static bool IsAsyncMethod(MethodInfo method)
        { 
            Type attType = typeof(AsyncStateMachineAttribute);

            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);

            return (attrib != null);
        }

        private async Task SendResult(HttpContext context, object result)
        {
            if(result is Task)
            {

            }
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(new JsonResult(result));
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
}