using System;
using System.Data.Common;

namespace NoEntityFramework.Exceptions
{
    /// <summary>
    /// The Exception that related to this library
    /// </summary>
    public class DatabaseException : DbException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseException"/> class
        /// </summary>
        public DatabaseException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseException"/> class with the
        ///     specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public DatabaseException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseException"/> class with the
        ///     specified error message and a reference to the inner exception that is the cause
        ///     of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public DatabaseException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
