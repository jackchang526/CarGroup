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
	  
	public partial class SerialToCarForCompare1 : PageBase
	{
		private int csID = 0;
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string strCsID = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(strCsID, out csID))
				{
					GetCarListByCsID();
					Response.Write(sb.ToString());
				}
			}
		}

		private void GetCarListByCsID()
		{
			if (csID > 0)
			{
				DataSet ds = base.GetCarIDAndNameForCS(csID, WebConfig.CachedDuration);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					// sb.Append("<iframe style=\"z-index:-1;position:absolute;width:100%;height:500px;_filter:alpha(opacity=0);opacity=0;border-style:none;\"></iframe>");
					sb.Append("<h3>点击车型加入对比</h3>");
					string currentCarYear = "";
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						if (currentCarYear != ds.Tables[0].Rows[i]["Car_YearType"].ToString())
						{
							if (currentCarYear != "")
							{
								sb.Append("</ul></dd></dl><div class=\"line\"></div>");
							}
							currentCarYear = ds.Tables[0].Rows[i]["Car_YearType"].ToString();
							sb.Append("<dl><dt>" + currentCarYear + "款</dt>");
							sb.Append("<dd><ul >");
						}
						sb.Append("<li><a href=\"javascript:addCarToCompareFromlist(" + ds.Tables[0].Rows[i]["car_id"].ToString().Trim() + ",'" + ds.Tables[0].Rows[i]["car_name"].ToString().Trim() + "','" + ds.Tables[0].Rows[i]["cs_name"].ToString().Trim() + "');\">" + ds.Tables[0].Rows[i]["car_name"].ToString().Trim() + "</a></li>");
					}
					sb.Append("</ul></dd></dl><a class=\"pop_close\" href=\"javascript:showCarListDivAndResetPosition(false,'pop_compare_forcarlist');\">关闭</a>");
				}
			}
		}

	}
}