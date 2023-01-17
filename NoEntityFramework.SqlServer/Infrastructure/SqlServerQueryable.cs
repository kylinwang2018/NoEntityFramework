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

        public object ParameterModel { get; set; }

        public void Dispose()
        {
            SqlCommand.Dispose();
            SqlConnection.Dispose();
        }
    }
}
