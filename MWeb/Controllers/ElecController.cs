using BitAuto.CarChannel.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MWeb.Controllers
{
    /// <summary>
    /// 新能源频道
    /// </summary>
    public class ElecController : Controller
    {
        Car_SerialBll carSerialBll = new Car_SerialBll();
        //
        // GET: /Elec/

        /// <summary>
        /// 新能源首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string rankMonth = string.Empty;
            carSerialBll.GetSeialSellRank(out rankMonth);
            ViewData["SaleRankYear"] = string.IsNullOrEmpty(rankMonth) ? DateTime.Now.Year.ToString() : rankMonth.Substring(0,4);

            return View();
        }

        /// <summary>
        /// 新能源销量排行
        /// </summary>
        /// <returns></returns>
        public ActionResult SaleRank()
        {
            return View();
        }

        /// <summary>
        /// 新能源无码大图
        /// </summary>
        /// <returns></returns>
        public ActionResult PhotoList()
        {
            return View();
        }

        /// <summary>
        /// 新能源补贴政策
        /// </summary>
        /// <returns></returns>
        public ActionResult Subsidy()
        {
            return View();
        }

        /// <summary>
        /// 新能源充电桩
        /// </summary>
        /// <returns></returns>
        public ActionResult ChargePile()
        {
            return View();
        }

        /// <summary>
        /// 新能源选车工具
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectCar()
        {
            return View();
        }

        /// <summary>
        /// 新能源选车工具
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectCarList()
        {
            return View();
        }
    }
}
