using System.IO;
using System.Web;

using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using System.Xml;
using System.Collections.Generic;
using BitAuto.CarChannel.Common.Cache;

namespace H5Web.handlers
{
    /// <summary>
    /// GetKoubeiImpression 的摘要说明
    /// </summary>
    public class GetKoubeiImpression : H5PageBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            base.SetPageCache(60);
            context.Response.ContentType = "application/json; charset=utf-8";
            string serialIds = context.Request.QueryString["csids"];
            if (string.IsNullOrWhiteSpace(serialIds))
            {
                return;
            }
            string[] csIdArr = serialIds.Split(',');
            var xmlPath = Path.Combine(WebConfig.DataBlockPath, @"Data\Koubei\SerialKouBei\{0}.xml");
            List<string> list = new List<string>();
            list.Add("{");
            for (int i = 0; i < csIdArr.Length; i++)
            {
                int csId = ConvertHelper.GetInteger(csIdArr[i]);
                if (csId > 0)
                {
                    string cacheKey = "H5SerialImpression_" + csId;
                    var obj = CacheManager.GetCachedData(cacheKey);
                    if (obj != null)
                    {
                        list.Add(string.Format("\"{0}\":{{{1}}}", csId, obj));
                        if (i != csIdArr.Length - 1)
                        {
                            list.Add(",");
                        }
                        continue;
                    }
                    string fileName = string.Format(xmlPath, csId);
                    if (File.Exists(fileName))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(fileName);
                        if (xmlDoc != null)
                        {
                            XmlNodeList goodNodeList = xmlDoc.SelectNodes("root/KoubeiImpression/Good/Item");
                            XmlNodeList badNodeList = xmlDoc.SelectNodes("root/KoubeiImpression/Bad/Item");
                            string goodImpressionStr = string.Empty;
                            string badImpressionStr = string.Empty;
                            if (goodNodeList != null && goodNodeList.Count > 0)
                            {
                                List<string> goodList = new List<string>();
                                foreach (XmlNode node in goodNodeList)
                                {
                                    XmlNode wordNode = node.SelectSingleNode("Keyword");
                                    if (wordNode == null)
                                    {
                                        continue;
                                    }
                                    XmlNode categoryNode = node.SelectSingleNode("CategoryName");

                                    goodList.Add(string.Format("\"{0}|{1}\"", wordNode.InnerText, categoryNode == null ? string.Empty : categoryNode.InnerText));
                                }
                                goodImpressionStr = string.Join(",", goodList.ToArray());
                            }
                            if (badNodeList != null && badNodeList.Count > 0)
                            {
                                List<string> badList = new List<string>();
                                foreach (XmlNode node in badNodeList)
                                {
                                    XmlNode wordNode = node.SelectSingleNode("Keyword");
                                    if (wordNode == null)
                                    {
                                        continue;
                                    }
                                    XmlNode categoryNode = node.SelectSingleNode("CategoryName");
                                    badList.Add(string.Format("\"{0}|{1}\"", wordNode.InnerText, categoryNode == null ? string.Empty : categoryNode.InnerText));
                                }
                                badImpressionStr = string.Join(",", badList.ToArray());
                            }
                            string cacheValue = string.Format("\"good\":[{0}],\"bad\":[{1}]", goodImpressionStr, badImpressionStr);
                            list.Add(string.Format("\"{0}\":{{{1}}}", csId, cacheValue));
                            CacheManager.InsertCache(cacheKey, cacheValue, 60 * 24);
                            if (i != csIdArr.Length - 1)
                            {
                                list.Add(",");
                            }
                        }
                    }
                    //list.Add("}");
                }
            }
            list.Add("}");
            context.Response.Write(string.Join("", list.ToArray()));
            context.Response.End();
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