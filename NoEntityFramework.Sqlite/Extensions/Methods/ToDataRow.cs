using NoEntityFramework.DataManipulators;
using System.Threading.Tasks;

namespace NoEntityFramework.Sqlite
{
    public static class ToDataRow
    {
        public static T? AsDataRow<T>(
            this ISqliteQueryable query) 
            where T : class, new ()
        {
            var dataTable = query.AsDataTable();
            return DataTableHelper.DataRowToT<T>(dataTable);
        }

        public static async Task<T?> AsDataRowAsync<T>(
            this ISqliteQueryable query)
            where T : class, new()
        {
            var dataTable = await query.AsDataTableAsync();
            return DataTableHelper.DataRowToT<T>(dataTable);
        }
    }
}
