using Ionta.OSC.ToolKit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.CustomControllers
{
    public class ExecuteInfo
    {
        public MethodInfo Handler { get; set; }
        public object[] Services { get; set; }
        public ControllerInfo Controller { get; set; }
        public object[] Parameter { get; set; }
    }
}
