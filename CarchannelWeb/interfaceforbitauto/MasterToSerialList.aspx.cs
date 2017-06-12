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
	public partial class MasterToSerialList : InterfacePageBase
	{
		private DataSet ds = new DataSet();
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<Root>");
				GetMasterToSerialData();
				GenerateXMLForMasterToSerial();
				sb.Append("</Root>");
				Response.Write(sb.ToString());
			}
		}

		private void GetMasterToSerialData()
		{
			string sql = @"SELECT  cs.cs_id, cs.cs_name, cs.cs_showname, cs.allspell, cmb.bs_id, cb.cb_id,
                                    cb.cb_name, ( CASE cb.cb_Country
                                                    WHEN '中国' THEN 0
                                                    ELSE 1
                                                  END ) AS CpCountry,
                                    LEFT(cmb.spell, 1) + ' ' + cmb.bs_name AS msname,
                                    LEFT(cmb.spell, 1) AS mspell, cmb.bs_name
                            FROM    dbo.Car_Basic car
                                    LEFT JOIN dbo.Car_Serial cs ON car.cs_id = cs.cs_id
                                    LEFT JOIN dbo.Car_Brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN dbo.Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                    LEFT JOIN dbo.Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                            WHERE   car.isState = 1
                                    AND cs.isState = 1
                                    AND cb.isState = 1
                                    AND car.Car_SaleState <> '停销'
                                    AND cs.CsSaleState <> '停销'
                                    AND cmb.bs_id IS NOT NULL
                            ORDER BY mspell, cmb.bs_id, CpCountry, cb.cb_id, cs.allspell";
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
		}

		private void GenerateXMLForMasterToSerial()
		{
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				string currentCsID = "";
				string currentBsID = "";
				string currentCbID = "";
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					if (i == 0)
					{
						currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString().Trim();
						currentBsID = ds.Tables[0].Rows[i]["bs_id"].ToString().Trim();
						currentCbID = ds.Tables[0].Rows[i]["cb_id"].ToString().Trim();
						sb.Append("<Master ID=\"" + currentBsID + "\" ");
						sb.Append(" Spell=\"" + ds.Tables[0].Rows[i]["mspell"].ToString().Trim() + "\" ");
						sb.Append(" Name=\"" + ds.Tables[0].Rows[i]["bs_name"].ToString().Trim() + "\" >");

						sb.Append("<Brand ID=\"" + currentCbID + "\" ");
						sb.Append(" Name=\"" + ds.Tables[0].Rows[i]["cb_name"].ToString().Trim() + "\" >");
					}
					else
					{
						if (currentCsID == ds.Tables[0].Rows[i]["cs_id"].ToString().Trim())
						{ continue; }
						// 不同主品牌
						if (currentBsID != ds.Tables[0].Rows[i]["bs_id"].ToString().Trim())
						{
							sb.Append("</Brand>");
							sb.Append("</Master>");
							currentBsID = ds.Tables[0].Rows[i]["bs_id"].ToString().Trim();
							currentCbID = ds.Tables[0].Rows[i]["cb_id"].ToString().Trim();
							sb.Append("<Master ID=\"" + currentBsID + "\" ");
							sb.Append(" Spell=\"" + ds.Tables[0].Rows[i]["mspell"].ToString().Trim() + "\" ");
							sb.Append(" Name=\"" + ds.Tables[0].Rows[i]["bs_name"].ToString().Trim() + "\" >");

							sb.Append("<Brand ID=\"" + currentCbID + "\" ");
							sb.Append(" Name=\"" + ds.Tables[0].Rows[i]["cb_name"].ToString().Trim() + "\" >");
						}
						else
						{
							// 相同主品牌 不同品牌
							if (currentCbID != ds.Tables[0].Rows[i]["cb_id"].ToString().Trim())
							{
								currentCbID = ds.Tables[0].Rows[i]["cb_id"].ToString().Trim();
								sb.Append("</Brand>");
								sb.Append("<Brand ID=\"" + currentCbID + "\" ");
								sb.Append(" Name=\"" + ds.Tables[0].Rows[i]["cb_name"].ToString().Trim() + "\" >");
							}
						}
					}
					currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString().Trim();
					sb.Append("<Serial ID=\"" + ds.Tables[0].Rows[i]["cs_id"].ToString().Trim() + "\" ");
					sb.Append(" Name=\"" + ds.Tables[0].Rows[i]["cs_name"].ToString().Trim() + "\" ");
					sb.Append(" ShowName=\"" + ds.Tables[0].Rows[i]["cs_showname"].ToString().Trim() + "\" />");
				}
				sb.Append("</Brand>");
				sb.Append("</Master>");
			}
		}
	}
}