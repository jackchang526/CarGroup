using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using System.Linq;

namespace BitAuto.CarChannel.CarchannelWeb.PageSerialV2
{
    public partial class CsPingCe : PageBase
    {
        private const string _PingceEditorCommentHtmlPath =
            "Data\\SerialSet\\PingceEditorCommentHtml\\Serial_Comment_{0}.html";

        private const string _PingceEditorCommentHtmlPathV2 =
            "Data\\SerialSet\\PingceEditorCommentHtmlV2\\Serial_Comment_{0}.html";

        private readonly CommonHtmlBll _commonhtmlBLL;
        protected string _LeftTagContent = string.Empty;
        protected string _MorePingCeContent = string.Empty;
        protected string _PageDescribe = string.Empty;
        protected string _PageKeyword = string.Empty;
        /*
																   * _PageTitle:页面标题
																   * _PageKeyword:页面关键字
																   * _PageDescribe:页面描述
																   */
        protected string _PageTitle = string.Empty;
        private DataSet _PingCeDs = new DataSet();

        private int _PingCeNewsCount;
        private readonly string _PingCheImageFilePath = "Data\\SerialNews\\pingce\\Image\\{0}.xml";
        //得到视频块和图片地地址
        private readonly string _PingCheShiPinFilePath = "Data\\SerialNews\\pingce\\ShiPinNew\\{0}.xml";
        //子品牌的图片块
        protected string _SerialImageHtml = string.Empty;
        //子品牌的视频块
        protected string _SerialShiPinHtml = string.Empty;

        private readonly Dictionary<int, string> _TagTitleDescribeList = new Dictionary<int, string>();
        //标记当前新闻是否为第一条评测
        private int _Tholder;
        //年款ID
        protected int _YearId;
        //protected string newsNavHtml = string.Empty;
        protected string baaUrl;

        protected string BaseUrl = string.Empty;
        protected string carCompareHtml = string.Empty;
        protected string CompetitiveKoubeiHtml = string.Empty; //竞品口碑

        protected SerialEntity cse; //子品牌信息

        protected string CsHead = string.Empty;
        protected int csID;
        protected int currentPageIndex;
        //左侧滑动标签内容
        // private string[] tagList = { "导语", "外观", "内饰", "空间", "视野", "灯光", "动力", "操控", "舒适性", "油耗", "配置与安全", "总结" };
        // modified by chengl Dec.28.2011
        private Dictionary<int, EnumCollection.PingCeTag> dicAllTagInfo =
            new Dictionary<int, EnumCollection.PingCeTag>();

        private Dictionary<int, EnumCollection.PingCeTag> dictPingceNews =
            new Dictionary<int, EnumCollection.PingCeTag>(); //车型详解的标签

        private Dictionary<int, string> dictSerialBlockHtml;
        protected bool HasPingCeNew;
        protected string HotCarCompare = string.Empty;
        protected string JsForCommonHead = string.Empty;
        protected string KoubeiRatingHtml = string.Empty;

        private int newsId;
        private int pageCount;
        protected string pageFirstTagName = string.Empty;
        protected int pageNum = 1;
        protected string pageTagName = string.Empty;
        protected string pincePageListHtml = string.Empty;

        protected string PingCeContent = string.Empty;

        protected string PingceEditorComment = string.Empty;
        protected string PingCeEditorName = string.Empty;
        protected string PingCeFilePath = string.Empty;
        protected string PingCePagination = string.Empty;
        protected string PingCePublishTime = string.Empty;
        protected string PingCeSourceName = string.Empty;
        protected string PingCeTitle = string.Empty;
        protected string PingCeUserName = string.Empty;
        private readonly StringBuilder sbPagination = new StringBuilder();
        protected string seoYear = string.Empty;

        private readonly Car_SerialBll serialBll;
        private readonly CarNewsBll carNewsBll;
        protected string serialPhotoHotCompareHtml = string.Empty; //车型图片对比
        protected string serialToSeeJson = string.Empty;
        protected string strCs_MasterName = string.Empty;
        protected string strCs_Name = string.Empty;
        protected string strCs_SeoName = string.Empty;
        protected string strCs_ShowName = string.Empty;
        //页面的标题内容
        private readonly Dictionary<string, int> TagTitleContent = new Dictionary<string, int>();
        protected string TuijianKoubeiHtml = string.Empty;

        protected string UCarHtml = string.Empty;

        public CsPingCe()
        {
            serialBll = new Car_SerialBll();
            _commonhtmlBLL = new CommonHtmlBll();
            carNewsBll = new CarNewsBll();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetPageParam();
                MakeSerialTopADCode(csID);
                GetSerialDetailInfo();
                GetSerialPingCeData();
                GetPingceEditorCommentData();
                //this.GetHotCarCompare();
                //newsNavHtml = RenderNewsNav();
                GetMorePingCeContent();
                //BuildPageMessage();
                //看过某车的还看过
                MakeSerialToSerialHtml();
                //ucSerialToSee.SerialId = csID;
                //ucSerialToSee.SerialName = strCs_ShowName;
                //视频新闻
                MakeSerialVideoHtml();

                MakeHotSerialCompare();
                //GetSerialPingCeShiPinHtml();
                //子品牌的图片块
                //GetSerialImageHtml();
                //UCarHtml = new Car_SerialBll().GetUCarHtml(csID);
                dictSerialBlockHtml = _commonhtmlBLL.GetCommonHtml(csID, CommonHtmlEnum.TypeEnum.Serial,
                    CommonHtmlEnum.TagIdEnum.SerialPingCe);
                GetTuijianKoubeiHtml();
                GetKoubeiRatingHtml();
                MakeCompetitiveKoubeiHtml(); //竞品口碑
            }
        }

        // 页面参数
        private void GetPageParam()
        {
            csID = ConvertHelper.GetInteger(Request.QueryString["csID"]);
            pageNum = ConvertHelper.GetInteger(Request.QueryString["page"]);
            currentPageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
            if (pageNum < 1)
                pageNum = 1;
            if (currentPageIndex < 1)
                currentPageIndex = 1;

            _YearId = ConvertHelper.GetInteger(Request.QueryString["year"]);
            newsId = ConvertHelper.GetInteger(Request.QueryString["newsid"]);
        }

        // 子品牌信息
        private void GetSerialDetailInfo()
        {
            if (csID >= 0)
            {
                // 取头导航
                //bool isSuccess = false;

                #region 子品牌基本数据

                cse = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csID);

                baaUrl = serialBll.GetForumUrlBySerialId(csID);
                strCs_ShowName = cse.ShowName;
                if (csID == 1568)
                    strCs_ShowName = "索纳塔八";
                strCs_SeoName = cse.SeoName;
                strCs_MasterName = cse.Brand.Name;
                strCs_Name = cse.Name;

                #endregion

                if (_YearId < 1)
                {
                    CsHead = GetCommonNavigation("CsPingCe", csID);
                }
                else
                {
                    var tagName = "CsPingCeForYear";
                    CsHead = GetCommonNavigation(tagName, csID).Replace("{0}", _YearId.ToString());
                    JsForCommonHead = "if(document.getElementById('carYearList_" + _YearId +
                                      "')){document.getElementById('carYearList_" + _YearId +
                                      "').className='current';}changeSerialYearTag(0," + _YearId + ",'');";

                    seoYear = _YearId + "款";
                }
            }
        }

        /// <summary>
        ///     得到页面信息
        /// </summary>
        private void BuildPageMessage()
        {
            var pageTitle = "【{0}{2}评测-{0}{3}评测_车型详解】_{1}-易车网";
            var pageKeyword = "{0}评测,{0}单车评测,{1}{2}";
            var pageDescribe = "{0}{1}评测:易车网车型详解频道为您提供最权威的{0}评测,涉及{0}外观,内饰,空间,视野,灯光,动力,操控,舒适性,油耗,配置与安全等,更多{0}评测信息尽在易车网。";

            if (_YearId > 0)
            {
                _PageTitle = string.Format(pageTitle, cse.SeoName + _YearId + "款",
                    cse.Brand.MasterBrand.Name + cse.Name + _YearId + "款", pageTagName, pageNum <= 1 ? "单车" : "");
                _PageKeyword = string.Format(pageKeyword, cse.SeoName + _YearId + "款", cse.Brand.MasterBrand.Name,
                    cse.Name + _YearId + "款");
                _PageDescribe = string.Format(pageDescribe, cse.SeoName + _YearId + "款", pageTagName);
            }
            else
            {
                _PageTitle = string.Format(pageTitle, cse.SeoName, cse.Brand.MasterBrand.Name + cse.Name, pageTagName,
                    pageNum <= 1 ? "单车" : "");
                _PageKeyword = string.Format(pageKeyword, cse.SeoName, cse.Brand.MasterBrand.Name, cse.Name);
                _PageDescribe = string.Format(pageDescribe, cse.SeoName, pageTagName);
            }
        }

        // 取子品牌评测数据
        private void GetSerialPingCeData()
        {
            dicAllTagInfo = CommonFunction.IntiPingCeTagInfo();
            if (_YearId > 0)
            {
                _PingCeDs = new CarNewsBll().GetTopSerialYearNewsAllData(csID, _YearId, CarNewsType.pingce, 4);
            }
            else
            {
                _PingCeDs = new CarNewsBll().GetTopSerialNewsAllData(csID, CarNewsType.pingce, 4);
            }
            if (_PingCeDs != null && _PingCeDs.Tables.Count > 0)
            {
                _PingCeNewsCount = _PingCeDs.Tables[0].Rows.Count;
            }
            if (newsId == 0
                //&& _PingCeDs != null
                //&& _PingCeDs.Tables["listNews"] != null
                //&& _PingCeDs.Tables["listNews"].Rows.Count > 0
                )
            {
                #region 移除之前车型详解评测获取规则

                /*
				// modified by chengl Jan.17.2012
				Dictionary<int, string> dicPingCeRainbow = base.GetAllPingCeNewsURLForCsPingCePage();
				if (dicPingCeRainbow.ContainsKey(csID))
				{
					string url = dicPingCeRainbow[csID];
					string[] arrTempURL = url.Split('/');
					if (int.TryParse(arrTempURL[arrTempURL.Length - 1].ToString().Substring(3, 7), out newsId))
					{ }
				}
				// newsId = ConvertHelper.GetInteger(_PingCeDs.Tables["listNews"].Rows[0]["newsid"]);
				*/

                #endregion

                //评测提取修改 by sk 2013.01.10
                dictPingceNews = GetPingceNewsByCsId(csID);
                if (dictPingceNews.ContainsKey(pageNum))
                {
                    var url = dictPingceNews[pageNum].url;
                    var arrTempURL = url.Split('/');
                    var pageName = arrTempURL[arrTempURL.Length - 1];
                    if (pageName.Length >= 10)
                    {
                        if (int.TryParse(pageName.Substring(3, 7), out newsId))
                        {
                        }
                        var arrPage = pageName.Split('-');
                        if (arrPage.Length > 1)
                        {
                            var endPos = arrPage[arrPage.Length - 1].IndexOf('.');
                            pageNum = ConvertHelper.GetInteger(arrPage[arrPage.Length - 1].Substring(0, endPos));
                            pageNum++;
                        }
                        else
                            pageNum = 1;
                    }
                }
                _Tholder = 1;
            }
            if (newsId > 0)
            {
                SerialPingCeNews serialPingCeNews = carNewsBll.GetSerialPingCeNews(newsId);

                if (serialPingCeNews != null)
                {
                    HasPingCeNew = true;
                    PingCeTitle = string.IsNullOrWhiteSpace(serialPingCeNews.Title) ?
                        serialPingCeNews.Title : serialPingCeNews.ShortTitle;
                    PingCeFilePath = serialPingCeNews.Url;
                    PingCeUserName = !string.IsNullOrWhiteSpace(serialPingCeNews.Author) ?
                        ("作者：" + serialPingCeNews.Author) : string.Empty;
                    PingCeSourceName = serialPingCeNews.Source.Name;
                    PingCePublishTime = serialPingCeNews.PublishTime.ToString("f");
                    SerialPingceNewsPageContent serialPingCeNewsPageContent = serialPingCeNews.PageContent.FirstOrDefault(x => x.PageIndex == pageNum);
                    if (serialPingCeNewsPageContent != null)
                    {
                        PingCeContent = serialPingCeNewsPageContent.Content;
                        PingCeTitle = serialPingCeNewsPageContent.Title;
                    }

                    BaseUrl = GetBaseUrl();
                    //初始化上一页，下一页的标签对象
                    _TagTitleDescribeList.Add(1, "导语");

                    //得到左侧滑动标记内容
                    GetTagContent(currentPageIndex, BaseUrl, PingCeTitle);

                    if (serialPingCeNews.PageContent.Count < 1) return;
                    //生成翻页控件
                    GetPageHtml(serialPingCeNews.PageContent.Count, BaseUrl);
                }

                //var ds = GetPingCeNewByNewID(newsId);
                //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 &&
                //	ds.Tables[0].Columns.Contains("content"))
                //{
                //	HasPingCeNew = true;
                //	var row = ds.Tables[0].Rows[0];
                //	if (ds.Tables[0].Columns.Contains("title"))
                //		PingCeTitle = row["title"].ToString();
                //	else
                //		PingCeTitle = row["facetitle"].ToString();
                //	PingCeFilePath = row["filepath"].ToString();
                //	PingCeUserName = row["author"].ToString();
                //	if (!string.IsNullOrEmpty(PingCeUserName))
                //	{
                //		PingCeUserName = "作者：" + PingCeUserName;
                //	}
                //	// Aug.12.2010 chengl 接口缺少sourceName 
                //	if (ds.Tables[0].Columns.Contains("sourceName"))
                //	{
                //		PingCeSourceName = row["sourceName"].ToString();
                //	}
                //	else
                //	{
                //		PingCeSourceName = "";
                //	}
                //	PingCePublishTime = Convert.ToDateTime(row["publishtime"].ToString()).ToString("f");
                //	var newsContent = row["content"].ToString();
                //	// string RegexString = "id=\"bt_pagebreak\"([^<].*?)</div>";
                //	var RegexString = "<div(?:[^<]*)?id=\"bt_pagebreak\"[^>]*>([^<]*)</div>";
                //	var r = new Regex(RegexString);
                //	var newsGroup = r.Split(newsContent);
                //	if (newsGroup.Length >= pageNum*2 - 1)
                //	{
                //		PingCeContent = newsGroup[pageNum*2 - 2];
                //	}

                //	BaseUrl = GetBaseUrl();
                //	//初始化上一页，下一页的标签对象
                //	_TagTitleDescribeList.Add(1, "导语");
                //	//初始化标签标题内容
                //	InitTagTitleContent(newsGroup);
                //	//得到左侧滑动标记内容
                //	GetTagContent(currentPageIndex, BaseUrl, PingCeTitle);
                //	//循环赋值头
                //	foreach (var entity in TagTitleContent)
                //	{
                //		if (entity.Value == pageNum) PingCeTitle = entity.Key;
                //	}
                //	if (newsGroup.Length < 1) return;
                //	//生成翻页控件
                //	GetPageHtml(newsGroup.Length, BaseUrl);
                // }
            }
            else if (currentPageIndex == 100)
            {
                HasPingCeNew = true;
                BaseUrl = GetBaseUrl();
                //初始化上一页，下一页的标签对象
                _TagTitleDescribeList.Add(1, "导语");
                //初始化标签标题内容
                //InitTagTitleContent(null);
                //得到左侧滑动标记内容
                GetTagContent(currentPageIndex, BaseUrl, PingCeTitle);
                //循环赋值头
                foreach (var entity in TagTitleContent)
                {
                    if (entity.Value == pageNum) PingCeTitle = entity.Key;
                }
            }
        }

        private string GetBaseUrl()
        {
            var baseUrl = string.Empty;
            if (_YearId > 0 && _Tholder == 1)
            {
                baseUrl = string.Format("/{0}/{1}/pingce/", cse.AllSpell.ToLower(), _YearId);
            }
            else if (_YearId > 0)
            {
                baseUrl = "/" + cse.AllSpell.ToLower() + "/" + _YearId + "/pingce/p" + newsId + "/";
            }
            else if (_Tholder == 1)
            {
                baseUrl = string.Format("/{0}/pingce/", cse.AllSpell.ToLower());
            }
            else
            {
                baseUrl = "/" + cse.AllSpell.ToLower() + "/pingce/p" + newsId + "/";
            }
            return baseUrl;
        }

        // 编辑试驾评测
        private void GetPingceEditorCommentData()
        {
            PingceEditorComment = string.Empty;
            if (pageNum <= 1)
            {
                if (_PingCeDs != null
                    && _PingCeDs.Tables.Count > 0
                    && _PingCeDs.Tables[0].Rows.Count > 0)
                {
                    DataRow newsRow = null;
                    if (newsId == 0)
                    {
                        newsRow = _PingCeDs.Tables[0].Rows[0];
                    }
                    else
                    {
                        var newsRows = _PingCeDs.Tables[0].Select("cmsnewsid=" + newsId);
                        if (newsRows.Length > 0)
                        {
                            newsRow = newsRows[0];
                        }
                    }

                    if (newsRow != null && !newsRow.IsNull("carId"))
                    {
                        var carId = newsRow["carId"].ToString();
                        if (!string.IsNullOrEmpty(carId))
                        {
                            var htmlFile = Path.Combine(WebConfig.DataBlockPath,
                                string.Format(_PingceEditorCommentHtmlPathV2, carId));
                            if (File.Exists(htmlFile))
                            {
                                PingceEditorComment = File.ReadAllText(htmlFile);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     获取推荐口碑
        /// </summary>
        private void GetTuijianKoubeiHtml()
        {
            var koubei = (int)CommonHtmlEnum.BlockIdEnum.KouBeiTuiJianV2;
            if (dictSerialBlockHtml.ContainsKey(koubei))
                TuijianKoubeiHtml = dictSerialBlockHtml[koubei];
        }

        private void GetKoubeiRatingHtml()
        {
            var koubei = (int)CommonHtmlEnum.BlockIdEnum.KouBeiRatingV2;
            if (dictSerialBlockHtml.ContainsKey(koubei))
                KoubeiRatingHtml = dictSerialBlockHtml[koubei];
        }

        /// <summary>
        ///     竞品口碑
        /// </summary>
        private void MakeCompetitiveKoubeiHtml()
        {
            var serialsummaryBlock = _commonhtmlBLL.GetCommonHtml(csID, CommonHtmlEnum.TypeEnum.Serial,
                CommonHtmlEnum.TagIdEnum.SerialSummary);
            var competitive = (int)CommonHtmlEnum.BlockIdEnum.CompetitiveKoubeiV2;
            if (serialsummaryBlock.ContainsKey(competitive))
                CompetitiveKoubeiHtml = serialsummaryBlock[competitive];
        }

        /// <summary>
        ///     得到更多评测内容
        /// </summary>
        private void GetMorePingCeContent()
        {
            if (_PingCeDs == null
                || _PingCeDs.Tables.Count < 1
                || _PingCeDs.Tables[0].Rows.Count < 1) return;

            var moreContent = new StringBuilder();

            var counter = 0;
            var isContainsSourceUrl = _PingCeDs.Tables[0].Columns.Contains("sourceUrl");
            var query = _PingCeDs.Tables[0].AsEnumerable().Where(dr =>
            {
                var nId = ConvertHelper.GetInteger(dr["cmsnewsid"]);
                return nId != newsId;
            });
            moreContent.Append("<div class=\"rel-cont-box col-sty\">");
            moreContent.Append("<div class=\"h-tit\">");
            moreContent.AppendFormat("<h2>更多{0}评测</h2>", cse.ShowName);
            moreContent.Append("</div>");
            moreContent.Append("<ul>");
            foreach (var dr in query)
            {
                var nId = ConvertHelper.GetInteger(dr["cmsnewsid"].ToString());
                if (nId == newsId)
                {
                    continue;
                }
                if (counter >= 3) break;
                var title = CommonFunction.NewsTitleDecode(dr["title"].ToString());
                var filePath = dr["FilePath"].ToString();
                var fileDateTime = ConvertHelper.GetDateTime(dr["publishtime"]).ToString("yyyy年MM月dd日 hh:mm");
                //首图地址
                var imageUrl = "";
                //源名称
                var sourceName = dr["sourceName"].ToString();
                //源地址
                var sourceUrl = !isContainsSourceUrl || dr.IsNull("sourceUrl")
                    ? string.Empty
                    : dr["sourceUrl"].ToString();
                var content = dr["content"].ToString();
                var editorName = ConvertHelper.GetString(dr["EditorName"]);
                var editUrl = GetEditNameAndUrl(dr);
                var commentNum = ConvertHelper.GetInteger(dr["CommentNum"]);
                //如果链接不等于空
                if (!string.IsNullOrEmpty(sourceUrl))
                {
                    sourceName = string.Format("<a href='{0}' target='_blank'>{1}</a>", sourceUrl, sourceName);
                }
                else if (sourceName == "易车")
                {
                    sourceName = string.Format("<a href='{0}' target='_blank'>{1}</a>", "http://www.bitauto.com/",
                        sourceName);
                }
                //得到图片链接
                if (!dr.IsNull("picture") && dr["picture"].ToString().IndexOf("/not") < 0)
                {
                    imageUrl = dr["picture"].ToString();
                    if (!imageUrl.Equals(""))
                        imageUrl = imageUrl.Insert(imageUrl.IndexOf(".com/") + 5, "wapimg-210-140/");
                }
                else if (!dr.IsNull("firstPicUrl"))
                {
                    imageUrl = dr["firstPicUrl"].ToString().Trim();
                    if (imageUrl.Length > 0)
                        imageUrl = imageUrl.Insert(imageUrl.IndexOf(".com/") + 5, "wapimg-210-140/");
                }

                //剪裁内容
                if (!string.IsNullOrEmpty(content))
                {
                    content = StringHelper.SubString(content, 140, true);
                }

                moreContent.Append("<li>");
                moreContent.Append("    <div class=\"img-box\">");
                moreContent.AppendFormat("        <a href=\"{0}\">", filePath);
                moreContent.AppendFormat("<img src=\"{0}\" alt=\"\">", imageUrl);
                moreContent.Append("        </a>");
                moreContent.Append("    </div>");
                moreContent.Append("    <div class=\"txt-box\">");
                moreContent.Append("<h3>");
                moreContent.AppendFormat("  <a href=\"{0}\" target=\"_blank\">{1}</a> ", filePath, title);
                moreContent.Append("</h3>");
                moreContent.Append("<div class=\"info-box\">");
                if (!string.IsNullOrEmpty(editUrl))
                    moreContent.AppendFormat("<span>作者：{0}</span>", editUrl);
                else
                {
                    if (!string.IsNullOrEmpty(editorName))
                        moreContent.AppendFormat("<span>作者：{0}</span>", editorName);
                }
                moreContent.AppendFormat("<span>{0}</span>", fileDateTime);
                //moreContent.AppendFormat("<span data-cnewsid=\"{0}\" class=\"bg-ico-sty pl\">{1}</span>", nId, commentNum);
                //moreContent.Append("<span class=\"bg-ico-sty ls\">34566</span> ");
                moreContent.Append("</div>");
                moreContent.Append("<p class=\"intro\">");
                moreContent.AppendFormat("{0}", content);
                moreContent.AppendFormat("<a href=\"{0}\" target=\"_blank\">[详细]</a>", filePath);
                moreContent.Append("</p>");
                moreContent.Append("</div>");
                moreContent.Append("</li>");
                counter++;
            }
            moreContent.Append("</ul>");
            moreContent.Append("</div>");
            _MorePingCeContent = moreContent.ToString();
        }

        /// <summary>
        ///     得到分页标识
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        private void GetPageHtml(int pagesize, string baseUrl)
        {
            //更改分页规则，新闻ID规则不变 by sk 2013.01.11
            sbPagination.Append("<div>");
            if (dictPingceNews.Count > 0)
            {
                #region

                var j = 0;
                foreach (var kvp in dictPingceNews)
                {
                    j++;
                    if (j == currentPageIndex)
                        sbPagination.Append("<a class=\"linknow\">" + j + "</a>");
                    else
                        sbPagination.Append("<a href=\"" + baseUrl + j + "/\">" + j + "</a>");
                }

                #region 20161227 songcl 去掉下一页按钮 （经过顾晓确认）

                //if (currentPageIndex < dictPingceNews.Count)
                //{
                //    var nextIndex = currentPageIndex + 1;
                //    if (dictPingceNews.ContainsKey(nextIndex))
                //        sbPagination.AppendLine("<a href=\"" + baseUrl + nextIndex + "/\" class=\"next_on\">下一页:" +
                //                                dictPingceNews[nextIndex].tagName + "</a>");
                //    else
                //        sbPagination.AppendLine("<a href=\"" + baseUrl + nextIndex + "/\" class=\"next_on\">下一页</a>");
                //}

                #endregion


                #endregion
            }
            else
            {
                #region

                pageCount = pagesize / 2 + 1;

                #region 20161227 songcl 去掉下一页按钮 （经过顾晓确认）

                //if (pageNum > 1)
                //{
                //    var preIndex = pageNum - 1;
                //    //sbPagination.Append("<a href=\"" + baseUrl + "\" class=\"preview_on\">首页</a>");
                //    if (preIndex == 1)
                //    {
                //        sbPagination.Append("<a href=\"" + baseUrl + "\" class=\"preview_on\">上一页:" +
                //                            _TagTitleDescribeList[preIndex] + "</a>");
                //    }
                //    else if (_TagTitleDescribeList.ContainsKey(preIndex)) //如果上一页包含缩略标题
                //    {
                //        sbPagination.Append("<a href=\"" + baseUrl + preIndex + "/\" class=\"preview_on\">上一页:" +
                //                            _TagTitleDescribeList[preIndex] + "</a>");
                //    }
                //    else
                //    {
                //        sbPagination.Append("<a href=\"" + baseUrl + preIndex + "/\" class=\"preview_on\">上一页</a>");
                //    }
                //}

                #endregion


                var startPageIndex = pageNum - 5;
                if (startPageIndex < 1)
                {
                    startPageIndex = 1;
                }
                var endPageIndex = startPageIndex + 10 - 1;
                if (endPageIndex > pageCount)
                    endPageIndex = pageCount;

                startPageIndex = endPageIndex - 10 + 1;
                if (startPageIndex < 1)
                {
                    startPageIndex = 1;
                }

                for (var i = startPageIndex; i <= endPageIndex; i++)
                {
                    if (i == pageNum)
                    {
                        sbPagination.Append("<a class=\"linknow\">" + i + "</a>");
                    }
                    else
                    {
                        if (i == 1)
                        {
                            sbPagination.Append("<a href=\"" + baseUrl + "\">" + i + "</a>");
                        }
                        else
                        {
                            sbPagination.Append("<a href=\"" + baseUrl + i + "/\">" + i + "</a>");
                        }
                    }
                }

                #region 20161227 songcl 去掉下一页按钮 （经过顾晓确认）

                //if (pageNum < pageCount)
                //{
                //    var nextIndex = pageNum + 1;
                //    if (!_TagTitleDescribeList.ContainsKey(nextIndex))
                //    {
                //        sbPagination.AppendLine("<a href=\"" + baseUrl + nextIndex + "/\" class=\"next_on\">下一页</a>");
                //    }
                //    else
                //    {
                //        sbPagination.AppendLine("<a href=\"" + baseUrl + nextIndex + "/\" class=\"next_on\">下一页:" +
                //                                _TagTitleDescribeList[nextIndex] + "</a>");
                //    }
                //    //sbPagination.AppendLine("<a href=\"" + baseUrl + pageCount + "/\" class=\"next_on\">末页</a>");
                //}

                #endregion


                #endregion
            }
            sbPagination.Append("</div>");
            PingCePagination = sbPagination.ToString();
        }

        /// <summary>
        ///     得到标记内容
        /// </summary>
        private void GetTagContent(int curPageNum, string baseUrl, string firstPageTitle)
        {
            var htmlCode = new StringBuilder();
            //新提取规则，新闻ID访问规则不变 by sk 2013.01.10
            if (dictPingceNews.Count > 0)
            {
                #region

                var pageIndex = 0;
                foreach (var kvp in dictPingceNews)
                {
                    pageIndex++;
                    if (curPageNum == pageIndex)
                    {
                        htmlCode.AppendFormat("<li class=\"current\">{0}</li>", kvp.Value.tagName);
                        pageTagName = kvp.Value.tagName;
                        pageFirstTagName = kvp.Value.tagName;
                        if (curPageNum == 1)
                        {
                            pageTagName = "";
                            pageFirstTagName = "评测";
                        }
                    }
                    else
                    {
                        htmlCode.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", baseUrl + pageIndex + "/",
                            kvp.Value.tagName);
                    }
                }

                #endregion
            }
            else
            {
                #region

                var title = "";
                var pageIndex = 0;
                // modified by chengl Dec.28.2011
                // foreach (string entity in tagList)
                foreach (var kvp in dicAllTagInfo)
                {
                    var isContains = false;
                    // if (entity == "导语" && curPageNum == 1)
                    if (kvp.Value.tagName == "导语" && curPageNum == 1)
                    {
                        htmlCode.Append("<li class=\"current\">导语</li>");
                        pageTagName = "";
                        pageFirstTagName = "评测";
                        continue;
                    }
                    // else if (entity == "导语")
                    if (kvp.Value.tagName == "导语")
                    {
                        htmlCode.AppendFormat("<li><a href=\"{0}\">导语</a></li>", baseUrl + "1/");
                        continue;
                    }
                    isContains = GetTitleIsContainsTag(kvp.Value, out title, out pageIndex);
                    if (isContains && pageIndex == curPageNum)
                    {
                        // htmlCode.AppendFormat("<li class=\"current\">{0}</li>", entity);
                        htmlCode.AppendFormat("<li class=\"current\">{0}</li>", kvp.Value.tagName);
                        PingCeTitle = title;
                        // pageTagName = entity;
                        pageTagName = kvp.Value.tagName;
                        pageFirstTagName = kvp.Value.tagName;
                    }
                    else if (isContains)
                    {
                        // htmlCode.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", baseUrl + pageIndex + "/", entity);
                        htmlCode.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", baseUrl + pageIndex + "/",
                            kvp.Value.tagName);
                    }
                    //htmlCode.AppendFormat("<li>{0}</li>", entity);
                }

                #endregion
            }
            _LeftTagContent = htmlCode.ToString();
        }

        /// <summary>
        ///     得到标签是否可以匹配到标题
        ///     modified by chengl Dec.28.2011
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="contentList"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        // private bool GetTitleIsContainsTag(string tag, out string title, out int pageIndex)
        private bool GetTitleIsContainsTag(EnumCollection.PingCeTag tag, out string title, out int pageIndex)
        {
            var r = new Regex(tag.tagRegularExpressions);
            title = "";
            pageIndex = 0;
            var IsContains = false;
            foreach (var entity in TagTitleContent)
            {
                // modified by chengl Dec.28.2011
                if (!r.IsMatch(entity.Key))
                {
                    continue;
                }

                // if (entity.Key.IndexOf(tag) < 0) continue;
                IsContains = true;
                title = entity.Key;
                pageIndex = entity.Value;
            }
            if (IsContains)
            {
                TagTitleContent.Remove(title);
                if (!_TagTitleDescribeList.ContainsKey(pageIndex))
                {
                    // _TagTitleDescribeList.Add(pageIndex, tag);
                    _TagTitleDescribeList.Add(pageIndex, tag.tagName);
                }
            }

            return IsContains;
        }

        /// <summary>
        ///     初始化标签标题内容
        /// </summary>
        private void InitTagTitleContent(string[] contentList)
        {
            var rex = new Regex(@"\$\$(?<title>.+)\$\$");
            var pageIndex = (contentList.Length + 1) / 2 + 1;
            var title = string.Empty;
            for (var i = contentList.Length - 2; i > 0; i -= 2)
            {
                pageIndex--;
                var tmpStr = contentList[i];
                try
                {
                    title = rex.Match(tmpStr).Result("${title}");
                }
                catch
                {
                }
                if (title.Length == 0)
                {
                    pageIndex--;
                    continue;
                }
                title = StringHelper.RemoveHtmlTag(title);
                if (!TagTitleContent.ContainsKey(title))
                {
                    TagTitleContent.Add(title, pageIndex);
                }
            }
        }

        /// <summary>
        ///     得到编辑链接
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetEditNameAndUrl(DataRow dr)
        {
            var uid = ConvertHelper.GetInteger(dr["EditorId"].ToString());
            if (uid == 0) return string.Empty;
            //用户列表
            var editerlist = new NewsEditerBll().GetNewsEditerList();
            if (editerlist == null || !editerlist.ContainsKey(uid)) return string.Empty;

            var editer = editerlist[uid];

            if (string.IsNullOrEmpty(editer.UserBlogUrl))
                return editer.UserName;
            return string.Format("<a href='{0}' target='_blank'>{1}</a> ", editer.UserBlogUrl, editer.UserName);
        }

        //protected string VideoIds = "";
        /// <summary>
        ///     获取子品牌视频
        /// </summary>
        private void MakeSerialVideoHtml()
        {
            var videoList = VideoBll.GetVideoBySerialId(csID);
            if (videoList.Count <= 0) return;
            var sb = new StringBuilder();

            //sb.Append("<div class=\"line-box\">");
            //sb.Append("<div class=\"side_title\">");
            //sb.AppendFormat("<h4><a href='http://v.bitauto.com/car/serial/{0}.html' target='_blank'>{1}视频</a></h4>",
            //    csID, strCs_ShowName.Replace("(进口)", "").Replace("（进口）", ""));
            //sb.Append("</div>");
            //sb.Append("<div class=\"theme_list play_ol\">");
            //sb.Append("<ul class=\"video_list\">");
            //foreach (var entity in videoList)
            //{

            //    sb.Append("<li class=\"fist\">");
            //    sb.AppendFormat("<a target=\"_blank\" href=\"{0}\" class=\"img\"><img src=\"{1}\"></a>", entity.ShowPlayUrl, entity.ImageLink);
            //    sb.AppendFormat("<p><a href=\"{0}\" target=\"_blank\">{1}</a></p>", entity.ShowPlayUrl, entity.ShortTitle);
            //    sb.Append("</li>");
            //}
            //sb.Append("</ul>");
            //sb.Append("</div>");
            //sb.Append("</div>");

            foreach (var entity in videoList)
            {
                //VideoIds += entity.VideoId + ",";
                sb.AppendFormat("<div class=\"img-info-layout img-info-layout-video img-info-layout-14079\" data-type=\"{1}\" data-id=\"{0}\">", entity.VideoId, entity.Source == 1 ? "vf" : "v");
                sb.Append("    <div class=\"img\">");
                sb.AppendFormat(
                    "<a target=\"_blank\" href=\"{0}\"><i class=\"bg-ico-sty play-btn\"></i><img src = \"{1}\"></a>",
                    entity.ShowPlayUrl, entity.ImageLink.Replace("/Video/", "/newsimg_140x79/Video/"));
                sb.Append("    </div>");
                sb.Append("    <ul class=\"p-list\">");
                sb.AppendFormat("        <li class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></li>",
                    entity.ShowPlayUrl,
                    entity.ShortTitle);
                sb.AppendFormat("<li class=\"num\"><span class=\"play\">{0}</span><span class=\"comment\">{1}</span></li>", "", "");
                sb.Append("    </ul>");
                sb.Append("</div>");
            }

            _SerialShiPinHtml = sb.ToString();
        }

        /// <summary>
        ///     得到品牌下的其他子品牌
        /// </summary>
        /// <returns></returns>
        protected string GetBrandOtherSerial()
        {
            var carSerialPhotoList = new Car_BrandBll().GetCarSerialPhotoListByCBID(cse.BrandId, false);

            carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

            if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
            {
                return "";
            }

            var forLastCount = 0;
            foreach (var entity in carSerialPhotoList)
            {
                if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Id)
                {
                    continue;
                }
                forLastCount++;
            }

            var contentBuilder = new StringBuilder(string.Empty);
            var serialUrl = "<a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}</a>";
            var index = 0;
            foreach (var entity in carSerialPhotoList)
            {
                var IsExitsUrl = true;
                if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Id)
                {
                    continue;
                }
                var priceRang = GetSerialPriceRangeByID(entity.SerialId);
                if (entity.SaleState == "待销")
                {
                    IsExitsUrl = false;
                    priceRang = "未上市";
                }
                else if (priceRang.Trim().Length == 0)
                {
                    IsExitsUrl = false;
                    priceRang = "暂无报价";
                }
                if (IsExitsUrl)
                {
                    priceRang = string.Format("<a href='http://price.bitauto.com/brand.aspx?newbrandid={0}'>{1}</a>",
                        entity.SerialId, priceRang);
                }
                var tempCsSeoName = string.IsNullOrEmpty(entity.CS_SeoName) ? entity.ShowName : entity.CS_SeoName;
                index++;
                contentBuilder.AppendFormat("<li><div class=\"txt\">{0}</div><span>{1}</span></li>"
                    , string.Format(serialUrl, entity.CS_AllSpell, tempCsSeoName), priceRang
                    );
            }

            var brandOtherSerial = new StringBuilder(string.Empty);
            if (contentBuilder.Length > 0)
            {
                brandOtherSerial.Append("<div class=\"col-sty col-side-sty effect-box\">");
                brandOtherSerial.AppendFormat("<h2>{0}其他车型</h2>", cse.Brand.Name);

                brandOtherSerial.Append("<div class=\"list-txt list-txt-m list-txt-default\">");

                brandOtherSerial.Append("<ul>");

                brandOtherSerial.Append(contentBuilder);

                brandOtherSerial.Append("</ul>");

                brandOtherSerial.Append("</div>");

                brandOtherSerial.Append("</div>");
            }

            return brandOtherSerial.ToString();
        }

        /// <summary>
        ///     看了还看
        /// </summary>
        private void MakeSerialToSerialHtml()
        {
            serialToSeeJson = serialBll.GetSerialSeeToSeeJson(csID, 6, 3);
        }

        //==================================modified by sk 2014.07.09 以下是废除的方法=====================================

        ///// <summary>
        ///// 得到品牌下的其他子品牌
        ///// </summary>
        ///// <returns></returns>
        //protected string GetBrandOtherSerial()
        //{
        //    return new Car_SerialBll().GetBrandOtherSerialList(cse);
        //}

        // 取子品牌图片对比

        /// <summary>
        ///     生成分页导航
        /// </summary>
        /// <param name="contentList"></param>
        /// <param name="curPageNum"></param>
        /// <returns></returns>
        private string GetPageListHtml(string[] contentList, int curPageNum, string baseUrl, string firstPageTitle)
        {
            var htmlCode = new StringBuilder();
            htmlCode.AppendLine("<div id=\"content_bit\">");
            htmlCode.AppendLine("<div class=\"clear\"></div>");
            htmlCode.AppendLine("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"con_nav\">");
            htmlCode.AppendLine("<tbody>");
            htmlCode.AppendLine("<tr><th scope=\"col\" colspan=\"2\">分页导航：</th></tr>");
            var rex = new Regex(@"\$\$(?<title>.+)\$\$");
            var pageIndex = 0;
            for (var i = -1; i < contentList.Length; i += 2)
            {
                pageIndex++;
                var url = baseUrl + pageIndex + "/";
                var title = "";
                if (i == -1)
                    title = firstPageTitle;
                else
                {
                    var tmpStr = contentList[i];
                    try
                    {
                        title = rex.Match(tmpStr).Result("${title}");
                    }
                    catch
                    {
                    }
                    if (title.Length == 0)
                    {
                        pageIndex--;
                        continue;
                    }
                }
                title = StringHelper.RemoveHtmlTag(title);
                if (pageIndex % 2 == 1)
                    htmlCode.AppendLine("<tr>");
                htmlCode.Append("<td>第" + pageIndex + "页、");
                if (pageIndex == curPageNum)
                {
                    htmlCode.Append(title);
                    PingCeTitle = title;
                }
                else
                    htmlCode.Append("<a title=\"" + title + "\" href=\"" + url + "\">" + title + "</a>");
                htmlCode.AppendLine("</td>");

                if (i + 2 > contentList.Length - 1 && pageIndex % 2 == 1)
                {
                    htmlCode.AppendLine("<td></td>");
                    pageIndex++;
                }

                if (pageIndex % 2 == 0)
                    htmlCode.AppendLine("</tr>");
            }

            htmlCode.AppendLine("</tbody>");
            htmlCode.AppendLine("</table></div>");
            return htmlCode.ToString();
        }

        private void GetSerialHotCompareCars()
        {
            var sb = new StringBuilder();
            var lshcd = GetSerialHotCompareByCsID(csID, 5);
            if (lshcd.Count > 0)
            {
                sb.Append("<div class=\"line-box line-box_t0\" id=\"\">");
                sb.Append("<div class=\"side_title\">");
                sb.Append("<h4><a rel=\"nofollow\" href=\"/tupianduibi/\" target=\"_blank\">车型图片对比</a></h4>");
                sb.Append("</div>");


                sb.Append("<ul class=\"text-list\">");
                foreach (var shcd in lshcd)
                {
                    sb.AppendFormat(
                        "<li><a href=\"/tupianduibi/?csids={2},{3}\" target=\"_blank\">{0}<em class=\"vs\">VS</em>{1}</a></li>",
                        strCs_ShowName, shcd.CompareCsShowName, shcd.CurrentCsID, shcd.CompareCsID);
                }
                sb.Append("</ul>");
                sb.Append("<div class=\"clear\"></div>");
                sb.Append("</div>");
            }
            serialPhotoHotCompareHtml = sb.ToString();
        }

        /// <summary>
        ///     生成小导航代码
        /// </summary>
        /// <returns></returns>
        private string RenderNewsNav()
        {
            var htmlCode = new StringBuilder();
            if (_YearId > 0)
            {
                htmlCode.AppendFormat("<div class=\"year\">{0}款</div>", _YearId);
                htmlCode.AppendFormat("<ul class=\"car_tab_daogou {0}\">", "car_tab_daogou_right");
            }
            else
            {
                htmlCode.AppendFormat("<ul class=\"car_tab_daogou {0}\">", "");
            }
            var sic = serialBll.GetSerialInfoCard(csID);
            if (_PingCeNewsCount > 0)
                htmlCode.AppendLine(string.Format("<li class=\"current\">易车评测<span>({0})</span></li>", _PingCeNewsCount));
            else
                htmlCode.AppendLine("<li class=\"current\">易车评测</li>");

            var titleTag = new Dictionary<CarNewsType, string>();
            titleTag.Add(CarNewsType.shijia, "体验试驾");
            //titleTag.Add("xinwen", "新闻");
            //titleTag.Add("hangqing", "行情");
            titleTag.Add(CarNewsType.daogou, "导购");
            titleTag.Add(CarNewsType.yongche, "用车");
            titleTag.Add(CarNewsType.gaizhuang, "改装");
            titleTag.Add(CarNewsType.anquan, "安全");
            titleTag.Add(CarNewsType.xinwen, "新闻");

            var baseUrl = string.Empty;
            if (_YearId > 0)
            {
                baseUrl = string.Format("/{0}/{1}/", cse.AllSpell.ToLower(), _YearId);
            }
            else
            {
                baseUrl = string.Format("/{0}/", cse.AllSpell.ToLower());
            }
            var newsBll = new CarNewsBll();
            foreach (var entity in titleTag)
            {
                var newsCount = newsBll.GetSerialNewsCount(csID, _YearId, entity.Key);
                if (newsCount > 0)
                    htmlCode.AppendFormat("<li><a href=\"{0}{1}/\">{2}<span>({3})</span></a></li>", baseUrl, entity.Key,
                        entity.Value, newsCount);
                else
                    htmlCode.AppendFormat("<li><a class=\"nolink\">{0}</a></li>", entity.Value);
            }
            htmlCode.Append("</ul>");
            return htmlCode.ToString();
        }

        // 子品牌竞争车型
        private void GetHotCarCompare()
        {
            if (csID > 0)
            {
                var htmlList = new List<string>();
                var carSerialBaseList = new Dictionary<string, List<Car_SerialBaseEntity>>();
                carSerialBaseList = serialBll.GetSerialCityCompareList(csID, HttpContext.Current);
                var compareBaseUrl = "/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?count=3&csids=" + csID +
                                     ",";
                if (carSerialBaseList != null && carSerialBaseList.Count > 0 && carSerialBaseList.ContainsKey("全国"))
                {
                    #region initCode_And_Delete

                    /*
                List<Car_SerialBaseEntity> serialCompareList = carSerialBaseList["全国"];
                htmlCode.Append("<div class=\"line_box newcarindex\" id=\"serialHotCompareList\">");
                htmlCode.Append("<h3><span>网友都用它和谁比</span></h3>");
                htmlCode.Append("<div class=\"more\"><a target=\"_blank\" href=\"http://car.bitauto.com/chexingduibi/\">车型对比>></a></div>");
                htmlCode.Append("<div class=\"ranking_list\" id=\"rank_model_box\">");
                htmlCode.Append("<div class=\"this\">" + cse.Cb_Name.Trim() + cse.Cs_Name.Trim() + " VS</div>");
                htmlCode.Append("<ol class=\"hot_ranking\">");

                for (int i = 0; i < serialCompareList.Count; i++)
                {
                    Car_SerialBaseEntity carSerial = serialCompareList[i];
                    htmlCode.Append("<li><em><a target=\"_blank\" href=\"/" + carSerial.SerialNameSpell.Trim().ToLower() + "/\" >");
                    htmlCode.Append(carSerial.SerialShowName.Trim() + "</a></em>");
                    htmlCode.Append("<span><a href=\"" + compareBaseUrl + carSerial.SerialId + "\" target=\"_blank\">对比</a></span></li>");
                }

                htmlCode.Append("</ol></div>");
                htmlCode.AppendLine("</div>");
                */

                    #endregion

                    var serialCompareList = carSerialBaseList["全国"];
                    htmlList.Add("<div class=\"line_box h160\" id=\"serialHotCompareList\">");
                    htmlList.Add("<h3><span>网友都用它和谁比</span></h3>");
                    htmlList.Add(
                        "<div class=\"more\"><a href=\"/chexingduibi/\" target=\"_blank\">车型对比&gt;&gt;</a></div>");
                    htmlList.Add("<div class=\"ranking_list\" id=\"rank_model_box\">");
                    htmlList.Add("<ol class=\"carContrast\">");

                    for (var i = 0; i < serialCompareList.Count; i++)
                    {
                        var carSerial = serialCompareList[i];
                        if (i == serialCompareList.Count - 1)
                            htmlList.Add("<li class=\"last\">");
                        else
                            htmlList.Add("<li>");
                        htmlList.Add("<em>" + StringHelper.SubString(strCs_ShowName, 10, false) + " <s>VS</s> ");
                        htmlList.Add(carSerial.SerialShowName.Trim() + "</em>");
                        htmlList.Add("<span><a href=\"" + compareBaseUrl + carSerial.SerialId +
                                     "\" target=\"_blank\">对比&gt;&gt;</a></span></li>");
                    }

                    htmlList.Add("</ol></div>");
                    htmlList.Add("</div>");
                }

                HotCarCompare = string.Concat(htmlList.ToArray());
            }
        }

        /// <summary>
        ///     取子品牌对比排行数据
        /// </summary>
        /// <returns></returns>
        private void MakeHotSerialCompare()
        {
            var carSerialBaseList = serialBll.GetSerialCityCompareList(csID, HttpContext.Current);
            var htmlList = new List<string>();
            var compareBaseUrl = "/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?count=3&csids=" + csID + ",";
            var serialCompareForPkUrl = "/duibi/" + csID + "-";
            if (carSerialBaseList != null && carSerialBaseList.ContainsKey("全国"))
            {
                var dicAllCsPic = GetAllSerialPicURL(true);
                var serialCompareList = carSerialBaseList["全国"];
                htmlList.Add("<div class=\"col-sty col-side-sty\" id=\"serialHotCompareList\">");
                htmlList.Add("    <h2>大家用它和谁比</h2>");
                htmlList.Add("    <div class=\"contrast-box\">");
                htmlList.Add("        <div class=\"img-list clearfix\">");

                for (var i = 0; i < serialCompareList.Count; i++)
                {
                    var carSerial = serialCompareList[i];
                    htmlList.Add("            <div class=\"img-info-layout-vertical  img-info-layout-vertical-14093\">");
                    htmlList.Add("                <div class=\"img\">");
                    htmlList.Add(string.Format("      <a href=\"{0}\" target=\"_blank\"><img src=\"{1}\"></a>",
                        serialCompareForPkUrl + carSerial.SerialId,
                        dicAllCsPic.ContainsKey(carSerial.SerialId) ? dicAllCsPic[carSerial.SerialId].Replace("_2.jpg", "_3.jpg") : WebConfig.DefaultCarPic));
                    htmlList.Add("                </div>");
                    htmlList.Add("                <ul class=\"p-list\">");
                    htmlList.Add(string.Format("      <li class=\"name\"><a href=\"{0}\" target=\"_blank\"><span>VS</span> {1}</a></li>",
                        serialCompareForPkUrl + carSerial.SerialId, carSerial.SerialShowName.Trim()));
                    htmlList.Add("                </ul>");
                    htmlList.Add("            </div>");
                }

                htmlList.Add("        </div>");
                htmlList.Add("    </div>");
                htmlList.Add("</div>");
            }

            carCompareHtml = string.Concat(htmlList.ToArray());
        }

        /// <summary>
        ///     得到子品牌评测
        /// </summary>
        private void GetSerialPingCeShiPinHtml()
        {
            var filePath = Path.Combine(WebConfig.DataBlockPath, string.Format(_PingCheShiPinFilePath, csID));
            if (!File.Exists(filePath)) return;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(filePath);
            if (xmlDoc == null) return;

            var xmlns = new XmlNamespaceManager(xmlDoc.NameTable);
            xmlns.AddNamespace("ns", "http://schemas.datacontract.org/2004/07/BitAuto.Video.RESTfulApi.ShowModel");

            var xNodelist = xmlDoc.SelectNodes("/ns:ListVideo/ns:ListCarVideo/ns:CarVideo", xmlns);
            if (xNodelist == null || xNodelist.Count < 1) return;
            var htmlCode = new StringBuilder();
            htmlCode.Append("<div class='line_box car_img_box'>");
            htmlCode.AppendFormat(
                "<h3><span><a href='http://v.bitauto.com/car/serial/{0}.html' target='_blank'>{1}视频</a></span></h3>",
                csID, strCs_ShowName.Replace("(进口)", "").Replace("（进口）", ""));
            htmlCode.Append("<div class='car_img car_v_box'>");
            var xNode = xNodelist[0];
            var title = xNode.SelectSingleNode("ns:ShortTitle", xmlns) != null
                ? xNode.SelectSingleNode("ns:ShortTitle", xmlns).InnerText
                : "";
            var image = xNode.SelectSingleNode("ns:ImageLink", xmlns) != null
                ? xNode.SelectSingleNode("ns:ImageLink", xmlns)
                    .InnerText.Replace(".bitautoimg.com/", ".bitautoimg.com/wapimg-150-9999/")
                : "";
            var fileUrl = xNode.SelectSingleNode("ns:ShowPlayUrl", xmlns) != null
                ? xNode.SelectSingleNode("ns:ShowPlayUrl", xmlns).InnerText
                : "";

            htmlCode.AppendFormat("<a target='_blank' href='{0}'>", fileUrl);
            htmlCode.AppendFormat("<img width='150' height='100' src='{0}'></a>", image);
            htmlCode.AppendFormat("<a target='_blank' href='{0}' class='car_img_t'>{1}</a>", fileUrl, title);
            htmlCode.Append("<ul class='video_list'>");
            for (var i = 1; i < xNodelist.Count; i++)
            {
                if (i > 6) break;
                xNode = xNodelist[i];
                title = xNode.SelectSingleNode("ns:ShortTitle", xmlns) != null
                    ? xNode.SelectSingleNode("ns:ShortTitle", xmlns).InnerText
                    : "";
                fileUrl = xNode.SelectSingleNode("ns:ShowPlayUrl", xmlns) != null
                    ? xNode.SelectSingleNode("ns:ShowPlayUrl", xmlns).InnerText
                    : "";

                htmlCode.Append("<li>");
                htmlCode.AppendFormat("<a target='_blank' href='{0}'>{1}</a>", fileUrl, title);
                htmlCode.Append("</li>");
            }
            htmlCode.Append("</ul>");
            htmlCode.Append("</div>");
            htmlCode.AppendFormat(
                "<div class='more'><a href='http://v.bitauto.com/car/serial/{0}.html' target='_blank'>更多&gt;&gt;</a></div>",
                csID);
            htmlCode.Append("</div>");
            _SerialShiPinHtml = htmlCode.ToString();
        }

        /// <summary>
        ///     得到子品牌的图片块
        /// </summary>
        private void GetSerialImageHtml()
        {
            var filePath = Path.Combine(WebConfig.DataBlockPath, string.Format(_PingCheImageFilePath, csID));
            if (!File.Exists(filePath)) return;

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            if (xmlDoc == null) return;
            var xNodelist = xmlDoc.SelectNodes("data/ImageList");
            if (xNodelist == null || xNodelist.Count < 1) return;
            var htmlCode = new StringBuilder();
            htmlCode.Append("<div class='line_box pic_list_box'>");
            htmlCode.AppendFormat(
                "<h3><span><a target='_blank' href='http://photo.bitauto.com/serial/{0}/'>{1}图片</a></span></h3>", csID,
                strCs_ShowName.Replace("(进口)", "").Replace("（进口）", ""));
            htmlCode.Append("<ul class='pic_list'>");

            foreach (XmlNode xNode in xNodelist)
            {
                var title = xNode.SelectSingleNode("SiteImageName").InnerText;
                var imgId = ConvertHelper.GetInteger(xNode.SelectSingleNode("SiteImageId").InnerText);
                var url = xNode.SelectSingleNode("SiteImageUrl").InnerText;
                url = GetPublishImage(2, url, imgId);
                htmlCode.Append("<li>");
                htmlCode.AppendFormat("<a target='_blank' href='http://photo.bitauto.com/serial/{0}/' title='{1}'>",
                    csID, title);
                htmlCode.AppendFormat("<img width='90' height='60' src='{0}'>", url);
                htmlCode.Append("</a>");
                htmlCode.AppendFormat("<a target='_blank' href='http://photo.bitauto.com/serial/{0}/'>{1}</a>", csID,
                    StringHelper.SubString(title, 13, false));
                htmlCode.Append("</li>");
            }

            htmlCode.Append("</ul>");
            htmlCode.Append("<div class='clear'></div>");
            htmlCode.AppendFormat(
                "<div class='more'><a href='http://photo.bitauto.com/serial/{0}/' target='_blank'>更多&gt;&gt;</a></div>",
                csID);
            htmlCode.Append("</div>");
            _SerialImageHtml = htmlCode.ToString();
        }
    }
}