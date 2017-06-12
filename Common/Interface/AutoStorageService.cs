using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Timers;
using System.IO;
using System.Xml;
using System.Net;
using System.Web.Caching;
using System.Text;

using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.com.bitauto.dealer;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.cn.com.baa.api;
using BitAuto.CarChannel.Common.com.bitauto.news;
using BitAuto.Utils;


namespace BitAuto.CarChannel.Common.Interface
{
	public class AutoStorageService
	{
		#region 私有成员
		private static string autoDataFile;
		private static readonly string _MasterBrandNumberPath = "Data\\MasterBrand\\NewsNumber.xml";
		private static readonly string _BrandNumberPath = "Data\\Brand\\NewsNumber.xml";


		#endregion

		/// <summary>
		/// 静态构造函数
		/// </summary>
		static AutoStorageService()
		{
		}

		public static void Start()
		{
			// autoDataFile = WebConfig.AutoDataFile;
		}
		/// <summary>
		/// 获取车型数据
		/// </summary>
		/// <returns></returns>
		public static XmlDocument GetAutoXml()
		{
			string cachekey = "AutoData";
			XmlDocument autoXml = (XmlDocument)CacheManager.GetCachedData(cachekey);

			if (autoXml == null)
			{
				if (File.Exists(WebConfig.AutoDataFile))
				{
					autoXml = CommonFunction.ReadXmlFromFile(WebConfig.AutoDataFile);

					//add by sk 2013.04.26 增加文件读取失败，换数据源
					if (autoXml == null || !autoXml.HasChildNodes)
					{
						autoXml = CommonFunction.ReadXml(WebConfig.BaseAutoDataUrl);
					}
					CacheDependency cacheDependency = new CacheDependency(WebConfig.AutoDataFile);
					CacheManager.InsertCache(cachekey, autoXml, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				}
			}
			return autoXml;
		}

		/// <summary>
		/// 获取车型数据
		/// </summary>
		/// <returns></returns>
		public static XmlDocument GetAllAutoXml()
		{
			string cachekey = "AllAutoData";
			XmlDocument autoXml = (XmlDocument)CacheManager.GetCachedData(cachekey);

			if (autoXml == null)
			{
				string xmlPath = Path.Combine(WebConfig.DataBlockPath, "Data\\AllAutoData.xml");
				if (File.Exists(xmlPath))
				{
					autoXml = CommonFunction.ReadXmlFromFile(xmlPath);

					//add by sk 2013.04.26 增加文件读取失败，换数据源
					if (autoXml == null || !autoXml.HasChildNodes)
					{
						autoXml = CommonFunction.ReadXml(WebConfig.BaseAllAutoDataUrl);
					}
					CacheDependency cacheDependency = new CacheDependency(xmlPath);
					CacheManager.InsertCache(cachekey, autoXml, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				}
			}
			return autoXml;
		}

		/// <summary>
		/// 获取所有车型的数据，包括概念车的数据
		/// </summary>
		/// <returns></returns>
		public static XmlDocument GetAllAutoAndLevelXml()
		{
			string cachekey = "AllAutoDataAndLevelXml";
			XmlDocument autoXml = (XmlDocument)CacheManager.GetCachedData(cachekey);

			if (autoXml == null)
			{
				string xmlPath = Path.Combine(WebConfig.DataBlockPath, "Data\\MasterToBrandToSerialAllSaleAndLevel.xml");
				if (File.Exists(xmlPath))
				{
					autoXml = CommonFunction.ReadXmlFromFile(xmlPath);

					//add by sk 2013.04.26 增加文件读取失败，换数据源
					if (autoXml == null || !autoXml.HasChildNodes)
					{
						autoXml = CommonFunction.ReadXml(WebConfig.BaseAllAutoDataAndLevelUrl);
					}
					CacheDependency cacheDependency = new CacheDependency(xmlPath);
					CacheManager.InsertCache(cachekey, autoXml, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				}
			}
			return autoXml;
		}

		/// <summary>
		/// 获取图片Url字典
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, XmlElement> GetImageUrlDic()
		{
			string cacheKey = "Serial_Image_Url_Dic";
			Dictionary<int, XmlElement> sImgDic = (Dictionary<int, XmlElement>)CacheManager.GetCachedData(cacheKey);
			if (sImgDic == null)
			{
				string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\ImageUrl.xml");
				XmlDocument doc = CommonFunction.ReadXmlFromFile(xmlFile);

				XmlNodeList imgUrlList = doc.SelectNodes("/SerialList/Serial");
				sImgDic = new Dictionary<int, XmlElement>();
				foreach (XmlElement sNode in imgUrlList)
				{
					int sId = Convert.ToInt32(sNode.GetAttribute("SerialId"));
					sImgDic[sId] = sNode;
				}

				//加入缓存
				CacheDependency cacheDependency = new CacheDependency(xmlFile);
				CacheManager.InsertCache(cacheKey, sImgDic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));

			}
			return sImgDic;
		}



		/// <summary>
		/// 获取城市字典
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, City> GetCitySpellDic()
		{
			string cacheKey = "CitySpellDictionary";
			Dictionary<string, City> cityDic = (Dictionary<string, City>)CacheManager.GetCachedData(cacheKey);
			if (cityDic == null)
			{
				string cityXml = Path.Combine(WebConfig.DataBlockPath, "Data\\city.xml");
				XmlDocument cityDoc = CommonFunction.ReadXmlFromFile(cityXml);
				XmlNodeList cityList = cityDoc.SelectNodes("/CityValueSet/CityItem");
				cityDic = new Dictionary<string, City>();
				foreach (XmlElement cityNode in cityList)
				{
					int cityId = Convert.ToInt32(cityNode.SelectSingleNode("CityId").InnerText);
					string cityName = cityNode.SelectSingleNode("CityName").InnerText;
					string citySpell = cityNode.SelectSingleNode("EngName").InnerText.ToLower();
					int cityLevel = Convert.ToInt32(cityNode.SelectSingleNode("Level").InnerText);
					cityDic[citySpell] = new City(cityId, cityName, citySpell, cityLevel);
				}
				// modified by chengl Nov.12.2009
				CacheDependency cacheDependency = new CacheDependency(cityXml);
				CacheManager.InsertCache(cacheKey, cityDic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				// modified end
			}
			return cityDic;
		}

		/// <summary>
		/// 获取城市列表
		/// </summary>
		/// <returns></returns>
		public static List<City> GetCityList()
		{
			string cacheKey = "CityList_Level_1_2";
			List<City> cityList = (List<City>)CacheManager.GetCachedData(cacheKey);
			if (cityList == null)
			{
				string cityXml = Path.Combine(WebConfig.DataBlockPath, "Data\\city.xml");
				XmlDocument cityDoc = CommonFunction.ReadXmlFromFile(cityXml);
				XmlNodeList cityNodeList = cityDoc.SelectNodes("/CityValueSet/CityItem");
				cityList = new List<City>();
				foreach (XmlElement cityNode in cityNodeList)
				{
					int cityId = Convert.ToInt32(cityNode.SelectSingleNode("CityId").InnerText);
					string cityName = cityNode.SelectSingleNode("CityName").InnerText;
					string citySpell = cityNode.SelectSingleNode("EngName").InnerText.ToLower();
					int cityLevel = Convert.ToInt32(cityNode.SelectSingleNode("Level").InnerText);
					City city = new City(cityId, cityName, citySpell, cityLevel);
					cityList.Add(city);
				}
				CacheDependency cacheDependency = new CacheDependency(cityXml);
				CacheManager.InsertCache(cacheKey, cityList, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
			}
			return cityList;
		}

		/// <summary>
		/// 获取城市对应对30个中心城的对应关系
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, City> GetCityTo30Dic()
		{
			string cacheKey = "city91_to_30center";
			Dictionary<int, City> to30Dic = (Dictionary<int, City>)CacheManager.GetCachedData(cacheKey);
			if (to30Dic == null)
			{
				to30Dic = new Dictionary<int, City>();
				string fileName = Path.Combine(WebConfig.DataBlockPath, "data\\citymapping.xml");
				DataSet ds = new DataSet();
				ds.ReadXml(fileName);
				//取中心城字典
				Dictionary<int, City> tmpDic = new Dictionary<int, City>();
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					int cityId = ConvertHelper.GetInteger(row["CityId"]);
					int centerId = ConvertHelper.GetInteger(row["Memo1"]);
					if (cityId == centerId)
					{
						string cityName = row["CityName"].ToString().Trim();
						string citySpell = row["EngName"].ToString().Trim();
						int cityLevel = ConvertHelper.GetInteger(row["Level"]);
						tmpDic[cityId] = new City(cityId, cityName, citySpell, cityLevel);
					}
				}

				//生成对应关系
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					int cityId = ConvertHelper.GetInteger(row["CityId"]);
					int centerId = ConvertHelper.GetInteger(row["Memo1"]);
					if (centerId != 0 && cityId != centerId && tmpDic.ContainsKey(centerId))
					{
						to30Dic[cityId] = tmpDic[centerId];
					}
				}

				CacheManager.InsertCache(cacheKey, to30Dic, 60);
			}
			return to30Dic;
		}

		/// <summary>
		/// 获取子品牌全拼与ID的对照
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, int> GetSerialSpellDic()
		{
			string cacheKey = "SerialSpellDictionary";
			Dictionary<string, int> sDic = (Dictionary<string, int>)CacheManager.GetCachedData(cacheKey);
			if (sDic == null)
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(WebConfig.AllSpellList);
				XmlNodeList itemList = xmlDoc.SelectNodes("/Params/Serial/Item");
				sDic = new Dictionary<string, int>();
				foreach (XmlElement item in itemList)
				{
					int sId = Convert.ToInt32(item.GetAttribute("ID"));
					string spell = item.GetAttribute("AllSpell");
					sDic[spell] = sId;
				}
				// modified by chengl Nov.12.2009
				CacheManager.InsertCache(cacheKey, sDic, WebConfig.CachedDuration);
				// modified end
			}
			return sDic;
		}


		// 		/// <summary>
		// 		/// 获取经销商促销信息
		// 		/// </summary>
		// 		/// <param name="serialId"></param>
		// 		/// <param name="cityId"></param>
		// 		/// <param name="count"></param>
		// 		/// <returns></returns>
		// 		public static DataSet GetVendorNews(int serialId, int cityId, int count)
		// 		{
		// 			DataSet vendorNews = null;
		// 			try
		// 			{
		// 				VendorNews vendor = new VendorNews();
		// 				vendorNews = vendor.GetNewsListBySerialID(serialId, count);
		// 				if (vendorNews != null && vendorNews.Tables.Count > 0)
		// 				{
		// 					vendorNews.Tables[0].Columns.Add("url");
		// 					foreach (DataRow row in vendorNews.Tables[0].Rows)
		// 					{
		// 						int newsId = Convert.ToInt32(row["news_Id"]);
		// 						int vendorId = Convert.ToInt32(row["VendorID"]);
		// 						DateTime publish = Convert.ToDateTime(row["news_PubTime"]);
		// 						row["url"] = "http://dealer.bitauto.com/" + vendorId + "/news/" + publish.ToString("yyyy")
		// 							+ "/" + publish.ToString("MM") + "/" + newsId + ".html";
		// 					}
		// 					vendorNews.AcceptChanges();
		// 				}
		// 			}
		// 			catch
		// 			{
		// 
		// 			}
		// 			return vendorNews;
		// 		}
		// 
		// 		/// <summary>
		// 		/// 获取经销商的信息
		// 		/// </summary>
		// 		/// <param name="serialId"></param>
		// 		/// <param name="cityId"></param>
		// 		/// <param name="count">获取数量</param>
		// 		/// <returns></returns>
		// 		public static DataSet GetVendorBySerialAndCity(int serialId, int cityId, int count)
		// 		{
		// 			// 			{vendorID}			object {System.Data.DataColumn}
		// 			// 			{vendorName}		object {System.Data.DataColumn}
		// 			// 			{vendorFullName}	object {System.Data.DataColumn}
		// 			// 			{vendorTel}		object {System.Data.DataColumn}
		// 			// 			{Vendorsite}	object {System.Data.DataColumn}
		// 			// 			{newsTitle}		object {System.Data.DataColumn}
		// 			// 			{newsurl}		object {System.Data.DataColumn}
		// 
		// 			DataSet ds = new VendorSearch().GetVendorNewsList(serialId, -1, cityId, count, "");
		// 			if(ds.Tables.Count > 0)
		// 			{
		// 				ds.Tables[0].Columns.Add("VendorAddress");
		// 				foreach(DataRow row in ds.Tables[0].Rows)
		// 				{
		// 					row["VendorAddress"] = "";
		// 					if(row["Vendorsite"] != DBNull.Value)
		// 					{
		// 						string vendorUrl = Convert.ToString(row["Vendorsite"]).Trim();
		// 						if (!vendorUrl.EndsWith("/"))
		// 						{
		// 							row["Vendorsite"] = vendorUrl + "/";
		// 
		// 						}
		// 					}
		// 				}
		// 			}
		// 			ds.AcceptChanges();
		// 			return ds;
		// 		}

		/// <summary>
		/// 获取新闻分类级别列表
		/// </summary>
		public static Dictionary<int, NewsCategory> GetNewsCategorys()
		{
			string cacheKey = "News_Categroy_Dictionary";

			Dictionary<int, NewsCategory> newsCategorys = (Dictionary<int, NewsCategory>)CacheManager.GetCachedData(cacheKey);
			if (newsCategorys == null)
			{
				newsCategorys = new Dictionary<int, NewsCategory>();
				string xmlUrl = WebConfig.NewsUrl + "?showcategory=1";
				XmlDocument cateDoc = new XmlDocument();
				try
				{
					cateDoc.Load(xmlUrl);
					XmlNodeList cateList = cateDoc.SelectNodes("/NewDataSet/NewsCategory");
					foreach (XmlElement cateNode in cateList)
					{
						//分析分类ID，路径及根ID，并加入分类字典
						int cateId = Convert.ToInt32(cateNode.SelectSingleNode("newscategoryid").InnerText);
						string catePath = cateNode.SelectSingleNode("newscategoryidpath").InnerText;
						NewsCategory newsCate = new NewsCategory(cateId);
						newsCate.CategoryPath = catePath;
						newsCategorys[cateId] = newsCate;
					}

					CacheManager.InsertCache(cacheKey, newsCategorys, WebConfig.CachedDuration);

					//存储文件
					string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\NewsCategorys.xml");
					string backupFile = Path.Combine(WebConfig.DataBlockPath, "Data\\Backup\\NewsCategorys.xml");
					if (File.Exists(xmlFile))
						File.Copy(xmlFile, backupFile, true);
					cateDoc.Save(xmlFile);
				}
				catch
				{

				}
			}

			return newsCategorys;
		}

		/// <summary>
		/// 获取新闻评论数
		/// </summary>
		/// <param name="newsIdList"></param>
		/// <returns></returns>
		private static Dictionary<int, int> GetNewsCommentNum(int[] newsIdList)
		{
			Dictionary<int, int> commentNumDic = new Dictionary<int, int>();
			NewsService ns = new NewsService();
			NewsReference[] refs = ns.SortNewsByComments(newsIdList);
			foreach (NewsReference nrf in refs)
			{
				commentNumDic[nrf.ID] = nrf.CommentCount;
			}
			return commentNumDic;
		}

		/// <summary>
		/// 添加新闻评论数
		/// </summary>
		/// <param name="xmlDoc"></param>
		public static void AppendNewsCommentNum(List<XmlElement> newsList)
		{
			Dictionary<int, int> numDic = new Dictionary<int, int>();		//评论数字典
			List<int> newsIdList = new List<int>();
			int counter = 0;

			try
			{
				//获取评论数
				foreach (XmlElement newsNode in newsList)
				{
					counter++;
					int newsId = Convert.ToInt32(newsNode.SelectSingleNode("newsid").InnerText);
					newsIdList.Add(newsId);
					if (newsIdList.Count > 9 || counter == newsList.Count)
					{
						Dictionary<int, int> tDic = GetNewsCommentNum(newsIdList.ToArray());
						foreach (int nId in tDic.Keys)
							numDic[nId] = tDic[nId];
						newsIdList.Clear();
					}
				}

				//加入新闻信息
				foreach (XmlElement newsNode in newsList)
				{
					int newsId = Convert.ToInt32(newsNode.SelectSingleNode("newsid").InnerText);
					if (numDic.ContainsKey(newsId))
					{
						XmlElement commentNumNode = newsNode.OwnerDocument.CreateElement("CommentNum");
						newsNode.AppendChild(commentNumNode);
						commentNumNode.InnerText = numDic[newsId].ToString();
					}
				}
			}
			catch
			{
			}
		}
		/// <summary>
		/// 得到树形子品牌数量
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, Dictionary<int, int>> GetCacheTreeSerialNewsCount()
		{
			string cachekey = "MasterToSerialDaogouCountCache";

			Dictionary<string, Dictionary<int, int>> serialCount = (Dictionary<string, Dictionary<int, int>>)CacheManager.GetCachedData(cachekey);

			if (serialCount == null)
			{
				if (File.Exists(Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\newsNum.xml")))
				{
					serialCount = GetTreeSerialNewsCount();
					if (serialCount == null || serialCount.Count < 1) return null;
					CacheDependency cacheDependency = new CacheDependency(Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\newsNum.xml"));
					CacheManager.InsertCache(cachekey, serialCount, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				}
			}
			return serialCount;
		}
		/// <summary>
		/// 得到树形子品牌数量
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, Dictionary<int, int>> GetTreeSerialNewsCount()
		{
			string filePath = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\newsNum.xml");

			if (!File.Exists(filePath))
			{
				return null;
			}

			XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(filePath);
			if (xmlDoc == null) return null;

			XmlNodeList xNodeList = xmlDoc.SelectNodes("SerilaList/Serial");

			if (xNodeList == null || xNodeList.Count < 1) return null;

			Dictionary<string, Dictionary<int, int>> serialCount = new Dictionary<string, Dictionary<int, int>>();

			foreach (XmlElement entity in xNodeList)
			{
				foreach (XmlAttribute attrEntity in entity.Attributes)
				{
					if (attrEntity.Name == "id")
					{
						continue;
					}
					else if (serialCount.ContainsKey(attrEntity.Name)
						&& serialCount[attrEntity.Name].ContainsKey(Convert.ToInt32(entity.GetAttribute("id"))))
					{
						continue;
					}
					else if (serialCount.ContainsKey(attrEntity.Name))
					{
						serialCount[attrEntity.Name].Add(Convert.ToInt32(entity.GetAttribute("id"))
							, Convert.ToInt32(entity.GetAttribute(attrEntity.Name)));
					}
					else
					{
						Dictionary<int, int> onlySerialCount = new Dictionary<int, int>();
						onlySerialCount.Add(Convert.ToInt32(entity.GetAttribute("id"))
							, Convert.ToInt32(entity.GetAttribute(attrEntity.Name)));
						serialCount.Add(attrEntity.Name, onlySerialCount);
					}

				}
			}

			return serialCount;
		}
		/// <summary>
		/// 得到树形的XML
		/// </summary>
		/// <returns></returns>
		public static XmlDocument GetTreeXml()
		{
            XmlDocument xmlDoc = AutoStorageService.GetAllAutoXml();

			if (xmlDoc == null)
			{
				return null;
			}

			XmlNodeList xNodeList = xmlDoc.SelectNodes("Params/MasterBrand");
			if (xNodeList == null || xNodeList.Count < 1)
			{
				return null;
			}

			StringBuilder treeXml = new StringBuilder("<data id=\"0\" countnum=\"0\">");
			string masterFormat = "<master id=\"{0}\" name=\"{1}\" countnum=\"0\" firstchar=\"{2}\" extra=\"{3}\">";
			string brandFormat = "<brand id=\"{0}\" name=\"{1}\" countnum=\"0\" extra=\"{2}\">";
            string serialFormat = "<serial id=\"{0}\" name=\"{1}\" countnum=\"0\" extra=\"{2}\" salestate=\"{3}\"/>";

			foreach (XmlElement masterElement in xNodeList)
			{
				treeXml.AppendLine(string.Format(masterFormat
								, masterElement.GetAttribute("ID")
								, masterElement.GetAttribute("Name")
								, masterElement.GetAttribute("Spell").Substring(0, 1).ToUpper()
								, masterElement.GetAttribute("AllSpell")));

				if (!masterElement.HasChildNodes)
				{
					treeXml.AppendLine("</master>");
					continue;
				}

				List<XmlElement> brandXElemList = new List<XmlElement>();

				foreach (XmlElement xNode in masterElement.ChildNodes)
				{
					brandXElemList.Add(xNode);
				}
				//如果子结点数量大于1
				//if (brandXElemList.Count > 1)
				//{
				//    brandXElemList.Sort(NodeCompare.TreeBrandCompare);
				//}

				foreach (XmlElement entity in brandXElemList)
				{
					bool IsContainsBrand = true;
					if (brandXElemList.Count == 1
					   && (entity.GetAttribute("Name") == masterElement.GetAttribute("Name")
					   || entity.GetAttribute("Name") == "进口" + masterElement.GetAttribute("Name"))
					   && entity.HasChildNodes)
					{
						IsContainsBrand = false;
					}
					//如果存在品牌
					if (IsContainsBrand)
					{
						treeXml.AppendLine(string.Format(brandFormat
										, entity.GetAttribute("ID")
										, entity.GetAttribute("Name")
										, entity.GetAttribute("AllSpell")));
					}

					if (!entity.HasChildNodes && IsContainsBrand)
					{
						treeXml.AppendLine("</brand>");
						continue;
					}
					else if (!entity.HasChildNodes)
					{
						continue;
					}

					List<XmlElement> xSerialElemList = new List<XmlElement>();
					foreach (XmlElement xSerialElement in entity.ChildNodes)
					{
						xSerialElemList.Add(xSerialElement);
					}
					//如果结点数量大于1
					if (xSerialElemList.Count > 1)
					{
						xSerialElemList.Sort(NodeCompare.TreeSerialCompare);
					}
					foreach (XmlElement serialElement in xSerialElemList)
					{
						string serialName = serialElement.GetAttribute("ShowName");
						if (serialElement.GetAttribute("CsSaleState") == "停销")
						{
							serialName += " 停产";
						}

						treeXml.AppendLine(string.Format(serialFormat
								 , serialElement.GetAttribute("ID")
								 , serialElement.GetAttribute("Name")
								 , serialElement.GetAttribute("AllSpell")
                                 , serialElement.GetAttribute("CsSaleState")));
					}

					if (IsContainsBrand)
					{
						treeXml.AppendLine("</brand>");
					}
				}
				treeXml.AppendLine("</master>");

			}

			treeXml.AppendLine("</data>");

			XmlDocument treeXmlDoc = new XmlDocument();
			try
			{
				treeXmlDoc.LoadXml(treeXml.ToString());
			}
			catch
			{
				return null;
			}
			return treeXmlDoc;
		}
		/// <summary>
		/// 得到缓存的树形XML结构
		/// </summary>
		/// <returns></returns>
		public static XmlDocument GetCacheTreeXml()
		{
			string cachekey = "AutoTreeXmlDocumentData";
			XmlDocument autoXml = (XmlDocument)CacheManager.GetCachedData(cachekey);

			if (autoXml == null)
			{
				if (File.Exists(WebConfig.AutoDataFile))
				{
					autoXml = GetTreeXml();
					if (autoXml == null) return null;
					CacheDependency cacheDependency = new CacheDependency(WebConfig.AutoDataFile);
					CacheManager.InsertCache(cachekey, autoXml, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				}
			}

			return autoXml;
		}
		/// <summary>
		/// 得到城市名称ID列表
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, City> GetCityNameIdList()
		{
			string cachekey = "AutoCityNameIdRelationList";

			Dictionary<int, City> cityList = (Dictionary<int, City>)CacheManager.GetCachedData(cachekey);

			if (cityList == null)
			{
				string filePath = Path.Combine(WebConfig.DataBlockPath, "Data\\city.xml");
				if (File.Exists(filePath))
				{
					cityList = new Dictionary<int, City>();
					XmlDocument xmlDoc = new XmlDocument();
					try
					{
						xmlDoc.Load(filePath);
						if (xmlDoc == null) return null;
						XmlNodeList xNodeList = xmlDoc.SelectNodes("CityValueSet/CityItem");
						if (xNodeList == null || xNodeList.Count < 1) return null;

						foreach (XmlElement entity in xNodeList)
						{
							int cityId = entity.SelectSingleNode("CityId") == null
									? 0 : Convert.ToInt32(entity.SelectSingleNode("CityId").InnerText.ToString());
							string name = entity.SelectSingleNode("CityName") == null
									? "" : entity.SelectSingleNode("CityName").InnerText.ToString();
							string citySpell = entity.SelectSingleNode("EngName") == null
									? "" : entity.SelectSingleNode("EngName").InnerText.ToLower();
							int cityLevel = entity.SelectSingleNode("Level") == null
									? 0 : Convert.ToInt32(entity.SelectSingleNode("Level").InnerText);
							City city = new City(cityId, name, citySpell, cityLevel);

							if (cityList.ContainsKey(cityId)) continue;

							cityList.Add(cityId, city);
						}

						CacheDependency cacheDependency = new CacheDependency(filePath);
						CacheManager.InsertCache(cachekey, cityList, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
					}
					catch
					{
						return null;
					}
				}
			}

			return cityList;
		}
		/// <summary>
		/// 得到城市名称全拼的列表
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, City> GetCityNameAllSpellList()
		{
			string cachekey = "AutoCityNameAllSpellRelationList";

			Dictionary<string, City> cityList = (Dictionary<string, City>)CacheManager.GetCachedData(cachekey);

			if (cityList == null)
			{
				string filePath = Path.Combine(WebConfig.DataBlockPath, "Data\\city.xml");
				if (File.Exists(filePath))
				{
					cityList = new Dictionary<string, City>();
					XmlDocument xmlDoc = new XmlDocument();
					try
					{
						xmlDoc.Load(filePath);
						if (xmlDoc == null) return null;
						XmlNodeList xNodeList = xmlDoc.SelectNodes("CityValueSet/CityItem");
						if (xNodeList == null || xNodeList.Count < 1) return null;

						foreach (XmlElement entity in xNodeList)
						{
							int cityId = entity.SelectSingleNode("CityId") == null
									? 0 : Convert.ToInt32(entity.SelectSingleNode("CityId").InnerText.ToString());
							string name = entity.SelectSingleNode("CityName") == null
									? "" : entity.SelectSingleNode("CityName").InnerText.ToString();
							string citySpell = entity.SelectSingleNode("EngName") == null
									? "" : entity.SelectSingleNode("EngName").InnerText.ToLower();
							int cityLevel = entity.SelectSingleNode("Level") == null
									? 0 : Convert.ToInt32(entity.SelectSingleNode("Level").InnerText);
							City city = new City(cityId, name, citySpell, cityLevel);

							if (cityList.ContainsKey(citySpell)) continue;

							cityList.Add(citySpell, city);
						}

						CacheDependency cacheDependency = new CacheDependency(filePath);
						CacheManager.InsertCache(cachekey, cityList, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
					}
					catch
					{
						return null;
					}
				}
			}

			return cityList;
		}
		/// <summary>
		/// 得到主品牌和品牌的新闻数量列表
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, Dictionary<string, int>> GetMasterBrandOrBrandNewsCountList(string brandType)
		{
			string cacheKey = "NewsCountByBrandType_" + brandType;
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
				return (Dictionary<int, Dictionary<string, int>>)obj;

			string filePath = string.Empty;
			string xPath = string.Empty;
			if (brandType.ToLower() == "masterbrand")
			{
				filePath = Path.Combine(WebConfig.DataBlockPath, _MasterBrandNumberPath);
				xPath = "root/MasterBrand";
			}
			else if (brandType.ToLower() == "brand")
			{
				filePath = Path.Combine(WebConfig.DataBlockPath, _BrandNumberPath);
				xPath = "root/Brand";
			}
			else
			{
				return null;
			}

			if (!File.Exists(filePath)) { return null; }

			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				xmlDoc.Load(filePath);
				XmlNodeList xNodeList = xmlDoc.SelectNodes(xPath);
				if (xNodeList == null || xNodeList.Count < 1) return null;
				Dictionary<int, Dictionary<string, int>> newCount = new Dictionary<int, Dictionary<string, int>>();
				foreach (XmlElement xElem in xNodeList)
				{
					int entityId = Convert.ToInt32(xElem.GetAttribute("ID"));
					if (entityId < 1) continue;
					Dictionary<string, int> tempValue = new Dictionary<string, int>();

					foreach (XmlAttribute xAttr in xElem.Attributes)
					{
						if (tempValue.ContainsKey(xAttr.Name)) continue;
						tempValue.Add(xAttr.Name, Convert.ToInt32(xAttr.Value));
					}

					if (newCount.ContainsKey(entityId))
					{
						newCount[entityId] = tempValue;
					}
					else
					{
						newCount.Add(entityId, tempValue);
					}
				}

				CacheDependency cd = new CacheDependency(filePath);
				CacheManager.InsertCache(cacheKey, newCount, cd, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				return newCount;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// 所有子品牌的行情新闻优惠价格(接口提供 朱永旭)
		/// http://api.admin.bitauto.com/api/list/MarketNewsToCar.aspx?typeId=1
		/// 城市ID、子品牌ID、优惠价格
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, Dictionary<int, decimal>> GetAllSerialDiscountPrice()
		{
			Dictionary<int, Dictionary<int, decimal>> dic = new Dictionary<int, Dictionary<int, decimal>>();
			string cacheKey = "GetAllSerialDiscountPrice";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{ return (Dictionary<int, Dictionary<int, decimal>>)obj; }
			else
			{
				string filePath = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\HangQingDiscountPrice.xml");
				if (File.Exists(filePath))
				{
					XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(filePath);
					if (xmlDoc != null)
					{
						XmlNodeList xnl = xmlDoc.SelectNodes("NewDataSet/MaxMarketNews");
						if (xnl == null || xnl.Count < 1) return null;
						foreach (XmlElement xe in xnl)
						{
							int cityid = Convert.ToInt32(xe.SelectSingleNode("CityId").InnerText);
							int csid = Convert.ToInt32(xe.SelectSingleNode("CsId").InnerText);
							decimal maxDiscountPrice = Convert.ToDecimal(xe.SelectSingleNode("MaxDiscountPrice").InnerText);

							if (dic.ContainsKey(cityid))
							{
								if (!dic[cityid].ContainsKey(csid))
								{
									dic[cityid].Add(csid, maxDiscountPrice);
								}
								else
								{
									if (maxDiscountPrice > dic[cityid][csid])
									{
										dic[cityid][csid] = maxDiscountPrice;
									}
								}
							}
							else
							{
								Dictionary<int, decimal> dicMDP = new Dictionary<int, decimal>();
								dicMDP.Add(csid, maxDiscountPrice);
								dic.Add(cityid, dicMDP);
							}
						}
						CacheDependency cacheDependency = new CacheDependency(Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\newsNum.xml"));
						CacheManager.InsertCache(cacheKey, dic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
					}
				}
			}
			return dic;
		}

		/// <summary>
		/// 获取新闻数，根据serial\newnum.xml
		/// </summary>
		public static int GetSerialNewsCount(int serialId, CarNewsType carNewsType)
		{
			return GetSerialNewsCount(serialId, carNewsType.ToString());
		}
		/// <summary>
		/// 获取新闻数，根据serial\newnum.xml
		/// </summary>
		public static int GetSerialNewsCount(int serialId, string elementAttr)
		{
			if (serialId > 1 && !string.IsNullOrEmpty(elementAttr))
			{
				Dictionary<string, Dictionary<int, int>> newsNum = GetCacheTreeSerialNewsCount();
				if (newsNum != null)
				{
					if (newsNum.ContainsKey(elementAttr) && newsNum[elementAttr].ContainsKey(serialId))
					{
						return newsNum[elementAttr][serialId];
					}
				}
			}
			return 0;
		}

		/// <summary>
		/// 置换城市显示的城市列表
		/// </summary>
		public static Dictionary<int, City> GetZhiHuanShowCityList()
		{
			string cacheKey = "AutoStorageService_GetZhiHuanShowCityList";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj == null)
			{
				Dictionary<int, City> cityList = new Dictionary<int, City>(49);
				cityList.Add(201, new City(201, "北京", "beijing"));
				cityList.Add(910, new City(910, "保定", "baoding"));
				cityList.Add(1401, new City(1401, "长春", "changchun"));
				cityList.Add(1301, new City(1301, "长沙", "changsha"));
				cityList.Add(3101, new City(3101, "重庆", "chongqing"));
				cityList.Add(2501, new City(2501, "成都", "chengdu"));
				cityList.Add(1102, new City(1102, "大庆", "daqing"));
				cityList.Add(1708, new City(1708, "大连", "dalian"));
				cityList.Add(504, new City(504, "东莞", "dongguan"));
				cityList.Add(518, new City(518, "佛山", "foshan"));
				cityList.Add(301, new City(301, "福州", "fuzhou"));
				cityList.Add(501, new City(501, "广州", "guangzhou"));
				cityList.Add(701, new City(701, "贵阳", "guiyang"));
				cityList.Add(1801, new City(1801, "呼和浩特", "huhehaote"));
				cityList.Add(1101, new City(1101, "哈尔滨", "haerbin"));
				cityList.Add(3001, new City(3001, "杭州", "hangzhou"));
				cityList.Add(101, new City(101, "合肥", "hefei"));
				cityList.Add(801, new City(801, "海口", "haikou"));
				cityList.Add(2101, new City(2101, "济南", "jinan"));
				cityList.Add(3006, new City(3006, "金华", "jinhua"));
				cityList.Add(2901, new City(2901, "昆明", "kunming"));
				cityList.Add(1002, new City(1002, "洛阳", "luoyang"));
				cityList.Add(401, new City(401, "兰州", "lanzhou"));
				cityList.Add(1501, new City(1501, "南京", "nanjing"));
				cityList.Add(3002, new City(3002, "宁波", "ningbo"));
				cityList.Add(601, new City(601, "南宁", "nanning"));
				cityList.Add(1601, new City(1601, "南昌", "nanchang"));
				cityList.Add(2102, new City(2102, "青岛", "qingdao"));
				cityList.Add(307, new City(307, "泉州", "quanzhou"));
				cityList.Add(901, new City(901, "石家庄", "shijiazhuang"));
				cityList.Add(1701, new City(1701, "沈阳", "shenyang"));
				cityList.Add(2401, new City(2401, "上海", "shanghai"));
				cityList.Add(1502, new City(1502, "苏州", "suzhou"));
				cityList.Add(502, new City(502, "深圳", "shenzhen"));
				cityList.Add(2601, new City(2601, "天津", "tianjin"));
				cityList.Add(2201, new City(2201, "太原", "taiyuan"));
				cityList.Add(902, new City(902, "唐山", "tangshan"));
				cityList.Add(3003, new City(3003, "温州", "wenzhou"));
				cityList.Add(1503, new City(1503, "无锡", "wuxi"));
				cityList.Add(1201, new City(1201, "武汉", "wuhan"));
				cityList.Add(2801, new City(2801, "乌鲁木齐", "wulumuqi"));
				cityList.Add(1518, new City(1518, "徐州", "xuzhou"));
				cityList.Add(302, new City(302, "厦门", "xiamen"));
				cityList.Add(2301, new City(2301, "西安", "xian"));
				cityList.Add(2103, new City(2103, "烟台", "yantai"));
				cityList.Add(1207, new City(1207, "宜昌", "yichang"));
				cityList.Add(1901, new City(1901, "银川", "yinchuan"));
				cityList.Add(2109, new City(2109, "淄博", "zibo"));
				cityList.Add(1001, new City(1001, "郑州", "zhengzhou"));
				obj = cityList;
				CacheManager.InsertCache(cacheKey, obj, 120);
			}
			return obj as Dictionary<int, City>;
		}

		/// <summary>
		/// 获取全部城市字典,parentid为省份id
		/// </summary>
		public static Dictionary<string, CityExtend> Get350CityDic()
		{
			string cacheKey = "AutoStorageService_Get350CityDictionary";
			Dictionary<string, CityExtend> cityDic = (Dictionary<string, CityExtend>)CacheManager.GetCachedData(cacheKey);
			if (cityDic == null)
			{
				string cityXml = Path.Combine(WebConfig.DataBlockPath, "Data\\city\\350city.xml");
				XmlDocument cityDoc = CommonFunction.ReadXmlFromFile(cityXml);
				XmlNodeList cityList = cityDoc.SelectNodes("/CityValueSet/CityItem");
				cityDic = new Dictionary<string, CityExtend>(cityList.Count);
				foreach (XmlElement cityNode in cityList)
				{
					int cityId = Convert.ToInt32(cityNode.SelectSingleNode("CityId").InnerText);
					string cityName = cityNode.SelectSingleNode("CityName").InnerText;
					string citySpell = cityNode.SelectSingleNode("EngName").InnerText.ToLower();
					int cityLevel = Convert.ToInt32(cityNode.SelectSingleNode("Level").InnerText);
					int parentId = Convert.ToInt32(cityNode.SelectSingleNode("ParentId").InnerText);
					cityDic[citySpell] = new CityExtend(cityId, cityName, citySpell, cityLevel, parentId);
				}
				CacheDependency cacheDependency = new CacheDependency(cityXml);
				CacheManager.InsertCache(cacheKey, cityDic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
			}
			return cityDic;
		}
		/// <summary>
		/// 获取全部城市字典,parentid为省份id
		/// </summary>
		public static Dictionary<int, CityExtend> Get350CityDicKeyCityId()
		{
			string cacheKey = "AutoStorageService_Get350CityDicKeyCityId";
			Dictionary<int, CityExtend> cityDic = (Dictionary<int, CityExtend>)CacheManager.GetCachedData(cacheKey);
			if (cityDic == null)
			{
				string cityXml = Path.Combine(WebConfig.DataBlockPath, "Data\\city\\350city.xml");
				XmlDocument cityDoc = CommonFunction.ReadXmlFromFile(cityXml);
				XmlNodeList cityList = cityDoc.SelectNodes("/CityValueSet/CityItem");
				cityDic = new Dictionary<int, CityExtend>(cityList.Count);
				foreach (XmlElement cityNode in cityList)
				{
					int cityId = Convert.ToInt32(cityNode.SelectSingleNode("CityId").InnerText);
					string cityName = cityNode.SelectSingleNode("CityName").InnerText;
					string citySpell = cityNode.SelectSingleNode("EngName").InnerText.ToLower();
					int cityLevel = Convert.ToInt32(cityNode.SelectSingleNode("Level").InnerText);
					int parentId = Convert.ToInt32(cityNode.SelectSingleNode("ParentId").InnerText);
					cityDic[cityId] = new CityExtend(cityId, cityName, citySpell, cityLevel, parentId);
				}
				CacheDependency cacheDependency = new CacheDependency(cityXml);
				CacheManager.InsertCache(cacheKey, cityDic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
			}
			return cityDic;
		}
	}
}
