using System;
using System.Collections.Generic;
using System.Data;

namespace NoEntityFramework
{
    public static class DataReaderExtension
    {
        /// <summary>
        ///     Process data from a <see cref="IDataReader"/> directly.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="reader"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> Select<TResult>(this IDataReader reader,
                                       Func<IDataReader, TResult> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
    }
}
