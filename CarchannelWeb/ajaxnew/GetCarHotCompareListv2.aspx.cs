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
	public partial class GetCarHotCompareListv2 : PageBase
	{
		private int carID = 0;
		private int top = 6;
		private string carYear = string.Empty;
		private Car_BasicEntity cbe = new Car_BasicEntity();
		private StringBuilder sb = new StringBuilder();
		private StringBuilder sbCompare = new StringBuilder();
		private bool isHasHotCarList = false;
		private bool isHasSameCarList = false;
		private string sqlSame = @"select car.car_id,car.car_name,cs.cs_id,cs.cs_name,
											car.Car_YearType 
											from car_basic car
											left join car_serial cs on car.cs_id=cs.cs_id
											where car.isState=1 and cs.isState=1 and
											car.car_SaleState='在销' and car.cs_id = {0}
											order by car.Car_YearType desc";

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);
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
						string compareYear = dsCompare.Tables[0].Rows[i]["Car_YearType"].ToString().Trim() == "" ? "" : dsCompare.Tables[0].Rows[i]["Car_YearType"].ToString().Trim() + "款";
						sbCompare.AppendLine("<li id=\"liHot_" + dsCompare.Tables[0].Rows[i]["cCarID"].ToString().Trim() + "\">" + dsCompare.Tables[0].Rows[i]["cs_name"].ToString().Trim() + " <span>" + dsCompare.Tables[0].Rows[i]["car_name"].ToString().Trim() + " " + compareYear + "</span>");
						sbCompare.AppendLine(" <a href=\"javascript:addCarToCompareFromlist(" + dsCompare.Tables[0].Rows[i]["cCarID"].ToString().Trim() + ",'" + dsCompare.Tables[0].Rows[i]["car_name"].ToString().Trim() + "','" + dsCompare.Tables[0].Rows[i]["cs_name"].ToString().Trim() + "');\">+对比</a></li>");
					}

				}
				if (loop > 1)
				{
					// 有热门对比车型
					isHasHotCarList = true;
					sbCompare.Insert(0, "<ul id=\"ulHotCarList\">");
					sbCompare.AppendLine("</ul>");
				}

				int loopSame = 1;
				DataSet dsSameCs = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, string.Format(sqlSame, cbe.Cs_Id));
				if (dsSameCs != null && dsSameCs.Tables.Count > 0 && dsSameCs.Tables[0].Rows.Count > 1)
				{
					isHasSameCarList = true;
					if (isHasHotCarList)
					{
						sbCompare.AppendLine("<ul id=\"ulSameCarList\" style=\"display:none;\">");
					}
					else
					{
						sbCompare.AppendLine("<ul id=\"ulSameCarList\" >");
					}
					foreach (DataRow dr in dsSameCs.Tables[0].Rows)
					{
						// 输出在销全部车型
						//if (loopSame > top)
						//{ break; }
						// 当前相同车 跳过
						if (dr["Car_ID"].ToString().Trim() == carID.ToString())
						{ continue; }
						loopSame++;
						string compareYear = dr["Car_YearType"].ToString().Trim() == "" ? "" : dr["Car_YearType"].ToString().Trim() + "款";
						sbCompare.AppendLine("<li id=\"liSame_" + dr["Car_ID"].ToString().Trim() + "\">" + dr["cs_name"].ToString().Trim() + " <span>" + dr["car_name"].ToString().Trim() + " " + compareYear + "</span>");
						sbCompare.AppendLine(" <a href=\"javascript:addCarToCompareFromlist(" + dr["Car_ID"].ToString().Trim() + ",'" + dr["car_name"].ToString().Trim() + "','" + dr["cs_name"].ToString().Trim() + "');\">+对比</a></li>");
					}
					sbCompare.AppendLine("</ul>");
				}

			}
		}

		private void GenerateHTML()
		{
			if (sbCompare.Length > 0)
			{
				// 有对比车型 
				sb.AppendLine("<h3>");
				if (isHasHotCarList)
				{
					// 有热门对比车
					sb.AppendLine("<a id=\"aHotCarList\" class=\"noneLine\" onclick=\"$('#aHotCarList').html('<b>大家都用他和谁比</b>');$('#aSameCarList').html('同系车型对比');$('#aHotCarList').addClass('noneLine');$('#aSameCarList').removeClass('noneLine');$('#ulSameCarList').hide();$('#ulHotCarList').show();\"  href=\"javascript:void(0);\"><b>大家都用他和谁比</b></a>");
					if (isHasSameCarList)
					{
						sb.AppendLine(" | ");
						sb.AppendLine("<a id=\"aSameCarList\" onclick=\"$('#aHotCarList').html('大家都用他和谁比');$('#aSameCarList').html('<b>同系车型对比</b>');$('#aHotCarList').removeClass('noneLine');$('#aSameCarList').addClass('noneLine');$('#ulSameCarList').show();$('#ulHotCarList').hide();\"  href=\"javascript:void(0);\">同系车型对比</a>");
					}
				}
				else
				{
					if (isHasSameCarList)
					{ sb.AppendLine("<a id=\"aSameCarList\" class=\"noneLine\"  href=\"javascript:void(0);\"><b>同系车型对比</b></a>"); }
				}
				sb.AppendLine("</h3>");
				sb.AppendLine(sbCompare.ToString());
				sb.AppendLine("<a class=\"btn\" href=\"javascript:addAllCarToCompare();\">全部加入对比</a>");
				sb.AppendLine("<a class=\"first_close\" href=\"javascript:closeHotCompare();\">关闭</a>");
				sb.AppendLine("<div id=\"tipbox_direction_up\" class=\"tipbox_direction_up\"><em>◆</em><span>◆</span></div>");
			}
			else
			{
				// 没有对比车型
			}
		}

	}
}