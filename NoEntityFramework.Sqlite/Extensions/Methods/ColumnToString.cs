using NoEntityFramework.DataManipulators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoEntityFramework.Sqlite
{
    public static class ColumnToString
    {
        public static List<string> GetColumnToString(
            this ISqliteQueryable query,
            int columnIndex = 0)
        {
            var dataTable = query.AsDataTable();
            return DataTableHelper.DataTableToListString(dataTable, columnIndex);
        }

        public static List<string> GetColumnToString(
            this ISqliteQueryable query,
            string columnName)
        {
            var dataTable = query.AsDataTable();
            return DataTableHelper.DataTableToListString(dataTable, columnName);
        }

        public static async Task<List<string>> GetColumnToStringAsync(
            this ISqliteQueryable query,
            int columnIndex = 0)
        {
            var dataTable = await query.AsDataTableAsync();
            return DataTableHelper.DataTableToListString(dataTable, columnIndex);
        }

        public static async Task<List<string>> GetColumnToStringAsync(
            this ISqliteQueryable query,
            string columnName)
        {
            var dataTable = await query.AsDataTableAsync();
            return DataTableHelper.DataTableToListString(dataTable, columnName);
        }
    }
}
