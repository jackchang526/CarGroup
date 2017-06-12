using System;
using System.Net;
using System.Xml;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.PageSerial
{
	public partial class CsPhoto : PageBase
	{
		protected string strCS_Name = string.Empty;
		protected string strCs_ShowName = string.Empty;
		protected string strCs_SeoName = string.Empty;
		protected string strCs_MasterName = string.Empty;
		protected int nPhotoCount = 0;
		protected string CsHeadHTML = string.Empty;
		protected int serialId = 0;
		protected string CsHotCompareCars = string.Empty;
		//protected string CsSerialToSerial = string.Empty;
		//private const string DIC_KEY_CS_NAME            = "CS_NAME";
		private const string DIC_KEY_CS_PHOTOCOUNT = "CS_PHOTOCOUNT";
		private const string DIC_KEY_CS_PHOTOCATEGORY = "CS_PHOTOCATEGORY";//车型图片(上)
		private const string DIC_KEY_CS_PHOTOLIST = "CS_PHOTOLIST";//车型图片(列表)
		private const string DIC_KEY_CS_PHOTOTYPELIST = "CS_PHOTOTYPELIST";
		private EnumCollection.SerialInfoCard sic;
		protected Car_SerialEntity cse;
		protected string UserBlock;
		protected string baaUrl;
		protected string ColorPicList;

		protected string PhotoProvideCateHTML = string.Empty;

		protected string nextSeePingceHtml;
		protected string nextSeeXinwenHtml;
		protected string nextSeeDaogouHtml;
		protected string UCarHtml;
		protected string PhotoProvideColorRGBHTML;
		protected string SerialPhotoHtml = string.Empty;
		//protected string serialToSeeHtml = string.Empty;
        protected string serialToSeeJson = string.Empty;
		// 子品牌信息栏
		protected string SerialInfoBarHtml;

		private Car_SerialBll serialBll;
		public CsPhoto()
		{
			serialBll = new Car_SerialBll();
		}
		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
			if (!IsPostBack)
			{
				GetParams();
				base.MakeSerialTopADCode(serialId);
				if (serialId > 0)
				{
					#region 子品牌名片及基本数据
					string catchKeyCard = "CsSummaryCsCard_CsID" + serialId.ToString();
					object serialInfoCardByCsID = null;
					CacheManager.GetCachedData(catchKeyCard, out serialInfoCardByCsID);
					sic = new EnumCollection.SerialInfoCard();
					if (serialInfoCardByCsID == null)
					{
						sic = base.GetSerialInfoCardByCsID(serialId);
						CacheManager.InsertCache(catchKeyCard, sic, 60);
					}
					else
					{
						sic = (EnumCollection.SerialInfoCard)serialInfoCardByCsID;
					}

					if (sic.CsID == 0)
					{
						Response.Redirect("/car/404error.aspx?info=无子品牌");
					}

					// add by chengl May.17.2012 高岩要求开放概念车
					//if (sic.CsLevel == "概念车")
					//{
					//    Response.Redirect("/car/404error.aspx?info=概念车无图片页");
					//}

					string catchKeyEntity = "CsSummaryEntity_CsID" + serialId.ToString();
					object serialInfoEntityByCsID = null;
					CacheManager.GetCachedData(catchKeyEntity, out serialInfoEntityByCsID);
					cse = new Car_SerialEntity();

					if (serialInfoEntityByCsID == null)
					{
						cse = serialBll.Get_Car_SerialByCsID(serialId);
						CacheManager.InsertCache(catchKeyEntity, cse, 60);
					}
					else
					{
						cse = (Car_SerialEntity)serialInfoEntityByCsID;
					}
					#endregion
					baaUrl = serialBll.GetForumUrlBySerialId(serialId);
					SerialPhotoHtml = Car_SerialBll.GetSerialPhotoHtml(serialId);
					strCS_Name = cse.Cs_Name;
					strCs_SeoName = cse.Cs_SeoName;
					strCs_ShowName = cse.Cs_ShowName;
					if (serialId == 1568)
						strCs_ShowName = "索纳塔八";
					strCs_MasterName = cse.Cb_Name.Trim();// .cse.MasterName;
					#region 弃用之前调用方式 by sk 2013.03.21
					//Dictionary<string, string> dicCSPhotoPageInfo = GetRenderedCSPhotoListHTMLByCSID(serialId);
					//if (dicCSPhotoPageInfo.Count > 0)
					//{
					//    strCS_Name = cse.Cs_Name;
					//    strCs_SeoName = cse.Cs_SeoName;
					//    strCs_ShowName = cse.Cs_ShowName;
					//    if (serialId == 1568)
					//        strCs_ShowName = "索纳塔八";
					//    strCs_MasterName = cse.Cb_Name.Trim();// .cse.MasterName;
					//    nPhotoCount = string.IsNullOrEmpty(dicCSPhotoPageInfo[DIC_KEY_CS_PHOTOCOUNT]) ? 0 : Convert.ToInt32(dicCSPhotoPageInfo[DIC_KEY_CS_PHOTOCOUNT]);
					//    //ltrCSPotoType.Text = dicCSPhotoPageInfo[DIC_KEY_CS_PHOTOTYPELIST];
					//    //ltrCSPhotoList.Text = dicCSPhotoPageInfo[DIC_KEY_CS_PHOTOLIST];
					//    //ltlCategory.Text = dicCSPhotoPageInfo[DIC_KEY_CS_PHOTOCATEGORY];

					//    #region 子品牌颜色图片
					//    // 子品牌颜色图片
					//    StringBuilder _sbColor = new StringBuilder();
					//    Dictionary<string, XmlNode> dicColor = serialBll.GetSerialColorPhotoByCsID(serialId, 0);
					//    if (dicColor != null && dicColor.Count > 0)
					//    {
					//        _sbColor.AppendLine("<div class=\"thepic_list_part_color thepic_list_oneline_color\">");
					//        _sbColor.AppendLine("<h5>车身颜色</h5>");
					//        _sbColor.AppendLine("<div class=\"carColor\">");
					//        _sbColor.AppendLine("<b id=\"LeftArr\" class=\"lGray\" >左</b>");
					//        _sbColor.AppendLine("<b id=\"RightArr\" class=\"r\" >右</b>");
					//        _sbColor.AppendLine("<div class=\"carColor_inner\">");
					//        _sbColor.AppendLine("<div id=\"innerBox\" style=\"top: 0pt; left: 0pt;\">");
					//        _sbColor.AppendLine("<ul id=\"colorBox\">");
					//        foreach (KeyValuePair<string, XmlNode> keyColor in dicColor)
					//        {
					//            if (keyColor.Value.Attributes["ImageUrl"] != null && keyColor.Value.Attributes["ImageUrl"].Value.Trim() != "" && keyColor.Value.Attributes["Link"] != null && keyColor.Value.Attributes["Link"].Value.Trim() != "")
					//            {
					//                _sbColor.AppendLine("<li>");
					//                _sbColor.AppendLine("<div>");
					//                _sbColor.AppendLine("<a target=\"_blank\" href=\"" + keyColor.Value.Attributes["Link"].Value.Trim() + "\">");
					//                _sbColor.AppendLine("<img src=\"" + keyColor.Value.Attributes["ImageUrl"].Value.Trim() + "\" alt=\"\" />");
					//                _sbColor.AppendLine("</a></div>");
					//                _sbColor.AppendLine("<a target=\"_blank\" href=\"" + keyColor.Value.Attributes["Link"].Value.Trim() + "\">" + keyColor.Key.Trim() + "</a>");
					//                _sbColor.AppendLine("</li>");
					//            }
					//        }
					//        _sbColor.AppendLine("</ul>");
					//        _sbColor.AppendLine("</div></div></div></div>");
					//        //_sbColor.AppendLine("<div class=\"line\"></div>");
					//        //_sbColor.AppendLine("<div class=\"clear\"></div>");
					//        ColorPicList = _sbColor.ToString();
					//    }
					//    #endregion

					//    #region 图库提供分类HTML
					//    PhotoProvideCateHTML = serialBll.GetPhotoProvideCateHTML(serialId);

					//    // add by chengl Oct.18.2011
					//    // 颜色分类
					//    List<EnumCollection.SerialColorItem> listESCI = serialBll.GetPhotoSerialCarColorByCsID(serialId);

					//    if (listESCI != null && listESCI.Count > 0)
					//    {
					//        List<string> listColorRGBHTML = new List<string>();
					//        listColorRGBHTML.Add("<div class=\"carPic02 carPic02_next\">");
					//        listColorRGBHTML.Add("<dl class=\"color\">");
					//        listColorRGBHTML.Add("<dt>按颜色：</dt>");
					//        listColorRGBHTML.Add("<dd class=\"w35\"><strong>不限</strong></dd>");
					//        listColorRGBHTML.Add("<dd class=\"w550\">");
					//        listColorRGBHTML.Add("<ul>");
					//        // 排重
					//        List<int> listHasColor = new List<int>();
					//        foreach (EnumCollection.SerialColorItem sci in listESCI)
					//        {
					//            if (!listHasColor.Contains(sci.ColorID))
					//            {
					//                listColorRGBHTML.Add("<li><a target=\"_blank\" href=\"http://photo.bitauto.com/serial/" + serialId + "/c" + sci.ColorID + "/\"><em><span style=\"background:" + sci.ColorRGB + "\">" + sci.ColorName + "</span></em>" + sci.ColorName + "</a></li>");
					//                listHasColor.Add(sci.ColorID);
					//            }
					//        }
					//        listColorRGBHTML.Add("</ul></dd></dl>");
					//        listColorRGBHTML.Add("<div class=\"clear\"></div>");
					//        listColorRGBHTML.Add("</div>");
					//        PhotoProvideColorRGBHTML = string.Concat(listColorRGBHTML.ToArray());
					//    }
					//    #endregion

					//}
					#endregion
					//bool isSuccess = false;
					//CsHeadHTML = this.GetRequestString(string.Format(WebConfig.HeadForSerial, CSID.ToString(), "CsPhoto"), 10, out isSuccess);
					CsHeadHTML = base.GetCommonNavigation("CsPhoto", serialId);
					SerialInfoBarHtml = base.GetCommonNavigation("SerialInfoBar", serialId);
					//图库内容   modified by sk 2013.03.21
					GetSerialPhotoHtml(serialId);
					//还看过的
					MakeSerialToSerialHtml();
					//ucSerialToSee.SerialId = serialId;
					//ucSerialToSee.SerialName = strCs_ShowName;

					CsHotCompareCars = this.GetSerialHotCompareCars();

					// modified by chengl May.5.2011
					// GetUserBlockByCarSerialId();
					//接下来要看的
					//InitNextSee();
					InitNextSeeNew();

					//UCarHtml = serialBll.GetUCarHtml(serialId);
				}
			}
		}
		// modified by sk 2013.03.21
		private void GetSerialPhotoHtml(int serialId)
		{
			SerialPhotoHtml = Car_SerialBll.GetSerialPhotoHtml(serialId);
			if (string.IsNullOrEmpty(SerialPhotoHtml))
			{
				StringBuilder sb = new StringBuilder();
				//sb.Append("<div class=\"line_box\">");
				//sb.Append("				<!--选车结果-->");
				//sb.Append("				<div class=\"error_page\">");
				//sb.Append("					<h1>");
				//sb.Append("						很抱歉，该车型暂无图片！</h1>");
				//sb.Append("					<p>");
				//sb.Append("					</p>");
				//sb.Append("					<h4>");
				//sb.Append("						建议您：</h4>");
				//sb.Append("					<ul>");
				//sb.Append("						<li>我们正在努力更新，请查看其他...</li>");
				//sb.AppendFormat("						<li><a href=\"http://car.bitauto.com/{0}/\" target=\"_blank\">返回{1}车型页&gt;&gt;</a></li>", cse.Cs_AllSpell, strCs_ShowName);
				//sb.Append("					</ul>");
				//sb.Append("				</div>");
				//sb.Append("				<div class=\"clear\">");
				//sb.Append("				</div>");
				//sb.Append("				<div class=\"more\" style=\"display: none;\">");
				//sb.Append("					<a href=\"javascript:\" id=\"carExplanation\" onclick=\"document.getElementById('carExplanationPopup').style.display='block';\">");
				//sb.Append("					</a>");
				//sb.Append("				</div>");
				//sb.Append("				<div class=\"popup-explanation\" id=\"carExplanationPopup\" style=\"display: none\">");
				//sb.Append("					<h6>");
				//sb.Append("						<span></span><a href=\"javascript:\" class=\"ico-close\" onclick=\"document.getElementById('carExplanationPopup').style.display='none';\">");
				//sb.Append("							关闭</a>");
				//sb.Append("					</h6>");
				//sb.Append("					<div class=\"popup-explanation-arrow\">");
				//sb.Append("					</div>");
				//sb.Append("					<p>");
				//sb.Append("					</p>");
				//sb.Append("				</div>");
				//sb.Append("			</div>");
				sb.Append("<div class=\"no-txt-box no-txt-m\">");
				sb.Append("");
				sb.Append("<p class=\"tit\">很抱歉，该车型暂无图片！</p>");
				sb.Append("<p>我们正在努力更新，请查看其他...</p>");
				sb.AppendFormat("<p><a href=\"http://car.bitauto.com/{0}/\" target=\"_blank\">返回{1}频道&gt;&gt;</a></p>",
					cse.Cs_AllSpell, strCs_ShowName);
				sb.Append("");
				sb.Append("</div>");
				SerialPhotoHtml = sb.ToString();
			}
		}

		// 取子品牌图片对比
		private string GetSerialHotCompareCars()
		{
			StringBuilder sb = new StringBuilder();
			List<EnumCollection.SerialHotCompareData> lshcd = base.GetSerialHotCompareByCsID(serialId, 5);
			if (lshcd.Count > 0)
			{
				sb.Append("<div class=\"line-box\" id=\"\">");
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
			return sb.ToString();
		}

		private void GetParams()
		{
			// modified by chengl Feb.22.2010
			if (this.Request.QueryString["CSID"] != null && this.Request.QueryString["CSID"].ToString() != "")
			{
				string tempCsID = this.Request.QueryString["CSID"].ToString();
				if (int.TryParse(tempCsID, out serialId))
				{ }
			}
			//if (null != Request.QueryString["CSID"])
			//{
			//    if (!string.IsNullOrEmpty(Request.QueryString["CSID"]))
			//    {
			//        CSID = Convert.ToInt32(Request.QueryString["CSID"]);
			//    }
			//}
		}

		/// <summary>
		/// 取得当前子品牌图片列表页面显示HTML
		/// </summary>
		/// <param name="nCPID"></param>
		/// <returns></returns>
		private Dictionary<string, string> GetRenderedCSPhotoListHTMLByCSID(int nCSID)
		{
			string cacheKey = "serial-photo-list-" + nCSID;

			object csPhotoListHTML = null;
			CacheManager.GetCachedData(cacheKey, out csPhotoListHTML);
			if (null == csPhotoListHTML)
			{
				csPhotoListHTML = RenderCSPhotoList(nCSID);
				CacheManager.InsertCache(cacheKey, csPhotoListHTML, WebConfig.CachedDuration);
			}

			return (Dictionary<string, string>)csPhotoListHTML;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, string> RenderCSPhotoList(int nCSID)
		{
			//页面显示元素标记集合
			Dictionary<string, string> dicCSPhotoPageInfo = new Dictionary<string, string>();

			//图片分类集合
			Dictionary<int, PhotoCategory> dicPhotoCategory = new Dictionary<int, PhotoCategory>();

			//load XML
			XmlDocument xmlDoc = new XmlDocument();
			#region 测试组
			// modified by chengl Dec.27.2009
			try
			{
				xmlDoc.LoadXml(GetCSPhotoListInfoFromXML(nCSID));
			}
			catch
			{ return dicCSPhotoPageInfo; }
			#endregion
			//子品牌名
			//XmlNode xnCSName = xmlDoc.SelectSingleNode("/D/E/N");
			//string strCsName = (null != xnCSName) ? xnCSName.InnerText : string.Empty;
			//dicCSPhotoPageInfo.Add(DIC_KEY_CS_NAME, strCsName);

			StringBuilder sbCbList = new StringBuilder();

			//子品牌下车型节点
			//<B>：此子品牌下车型节点
			//--- <N>：此车型图片张数。
			//--- <C>：此车型ID
			//--- <M>：此车型名
			//--- <E>：此车型排量。
			Dictionary<string, DataTable> dicPhotoType = new Dictionary<string, DataTable>();
			List<string> lisType = new List<string>();

			XmlNodeList xnlCbList = xmlDoc.SelectNodes("/D/B");
			if (null != xnlCbList && xnlCbList.Count > 0)
			{
				//聚合
				foreach (XmlElement xnCbList in xnlCbList)
				{
					if (xnCbList.ChildNodes.Count == 4)
					{
						string strE = xnCbList["E"].InnerText;
						if (!string.IsNullOrEmpty(strE))
						{
							if (!lisType.Contains(strE))
							{
								lisType.Add(strE);
							}
						}
					}
				}
				lisType.Sort();
				foreach (string str in lisType)
				{
					DataTable dtPhotoType = new DataTable(str);
					dtPhotoType.Columns.Add(new DataColumn("id", typeof(int)));
					dtPhotoType.Columns.Add(new DataColumn("count", typeof(int)));
					dtPhotoType.Columns.Add(new DataColumn("name", typeof(string)));

					foreach (XmlElement xnCbList in xnlCbList)
					{
						if (xnCbList.ChildNodes.Count == 4)
						{
							string strN = xnCbList["N"].InnerText;
							string strC = xnCbList["C"].InnerText;
							string strM = xnCbList["M"].InnerText;
							string strE = xnCbList["E"].InnerText;

							if (!string.IsNullOrEmpty(strE))
							{
								if (str == strE)
								{
									DataRow dr = dtPhotoType.NewRow();

									dr["id"] = strC;
									dr["count"] = string.IsNullOrEmpty(strN) ? 0 : Convert.ToInt32(strN);
									dr["name"] = strM;

									dtPhotoType.Rows.Add(dr);
								}
							}
						}
					}
					dicPhotoType.Add(str, dtPhotoType);
				}

				if (dicPhotoType.Count > 0)
				{
					//build xml
					// sbCbList.AppendLine("<ul class=\"pailiang\">");

					// 子品牌下车型年款
					DataSet dsCars = base.GetCarReferPriceByCsID(serialId, true);

					foreach (DataTable dt in dicPhotoType.Values)
					{
						sbCbList.AppendFormat("<dl><dt>{0}</dt><dd>", dt.TableName == "L" ? "未知" : dt.TableName);

						int nTCount = 0;
						foreach (DataRow dr in dt.Rows)
						{
							if (dsCars != null && dsCars.Tables.Count > 0 && dsCars.Tables[0].Rows.Count > 0)
							{
								DataRow[] drs = dsCars.Tables[0].Select(" car_id ='" + dr["id"].ToString() + "' ");
								if (drs != null && drs.Length > 0)
								{
									string carYear = "";
									if (drs[0]["Car_YearType"].ToString() != "")
									{ carYear = drs[0]["Car_YearType"].ToString().Trim() + "款 "; }
									if (nTCount > 0)
									{ sbCbList.Append("<span>|</span>"); }
									// sbCbList.AppendFormat("<span {0}><a target=\"_blank\" href=\"http://photo.bitauto.com/model/{1}\">{2}<strong>({3}张)</strong></a></span>", strCss, dr["id"].ToString(), dr["name"].ToString(), dr["count"].ToString());
									sbCbList.AppendFormat("<a href=\"http://photo.bitauto.com/model/{0}/\" target=\"_blank\">{1}<strong>({2}张)</strong></a>", dr["id"].ToString(), carYear + dr["name"].ToString(), dr["count"].ToString());
									nTCount++;
								}
							}
						}
						sbCbList.AppendLine("</dd></dl>");
					}

					// sbCbList.AppendLine("</ul>");
				}
			}
			dicCSPhotoPageInfo.Add(DIC_KEY_CS_PHOTOTYPELIST, sbCbList.ToString());

			sbCbList.Length = 0;

			//子品牌下车型图片分类列表
			//<A>：分类节点：
			//--- <N>：此分类的图片张数。
			//--- <G>：此分类的分类ID。
			//--- <D>：此分类的分类名。
			XmlNodeList xnlCategoryList = xmlDoc.SelectNodes("/D/A");
			if (null != xnlCategoryList)
			{
				int nCount = 0;
				//int nSplit = Convert.ToInt32(xnlCategoryList.Count / 2);
				//int nStar = (xnlCategoryList.Count % 2 == 0) ? nSplit + 1 : nSplit + 2;
				//int nEnd = (xnlCategoryList.Count % 2 == 0) ? nSplit : nSplit + 1;

				foreach (XmlElement xnCbList in xnlCategoryList)
				{
					if (xnCbList.ChildNodes.Count == 3)
					{
						string strN = xnCbList["N"].InnerText;
						string strG = xnCbList["G"].InnerText;
						string strD = GetImageCategoryText(strG);
						if (strD.Length == 0)
							strD = xnCbList["D"].InnerText;
						if (strD == "颜色")
						{ continue; }
						nCount++;

						//if (nCount == 1 || nCount == nStar)
						//    sbCbList.AppendLine("<ul class=\"half\">");

						//汇总图片总数
						int nPCount = string.IsNullOrEmpty(strN) == false ? int.Parse(strN) : 0;
						nPhotoCount += nPCount;

						sbCbList.AppendLine(string.Format("<a target=\"_blank\" href=\"http://photo.bitauto.com/serialmore/{0}/{1}/1/\">{2}<strong>({3})</strong></a>", serialId, strG, strD, nPCount));

						//保存图片分类供图片列表显示调用
						dicPhotoCategory.Add(int.Parse(strG), new PhotoCategory(int.Parse(strG), strD, string.Format("http://photo.bitauto.com/serialmore/{0}/{1}/1/", serialId, strG), nPCount));

						//if (nCount == nEnd || nCount == xnlCategoryList.Count)
						//    sbCbList.AppendLine("</ul>");
					}
				}
			}
			dicCSPhotoPageInfo.Add(DIC_KEY_CS_PHOTOCATEGORY, sbCbList.ToString());
			dicCSPhotoPageInfo.Add(DIC_KEY_CS_PHOTOCOUNT, nPhotoCount.ToString());

			sbCbList.Length = 0;

			//子品牌下车型图片列表
			//<C>：图片节点：
			//--- <I>：图片ID。（通过此获得图片发布URL）
			//--- <U>：图片文件路径。（通过此获得图片发布URL）
			//--- <C>：车型ID。（可忽略）
			//--- <D>：图片说明。
			//--- <P>：图片的分类ID。（通过此聚合分类下图片）
			XmlNodeList xnlPhotoList = xmlDoc.SelectNodes("/D/C");
			if (null != xnlPhotoList)
			{
				int lastUl = 1;
				foreach (PhotoCategory pc in dicPhotoCategory.Values)
				{
					if (pc.Count > 0)
					{
						//if (pc.Name == "颜色")
						//{
						//    continue;
						//}
						sbCbList.AppendLine("<div class=\"thepic_list_part\">");
						sbCbList.AppendLine(string.Format("<h5><a target=\"_blank\" href=\"{0}\">{1}</a></h5>", pc.Link, pc.Name));
						//if (lastUl == dicPhotoCategory.Count)
						//{
						//    sbCbList.AppendLine("<ul class=\"thepic_list thepic_list_oneline bgNo\">");
						//}
						//else
						//{
						//    sbCbList.AppendLine("<ul class=\"thepic_list thepic_list_oneline\">");
						//}
						sbCbList.AppendLine("<ul class=\"thepic_list thepic_list_oneline\">");
						lastUl++;

						int nNum = 0;
						foreach (XmlElement xnCbList in xnlPhotoList)
						{
							if (xnCbList.ChildNodes.Count >= 4)
							{
								string strI = xnCbList["I"].InnerText;//
								string strU = xnCbList["U"].InnerText;//
								string strD = xnCbList["D"].InnerText;//
								string strP = xnCbList["P"].InnerText;//

								//string strC = xnCbList["C"].InnerText;//

								if (int.Parse(strP) == pc.ID)
								{
									nNum++;
									if (nNum > 4)
										break;
									if (!string.IsNullOrEmpty(strU))
									{
										//string[] imgNames = strU.Split('.');
										//string img = string.Format("{0}_{1}_1.{2}", imgNames[0], strI, imgNames[1]);
										int imgId = ConvertHelper.GetInteger(strI);
										string img = CommonFunction.GetPublishHashImgUrl(1, strU, imgId);

										sbCbList.AppendLine("<li>");
										sbCbList.AppendLine(string.Format("<div><a target=\"_blank\" href=\"http://photo.bitauto.com/picture/{0}/{1}/\"><img src=\"{2}\" alt=\"{3}{4}\" /></a></div>", serialId, strI, img, strCs_ShowName, strD));
										sbCbList.AppendLine(string.Format("<p><a target=\"_blank\" href=\"http://photo.bitauto.com/picture/{0}/{1}/\">{2}</a></p>", serialId, strI, strD));
										sbCbList.AppendLine("</li>");
									}
								}
							}
						}

						sbCbList.AppendLine("</ul>");
						sbCbList.AppendLine(string.Format("<div class=\"morep\"><a target=\"_blank\" href=\"{0}\">更多照片&gt;&gt;</a></div>", pc.Link));
						sbCbList.AppendLine("</div>");
					}
				}
			}

			dicCSPhotoPageInfo.Add(DIC_KEY_CS_PHOTOLIST, sbCbList.ToString());

			return dicCSPhotoPageInfo;
		}

		/// <summary>
		/// 子品牌还关注
		/// </summary>
		/// <returns></returns>
		private void MakeSerialToSerialHtml()
		{
            serialToSeeJson = serialBll.GetSerialSeeToSeeJson(serialId, 8);
        }

		/// <summary>
		/// 获取图片分类的显示文本
		/// </summary>
		/// <param name="gText"></param>
		/// <returns></returns>
		private string GetImageCategoryText(string gText)
		{
			string dText = "";
			switch (gText)
			{
				case "6":
					dText = "外观";
					break;
				case "7":
					dText = "内饰";
					break;
				case "8":
					dText = "空间";
					break;
				case "12":
					dText = "图解";
					break;
			}
			return dText;
		}

		/// <summary>
		/// 获取XML
		/// </summary>
		/// <returns></returns>
		public static string GetCSPhotoListInfoFromXML(int nCSID)
		{
			string strCar_Serial_PhotoInfo_XML = string.Empty;

			//string cacheKey = "Car_Serial_PhotoInfo_XML_" + nCSID;	//此缓存已经不再使用	

			WebClient wc = new WebClient();
			wc.Encoding = Encoding.UTF8;
			//图库接口本地化更改 by sk 2012.12.21
			string xmlUrl = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialPhotoListPath, nCSID));
			//string xmlUrl = string.Format(WebConfig.PhotoService, nCSID.ToString());

			try
			{
				strCar_Serial_PhotoInfo_XML = wc.DownloadString(xmlUrl);
			}
			catch
			{ }

			if (!string.IsNullOrEmpty(strCar_Serial_PhotoInfo_XML))
			{
				strCar_Serial_PhotoInfo_XML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n" + strCar_Serial_PhotoInfo_XML;
			}

			return strCar_Serial_PhotoInfo_XML;
		}
		///// <summary>
		///// 得到品牌下的其他子品牌
		///// </summary>
		///// <returns></returns>
		//protected string GetBrandOtherSerial()
		//{
		//    return new Car_SerialBll().GetBrandOtherSerialList(cse);
		//}

		/// <summary>
		/// 得到品牌下的其他子品牌
		/// </summary>
		/// <returns></returns>
		protected string GetBrandOtherSerial()
		{
			List<CarSerialPhotoEntity> carSerialPhotoList = new Car_BrandBll().GetCarSerialPhotoListByCBID(cse.Cb_Id, false);

			carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

			if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
			{
				return "";
			}

			int forLastCount = 0;
			foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
			{
				if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Cs_Id)
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
				if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Cs_Id)
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
					cse.Cb_AllSpell, cse.Cb_Name);
				brandOtherSerial.Append("</div>");

				brandOtherSerial.Append("<ul class=\"text-list\">");

				brandOtherSerial.Append(contentBuilder.ToString());

				brandOtherSerial.Append("</ul>");
			}

			return brandOtherSerial.ToString();
		}

		/// <summary>
		/// 取子品牌相关用户
		/// </summary>
		private void GetUserBlockByCarSerialId()
		{
			StringBuilder sbUserBlock = new StringBuilder();
			// 计划购买
			DataTable dtWant = base.GetUserByCarSerialId(sic.CsID, 2, 3);
			if (dtWant != null && dtWant.Rows.Count > 0)
			{
				sbUserBlock.AppendLine("<div class=\"line_box zh_driver\">");
				sbUserBlock.AppendLine("<h3><span>和想买这款车的人聊聊</span></h3>");
				sbUserBlock.AppendLine("<div class=\"index_friend_r_l\">");
				sbUserBlock.AppendLine("<ul>");
				for (int i = 0; i < dtWant.Rows.Count; i++)
				{
					sbUserBlock.AppendLine("<li><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtWant.Rows[i]["userId"].ToString() + "/\" title=\"\">");
					sbUserBlock.AppendLine("<img height=\"60\" width=\"60\" src=\"" + dtWant.Rows[i]["userAvatar"].ToString() + "\" alt=\"\"></a>");
					sbUserBlock.AppendLine("<strong><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtWant.Rows[i]["userId"].ToString() + "/\">" + dtWant.Rows[i]["username"].ToString() + "</a></strong>");
					sbUserBlock.AppendLine("<p><a class=\"add_friend\" href=\"#\" onclick=\"javascript:AjaxAddFriend.show(" + dtWant.Rows[i]["userId"].ToString() + ", " + sic.CsID.ToString() + ");return false;\">加为好友</a></p></li>");
				}
				sbUserBlock.AppendLine("</ul>");
				sbUserBlock.AppendLine("</div><div class=\"clear\"> </div>");
				sbUserBlock.AppendLine("<div class=\"more\"><a target=\"_blank\" href=\"http://i.bitauto.com/FriendMore_c0_s" + sic.CsID.ToString() + "_p1_sort1_r010.html\">更多&gt;&gt;</a></div>");
				sbUserBlock.AppendLine("</div>");
			}
			// 车主
			DataTable dtOwner = base.GetUserByCarSerialId(sic.CsID, 3, 3);
			if (dtOwner != null && dtOwner.Rows.Count > 0)
			{
				sbUserBlock.AppendLine("<div class=\"line_box zh_driver\">");
				sbUserBlock.AppendLine("<h3><span>和车主聊聊</span></h3>");
				sbUserBlock.AppendLine("<div class=\"index_friend_r_l\">");
				sbUserBlock.AppendLine("<ul>");
				for (int i = 0; i < dtOwner.Rows.Count; i++)
				{
					sbUserBlock.AppendLine("<li><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtOwner.Rows[i]["userId"].ToString() + "/\" title=\"\">");
					sbUserBlock.AppendLine("<img height=\"60\" width=\"60\" src=\"" + dtOwner.Rows[i]["userAvatar"].ToString() + "\" alt=\"\"></a>");
					sbUserBlock.AppendLine("<strong><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtOwner.Rows[i]["userId"].ToString() + "/\">" + dtOwner.Rows[i]["username"].ToString() + "</a></strong>");
					sbUserBlock.AppendLine("<p><a class=\"add_friend\" href=\"#\" onclick=\"AjaxAddFriend.show(" + dtOwner.Rows[i]["userId"].ToString() + ", " + sic.CsID.ToString() + ",3);return false;\">加为好友</a></p></li>");
				}
				sbUserBlock.AppendLine("</ul>");
				sbUserBlock.AppendLine("</div><div class=\"clear\"> </div>");
				sbUserBlock.AppendLine("<div class=\"more\"><a target=\"_blank\" href=\"http://i.bitauto.com/FriendMore_c0_s" + sic.CsID.ToString() + "_p1_sort1_r001.html\">更多&gt;&gt;</a></div>");
				sbUserBlock.AppendLine("</div>");
			}
			UserBlock = sbUserBlock.ToString();
		}

		//[Obsolete("新闻服务上线后，将由InitNextSeeNew方法代替。")]
		//private void InitNextSee()
		//{
		//    nextSeePingceHtml = String.Empty;
		//    nextSeeXinwenHtml = String.Empty;
		//    nextSeeDaogouHtml = String.Empty;
		//    string serialSpell = sic.CsAllSpell.Trim().ToLower();
		//    DataSet newsDs = new Car_SerialBll().GetNewsListBySerialId(serialId, "pingce");
		//    if (newsDs != null && newsDs.Tables.Contains("listNews") && newsDs.Tables["listNews"].Rows.Count > 0)
		//        nextSeePingceHtml = "<li><a href=\"/" + serialSpell + "/pingce/\" target=\"_self\">" + strCs_ShowName + "<span>车型详解</span></a></li>";
		//    newsDs = new Car_SerialBll().GetNewsListBySerialId(serialId, "xinwen");
		//    if (newsDs != null && newsDs.Tables.Contains("listNews") && newsDs.Tables["listNews"].Rows.Count > 0)
		//        nextSeeXinwenHtml = "<li><a href=\"/" + serialSpell + "/xinwen/\" target=\"_self\">" + strCs_ShowName + "<span>新闻</span></a></li>";
		//    newsDs = new Car_SerialBll().GetNewsListBySerialId(serialId, "daogou");
		//    if (newsDs != null && newsDs.Tables.Contains("listNews") && newsDs.Tables["listNews"].Rows.Count > 0)
		//        nextSeeDaogouHtml = "<li><a href=\"/" + serialSpell + "/daogou/\" target=\"_self\">" + strCs_ShowName + "<span>导购</span></a></li>";
		//}
		private void InitNextSeeNew()
		{
			nextSeePingceHtml = String.Empty;
			nextSeeXinwenHtml = String.Empty;
			nextSeeDaogouHtml = String.Empty;
			string serialSpell = sic.CsAllSpell.Trim().ToLower();
			CarNewsBll newsBll = new CarNewsBll();
			if (newsBll.IsSerialNews(serialId, 0, CarNewsType.pingce))
				nextSeePingceHtml = "<li><a href=\"/" + serialSpell + "/pingce/\" target=\"_self\">" + strCs_ShowName + "车型详解</a></li>";

			//未使用-anh 20120326
			//if (newsBll.IsSerialNews(serialId, 0, CarNewsType.xinwen))
			//    nextSeeXinwenHtml = "<li><a href=\"/" + serialSpell + "/xinwen/\" target=\"_self\">" + strCs_ShowName + "<span>新闻</span></a></li>";

			if (newsBll.IsSerialNews(serialId, 0, CarNewsType.daogou))
				nextSeeDaogouHtml = "<li><a href=\"/" + serialSpell + "/daogou/\" target=\"_self\">" + strCs_ShowName + "导购</a></li>";
		}
	}

	class PhotoCategory
	{
		private int _id;
		private string _name;
		private string _link;
		private int _count;

		public PhotoCategory(int id, string name, string link, int count)
		{
			_id = id;
			_name = name;
			_link = link;
			_count = count;
		}

		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Link
		{
			get { return _link; }
			set { _link = value; }
		}

		public int Count
		{
			get { return _count; }
			set { _count = value; }
		}
	}

}