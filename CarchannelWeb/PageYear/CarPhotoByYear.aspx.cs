using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Net;
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

namespace BitAuto.CarChannel.CarchannelWeb.PageYear
{
	public partial class CarPhotoByYear : PageBase
	{
		protected string strCS_Name = string.Empty;
		protected string strCs_ShowName = string.Empty;
		protected string strCs_SeoName = string.Empty;
		protected string strCs_MasterName = string.Empty;
		protected int nPhotoCount = 0;
		protected string CsHeadHTML = string.Empty;
		protected int CSID = 0;
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

		protected string JsTagForYear = string.Empty;
		protected string PhotoProvideCateHTML = string.Empty;
		protected string SerialYearPhotoHtml = string.Empty;

		protected string nextSeePingceHtml;
		protected string nextSeeXinwenHtml;
		protected string nextSeeDaogouHtml;
		protected string UCarHtml;
		protected string _serialSpell = "";
		protected string serialToSeeHtml = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			if (!IsPostBack)
			{
				if (CSID > 0)
				{
					ShowPhotoBlock();
					#region 子品牌名片及基本数据
					string catchKeyCard = "CsSummaryCsCard_CsID" + CSID.ToString();
					object serialInfoCardByCsID = null;
					CacheManager.GetCachedData(catchKeyCard, out serialInfoCardByCsID);
					sic = new EnumCollection.SerialInfoCard();
					if (serialInfoCardByCsID == null)
					{
						sic = base.GetSerialInfoCardByCsID(CSID);
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

					// 广告
					base.MakeSerialTopADCode(sic.CsID);

					string catchKeyEntity = "CsSummaryEntity_CsID" + CSID.ToString();
					object serialInfoEntityByCsID = null;
					CacheManager.GetCachedData(catchKeyEntity, out serialInfoEntityByCsID);
					cse = new Car_SerialEntity();
					if (serialInfoEntityByCsID == null)
					{
						cse = (new Car_SerialBll()).Get_Car_SerialByCsID(CSID);
						CacheManager.InsertCache(catchKeyEntity, cse, 60);
					}
					else
					{
						cse = (Car_SerialEntity)serialInfoEntityByCsID;
					}
					#endregion

					InitNextSee();

					baaUrl = new Car_SerialBll().GetForumUrlBySerialId(CSID);
					strCS_Name = cse.Cs_Name;
					strCs_SeoName = cse.Cs_SeoName;
					strCs_ShowName = cse.Cs_ShowName;
					if (CSID == 1568)
						strCs_ShowName = "索纳塔八";
					strCs_MasterName = cse.Cb_Name.Trim();
					//图库内容  modified by sk 2013.03.21
					GetSerialYearPhotoHtml(CSID, CarYear);
					//bool isSuccess = false;
					//CsHeadHTML = this.GetRequestString(string.Format(WebConfig.HeadForSerial, CSID.ToString(), "CsPhoto"), 10, out isSuccess);
					CsHeadHTML = base.GetCommonNavigation("CsPhotoForYear", CSID).Replace("{0}", CarYear.ToString());
					JsTagForYear = "if(document.getElementById('carYearList_" + CarYear.ToString() + "')){document.getElementById('carYearList_" + CarYear.ToString() + "').className='current';}changeSerialYearTag(0," + CarYear.ToString() + ",'');";
					//还看过的
					//CsSerialToSerial = RenderSerialToSerial();
					//MakeSerialToSerialHtml();

					ucSerialToSee.SerialId = CSID;
					ucSerialToSee.SerialName = strCs_ShowName;

					CsHotCompareCars = this.GetSerialHotCompareCars();

					// modified by chengl Apr.27.2011
					// GetUserBlockByCarSerialId();
					// modified by sk 2013.03.21
					//PhotoProvideCateHTML = new Car_SerialBll().GetPhotoProvideCateHTML(CSID);

					//UCarHtml = new Car_SerialBll().GetUCarHtml(CSID);
				}
			}
		}
		private void GetSerialYearPhotoHtml(int serialId, int year)
		{
			SerialYearPhotoHtml = Car_SerialBll.GetSerialYearPhotoHtml(serialId, year);
			if (string.IsNullOrEmpty(SerialYearPhotoHtml))
			{
				//车型对应年款图片还没有，取子品牌图片
				SerialYearPhotoHtml = Car_SerialBll.GetSerialPhotoHtml(serialId);
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
				//sb.AppendFormat("						<li><a href=\"http://car.bitauto.com/{0}/\" target=\"_blank\">返回{1}车型页&gt;&gt;</a></li>", _serialSpell, strCs_ShowName);
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
					_serialSpell, strCs_ShowName);
				sb.Append("");
				sb.Append("</div>");
				sb.Append(SerialYearPhotoHtml);
				SerialYearPhotoHtml = sb.ToString();
			}
		}
		// 取子品牌图片对比
		private string GetSerialHotCompareCars()
		{
			StringBuilder sb = new StringBuilder();
			List<EnumCollection.SerialHotCompareData> lshcd = base.GetSerialHotCompareByCsID(CSID, 5);
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
			return sb.ToString();
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			int.TryParse(this.Request.QueryString["CSID"], out CSID);
			int.TryParse(this.Request.QueryString["year"], out _carYear);
			//CSID = CSID == 0 ? 2382 : CSID;
			//_carYear = _carYear == 0 ? 2008 : _carYear;
		}

		/// <summary>
		/// 子品牌还关注
		/// </summary>
		/// <returns></returns>
		private void MakeSerialToSerialHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(CSID, 6);
			if (lsts.Count > 0)
			{
				int loop = 0;
				foreach (EnumCollection.SerialToSerial sts in lsts)
				{
					string csName = sts.ToCsShowName.ToString();
					string shortName = StringHelper.SubString(csName, 12, true);
					if (shortName.StartsWith(csName))
						shortName = csName;

					loop++;
					htmlCode.Append("<li>");
					htmlCode.AppendFormat("<a target=\"_blank\" href=\"/{0}/\"><img src=\"{1}\" width=\"90\" height=\"60\"></a>",
						sts.ToCsAllSpell.ToString().ToLower(),
						 sts.ToCsPic.ToString());
					if (shortName != csName)
						htmlCode.AppendFormat("<p><a target=\"_blank\" href=\"/{0}/\" title=\"{1}\">{2}</a></p>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName, shortName);
					else
						htmlCode.AppendFormat("<p><a target=\"_blank\" href=\"/{0}/\">{1}</a></p>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName);
					htmlCode.AppendFormat("<p><span>{0}</span></p>", StringHelper.SubString(sts.ToCsPriceRange.ToString(), 14, false));
					htmlCode.AppendFormat("</li>");
				}
			}
			serialToSeeHtml = htmlCode.ToString();
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

			//string cacheKey = "Car_Serial_PhotoInfo_XML_" + nCSID; //此缓存不再使用

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
				string priceRang = base.GetSerialIntPriceRangeByID(entity.SerialId);
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


		#region by liurw
		/// <summary>
		/// 数据服务地址，参数1：子品牌ID，参数2：年款
		/// </summary>
		public string DataServiceUrl
		{
			get {
				//图库接口本地化更改 by sk 2012.12.21
				return System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialYearPath);
				//return ConfigurationManager.AppSettings["PhotoServiceForYear"];
			}
		}

		public string SerialPhotoServiceUrl
		{
			get { return ConfigurationManager.AppSettings["SerialPhoto12ImageInterface"]; }
		}

		private int _carYear;
		/// <summary>
		/// 当前年款
		/// </summary>
		public int CarYear
		{
			get { return _carYear; }
			set { _carYear = value; }
		}

		private bool _hasImage = true;
		/// <summary>
		/// 是否有图
		/// </summary>
		public bool HasImage
		{
			get { return _hasImage; }
		}

		private int _serialImageCount = 0;
		/// <summary>
		/// 子品牌图片数量
		/// </summary>
		public int SerialImageCount
		{
			get { return _serialImageCount; }
		}

		private void GetCarListBlock(XPathNodeIterator oilIterator, ref StringBuilder sb)
		{
			if (oilIterator.Count > 0 && sb != null)
			{
				XmlNamespaceManager ns = new XmlNamespaceManager(oilIterator.Current.NameTable);

				sb.Append("<h4>按车型</h4>");
				while (oilIterator.MoveNext())
				{
					sb.AppendFormat("<dl><dt>{0}</dt><dd>", oilIterator.Current.GetAttribute("Value", ns.DefaultNamespace));
					XPathNodeIterator carIterator = oilIterator.Current.Select("Car");
					while (carIterator.MoveNext())
					{
						int carId;
						int.TryParse(carIterator.Current.GetAttribute("CarId", ns.DefaultNamespace), out carId);

						sb.AppendFormat("<a href=\"{2}\" target=\"_blank\">{0}<strong>({1})</strong></a><span>|</span>"
							, carIterator.Current.GetAttribute("CarName", ns.DefaultNamespace)
							, carIterator.Current.GetAttribute("ImageCount", ns.DefaultNamespace)
							, carIterator.Current.GetAttribute("Link", ns.DefaultNamespace)
							);

					}

					if (sb.ToString().EndsWith("<span>|</span>"))
					{
						sb = sb.Remove(sb.Length - 14, 14);
					}
					sb.Append("</dd></dl><div class=\"clear\"></div>");
				}
			}
		}

		private void GetGroupListBlock(XPathNodeIterator groupIterator, ref StringBuilder sb)
		{
			if (groupIterator.Count > 0 && sb != null)
			{
				XmlNamespaceManager ns = new XmlNamespaceManager(groupIterator.Current.NameTable);
				sb.Append("<h4 class=\"dl\">按主题分类</h4><div class=\"c0603_02\">");

				while (groupIterator.MoveNext())
				{
					sb.AppendFormat("<a target=\"_blank\" href=\"{2}\">{0}<span>({1}张)</span></a>"
						, groupIterator.Current.GetAttribute("GroupName", ns.DefaultNamespace)
						, groupIterator.Current.GetAttribute("ImageCount", ns.DefaultNamespace)
						, groupIterator.Current.GetAttribute("Link", ns.DefaultNamespace));
				}
				sb.Append("</div>");
			}
		}

		/// <summary>
		/// 输出图片列表
		/// </summary>
		private void ShowPhotoBlock()
		{
			if (string.IsNullOrEmpty(this.DataServiceUrl))
			{
				return;
			}

			int yearCarId = 0;
			StringBuilder sb = new StringBuilder();
			XmlDocument xml = GetDataFromPhotoService(this.DataServiceUrl, "photo-serial-year-" + CSID + "_" + this.CarYear);
			XPathNavigator navigator = xml.CreateNavigator();
			XmlNamespaceManager ns = new XmlNamespaceManager(navigator.NameTable);
			string imageUrlBase = "http://img{0}.bitautoimg.com/autoalbum/";
			XPathNodeIterator serialIterator = navigator.Select("ImageData/Serial");
			while (serialIterator.MoveNext())
			{
				int.TryParse(serialIterator.Current.GetAttribute("YearCarId", ns.DefaultNamespace), out yearCarId);
			}
			XPathNodeIterator oilIterator = navigator.Select("ImageData/CarAccount/OilWear");

			GetCarListBlock(oilIterator, ref sb);

			XPathNodeIterator groupIterator = navigator.Select("ImageData/CarImageGroup/Group");

			StringBuilder sb1 = new StringBuilder();
			if (groupIterator.Count > 0)
			{
				//sb.Append("<h4 class=\"dl\">按主题分类</h4><div class=\"c0603_02\">");
				int indexGroupNode = 1;
				while (groupIterator.MoveNext())
				{
					//sb.AppendFormat("<a href=\"{2}\" target=\"_blank\">{0}<span>({1}张)</span></a>"
					//    , groupIterator.Current.GetAttribute("GroupName", ns.DefaultNamespace)
					//    , groupIterator.Current.GetAttribute("ImageCount", ns.DefaultNamespace)
					//    , groupIterator.Current.GetAttribute("Link", ns.DefaultNamespace));

					XPathNodeIterator imageIterator = groupIterator.Current.Select("ImageInfo");
					if (imageIterator.Count > 0)
					{
						if (indexGroupNode == groupIterator.Count)
						{
							// 最后1个分类
							sb1.AppendFormat("<div class=\"thepic_list_part\"><h5><a href=\"{1}\" target=\"_blank\">{0}</a></h5><ul class=\"thepic_list thepic_list_oneline bgNo\">"
								, groupIterator.Current.GetAttribute("GroupName", ns.DefaultNamespace)
							   , groupIterator.Current.GetAttribute("Link", ns.DefaultNamespace));
						}
						else
						{
							sb1.AppendFormat("<div class=\"thepic_list_part\"><h5><a href=\"{1}\" target=\"_blank\">{0}</a></h5><ul class=\"thepic_list thepic_list_oneline\">"
								, groupIterator.Current.GetAttribute("GroupName", ns.DefaultNamespace)
							   , groupIterator.Current.GetAttribute("Link", ns.DefaultNamespace));
						}
						indexGroupNode++;
						while (imageIterator.MoveNext())
						{
							int imgID = int.Parse(imageIterator.Current.GetAttribute("ImageId", ns.DefaultNamespace)) % 4 + 1;
							sb1.AppendFormat("<li><div><a href=\"{2}\" target=\"_blank\"><img alt=\"{0}\" src=\"{1}\"></a></div><p><a href=\"{2}\" target=\"_blank\">{0}</a></p></li>"
								, imageIterator.Current.GetAttribute("ImageName", ns.DefaultNamespace)
								, string.Format(string.Format(imageUrlBase, imgID.ToString()) + imageIterator.Current.GetAttribute("ImageUrl", ns.DefaultNamespace), 1)
								, imageIterator.Current.GetAttribute("Link", ns.DefaultNamespace)
								);
						}

						sb1.Append("</ul>");
						if (imageIterator.Count == 4)
						{
							sb1.AppendFormat("<div class=\"morep\"><a href=\"http://photo.bitauto.com/modelmore/{2}/{1}/1/\" target=\"_blank\">更多照片&gt;&gt;</a></div>", CSID, groupIterator.Current.GetAttribute("GroupId", ns.DefaultNamespace), yearCarId);
						}
						sb1.Append("</div>");
					}

				}
				//sb.Append("</div>");
			}

			if (sb1.ToString().Trim().Length == 0)
			{
				sb.Append("<div class=\"c0604_03\">该年款暂无图片，我们正在努力更新，请查看其他……</div>");
				this._hasImage = false;
				ShowSerialImageBlock();
			}
			//litImageList.Text = sb1.ToString();
			//litCarList.Text = sb.ToString();

			#region 子品牌颜色图片
			// 子品牌颜色图片
			StringBuilder _sbColor = new StringBuilder();
			Dictionary<string, XmlNode> dicColor = new Car_SerialBll().GetSerialColorPhotoByCsID(CSID, this.CarYear);
			if (dicColor != null && dicColor.Count > 0)
			{
				_sbColor.AppendLine("<div class=\"thepic_list_part_color thepic_list_oneline_color\">");
				_sbColor.AppendLine("<h5>车身颜色</h5>");
				_sbColor.AppendLine("<div class=\"carColor\">");
				_sbColor.AppendLine("<b id=\"LeftArr\" class=\"lGray\" >左</b>");
				_sbColor.AppendLine("<b id=\"RightArr\" class=\"r\" >右</b>");
				_sbColor.AppendLine("<div class=\"carColor_inner\">");
				_sbColor.AppendLine("<div id=\"innerBox\" style=\"top: 0pt; left: 0pt;\">");
				_sbColor.AppendLine("<ul id=\"colorBox\">");
				foreach (KeyValuePair<string, XmlNode> keyColor in dicColor)
				{
					if (keyColor.Value.Attributes["ImageUrl"] != null && keyColor.Value.Attributes["ImageUrl"].Value.Trim() != "" && keyColor.Value.Attributes["Link"] != null && keyColor.Value.Attributes["Link"].Value.Trim() != "")
					{
						_sbColor.AppendLine("<li>");
						_sbColor.AppendLine("<div>");
						_sbColor.AppendLine("<a target=\"_blank\" href=\"" + keyColor.Value.Attributes["Link"].Value.Trim() + "\">");
						_sbColor.AppendLine("<img src=\"" + keyColor.Value.Attributes["ImageUrl"].Value.Trim() + "\" alt=\"\" />");
						_sbColor.AppendLine("</a></div>");
						_sbColor.AppendLine("<a target=\"_blank\" href=\"" + keyColor.Value.Attributes["Link"].Value.Trim() + "\">" + keyColor.Key.Trim() + "</a>");
						_sbColor.AppendLine("</li>");
					}
				}
				_sbColor.AppendLine("</ul>");
				_sbColor.AppendLine("</div></div></div></div>");
				//_sbColor.AppendLine("<div class=\"line\"></div>");
				//_sbColor.AppendLine("<div class=\"clear\"></div>");
				ColorPicList = _sbColor.ToString();
			}
			#endregion

		}

		//显示无图时子品牌图片
		private void ShowSerialImageBlock()
		{
			//showgroup & showcar param to control whether to display the tow node in the result xml.
			XmlDocument xml = GetDataFromPhotoService(string.Format(this.SerialPhotoServiceUrl, this.CSID) + "&showgroup=true&showcar=true", "photo-serial-standard-" + this.CSID + "-12");

			StringBuilder sb = new StringBuilder();

			XPathNavigator navigator = xml.CreateNavigator();
			XmlNamespaceManager ns = new XmlNamespaceManager(navigator.NameTable);
			string imageUrlBase = "http://img{0}.bitautoimg.com/autoalbum/";

			XPathNodeIterator oilIterator = navigator.Select("ImageData/CarAccount/OilWear");
			// this.GetCarListBlock(oilIterator, ref sb);

			XPathNavigator serialGroupNav = navigator.SelectSingleNode("ImageData/SerialImageGroup");
			if (serialGroupNav != null)
			{
				int count;
				int.TryParse(serialGroupNav.GetAttribute("ImageCount", ns.DefaultNamespace), out count);
				this._serialImageCount = count;
			}

			XPathNodeIterator groupIterator = navigator.Select("ImageData/SerialImageGroup/Group");
			// this.GetGroupListBlock(groupIterator, ref sb);

			//litCarList1.Text = sb.ToString();

			sb = new StringBuilder();

			XPathNodeIterator imageIterator = navigator.Select("ImageData/ImageList/ImageInfo");
			while (imageIterator.MoveNext())
			{
				int imgID = int.Parse(imageIterator.Current.GetAttribute("ImageId", ns.DefaultNamespace)) % 4 + 1;
				sb.AppendFormat("<li><a target=\"_blank\" href=\"{2}\"><img src=\"{1}\" alt=\"{0}\"></a><a href=\"{2}\">{0}</a></li>"
					, imageIterator.Current.GetAttribute("ImageName", ns.DefaultNamespace)
					, string.Format(string.Format(imageUrlBase, imgID.ToString()) + imageIterator.Current.GetAttribute("ImageUrl", ns.DefaultNamespace), 1)
					, imageIterator.Current.GetAttribute("Link", ns.DefaultNamespace)
					);
			}

			//litImageList1.Text = sb.ToString();

		}


		//获取图片子品牌年款接口数据
		private XmlDocument GetDataFromPhotoService(string dataUri, string cachekey)
		{
			XmlDocument xml = new XmlDocument();
			XmlReader reader = null;
			try
			{
				reader = XmlReader.Create(string.Format(dataUri, CSID, CarYear));
				xml.Load(reader);

				if (HttpContext.Current != null)
				{
					HttpContext.Current.Cache.Insert(cachekey, xml, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
				}
			}
			catch
			{
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}
			return xml;

		}
		#endregion


		private void InitNextSee()
		{
			nextSeePingceHtml = String.Empty;
			nextSeeXinwenHtml = String.Empty;
			nextSeeDaogouHtml = String.Empty;
			string serialSpell = sic.CsAllSpell.Trim().ToLower();
			_serialSpell = serialSpell;
			string serialShowName = sic.CsShowName;
			CarNewsBll newsBll = new CarNewsBll();
			if (newsBll.IsSerialNews(CSID, 0, CarNewsType.pingce))
				nextSeePingceHtml = "<li><a href=\"/" + serialSpell + "/pingce/\" target=\"_self\">" + serialShowName + "车型详解</a></li>";
			if (newsBll.IsSerialNews(CSID, 0, CarNewsType.xinwen))
				nextSeeXinwenHtml = "<li><a href=\"/" + serialSpell + "/xinwen/\" target=\"_self\">" + serialShowName + "新闻</a></li>";
			if (newsBll.IsSerialNews(CSID, 0, CarNewsType.daogou))
				nextSeeDaogouHtml = "<li><a href=\"/" + serialSpell + "/daogou/\" target=\"_self\">" + serialShowName + "导购</a></li>";
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