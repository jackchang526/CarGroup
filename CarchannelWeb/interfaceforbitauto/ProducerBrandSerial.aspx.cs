using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 厂商、品牌、子品牌数据(汪强)
	/// </summary>
	public partial class ProducerBrandSerial : InterfacePageBase
	{
		StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				GetAllProducerBrandSerialData();
				Response.Write(sb.ToString());
			}
		}

		/// <summary>
		/// 取厂商、品牌、子品牌数据
		/// </summary>
		private void GetAllProducerBrandSerialData()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<ProducerBrandSerial>");
			DataSet ds = new DataSet();
			if (HttpContext.Current.Cache.Get("interfaceforbitauto_ProducerBrandSerial") != null)
			{
				ds = (DataSet)HttpContext.Current.Cache.Get("interfaceforbitauto_ProducerBrandSerial");
			}
			else
			{
				string sql = @"SELECT  cp.*, cb.cb_id, cb.cb_name, cb.allSpell AS cbAllSpell, cs.cs_id,
                                        cs.cs_Name, cs.cs_showname, cs.allSpell AS csAllSpell, cs.cs_OtherName,
                                        cb.cb_OtherName
                                FROM    Car_Serial cs
                                        LEFT JOIN Car_Brand cb ON cs.cb_id = cb.cb_id
                                        LEFT JOIN Car_Producer cp ON cb.cp_id = cp.cp_id
                                WHERE   cp.isState = 1
                                        AND cb.isState = 1
                                        AND cs.isState = 1
                                ORDER BY cp.cp_id, cb.cb_id, cs.cs_id";
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				HttpContext.Current.Cache.Insert("interfaceforbitauto_ProducerBrandSerial", ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
			}
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				int currentCPID = 0;
				int currentCBID = 0;
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					// 不同厂商
					if (ds.Tables[0].Rows[i]["cp_id"].ToString() != currentCPID.ToString())
					{
						if (currentCPID != 0)
						{
							sb.Append("</Brand>");
							sb.Append("</Producer>");
						}
						currentCPID = int.Parse(ds.Tables[0].Rows[i]["cp_id"].ToString());
						sb.Append("<Producer>");
						sb.Append("<Cp_ID>" + ds.Tables[0].Rows[i]["cp_id"].ToString().Trim() + "</Cp_ID>");
						sb.Append("<Cp_Name>" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_Name"].ToString().Trim()) + "</Cp_Name>");
						sb.Append("<Cp_ShortName>" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_ShortName"].ToString().Trim()) + "</Cp_ShortName>");
						sb.Append("<Cp_Byname>" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_Byname"].ToString().Trim()) + "</Cp_Byname>");
						sb.Append("<Cp_EName>" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_EName"].ToString().Trim()) + "</Cp_EName>");
						sb.Append("<Cp_Url>" + ds.Tables[0].Rows[i]["Cp_Url"].ToString().Trim() + "</Cp_Url>");
						sb.Append("<Cp_Phone>" + ds.Tables[0].Rows[i]["Cp_Phone"].ToString().Trim() + "</Cp_Phone>");
						sb.Append("<Cp_Introduction><![CDATA[" + ds.Tables[0].Rows[i]["Cp_Introduction"].ToString().Trim() + "]]></Cp_Introduction>");
						// 不同品牌
						if (ds.Tables[0].Rows[i]["cb_id"].ToString() != currentCBID.ToString())
						{
							currentCBID = int.Parse(ds.Tables[0].Rows[i]["cb_id"].ToString());
							sb.Append("<Brand>");
							sb.Append("<Cb_ID>" + ds.Tables[0].Rows[i]["cb_id"].ToString().Trim() + "</Cb_ID>");
							sb.Append("<Cb_AllSpell>" + ds.Tables[0].Rows[i]["cbAllSpell"].ToString().Trim().ToLower() + "</Cb_AllSpell>");
							sb.Append("<Cb_Name>" + ds.Tables[0].Rows[i]["Cb_Name"].ToString().Trim() + "</Cb_Name>");
							sb.Append("<Cb_OtherName>" + ds.Tables[0].Rows[i]["cb_OtherName"].ToString().Trim() + "</Cb_OtherName>");
						}
					}
					// 相同厂商
					else
					{
						// 不同品牌
						if (ds.Tables[0].Rows[i]["cb_id"].ToString() != currentCBID.ToString())
						{
							currentCBID = int.Parse(ds.Tables[0].Rows[i]["cb_id"].ToString());
							sb.Append("</Brand>");
							sb.Append("<Brand>");
							sb.Append("<Cb_ID>" + ds.Tables[0].Rows[i]["cb_id"].ToString().Trim() + "</Cb_ID>");
							sb.Append("<Cb_AllSpell>" + ds.Tables[0].Rows[i]["cbAllSpell"].ToString().Trim().ToLower() + "</Cb_AllSpell>");
							sb.Append("<Cb_Name>" + ds.Tables[0].Rows[i]["Cb_Name"].ToString().Trim() + "</Cb_Name>");
							sb.Append("<Cb_OtherName>" + ds.Tables[0].Rows[i]["cb_OtherName"].ToString().Trim() + "</Cb_OtherName>");
						}
					}
					sb.Append("<Serial>");
					sb.Append("<Cs_ID>" + ds.Tables[0].Rows[i]["cs_id"].ToString().Trim() + "</Cs_ID>");
					sb.Append("<Cs_AllSpell>" + ds.Tables[0].Rows[i]["csAllSpell"].ToString().Trim().ToLower() + "</Cs_AllSpell>");
					sb.Append("<Cs_Name>" + ds.Tables[0].Rows[i]["Cs_Name"].ToString().Trim() + "</Cs_Name>");
					sb.Append("<Cs_ShowName>" + ds.Tables[0].Rows[i]["cs_Showname"].ToString().Trim() + "</Cs_ShowName>");
					sb.Append("<Cs_OtherName>" + ds.Tables[0].Rows[i]["cs_OtherName"].ToString().Trim() + "</Cs_OtherName>");
					sb.Append("</Serial>");
				}
				sb.Append("</Brand>");
				sb.Append("</Producer>");
			}
			sb.Append("</ProducerBrandSerial>");
		}
	}
}