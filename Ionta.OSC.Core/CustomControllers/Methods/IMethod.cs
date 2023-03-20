using System.Reflection;

namespace Ionta.OSC.Core.CustomControllers.Methods
{
    internal interface IMethod
    {
        string Name { get; }
        Task<object[]> GetParametrs(RequestInfo request, MethodInfo handler);
    }
}