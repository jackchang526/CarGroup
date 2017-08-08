using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BitAuto.CarChannel.CarchannelWeb.PageMasterV2
{
    public partial class CarMasterPage : PageBase
    {
        private CommonHtmlBll _commonhtmlBLL;
        private Car_MasterBrandBll _masterBrandBll;
        private Dictionary<int, string> dictMasterBlockHtml;//静态块内容

        protected int masterId = 0;			//主品牌ID
        protected string masterName = "";	//主品牌名称
        protected string strMasterSeoName = "";
        protected string masterUrlSpell = string.Empty; // 主品牌全拼
        protected string masterCountry = string.Empty;//主品牌 进口 国产
        protected string masterIntroduce = "";	//主品牌简介
        protected string masterLogoStory = "";	//车标故事
        protected string masterTopNews = "";	//主品牌页顶部新闻
        protected string masterNews = "";		//主品牌新闻
        protected string serialListByBrand = "";	//子品牌列表
        protected string brandIntro = "";			//主品牌介绍
        protected string logoStory = "";			//车标故事
        protected string imageHtml = "";			//图片
        protected string videoHtml = "";			//视频
        protected string koubeiHtml = "";			//口碑部分Html
        protected string askEntriesHtml = "";		//答疑Html
        protected string forumHtml = "";			//论坛Html
        protected string vendorsHtml = "";			//经销商Html
        protected string venderScript = "";			//生成经销商的脚本
        protected string usecarHtml = "";			//二手车Html
        //protected string guilderString = "";        //导航条
        protected string FirstBrandSpell = "";        //第一个品牌SPELL
        protected string _BrandRankingHTML = string.Empty;
        protected string masterHotSerial = "";   // 主品牌热门子品牌
        protected string HotImageJs;
        protected string brandListDealerHtml = string.Empty;//品牌经销商
        protected string hotMasterBrandHtml = string.Empty;//热门主品牌

        // private List<NameObject> serialImageList;	//子品牌的图片列表
        private List<NameObject> serialImageListSale;	        //子品牌在销的图片列表
        private List<NameObject> serialImageListWaitSale;	//子品牌待销的图片列表
        private List<NameObject> serialImageListStopSale;	//子品牌停销的图片列表
        private List<NameObject> serialKoubeiList;	//子品牌口碑列表
        private List<NameObject> brandList;			//品牌列表
        protected List<string> brandIds = new List<string>();

        private Car_BrandBll cbb = new Car_BrandBll();
        private int _PageSize = 6;
        private int _maxNewsCount = 8;
        private List<string> ListForNewsTitle = new List<string>();

        private int firstBrandId = 0;				//第一个品牌ID，为二手车更多页准备，因为二手车没有主品牌这个级别
        protected string FriendLinkNew = string.Empty;
        protected bool IsHasFriendLink = false;

        protected bool _isZhihuanData = false;

        // 只有1个品牌的时候 的品牌ID 广告使用 add by chengl Jun.5.2013
        protected int OnlyOneCbID = 0;

        protected string longTitleCss = string.Empty;

        public CarMasterPage()
        {
            _masterBrandBll = new Car_MasterBrandBll();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
            Response.ContentType = "text/html";
            GetParameter();

            //静态块内容
            _commonhtmlBLL = new CommonHtmlBll();
            dictMasterBlockHtml = _commonhtmlBLL.GetCommonHtml(masterId, CommonHtmlEnum.TypeEnum.Master, CommonHtmlEnum.TagIdEnum.MasterBrandPageV2);
            // serialImageList = new List<NameObject>();
            serialImageListSale = new List<NameObject>();
            serialImageListWaitSale = new List<NameObject>();
            serialImageListStopSale = new List<NameObject>();

            serialKoubeiList = new List<NameObject>();
            brandList = new List<NameObject>();

            RenderIntroduction();
            //RenderNews();
            RenderNewsNew();
            RenderBrandList();
            RenderDealerByBrand();
            // RenderImages();
            //RenderVideos();
            RenderVideosNew();
            // RenderKoubei();
            RenderAsk();
            // RenderForum();
            // RenderVendors();
            //RenderUsecar();
            //InitGuilderString();
            RenderBrandRanking();
            // 主品牌 品牌定制友情链接
            // modified by chengl Jun.3.2011
            //RenderFriendLinkNew();
            // 主品牌热门子品牌
            RenderMasterHotSerials();
            //热门主品牌
            RenderHotMasterBrand();
        }

        private void GetParameter()
        {
            masterId = ConvertHelper.GetInteger(Request.QueryString["bsid"]);
        }

        /// <summary>
        /// 生成简介与车标故事
        /// </summary>
        private void RenderIntroduction()
        {
            //加载json文件
            string sPath = Path.Combine(WebConfig.DataBlockPath, "Data\\Yichehui\\FlagshipUrl.json");
            string jsonStr = CommonFunction.GetFileContent(sPath);
            string flagShipUrl = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                //解析json
                JObject jObject = (JObject)JsonConvert.DeserializeObject(jsonStr);
                JToken jTokenMaster = jObject["master"];
                foreach (JObject jObjectMaster in jTokenMaster)
                {
                    string id = jObjectMaster["id"].ToString();
                    if (id == masterId.ToString())
                    {
                        flagShipUrl = jObjectMaster["url"].ToString();
                    }
                }
            }
            StringBuilder htmlCode = new StringBuilder();
            DataRow drInfo = new Car_BrandBll().GetCarMasterBrandInfoByBSID(masterId);
            if (drInfo == null)
            { return; }
            masterName = drInfo["bs_name"].ToString().Trim();
            masterUrlSpell = drInfo["urlspell"].ToString().ToLower();
            masterCountry = ConvertHelper.GetString(drInfo["bs_Country"]) != "中国" ? "进口" : "";
            CommonFunction commonFunction = new CommonFunction();
            //品牌简介
            brandIntro = drInfo["bs_introduction"] == DBNull.Value ? "" : Convert.ToString(drInfo["bs_introduction"]).Trim();
            //车标故事
            logoStory = drInfo["bs_logoinfo"] == DBNull.Value ? "" : Convert.ToString(drInfo["bs_logoinfo"]).Trim();

            htmlCode.AppendLine("<div class=\"figure-140x140\">");
            htmlCode.AppendLine("<div class=\"figure\">");
            htmlCode.AppendLine("<a class=\"figure-inner\"><img src=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_" + masterId + "_100.png\" onerror=\"javascript:this.src=''\"/></a>");
            if (!string.IsNullOrEmpty(flagShipUrl))
            {
                htmlCode.AppendLine(string.Format("<a class=\"btn btn-primary btn-block\" target=\"_blank\" href=\"{0}\">品牌旗舰店</a>", flagShipUrl));
            }
            htmlCode.AppendLine("</div>");

            htmlCode.AppendLine("<div class=\"desc\">");
            htmlCode.AppendLine(string.Format("<h5 class=\"brand-story\">{0}品牌故事</h5>", masterName));
            string shortIntro = string.Empty;
            if (string.IsNullOrEmpty(brandIntro))
            {
                htmlCode.AppendLine("<p class=\"no-story-txt\">抱歉，还没有该品牌的品牌故事！</p>");
            }
            else
            {
                shortIntro = commonFunction.GetShortString(brandIntro, 70);
                if (shortIntro != brandIntro)
                {
                    htmlCode.AppendLine(string.Format("<p id=\"shortBrandStory\" style=\"height: 40px;\">{0}<a href=\"javascript:;\" onclick=\"BrandZhanKai();\" target=\"_self\" class=\"more\">[展开+]</a></p>", shortIntro));
                    htmlCode.AppendLine(string.Format("<p id=\"detailBrandStory\" style=\"height: auto;display:none;\">{0}<a href=\"javascript:;\" onclick=\"BrandShouQi();\" target=\"_self\" class=\"more\">[收起-]</a></p>", brandIntro));
                }
                else
                {
                    htmlCode.AppendLine(string.Format("<p id=\"pBrandStory\" style=\"height: 40px;\"><em id=\"emBrandStory\">{0}</em></p>", shortIntro));
                }
            }
           
            htmlCode.AppendLine(string.Format("<h5 class=\"logo-story\">{0}车标故事</h5>", masterName));
            if (string.IsNullOrEmpty(logoStory))
            {
                htmlCode.AppendLine("<p class=\"no-story-txt\">抱歉，还没有该品牌的车标故事！</p>");
            }
            else
            {
                shortIntro = commonFunction.GetShortString(logoStory, 70);
                if (shortIntro != logoStory)
                {
                    htmlCode.AppendLine(string.Format("<p id=\"shortLogoStory\" style=\"height: 40px;\">{0}<a href=\"javascript:;\" onclick=\"CheBiaoZhanKai();\" target=\"_self\" class=\"more\">[展开+]</a></p>", shortIntro));
                    htmlCode.AppendLine(string.Format("<p id=\"detailLogoStory\" style=\"height: auto;display:none;\">{0}<a href=\"javascript:;\" onclick=\"CheBiaoShouQi();\" target=\"_self\" class=\"more\">[收起-]</a></p>", logoStory));
                }
                else
                {
                    htmlCode.AppendLine(string.Format("<p id=\"pBrandStory\" style=\"height: 40px;\">{0}</p>", shortIntro));
                }
            }
            htmlCode.AppendLine("</div>");
            htmlCode.AppendLine("</div>");
            masterIntroduce = htmlCode.ToString();
        }

        #region 主品牌新闻块


        /// <summary>
        /// 生成主品牌新闻
        /// </summary>
        private void RenderNewsNew()
        {
            StringBuilder htmlTitle = new StringBuilder();
            StringBuilder htmlCode = new StringBuilder();
            try
            {
                CarNewsBll newsBll = new CarNewsBll();
                DataSet ds = newsBll.GetTopMasterBrandFocusNews(masterId, CarNewsType.xinwen, 20);
                if (ds == null || ds.Tables.Count <= 1 || ds.Tables[0].Rows.Count <= 0)
                    return;

                DataRow firstNewsRow = null;
                DataTable newsOrderTable = ds.Tables[1];
                DataTable newsTable = ds.Tables[0];
                if (newsOrderTable.Rows.Count > 0)
                {
                    if (newsOrderTable.Rows[0]["OrderNumber"].ToString() == "1")
                    {
                        firstNewsRow = newsOrderTable.Rows[0];
                    }
                }
                //if (firstNewsRow == null)
                //{
                //    firstNewsRow = base.GetTopNewsFirstRow(newsTable, new int[] { 33, 31, 32, 4, 179, 102, 115, 120, 29, 30 });
                //    if (firstNewsRow == null)
                //        firstNewsRow = newsTable.Rows[0];
                //}

                // 有最新新闻
                htmlTitle.AppendLine("<div class=\"section-header header2\"><div class=\"box\">");
                htmlTitle.AppendFormat("<h2><a href=\"/{0}/xinwen/\" target=\"_blank\">{1}-新闻</a></h2>", masterUrlSpell, masterName);
                htmlTitle.AppendLine("</div>");
                htmlTitle.AppendLine("<div class=\"more\">");
                htmlTitle.AppendFormat("<a href=\"/{0}/xinwen/\" target=\"_blank\">新闻</a>", masterUrlSpell);
                htmlTitle.AppendFormat("<a href=\"/{0}/daogou/\" target=\"_blank\">导购</a>", masterUrlSpell);
                htmlTitle.AppendFormat("<a href=\"/{0}/pingce/\" target=\"_blank\">评测</a>", masterUrlSpell);
                htmlTitle.AppendFormat("<a href=\"/{0}/yongche/\" target=\"_blank\">用车</a>", masterUrlSpell);
                htmlTitle.AppendFormat("<a href=\"/{0}/wenzhang/\" target=\"_blank\">更多&gt;&gt;</a>", masterUrlSpell);
                htmlTitle.AppendLine("</div>");
                htmlTitle.AppendLine("</div>");

                //int loop = 0;
                htmlCode.Append("<div id=\"data_table_MasterNews_0\"  class=\"list-txt list-txt-m list-txt-default\">");
                //loop++;

                htmlCode.AppendLine("<ul>");
                int i = 0;
                string newsTitle = string.Empty;
                string newsUrl = string.Empty;
                if (firstNewsRow != null)
                {
                    newsTitle = firstNewsRow["FaceTitle"].ToString();
                    newsUrl = firstNewsRow["filepath"].ToString();
                    //过滤Html标签
                    newsTitle = CommonFunction.NewsTitleDecode(newsTitle);

                    ListForNewsTitle.Add(newsTitle);

                    htmlCode.AppendFormat("<li><div class=\"txt\"><a href=\"{0}\" target=\"_blank\">{1}</a></div><span>{2}</span></li>"
                                           , newsUrl
                                           , newsTitle
                                           , Convert.ToDateTime(firstNewsRow["publishtime"]).ToString("yyyy-MM-dd"));
                    i++;
                }
                foreach (DataRow newsRow in newsTable.Rows)
                {
                    if (i >= _maxNewsCount)
                    { break; }

                    if (newsOrderTable.Rows.Count > 0)
                    {
                        while (true)
                        {
                            DataRow[] rows = newsOrderTable.Select("OrderNumber=" + (i + 1));
                            if (rows.Length > 0)
                            {
                                DataRow newsOrderRow = rows[0];
                                newsTitle = CommonFunction.NewsTitleDecode(newsOrderRow["FaceTitle"].ToString());
                                if (!ListForNewsTitle.Contains(newsTitle))
                                {
                                    ListForNewsTitle.Add(newsTitle);
                                    htmlCode.AppendFormat("<li><div class=\"txt\"><a href=\"{0}\" target=\"_blank\">{1}</a></div><span>{2}</span></li>"
                                        , newsOrderRow["filepath"].ToString()
                                        , newsTitle
                                        , Convert.ToDateTime(newsOrderRow["publishtime"]).ToString("yyyy-MM-dd"));
                                    i++;
                                }
                                else
                                    break;
                                if (i >= _maxNewsCount)
                                { break; }
                            }
                            else
                                break;
                        }

                        if (i >= _maxNewsCount)
                        { break; }
                    }

                    newsTitle = CommonFunction.NewsTitleDecode(newsRow["FaceTitle"].ToString());
                    if (!ListForNewsTitle.Contains(newsTitle))
                    {
                        ListForNewsTitle.Add(newsTitle);
                        newsUrl = newsRow["filepath"].ToString();
                        string newsDate = Convert.ToDateTime(newsRow["publishtime"]).ToString("yyyy-MM-dd");
                        htmlCode.AppendFormat("<li><div class=\"txt\"><a href=\"{0}\" target=\"_blank\">{1}</a></div><span>{2}</span></li>", newsUrl, newsTitle, newsDate);
                        i++;
                    }
                }
                htmlCode.AppendLine("</ul>");
                htmlCode.AppendLine("</div>");
                masterTopNews = htmlTitle.ToString() + htmlCode.ToString();
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// 根据新闻分类取分类内容
        /// </summary>
        /// <param name="doc">新闻内容块</param>
        /// <param name="cate">分类</param>
        /// <param name="_sbTitle">标头分类标签</param>
        /// <param name="_sbContent">分类的新闻内容</param>
        [Obsolete("新闻服务上线后，将由GetCateNewXmlContentNew方法代替。")]
        private void GetCateNewXmlContent(XmlDocument doc, string cate, ref int loop, ref StringBuilder _sbTitle, ref StringBuilder _sbContent)
        {
            if (doc != null && doc.HasChildNodes)
            {
                XmlNodeList xNodeList = doc.SelectNodes("root/listnews");
                if (xNodeList == null || xNodeList.Count < 1) return;

                _sbTitle.AppendLine("<li class=\"\"><a>" + cate + "</a></li>");

                _sbContent.AppendLine("<div id=\"data_table_MasterNews_" + loop.ToString() + "\" style=\"display: none;\"><div class=\"mainlist_box topa2 car_zs0519_01\">");
                int isFirst = 0;
                for (int i = 0; i < xNodeList.Count; i++)
                {
                    if (isFirst >= _maxNewsCount + 1)
                    { break; }
                    string title = xNodeList[i].SelectSingleNode("title").InnerText;
                    if (ListForNewsTitle.Contains(title))
                    { continue; }
                    string filepath = xNodeList[i].SelectSingleNode("filepath").InnerText;
                    string newsDate = Convert.ToDateTime(xNodeList[i].SelectSingleNode("publishtime").InnerText).ToString("MM-dd");

                    if (isFirst == 0)
                    {
                        // 
                        _sbContent.AppendLine("<ul class=\"list_date\">");
                        // _sbContent.AppendLine("<h2><a target=\"_blank\" href=\"" + filepath + "\">" + title + "</a></h2>");
                    }
                    //else if (isFirst == 1)
                    //{
                    //    _sbContent.AppendLine("<ul class=\"list_date\">");
                    //    _sbContent.AppendLine("<li><a href=\"" + filepath + "\" title=\"" + title + "\">" + title + "</a>");
                    //    _sbContent.AppendLine("<small>" + newsDate + "</small></li>");
                    //}
                    //else
                    //{
                    _sbContent.AppendLine("<li><a href=\"" + filepath + "\" title=\"" + title + "\">" + title + "</a>");
                    _sbContent.AppendLine("<small>" + newsDate + "</small></li>");
                    //}
                    isFirst++;
                }
                if (isFirst >= 1)
                { _sbContent.AppendLine("</ul>"); }
                _sbContent.AppendLine("<div class=\"clear\"></div>");
                _sbContent.AppendLine("</div>");
                _sbContent.AppendLine("</div>");
                loop++;
            }
        }
        /// <summary>
        /// 根据新闻分类取分类内容
        /// </summary>
        /// <param name="doc">新闻内容块</param>
        /// <param name="cate">分类</param>
        /// <param name="_sbTitle">标头分类标签</param>
        /// <param name="_sbContent">分类的新闻内容</param>
        private void GetCateNewXmlContentNew(DataSet newsDataSet, string cate, ref int loop, ref StringBuilder _sbTitle, ref StringBuilder _sbContent)
        {
            if (newsDataSet != null && newsDataSet.Tables.Count > 0 && newsDataSet.Tables[0].Rows.Count > 0)
            {
                DataTable table = newsDataSet.Tables[0];
                _sbTitle.AppendFormat("<li class=\"\"><a>{0}</a></li>", cate);

                _sbContent.AppendFormat("<div id=\"data_table_MasterNews_{0}\" style=\"display: none;\"><div class=\"mainlist_box topa2 car_zs0519_01\">", loop.ToString());
                int isFirst = 0;
                DataRow newsRow = null;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (isFirst >= _maxNewsCount + 1)
                    { break; }
                    newsRow = table.Rows[i];
                    string title = CommonFunction.NewsTitleDecode(newsRow["title"].ToString());
                    if (ListForNewsTitle.Contains(title))
                    { continue; }
                    string filepath = newsRow["filepath"].ToString();
                    string newsDate = Convert.ToDateTime(newsRow["publishtime"]).ToString("MM-dd");

                    if (isFirst == 0)
                    {
                        _sbContent.AppendLine("<ul class=\"list_date\">");
                    }

                    _sbContent.AppendFormat("<li><a href=\"{0}\" title=\"{1}\">{1}</a><small>{2}</small></li>", filepath, title, newsDate);
                    isFirst++;
                }
                if (isFirst >= 1)
                { _sbContent.AppendLine("</ul>"); }
                _sbContent.AppendLine("<div class=\"clear\"></div>");
                _sbContent.AppendLine("</div>");
                _sbContent.AppendLine("</div>");
                loop++;
            }
        }
        /// <summary>
        /// 降价新闻
        /// </summary>
        private void GetCateJiangJiaNews(ref int loop, ref StringBuilder _sbTitle, ref StringBuilder _sbContent)
        {
            List<News> newsList = new CarNewsBll().GetMasterBrandJiangJiaTopNews(masterId, 0, 0, 7, 1);
            if (newsList != null && newsList.Count > 0)
            {
                _sbTitle.Append("<li class=\"\"><a>降价</a></li>");

                _sbContent.AppendFormat("<div id=\"data_table_MasterNews_{0}\" style=\"display: none;\"><div class=\"mainlist_box topa2 car_zs0519_01\">", loop.ToString());
                _sbContent.AppendLine("<ul class=\"list_date\">");

                News newsRow = null;
                for (int i = 0; i < newsList.Count; i++)
                {
                    newsRow = newsList[i];
                    string title = newsRow.Title;
                    string filepath = newsRow.PageUrl;
                    string newsDate = newsRow.PublishTime.ToString("MM-dd");


                    _sbContent.AppendFormat("<li><a href=\"{0}\" title=\"{1}\">{1}</a><small>{2}</small></li>", filepath, title, newsDate);
                }
                _sbContent.AppendLine("</ul>");
                _sbContent.AppendLine("<div class=\"clear\"></div>");
                _sbContent.AppendLine("</div>");
                _sbContent.AppendLine("</div>");
                loop++;
            }
        }
        #endregion

        /// <summary>
        /// 生成主品牌下各品牌的子品牌列表
        /// </summary>
        private void RenderBrandList()
        {
            StringBuilder htmlTitle = new StringBuilder();
            StringBuilder htmlCode = new StringBuilder();
            StringBuilder htmlAllSerial = new StringBuilder();
            StringBuilder htmlAllwaitSaleHtml = new StringBuilder();
            StringBuilder htmlAllnoPriceHtml = new StringBuilder();
            StringBuilder htmlAllstopSaleHtml = new StringBuilder();
            string allBrandHtml = "";

            DataSet brandDs = new Car_BrandBll().GetCarSerialPhotoListByBSID(masterId, true);
            if (brandDs != null && brandDs.Tables.Count > 0)
            {
                if (brandDs.Tables.Count > 4)
                {
                    longTitleCss = "title-box-long";
                }
                // 有品牌
                htmlTitle.AppendLine("<div class=\"section-header header2 mb0\">");
                htmlTitle.AppendLine("<div class=\"box\"><h2>" + masterName + "-车型</h2>");
                //变态逻辑 下不为例 add 2016.09.27
                if (masterId == 2)
                {
                    htmlTitle.AppendLine("<ul id=\"car_MasterSerialList_ul\" class=\"nav\" style=\"width: 590px;height: 24px;overflow: hidden;\">");
                }
                else
                {
                    htmlTitle.AppendLine("<ul id=\"car_MasterSerialList_ul\" class=\"nav\">");
                }
                int loop = 0;
                if (brandDs.Tables.Count > 1)
                {
                    htmlTitle.AppendLine("<li class=\"current\"><a target=\"_blank\" href=\"#\">全部</a></li>");
                    loop = 1;
                }

                // List<int> zhihuanList = cbb.GetZhiHuanList();
                Dictionary<int, City> cityList = AutoStorageService.GetZhiHuanShowCityList();
                foreach (DataTable brandTable in brandDs.Tables)
                {
                    if (brandTable.Rows.Count == 0)
                        continue;
                    string brandSpell = ConvertHelper.GetString(brandTable.Rows[0]["cbspell"]).Trim().ToLower();
                    int brandId = ConvertHelper.GetInteger(brandTable.Rows[0]["cb_id"]);
                    // 如果只有1个品牌 广告需要传品牌ID add by chengl Jun.5.2013 
                    if (brandDs.Tables.Count == 1)
                    { OnlyOneCbID = brandId; }
                    brandIds.Add(brandId.ToString());
                    if (firstBrandId == 0)
                        firstBrandId = brandId;
                    if (brandDs.Tables.Count > 1)
                    {
                        string brandUrl = "/" + brandSpell + "/";
                        htmlTitle.AppendLine("<li class=\"" + (brandDs.Tables.Count > 1 ? "" : "current") +
                                             "\"><a target=\"_blank\" href=\"" + brandUrl + "\">" + brandTable.TableName +
                                             "</a></li>");
                    }

                    brandList.Add(new NameObject(brandId, brandTable.TableName, brandSpell));

                    htmlCode.AppendLine("<div class=\"list-box col4-180-box clearfix\" id=\"data_table_MasterSerialList_" + loop.ToString() + "\" style=\"" + (brandDs.Tables.Count > 1 ? "display: none;" : "") + "\">");
                    loop++;
                    string waitSaleHtml = "";
                    string stopSaleHtml = "";
                    string noPriceHtml = "";

                    foreach (DataRow row in brandTable.Rows)
                    {
                        int serialId = ConvertHelper.GetInteger(row["cs_id"]);
                        string csName = ConvertHelper.GetString(row["cs_name"]);
                        string csShowName = ConvertHelper.GetString(row["cs_ShowName"]);
                        // modified by chengl Jun.3.2011
                        if (serialId == 1568)
                        { csShowName = "索纳塔八"; }
                        string csSpell = ConvertHelper.GetString(row["csspell"]).Trim().ToLower();
                        string imgUrl = ConvertHelper.GetString(row["csImageUrl"]).ToLower();
                        string csLevel = ConvertHelper.GetString(row["cslevel"]);
                        // if (csLevel == "概念车" || csLevel == "皮卡")
                        if (csLevel == "概念车")
                            continue;
                        // modifed by chengl Apr.16.2010
                        if (csName.IndexOf("停用") >= 0)
                        { continue; }

                        string sellState = ConvertHelper.GetString(row["CsSaleState"]);
                        string priceRange = sellState;

                        // 无图片的
                        if (sellState.Trim() == "停销" && imgUrl.IndexOf("150-100.gif") > 0)
                        { continue; }

                        string serialUrl = "/" + csSpell + "/";
                        string levelUrl = "";
                        //if (csLevel != "概念车")
                        //{
                        levelUrl = "/" + Car_LevelBll.GetLevelSpellByFullName(csLevel) + "/";
                        //}
                        // string levelUrl = "/" + Car_LevelBll.GetLevelSpellByFullName(csLevel) + "/";
                        string priceUrl = "/" + csSpell + "/baojia/";

                        if (sellState == "在销")
                        {
                            priceRange = base.GetSerialPriceRangeByID(serialId);
                            if (priceRange.Trim().Length == 0)
                                priceRange = "<small>暂无报价</small>";
                        }
                        else if (sellState == "待销")
                        {
                            priceRange = "<small>未上市</small>";
                        }
                        else
                        {
                            // 停销
                            priceRange = "<small>停售</small>";
                        }
                        StringBuilder serialNodeCode = new StringBuilder();
                        serialNodeCode.AppendLine("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
                        serialNodeCode.AppendLine("<div class=\"img\"><a href=\"" + serialUrl + "\" target=\"_blank\"><img src=\"" + imgUrl.Replace("_1.jpg", "_3.jpg") + "\" alt=\"" + csShowName + "\"  /></a></div>");
                        serialNodeCode.AppendLine("<ul class=\"p-list\">");
                        serialNodeCode.AppendLine("<li class=\"name\"><a target=\"_blank\" href=\"" + serialUrl + "\">" + csShowName + "</a></li>");
                        serialNodeCode.AppendLine("<li class=\"price\"><a target=\"_blank\" href=\"" + serialUrl + "\">" + priceRange + "</a></li>");
                        serialNodeCode.AppendLine("</ul>");
                        serialNodeCode.AppendLine("</div>");


                        string showName = csShowName;//.Replace("(进口)", "");
                        string shortcsName = StringHelper.SubString(showName, 11, true);
                        if (shortcsName.StartsWith(showName) || shortcsName.Length - showName.Length > 1)
                            shortcsName = showName;

                        # region
                        if (sellState == "在销")
                        {
                            if (priceRange == "暂无报价")
                            {
                                noPriceHtml += serialNodeCode.ToString();
                                if (brandDs.Tables.Count > 1)
                                {
                                    htmlAllnoPriceHtml.AppendLine(serialNodeCode.ToString());
                                }
                            }
                            else
                            {
                                //!!!!!!!!!!!!!!!!!!!!!!!!!!
                                //htmlCode.AppendLine(serialNodeCode.ToString());
                                if (brandDs.Tables.Count > 1)
                                {
                                    htmlAllSerial.AppendLine(serialNodeCode.ToString());
                                }
                            }
                        }
                        else if (sellState == "待销")
                        {
                            waitSaleHtml += serialNodeCode.ToString();
                            if (brandDs.Tables.Count > 1)
                            {
                                htmlAllwaitSaleHtml.AppendLine(serialNodeCode.ToString());
                            }
                        }
                        else
                        {
                            stopSaleHtml += serialNodeCode.ToString();
                            if (brandDs.Tables.Count > 1)
                            {
                                htmlAllstopSaleHtml.AppendLine(serialNodeCode.ToString());
                            }
                        }

                        //图片部分
                        if (imgUrl != WebConfig.DefaultCarPic)
                        {
                            //图片数
                            int picCount = 0;
                            string tempUrl = "";
                            base.GetSerialPicAndCountByCsID(serialId, out tempUrl, out picCount, false);
                            if (picCount > 0)
                            {
                                serialUrl += "tupian/";
                                NameObject sh = new NameObject(serialId, csShowName, csSpell);
                                // sh.HtmlCode = "<li><a href=\"" + serialUrl + "\" target=\"_blank\"><img src=\"" + imgUrl.Replace("_1.jpg", "_2.jpg") + "\" alt=\"" + csShowName + "\" width=\"120\" height=\"80\" /></a>";
                                sh.HtmlCode = "<li><a href=\"" + serialUrl + "\" target=\"_blank\"><img src=\"" + tempUrl + "\" alt=\"" + csShowName + "\" width=\"120\" height=\"80\" /></a>";
                                string shortName = StringHelper.SubString(csShowName, 17, true);
                                if (shortName.StartsWith(csShowName) || shortName.Length - csShowName.Length > 1)
                                    shortName = csShowName;
                                sh.HtmlCode += "<div class=\"w120\">";
                                if (shortName == csShowName)
                                    sh.HtmlCode += "<a href=\"" + serialUrl + "\" target=\"_blank\">" + csShowName + "</a>";
                                else
                                    sh.HtmlCode += "<a href=\"" + serialUrl + "\" title=\"" + csShowName + "\" target=\"_blank\">" + shortName + "</a>";

                                sh.HtmlCode += "</div><span>" + picCount + "张</span></li>";
                                if (sellState == "在销")
                                {
                                    // 在销
                                    serialImageListSale.Add(sh);
                                }
                                else if (sellState == "待销")
                                {
                                    // 待销
                                    serialImageListWaitSale.Add(sh);
                                }
                                else
                                {
                                    // 停销
                                    serialImageListStopSale.Add(sh);
                                }
                                // serialImageList.Add(sh);
                            }
                        }

                        //口碑列表
                        NameObject kbSh = new NameObject(serialId, csShowName, csSpell);
                        serialKoubeiList.Add(kbSh);

                        #endregion

                        htmlCode.AppendLine(serialNodeCode.ToString());
                    }

                    //htmlCode.AppendLine(noPriceHtml);
                    //htmlCode.AppendLine(waitSaleHtml);
                    //htmlCode.AppendLine(stopSaleHtml);

                    htmlCode.AppendLine("</div>");
                }
                //htmlCode.Append("</div>");
                htmlTitle.AppendLine("</ul></div></div>");
                //if (masterName == "一汽")
                //{
                //	htmlTitle.Append("</div>");
                //}
                if (brandDs.Tables.Count > 1)
                {
                    allBrandHtml = "<div id=\"data_table_MasterSerialList_0\" class=\"list-box col4-180-box clearfix\" >" + htmlAllSerial.ToString() + htmlAllnoPriceHtml.ToString() + htmlAllwaitSaleHtml.ToString() + htmlAllstopSaleHtml.ToString() + "</div>";
                }
            }
            serialListByBrand = htmlTitle + allBrandHtml + htmlCode;
        }
        /// <summary>
        /// 经销商块
        /// </summary>
        private void RenderDealerByBrand()
        {
            StringBuilder sb = new StringBuilder();
            MasterBrandEntity masterEntity = (MasterBrandEntity)DataManager.GetDataEntity(EntityType.MasterBrand, masterId);
            if (masterEntity != null && masterEntity.BrandList.Length > 0)
            {
                StringBuilder tabBrandHtml = new StringBuilder();
                List<string> brandListHtml = new List<string>();
                tabBrandHtml.Append("<div class=\"section-header header2 mb0\"><div class=\"box\">");
                tabBrandHtml.Append(string.Format("<h2><a href=\"http://dealer.bitauto.com/{0}/\" >{1}-经销商推荐</a></h2>", masterUrlSpell, masterName));
                if (masterEntity.BrandList.Count() > 1)
                {
                    tabBrandHtml.Append("<ul class=\"nav\" id=\"salesAgentRec\">");   //car_MasterSerialList_ul  salesAgentRec
                }
                int i = 0;
                foreach (BrandEntity brand in masterEntity.BrandList)
                {
                    string brandUrl = "http://dealer.bitauto.com/" + brand.AllSpell + "/";
                    if (masterEntity.BrandList.Count() > 1&&masterEntity.BrandList.Count()<=3)
                    {
                        tabBrandHtml.Append(
                            string.Format(
                                "<li class=\"tabBrand {0}\"><a  href=\"http://dealer.bitauto.com/{1}/\" target=\"_blank\" data-index=\"{3}\">{2}</a></li>",
                                i == 0 ? "current" : "", brand.AllSpell, brand.ShowName,i));
                    }
                    else if (masterEntity.BrandList.Count() > 3)
                    {
                        if (i < 2)
                        {
                            tabBrandHtml.Append(
                                string.Format(
                                    "<li class=\"tabBrand {0}\"><a  href=\"http://dealer.bitauto.com/{1}/\" target=\"_blank\" data-index=\"{3}\">{2}</a></li>",
                                    i == 0 ? "current" : "", brand.AllSpell, brand.ShowName,i));
                        }
                        else
                        {
                            if (i == 2)
                            {
                                tabBrandHtml.Append("<li class=\"drop-layer-box\" id=\"moreSalesAgentRec\" ><a  href=\"javascript:void(0);\" class=\"arrow-down\">更多经销商推荐</a><div class=\"drop-layer\" style=\"top: 24px; width:160px;\">");
                            }
                            tabBrandHtml.Append(string.Format("<a class=\"tabBrand\" href=\"http://dealer.bitauto.com/{0}/\" target=\"_blank\" data-index=\"{2}\">{1}</a>",
                                                             brand.AllSpell, brand.ShowName,i));
                            if (i == masterEntity.BrandList.Count())
                            {
                                tabBrandHtml.Append("</div></li>");
                            }
                        }
                    }
                    else
                    { }

                    brandListHtml.Add(string.Format("<div id=\"data_table_MasterDealer_{0}\" class=\"row\" style=\"display: {1};\">", i, i == 0 ? "block" : "none"));
                    brandListHtml.Add(string.Format("<script type=\"text/javascript\">document.writeln(\"<ins Id=\\\"ep_union_{1}\\\" Partner=\\\"1\\\" Version=\\\"\\\" isUpdate=\\\"1\\\" type=\\\"1\\\" city_type=\\\"1\\\" city_id=\\\"\" + bit_locationInfo.cityId + \"\\\" city_name=\\\"0\\\" car_type=\\\"1\\\" brandId=\\\"{0}\\\" serialId=\\\"0\\\" carId=\\\"0\\\"></ins>\");</script>", brand.Id, i==0?140:(143+i)));
                    brandListHtml.Add("<div class=\"hideline\">");
                    brandListHtml.Add("</div>");
                    brandListHtml.Add("</div>");
                    i++;
                }

                if (masterEntity.BrandList.Count() > 1)
                {
                    tabBrandHtml.Append("</ul>");
                }
                tabBrandHtml.Append("</div></div>");

                sb.Append(tabBrandHtml);

                sb.Append(string.Concat(brandListHtml.ToArray()));
            }
            brandListDealerHtml = sb.ToString();
        }


        /// <summary>
        /// 生成视频
        /// </summary>
        private void RenderVideosNew()
        {
            #region modified by sk 2013-09-13 废除视频读取
            /*
			 StringBuilder htmlCode = new StringBuilder();
			DataSet ds = new CarNewsBll().GetTopMasterBrandNewsAllData(masterId, CarNewsType.video, 10);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataTable table = ds.Tables[0];

				htmlCode.AppendLine("<div class=\"line_box zppvideo\">");
				htmlCode.AppendFormat("<h3><span><a target=\"_blank\" href=\"http://v.bitauto.com/car/master/{0}.html\">{1}-汽车视频</a></span></h3>", masterId.ToString(), masterName);
				htmlCode.AppendLine("<UL>");

				foreach (DataRow videoRow in table.Rows)
				{
					string videoTitle = CommonFunction.NewsTitleDecode(videoRow["title"].ToString());
					string faceTitle = CommonFunction.NewsTitleDecode(videoRow["facetitle"].ToString());
					string shortTitle = StringHelper.SubString(StringHelper.RemoveHtmlTag(faceTitle), 16, true);
					if (shortTitle.StartsWith(faceTitle) || shortTitle.Length - faceTitle.Length > 1)
						shortTitle = faceTitle;

					string imgUrl = videoRow["picture"].ToString();
					if (imgUrl.Trim().Length == 0)
						imgUrl = WebConfig.DefaultVideoPic;
					string filepath = videoRow["filepath"].ToString();

					string duration = videoRow["duration"].ToString().Trim();

					htmlCode.AppendFormat(
						"<li><a href=\"{0}\" target=\"_blank\" class=\"v_bg\" alt=\"视频播放\" rel=\"nofollow\"></a><a href=\"{0}\" target=\"_blank\" rel=\"nofollow\"><img src=\"{1}\" alt=\"{2}\" width=\"120\" height=\"80\" /></a>",
						filepath, imgUrl, videoTitle);

					htmlCode.AppendFormat(
						"<a class=\"title\" href=\"{0}\" title=\"{1}\" target=\"_blank\">{2}</a><div>时长：{3}</div></li>",
						filepath, videoTitle, shortTitle, duration);
				}
				htmlCode.AppendLine("</UL>");
				htmlCode.AppendFormat("<div class=\"more\"><a target=\"_blank\" href=\"http://v.bitauto.com/car/master/{0}.html\" rel=\"nofollow\">更多>></a></div>", masterId.ToString());
				htmlCode.AppendLine("</div>");
			}
			videoHtml = htmlCode.ToString();
			 */
            #endregion
            int video = (int)CommonHtmlEnum.BlockIdEnum.Video;
            if (dictMasterBlockHtml.ContainsKey(video))
                videoHtml = dictMasterBlockHtml[video];
        }

        /// <summary>
        /// 生成口碑部分
        /// </summary>
        private void RenderKoubei()
        {
            StringBuilder htmlCode = new StringBuilder();
            //serialKoubeiList.Sort(NameObject.Comparer);
            List<NameObject> tempList = new List<NameObject>();

            int counter = 0;
            foreach (NameObject sh in serialKoubeiList)
            {
                DataSet koubieDs = base.GetSerialDianPingByCsID(sh.ObjectId);
                if (koubieDs != null && koubieDs.Tables.Count > 0 && koubieDs.Tables[0].Rows.Count > 0)
                {
                    if (sh.ObjectName.IndexOf("停用") >= 0)
                    { continue; }
                    sh.HtmlCode = "<tr>";
                    sh.HtmlCode += "<td style=\"width:180px;\" class=\"name\"><a href=\"/" + sh.ObjectSpell + "/koubei/\" target=\"_blank\">" + sh.ObjectName + "：</a></td>";
                    counter = 0;
                    foreach (DataRow row in koubieDs.Tables[0].Rows)
                    {
                        counter++;
                        string kbTitle = ConvertHelper.GetString(row["title"]);
                        kbTitle = new CommonFunction().DropHTML(kbTitle);
                        string shortTitle = StringHelper.SubString(kbTitle, 34, true);
                        if (shortTitle.StartsWith(kbTitle))
                            shortTitle = kbTitle;
                        string url = ConvertHelper.GetString(row["url"]);
                        if (shortTitle == kbTitle)
                            sh.HtmlCode += "<td><a href=\"" + url + "\" target=\"_blank\">" + kbTitle + "</a></td>";
                        else
                            sh.HtmlCode += "<td><a href=\"" + url + "\" title=\"" + kbTitle + "\" target=\"_blank\">" + shortTitle + "</a></td>";
                        if (counter >= 2)
                            break;
                    }
                    if (counter < 2)
                    {
                        sh.HtmlCode += "<td></td>";
                    }
                    sh.HtmlCode += "</tr>";
                    tempList.Add(sh);
                }
            }

            if (tempList.Count > 0)
            {

                htmlCode.AppendLine("<div class=\"line_box ppkoubei\">");
                htmlCode.AppendLine("<h2><span>" + masterName + "口碑</span></h2>");
                htmlCode.AppendLine("<div class=\"price_tab_table\">");
                htmlCode.AppendLine("<table width=\"100%\">");
                counter = 0;
                foreach (NameObject sh in tempList)
                {
                    counter++;
                    if (counter == tempList.Count)
                        htmlCode.AppendLine(sh.HtmlCode.Replace("<tr>", "<tr class=\"nobg\">"));
                    else
                        htmlCode.AppendLine(sh.HtmlCode);
                }
                htmlCode.AppendLine("</table>");
                htmlCode.AppendLine("</div></div>");
            }

            koubeiHtml = htmlCode.ToString();
            //DataSet GetSerialDianPingByCsID(int csID)
        }

        /// <summary>
        /// 生成答疑部分
        /// modified by chengl Jun.22.2011
        /// 答疑块改版
        /// </summary>
        private void RenderAsk()
        {
            //string askHTMLFile = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\AskForCar\\Masterbrand\\Html\\{0}.htm", masterId));
            //if (File.Exists(askHTMLFile))
            //{ askEntriesHtml = File.ReadAllText(askHTMLFile); }

            askEntriesHtml = _commonhtmlBLL.GetCommonHtmlByBlockId(masterId, CommonHtmlEnum.TypeEnum.Master, CommonHtmlEnum.TagIdEnum.MasterBrandPage, CommonHtmlEnum.BlockIdEnum.Ask);

        }

        ///// <summary>
        ///// 生成论坛代码
        ///// </summary>
        //private void RenderForum()
        //{
        //	StringBuilder htmlCode = new StringBuilder();
        //	htmlCode.AppendLine("<div class=\"line_box ppbbs\">");

        //	BrandForum bf = new Car_BrandBll().GetBrandForm("masterbrand", masterId);

        //	//获取大本营地址
        //	string campUrl = bf.CampForumUrl;
        //	if (campUrl.Length > 0)
        //		htmlCode.AppendLine("<h3><span><a href=\"" + campUrl + "\" target=\"_blank\">" + masterName + "汽车论坛</a></span></h3>");
        //	else
        //		htmlCode.AppendLine("<h3><span>" + masterName + "汽车论坛</span></h3>");
        //	htmlCode.AppendLine("<div class=\"price_tab_table\">");
        //	DataTable forumsTable = bf.ForumList;
        //	string baaMoreLink = string.Empty;

        //	DataTable subjectsTable = bf.SubjectList;

        //	if (forumsTable != null && forumsTable.Rows.Count > 0)
        //	{
        //		for (int i = 0; i < forumsTable.Rows.Count; i++)
        //		{
        //			if (i > 5)
        //			{ break; }
        //			if (i != 0)
        //			{
        //				baaMoreLink += "| <a target=\"_blank\" href=\"" + forumsTable.Rows[i]["url"].ToString() + "\">" + forumsTable.Rows[i]["ForumName"].ToString().Replace("易车会", string.Empty) + "</a> ";
        //			}
        //			else
        //			{
        //				baaMoreLink = "<a target=\"_blank\" href=\"" + forumsTable.Rows[i]["url"].ToString() + "\">" + forumsTable.Rows[i]["ForumName"].ToString().Replace("易车会", string.Empty) + "</a> ";
        //			}
        //		}
        //	}

        //	if (subjectsTable != null && subjectsTable.Rows.Count > 0)
        //	{
        //		htmlCode.AppendLine("<table width=\"100%\">");
        //		for (int i = 0; i < subjectsTable.Rows.Count; i++)
        //		{
        //			if (i == subjectsTable.Rows.Count - 1)
        //				htmlCode.Append("<tr class=\"nobg\">");
        //			else
        //				htmlCode.AppendLine("<tr>");
        //			htmlCode.AppendLine("<td class=\"name1\"><a target=\"_blank\" href=\"" + subjectsTable.Rows[i]["url"].ToString() + "\">" + StringHelper.SubString(new CommonFunction().DropHTML(subjectsTable.Rows[i]["title"].ToString().Trim()), 40, true) + "</a></td>");
        //			htmlCode.AppendLine("<td class=\"name2\"><a target=\"_blank\" href=\"" + subjectsTable.Rows[i]["furl"].ToString() + "\">" + subjectsTable.Rows[i]["ForumName"].ToString() + "</a></td>");
        //			htmlCode.AppendLine("<td>" + subjectsTable.Rows[i]["replies"].ToString() + "/" + subjectsTable.Rows[i]["views"].ToString() + "</td>");

        //			htmlCode.AppendLine("<td><a target=\"_blank\" href=\"" + subjectsTable.Rows[i]["postermyurl"].ToString() + "\">" + subjectsTable.Rows[i]["poster"].ToString() + "</a><div>" + Convert.ToDateTime(subjectsTable.Rows[i]["postdatetime"].ToString()).ToShortDateString() + "</div></td>");
        //			htmlCode.AppendLine("<td><a target=\"_blank\" href=\"" + subjectsTable.Rows[i]["lastpostermyurl"].ToString() + "\">" + subjectsTable.Rows[i]["lastposter"].ToString() + "</a><div>" + Convert.ToDateTime(subjectsTable.Rows[i]["lastpost"].ToString()).ToShortDateString() + "</div></td>");
        //			htmlCode.AppendLine("</tr>");
        //		}
        //		htmlCode.AppendLine("</table>");
        //	}

        //	htmlCode.AppendLine("<div class=\"more\">" + baaMoreLink + "</div>");
        //	htmlCode.AppendLine("<div class=\"clear\"></div>");
        //	htmlCode.AppendLine("</div></div>");

        //	forumHtml = htmlCode.ToString();
        //}

        private void RenderVendors()
        {
            StringBuilder htmlCode = new StringBuilder();
            StringBuilder scriptCode = new StringBuilder();
            foreach (NameObject no in brandList)
            {
                string divName = "venderInfo_" + no.ObjectId;
                string loaderName = "loader" + no.ObjectId;
                htmlCode.AppendLine("<div id=\"" + divName + "\" class=\"line_box\" ></div>");
                scriptCode.AppendLine("var " + loaderName + " = new DealerLoader(" + no.ObjectId + ",'" + divName + "');");
                scriptCode.AppendLine(loaderName + ".LoadBrandDealer();");
                //GetBrandDealerInfoByID(no.ObjectId, no.ObjectName, no.ObjectSpell.ToLower(), ref htmlCode);

                //htmlCode.AppendLine("<div class=\"line_box\">");
                //htmlCode.AppendLine("<h2 class=\"car\"><span><a href=\"\">" + no.ObjectName + "经销商</a></span></h2>");
                //htmlCode.AppendLine("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"table_dealer\">");
                //htmlCode.AppendLine("<thead><tr>");
                //htmlCode.AppendLine("<th width=\"17%\">经销商</th>");
                //htmlCode.AppendLine("<th width=\"34%\">电话</th>");
                //htmlCode.AppendLine("<th width=\"40%\">促销信息</th>");
                //htmlCode.AppendLine("</tr></thead>");
                //htmlCode.AppendLine("<tbody>");
                //htmlCode.AppendLine("</tbody>");
                //htmlCode.AppendLine("</table>");
                //htmlCode.AppendLine("<div class=\"more\"><a href=\"\">更多>></a></div>");
                //htmlCode.AppendLine("</div>");
            }
            vendorsHtml = htmlCode.ToString();
            venderScript = scriptCode.ToString();
        }


        /// <summary>
        /// 得到品牌的排行
        /// </summary>
        /// <returns></returns>
        private void RenderBrandRanking()
        {
            string cacheKey = "BrandPvRanking_60_Num";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                _BrandRankingHTML = (string)obj;
                return;
            }

            DataSet rankingDataSet = cbb.GetBrandPvRanking(60);
            if (rankingDataSet == null) return;
            List<string> contentlist = new List<string>();
            //添加头部的内容
            contentlist.Add("<div class=\"line_box\">");
            contentlist.Add("<h3>厂商关注排行榜</h3><div class=\"cpRankTab\" id=\"sub_ul\">");
            contentlist.Add("<ul><li class=\"current\">1-20名</li><li class=\"\">21-40名</li>");
            contentlist.Add("<li class=\"\" style=\"border-right:0\">41-60名</li></ul></div>");
            contentlist.Add("<div class=\"clear\"></div>");
            //拼接字符串
            for (int i = 0; i < 3; i++)
            {
                contentlist.Add(string.Format("<div class=\"cpRank\" id=\"sub_con_{0}\" {1}>", i, (i != 0 ? "style=\"display:none\"" : "")));
                int index = 0;
                for (int j = i * 20; j < (i + 1) * 20; j++)
                {
                    if (j % 10 == 0 && index != 0) contentlist.Add("</ul><ul>");
                    else if (j % 10 == 0) contentlist.Add("<ul>");
                    DataRow dr = rankingDataSet.Tables[0].Rows[j];
                    //拼接内容字符串
                    contentlist.Add(string.Format("<li><span class=\"num{0}\">{0}</span><a href=\"/{1}/\" title=\"{2}\" target=\"_blank\">{2}</a></li>"
                                    , (j + 1), dr["allspell"], dr["cb_name"]));
                    index++;
                    if (j + 1 == (i + 1) * 20) contentlist.Add("</ul>");
                }

                contentlist.Add("</div>");
            }

            //底部最后一个div
            contentlist.Add("</div>");
            //给字符串赋值
            _BrandRankingHTML = String.Concat(contentlist.ToArray());

            CacheManager.InsertCache(cacheKey, _BrandRankingHTML, 60 * 10);
        }
        /// <summary>
        /// 初始化导航条
        /// </summary>
        public void InitGuilderString()
        {
            //guilderString = new Car_BrandBll().GetRelationHeader(masterId, masterName, masterUrlSpell, 0, "masterbrand", "");
        }

        /// <summary>
        /// 主品牌下热门子品牌
        /// </summary>
        private void RenderMasterHotSerials()
        {

            StringBuilder sb = new StringBuilder();
            List<EnumCollection.SerialSortForInterface> listSFI = base.GetAllSerialNewly30DayToList();

            if (listSFI != null && listSFI.Count > 0)
            {
                int loop = 0;
                sb.AppendLine("<div class=\"line_box zs100412_1\">");
                sb.AppendLine("<h3><span>热门车型</span></h3>");
                sb.AppendLine("<ul class=\"pic_list\">");
                foreach (EnumCollection.SerialSortForInterface sfi in listSFI)
                {
                    if (loop >= 6)
                    { break; }
                    sb.AppendLine("<li><a target=\"_blank\" href=\"/" + sfi.CsAllSpell + "/\" rel=\"nofollow\">");
                    string defaultPic = "";
                    int picCount = 0;
                    this.GetSerialPicAndCountByCsID(sfi.CsID, out defaultPic, out picCount, true);
                    if (defaultPic.Trim().Length == 0)
                    { defaultPic = WebConfig.DefaultCarPic; }
                    sb.AppendLine("<img width=\"90\" height=\"60\" src=\"" + defaultPic.Replace("_2.", "_5.") + "\" alt=\"" + sfi.CsShowName + "\">");
                    sb.AppendLine("</a><a target=\"_blank\" href=\"/" + sfi.CsAllSpell + "/\">" + sfi.CsShowName + "</a>");
                    sb.AppendLine("<div>" + GetSerialPriceRangeByID(sfi.CsID) + "</div>");
                    sb.AppendLine("</li>");
                    loop++;
                }
                sb.AppendLine("</ul>");
                sb.AppendLine("<div class=\"hiedline\"></div>");
                sb.AppendLine("<div class=\"more\"><a href=\"#\"></a></div>");
                sb.AppendLine("<div class=\"clear\"></div>");
                sb.AppendLine("</div>");
                masterHotSerial = sb.ToString();
            }
            //StringBuilder sb = new StringBuilder();
            //DataSet dsHotSerial = new Car_BrandBll().GetMasterHotSerial(masterId, 6);
            //if (dsHotSerial != null && dsHotSerial.Tables.Count > 0 && dsHotSerial.Tables[0].Rows.Count > 0)
            //{
            //    sb.AppendLine("<div class=\"line_box zs100412_1\">");
            //    sb.AppendLine("<h3><span>" + masterName + "热门车型</span></h3>");
            //    sb.AppendLine("<ul class=\"pic_list\">");
            //    foreach (DataRow dr in dsHotSerial.Tables[0].Rows)
            //    {
            //        sb.AppendLine("<li><a target=\"_blank\" href=\"/" + dr["allSpell"].ToString() + "/\">");
            //        string defaultPic = "";
            //        int picCount = 0;
            //        this.GetSerialPicAndCountByCsID(int.Parse(dr["cs_id"].ToString()), out defaultPic, out picCount, true);
            //        if (defaultPic.Trim().Length == 0)
            //        { defaultPic = WebConfig.DefaultCarPic; }
            //        sb.AppendLine("<img width=\"90\" height=\"60\" src=\"" + defaultPic.Replace("_2.", "_5.") + "\" alt=\"" + dr["cs_showname"].ToString().Trim() + "\">");
            //        sb.AppendLine("</a><a target=\"_blank\" href=\"/" + dr["allSpell"].ToString() + "/\">" + dr["cs_showname"].ToString().Trim() + "</a>");
            //        sb.AppendLine("<div>" + GetSerialPriceRangeByID(int.Parse(dr["cs_id"].ToString())) + "</div>");
            //        sb.AppendLine("</li>");
            //    }
            //    sb.AppendLine("</ul>");
            //    sb.AppendLine("<div class=\"hiedline\"></div>");
            //    sb.AppendLine("<div class=\"more\"><a href=\"#\"></a></div>");
            //    sb.AppendLine("<div class=\"clear\"></div>");
            //    sb.AppendLine("</div>");
            //    masterHotSerial = sb.ToString();
            //}
        }
        /// <summary>
        /// 热门主品牌
        /// </summary>
        private void RenderHotMasterBrand()
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = _masterBrandBll.GetHotMasterBrand(24);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.AppendFormat("<div class=\"figure-55-55\">");
                    sb.AppendFormat("<a target=\"_blank\" class=\"figure\" href=\"/{0}/\"><img src=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_{1}_55.png\" alt=\"{2}\" /></a><a class=\"title\" href=\"/{0}/\" target=\"_blank\">{2}</a>", dr["urlspell"], dr["bs_Id"], dr["bs_Name"]);
                    sb.AppendFormat("</div>");
                }
            }
            hotMasterBrandHtml = sb.ToString();
        }
        /// <summary>
        /// 品牌 主品牌定制友情链接
        /// </summary>
        private void RenderFriendLinkNew()
        {
            string ids = ",2,6,7,10,15,16,17,18,19,25,28,30,32,34,49,78,80,82,84,85,88,91,96,98,99,100,103,108,111,113,127,141,143,";
            if (ids.IndexOf("," + masterId.ToString() + ",") >= 0)
            {
                string filePath = Server.MapPath(string.Format("~/include/special/seo/00001/car_link_{0}_Manual.shtml", masterId.ToString()));
                if (File.Exists(filePath))
                {
                    try
                    {
                        FriendLinkNew = File.ReadAllText(filePath, Encoding.GetEncoding("gb2312"));
                        IsHasFriendLink = true;
                    }
                    catch { }
                }
            }
        }
        #region 置换服务有数据的城市，暂时不用了
        /*
	/// <summary>
	/// 置换块所用的城市信息
	/// </summary>
	protected List<City> GetZhiHuanCityList(int brandId)
	{
		List<int> cityIds = cbb.GetZhiHuanCityIdList(brandId);
		if (cityIds != null && cityIds.Count > 0)
		{
			Dictionary<int, City> cityList = AutoStorageService.GetZhiHuanShowCityList();
			if (cityList != null && cityList.Count > 0)
			{
				List<City> newCityList = new List<City>(cityIds.Count);
				foreach (KeyValuePair<int, City> kv in cityList)
				{
					if (!cityIds.Contains(kv.Key)) continue;
					newCityList.Add(kv.Value);
				}
				return newCityList;
			}
		}
		return null;
	} 
	*/
        #endregion
    }
}