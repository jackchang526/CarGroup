using System;
using System.Xml;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    public partial class SerialBaseInfo : PageBase
    {

        #region member

        private int csID = 0;
        private string callback = string.Empty;
        private List<string> jsonString = new List<string>();
        private string jsonVarName = string.Empty;
        private string op = string.Empty;
        private List<int> listCsID = new List<int>();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.ContentType = "application/x-javascript";
            if (!this.IsPostBack)
            {
				base.SetPageCache(60 * 4);
                GetPageParam();
                if (op == "getcsforwireless")
                {
                    GetCsJsonData();
                }
                else if (op == "getviewedcar")
                {
                    // 取浏览过的车型
                    GetViewdCarList();
                    return;
                }
                else if (op == "getnewcarbyyear")
                {
                    // 取某年的上市车
                    GetNewCarByYear();
                }
                else
                { }
                ResponseJson();
            }
        }

        #region private Method

        private void GetNewCarByYear()
        {
            #region 取查询年款
            int year = DateTime.Now.Year;
            string strYear = this.Request.QueryString["year"];
            if (!string.IsNullOrEmpty(strYear) && int.TryParse(strYear, out year))
            { }
            if (year <= 0 || year >= 9999)
            {
                year = DateTime.Now.Year;
            }
            #endregion

            if (year > 0)
            {
                // 按年数据缓存1小时
                string cacheKey = "SerialBaseInfo_GetNewCarByYear_" + year.ToString();
                object obj = CacheManager.GetCachedData(cacheKey);
                if (obj != null)
                {
                    jsonString.Add(obj.ToString());
                }
                else
                {
                    #region 查询数据

                    string sql = @"SELECT  cs.cs_id, cs.csname, cs.csshowname, cl.classvalue AS carLevel,
                                            cs.allspell, car.car_id, car.car_name, car.car_ReferPrice,
                                            cdb1.pvalue AS pyear, cdb2.pvalue AS pmonth, cdb3.pvalue AS pday,
                                            cb.cb_country AS CpCountry
                                    FROM    car_relation car
                                            LEFT JOIN car_serial cs ON car.cs_id = cs.cs_id
                                            LEFT JOIN class cl ON cs.carlevel = cl.classid
                                            LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                            LEFT JOIN cardatabase cdb1 ON car.car_id = cdb1.carid
                                                                          AND cdb1.paramid = 385
                                            LEFT JOIN cardatabase cdb2 ON car.car_id = cdb2.carid
                                                                          AND cdb2.paramid = 384
                                            LEFT JOIN cardatabase cdb3 ON car.car_id = cdb3.carid
                                                                          AND cdb3.paramid = 383
                                    WHERE   car.isstate = 0
                                            AND cs.isstate = 0
                                            AND cdb1.pvalue = '{0}'
                                            AND ISNUMERIC(cdb2.pvalue) >= 1
                                            AND ISNUMERIC(cdb3.pvalue) >= 1
                                    ORDER BY CONVERT(INT, cdb2.pvalue) DESC, CONVERT(INT, cdb3.pvalue) DESC,
                                            cs.cs_id, car.car_ReferPrice";
                    DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
                        , CommandType.Text, string.Format(sql, year));
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        // 白底
                        Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();
                        // 多个价格区间
                        Dictionary<int, string> dicCsMultiPriceRange = GetCsMultiPriceRange();

                        List<string> tempCsNode = new List<string>();
                        int lastCsID = 0;
                        DateTime lastDT = DateTime.Now;
                        decimal minP = 0;
                        decimal maxP = 0;
                        jsonString.Add("[");
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int csid = int.Parse(dr["cs_id"].ToString());
                            string csName = dr["csname"].ToString().Trim();
                            string csShowName = dr["csshowname"].ToString().Trim();
                            string csLevel = dr["carLevel"].ToString().Trim();
                            string csAllspell = dr["allspell"].ToString().Trim();
                            int carid = int.Parse(dr["car_id"].ToString());
                            string carName = dr["car_name"].ToString().Trim();
                            string cpCountry = dr["CpCountry"].ToString().Trim() == "90" ? "国产" : "进口";
                            int pyear = int.Parse(dr["pyear"].ToString());
                            int pmonth = int.Parse(dr["pmonth"].ToString());
                            int pday = int.Parse(dr["pday"].ToString());
                            DateTime tempDt = DateTime.Now.AddDays(1);
                            if (DateTime.TryParse(pyear + "-" + pmonth + "-" + pday, out tempDt))
                            {
                            }
                            if (tempDt >= DateTime.Now)
                            {
                                // 如果上市时间大于当前时间 则跳过这条记录 (默认时间大于当前时间，防止时间转换失败)
                                continue;
                            }

                            if (lastCsID != csid || tempDt != lastDT)
                            {
                                // 如果换子品牌或者 换上市时间 则将之前的数据补齐价格区间
                                if (lastCsID > 0)
                                {
                                    // 已有数据 补价格区间
                                    jsonString.Add(string.Format(",Price:\"{0}\",Range:[{1}]"
                                        , CommonFunction.GetUnicodeByString(GetPriceStr(minP, maxP))
                                        , dicCsMultiPriceRange.ContainsKey(lastCsID) ? dicCsMultiPriceRange[lastCsID] : ""));
                                    jsonString.Add("},");
                                    minP = 0;
                                    maxP = 0;
                                }
                                jsonString.Add("{");
                                jsonString.Add(string.Format("ID:\"{0}\",Name:\"{1}\",ShowName:\"{2}\",Level:\"{3}\",AllSpell:\"{4}\",Country:\"{5}\",MakeDay:\"{6}\",Pic:\"{7}\""
                                    , csid, CommonFunction.GetUnicodeByString(csName)
                                    , CommonFunction.GetUnicodeByString(csShowName)
                                    , CommonFunction.GetUnicodeByString(csLevel)
                                    , csAllspell
                                    , CommonFunction.GetUnicodeByString(cpCountry)
                                    , tempDt.ToString("yyyy-MM-dd")
                                    , dicPicWhite.ContainsKey(csid) ? dicPicWhite[csid] : WebConfig.DefaultCarPic));

                                lastCsID = csid;
                                lastDT = tempDt;
                            }
                            decimal referPrice = 0;
                            if (decimal.TryParse(dr["car_ReferPrice"].ToString().Trim(), out referPrice))
                            {
                                if (referPrice > maxP || maxP == 0)
                                { maxP = referPrice; }
                                if (referPrice < minP || minP == 0)
                                { minP = referPrice; }
                            }
                        }
                        jsonString.Add(string.Format(",Price:\"{0}\",Range:[{1}]"
                            , CommonFunction.GetUnicodeByString(GetPriceStr(minP, maxP))
                            , dicCsMultiPriceRange.ContainsKey(lastCsID) ? dicCsMultiPriceRange[lastCsID] : ""));
                        jsonString.Add("}");
                        jsonString.Add("]");
                    }
                    CacheManager.InsertCache(cacheKey, string.Join("", jsonString.ToArray()), 60);
                    #endregion
                }
            }
            else
            { }


        }

        private Dictionary<int, string> GetCsMultiPriceRange()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            XmlDocument xmlDoc = AutoStorageService.GetAllAutoAndLevelXml();
            if (xmlDoc != null && xmlDoc.HasChildNodes)
            {
                XmlNodeList serialList = xmlDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                foreach (XmlElement sNode in serialList)
                {
                    int csid = int.Parse(sNode.Attributes["ID"].Value.Trim());
                    string MultiPriceRange = sNode.Attributes["MultiPriceRange"].Value.Trim();
                    string multRang = GetAllPriceRangeByMinAndMaxPrice(MultiPriceRange);
                    if (multRang != "" && !dic.ContainsKey(csid))
                    { dic.Add(csid, multRang); }
                }
            }
            return dic;
        }

        /// <summary>
        /// 价格字符串
        /// </summary>
        /// <param name="minP"></param>
        /// <param name="maxP"></param>
        /// <returns></returns>
        private string GetPriceStr(decimal minP, decimal maxP)
        {
            string price = "暂无";
            if (minP > 0 && maxP > 0)
            {
                if (minP < maxP)
                {
                    price = minP + "万-" + maxP + "万";
                }
                else
                {
                    price = maxP + "万";
                }
            }
            return price;
        }

        /// <summary>
        /// 返回所属的价格区间
        /// </summary>
        /// <param name="multiPriceRange"></param>
        /// <returns></returns>
        private string GetAllPriceRangeByMinAndMaxPrice(string multiPriceRange)
        {
            string range = "";
            if (!string.IsNullOrEmpty(multiPriceRange))
            {
                List<string> tempRange = new List<string>();
                string[] arrayTemp = multiPriceRange.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tempStr in arrayTemp)
                {
                    string key = "";
                    switch (tempStr)
                    {
                        case "1": key = string.Format("\"{0}\"", CommonFunction.GetUnicodeByString("8万以下")); break;
                        case "2": key = string.Format("\"{0}\"", CommonFunction.GetUnicodeByString("8万以下")); break;
                        case "3": key = string.Format("\"{0}\"", CommonFunction.GetUnicodeByString("8-12万")); break;
                        case "4": key = string.Format("\"{0}\"", CommonFunction.GetUnicodeByString("12-18万")); break;
                        case "5": key = string.Format("\"{0}\"", CommonFunction.GetUnicodeByString("18-25万")); break;
                        case "6": key = string.Format("\"{0}\"", CommonFunction.GetUnicodeByString("25-40万")); break;
                        case "7": key = string.Format("\"{0}\"", CommonFunction.GetUnicodeByString("40万以上")); break;
                        case "8": key = string.Format("\"{0}\"", CommonFunction.GetUnicodeByString("40万以上")); break;
                        default: break;
                    }
                    if (key != "" && !tempRange.Contains(key))
                    { tempRange.Add(key); }
                }
                if (tempRange.Count > 0)
                {
                    range = string.Join(",", tempRange.ToArray());
                }
            }
            return range;
        }

        /// <summary>
        /// 取参数
        /// </summary>
        private void GetPageParam()
        {
            string strCsID = this.Request.QueryString["csID"];
            if (!string.IsNullOrEmpty(strCsID) && int.TryParse(strCsID, out csID))
            { }

            if (!string.IsNullOrEmpty(this.Request.QueryString["op"]))
            { op = this.Request.QueryString["op"].ToString().ToLower(); }

            string strCsIDs = this.Request.QueryString["csIDList"];
            if (!string.IsNullOrEmpty(strCsIDs))
            {
                string[] csids = strCsIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (csids.Length > 0)
                {
                    foreach (string csItem in csids)
                    {
                        int csid = 0;
                        if (int.TryParse(csItem.Trim(), out  csid))
                        {
                            if (csid > 0 && !listCsID.Contains(csid))
                            { listCsID.Add(csid); }
                        }
                    }
                }
            }
            callback = this.Request.QueryString["callback"];

            jsonVarName = this.Request.QueryString["jsonVarName"];
        }

        /// <summary>
        /// 生成数据
        /// </summary>
        private void GetCsJsonData()
        {
            if (csID > 0)
            {
                CarNewsBll newsBll = new CarNewsBll();
                Dictionary<string, string> dicCsBaseInfo = new Car_SerialBll().GetSerialBaseInfoFromMemCache(csID);
                if (dicCsBaseInfo != null && dicCsBaseInfo.Count > 0)
                {
                    string allSpell = dicCsBaseInfo.ContainsKey("CsAllSpell") ? dicCsBaseInfo["CsAllSpell"] : "";
                    jsonString.Add("{");
                    jsonString.Add("CsID:\"" + (dicCsBaseInfo.ContainsKey("CsID") ? dicCsBaseInfo["CsID"] : "") + "\"");
                    jsonString.Add(",CsName:\"" + (dicCsBaseInfo.ContainsKey("CsName") ? CommonFunction.GetUnicodeByString(dicCsBaseInfo["CsName"]) : "") + "\"");
                    jsonString.Add(",CsSummaryLink:\"http://car.m.yiche.com/" + allSpell + "/\"");
                    jsonString.Add(",CsPeiZhiLink:\"http://car.m.yiche.com/" + allSpell + "/peizhi/\"");
                    jsonString.Add(",CsTuPianLink:\"http://photo.m.yiche.com/serial/" + csID + "/\"");
                    jsonString.Add(",CsYouHaoLink:\"http://car.m.yiche.com/" + allSpell + "/youhao/\"");

                    string xiangjie = "http://car.m.yiche.com/" + allSpell + "/wenzhang/";
                    #region 车型详解逻辑
                    //// 是否有评测
                    //if (dicCsBaseInfo.ContainsKey("CsPingCe") && dicCsBaseInfo["CsPingCe"] != "")
                    //{ xiangjie = "http://car.m.yiche.com/" + allSpell + "/pingce/"; }
                    //// 是否有试驾
                    //else if (newsBll.IsSerialNews(csID, 0, CarNewsType.shijia))
                    //{ xiangjie = "http://car.m.yiche.com/" + allSpell + "/pingce/"; }
                    //// 是否有导购
                    //else if (newsBll.IsSerialNews(csID, 0, CarNewsType.daogou))
                    //{ xiangjie = "http://car.m.yiche.com/" + allSpell + "/pingce/"; }
                    //// 是否有用车
                    //else if (newsBll.IsSerialNews(csID, 0, CarNewsType.yongche))
                    //{ xiangjie = "http://car.m.yiche.com/" + allSpell + "/pingce/"; }
                    //// 是否有改装
                    //else if (newsBll.IsSerialNews(csID, 0, CarNewsType.gaizhuang))
                    //{ xiangjie = "http://car.m.yiche.com/" + allSpell + "/pingce/"; }
                    //// 是否有安全
                    //else if (newsBll.IsSerialNews(csID, 0, CarNewsType.anquan))
                    //{ xiangjie = "http://car.m.yiche.com/" + allSpell + "/pingce/"; }
                    //else
                    //{ }
                    #endregion
                    jsonString.Add(",CsXiangJieLink:\"" + xiangjie + "\"");

                    jsonString.Add(",CsKouBeiLink:\"http://car.m.yiche.com/" + allSpell + "/koubei/\"");
                    jsonString.Add(",CsShiPinLink:\"http://car.m.yiche.com/" + allSpell + "/shipin/\"");
                    jsonString.Add(",CsBBS:\"" + (dicCsBaseInfo.ContainsKey("CsBBS") ? dicCsBaseInfo["CsBBS"].Replace("baa.bitauto.com", "baa.m.yiche.com") : "") + "\"");
                    jsonString.Add(",CsBaoJiaLink:\"http://price.m.yiche.com/nb" + csID + "/\"");
                    jsonString.Add(",CsUsedCarLink:\"http://m.taoche.com/" + allSpell + "/\"");
                    jsonString.Add(",CsYangHuLink:\"http://yanghu.m.yiche.com/xuanze/0/" + csID + "/?source=cxytab&atype&type=0\"");
                    jsonString.Add("}");
                }
            }
        }

        /// <summary>
        /// 浏览过的车型
        /// </summary>
        private void GetViewdCarList()
        {
            if (listCsID.Count > 0)
            {
                foreach (int csid in listCsID)
                {
                    CarNewsBll newsBll = new CarNewsBll();
                    Dictionary<string, string> dicCsBaseInfo = new Car_SerialBll().GetSerialBaseInfoFromMemCache(csid);
                    if (dicCsBaseInfo != null && dicCsBaseInfo.Count > 0)
                    {
                        string allSpell = dicCsBaseInfo.ContainsKey("CsAllSpell") ? dicCsBaseInfo["CsAllSpell"] : "";
                        string defaultPic = "";
                        string priceRange = "";
                        int picCount = 0;
                        this.GetSerialPicAndCountByCsID(csid, out defaultPic, out picCount, true);
                        if (defaultPic.Trim().Length == 0)
                            defaultPic = WebConfig.DefaultCarPic;
                        //defaultPic = defaultPic.Replace("_2.", "_5.");
                        priceRange = this.GetSerialPriceRangeByID(csid);


                        jsonString.Add(string.Format("{{CsID:\"{0}\",CsName:\"{1}\",CsImage:\"{2}\",CsPrice:\"{3}\",CsAllSpell:\"{4}\"}}",
                            (dicCsBaseInfo.ContainsKey("CsID") ? dicCsBaseInfo["CsID"] : ""),
                            (dicCsBaseInfo.ContainsKey("CsShowName") ? CommonFunction.GetUnicodeByString(dicCsBaseInfo["CsShowName"]) : ""),
                            defaultPic,
                            priceRange,
                            '/'+ allSpell));
                    }
                }

                string viewedCar = "var jjviewed={" + string.Format("\"nlist\":[{0}]"
                    , string.Join(",", jsonString.ToArray())) + "}";

                Response.Write(viewedCar);
            }
        }

        /// <summary>
        /// 输出
        /// </summary>
        private void ResponseJson()
        {
            if (jsonString.Count > 0)
            {
                // JsonP
                if (!string.IsNullOrEmpty(callback))
                {
                    jsonString.Insert(0, callback + "(");
                    jsonString.Add(")");
                }
                else if (!string.IsNullOrEmpty(jsonVarName))
                {
                    // 非jsonP
                    jsonString.Insert(0, "var " + jsonVarName + " = ");
                    jsonString.Add(";");
                }
                else
                { }
            }
            else
            {
                // JsonP
                if (!string.IsNullOrEmpty(callback))
                {
                    jsonString.Add(callback + "({})");
                }
                else if (!string.IsNullOrEmpty(jsonVarName))
                {
                    // 非jsonP
                    jsonString.Add("var " + jsonVarName + " = null;");
                }
                else
                { jsonString.Add("参数错误"); }
            }
            Response.Write(string.Concat(jsonString.ToArray()));
        }

        #endregion
    }
}