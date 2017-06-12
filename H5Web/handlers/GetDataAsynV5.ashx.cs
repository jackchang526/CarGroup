using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace H5Web.handlers
{
    /// <summary>
    /// GetDataAsynV5 的摘要说明
    /// </summary>
    public class GetDataAsynV5 : H5PageBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            SetPageCache(60);

            context.Response.ContentType = "text/html";

            var h5ServiceDic = GetH5ServiceDicV3();

            var list = new List<string>();

            #region param

            var serviceName = context.Request["service"];
            if (!h5ServiceDic.ContainsKey(serviceName))
            {
                return;
            }
            list.Add(serviceName);

            var methodName = context.Request["method"];
            if (!h5ServiceDic[serviceName].ContainsKey(methodName))
            {
                return;
            }
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

            var cacheKey = string.Format("H5V4_2016012514_{0}", string.Join("_", list));
            var obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null && Convert.ToString(obj).TrimEnd().Length > 0)
            {
                var res = (string)obj;
                context.Response.Write(res);
            }
            else
            {
                Uri url = null;

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
                                new Uri(string.Format(h5ServiceDic[serviceName][methodName], dealerid, masterbrandid,
                                    csid));
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
                }

                #endregion

                if (url == null) return;

                #region 异步请求

                string msg;
                HttpGetWebResponse(url.ToString(), 100, out msg); 

                #endregion
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
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


        /// <summary>
        /// 获取请求结果
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="timeout">超时时间(秒)</param>
        /// <param name="requestXML">请求xml内容</param>
        /// <param name="isPost">是否post提交</param>
        /// <param name="msg">抛出的错误信息</param>
        /// <returns>返回请求结果</returns>
        private string HttpPostWebRequest(string requestUrl, int timeout, string requestXML, bool isPost, out string msg)
        {
            return HttpPostWebRequest(requestUrl, timeout, requestXML, isPost, "utf-8", out msg);
        }

        /// <summary>
        /// 获取请求结果
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="timeout">超时时间(秒)</param>
        /// <param name="requestXML">请求xml内容</param>
        /// <param name="isPost">是否post提交</param>
        /// <param name="encoding">编码格式 例如:utf-8</param>
        /// <param name="msg">抛出的错误信息</param>
        /// <returns>返回请求结果</returns>
        private string HttpPostWebRequest(string requestUrl, int timeout, string requestXML, bool isPost, string encoding, out string msg)
        {
            msg = string.Empty;
            string result = string.Empty;
            try
            {
                byte[] bytes = System.Text.Encoding.GetEncoding(encoding).GetBytes(requestXML);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = requestUrl;
                request.Method = isPost ? "POST" : "GET";
                request.ContentLength = bytes.Length;
                request.Timeout = timeout * 1000;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding(encoding));
                    result = reader.ReadToEnd();
                    reader.Close();
                    responseStream.Close();
                    request.Abort();
                    response.Close();
                    return result.Trim();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
            }

            return result;
        }

        /// <summary>
        /// get方式输出结果
        /// </summary>
        /// <param name="responseUrl"></param>
        /// <param name="timeOut">秒</param>
        /// <param name="msg"> </param>
        private void HttpGetWebResponse(string responseUrl, int timeOut, out string msg)
        {
            msg = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(responseUrl);
                request.Method = "GET";
                request.Timeout = timeOut * 1000;
                //request.ContentType = "application/x-www-form-urlencoded";
                request.GetResponse();
                request.Abort();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        /// <summary>
        /// get方式输出结果
        /// </summary>
        /// <param name="responseUrl"></param>
        /// <param name="timeOut">秒</param>
        /// <param name="responseParams">输出参数</param>
        /// <param name="msg"> </param>
        private void HttpGetWebResponse(string responseUrl, int timeOut, string responseParams, out string msg)
        {
            string url = responseUrl;
            if (url.Trim().Contains("?"))
            {
                url += "&" + responseParams;
            }
            else
            {
                url += "?" + responseParams;
            }

            HttpGetWebResponse(url, timeOut, out msg);
        }

        /// <summary>
        /// 获取通知地址
        /// </summary>
        /// <param name="responseUrl"></param>
        /// <param name="responseParams"></param>
        /// <returns></returns>
        private string GetWebResponseUrl(string responseUrl, string responseParams)
        {
            string url = responseUrl;
            if (url.Trim().Contains("?"))
            {
                url += "&" + responseParams;
            }
            else
            {
                url += "?" + responseParams;
            }

            return url;
        }

        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <param name="xmlD"></param>
        /// <param name="selectSingleNode"></param>
        /// <returns></returns>
        private string GetSingleNodeValue(XmlDocument xmlD, string selectSingleNode)
        {
            string result = string.Empty;
            if (xmlD != null)
            {
                var node = xmlD.SelectSingleNode(selectSingleNode);
                if (node != null)
                {
                    result = node.InnerText;
                }
            }

            return result;
        }


    }
}