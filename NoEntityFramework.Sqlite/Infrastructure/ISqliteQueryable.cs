using Microsoft.Data.Sqlite;

namespace NoEntityFramework.Sqlite
{
    public interface ISqliteQueryable
    {
        ISqlConnectionFactory ConnectionFactory { get; }

        ISqliteLogger Logger { get; }

        SqliteCommand SqlCommand { get; set; }

        SqliteConnection SqlConnection { get; } 

        object? ParameterModel { get; set; }
    }
}