using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface
{
	public partial class ExhibitionInterface : InterfacePageBase
	{
		public string xXML = "";
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(Request.QueryString["Ex_ID"]))
			{
				xXML = BitAuto.CarChannel.BLL.Exhibition.ExhibitionXMLString(Convert.ToInt32(Request.QueryString["Ex_ID"].ToString()), 10);
			}
		}
	}
}