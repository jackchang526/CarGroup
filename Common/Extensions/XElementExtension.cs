using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;

namespace BitAuto.CarChannel.Common.Extensions
{
    public static class XElementExtension
    {
        public static T Value<T>(this XElement element, params T[] defVal)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            if (element != null && converter.IsValid(element.Value))
            {
                return (T)converter.ConvertFromString(element.Value);
            }
            return defVal == null ? default(T) : defVal.FirstOrDefault();
        }
    }

}
