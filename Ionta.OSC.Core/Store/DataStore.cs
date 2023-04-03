using System.Reflection;

using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.ToolKit.Store;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Ionta.OSC.Core.Store
{
    public class DataStore : DbContext, IDataStore
    {
        private readonly Dictionary<string, Type> _entities = new();
        private readonly IConfiguration _configuration;
        public readonly IAssemblyManager _assemblyManager;
        
        public DataStore(DbContextOptions<DataStore> options, IAssemblyManager assemblyManager, IConfiguration configuration)
            : base(options)
        { 
            _assemblyManager = assemblyManager;
            _assemblyManager.OnChange += InitEntity;
            _assemblyManager.OnUnloading += OnUnloading;
            _configuration = configuration;
            Database.EnsureCreated();
        }

        private void OnUnloading(Assembly[] obj)
        {
            var entities = _assemblyManager.GetEntities();
            
            _entities.Clear();
            foreach (var entity in entities)
            {
                _entities.Add(nameof(entity), entity);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RegisterAllEntities<BaseEntity>(_assemblyManager.GetAssemblies(), _configuration);
            return;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
        }


        private static MethodInfo setMethod = typeof(DataStore).GetMethods()
            .Single(m => m.Name == nameof(DataStore.GetEntity) && m.GetParameters().Length == 0);
        public void InitEntity(Assembly[] assemblies)
        {
            var entities = _assemblyManager.GetEntities(assemblies);
            foreach(var entity in entities)
            {
                _entities.Add(nameof(entity), entity);
            }
        }
        
        private IQueryable<BaseEntity> GetSet<T>(Type entity) where T: class
        {
            var genericMethod = setMethod.MakeGenericMethod(entity);
            dynamic result = genericMethod.Invoke(this, new object[] { });
            return result;
        }

        public IQueryable<BaseEntity> GetEntity(string entityName)
        {
            return GetSet<BaseEntity>(_entities[entityName]);
        }

        public IQueryable<BaseEntity> GetEntity(Type type)
        {
            return GetSet<BaseEntity>(type);
        }
        
        public DbSet<T> GetEntity<T>() where T: class
        {
            return Set<T>();
        }

        public async Task SaveAsync()
        {
            await base.SaveChangesAsync();
        }

        public async Task ExecuteSql(string sql) 
        {
            //await Database.ExecuteSqlInterpolatedAsync($"{sql}");
        }
    }

    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
            => context is DataStore dynamicContext
                ? (context.GetType(), dynamicContext._assemblyManager.GetEntities())
                : (object)context.GetType();

        public object Create(DbContext context, bool designTime) => context is DataStore dynamicContext
                ? (context.GetType(), dynamicContext._assemblyManager.GetEntities())
                : (object)context.GetType();
    }
}