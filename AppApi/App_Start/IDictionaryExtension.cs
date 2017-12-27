using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppApi.Car.Extensions
{
    public static class IDictionaryExtension
    {
        //safe dic value getter
        public static T Get<T>(this IDictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key) && dic[key] != null && dic[key] is T)
            {
                return (T)dic[key];
            }
            return default(T);
        }

        /// <summary>
        /// 安全返回字典值
        /// </summary>
        /// <typeparam name="TKey">Key/typeparam>
        /// <typeparam name="TValue">Value</typeparam>
        /// <param name="dictionary">当前字典</param>
        /// <param name="key">字典键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字典值</returns>
        public static TValue GetValueOrDefault<TKey, TValue>
        (this IDictionary<TKey, TValue> dictionary,
         TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }

        /// <summary>
        /// 安全返回字典值
        /// </summary>
        /// <typeparam name="TKey">Key/typeparam>
        /// <typeparam name="TValue">Value</typeparam>
        /// <param name="dictionary">当前字典</param>
        /// <param name="key">字典键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字典值</returns>
        public static TValue GetValueOrDefault<TKey, TValue>
        (this IDictionary<TKey, TValue> dictionary,
         TKey key,
         TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        /// <summary>
        /// 安全返回字典值
        /// </summary>
        /// <typeparam name="TKey">Key/typeparam>
        /// <typeparam name="TValue">Value</typeparam>
        /// <param name="dictionary">当前字典</param>
        /// <param name="key">字典键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字典值</returns>
        public static TValue GetValueOrDefault<TKey, TValue>
            (this IDictionary<TKey, TValue> dictionary,
             TKey key,
             Func<TValue> defaultValueProvider)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value
                 : defaultValueProvider();
        }






        #region Dictionary
         //safe dic value getter
        public static T Get<T>(this Dictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key) && dic[key] != null && dic[key] is T)
            {
                return (T)dic[key];
            }
            return default(T);
        }

        /// <summary>
        /// 安全返回字典值
        /// </summary>
        /// <typeparam name="TKey">Key/typeparam>
        /// <typeparam name="TValue">Value</typeparam>
        /// <param name="dictionary">当前字典</param>
        /// <param name="key">字典键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字典值</returns>
        public static TValue GetValueOrDefault<TKey, TValue>
        (this Dictionary<TKey, TValue> dictionary,
         TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }

        /// <summary>
        /// 安全返回字典值
        /// </summary>
        /// <typeparam name="TKey">Key/typeparam>
        /// <typeparam name="TValue">Value</typeparam>
        /// <param name="dictionary">当前字典</param>
        /// <param name="key">字典键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字典值</returns>
        public static TValue GetValueOrDefault<TKey, TValue>
        (this Dictionary<TKey, TValue> dictionary,
         TKey key,
         TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        /// <summary>
        /// 安全返回字典值
        /// </summary>
        /// <typeparam name="TKey">Key/typeparam>
        /// <typeparam name="TValue">Value</typeparam>
        /// <param name="dictionary">当前字典</param>
        /// <param name="key">字典键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字典值</returns>
        public static TValue GetValueOrDefault<TKey, TValue>
            (this Dictionary<TKey, TValue> dictionary,
             TKey key,
             Func<TValue> defaultValueProvider)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value
                 : defaultValueProvider();
        }
        #endregion
    }
}
