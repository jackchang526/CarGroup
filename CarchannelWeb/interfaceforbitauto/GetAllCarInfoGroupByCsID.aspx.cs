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
	public partial class GetAllCarInfoGroupByCsID : InterfacePageBase
	{
		private DataSet ds = new DataSet();
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<Params Time=\"" + DateTime.Now.ToShortDateString() + "\">");
				GetAllCarInfoGroupDataByCsID();
				GenerateXML();
				sb.Append("</Params >");
				Response.Write(sb.ToString());
			}
		}

		private void GetAllCarInfoGroupDataByCsID()
		{
			string sql = " select cr.cs_id,cr.car_id,cr.car_name,cr.Car_YearType,cei.Engine_Exhaust ";
			sql += " ,(case when cei.UnderPan_TransmissionType like '%手动' then 1 ";
			sql += " when cei.UnderPan_TransmissionType like '%自动' then 2 ";
			sql += " when cei.UnderPan_TransmissionType like '%手自一体' then 3 ";
			sql += " else 4 end) as TransmissionType ";
			sql += " ,cr.car_ReferPrice ";
			sql += " ,bat1.bitautoTestURL as carTestURL,bat2.bitautoTestURL as csTestURL ";
			sql += " from dbo.Car_basic cr ";
			sql += " left join dbo.Car_Serial cs on cr.cs_id=cs.cs_id ";
			sql += " left join dbo.Car_Brand cb on cs.cb_id = cb.cb_id ";
			sql += " left join dbo.Car_Extend_Item cei on cr.car_id = cei.car_id ";
			sql += " left join dbo.BitAutoTest bat1 on cr.car_id = bat1.car_id ";
			sql += " left join dbo.BitAutoTest bat2 on cs.cs_id = bat2.cs_id ";
			sql += " where cr.isState = 1 and cs.isState = 1 and cb.isState = 1 ";
			//sql += " and cr.Car_SaleState<>'停销' ";
			sql += " order by cr.cs_id,cr.Car_YearType desc,Engine_Exhaust,TransmissionType,cr.car_ReferPrice ";
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
		}

		private void GenerateXML()
		{
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					string csid = "";
					string carYear = "";
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						if (csid != ds.Tables[0].Rows[i]["cs_id"].ToString())
						{
							csid = ds.Tables[0].Rows[i]["cs_id"].ToString();
							if (i != 0)
							{
								sb.Append("</Serial>");
							}
							// 易车测试
							sb.Append("<Serial CsID=\"" + csid + "\" CsTestURL=\"" + ds.Tables[0].Rows[i]["csTestURL"].ToString() + "\" >");
						}
						sb.Append("<Item  CarID=\"" + ds.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
						carYear = ds.Tables[0].Rows[i]["Car_YearType"].ToString();
						if (carYear.Length > 3)
						{
							carYear = " " + carYear.Substring(2, 2) + "款";
						}
						sb.Append(" Engine_Exhaust=\"" + ds.Tables[0].Rows[i]["Engine_Exhaust"].ToString() + "\" ");
						sb.Append(" CarTestURL=\"" + ds.Tables[0].Rows[i]["carTestURL"].ToString() + "\" ");
						sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["car_name"].ToString()) + carYear + "\" />");
					}
					sb.Append("</Serial>");
				}

			}
		}
	}
}