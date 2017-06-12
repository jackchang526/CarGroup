using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.DAL
{
	public class SerialCompareListDal
	{
		/// <summary>
		/// 获取子品牌最热门对比
		/// </summary>
		/// <param name="top"></param>
		/// <returns></returns>
		public DataSet GetHotSerialCompareList(int top)
		{
			if (top <= 0) top = 3;
			string sql = @"SELECT TOP (@TopN)
					cs.cs_Id AS csid,cs.cs_ShowName AS csShowName,cs.allSpell AS allspell,cs2.cs_Id AS tocsid,cs2.cs_ShowName AS tocsShowName,cs2.allSpell AS toAllspell,CompareCount
			FROM    Car_CsCompareList csc
					LEFT JOIN dbo.Car_Serial cs ON csc.CsID = cs.cs_Id
												   AND cs.IsState = 1
					LEFT JOIN dbo.Car_Serial cs2 ON csc.OtherCsID = cs2.cs_Id
													AND cs2.IsState = 1
			ORDER BY CompareCount DESC";
			SqlParameter[] _params = { 
										 new SqlParameter("@TopN", SqlDbType.Int)
									 };
			_params[0].Value = top;
			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
		}
	}
}
