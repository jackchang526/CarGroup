using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.CarService
{
	public partial class GetSerialTop : PageBase
	{
		private int csID = 0;
		private int tagID = 0;
		protected string html = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			Response.Clear();
			#region Get Param And Content
			this.GetPageParam();
			if (csID > 0 && tagID > 0)
			{
				if (tagID == 12)
				{
					// 子品牌报价
					html = base.GetCommonNavigation("CsPrice", csID);
				}
				else if (tagID == 23)
				{
					// CMS不带面包削
					html = base.GetCommonNavigation("CsCMSNews", csID);
				}
				else if (tagID == 26)
				{
					// Ucar
					html = base.GetCommonNavigation("CsUcar", csID);
				}
				else if (tagID == 38)
				{
					// Ucar
					html = base.GetCommonNavigation("CsJiangJia", csID);
				}
				else
				{
					html = "";
				}
			}
			#endregion
		}

		// 取参数
		private void GetPageParam()
		{
			// 子品牌ID
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string csIDStr = this.Request.QueryString["csID"].ToString().Trim();
				if (int.TryParse(csIDStr, out csID))
				{ }
				else
				{
					csID = 0;
				}
			}

			// 栏目ID
			if (this.Request.QueryString["tagName"] != null && this.Request.QueryString["tagName"].ToString() != "")
			{
				string tagName = this.Request.QueryString["tagName"].ToString().Trim().ToLower();
				if (tagName == "csprice")
				{ tagID = 12; }
				else if (tagName == "csnewsnocrumb")
				{ tagID = 23; }
				else if (tagName == "csucar")
				{ tagID = 26; }
				else if (tagName == "csjiangjia")
				{ tagID = 38; }
				else { }
			}

		}

	}
}