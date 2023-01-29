using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.CustomControllers.Methods
{
    internal interface IMethod
    {
        string Name { get; }
        Task<object[]> GetParametrs(RequestInfo request, MethodInfo handler);
    }
}
