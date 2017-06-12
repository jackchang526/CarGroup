using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 子品牌各级级别信息，报价，论坛地址(胡利军)
	/// </summary>
	public partial class AllSerialInfo : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();
		private bool onlyIdAndName;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);
			onlyIdAndName = false;
			if (Request["onlyIdAndName"] != null && Request["onlyIdAndName"].ToLower() == "true")
				onlyIdAndName = true;
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<SerialInfo>");
			GetSerialInfo();
			sb.Append("</SerialInfo>");
			Response.Write(sb.ToString());
		}

		private void GetSerialInfo()
		{
			DataSet ds = new DataSet();
			if (HttpContext.Current.Cache.Get("interfaceforbitauto_AllSerialInfo") != null)
			{
				ds = (System.Data.DataSet)HttpContext.Current.Cache.Get("interfaceforbitauto_AllSerialInfo");
			}
			else
			{
				string sql = @"SELECT  ISNULL(cp.cp_id, 0) AS cp_id, cp.Cp_ShortName, cmb.bs_id, cmb.bs_name,
                                        cmb.urlSpell AS bsAllSpell, cb.cb_id, cb.cb_name,
                                        cb.allspell AS cbAllSpell, cs.cs_id, cs.cs_name,
                                        cs.allspell AS csAllSpell, cs.cs_ShowName, cs.cs_CarLevel,
                                        cs.cs_seoname
                                FROM    car_serial cs
                                        LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                        LEFT JOIN Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                        LEFT JOIN Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                                        LEFT JOIN Car_Producer cp ON cb.cp_id = cp.cp_id
                                WHERE   cs.isState = 1
                                        AND cb.isState = 1
                                        AND cp.isState = 1
                                ORDER BY cp.cp_id, cmb.bs_id, cb.cb_id, cs.cs_id";
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				HttpContext.Current.Cache.Insert("interfaceforbitauto_AllSerialInfo", ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
			}


			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Serial ID=\"" + ds.Tables[0].Rows[i]["cs_id"].ToString().Trim() + "\" ");
					sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(
						ds.Tables[0].Rows[i]["cs_name"].ToString().Trim()) + "\" ");
					sb.Append(" CsShowName=\"" + System.Security.SecurityElement.Escape(
						ds.Tables[0].Rows[i]["cs_ShowName"].ToString().Trim()) + "\" ");
					if (!onlyIdAndName)
						sb.Append(" CsAllSpell=\"" + ds.Tables[0].Rows[i]["csAllSpell"].ToString().Trim() + "\" ");
					sb.Append(" CbID=\"" + ds.Tables[0].Rows[i]["cb_id"].ToString().Trim() + "\" ");
					sb.Append(" CbName=\"" + System.Security.SecurityElement.Escape(
						ds.Tables[0].Rows[i]["cb_name"].ToString().Trim()) + "\" ");
					if (!onlyIdAndName)
						sb.Append(" CbAllSpell=\"" + ds.Tables[0].Rows[i]["cbAllSpell"].ToString().Trim() + "\" ");
					sb.Append(" BsID=\"" + ds.Tables[0].Rows[i]["bs_id"].ToString().Trim() + "\" ");
					sb.Append(" BsName=\"" + System.Security.SecurityElement.Escape(
						ds.Tables[0].Rows[i]["bs_name"].ToString().Trim()) + "\" ");
					if (!onlyIdAndName)
					{
						sb.Append(" BsAllSpell=\"" + ds.Tables[0].Rows[i]["bsAllSpell"].ToString().Trim() + "\" ");
						sb.Append(" CpID=\"" + ds.Tables[0].Rows[i]["cp_id"].ToString().Trim() + "\" ");
						sb.Append(" CpShortName=\"" + System.Security.SecurityElement.Escape(
							ds.Tables[0].Rows[i]["Cp_ShortName"].ToString().Trim()) + "\" ");
						sb.Append(" CsPrice=\"" + base.GetSerialPriceRangeByID(int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString().Trim())) + "\" ");
						sb.Append(" CsBBS=\"" + new Car_SerialBll().GetForumUrlBySerialId(int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString().Trim())) + "\" ");
						// modified by chengl Aug.25.2010
						sb.Append(" CsLevel=\"" + ds.Tables[0].Rows[i]["cs_CarLevel"].ToString().Trim() + "\" ");
						sb.Append(" CsLevelURL=\"" + GetCsLevelURLByName(ds.Tables[0].Rows[i]["cs_CarLevel"].ToString().Trim()) + "\" ");
					}
					sb.Append(" CsSEOName=\"" + System.Security.SecurityElement.Escape(
							ds.Tables[0].Rows[i]["cs_seoname"].ToString().Trim()) + "\" ");
					sb.Append("/>");
				}
			}
		}

		// 根据级别名去级别页地址
		private string GetCsLevelURLByName(string levelName)
		{
			string url = "";
			switch (levelName)
			{
				case "微型车": url = "weixingche"; break;
				case "小型车": url = "xiaoxingche"; break;
				case "紧凑型车": url = "jincouxingche"; break;
				case "中大型车": url = "zhongdaxingche"; break;
				case "中型车": url = "zhongxingche"; break;
				case "豪华车": url = "haohuaxingche"; break;
				case "MPV": url = "mpv"; break;
				case "SUV": url = "suv"; break;
				case "跑车": url = "paoche"; break;
				case "其它": url = "qita"; break;
				case "面包车": url = "mianbaoche"; break;
				case "皮卡": url = "pika"; break;
				default: url = ""; break;
			}
			return url;
		}

	}
}