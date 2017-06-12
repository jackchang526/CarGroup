using System;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface
{
	public partial class GetIndexData : InterfacePageBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string indexUrl = "http://index.bitauto.com/interface/getdata.aspx" + Request.Url.Query;
			string resText = String.Empty;
			try
			{
				WebClient wc = new WebClient();
				wc.Encoding = Encoding.UTF8;
				resText = wc.DownloadString(indexUrl);
			}
			catch (System.Exception ex)
			{

			}
			Response.Write(resText);
			Response.End();
		}
	}
}