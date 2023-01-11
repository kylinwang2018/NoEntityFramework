using System.Data;
using NoEntityFramework.DataAnnotations;

namespace SqlServerDemo
{
    public class User
    {
        [SqlDbParameter(Name = "@Id", Type = SqlDbType.Int)]
        public int Id { get; set; }

        [SqlDbParameter(Name = "@Name", Type = SqlDbType.NVarChar)]
        public string? Name { get; set; }
    }
}
