using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.DAL
{
	public class SerialAwardsDal
	{
		public static DataTable GetAward(int awardId)
		{
			const string sql = @"SELECT csa.Id,csa.AwardsName,csa.LogoUrl,csa.Summary,csa.OfficialUrl
FROM dbo.Car_SerialAwards csa  WHERE csa.Id = @AwardId";
			SqlParameter[] parameters =
			{
				new SqlParameter("@AwardId", SqlDbType.Int)
			};
			parameters[0].Value = awardId;
			var ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, parameters);
			return ds.Tables[0];
		}

		public static string[] GetYears(int awardId)
		{
			const string sql = @"SELECT csay.[Year] FROM dbo.Car_SerialAwardsYear csay WHERE csay.AwardsId = @AwardId ORDER BY csay.[Year] DESC";
			SqlParameter[] parameters =
			{
				new SqlParameter("@AwardId", SqlDbType.Int)
			};
			parameters[0].Value = awardId;
			var dt = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql.ToString(), parameters).Tables[0];
			if (dt == null || dt.Rows.Count == 0)
			{
				return null;
			}
			var strArray = dt.Rows.OfType<DataRow>().Select(m => m["Year"].ToString()).ToArray();
			return strArray;
		}

		public static DataTable GetCarSerials(int awardId, int year, int childAwardId)
		{
			const string sql = @"SELECT cs.cs_Id,cs.csShowName AS csName,csac.SerialName as csWriteName,cs.allSpell
FROM dbo.Car_SerialAwardsYear csay
inner join dbo.Car_SerialAwardsCar csac ON csac.AwardsYearId = csay.Id
left JOIN dbo.Car_Serial cs ON cs.cs_Id = csac.SerialId
WHERE csay.AwardsId = @AwardId AND csay.[Year] = @Year AND csac.ChildAwardsId = @ChildAwardId ORDER BY cs.cs_Id DESC";
			SqlParameter[] parameters =
			            {
			                new SqlParameter("@AwardId", SqlDbType.Int),
			                new SqlParameter("@Year", SqlDbType.Int),
			                new SqlParameter("@ChildAwardId", SqlDbType.Int)
			            };
			parameters[0].Value = awardId;
			parameters[1].Value = year;
			parameters[2].Value = childAwardId;
			var dt = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, parameters).Tables[0];
			if (dt == null || dt.Rows.Count == 0)
			{
				return null;				
			}
			return dt;
		}

		public static DataTable GetCars(int awardId, int year)
		{
			const string sql = @"SELECT csay.[Year],csay.AwardsId,csac.Id, csac.AwardsName AS ChildAwardName
FROM dbo.Car_SerialAwardsYear csay LEFT JOIN dbo.Car_SerialAwardsChild csac ON csac.AwardsYearId = csay.Id 
WHERE csay.AwardsId = @awardsId AND csay.[Year] = @year";
			SqlParameter[] parameters =
			            {
			                new SqlParameter("@awardsId", SqlDbType.Int),
			                new SqlParameter("@year", SqlDbType.Int)
			            };
			parameters[0].Value = awardId;
			parameters[1].Value = year;
			var dt = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, parameters).Tables[0];
			if (dt == null || dt.Rows.Count == 0)
			{
				return null;
			}
			return dt;
			
		}
	}

	public class SerialAwardmm
	{
		public int Year { get; set; }

		public int AwardsId { get; set; }

		public int ChildAwardId { get; set; }

		public string ChildAwardName { get; set; }

		public int CarSerialId { get; set; }

		public string CarSerialName { get; set; }
	}
}
