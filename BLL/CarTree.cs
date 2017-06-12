using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using BitAuto.Utils;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;
using System.IO;

namespace BitAuto.CarChannel.BLL
{
	public class CarTree : TreeData
	{
		private Car_BrandBll cbb = new Car_BrandBll();
		private Car_BrandDal cbd = new Car_BrandDal();
		public Dictionary<int, string> dicCsPhoto;
		public string TreeXmlData()
		{
 			XmlDocument xmlDoc = AutoStorageService.GetCacheTreeXml();
			if (xmlDoc == null) return "";
			XmlNodeList xNodeList = xmlDoc.SelectNodes("data/master");
			List<XmlElement> nobrandEle = new List<XmlElement>();
			foreach (XmlElement masterElement in xNodeList)
			{
				if (masterElement.ChildNodes.Count < 1) continue;
				XmlElement xElem = (XmlElement)masterElement.ChildNodes[0];

				if (xElem.Name.ToLower() == "serial")
				{
					int serialCount = 0;
					List<XmlElement> noPicEle = new List<XmlElement>();
					noPicEle = GetNoPicSerialList(masterElement.ChildNodes, out serialCount);
					masterElement.SetAttribute("countnum", serialCount.ToString());
					foreach (XmlElement serialElement in noPicEle)
					{
						masterElement.RemoveChild(serialElement);
					}
					//无子品牌的主品牌
					if (serialCount == 0)
					{
						nobrandEle.Add(masterElement);
					}
					continue;
				}

				int totalSerialCount = 0;
				List<XmlElement> noSeialEle = new List<XmlElement>();
				foreach (XmlElement brandElement in masterElement.ChildNodes)
				{
					int serialCount = 0;
					List<XmlElement> noPicEle = new List<XmlElement>();
					noPicEle = GetNoPicSerialList(brandElement.ChildNodes, out serialCount);
					foreach (XmlElement serialElement in noPicEle)
					{
						brandElement.RemoveChild(serialElement);
					}
					totalSerialCount += serialCount;
					brandElement.SetAttribute("countnum", serialCount.ToString());
					// 无有图片的子品牌的品牌
					if (serialCount == 0)
					{
						noSeialEle.Add(brandElement);
					}
				}
				foreach (XmlElement brandElement in noSeialEle)
				{
					masterElement.RemoveChild(brandElement);
				}
				masterElement.SetAttribute("countnum", totalSerialCount.ToString());
				//无品牌的主品牌
				if (totalSerialCount == 0)
				{
					nobrandEle.Add(masterElement);
				}
			}
			foreach (XmlElement masterElement in nobrandEle)
			{
				xmlDoc.FirstChild.RemoveChild(masterElement);
			}
			return xmlDoc.InnerXml;
		}
		/// <summary>
		///  获取品牌/主品牌下没有图片的子品牌列表
		/// </summary>
		/// <param name="serialList">品牌/主品牌下所有子品牌</param>
		/// <param name="serialCount">品牌/主品牌下有图片的子品牌数量</param>
		/// <returns></returns>
		private List<XmlElement> GetNoPicSerialList(XmlNodeList serialList, out int serialCount)
		{
			serialCount = 0;
			List<XmlElement> result = new List<XmlElement>();
			foreach (XmlElement serialElement in serialList)
			{
				int serialid = 0;
				if (int.TryParse(serialElement.Attributes["id"].Value, out serialid) && serialid > 0)
				{
					//子品牌封面图
					string imageUrl = Car_SerialBll.GetSerialImageUrl(serialid);
					// 无图片的子品牌
					if (serialElement.Attributes["salestate"].Value.Trim() == "停销" && imageUrl.IndexOf("150-100.gif") > 0)
					{
						result.Add(serialElement);
						continue;
					}
					serialCount++;
				}
			}
			return result;
		}
		public int GetMasterBrandId(int masterBrandId) { return 0; }
		public int GetBrandId(int brandId) { return 0; }
		public int GetSerialId(int serialId) { return 0; }
		public string GetForcusImageArea() { return ""; }
		public string GetForcusNewsAree() { return ""; }
		public DataSet GetNewsListBySerialId(int serialId, int year) { return null; }
		/// <summary>
		/// 得到新热门的主品牌列表
		/// </summary>
		/// <returns></returns>
		public List<Car_MasterBrandEntity> GetHotMasterBrandEntityList(int entityCount)
		{
			string cacheKey = "treeHotMasterBrandEntity_CheXing";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				return (List<Car_MasterBrandEntity>)obj;
			}
			DataSet ds = cbd.GetSaleingMasterBrand();
			if (ds == null)
			{
				return null;
			}
			List<int> OrderedMasterBrandList = cbb.GetAllMasterOrderByUV();
			if (OrderedMasterBrandList == null || OrderedMasterBrandList.Count < 1)
			{
				return null;
			}
			List<Car_MasterBrandEntity> masterBrandEntityList = new List<Car_MasterBrandEntity>();
			int count = 0;
			foreach (int entity in OrderedMasterBrandList)
			{
				DataRow[] drList = ds.Tables[0].Select("bs_id=" + entity.ToString(), "");
				if (drList == null || drList.Length < 1) continue;
				count++;
				if (count > entityCount) break;
				DataRow dr = drList[0];
				Car_MasterBrandEntity cmbEntity = new Car_MasterBrandEntity();
				cmbEntity.Bs_Id = ConvertHelper.GetInteger(dr["bs_id"].ToString());
				cmbEntity.Bs_Name = dr["bs_Name"].ToString();
				cmbEntity.Bs_allSpell = dr["bsurlspell"].ToString();
				masterBrandEntityList.Add(cmbEntity);
			}
			CacheManager.InsertCache(cacheKey, masterBrandEntityList, 60);
			return masterBrandEntityList;
		}
		/// <summary>
		/// 得到热门子品牌列表
		/// </summary>
		/// <param name="entityCount"></param>
		/// <returns></returns>
		public List<Car_SerialBaseEntity> GetHotSerailEntityList(int entityCount)
		{
			string cacheKey = "treeHotSerialEntity_CheXing";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				return (List<Car_SerialBaseEntity>)obj;
			}
			DataSet ds = new PageBase().GetAllSerialNewly30Day();

			if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
			{
				return null;
			}

			List<Car_SerialBaseEntity> hotCarSerialList = new List<Car_SerialBaseEntity>();
			int count = 0;
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				if (!dr.IsNull("cssaleState") && dr["cssaleState"].ToString().IndexOf("在销") < 0) continue;
				count++;
				if (count > entityCount) break;

				Car_SerialBaseEntity csb = new Car_SerialBaseEntity();
				csb.SerialId = ConvertHelper.GetInteger(dr["cs_ID"].ToString());
				csb.SerialName = dr["cs_name"].ToString();
				csb.SerialShowName = dr["cs_showname"].ToString();
				csb.SerialNameSpell = dr["allspell"].ToString();
				hotCarSerialList.Add(csb);
			}
			CacheManager.InsertCache(cacheKey, hotCarSerialList, 60);
			return hotCarSerialList;
		}
	}
}
