using Ionta.OSC.Core.Assemblys.V2.Handlers;
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
        public Dictionary<long,string> Context { get; private set; }
        private readonly IMemoryCache _cache;
        private static readonly object _lock = new object();
        public event Action<Assembly[]> OnLoad;
        public event Action<Assembly[]> OnUnloading;
        public AssembliesStore(IMemoryCache cache) 
        {
            _cache = cache;
            assebliesContext.CollectionChanged += OnChange;
            Context = new Dictionary<long, string>();
        }

        private void OnChange(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ((MemoryCache)_cache).Clear();
        }



        public void Load(IEnumerable<byte[]> assemblies, long id)
        {
            var context = new AssemblyLoadContext(name: Guid.NewGuid().ToString(), isCollectible: true);
            context.Resolving += OnAssemblyResolve;
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
            OnLoad?.Invoke(context.Assemblies.ToArray());
        }

        public void Unload(long id)
        {
            string contextName = Context[id];
            var context = assebliesContext.FirstOrDefault(e => e.Name == contextName);
            if (context == null) return;
            OnUnloading?.Invoke(context.Assemblies.ToArray());
            context.Unload();
            assebliesContext.Remove(context);
            Context.Remove(id);
        }

        private IEnumerable<U>? Get<T,U>(Assembly assembly) 
            where T : class
            where U : class
        {
            var isGetValue = _cache.TryGetValue(typeof(T).Name+assembly.FullName, out var result);
            if (isGetValue) return result as IEnumerable<U>;

            var name = nameof(IGetTypeHandler<U>)+"`1";

            var Handlers = GetType()
                .Assembly
                .GetTypes()
                .Where(e => e.GetInterface(name) != null);
            var instances = Handlers.Where(e => e.GetInterface(name).GenericTypeArguments[0] == typeof(U)).Select(e => (IGetTypeHandler<U>)Activator.CreateInstance(e));

            var handler = instances.FirstOrDefault(e => e.Type == typeof(T));
            if(handler == null) return null;

            var instance = handler.Handle(assembly);
            
            _cache.Set(typeof(T), instance, DateTimeOffset.UtcNow.AddMinutes(3));

            return instance;
        }

        public IEnumerable<T> Get<T>() where T : class
        {
            IEnumerable<T> result = new List<T>();
            foreach(var context in assebliesContext)
            {
                foreach (var assembly in context.Assemblies)
                {
                    var data = Get<T,T>(assembly);
                    if(result != null) result = result.Union(data);
                }
            }
            return result;
        }

        public IEnumerable<U>? GetWithType<T,U>() 
            where T : class
            where U : class
        {
            IEnumerable<U> result = new List<U>();
            foreach (var context in assebliesContext)
            {
                foreach (var assembly in context.Assemblies)
                {
                    var data = Get<T, U>(assembly);
                    if (result != null) result = result.Union(data);
                }
            }
            return result;
        }

        public IEnumerable<Assembly> GetAllAssembly()
        {
            foreach (var context in assebliesContext)
            {
                foreach (var assembly in context.Assemblies)
                {
                    yield return assembly;
                }
            }
        }
        protected virtual Assembly OnAssemblyResolve(AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName)
        {
            lock (_lock)
            {
                var assemblies = assemblyLoadContext.Assemblies.ToList();
                var assembly = assemblyLoadContext.LoadFromAssemblyName(assemblyName);
                return assembly;
            }
        }
    }

}
