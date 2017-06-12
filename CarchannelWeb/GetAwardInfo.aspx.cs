using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannel.CarchannelWeb
{
	public partial class GetAwardInfo : PageBase
	{
		public string AwardHtml = string.Empty;
		public string YearName = string.Empty;
		public string AwardName = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);
			var hasAwardId = Request["awardId"];
			if (!string.IsNullOrEmpty(hasAwardId))
			{
				int awardId = 0;
				var awardIdResult = int.TryParse(Request["awardId"], out awardId);
				if (awardIdResult)
				{
					var hasYear = Request["year"];
					if (!string.IsNullOrEmpty(hasYear))
					{
						int year = 0;
						var yearResult = int.TryParse(Request["year"], out year);
						if (yearResult)
						{
							hidYear.Value = Request["year"];
							YearName = hidYear.Value;
						}
					}
					var awardInfo = GetData(awardId);
					if (awardInfo != null)
					{
						if (!string.IsNullOrEmpty(YearName) && awardInfo.YearInfos.FirstOrDefault(m => m.YearName == YearName) == null)
						{
							Response.Redirect("/404error.aspx?info=无效数据");							
						}
						AwardName = awardInfo.AwardName;
						AwardHtml = CreateHtml(awardInfo);
					}
					else
					{
						Response.Redirect("/404error.aspx?info=无效数据");
					}
				}
			}
		}

		private string CreateHtml(AwardInfo awardInfo)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("<div class=\"bt_page\">");
			#region
			stringBuilder.Append("<div class=\"jiangxiang_box\">");
			stringBuilder.Append("<div class=\"img_info\"><img src=\"http://image.bitauto.com/" + awardInfo.AwardLogo + "\"></div>");
			stringBuilder.Append("<dl class=\"text_info\">");
			//stringBuilder.Append("<dt><h1>" + awardInfo.AwardName + "</h1><span><a href=\"" + (string.IsNullOrEmpty(awardInfo.AwardOfficialUrl) ? "#" : awardInfo.AwardOfficialUrl) + "\">官网</a></span></dt>");

			stringBuilder.Append("<dt>");
			if (awardInfo.AwardName.Length > 39)
			{
				stringBuilder.Append("<h1>" + awardInfo.AwardName.Substring(0, 39) + "</h1>");	
			}
			else
			{
				stringBuilder.Append("<h1>" + awardInfo.AwardName + "</h1>");
			}
			if (!string.IsNullOrEmpty(awardInfo.AwardOfficialUrl))
			{
				stringBuilder.Append("<span><a target=\"_blank\" href=\"" + awardInfo.AwardOfficialUrl + "\">官网</a></span>");					
			}
			stringBuilder.Append("</dt>");
			stringBuilder.Append("<dd>" + awardInfo.AwardIntro + "</dd>");
			stringBuilder.Append("</dl>");
			stringBuilder.Append("</div>");
			#endregion
			stringBuilder.Append("<div class=\"line-box\">");
			stringBuilder.Append("<div class=\"title-con min-heightbox\">");
			#region
			stringBuilder.Append("<div class=\"title-box\">");
			stringBuilder.Append("<h3>历届获奖车型</h3>");
			stringBuilder.Append("</div>");
			#endregion
			stringBuilder.Append("<div class=\"time-award\">");
			#region
			foreach (var yearInfo in awardInfo.YearInfos)
			{
				stringBuilder.Append("<div id=\"y" + yearInfo.YearName + "\" class=\"time-box\">");
				stringBuilder.Append("<div class=\"year\">" + yearInfo.YearName + "年</div>");
				stringBuilder.Append("<div class=\"car-box\">");
				stringBuilder.Append("<div class=\"car-box-con\">");
				foreach (var childAwardSerialCar in yearInfo.ChildAwardSerialCars)
				{
					stringBuilder.Append("<dl>");
					var childInfo = childAwardSerialCar.SerialCarInfos;
					if (!string.IsNullOrEmpty(childAwardSerialCar.ChildAwardName) && childInfo != null)
					{
						stringBuilder.Append("<dt>" + childAwardSerialCar.ChildAwardName + "</dt>");
					}
					if (childInfo != null)
					{
						foreach (var serialCarInfo in childAwardSerialCar.SerialCarInfos)
						{
							
							if (serialCarInfo.CsId == null)
							{
								var csNameArray = serialCarInfo.CsName.Split(',');
								foreach (var csName in csNameArray)
								{
									stringBuilder.Append("<dd title=\"此车型官方未引进或已停产\">" + csName + "</dd>");
								}
							}
							else
							{
								stringBuilder.Append("<dd>");
								stringBuilder.Append("<a href=\"/" + serialCarInfo.AllSpell + "/\" target=\"_blank\">" + serialCarInfo.CsName +
								                     "</a>");
								stringBuilder.Append("</dd>");
							}
						}
					}
					stringBuilder.Append("</dl>");
				}
				stringBuilder.Append("</div>");
				stringBuilder.Append("</div>");
				stringBuilder.Append("<div class=\"ico-car-award-dot\"></div>");
				stringBuilder.Append("</div>");
			}
			#endregion
			stringBuilder.Append("<div class=\"blank-block\"></div>");
			stringBuilder.Append("</div>");
			stringBuilder.Append("</div>");
			stringBuilder.Append("</div>");
			stringBuilder.Append("</div>");
			return stringBuilder.ToString();
		}

		private AwardInfo GetData(int awardId)
		{
			var serialAwardDt = SerialAwardsDal.GetAward(awardId);
			if (serialAwardDt == null || serialAwardDt.Rows.Count == 0)
			{
				return null;
			}
			var awardInfo = new AwardInfo();
			awardInfo.AwardId = int.Parse(serialAwardDt.Rows[0]["Id"].ToString());
			awardInfo.AwardIntro = serialAwardDt.Rows[0]["Summary"].ToString();
			awardInfo.AwardLogo = serialAwardDt.Rows[0]["LogoUrl"].ToString();
			awardInfo.AwardName = serialAwardDt.Rows[0]["AwardsName"].ToString();
			awardInfo.AwardOfficialUrl = serialAwardDt.Rows[0]["OfficialUrl"].ToString();
			var yearArray = SerialAwardsDal.GetYears(awardId);
			awardInfo.YearInfos = GetYearInfos(awardId,yearArray);
			return awardInfo;
		}

		private List<YearInfo> GetYearInfos(int awardId, string[] yearArray)
		{
			if (yearArray == null || yearArray.Length == 0)
			{
				return null;
			}
			var yearInfos = new List<YearInfo>();
			foreach (var year in yearArray)
			{
				var yearInfo = GetYearData(awardId, year);
				if (yearInfo.ChildAwardSerialCars != null && yearInfo.ChildAwardSerialCars.Count > 0)
				{
					yearInfos.Add(yearInfo);
					//if (yearInfos.Count == 8)
					//{
					//    return yearInfos;
					//}
				}
			}
			return yearInfos;
		}

		private YearInfo GetYearData(int awardId,string year)
		{
			var yearInfo = new YearInfo();
			yearInfo.YearName = year;
			yearInfo.ChildAwardSerialCars = GetChildAwardSerialCars(awardId,year);
			return yearInfo;
		}

		private List<ChildAwardSerialCar> GetChildAwardSerialCars(int awardId, string year)
		{
			var dt = SerialAwardsDal.GetCars(awardId, int.Parse(year));
			var childAwardSerialCars = new List<ChildAwardSerialCar>();
			//先插入当前奖项下未获得子奖项的车系
			var childAwardCs = new ChildAwardSerialCar();
			childAwardCs.ChildAwardName = string.Empty;
			var serialCars = GetSerialCarInfos(awardId, int.Parse(year), 0);
			if (serialCars != null && serialCars.Count > 0)
			{
				childAwardCs.SerialCarInfos = serialCars;
				childAwardSerialCars.Add(childAwardCs);
			}
			foreach (DataRow row in dt.Rows)
			{
				var childAwardName = row["ChildAwardName"].ToString();
				var childAwardId = row["Id"].ToString();
				if (string.IsNullOrEmpty(childAwardName) || string.IsNullOrEmpty(childAwardId))
				{
					continue;
				}

				var childAwardSerialCar = new ChildAwardSerialCar();
				childAwardSerialCar.ChildAwardName = childAwardName;
				var serialCarInfos = GetSerialCarInfos(awardId, int.Parse(year), int.Parse(childAwardId));
				if (serialCarInfos != null && serialCarInfos.Count > 0)
				{
					childAwardSerialCar.SerialCarInfos = serialCarInfos;
					childAwardSerialCars.Add(childAwardSerialCar);
				}
			}
			return childAwardSerialCars;
		}

		private List<SerialCarInfo> GetSerialCarInfos(int awardId, int year, int childAwardId) //传入前已判断非空
		{
			var dt = SerialAwardsDal.GetCarSerials(awardId, year, childAwardId);
			if (dt == null)
			{
				return null;
			}
			var serialCarInfos = new List<SerialCarInfo>();
			foreach (DataRow row in dt.Rows)
			{
				var serialCarInfo = new SerialCarInfo();
				if (row["cs_Id"] != DBNull.Value)
				{
					serialCarInfo.CsId = int.Parse(row["cs_Id"].ToString());
				}
				serialCarInfo.AllSpell = row["allSpell"].ToString();
				var csName = row["csName"].ToString();
				var csWriteName = row["csWriteName"].ToString();
				serialCarInfo.CsName = string.IsNullOrEmpty(csName)?csWriteName:csName;
				//serialCarInfo.CsImageUrl = Car_SerialBll.GetSerialImageUrl(serialCarInfo.CsId,"1");
				//var price = GetSerialPriceRangeByID(serialCarInfo.CsId);
				//serialCarInfo.Price = string.IsNullOrEmpty(price) ? "暂无报价" : price;
				serialCarInfos.Add(serialCarInfo);
			}
			return serialCarInfos;
		}
	}

	
}