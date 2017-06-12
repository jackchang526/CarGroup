using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using BitAuto.CarChannel.Common;

namespace WirelessWeb.handlers
{
    /// <summary>
    /// YaGaoSelectCar 的摘要说明
    /// </summary>
    public class YaGaoSelectCar : IHttpHandler
    {
        private int yagao = 0;
        private int price = 0;
        private int stage = 0;
        private string msg = "{{\"msg\":\"{0}\",\"result\":{{{1}}}}}";
        public void ProcessRequest(HttpContext context)
        {
			BitAuto.CarChannel.Common.Cache.CacheManager.SetPageCache(60 * 5);
            context.Response.ContentType = "application/x-javascript";
            GetParmas(context);
            GetData(context);
            context.Response.Write(msg);
            context.Response.End();
        }

        private void GetData(HttpContext context)
        {
            Dictionary<int, Dictionary<int, Dictionary<string, string>>> configDic = GetConfigData();
            if (configDic == null)
            {
                context.Response.Write(msg);
                context.Response.End();
            }
            else
            {
                if (configDic.ContainsKey(yagao) && configDic[yagao].ContainsKey(stage))
                {
                    Dictionary<string, string> resultDic = configDic[yagao][stage];
                    if (resultDic.ContainsKey(price.ToString()))
                    {
                        string recommend = resultDic["Recommend"];
                        string configure = resultDic["Configure"];
                        string csIds = resultDic[price.ToString()];
                        //msg = string.Format(msg, string.Empty, string.Format("\"recommend\":\"{0}\",\"configure\":\"{1}\",\"csId\":\"{2}\"", recommend, configure, csId));
                        if (string.IsNullOrWhiteSpace(csIds))
                        {
                            msg = string.Format(msg, "CsIDs配置节点为空", string.Empty);
                            return;
                        }
                        string[] csIdArr = csIds.Split(',');
                        StringBuilder sb = new StringBuilder();
                        Dictionary<int,string> picDic = new PageBase().GetAllSerialPicURL(true);
                        foreach (string csId in csIdArr)
                        {
                            int serialId = ConvertHelper.GetInteger(csId);
                            SerialEntity _serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
                            if (_serialEntity == null)
                                continue;
                            sb.AppendFormat("{{\"csId\":\"{0}\",\"csName\":\"{1}\",\"referPrice\":\"{2}\",\"imgUrl\":\"{3}\",\"allSpell\":\"{4}\"}},", csId, _serialEntity.ShowName, _serialEntity.ReferPrice, picDic.ContainsKey(serialId) ? picDic[serialId].Replace("_2.","_1.") : WebConfig.DefaultCarPic, _serialEntity.AllSpell);
                        }
                        msg = string.Format(msg, string.Empty, string.Format("\"recommend\":\"{0}\",\"configure\":\"{1}\",\"serial\":[{2}]", recommend, configure, sb.ToString().TrimEnd(',')));
                    }
                    else
                    {
                        msg = string.Format(msg, "参数错误", string.Empty);
                    }
                }
                else
                {
                    msg = string.Format(msg, "参数错误", string.Empty);
                }
            }
        }

        private Dictionary<int, Dictionary<int, Dictionary<string, string>>> GetConfigData()
        {
            const string cacheKey = "Car_Wireless_YaoGaoConfig";
            object configObj = CacheManager.GetCachedData(cacheKey);
            if (configObj == null)
            {
                string filePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\YaGaoSelectCar.xml";
                try
                {
                    if (File.Exists(filePath))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(filePath);
                        if (filePath == null)
                        {
                            msg = string.Format(msg, "YaGaoSelectCar.xml解析错误", string.Empty);
                            return null;
                        }
                        Dictionary<int, Dictionary<int, Dictionary<string, string>>> configDic = null;
                        Dictionary<int, Dictionary<string, string>> tagDic = null;
                        Dictionary<string, string> priceDic = null;
                        XmlNodeList itemNodeList = xmlDoc.SelectNodes("Root/Item");

                        configDic = new Dictionary<int, Dictionary<int, Dictionary<string, string>>>();
                        foreach (XmlNode itemNode in itemNodeList)
                        {
                            XmlNodeList tagNodeList = itemNode.SelectNodes("Tag");
                            tagDic = new Dictionary<int, Dictionary<string, string>>();

                            foreach (XmlNode tagNode in tagNodeList)
                            {
                                priceDic = new Dictionary<string, string>();
                                XmlNode recommendNode = tagNode.SelectSingleNode("Recommend");
                                XmlNode configureNode = tagNode.SelectSingleNode("Configure");
                                XmlNodeList priceNodeList = tagNode.SelectNodes("Prices");
                                priceDic.Add("Recommend", recommendNode == null ? string.Empty : recommendNode.InnerText);
                                priceDic.Add("Configure", configureNode == null ? string.Empty : configureNode.InnerText);

                                foreach (XmlNode priceNode in priceNodeList)
                                {
                                    priceDic.Add(priceNode.Attributes["id"].Value, priceNode.SelectSingleNode("CsIDs").InnerText);
                                }
                                tagDic.Add(ConvertHelper.GetInteger(tagNode.Attributes["id"].Value), priceDic);
                            }
                            configDic.Add(ConvertHelper.GetInteger(itemNode.Attributes["id"].Value), tagDic);
                        }
                        CacheManager.InsertCache(cacheKey, configDic, 60 * 24);
                        return configDic;
                    }
                    else
                    {
                        msg = string.Format(msg, "YaGaoSelectCar.xml文件不存在", string.Empty);
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
                return (Dictionary<int, Dictionary<int, Dictionary<string, string>>>)configObj;
            }
        }

        private void GetParmas(HttpContext context)
        {
            yagao = ConvertHelper.GetInteger(context.Request["yagao"]);
            price = ConvertHelper.GetInteger(context.Request["price"]);
            stage = ConvertHelper.GetInteger(context.Request["stage"]);
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