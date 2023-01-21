using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    internal class SqlServerQueryable : ISqlServerQueryable
    {
        public SqlServerQueryable(
            ISqlConnectionFactory connectionFactory,
            ISqlServerLogger logger,
            SqlConnection sqlConnection,
            SqlCommand sqlCommand)
        {
            ConnectionFactory = connectionFactory;
            Logger = logger;
            SqlConnection = sqlConnection;
            SqlCommand = sqlCommand;
            SqlCommand.Connection = SqlConnection;
        }
        public ISqlConnectionFactory ConnectionFactory { get; }

        public ISqlServerLogger Logger { get; }

        public SqlCommand SqlCommand { get; set; }

        public SqlConnection SqlConnection { get; }

#if NETSTANDARD2_0
        public object ParameterModel { get; set; } = null;
#else
        public object? ParameterModel { get; set; }
#endif

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
