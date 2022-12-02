using System;

namespace Ionta.OSC.ToolKit.Controllers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : Attribute
    {
        public string Prefix { get;}
        public ControllerAttribute(string path) => Prefix = path;
    }
}