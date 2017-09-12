using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace MWeb.Controllers
{
	public class CarSummaryController : Controller
	{
		//
		// GET: /CarSummary/
		#region member

		protected int CarId = 0;
		protected string CarName = string.Empty;
		protected string PicUrl = string.Empty;
		protected string ImgLink = string.Empty;
		protected string PicMiaoShu = string.Empty;
		protected string CarList = string.Empty;//车款列表
		protected string Exhaust = string.Empty;//排量 判断 增压方式
		protected bool IsElectrombile = false;//是否是电动车
		protected string PowerConsumptive100 = string.Empty;//百公里耗电
		protected string BatteryCapacity = string.Empty;//电池容量
		protected string Mileage = string.Empty;//续航里程
		protected string ChargeTime = string.Empty;//充电时间
		protected EnumCollection.CarInfoForCarSummary cfcs;
		protected string EngineAllString = string.Empty;//发动机
		protected string TransmissionType = string.Empty;//变速箱(档位数)
		protected string ColorHeader = string.Empty;//车身颜色
		private List<EnumCollection.CarInfoForSerialSummary> _ls = new List<EnumCollection.CarInfoForSerialSummary>();
		protected string UcarPrice = string.Empty;//二手车报价
		protected string KouBeiHtml = string.Empty;//口碑html
		//参考成交价
		protected string CankaoPrice = string.Empty;
		//贷款首付
		protected string LoanDownPay = string.Empty;
		//月付
		protected string MonthPay = string.Empty;
		//预估总价
		protected string TotalPay = string.Empty;
		//车身颜色块
		protected string ColorHtml = string.Empty;
		//购置税内容
		protected string TaxContent = string.Empty;
		protected CarEntity ce;
		protected PageBase pageBase;
		protected Car_BasicBll car_Basic;
		protected Car_SerialBll car_SerialBll;
		protected CarPriceComputer priceComputer;
		#endregion

		public CarSummaryController()
		{
			cfcs = new EnumCollection.CarInfoForCarSummary();
			ce = new CarEntity();
			pageBase = new PageBase();
			car_Basic = new Car_BasicBll();
			car_SerialBll = new Car_SerialBll();
		}
		[OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream, VaryByParam = "*")]
		public ActionResult Index(int carid)
		{
			//GetPageParam(RouteData.Values);
			CarId = carid;
			GetCarData();
			if (ce == null || ce.Id == 0 || ce.Serial == null)
			{
				Response.Redirect("/error", true);
				return new EmptyResult();
			}
			return View(ce);
		}
		#region private Method

		/// <summary>
		/// 取车型数据
		/// </summary>
		private void GetCarData()
		{
			if (CarId > 0)
			{
				ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, CarId);
				if (ce != null && ce.Id > 0 && ce.Serial != null)
				{
					ComputerCarPrice();
					RenderCarPriceHtml();
					GetCarParam();
					GetCarKouBei();
				}
				//else
				//{
				//	Response.Redirect("/error", true);
				//}
			}
		}

		/// <summary>
		/// 生成报价Html
		/// </summary>
		private void RenderCarPriceHtml()
		{
			if (ce.SaleState.Trim() == "停销")
			{
				Dictionary<int, string> dic = car_Basic.GetAllUcarPrice();
				if (dic.ContainsKey(CarId))
				{
					UcarPrice = dic[CarId];
				}
				else
				{
					UcarPrice = "暂无报价";
				}
			}
			ViewBag.UcarPrice = UcarPrice;
		}

		/// <summary>
		/// 计算车款贷款信息
		/// </summary>
		private void ComputerCarPrice()
		{
			priceComputer = new CarPriceComputer(CarId);
			priceComputer.ComputeCarPrice();
			priceComputer.LoanPaymentYear = 3;
			priceComputer.ComputeCarAutoLoan();
			double loan = 0;
			if (priceComputer.LoanFirstDownPayments > 0)
				loan = priceComputer.LoanFirstDownPayments + priceComputer.AcquisitionTax + priceComputer.Compulsory + priceComputer.Insurance + priceComputer.VehicleTax + priceComputer.Chepai;

			TotalPay = string.IsNullOrEmpty(priceComputer.FormatTotalPrice) ? "暂无" : priceComputer.FormatTotalPrice + "元";
			LoanDownPay = loan > 0 ? (loan / 10000).ToString("F2") + "万元" : "暂无";

			MonthPay = priceComputer.LoanMonthPayments > 0 ? priceComputer.LoanMonthPayments + "元" : "暂无";
			CankaoPrice = pageBase.GetCarPriceByID(CarId);
			if (string.IsNullOrEmpty(CankaoPrice))
			{
				CankaoPrice = "暂无";
			}
			ViewBag.TotalPay = TotalPay;
			ViewBag.LoanDownPay = LoanDownPay;
			ViewBag.MonthPay = MonthPay;
			ViewBag.CankaoPrice = CankaoPrice;
		}

		/// <summary>
		/// 取车型完整参数
		/// </summary>
		private void GetCarParam()
		{
			cfcs = priceComputer.CarEntity;
			//if (cfcs.CarID < 1)
			//{
			//    Response.Redirect("~/error");
			//}
			Dictionary<int, string> dict = car_Basic.GetCarAllParamByCarID(CarId);
			//add by 2014.05.04 电动车参数
			IsElectrombile = dict.ContainsKey(578) && dict[578] == "电力"; //是否是电动车

			BatteryCapacity = dict.ContainsKey(876) ? dict[876] : "";//电池容量
			PowerConsumptive100 = dict.ContainsKey(868) ? dict[868] : "";//百公里耗电
			Mileage = dict.ContainsKey(883) ? dict[883] : "";//续航里程
			ChargeTime = dict.ContainsKey(879) ? dict[879] : "";//充电时间
			Exhaust = (dict.ContainsKey(425) && dict[425].Contains("增压")) ? (string.IsNullOrEmpty(cfcs.Engine_Exhaust) ? "" : cfcs.Engine_Exhaust.Replace("L", "T")) : cfcs.Engine_Exhaust;//排量
			//减税 购置税
			double dEx = 0.0;
			if (!string.IsNullOrEmpty(cfcs.Engine_Exhaust))
			{
				Double.TryParse(cfcs.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
			}
			if (dict.ContainsKey(987) && (dict[987] == "第1批" || dict[987] == "第2批" || dict[987] == "第3批" || dict[987] == "第4批" || dict[987] == "第5批" || dict[987] == "第6批") && dict.ContainsKey(986))
			{
				if (dict[986].ToString() == "减半")
				{
					TaxContent = "购置税减半";
				}
				else if (dict[986].ToString() == "免征")
				{
					TaxContent = "免征购置税";
				}
			}
			else if (dEx > 0 && dEx <= 1.6)
			{
				TaxContent = "购置税75折";
			}

			// 最大功率—功率值 气缸排列型式 汽缸数
			string engineMaxPower = dict.ContainsKey(430) ? dict[430] + "kw " : "";
			string engineCylinderRank = dict.ContainsKey(418) ? dict[418] : "";
			if (engineCylinderRank.StartsWith("L") || engineCylinderRank.StartsWith("V") || engineCylinderRank.StartsWith("B") || engineCylinderRank.StartsWith("W"))
			{ engineCylinderRank = engineCylinderRank.Substring(0, 1); }
			else
			{ engineCylinderRank = ""; }
			string engineCylinderNum = dict.ContainsKey(417) ? dict[417] : "";
			EngineAllString = engineMaxPower + engineCylinderRank + engineCylinderNum;//发动机

			// 车身颜色
			string carColors = dict.ContainsKey(598) ? dict[598].Replace("，", ",") : "";
			var listColor = new List<string>();
			if (carColors != "")
			{
				string[] colorArray = carColors.Split(',');
				if (colorArray.Length > 0)
				{
					foreach (string color in colorArray)
					{
						if (!listColor.Contains(color))
						{ listColor.Add(color); }
					}
				}
			}
			// 车型车身颜色 色块
			ColorHeader = GetCarColorHtml(listColor);

			//赋值到viewbag
			ViewBag.Cfcs = cfcs;
			ViewBag.IsElectrombile = IsElectrombile;
			ViewBag.PowerConsumptive100 = PowerConsumptive100;
			ViewBag.BatteryCapacity = BatteryCapacity;
			ViewBag.Mileage = Mileage;
			ViewBag.ChargeTime = ChargeTime;
			ViewBag.Exhaust = Exhaust;
			ViewBag.TaxContent = TaxContent;
			ViewBag.EngineAllString = EngineAllString;
			ViewBag.ColorHeader = ColorHeader;
		}


		/// <summary>
		/// 生成车型颜色块Html
		/// </summary>
		private string GetCarColorHtml(List<string> listNameColor)
		{
			string colorHtml = "";
			var listColorHtml = new List<string>();
			DataSet dsAllCsColor = car_SerialBll.GetAllSerialColorRGB();
			DataRow[] drs = dsAllCsColor.Tables[0].Select("cs_id='" + ce.Serial.Id + "'");
			if (drs.Length > 0)
			{
				foreach (DataRow dr in drs)
				{
					if (dr["colorName"].ToString().Trim() != ""
						&& listNameColor.Contains(dr["colorName"].ToString().Trim()))
					{
						listColorHtml.Add("<em style=\"background:" + dr["colorRGB"].ToString().Trim() + "\"></em>");
					}
				}
			}
			if (listColorHtml.Count > 0)
			{
				listColorHtml.Insert(0, "<dd>");
				listColorHtml.Add("</dd>");
				colorHtml = string.Concat(listColorHtml.ToArray());
			}
			return colorHtml;
		}

		private void GetCarKouBei()
		{
			StringBuilder stringBuilder = new StringBuilder();
			XmlDocument doc = car_Basic.GetCarKouBei(CarId);
			if (doc != null && doc.HasChildNodes)
			{
				XmlNodeList xnl = doc.SelectNodes("/UserCarList/Item");
				stringBuilder.Append("<div class=\"tt-first\">");
				stringBuilder.Append("<h3>口碑</h3>");
				stringBuilder.Append("<div class=\"opt-more opt-more-gray\"><a href=\"http://car.m.yiche.com/" +
									 ce.Serial.AllSpell + "/koubei/m" + ce.Id + "/#acCar\">更多</a></div>");
				stringBuilder.Append("</div>");
				stringBuilder.Append("<div class=\"kb-list-cont\" data-channelid=\"27.24.1333\">");
				if (xnl != null && xnl.Count > 0)
				{
					int loop = 0;
					foreach (XmlNode itemNode in xnl)
					{
						if (loop == 3)
						{
							break;
						}
						var topicListItems = itemNode.SelectNodes("./TopicList/Item");
						if (!(topicListItems != null && topicListItems.Count > 0))
						{
							continue;
						}
						stringBuilder.Append("<div class=\"kb-part\">");
						stringBuilder.Append("<div class=\"user-info\">");
						stringBuilder.Append("<span class=\"user-img\">");
						stringBuilder.Append("<img src=\"" + itemNode.SelectSingleNode("./UserImage").InnerText + "\" alt=\"\">");
						stringBuilder.Append("</span>");
						stringBuilder.Append("<div class=\"user-tit\">");
						stringBuilder.Append("<h3>" + ce.CarYear + "款 " + ce.Name + "</h3>");
						stringBuilder.Append("<div class=\"p-1\">");
						stringBuilder.Append("<span class=\"name\">" +
											 itemNode.SelectSingleNode("./UserName").InnerText + "</span>");
						stringBuilder.Append("</div></div></div>");
						if (topicListItems != null)
						{
							stringBuilder.Append("<div class=\"kb-txt\">");
							foreach (XmlNode topicListItem in topicListItems)
							{
								var fuel = decimal.Parse(topicListItem.SelectSingleNode("./Fuel").InnerText);
								fuel = Math.Round(fuel, 2);
								stringBuilder.Append("<p class=\"p-txt\">");
								var koubeiCreateTime =
									DateTime.Parse(topicListItem.SelectSingleNode("./CreateTime").InnerText);
								stringBuilder.Append("<span>" + koubeiCreateTime.ToString("yyyy-MM-dd") +
													 "</span><span>已行驶" +
													 topicListItem.SelectSingleNode("./Mileage").InnerText +
													 "公里</span><span>实测油耗: " + fuel + "L</span>");
								stringBuilder.Append("</p>");
								stringBuilder.Append("<div class=\"cont-txt\">");
								stringBuilder.Append("<div class=\"cont-nr-box\">");
								stringBuilder.Append("<a href=\"http://car.m.yiche.com/" + ce.Serial.AllSpell +
													 "/koubei/" +
													 topicListItem.SelectSingleNode("./TopicId").InnerText +
													 "/\" class=\"more-msg\">");
								stringBuilder.Append("<h4>" + topicListItem.SelectSingleNode("./Title").InnerText +
													 "</h4>");
								var kouBeiContent = topicListItem.SelectSingleNode("./Content").InnerText;
								if (kouBeiContent.Length > 100)
								{
									kouBeiContent = kouBeiContent.Substring(0, 100);
									stringBuilder.Append("<p>" + kouBeiContent + "</p>");
									stringBuilder.Append("<span class=\"more-btn\"></span>");
								}
								else
								{
									stringBuilder.Append("<p>" + kouBeiContent + "</p>");
								}
								stringBuilder.Append("</a>");
								stringBuilder.Append("</div>");
								stringBuilder.Append("</div>");
							}
							stringBuilder.Append("</div>");
						}
						stringBuilder.Append("</div>");
						loop++;
					}
				}
				stringBuilder.Append("</div>");
				KouBeiHtml = stringBuilder.ToString();
			}
			ViewBag.KouBeiHtml = KouBeiHtml;
		}

		#endregion
	}
}
