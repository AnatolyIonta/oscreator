using Ionta.OSC.ToolKit.Controllers;

namespace Ionta.OSC.Core.CustomControllers.ControllerHandler
{
    public interface IControllerHandler
    {
        Task<ExecuteInfo?> ExecuteController(RequestInfo request, ControllerInfo controller);
    }
}