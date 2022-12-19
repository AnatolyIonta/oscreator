using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ionta.OSC.ToolKit.Store;
using Ionta.OSC.ToolKit.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace Ionta.StoreLoader
{
    public class DataStore : DbContext, IDataStore
    {
        private readonly Dictionary<string, Type> _entities = new();
        public readonly IAssemblyManager _assemblyManager;
        
        public DataStore(DbContextOptions<DataStore> options, IAssemblyManager assemblyManager)
            : base(options)
        { 
            _assemblyManager = assemblyManager;
            _assemblyManager.OnChange += InitEntity;
            _assemblyManager.OnUnloading += OnUnloading;
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
            modelBuilder.RegisterAllEntities<BaseEntity>(_assemblyManager.GetAssemblies());
            return;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
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
    }

    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
            => context is DataStore dynamicContext
                ? (context.GetType(), dynamicContext._assemblyManager.GetEntities())
                : (object)context.GetType();

        public object Create(DbContext context, bool designTime)
        {
            throw new NotImplementedException();
        }
    }
}