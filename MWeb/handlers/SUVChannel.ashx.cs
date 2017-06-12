using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace MWeb.handlers
{
    /// <summary>
    /// SUVChannel 的摘要说明
    /// </summary>
    public class SUVChannel : IHttpHandler
    {
        private int channelId = 0;
        private int sort = 0;
        private string msg = "{{\"msg\":\"{0}\",\"result\":{{{1}}}}}";
        private PageBase pageBase;

        //[OutputCache(Duration = 18000, VaryByParam = "*")]
        public void ProcessRequest(HttpContext context)
        {
            //base.SetPageCache(60 * 5);
            context.Response.ContentType = "application/x-javascript";
            pageBase = new PageBase();
            GetParmas(context);
            //GetConfigData();
            GetData(context);
            context.Response.Write(msg);
            context.Response.End();
        }

        private void GetData(HttpContext context)
        {
            Dictionary<int, Dictionary<string, string>> configDic = GetConfigData();
            if (configDic == null)
            {
                context.Response.Write(msg);
                context.Response.End();
            }
            else
            {
                if (configDic.ContainsKey(channelId))
                {
                    Dictionary<string, string> resultDic = configDic[channelId];
                    string csIds = resultDic["CsId"];
                    if (string.IsNullOrWhiteSpace(csIds))
                    {
                        msg = string.Format(msg, "CsIDs配置节点为空", string.Empty);
                        return;
                    }

                    Dictionary<int, int> allSerialUvDic = Car_SerialBll.GetAllSerialUVDict();
                    Dictionary<int, string> picDic = pageBase.GetAllSerialPicURL(true);

                    List<SerialEntity> list = new List<SerialEntity>();
                    StringBuilder sbSerial = new StringBuilder();
                    string[] csIdArr = csIds.Split(',');
                    foreach (string csId in csIdArr)
                    {
                        int serialId = ConvertHelper.GetInteger(csId);
                        SerialEntity _serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
                        if (_serialEntity == null)
                            continue;
                        if (allSerialUvDic.ContainsKey(serialId))
                        {
                            _serialEntity.PvNum = allSerialUvDic[serialId];
                        }
                        else
                        {
                            _serialEntity.PvNum = 0;
                        }
                        if (_serialEntity.ReferPrice != "暂无")
                        {
                            //取一个最低报价
                            string[] priceSeg = _serialEntity.ReferPrice.Replace("万", "").Split('-');
                            if (priceSeg.Length > 0)
                            {
                                double minPrice = 0.0;
                                bool isDouble = Double.TryParse(priceSeg[0], out minPrice);
                                if (isDouble)
                                    _serialEntity.MinPrice = minPrice;
                            }
                        }
                        list.Add(_serialEntity);

                        //sbSerial.AppendFormat("{{\"csId\":\"{0}\",\"csName\":\"{1}\",\"referPrice\":\"{2}\",\"imgUrl\":\"{3}\",\"allSpell\":\"{4}\"}},", csId, _serialEntity.ShowName, _serialEntity.ReferPrice, picDic.ContainsKey(serialId) ? picDic[serialId].Replace("_2.", "_1.") : WebConfig.DefaultCarPic, _serialEntity.AllSpell);
                    }

                    SortList(list);//排序

                    foreach (SerialEntity serial in list)
                    {
                        sbSerial.AppendFormat("{{\"csId\":\"{0}\",\"csName\":\"{1}\",\"referPrice\":\"{2}\",\"imgUrl\":\"{3}\",\"allSpell\":\"{4}\"}},", serial.Id, serial.ShowName, serial.ReferPrice, picDic.ContainsKey(serial.Id) ? picDic[serial.Id].Replace("_2.", "_3.") : WebConfig.DefaultCarPic, serial.AllSpell);
                    }

                    StringBuilder sbTag = new StringBuilder();
                    string[] tags = resultDic["Tag"].Split(',');
                    foreach (string tag in tags)
                    {
                        string[] tagArr = tag.Split('|');
                        //sbTag.AppendFormat("{{\"title\":\"{0}\",\"content\":\"{1}\"}},", Server.UrlDecode(tagArr[0]), Server.UrlDecode(tagArr[1]));
                        sbTag.AppendFormat("{{\"title\":\"{0}\",\"content\":\"{1}\"}},", tagArr[0], tagArr[1]);
                    }
                    msg = string.Format(msg, string.Empty, string.Format("\"tag\":[{0}],\"serial\":[{1}]", sbTag.ToString().TrimEnd(','), sbSerial.ToString().TrimEnd(',')));
                }
                else
                {
                    msg = string.Format(msg, "参数错误", string.Empty);
                }
            }
        }

        private Dictionary<int, Dictionary<string, string>> GetConfigData()
        {
            const string cacheKey = "Car_Wireless_SUVChannelConfig";
            object configObj = CacheManager.GetCachedData(cacheKey);
            if (configObj == null)
            {
                string filePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\SUVChannel.xml";
                try
                {
                    if (File.Exists(filePath))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(filePath);
                        if (filePath == null)
                        {
                            msg = string.Format(msg, "SUVChannel.xml解析错误", string.Empty);
                            return null;
                        }
                        Dictionary<int, Dictionary<string, string>> configDic = new Dictionary<int, Dictionary<string, string>>();
                        XmlNodeList channelNodeList = xmlDoc.SelectNodes("Root/Channel");

                        foreach (XmlNode channelNode in channelNodeList)
                        {
                            StringBuilder tagSb = new StringBuilder();
                            int id = ConvertHelper.GetInteger(channelNode.Attributes["id"].Value);
                            XmlNodeList tagNodeList = channelNode.SelectNodes("Tags/Tag");
                            foreach (XmlNode tagNode in tagNodeList)
                            {
                                Dictionary<string, string> tagDic = new Dictionary<string, string>();
                                XmlNode title = tagNode.SelectSingleNode("Title");
                                XmlNode content = tagNode.SelectSingleNode("Content");
                                tagDic.Add("Title", title.InnerText);
                                tagDic.Add("Content", content.InnerText);
                                //tagSb.AppendFormat("{0}|{1},", Server.UrlEncode(title.InnerText), Server.UrlEncode(content.InnerText));
                                tagSb.AppendFormat("{0}|{1},", title.InnerText, content.InnerText);
                            }
                            XmlNode csIdNode = channelNode.SelectSingleNode("CsId");
                            Dictionary<string, string> channelDic = new Dictionary<string, string>();
                            channelDic.Add("CsId", csIdNode.InnerText);
                            channelDic.Add("Tag", tagSb.Length > 0 ? tagSb.ToString().Substring(0, tagSb.Length - 1) : string.Empty);
                            configDic.Add(id, channelDic);
                        }
                        CacheManager.InsertCache(cacheKey, configDic, 60 * 24);
                        return configDic;
                    }
                    else
                    {
                        msg = string.Format(msg, "SUVChannel.xml文件不存在", string.Empty);
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
                return (Dictionary<int, Dictionary<string, string>>)configObj;
            }
        }

        private void SortList(List<SerialEntity> list)
        {
            if (list == null || list.Count == 0) return;

            switch (sort)
            {
                case 2:
                    list.Sort(delegate (SerialEntity s1, SerialEntity s2)
                    { //暂无报价 永远排后
                        if (s1.ReferPrice == "暂无" || s2.ReferPrice == "暂无")
                        {
                            if (s1.ReferPrice == s2.ReferPrice)
                                return s2.PvNum - s1.PvNum;
                            if (s1.ReferPrice == "暂无")
                                return 1;
                            if (s2.ReferPrice == "暂无")
                                return -1;
                        }
                        if (s1.MinPrice == s2.MinPrice)
                            return s2.PvNum - s1.PvNum;
                        if (s1.MinPrice > s2.MinPrice)
                            return 1;
                        return -1;
                    });
                    break;
                case 3:
                    list.Sort(delegate (SerialEntity s1, SerialEntity s2)
                    {
                        if (s1.MinPrice == s2.MinPrice)
                            return s2.PvNum - s1.PvNum;
                        if (s1.MinPrice > s2.MinPrice)
                            return -1;
                        return 1;
                    });
                    break;
                default:
                    list.Sort(delegate (SerialEntity s1, SerialEntity s2)
                    {
                        if (s1.PvNum == s2.PvNum)
                            return s1.Id - s2.Id;
                        if (s1.PvNum > s2.PvNum)
                            return -1;
                        return 1;
                    });
                    break;
            }
        }

        private void GetParmas(HttpContext context)
        {
            channelId = ConvertHelper.GetInteger(context.Request["channelId"]);
            sort = ConvertHelper.GetInteger(context.Request["sort"]);
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
