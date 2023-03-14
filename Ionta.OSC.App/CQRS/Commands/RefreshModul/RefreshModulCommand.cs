using MediatR;

namespace Ionta.OSC.App.CQRS.Commands
{
    public class RefreshModulCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}