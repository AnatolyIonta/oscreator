using Ionta.OSC.ToolKit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.CustomControllers.ControllerHandler
{
    public interface IControllerHandler
    {
        Task<ExecuteInfo?> ExecuteController(RequestInfo request, ControllerInfo controller);
    }
}
