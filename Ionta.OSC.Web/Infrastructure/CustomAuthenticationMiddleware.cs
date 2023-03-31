using Ionta.OSC.App.Services;
using Ionta.OSC.Core.CustomControllers.ControllerLoaderService;
using Ionta.OSC.ToolKit.Auth;

using Microsoft.Net.Http.Headers;

namespace Ionta.OSC.Web.Infrastructure
{
    public class CustomAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IControllerLoaderService _controllerLoader;
        private readonly IAuthenticationService _authentication;
        private readonly IServiceProvider _serviceProvider;
        public CustomAuthenticationMiddleware(RequestDelegate next, IControllerLoaderService controllerLoader, IAuthenticationService authentication, IServiceProvider serviceProvider) { 
            _next = next;
            _controllerLoader = controllerLoader;
            _authentication = authentication;
            _serviceProvider = serviceProvider;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var controller = await _controllerLoader.GetControllerInfoFromPath(context.Request.Path);

            if(controller == null) await _next.Invoke(context);

            using (var scoped = _serviceProvider.CreateScope())
            {
                var _userProvider = scoped.ServiceProvider.GetRequiredService<IUserProvider>();
                if (_userProvider.GetCurrentId() == null && controller.InternalAuthorize)
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
            
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
