using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.DAL.Data
{
    public class TMasterBrandDAL
    {
        /// <summary>
        /// 根据主品牌ID获取主品牌信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetMasterBrandDataById(int id)
        {
            string sqlStr = "SELECT bs_Id,bs_Name,bs_Country,bs_LogoInfo,spell,IsState,urlspell,bs_introduction,bs_seoname "
                + " FROM Car_MasterBrand WHERE bs_Id=@bsid and isState=1";
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr,new SqlParameter("@bsid",id));
            return ds;
        }
        /// <summary>
        /// 可根据主品牌ID获取主品牌信息
        /// </summary>
        /// <param name="stime"></param>
        /// <returns></returns>
        public DataSet GetMasterBrandInfoById(int id)
        {
            string sql = "SELECT mb.bs_id,bs_name,mb.bs_introduction,mb.createtime,mb.updatetime,mb.isstate,c.classvalue FROM Car_MasterBrand mb left join [class] c on mb.bs_country=c.classid WHERE mb.bs_id=@id and mb.isstate=0";
            SqlParameter[] _param = { new SqlParameter("@id", SqlDbType.Int) };
            _param[0].Value = id;
            return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
        }
		/// <summary>
		/// 可根据主品牌ID获取主品牌信息
		/// </summary>
		/// <param name="masterId"></param>
		/// <returns></returns>
		public static DataSet GetPartMasterInfoById(int masterId)
		{
			string sql = "SELECT mb.bs_id,bs_name,mb.spell,mb.urlspell,mb.isstate,mb.bs_seoname,mb.updatetime FROM Car_MasterBrand mb WHERE mb.bs_id=@id";
			SqlParameter[] _param = { new SqlParameter("@id", SqlDbType.Int) };
			_param[0].Value = masterId;
			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
		}
    }
}
