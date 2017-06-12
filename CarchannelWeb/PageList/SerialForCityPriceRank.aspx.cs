using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;
using System.Text;
using BitAuto.CarChannel.BLL;
using System.Xml;

namespace BitAuto.CarChannel.CarchannelWeb.PageList
{
	public partial class SerialForCityPriceRank : PageBase
	{
		private string citySpell;
		private int cityId;

		protected int price;
		protected string cityName;
		protected string fullName;
		protected string seoFullName = string.Empty;

		protected string cityListHtml = string.Empty;
		protected string serialsHtml = string.Empty;
		protected string priceHtml = string.Empty;
		string[] cityList;
		Dictionary<string, City> cityDic;
		Dictionary<int, string> dictPrice;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);
			InitData();
			Getparameters();
			RenderPrice();
			RenderCityList();
			RenderSerialPaihang();
		}

		private void InitData()
		{
			cityDic = AutoStorageService.GetCitySpellDic();
			cityList = new string[] { "beijing", "shanghai", "guangzhou", "shenzhen", "fuzhou", "nanjing", "suzhou", "hangzhou", "ningbo", "hefei", "zhengzhou", "nanchang", "wuhan", "changsha", "chengdu", "chongqing", "kunming", "xian", "lanzhou", "taiyuan", "shijiazhuang", "jinan", "qingdao", "tianjin", "changchun", "shenyang", "dalian", "haerbin", "huhehaote" };
			dictPrice = new Dictionary<int, string>() { { 1, "5万以下" }, { 2, "5-8万" }, { 3, "8-12万" }, { 4, "12-18万" }, { 5, "18-25万" }, { 6, "25-40万" }, { 7, "40-80万" }, { 8, "80万以上" } };
		}

		private void Getparameters()
		{
			price = ConvertHelper.GetInteger(Request.QueryString["price"]);
			citySpell = Request.QueryString["city"];
			if (String.IsNullOrEmpty(citySpell))
			{
				cityName = "全国";
				citySpell = "";
			}
			if (dictPrice.ContainsKey(price))
				fullName = dictPrice[price];
			seoFullName = cityDic.ContainsKey(citySpell) ? cityDic[citySpell].CityName + fullName : fullName;
		}

		private void RenderPrice()
		{
			List<string> list = new List<string>();

			int i = 0;
			foreach (var p in dictPrice)
			{
				i++;
				if (i == dictPrice.Count)
					list.Add(string.Format("<li class=\"last {2}\"><a href=\"/jiage/{0}/\">{1}</a></li>",
					p.Key, p.Value, p.Key == price ? "current" : ""));
				else
					list.Add(string.Format("<li class=\"{2}\"><a href=\"/jiage/{0}/\">{1}<span>|</span></a></li>",
						p.Key, p.Value, p.Key == price ? "current" : ""));
			}
			priceHtml = string.Concat(list.ToArray());
		}

		private void RenderCityList()
		{
			StringBuilder htmlCode = new StringBuilder();
			string baseUrl = "/jiage/" + price + "/";
			if (citySpell.Length == 0)
				htmlCode.AppendLine("<dd class=\"current\">全国</dd>");
			else
				htmlCode.AppendLine("<dd><a href=\"" + baseUrl + "\">全国</a></dd>");
			foreach (string spell in cityList)
			{
				string tmpCityName = cityDic[spell].CityName;
				if (spell == citySpell)
				{
					htmlCode.AppendLine("<dd class=\"current\">" + tmpCityName + "</dd>");
					cityId = cityDic[spell].CityId;
					cityName = tmpCityName;
				}
				else
				{
					htmlCode.AppendFormat("<dd><a href=\"{0}{1}/\">{2}</a></dd>", baseUrl, spell, tmpCityName);
				}
			}
			cityListHtml = htmlCode.ToString();
		}

		private void RenderSerialPaihang()
		{
			StringBuilder htmlCode = new StringBuilder();
			string baseUrl = "/";
			XmlNodeList serialList = new Car_LevelBll().GetSerialPVSortByPriceAndCity(cityId, price.ToString());
			if (serialList != null)
			{
				int counter = 0;
				foreach (XmlElement serialNode in serialList.Cast<XmlElement>().Take(300))
				{
					counter++;
					string showName = serialNode.GetAttribute("ShowName");
					string spell = serialNode.GetAttribute("AllSpell");
					if (counter <= 3)
					{
						htmlCode.AppendFormat("<li class=\"fist_three\"><em>{0}</em><a href=\"{1}\" target=\"_blank\">{2}</a></li>",
							counter, baseUrl + spell + "/", showName);
					}
					else
					{
						htmlCode.AppendFormat("<li><em>{0}</em><a href=\"{1}\" target=\"_blank\">{2}</a></li>", counter, baseUrl + spell + "/", showName);
					}
				}
			}
			serialsHtml = htmlCode.ToString();
		}
	}
}