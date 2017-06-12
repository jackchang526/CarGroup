using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class TransferCsIDToCarIDForCompare : PageBase
	{
		private int carCount = 3;
		private string carIDsInfoForCsID = "";
		private string urlForCompare = "http://car.bitauto.com/chexingduibi/";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.GetCarIDsByCsID();
				if (carIDsInfoForCsID != "")
				{
					urlForCompare += "?carids=" + carIDsInfoForCsID + "&isrec=1#CarHotCompareList";
				}
				Response.Redirect(urlForCompare);
			}
		}

		private void GetCarIDsByCsID()
		{
			if (this.Request.QueryString["Count"] != null && this.Request.QueryString["Count"].ToString() != "")
			{
				string strCount = this.Request.QueryString["Count"].ToString();
				if (int.TryParse(strCount, out carCount))
				{
					if (carCount < 0 || carCount > 10)
					{ carCount = 3; }
				}
			}

			if (this.Request.QueryString["csids"] != null && this.Request.QueryString["csids"].ToString() != "")
			{
				string strCsIDs = this.Request.QueryString["csids"].ToString();
				string[] arrCsIDs = strCsIDs.Split(',');
				if (arrCsIDs.Length > 0)
				{
					for (int i = 0; i < arrCsIDs.Length; i++)
					{
						if (i > 10)
						{ break; }
						int csid = 0;
						if (int.TryParse(arrCsIDs[i], out csid))
						{
							if (csid > 0)
							{
								DataSet ds = base.GetCarIDAndNameForCS(csid, WebConfig.CachedDuration);
								if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
								{
									for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
									{
										if (j >= carCount)
										{ break; }
										if (carIDsInfoForCsID != "" && carIDsInfoForCsID.Length > 0)
										{
											carIDsInfoForCsID += "," + ds.Tables[0].Rows[j]["car_id"].ToString().Trim();
											// carIDsInfoForCsID += "|" + "id" + ds.Tables[0].Rows[j]["car_id"].ToString() + "," + ds.Tables[0].Rows[j]["car_name"].ToString().Trim() + "," + ds.Tables[0].Rows[j]["Engine_Exhaust"].ToString().Trim() + "," + ds.Tables[0].Rows[j]["TransmissionType"].ToString().Trim();
										}
										else
										{
											carIDsInfoForCsID = ds.Tables[0].Rows[j]["car_id"].ToString().Trim();
											// carIDsInfoForCsID = "id" + ds.Tables[0].Rows[j]["car_id"].ToString() + "," + ds.Tables[0].Rows[j]["car_name"].ToString().Trim() + "," + ds.Tables[0].Rows[j]["Engine_Exhaust"].ToString().Trim() + "," + ds.Tables[0].Rows[j]["TransmissionType"].ToString().Trim();
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}