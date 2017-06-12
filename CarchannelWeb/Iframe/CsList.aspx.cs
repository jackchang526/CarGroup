using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.Iframe
{
	public partial class CsList : PageBase
	{
		protected string dept = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParam();
			base.SetPageCache(10);
		}

		private void GetParam()
		{
			if (this.Request.QueryString["dept"] != null && this.Request.QueryString["dept"].ToString() != "")
			{
				dept = this.Request.QueryString["dept"].ToString().Trim().ToLower();
			}
		}
	}
}