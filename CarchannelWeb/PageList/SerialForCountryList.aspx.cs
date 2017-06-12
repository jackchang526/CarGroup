using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Caching;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL;

namespace BitAuto.CarChannel.CarchannelWeb.PageList
{
	public partial class SerialForCountryList : PageBase
	{
		int masterBrandNum = 0;
		int masterCounter = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
            base.SetPageCache(10);
            lrContent.Text = GetRenderedHtml();
		}

		private string GetRenderedHtml()
		{
			string cacheKey = "serial-country-list";
			object objHtml = null;
			CacheManager.GetCachedData(cacheKey, out objHtml);
			if (objHtml == null)
			{
				objHtml = RenderCountry();
				CacheDependency cacheDepend = new CacheDependency(WebConfig.AutoDataFile);
				CacheManager.InsertCache(cacheKey, objHtml, cacheDepend, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
			}

			return (string)objHtml;
		}


		/// <summary>
		/// 按国家分类
		/// </summary>
		/// <returns></returns>
		private string RenderCountry()
		{
			//获取数据xml
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			//遍历所有主品牌节点
			XmlNodeList mbNodeList = mbDoc.SelectNodes("/Params/MasterBrand");
			masterBrandNum = mbNodeList.Count;

			string[] countryList = new string[] { "zz", "dx", "rx", "mx", "hx", "fx", "yx", "yx2", "other" };
			Dictionary<string, CountryMasterBrands> cmDic = new Dictionary<string, CountryMasterBrands>();

			foreach (string cLabel in countryList)
				cmDic[cLabel] = new CountryMasterBrands(cLabel);

			//将主品牌加入字典
			foreach (XmlElement mbNode in mbNodeList)
			{

				string newCountryName = mbNode.GetAttribute("Country");
				string countryLabel = CountryMasterBrands.GetCountryLabel(newCountryName);
				cmDic[countryLabel].Add(mbNode);
			}

			//生成Html
			List<string> htmlCode = new List<string>();
			htmlCode.Add("<dl class=\"bybrand_list bynation_list\">");

			bool isFirstCountry = true;

			foreach (string cLabel in countryList)
			{
				CountryMasterBrands cmBrand = cmDic[cLabel];
				//国系
				if (isFirstCountry)
				{
					htmlCode.Add("<dt><label>" + cmBrand.CountryName + "</label><div><span id=\"" + cLabel + "\">&nbsp;</span></div></dt>");
					isFirstCountry = false;
				}
				else
					htmlCode.Add("<dt><label class=\"" + GetCountryLabelClass(cLabel) + "\">" + cmBrand.CountryName + "</label><div><span id=\"" + cLabel + "\">&nbsp;</span></div></dt>");
				RenderMasterBand(htmlCode, cmBrand);
			}

			htmlCode.Add("</dl>");

			return String.Concat(htmlCode.ToArray());

		}

		/// <summary>
		/// 生成主品牌的代码
		/// </summary>
		/// <param name="htmlCode"></param>
		/// <param name="masterBrandList"></param>
		private void RenderMasterBand(List<string> htmlCode, CountryMasterBrands masterBrandList)
		{
			foreach (XmlElement mbNode in masterBrandList)
			{
				masterCounter++;
				//生成主品牌图标
				string mbId = mbNode.GetAttribute("ID");
				string mbName = mbNode.GetAttribute("Name");
				string masterSpell = mbNode.GetAttribute("AllSpell").ToLower();
				htmlCode.Add("<dd class=\"b\">");
				htmlCode.Add("<a href=\"/" + masterSpell + "/\" target=\"_blank\"><div class=\"brand m_" + mbId + "_b\"></div></a>");
				htmlCode.Add("<div class=\"brandname\"><a href=\"/" + masterSpell + "/\" target=\"_blank\">" + mbName + "</a></div>");
				htmlCode.Add("</dd>");
				//生成主品牌列表
				RenderBrands(htmlCode, mbNode);
			}
		}

		/// <summary>
		/// 生成主品牌下各品牌的Html
		/// </summary>
		/// <param name="htmlCode">代码容器</param>
		/// <param name="mbNode">主品牌信息</param>
		private void RenderBrands(List<string> htmlCode, XmlElement mbNode)
		{
			htmlCode.Add("<dd class=\"have\">");
			//获取品牌信息
			List<XmlElement> brandList = new List<XmlElement>();
			foreach (XmlElement ele in mbNode.SelectNodes("Brand"))
			{
				brandList.Add(ele);
			}
			//添加排序条件
			brandList.Sort(NodeCompare.CompareBrandNodeSelfFirst);

			bool isFirstBrand = true;
			int brandCounter = 0;
			foreach (XmlElement brandNode in brandList)
			{
				brandCounter++;
				//生成品牌Html
				string brandId = brandNode.GetAttribute("ID");
				string brandName = brandNode.GetAttribute("Name");
				string brandSpell = brandNode.GetAttribute("AllSpell");
				if (isFirstBrand)
				{
					htmlCode.Add("<h2><a href=\"/" + brandSpell + "/\" target=\"_blank\">" + brandName + "</a></h2>");
					isFirstBrand = false;
				}
				else
					htmlCode.Add("<h2 class=\"border\"><a href=\"/" + brandSpell + "/\" target=\"_blank\">" + brandName + "</a></h2>");

				//加入列表
				XmlNodeList serialNodeList = brandNode.SelectNodes("Serial");
				List<XmlElement> serialList = new List<XmlElement>();
				foreach (XmlElement serialNode in serialNodeList)
				{
					serialList.Add(serialNode);
				}

				//生成子品牌列表
				new Car_SerialBll().RenderSerialsBySpell(htmlCode, serialList, false);
			}

			htmlCode.Add("</dd>");

			//只在全页最后一个品牌后不输出此行
			if (masterCounter != masterBrandNum || brandCounter != brandList.Count)
				htmlCode.Add("<dd class=\"line\"></dd>");
		}

		/// <summary>
		/// 取国家的Label样式名
		/// </summary>
		/// <param name="countryID"></param>
		private string GetCountryLabelClass(string countryID)
		{
			string className = "";
			// 自主
			if (countryID == "zz")
			{ className = "china"; }
			// 德国
			if (countryID == "dx")
			{ className = "germany"; }
			// 小日本 
			if (countryID == "rx")
			{ className = "japan"; }
			// 美国
			if (countryID == "mx")
			{ className = "america"; }
			// 高丽棒子 
			if (countryID == "hx")
			{ className = "korea"; }
			// 法国
			if (countryID == "fx")
			{ className = "france"; }
			// 英国
			if (countryID == "yx")
			{ className = "britain"; }
			// 意大利
			if (countryID == "yx2")
			{ className = "italy"; }
			// 其他
			if (countryID == "other")
			{ className = "others"; }
			return className;
		}
	}
}