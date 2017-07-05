using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;

namespace MWeb.Controllers
{
    public class NewsListController : Controller
    {
        private string _newsType;
        private string _pageTitle;
        private string _pageKeywords;
        private string _pageDescription;
        private int _serialId;
        private CarNewsType _carNewsType;
        private SerialEntity _serialEntity;
        private string _serialAllSpell;
        private PageBase _pageBase;
        private int _currentNewsCount = 0;
        private int _pageSize = 10;
        private int _pageIndex = 1;

        [OutputCache(Duration = 600, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int id, string newstags)
        {
            _pageBase = new PageBase();
            ViewData["SerialId"] = id;
            GetPageParam(id, newstags);
            GetSerialDetailInfo();
            
            ViewData["NewsNavHtml"] = RenderNewsNavNew();
            ViewData["NewsHtml"] = RenderNewsList();
            ViewData["CsHeadHTML"] = _pageBase.GetCommonNavigation("MCsWenZhang", _serialId);
			ViewData["serialEntity"] = _serialEntity;
            return View();
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetPageParam(int csId, string newstags)
        {
            _serialId = csId;
            _newsType = newstags;
            if (_newsType == null)
                _newsType = "wenzhang";
            _newsType = _newsType.ToLower();
            ViewData["NewsType"] = _newsType;
            switch (_newsType)
            {
                case "wenzhang":
                    _pageTitle = "【{0}新闻】{1}{2}新闻_最新{0}报道-手机易车网";
                    _pageKeywords = "{0}新闻,{1}{2}新闻,{0}上市新闻,{0}导购,手机易车网,car.m.yiche.com";
                    _pageDescription = "{1}{2}新闻,手机易车网提供最权威的{0}新闻资讯、{0}最新行情资讯、{0}优惠、{0}经销商降价信息、网友点评讨论等。";
                    _carNewsType = CarNewsType.wenzhang;
                    break;
                case "xinwen":
                    _pageTitle = "【{0}新闻-{0}上市新闻】_{1}{0}-手机易车网";
                    _pageKeywords = "{0}新闻,{0}上市新闻,{1}{0}";
                    _pageDescription = "{0}新闻:易车网车型频道为您提供最权威的{1}{0}新闻资讯、{1}{0}上市新闻、{0}最新行情资讯、{0}优惠、{0}经销商降价信息、网友点评讨论等。";
                    _carNewsType = CarNewsType.xinwen;
                    break;
                case "daogou":
                    _pageTitle = "【{0}导购-{0}促销信息】_{1}{0}-易车网";
                    _pageKeywords = "{0}导购,{0}促销,{1}{2}";
                    _pageDescription = "{0}导购:易车网车型频道为您提供最权威的{1}{0}评测导购资讯、最及时的{1}{0}优惠促销信息、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    _carNewsType = CarNewsType.daogou;
                    break;
                case "shijia":
                    _pageTitle = "【{0}促销-{0}试驾】_{1}{0}-易车网";
                    _pageKeywords = "{0}试驾,{0}促销,{1}{2}";
                    _pageDescription = "{0}试驾:易车网车型频道为您提供最权威的{1}{0}试驾信息、最及时的{1}{0}优惠促销信息、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    _carNewsType = CarNewsType.shijia;
                    break;
                case "yongche":
                    _pageTitle = "【{0}用车-{0}用车指南】_{1}{0}-易车网";
                    _pageKeywords = "{0}用车,{0}用车指南,{1}{2}";
                    _pageDescription = "{0}用车:易车网车型频道为您提供最权威的{1}{0}用车指南、最及时的{1}{0}优惠促销信息、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    _carNewsType = CarNewsType.yongche;
                    break;
                case "pingce":
                    _pageTitle = "【{0}评测】{1}{2}评测_最新{0}车型详解-手机易车网";
                    _pageKeywords = "{0}评测,{0}单车评测,{1}{2}优点,{0}缺点,{0}车型详解,手机易车网,car.m.yiche.com";
                    _pageDescription = "{1}{2}评测,手机易车网提供{0}深度评测,包含{0}外观,内饰,空间,视野,灯光,动力,操控,舒适性,油耗,配置与安全性等{0}评测内容。";
                    _carNewsType = CarNewsType.pingce;
                    break;
                case "gaizhuang":
                    _pageTitle = "【{0}改装-{0}单车改装指南】_{1}{0}-易车网";
                    _pageKeywords = "{0}改装,{0}单车改装,{1}{2}";
                    _pageDescription = "{0}改装:易车网车型频道为您提供最权威的{1}{0}改装指南、最及时的{1}{0}内饰改装资讯、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    _carNewsType = CarNewsType.gaizhuang;
                    break;
                case "anquan":
                    _pageTitle = "【{0}安全-{0}碰撞安全测试】_{1}{0}-易车网";
                    _pageKeywords = "{0}安全,{0}碰撞测试,{1}{0}";
                    _pageDescription = "{0}安全:易车网车型频道为您提供最权威的{1}{0}安全指南、最及时的{1}{0}碰撞安全测试、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    _carNewsType = CarNewsType.anquan;
                    break;
                default:
                    _newsType = "xinwen";
                    _pageTitle = "【{0}新闻-{0}上市新闻】_{1}{0}-手机易车网";
                    _pageKeywords = "{0}新闻,{0}上市新闻,{1}{0}";
                    _pageDescription = "{0}新闻:易车网车型频道为您提供最权威的{1}{0}新闻资讯、{1}{0}上市新闻、{0}最新行情资讯、{0}优惠、{0}经销商降价信息、网友点评讨论等。";
                    _carNewsType = CarNewsType.xinwen;
                    break;
            }
        }

        // 子品牌信息
        private void GetSerialDetailInfo()
        {
            string serialShowName = string.Empty;
            string serialName = string.Empty;
            string serialMasterBrandName = string.Empty;
            string serialSeoName = string.Empty;
            if (_serialId >= 0)
            {
                #region 子品牌基本数据
                _serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, _serialId);

                if (_serialId == 1568)
                {
                    serialShowName = "索纳塔八";
                    ViewData["SerialShowName"] = serialShowName;
                }
                else
                {
                    serialShowName = _serialEntity.ShowName;
                    ViewData["SerialShowName"] = serialShowName;
                }
                serialName = _serialEntity.Name;
                ViewData["SerialName"] = serialName;
                serialMasterBrandName = _serialEntity.Brand.MasterBrand.Name;
                ViewData["MabName"] = serialMasterBrandName;
                serialSeoName = _serialEntity.SeoName;
                _serialAllSpell = _serialEntity.AllSpell;
                ViewData["SerialAllSpell"] = _serialEntity.AllSpell;
                ViewData["MabAllSpell"] = _serialEntity.Brand.MasterBrand.AllSpell;
                #endregion
            }
            if (_newsType == "daogou")
            {
                ViewData["PageTitle"] = String.Format(_pageTitle, serialSeoName, serialMasterBrandName);
                ViewData["PageKeywords"] = String.Format(_pageKeywords, serialSeoName, serialMasterBrandName, serialShowName);
                ViewData["PageDescription"] = String.Format(_pageDescription, serialSeoName, serialMasterBrandName);
            }
            else
            {
                ViewData["PageTitle"] = String.Format(_pageTitle, serialSeoName, serialMasterBrandName, serialName);
                ViewData["PageKeywords"] = String.Format(_pageKeywords, serialSeoName, serialMasterBrandName, serialShowName, serialName);
                ViewData["PageDescription"] = String.Format(_pageDescription, serialSeoName, serialMasterBrandName, serialName);
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
            //titleTag.Add(CarNewsType.shijia, "体验试驾");
            //titleTag.Add("xinwen", "新闻");
            //titleTag.Add("hangqing", "行情");
            titleTag.Add(CarNewsType.daogou, "导购");
            titleTag.Add(CarNewsType.yongche, "用车");
            //titleTag.Add(CarNewsType.gaizhuang, "改装");
            //titleTag.Add(CarNewsType.anquan, "安全");
            titleTag.Add(CarNewsType.keji, "科技");
            titleTag.Add(CarNewsType.wenhua, "文化");
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
                    if (entity.Key == _carNewsType)
                        listTemp.Add(string.Format("<li class=\"current\"><a href=\"/{0}/{1}/\"><span>{2}</span></a></li>", _serialAllSpell.ToLower(), entity.Key.ToString(), entity.Value));
                    else
                        listTemp.Add(string.Format("<li><a href=\"/{0}/{1}/\"><span>{2}</span></a></li>", _serialAllSpell.ToLower(), entity.Key.ToString(), entity.Value));
                    continue;
                }

                if (entity.Key == _carNewsType)
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
        /// <summary>
        /// 获取文章内容
        /// </summary>
        /// <returns></returns>
        private string RenderNewsList()
        {
            StringBuilder htmlCode = new StringBuilder();
            int newsCount = 0;
            DataSet newsDs = null;

            if (_newsType == "wenzhang")
            {
                var carTypeIdList = new List<int>
                    {
                        (int)CarNewsType.serialfocus, //add 2016-10-11 by gux,liub
                        //(int) CarNewsType.shijia,
                        (int) CarNewsType.pingce,
                        (int) CarNewsType.daogou,
                        (int) CarNewsType.yongche,
                        //(int) CarNewsType.gaizhuang,
                        //(int) CarNewsType.anquan,
                        (int) CarNewsType.xinwen,
                        //(int) CarNewsType.pingce,
                        //(int) CarNewsType.treepingce,
                        (int) CarNewsType.keji,
                        (int) CarNewsType.wenhua
                    };
                newsDs = new CarNewsBll().GetSerialNewsAllData(_serialId, carTypeIdList, _pageSize, _pageIndex, ref newsCount);
            }
            else
            {
                newsDs = new CarNewsBll().GetSerialNewsAllData(_serialId, _carNewsType, _pageSize, _pageIndex, ref newsCount);
            }


            _currentNewsCount = newsCount;
            ViewData["NewsCount"] = newsCount;
            ViewData["PageSize"] = 10;

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
                    if (_newsType != "hangqing")
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
        /// 截取指定长度字符串(按字节算)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string StrCut(string str, int length)
        {
            int len = 0;
            byte[] b;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                b = Encoding.Default.GetBytes(str.Substring(i, 1));
                if (b.Length > 1)
                    len += 2;
                else
                    len++;

                if (len > length)
                {
                    sb.Append("...");
                    break;
                }

                sb.Append(str[i]);
            }
            return sb.ToString();
        }
    }
}
