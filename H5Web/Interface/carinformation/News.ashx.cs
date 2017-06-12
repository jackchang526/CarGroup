using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Linq;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace H5Web.Interface.carinformation
{
    /// <summary>
    ///     评测新闻
    /// </summary>
    public class News : H5PageBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            SetPageCache(60);
            context.Response.ContentType = "text/plain";

            try
            {
                #region

                if (context.Request.QueryString["csid"] == null &&
                    string.IsNullOrEmpty(context.Request.QueryString["csid"]))
                {
                    return;
                }

                var serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);

                var topCount = 9;

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

                var cacheKey = "Interface_News_" + string.Join("_", keyList);

                var obj = CacheManager.GetCachedData(cacheKey);

                if (obj != null)
                {
                    context.Response.Write(obj);
                }
                else
                {
                    var xmlPath = Path.Combine(WebConfig.DataBlockPath,
                        string.Format(@"Data\SerialNews\H5V3News\{0}.xml", serialId));

                    #region

                    if (File.Exists(xmlPath))
                    {
                        var baseSerialEntity = (SerialEntity) DataManager.GetDataEntity(EntityType.Serial, serialId);

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
                                        if (
                                            !filterDictionary[item.Element("newsid").Value].Contains(dealerid.ToString()))
                                        {
                                            listXElements.Add(item);
                                        }
                                    }
                                    else
                                    {
                                        listXElements.Add(item); //留下不存在于配置文件中的数据
                                    }
                                }

                                #region 拼装html

                                var stringBuilder = new StringBuilder();
                                stringBuilder.Append("<header>");
                                stringBuilder.Append("<h2>评测导购</h2>");
                                stringBuilder.Append("</header>");

                                for (var i = 0; i < listXElements.Take(topCount).Count(); i++)
                                {
                                    if (i%3 == 0)
                                    {
                                        stringBuilder.AppendFormat("<div class='slide' data-anchor='slide4-{0}'>",
                                            i/3 + 1);
                                        stringBuilder.Append("    <div class='con_top_bg'></div>");
                                        stringBuilder.Append("    <!--内容容器开始-->");
                                        stringBuilder.Append("    <div class='contain'>");
                                        stringBuilder.Append("        <ul class='con_list_ul'>");
                                    }
                                    var imgElement = listXElements[i].Element("img");
                                    if (imgElement != null && imgElement.Value != "")
                                    {
                                        stringBuilder.Append("                    <li>");
                                    }
                                    else
                                    {
                                        stringBuilder.Append("                    <li class='nopic'>");
                                    }
                                    var pageUrl = "";
                                    var urlElement = listXElements[i].Element("url");
                                    if (urlElement != null && urlElement.Value != "")
                                    {
                                        pageUrl = urlElement.Value.Replace("news.m.yiche.com", "news.h5.yiche.com");
                                    }
                                    stringBuilder.AppendFormat("                        <a href='{0}'>", pageUrl);

                                    if (imgElement != null && imgElement.Value != "")
                                    {
                                        stringBuilder.Append("                            <div class='con_list_img'>");
                                        stringBuilder.AppendFormat("                                <img src='{0}'/>",
                                            imgElement.Value);
                                        stringBuilder.Append("                            </div>");
                                    }


                                    stringBuilder.Append("                            <div class='con_list'>");

                                    var titleElement = listXElements[i].Element("title");
                                    var title = "";
                                    if (titleElement != null)
                                    {
                                        title = titleElement.Value;
                                    }
                                    stringBuilder.AppendFormat("                                <h4>{0}</h4>", title);

                                    var newsCategoryShowNameElement = listXElements[i].Element("newscategoryshowname");
                                    if (newsCategoryShowNameElement != null)
                                    {
                                        var newsCategoryShowName = newsCategoryShowNameElement.Value;
                                        if (newsCategoryShowName == "车型详解" || newsCategoryShowName == "购车手册")
                                        {
                                            stringBuilder.Append("                                <p>");
                                            stringBuilder.AppendFormat(
                                                "                                    <strong>{0}</strong>",
                                                newsCategoryShowName);
                                            stringBuilder.Append("                                </p>");
                                        }
                                        else
                                        {
                                            var publishTime = string.Empty;
                                            var publishtimeElement = listXElements[i].Element("publishtime");
                                            if (publishtimeElement != null)
                                            {
                                                publishTime =
                                                    Convert.ToDateTime(publishtimeElement.Value).ToString("yyyy-MM-dd");
                                            }

                                            var author = string.Empty;
                                            var authorElement = listXElements[i].Element("author");
                                            if (authorElement != null)
                                            {
                                                author = authorElement.Value;
                                            }
                                            stringBuilder.AppendFormat("<p>{0}{1}{2}</p>", publishTime,
                                                string.IsNullOrEmpty(author) ? "" : "/", author);
                                        }
                                    }

                                    stringBuilder.Append("                            </div>");
                                    stringBuilder.Append("                        </a>");
                                    stringBuilder.Append("                    </li>");


                                    if ((i == listXElements.Count - 1) || (i + 1)%3 == 0)
                                    {
                                        stringBuilder.Append("        </ul>");
                                        stringBuilder.Append("    </div>");
                                        stringBuilder.Append("    <!--内容容器结束-->");
                                        stringBuilder.Append("</div>");
                                    }
                                }

                                #endregion

                                var cacheDependency = new CacheDependency(filePath);
                                CacheManager.InsertCache(cacheKey, stringBuilder.ToString(), cacheDependency,
                                    DateTime.Now.AddMinutes(WebConfig.CachedDuration));

                                context.Response.Write(stringBuilder.ToString());

                                #endregion
                            }
                        }
                        else
                        {
                            context.Response.Write("");
                        }
                    }
                    else
                    {
                        context.Response.Write("");
                    }

                    #endregion
                }
            }
            catch (Exception)
            {
                context.Response.Write("");
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