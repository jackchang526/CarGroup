using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        SelectCarToolNewBll selectCarToolNewBll = new SelectCarToolNewBll();
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

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("f", "16");
            param.Add("pagesize", "10");
            SelectCarResult elecResult = selectCarToolNewBll.GetSelectCarResultWithElecInfo(param);
            param["f"] = "128";
            SelectCarResult mixElecResult = selectCarToolNewBll.GetSelectCarResultWithElecInfo(param);
            ViewData["ElecResultHtml"] = GetHotElecHtml(elecResult);
            ViewData["MixElecResultHtml"] = GetHotElecHtml(mixElecResult);
            return View();
        }

        /// <summary>
        /// 热门电动车
        /// </summary>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetHotElecHtml(SelectCarResult result)
        {
            if (result == null || result.ResList.Count == 0) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (SelectCarDetailInfo detail in result.ResList)
            {
                sb.Append("<li>");
                sb.AppendFormat("<a href=\"/{0}/\">",detail.AllSpell);
                sb.AppendFormat("<img src=\"{0}\">", detail.ImageUrl);
                sb.AppendFormat("<strong>{0}</strong>", detail.ShowName);
                sb.AppendFormat("<em>{0}</em>", detail.PriceRange);
                if (!string.IsNullOrEmpty(detail.BatteryLife))
                {
                    sb.AppendFormat("<span class=\"bt\">{0}公里</span>", detail.BatteryLife);
                }
                else
                {
                    sb.Append("<span class=\"bt\">暂无数据</span>");
                }
                sb.Append("</a>");
                sb.Append("</li>");
            }
            return sb.ToString();
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
