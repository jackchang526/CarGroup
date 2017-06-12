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
	/// 取概念车的子品牌 (傅强)
	/// </summary>
	public partial class GetSerialForNotion : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<Root>");
				GetSerialDataForNotion();
				sb.Append("</Root>");
				Response.Write(sb.ToString());
			}
		}

		private void GetSerialDataForNotion()
		{
			string sqlForNotion = " select cs_id,cs_name,cs_showname from car_serial where cs_carlevel = '概念车' and isState=1 order by cs_id ";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlForNotion);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Item ID=\"" + ds.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					sb.Append(" Name=\"" + ds.Tables[0].Rows[i]["cs_name"].ToString().Trim() + "\" ");
					sb.Append(" ShowName=\"" + ds.Tables[0].Rows[i]["cs_showname"].ToString().Trim() + "\" />");
				}
			}
		}
	}
}