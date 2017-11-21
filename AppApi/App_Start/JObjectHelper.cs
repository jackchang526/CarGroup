using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppApi.App
{
    public class JObjectHelper
    {
        /// <summary>
        /// 获取JObject的指定键int值
        /// </summary>
        /// <param name="jdata"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetIntValue(JObject jdata, string key)
        {
            if (jdata.Property(key) == null) return -1;

            var value = jdata[key];
            int result;
            if (!int.TryParse(value.ToString(), out result))
            {
                return -1;
            }
            else return result;
        }

        /// <summary>
        /// 获取JObject的指定键的string值
        /// </summary>
        /// <param name="jdata"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringValue(JObject jdata, string key)
        {
            if (jdata.Property(key) == null) return string.Empty;
            return jdata[key].ToString();
        }

        /// <summary>
        /// 获取JObject的指定键的DateTime值
        /// </summary>
        /// <param name="jdata"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeValue(JObject jdata, string key)
        {
            if (jdata.Property(key) == null) return default(DateTime);

            var value = jdata[key];
            DateTime result;
            if (!DateTime.TryParse(value.ToString(), out result))
            {
                return default(DateTime);
            }
            else return result;
        }
        /// <summary>
        /// 安全的JObject转换
        /// </summary>
        /// <param name="jsonString">JSON字符串</param>
        /// <returns>返回JObject对象</returns>
        public static JObject TryParse(string jsonString)
        {
            try
            {
                return JObject.Parse(jsonString);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 安全的JObject转换
        /// </summary>
        /// <param name="jsonString">JSON字符串</param>
        /// <returns>返回JObject对象</returns>
        public static JArray TryParseArray(string jsonString)
        {
            try
            {
                return JArray.Parse(jsonString);
            }
            catch
            {
                return null;
            }
        }
    }
}