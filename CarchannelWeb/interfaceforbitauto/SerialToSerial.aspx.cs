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
	///  子品牌还关注接口数据(刘荣伟)
	/// </summary>
	public partial class SerialToSerial : InterfacePageBase
	{
		private string hotTop10Serial = string.Empty;
		private bool isAllSerial = false;
		private StringBuilder sb = new StringBuilder();
		private Dictionary<int, bool> dicAllSerial = new Dictionary<int, bool>();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				GetTop10HotSerial();
				GetAllSerial();
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<SerialList>");
				this.GetSerialToSerial();
				sb.Append("</SerialList>");
				Response.Write(sb.ToString());
			}
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["isAll"] != null && this.Request.QueryString["isAll"].ToString() == "1")
			{
				isAllSerial = true;
			}
		}

		private void GetSerialToSerial()
		{
			//string catchkey = "interfaceforbitauto_SerialToSerial";
			//object interfaceforbitauto_SerialToSerial = null;
			DataSet ds = new DataSet();
			//CacheManager.GetCachedData(catchkey, out interfaceforbitauto_SerialToSerial);
			//if (interfaceforbitauto_SerialToSerial != null)
			//{
			//    ds = (DataSet)interfaceforbitauto_SerialToSerial;
			//}
			//else
			//{
			string sql = " select cs_id,pcs_id,pv_num from dbo.Serial_To_Serial order by cs_id,pv_num desc ";
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			//    CacheManager.InsertCache(catchkey, ds, 10);
			//}

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				string currentCsID = "";
				string pcsIDs = "";
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					if (currentCsID != ds.Tables[0].Rows[i]["cs_id"].ToString().Trim())
					{
						// 不同子品牌
						if (currentCsID != "")
						{
							sb.Append("<Item ID=\"" + currentCsID + "\" Other=\"" + pcsIDs + "\" />");
							if (dicAllSerial.ContainsKey(int.Parse(currentCsID)))
							{ dicAllSerial[int.Parse(currentCsID)] = true; }
						}
						pcsIDs = ds.Tables[0].Rows[i]["pcs_id"].ToString().Trim();
						currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString().Trim();
					}
					else
					{
						// 相同子品牌
						if (pcsIDs != "")
						{
							pcsIDs += "," + ds.Tables[0].Rows[i]["pcs_id"].ToString().Trim();
						}
						else
						{
							pcsIDs = ds.Tables[0].Rows[i]["pcs_id"].ToString().Trim();
						}
					}
				}
				sb.Append("<Item ID=\"" + currentCsID + "\" Other=\"" + pcsIDs + "\" />");
				if (dicAllSerial.ContainsKey(int.Parse(currentCsID)))
				{ dicAllSerial[int.Parse(currentCsID)] = true; }
				if (isAllSerial)
				{
					// 补齐其他子品牌
					sb.Append("<!-- 补齐其他子品牌 -->");
					foreach (KeyValuePair<int, bool> kvp in dicAllSerial)
					{
						if (!kvp.Value)
						{
							sb.Append("<Item ID=\"" + kvp.Key + "\" Other=\"" + hotTop10Serial + "\" />");
						}
					}
				}
			}
		}

		/// <summary>
		/// 取UV前10子品牌 补齐还关注
		/// </summary>
		private void GetTop10HotSerial()
		{
			string sql = "select top 10 cs_id  from dbo.Car_Serial_30UV order by UVCount desc";
			DataSet dsTop10 = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			if (dsTop10 != null && dsTop10.Tables.Count > 0 && dsTop10.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsTop10.Tables[0].Rows)
				{
					if (hotTop10Serial != "")
					{ hotTop10Serial += "," + dr["cs_id"].ToString(); }
					else
					{ hotTop10Serial = dr["cs_id"].ToString(); }
				}
			}
		}

		/// <summary>
		/// 取所有有效的子品牌
		/// </summary>
		private void GetAllSerial()
		{
			string sql = "select cs_id from car_serial where isState=1 order by cs_id";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int csid = int.Parse(dr["cs_id"].ToString());
					if (!dicAllSerial.ContainsKey(csid))
					{ dicAllSerial.Add(csid, false); }
				}
			}
		}
	}
}