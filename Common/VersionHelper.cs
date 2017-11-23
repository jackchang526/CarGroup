using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BitAuto.CarChannel.Common
{
    public class VersionHelper
    {
        
        /// <summary>
        /// 版本比较
        /// </summary>
        /// <param name="compareVer">与当前版本号进行比较的Version对象</param>
        /// <returns>int值，表示与当前版本的大小关系</returns>
        public static int CompareVersion(Version compareVer)
        {
            var ver = GetAppVersion();
            return ver.CompareTo(compareVer);
        }
        /// <summary>
        /// 版本字符串比较
        /// </summary>
        /// <param name="strVersion">与当前版本号进行比较的版本号字符串</param>
        /// <returns>int值，表示与当前版本的大小关系</returns>
        public static int CompareVersion(string strVersion)
        {
            Version compareVer = GetVersion(strVersion);
            return CompareVersion(compareVer);
        }

        /// <summary>
        /// 最小版本号
        /// </summary>
        protected static readonly Version MinVersion = new Version();

        /// <summary>
        /// 最大版本号
        /// </summary>
        protected static Version MaxVersion = new Version("9999.0");

        /// <summary>
        /// 判断当前版本是否受支持
        /// </summary>
        /// <param name="version">要检查的版本对象</param>
        /// <returns>传入的版本是否受支持</returns>
        public static bool IsSurportedVersion(string expression, Version compareVer)
        {
            if (string.IsNullOrWhiteSpace(expression) || expression.Trim() == "*") return true;
            var verArr = expression.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries);
            Version minVer = null;
            Version maxVer = null;
            foreach (var ver in verArr)
            {
                Version version = null; ;
                if (Version.TryParse(ver, out version))
                {
                    if (version == compareVer) return true;
                }
                else
                {
                    var verRangeArr = ver.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
                    if (verRangeArr.Length != 2) continue;

                    if (verRangeArr[0] == "*")
                    {
                        minVer = MinVersion;
                    }
                    else
                    {
                        minVer = GetVersion(verRangeArr[0], MinVersion);
                    }
                    if (verRangeArr[1] == "*")
                    {
                        maxVer = MaxVersion;
                    }
                    else
                    {
                        maxVer = GetVersion(verRangeArr[1], MaxVersion);
                    }
                    if (compareVer >= minVer && compareVer <= maxVer)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 安全的版本号
        /// </summary>
        /// <param name="versionStr">版本号字符串</param>
        /// <param name="defaultVer">默认版本号</param>
        /// <returns>版本号</returns>
        public static Version GetVersion(string versionStr, Version defaultVer = null)
        {
            if (string.IsNullOrWhiteSpace(versionStr)) return MinVersion;

            Version version;
            if (Version.TryParse(versionStr, out version))
            {
                return version;
            }

            return defaultVer ?? MinVersion;
        }

        /// <summary>
        /// 安卓版本号UserAgent匹配正则
        /// </summary>
        private static readonly Regex AndroidVersionRegex = new Regex(@"(?<=bitauto.application ).*?(?= \(Android)", RegexOptions.Compiled);

        /// <summary>
        /// IOS版本号UserAgent匹配正则
        /// </summary>
        private static readonly Regex IOSVersionRegex = new Regex(@"(?<= rv:).*?(?= \()", RegexOptions.Compiled);

        /// <summary>
        /// 判断是否为极速版本App
        /// </summary>
        private static readonly Regex IsFastVerRegex = new Regex(@"^fastbitauto\.", RegexOptions.Compiled);

        /// <summary>
        /// 第一版本号头键
        /// </summary>
        public static readonly string AppVerHeaderName = "appver";

        /// <summary>
        /// 第二版本号头键
        /// </summary>
        public static readonly string AppVerSecondHeaderName = "app_ver";

        /// <summary>
        /// 设备号的http头键
        /// </summary>
        public static readonly string AppDeviceIdHeaderName = "devid";

        public static readonly string ApiVerParaName = "apiver";

        /// <summary>
        /// 获取请求上下文中的版本信息（优先顺序querystring, header(先appver再app_ver), useragent）
        /// </summary>
        /// <returns>版本信息字符串</returns>
        public static string GetAppVerString()
        {
            if (HttpContext.Current == null) return string.Empty;
            var request = HttpContext.Current.Request;
            //从请数据中获取||从请求头中获取
            var strVer = (request[AppVerHeaderName] ?? request[AppVerSecondHeaderName]) ?? (request.Headers[AppVerHeaderName] ?? request.Headers[AppVerSecondHeaderName]);
            if (string.IsNullOrWhiteSpace(strVer))
            {
                //从UA中正则匹配
                if (!string.IsNullOrWhiteSpace(request.UserAgent))
                {
                    if (AndroidVersionRegex.IsMatch(request.UserAgent))
                    {
                        strVer = AndroidVersionRegex.Match(request.UserAgent).ToString();
                    }
                    else if (IOSVersionRegex.IsMatch(request.UserAgent))
                    {
                        strVer = IOSVersionRegex.Match(request.UserAgent).ToString();
                    }
                }
            }

            //如果是极速版的请求使用apiver参数或者头来重写版本号
            if (IsFastYiCheApp())
            {
                strVer = request[ApiVerParaName] ?? request.Headers[ApiVerParaName];
            }

            return strVer ?? string.Empty;
        }

        /// <summary>
        /// 判断是否为极速版
        /// </summary>
        /// <returns></returns>
        public static bool IsFastYiCheApp()
        {
            var request = HttpContext.Current.Request;
            return (string.IsNullOrWhiteSpace(request[ApiVerParaName]) || string.IsNullOrWhiteSpace(request.Headers[ApiVerParaName])) && IsFastVerRegex.IsMatch(request.UserAgent);
        }

        /// <summary>
        /// 获取APP版本号
        /// </summary>
        /// <returns>APP版本对象</returns>
        public static Version GetAppVersion()
        {
            var strVer = GetAppVerString();
            //返回Version对象
            return GetVersion(strVer);
        }

        /// <summary>
        /// 获取APP中的版本号信息
        /// </summary>
        /// <returns>版本号信息</returns>
        public static string GetAppDeviceId()
        {
            if (HttpContext.Current == null) return string.Empty;
            var request = HttpContext.Current.Request;
            string deviceId = request[AppDeviceIdHeaderName] ?? request.Headers[AppDeviceIdHeaderName];
            return deviceId ?? string.Empty;
        }
    }
}
