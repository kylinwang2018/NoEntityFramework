using System.Data;
using MySql.Data.MySqlClient;
using NoEntityFramework.DataAnnotations;

namespace MySqlDemo
{
    public class User
    {
        [MySqlDbParameter(Name = "@id", Type = MySqlDbType.Int32)]
        public int Id { get; set; }

        [MySqlDbParameter(Name = "@name", Type = MySqlDbType.VarChar)]
        public string? Name { get; set; }
    }
}
