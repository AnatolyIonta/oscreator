using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.Utilities
{
    public static class EasyReflection
    {
        public static object? ExecuteGenericMethod(Type sourceType, object source, string methodName, Type genericType, params object[] param)
        {
            MethodInfo genericFoo = sourceType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            genericFoo = genericFoo.MakeGenericMethod(genericType);
            dynamic expression = genericFoo.Invoke(source, param);
            return expression;
        }

        public static IEnumerable<Type> GetTypesWithInterface(Assembly[] assemblies, Type interface_)
        {
            return assemblies.SelectMany(a => a.GetTypes()).Where(a => interface_.IsAssignableFrom(a));
        }
    }
}
