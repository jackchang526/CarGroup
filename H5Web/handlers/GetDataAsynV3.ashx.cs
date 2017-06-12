using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace H5Web.handlers
{
    /// <summary>
    ///     GetDataAsynV3 的摘要说明
    /// </summary>
    public class GetDataAsynV3 : H5PageBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            SetPageCache(60);
            //OutputCachedPage page = new OutputCachedPage();
            //page.ProcessRequest(HttpContext.Current);

            context.Response.ContentType = "text/html";
            var h5ServiceDic = GetH5ServiceDicV3();
            var list = new List<string>();

            #region param

            var serviceName = context.Request["service"];
            list.Add(serviceName);

            var methodName = context.Request["method"];
            list.Add(methodName);

            var csid = context.Request["csid"];
            if (!string.IsNullOrEmpty(csid))
            {
                var csID = ConvertHelper.GetInteger(csid);
                if (csID > 0)
                {
                    list.Add(csID.ToString());
                }
            }

            var cityId = context.Request["cityId"];
            if (!string.IsNullOrEmpty(cityId))
            {
                var cityID = ConvertHelper.GetInteger(cityId);
                if (cityID > 0)
                {
                    list.Add(cityID.ToString());
                }
            }

            var dealerid = context.Request["dealerid"];
            if (!string.IsNullOrEmpty(dealerid))
            {
                var dealerID = ConvertHelper.GetInteger(dealerid);
                if (dealerID > 0)
                {
                    list.Add(dealerID.ToString());
                }
            }

            var type = context.Request["type"];
            if (!string.IsNullOrEmpty(type))
            {
                list.Add(type);
            }

            var brokerid = context.Request["brokerid"];
            if (!string.IsNullOrEmpty(brokerid))
            {
                var brokerID = ConvertHelper.GetInteger(brokerid);
                if (brokerID > 0)
                {
                    list.Add(brokerID.ToString());
                }
            }

            var dealerpersonid = context.Request["dealerpersonid"];
            if (!string.IsNullOrEmpty(dealerpersonid))
            {
                var dealerpersonId = ConvertHelper.GetInteger(dealerpersonid);
                if (dealerpersonId > 0)
                {
                    list.Add(dealerpersonId.ToString());
                }
            }

            var agentid = context.Request["agentid"];
            if (!string.IsNullOrEmpty(agentid))
            {
                var agengId = ConvertHelper.GetInteger(agentid);
                if (agengId > 0)
                {
                    list.Add(agengId.ToString());
                }
            }

            var masterbrandid = context.Request["masterbrandid"];
            if (!string.IsNullOrEmpty(masterbrandid))
            {
                var masterBrandId = ConvertHelper.GetInteger(masterbrandid);
                if (masterBrandId > 0)
                {
                    list.Add(masterBrandId.ToString());
                }
            }

            #endregion

            var cacheKey = string.Format("H5V3_20160825_{0}", string.Join("_", list));

            var obj = CacheManager.GetCachedData(cacheKey);
            // var obj = MemCache.GetMemCacheByKey(cacheKey);

            //Thread.Sleep(3000);

            if (obj != null && Convert.ToString(obj).TrimEnd().Length > 0)
            {
                var res = (string) obj;
                switch (methodName.ToLower())
                {
                    case "share":
                        //case "youhuigouche":
                        context.Response.ContentType = "application/json; charset=utf-8";
                        context.Response.Write(res);
                        break;
                    case "header":
                    case "dealermapdetail":
                    case "userdealermapdetail":
                    case "dealersaleshare":
                    case "dealershare":
                        context.Response.ContentType = "application/x-javascript";
                        context.Response.Write("document.write('" + CommonFunction.GetUnicodeByString(res) + "');");
                        break;
                    default:
                        context.Response.Write(res);
                        break;
                }
                context.Response.End();
            }
            else
            {
                Uri url = null;

                if (!string.IsNullOrEmpty(serviceName) && !string.IsNullOrEmpty(methodName))
                {
                    if (!h5ServiceDic.ContainsKey(serviceName))
                    {
                        return;
                    }
                    if (!h5ServiceDic[serviceName].ContainsKey(methodName))
                    {
                        return;
                    }

                    #region 组织Url

                    switch (serviceName.ToLower())
                    {
                        case "yichemall":
                            url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], csid, cityId));
                            break;
                        case "daikuan":
                            url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], csid, cityId));
                            break;
                        case "huimaiche":
                            switch (methodName.ToLower())
                            {
                                case "youhuigouche":
                                    url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], csid, cityId));
                                    break;
                                case "discount":
                                    url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], agentid));
                                    break;
                                case "header":
                                    url =
                                        new Uri(string.Format(h5ServiceDic[serviceName][methodName], csid, agentid));
                                    break;
                                default:
                                    url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], csid, agentid));
                                    break;
                            }
                            break;
                        case "agent":
                            url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], brokerid, csid, type));
                            break;
                        case "dealer":
                            if (methodName.ToLower() == "userdealerstaticmap" ||
                                methodName.ToLower() == "userdealermapdetail")
                            {
                                url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], cityId, csid));
                            }
                            else if (methodName.ToLower() == "ending")
                            {
                                url =
                                    new Uri(string.Format(h5ServiceDic[serviceName][methodName], dealerid, masterbrandid, csid));
                            }
                            else
                            {
                                url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], dealerid, csid));
                            }
                            break;
                        case "dealersale":
                            if (methodName.ToLower() == "header")
                            {
                                url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], dealerpersonid));
                            }
                            else
                            {
                                url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], csid, dealerpersonid));
                            }
                            break;
                        case "market":
                            url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], csid, cityId));
                            break;
                        case "ershouche":
                            url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], cityId, csid));
                            break;
                        case "yanghu":
                            url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], cityId, csid));
                            break;
                        case "yixin":
                            url = new Uri(h5ServiceDic[serviceName][methodName]);
                            break;
                    }

                    #endregion
                }

                if (url == null) return;

                #region 异步请求

                var webClient = new WebClient {Encoding = Encoding.UTF8};

                webClient.DownloadStringAsync(url);

                webClient.DownloadStringCompleted += (s, evt) =>
                {
                    if (evt.Error != null)
                    {
                        // 记录异常
                        CommonFunction.WriteInvokeLog(string.Format("\r\n接口异常:{0}\r\nIP:{1}\r\nUrl:{2}\r\n{3}\r\n"
                            , url, WebUtil.GetClientIP(), context.Request.Url, evt.Error));
                        CacheManager.InsertCache(cacheKey, "", WebConfig.CachedDuration);
                        // MemCache.SetMemCacheByKey(cacheKey, "", 1000*60*15);
                        context.Response.Write("");
                    }
                    else
                    {
                        CacheRes(cacheKey, evt.Result); //缓存
                        switch (methodName.ToLower())
                        {
                            case "share": //返回js对象
                                //case "youhuigouche":
                                context.Response.ContentType = "application/json; charset=utf-8";
                                context.Response.Write(evt.Result);
                                break;
                            case "header":
                            case "dealermapdetail":
                            case "userdealermapdetail":
                            case "dealersaleshare":
                            case "dealershare":
                                context.Response.ContentType = "application/x-javascript";
                                context.Response.Write("document.write('" +
                                                       CommonFunction.GetUnicodeByString(evt.Result) + "');");
                                break;
                            default:
                                context.Response.Write(evt.Result);
                                break;
                        }
                    }
                    context.Response.End();
                };

                #endregion
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        ///     目标字符串中是否含有指定字符串
        /// </summary>
        /// <param name="sourse">目标字符串</param>
        /// <param name="tags">指定的字符串数组</param>
        /// <returns>指定的字符串数组任意元素存在于目标字符串则返回true</returns>
        private static bool IsContainsTag(string sourse, params string[] tags)
        {
            return tags.Any(item => sourse.ToLower().IndexOf(item.ToLower(), StringComparison.Ordinal) > 0);
        }

        /// <summary>
        ///     将res.Length > 0的数据缓存
        /// </summary>
        /// <param name="cacheKey">Key</param>
        /// <param name="res">缓存结果</param>
        private static void CacheRes(string cacheKey, string res)
        {
            if (res.Length > 0)
            {
                // MemCache.SetMemCacheByKey(cacheKey, res, 1000*60*15);

                CacheManager.InsertCache(cacheKey, res, WebConfig.CachedDuration);
            }
        }
    }
}