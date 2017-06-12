using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.Interface
{
	public partial class AsynAjax : OldPageBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Response.ContentType = "text/plain";

			string chartType = Request.QueryString["CT"];
			int CSID = int.Parse(Request.QueryString["CS"]);
			string jsChart = Request.QueryString["JT"];

			string charArray = string.Empty;
			if (!string.IsNullOrEmpty(chartType))
			{
				switch (chartType)
				{
					case "CarPR":
						#region PR图表

						charArray = base.GetFlashChartData(base.Get_PriceChartData(CSID, 30));


						#endregion
						break;
					case "CarPV":
						#region CarPV图表
						charArray = this.GetFlashChartData(base.Get_CarPVCharData(CSID, 30));
						#endregion
						break;
					case "SerPV":
						#region SerialPV图表
						charArray = this.GetFlashChartData(base.Get_SerialPVChartData(CSID, 30));
						#endregion
						break;
				}
			}
			else if (!string.IsNullOrEmpty(jsChart))
			{
				switch (jsChart)
				{
					case "CarPR":
						#region PR图表

						DataTable dt = base.Get_PriceChartData(CSID, 30);

						foreach (DataRow dr in dt.Rows)
						{
							charArray += string.Format(",[\"{0}\",{1}]", ((DateTime)dr[1]).ToString("yyyyMMdd"), dr[0]);
						}

						if (charArray.Length > 0)
						{
							charArray = charArray.Remove(0, 1);
						}

						charArray = string.Format("CarPvData=[{0}];", charArray);

						#endregion
						break;
					case "CarPV":
						#region CarPV图表
						charArray = string.Empty;
						dt = this.Get_CarPVCharData(CSID, 30);

						foreach (DataRow dr in dt.Rows)
						{
							charArray += string.Format(",[\"{0}\",{1}]", ((DateTime)dr[1]).ToString("yyyyMMdd"), dr[0]);
						}

						if (charArray.Length > 0)
						{
							charArray = charArray.Remove(0, 1);
						}

						charArray = string.Format("CarPrData=[{0}];", charArray);

						#endregion
						break;
					case "SerPV":
						#region SerialPV图表
						charArray = string.Empty;
						dt = this.Get_SerialPVChartData(CSID, 30);

						foreach (DataRow dr in dt.Rows)
						{
							charArray += string.Format(",[\"{0}\",{1}]", ((DateTime)dr[1]).ToString("yyyyMMdd"), dr[0]);
						}

						if (charArray.Length > 0)
						{
							charArray = charArray.Remove(0, 1);
						}

						charArray = string.Format("SerPvData=[{0}];", charArray);

						#endregion
						break;
				}
			}

			Response.ClearContent();
			Response.Write(charArray);
			Response.End();
		}
	}
}