using Ionta.OSC.App.Dtos;
using Ionta.OSC.Core.AssembliesInformation.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Queries.GetAsembliesInfo
{
    public class GetAsembliesInfoQuery : IRequest<AssemblyInfoDto>
    {
    }
}
