using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Linq;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Formatting = Newtonsoft.Json.Formatting;

namespace H5Web.handlers
{
    /// <summary>
    ///     GetNewsListFilter 的摘要说明
    /// </summary>
    public class GetNewsListFilter : H5PageBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            #region

            SetPageCache(60);

            context.Response.ContentType = "application/json; charset=utf-8";

            if (context.Request.QueryString["csid"] == null && string.IsNullOrEmpty(context.Request.QueryString["csid"]))
            {
                return;
            }

            var serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);

            var topCount = 4;

            if (context.Request.QueryString["top"] != null)
            {
                int.TryParse(context.Request.QueryString["top"], out topCount);
            }

            var dealerid = 0;

            if (context.Request.QueryString["dealerid"] != null)
            {
                int.TryParse(context.Request.QueryString["dealerid"], out dealerid);
            }

            var keyList = new List<int> {serialId, topCount, dealerid};

            #endregion

            var cacheKey = "H5V3_GetNewsListFilter_" + string.Join("_", keyList);

            var obj = CacheManager.GetCachedData(cacheKey);

            if (obj != null)
            {
                context.Response.Write(obj);
            }
            else
            {
                //string xmlPath = @"\\192.168.0.174\Data\SerialNews\H5V3News\1765.xml";
                var xmlPath = Path.Combine(WebConfig.DataBlockPath,
                    string.Format(@"Data\SerialNews\H5V3News\{0}.xml", serialId));

                #region

                if (File.Exists(xmlPath))
                {
                    var xDoc = XDocument.Load(xmlPath);

                    var root = xDoc.Root;

                    if (root != null)
                    {
                        var descendants = root.Descendants("Item");

                        var xElements = descendants as XElement[] ?? descendants.ToArray();
                        if (xElements.Any())
                        {
                            #region

                            var filePath = HttpContext.Current.Server.MapPath("~") + "\\config\\NewsIdList.xml";
                            //var filePath = WebConfig.DataBlockPath + @"Data\NewsIdList.xml"; //需要过滤掉的新闻ID列表文件路径
                            var filterDictionary = GetFilterNewsIdList(filePath, keyList);

                            var newsIdList = filterDictionary.Keys.ToList();

                            var listXElements = new List<XElement>();
                            foreach (var item in xElements)
                            {
                                #region 容错处理 如果没有newsid 不参与过滤

                                if (item.Element("newsid") == null)
                                {
                                    listXElements.Add(item);
                                    continue;
                                }

                                #endregion

                                if (filterDictionary.Keys.Contains(item.Element("newsid").Value))
                                {
                                    //过滤掉存在于配置文件中且不与经销商相关的数据
                                    if (filterDictionary[item.Element("newsid").Value].Count <= 0) continue;
                                    //留下存在于配置文件中且不包含当前经销商的数据
                                    if (!filterDictionary[item.Element("newsid").Value].Contains(dealerid.ToString()))
                                    {
                                        listXElements.Add(item);
                                    }
                                }
                                else
                                {
                                    listXElements.Add(item); //留下不存在于配置文件中的数据
                                }
                            }

                            var list = from item in listXElements
                                select new
                                {
                                    NewsId = item.Element("newsid") != null ? item.Element("newsid").Value : "",
                                    Title = item.Element("title").Value,
                                    PublishTime = Convert.ToDateTime(item.Element("publishtime").Value),
                                    PageUrl = item.Element("url").Value.Replace("news.m.yiche.com", "news.h5.yiche.com"),
                                    Author = item.Element("author").Value,
                                    CarImage = item.Element("img").Value,
                                    NewsCategoryShowName = item.Element("newscategoryshowname").Value
                                };

                            var timeConverter = new IsoDateTimeConverter(); //这里使用自定义日期格式，默认是ISO8601格式

                            timeConverter.DateTimeFormat = "yyyy-MM-dd"; //设置时间格式 
                            var serializeObject = JsonConvert.SerializeObject(list.Take(topCount), Formatting.Indented,
                                timeConverter);
                            var cacheDependency = new CacheDependency(filePath);
                            CacheManager.InsertCache(cacheKey, serializeObject, cacheDependency,
                                DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                            //CacheManager.InsertCache(cacheKey, serializeObject, WebConfig.CachedDuration);
                            context.Response.Write(serializeObject);

                            #endregion
                        }
                    }
                    else
                    {
                        context.Response.Write("");
                    }
                }

                #endregion
            }
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return false; }
        }

        protected Dictionary<string, List<string>> GetFilterNewsIdList(string filePath, List<int> keyList)
        {
            var dic = new Dictionary<string, List<string>>();
            try
            {
                var cacheKey = "filterkey_" + string.Join("_", keyList);
                var obj = CacheManager.GetCachedData(cacheKey);
                if (obj != null)
                {
                    dic = (Dictionary<string, List<string>>) obj;
                }
                else
                {
                    if (File.Exists(filePath))
                    {
                        #region

                        var xmlDoc = new XmlDocument();
                        xmlDoc.Load(filePath);
                        var entries = xmlDoc.SelectNodes("/root/news");
                        if (entries != null && entries.Count > 0)
                        {
                            foreach (XmlNode xn in entries)
                            {
                                var newsid = xn.Attributes["id"].Value.Trim();
                                if (newsid == "")
                                {
                                    continue;
                                }
                                var dealerIdList = xn.SelectNodes("dealer");
                                if (!dic.ContainsKey(newsid))
                                {
                                    dic.Add(newsid, new List<string>());
                                }
                                foreach (XmlNode item in dealerIdList)
                                {
                                    var dealerId = item.InnerText;

                                    if (!dic[newsid].Contains(dealerId) && dealerId != "")
                                    {
                                        dic[newsid].Add(dealerId);
                                    }
                                }
                            }
                            var cacheDependency = new CacheDependency(filePath);
                            CacheManager.InsertCache(cacheKey, dic, cacheDependency,
                                DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                        }

                        #endregion
                    }
                    //else
                    //{
                    //    File.CreateText(filePath);
                    //}
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return dic;
        }
    }
}