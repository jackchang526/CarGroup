using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// GetCarInfoForCompare 的摘要说明
	/// </summary>
	public class GetCarInfoForCompare : IHttpHandler
	{
		private string carInfo = string.Empty;
		private int carID = 0; 

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/xml";
			if (context.Request.QueryString["carID"] != null && context.Request.QueryString["carID"].ToString() != "")
			{
				string carIDStr = context.Request.QueryString["carID"].ToString();
				if (int.TryParse(carIDStr, out carID))
				{
					if (carID > 0)
					{
						string catchkey = "GetCarInfoForCompare_" + carID.ToString();
						object getCarInfoForCompare_ = null;
						CacheManager.GetCachedData(catchkey, out getCarInfoForCompare_);
						if (getCarInfoForCompare_ == null)
						{
							CommonFunction cf = new CommonFunction();
							carInfo = cf.GetCarInfoForCompare(carID);
							CacheManager.InsertCache(catchkey, carInfo, 15);
						}
						else
						{
							carInfo = Convert.ToString(getCarInfoForCompare_);
						}

					}
				}
			}
			context.Response.Write(carInfo);
			context.Response.End(); 
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}