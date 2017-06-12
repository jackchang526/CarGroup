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
	/// 车型对比统计数据
	/// </summary>
	public partial class ForCarCompareList : InterfacePageBase
	{
		private int carID = 0;
		private int topCount = 5;
		private string xmlPath = "http://carser.bitauto.com/forpicmastertoserial/CarCompareStat/{0}.xml";
		private bool isContainSameSerial = false;
		private StringBuilder sb = new StringBuilder();
		// private List<CarData> lcd = new List<CarData>();
		private List<int> lcount = new List<int>();
		private int currentCsID = 0;
		private string temp = string.Empty;
		private string tempCurrent = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			temp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
			if (!this.IsPostBack)
			{
				this.CheckPageParam();
				if (carID != 0 && topCount != 0)
				{
					temp += "<Car ID=\"" + carID.ToString() + "\" {0} >";
					GetCarCompareDataByID();
					temp += "{1}";
					temp += "</Car>";
				}
			}
			Response.Write(string.Format(temp, tempCurrent, sb.ToString()));
		}

		/// <summary>
		/// 取参数
		/// </summary>
		private void CheckPageParam()
		{
			if (this.Request.QueryString["carID"] != null && this.Request.QueryString["carID"].ToString() != "")
			{
				string carIDStr = this.Request.QueryString["carID"].ToString();
				if (int.TryParse(carIDStr, out carID))
				{ }
				else
				{
					carID = 0;
				}
			}
			if (this.Request.QueryString["top"] != null && this.Request.QueryString["top"].ToString() != "")
			{
				string top = this.Request.QueryString["top"].ToString();
				if (int.TryParse(top, out topCount))
				{
					if (topCount < 0 || topCount > 1000)
					{
						topCount = 5;
					}
				}
				else
				{
					topCount = 0;
				}
			}
			if (this.Request.QueryString["isContainSameSerial"] != null && this.Request.QueryString["isContainSameSerial"].ToString() != "")
			{
				if (this.Request.QueryString["isContainSameSerial"].ToString().Trim() == "1")
				{
					isContainSameSerial = true;
				}
				else
				{
					isContainSameSerial = false;
				}
			}
		}

		private void GetCarCompareDataByID()
		{

			string sql = " select car.car_id,car.car_name,cs.cs_id,cs.cs_name ";
			sql += " from car_basic car ";
			sql += " left join car_serial cs on car.cs_id = cs.cs_id ";
			sql += " where car.isState=1 and cs.isState=1 ";
			DataSet ds = new DataSet();
			if (HttpContext.Current.Cache.Get("ForCarCompareList_AllData") != null)
			{
				ds = (DataSet)HttpContext.Current.Cache.Get("ForCarCompareList_AllData");
			}
			else
			{
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				HttpContext.Current.Cache.Insert("ForCarCompareList_AllData", ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
			}
			// DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(base.szConnString, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow[] dr = ds.Tables[0].Select(" car_id = " + carID.ToString());
				if (dr.Length > 0)
				{
					currentCsID = int.Parse(dr[0]["cs_id"].ToString());
					string currentCarName = dr[0]["car_name"].ToString();
					string currentCsName = dr[0]["cs_name"].ToString();
					tempCurrent = "CarName=\"" + System.Security.SecurityElement.Escape(currentCarName) + "\" CsID=\"" + currentCsID.ToString() + "\" CsName=\"" + System.Security.SecurityElement.Escape(currentCsName) + "\"";
				}
			}
			else
			{ return; }


			DataSet dsCompare = new Car_BasicBll().GetCarCompareListByCarID(carID);
			if (dsCompare != null && dsCompare.Tables.Count > 0 && dsCompare.Tables[0].Rows.Count > 0)
			{
				int loop = 0;
				for (int i = 0; i < dsCompare.Tables[0].Rows.Count; i++)
				{
					if (loop >= 5)
					{ break; }
					if (currentCsID.ToString() == dsCompare.Tables[0].Rows[i]["cs_id"].ToString())
					{
						continue;
					}
					sb.Append("<Item CarID=\"" + dsCompare.Tables[0].Rows[i]["cCarID"].ToString() + "\" ");
					sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(dsCompare.Tables[0].Rows[i]["Car_Name"].ToString()) + "\" ");
					sb.Append(" CsID=\"" + dsCompare.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(dsCompare.Tables[0].Rows[i]["cs_showname"].ToString()) + "\" />");
					loop++;
				}
			}
		}
	}
}