using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using System.Text;

namespace WirelessWeb.handlers
{
	/// <summary>
	/// GetCarPriceJson 的摘要说明
	/// </summary>
	public class GetCarPriceJson : WirelessPageBase, IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;

		int carId = 0;
		CarEntity ce;
		public void ProcessRequest(HttpContext context)
		{
			base.SetPageCache(10);
			context.Response.ContentType = "application/x-javascript";
			response = context.Response;
			request = context.Request;
			GetParamter();
			RenderJson();
		}
		private void GetParamter()
		{
			carId = ConvertHelper.GetInteger(request.QueryString["carid"]);
		}

		private void RenderJson()
		{
			StringBuilder sb = new StringBuilder();
			if (carId > 0)
			{
				ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
				if (ce != null && ce.Id > 0)
				{
					sb.AppendFormat("\"ID\":{0},\"YearType\":{1},\"Name\":\"{2}\",\"CarReferPrice\":{3},\"SerialShowName\":\"{4}\"",
						ce.Id, ce.CarYear, ce.Name, ce.ReferPrice, ce.Serial.ShowName);
				}
			}
			response.Write(string.Format("var tempInfo ={{{0}}}", sb.ToString()));
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