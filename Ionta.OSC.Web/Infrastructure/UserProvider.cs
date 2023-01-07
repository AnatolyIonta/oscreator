using Ionta.OSC.App.Services;
using Microsoft.AspNetCore.SignalR;

namespace Ionta.OSC.Web.Infrastructure
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long? GetCurrentId()
        {
            var claim = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "Sub");
            if (claim == null)
            {
                return null;
            }
            return long.Parse(claim.Value);
        }
    }

}
