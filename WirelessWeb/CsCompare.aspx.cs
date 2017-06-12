using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;

namespace WirelessWeb
{
	public partial class CsCompare : PageBase
	{
		#region member
		protected int csID = 0;
		private int maxCount = 40;
		protected string carIDs = string.Empty;
		protected string carIDAndName = string.Empty;
		protected SerialEntity se = new SerialEntity();
		protected string hotCarIDs = string.Empty;
		protected string CsHeadHTML = string.Empty;
	    protected string returnPeizhiUrl = "/tempaspx/cscompare{0}.aspx?csid={0}";
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{ 
				base.SetPageCache(30);
				this.GetPageParam();
				this.GetCarIDByCsID();
				this.GetHot2Car();
				CsHeadHTML = base.GetCommonNavigation("MCsCompare", csID);
		}

		#region Page Method

		// 取参数
		private void GetPageParam()
		{
			string csIDstr = this.Request.QueryString["csID"];
			if (!string.IsNullOrEmpty(csIDstr)
				&& int.TryParse(csIDstr, out csID))
			{
			}
		    returnPeizhiUrl = string.Format(returnPeizhiUrl, csID);
		}

		// 取子品牌下车型ID
		private void GetCarIDByCsID()
		{
			if (csID > 0)
			{
				se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csID);
				DataSet ds = new DataSet();
				if (se.SaleState == "停销")
				{
					// 停销子品牌取最新年款的车型
					ds = base.GetCarIDAndNameForNoSaleCS(csID, WebConfig.CachedDuration);
				}
				else
				{
					ds = base.GetCarIDAndNameForCS(csID, WebConfig.CachedDuration);
				}
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						if (i > maxCount - 1)
						{
							break;
						}
						if (carIDAndName != "" && carIDAndName.Length > 0)
						{
							carIDAndName += "|" + "id" + ds.Tables[0].Rows[i]["car_id"].ToString() + "," + ds.Tables[0].Rows[i]["car_name"].ToString().Trim() + "," + ds.Tables[0].Rows[i]["Engine_Exhaust"].ToString().Trim() + "," + ds.Tables[0].Rows[i]["TransmissionType"].ToString().Trim();
							carIDs += "," + ds.Tables[0].Rows[i]["car_id"].ToString();
						}
						else
						{
							carIDAndName = "id" + ds.Tables[0].Rows[i]["car_id"].ToString() + "," + ds.Tables[0].Rows[i]["car_name"].ToString().Trim() + "," + ds.Tables[0].Rows[i]["Engine_Exhaust"].ToString().Trim() + "," + ds.Tables[0].Rows[i]["TransmissionType"].ToString().Trim();
							carIDs = ds.Tables[0].Rows[i]["car_id"].ToString();
						}
					}
				}
			}
		}

		/// <summary>
		/// 取关注最高的2个车默认显示
		/// </summary>
		private void GetHot2Car()
		{
			DataSet ds = base.GetHotCarInfoByCsID(csID);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				if (ds.Tables[0].Rows[0]["pv_sumnum"].ToString() == "")
				{ return; }
				List<string> tempHot2CarID = new List<string>();
				for (int i = 0; i < (ds.Tables[0].Rows.Count >= 2 ? 2 : ds.Tables[0].Rows.Count); i++)
				{
					tempHot2CarID.Add(",");
					tempHot2CarID.Add(ds.Tables[0].Rows[i]["car_id"].ToString());
				}
				if (tempHot2CarID.Count > 0)
				{ tempHot2CarID.Add(","); }
				hotCarIDs = string.Concat(tempHot2CarID.ToArray());
			}
		}

		#endregion
	}
}