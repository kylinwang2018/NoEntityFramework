using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NoEntityFramework.Npgsql
{
    internal static class ParameterAttacher
    {
        /// <summary>
        /// <para>
        ///     Attach an array of <see cref="NpgsqlParameter"/> to a <see cref="NpgsqlCommand"/>.
        /// </para>
        /// <para>
        ///     Assign a value of <see cref="DBNull"/> to any parameter with a direction of
        ///     InputOutput and a value of null.  
        /// </para>
        /// <para>
        ///     Prevent default values from being used, but
        ///     this will be the less common case than an intended pure output parameter (derived as InputOutput)
        ///     where the user provided no input value.
        /// </para>
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">An array of <see cref="NpgsqlParameter"/> to be added to command</param>
        public static NpgsqlCommand AttachParameters(this NpgsqlCommand command, params NpgsqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            foreach (var p in commandParameters)
            {
                // Check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput ||
                     p.Direction == ParameterDirection.Input) &&
                    (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                command.Parameters.Add(p);
            }
            return command;
        }

        /// <summary>
        /// <para>
        ///     Attach an array of <see cref="NpgsqlParameter"/> to a <see cref="NpgsqlCommand"/>.
        /// </para>
        /// <para>
        ///     Assign a value of <see cref="DBNull"/> to any parameter with a direction of
        ///     InputOutput and a value of null.  
        /// </para>
        /// <para>
        ///     Prevent default values from being used, but
        ///     this will be the less common case than an intended pure output parameter (derived as InputOutput)
        ///     where the user provided no input value.
        /// </para>
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">An array of <see cref="NpgsqlParameter"/> to be added to command</param>
        public static NpgsqlCommand AttachParameters(this NpgsqlCommand command, Array commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");

            foreach (NpgsqlParameter p in commandParameters)
            {
                if (p != null)
                {
                    // Check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput ||
                        p.Direction == ParameterDirection.Input) &&
                        (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    command.Parameters.Add(p);
                }
            }
            return command;
        }
    }
}
