using NoEntityFramework.SqlServer;

namespace SqlServerDemo
{
    public class ApplicationDbContext : SqlServerDbContext
    {
        public ApplicationDbContext(ISqlServerOptions<ApplicationDbContext> sqlServerOptions) : base(sqlServerOptions)
        {
        }
    }
}
