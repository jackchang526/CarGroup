using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Web.Caching;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using System.Data;
using BitAuto.Utils.Data;
using System.Data.SqlClient;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL.IndexManager
{
	public class UVIndex : AbstractIndex
	{

		#region IIndex 成员
		/// <summary>
		/// 获取购车指数的前十数据
		/// </summary>
		/// <param name="dateObj">日期数据，用于存储时间，可存储如下结构：年-周、年-季度、年-月</param>
		/// <returns></returns>
		public override Dictionary<string, List<IndexItem>> GetTopListData(DateObj dateObj)
		{
			string topFile = Path.Combine(WebConfig.IndexDataBlockPath, "UV\\Month\\" + dateObj.Year + "\\" + dateObj.DateNum + "\\ListTop10.xml");
			Dictionary<string, List<IndexItem>> topDic = new Dictionary<string, List<IndexItem>>();

			if (File.Exists(topFile))
			{
				XmlDocument topDoc = new XmlDocument();
				topDoc.Load(topFile);

				//厂商				
				//XmlNodeList tmpNodeList = topDoc.SelectNodes("/Root/Cp/Item");
				//topDic["CP"] = this.GetIndexItemList(IndexType.UV, BrandType.Producer, tmpNodeList);

				//主品牌
				XmlNodeList tmpNodeList = topDoc.SelectNodes("/Root/Bs/Item");
				topDic["BS"] = this.GetIndexItemList(IndexType.UV, BrandType.MasterBrand, tmpNodeList);

				//子品牌
				tmpNodeList = topDoc.SelectNodes("/Root/Cs/Item");
				topDic["CS"] = this.GetIndexItemList(IndexType.UV, BrandType.Serial, tmpNodeList);
				/*
                //取级别的前十
                foreach (SerialLevelSpellEnum levelSpellEnum in Enum.GetValues(typeof(SerialLevelSpellEnum)))
                {
                    int levelId = (int)levelSpellEnum;
                    string levelSpell = levelSpellEnum.ToString();
                    string levelName = ((SerialLevelEnum)levelId).ToString();

                    if (levelName == "紧凑型" || levelName == "中大型")
                    {
                        levelName += "车";
                    }

                    tmpNodeList = topDoc.SelectNodes("/Root/AllLevel/Level[@Name=\"" + levelName + "\"]/Item");
                    topDic[levelSpell] = this.GetIndexItemList(IndexType.UV, BrandType.Serial, tmpNodeList);
                }
				*/
			}

			return topDic;
		}


		/// <summary>
		/// 获取关注指数某地区的指数前十
		/// </summary>
		/// <param name="dateObj"></param>
		/// <param name="regionType"></param>
		/// <param name="regionId"></param>
		/// <returns></returns>
		public Dictionary<string, List<IndexItem>> GetRegionalTopListData(DateObj dateObj, RegionType regionType, int regionId)
		{
			Dictionary<string, List<IndexItem>> topDic = new Dictionary<string, List<IndexItem>>();
			//string xmlPath = "";
			//if (regionType == RegionType.Province)
			//    xmlPath = "/Root/Province[@ID=" + regionId + "]/Item";
			//else
			//    xmlPath = "/Root/City[@ID=" + regionId + "]/Item";

			string filePath = Path.Combine(WebConfig.IndexDataBlockPath, "UV\\Month\\" + dateObj.Year + "\\" + dateObj.DateNum);

			/*
			//厂商
			string topFile = Path.Combine(filePath, "ListMoreCp.xml");
			if (File.Exists(topFile))
			{
				//XmlDocument cpDoc = new XmlDocument();
				//cpDoc.Load(topFile);
				//XmlNodeList cpNodeList = cpDoc.SelectNodes(xmlPath);
				//topDic["CP"] = this.GetIndexItemList(IndexType.UV, BrandType.Producer, cpNodeList);
				IndexItem[] itemList = IndexBll.GetIndexItemsByAreaId(topFile, regionType, regionId);
				topDic["CP"] = this.GetIndexItemList(IndexType.UV, BrandType.Producer, itemList);
			}
			 * */
			//品牌
			string topFile = Path.Combine(filePath, "ListMoreBs.xml");
			if (File.Exists(topFile))
			{
				//XmlDocument bsDoc = new XmlDocument();
				//bsDoc.Load(topFile);
				//XmlNodeList bsNodeList = bsDoc.SelectNodes(xmlPath);
				//topDic["BS"] = this.GetIndexItemList(IndexType.UV, BrandType.MasterBrand, bsNodeList);
				IndexItem[] itemList = IndexBll.GetIndexItemsByAreaId(topFile, regionType, regionId);
				topDic["BS"] = this.GetIndexItemList(IndexType.UV, BrandType.MasterBrand, itemList);
			}
			//子品牌
			topFile = Path.Combine(filePath, "ListMoreLevel0.xml");
			if (File.Exists(topFile))
			{
				//XmlDocument csDoc = new XmlDocument();
				//csDoc.Load(topFile);
				//XmlNodeList csNodeList = csDoc.SelectNodes(xmlPath);
				//topDic["CS"] = this.GetIndexItemList(IndexType.UV, BrandType.Serial, csNodeList);
				IndexItem[] itemList = IndexBll.GetIndexItemsByAreaId(topFile, regionType, regionId);
				topDic["CS"] = this.GetIndexItemList(IndexType.UV, BrandType.Serial, itemList);
			}
			/*
			//取级别的前十
			foreach (SerialLevelSpellEnum levelSpellEnum in Enum.GetValues(typeof(SerialLevelSpellEnum)))
			{
				int levelId = (int)levelSpellEnum;
				string levelSpell = levelSpellEnum.ToString();
				topFile = Path.Combine(filePath, "ListMoreLevel" + levelId + ".xml");
				if (File.Exists(topFile))
				{
					//XmlDocument levelDoc = new XmlDocument();
					//levelDoc.Load(topFile);
					//XmlNodeList levelNodeList = levelDoc.SelectNodes(xmlPath);
					//topDic[levelSpell] = this.GetIndexItemList(IndexType.UV, BrandType.Serial, levelNodeList);
					IndexItem[] itemList = IndexBll.GetIndexItemsByAreaId(topFile, regionType, regionId);
					topDic[levelSpell] = this.GetIndexItemList(IndexType.UV, BrandType.Serial, itemList);
				}
			}
			 */
			return topDic;
		}


		/// <summary>
		/// 获取指定月各城市各子品牌的关注指数排行
		/// </summary>
		/// <param name="dateObj"></param>
		/// <returns></returns>
		public static Dictionary<int, List<int>> GetSerialsCityIndexData(DateObj dateObj)
		{
			string cacheKey = "uv_serial_city_dic" + dateObj.Year + "_" + dateObj.DateNum;
			Dictionary<int, List<int>> cityIndexDic = (Dictionary<int, List<int>>)CacheManager.GetCachedData(cacheKey);
			if (cityIndexDic == null)
			{
				cityIndexDic = new Dictionary<int, List<int>>();
				string fileName = Path.Combine(WebConfig.IndexDataBlockPath, "UV\\Month\\" + dateObj.Year + "\\" + dateObj.DateNum + "\\ListMoreLevel0.xml");
				if (File.Exists(fileName))
				{
					XmlDocument indexDoc = new XmlDocument();
					indexDoc.Load(fileName);
					XmlNodeList cityNodeList = indexDoc.SelectNodes("/Root/City");
					foreach (XmlElement cityNode in cityNodeList)
					{
						int cityId = 0;
						bool isId = Int32.TryParse(cityNode.GetAttribute("ID"), out cityId);
						if (!isId)
							continue;
						cityIndexDic[cityId] = new List<int>();
						List<int> serialList = cityIndexDic[cityId];

						//子品牌指数数据
						XmlNodeList serialNodeList = cityNode.SelectNodes("Item");
						foreach (XmlElement serialNode in serialNodeList)
						{
							int serialId = 0;
							isId = Int32.TryParse(serialNode.GetAttribute("ID"), out serialId);
							if (!isId)
								continue;
							serialList.Add(serialId);
						}
					}
					CacheDependency cacheDepend = new CacheDependency(fileName);
					CacheManager.InsertCache(cacheKey, cityIndexDic, cacheDepend, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				}
			}

			return cityIndexDic;
		}
		/// <summary>
		/// 获取某城市子品牌UV指数排行
		/// </summary>
		/// <param name="cityId"></param>
		/// <returns></returns>
		public static Dictionary<int, List<int>> GetSerialCityIndexData(int cityId)
		{
			string cacheKey = "uv_serial_city_index_dic_" + cityId;
			Dictionary<int, List<int>> cityIndexDic = (Dictionary<int, List<int>>)CacheManager.GetCachedData(cacheKey);
			if (cityIndexDic == null)
			{
				cityIndexDic = new Dictionary<int, List<int>>();
				string sql = @"SELECT csID, cityID, SUM(UVCount) AS uvCount FROM dbo.StatisticSerialPVUVCity WHERE  CityID = @cityid GROUP BY csID, cityID ORDER BY	uvCount DESC";
				SqlParameter[] param = { new SqlParameter("@cityid", SqlDbType.Int) };
				param[0].Value = cityId;

				DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, param);
				if (ds.Tables[0].Rows.Count > 0)
				{
					List<int> list = new List<int>();
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						list.Add(ConvertHelper.GetInteger(dr["csid"]));
					}
					cityIndexDic.Add(cityId, list);
				}
				CacheManager.InsertCache(cacheKey, cityIndexDic, WebConfig.CachedDuration);
			}
			return cityIndexDic;
		}
		#endregion
	}
}
