using NoEntityFramework.DataManipulators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    public static class ColumnToString
    {
        public static List<string> GetColumnToString(
            this ISqlServerQueryable sqlServerQueryable,
            int columnIndex = 0)
        {
            var dataTable = sqlServerQueryable.AsDataTable();
            return DataTableHelper.DataTableToListString(dataTable, columnIndex);
        }

        public static List<string> GetColumnToString(
            this ISqlServerQueryable sqlServerQueryable,
            string columnName)
        {
            var dataTable = sqlServerQueryable.AsDataTable();
            return DataTableHelper.DataTableToListString(dataTable, columnName);
        }

        public static async Task<List<string>> GetColumnToStringAsync(
            this ISqlServerQueryable sqlServerQueryable,
            int columnIndex = 0)
        {
            var dataTable = await sqlServerQueryable.AsDataTableAsync();
            return DataTableHelper.DataTableToListString(dataTable, columnIndex);
        }

        public static async Task<List<string>> GetColumnToStringAsync(
            this ISqlServerQueryable sqlServerQueryable,
            string columnName)
        {
            var dataTable = await sqlServerQueryable.AsDataTableAsync();
            return DataTableHelper.DataTableToListString(dataTable, columnName);
        }
    }
}
