using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Ionta.OSC.ToolKit.Store
{
    public interface IDataStore
    {
        public void InitEntity(Assembly[] assemblies);
        public DbSet<T> GetEntity<T>() where T : class;
        public IQueryable<BaseEntity> GetEntity(Type type);
    }
}