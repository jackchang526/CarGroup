using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 通过子品牌ID跳转到新闻接口(王志腾)
	/// </summary>
	public partial class RedirectForNewsByCsID : InterfacePageBase
	{
		private int csID = 0;
		private int cateID = 1;
		// cateID 类别ID 1：评测文章

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.GetPageParam();
				if (csID > 0)
				{
					this.GetSerialCateNewID();
				}
			}
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string strCsID = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(strCsID, out csID))
				{

				}
			}

			if (this.Request.QueryString["cateID"] != null && this.Request.QueryString["cateID"].ToString() != "")
			{
				string strCateID = this.Request.QueryString["cateID"].ToString();
				if (int.TryParse(strCateID, out cateID))
				{
					if (cateID < 0)
					{
						cateID = 1;
					}
				}
			}
		}

		private void GetSerialCateNewID()
		{
			// 取评测文章
			if (cateID == 1)
			{
				int newID = base.GetPingCeNewIDByCsID(csID);
				if (newID > 0)
				{
					Response.Redirect("http://api.admin.bitauto.com/api/newslist.aspx?newsid=" + newID.ToString() + "&showtype=3");
				}
			}
			//CommonService cs = new CommonService();
			//Hashtable ht = cs.GetPingCeCompareSerialList();
		}
	}
}