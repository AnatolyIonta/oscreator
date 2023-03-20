using Ionta.OSC.App.Dtos;

using System.Threading.Tasks;

namespace Ionta.OSC.App.Services.Auth
{
    public interface IAuthService
    {
        public Task<JWTDto> Handle(string login, string password);
    }
}