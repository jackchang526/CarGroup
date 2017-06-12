using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannel.CarchannelWeb.PageSerial
{
	public partial class CsPingCe : PageBase
	{
		protected int csID = 0;
		protected int pageNum = 1;
		private int pageCount = 0;

		protected string CsHead = string.Empty;
		protected string strCs_ShowName = string.Empty;
		protected string strCs_SeoName = string.Empty;
		protected string strCs_MasterName = string.Empty;
		protected string strCs_Name = string.Empty;

		protected string PingCeContent = string.Empty;
		protected string PingCeTitle = string.Empty;
		protected string PingCeFilePath = string.Empty;
		protected string PingCeUserName = string.Empty;
		protected string PingCeEditorName = string.Empty;
		protected string PingCeSourceName = string.Empty;
		protected string PingCePublishTime = string.Empty;
		protected string PingCePagination = string.Empty;
		protected bool HasPingCeNew = false;
		protected string HotCarCompare = string.Empty;
		protected string _MorePingCeContent = string.Empty;
        private CommonHtmlBll _commonhtmlBLL;
        private Dictionary<int, string> dictSerialBlockHtml;
	    protected string TuijianKoubeiHtml = string.Empty;
	    protected string KoubeiRatingHtml = string.Empty;
        protected string CompetitiveKoubeiHtml = string.Empty;//竞品口碑
		protected string pincePageListHtml = string.Empty;
		//protected string newsNavHtml = string.Empty;
		protected string baaUrl;
		protected string _LeftTagContent = string.Empty;
        protected string serialToSeeJson = string.Empty;
		protected string carCompareHtml = string.Empty;
		//子品牌的视频块
		protected string _SerialShiPinHtml = string.Empty;
		//子品牌的图片块
		protected string _SerialImageHtml = string.Empty;
		protected string serialPhotoHotCompareHtml = string.Empty;//车型图片对比
		/*
		 * _PageTitle:页面标题
		 * _PageKeyword:页面关键字
		 * _PageDescribe:页面描述
		 */
		protected string _PageTitle = string.Empty;
		protected string _PageKeyword = string.Empty;
		protected string _PageDescribe = string.Empty;
        protected string pageTagName = string.Empty;
        protected string pageFirstTagName = string.Empty;
		protected string seoYear = string.Empty;
		//年款ID
		protected int _YearId = 0;
		protected string JsForCommonHead = string.Empty;
		private StringBuilder sbPagination = new StringBuilder();
		private DataSet _PingCeDs = new DataSet();

		protected SerialEntity cse;				//子品牌信息

		private int newsId = 0;
		//左侧滑动标签内容
		// private string[] tagList = { "导语", "外观", "内饰", "空间", "视野", "灯光", "动力", "操控", "舒适性", "油耗", "配置与安全", "总结" };
		// modified by chengl Dec.28.2011
		private Dictionary<int, EnumCollection.PingCeTag> dicAllTagInfo = new Dictionary<int, EnumCollection.PingCeTag>();

		private Dictionary<int, string> _TagTitleDescribeList = new Dictionary<int, string>();
		//标记当前新闻是否为第一条评测
		private int _Tholder = 0;
		//页面的标题内容
		private Dictionary<string, int> TagTitleContent = new Dictionary<string, int>();
		//得到视频块和图片地地址
		private string _PingCheShiPinFilePath = "Data\\SerialNews\\pingce\\ShiPinNew\\{0}.xml";
		private string _PingCheImageFilePath = "Data\\SerialNews\\pingce\\Image\\{0}.xml";

		protected string PingceEditorComment = string.Empty;
		private const string _PingceEditorCommentHtmlPath = "Data\\SerialSet\\PingceEditorCommentHtml\\Serial_Comment_{0}.html";

		protected string UCarHtml = string.Empty;

		private int _PingCeNewsCount = 0;
		protected int currentPageIndex = 0;

		Dictionary<int, EnumCollection.PingCeTag> dictPingceNews = new Dictionary<int, EnumCollection.PingCeTag>();//车型详解的标签

		private Car_SerialBll serialBll;

        protected string BaseUrl = string.Empty;

		public CsPingCe()
		{
			serialBll = new Car_SerialBll();
            _commonhtmlBLL = new CommonHtmlBll();
            
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.GetPageParam();
				base.MakeSerialTopADCode(csID);
                this.GetSerialDetailInfo();
                this.GetSerialPingCeData();
                this.GetPingceEditorCommentData();
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
                dictSerialBlockHtml = _commonhtmlBLL.GetCommonHtml(csID, CommonHtmlEnum.TypeEnum.Serial, CommonHtmlEnum.TagIdEnum.SerialPingCe);
			    GetTuijianKoubeiHtml();
			    GetKoubeiRatingHtml();
                MakeCompetitiveKoubeiHtml();//竞品口碑
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
					CsHead = base.GetCommonNavigation("CsPingCe", csID);
				}
				else
				{
					string tagName = "CsPingCeForYear";
					CsHead = base.GetCommonNavigation(tagName, csID).Replace("{0}", _YearId.ToString());
					JsForCommonHead = "if(document.getElementById('carYearList_" + _YearId.ToString() + "')){document.getElementById('carYearList_" + _YearId.ToString() + "').className='current';}changeSerialYearTag(0," + _YearId.ToString() + ",'');";

					seoYear = _YearId + "款";
				}
			}
		}

		/// <summary>
		/// 得到页面信息
		/// </summary>
		private void BuildPageMessage()
		{
			string pageTitle = "【{0}{2}评测-{0}{3}评测_车型详解】_{1}-易车网";
			string pageKeyword = "{0}评测,{0}单车评测,{1}{2}";
			string pageDescribe = "{0}{1}评测:易车网车型详解频道为您提供最权威的{0}评测,涉及{0}外观,内饰,空间,视野,灯光,动力,操控,舒适性,油耗,配置与安全等,更多{0}评测信息尽在易车网。";

			if (_YearId > 0)
			{
				_PageTitle = string.Format(pageTitle, cse.SeoName + _YearId + "款", cse.Brand.MasterBrand.Name + cse.Name + _YearId + "款", pageTagName, (pageNum <= 1 ? "单车" : ""));
				_PageKeyword = string.Format(pageKeyword, cse.SeoName + _YearId + "款", cse.Brand.MasterBrand.Name, cse.Name + _YearId + "款");
				_PageDescribe = string.Format(pageDescribe, cse.SeoName + _YearId + "款", pageTagName);
			}
			else
			{
				_PageTitle = string.Format(pageTitle, cse.SeoName, cse.Brand.MasterBrand.Name + cse.Name, pageTagName, (pageNum <= 1 ? "单车" : ""));
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
				dictPingceNews = base.GetPingceNewsByCsId(csID);
				if (dictPingceNews.ContainsKey(pageNum))
				{
					string url = dictPingceNews[pageNum].url;
					string[] arrTempURL = url.Split('/');
					string pageName = arrTempURL[arrTempURL.Length - 1];
					if (pageName.Length >= 10)
					{
						if (int.TryParse(pageName.Substring(3, 7), out newsId))
						{ }
						string[] arrPage = pageName.Split(new char[] { '-' });
						if (arrPage.Length > 1)
						{
							int endPos = arrPage[arrPage.Length - 1].IndexOf('.');
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
				DataSet ds = base.GetPingCeNewByNewID(newsId);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("content"))
				{
					HasPingCeNew = true;
					DataRow row = ds.Tables[0].Rows[0];
					if (ds.Tables[0].Columns.Contains("title"))
						PingCeTitle = row["title"].ToString();
					else
						PingCeTitle = row["facetitle"].ToString();
					PingCeFilePath = row["filepath"].ToString();
					PingCeUserName = row["author"].ToString();
					if (!string.IsNullOrEmpty(PingCeUserName))
					{
						PingCeUserName = "作者：" + PingCeUserName;
					}
					// Aug.12.2010 chengl 接口缺少sourceName 
					if (ds.Tables[0].Columns.Contains("sourceName"))
					{
						PingCeSourceName = row["sourceName"].ToString();
					}
					else
					{
						PingCeSourceName = "";
					}
					PingCePublishTime = Convert.ToDateTime(row["publishtime"].ToString()).ToString("f");
					string newsContent = row["content"].ToString();
					// string RegexString = "id=\"bt_pagebreak\"([^<].*?)</div>";
					string RegexString = "<div(?:[^<]*)?id=\"bt_pagebreak\"[^>]*>([^<]*)</div>";
					Regex r = new Regex(RegexString);
					string[] newsGroup = r.Split(newsContent);
					if (newsGroup.Length >= ((pageNum * 2) - 1))
					{
						PingCeContent = newsGroup[((pageNum * 2) - 2)];
					}

                    BaseUrl = GetBaseUrl();
					//初始化上一页，下一页的标签对象
					_TagTitleDescribeList.Add(1, "导语");
					//初始化标签标题内容
					InitTagTitleContent(newsGroup);
					//得到左侧滑动标记内容
                    GetTagContent(newsGroup, currentPageIndex, BaseUrl, PingCeTitle);
					//循环赋值头
					foreach (KeyValuePair<string, int> entity in TagTitleContent)
					{
						if (entity.Value == pageNum) PingCeTitle = entity.Key;
					}
					if (newsGroup.Length < 1) return;
					//生成翻页控件
                    GetPageHtml(newsGroup.Length, BaseUrl);
				}
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
                GetTagContent(null, currentPageIndex, BaseUrl, PingCeTitle);
                //循环赋值头
                foreach (KeyValuePair<string, int> entity in TagTitleContent)
                {
                    if (entity.Value == pageNum) PingCeTitle = entity.Key;
                }
            }
		}

        private string GetBaseUrl()
        {
            string baseUrl = string.Empty;
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
			this.PingceEditorComment = string.Empty;
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
						DataRow[] newsRows = _PingCeDs.Tables[0].Select("cmsnewsid=" + newsId.ToString());
						if (newsRows.Length > 0)
						{
							newsRow = newsRows[0];
						}
					}

					if (newsRow != null && !newsRow.IsNull("carId"))
					{
						string carId = newsRow["carId"].ToString();
						if (!string.IsNullOrEmpty(carId))
						{
							string htmlFile = Path.Combine(WebConfig.DataBlockPath, string.Format(_PingceEditorCommentHtmlPath, carId));
							if (File.Exists(htmlFile))
							{
								this.PingceEditorComment = File.ReadAllText(htmlFile);
							}
						}
					}
				}
			}
		}

        /// <summary>
        /// 获取推荐口碑
        /// </summary>
	    private void GetTuijianKoubeiHtml()
	    {
            int koubei = (int)CommonHtmlEnum.BlockIdEnum.KouBeiTuiJian;
            if (dictSerialBlockHtml.ContainsKey(koubei))
                TuijianKoubeiHtml = dictSerialBlockHtml[koubei];
	    }

	    private void GetKoubeiRatingHtml()
	    {
            int koubei = (int)CommonHtmlEnum.BlockIdEnum.KouBeiRating;
            if (dictSerialBlockHtml.ContainsKey(koubei))
                KoubeiRatingHtml = dictSerialBlockHtml[koubei];
	    }
        /// <summary>
        /// 竞品口碑
        /// </summary>
        private void MakeCompetitiveKoubeiHtml()
        {
            var serialsummaryBlock = _commonhtmlBLL.GetCommonHtml(csID, CommonHtmlEnum.TypeEnum.Serial, CommonHtmlEnum.TagIdEnum.SerialSummary);
            int competitive = (int)CommonHtmlEnum.BlockIdEnum.CompetitiveKoubei;
            if (serialsummaryBlock.ContainsKey(competitive))
                CompetitiveKoubeiHtml = serialsummaryBlock[competitive];
        }

		/// <summary>
		/// 得到更多评测内容
		/// </summary>
		private void GetMorePingCeContent()
		{
			if (_PingCeDs == null
				|| _PingCeDs.Tables.Count < 1
				|| _PingCeDs.Tables[0].Rows.Count < 1) return;

			StringBuilder moreContent = new StringBuilder();
			moreContent.Append("<div class=\"line-box\">");

			moreContent.Append("<div class=\"title-con\">");
			moreContent.Append("<div class=\"title-box\">");
			moreContent.AppendFormat("<h3>更多{0}评测</h3>", cse.ShowName);
			moreContent.Append("</div>");
			moreContent.Append("</div>");
			//moreContent.AppendFormat("<h3><span>更多{0}评测</span></h3>", cse.ShowName);
			int index = 0;
			int counter = 0;
			bool IsContainsSourceUrl = _PingCeDs.Tables[0].Columns.Contains("sourceUrl");
			moreContent.Append("<div class=\"tuwenlistbox\"><ul>");
			var query = _PingCeDs.Tables[0].AsEnumerable().Where(dr =>
			{
				int nId = ConvertHelper.GetInteger(dr["cmsnewsid"]);
				return nId != newsId;
			});
			foreach (DataRow dr in query)
			{
				int nId = ConvertHelper.GetInteger(dr["cmsnewsid"].ToString());
				if (nId == newsId)
				{
					index++;
					continue;
				}
				if (counter >= 3) break;
				string title = CommonFunction.NewsTitleDecode(dr["title"].ToString());
				//modified by sk 2013-10-09
				//string filePath = string.Format("/{0}/pingce/p{1}/", cse.AllSpell.ToLower(), nId);
				string filePath = dr["FilePath"].ToString();
				// modified by chengl Jan.17.2012
				// if (index == 0)
				//if (nId == newsId)
				//{
				//    filePath = string.Format("/{0}/pingce/", cse.AllSpell.ToLower());
				//}
				string fileDateTime = ConvertHelper.GetDateTime(dr["publishtime"]).ToString("yyyy年MM月dd日 hh:mm");
				//首图地址
				string imageUrl = "";
				//源名称
				string sourceName = dr["sourceName"].ToString();
				//源地址
				string sourceUrl = !IsContainsSourceUrl || dr.IsNull("sourceUrl") ? string.Empty : dr["sourceUrl"].ToString();
				string content = dr["content"].ToString();
				string editorName = ConvertHelper.GetString(dr["EditorName"]);
				string editUrl = GetEditNameAndUrl(dr);
				//如果链接不等于空
				if (!string.IsNullOrEmpty(sourceUrl))
				{
					sourceName = string.Format("<a href='{0}' target='_blank'>{1}</a>", sourceUrl, sourceName);
				}
				else if (sourceName == "易车")
				{
					sourceName = string.Format("<a href='{0}' target='_blank'>{1}</a>", "http://www.bitauto.com/", sourceName);
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
					content = StringHelper.SubString(content, 200, true);
				}

				///*
				// * 参数0:作者
				// * 参数1:时间
				// * 参数2:文章标题
				// * 参数3:文章地址
				// */
				//moreContent.Append("<dl>");
				//if (imageUrl.Length > 0)
				//    moreContent.AppendFormat("<dt><a target='_blank' href='{0}'><img style='width:150px;height:100px' alt='{2}' src='{1}'></a></dt>", filePath, imageUrl, title);
				//moreContent.AppendFormat("<dd><p><strong><a target='_blank' href='{0}'>{1}</a></strong></p>", filePath, title);
				//moreContent.AppendFormat("<p>{0} <a target='_blank' href='{1}'>查看详情&gt;&gt;</a></p>", content, filePath);
				//moreContent.AppendFormat("<p>{0}&nbsp;&nbsp;&nbsp;&nbsp;来源：{1}&nbsp;&nbsp;&nbsp;&nbsp;作者:{2}</p>"
				//    , fileDateTime, sourceName, editUrl);
				//moreContent.Append("</dd></dl>");

				moreContent.AppendFormat("<li class=\"{0}\">", (counter == 2 || query.Count() == (counter + 1)) ? "last" : "");
				if (imageUrl.Length > 0)
					moreContent.AppendFormat("<a href=\"{0}\" class=\"img\" target=\"_blank\"><img src=\"{1}\"></a>", filePath, imageUrl);
				moreContent.Append("<div class=\"textcont\">");
				moreContent.AppendFormat("<h3><a href=\"{0}\" target=\"_blank\">{1}</a></h3>", filePath, title);
				moreContent.Append("<p class=\"info\">");
				moreContent.AppendFormat("<span>{0}</span>", fileDateTime);
				moreContent.AppendFormat("<span>来源:<em>{0}</em></span>", sourceName);
				if (!string.IsNullOrEmpty(editUrl))
					moreContent.AppendFormat("<span>作者:<em>{0}</em></span></p>", editUrl);
				else
				{
					if (!string.IsNullOrEmpty(editorName))
						moreContent.AppendFormat("<span>作者:<em>{0}</em></span></p>", editorName);
				}
				moreContent.AppendFormat("<p class=\"text\">{0} <a href=\"{1}\" target=\"_blank\">详细&gt;&gt;</a></p>", content, filePath);
				moreContent.Append("</div>");
				moreContent.Append("</li>");
				index++;
				counter++;
			}
			moreContent.Append("</ul></div>");
			moreContent.Append("<div class=\"clear\"></div>");
			moreContent.Append("</div>");
			_MorePingCeContent = moreContent.ToString();
		}

		/// <summary>
		/// 得到分页标识
		/// </summary>
		/// <param name="pagesize"></param>
		/// <param name="baseUrl"></param>
		/// <returns></returns>
		private void GetPageHtml(int pagesize, string baseUrl)
		{
			//更改分页规则，新闻ID规则不变 by sk 2013.01.11
			sbPagination.Append("<div id='pagePagination' class=\"the_pages\"><div>");
			if (dictPingceNews.Count > 0)
			{
				//if (currentPageIndex > 1)
				//{
				//    int preIndex = currentPageIndex - 1;
				//    if (dictPingceNews.ContainsKey(preIndex))
				//        sbPagination.Append("<a href=\"" + baseUrl + preIndex + "/\" class=\"preview_on\">上一页:" + dictPingceNews[preIndex].tagName + "</a>");
				//    else
				//        sbPagination.Append("<a href=\"" + baseUrl + preIndex + "/\" class=\"preview_on\">上一页</a>");
				//}
				int j = 0;
				foreach (KeyValuePair<int, EnumCollection.PingCeTag> kvp in dictPingceNews)
				{
					j++;
					if (j == currentPageIndex)
						sbPagination.Append("<a class=\"linknow\">" + j + "</a>");
					else
						sbPagination.Append("<a href=\"" + baseUrl + j + "/\">" + j + "</a>");
				}
				if (currentPageIndex < dictPingceNews.Count)
				{
					int nextIndex = currentPageIndex + 1;
					if (dictPingceNews.ContainsKey(nextIndex))
						sbPagination.AppendLine("<a href=\"" + baseUrl + nextIndex + "/\" class=\"next_on\">下一页:" + dictPingceNews[nextIndex].tagName + "</a>");
					else
						sbPagination.AppendLine("<a href=\"" + baseUrl + nextIndex + "/\" class=\"next_on\">下一页</a>");
				}
			}
			else
			{
				pageCount = (pagesize / 2) + 1;
				if (pageNum > 1)
				{
					int preIndex = pageNum - 1;
					//sbPagination.Append("<a href=\"" + baseUrl + "\" class=\"preview_on\">首页</a>");
					if (preIndex == 1)
					{
						sbPagination.Append("<a href=\"" + baseUrl + "\" class=\"preview_on\">上一页:" + _TagTitleDescribeList[preIndex] + "</a>");
					}
					else if (_TagTitleDescribeList.ContainsKey(preIndex))//如果上一页包含缩略标题
					{
						sbPagination.Append("<a href=\"" + baseUrl + preIndex + "/\" class=\"preview_on\">上一页:" + _TagTitleDescribeList[preIndex] + "</a>");
					}
					else
					{
						sbPagination.Append("<a href=\"" + baseUrl + preIndex + "/\" class=\"preview_on\">上一页</a>");
					}
				}

				int startPageIndex = pageNum - 5;
				if (startPageIndex < 1)
				{ startPageIndex = 1; }
				int endPageIndex = startPageIndex + 10 - 1;
				if (endPageIndex > pageCount)
					endPageIndex = pageCount;

				startPageIndex = endPageIndex - 10 + 1;
				if (startPageIndex < 1)
				{ startPageIndex = 1; }

				for (int i = startPageIndex; i <= endPageIndex; i++)
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

				if (pageNum < pageCount)
				{
					int nextIndex = pageNum + 1;
					if (!_TagTitleDescribeList.ContainsKey(nextIndex))
					{
						sbPagination.AppendLine("<a href=\"" + baseUrl + nextIndex + "/\" class=\"next_on\">下一页</a>");
					}
					else
					{
						sbPagination.AppendLine("<a href=\"" + baseUrl + nextIndex + "/\" class=\"next_on\">下一页:" + _TagTitleDescribeList[nextIndex] + "</a>");
					}
					//sbPagination.AppendLine("<a href=\"" + baseUrl + pageCount + "/\" class=\"next_on\">末页</a>");
				}
			}
			sbPagination.Append("</div></div>");
			PingCePagination = sbPagination.ToString();
		}

		/// <summary>
		/// 得到标记内容
		/// </summary>
		private void GetTagContent(string[] contentList, int curPageNum, string baseUrl, string firstPageTitle)
		{
			StringBuilder htmlCode = new StringBuilder();
			//新提取规则，新闻ID访问规则不变 by sk 2013.01.10
			if (dictPingceNews.Count > 0)
			{
				int pageIndex = 0;
				foreach (KeyValuePair<int, EnumCollection.PingCeTag> kvp in dictPingceNews)
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
						htmlCode.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", baseUrl + pageIndex + "/", kvp.Value.tagName);
					}
				}
			}
			else
			{
				string title = "";
				int pageIndex = 0;
				// modified by chengl Dec.28.2011
				// foreach (string entity in tagList)
				foreach (KeyValuePair<int, EnumCollection.PingCeTag> kvp in dicAllTagInfo)
				{
					bool IsContains = false;
					// if (entity == "导语" && curPageNum == 1)
					if (kvp.Value.tagName == "导语" && curPageNum == 1)
					{
						htmlCode.Append("<li class=\"current\">导语</li>");
						pageTagName = "";
                        pageFirstTagName = "评测"; 
						continue;
					}
					// else if (entity == "导语")
					else if (kvp.Value.tagName == "导语")
					{
						htmlCode.AppendFormat("<li><a href=\"{0}\">导语</a></li>", baseUrl + "1/");
						continue;
					}
					IsContains = GetTitleIsContainsTag(kvp.Value, out title, out pageIndex);
					if (IsContains && pageIndex == curPageNum)
					{
						// htmlCode.AppendFormat("<li class=\"current\">{0}</li>", entity);
						htmlCode.AppendFormat("<li class=\"current\">{0}</li>", kvp.Value.tagName);
						PingCeTitle = title;
						// pageTagName = entity;
						pageTagName = kvp.Value.tagName;
                        pageFirstTagName = kvp.Value.tagName;
						continue;
					}
					else if (IsContains)
					{
						// htmlCode.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", baseUrl + pageIndex + "/", entity);
						htmlCode.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", baseUrl + pageIndex + "/", kvp.Value.tagName);
						continue;
					}
					//htmlCode.AppendFormat("<li>{0}</li>", entity);
				}
			}
			_LeftTagContent = htmlCode.ToString();

		}

		/// <summary>
		/// 得到标签是否可以匹配到标题
		/// modified by chengl Dec.28.2011
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="contentList"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		// private bool GetTitleIsContainsTag(string tag, out string title, out int pageIndex)
		private bool GetTitleIsContainsTag(EnumCollection.PingCeTag tag, out string title, out int pageIndex)
		{
			Regex r = new Regex(tag.tagRegularExpressions);
			title = "";
			pageIndex = 0;
			bool IsContains = false;
			foreach (KeyValuePair<string, int> entity in TagTitleContent)
			{
				// modified by chengl Dec.28.2011
				if (!r.IsMatch(entity.Key))
				{ continue; }

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
		/// 初始化标签标题内容
		/// </summary>
		private void InitTagTitleContent(string[] contentList)
		{
			Regex rex = new Regex(@"\$\$(?<title>.+)\$\$");
			int pageIndex = (contentList.Length + 1) / 2 + 1;
			string title = string.Empty;
			for (int i = contentList.Length - 2; i > 0; i -= 2)
			{
				pageIndex--;
				string tmpStr = contentList[i];
				try
				{
					title = rex.Match(tmpStr).Result("${title}");
				}
				catch { }
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
		/// 得到编辑链接
		/// </summary>
		/// <param name="dr"></param>
		/// <returns></returns>
		private string GetEditNameAndUrl(DataRow dr)
		{
			int uid = ConvertHelper.GetInteger(dr["EditorId"].ToString());
			if (uid == 0) return string.Empty;
			//用户列表
			Dictionary<int, Editer> editerlist = new NewsEditerBll().GetNewsEditerList();
			if (editerlist == null || !editerlist.ContainsKey(uid)) return string.Empty;

			Editer editer = editerlist[uid];

			if (string.IsNullOrEmpty(editer.UserBlogUrl))
				return editer.UserName;
			return string.Format("<a href='{0}' target='_blank'>{1}</a> ", editer.UserBlogUrl, editer.UserName);

		}

		/// <summary>
		/// 获取子品牌视频
		/// </summary>
		private void MakeSerialVideoHtml()
		{
			var videoList = VideoBll.GetVideoBySerialId(csID);
			if (videoList.Count <= 0) return;
			StringBuilder sb = new StringBuilder();
			sb.Append("<div class=\"line-box\">");
			sb.Append("<div class=\"side_title\">");
			sb.AppendFormat("<h4><a href='http://v.bitauto.com/car/serial/{0}.html' target='_blank'>{1}视频</a></h4>",
				csID, strCs_ShowName.Replace("(进口)", "").Replace("（进口）", ""));
			sb.Append("</div>");
			sb.Append("<div class=\"theme_list play_ol\">");
			sb.Append("<ul class=\"video_list\">");
			foreach (var entity in videoList)
			{

				sb.Append("<li class=\"fist\">");
				sb.AppendFormat("<a target=\"_blank\" href=\"{0}\" class=\"img\"><img src=\"{1}\"></a>", entity.ShowPlayUrl, entity.ImageLink);
				sb.AppendFormat("<p><a href=\"{0}\" target=\"_blank\">{1}</a></p>", entity.ShowPlayUrl, entity.ShortTitle);
				sb.Append("</li>");
			}
			sb.Append("</ul>");
			sb.Append("</div>");
			sb.Append("</div>");
			_SerialShiPinHtml = sb.ToString();
		}

		/// <summary>
		/// 得到品牌下的其他子品牌
		/// </summary>
		/// <returns></returns>
		protected string GetBrandOtherSerial()
		{
			List<CarSerialPhotoEntity> carSerialPhotoList = new Car_BrandBll().GetCarSerialPhotoListByCBID(cse.BrandId, false);

			carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

			if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
			{
				return "";
			}

			int forLastCount = 0;
			foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
			{
				if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Id)
				{
					continue;
				}
				forLastCount++;
			}

			StringBuilder contentBuilder = new StringBuilder(string.Empty);
			string serialUrl = "<a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}</a>";
			int index = 0;
			foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
			{
				bool IsExitsUrl = true;
				if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Id)
				{
					continue;
				}
				string priceRang = base.GetSerialPriceRangeByID(entity.SerialId);
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
					priceRang = string.Format("<a href='http://price.bitauto.com/brand.aspx?newbrandid={0}'>{1}</a>", entity.SerialId, priceRang);
				}
				string tempCsSeoName = string.IsNullOrEmpty(entity.CS_SeoName) ? entity.ShowName : entity.CS_SeoName;
				index++;
				contentBuilder.AppendFormat("<li>{0}<span class=\"dao\">{1}</span></li>"
					, string.Format(serialUrl, entity.CS_AllSpell, tempCsSeoName), priceRang
					 );
			}

			StringBuilder brandOtherSerial = new StringBuilder(string.Empty);
			if (contentBuilder.Length > 0)
			{
				brandOtherSerial.Append("<div class=\"side_title\">");
				brandOtherSerial.AppendFormat("<h4><a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}其他车型</a></h4>",
					cse.Brand.AllSpell, cse.Brand.Name);
				brandOtherSerial.Append("</div>");

				brandOtherSerial.Append("<ul class=\"text-list\">");

				brandOtherSerial.Append(contentBuilder.ToString());

				brandOtherSerial.Append("</ul>");
			}

			return brandOtherSerial.ToString();
		}

		private void MakeSerialToSerialHtml()
		{
            serialToSeeJson = serialBll.GetSerialSeeToSeeJson(csID, 8);
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
		/// 生成分页导航
		/// </summary>
		/// <param name="contentList"></param>
		/// <param name="curPageNum"></param>
		/// <returns></returns>
		private string GetPageListHtml(string[] contentList, int curPageNum, string baseUrl, string firstPageTitle)
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<div id=\"content_bit\">");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"con_nav\">");
			htmlCode.AppendLine("<tbody>");
			htmlCode.AppendLine("<tr><th scope=\"col\" colspan=\"2\">分页导航：</th></tr>");
			Regex rex = new Regex(@"\$\$(?<title>.+)\$\$");
			int pageIndex = 0;
			for (int i = -1; i < contentList.Length; i += 2)
			{
				pageIndex++;
				string url = baseUrl + pageIndex + "/";
				string title = "";
				if (i == -1)
					title = firstPageTitle;
				else
				{
					string tmpStr = contentList[i];
					try
					{
						title = rex.Match(tmpStr).Result("${title}");
					}
					catch { }
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
			StringBuilder sb = new StringBuilder();
			List<EnumCollection.SerialHotCompareData> lshcd = base.GetSerialHotCompareByCsID(csID, 5);
			if (lshcd.Count > 0)
			{
				sb.Append("<div class=\"line-box line-box_t0\" id=\"\">");
				sb.Append("<div class=\"side_title\">");
				sb.Append("<h4><a rel=\"nofollow\" href=\"/tupianduibi/\" target=\"_blank\">车型图片对比</a></h4>");
				sb.Append("</div>");


				sb.Append("<ul class=\"text-list\">");
				foreach (EnumCollection.SerialHotCompareData shcd in lshcd)
				{
					sb.AppendFormat("<li><a href=\"/tupianduibi/?csids={2},{3}\" target=\"_blank\">{0}<em class=\"vs\">VS</em>{1}</a></li>",
						strCs_ShowName, shcd.CompareCsShowName, shcd.CurrentCsID, shcd.CompareCsID);
				}
				sb.Append("</ul>");
				sb.Append("<div class=\"clear\"></div>");
				sb.Append("</div>");
			}
			serialPhotoHotCompareHtml = sb.ToString();
		}

		/// <summary>
		/// 生成小导航代码
		/// </summary>
		/// <returns></returns>
		private string RenderNewsNav()
		{
			StringBuilder htmlCode = new StringBuilder();
			if (_YearId > 0)
			{
				htmlCode.AppendFormat("<div class=\"year\">{0}款</div>", _YearId);
				htmlCode.AppendFormat("<ul class=\"car_tab_daogou {0}\">", "car_tab_daogou_right");
			}
			else
			{
				htmlCode.AppendFormat("<ul class=\"car_tab_daogou {0}\">", "");
			}
			EnumCollection.SerialInfoCard sic = serialBll.GetSerialInfoCard(csID);
			if (_PingCeNewsCount > 0)
				htmlCode.AppendLine(string.Format("<li class=\"current\">易车评测<span>({0})</span></li>", _PingCeNewsCount.ToString()));
			else
				htmlCode.AppendLine("<li class=\"current\">易车评测</li>");

			Dictionary<CarNewsType, string> titleTag = new Dictionary<CarNewsType, string>();
			titleTag.Add(CarNewsType.shijia, "体验试驾");
			//titleTag.Add("xinwen", "新闻");
			//titleTag.Add("hangqing", "行情");
			titleTag.Add(CarNewsType.daogou, "导购");
			titleTag.Add(CarNewsType.yongche, "用车");
			titleTag.Add(CarNewsType.gaizhuang, "改装");
			titleTag.Add(CarNewsType.anquan, "安全");
			titleTag.Add(CarNewsType.xinwen, "新闻");

			string baseUrl = string.Empty;
			if (_YearId > 0)
			{
				baseUrl = string.Format("/{0}/{1}/", cse.AllSpell.ToLower(), _YearId.ToString());
			}
			else
			{
				baseUrl = string.Format("/{0}/", cse.AllSpell.ToLower());
			}
			CarNewsBll newsBll = new CarNewsBll();
			foreach (KeyValuePair<CarNewsType, string> entity in titleTag)
			{
				int newsCount = newsBll.GetSerialNewsCount(csID, _YearId, entity.Key);
				if (newsCount > 0)
					htmlCode.AppendFormat("<li><a href=\"{0}{1}/\">{2}<span>({3})</span></a></li>", baseUrl, entity.Key.ToString(), entity.Value, newsCount.ToString());
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
				List<string> htmlList = new List<string>();
				Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = new Dictionary<string, List<Car_SerialBaseEntity>>();
				carSerialBaseList = serialBll.GetSerialCityCompareList(csID, HttpContext.Current);
				string compareBaseUrl = "/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?count=3&csids=" + csID + ",";
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

					List<Car_SerialBaseEntity> serialCompareList = carSerialBaseList["全国"];
					htmlList.Add("<div class=\"line_box h160\" id=\"serialHotCompareList\">");
					htmlList.Add("<h3><span>网友都用它和谁比</span></h3>");
					htmlList.Add("<div class=\"more\"><a href=\"/chexingduibi/\" target=\"_blank\">车型对比&gt;&gt;</a></div>");
					htmlList.Add("<div class=\"ranking_list\" id=\"rank_model_box\">");
					htmlList.Add("<ol class=\"carContrast\">");

					for (int i = 0; i < serialCompareList.Count; i++)
					{
						Car_SerialBaseEntity carSerial = serialCompareList[i];
						if (i == serialCompareList.Count - 1)
							htmlList.Add("<li class=\"last\">");
						else
							htmlList.Add("<li>");
						htmlList.Add("<em>" + BitAuto.Utils.StringHelper.SubString(strCs_ShowName, 10, false) + " <s>VS</s> ");
						htmlList.Add(carSerial.SerialShowName.Trim() + "</em>");
						htmlList.Add("<span><a href=\"" + compareBaseUrl + carSerial.SerialId + "\" target=\"_blank\">对比&gt;&gt;</a></span></li>");
					}

					htmlList.Add("</ol></div>");
					htmlList.Add("</div>");
				}

				HotCarCompare = String.Concat(htmlList.ToArray());
			}
		}

		/// <summary>
		/// 取子品牌对比排行数据
		/// </summary>
		/// <returns></returns>
		private void MakeHotSerialCompare()
		{
			Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = serialBll.GetSerialCityCompareList(csID, HttpContext.Current);
			List<string> htmlList = new List<string>();
			string compareBaseUrl = "/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?count=3&csids=" + csID + ",";
			string serialCompareForPkUrl = "/duibi/" + csID + "-";
			if (carSerialBaseList != null && carSerialBaseList.ContainsKey("全国"))
			{
				List<Car_SerialBaseEntity> serialCompareList = carSerialBaseList["全国"];
				htmlList.Add("<div class=\"line-box\" id=\"serialHotCompareList\">");

				htmlList.Add("<div class=\"side_title\">");
				htmlList.Add("<h4><a rel=\"nofollow\" href=\"/chexingduibi/\" target=\"_blank\">大家都用他和谁比</a></h4>");
				htmlList.Add("</div>");


				//htmlList.Add("<div id=\"rank_model_box\" class=\"ranking_list\">");
				htmlList.Add("<ul class=\"text-list\">");

				for (int i = 0; i < serialCompareList.Count; i++)
				{
					Car_SerialBaseEntity carSerial = serialCompareList[i];
					htmlList.Add("<li>");
					htmlList.Add(string.Format("<a href=\"" + serialCompareForPkUrl + carSerial.SerialId + "/\" target=\"_blank\">{0}<em class=\"vs\">VS</em>{1}</a>",
						BitAuto.Utils.StringHelper.SubString(strCs_ShowName, 10, false),
						carSerial.SerialShowName.Trim()));
					htmlList.Add("</li>");
				}

				htmlList.Add("</ul>");
				htmlList.Add("<div class=\"clear\"></div>");
				htmlList.Add("</div>");
			}

			carCompareHtml = String.Concat(htmlList.ToArray());
		}

		/// <summary>
		/// 得到子品牌评测
		/// </summary>
		private void GetSerialPingCeShiPinHtml()
		{
			string filePath = Path.Combine(WebConfig.DataBlockPath, string.Format(_PingCheShiPinFilePath, csID));
			if (!File.Exists(filePath)) return;

			XmlDocument xmlDoc = new XmlDocument();

			xmlDoc.Load(filePath);
			if (xmlDoc == null) return;

			XmlNamespaceManager xmlns = new XmlNamespaceManager(xmlDoc.NameTable);
			xmlns.AddNamespace("ns", "http://schemas.datacontract.org/2004/07/BitAuto.Video.RESTfulApi.ShowModel");

			XmlNodeList xNodelist = xmlDoc.SelectNodes("/ns:ListVideo/ns:ListCarVideo/ns:CarVideo", xmlns);
			if (xNodelist == null || xNodelist.Count < 1) return;
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.Append("<div class='line_box car_img_box'>");
			htmlCode.AppendFormat("<h3><span><a href='http://v.bitauto.com/car/serial/{0}.html' target='_blank'>{1}视频</a></span></h3>", csID, strCs_ShowName.Replace("(进口)", "").Replace("（进口）", ""));
			htmlCode.Append("<div class='car_img car_v_box'>");
			XmlNode xNode = xNodelist[0];
			string title = xNode.SelectSingleNode("ns:ShortTitle", xmlns) != null ?
				xNode.SelectSingleNode("ns:ShortTitle", xmlns).InnerText : "";
			string image = xNode.SelectSingleNode("ns:ImageLink", xmlns) != null ?
				xNode.SelectSingleNode("ns:ImageLink", xmlns).InnerText.Replace(".bitautoimg.com/", ".bitautoimg.com/wapimg-150-9999/") : "";
			string fileUrl = xNode.SelectSingleNode("ns:ShowPlayUrl", xmlns) != null ?
				xNode.SelectSingleNode("ns:ShowPlayUrl", xmlns).InnerText : "";

			htmlCode.AppendFormat("<a target='_blank' href='{0}'>", fileUrl);
			htmlCode.AppendFormat("<img width='150' height='100' src='{0}'></a>", image);
			htmlCode.AppendFormat("<a target='_blank' href='{0}' class='car_img_t'>{1}</a>", fileUrl, title);
			htmlCode.Append("<ul class='video_list'>");
			for (int i = 1; i < xNodelist.Count; i++)
			{
				if (i > 6) break;
				xNode = xNodelist[i];
				title = xNode.SelectSingleNode("ns:ShortTitle", xmlns) != null ?
					xNode.SelectSingleNode("ns:ShortTitle", xmlns).InnerText : "";
				fileUrl = xNode.SelectSingleNode("ns:ShowPlayUrl", xmlns) != null ?
					xNode.SelectSingleNode("ns:ShowPlayUrl", xmlns).InnerText : "";

				htmlCode.Append("<li>");
				htmlCode.AppendFormat("<a target='_blank' href='{0}'>{1}</a>", fileUrl, title);
				htmlCode.Append("</li>");
			}
			htmlCode.Append("</ul>");
			htmlCode.Append("</div>");
			htmlCode.AppendFormat("<div class='more'><a href='http://v.bitauto.com/car/serial/{0}.html' target='_blank'>更多&gt;&gt;</a></div>", csID);
			htmlCode.Append("</div>");
			_SerialShiPinHtml = htmlCode.ToString();
		}

		/// <summary>
		/// 得到子品牌的图片块
		/// </summary>
		private void GetSerialImageHtml()
		{
			string filePath = Path.Combine(WebConfig.DataBlockPath, string.Format(_PingCheImageFilePath, csID));
			if (!File.Exists(filePath)) return;

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(filePath);

			if (xmlDoc == null) return;
			XmlNodeList xNodelist = xmlDoc.SelectNodes("data/ImageList");
			if (xNodelist == null || xNodelist.Count < 1) return;
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.Append("<div class='line_box pic_list_box'>");
			htmlCode.AppendFormat("<h3><span><a target='_blank' href='http://photo.bitauto.com/serial/{0}/'>{1}图片</a></span></h3>", csID, strCs_ShowName.Replace("(进口)", "").Replace("（进口）", ""));
			htmlCode.Append("<ul class='pic_list'>");

			foreach (XmlNode xNode in xNodelist)
			{
				string title = xNode.SelectSingleNode("SiteImageName").InnerText;
				int imgId = ConvertHelper.GetInteger(xNode.SelectSingleNode("SiteImageId").InnerText);
				string url = xNode.SelectSingleNode("SiteImageUrl").InnerText;
				url = GetPublishImage(2, url, imgId);
				htmlCode.Append("<li>");
				htmlCode.AppendFormat("<a target='_blank' href='http://photo.bitauto.com/serial/{0}/' title='{1}'>", csID, title);
				htmlCode.AppendFormat("<img width='90' height='60' src='{0}'>", url);
				htmlCode.Append("</a>");
				htmlCode.AppendFormat("<a target='_blank' href='http://photo.bitauto.com/serial/{0}/'>{1}</a>", csID, StringHelper.SubString(title, 13, false));
				htmlCode.Append("</li>");
			}

			htmlCode.Append("</ul>");
			htmlCode.Append("<div class='clear'></div>");
			htmlCode.AppendFormat("<div class='more'><a href='http://photo.bitauto.com/serial/{0}/' target='_blank'>更多&gt;&gt;</a></div>", csID);
			htmlCode.Append("</div>");
			_SerialImageHtml = htmlCode.ToString();
		}
	}
}