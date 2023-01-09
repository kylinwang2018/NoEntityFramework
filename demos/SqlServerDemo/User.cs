using System.Data;
using NoEntityFramework.DataAnnotations;

namespace SqlServerDemo
{
    public class User
    {
        [SqlDbParameter(Name = "@Id", DbType = SqlDbType.Int)]
        public int Id { get; set; }

        [SqlDbParameter(Name = "@Name", DbType = SqlDbType.NVarChar)]
        public string? Name { get; set; }
    }
}
