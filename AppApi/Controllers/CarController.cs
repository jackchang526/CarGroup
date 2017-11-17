using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Model.AppApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;

namespace AppApi.Controllers
{
    [JsonHandleError] //出错时以Json格式显示
    public class CarController : BaseController
    {
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
        public ActionResult GetCarParameterGroup()
        {
            var result = CarBasicService.GetCarParameterJsonConfig();
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
        public ActionResult GetCarStylePropertys(string carIds)
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

            var paramList = CarBasicService.GetCarParamterListWithWebCacheByCarIds(carList);
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

        }   /// <summary>
            /// 车型参数
            /// </summary>
            /// <param name="serialId"></param>
            /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Downstream)]
        public ActionResult GetCarListByCSIdAndCityId(int? csid, int? cityId)
        {

            List<string> carSaleList = new List<string>();
            List<string> carNoSaleList = new List<string>();
            List<string> carWaitSaleList = new List<string>();

            var carinfoList = CarBasicService.GetCarInfoForSerialSummaryBySerialId(csid.GetValueOrDefault(0));



            return AutoJson(new
            {
                success = true,
                status = WebApiResultStatus.成功,
                message = "ok",
                data = new
                {
                    ParamList = carinfoList
                }
            });
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

        #endregion
    }
}
