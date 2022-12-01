using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AssemblyLoader.Loader
{
    public class AssemblyManager
    {
        private List<Assembly> assemblies = new List<Assembly>();

        public bool IsChange { get; private set; }

        public void InitAssembly(params Assembly[] assemblies)
        {
            this.assemblies.AddRange(assemblies);
            IsChange = true;
        }

        public IEnumerable<ControllerInfo> GetCommand()
        {
            foreach (var assemble in assemblies)
            {
                var controllers = assemble.GetTypes()
                    .Where(a => a.GetCustomAttribute(typeof(Controller)) != null);
                
                foreach (var controller in controllers)
                {
                    var methods = controller
                        .GetMethods()
                        .Where(m => m.GetCustomAttribute(typeof(Command)) != null);
                    yield return new ControllerInfo()
                    {
                        Handlers = methods.Select(m => new HandlerInfo()
                        {
                            Handler = m,
                            Path = ((Command)m.GetCustomAttribute(typeof(Command)))?.Path
                        }),
                        Path = ((Controller)controller.GetCustomAttribute(typeof(Controller)))?.Prefix,
                        Type = controller
                    };
                    
                }
            }

            IsChange = false;
        }
    }

    public class ControllerInfo
    {
        public string Path { get; set; }
        public Type Type { get; set; }
        public IEnumerable<HandlerInfo> Handlers { get; set; }
    }

    public class HandlerInfo
    {
        public string Path { get; set; }
        public MethodInfo Handler { get; set; }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class Command : Attribute
    {
        public string Path { get;}
        public Command(string path) => Path = path;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Controller : Attribute
    {
        public string Prefix { get;}
        public Controller(string path) => Prefix = path;
    }

    public class Parameter
    {
        public string key;
        public object value;
    }
}