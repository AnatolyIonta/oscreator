using Ionta.OSC.Core.CustomControllers.ControllerLoaderService;
using Ionta.OSC.ToolKit.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Ionta.OSC.Web.Infrastructure
{
    public class CustomAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IControllerLoaderService _controllerLoader;
        private readonly IAuthenticationService _authentication;
        public CustomAuthenticationMiddleware(RequestDelegate next, IControllerLoaderService controllerLoader, IAuthenticationService authentication) { 
            _next = next;
            _controllerLoader = controllerLoader;
            _authentication = authentication;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var controller = await _controllerLoader.GetControllerInfoFromPath(context.Request.Path);
            if (controller != null && controller.Authorize)
            {
                var accessToken = context.Request.Headers[HeaderNames.Authorization];
                if (!_authentication.ValidateToken(accessToken.ToString().Replace("Bearer ", "")))
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
            await _next.Invoke(context);
        }
    }
}
