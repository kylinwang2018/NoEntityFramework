using System.Data;
using NoEntityFramework.DataAnnotations;
using NpgsqlTypes;

namespace PostgresDemo
{
    public class User
    {
        [PostgresDbParameter(Name = "var_id", Type = NpgsqlDbType.Integer)]
        public int Id { get; set; }

        [PostgresDbParameter(Name = "var_name", Type = NpgsqlDbType.Varchar)]
        public string? Name { get; set; }
    }
}
