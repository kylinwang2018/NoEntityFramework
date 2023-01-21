using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace NoEntityFramework.MySql.Models
{
    public class MySqlRetryLogicOption
    {
        public int NumberOfTries { get; set; } = 6;
        public TimeSpan DeltaTime { get; set; } = TimeSpan.FromSeconds(5);
        public IMySqlLogger Logger { get; set; }
    }
}
