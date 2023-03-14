using MediatR;

namespace Ionta.OSC.App.CQRS.Commands
{
    public class LoadAssemblyCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}