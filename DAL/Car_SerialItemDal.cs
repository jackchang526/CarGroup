using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using BitAuto.Utils.Data;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.DAL
{
    public class Car_SerialItemDal
    {
        public Car_SerialItemDal()
        { }

        public Car_SerialItemEntity Populate_Car_SerialItemEntity_FromDr(DataRow row)
        {
            Car_SerialItemEntity Obj = new Car_SerialItemEntity();
            if (row != null)
            {
                Obj.Body_Doors = row["Body_Doors"].ToString().Trim();
                Obj.Car_MarketDate = row["Car_MarketDate"].ToString().Trim();
                Obj.Car_RepairPolicy = row["Car_RepairPolicy"].ToString().Trim();
                Obj.cs_Id = ((row["cs_Id"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["cs_Id"]);
                Obj.Engine_Exhaust = row["Engine_Exhaust"].ToString().Trim();
                Obj.Prices = row["Prices"].ToString().Trim();
                Obj.PricesRange = row["PricesRange"].ToString().Trim();
                Obj.ReferPriceRange = row["ReferPriceRange"].ToString().Trim();
                Obj.UnderPan_Num_Type = row["UnderPan_Num_Type"].ToString().Trim();
                Obj.NormalChargeTime = row.Table.Columns.Contains("NormalChargeTime") && row["NormalChargeTime"] != DBNull.Value ?  row["NormalChargeTime"].ToString() : string.Empty;
                Obj.BatteryLife = row.Table.Columns.Contains("BatteryLife") && row["BatteryLife"] != DBNull.Value ? row["BatteryLife"].ToString() : string.Empty;
            }
            else
            {
                return null;
            }
            return Obj;
        }

        /*
        public Car_SerialItemEntity Populate_Car_SerialItemEntity_FromDr(IDataReader dr)
        {
            Car_SerialItemEntity Obj = new Car_SerialItemEntity();
            Obj.Body_Doors = dr["Body_Doors"].ToString().Trim();
            Obj.Car_MarketDate = dr["Car_MarketDate"].ToString().Trim();
            Obj.Car_RepairPolicy = dr["Car_RepairPolicy"].ToString().Trim();
            Obj.cs_Id = ((dr["cs_Id"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["cs_Id"]);
            Obj.Engine_Exhaust = dr["Engine_Exhaust"].ToString().Trim();
            Obj.Prices = dr["Prices"].ToString().Trim();
            Obj.PricesRange = dr["PricesRange"].ToString().Trim();
            Obj.ReferPriceRange = dr["ReferPriceRange"].ToString().Trim();
            Obj.UnderPan_Num_Type = dr["UnderPan_Num_Type"].ToString().Trim();
            return Obj;
        }
        */
        /// <summary>
        /// 取所有子品牌扩展信息
        /// </summary>
        /// <returns></returns>
        public IList<Car_SerialItemEntity> Get_Car_SerialItemAll()
        {
            IList<Car_SerialItemEntity> Obj = new List<Car_SerialItemEntity>();
            string sqlStr = " select cs_Id,Body_Doors,Engine_Exhaust,Car_RepairPolicy, ";
            sqlStr += " Prices,Car_MarketDate,PricesRange,ReferPriceRange,UnderPan_Num_Type,NormalChargeTime,BatteryLife ";
            sqlStr += " from dbo.Car_Serial_Item ";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Obj.Add(Populate_Car_SerialItemEntity_FromDr(dr));
                    }
                }
            }
            catch
            {
                return Obj;
            }
            return Obj;
        }
        /*
        /// <summary>
        /// 根据子品牌ID取子品牌扩展信息
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public Car_SerialItemEntity Get_Car_SerialItemByCsID(int csID)
        {
            Car_SerialItemEntity Obj = new Car_SerialItemEntity();
            string sqlStr = " select cs_Id,Body_Doors,Engine_Exhaust,Car_RepairPolicy, ";
            sqlStr += " Prices,Car_MarketDate,PricesRange,ReferPriceRange ";
            sqlStr += " from dbo.Car_Serial_Item where cs_id=@cs_id ";
            SqlParameter[] _param ={
                                      new SqlParameter("@cs_id",SqlDbType.Int)
                                  };
            _param[0].Value = csID;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Obj = Populate_Car_SerialItemEntity_FromDr(ds.Tables[0].Rows[0]);
                }
            }
            catch
            {
                return Obj;
            }
            return Obj;
        }
        */
        /// <summary>
        /// 获取子品牌关注对应的子品牌
        /// </summary>
        /// <returns>子品牌列表</returns>
        public IList<Car_SerialItemEntity> Get_SerialToSerial(int topNum, int cs_id)
        {
            IList<Car_SerialItemEntity> Obj = new List<Car_SerialItemEntity>();
            string sqlStr = "AI_GetCarSerialDA";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@queryType","SerialToSerial"),
                new SqlParameter("@queryString",topNum.ToString()+"|"+cs_id.ToString())
            };
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.StoredProcedure, sqlStr, paras);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Obj.Add(Populate_Car_SerialItemEntity_FromDr(dr));
                }
            }
            catch
            {
                return Obj;
            }
            return Obj;
        }
    }
}
