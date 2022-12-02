using System;
using System.Linq;
using System.Reflection;

namespace Ionta.OSC.ToolKit.Store
{
    public interface IDataStore
    {
        public void InitEntity(Assembly[] assemblies);
        public IQueryable<T> GetEntity<T>() where T : class;
        public IQueryable<BaseEntity> GetEntity(Type type);
    }
}