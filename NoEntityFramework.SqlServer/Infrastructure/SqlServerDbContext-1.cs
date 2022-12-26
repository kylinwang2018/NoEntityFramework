using Microsoft.Data.SqlClient;
using NoEntityFramework.DataManipulators;
using NoEntityFramework.Exceptions;
using NoEntityFramework.SqlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace NoEntityFramework
{
    public partial class SqlServerDbContext : ISqlServerDbContext
    {
        #region GetColumnToString
        public List<string> GetColumnToString(SqlCommand cmd, int columnIndex = 0)
        {
            var datatable = GetDataTable(cmd);
            return DataTableHelper.DataTableToListString(datatable, columnIndex);
        }

        public List<string> GetColumnToString(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToListString(datatable, columnIndex);
        }

        public List<string> GetColumnToString(SqlCommand cmd, string columnName)
        {
            var datatable = GetDataTable(cmd);
            return DataTableHelper.DataTableToListString(datatable, columnName);
        }

        public List<string> GetColumnToString(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToListString(datatable, columnName);
        }
        #endregion

        #region GetDataTable
        public DataTable GetDataTable(SqlCommand cmd)
        {
            var dataTable = new DataTable();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                cmd.Connection = sqlConnection;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                using var sqlDataAdapter = _connectionFactory.CreateDataAdapter();
                sqlDataAdapter.SelectCommand = cmd;
                sqlDataAdapter.Fill(dataTable);

                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return dataTable;
        }

        public DataTable GetDataTable(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            try
            {
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;

                var dataTable = new DataTable();
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.Close();
                    connection.Open();
                }
                using (var sqlTransaction = connection.BeginTransaction())
                {
                    using var sqlDataAdapter = _connectionFactory.CreateDataAdapter();
                    cmd.Transaction = sqlTransaction;
                    sqlDataAdapter.SelectCommand = cmd;
                    sqlDataAdapter.Fill(dataTable);
                    sqlTransaction.Commit();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    connection.Close();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
        }

        public DataTable GetDataTable(string query, CommandType commandType)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            return GetDataTable(sqlCommand);
        }

        public DataTable GetDataTable(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.AttachParameters(sqlParameters);
            return GetDataTable(sqlCommand);
        }

        public List<T> GetDataTable<T>(SqlCommand cmd) where T : class, new()
        {
            var datatable = GetDataTable(cmd);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public List<T> GetDataTable<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public List<T> GetDataTable<T>(string query, CommandType commandType) where T : class, new()
        {
            var datatable = GetDataTable(query, commandType);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public List<T> GetDataTable<T>(string query, CommandType commandType, params SqlParameter[] sqlParameters) where T : class, new()
        {
            var datatable = GetDataTable(query, commandType, sqlParameters);
            return DataTableHelper.DataTableToList<T>(datatable);
        }
        #endregion

        #region GetDataSet
        public DataSet GetDataSet(SqlCommand cmd)
        {
            var dataSet = new DataSet();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                cmd.Connection = sqlConnection;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                using var sqlDataAdapter = _connectionFactory.CreateDataAdapter();
                sqlDataAdapter.SelectCommand = cmd;
                sqlDataAdapter.Fill(dataSet);

                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return dataSet;
        }

        public DataSet GetDataSet(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            try
            {
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;

                var dataSet = new DataSet();
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.Close();
                    connection.Open();
                }
                using (var sqlTransaction = connection.BeginTransaction())
                {
                    using var sqlDataAdapter = _connectionFactory.CreateDataAdapter();
                    cmd.Transaction = sqlTransaction;
                    sqlDataAdapter.SelectCommand = cmd;
                    sqlDataAdapter.Fill(dataSet);
                    sqlTransaction.Commit();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    connection.Close();
                }
                return dataSet;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
        }

        public DataSet GetDataSet(string query, CommandType commandType)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            return GetDataSet(sqlCommand);
        }

        public DataSet GetDataSet(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.AttachParameters(sqlParameters);
            return GetDataSet(sqlCommand);
        }
        #endregion

        #region GetDataRow
        public T? GetDataRow<T>(SqlCommand cmd) where T : class, new()
        {
            var datatable = GetDataTable(cmd);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public T? GetDataRow<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public T? GetDataRow<T>(string query, CommandType commandType) where T : class, new()
        {
            var datatable = GetDataTable(query, commandType);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public T? GetDataRow<T>(string query, CommandType commandType, params SqlParameter[] sqlParameters) where T : class, new()
        {
            var datatable = GetDataTable(query, commandType, sqlParameters);
            return DataTableHelper.DataRowToT<T>(datatable);
        }
        #endregion

        #region GetDictionary
        public Dictionary<T, U>? GetDictionary<T, U>(string query, CommandType commandType)
        {
            return GetDictionary<T, U>(query, commandType, 0, 1);
        }

        public Dictionary<T, U>? GetDictionary<T, U>(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            return GetDictionary<T, U>(query, commandType, 0, 1, sqlParameters);
        }

        public Dictionary<T, U>? GetDictionary<T, U>(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex)
        {
            var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandText = query;
            return GetDictionary<T, U>(sqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public Dictionary<T, U>? GetDictionary<T, U>(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex, params SqlParameter[] sqlParameters)
        {
            var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandText = query;
            sqlCommand.AttachParameters(sqlParameters);
            return GetDictionary<T, U>(sqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public Dictionary<T, U>? GetDictionary<T, U>(SqlCommand cmd)
        {
            return GetDictionary<T, U>(cmd, 0, 1);
        }

        public Dictionary<T, U>? GetDictionary<T, U>(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                using var sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], (U)sqlDataReader[valueColumnIndex]);
                }
                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public Dictionary<string, string>? GetDictionary(string query, CommandType commandType)
        {
            return GetDictionary(query, commandType, 0, 1);
        }

        public Dictionary<string, string>? GetDictionary(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            return GetDictionary(query, commandType, 0, 1, sqlParameters);
        }

        public Dictionary<string, string>? GetDictionary(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex)
        {
            var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandText = query; 
            return GetDictionary(sqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public Dictionary<string, string>? GetDictionary(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex, params SqlParameter[] sqlParameters)
        {
            var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandText = query;
            sqlCommand.AttachParameters(sqlParameters);
            return GetDictionary(sqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public Dictionary<string, string>? GetDictionary(SqlCommand cmd)
        {
            return GetDictionary(cmd, 0, 1);
        }

        public Dictionary<string, string>? GetDictionary(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                using var sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                }
                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }
        #endregion

        #region GetDictionaryOfObjects
        public Dictionary<T, U>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                using var sqlDataReader = cmd.ExecuteReader();
                var typeFromHandle = typeof(U);
                var objectPropertiesCache = ModelCache._outputModelCache;
                List<PropertyInfo> list;
                lock (objectPropertiesCache)
                {
                    if (!ModelCache._outputModelCache.TryGetValue(typeFromHandle, out list))
                    {
                        list = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        list.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        ModelCache._outputModelCache.Add(typeFromHandle, list);
                    }
                }
                while (sqlDataReader.Read())
                {
                    U u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in list)
                    {
                        try
                        {
                            bool isEnum = propertyInfo.PropertyType.IsEnum;
                            if (isEnum)
                            {
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)sqlDataReader[propertyInfo.GetColumnName()]), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(u, Convert.ChangeType(sqlDataReader[propertyInfo.GetColumnName()], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], u);
                }
                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public Dictionary<T, U>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                using var sqlDataReader = cmd.ExecuteReader();
                var typeFromHandle = typeof(U);
                var objectPropertiesCache = ModelCache._outputModelCache;
                List<PropertyInfo> list;
                lock (objectPropertiesCache)
                {
                    if (!ModelCache._outputModelCache.TryGetValue(typeFromHandle, out list))
                    {
                        list = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        list.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        ModelCache._outputModelCache.Add(typeFromHandle, list);
                    }
                }
                while (sqlDataReader.Read())
                {
                    U u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in list)
                    {
                        try
                        {
                            bool isEnum = propertyInfo.PropertyType.IsEnum;
                            if (isEnum)
                            {
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)sqlDataReader[propertyInfo.GetColumnName()]), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(u, Convert.ChangeType(sqlDataReader[propertyInfo.GetColumnName()], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                    dictionary.Add((T)sqlDataReader[keyColumnName], u);
                }
                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                using var sqlDataReader = cmd.ExecuteReader();
                var typeFromHandle = typeof(U);
                var objectPropertiesCache = ModelCache._outputModelCache;
                List<PropertyInfo> list;
                lock (objectPropertiesCache)
                {
                    if (!ModelCache._outputModelCache.TryGetValue(typeFromHandle, out list))
                    {
                        list = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        list.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        ModelCache._outputModelCache.Add(typeFromHandle, list);
                    }
                }
                while (sqlDataReader.Read())
                {
                    bool flag = false;
                    if (dictionary.TryGetValue((T)((object)sqlDataReader[keyColumnIndex]), out List<U> list2))
                        flag = true;
                    U u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in list)
                    {
                        try
                        {
                            bool isEnum = propertyInfo.PropertyType.IsEnum;
                            if (isEnum)
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)sqlDataReader[propertyInfo.GetColumnName()]), null);
                            else
                                propertyInfo.SetValue(u, Convert.ChangeType(sqlDataReader[propertyInfo.GetColumnName()], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                        }
                    }
                    if (flag)
                        list2.Add(u);
                    else
                    {
                        list2 = new List<U>
                        {
                            u
                        };
                        dictionary.Add((T)sqlDataReader[keyColumnIndex], list2);
                    }
                }
                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                using var sqlDataReader = cmd.ExecuteReader();
                var typeFromHandle = typeof(U);
                var objectPropertiesCache = ModelCache._outputModelCache;
                List<PropertyInfo> list;
                lock (objectPropertiesCache)
                {
                    if (!ModelCache._outputModelCache.TryGetValue(typeFromHandle, out list))
                    {
                        list = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        list.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        ModelCache._outputModelCache.Add(typeFromHandle, list);
                    }
                }
                while (sqlDataReader.Read())
                {
                    var flag = false;
                    if (dictionary.TryGetValue((T)((object)sqlDataReader[keyColumnName]), out List<U> list2))
                    {
                        flag = true;
                    }
                    U u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in list)
                    {
                        try
                        {
                            bool isEnum = propertyInfo.PropertyType.IsEnum;
                            if (isEnum)
                            {
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)sqlDataReader[propertyInfo.GetColumnName()]), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(u, Convert.ChangeType(sqlDataReader[propertyInfo.GetColumnName()], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (flag)
                    {
                        list2.Add(u);
                    }
                    else
                    {
                        list2 = new List<U>
                        {
                            u
                        };
                        dictionary.Add((T)((object)sqlDataReader[keyColumnName]), list2);
                    }
                }
                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public Dictionary<T, U>? GetDictionaryOfObjects<T, U>(string query, CommandType commandType, int keyColumnIndex) where U : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return GetDictionaryOfObjects<T, U>(sqlCommand, keyColumnIndex);
        }

        public Dictionary<T, U>? GetDictionaryOfObjects<T, U>(string query, CommandType commandType, string keyColumnName) where U : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return GetDictionaryOfObjects<T, U>(sqlCommand, keyColumnName);
        }

        public Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(string query, CommandType commandType, int keyColumnIndex) where U : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return GetDictionaryOfListObjects<T, U>(sqlCommand, keyColumnIndex);
        }

        public Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(string query, CommandType commandType, string keyColumnName) where U : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return GetDictionaryOfListObjects<T, U>(sqlCommand, keyColumnName);
        }
        #endregion

        #region GetListListString
        public List<List<string>>? GetListListString(SqlCommand cmd)
        {
            var list = new List<List<string>>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                using var sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    var list2 = new List<string>();
                    for (int i = 0; i < sqlDataReader.FieldCount; i++)
                    {
                        list2.Add(sqlDataReader[i].ToString());
                    }
                    list.Add(list2);
                }
                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (list.Any())
                return null;
            else
                return list;
        }

        public List<List<string>>? GetListListString(SqlCommand cmd, string dateFormat)
        {
            var list = new List<List<string>>();
            bool flag = true;
            bool[]? array = null;
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                using var sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    List<string> list2 = new List<string>();
                    if (flag)
                    {
                        flag = false;
                        array = new bool[sqlDataReader.FieldCount];
                        for (int i = 0; i < sqlDataReader.FieldCount; i++)
                        {
                            if (sqlDataReader[i].GetType() == typeof(DateTime))
                            {
                                array[i] = true;
                                list2.Add(((DateTime)sqlDataReader[i]).ToString(dateFormat));
                            }
                            else
                            {
                                list2.Add(sqlDataReader[i].ToString());
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < sqlDataReader.FieldCount; j++)
                        {
                            if (array[j])
                            {
                                list2.Add(((DateTime)sqlDataReader[j]).ToString(dateFormat));
                            }
                            else
                            {
                                list2.Add(sqlDataReader[j].ToString());
                            }
                        }
                    }
                    list.Add(list2);
                }
                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (list.Any())
                return null;
            else
                return list;
        }

        public List<List<string>>? GetListListString(string query, CommandType commandType)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return GetListListString(sqlCommand);
        }

        public List<List<string>>? GetListListString(string query, CommandType commandType, string dateFormat)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return GetListListString(sqlCommand, dateFormat);
        }
        #endregion

        #region GetListOf<T>
        public List<T> GetListOf<T>(SqlCommand cmd, SqlConnection connection) where T: class, new()
        {
            if (_options.EnableStatistics)
                connection.StatisticsEnabled = true;
            var list = new List<T>();
            var type = typeof(T);
            cmd.Connection = connection;
            var objectPropertiesCache = ModelCache._outputModelCache;
            List<PropertyInfo> list2;
            lock (objectPropertiesCache)
            {
                if (!ModelCache._outputModelCache.TryGetValue(type, out list2))
                {
                    list2 = new List<PropertyInfo>(type.GetProperties());
                    list2.RemoveAll((PropertyInfo item) => !item.CanWrite);
                    ModelCache._outputModelCache.Add(type, list2);
                }
            }
            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.Close();
                    connection.Open();
                }
                using var sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    T obj = (T)Activator.CreateInstance(type);
                    foreach (var propertyInfo in list2)
                    {
                        try
                        {
                            if (propertyInfo.PropertyType.IsEnum)
                            {
                                propertyInfo.SetValue(obj, Enum.ToObject(propertyInfo.PropertyType, (int)sqlDataReader[propertyInfo.GetColumnName()]), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(obj, Convert.ChangeType(sqlDataReader[propertyInfo.GetColumnName()], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                    list.Add(obj);
                }
                LogSqlInfo(cmd, connection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return list;
        }

        public List<T> GetListOf<T>(SqlCommand cmd) where T : class, new()
        {
            using var sqlConnection = _connectionFactory.CreateConnection();
            if (_options.EnableStatistics)
                sqlConnection.StatisticsEnabled = true;
            sqlConnection.RetryLogicProvider = _sqlRetryProvider;
            sqlConnection.Open();
            cmd.Connection = sqlConnection;
            return GetListOf<T>(cmd, sqlConnection);
        }

        public List<T> GetListOf<T>(string query, CommandType commandType) where T : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return GetListOf<T>(sqlCommand);
        }

        public List<T> GetListOf<T>(string query, CommandType commandType, params SqlParameter[] sqlParameters) where T : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.AttachParameters(sqlParameters);
            return GetListOf<T>(sqlCommand);
        }

        #endregion

        #region GetScalar
        public object GetScalar(SqlCommand cmd)
        {
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                using var sqlTransaction = sqlConnection.BeginTransaction();
                cmd.Connection = sqlConnection;
                cmd.Transaction = sqlTransaction;
                var obj = cmd.ExecuteScalar();
                sqlTransaction.Commit();
                LogSqlInfo(cmd, sqlConnection);
                return obj;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
        }

        public object GetScalar(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            if (_options.EnableStatistics)
                connection.StatisticsEnabled = true;
            object result;
            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.Close();
                    connection.Open();
                }
                using (var sqlTransaction = connection.BeginTransaction())
                {
                    cmd.Transaction = sqlTransaction;
                    result = cmd.ExecuteScalar();
                    sqlTransaction.Commit();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return result;
        }

        public object GetScalar(string query, CommandType commandType)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            return GetScalar(sqlCommand);
        }

        public object GetScalar(string query, CommandType commandType, Array sqlParameters)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.AttachParameters(sqlParameters);
            return GetScalar(sqlCommand);
        }

        public object GetScalar(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.AttachParameters(sqlParameters);
            return GetScalar(sqlCommand);
        }
        #endregion

        #region ExecuteNonQuery
        public int ExecuteNonQuery(SqlCommand cmd)
        {
            int result;
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                sqlConnection.Open();
                using var sqlTransaction = sqlConnection.BeginTransaction();
                cmd.Connection = sqlConnection;
                cmd.Transaction = sqlTransaction;
                int num = cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                result = num;
                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            return result;
        }

        public int ExecuteNonQuery(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            int result;
            try
            {
                cmd.Connection = connection;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;

                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.Close();
                    connection.Open();
                }
                using (var sqlTransaction = connection.BeginTransaction())
                {
                    cmd.Transaction = sqlTransaction;
                    result = cmd.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return result;
        }

        public int ExecuteNonQuery(string query, CommandType commandType)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            return ExecuteNonQuery(sqlCommand);
        }

        public int ExecuteNonQuery(string query, CommandType commandType, Array sqlParameters)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.AttachParameters(sqlParameters);
            return ExecuteNonQuery(sqlCommand);
        }
        public int ExecuteNonQuery(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.AttachParameters(sqlParameters);
            return ExecuteNonQuery(sqlCommand);
        }
        #endregion

        #region ExecuteReader
        public SqlDataReader ExecuteReader(string query, CommandType commandType)
        {
            SqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                connection.Open();
                var cmd = _connectionFactory.CreateCommand();
                cmd.Connection = connection;
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _options.DbCommandTimeout;
                cmd.RetryLogicProvider = _sqlRetryProvider;
                var reader = cmd.ExecuteReader();
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                connection?.Close();
                throw;
            }
        }

        public SqlDataReader ExecuteReader(string query, CommandType commandType, CommandBehavior commandBehavior)
        {
            SqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                connection.Open();
                var cmd = _connectionFactory.CreateCommand();
                cmd.Connection = connection;
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _options.DbCommandTimeout;
                cmd.RetryLogicProvider = _sqlRetryProvider;
                var reader = cmd.ExecuteReader(commandBehavior);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                connection?.Close();
                throw;
            }
        }

        public SqlDataReader ExecuteReader(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            SqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                connection.Open();

                return ExecuteReader(connection, null, commandType, query, sqlParameters);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                connection?.Close();
                throw;
            }
        }

        public SqlDataReader ExecuteReader(string query, CommandType commandType, CommandBehavior commandBehavior, params SqlParameter[] sqlParameters)
        {
            SqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                connection.Open();

                return ExecuteReader(connection, null, commandType, query, commandBehavior, sqlParameters);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                connection?.Close();
                throw;
            }
        }

        public SqlDataReader ExecuteReader(SqlCommand cmd)
        {
            SqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                connection.Open();
                var reader = cmd.ExecuteReader();
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.Close();
                throw;
            }
        }

        public SqlDataReader ExecuteReader(SqlCommand cmd, CommandBehavior commandBehavior)
        {
            SqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                connection.Open();
                var reader = cmd.ExecuteReader(commandBehavior);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.Close();
                throw;
            }
        }

        public SqlDataReader ExecuteReader(SqlCommand cmd, SqlConnection connection)
        {
            try
            {
                cmd.Connection = connection;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;

                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.Close();
                    connection.Open();
                }
                var reader = cmd.ExecuteReader();
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.Close();
                throw;
            }
        }

        public SqlDataReader ExecuteReader(SqlCommand cmd, SqlConnection connection, CommandBehavior commandBehavior)
        {
            try
            {
                cmd.Connection = connection;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;

                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.Close();
                    connection.Open();
                }
                var reader = cmd.ExecuteReader(commandBehavior);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.Close();
                throw;
            }
        }

        public SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction? transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            var cmd = _connectionFactory.CreateCommand();
            cmd.CommandTimeout = _options.DbCommandTimeout;
            cmd.RetryLogicProvider = _sqlRetryProvider;
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            cmd.Connection = connection;
            cmd.AttachParameters(commandParameters);
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));
                cmd.Transaction = transaction;
            }

            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.Close();
                    connection.Open();
                }
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;

                // Create a reader
                var reader = cmd.ExecuteReader();
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(commandText, ex);
                connection?.Close();
                throw;
            }
        }

        public SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction? transaction, CommandType commandType, string commandText, CommandBehavior commandBehavior, params SqlParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            var cmd = _connectionFactory.CreateCommand();
            cmd.CommandTimeout = _options.DbCommandTimeout;
            cmd.RetryLogicProvider = _sqlRetryProvider;
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            cmd.Connection = connection;
            cmd.AttachParameters(commandParameters);
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));
                cmd.Transaction = transaction;
            }

            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.Close();
                    connection.Open();
                }
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;

                // Create a reader
                var reader = cmd.ExecuteReader(commandBehavior);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(commandText, ex);
                connection?.Close();
                throw;
            }
        }
        #endregion

    }
}
