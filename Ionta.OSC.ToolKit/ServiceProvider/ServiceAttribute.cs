using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.ToolKit.ServiceProvider
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public Type Inteface { get; }
        public ServiceType Type;
        public ServiceAttribute(Type _interface, ServiceType type) 
        {
            Inteface = _interface;
            Type = type;
        }
        public ServiceAttribute(ServiceType type)
        {
            Type = type;
        }
    }

    public enum ServiceType
    {
        Scoped,
        Singelton,
        //Transiten
    }
}
