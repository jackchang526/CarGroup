using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.DAL;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// GoCarTypeAndPriceForCar_Json 的摘要说明
	/// </summary>
	public class GoCarTypeAndPriceForCar_Json : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			//context.Response.Write("Hello World");
			context.Response.Write(GetCarBySerialTypeOfSerialJson());
		}

		/// <summary>
		/// 得到不包含车型的子品牌JSON对象
		/// </summary>
		/// <returns></returns>
		private string GetCarBySerialTypeOfSerialJson()
		{
			StringBuilder serialStrBuilder = new StringBuilder("var carData=");
			string sql = " select cs.cs_id,car.car_id,car.car_name ";
			sql += " from car_basic car ";
			sql += " left join car_serial cs on car.cs_id = cs.cs_id ";
			sql += " where car.isState=1 and cs.isState=1 and car.Car_SaleState<>'停销' ";
			sql += " order by cs.cs_id,car.car_id ";

			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				serialStrBuilder.Append("{");
				string currentCsID = "";
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					if (currentCsID != ds.Tables[0].Rows[i]["cs_id"].ToString().Trim())
					{
						if (currentCsID != "")
						{ serialStrBuilder.Append("],"); }
						currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString().Trim();
						serialStrBuilder.Append("s" + currentCsID + ":[");
					}
					else
					{
						serialStrBuilder.Append(",");
					}
					serialStrBuilder.Append("{id:" + ds.Tables[0].Rows[i]["car_id"].ToString().Trim() + ",name:\"" + ds.Tables[0].Rows[i]["car_name"].ToString().Trim().Replace("\"", "'") + "\"}");
				}
				serialStrBuilder.Append("]}");
			}
			else
			{
				return (serialStrBuilder.Append("[]")).ToString();
			}

			return serialStrBuilder.ToString();
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}