using System.Data;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using NoEntityFramework.DataAnnotations;

namespace SqliteDemo
{
    public class User
    {
        [SqliteDbParameter(Name = "$Id", Type = SqliteType.Integer)]
        public int Id { get; set; }

        [SqliteDbParameter(Name = "$Name", Type = SqliteType.Text)]
        public string? Name { get; set; }
    }
}
