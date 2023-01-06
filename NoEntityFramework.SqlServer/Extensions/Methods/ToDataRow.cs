using NoEntityFramework.DataManipulators;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///    Cast a data row to an object. 
    /// </summary>
    public static class ToDataRow
    {
        /// <summary>
        ///     Cast a data row to an object.
        /// </summary>
        /// <typeparam name="T">The type of the object want to cast to.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        public static T? AsDataRow<T>(
            this ISqlServerQueryable query) 
            where T : class, new ()
        {
            var dataTable = query.AsDataTable();
            return DataTableHelper.DataRowToT<T>(dataTable);
        }

        /// <summary>
        ///     Cast a data row to an object.
        /// </summary>
        /// <typeparam name="T">The type of the object want to cast to.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        public static async Task<T?> AsDataRowAsync<T>(
            this ISqlServerQueryable query)
            where T : class, new()
        {
            var dataTable = await query.AsDataTableAsync();
            return DataTableHelper.DataRowToT<T>(dataTable);
        }
    }
}
