using Microsoft.Extensions.Options;

namespace NoEntityFramework.Sqlite
{
    public interface ISqliteOptions<out TDbContext>
        where TDbContext : class, ISqliteDbContext
    {
        ISqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }
        ISqliteLogger<TDbContext> Logger { get; }
        IOptionsMonitor<RelationalDbOptions> Options { get; }
    }
}