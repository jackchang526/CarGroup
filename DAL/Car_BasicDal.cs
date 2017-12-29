using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using BitAuto.Utils.Data;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace BitAuto.CarChannel.DAL
{
	public class Car_BasicDal
	{
		public Car_BasicDal()
		{ }

		public Car_BasicEntity Populate_Car_BasicEntity_FromDr(DataRow row)
		{
			Car_BasicEntity Obj = new Car_BasicEntity();
			if (row != null)
			{
				Obj.Car_CreateTime = ((row["CreateTime"]) == DBNull.Value) ? Convert.ToDateTime("1900 - 1 - 1") : Convert.ToDateTime(row["CreateTime"]);
				Obj.Car_Id = ((row["Car_Id"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["Car_Id"]);
				Obj.Car_IsLock = ((row["IsLock"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["IsLock"]);
				Obj.Car_IsState = ((row["IsState"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["IsState"]);
				Obj.Car_Name = row["Car_Name"].ToString();
				Obj.Car_OLdCar_Id = ((row["OLdCar_Id"]) == DBNull.Value) ? 0 : Convert.ToInt64(row["OLdCar_Id"]);
				Obj.Car_ProduceState = row["Car_ProduceState"].ToString();
				Obj.Car_ReferPrice = ((row["car_ReferPrice"]) == DBNull.Value) ? 0 : Convert.ToDouble(row["car_ReferPrice"]);
				Obj.Car_SaleState = row["Car_SaleState"].ToString();
				Obj.Car_SpellFirst = row["SpellFirst"].ToString();
				Obj.Car_UpdateTime = ((row["UpdateTime"]) == DBNull.Value) ? Convert.ToDateTime("1900 - 1 - 1") : Convert.ToDateTime(row["UpdateTime"]);
				Obj.Car_YearType = ((row["Car_YearType"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["Car_YearType"]);
				Obj.Cb_id = ((row["cb_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["cb_id"]);
				Obj.Cb_Name = row["cb_name"].ToString();
				Obj.Cp_id = ((row["cp_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["cp_id"]);
				Obj.Cp_Name = row["cp_name"].ToString();
				Obj.Cs_AllSpell = row["cs_AllSpell"].ToString();
				Obj.Cs_Id = ((row["cs_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["cs_id"]);
				Obj.Cs_Name = row["cs_Name"].ToString().Trim();
				Obj.Cs_ShowName = row["Cs_ShowName"].ToString().Trim();
				Obj.MasterName = row["bs_Name"].ToString().Trim();
			}
			else
			{
				return null;
			}
			return Obj;
		}

		public Car_BasicEntity Populate_Car_BasicEntity_FromDr(IDataReader dr)
		{
			Car_BasicEntity Obj = new Car_BasicEntity();
			Obj.Car_CreateTime = ((dr["CreateTime"]) == DBNull.Value) ? Convert.ToDateTime("1900 - 1 - 1") : Convert.ToDateTime(dr["CreateTime"]);
			Obj.Car_Id = ((dr["Car_Id"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["Car_Id"]);
			Obj.Car_IsLock = ((dr["IsLock"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["IsLock"]);
			Obj.Car_IsState = ((dr["IsState"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["IsState"]);
			Obj.Car_Name = dr["Car_Name"].ToString();
			Obj.Car_OLdCar_Id = ((dr["OLdCar_Id"]) == DBNull.Value) ? 0 : Convert.ToInt64(dr["OLdCar_Id"]);
			Obj.Car_ProduceState = dr["Car_ProduceState"].ToString();
			Obj.Car_ReferPrice = ((dr["car_ReferPrice"]) == DBNull.Value) ? 0 : Convert.ToDouble(dr["car_ReferPrice"]);
			Obj.Car_SaleState = dr["Car_SaleState"].ToString();
			Obj.Car_SpellFirst = dr["SpellFirst"].ToString();
			Obj.Car_UpdateTime = ((dr["UpdateTime"]) == DBNull.Value) ? Convert.ToDateTime("1900 - 1 - 1") : Convert.ToDateTime(dr["UpdateTime"]);
			Obj.Car_YearType = ((dr["Car_YearType"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["Car_YearType"]);
			Obj.Cb_id = ((dr["cb_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["cb_id"]);
			Obj.Cb_Name = dr["cb_name"].ToString();
			Obj.Cp_id = ((dr["cp_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["cp_id"]);
			Obj.Cp_Name = dr["cp_name"].ToString();
			Obj.Cs_AllSpell = dr["cs_AllSpell"].ToString();
			Obj.Cs_Id = ((dr["cs_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["cs_id"]);
			Obj.Cs_Name = dr["cs_Name"].ToString().Trim();
			Obj.Cs_ShowName = dr["Cs_ShowName"].ToString().Trim();
			Obj.MasterName = dr["bs_Name"].ToString().Trim();
			return Obj;
		}

		/// <summary>
		/// 取所有车型 包括无效的
		/// add by chengl Apr.10.2013
		/// </summary>
		/// <returns></returns>
		public DataSet GetAllCarContainNoValid()
		{
			DataSet ds = new DataSet();
			string sql = @"SELECT car.car_id,car.car_name,car.isState,car.cs_id,cs.isState as csisState
				FROM [Car_Basic] car 
				left join car_serial cs on car.cs_id=cs.cs_id
				order by car.car_id";
			ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			return ds;
		}

		/// <summary>
		/// 根据车型ID取车型基本表信息
		/// </summary>
		/// <param name="carid">车型ID</param>
		/// <returns></returns>
		public DataSet GetCarDetailById(int carid)
		{
			string sql = @"select car.*,cs.CsName,cs.CsShowName,cs.AllSpell from dbo.vCar_Basic car
left join Car_Serial cs on car.Cs_Id=cs.cs_id where car.Car_Id=@carid";
			SqlParameter[] parameters = { 
											new SqlParameter("@carid",SqlDbType.Int)  
										};
			parameters[0].Value = carid;
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, parameters);
			return ds;
		}


		/// <summary>
		/// 取所有有效车型 根据子品牌ID、车型ID排序
		/// </summary>
		/// <returns></returns>
        public DataSet GetAllCarOrderbyCs(int serialId)
        {
            string sql = string.Format(@"SELECT  cs.cs_id, cs.cs_name, car.car_id, car.car_name
                            FROM    car_basic car
                                    LEFT JOIN car_serial cs ON car.cs_id = cs.cs_id
                            WHERE   {0} car.isState = 1
                                    AND cs.isState = 1
                            ORDER BY cs.cs_id, car.car_id", serialId > 0 ? "cs.cs_Id = @SerialId AND" : "");
            SqlParameter[] paramsters =
		    {
		        new SqlParameter("@SerialId",SqlDbType.Int)
		    };
            paramsters[0].Value = serialId;
            return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, paramsters);
        }

		/// <summary>
		/// 取子品牌下所有车型(包括停销)
		/// </summary>
		/// <param name="csID"></param>
		/// <returns></returns>
		public IList<Car_BasicEntity> Get_Car_BasicByCsID(int csID)
		{
			IList<Car_BasicEntity> Obj = new List<Car_BasicEntity>();
			string sqlStr = @"SELECT  car.Car_Id, car.Car_ProduceState, car.Car_SaleState, car.Car_Name,
                                        car.SpellFirst, car.car_ReferPrice, car.Car_YearType, car.IsState,
                                        car.IsLock, car.OLdCar_Id, car.CreateTime, car.UpdateTime, cs.cs_id,
                                        cs.cs_name, cs.cs_ShowName, cs.allSpell AS cs_AllSpell, cb.cb_id,
                                        cb.cb_name, ISNULL(cp.cp_id, 0) AS cp_id, cp.cp_name, cmb.bs_name
                                FROM    dbo.Car_Basic car
                                        LEFT JOIN dbo.Car_Serial cs ON car.cs_id = cs.cs_id
                                        LEFT JOIN dbo.Car_Brand cb ON cs.cb_id = cb.cb_id
                                        LEFT JOIN dbo.Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                        LEFT JOIN dbo.Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                                        LEFT JOIN dbo.Car_Producer cp ON cb.cp_id = cp.cp_id
                                WHERE car.isState = 1
                                        AND cs.isState = 1
                                        AND cb.isState = 1
                                        AND cp.isState = 1
                                        AND car.cs_id = @cs_id";
			SqlParameter[] _param ={
                                      new SqlParameter("@cs_id",SqlDbType.Int)
                                  };
			_param[0].Value = csID;
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						Obj.Add(Populate_Car_BasicEntity_FromDr(dr));
					}
				}
			}
			catch
			{
				return Obj;
			}
			return Obj;
		}

		/// <summary>
		/// 根据车型ID取车型数据
		/// </summary>
		/// <param name="carID"></param>
		/// <returns></returns>
		public Car_BasicEntity Get_Car_BasicByCarID(int carID)
		{
			Car_BasicEntity Obj = new Car_BasicEntity();
			string sqlStr = @"SELECT  car.Car_Id, car.Car_ProduceState, car.Car_SaleState, car.Car_Name,
                                    cmb.bs_Name, car.SpellFirst, car.car_ReferPrice, car.Car_YearType,
                                    car.IsState, car.IsLock, car.OLdCar_Id, car.CreateTime, car.UpdateTime,
                                    cs.cs_id, cs.cs_name, cs.cs_ShowName, cs.allSpell AS cs_AllSpell,
                                    cb.cb_id, cb.cb_name, ISNULL(cp.cp_id, 0) AS cp_id, cp.cp_name
                            FROM    dbo.Car_Basic car
                                    LEFT JOIN dbo.Car_Serial cs ON car.cs_id = cs.cs_id
                                    LEFT JOIN dbo.Car_Brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN dbo.Car_MasterBrand_Rel cmr ON cmr.cb_Id = cb.cb_id
                                    LEFT JOIN dbo.Car_MasterBrand cmb ON cmb.bs_Id = cmr.bs_Id
                                    LEFT JOIN dbo.Car_Producer cp ON cb.cp_id = cp.cp_id
                            WHERE car.isState = 1
                                    AND cs.isState = 1
                                    AND cb.isState = 1
                                    AND cp.isState = 1
                                    AND car.car_id = @car_id";
			SqlParameter[] _param ={
                                      new SqlParameter("@car_Id",SqlDbType.Int)
                                  };
			_param[0].Value = carID;
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					Obj = Populate_Car_BasicEntity_FromDr(ds.Tables[0].Rows[0]);
				}
			}
			catch
			{
				return Obj;
			}
			return Obj;
		}

		/// <summary>
		/// 取所有车型
		/// </summary>
		/// <returns></returns>
		public IList<Car_BasicEntity> Get_Car_BasicAll()
		{
			IList<Car_BasicEntity> Obj = new List<Car_BasicEntity>();
			string sqlStr = @"SELECT  car.Car_Id, car.Car_ProduceState, car.Car_SaleState, car.Car_Name,
                                        car.SpellFirst, car.car_ReferPrice, car.Car_YearType, car.IsState,
                                        car.IsLock, car.OLdCar_Id, car.CreateTime, car.UpdateTime, cs.cs_id,
                                        cs.cs_name, cs.cs_ShowName, cs.allSpell AS cs_AllSpell, cb.cb_id,
                                        cb.cb_name, ISNULL(cp.cp_id, 0) AS cp_id, cp.cp_name
                                FROM    dbo.Car_Basic car
                                        LEFT JOIN dbo.Car_Serial cs ON car.cs_id = cs.cs_id
                                        LEFT JOIN dbo.Car_Brand cb ON cs.cb_id = cb.cb_id
                                        LEFT JOIN dbo.Car_Producer cp ON cb.cp_id = cp.cp_id
                                WHERE car.isState = 1
                                        AND cs.isState = 1
                                        AND cb.isState = 1
                                        AND cp.isState = 1";
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						Obj.Add(Populate_Car_BasicEntity_FromDr(dr));
					}
				}
			}
			catch
			{
				return Obj;
			}
			return Obj;
		}

		/// <summary>
		/// 获取所有车型的数据
		/// </summary>
		/// <returns></returns>
		public DataSet GetCarData()
		{
            string sql = @"SELECT  car.Car_Id AS CarId, car.Car_Name AS CarName,
                                    car.Car_SaleState AS SaleState, car.car_ReferPrice AS ReferPrice,
                                    car.Car_YearType AS CarYear, cx.Engine_Exhaust AS Exhaust,
                                    cx.Body_Type AS BodyType,
                                    cx.UnderPan_TransmissionType AS TransmissionType,
                                    cx.Car_RepairPolicy AS RepairPolicy, cs.cs_id AS SerialId,
                                    cs.cs_name AS SerialName, cs.cs_ShowName AS SerialShowName,
                                    cs.cs_CarLevel AS Carlevel, cs.allSpell AS SeialAllSpell,
                                    cp.Cp_ShortName AS ProducerName, cb.cb_Name AS BrandName,
                                    cb.cb_Country AS Cp_Country
                            FROM    dbo.Car_Basic car
                                    LEFT JOIN dbo.Car_Extend_Item cx ON car.Car_Id = cx.Car_Id
                                    LEFT JOIN dbo.Car_Serial cs ON car.cs_id = cs.cs_id
                                    LEFT JOIN dbo.Car_Brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN dbo.Car_Producer cp ON cb.cp_id = cp.cp_id
                            WHERE   car.isState = 1
                                    AND cs.isState = 1
                                    AND cb.isState = 1
                                    AND cp.isState = 1
                                    AND car.Car_SaleState <> '停销'";


            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			return ds;
		}

		/// <summary>
		/// 取车型的对比列表
		/// </summary>
		/// <param name="carID"></param>
		/// <returns></returns>
		public DataSet GetCarCompareListByCarID(int carID)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine(" select top 50 ccl.carID,ccl.cCarID,ccl.total,car.Car_Name, ");
			sqlBuilder.AppendLine(" car.Car_SaleState,car.Car_YearType,car.car_ReferPrice, ");
			sqlBuilder.AppendLine(" cs.cs_id,cs.cs_name,cs.cs_showname,cs.allSpell  ");
			sqlBuilder.AppendLine(" from Car_CompareList ccl ");
			sqlBuilder.AppendLine(" left join dbo.Car_Basic car on ccl.cCarID=car.car_id ");
			sqlBuilder.AppendLine(" left join car_serial cs on car.cs_id=cs.cs_id ");
			sqlBuilder.AppendLine(" where car.isState=1 and cs.isState=1 and ccl.carID=@carid ");
			sqlBuilder.AppendLine(" order by ccl.total desc ");
			SqlParameter[] _param ={
                                      new SqlParameter("@carId",SqlDbType.Int)
                                  };
			_param[0].Value = carID;
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlBuilder.ToString(), _param);
			return ds;
		}

		/// <summary>
		/// 根据参数ID取车型扩展参数
		/// </summary>
		/// <param name="paramid">参数ID</param>
		/// <returns></returns>
		public DataSet GetCarParamEx(int paramid)
		{
			string sql = @"select car.car_id,car.cs_id,cdb.pvalue,cl.classvalue as car_SaleState
                                from dbo.Car_relation car
                                left join car_serial cs on car.cs_id=cs.cs_id
                                left join dbo.CarDataBase cdb on car.car_id=cdb.carid and cdb.paramid=@paramid
                                left join class cl on cl.classid = car.car_SaleState
                                where car.isState=0 and cs.isState=0 and cdb.pvalue is not null";
			SqlParameter[] _param ={
                                      new SqlParameter("@paramid",SqlDbType.Int)
                                  };
			_param[0].Value = paramid;
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
			return ds;
		}
		/// <summary>
		/// 得到CNCAP市区工况油耗； CNCAP市郊工况油耗；易车测试油耗的测试接口,对答疑
		/// </summary>
		/// <returns></returns>
		public DataSet GetOilMessageByAskInterface()
		{
			// modified by chengl Mar.22.2013 牛B的SQL随风去吧
			return new DataSet();
			/*
			string sql = @"select CarId,[788] as Perf_MeasuredFuel
                            ,[855] as Perf_CNCAPSuburbsfuelconsumption
                            ,[854] as Perf_CNCAPfuelconsumption
                            ,[783] as Perf_ShiQuYouHao 
                            ,[784] as Perf_ShiJiaoYouHao
                            ,[862] as Perf_Prototypetestingfuelconsumption from 
                            (
	                            select carId,ParamId,cast(Pvalue as decimal(18,2)) as Pvalue from dbo.CarDataBase 
	                            where paramId = 854 or paramId = 855 or paramId = 788 or paramId = 783 or paramId = 784 or paramId = 862
                            ) as p
                            pivot
                            (
	                            sum(Pvalue)
	                            FOR ParamId IN
	                            ( [788], [855], [854],[783], [784], [862] )
                            ) AS pvt
                            order by CarId";

			try
			{
				using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql))
				{
					if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return null;

					return ds;
				}
			}
			catch
			{
				return null;
			}
			*/
		}

		/// <summary>
		/// 取车型全部参数项
		/// </summary>
		/// <param name="carID">车型ID</param>
		/// <returns></returns>
		public Dictionary<int, string> GetCarAllParamByCarID(int carID)
		{
			Dictionary<int, string> dic = new Dictionary<int, string>();
            string catchkey = "GetCarAllParamByCarID_" + carID.ToString();
            object objGetCarAllParamByCarId= null;
            CacheManager.GetCachedData(catchkey, out objGetCarAllParamByCarId);
            if (objGetCarAllParamByCarId == null)
		    {
                string sql = "select carid,paramid,pvalue from dbo.CarDataBase where carid=@carID";
                SqlParameter[] _param ={
                                      new SqlParameter("@carID",SqlDbType.Int)
                                  };
                _param[0].Value = carID;
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int paramid = 0;
                        if (int.TryParse(dr["paramid"].ToString(), out paramid))
                        {
                            if (paramid > 0 && dr["pvalue"].ToString().Trim() != "" && !dic.ContainsKey(paramid))
                            {
                                dic.Add(paramid, dr["pvalue"].ToString().Trim());
                            }
                        }
                    }
                }
                CacheManager.InsertCache(catchkey, dic, WebConfig.CachedDuration);
            }
            else
            {
                dic = (Dictionary<int, string>)objGetCarAllParamByCarId;
            }
			return dic;
		}
        /// <summary>
        /// 车型所有选配参数
        /// </summary>
        /// <param name="carID"></param>
        /// <returns></returns>
        public Dictionary<int, Dictionary<string, double>> GetCarAllParamOptionalByCarID(int carID)
        {
            Dictionary<int, Dictionary<string, double>> dic = new Dictionary<int, Dictionary<string, double>>();
            string catchkey = "GetCarAllParamOptionalByCarID_" + carID.ToString();
            object objGetCarAllParamByCarId = null;
            CacheManager.GetCachedData(catchkey, out objGetCarAllParamByCarId);
            if (objGetCarAllParamByCarId == null)
            {
                string sql = "select CarId,PropertyId,PropertyValue,Price from dbo.CarDataBaseOptional where CarId=@carID order by PropertyId,Price";
                SqlParameter[] _param ={
                                      new SqlParameter("@carID",SqlDbType.Int)
                                  };
                _param[0].Value = carID;
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int paramid = 0;
                        if (int.TryParse(dr["PropertyId"].ToString(), out paramid))
                        {
                            if (paramid > 0 && dr["PropertyValue"].ToString().Trim() != "")
                            {
                                if (!dic.ContainsKey(paramid))
                                {
                                    Dictionary<string, double> dicCs = new Dictionary<string, double>();
                                    dicCs.Add(dr["PropertyValue"].ToString().Trim(), ((dr["Price"]) == DBNull.Value) ? 0 : Convert.ToDouble(dr["Price"]));
                                    dic.Add(paramid, dicCs);
                                }
                                else
                                {
                                    if (!dic[paramid].ContainsKey(dr["PropertyValue"].ToString().Trim()))
                                    { dic[paramid].Add(dr["PropertyValue"].ToString().Trim(), ((dr["Price"]) == DBNull.Value) ? 0 : Convert.ToDouble(dr["Price"])); }
                                }
                            }
                        }
                    }
                }
                CacheManager.InsertCache(catchkey, dic, WebConfig.CachedDuration);
            }
            else
            {
                dic = (Dictionary<int, Dictionary<string, double>>)objGetCarAllParamByCarId;
            }
            return dic;
        }
        /// <summary>
        /// 获取车款参数值
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="paramId"></param>
        /// <returns></returns>
        public string GetCarParamValue(int carId, int paramId)
		{ 
			string sql = @"SELECT  TOP 1 pvalue
							FROM    dbo.CarDataBase
							WHERE   carid = @CarId
									AND ParamId = @ParamId";
			SqlParameter[] _param ={
                                      new SqlParameter("@CarId",SqlDbType.Int),
									  new SqlParameter("@ParamId",SqlDbType.Int)
                                  };
			_param[0].Value = carId;
			_param[1].Value = paramId;
			return ConvertHelper.GetString(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param));
		}
		/// <summary>
		/// 根据多个车款 参数值
		/// </summary>
		/// <param name="arrCarId"></param>
		/// <param name="paramId"></param>
		/// <returns></returns>
		public Dictionary<int, string> GetCarParamValueByCarIds(int[] arrCarId, int paramId)
		{
			Dictionary<int, string> dic = new Dictionary<int, string>();
			string sql = string.Format(@"SELECT  carid, paramid, pvalue
							FROM    dbo.CarDataBase cdb
							WHERE   ParamId = @ParamId AND CarId IN({0})", string.Join(",", arrCarId));
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString,
				CommandType.Text,
				sql,
				new SqlParameter("@ParamId", paramId));
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int carId = 0;
                    if (int.TryParse(dr["carid"].ToString(), out carId))
					{
                        if (carId > 0 && dr["pvalue"].ToString().Trim() != "" && !dic.ContainsKey(carId))
						{
                            dic.Add(carId, dr["pvalue"].ToString().Trim());
						}
					}
				}
			}
			return dic;
		}

		/// <summary>
		/// 取子品牌车型
		/// </summary>
		/// <param name="csid">子品牌ID 大于0为取特定子品牌 等于0为取全部子品牌</param>
		/// <returns></returns>
		public DataSet GetCarListGroupbyYear(int csid,bool isOnlySale)
		{
			DataSet ds = new DataSet();
			string sql = @"select car.car_id,car.car_name,cs.cs_id, 
                    (case when car.Car_YearType is null then '无' else CONVERT(varchar(50),car.Car_YearType)+ ' 款' end) as Car_YearType,cei.Engine_Exhaust 
                     ,(case when cei.UnderPan_TransmissionType like '%手动' then 1 
                     when cei.UnderPan_TransmissionType like '%自动' then 2 
                     when cei.UnderPan_TransmissionType like '%手自一体' then 3  
                     else 4 end) as TransmissionType 
                     ,car.car_ReferPrice,car.Car_YearType as CarYearType,car.Car_SaleState
				  from car_basic car 
                     left join car_serial cs on car.cs_id=cs.cs_id 
                     left join dbo.Car_Extend_Item cei on car.car_id = cei.car_id 
                     where car.isState=1 and cs.isState=1 {0}  {1}
                     order by cs_id,car.Car_YearType desc,TransmissionType,car.car_ReferPrice";
			string saleStr = "";
			if (isOnlySale)
			{
				saleStr = " and car.Car_SaleState='在销' and car.Car_ProduceState='在产' ";
			}
			if (csid > 0)
				sql = string.Format(sql, "and cs.cs_id=@csid", saleStr);
			else
				sql = string.Format(sql, "", saleStr);
			SqlParameter[] param = { new SqlParameter("@csid", SqlDbType.Int) };
			param[0].Value = csid;
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, param);
			return ds;
		}

		#region 车型对比

		/// <summary>
		/// 取车型参数
		/// </summary>
		/// <param name="carIDs"></param>
		/// <returns></returns>
		public DataSet GetCarParamForCompare(string carIDs)
		{
			//            string sql = @" select cdb.CarId,cdb.ParamId,cdb.Pvalue,pl.AliasName 
			//								from dbo.CarDataBase cdb 
			//								left join paramList pl on cdb.paramid=pl.paramid 
			//								where cdb.carid in ({0}) order by cdb.carid,cdb.ParamId ";
			//            string sql = @" select cdb.CarId,cdb.ParamId,cdb.Pvalue
			//								from dbo.CarDataBase cdb 
			//								where cdb.carid in ({0}) ";
			//            DataSet ds = new DataSet();
			//            if (carIDs.Length <= 300)
			//            {
			//                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, string.Format(sql, BitAuto.Utils.StringHelper.SqlFilter(carIDs)));
			//            }
			SqlParameter[] param = { 
								new SqlParameter("@carids",SqlDbType.Structured)
								   };
			DataSet ds = new DataSet();

			#region modified by chengl Dec.17.2013 改成表值参数传递
			DataTable dt = new DataTable();
			if (!string.IsNullOrEmpty(carIDs))
			{
				List<int> carList = new List<int>();
				dt.Columns.Add("CarID", typeof(int));
				string[] carArray = carIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (carArray.Length > 0)
				{
					foreach (string idStr in carArray)
					{
						int id = 0;
						if (int.TryParse(idStr.Trim(), out id))
						{
							if (id > 0 && !carList.Contains(id))
							{ carList.Add(id); }
						}
					}
				}
				if (carList.Count > 0)
				{
					foreach (int id in carList)
					{
						DataRow dr = dt.NewRow();
						dr["CarID"] = id;
						dt.Rows.Add(dr);
					}
				}
			}
			#endregion

			if (dt.Rows.Count > 0)
			{
				param[0].Value = dt;
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.StoredProcedure, "[Dts_CarParamForCompare_New]", param);
			}
			return ds;
		}

        /// <summary>
        /// 获取车款选装参数
        /// </summary>
        /// <param name="carIds"></param>
        /// <returns></returns>
        public DataSet GetCarOptionalForCompare(string carIDs)
        {
            SqlParameter[] param = {
                                new SqlParameter("@carids",SqlDbType.Structured)
                                   };
            DataSet ds = new DataSet();

            #region modified by chengl Dec.17.2013 改成表值参数传递
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(carIDs))
            {
                List<int> carList = new List<int>();
                dt.Columns.Add("CarID", typeof(int));
                string[] carArray = carIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (carArray.Length > 0)
                {
                    foreach (string idStr in carArray)
                    {
                        int id = 0;
                        if (int.TryParse(idStr.Trim(), out id))
                        {
                            if (id > 0 && !carList.Contains(id))
                            { carList.Add(id); }
                        }
                    }
                }
                if (carList.Count > 0)
                {
                    foreach (int id in carList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["CarID"] = id;
                        dt.Rows.Add(dr);
                    }
                }
            }
            #endregion

            if (dt.Rows.Count > 0)
            {
                param[0].Value = dt;
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.StoredProcedure, "[Dts_CarOptionalForCompare]", param);
            }
            return ds;
        }

        /// <summary>
        /// 取所有参数ID与英文名对于表
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllParamAliasName()
		{
			string sql = "select ParamId,AliasName from dbo.ParamList where isState=1";
			DataSet ds = new DataSet();
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
			return ds;
		}

		/// <summary>
		/// 取车型基本信息
		/// </summary>
		/// <param name="carids"></param>
		/// <returns></returns>
		public DataSet GetCarBaseInfoForCompare(string carIDs)
		{
			string sql = @" select cs.cs_id,cs.cs_name,cs.Cs_ShowName,
																			cs.AllSpell,car.car_id,car.car_name,
																			car.Car_ProduceState,car.Car_SaleState,
																			car.car_ReferPrice,car.Car_YearType
																			from car_basic car
																			left join car_serial cs on car.cs_id=cs.cs_id
																			where car.isState=1 and cs.isState=1 
																			and car.car_id in ({0}) order By charindex(','+ rtrim(ltrim(str(car_id))) +',', ',{0},')";
			DataSet ds = new DataSet();
			if (carIDs.Length <= 240)
			{
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, string.Format(sql, BitAuto.Utils.StringHelper.SqlFilter(carIDs)));
			}
			return ds;
		}

		/// <summary>
		/// 根据子品牌ID 取旗下车型及PV
		/// </summary>
		/// <param name="csIDs"></param>
		/// <returns></returns>
		public DataSet GetCarBaseInfoForCompareByCsIDs(string csIDs)
		{
			string sql = string.Format(@"SELECT  ccp.PVSum AS Pv_SumNum, cs.cs_id, cs.cs_name, cs.Cs_ShowName,
												cs.AllSpell, car.car_id, car.car_name, car.Car_ProduceState,
												car.Car_SaleState, car.car_ReferPrice, car.Car_YearType
										FROM    Car_Basic car WITH ( NOLOCK )
												LEFT JOIN car_serial cs WITH ( NOLOCK ) ON car.cs_id = cs.cs_id
												LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON ccp.CarId = car.car_id
										WHERE   car.isState = 1
												AND cs.isState = 1
												AND car.cs_id IN ( {0} )
										ORDER BY CHARINDEX(',' + LTRIM(RTRIM(STR(cs.cs_id))) + ',', ',{0},'),
												Pv_SumNum DESC", BitAuto.Utils.StringHelper.SqlFilter(csIDs));
			DataSet ds = new DataSet();
			if (csIDs.Length <= 50)
			{
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			}
			return ds;
		}

		/// <summary>
		/// 获取所有非停销车型信息
		/// </summary>
		/// <returns></returns>
		public DataSet GetAllCar()
		{
			string sqlStr = @"SELECT Car_Id,car.Cs_Id,Car_Name,Car_YearType,car_ReferPrice,d.Pvalue as exhaus FROM Car_relation car
							LEFT JOIN dbo.Car_Serial cs ON cs.cs_Id=car.Cs_Id
							LEFT JOIN CarDataBase d ON car.Car_Id=d.CarId AND ParamId=785
							WHERE car.IsState=0 AND car.car_SaleState<>96 and cs.CsSaleState<>'停销'";
			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
		}
		/// <summary>
		/// 根据车型ID获取所有参数带参数名称
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DataSet GetParamListByCarId(int id)
		{
			string sql = "select p.paramid,paramname,pvalue from paramlist p right join cardatabase cdb on p.paramid=cdb.paramid where carid=@carid";
			SqlParameter[] _param ={
                                      new SqlParameter("@carid",SqlDbType.Int)
                                  };
			_param[0].Value = id;
			return BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
		}
		/// <summary>
		/// 根据ClassID集合取值
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DataSet GetClassValueById(string classIds)
		{
			string sql = "select classid,classvalue from [class] where classid in (" + classIds + ")";
			//SqlParameter[] _param ={
			//                          new SqlParameter("@classIds",SqlDbType.VarChar)
			//                      };
			//_param[0].Value = classIds;
			return BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
		}
		#endregion
		/// <summary>
		/// 获取车型信息 根据子品牌ID
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public DataSet GetAllCarInfoForSerialSummary(int serialId)
		{
			string sql = @"SELECT  car.car_id, car.car_name, car.car_ReferPrice, car.Car_YearType,
									car.Car_ProduceState, car.Car_SaleState, cs.cs_id, cei.Engine_Exhaust,
									cei.UnderPan_TransmissionType, ccp.PVSum AS Pv_SumNum, cs.cs_name,
									cs.allSpell,cei.Body_Type
							FROM    dbo.Car_Basic car WITH ( NOLOCK )
									LEFT JOIN dbo.Car_Extend_Item cei WITH ( NOLOCK ) ON car.car_id = cei.car_id
									LEFT JOIN Car_serial cs WITH ( NOLOCK ) ON car.cs_id = cs.cs_id
									LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON car.Car_Id = ccp.CarId
							WHERE   car.isState = 1
									AND cs.isState = 1
									AND cs.cs_Id = @serialId";

			SqlParameter[] _params = { 
										 new SqlParameter("@serialId", SqlDbType.Int) 								 
									 };
			_params[0].Value = serialId;

			return BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
		}
		/// <summary>
		/// 获取同级别热门车款 （不包含某子品牌）
		/// </summary>
		/// <param name="carLevel"></param>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public DataSet GetHotCarForCompare(string carLevel, int serialId)
		{
			string sql = @"SELECT TOP 15
									cs.cs_id, cs.cs_Name, cs.cs_ShowName, car.car_id, car.car_name,
									car.Car_YearType, ccp.PVSum AS Pv_SumNum
							FROM    dbo.Car_Basic car WITH ( NOLOCK )
									LEFT JOIN Car_serial cs WITH ( NOLOCK ) ON car.cs_id = cs.cs_id
									LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON car.Car_Id = ccp.CarId
							WHERE   car.isState = 1
									AND cs.cs_CarLevel = @carLevel
									AND cs.isState = 1
									AND cs.cs_Id <> @serialId
							ORDER BY Pv_SumNum DESC, car.Car_Id DESC";
			SqlParameter[] _params = { 
										 new SqlParameter("@carLevel", SqlDbType.VarChar),
										 new SqlParameter("@serialId", SqlDbType.Int) 
									 };
			_params[0].Value = carLevel;
			_params[1].Value = serialId;
			return BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
		}

		/// <summary>
		/// 获取子品牌 下 热门车型 最新年款 最热门车
		/// </summary>
		/// <param name="serialId">子品牌Id</param>
		/// <returns></returns>
		public DataSet GetHotCarBySerialId(int serialId)
		{
			string sql = @" SELECT  car.car_id, car.car_name, car.Car_YearType, ccp.PVSum AS Pv_SumNum
							FROM    car_basic car WITH ( NOLOCK )
									LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON ccp.CarId = car.car_id
							WHERE   car.isState = 1
									AND car.cs_id = @serialId
							ORDER BY car.Car_YearType DESC, Pv_SumNum DESC";

			SqlParameter[] _params = { 
 										 new SqlParameter("@serialId", SqlDbType.Int)
									 };
			_params[0].Value = serialId;
			return BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
		}
		/// <summary>
		/// 获取 子品牌下所有车款
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public DataSet GetCarBaseDataBySerialId(int serialId, bool isAll = false)
		{
			string sql = @" SELECT  car.Car_Id,car.Car_Name,car.Car_YearType,car.car_ReferPrice,cei.Engine_Exhaust,cei.UnderPan_TransmissionType,car.Car_SaleState,car.Car_ProduceState
FROM    car_basic car
        LEFT JOIN dbo.Car_Extend_Item cei ON car.car_id = cei.car_id
WHERE   car.isState = 1 {0} AND car.Cs_Id=@serialId";
			if (isAll)
				sql = string.Format(sql, "");
			else
				sql = string.Format(sql, "AND car.Car_SaleState='在销'");

			SqlParameter[] _params = { 
 										 new SqlParameter("@serialId", SqlDbType.Int)
									 };
			_params[0].Value = serialId;
			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
		}
	}
}
