using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.DAL
{
	public class CommonHtmlDal
	{
		/// <summary>
		/// 获取生成块内容
		/// </summary>
		/// <param name="Id">主品牌、品牌、子品牌、车型 ID</param>
		/// <param name="TypeId">Id参数的类型</param>
		/// <param name="TagId">页面标示</param>
		/// <returns></returns>
		public DataSet GetCommonHtml(int Id, int TypeId, int TagId)
		{
			string sql = @"SELECT [BlockID],[HtmlContent] FROM [Car_CommonHtml] WHERE ID=@Id AND TypeID=@TypeId AND TagID=@TagId";
			SqlParameter[] _params = { 
										 new SqlParameter("@Id", SqlDbType.Int),
										 new SqlParameter("@TypeId", SqlDbType.Int),
										 new SqlParameter("@TagId", SqlDbType.Int)
									 };
			_params[0].Value = Id;
			_params[1].Value = TypeId;
			_params[2].Value = TagId;
			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
		}
		/// <summary>
		/// 获取单个块内容 唯一
		/// </summary>
		/// <param name="Id">主品牌、品牌、子品牌、车型 ID</param>
		/// <param name="TypeId">Id参数的类型</param>
		/// <param name="TagId">页面标示</param>
		/// <param name="blockId">块标示ID</param>
		/// <returns></returns>
		public DataSet GetCommonHtml(int Id, int TypeId, int TagId, int blockId)
		{
			string sql = @"SELECT [BlockID],[HtmlContent] FROM [Car_CommonHtml] WHERE ID=@Id AND TypeID=@TypeId AND TagID=@TagId AND blockid=@blockid";
			SqlParameter[] _params = { 
										 new SqlParameter("@Id", SqlDbType.Int),
										 new SqlParameter("@TypeId", SqlDbType.Int),
										 new SqlParameter("@TagId", SqlDbType.Int),
										 new SqlParameter("@blockid", SqlDbType.Int)
									 };
			_params[0].Value = Id;
			_params[1].Value = TypeId;
			_params[2].Value = TagId;
			_params[3].Value = blockId;
			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
		}
	}
}
