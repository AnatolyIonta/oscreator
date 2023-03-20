using Ionta.OSC.ToolKit.Controllers;

namespace Ionta.OSC.Core.CustomControllers.ControllerLoaderService
{
    public interface IControllerLoaderService
    {
        Task<ExecuteInfo?> FindController(RequestInfo request);
        Task<ControllerInfo> GetControllerInfoFromPath(string path);
    }
}
