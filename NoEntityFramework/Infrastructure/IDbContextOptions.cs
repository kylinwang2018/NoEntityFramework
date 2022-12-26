namespace NoEntityFramework
{
    /// <summary>
    /// The options to be used by a DbContext. 
    /// You normally override <see cref="DbContextOptions"/> to
    /// create instances of this class and it is not designed to be directly constructed in your application code.
    /// </summary>
    public interface IDbContextOptions
    {
        /// <summary>
        /// Gets or sets the string used to open a SQL Server database.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Specifies the wait time before terminating the attempt to execute a query and generating an error.
        /// </summary>
        int DbCommandTimeout { get; set; }

        /// <summary>
        /// Preferred gap time to delay before retry
        /// </summary>
        int DeltaTime { get; set; }

        /// <summary>
        /// Maximum gap time for each delay time before retry
        /// </summary>
        int MaxTimeInterval { get; set; }

        /// <summary>
        /// Tries n times before throwing an exception
        /// </summary>
        int NumberOfTries { get; set; }

        /// <summary>
        /// Log database execution time and network travel time
        /// </summary>
        bool EnableStatistics { get; set; }
    }
}