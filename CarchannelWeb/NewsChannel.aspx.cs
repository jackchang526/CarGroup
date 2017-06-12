using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb
{
	public partial class NewsChannel : PageBase
	{
		private string newsType = "";		//新闻类型：厂商，主品牌，品牌
		private int typeId = 0;				//相应的ID
		private int pageIndex = 0;			//页号ID
		private int pageSize = 50;			//页面大小
		protected string pageTitle = "";	//页面标题
		protected string metaKeywords = "";	//meta关键字
		protected string metaDescription = "";	//meta描述
		protected string bodyTitle = "";	//新闻块标题
		protected string navString = "";	//面包屑导航
		protected string typeName = "";		//相应的名称
		protected string typeSeoName = "";  //相应的SEO名称
		protected string typeSpell = "";	//相应的全拼
		protected string newsContent = "";	//新闻列表
		protected string prdShortName = "";	//厂商的简称

		bool fromCarChannel = true;
		private int masterId = 0;
		private string masterName = "";
		private string masterSpell = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			GetParameter();
			newsContent = RenderNewsContent();
		}

		/// <summary>
		/// 生成代码
		/// </summary>
		/// <returns></returns>
		private string RenderNewsContent()
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<div id=\"newslist\" class=\"list_date\">");
			int counter = 0;
			int rowCount = 0;
			DataSet ds = null;
			switch (newsType)
			{
				case "producer":
					ds = new CarNewsBll().GetProducerNews(typeId, BitAuto.CarChannel.Common.Enum.CarNewsType.xinwen, pageSize, pageIndex, out rowCount);
					break;
				case "masterbrand":
					ds = new CarNewsBll().GetMasterBrandNews(typeId, BitAuto.CarChannel.Common.Enum.CarNewsType.xinwen, pageSize, pageIndex, ref rowCount);
					break;
				case "brand":
					ds = new CarNewsBll().GetBrandNews(typeId, BitAuto.CarChannel.Common.Enum.CarNewsType.xinwen, pageSize, pageIndex, ref rowCount);
					break;
			}

			int pageCount = rowCount / pageSize + (rowCount % pageSize == 0 ? 0 : 1);

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRowCollection rows = ds.Tables[0].Rows;
				foreach (DataRow row in rows)
				{
					counter++;
					string newsTitle = HttpUtility.HtmlDecode(row["title"].ToString());
					//过滤Html标签
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
					string filePath = row["filepath"].ToString();
					string newsDate = Convert.ToDateTime(row["publishtime"]).ToString("yyyy-MM-dd");

					int position = counter % 5;
					if (position == 1)
						htmlCode.AppendLine("<ul>");
					htmlCode.AppendLine("<li><a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a><small>" + newsDate + "</small></li>");
					if (position == 0 || counter == rows.Count)
						htmlCode.AppendLine("</ul>");
				}
			}
			htmlCode.AppendLine("</div>");

			//翻页
			if (pageCount > 1)
			{
				string baseUrl = "http://car.bitauto.com/" + typeSpell + "/xinwen/{0}/";
				if (newsType == "producer")
					baseUrl = "http://car.bitauto.com/producer/" + typeId + "xinwen-{0}.html";
				if (newsType == "brand" && !fromCarChannel)
				{
					baseUrl = "http://news.bitauto.com/pinpai/" + typeSpell + "/xinwen/{0}/";
				}

				this.AspNetPager1.UrlRewritePattern = baseUrl;
				this.AspNetPager1.RecordCount = rowCount;
				this.AspNetPager1.PageSize = pageSize;
				this.AspNetPager1.CurrentPageIndex = pageIndex;
				this.AspNetPager1.Visible = true;
			}
			return htmlCode.ToString();
		}
		private void GetParameter()
		{
			newsType = ConvertHelper.GetString(Request.QueryString["newsType"]);
			if (newsType == null)
				newsType = "";
			newsType = newsType.ToLower();
			if (newsType != "producer" && newsType != "masterbrand" && newsType != "brand")
				Response.Redirect("http://car.bitauto.com");

			typeId = ConvertHelper.GetInteger(Request.QueryString["id"]);
			if (typeId <= 0)
			{
				if (newsType == "brand")
				{
					typeSpell = ConvertHelper.GetString(Request.QueryString["brandallspell"]).Trim().ToLower();
					Dictionary<string, int> brandDic = new Car_BrandBll().GetBrandSpellDictionary();
					if (brandDic.ContainsKey(typeSpell))
					{
						fromCarChannel = false;
						typeId = brandDic[typeSpell];
					}
				}
				if (typeId <= 0)
					Response.Redirect("http://car.bitauto.com");
			}
			pageIndex = ConvertHelper.GetInteger(Request.QueryString["pageindex"]);
			if (pageIndex <= 0)
				pageIndex = 1;

			typeName = new NewsChannelBll().GetNameByTypeId(newsType, typeId, out typeSpell, out prdShortName, out typeSeoName);

			typeSpell = typeSpell.Trim().ToLower();

			if (newsType == "brand" && fromCarChannel)
			{
				masterId = new Car_BrandBll().GetMasterbrandByBrand(typeId, out masterSpell, out masterName);
			}

			switch (newsType)
			{
				case "producer":
					pageTitle = "【" + typeSeoName + "新闻】-易车网BitAuto.com";
					metaKeywords = typeSeoName + "新闻";
					metaDescription = typeSeoName + ":易车网(BitAuto.com)车型品牌库为您提供，全国近40个重点汽车市场的数千家" + typeName + "经销商的实时汽车报价，海量的" + typeName + "汽车图片，最精彩的" + typeName + "汽车新闻、行情、评测、导购内容,是全国数千万购车意向客户的首选专业汽车导购网站";
					bodyTitle = typeName + "新闻";
					navString = "<a href=\"http://news.bitauto.com\">新闻</a>  &gt; <a href=\"http://news.bitauto.com/pinpai/\">品牌</a> &gt; <a href=\"http://car.bitauto.com/producer/" + typeId + ".html\">" + typeName + "</a> &gt; <strong>新闻</strong>";
					break;
				case "masterbrand":
					pageTitle = "【" + typeSeoName + "汽车新闻】-易车网BitAuto.com";
					metaKeywords = typeSeoName + "汽车新闻";
					metaDescription = typeSeoName + "汽车:易车网(BitAuto.com)车型品牌库为您提供，全国近40个重点汽车市场的数千家" + typeSeoName + "经销商的实时汽车报价，海量的" + typeSeoName + "汽车图片，最精彩的" + typeSeoName + "汽车新闻、行情、评测、导购内容,是全国数千万购车意向客户的首选专业汽车导购网站";
					bodyTitle = typeName + "新闻";
					navString = "<a href=\"http://car.bitauto.com/\">车型</a> &gt; <a href=\"http://car.bitauto.com/" + typeSpell + "/\">" + typeName + "</a> &gt; <strong>新闻</strong>";
					break;
				case "brand":
					int producerId = 0;
					string producerName = new NewsChannelBll().GetProducerBrand(typeId, out producerId);
					pageTitle = "【" + typeSeoName + "汽车新闻】-易车网BitAuto.com";
					metaKeywords = typeSeoName + "汽车新闻";
					metaDescription = typeSeoName + "汽车:易车网(BitAuto.com)车型品牌库为您提供，全国近40个重点汽车市场的数千家" + typeSeoName + "经销商的实时汽车报价，海量的" + typeSeoName + "汽车图片，最精彩的" + typeSeoName + "汽车新闻、行情、评测、导购内容,是全国数千万购车意向客户的首选专业汽车导购网站";
					bodyTitle = typeName + "新闻";
					if (fromCarChannel)
					{
						navString = "<a href=\"http://car.bitauto.com/\">车型</a> &gt; <a href=\"http://car.bitauto.com/" + masterSpell + "/\">" + masterName
							+ "</a> &gt; <a href=\"http://car.bitauto.com/" + typeSpell + "/\">" + typeName + "</a> &gt; <strong>新闻</strong>";
					}
					else
					{
						navString = "<a href=\"http://news.bitauto.com\">新闻</a>  &gt; <a href=\"http://news.bitauto.com/pinpai/\">品牌</a>"
							+ " &gt; <a href=\"http://car.bitauto.com/producer/" + producerId + ".html\">" + producerName + "</a>"
							+ " &gt; <a href=\"http://news.bitauto.com/pinpai/" + typeSpell + "/\">" + typeName + "</a> &gt; <strong>新闻</strong>";
					}
					break;
			}

		}
	}
}