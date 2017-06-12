using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.CarChannel.Common;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.DAL
{
	public class SerialGoodsDal
	{
		#region 废除
		/// <summary>
		/// 获取子品牌促销商品 根据城市ID 没有取全国
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="cityId"></param>
		/// <returns></returns>
		public DataSet GetSerialGoodsByCity(int serialId,int cityId)
		{
			SqlParameter[] _params = { 
										 new SqlParameter("@CityId", SqlDbType.Int),
										 new SqlParameter("@SerialId", SqlDbType.Int)
									 };
			_params[0].Value = cityId;
			_params[1].Value = serialId;
			return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "SP_Mai_Goods_Summary_GetSerialGoods", _params);
		}
		/// <summary>
		/// 获取商品促销奖品
		/// </summary>
		/// <param name="goodsId"></param>
		/// <returns></returns>
		public DataSet GetGoodsPromotion(int goodsId)
		{
			string sql = @"SELECT  [GoodsId],[Name],[Description]
							FROM    [dbo].[mai_Goods_Promotion]
							WHERE   GoodsId = @GoodsId";
			SqlParameter[] _params = { 
										 new SqlParameter("@GoodsId", SqlDbType.Int)
									 };
			_params[0].Value = goodsId;
			return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, _params);
		}
		/// <summary>
		/// 获取商品 促销车型（全国）
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public DataSet GetGoodsCarList(int serialId)
		{
			SqlParameter[] _params = { 
										 new SqlParameter("@serialId", SqlDbType.Int)
									 };
			_params[0].Value = serialId;
			return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "SP_Mai_Goods_Summary_GetGoodsCarList", _params);
		}
		#endregion

		/// <summary>
		/// 获取子品牌促销商品 根据城市ID 没有取全国
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="cityId"></param>
		/// <returns></returns>
		public DataSet GetGoodsCarListNew(int serialId, int cityId)
		{
			SqlParameter[] _params = { 
										 new SqlParameter("@CityId", SqlDbType.Int),
										 new SqlParameter("@SerialId", SqlDbType.Int)
									 };
			_params[0].Value = cityId;
			_params[1].Value = serialId;
			return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "SP_Mai_Goods_Cars_GetGoods", _params);
		}

		/// <summary>
		/// 购车返现
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="cityId"></param>
		/// <returns></returns>
		public DataSet GetCashBacksCarList(int serialId, int cityId)
		{
			SqlParameter[] _params = { 
										 new SqlParameter("@CityId", SqlDbType.Int),
										 new SqlParameter("@SerialId", SqlDbType.Int)
									 };
			_params[0].Value = cityId;
			_params[1].Value = serialId;
			return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "SP_CashBack_Car_Get", _params);
		}

	}
}
