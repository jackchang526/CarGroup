using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.CarChannel.Common.Extensions
{
    public static class DataRowExtension
    {
        public static HashSet<string> GetColums(this DataRow row)
        {
            HashSet<string> columns = new HashSet<string>();
            foreach(DataColumn column in row.Table.Columns)
            {
                columns.Add(column.ColumnName);
            }

            return columns;
        }

        public static T GetValueOrDefault<T>(this DataRow row, string fieldName, HashSet<string> columns = null)
        {
            if (columns == null)
            {
                columns = row.GetColums();
            }

            if (!columns.Contains(fieldName))
            {
                return default(T);
            }

            return row.Field<T>(fieldName);
        }

        public static T GetValueOrDefault<T>(this DataRow row, int ordinal)
        {
            return (T)(row.IsNull(ordinal) ? default(T) : row.Field<T>(ordinal));
        }
    }
}
