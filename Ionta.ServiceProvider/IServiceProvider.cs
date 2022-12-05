using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.ServiceTools
{
    public interface IServiceProvider
    {
        public object? GetService(Type serviceType);
        public void AddScoped<I, T>();
        public void AddScoped<T>();
        public void AddScoped<T>(Func<T> func);
        public void AddSingeltone<I,T>();
        public void AddSingeltone<T>();
    }
}
