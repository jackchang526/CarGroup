using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class ForBitAutoCompare : InterfacePageBase
	{
		private string carIDs = string.Empty;
		private StringBuilder sb = new StringBuilder();
		protected string stringScript = string.Empty;
		private bool isNewID = false;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.GetOldCarIDForCompare();
			}
		}

		// 取需要对比的车型老ID
		private void GetOldCarIDForCompare()
		{
			if (this.Request.QueryString["isNewID"] != null && this.Request.QueryString["isNewID"].ToString() != "" && this.Request.QueryString["isNewID"].ToString() == "1")
			{
				isNewID = true;
			}

			//sb.Append("<script type=\"text/javascript\" language=\"javascript\" >");
			//sb.Append("CookieForCompare.setCookie(\"ActiveNewCompare\", \"\"); ");
			if (this.Request.QueryString["carIDs"] != null && this.Request.QueryString["carIDs"].ToString() != "")
			{
				carIDs = this.Request.QueryString["carIDs"].ToString();
				string[] carID = carIDs.Split(',');
				if (carID.Length > 0)
				{
					int lenght = carID.Length > 10 ? 10 : carID.Length;
					for (int i = 0; i < lenght; i++)
					{
						int id = 0;
						// modified by chengl Dec.29.2010
						if (int.TryParse(carID[i], out id))
						{
							if (id > 0)
							{
								this.GetNewIDAndCookie(id.ToString());
							}
						}
					}
				}
			}
			Response.Redirect("http://car.bitauto.com/chexingduibi/?carIDs=" + sb.ToString());
			//sb.Append("window.location.href = 'http://car.bitauto.com/chexingduibi/'; ");
			//sb.Append("</script >");
			//stringScript = sb.ToString();
		}

		// 取需要对比的新车型ID
		private void GetNewIDAndCookie(string carid)
		{
			DataSet ds = base.GetAllCarIDForCompare();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow[] drCurrentCar;
				if (isNewID)
				{
					// 新ID
					drCurrentCar = ds.Tables[0].Select(" car_id = " + carid);
				}
				else
				{
					// 老ID
					drCurrentCar = ds.Tables[0].Select(" oldcar_id = " + carid);
				}
				if (drCurrentCar.Length > 0)
				{
					// sb.Append("addCarToCompare('" + drCurrentCar[0]["car_id"].ToString() + "','" + drCurrentCar[0]["car_name"].ToString() + "','" + drCurrentCar[0]["cs_name"].ToString() + "');");
					if (sb.Length > 0)
					{
						sb.Append("," + drCurrentCar[0]["car_id"].ToString());
					}
					else
					{
						sb.Append(drCurrentCar[0]["car_id"].ToString());
					}
				}
			}
		}
	}
}