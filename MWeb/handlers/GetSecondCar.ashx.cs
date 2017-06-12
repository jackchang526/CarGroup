using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace MWeb.handlers
{
    /// <summary>
    /// GetSecondCar 秒懂车
    /// </summary>
    public class GetSecondCar : IHttpHandler
    {
        private int csId = 0;
        private string msg = "{{\"msg\":\"{0}\",\"result\":{1}}}";
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.CarChannel.Common.Cache.CacheManager.SetPageCache(60 * 5);
            context.Response.ContentType = "application/x-javascript";
            GetParmas(context);
            GetData(context);
            context.Response.Write(msg);
            context.Response.End();
        }
        private void GetParmas(HttpContext context)
        {
            csId = ConvertHelper.GetInteger(context.Request["csid"]);
        }

        private void GetData(HttpContext context)
        {
            Dictionary<int, string> configDic = GetConfigData();
            if (configDic == null)
            {
                context.Response.Write(msg);
                context.Response.End();
            }
            else
            {
                if (configDic.ContainsKey(csId))
                {
                    StringBuilder sb = new StringBuilder();
                    Car_SerialBll csBll = new Car_SerialBll();
                    string serialWhiteImageUrl = Car_SerialBll.GetSerialImageUrl(csId).Replace("_2.", "_3.");
                    sb.AppendFormat("{{\"csId\":\"{0}\",\"imgUrl\":\"{1}\",\"url\":\"{2}\"}},", csId, serialWhiteImageUrl, configDic[csId]);
                    msg = string.Format(msg, string.Empty, sb.ToString().TrimEnd(','));
                }
                else
                {
                    msg = string.Format(msg, "参数错误", string.Empty);
                }
            }
        }


        /// <summary>
        /// 获取XML配置
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, string> GetConfigData()
        {
            const string cacheKey = "Car_Wireless_GetSecondCarConfig";
            object configObj = CacheManager.GetCachedData(cacheKey);
            if (configObj == null)
            {
                string filePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\SecondCar.xml";
                try
                {
                    if (File.Exists(filePath))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(filePath);
                        if (filePath == null)
                        {
                            msg = string.Format(msg, "SecondCar.xml解析错误", string.Empty);
                            return null;
                        }
                        Dictionary<int, string> secondCarDic = null;
                        XmlNodeList itemNodeList = xmlDoc.SelectNodes("Root/Item");

                        secondCarDic = new Dictionary<int, string>();
                        foreach (XmlNode itemNode in itemNodeList)
                        {
                            var csId = ConvertHelper.GetInteger(itemNode.Attributes["Id"].Value);
                            if (!secondCarDic.ContainsKey(csId) && csId > 0)
                            {
                                secondCarDic.Add(csId, string.Format(itemNode.Attributes["Url"].Value, csId));
                            }
                        }
                        CacheManager.InsertCache(cacheKey, secondCarDic, 60 * 24);
                        return secondCarDic;
                    }
                    else
                    {
                        msg = string.Format(msg, "SecondCar.xml文件不存在", string.Empty);
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    msg = string.Format(msg, ex.ToString(), string.Empty);
                    return null;
                }
            }
            else
            {
                return (Dictionary<int, string>)configObj;
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}