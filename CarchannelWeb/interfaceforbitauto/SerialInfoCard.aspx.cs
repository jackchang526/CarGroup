using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 子品名片
	/// </summary>
	public partial class SerialInfoCard : InterfacePageBase
	{
		private int csID = 0;
		private EnumCollection.SerialInfoCard sic = new EnumCollection.SerialInfoCard();
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(15);
			Response.ContentType = "Text/XML";
			if (!this.IsPostBack)
			{
				this.CheckParam();
				this.GetSerialInfoCardByCsID();
			}
		}

		private void CheckParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string csIDstr = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(csIDstr, out csID))
				{ }
			}
		}

		/// <summary>
		/// 取子品牌名片数据
		/// </summary>
		private void GetSerialInfoCardByCsID()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<SerialInfoCard>");
			if (csID > 0)
			{
				sic = base.GetSerialInfoCardByCsID(csID);
				if (sic.CsID > 0)
				{
					sb.Append("<SerialInfo CsID=\"" + sic.CsID.ToString() + "\" ");
					sb.Append(" CsName=\"" + sic.CsName.Trim() + "\" ");
					sb.Append(" CsShowName=\"" + sic.CsShowName.Trim() + "\" ");
					sb.Append(" CsAllSpell=\"" + sic.CsAllSpell.Trim() + "\" ");
					sb.Append(" CsDefaultPic=\"" + sic.CsDefaultPic.Trim() + "\" "); // 子品牌默认图
					sb.Append(" CsPriceRange=\"" + sic.CsPriceRange.Trim() + "\" ");    // 报价区间
					sb.Append(" CsEngineExhaust=\"" + sic.CsEngine_Exhaust.Replace("</span>", "").Replace("<span>", " ") + "\" "); // 排量

					// sb.Append(" CsOfficialFuelCost=\"" + sic.CsOfficialFuelCost.Trim() + "\" ");// 官方油耗
					if (sic.CsSummaryFuelCost.Length > 0)
						sb.Append(" CsOfficialFuelCost=\"" + sic.CsSummaryFuelCost.Trim() + "\" ");                    //综合工况油耗
					else
						sb.Append(" CsOfficialFuelCost=\"" + sic.CsOfficialFuelCost.Trim() + "\" ");               //官方油耗

					sb.Append(" CsTransmissionType=\"" + sic.CsTransmissionType.Trim() + "\" ");// 变速器
					sb.Append(" CsPicCount=\"" + sic.CsPicCount.ToString().Trim() + "\" "); // 子品牌图片总数
					sb.Append(" CsDianPingCount=\"" + sic.CsDianPingCount.ToString().Trim() + "\" "); // 子品牌点评总数
					sb.Append(" CsAskCount=\"" + sic.CsAskCount.ToString().Trim() + "\" />"); // 子品牌答疑总数

					sb.Append("<SerialNews>");
					sb.Append("<Item Name=\"上市专题\" URL=\"" + sic.CsNewShangShi.Trim() + "\" />");
					sb.Append("<Item Name=\"购车手册\" URL=\"" + sic.CsNewGouCheShouChe.Trim() + "\" />");
					sb.Append("<Item Name=\"销售数据\" URL=\"" + sic.CsNewXiaoShouShuJu.Trim() + "\" />");
					sb.Append("<Item Name=\"维修保养\" URL=\"" + sic.CsNewWeiXiuBaoYang.Trim() + "\" />");
					sb.Append("<Item Name=\"科技\" URL=\"" + sic.CsNewKeJi.Trim() + "\" />");
					sb.Append("</SerialNews>");
				}
			}
			sb.Append("</SerialInfoCard>");
			Response.Write(sb.ToString());
		}
	}
}