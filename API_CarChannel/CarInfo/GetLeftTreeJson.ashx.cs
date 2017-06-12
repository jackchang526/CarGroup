using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL.CarTreeData;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using System.Text;
using System.Web.UI;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetLeftTreeJson 的摘要说明
	/// </summary>
	public class GetLeftTreeJson : IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;

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
		protected string carIds;//传输车型
		private string callback = string.Empty;

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


		public void ProcessRequest(HttpContext context)
		{
			OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
			{
				Duration = 60 * 10,
				Location = OutputCacheLocation.Any,
				VaryByParam = "*"
			});
			page.ProcessRequest(HttpContext.Current);

			context.Response.ContentType = "application/x-javascript";
			response = context.Response;
			request = context.Request;
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

			dataCollection = DataNode.GetDataCollectionFromUrl(tagType);//从url里面读取
			if (dataCollection == null)
			{
				dataCollection = DataNode.GetDataCollection(tagType);//从本地文件读取
			}
			htmlList = new List<string>();

			if (expand == 1)
			{
				DataNode dataNode = dataCollection.GetDataNodeById(objId);
				string jsonMaster = string.Empty;
				if (dataNode != null)
				{
					jsonMaster = MakeTreeHtmlByMasterbrand(dataNode);
				}
				response.Write("var masterContent=[");
				response.Write(jsonMaster);
				response.Write("];");
				//Response.End();
			}
			else
			{
				//取各品牌层次ID
				GetMasterToSerialIds();
				MakeLeftTreeHtml();
			}

		}

		private void GetParameter()
		{
			tagType = request.QueryString["tagtype"];
			pageType = request.QueryString["pagetype"];
			cityCode = request.QueryString["cityCode"];
			keyWord = request.QueryString["keyword"];
			showType = request.QueryString["showtype"];
			customBaseUrl = request.QueryString["baseurl"];
			objId = ConvertHelper.GetInteger(request.QueryString["objid"]);
			cityId = request.QueryString["cityid"];
			carIds = request.QueryString["carIds"];
			expand = ConvertHelper.GetInteger(request.QueryString["expand"]);
			callback = request.QueryString["callback"];
			if (string.IsNullOrEmpty(callback))
				callback = "JsonpCallBack";
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
			string jsonString = string.Empty;

			//string memCacheKey = string.Format("Car_LeftTree_Json_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}", tagType, pageType, objId, cityCode, keyWord, showType, customBaseUrl, cityId, carIds);
			//object objMemCache = MemCache.GetMemCacheByKey(memCacheKey);
			//if (objMemCache != null)
			//{
			//	jsonString = (string)objMemCache;
			//}
			//else
			{
				string navStr = MakeCharNav();
				string setCityUrlData = MakeSetTagCityUrlScript();
				string leftJson = MakeTreeHtml();
				StringBuilder sb = new StringBuilder();
				sb.Append("JsonpCallBack({");
				sb.Append(navStr);
				if (!string.IsNullOrEmpty(leftJson) || !string.IsNullOrEmpty(setCityUrlData))
				{
					sb.Append(",");
					sb.Append(leftJson);
					sb.Append(setCityUrlData);
				}
				sb.Append("})");
				jsonString = sb.ToString();
				//// 加入memcache
				//MemCache.SetMemCacheByKey(memCacheKey, jsonString, 10 * 60 * 1000);
			}
			response.Write(jsonString);
		}

		/// <summary>
		/// 生成字符导航
		/// </summary>
		/// <param name="htmlList"></param>
		private string MakeCharNav()
		{
			List<string> listTemp = new List<string>();
			for (int i = 0; i < charString.Length; i++)
			{
				char curChar = charString[i];
				char lowerCurChar = Char.ToLower(curChar);
				int isNone = dataCollection.ContainsChar(curChar) ? 1 : 0;
				listTemp.Add(string.Format("{0}:{1}", curChar, isNone));
			}
			return string.Format("char:{{{0}}}", string.Join(",", listTemp.ToArray()));
		}

		/// <summary>
		/// 生成树内的数据HTML
		/// </summary>
		/// <param name="htmlList"></param>
		/// <param name="dataCollection"></param>
		private string MakeTreeHtml()
		{
			List<string> listTemp = new List<string>();
			for (int i = 0; i < charString.Length; i++)
			{
				char curChar = charString[i];
				if (!dataCollection.ContainsChar(curChar))
					continue;
				listTemp.Add(string.Format("{0}:[{1}]", curChar, MakeTreeHtmlByChar(curChar)));
			}
			return string.Format("brand:{{{0}}}", string.Join(",", listTemp.ToArray()));
		}

		//按字母生成HTML
		private string MakeTreeHtmlByChar(char curChar)
		{
			List<string> listTemp = new List<string>();
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
				string curIdStr = String.Empty;
				int isCur = 0;
				if (dataNode.Id == masterId && pageType == "masterbrand")
				{
					isCur = 1;
				}
				string childStr = string.Empty;
				//只有在当前主品牌时才展开
				if (dataNode.Id == masterId)
				{
					childStr = string.Format(",child:[{0}]", MakeTreeHtmlByMasterbrand(dataNode));
				}
				if (needExpandMaster)
				{
					listTemp.Add(string.Format("{{type:\"mb\",name:\"{0}\",url:\"{1}\",cur:{2},expand:1,id:{5},num:{3}{4}}}", dataNode.Name, url, isCur, dataNode.Count, childStr, dataNode.Id));
				}
				else
					listTemp.Add(string.Format("{{type:\"mb\",id:{5},name:\"{0}\",url:\"{1}\",cur:{2},num:{3}{4}}}", dataNode.Name, url, isCur, dataNode.Count, childStr, dataNode.Id));
			}
			return string.Join(",", listTemp.ToArray());
		}

		/// <summary>
		/// 按主品牌生成HTML
		/// </summary>
		/// <param name="dataNode"></param>
		private string MakeTreeHtmlByMasterbrand(DataNode dataNode)
		{
			List<string> listTemp = new List<string>();
			//每个品牌
			foreach (DataNode childNode in dataNode.ChildNodeList)
			{
				//计算样式名
				string className = String.Empty;
				if (childNode.BrandType == "brand")
				{
					int isCur = 0;
					if (childNode.Id == brandId && pageType == "brand")
					{
						isCur = 1;
					}
					string url = GetTagUrlInBar(childNode, "brand");
					string childStr = string.Empty;
					//生成子品牌HTML
					if (childNode.ChildNodeList != null)
					{
						List<string> listChild = new List<string>();
						foreach (DataNode serialNode in childNode.ChildNodeList)
							listChild.Add(MakeSerialTreeHtml(serialNode));
						childStr = string.Format(",child:[{0}]", string.Join(",", listChild.ToArray()));
					}
					listTemp.Add(string.Format("{{type:\"cb\",name:\"{0}\",url:\"{1}\",cur:{2},num:{3}{4}}}", childNode.Name, url, isCur, childNode.Count, childStr));
				}
				else
					listTemp.Add(MakeSerialTreeHtml(childNode));
			}
			return string.Join(",", listTemp.ToArray());
		}

		/// <summary>
		/// 生成子品牌的HTML
		/// </summary>
		/// <param name="dataNode"></param>
		private string MakeSerialTreeHtml(DataNode dataNode)
		{
			string result = string.Empty;
			string curIdStr = String.Empty;
			int isCur = 0;
			if (dataNode.Id == serialId)
			{
				isCur = 1;
			}
			string serialName = dataNode.Name;
			if (dataNode.Id == 1568)
			{
				serialName = "索纳塔八";
			}
			string url = GetTagUrlInBar(dataNode, "serial");
			if (tagType == "chexing")
			{
				int isButie = 0;
				if (!String.IsNullOrEmpty(dataNode.Subsidies))
					isButie = 1;
				result = string.Format("{{type:\"cs\",name:\"{0}\",url:\"{1}\",butie:{2},salestate:\"{3}\",cur:{4}}}", serialName, url, isButie, dataNode.SaleState, isCur);
			}
			else if (tagType == "yanghu" || tagType == "zhishu" || tagType == "xiaoliang")
				result = string.Format("{{type:\"cs\",name:\"{0}\",url:\"{1}\",cur:{2}}}", serialName, url, isCur);
			else
				result = string.Format("{{type:\"cs\",name:\"{0}\",url:\"{1}\",num:{2},cur:{3}}}", serialName, url, dataNode.Count, isCur);
			return result;
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
								case "carids":
									if (!String.IsNullOrEmpty(carIds))
										hasOtherParaValue = true;
									otherParasStr = otherParasStr.Replace("@carids@", carIds);
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
		private string MakeSetTagCityUrlScript()
		{
			if (pageType == "search")
				return string.Empty;
			List<string> cityTags = new List<string>(new string[] { "baojia", "jingxiaoshang", "yanghu" });

			if (!cityTags.Contains(tagType))
				return string.Empty;

			List<string> listTemp = new List<string>();
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
					listTemp.Add("{tagname:\"" + tagName + "\",tagurl:\"" + tagUrl + "\",otherpara:\"" + otherPara + "\",allspell:\"" + allSpell + "\"}");
				}
			}
			return string.Format(",setcityurl:[{0}]", string.Join(",", listTemp.ToArray()));
		}
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		private sealed class OutputCachedPage : Page
		{
			private OutputCacheParameters _cacheSettings;

			public OutputCachedPage(OutputCacheParameters cacheSettings)
			{
				ID = Guid.NewGuid().ToString();
				_cacheSettings = cacheSettings;
			}

			protected override void FrameworkInitialize()
			{
				base.FrameworkInitialize();
				InitOutputCache(_cacheSettings);
			}
		}
	}
}