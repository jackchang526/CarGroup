using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.CarInfo
{
	/// <summary>
	/// 二手车 车型指导价 购车总价(潘伟江 临时)
	/// </summary>
	public partial class CarPriceInfo : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();
		private DataSet dsCar = new DataSet();
		private string dept = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				base.SetPageCache(10);
				GetPageParam();
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<CarList>");
				if (dept == "ucar")
				{
					GetAllCar();
					GetCarPrices();
				}
				sb.Append("</CarList>");
				Response.Write(sb.ToString());
			}
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["dept"] != null && this.Request.QueryString["dept"].ToString() != "")
			{
				dept = this.Request.QueryString["dept"].ToString().ToLower();
			}
		}

		private void GetAllCar()
		{
			string sqlCar = @" select car.car_id,car.car_name,car.cs_id,car.car_ReferPrice
                                        from dbo.Car_relation car
                                        left join car_serial cs on car.cs_id=cs.cs_id
                                        where car.isState=0 and cs.isState=0 ";
			dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlCar);
		}

		private void GetCarPrices()
		{
			if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
			{

				foreach (DataRow dr in dsCar.Tables[0].Rows)
				{
					int carID = int.Parse(dr["car_id"].ToString());
					CarPriceComputer priceComputer = new CarPriceComputer(carID);
					priceComputer.ComputeCarPrice();
					sb.Append("<Car ID=\"" + carID.ToString() + "\"");
					sb.Append(" ReferPrice=\"" + dr["car_ReferPrice"].ToString() + "\" ");
					sb.Append(" CarTotalPrice=\"" + priceComputer.FormatTotalPrice.Trim().Replace("万", "") + "\" ");
					sb.Append("/>");
				}
			}
		}
	}
}