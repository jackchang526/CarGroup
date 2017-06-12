using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.DAL
{
	public class CarInfoDal
	{
		#region 获取车型信息
		/// <summary>
		/// 获取车型信息
		/// </summary>
		/// <param name="pv"></param>
		/// <returns></returns>
		public DataSet GetCarInfoByParams()
		{
			string sqlStr = @"SELECT  car.car_id, car.car_name, car.car_ReferPrice, car.Car_YearType,
									car.Car_ProduceState, car.Car_SaleState, cs.cs_id, cei.Engine_Exhaust,
									cei.UnderPan_TransmissionType, cs.cs_carLevel, ccp.PVSum AS Pv_SumNum
							FROM    dbo.Car_Basic car WITH ( NOLOCK )
									LEFT JOIN dbo.Car_Extend_Item cei WITH ( NOLOCK ) ON car.car_id = cei.car_id
									LEFT JOIN Car_serial cs WITH ( NOLOCK ) ON car.cs_id = cs.cs_id
									LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON car.Car_Id = ccp.CarId
							WHERE   car.isState = 1
									AND cs.isState = 1";
			return BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
		}

		public DataSet GetCarPVDataBySerialId(int serialId)
		{
			string sqlStr = @"SELECT  car.car_id, ccp.PVSum AS Pv_SumNum
								FROM    dbo.Car_Basic car WITH ( NOLOCK )
										LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON car.Car_Id = ccp.CarId
								WHERE   car.Cs_Id = @SerialId
										AND car.isState = 1";

			SqlParameter[] _params = {  
									 	 new SqlParameter("@SerialId",SqlDbType.Int)
									 };
			_params[0].Value = serialId;

			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _params);
		}

		public DataSet GetCarPVData()
		{
			string sqlStr = @"SELECT  car.car_id, ccp.PVSum AS Pv_SumNum
								FROM    dbo.Car_Basic car WITH ( NOLOCK )
										LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON car.Car_Id = ccp.CarId
								WHERE   car.isState = 1";
			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
		}
		#endregion

		#region 获取津贴
		/// <summary>
		/// 获取津贴
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, string> GetSubsidy()
		{
			Dictionary<string, string> subsidyDic = new Dictionary<string, string>();
			string sqlStr = "SELECT CarId,Pvalue FROM CarDataBase WHERE paramid=853";
			using (SqlDataReader sdr = BitAuto.Utils.Data.SqlHelper.ExecuteReader(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr))
			{
				while (sdr.Read())
				{
					subsidyDic.Add(Convert.ToString(sdr["CarId"]), Convert.ToString(sdr["Pvalue"]));
				}
			}
			return subsidyDic;
		}
		#endregion

		#region 获取车型信息
		/// <summary>
		/// 获取车型信息
		/// </summary>
		/// <returns></returns>
		public DataSet GetAllCarInfoDs()
		{
			string sql = @"select cs.cs_id,cs.csname,car.car_id,car.car_name,car.Car_YearType,
									car.car_ReferPrice,car.car_SaleState,car.car_ProduceState,
									cdb1.pvalue as Engine_Exhaust,cdb2.pvalue as UnderPan_TransmissionType,cdb3.pvalue as Engine_ExhaustForFloat
									from dbo.Car_relation car
									left join dbo.Car_Serial cs on car.cs_id = cs.cs_id
									left join dbo.CarDataBase cdb1 on car.car_id=cdb1.carID and cdb1.paramid=423
									left join dbo.CarDataBase cdb2 on car.car_id=cdb2.carID and cdb2.paramid=712
									left join dbo.CarDataBase cdb3 on car.car_id=cdb3.carID and cdb3.paramid=785
									where car.isState=0 and cs.isState=0
									order by car.car_id";
			return BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
		}
		#endregion
	}
}
