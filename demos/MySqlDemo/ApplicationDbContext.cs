using NoEntityFramework.MySql;

namespace MySqlDemo
{
    public class ApplicationDbContext : MySqlDbContext
    {
        public ApplicationDbContext(IMySqlOptions<ApplicationDbContext> mySqlOptions) : base(mySqlOptions)
        {
        }
    }
}
