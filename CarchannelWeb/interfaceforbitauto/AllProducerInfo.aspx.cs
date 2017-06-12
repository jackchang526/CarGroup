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
	public partial class AllProducerInfo : InterfacePageBase
	{
		private string cpIDs = string.Empty;
		StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.GetPageParam();
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<ProducerInfo>");
				this.GetAllProducer();
				sb.Append("</ProducerInfo>");
				Response.Write(sb.ToString());
			}
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["DelCpIDs"] != null && this.Request.QueryString["DelCpIDs"].ToString() != "")
			{
				string tempCP = this.Request.QueryString["DelCpIDs"].ToString();
				string[] arrTempID = tempCP.Split(',');
				for (int i = 0; i < arrTempID.Length; i++)
				{
					int cpid = 0;
					if (int.TryParse(arrTempID[i], out cpid))
					{
						if (cpid > 0)
						{
							cpIDs += cpid.ToString() + ",";
						}
					}
				}
			}
		}

		private void GetAllProducer()
		{
			StringBuilder sbGuo = new StringBuilder();
			StringBuilder sbJin = new StringBuilder();
			DataSet ds = base.GetAllProducerInfoForCMS();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					if (ds.Tables[0].Rows[i]["Country"].ToString() == "国产")
					{
						if (cpIDs != "" && cpIDs.IndexOf(ds.Tables[0].Rows[i]["cp_id"].ToString() + ",") >= 0)
						{
							continue;
						}
						sbGuo.Append("<ProducerGuoChan CpID=\"" + ds.Tables[0].Rows[i]["cp_id"].ToString() + "\" ");
						sbGuo.Append(" CpShortName=\"" + ds.Tables[0].Rows[i]["Cp_ShortName"].ToString().Replace("&", "&amp;") + "\" ");
						sbGuo.Append(" CbLogo=\"http://image.bitautoimg.com/bt/car/default/images/carimage/p_" + ds.Tables[0].Rows[i]["cp_id"].ToString() + "_m.jpg\" />");
					}
					else
					{
						if (cpIDs != "" && cpIDs.IndexOf(ds.Tables[0].Rows[i]["cp_id"].ToString() + ",") >= 0)
						{
							continue;
						}
						sbJin.Append("<ProducerJinKou CpID=\"" + ds.Tables[0].Rows[i]["cp_id"].ToString() + "\" ");
						sbJin.Append(" CpShortName=\"" + ds.Tables[0].Rows[i]["Cp_ShortName"].ToString().Replace("&", "&amp;") + "\" ");
						sbJin.Append(" CbLogo=\"http://image.bitautoimg.com/bt/car/default/images/carimage/p_" + ds.Tables[0].Rows[i]["cp_id"].ToString() + "_m.jpg\" />");
					}
				}
			}
			sb.Append("<ProducerList Name=\"国产\">");
			sb.Append(sbGuo.ToString());
			sb.Append("</ProducerList>");
			sb.Append("<ProducerList Name=\"进口\">");
			sb.Append(sbJin.ToString());
			sb.Append("</ProducerList>");
		}
	}
}