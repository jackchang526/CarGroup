using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using System.Data;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.BLL
{
	public class SerialGoodsBll
	{
		private static readonly SerialGoodsDal serialGoodsDAL = new SerialGoodsDal();
		#region 废除
		/// <summary>
		/// 获取子品牌促销商品 根据城市ID 没有取全国
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="cityId"></param>
		/// <returns></returns>
		public List<SerialGoodsEntity> GetSerialGoodsByCity(int serialId, int cityId)
		{
			List<SerialGoodsEntity> list = new List<SerialGoodsEntity>();
			try
			{
				DataSet ds = serialGoodsDAL.GetSerialGoodsByCity(serialId, cityId);
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					SerialGoodsEntity entity = new SerialGoodsEntity();
					entity.GoodsId = ConvertHelper.GetInteger(dr["id"]);
					entity.SerialId = ConvertHelper.GetInteger(dr["cs_id"]);
					entity.GoodsUrl = dr["GoodsUrl"].ToString();
					entity.CoverImageUrl = dr["CoverImageUrl"].ToString();
					entity.PromotTitle = dr["PromotTitle"].ToString();
					entity.StartTime = ConvertHelper.GetDateTime(dr["StartTime"]);
					entity.EndTime = ConvertHelper.GetDateTime(dr["EndTime"]);
					entity.MinMarketPrice = ConvertHelper.GetDecimal(dr["MarketPrice"]);
					entity.MinBitautoPrice = ConvertHelper.GetDecimal(dr["BitautoPrice"]);
					list.Add(entity);
				}
			}
			catch (Exception ex) { CommonFunction.WriteLog(ex.ToString()); }
			return list;
		}
		/// <summary>
		/// 获取商品促销奖品
		/// </summary>
		/// <param name="goodsId"></param>
		/// <returns></returns>
		public List<GoodsPromotionEntity> GetGoodsPromotion(int goodsId)
		{
			List<GoodsPromotionEntity> list = new List<GoodsPromotionEntity>();
			try
			{
				DataSet ds = serialGoodsDAL.GetGoodsPromotion(goodsId);
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					list.Add(new GoodsPromotionEntity()
					{
						GoodsId = ConvertHelper.GetInteger(dr["GoodsId"]),
						Name = dr["Name"].ToString(),
						Description = ConvertHelper.GetString(dr["Description"])
					});
				}
			}
			catch (Exception ex) { CommonFunction.WriteLog(ex.ToString()); }
			return list;
		}
		/// <summary>
		/// 获取商品 促销车型（全国）
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public List<SerialGoodsCarEntity> GetGoodsCarList(int serialId)
		{
			List<SerialGoodsCarEntity> list = new List<SerialGoodsCarEntity>();
			try
			{
				DataSet ds = serialGoodsDAL.GetGoodsCarList(serialId);
				foreach (DataRow dr in ds.Tables[0].Rows.Cast<DataRow>().Take(3))
				{
					list.Add(new SerialGoodsCarEntity()
					{
						GoodsId = ConvertHelper.GetInteger(dr["GoodsId"]),
						CarId = ConvertHelper.GetInteger(dr["Car_Id"]),
						GoodsUrl = dr["GoodsUrl"].ToString(),
						MarketPrice = ConvertHelper.GetDecimal(dr["MarketPrice"]),
						BitautoPrice = ConvertHelper.GetDecimal(dr["BitautoPrice"])
					});
				}
			}
			catch (Exception ex) { CommonFunction.WriteLog(ex.ToString()); }
			return list;
		}
		#endregion
		/// <summary>
		/// 获取易车惠 车型
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="cityId"></param>
		/// <returns></returns>
		public List<CarGoodsEntity> GetGoodsCarListNew(int serialId, int cityId)
		{
			List<int> carIdList = new List<int>();
			List<CarGoodsEntity> list = new List<CarGoodsEntity>();
			try
			{
				DataSet ds = serialGoodsDAL.GetGoodsCarListNew(serialId, cityId);
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int carId=ConvertHelper.GetInteger(dr["CarId"]);
					if(carIdList.Contains(carId))
						continue;
					carIdList.Add(carId);
					CarGoodsEntity entity = new CarGoodsEntity();
					entity.SerialId = ConvertHelper.GetInteger(dr["SerialId"]);
					entity.CarId = carId;
					entity.CityId = ConvertHelper.GetInteger(dr["CityId"]);
					entity.GoodsUrl = dr["GoodsUrl"].ToString();
					entity.MinMarketPrice = ConvertHelper.GetDecimal(dr["MarketPrice"]);
					entity.MinBitautoPrice = ConvertHelper.GetDecimal(dr["BitautoPrice"]);
					list.Add(entity);
				}
			}
			catch (Exception ex) { CommonFunction.WriteLog(ex.ToString()); }
			return list;
		}

		/// <summary>
		/// 按子品牌ID、城市ID取购车返现数据
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="cityId"></param>
		/// <returns></returns>
		public List<CarCashBack> GetCashBacksCarList(int serialId, int cityId)
		{
			List<int> carIdList = new List<int>();
			List<CarCashBack> list = new List<CarCashBack>();
			try
			{
				DataSet ds = serialGoodsDAL.GetCashBacksCarList(serialId, cityId);
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int carId = ConvertHelper.GetInteger(dr["CarId"]);
					if (carIdList.Contains(carId))
						continue;
					carIdList.Add(carId);
					CarCashBack entity = new CarCashBack();
					entity.SerialId = ConvertHelper.GetInteger(dr["SerialId"]);
					entity.CarId = carId;
					entity.CityId = ConvertHelper.GetInteger(dr["CityId"]);
					entity.BackPrice = ConvertHelper.GetDecimal(dr["BackPrice"]);
					entity.Url = dr["Url"].ToString().Trim();
					list.Add(entity);
				}
			}
			catch (Exception ex) { CommonFunction.WriteLog(ex.ToString()); }
			return list;
		}
	}
}
