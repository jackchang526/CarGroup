using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannel.DAL
{
	public class SelectCarToolDal
	{
		/// <summary>
		/// 获取最热门的几个子品牌
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public DataSet GetTopPVSerials(int num)
		{
			string sqlStr = "SELECT TOP " + num + " p.CS_Id,s.cs_ShowName,s.allSpell FROM Serial_PvRank p "
				+ "LEFT JOIN dbo.Car_Serial s ON p.CS_ID = s.cs_Id AND s.IsState = 1 WHERE s.CsSaleState<>'停销' ORDER BY Pv_SumNum DESC";
			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
		}

		/// <summary>
		/// 根据条件选车
		/// </summary>
		/// <param name="paras"></param>
		/// <returns></returns>
		public DataSet SelectCar(SelectCarParameters paras)
		{
			string condition = "1=1";
			List<SqlParameter> sqlParas = new List<SqlParameter>();
			if (paras.MinPrice != 0)
			{
				condition += " AND a.MaxPrice >= @minPrice";
				sqlParas.Add(new SqlParameter("@minPrice", paras.MinPrice));
			}
			if (paras.MinPrice == 0 && paras.MaxPrice != 0)
			{
				condition += " AND a.MaxPrice > 0 AND a.MinPrice <= @maxPrice";
				sqlParas.Add(new SqlParameter("@maxPrice", paras.MaxPrice));
			}
			else if (paras.MaxPrice != 0)
			{
				condition += " AND a.MinPrice <= @maxPrice";
				sqlParas.Add(new SqlParameter("@maxPrice", paras.MaxPrice));
			}

			if (paras.MinReferPrice != 0)
			{
				condition += " AND a.CarReferPrice >= @minReferPrice";
				sqlParas.Add(new SqlParameter("@minReferPrice", paras.MinReferPrice));
			}
			if (paras.MaxReferPrice != 0)
			{
				condition += " AND a.CarReferPrice <= @maxReferPrice";
				sqlParas.Add(new SqlParameter("@maxReferPrice", paras.MaxReferPrice));
			}

			if (paras.MinDis != 0)
			{
				condition += " AND a.ExhaustL >= @minDis";
				sqlParas.Add(new SqlParameter("@minDis", paras.MinDis));
			}
			if (paras.MaxDis != 0)
			{
				condition += " AND a.ExhaustL <= @maxDis";
				sqlParas.Add(new SqlParameter("@maxDis", paras.MaxDis));
			}
			if (paras.TransmissionType != 0)
			{
				condition += " AND a.TransmissionType&@trans > 0";
				sqlParas.Add(new SqlParameter("@trans", paras.TransmissionType));
			}
			if (paras.BodyForm != 0)
			{
				condition += " AND a.BodyForm&@body > 0";
				sqlParas.Add(new SqlParameter("@body", paras.BodyForm));
			}
			if (paras.Level != 0)
			{
				condition += " AND a.CarLevel&@level > 0";
				sqlParas.Add(new SqlParameter("@level", paras.Level));
			}
			else
			{
				// add by chengl Mar.5.2014 排除概念车
				condition += " AND a.CarLevel > 0";
			}
			if (paras.Purpose != 0)
			{
				condition += " AND a.Purpose&@purpose > 0";
				sqlParas.Add(new SqlParameter("@purpose", paras.Purpose));
			}
			if (paras.Country != 0)
			{
				condition += " AND a.CarCountry&@country > 0";
				sqlParas.Add(new SqlParameter("@country", paras.Country));
			}
			if (paras.ComfortableConfig != 0)
			{
				condition += " AND a.Comfortable&@comf = @comf";
				sqlParas.Add(new SqlParameter("@comf", paras.ComfortableConfig));
			}
			if (paras.SafetyConfig != 0)
			{
				condition += " AND a.Safety&@safe = @safe";
				sqlParas.Add(new SqlParameter("@safe", paras.SafetyConfig));
			}
			if (paras.DriveType > 0)
			{
				condition += " AND a.DriveType&@DriveType > 0";
				sqlParas.Add(new SqlParameter("@DriveType", paras.DriveType));
			}
			if (paras.FuelType > 0)
			{
				condition += " AND a.FuelType&@FuelType > 0";
				sqlParas.Add(new SqlParameter("@FuelType", paras.FuelType));
			}
			if (paras.MinBodyDoors != 0)
			{
				condition += " AND a.Doors >= @MinBodyDoors";
				sqlParas.Add(new SqlParameter("@MinBodyDoors", paras.MinBodyDoors));
			}
			if (paras.MaxBodyDoors != 0)
			{
				condition += " AND a.Doors <= @MaxBodyDoors";
				sqlParas.Add(new SqlParameter("@MaxBodyDoors", paras.MaxBodyDoors));
			}

			if (paras.MinPerfSeatNum > 0 && paras.MaxPerfSeatNum > 0)
			{
				condition += " AND a.SeatNum >= @MinSeatNum AND a.SeatNum <= @MaxSeatNum";
				sqlParas.Add(new SqlParameter("@MinSeatNum", paras.MinPerfSeatNum));
				sqlParas.Add(new SqlParameter("@MaxSeatNum", paras.MaxPerfSeatNum));
			}
			else
			{
				if (paras.MinPerfSeatNum > 0)
				{
					condition += " AND a.SeatNum = @SeatNum";
					sqlParas.Add(new SqlParameter("@SeatNum", paras.MinPerfSeatNum));
				}
			}
			if (paras.IsWagon > 0)
			{
				condition += " AND a.IsWagon=@IsWagon";
				sqlParas.Add(new SqlParameter("@IsWagon", paras.IsWagon));
			}

			if (paras.BrandType != 0)
			{
				switch (paras.BrandType)
				{
					case 8:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 4));
						break;
					case 9:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 18));
						break;
					case 10:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 8));
						break;
					case 11:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 484));
						break;
					case 12:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 2));
						break;
					case 16:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 16));
						break;
					default:
						condition += " AND a.BrandType&@brandType > 0";
						sqlParas.Add(new SqlParameter("@brandType", paras.BrandType));
						break;
				}
				//condition += " AND a.BrandType&@brandType > 0";
				//sqlParas.Add(new SqlParameter("@brandType", paras.BrandType));
			}
			if (paras.CarConfig != 0)
			{
				condition += " AND a.CarConfig&@cfg = @cfg";
				sqlParas.Add(new SqlParameter("@cfg", paras.CarConfig));
			}

			string sqlStr = "SELECT a.carId,a.CarName,a.csId,a.CarReferPrice,b.cs_ShowName,b.allSpell "				//,c.Pv_SumNum,a.MinPrice,a.MaxPrice,a.Exhaust,a.TransmissionType
				+ " FROM CarInfoForSelecting a INNER JOIN Car_Serial b ON a.csId=b.cs_Id AND b.IsState=1 AND (b.CsSaleState='在销' OR b.CsSaleState='待销') "
				//+ " LEFT JOIN Serial_PvRank c ON a.csId=c.CS_Id "
				+ " INNER JOIN Car_Basic d ON a.carId=d.Car_Id AND d.IsState=1 AND d.Car_SaleState<>'停销' ";
			if (condition.Length > 0)
			{
				sqlStr += " WHERE " + condition;
			}

			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, sqlParas.ToArray());
		}
		/// <summary>
		/// 获取车型的具体信息
		/// </summary>
		/// <returns></returns>
		public DataSet SelectCarInfo()
		{
			// 			string sqlStr = "SELECT a.carId,a.CarName,a.csId,b.cs_ShowName,b.allspell,c.Pv_SumNum ,a.MinPrice,a.MaxPrice,"
			// 				+ "d.Engine_Exhaust,d.UnderPan_TransmissionType,e.car_ReferPrice"
			// 				+ " FROM CarInfoForSelecting a INNER JOIN Car_Serial b ON a.csId=b.cs_Id"
			// 				+ " INNER JOIN Serial_PvRank c ON a.csId=c.CS_Id"
			// 				+ " INNER JOIN dbo.Car_Extend_Item d ON a.carId=d.Car_Id"
			// 				+ " INNER JOIN dbo.Car_Basic e ON a.carId=e.Car_Id";
			string sqlStr = "SELECT a.carId,e.Car_Name,e.Car_YearType,a.csId,b.cs_ShowName,b.allspell,c.pvNum ,a.MinPrice,a.MaxPrice,"
				+ "d.Engine_Exhaust,d.UnderPan_TransmissionType,e.car_ReferPrice"
				+ " FROM CarInfoForSelecting a INNER JOIN Car_Serial b ON a.csId=b.cs_Id"
				+ " INNER JOIN dbo.Car_Basic e ON a.carId=e.Car_Id AND e.Car_SaleState<>'停销' AND e.IsState=1 "
				+ " LEFT JOIN ("
				+ " SELECT Car_Id ,sum(Pv_SumNum) AS pvNum FROM Chart_Car_Pv "
				+ " WHERE CreateDateStr>dateadd(m,-1,getdate()) GROUP BY car_id "
				+ ") c ON a.carId=c.Car_Id "
				+ " LEFT JOIN dbo.Car_Extend_Item d ON a.carId=d.Car_Id";

			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
		}
		/// <summary>
		/// 获取车型油耗
		/// </summary>
		/// <returns></returns>
		public DataSet SelectCarOil()
		{
			string sqlStr = "SELECT Carid,Pvalue FROM dbo.CarDataBase WHERE paramid=658";
			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
		}
	}
}
