using MediatR;

namespace Ionta.OSC.App.CQRS.Commands
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}
