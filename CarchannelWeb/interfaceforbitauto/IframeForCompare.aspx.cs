using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class IframeForCompare : InterfacePageBase
	{
		private string carIDs = string.Empty;
		protected string carIDAndName = string.Empty;
		private ArrayList alCarIDs = new ArrayList();
		private DataSet dsCar = new DataSet();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.GetCompareCarIDs();
			}
		}

		/// <summary>
		/// 取需要对比的车型ID
		/// </summary>
		private void GetCompareCarIDs()
		{
			if (this.Request.QueryString["carIDs"] != null && this.Request.QueryString["carIDs"].ToString() != "")
			{
				dsCar = base.GetAllCarInfo();

				string tempcarIDs = this.Request.QueryString["carIDs"].ToString();
				if (tempcarIDs.IndexOf(",") > 0)
				{
					string[] arrCarIDs = tempcarIDs.Split(',');
					if (arrCarIDs.Length > 0)
					{
						int loop = 1;
						for (int i = 0; i < arrCarIDs.Length; i++)
						{
							if (loop > 10)
							{ break; }
							int iCarID = 0;
							if (int.TryParse(arrCarIDs[i], out iCarID))
							{
								if (iCarID > 0)
								{
									GetCarInfoByID(iCarID);
									loop++;
								}
							}
						}
					}
				}
				else
				{
					int iCarID = 0;
					if (int.TryParse(tempcarIDs, out iCarID))
					{
						GetCarInfoByID(iCarID);
					}
				}
			}
		}

		private void GetCarInfoByID(int carID)
		{
			if (carID > 0 && dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = dsCar.Tables[0].Select(" car_id = '" + carID.ToString() + "' ");
				if (drs != null && drs.Length > 0)
				{
					if (carIDAndName != "")
					{
						carIDAndName += "|id" + drs[0]["car_id"].ToString() + "," + drs[0]["car_name"].ToString().Trim();
					}
					else
					{
						carIDAndName = "id" + drs[0]["car_id"].ToString() + "," + drs[0]["car_name"].ToString().Trim();
					}
				}
			}
		}
	}
}