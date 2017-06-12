using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.DAL.Data
{
	public class TBrandDAL
	{
		public DataSet GetBrandDataById(int brandId)
		{
			string sqlStr = @"SELECT  cb.cb_Id, cmr.bs_id, cb.cp_Id, cb_Name, cb_url, cb_Country, Cp_Country,
                                        cb_introduction, cb.spell, cb.IsState, cb.allSpell, cb_seoname
                                FROM    Car_Brand cb
                                        LEFT JOIN dbo.Car_MasterBrand_Rel cmr ON cmr.cb_Id = cb.cb_Id
                                        LEFT JOIN Car_Producer cp ON cp.Cp_Id = cb.cp_Id
                                WHERE   cb.cb_Id = @brandid
                                        AND cb.isState = 1";
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, new SqlParameter("brandid", brandId));
			return ds;
		}

		/// <summary>
		/// 根据生产商ID，获取相关品牌
		/// </summary>
		/// <param name="producerId"></param>
		/// <returns></returns>
		public DataSet GetBrandDataByProducerId(int producerId)
		{
			string sqlStr = @"SELECT cb.cb_Id,cmr.bs_id,cb.cp_Id,cb_Name,cb_Country,Cp_Country,cb_introduction,cb.spell,cb.IsState,cb.allSpell,cb_seoname
								FROM Car_Brand cb
								LEFT JOIN dbo.Car_MasterBrand_Rel cmr ON cmr.cb_Id=cb.cb_Id
								LEFT JOIN Car_Producer cp ON cp.Cp_Id=cb.cp_Id
								Where cp_Id=@cp_Id";
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, new SqlParameter("@cp_Id", producerId));
			return ds;
		}
		/// <summary>
		/// 根据生产商ID，获取相关品牌
		/// </summary>
		/// <param name="producerId"></param>
		/// <returns></returns>
		public DataSet GetBrandDataByMasterBrandId(int masterBrandId)
		{
			string sqlStr = @"SELECT cb.cb_Id,cmr.bs_id,cb.cp_Id,cb_Name,cb_url,cb_Country,Cp_Country,cb_introduction,cb.spell,cb.IsState,cb.allSpell,cb_seoname
								FROM Car_Brand cb
								LEFT JOIN dbo.Car_MasterBrand_Rel cmr ON cmr.cb_Id=cb.cb_Id
								LEFT JOIN Car_Producer cp ON cp.Cp_Id=cb.cp_Id
								Where bs_Id=@bs_Id and cb.IsState=1 order by cb.spell";
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, new SqlParameter("@bs_Id", masterBrandId));
			return ds;
		}
		/// <summary>
		/// 可根据品牌ID获取品牌信息
		/// </summary>
		/// <param name="stime"></param>
		/// <returns></returns>
		public DataSet GetBrandInfoById(int id)
		{
			string sql = "select t.*,mb.bs_name from (select cb.cb_id,cb_name,c.classvalue,r.bs_id from Car_Brand cb ";
			sql += "left join [Car_MasterBrand_Rel] r on cb.cb_id=r.cb_id ";
			sql += "left join [class] c on c.classid=cb.cb_country where cb.cb_id=@id and cb.isstate=0 and r.isstate=0) t inner join Car_MasterBrand mb on mb.bs_id=t.bs_id ";
			SqlParameter[] _param = { new SqlParameter("@id", SqlDbType.Int) };
			_param[0].Value = id;
			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
		}
		/// <summary>
		/// 根据品牌ID取品牌部分数据
		/// </summary>
		/// <param name="brandId">品牌ID</param>
		/// <returns></returns>
		public static DataSet GetPartBrandDataById(int brandId)
		{
			string sqlStr = @"SELECT  cb.cb_country AS CpCountry, cl.classvalue AS CpCountryName, cb.cb_Id,
                                        cmr.bs_id, cb.cp_Id, cb_Name, cb_Country, cb.spell, cb.IsState,
                                        cb.allSpell, cb_seoname, cb.updatetime, cp.cp_name
                                FROM    Car_Brand cb
                                        LEFT JOIN dbo.Car_MasterBrand_Rel cmr ON cmr.cb_Id = cb.cb_Id
                                                                                 AND cmr.isState = 0
                                        LEFT JOIN Car_Producer cp ON cp.Cp_Id = cb.cp_Id
                                        LEFT JOIN class cl ON cl.classid = cb.cb_country
                                WHERE   cb.cb_Id = @brandid";
			SqlParameter[] _param = { new SqlParameter("@brandid", SqlDbType.Int) };
			_param[0].Value = brandId;
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr, _param);
			return ds;
		}
	}
}
