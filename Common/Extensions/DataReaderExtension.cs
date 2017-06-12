using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.CarChannel.Common.Extensions
{
    public static class DataReaderExtension
    {
        public static HashSet<string> GetColums(this IDataReader row)
        {
            HashSet<string> columns = new HashSet<string>();
            for (int i = 0; i < row.FieldCount; i++)
            {
                columns.Add(row.GetName(i));
            }

            return columns;
        }

        public static T GetValueOrDefault<T>(this IDataReader row, string fieldName, HashSet<string> columns = null)
        {
            if (columns == null)
            {
                columns = row.GetColums();
            }

            if (!columns.Contains(fieldName))
            {
                return default(T);
            }

            int ordinal = row.GetOrdinal(fieldName);
            return row.GetValueOrDefault<T>(ordinal);
        }

        public static T GetValueOrDefault<T>(this IDataRecord row, int ordinal)
        {
            return (T)(row.IsDBNull(ordinal) ? default(T) : row.GetValue(ordinal));
        }
    }
}
