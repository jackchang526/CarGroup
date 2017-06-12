using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetCarPriceComputer 的摘要说明
	/// </summary>
	public class GetCarPriceComputer : PageBase, IHttpHandler
	{
		private int carId = 0;
		private string callback = string.Empty;
		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(60);
			string msg = "{{\"msg\":\"{0}\",\"price\":{1}}}";
			context.Response.ContentType = "application/x-javascript";
			GetParmas(context);

			if (carId == 0)
			{
				msg = string.Format(msg, "参数错误", string.Empty);
				return;
			}
			CarPriceComputer priceComputer = new CarPriceComputer(carId);
			priceComputer.ComputeCarPrice();
			priceComputer.LoanPaymentYear = 3;
			priceComputer.ComputeCarAutoLoan();
			string priceJson = string.Format("{{\"totalprice\":\"{0}\",\"insurance\":\"{1}\",\"loanfirstdownpayments\":\"{2}\",\"loanmonthpayments\":\"{3}\"}}"
				, string.IsNullOrWhiteSpace(priceComputer.FormatTotalPrice) ? "暂无" : priceComputer.FormatTotalPrice
				, string.IsNullOrWhiteSpace(priceComputer.FormatInsurance) ? "暂无" : priceComputer.FormatInsurance
				, priceComputer.LoanFirstDownPayments > 0 ? ((double)(priceComputer.LoanFirstDownPayments + priceComputer.AcquisitionTax + priceComputer.Compulsory + priceComputer.Insurance + priceComputer.VehicleTax + priceComputer.Chepai) / 10000).ToString("F2") + "万" : "暂无"
				, priceComputer.LoanMonthPayments > 0 ? priceComputer.LoanMonthPayments + "元" : "暂无");

			msg = string.Format(msg, string.Empty, priceJson);
			if (!string.IsNullOrWhiteSpace(callback)) msg = string.Format("{0}({1})", callback, msg);
			context.Response.Write(msg);
		}

		private void GetParmas(HttpContext context)
		{
			carId = ConvertHelper.GetInteger(context.Request["carid"]);
			callback = context.Request["callback"];
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