using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BusinessServiceAPI
{
	public class PageBaseForBusiness : System.Web.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		protected void SetPageCache(int interval)
		{
			if (HttpContext.Current != null && HttpContext.Current.Response.Cache != null)
			{
				HttpContext.Current.Response.Cache.SetNoServerCaching();
				HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
				HttpContext.Current.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(interval));
			}
		}
	}
}
