using System;
using System.Xml;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCCB = BitAuto.CarChannel.BLL;
using BCCC = BitAuto.CarChannel.Common;
using BCCCache = BitAuto.CarChannel.Common.Cache;
using BCCM = BitAuto.CarChannel.Model;
using BU = BitAuto.Utils;


namespace BitAuto.CarChannel.CarchannelWeb
{
	public partial class CarshowMasterbrand : BitAuto.CarChannel.Common.PageBase
	{
		protected string masterSpell;
		protected string masterName;
		protected int masterId;
		protected int exhibitionID = 5;
		protected BCCM.Exhibition modelExhibition;
		protected BCCM.Pavilion modelPavilion;
		protected string pavlionUrl = "";
		protected string m_WomanModule = "";
		protected int m_WomanModuleCount = 0;
		protected string m_VideString = "";
		protected int m_Video = 0;
		protected string m_WomanModulemoreUrl = "";
		protected string m_VideoMoreUrl = "";

		/// <summary>
		/// 新闻列表
		/// </summary>
		public string NewString
		{
			get
			{
				return GetNewString();
			}
		}
		/// <summary>
		/// 封面图片
		/// </summary>
		public string CoverFigureString
		{
			get
			{
				return PageCoverFigure();
			}
		}
		/// <summary>
		/// 显示车型图片列表
		/// </summary>
		public string IsCarTypeString
		{
			get
			{
				return ShowCarSerial();
			}
		}
		/// <summary>
		/// 车模字符串
		/// </summary>
		public string WomanModuleString
		{
			get
			{
				return m_WomanModule;
			}
		}
		/// <summary>
		/// 其他厂家
		/// </summary>
		public string ShowCompany
		{
			get
			{
				return ShowOtherCompany();
			}
		}
		/// <summary>
		/// 视频
		/// </summary>
		public string Video
		{
			get
			{
				return m_VideString;
			}
		}
		/// <summary>
		/// 主品牌名称
		/// </summary>
		public string MasterName
		{
			get
			{
				return masterName;
			}
		}
		/// <summary>
		/// 展馆链接
		/// </summary>
		public string GetPavUrl
		{

			get
			{
				return GetPavilionUrl(modelPavilion.Name, modelPavilion.ID);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParameter();
			GetExhibition();
			m_WomanModule = ShowCarWomanModule();
			m_VideString = ShowVideo();
		}
		/// <summary>
		/// 得到展会的对象及展馆对象
		/// </summary>
		private void GetExhibition()
		{
			modelPavilion = BCCB.Exhibition.GetPavilionByMasterBrandID(masterId, exhibitionID, 5, out modelExhibition);
			if (modelExhibition == null || modelExhibition.ID < 1)
			{
				Response.Redirect("http://car.bitauto.com/car/404error.aspx?info=在展会中无此品牌信息！");
				return;
			}
			if (modelPavilion == null || modelPavilion.ID < 1)
			{
				Response.Redirect("http://car.bitauto.com/car/404error.aspx?info=在展会中无此品牌信息！");
			}
		}
		/// <summary>
		/// 得到参数
		/// </summary>
		private void GetParameter()
		{
			masterSpell = BU.ConvertHelper.GetString(Request.QueryString["spell"]).Trim().ToLower();
			if (!String.IsNullOrEmpty(masterSpell))
			{
				masterId = new BCCB.Car_BrandBll().GetMasterbrandBySpell(masterSpell, out masterName);
			}
			else
			{
				Response.Redirect("http://car.bitauto.com/car/404error.aspx?info=无此品牌信息！");
			}
		}
		/// <summary>
		/// 根据展馆名称和展馆ID得到展馆的URL
		/// </summary>
		/// <param name="pavName">展馆名称</param>
		/// <param name="pavID">展馆ID</param>
		private string GetPavilionUrl(string pavName, int pavID)
		{
			return "http://chezhan.bitauto.com/guangzhou-chezhan/zhanguan/gd_2009/" + modelPavilion.Name.Replace("馆", "");
		}
		/// <summary>
		/// 得到新的列表
		/// </summary>
		private string GetNewString()
		{
			StringBuilder htmlCode = new StringBuilder();
			List<XmlElement> newsList = new BCCB.Car_BrandBll().GetCarshowTopNews("masterbrand", masterId);
			if (newsList.Count > 0)
			{
				htmlCode.AppendLine("<div class=\"line_box\">");
				htmlCode.AppendLine("<div class=\"ka_topnews\">");
				XmlElement firstNews = newsList[0];
				string newsTitle = firstNews.SelectSingleNode("title").InnerText;
				newsTitle = BU.StringHelper.RemoveHtmlTag(newsTitle);
				string filePath = firstNews.SelectSingleNode("filepath").InnerText;
				htmlCode.AppendLine("<h2><a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a></h2>");
				htmlCode.AppendLine("<div class=\"demarcation\"></div>");
				for (int i = 1; i < newsList.Count; i++)
				{
					if (i == 1 || i == 5)
						htmlCode.AppendLine("<ul>");

					XmlElement newsNode = newsList[i];
					newsTitle = newsNode.SelectSingleNode("title").InnerText;
					newsTitle = BU.StringHelper.RemoveHtmlTag(newsTitle);
					filePath = newsNode.SelectSingleNode("filepath").InnerText;
					string newsDate = Convert.ToDateTime(newsNode.SelectSingleNode("publishtime").InnerText).ToString("MM-dd");
					htmlCode.AppendLine("<li><span><a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a><em>" + newsDate + "</em></li>");

					if (i == 4 || i == newsList.Count - 1)
						htmlCode.AppendLine("</ul>");
				}
				htmlCode.AppendLine("</div>");
				htmlCode.AppendLine("<div class=\"clear\"></div>");
				htmlCode.AppendLine("</div>");
			}

			return htmlCode.ToString();
		}
		/// <summary>
		/// 焦点图
		/// </summary>
		/// <returns></returns>
		private string PageCoverFigure()
		{

			int classID = 0;
			string imgUrl = new BCCB.Car_SerialBll().GetCarShowDefaultImage(masterId, "master", out classID);
			classID = new BCCB.Car_SerialBll().GetMasterBrandImageClassID(masterId);
			string imgUrlGo = "http://photo.bitauto.com/exhibit/class/" + classID + "/0";
			int ImageCount = 0;
			BCCB.BrandForum brandForum = new BitAuto.CarChannel.BLL.BrandForum();
			BCCB.Car_BrandBll brandBLL = new BitAuto.CarChannel.BLL.Car_BrandBll();
			ImageCount = new BCCB.Car_SerialBll().GetCarshowImageCount(masterId, "master");//品牌图片数量
			brandForum = brandBLL.GetBrandForm("masterBrand", masterId);
			string pavUrl = GetPavilionUrl(modelPavilion.Name, modelPavilion.ID);
			StringBuilder strBuilder = new StringBuilder("<div class=\"line_box\"><div class=\"ka_focus\">");

			strBuilder.AppendFormat("<div class=\"photo\"><a href=\"{1}\" target=\"_blank\"><img src=\"{0}\" width=\"300\" height=\"200\" /></a></div>", imgUrl, imgUrlGo);
			strBuilder.Append("<div class=\"text\">");
			//添加厂家及展馆说明
			strBuilder.Append("<ul class=\"one\">");
			strBuilder.AppendFormat("<li><strong>厂家：</strong><a target=\"_blank\" >{0}</a>"
								   + "<a href=\"{1}\" target=\"_blank\" class=\"hui\">进入论坛&gt;&gt;</a></li>"
								   , masterName
								   , brandForum.CampForumUrl);
			strBuilder.AppendFormat("<li class=\"r\"><strong>展馆：</strong><a href=\"{0}\" target=\"_blank\">{1}</a>"
								   + "<a href=\"{0}\" target=\"_blank\" class=\"hui\">进入&gt;&gt;</a></li>"
								   , pavUrl
								   , modelPavilion.Name);
			strBuilder.Append("</ul>");
			//添加图片及车模和视频的数量
			strBuilder.Append("<ul class=\"two\">");
			strBuilder.AppendFormat("<li><a href=\"{1}\" target=\"_blank\">图片</a>({0}张)</li>", ImageCount, imgUrlGo);
			strBuilder.AppendFormat("<li><a href=\"{1}\" target=\"_blank\">车模</a>({0}张)</li>", m_WomanModuleCount, m_WomanModulemoreUrl);
			strBuilder.AppendFormat("<li><a href=\"{1}\" target=\"_blank\">视频</a>({0}张)</li>", m_Video, m_VideoMoreUrl);
			strBuilder.Append("</ul>");
			strBuilder.Append("</div></div></div>");

			return strBuilder.ToString();
		}
		/// <summary>
		/// 显示子品牌
		/// </summary>
		/// <returns>列表字符串</returns>
		private string ShowCarSerial()
		{

			#region OldCode
			/*
        if (modelPavilion == null
			|| modelPavilion.MasterBrandList.Count < 1
			|| !modelPavilion.MasterBrandList.ContainsKey(masterId))
		{
			return "";
		}
		StringBuilder serialBuilder = new StringBuilder("<div class=\"line_box\"><h3><span><a >"
									+ masterName + "车型</a></span></h3><div class=\"lh_atlaspiclist\"><ul class=\"lh_atlas \">");

		string serialUrl = "";
		string serialName = "";
		string serialImageUrl = "";
        int classID = 0;
		string masterAllSpell = new BCCB.Car_BrandBll().GetCarMasterBrandInfoByBSID(masterId)["urlspell"].ToString();
		BCCM.Car_SerialEntity modelSerail;
		foreach (int serialID in modelPavilion.MasterBrandList[masterId])
		{
			modelSerail = new BCCB.Car_SerialBll().Get_Car_SerialByCsID(serialID);
			if (modelSerail == null)
			{
				continue;
			}
			serialUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/2009/" + masterAllSpell.ToLower() + "/" + modelSerail.Cs_AllSpell.ToLower() + "/";
			serialName = modelSerail.Cs_ShowName;
            serialImageUrl = new BCCB.Car_SerialBll().GetCarShowDefaultImage(serialID, "serial", 1, out classID);
			serialBuilder.AppendFormat("<li><a target=\"_blank\" href=\"{2}\"><img height=\"100\" width=\"150\" alt=\"{0}\" src=\"{1}\"/></a><p><a target=\"_blank\" href=\"{2}\">{0}</a></p></li>"
									  , serialName
									  , serialImageUrl
									  , serialUrl);
		}
		serialBuilder.Append("</ul></div><div class=\"clear\"></div><div class=\"more\"></div></div>");
		return serialBuilder.ToString();
         */

			#endregion

			if (modelPavilion == null
				|| modelPavilion.MasterBrandList.Count < 1
				|| !modelPavilion.MasterBrandList.ContainsKey(masterId))
			{
				return "";
			}
			StringBuilder serialBuilder = new StringBuilder("<div class=\"line_box\"><h3><span><a >"
										+ masterName + "车型</a></span></h3><div class=\"lh_atlaspiclist\"><ul class=\"lh_atlas \">");
			string serialUrl = "";
			string serialName = "";
			string serialImageUrl = "";
			int classID = 0;
			string masterAllSpell = new BCCB.Car_BrandBll().GetCarMasterBrandInfoByBSID(masterId)["urlspell"].ToString();
			BCCM.Car_SerialBaseEntity modelSerialEntity;
			string[] dirctionOrder = { "首发新车", "上市新车", "概念车", "即将上市", "无" };
			Dictionary<string, List<string>> serialList = new Dictionary<string, List<string>>();
			serialList.Add("无", new List<string>());
			bool isExits = false;

			foreach (int serialID in modelPavilion.MasterBrandList[masterId])
			{
				modelSerialEntity = new BCCB.Car_SerialBll().GetSerialBaseEntity(serialID);
				if (modelSerialEntity == null)
				{
					continue;
				}
				serialUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/2009/"
					+ masterAllSpell.ToLower()
					+ "/"
					+ modelSerialEntity.SerialNameSpell.ToLower() + "/";
				serialName = modelSerialEntity.SerialShowName;
				serialImageUrl = new BCCB.Car_SerialBll().GetCarShowDefaultImage(serialID, "serial", 1, out classID);
				string serialString = "";
				//归类子品牌数据到相关的属性
				foreach (KeyValuePair<int, BCCM.Attribute> attrKey in modelExhibition.AttributeList)
				{
					if (attrKey.Value.SerialIDList != null
						&& attrKey.Value.SerialIDList.Count > 1
						&& attrKey.Value.SerialIDList.ContainsKey(serialID))
					{
						serialString = "<li><a target=\"_blank\" href=\"" + serialUrl + "\"><img height=\"100\" width=\"150\" alt=\""
											  + serialName + "\" src=\"" + serialImageUrl + "\"/></a>"
											  + "<p><a target=\"_blank\" href=\"" + serialUrl + "\">" + serialName + "</a>" + GetSerialProvString(serialID)
											  + "</p><p>价格：" + GetSerialPriceRange(base.GetSerialPriceRangeByID(serialID)) + "</p></li>";

						if (!serialList.ContainsKey(attrKey.Value.Name))
						{
							serialList.Add(attrKey.Value.Name, new List<string>());
							serialList[attrKey.Value.Name].Add(serialString);
							isExits = true;
							break;
						}
						serialList[attrKey.Value.Name].Add(serialString);
						isExits = true;
						break;
					}

				}

				serialString = "<li><a target=\"_blank\" href=\"" + serialUrl + "\"><img height=\"100\" width=\"150\" alt=\""
											   + serialName + "\" src=\"" + serialImageUrl + "\"/></a>"
											   + "<p><a target=\"_blank\" href=\"" + serialUrl + "\">" + serialName + "</a>" + GetSerialProvString(serialID)
											   + "</p><p>价格：" + GetSerialPriceRange(base.GetSerialPriceRangeByID(serialID)) + "</p></li>";

				if (!isExits)
				{
					serialList["无"].Add(serialString);
				}
				isExits = false;
				//serialBuilder.AppendFormat("<li><a target=\"_blank\" href=\"{2}\"><img height=\"100\" width=\"150\" alt=\"{0}\" src=\"{1}\"/></a>"
				//                            + "<p><a target=\"_blank\" href=\"{2}\">{0}</a>{4}</p><p>价格：{3}</p></li>"
				//                          , serialName
				//                          , serialImageUrl
				//                          , serialUrl
				//                          , GetSerialPriceRange(base.GetSerialPriceRangeByID(serialID))
				//                          , GetSerialProvString(serialID));
			}

			foreach (string str in dirctionOrder)
			{
				if (!serialList.ContainsKey(str)
					|| serialList[str] == null
					|| serialList[str].Count < 1)
				{
					continue;
				}
				foreach (string tempstr in serialList[str])
				{
					serialBuilder.Append(tempstr);
				}
			}

			serialBuilder.Append("</ul></div><div class=\"clear\"></div><div class=\"more\"></div></div>");
			return serialBuilder.ToString();
		}
		/// <summary>
		/// 子品牌报价区间
		/// </summary>
		/// <param name="Source">原字符串</param>
		/// <returns></returns>
		protected string GetSerialPriceRange(string Source)
		{
			if (!String.IsNullOrEmpty(Source))
			{
				return Source;
			}
			return "暂无报价";
		}
		/// <summary>
		/// 通过子品牌ID得到子品牌属性
		/// </summary>
		/// <param name="serialID">子品牌ID</param>
		/// <returns>属性字符串</returns>
		protected string GetSerialProvString(int serialID)
		{
			List<BCCM.Attribute> attributeList = new List<BitAuto.CarChannel.Model.Attribute>();
			attributeList = BCCB.Exhibition.GetAttributeListBySerialID(serialID, exhibitionID, 5);
			if (attributeList == null || attributeList.Count < 1)
			{
				return "";
			}

			BCCM.Attribute attributeObject = new BitAuto.CarChannel.Model.Attribute();
			attributeObject = attributeList[0];

			if (attributeObject.Name.Trim() == "首发新车")
			{
				return "<span class=\"sf\"><a href=\"http://chezhan.bitauto.com/guangzhou-chezhan/xinchefabu/\">首发</a></span>";
			}
			if (attributeObject.Name.Trim() == "上市新车")
			{
				return "<span class=\"sf\"><a href=\"http://chezhan.bitauto.com/guangzhou-chezhan/xinchefabu/index2.shtml\">上市</a></span>";
			}
			if (attributeObject.Name.Trim() == "概念车")
			{
				return "<span class=\"sf\"><a href=\"http://chezhan.bitauto.com/guangzhou-chezhan/xinchefabu/index4.shtml\">概念车</a></span>";
			}
			if (attributeObject.Name.Trim() == "即将上市")
			{
				return "<span class=\"sf\"><a href=\"http://chezhan.bitauto.com/guangzhou-chezhan/xinchefabu/index3.shtml\">即将</a></span>";
			}
			return "";
		}
		/// <summary>
		/// 展出车模
		/// </summary>
		/// <returns>展出车模字符串</returns>
		private string ShowCarWomanModule()
		{

			int classId = 0;	//主品牌分类ID
			List<XmlElement> imgList = new BCCB.Car_SerialBll().GetCarshowMasterModelImages(masterId, out classId, out m_WomanModuleCount);
			if (imgList == null || imgList.Count < 1)
			{
				return "";
			}
			StringBuilder htmlCode = new StringBuilder();
			m_WomanModulemoreUrl = "http://photo.bitauto.com/exhibit/class/" + classId;
			htmlCode.AppendLine("<div class=\"line_box p_mo1\">");
			htmlCode.AppendLine("<h3><span><a href=\"" + m_WomanModulemoreUrl + "\" target=\"_blank\">" + masterName + "车模</a></span></h3>");
			htmlCode.AppendLine("<div class=\"p_piclist1\">");
			htmlCode.AppendLine("<ul>");
			foreach (XmlElement imgNode in imgList)
			{
				int imgId = BU.ConvertHelper.GetInteger(imgNode.SelectSingleNode("SiteImageId").InnerText);
				int albumId = BU.ConvertHelper.GetInteger(imgNode.SelectSingleNode("CommonClassId").InnerText);
				string imgName = imgNode.SelectSingleNode("SiteImageName").InnerText.Trim();
				string imgUrl = imgNode.SelectSingleNode("SiteImageUrl").InnerText.Trim();
				string albumName = imgNode.SelectSingleNode("CommonClassName").InnerText.Trim();
				if (imgId == 0 || imgUrl.Length == 0)
					imgUrl = BCCC.WebConfig.DefaultCarPic;
				else
					imgUrl = new BCCC.OldPageBase().GetPublishImage(1, imgUrl, imgId);

				string imgBaseUrl = "http://photo.bitauto.com/exhibit/picture/" + albumId + "/" + imgId;

				htmlCode.AppendLine("<li><a href=\"" + imgBaseUrl + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" alt=\""
					 + albumName + " " + imgName + "\" /></a><a href=\"" + imgBaseUrl + "\" target=\"_blank\">" + albumName + "</a></li>");
			}
			htmlCode.AppendLine("");
			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("</div>");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("<div class=\"more\" style=\"_right:45px\"><a target=\"_black\" href=\"" + m_WomanModulemoreUrl + "\">更多>></a></div>");
			htmlCode.AppendLine("</div>");
			return htmlCode.ToString();

		}
		/// <summary>
		/// 显示视频
		/// </summary>
		/// <returns>视频字符串</returns>
		private string ShowVideo()
		{
			StringBuilder htmlCode = new StringBuilder();
			XmlNodeList videoList = new BCCB.Car_BrandBll().GetMasterBrandVideos(masterId, out m_Video);
			if (videoList != null && videoList.Count > 0)
			{
				m_VideoMoreUrl = "http://v.bitauto.com/car/master/" + masterId.ToString();
				htmlCode.AppendLine("<div class=\"line_box\">");
				htmlCode.AppendLine("<h3><span><a href=\"" + m_VideoMoreUrl.ToString() + "\" target=\"_blank\">" + masterName + "视频</a></span></h3>");
				htmlCode.AppendLine("<div class=\"p_piclist2 p_v3\">");
				htmlCode.AppendLine("<ul>");
				foreach (XmlElement videoNode in videoList)
				{
					string videoTitle = videoNode.SelectSingleNode("title").InnerText;
					videoTitle = BU.StringHelper.RemoveHtmlTag(videoTitle);
					string faceTitle = videoNode.SelectSingleNode("facetitle").InnerText;
					string shortTitle = BU.StringHelper.SubString(BU.StringHelper.RemoveHtmlTag(faceTitle), 14, true);
					if (shortTitle.StartsWith(faceTitle) || shortTitle.Length - faceTitle.Length > 1)
						shortTitle = faceTitle;

					string imgUrl = videoNode.SelectSingleNode("picture").InnerText;
					if (imgUrl.Trim().Length == 0)
						imgUrl = BCCC.WebConfig.DefaultVideoPic;
					string filepath = videoNode.SelectSingleNode("filepath").InnerText;

					// modified by chengl Jul.22.2010
					string duration = "";
					XmlNode xnDuration = videoNode.SelectSingleNode("duration");
					if (xnDuration != null)
					{ duration = videoNode.SelectSingleNode("duration").InnerText; }
					// string duration = videoNode.SelectSingleNode("duration").InnerText;
					htmlCode.Append("<li><a href=\"" + filepath + "\" target=\"_blank\" class=\"v_bg\" alt=\"视频播放\"></a><a href=\"" + filepath + "\" target=\"_blank\"><img src=\""
						+ imgUrl +
						"\" alt=\"" + videoTitle + "\" /></a>");
					htmlCode.AppendLine("<a href=\"" + filepath + "\" title=\"" + videoTitle + "\" target=\"_blank\">" + shortTitle + "</a></li>");
				}
				htmlCode.AppendLine("</ul>");
				htmlCode.AppendLine("</div>");
				htmlCode.AppendLine("<div class=\"clear\"></div>");

				htmlCode.AppendLine("<div class=\"more\" style=\"_right:45px\"><a href=\"" + m_VideoMoreUrl + "\" target=\"_blank\">更多>></a></div>");
				htmlCode.AppendLine("</div>");
			}
			return htmlCode.ToString();

		}
		/// <summary>
		/// 其他厂家
		/// </summary>
		/// <returns>其他厂家字符串</returns>
		private string ShowOtherCompany()
		{
			if (modelPavilion == null || modelPavilion.MasterBrandList.Count < 1)
			{
				return "";
			}

			StringBuilder otherBuilder = new StringBuilder("<div class=\"line_box\"><h3><span><a >"
									 + modelPavilion.Name + "展馆其他厂家</a></span></h3><div class=\"ka_dealer_logo\"><ul>");

			string otherMasterBrandUrl = "";
			string otherImageUrl = "";
			string otherMasterBrandName = "";
			foreach (KeyValuePair<int, int[]> serialKeyPair in modelPavilion.MasterBrandList)
			{
				if (serialKeyPair.Key == masterId)
				{
					continue;
				}

				otherImageUrl = "http://img1.bitauto.com/bt/car/default/images/carimage/m_" + serialKeyPair.Key + "_b.jpg";
				DataRow masterBrandDr = new BCCB.Car_BrandBll().GetCarMasterBrandInfoByBSID(serialKeyPair.Key);

				if (masterBrandDr == null)
				{
					continue;
				}
				otherMasterBrandName = masterBrandDr["bs_Name"].ToString();
				otherMasterBrandUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/2009/" + masterBrandDr["urlspell"].ToString().ToLower() + "/";

				otherBuilder.AppendFormat("<li><a href=\"{1}\" target=\"_black\"><img src=\"{0}\" /></a><p><a href=\"{1}\" target=\"_black\">{2}</a></p></li>"
											, otherImageUrl
											, otherMasterBrandUrl
											, otherMasterBrandName);
			}

			otherBuilder.Append("</ul></div><div class=\"clear\"></div><div class=\"more\"></div></div>");
			return otherBuilder.ToString();
		}
	}
}