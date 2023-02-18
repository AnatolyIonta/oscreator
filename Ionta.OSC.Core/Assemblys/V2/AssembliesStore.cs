using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.Loader;

namespace Ionta.OSC.Core.Assemblys.V2
{
    public class AssembliesStore : IAssemblyStore
    {
        private readonly ObservableCollection<AssemblyLoadContext> assebliesContext = new ObservableCollection<AssemblyLoadContext>();
        public Dictionary<int,string> Context { get; private set; }
        private readonly IMemoryCache _cache;
        public AssembliesStore(IMemoryCache cache) 
        { 
            _cache = cache;
            assebliesContext.CollectionChanged += OnChange;
            Context = new Dictionary<int, string>();
        }

        private void OnChange(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _cache.Dispose();
        }

        public void Load(IEnumerable<byte[]> assemblies, long id)
        {
            var context = new AssemblyLoadContext(name: Guid.NewGuid().ToString(), isCollectible: true);
            foreach (var assembly in assemblies)
            {
                using (var assemblyStream = new MemoryStream(assembly))
                {
                    // загружаем сборку
                    context.LoadFromStream(assemblyStream);
                }
            }
            assebliesContext.Add(context);
            Context.Add(id, context.Name);
        }

        public void Unload(long id)
        {
            string contextName = Context[id];
            var context = assebliesContext.FirstOrDefault(e => e.Name == contextName);
            if (context == null) return;
            context.Unload();
            assebliesContext.Remove(context);
            Context.Remove(id);
        }

        private IEnumerable<U>? Get<T,U>(Assembly assembly) 
            where T : class
            where U : class
        {
            var isGetValue = _cache.TryGetValue(typeof(T), out var result);
            if (isGetValue) return result as IEnumerable<U>;

            var Handlers = GetType()
                .Assembly
                .GetTypes()
                .Where(e => e.GetInterface(nameof(IGetTypeHandler)) != null);

            var instances = Handlers.Select(e => (IGetTypeHandler)Activator.CreateInstance(e));

            var handler = instances.FirstOrDefault(e => e.Type == typeof(T));
            if(handler == null) return null;

            var instance = (IEnumerable<U>)handler.Handle(assembly);

            _cache.Set(typeof(T), instance);

            return instance;
        }

        public IEnumerable<T>? Get<T>() where T : class
        {
            foreach(var context in assebliesContext)
            {
                foreach(var assembly in context.Assemblies)
                {
                    var result = Get<T,T>(assembly);
                    if(result != null) return result;
                }
            }
            return null;
        }

        public IEnumerable<U>? GetWithType<T,U>() 
            where T : class
            where U : class
        {
            foreach (var context in assebliesContext)
            {
                foreach (var assembly in context.Assemblies)
                {
                    var result = Get<T, U>(assembly);
                    if (result != null) return result;
                }
            }
            return null;
        }
    }
}
