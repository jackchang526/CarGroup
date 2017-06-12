using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	public partial class GetCarHotCompareList : PageBase
	{
		private int carID = 0;
		private int top = 6;
		private string carYear = string.Empty;
		private Car_BasicEntity cbe = new Car_BasicEntity();
		private StringBuilder sb = new StringBuilder();
		private StringBuilder sbCompare = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				GetCarHotCompare();
				GenerateHTML();
				Response.Write(sb.ToString());
			}
		}

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{
			if (this.Request.QueryString["carID"] != null && this.Request.QueryString["carID"].ToString() != "")
			{
				string strCarID = this.Request.QueryString["carID"].ToString();
				if (int.TryParse(strCarID, out carID))
				{ }
			}
		}

		/// <summary>
		/// 取车型的对比车型列表
		/// </summary>
		private void GetCarHotCompare()
		{
			if (carID > 0)
			{
				string catchkeyCar = "CarSummary_Car" + carID.ToString();
				object carInfoByCarID = null;
				CacheManager.GetCachedData(catchkeyCar, out carInfoByCarID);
				if (carInfoByCarID == null)
				{
					cbe = (new Car_BasicBll()).Get_Car_BasicByCarID(carID);
					CacheManager.InsertCache(catchkeyCar, cbe, 60);
				}
				else
				{
					cbe = (Car_BasicEntity)carInfoByCarID;
				}
				carYear = cbe.Car_YearType > 0 ? cbe.Car_YearType.ToString() + "款 " : "";
				string currentCsID = cbe.Cs_Id.ToString();
				int loop = 1;
				DataSet dsCompare = new Car_BasicBll().GetCarCompareListByCarID(carID);
				if (dsCompare != null && dsCompare.Tables.Count > 0 && dsCompare.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < dsCompare.Tables[0].Rows.Count; i++)
					{
						if (loop > top)
						{ break; }
						if (dsCompare.Tables[0].Rows[i]["cs_id"].ToString() == currentCsID)
						{ continue; }
						loop++;
						string csPic = "";
						int csPicCount = 0;
						string compareYear = dsCompare.Tables[0].Rows[i]["Car_YearType"].ToString().Trim() == "" ? "" : dsCompare.Tables[0].Rows[i]["Car_YearType"].ToString().Trim() + "款 ";
						base.GetSerialPicAndCountByCsID(int.Parse(dsCompare.Tables[0].Rows[i]["cs_id"].ToString().Trim()), out csPic, out csPicCount, false);
						sbCompare.Append("<li><a target=\"_blank\" href=\"/" + dsCompare.Tables[0].Rows[i]["allSpell"].ToString().Trim().ToLower() + "/m" + dsCompare.Tables[0].Rows[i]["cCarID"].ToString().Trim() + "/\">");
						sbCompare.Append("<img alt=\"" + compareYear + dsCompare.Tables[0].Rows[i]["cs_name"].ToString().Trim() + " " + dsCompare.Tables[0].Rows[i]["car_name"].ToString().Trim() + "\" height=\"80\" width=\"120\" src=\"" + csPic.Replace("{0}", "2") + "\" alt=\"\"></a>");
						sbCompare.Append("<div class=\"name\"> <a title=\"" + compareYear + dsCompare.Tables[0].Rows[i]["cs_name"].ToString().Trim() + " " + dsCompare.Tables[0].Rows[i]["car_name"].ToString().Trim() + "\" target=\"_blank\" href=\"/" + dsCompare.Tables[0].Rows[i]["allSpell"].ToString().Trim().ToLower() + "/m" + dsCompare.Tables[0].Rows[i]["cCarID"].ToString().Trim() + "/\">" + compareYear + dsCompare.Tables[0].Rows[i]["cs_name"].ToString().Trim() + " " + dsCompare.Tables[0].Rows[i]["car_name"].ToString().Trim() + "</a></div>");
						sbCompare.Append("<a href=\"javascript:addCarToCompareFromlist(" + dsCompare.Tables[0].Rows[i]["cCarID"].ToString().Trim() + ",'" + dsCompare.Tables[0].Rows[i]["car_name"].ToString().Trim().Replace("'", "‘") + "','" + dsCompare.Tables[0].Rows[i]["cs_name"].ToString().Trim().Replace("'", "‘") + "');\" >加入对比</a></li>");
					}
				}
			}
		}

		private void GenerateHTML()
		{
			if (sbCompare.Length > 0)
			{
				// 有对比车型 
				sb.Append("<div class=\"line_box dbzd\">");
				sb.Append("<h3>");
				sb.Append("<span>与<strong>" + carYear + cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim() + "</strong>对比最多的车</span></h3>");
				sb.Append("<ul>");
				sb.Append(sbCompare.ToString());
				sb.Append("</ul>");
				sb.Append("<div class=\"clear\"></div></div>");
			}
			else
			{
				// 没有对比车型
			}
		}

	}
}