using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.CarchannelWeb.Test
{
	public partial class TestForMemcache : System.Web.UI.Page
	{

		protected void Page_Load(object sender, EventArgs e)
		{
			string key = this.Request.QueryString["key"];
			if (!string.IsNullOrEmpty(key))
			{
				object obj = MemCache.GetMultipleMemCacheByKey(new List<string>() { key });
				if (obj != null)
				{
					Response.Write(key + ":" + obj.ToString());
				}
			}
		}
	}
}