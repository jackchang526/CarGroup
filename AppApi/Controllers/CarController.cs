using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Model.AppApi;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;
using YiChe.Core.Extensions;

namespace AppApi.Controllers
{
    [JsonHandleError] //出错时以Json格式显示
    public class CarController : BaseController
    {
        bool isVersion87 = VersionHelper.CompareVersion("8.7") >= 0;
        #region Service

        static Car_MasterBrandBll _Car_MasterBrandBll;

        public static Car_MasterBrandBll CarMasterBrandService
        {
            get
            {
                if (_Car_MasterBrandBll == null)
                    _Car_MasterBrandBll = new Car_MasterBrandBll();

                return _Car_MasterBrandBll;
            }
            set { _Car_MasterBrandBll = value; }
        }



        static Car_BasicBll _carBasicService;

        public static Car_BasicBll CarBasicService
        {
            get
            {
                if (_carBasicService == null)
                    _carBasicService = new Car_BasicBll();

                return _carBasicService;
            }
            set { _carBasicService = value; }
        }
        static Car_SerialBll _carSerialService;

        public static Car_SerialBll CarSerialService
        {
            get
            {
                if (_carSerialService == null)
                    _carSerialService = new Car_SerialBll();

                return _carSerialService;
            }
            set { _carSerialService = value; }
        }
        #endregion
        /// <summary>
        /// 选配包
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, VaryByParam = "serialId", Location = OutputCacheLocation.Downstream)]
        public ActionResult GetCarSerialPackageEntityListBySerialId(int? serialId)
        {
            if (serialId.GetValueOrDefault(0) < 1)
            {
                return JsonNet(new { success = true, status = 0, message = "参数错误", data = "" }, JsonRequestBehavior.AllowGet);
            }
            var result = CarSerialService.GetCarSerialPackageEntityListBySerialId(serialId.GetValueOrDefault(0));
            return AutoJson(new
            {
                success = true,
                status = WebApiResultStatus.成功,
                message = "ok",
                data = new
                {
                    result = result
                }
            });
        }
        /// <summary>
        /// 车型参数模板
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Downstream)]
        public ActionResult GetCarParameterGroup(string version)
        {

            var result = CarBasicService.GetCarParameterJsonConfig(isVersion87);
            return AutoJson(new
            {
                success = true,
                status = WebApiResultStatus.成功,
                message = "ok",
                data = new
                {
                    GroupList = result
                }
            });
        }
        /// <summary>
        /// 车型参数
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Downstream)]
        public ActionResult GetCarStylePropertys(string carIds, string version)
        {
            if (string.IsNullOrWhiteSpace(carIds) || (!Regex.IsMatch(carIds, @"([,0-9]*)")))
            {
                return JsonNet(new { success = false, status = WebApiResultStatus.参数错误, message = "参数错误", data = "" }, JsonRequestBehavior.AllowGet);
            }
            List<int> carList = new List<int>();
            string[] carArr = carIds.Split(new char[] { ',' });
            int firstCarId = 0;
            int carCount = 0;
            foreach (var item in carArr)
            {
                int carid = TypeParse.StrToInt(item, 0);
                if (carid > 0)
                {
                    firstCarId = carid;
                    carList.Add(carid);
                    carCount++;
                }
            }

            var paramList = CarBasicService.GetCarParamterListWithWebCacheByCarIds(carList, isVersion87);
            return AutoJson(new
            {
                success = true,
                status = WebApiResultStatus.成功,
                message = "ok",
                data = new
                {
                    ParamList = paramList
                }
            });

        }
        /// <summary>
        /// 根据车型id和城市id获取车款列表
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Downstream)]
        public ActionResult GetCarListByCSIdAndCityId(int? csid, int? cityId, bool? includeStopSale = false)
        {
            if (csid.GetValueOrDefault(0) < 1 || cityId.GetValueOrDefault(0) < 1)
            {
                return JsonNet(new { success = true, status = 0, message = "参数错误", data = "" }, JsonRequestBehavior.AllowGet);
            }
            var CarGroupList = CarBasicService.GetCarGroupBySerialIdAndCSID(cityId.GetValueOrDefault(0), csid.GetValueOrDefault(0), includeStopSale.GetValueOrDefault(false));
            return AutoJson(new
            {
                success = true,
                status = WebApiResultStatus.成功,
                message = "ok",
                data = new
                {
                    CarGroupList = CarGroupList
                }
            });
        }

        /// <summary>
        /// 获取车型名片
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Downstream)]
        public ActionResult GetSerialInfo(int? csId, int? cityId)
        {
            if (csId.GetValueOrDefault(0) < 1 || cityId.GetValueOrDefault(0) < 1)
            {
                return JsonNet(new { success = true, status = 0, message = "参数错误", data = "" }, JsonRequestBehavior.AllowGet);
            }
            var serialInfo = CarSerialService.GetSerialInfoCard(csId.GetValueOrDefault(0));

            var result = new
            {
                csID = serialInfo.CsID,
                csName = serialInfo.CsShowName,//车型名称
                masterd = serialInfo,//大品牌logo
                //guidePriceRange = serialInfo.CsPriceRange,//参考价区间 
                referencePriceRange = serialInfo.CsPriceRange, //指导价区间
                //converImg = serialInfo.CoverImageUrl,
                //coverImg = serialExt == null ? "" : serialExt.CoverImageUrl,
                //whiteCoverImg = serialExt == null ? "" : serialExt.WhiteCoverUrl,
                imgCount = serialInfo.CsPicCount,//图片数量
                oil = serialInfo.CsSummaryFuelCost,// serialInfo.MinOil.ToString("F1") == "0.0" || serialInfo.MaxOil.ToString("F1") == "0.0" ? "暂无" : serialInfo.MinOil.ToString("F1") + "-" + serialInfo.MaxOil.ToString("F1") + "L",//参考油耗（在销车款的 综合工况油耗的最低和最高 ）
                //country = serialInfo.CountryName,//国别
                carType = serialInfo.CsLevel,//车型
                shareUrl = string.Format("http://car.h5.yiche.com/{0}/?WT.mc_id=nbycapp", serialInfo.CsAllSpell),//分享地址 子品牌全拼
                //serialLink = string.Format("http://m.yichemall.com/car/detail/index?modelId={0}&source=ycapp-tmall-1", serialInfo.Id),
                //forumId = forumId,
                //brandId = serialInfo.MakeId,//主品牌
                saleStatus = serialInfo.CsSaleState,
                //firstCarTimeToMarket = serialInfo.MinTimeToMarket,
                //lastCarTimeToMarket = serialInfo.MaxTimeToMarket,
                //lastCarTimeToMarketText = serialInfo,
                //isHaveImage = isHaveImage,
                newSaleStatus = CarSerialService.GetNewSerialIntoMarketText(serialInfo.CsID, true)
            };


            return AutoJson(new
            {
                success = true,
                status = WebApiResultStatus.成功,
                message = "ok",
                data = new
                {
                    //CarGroupList = CarGroupList
                }
            });
        }

        /// <summary>
        /// 网友还看过那些车
        /// </summary>
        /// <param name="csID">子品牌</param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Downstream)]
        public ActionResult GetSerialListForUser(int? csID)
        {
            var serialList = new PageBase().GetSerialToSerialByCsID(csID.GetValueOrDefault(), 6, 3);
            var result = new List<object>();
            if (serialList != null && serialList.Count > 0)
            {

                foreach (EnumCollection.SerialToSerial serialToSerial in serialList)
                {
                    string saleState = string.Empty;
                    if (string.IsNullOrWhiteSpace(serialToSerial.ToCsPriceRange))
                    {
                        if (!string.IsNullOrEmpty(serialToSerial.ToCsSaleState) && "待销" == serialToSerial.ToCsSaleState)
                        {
                            saleState = "未上市";
                        }
                        else
                        {
                            saleState = "暂无报价";
                        }
                    }
                    else
                    {
                        saleState = serialToSerial.ToCsPriceRange;
                    }
                    result.Add(new
                    {
                        CSId = serialToSerial.ToCsID,
                        Name = serialToSerial.ToCsShowName,
                        ShowName = serialToSerial.ToCsShowName,
                        Pic = serialToSerial.ToCsPic,
                        PriceRange = saleState,
                        AllSpell = serialToSerial.ToCsAllSpell
                    });
                }

            }
            return JsonNet(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
        }

        #region zhangzhiyang work zone

        /// <summary>
        /// 获取品牌故事
        /// </summary>
        /// <param name="masterid">主品牌id</param>
        /// <returns></returns>
        public ActionResult GetMasterBrandStory(int masterid)
        {
            //验证
            var ds = CarMasterBrandService.GetMasterBrandStory(masterid);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return JsonNet(new
                {
                    success = true,
                    status = WebApiResultStatus.成功,
                    message = "成功",
                    data =
                            new
                            {
                                masterId = ds.Tables[0].Rows[0]["Id"],
                                masterName = ds.Tables[0].Rows[0]["Name"],
                                logoMeaning = ds.Tables[0].Rows[0]["LogoMeaning"],
                                introduction = ds.Tables[0].Rows[0]["Introduction"]
                            }
                }, JsonRequestBehavior.AllowGet);
            }
            return JsonNet(new
            {
                success = false,
                status = WebApiResultStatus.未找到数据,
                message = "未找到数据",
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据车款编号获取对应车身颜色或内饰颜色
        /// </summary>
        /// <param name="carStyleId">车款编号</param>
        /// <param name="type">0车身颜色 1内饰颜色</param>
        /// <returns></returns>
        public ActionResult GetCarStyleColorById(int? carStyleId, int? type)
        {
            var wrs = WebApiResultStatus.参数错误;
            type = type.GetValueOrDefault(0);
            if (carStyleId == null || (type != 0 && type != 1))
                return JsonNet(new { success = false, status = wrs, message = wrs.ToString(), data = new { list = string.Empty } }, JsonRequestBehavior.AllowGet);
            wrs = WebApiResultStatus.成功;
            var data = CarMasterBrandService.GetCarStyleColorById(carStyleId.Value, type.Value);
            if (data != null && data.Count > 0)
            {
                var showData = (from c in data select new { c.Name, c.Value }).ToList();
                return JsonNet(new { success = true, status = wrs, message = wrs.ToString(), data = new { list = showData } }, JsonRequestBehavior.AllowGet);
            }
            return JsonNet(new { success = true, status = wrs, message = wrs.ToString(), data = new { list = string.Empty } }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据车系编号和颜色类型获取车系颜色
        /// </summary>
        /// <param name="modelColor">颜色类型</param>
        /// <returns></returns>
        public ActionResult GetCarModelColorByModelId(int ModelId, int ColorType = 0)
        {
            var wrs = WebApiResultStatus.参数错误;
            if (ColorType < 0 || ModelId <= 0)
            {
                return JsonNet(new { success = false, status = wrs, message = wrs.ToString(), data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            wrs = WebApiResultStatus.成功;
            var data = CarMasterBrandService.GetCarModelColorByModelId(ModelId, ColorType);
            if (data != null && data.Count > 0)
            {
                var showData = (from c in data select new { c.Name, c.Value }).ToList();
                return JsonNet(new { success = true, status = wrs, message = wrs.ToString(), data = showData }, JsonRequestBehavior.AllowGet);
            }
            return JsonNet(new { success = true, status = wrs, message = wrs.ToString(), data = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  获取主品牌列表接口
        /// </summary>
        /// <param name="allMasterBrand">是否返回全部主品牌(包含停销)</param>
        /// <returns></returns>
        [OutputCache(Duration = 900, Location = OutputCacheLocation.Downstream)]
        public ActionResult GetMasterBrandList(bool? allMasterBrand)
        {
            var list = CarMasterBrandService.GetCarMasterBrandList(allMasterBrand.GetValueOrDefault());
            return JsonNet(new { status = 1, message = "ok", data = list }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取车型属性
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        [OutputCache(Duration = 960, Location = OutputCacheLocation.Downstream)]
        public ActionResult GetCarStyleById(int carId)
        {
            var car = CarSerialService.GetStyleInfoById(carId);
            if (car != null)
            {
                var urlDic = CarSerialImgUrlService.GetImageUrlDicNew();
                var carImage = string.Empty;
                if (urlDic != null && urlDic.Count > 0 && urlDic.ContainsKey(car.ModelId))
                {
                    if (urlDic[car.ModelId].GetAttribute("ImageUrl2").ToString().Trim() != "")
                    {
                        // 有新封面
                        carImage = urlDic[car.ModelId].GetAttribute("ImageUrl2").ToString().Trim();
                    }
                    else
                    {
                        // 没有新封面
                        if (urlDic[car.ModelId].GetAttribute("ImageUrl").ToString().Trim() != "")
                        {
                            carImage = urlDic[car.ModelId].GetAttribute("ImageUrl").ToString().Trim();
                        }
                        else
                        {
                            carImage = WebConfig.DefaultCarPic;
                        }
                    }
                }
                return JsonNet(new
                {
                    status = WebApiResultStatus.成功,
                    message = "OK",
                    data = new
                    {
                        carId = carId,
                        carName = car.Name,
                        carImage = carImage,
                        carYear = car.Year,
                        carSerialId = car.ModelId,
                        carSerialName = car.CarSerialName
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return JsonNet(new
                {
                    status = WebApiResultStatus.未找到数据,
                    message = "OK",
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 根据车款ID获取相关属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Downstream)]
        public ActionResult GetCarStylePropertyById(int? id)
        {
            Dictionary<string, string> carProperty = CarSerialService.GetCarStylePropertyById(id.GetValueOrDefault(0));
            if (carProperty.Keys.Count == 0)
            {
                return JsonNet(new { success = false, status = 0, message = "未找到数据", data = "" }, JsonRequestBehavior.AllowGet);
            }

            var taxPer = carProperty.GetValueOrDefault("traveltax");
            var travePerTax = 1.0;
            var tax = "无";
            switch (taxPer)
            {
                case "免征":
                    tax = "免征";
                    travePerTax = 0;
                    break;
                case "减半":
                    tax = "75折";
                    travePerTax = 0.75;
                    break;
                case "待查":
                    tax = "待查";
                    travePerTax = 1.0;
                    break;
                default:
                    tax = "无";
                    travePerTax = 1.0;
                    break;
            }
            int taxrelief = TypeParse.StrToInt(carProperty.GetValueOrDefault("taxrelief"), 0);
            string strExhaust = carProperty.GetValueOrDefault("exhaustforfloat");
            float floatExhaust = 0;
            double purchasetax = 0.0854700854700855;
            if (float.TryParse(strExhaust, out floatExhaust))
            {
                if (floatExhaust > 0 && floatExhaust <= (float)1.6)
                {
                    purchasetax = 0.064102564102564125;
                }
            }

            if (taxrelief == 2)
            {
                purchasetax = 0;
            }

            var result = new
            {
                carID = id,
                isGuoChan = carProperty.GetValueOrDefault("isGuoChan"),
                engine = carProperty.GetValueOrDefault("engine"),
                exhaustforfloat = strExhaust,
                traveltax = tax,
                seatNum = carProperty.GetValueOrDefault("seatNum"),
                purchasetax = purchasetax,
                fuelType = TransferFuelType(carProperty.GetValueOrDefault("fuelType")),
                travePerTax = travePerTax
            };
            return JsonNet(new { success = true, status = 1, message = "成功", data = result }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 转换枚举名字
        /// </summary>
        /// <param name="fuelType"></param>
        /// <returns></returns>
        private string TransferFuelType(string fuelType)
        {
            var newFuelType = string.Empty;
            switch (fuelType)
            {
                case "汽油":
                    newFuelType = "0";
                    break;
                case "柴油":
                    newFuelType = "1";
                    break;
                case "纯电":
                    newFuelType = "2";
                    break;
                case "油电混合":
                    newFuelType = "3";
                    break;
                case "插电混合":
                    newFuelType = "4";
                    break;
                case "天然气":
                    newFuelType = "5";
                    break;
            }
            return newFuelType;
        }

        /// <summary>
        /// 获取车型列表接口
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OutputCache(Duration = 900, Location = OutputCacheLocation.Downstream)]
        public ActionResult GetSerialList(SerialListQueryEntity query)
        {
            if (query.MasterId <= 0)
            {
                return JsonNet(new { status = (int)WebApiResultStatus.参数错误, message = "参数有误" }, JsonRequestBehavior.AllowGet);
            }
            var list = CarSerialService.GetCarBrandAndSerial(query.MasterId, query.AllSerial);
            return JsonNet(new { status = 1, message = "ok", data = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取车型列表接口（带车型图片数量）
        /// </summary>
        /// <param name="masterId"></param>
        /// <param name="allSerial"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Downstream)]
        public ActionResult GetSerialListWithImage(int masterId, bool? allSerial)
        {
            if (masterId <= 0)
            {
                return JsonNet(new { status = (int)WebApiResultStatus.参数错误, message = "参数有误" }, JsonRequestBehavior.AllowGet);
            }
            var list = CarSerialService.GetCarBrandAndSerial(masterId, allSerial.GetValueOrDefault(false));

            List<int> serialIds = new List<int>();
            foreach (var brand in list)
            {
                serialIds.AddRange(brand.SerialList.Select(x => x.SerialId));
            }
            var ids = serialIds.Select(x => x).Distinct();
            Dictionary<int, int> imageCounts = new Dictionary<int, int>();
            foreach (var item in ids)
            {
                imageCounts.Add(item, GetSerialPicAndCountByCsID(item));
            }
            return JsonNet(new
            {
                status = 1,
                message = "ok",
                data = new
                {
                    brands = list,
                    images = imageCounts
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据车型ID获取图片数量
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        private int GetSerialPicAndCountByCsID(int csID)
        {
            int csCount = 0;
            var imgDoc = this.GetAllSerialConverImgAndCount();
            if (imgDoc != null)
            {
                XmlNode imgNode = imgDoc.SelectSingleNode("/SerialList/Serial[@SerialId='" + csID + "']");
                if (imgNode != null)
                {
                    csCount = ConvertHelper.GetInteger(imgNode.Attributes["ImageCount"].Value);
                }
            }
            return csCount;
        }


        /// <summary>
        /// 车系封面图及图片数量
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetAllSerialConverImgAndCount()
        {
            string cacheKey = "Car_GetAllSerialConverImgAndCount";
            object allSerialPicAndCount = null;
            CacheManager.GetCachedData(cacheKey, out allSerialPicAndCount);
            if (allSerialPicAndCount == null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                string photoDataPath = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialCoverImageAndCountPath);
                xmlDoc.Load(photoDataPath);
                CacheManager.InsertCache(cacheKey, xmlDoc, 60);
                return xmlDoc;
            }
            else
            {
                return allSerialPicAndCount as XmlDocument;
            }
        }

        #endregion
    }

}
