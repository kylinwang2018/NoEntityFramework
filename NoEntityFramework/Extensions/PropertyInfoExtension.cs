using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace NoEntityFramework
{
    public static class PropertyInfoExtension
    {
        /// <summary>
        ///     <para>
        ///         Get the column name of select property.
        ///     </para>
        ///     <para>
        ///         If the property contains a <see cref="ColumnAttribute"/> with a ColumnName, 
        ///         this ColumnName will be returned; otherwise the property name will be returned.
        ///     </para>
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> object</param>
        /// <returns>The column name</returns>
        public static string GetColumnName(this PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;

            var attr = propertyInfo.GetCustomAttributes<ColumnAttribute>(false).FirstOrDefault();
            if (attr != null && !string.IsNullOrEmpty(attr.Name)) 
                name = attr.Name;

            return name;
        }
    }
}
