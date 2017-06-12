using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannel.CarchannelWeb.PageSerial
{
	public partial class CsRegionNews : PageBase
	{
		protected int serialId;			//子品牌ID
		protected string serialSpell;	//子品牌全拼
		protected string serialName;	//子品牌名称
		protected string serialShowName;//子品牌显示名
		protected string serialSEOName; //子品牌SEO名称
		private int cityId;				//城市ID
		protected string citySpell;		//城市拼音
		protected string cityName;		//城市名称
		protected string newsType;		//新闻类型
		protected string pageTitle;		//页面标题
		protected string pageKeywords;	//页面SEO关键字
		protected string pageDesc;		//页面SEO说明
		protected string CsHead = string.Empty;		//子品牌综述页头
		protected string cityLogoStyle = string.Empty;		//城市Logo样式

		private string contentTitle = string.Empty;

		private int pageSize = 20;
		private int pageIndex = 1;
		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(15);
			GetParameter();
			base.MakeSerialTopADCode(serialId);
			lrContent.Text = RenderContentHtml();
			//bool isSuccess = false;
			//CsHead = this.GetRequestString(string.Format(WebConfig.HeadForSerial + "&cityID=" + cityId.ToString(), serialId.ToString(), "CsZone"), 10, out isSuccess);
			CsHead = base.GetCommonNavigation("CsCity", serialId).Replace("#CityID#", cityId.ToString()).Replace("#CityName#", cityName.ToString()).Replace("#CitySpell#", citySpell.ToString());
		}
		private void GetParameter()
		{
			serialSpell = Convert.ToString(Request.QueryString["serial"]);
			if (String.IsNullOrEmpty(serialSpell))
				Response.Redirect("http://car.bitauto.com");

			citySpell = Convert.ToString(Request.QueryString["city"]);
			if (String.IsNullOrEmpty(citySpell))
				Response.Redirect("http://car.bitauto.com");

			Dictionary<string, int> serialDic = AutoStorageService.GetSerialSpellDic();
			if (!serialDic.ContainsKey(serialSpell))
			{
				Response.Redirect("http://car.bitauto.com");
			}
			serialId = serialDic[serialSpell];
			Car_SerialBaseEntity serialBase = new Car_SerialBll().GetSerialBaseEntity(serialId);
			// modified by chengl Mar.2.2010
			if (serialBase == null)
			{ Response.Redirect("http://car.bitauto.com"); }
			serialName = serialBase.SerialName;
			serialShowName = serialBase.SerialShowName;
			serialSEOName = serialBase.SerialSeoName;

			cityLogoStyle = "bt_logo_loca";
			Dictionary<string, City> cityDic = AutoStorageService.GetCitySpellDic();
			if (cityDic.ContainsKey(citySpell))
			{
				cityId = cityDic[citySpell].CityId;
				cityName = cityDic[citySpell].CityName;
				if (cityName.Length > 2)
					cityLogoStyle += cityName.Length.ToString();
			}
			else
			{
				// 不存在城市 404
				Response.Redirect("http://car.bitauto.com/car/404error.aspx?info=城市不存在");
			}

			newsType = Request.QueryString["type"];
			if (newsType == "hangqing")
			{
				pageTitle = "【" + cityName + serialSEOName + "行情导购信息】-易车网";
				pageKeywords = cityName + serialSEOName + "，" + cityName + serialSEOName + "行情导购";
				pageDesc = cityName + serialSEOName + "行情导购:易车网(BitAuto.com)为您提供最新的" + cityName + "地区" + serialSEOName + "行情导购信息,同时为您提供全国各地的" + serialSEOName + "最新汽车行情信息，是您购车的首选网站！";
				contentTitle = cityName + serialSEOName + "行情导购";
			}
			else if (newsType == "cuxiao")
			{
				pageTitle = "【" + cityName + serialSEOName + "降价促销-" + cityName + serialSEOName + "优惠促销活动】_" + serialBase.MasterBrandName + serialSEOName + "-易车网";
				pageKeywords = cityName + serialSEOName + "降价促销，" + cityName + serialSEOName + "优惠促销";
				pageDesc = cityName + serialSEOName + "促销:易车网车型频道为您提供最权威的" + cityName + serialSEOName + "降价促销信息、" + cityName + serialSEOName + "优惠促销活动、" + serialSEOName + "最新行情报价、" + serialSEOName + "优惠、" + serialSEOName + "经销商降价信息、网友点评讨论等。";
				contentTitle = cityName + serialSEOName + "促销·活动";
			}
			else
			{
				newsType = "diannei";
				pageTitle = "【" + cityName + serialSEOName + "店内活动】-易车网";
				pageKeywords = cityName + serialSEOName + "店内活动";
				pageDesc = "";
				contentTitle = cityName + serialSEOName + "店内活动";
			}

			if (Request.QueryString["pageIndex"] != null && Request.QueryString["pageIndex"].ToString() != "")
			{
				bool s = int.TryParse(Request.QueryString["pageIndex"], out pageIndex);
				if (!s || pageIndex == 0)
				{
					pageIndex = 1;
				}
				//pageIndex = Convert.ToInt32(Request.QueryString["pageIndex"]);
				//if (pageIndex == 0)
				//	pageIndex = 1;
			}
		}

		private string RenderContentHtml()
		{
			List<string> htmlList = new List<string>();
			RenderNewsList(htmlList);
			return String.Concat(htmlList.ToArray());
		}

		private void RenderNewsList(List<string> htmlList)
		{
			//htmlCode.AppendLine("<div class=\"col-con\">");
			// htmlList.Add("<div class=\"line_box dglist\">");
			// modified by chengl Oct.12.2011
			// htmlList.Add("<h3 class=\"w718\"><span>" + contentTitle + "</span></h3>");
			//获取新闻数据
			List<News> newsList = null;
			if (newsType == "hangqing")
				newsList = new Car_SerialBll().GetHangqingNewsBySerialAndCity(serialId, cityName);
			else
			{
				newsList = new Car_SerialBll().GetSerialCityNews(serialId, cityId, newsType);
			}

			// fixed by chengl May.28.2013 exception when newsList is null
			if (newsList == null)
			{ return; }

			htmlList.Add("<div class=\"line_box dglist\">");
			htmlList.Add("<h3 class=\"w718\"><span>" + contentTitle + "</span></h3>");

			int newsCount = newsList.Count;
			int pageCount = newsCount / pageSize + (newsCount % pageSize == 0 ? 0 : 1);

			int startIndex = (pageIndex - 1) * pageSize + 1;
			int endIndex = startIndex + pageSize - 1;

			int rowCounter = 0;
			foreach (News news in newsList)
			{
				rowCounter++;
				if (rowCounter < startIndex)
					continue;

				string newsTitle = "";
				string newsUrl = "";
				DateTime publishTime = DateTime.Now;
				string from = "";
				string author = "";
				int commentNum = 0;
				if (newsType == "hangqing")
				{
					newsTitle = news.Title;
					newsUrl = news.PageUrl;
					publishTime = news.PublishTime;
					from = news.SourceName;
					author = news.Author;
					commentNum = news.CommentNum;
				}
				else
				{
					newsTitle = news.Title;
					newsUrl = news.PageUrl;
					publishTime = news.PublishTime;
				}
				newsTitle = StringHelper.RemoveHtmlTag(newsTitle);

				htmlList.Add("<dl>");
				htmlList.Add("<dt><a href=\"" + newsUrl + "\" target=\"_blank\">" + newsTitle + "</a></dt>");
				htmlList.Add("<dd class=\"info\">");
				htmlList.Add(publishTime.ToString("yyyy年MM月dd日 HH:mm"));
				htmlList.Add("&nbsp; &nbsp; &nbsp;");
				if (newsType == "cuxiao")
					htmlList.Add("来源：经销商");
				else if (from.Length == 0)
					htmlList.Add("来源：不详");
				else
					htmlList.Add("来源：" + from);
				if (newsType != "cuxiao")
				{
					if (author.Length == 0)
						htmlList.Add("&nbsp; &nbsp; &nbsp;作者：不详");
					else
						htmlList.Add("&nbsp; &nbsp; &nbsp;作者：" + author);
					htmlList.Add("&nbsp; &nbsp; &nbsp;评论：" + commentNum + "条");
				}
				htmlList.Add("</dd>");
				htmlList.Add("</dl>");

				if (rowCounter == endIndex)
					break;
			}

			//生成页号导航
			if (pageCount > 1)
			{
				htmlList.Add("<div class=\"the_pages\"><div>");
				string baseUrl = "http://" + citySpell + ".bitauto.com/car/" + serialSpell + "/" + newsType + "/";

				if (pageIndex > 1)
				{
					int preIndex = pageIndex - 1;
					htmlList.Add("<a href=\"" + baseUrl + "1\" class=\"preview_on\">首页</a>");
					htmlList.Add("<a href=\"" + baseUrl + preIndex + "\" class=\"preview_on\">上一页</a>");
				}

				int startPageIndex = pageIndex - 5;
				if (startPageIndex < 1)
					startPageIndex = 1;
				int endPageIndex = startPageIndex + 10;
				if (endPageIndex > pageCount)
					endPageIndex = pageCount;


				for (int i = startPageIndex; i <= endPageIndex; i++)
				{
					if (i == pageIndex)
						htmlList.Add("<a class=\"linknow\">" + i + "</a>");
					else
						htmlList.Add("<a href=\"" + baseUrl + i + "\">" + i + "</a>");
				}

				if (pageIndex < pageCount)
				{
					int nextIndex = pageIndex + 1;
					htmlList.Add("<a href=\"" + baseUrl + nextIndex + "\" class=\"next_on\">下一页</a>");
					htmlList.Add("<a href=\"" + baseUrl + pageCount + "\" class=\"next_on\">末页</a>");
				}
				htmlList.Add("</div></div>");
			}


			htmlList.Add("</div>");
		}

		private void RenderLookedBrand(StringBuilder htmlCode)
		{
			//htmlCode.AppendLine("<script charset=\"gb2312\" type=\"text/javascript\" src=\"http://go.bitauto.com/handlers/ModelsBrowsedRecently.ashx\"></script>");
			//htmlCode.AppendLine("<div class=\"line_box review_list\">");
			//htmlCode.AppendLine("<h3><span>我看过的品牌</span></h3>");
			//htmlCode.AppendLine("<ul id=\"ulForVisitSerial\" class=\"list\">");
			//htmlCode.AppendLine("</ul>");
			//htmlCode.AppendLine("</div>");
			//htmlCode.AppendLine("</div>"); 
		}
	}
}