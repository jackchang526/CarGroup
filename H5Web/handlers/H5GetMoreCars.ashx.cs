using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace H5Web.handlers
{
    /// <summary>
    /// H5GetMoreCars 的摘要说明
    /// </summary>
    public class H5GetMoreCars : H5PageBase,IHttpHandler
    {
        private Car_BasicBll _carBll;
        private string _carList;
        private List<CarInfoForSerialSummaryEntity> _serialCarList;
        private SerialEntity _serialEntity;
        private int _serialId;
        private string _serialShowName;
        private string _serialYear;
        private int pageNum;
        private int pageSize = 10;
        private HttpRequest request;
        private HttpResponse response;
        protected bool isElectrombile = false;
        // 经纪人ID
        protected int Brokerid = 0;
        // 经销商ID
        protected int Dealerid = 0;
        /// <summary>
        /// 车系为电动车的续航里程区间
        /// </summary>
        protected string mileageRange = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
			base.SetPageCache(60);

            context.Response.ContentType = "text/html";
            request = context.Request;
            response = context.Response;
            GetParamter();
            InitSerialInfo();
            if (pageNum <= 1) return;
            CarList(_serialYear);
        }

        public bool IsReusable
        {
            get { return false; }
        }

        private void GetParamter()
        {
            _serialId = ConvertHelper.GetInteger(request.QueryString["ID"]);
            _serialYear = request.QueryString["year"];
            pageNum = ConvertHelper.GetInteger(request.QueryString["page"]);
            _serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, _serialId);
            _serialShowName = _serialEntity.ShowName;
        }

        private void InitSerialInfo()
        {
            _carBll = new Car_BasicBll();
            _serialCarList = _carBll.GetCarInfoForSerialSummaryBySerialId(_serialId);
            _serialCarList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
        }

        /// <summary>
        ///     车型列表
        /// </summary>
        private void CarList(string year)
        {
            var htmlCode = new StringBuilder();
            var stringBuilder = new StringBuilder();
            string serialAllSpell = request.QueryString["serialAllSpell"];
            int minMileage = 0;
            int maxMileage = 0;
            int count = (pageNum - 1) * 10;
            List<CarInfoForSerialSummaryEntity> currentYearCarList;
            IEnumerable<IGrouping<object, CarInfoForSerialSummaryEntity>> querySale;
            Brokerid = ConvertHelper.GetInteger(request.QueryString["brokerid"]);
            Dealerid= ConvertHelper.GetInteger(request.QueryString["dealerid"]);
            #region 数据筛选

            switch (year)
            {
                case "unlisted":
                    currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "待销");

                    querySale = currentYearCarList.Skip(count).Take(10).GroupBy(
                        p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower },
                        p => p);
                    break;
                default:
                    currentYearCarList = _serialEntity.SaleState == "停销" ? _serialCarList.FindAll(p => p.SaleState == "停销" && p.CarYear == year) : _serialCarList.FindAll(p => p.SaleState == "在销" && p.CarYear == year);
                    querySale = currentYearCarList.Skip(count).Take(10).GroupBy(
                        p =>
                            new
                            {
                                p.Engine_Exhaust,
                                p.Engine_InhaleType,
                                p.Engine_AddPressType,
                                p.Engine_MaxPower
                            },
                        p => p);
                    break;
            }

            #endregion

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
                    if (carInfo.CarName.StartsWith(_serialShowName))
                    {
                        carFullName = carInfo.CarName.Substring(_serialShowName.Length);
                    }
                    if (year == "未上市")
                    {
                        carFullName = "未上市" + carFullName;
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
                            : "http://car.m.yiche.com/" + serialAllSpell + "/m" + carInfo.CarID + "/");//如果车系为停售，点击车款后，呈现界面不友好，改为进入车款的配置页


                    stringBuilder.AppendFormat("<h2>{0}{1}{2}</h2>", carFullName + " " + Perf_SeatNum, parallelImport, stopPrd);

                    if (Brokerid > 0)
                    {
                        stringBuilder.AppendFormat("<span>{0}</span>", carInfo.ReferPrice.Trim().Length == 0 ? "暂无" : carInfo.ReferPrice.Trim() + "万");
                        stringBuilder.Append("</a>");
                        stringBuilder.AppendFormat("<div class='car-call'><a href='Agent.aspx?carid={0}&brokerid={1}'>询最低价</a></div>", carInfo.CarID,Brokerid);
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
            response.Write(stringBuilder.ToString());
        }
    }
}