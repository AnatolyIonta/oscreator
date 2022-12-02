using System.Reflection;

namespace Ionta.OSC.ToolKit.Controllers
{
    public class HandlerInfo
    {
        public string Path { get; set; }
        public MethodInfo Handler { get; set; }
    }
}