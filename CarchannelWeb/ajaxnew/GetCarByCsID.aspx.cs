using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	public partial class ajaxnew_GetCarByCsID : PageBase
	{
		#region Param
		private int csID = 0;
		private string type = string.Empty;
		private string name = string.Empty;
		private StringBuilder sb = new StringBuilder();
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);

			if (!this.IsPostBack)
			{
				// 检查参数
				this.CheckParam();
				if (csID > 0 && csID < 10000)
				{
					this.GetCarByCsID();
					if (name.ToLower() == "have")
					{
						if (string.IsNullOrEmpty(sb.ToString())) { Response.Write("var cartypeList=[];"); Response.End(); return; }
						Response.Write("var cartypeList=" + sb.ToString()); Response.End();
					}
					Response.Write(sb.ToString());
				}
			}
		}

		private void CheckParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string strCsID = this.Request.QueryString["csID"].ToString().Trim();
				if (int.TryParse(strCsID, out csID))
				{
				}
			}
			if (this.Request.QueryString["type"] != null && this.Request.QueryString["type"].ToString() != "")
			{
				type = this.Request.QueryString["type"].ToString().Trim();
			}

			name = string.IsNullOrEmpty(Request.QueryString["name"]) ? "" : Request.QueryString["name"].ToString();
		}

		private void GetCarByCsID()
		{
			DataSet ds = new DataSet();
			if (HttpContext.Current.Cache.Get("ajaxnew_GetCarByCsID_" + csID.ToString()) != null)
			{
				ds = (System.Data.DataSet)HttpContext.Current.Cache.Get("ajaxnew_GetCarByCsID_" + csID.ToString());
			}
			else
			{
				ds = base.GetCarReferPriceByCsID(csID);
				HttpContext.Current.Cache.Insert("ajaxnew_GetCarByCsID_" + csID.ToString(), ds, null, DateTime.Now.AddMinutes(20), TimeSpan.Zero);
			}
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				if (type.ToLower() == "json")
				{
					sb.Append("[");
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						if (i != 0)
						{
							sb.Append(",");
						}
						string carYear = ds.Tables[0].Rows[i]["Car_YearType"].ToString().Trim();
						sb.Append("{");
						sb.Append("\"ID\":" + ds.Tables[0].Rows[i]["car_id"].ToString().Trim() + ",");
						sb.Append("\"YearType\":\"" + ds.Tables[0].Rows[i]["Car_YearType"].ToString().Trim() + "\",");
						sb.Append("\"Name\":\"" + ds.Tables[0].Rows[i]["car_Name"].ToString().Trim() + "\",");
						string tempPrice = ds.Tables[0].Rows[i]["car_ReferPrice"].ToString().Trim() == "" ? "0" : ds.Tables[0].Rows[i]["car_ReferPrice"].ToString().Trim();
						sb.Append("\"CarReferPrice\":" + tempPrice + "");
						sb.Append("}");
					}
					sb.Append("]");
				}
			}
		}
	}
}