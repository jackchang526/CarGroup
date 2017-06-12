using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.DAL
{
	public class SerialDal
	{
		/// <summary>
		/// 获取所有子品牌
		/// </summary>
		/// <returns></returns>
		public static DataSet GetSerials()
		{
			string sqlStr = "SELECT cs.cs_Id,cs.cs_ShowName,cs.cs_name,cs.allSpell,cs.cb_id,cs.cs_CarLevel,cs.CsSaleState,csi.Engine_Exhaust,csi.UnderPan_Num_Type FROM Car_Serial cs "
					+ " LEFT JOIN Car_Serial_Item csi ON csi.cs_Id=cs.cs_Id  WHERE cs.IsState=1";
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
			return ds;
		}

		/// <summary>
		/// 获取所有子品牌带品牌，主品牌的信息
		/// </summary>
		/// <returns></returns>
		public static DataSet GetSerialsWithBrand()
		{
			string cacheKey = "allSerialsWithBrandAndMasterbrand";
			DataSet ds = CacheManager.GetCachedData(cacheKey) as DataSet;

			if (ds == null)
			{
				string sqlStr = "SELECT cmr.bs_Id,cs.cb_Id,cb.cb_Name,cb.cb_Country,cs_Id,cs_Name,cs_ShowName,cs.allSpell FROM Car_Serial cs";
				sqlStr += " LEFT JOIN Car_Brand cb ON cs.cb_Id=cb.cb_Id";
				sqlStr += " LEFT JOIN Car_MasterBrand_Rel cmr ON cmr.cb_Id=cb.cb_Id";
				sqlStr += " WHERE cs.IsState=1";
				ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);

				CacheManager.InsertCache(cacheKey, ds, 60);
			}

			return ds;
		}

		/// <summary>
		/// 获取子品牌下最新年款的全部车型颜色
		/// </summary>
		/// <returns></returns>
		public static DataSet GetSerialYearCarColor()
		{
			string cacheKey = "allSerialYearCarColor";
			DataSet ds = CacheManager.GetCachedData(cacheKey) as DataSet;
			if (ds == null)
			{
				string sqlStr = @"select paramid,pvalue,caryear,cs_id from (select carid,paramid,pvalue,caryear,cs_id from 
								cardatabase right join(select car_id,newtable.caryear,newtable.cs_id from car_relation right join  
								(select max(caryear) as caryear,cs_id from car_serialyear group by cs_id) as newtable
								on newtable.caryear=car_relation.car_yeartype and newtable.cs_id=car_relation.Cs_id
								) as cartypetable on cardatabase.carid=cartypetable.car_id where ParamId in (598,801)
								)as serialtypecolor group by caryear,cs_id,paramid,pvalue order by cs_id";
				ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
				CacheManager.InsertCache(cacheKey, ds, 60);
			}
			return ds;
		}
	}
}
