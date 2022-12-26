using NoEntityFramework.PostgresSQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NoEntityFramework
{
    public interface INpgsqlDbOptions<out TDbContext>
        where TDbContext : class, INpgsqlDbContext
    {
        INpgsqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }
        ILogger<TDbContext> Logger { get; }
        IOptionsMonitor<RelationalDbOptions> Options { get; }
    }
}