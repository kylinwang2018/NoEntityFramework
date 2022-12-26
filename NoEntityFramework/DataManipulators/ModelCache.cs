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
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> OutputModelCache = new ConcurrentDictionary<Type, List<PropertyInfo>>();

        public static IList<PropertyInfo> GetProperties(Type type)
        {
            if (OutputModelCache.TryGetValue(type, out var list))
                return list;

            list = new List<PropertyInfo>(
                type.GetProperties()
                    .Where(s => s.CanWrite)
                    .Where(s => !s.GetCustomAttributes<IgnoredAttribute>().Any())
            );
            OutputModelCache.TryAdd(type, list);

            return list;
        }
    }
}
