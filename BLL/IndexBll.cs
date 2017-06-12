using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Data;
using System.Xml;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;

using BitAuto.CarChannel.Model;
using BitAuto.Utils;


namespace BitAuto.CarChannel.BLL
{
	public class IndexBll
	{
		#region 私有方法
		/// <summary>
		/// 获取目录中最大的一个数
		/// </summary>
		/// <param name="basePath"></param>
		/// <returns></returns>
		private static int GetMaxNumInPath(string basePath)
		{
			// add by chengl Jul.23.2012
			if (!Directory.Exists(basePath)) return 0;
			string[] dirs = Directory.GetDirectories(basePath);
			int maxNum = 0;
			foreach (string dir in dirs)
			{
				int pos = dir.LastIndexOf("\\");
				if (pos > 0)
				{
					int dateNum = 0;
					Int32.TryParse(dir.Substring(pos + 1), out dateNum);
					if (dateNum > maxNum)
						maxNum = dateNum;
				}
			}
			return maxNum;
		}

		/// <summary>
		/// 获取目录中最小的一个数
		/// </summary>
		/// <param name="basePath"></param>
		/// <returns></returns>
		private static int GetMinNumInPath(string basePath)
		{
			// add by chengl Jul.23.2012
			if (!Directory.Exists(basePath)) return 0;
			string[] dirs = Directory.GetDirectories(basePath);
			int minNum = 0;
			foreach (string dir in dirs)
			{
				int pos = dir.LastIndexOf("\\");
				if (pos > 0)
				{
					int dateNum = 0;
					if (Int32.TryParse(dir.Substring(pos + 1), out dateNum))
					{
						if (dateNum < minNum || minNum == 0)
							minNum = dateNum;
					}
				}
			}
			return minNum;
		}

		/// <summary>
		/// 获取指数文件存在的路径
		/// </summary>
		/// <param name="indexType"></param>
		/// <returns></returns>
		private static string GetIndexSubPath(string indexType)
		{
			string subPath = "";
			switch(indexType)
			{
				case "guanzhu":
					subPath = "UV";
					break;
				case "duibi":
					subPath = "Compare";
					break;
				case "gouche":
					subPath = "Dealer";
					break;
				case "xiaoliang":
					subPath = "Sale";
					break;
			}
			return subPath;
		}

		#endregion

		/// <summary>
		/// 获取指定数据类型的最后一个月,季度，周
		/// </summary>
		/// <param name="indexType">数据类型(Dealer|Media|Sale|UV)</param>
		/// <param name="dateType">日期类型(Season|Month|Week)</param>
		/// <returns></returns>
		public static DateObj GetLastDate(string indexType, DateType dateType)
		{
			DateObj dateObj = new DateObj();
			string subPath = GetIndexSubPath(indexType);
			string keyName = "Last_indexType" + subPath + "_DateType" + dateType.ToString();
			object dateCache = CacheManager.GetCachedData(keyName);
			if (dateCache != null)
			{
				dateObj = (DateObj)dateCache;
			}
			else
			{
				string basePath = Path.Combine(WebConfig.IndexDataBlockPath, subPath + "\\" + dateType.ToString());
				dateObj.Year = GetMaxNumInPath(basePath);
				if (dateObj.Year > 0)
				{
					basePath = Path.Combine(basePath, dateObj.Year.ToString());
					dateObj.DateNum = GetMaxNumInPath(basePath);
				}
				CacheManager.InsertCache(keyName, dateObj, 60);
			}
			return dateObj;
		}

		/// <summary>
		/// 获取上牌量数据的最后日期
		/// </summary>
		/// <returns></returns>
		public static DateObj GetShangpaiLastMonthDate()
		{
			DateObj dateObj = new DateObj();
			string keyName = "Last_shangpai_Sort_DateType";
			object dateCache = CacheManager.GetCachedData(keyName);
			if (dateCache != null)
			{
				dateObj = (DateObj)dateCache;
			}
			else
			{
				string basePath = Path.Combine(WebConfig.IndexDataBlockPath, "Sale\\Month");
				string[] dirs = Directory.GetDirectories(basePath, "Sort", SearchOption.AllDirectories);
				List<String> dirList = new List<string>();
				foreach (string dir in dirs)
					dirList.Add(dir);
				dirList.Sort();
				string maxDir = dirList[dirList.Count - 1];
				maxDir = maxDir.Substring(basePath.Length + 1);

				//取年
				int pos = maxDir.IndexOf("\\");
				string yearStr = maxDir.Substring(0, pos);
				dateObj.Year = Convert.ToInt32(yearStr);

				//按最大年再取月
				string yearPath = Path.Combine(basePath, yearStr);
				dirs = Directory.GetDirectories(yearPath, "Sort", SearchOption.AllDirectories);

				//取月
				//取月
				dateObj.DateNum = 1;
				foreach (string monthDir in dirs)
				{
					maxDir = monthDir.Substring(yearPath.Length + 1);
					pos = maxDir.IndexOf("\\");
					string monthStr = maxDir.Substring(0, pos);
					int month = Convert.ToInt32(monthStr);
					if (month > dateObj.DateNum)
						dateObj.DateNum = month;
				}

				CacheManager.InsertCache(keyName, dateObj, 60);
			}
			return dateObj;
		}

		/// <summary>
		/// 获取指定数据类型的最早一个月,季度，周
		/// </summary>
		/// <param name="indexType">数据类型(Dealer|Media|Sale|UV)</param>
		/// <param name="dateType">日期类型(Season|Month|Week)</param>
		/// <returns></returns>
		public static DateObj GetFirstDate(string indexType, DateType dateType)
		{
			DateObj dateObj = new DateObj();
			string subPath = GetIndexSubPath(indexType);
			string keyName = "First_indexType" + subPath + "_DateType" + dateType.ToString();
			object dateCache = CacheManager.GetCachedData(keyName);
			if (dateCache != null)
			{
				dateObj = (DateObj)dateCache;
			}
			else
			{
				string basePath = Path.Combine(WebConfig.IndexDataBlockPath, subPath + "\\" + dateType.ToString());
				dateObj.Year = GetMinNumInPath(basePath);
				if (dateObj.Year > 0)
				{
					basePath = Path.Combine(basePath, dateObj.Year.ToString());
					dateObj.DateNum = GetMinNumInPath(basePath);
				}
				CacheManager.InsertCache(keyName, dateObj, 60);
			}
			return dateObj;
		}


		/// <summary>
		/// 获取子品牌各城市的对比排行
		/// </summary>
		/// <returns></returns>
		public static List<XmlElement> GetCitiesCompare(int serialId)
		{
			string cacheKey = "Serial_Cities_Compare_" + serialId;
			List<XmlElement> cityList = (List<XmlElement>)CacheManager.GetCachedData(cacheKey);
			if (cityList == null)
			{
				cityList = new List<XmlElement>();
				string fileName = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialCityCompare\\" + serialId + "_CityCompare.xml");
				if (File.Exists(fileName))
				{
					try
					{
						XmlDocument doc = new XmlDocument();
						doc.Load(fileName);
						XmlNodeList cityNodeList = doc.SelectNodes("/CityCompare/City");
						foreach (XmlElement cityNode in cityNodeList)
						{
							cityList.Add(cityNode);
						}
					}
					catch { }
				}
				CacheManager.InsertCache(cacheKey, cityList, 60);
			}
			return cityList;
		}

		/// <summary>
		/// 获取城市列表
		/// </summary>
		/// <returns></returns>
		public static List<City> GetCityList()
		{
			List<City> cityList = new List<City>();
			string xmlFile = HttpContext.Current.Server.MapPath("~/App_Data/SpecialCityForExponential.xml");
			if (File.Exists(xmlFile))
			{
				DataSet ds = new DataSet();
				ds.ReadXml(xmlFile);

				City qg = new City();
				qg.CityId = 0;
				qg.CityName = "全国";
				qg.CitySpell = "quanguo";
				cityList.Add(qg);

				foreach (DataRow row in ds.Tables[0].Rows)
				{
					City city = new City();
					city.CityId = Convert.ToInt32(row["CityId"]);
					city.CityName = row["CityName"].ToString();
					city.CitySpell = row["EngName"].ToString();
					cityList.Add(city);
				}
			}
			return cityList;
		}

		/// <summary>
		/// 获取用城市ID索引的城市字典
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, City> GetCityIdDic()
		{
			string cacheKey = "City_ID_Dic";
			Dictionary<int, City> cityDic = (Dictionary<int, City>)CacheManager.GetCachedData(cacheKey);
			if (cityDic == null)
			{
				cityDic = new Dictionary<int, City>();
				List<City> cityList = GetCityList();
				foreach (City city in cityList)
				{
					cityDic[city.CityId] = city;
				}
				CacheManager.InsertCache(cacheKey, cityDic, 30);
			}
			return cityDic;
		}

		/// <summary>
		/// 根据
		/// </summary>
		/// <param name="regionType"></param>
		/// <param name="regionId"></param>
		/// <returns></returns>
		public static IndexItem[] GetIndexItemsByAreaId(string xmlFile, RegionType regionType, int regionId )
		{
			string regType = regionType.ToString();
			List<IndexItem> itemList = new List<IndexItem>();
			using (XmlTextReader reader = new XmlTextReader(xmlFile))
			{
				bool inRegion = false;
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						if(reader.Name == regType)
						{
							reader.MoveToAttribute("ID");
							if (ConvertHelper.GetInteger(reader.Value) == regionId)
							{
								//如果ID相等，说明已经进入该区域
								inRegion = true;
							}
							//如果曾在区域中，而城市ID发生了变化，则说明已经出了区域
							else if(inRegion)
								break;
						}

						//在区域中读节点
						if (inRegion)
						{
							if (reader.Name == "Item")
							{
								IndexItem item = new IndexItem();
								reader.MoveToAttribute("ID");
								item.ID = ConvertHelper.GetInteger(reader.Value);
								reader.MoveToAttribute("Count");
								item.Index = ConvertHelper.GetInteger(reader.Value);
								itemList.Add(item);
							}
						}
					}
				}
				reader.Close();
			}
			return itemList.ToArray();
		}

		#region 根据member cache 取 子品牌指数按级别排名

		/// <summary>
		/// 根据指数类型取子品牌按级别指数排名不分地区(目前只有关注、购车、销量 指数)
		/// </summary>
		/// <param name="it"></param>
		/// <returns></returns>
		public static Dictionary<int, int> GetSerialLevelRankByIndexType(IndexType it)
		{
			Dictionary<int, int> dic = new Dictionary<int, int>();
			string keyTemp = "";
			if (it == IndexType.UV)
			{ keyTemp = "Car_Dictionary_SerialLevelUVRank"; }
			else if (it == IndexType.Dealer)
			{ keyTemp = "Car_Dictionary_SerialLevelDealerRank"; }
			else if (it == IndexType.Sale)
			{ keyTemp = "Car_Dictionary_SerialLevelSaleRank"; }
			if (keyTemp != "")
			{
				object obj = MemCache.GetMemCacheByKey(keyTemp) ;
				if (obj != null)
				{
					dic = obj as Dictionary<int, int>;
				}
			}
			return dic;
		}

		#endregion

	}
}
