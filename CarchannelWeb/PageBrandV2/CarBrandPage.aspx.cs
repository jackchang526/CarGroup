using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BitAuto.CarChannel.CarchannelWeb.PageBrandV2
{
    public partial class CarBrandPage : PageBase
    {
        private CommonHtmlBll _commonhtmlBLL;
        private Car_MasterBrandBll _masterBrandBll;
        private Dictionary<int, string> dictBrandBlockHtml;//静态块内容

        protected int brandId = 0;
        protected string brandName = "";
        protected string brandSpell = "";
        protected string masterName = "";
        protected string masterSpell = "";


        protected string brandIntroduce = "";	//主品牌简介
        protected string brandLogoStory = "";	//车标故事
        protected string brandTopNews = "";	//品牌页顶部新闻
        // protected string brandNews = "";		//主品牌新闻
        protected string serialListByBrand = "";	//子品牌列表
        protected string strBrandSeoName = "";  //品牌名称
        protected string brandIntro = "";			//品牌介绍
        protected string logoStory = "";			//车标故事
        protected string imageHtml = "";			//图片
        protected string videoHtml = "";			//视频
        protected string koubeiHtml = "";			//口碑部分Html
        protected string askEntriesHtml = "";		//答疑Html
        protected string forumHtml = "";			//论坛Html
        protected string vendorsHtml = "";			//经销商Html
        protected string usecarHtml = "";			//二手车信息
        //protected string guilderString = "";        //导般条定字符串
        protected string _BrandRankingHTML = string.Empty;//得到指定品牌的排行
        protected string hotMasterBrandHtml = string.Empty;//热门主品牌

        protected string brandHotSerial = "";
        private Car_BrandBll cbb = new Car_BrandBll();
        private List<NameObject> serialImageList;	//子品牌的图片列表
        private List<NameObject> serialKoubeiList;	//子品牌口碑列表
        protected int masterId = 0;
        protected string FriendLinkNew = string.Empty;
        protected bool IsHasFriendLink = false;
        private int _PageSize = 6;
        private int _maxNewsCount = 8;
        private List<string> ListForNewsTitle = new List<string>();

        protected string logo55 = string.Empty;
        protected string logo100 = string.Empty;

		protected BrandEntity brandEntity = null;

        // protected bool _isZhihuanData = false;

        public CarBrandPage()
        {
            _masterBrandBll = new Car_MasterBrandBll();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
            Response.ContentType = "text/html";
            GetParameter();
            serialImageList = new List<NameObject>();
            serialKoubeiList = new List<NameObject>();

            //静态块内容
            _commonhtmlBLL = new CommonHtmlBll();
            dictBrandBlockHtml = _commonhtmlBLL.GetCommonHtml(brandId, CommonHtmlEnum.TypeEnum.Brand, CommonHtmlEnum.TagIdEnum.BrandPageV2);

            RenderIntroduce();
            //RenderNews();
            RenderNewsNew();
            RenderBrandSerials();
            // RenderImages();
            //RenderVideos();
            RenderVideosNew();
            // RenderKoubei();
            // RenderAsk();
            // 经销商
            //RenderDealer();
            // RenderForum();
            //RenderUsecar();
            //InitGuilderString();
            RenderBrandRanking();
            // 主品牌 品牌定制友情链接
            // modified by chengl Jun.3.2011
            //RenderFriendLinkNew();
            // 品牌下热门子品牌
            RenderBrandHotSerials();
            //热门主品牌
            RenderHotMasterBrand();
        }

        private void GetParameter()
        {
            brandId = ConvertHelper.GetInteger(Request.QueryString["cbid"]);
        }

        /// <summary>
        /// 生成简介与车标故事
        /// </summary>
        private void RenderIntroduce()
        {
            //加载json文件
            string sPath = WebConfig.DataBlockPath + "Data\\Yichehui\\FlagshipUrl.json";
            string jsonStr = CommonFunction.GetFileContent(sPath);
            string flagShipUrl = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                //解析json
                JObject jObject = (JObject)JsonConvert.DeserializeObject(jsonStr);
                JToken jTokenMaster = jObject["brand"];
                foreach (JObject jObjectMaster in jTokenMaster)
                {
                    string id = jObjectMaster["id"].ToString();
                    if (id == brandId.ToString())
                    {
                        flagShipUrl = jObjectMaster["url"].ToString();
                    }
                }
            }
            CommonFunction commonFunction = new CommonFunction();
            brandEntity = (BrandEntity)DataManager.GetDataEntity(EntityType.Brand, brandId);
            if (brandEntity != null && brandEntity.Id > 0)
            {
                brandName = brandEntity.ShowName;
                strBrandSeoName = brandEntity.SeoName;
                masterId = brandEntity.MasterBrandId;
                masterName = brandEntity.MasterBrand.Name;
                masterSpell = brandEntity.MasterBrand.AllSpell;
                brandSpell = brandEntity.AllSpell;

                StringBuilder htmlCode = new StringBuilder();
                brandIntro = brandEntity.Introduction;
                logoStory = brandEntity.MasterBrand.LogoInfo;

                htmlCode.AppendLine("<div class=\"figure-140x140\">");
                htmlCode.AppendLine("<div class=\"figure\">");
                //midified by sk for wangsong1 20170427  单独处理长安铃木用品牌logo
                logo55 = brandId == 10038 ? "http://image.bitautoimg.com/bt/car/default/images/carimage/b_" + brandId + "_b100.jpg" : "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_" + masterId + "_55.png";
                logo100 = brandId == 10038 ? "http://image.bitautoimg.com/bt/car/default/images/carimage/b_" + brandId + "_b100.jpg" : "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_" + masterId + "_100.png";
                htmlCode.AppendLine("<a class=\"figure-inner\"><img src=\"" + logo100 + "\"/></a>");
                if (!string.IsNullOrEmpty(flagShipUrl))
                {
                    htmlCode.AppendLine(string.Format("<a class=\"btn btn-primary btn-block\" target=\"_blank\" href=\"{0}\">品牌旗舰店</a>", flagShipUrl));
                }
                htmlCode.AppendLine("</div>");
                htmlCode.AppendLine("<div class=\"desc\">");
                htmlCode.AppendLine(string.Format("<h5 class=\"brand-story\">{0}品牌故事</h5>", brandName));
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

                htmlCode.AppendLine(string.Format("<h5 class=\"logo-story\">{0}车标故事</h5>", brandName));
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
                brandIntroduce = htmlCode.ToString();
            }
        }

        #region 品牌新闻块

        /// <summary>
        /// 生成新闻Html
        /// </summary>
        private void RenderNewsNew()
        {
            StringBuilder htmlTitle = new StringBuilder();
            StringBuilder htmlCode = new StringBuilder();
            try
            {
                CarNewsBll newsBll = new CarNewsBll();
                DataSet ds = newsBll.GetTopBrandFocusNews(brandId, CarNewsType.xinwen, 20);
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
                //    {
                //        firstNewsRow = newsTable.Rows[0];
                //    }
                //}
                // 有最新新闻
                htmlTitle.AppendLine("<div class=\"section-header header2\"><div class=\"box\">");
                htmlTitle.AppendFormat("<h2><a href=\"/{0}/xinwen/\" target=\"_blank\">{1}-新闻</a></h2>", brandSpell, brandName);
                htmlTitle.AppendLine("</div>");
                htmlTitle.AppendLine("<div class=\"more\">");
                htmlTitle.AppendFormat("<a href=\"/{0}/xinwen/\" target=\"_blank\">新闻</a>", brandSpell);
                htmlTitle.AppendFormat("<a href=\"/{0}/daogou/\" target=\"_blank\">导购</a>", brandSpell);
                htmlTitle.AppendFormat("<a href=\"/{0}/pingce/\" target=\"_blank\">评测</a>", brandSpell);
                htmlTitle.AppendFormat("<a href=\"/{0}/yongche/\" target=\"_blank\">用车</a>", brandSpell);
                htmlTitle.AppendFormat("<a href=\"/{0}/wenzhang/\" target=\"_blank\">更多&gt;&gt;</a>", brandSpell);
                htmlTitle.AppendLine("</div>");
                htmlTitle.AppendLine("</div>");

                htmlCode.Append("<div id=\"data_table_BrandNews_0\" class=\"list-txt list-txt-m list-txt-default\">");
                htmlCode.AppendLine("<ul>");
                int i = 0;
                string newsTitle = string.Empty;
                string newsUrl = string.Empty;
                if (firstNewsRow != null)
                {
                    newsTitle = CommonFunction.NewsTitleDecode(firstNewsRow["FaceTitle"].ToString());
                    ListForNewsTitle.Add(newsTitle);

                    newsUrl = firstNewsRow["filepath"].ToString();
                    htmlCode.AppendFormat("<li><div class=\"txt\"><a href=\"{0}\" target=\"_blank\">{1}</a></div><span>{2}</span></li>"
                                           , newsUrl, newsTitle, Convert.ToDateTime(firstNewsRow["publishtime"]).ToString("yyyy-MM-dd"));

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

                brandTopNews = htmlTitle.ToString() + htmlCode.ToString();

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

                _sbContent.AppendLine("<div id=\"data_table_BrandNews_" + loop.ToString() + "\" style=\"display: none;\"><div class=\"mainlist_box topa2 car_zs0519_01\">");
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

                    //XmlNode cateNode = xNodeList[i].SelectSingleNode("CategoryPath");
                    //string catePath = "";
                    //if (cateNode != null)
                    //{ catePath = cateNode.InnerText; }
                    //string newsCategory = Car_SerialBll.GetNewsKind(catePath);

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

                _sbContent.AppendFormat("<div id=\"data_table_BrandNews_{0}\" style=\"display: none;\"><div class=\"mainlist_box topa2 car_zs0519_01\">", loop.ToString());

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

                    _sbContent.AppendFormat("<li><a href=\"{0}\" title=\"{1}\">{1}</a>", filepath, title);
                    _sbContent.AppendFormat("<small>{0}</small></li>", newsDate);

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
            List<News> newsList = new CarNewsBll().GetBrandJiangJiaTopNews(brandId, 0, 0, 7, 1);
            if (newsList != null && newsList.Count > 0)
            {
                _sbTitle.Append("<li class=\"\"><a>降价</a></li>");

                _sbContent.AppendFormat("<div id=\"data_table_BrandNews_{0}\" style=\"display: none;\"><div class=\"mainlist_box topa2 car_zs0519_01\">", loop.ToString());

                _sbContent.AppendLine("<ul class=\"list_date\">");

                News newsRow = null;
                for (int i = 0; i < newsList.Count; i++)
                {
                    newsRow = newsList[i];
                    string title = newsRow.Title;
                    string filepath = newsRow.PageUrl;
                    string newsDate = newsRow.PublishTime.ToString("MM-dd");

                    _sbContent.AppendFormat("<li><a href=\"{0}\" title=\"{1}\">{1}</a>", filepath, title);
                    _sbContent.AppendFormat("<small>{0}</small></li>", newsDate);
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
        /// 生成品牌的子品牌列表,分在销与停销
        /// </summary>
        private void RenderBrandSerials()
        {
            StringBuilder htmlCode = new StringBuilder();
            List<CarSerialPhotoEntity> serialList = new Car_BrandBll().GetCarSerialPhotoListByCBID(brandId, true);
            List<CarSerialPhotoEntity> stopSaleList = new List<CarSerialPhotoEntity>();
            for (int i = serialList.Count - 1; i >= 0; i--)
            {
                CarSerialPhotoEntity serialEntry = serialList[i];
                if (serialEntry.SaleState == "停销")
                {
                    stopSaleList.Insert(0, serialEntry);
                    serialList.Remove(serialEntry);
                }
            }
            htmlCode.AppendLine("<div class=\"section-header header2 mb0 h-default\">");
            htmlCode.AppendLine("<div class=\"box\"><h2>" + brandName + "-全部车型</h2>");
            htmlCode.AppendLine("</div>");
            htmlCode.AppendLine("</div>");
            htmlCode.AppendLine("<div class=\"list-box col4-180-box clearfix\">");
            //在销车型
            if (serialList.Count > 0)
            {
                foreach (CarSerialPhotoEntity serialEntry in serialList)
                {
                    // if (serialEntry.SerialLevel == "概念车" || serialEntry.SerialLevel == "皮卡")
                    if (serialEntry.SerialLevel == "概念车")
                        continue;
                    // modifed by chengl Apr.16.2010
                    if (serialEntry.CS_Name.IndexOf("停用") >= 0)
                    { continue; }

                    serialImageList.Add(new NameObject(serialEntry.SerialId, serialEntry.ShowName, serialEntry.CS_AllSpell));
                    serialKoubeiList.Add(new NameObject(serialEntry.SerialId, serialEntry.ShowName, serialEntry.CS_AllSpell));

                    string serilaUrl = "/" + serialEntry.CS_AllSpell + "/";
                    string levelUrl = "";
                    //if (serialEntry.SerialLevel != "概念车")
                    //{
                    levelUrl = "/" + Car_LevelBll.GetLevelSpellByFullName(serialEntry.SerialLevel) + "/";
                    //}
                    if (serialEntry.SaleState == "在销")
                    {
                        //改为指导价
                        serialEntry.SaleState = base.GetSerialReferPriceByID(serialEntry.SerialId);
                        if (serialEntry.SaleState.Trim().Length == 0)
                            serialEntry.SaleState = "<small>暂无指导价</small>";
                        else
                        {
                            // serialEntry.SaleState = "<a href=\"/" + serialEntry.CS_AllSpell + "/baojia/\" target=\"_blank\" rel=\"nofollow\">" + serialEntry.SaleState + "</a>";
                            serialEntry.SaleState = serialEntry.SaleState;
                        }
                    }
                    else if (serialEntry.SaleState == "待销")
                        serialEntry.SaleState = "<small>未上市</small>";
                    string imageUrl = Car_SerialBll.GetSerialCoverHashImgUrl(serialEntry.SerialId);

                    htmlCode.AppendLine("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
                    htmlCode.AppendLine("<div class=\"img\"><a href=\"" + serilaUrl + "\" target=\"_blank\"><img src=\"" + imageUrl.Replace("_2.jpg", "_3.jpg") + "\" alt=\"" + serialEntry.CS_Name + "\" /></a></div>");
                    htmlCode.AppendLine("<ul class=\"p-list\"><li class=\"name\"><a href=\"" + serilaUrl + "\" target=\"_blank\">" + serialEntry.CS_Name + "</a></li>");
                    htmlCode.AppendLine("<li class=\"price\"><a href=\"" + serilaUrl + "\" target=\"_blank\">" + serialEntry.SaleState + "</a></li>");
                    htmlCode.AppendLine("</ul>");
                    htmlCode.AppendLine("</div>");
                }
            }
            //停销车型
            if (stopSaleList.Count > 0)
            {
                foreach (CarSerialPhotoEntity serialEntry in stopSaleList)
                {
                    if (serialEntry.SerialLevel == "概念车" || serialEntry.SerialLevel == "皮卡")
                        continue;
                    // modifed by chengl Apr.16.2010
                    if (serialEntry.CS_Name.IndexOf("停用") >= 0)
                    { continue; }

                    // 无图片的
                    if (serialEntry.CS_ImageUrl.ToLower().IndexOf("150-100.gif") > 0)
                    { continue; }

                    serialKoubeiList.Add(new NameObject(serialEntry.SerialId, serialEntry.ShowName, serialEntry.CS_AllSpell));

                    string serilaUrl = "/" + serialEntry.CS_AllSpell + "/";
                    string levelUrl = "/" + Car_LevelBll.GetLevelSpellByFullName(serialEntry.SerialLevel) + "/";
                    string imageUrl = Car_SerialBll.GetSerialCoverHashImgUrl(serialEntry.SerialId);

                    htmlCode.AppendLine("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
                    htmlCode.AppendLine("<div class=\"img\"><a href=\"" + serilaUrl + "\" target=\"_blank\"><img src=\"" + imageUrl.Replace("_2.jpg", "_3.jpg") + "\" alt=\"" + serialEntry.CS_Name + "\" /></a></div>");
                    string showName = serialEntry.ShowName.Replace("(进口)", "");
                    string shortcsName = StringHelper.SubString(showName, 11, true);
                    if (shortcsName.StartsWith(showName) || shortcsName.Length - showName.Length > 1)
                        shortcsName = showName;
                    if (shortcsName != showName)
                        htmlCode.AppendLine("<ul class=\"p-list\"><li class=\"name\"><a href=\"" + serilaUrl + "\" target=\"_blank\">" + shortcsName + "</a></li>");
                    else
                        htmlCode.AppendLine("<ul class=\"p-list\"><li class=\"name\"><a href=\"" + serilaUrl + "\" target=\"_blank\">" + showName + "</a></li>");
                    htmlCode.AppendLine("<li class=\"price\"><small>停售</small>" + "</li>");
                    htmlCode.AppendLine("</ul>");
                    htmlCode.AppendLine("</div>");
                }
            }
            htmlCode.AppendLine("</div>");
            htmlCode.AppendLine("<div class=\"more ad22017\"><ins id=\"div_e2e467ea-524d-4424-8368-03cf82f82a43\" type=\"ad_play\" adplay_IP=\"\" adplay_AreaName=\"\"  adplay_CityName=\"\"    adplay_BrandID=\""
                + brandId.ToString() + "\"  adplay_BrandName=\"\"  adplay_BrandType=\"\"  adplay_BlockCode=\"e2e467ea-524d-4424-8368-03cf82f82a43\"> </ins></div>");
            serialListByBrand = htmlCode.ToString();
        }



        /// <summary>
        /// 生成单个子品牌的图片的Html
        /// </summary>
        /// <param name="serialObj"></param>
        /// <param name="serialImageList"></param>
        /// <returns></returns>
        private string GetSerialImageHtml(NameObject serialObj, XmlNodeList imgNodeList)
        {
            StringBuilder htmlCode = new StringBuilder();

            htmlCode.AppendLine("<ul>");

            string serialUrl = "/" + serialObj.ObjectSpell + "/";
            int counter = 0;
            foreach (XmlElement imgNode in imgNodeList)
            {
                counter++;
                if (counter > 5)
                    break;
                int imgId = ConvertHelper.GetInteger(imgNode.SelectSingleNode("SiteImageId").InnerText);
                string imgUrl = imgNode.SelectSingleNode("SiteImageUrl").InnerText;
                string imgName = "";
                if (imgNode.SelectSingleNode("PropertyName") != null && imgNode.SelectSingleNode("PropertyName").HasChildNodes)
                {
                    imgName = imgNode.SelectSingleNode("PropertyName").InnerText;
                }
                string realImgUrl = new OldPageBase().GetPublishImage(2, imgUrl, imgId);
                htmlCode.AppendLine("<li><a href=\"http://photo.bitauto.com/picture/" + serialObj.ObjectId.ToString() + "/" + imgId.ToString() + "\" target=\"_blank\"><img src=\"" + realImgUrl + "\" alt=\"" + serialObj.ObjectName + " " + imgName + "\"");
                htmlCode.AppendLine(" width=\"120\" height=\"80\" /></a><div class=\"w120\"><a href=\"http://photo.bitauto.com/picture/" + serialObj.ObjectId.ToString() + "/" + imgId.ToString() + "\" target=\"_blank\">" + serialObj.ObjectName + "</a></div></li>");
            }
            htmlCode.AppendLine("</ul>");
            return htmlCode.ToString();
        }

        /// <summary>
        /// 生成视频部分
        /// </summary>
        private void RenderVideosNew()
        {
            #region modified by sk 2013-09-13 废弃旧视频获取
            /*
			 StringBuilder htmlCode = new StringBuilder();
			DataSet ds = new CarNewsBll().GetTopBrandNewsAllData(brandId, CarNewsType.video, 10);

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataTable table = ds.Tables[0];

				htmlCode.AppendLine("<div class=\"line_box zppvideo\">");
				htmlCode.AppendFormat("<h3><span><a target=\"_blank\" href=\"http://v.bitauto.com/car/brand/{0}.html\">{1}-视频</a></span></h3>", brandId.ToString(), brandName.Replace("·", "&bull;"));
				htmlCode.AppendLine("<UL>");

				foreach (DataRow videoRow in table.Rows)
				{
					string videoTitle = CommonFunction.NewsTitleDecode(videoRow["title"].ToString());
					string shortTitle = CommonFunction.NewsTitleDecode(videoRow["facetitle"].ToString());
					shortTitle = StringHelper.SubString(StringHelper.RemoveHtmlTag(shortTitle), 16, true);

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
				htmlCode.AppendFormat("<div class=\"more\"><a target=\"_blank\" href=\"http://v.bitauto.com/car/brand/{0}.html\" rel=\"nofollow\">更多>></a></div>", brandId.ToString());
				htmlCode.AppendLine("</div>");
			}
			videoHtml = htmlCode.ToString();
			 */
            #endregion
            int video = (int)CommonHtmlEnum.BlockIdEnum.Video;
            if (dictBrandBlockHtml.ContainsKey(video))
                videoHtml = dictBrandBlockHtml[video];
        }
        /// <summary>
        /// 生成口碑部分
        /// </summary>
        private void RenderKoubei()
        {
            StringBuilder htmlCode = new StringBuilder();
            serialKoubeiList.Sort(NameObject.Comparer);
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
                    sh.HtmlCode += "<td class=\"name\"><a href=\"/" + sh.ObjectSpell + "/koubei/\" target=\"_blank\">" + sh.ObjectName + "：</a></td>";
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
                htmlCode.AppendLine("<h2><span>" + brandName.Replace("·", "&bull;") + "口碑</span></h2>");
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
        //private void RenderAsk()
        //{
        //	string askHTMLFile = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\AskForCar\\Brand\\Html\\{0}.htm", brandId));
        //	if (File.Exists(askHTMLFile))
        //	{ askEntriesHtml = File.ReadAllText(askHTMLFile); }

        //	//StringBuilder htmlCode = new StringBuilder();
        //	//XmlNamespaceManager xnm = null;
        //	//XmlNodeList askList = new Car_BrandBll().GetBrandAskEntries(brandId, out xnm);
        //	//if (askList.Count > 0)
        //	//{
        //	//    htmlCode.AppendLine("<div class=\"line_box ppdayi\">");
        //	//    htmlCode.AppendLine("<h3><span><a href=\"http://ask.bitauto.com/search?keyword=" + Server.UrlEncode(brandName) + "\" target=\"_blank\">" + brandName + "汽车答疑</a></span></h3>");
        //	//    htmlCode.AppendLine("<ul>");

        //	//    foreach (XmlElement askNode in askList)
        //	//    {

        //	//        string askTitle = askNode.SelectSingleNode("atom:title", xnm).InnerText;
        //	//        askTitle = new CommonFunction().DropHTML(askTitle);
        //	//        string shortTitle = StringHelper.SubString(askTitle, 48, true);
        //	//        if (shortTitle.StartsWith(askTitle))
        //	//            shortTitle = askTitle;
        //	//        string askUrl = askNode.SelectSingleNode("atom:link", xnm).Attributes["href"].Value;
        //	//        if (shortTitle == askTitle)
        //	//            htmlCode.AppendLine("<li><a href=\"" + askUrl + "\" target=\"_blank\">" + askTitle + "</a></li>");
        //	//        else
        //	//            htmlCode.AppendLine("<li><a href=\"" + askUrl + "\" title=\"" + askTitle + "\" target=\"_blank\">" + shortTitle + "</a></li>");
        //	//    }
        //	//    htmlCode.AppendLine("</ul>");
        //	//    htmlCode.AppendLine("<div class=\"clear\"></div>");
        //	//    htmlCode.AppendLine("<div class=\"more\"><a href=\"http://ask.bitauto.com/search?keyword=" + Server.UrlEncode(brandName) + "\" target=\"_blank\">更多>></a></div>");
        //	//    htmlCode.AppendLine("</div>");
        //	//}
        //	//askEntriesHtml = htmlCode.ToString();
        //}

        /// <summary>
        /// 生成经销商
        /// </summary>
        private static string RenderDealer(HttpContext context)
        {
            int bid = ConvertHelper.GetInteger(context.Request.QueryString["cbid"]);
            string bSpell = "";
            string bName = "";
            DataSet brandDs = new Car_BrandBll().GetCarBrandInfoByCBID(bid);
            if (brandDs != null && brandDs.Tables.Count > 0 && brandDs.Tables[0].Rows.Count > 0)
            {
                bName = brandDs.Tables[0].Rows[0]["cb_name"].ToString().Trim();
                bSpell = brandDs.Tables[0].Rows[0]["allSpell"] == DBNull.Value ? "" : Convert.ToString(brandDs.Tables[0].Rows[0]["allSpell"]).ToLower();
            }
            int cityId = 201;
            if (context.Request.Cookies["bitauto_ipregion"] != null && context.Request.Cookies["bitauto_ipregion"].Value != "")
            {
                string cookieTemp = context.Server.UrlDecode(context.Request.Cookies["bitauto_ipregion"].Value);
                if (cookieTemp != "" && cookieTemp.IndexOf(";") > 0)
                {
                    string[] arrCityInfo = cookieTemp.Split(';');
                    if (arrCityInfo.Length > 1 && arrCityInfo[1].IndexOf(",") > 0)
                    {
                        string[] arrCityID = arrCityInfo[1].Split(',');
                        if (arrCityID.Length > 2)
                        {
                            if (Int32.TryParse(arrCityID[0], out cityId))
                            {
                                if (cityId <= 0)
                                {
                                    cityId = 201;
                                }
                            }
                        }
                    }
                }
            }
            // DataSet dsDealer = base.GetBrandDealerInfoByCbID(brandId);
            DataSet dsDealer = new PageBase().GetBrandCityDealerInfoByCbID(bid, cityId);
            StringBuilder htmlCode = new StringBuilder();
            if (dsDealer != null && dsDealer.Tables.Count > 0 && dsDealer.Tables[0].Rows.Count > 0)
            {
                htmlCode.AppendLine("<div class=\"line_box\">");
                htmlCode.AppendLine("<h2><span><a target=\"_blank\" href=\"http://dealer.bitauto.com/" + bSpell + "/\">" + bName.Replace("·", "&bull;") + "经销商</a></span></h2>");
                htmlCode.AppendLine("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"table_dealer\">");
                htmlCode.AppendLine("<thead><tr><th width=\"17%\">经销商</th><th width=\"34%\">电话</th><th width=\"40%\">促销信息</th></tr></thead>");
                htmlCode.AppendLine("<tbody>");
                for (int i = 0; i < dsDealer.Tables[0].Rows.Count; i++)
                {
                    string vendorName = "";
                    string shortName = "";
                    string nameTitle = "";

                    vendorName = dsDealer.Tables[0].Rows[i]["vendorName"].ToString();
                    shortName = StringHelper.SubString(vendorName, 16, true);
                    if (vendorName != shortName)
                    { nameTitle = " title=\"" + vendorName + "\" "; }

                    htmlCode.AppendLine("<tr class=\"dealer_tr\">");
                    htmlCode.AppendLine("<td><a target=\"_blank\" href=\"http://dealer.bitauto.com/" + dsDealer.Tables[0].Rows[i]["vendorID"].ToString() + "/\" " + nameTitle + " >" + shortName + "</a><a href=\"javascript:void(0);\" onclick=\"showDealerInfo(this," + dsDealer.Tables[0].Rows[i]["vendorID"].ToString() + ")\" title=\"经销商名片\" class=\"dealer_com\">经销商名片</a></td>");

                    string str400 = new PageBase().GetDealerFor400(dsDealer.Tables[0].Rows[i]["vendorID"].ToString());
                    if (str400 != "")
                    {
                        htmlCode.Append("<td><span class=\"official\" title=\"易车网认证电话，请放心拨打！\">" + str400 + "</span></td>");
                    }
                    else
                    {
                        htmlCode.Append("<td>" + dsDealer.Tables[0].Rows[i]["vendorTel"].ToString() + "</td>");
                    }
                    if (dsDealer.Tables[0].Columns.Contains("newsurl") && dsDealer.Tables[0].Columns.Contains("newsTitle"))
                    {
                        htmlCode.Append("<td><a target=\"_blank\" href=\"http://dealer.bitauto.com" + dsDealer.Tables[0].Rows[i]["newsurl"].ToString() + "\">" + dsDealer.Tables[0].Rows[i]["newsTitle"].ToString() + "</a></td>");
                    }
                    else
                    {
                        htmlCode.Append("<td></td>");
                    }
                    htmlCode.AppendLine("</tr>");
                }
                htmlCode.AppendLine("</tbody></table>");
                htmlCode.AppendLine("<div class=\"more\"><a target=\"_blank\" href=\"http://dealer.bitauto.com/" + bSpell + "/\">更多>></a></div>");
                htmlCode.AppendLine("</div>");
            }
            return htmlCode.ToString();
        }

        ///// <summary>
        ///// 生成论坛代码
        ///// </summary>
        //private void RenderForum()
        //{
        //	StringBuilder htmlCode = new StringBuilder();
        //	htmlCode.AppendLine("<div class=\"line_box ppbbs\">");

        //	BrandForum bf = new Car_BrandBll().GetBrandForm("brand", brandId);

        //	//获取大本营地址
        //	string campUrl = bf.CampForumUrl;
        //	if (campUrl.Length > 0)
        //		htmlCode.AppendLine("<h3><span><a href=\"" + campUrl + "\" target=\"_blank\">" + brandName + "汽车论坛</a></span></h3>");
        //	else
        //		htmlCode.AppendLine("<h3><span>" + brandName + "汽车论坛</span></h3>");
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


        ///// <summary>
        ///// 生成二手车代码
        ///// </summary>
        //private void RenderUsecar()
        //{
        //    usecarHtml = "";
        //    //获取数据
        //    XmlDocument xmlDoc = new Car_BrandBll().GetUCarInfo(brandId, "Brand");
        //    XmlNodeList carList = xmlDoc.SelectNodes("/NewDataSet/item");
        //    if (carList.Count == 0)
        //        return;

        //    // modified by chengl Sep.29.2011
        //    // StringBuilder htmlCode = new StringBuilder();
        //    List<string> listHTML = new List<string>();
        //    listHTML.Add("<div class=\"line_box ucar_box\">");
        //    listHTML.Add("<h3><a target=\"_blank\" href=\"http://www.taoche.com/buycar/brand/" + brandSpell + "/\">二手" + brandName.Replace("·", "&bull;") + "</a></h3>");
        //    listHTML.Add("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");
        //    listHTML.Add("<tbody><tr>");
        //    // listHTML.Add("<th width=\"16%\" style=\"text-align:left\">年份</th>");
        //    listHTML.Add("<th width=\"46%\" style=\"text-align:left\">车源信息</th>");
        //    listHTML.Add("<th width=\"25%\">地区</th>");
        //    listHTML.Add("<th width=\"20%\">价格</th>");
        //    listHTML.Add("</tr>");
        //    //这里生成列表
        //    int counter = 0;
        //    foreach (XmlElement carNode in carList)
        //    {
        //        string city = carNode.SelectSingleNode("CityName").InnerText;
        //        if (carNode.SelectSingleNode("CityUrL") == null)
        //        { continue; }
        //        string cityUrL = carNode.SelectSingleNode("CityUrL").InnerText;
        //        // string producerName = carNode.SelectSingleNode("ProducerName").InnerText;
        //        string serialName = carNode.SelectSingleNode("BrandName").InnerText;
        //        string carName = carNode.SelectSingleNode("CarName").InnerText;
        //        string buyCarDate = carNode.SelectSingleNode("BuyCarDate").InnerText;
        //        if (buyCarDate.IndexOf("年") >= 0)
        //        {
        //            buyCarDate = buyCarDate.Substring(0, buyCarDate.IndexOf("年") + 1);
        //        }
        //        // string carColr = carNode.SelectSingleNode("Color").InnerText; ;
        //        // string carMile = carNode.SelectSingleNode("DrivingMileage").InnerText;
        //        string carPrice = carNode.SelectSingleNode("DisplayPrice").InnerText;
        //        // string vendorName = carNode.SelectSingleNode("VendorFullName").InnerText;
        //        string ucarUrl = carNode.SelectSingleNode("CarlistUrl").ChildNodes[0].Value;

        //        listHTML.Add("<tr>");
        //        // listHTML.Add("<td style=\"text-align:left\">" + buyCarDate + "</td>");
        //        listHTML.Add("<td style=\"text-align:left\"><a title=\"" + serialName + " " + carName + "\" target=\"_blank\" href=\"" + ucarUrl + "\" class=\"car_name w100\">" + serialName + " " + carName + "</a></td>");
        //        listHTML.Add("<td class=\"cgray\"><a target=\"_blank\" href=\"" + cityUrL + "\">" + city + "</a></td>");
        //        listHTML.Add("<td class=\"ucar_price\">" + carPrice + "</td>");
        //        listHTML.Add("</tr>");
        //        counter++;
        //        // if (counter >= 5)
        //        if (counter >= 10)
        //        { break; }
        //    }
        //    listHTML.Add("</tbody></table>");
        //    listHTML.Add("</div>");
        //    usecarHtml = string.Concat(listHTML.ToArray());
        //}
        /// <summary>
        /// 生成导航条字符串
        /// </summary>
        /// <returns></returns>
        private void InitGuilderString()
        {
            //guilderString = new Car_BrandBll().GetRelationHeader(brandId, brandName, brandSpell, masterId, "brand", "");
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
        /// 品牌下热门子品牌
        /// </summary>
        private void RenderBrandHotSerials()
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
                brandHotSerial = sb.ToString();
            }
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
                    sb.AppendFormat("<div class=\"figure-55-55\"><a target=\"_blank\"  class=\"figure\" href=\"/{0}/\"><img src=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_{1}_55.png\" alt=\"{2}\" /></a><a class=\"title\" href=\"/{0}/\" target=\"_blank\">{2}</a></div>", dr["urlspell"], dr["bs_Id"], dr["bs_Name"]);
                }
            }
            hotMasterBrandHtml = sb.ToString();
        }
        /// <summary>
        /// 品牌 主品牌定制友情链接
        /// </summary>
        private void RenderFriendLinkNew()
        {
            FriendLinkNew = String.Empty;
            string ids = ",10005,10008,10021,10038,10046,10065,20063,20109,";
            if (ids.IndexOf("," + brandId.ToString() + ",") >= 0)
            {
                string filePath = Server.MapPath(string.Format("~/include/special/seo/00001/car_link_{0}_Manual.shtml", brandId.ToString()));
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

        #region 暂时不用了
        /*
	/// <summary>
	/// 置换块所用的城市信息
	/// </summary>
	protected List<City> GetZhiHuanCityList()
	{
		if (!_isZhihuanData) return null;
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