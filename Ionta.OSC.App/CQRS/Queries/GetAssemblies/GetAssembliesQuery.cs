using Ionta.OSC.App.Dtos;
using MediatR;

namespace Ionta.OSC.App.CQRS.Queries
{
    public class GetAssembliesQuery : IRequest<DtosList<AssemblyDto>>
    {

    }
}
