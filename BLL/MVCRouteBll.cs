using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.DAL;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace BitAuto.CarChannel.BLL
{
	/// <summary>
	/// mvc全拼转id用
	/// </summary>
	public class MVCRouteBll
	{
		/// <summary>
		/// 车系全拼对应的id字典
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, int> GetAllSerialDic()
		{
			string cacheKey = "Car_M_MVC_SerialSpellToId";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				return (Dictionary<string, int>)obj;
			}

			DataSet serialDs = new Car_SerialBll().GetAllValidSerial();
			if (serialDs == null || serialDs.Tables.Count == 0 || serialDs.Tables[0].Rows.Count == 0)
				return null;
	
			Dictionary<string, int> serialDic = new Dictionary<string, int>();
			foreach (DataRow dr in serialDs.Tables[0].Rows)
			{
				string spell = ConvertHelper.GetString(dr["allspell"]);
				int csId = ConvertHelper.GetInteger(dr["cs_id"]);
				if (!serialDic.ContainsKey(spell))
				{
					serialDic.Add(spell, csId);
				}
			}
			CacheManager.InsertCache(cacheKey, serialDic, WebConfig.CachedDuration);
			return serialDic;
		}

		/// <summary>
		/// 品牌全拼对应的id字典
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, int> GetAllBrandDic()
		{
			string cacheKey = "Car_M_MVC_BrandSpellToId";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				return (Dictionary<string, int>)obj;
			}

			DataSet brandDs = new Car_BrandBll().GetAllBrand();
			if (brandDs == null || brandDs.Tables.Count == 0 || brandDs.Tables[0].Rows.Count == 0)
				return null;

			Dictionary<string, int> brandDic = new Dictionary<string, int>();
			foreach (DataRow dr in brandDs.Tables[0].Rows)
			{
				string spell = ConvertHelper.GetString(dr["allspell"]);
				int cbId = ConvertHelper.GetInteger(dr["cb_Id"]);
				if (!brandDic.ContainsKey(spell))
				{
					brandDic.Add(spell, cbId);
				}
			}
			CacheManager.InsertCache(cacheKey, brandDic, WebConfig.CachedDuration);
			return brandDic;
		}

		/// <summary>
		/// 主品牌全拼对应的id字典
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, int> GetAllMasterBrandDic()
		{
			string cacheKey = "Car_M_MVC_MasterBrandSpellToId";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				return (Dictionary<string, int>)obj;
			}

			DataSet masterbrandDs = MasterBrandDal.GetMasterBrands();
			if (masterbrandDs == null || masterbrandDs.Tables.Count == 0 || masterbrandDs.Tables[0].Rows.Count == 0)
				return null;

			Dictionary<string, int> masterbrandDic = new Dictionary<string, int>();
			foreach (DataRow dr in masterbrandDs.Tables[0].Rows)
			{
				string spell = ConvertHelper.GetString(dr["urlspell"]);
				int cbId = ConvertHelper.GetInteger(dr["bs_Id"]);
				if (!masterbrandDic.ContainsKey(spell))
				{
					masterbrandDic.Add(spell, cbId);
				}
			}
			CacheManager.InsertCache(cacheKey, masterbrandDic, WebConfig.CachedDuration);
			return masterbrandDic;
		}
	}
}
