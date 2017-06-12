using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 厂商信息接口 for 胡利军 Jul.16.2009
	/// </summary>
	public partial class producerInfo : OldPageBase
	{
		private string cpID = string.Empty;
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.CheckParam();
				this.GetProducer();
			}
		}

		private void CheckParam()
		{
			if (this.Request.QueryString["cpid"] != null && this.Request.QueryString["cpid"].ToString() != "")
			{
				int iCpID = 0;
				if (int.TryParse(this.Request.QueryString["cpid"].ToString(), out iCpID))
				{
					if (iCpID > 0 && iCpID < 100000)
					{
						cpID = iCpID.ToString();
					}
				}
			}
		}

		private void GetProducer()
		{
			string sql = string.Empty;
			DataSet ds = new DataSet();
			if (cpID != "")
			{
				// 取特定厂商信息
				if (HttpContext.Current.Cache.Get("InterfaceForBitautoProducer_" + cpID) != null)
				{
					ds = (DataSet)HttpContext.Current.Cache.Get("InterfaceForBitautoProducer_" + cpID);
				}
				else
				{
					ds = base.GetProducerByCpID(int.Parse(cpID));
					HttpContext.Current.Cache.Insert("InterfaceForBitautoProducer_" + cpID, ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
				}
			}
			else
			{
				// 取所有厂商信息
				if (HttpContext.Current.Cache.Get("InterfaceForBitautoAllProducer") != null)
				{
					ds = (DataSet)HttpContext.Current.Cache.Get("InterfaceForBitautoAllProducer");
				}
				else
				{
					ds = base.GetProducerAll();
					HttpContext.Current.Cache.Insert("InterfaceForBitautoAllProducer", ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
				}
			}

			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<ProducerInfo>");
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Item CpID=\"" + ds.Tables[0].Rows[i]["cp_id"].ToString() + "\" ");
					sb.Append(" CpName=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_Name"].ToString()) + "\" ");
					sb.Append(" CpShortName=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_ShortName"].ToString()) + "\" ");
					sb.Append(" CpEName=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_EName"].ToString()) + "\" ");
					sb.Append(" CpCountry=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_Country"].ToString()) + "\" ");
					sb.Append(" CpUrl=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_Url"].ToString()) + "\" ");
					sb.Append(" CpPhone=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_Phone"].ToString()) + "\" ");
					sb.Append(" Spell=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Spell"].ToString()) + "\" ");
					sb.Append(" OldCpId=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["OldCp_Id"].ToString()) + "\" />");
				}
			}
			sb.Append("</ProducerInfo>");

			Response.Write(sb.ToString());
		}
	}
}