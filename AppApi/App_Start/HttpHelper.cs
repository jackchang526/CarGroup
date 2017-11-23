using BitAuto.CarChannel.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AppApi.App
{
    public static class HttpHelper
    {
        /// <summary>
        /// 获取页面日志的消息
        /// </summary>
        /// <returns>消息内容</returns>
        public static string GetHttpLogMessage(HttpContext context = null, bool isIncludeCookie = true)
        {
            context = context ?? HttpContext.Current;
            if (context != null)
            {
                var request = context.Request;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("\r\nUrl:{2}\r\nStatus:{3};IP:{0};Reffer:{1}", GetClientIP(context), (request.UrlReferrer == null ? string.Empty : request.UrlReferrer.ToString()), request.Url.ToString(), context.Response.StatusCode);
                if (context.User != null && context.User.Identity != null)
                {
                    sb.Append("\r\n用户:" + (context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "匿名"));
                }
                if (request.Headers["devid"] != null)
                {
                    sb.Append("\r\n设备:" + request.Headers["devid"]);
                }
                var appVer = VersionHelper.GetAppVerString();
                if (!string.IsNullOrWhiteSpace(appVer))
                {
                    sb.Append("\r\n版本:" + appVer);
                }
                if (request.QueryString.Count > 0)
                {
                    sb.Append("\r\nGET参数:" + HttpUtility.UrlDecode(request.QueryString.ToString()));
                }
                if (context.Request.Form.Count > 0)
                {
                    sb.Append("\r\nPOST参数:" + HttpUtility.UrlDecode(request.Form.ToString()));
                }

                if (isIncludeCookie)
                {
                    var len = context.Request.Cookies.Count;
                    for (int i = 0; i < len; i++)
                    {
                        var cookie = context.Request.Cookies[i];
                        sb.AppendFormat("\r\ncookie-{0}:{1}", cookie.Name, HttpUtility.UrlDecode(cookie.Value));
                    }
                }
                sb.AppendFormat("\r\nUA:{0}", request.UserAgent);
                return sb.ToString();
            }
            return string.Empty;
        }

        public static string GetClientIP()
        {
            if (HttpContext.Current == null) return string.Empty;

            return GetClientIP(HttpContext.Current);
        }

        public static string GetClientIP(HttpContext context)
        {
            string userHostAddress = string.Empty;
            userHostAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(userHostAddress))
            {
                if (userHostAddress.IndexOf(".") < -1)
                {
                    userHostAddress = null;
                }
                else if ((userHostAddress.IndexOf(",") > -1) || (userHostAddress.IndexOf(";") > -1))
                {
                    userHostAddress = userHostAddress.Replace(" ", "").Replace("'", "").Replace("\"", "");
                    string[] strArray = userHostAddress.Split(",;".ToCharArray());
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        if (IsIPAddress(strArray[i])
                            && (strArray[i].Substring(0, 3) != "10.")
                            && (strArray[i].Substring(0, 7) != "192.168")
                            && (strArray[i].Substring(0, 7) != "172.16.")
                            && (strArray[i].Substring(0, 7) != "172.26."))
                        {
                            return strArray[i];
                        }
                    }
                }
                else
                {
                    if (IsIPAddress(userHostAddress))
                    {
                        return userHostAddress;
                    }
                    userHostAddress = null;
                }
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = context.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = context.Request.UserHostAddress;
            }
            return userHostAddress;
        }

        public static string GetWholeClientIP()
        {
            return GetWholeClientIP(HttpContext.Current);
        }

        public static string GetWholeClientIP(HttpContext context)
        {
            string userHostAddress = string.Empty;
            userHostAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = context.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = context.Request.UserHostAddress;
            }
            return userHostAddress;
        }

        private static bool IsIPAddress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return false;
            }
            ipAddress = ipAddress.Trim();
            if ((ipAddress.Length < 7) || (ipAddress.Length > 15))
            {
                return false;
            }
            string pattern = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(ipAddress);
        }

        /// <summary>
        /// 消息处理程序所在服务器IP
        /// </summary>
        public static string GetServerIP()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().ToString();
        }

        /// <summary>
        /// 是否ajax请求
        /// </summary>
        /// <returns></returns>
        public static bool IsAjaxRequest()
        {
            if (HttpContext.Current == null)
                return false;

            var context = HttpContext.Current;
            var headers = context.Request.Headers;

            //判断是否是ajax请求 扩展方法取header中X-Requested-With信息，不是很准确
            return
                (context.Request["X-Requested-With"] == "XMLHttpRequest") ||
                (headers != null && (headers["X-Requested-With"] == "XMLHttpRequest" || (headers["Accept"] + string.Empty).Contains("json")));
        }

        /// <summary>
        /// 将Form参数转换成有序字典
        /// </summary>
        /// <returns>有序字典</returns>
        public static IDictionary<string, string> ConvertFormToDict()
        {
            if (HttpContext.Current == null) return new SortedDictionary<string, string>();

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            var form = HttpContext.Current.Request.Form;
            foreach (var key in form.AllKeys)
            {
                dic.Add(key, form[key]);
            }
            return dic;
        }

        /// <summary>
        /// 将Form参数转换成有序字典
        /// </summary>
        /// <returns>有序字典</returns>
        public static IDictionary<string, string> ConvertQueryToDict()
        {
            if (HttpContext.Current == null) return new SortedDictionary<string, string>();

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            var query = HttpContext.Current.Request.QueryString;
            foreach (var key in query.AllKeys)
            {
                if (key == null)
                    continue;
                dic.Add(key, query[key] ?? "");
            }
            return dic;
        }


        /// <summary>
        /// 访问接口返回json
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>Jobject对象</returns>
        public static JObject GetJson(string url, int timeout)
        {
            using (var client = new WebClientEx(timeout))
            {
                try
                {
                    client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    client.UseDefaultCredentials = true;
                    client.Encoding = Encoding.UTF8;
                    var str = client.DownloadString(url);

                    return JObjectHelper.TryParse(str);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取UA
        /// </summary>
        /// <returns></returns>
        public static string GetUserAgent()
        {
            if (HttpContext.Current == null) return string.Empty;

            return HttpContext.Current.Request.UserAgent;
        }
    }
    public class WebClientEx : WebClient
    {
        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        public WebClientEx() : this(60000) { }

        public WebClientEx(int timeout)
        {
            this.Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }
    }
}