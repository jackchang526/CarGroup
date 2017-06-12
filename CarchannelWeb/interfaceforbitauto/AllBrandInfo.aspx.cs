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
	/// <summary>
	/// 所有品牌名，LOGO(胡利军) New
	/// </summary>
	public partial class AllBrandInfo : InterfacePageBase
	{
		StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<BrandInfo>");
				this.GetAllBrand();
				sb.Append("</BrandInfo>");
				Response.Write(sb.ToString());
			}
		}

		private void GetAllBrand()
		{
			StringBuilder sbGuo = new StringBuilder();
			StringBuilder sbJin = new StringBuilder();
			DataSet ds = base.GetAllBrandInfoForCMS();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					if (ds.Tables[0].Rows[i]["Country"].ToString() == "国产")
					{
						sbGuo.Append("<Brand CbID=\"" + ds.Tables[0].Rows[i]["cb_id"].ToString() + "\" ");
						sbGuo.Append(" CbName=\"" + ds.Tables[0].Rows[i]["cb_name"].ToString().Replace("&", "&amp;") + "\" ");
						sbGuo.Append(" CbLogo=\"http://image.bitautoimg.com/bt/car/default/images/carimage/b_" + ds.Tables[0].Rows[i]["cb_id"].ToString() + "_m.jpg\" />");
					}
					else
					{
						sbJin.Append("<Brand CbID=\"" + ds.Tables[0].Rows[i]["cb_id"].ToString() + "\" ");
						sbJin.Append(" CbName=\"" + ds.Tables[0].Rows[i]["cb_name"].ToString().Replace("&", "&amp;") + "\" ");
						sbJin.Append(" CbLogo=\"http://image.bitautoimg.com/bt/car/default/images/carimage/b_" + ds.Tables[0].Rows[i]["cb_id"].ToString() + "_m.jpg\" />");
					}
				}
			}
			sb.Append("<BrandList Name=\"国产\">");
			sb.Append(sbGuo.ToString());
			sb.Append("</BrandList>");
			sb.Append("<BrandList Name=\"进口\">");
			sb.Append(sbJin.ToString());
			sb.Append("</BrandList>");
		}
	}
}