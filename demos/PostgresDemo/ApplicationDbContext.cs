using NoEntityFramework.Npgsql;

namespace PostgresDemo
{
    public class ApplicationDbContext : PostgresDbContext
    {
        public ApplicationDbContext(IPostgresOptions<ApplicationDbContext> postgresOptions) : base(postgresOptions)
        {
        }
    }
}
