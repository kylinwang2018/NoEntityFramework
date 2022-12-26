using NoEntityFramework.DataManipulators;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    public static class ToDataRow
    {
        public static T? AsDataRow<T>(
            this ISqlServerQueryable sqlServerQueryable) 
            where T : class, new ()
        {
            var dataTable = sqlServerQueryable.AsDataTable();
            return DataTableHelper.DataRowToT<T>(dataTable);
        }

        public static async Task<T?> AsDataRowAsync<T>(
            this ISqlServerQueryable sqlServerQueryable)
            where T : class, new()
        {
            var dataTable = await sqlServerQueryable.AsDataTableAsync();
            return DataTableHelper.DataRowToT<T>(dataTable);
        }
    }
}
