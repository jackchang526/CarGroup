using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.BLL.CarTreeData;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Tree
{
	public partial class LeftTreeJs : PageBase
	{
		protected string treeHtml;

		protected string tagType;			//各标签
		protected string pageType;		//页面类型，home,masterbrand,brand,serial,search
		protected int objId;
		protected string cityId;		//城市ID，无此信息时为空字符串
		protected string cityCode;		//城市拼写，或其他城市信息的规则字符串
		protected string keyWord;
		protected string allSpell;
		protected string customBaseUrl;			//调用方指定的基路径
		protected string showType;				//经销商专用的一个参数

		protected int expand;				//是否只取主品牌的数据HTML

		protected string SearchParametersScript;		//设置选车参数与Cookie的脚本
		protected bool needSetSearchParas;				//是否需要设置选车参数

		protected string SetUrlCityScript;				//设置标签URL里City的脚本
		protected bool needSetUrlCity;					//是否需要设置标签上的Url里的城市


		protected bool needExpandMaster;	//如果一个标签的主品牌的UrlRule为空，则这个主品牌需要异步展开，而不是跳转链接。

		private int masterId;
		private int brandId;
		private int serialId;
		private string charString;

		private Dictionary<string, TagData> tagDic;		//标签数据字典
		private DataNodeCollection dataCollection;		//标签中各品牌数据
		private List<string> htmlList;					//HTML代码

		private int dataIndex;							//因dataCollection是按字母排序的，所以按字母生成时，用此变量做各字母的开始索引

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(15);
			Response.ContentType = "application/x-javascript";
			charString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			treeHtml = String.Empty;
			needExpandMaster = false;
			allSpell = String.Empty;

			SearchParametersScript = String.Empty;
			needSetSearchParas = false;
			SetUrlCityScript = String.Empty;
			needSetUrlCity = false;

			//获取参数
			GetParameter();

			//取基础数据
			tagDic = TagData.GetTagDataDictionary();
			if (!tagDic.ContainsKey(tagType))
				return;

			dataCollection = DataNode.GetDataCollection(tagType);
			htmlList = new List<string>();

			if (expand == 1)
			{
				DataNode dataNode = dataCollection.GetDataNodeById(objId);
				if (dataNode != null)
				{
					MakeTreeHtmlByMasterbrand(dataNode);
				}
				Response.Write("var masterContent='");
				Response.Write(String.Concat(htmlList.ToArray()));
				Response.Write("';");
				//Response.End();
			}
			else
			{
				//取各品牌层次ID
				GetMasterToSerialIds();
				MakeLeftTreeHtml();

				/*不再传递选车参数20120517,by ddl
				//MakeSetSearchParasScript();
				 * */

				MakeSetTagCityUrlScript();
			}
		}

		private void GetParameter()
		{
			tagType = BitAuto.Utils.StringHelper.XSSFilter(string.IsNullOrEmpty(Request.QueryString["tagtype"]) ? "" : Request.QueryString["tagtype"]);
			pageType = BitAuto.Utils.StringHelper.XSSFilter(string.IsNullOrEmpty(Request.QueryString["pagetype"]) ? "" : Request.QueryString["pagetype"]);
			cityCode = BitAuto.Utils.StringHelper.XSSFilter(string.IsNullOrEmpty(Request.QueryString["cityCode"]) ? "" :Request.QueryString["cityCode"]);
			keyWord = BitAuto.Utils.StringHelper.XSSFilter(string.IsNullOrEmpty(Request.QueryString["keyword"]) ? "" :Request.QueryString["keyword"]);
			showType = BitAuto.Utils.StringHelper.XSSFilter(string.IsNullOrEmpty(Request.QueryString["showtype"]) ? "" :Request.QueryString["showtype"]);
			customBaseUrl = BitAuto.Utils.StringHelper.XSSFilter(string.IsNullOrEmpty(Request.QueryString["baseurl"]) ? "" : Request.QueryString["baseurl"]);
			objId = ConvertHelper.GetInteger(Request.QueryString["objid"]);
			cityId = BitAuto.Utils.StringHelper.XSSFilter(string.IsNullOrEmpty(Request.QueryString["cityid"]) ? "" : Request.QueryString["cityid"]);
			expand = ConvertHelper.GetInteger(Request.QueryString["expand"]);

			if (tagType == null)
				tagType = String.Empty;
			else
				tagType = tagType.Trim().ToLower();

			if (String.IsNullOrEmpty(pageType))
				pageType = "home";
			else
				pageType = pageType.Trim().ToLower();

			if (cityCode == null)
				cityCode = String.Empty;
			else
				cityCode = cityCode.Trim().ToLower();

			if (cityId == null)
				cityId = String.Empty;
			if (customBaseUrl == null) customBaseUrl = String.Empty;
			if (showType == null) showType = String.Empty;


			if (keyWord == null) keyWord = String.Empty;
		}

		/// <summary>
		/// 获取主品牌，品牌，子品牌，各层级的ID
		/// </summary>
		private void GetMasterToSerialIds()
		{
			if (objId == 0)
				return;
			switch (pageType)
			{
				case "masterbrand":
					MasterBrandEntity masterEntity = (MasterBrandEntity)DataManager.GetDataEntity(EntityType.MasterBrand, objId);
					if (masterEntity != null)
					{
						masterId = objId;
						allSpell = masterEntity.AllSpell;
					}
					break;
				case "brand":
					BrandEntity brandEntity = (BrandEntity)DataManager.GetDataEntity(EntityType.Brand, objId);
					if (brandEntity != null)
					{
						brandId = brandEntity.Id;
						masterId = brandEntity.MasterBrand.Id;
						allSpell = brandEntity.AllSpell;
					}
					break;
				case "serial":
					SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, objId);
					if (serialEntity != null)
					{
						serialId = serialEntity.Id;
						brandId = serialEntity.Brand.Id;
						masterId = serialEntity.Brand.MasterBrand.Id;
						allSpell = serialEntity.AllSpell;
					}
					break;
			}
		}

		/// <summary>
		/// 生成树形的HTML
		/// </summary>
		private void MakeLeftTreeHtml()
		{
			MakeCharNav();
			MakeTreeHtml();
			treeHtml = String.Concat(htmlList.ToArray());
		}

		/// <summary>
		/// 生成字符导航
		/// </summary>
		/// <param name="htmlList"></param>
		private void MakeCharNav()
		{
			dataIndex = 0;
			htmlList.Add("<div class=\"treeSubNavv1\"><ul id=\"tree_letter\">");
			for (int i = 0; i < charString.Length; i++)
			{
				char curChar = charString[i];
				char lowerCurChar = Char.ToLower(curChar);
				if (dataCollection.ContainsChar(curChar))
					htmlList.Add("<li  class=\"t-" + lowerCurChar + "\"><a href=\"#\" onclick=\"treeHref(" + (i + 1) + ");return false;\">" + curChar + "</a></li>");
				else
					htmlList.Add("<li class=\"none t-" + lowerCurChar + "\">" + curChar + "</li>");
			}
			htmlList.Add("</ul></div>");
		}

		/// <summary>
		/// 生成树内的数据HTML
		/// </summary>
		/// <param name="htmlList"></param>
		/// <param name="dataCollection"></param>
		private void MakeTreeHtml()
		{
			htmlList.Add("<div class=\"treev1\" id=\"treev1\"><ul>");
			for (int i = 0; i < charString.Length; i++)
			{
				char curChar = charString[i];
				if (!dataCollection.ContainsChar(curChar))
					continue;
				htmlList.Add("<li class=\"root\" id=\"letter" + (i + 1) + "\"><b>" + curChar + "</b>");
				htmlList.Add("<ul class=\"tree-item-box\">");
				MakeTreeHtmlByChar(curChar);
				htmlList.Add("</ul></li>");
			}
			htmlList.Add("<li id=\"tree-bottom\" style=\"height:300px; overflow:hidden\"></li>");
			htmlList.Add("</ul></div>");
		}

		//按字母生成HTML
		private void MakeTreeHtmlByChar(char curChar)
		{
			if (tagDic[tagType].UrlDictionary["masterbrand"].UrlRule.Length == 0)
				needExpandMaster = true;

			for (int i = dataIndex; i < dataCollection.Count; i++)
			{
				DataNode dataNode = dataCollection[i];
				if (dataNode.FirstChar != curChar)
				{
					dataIndex = i;
					break;
				}
				//主品牌Url
				string url = GetTagUrlInBar(dataNode, "masterbrand");
				string className = "mainBrand";
				string curIdStr = String.Empty;
				if (dataNode.Id == masterId)
				{
					if (pageType == "masterbrand")
					{
						className = "mainBrand current current_unfold";
						curIdStr = " id=\"curObjTreeNode\"";
					}
					else
					{
						className = "mainBrand current_unfold";
					}

				}
				if (needExpandMaster)
				{
					htmlList.Add("<li" + curIdStr + "><a href=\"#\" onclick=\"expandMaster(this," + dataNode.Id + ");return false;\" class=\"" + className + "\"><big>" + dataNode.Name + "</big><span>(" + dataNode.Count + ")</span></a>");
				}
				else
					htmlList.Add("<li" + curIdStr + "><a href=\"" + url + "\" class=\"" + className + "\"><big>" + dataNode.Name + "</big><span>(" + dataNode.Count + ")</span></a>");

				//只有在当前主品牌时才展开
				if (dataNode.Id == masterId)
					MakeTreeHtmlByMasterbrand(dataNode);

				htmlList.Add("</li>");
			}
		}

		/// <summary>
		/// 按主品牌生成HTML
		/// </summary>
		/// <param name="dataNode"></param>
		private void MakeTreeHtmlByMasterbrand(DataNode dataNode)
		{
			htmlList.Add("<ul class=\"tree-items\">");
			//每个品牌
			foreach (DataNode childNode in dataNode.ChildNodeList)
			{
				//计算样式名
				string className = String.Empty;
				if (childNode.BrandType == "brand")
				{
					className = "brandType";
					string curIdStr = String.Empty;
					if (childNode.Id == brandId)
					{
						//如果是子品牌页面，且此子品牌不在此树中，选中该品牌
						//|| (pageType == "serial" && childNode.ChildNodeList != null && childNode.ChildNodeList.GetDataNodeById(serialId) == null)
						if (pageType == "brand")
						{
							className += " current";
							curIdStr = " id=\"curObjTreeNode\"";
						}
					}

					string url = GetTagUrlInBar(childNode, "brand");
					if (String.IsNullOrEmpty(url))
						htmlList.Add("<li" + curIdStr + "><a class=\"" + className + "\"><big>" + childNode.Name + "</big><span>(" + childNode.Count + ")</span></a>");
					else
						htmlList.Add("<li" + curIdStr + "><a href=\"" + url + "\" class=\"" + className + "\"><big>" + childNode.Name + "</big><span>(" + childNode.Count + ")</span></a>");
					//生成子品牌HTML
					if (childNode.ChildNodeList != null)
					{
						htmlList.Add("<ul>");
						foreach (DataNode serialNode in childNode.ChildNodeList)
							MakeSerialTreeHtml(serialNode);
						htmlList.Add("</ul>");
					}
					htmlList.Add("</li>");
				}
				else
					MakeSerialTreeHtml(childNode);
			}
			htmlList.Add("</ul>");
		}

		/// <summary>
		/// 生成子品牌的HTML
		/// </summary>
		/// <param name="dataNode"></param>
		private void MakeSerialTreeHtml(DataNode dataNode)
		{
			string className = "subBrand";
			string curIdStr = String.Empty;
			if (dataNode.Id == serialId)
			{
				className += " current";
				curIdStr = " id=\"curObjTreeNode\"";
			}
			string serialName = dataNode.Name;
			if (dataNode.Id == 1568)
			{
				serialName = "索纳塔八";
			}
			string url = GetTagUrlInBar(dataNode, "serial");
			if (tagType == "chexing")
			{
				if (dataNode.SaleState == "停销")
					htmlList.Add("<li" + curIdStr + " class=\"saleoff\">");
				else
					htmlList.Add("<li" + curIdStr + ">");
				htmlList.Add("<a href=\"" + url + "\" class=\"" + className + "\"><big>" + serialName + "</big>");
				if (!String.IsNullOrEmpty(dataNode.Subsidies))
					htmlList.Add("<span class=\"green\">补贴</span>");
				htmlList.Add("</a>");
			}
			else if (tagType == "yanghu" || tagType == "zhishu" || tagType == "xiaoliang")
				htmlList.Add("<li" + curIdStr + "><a href=\"" + url + "\" class=\"" + className + "\"><big>" + serialName + "</big></a>");
			else
				htmlList.Add("<li" + curIdStr + "><a href=\"" + url + "\" class=\"" + className + "\"><big>" + serialName + "</big><span>(" + dataNode.Count + ")</span></a>");
			htmlList.Add("</li>");
		}

		/// <summary>
		/// 获取URL
		/// </summary>
		/// <param name="dataNode"></param>
		/// <returns></returns>
		private string GetTagUrlInBar(DataNode dataNode, string brandType)
		{
			TagData tag = tagDic[tagType];
			string url = String.Empty;
			if (tag.UrlDictionary.ContainsKey(brandType))
			{
				TagUrl tagUrl = tag.UrlDictionary[brandType];
				if (!String.IsNullOrEmpty(tagUrl.UrlRule))
				{
					url = tagUrl.UrlRule.Replace("@objid@", dataNode.Id.ToString());
					url = url.Replace("@objspell@", dataNode.AllSpell);
					url = url.Replace("@cityid@", cityId);
					url = url.Replace("@citycode@", cityCode);
					url = url.Replace("@keyword@", keyWord);
					if (!String.IsNullOrEmpty(tagUrl.OtherParas))
					{
						//附加参数检查，如果有一个有值，则应全部替换掉
						bool hasOtherParaValue = false;
						string otherParasStr = tagUrl.OtherParas;
						foreach (string paraName in tagUrl.OtherParaNames)
						{
							switch (paraName)
							{
								case "cityid":
									if (!String.IsNullOrEmpty(cityId))
										hasOtherParaValue = true;
									otherParasStr = otherParasStr.Replace("@cityid@", cityId);
									break;
								case "citycode":
									if (!String.IsNullOrEmpty(cityCode))
										hasOtherParaValue = true;
									otherParasStr = otherParasStr.Replace("@citycode@", cityCode);
									break;
								case "showtype":
									if (!String.IsNullOrEmpty(showType))
										hasOtherParaValue = true;
									otherParasStr = otherParasStr.Replace("@showtype@", showType);
									break;
							}
						}
						if (hasOtherParaValue)
							url += otherParasStr;
					}
					if (!string.IsNullOrEmpty(tagUrl.EndString))
						url += tagUrl.EndString;
					if (customBaseUrl.Length > 0)
						url = customBaseUrl + url;
				}
			}
			//return tag.BaseUrl + url;
			return url;
		}

		/// <summary>
		/// 设置选车参数URL和Cookie
		/// </summary>
		private void MakeSetSearchParasScript()
		{
			if (pageType != "search")
				return;
			string[] serchTags = new string[] { "chexing", "shipin", "baojia", "tupian", "koubei", "hangqing", "daogou", "pingce", "tujie", "keji", "zhishu", "anquan", "yanghu" };
			foreach (string tagName in serchTags)
			{
				if (tagName == tagType)
				{
					needSetSearchParas = true;
					break;
				}
			}

			if (!needSetSearchParas)
				return;

			List<string> scriptCode = new List<string>();
			scriptCode.Add("function SetTagSearchUrl()\r\n");
			scriptCode.Add("{\r\n");
			foreach (string tagName in serchTags)
			{
				if (tagName == tagType)
					scriptCode.Insert(0, "setSearchCookie();\r\n");
				else
				{
					string tagBaseUrl = customBaseUrl.Length == 0 ? tagDic[tagName].BaseUrl : customBaseUrl;
					string tagSearchUrl = tagBaseUrl + tagDic[tagName].UrlDictionary[pageType].UrlRule;
					scriptCode.Add("setSearchUrl('" + tagName + "','" + tagSearchUrl + "');\r\n");
				}
			}
			scriptCode.Add("}\r\n");
			scriptCode.Add("addLoadEvent(SetTagSearchUrl);\r\n");
			SearchParametersScript = String.Concat(scriptCode.ToArray());
		}

		/// <summary>
		/// 设置支持标签间传递城市的Url的脚本
		/// </summary>
		private void MakeSetTagCityUrlScript()
		{
			if (pageType == "search")
				return;
			List<string> cityTags = new List<string>(new string[] { "baojia", "jingxiaoshang", "yanghu" });

			if (!cityTags.Contains(tagType))
				return;

			List<string> scriptCode = new List<string>();
			scriptCode.Add("function SetUrlCity()\r\n");
			scriptCode.Add("{\r\n");
			foreach (string tagName in cityTags)
			{
				if (!tagDic.ContainsKey(tagName))
					continue;
				if (!tagDic[tagName].UrlDictionary.ContainsKey(pageType))
					break;
				if (tagName == tagType)
					needSetUrlCity = true;
				else
				{
					string tagUrl = tagDic[tagName].BaseUrl + tagDic[tagName].UrlDictionary[pageType].UrlRule;
					string otherPara = tagDic[tagName].UrlDictionary[pageType].OtherParas;
					/*
					if (tagName == "jingxiaoshang" && pageType == "masterbrand")
					{
						string urlType = "brand";
						DataNode masterNode = dataCollection.GetDataNodeById(objId);
						if (masterNode != null && masterNode.ChildNodeList != null)
						{
							DataNode brandNode = masterNode.ChildNodeList[0];
							tagUrl = tagDic[tagName].BaseUrl + tagDic[tagName].UrlDictionary[urlType].UrlRule;
							tagUrl = tagUrl.Replace("@objspell@", brandNode.AllSpell);
							otherPara = tagDic[tagName].UrlDictionary[urlType].OtherParas;
						}
					}
					*/
					scriptCode.Add("SetTagUrlCity('" + tagName + "','" + tagUrl + "','" + otherPara + "');\r\n");
				}
			}

			if (!needSetUrlCity)
				return;

			scriptCode.Add("}\r\n");
			scriptCode.Add("SetUrlCity();\r\n");
			SetUrlCityScript = String.Concat(scriptCode.ToArray());
		}
	}
}