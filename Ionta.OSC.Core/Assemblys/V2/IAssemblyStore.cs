using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.Assemblys.V2
{
    public interface IAssemblyStore
    {
        void Load(IEnumerable<byte[]> assemblies, long id);

        void Unload(long id);

        IEnumerable<T>? Get<T>() where T : class;

        IEnumerable<U>? GetWithType<T, U>() where T : class where U : class;

        public IEnumerable<Assembly> GetAllAssembly();
    }
}
