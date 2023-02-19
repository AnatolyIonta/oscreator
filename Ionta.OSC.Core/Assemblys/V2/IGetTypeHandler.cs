using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.Assemblys.V2
{
    public interface IGetTypeHandler<T>
    {
        public Type Type { get; }
        public IEnumerable<T> Handle(Assembly assembly);
    }
}
