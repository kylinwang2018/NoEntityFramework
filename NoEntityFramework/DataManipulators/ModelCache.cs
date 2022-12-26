using NoEntityFramework.DataAnnotations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NoEntityFramework.DataManipulators
{
    public class ModelCache
    {
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _outputModelCache = new ConcurrentDictionary<Type, List<PropertyInfo>>();

        public static IList<PropertyInfo> GetProperties(Type type)
        {
            if (!_outputModelCache.TryGetValue(type, out List<PropertyInfo> list))
            {
                list = new List<PropertyInfo>(
                    type.GetProperties()
                    .Where(s => s.CanWrite)
                    .Where(s => !s.GetCustomAttributes<IgnoredAttribute>().Any())
                    );
                _outputModelCache.TryAdd(type, list);
            }

            return list;
        }
    }
}
