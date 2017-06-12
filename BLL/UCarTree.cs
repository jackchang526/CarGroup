using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Caching;

using BitAuto.Utils;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.BLL.ucar;

namespace BitAuto.CarChannel.BLL
{
	//public class UCarTree : TreeData
	//{
	//	private readonly string filePath = Path.Combine(WebConfig.DataBlockPath, "data\\UCarAmountData.xml");
	//	/// <summary>
	//	/// UCar树形
	//	/// </summary>
	//	/// <returns></returns>
	//	public string TreeXmlData()
	//	{
	//		if (!File.Exists(filePath)) return "";
	//		string cacheKey = "UCarTreeXML";
	//		object obj = CacheManager.GetCachedData(cacheKey);
	//		if (obj != null) return (string)obj;

	//		string treeXML = GetNoCacheTreeXmlData();
	//		if (string.IsNullOrEmpty(treeXML)) return "";

	//		CacheDependency cd = new CacheDependency(filePath);
	//		CacheManager.InsertCache(cacheKey, treeXML, cd, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
	//		return treeXML;
	//	}
	//	/// <summary>
	//	/// 得到没有车型的XML文件
	//	/// </summary>
	//	/// <returns></returns>
	//	public string GetNoCacheTreeXmlData()
	//	{
	//		if (!File.Exists(filePath)) return "";
	//		XmlDocument xmlDoc = new XmlDocument();
	//		try
	//		{
	//			xmlDoc = GetUCarXmlData();
	//			if (xmlDoc == null) return "";
	//			XmlNodeList xNodeList = xmlDoc.SelectNodes("Params/MasterBrand");

	//			StringBuilder treeXml = new StringBuilder("<data id=\"0\" countnum=\"0\">");
	//			string masterFormat = "<master id=\"{0}\" name=\"{1}\" countnum=\"{4}\" firstchar=\"{2}\" extra=\"{3}\">";
	//			string brandFormat = "<brand id=\"{0}\" name=\"{1}\" countnum=\"{3}\" extra=\"{2}\">";
	//			string serialFormat = "<serial id=\"{0}\" name=\"{1}\" countnum=\"{3}\" extra=\"{2}\" />";

	//			foreach (XmlElement masterElement in xNodeList)
	//			{
	//				treeXml.AppendLine(string.Format(masterFormat
	//								, masterElement.GetAttribute("ID")
	//								, masterElement.GetAttribute("Name")
	//								, masterElement.GetAttribute("Spell").Substring(0, 1).ToUpper()
	//								, masterElement.GetAttribute("AllSpell")
	//								, masterElement.GetAttribute("UCarAmount")));

	//				if (!masterElement.HasChildNodes)
	//				{
	//					treeXml.AppendLine("</master>");
	//					continue;
	//				}

	//				foreach (XmlElement entity in masterElement.ChildNodes)
	//				{
	//					bool IsContainsBrand = true;
	//					if (masterElement.ChildNodes.Count == 1
	//					   && (entity.GetAttribute("Name") == masterElement.GetAttribute("Name")
	//					   || entity.GetAttribute("Name") == "进口" + masterElement.GetAttribute("Name"))
	//					   && entity.HasChildNodes)
	//					{
	//						IsContainsBrand = false;
	//					}
	//					//如果存在品牌
	//					if (IsContainsBrand)
	//					{
	//						treeXml.AppendLine(string.Format(brandFormat
	//										, entity.GetAttribute("ID")
	//										, entity.GetAttribute("Name")
	//										, entity.GetAttribute("AllSpell")
	//										, entity.GetAttribute("UCarAmount")));
	//					}

	//					if (!entity.HasChildNodes && IsContainsBrand)
	//					{
	//						treeXml.AppendLine("</brand>");
	//						continue;
	//					}
	//					else if (!entity.HasChildNodes)
	//					{
	//						continue;
	//					}
	//					foreach (XmlElement serialElement in entity.ChildNodes)
	//					{
	//						string serialName = serialElement.GetAttribute("ShowName");
	//						if (serialElement.GetAttribute("CsSaleState") == "停销")
	//						{
	//							serialName += " 停产";
	//						}

	//						treeXml.AppendLine(string.Format(serialFormat
	//								 , serialElement.GetAttribute("ID")
	//								 , serialElement.GetAttribute("Name")
	//								 , serialElement.GetAttribute("AllSpell")
	//								 , serialElement.GetAttribute("UCarAmount")));
	//					}

	//					if (IsContainsBrand)
	//					{
	//						treeXml.AppendLine("</brand>");
	//					}
	//				}
	//				treeXml.AppendLine("</master>");

	//			}

	//			treeXml.AppendLine("</data>");
	//			return treeXml.ToString();
	//		}
	//		catch
	//		{
	//			return "";
	//		}
	//	}
	//	/// <summary>
	//	/// 得到UCar的XML数据
	//	/// </summary>
	//	/// <returns></returns>
	//	public XmlDocument GetUCarXmlData()
	//	{
	//		if (!File.Exists(filePath)) return null;
	//		string cacheKey = "UCarTreeXMLData";
	//		object obj = CacheManager.GetCachedData(cacheKey);
	//		if (obj != null) return (XmlDocument)obj;
	//		XmlDocument xmlDoc = new XmlDocument();
	//		try
	//		{
	//			xmlDoc.Load(filePath);

	//			CacheDependency cd = new CacheDependency(filePath);
	//			CacheManager.InsertCache(cacheKey, xmlDoc, cd, DateTime.Now.AddMinutes(WebConfig.CachedDuration));

	//			return xmlDoc;
	//		}
	//		catch
	//		{
	//			return null;
	//		}

            
	//	}
	//	public int GetMasterBrandId(int masterBrandId) { return 0; }
	//	public int GetBrandId(int brandId) { return 0; }
	//	/// <summary>
	//	/// 得到子品牌的二手车数量
	//	/// </summary>
	//	/// <param name="serialId"></param>
	//	/// <returns></returns>
	//	public int GetSerialId(int serialId)
	//	{
	//		if (serialId < 1) return 0;
	//		if (!File.Exists(filePath)) return 0;
	//		string cacheKey = "UCarTreeSerialNumber";
	//		object obj = CacheManager.GetCachedData(cacheKey);
	//		if (obj != null)
	//		{
	//			Dictionary<int, int> serialList = (Dictionary<int, int>)obj;
	//			if (serialList.ContainsKey(serialId)) return serialList[serialId];
	//			else return 0;
	//		}

	//		XmlDocument xmlDoc = new XmlDocument();
	//		try
	//		{
	//			xmlDoc.Load(filePath);
	//			if (xmlDoc == null) return 0;
	//			XmlNodeList xNodeList = xmlDoc.SelectNodes("Params/MasterBrand/Brand/Serial");
	//			if (xNodeList == null || xNodeList.Count < 1) return 0;
	//			Dictionary<int, int> serialList = new Dictionary<int, int>();
	//			foreach (XmlElement xElem in xNodeList)
	//			{
	//				if (serialList.ContainsKey(ConvertHelper.GetInteger(xElem.GetAttribute("ID")))) continue;
	//				serialList.Add(ConvertHelper.GetInteger(xElem.GetAttribute("ID"))
	//							 , ConvertHelper.GetInteger(xElem.GetAttribute("UCarAmount")));
	//			}
	//			CacheDependency cd = new CacheDependency(filePath);
	//			CacheManager.InsertCache(cacheKey, serialList, cd, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
	//			if (serialList.ContainsKey(serialId)) return serialList[serialId];
	//			else return 0;
	//		}
	//		catch
	//		{
	//			return 0;
	//		}
	//	}
	//	public string GetForcusImageArea() { return ""; }
	//	public string GetForcusNewsAree() { return ""; }
	//	public DataSet GetNewsListBySerialId(int serialId, int year) { return null; }
	//	public List<Car_MasterBrandEntity> GetHotMasterBrandEntityList(int entityCount) { return null; }
	//	public List<Car_SerialBaseEntity> GetHotSerailEntityList(int entityCount) { return null; }
	//	/// <summary>
	//	/// 得到搜索结果
	//	/// </summary>
	//	public DataTable GetSearchList(int pMin,
	//									int pMax,
	//									int level,
	//									double dMin,
	//									double dMax,
	//									int tValue,
	//									int provinces,
	//									int citys,
	//									int yMin,
	//									int yMax,
	//									int dmMin,
	//									int dmMax,
	//									int carSource,
	//									int orderCondition,
	//									int order,
	//									int pageIndex,
	//									int pageSize,
	//									out int rowCount)
	//	{
	//		rowCount = 0;
	//		SearchManage sm = new SearchManage();
	//		//报价
	//		if (pMax > 0 && pMax != 9999)
	//		{
	//			sm.HighPrice = pMax ;
	//			sm.LowPrice = pMin;
	//		}
	//		else if (pMax == 9999)
	//		{
	//			sm.LowPrice = pMin;
	//		}
	//		//级别
	//		if (level > 0) sm.CarLevel = level;
	//		//排量
	//		if (dMin == 3.0)
	//		{
	//			sm.LowExhaust = dMin;
	//		}
	//		else if (dMax > 0 && dMax != 3.0)
	//		{
	//			sm.HighExhaust = dMax;
	//			sm.LowExhaust = dMin;
	//		}            
	//		//档位
	//		if (tValue > 0)
	//		{
	//			sm.GearBoxType = tValue;
	//		}
	//		//城市
	//		if (provinces > 0)
	//		{
	//			sm.ProvinceId = provinces;
	//			sm.CityId = citys;
	//		}
	//		//使用年限
	//		if(yMax < 1)
	//		{
	//			sm.LowAge = -1;
	//			sm.HighAge = -1;
	//		}
	//		else if (yMax == 1)
	//		{
	//			sm.LowAge = 0;
	//			sm.HighAge = 0;
	//		}
	//		else if (yMax != 6)
	//		{
	//			sm.LowAge = yMin;
	//			sm.HighAge = yMax;
	//		}
	//		else if (yMax == 6)
	//		{
	//			sm.LowAge = 6;
	//		}
	//		//行驶里程
	//		if (dmMax > 0 && dmMax != 10)
	//		{
	//			sm.LowDrivingMileage = dmMin * 10000;
	//			sm.HighDrivingMileage = dmMax * 10000;
	//		}
	//		else if (dmMax == 10)
	//		{
	//			sm.LowDrivingMileage = dmMax * 10000;
	//		}
	//		//汽车来源
	//		if (carSource > 0)
	//		{
	//			sm.SourceType = carSource;
	//		}
	//		//排序条件
	//		sm.OrderId = orderCondition;
	//		sm.OrderDirection = order;
	//		//页码
	//		sm.PageIndex = pageIndex;
	//		sm.PageSize = pageSize;


	//		BitAutoCarSource bcs = new BitAutoCarSource();
	//		try
	//		{
	//			DataTable dt = bcs.GetCarListBySearchManager(sm,out rowCount);
	//			return dt;
	//		}
	//		catch
	//		{
	//			return null;
	//		}
	//		return null;

            
	//	}
	//	/// <summary>
	//	/// 得到搜索结果
	//	/// </summary>
	//	/// <param name="serialId"></param>
	//	/// <param name="provinces"></param>
	//	/// <param name="citys"></param>
	//	/// <param name="orderCondition"></param>
	//	/// <param name="order"></param>
	//	/// <param name="pageIndex"></param>
	//	/// <param name="pageSize"></param>
	//	/// <returns></returns>
	//	public DataTable GetSearchList(int serialId,
	//									int provinces,
	//									int citys,
	//									int orderCondition,
	//									int order,
	//									int pageIndex,
	//									int pageSize,
	//									out int rowCount)
	//	{
	//		rowCount = 0;
	//		SearchManage sm = new SearchManage();
	//		sm.SeriesId = serialId;
	//		sm.LowAge = -1;
	//		sm.HighAge = -1;
	//		//城市
	//		if (provinces > 0)
	//		{
	//			sm.ProvinceId = provinces;
	//			sm.CityId = citys;
	//		}
	//		//排序条件
	//		sm.OrderId = orderCondition;
	//		sm.OrderDirection = order;
	//		//页码
	//		sm.PageIndex = pageIndex;
	//		sm.PageSize = pageSize;

	//		BitAutoCarSource bcs = new BitAutoCarSource();
	//		try
	//		{
	//			DataTable dt = bcs.GetCarListBySearchManager(sm, out rowCount);
	//			return dt;
	//		}
	//		catch
	//		{
	//			return null;
	//		}
	//	}
	//}
}
