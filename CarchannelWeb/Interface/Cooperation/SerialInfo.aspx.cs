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

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Cooperation
{
	/// <summary>
	/// 合作站根据子品牌ID取子品牌信息(运营产品研发部 汪强)
	/// </summary>
	public partial class SerialInfo : InterfacePageBase
	{
		private string cooperation = string.Empty;
		private int csID = 0;
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.AppendLine("<SerialInfo>");
				GetPageParam();
				GetSerialInfoByCooperation();
				sb.AppendLine("</SerialInfo>");
				Response.Write(sb.ToString());
			}
		}

		/// <summary>
		/// 取页面参数
		/// </summary>
		private void GetPageParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				if (int.TryParse(this.Request.QueryString["csID"].ToString(), out csID))
				{ }
			}

			if (this.Request.QueryString["cooperation"] != null && this.Request.QueryString["cooperation"].ToString() != "")
			{
				cooperation = this.Request.QueryString["cooperation"].ToString();
			}
		}

		/// <summary>
		/// 取子品牌信息
		/// </summary>
		private void GetSerialInfoByCooperation()
		{
			if (csID > 0 && cooperation != "")
			{
				DataSet ds = base.GetAllSErialInfo();
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					DataRow[] drs = ds.Tables[0].Select(" cs_id='" + csID.ToString() + "' ");
					if (drs != null && drs.Length > 0)
					{
						sb.AppendLine("<Serial ID=\"" + csID.ToString() + "\" ");
						sb.Append("CsName=\"" + drs[0]["cs_name"].ToString().Trim() + "\" ");
						sb.Append("CsShowName=\"" + drs[0]["cs_ShowName"].ToString().Trim() + "\" ");
						if (cooperation == "xinhuawang")
						{
							sb.Append("CooperationLink=\"http://xinhua.car.bitauto.com/" + drs[0]["allSpell"].ToString().Trim().ToLower() + "/\" ");
						}
						else
						{
							sb.Append("CooperationLink=\"http://car.bitauto.com/" + drs[0]["allSpell"].ToString().Trim().ToLower() + "/\" ");
						}
						string csPic = "";
						int csCount = 0;
						base.GetSerialPicAndCountByCsID(csID, out csPic, out csCount, true);
						sb.Append("CsPic=\"" + csPic.Replace("_2.", "_1.") + "\" />");
					}
				}
			}
		}
	}
}