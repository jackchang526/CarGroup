using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	public partial class GetHotCompareSerial : PageBase
	{
		private string csName = string.Empty;
		private int csID = 0;
		private string temp = string.Empty;
		private string tempCurrent = string.Empty;
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				GetHotCompareSerialByCsID();
			}
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string tempCsID = this.Request.QueryString["csID"].ToString().Trim();
				if (int.TryParse(tempCsID, out csID))
				{ }
			}

			if (this.Request.QueryString["csName"] != null && this.Request.QueryString["csName"].ToString() != "")
			{
				csName = this.Request.QueryString["csName"].ToString().Trim();
			}
		}

		private void GetHotCompareSerialByCsID()
		{
			if (csID > 0)
			{
				StringBuilder sb = new StringBuilder();
				List<EnumCollection.SerialHotCompareData> lshcd = base.GetSerialHotCompareByCsID(csID, 5);
				if (lshcd.Count > 0)
				{
					sb.AppendLine("<h3><span>" + csName + "网友都用它和谁比</span> <a href=\"http://car.bitauto.com/chexingduibi/\">车型对比>></a></h3>");
					sb.AppendLine("<div class=\"tab_list\"></div>");
					sb.AppendLine("<div id=\"rank_model_box\">");
					sb.Append("<ol class=\"hot_ranking\">");
					foreach (EnumCollection.SerialHotCompareData shcd in lshcd)
					{
						sb.AppendLine("<li><em><a target=\"_blank\" href=\"/" + shcd.CompareCsAllSpell.ToLower() + "/\">");
						sb.AppendLine(shcd.CompareCsShowName + "</a></em>");
						sb.AppendLine("<span>" + shcd.CompareCsPriceRange + "</span></li>");
					}
					sb.AppendLine("</ol>");
					sb.AppendLine("</div>");
				}
				Response.Write(sb.ToString());
			}
		}
	}
}