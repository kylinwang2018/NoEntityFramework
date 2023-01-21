using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NoEntityFramework.MySql.Models;

namespace NoEntityFramework.MySql
{
    internal class MySqlQueryable : IMySqlQueryable
    {
        public MySqlQueryable(
            IMySqlConnectionFactory connectionFactory,
            IMySqlLogger logger,
            MySqlConnection sqlConnection,
            MySqlCommand sqlCommand)
        {
            ConnectionFactory = connectionFactory;
            Logger = logger;
            SqlConnection = sqlConnection;
            SqlCommand = sqlCommand;
            SqlCommand.Connection = SqlConnection;
            RetryLogicOption = new MySqlRetryLogicOption()
            {
                DeltaTime = TimeSpan.FromSeconds(5),
                NumberOfTries = 6
            };
        }
        public IMySqlConnectionFactory ConnectionFactory { get; }

        public IMySqlLogger Logger { get; }

        public MySqlCommand SqlCommand { get; set; }

        public MySqlConnection SqlConnection { get; }

#if NETSTANDARD2_0
        public object ParameterModel { get; set; }
#else
        public object? ParameterModel { get; set; }
#endif

        public MySqlRetryLogicOption RetryLogicOption { get; set; }

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
