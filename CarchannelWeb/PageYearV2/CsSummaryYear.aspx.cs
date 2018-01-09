using System;
using System.Xml;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Interface;
using System.IO;

namespace BitAuto.CarChannel.CarchannelWeb.PageYearV2
{
	public partial class CsSummaryYear : PageBase
	{
		protected int serialId;
		protected int carYear;
		protected string serialSeoName;
		protected string masterBrandName;
		protected string serialSpell;
		protected string serialName;
		protected string serialShowName;
		protected string serialEncodeName;
		protected string serialReferPrice;
		protected string serialPrice;
		protected string serialExhaust = string.Empty;
		protected string serialExhaustalt = string.Empty;
		protected string serialTransmission = string.Empty;
		protected string CsPicJiaodian;			//子品牌焦点图
		protected string CsDetailInfo;			//子品牌概况
		protected string CsMustSeeInfo;			//子品牌必看
		protected string SerialInfoBarHtml;		//顶部的信息条

        //年款焦点图参数
        protected int focusPhotoCount = 0;
        protected string focusPhotoCountUrl = string.Empty;

		protected string topCarListHtml = string.Empty;
		protected string carListTableHtml;
		protected string focusNewsHtml;
		protected string xinwenHtml;
		protected string hangqingHtml;
		protected string shijiaHtml;
		protected string daogouHtml;
		protected string forumHtml;
		protected string rainbowHtml;
		protected string serialImageHtml;
		protected string videosHtml;
		protected string dianpingHtml;
		protected string hotNewsHtml;
		protected string serialToSeeHtml;
		protected string hotSerialCompareHtml;
		protected string intensionHtml;
		protected string CsHead;
		protected string JsTagForYear;
		protected string UserBlock;
		protected string baaUrl;
		protected string serialAskHtml = string.Empty;
        protected string taxContent = string.Empty; //减税标识

		protected string nextSeePingceHtml;
		protected string nextSeeXinwenHtml;
		protected string nextSeeDaogouHtml;

		protected EnumCollection.SerialInfoCard sic;	//子品牌名片
		protected Car_SerialEntity cse;				//子品牌信息
		private string baseUrl;
		protected Car_SerialBaseEntity serialBaseInfo;
		//private string netFuel;

		protected string UCarHtml = string.Empty;
		protected string serialUCarPrice = string.Empty;//二手车报价
		protected string referPrice = string.Empty;//指导价报价区间
		protected string minPriceRange = string.Empty;//参考最低报价区间
		protected string transmissionTypes = string.Empty;//年款变速箱
		protected bool isElectrombile = false;//是否是全系电动车
		protected string chargeTimeRange = string.Empty;//充电时间区间
		protected string fastChargeTimeRange = string.Empty;//快充时间区间
		protected string mileageRange = string.Empty;//续航里程区间

		protected string koubeiImpressionHtml = string.Empty;//口碑印象html块
		protected string serialColorHtml = string.Empty;//卡片区 颜色html块
        protected string serialColorDisplayHtml = string.Empty;//卡片区 颜色鼠标悬浮替换图片html块
		protected string shijiaOrHuimaiche = string.Empty;//低价 惠买车
		protected string chedaiADLink = string.Empty;//贷款链接



        protected bool isNoSaleYear = true;	// 当前年款是否是停销年款
		private Dictionary<int, string> dicUcarPrice;	// 二手车报价区间
		private Dictionary<int, string> dictSerialBlockHtml;//静态块内容
		private Dictionary<int, string> dictUCarPrice;//二手车价格

		private CommonHtmlBll _commonhtmlBLL;
		private Car_BasicBll _carBLL;
		private Car_SerialBll _serialBLL;

		#region 子品牌最热车型
		// 最热车型ID
		private int hotCarID = 0;
		// 最热车型热度
		private int hotCarHotCount = 0;
		#endregion

		public CsSummaryYear()
		{
			_serialBLL = new Car_SerialBll();
			_carBLL = new Car_BasicBll();
			_commonhtmlBLL = new CommonHtmlBll();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);

			GetParamter();

			base.MakeSerialTopADCode(serialId);


			//子品牌信息
			sic = _serialBLL.GetSerialInfoCard(serialId, carYear);
			if (sic.CsID == 0)
			{
				Response.Redirect("/car/404error.aspx?info=无子品牌");
			}
			cse = _serialBLL.GetSerialInfoEntity(serialId);
			serialSpell = cse.Cs_AllSpell.ToLower();
			serialShowName = cse.Cs_ShowName;
			if (sic.CsID == 1568)
				serialShowName = "索纳塔八";
			serialSeoName = cse.Cs_SeoName + " " + carYear.ToString() + "款";
			serialEncodeName = HttpUtility.UrlEncode(serialShowName);
			serialName = cse.Cs_Name;
			serialExhaust = ""; //CommonFunction.GetExhaust(sic.CsEngineExhaust);
			serialTransmission = ""; //CommonFunction.GetTransmission(sic.CsTransmissionType);
			baseUrl = "http://car.bitauto.com/" + serialSpell.ToLower() + "/";
			serialBaseInfo = _serialBLL.GetSerialBaseEntity(serialId);
			masterBrandName = cse.Cb_Name.Trim();
			baaUrl = _serialBLL.GetForumUrlBySerialId(serialId);
			// modified by chengl Apr.21.2010
			CsHead = base.GetCommonNavigation("CsSummaryForYear", serialId).Replace("{0}", carYear.ToString());
			// CsHead = base.GetCommonNavigation("CsSummary", serialId);
			//静态块内容
			dictSerialBlockHtml = _commonhtmlBLL.GetCommonHtml(serialId, CommonHtmlEnum.TypeEnum.Serial, CommonHtmlEnum.TagIdEnum.SerialSummary);
			//二手车报价区间
			dictUCarPrice = _serialBLL.GetSerialUCarYearPrice(serialId);
			//二手车价格区间
			if (dictUCarPrice.ContainsKey(carYear))
				serialUCarPrice = dictUCarPrice[carYear];
			//贷款的链接
			chedaiADLink = "http://fenqi.taoche.com/www/" + serialSpell + "/?from=yc9";
			// 子品牌车贷广告
			Dictionary<string, List<LinkADForCs>> dicLinkAD = _serialBLL.GetLinkAD();
			if (dicLinkAD.ContainsKey("AD_CsSummaryForCheDai")
				&& dicLinkAD["AD_CsSummaryForCheDai"].Count > 0)
			//	&& dicLinkAD["AD_CsSummaryForCheDai"].ListCsID.Contains(serialId)
			//	&& dicLinkAD["AD_CsSummaryForCheDai"].Link != "")
			{
				foreach (LinkADForCs lfs in dicLinkAD["AD_CsSummaryForCheDai"])
				{
					if (lfs.ListCsID.Contains(serialId) && lfs.Link != "")
					{
						chedaiADLink = lfs.Link;
						break;
					}
				}
			}
			// 惠买车地址，如果有，则替换 预约试驾
			//shijiaOrHuimaiche = string.Format("<a href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">试驾</a>", serialId);
			Dictionary<int, string> dicHuiMaiChe = _serialBLL.GetEPHuiMaiCheAllCsUrl();
			if (dicHuiMaiChe != null && dicHuiMaiChe.ContainsKey(serialId))
			{
				shijiaOrHuimaiche = string.Format("<a class=\"btn\" id=\"btnDijia\" href=\"{0}?tracker_u=11_yccx\" target=\"_blank\">买新车</a>", dicHuiMaiChe[serialId]);
			}

			//生成页面
			RenderPage();
			InitNextSee();
		}

		/// <summary>
		/// 获取参数
		/// </summary>
		private void GetParamter()
		{
			serialId = ConvertHelper.GetInteger(Request.QueryString["ID"]);
			if (serialId <= 0)
			{
				Response.Redirect("http://car.bitauto.com/");
			}
			carYear = ConvertHelper.GetInteger(Request.QueryString["year"]);
			if (carYear <= 0)
				Response.Redirect("http://car.bitauto.com/");
			JsTagForYear = "if(document.getElementById('carYearList_" + carYear.ToString() + "')){document.getElementById('carYearList_" + carYear.ToString() + "').className='current';}changeSerialYearTag(0," + carYear.ToString() + ",'');";
		}

		/// <summary>
		/// 生成页面代码
		/// </summary>
		private void RenderPage()
		{
			//MakeSerialList();
			MakeCarListHtmlNew();
			//MakeSerialFocus();
			MakeSerialFocusV2();
			MakeSerialYearColorHtml();
            //MakeKoubeiImpressionHtml();
			//MakeSerialOverview();
			MakeSerialInfoBar();
			//MakeMustSee();
			//MakeTopNews();
			// modified by chengl May.5.2011
			// MakeRainbowHtml();
			MakeSerialImages();
			//MakeVideHtml();
			//MakeDianpingHtml();
			//MakeKoubeiDianpingHtml();
			//dianpingHtml = new Car_SerialBll().GetDianpingHtml(serialId);
			//MakeHotNewsHtml();
			MakeSerialToSerialHtml();
			MakeHotSerialCompare();
			// MakeSerialIntensionHtml();
			// modified by chengl May.5.2011
			// GetUserBlockByCarSerialId();
			//MakeSerialAskHtml();
			//二手车
			//UCarHtml = new Car_SerialBll().GetUCarHtml(serialId);
		}

		private void MakeKoubeiDianpingHtml()
		{
			int koubei = (int)CommonHtmlEnum.BlockIdEnum.KoubeiReportNew;
			if (dictSerialBlockHtml.ContainsKey(koubei))
				dianpingHtml = dictSerialBlockHtml[koubei];
		}

		private void MakeSerialInfoBar()
		{
			StringBuilder htmlCode = new StringBuilder();
			if (topCarListHtml.Length > 0)
			{
				htmlCode.AppendLine("<div class=\"line_box zs01\">");
				// modified by chengl Jun.13.2011
				// htmlCode.AppendLine("<dl class=\"p\" onmouseover=\"showCarList()\" onmouseout=\"hideCarList()\">");
				// htmlCode.AppendLine("<dt id=\"tit\">" + carYear + "款</dt>");
				// htmlCode.AppendLine("<dd id=\"this\" style=\"display:none\">");
				// htmlCode.AppendLine(topCarListHtml);
				// htmlCode.AppendLine("</dd>");
				// htmlCode.AppendLine("</dl>");
				htmlCode.AppendLine("<ul class=\"s\">");
				htmlCode.AppendLine("<li class=\"s1\"><label>厂家指导价：</label>" + serialReferPrice + "</li>");
				htmlCode.AppendLine("<li class=\"s2\"><label>参考成交价：</label><span class=\"important\">" + serialPrice + "</span></li>");
				htmlCode.AppendLine("<li class=\"s3\"><label>排量：</label>" + serialExhaust + "</li>");
				htmlCode.AppendLine("<li class=\"s4\"><label>变速箱：</label>" + serialTransmission + "</li>");
				htmlCode.AppendLine("</ul>");
				htmlCode.AppendLine("<div class=\"clear\"></div>");
				htmlCode.AppendLine("</div>");
			}
			SerialInfoBarHtml = htmlCode.ToString();
		}
		#region 新版 车型列表 add by sk 2013.08.05
		/// <summary>
		/// 子品牌 车款列表 html
		/// </summary>
		/// <param name="serialId">子品牌ID</param>
		private void MakeCarListHtmlNew()
		{
			StringBuilder sb = new StringBuilder();
			List<string> carSaleListHtml = new List<string>();
			List<string> carNoSaleListHtml = new List<string>();
			List<string> carWaitSaleListHtml = new List<string>();

			List<CarInfoForSerialSummaryEntity> carinfoList = _carBLL.GetCarInfoForSerialSummaryBySerialId(serialId);
			int maxPv = 0;
			List<string> saleYearList = new List<string>();
			List<string> noSaleYearList = new List<string>();
			var currentYearCarList = carinfoList.Where(p => p.CarYear == carYear.ToString()).ToList();
			// 判断年款是否存在 modified by chengl Apr.21.2010
            if (currentYearCarList.Count <= 0)
            { Response.Redirect("/car/404error.aspx?info=无效年款"); }
            else
            {
                isNoSaleYear = currentYearCarList.Find(p => p.SaleState != "停销") != null ? false : true;
            }

            var carids =currentYearCarList.Select(p=>p.CarID);
            //购置税减免批次
            var dictPurchaseTaxParamN = _carBLL.GetCarParamValueByCarIds(carids.ToArray(), 987);
            //购置税减免
            var dictPurchaseTaxParam = _carBLL.GetCarParamValueByCarIds(carids.ToArray(), 986);
			foreach (CarInfoForSerialSummaryEntity carInfo in carinfoList)
			{
				if (carInfo.CarPV > maxPv)
					maxPv = carInfo.CarPV;
				if (carInfo.CarYear.Length > 0)
				{
					string yearType = carInfo.CarYear + "款";

					if (carInfo.SaleState == "停销")
					{
						if (!noSaleYearList.Contains(yearType))
							noSaleYearList.Add(yearType);
					}
					else
					{
						if (!saleYearList.Contains(yearType))
							saleYearList.Add(yearType);
					}
				}
                //添加减税逻辑
                if (!isNoSaleYear)
                {
                    //减税 免征
                    double dEx = 0.0;
                    Double.TryParse(carInfo.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
                    if (dictPurchaseTaxParamN.ContainsKey(carInfo.CarID) && (dictPurchaseTaxParamN[carInfo.CarID] == "第1批" || dictPurchaseTaxParamN[carInfo.CarID] == "第2批" || dictPurchaseTaxParamN[carInfo.CarID] == "第3批" || dictPurchaseTaxParamN[carInfo.CarID] == "第4批" || dictPurchaseTaxParamN[carInfo.CarID] == "第5批" || dictPurchaseTaxParamN[carInfo.CarID] == "第6批") && dictPurchaseTaxParam.ContainsKey(carInfo.CarID))
                    {
                        if (dictPurchaseTaxParam[carInfo.CarID] == "减半")
                        {
                            taxContent= "减税";
                        }
                        else if (dictPurchaseTaxParam[carInfo.CarID] == "免征")
                        {
                            taxContent= "免税";
                        }
                    }
                    else if (dEx > 0 && dEx <= 1.6)
                    {
                        taxContent= "减税";
                    }
                }
			}
			//排除包含在售年款
			foreach (string year in saleYearList)
			{
				if (noSaleYearList.Contains(year))
				{
					noSaleYearList.Remove(year);
				}
			}

			currentYearCarList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
			noSaleYearList.Sort(NodeCompare.CompareStringDesc);
			sb.Append("<div class=\"section-header header2 mb0\">");
			sb.Append("<div class=\"box\">");
			sb.AppendFormat("<h2>{0}款{1}车款</h2>", carYear, cse.Cs_SeoName);
			sb.Append("</div>");
			sb.Append("</div>");

			if (currentYearCarList.Count > 0)
			{
				//根据车型列表获取年款信息
				GetSerialYearInfoByCarList(currentYearCarList);
				sb.AppendFormat("<div class=\"list-table\">", 0);
                if (!isNoSaleYear)
                {
                    sb.Append("<table id=\"compare_sale\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">");
                }
                else
                {
                    sb.Append("<table id=\"compare_nosale\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">");
                }
				sb.Append("<colgroup><col width=\"40%\"><col width=\"8%\"><col width=\"11%\"><col width=\"10%\"><col width=\"11%\"><col width=\"20%\"></colgroup>");
                sb.Append("<tbody>");
				sb.Append(GetCarListHtml(currentYearCarList, maxPv));
				sb.Append("</tbody>");
				sb.Append("</table>");
				sb.Append("</div>");
			}
			carListTableHtml = sb.ToString();
		}
		/// <summary>
		/// 根据当前车款获取年款信息
		/// </summary>
		/// <param name="currentYearCarList">当前年款车型信息</param>
		private void GetSerialYearInfoByCarList(List<CarInfoForSerialSummaryEntity> currentYearCarList)
		{
			//厂商指导价区间
			var carReferPrice = currentYearCarList.Where(p => (p.ReferPrice != "" && p.ReferPrice != "暂无")).Select(p => ConvertHelper.GetDecimal(p.ReferPrice));
			if (carReferPrice.Count() > 0)
			{
				var minrReferPrice = carReferPrice.Min();
				var maxrReferPrice = carReferPrice.Max();
				if (minrReferPrice == maxrReferPrice)
					referPrice = minrReferPrice + "万";
				else
					referPrice = string.Format("{0}-{1}万", minrReferPrice, maxrReferPrice);
			}
			else
				referPrice = "暂无报价";
			if (isNoSaleYear)
			{
				minPriceRange = "停售";
			}
			else
			{
				//参考报价
				var carPrice = currentYearCarList.Where(p => (p.CarPriceRange != "" && p.CarPriceRange != "停售")).Select(p => ConvertHelper.GetDecimal(p.CarPriceRange.Split('-')[0].Replace("万", "")));
				if (carPrice.Count() > 0)
				{
					var minPrice = carPrice.Min();
					var maxPrice = carPrice.Max();
					if (minPrice == maxPrice)
						minPriceRange = minPrice + "万";
					else
						minPriceRange = string.Format("{0}-{1}万", minPrice, maxPrice);
				}
				else
					minPriceRange = "暂无报价";
			}
			//变速箱
			var transList = currentYearCarList.Select(p => (p.TransmissionType.IndexOf("手动") != -1 ? "手动" : "自动"))
				.GroupBy(p => p, p => p).Select(p => p.Key).ToList();
			transmissionTypes = string.Join("/", transList.ToArray());

			//add by 2014.05.04 在销车款 电动车
			var fuelTypeList = currentYearCarList.Where(p => p.Oil_FuelType != "")
											  .GroupBy(p => p.Oil_FuelType)
											  .Select(g => g.Key).ToList();
			isElectrombile = fuelTypeList.Count == 1 && fuelTypeList[0] == "纯电" ? true : false;
			//add by 2014.03.18 在销车款 排量输出
			var exhaustList = currentYearCarList.Where(p => p.Engine_Exhaust.EndsWith("L"))
				.Select(p => p.Engine_InhaleType.IndexOf("增压") >= 0 ? p.Engine_Exhaust.Replace("L", "T") : p.Engine_Exhaust)
											.GroupBy(p => p)
											.Select(group => group.Key).ToList();
			if (exhaustList.Count > 0)
			{
				exhaustList.Sort(NodeCompare.ExhaustCompareNew);
				if (exhaustList.Count > 3)
				{
					serialExhaust = string.Concat(exhaustList[0], " ", exhaustList[1]
						, "..."
						, exhaustList[exhaustList.Count - 1], fuelTypeList.Contains("纯电") ? " 电动" : "");
				}
				else
					serialExhaust = string.Join(" ", exhaustList.ToArray()) + (fuelTypeList.Contains("纯电") ? " 电动" : "");
				serialExhaustalt = string.Join(" ", exhaustList.ToArray());
			}
		}
		/// <summary>
		/// 车型列表html
		/// </summary>
		/// <param name="carList">车款列表 list</param>
		/// <param name="serialInfo">子品牌信息</param>
		/// <param name="maxPv">最大pv</param>
		/// <returns></returns>
		private string GetCarListHtml(List<CarInfoForSerialSummaryEntity> carList, int maxPv)
		{
			List<string> carListHtml = new List<string>();
			if (isNoSaleYear)
			{
				// 如果是停销年款 取二手车报价
				dicUcarPrice = new Car_BasicBll().GetAllUcarPrice();
			}
			var querySale = carList.GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower }, p => p);
			int groupIndex = 0;

			int minChargeTime = 0;
			int maxChargeTime = 0;
			int minFastChargeTime = 0;
			int maxFastChargeTime = 0;
			int minMileage = 0;
			int maxMileage = 0;

			foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in querySale)
			{
				var key = CommonFunction.Cast(info.Key, new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0, Electric_Peakpower = 0 });
				string strMaxPowerAndInhaleType = string.Empty;
				string maxPower = key.Engine_MaxPower == 9999 ? "" : key.Engine_MaxPower + "kW";
				string inhaleType = key.Engine_InhaleType;
				if (!string.IsNullOrEmpty(maxPower) || !string.IsNullOrEmpty(inhaleType))
				{
					if (inhaleType == "增压")
					{
						inhaleType = string.IsNullOrEmpty(key.Engine_AddPressType) ? inhaleType : key.Engine_AddPressType;
					}
					if (key.Electric_Peakpower > 0)
					{
						maxPower = string.Format("发动机：{0}，发电机：{1}", maxPower, key.Electric_Peakpower + "kW");
					}
					strMaxPowerAndInhaleType = string.Format("<b>/</b> {0}{1}", maxPower, " " + inhaleType);
				}

                //start 处理不同排量对应的头行
                carListHtml.Add(string.Format("<tr id=\"car_filter_gid_{0}\" class=\"table-tit\">", groupIndex));
                if (!isNoSaleYear)
                {
                    carListHtml.Add(string.Format("<th class=\"first-item\"><strong>{0}</strong> {1}</th>",
                        key.Engine_Exhaust.Replace("L", "升"),
                        strMaxPowerAndInhaleType));
                    carListHtml.Add("<th>关注度</th>");
                    carListHtml.Add("<th>变速箱</th>");
                    carListHtml.Add("<th class=\"txt-right txt-right-padding\">指导价</th>");
                    carListHtml.Add("<th class=\"txt-right\">参考最低价</th>");
                    carListHtml.Add("<th>&nbsp;</th>");
                    //carListHtml.Add("<th><div class=\"doubt\" onmouseover=\"javascript:$(this).children('.prompt-layer').show();return false;\" onmouseout=\"javascript:$(this).children('.prompt-layer').hide();return false;\"><div class=\"prompt-layer\" style=\"display:none\">全国参考最低价</div></div></th>");
                }
                else
                {
                    carListHtml.Add(string.Format("<th class=\"first-item\"><strong>{0}</strong> {1}</th>",
                        key.Engine_Exhaust.Replace("L", "升"),
                        strMaxPowerAndInhaleType));
                    carListHtml.Add("<th>关注度</th>");
                    carListHtml.Add("<th>变速箱</th>");
                    carListHtml.Add("<th class=\"txt-right txt-right-padding\">指导价</th>");
                    carListHtml.Add("<th class=\"txt-right\">二手车报价</th>");
                    carListHtml.Add("<th>&nbsp;</th>");
                }
                carListHtml.Add("</tr>");
				groupIndex++;
				List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList<CarInfoForSerialSummaryEntity>();//分组后的集合
                int trFlag = 0;//奇偶行样式标记
				foreach (CarInfoForSerialSummaryEntity entity in carGroupList)
				{
                    trFlag++;
					string yearType = entity.CarYear.Trim();
					if (yearType.Length > 0)
						yearType += "款";
					else
						yearType = "未知年款";
					string stopPrd = "";
					if (entity.ProduceState == "停产")
                        stopPrd = " <span class=\"color-block3\">停产</span>";
					Dictionary<int, string> dictCarParams = _carBLL.GetCarAllParamByCarID(entity.CarID);
					//add by 2014.05.04 获取电动车参数
					if (isElectrombile)
					{
						//普通充电时间
						if (dictCarParams.ContainsKey(879))
						{
							var chargeTime = ConvertHelper.GetInteger(dictCarParams[879]);
							if (minChargeTime == 0 && chargeTime > 0)
								minChargeTime = chargeTime;
							if (chargeTime < minChargeTime)
								minChargeTime = chargeTime;
							if (chargeTime > maxChargeTime)
								maxChargeTime = chargeTime;
						}
						//快速充电时间
						if (dictCarParams.ContainsKey(878))
						{
							var fastChargeTime = ConvertHelper.GetInteger(dictCarParams[878]);
							if (minFastChargeTime == 0 && fastChargeTime > 0)
								minFastChargeTime = fastChargeTime;
							if (fastChargeTime < minFastChargeTime)
								minFastChargeTime = fastChargeTime;
							if (fastChargeTime > maxFastChargeTime)
								maxFastChargeTime = fastChargeTime;
						}
						//纯电最高续航里程
						if (dictCarParams.ContainsKey(883))
						{
							var mileage = ConvertHelper.GetInteger(dictCarParams[883]);
							if (minMileage == 0 && mileage > 0)
								minMileage = mileage;
							if (mileage < minMileage)
								minMileage = mileage;
							if (mileage > maxMileage)
								maxMileage = mileage;
						}
					}
					// 节能补贴 Sep.2.2010
					string hasEnergySubsidy = "";
					//补贴功能临时去掉 modified by chengl Oct.24.2013
                    if ((dictCarParams.ContainsKey(963) && (dictCarParams[963] == "第10批")) && dictCarParams.ContainsKey(853) && dictCarParams[853] == "3000元")
                    {
                        hasEnergySubsidy = " <a href=\"http://news.bitauto.com/zcxwtt/20130924/1406235633.html\" class=\"color-block2\" title=\"可获得3000元节能补贴\" target=\"_blank\">补贴</a>";
                    }
                    //============2016-02-26 减税 购置税============================
                    string strTravelTax = "";
                    double dEx = 0.0;
                    Double.TryParse(entity.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
                    if (entity.SaleState == "在销")
                    {
                        if (dictCarParams.ContainsKey(987) && (dictCarParams[987] == "第1批" || dictCarParams[987] == "第2批" || dictCarParams[987] == "第3批" || dictCarParams[987] == "第4批" || dictCarParams[987] == "第5批" || dictCarParams[987] == "第6批") && dictCarParams.ContainsKey(986))
                        {
                            strTravelTax = " <a target=\"_blank\" title=\"{0}\" href=\"http://news.bitauto.com/sum/20170105/1406774416.html\" class=\"color-block2\">{1}</a>";
                            if (dictCarParams[986].ToString() == "减半")
                            {
                                strTravelTax = string.Format(strTravelTax, "购置税减半", "减税");
                            }
                            else if (dictCarParams[986].ToString() == "免征")
                            {
                                strTravelTax = string.Format(strTravelTax, "免征购置税", "免税");
                            }
                        }
                        //else if (dEx > 0 && dEx <= 1.6)
                        //{
                        //    strTravelTax = " <a target=\"_blank\" title=\"购置税75折\" href=\"http://news.bitauto.com/sum/20170105/1406774416.html\" class=\"color-block2\">减税</a>";
                        //}
                    }
                    //carListHtml.Add(string.Format("<tr id=\"car_filter_id_{0}\" class=\"{1}\">", entity.CarID, trFlag % 2 == 1 ? "" : "hover-bg-color"));
                    carListHtml.Add(string.Format("<tr id=\"car_filter_id_{0}\">", entity.CarID));
                    carListHtml.Add(string.Format("<td class=\"txt-left\" id=\"carlist_{1}\"><a href=\"/{0}/m{1}/\" target=\"_blank\">{2} {3}</a> {4}</td>",
						serialSpell, entity.CarID, yearType, entity.CarName, strTravelTax + hasEnergySubsidy + stopPrd));
					//计算百分比
                    carListHtml.Add("<td><div class=\"w\">");
					int percent = (int)Math.Round((double)entity.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero);
					carListHtml.Add(string.Format("<div class=\"p\" style=\"width: {0}%\"></div>", percent));
                    carListHtml.Add("</div></td>");
                    // 档位个数
                    //string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" && dictCarParams[724] != "待查") ? dictCarParams[724] + "挡" : "";
                    string transmissionType = _carBLL.GetCarTransmissionType(dictCarParams.ContainsKey(724) ? dictCarParams[724] : string.Empty, entity.TransmissionType);
                    carListHtml.Add(string.Format("<td>{0}</td>", transmissionType));
					carListHtml.Add(string.Format("<td class=\"txt-right\"><span>{0}</span><a title=\"购车费用计算\" class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid={1}\" target=\"_blank\"></a></td>", string.IsNullOrEmpty(entity.ReferPrice) ? "暂无" : entity.ReferPrice + "万", entity.CarID));
					if (!isNoSaleYear)
					{
						if (entity.CarPriceRange.Trim().Length == 0)
                            carListHtml.Add(string.Format("<td class=\"txt-right\"><span>{0}</span></td>", "暂无报价"));
						else
						{
							//取最低报价
							string minPrice = entity.CarPriceRange;
							if (entity.CarPriceRange.IndexOf("-") != -1)
								minPrice = entity.CarPriceRange.Substring(0, entity.CarPriceRange.IndexOf('-'));

                            carListHtml.Add(string.Format("<td class=\"txt-right\"><span><a href=\"/{0}/m{1}/baojia/\" target=\"_blank\">{2}</a></span></td>", serialSpell, entity.CarID, minPrice));
						}
					}
					else
					{
						// 停销年款用二手车报价
						if (dicUcarPrice != null && dicUcarPrice.Count > 0 && dicUcarPrice.ContainsKey(entity.CarID))
						{
                            carListHtml.Add(string.Format("<td class=\"txt-right\"><span><a target=\"_blank\" href=\"http://www.taoche.com/all/?carid={0}&ref=pc_yc_ckzs_tsck_esc\">{1}</a></span></td>", entity.CarID, dicUcarPrice[entity.CarID]));
						}
						else
						{
                            carListHtml.Add("<td class=\"txt-right\">暂无报价</td>");
						}
					}
                    carListHtml.Add("<td class=\"txt-right\">");
					if (!isNoSaleYear)
					{
                        carListHtml.Add(string.Format("<a class=\"btn btn-primary btn-xs\" href=\"http://dealer.bitauto.com/zuidijia/nb{0}/nc{1}/?T=2\" target=\"_blank\">询底价</a>", serialId, entity.CarID));
					}
					else
					{
                        carListHtml.Add(string.Format("<a class=\"btn btn-primary btn-xs\" href=\"http://www.taoche.com/all/?carid={0}&ref=pc_yc_ckzs_tsck_esc\" target=\"_blank\">二手车</a>", entity.CarID));
					}
                    carListHtml.Add(string.Format(" <a class=\"btn btn-secondary btn-xs\" target=\"_self\" href=\"javascript:;\" data-use=\"compare\" data-id=\"{0}\">+ 对比</a>", entity.CarID));
                    carListHtml.Add("</td>");
					carListHtml.Add("</tr>");
				}
			}
			//add by 2014.05.04 电动车 参数
			if (maxChargeTime > 0)
			{
				chargeTimeRange = minChargeTime == maxChargeTime ? string.Format("{0}分钟", minChargeTime) : string.Format("{0}-{1}分钟", minChargeTime, maxChargeTime);
			}
			if (maxFastChargeTime > 0)
			{
				fastChargeTimeRange = minFastChargeTime == maxFastChargeTime ? string.Format("{0}分钟", minFastChargeTime) : string.Format("{0}-{1}分钟", minFastChargeTime, maxFastChargeTime);
			}
			if (maxMileage > 0)
			{
				mileageRange = minMileage == maxMileage ? string.Format("{0}公里", minMileage) : string.Format("{0}-{1}公里", minMileage, maxMileage);
			}
			return string.Concat(carListHtml.ToArray());
		}
		#endregion

		private void MakeSerialFocusV2()
		{
			XmlDocument doc = _serialBLL.GetSerialYearPhoto(serialId, carYear, 4);
			if (doc != null && doc.HasChildNodes)
			{
				XmlNodeList nodeList = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
				if (nodeList != null && nodeList.Count > 0)
				{
					foreach (XmlNode xn in nodeList)
					{
						CsPicJiaodian = "<a href=\"" + xn.Attributes["Link"].Value.ToString() + "\" target=\"_blank\"><img alt=\"" + serialShowName + xn.Attributes["ImageName"].Value.ToString() + "\" src=\"" + string.Format(xn.Attributes["ImageUrl"].Value,4) + "\" width=\"300\" height=\"200\"></a>";
						break;
					}
				}
				else
				{
                    CsPicJiaodian = "<a href=\"###\" target=\"_blank\"><img src=\"" + WebConfig.DefaultCarPic + "\" width=\"300\" height=\"200\"></a>";   //如果没有图片链接，则取焦点颜色块处的实拍图跳转的链接,在前端替换掉###
				}
				return;
			}
			// 如果没有年款图 则取子品牌焦点图
			doc = _serialBLL.GetSerialFocusImageForNew(serialId);

			if (doc != null && doc.HasChildNodes)
			{
				XmlNodeList nodeList = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
				if (nodeList != null && nodeList.Count > 0)
				{
					foreach (XmlNode xn in nodeList)
					{
						//int imgID = int.Parse(xn.Attributes["ImageId"].Value.ToString()) % 4 + 1;
						CsPicJiaodian = "<a href=\"" + xn.Attributes["Link"].Value.ToString() + "\" target=\"_blank\"><img alt=\"" + serialShowName + xn.Attributes["ImageName"].Value.ToString() + "\" src=\"" + string.Format(xn.Attributes["ImageUrl"].Value.ToString(), "4") + "\" width=\"300\" height=\"200\"></a>";
						break;
					}
				}
				else
				{
                    CsPicJiaodian = "<a href=\"###\" target=\"_blank\"><img src=\"" + WebConfig.DefaultCarPic + "\" width=\"300\" height=\"200\"></a>";//如果没有图片链接，则取焦点颜色块处的实拍图跳转的链接,在前端替换掉###
				}
				return;
			}
		}

		/// <summary>
		/// 生成颜色Hmtl
		/// </summary>
		private void MakeSerialYearColorHtml()
		{
			StringBuilder sb = new StringBuilder();
            StringBuilder sbDisplayColorHtml = new StringBuilder();
			var colorList = _serialBLL.GetColorRGBBySerialId(sic.CsID, carYear, sic.ColorList);
			if (colorList.Count > 0)
			{
				var maxColorNum = 26;
				foreach (var colorEntity in colorList.Take(maxColorNum))
				{
					if (string.IsNullOrEmpty(colorEntity.ColorLink))
                        sb.AppendFormat("<li><span style=\"background: {0}\" title=\"{1}\"></span></li>", colorEntity.ColorRGB, colorEntity.ColorName);
					else
					{
						sb.Append("<li>");
                        sb.AppendFormat("<a href=\"{1}\" target=\"_blank\"><span style=\"background: {0}\"></span></a>", colorEntity.ColorRGB, colorEntity.ColorLink);
                        sb.Append("</li>");
					}
                    sbDisplayColorHtml.AppendFormat("<img src=\"{0}\" width=\"300\" height=\"200\" style=\"display:none\">", colorEntity.ColorImageUrl.Replace("_5.", "_4."));
				}
			
				serialColorHtml = sb.ToString();
                serialColorDisplayHtml = sbDisplayColorHtml.ToString();
			}
		}

		
		/// <summary>
		/// 生成图片部分
		/// </summary>
		private void MakeSerialImages()
		{
			// modified by changl Apr.8.2011 先取年款的默认车型 取不到再取子品牌的图片
			Dictionary<string, string> dicAllsy = new Car_SerialBll().GetSerialYearDefaultCarDictionary();
			if (dicAllsy.ContainsKey(sic.CsID.ToString() + "_" + carYear.ToString()))
			{
				int carid = 0;
				string defaultCarid = dicAllsy[sic.CsID.ToString() + "_" + carYear.ToString()];
				if (int.TryParse(defaultCarid, out carid))
				{
					if (carid > 0)
					{
						StringBuilder PhotoData = new StringBuilder();
						// 存在年款的默认车型
						XmlDocument carPhoto = new Car_BasicBll().GetCarSummaryPhoto(sic.CsID, carid, 1);
						if (carPhoto != null && carPhoto.HasChildNodes)
						{
							string moreLink = string.Empty;
							int picCount = 0;
							RenderPicMoreLinkByCategory(carPhoto, out moreLink, out picCount);
							if (picCount <= 0)
							{ return; }

							string allPicUrl = "http://car.bitauto.com/" + cse.Cs_AllSpell.Trim() + "/m" + carid.ToString() + "/tupian/";
                            focusPhotoCount = picCount;
                            focusPhotoCountUrl = allPicUrl;
                            PhotoData.Append("<div class=\"section-header header2 mb0\">");
							PhotoData.Append("<div class=\"box\">");
							PhotoData.Append("<h2><a href=\"" + allPicUrl + "\">" + cse.Cs_ShowName.Trim() + "图片 </a></h2>");
							PhotoData.Append("<span class=\"header-note1\">共" + picCount + "张图片</span>");
                            PhotoData.Append("</div>");
							PhotoData.Append("<div class=\"more\">" + moreLink + " <a href=\"http://photo.bitauto.com/model/" + carid.ToString() + "/\" target=\"_blank\">更多&gt;&gt;</a></div>");
							PhotoData.Append("</div>");

							#region 子品牌颜色图片
							//// 子品牌颜色图片
							//Dictionary<string, XmlNode> dicColor = new Car_SerialBll().GetSerialColorPhotoByCsID(serialId, carYear);
							//if (dicColor != null && dicColor.Count > 0)
							//{
							//    PhotoData.AppendLine("<h4>车身颜色<span class=\"a\">(" + dicColor.Count.ToString() + "张)</span></h4>");
							//    PhotoData.AppendLine("<div class=\"carColor\">");
							//    PhotoData.AppendLine("<b id=\"LeftArr\" class=\"lGray\" >左</b>");
							//    PhotoData.AppendLine("<b id=\"RightArr\" class=\"r\" >右</b>");
							//    PhotoData.AppendLine("<div class=\"carColor_inner\">");
							//    PhotoData.AppendLine("<div id=\"innerBox\" style=\"top:0; left:0\">");
							//    PhotoData.AppendLine("<ul id=\"colorBox\">");
							//    foreach (KeyValuePair<string, XmlNode> keyColor in dicColor)
							//    {
							//        if (keyColor.Value.Attributes["ImageUrl"] != null && keyColor.Value.Attributes["ImageUrl"].Value.Trim() != "" && keyColor.Value.Attributes["Link"] != null && keyColor.Value.Attributes["Link"].Value.Trim() != "")
							//        {
							//            PhotoData.AppendLine("<li>");
							//            PhotoData.AppendLine("<div>");
							//            PhotoData.AppendLine("<a target=\"_blank\" href=\"" + keyColor.Value.Attributes["Link"].Value.Trim() + "\">");
							//            PhotoData.AppendLine("<img src=\"" + keyColor.Value.Attributes["ImageUrl"].Value.Trim() + "\" alt=\"\" />");
							//            PhotoData.AppendLine("</a></div>");
							//            PhotoData.AppendLine("<a target=\"_blank\" href=\"" + keyColor.Value.Attributes["Link"].Value.Trim() + "\">" + keyColor.Key.Trim() + "</a>");
							//            PhotoData.AppendLine("</li>");
							//        }
							//    }
							//    PhotoData.AppendLine("</ul>");
							//    PhotoData.AppendLine("</div></div></div>");
							//    PhotoData.AppendLine("<div class=\"line\"></div>");
							//    PhotoData.AppendLine("<div class=\"clear\"></div>");
							//}
							#endregion

							bool isHasContent = false;
                            PhotoData.AppendLine("<div class=\"list-box\">");
							// 外观
							RenderPicByCategory(ref PhotoData, carPhoto, 6, ref isHasContent);
							// 内饰
							RenderPicByCategory(ref PhotoData, carPhoto, 7, ref isHasContent);
							// 空间
							RenderPicByCategory(ref PhotoData, carPhoto, 8, ref isHasContent);
							// 图解
							RenderPicByCategory(ref PhotoData, carPhoto, 12, ref isHasContent);
							PhotoData.AppendLine("</div>");
							if (isHasContent)
							{
								//PhotoData.AppendLine("<div class=\"clear\"></div>");
							}
							else
							{
								// 当没有外观 内饰 空间 图解时 显示其他所有分类图片
								RenderPicAllCategory(ref PhotoData, carPhoto);
							}
						}
						serialImageHtml = PhotoData.ToString();
					}
				}
			}
			else
			{
				#region 年款没有默认车型 取子品牌图片
				// 年款没有默认车型 取子品牌图片
				StringBuilder htmlCode = new StringBuilder();
				//获取数据
				// 焦点图&中部图库组图(外观，内饰，空间，图解)
				//图库接口本地化更改 by sk 2012.12.21
				string xmlPicPath = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialPhotoListPath, serialId));
				//string xmlPicPath = string.Format(WebConfig.PhotoService, serialId.ToString());
				// 此 Cache 将通用于图片页和车型综述页
				DataSet dsCsPic = this.GetXMLDocToDataSetByURLForCache("CarChannel_SerialAllPic_" + serialId.ToString(), xmlPicPath, 60);
				if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("A"))
				{
					// 外观 6、图解 12、官方图 11、到店实拍 0 更多link
					string moreCateLink = "";
					//总数
					int picNum = 0;
					int cateId = 0;		//分类ID
					Dictionary<int, int> categoryPicNum = new Dictionary<int, int>();
					if (dsCsPic.Tables.Contains("A"))
					{
						foreach (DataRow row in dsCsPic.Tables["A"].Rows)
						{
							int cateNum = Convert.ToInt32(row["N"]);
							picNum += cateNum;
							cateId = Convert.ToInt32(row["G"]);
							// 分类更多link
							if (cateId == 6)
							{
								moreCateLink += "<a target=\"_blank\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/6/\">外观</a>";
							}
							else if (cateId == 12)
							{
								moreCateLink += "<a target=\"_blank\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/12/\">图解</a>";
							}
							else if (cateId == 11)
							{
								moreCateLink += "<a target=\"_blank\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/11/\">官方图</a>";
							}
							else if (cateId == 0)
							{
								moreCateLink += "<a target=\"_blank\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/0/\">到店实拍</a>";
							}
							else
							{ }
							if (cateId != 6 && cateId != 7 && cateId != 8 && cateId != 12)
								continue;
							categoryPicNum[cateId] = cateNum;

						}
					}
					string allPicUrl = baseUrl + "tupian/";
                    focusPhotoCount = picNum;
                    focusPhotoCountUrl = allPicUrl;

                    htmlCode.Append("<div class=\"section-header header2 mb0\">");
					htmlCode.Append("<div class=\"box\">");
					htmlCode.Append("<h2><a href=\"" + allPicUrl + "\">" + serialShowName + "图片 </a></h2>");
                    htmlCode.Append("<span class=\"header-note1\">共" + picNum + "张图片</span>");
                    htmlCode.Append("</div>");
					htmlCode.Append("<div class=\"more\">" + moreCateLink + "<a href=\"http://photo.bitauto.com/serial/" + serialId.ToString() + "/\" target=\"_blank\">更多&gt;&gt;</a></div>");
					htmlCode.Append("</div>");

					//htmlCode.Append("<h3><span><a href=\"" + allPicUrl + "\" target=\"_blank\">" + serialShowName + "-图片 </a></span>");
					//htmlCode.AppendLine("<label class=\"h3sublink\"><a href=\"" + allPicUrl + "\">" + picNum + "张</a></label></h3>");

					//htmlCode.Append("<div class=\"clear\"></div>");
					//htmlCode.AppendLine("<div class=\"more pic13_more\">" + moreCateLink + "<a href=\"http://photo.bitauto.com/serial/" + serialId.ToString() + "/\" target=\"_blank\">更多&gt;&gt;</a></div>");
					//htmlCode.Append("<div class=\"clear\"></div>");
					#region 子品牌颜色图片
					//// 子品牌颜色图片
					//Dictionary<string, XmlNode> dicColor = new Car_SerialBll().GetSerialColorPhotoByCsID(serialId, carYear);
					//if (dicColor != null && dicColor.Count > 0)
					//{
					//    htmlCode.AppendLine("<h4>车身颜色<span class=\"a\">(" + dicColor.Count.ToString() + "张)</span></h4>");
					//    htmlCode.AppendLine("<div class=\"carColor\">");
					//    htmlCode.AppendLine("<b id=\"LeftArr\" class=\"lGray\" >左</b>");
					//    htmlCode.AppendLine("<b id=\"RightArr\" class=\"r\" >右</b>");
					//    htmlCode.AppendLine("<div class=\"carColor_inner\">");
					//    htmlCode.AppendLine("<div id=\"innerBox\" style=\"top:0; left:0\">");
					//    htmlCode.AppendLine("<ul id=\"colorBox\">");
					//    foreach (KeyValuePair<string, XmlNode> keyColor in dicColor)
					//    {
					//        if (keyColor.Value.Attributes["ImageUrl"] != null && keyColor.Value.Attributes["ImageUrl"].Value.Trim() != "" && keyColor.Value.Attributes["Link"] != null && keyColor.Value.Attributes["Link"].Value.Trim() != "")
					//        {
					//            htmlCode.AppendLine("<li>");
					//            htmlCode.AppendLine("<div>");
					//            htmlCode.AppendLine("<a target=\"_blank\" href=\"" + keyColor.Value.Attributes["Link"].Value.Trim() + "\">");
					//            htmlCode.AppendLine("<img src=\"" + keyColor.Value.Attributes["ImageUrl"].Value.Trim() + "\" alt=\"\" />");
					//            htmlCode.AppendLine("</a></div>");
					//            htmlCode.AppendLine("<a target=\"_blank\" href=\"" + keyColor.Value.Attributes["Link"].Value.Trim() + "\">" + keyColor.Key.Trim() + "</a>");
					//            htmlCode.AppendLine("</li>");
					//        }
					//    }
					//    htmlCode.AppendLine("</ul>");
					//    htmlCode.AppendLine("</div></div></div>");
					//    htmlCode.AppendLine("<div class=\"line\"></div>");
					//    htmlCode.AppendLine("<div class=\"clear\"></div>");
					//}
					#endregion

					int count = 0;		//有图片类型计数
                    htmlCode.AppendLine("<div class=\"list-box\">");
					if (categoryPicNum.Count > 0 && dsCsPic.Tables.Contains("C") && dsCsPic.Tables["C"].Rows.Count > 0)
					{
						RenderPicByCategroy(htmlCode, 6, "外观", categoryPicNum, dsCsPic.Tables["C"], ref count);
						RenderPicByCategroy(htmlCode, 7, "内饰", categoryPicNum, dsCsPic.Tables["C"], ref count);
						RenderPicByCategroy(htmlCode, 8, "空间", categoryPicNum, dsCsPic.Tables["C"], ref count);
						RenderPicByCategroy(htmlCode, 12, "图解", categoryPicNum, dsCsPic.Tables["C"], ref count);
					}
					else
					{

						//显示其他图
						if (dsCsPic.Tables.Contains("A"))
						{
							foreach (DataRow row in dsCsPic.Tables["A"].Rows)
							{
								int cateNum = Convert.ToInt32(row["N"]);
								cateId = Convert.ToInt32(row["G"]);
								categoryPicNum[cateId] = cateNum;
							}

							foreach (DataRow row in dsCsPic.Tables["A"].Rows)
							{
								cateId = Convert.ToInt32(row["G"]);
								string cateName = Convert.ToString(row["D"]);
								picNum += categoryPicNum[cateId];
								RenderPicByCategroy(htmlCode, cateId, cateName, categoryPicNum, dsCsPic.Tables["C"], ref count);
							}
						}
					}
					htmlCode.AppendLine("</div>");
				}

				serialImageHtml = htmlCode.ToString();
				#endregion
			}


		}

		/// <summary>
		/// 生成每个分类的图片页面
		/// </summary>
		/// <param name="htmlCode"></param>
		/// <param name="cateId"></param>
		/// <param name="cateName"></param>
		/// <param name="picNum"></param>
		/// <param name="dt">图片数据</param>
		/// <param name="isLast">是否是最后一个</param>
		private void RenderPicByCategroy(StringBuilder htmlCode, int cateId, string cateName, Dictionary<int, int> picNumDic, DataTable dt, ref int count)
		{
			if (dt == null || dt.Rows.Count < 1)
				return;
			if (!picNumDic.ContainsKey(cateId))
				return;
			count++;
			int picNum = picNumDic[cateId];

			htmlCode.Append("<h6 class=\"title\"><a href=\"http://photo.bitauto.com/serialmore/" + serialId + "/" + cateId + "/\" target=\"_blank\">" + cateName + "&gt;&gt;</a><span class=\"header-note1\">" + picNum + "张图片</em></h6>");

            htmlCode.Append("<div class=\"row block-4col-180\">");
			int loop = 1;
			foreach (DataRow row in dt.Select("P='" + cateId + "'"))
			{
				int imgId = row["I"] == DBNull.Value ? 0 : Convert.ToInt32(row["I"]);
				string imgUrl = row["U"] == DBNull.Value ? "" : Convert.ToString(row["U"]);
				if (imgId == 0 || imgUrl.Length == 0)
					imgUrl = WebConfig.DefaultCarPic;
				else
					imgUrl = CommonFunction.GetPublishHashImgUrl(1, imgUrl, imgId);
				// imgUrl = new OldPageBase().GetPublishImage(1, imgUrl, imgId);

				string picName = Convert.ToString(row["D"]);
				string picUlr = "http://photo.bitauto.com/picture/" + serialId + "/" + imgId + "/";
                htmlCode.AppendLine("<div class=\"col-xs-3\">");
                htmlCode.AppendLine("<div class=\"img-info-layout-vertical img-info-layout-vertical-180120\">");
                htmlCode.AppendLine(string.Format("<div class=\"img\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" alt=\"{2}\"></a></div>",picUlr,imgUrl.Replace("_1.","_3."),serialShowName + picName));
                htmlCode.AppendLine(string.Format("<ul class=\"p-list\"><li class=\"name\"><a href=\"{0}\" target=\"_blank\">{1}</a></li></ul>", picUlr, picName));
                htmlCode.AppendLine("</div>");
                htmlCode.AppendLine("</div>");
                //htmlCode.AppendLine("<li><a href=\"" + picUlr + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" alt=\"" + serialShowName + picName + "\" width=\"165\" height=\"110\"></a><div class=\"title_pic\"><a href=\"" + picUlr + "\" target=\"_blank\">" + picName + "</a></div></li>");
				loop++;
				if (loop > 4)
				{ break; }
			}
			htmlCode.AppendLine("</div>");
			//if (count < picNumDic.Count)
			//    htmlCode.AppendLine("<div class=\"line\"></div>");
			//htmlCode.AppendLine("<div class=\"clear\"></div>  ");
			//if (count == picNumDic.Count)
			//    htmlCode.AppendLine("</div>");
			//htmlCode.AppendLine("<div class=\"clear\"></div>");
			//htmlCode.AppendLine("</div>");
		}

		

		/// <summary>
		/// 子品牌还关注
		/// </summary>
		/// <returns></returns>
		private void MakeSerialToSerialHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(serialId, 6);
			if (lsts.Count > 0)
			{
				int loop = 0;
				foreach (EnumCollection.SerialToSerial sts in lsts)
				{
					string csName = sts.ToCsShowName.ToString();
					string shortName = StringHelper.SubString(csName, 12, true);
					if (shortName.StartsWith(csName))
						shortName = csName;

					loop++;
					htmlCode.Append("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-14093\">");
					htmlCode.AppendFormat("<div class=\"img\"><a target=\"_blank\" href=\"/{0}/\"><img src=\"{1}\" alt=\"{2}\"></a></div>",
						sts.ToCsAllSpell.ToString().ToLower(),
						 sts.ToCsPic.ToString().Replace("_5","_3"),csName);
                    htmlCode.Append("<ul class=\"p-list\">");
					if (shortName != csName)
						htmlCode.AppendFormat("<li class=\"name no-wrap\"><a target=\"_blank\" href=\"/{0}/\" title=\"{1}\">{2}</a></li>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName, shortName);
					else
						htmlCode.AppendFormat("<li class=\"name no-wrap\"><a target=\"_blank\" href=\"/{0}/\">{1}</a></li>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName);
					htmlCode.AppendFormat("<li class=\"price\"><a href=\"{1}\">{0}</a></li>",sts.ToCsSaleState=="待销"?"未上市": (sts.ToCsPriceRange.ToString().Length>0?StringHelper.SubString(sts.ToCsPriceRange.ToString(), 14, false):"暂无指导价"),sts.ToCsAllSpell.ToString().ToLower());
					htmlCode.AppendFormat("</ul>");
                    htmlCode.AppendFormat("</div>");
				}
			}
			serialToSeeHtml = htmlCode.ToString();
		}

		/// <summary>
		/// 取子品牌对比排行数据
		/// </summary>
		/// <returns></returns>
		private void MakeHotSerialCompare()
		{
            //取对比车型的白底图
            Dictionary<int, string> dictDuibiPic = this.GetAllSerialPicURLWhiteBackground();

            List<EnumCollection.SerialHotCompareData> lstCompareData = this.GetSerialHotCompareByCsID(serialId, 6);
			List<string> htmlList = new List<string>();
			string compareBaseUrl = "/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?count=3&csids=" + serialId + ",";
			string serialCompareForPkUrl = "/duibi/" + serialId + "-";
            if (lstCompareData != null && lstCompareData.Count > 0 )
			{
				htmlList.Add("<div class=\"col2-140-box clearfix\">");
                for (int i = 0; i < lstCompareData.Count; i++)
				{
                    EnumCollection.SerialHotCompareData carSerial = lstCompareData[i];
					string imgUrl = dictDuibiPic.ContainsKey(carSerial.CompareCsID) ? dictDuibiPic[carSerial.CompareCsID].Replace("_2", "_3") : WebConfig.DefaultCarPic;
                    htmlList.Add("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-14093\">");
                    htmlList.Add(string.Format("<div class=\"img\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" alt=\"{2}\"></a></div>",
									serialCompareForPkUrl + carSerial.CompareCsID + "/", imgUrl, carSerial.CompareCsName.Trim()));
					htmlList.Add(string.Format("<ul class=\"p-list\"><li class=\"name no-wrap\"><a href=\"{1}\" target=\"_blank\"><span>VS  </span>{0}</a></li></ul>",
						        carSerial.CompareCsName.Trim(),serialCompareForPkUrl + carSerial.CompareCsID + "/"));
                    htmlList.Add("</div>");
				}
				htmlList.Add("</div>");
			}

			hotSerialCompareHtml = String.Concat(htmlList.ToArray());
		}

        /// <summary>
		/// 得到品牌下的其他子品牌
		/// </summary>
		/// <returns></returns>
		protected string GetBrandOtherSerial()
        {
            List<CarSerialPhotoEntity> carSerialPhotoList = new Car_BrandBll().GetCarSerialPhotoListByCBID(cse.Cb_Id, false);

            carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

            if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
            {
                return "";
            }

            int forLastCount = 0;
            foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
            {
                if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Cs_Id)
                {
                    continue;
                }
                forLastCount++;
            }

            StringBuilder contentBuilder = new StringBuilder(string.Empty);
            string serialUrl = "<a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}</a>";
            int index = 0;
            foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
            {
                bool IsExitsUrl = true;
                if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Cs_Id)
                {
                    continue;
                }
                string priceRang = base.GetSerialReferPriceByID(entity.SerialId);
                if (entity.SaleState == "待销")
                {
                    IsExitsUrl = false;
                    priceRang = "未上市";
                }
                else if (priceRang.Trim().Length == 0)
                {
                    IsExitsUrl = false;
                    priceRang = "暂无指导价";
                }
                if (IsExitsUrl)
                {
                    priceRang = string.Format("<a href='http://price.bitauto.com/brand.aspx?newbrandid={0}'>{1}</a>", entity.SerialId, priceRang);
                }
                string tempCsSeoName = string.IsNullOrEmpty(entity.CS_SeoName) ? entity.ShowName : entity.CS_SeoName;
                index++;
                contentBuilder.AppendFormat("<li><div class=\"txt\">{0}</div><span>{1}</span></li>"
                    , string.Format(serialUrl, entity.CS_AllSpell, tempCsSeoName), priceRang
                     );
            }

            StringBuilder brandOtherSerial = new StringBuilder(string.Empty);
            if (contentBuilder.Length > 0)
            {
                brandOtherSerial.AppendFormat("<h3 class=\"top-title\"><a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}其他车型</a></h3>",
                    cse.Cb_AllSpell, cse.Cb_Name);

                brandOtherSerial.Append("<div class=\"list-txt list-txt-s list-txt-default list-txt-style5\"><ul>");

                brandOtherSerial.Append(contentBuilder.ToString());

                brandOtherSerial.Append("</ul></div>");
            }

            return brandOtherSerial.ToString();
        }
        #region new车型图片

        /// <summary>
        /// 按特定分类取图片
        /// </summary>
        /// <param name="_sb"></param>
        /// <param name="_sbMore"></param>
        /// <param name="cateid"></param>
        private void RenderPicByCategory(ref StringBuilder _sb, XmlDocument doc, int cateid, ref bool isHasContent)
		{
			if (doc != null && doc.HasChildNodes)
			{
				XmlNodeList xnl = doc.SelectNodes("/ImageData/CarImageGroup/Group[@GroupId='" + cateid.ToString() + "']");
				if (xnl != null && xnl.Count > 0 && xnl[0].HasChildNodes)
				{
					//if (isHasContent)
					//{
					//    // 如果之前有分类图片 增加分割
					//    _sb.AppendLine("<div class=\"line\"></div>");
					//    _sb.AppendLine("<div class=\"clear\"></div>");
					//}

					string cateName = xnl[0].Attributes["GroupName"].Value.ToString();
					string picNum = xnl[0].Attributes["ImageCount"].Value.ToString();
					string cateURL = xnl[0].Attributes["Link"].Value.ToString();
					//_sb.AppendLine("<div class=\"blank_box\">");
					//_sb.AppendLine("<h2><a target=\"_blank\" href=\"" + cateURL + "\">" + cateName + "<span class=\"a\">(" + picNum + "张)</span></a></h2>");

					_sb.Append("<h6 class=\"title\"><a target=\"_blank\" href=\"" + cateURL + "\">" + cateName + "&gt;&gt;</a><span class=\"header-note1\">" + picNum + "张图片</span></h6>");

                    _sb.Append("<div class=\"row block-4col-180\">");
					foreach (XmlNode xn in xnl[0])
					{
						string picUlr = xn.Attributes["Link"].Value.ToString();
						string imgUrl = xn.Attributes["ImageUrl"].Value.ToString();
						string picName = xn.Attributes["ImageName"].Value.ToString();
                        _sb.AppendLine("<div class=\"col-xs-3\">");
                        _sb.AppendLine("<div class=\"img-info-layout-vertical img-info-layout-vertical-180120\">");
                        _sb.AppendLine(string.Format("<div class=\"img\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" alt=\"{2}\"></a></div>",picUlr,imgUrl.Replace("_1.","_3."),picName));
                        _sb.AppendLine(string.Format("<ul class=\"p-list\"><li class=\"name\"><a href=\"{0}\">{1}</a></li></ul>",picUlr,picName));
                        _sb.Append("</div>");
                        _sb.Append("</div>");
					}
					_sb.Append("</div>");
					//_sb.AppendLine("<div class=\"clear\"></div>");
					//_sb.AppendLine("<div class=\"more\"><a href=\"" + cateURL + "\" target=\"_blank\">更多照片&gt;&gt;</a></div>");
					//_sb.AppendLine("</div>");
					isHasContent = isHasContent || true;
				}
			}
		}

		/// <summary>
		/// 全部分类
		/// </summary>
		/// <param name="_sb"></param>
		/// <param name="doc"></param>
		private void RenderPicAllCategory(ref StringBuilder _sb, XmlDocument doc)
		{
			if (doc != null && doc.HasChildNodes)
			{
				XmlNodeList xnl = doc.SelectNodes("/ImageData/CarImageGroup/Group");
				if (xnl != null && xnl.Count > 0)
				{
					bool isHasContent = false;
					foreach (XmlNode xnCate in xnl)
					{
						//if (isHasContent)
						//{
						//    // 如果之前有分类图片 增加分割
						//    _sb.AppendLine("<div class=\"line\"></div>");
						//    _sb.AppendLine("<div class=\"clear\"></div>");
						//}

						string cateName = xnCate.Attributes["GroupName"].Value.ToString();
						string picNum = xnCate.Attributes["ImageCount"].Value.ToString();
						string cateURL = xnCate.Attributes["Link"].Value.ToString();
						//_sb.AppendLine("<h4><a target=\"_blank\" href=\"" + cateURL + "\">" + cateName + "<span class=\"a\">(" + picNum + "张)</span></a></h4>");

                        _sb.Append("<h6 class=\"title\"><a target=\"_blank\" href=\"" + cateURL + "\">" + cateName + "&gt;&gt;</a><em>" + picNum + "张</em></h6>");

                        _sb.Append("<div class=\"row block-4col-180\">");
						foreach (XmlNode xn in xnCate.ChildNodes)
						{
							string picUlr = xn.Attributes["Link"].Value.ToString();
							string imgUrl = xn.Attributes["ImageUrl"].Value.ToString();
							string picName = xn.Attributes["ImageName"].Value.ToString();
                            _sb.AppendLine(string.Format("<div class=\"img\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" alt=\"{2}\"></a></div>", picUlr, imgUrl, picName));
                            _sb.AppendLine(string.Format("<ul class=\"p-list\"><li class=\"name\"><a href=\"{0}\">{1}</a></li></ul>", picUlr, picName));
							//_sb.AppendLine("<li><a target=\"_blank\" href=\"" + picUlr + "\"><img src=\"" + imgUrl + "\" width=\"165\" height=\"110\"></a><div class=\"title_pic\"><a target=\"_blank\" href=\"" + picUlr + "\">" + picName + "</a></div></li>");
						}
						_sb.Append("</div>");
						isHasContent = isHasContent || true;
					}
					//_sb.AppendLine("<div class=\"clear\"></div>");
				}
			}
		}

		/// <summary>
		/// 取更多分类link 及全部图片张数
		/// </summary>
		/// <param name="_sb"></param>
		/// <param name="cateid"></param>
		private void RenderPicMoreLinkByCategory(XmlDocument doc, out string moreLink, out int picCount)
		{
			moreLink = "";
			picCount = 0;
			StringBuilder sbMoreLink = new StringBuilder();
			if (doc != null && doc.HasChildNodes)
			{
				XmlNodeList xnl = doc.SelectNodes("/ImageData/CarImageGroup/Group");
				if (xnl != null && xnl.Count > 0)
				{
					foreach (XmlNode xnCate in xnl)
					{
						string cateid = xnCate.Attributes["GroupId"].Value.ToString();

						// 外观 6、图解 12、官方图 11、到店实拍 0 更多link
						if (cateid == "6" || cateid == "12" || cateid == "11" || cateid == "0")
						{
                            //if (sbMoreLink.Length > 0)
                            //{
                            //    sbMoreLink.Append(" | ");
                            //}
							sbMoreLink.Append("<a target=\"_blank\" href=\"" + xnCate.Attributes["Link"].Value.ToString() + "\">" + xnCate.Attributes["GroupName"].Value.ToString() + "</a>");
						}

						int pic = 0;
						if (int.TryParse(xnCate.Attributes["ImageCount"].Value.ToString(), out pic))
						{
							if (pic > 0)
							{
								picCount += pic;
							}
						}
					}
				}
			}
			if (sbMoreLink.Length > 0)
			{ moreLink = sbMoreLink.ToString(); }
		}

		#endregion

		private void InitNextSee()
		{
			nextSeePingceHtml = String.Empty;
			nextSeeXinwenHtml = String.Empty;
			nextSeeDaogouHtml = String.Empty;
			string serialSpell = cse.Cs_AllSpell.Trim().ToLower();
			string serialShowName = cse.Cs_ShowName;
			CarNewsBll newsBll = new CarNewsBll();
			if (newsBll.IsSerialNews(serialId, 0, CarNewsType.pingce))
				nextSeePingceHtml = "<li><div class=\"txt\"><a href=\"/" + serialSpell + "/pingce/\" target=\"_self\">" + serialShowName + "车型详解</a></div></li>";
			//if (newsBll.IsSerialNews(serialId, 0, CarNewsType.xinwen))
			//    nextSeeXinwenHtml = "<li><a href=\"/" + serialSpell + "/xinwen/\" target=\"_self\">" + serialShowName + "<span>新闻</span></a></li>";
			if (newsBll.IsSerialNews(serialId, 0, CarNewsType.daogou))
                nextSeeDaogouHtml = "<li><div class=\"txt\"><a href=\"/" + serialSpell + "/daogou/\" target=\"_self\">" + serialShowName + "导购</a></div></li>";

		}
	}
}