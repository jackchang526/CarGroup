using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using BitAuto.CarChannel.Common;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.DAL
{
	public class ProduceAndSellDataDal
	{
		public Dictionary<string,DataTable> GetSellData(DateTime dataDate,List<int> dataTypeList)
		{
			Dictionary<string,DataTable> dataRes = new Dictionary<string,DataTable>();
			string curDate = dataDate.ToString("yyyy-MM-01");
			string preMonthDate = dataDate.AddMonths(-1).ToString("yyyy-MM-01");
			string preYearDate = dataDate.AddYears(-1).ToString("yyyy-MM-01");
			//先查询当月的数据
			DataSet ds = GetCurMonthSellData(curDate, dataTypeList);
			dataRes.Add(curDate,ds.Tables[0]);

			//取子品牌ID
			string serialIdList = "";
			int idCounter = 0;
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				idCounter++;
				string serialId = Convert.ToString(row["CsId"]);
				serialIdList += serialId;
				if (idCounter < ds.Tables[0].Rows.Count)
					serialIdList += ",";
			}

			//取前一月的数据
			ds = GetOtherMonthSellData(preMonthDate, serialIdList);
			dataRes[preMonthDate] = ds.Tables[0];
			ds = GetOtherMonthSellData(preYearDate, serialIdList);
			dataRes[preYearDate] = ds.Tables[0];

			//当前累计
			ds = GetSellDataCount(dataDate, serialIdList);
			dataRes["CurrentCount"] = ds.Tables[0];
			//同期累计
			ds = GetSellDataCount(dataDate.AddYears(-1), serialIdList);
			dataRes["PreYearCount"] = ds.Tables[0];

			return dataRes;
		}

		/// <summary>
		/// 查询当月的数据，取前10
		/// </summary>
		/// <param name="dataDate"></param>
		/// <param name="dataType"></param>
		/// <returns></returns>
		private DataSet GetCurMonthSellData(string dataDate,List<int> dataTypeList)
		{
			StringBuilder sqlBuilder = new StringBuilder();

			//查询级别范围
			string levelCondition = "";
			if (dataTypeList.Count == 1)
			{
				levelCondition = " b.carlevel=" + dataTypeList[0];
			}
			else
			{
				levelCondition = " b.carlevel IN (";
				int levelCounter = 0;
				foreach (int level in dataTypeList)
				{
					levelCounter++;
					levelCondition += level.ToString();
					if (levelCounter < dataTypeList.Count)
						levelCondition += ",";
				}
				levelCondition += ")";
			}

			sqlBuilder.AppendLine("SELECT TOP 10 a.CsId,b.csShowName,b.allSpell,a.SellNum FROM dbo.CarProduceAndSellData a");
			sqlBuilder.AppendLine("INNER JOIN dbo.Car_Serial b ON a.CsId = b.cs_Id AND b.IsState=0");
			if (levelCondition.Length > 0)
				sqlBuilder.AppendLine("AND " + levelCondition);
			sqlBuilder.AppendLine("WHERE DataDate='" + dataDate + "' ORDER BY a.SellNum DESC");
			

			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlBuilder.ToString());
		}

		/// <summary>
		/// 取其他月份的销售数据
		/// </summary>
		/// <param name="dataDate"></param>
		/// <param name="serialIdList"></param>
		/// <returns></returns>
		private DataSet GetOtherMonthSellData(string dataDate,string serialIdList)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine("SELECT CsId,SellNum FROM dbo.CarProduceAndSellData");
			sqlBuilder.AppendLine("WHERE DataDate='" + dataDate + "'");
			if (serialIdList.Length > 0)
				sqlBuilder.AppendLine("AND CsId IN (" + serialIdList + ")");
			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlBuilder.ToString());
		}

		/// <summary>
		/// 统计数据
		/// </summary>
		/// <param name="curDate"></param>
		/// <param name="serialIdList"></param>
		/// <returns></returns>
		private DataSet GetSellDataCount(DateTime curDate,string serialIdList)
		{
			string startDate = new DateTime(curDate.Year, 1, 1).ToString("yyyy-MM-dd");
			string endDate = curDate.ToString("yyyy-MM-01");
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine("SELECT CsId,SUM(SellNum) AS SellCount FROM dbo.CarProduceAndSellData");
			sqlBuilder.AppendLine("WHERE DataDate BETWEEN '" + startDate + "' AND '" + endDate + "'");
			if (serialIdList.Length > 0)
				sqlBuilder.AppendLine("AND CsId IN (" + serialIdList + ")");
			sqlBuilder.AppendLine("GROUP BY CsId");

			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlBuilder.ToString());
		}



		/// <summary>
		/// 按厂商查询销量
		/// </summary>
		/// <param name="pId"></param>
		/// <returns></returns>
		public DataSet GetQueryDataByProducer(int pId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine("SELECT TOP 6 a.DataDate,SUM(a.SellNum) AS SellCount");
			sqlBuilder.AppendLine("FROM dbo.CarProduceAndSellData a");
			sqlBuilder.AppendLine("INNER JOIN dbo.Car_Serial b ON a.CsId=b.cs_Id");
			sqlBuilder.AppendLine("INNER JOIN dbo.Car_Brand c ON c.cb_Id=b.cb_Id");
			sqlBuilder.AppendLine("WHERE c.cp_Id = @pId");
			sqlBuilder.AppendLine("GROUP BY a.DataDate");
			sqlBuilder.AppendLine("ORDER BY a.DataDate DESC");

			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlBuilder.ToString(), new SqlParameter("@pId", pId));
		}

		/// <summary>
		/// 按品牌查询销量
		/// </summary>
		/// <param name="bId"></param>
		/// <returns></returns>
		public DataSet GetQueryDataByBrand(int bId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine("SELECT TOP 6 a.DataDate,SUM(a.SellNum) AS SellCount");
			sqlBuilder.AppendLine("FROM dbo.CarProduceAndSellData a");
			sqlBuilder.AppendLine("INNER JOIN dbo.Car_Serial b ON a.CsId=b.cs_Id");
			sqlBuilder.AppendLine("WHERE b.cb_Id = @bId");
			sqlBuilder.AppendLine("GROUP BY a.DataDate");
			sqlBuilder.AppendLine("ORDER BY a.DataDate DESC");

			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlBuilder.ToString(), new SqlParameter("@bId", bId));
		}

		/// <summary>
		/// 按子品牌查询销量
		/// </summary>
		/// <param name="sId"></param>
		/// <returns></returns>
		public DataSet GetQueryDataBySerial(int sId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine("SELECT TOP 6 DataDate,SUM(SellNum) AS SellCount");
			sqlBuilder.AppendLine("FROM dbo.CarProduceAndSellData");
			sqlBuilder.AppendLine("WHERE CsId = @sId");
			sqlBuilder.AppendLine("GROUP BY DataDate");
			sqlBuilder.AppendLine("ORDER BY DataDate DESC");

			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlBuilder.ToString(), new SqlParameter("@sId", sId));
		}

		/// <summary>
		/// 获取前六个月所有销量
		/// </summary>
		/// <returns></returns>
		public DataSet GetQueryDataAll()
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine("SELECT TOP 6 DataDate,SUM(SellNum) AS SellCount");
			sqlBuilder.AppendLine("FROM dbo.CarProduceAndSellData");
			sqlBuilder.AppendLine("GROUP BY DataDate");
			sqlBuilder.AppendLine("ORDER BY DataDate DESC");

			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlBuilder.ToString());
		}

		/// <summary>
		/// 获取有数据的最后一个月
		/// </summary>
		/// <returns></returns>
		public List<DateTime> GetLastDataMonths()
		{
			List<DateTime> lastMonths = new List<DateTime>();
			string sqlStr = "SELECT TOP 12 DataDate FROM CarProduceAndSellData GROUP BY DataDate ORDER BY datadate DESC";
			DataSet monthsDs = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
			if(monthsDs != null && monthsDs.Tables.Count > 0)
			{
				foreach (DataRow row in monthsDs.Tables[0].Rows)
				{
					if (row["DataDate"] != null && row["DataDate"] != DBNull.Value)
						lastMonths.Add(Convert.ToDateTime(row["DataDate"]));
				}
			}

			return lastMonths;
		}
	}
}
