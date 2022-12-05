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
using IServiceProvider = Ionta.ServiceTools.IServiceProvider;

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
            infoManager.OnChange += LoadControllers;
            _manager = infoManager;
            _next = next;
            _services = services;
            infoManager.InitAssembly(Assembly.GetAssembly(GetType()));
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

                            await SendResult(context,handler.Handler.Invoke(instance,new []{parameter}));
                            return;
                        }
                    }
                }
            }
            
            await _next.Invoke(context);
        }

        private async Task SendResult(HttpContext context, object result)
        {
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