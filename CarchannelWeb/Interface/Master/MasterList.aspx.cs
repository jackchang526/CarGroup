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

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Master
{
	/// <summary>
	/// 主品牌列表 主品牌简介(车语传媒 杨光)
	/// </summary>
	public partial class MasterList : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Response.ContentType = "Text/XML";
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<MasterList>");
				GetMasterData();
				sb.Append("</MasterList>");
				Response.Write(sb.ToString());
			}
		}

		private void GetMasterData()
		{
			string sqlGetMaster = " select bs_id,bs_name,bs_LogoInfo,bs_introduction,urlspell from dbo.Car_MasterBrand where isState=1 ";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlGetMaster);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Master>");
					sb.Append("<BsID>");
					sb.Append(ds.Tables[0].Rows[i]["bs_id"].ToString().Trim());
					sb.Append("</BsID>");
					sb.Append("<BsName>");
					sb.Append(ds.Tables[0].Rows[i]["bs_name"].ToString().Trim());
					sb.Append("</BsName>");
					sb.Append("<BsIntroduction>");
					sb.Append("<![CDATA[" + ds.Tables[0].Rows[i]["bs_introduction"].ToString().Trim() + "]]>");
					sb.Append("</BsIntroduction>");
					sb.Append("<BsLogoInfo>");
					sb.Append("<![CDATA[" + ds.Tables[0].Rows[i]["bs_LogoInfo"].ToString().Trim() + "]]>");
					sb.Append("</BsLogoInfo>");
					sb.Append("</Master>");
				}
			}
		}
	}
}