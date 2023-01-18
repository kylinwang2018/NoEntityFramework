using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using NoEntityFramework.Sqlite.Models;

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

        public object ParameterModel { get; set; } = null;

        public SqliteRetryLogicOption RetryLogicOption { get; set; }

        public void Dispose()
        {
            SqlCommand.Dispose();
            SqlConnection.Dispose();
        }
    }
}
