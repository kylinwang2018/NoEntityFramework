using Microsoft.Extensions.Options;
using Npgsql;

namespace NoEntityFramework.Npgsql
{
    internal class NpgsqlConnectionFactory<TDbContext,TOption> : INpgsqlConnectionFactory<TDbContext, TOption>
        where TDbContext : class, IDbContext
        where TOption : class, IDbContextOptions
    {
        private readonly string _sqlConnectionString;

        public NpgsqlConnectionFactory(IOptionsMonitor<TOption> options)
        {
            _sqlConnectionString = options.Get(typeof(TDbContext).ToString()).ConnectionString;
        }

        public NpgsqlCommand CreateCommand()
        {
            return new NpgsqlCommand();
        }

        public NpgsqlCommand CreateCommand(string query)
        {
            return new NpgsqlCommand(query);
        }

        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_sqlConnectionString);
        }

        public NpgsqlConnection CreateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        public NpgsqlDataAdapter CreateDataAdapter()
        {
            return new NpgsqlDataAdapter();
        }
    }
}
