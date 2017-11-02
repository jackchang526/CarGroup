using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
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

        }


        #region 主品牌的品牌故事

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
                    success = false,
                    status = WebApiResultStatus.未找到数据,
                    message = "未找到数据",
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }
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

        #endregion
    }
}
