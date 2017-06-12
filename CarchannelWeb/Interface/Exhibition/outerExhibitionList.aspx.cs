using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Exhibition
{
	public partial class outerExhibitionList : System.Web.UI.Page
	{
		private BitAuto.CarChannel.BLL.Exhibition exhibitionBLL = new BitAuto.CarChannel.BLL.Exhibition();

		protected void Page_Load(object sender, EventArgs e)
		{
			DataSet ds = exhibitionBLL.GetExhibitionList();

			if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
			{
				Response.Write("");
				Response.End();
			}

			string xmlContent = ds.GetXml();

			Response.Write(xmlContent);
			Response.End();

		}
	}
}