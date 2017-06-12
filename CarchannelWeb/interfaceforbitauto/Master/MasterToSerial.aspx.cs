using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.Master
{
	/// <summary>
	/// 主品牌、品牌、子品牌 级联列表(互动产品熊玉辉dept:bitautocheguanjia)
	/// </summary>
	public partial class MasterToSerial : InterfacePageBase
	{
		private string dept = "";
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				if (dept == "bitautocheguanjia")
				{
					GetMasterToSerial();
				}
				Response.Write(sb.ToString());
			}
		}

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{
			if (this.Request.QueryString["dept"] != null && this.Request.QueryString["dept"].ToString() != "")
			{
				dept = this.Request.QueryString["dept"].ToString().ToLower();
			}
		}

		/// <summary>
		/// 取主品牌到子品牌数据 包括停销数据
		/// </summary>
		private void GetMasterToSerial()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<Root>");
			string sql = @"SELECT  cmb.bs_id, cmb.bs_name, LEFT(cmb.spell, 1) AS firstSpell, cb.cb_id,
                                    cb.cb_name, cs.cs_id, cs.cs_name, cs.cs_showname,
                                    ( CASE cb.cb_Country
                                        WHEN '中国' THEN '国产'
                                        ELSE '进口'
                                      END ) AS Cp_Country
                            FROM    car_basic car
                                    LEFT JOIN car_serial cs ON car.cs_id = cs.cs_id
                                    LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                    LEFT JOIN Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                            WHERE   car.isState = 1
                                    AND cs.isState = 1
                                    AND cb.isState = 1
                            ORDER BY firstSpell, cmb.bs_id, Cp_Country, cb.cb_id, cs.cs_name";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				string currentBS = "";
				string currentCB = "";
				string currentCS = "";
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					if (currentBS == "")
					{
						// 第1行
						currentBS = dr["bs_id"].ToString().Trim();
						currentCB = dr["cb_id"].ToString().Trim();
						currentCS = dr["cs_id"].ToString().Trim();
						sb.Append("<Master BsID=\"" + currentBS + "\" BsName=\"" + dr["bs_name"].ToString().Trim().Replace("&", "＆").Replace("\"", "'") + "\" FirstSpell=\"" + dr["firstSpell"].ToString().Trim() + "\" >");
						sb.Append("<Brand CbID=\"" + currentCB + "\" CbName=\"" + dr["cb_name"].ToString().Trim().Replace("&", "＆").Replace("\"", "'") + "\" >");
					}
					else
					{
						if (currentBS == dr["bs_id"].ToString().Trim())
						{
							// 相同主品牌
							if (currentCB == dr["cb_id"].ToString().Trim())
							{
								// 相同品牌
								if (currentCS == dr["cs_id"].ToString().Trim())
								{
									// 相同子品牌
									continue;
								}
								else
								{
									// 不同子品牌
									currentCS = dr["cs_id"].ToString().Trim();
									// sb.Append("</Serial>");
								}
							}
							else
							{
								// 不同品牌
								// sb.Append("</Serial>");
								sb.Append("</Brand>");
								currentCS = dr["cs_id"].ToString().Trim();
								currentCB = dr["cb_id"].ToString().Trim();
								sb.Append("<Brand CbID=\"" + currentCB + "\" CbName=\"" + dr["cb_name"].ToString().Trim().Replace("&", "＆").Replace("\"", "'") + "\" >");
							}
						}
						else
						{
							// 不同主品牌
							// sb.Append("</Serial>");
							sb.Append("</Brand>");
							sb.Append("</Master>");
							currentCS = dr["cs_id"].ToString().Trim();
							currentCB = dr["cb_id"].ToString().Trim();
							currentBS = dr["bs_id"].ToString().Trim();
							sb.Append("<Master BsID=\"" + currentBS + "\" BsName=\"" + dr["bs_name"].ToString().Trim().Replace("&", "＆").Replace("\"", "'") + "\" FirstSpell=\"" + dr["firstSpell"].ToString().Trim() + "\" >");
							sb.Append("<Brand CbID=\"" + currentCB + "\" CbName=\"" + dr["cb_name"].ToString().Trim().Replace("&", "＆").Replace("\"", "'") + "\" >");
						}
					}
					sb.Append("<Serial CsID=\"" + currentCS + "\" CsName=\"" + dr["cs_name"].ToString().Trim().Replace("&", "＆").Replace("\"", "'") + "\" CsShowName=\"" + dr["cs_showname"].ToString().Trim().Replace("&", "＆").Replace("\"", "'") + "\" />");
				}
				sb.Append("</Brand>");
				sb.Append("</Master>");
				sb.Append("</Root>");
			}
		}
	}
}