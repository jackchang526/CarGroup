using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;

namespace BitAuto.CarChannel.Common.Extensions
{
    public static class XAttributeExtension
    {
        public static T Value<T>(this XAttribute attr, params T[] defVal)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            if (attr != null && converter.IsValid(attr.Value))
            {
                return (T)converter.ConvertFromString(attr.Value);
            }
            return defVal == null ? default(T) : defVal.FirstOrDefault();
        }

        public static string Value(this XAttribute attr, string defVal = null)
        {
            if (attr == null) return null;
            return attr.Value;
        }
    }
}
