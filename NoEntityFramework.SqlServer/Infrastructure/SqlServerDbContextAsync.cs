using Microsoft.Data.SqlClient;
using NoEntityFramework.DataManipulators;
using NoEntityFramework.Exceptions;
using NoEntityFramework.SqlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NoEntityFramework
{
    public partial class SqlServerDbContext : ISqlServerDbContext
    {
        #region GetColumnToString
        public async Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, int columnIndex = 0)
        {
            var datatable = await GetDataTableAsync(cmd);
            return DataTableHelper.DataTableToListString(datatable, columnIndex);
        }

        public async Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            var datatable = await GetDataTableAsync(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToListString(datatable, columnIndex);
        }

        public async Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, string columnName)
        {
            var datatable = await GetDataTableAsync(cmd);
            return DataTableHelper.DataTableToListString(datatable, columnName);
        }

        public async Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            var datatable = await GetDataTableAsync(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToListString(datatable, columnName);
        }
        #endregion

        #region GetDataTable
        public async Task<DataTable> GetDataTableAsync(SqlCommand cmd)
        {
            var dataTable = new DataTable();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                cmd.Connection = sqlConnection;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                using (var sqlDataAdapter = _connectionFactory.CreateDataAdapter())
                {
                    sqlDataAdapter.SelectCommand = cmd;
                    sqlDataAdapter.Fill(dataTable);
                }
                LogSqlInfo(cmd, sqlConnection);

            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return dataTable;
        }

        public async Task<DataTable> GetDataTableAsync(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            try
            {
                var dataTable = new DataTable();
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseAsync();
                    await connection.OpenAsync();
                }
                using (var sqlTransaction = connection.BeginTransaction())
                {
                    using var sqlDataAdapter = _connectionFactory.CreateDataAdapter();
                    cmd.Transaction = sqlTransaction;
                    sqlDataAdapter.SelectCommand = cmd;
                    sqlDataAdapter.Fill(dataTable);
                    await sqlTransaction.CommitAsync();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    await connection.CloseAsync();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
        }

        public async Task<DataTable> GetDataTableAsync(string query, CommandType commandType)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            return await GetDataTableAsync(sqlCommand);
        }

        public async Task<DataTable> GetDataTableAsync(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.AttachParameters(sqlParameters);
            return await GetDataTableAsync(sqlCommand);
        }

        public async Task<List<T>> GetDataTableAsync<T>(SqlCommand cmd) where T : class, new()
        {
            var datatable = await GetDataTableAsync(cmd);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public async Task<List<T>> GetDataTableAsync<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            var datatable = await GetDataTableAsync(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public async Task<List<T>> GetDataTableAsync<T>(string query, CommandType commandType) where T : class, new()
        {
            var datatable = await GetDataTableAsync(query, commandType);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public async Task<List<T>> GetDataTableAsync<T>(string query, CommandType commandType, params SqlParameter[] sqlParameters) where T : class, new()
        {
            var datatable = await GetDataTableAsync(query, commandType, sqlParameters);
            return DataTableHelper.DataTableToList<T>(datatable);
        }
        #endregion

        #region GetDataSet
        public async Task<DataSet> GetDataSetAsync(SqlCommand cmd)
        {
            var dataSet = new DataSet();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                cmd.Connection = sqlConnection;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                using (var sqlDataAdapter = _connectionFactory.CreateDataAdapter())
                {
                    sqlDataAdapter.SelectCommand = cmd;
                    sqlDataAdapter.Fill(dataSet);
                }
                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return dataSet;
        }

        public async Task<DataSet> GetDataSetAsync(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            try
            {
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;

                var dataSet = new DataSet();
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseAsync();
                    await connection.OpenAsync();
                }
                using (var sqlTransaction = connection.BeginTransaction())
                {
                    using var sqlDataAdapter = _connectionFactory.CreateDataAdapter();
                    cmd.Transaction = sqlTransaction;
                    sqlDataAdapter.SelectCommand = cmd;
                    sqlDataAdapter.Fill(dataSet);
                    await sqlTransaction.CommitAsync();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    await connection.CloseAsync();
                }
                return dataSet;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
        }

        public async Task<DataSet> GetDataSetAsync(string query, CommandType commandType)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            return await GetDataSetAsync(sqlCommand);
        }

        public async Task<DataSet> GetDataSetAsync(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.AttachParameters(sqlParameters);
            return await GetDataSetAsync(sqlCommand);
        }
        #endregion

        #region GetDataRow
        public async Task<T> GetDataRowAsync<T>(SqlCommand cmd) where T : class, new()
        {
            var datatable = await GetDataTableAsync(cmd);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public async Task<T> GetDataRowAsync<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            var datatable = await GetDataTableAsync(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public async Task<T> GetDataRowAsync<T>(string query, CommandType commandType) where T : class, new()
        {
            var datatable = await GetDataTableAsync(query, commandType);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public async Task<T> GetDataRowAsync<T>(string query, CommandType commandType, params SqlParameter[] sqlParameters) where T : class, new()
        {
            var datatable = await GetDataTableAsync(query, commandType, sqlParameters);
            return DataTableHelper.DataRowToT<T>(datatable);
        }
        #endregion

        #region GetDictionary
        public async Task<Dictionary<T, U>> GetDictionaryAsync<T, U>(string query, CommandType commandType)
        {
            return await GetDictionaryAsync<T, U>(query, commandType, 0, 1);
        }

        public async Task<Dictionary<T, U>> GetDictionaryAsync<T, U>(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            return await GetDictionaryAsync<T, U>(query, commandType, 0, 1, sqlParameters);
        }

        public async Task<Dictionary<T, U>> GetDictionaryAsync<T, U>(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex)
        {
            var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandText = query;
            return await GetDictionaryAsync<T, U>(sqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public async Task<Dictionary<T, U>> GetDictionaryAsync<T, U>(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex, params SqlParameter[] sqlParameters)
        {
            var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandText = query;
            sqlCommand.AttachParameters(sqlParameters);
            return await GetDictionaryAsync<T, U>(sqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public async Task<Dictionary<T, U>> GetDictionaryAsync<T, U>(SqlCommand cmd)
        {
            return await GetDictionaryAsync<T, U>(cmd, 0, 1);
        }

        public async Task<Dictionary<T, U>> GetDictionaryAsync<T, U>(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                using (var sqlDataReader = await cmd.ExecuteReaderAsync())
                {
                    if (sqlDataReader.FieldCount < 2 &&
                        !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                        throw new DatabaseException("Query did not return at least two columns of data.");
                    while (await sqlDataReader.ReadAsync())
                    {
                        dictionary.Add((T)sqlDataReader[keyColumnIndex], (U)sqlDataReader[valueColumnIndex]);
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

        public async Task<Dictionary<string, string>> GetDictionaryAsync(string query, CommandType commandType)
        {
            return await GetDictionaryAsync(query, commandType, 0, 1);
        }

        public async Task<Dictionary<string, string>> GetDictionaryAsync(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            return await GetDictionaryAsync(query, commandType, 0, 1, sqlParameters);
        }

        public async Task<Dictionary<string, string>> GetDictionaryAsync(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex)
        {
            var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandText = query;
            return await GetDictionaryAsync(sqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public async Task<Dictionary<string, string>> GetDictionaryAsync(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex, params SqlParameter[] sqlParameters)
        {
            var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandText = query;
            sqlCommand.AttachParameters(sqlParameters);
            return await GetDictionaryAsync(sqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public async Task<Dictionary<string, string>> GetDictionaryAsync(SqlCommand cmd)
        {
            return await GetDictionaryAsync(cmd, 0, 1);
        }

        public async Task<Dictionary<string, string>> GetDictionaryAsync(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                using (var sqlDataReader = await cmd.ExecuteReaderAsync())
                {
                    if (sqlDataReader.FieldCount < 2 &&
                        !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                        throw new DatabaseException("Query did not return at least two columns of data.");
                    while (await sqlDataReader.ReadAsync())
                    {
                        dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
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
        #endregion

        #region GetDictionaryOfObjects
        public async Task<Dictionary<T, U>> GetDictionaryOfObjectsAsync<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();

                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                using (var sqlDataReader = await cmd.ExecuteReaderAsync())
                {
                    var typeFromHandle = typeof(U);
                    var list = ModelCache.GetProperties(typeFromHandle);
                    while (await sqlDataReader.ReadAsync())
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

        public async Task<Dictionary<T, U>> GetDictionaryOfObjectsAsync<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                using (var sqlDataReader = await cmd.ExecuteReaderAsync())
                {
                    var typeFromHandle = typeof(U);
                    var list = ModelCache.GetProperties(typeFromHandle);
                    while (await sqlDataReader.ReadAsync())
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

        public async Task<Dictionary<T, List<U>>> GetDictionaryOfListObjectsAsync<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                using (var sqlDataReader = await cmd.ExecuteReaderAsync())
                {
                    var typeFromHandle = typeof(U);
                    var list = ModelCache.GetProperties(typeFromHandle);
                    while (await sqlDataReader.ReadAsync())
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

        public async Task<Dictionary<T, List<U>>> GetDictionaryOfListObjectsAsync<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                using (var sqlDataReader = await cmd.ExecuteReaderAsync())
                {
                    var typeFromHandle = typeof(U);
                    var list = ModelCache.GetProperties(typeFromHandle);
                    while (await sqlDataReader.ReadAsync())
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

        public async Task<Dictionary<T, U>> GetDictionaryOfObjectsAsync<T, U>(string query, CommandType commandType, int keyColumnIndex) where U : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return await GetDictionaryOfObjectsAsync<T, U>(sqlCommand, keyColumnIndex);
        }

        public async Task<Dictionary<T, U>> GetDictionaryOfObjectsAsync<T, U>(string query, CommandType commandType, string keyColumnName) where U : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return await GetDictionaryOfObjectsAsync<T, U>(sqlCommand, keyColumnName);
        }

        public async Task<Dictionary<T, List<U>>> GetDictionaryOfListObjectsAsync<T, U>(string query, CommandType commandType, int keyColumnIndex) where U : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return await GetDictionaryOfListObjectsAsync<T, U>(sqlCommand, keyColumnIndex);
        }

        public async Task<Dictionary<T, List<U>>> GetDictionaryOfListObjectsAsync<T, U>(string query, CommandType commandType, string keyColumnName) where U : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return await GetDictionaryOfListObjectsAsync<T, U>(sqlCommand, keyColumnName);
        }
        #endregion

        #region GetListListString
        public async Task<List<List<string>>> GetListListStringAsync(SqlCommand cmd)
        {
            var list = new List<List<string>>();
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                using (var sqlDataReader = await cmd.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        var list2 = new List<string>();
                        for (int i = 0; i < sqlDataReader.FieldCount; i++)
                        {
                            list2.Add(sqlDataReader[i].ToString());
                        }
                        list.Add(list2);
                    }
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

        public async Task<List<List<string>>> GetListListStringAsync(SqlCommand cmd, string dateFormat)
        {
            var list = new List<List<string>>();
            bool flag = true;
            bool[] array = null;
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                using (var sqlDataReader = await cmd.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
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

        public async Task<List<List<string>>> GetListListStringAsync(string query, CommandType commandType)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return await GetListListStringAsync(sqlCommand);
        }

        public async Task<List<List<string>>> GetListListStringAsync(string query, CommandType commandType, string dateFormat)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return await GetListListStringAsync(sqlCommand, dateFormat);
        }
        #endregion

        #region GetListOf<T>
        public async Task<List<T>> GetListOfAsync<T>(SqlCommand cmd, SqlConnection connection) where T : class, new()
        {
            if (_options.EnableStatistics)
                connection.StatisticsEnabled = true;
            var list = new List<T>();
            var type = typeof(T);
            cmd.Connection = connection;
            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseAsync();
                    await connection.OpenAsync();
                }
                using var sqlDataReader = await cmd.ExecuteReaderAsync();
                var list2 = ModelCache.GetProperties(type);
                while (await sqlDataReader.ReadAsync())
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

        public async Task<List<T>> GetListOfAsync<T>(SqlCommand cmd) where T : class, new()
        {
            using var sqlConnection = _connectionFactory.CreateConnection();
            if (_options.EnableStatistics)
                sqlConnection.StatisticsEnabled = true;
            sqlConnection.RetryLogicProvider = _sqlRetryProvider;
            await sqlConnection.OpenAsync();
            cmd.Connection = sqlConnection;
            return await GetListOfAsync<T>(cmd, sqlConnection);
        }

        public async Task<List<T>> GetListOfAsync<T>(string query, CommandType commandType) where T : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return await GetListOfAsync<T>(sqlCommand);
        }

        public async Task<List<T>> GetListOfAsync<T>(string query, CommandType commandType, params SqlParameter[] sqlParameters) where T : class, new()
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.AttachParameters(sqlParameters);
            return await GetListOfAsync<T>(sqlCommand);
        }

        #endregion

        #region GetScalar
        public async Task<object> GetScalarAsync(SqlCommand cmd)
        {
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = sqlTransaction;
                    var obj = await cmd.ExecuteScalarAsync();
                    await sqlTransaction.CommitAsync();
                    LogSqlInfo(cmd, sqlConnection);
                    return obj;
                }
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
        }

        public async Task<object> GetScalarAsync(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            if (_options.EnableStatistics)
                connection.StatisticsEnabled = true;
            cmd.Connection = connection;
            object result;
            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseAsync();
                    await connection.OpenAsync();
                }
                using (var sqlTransaction = connection.BeginTransaction())
                {
                    cmd.Transaction = sqlTransaction;
                    result = await cmd.ExecuteScalarAsync();
                    await sqlTransaction.CommitAsync();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return result;
        }

        public async Task<object> GetScalarAsync(string query, CommandType commandType)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            return await GetScalarAsync(sqlCommand);
        }

        public async Task<object> GetScalarAsync(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.AttachParameters(sqlParameters);
            return await GetScalarAsync(sqlCommand);
        }
        #endregion

        #region ExecuteNonQuery
        public async Task<int> ExecuteNonQueryAsync(SqlCommand cmd)
        {
            int result;
            try
            {
                using var sqlConnection = _connectionFactory.CreateConnection();
                if (_options.EnableStatistics)
                    sqlConnection.StatisticsEnabled = true;
                sqlConnection.RetryLogicProvider = _sqlRetryProvider;
                await sqlConnection.OpenAsync();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = sqlTransaction;
                    int num = await cmd.ExecuteNonQueryAsync();
                    await sqlTransaction.CommitAsync();
                    result = num;
                }
                LogSqlInfo(cmd, sqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            return result;
        }

        public async Task<int> ExecuteNonQueryAsync(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            int result;
            try
            {
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseAsync();
                    await connection.OpenAsync();
                }
                using (var sqlTransaction = connection.BeginTransaction())
                {
                    cmd.Transaction = sqlTransaction;
                    result = await cmd.ExecuteNonQueryAsync();
                    await sqlTransaction.CommitAsync();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return result;
        }

        public async Task<int> ExecuteNonQueryAsync(string query, CommandType commandType)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            return await ExecuteNonQueryAsync(sqlCommand);
        }

        public async Task<int> ExecuteNonQueryAsync(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using var sqlCommand = _connectionFactory.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = _options.DbCommandTimeout;
            sqlCommand.RetryLogicProvider = _sqlRetryProvider;
            sqlCommand.AttachParameters(sqlParameters);
            return await ExecuteNonQueryAsync(sqlCommand);
        }
        #endregion

        #region ExecuteReader
        public async Task<SqlDataReader> ExecuteReaderAsync(string query, CommandType commandType)
        {
            SqlConnection connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                await connection.OpenAsync();
                var cmd = _connectionFactory.CreateCommand();
                cmd.Connection = connection;
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _options.DbCommandTimeout;
                cmd.RetryLogicProvider = _sqlRetryProvider;
                var reader = await cmd.ExecuteReaderAsync();
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                await connection.CloseAsync();
                throw;
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string query, CommandType commandType, CommandBehavior commandBehavior)
        {
            SqlConnection connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                await connection.OpenAsync();
                var cmd = _connectionFactory.CreateCommand();
                cmd.Connection = connection;
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _options.DbCommandTimeout;
                cmd.RetryLogicProvider = _sqlRetryProvider;
                var reader = await cmd.ExecuteReaderAsync(commandBehavior);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                await connection.CloseAsync();
                throw;
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string query, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            SqlConnection connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                await connection.OpenAsync();

                return await ExecuteReaderAsync(connection, null, commandType, query, sqlParameters);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                await connection.CloseAsync();
                throw;
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string query, CommandType commandType, CommandBehavior commandBehavior, params SqlParameter[] sqlParameters)
        {
            SqlConnection connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                await connection.OpenAsync();

                return await ExecuteReaderAsync(connection, null, commandType, query, commandBehavior, sqlParameters);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                await connection.CloseAsync();
                throw;
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(SqlCommand cmd)
        {
            SqlConnection connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                await connection.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                await connection.CloseAsync();
                throw;
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(SqlCommand cmd, CommandBehavior commandBehavior)
        {
            SqlConnection connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.RetryLogicProvider = _sqlRetryProvider;
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                await connection.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync(commandBehavior);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                await connection.CloseAsync();
                throw;
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(SqlCommand cmd, SqlConnection connection)
        {
            try
            {
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseAsync();
                    await connection.OpenAsync();
                }
                var reader = await cmd.ExecuteReaderAsync();
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                await connection.CloseAsync();
                throw;
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(SqlCommand cmd, SqlConnection connection, CommandBehavior commandBehavior)
        {
            try
            {
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseAsync();
                    await connection.OpenAsync();
                }
                var reader = await cmd.ExecuteReaderAsync(commandBehavior);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                await connection.CloseAsync();
                throw;
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(SqlConnection connection, SqlTransaction? transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
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
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseAsync();
                    await connection.OpenAsync();
                }

                // Create a reader
                var reader = await cmd.ExecuteReaderAsync();
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(commandText, ex);
                await connection.CloseAsync();
                throw;
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(SqlConnection connection, SqlTransaction? transaction, CommandType commandType, string commandText, CommandBehavior commandBehavior, params SqlParameter[] commandParameters)
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
                if (_options.EnableStatistics)
                    connection.StatisticsEnabled = true;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseAsync();
                    await connection.OpenAsync();
                }

                // Create a reader
                var reader = await cmd.ExecuteReaderAsync(commandBehavior);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(commandText, ex);
                await connection.CloseAsync();
                throw;
            }
        }
        #endregion

    }
}
