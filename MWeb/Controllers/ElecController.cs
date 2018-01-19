﻿using BitAuto.CarChannel.BLL;
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
            ViewData["SaleRankYear"] = string.IsNullOrEmpty(rankMonth) ? DateTime.Now.Year.ToString() : rankMonth.Substring(0, 4);

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
                sb.AppendFormat("<a href=\"/{0}/\">", detail.AllSpell);
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
            var query = HttpContext.Request.Url.PathAndQuery;
            Dictionary<string, string> param = new Dictionary<string, string>();
            var pp = query.Split('&');

            foreach (var item in pp)
            {
                if (item.Contains("="))
                {
                    var kv = item.Split('=');
                    if (kv.Length > 1)
                    {
                        param.Add(kv[0], kv[1]);
                    }
                }
            }
            if (!param.ContainsKey("f"))
            {
                param.Add("f", "16,128");
            }
            SelectCarToolNewBll selectCarToolNewBll = new SelectCarToolNewBll();
            var result = selectCarToolNewBll.GetSelectCarResultWithElecInfo(param);

            StringBuilder carList = new StringBuilder();
            foreach (var item in result.ResList)
            {

                carList.AppendLine("<li>");
                carList.AppendLine(string.Format("    <a href=\"\\{0}\\\" class=\"car\" >", item.AllSpell));
                carList.AppendLine("        <div class=\"img-box\" >");
                carList.AppendLine(string.Format(" <img src=\"{0}\" >", item.ImageUrl));
                //carList.AppendLine("            <i class=\"ico-shangshi\">新上车款</i>");
                carList.AppendLine("        </div>");
                carList.AppendLine(string.Format(" <strong>{0}</strong>", item.ShowName));
                carList.AppendLine(string.Format(" <p><em>{0}起</em></p>", item.PriceRange));
                if (!string.IsNullOrEmpty(item.NormalChargeTime))
                {
                    carList.AppendLine(string.Format("        <span class=\"bt\">充电时间{0}小时</span>", item.NormalChargeTime));
                }
                else {
                    //carList.AppendLine("        <span class=\"bt\"></span>");
                }
                carList.AppendLine("    </a>");
                carList.AppendLine("</li>");
            }

            ViewData["carList"] = carList.ToString();
            ViewData["CarNumber"] = result.CarNumber;
            ViewData["Count"] = result.Count;
            return View();
        }
    }
}
