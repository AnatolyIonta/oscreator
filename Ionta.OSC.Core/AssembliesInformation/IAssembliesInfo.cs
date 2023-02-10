using Ionta.OSC.Core.AssembliesInformation.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.AssembliesInformation
{
    public interface IAssembliesInfo
    {
        AssemblyInfoDto GetInfo(Assembly[] assemblies);
    }
}
