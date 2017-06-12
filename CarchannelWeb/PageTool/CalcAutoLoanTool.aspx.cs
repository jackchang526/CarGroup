using System;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;

namespace BitAuto.CarChannel.CarchannelWeb.PageTool
{
	public partial class CalcAutoLoanTool : PageBase
	{
		protected int DownpaymentRatioInt = 0;
		protected int RepaymentYearsInt = 0;
		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);
			var carPrice = Request.QueryString["CarPrice"];
			if (carPrice != null)
			{
				int result = 0;
				if (Int32.TryParse(carPrice,out result))
				{
					hidCarPrice.Value = result.ToString();
				}
			}
			var downpaymentRatio = Request.QueryString["downpaymentRatio"];
			if (downpaymentRatio != null)
			{
				Int32.TryParse(downpaymentRatio, out DownpaymentRatioInt);
			}
			var repaymentYears = Request.QueryString["repaymentYears"];
			if (repaymentYears != null)
			{
				Int32.TryParse(repaymentYears, out RepaymentYearsInt);
			}

			int carId = ConvertHelper.GetInteger(Request.QueryString["carID"]);
			int serialId = ConvertHelper.GetInteger(Request.QueryString["serialId"]);
			int masterId = 0;
			if (carId > 0)
			{
				base.GetIDsByCarID(carId, out masterId, out serialId);
			}
			if (serialId > 0)
			{
				Car_SerialBaseEntity baseSerial = new Car_SerialBll().GetSerialBaseEntity(serialId);
				// add by chengl Dec.21.2011
				if (baseSerial != null)
				{
					masterId = baseSerial.MasterbrandId;
				    hidCsName.Value = baseSerial.SerialShowName;
				}
			}
			if (carId == 0)
				carId = -1;
			if (serialId == 0)
				serialId = -1;
			if (masterId == 0)
				masterId = -1;
			hidCarID.Value = carId.ToString();
			hidCsID.Value = serialId.ToString();
			hidBsID.Value = masterId.ToString();

		}
	}
}