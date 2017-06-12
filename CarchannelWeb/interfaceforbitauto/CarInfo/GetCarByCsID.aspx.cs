using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.CarInfo
{
	/// <summary>
	/// 刘培培、胡利军
	/// </summary>
	public partial class GetCarByCsID : InterfacePageBase
	{
		private string dept = "";
		private int csID = 0;
		private StringBuilder sb = new StringBuilder();
		private DataSet dsCarPrice = new DataSet();
		// http://imgsvr.bitauto.com/autochannel/CarImageService.aspx?dataname=carimagegroupaccount&showgroup=true

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				GetCarListByCsID();
				Response.Write(sb.ToString());
			}
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["dept"] != null && this.Request.QueryString["dept"].ToString() != "")
			{
				dept = this.Request.QueryString["dept"].ToString().ToLower();
			}

			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string strCsID = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(strCsID, out csID))
				{ }
			}
		}

		private void GetCarListByCsID()
		{
			if (dept == "bitauto")
			{
				// 车型报价范围
				dsCarPrice = base.GetAllCarPrice();

				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<CarList>");

				string sql = @" select cs.cs_id,cs.csname,car.car_id,car.car_name,car.Car_YearType,
car.car_ReferPrice,car.car_SaleState,car.car_ProduceState,
cdb1.pvalue as Engine_Exhaust,cdb2.pvalue as UnderPan_TransmissionType,cdb3.pvalue as Engine_ExhaustForFloat
from dbo.Car_relation car
left join dbo.Car_Serial cs on car.cs_id = cs.cs_id
left join dbo.CarDataBase cdb1 on car.car_id=cdb1.carID and cdb1.paramid=423
left join dbo.CarDataBase cdb2 on car.car_id=cdb2.carID and cdb2.paramid=712
left join dbo.CarDataBase cdb3 on car.car_id=cdb3.carID and cdb3.paramid=785
where car.isState=0 and cs.isState=0 and car.Car_SaleState<>96 {0} ";
				// order by car.car_id ";
				// sql += " and cr.Car_SaleState<>'停销' ";
				sql += " order by car.cs_id,car.Car_YearType desc,Engine_Exhaust,UnderPan_TransmissionType,car.car_ReferPrice ";
				if (csID > 0)
				{ sql = string.Format(sql, " and car.cs_id=" + csID.ToString()); }
				else
				{ sql = string.Format(sql, " "); }
				DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{

					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						sb.Append("<Car ID=\"" + dr["car_id"].ToString().Trim() + "\" ");
						sb.Append(" CsID=\"" + dr["cs_id"].ToString().Trim() + "\" ");
						sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(dr["csname"].ToString().Trim()) + "\" ");
						sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(dr["car_name"].ToString().Trim()) + "\" ");
						sb.Append(" CarYearType=\"" + dr["Car_YearType"].ToString().Trim() + "\" ");
						sb.Append(" CarEngineExhaust=\"" + dr["Engine_ExhaustForFloat"].ToString().Trim() + "\" ");
						sb.Append(" CarTransmissionType=\"" + dr["UnderPan_TransmissionType"].ToString().Trim() + "\" ");
						// sb.Append(" CarPicCount=\"" + GetCarPicCountByCarID(int.Parse(dr["car_id"].ToString().Trim())) + "\" "); 
						sb.Append(" CarPriceRange=\"" + GetCarPriceRangeByID(int.Parse(dr["car_id"].ToString().Trim())) + "\" ");
						sb.Append(" CarReferPrice=\"" + dr["car_ReferPrice"].ToString().Trim() + "\" />");
						// sb.Append("");
					}
				}

				sb.Append("</CarList>");

			}
		}

		/// <summary>
		/// 取车型报价区间
		/// </summary>
		/// <param name="carID"></param>
		/// <returns></returns>
		private string GetCarPriceRangeByID(int carID)
		{
			string result = string.Empty;
			if (dsCarPrice != null && dsCarPrice.Tables.Count > 0 && dsCarPrice.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = dsCarPrice.Tables[0].Select(" Id='" + carID.ToString() + "' ");
				if (drs.Length > 0)
				{
					try
					{
						decimal min = Math.Round(decimal.Parse(drs[0]["MinPrice"].ToString()), 2);
						decimal max = Math.Round(decimal.Parse(drs[0]["MaxPrice"].ToString()), 2);
						result = min.ToString() + "万-" + max.ToString() + "万";
					}
					catch
					{ }
				}
			}
			return result;
		}
	}
}