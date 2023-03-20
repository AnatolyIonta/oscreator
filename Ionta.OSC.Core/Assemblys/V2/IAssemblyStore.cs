using System.Reflection;

namespace Ionta.OSC.Core.Assemblys.V2
{
    public interface IAssemblyStore
    {
        void Load(IEnumerable<byte[]> assemblies, long id);

        void Unload(long id);

        IEnumerable<T>? Get<T>() where T : class;
        IEnumerable<T>? Get<T>(Assembly[] assemblies) where T : class;

        IEnumerable<U>? GetWithType<T, U>() where T : class where U : class;
        IEnumerable<U>? GetWithType<T, U>(Assembly[] assemblies) where T : class where U : class;

        public IEnumerable<Assembly> GetAllAssembly();

        event Action<Assembly[]> OnLoad;
        event Action<Assembly[]> OnUnloading;
    }
}
