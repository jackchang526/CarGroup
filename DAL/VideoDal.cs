using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;

namespace BitAuto.CarChannel.DAL
{
	public class VideoDal
	{
		/// <summary>
		/// 获取子品牌视频数据量
		/// </summary>
		/// <param name="serialId">子品牌ID</param>
		/// <returns></returns>
		public static int GetVideoCountBySerialId(int serialId)
		{
			string sql = @"SELECT COUNT(*) FROM dbo.Car_VideoToSerial WHERE SerialId=@SerialId";
			SqlParameter[] _params ={
			                          new SqlParameter("@SerialId",SqlDbType.Int)
			                      };
			_params[0].Value = serialId;
			return ConvertHelper.GetInteger(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, _params));
		}
		/// <summary>
		/// 获取视频 根据子品牌或者 分类ID
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="categoryType"></param>
		/// <param name="top"></param>
		/// <returns></returns>
		public static DataSet GetVideoBySerialIdAndCategoryId(int serialId, int CategoryId, int top)
		{
			string sql = string.Format(@"SELECT  {0} v.VideoId,ShortTitle,v.ImageLink,v.ShowPlayUrl
							FROM    dbo.Car_VideoToSerial vs
									LEFT JOIN dbo.Car_Videos v ON vs.VideoId = v.VideoId
							WHERE   vs.SerialId = @SerialId
									AND v.CategoryId = @CategoryId ORDER BY v.Publishtime DESC", top > 0 ? "TOP(@top)" : "");
			SqlParameter[] _params ={
			                          new SqlParameter("@SerialId",SqlDbType.Int),
									   new SqlParameter("@CategoryId",SqlDbType.Int),
									   new SqlParameter("@top",SqlDbType.Int)
			                      };
			_params[0].Value = serialId;
			_params[1].Value = CategoryId;
			_params[2].Value = top;
			return BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, _params);
		}
		/// <summary>
		/// 获取多个分类视频
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="CategoryIdList">分类ID List</param>
		/// <param name="top"></param>
		/// <returns></returns>
		public static DataSet GetVideoBySerialIdAndCategoryId(int serialId, List<int> CategoryIdList, int top)
		{
			string sql = string.Format(@"SELECT {0}
												v.VideoId,ShortTitle,v.ImageLink,v.ShowPlayUrl
										FROM    dbo.Car_VideoToSerialV2 vs
												LEFT JOIN dbo.Car_VideosV2 v ON vs.Id = v.Id
										WHERE   v.CategoryId IN ({1}) AND vs.SerialId=@SerialId
										ORDER BY v.Publishtime DESC",
																					 top > 0 ? "TOP(@top)" : "",
																					 string.Join(",", CategoryIdList.ToArray()));
			SqlParameter[] _params ={
			                          new SqlParameter("@SerialId",SqlDbType.Int),
									   new SqlParameter("@top",SqlDbType.Int)
			                      };
			_params[0].Value = serialId;
			_params[1].Value = top;
			return BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, _params);
		}

		public static DataSet GetVideoBySerialId(int serialId, int top = 5)
		{
			string sql = string.Format(@"SELECT {0} v.VideoId,ShortTitle,v.ImageLink,v.ShowPlayUrl,v.Duration
											FROM    [dbo].[Car_VideoToSerialV2] vs
													LEFT JOIN dbo.Car_VideosV2 v ON vs.Id = v.Id
											WHERE   vs.SerialId = @SerialId
											ORDER BY v.Publishtime DESC ", top > 0 ? "TOP(@top)" : "");
			SqlParameter[] _params ={
			                          new SqlParameter("@SerialId",SqlDbType.Int),
									   new SqlParameter("@top",SqlDbType.Int)
			                      };
			_params[0].Value = serialId;
			_params[1].Value = top;
			return BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, _params);
		}
		/// <summary>
		/// 根据视频编号获取视频
		/// </summary>
		/// <param name="videoId"></param>
		/// <returns></returns>
		public static DataSet GetVideoById(int videoId) {
			string sql = @"SELECT [Title], [ShortTitle], [ImageLink], [ShowPlayUrl], [Duration]
				FROM    [dbo].[Car_VideosV2]
				WHERE   VideoId = @VideoId";
			SqlParameter[] _params ={
			                          new SqlParameter("@VideoId",SqlDbType.Int)
			                      };
			_params[0].Value = videoId;
			return BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, _params);
		}
	}
}
