using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace NoEntityFramework.Sqlite
{
    internal class SqliteQueryable : ISqliteQueryable
    {
        public SqliteQueryable(
            ISqlConnectionFactory connectionFactory,
            ISqliteLogger logger,
            SqliteConnection sqlConnection,
            SqliteCommand sqlCommand)
        {
            ConnectionFactory = connectionFactory;
            Logger = logger;
            SqlConnection = sqlConnection;
            SqlCommand = sqlCommand;
            SqlCommand.Connection = SqlConnection;
        }
        public ISqlConnectionFactory ConnectionFactory { get; }

        public ISqliteLogger Logger { get; }

        public SqliteCommand SqlCommand { get; set; }

        public SqliteConnection SqlConnection { get; }

        public object? ParameterModel { get; set; }
    }
}
