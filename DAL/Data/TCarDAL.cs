using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.DAL.Data
{
	public class TCarDAL
	{
		/// <summary>
		/// 获取车型基本数据
		/// </summary>
		/// <param name="carId"></param>
		/// <returns></returns>
		public DataSet GetCarDataById(int carId)
		{
			string sqlStr = @"SELECT  car.Car_Id, Car_ProduceState, Car_SaleState, Cs_Id, Car_Name,
										car_ReferPrice, Car_YearType, IsState, ccp.PVSum AS Pv_SumNum
								FROM    Car_Basic car WITH ( NOLOCK )
										LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON car.Car_Id = ccp.CarId
								WHERE   car.Car_Id = @carid
										AND car.isState = 1";
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
				, CommandType.Text
				, sqlStr
				, new SqlParameter[] { new SqlParameter("@carid", carId) });
			return ds;
		}

		/// <summary>
		/// 根据子品牌ID获取所有车型信息
		/// modified by chengl 时间参数化 避免重新编译
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public DataSet GetCarsDataBySerialId(int serialId)
		{
			string sqlStr = @"SELECT  car.Car_Id, Car_ProduceState, Car_SaleState, Cs_Id, Car_Name,
										car_ReferPrice, Car_YearType, IsState, ccp.PVSum AS Pv_SumNum
								FROM    Car_Basic car WITH ( NOLOCK )
										LEFT JOIN dbo.Car_Basic_PV ccp WITH ( NOLOCK ) ON ccp.CarId = car.Car_Id
								WHERE   car.IsState = 1
										AND car.Cs_Id = @csid";
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
				, CommandType.Text
				, sqlStr
				, new SqlParameter[] { new SqlParameter("@csid", serialId) });
			return ds;
		}

		/// <summary>
		/// 获取车型参数
		/// </summary>
		/// <param name="paraId"></param>
		/// <param name="carId"></param>
		/// <returns></returns>
		public DataSet GetCarParameter(int carId, int paraId)
		{
			string sqlStr = "SELECT CarId,car.ParamId,car.Pvalue,p.ParamName,p.AliasName FROM CarDataBase car "
					+ " LEFT JOIN ParamList p ON car.ParamId=p.ParamId "
					+ " WHERE CarId=@carId and car.ParamId=@paraId";
			SqlParameter idPara = new SqlParameter("@carId", carId);
			SqlParameter paraIdPara = new SqlParameter("@paraId", paraId);

			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr, idPara, paraIdPara);
			return ds;
		}

		/// <summary>
		/// 获取车型参数
		/// </summary>
		/// <param name="paraId"></param>
		/// <param name="carId"></param>
		/// <returns></returns>
		public DataSet GetCarParameter(int carId, string aliasName)
		{
			string sqlStr = "SELECT CarId,car.ParamId,car.Pvalue,p.ParamName,p.AliasName FROM CarDataBase car "
					+ " LEFT JOIN ParamList p ON car.ParamId=p.ParamId "
					+ " WHERE CarId=@carId and p.AliasName=@aliasName";
			SqlParameter idPara = new SqlParameter("@carId", carId);
			SqlParameter namePara = new SqlParameter("@aliasName", aliasName);

			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr, idPara, namePara);
			return ds;
		}

		/// <summary>
		/// 根据参数分类获取参数列表
		/// </summary>
		/// <param name="paraClass"></param>
		/// <returns></returns>
		public DataSet GetCarParater(int carId, string paraClass)
		{
			List<SqlParameter> paraList = new List<SqlParameter>();
			string sqlStr = "SELECT CarId,car.ParamId,car.Pvalue,p.ParamName,p.AliasName FROM CarDataBase car"
					+ "LEFT JOIN ParamList p ON car.ParamId=p.ParamId"
					+ "WHERE Carid=@carId";
			paraList.Add(new SqlParameter("@carId", carId));

			if (paraClass.Length > 0)
			{
				sqlStr += "AND car.ParamId IN (SELECT ParamId FROM ParamList WHERE GradeNum LIKE @pClass)";
				paraList.Add(new SqlParameter("@pClass", paraClass + "%"));
			}

			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr, paraList.ToArray());
			return ds;
		}
		/// <summary>
		/// 根据车型ID取车型信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DataSet GetCarInfoById(int id)
		{
			StringBuilder sbSql = new StringBuilder();
			sbSql.Append("select car_id,car_name,cs_id,car_producestate,car_salestate,car_yeartype,car_referprice,c2.classvalue as salestate,c.classvalue as producestate ");
			sbSql.Append("from Car_relation car ");
			sbSql.Append("left join [class] c on car.car_producestate=c.classid ");
			sbSql.Append("left join [class] c2 on car.car_salestate=c2.classid ");
			sbSql.Append("where car.car_id=@id and car.isstate=0");
			SqlParameter[] _param = { new SqlParameter("@id", SqlDbType.Int) };
			_param[0].Value = id;
			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sbSql.ToString(), _param);
		}
		/// <summary>
		/// 根据车型ID取车型部分数据
		/// modified by chengl 加子品牌级别，厂商
		/// </summary>
		/// <param name="carId"></param>
		/// <returns></returns>
		public static DataSet GetPartCarInfoById(int carId)
		{
            string sqlStr = @"SELECT  car.car_id, car.car_name, car.car_yeartype, car.car_ReferPrice,
                                        car.cs_id, car.isState, cs.cs_CarLevel, cp.cp_name, car.Car_SaleState
                                FROM    [Car_Basic] car
                                        LEFT JOIN car_serial cs ON car.cs_id = cs.cs_id
                                        LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                        LEFT JOIN car_Producer cp ON cb.cp_id = cp.cp_id
                                WHERE   car.car_id = @CarId";
            SqlParameter[] _param = { new SqlParameter("@CarId", SqlDbType.Int) };
			_param[0].Value = carId;
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);
			return ds;
		}
		/// <summary>
		/// 根据车型ID取车型参数配置信息
		/// </summary>
		/// <param name="carId"></param>
		/// <returns></returns>
		public static DataSet GetCarParamsListByCarId(int carId)
		{
			string sqlStr = @"SELECT CarId,p.ParamId,car.Pvalue,p.ParamName,p.AliasName FROM ParamList p
							 LEFT JOIN CarDataBase car ON car.ParamId=p.ParamId and car.CarId=@carid";
			SqlParameter[] _param = { new SqlParameter("@carid", SqlDbType.Int) };
			_param[0].Value = carId;
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr, _param);
			return ds;
		}
	}
}
