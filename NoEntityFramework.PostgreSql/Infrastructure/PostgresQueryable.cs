using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NoEntityFramework.Npgsql.Models;
using Npgsql;

namespace NoEntityFramework.Npgsql
{
    internal class PostgresQueryable : IPostgresQueryable
    {
        public PostgresQueryable(
            IPostgresConnectionFactory connectionFactory,
            IPostgresLogger logger,
            NpgsqlConnection sqlConnection,
            NpgsqlCommand sqlCommand)
        {
            ConnectionFactory = connectionFactory;
            Logger = logger;
            SqlConnection = sqlConnection;
            SqlCommand = sqlCommand;
            SqlCommand.Connection = SqlConnection;
            RetryLogicOption = new NpgsqlRetryLogicOption()
            {
                DeltaTime = TimeSpan.FromSeconds(5),
                NumberOfTries = 6
            };
        }
        public IPostgresConnectionFactory ConnectionFactory { get; }

        public IPostgresLogger Logger { get; }

        public NpgsqlCommand SqlCommand { get; set; }

        public NpgsqlConnection SqlConnection { get; }

        public object ParameterModel { get; set; } = null;

        public NpgsqlRetryLogicOption RetryLogicOption { get; set; }

        public void Dispose()
        {
            SqlCommand.Dispose();
            SqlConnection.Dispose();
        }
    }
}
