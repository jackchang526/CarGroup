using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.DAL
{
	public class SerialFourthStageDal
	{
		/// <summary>
		/// 获取第四级外观内饰page
		/// </summary>
		/// <param name="csId"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public DataSet GetSerialExteriorInteriorType(int csId, int type)
		{
			DataSet result = null;
			string sqlStr = "select TOP 5 SerialId,Title,Discription,ImageUrl,OrderWeight,[Type] from Car_H5_Serial_Data";
			sqlStr += "  where [Type]=@Type and SerialId=@SerialId and [State] = 0";
			sqlStr += "  order by OrderWeight asc ";
			SqlParameter[] _param ={
                                      new SqlParameter("@SerialId",SqlDbType.Int),
                                      new SqlParameter("@Type",SqlDbType.Int)
                                  };
			_param[0].Value = csId;
			_param[1].Value = type;
			try
			{
				result = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);
			}
			catch
			{
				return null;
			}
			return result;
		}
		/// <summary>
		/// 获子品牌评测与导购新闻列表 易车原创
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="carNewsTypeId"></param>
		/// <param name="top"></param>
		/// <returns></returns>
		public DataSet GetSerialNewsWithCreative(int serialId, List<int> carNewsTypeId, int top)
		{
			if (serialId <= 0 || carNewsTypeId.Count <= 0 || top <= 0) return null;
			string sqlStr = string.Format(@"SELECT TOP ( @Top )
												n.Picture ,
												n.Author ,
												n.FirstPicUrl ,
												mbn.FilePath ,
												mbn.CmsNewsId ,
												mbn.Title ,
												mbn.PublishTime
										FROM    SerialNews AS mbn
												INNER JOIN News n ON n.ID = mbn.CarNewsId
										WHERE   mbn.SerialId = @SerialId
												AND n.CreativeType = 0
												AND CarNewsTypeId IN ({0})
										ORDER BY mbn.PublishTime DESC", string.Join(",", carNewsTypeId));
			SqlParameter[] param = new SqlParameter[]
			{
				new SqlParameter("@Top",top), 
				new SqlParameter("@SerialId",serialId), 
			};
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString,
													CommandType.Text,
													sqlStr,
													param);
			return ds;
		}
		/// <summary>
		/// 获取子品牌亮点
		/// modified by chengl 取全部 2015-7-2
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public DataSet GetSerialSparkle(int serialId)
		{
			DataSet result = null;
			string sqlStr = @"select s.H5SId,Name,OrderId,sr.cs_Id FROM [dbo].[H5_Sparkle] as s inner join [dbo].[Sparkle_Serial_Rel] as sr on sr.H5SId = s.H5SId";
			sqlStr += "   where sr.cs_Id = @SerialId and s.IsState = 1";
			sqlStr += "   order by s.OrderId ";
			List<SqlParameter> sqlParas = new List<SqlParameter>();
			// sqlParas.Add(new SqlParameter("@Top", top));
			sqlParas.Add(new SqlParameter("@SerialId", serialId));
			try
			{
				result = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, sqlParas.ToArray());
			}
			catch
			{
				return null;
			}
			return result;
		}

	}
}
