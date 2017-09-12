using BitAuto.CarChannel.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using BitAuto.CarChannel.BLL.Data;
using System.Xml;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;

namespace MWeb.Controllers
{
    public class CarCompareController : Controller
    {
        //  
        // GET: /CarCompare/
        protected int carID = 0;
        protected string exhaustForFloat = string.Empty;   //
        protected string transmissionType = string.Empty;
        protected string carAllParamHTML = string.Empty;
        protected string carAllParamMenu = string.Empty;
        protected string carAllParamPop = string.Empty;
        protected CarEntity ce;
        public CarCompareController()
        {
            ce = new CarEntity();
        }
        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream, VaryByParam = "*")]
        public ActionResult Index()
        {
            GetPageParam(RouteData.Values);
            GetCarData();
			if (ce == null || ce.Id == 0 || ce.Serial == null)
			{
				Response.Redirect("/error", true);
				return new EmptyResult();
			}
            return View(ce);
        }
        private void GetPageParam(RouteValueDictionary values)
        {
            carID=ConvertHelper.GetInteger(values["carid"]);
            ViewBag.CarId = carID;
        }
        /// <summary>
        /// 获取车型数据
        /// </summary>
        private void GetCarData()
        {
            if (carID > 0)
            {
                //从缓存中获取车型实体
                ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carID);
                if (ce != null && ce.Id > 0)
                {
                    GetCarAllParam();    //获取当前车款所有参数
                }
            }
        }
        private void GetCarAllParam()
        {
            Dictionary<int, string> dic = new Car_BasicBll().GetCarAllParamByCarID(carID);
            Dictionary<int, Dictionary<string, double>> dicOptional = new Car_BasicBll().GetCarAllParamOptionalByCarID(carID);
            exhaustForFloat = dic.ContainsKey(785) ? dic[785] + "L" : "暂无";  //785代表所有排量
            //档位个数
            string forwardGearNum = (dic.ContainsKey(724) && dic[724] != "无级") ? dic[724] + "档" : "";
            transmissionType = dic.ContainsKey(712) ? forwardGearNum + dic[712] : "暂无";
            //车身颜色
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
                        {
                            listColor.Add(color);
                        }
                    }
                }
            }
            //车型车身颜色 色块
            string carColorHTML = GetCarColor(listColor);

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
                if (System.IO.File.Exists(filePath))
                {
                    docPC.Load(filePath);
                    CacheManager.InsertCache(cache, docPC, new System.Web.Caching.CacheDependency(filePath), DateTime.Now.AddMinutes(60));
                }
            }

            //车型全部参数HTML
            List<string> listCarAllParamHTML = new List<string>();
            //车型全部参数菜单
            List<string> listMenu = new List<string>();
            //选配具体内容
            List<string> listPop = new List<string>();
            #region 车型详细参数
            if (docPC != null && docPC.HasChildNodes)
            {
                XmlNode rootPC = docPC.DocumentElement;
                if (rootPC != null && rootPC.ChildNodes.Count > 0)
                {
                    int isFirstTrTd = 0;
                    foreach (XmlNode xng in rootPC.ChildNodes)
                    {
                        if (xng.NodeType != XmlNodeType.Element)
                        { continue; }
                        // 大类，参数或配置
                        int loop = 0;
                        if (xng != null && xng.ChildNodes.Count > 0)
                        {
                            foreach (XmlNode xnClass in xng.ChildNodes)
                            {
                                if (xnClass.NodeType != XmlNodeType.Element)
                                { continue; }
                                List<string> listTempClass = new List<string>();
                                bool isHasChild = false;
                                listTempClass.Add("<tr id= \"subMenu_" + loop + "\"><td class=\"pd0\" colspan=\"2\"><h3><span>");
                                listTempClass.Add(xnClass.Attributes["Name"].Value);
                                listTempClass.Add("</span></h3></td></tr>");

                                // 参数分类
                                if (xnClass != null && xnClass.ChildNodes.Count > 0)
                                {
                                    foreach (XmlNode xn in xnClass.ChildNodes)
                                    {
                                        #region 具体参数
                                        // 每个参数
                                        if (xn.NodeType != XmlNodeType.Element)
                                        { continue; }
                                        string pvalue = string.Empty;
                                        int pid = 0;
                                        //合并参数
                                        if (xn.Attributes.GetNamedItem("Value").Value.IndexOf(",") != -1)
                                        {
                                            string[] arrKey = xn.Attributes.GetNamedItem("Value").Value.Split(',');
                                            string[] arrUnit = xn.Attributes.GetNamedItem("Unit").Value.Split(',');
                                            string[] arrParam = xn.Attributes.GetNamedItem("ParamID").Value.Split(',');
                                            List<string> list = new List<string>();
                                            for (var i = 0; i < arrParam.Length; i++)
                                            {
                                                int paraId = 0;
                                                if (!(int.TryParse(arrParam[i], out paraId))){ continue; }
                                                if (!(dic.ContainsKey(paraId) && dic[paraId] != "待查"))
                                                    continue;
                                                //档位数 0 不显示
                                                if (arrParam[i] == "724")
                                                {
                                                    var d = ConvertHelper.GetInteger(dic[paraId]);
                                                    if (d <= 0) continue;
                                                }

                                                list.Add(string.Format("{0}{1}", dic[paraId], arrUnit[i]));
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
                                            
                                            if (xn.Attributes["ParamID"] != null
                                                && xn.Attributes["ParamID"].Value != ""
                                                && int.TryParse(xn.Attributes["ParamID"].Value, out pid))
                                            { }
                                            if (!(dic.ContainsKey(pid))&& !(dicOptional.ContainsKey(pid)) && pid <= 0 )
                                            { continue; }
                                            //参配值
                                            if (dic.ContainsKey(pid) && dic[pid] != "待查")
                                            {
                                                pvalue = string.Format("{0}{1}", dic[pid], xn.Attributes.GetNamedItem("Unit").Value);
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(pvalue)|| dicOptional.ContainsKey(pid))
                                        {
                                            isHasChild = true || isHasChild;
                                            listTempClass.Add("<tr>");

                                            // 燃料类型 汽油的话同时显示 燃油标号
                                            string pvalueOther;
                                            if (pid == 578 && pvalue == "汽油")
                                            {
                                                if (dic.ContainsKey(577) && dic[577] != "待查")
                                                {
                                                    pvalueOther = dic[577];
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

                                            //解决 变速箱 无极变速 替换成 -
                                            if (xn.Attributes.GetNamedItem("Name").Value != "燃油变速箱")
                                            {
                                                if (pvalue.IndexOf("有") == 0)
                                                { pvalue = "●"; }
                                                if (pvalue.IndexOf("选配") == 0)
                                                { pvalue = "○"; }
                                                if (pvalue.IndexOf("无") == 0)
                                                { pvalue = "-"; }
                                            }

                                            if (isFirstTrTd <= 1)
                                            {
                                                listTempClass.Add("<th>" + xn.Attributes["Name"].Value + "</th>");
                                            }
                                            else
                                            {
                                                listTempClass.Add("<th>");
                                                if (pid == 598)
                                                {
                                                    // 车身颜色
                                                    pvalue = carColorHTML;
                                                }
                                                listTempClass.Add(xn.Attributes["Name"].Value + "</th>");
                                            }
                                            isFirstTrTd++;
                                            if (pid == 598)
                                            {
                                                listTempClass.Add("<td class=\"m-car-color\">" + pvalue + "</td>");
                                            }
                                            else
                                            {
                                                if (pid > 0 && dicOptional.ContainsKey(pid))
                                                {
                                                    var optionalPara = dicOptional[pid];
                                                    if (optionalPara.Count <= 1)
                                                    {
                                                        var name = optionalPara.Single().Key;
                                                        var price = optionalPara.Single().Value;
                                                        if (string.IsNullOrEmpty(pvalue))
                                                        {
                                                            listTempClass.Add("<td><div class=\"optional\">" + "○ " + name + "￥" + price + "起</div></td>");
                                                        }
                                                        else
                                                        {
                                                            listTempClass.Add("<td><div>" + "● " + pvalue + "</div><div class=\"optional\">" + "○ " + name + "￥" + price + "起</div></td>");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(pvalue))
                                                        {
                                                            listTempClass.Add("<td>");
                                                        }
                                                        else
                                                        {
                                                            listTempClass.Add("<td><div>" + "● " + pvalue + "</div>");
                                                        }
                                                        double minPrice = optionalPara.Values.Min();
                                                        listTempClass.Add("<div class=\"optional type1\" data-optionid=" + pid + ">" + "○ 选装￥" + minPrice + "起</div></td>");
                                                        listPop.Add("<ul data-optionID=\"" + pid + "\" style=\"display: none; \">");
                                                        foreach (var para in optionalPara.Keys)
                                                        {                                                           
                                                            listPop.Add("<li> <span class=\"l\">" + para + "</span> <span class=\"r\">" + optionalPara[para] + "</span></li>");
                                                        }
                                                        listPop.Add("</ul>");
                                                    }
                                                }
                                                else
                                                {
                                                    listTempClass.Add("<td>" + pvalue + "</td>");}
                                                }
                                        }
                                        else
                                        {
                                            // pid <0 年款
                                            if (xn.Attributes["Value"] != null)
                                            {
                                                if (xn.Attributes["Value"].Value.ToString() == "Car_YearType"
                                                    && ce.CarYear > 0)
                                                {

                                                    listTempClass.Add("<tr>");
                                                    // 年款
                                                    listTempClass.Add("<th>");
                                                    listTempClass.Add(xn.Attributes["Name"].Value + "</th>");
                                                    isFirstTrTd++;
                                                    listTempClass.Add("<td>" + ce.CarYear.ToString() + "款</td>");
                                                }
                                            }
                                        }
                                        listTempClass.Add("</tr>");
                                        #endregion
                                    }
                                    // 如果有子项
                                    if (isHasChild)
                                    {
                                        listCarAllParamHTML.Add(string.Concat(listTempClass.ToArray()));
                                        listTempClass.Clear();
                                        listMenu.Add("<li><a href=\"#subMenu_" + loop + "\"><span>" + xnClass.Attributes["Name"].Value + "</span></a></li>");
                                        loop++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            if (listCarAllParamHTML.Count > 0)
            {
                // 头尾
                listCarAllParamHTML.Insert(0, "<table>");
                listCarAllParamHTML.Add("</table>");
                carAllParamHTML = string.Concat(listCarAllParamHTML.ToArray());
                carAllParamMenu = string.Concat(listMenu.ToArray());
                carAllParamPop = string.Concat(listPop.ToArray());
            }
            ViewBag.CarAllParamHTML = carAllParamHTML;
            ViewBag.CarAllParamMenu = carAllParamMenu;
            ViewBag.CarAllParamPop= carAllParamPop;
        }
        /// <summary>
        /// 生成车型颜色块
        /// </summary>
        /// <param name="listNameColor"></param>
        /// <returns></returns>
        private string GetCarColor(List<string> listNameColor)
        {
            StringBuilder sb = new StringBuilder();
            string colorHTML = "";
            List<string> listColorHTML = new List<string>();
            DataSet dsAllCsColor = new Car_SerialBll().GetAllSerialColorRGB();
            DataRow[] drs = dsAllCsColor.Tables[0].Select(" cs_id='" + ce.Serial.Id.ToString() + "' ");
            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    if (dr["colorName"].ToString().Trim() != ""
                        && listNameColor.Contains(dr["colorName"].ToString().Trim()))
                    {
                        listColorHTML.Add("<li><span style=\"background:" + dr["colorRGB"].ToString().Trim() + "\"></span></li>");
                    }
                }
            }
            if (listColorHTML.Count > 0)
            {
                listColorHTML.Insert(0, "<ul>");
                listColorHTML.Add("</ul>");
                colorHTML = string.Concat(listColorHTML.ToArray());
            }
            return colorHTML;
        }
    }
}
