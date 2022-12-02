using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ionta.OSC.ToolKit.Store
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public Guid ExternalId { get; set; }

        public T Get<T>(string name)
        {
            var prop = typeof(BaseEntity).GetProperty(name, BindingFlags.Public);
            if (prop == null) throw new KeyNotFoundException();
            
            var getAccessor = prop.GetMethod;
            if (!getAccessor.IsPublic) throw new Exception("Property is not public");
            
            return (T)getAccessor.Invoke(this,new object[]{});
        }
        
        public void Set(string name, object value)
        {
            var prop = typeof(BaseEntity).GetProperty(name, BindingFlags.Public);
            if (prop == null) throw new KeyNotFoundException();
            
            var setAccessor = prop.SetMethod;
            if(!setAccessor.IsPublic) throw new Exception("Property is not public");

            setAccessor.Invoke(this,new object[]{value});
        }
    }
}