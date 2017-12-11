using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace BitAuto.CarChannel.CarchannelWeb.PageCarV2
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
       // protected string carFuelType = ["汽油", "柴油", "纯电动", "油电混合", "插电混合", "客车", "卡车", "天然气"];
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
                    TaxContent = "购置税75折";
                }
                //add by 2014.05.04 电动车参数
                isElectrombile = dict.ContainsKey(578) && dict[578] == "纯电" ? true : false;
                batteryCapacity = dict.ContainsKey(876) ? dict[876] : "";
                powerConsumptive100 = dict.ContainsKey(868) ? dict[868] : "";
                mileage = dict.ContainsKey(883) ? dict[883] : "";
                chargeTime = dict.ContainsKey(879) ? dict[879] : "";
                fastChargeTime = dict.ContainsKey(878) ? dict[878] : "";
                exhaust = (dict.ContainsKey(425) && dict[425].Contains("增压")) ? cfcs.Engine_Exhaust.Replace("L", "T") : cfcs.Engine_Exhaust;
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
                { ucarPrice = "<a target=\"_blank\" href=\"http://yiche.taoche.com/buycar/b-" + cbe.Serial.AllSpell + "/?page=1&carid=" + carID + "&ref=pc_yc_cxzs_gs_escjg\"><em>" + dic[carID] + "</em></a>"; }
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
                    carPrice = "<a target=\"_blank\" href=\"" + priceUrl + "\"><em id=\"car-area-price\">" + cfcs.CarPriceRange.Replace("万", "") + "万</em></a>";
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
                sb.AppendFormat("<a href=\"/{0}/m{1}/\" class=\"{3}\">{2}</a>",
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
                hotCarsStr.Append("<h3 class=\"top-title\">" + cbe.Serial.ShowName.Replace("(进口)", "").Replace("（进口）", "") + "热门车款</h3>");
                hotCarsStr.Append("<div class=\"list-txt list-txt-s list-txt-default list-txt-style5\"><ul>");
                var count = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["car_id"].ToString().Equals(carID.ToString()))
                    {//不显示当前页的车型
                        continue;
                    }
                    if (count >= 5)
                    { break; }
                    //<li><div class="txt"><a href="">标致207两厢</a></div><span>10.56万</span></li>
                    hotCarsStr.Append(string.Format("<li><div class=\"txt\"><a title=\"{0} {1}\" alt=\"{0} {1}\" href=\"http://car.bitauto.com/{2}/m{3}/\" target=\"_blank\">{5}款 {1}</a></div><span>{4}万</span></li>",
                        cbe.Serial.ShowName, ds.Tables[0].Rows[i]["car_name"], cbe.Serial.AllSpell.ToLower(), ds.Tables[0].Rows[i]["car_id"], ds.Tables[0].Rows[i]["car_ReferPrice"], ds.Tables[0].Rows[i]["Car_YearType"]));
                    count++;
                }
                hotCarsStr.Append("</ul></div>");
                hotCars = hotCarsStr.ToString();
            }
        }
        /*
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
        */
        /// <summary>
        /// 取车型焦点图
        /// add Jul.6.2011
        /// </summary>
        private void GetCarFocusImage()
        {
            //XmlDocument doc = basicBll.GetCarDefaultPhoto(cbe.Serial.Id, carID, cbe.CarYear);
            var photoCountDic = basicBll.GetCarPhotoCount();
            if (photoCountDic.ContainsKey(carID))
            {
                PhotoCount = int.Parse(photoCountDic[carID]);
            }
            Dictionary<int, XmlElement> carCoverImg = basicBll.GetCarDefaultPhotoXmlElement();
            int imageId = 0;
            if (carCoverImg != null && carCoverImg.ContainsKey(carID))
            {
                XmlElement imageItem = carCoverImg[carID];
                imageId = ConvertHelper.GetInteger(imageItem.Attributes["ImageId"].Value);
                if (imageId > 0)
                {
                    ImgLink = string.Format("http://photo.bitauto.com/picture/{0}/{1}/"
                        , cbe.SerialId
                        , imageItem.Attributes["ImageId"].Value);//imageItem.Attributes["Link"].Value;
                    PicUrl = imageItem.Attributes["ImageUrl"].Value;
                    PicUrl = PicUrl.Replace("_2.", "_4.");
                    CarPicName = string.Format("共{0}张图片", PhotoCount);
                }
            }
            if (imageId == 0)
            {
                // 用子品牌焦点图
                List<SerialFocusImage> imgList = serialBLL.GetSerialFocusImageList(cbe.Serial.Id);
                if (imgList.Count > 0)
                {
                    SerialFocusImage csImg = imgList[0];
                    PicUrl = string.Format(csImg.ImageUrl, 4);
                    ImgLink = csImg.TargetUrl;
                    CarPicName = string.Format("当前车款暂无图片，图片显示为:<br>{0}", csImg.CarName);
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
                sbCompare.AppendLine("<div class=\"layout-2 carcompare-section\">");
                sbCompare.AppendLine("<div class=\"section-header header2 mbl\">");
                sbCompare.AppendLine("<div class=\"box\"><h2>网友用它和谁比</h2></div>");
                sbCompare.AppendLine("<div class=\"more\"><a href=\"/chexingduibi/\" target=\"_blank\">对比工具&gt;&gt;</a></div>");
                sbCompare.AppendLine("</div>");
                sbCompare.AppendLine("<div class=\"row col3-260-box\">");
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
             
                int loop1 = 0;
                for (int j = 0; j < dsCompare.Tables[0].Rows.Count; j++)
                {
                    if (loop1 >= 3)
                    { break; }
                    if (dsCompare.Tables[0].Rows[j]["cs_id"].ToString() == cbe.Serial.Id.ToString())
                    { continue; }
                    loop1++;

                    Dictionary<int, string> dictCarParams = basicBll.GetCarAllParamByCarID(carID);
                    string serialName = cbe.Serial.ShowName;
                    string serialSpell = cbe.Serial.AllSpell;
                    int innerCsId =Convert.ToInt32(dsCompare.Tables[0].Rows[j]["cs_id"].ToString().Trim());
                    //string innerCsShowName = dsCompare.Tables[0].Rows[j]["cs_showname"].ToString().Trim();
                    string compareImage = serialBLL.GetSerialInfoCard(innerCsId).CsDefaultPic;

                    string innerAllSpell = dsCompare.Tables[0].Rows[j]["allSpell"].ToString().Trim().ToLower();
                    string innerCarId = dsCompare.Tables[0].Rows[j]["cCarID"].ToString().Trim();
                    string innerCarYear = dsCompare.Tables[0].Rows[j]["Car_YearType"].ToString().Trim();
                    string innerCsName = dsCompare.Tables[0].Rows[j]["cs_name"].ToString().Trim();
                    string innerCarName = dsCompare.Tables[0].Rows[j]["car_name"].ToString().Trim();
                    string referPrice= dsCompare.Tables[0].Rows[j]["car_ReferPrice"].ToString().Trim();
                    dicCarInfos.Add(int.Parse(innerCarId), innerCarName);
                    sbCompare.Append("<div class=\"special-layout-18\">");
                    sbCompare.Append("<div class=\"figure\">");
                    sbCompare.Append(string.Format("<a href=\"/chexingduibi/?carids={0},{1}\" target=\"_blank\" class=\"clearfix\">", carID, innerCarId));
                    sbCompare.Append("<div class=\"left\"><img src=\""+cbe.Serial.DefaultPic+"\" alt=\"\"></div>");
                    sbCompare.Append("<div class=\"right\"><img src=\""+compareImage+"\" alt=\"\"></div>");
                    sbCompare.Append("<span class=\"ico\"></span>");
                    sbCompare.Append("</a>");
                    sbCompare.Append("</div>");
                    sbCompare.Append("<div class=\"desc\">");
                    sbCompare.Append("<h6><a href=\"/"+innerAllSpell+"/m"+innerCarId+"/\" class=\"title\" target=\"_blank\">" + innerCsName + "</a></h6>");
                    sbCompare.Append("<a href=\"/"+innerAllSpell+"/m"+innerCarId+"/\" target=\"_blank\"><p class=\"type\">"+ innerCarYear + "款 " + innerCarName+"</p></a>");
                    sbCompare.Append("<p class=\"price\">厂商指导价：<em>" + (!string.IsNullOrEmpty(referPrice) ? referPrice + "万" : "暂无") + "</em></p>");
                    sbCompare.Append("<div class=\"action\" id=\"carcompare_btn_new_"+innerCarId+"\">");
                    sbCompare.Append("<a class=\"btn btn-secondary btn-sm\" target=\"_blank\" href=\"/chexingduibi/?carids=" + carID + "," + innerCarId + "\" cid=\"" + innerCarId + "\">对比</a>");
                    sbCompare.Append("</div>");
                    sbCompare.Append("</div>");
                    sbCompare.Append("</div>");
                }
                sbCompare.Append("</div>");
                sbCompare.Append("</div>");
                carHotCompareHtml = sbCompare.ToString();
            }
        }

        /// <summary>
        /// 取车型完整参数
        /// </summary>
        private void GetCarAllParam()
        {
            carConfigData = GetCarConfigurationForCarSummary(carID, carFullName, cbe.Serial.AllSpell);
            //Dictionary<int, string> dic = basicBll.GetCarAllParamByCarID(carID);
            //if (dic != null && dic.Count > 0)
            //{
            //    // 车身颜色
            //    string carColors = dic.ContainsKey(598) ? dic[598].Replace("，", ",") : "";
            //    List<string> listColor = new List<string>();
            //    if (carColors != "")
            //    {
            //        string[] colorArray = carColors.Split(',');
            //        if (colorArray.Length > 0)
            //        {
            //            foreach (string color in colorArray)
            //            {
            //                if (!listColor.Contains(color))
            //                { listColor.Add(color); }
            //            }
            //        }
            //    }

            //    // 车型详细参数配置
                

            //    string topRGBHTML = "";
            //    string topRGBTitle = "";
                //new Car_SerialBll().GetCarColorRGBByCsID(cbe.Serial.Id, cbe.CarYear, 1, 13, "top", listColor, out topRGBHTML, out topRGBTitle);
                //OutStat_BodyColor = "<span class=\"c\">" + topRGBHTML + "</span>";
                //卡片区颜色块
                //MakeSerialYearColorHtml(listColor);

                //if (topRGBHTML.Length > 0)
                //{
                //serialBLL.GetCarColorRGBByCsIDFor1200(cbe.Serial.Id, cbe.CarYear, 1, 20, "bottom", listColor, out topRGBHTML, out topRGBTitle);
               // carConfigData = carConfigData.Replace("#colorblock#", strColorHtmlBlock);
               // carConfigData = carConfigData.Replace("<!--车身颜色-->", topRGBHTML);
                //}

            //}
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
        private string GetCarConfigurationForCarSummary(int carID, string name, string allSpell)
        {
            string result = "";
            string fristTableHtml = string.Empty;
            string otherTablesHtml = string.Empty;
            StringBuilder sbParameter = new StringBuilder();
            StringBuilder sbTemp = new StringBuilder();
            List<int> listValidCarID = new List<int>();
            listValidCarID.Add(carID);
            Dictionary<int, Dictionary<string, string>> dic = basicBll.GetCarCompareDataWithOptionalByCarIDs(listValidCarID);
            if (!dic.ContainsKey(carID) || dic[carID].Count == 0)
            { return ""; }
            else
            {
                XmlDocument docPC = new XmlDocument();
                string cache = "CarSummary_ParameterConfigurationNewV2";
                object parameterConfiguration = null;
                CacheManager.GetCachedData(cache, out parameterConfiguration);
                if (parameterConfiguration != null)
                {
                    docPC = (XmlDocument)parameterConfiguration;
                }
                else
                {
                    var filePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfigurationNewV2.config";
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
                                        GetPartialTableData(sbTemp, parameterList, dic, sbParameter, true);
                                        fristTableHtml = sbParameter.ToString();
                                        sbParameter = sbParameter.Remove(0, sbParameter.Length);
                                    }
                                    else
                                    {
                                        GetPartialTableData(sbTemp, parameterList, dic, sbParameter, false);
                                    }
                                }
                            }
                        }
                    }
                    else { return null; }
                    otherTablesHtml = sbParameter.ToString();
                    if (!string.IsNullOrEmpty(otherTablesHtml))
                    {
                        result = fristTableHtml + "<div class=\"moreinfo\" style=\"display:none\">" + otherTablesHtml + "</div>";
                    }
                }
            }
            return result;
        }
        private string strColorHtmlBlock = string.Empty;
        /// <summary>
        /// 组织"参数配置"html结构中 table元素下的thead与tbody元素
        /// </summary>
        /// <param name="sbTemp"></param>
        /// <param name="parameterList"></param>
        /// <param name="dic"></param>
        /// <param name="sbParameter"></param>
        private void GetPartialTableData(StringBuilder sbTemp, XmlNode parameterList, Dictionary<int, Dictionary<string, string>> dic, StringBuilder sbParameter,bool isFirstTable) 
        {
            sbTemp.AppendLine("<div class=\"caption-1\"><h6>" + parameterList.Attributes.GetNamedItem("Name").Value + "</h6></div>");
            sbTemp.AppendLine("<div class=\"special-layout-18 layout-1\">");
            sbTemp.AppendLine("<table>");
            sbTemp.AppendLine("<colgroup>");
            sbTemp.AppendLine("<col width=\"15%\">");
            sbTemp.AppendLine("<col width=\"18%\">");
            sbTemp.AppendLine("<col width=\"15%\">");
            sbTemp.AppendLine("<col width=\"19%\">");
            sbTemp.AppendLine("<col width=\"15%\">");
            sbTemp.AppendLine("<col width=\"18%\">");
            sbTemp.AppendLine("</colgroup>");
            sbTemp.AppendLine("<tbody>");
  
            bool isHasChild = false;
            int loopCount = 0;
            XmlNodeList xmlNode = parameterList.ChildNodes;
            foreach (XmlNode item in xmlNode)
            {
                if (item.NodeType != XmlNodeType.Element)
                { continue; }
                //if (dic[carID].ContainsKey(item.Attributes.GetNamedItem("Value").Value)
                //	&& dic[carID][item.Attributes.GetNamedItem("Value").Value] != "待查")
                    string pvalue = string.Empty;
                //合并参数 燃油变速箱
                if (parameterList.Attributes.GetNamedItem("Name").Value == "基本信息" && (item.Attributes.GetNamedItem("ParamID").Value == "724" || item.Attributes.GetNamedItem("ParamID").Value == "712"))
                {
                    if (item.Attributes.GetNamedItem("ParamID").Value == "724")
                    {
                        continue;
                    }
                    string[] arrKey = new string[2];
                    string[] arrUnit = new string[2];
                    string[] arrParam = new string[2];                    
                    if (item.PreviousSibling.NodeType == XmlNodeType.Element && item.PreviousSibling != null)
                    {
                        arrKey[0] = item.PreviousSibling.Attributes.GetNamedItem("Value").Value;
                        arrUnit[0] = item.PreviousSibling.Attributes.GetNamedItem("Unit").Value;
                        arrParam[0] = item.PreviousSibling.Attributes.GetNamedItem("ParamID").Value;
                    }
                    arrKey[1] = item.Attributes.GetNamedItem("Value").Value;
                    arrUnit[1] = item.Attributes.GetNamedItem("Unit").Value;
                    arrParam[1] = item.Attributes.GetNamedItem("ParamID").Value;

                    List<string> list = new List<string>();
                    for (var i = 0; i < arrKey.Length; i++)
                    {
                        if (!(dic[carID].ContainsKey(arrKey[i]) && dic[carID][arrKey[i]] != "待查"))
                            continue;
                        //档位数 0 不显示
                        if (arrParam[i] == "724")
                        {
                            var d = ConvertHelper.GetInteger(dic[carID][arrKey[i]]);
                            var t = dic[carID].ContainsKey(arrKey[i + 1]) ? dic[carID][arrKey[i + 1]].Trim() : "";
                            if (d <= 0 || t == "单速变速箱" || t == "E-CVT无级变速" || t == "CVT无级变速" || t == "")
                            {
                                continue;
                            }
                        }
                        //变速箱类型 变速箱为空不显示变速箱与挡位
                        if (arrParam[i] == "712")
                        {
                            var t = dic[carID][arrKey[i]];
                            if (string.IsNullOrEmpty(t))
                            {
                                if (list.Count == 1)
                                {
                                    list.RemoveAt(0);
                                }
                                continue;
                            }
                        }
                        list.Add(string.Format("{0}{1}", dic[carID][arrKey[i]], arrUnit[i]));
                    }
                    if (list.Count <= 0) continue;
                    //解决2个参数 其中“有” 后面参数有值 替换成 实心圈
                    var you = list.Find(p => p.IndexOf("有") != -1);
                    if (you != null && list.Count > 1)
                        list.Remove(you);
                    pvalue = string.Join(" ", list.ToArray());
                }
                else
                    {
                    if (!(dic[carID].ContainsKey(item.Attributes.GetNamedItem("Value").Value)))
                       { continue; }
                        if (dic[carID].ContainsKey(item.Attributes.GetNamedItem("Value").Value)&& dic[carID][item.Attributes.GetNamedItem("Value").Value] != "待查")
                        {
                            pvalue = dic[carID][item.Attributes.GetNamedItem("Value").Value];
                        }
                    }

                    isHasChild = true || isHasChild;
                    if (loopCount % 3 == 0)
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
                            //switch (pvalueOther)
                            //{
                            //    case "90号": pvalueOther = pvalueOther + "(北京89号)"; break;
                            //    case "93号": pvalueOther = pvalueOther + "(北京92号)"; break;
                            //    case "97号": pvalueOther = pvalueOther + "(北京95号)"; break;
                            //    default: break;
                            //}
                            pvalue = pvalue + " " + pvalueOther;
                        }
                    }

                //sbTemp.AppendLine("<td><span class=\"title\">" + item.Attributes.GetNamedItem("Name").Value + ":</span></td>");

                //Note:设页面中2个td为一组，一行有3组，颜色块占1行中的2组,并且补齐1行中的6个td保持行线完整
                // 车身颜色呈现特殊化
                if (item.Attributes.GetNamedItem("Name").Value == "车身颜色")
                {
                    strColorHtmlBlock = "<td><span class=\"title\">" + item.Attributes.GetNamedItem("Name").Value + "：</span></td>";
                    strColorHtmlBlock += "<td colspan=\"3\"><div class=\"focus-color-warp\"><ul id=\"color-listbox\">";
                    var pvalueColorList = pvalue.Split('|');
                    foreach (var color in pvalueColorList)
                    {
                        if (color.IndexOf(",") != -1)
                        {                          
                            string colorRGB = color.Split(',')[1];
                            var title = color.Split(',')[0];
                            strColorHtmlBlock += "<li><a href=\"javascript:void(0);\" title=\"" + title.Trim() + "\"><span style=\"background:" + colorRGB.Trim() + "\"></span></a></li>";
                        }                       
                    }
                    strColorHtmlBlock += "</ul></div></td>";

                    if (loopCount != 0)
                    {
                        if (loopCount % 3 == 2)   //余1组td时，不足以填充颜色块，则补充当前行，并将颜色块移到下一行
                        {
                            sbTemp.AppendLine("<td></td><td></td></tr><tr>#colorblock#");
                            loopCount += 3;
                        }
                        else   //余0或2组td时，将颜色块直接填充到当前行
                        {
                            sbTemp.AppendLine("#colorblock#");
                            loopCount += 2;
                        }
                    }
                    else   //颜色块占 首行的前两组时
                    {
                        sbTemp.AppendLine("#colorblock#");
                        loopCount += 2;
                    }
                    if (pvalueColorList.Length > 0)
                    {
                        sbTemp.Replace("#colorblock#", strColorHtmlBlock);
                    }                    
                }
                else
                {
                    sbTemp.AppendLine("<td><span class=\"title\">" + item.Attributes.GetNamedItem("Name").Value + "：</span></td>");
                    if (pvalue.IndexOf(",") == -1)
                    {
                        //解决 变速箱挡位合并 单位的问题
                        if (item.Attributes.GetNamedItem("ParamID").Value != "712")
                        {
                            if (pvalue.IndexOf("有") == 0)
                            { pvalue = "●"; }
                            if (pvalue =="选配")
                            { pvalue = "○"; }
                            if (pvalue == "无")
                            { pvalue = "-"; }

                            pvalue = string.Format("{0}{1}", pvalue, item.Attributes.GetNamedItem("Unit").Value);
                        }
                       
                        if (pvalue.IndexOf("|") == -1)
                        {
                            sbTemp.AppendLine("<td><span class=\"info\">" + pvalue + "</span></td>");
                        }
                        else
                        {
                            var name = pvalue.Split('|')[0];
                            string price = Convert.ToSingle(pvalue.Split('|')[1]).ToString("N0");
                            sbTemp.AppendLine("<td><div class=\"info\"><div class=\"optional type2\"><div class=\"l\"><i>○</i>" + name + " " + price + "元</div></div></div></td>");
                        }
                    }
                    else
                    {
                        var pvalueList = pvalue.Split(',');
                        sbTemp.AppendLine("<td><div class=\"info\">");
                        foreach (var pval in pvalueList)
                        {
                            if (pval.IndexOf("|") == -1)
                            {
                                if (pval != "无")
                                {
                                    sbTemp.AppendLine("<div class=\"optional type2 std\"><div class=\"l\"><i>●</i>" + pval + "</div></div>");
                                }
                            }
                            else
                            {
                                var name = pval.Split('|')[0];
                                string price = Convert.ToSingle(pval.Split('|')[1]).ToString("N0");
                                sbTemp.AppendLine("<div class=\"optional type2\"><div class=\"l\"><i>○</i>" + name + " " + price + "元</div></div>");
                            }
                        }
                        sbTemp.AppendLine("</div></td>");

                    }
                   
                    loopCount++;
                }
            }
            if (loopCount != 0)   //处理一行结尾的tr
            {
                if (loopCount % 3 != 0)
                {
                    if (loopCount % 3 == 1)   //补齐下划线
                    {
                        sbTemp.AppendLine("<td></td><td></td><td></td><td></td></tr>");
                    }
                    if (loopCount % 3 == 2)
                    {
                        sbTemp.AppendLine("<td></td><td></td></tr>");
                    }
                }
                else    
                {
                    sbTemp.AppendLine("</tr>");
                }
            }
           
            sbTemp.AppendLine("</tbody>");
            sbTemp.AppendLine("</table>");
            sbTemp.AppendLine("</div>");
            // 如果有子项
            if (isHasChild)
            {
                sbParameter.Append(sbTemp.ToString());
            }
            if (sbTemp.Length > 0)
            { 
                sbTemp.Remove(0, sbTemp.Length); 
            }
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
                 
                        //<div class="img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-14093">
                        //    <div class="img">
                        //        <a href="#"><img src="http://img4.bitautoimg.com/bitauto//2016/05/20/145145725.jpg" alt="图片描述"></a>
                        //    </div>
                        //    <ul class="p-list">
                        //        <li class="name no-wrap"><a href="#">速腾</a></li>
                        //        <li class="price"><a href="#">12.55-13.55万</a></li>
                        //    </ul>
                        //</div>
                 
                htmlCode.AppendLine("<div class=\"col2-140-box clearfix\">");
                int loop = 0;
                foreach (EnumCollection.SerialToSerial sts in lsts)
                {
                    string csName = sts.ToCsShowName.ToString();

                    string shortName = StringHelper.SubString(csName, 12, true);
                    if (shortName.StartsWith(csName))
                        shortName = csName;
                    loop++;
                    htmlCode.AppendFormat("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-14093\">");
                    htmlCode.AppendFormat("<div class=\"img\"><a target=\"_blank\" href=\"/{0}/\"><img src=\"{1}\" alt=\"\"></a></div>",
                        sts.ToCsAllSpell.ToString().ToLower(),
                         sts.ToCsPic.ToString().Replace("_5","_3"));
                    htmlCode.AppendLine("<ul class=\"p-list\">");
                    if (shortName != csName)
                        htmlCode.AppendFormat("<li class=\"name no-wrap\"><a target=\"_blank\" href=\"/{0}/\" title=\"{1}\">{2}</a></li>",
                            sts.ToCsAllSpell.ToString().ToLower(),
                            csName, shortName);
                    else
                        htmlCode.AppendFormat("<li class=\"name no-wrap\"><a target=\"_blank\" href=\"/{0}/\">{1}</a></li>",
                            sts.ToCsAllSpell.ToString().ToLower(),
                            csName);
                    htmlCode.AppendFormat("<li class=\"price\"><a href=\"{1}\">{0}</a></li>", sts.ToCsSaleState == "待销" ? "未上市" : (sts.ToCsPriceRange.ToString().Length > 0 ? StringHelper.SubString(sts.ToCsPriceRange.ToString(), 14, false) : "暂无指导价"), sts.ToCsAllSpell.ToString().ToLower());
                    htmlCode.AppendLine("</ul>");
                    htmlCode.AppendFormat("</div>");
                }
                htmlCode.AppendLine("</div>");
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
            string serialUrl = "<div class=\"txt\"><a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}</a></div>";
            int index = 0;
            foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
            {
                bool IsExitsUrl = true;
                if (entity.SerialLevel == "概念车" || entity.SerialId == cbe.Serial.Id)
                {
                    continue;
                }
                //string priceRang = base.GetSerialPriceRangeByID(entity.SerialId);
                //价格取指导价
                string priceRang = base.GetSerialReferPriceByID(entity.SerialId);
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
                contentBuilder.AppendFormat("<li>{0}<span>{1}</span></li>"
                    , string.Format(serialUrl, entity.CS_AllSpell, tempCsSeoName), priceRang
                     );
            }

            StringBuilder brandOtherSerial = new StringBuilder(string.Empty);
            if (contentBuilder.Length > 0)
            {
                brandOtherSerial.AppendFormat("<h3 class=\"top-title\"><a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}其他车型</a></h3>",
                    cbe.Serial.Brand.AllSpell, cbe.Serial.Brand.Name);
                brandOtherSerial.Append("<div class=\"list-txt list-txt-s list-txt-default list-txt-style5\"><ul>");
                brandOtherSerial.Append(contentBuilder.ToString());
                brandOtherSerial.Append("</ul></div>");
            }

            return brandOtherSerial.ToString();
        }
    }
}