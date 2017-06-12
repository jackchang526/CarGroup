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
	/// <summary>
	/// 子品牌昨天&前天排行(刘荣伟)
	/// </summary>
	public partial class SerialSortList : InterfacePageBase
	{
		private int top = 0;
		StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.CheckParam();
				this.GenerateSerialSortData();
			}
		}

		private void CheckParam()
		{
			if (this.Request.QueryString["top"] != null && this.Request.QueryString["top"].ToString() != "")
			{
				string sTop = this.Request.QueryString["top"].ToString();
				if (int.TryParse(sTop, out top))
				{
					if (top < 0)
					{
						top = 0;
					}
				}
			}
		}

		private void GenerateSerialSortData()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<SerialSortList>");
			this.GetDateForSerialData(ref sb);
			sb.Append("</SerialSortList>");
			Response.Write(sb.ToString());
		}

		private void GetDateForSerialData(ref StringBuilder _sb)
		{
			DataSet dsYes = new DataSet();
			if (HttpContext.Current.Cache.Get("InterfaceForBitautoSerialSortListYes") != null)
			{
				dsYes = (DataSet)HttpContext.Current.Cache.Get("InterfaceForBitautoSerialSortListYes");
			}
			else
			{
				// modified by chengl Dec.7.2009 取前台数据
				string sqlYes = " select csp.cs_id,csp.pv_sumnum as FeignedPV, ";
				sqlYes += " Rank() OVER(ORDER  BY csp.pv_sumnum desc) AS Rank, ";
				sqlYes += " cs.oldcb_id,cs.cs_name,cs.cs_showName,cs.allspell,csp.createDateStr ";
				sqlYes += " from dbo.Chart_Serial_Pv csp  ";
				sqlYes += " left join dbo.Car_Serial cs on csp.cs_id = cs.cs_id ";
				sqlYes += " where csp.createDateStr>=convert(varchar(10),getdate()-2,120)  ";
				sqlYes += " and csp.createDateStr<convert(varchar(10),getdate()-1,120) ";
				sqlYes += " and  cs.cs_CarLevel is not null ";
				sqlYes += " order by csp.pv_sumnum desc ";
				dsYes = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlYes);
				HttpContext.Current.Cache.Insert("InterfaceForBitautoSerialSortListYes", dsYes, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
			}

			DataSet dsbeforeyes = new DataSet();
			if (HttpContext.Current.Cache.Get("InterfaceForBitautoSerialSortListBeforeyes") != null)
			{
				dsbeforeyes = (DataSet)HttpContext.Current.Cache.Get("InterfaceForBitautoSerialSortListBeforeyes");
			}
			else
			{
				string sqlbeforeYes = " select csp.cs_id,csp.pv_sumnum as FeignedPV, ";
				sqlbeforeYes += " Rank() OVER(ORDER  BY csp.pv_sumnum desc) AS Rank, ";
				sqlbeforeYes += " cs.oldcb_id,cs.cs_name,cs.cs_showName,cs.allspell,csp.createDateStr ";
				sqlbeforeYes += " from dbo.Chart_Serial_Pv csp  ";
				sqlbeforeYes += " left join dbo.Car_Serial cs on csp.cs_id = cs.cs_id ";
				sqlbeforeYes += " where csp.createDateStr>=convert(varchar(10),getdate()-3,120)  ";
				sqlbeforeYes += " and csp.createDateStr<convert(varchar(10),getdate()-2,120) ";
				sqlbeforeYes += " and  cs.cs_CarLevel is not null ";
				sqlbeforeYes += " order by csp.pv_sumnum desc ";
				dsbeforeyes = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlbeforeYes);
				HttpContext.Current.Cache.Insert("InterfaceForBitautoSerialSortListBeforeyes", dsbeforeyes, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
			}

			if (dsYes != null && dsYes.Tables.Count > 0 && dsYes.Tables[0].Rows.Count > 0 && dsbeforeyes != null && dsbeforeyes.Tables.Count > 0 && dsbeforeyes.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsYes.Tables[0].Rows.Count; i++)
				{
					if (top > 0 && i == top)
					{
						return;
					}
					_sb.Append("<Item CsID=\"" + dsYes.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					_sb.Append(" CsName=\"" + dsYes.Tables[0].Rows[i]["cs_name"].ToString() + "\" ");
					_sb.Append(" CsShowName=\"" + dsYes.Tables[0].Rows[i]["cs_showName"].ToString() + "\" ");
					_sb.Append(" CsAllSpell=\"" + dsYes.Tables[0].Rows[i]["allspell"].ToString() + "\" ");
					_sb.Append(" YesSort=\"" + dsYes.Tables[0].Rows[i]["Rank"].ToString() + "\" ");
					_sb.Append(" BefYesSort=\"" + GetBefYesSort(int.Parse(dsYes.Tables[0].Rows[i]["cs_id"].ToString()), "Rank", dsbeforeyes.Tables[0]) + "\" />");
				}
			}
		}

		private string GetBefYesSort(int csID, string colName, DataTable dt)
		{
			string temp = "0";
			DataRow[] drs = dt.Select(" cs_id = " + csID.ToString() + " ");
			if (drs != null && drs.Length > 0)
			{
				temp = drs[0][colName].ToString();
			}
			return temp;
		}
	}
}