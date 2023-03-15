using MediatR;

namespace Ionta.OSC.App.CQRS.Commands
{
    public class ActivateAssemblyCommand : IRequest<bool>
    {
        public int AssemblyId { get; set; } 
        public bool IsActive { get; set; }
    }
}