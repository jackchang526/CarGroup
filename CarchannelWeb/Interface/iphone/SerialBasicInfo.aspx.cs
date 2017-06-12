using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.iphone
{
	/// <summary>
	/// iphone 子品牌名片基本信息(杨立锋)
	/// </summary>
	public partial class SerialBasicInfo : InterfacePageBase
	{
		private int csID = 0;
		private StringBuilder sb = new StringBuilder();
		protected EnumCollection.SerialInfoCard sic;	//子品牌名片
		private Car_SerialEntity cse;				//子品牌信息 

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<SerialBasicInfo>");
				GetPageParam();
				GetSerialBasicInfo();
				sb.Append("</SerialBasicInfo>");
				Response.Write(sb.ToString());
			}
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string strCSID = this.Request.QueryString["csID"].ToString().Trim();
				if (int.TryParse(strCSID, out csID))
				{ }
			}
		}

		private void GetSerialBasicInfo()
		{
			if (csID > 0)
			{
				sic = new Car_SerialBll().GetSerialInfoCard(csID);
				cse = new Car_SerialBll().GetSerialInfoEntity(csID);
				if (sic.CsID != 0)
				{
					sb.Append("<Serial ID=\"" + sic.CsID.ToString() + "\" ");
					sb.Append(" Name=\"" + sic.CsName.Trim() + "\" ");
					sb.Append(" ShowName=\"" + sic.CsShowName.Trim() + "\" ");
					sb.Append(" CsPic=\"" + sic.CsDefaultPic + "\" ");
					sb.Append(" AllSpell=\"" + sic.CsAllSpell.Trim().ToLower() + "\" ");
					sb.Append(" CsPriceRange=\"" + sic.CsPriceRange.Trim() + "\" ");
					sb.Append(" CsTransmissionType=\"" + sic.CsTransmissionType.Trim() + "\" ");
					sb.Append(" EngineExhaust=\"" + sic.CsEngineExhaust.Trim() + "\" ");
					sb.Append(" CsOfficialFuelCost=\"" + sic.CsOfficialFuelCost.Trim() + "\" ");
					sb.Append(" CsGuestFuelCost=\"" + sic.CsGuestFuelCost.Trim() + "\" ");
					sb.Append(" CsVirtues=\"" + cse.Cs_Virtues.Trim() + "\" ");
					sb.Append(" CsDefect=\"" + cse.Cs_Defect.Trim() + "\" />");
				}
			}
		}
	}
}