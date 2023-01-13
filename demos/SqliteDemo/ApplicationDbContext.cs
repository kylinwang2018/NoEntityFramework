using NoEntityFramework.Sqlite;

namespace SqliteDemo
{
    public class ApplicationDbContext : SqliteDbContext
    {
        public ApplicationDbContext(ISqliteOptions<ApplicationDbContext> sqliteOptions) : base(sqliteOptions)
        {
        }
    }
}
