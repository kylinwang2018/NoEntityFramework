using NoEntityFramework.DataManipulators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     Execute the query then get a column to string.
    /// </summary>
    public static class ColumnToString
    {
        /// <summary>
        ///     Execute the query then get a column as a list of string.
        /// </summary>
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="columnIndex">the index of the column, start from <see langword="0" /> by default.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        public static List<string> GetColumnAsString(
            this ISqlServerQueryable sqlServerQueryable,
            int columnIndex = 0)
        {
            var dataTable = sqlServerQueryable.AsDataTable();
            return DataTableHelper.DataTableToListString(dataTable, columnIndex);
        }

        /// <summary>
        ///     Execute the query then get a column as a list of string.
        /// </summary>
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="columnName">the name of the column.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        public static List<string> GetColumnAsString(
            this ISqlServerQueryable sqlServerQueryable,
            string columnName)
        {
            var dataTable = sqlServerQueryable.AsDataTable();
            return DataTableHelper.DataTableToListString(dataTable, columnName);
        }

        /// <summary>
        ///     Execute the query then get a column as a list of string.
        /// </summary>
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="columnIndex">the index of the column, start from <see langword="0" /> by default.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        public static async Task<List<string>> GetColumnAsStringAsync(
            this ISqlServerQueryable sqlServerQueryable,
            int columnIndex = 0)
        {
            var dataTable = await sqlServerQueryable.AsDataTableAsync();
            return DataTableHelper.DataTableToListString(dataTable, columnIndex);
        }

        /// <summary>
        ///     Execute the query then get a column as a list of string.
        /// </summary>
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="columnName">the name of the column.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        public static async Task<List<string>> GetColumnAsStringAsync(
            this ISqlServerQueryable sqlServerQueryable,
            string columnName)
        {
            var dataTable = await sqlServerQueryable.AsDataTableAsync();
            return DataTableHelper.DataTableToListString(dataTable, columnName);
        }
    }
}
