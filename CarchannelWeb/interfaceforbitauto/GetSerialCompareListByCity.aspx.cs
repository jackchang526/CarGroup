using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 取子品牌对比前5的子品牌数据 (王泊)
	/// </summary>
	public partial class GetSerialCompareListByCity : InterfacePageBase
	{
		private int csID = 0;
		private int cityID = 0;
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<SerialCompare>");
				GetParam();
				GetSerialCompareByCity();
				sb.Append("</SerialCompare>");
				Response.Write(sb.ToString());
			}
		}

		private void GetParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string strCsID = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(strCsID, out csID))
				{
				}
			}
			if (this.Request.QueryString["cityID"] != null && this.Request.QueryString["cityID"].ToString() != "")
			{
				string strCityID = this.Request.QueryString["cityID"].ToString();
				if (int.TryParse(strCityID, out cityID))
				{
				}
			}
		}

		private void GetSerialCompareByCity()
		{
			if (csID > 0)
			{
				Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = new Dictionary<string, List<Car_SerialBaseEntity>>();
				carSerialBaseList = new Car_SerialBll().GetSerialCityCompareList(csID, HttpContext.Current);
				if (carSerialBaseList != null && carSerialBaseList.Count > 0 && carSerialBaseList.ContainsKey("全国"))
				{
					List<Car_SerialBaseEntity> serialCompareList = carSerialBaseList["全国"];
					for (int i = 0; i < serialCompareList.Count; i++)
					{
						Car_SerialBaseEntity carSerial = serialCompareList[i];
						sb.Append("<Serial CsID=\"" + carSerial.SerialId + "\" ");
						sb.Append(" CsShowName=\"" + carSerial.SerialShowName.Trim() + "\" ");
						sb.Append(" AllSpell=\"" + carSerial.SerialNameSpell.Trim().ToLower() + "\" />");
					}
				}
			}
		}
	}
}