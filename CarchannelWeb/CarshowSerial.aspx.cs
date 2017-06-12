using System;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using model = BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Model;
using bll = BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb
{
	public partial class CarshowSerial : PageBase
	{
		protected Car_SerialBaseEntity serialBase;
		protected string encodedSerialName;
		protected string focusImageHtml;
		protected string topNewsHtml;
		protected string hotCarHtml;
		protected string serialImagesHtml;
		protected string modelImagesHtml;
		protected string otherSerialHtml;
		protected string masterVideosHtml;
		protected string sameTypeSerialHtml;
		protected string dianpingHtml;
		protected int dianpingCount;
		protected string serialDianpingHtml;
		protected string pavilion;

		private int exhibitionId = 5;
		private model.Exhibition exhibitionInfo;
		private model.Pavilion pavInfo;
		private int[] allSerialInMaster;

		private int serialImageCount;
		private int modelImageCount;
		private int masterVideoCount;
		private string serialImageMoreUrl;
		private string modelImageMoreUrl;
		private string masterMoreVideoUrl;

		protected string CsHead = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParameter();

			RenderSerialImages();
			RenderModelImages();
			RenderVideos();

			RenderFocusImage();
			RenderTopNews();
			RenderHotCars();
			RenderOtherSerial();
			RenderSameTypeSerialHtml();
			// RenderDianping();

			//bool isSuccess = false;
			//CsHead = this.GetRequestString(string.Format(WebConfig.HeadForSerial, serialBase.SerialId.ToString(), "GuangZhou2009"), 10, out isSuccess);
			// base.GetCommonNavigation("CsGuangZhou2009", serialBase.SerialId);
		}

		private void GetParameter()
		{
			string serialSpell = ConvertHelper.GetString(Request.QueryString["spell"]);
			if (serialSpell.Length == 0)
				Response.Redirect("http://car.bitauto.com/car/404error.aspx?info=需要子品牌参数！");
			Car_SerialBll serialBll = new Car_SerialBll();

			int serialId = serialBll.GetSerialIdBySpell(serialSpell);
			if (serialId > 0)
				serialBase = serialBll.GetSerialBaseEntity(serialId);

			if (serialId <= 0 || serialBase == null)
				Response.Redirect("http://car.bitauto.com/car/404error.aspx?info=无此子品牌！");

			encodedSerialName = HttpUtility.UrlEncode(serialBase.SerialShowName);

			serialDianpingHtml = base.GetSerialDetailDianPingByCsID(serialBase.SerialId, 1, 5);

			//所在展馆
			exhibitionInfo = bll.Exhibition.GetModelExhibitionByExhibitionID(exhibitionId, 60);
			foreach (model.Pavilion pav in exhibitionInfo.PavilionList.Values)
			{

				if (!pav.MasterBrandList.ContainsKey(serialBase.MasterbrandId))
					continue;
				int[] serialList = pav.MasterBrandList[serialBase.MasterbrandId];
				foreach (int tempSerialId in serialList)
				{
					if (tempSerialId == serialBase.SerialId)
					{
						pavInfo = pav;
						allSerialInMaster = serialList;
						break;
					}
				}
				if (pavInfo != null)
					break;
			}

			if (pavInfo == null)
				Response.Redirect("http://car.bitauto.com/car/404error.aspx?info=此子品牌无展馆信息！");

			pavilion = pavInfo.Name;

		}

		private void RenderFocusImage()
		{
			int classId = 0;
			string imgUrl = new Car_SerialBll().GetCarShowDefaultImage(serialBase.SerialId, "serial", out classId);
			BrandForum bf = new Car_BrandBll().GetBrandForm("masterbrand", serialBase.MasterbrandId);

			string imgToUrl = "http://photo.bitauto.com/exhibit/picture/" + classId + "/0";

			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<div class=\"line_box\">");
			htmlCode.AppendLine("<div class=\"ka_focus\">");
			htmlCode.AppendLine("<div class=\"photo\"><a href=\"" + imgToUrl + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" width=\"300\" height=\"200\" /></a></div>");
			htmlCode.AppendLine("<div class=\"text\">");
			htmlCode.AppendLine("<ul class=\"one\">");
			htmlCode.Append("<li><strong>厂家：</strong><a href=\"http://chezhan.bitauto.com/guangzhou-chezhan/2009/" + serialBase.MasterbrandSpell
				+ "/\" target=\"_blank\" >" + serialBase.MasterBrandName + "</a>");

			if (bf.CampForumUrl.Length > 0)
				htmlCode.Append(" <a href=\"" + bf.CampForumUrl + "\" target=\"_blank\" class=\"hui\">进入论坛&gt;&gt;</a>");
			htmlCode.AppendLine("</li>");

			string pavUrl = "http://Chezhan.bitauto.com/Guangzhou-chezhan/gd_2009/zhanguan/" + pavilion.Replace("馆", "") + "/";
			htmlCode.AppendLine("<li class=\"r\"><strong>展馆：</strong><a href=\"" + pavUrl + "\" target=\"_blank\">" + pavInfo.Name
				+ "</a><a href=\"" + pavUrl + "\" target=\"_blank\" class=\"hui\">进入&gt;&gt;</a></li>");
			htmlCode.AppendLine("</ul>");

			htmlCode.AppendLine("<ul class=\"two\">");
			htmlCode.AppendLine("<li><a href=\"" + serialImageMoreUrl + "\" target=\"_blank\">图片</a>(" + serialImageCount + "张)</li>");
			htmlCode.AppendLine("<li><a href=\"" + modelImageMoreUrl + "\" target=\"_blank\">车模</a>(" + modelImageCount + "张)</li>");
			htmlCode.AppendLine("<li><a href=\"" + masterMoreVideoUrl + "\" target=\"_blank\">视频</a>(" + masterVideoCount + "个)</li>");
			htmlCode.AppendLine("");
			htmlCode.AppendLine("</div></div></div>");
			focusImageHtml = htmlCode.ToString();
		}

		private void RenderTopNews()
		{
			StringBuilder htmlCode = new StringBuilder();
			List<XmlElement> newsList = new Car_BrandBll().GetCarshowTopNews("serial", serialBase.SerialId);
			if (newsList.Count > 0)
			{
				htmlCode.AppendLine("<div class=\"line_box\">");
				htmlCode.AppendLine("<div class=\"ka_topnews\">");
				XmlElement firstNews = newsList[0];
				string newsTitle = firstNews.SelectSingleNode("title").InnerText;
				newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
				string filePath = firstNews.SelectSingleNode("filepath").InnerText;
				htmlCode.AppendLine("<h2><a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a></h2>");
				htmlCode.AppendLine("<div class=\"demarcation\"></div>");
				for (int i = 1; i < newsList.Count; i++)
				{
					if (i == 1 || i == 5)
						htmlCode.AppendLine("<ul>");

					XmlElement newsNode = newsList[i];
					newsTitle = newsNode.SelectSingleNode("title").InnerText;
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
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
			topNewsHtml = htmlCode.ToString();
		}

		private void RenderHotCars()
		{
			//获取数据
			XmlDocument xmlDoc = new Car_SerialBll().GetSerialHotNews(serialBase.SerialId);
			XmlNodeList newsList = xmlDoc.SelectNodes("NewDataSet/NewsCommentTop");

			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<div class=\"line_box\">");
			htmlCode.AppendLine("<h3><span>" + serialBase.SerialName + "热门文章</span></h3>");
			htmlCode.AppendLine("<div class=\"wd_unit\">");
			htmlCode.AppendLine("<ol class=\"wd_hotrank\">");
			foreach (XmlElement newsNode in newsList)
			{
				string newsTitle = newsNode.SelectSingleNode("NewsTitle").InnerText;
				//过滤Html标签
				newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
				string shortNewsTitle = StringHelper.SubString(newsTitle, 26, true);
				string filePath = newsNode.SelectSingleNode("NewsUrl").InnerText;
				string pubTime = newsNode.SelectSingleNode("Time").InnerText;
				pubTime = Convert.ToDateTime(pubTime).ToString("MM-dd");
				if (shortNewsTitle != newsTitle)
					htmlCode.AppendLine("<li><a href=\"" + filePath + "\" title=\"" + newsTitle + "\" target=\"_blank\">" + shortNewsTitle + "</a></li>");
				else
					htmlCode.AppendLine("<li><a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a></li>");
			}
			//htmlCode.AppendLine("<li><a href=\"\"  target=\"_blank\">价值4300万的布加</a><small class=\"up\">1</small></li>");
			htmlCode.AppendLine("</ol>");
			htmlCode.AppendLine("</div>");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("</div>");
			hotCarHtml = htmlCode.ToString();
		}

		private void RenderSerialImages()
		{
			//取图片
			int classId = 0;
			int imgCount = 0;
			List<XmlElement> imgList = new Car_SerialBll().GetCarshowSerilaImages(serialBase.SerialId, out classId, out imgCount);
			serialImageCount = imgCount;

			string imgBaseUrl = "http://photo.bitauto.com/exhibit/picture/" + classId + "/";
			string moreUrl = "http://photo.bitauto.com/exhibit/picture/" + classId + "/0";
			serialImageMoreUrl = moreUrl;

			StringBuilder htmlCode = new StringBuilder();
			//<h3><span><a href="">高尔夫6代车展图片</a></span> <span class="line">|</span> <a href="">车型图片</a></h3>
			htmlCode.AppendLine("<h3><span><a href=\"" + moreUrl + "\" target=\"_blank\">" + serialBase.SerialShowName
				+ "车展图片</a></span><span class=\"line\">|</span> <a class=\"wx_slink\" href=\"http://photo.bitauto.com/serial/" + serialBase.SerialId + ".html\" target=\"_blank\">车型图片</a></h3>");
			htmlCode.AppendLine("<div class=\"lh_atlaspiclist\">");
			htmlCode.AppendLine("<ul class=\"lh_atlas\">");
			foreach (XmlElement imgNode in imgList)
			{
				int imgId = ConvertHelper.GetInteger(imgNode.GetAttribute("imgId"));
				string imgName = imgNode.GetAttribute("imgName");
				string imgUrl = imgNode.GetAttribute("imgUrl").Trim();
				if (imgId == 0 || imgUrl.Length == 0)
					imgUrl = WebConfig.DefaultCarPic;
				else
					imgUrl = new OldPageBase().GetPublishImage(1, imgUrl, imgId);

				htmlCode.AppendLine("<li><a href=\"" + imgBaseUrl + imgId + "\" target=\"_blank\"><img height=\"100\" width=\"150\" src=\"" + imgUrl + "\" alt=\""
					+ serialBase.SerialShowName + " " + imgName + "\"/></a><p><a href=\"" + imgBaseUrl + imgId + "\" target=\"_blank\">" + imgName + "</a></p></li>");
			}

			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("<div class=\"more\"><a href=\"" + moreUrl + "\" target=\"_blank\">更多&gt;&gt;</a></div>");
			htmlCode.AppendLine("</div>");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			serialImagesHtml = htmlCode.ToString();
		}

		private void RenderModelImages()
		{
			int classId = 0;	//主品牌分类ID
			int imgCount = 0;	//模特图片数量
			List<XmlElement> imgList = new Car_SerialBll().GetCarshowMasterModelImages(serialBase.MasterbrandId, out classId, out imgCount);
			modelImageCount = imgCount;
			StringBuilder htmlCode = new StringBuilder();
			string moreUrl = "http://photo.bitauto.com/exhibit/class/" + classId;
			modelImageMoreUrl = moreUrl;
			htmlCode.AppendLine("<h3><span><a href=\"" + moreUrl + "\" target=\"_blank\">" + serialBase.MasterBrandName + "车模</a></span></h3>");
			htmlCode.AppendLine("<div class=\"p_piclist1\">");
			htmlCode.AppendLine("<ul>");
			foreach (XmlElement imgNode in imgList)
			{
				int imgId = ConvertHelper.GetInteger(imgNode.SelectSingleNode("SiteImageId").InnerText);
				int albumId = ConvertHelper.GetInteger(imgNode.SelectSingleNode("CommonClassId").InnerText);
				string imgName = imgNode.SelectSingleNode("SiteImageName").InnerText.Trim();
				string imgUrl = imgNode.SelectSingleNode("SiteImageUrl").InnerText.Trim();
				string albumName = imgNode.SelectSingleNode("CommonClassName").InnerText.Trim();
				if (imgId == 0 || imgUrl.Length == 0)
					imgUrl = WebConfig.DefaultCarPic;
				else
					imgUrl = new OldPageBase().GetPublishImage(1, imgUrl, imgId);

				string imgBaseUrl = "http://photo.bitauto.com/exhibit/picture/" + albumId + "/" + imgId;

				htmlCode.AppendLine("<li><a href=\"" + imgBaseUrl + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" alt=\""
					+ serialBase.SerialShowName + " " + albumName + " " + imgName + "\" /></a><a href=\"" + imgBaseUrl + "\" target=\"_blank\">" + albumName + "</a></li>");
			}
			htmlCode.AppendLine("");
			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("</div>");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("<div class=\"more\"><a href=\"" + moreUrl + "\" target=\"_blank\">更多&gt;&gt;</a></div>");

			modelImagesHtml = htmlCode.ToString();
		}

		private void RenderOtherSerial()
		{
			otherSerialHtml = "";
			if (allSerialInMaster == null || allSerialInMaster.Length == 0)
				return;

			string morUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/2009/" + serialBase.MasterbrandSpell + "/";
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<h3><span><a href=\"" + morUrl + "\" target=\"_blank\">" + serialBase.MasterBrandName + "其它车型</a></span></h3>");
			htmlCode.AppendLine("<div class=\"lh_atlaspiclist\">");
			htmlCode.AppendLine("<ul class=\"lh_atlas\">");

			string[] attrList = new string[] { "首发", "上市", "概念", "即将", "none" };
			Dictionary<string, List<string>> serialDic = new Dictionary<string, List<string>>();
			foreach (int serialId in allSerialInMaster)
			{
				if (serialId == serialBase.SerialId)
					continue;
				string attrName = "none";
				StringBuilder serialCode = new StringBuilder();
				Car_SerialBaseEntity sBase = new Car_SerialBll().GetSerialBaseEntity(serialId);
				if (sBase != null)
				{
					int classId = 0;
					string imgUrl = new Car_SerialBll().GetCarShowDefaultImage(serialId, "serial", out classId);
					string serialUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/2009/" + sBase.MasterbrandSpell + "/" + sBase.SerialNameSpell + "/";
					serialCode.Append("<li><a href=\"" + serialUrl + "\" target=\"_blank\"><img height=\"100\" width=\"150\" src=\"" + imgUrl + "\" alt=\"" + sBase.SerialShowName + "\"/></a><p><a href=\"" + serialUrl + "\" target=\"_blank\">" + sBase.SerialShowName + "</a>");

					List<model.Attribute> serialAttrs = bll.Exhibition.GetAttributeListBySerialID(sBase.SerialId, exhibitionId, 10);
					if (serialAttrs != null && serialAttrs.Count > 0)
					{
						attrName = serialAttrs[0].Name;
						string attrUrl = "none";
						switch (attrName)
						{
							case "首发新车":
								attrName = "首发";
								attrUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/gd_2009/xinchefabu/";
								break;
							case "上市新车":
								attrName = "上市";
								attrUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/gd_2009/xinchefabu/index2.shtml";
								break;
							case "概念车":
								attrName = "概念";
								attrUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/gd_2009/xinchefabu/index4.shtml";
								break;
							case "即将上市":
								attrName = "即将";
								attrUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/gd_2009/xinchefabu/index3.shtml";
								break;
							default:
								attrName = "none";
								break;
						}
						serialCode.Append("<span class=\"sf\"><a href=\"" + attrUrl + "\" target=\"_blank\">" + attrName + "</a></span>");
					}
					serialCode.Append("</p>");
					string price = base.GetSerialPriceRangeByID(sBase.SerialId);
					if (price.Length == 0)
						serialCode.Append("<p>暂无报价</p>");
					else
						serialCode.Append("<p>价格：" + price + "</p>");
					serialCode.AppendLine("</li>");
				}

				if (!serialDic.ContainsKey(attrName))
					serialDic[attrName] = new List<string>();

				serialDic[attrName].Add(serialCode.ToString());
			}

			//输出到Html代码中
			foreach (string tAttrName in attrList)
			{
				if (!serialDic.ContainsKey(tAttrName))
					continue;
				foreach (string tempCode in serialDic[tAttrName])
				{
					htmlCode.AppendLine(tempCode);
				}
			}

			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("</div>");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("<div class=\"more\"><a href=\"" + morUrl + "\" target=\"_blank\">更多&gt;&gt;</a></div>");

			otherSerialHtml = htmlCode.ToString();
		}

		private void RenderVideos()
		{
			StringBuilder htmlCode = new StringBuilder();
			int videoCount = 0;
			XmlNodeList videoList = new Car_BrandBll().GetMasterBrandVideos(serialBase.MasterbrandId, out videoCount);
			masterVideoCount = videoCount;
			masterMoreVideoUrl = "http://v.bitauto.com/car/master/" + serialBase.MasterbrandId.ToString();
			if (videoList != null && videoList.Count > 0)
			{
				htmlCode.AppendLine("<h3><span><a href=\"" + masterMoreVideoUrl + "\" target=\"_blank\">" + serialBase.MasterBrandName + "视频</a></span></h3>");
				htmlCode.AppendLine("<div class=\"p_piclist2 p_v3\">");
				htmlCode.AppendLine("<ul>");
				foreach (XmlElement videoNode in videoList)
				{
					string videoTitle = videoNode.SelectSingleNode("title").InnerText;
					videoTitle = StringHelper.RemoveHtmlTag(videoTitle);
					string faceTitle = videoNode.SelectSingleNode("facetitle").InnerText;
					string shortTitle = StringHelper.SubString(StringHelper.RemoveHtmlTag(faceTitle), 14, true);
					if (shortTitle.StartsWith(faceTitle) || shortTitle.Length - faceTitle.Length > 1)
						shortTitle = faceTitle;

					string imgUrl = videoNode.SelectSingleNode("picture").InnerText;
					if (imgUrl.Trim().Length == 0)
						imgUrl = WebConfig.DefaultVideoPic;
					string filepath = videoNode.SelectSingleNode("filepath").InnerText;

					// modified by chengl Jul.22.2010
					string duration = "";
					XmlNode xnDuration = videoNode.SelectSingleNode("duration");
					if (xnDuration != null)
					{ duration = videoNode.SelectSingleNode("duration").InnerText; }
					// string duration = videoNode.SelectSingleNode("duration").InnerText;
					htmlCode.Append("<li><a href=\"" + filepath + "\" target=\"_blank\" class=\"v_bg\" alt=\"视频播放\"></a><a href=\"" + filepath + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" alt=\"" + videoTitle + "\" /></a>");
					htmlCode.AppendLine("<a href=\"" + filepath + "\" title=\"" + videoTitle + "\" target=\"_blank\">" + shortTitle + "</a></li>");
				}
				htmlCode.AppendLine("</ul>");
				htmlCode.AppendLine("</div>");
				htmlCode.AppendLine("<div class=\"clear\"></div>");
				htmlCode.AppendLine("<div class=\"more\" style=\"_right:45px\"><a href=\"" + masterMoreVideoUrl + "\" target=\"_blank\">更多>></a></div>");
			}
			masterVideosHtml = htmlCode.ToString();
		}

		private void RenderSameTypeSerialHtml()
		{
			List<model.Attribute> serialAttrs = bll.Exhibition.GetAttributeListBySerialID(serialBase.SerialId, exhibitionId, 60);
			StringBuilder htmlCode = new StringBuilder();
			if (serialAttrs != null && serialAttrs.Count > 0)
			{
				htmlCode.Append("<div class=\"col-all\">");
				string moreUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/gd_2009/xinchefabu";
				foreach (model.Attribute attr in serialAttrs)
				{
					if (attr.Name == "全球首发")
						moreUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/gd_2009/xinchefabu?tags=1";
					else if (attr.Name == "车展上市")
						moreUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/gd_2009/xinchefabu?tags=2";
					else if (attr.Name == "即将上市")
						moreUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/gd_2009/xinchefabu?tags=3";
					else if (attr.Name == "概念车")
						moreUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/gd_2009/xinchefabu?tags=4";

					htmlCode.AppendLine("<div class=\"line_box\">");
					htmlCode.AppendLine("<h3><span><a href=\"" + moreUrl + "\" target=\"_blank\">" + attr.Name + "其他车型</a></span></h3>");
					htmlCode.AppendLine("<div class=\"lh_atlaspiclist\">");
					htmlCode.AppendLine("<ul class=\"lh_atlas\">");
					int counter = 0;
					foreach (int serialId in attr.SerialIDList.Keys)
					{
						if (serialId == serialBase.SerialId)
							continue;
						counter++;
						Car_SerialBaseEntity sBase = new Car_SerialBll().GetSerialBaseEntity(serialId);
						if (sBase != null)
						{
							int classId = 0;
							string imgUrl = new Car_SerialBll().GetCarShowDefaultImage(serialId, "serial", out classId);
							string serialUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/2009/" + sBase.MasterbrandSpell + "/" + sBase.SerialNameSpell + "/";
							htmlCode.AppendLine("<li><a href=\"" + serialUrl + "\" target=\"_blank\"><img height=\"100\" width=\"150\" src=\"" + imgUrl + "\" alt=\"" + sBase.SerialShowName + "\"/></a><p><a href=\"" + serialUrl + "\" target=\"_blank\">" + sBase.SerialShowName + "</a></p></li>");
						}
						if (counter >= 10)
							break;
					}
					htmlCode.AppendLine("</ul>");
					htmlCode.AppendLine("</div>");
					htmlCode.AppendLine("<div class=\"clear\"></div>");
					htmlCode.AppendLine("<div class=\"more\"><a href=\"" + moreUrl + "\">更多&gt;&gt;</a></div>");
					htmlCode.AppendLine("</div>");
				}
				htmlCode.AppendLine("</div>");
			}
			sameTypeSerialHtml = htmlCode.ToString();
		}

	}
}