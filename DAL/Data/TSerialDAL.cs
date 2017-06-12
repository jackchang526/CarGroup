using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.DAL.Data
{
	public class TSerialDAL
	{
		/// <summary>
		/// 获取指定子品牌信息
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
        public DataSet GetSerialDataById(int serialId)
        {
            string sqlStr = " select cs.*,csi.Body_Doors,csi.Engine_Exhaust,csi.UnderPan_Num_Type,bat.bitautoTestURL,csi.ReferPriceRange ";
            sqlStr += " from dbo.Car_Serial cs ";
            sqlStr += " left join Car_Serial_Item csi on cs.cs_id = csi.cs_id ";
            sqlStr += " left join BitAutoTest bat on cs.cs_id = bat.cs_id ";
            sqlStr += " where cs.cs_Id=@csid and cs.isState=1";

            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, new SqlParameter("@csid", serialId));
            return ds;
        }
		/// <summary>
		/// 获取子品牌下的车型列表
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public DataSet GetCarIdListBySerialId(int serialId)
		{
			string sqlStr = "SELECT Car_Id FROM Car_Basic WHERE Cs_Id=@csid";

			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr,new SqlParameter("@csid",serialId));
			return ds;
		}

        /// <summary>
        /// 根据品牌id获取子品牌信息
        /// </summary>
        /// <param name="m_id"></param>
        /// <returns></returns>
        public DataSet GetSerailDataByBrandId(int brandId)
        {
            string sqlStr = " select cs.*,csi.Body_Doors,csi.Engine_Exhaust,csi.UnderPan_Num_Type,bat.bitautoTestURL ";
            sqlStr += " from dbo.Car_Serial cs ";
            sqlStr += " left join Car_Serial_Item csi on cs.cs_id = csi.cs_id ";
            sqlStr += " left join BitAutoTest bat on cs.cs_id = bat.cs_id ";
            sqlStr += " where cs.cb_Id=@cb_Id and cs.IsState=1 order by cs.spell" ;

            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, new SqlParameter("@cb_Id", brandId));
            return ds;
        }
        /// <summary>
        /// 根据子品牌ID获取子品牌信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetSerailInfoById(int id)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select cs_Id,csname,csshowname,cb_id,cspurpose,cssalestate,c.classvalue as bodyform,c2.classvalue as carlevel from Car_Serial s ");
            sbSql.Append("left join [class] c on c.classid=s.csbodyform ");
            sbSql.Append("left join [class] c2 on c2.classid=s.carlevel ");
            sbSql.Append("where cs_id=@id and s.isstate=0");
            SqlParameter[] _param = { new SqlParameter("@id", SqlDbType.Int) };
            _param[0].Value = id;
            return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sbSql.ToString(), _param);
        }
		/// <summary>
		/// 根据子品牌ID获取子品牌信息
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public static DataSet GetPartSerialDataById(int serialId)
		{
			string sqlStr = @"SELECT cs_id,cl.classvalue as cscarlevel,csname,spell,isstate,cssalestate,csshowname,cs_seoname,allspell,carlevel,updatetime,cb_id 
				FROM Car_Serial cs 
				left join class cl on cs.carlevel = cl.classid
				Where cs.cs_id=@serialId";
			SqlParameter[] _param = { new SqlParameter("@serialId", SqlDbType.Int) };
			_param[0].Value = serialId;
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr, _param);
			return ds;
		}
    }
}
