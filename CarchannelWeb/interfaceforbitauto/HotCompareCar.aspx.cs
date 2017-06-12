using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class HotCompareCar : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();
		// private string xmlPath = "http://carser.bitauto.com/forpicmastertoserial/HotCar.xml";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<CompareCar>");
				GetCompareData();
				sb.Append("</CompareCar>");
				Response.Write(sb.ToString());
			}
		}

		private void GetCompareData()
		{
			DataSet dsCs = base.GetAllSErialInfo();

			//        string sql = @"select top 5 csid,comparecsid,sum(count) as compareCount from dbo.StCompareCity0
			//                                    group by csid,comparecsid
			//                                    order by compareCount desc";
			string sql = @"select top 5 csid,othercsid as comparecsid, compareCount
									from Car_CsCompareList
									order by compareCount desc";
			//string sql = " select top 5 * ";
			//sql += " from dbo.CompareEveryDay ";
			//sql += " where CompareTime>='" + DateTime.Now.AddDays(-2).ToShortDateString() + "' and CompareTime<'" + DateTime.Now.AddDays(-1).ToShortDateString() + "'";
			//sql += " order by [sum] desc ";
			DataSet dsCompare = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			if (dsCompare != null && dsCompare.Tables.Count > 0 && dsCompare.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsCompare.Tables[0].Rows.Count; i++)
				{
					string carID1 = "";
					string carID2 = "";
					string name1 = "";
					string name2 = "";
					if (dsCs != null && dsCs.Tables.Count > 0 && dsCs.Tables[0].Rows.Count > 0)
					{
						DataRow[] drs = dsCs.Tables[0].Select(" cs_id = " + dsCompare.Tables[0].Rows[i]["csid"].ToString() + " ");
						if (drs != null && drs.Length > 0)
						{
							carID1 = drs[0]["cs_id"].ToString();
							name1 = drs[0]["cs_showname"].ToString().Trim().Replace("&", "&amp;");
						}
						DataRow[] drs2 = dsCs.Tables[0].Select(" cs_id = " + dsCompare.Tables[0].Rows[i]["comparecsid"].ToString() + " ");
						if (drs2 != null && drs2.Length > 0)
						{
							carID2 = drs2[0]["cs_id"].ToString();
							name2 = drs2[0]["cs_showname"].ToString().Trim().Replace("&", "&amp;");
						}
					}

					sb.Append("<Item Name1=\"" + BitAuto.Utils.StringHelper.SubString(name1, 18, true) + "\" ");
					sb.Append(" Name2=\"" + BitAuto.Utils.StringHelper.SubString(name2, 18, true) + "\" ");
					sb.Append(" URL=\"http://car.bitauto.com/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?csids=" + carID1 + "," + carID2 + "\" />");
				}
			}
			
		}
	}
}