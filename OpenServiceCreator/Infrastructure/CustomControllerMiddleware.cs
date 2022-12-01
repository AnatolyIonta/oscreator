using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AssemblyLoader.Loader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpenServiceCreator.Infrastructure
{
    public class CustomControllerMiddleware
    {
        private ControllerInfo[] _info;
        private readonly AssemblyLoader.Loader.AssemblyManager _manager;
        private readonly RequestDelegate _next;
        
        public CustomControllerMiddleware(RequestDelegate next, AssemblyLoader.Loader.AssemblyManager infoManager)
        {
            _manager = infoManager;
            _next = next;
            infoManager.InitAssembly(Assembly.GetAssembly(GetType()));
            _info = infoManager.GetCommand().ToArray();
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            if (_manager.IsChange) _info = _manager.GetCommand().ToArray();
            
            foreach (var controller in _info)
            {
                if(context.Request.Path.Value == null) await SendResult(context,"ok");
                if (context.Request.Path.Value.StartsWith("/"+controller.Path))
                {
                    foreach (var handler in controller.Handlers)
                    {
                        if (context.Request.Path.Value == $"/{controller.Path}/{handler.Path}")
                        {
                            var constructorInfo = controller.Type.GetConstructor(new Type[] { });
                            var instance = constructorInfo.Invoke(new object[] { });

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