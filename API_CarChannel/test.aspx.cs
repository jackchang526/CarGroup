using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using BitAuto.Beyond.Caching.RefreshCache;
using BitAuto.Utils;

namespace BitAuto.CarChannelAPI.Web
{
	public partial class test : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			int cityId = ConvertHelper.GetInteger(Request.QueryString["cityId"]);
			int csId = ConvertHelper.GetInteger(Request.QueryString["csid"]);
			RefreshCache rc = new RefreshCache();
			DataSet ds = rc.GetCacheData(cityId, csId);
			Response.Write(string.Format("csid={0},cityid={1}",csId,cityId));
			if (ds == null)
				Response.Write("查无结果");
			else if (ds.Tables.Count == 0)
				Response.Write("无结果表");
			else
				Response.Write(ds.Tables[0].Rows.Count.ToString() + "条经销商");
		}
	}
}