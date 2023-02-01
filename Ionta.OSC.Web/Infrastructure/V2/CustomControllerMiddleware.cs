using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.Core.CustomControllers;
using Ionta.OSC.Core.CustomControllers.ControllerLoaderService;
using Ionta.OSC.Core.Exeption;
using Ionta.OSC.Core.ServiceTools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ionta.OSC.Web.Infrastructure.V2
{
    public class CustomControllerMiddleware
    {
        private readonly IControllerLoaderService _controllerLoader;
        private readonly RequestDelegate _next;

        public CustomControllerMiddleware(RequestDelegate next, IControllerLoaderService controllerLoader)
        {
            _next = next;
            _controllerLoader = controllerLoader;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestInfo = new RequestInfo(context.Request.Path, context.Request.Method, context.Request.Body, context.Request.Query);
            var executeInfo = await _controllerLoader.FindController(requestInfo);
            if (executeInfo != null)
            {
                try { 
                    await Execute(context, executeInfo);
                    return;
                }
                catch (ManyParameters ex) { await SendError(context, ex.Message, 400); }
                catch (MethodNotFound ex) { await SendError(context, ex.Message, 405); }
                catch (MethodsNotMatch) { await SendError(context, "", 404); }
            }
            await _next.Invoke(context);
        }

        private async Task Execute(HttpContext context, Ionta.OSC.Core.CustomControllers.ExecuteInfo info)
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

        private static bool IsAsyncMethod(MethodInfo method)
        {
            Type attType = typeof(AsyncStateMachineAttribute);

            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);

            return (attrib != null);
        }

        private async Task SendError(HttpContext context, string message, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(message);
        }

        private async Task SendResult(HttpContext context, object result)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(new JsonResult(result) { StatusCode = 200 });
        }
    }
}
