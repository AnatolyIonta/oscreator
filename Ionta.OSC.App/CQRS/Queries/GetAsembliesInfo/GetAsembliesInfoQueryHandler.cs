using Ionta.OSC.Core.AssembliesInformation;
using Ionta.OSC.Core.AssembliesInformation.Dtos;
using Ionta.OSC.Core.Assemblys;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Queries
{
    public class GetAsembliesInfoQueryHandler : IRequestHandler<GetAsembliesInfoQuery, AssemblyInfoDto>
    {
        private readonly IAssembliesInfo _assembliesInfo;
        private readonly IAssemblyManager _assemblyManager;
        public GetAsembliesInfoQueryHandler(IAssembliesInfo assembliesInfo, IAssemblyManager assemblyManager) 
        { 
            _assembliesInfo = assembliesInfo;
            _assemblyManager = assemblyManager;
        }
        public async Task<AssemblyInfoDto> Handle(GetAsembliesInfoQuery request, CancellationToken cancellationToken)
        {
            return _assembliesInfo.GetInfo(_assemblyManager.GetAssemblies());
        }
    }
}