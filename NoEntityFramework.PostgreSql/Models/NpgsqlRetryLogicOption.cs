using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace NoEntityFramework.Npgsql.Models
{
    public class NpgsqlRetryLogicOption
    {
        public int NumberOfTries { get; set; } = 6;
        public TimeSpan DeltaTime { get; set; } = TimeSpan.FromSeconds(5);
        public IPostgresLogger Logger { get; set; }
    }
}
