using System;
using System.Collections.Generic;
using System.Text;

namespace NoEntityFramework.Sqlite.Models
{
    public class SqliteRetryLogicOption
    {
        public int NumberOfTries { get; set; } = 6;
        public TimeSpan DeltaTime { get; set; } = TimeSpan.FromSeconds(5);
        public ISqliteLogger Logger { get; set; }
    }
}
