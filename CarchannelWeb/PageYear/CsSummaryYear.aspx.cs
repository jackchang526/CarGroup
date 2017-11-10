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

namespace BitAuto.CarChannel.CarchannelWeb.PageYear
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
		protected string shijiaOrHuimaiche = string.Empty;//低价 惠买车
		protected string chedaiADLink = string.Empty;//贷款链接



		private bool isNoSaleYear = true;	// 当前年款是否是停销年款
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
			chedaiADLink = "http://www.daikuan.com/www/" + serialSpell + "/?from=yc9";
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
				shijiaOrHuimaiche = string.Format("<span class=\"button_gray btn-qt-w\" id=\"btnDijia\"><a href=\"{0}?tracker_u=11_yccx\" target=\"_blank\">买新车</a></span>", dicHuiMaiChe[serialId]);
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
			MakeKoubeiImpressionHtml();
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

			sb.Append("<div class=\"line-box\" id=\"car_list\" style=\"z-index:999;\">");

			sb.Append("<div class=\"title-con\">");
			sb.Append("<div class=\"title-box title-box2\">");
			sb.AppendFormat("<h4>{0}款{1}车款</h4>", carYear, cse.Cs_SeoName);
			sb.Append("</div>");
			sb.Append("</div>");


			//sb.Append("    <h3>");
			//sb.AppendFormat("        <span>{0}款{1}车款</span>", carYear, cse.Cs_SeoName);
			//sb.Append(" <em class=\"h3_spcar\">");
			//sb.Append("<a href=\"" + baseUrl + "#car_list\">全部在售</a>");

			//// modified by chengl Jun.24.2011
			//// 根据在销年款生成
			//saleYearList.Sort((x, y) => string.Compare(y, x));
			//for (int i = 0; i < saleYearList.Count; i++)
			//{
			//    string yearStr = saleYearList[i];
			//    if (yearStr == carYear + "款")
			//        continue;
			//    string url = baseUrl + yearStr.Replace("款", "") + "/";
			//    sb.Append("<s>|</s><a href=\"" + url + "#car_list\">" + yearStr + "</a>");
			//}
			//if (serialId != 1568 && base.CheckSerialHasNoSale(serialId) && noSaleYearList.Count > 0)
			//{
			//    if (saleYearList.Count > 0)
			//        sb.Append("<s>|</s>");
			//    //tableCode.Append("<a href=\"http://www.cheyisou.com/chexing/" + Server.UrlEncode(serialShowName) + "/1.html?para=os|0|en|utf8\" target=\"_blank\">停售车款</a>");
			//    sb.Append("<dl id=\"bt_car_spcar_table\"><dt>停售年款<em></em></dt><dd style=\"display:none;\">");
			//    for (int i = 0; i < noSaleYearList.Count; i++)
			//    {
			//        string url = baseUrl + noSaleYearList[i].Replace("款", "") + "/";
			//        if (i == noSaleYearList.Count - 1)
			//            sb.Append("<a href=\"" + url + "\" target=\"_self\" class=\"last_a\">" + noSaleYearList[i] + "</a>");
			//        else
			//        {
			//            sb.Append("<a href=\"" + url + "\" target=\"_self\">" + noSaleYearList[i] + "</a>");
			//        }
			//    }
			//    sb.Append("</dd></dl>");
			//}

			////if (noSaleYearList.Count > 0)
			////{
			////    //sb.Append("<ul id=\"car_nosaleyearlist\">");
			////    sb.Append("                <li id=\"car_nosaleyearlist\" class=\"last\">停售车款<em></em>");
			////    sb.Append("                    <dl style=\"display: none;\">");
			////    for (int i = 0; i < noSaleYearList.Count; i++)
			////    {
			////        string url = string.Format("/{0}/{1}/#car_list", serialSpell, noSaleYearList[i].Replace("款", ""));
			////        if (i == noSaleYearList.Count - 1)
			////            sb.AppendFormat("<dd class=\"last\"><a href=\"{0}\" target=\"_blank\">{1}</a></dd>", url, noSaleYearList[i]);
			////        else
			////            sb.AppendFormat("<dd><a href=\"{0}\" target=\"_blank\">{1}</a></dd>", url, noSaleYearList[i]);
			////    }
			////    sb.Append("</dl>");
			////    sb.Append("</li>");
			////    //sb.Append("</ul>");
			////}
			//sb.Append("</em>");
			//// modified by chengl Oct.15.2013
			//// sb.Append(" <span class=\"h_text\"><a target=\"_blank\" href=\"http://app.yiche.com/qichehui/\">下载易车客户端，体验信息最全的汽车应用！</a></span>");

			//sb.Append("    </h3>");
			if (currentYearCarList.Count > 0)
			{
				isNoSaleYear = currentYearCarList.Find(p => p.SaleState != "停销") != null ? false : true;

				//根据车型列表获取年款信息
				GetSerialYearInfoByCarList(currentYearCarList);

				sb.AppendFormat("    <div class=\"c-list-2014\" id=\"data_tab_jq5_{0}\" style=\"display: block;\">", 0);
				sb.Append("        <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" id=\"compare\">");
				sb.Append("            <tbody>");
				sb.Append(GetCarListHtml(currentYearCarList, maxPv));
				sb.Append("            </tbody>");
				sb.Append("        </table>");
				sb.Append("    </div>");


			}
			//sb.Append("    <div class=\"clear\"></div>");
			//sb.Append("    <div class=\"table_more\">");
			//sb.Append("<div class=\"button_gray button_65_20\"><a href=\"http://www.taoche.com/buycar/serial/" + serialSpell + "/?ref=car2\" target=\"_blank\" class=\"more2new\">买二手车</a></div>");
			//sb.Append("<div class=\"button_gray button_65_20\"><a id=\"LinkForBaaAttention\" href=\"http://i.bitauto.com/baaadmin/car/guanzhu_" + serialId + "/\" target=\"_blank\" class=\"more2new\">加关注</a></div>");
			//sb.AppendFormat("<div class=\"button_gray button_65_20\"><a class=\"more2new\" href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">预约试驾</a></div>", serialId);
			//sb.Append("    </div>");
			sb.Append("</div>");
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
			isElectrombile = fuelTypeList.Count == 1 && fuelTypeList[0] == "电力" ? true : false;
			//add by 2014.03.18 在销车款 排量输出
			var exhaustList = currentYearCarList.Where(p => p.Engine_Exhaust.EndsWith("L"))
				.Select(p => p.Engine_InhaleType == "增压" ? p.Engine_Exhaust.Replace("L", "T") : p.Engine_Exhaust)
											.GroupBy(p => p)
											.Select(group => group.Key).ToList();
			if (exhaustList.Count > 0)
			{
				exhaustList.Sort(NodeCompare.ExhaustCompareNew);
				if (exhaustList.Count > 3)
				{
					serialExhaust = string.Concat(exhaustList[0], " ", exhaustList[1]
						, "..."
						, exhaustList[exhaustList.Count - 1], fuelTypeList.Contains("电力") ? " 电动" : "");
				}
				else
					serialExhaust = string.Join(" ", exhaustList.ToArray()) + (fuelTypeList.Contains("电力") ? " 电动" : "");
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
			//if (carList.Count == 0)
			//    carListHtml.Add("<tr>暂无车款！</tr>");

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
					strMaxPowerAndInhaleType = string.Format("<b>/</b>{0}{1}", maxPower, " " + inhaleType);
				}

				if (groupIndex == 0)
				{
					carListHtml.Add(string.Format("<tr id=\"car_filter_gid_{0}\" class=\"titlebg\">", groupIndex));
					if (!isNoSaleYear)
					{
						carListHtml.Add("    <th width=\"44%\" class=\"first-item\">");
						carListHtml.Add(string.Format("<div class=\"pdL10\"><strong>{0}</strong> {1}</div>",
							key.Engine_Exhaust.Replace("L", "升"),
							strMaxPowerAndInhaleType));
						carListHtml.Add("    </th>");
						carListHtml.Add("    <th width=\"8%\" class=\"pd-left-one\">关注度</th>");
						carListHtml.Add("    <th width=\"10%\" class=\"pd-left-one\">变速箱</th>");
						carListHtml.Add("    <th width=\"10%\" class=\"pd-left-two\">指导价</th>");
						carListHtml.Add("    <th width=\"10%\" class=\"pd-left-three\">参考最低价</th>");
						carListHtml.Add("    <th width=\"18%\"><div class=\"wenh\" onmouseover=\"javascript:$(this).children('.tc-wenh').show();return false;\" onmouseout=\"javascript:$(this).children('.tc-wenh').hide();return false;\"><div class=\"tc tc-wenh\" style=\"display:none;\">");
						carListHtml.Add("    <div class=\"tc-box\"><i></i><p>全国参考最低价</p></div></div></div></th>");
					}
					else
					{
						carListHtml.Add("    <th width=\"35%\" class=\"first-item\">");
						carListHtml.Add(string.Format("<div class=\"pdL10\"><strong>{0}</strong> {1}</div>",
							key.Engine_Exhaust.Replace("L", "升"),
							strMaxPowerAndInhaleType));
						carListHtml.Add("    </th>");
						carListHtml.Add("    <th width=\"10%\" class=\"pd-left-one\">关注度</th>");
						carListHtml.Add("    <th width=\"10%\" class=\"pd-left-one\">变速箱</th>");
						carListHtml.Add("    <th width=\"10%\" width=\"10%\" class=\"pd-left-two\">指导价</th>");
						carListHtml.Add("    <th width=\"17%\" class=\"pd-left-three\">二手车报价</th>");
						carListHtml.Add("    <th width=\"18%\">&nbsp;</th>");
					}
					carListHtml.Add("</tr>");
				}
				else
				{
					carListHtml.Add(string.Format("<tr id=\"car_filter_gid_{0}\" class=\"titlebg\">", groupIndex));
					if (!isNoSaleYear)
					{
						carListHtml.Add("    <th width=\"44%\" class=\"first-item\">");
						carListHtml.Add(string.Format("        <div class=\"pdL10\"><strong>{0}</strong> {1}</div>",
							key.Engine_Exhaust.Replace("L", "升"),
							strMaxPowerAndInhaleType));
						carListHtml.Add("    </th>");
						carListHtml.Add("    <th width=\"8%\" class=\"pd-left-one\"></th>");
						carListHtml.Add("    <th width=\"10%\" class=\"pd-left-one\"></th>");
						carListHtml.Add("    <th width=\"10%\" class=\"pd-left-two\"></th>");
						carListHtml.Add("    <th width=\"10%\" class=\"pd-left-three\"></th>");
						carListHtml.Add("    <th width=\"18%\">&nbsp;</th>");
					}
					else
					{
						carListHtml.Add("    <th width=\"35%\" class=\"first-item\">");
						carListHtml.Add(string.Format("<div class=\"pdL10\"><strong>{0}</strong> {1}</div>",
							key.Engine_Exhaust.Replace("L", "升"),
							strMaxPowerAndInhaleType));
						carListHtml.Add("    </th>");
						carListHtml.Add("    <th width=\"10%\" class=\"pd-left-one\"></th>");
						carListHtml.Add("    <th width=\"10%\" class=\"pd-left-one\"></th>");
						carListHtml.Add("    <th width=\"100px\" width=\"10%\" class=\"pd-left-two\"></th>");
						carListHtml.Add("    <th width=\"17%\" class=\"pd-left-three\"></th>");
						carListHtml.Add("    <th width=\"18%\">&nbsp;</th>");
					}
					carListHtml.Add("</tr>");
				}
				groupIndex++;
				List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList<CarInfoForSerialSummaryEntity>();//分组后的集合

				foreach (CarInfoForSerialSummaryEntity entity in carGroupList)
				{
					string yearType = entity.CarYear.Trim();
					if (yearType.Length > 0)
						yearType += "款";
					else
						yearType = "未知年款";
					string stopPrd = "";
					if (entity.ProduceState == "停产")
						stopPrd = " <span class=\"tingchan\">停产</span>";
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
					//if (dictCarParams.ContainsKey(853) && dictCarParams[853] == "3000元")
					//{
					//    hasEnergySubsidy = " <a href=\"http://news.bitauto.com/zcxwtt/20130924/1406235633.html\" class=\"butie\" title=\"可获得3000元节能补贴\" target=\"_blank\">补贴</a>";
					//}
                    //============2016-02-26 减税 购置税============================
                    string strTravelTax = "";
                    double dEx = 0.0;
                    Double.TryParse(entity.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
                    if (entity.SaleState == "在销")
                    {
                        if (dictCarParams.ContainsKey(987) && (dictCarParams[987] == "第1批" || dictCarParams[987] == "第2批" || dictCarParams[987] == "第3批" || dictCarParams[987] == "第4批" || dictCarParams[987] == "第5批" || dictCarParams[987] == "第6批") && dictCarParams.ContainsKey(986))
                        {
                            strTravelTax = " <a target=\"_blank\" title=\"{0}\" href=\"http://news.bitauto.com/sum/20160331/0206583470.html\" class=\"hundong\">减税</a>";
                            if (dictCarParams[986].ToString() == "减半")
                            {
                                strTravelTax = string.Format(strTravelTax, "购置税减半");
                            }
                            else if (dictCarParams[986].ToString() == "免征")
                            {
                                strTravelTax = string.Format(strTravelTax, "免征购置税");
                            }
                        }
                        else if (dEx > 0 && dEx <= 1.6)
                        {
                            strTravelTax = " <a target=\"_blank\" title=\"购置税减半\" href=\"http://news.bitauto.com/sum/20160331/0206583470.html\" class=\"hundong\">减税</a>";
                        }
                    }
					////易车惠
					//string strYiCheHui = "";
					//var carGoods = serialGoodsCarList.Find(p => p.CarId == entity.CarID);
					//if (carGoods != null)
					//{
					//    var goodsUrl = carGoods.GoodsUrl.Replace("/detail", "/all/detail") + "?WT.mc_id=car2";
					//    strYiCheHui = string.Format("<a target=\"_blank\" title=\"{0}\" href=\"{1}\" class=\"ad-yichehui-list\">易车惠特价&gt;&gt;</a>", "", goodsUrl);
					//}
					//string strBest = "<a href=\"#\" class=\"ico-tuijian\">推荐</a>";
					carListHtml.Add(string.Format("<tr id=\"car_filter_id_{0}\">", entity.CarID));
					carListHtml.Add("<td>");
					carListHtml.Add(string.Format("    <div class=\"pdL10\" id=\"carlist_{1}\"><a href=\"/{0}/m{1}/\" target=\"_blank\">{2} {3}</a> {4}</div>",
						serialSpell, entity.CarID, yearType, entity.CarName, strTravelTax + hasEnergySubsidy + stopPrd));
					carListHtml.Add("</td>");
					carListHtml.Add("<td>");
					carListHtml.Add("    <div class=\"w\">");
					//计算百分比
					int percent = (int)Math.Round((double)entity.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero);

					carListHtml.Add(string.Format("        <div class=\"p\" style=\"width: {0}%\"></div>", percent));
					carListHtml.Add("    </div>");
					carListHtml.Add("</td>");
					// 档位个数
					string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" && dictCarParams[724] != "待查") ? dictCarParams[724] + "挡" : "";

					carListHtml.Add(string.Format("<td>{0}</td>", forwardGearNum + entity.TransmissionType));
					carListHtml.Add(string.Format("<td style=\"text-align: right\"><span>{0}</span><a title=\"购车费用计算\" class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid={1}\" target=\"_blank\"></a></td>", string.IsNullOrEmpty(entity.ReferPrice) ? "暂无" : entity.ReferPrice + "万", entity.CarID));
					if (!isNoSaleYear)
					{
						if (entity.CarPriceRange.Trim().Length == 0)
							carListHtml.Add(string.Format("    <td style=\"text-align: right\"><span>{0}</span></td>", "暂无报价"));
						else
						{
							//取最低报价
							string minPrice = entity.CarPriceRange;
							if (entity.CarPriceRange.IndexOf("-") != -1)
								minPrice = entity.CarPriceRange.Substring(0, entity.CarPriceRange.IndexOf('-'));

							carListHtml.Add(string.Format("<td style=\"text-align: right\"><span><a href=\"/{0}/m{1}/baojia/\" target=\"_blank\">{2}</a></span></td>", serialSpell, entity.CarID, minPrice));
						}
					}
					else
					{
						// 停销年款用二手车报价
						if (dicUcarPrice != null && dicUcarPrice.Count > 0 && dicUcarPrice.ContainsKey(entity.CarID))
						{
							carListHtml.Add(string.Format("<td style=\"text-align:right\"><span><a target=\"_blank\" href=\"http://www.taoche.com/all/?carid={0}&ref=car3\">{1}</a></span></td>", entity.CarID, dicUcarPrice[entity.CarID]));
						}
						else
						{
							carListHtml.Add("<td style=\"text-align:right\">暂无报价</td>");
						}
					}
					carListHtml.Add("<td>");
					if (!isNoSaleYear)
					{
						carListHtml.Add(string.Format("<div class=\"car-summary-btn-xunjia button_gray\"><a href=\"http://dealer.bitauto.com/zuidijia/nb{0}/nc{1}/?T=2\" target=\"_blank\">询价</a></div>", serialId, entity.CarID));
					}
					else
					{
						carListHtml.Add(string.Format("<div class=\"car-summary-btn-xunjia button_gray\"><a href=\"http://www.taoche.com/all/?carid={0}&ref=car3\" target=\"_blank\">二手车</a></div>", entity.CarID));
					}
					carListHtml.Add(string.Format("<div class=\"car-summary-btn-duibi button_gray\" id=\"carcompare_btn_new_{0}\"><a target=\"_self\" href=\"javascript:;\" cid=\"{0}\"><span>对比</span></a></div>", entity.CarID));
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
		/// <summary>
		/// 生成在销的车款列表
		/// </summary>
		private void MakeSerialList()
		{
			StringBuilder tableCode = new StringBuilder();
			//获取数据
			List<EnumCollection.CarInfoForSerialSummary> ls = base.GetAllCarInfoForSerialSummaryByCsID(serialId, true);
			ls.Sort(NodeCompare.CompareCarByExhaust);
			List<string> exhaustList = new List<string>();
			List<string> yearList = new List<string>();
			// modified by chengl Jun.24.2011
			// 在销年款
			List<string> yearSaleList = new List<string>();
			//停销年款
			List<string> noSaleYearList = new List<string>();
			Dictionary<string, string> yearHtmlDic = new Dictionary<string, string>();
			int maxPv = 0;
			double maxPrice = Double.MinValue;
			double minPrice = Double.MaxValue;
			double maxDealerPrice = Double.MinValue;
			double minDealerPrice = Double.MaxValue;
			//double maxNetFuel = Double.MinValue;
			//double minNetFuel = Double.MaxValue;
			foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
			{
				// modified by chengl Oct.12.2011
				// 判断当前是否是停销年款
				if (carInfo.CarYear == carYear.ToString())
				{
					// 当前年款有1个非停销车 则此年款不是停销年款
					if (carInfo.SaleState != "停销")
					{ isNoSaleYear = false; }
				}

				if (carInfo.CarPV > maxPv)
					maxPv = carInfo.CarPV;
				// modified by chengl Jun.24.2011
				// 显示停销年款
				if (carInfo.CarYear.Length > 0)// && carInfo.SaleState != "停销")
				{
					string yearType = carInfo.CarYear + "款";
					if (!yearList.Contains(yearType))
						yearList.Add(yearType);

					// 是否是在销年款
					if (carInfo.SaleState != "停销")
					{
						if (!yearSaleList.Contains(yearType))
						{ yearSaleList.Add(yearType); }
					}
					else
					{
						if (!noSaleYearList.Contains(yearType))
						{ noSaleYearList.Add(yearType); }
					}
				}


				#region 年款报价
				if (carInfo.CarYear == carYear.ToString())
				{
					double referPrice = 0.0;
					bool isDouble = Double.TryParse(carInfo.ReferPrice.Replace("万", ""), out referPrice);
					if (isDouble)
					{
						if (referPrice > maxPrice)
							maxPrice = referPrice;
						if (referPrice < minPrice)
							minPrice = referPrice;
					}

					//报价
					string[] priceRange = carInfo.CarPriceRange.Replace("万", "").Split('-');
					foreach (string priceStr in priceRange)
					{
						double price = 0.0;
						isDouble = Double.TryParse(priceStr, out price);
						if (isDouble)
						{
							if (price > maxDealerPrice)
								maxDealerPrice = price;
							if (price < minDealerPrice)
								minDealerPrice = price;
						}
					}
				#endregion
					//string netFuelStr = new Car_BasicBll().GetCarNetfriendsFuel(carInfo.CarID);
					//if (netFuelStr != "无")
					//{
					//	netFuelStr = netFuelStr.Replace("L", "");
					//	double fuel = 0.0;
					//	isDouble = Double.TryParse(netFuelStr, out fuel);
					//	if (isDouble)
					//	{
					//		if (fuel > maxNetFuel)
					//			maxNetFuel = fuel;
					//		if (fuel < minNetFuel)
					//			minNetFuel = fuel;
					//	}
					//}
					#region 车型网友油耗
				}

					#endregion
			}
			//排除包含在售年款
			foreach (string year in yearSaleList)
			{
				if (noSaleYearList.Contains(year))
				{
					noSaleYearList.Remove(year);
				}
			}
			// 判断年款是否存在 modified by chengl Apr.21.2010
			if (!yearList.Contains(carYear + "款"))
			{ Response.Redirect("/car/404error.aspx?info=无效年款"); }

			yearList.Sort(NodeCompare.CompareStringDesc);
			noSaleYearList.Sort(NodeCompare.CompareStringDesc);

			////年款的网友油耗
			//if (maxNetFuel == Double.MinValue && minNetFuel == Double.MaxValue)
			//	netFuel = "暂无";
			//else
			//	netFuel = minNetFuel + "-" + maxNetFuel + "L";

			if (maxPrice == Double.MinValue && minPrice == Double.MaxValue)
				serialReferPrice = "暂无";
			else
				serialReferPrice = minPrice + "万-" + maxPrice + "万";

			if (sic.CsSaleState == "停销")
				serialPrice = "停售";
			else if (sic.CsSaleState == "待销")
				serialPrice = "未上市";
			else if (maxDealerPrice == Double.MinValue && minDealerPrice == Double.MaxValue)
				serialPrice = "暂无";
			else
				serialPrice = minDealerPrice + "万-" + maxDealerPrice + "万";

			tableCode.AppendLine("<h3><span>" + carYear + "款" + serialShowName + "车款</span><em class=\"h3_spcar\">");
			tableCode.Append("<a href=\"" + baseUrl + "#car_list\">全部在售</a>");

			// modified by chengl Jun.24.2011
			// 根据在销年款生成
			for (int i = 0; i < yearSaleList.Count; i++)
			{
				string yearStr = yearSaleList[i];
				if (yearStr == carYear + "款")
					continue;
				string url = baseUrl + yearStr.Replace("款", "") + "/";
				tableCode.Append("<s>|</s><a href=\"" + url + "#car_list\">" + yearStr + "</a>");
			}
			//for (int i = 0; i < yearList.Count; i++)
			//{
			//    string yearStr = yearList[i];
			//    if (yearStr == carYear + "款")
			//        continue;
			//    string url = baseUrl + yearStr.Replace("款", "") + "/";
			//    tableCode.Append("|<a href=\"" + url + "#car_list\">" + yearStr + "</a>");
			//}

			if (serialId != 1568 && base.CheckSerialHasNoSale(serialId))
			{
				if (yearList.Count > 0)
					tableCode.Append("<s>|</s>");
				//tableCode.Append("<a href=\"http://www.cheyisou.com/chexing/" + Server.UrlEncode(serialShowName) + "/1.html?para=os|0|en|utf8\" target=\"_blank\">停售车款</a>");
				tableCode.Append("<dl id=\"bt_car_spcar_table\"><dt>停售年款<em></em></dt><dd style=\"display:none;\">");
				for (int i = 0; i < noSaleYearList.Count; i++)
				{
					string url = baseUrl + noSaleYearList[i].Replace("款", "") + "/";
					if (i == noSaleYearList.Count - 1)
						tableCode.Append("<a href=\"" + url + "\" target=\"_self\" class=\"last_a\">" + noSaleYearList[i] + "</a>");
					else
					{
						tableCode.Append("<a href=\"" + url + "\" target=\"_self\">" + noSaleYearList[i] + "</a>");
					}
				}
				tableCode.Append("</dd></dl>");
			}
			tableCode.Append("</em></h3>");

			tableCode.AppendLine("<div class=\"comparetable\">");

			tableCode.AppendLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"compare2\" id=\"compare\">");
			/* tableCode.AppendLine("<tr><th width=\"250px\">车款名称</th>");
			 tableCode.AppendLine("<th width=\"50px\">热度</th>");
			 tableCode.AppendLine("<th width=\"70px\">变速箱</th>");
			 tableCode.AppendLine("<th width=\"85px\">厂家指导价</th>");
			 tableCode.AppendLine("<th width=\"170px\">商家报价</th>");
			 tableCode.AppendLine("<th width=\"73px\">车型对比</th>");
			 tableCode.AppendLine("</tr>");*/

			StringBuilder temp = new StringBuilder();
			List<string> transList = new List<string>();
			string carIDs = string.Empty;
			int index = 0;
			if (isNoSaleYear)
			{
				// 如果是停销年款 取二手车报价
				dicUcarPrice = new Car_BasicBll().GetAllUcarPrice();
			}
			BitAuto.CarChannel.BLL.Car_BasicBll basicBll = new BitAuto.CarChannel.BLL.Car_BasicBll();
			foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
			{
				string carUrl = "http://car.bitauto.com/" + serialSpell + "/m" + carInfo.CarID + "/";
				string yearType = carInfo.CarYear.Trim();
				if (yearType.Length > 0)
				{
					yearType += "款";
					if (!yearHtmlDic.ContainsKey(yearType))
						yearHtmlDic[yearType] = "<ul>";
					yearHtmlDic[yearType] += "<li><a href=\"" + carUrl + "\" target=\"_blank\">" + carInfo.CarName + "</a></li>";
				}

				//不是本年款的不显示
				if (carInfo.CarYear != carYear.ToString())
					continue;

				//累加排量
				serialExhaust += "、" + carInfo.Engine_Exhaust;
				if (!exhaustList.Contains(carInfo.Engine_Exhaust))
				{
					if (carIDs != "")
					{
						tableCode.Append(string.Format(temp.ToString(), carIDs));
						temp.Remove(0, temp.Length);
					}
					carIDs = "";
					//显示排量行
					exhaustList.Add(carInfo.Engine_Exhaust);
					if (index < 1)
					{
						temp.AppendLine("<tr style=\"\"><th width=\"255px\" class=\"firstItem\"><b>" + carInfo.Engine_Exhaust + "排量</b></th>");
						temp.AppendLine("<th width='50px'>热度</th>");
						temp.AppendLine("<th width='70px'>变速箱</th>");
						temp.AppendLine("<th width='85px'>厂家指导价</th>");
						if (!isNoSaleYear)
						{
							temp.AppendLine("<th width='180px'>参考成交价</th>");
						}
						else
						{
							temp.AppendLine("<th width='126px'>二手车报价</th>");
							temp.AppendLine("<th width=\"\">&nbsp;</th>");
						}
						temp.AppendLine("<th width='58px'>&nbsp;</th>");
						temp.AppendLine("</tr>");
					}
					else
					{
						temp.AppendLine("<tr style=\"\"><th width=\"255px\" colspan=\"6\" class=\"firstItem\"><b>" + carInfo.Engine_Exhaust + "排量</b></th></tr>");
					}
					index++;
				}
				if (carIDs != "")
				{ carIDs += "," + carInfo.CarID; }
				else
				{ carIDs += carInfo.CarID; }



				string carFullName = yearType + " " + serialShowName + "&nbsp;" + carInfo.CarName;
				if (carInfo.CarName.StartsWith(serialShowName))
					carFullName = yearType + " " + serialShowName + "&nbsp;" + carInfo.CarName.Substring(serialShowName.Length);

				string stopPrd = "";
				if (carInfo.ProduceState == "停产")
					stopPrd += " <span class=\"tc\">停产</span>";

				// 节能补贴 Sep.2.2010
				string hasEnergySubsidy = "";
				bool isHasEnergySubsidy = new Car_BasicBll().CarHasParamEx(carInfo.CarID, 853);
				if (isHasEnergySubsidy)
				{
					hasEnergySubsidy = " <span class=\"butie\"><a href=\"http://news.bitauto.com/zcxwtt/20130924/1406235633.html\" title=\"可获得3000元节能补贴\" target=\"_blank\">补贴</a></span>";
				}
				//============2012-04-09 减税============================
				Dictionary<int, string> dict = basicBll.GetCarAllParamByCarID(carInfo.CarID);
				string strTravelTax = "";
				if (dict.ContainsKey(895))
				{
					strTravelTax = " <span class=\"jianshui\"><a target=\"_blank\" title=\"{0}\" href=\"http://news.bitauto.com/others/20120308/0805618954.html\">减税</a></span>";
					if (dict[895] == "减半")
						strTravelTax = string.Format(strTravelTax, "减征50%车船使用税");
					else if (dict[895] == "免征")
						strTravelTax = string.Format(strTravelTax, "免征车船使用税");
					else
						strTravelTax = "";
				}
				temp.AppendLine("<tr><td><a href=\"" + carUrl + "\" target=\"_blank\">" + carFullName + "</a>" + stopPrd + strTravelTax + hasEnergySubsidy + "</td>");
				//计算百分比
				int percent = (int)Math.Round((double)carInfo.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero);
				temp.AppendLine("<td><div class=\"w\"><div class=\"p\"  style=\"width:" + percent + "%\"></div></div></td>");

				// add by chengl Dec.15.2011
				if (carInfo.CarPV > hotCarHotCount && carInfo.CarPriceRange.Trim().Length > 0)
				{
					// 取有报价车型中最热的车型 "预约试驾" url使用
					hotCarID = carInfo.CarID;
					hotCarHotCount = carInfo.CarPV;
				}

				//变速器类型
				string tempTransmission = carInfo.TransmissionType;
				if (tempTransmission.IndexOf("挡") >= 0)
				{
					tempTransmission = tempTransmission.Substring(tempTransmission.IndexOf("挡") + 1, tempTransmission.Length - tempTransmission.IndexOf("挡") - 1);
				}
				tempTransmission = tempTransmission.Replace("变速器", "");
				temp.AppendLine("<td>" + tempTransmission + "</td>");

				if (transList.Count < 2)
				{
					if (tempTransmission.IndexOf("手动") == -1)
						tempTransmission = "自动";
					if (!transList.Contains(tempTransmission))
						transList.Add(tempTransmission);
				}

				//指导价
				if (carInfo.ReferPrice.Trim().Length == 0)
					temp.AppendLine("<td style=\"text-align:right\">暂无</td>");
				else
				{
					if (carInfo.CarPriceRange == "停售")
					{
						temp.AppendLine("<td style=\"text-align:right\"><span >" + carInfo.ReferPrice + "万</span><a class=\"icon_cal\" ></a></td>");
					}
					else
					{
						temp.AppendLine("<td style=\"text-align:right\"><span>" + carInfo.ReferPrice + "万</span><a title=\"购车费用计算\" target=\"_blank\" class=\"icon_cal\" href=\"http://car.bitauto.com/gouchejisuanqi/?carid=" + carInfo.CarID + "\"></a></td>");
					}
				}
				// 			if (carInfo.PerfFuelCostPer100.Trim().Length == 0)
				// 				temp.AppendLine("<td style=\"text-align:right\"></td>");
				// 			else
				// 				temp.AppendLine("<td style=\"text-align:right\">" + carInfo.PerfFuelCostPer100 + "L</td>");
				//报价
				if (!isNoSaleYear)
				{
					// 非停销年款
					if (carInfo.CarPriceRange.Trim().Length == 0)
						temp.AppendLine("<td class=\"noPrice3\" style=\"text-align:right\">暂无报价</td>");
					else
					{
						if (carInfo.CarPriceRange == "停售")
						{
							temp.AppendLine("<td style=\"text-align:right\"><span><a>" + carInfo.CarPriceRange + "</a></span></td>");
						}
						else
						{
							//20130412 edit anh
							temp.AppendLine("<td style=\"text-align:right\"><span><a href=\"http://car.bitauto.com/" + serialSpell + "/m" + carInfo.CarID + "/baojia/\" target=\"_blank\">" + carInfo.CarPriceRange + "</a></span> <a href=\"http://dealer.bitauto.com/zuidijia/nb" + serialId + "/nc" + carInfo.CarID + "/\" target=\"_blank\">询价>></a></td>");
						}
					}
				}
				else
				{
					// 停销年款用二手车报价
					if (dicUcarPrice != null && dicUcarPrice.Count > 0 && dicUcarPrice.ContainsKey(carInfo.CarID))
					{
						//temp.AppendLine("<td style=\"text-align:right\"><span><a target=\"_blank\" href=\"http://yiche.taoche.com/buycar/b-"
						//    + serialSpell + "/?page=1&carid=" + carInfo.CarID
						//    + "\" >" + dicUcarPrice[carInfo.CarID]
						//    + "</a></span></td>");
						temp.AppendFormat("<td style=\"text-align:right\"><span><a target=\"_blank\" href=\"http://www.taoche.com/buycar/serial/{0}/?ref=car3\">{1}</a></span></td>", serialSpell, dicUcarPrice[carInfo.CarID]);
						temp.AppendFormat("<td class=\"small\"><a class=\"addCompare\" href=\"http://www.taoche.com/buycar/serial/{0}/?ref=car3\" target=\"_blank\">二手车</a></td>", serialSpell);
					}
					else
					{
						temp.AppendLine("<td class=\"noPrice3\" style=\"text-align:right\">暂无报价</td>");
						temp.Append("<td class=\"small\"></td>");
					}
				}
				temp.Append("<td id=\"tdForCompareCar_" + carInfo.CarID + "\" class=\"small\">");
				temp.AppendLine("<a class=\"addCompare\" href=\"javascript:addCarToCompare('" + carInfo.CarID.ToString() + "','" + carInfo.CarName.ToString() + "');\" >+对比</a></td></tr>");
			}
			if (carIDs != "")
			{
				tableCode.Append(string.Format(temp.ToString(), carIDs));
			}
			else
				tableCode.Append("<tr><td class=\"noline\" colspan=\"7\">暂无在销车型！</td></tr>");
			tableCode.AppendLine("</table>");
			// modified by chengl Dec.13.2011
			// tableCode.AppendLine("<div class=\"more\"><a href=\"http://yiche.taoche.com/buycar/b-" + serialSpell + "/\" target=\"_blank\" class=\"more2new\">买二手车</a><a href=\"http://go.bitauto.com/goumai/?id=" + serialId + "\" target=\"_blank\" class=\"more2new\">计划购买</a><a href=\"http://go.bitauto.com/guanzhu/?id=" + serialId + "\" target=\"_blank\" class=\"more2new\">加入收藏</a><a class=\"more2new\" href=\"http://ask.bitauto.com/browse/" + serialId + "/\" target=\"_blank\">买前咨询</a></div>");

			// modified by chengl May.25.2012
			tableCode.AppendLine("<div class=\"more\"><a href=\"http://www.taoche.com/buycar/serial/" + serialSpell + "/?ref=car2\" target=\"_blank\" class=\"more2new\">买二手车</a><a id=\"LinkForBaaAttention\" href=\"http://i.bitauto.com/baaadmin/car/guanzhu_" + serialId + "/\" target=\"_blank\" class=\"more2new\">加关注</a>");
			// tableCode.AppendLine("<div class=\"more\"><a href=\"http://yiche.taoche.com/similarcar/serial/" + serialSpell + "/paesf0bxc/?from=bitauto\" target=\"_blank\" class=\"more2new\">买二手车</a><a href=\"http://i.bitauto.com/baaadmin/car/goumai_" + serialId + "/\" target=\"_blank\" class=\"more2new\">计划购买</a><a href=\"http://i.bitauto.com/baaadmin/car/guanzhu_" + serialId + "/\" target=\"_blank\" class=\"more2new\">加入收藏</a>");
			//20130412 edit anh
			//if (hotCarID > 0)
			//{
			//    tableCode.AppendLine("<a class=\"more2new\" href=\"/" + serialSpell + "/m" + hotCarID + "/baojia/#V\" target=\"_blank\">预约试驾</a>");
			//}
			//else
			//{
			tableCode.AppendFormat("<a class=\"more2new\" href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">预约试驾</a>", serialId);
			//}
			tableCode.AppendLine("</div>");

			tableCode.AppendLine("</div>");
			tableCode.AppendLine("<div class=\"clear\"></div>");
			carListTableHtml = tableCode.ToString();

			//年款下拉列表
			StringBuilder listCode = new StringBuilder();

			//listCode.AppendLine("<h5><a>" + carYear + "款</a></h5>");
			listCode.AppendLine(yearHtmlDic[carYear + "款"] + "</ul>");

			topCarListHtml = listCode.ToString();

			//年款的排量与变速器
			serialExhaust = CommonFunction.GetExhaust(serialExhaust);
			serialTransmission = String.Join("　", transList.ToArray());

		}
        

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
						CsPicJiaodian = "<a href=\"" + xn.Attributes["Link"].Value.ToString() + "\" target=\"_blank\"><img alt=\"" + serialShowName + xn.Attributes["ImageName"].Value.ToString() + "\" src=\"" + xn.Attributes["ImageUrl"].Value.ToString() + "\" width=\"300\" height=\"200\"></a>";
						break;
					}
				}
				else
				{
					CsPicJiaodian = "<img src=\"" + WebConfig.DefaultCarPic + "\" width=\"300\" height=\"200\">";
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
						int imgID = int.Parse(xn.Attributes["ImageId"].Value.ToString()) % 4 + 1;
						CsPicJiaodian = "<a href=\"" + xn.Attributes["Link"].Value.ToString() + "\" target=\"_blank\"><img alt=\"" + serialShowName + xn.Attributes["ImageName"].Value.ToString() + "\" src=\"http://img" + imgID + ".bitautoimg.com/autoalbum/" + string.Format(xn.Attributes["ImageUrl"].Value.ToString(), "4") + "\" width=\"300\" height=\"200\"></a>";
						break;
					}
				}
				else
				{
					CsPicJiaodian = "<img src=\"" + WebConfig.DefaultCarPic + "\" width=\"300\" height=\"200\">";
				}
				return;
			}
		}

		private void MakeKoubeiImpressionHtml()
		{
			int koubei = (int)CommonHtmlEnum.BlockIdEnum.KoubeiImpression;
			if (dictSerialBlockHtml.ContainsKey(koubei))
			{
				koubeiImpressionHtml = dictSerialBlockHtml[koubei];
				return;
			}

			string filePath = Path.Combine(WebConfig.DataBlockPath, string.Format(@"Data\SerialDianping\Impression\Xml\Impression_{0}.xml", serialId));
			XmlDocument xmlDoc = new XmlDocument();
			if (File.Exists(filePath))
				xmlDoc.Load(filePath);
			if (xmlDoc == null) return;

			StringBuilder sb = new StringBuilder();

			XmlNode impressionNode = xmlDoc.SelectSingleNode("/Root/Serial/Impression");
			XmlNode virtuesNode = xmlDoc.SelectSingleNode("/Root/Serial/Virtues");
			XmlNode defectNode = xmlDoc.SelectSingleNode("/Root/Serial/Defect");

			string impression = impressionNode == null ? String.Empty : impressionNode.InnerText.Trim();
			string virtues = virtuesNode == null ? String.Empty : virtuesNode.InnerText.Trim();
			string defect = defectNode == null ? String.Empty : defectNode.InnerText.Trim();

			if (impression.Length > 0 || virtues.Length > 0 || defect.Length > 0)
			{
				impression = StringHelper.RemoveHtmlTag(StringHelper.SubString(impression, 96, true));
				virtues = StringHelper.RemoveHtmlTag(virtues);
				defect = StringHelper.RemoveHtmlTag(defect);

				string reportUrl = string.Format("/{0}/koubei/", serialSpell);

				sb.Append("	<div class=\"line-box\">");
				sb.Append("<div class=\"side_title\">");
				sb.AppendFormat("<h4><a href=\"{0}\" target=\"_blank\">网友对此车的印象</a></h4>", reportUrl);
				sb.Append("</div>");

				//sb.AppendFormat("<p>{1}<a href=\"{0}\" target=\"_blank\">查看详情&gt;&gt;</a></p>", reportUrl, impression);
				sb.Append("<div class=\"youque_box\"><h6 class=\"fl\">优点：</h6>");
				if (StringHelper.GetRealLength(virtues) > 48)
					sb.AppendFormat("<p class=\"txt\" title=\"{0}\">{1}</p>", virtues, StringHelper.SubString(virtues, 46, true));
				else
					sb.AppendFormat("<p class=\"txt\">{0}</p>", virtues);
				sb.Append("</div>");
				sb.Append("<div class=\"youque_box quedian\"><h6 class=\"fl\">缺点：</h6>");
				if (StringHelper.GetRealLength(defect) > 48)
					sb.AppendFormat("<p class=\"txt\" title=\"{0}\">{1}</p>", defect, StringHelper.SubString(defect, 46, true));
				else
					sb.AppendFormat("<p class=\"txt\">{0}</p>", defect);
				sb.Append("</div>");
				sb.Append("        <div class=\"btn_box\">");
				sb.Append("        	<span class=\"button_orange\"><a href=\"http://ask.bitauto.com/tiwen/\" target=\"_blank\" class=\"koubei_b_tiwen\">我要提问</a></span>");
				sb.AppendFormat("   <span class=\"button_gray\"><a href=\"http://car.bitauto.com/{0}/koubei/#fabu\" target=\"_blank\" class=\"koubei_b_dianping\">我要点评</a></span>", serialSpell);
				sb.Append("        </div>");
				sb.Append("<div class=\"clear\"></div>");
				sb.Append("    </div>");
			}
			koubeiImpressionHtml = sb.ToString();
		}
		/// <summary>
		/// 生成颜色Hmtl
		/// </summary>
		private void MakeSerialYearColorHtml()
		{
			StringBuilder sb = new StringBuilder();
			var colorList = _serialBLL.GetColorRGBBySerialId(sic.CsID, carYear, sic.ColorList);
			if (colorList.Count > 0)
			{
				var maxColorNum = 26;
				foreach (var colorEntity in colorList.Take(maxColorNum))
				{
					if (string.IsNullOrEmpty(colorEntity.ColorLink))
						sb.AppendFormat("<em><b style=\"background: {0}\" title=\"{1}\"></b></em>", colorEntity.ColorRGB, colorEntity.ColorName);
					else
					{
						sb.Append("<em>");
						sb.AppendFormat("<a href=\"{1}\" target=\"_blank\"><b style=\"background: {0}\"></b></a>", colorEntity.ColorRGB, colorEntity.ColorLink);
						sb.Append("<div class=\"tc tc-color-box\" style=\"display:none; \">");
						sb.Append("<div class=\"tc-box tc-color\">");
						sb.Append("<i></i>");
						sb.AppendFormat("<a target=\"_blank\" href=\"{0}\">", colorEntity.ColorLink);
						sb.AppendFormat("<img src=\"{0}\" width=\"150\" height=\"100\">", colorEntity.ColorImageUrl.Replace("_5.", "_1."));
						sb.Append("</a>");
						sb.AppendFormat("<p><a target=\"_blank\" href=\"{0}\">{1}</a></p>", colorEntity.ColorLink, colorEntity.ColorName);
						sb.Append("</div>");
						sb.Append("</div>");
						sb.Append("</em>");
					}
				}
				//if (colorList.Count > maxColorNum)
				//{
				//    sb.Append("<a href=\"javascript:;\" id=\"more-color\" class=\"more\"><strong></strong></a>");
				//    sb.Append("<div id=\"more-color-sty\" class=\"color-sty more-color-sty\" style=\"display:none;\">");
				//    foreach (var colorEntity in colorList.Skip(maxColorNum))
				//    {
				//        if (string.IsNullOrEmpty(colorEntity.ColorLink))
				//            sb.AppendFormat("<em><b style=\"background: {0}\" title=\"{1}\"></b></em>", colorEntity.ColorRGB, colorEntity.ColorName);
				//        else
				//        {
				//            //sb.AppendFormat("<em><a href=\"{1}\" target=\"_blank\"><b style=\"background: {0}\"></b></a></em>", colorEntity.ColorRGB, colorEntity.ColorLink);
				//            sb.Append("<em>");
				//            sb.AppendFormat("<a href=\"{1}\" target=\"_blank\"><b style=\"background: {0}\"></b></a>", colorEntity.ColorRGB, colorEntity.ColorLink);
				//            sb.Append("<div class=\"tc tc-color-box\" style=\"display:none; \">");
				//            sb.Append("<div class=\"tc-box tc-color\">");
				//            sb.Append("<i></i>");
				//            sb.AppendFormat("<a target=\"_blank\" href=\"{0}\">", colorEntity.ColorLink);
				//            sb.AppendFormat("<img src=\"{0}\" width=\"150\" height=\"100\">", colorEntity.ColorImageUrl.Replace("_5.", "_1."));
				//            sb.Append("</a>");
				//            sb.AppendFormat("<p><a target=\"_blank\" href=\"{0}\">{1}</a></p>", colorEntity.ColorLink, colorEntity.ColorName);
				//            sb.Append("</div>");
				//            sb.Append("</div>");
				//            sb.Append("</em>");
				//        }
				//    }
				//    sb.Append("</div>");
				//}
				serialColorHtml = sb.ToString();
			}
		}

		/// <summary>
		/// 生成子品牌概况Html
		/// </summary>
		private void MakeSerialOverview()
		{

			//StringBuilder htmlCode = new StringBuilder();
			////htmlCode.AppendLine("<div class=\"line_box zs02 zs100412_4\">");
			////htmlCode.Append("<h3><span><a>");
			////htmlCode.AppendLine(carYear + "款" + serialShowName + "</a></span></h3>");
			//htmlCode.AppendLine("<ul class=\"d\">");
			////<li class="w">颜色：<em style="background:black"></em><em style="background:blue"></em><em style="background:red"></em></li>

			//// 停销也按色块显示 anh 20110830
			//// modified by chengl Sep.26.2010
			//// 停销子品牌显示全部颜色(文字形式)
			////if (sic.CsSaleState == "停销")
			////{
			////    string colorHtml = "";
			////    string allColorHtml = "";
			////    foreach (string colorStr in sic.ColorList)
			////    {
			////        if (allColorHtml.Length > 0)
			////            allColorHtml += "　";
			////        allColorHtml += colorStr;
			////    }
			////    if (sic.ColorList.Count > 4)
			////    {
			////        if (sic.ColorList[0].Length + sic.ColorList[1].Length + sic.ColorList[2].Length + sic.ColorList[sic.ColorList.Count - 1].Length > 16)
			////        {
			////            colorHtml = sic.ColorList[0] + "　…　" + sic.ColorList[sic.ColorList.Count - 1];
			////        }
			////        else
			////        {
			////            colorHtml = sic.ColorList[0] + "　" + sic.ColorList[1] + "　" + sic.ColorList[2] + "　" + sic.ColorList[3] + "　…　" + sic.ColorList[sic.ColorList.Count - 1];
			////        }
			////    }
			////    else
			////    {
			////        colorHtml = allColorHtml;
			////    }

			////    htmlCode.Append("<li class=\"w\">颜色：<span class=\"c w330\" title=\"" + allColorHtml + "\">");
			////    htmlCode.Append(colorHtml);
			////    htmlCode.Append("</span></li>");
			////}
			////else
			////{
			//string rgbHTML = "";
			//string rgbTitle = "";
			//List<string> listColorName = new List<string>();
			//List<string> listColorRGB = new List<string>();
			//new Car_SerialBll().GetSerialColorRGBByCsID(sic.CsID, carYear, 1, sic.ColorList
			//    , out rgbHTML, out rgbTitle, out listColorName, out listColorRGB);
			//// new Car_SerialBll().GetSerialColorRGBByCsID(sic.CsID, carYear, sic.ColorList, out rgbHTML, out rgbTitle);
			//// htmlCode.Append("<li class=\"w\"><label>颜色：</label><span class=\"c\" title=\"" + rgbTitle + "\">");
			//htmlCode.Append("<li class=\"w\"><label>颜色：</label><span class=\"c w330\" >");
			//htmlCode.Append(rgbHTML);
			//htmlCode.Append("</span></li>");
			////}
			//// modified end

			//htmlCode.AppendLine("<li><label>保修：</label>" + sic.SerialRepairPolicy + "</li>");

			////         if (sic.CsID == 2566 || sic.CsID == 2844 || sic.CsID == 2944)
			////         {
			////             // 2566:睿翼  2844:马自达CX-7 2944:睿翼轿跑 显示 综合工况油耗
			////             htmlCode.Append("<li class=\"s\">综合工况油耗：");
			////         }
			////         else
			////             htmlCode.Append("<li class=\"s\">官方油耗：");
			////         htmlCode.Append(sic.CsOfficialFuelCost + "</li>");

			//if (sic.CsSummaryFuelCost.Length > 0)
			//    htmlCode.Append("<li class=\"s\"><label>综合工况油耗：</label>" + sic.CsSummaryFuelCost + "</li>");
			//else
			//    htmlCode.Append("<li class=\"s\"><label>官方油耗：</label>" + sic.CsOfficialFuelCost + "</li>");

			//htmlCode.AppendLine("<li><label>厂家：</label><a href=\"http://car.bitauto.com/producer/" + cse.Cp_id + ".html\" target=\"_blank\">" + cse.Cp_ShortName + "</a></li>");
			////<li>网友发布：<a href="">11.xL-11.xxL</a></li>

			//htmlCode.AppendLine("<li class=\"s\"><label>网友发布：</label><a href=\"" + baseUrl + "youhao/\" target=\"_blank\">" + netFuel + "</a></li>");
			//htmlCode.AppendLine("</ul>");
			////htmlCode.AppendLine("<div class=\"clear\"></div>");
			////if (sic.OfficialSite.Length > 0)
			////    htmlCode.AppendLine("<div class=\"more\"><a href=\"" + sic.OfficialSite + "\" target=\"_blank\">官方网站</a></div>");

			//CsDetailInfo = htmlCode.ToString();
		}

		private void MakeMustSee()
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<dl class=\"zs02\">");
			htmlCode.AppendLine("<dt>买车必看</dt>");
			htmlCode.AppendLine("<dd class=\"sublink\">");
			string linkStr = "";
			if (sic.CsNewYiCheCheShi.Trim().Length > 0)
			{
				linkStr = "<a href=\"" + sic.CsNewYiCheCheShi + "\" target=\"_blank\">易车评测</a>";
			}
			if (sic.CsNewMaiCheCheShi.Trim().Length > 0)
			{
				if (linkStr.Length > 0)
					linkStr += " | ";
				linkStr += "<a href=\"" + sic.CsNewMaiCheCheShi + "\" target=\"_blank\">买车测试</a>";
			}
			htmlCode.AppendLine(linkStr);
			htmlCode.AppendLine(" | <a class=\"nolink\">口碑报告</a></dd>");

			htmlCode.AppendLine("<dd>");
			htmlCode.AppendLine("<ul class=\"gb\">");
			htmlCode.AppendLine("<li title=\"" + cse.Cs_Virtues.Trim() + "\" class=\"g\">" + StringHelper.SubString(cse.Cs_Virtues.Trim(), 68, false) + "</li>");
			htmlCode.AppendLine("<li title=\"" + cse.Cs_Defect.Trim() + "\" class=\"b\">" + StringHelper.SubString(cse.Cs_Defect.Trim(), 68, false) + "</li>");
			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("<ul class=\"l\">");
			htmlCode.AppendLine("<li>");
			if (sic.CsNewShangShi.Trim().Length == 0)
				htmlCode.AppendLine("<a class=\"nolink\">上市专题</a></li>");
			else
				htmlCode.AppendLine("<a target=\"_blank\" href=\"" + sic.CsNewShangShi + "\">上市专题</a></li>");
			if (sic.CsNewGouCheShouChe.Trim().Length == 0)
				htmlCode.AppendLine("<li><a class=\"nolink\">购车手册</a></li>");
			else
				htmlCode.AppendLine("<li><a target=\"_blank\" href=\"" + sic.CsNewGouCheShouChe + "\">购车手册</a></li>");
			if (new ProduceAndSellDataBll().HasSerialData(serialId))
				htmlCode.AppendLine("<li><a href=\"" + String.Format(sic.CsNewXiaoShouShuJu.Trim(), serialId) + "\" target=\"_blank\">销量</a></li>");
			else
				htmlCode.AppendLine("<li><a class=\"nolink\">销量</a></li>");
			if (sic.CsNewKeJi.Trim().Length == 0)
				htmlCode.AppendLine("<li><a class=\"nolink\">科技</a></li>");
			else
				htmlCode.AppendLine("<li><a href=\"" + sic.CsNewKeJi.Trim() + "\" target=\"_blank\">科技</a></li>");
			if (sic.CsNewAnQuan.Trim().Length == 0)
				htmlCode.AppendLine("<li><a class=\"nolink\">安全</a></li>");
			else
				htmlCode.AppendLine("<li><a href=\"" + sic.CsNewAnQuan.Trim() + "\" target=\"_blank\">安全</a></li>");
			if (sic.CsNewWeiXiuBaoYang.Trim().Length == 0)
				htmlCode.AppendLine("<li><a class=\"nolink\">维修保养</a></li>");
			else
				htmlCode.AppendLine("<li><a target=\"_blank\" href=\"" + sic.CsNewWeiXiuBaoYang + "\">维修保养</a></li>");
			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("</dd>");
			htmlCode.AppendLine("</dl>");
			CsMustSeeInfo = htmlCode.ToString();
		}

		private void MakeTopNews()
		{
			//StringBuilder htmlCode = new StringBuilder();
			////获取数据
			//XmlDocument xmlDoc = new Car_SerialBll().GetSerialFocusNews(serialId);

			////导购新闻
			//XmlNodeList newsList = xmlDoc.SelectNodes("/root/Introduce/listNews");
			//daogouHtml = MakeOtherNews(newsList, "introduce");
			//导购新闻
			daogouHtml = MakeDaogouNews();
		}

		private string MakeTypeNews(string newsType)
		{
			StringBuilder htmlCode = new StringBuilder();
			DataSet newsDs = new Car_SerialBll().GetNewsListBySerialId(serialId, newsType);
			if (newsDs != null && newsDs.Tables.Count > 0 && newsDs.Tables[0].Rows.Count > 0 && newsDs.Tables.Contains("listNews"))
			{
				int newsCounter = 0;
				foreach (DataRow row in newsDs.Tables["listNews"].Rows)
				{
					newsCounter++;
					string newsTitle = Convert.ToString(row["title"]);
					int newsId = ConvertHelper.GetInteger(row["newsid"]);
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
					string newsUrl = Convert.ToString(row["filepath"]);
					DateTime publishTime = Convert.ToDateTime(row["publishtime"]);
					htmlCode.AppendLine("<li><a target=\"_blank\" title=\"" + newsTitle + "\" href=\"" + newsUrl + "\">" + newsTitle + "</a><small>" + publishTime.ToString("MM-dd") + "</small></li>");
					if (newsCounter >= 12)
						break;
				}
			}
			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成导购新闻推荐
		/// </summary>
		/// <param name="htmlCode"></param>
		/// <param name="newsList"></param>
		private string MakeDaogouNews()
		{
			StringBuilder htmlCode = new StringBuilder();
			DataSet ds = new CarNewsBll().GetTopSerialNews(sic.CsID, CarNewsType.daogou, 6);
			DataRowCollection rows = null;
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				rows = ds.Tables[0].Rows;
			string codeTitle = serialShowName + "-导购推荐";
			string moreUrl = baseUrl + "daogou/";

			//htmlCode.AppendLine("<h3><span><a href=\"" + moreUrl + "\" target=\"_blank\">" + codeTitle + "</a></span></h3>");
			htmlCode.AppendLine("<div class=\"h4_box\"><h4><span>导购推荐</span></h4>");
			if (rows != null && rows.Count > 0)
				htmlCode.AppendLine("<div class=\"more\"><a href=\"" + moreUrl + "\" target=\"_blank\">更多&gt;&gt; </a></div>");
			htmlCode.AppendLine("</div>");

			htmlCode.AppendLine("<div class=\"mainlist_box reco\">");
			htmlCode.AppendLine("<ul class=\"list_date\">");
			if (rows != null && rows.Count > 0)
			{
				foreach (DataRow newsRow in rows)
				{
					string newsTitle = CommonFunction.NewsTitleDecode(newsRow["title"].ToString());
					//过滤Html标签
					string shortNewsTitle = newsTitle;
					string filePath = newsRow["filepath"].ToString();
					string pubTime = Convert.ToDateTime(newsRow["publishtime"]).ToString("MM-dd");
					if (shortNewsTitle != newsTitle)
						htmlCode.AppendLine("<li><a href=\"" + filePath + "\" title=\"" + newsTitle + "\" target=\"_blank\">" + shortNewsTitle + "</a><small>" + pubTime + "</small></li>");
					else
						htmlCode.AppendLine("<li><a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a><small>" + pubTime + "</small></li>");
				}
			}
			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("</div>");
			return htmlCode.ToString();
		}
		/// <summary>
		/// 生成论坛新闻推荐
		/// </summary>
		/// <param name="htmlCode"></param>
		/// <param name="newsList"></param>
		private string MakeForumNews(XmlNodeList newsList)
		{
			StringBuilder htmlCode = new StringBuilder();
			// string bbsURL = new Car_SerialBll().GetForumUrlBySerialId(serialId);

			string codeTitle = serialShowName + "-论坛话题";
			string moreUrl = baaUrl;

			htmlCode.AppendLine("<h3><span><a href=\"" + moreUrl + "\" target=\"_blank\">" + codeTitle + "</a></span></h3>");
			if (newsList.Count > 0)
				htmlCode.AppendLine("<div class=\"more\"><a href=\"" + moreUrl + "\" target=\"_blank\">更多&gt;&gt; </a></div>");
			htmlCode.AppendLine("<div class=\"mainlist_box reco\">");
			htmlCode.AppendLine("<ul class=\"list_date\">");
			int loop = 1;
			foreach (XmlElement newsNode in newsList)
			{
				string newsTitle = newsNode.SelectSingleNode("title").InnerText.Trim();
				//过滤Html标签
				newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
				string shortNewsTitle = StringHelper.SubString(newsTitle, 40, true);
				string filePath = newsNode.SelectSingleNode("url").InnerText;
				string pubTime = string.Empty;
				if (shortNewsTitle != newsTitle)
					htmlCode.AppendLine("<li><a href=\"" + filePath + "\" title=\"" + newsTitle + "\" target=\"_blank\">" + shortNewsTitle + "</a><small>" + pubTime + "</small></li>");
				else
					htmlCode.AppendLine("<li><a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a><small>" + pubTime + "</small></li>");

				// modified by chengl Jul.22.2010
				loop++;
				if (loop > 5)
				{ break; }
			}
			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("</div>");
			return htmlCode.ToString();
		}


		/// <summary>
		/// 生成彩虹条
		/// </summary>
		private void MakeRainbowHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			//获取数据
			string rainbowStr = new RainbowListBll().GetRainbowListXML_CSID(serialId);
			XmlDocument rainbowDoc = new XmlDocument();
			rainbowDoc.LoadXml(rainbowStr);
			string headStr = "";
			string conStr = "";
			XmlElement serialNode = (XmlElement)rainbowDoc.SelectSingleNode("/RainbowRoot/Serial");
			bool isShow = Convert.ToBoolean(serialNode.GetAttribute("IsShow"));
			if (isShow)
			{
				XmlNodeList eleList = rainbowDoc.SelectNodes("/RainbowRoot/Serial/Item");
				string importWidth = importWidth = "style=\"width:70px\""; ;
				if (eleList.Count == 6)
					importWidth = "style=\"width:119px\"";

				bool hasCar = false;			//是否已经有车图显示了
				for (int i = eleList.Count - 1; i >= 0; i--)
				{
					XmlElement ele = (XmlElement)eleList[i];
					string name = ele.GetAttribute("Name");
					//国产车型去掉三项
					if (eleList.Count > 6 && (name == "易车评测" || name == "口碑"))
						continue;
					string url = ele.GetAttribute("URL");
					string time = "";
					if (url.Trim().Length > 0)
						time = Convert.ToDateTime(ele.GetAttribute("Time")).ToString("yyyy-MM-dd");

					// modified by chengl Dec.8.2009 if KouBei Tag goto koubei link
					if (name == "口碑")
					{
						url = baseUrl + "/koubei/";
						time = DateTime.Now.ToShortDateString();
					}

					//计算彩虹条
					headStr = "<th scope=\"col\"><div " + importWidth + ">" + name + "</div></th>" + headStr;
					if (url.Length > 0)
					{
						if (hasCar)
							conStr = "<td class=\"rainbow_" + (i + 1) + "\"><a href=\"" + url + "\" target=\"_blank\">" + time + "</a></td>" + conStr;
						else
						{
							conStr = "<td class=\"rainbow_comp\"><a href=\"" + url + "\" target=\"_blank\">" + time + "</a></td>" + conStr;
							hasCar = true;
						}
						//showRainbow = true;
					}
					else
						conStr = "<td class=\"rainbow_none\">及时关注</td>" + conStr;
				}
				// string forumUrl = new Car_SerialBll().GetForumUrlBySerialId(serialId);

				htmlCode.AppendLine("<div class=\"line_box rainbow_box\">");
				htmlCode.AppendLine("<h3 class=\"gr\"><span><span class=\"caption\">" + serialShowName + "追踪报道</span></span></h3>");
				if (eleList.Count == 6)
				{
					//进口车
					htmlCode.AppendLine("<table class=\"table_rainbow2 table_rainbow\">");
				}
				else
				{
					htmlCode.AppendLine("<table class=\"table_rainbow\">");
				}
				htmlCode.AppendLine("<tbody><tr>");
				htmlCode.AppendLine(headStr);
				htmlCode.AppendLine("</tr><tr>");
				htmlCode.AppendLine(conStr);
				htmlCode.AppendLine("</tbody></table>");
				htmlCode.AppendLine("<div class=\"more\"></div>");
				htmlCode.AppendLine("</div>");
			}
			rainbowHtml = htmlCode.ToString();
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

							// string picNum = "0";
							string allPicUrl = "http://car.bitauto.com/" + cse.Cs_AllSpell.Trim() + "/m" + carid.ToString() + "/tupian/";

							PhotoData.Append("<div class=\"title-con\">");
							PhotoData.Append("<div class=\"title-box title-box2\">");
							PhotoData.Append("<h4><a href=\"" + allPicUrl + "\">" + cse.Cs_ShowName.Trim() + "图片 </a></h4>");
							PhotoData.Append("<span>共" + picCount + "张图片</span>");
							PhotoData.Append("<div class=\"more\">" + moreLink + " | <a href=\"http://photo.bitauto.com/model/" + carid.ToString() + "/\" target=\"_blank\">更多&gt;&gt;</a></div>");
							PhotoData.Append("</div>");
							PhotoData.Append("</div>");



							//PhotoData.Append("<h3><span><a href=\"" + allPicUrl + "\">" + cse.Cs_ShowName.Trim() + "-图片 </a></span>");
							//PhotoData.AppendLine("<label class=\"h3sublink\"><a href=\"" + allPicUrl + "\">" + picCount.ToString() + "张</a></label></h3>");

							//PhotoData.Append("<div class=\"clear\"></div>");
							//PhotoData.AppendLine("<div class=\"more pic13_more\">" + moreLink + " <a href=\"http://photo.bitauto.com/model/" + carid.ToString() + "/\" target=\"_blank\">更多&gt;&gt;</a></div>");
							//PhotoData.Append("<div class=\"clear\"></div>");

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
							//PhotoData.AppendLine("<div class=\"pic_album\">");
							// 外观
							RenderPicByCategory(ref PhotoData, carPhoto, 6, ref isHasContent);
							// 内饰
							RenderPicByCategory(ref PhotoData, carPhoto, 7, ref isHasContent);
							// 空间
							RenderPicByCategory(ref PhotoData, carPhoto, 8, ref isHasContent);
							// 图解
							RenderPicByCategory(ref PhotoData, carPhoto, 12, ref isHasContent);
							//PhotoData.AppendLine("</div>");
							if (isHasContent)
							{
								PhotoData.AppendLine("<div class=\"clear\"></div>");
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
								moreCateLink += "<a target=\"_blank\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/6/\">外观</a> | ";
							}
							else if (cateId == 12)
							{
								moreCateLink += "<a target=\"_blank\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/12/\">图解</a> | ";
							}
							else if (cateId == 11)
							{
								moreCateLink += "<a target=\"_blank\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/11/\">官方图</a> | ";
							}
							else if (cateId == 0)
							{
								moreCateLink += "<a target=\"_blank\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/0/\">到店实拍</a> | ";
							}
							else
							{ }
							if (cateId != 6 && cateId != 7 && cateId != 8 && cateId != 12)
								continue;
							categoryPicNum[cateId] = cateNum;

						}
					}
					string allPicUrl = baseUrl + "tupian/";

					htmlCode.Append("<div class=\"title-con\">");
					htmlCode.Append("<div class=\"title-box title-box2\">");
					htmlCode.Append("<h4><a href=\"" + allPicUrl + "\">" + serialShowName + "图片 </a></h4>");
					htmlCode.Append("<span>共" + picNum + "张图片</span>");
					htmlCode.Append("<div class=\"more\">" + moreCateLink + "<a href=\"http://photo.bitauto.com/serial/" + serialId.ToString() + "/\" target=\"_blank\">更多&gt;&gt;</a></div>");
					htmlCode.Append("</div>");
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
					htmlCode.AppendLine("<div class=\"clear\"></div>");
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

			//htmlCode.AppendLine("<div class=\"blank_box\">");

			//htmlCode.AppendLine("<h2><a href=\"http://photo.bitauto.com/serialmore/" + serialId + "/" + cateId + "/\" target=\"_blank\">" + cateName + "<span class=\"a\">(" + picNum + "张)</span></a></h2>");

			htmlCode.Append("<div class=\"title-con-2\">");
			htmlCode.Append("<h5><a href=\"http://photo.bitauto.com/serialmore/" + serialId + "/" + cateId + "/\" target=\"_blank\">" + cateName + "&gt;&gt;</a><em>" + picNum + "张</em></h5>");
			htmlCode.Append("</div>");

			htmlCode.Append("<div class=\"carpic_list\">");
			htmlCode.AppendLine("<ul>");
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
				htmlCode.AppendLine("<li><a href=\"" + picUlr + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" alt=\"" + serialShowName + picName + "\" width=\"165\" height=\"110\"></a><div class=\"title_pic\"><a href=\"" + picUlr + "\" target=\"_blank\">" + picName + "</a></div></li>");
				loop++;
				if (loop > 4)
				{ break; }
			}
			htmlCode.AppendLine("</ul>");
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
		/// 生成视频
		/// </summary>
		private void MakeVideHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			//取数据
			XmlNodeList videoList = new Car_SerialBll().GetSerialVideo(serialId);
			if (videoList.Count > 0)
			{
				htmlCode.AppendLine("<h3>");
				htmlCode.AppendLine("<span><a href=\"" + baseUrl + "shipin/\" target=\"_blank\">");
				htmlCode.AppendLine(serialShowName + "视频</a></span></h3>");
				htmlCode.AppendLine("<div class=\"more\">");
				htmlCode.AppendLine("<a href=\"" + baseUrl + "shipin/\" target=\"_blank\">更多&gt;&gt;</a></div>");
				htmlCode.AppendLine("<div class=\"pic_album\">");
				htmlCode.AppendLine("<ul class=\"list_pic\">");
				try
				{
					foreach (XmlElement videoNode in videoList)
					{
						string videoTitle = videoNode.SelectSingleNode("title").InnerText;
						videoTitle = StringHelper.RemoveHtmlTag(videoTitle);

						string shortTitle = videoNode.SelectSingleNode("facetitle").InnerText;
						shortTitle = StringHelper.RemoveHtmlTag(shortTitle);

						string imgUrl = videoNode.SelectSingleNode("picture").InnerText;
						if (imgUrl.Trim().Length == 0)
							imgUrl = WebConfig.DefaultVideoPic;
						string filepath = videoNode.SelectSingleNode("filepath").InnerText;

						htmlCode.Append("<li><a href=\"" + filepath + "\" target=\"_blank\" class=\"v_bg\" alt=\"视频播放\"></a><a href=\"" + filepath + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" alt=\"" + serialShowName + videoTitle + "\" width=\"165\" height=\"110\" /></a>");
						if (shortTitle != videoTitle)
							htmlCode.AppendLine("<div class=\"name\"><a href=\"" + filepath + "\" title=\"" + videoTitle + "\" target=\"_blank\">" + shortTitle + "</a></div></li>");
						else
							htmlCode.AppendLine("<div class=\"name\"><a href=\"" + filepath + "\" target=\"_blank\">" + videoTitle + "</a></div></li>");
					}
				}
				catch
				{ }
				htmlCode.AppendLine("</ul>");
				htmlCode.AppendLine("<div class=\"clear\"></div>");
				htmlCode.AppendLine("</div>");
			}

			videosHtml = htmlCode.ToString();
		}


		private void MakeDianpingHtml()
		{
			XmlDocument dpDoc = new Car_SerialBll().GetCarshowSerialDianping(serialId);
			if (dpDoc == null || !dpDoc.HasChildNodes)
			{ return; }
			StringBuilder htmlCode = new StringBuilder();
			int count = ConvertHelper.GetInteger(dpDoc.DocumentElement.GetAttribute("count"));
			int dianpingCount = count;
			htmlCode.AppendLine("<h3 class=\"commu\"><span><span class=\"caption\">" + serialShowName + "-点评精选</span></span><strong><em>" + count + "条</em>|");
			htmlCode.Append("<a href=\"http://koubei.bitauto.com/" + serialSpell + "/koubei/tianjia/\">我要点评</a>");
			htmlCode.Append("|<a href=\"http://i.bitauto.com/FriendMore_c0_s" + serialId + "_p1_sort1_r001.html\">和车主聊聊</a>");
			htmlCode.Append("|<a href=\"http://i.bitauto.com/FriendMore_c0_s" + serialId + "_p1_sort1_r010.html\">和想买的人聊聊</a></strong></h3>");

			StringBuilder tabCode = new StringBuilder();
			StringBuilder conCode = new StringBuilder();
			string moreUrl = baseUrl + "koubei/gengduo/";
			for (int i = 3; i >= 1; i--)
			{
				XmlElement dpNode = (XmlElement)dpDoc.SelectSingleNode("/SerialDianping/Dianping[@type=\"" + i + "\"]");
				if (dpNode == null)
					continue;
				count = ConvertHelper.GetInteger(dpNode.GetAttribute("count"));
				htmlCode.AppendLine("<div class=\"list_li\">");
				switch (i)
				{
					case 1:
						htmlCode.AppendLine("<h4 class=\"cha\">");
						htmlCode.AppendLine("<a href=\"" + moreUrl + "\" target=\"_blank\">差评</a><strong><em>(" + count + "条)</em></strong></h4>");
						break;
					case 2:
						htmlCode.AppendLine("<h4 class=\"zhong\">");
						htmlCode.AppendLine("<a href=\"" + moreUrl + "\" target=\"_blank\">中评</a><strong><em>(" + count + "条)</em></strong></h4>");
						break;
					case 3:
						htmlCode.AppendLine("<h4 class=\"hao\">");
						htmlCode.AppendLine("<a href=\"" + moreUrl + "\" target=\"_blank\">好评</a><strong><em>(" + count + "条)</em></strong></h4>");
						break;
				}

				htmlCode.AppendLine("<ul>");
				int counter = 0;
				foreach (XmlElement ele in dpNode.ChildNodes)
				{
					counter++;
					string title = ele.SelectSingleNode("title").InnerText;
					string url = ele.SelectSingleNode("url").InnerText;
					string shortTitle = title;
					if (StringHelper.GetRealLength(title) > 24)
						shortTitle = StringHelper.SubString(title, 24, false);
					htmlCode.AppendLine("<li><a href=\"" + url + "\" target=\"_blank\" title=\"" + title + "\">" + shortTitle + "</a></li>");
					if (counter >= 7)
						break;
				}
				htmlCode.AppendLine("</ul>");
				htmlCode.AppendLine("<div class=\"more\">");
				htmlCode.AppendLine("<a target=\"_blank\" href=\"" + moreUrl + "\">更多&gt;&gt;</a></div>");
				htmlCode.AppendLine("</div>");

			}
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("<div class=\"more\">");
			htmlCode.AppendLine("<a target=\"_blank\" href=\"" + moreUrl + "\">更多&gt;&gt;</a></div>");

			dianpingHtml = htmlCode.ToString();
		}


		/// <summary>
		/// 生成热门文章
		/// </summary>
		/// <param name="htmlCode"></param>
		private void MakeHotNewsHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			//获取数据
			XmlDocument xmlDoc = new Car_SerialBll().GetSerialHotNews(serialId);
			XmlNodeList newsList = xmlDoc.SelectNodes("NewDataSet/NewsCommentTop");
			htmlCode.AppendLine("<div class=\"line_box hot_article\">");
			htmlCode.AppendLine("<h3><span>" + serialName + "热门文章</span></h3>");
			htmlCode.AppendLine("<div id=\"rank_newcar_box\">");
			htmlCode.AppendLine("<ol class=\"hot_ranking\">");
			int counter = 0;
			foreach (XmlElement newsNode in newsList)
			{
				counter++;
				string newsTitle = newsNode.SelectSingleNode("NewsTitle").InnerText;
				//过滤Html标签
				newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
				string shortNewsTitle = StringHelper.SubString(newsTitle, 26, true);
				string filePath = newsNode.SelectSingleNode("NewsUrl").InnerText;
				string pubTime = newsNode.SelectSingleNode("Time").InnerText;
				pubTime = Convert.ToDateTime(pubTime).ToString("MM-dd");
				if (shortNewsTitle != newsTitle)
					htmlCode.AppendLine("<li><a href=\"" + filePath + "\" title=\"" + newsTitle + "\" target=\"_blank\">" + shortNewsTitle + "</a></li>");
				else
					htmlCode.AppendLine("<li><a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a></li>");
				if (counter >= 5)
					break;
			}
			htmlCode.AppendLine("</ol></div>");
			htmlCode.AppendLine("</div>");
			hotNewsHtml = htmlCode.ToString();
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
					htmlCode.Append("<li>");
					htmlCode.AppendFormat("<a target=\"_blank\" href=\"/{0}/\"><img src=\"{1}\" width=\"90\" height=\"60\"></a>",
						sts.ToCsAllSpell.ToString().ToLower(),
						 sts.ToCsPic.ToString());
					if (shortName != csName)
						htmlCode.AppendFormat("<p><a target=\"_blank\" href=\"/{0}/\" title=\"{1}\">{2}</a></p>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName, shortName);
					else
						htmlCode.AppendFormat("<p><a target=\"_blank\" href=\"/{0}/\">{1}</a></p>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName);
					htmlCode.AppendFormat("<p><span>{0}</span></p>", StringHelper.SubString(sts.ToCsPriceRange.ToString(), 14, false));
					htmlCode.AppendFormat("</li>");
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
			Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = new Car_SerialBll().GetSerialCityCompareList(serialId, HttpContext.Current);
			List<string> htmlList = new List<string>();
			string compareBaseUrl = "/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?count=3&csids=" + serialId + ",";
			string serialCompareForPkUrl = "/duibi/" + serialId + "-";
			if (carSerialBaseList != null && carSerialBaseList.Count > 0 && carSerialBaseList.ContainsKey("全国"))
			{
				List<Car_SerialBaseEntity> serialCompareList = carSerialBaseList["全国"];
				//htmlList.Add("<div class=\"line_box h160\" id=\"serialHotCompareList\">");
				//htmlList.Add("<h3><span>网友都用它和谁比</span></h3>");
				//htmlList.Add("<div class=\"more\"><a href=\"/chexingduibi/\">车型对比&gt;&gt;</a></div>");
				//htmlList.Add("<div class=\"ranking_list\" id=\"rank_model_box\">");
				htmlList.Add("<ul class=\"text-list\">");

				for (int i = 0; i < serialCompareList.Count; i++)
				{
					Car_SerialBaseEntity carSerial = serialCompareList[i];
					htmlList.Add(string.Format("<li><a href=\"{2}\" target=\"_blank\">{0}<em class=\"vs\">VS</em>{1}</a></li>",
						BitAuto.Utils.StringHelper.SubString(serialShowName, 10, false),
						carSerial.SerialShowName.Trim(),
						  serialCompareForPkUrl + carSerial.SerialId + "/"));
				}

				htmlList.Add("</ul>");
			}

			hotSerialCompareHtml = String.Concat(htmlList.ToArray());
		}

		private void MakeSerialIntensionHtml()
		{
			intensionHtml = "";
			Dictionary<int, List<XmlElement>> intensionDic = new Car_SerialBll().GetSerialIntensionDic();
			if (!intensionDic.ContainsKey(serialId))
				return;

			StringBuilder htmlCode = new StringBuilder();
			int counter = 0;
			foreach (XmlElement userNode in intensionDic[serialId])
			{
				counter++;
				string userName = userNode.SelectSingleNode("name").InnerText;
				string shortName = userName;
				if (StringHelper.GetRealLength(userName) > 8)
					shortName = StringHelper.SubString(userName, 8, true);
				userName = StringHelper.RemoveHtmlTag(userName);
				shortName = StringHelper.RemoveHtmlTag(shortName);
				string userUrl = userNode.SelectSingleNode("url").InnerText;
				htmlCode.AppendLine("<li><a href=\"" + userUrl + "\" title=\"" + userName + "\" target=\"_blank\">" + shortName + "</a></li>");
				if (counter >= 9)
					break;
			}
			intensionHtml = htmlCode.ToString();
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
				string priceRang = base.GetSerialPriceRangeByID(entity.SerialId);
				if (entity.SaleState == "待销")
				{
					IsExitsUrl = false;
					priceRang = "未上市";
				}
				else if (priceRang.Trim().Length == 0)
				{
					IsExitsUrl = false;
					priceRang = "暂无报价";
				}
				if (IsExitsUrl)
				{
					priceRang = string.Format("<a href='http://price.bitauto.com/brand.aspx?newbrandid={0}'>{1}</a>", entity.SerialId, priceRang);
				}
				string tempCsSeoName = string.IsNullOrEmpty(entity.CS_SeoName) ? entity.ShowName : entity.CS_SeoName;
				index++;
				contentBuilder.AppendFormat("<li>{0}<span class=\"dao\">{1}</span></li>"
					, string.Format(serialUrl, entity.CS_AllSpell, tempCsSeoName), priceRang
					 );
			}

			StringBuilder brandOtherSerial = new StringBuilder(string.Empty);
			if (contentBuilder.Length > 0)
			{
				brandOtherSerial.Append("<div class=\"side_title\">");
				brandOtherSerial.AppendFormat("<h4><a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}其他车型</a></h4>",
					cse.Cb_AllSpell, cse.Cb_Name);
				brandOtherSerial.Append("</div>");

				brandOtherSerial.Append("<ul class=\"text-list\">");

				brandOtherSerial.Append(contentBuilder.ToString());

				brandOtherSerial.Append("</ul>");
			}

			return brandOtherSerial.ToString();
		}

		/// <summary>
		/// 取子品牌相关用户
		/// </summary>
		private void GetUserBlockByCarSerialId()
		{
			StringBuilder sbUserBlock = new StringBuilder();
			// 计划购买
			DataTable dtWant = base.GetUserByCarSerialId(sic.CsID, 2, 3);
			if (dtWant != null && dtWant.Rows.Count > 0)
			{
				sbUserBlock.AppendLine("<div class=\"line_box zh_driver\">");
				sbUserBlock.AppendLine("<h3><span>和想买这款车的人聊聊</span></h3>");
				sbUserBlock.AppendLine("<div class=\"index_friend_r_l\">");
				sbUserBlock.AppendLine("<ul>");
				for (int i = 0; i < dtWant.Rows.Count; i++)
				{
					sbUserBlock.AppendLine("<li><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtWant.Rows[i]["userId"].ToString() + "/\" title=\"\">");
					sbUserBlock.AppendLine("<img height=\"60\" width=\"60\" src=\"" + dtWant.Rows[i]["userAvatar"].ToString() + "\" alt=\"\"></a>");
					sbUserBlock.AppendLine("<strong><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtWant.Rows[i]["userId"].ToString() + "/\">" + dtWant.Rows[i]["username"].ToString() + "</a></strong>");
					sbUserBlock.AppendLine("<p><a class=\"add_friend\" href=\"#\" onclick=\"javascript:AjaxAddFriend.show(" + dtWant.Rows[i]["userId"].ToString() + ", " + sic.CsID.ToString() + ");return false;\">加为好友</a></p></li>");
				}
				sbUserBlock.AppendLine("</ul>");
				sbUserBlock.AppendLine("</div><div class=\"clear\"> </div>");
				sbUserBlock.AppendLine("<div class=\"more\"><a target=\"_blank\" href=\"http://i.bitauto.com/FriendMore_c0_s" + sic.CsID.ToString() + "_p1_sort1_r010.html\">更多&gt;&gt;</a></div>");
				sbUserBlock.AppendLine("</div>");
			}
			// 车主
			DataTable dtOwner = base.GetUserByCarSerialId(sic.CsID, 3, 3);
			if (dtOwner != null && dtOwner.Rows.Count > 0)
			{
				sbUserBlock.AppendLine("<div class=\"line_box zh_driver\">");
				sbUserBlock.AppendLine("<h3><span>和车主聊聊</span></h3>");
				sbUserBlock.AppendLine("<div class=\"index_friend_r_l\">");
				sbUserBlock.AppendLine("<ul>");
				for (int i = 0; i < dtOwner.Rows.Count; i++)
				{
					sbUserBlock.AppendLine("<li><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtOwner.Rows[i]["userId"].ToString() + "/\" title=\"\">");
					sbUserBlock.AppendLine("<img height=\"60\" width=\"60\" src=\"" + dtOwner.Rows[i]["userAvatar"].ToString() + "\" alt=\"\"></a>");
					sbUserBlock.AppendLine("<strong><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtOwner.Rows[i]["userId"].ToString() + "/\">" + dtOwner.Rows[i]["username"].ToString() + "</a></strong>");
					sbUserBlock.AppendLine("<p><a class=\"add_friend\" href=\"#\" onclick=\"AjaxAddFriend.show(" + dtOwner.Rows[i]["userId"].ToString() + ", " + sic.CsID.ToString() + ",3);return false;\">加为好友</a></p></li>");
				}
				sbUserBlock.AppendLine("</ul>");
				sbUserBlock.AppendLine("</div><div class=\"clear\"> </div>");
				sbUserBlock.AppendLine("<div class=\"more\"><a target=\"_blank\" href=\"http://i.bitauto.com/FriendMore_c0_s" + sic.CsID.ToString() + "_p1_sort1_r001.html\">更多&gt;&gt;</a></div>");
				sbUserBlock.AppendLine("</div>");
			}
			UserBlock = sbUserBlock.ToString();
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

					_sb.Append("<div class=\"title-con-2\">");
					_sb.Append("<h5><a target=\"_blank\" href=\"" + cateURL + "\">" + cateName + "&gt;&gt;</a><em>" + picNum + "张</em></h5>");
					_sb.Append("</div>");


					_sb.Append("<div class=\"carpic_list\">");
					_sb.AppendLine("<ul>");
					foreach (XmlNode xn in xnl[0])
					{
						string picUlr = xn.Attributes["Link"].Value.ToString();
						string imgUrl = xn.Attributes["ImageUrl"].Value.ToString();
						string picName = xn.Attributes["ImageName"].Value.ToString();
						_sb.AppendLine("<li><a target=\"_blank\" href=\"" + picUlr + "\"><img src=\"" + imgUrl + "\" width=\"165\" height=\"110\"></a><div class=\"title_pic\"><a target=\"_blank\" href=\"" + picUlr + "\">" + picName + "</a></div></li>");
					}
					_sb.AppendLine("</ul>");
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

						_sb.Append("<div class=\"title-con-2\">");
						_sb.Append("<h5><a target=\"_blank\" href=\"" + cateURL + "\">" + cateName + "&gt;&gt;</a><em>" + picNum + "张</em></h5>");
						_sb.Append("</div>");

						_sb.Append("<div class=\"carpic_list\">");
						_sb.AppendLine("<ul>");
						foreach (XmlNode xn in xnCate.ChildNodes)
						{
							string picUlr = xn.Attributes["Link"].Value.ToString();
							string imgUrl = xn.Attributes["ImageUrl"].Value.ToString();
							string picName = xn.Attributes["ImageName"].Value.ToString();
							_sb.AppendLine("<li><a target=\"_blank\" href=\"" + picUlr + "\"><img src=\"" + imgUrl + "\" width=\"165\" height=\"110\"></a><div class=\"title_pic\"><a target=\"_blank\" href=\"" + picUlr + "\">" + picName + "</a></div></li>");
						}
						_sb.AppendLine("</ul>");
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
							if (sbMoreLink.Length > 0)
							{
								sbMoreLink.Append(" | ");
							}
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
				nextSeePingceHtml = "<li><a href=\"/" + serialSpell + "/pingce/\" target=\"_self\">" + serialShowName + "车型详解</a></li>";
			//if (newsBll.IsSerialNews(serialId, 0, CarNewsType.xinwen))
			//    nextSeeXinwenHtml = "<li><a href=\"/" + serialSpell + "/xinwen/\" target=\"_self\">" + serialShowName + "<span>新闻</span></a></li>";
			if (newsBll.IsSerialNews(serialId, 0, CarNewsType.daogou))
				nextSeeDaogouHtml = "<li><a href=\"/" + serialSpell + "/daogou/\" target=\"_self\">" + serialShowName + "导购</a></li>";

		}
	}
}