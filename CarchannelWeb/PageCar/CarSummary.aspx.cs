using System;
using System.Data;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using System.IO;

namespace BitAuto.CarChannel.CarchannelWeb.PageCar
{
    /// <summary>
    /// 车型综述
    /// </summary>
    public partial class CarSummary : PageBase
    {
        #region Param
        protected int carID = 0;
        protected Dictionary<int, string> dicCarInfos = new Dictionary<int, string>();
        protected string carConfigData = string.Empty;
        protected string carPhotoData = string.Empty;
        private StringBuilder PhotoData = new StringBuilder();
        //private StringBuilder HotNews = new StringBuilder();
        protected EnumCollection.CarInfoForCarSummary cfcs = new EnumCollection.CarInfoForCarSummary();
        protected string csDefaultPic = string.Empty;
        protected int csPicCount = 0;
        protected string hotSerialNews = string.Empty;
        protected string hotCars = string.Empty;
        protected string carAsk = string.Empty;
        protected string serialDefaultPic = string.Empty;
        protected int serialPicCount = 0;
        protected CarEntity cbe;
        protected string CarHeadHTML = string.Empty;
        protected string CarHotCompare = string.Empty;
        protected string CarCsKouBei = string.Empty;
        protected string CarLevelURL = string.Empty;
        protected string waiguanUrl = string.Empty;
        protected string neishiUrl = string.Empty;
        protected string kongjianUrl = string.Empty;
        protected string tushuoUrl = string.Empty;
        protected string qitaUrl = string.Empty;
        protected string forumUrl = string.Empty;
        protected string carPriceHtml = string.Empty;
        protected string fuelString = string.Empty;
        protected string summaryFuelHtml = string.Empty;
        protected string UserBlock;
        protected string carPriceFor = "0";
        protected string carPicLink = "#";
        protected string ImgLink = string.Empty;
        protected int PhotoCount = 0;
        protected string PicUrl = string.Empty;
        protected string CarPicName = string.Empty;


        // 节能补贴
        protected string hasEnergySubsidy = string.Empty;
        //减税车船使用税
        protected string strTravelTax = "";

        // 车型焦点图
        protected string carFocusImageHtml = string.Empty;
        protected string SerialToSerialHtml = string.Empty;
        // 车型热门对比车型
        protected string carHotCompareHtml = string.Empty;
        protected string LengthWidthHeight = string.Empty;
        protected string Oil_FuelTabType = string.Empty;
        protected string DoorsSeatNumType = string.Empty;
        protected string EngineAllString = string.Empty;
        protected string OutStat_BodyColor = string.Empty;

        protected double CarReferPriceDecimal = 0;
        protected CarPriceComputer priceComputer;

        protected string UCarHtml = string.Empty;
        protected string carPriceTtileHtml = string.Empty;
        protected string carColorHtml = string.Empty;//车款颜色html块
        protected string carColorUrls = string.Empty;
        protected string carListHtml = string.Empty;

        //车型频道非标准广告(特定子品牌车型贷款链接) 高总 2013.02.27 
        // protected string buyCarLink = string.Empty;

        protected string carFullName = string.Empty;//车款全称
        protected string carName = string.Empty;//车款名称 带年款
        protected string serialAllSpell = string.Empty;//子品牌 全拼
        protected bool isElectrombile = false;//是否是电动车
        protected string batteryCapacity = string.Empty;//电池容量
        protected string powerConsumptive100 = string.Empty;//百公里耗电
        protected string mileage = string.Empty;//续航里程
        protected string chargeTime = string.Empty;//充电时间
        protected string fastChargeTime = string.Empty;//快速充电时间
        protected string exhaust = string.Empty;//排量 判断 增压方式

        protected string ucarPrice = string.Empty;//二手车报价
        protected string carPrice = string.Empty;//参考成交价
        //购置税内容
        protected string TaxContent = string.Empty;
        #endregion

        private Car_BasicBll basicBll;
        private Car_SerialBll serialBLL;
        public CarSummary()
        {
            serialBLL = new Car_SerialBll();
            basicBll = new Car_BasicBll();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
            this.CheckParam();
            this.GetCarData();
        }

        /// <summary>
        /// 取参数
        /// </summary>
        private void CheckParam()
        {
            if (this.Request.QueryString["carID"] != null && this.Request.QueryString["carID"].ToString() != "")
            {
                string carIDStr = this.Request.QueryString["carID"].ToString();
                if (int.TryParse(carIDStr, out carID))
                { }
            }
        }


        /// <summary>
        /// 取数据
        /// </summary>
        private void GetCarData()
        {
            if (carID > 0)
            {
                cfcs = base.GetCarInfoForCarSummaryByCarID(carID);
                priceComputer = new CarPriceComputer(carID);
                priceComputer.ComputeCarPrice();
                priceComputer.LoanPaymentYear = 3;
                priceComputer.ComputeCarAutoLoan();


                cfcs.CarTotalPrice = priceComputer.FormatTotalPrice;
                if (double.TryParse(cfcs.ReferPrice, out CarReferPriceDecimal))
                {
                    if (CarReferPriceDecimal > 0)
                    { CarReferPriceDecimal = CarReferPriceDecimal * 10000; }
                }

                // modified by chengl Nov.9.2009
                if (cfcs.CarID <= 0)
                {
                    Response.Redirect("/404error.aspx?info=无效车型");
                }

                decimal tempCarPrice = 0;
                if (cfcs.ReferPrice != "")
                {
                    if (decimal.TryParse(cfcs.ReferPrice, out tempCarPrice))
                    {
                        carPriceFor = Convert.ToString(Math.Round(tempCarPrice * 10000));
                    }
                }

                #region 车型及子品牌信息
                cbe = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carID);

                carFullName = cbe.Serial.Name + " " + (cbe.CarYear > 0 ? cbe.CarYear + "款 " : "") + cfcs.CarName;
                carName = (cbe.CarYear > 0 ? cbe.CarYear + "款 " : "") + cfcs.CarName;

                serialAllSpell = cbe.Serial.AllSpell;
                // 广告
                base.MakeSerialTopADCode(cbe.Serial.Id);

                //fuelString = basicBll.GetCarNetfriendsFuel(carID);
                //if (fuelString != "无")
                //{ fuelString = "<a href=\"http://car.bitauto.com/" + cbe.Serial.AllSpell.ToLower() + "/youhao/\" target=\"_blank\">" + fuelString + "/100km</a>"; }

                Dictionary<int, string> dict = basicBll.GetCarAllParamByCarID(carID);

                // 节能补贴 Sep.2.2010 [2012-04-09 样式修改]
                bool isHasEnergySubsidy = basicBll.CarHasParamEx(carID, 853);
                //modified by sk 2015.01.30 只显示 第七 八 批 补贴批次
                //modified by sk 2015.06.02 只显示 第9批 补贴批次
                if ((dict.ContainsKey(963) && (dict[963] == "第10批")) && isHasEnergySubsidy)
                {
                    hasEnergySubsidy = "<a target=\"_blank\" title=\"可获得3000元节能补贴\" href=\"http://news.bitauto.com/others/20150605/1006534895.html\" class=\"butie\">补</a>";
                }

                //if (dict.ContainsKey(895))
                //{
                //    strTravelTax = " <a target=\"_blank\" title=\"{0}\" href=\"http://news.bitauto.com/others/20120308/0805618954.html\" class=\"jianpai\"><span>减</span></a>";
                //    if (dict[895] == "减半")
                //        strTravelTax = string.Format(strTravelTax, "减征50%车船使用税");
                //    else if (dict[895] == "免征")
                //        strTravelTax = string.Format(strTravelTax, "免征车船使用税");
                //    else
                //        strTravelTax = "";
                //}
                //减税 购置税
                double dEx = 0.0;
                Double.TryParse(cfcs.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
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
                    TaxContent = "购置税减半";
                }
                //add by 2014.05.04 电动车参数
                isElectrombile = dict.ContainsKey(578) && dict[578] == "电力" ? true : false;
                batteryCapacity = dict.ContainsKey(876) ? dict[876] : "";
                powerConsumptive100 = dict.ContainsKey(868) ? dict[868] : "";
                mileage = dict.ContainsKey(883) ? dict[883] : "";
                chargeTime = dict.ContainsKey(879) ? dict[879] : "";
                fastChargeTime = dict.ContainsKey(878) ? dict[878] : "";
                exhaust = (dict.ContainsKey(425) && dict[425] == "增压") ? cfcs.Engine_Exhaust.Replace("L", "T") : cfcs.Engine_Exhaust;
                //论坛Url
                // forumUrl = new Car_SerialBll().GetForumUrlBySerialId(cbe.Serial.Id);
                #endregion

                //#region 子品牌默认图(modified by chengl Apr.8.2011)

                //// 如果有车型封面就取车型封面 没有则用子品牌的封面 
                //Dictionary<int, string> dic = new Car_BasicBll().GetCarDefaultPhotoDictionary(4);
                //if (dic.ContainsKey(cbe.Car_Id))
                //{
                //    // 有车型封面
                //    csDefaultPic = dic[cbe.Car_Id];
                //    string RegexString = "_([0-9]+)_4.";
                //    Regex r = new Regex(RegexString);
                //    string[] imgGroup = r.Split(csDefaultPic);
                //    if (imgGroup.Length == 3)
                //    {
                //        carPicLink = "http://photo.bitauto.com/picture/" + cbe.Serial.Id.ToString() + "/" + cbe.Car_Id.ToString() + "/" + imgGroup[1] + "/";
                //    }
                //}
                //else
                //{
                //    // 取子品牌图片
                //    base.GetSerialPicAndCountByCsID(cbe.Serial.Id, out csDefaultPic, out csPicCount, false);
                //    if (csDefaultPic != "")
                //    {
                //        csDefaultPic = csDefaultPic.Replace("_2.", "_4.");
                //    }
                //    else
                //    {
                //        csDefaultPic = WebConfig.DefaultCarPic;
                //    }
                //    string RegexString = "_([0-9]+)_4.";
                //    Regex r = new Regex(RegexString);
                //    string[] imgGroup = r.Split(csDefaultPic);
                //    if (imgGroup.Length == 3)
                //    {
                //        carPicLink = "http://photo.bitauto.com/picture/" + cbe.Serial.Id.ToString() + "/" + imgGroup[1] + "/";
                //    }

                //}
                //#endregion

                //生成车型报价
                RenderCarPriceHtml();

                // 车型所属子品牌图片
                // del by chengl Jul.7.2011
                // GetCarPhotoData(cbe.Serial.Id, cbe.Car_Id);

                // 车型热门新闻
                //RenderHotNews(ref HotNews);
                //hotSerialNews = HotNews.ToString();

                //获取车款下拉列表
                MakeCarListHtml();
                // 热门车型
                this.GetHotCar();
                // 车型答疑
                //carAsk = this.RenderAskHandler();
                // 竞争车型
                // CarHotCompare = GetHotCarCompare();
                // 子品牌口碑数据
                //CarCsKouBei = GetSerialKouBei();
                //和车主聊聊
                //GetUserBlockByCarSerialId();
                // 子品牌点评数据

                // add Jul.6.2011 增加车型焦点图
                GetCarFocusImage();

                // add Jul.6.2011 增加车型热门对比车型
                GetCarHotCompare();

                // add Jul.6.2011 增加车型详细参数
                GetCarAllParam();

                // add Jul.7.2011 增加子品牌还关注
                RenderSerialToSerial();

                //add 车型频道非标准广告(特定子品牌车型贷款链接) 高总 2013.02.27 
                // GetBuyCarLink();

                // 头
                //bool isSuccess = false;
                //CarHeadHTML = this.GetRequestString(string.Format(WebConfig.HeadForCar, carID.ToString(), "CarSummary"), 10, out isSuccess);
                string subDir = Convert.ToString(carID / 1000);
                // CarHeadHTML = base.GetCommonNavigation("CarSummary\\" + subDir, carID);
                CarHeadHTML = base.GetCommonNavigation("CarSummary", carID);

                //UCarHtml = new Car_SerialBll().GetUCarHtml(cbe.SerialId);

                ////官方或综合工况油耗
                //if (cfcs.CarSummaryFuelCost.Length > 0)
                //    summaryFuelHtml = "<li><span>综合工况油耗：</span>" + cfcs.CarSummaryFuelCost + "</li>";
                //else
                //    summaryFuelHtml = "<li><span>官方油耗：</span>" + cfcs.PerfFuelCostPer100 + "</li>";
            }
            else
            {
                Response.Redirect("/404error.aspx?info=无效车型");
            }
        }
        ///// <summary>
        ///// 车型频道非标准广告(特定子品牌车型贷款链接) 高总 2013.02.27
        ///// </summary>
        //private void GetBuyCarLink()
        //{
        //    buyCarLink = "http://market.bitauto.com/gmac2010/zxsq.aspx";
        //    List<int> listSerialIdsForBuyCar = new List<int> { 1679, 2583, 1698, 2409, 2190, 1699, 1695, 3316, 3753, 2567, 1769, 2022, 2196, 3037, 1922, 2678, 2856, 2070 };
        //    int serialId = listSerialIdsForBuyCar.Find(id => id == cbe.Serial.Id);
        //    if (serialId > 0)
        //    { buyCarLink = "http://market.bitauto.com/nissan/index.aspx"; }

        //    //// 车型频道非标准广告(特定子品牌车型贷款链接) 高总 2013.03.26 
        //    //listSerialIdsForBuyCar.Clear();
        //    //listSerialIdsForBuyCar = new List<int> { 2611, 3273, 1811, 2674, 1909, 2381, 1765, 2714, 2875, 2593, 2075, 2573, 1798, 2256, 3221, 1802, 1796, 2776, 2370, 2871, 2413, 2710, 2753 };
        //    //serialId = 0;
        //    //serialId = listSerialIdsForBuyCar.Find(id => id == cbe.Serial.Id);
        //    //if (serialId > 0)
        //    //{ buyCarLink = "https://ccclub.cmbchina.com/fincreditweb/Apply/ApplyDetail.aspx?WT.mc_id=C31YCCX051303273"; }
        //}
        /// <summary>
        /// 生成报价Html
        /// modified by chengl Oct.10.2011
        /// </summary>
        private void RenderCarPriceHtml()
        {
            if (cbe.SaleState.Trim() == "停销")
            {
                Dictionary<int, string> dic = basicBll.GetAllUcarPrice();
                if (dic.ContainsKey(carID))
                { ucarPrice = "<a target=\"_blank\" href=\"http://yiche.taoche.com/buycar/b-" + cbe.Serial.AllSpell + "/?page=1&carid=" + carID + "\">" + dic[carID] + "</a>"; }
                else
                { ucarPrice = "暂无报价"; }
            }
            else
            {
                // 非停销车
                if (cfcs.CarPriceRange.Trim().Length == 0)
                { carPrice = "暂无报价"; }
                else
                {
                    string priceUrl = "http://car.bitauto.com/" + cbe.Serial.AllSpell.ToLower() + "/m" + carID + "/baojia/";
                    carPrice = "<a target=\"_blank\" href=\"" + priceUrl + "\">" + cfcs.CarPriceRange.Replace("万", "") + "万</a>";
                }
            }
        }

        private void MakeCarListHtml()
        {
            StringBuilder sb = new StringBuilder();
            List<CarInfoForSerialSummaryEntity> carList;
            if (cbe.Serial.SaleState == "在销")
            {
                carList = basicBll.GetCarBaseListBySerialId(cbe.SerialId);
            }
            else
            {
                carList = basicBll.GetCarBaseListBySerialId(cbe.SerialId, true);
            }
            foreach (var item in carList)
            {
                int year = ConvertHelper.GetInteger(item.CarYear);
                sb.AppendFormat("<li class=\"{3}\"><a href=\"/{0}/m{1}/\">{2}</a></li>",
                    serialAllSpell, item.CarID, (year > 0 ? year + "款 " : "") + item.CarName,
                    carID == item.CarID ? "current" : "");
            }
            carListHtml = sb.ToString();
        }


        /// <summary>
        /// 取子品牌热门车型
        /// </summary>
        private void GetHotCar()
        {
            DataSet ds = base.GetHotCarInfoByCsID(cbe.Serial.Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 1 && ds.Tables[0].Rows[0]["car_id"].ToString().Equals(carID.ToString()))
                { //如果只有一个车型且是当前车型页，整块不显示
                    hotCars = string.Empty;
                    return;
                }
                var hotCarsStr = new StringBuilder();
                hotCarsStr.Append("<div class=\"side_title\">");
                hotCarsStr.Append("<h4>" + cbe.Serial.ShowName.Replace("(进口)", "").Replace("（进口）", "") + "热门车型</h4>");
                hotCarsStr.Append("</div><ul class=\"text-list\">");

                var count = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["car_id"].ToString().Equals(carID.ToString()))
                    {//不显示当前页的车型
                        continue;
                    }
                    if (count >= 5)
                    { break; }
                    hotCarsStr.Append(string.Format("<li><a title=\"{0} {1}\" alt=\"{0} {1}\" href=\"http://car.bitauto.com/{2}/m{3}/\" target=\"_blank\">{0} {1}</a></li>",
                        cbe.Serial.ShowName, ds.Tables[0].Rows[i]["car_name"], cbe.Serial.AllSpell.ToLower(), ds.Tables[0].Rows[i]["car_id"]));
                    //hotCars += "<li><a title=\"" + cbe.Serial.ShowName + " " + ds.Tables[0].Rows[i]["car_name"].ToString() + "\" alt=\"" + cbe.Serial.ShowName + " " + ds.Tables[0].Rows[i]["car_name"].ToString() + "\" href=\"http://car.bitauto.com/" + cbe.Serial.AllSpell.ToLower() + "/m" + ds.Tables[0].Rows[i]["car_id"].ToString() + "/\" target=\"_blank\">";
                    //hotCars += cbe.Serial.ShowName + " " + ds.Tables[0].Rows[i]["car_name"].ToString() + "</a></li>";
                    count++;
                }
                hotCarsStr.Append("</ul><div class=\"clear\"></div>");
                hotCars = hotCarsStr.ToString();
            }
        }

        /// <summary>
        /// 取车型焦点图
        /// add Jul.6.2011
        /// </summary>
        private void GetCarFocusImage()
        {
            XmlDocument doc = basicBll.GetCarDefaultPhoto(cbe.Serial.Id, carID, cbe.CarYear);
            var photoCountDic = basicBll.GetCarPhotoCount();
            if (photoCountDic.ContainsKey(carID))
            {
                PhotoCount = int.Parse(photoCountDic[carID]);
            }
            if (doc != null && doc.HasChildNodes)
            {
                XmlNodeList xnl = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
                if (xnl != null && xnl.Count > 0)
                {
                    ImgLink = xnl[0].Attributes["Link"].Value;
                    PicUrl = xnl[0].Attributes["ImageUrl"].Value;
                    PicUrl = PicUrl.Replace("_2.", "_4.");
                    var xmlCarId = int.Parse(xnl[0].Attributes["CarId"].Value);
                    if (carID != xmlCarId)
                    {
                        string carYear = xnl[0].Attributes["CarYear"].Value;
                        string carModelName = xnl[0].Attributes["CarModelName"].Value;
                        CarPicName = string.Format("当前车款暂无图片，图片显示为:<br>{0}款 {1} ", carYear, carModelName);
                    }
                    else
                    {
                        if (PhotoCount > 0)
                        {
                            CarPicName = string.Format("共{0}张图片", PhotoCount);
                        }
                    }
                }
            }
            else
            {
                // 用子品牌焦点图
                List<SerialFocusImage> imgList = serialBLL.GetSerialFocusImageList(cbe.Serial.Id);
                if (imgList.Count > 0)
                {
                    SerialFocusImage csImg = imgList[0];
                    string bigImgUrl = csImg.ImageUrl;
                    ImgLink = csImg.TargetUrl;
                    if (csImg.ImageId > 0)
                    {
                        PicUrl = String.Format(bigImgUrl, 4);
                        if (PhotoCount > 0)
                        {
                            CarPicName = string.Format("共{0}张图片", PhotoCount);
                        }
                    }
                }
                else
                {
                    PicUrl = WebConfig.DefaultCarPic;
                }

            }
        }



        /// <summary>
        /// 取车型热门对比车型
        /// </summary>
        private void GetCarHotCompare()
        {
            int loop = 0;
            var sbCompare = new StringBuilder();
            DataSet dsCompare = basicBll.GetCarCompareListByCarID(carID);
            if (dsCompare != null && dsCompare.Tables.Count > 0 && dsCompare.Tables[0].Rows.Count > 0)
            {
                sbCompare.AppendLine("<div class=\"c-list-2014 c-list-2014-pop c-list-set\">");
                sbCompare.Append("<div class=\"xg-tit-box\">");
                sbCompare.Append("<h3>网友用它和谁比</h3>");
                sbCompare.Append("<span class=\"button_orange btn-89-20\">");
                string carIDs = carID.ToString();
                for (int i = 0; i < dsCompare.Tables[0].Rows.Count; i++)
                {
                    if (loop >= 4)
                    { break; }
                    if (dsCompare.Tables[0].Rows[i]["cs_id"].ToString() == cbe.Serial.Id.ToString())
                    { continue; }
                    loop++;
                    carIDs += "," + dsCompare.Tables[0].Rows[i]["cCarID"].ToString().Trim();
                }
                if (loop == 0)
                {
                    return;
                }
                sbCompare.Append("<a id=\"duibiAll\" href=\"javascript:void(0)\">全部加入对比</a>");
                sbCompare.Append("</span>");
                sbCompare.Append("</div>");
                sbCompare.Append("<table id=\"duibiTable\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">");
                sbCompare.Append("<tbody>");
                int loop1 = 0;
                for (int j = 0; j < dsCompare.Tables[0].Rows.Count; j++)
                {
                    if (loop1 >= 4)
                    { break; }
                    if (dsCompare.Tables[0].Rows[j]["cs_id"].ToString() == cbe.Serial.Id.ToString())
                    { continue; }
                    loop1++;

                    Dictionary<int, string> dictCarParams = basicBll.GetCarAllParamByCarID(carID);
                    string innerAllSpell = dsCompare.Tables[0].Rows[j]["allSpell"].ToString().Trim().ToLower();
                    string innerCarId = dsCompare.Tables[0].Rows[j]["cCarID"].ToString().Trim();
                    string innerCarYear = dsCompare.Tables[0].Rows[j]["Car_YearType"].ToString().Trim();
                    string innerCsName = dsCompare.Tables[0].Rows[j]["cs_name"].ToString().Trim();
                    string innerCarName = dsCompare.Tables[0].Rows[j]["car_name"].ToString().Trim();
                    dicCarInfos.Add(int.Parse(innerCarId), innerCarName);
                    sbCompare.Append("<tr id=\"car_filter_id_" + innerCarId + "\">");
                    sbCompare.Append("<td width=\"45%\">");
                    sbCompare.Append("<div class=\"pdL10\">");
                    sbCompare.Append("<a href=\"/" + innerAllSpell + "/m" + innerCarId + "/\" target=\"_blank\">" +
                                     innerCsName + " " + innerCarYear + "款 <em>" + innerCarName + "</em></a>");
                    sbCompare.Append("</div>");
                    sbCompare.Append("</td>");
                    if (dictCarParams.Count == 0)
                    {
                        sbCompare.Append("<td width=\"15%\"></td>");
                    }
                    else
                    {

                        sbCompare.Append("<td width=\"15%\">" + (dictCarParams.ContainsKey(724) ? (dictCarParams[724] + "档") : "") + (dictCarParams.ContainsKey(712) ? (dictCarParams[712]) : "") + "</td>");

                    }
                    sbCompare.Append("<td width=\"20%\">");
                    sbCompare.Append("<span>厂商指导价：<a href=\"#\" target=\"_blank\">" + dsCompare.Tables[0].Rows[j]["car_ReferPrice"].ToString().Trim() + "万</a></span>");
                    sbCompare.Append("</td>");
                    sbCompare.Append("<td>");
                    sbCompare.Append("<div id=\"carcompare_btn_new_" + innerCarId + "\" class=\"button_gray db-btn\"><a target=\"_self\" href=\"javascript:;\" cid=\"" + innerCarId + "\"><span>对比</span></a></div>");
                    sbCompare.Append("</td>");
                    sbCompare.Append("</tr>");
                }
                sbCompare.Append("</tbody>");
                sbCompare.Append("</table>");
                sbCompare.Append("</div>");
                carHotCompareHtml = sbCompare.ToString();
            }
        }

        /// <summary>
        /// 取车型完整参数
        /// </summary>
        private void GetCarAllParam()
        {
            Dictionary<int, string> dic = basicBll.GetCarAllParamByCarID(carID);
            if (dic != null && dic.Count > 0)
            {
                // 长宽高
                string OutSet_Length = dic.ContainsKey(588) ? dic[588] : "暂无";
                string OutSet_Width = dic.ContainsKey(593) ? dic[593] : "暂无";
                string OutSet_Height = dic.ContainsKey(586) ? dic[586] : "暂无";
                LengthWidthHeight = OutSet_Length + "/" + OutSet_Width + "/" + OutSet_Height;

                // 燃油标号
                string srcOil_FuelTab = (dic.ContainsKey(577) && dic[577] != "待查") ? dic[577] : "";
                var Oil_FuelTab = string.Empty;
                // modified by chengl May.31.2012
                switch (srcOil_FuelTab)
                {
                    case "90号": Oil_FuelTab = "(北京89号)"; break;
                    case "93号": Oil_FuelTab = "(北京92号)"; break;
                    case "97号": Oil_FuelTab = "(北京95号)"; break;
                    default: break;
                }
                // modified by chengl Oct.11.2013
                string Oil_FuelType = (dic.ContainsKey(578) && dic[578] != "待查") ? dic[578] : "";
                Oil_FuelTabType = (Oil_FuelType == "" ? "" : Oil_FuelType + "") + srcOil_FuelTab + "<i>" + Oil_FuelTab + "</i>";

                // 车门数 乘员人数 车身型式
                string Body_Doors = dic.ContainsKey(563) ? (dic[563] != "待查" ? dic[563] + "门" : "") : "";
                string Perf_SeatNum = dic.ContainsKey(665) ? dic[665] + "座" : "";
                string Body_Type = dic.ContainsKey(574) ? dic[574] : "";
                DoorsSeatNumType = Body_Doors + Perf_SeatNum + Body_Type;

                // 最大功率—功率值 气缸排列型式 汽缸数
                string Engine_MaxPower = dic.ContainsKey(430) ? dic[430] + "kw " : "";
                string Engine_CylinderRank = dic.ContainsKey(418) ? dic[418] : "";
                if (Engine_CylinderRank.StartsWith("L") || Engine_CylinderRank.StartsWith("V") || Engine_CylinderRank.StartsWith("B") || Engine_CylinderRank.StartsWith("W"))
                { Engine_CylinderRank = Engine_CylinderRank.Substring(0, 1); }
                else
                { Engine_CylinderRank = ""; }
                string Engine_CylinderNum = dic.ContainsKey(417) ? dic[417] : "";
                EngineAllString = Engine_MaxPower + Engine_CylinderRank + Engine_CylinderNum;
                // 车身颜色
                string carColors = dic.ContainsKey(598) ? dic[598].Replace("，", ",") : "";
                List<string> listColor = new List<string>();
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

                // 车型详细参数配置
                carConfigData = this.GetCarConfigurationForCarSummaey(carID, carFullName, cbe.Serial.AllSpell);

                string topRGBHTML = "";
                string topRGBTitle = "";
                //new Car_SerialBll().GetCarColorRGBByCsID(cbe.Serial.Id, cbe.CarYear, 1, 13, "top", listColor, out topRGBHTML, out topRGBTitle);
                //OutStat_BodyColor = "<span class=\"c\">" + topRGBHTML + "</span>";
                //卡片区颜色块
                MakeSerialYearColorHtml(listColor);

                //if (topRGBHTML.Length > 0)
                //{
                serialBLL.GetCarColorRGBByCsID(cbe.Serial.Id, cbe.CarYear, 1, 20, "bottom", listColor, out topRGBHTML, out topRGBTitle);
                carConfigData = carConfigData.Replace("<!--车身颜色-->", topRGBHTML);
                //}

            }
        }

        protected string ColorImageUrl = string.Empty;

        /// <summary>
        /// 生成颜色Hmtl
        /// </summary>
        private void MakeSerialYearColorHtml(List<string> colorNameList)
        {
            var sb = new StringBuilder();
            var sbImageUrl = new StringBuilder();
            var colorList = serialBLL.GetColorRGBBySerialId(cbe.Serial.Id, 0, colorNameList);
            //Dictionary<string, XmlNode> dic = serialBLL.GetSerialColorPhotoByCsID(cbe.SerialId, 0);//图库数据 子品牌颜色图片
            var maxColorNum = 12;
            if (colorList.Count > 0)
            {
                foreach (var colorEntity in colorList.Take(maxColorNum))
                {
                    //if (dic.ContainsKey(colorEntity.ColorName))
                    //{
                    //    var imageUrl = dic[colorEntity.ColorName].Attributes["ImageUrl"].Value.Trim();
                    //    imageUrl = imageUrl.Replace("_5.", "_3.");
                    //    sbImageUrl.Append("<div class=\"img-wrap-b\" style=\"display: none;\">");
                    //    sbImageUrl.Append("<img src=\"" + imageUrl + "\">");
                    //    sbImageUrl.Append("</div>");
                    //}

                    var imageUrl = colorEntity.ColorImageUrl;
                    imageUrl = imageUrl.Replace("_5.", "_3.");
                    sbImageUrl.Append("<div class=\"img-wrap-b\" style=\"display: none;\">");
                    sbImageUrl.Append("<img src=\"" + imageUrl + "\" alt=\"\" />");
                    sbImageUrl.Append("</div>");

                    if (string.IsNullOrEmpty(colorEntity.ColorLink))
                        sb.AppendFormat("<span><em><b style=\"background: {0}\" title=\"{1}\"></b></em></span>", colorEntity.ColorRGB, colorEntity.ColorName);
                    else
                    {
                        sb.Append("<span>");
                        sb.Append("<em>");
                        sb.AppendFormat("<a title=\"" + colorEntity.ColorName + "\" href=\"{1}\" target=\"_blank\"><b style=\"background: {0}\"></b></a>", colorEntity.ColorRGB, colorEntity.ColorLink);
                        sb.Append("<div class=\"tc tc-color-box\" style=\"display:none; \">");
                        sb.Append("<div class=\"tc-box tc-color\">");
                        sb.Append("<i></i>");
                        sb.AppendFormat("<a target=\"_blank\" href=\"{0}\">", colorEntity.ColorLink);
                        sb.AppendFormat("<img src=\"{0}\" alt=\"\" width=\"150\" height=\"100\">", colorEntity.ColorImageUrl.Replace("_5.", "_1."));
                        sb.Append("</a>");
                        sb.AppendFormat("<p><a target=\"_blank\" href=\"{0}\">{1}</a></p>", colorEntity.ColorLink, colorEntity.ColorName);
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</em>");
                        sb.Append("</span>");
                    }
                }
                if (colorList.Count > maxColorNum)
                {
                    sb.Append("<a href=\"javascript:;\" id=\"more-color\" class=\"more\"><strong></strong></a>");
                    sb.Append("<div id=\"more-color-sty\" class=\"color-sty more-color-sty more-pos\" style=\"display:none;\">");
                    foreach (var colorEntity in colorList.Skip(maxColorNum))
                    {
                        //if (dic.ContainsKey(colorEntity.ColorName))
                        //{
                        //    var imageUrl = dic[colorEntity.ColorName].Attributes["ImageUrl"].Value.Trim();
                        //    imageUrl = imageUrl.Replace("_5.", "_3.");
                        //    sbImageUrl.Append("<div class=\"img-wrap-b\" style=\"display: none;\">");
                        //    sbImageUrl.Append("<img src=\"" + imageUrl + "\">");
                        //    sbImageUrl.Append("</div>");
                        //}

                        var imageUrl = colorEntity.ColorImageUrl;
                        imageUrl = imageUrl.Replace("_5.", "_3.");
                        sbImageUrl.Append("<div class=\"img-wrap-b\" style=\"display: none;\">");
                        sbImageUrl.Append("<img src=\"" + imageUrl + "\" alt=\"\" />");
                        sbImageUrl.Append("</div>");

                        if (string.IsNullOrEmpty(colorEntity.ColorLink))
                            sb.AppendFormat("<span><em><b style=\"background: {0}\" title=\"{1}\"></b></em></span>", colorEntity.ColorRGB, colorEntity.ColorName);
                        else
                        {
                            sb.Append("<span>");
                            sb.Append("<em>");
                            sb.AppendFormat("<a title=\"" + colorEntity.ColorName + "\" href=\"{1}\" target=\"_blank\"><b style=\"background: {0}\"></b></a>", colorEntity.ColorRGB, colorEntity.ColorLink);
                            sb.Append("<div class=\"tc tc-color-box\" style=\"display:none; \">");
                            sb.Append("<div class=\"tc-box tc-color\">");
                            sb.Append("<i></i>");
                            sb.AppendFormat("<a target=\"_blank\" href=\"{0}\">", colorEntity.ColorLink);
                            sb.AppendFormat("<img src=\"{0}\" alt=\"\" width=\"150\" height=\"100\">", colorEntity.ColorImageUrl.Replace("_5.", "_1."));
                            sb.Append("</a>");
                            sb.AppendFormat("<p><a target=\"_blank\" href=\"{0}\">{1}</a></p>", colorEntity.ColorLink, colorEntity.ColorName);
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</em>");
                            sb.Append("</span>");
                        }
                    }
                    sb.Append("</div>");
                }
                carColorHtml = sb.ToString();
                carColorUrls = sbImageUrl.ToString();
            }
        }

        /// <summary>
        /// 获取车款参数数据
        /// </summary>
        /// <returns></returns>
        private string GetCarConfigurationForCarSummaey(int carID, string name, string allSpell)
        {
            string result = "";
            StringBuilder sbParameter = new StringBuilder();
            StringBuilder sbTemp = new StringBuilder();
            List<int> listValidCarID = new List<int>();
            listValidCarID.Add(carID);
            Dictionary<int, Dictionary<string, string>> dic = basicBll.GetCarCompareDataByCarIDs(listValidCarID);
            if (!dic.ContainsKey(carID) || dic[carID].Count == 0)
            { return ""; }
            else
            {
                XmlDocument docPC = new XmlDocument();
                string cache = "CarSummary_ParameterConfigurationNew";
                object parameterConfiguration = null;
                CacheManager.GetCachedData(cache, out parameterConfiguration);
                if (parameterConfiguration != null)
                {
                    docPC = (XmlDocument)parameterConfiguration;
                }
                else
                {
                    var filePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfigurationNew.config";
                    if (File.Exists(filePath))
                    {
                        docPC.Load(filePath);
                        CacheManager.InsertCache(cache, docPC, new System.Web.Caching.CacheDependency(filePath), DateTime.Now.AddMinutes(60));
                    }
                }

                // 参数配置
                if (docPC != null && docPC.HasChildNodes)
                {
                    XmlNode rootPC = docPC.DocumentElement;
                    // 显示 参数
                    if (docPC.ChildNodes.Count > 1)
                    {
                        sbParameter.AppendLine("<div id=\"DicCarParameter\" class=\"line-box\">");

                        sbParameter.Append("<div class=\"title-con\">");
                        sbParameter.Append("<div class=\"title-box title-box2\">");
                        sbParameter.AppendFormat("<h4><a href=\"/{0}/m{1}/peizhi/\" target=\"_blank\">参数配置</a></h4><span>注：●标配 ○选配 -无</span>", allSpell, carID);
                        sbParameter.AppendLine("<div class=\"more\"><a data-channelid=\"2.152.1445\" href=\"/" + allSpell + "/m" + carID.ToString() + "/peizhi/\" target=\"_blank\">对比" + cbe.Serial.Name + "全系车型&gt;&gt;</a></div>");

                        sbParameter.Append("</div>");
                        sbParameter.Append("</div>");


                        //sbParameter.AppendLine("<h3><span>" + name + " 参数配置</span><i>注：●标配 ○选配 -无</i></h3>");
                        sbParameter.Append("<div class=\"car_config car_top_set\">");
                        sbParameter.AppendLine("<table cellspacing=\"0\" cellpadding=\"0\"></table>");
                        XmlNode parameter = rootPC.ChildNodes[0];

                        int itemCountFlag = 0;//加一个区分table显示样式的计数器
                        foreach (XmlNode parameterList in parameter)
                        {
                            itemCountFlag++;
                            if (parameterList.NodeType == XmlNodeType.Element)
                            {
                                if (parameterList.HasChildNodes && parameterList.Attributes.GetNamedItem("Type").Value.ToString() == "2")
                                {
                                    if (itemCountFlag == 1)
                                    {
                                        sbParameter.AppendLine("<table>");
                                    }
                                    if (itemCountFlag == 3)
                                    {
                                        sbParameter.AppendLine("<table style=\"display:none\">");
                                    }
                                    GetPartialTableData(sbTemp, parameterList, dic, sbParameter);
                                    if (itemCountFlag == 2)
                                    {
                                        sbParameter.AppendLine("</table>");
                                    }
                                }
                            }
                        }
                        if (itemCountFlag >2) 
                        {
                            sbParameter.AppendLine("</table>");
                        }
                        sbParameter.Append("<div class=\"ts-box\">以上参数配置信息仅供参考，实际请以店内销售车辆为准。如果发现信息有误，<a href=\"javascript:void(0);\" name=\"correcterror\">欢迎您及时指正！</a></div>");

                        sbParameter.AppendLine("</div>");
                        sbParameter.AppendLine("</div>");
                    }
                    result = sbParameter.ToString();
                }
            }
            return result;
        }
        /// <summary>
        /// 组织"参数配置"html结构中 table元素下的thead与tbody元素
        /// </summary>
        /// <param name="sbTemp"></param>
        /// <param name="parameterList"></param>
        /// <param name="dic"></param>
        /// <param name="sbParameter"></param>
        private void GetPartialTableData(StringBuilder sbTemp, XmlNode parameterList, Dictionary<int, Dictionary<string, string>> dic, StringBuilder sbParameter) 
        {
            sbTemp.AppendLine("<thead><tr><th colspan=\"4\">" + parameterList.Attributes.GetNamedItem("Name").Value + " <a href=\"javascript:void(0);\" name=\"correcterror\" class=\"car-config-error\">我要纠错</a></th></tr></thead><tbody>");
            bool isHasChild = false;
            int loopCount = 0;
            XmlNodeList xmlNode = parameterList.ChildNodes;
            foreach (XmlNode item in xmlNode)
            {
                if (item.NodeType != XmlNodeType.Element)
                { continue; }
                //if (dic[carID].ContainsKey(item.Attributes.GetNamedItem("Value").Value)
                //	&& dic[carID][item.Attributes.GetNamedItem("Value").Value] != "待查")
                {
                    string pvalue = string.Empty;
                    //合并参数
                    if (item.Attributes.GetNamedItem("Value").Value.IndexOf(",") != -1)
                    {
                        string[] arrKey = item.Attributes.GetNamedItem("Value").Value.Split(',');
                        string[] arrUnit = item.Attributes.GetNamedItem("Unit").Value.Split(',');
                        string[] arrParam = item.Attributes.GetNamedItem("ParamID").Value.Split(',');
                        List<string> list = new List<string>();
                        for (var i = 0; i < arrKey.Length; i++)
                        {
                            if (!(dic[carID].ContainsKey(arrKey[i]) && dic[carID][arrKey[i]] != "待查"))
                                continue;
                            //档位数 0 不显示
                            if (arrParam[i] == "724")
                            {
                                var d = ConvertHelper.GetInteger(dic[carID][arrKey[i]]);
                                if (d <= 0) continue;
                            }
                            ////CD DVD 
                            //if (arrParam[i] == "510" || arrParam[i] == "490")
                            //{
                            //    if (dic[carID][arrKey[i]].IndexOf("有") != -1)
                            //        continue;
                            //}
                            list.Add(string.Format("{0}{1}", dic[carID][arrKey[i]], arrUnit[i]));
                        }
                        if (list.Count <= 0) continue;
                        //解决2个参数 其中“有” 后面参数有值 替换成 实心圈
                        var you = list.Find(p => p.IndexOf("有") != -1);
                        if (you != null && list.Count > 1)
                            list.Remove(you);
                        //进气形式 2个参数 增压 显示 增压方式 不是则显示 进气形式
                        if (item.Attributes.GetNamedItem("Name").Value == "进气形式")
                        {
                            if (list.Count > 1)
                            {
                                if (list[0] == "增压")
                                    list.RemoveAt(0);
                                else
                                    list.RemoveAt(1);
                            }
                        }
                        pvalue = string.Join(" ", list.ToArray());
                    }
                    else
                    {
                        if (!(dic[carID].ContainsKey(item.Attributes.GetNamedItem("Value").Value)
                    && dic[carID][item.Attributes.GetNamedItem("Value").Value] != "待查")) continue;
                        pvalue = string.Format("{0}{1}", dic[carID][item.Attributes.GetNamedItem("Value").Value], item.Attributes.GetNamedItem("Unit").Value);
                    }

                    isHasChild = true || isHasChild;
                    if (loopCount % 2 == 0)
                    {
                        if (loopCount != 0)
                        {
                            sbTemp.AppendLine("</tr>");
                        }
                        sbTemp.AppendLine("<tr>");
                    }

                    // 牛B的逻辑不硬编码都不行
                    // 燃料类型 汽油的话同时显示 燃油标号
                    string pvalueOther;
                    if (item.Attributes.GetNamedItem("ParamID").Value == "578"
                        && pvalue == "汽油")
                    {
                        if (dic[carID].ContainsKey("CarParams/Oil_FuelTab")
                    && dic[carID]["CarParams/Oil_FuelTab"] != "待查")
                        {
                            pvalueOther = dic[carID]["CarParams/Oil_FuelTab"];
                            switch (pvalueOther)
                            {
                                case "90号": pvalueOther = pvalueOther + "(北京89号)"; break;
                                case "93号": pvalueOther = pvalueOther + "(北京92号)"; break;
                                case "97号": pvalueOther = pvalueOther + "(北京95号)"; break;
                                default: break;
                            }
                            pvalue = pvalue + " " + pvalueOther;
                        }
                    }
                    // 进气型式 如果自然吸气直接显示，如果是增压则显示增压方式
                    if (item.Attributes.GetNamedItem("ParamID").Value == "425"
                        && pvalue == "增压")
                    {
                        if (dic[carID].ContainsKey("CarParams/Engine_AddPressType")
                    && dic[carID]["CarParams/Engine_AddPressType"] != "待查")
                        {
                            pvalueOther = dic[carID]["CarParams/Engine_AddPressType"];
                            pvalue = pvalue + " " + pvalueOther;
                        }
                    }
                    //解决 变速箱 无极变速 替换成 -
                    if (item.Attributes.GetNamedItem("Name").Value != "变速箱")
                    {
                        if (pvalue.IndexOf("有") == 0)
                        { pvalue = "●"; }
                        if (pvalue.IndexOf("选配") == 0)
                        { pvalue = "○"; }
                        if (pvalue.IndexOf("无") == 0)
                        { pvalue = "-"; }
                    }

                    sbTemp.AppendLine("<th>" + item.Attributes.GetNamedItem("Name").Value + "</th>");
                    // 车身颜色呈现特殊化
                    if (item.Attributes.GetNamedItem("Name").Value == "车身颜色")
                    {
                        sbTemp.AppendLine("<td colspan=\"3\" class=\"td-p-sty\"><span class=\"c w530\"><!--车身颜色--></span></td>");
                        loopCount++;
                    }
                    else
                    {
                        sbTemp.AppendLine("<td class=\"td-b-sty td-p-sty\">" + pvalue + "</td>");
                    }
                    //sbTemp.AppendLine("<td>" + pvalue + "</td>");
                    loopCount++;
                }
            }
            if (loopCount % 2 == 1)
            {
                sbTemp.AppendLine("<th></th>");
                sbTemp.AppendLine("<td></td>");
            }
            // 如果有子项
            if (isHasChild)
            {
                sbParameter.AppendLine(sbTemp.ToString() + "</tr></tbody>");
            }
            if (sbTemp.Length > 0)
            { sbTemp.Remove(0, sbTemp.Length); }
        }


        /// <summary>
        /// 子品牌还关注
        /// </summary>
        /// <returns></returns>
        private void RenderSerialToSerial()
        {
            StringBuilder htmlCode = new StringBuilder();
            List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(cbe.Serial.Id, 6);
            if (lsts.Count > 0)
            {

                htmlCode.AppendLine("<ul class=\"pic_list\">");
                int loop = 0;
                foreach (EnumCollection.SerialToSerial sts in lsts)
                {
                    string csName = sts.ToCsShowName.ToString();

                    string shortName = StringHelper.SubString(csName, 12, true);
                    if (shortName.StartsWith(csName))
                        shortName = csName;
                    loop++;
                    htmlCode.AppendFormat("<li>");
                    htmlCode.AppendFormat("<a target=\"_blank\" href=\"/{0}/\"><img src=\"{1}\" alt=\"\" width=\"90\" height=\"60\"></a>",
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
                htmlCode.AppendLine("</ul>");
            }
            SerialToSerialHtml = htmlCode.ToString();
            htmlCode.Remove(0, htmlCode.Length);
        }

        /// <summary>
        /// 得到品牌下的其他子品牌
        /// </summary>
        /// <returns></returns>
        protected string GetBrandOtherSerial()
        {
            List<CarSerialPhotoEntity> carSerialPhotoList = new Car_BrandBll().GetCarSerialPhotoListByCBID(cbe.Serial.BrandId, false);

            carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

            if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
            {
                return "";
            }

            int forLastCount = 0;
            foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
            {
                if (entity.SerialLevel == "概念车" || entity.SerialId == cbe.Serial.Id)
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
                if (entity.SerialLevel == "概念车" || entity.SerialId == cbe.Serial.Id)
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
                    cbe.Serial.Brand.AllSpell, cbe.Serial.Brand.Name);
                brandOtherSerial.Append("</div>");

                brandOtherSerial.Append("<ul class=\"text-list\">");

                brandOtherSerial.Append(contentBuilder.ToString());

                brandOtherSerial.Append("</ul>");
            }

            return brandOtherSerial.ToString();
        }
    }
}