using System.Collections.Generic;
using System.Reflection;

namespace NoEntityFramework.Utilities
{
    /// <summary>
    /// The helper class that get all assembly you want.
    /// </summary>
    public class AppAssembly
    {
        /// <summary>
        /// Get All assemblys which the dll file starts with the spcific word.
        /// </summary>
        /// <param name="assemblyName">The name start with</param>
        /// <returns>All <see cref="Assembly"/> from the folder</returns>
        public static Assembly[] GetAll(params string[] assemblyName)
        {
            var loadedAssemblies = new List<Assembly>();
            foreach (var assembly in assemblyName)
            {
                try
                {
                    loadedAssemblies.Add(Assembly.Load(assembly));
                }
                catch { }
            }
            var assemblies = loadedAssemblies
                .ToArray();
            return assemblies;
        }
    }
}
