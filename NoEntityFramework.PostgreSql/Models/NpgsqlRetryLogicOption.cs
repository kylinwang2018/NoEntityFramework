using System;

namespace NoEntityFramework.PostgresSQL
{
    public class NpgsqlRetryLogicOption
    {
        public int NumberOfTries { get; set; } = 5;
        public TimeSpan DeltaTime { get; set; } = TimeSpan.FromSeconds(5);
    }
}
