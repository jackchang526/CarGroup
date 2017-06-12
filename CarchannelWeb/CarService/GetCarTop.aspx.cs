using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.CarService
{
	public partial class GetCarTop : PageBase
	{
		private int carID = 0;
		private int tagID = 0;
		protected string html = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			Response.Clear();
			#region Get Param And Content
			this.GetPageParam();
			if (carID > 0 && tagID > 0)
			{
				// modified by chengl Oct.17.2011 车型头文件按1000分目录
				string subDir = Convert.ToString(carID / 1000);
				if (tagID == 16)
				{
					// 车型报价头
					// html = base.GetCommonNavigation("CarPrice\\" + subDir, carID);
					html = base.GetCommonNavigation("CarPrice", carID);
				}
				else if (tagID == 31)
				{
					// html = base.GetCommonNavigation("CarUcar\\" + subDir, carID);
					html = base.GetCommonNavigation("CarUcar", carID);
				}
				else if (tagID == 37)
				{
					// html = base.GetCommonNavigation("CarJiangJia\\" + subDir, carID);
					html = base.GetCommonNavigation("CarJiangJia", carID);
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
			if (this.Request.QueryString["carID"] != null && this.Request.QueryString["carID"].ToString() != "")
			{
				string carIDStr = this.Request.QueryString["carID"].ToString().Trim();
				if (int.TryParse(carIDStr, out carID))
				{ }
				else
				{
					carID = 0;
				}
			}

			// 栏目ID
			if (this.Request.QueryString["tagName"] != null && this.Request.QueryString["tagName"].ToString() != "")
			{
				string tagName = this.Request.QueryString["tagName"].ToString().Trim().ToLower();
				if (tagName == "carprice")
				{ tagID = 16; }
				else if (tagName == "carucar")
				{ tagID = 31; }
				else if (tagName == "carjiangjia")
				{ tagID = 37; }
				else { }
			}
		}

	}
}