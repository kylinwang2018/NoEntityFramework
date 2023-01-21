using Microsoft.Data.Sqlite;
using NoEntityFramework.Sqlite.Models;
using System;
using System.Threading.Tasks;

namespace NoEntityFramework.Sqlite
{
    internal class SqliteQueryable : ISqliteQueryable
    {
        public SqliteQueryable(
            ISqliteConnectionFactory connectionFactory,
            ISqliteLogger logger,
            SqliteConnection sqlConnection,
            SqliteCommand sqlCommand)
        {
            ConnectionFactory = connectionFactory;
            Logger = logger;
            SqlConnection = sqlConnection;
            SqlCommand = sqlCommand;
            SqlCommand.Connection = SqlConnection;
            RetryLogicOption = new SqliteRetryLogicOption()
            {
                DeltaTime = TimeSpan.FromSeconds(5),
                NumberOfTries = 6
            };
        }
        public ISqliteConnectionFactory ConnectionFactory { get; }

        public ISqliteLogger Logger { get; }

        public SqliteCommand SqlCommand { get; set; }

        public SqliteConnection SqlConnection { get; }

#if NETSTANDARD2_0
        public object ParameterModel { get; set; } = null;
#else
        public object? ParameterModel { get; set; }
#endif

        public SqliteRetryLogicOption RetryLogicOption { get; set; }

        public void Dispose()
        {
            SqlCommand.Dispose();
            SqlConnection.Dispose();
        }
#if NETSTANDARD2_1
        public async ValueTask DisposeAsync()
        {
            await SqlCommand.DisposeAsync();
            await SqlConnection.DisposeAsync();
        }
#endif
    }
}
