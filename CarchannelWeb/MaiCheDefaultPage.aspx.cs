using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.CarChannel.CarchannelWeb
{
	public partial class MaiCheDefaultPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			HttpContext.Current.Response.Cache.SetNoServerCaching();
			HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
			HttpContext.Current.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(30));
		}
	}
}