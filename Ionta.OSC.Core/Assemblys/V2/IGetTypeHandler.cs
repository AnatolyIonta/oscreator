using System.Reflection;

namespace Ionta.OSC.Core.Assemblys.V2
{
    public interface IGetTypeHandler<T>
    {
        public Type Type { get; }
        public IEnumerable<T> Handle(Assembly assembly);
    }
}
