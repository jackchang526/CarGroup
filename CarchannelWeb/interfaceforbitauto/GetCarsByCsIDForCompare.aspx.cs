using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class GetCarsByCsIDForCompare : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();
		private int csID = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.CheckParam();
				this.GetCarsDataByCsIDForCompare();
				Response.Write("var cars = '" + sb.ToString() + "';whenGetCarDataByCsID();");
			}
		}

		private void CheckParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string csIDStr = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(csIDStr, out csID))
				{ }
			}
		}

		private void GetCarsDataByCsIDForCompare()
		{
			if (csID > 0)
			{
				DataSet ds = base.GetAllCarInfo();
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					DataRow[] drs = ds.Tables[0].Select(" cs_id = " + csID.ToString() + " ");
					if (drs != null && drs.Length > 0)
					{
						foreach (DataRow dr in drs)
						{
							if (sb.Length > 1)
							{
								sb.Append("|" + dr["car_id"].ToString() + "^" + dr["car_name"].ToString() + "^" + dr["cs_name"].ToString() + "^" + dr["allspell"].ToString().ToLower());
							}
							else
							{
								sb.Append(dr["car_id"].ToString() + "^" + dr["car_name"].ToString() + "^" + dr["cs_name"].ToString() + "^" + dr["allspell"].ToString().ToLower());
							}
						}
					}
				}
			}
		}
	}
}