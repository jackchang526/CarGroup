using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.AppCode
{
	public class PageHelper
	{
		public static void SetPageCache(int interval)
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