using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WirelessWeb
{
    public partial class CarCompare : WirelessPageBase
	{
        protected int carID = 0;
        protected CarEntity ce;

        protected string exhaustForFloat = string.Empty;   //
        protected string transmissionType = string.Empty;
        protected string carAllParamHTML = string.Empty;
        protected string carAllParamMenu = string.Empty;
        protected string carList = string.Empty;

        private List<EnumCollection.CarInfoForSerialSummary> ls = new List<EnumCollection.CarInfoForSerialSummary>();

        Car_SerialBll carBLL;
        public CarCompare()
        {
            carBLL = new Car_SerialBll();
        }
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!this.IsPostBack) 
            {
                base.SetPageCache(30);
                GetPageParam();
                GetCarData();
            }
		}
        /// <summary>
        /// 取页面参数 
        /// </summary>
        private void GetPageParam() 
        {
            string strCarID = this.Request.QueryString["CarID"];
            if (!string.IsNullOrEmpty(strCarID) && int.TryParse(strCarID, out carID)) 
            { }
        }
        /// <summary>
        /// 获取车型数据
        /// </summary>
        private void GetCarData() 
        {
            if (carID > 0) 
            {
                //从缓存中获取车型实体
                ce =(CarEntity)DataManager.GetDataEntity(EntityType.Car, carID);
                if (ce != null && ce.Id > 0) 
                {
                    GetCarAllParam();    //获取当前车款所有参数
                   // GetCarListByCsID();  //获取当前车款同一系列下的其它所有车款
                }
            }
        }
        /// <summary>
        /// 获取车型完整参数
        /// </summary>
        private void GetCarAllParam() 
        {
            Dictionary<int, string> dic = new Car_BasicBll().GetCarAllParamByCarID(carID);
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

            //车型全部参数HTML
            List<string> listCarAllParamHTML = new List<string>();
            //车型全部参数菜单
            List<string> listMenu = new List<string>();
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
                                        int pid = 0;
                                        if (xn.Attributes["ParamID"] != null
                                            && xn.Attributes["ParamID"].Value != ""
                                            && int.TryParse(xn.Attributes["ParamID"].Value, out pid))
                                        { }
                                        if (pid > 0 && dic.ContainsKey(pid)
                                            && dic[pid] != "待查")
                                        {
                                            isHasChild = true || isHasChild;
                                            listTempClass.Add("<tr>");

                                            string pvalue = dic[pid] + xn.Attributes["Unit"].Value;
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
                                            // 进气型式 如果自然吸气直接显示，如果是增压则显示增压方式
                                            if (pid == 425 && pvalue == "增压")
                                            {
                                                if (dic.ContainsKey(408) && dic[408] != "待查")
                                                {
                                                    pvalueOther = dic[408];
                                                    pvalue = pvalue + " " + pvalueOther;
                                                }
                                            }
                                            //解决 变速箱 无极变速 替换成 -
                                            if (xn.Attributes.GetNamedItem("Name").Value != "变速箱")
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
                                                listTempClass.Add("<td>" + pvalue + "</td>");
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
            }
        }
        /// <summary>
        /// 取同子品牌下其他车型
        /// </summary>
        private void GetCarListByCsID()   
        {
            //在售车系：提取全部未上市+在售车款。
            //停售车系：提取全部车款。
            //未上市车系：提取全部车款
            if (ce.Id > 0 && ce.Serial.Id > 0)
            {
                if (ce.Serial.SaleState == "停销" || ce.Serial.SaleState == "待销")
                {
                    ls = base.GetAllCarInfoForSerialSummaryByCsID(ce.Serial.Id, true);
                }
                else
                {
                    ls = base.GetAllCarInfoForSerialSummaryByCsID(ce.Serial.Id);
                }

                if (ls.Count > 0)
                {
                    var year = string.Empty;
                    ls.Sort(NodeCompare.CompareCarByYear);
                    List<string> listCarList = new List<string>(20);
                    foreach (EnumCollection.CarInfoForSerialSummary cifss in ls)
                    {
                        string referPrice = cifss.ReferPrice.Trim();
                        if (referPrice.Length == 0)
                        {
                            referPrice = "暂无报价";
                        }
                        if (referPrice != "停售" && referPrice != "暂无报价")
                        {
                            referPrice += "万";
                        }

                        if (!string.IsNullOrEmpty(cifss.CarYear) && cifss.CarYear != year)
                        {
                            year = cifss.CarYear;
                            listCarList.Add("<dt><span>" + year + "款</span></dt>");
                        }

                        if (cifss.CarID == carID)
                        {
                            var ddHtml = "<dd class=\"current\"><a href=\"#\"><p>" + cifss.CarName + "</p><strong>" +
                                         referPrice + "</strong></a></dd>";
                            listCarList.Add(ddHtml);
                        }
                        else
                        {
                            listCarList.Add("<dd><a href=\"/" + ce.Serial.AllSpell + "/m" + cifss.CarID + "/peizhi/\" ><p>" + cifss.CarName + "</p><strong>" + referPrice + "</strong></a></dd>");
                        }
                    }
                    carList = string.Concat(listCarList.ToArray());
                }
            }
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