using Ionta.OSC.Core.AssembliesInformation.Dtos;
using System.Reflection;

namespace Ionta.OSC.Core.AssembliesInformation
{
    public interface IAssembliesInfo
    {
        AssemblyInfoDto GetInfo(Assembly[] assemblies);
    }
}