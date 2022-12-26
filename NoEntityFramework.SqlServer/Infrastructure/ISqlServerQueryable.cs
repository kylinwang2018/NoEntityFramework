using Microsoft.Data.SqlClient;
using NoEntityFramework.SqlServer.Infrastructure;

namespace NoEntityFramework.SqlServer
{
    public interface ISqlServerQueryable
    {
        ISqlConnectionFactory ConnectionFactory { get; }

        ISqlServerLogger Logger { get; }

        SqlCommand SqlCommand { get; set; }

        SqlConnection SqlConnection { get; } 

        object? ParameterModel { get; set; }
    }
}