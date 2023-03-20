﻿using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.Core.CustomControllers.ControllerHandler;
using Ionta.OSC.ToolKit.Controllers;

using Microsoft.Extensions.Caching.Memory;

namespace Ionta.OSC.Core.CustomControllers.ControllerLoaderService
{
    public class ControllerLoaderService : IControllerLoaderService
    {
        private readonly IMemoryCache _cache;
        private readonly IAssemblyManager _manager;
        private readonly IControllerHandler _controllerHandler;

        public ControllerLoaderService(IMemoryCache cache, IAssemblyManager manager, IControllerHandler controllerHandler)
        {
            _cache = cache;
            _manager = manager;
            _controllerHandler = controllerHandler;
        }

        public async Task<ExecuteInfo?> FindController(RequestInfo request)
        {
            var _info = GetControllers();
            foreach (var controller in _info)
            {
                if (request.Path.ToLower().StartsWith("/" + controller.Path.ToLower()))
                {
                    return await _controllerHandler.ExecuteController(request, controller);
                }
            }
            return null;
        }

        public async Task<ControllerInfo> GetControllerInfoFromPath(string path)
        {
            var _info = GetControllers();
            foreach (var controller in _info)
            {
                if (path.ToLower().StartsWith("/" + controller.Path.ToLower()))
                {
                    return controller;
                }
            }
            return null;
        }

        private List<ControllerInfo> GetControllers()
        {
            var controllers = _manager.GetControllers(_manager.GetAssemblies())?.ToList() ?? new List<ControllerInfo>();
            return controllers;
        }
    }
}
