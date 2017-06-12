using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.DAL.Data
{
	public class TProducerDAL
	{
		public DataSet GetProducerDataById(int id)
		{
			string sqlStr = "SELECT Cp_Id,Cp_Name,Cp_ShortName,Cp_Country,Cp_Url,Spell,Cp_Introduction,IsState,cp_seoname FROM Car_Producer WHERE Cp_Id=@cpid and isState=1";
			DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr,new SqlParameter("@cpid",id));
			return ds;
		}
	}
}
