using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;

namespace WirelessWeb
{
	public partial class CarSummary : WirelessPageBase
	{

		#region member

		protected int CarId = 0;
		protected string CarName = string.Empty;
		protected CarEntity Ce;
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
		protected EnumCollection.CarInfoForCarSummary Cfcs = new EnumCollection.CarInfoForCarSummary();
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
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				SetPageCache(30);
				GetPageParam();
				GetCarData();
			}
		}

		#region private Method

		/// <summary>
		/// 取页面参数
		/// </summary>
		private void GetPageParam()
		{
			string strCarId = Request.QueryString["CarID"];
			if (!string.IsNullOrEmpty(strCarId)
				&& int.TryParse(strCarId, out CarId))
			{ }
		}

		/// <summary>
		/// 取车型数据
		/// </summary>
		private void GetCarData()
		{
			if (CarId > 0)
			{
				Ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, CarId);
				if (Ce != null && Ce.Id > 0)
				{
					GetCarFocusImage();
					ComputerCarPrice();
					RenderCarPriceHtml();
					GetCarParam();
					//GetCarListByCsId();
					GetCarKouBei();
				}
			}
		}

		/// <summary>
		/// 生成报价Html
		/// </summary>
		private void RenderCarPriceHtml()
		{
			if (Ce.SaleState.Trim() == "停销")
			{
				Dictionary<int, string> dic = new Car_BasicBll().GetAllUcarPrice();
				if (dic.ContainsKey(CarId))
				{
					UcarPrice = dic[CarId];
				}
				else
				{
					UcarPrice = "暂无报价";
				}
			}
		}

		/// <summary>
		/// 取同子品牌下其他车型
		/// </summary>
		private void GetCarListByCsId()
		{
			//在售车系：提取全部未上市+在售车款。
			//停售车系：提取全部车款。
			//未上市车系：提取全部车款
			if (Ce.Id > 0 && Ce.Serial.Id > 0)
			{
                //if (Ce.Serial.SaleState == "停销" || Ce.Serial.SaleState == "待销")
                //{
                //    _ls = base.GetAllCarInfoForSerialSummaryByCsID(Ce.Serial.Id, true);
                //}
                //else
                //{
                //    _ls = base.GetAllCarInfoForSerialSummaryByCsID(Ce.Serial.Id);
                //}
                _ls = base.GetAllCarInfoForSerialSummaryByCsID(Ce.Serial.Id, true);
				if (_ls.Count > 0)
				{
					var year = string.Empty;
					_ls.Sort(NodeCompare.CompareCarByYear);
					List<string> listCarList = new List<string>(20);
					foreach (EnumCollection.CarInfoForSerialSummary cifss in _ls)
					{
						string referPrice = cifss.ReferPrice.Trim();
						if (referPrice.Length == 0)
						{
							referPrice = "暂无报价";
						}
						if (referPrice != "停售" && referPrice != "暂无报价")
						{
							referPrice += "万";
						}

						if (!string.IsNullOrEmpty(cifss.CarYear) && cifss.CarYear != year)
						{
							year = cifss.CarYear;
							listCarList.Add("<dt><span>" + year + "款</span></dt>");
						}

						if (cifss.CarID == CarId)
						{
							var ddHtml = "<dd class=\"current\"><a href=\"#\"><p>" + cifss.CarName + "</p><strong>" +
										 referPrice + "</strong></a></dd>";
							listCarList.Add(ddHtml);
						}
						else
						{
							listCarList.Add("<dd><a href=\"/" + Ce.Serial.AllSpell + "/m" + cifss.CarID + "/\" ><p>" + cifss.CarName + "</p><strong>" + referPrice + "</strong></a></dd>");
						}
					}
					CarList = string.Concat(listCarList.ToArray());
				}
			}
		}

		/// <summary>
		/// 计算车款贷款信息
		/// </summary>
		private void ComputerCarPrice()
		{
			var priceComputer = new CarPriceComputer(CarId);
			priceComputer.ComputeCarPrice();
			priceComputer.LoanPaymentYear = 3;
			priceComputer.ComputeCarAutoLoan();
			double loan = 0;
			if (priceComputer.LoanFirstDownPayments > 0)
				loan = priceComputer.LoanFirstDownPayments + priceComputer.AcquisitionTax + priceComputer.Compulsory + priceComputer.Insurance + priceComputer.VehicleTax + priceComputer.Chepai;

			TotalPay = string.IsNullOrEmpty(priceComputer.FormatTotalPrice) ? "暂无" : priceComputer.FormatTotalPrice + "元";
			LoanDownPay = loan > 0 ? (loan / 10000).ToString("F2") + "万元" : "暂无";

			MonthPay = priceComputer.LoanMonthPayments > 0 ? priceComputer.LoanMonthPayments + "元" : "暂无";
			CankaoPrice = GetCarPriceByID(CarId);
			if (string.IsNullOrEmpty(CankaoPrice))
			{
				CankaoPrice = "暂无";
			}
		}

		/// <summary>
		/// 取车型完整参数
		/// </summary>
		private void GetCarParam()
		{
			Cfcs = base.GetCarInfoForCarSummaryByCarID(CarId);
			Dictionary<int, string> dict = new Car_BasicBll().GetCarAllParamByCarID(CarId);
			//add by 2014.05.04 电动车参数
			IsElectrombile = dict.ContainsKey(578) && dict[578] == "电力"; //是否是电动车

			BatteryCapacity = dict.ContainsKey(876) ? dict[876] : "";//电池容量
			PowerConsumptive100 = dict.ContainsKey(868) ? dict[868] : "";//百公里耗电
			Mileage = dict.ContainsKey(883) ? dict[883] : "";//续航里程
			ChargeTime = dict.ContainsKey(879) ? dict[879] : "";//充电时间
			Exhaust = (dict.ContainsKey(425) && dict[425] == "增压") ? Cfcs.Engine_Exhaust.Replace("L", "T") : Cfcs.Engine_Exhaust;//排量
            //减税 购置税
            double dEx = 0.0;
            Double.TryParse(Cfcs.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
            if (dict.ContainsKey(987) && (dict[987] == "第1批" || dict[987] == "第2批" || dict[987] == "第3批" || dict[987] == "第4批" || dict[987] == "第5批" || dict[987] == "第6批") && dict.ContainsKey(986))
            {
                if(dict[986].ToString() == "减半")
                {
                    TaxContent = "购置税减半";
                }
                else if(dict[986].ToString() == "免征")
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
		}


		/// <summary>
		/// 生成车型颜色块Html
		/// </summary>
		private string GetCarColorHtml(List<string> listNameColor)
		{
			string colorHtml = "";
			var listColorHtml = new List<string>();
			DataSet dsAllCsColor = new Car_SerialBll().GetAllSerialColorRGB();
			DataRow[] drs = dsAllCsColor.Tables[0].Select(" cs_id='" + Ce.Serial.Id + "' ");
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

		/// <summary>
		/// 取车型焦点图
		/// </summary>
		private void GetCarFocusImage()
		{
			XmlDocument doc = new Car_BasicBll().GetCarDefaultPhoto(Ce.Serial.Id, CarId, Ce.CarYear);
			if (doc != null && doc.HasChildNodes)
			{
				XmlNodeList xnl = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
				if (xnl != null && xnl.Count > 0)
				{
					ImgLink = xnl[0].Attributes["Link"].Value;
					ImgLink = ImgLink.Replace(".bitauto", ".m.yiche");
					PicUrl = xnl[0].Attributes["ImageUrl"].Value;

					var xmlCarId = xnl[0].Attributes["CarId"].Value;
					if (CarId != int.Parse(xmlCarId))
					{
						string carYear = xnl[0].Attributes["CarYear"].Value;
						string carModelName = xnl[0].Attributes["CarModelName"].Value;
						CarName = string.Format("当前车款暂无图片，图片显示为:<br>{0}款 {1}", carYear, carModelName);
					}
				}
			}
			else
			{
				// 用子品牌焦点图
				List<SerialFocusImage> imgList = new Car_SerialBll().GetSerialFocusImageList(Ce.Serial.Id);
				if (imgList.Count > 0)
				{
					SerialFocusImage csImg = imgList[0];
					string bigImgUrl = csImg.ImageUrl;
					if (csImg.ImageId > 0)
					{
						ImgLink = csImg.TargetUrl.Replace(".bitauto", ".m.yiche");
						PicUrl = String.Format(bigImgUrl, 4);
					}
				}
				else
				{
					PicUrl = "http://image.bitautoimg.com/autoalbum/V2.1/images/300-200.gif";
					//PicUrl = WebConfig.DefaultCarPic;
				}
			}
			PicUrl = PicUrl.Replace("_2.", "_3.");
			PicUrl = PicUrl.Replace("_4.", "_3.");
		}

		private void GetCarKouBei()
		{
			XmlDocument doc = new Car_BasicBll().GetCarKouBei(CarId);
			if (doc != null && doc.HasChildNodes)
			{
				XmlNodeList xnl = doc.SelectNodes("/UserCarList/Item");
				var stringBuilder = new StringBuilder();
				stringBuilder.Append("<div class=\"tt-first\">");
				stringBuilder.Append("<h3>口碑</h3>");
				stringBuilder.Append("<div class=\"opt-more opt-more-gray\"><a href=\"/" + Ce.Serial.AllSpell + "/koubei/m" + Ce.Id + "/#acCar\">更多</a></div>");
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
						stringBuilder.Append("<h3>" + Ce.CarYear + "款 " + Ce.Name + "</h3>");
						stringBuilder.Append("<div class=\"p-1\">");
						stringBuilder.Append("<span class=\"name\">" + itemNode.SelectSingleNode("./UserName").InnerText + "</span>");
						//stringBuilder.Append("<div class=\"kb-rank\">");
						//stringBuilder.Append("<span class=\"big-star\"><em style=\"width: 65%\"></em></span>");
						//stringBuilder.Append("<strong></strong>");
						//stringBuilder.Append("</div>");
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
								stringBuilder.Append("<span>" + koubeiCreateTime.ToString("yyyy-MM-dd") + "</span><span>已行驶" + topicListItem.SelectSingleNode("./Mileage").InnerText + "公里</span><span>实测油耗: " + fuel + "L</span>");
								stringBuilder.Append("</p>");
								stringBuilder.Append("<div class=\"cont-txt\">");
								stringBuilder.Append("<div class=\"cont-nr-box\">");
								stringBuilder.Append("<a href=\"/" + Ce.Serial.AllSpell + "/koubei/" + topicListItem.SelectSingleNode("./TopicId").InnerText + "/\" class=\"more-msg\">");
								stringBuilder.Append("<h4>" + topicListItem.SelectSingleNode("./Title").InnerText + "</h4>");
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
		}

		#endregion

	}
}