using NoEntityFramework.DataManipulators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoEntityFramework.Npgsql
{
    /// <summary>
    ///     Execute the query then get a column to string.
    /// </summary>
    public static class ColumnToString
    {
        /// <summary>
        ///     Execute the query then get a column as a list of string.
        /// </summary>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="columnIndex">the index of the column, start from <see langword="0" /> by default.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        public static List<string> GetColumnToString(
            this IPostgresQueryable query,
            int columnIndex = 0)
        {
            var dataTable = query.AsDataTable();
            return DataTableHelper.DataTableToListString(dataTable, columnIndex);
        }

        /// <summary>
        ///     Execute the query then get a column as a list of string.
        /// </summary>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="columnName">the name of the column.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        public static List<string> GetColumnToString(
            this IPostgresQueryable query,
            string columnName)
        {
            var dataTable = query.AsDataTable();
            return DataTableHelper.DataTableToListString(dataTable, columnName);
        }

        /// <summary>
        ///     Execute the query then get a column as a list of string.
        /// </summary>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="columnIndex">the index of the column, start from <see langword="0" /> by default.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        public static async Task<List<string>> GetColumnToStringAsync(
            this IPostgresQueryable query,
            int columnIndex = 0)
        {
            var dataTable = await query.AsDataTableAsync();
            return DataTableHelper.DataTableToListString(dataTable, columnIndex);
        }

        /// <summary>
        ///     Execute the query then get a column as a list of string.
        /// </summary>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="columnName">the name of the column.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        public static async Task<List<string>> GetColumnToStringAsync(
            this IPostgresQueryable query,
            string columnName)
        {
            var dataTable = await query.AsDataTableAsync();
            return DataTableHelper.DataTableToListString(dataTable, columnName);
        }
    }
}
