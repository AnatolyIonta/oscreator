using System;
using System.Collections.Generic;

namespace Ionta.OSC.ToolKit.Controllers
{
    public class ControllerInfo
    {
        public string Path { get; set; }
        public Type Type { get; set; }
        public IEnumerable<HandlerInfo> Handlers { get; set; }
    }
}