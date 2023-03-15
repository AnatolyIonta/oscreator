using Ionta.OSC.Core.AssembliesInformation.Dtos;
using MediatR;

namespace Ionta.OSC.App.CQRS.Queries
{
    public class GetAsembliesInfoQuery : IRequest<AssemblyInfoDto>
    {
    }
}