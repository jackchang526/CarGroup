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

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 车型列表(杨立锋)
	/// </summary>
	public partial class GetCarInfoByCsID : InterfacePageBase
	{
		private int csID = 0;
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<CarList>");
				GetPageParam();
				GetCarInfoDataByCsID();
				sb.Append("</CarList>");
				Response.Write(sb.ToString());
			}
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string tempCsID = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(tempCsID, out csID))
				{ }
			}
		}

		private void GetCarInfoDataByCsID()
		{
			DataSet dsCar = new DataSet();
			if (HttpContext.Current.Cache.Get("interfaceforbitauto_GetCarInfoByCsID" + csID.ToString()) != null)
			{
				dsCar = (DataSet)HttpContext.Current.Cache.Get("interfaceforbitauto_GetCarInfoByCsID" + csID.ToString());
			}
			else
			{
				string sql = @" select car.*
                                    from dbo.Car_Basic car
                                    left join Car_serial cs on car.cs_id = cs.cs_id
                                    where car.isState=1 and cs.isState=1 and car.car_saleState<>'停销' ";
				if (csID > 0)
				{
					sql += " and car.cs_id = " + csID.ToString();
				}

				dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				HttpContext.Current.Cache.Insert("interfaceforbitauto_GetCarInfoByCsID" + csID.ToString(), dsCar, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
			}


			if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsCar.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Car ID=\"" + dsCar.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
					sb.Append(" Name=\"" + dsCar.Tables[0].Rows[i]["car_name"].ToString().Trim().Replace("&", "&amp;") + "\" ");
					sb.Append(" CsID=\"" + dsCar.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					sb.Append(" ReferPrice=\"" + dsCar.Tables[0].Rows[i]["car_ReferPrice"].ToString() + "\" />");
				}
			}
		}
	}
}