using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace H5Web.handlers
{
    /// <summary>
    ///     GetNewsList 的摘要说明
    /// </summary>
	public class GetNewsList : H5PageBase,IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
			base.SetPageCache(60);

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

            var keyList = new List<int> {serialId, topCount};

            var cacheKey = string.Format("H5V3_GetNewsList_{0}", string.Join("_", keyList));

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
                            var list = from item in xElements
                                       select new
                                       {
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

                            CacheManager.InsertCache(cacheKey, serializeObject, WebConfig.CachedDuration);

                            context.Response.Write(serializeObject);
                            //context.Response.Write("var newslist=" + serializeObject);
                        }
                    }
                    else
                    {
                        context.Response.Write("");
                    }
                }
            }
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}