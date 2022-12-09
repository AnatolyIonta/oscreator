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

namespace Ionta.StoreLoader
{
    public class DataStore : DbContext, IDataStore
    {
        private readonly Dictionary<string, IQueryable<BaseEntity>> _entities = new();
        public readonly IAssemblyManager _assemblyManager;
        private ModelBuilder _builder;
        
        public DataStore(DbContextOptions<DataStore> options, IAssemblyManager assemblyManager)
            : base(options)
        { 
            _assemblyManager = assemblyManager;
            //assemblyManager.OnChange += InitEntity;
            Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            _builder = modelBuilder;
            _builder.RegisterAllEntities<BaseEntity>(_assemblyManager.GetAssemblies());
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
            
        }
        
        private IQueryable<BaseEntity> GetSet<T>(Type entity) where T: class
        {
            var genericMethod = setMethod.MakeGenericMethod(entity);
            dynamic result = genericMethod.Invoke(this, new object[] { });
            return result;
        }

        public IQueryable<BaseEntity> GetEntity(Type type)
        {
            return GetSet<BaseEntity>(type);
        }
        
        public DbSet<T> GetEntity<T>() where T: class
        {
            return Set<T>();
        }
    }

    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
            => context is DataStore dynamicContext
                ? (context.GetType(), dynamicContext._assemblyManager.GetEntities())
                : (object)context.GetType();
    }
}