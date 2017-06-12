using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;

namespace WirelessWeb
{
    public partial class CsPingCe : WirelessPageBase
    {
        protected int _serialId;
        protected string _serialShowName;
        protected string _serialSeoName;
        protected string _serialAllSpell;
        protected string _serialNavName;

        //Dictionary<int, EnumCollection.PingCeTag> dicAllTagInfo;

        protected string _newsNavHtml = "";
        protected string _newsHtml;
        protected string CsHeadHTML = string.Empty;
        protected int _pageSize = 10;
        private int _pageIndex = 1;
        protected int _currentNewsCount = 0;
        //文章类别
        protected string newsType;
        protected CarNewsType _CarNewsType;
        protected string pageTitle = "";
        protected string pageKeywords = "";
        protected string pageDescription = "";
        //页面的标题内容
        private Dictionary<string, int> TagTitleContent = new Dictionary<string, int>();

        //子品牌信息
        protected SerialEntity _serialEntity;
        protected string _serialMasterBrandName;
        protected string _serialName;
        CarNewsBll carNewsBLL;
        public CsPingCe()
        {
            carNewsBLL = new CarNewsBll();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageCache(30);
            GetPageParam();
            GetSerialDetailInfo();
            _newsNavHtml = RenderNewsNavNew();
            //GetSerialPingCeData();
            _newsHtml = RenderNewsList();
            CsHeadHTML = base.GetCommonNavigation("MCsWenZhang", _serialId);
        }

        /// <summary>
        /// 获取文章内容
        /// </summary>
        /// <returns></returns>
        private string RenderNewsList()
        {
            StringBuilder htmlCode = new StringBuilder();
            int newsCount = 0;
            DataSet newsDs = null;

            if (newsType == "wenzhang")
            {
                List<int> carTypeIdList = new List<int>() 
				{ 
                (int)CarNewsType.serialfocus,
				(int)CarNewsType.shijia,
				(int)CarNewsType.daogou,
				(int)CarNewsType.yongche,
				(int)CarNewsType.gaizhuang,
				(int)CarNewsType.anquan,
				(int)CarNewsType.xinwen,
                (int)CarNewsType.pingce
				};
                newsDs = new CarNewsBll().GetSerialNewsAllData(_serialId, carTypeIdList, _pageSize, _pageIndex, ref newsCount);
            }
            else
            {
                newsDs = new CarNewsBll().GetSerialNewsAllData(_serialId, _CarNewsType, _pageSize, _pageIndex, ref newsCount);
            }


            _currentNewsCount = newsCount;

            if (newsDs != null && newsDs.Tables.Count > 0 && newsDs.Tables[0].Rows.Count > 0)
            {
                //htmlCode.AppendLine(" <div class=\"wrap wrap-b10\">");
                htmlCode.AppendLine(" <div class=\"card-news card-news-list\"><ul id=\"newsd\">");

                DataRowCollection drsNews = newsDs.Tables[0].Rows;
                int pageCount = newsCount / _pageSize + (newsCount % _pageSize == 0 ? 0 : 1);
                int newsCounter = 0;

                foreach (DataRow row in drsNews)
                {
                    newsCounter++;

                    string newsTitle = CommonFunction.NewsTitleDecode(row["title"].ToString());
                    //modified by sk 22个文字 2017-03-06
                    newsTitle = StringHelper.GetRealLength(newsTitle) > 44 ? StringHelper.SubString(newsTitle, 44, false) : newsTitle;
                    int newsId = ConvertHelper.GetInteger(row["cmsnewsid"]);
                    string newsUrl = Convert.ToString(row["filepath"]).Replace("news.bitauto.com", "news.m.yiche.com");

                    string firstPicUrl = string.Empty;
                    if (newsType != "hangqing")
                        firstPicUrl = ConvertHelper.GetString(row["FirstPicUrl"]);
                    //if (newsType == "pingce")
                    //    newsUrl = "/" + _serialAllSpell + "/pingce/p" + newsId + "/";
                    DateTime publishTime = Convert.ToDateTime(row["publishtime"]);
                    string newsContent = Convert.ToString(row["content"]);
                    string from = Convert.ToString(row["sourceName"]);
                    string author = Convert.ToString(row["author"]);
                    int commentNum = row["CommentNum"] == DBNull.Value ? 0 : Convert.ToInt32(row["CommentNum"]);
                    string image = string.Empty;
                    if (!string.IsNullOrEmpty(firstPicUrl))
                    {
                        firstPicUrl = firstPicUrl.Replace("/bitauto/", "/newsimg_180_w0_1/bitauto/")
                            .Replace("/autoalbum/", "/newsimg_180_w0_1/autoalbum/");
                        if (firstPicUrl.IndexOf(".bitauto") != -1)
                        {
                            image = firstPicUrl;
                        }
                    }
                    htmlCode.AppendFormat("<li><a href=\"{0}\">", newsUrl);
                    if (!string.IsNullOrEmpty(image))
                    {
                        htmlCode.AppendFormat("<div class=\"img-box\"><img src=\"{0}\"></div>", image);
                    }
                    htmlCode.AppendFormat("<h4>{0}</h4>", newsTitle);
                    htmlCode.AppendFormat("<em><span>{0}</span><span>{2}</span>{1}</em>",
                        publishTime.ToString("yyyy-MM-dd"), string.Format("<i class=\"ico-comment huifu comment_0_{1}\"></i>", commentNum, newsId),
                        !string.IsNullOrEmpty(author) ? StrCut(author, 6) : "");
                    htmlCode.Append("</a>");
                    htmlCode.Append("</li>");
                }
                htmlCode.AppendLine("</ul></div>");
                //htmlCode.AppendLine("</div>");
                //生成页号导航
                if (pageCount > 1)
                {
                    htmlCode.Append("<div class=\"m-pages\">");
                    htmlCode.Append("<a href=\"#\" id=\"m-pages-pre\" class=\"m-pages-pre m-pages-none\">上一页</a>");
                    htmlCode.Append("<div class=\"m-pages-num\">");
                    htmlCode.AppendFormat("<div class=\"m-pages-num-con\" id=\"m-pages-num-con\">1/{0}</div>", pageCount);
                    htmlCode.Append("<div class=\"m-pages-num-arrow\"></div>");
                    htmlCode.Append("</div>");
                    htmlCode.Append("<select id=\"m-page-select\">");
                    for (int i = 0; i < pageCount; i++)
                    {
                        htmlCode.AppendFormat("<option value=\"{0}\">{0}</option>", i + 1);
                    }
                    htmlCode.Append("</select>");
                    htmlCode.Append("<a href=\"#\" id=\"m-pages-next\" class=\"m-pages-next\">下一页</a>");
                    htmlCode.Append("</div>");

                }

            }
            return htmlCode.ToString();
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetPageParam()
        {
            _serialId = ConvertHelper.GetInteger(Request.QueryString["csID"]);
            newsType = Request.QueryString["type"];
            if (newsType == null)
                newsType = "wenzhang";
            newsType = newsType.ToLower();

            switch (newsType)
            {
                case "wenzhang":
                    pageTitle = "【{0}新闻】{1}{2}新闻_最新{0}报道-手机易车网";
                    pageKeywords = "{0}新闻,{1}{2}新闻,{0}上市新闻,{0}导购,手机易车网,car.m.yiche.com";
                    pageDescription = "{1}{2}新闻,手机易车网提供最权威的{0}新闻资讯、{0}最新行情资讯、{0}优惠、{0}经销商降价信息、网友点评讨论等。";
                    _CarNewsType = CarNewsType.wenzhang;
                    break;
                case "xinwen":
                    pageTitle = "【{0}新闻-{0}上市新闻】_{1}{0}-手机易车网";
                    pageKeywords = "{0}新闻,{0}上市新闻,{1}{0}";
                    pageDescription = "{0}新闻:易车网车型频道为您提供最权威的{1}{0}新闻资讯、{1}{0}上市新闻、{0}最新行情资讯、{0}优惠、{0}经销商降价信息、网友点评讨论等。";
                    _CarNewsType = CarNewsType.xinwen;
                    break;
                case "daogou":
                    pageTitle = "【{0}导购-{0}促销信息】_{1}{0}-易车网";
                    pageKeywords = "{0}导购,{0}促销,{1}{2}";
                    pageDescription = "{0}导购:易车网车型频道为您提供最权威的{1}{0}评测导购资讯、最及时的{1}{0}优惠促销信息、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    _CarNewsType = CarNewsType.daogou;
                    break;
                case "shijia":
                    pageTitle = "【{0}促销-{0}试驾】_{1}{0}-易车网";
                    pageKeywords = "{0}试驾,{0}促销,{1}{2}";
                    pageDescription = "{0}试驾:易车网车型频道为您提供最权威的{1}{0}试驾信息、最及时的{1}{0}优惠促销信息、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    _CarNewsType = CarNewsType.shijia;
                    break;
                case "yongche":
                    pageTitle = "【{0}用车-{0}用车指南】_{1}{0}-易车网";
                    pageKeywords = "{0}用车,{0}用车指南,{1}{2}";
                    pageDescription = "{0}用车:易车网车型频道为您提供最权威的{1}{0}用车指南、最及时的{1}{0}优惠促销信息、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    _CarNewsType = CarNewsType.yongche;
                    break;
                case "pingce":
                    pageTitle = "【{0}评测】{1}{2}评测_最新{0}车型详解-手机易车网";
                    pageKeywords = "{0}评测,{0}单车评测,{1}{2}优点,{0}缺点,{0}车型详解,手机易车网,car.m.yiche.com";
                    pageDescription = "{1}{2}评测,手机易车网提供{0}深度评测,包含{0}外观,内饰,空间,视野,灯光,动力,操控,舒适性,油耗,配置与安全性等{0}评测内容。";
                    _CarNewsType = CarNewsType.pingce;
                    break;
                case "gaizhuang":
                    pageTitle = "【{0}改装-{0}单车改装指南】_{1}{0}-易车网";
                    pageKeywords = "{0}改装,{0}单车改装,{1}{2}";
                    pageDescription = "{0}改装:易车网车型频道为您提供最权威的{1}{0}改装指南、最及时的{1}{0}内饰改装资讯、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    _CarNewsType = CarNewsType.gaizhuang;
                    break;
                case "anquan":
                    pageTitle = "【{0}安全-{0}碰撞安全测试】_{1}{0}-易车网";
                    pageKeywords = "{0}安全,{0}碰撞测试,{1}{0}";
                    pageDescription = "{0}安全:易车网车型频道为您提供最权威的{1}{0}安全指南、最及时的{1}{0}碰撞安全测试、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    _CarNewsType = CarNewsType.anquan;
                    break;
                default:
                    newsType = "xinwen";
                    pageTitle = "【{0}新闻-{0}上市新闻】_{1}{0}-手机易车网";
                    pageKeywords = "{0}新闻,{0}上市新闻,{1}{0}";
                    pageDescription = "{0}新闻:易车网车型频道为您提供最权威的{1}{0}新闻资讯、{1}{0}上市新闻、{0}最新行情资讯、{0}优惠、{0}经销商降价信息、网友点评讨论等。";
                    _CarNewsType = CarNewsType.xinwen;
                    break;
            }
        }

        // 子品牌信息
        private void GetSerialDetailInfo()
        {
            if (_serialId >= 0)
            {
                #region 子品牌基本数据
                _serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, _serialId);

                if (_serialId == 1568)
                    _serialShowName = "索纳塔八";
                else
                    _serialShowName = _serialEntity.ShowName;

                _serialName = _serialEntity.Name;
                _serialMasterBrandName = _serialEntity.Brand.MasterBrand.Name;
                _serialSeoName = _serialEntity.SeoName;
                _serialAllSpell = _serialEntity.AllSpell;
                _serialNavName = _serialEntity.Name;
                #endregion
            }
            if (newsType == "daogou")
            {
                pageTitle = String.Format(pageTitle, _serialSeoName, _serialMasterBrandName);
                pageKeywords = String.Format(pageKeywords, _serialSeoName, _serialMasterBrandName, _serialShowName);
                pageDescription = String.Format(pageDescription, _serialSeoName, _serialMasterBrandName);
            }
            else
            {
                pageTitle = String.Format(pageTitle, _serialSeoName, _serialMasterBrandName, _serialName);
                pageKeywords = String.Format(pageKeywords, _serialSeoName, _serialMasterBrandName, _serialShowName, _serialName);
                pageDescription = String.Format(pageDescription, _serialSeoName, _serialMasterBrandName, _serialName);
            }
        }

        /// <summary>
        /// 生成小导航代码
        /// </summary>
        private string RenderNewsNavNew()
        {
            Dictionary<CarNewsType, string> titleTag = new Dictionary<CarNewsType, string>();
            titleTag.Add(CarNewsType.wenzhang, "全部");
            titleTag.Add(CarNewsType.pingce, "评测");
            titleTag.Add(CarNewsType.shijia, "试驾");
            titleTag.Add(CarNewsType.daogou, "导购");
            titleTag.Add(CarNewsType.gaizhuang, "改装");
            titleTag.Add(CarNewsType.xinwen, "新闻");

            CarNewsBll newsBll = new CarNewsBll();
            StringBuilder htmlCode = new StringBuilder();

            htmlCode.Append("<div class=\"sub-tags\">");
            htmlCode.Append("<ul>");
            List<string> listTemp = new List<string>();
            foreach (KeyValuePair<CarNewsType, string> entity in titleTag)
            {
                //所有文章
                if (entity.Key == CarNewsType.wenzhang)
                {
                    if (entity.Key == _CarNewsType)
                        listTemp.Add(string.Format("<li class=\"current\"><a href=\"/{0}/{1}/\"><span>{2}</span></a></li>", _serialAllSpell.ToLower(), entity.Key.ToString(), entity.Value));
                    else
                        listTemp.Add(string.Format("<li><a href=\"/{0}/{1}/\"><span>{2}</span></a></li>", _serialAllSpell.ToLower(), entity.Key.ToString(), entity.Value));
                    continue;
                }

                if (entity.Key == _CarNewsType)
                {
                    if (_currentNewsCount > 0)
                        listTemp.Add(string.Format("<li class=\"current\"><a href=\"/{1}/{2}/\"><span>{0}</span></a></li>", entity.Value, _serialAllSpell.ToLower(), entity.Key.ToString()));
                    else
                        listTemp.Add(string.Format("<li class=\"current\"><a><span>{0}</span></a></li>", entity.Value));
                    continue;
                }
                int newsCount = newsBll.GetSerialNewsCount(_serialId, 0, entity.Key);
                if (newsCount > 0)
                    listTemp.Add(string.Format("<li><a href=\"/{0}/{1}/\"><span>{2}</span></a></li>", _serialAllSpell.ToLower(), entity.Key.ToString(), entity.Value));

            }
            htmlCode.Append(string.Concat(listTemp.ToArray()));
            htmlCode.Append("</ul>");
            htmlCode.Append("</div>");
            return htmlCode.ToString();
        }

        #region 评测旧代码
        //取子品牌评测数据
        //private void GetSerialPingCeData()
        //{
        //    dicAllTagInfo = CommonFunction.IntiPingCeTagInfo();
        //    DataSet dsPingceNews = carNewsBLL.GetTopSerialNewsAllData(_serialId, CarNewsType.pingce, 10);
        //    int newsId = 0;
        //    Dictionary<int, string> dicPingCeRainbow = base.GetAllPingCeNewsURLForCsPingCePage();
        //    if (dicPingCeRainbow.ContainsKey(_serialId))
        //    {
        //        string url = dicPingCeRainbow[_serialId];
        //        string[] arrTempURL = url.Split('/');
        //        string pageName = arrTempURL[arrTempURL.Length - 1];
        //        if (pageName.Length >= 10)
        //        {
        //            newsId = ConvertHelper.GetInteger(pageName.Substring(3, 7));
        //        }
        //    }

        //    if (newsId > 0)
        //    {
        //        DataSet ds = base.GetPingCeNewByNewID(newsId);
        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("content"))
        //        {
        //            DataRow row = ds.Tables[0].Rows[0];
        //            string title;
        //            if (ds.Tables[0].Columns.Contains("title"))
        //                title = row["title"].ToString();
        //            else
        //                title = row["facetitle"].ToString();
        //            //PingCeFilePath = row["filepath"].ToString();
        //            string newsContent = row["content"].ToString();

        //            string RegexString = "<div(?:[^<]*)?id=\"bt_pagebreak\"[^>]*>([^<]*)</div>";
        //            Regex r = new Regex(RegexString);
        //            string[] newsGroup = r.Split(newsContent);
        //            if (newsGroup.Length < 1)
        //                return;
        //            //初始化标签标题内容
        //            InitTagTitleContent(newsGroup);

        //            StringBuilder htmlCode = new StringBuilder();
        //            string newsBaseUrl = "http://news.m.yiche.com" + row["filepath"].ToString();
        //            //modified by sk 2013.05.30 取新闻简介及第一张图片
        //            string firstImageUrl = "http://img3.bitautoimg.com/images/not.gif";
        //            string summary = string.Empty;
        //            if (dsPingceNews != null && dsPingceNews.Tables[0].Rows.Count > 0)
        //            {
        //                DataRow[] newsRows = dsPingceNews.Tables[0].Select("cmsnewsid=" + newsId);
        //                if (newsRows.Length > 0)
        //                {
        //                    DataRow newsRow = newsRows[0];
        //                    firstImageUrl = ConvertHelper.GetString(newsRow["firstPicUrl"]);
        //                    if (firstImageUrl.Length > 0)
        //                        firstImageUrl = firstImageUrl.Insert(firstImageUrl.IndexOf(".com/") + 5, "wapimg-240-160/");
        //                    summary = ConvertHelper.GetString(newsRow["content"]);
        //                }
        //            }
        //            //htmlCode.AppendFormat("<h2><a href=\"{0}\">{1}</a></h2>", newsBaseUrl, title);
        //            //htmlCode.Append(GetTagContent(newsGroup, newsBaseUrl));
        //            htmlCode.Append("<div class=\"clear\"></div>");

        //            htmlCode.Append("<div class=\"m-car-detailed-head\">");
        //            htmlCode.Append("		<div class=\"m-car-detailed-ico\">详解</div>");
        //            htmlCode.AppendFormat("		<h3><a href=\"{0}\">{1}</a></h3>", newsBaseUrl, title);
        //            htmlCode.Append("		<div class=\"clear\"></div>");
        //            htmlCode.Append("		<dl>");
        //            htmlCode.AppendFormat("			<dt><a href=\"{0}\"><img src=\"{1}\"></a></dt>", newsBaseUrl, firstImageUrl);
        //            htmlCode.AppendFormat("			<dd>{1} <a href=\"{0}\">详细&gt;&gt;</a></dd>", newsBaseUrl, StringHelper.SubString(summary, 70, true));
        //            htmlCode.Append("		</dl>");
        //            htmlCode.Append("	</div>");
        //            _pingceHtml = htmlCode.ToString();
        //        }
        //    }
        //}
        ///// <summary>
        ///// 初始化标签标题内容
        ///// </summary>
        //private void InitTagTitleContent(string[] contentList)
        //{
        //    Regex rex = new Regex(@"\$\$(?<title>.+)\$\$");
        //    int pageIndex = (contentList.Length + 1) / 2 + 1;
        //    string title = string.Empty;
        //    for (int i = contentList.Length - 2; i > 0; i -= 2)
        //    {
        //        pageIndex--;
        //        string tmpStr = contentList[i];
        //        try
        //        {
        //            title = rex.Match(tmpStr).Result("${title}");
        //        }
        //        catch { }
        //        if (title.Length == 0)
        //        {
        //            pageIndex--;
        //            continue;
        //        }
        //        title = StringHelper.RemoveHtmlTag(title);
        //        if (!TagTitleContent.ContainsKey(title))
        //        {
        //            TagTitleContent.Add(title, pageIndex);
        //        }
        //    }

        //}
        ///// <summary>
        ///// 得到标记内容
        ///// </summary>
        //private string GetTagContent(string[] contentList, string newsBaseUrl)
        //{
        //    string title = string.Empty;
        //    StringBuilder htmlCode = new StringBuilder();
        //    htmlCode.Append("<div class=\"m-letter m-letter-car\"><ul>");
        //    foreach (KeyValuePair<int, EnumCollection.PingCeTag> kvp in dicAllTagInfo)
        //    {
        //        int index = 0;
        //        bool isContains = kvp.Value.tagName == "导语" || GetTitleIsContainsTag(kvp.Value, out title, out index);
        //        if (isContains)
        //        {
        //            string url = kvp.Value.tagName == "导语" ? newsBaseUrl : newsBaseUrl.Insert(newsBaseUrl.LastIndexOf('.'), "-" + (index - 1).ToString());
        //            htmlCode.AppendFormat("<li><a href=\"{1}\"><span>{0}</span></a></li>", kvp.Value.tagName, url);
        //        }
        //        else
        //        {
        //            htmlCode.AppendFormat("<li class=\"none\">{0}</li>", kvp.Value.tagName);
        //        }
        //    }
        //    htmlCode.Append("</ul></div>");
        //    return htmlCode.ToString();
        //}

        //private bool GetTitleIsContainsTag(EnumCollection.PingCeTag tag, out string title, out int index)
        //{
        //    Regex r = new Regex(tag.tagRegularExpressions);
        //    title = string.Empty;
        //    bool IsContains = false;
        //    index = 0;
        //    foreach (KeyValuePair<string, int> entity in TagTitleContent)
        //    {
        //        // modified by chengl Dec.28.2011
        //        if (!r.IsMatch(entity.Key))
        //        { continue; }

        //        // if (entity.Key.IndexOf(tag) < 0) continue;
        //        IsContains = true;
        //        title = entity.Key;
        //        index = entity.Value;
        //    }
        //    if (IsContains)
        //    {
        //        TagTitleContent.Remove(title);
        //    }

        //    return IsContains;
        //}
        #endregion
    }
}