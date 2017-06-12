using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.Iframe
{
	public partial class CarGroup : PageBase
	{
		protected string contentHtml;
		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);
			string fileName = Path.Combine(WebConfig.DataBlockPath, "data\\SerialSet\\CarGroupByLevelAndPrice.html");
			if (File.Exists(fileName))
				contentHtml = File.ReadAllText(fileName);
			else
				contentHtml = String.Empty;
		}
	}
}