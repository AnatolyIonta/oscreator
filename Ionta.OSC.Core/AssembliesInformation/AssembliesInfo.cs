using Ionta.OSC.Core.AssembliesInformation.Dtos;
using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.Core.CustomControllers.ControllerLoaderService;
using Ionta.OSC.ToolKit.Controllers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.AssembliesInformation
{
    public class AssembliesInfo : IAssembliesInfo
    {
        private readonly IAssemblyManager _manager;
        private readonly IMemoryCache _cache;
        public AssembliesInfo(IAssemblyManager assemblyManager, IMemoryCache cache)
        {
            _manager = assemblyManager;
            _cache = cache;
        }

        public AssemblyInfoDto GetInfo(Assembly[] assemblies) 
        {
            var result = new AssemblyInfoDto();
            result.Controllers = GetControllerDtos(assemblies);
            result.Tables = GetTablesDtos(assemblies).ToArray();
            return result;
        }

        private ControllerDto[] GetControllerDtos(Assembly[] assemblies)
        {
            var result = new List<ControllerDto>();
            var controllers = GetControllers();

            foreach(var controller in controllers)
            {
                result.Add(CreateControllerDto(controller));
            }

            return result.ToArray();
        }

        private IEnumerable<TableDto> GetTablesDtos(Assembly[] assemblies)
        {
            var entities = _manager.GetEntities(assemblies) ?? new List<Type>();
            foreach(var entity in entities)
            {
                yield return GetTableInfo(entity);
            }
        }

        private TableDto GetTableInfo(Type type)
        {
            var table = new TableDto();
            table.Name = type.Name;
            table.Attributes = new();
            var parameters = type.GetProperties();
            foreach (var param in parameters)
            {
                table.Attributes.Add(param.Name, param.PropertyType.Name);
            }
            return table;
        }

        private ControllerDto CreateControllerDto(ControllerInfo info)
        {
            var result = new ControllerDto()
            {
                IsAuth = info.Authorize,
                Methods = new List<MethodDto>()
            };

            foreach (var method in info.Handlers)
            {
                result.Methods.Add(CreateMethodDto(method, info.Path));
            }

            return result;
        }

        private MethodDto CreateMethodDto(HandlerInfo method, string path)
        {
            var methodDto = new MethodDto();
            methodDto.Path = path + "/" + method.Path;
            methodDto.Method = method.Method == Method.Get ? "get" : "post";
            methodDto.Parameters = GetParametrs(method.Handler);

            return methodDto;
        }

        private Dictionary<string, object> GetParametrs(MethodInfo info)
        {
            var result = new Dictionary<string, object>();
            var parameters = info.GetParameters();
            foreach(var param in parameters)
            {
                object paramType = null;
                if (param.ParameterType.IsClass)
                {
                    paramType =  param.ParameterType.GetProperties().ToDictionary(e => e.Name, e => e.PropertyType.Name);
                }
                else
                {
                    paramType = param.ParameterType.Name;
                }
                result.Add(param.Name, paramType);
            }
            return result;
        }

        private List<ControllerInfo> GetControllers()
        {
            var controllers = _manager.GetControllers(_manager.GetAssemblies())?.ToList() ?? new List<ControllerInfo>();
            return controllers;
        }
    }
}
