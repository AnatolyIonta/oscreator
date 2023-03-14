using MediatR;

namespace Ionta.OSC.App.CQRS.Commands
{
    public class DeleteModulCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }
}