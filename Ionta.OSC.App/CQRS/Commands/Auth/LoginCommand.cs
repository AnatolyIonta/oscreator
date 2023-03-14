using Ionta.OSC.App.Dtos;
using MediatR;

namespace Ionta.OSC.App.CQRS.Commands
{
    public class LoginCommand : IRequest<JWTDto>
    {
        public string Login { get; set; }
        public string Password { get; set; }

    }
}
