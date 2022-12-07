using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ionta.OSC.ToolKit.Store;
using Ionta.OSC.ToolKit.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Ionta.StoreLoader
{
    public class DataStore : DbContext, IDataStore
    {
        private readonly Dictionary<string, IQueryable<BaseEntity>> _entities = new();
        private readonly IAssemblyManager _assemblyManager;
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
        
        private static MethodInfo setMethod = typeof(DataStore).GetMethods()
            .Single(m => m.Name == nameof(DataStore.GetEntity) && m.GetParameters().Length == 0);
        public void InitEntity(Assembly[] assemblies)
        {
            var EntityTypes = _assemblyManager.GetEntities(assemblies);
            foreach (var entity in EntityTypes)
            {
                var genericMethod = setMethod.MakeGenericMethod(entity);
                var dataSet = genericMethod.Invoke(this, new[] { entity.Name });
                if(dataSet != null)
                    _entities.Add(entity.Name, (IQueryable<BaseEntity>)dataSet);
            }
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
}