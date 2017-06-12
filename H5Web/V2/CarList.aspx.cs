using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace H5Web.V2
{
    public partial class CarList : H5PageBase
    {
        /// <summary>
        ///     车型列表html
        /// </summary>
        protected string CarListHtml;
        protected int YearCount;
        protected int SerialId = 0;
        protected int PageCount = 0;
        private const int PageSize = 10;
        protected string SerialShowName;
        protected string SerialAllSpell;
        protected int MaxYear = 0;
        // 经纪人ID
        protected int Brokerid = 0;
        // 经销商ID
        protected int Dealerid = 0;

        /// <summary>
        /// 是否是全系电动车
        /// </summary>
        protected bool IsElectrombile = false;
        /// <summary>
        /// 车系为电动车的续航里程区间
        /// </summary>
        protected string MileageRange = string.Empty;
        protected SerialEntity BaseSerialEntity;
        private List<CarInfoForSerialSummaryEntity> _serialCarList = new List<CarInfoForSerialSummaryEntity>();
        private readonly Car_BasicBll _carBll= new Car_BasicBll();
        protected void Page_Load(object sender, EventArgs e)
        {
            //base.SetPageCache(30);//设置页面缓存

            if (Request.QueryString["brokerid"] != null)
            {
                int.TryParse(Request.QueryString["brokerid"], out Brokerid);
            }

            if (Request.QueryString["csid"] != null)
            {
                int.TryParse(Request.QueryString["csid"], out SerialId);
            }

            if (Request.QueryString["dealerid"] != null)
            {
                int.TryParse(Request.QueryString["dealerid"], out Dealerid);
            }

            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);
            SerialShowName = BaseSerialEntity.ShowName;
            SerialAllSpell = BaseSerialEntity.AllSpell;

            _serialCarList = _carBll.GetCarInfoForSerialSummaryBySerialId(SerialId);

            if (Dealerid <= 0)
            {
                GetCarList();
            }

            #region add by songcl 2015-07-10 在销车系全系为电动车

            var fuelTypeList = _serialCarList.FindAll(p => p.SaleState == "在销").Where(p => p.Oil_FuelType != "")
                .GroupBy(p => p.Oil_FuelType)
                .Select(g => g.Key).ToList();
            IsElectrombile = fuelTypeList.Count == 1 && fuelTypeList[0] == "电力";

            #endregion

        }

        private void GetCarList()
        {
            _serialCarList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);

            //年款
            var yearList = new List<string>();

            if (BaseSerialEntity.SaleState == "待销") //车系待销
            {
                //待销不列年款
            }
            else if (BaseSerialEntity.SaleState == "停销")  //车系停销
            {
                //取车系为停销状态的停销年款
                yearList =
                    _serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "停销")
                        .Select(p => p.CarYear)
                        .Distinct()
                        .ToList();
            }
            else  //车系在销
            {
                //取车系为在销状态的在销年款
                yearList =
                    _serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "在销")
                        .Select(p => p.CarYear)
                        .Distinct()
                        .ToList();
            }

            yearList.Sort(NodeCompare.CompareStringDesc);
            YearCount = yearList.Count;
            //在售和停售数据集合
            //List<CarInfoForSerialSummaryEntity> salingAndNoSaleList = _serialCarList.FindAll(p => p.SaleState != "待销");

            //停售数据集合
            //List<CarInfoForSerialSummaryEntity> carinfoNoSaleList = _serialCarList.FindAll(p => p.SaleState == "停销");

            //在售数据集合
            //List<CarInfoForSerialSummaryEntity> carinfoSaleList = _serialCarList.FindAll(p => p.SaleState == "在销");

            //待售（未上市）数据集合
            List<CarInfoForSerialSummaryEntity> carinfoWaitSaleList = _serialCarList.FindAll(p => p.SaleState == "待销");

            var htmlCode = new StringBuilder();

            htmlCode.Append("<div class='second-tags-scroll mgt12'>");
            htmlCode.Append("<ul id='yeartag'>");

            var targetList = new List<string>();
            if (carinfoWaitSaleList.Count > 0)
            {
                targetList.Add("未上市");
            }
            targetList.AddRange(yearList);
            foreach (string year in targetList)
            {
                switch (year)
                {
                    case "未上市":
                        htmlCode.AppendFormat("<li id='unlisted'><a><span>未上市</span></a></li>");
                        break;
                    default:
                        htmlCode.AppendFormat("<li id='{0}'><a><span>{0}款</span></a></li>", year);
                        break;
                }
            }

            htmlCode.Append("</ul>");
            htmlCode.Append("</div>");

            GetCarHtml(targetList, htmlCode);

            CarListHtml = htmlCode.ToString();
        }

        /// <summary>
        ///     songcl 2015-08-13
        /// </summary>
        /// <param name="yearList"></param>
        /// <param name="stringBuilder"></param>
        /// <returns></returns>
        private void GetCarHtml(IEnumerable<string> yearList, StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat("<div id='carList{0}'>", SerialId);
            int minMileage = 0;
            int maxMileage = 0;
            foreach (string item in yearList)
            {
                #region 数据筛选

                string year = item;
                List<CarInfoForSerialSummaryEntity> currentYearCarList;
                switch (year)
                {
                    case "未上市":
                        currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "待销");
                        break;
                    default:

                        #region 筛选最新年款

                        if (ConvertHelper.GetInteger(year) > MaxYear)
                        {
                            MaxYear = ConvertHelper.GetInteger(year);
                        }

                        #endregion

                        currentYearCarList = BaseSerialEntity.SaleState == "停销" ? 
                            _serialCarList.FindAll(p => p.SaleState == "停销" && p.CarYear == year) 
                            : _serialCarList.FindAll(p => p.SaleState == "在销" && p.CarYear == year);
                        break;
                }

                int carCount = currentYearCarList.Count;
                IEnumerable<IGrouping<object, CarInfoForSerialSummaryEntity>> querySale = currentYearCarList.Take(10).GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower }, p => p);
                PageCount = carCount / PageSize + (carCount % PageSize == 0 ? 0 : 1);

                //start add by sk 2014-09-03 候姐 整组 停产 把整组移到最底 且保持前 排序规则
                var listGroupNew = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
                var listGroupOff = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
                foreach (var info in querySale)
                {
                    CarInfoForSerialSummaryEntity isStopState = info.FirstOrDefault(p => p.ProduceState != "停产");
                    if (isStopState != null)
                        listGroupNew.Add(info);
                    else
                        listGroupOff.Add(info);
                }
                listGroupNew.AddRange(listGroupOff);

                #endregion

                stringBuilder.AppendFormat("<div id='yearDiv{0}' style='display:none;' class='sum-car-type-box' pagecount='{1}'>",
                    item, PageCount);
                foreach (var info in listGroupNew)
                {
                    #region 基础信息准备

                    var key = CommonFunction.Cast(info.Key,
                        new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0 });
                    string strMaxPowerAndInhaleType = string.Empty;
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
                        strMaxPowerAndInhaleType = string.Format("{0}{1}", maxPower, " " + inhaleType);
                    }

                    #endregion
                    
                    stringBuilder.Append("<div class='tt-small'>");
                    stringBuilder.AppendFormat("<span>{0}{1}{2}</span>", key.Engine_Exhaust,
                        (string.IsNullOrEmpty(key.Engine_Exhaust) || string.IsNullOrEmpty(strMaxPowerAndInhaleType))
                            ? ""
                            : "/", strMaxPowerAndInhaleType);
                    stringBuilder.Append("</div>");

                    stringBuilder.Append("<div class='car-card car-price'>");
                    stringBuilder.Append("<ul>");

                    #region

                    List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList(); //分组后的集合

                    foreach (
                        CarInfoForSerialSummaryEntity carInfo in carGroupList)
                    {
                        string carFullName = "";
                        carFullName = carInfo.CarName;
                        if (carInfo.CarName.StartsWith(SerialShowName))
                        {
                            carFullName = carInfo.CarName.Substring(SerialShowName.Length);
                        }
                        if (year == "未上市")
                        {
                            carFullName ="未上市" + carFullName;
                        }
                        else
                        {
                            carFullName = carInfo.CarYear + "款 " + carFullName;
                        }
                        
                        string stopPrd = "";
                        if (carInfo.ProduceState == "停产")
                            stopPrd = "<em class='tingchan'>停产</em>";

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

                        Dictionary<int, string> dictCarParams = _carBll.GetCarAllParamByCarID(carInfo.CarID);

                        #region 纯电动车续航里程

                        if (IsElectrombile)
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
                                MileageRange = minMileage == maxMileage
                                    ? string.Format("{0}公里", minMileage)
                                    : string.Format("{0}-{1}公里", minMileage, maxMileage);
                            }
                        }

                        #endregion


                        // 档位个数
                        string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" &&
                                                 dictCarParams[724] != "待查")
                            ? dictCarParams[724] + "挡"
                            : "";

                        //平行进口车标签
                        string parallelImport = "";
                        if (dictCarParams.ContainsKey(382) && dictCarParams[382] == "平行进口")
                        {
                            parallelImport = "<em>平行进口</em>";
                        }

                        Dictionary<int, string> dic = _carBll.GetCarAllParamByCarID(carInfo.CarID);
                        string Perf_SeatNum = dic.ContainsKey(665) ? dic[665] + "座" : "";//成员人数

                        stringBuilder.Append("<li>");

                        stringBuilder.AppendFormat(
                            "<a  id='carlist_" + carInfo.CarID + "' class='car-info' href='{0}'>",
                            carInfo.SaleState != "停销"
                                ? "http://price.m.yiche.com/nc" + carInfo.CarID + "/"
                                : "http://car.m.yiche.com/" + SerialAllSpell + "/m" + carInfo.CarID + "/");//如果车系为停售，点击车款后，呈现界面不友好，改为进入车款的配置页


                        stringBuilder.AppendFormat("<h2>{0}{1}{2}</h2>", carFullName + " " + Perf_SeatNum, parallelImport, stopPrd);
                        if (Brokerid > 0)
                        {
                            stringBuilder.AppendFormat("<span>{0}</span>", carInfo.ReferPrice.Trim().Length == 0 ? "暂无" : carInfo.ReferPrice.Trim() + "万");
                            stringBuilder.Append("</a>");
                            stringBuilder.AppendFormat("<div class='car-call'><a href='Agent.aspx?carid={0}&brokerid={1}'>询最低价</a></div>", carInfo.CarID, Brokerid);
                        }
                        else
                        {
                            stringBuilder.AppendFormat("<span>{0}</span>", carMinPrice);
                            stringBuilder.AppendFormat("<del>{0}</del>",
                                carInfo.ReferPrice.Trim().Length == 0 ? "暂无" : carInfo.ReferPrice.Trim() + "万");
                            stringBuilder.Append("</a>");
                            stringBuilder.AppendFormat("<div class='car-call'><a href='http://price.m.yiche.com/zuidijia/nc{0}/?t=yichemobiletest2&leads_source=20018\'>询最低价</a></div>", carInfo.CarID);
                        }
                        
                        stringBuilder.Append("</li>");
                    }

                    #endregion

                    stringBuilder.Append("</ul>");
                    stringBuilder.Append("</div>");
                }

                if (PageCount > 1)
                {
                    stringBuilder.Append("<div class='box'><a id='btnMoreCar' page='2' class='btn-more btn-add-more' href='javascript:void(0);'><i>加载更多</i></a></div>");
                }
                stringBuilder.Append("</div>");
            }
            stringBuilder.Append("</div>");
        }

    }
}