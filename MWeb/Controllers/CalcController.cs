using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;

namespace MWeb.Controllers
{
    /// <summary>
    /// 购车计算器控制器
    /// </summary>
    public class CalcController : Controller
    {
        /// <summary>
        /// 全款计算选车页
        /// </summary>
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Downstream)]
        public ActionResult CalcAutoCash(int carId = 0)
        {
            ViewData["CarId"] = carId;
            double carReferPrice = 0;
            int carSerialId = 0;
            int carBrandId = 0;
            string carFullName = string.Empty;
            if (carId > 0)
            {
                CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                if (ce != null && ce.Id > 0)
                {
                    carReferPrice = Math.Round(ce.ReferPrice * 10000);
                    carSerialId = ce.SerialId;
                    //carBrandId = ce.Serial.Brand.MasterBrandId;
					carBrandId = ce.Serial != null && ce.Serial.Brand != null ? ce.Serial.Brand.MasterBrandId : 0;
					carFullName = string.Format("{0}款 {1} {2}", ce.CarYear, ce.Serial != null ? ce.Serial.ShowName : string.Empty, ce.Name);
                }
            }
            ViewData["CarReferPrice"] = carReferPrice;
            ViewData["CarSerialId"] = carSerialId;
            ViewData["CarBrandId"] = carBrandId;
            ViewData["CarFullName"] = carFullName;
            return View();
        }
        /// <summary>
        /// 全款计算详情页
        /// </summary>
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Downstream)]
        public ActionResult CalcAutoCashDetail(int carId = 0)
        {
            ViewData["CarId"] = carId;
            double carReferPrice = 0;
            int carSerialId = 0;
            int carBrandId = 0;
            string serialName = string.Empty;
            string carFullName = string.Empty;
            if (carId > 0)
            {
                CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                if (ce != null && ce.Id > 0)
                {
                    carReferPrice = Math.Round(ce.ReferPrice * 10000);
                    carSerialId = ce.SerialId;
					carBrandId = ce.Serial != null && ce.Serial.Brand != null ? ce.Serial.Brand.MasterBrandId : 0;
					serialName = ce.Serial != null ? ce.Serial.ShowName : string.Empty;
					carFullName = string.Format("{0}款 {1} {2}", ce.CarYear, serialName, ce.Name);
					ViewData["CarEntity"] = ce;
				}
            }
            ViewData["CarReferPrice"] = carReferPrice;
            ViewData["CarSerialId"] = carSerialId;
            ViewData["CarBrandId"] = carBrandId;
            ViewData["SerialName"] = serialName;
            ViewData["CarFullName"] = carFullName;
            return View();
        }
        /// <summary>
        /// 贷款计算选车页
        /// </summary>
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Downstream)]
        public ActionResult CalcAutoLoan(int carId = 0)
        {
            ViewData["CarId"] = carId;
            double carReferPrice = 0;
            int carSerialId = 0;
            int carBrandId = 0;
            string carFullName = string.Empty;
            if (carId > 0)
            {
                CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                if (ce != null && ce.Id > 0)
                {
                    carReferPrice = Math.Round(ce.ReferPrice * 10000);
                    carSerialId = ce.SerialId;
                    carBrandId = ce.Serial != null && ce.Serial.Brand != null ? ce.Serial.Brand.MasterBrandId : 0;
					carFullName = string.Format("{0}款 {1} {2}", ce.CarYear, ce.Serial != null ? ce.Serial.ShowName : string.Empty, ce.Name);
                }
            }
            ViewData["CarReferPrice"] = carReferPrice;
            ViewData["CarSerialId"] = carSerialId;
            ViewData["CarBrandId"] = carBrandId;
            ViewData["CarFullName"] = carFullName;
            return View();
        }
        /// <summary>
        /// 贷款计算详情页
        /// </summary>
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Downstream)]
        public ActionResult CalcAutoLoanDetail(int carId = 0)
        {
            ViewData["CarId"] = carId;
            double carReferPrice = 0;
            int carSerialId = 0;
            int carBrandId = 0;
            string serialName = string.Empty;
            string carFullName = string.Empty;
            string csAllSpell = string.Empty;
            if (carId > 0)
            {
                CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                if (ce != null && ce.Id > 0)
                {
                    carReferPrice = Math.Round(ce.ReferPrice * 10000);
                    carSerialId = ce.SerialId;
                    carBrandId = ce.Serial != null && ce.Serial.Brand != null ? ce.Serial.Brand.MasterBrandId : 0;
					serialName = ce.Serial != null ? ce.Serial.ShowName : string.Empty;
					csAllSpell = ce.Serial != null ? ce.Serial.AllSpell : string.Empty;
                    carFullName = string.Format("{0}款 {1} {2}", ce.CarYear, serialName, ce.Name);
					ViewData["CarEntity"] = ce;
                }
            }
            ViewData["CarReferPrice"] = carReferPrice;
            ViewData["CarSerialId"] = carSerialId;
            ViewData["CarBrandId"] = carBrandId;
            ViewData["SerialName"] = serialName;
            ViewData["CsAllSpell"] = csAllSpell;
            ViewData["CarFullName"] = carFullName;
            return View();
        }
        /// <summary>
        /// 保险计算选车页
        /// </summary>
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Downstream)]
        public ActionResult CalcInsurance(int carId = 0)
        {
            ViewData["CarId"] = carId;
            double carReferPrice = 0;
            int carSerialId = 0;
            int carBrandId = 0;
            string carFullName = string.Empty;
            if (carId > 0)
            {
                CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                if (ce != null && ce.Id > 0)
                {
                    carReferPrice = Math.Round(ce.ReferPrice * 10000);
                    carSerialId = ce.SerialId;
                    carBrandId = ce.Serial != null && ce.Serial.Brand != null ? ce.Serial.Brand.MasterBrandId : 0;
					carFullName = string.Format("{0}款 {1} {2}", ce.CarYear, ce.Serial != null ? ce.Serial.ShowName : string.Empty, ce.Name);
                }
            }
            ViewData["CarReferPrice"] = carReferPrice;
            ViewData["CarSerialId"] = carSerialId;
            ViewData["CarBrandId"] = carBrandId;
            ViewData["CarFullName"] = carFullName;
            return View();
        }
        /// <summary>
        /// 保险计算详情页
        /// </summary>
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Downstream)]
        public ActionResult CalcInsuranceDetail(int carId = 0)
        {
            ViewData["CarId"] = carId;
            double carReferPrice = 0;
            int carSerialId = 0;
            int carBrandId = 0;
            string serialName = string.Empty;
            string carFullName = string.Empty;
            if (carId > 0)
            {
                CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                if (ce != null && ce.Id > 0)
                {
                    carReferPrice = Math.Round(ce.ReferPrice * 10000);
                    carSerialId = ce.SerialId;
                    carBrandId = ce.Serial != null && ce.Serial.Brand != null ? ce.Serial.Brand.MasterBrandId : 0;
					serialName = ce.Serial != null ? ce.Serial.ShowName : string.Empty;
                    carFullName = string.Format("{0}款 {1} {2}", ce.CarYear, serialName, ce.Name);
					ViewData["CarEntity"] = ce;
                }
            }
            ViewData["CarReferPrice"] = carReferPrice;
            ViewData["CarSerialId"] = carSerialId;
            ViewData["CarBrandId"] = carBrandId;
            ViewData["SerialName"] = serialName;
            ViewData["CarFullName"] = carFullName;
            return View();
        }
    }
}
