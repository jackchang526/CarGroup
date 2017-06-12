using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common.Cache;
using System.Data.SqlClient;

namespace BitAuto.CarChannel.DAL
{
	public class MasterBrandDal
	{
		public static DataSet GetMasterBrands()
		{
			string catchkey = "GetAllMasterBrands";

			if (CacheManager.GetCachedData(catchkey) == null)
			{
				string sqlStr = "SELECT bs_Id,bs_Name,spell,bs_introduction,urlspell FROM Car_MasterBrand WHERE IsState=1";

				DataSet ds = new DataSet();
				ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
				CacheManager.InsertCache(catchkey, ds, 60);
			}

			return (CacheManager.GetCachedData(catchkey) as DataSet);
		}
		/// <summary>
		/// 获取热门主品牌信息
		/// </summary>
		/// <param name="top">前几个热门主品牌</param>
		/// <returns></returns>
		public DataSet GetHotMasterBrand(int top)
		{
			string sql = @"SELECT {0}
									cmb.bs_Id, cmb.bs_Name, cmb.urlspell, LEFT(cmb.spell, 1) AS spell,
									mb30.UVCount
							FROM    Car_MasterBrand cmb
									LEFT JOIN Car_MasterBrand_30UV mb30 ON cmb.bs_id = mb30.bs_id
							WHERE   cmb.isState = 1
							ORDER BY UVCount DESC";
			sql = string.Format(sql, top > 0 ? "TOP (@num)" : "");
			SqlParameter[] _params = { 
										 new SqlParameter("@num",DbType.Int32)
										 };
			_params[0].Value = top;
			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
		}
	}
}
