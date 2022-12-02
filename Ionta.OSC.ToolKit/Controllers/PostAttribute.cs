using System;

namespace Ionta.OSC.ToolKit.Controllers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PostAttribute : Attribute
    {
        public string Path { get;}
        public PostAttribute(string path) => Path = path;
    }
}