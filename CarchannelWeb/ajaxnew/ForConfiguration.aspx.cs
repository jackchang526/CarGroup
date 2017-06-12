using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	public partial class ForConfiguration : PageBase
	{
		private int carID = 0;
		private int showType = 0;        // 显示类型(0:显示全部页面输出,1:参数,2:配置,3:ajax参数及配置输出)
		private int operate = -1;          // 操作类型(-1:ajax请求,0:保存,1:打印)
		protected string carConfigData = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.CheckPara();
				this.GetCarConfiguration();
			}
		}

		private void CheckPara()
		{
			#region 取参数
			if (this.Request.QueryString["carID"] != null && this.Request.QueryString["carID"].ToString() != "")
			{
				try
				{
					carID = int.Parse(this.Request.QueryString["carID"].ToString());
				}
				catch
				{
					carID = 0;
				}
			}
			if (this.Request.QueryString["showType"] != null && this.Request.QueryString["showType"].ToString() != "")
			{
				try
				{
					showType = int.Parse(this.Request.QueryString["showType"].ToString());
					if (showType < 1 || showType > 4)
					{
						showType = 0;
					}
				}
				catch
				{
					showType = 0;
				}
			}
			if (this.Request.QueryString["operate"] != null && this.Request.QueryString["operate"].ToString() != "")
			{
				try
				{
					operate = int.Parse(this.Request.QueryString["operate"].ToString());
				}
				catch
				{
					operate = -1;
				}
			}
			#endregion
		}

		private void GetCarConfiguration()
		{
			if (carID > 0)
			{
				carConfigData = base.GetConfigurationNew(carID, showType, operate);
			}
		}
	}
}