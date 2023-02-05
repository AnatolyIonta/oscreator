using Ionta.OSC.App.CQRS.Queries.GetAssemblies;
using Ionta.OSC.App.Dtos;
using Ionta.OSC.Core.AssembliesInformation;
using Ionta.OSC.Core.AssembliesInformation.Dtos;
using Ionta.OSC.Core.Assemblys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Queries.GetAsembliesInfo
{
    public class GetAsembliesInfoQueryHandler : IRequestHandler<GetAsembliesInfoQuery, ControllerDto[]>
    {
        private readonly IAssembliesInfo _assembliesInfo;
        private readonly IAssemblyManager _assemblyManager;
        public GetAsembliesInfoQueryHandler(IAssembliesInfo assembliesInfo, IAssemblyManager assemblyManager) 
        { 
            _assembliesInfo = assembliesInfo;
            _assemblyManager = assemblyManager;
        }
        public async Task<ControllerDto[]> Handle(GetAsembliesInfoQuery request, CancellationToken cancellationToken)
        {
            return _assembliesInfo.GetControllerDtos(_assemblyManager.GetAssemblies());
        }
    }
}
