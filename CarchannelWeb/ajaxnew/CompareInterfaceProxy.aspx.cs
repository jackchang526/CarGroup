using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	public partial class CompareInterfaceProxy : PageBase
	{
		private int type = 1;       // 1：同价位，2：最热门
		private int carID = 0;
		private string carIDs = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.CheckPara();
				this.GetCompareCarData();
			}
		}

		private void CheckPara()
		{
			if (this.Request.QueryString["type"] != null && this.Request.QueryString["type"].ToString() != "")
			{
				string typeStr = this.Request.QueryString["type"].ToString();
				if (int.TryParse(typeStr, out type))
				{
					if (type < 1 || type > 2)
					{
						type = 1;
					}
				}
			}
			if (this.Request.QueryString["carID"] != null && this.Request.QueryString["carID"].ToString() != "")
			{
				string carIDStr = this.Request.QueryString["carID"].ToString();
				if (int.TryParse(carIDStr, out carID))
				{ }
			}
		}

		private void GetCompareCarData()
		{
			if (carID > 0)
			{
				// 同价位
				if (type == 1)
				{
					carIDs = carID.ToString();
					ArrayList alCars = base.GetTheSamePriceRangeCarsByCarID(carID, 10, 3);
					if (alCars != null && alCars.Count > 0)
					{
						foreach (string carids in alCars)
						{
							carIDs += "," + carids;
						}
					}
				}
				// 最热门
				if (type == 2)
				{
					Car_BasicEntity cbe = new Car_BasicEntity();
					carIDs = carID.ToString();
					string catchkeyCar = "CarSummary_Car" + carID.ToString();
					object carInfoByCarID = null;
					CacheManager.GetCachedData(catchkeyCar, out carInfoByCarID);
					if (carInfoByCarID == null)
					{
						cbe = (new Car_BasicBll()).Get_Car_BasicByCarID(carID);
						CacheManager.InsertCache(catchkeyCar, cbe, 60);
					}
					else
					{
						cbe = (Car_BasicEntity)carInfoByCarID;
					}

					List<EnumCollection.CarHotCompareData> lchcd = base.GetCarHotCompareByCarID(cbe.Cs_Id, carID, 6, false);
					if (lchcd.Count > 0)
					{
						foreach (EnumCollection.CarHotCompareData chcd in lchcd)
						{
							carIDs += "," + chcd.CompareCarID.ToString();
						}
					}
				}
			}
			Response.Redirect("/interfaceforbitauto/ForBitAutoCompare.aspx?isNewid=1&carids=" + carIDs);
		}
	}
}