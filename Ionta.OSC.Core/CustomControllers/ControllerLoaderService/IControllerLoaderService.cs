using Ionta.OSC.ToolKit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.CustomControllers.ControllerLoaderService
{
    public interface IControllerLoaderService
    {
        Task<ExecuteInfo?> FindController(RequestInfo request);
        Task<ControllerInfo> GetControllerInfoFromPath(string path);
    }
}
