using NoEntityFramework.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoEntityFramework.SqlServer.Infrastructure;

namespace NoEntityFramework
{
    public interface ISqlServerOptions<out TDbContext>
        where TDbContext : class, ISqlServerDbContext
    {
        ISqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }
        ISqlServerLogger<TDbContext> Logger { get; }
        IOptionsMonitor<RelationalDbOptions> Options { get; }
    }
}