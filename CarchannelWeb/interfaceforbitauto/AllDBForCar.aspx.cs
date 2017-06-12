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
using System.Xml;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class AllDBForCar : InterfacePageBase
	{
		private StringBuilder _sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.GetAllCar();
			}
		}

		private void GetAllCar()
		{
			DataSet dsCP = new DataSet();
			DataSet dsCB = new DataSet();
			DataSet dsCS = new DataSet();
			DataSet dsCar = new DataSet();

			#region 取数据

			if (HttpContext.Current.Cache.Get("InterfaceForBitautoCP") != null)
			{
				dsCP = (System.Data.DataSet)HttpContext.Current.Cache.Get("InterfaceForBitautoCP");
			}
			else
			{
				dsCP = base.GetAllCPForInterface();
				HttpContext.Current.Cache.Insert("InterfaceForBitautoCP", dsCP, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
			}

			if (HttpContext.Current.Cache.Get("InterfaceForBitautoCB") != null)
			{
				dsCB = (System.Data.DataSet)HttpContext.Current.Cache.Get("InterfaceForBitautoCB");
			}
			else
			{
				dsCB = base.GetAllCBForInterface();
				HttpContext.Current.Cache.Insert("InterfaceForBitautoCB", dsCB, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
			}

			if (HttpContext.Current.Cache.Get("InterfaceForBitautoCS") != null)
			{
				dsCS = (System.Data.DataSet)HttpContext.Current.Cache.Get("InterfaceForBitautoCS");
			}
			else
			{
				string sqlCs = @"SELECT  a.cs_id, a.cb_id, OldCb_Id, cs_name, a.cs_showname, a.Spell,
                                            ISNULL(d.cp_id, 0) AS cp_id, bat.bitautoTestURL, a.cs_seoname
                                    FROM    Car_Serial a
                                            INNER JOIN dbo.Car_Brand c ON c.cb_id = a.cb_id
                                            INNER JOIN dbo.Car_Producer d ON c.cp_id = d.cp_id
                                            LEFT JOIN dbo.BitAutoTest bat ON a.cs_id = bat.cs_id
                                    WHERE   a.IsState >= 1";
				dsCS = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCs); // base.GetAllCSForInterface();
				HttpContext.Current.Cache.Insert("InterfaceForBitautoCS", dsCS, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
			}

			if (HttpContext.Current.Cache.Get("InterfaceForBitautoCar") != null)
			{
				dsCar = (System.Data.DataSet)HttpContext.Current.Cache.Get("InterfaceForBitautoCar");
			}
			else
			{
				dsCar = base.GetAllCarForInterface();
				HttpContext.Current.Cache.Insert("InterfaceForBitautoCar", dsCar, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
			}

			#endregion

			#region 生成数据

			_sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			_sb.Append("<AutoCarChannel>");

			if (dsCP != null && dsCP.Tables[0].Rows.Count > 0)
			{
				_sb.Append("<Producer>");
				for (int i = 0; i < dsCP.Tables[0].Rows.Count; i++)
				{
					_sb.Append("<Item Cp_Id=\"" + dsCP.Tables[0].Rows[i]["cp_id"].ToString() + "\" ");
					_sb.Append(" OldCp_Id=\"" + dsCP.Tables[0].Rows[i]["OldCp_Id"].ToString() + "\" ");
					_sb.Append(" Cp_Country=\"" + dsCP.Tables[0].Rows[i]["Cp_Country"].ToString() + "\" ");
					_sb.Append(" Spell=\"" + System.Security.SecurityElement.Escape(dsCP.Tables[0].Rows[i]["Spell"].ToString()) + "\" ");
					_sb.Append(" Cp_Name=\"" + System.Security.SecurityElement.Escape(
						dsCP.Tables[0].Rows[i]["cp_name"].ToString()) + "\" />");
				}
				_sb.Append("</Producer>");
			}

			if (dsCB != null && dsCB.Tables[0].Rows.Count > 0)
			{
				_sb.Append("<Brand>");
				for (int i = 0; i < dsCB.Tables[0].Rows.Count; i++)
				{
					_sb.Append("<Item Cb_Id=\"" + dsCB.Tables[0].Rows[i]["cb_id"].ToString() + "\" ");
					_sb.Append(" Cp_Id=\"" + dsCB.Tables[0].Rows[i]["cp_id"].ToString() + "\" ");
					_sb.Append(" Spell=\"" + System.Security.SecurityElement.Escape(dsCB.Tables[0].Rows[i]["Spell"].ToString()) + "\" ");
					_sb.Append(" Cb_Name=\"" + System.Security.SecurityElement.Escape(dsCB.Tables[0].Rows[i]["cb_name"].ToString()) + "\" ");
					_sb.Append(" OldCs_Id=\"" + dsCB.Tables[0].Rows[i]["OldCs_Id"].ToString() + "\" />");
				}
				_sb.Append("</Brand>");
			}

			if (dsCS != null && dsCS.Tables[0].Rows.Count > 0)
			{
				//DataSet dsPic = base.GetAllSerialPicAndCount();
				XmlDocument imgXml = base.GetAllSerialConverImgAndCount();
				_sb.Append("<Serial>");
				for (int i = 0; i < dsCS.Tables[0].Rows.Count; i++)
				{
					_sb.Append("<Item Cs_Id=\"" + dsCS.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					_sb.Append(" Cb_Id=\"" + dsCS.Tables[0].Rows[i]["cb_id"].ToString() + "\" ");
					_sb.Append(" Cp_Id=\"" + dsCS.Tables[0].Rows[i]["cp_id"].ToString() + "\" ");
					_sb.Append(" Spell=\"" + System.Security.SecurityElement.Escape(dsCS.Tables[0].Rows[i]["Spell"].ToString()) + "\" ");
					_sb.Append(" Cs_Name=\"" + System.Security.SecurityElement.Escape(dsCS.Tables[0].Rows[i]["cs_name"].ToString().Trim()) + "\" ");
					_sb.Append(" Cs_ShowName=\"" + System.Security.SecurityElement.Escape(dsCS.Tables[0].Rows[i]["Cs_ShowName"].ToString().Trim()) + "\" ");
					if (dsCS.Tables[0].Rows[i]["cs_seoname"].ToString().Trim() != "")
					{ _sb.Append(" Cs_SEOName=\"" + System.Security.SecurityElement.Escape(dsCS.Tables[0].Rows[i]["cs_seoname"].ToString().Trim()) + "\" "); }
					else
					{ _sb.Append(" Cs_SEOName=\"" + System.Security.SecurityElement.Escape(dsCS.Tables[0].Rows[i]["Cs_ShowName"].ToString().Trim()) + "\" "); }
					//if (dsPic != null && dsPic.Tables.Count > 1 && dsPic.Tables[1].Rows.Count > 0)
					//{
					//	DataRow[] drsPic = dsPic.Tables[1].Select(" SerialId = '" + dsCS.Tables[0].Rows[i]["cs_id"].ToString() + "' ");
					//	if (drsPic != null && drsPic.Length > 0)
					//	{
					//		_sb.Append(" ImageCommonClassId=\"" + drsPic[0]["ImageId"].ToString() + "\" ");
					//	}
					//	else
					//	{
					//		_sb.Append(" ImageCommonClassId=\"\" ");
					//	}
					//}
					if (imgXml != null)
					{
						XmlNode imgNode = imgXml.SelectSingleNode("/SerialList/Serial[@SerialId='" + dsCS.Tables[0].Rows[i]["cs_id"].ToString() + "']");
						string imgid = string.Empty;
						if (imgNode != null)
						{
							imgid = imgNode.Attributes["ImageId"].Value;
						}
						_sb.Append(" ImageCommonClassId=\"" + imgid + "\"");
					}
					else
					{
						_sb.Append(" ImageCommonClassId=\"\" ");
					}
					// _sb.Append(" ImageCommonClassId=\"" + dsCS.Tables[0].Rows[i]["CommonClassId"].ToString() + "\" ");
					_sb.Append(" OldCb_Id=\"" + dsCS.Tables[0].Rows[i]["OldCb_Id"].ToString() + "\" />");
				}
				_sb.Append("</Serial>");
			}

			if (dsCar != null && dsCar.Tables[0].Rows.Count > 0)
			{
				_sb.Append("<Basic>");
				for (int i = 0; i < dsCar.Tables[0].Rows.Count; i++)
				{
					_sb.Append("<Item Car_Id=\"" + dsCar.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
					_sb.Append(" Cs_Id=\"" + dsCar.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					_sb.Append(" Cb_Id=\"" + dsCar.Tables[0].Rows[i]["cb_id"].ToString() + "\" ");
					_sb.Append(" Cp_Id=\"" + dsCar.Tables[0].Rows[i]["cp_id"].ToString() + "\" ");
					_sb.Append(" Car_Name=\"" + System.Security.SecurityElement.Escape(dsCar.Tables[0].Rows[i]["car_name"].ToString()).Replace("\"", "") + "\" ");
					_sb.Append(" Car_SaleState=\"" + dsCar.Tables[0].Rows[i]["Car_SaleState"].ToString() + "\" ");
					string carYearType = "";
					if (dsCar.Tables[0].Rows[i]["Car_YearType"].ToString() != "" && dsCar.Tables[0].Rows[i]["Car_YearType"].ToString().Length >= 4)
					{
						carYearType = dsCar.Tables[0].Rows[i]["Car_YearType"].ToString().Substring(2, 2) + "款";
					}
					_sb.Append(" Car_YearType=\"" + carYearType + "\" ");
					_sb.Append(" OldCar_Id=\"" + dsCar.Tables[0].Rows[i]["OldCar_Id"].ToString() + "\" />");
				}
				_sb.Append("</Basic>");
			}

			_sb.Append("</AutoCarChannel>");
			Response.Write(_sb.ToString());

			#endregion
		}
	}
}