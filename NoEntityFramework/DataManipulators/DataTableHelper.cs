using Newtonsoft.Json;
using NoEntityFramework.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace NoEntityFramework.DataManipulators
{
    public class DataTableHelper
    {
        public static string[][] DataTableTo2dArray(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            var array = new string[dt.Rows.Count][];
            var num = 0;
            foreach (DataRow row in dt.Rows)
            {
                array[num] = new string[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    array[num][i] = row[i].ToString();
                }

                num++;
            }

            return array;
        }

        public static string[][] DataTableTo2dArray(DataTable dt, string dateFormat)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            var array = new string[dt.Rows.Count][];
            var array2 = new bool[dt.Columns.Count];
            var num = 0;
            foreach (DataRow row in dt.Rows)
            {
                array[num] = new string[dt.Columns.Count];
                if (num == 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (row[i].GetType() == typeof(DateTime))
                        {
                            array2[i] = true;
                            array[num][i] = ((DateTime)row[i]).ToString(dateFormat);
                        }
                        else
                        {
                            array[num][i] = row[i].ToString();
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (array2[j])
                        {
                            array[num][j] = ((DateTime)row[j]).ToString(dateFormat);
                        }
                        else
                        {
                            array[num][j] = row[j].ToString();
                        }
                    }
                }

                num++;
            }

            return array;
        }

        public static string[][] DataTableTo2dArray(DataTable dt, IEnumerable<int> keepOnlyColumns)
        {
            return DataTableTo2dArray(dt, keepOnlyColumns, removeDuplicates: false);
        }

        public static string[][] DataTableTo2dArray(DataTable dt, IEnumerable<int> keepOnlyColumns, bool removeDuplicates)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            var array = new string[dt.Rows.Count][];
            var num = 0;
            int num2;
            if (removeDuplicates)
            {
                OrderedSet<int> orderedSet = new OrderedSet<int>(keepOnlyColumns);
                foreach (DataRow row in dt.Rows)
                {
                    array[num] = new string[orderedSet.Count()];
                    num2 = 0;
                    foreach (int item in orderedSet)
                    {
                        array[num][num2] = row[item].ToString();
                        num2++;
                    }

                    num++;
                }
            }
            else
            {
                foreach (DataRow row2 in dt.Rows)
                {
                    array[num] = new string[keepOnlyColumns.Count()];
                    num2 = 0;
                    foreach (int keepOnlyColumn in keepOnlyColumns)
                    {
                        array[num][num2] = row2[keepOnlyColumn].ToString();
                        num2++;
                    }

                    num++;
                }
            }

            return array;
        }

        public static string[][] DataTableTo2dArray(DataTable dt, string dateFormat, IEnumerable<int> keepOnlyColumns)
        {
            return DataTableTo2dArray(dt, dateFormat, keepOnlyColumns, removeDuplicates: false);
        }

        public static string[][] DataTableTo2dArray(DataTable dt, string dateFormat, IEnumerable<int> keepOnlyColumns, bool removeDuplicates)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            var array = new string[dt.Rows.Count][];
            int num = 0;
            int num2;
            if (removeDuplicates)
            {
                OrderedSet<int> orderedSet = new OrderedSet<int>(keepOnlyColumns);
                bool[] array2 = new bool[orderedSet.Count];
                foreach (DataRow row in dt.Rows)
                {
                    array[num] = new string[orderedSet.Count];
                    num2 = 0;
                    if (num == 0)
                    {
                        foreach (int item in orderedSet)
                        {
                            if (row[item].GetType() == typeof(DateTime))
                            {
                                array2[num2] = true;
                                array[num][num2] = ((DateTime)row[item]).ToString(dateFormat);
                            }
                            else
                            {
                                array[num][num2] = row[item].ToString();
                            }

                            num2++;
                        }
                    }
                    else
                    {
                        foreach (int item2 in orderedSet)
                        {
                            if (array2[num2])
                            {
                                array[num][num2] = ((DateTime)row[item2]).ToString(dateFormat);
                            }
                            else
                            {
                                array[num][num2] = row[item2].ToString();
                            }

                            num2++;
                        }
                    }

                    num++;
                }
            }
            else
            {
                bool[] array3 = new bool[keepOnlyColumns.Count()];
                foreach (DataRow row2 in dt.Rows)
                {
                    array[num] = new string[keepOnlyColumns.Count()];
                    num2 = 0;
                    if (num == 0)
                    {
                        foreach (int keepOnlyColumn in keepOnlyColumns)
                        {
                            if (row2[keepOnlyColumn].GetType() == typeof(DateTime))
                            {
                                array3[num2] = true;
                                array[num][num2] = ((DateTime)row2[keepOnlyColumn]).ToString(dateFormat);
                            }
                            else
                            {
                                array[num][num2] = row2[keepOnlyColumn].ToString();
                            }

                            num2++;
                        }
                    }
                    else
                    {
                        foreach (int keepOnlyColumn2 in keepOnlyColumns)
                        {
                            if (array3[num2])
                            {
                                array[num][num2] = ((DateTime)row2[keepOnlyColumn2]).ToString(dateFormat);
                            }
                            else
                            {
                                array[num][num2] = row2[keepOnlyColumn2].ToString();
                            }

                            num2++;
                        }
                    }

                    num++;
                }
            }

            return array;
        }

        public static string DataTableToHTMLTableString(DataTable data)
        {
            string[] array = new string[data.Rows.Count];
            string[] array2 = new string[data.Columns.Count];
            long num = 0L;
            foreach (DataColumn column in data.Columns)
            {
                array2[num++] = HttpUtility.HtmlEncode(column.ColumnName);
            }

            num = 0L;
            foreach (DataRow row in data.Rows)
            {
                array[num++] = "<tr><td>" + string.Join("</td><td>", row.ItemArray.Select((object o) => HttpUtility.HtmlEncode(o.ToString())).ToArray()) + "</td></tr>";
            }

            return "<table><thead><tr><th>" + string.Join("</th><th>", array2) + "</th></tr></thead><tbody>" + string.Join("", array) + "</tbody></table>";
        }

        public static string DataTableToHTMLTableString(DataTable data, string dateFormat)
        {
            return DataTableToHTMLTableString(data, dateFormat, null);
        }

        public static string DataTableToHTMLTableString(DataTable data, string dateFormat, string tableClass)
        {
            string[] array = new string[data.Rows.Count];
            string[] array2 = new string[data.Columns.Count];
            if (dateFormat != null)
            {
                bool[] dateTimeColumn = new bool[data.Columns.Count];
                long num = 0L;
                foreach (DataColumn column in data.Columns)
                {
                    array2[num] = HttpUtility.HtmlEncode(column.ColumnName);
                    dateTimeColumn[num] = column.DataType == typeof(DateTime);
                    num++;
                }

                num = 0L;
                foreach (DataRow row in data.Rows)
                {
                    array[num++] = "<tr><td>" + string.Join("</td><td>", row.ItemArray.Select((object o, int i) => dateTimeColumn[i] ? ((DateTime)o).ToString(dateFormat) : HttpUtility.HtmlEncode(o.ToString())).ToArray()) + "</td></tr>";
                }
            }
            else
            {
                long num2 = 0L;
                foreach (DataColumn column2 in data.Columns)
                {
                    array2[num2++] = HttpUtility.HtmlEncode(column2.ColumnName);
                }

                num2 = 0L;
                foreach (DataRow row2 in data.Rows)
                {
                    array[num2++] = "<tr><td>" + string.Join("</td><td>", row2.ItemArray.Select((object o) => HttpUtility.HtmlEncode(o.ToString())).ToArray()) + "</td></tr>";
                }
            }

            return (string.IsNullOrEmpty(tableClass) ? "<table>" : ("<table class=\"" + tableClass + "\">")) + "<thead><tr><th>" + string.Join("</th><th>", array2) + "</th></tr></thead><tbody>" + string.Join("", array) + "</tbody></table>";
        }

        public static List<List<string>> DataTableToListListString(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            var list = new List<List<string>>(dt.Rows.Count);
            foreach (DataRow row in dt.Rows)
            {
                List<string> list2 = new List<string>(dt.Columns.Count);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    list2.Add(row[i].ToString());
                }

                list.Add(list2);
            }

            return list;
        }

        public static List<List<string>> DataTableToListListString(DataTable dt, string dateFormat)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            var list = new List<List<string>>(dt.Rows.Count);
            bool[] array = new bool[dt.Columns.Count];
            int num = 0;
            foreach (DataRow row in dt.Rows)
            {
                List<string> list2 = new List<string>(dt.Columns.Count);
                if (num == 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (row[i].GetType() == typeof(DateTime))
                        {
                            array[i] = true;
                            list2.Add(((DateTime)row[i]).ToString(dateFormat));
                        }
                        else
                        {
                            list2.Add(row[i].ToString());
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (array[j])
                        {
                            list2.Add(((DateTime)row[j]).ToString(dateFormat));
                        }
                        else
                        {
                            list2.Add(row[j].ToString());
                        }
                    }
                }

                list.Add(list2);
                num++;
            }

            return list;
        }

        public static List<List<string>> DataTableToListListString(DataTable dt, IEnumerable<int> keepOnlyColumns)
        {
            return DataTableToListListString(dt, keepOnlyColumns, removeDuplicates: false);
        }

        public static List<List<string>> DataTableToListListString(DataTable dt, IEnumerable<int> keepOnlyColumns, bool removeDuplicates)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            var list = new List<List<string>>(dt.Rows.Count);
            if (removeDuplicates)
            {
                OrderedSet<int> orderedSet = new OrderedSet<int>(keepOnlyColumns);
                foreach (DataRow row in dt.Rows)
                {
                    List<string> list2 = new List<string>(orderedSet.Count());
                    foreach (int item in orderedSet)
                    {
                        list2.Add(row[item].ToString());
                    }

                    list.Add(list2);
                }
            }
            else
            {
                foreach (DataRow row2 in dt.Rows)
                {
                    List<string> list3 = new List<string>(keepOnlyColumns.Count());
                    foreach (int keepOnlyColumn in keepOnlyColumns)
                    {
                        list3.Add(row2[keepOnlyColumn].ToString());
                    }

                    list.Add(list3);
                }
            }

            return list;
        }

        public static List<List<string>> DataTableToListListString(DataTable dt, string dateFormat, IEnumerable<int> keepOnlyColumns)
        {
            return DataTableToListListString(dt, dateFormat, keepOnlyColumns, removeDuplicates: false);
        }

        public static List<List<string>> DataTableToListListString(DataTable dt, string dateFormat, IEnumerable<int> keepOnlyColumns, bool removeDuplicates)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            var list = new List<List<string>>(dt.Rows.Count);
            int num = 0;
            int num2;
            if (removeDuplicates)
            {
                OrderedSet<int> orderedSet = new OrderedSet<int>(keepOnlyColumns);
                bool[] array = new bool[orderedSet.Count];
                foreach (DataRow row in dt.Rows)
                {
                    List<string> list2 = new List<string>(orderedSet.Count());
                    num2 = 0;
                    if (num == 0)
                    {
                        foreach (int item in orderedSet)
                        {
                            if (row[item].GetType() == typeof(DateTime))
                            {
                                array[num2] = true;
                                list2.Add(((DateTime)row[item]).ToString(dateFormat));
                            }
                            else
                            {
                                list2.Add(row[item].ToString());
                            }

                            num2++;
                        }
                    }
                    else
                    {
                        foreach (int item2 in orderedSet)
                        {
                            if (array[num2])
                            {
                                list2.Add(((DateTime)row[item2]).ToString(dateFormat));
                            }
                            else
                            {
                                list2.Add(row[item2].ToString());
                            }

                            num2++;
                        }
                    }

                    list.Add(list2);
                    num++;
                }
            }
            else
            {
                bool[] array2 = new bool[keepOnlyColumns.Count()];
                foreach (DataRow row2 in dt.Rows)
                {
                    List<string> list3 = new List<string>(keepOnlyColumns.Count());
                    num2 = 0;
                    if (num == 0)
                    {
                        foreach (int keepOnlyColumn in keepOnlyColumns)
                        {
                            if (row2[keepOnlyColumn].GetType() == typeof(DateTime))
                            {
                                array2[num2] = true;
                                list3.Add(((DateTime)row2[keepOnlyColumn]).ToString(dateFormat));
                            }
                            else
                            {
                                list3.Add(row2[keepOnlyColumn].ToString());
                            }

                            num2++;
                        }
                    }
                    else
                    {
                        foreach (int keepOnlyColumn2 in keepOnlyColumns)
                        {
                            if (array2[num2])
                            {
                                list3.Add(((DateTime)row2[keepOnlyColumn2]).ToString(dateFormat));
                            }
                            else
                            {
                                list3.Add(row2[keepOnlyColumn2].ToString());
                            }

                            num2++;
                        }
                    }

                    list.Add(list3);
                    num++;
                }
            }

            return list;
        }

        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>(dt.Rows.Count);
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>(dt.Columns.Count);
                foreach (DataColumn column in dt.Columns)
                {
                    dictionary[column.ColumnName] = row[column];
                }

                list.Add(dictionary);
            }

            return list;
        }

        public static string DataTableToJSON(DataTable dt)
        {
            List<Dictionary<string, object>> list = DataTableToList(dt);
            return JsonConvert.SerializeObject((object)list);
        }

        public static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        {
            try
            {
                Type typeFromHandle = typeof(T);
                List<PropertyInfo> value;
                lock (ModelCache._outputModelCache)
                {
                    if (!ModelCache._outputModelCache.TryGetValue(typeFromHandle, out value))
                    {
                        value = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        value.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        ModelCache._outputModelCache.Add(typeFromHandle, value);
                    }
                }

                List<T> list = new List<T>(dt.Rows.Count);
                foreach (DataRow row in dt.Rows)
                {
                    T val = new T();
                    foreach (PropertyInfo item in value)
                    {
                        try
                        {
                            if (item.PropertyType.IsEnum)
                            {
                                item.SetValue(val, Enum.ToObject(item.PropertyType, (int)row[item.GetColumnName()]), null);
                            }
                            else
                            {
                                item.SetValue(val, Convert.ChangeType(row[item.GetColumnName()], Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }

                    list.Add(val);
                }

                return list;
            }
            catch
            {
                return new List<T>();
            }
        }

        public static T DataRowToT<T>(DataTable dt) where T : class, new()
        {
            try
            {
                Type typeFromHandle = typeof(T);
                List<PropertyInfo> value;
                lock (ModelCache._outputModelCache)
                {
                    if (!ModelCache._outputModelCache.TryGetValue(typeFromHandle, out value))
                    {
                        value = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        value.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        ModelCache._outputModelCache.Add(typeFromHandle, value);
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dataRow = dt.Rows[0];
                    T val = new T();
                    foreach (PropertyInfo item in value)
                    {
                        try
                        {
                            if (item.PropertyType.IsEnum)
                            {
                                item.SetValue(val, Enum.ToObject(item.PropertyType, (int)dataRow[item.GetColumnName()]), null);
                            }
                            else
                            {
                                item.SetValue(val, Convert.ChangeType(dataRow[item.GetColumnName()], Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }

                    return val;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static T DataRowToT<T>(DataTable dt, T objectToFill) where T : class, new()
        {
            try
            {
                Type typeFromHandle = typeof(T);
                List<PropertyInfo> value;
                lock (ModelCache._outputModelCache)
                {
                    if (!ModelCache._outputModelCache.TryGetValue(typeFromHandle, out value))
                    {
                        value = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        value.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        ModelCache._outputModelCache.Add(typeFromHandle, value);
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dataRow = dt.Rows[0];
                    foreach (PropertyInfo item in value)
                    {
                        object obj = dataRow[item.GetColumnName()];
                        if (obj == null)
                        {
                            continue;
                        }

                        try
                        {
                            if (item.PropertyType.IsEnum)
                            {
                                item.SetValue(objectToFill, Enum.ToObject(item.PropertyType, (int)dataRow[item.GetColumnName()]), null);
                            }
                            else
                            {
                                item.SetValue(objectToFill, Convert.ChangeType(dataRow[item.GetColumnName()], Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }

                    return objectToFill;
                }

                return objectToFill;
            }
            catch
            {
                return objectToFill;
            }
        }

        public static T DataRowToT<T>(DataRow row) where T : class, new()
        {
            try
            {
                Type typeFromHandle = typeof(T);
                List<PropertyInfo> value;
                lock (ModelCache._outputModelCache)
                {
                    if (!ModelCache._outputModelCache.TryGetValue(typeFromHandle, out value))
                    {
                        value = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        value.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        ModelCache._outputModelCache.Add(typeFromHandle, value);
                    }
                }

                if (row == null)
                {
                    return null;
                }

                T val = new T();
                foreach (PropertyInfo item in value)
                {
                    try
                    {
                        if (item.PropertyType.IsEnum)
                        {
                            item.SetValue(val, Enum.ToObject(item.PropertyType, (int)row[item.GetColumnName()]), null);
                        }
                        else
                        {
                            item.SetValue(val, Convert.ChangeType(row[item.GetColumnName()], Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType), null);
                        }
                    }
                    catch
                    {
                    }
                }

                return val;
            }
            catch
            {
                return null;
            }
        }

        public static T DataRowToT<T>(DataRow row, T objectToFill) where T : class, new()
        {
            try
            {
                Type typeFromHandle = typeof(T);
                List<PropertyInfo> value;
                lock (ModelCache._outputModelCache)
                {
                    if (!ModelCache._outputModelCache.TryGetValue(typeFromHandle, out value))
                    {
                        value = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        value.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        ModelCache._outputModelCache.Add(typeFromHandle, value);
                    }
                }

                if (row == null)
                {
                    return objectToFill;
                }

                foreach (PropertyInfo item in value)
                {
                    object obj = row[item.GetColumnName()];
                    if (obj == null)
                    {
                        continue;
                    }

                    try
                    {
                        if (item.PropertyType.IsEnum)
                        {
                            item.SetValue(objectToFill, Enum.ToObject(item.PropertyType, (int)row[item.GetColumnName()]), null);
                        }
                        else
                        {
                            item.SetValue(objectToFill, Convert.ChangeType(row[item.GetColumnName()], Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType), null);
                        }
                    }
                    catch
                    {
                    }
                }

                return objectToFill;
            }
            catch
            {
                return objectToFill;
            }
        }

        public static List<string> DataTableToListString(DataTable? dt, int columnNumber = 0)
        {
            if (columnNumber < 0 || columnNumber > dt.Columns.Count - 1)
            {
                return new List<string>();
            }

            List<string> list = new List<string>(dt.Rows.Count);
            foreach (DataRow row in dt.Rows)
            {
                if (row[columnNumber] != DBNull.Value)
                    list.Add(row[columnNumber].ToString());
                else
                    list.Add(default);
            }

            return list;
        }

        public static List<T> DataTableToListCast<T>(DataTable dt, int columnNumber = 0)
        {
            if (columnNumber < 0 || columnNumber > dt.Columns.Count - 1)
            {
                return new List<T>() ;
            }

            var list = new List<T>(dt.Rows.Count);
            foreach (DataRow row in dt.Rows)
            {
                if (row[columnNumber] != DBNull.Value)
                    list.Add((T)row[columnNumber]);
                else
                    list.Add(default);
            }

            return list;
        }

        public static List<string> DataTableToListString(DataTable dt, string columnName)
        {
            if (!dt.Columns.Contains(columnName))
            {
                return new List<string>();
            }

            List<string> list = new List<string>(dt.Rows.Count);
            foreach (DataRow row in dt.Rows)
            {
                if (row[columnName] != DBNull.Value)
                    list.Add(row[columnName].ToString());
                else
                    list.Add(string.Empty);
            }

            return list;
        }

        public static List<T> DataTableToListCast<T>(DataTable dt, string columnName)
        {
            if (!dt.Columns.Contains(columnName))
            {
                return new List<T>();
            }

            List<T> list = new List<T>(dt.Rows.Count);
            foreach (DataRow row in dt.Rows)
            {
                if (row[columnName] != DBNull.Value)
                    list.Add((T)row[columnName]);
                else
                    list.Add(default);
            }

            return list;
        }

        public static DataTable ListTToDataTable<T>(IEnumerable<T> list) where T : class
        {
            Type typeFromHandle = typeof(T);
            PropertyInfo[] properties = typeFromHandle.GetProperties();
            DataTable dataTable = new DataTable();
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType));
            }

            foreach (T item in list)
            {
                object[] array2 = new object[properties.Length];
                for (int j = 0; j < properties.Length; j++)
                {
                    array2[j] = properties[j].GetValue(item, null);
                }

                dataTable.Rows.Add(array2);
            }

            return dataTable;
        }

        public static string GetClassStringForDataTable(DataTable dt)
        {
            return GetClassStringForDataTable(dt, "MyClass");
        }

        public static string GetClassStringForDataTable(DataTable dt, string className)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("class " + className);
            stringBuilder.AppendLine("{");
            foreach (DataColumn column in dt.Columns)
            {
                stringBuilder.Append("    public ");
                switch (column.DataType.ToString())
                {
                    case "System.Int32":
                        stringBuilder.Append("int");
                        break;
                    case "System.Int16":
                        stringBuilder.Append("short");
                        break;
                    case "System.DateTime":
                        stringBuilder.Append("DateTime");
                        break;
                    case "System.String":
                        stringBuilder.Append("string");
                        break;
                    case "System.Int64":
                        stringBuilder.Append("long");
                        break;
                    case "System.Decimal":
                        stringBuilder.Append("decimal");
                        break;
                }

                stringBuilder.AppendLine(" " + column.ColumnName + " { get; set; }");
            }

            stringBuilder.AppendLine("}");
            return stringBuilder.ToString();
        }
    }
}
