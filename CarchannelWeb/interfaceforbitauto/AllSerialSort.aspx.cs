using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class AllSerialSort : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();
		private int top = 8;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				if (this.Request.QueryString["top"] != null && this.Request.QueryString["top"].ToString() != "")
				{
					try
					{
						top = int.Parse(this.Request.QueryString["top"].ToString());
						if (top < 0 || top > 10)
						{ top = 8; }
					}
					catch
					{ top = 8; }
				}
				this.GetSerialSort();
			}
		}

		private void GetSerialSort()
		{
			string temp = "";

			if (HttpContext.Current.Cache.Get("InterfaceForBitautoSerialSort") != null)
			{
				temp = Convert.ToString(HttpContext.Current.Cache.Get("InterfaceForBitautoSerialSort"));
			}
			else
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				DataSet ds1 = base.GetAllSerialSort(top, false);
				DataSet ds2 = base.GetAllSerialSort(top, true);
				sb.Append("<SerialSort  Time=\"" + DateTime.Now.ToString() + "\">");

				sb.Append("<Serial  Country=\"国产\">");

				if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
					{
						sb.Append("<Item Sort=\"" + Convert.ToString(i + 1) + "\" ");
						sb.Append("CsID=\"" + ds1.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
						sb.Append("CsName=\"" + ds1.Tables[0].Rows[i]["cs_name"].ToString().Replace("&", "&amp;") + "\" ");
						sb.Append("Prices=\"" + GetSerialPriceRangeByID(int.Parse(ds1.Tables[0].Rows[i]["cs_id"].ToString())).Replace("万", string.Empty) + "\" ");
						sb.Append("OldCbID=\"" + ds1.Tables[0].Rows[i]["oldcb_id"].ToString() + "\" ");
						sb.Append("URL=\"http://car.bitauto.com/" + ds1.Tables[0].Rows[i]["allspell"].ToString().ToLower() + "/\" />");
					}
				}
				sb.Append("</Serial>");

				sb.Append("<Serial Country=\"进口\">");

				if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
					{
						sb.Append("<Item Sort=\"" + Convert.ToString(i + 1) + "\" ");
						sb.Append("CsID=\"" + ds2.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
						sb.Append("CsName=\"" + ds2.Tables[0].Rows[i]["cs_name"].ToString().Replace("&", "&amp;") + "\" ");
						sb.Append("Prices=\"" + GetSerialPriceRangeByID(int.Parse(ds2.Tables[0].Rows[i]["cs_id"].ToString())).Replace("万", string.Empty) + "\" ");
						sb.Append("OldCbID=\"" + ds2.Tables[0].Rows[i]["oldcb_id"].ToString() + "\" ");
						sb.Append("URL=\"http://car.bitauto.com/" + ds2.Tables[0].Rows[i]["allspell"].ToString() + "/\" />");
					}
				}
				sb.Append("</Serial>");
				sb.Append("</SerialSort>");
				temp = sb.ToString();
				HttpContext.Current.Cache.Insert("InterfaceForBitautoSerialSort", temp, null, DateTime.Now.AddMinutes(20), TimeSpan.Zero);
			}
			Response.Write(temp);
		}
	}
}