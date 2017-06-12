using System;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL;

namespace BitAuto.CarChannel.CarchannelWeb.PageList
{
	public partial class SerialForSinglePriceList : System.Web.UI.Page
	{
		int price = 0;			//报价区间代号
		protected string priceText;
		protected int priceTagId;				//标签当前ID
		Dictionary<int, string> priceTextDic;	//报价文本字典
		Dictionary<int, string> priceTagDic;	//报价标签文本

		protected string hotSerialHtml;
		protected string hotNewCarHtml;
		protected string navHtml;

		protected void Page_Load(object sender, EventArgs e)
		{
			InitData();
			GetParameter();
			lrContent.Text = RenderByPrice();
			//生成Tab标签
			RenderPriceTab();

		}

		private void GetParameter()
		{
			string priceStr = Convert.ToString(Request.QueryString["price"]);
			if (String.IsNullOrEmpty(priceStr))
				priceStr = "1";

			price = Convert.ToInt32(priceStr);

			priceTagId = price + 10;
			priceText = priceTextDic[price];
		}

		// 	private string GetRenderedHtml()
		// 	{
		// 		string cacheKey = "serial-singleprice-list-" + price;
		// 		object objHtml = null;
		// 		CacheManager.GetCachedData(cacheKey, out objHtml);
		// 		if (objHtml == null)
		// 		{
		// 			objHtml = RenderByPrice();
		// 			CacheDependency cacheDepend = new CacheDependency(WebConfig.AutoDataFile);
		// 			CacheManager.InsertCache(cacheKey, objHtml, cacheDepend, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
		// 		}
		// 
		// 		return (string)objHtml;
		// 	}

		/// <summary>
		/// 按子品牌报价生成Html代码
		/// </summary>
		/// <returns></returns>
		private string RenderByPrice()
		{
			//获取数据xml
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			//遍历所有子品牌节点
			XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial[contains(@MultiPriceRange,\"," + price + ",\")]");

			Dictionary<string, string> htmlDic = new Car_SerialBll().GetHomepageHotSerial(serialNodeList, price.ToString());
			hotSerialHtml = htmlDic["hotSerial"];
			hotNewCarHtml = htmlDic["hotNewCar"];

			//将所有子品牌按价格,箱式，分类列表
			List<XmlElement> serialNodes = new List<XmlElement>();
			foreach (XmlElement serialNode in serialNodeList)
			{
				serialNodes.Add(serialNode);
			}

			//生成Html
			List<string> htmlCode = new List<string>();

			//htmlCode.AppendLine("<div class=\"line_box newbyl\">");
			new Car_SerialBll().RenderForPrice(htmlCode, serialNodes, price, true, true, priceTextDic[price], false);
			//htmlCode.AppendLine("</div>");

			return String.Concat(htmlCode.ToArray());
		}

		/// <summary>
		/// 生成导航
		/// </summary>
		/// <returns></returns>
		private void RenderPriceTab()
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<ul class=\"tab\">");
			htmlCode.AppendLine("<li><a href=\"/\">汽车大全</a></li>");
			for (int i = 1; i <= 8; i++)
			{
				string onClass = "";
				if (i == price)
					htmlCode.AppendLine("<li class=\"on\"><a>" + priceTextDic[i] + "</a></li>");
				else
					htmlCode.AppendLine("<li><a href=\"/price/" + i + "/\" >" + priceTextDic[i] + "</a></li>");
			}
			htmlCode.AppendLine("</ul>");
			navHtml = htmlCode.ToString();
		}


		/// <summary>
		/// 初始化原始数据
		/// </summary>
		private void InitData()
		{
			priceTextDic = new Dictionary<int, string>();
			priceTextDic[1] = "5万以下";
			priceTextDic[2] = "5万-8万";
			priceTextDic[3] = "8万-12万";
			priceTextDic[4] = "12万-18万";
			priceTextDic[5] = "18万-25万";
			priceTextDic[6] = "25万-40万";
			priceTextDic[7] = "40万-80万";
			priceTextDic[8] = "80万以上";

			priceTagDic = new Dictionary<int, string>();
			priceTagDic[1] = "0-5";
			priceTagDic[2] = "5-8";
			priceTagDic[3] = "8-12";
			priceTagDic[4] = "12-18";
			priceTagDic[5] = "18-25";
			priceTagDic[6] = "25-40";
			priceTagDic[7] = "10-80";
			priceTagDic[8] = "80-";
		}
	}
}