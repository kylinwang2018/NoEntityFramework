using System;
using System.Collections.Generic;
using System.Text;

namespace NoEntityFramework.MongoDb
{
    /// <summary>
    /// The options to be used by a DbContext. 
    /// You normally override this <see cref="MongoDbContextOptions"/> to
    /// create instances of this class and it is not designed to be directly constructed in your application code.
    /// </summary>
    public class MongoDbContextOptions : DbContextOptions, IMongoDbContextOptions
    {
        /// <summary>
        /// The name of your MongoDb database you are going to use.
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return _databaseName;
            }
            set
            {
                if (isDatabaseNameInitialized)
                    throw new FieldAccessException("DatabaseName cannot be modified after initialized.");
                _databaseName = value;
                isDatabaseNameInitialized = true;
            }
        }

        private string _databaseName = string.Empty;
        private bool isDatabaseNameInitialized = false;
    }
}
