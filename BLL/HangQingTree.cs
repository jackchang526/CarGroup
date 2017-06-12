using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
using System.Web.Caching;

using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.BLL
{
	public class HangQingTree : TreeData
	{
		private string _TagType = "hangqing";
		private string _CityNewsNumberFile = Path.Combine(WebConfig.DataBlockPath, "Data\\City\\cityandprovincenewsnumber.xml");
		private string _CityAndProvinceRelationMap = Path.Combine(WebConfig.DataBlockPath, "Data\\City\\needrelationmap.xml");
		private string _CityFilePath = Path.Combine(WebConfig.DataBlockPath, "Data\\City\\hangqing\\citynews\\{0}.xml");
		private string _ProvinceFilePath = Path.Combine(WebConfig.DataBlockPath, "Data\\City\\hangqing\\provincenews\\{0}.xml");

		private string _HangQingDealer = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\HangQingDealer\\{0}_HangQingDealerAllCity.xml");
		//public List<int> _Municipalities = new List<int>();
		public Dictionary<int, int> _Municipalities = new Dictionary<int, int>();

		// 所有子品牌行情价
		// CMS接口 http://api.admin.bitauto.com/api/list/marketprice.aspx (周新锋)
		private string _HangQingAllSerialPrice = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\HangQingAllSerialPrice.xml");

		// 所有车型行情价
		// CMS接口 http://api.admin.bitauto.com/api/list/marketprice.aspx?serial=true (李东)
		// modified by chengl Apr.21.2014
		private string _HangQingAllCarPrice = Path.Combine(WebConfig.DataBlockPath, "Data\\AllCarHangQingPrice.xml");

		/// <summary>
		/// 构造函数
		/// </summary>
		public HangQingTree()
		{
			_Municipalities.Add(0, 0);//全国
			_Municipalities.Add(2, 201);//北京
			_Municipalities.Add(24, 2401);//上海
			_Municipalities.Add(26, 2601);//天津
			_Municipalities.Add(31, 3101);//重庆
		}

		public string TreeXmlData()
		{
			XmlDocument xmlDoc = AutoStorageService.GetCacheTreeXml();

			XmlNodeList xNodeList = xmlDoc.SelectNodes("data/master");
			XmlElement xElem = (XmlElement)xmlDoc.SelectSingleNode("data");
			if (xmlDoc == null) return "";
			StringBuilder xmlString = new StringBuilder("");
			StringBuilder masterInData = new StringBuilder("");
			string masterElementString = "<master id=\"{0}\" name=\"{1}\" countnum=\"{2}\" firstchar=\"{3}\" extra=\"{4}\">";
			string brandElementString = "<brand id=\"{0}\" name=\"{1}\" countnum=\"{2}\" extra=\"{3}\">";
			string serialElementString = "<serial id=\"{0}\" name=\"{1}\" countnum=\"{2}\" extra=\"{3}\"/>";
			int newsTotalCount = 0;

			foreach (XmlElement masterElement in xNodeList)
			{
				int masterBrandNewsCount = 0;
				StringBuilder brandInMasterBrand = new StringBuilder();
				if (masterElement.ChildNodes.Count < 1) continue;

				XmlElement xElemEntity = (XmlElement)masterElement.ChildNodes[0];
				if (xElemEntity.Name.ToLower() == "brand")
				{

					foreach (XmlElement brandElement in masterElement.ChildNodes)
					{
						int BrandNewsCount = 0;
						StringBuilder SerialInBrand = new StringBuilder();

						foreach (XmlElement serialElement in brandElement.ChildNodes)
						{
							int serialNewsCount = GetSerialId(Convert.ToInt32(serialElement.GetAttribute("id")));
							if (serialNewsCount == 0) continue;
							SerialInBrand.AppendFormat(serialElementString
												, serialElement.GetAttribute("id")
												, serialElement.GetAttribute("name")
												, serialNewsCount.ToString()
												, serialElement.GetAttribute("extra"));

							BrandNewsCount += serialNewsCount;
						}
						if (BrandNewsCount == 0) continue;
						brandInMasterBrand.AppendFormat(brandElementString
												, brandElement.GetAttribute("id")
												, brandElement.GetAttribute("name")
												, BrandNewsCount.ToString()
												, brandElement.GetAttribute("extra"));
						brandInMasterBrand.Append(SerialInBrand);
						brandInMasterBrand.Append("</brand>");

						masterBrandNewsCount += BrandNewsCount;
					}

				}
				else
				{
					foreach (XmlElement serialElement in masterElement.ChildNodes)
					{
						int serialNewsCount = GetSerialId(Convert.ToInt32(serialElement.GetAttribute("id")));
						if (serialNewsCount == 0) continue;

						masterBrandNewsCount += serialNewsCount;
						brandInMasterBrand.AppendFormat(serialElementString
												, serialElement.GetAttribute("id")
												, serialElement.GetAttribute("name")
												, serialNewsCount.ToString()
												, serialElement.GetAttribute("extra"));
					}
				}
				//给主品牌赋值
				newsTotalCount += masterBrandNewsCount;
				if (masterBrandNewsCount == 0) continue;

				masterInData.AppendFormat(masterElementString
									, masterElement.GetAttribute("id")
									, masterElement.GetAttribute("name")
									, masterBrandNewsCount.ToString()
									, masterElement.GetAttribute("firstchar")
									, masterElement.GetAttribute("extra"));
				masterInData.Append(brandInMasterBrand);
				masterInData.Append("</master>");
			}

			xmlString.AppendFormat("<data id=\"0\" countnum=\"{0}\">", newsTotalCount.ToString());
			xmlString.Append(masterInData);
			xmlString.Append("</data>");

			return xmlString.ToString();
		}
		/// <summary>
		/// 得到主品牌的行情文章数量
		/// </summary>
		/// <param name="masterBrandId"></param>
		/// <returns></returns>
		public int GetMasterBrandId(int masterBrandId) { return 0; }
		/// <summary>
		/// 得到品牌的行情文章数量
		/// </summary>
		/// <param name="brandId"></param>
		/// <returns></returns>
		public int GetBrandId(int brandId) { return 0; }
		/// <summary>
		/// 得到子品牌的行情文章数量
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public int GetSerialId(int serialId)
		{
			Dictionary<string, Dictionary<int, int>> daoGouNum = AutoStorageService.GetCacheTreeSerialNewsCount();
			if (daoGouNum == null
				|| !daoGouNum.ContainsKey(_TagType)
				|| !daoGouNum[_TagType].ContainsKey(serialId))
			{
				return 0;
			}

			return daoGouNum[_TagType][serialId];
		}
		/// <summary>
		/// 得到焦点图字符串
		/// </summary>
		/// <returns></returns>
		public string GetForcusImageArea() { return ""; }
		/// <summary>
		/// 得到焦点新闻字符串
		/// </summary>
		/// <returns></returns>
		public string GetForcusNewsAree() { return ""; }
		/// <summary>
		/// 得到子品牌新闻的数据
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public DataSet GetNewsListBySerialId(int serialId, int year)
		{
			DataSet newsDs = new DataSet();
			try
			{
				string xmlFile = "";
				if (year == 0)
					xmlFile = Path.Combine(WebConfig.DataBlockPath
							, string.Format("Data\\SerialNews\\{0}\\Serial_All_News_{1}.xml"
							, _TagType
							, serialId.ToString()));
				else
					xmlFile = Path.Combine(WebConfig.DataBlockPath
							, string.Format("Data\\SerialNews\\{0}\\Serial_All_News_{1}_{2}.xml"
							, _TagType
							, serialId.ToString()
							, year.ToString()));
				newsDs.ReadXml(xmlFile);
			}
			catch
			{ }
			return newsDs;
		}
		/// <summary>
		/// 得到子品牌新闻数量通过子品牌ID和城市还有省份ID
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="isProvinceOrCity"></param>
		/// <param name="objectId"></param>
		/// <returns></returns>
		public int GetSerialNewsNumberByIDAndCityId(int serialId, bool isProvinceOrCity, int objectId)
		{
			string filePath = Path.Combine(WebConfig.DataBlockPath
				, string.Format("Data\\SerialNews\\hangqing\\Serial_All_News_CityNum_{0}.xml", serialId));

			if (!File.Exists(filePath)) return 0;
			if (objectId < 0) return 0;
			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(filePath);

				string xPath = string.Empty;
				// modified by chengl Nov.4.2011
				//if (isProvinceOrCity && objectId != 0)
				//{
				//    xPath = string.Format("root/Province/City[@ID={0}]", objectId);
				//}
				//else
				//{
				//    xPath = string.Format("root/Province[@ID={0}]", objectId);
				//}
				xPath = string.Format("root/City[@id='{0}']", objectId);
				XmlNode xNode = xmlDoc.SelectSingleNode(xPath);
				if (xNode == null) return 0;
				if (xNode.Attributes["newscount"] == null) return 0;
				return ConvertHelper.GetInteger(((XmlElement)xNode).GetAttribute("newscount"));
				// return ConvertHelper.GetInteger(((XmlElement)xNode).GetAttribute(_TagType));
			}
			catch
			{
				return 0;
			}
		}
		public List<Car_MasterBrandEntity> GetHotMasterBrandEntityList(int entityCount) { return null; }
		public List<Car_SerialBaseEntity> GetHotSerailEntityList(int entityCount) { return null; }
		/// <summary>
		/// 得到省份新闻数量
		/// </summary>
		/// <returns></returns>
		public List<XmlElement> GetProvinceNewsNumber()
		{
			string cacheKey = "defaultHangQingProvinceNewsNumber";
			//先得到对象里面的值
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (List<XmlElement>)obj;
			//如果城市文件存在
			if (!File.Exists(_CityNewsNumberFile)) return null;

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(_CityNewsNumberFile);
				//如果装载文件失败
				if (xmlDoc == null) return null;
				List<XmlElement> provList = new List<XmlElement>();

				//如果是直辖市
				foreach (KeyValuePair<int, int> entity in _Municipalities)
				{
					XmlNode provNode = xmlDoc.SelectSingleNode("root/Province[@ID=" + entity.Key + "]");
					if (provNode == null) continue;
					//城市列表
					provList.Add((XmlElement)provNode);

				}
				//得到所有省份的列表
				XmlNodeList xProvNodeList = xmlDoc.SelectNodes("root/Province");
				CacheDependency cd = new CacheDependency(_CityNewsNumberFile);
				if (xProvNodeList == null || xProvNodeList.Count < 1)
				{
					CacheManager.InsertCache(cacheKey, provList, cd, DateTime.Now.AddMinutes(10));
					return provList;
				}
				List<XmlElement> initProvList = new List<XmlElement>();
				//初始省份初始数组用于排序
				foreach (XmlElement entity in xProvNodeList)
				{
					int provId = ConvertHelper.GetInteger(entity.GetAttribute("ID"));
					if (_Municipalities.ContainsKey(provId)) continue;
					initProvList.Add(entity);
				}
				//对城市列表进行排序
				initProvList.Sort(NodeCompare.CompareProvinceOrder);
				//对返回结果列表，进行赋值
				foreach (XmlElement xElem in initProvList)
				{
					provList.Add(xElem);
				}

				CacheManager.InsertCache(cacheKey, provList, cd, DateTime.Now.AddMinutes(10));
				return provList;
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// 得到省份和城市的关系
		/// </summary>
		/// <returns></returns>
		public XmlDocument GetProvinceAndCityRelation()
		{
			if (!File.Exists(_CityNewsNumberFile)) return null;
			//城市和省份对应关系图
			string cacheKey = "cityandprovincerelationmap";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (XmlDocument)obj;

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(_CityNewsNumberFile);

				XmlNodeList xNodeList = xmlDoc != null ? xmlDoc.SelectNodes("root/Province") : null;
				if (xNodeList == null || xNodeList.Count < 1) return null;

				CacheDependency cd = new CacheDependency(_CityAndProvinceRelationMap);
				CacheManager.InsertCache(cacheKey, xmlDoc, cd, DateTime.Now.AddMinutes(5));

				return xmlDoc;
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// 得到省份和城市的关系
		/// </summary>
		/// <returns></returns>
		public XmlDocument GetProvinceAndCityRelationXmlDocument()
		{
			if (!File.Exists(_CityAndProvinceRelationMap)) return null;
			//城市和省份对应关系图
			string cacheKey = "getprovinceandcityrelationxmldocument";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (XmlDocument)obj;

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(_CityAndProvinceRelationMap);

				XmlNodeList xNodeList = xmlDoc != null ? xmlDoc.SelectNodes("root/Province") : null;
				if (xNodeList == null || xNodeList.Count < 1) return null;

				CacheDependency cd = new CacheDependency(_CityAndProvinceRelationMap);
				CacheManager.InsertCache(cacheKey, xmlDoc, cd, DateTime.Now.AddMinutes(5));

				return xmlDoc;
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// 得到新闻的XML列表
		/// </summary>
		/// <param name="isProvinceOrCity">是省份还是城市：如果为省份则为false,城市为true</param>
		/// <param name="objectId">对象ID</param>
		/// <param name="pageIndex">当前页面</param>
		/// <param name="pagesize">当前页面大小</param>
		/// <returns></returns>
		public XmlDocument GetNewsXmlDocument(bool isProvinceOrCity, int objectId, int pageIndex, int pagesize)
		{
			//得到文件地址            
			string cacheKey = (isProvinceOrCity ? "CityNewsList" : "ProvinceNewsList") + "_" + objectId + "_" + pageIndex + "_" + pagesize;
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (XmlDocument)obj;
			//得到文件地址
			string filePath = string.Format(isProvinceOrCity ? _CityFilePath : _ProvinceFilePath, objectId);
			if (!File.Exists(filePath)) return null;
			XmlDocument xmlDoc = new XmlDocument();

			if (pageIndex >= 50)
			{
				xmlDoc = GetFiftyPageNewsList(isProvinceOrCity, objectId, pageIndex, pagesize);
			}
			else
			{

				int startindex = pageIndex * pagesize;
				xmlDoc = CommonFunction.GetNewsList(pagesize, startindex, filePath);
			}
			if (xmlDoc == null) return null;
			CacheDependency cd = new CacheDependency(filePath);
			CacheManager.InsertCache(cacheKey, xmlDoc, cd, DateTime.Now.AddMinutes(5));
			return xmlDoc;
		}
		/// <summary>
		/// 得到50页以后的数据
		/// </summary>
		/// <param name="isProvinceOrCity">是省份还是城市：如果为省份则为false,城市为true</param>
		/// <param name="objectId">对象ID</param>
		/// <param name="pageIndex">当前页面</param>
		/// <param name="pagesize">当前页面大小</param>
		/// <returns></returns>
		private XmlDocument GetFiftyPageNewsList(bool isProvinceOrCity, int objectId, int pageIndex, int pagesize)
		{
			//链接参数
			string urlParam = "?pageindex={0}&pagesize={1}&nonewstype=2&ismain=1&categoryId=3&{2}";
			string cityParam = isProvinceOrCity ? string.Format("cityid={0}", objectId) : string.Format("provinceid={0}", objectId);

			string url = WebConfig.NewsRequestUrl + string.Format(urlParam, pageIndex + 1, pagesize, cityParam);

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(url);
				if (xmlDoc == null) return null;
				//得到新闻列表
				XmlNodeList litsNews = xmlDoc.SelectNodes("NewDataSet/listNews");
				if (litsNews == null || litsNews.Count < 1) return null;

				StringBuilder content = new StringBuilder();

				foreach (XmlElement entity in litsNews)
				{
					content.AppendFormat("<listnews>{0}</listnews>", entity.InnerXml);
				}

				if (string.IsNullOrEmpty(content.ToString())) return null;

				XmlDocument newsXml = new XmlDocument();
				newsXml.LoadXml("<root>" + content.ToString() + "</root>");
				return newsXml;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// 取子品牌 莫个城市的新闻经销商(朱永旭)
		/// http://api.admin.bitauto.com/api/list/MarketNewsToCar.aspx?typeId=2&csid=1991
		/// </summary>
		/// <param name="csid">子品牌ID</param>
		/// <param name="city">城市ID</param>
		/// <param name="top">取数据条数</param>
		/// <returns></returns>
		public List<XmlElement> GetSerialHangQingDealer(int csid, int city, int top)
		{
			if (top < 1 || top > 100)
			{ top = 5; }
			List<XmlElement> listXE = new List<XmlElement>();
			if (File.Exists(string.Format(_HangQingDealer, csid)))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(string.Format(_HangQingDealer, csid));
				XmlNodeList xnl = null;
				if (city > 0)
				{
					// 有城市信息
					xnl = doc.SelectNodes("/MarketPrices/MarketPrice[CityId='" + city.ToString() + "']");
				}
				else
				{
					// 无城市信息
					xnl = doc.SelectNodes("/MarketPrices/MarketPrice");
				}
				if (xnl != null && xnl.Count > 0)
				{
					foreach (XmlElement xe in xnl)
					{
						if (!listXE.Contains(xe))
						{
							listXE.Add(xe);
							if (listXE.Count >= top)
							{ break; }
						}
					}
				}
			}
			return listXE;
		}

		/// <summary>
		/// 取所有子品牌行情价(周新锋)
		/// http://api.admin.bitauto.com/api/list/marketprice.aspx
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, string> GetAllSerialHangQingPrice()
		{
			string cacheKey = "HangQingTree_GetAllSerialHangQingPrice";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (Dictionary<int, string>)obj;

			Dictionary<int, string> dic = new Dictionary<int, string>();
			if (File.Exists(_HangQingAllSerialPrice))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(_HangQingAllSerialPrice);
				XmlNodeList xnl = doc.SelectNodes("/NewDataSet/Table");
				if (xnl != null && xnl.Count > 0)
				{
					foreach (XmlNode xn in xnl)
					{
						int csid = 0;
						decimal minP = 0;
						decimal maxP = 0;
						if (int.TryParse(xn.SelectSingleNode("CsBrandId").InnerText, out csid))
						{ }
						if (decimal.TryParse(xn.SelectSingleNode("minPrice").InnerText, out minP))
						{ }
						if (decimal.TryParse(xn.SelectSingleNode("maxPrice").InnerText, out maxP))
						{ }
						if (csid > 0 && minP > 0 && maxP > 0 && !dic.ContainsKey(csid))
						{
							dic.Add(csid, minP.ToString("F2") + "万-" + maxP.ToString("F2") + "万");
						}
					}
				}
			}
			CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
			return dic;
		}

		/// <summary>
		/// 取所有车型行情价(李东)
		/// http://api.admin.bitauto.com/api/list/marketprice.aspx?serial=true
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, string> GetAllCarHangQingPrice()
		{
			string cacheKey = "HangQingTree_GetAllCarHangQingPrice";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (Dictionary<int, string>)obj;

			Dictionary<int, string> dic = new Dictionary<int, string>();
			if (File.Exists(_HangQingAllCarPrice))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(_HangQingAllCarPrice);
				XmlNodeList xnl = doc.SelectNodes("/NewDataSet/Table");
				if (xnl != null && xnl.Count > 0)
				{
					foreach (XmlNode xn in xnl)
					{
						int carid = 0;
						decimal minP = 0;
						decimal maxP = 0;
						if (int.TryParse(xn.SelectSingleNode("CarTypeId").InnerText, out carid))
						{ }
						if (decimal.TryParse(xn.SelectSingleNode("minPrice").InnerText, out minP))
						{ }
						if (decimal.TryParse(xn.SelectSingleNode("maxPrice").InnerText, out maxP))
						{ }
						if (carid > 0 && minP > 0 && maxP > 0 && !dic.ContainsKey(carid))
						{
							dic.Add(carid, minP.ToString("F2") + "万-" + maxP.ToString("F2") + "万");
						}
					}
				}
			}
			CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
			return dic;
		}

	}
}
