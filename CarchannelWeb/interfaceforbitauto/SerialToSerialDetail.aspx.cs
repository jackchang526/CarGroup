using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 熊玉辉、杨立峰 (子品牌还关注)
	/// </summary>
	public partial class SerialToSerialDetail : InterfacePageBase
	{
		private int csID = 0;
		private string csName = string.Empty;
		private int top = 6;
		private StringBuilder sb = new StringBuilder();
		private Car_SerialEntity cse;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<SerialToSerial ID=\"{0}\" CsName=\"{1}\">");
				GetPageParam();
				if (csID > 0)
				{
					cse = new Car_SerialBll().GetSerialInfoEntity(csID);
					csName = cse.Cs_Name.Trim();
					GetSerialToSerialDetailByCsID();
				}
				sb.Append("</SerialToSerial>");
				Response.Write(string.Format(sb.ToString(), csID, csName));
			}
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string strCsID = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(strCsID, out csID))
				{
				}
			}
		}

		private void GetSerialToSerialDetailByCsID()
		{
			List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(csID, top);
			if (lsts.Count > 0)
			{
				foreach (EnumCollection.SerialToSerial sts in lsts)
				{
					sb.Append("<Item CsID=\"" + sts.ToCsID.ToString() + "\" ");
					sb.Append(" CsName=\"" + sts.ToCsName.ToString().Trim() + "\" ");
					sb.Append(" CsShowName=\"" + sts.ToCsShowName.ToString().Trim() + "\" ");
					sb.Append(" AllSpell=\"" + sts.ToCsAllSpell.ToString().Trim().ToLower() + "\" ");
					sb.Append(" CsPriceRange=\"" + sts.ToCsPriceRange.ToString().Trim().ToLower() + "\" ");
					sb.Append(" CsPic=\"" + sts.ToCsPic.ToString() + "\" />");
				}
			}
		}
	}
}