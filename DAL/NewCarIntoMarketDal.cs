using BitAuto.CarChannel.Common;
using BitAuto.Utils.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.DAL
{
    public class NewCarIntoMarketDal
    {
        /// <summary>
        /// 新车上市已上市车系
        /// </summary>
        /// <returns></returns>
        public IList<Model.NewCarIntoMarketEntity> GetNewCarIntoMarketList(bool isMarketed)
        {
            string sqlStr;
            if (isMarketed)
            {
                sqlStr = "SELECT [CsId],[Type],[YearType],[MarketDay] FROM [NewCarIntoMarket] WHERE [Type] = 1 OR [Type] = 2 ORDER BY MarketDay DESC,CsId";
            }
            else {
                sqlStr = "SELECT [CsId],[Type],[YearType],[MarketDay] FROM [NewCarIntoMarket] WHERE [Type] = 3 OR [Type] = 4 ORDER BY  case when MarketDay is null then '9999-12-31' else MarketDay end ,CsId";
            }

            IList<Model.NewCarIntoMarketEntity> Obj = new List<Model.NewCarIntoMarketEntity>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Obj.Add(DataRowToEntity(dr));
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
        /// 新车上市全部车系
        /// </summary>
        /// <returns></returns>
        public IList<Model.NewCarIntoMarketEntity> GetNewCarIntoMarketList()
        {
            string sqlStr = "SELECT [CsId],[Type],[YearType],[MarketDay],[UpdateTime] FROM [NewCarIntoMarket] ORDER BY MarketDay DESC,CsId";
            IList<Model.NewCarIntoMarketEntity> Obj = new List<Model.NewCarIntoMarketEntity>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Obj.Add(DataRowToEntity(dr));
                    }
                }
            }
            catch
            {
                return Obj;
            }
            return Obj;
        }
         
        private Model.NewCarIntoMarketEntity DataRowToEntity(DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            Model.NewCarIntoMarketEntity entity = new Model.NewCarIntoMarketEntity();
            entity.CsId = ((row["CsId"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["CsId"]);
            entity.Type = ((row["Type"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["Type"]);
            entity.YearType = ((row["YearType"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["YearType"]);
            entity.MarketDay = ((row["MarketDay"]) == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["MarketDay"]);
            //entity.UpdateTime = ((row["UpdateTime"]) == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["UpdateTime"]);
            return entity;
        }
    }
}
