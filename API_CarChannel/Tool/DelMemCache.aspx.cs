using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannelAPI.Web.Tool
{
	/// <summary>
	/// 临时清memcache使用 add by chengl Aug.26.2013
	/// </summary>
	public partial class DelMemCache : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				if (this.Request.QueryString["key"] != null
					&& this.Request.QueryString["key"].ToString() != ""
					&& this.Request.QueryString["password"] != null
					&& this.Request.QueryString["password"].ToString() == "bitauto"
					)
				{
					string key = this.Request.QueryString["key"].ToString().Trim();
					MemCache.DelMemCacheByKey(key);
					Response.Write(DateTime.Now.ToString());
				}
			}
		}
	}
}