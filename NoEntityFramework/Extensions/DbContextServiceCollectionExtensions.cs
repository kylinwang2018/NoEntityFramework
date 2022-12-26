using Microsoft.Extensions.DependencyInjection;

namespace NoEntityFramework
{
    public class DbContext<TDbContext> 
        where TDbContext : class, IDbContext
    {
        public IServiceCollection ServiceCollection { get; set; }
    }
}
