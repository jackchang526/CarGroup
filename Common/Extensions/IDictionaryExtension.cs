using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Common.Extensions
{
    public static class IDictionaryExtension
    {
        public static T Get<T>(this IDictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key) && dic[key] is T)
            {
                return (T)dic[key];
            }
            return default(T);
        }
    }
}
