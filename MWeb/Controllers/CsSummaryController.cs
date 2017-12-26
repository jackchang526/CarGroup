using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;

namespace MWeb.Controllers
{
    public class CsSummaryController : Controller
    {
        // 车系综述页
        // GET: /CsSummary/
        private readonly Car_BasicBll carBLL;
        private readonly CommonHtmlBll commonhtmlBLL;
        private readonly Car_SerialBll serialBLL;
        private CarNewsBll newsBLL;
        private PageBase pageBase;

        private int serialId = 0;
        protected SerialEntity serialEntity; //车系实体
        protected EnumCollection.SerialInfoCard serialInfoCard; //车系名片
        private List<CarInfoForSerialSummaryEntity> serialCarList = null; //车款列表
        private bool isElectrombile = false;//是否是电动车
        protected int pageCount = 0; //车款页数
        private const int pageSize = 7;//每页车款的显示数量
        protected int maxPv = 0;//车款的最大pv数量
        protected string nearestYear = string.Empty;//所有停售车款里的最近年份
                                                    //protected int yearCount;
		//焦点图测试的10个车系
		//private List<int> TestCsIds = new List<int>() { 1879, 2064, 2406, 2408, 2593, 2713, 2714, 2750, 3152, 4502 };

        /// <summary>
        /// 车系为电动车的续航里程区间
        /// </summary>
        protected string mileageRange = string.Empty;
        
        public CsSummaryController()
        {
            carBLL = new Car_BasicBll();
            commonhtmlBLL = new CommonHtmlBll();
            serialBLL = new Car_SerialBll();
            newsBLL = new CarNewsBll();
            pageBase = new PageBase();
        }

        [OutputCache(Duration = 600, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int id)
        {
            serialId = id;
			InitParam();//处理其他参数
            InitData(); //初始化数据
			if (serialEntity == null || serialEntity.Id <= 0)
			{
				Response.Redirect("/error", true);
				return new EmptyResult();
			}
            InitTitle();  //初始化title信息
            InitSerialData(); //车系信息
			
            GetCarList(); //车款列表
            GetChanDiName();//产地
            GetSerialColors(); //车系颜色
            GetSerialBlockHtml();//静态块
            InitNewsData();//新闻
            GetVideo();//视频
            MakeForumNewsHtml();//论坛
            //MakeSerialToSerialHtml();//看了还看
            GetSerialHotCompareCars();//找相似
            //GetVrUrl();//获取vr地址
            GetBaoZhiLv();//保值率
            /*
            bool isTestCs = TestCsIds.Contains(serialId);
			if (isTestCs)
			{
				MakeSerialInfoHtmlV2(); //焦点图
				return View("~/Views/CsSummary/IndexFocus.cshtml");
			}*/
			
			MakeSerialInfoHtml(); //焦点图

            #region 综述页新车上市提示 20170920

            //GetTab();
            ViewData["showText"] = serialBLL.GetNewSerialIntoMarketText(serialId, true);

            #endregion

            return View();
        }

		/// <summary>
		/// 其他参数
		/// </summary>
		private void InitParam()
		{
			string wtmcid = Request["WT.mc_id"];
			ViewData["wtmcid"] = wtmcid;
		}

        /// <summary>
        /// 数据初始化
        /// </summary>
        private void InitData()
        {
			if (serialId < 1)
			{
				//Response.Redirect("/error", true);
				return;
			}

            serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
			if (serialEntity == null || serialEntity.Id <= 0)
			{
				//Response.Redirect("/error", true);
				return;
			}

            serialInfoCard = new Car_SerialBll().GetSerialInfoCard(serialId);

            ViewData["HeadHtml"] = pageBase.GetCommonNavigation("MCsSummary", serialId);


            ViewData["SerialEntity"] = serialEntity;
            ViewData["SerialInfoCard"] = serialInfoCard;
        }

        private void InitSerialData()
        {
            string serialPrice = string.Empty;
            if (serialInfoCard.CsSaleState == "停销")
            {
                serialPrice = "停售";
            }
            else if (serialInfoCard.CsPriceRange == null || serialInfoCard.CsPriceRange.Length == 0 || serialInfoCard.CsPriceRange == "未上市")
                serialPrice = "暂无";
            else
                serialPrice = serialInfoCard.CsPriceRange.Replace("万-", "-");

            ViewData["serialPrice"] = serialPrice;

            Dictionary<int, string> dictUCarPriceRange = serialBLL.GetUCarSerialPrice();
            string uCarPriceRange = string.Empty;
            if (dictUCarPriceRange.ContainsKey(serialId))
            {
                uCarPriceRange = dictUCarPriceRange[serialId];
            }
            if (string.IsNullOrWhiteSpace(uCarPriceRange))
            {
                uCarPriceRange = "暂无报价";
            }

            ViewData["uCarPriceRange"] = uCarPriceRange;

            ViewBag.serialTotalPV = serialBLL.GetSerialTotalPVWithCache(serialId);
            serialCarList = carBLL.GetCarInfoForSerialSummaryBySerialId(serialId);
            var fuelTypeList = serialCarList.FindAll(p => p.SaleState == "在销").Where(p => p.Oil_FuelType != "")
                .GroupBy(p => p.Oil_FuelType)
                .Select(g => g.Key).ToList();
            isElectrombile = fuelTypeList.Count == 1 && fuelTypeList[0] == "纯电";
            ViewData["isElectrombile"] = isElectrombile;

            string serialSummaryFuelCost = serialInfoCard.CsSummaryFuelCost;
            //string serialGuestFuelCost = serialInfoCard.CsGuestFuelCost;
            string serialFuelCost = string.Empty;
            if (!string.IsNullOrEmpty(serialSummaryFuelCost) && serialSummaryFuelCost != "0-0L")
            {
                serialFuelCost = serialSummaryFuelCost; //+ "(综合)";
            }
            ViewData["serialFuelCost"] = serialFuelCost;

            Dictionary<int, string> serialPicUrl = pageBase.GetAllSerialPicURL(true);//白底封面图
            string whitePicUrl = WebConfig.DefaultCarPic;
            if (serialPicUrl.ContainsKey(serialId))
            {
                whitePicUrl = serialPicUrl[serialId].Replace("_2.","_4.");
            }
            ViewData["WhitePicUrl"] = whitePicUrl;
        }

        /// <summary>
        ///     获取车款列表
        ///     songcl 2015-07-02
        /// </summary>
        private void GetCarList()
        {
            if (serialCarList.Count == 0)
            {
                return;
            }
            serialCarList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
            var yearList = new List<string>();//年款
            if (serialEntity.SaleState == "待销")
            {
                //待销不列年款
            }
            else if (serialEntity.SaleState == "停销")
            {
                //取车系为停销状态的停销年款
                yearList =
                    serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "停销")
                        .Select(p => p.CarYear)
                        .Distinct()
                        .ToList();
            }
            else
            {
                //取车系为在销状态的在销年款
                yearList =
                    serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "在销")
                        .Select(p => p.CarYear)
                        .Distinct()
                        .ToList();
            }

            yearList.Sort(NodeCompare.CompareStringDesc);

            //在售和停售数据集合
            //List<CarInfoForSerialSummaryEntity> salingAndNoSaleList = _serialCarList.FindAll(p => p.SaleState != "待销");

            //停售数据集合
            List<CarInfoForSerialSummaryEntity> carinfoNoSaleList = serialCarList.FindAll(p => p.SaleState == "停销");

            //在售数据集合
            List<CarInfoForSerialSummaryEntity> carinfoSaleList = serialCarList.FindAll(p => p.SaleState == "在销");

            //待售（未上市）数据集合
            List<CarInfoForSerialSummaryEntity> carinfoWaitSaleList = serialCarList.FindAll(p => p.SaleState == "待销");


            var htmlCode = new StringBuilder();
            htmlCode.Append("<div class=\"second-tags-scroll-box mgt10\">");
            htmlCode.Append("<div class=\"pd15\">");
            htmlCode.Append("<div class='second-tags-scroll mgt12'data-channelid=\"27.23.727\">");
            htmlCode.Append("<ul id='yeartag'>");

            var targetList = new List<string>();//当前车系状态下的所有tab标签

            var htmlCondition = new StringBuilder();
            if (serialEntity.SaleState == "在销")
            {
                htmlCondition.Append("<div class=\"sort sort3 sort-bg-white sort-pop tags-list\"><ul>");
                if (carinfoSaleList.Count > 0)
                {
                    targetList.Add("全部在售");
                }
                targetList.AddRange(yearList);
                if (carinfoWaitSaleList.Count > 0)
                {
                    targetList.Add("未上市");
                }
                if (carinfoNoSaleList.Count > 0)
                {
                    targetList.Add("停售");
                    if (yearList.Count > 0)
                    {
                        var stopCarYears = new List<string>();
                        stopCarYears = serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "停销")
                                .Select(p => p.CarYear)
                                .Distinct()
                                .ToList();

                        if (stopCarYears.Count > 0)
                        {
                            stopCarYears.Sort(NodeCompare.CompareStringDesc);
                            nearestYear = stopCarYears[0];
                        }
                        htmlCondition.Append("<li data-action=\"stopyear\" class=\"m-btn current\" style=\"display:none\" data-channelid=\"27.23.510\"><a><span>" + nearestYear + "款</span><i></i></a></li>");
                    }
                }
                htmlCondition.Append("<li data-action=\"level\" class=\"m-btn\" data-channelid=\"27.23.511\"><a><span>排量</span><i></i></a></li>");
                htmlCondition.Append("<li data-action=\"bodyform\" class=\"m-btn\" data-channelid=\"27.23.512\"><a><span>变速箱</span><i></i></a></li>");
                htmlCondition.Append("</ul><div class=\"clear\"></div></div>");
            }
            else if (serialEntity.SaleState == "待销")
            {
                targetList.Add("未上市");
            }
            else
            {
                targetList.AddRange(yearList);
                htmlCondition.Append("<div class=\"sort sort3 sort-bg-white sort-pop tags-list\"><ul>");
                htmlCondition.Append("<li data-action=\"level\" class=\"m-btn\" data-channelid=\"27.23.511\"><a><span>排量</span><i></i></a></li>");
                htmlCondition.Append("<li data-action=\"bodyform\" class=\"m-btn\" data-channelid=\"27.23.512\"><a><span>变速箱</span><i></i></a></li>");
                htmlCondition.Append("</ul><div class=\"clear\"></div></div>");
            }
            int flag = 0;
            foreach (string curtab in targetList)
            {
                switch (curtab)
                {
                    case "全部在售":
                        htmlCode.Append("<li id='all' class='current'><a><span>" + curtab + "</span></a></li>");
                        break;
                    case "未上市":
                        htmlCode.AppendFormat("<li id='unlisted' {0}><a><span>" + curtab + "</span></a></li>",
                            targetList.Contains("全部在售") || targetList.Count > 2 ? "" : "class='current'");
                        break;
                    case "停售":
                        htmlCode.AppendFormat("<li id='nosalelist'><a><span>" + curtab + "</span></a></li>",
                            targetList.Contains("全部在售") || targetList.Count > 2 ? "" : "class='current'");
                        break;
                    default:
                        htmlCode.AppendFormat("<li id='{0}' {1}><a><span>{0}款</span></a></li>", curtab,
                            targetList.Contains("全部在售") || flag > 0 ? "" : "class='current'");
                        flag++;
                        break;
                }
            }
            htmlCode.Append("</ul>");
            htmlCode.Append("</div>");
            htmlCode.Append("</div>");
            htmlCode.Append("<div class=\"right-mask\"></div>");
            htmlCode.Append("</div>");
            htmlCode.Append(htmlCondition.ToString());

            GetCarHtml(targetList, htmlCode);

            //_carList = htmlCode.ToString();
            ViewData["YearCount"] = yearList.Count;
            ViewData["CarListHtml"] = htmlCode.ToString();
        }

        private void GetCarHtml(IEnumerable<string> yearList, StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat("<div id='carList{0}'>", serialId);
            int counter = 0;
            int minMileage = 0;
            int maxMileage = 0;
            bool flag = true;
            foreach (string item in yearList)
            {
                stringBuilder.AppendFormat("<div id='yearDiv{0}' {1} class='sum-car-type-box' pagecount='{2}'>",
                    counter,
                    counter > 0 ? "style='display:none;'" : "", pageCount);
                if (flag)
                {
                    #region 数据筛选

                    string year = item;
                    List<CarInfoForSerialSummaryEntity> currentYearCarList;
                    IEnumerable<IGrouping<object, CarInfoForSerialSummaryEntity>> querySale;

                    maxPv = serialCarList.Max(m => m.CarPV);
                    switch (year)
                    {
                        case "全部在售":
                            currentYearCarList = serialCarList.FindAll(p => p.SaleState == "在销");

                            querySale = currentYearCarList.GroupBy(
                                p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower },
                                p => p);
                            break;
                        case "未上市":
                            currentYearCarList = serialCarList.FindAll(p => p.SaleState == "待销");

                            querySale = currentYearCarList.GroupBy(
                                p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower },
                                p => p);
                            break;
                        case "停售":
                            //停售tab栏默认只显示当前所有停售车款里的最新年份
                            var stopCarYears = new List<string>();
                            stopCarYears = serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "停销")
                                    .Select(p => p.CarYear)
                                    .Distinct()
                                    .ToList();
                            if (stopCarYears.Count > 0)
                            {
                                stopCarYears.Sort(NodeCompare.CompareStringDesc);
                                nearestYear = stopCarYears[0];
                            }
                            currentYearCarList = serialCarList.FindAll(p => p.SaleState == "停销" && p.CarYear == nearestYear);

                            querySale = currentYearCarList.GroupBy(
                                p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower },
                                p => p);
                            break;
                        default:
                            if (serialEntity.SaleState == "停销")
                            {
                                currentYearCarList = serialCarList.FindAll(p => p.SaleState == "停销" && p.CarYear == year);
                                //取车系为停销状态的车款列表
                            }
                            else
                            {
                                currentYearCarList = serialCarList.FindAll(p => p.SaleState == "在销" && p.CarYear == year);
                                //取车系为在销状态的车款列表
                            }
                            querySale = currentYearCarList.GroupBy(
                                p =>
                                    new
                                    {
                                        p.Engine_Exhaust,
                                        p.Engine_InhaleType,
                                        p.Engine_AddPressType,
                                        p.Engine_MaxPower,
                                        p.Electric_Peakpower
                                    },
                                p => p);
                            break;
                    }

                    int carCount = currentYearCarList.Count;
                    //querySale = currentYearCarList.Take(pageSize).GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower }, p => p); //取第一页
                    pageCount = carCount / pageSize + (carCount % pageSize == 0 ? 0 : 1);

                    //start add by sk 2014-09-03 候姐 整组 停产 把整组移到最底 且保持前 排序规则 2017-05-22 新加平行进口车 组，放在停产组下边
                    var listGroupNew = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
                    var listGroupOff = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
					var listGroupImport = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();

					var importGroup = currentYearCarList.Take(pageSize).GroupBy(p => new { p.IsImport }, p => p); //取第一页
					foreach (var info in importGroup)
                    {
						var key = CommonFunction.Cast(info.Key, new { IsImport = 0 });
						if (key.IsImport == 1)
						{
							listGroupImport.Add(info);
						}
						else
						{
							var qs = info.ToList().GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower }, p => p);
							foreach (IGrouping<object, CarInfoForSerialSummaryEntity> subInfo in qs)
							{
								CarInfoForSerialSummaryEntity isStopState = subInfo.FirstOrDefault(p => p.ProduceState != "停产");
								if (isStopState != null)
									listGroupNew.Add(subInfo);
								else
									listGroupOff.Add(subInfo);
							}
						}
                    }
                    listGroupNew.AddRange(listGroupOff);
					listGroupNew.AddRange(listGroupImport);

                    #endregion
					int groupIndex = 0;
					foreach (IGrouping<object, CarInfoForSerialSummaryEntity> group in listGroupNew)
                    {
                        #region 基础信息准备
						var key =
							new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0, Electric_Peakpower = 0 } ;
						string strMaxPowerAndInhaleType = string.Empty;                       
						if (groupIndex == listGroupNew.Count - 1 && listGroupImport.Any())
						{
							strMaxPowerAndInhaleType = "平行进口车";
						}
						else
						{
							key = CommonFunction.Cast(group.Key,
						    new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0, Electric_Peakpower = 0 });
							string maxPower = key.Engine_MaxPower == 9999 ? "" : key.Engine_MaxPower + "kW";
							string inhaleType = key.Engine_InhaleType;
							if (!string.IsNullOrEmpty(maxPower) || !string.IsNullOrEmpty(inhaleType))
							{
								if (inhaleType == "增压")
								{
									inhaleType = string.IsNullOrEmpty(key.Engine_AddPressType)
										? inhaleType
										: key.Engine_AddPressType;
								}
								if (key.Electric_Peakpower > 0)
								{
									maxPower = string.Format("发动机：{0}，发电机：{1}", maxPower, key.Electric_Peakpower + "kW");
								}
								strMaxPowerAndInhaleType = string.Format("{0}{1}", maxPower, " " + inhaleType);
							}
						}
						groupIndex++;
                        #endregion

                        stringBuilder.Append("<div class='tt-small'>");
                        stringBuilder.AppendFormat("<span>{0}{1}{2}</span>", key.Engine_Exhaust.Replace("L", "升").Replace("T", "升"),
                            (string.IsNullOrEmpty(key.Engine_Exhaust) || string.IsNullOrEmpty(strMaxPowerAndInhaleType))
                                ? ""
                                : "/", strMaxPowerAndInhaleType);
                        stringBuilder.Append("</div>");

                        stringBuilder.Append("<div class='car-card'>");
                        stringBuilder.Append("<ul>");

                        #region

                        List<CarInfoForSerialSummaryEntity> carGroupList = group.ToList(); //分组后的集合


                        foreach (CarInfoForSerialSummaryEntity carInfo in carGroupList)
                        {
                            //if (num > 9)
                            //    break;

                            string carFullName = "";
                            carFullName = carInfo.CarName;
                            if (carInfo.CarName.StartsWith(serialEntity.ShowName))
                            {
                                carFullName = carInfo.CarName.Substring(serialEntity.ShowName.Length);
                            }
                            if (year == "全部在售" || year == "未上市")
                            {
                                /////////////////////////////
                            }
                            carFullName = carInfo.CarYear + "款 " + carFullName;
                            string stopPrd = "";
                            if (carInfo.ProduceState == "停产")
                                stopPrd = "<em>停产" + (serialEntity.SaleState == "停销" ? "停售" : "在售") + "</em>";
                            if (carInfo.ProduceState == "停产" && carInfo.SaleState == "停销")
                                stopPrd = "<em>停售</em>";

                            string carMinPrice;
                            string carPriceRange = carInfo.CarPriceRange.Trim();
                            if (carInfo.SaleState == "待销")//顾晓 确认的逻辑 （待销的车款没有价格，全部显示未上市） 2015-07-09
                            {
                                carMinPrice = "未上市";
                            }
                            else if (carInfo.CarPriceRange.Trim().Length == 0)
                            {
                                carMinPrice = "暂无报价";
                            }
                            else
                            {
                                if (carPriceRange.IndexOf('-') != -1)
                                    carMinPrice = carPriceRange.Substring(0, carPriceRange.IndexOf('-')); //+ "万"
                                else
                                    carMinPrice = carPriceRange;
                            }

                            Dictionary<int, string> dictCarParams = carBLL.GetCarAllParamByCarID(carInfo.CarID);

                            #region 纯电动车续航里程

                            if (isElectrombile)
                            {
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
                                if (maxMileage > 0)
                                {
                                    mileageRange = minMileage == maxMileage
                                        ? string.Format("{0}公里", minMileage)
                                        : string.Format("{0}-{1}公里", minMileage, maxMileage);
                                }
                                ViewData["mileageRange"] = mileageRange;
                            }

                            #endregion


                            // 档位个数
                            ////string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" &&
                            //                         dictCarParams[724] != "待查")
                            //    ? dictCarParams[724] + "挡"
                            //    : "";

                            //平行进口车标签
                            //string parallelImport = "";
                            //if (dictCarParams.ContainsKey(382) && dictCarParams[382] == "平行进口")
                            //{
                            //	parallelImport = "<em>平行进口</em>";
                            //}
                            string transmissionType = carBLL.GetCarTransmissionType(dictCarParams.ContainsKey(724) ? dictCarParams[724] : string.Empty, carInfo.TransmissionType);

                            stringBuilder.Append("<li>");

                            stringBuilder.AppendFormat(
                                "<a  id='carlist_" + carInfo.CarID + "' class='car-info' href='{0}' data-channelid=\"27.23.915\">",
                                 "/" + serialEntity.AllSpell + "/m" + carInfo.CarID + "/");

                            //新车上市 即将上市 状态
                            string marketflag = serialBLL.GetCarMarketText(carInfo.CarID, carInfo.SaleState, carInfo.MarketDateTime, carInfo.ReferPrice);//GetMarketFlag(carInfo);
                            if (!string.IsNullOrEmpty(marketflag))
                            {
                                marketflag = string.Format("<em class=\"the-new\">{0}</em>", marketflag);
                            }
                            stringBuilder.AppendFormat("<h2>{0}{1}</h2>", carFullName, marketflag);
                            stringBuilder.AppendFormat("<dl><dt>{0}</dt></dl>", carMinPrice);

                            stringBuilder.Append("<div class=\"car-info-bottom\">");//第二行开始


                            //add date :2016-2-3  添加热度
                            int percent = 0;
                            if (maxPv > 0)
                            { percent = (int)Math.Round((double)carInfo.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero); }
                            //减税 购置税
                            string strTravelTax = string.Empty;
                            double dEx = 0.0;
                            Double.TryParse(carInfo.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
                            if (carInfo.SaleState == "在销")
                            {
                                if (dictCarParams.ContainsKey(987) && (dictCarParams[987] == "第1批" || dictCarParams[987] == "第2批" || dictCarParams[987] == "第3批" || dictCarParams[987] == "第4批" || dictCarParams[987] == "第5批" || dictCarParams[987] == "第6批") && dictCarParams.ContainsKey(986))
                                {
                                    if (dictCarParams[986] == "免征")
                                    {
                                        strTravelTax = "<em>免税</em>";
                                    }
                                    else
                                    {
                                        strTravelTax = "<em>减税</em>";
                                    }
                                }
                                else if (dEx > 0 && dEx <= 1.6)
                                {
                                    strTravelTax = "<em>减税</em>";
                                }
                            }
                            stringBuilder.AppendFormat("<span>{0}</span>", transmissionType);
                            stringBuilder.AppendFormat("<div class=\"gzd-box\" style=\"\"><div class=\"tit-box\">热度</div><span class=\"gz-sty\"><i data-pv=\"{0}\" style=\"width:{0}%\"></i></span></div>", percent);
                            stringBuilder.AppendFormat("{0}{1}", strTravelTax, stopPrd);
                            stringBuilder.AppendFormat("<b>指导价:{0}</b>", carInfo.ReferPrice.Trim().Length == 0 ? "暂无" : carInfo.ReferPrice.Trim() + "万");
                            stringBuilder.Append("</div>");//第二行结束
                            stringBuilder.Append("</a>");

                            bool maiBtnFlag = false;
                            //if (year != "未上市" && year != "停售" && carInfo.SaleState != "待销" && carInfo.SaleState != "停销")
                            //{ maiBtnFlag = true; }
                            string ulStyle = "car-btn";
                            if (!maiBtnFlag)
                            {
                                ulStyle = "car-btn car-btn-three";
                            }
                            stringBuilder.Append("<ul class='" + ulStyle + "'>");
                            stringBuilder.AppendFormat(
                                 "<li class='btn-duibi'><a id=\"car-compare-{0}\" href=\"#compare\"  data-action=\"car\" data-id=\"{0}\" data-name=\"{1} {2}\" data-channelid=\"27.23.910\"><span>对比</span></a></li>",
                                carInfo.CarID, serialEntity.ShowName, carFullName);
                            stringBuilder.AppendFormat(
                                "<li class='btn-calculator'><a id = \"car_filter_id_{0}_{1}\" href='/gouchejisuanqi/?carID={0}' data-channelid=\"27.23.911\"><span>计算器</span></a></li>",
                                carInfo.CarID, counter);
                            if (maiBtnFlag)
                            {
                                stringBuilder.AppendFormat("<li><a data-car=\"{0}\" href='javascript:void(0)' class=\"btn-mmm\"  data-action=\"mmm\" data-channelid=\"27.23.1321\">买买买</a></li>", carInfo.CarID);
                            }
                            if (carInfo.SaleState != "停销")
                            {
                                //add by gux 20170425
                                string wtQuery = new int[] { 4123, 4881, 2608, 1574, 2573, 3987, 2032, 1905, 4847, 1798 }.Contains(serialId) ? "&WT.mc_id=nbclx" : string.Empty;

                                stringBuilder.AppendFormat(
                                "<li class=\"btn-xundijia btn-one-color\"><a id =\"car_filterzuidi_id_{0}_{1}\" href=\"http://price.m.yiche.com/zuidijia/nc{0}/?leads_source=m002008" + wtQuery + "\" data-channelid=\"27.23.912\">询底价</a></li>",
                                carInfo.CarID, counter);
                            }
                            else
                            {
                                stringBuilder.AppendFormat("<li class='btn-org'><a href='http://m.taoche.com/all/?carid={0}&WT.mc_id=yichezswap&leads_source=m002015' data-channelid=\"27.23.913\">买二手车</a></li>", carInfo.CarID);
                            }
                            stringBuilder.Append("</ul>");

                            stringBuilder.Append("</li>");
                            //num++;
                        }

                        #endregion

                        stringBuilder.Append("</ul>");
                        stringBuilder.Append("</div>");
                    }
                    if (pageCount > 1)
                    {
                        stringBuilder.AppendFormat("<a id='btnLoadNext{0}' page='2'  class='btn-more btn-add-more b-shadow' href='javascript:void(0);'><i>加载更多</i></a>", counter);
                    }
                }
                flag = false;
                stringBuilder.Append("</div>");
                counter++;
            }
            stringBuilder.Append("</div>");

            ViewData["PageCount"] = pageCount;
            ViewData["NearestYear"] = nearestYear;
        }       

        /// <summary>
        /// 焦点图测试版本
        /// </summary>
        private void MakeSerialInfoHtmlV2()
		{
			string liFormatter = "<li class=\"swiper-slide\"><a href=\"http://photo.m.yiche.com/picture/{0}/{1}/\" data-channelid=\"{4}\"><img src=\"{2}\">{3}</a></li>";
			//string summaryInfoStr = "<div class=\"sum-mask\"><div class=\"sum-mask-info\"><h2>{0}</h2><strong>{1}</strong><p><span>{2}</span><span>指导价：{3}</span></p></div><a href=\"javascript:;\" class=\"ico-favorite\" id=\"favstar\" data-channelid=\"27.23.726\"></a></div>";
			//summaryInfoStr = string.Format(summaryInfoStr, serialEntity.SeoName, ViewData["serialPrice"], ViewData["serialTotalPV"], serialEntity.ReferPrice.Replace("万-", "-"));

			var focusImgId = new Dictionary<int, string>();
			List<string> focusImg = new List<string>();
			List<SerialFocusImage> imgList = serialBLL.GetSerialFocusImageList(serialEntity.Id);
			string xmlPicPath = Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialPhotoListPath, serialId));
			DataSet dsCsPic = pageBase.GetXMLDocToDataSetByURLForCache("CarChannel_SerialAllPic_" + serialEntity.Id, xmlPicPath, 60);
			Dictionary<int, Dictionary<int, string>> dicPicNoneWhite = pageBase.GetAllSerialPicNoneWhiteBackground(8);

			if (imgList != null && imgList.Count > 0)
			{
				//大图默认显示焦点图第一张，如果没有焦点图，显示封面图
				SerialFocusImage image = imgList[0];
				string _serialImage =
					string.Format(liFormatter
						, serialId
						, image.ImageId
						, String.Format(image.ImageUrl, 3)
						, string.Empty
						//, summaryInfoStr
						, "27.23.723");
				if (!focusImgId.ContainsKey(image.ImageId))
				{
					focusImgId.Add(image.ImageId, _serialImage);
					focusImg.Add(_serialImage);
				}

				//第二张图取子品牌焦点图第3张（完整内饰）
				if (imgList.Count >= 3)
				{
					SerialFocusImage csImg = imgList[2];
					string smallImgUrl = csImg.ImageUrl;
					if (csImg.ImageId > 0)
					{
						smallImgUrl = String.Format(smallImgUrl, 3);
					}
					string secondImg =
						string.Format(liFormatter
							, serialId
							, csImg.ImageId
							, smallImgUrl
							, string.IsNullOrEmpty(csImg.GroupName) ? string.Empty : "<em class=\"btn-pic\">点击查看" + csImg.GroupName + "图册</em>"
							, "27.23.724");
					if (!focusImgId.ContainsKey(csImg.ImageId))
					{
						focusImgId.Add(csImg.ImageId, secondImg);
						focusImg.Add(secondImg);
					}
				}
			}
			else
			{

				if (dicPicNoneWhite.ContainsKey(serialId))
				{
					string _serialImage =
						string.Format(liFormatter
							, serialId
							, dicPicNoneWhite[serialId].FirstOrDefault().Key
							, dicPicNoneWhite[serialId].FirstOrDefault().Value
							, string.Empty
							, "27.23.723");
					if (!focusImgId.ContainsKey(dicPicNoneWhite[serialId].FirstOrDefault().Key))
					{
						focusImgId.Add(dicPicNoneWhite[serialId].FirstOrDefault().Key, _serialImage);
						focusImg.Add(_serialImage);
					}
				}
			}

			DataTable dtC = null;
			//第三张图取空间分类第1张图
			if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("C"))
			{
				//if (dsCsPic.Tables.Contains("C") && dsCsPic.Tables["C"].Rows.Count > 0)
				//{
				dtC = dsCsPic.Tables["C"];
				//取空间图片第一张图片,与前两张图片不重复

				DataRow[] drP8 = dtC.Select("P='8'");
				foreach (DataRow row in drP8) //空间
				{
					int imgId = row["I"] == DBNull.Value ? 0 : Convert.ToInt32(row["I"]);
					string imgUrl = row["U"] == DBNull.Value ? "" : Convert.ToString(row["U"]);
					if (imgId == 0 || imgUrl.Length == 0 || focusImgId.ContainsKey(imgId))
					{
						continue;
					}
					imgUrl = CommonFunction.GetPublishHashImgUrl(3, imgUrl, imgId);
					string thirdImg =
						string.Format(liFormatter
						, serialId
						, imgId
						, imgUrl
						, "<em class=\"btn-pic\">点击查看空间图册</em>"
						, "27.23.725");
					if (!focusImgId.ContainsKey(imgId))
					{
						focusImgId.Add(imgId, thirdImg);
						focusImg.Add(thirdImg);
					}
					break;
				}
			}

			if (focusImg.Count < 3)
			{
				//如果不够3张,则显示焦点图第2张
				if (imgList.Count >= 2)
				{
					SerialFocusImage csImg = imgList[1];
					string smallImgUrl = csImg.ImageUrl;
					if (csImg.ImageId > 0)
					{
						smallImgUrl = String.Format(smallImgUrl, 3);
					}
					string secondImg =
						string.Format(
							liFormatter
							, serialId
							, csImg.ImageId
							, smallImgUrl
							, string.IsNullOrEmpty(csImg.GroupName) ? string.Empty : "<em class=\"btn-pic\">点击查看" + csImg.GroupName + "图册</em>"
							, "27.23.724");
					if (!focusImgId.ContainsKey(csImg.ImageId))
					{
						focusImgId.Add(csImg.ImageId, secondImg);
						focusImg.Add(secondImg);
					}
				}
				else
				{
					//如果没有焦点图第2张,取图解第一张
					XmlNode firstTujieNode = GetFirstTujieImage(dtC);
					if (firstTujieNode != null)
					{
						string groupName = firstTujieNode.Attributes["GroupName"].Value;
						string backupImg = string.Empty;
						int imgId = Convert.ToInt32(firstTujieNode.Attributes["ImageId"].Value);
						backupImg = string.Format(liFormatter
							, serialId
							, imgId
							, firstTujieNode.Attributes["ImageUrl"].Value.Replace("_4.", "_8.")
							, string.IsNullOrEmpty(groupName) ? string.Empty : "<em class=\"btn-pic\">点击查看" + groupName + "图册</em>"
							, focusImg.Count > 1 ? "27.23.724" : "27.23.723"
						);
						if (!focusImgId.ContainsKey(imgId))
						{
							focusImgId.Add(imgId, backupImg);
							focusImg.Add(backupImg);
						}
					}
				}
			}
			ViewData["FocusImgList"] = focusImg;
		}

        //焦点图
        private void MakeSerialInfoHtml()
        {
            #region 焦点图

            var focusImgId = new Dictionary<int, string>();
            List<string> focusImg = new List<string>();
            List<SerialFocusImage> imgList = serialBLL.GetSerialFocusImageList(serialEntity.Id);
            //子品牌幻灯页
            List<SerialFocusImage> imgSlideList = serialBLL.GetSerialSlideImageList(serialId);
            List<SerialFocusImage> sourceList = new List<SerialFocusImage>();
            string xmlPicPath = Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialPhotoListPath, serialId));
            DataSet dsCsPic = pageBase.GetXMLDocToDataSetByURLForCache("CarChannel_SerialAllPic_" + serialEntity.Id, xmlPicPath, 60);
            Dictionary<int, Dictionary<int, string>> dicPicNoneWhite = pageBase.GetAllSerialPicNoneWhiteBackground();
            
            #region 初始化数据源
            foreach (SerialFocusImage img in imgList)
            {
                sourceList.Add(img);
            }
            //焦点图不足，补幻灯页
            foreach (SerialFocusImage imgS in imgSlideList)
            {
                if (sourceList.Count > 3)
                {
                    break;
                }
                //焦点图片排重
                SerialFocusImage focusImage = imgList.Find(p => p.ImageId == imgS.ImageId);
                if (focusImage != null)
                    continue;
                sourceList.Add(imgS);
            }
            #endregion

            if (sourceList != null && sourceList.Count > 0)
            {
                //大图默认显示焦点图第一张，如果没有焦点图，显示封面图
                SerialFocusImage image = sourceList[0];
                string _serialImage =
                    string.Format(
                        "<a href=\"http://photo.m.yiche.com/picture/{0}/{1}/\" data-channelid=\"27.23.723\" class=\"left-area\"><img alt=\"{4}{5}\" src=\"{2}\">{3}</a>",
                        serialId, image.ImageId, String.Format(image.ImageUrl, 4),
                        string.IsNullOrEmpty(image.GroupName) ? string.Empty : "<em>" + image.GroupName + "</em>",
                        serialEntity.SeoName,
                        image.GroupName);
                if (!focusImgId.ContainsKey(image.ImageId))
                {
                    focusImgId.Add(image.ImageId, _serialImage);
                    focusImg.Add(_serialImage);
                }

                //第二张图取子品牌焦点图第2张（完整内饰）
                if (sourceList.Count >= 2)
                {
                    SerialFocusImage csImg = sourceList[1];
                    string smallImgUrl = csImg.ImageUrl;
                    if (csImg.ImageId > 0)
                    {
                        smallImgUrl = String.Format(smallImgUrl, 4);
                    }
                    string secondImg =
                        string.Format(
                            "<a href=\"http://photo.m.yiche.com/picture/{0}/{3}/\" data-channelid=\"27.23.724\" class=\"img-box\"><img alt=\"{4}{5}\" src=\"{1}\">{2}</a>",
                            serialId, smallImgUrl,
                            string.IsNullOrEmpty(csImg.GroupName) ? string.Empty : "<em>" + csImg.GroupName + "</em>",
                            csImg.ImageId,
                            serialEntity.SeoName,
                            csImg.GroupName);
                    if (!focusImgId.ContainsKey(csImg.ImageId))
                    {
                        focusImgId.Add(csImg.ImageId, secondImg);
                        focusImg.Add(secondImg);
                    }
                }
            }


            DataTable dtC = null;
            if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("C"))
            {
                dtC = dsCsPic.Tables["C"];
            }
            if (focusImg.Count < 3)
            {
                //第三张,取图解第一张
                XmlNode firstTujieNode = GetFirstTujieImage(dtC);
                if (firstTujieNode != null)
                {
                    string groupName = firstTujieNode.Attributes["GroupName"].Value;
                    string backupImg = string.Empty;

					if (focusImg.Count == 1)
					{
						backupImg = string.Format("<a href=\"{0}\" data-channelid=\"27.23.724\" class=\"img-box\"><img alt=\"{3}{4}\" src=\"{1}\">{2}</a>",
							firstTujieNode.Attributes["Link"].Value,
							firstTujieNode.Attributes["ImageUrl"].Value.Replace("_4.", "_4."),
							string.IsNullOrEmpty(groupName) ? string.Empty : "<em>" + groupName + "</em>",
							serialEntity.SeoName,
							groupName);
					}
					else if (focusImg.Count == 0)
                    {
						backupImg = string.Format("<a href=\"{0}\" data-channelid=\"27.23.723\"  class=\"left-area\"><img alt=\"{3}{4}\" src=\"{1}\">{2}</a>",
							firstTujieNode.Attributes["Link"].Value,
							firstTujieNode.Attributes["ImageUrl"].Value.Replace("_4.", "_4."),
							string.IsNullOrEmpty(groupName) ? string.Empty : "<em>" + groupName + "</em>",
							serialEntity.SeoName,
							groupName);
					}
                    else if (focusImg.Count == 2)
                    {
                        backupImg = string.Format("<a href=\"{0}\" data-channelid=\"27.23.725\" class=\"img-box\"><img alt=\"{3}{4}\" src=\"{1}\">{2}</a>",
                            firstTujieNode.Attributes["Link"].Value,
                            firstTujieNode.Attributes["ImageUrl"].Value.Replace("_4.", "_4."),
                            string.IsNullOrEmpty(groupName) ? string.Empty : "<em>" + groupName + "</em>",
                            serialEntity.SeoName,
                            groupName);
                    }
                    if (!focusImgId.ContainsKey(Convert.ToInt32(firstTujieNode.Attributes["ImageId"].Value)))
                    {
                        focusImgId.Add(Convert.ToInt32(firstTujieNode.Attributes["ImageId"].Value), backupImg);
                        focusImg.Add(backupImg);
                    }
                }
                
            }
            if (focusImg.Count < 3)
            {
                //第三张图取子品牌焦点图以及幻灯页第3张
                if (sourceList.Count >= 3)
                {
                    SerialFocusImage csImg = sourceList[2];
                    string smallImgUrl = csImg.ImageUrl;
                    if (csImg.ImageId > 0)
                    {
                        smallImgUrl = String.Format(smallImgUrl, 4);
                    }
                    string secondImg =
                        string.Format(
                            "<a href=\"http://photo.m.yiche.com/picture/{0}/{3}/\" data-channelid=\"27.23.725\" class=\"img-box\"><img alt=\"{4}{5}\" src=\"{1}\">{2}</a>",
                            serialId, smallImgUrl,
                            string.IsNullOrEmpty(csImg.GroupName) ? string.Empty : "<em>" + csImg.GroupName + "</em>",
                            csImg.ImageId,
                            serialEntity.SeoName,
                            csImg.GroupName);
                    if (!focusImgId.ContainsKey(csImg.ImageId))
                    {
                        focusImgId.Add(csImg.ImageId, secondImg);
                        focusImg.Add(secondImg);
                    }
                }
            }
                ViewData["FocusImgList"] = focusImg;
            #endregion
        }

        /// <summary>
        ///     取第一张图解
        /// </summary>
        /// <param name="dsCsPic"></param>
        private XmlNode GetFirstTujieImage(DataTable dt)
        {
            XmlElement element = null;
            if (dt == null || dt.Rows.Count == 0)
            {
                return element;
            }
            var xmlDoc = new XmlDocument();
            //取图解第一张

            IEnumerable<DataRow> rows = dt.Rows.Cast<DataRow>();
            DataRow row = rows.FirstOrDefault(dr => ConvertHelper.GetInteger(dr["P"]) == 12);
            //dt.Select("P='" + cateId + "'");
            if (row != null)
            {
                int imgId = row["I"] == DBNull.Value ? 0 : Convert.ToInt32(row["I"]);
                string imgUrl = row["U"] == DBNull.Value ? "" : Convert.ToString(row["U"]);
                if (imgId == 0 || imgUrl.Length == 0)
                    imgUrl = WebConfig.DefaultCarPic;
                else
                    imgUrl = CommonFunction.GetPublishHashImgUrl(4, imgUrl, imgId);
                string picUrl = "http://photo.m.yiche.com/picture/" + serialId + "/" + imgId + "/";
                element = xmlDoc.CreateElement("CarImage");
                element.SetAttribute("ImageId", imgId.ToString());
                element.SetAttribute("ImageUrl", imgUrl);
                element.SetAttribute("GroupName", "图解");
                element.SetAttribute("ImageName", "图解");
                element.SetAttribute("Link", picUrl);
            }
            return element;
        }

        /// <summary>
        /// 产地
        /// </summary>
        private void GetChanDiName()
        {
            string masterCountry = serialEntity.Brand != null && serialEntity.Brand.MasterBrand != null ? serialEntity.Brand.MasterBrand.Country : string.Empty; //主品牌国别
            string producerCountry = serialEntity.Brand != null ? serialEntity.Brand.Country : string.Empty; //厂商国别
            string chanDi = string.Empty;
            if (masterCountry == producerCountry)
            {
                chanDi = producerCountry;

                if (masterCountry.Contains("中国"))
                {
                    chanDi = masterCountry + "自主";
                }
                else
                {
                    chanDi = masterCountry + "进口";
                }
            }
            else
            {
                chanDi = producerCountry + masterCountry + "合资";
            }
            ViewData["ChanDi"] = chanDi;
        }

        /// <summary>
        ///     论坛话题新闻
        /// </summary>
        private void MakeForumNewsHtml()
        {
            var sb = new StringBuilder();
            XmlDocument xmlDoc = serialBLL.GetSerialForumNews(serialId);
            if (xmlDoc == null) return;
            XmlNodeList newsList = xmlDoc.SelectNodes("/root/Forum/ForumSubject");
            if (newsList.Count <= 1) return;
            string baaUrl = serialBLL.GetForumUrlBySerialId(serialId).Replace("baa.bitauto.com", "baa.m.yiche.com");

            sb.Append("<div class='tt-first' data-channelid=\"27.23.736\">");
            sb.Append("<h3>社区</h3>");
            sb.AppendFormat("<div class='opt-more'><a href='{0}'>更多</a></div>", baaUrl);
            sb.Append("</div>");
            sb.Append("<div class='card-news card-news-bbs b-shadow' id='m_hotforum' data-channelid=\"27.23.737\">");
            sb.Append("<ul>");
            int i = 0;
            foreach (XmlNode node in newsList)
            {
                i++;
                if (i > 3) break;
                string newsTitle = node.SelectSingleNode("title").InnerText.Trim();
                //过滤Html标签
                newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
                newsTitle = StringHelper.SubString(newsTitle, 40, true);
                string tid = node.SelectSingleNode("tid").InnerText;
                string filePath = node.SelectSingleNode("url").InnerText;
                string replies = node.SelectSingleNode("replies").InnerText;
                string poster = node.SelectSingleNode("poster").InnerText;
                string pubTime = "";
                // modified by chengl Jun.15.2012
                if (node.SelectSingleNode("postdatetime") != null)
                {
                    pubTime = node.SelectSingleNode("postdatetime").InnerText;
                    pubTime = Convert.ToDateTime(pubTime).ToString("yyyy-MM-dd");
                }
                sb.AppendFormat("<li class='news-img-list'>");
                sb.AppendFormat("<a href='{0}'>", filePath.Replace("baa.bitauto.com", "baa.m.yiche.com"));
                sb.AppendFormat("<h4 class=\"h25\">{0}</h4>", newsTitle);
                XmlNode imglistNode = node.SelectSingleNode("imgList");
                if (imglistNode != null)
                {
                    XmlNodeList xmlNodeList = imglistNode.SelectNodes("img");
                    if (xmlNodeList != null && xmlNodeList.Count > 0)
                    {
                        sb.AppendFormat("<ul>");
                        int j = 0;
                        foreach (XmlNode item in xmlNodeList)
                        {
                            if (j >= 3)
                            {
                                break;
                            }
                            sb.AppendFormat("<li><span><img src='{0}' alt=\"{1}\"/></span></li>",
                                item.InnerText.Replace("_120_80_", "_216_144_"), newsTitle);
                            j++;
                        }
                        sb.AppendFormat("</ul>");
                    }
                }

                sb.AppendFormat("<em><span>{0}</span><span>{1}</span><i class='ico-comment'>{2}</i></em>", poster,
                    pubTime, replies);
                sb.AppendFormat("</a>");
                sb.AppendFormat("</li>");
            }
            sb.Append("<ins id=\"div_3bf56f1a-a766-437c-83de-572a58dc3909\" data-type=\"ad_play\" data-adplay_ip=\"\" data-adplay_areaname=\"\" data-adplay_cityname=\"\" data-adplay_brandid=\""+serialEntity.Id+"\" data-adplay_brandname=\"\" data-adplay_brandtype=\"\" data-adplay_blockcode=\"3bf56f1a-a766-437c-83de-572a58dc3909\"> </ins>");
            sb.Append("</ul>");
            sb.Append("</div>");

            ViewData["BaaUrl"] = baaUrl;
            ViewData["ForumNewsHtml"] = sb.ToString();
        }

        /// <summary>
        /// 综合对比
        /// </summary>
        /// <returns></returns>
        private void GetSerialHotCompareCars()
        {
            StringBuilder sb = new StringBuilder();
            //List<EnumCollection.SerialHotCompareData> lshcd = base.GetSerialHotCompareByCsID(serialId, 6);
            Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = serialBLL.GetSerialCityCompareList(serialId, null);
            if (carSerialBaseList != null && carSerialBaseList.ContainsKey("全国"))
            {
                var dicAllCsPic = pageBase.GetAllSerialPicURL(true);
                List<Car_SerialBaseEntity> serialBaseEntityList = carSerialBaseList["全国"];

                var htmlCode = new StringBuilder();
                htmlCode.Append("<div class='tt-first' id='m-tabs-kankan'>");
                htmlCode.Append("<h3>找相似</h3>");
                htmlCode.Append("</div>");
                htmlCode.Append("<div class='swiper-container-kankan swiper-container'><div class='swiper-wrapper'>");
                htmlCode.Append("<div class='swiper-slide'>");
                htmlCode.Append("<div class='car-list3' data-channelid=\"27.23.743\">");
                htmlCode.Append("<ul>");
                foreach (Car_SerialBaseEntity carSerial in serialBaseEntityList)
                {
                    string imgUrl = dicAllCsPic.ContainsKey(carSerial.SerialId) ? dicAllCsPic[carSerial.SerialId].Replace("_2.jpg", "_3.jpg") : WebConfig.DefaultCarPic;
                    string referPrice = pageBase.GetSerialReferPriceByID(carSerial.SerialId);
                    if (string.IsNullOrEmpty(referPrice))
                    {
                        referPrice = "暂无指导价";
                    }
                    htmlCode.Append("<li>");
                    htmlCode.AppendFormat("<a href='/{0}/'>", carSerial.SerialNameSpell);
                    htmlCode.AppendFormat("<img src='{0}'/>", imgUrl);
                    htmlCode.AppendFormat("<strong>{0}</strong>", carSerial.SerialShowName);
                    htmlCode.AppendFormat("<p>{0}</p>", referPrice);
                    htmlCode.Append("</a>");
                    htmlCode.Append("</li>");
                }
                htmlCode.Append("</ul>");
                htmlCode.Append("</div></div>");
                htmlCode.Append("</div></div>");
                ViewData["CsHotCompareCars"] = htmlCode.ToString();// htmlCode.Insert(0, htmlTagCode).ToString();
            }
        }

        /// <summary>
        /// 子品牌还关注
        /// </summary>
        /// <returns></returns>
        private void MakeSerialToSerialHtml()
        {
            var htmlCode = new StringBuilder();
            //var htmlTagCode = new StringBuilder();

            htmlCode.Append("<div class='tt-first' id='m-tabs-kankan'>");
            htmlCode.Append("<h3>看了还看</h3>");
            htmlCode.Append("</div>");
            htmlCode.Append("<div class='swiper-container-kankan swiper-container'><div class='swiper-wrapper'>");
            List<EnumCollection.SerialToSerial> lsts = pageBase.GetSerialToSerialByCsID(serialId, 6);
            if (lsts.Count > 0)
            {
                htmlCode.Append(" <div class='swiper-slide'>");
                htmlCode.Append("<div class='car-list3' data-channelid=\"27.23.743\">");
                htmlCode.Append("<ul>");
                for (int i = 0; i < lsts.Count && i < 6; i++)
                {
                    EnumCollection.SerialToSerial sts = lsts[i];
                    string csName = sts.ToCsShowName;
                    htmlCode.Append("<li>");
                    htmlCode.AppendFormat("<a href='/{0}/'>", sts.ToCsAllSpell.ToLower());
                    htmlCode.AppendFormat("<img src='{0}' alt=\"{1}\" />", sts.ToCsPic.ToString(CultureInfo.InvariantCulture).Replace("_5.", "_3."), csName);
                    htmlCode.AppendFormat("<strong>{0}</strong>", csName);
                    htmlCode.AppendFormat("<p>{0}</p>",
                        string.IsNullOrEmpty(sts.ToCsPriceRange) ? "暂无指导价" : sts.ToCsPriceRange);
                    htmlCode.Append("</a>");
                    htmlCode.Append("</li>");
                }
                htmlCode.Append("</ul>");
                htmlCode.Append("</div></div>");
            }
            htmlCode.Append("</div></div>");
            ViewData["SerialToSee"] = htmlCode.ToString();// htmlCode.Insert(0, htmlTagCode).ToString();
        }

        /// <summary>
        /// 车系视频
        /// </summary>
        public void GetVideo()
        {
            List<VideoEntity> videoList = VideoBll.GetNewAndHotVideoBySerialIdForWireless(serialId);
            ViewData["VideoList"] = videoList;
        }

        private void InitNewsData()
        {
            bool pingceLiFlag = false;//文章-评测 是否大于3条
            bool daogouLiFlag = false;//文章-导购 是否大于3条

            string newsPingceHtml = GetCategoryNewsHtml(CarNewsType.pingce, ref pingceLiFlag);
            string newsDaogouHtml = GetCategoryNewsHtml(CarNewsType.daogou, ref daogouLiFlag);
            ViewData["NewsPingceHtml"] = newsPingceHtml;
            ViewData["PingceLiFlag"] = pingceLiFlag;
            ViewData["NewsDaogouHtml"] = newsDaogouHtml;
            ViewData["DaogouLiFlag"] = daogouLiFlag;
        }

        /// <summary>
        /// 分类新闻
        /// </summary>
        /// <param name="carNewsType">新闻分类</param>
        /// <param name="ligt3Flag"></param>
        /// <returns></returns>
        private string GetCategoryNewsHtml(CarNewsType carNewsType, ref bool ligt3Flag)
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsPingce = newsBLL.GetTopSerialNewsAllData(serialId, carNewsType, 6);
            if (dsPingce != null && dsPingce.Tables.Count > 0 && dsPingce.Tables[0].Rows.Count > 0)
            {
                int index = 0;
                foreach (DataRow dr in dsPingce.Tables[0].Rows)
                {
                    int newsId = ConvertHelper.GetInteger(dr["CmsNewsId"]);
                    string url = ConvertHelper.GetString(dr["FilePath"]);

                    DateTime date = ConvertHelper.GetDateTime(dr["PublishTime"]);
                    string editorName = ConvertHelper.GetString(dr["EditorName"]);
                    string faceTitle = ConvertHelper.GetString(dr["FaceTitle"]);
                    //modified by sk 22个文字 2017-03-06
                    faceTitle = StringHelper.GetRealLength(faceTitle) > 44 ? StringHelper.SubString(faceTitle, 44, false) : faceTitle;
                    string picUrl = ConvertHelper.GetString(dr["Picture"]);
                    string imageUrl = (!string.IsNullOrEmpty(picUrl) && picUrl.IndexOf("/not") < 0) ? picUrl : ConvertHelper.GetString(dr["FirstPicUrl"]);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        imageUrl = imageUrl.Replace("/bitauto/", "/newsimg_180x120/bitauto/")
                            .Replace("/autoalbum/", "/newsimg_180x120/autoalbum/");
                    }
                    string newsUrl = Convert.ToString(dr["filepath"]).Replace("news.bitauto.com", "news.m.yiche.com");
                    if (index >= 3)
                    {
                        sb.AppendFormat("<li style=\"display:none;\" class=\"{0}\">", string.IsNullOrEmpty(imageUrl) ? "news-noimg" : "");
                    }
                    else
                    {
                        sb.AppendFormat("<li class=\"{0}\">", string.IsNullOrEmpty(imageUrl) ? "news-noimg" : "");
                    }
                    sb.AppendFormat("    <a href=\"{0}\">", newsUrl);
                    sb.Append(string.IsNullOrEmpty(imageUrl) ? "" : string.Format("<div class=\"img-box\"><span><img src=\"{0}\"></span></div>", imageUrl));
                    sb.AppendFormat("        <div class=\"con-box\"><h4>{0}</h4><em><span>{1}</span><span>{2}</span><i class=\"ico-comment huifu comment_0_{3}\"></i></em>", faceTitle, date.ToString("yyyy-MM-dd"), editorName, newsId);
                    sb.AppendFormat("        </div>");
                    sb.AppendFormat("    </a>");
                    sb.AppendFormat("</li>");
                    index++;
                }
                if (index > 3)
                    ligt3Flag = true;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 车系静态块
        /// </summary>
        private void GetSerialBlockHtml()
        {
            Dictionary<int, string> dictSerialBlockHtmlV2 = commonhtmlBLL.GetCommonHtml(
                serialId,
                CommonHtmlEnum.TypeEnum.Serial,
                CommonHtmlEnum.TagIdEnum.WirelessSerialSummaryV2); //静态块内容 (新增)口碑+详情视图区域

            const int hexinReport = (int)CommonHtmlEnum.BlockIdEnum.HexinReport; //车型详解
            string carDetailsViewZoneHtml = string.Empty;
            if (dictSerialBlockHtmlV2.ContainsKey(hexinReport))
            {
                carDetailsViewZoneHtml = dictSerialBlockHtmlV2[hexinReport];
            }

            string serialNews = string.Empty;
            const int focusNewsForWireless = (int)CommonHtmlEnum.BlockIdEnum.FocusNewsForWireless; //焦点新闻
            if (dictSerialBlockHtmlV2.ContainsKey(focusNewsForWireless))
                serialNews = dictSerialBlockHtmlV2[focusNewsForWireless];

            string koubeiImpressionHtml = string.Empty; //口碑印象
            const int koubei = (int)CommonHtmlEnum.BlockIdEnum.KoubeiReportNew;
            if (dictSerialBlockHtmlV2.ContainsKey(koubei))
            {
                koubeiImpressionHtml = dictSerialBlockHtmlV2[koubei];
            }

            ViewData["CarDetailsViewZoneHtml"] = carDetailsViewZoneHtml;
            ViewData["SerialNews"] = serialNews;
            ViewData["KoubeiImpressionHtml"] = koubeiImpressionHtml;
        }


        /// <summary>
        /// 车系颜色
        /// </summary>
        public void GetSerialColors()
        {
            List<SerialColorEntity> SerialColorList = serialBLL.GetProduceSerialColors(serialId);
            ViewData["SerialColorList"] = SerialColorList;
        }

        ///// <summary>
        ///// 获取vr url
        ///// </summary>
        //private void GetVrUrl()
        //{
        //    Dictionary<int, string> vrDic = serialBLL.GetSerialVRUrl();
        //    string VRUrl = string.Empty;
        //    if (vrDic != null && vrDic.ContainsKey(serialId))
        //    {
        //         VRUrl = vrDic[serialId];
        //    }
        //    ViewData["VRUrl"] = VRUrl;
        //}

        /// <summary>
        /// 五年保值率
        /// </summary>
        protected void GetBaoZhiLv()
        {
            Dictionary<int, XmlElement> dic = serialBLL.GetSeialBaoZhiLv();
            string baoZhiLv = string.Empty;
            //string[] baoZhiLvLevel = { "weixingche", "xiaoxingche", "jincouxingche", "zhongxingche", "zhongdaxingche", "haohuaxingche", "mpv", "suv", "paoche", "mianbaoche" };
            if (dic != null && dic.ContainsKey(serialId))
            {
                XmlElement ele = dic[serialId];
                if (ele != null)
                {
                    string levelSpell = BitAuto.CarUtils.Define.CarLevelDefine.GetLevelSpellByName(serialEntity.Level.Name);
                    baoZhiLv = string.Format("<dl class=\"sum-baozhilv\"><dt class=\"w3\">保值率：</dt><dd>{0}% <a href=\"/{1}/baozhilv/\" data-channelid=\"27.23.2041\">排行 ></a></dd></dl>"
                        , Math.Round(ConvertHelper.GetDouble(ele.Attributes["ResidualRatio5"].InnerText) * 100, 1)
                        , levelSpell
                        );
                }
            }
            ViewData["baoZhiLv"] = baoZhiLv;
        }

        /// <summary>
        /// 初始化title信息
        /// </summary>
        private void InitTitle()
        {
            string title = string.Format("【{0}】最新{0}报价_参数_图片_{1}{2}社区-手机易车网"
                , serialEntity.SeoName
                , serialEntity.Brand.MasterBrand.Name
                , serialEntity.Name);

            string keyWords = string.Format("{0},{0}报价,{0}价格,{0}参数,{0}社区,手机易车网,car.m.yiche.com"
                , serialEntity.SeoName);

            string description =
               string.Format("{1}{2},易车提供全国官方4S店{0}报价,最新{1}{2}降价优惠信息。以及{0}报价,{0}图片,{0}在线询价服务,低价买车尽在手机易车网。"
                   , serialEntity.SeoName
                   , serialEntity.Brand.MasterBrand.Name
                   , serialEntity.Name);

            ViewData["title"] = title;
            ViewData["keyWords"] = keyWords;
            ViewData["description"] = description;
        }

        /*

        public void GetTab()
        {
            var showText = "";

            //在销车系下有待销车款
            if (serialEntity.SaleState.Trim() == "在销" || serialEntity.SaleState.Trim() == "停销")
            {
                //筛选待销车款
                IEnumerable<CarInfoForSerialSummaryEntity> newCarList = serialCarList.Where(i => i.SaleState.Trim() == "待销");
                //上市车系下有待销车款
                if (newCarList.Count() > 0)
                {
                    //筛选填写了上市时间的待销车款
                    IEnumerable<CarInfoForSerialSummaryEntity> newCarMarketDateTimeList = newCarList.Where(a => DateTime.Compare(a.MarketDateTime, DateTime.MinValue) != 0);
                    //存在填写了上市时间的待销车款
                    if (newCarMarketDateTimeList.Count() > 0)
                    {
                        CarInfoForSerialSummaryEntity car = newCarMarketDateTimeList.First();//从已经填写的时间中选择最早的时间
                        //排除未上市车填写了过去的上市时间（这种情况属于数据错误，通过程序筛选控制）
                        if (DateTime.Compare(car.MarketDateTime, DateTime.Now) >= 0)
                        {
                            showText = "将于" + car.MarketDateTime.ToString("yy年MM月dd日") + "上市";
                        }
                    }
                    //没有填写上市时间
                    else
                    {
                        //判断车款是否有实拍图或者填写了指导价
                        int count = 0;
                        foreach (var item in newCarList)
                        {
                            count = carBLL.GetSerialCarRellyPicCount(item.CarID);
                            //存在实拍图
                            if (count > 0)
                            {
                                showText = "新款即将上市";
                                break;
                            }
                            else
                            {
                                //是否有指导价
                                if (item.ReferPrice != "")
                                {
                                    showText = "新款即将上市";
                                    break;
                                }
                            }
                        }                        
                    }
                }
                //新车款上市初期
                else
                {
                    //车款中筛选填写了上市时间的车款
                    IEnumerable<CarInfoForSerialSummaryEntity> newCarMarketDateTimeList = serialCarList.Where(a => DateTime.Compare(a.MarketDateTime, DateTime.MinValue) != 0);
                    if (newCarMarketDateTimeList.Count() > 0)
                    {
                        CarInfoForSerialSummaryEntity car = newCarMarketDateTimeList.First();//倒叙排列，取第一个即可
                        if (car != null)
                        {
                            int days = GetDaysAboutCurrentDateTime(car.MarketDateTime);
                            if (days >= 0 && days <= 30)
                            {
                                //只有一个年款    ***新车上市***
                                if (serialCarList.GroupBy(i => i.CarYear).Count() == 1)
                                {
                                    showText = "新车上市";
                                }
                                //不止一个年款    ***新款上市***
                                else
                                {
                                    showText = "新款上市";
                                }
                            }
                        }
                    }                                   
                }
            }
            //待查 待销(未上市)
            else
            {
                //筛选填写了上市时间的待销车  车系是待销状态，该车系下的车款全部是待销或者停销
                IEnumerable<CarInfoForSerialSummaryEntity> newCarList = serialCarList.Where(a => DateTime.Compare(a.MarketDateTime, DateTime.MinValue) != 0);
                //存在填写了上市时间的待销车
                if (newCarList.Count() > 0)
                {
                    CarInfoForSerialSummaryEntity car = newCarList.First();//从已经填写的时间中选择最早的时间
                    //排除未上市车填写了过去的上市时间（这种情况属于数据错误，通过程序筛选控制）
                    if (DateTime.Compare(car.MarketDateTime, DateTime.Now) >= 0)
                    {
                        showText = "将于" + car.MarketDateTime.ToString("yy年MM月dd日") + "上市";
                    }                        
                }
                //没有上市时间，判断有没有实拍图、指导价
                else
                {
                    int count = 0;
                    foreach (var item in serialCarList)
                    {
                        count = carBLL.GetSerialCarRellyPicCount(item.CarID);
                        if (count > 0)
                        {
                            showText = "即将上市";
                            break;
                        }
                        //是否有指导价
                        else
                        {
                            //是否有指导价
                            if (item.ReferPrice.Trim() != "")
                            {
                                showText = "即将上市";
                                break;
                            }
                        }
                    }                    
                }
            }

            ViewData["showText"] = showText;
        }

        private string GetMarketFlag(CarInfoForSerialSummaryEntity entity)
        {
            string marketflag = "";

            if (entity != null)
            {
                if (entity.MarketDateTime != DateTime.MinValue)
                {
                    int days = GetDaysAboutCurrentDateTime(entity.MarketDateTime);
                    if (days >= 0 && days <= 30)
                    {
                        if (entity.SaleState.Trim() == "在销")
                        {
                            marketflag = "<em class=\"the-new\">新上市</em>";
                        }                            
                    }
                    else if (days >= -30 && days < 0)
                    {
                        if (entity.SaleState == "待销")
                        {
                            marketflag = "<em class=\"the-new\">即将上市</em>";
                        }                            
                    }
                }
                else
                {
                    if (entity.SaleState.Trim() == "待销")
                    {
                        var picCount = carBLL.GetSerialCarRellyPicCount(entity.CarID);
                        if (picCount > 0)
                        {
                            marketflag = "<em class=\"the-new\">即将上市</em>";
                        }
                        else
                        {
                            if (entity.ReferPrice != "")
                            {
                                marketflag = "<em class=\"the-new\">即将上市</em>";
                            }
                        }
                    }
                }
            }
            return marketflag;
        }       

        public int GetDaysAboutCurrentDateTime(DateTime dt)
        {
            DateTime currentDateTime = DateTime.Now.Date;
            int days = (currentDateTime - dt).Days;
            return days;
        }

    */
    }
}
