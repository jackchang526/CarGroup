using System;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Threading;
using System.Linq;

using BitAuto.CarChannel.BLL.Das;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.BLL.IndexManager;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;

using BitAuto.Utils;
using BitAuto.CarChannel.BLL.ucar.api;
using BitAuto.CarUtils.Define;
using System.Collections;
using BitAuto.CarChannel.Model.AppModel;
using BitAuto.CarChannel.Model.AppApi;

namespace BitAuto.CarChannel.BLL
{
    public class Car_SerialBll
    {
        private static readonly Car_SerialDal csd = new Car_SerialDal();

        private static readonly string _SerialAskNumber = "Data\\AllSerialAskNumber.xml";
        private static readonly string _SerialKouBeiNumber = "Data\\AllSerialKouBeiNumber.xml";
        private static readonly string _SerialImageNumber = "Data\\AllSerialImageNumber.xml";
        private static readonly string _SerialMaintanceMessage = "Data\\SerialNews\\Maintance\\Message\\{0}.html";
        private static readonly string _SerailMaintancePrice = "Data\\SerialNews\\Maintance\\Price\\{0}.html";
        private static readonly string _SerialMaintanceMassageDirectory = "Data\\SerialNews\\Maintance\\Message";
        private static readonly string _SerialMaintanceNews = "Data\\SerialNews\\Maintance\\News\\{0}.xml";
        private static readonly string _SerialFocusNews = "Data\\SerialNews\\FocusNews\\Serial_FocusNews_{0}.xml";
        private static readonly string _SerialNews = "Data\\SerialNews\\{1}\\Serial_All_News_{0}.xml";
        private static readonly string _SerialEditorComment = "Data\\SerialSet\\EditorComment.xml";

        public static Dictionary<string, int[]> KindCateDic;

        private int m_serialId;
        private int m_cityId;
        private static object m_updateLock;

        static Car_SerialBll()
        {
            KindCateDic = new Dictionary<string, int[]>();
            KindCateDic["xinwen"] = new int[] { 152, 34, 148, 146, 151, 153, 198, 145, 149, 123, 127, 2, 13, 210, 98 };
            KindCateDic["daogou"] = new int[] { 4, 179 };
            KindCateDic["shijia"] = new int[] { 29, 30 };
            KindCateDic["hangqing"] = new int[] { 3 };
            KindCateDic["yongche"] = new int[] { 87, 88, 143, 142, 86, 85, 173, 56, 54, 53, 55, 201 };
            KindCateDic["shipin"] = new int[] { 66, 74, 75, 227, 233, 234, 235, 236, 237, 275 };
            KindCateDic["pingce"] = new int[] { 31, 221 };
            m_updateLock = new object();
        }

        public Car_SerialBll()
        { }

        /// <summary>
        /// 取所有有效子品牌 id,子品牌名
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllValidSerial()
        {
            return csd.GetAllValidSerial();
        }

        /// <summary>
        /// 取所有有效子品牌 基本数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, EnumCollection.CsBaseInfo> GetAllCsBaseInfoDic()
        {
            Dictionary<int, EnumCollection.CsBaseInfo> dic = new Dictionary<int, EnumCollection.CsBaseInfo>();
            string cacheKey = "Car_SerialBll_GetAllCsBaseInfoDic";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj == null)
            {
                DataSet ds = csd.GetAllValidSerial();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int csid = int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString());
                        string csName = ds.Tables[0].Rows[i]["cs_name"].ToString().Trim();
                        string csAllSpell = ds.Tables[0].Rows[i]["allspell"].ToString().Trim().ToLower();
                        string csShowName = ds.Tables[0].Rows[i]["cs_showname"].ToString().Trim();
                        EnumCollection.CsBaseInfo cbi = new EnumCollection.CsBaseInfo();
                        cbi.CsID = csid;
                        cbi.CsName = csName;
                        cbi.CsAllSpell = csAllSpell;
                        cbi.CsShowName = csShowName;
                        if (!dic.ContainsKey(csid))
                        { dic.Add(csid, cbi); }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
            }
            else
            { dic = (Dictionary<int, EnumCollection.CsBaseInfo>)obj; }
            return dic;
        }

        /// <summary>
        /// 获取一条新闻的类别名称
        /// </summary>
        /// <param name="catePath"></param>
        /// <returns></returns>
        public static string GetNewsKind(string catePath)
        {
            string[] cateIds = catePath.Split(',');
            string kindName = "";
            foreach (string tempCateId in cateIds)
            {
                int cateId = 0;
                bool success = Int32.TryParse(tempCateId, out cateId);
                if (!success)
                    continue;
                bool isKind = false;
                foreach (string kind in KindCateDic.Keys)
                {
                    foreach (int id in KindCateDic[kind])
                    {
                        if (id == cateId)
                        {
                            isKind = true;
                            kindName = kind;
                            break;
                        }
                    }

                    if (isKind)
                        break;
                }

                if (isKind)
                    break;
            }
            return kindName;
        }

        public DataSet GetCSRBItemByIDs(string IDs)
        {
            return csd.GetCSRBItemByIDs(IDs);
        }

        /// <summary>
        /// 根据彩虹条ID取有彩虹条的子品牌ID、url
        /// </summary>
        /// <param name="rainbowItemID"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetAllSerialRainbowItemByRainbowItemID(int rainbowItemID)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string cacheKey = "Car_SerialBll_GetAllSerialRainbowItemByRainbowItemID_" + rainbowItemID.ToString();
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj == null)
            {
                dic = csd.GetAllSerialRainbowItemByRainbowItemID(rainbowItemID);
                CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
            }
            else
            {
                dic = (Dictionary<int, string>)obj;
            }
            return dic;
        }

        public Dictionary<string, DataSet> GetCSRainbowsList(int nRainbowDisplayStatus)
        {
            return csd.GetCSRainbowsList(nRainbowDisplayStatus);
        }

        public DataSet GetCSRainbowsListByID(int nCSID, string strCSRBItemIDs)
        {
            return csd.GetCSRainbowsListByID(nCSID, strCSRBItemIDs);
        }

        public bool IsDomesticCar(int nCSID)
        {
            return csd.IsDomesticCar(nCSID);
        }
        /// <summary>
        /// 取所有有效子品牌
        /// </summary>
        /// <returns></returns>
        public IList<Car_SerialEntity> Get_Car_SerialAll()
        {
            return csd.Get_Car_SerialAll();
        }

        /// <summary>
        /// 取品牌下子品牌
        /// </summary>
        /// <param name="cbID"></param>
        /// <returns></returns>
        public IList<Car_SerialEntity> Get_Car_SerialByCbID(int cbID)
        {
            return csd.Get_Car_SerialByCbID(cbID);
        }

        /// <summary>
        /// 根据子品牌ID取子品牌
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public Car_SerialEntity Get_Car_SerialByCsID(int csID)
        {
            return csd.Get_Car_SerialByCsID(csID);
        }

        //public DataSet GetSerialCityHotCompare(int cityID, int csID)
        //{
        //    string cacheKey = "GetSerialCityHotCompare_city" + cityID.ToString() + "_csID" + csID.ToString();
        //    DataSet ds = (DataSet)CacheManager.GetCachedData(cacheKey);
        //    if (ds == null)
        //    {
        //        ds = csd.GetSerialCityHotCompare(cityID, csID);
        //        CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
        //    }
        //    return ds;
        //}

        /// <summary>
        /// 根据子品牌ID取车型的信息，如排量，变速器
        /// </summary>
        /// <param name="csId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public DataSet GetCarExtendInfoBySerial(int csId, int cityId)
        {
            //string cacheKey = "Serial_" + csId + "_Car_City_" + cityId + "_ExtendInfo"; //不再使用

            string cacheKey = "Serial_" + csId + "_ExtendInfo";
            DataSet ds = (DataSet)CacheManager.GetCachedData(cacheKey);
            if (ds == null)
            {
                ds = csd.GetCarExtendInfoBySerial(csId);
                if (ds != null)
                    CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                //拷贝一个DataSet，以避免多线程中的冲突
                ds = ds.Copy();
                //此处取商家报价
                string priceFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialCityPrice\\cityPrice_" + csId + ".xml");
                XmlDocument priceDoc = CommonFunction.ReadXmlFromFile(priceFile);
                //此城市车型报价
                XmlElement serialNode = (XmlElement)priceDoc.SelectSingleNode("/PriceScope/Serial[@CityId=\"" + cityId + "\"]");

                ds.Tables[0].Columns.Add("Price");
                ds.Tables[0].Columns.Add("PriceUrl");
                ds.Tables[0].Columns.Add("PriceTendUrl");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int carId = Convert.ToInt32(row["Car_Id"]);
                    XmlElement carNode = (XmlElement)serialNode.SelectSingleNode("Car[@CarId=\"" + carId + "\"]");
                    if (carNode == null)
                        row["Price"] = "";
                    else
                    {
                        double minPrice = Convert.ToDouble(carNode.GetAttribute("MinPrice"));
                        double maxPrice = Convert.ToDouble(carNode.GetAttribute("MaxPrice"));
                        row["Price"] = minPrice.ToString() + "万-" + maxPrice + "万";
                    }

                    //处理变速箱显示
                    string transmissionType = Convert.ToString(row["UnderPan_TransmissionType"]);
                    int pos = transmissionType.IndexOf("挡") + 1;
                    if (pos < transmissionType.Length)
                        transmissionType = transmissionType.Substring(pos);
                    row["UnderPan_TransmissionType"] = transmissionType;

                    //报价URL
                    row["PriceUrl"] = "http://price.bitauto.com/car.aspx?newcarId=" + carId + "&citycode=" + cityId;

                    //走势图链接
                    row["PriceTendUrl"] = "http://price.bitauto.com/trend.aspx?newcarid=" + carId;

                }
            }
            return ds;
        }

        /// <summary>
        /// 为报价页生成子品牌列表
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <param name="serialNodes"></param>
        /// <param name="priceId"></param>
        /// <param name="isFirstPice"></param>
        /// <param name="isLastPrice"></param>
        /// <param name="priceText"></param>
        /// <param name="isHomePage">是否主页</param>
        public void RenderForPrice(List<string> htmlCode, List<XmlElement> serialNodes, int priceId,
            bool isFirstPice, bool isLastPrice, string priceText, bool isHomePage)
        {
            if (priceId <= 3 || priceId > 9)
                RenderForPrice1(htmlCode, serialNodes, isFirstPice, isLastPrice, priceText, isHomePage);
            else
                RenderForPrice2(htmlCode, serialNodes, isFirstPice, isLastPrice, priceText, isHomePage);
        }

        /// <summary>
        /// 按价格列表(新加label名)
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <param name="serialNodes"></param>
        /// <param name="priceId"></param>
        /// <param name="isFirstPice"></param>
        /// <param name="isLastPrice"></param>
        /// <param name="priceText"></param>
        /// <param name="isHomePage"></param>
        /// <param name="labelName"></param>
        public void RenderForPriceNew(List<string> htmlCode, List<XmlElement> serialNodes, int priceId,
            bool isFirstPice, bool isLastPrice, string priceText, bool isHomePage, string labelName)
        {
            if (priceId <= 3 || priceId > 9)
                RenderForPrice1New(htmlCode, serialNodes, isFirstPice, isLastPrice, priceText, isHomePage, labelName);
            else
                RenderForPrice2New(htmlCode, serialNodes, isFirstPice, isLastPrice, priceText, isHomePage, labelName);
        }

        /// <summary>
        /// 按规则1：取车身形式加级别
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <param name="serialNodes"></param>
        /// <param name="isLastPrice"></param>
        /// <param name="isHomePage">是否主页</param>
        private void RenderForPrice1(List<string> htmlCode, List<XmlElement> serialNodes, bool isFirstPrice, bool isLastPrice, string priceText, bool isHomePage)
        {
            string[] serialClasses = new string[] { "两厢轿车", "三厢轿车", "跑车", "SUV", "MPV", "其它" };
            Dictionary<string, List<XmlElement>> serialList = new Dictionary<string, List<XmlElement>>();
            foreach (XmlElement serialNode in serialNodes)
            {
                //取级别
                string level = serialNode.GetAttribute("CsLevel").ToUpper();
                if (level == "跑车" || level == "SUV" || level == "MPV" || level == "其它")
                {
                    if (!serialList.ContainsKey(level))
                        serialList[level] = new List<XmlElement>();
                    serialList[level].Add(serialNode);
                }
                else
                {
                    //取车身样式
                    string bodyType = serialNode.GetAttribute("BodyType");
                    if (bodyType.IndexOf("两厢") > -1)
                    {
                        bodyType = "两厢轿车";
                    }
                    else if (bodyType.IndexOf("三厢") > -1)
                    {
                        bodyType = "三厢轿车";
                    }
                    else
                        bodyType = "其它";

                    if (!serialList.ContainsKey(bodyType))
                        serialList[bodyType] = new List<XmlElement>();
                    serialList[bodyType].Add(serialNode);
                }
            }

            if (isHomePage)
                RenderForHomePageClassification(serialClasses, htmlCode, serialList, isFirstPrice, isLastPrice, priceText);
            else
                RenderForClassification(serialClasses, htmlCode, serialList, isFirstPrice, isLastPrice, priceText);
        }

        private void RenderForPrice1New(List<string> htmlCode, List<XmlElement> serialNodes, bool isFirstPrice, bool isLastPrice, string priceText, bool isHomePage, string labelName)
        {
            string[] serialClasses = new string[] { "两厢轿车", "三厢轿车", "跑车", "SUV", "MPV", "其它" };
            Dictionary<string, List<XmlElement>> serialList = new Dictionary<string, List<XmlElement>>();
            foreach (XmlElement serialNode in serialNodes)
            {
                //取级别
                string level = serialNode.GetAttribute("CsLevel").ToUpper();
                if (level == "跑车" || level == "SUV" || level == "MPV" || level == "其它")
                {
                    if (!serialList.ContainsKey(level))
                        serialList[level] = new List<XmlElement>();
                    serialList[level].Add(serialNode);
                }
                else
                {
                    //取车身样式
                    string bodyType = serialNode.GetAttribute("BodyType");
                    if (bodyType.IndexOf("两厢") > -1)
                    {
                        bodyType = "两厢轿车";
                    }
                    else if (bodyType.IndexOf("三厢") > -1)
                    {
                        bodyType = "三厢轿车";
                    }
                    else
                        bodyType = "其它";

                    if (!serialList.ContainsKey(bodyType))
                        serialList[bodyType] = new List<XmlElement>();
                    serialList[bodyType].Add(serialNode);
                }
            }

            //if (isHomePage)
            RenderForHomePageClassificationNew(serialClasses, htmlCode, serialList, isFirstPrice, isLastPrice, priceText, labelName);
            //else
            //    RenderForClassification(serialClasses, htmlCode, serialList, isFirstPrice, isLastPrice, priceText);
        }

        /// <summary>
        /// 按规则2：取系别加级别
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <param name="serialNodes"></param>
        /// <param name="isLastPrice"></param>
        /// <param name="isHomePage">是否主页</param>
        private void RenderForPrice2(List<string> htmlCode, List<XmlElement> serialNodes, bool isFirstPrice, bool isLastPrice, string priceText, bool isHomePage)
        {
            string[] serialClasses = new string[] { "日系轿车", "美系轿车", "欧系轿车", "韩系轿车", "自主品牌", "跑车", "SUV", "MPV", "其它" };
            Dictionary<string, List<XmlElement>> serialList = new Dictionary<string, List<XmlElement>>();
            foreach (XmlElement serialNode in serialNodes)
            {
                //取级别
                string level = serialNode.GetAttribute("CsLevel").ToUpper();
                if (level == "跑车" || level == "SUV" || level == "MPV" || level == "其它")
                {
                    if (!serialList.ContainsKey(level))
                        serialList[level] = new List<XmlElement>();
                    serialList[level].Add(serialNode);
                }
                else
                {
                    //取主品牌国家
                    string countryName = serialNode.ParentNode.ParentNode.Attributes["Country"].Value;

                    if (countryName == "日本")
                    {
                        countryName = "日系轿车";
                    }
                    else if (countryName == "美国")
                    {
                        countryName = "美系轿车";
                    }
                    else if (countryName == "德国" || countryName == "法国" || countryName == "英国"
                   || countryName == "意大利" || countryName == "瑞典" || countryName == "捷克")
                    {
                        countryName = "欧系轿车";
                    }
                    else if (countryName == "韩国")
                    {
                        countryName = "韩系轿车";
                    }
                    else if (countryName == "中国")
                    {
                        countryName = "自主品牌";
                    }
                    else
                        countryName = "其它";

                    if (!serialList.ContainsKey(countryName))
                        serialList[countryName] = new List<XmlElement>();
                    serialList[countryName].Add(serialNode);
                }
            }

            if (isHomePage)
                RenderForHomePageClassification(serialClasses, htmlCode, serialList, isFirstPrice, isLastPrice, priceText);
            else
                RenderForClassification(serialClasses, htmlCode, serialList, isFirstPrice, isLastPrice, priceText);
        }

        private void RenderForPrice2New(List<string> htmlCode, List<XmlElement> serialNodes, bool isFirstPrice, bool isLastPrice, string priceText, bool isHomePage, string labelName)
        {
            string[] serialClasses = new string[] { "日系轿车", "美系轿车", "欧系轿车", "韩系轿车", "自主品牌", "跑车", "SUV", "MPV", "其它" };
            Dictionary<string, List<XmlElement>> serialList = new Dictionary<string, List<XmlElement>>();
            foreach (XmlElement serialNode in serialNodes)
            {
                //取级别
                string level = serialNode.GetAttribute("CsLevel").ToUpper();
                if (level == "跑车" || level == "SUV" || level == "MPV" || level == "其它")
                {
                    if (!serialList.ContainsKey(level))
                        serialList[level] = new List<XmlElement>();
                    serialList[level].Add(serialNode);
                }
                else
                {
                    //取主品牌国家
                    string countryName = serialNode.ParentNode.ParentNode.Attributes["Country"].Value;

                    if (countryName == "日本")
                    {
                        countryName = "日系轿车";
                    }
                    else if (countryName == "美国")
                    {
                        countryName = "美系轿车";
                    }
                    else if (countryName == "德国" || countryName == "法国" || countryName == "英国"
                   || countryName == "意大利" || countryName == "瑞典" || countryName == "捷克")
                    {
                        countryName = "欧系轿车";
                    }
                    else if (countryName == "韩国")
                    {
                        countryName = "韩系轿车";
                    }
                    else if (countryName == "中国")
                    {
                        countryName = "自主品牌";
                    }
                    else
                        countryName = "其它";

                    if (!serialList.ContainsKey(countryName))
                        serialList[countryName] = new List<XmlElement>();
                    serialList[countryName].Add(serialNode);
                }
            }

            //if (isHomePage)
            RenderForHomePageClassificationNew(serialClasses, htmlCode, serialList, isFirstPrice, isLastPrice, priceText, labelName);
            //else
            //    RenderForClassification(serialClasses, htmlCode, serialList, isFirstPrice, isLastPrice, priceText);
        }

        private void RenderForClassification(string[] serialClasses, List<string> htmlCode,
            Dictionary<string, List<XmlElement>> serialList, bool isFirstPrice, bool isLastPrice, string priceText)
        {
            //生成Html
            bool isFirstClass = true;
            int classCounter = 0;
            foreach (string serClass in serialClasses)
            {
                if (!serialList.ContainsKey(serClass))
                    continue;

                classCounter++;
                string anchor = GetanchorName(serClass);

                htmlCode.Add("<div class=\"line_box newbyl\">");
                htmlCode.Add("<h3" + anchor + "><span>" + serClass + "</span><label> " + priceText + "</label></h3>");
                htmlCode.Add("<div class=\"byletters_list\">");
                RenderSerialsByPv(htmlCode, serialList[serClass], true);

                if (isLastPrice && classCounter == serialList.Count)
                    htmlCode.Add("<div class=\"hideline\"></div>");

                htmlCode.Add("</div>");
                htmlCode.Add("<div class=\"clear\"></div>");
                if (!isFirstClass || !isFirstPrice)
                {
                    htmlCode.Add("<div class=\"more\"><a href=\"#pageTop\">返回顶部↑</a></div>");
                }
                if (isFirstClass)
                    isFirstClass = false;

                htmlCode.Add("</div>");
            }
        }

        private void RenderForHomePageClassification(string[] serialClasses, List<string> htmlCode,
                        Dictionary<string, List<XmlElement>> serialList, bool isFirstPrice, bool isLastPrice, string priceText)
        {
            //生成Html
            bool isFirstClass = true;
            int classCounter = 0;
            foreach (string serClass in serialClasses)
            {
                if (!serialList.ContainsKey(serClass))
                    continue;

                classCounter++;
                string anchor = GetanchorName(serClass);

                if (isFirstClass && isFirstPrice)
                {
                    htmlCode.Add("<dt" + anchor + "><label>" + serClass + "</label>  (" + priceText + ")</dt>");
                    isFirstClass = false;
                }
                else
                    htmlCode.Add("<dt" + anchor + "><label>" + serClass + "</label><a href=\"#pageTop\" class=\"gotop\">返回顶部↑</a>  (" + priceText + ")</dt>");

                htmlCode.Add("<dd>");


                RenderSerialsByPv(htmlCode, serialList[serClass], true);

                if (isLastPrice && classCounter == serialList.Count)
                    htmlCode.Add("<div class=\"hideline\"></div>");

                htmlCode.Add("</dd>");
            }
        }

        private void RenderForHomePageClassificationNew(string[] serialClasses, List<string> htmlCode,
                        Dictionary<string, List<XmlElement>> serialList, bool isFirstPrice, bool isLastPrice, string priceText, string labelName)
        {
            //生成Html
            bool isFirstClass = true;
            int classCounter = 0;
            foreach (string serClass in serialClasses)
            {
                if (!serialList.ContainsKey(serClass))
                    continue;

                classCounter++;
                //string anchor = GetanchorName(serClass);

                //if (isFirstClass && isFirstPrice)
                //{
                //    htmlCode.Add("<dt><label>" + serClass + "</label><div><span id=\"" + labelName + "\">&nbsp;</span></div>  (" + priceText + ")</dt>");
                //    isFirstClass = false;
                //}
                //else
                //    htmlCode.Add("<dt><label>" + serClass + "</label><div><span id=\"" + labelName + "\">&nbsp;</span></div><a href=\"#pageTop\" class=\"gotop\">返回顶部↑</a>  (" + priceText + ")</dt>");

                //htmlCode.Add("<dd>");


                //RenderSerialsByPv(htmlCode, serialList[serClass], true);

                //if (isLastPrice && classCounter == serialList.Count)
                //    htmlCode.Add("<div class=\"hideline\"></div>");

                //htmlCode.Add("</dd>");
                htmlCode.Add("<dd class=\"b\"><div class=\"brandname\"  ><span>" + serClass + "</span></div></dd>");
                htmlCode.Add("<dd class=\"have\">");
                RenderSerialsByPv(htmlCode, serialList[serClass], true);
                if (isLastPrice && classCounter == serialList.Count)
                    htmlCode.Add("<div class=\"hideline\"></div>");
                htmlCode.Add("</dd>");
                htmlCode.Add("<dd class=\"line\"></dd>");
            }
        }

        /// <summary>
        /// 获取锚点名称
        /// </summary>
        /// <param name="serialClass"></param>
        /// <returns></returns>
        private string GetanchorName(string serialClass)
        {
            string anchorName = "";
            switch (serialClass)
            {
                case "日系轿车":
                    anchorName = " id=\"rx\"";
                    break;
                case "欧系轿车":
                    anchorName = " id=\"ox\"";
                    break;
                case "跑车":
                    anchorName = " id=\"L9\"";
                    break;
                case "SUV":
                    anchorName = " id=\"L8\"";
                    break;
                case "两厢轿车":
                    anchorName = " id=\"lx\"";
                    break;
                case "三厢轿车":
                    anchorName = " id=\"sx\"";
                    break;
                case "其它":
                    anchorName = " id=\"l10\"";
                    break;
            }
            return anchorName;
        }

        /// <summary>
        /// 生成子品牌的Html
        /// </summary>
        /// <param name="htmlCode">代码容器</param>
        /// <param name="brandNode">品牌信息</param>
        public void RenderSerialsByPv(List<string> htmlCode, List<XmlElement> serialList, bool isShowName)
        {
            RenderSerials(htmlCode, serialList, "PV", true, isShowName);
        }

        /// <summary>
        /// 按拼音排序生成子品牌的Html
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <param name="serialList"></param>
        public void RenderSerialsBySpell(List<string> htmlCode, List<XmlElement> serialList, bool isShowName)
        {
            RenderSerials(htmlCode, serialList, "SPELL", true, isShowName);
        }

        /// <summary>
        /// 生成子品牌Html
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <param name="serialList"></param>
        /// <param name="sort"></param>
        /// <param name="hasLevel">是否带级别链接</param>
        /// <param name="isShowName">是否显示显示名，否则显示名称</param>
        private void RenderSerials(List<string> htmlCode, List<XmlElement> serialList, string sort, bool hasLevel, bool isShowName)
        {
            htmlCode.Add("<ul>");

            if (sort.ToUpper() == "PV")
            {
                //按关注度排序
                serialList.Sort(NodeCompare.CompareSerialByPvDesc);
            }
            else
            {
                serialList.Sort(NodeCompare.CompareSerialBySpellAsc);
            }

            // add by chengl Dec.5.2013
            // 年度十佳车
            List<BestTopCar> listAllBestCar = Car_SerialBll.GetAllBestTopCar();

            ////十佳车型
            //Dictionary<int, string> bestCarDic = Car_SerialBll.GetBestCarTop10();

            //// add by chengl Feb.22.2012
            //// 2012十佳车型
            //Dictionary<int, string> bestCarDic2012 = Car_SerialBll.GetBestCarTop10ByYear(2012);

            foreach (XmlElement serialNode in serialList)
            {
                htmlCode.Add("<li>");
                string serialId = serialNode.GetAttribute("ID");
                string serialName = "";
                if (isShowName)
                    serialName = serialNode.GetAttribute("ShowName");
                else
                    serialName = serialNode.GetAttribute("Name");

                if (serialId == "1568")
                    serialName = "索纳塔八";

                string serialLevel = serialNode.GetAttribute("CsLevel");
                string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();
                //EnumCollection.SerialLevelEnum levelEnum = (EnumCollection.SerialLevelEnum)Enum.Parse(typeof(EnumCollection.SerialLevelEnum), serialLevel);
                int hasNew = Convert.ToInt32(serialNode.GetAttribute("CsHasNew"));
                htmlCode.Add("<div class=\"name\"><a href=\"/" + serialSpell + "/\" target=\"_blank\">" + serialName + "</a>");

                //是否带车型级别
                if (hasLevel && serialLevel != "其它")
                {
                    //htmlCode.Add("<a href=\"/" + ((EnumCollection.SerialLevelSpellEnum)levelEnum).ToString() + "/\" class=\"classify\" target=\"_blank\">[" + serialLevel + "]</a>");
                    var levelSpell = CarLevelDefine.GetLevelSpellByName(serialLevel);
                    htmlCode.Add("<a href=\"/" + levelSpell + "/\" class=\"classify\" target=\"_blank\">[" + serialLevel + "]</a>");
                }
                if (hasNew == 1)
                {
                    htmlCode.Add("<span class=\"new\">新</span>");
                }

                string bestCarStr = "";

                // 新10佳车 add by chengl Dec.5.2013
                if (listAllBestCar != null && listAllBestCar.Count > 0)
                {
                    foreach (BestTopCar btc in listAllBestCar)
                    {
                        if (btc.ListCsList.Contains(ConvertHelper.GetInteger(serialId)))
                        {
                            bestCarStr = " <a href=\"" + btc.Link + "\" target=\"_blank\"><img class=\"ico_shijia\" src=\"http://image.bitautoimg.com/uimg/car/images/ico_shijiache.gif\" alt=\"" + btc.Title + "\" title=\"" + btc.Title + "\" /></a>";
                            break;
                        }
                    }
                }

                //if (bestCarDic.ContainsKey(ConvertHelper.GetInteger(serialId)))
                //    bestCarStr = " <a href=\"http://www.bitauto.com/top10cars/gd_2011/\" target=\"_blank\"><img src=\"http://image.bitautoimg.com/uimg/car/images/ico_shijiache.gif\" alt=\"2011年度十佳车\" title=\"2011年度十佳车\" align=\"top\" /></a>";
                //// add by chengl Feb.22.2012
                //if (bestCarDic2012.ContainsKey(ConvertHelper.GetInteger(serialId)))
                //{
                //    bestCarStr = " <a href=\"http://www.bitauto.com/top10cars/\" target=\"_blank\"><img src=\"http://image.bitautoimg.com/uimg/car/images/ico_shijiache.gif\" alt=\"2012年度十佳车\" title=\"2012年度十佳车\" align=\"top\" /></a>";
                //}

                htmlCode.Add(bestCarStr);
                htmlCode.Add("</div>");
                /*
				htmlCode.Append("<div><a href=\"http://car.bitauto.com/" + serialSpell + "/baojia/\" target=\"_blank\">报价</a>");
				htmlCode.Append("<a href=\"http://photo.bitauto.com/serial/" + serialId + "\" target=\"_blank\">图片</a>");
				htmlCode.AppendLine("<a href=\"" + GetForumUrlBySerialId(Convert.ToInt32(serialId)) + "\" target=\"_blank\">论坛</a></div>");
				*/
                string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(serialId));
                if (priceRange.Trim().Length == 0)
                    htmlCode.Add("<div class=\"bj\">暂无报价</div>");
                else
                    htmlCode.Add("<div class=\"bj\">" + priceRange + "</div>");
                htmlCode.Add("</li>");
            }
            htmlCode.Add("</ul>");
        }

        /// <summary>
        /// 生成不带级别链接的子品牌列表
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <param name="serialList"></param>
        public void RenderSerialsByPVNoLevel(List<string> htmlCode, List<XmlElement> serialList, bool isShowName)
        {
            RenderSerials(htmlCode, serialList, "PV", false, isShowName);
        }

        /// <summary>
        /// 生成不带级别的按拼写排序的子品牌列表
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <param name="serialList"></param>
        public void RenderSerialsBySpellNoLevel(List<string> htmlCode, List<XmlElement> serialList, bool isShowName)
        {
            RenderSerials(htmlCode, serialList, "SPELL", false, isShowName);
        }

        public void RenderSerialsByImage(List<string> htmlCode, List<XmlElement> serialList)
        {
            //按关注度排序
            serialList.Sort(NodeCompare.CompareSerialByPvDesc);
            htmlCode.Add("<div class=\"jblist\" id=\"alljb\">");
            htmlCode.Add("<ul class=\"l\" id=\"jbheight\" style=\"height: 750px\">");

            //图片Url
            int sCounter = 0;
            string imgUrlArray = "";
            foreach (XmlElement serialNode in serialList)
            {
                sCounter++;
                int sId = Convert.ToInt32(serialNode.GetAttribute("ID"));
                string sName = serialNode.GetAttribute("ShowName");
                string allSpell = serialNode.GetAttribute("AllSpell");

                string realUrl = GetSerialCoverHashImgUrl(sId);

                //生成Html
                htmlCode.Add("<li><span");
                if (sCounter <= 3)
                    htmlCode.Add(" class=\"q3\"");
                htmlCode.Add(">" + sCounter + "</span><div><a href=\"http://car.bitauto.com/" + allSpell + "/\" target=\"_blank\">");
                if (sCounter <= 30)
                    htmlCode.Add("<img src=\"" + realUrl + "\" alt=\"" + sName + "\" />");
                else
                {
                    htmlCode.Add("<img src=\"\" alt=\"" + sName + "\" />");
                    imgUrlArray += "carlist[" + (sCounter - 1) + "].src=\"" + realUrl + "\";\r\n";
                }
                htmlCode.Add("</a><a href=\"http://car.bitauto.com/" + allSpell + "/\" target=\"_blank\">" + sName + "</a><p>");

                string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(sId));
                if (priceRange.Trim().Length == 0)
                    htmlCode.Add("暂无报价");
                else
                    htmlCode.Add(priceRange);
                htmlCode.Add("</p></div></li>");
            }

            htmlCode.Add("</ul>");
            htmlCode.Add("<div class=\"hideline\"></div>");
            htmlCode.Add("</div>");
            //显示全部的链接
            htmlCode.Add("<div id=\"divClear\" class=\"clear\"></div>");
            htmlCode.Add("</div>");
            if (sCounter > 30)
            {
                htmlCode.Add("<div id=\"showAllSerial\" class=\"car0518_01\"><span id=\"showallcar\">查看全部子品牌</span></div>");

                //输出脚本
                htmlCode.Add("<script>\r\n");
                htmlCode.Add("carlist=document.getElementById(\"alljb\").getElementsByTagName('img');\r\n");
                htmlCode.Add("for(i=30;i<carlist.length;i++){\r\n");
                htmlCode.Add("carlist[i].src=\"http://image.bitautoimg.com/autoalbum/V2.1/images/150-100.gif\";\r\n");
                htmlCode.Add("}\r\n");
                htmlCode.Add("document.getElementById(\"showallcar\").onclick=function (){\r\n");
                htmlCode.Add("document.getElementById(\"jbheight\").style.height =(document.getElementById(\"jbheight\").style.height == '750px'?'auto':'750px');\r\n ");
                htmlCode.Add("document.getElementById(\"showallcar\").innerHTML =(document.getElementById(\"showallcar\").innerHTML == '只显示前30名'?'查看全部子品牌':'只显示前30名'); \r\n");
                htmlCode.Add("document.getElementById(\"showallcar\").className =(document.getElementById(\"showallcar\").className == 'h'?'s':'h');\r\n");
                htmlCode.Add("showallcar();\r\n");
                htmlCode.Add("return false;\r\n");
                htmlCode.Add("};\r\n");
                htmlCode.Add("function showallcar() {");
                htmlCode.Add(imgUrlArray);
                htmlCode.Add("}");
                htmlCode.Add("</script>");
            }
        }

        /// <summary>
        /// 获取子品牌
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public DataSet GetSerialFocusImage(int serialId)
        {
            string xmlFile = "Serial_FocusImage_" + serialId + ".xml";
            xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\FocusImage\\" + xmlFile);
            DataSet ds = new DataSet();
            if (File.Exists(xmlFile))
            {
                ds.ReadXml(xmlFile);
            }

            return ds;
        }


        /// <summary>
        /// 子品牌新焦点图
        /// </summary>
        /// <param name="serialID"></param>
        /// <returns></returns>
        public XmlDocument GetSerialFocusImageForNew(int serialID)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                //图库接口本地化更改 by sk 2012.12.21
                string xmlFile = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialFocusImagePath, serialID));
                //string xmlFile = "Serial_FocusImage_" + serialID + ".xml";
                //xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\FocusImageNew\\" + xmlFile);
                if (File.Exists(xmlFile))
                {
                    doc.Load(xmlFile);
                }
            }
            catch (Exception ex)
            {

            }
            return doc;
        }
        /// <summary>
        /// 子品牌综述页 图片分类位置图片
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public XmlDocument GetSerialPositionImageXml(int serialId)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                //图库接口本地化更改 by sk 2012.12.21
                string xmlFile = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialPositionImagePath, serialId));
                if (File.Exists(xmlFile))
                {
                    doc.Load(xmlFile);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return doc;
        }
        /// <summary>
        /// 获取子品牌分类位置图片
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public SerialPositionEntity GetSerialPositionImage(int serialId)
        {
            //string cacheKey = "Car_Serial_PositionImage_Entity_" + serialId;
            //SerialPositionEntity positionImageEntity = (SerialPositionEntity)CacheManager.GetCachedData(cacheKey);
            //if (positionImageEntity == null)
            //{
            SerialPositionEntity positionImageEntity = new SerialPositionEntity();
            XmlDocument xmlDoc = this.GetSerialPositionImageXml(serialId);
            if (xmlDoc.HasChildNodes)
            {
                List<SerialCategoryEntity> cateList = new List<SerialCategoryEntity>();
                List<SerialPositionImagesEntity> positionImageList = new List<SerialPositionImagesEntity>();
                int imageCount = 0;
                string serialUrl = string.Empty;
                XmlNode rootNode = xmlDoc.SelectSingleNode("/SerialBrand");
                if (rootNode != null)
                {
                    imageCount = ConvertHelper.GetInteger(rootNode.Attributes["ImageCount"].Value);
                    serialUrl = rootNode.Attributes["Url"].Value;
                }
                positionImageEntity.ImageCount = imageCount;
                positionImageEntity.Url = serialUrl;
                XmlNodeList cateNodeList = xmlDoc.SelectNodes("/SerialBrand/Categories/Category");
                foreach (XmlNode node in cateNodeList)
                {
                    cateList.Add(new SerialCategoryEntity()
                    {
                        Id = ConvertHelper.GetInteger(node.Attributes["ID"].Value),
                        ImageCount = ConvertHelper.GetInteger(node.Attributes["ImageCount"].Value),
                        Name = node.Attributes["Name"].Value,
                        Url = node.Attributes["Url"].Value
                    });
                }
                positionImageEntity.SerialCategoryList = cateList;
                XmlNodeList positionImageNodeList = xmlDoc.SelectNodes("/SerialBrand/PositionImages/Image");
                foreach (XmlNode node in positionImageNodeList)
                {
                    positionImageList.Add(new SerialPositionImagesEntity()
                    {
                        ImageId = ConvertHelper.GetInteger(node.Attributes["ImageID"].Value),
                        ImageName = node.Attributes["ImageName"].Value,
                        ImageUrl = node.Attributes["ImageUrl"].Value,
                        CarModelId = ConvertHelper.GetInteger(node.Attributes["CarModelID"].Value),
                        PositionId = ConvertHelper.GetInteger(node.Attributes["PositionID"].Value),
                        PositionName = node.Attributes["PositionName"].Value,
                        Url = node.Attributes["Url"].Value
                    });
                }
                positionImageEntity.SerialPositionImageList = positionImageList;
                //        CacheManager.InsertCache(cacheKey, positionImageEntity, 30);
            }
            //}
            return positionImageEntity;
        }
        /// <summary>
        /// 子品牌 新版综述页 图片列表
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="count">数量，默认11张，1200版综述页取12张</param>
        /// <returns></returns>
        public List<XmlNode> GetSerialElevenPositionImage(int serialId, int count = 11)
        {
            List<XmlNode> list = new List<XmlNode>();
            string serialElevenImagePath = Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialElevenImagePath, serialId));
            XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(serialElevenImagePath);
            if (xmlDoc != null)
            {
                XmlNodeList nodeList = xmlDoc.SelectNodes("//CarImage");
                foreach (XmlNode node in nodeList)
                {
                    list.Add(node);
                }
            }
            //不够11张图片 按照该子品牌默认车款图片播放顺序
            if (list.Count < count)
            {
                string SerialDefaultCarFillImagePath = Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialDefaultCarFillImagePath, serialId));
                XmlDocument fillImageXml = CommonFunction.ReadXmlFromFile(SerialDefaultCarFillImagePath);
                if (fillImageXml != null)
                {
                    int loop = 0;
                    int beforCount = list.Count;
                    XmlNodeList nodeList = fillImageXml.SelectNodes("//CarImage");
                    foreach (XmlNode node in nodeList)
                    {
                        string imageId = node.Attributes["ImageId"].Value;
                        if (list.Find(p => p.Attributes["ImageId"].Value == imageId) != null) continue;
                        loop++;
                        if (loop > (count - beforCount)) break;
                        list.Add(node);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取子品牌的焦点图列表，包括广告的设置
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public List<SerialFocusImage> GetSerialFocusImageList(int serialId)
        {
            string cacheKey = "serial_forcus_image_List_" + serialId;
            List<SerialFocusImage> imgList = (List<SerialFocusImage>)CacheManager.GetCachedData(cacheKey);
            if (imgList == null)
            {
                imgList = new List<SerialFocusImage>();
                XmlDocument doc = GetSerialFocusImageForNew(serialId);
                Dictionary<int, SerialFocusImage> imgDic = GetSerialFocusImageAD();
                XmlNodeList xnl = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
                if (imgDic.ContainsKey(serialId))
                    imgList.Add(imgDic[serialId]);
                foreach (XmlElement imgNode in xnl)
                {
                    SerialFocusImage csImg = new SerialFocusImage();
                    csImg.ImageId = ConvertHelper.GetInteger(imgNode.GetAttribute("ImageId"));
                    csImg.ImageName = imgNode.GetAttribute("ImageName");
                    csImg.ImageUrl = imgNode.GetAttribute("ImageUrl");
                    if (csImg.ImageUrl.ToLower().IndexOf("bitautoimg.com") == -1)
                    {
                        csImg.ImageUrl = CommonFunction.GetPublishHashImageDomain(csImg.ImageId) + csImg.ImageUrl;
                    }
                    csImg.TargetUrl = imgNode.GetAttribute("Link");
                    csImg.GroupName = imgNode.GetAttribute("GroupName");
                    csImg.CarName = imgNode.GetAttribute("CarModelName");
                    imgList.Add(csImg);
                }

                CacheManager.InsertCache(cacheKey, imgList, 30);
            }

            return imgList;
        }
        /// <summary>
        /// 获取子品牌焦点图广告的列表
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public Dictionary<int, SerialFocusImage> GetSerialFocusImageAD()
        {
            string cacheKey = "Get_Serial_FocusImage_AD";
            Dictionary<int, SerialFocusImage> imgDic = (Dictionary<int, SerialFocusImage>)CacheManager.GetCachedData(cacheKey);
            if (imgDic == null)
            {
                imgDic = new Dictionary<int, SerialFocusImage>();
                string fileName = HttpContext.Current.Request.MapPath("~/App_Data/SerialFocusImgADConfig.xml");
                XmlDocument adDoc = new XmlDocument();
                if (File.Exists(fileName))
                {
                    adDoc.Load(fileName);
                    XmlNodeList serialeNodeList = adDoc.SelectNodes("/AdList/Serial");
                    foreach (XmlElement serialNode in serialeNodeList)
                    {
                        DateTime startDate = ConvertHelper.GetDateTime(serialNode.GetAttribute("startDate"));
                        DateTime endDate = ConvertHelper.GetDateTime(serialNode.GetAttribute("endDate"));
                        DateTime curDate = DateTime.Now;
                        if (startDate > curDate || endDate.AddDays(1) < curDate)
                            continue;
                        int serialId = ConvertHelper.GetInteger(serialNode.GetAttribute("id"));
                        if (!imgDic.ContainsKey(serialId))
                            imgDic[serialId] = new SerialFocusImage();
                        imgDic[serialId].ImageId = 0;
                        imgDic[serialId].ImageName = "";
                        imgDic[serialId].ImageUrl = serialNode.GetAttribute("imgUrl");
                        imgDic[serialId].TargetUrl = serialNode.GetAttribute("targetUrl");
                    }

                    CacheManager.InsertCache(cacheKey, imgDic, 30);
                }
            }
            return imgDic;
        }

        /// <summary>
        /// 获取所有子品牌的友情链接的设置
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetSerialFriendLinkSetting()
        {
            string cacheKey = "Get_Serial_FriendLink_Setting";
            Dictionary<int, string> flDic = (Dictionary<int, string>)CacheManager.GetCachedData(cacheKey);
            if (flDic == null)
            {
                flDic = new Dictionary<int, string>();
                string fileName = HttpContext.Current.Request.MapPath("~/App_Data/SerialFriendLink.xml");
                XmlDocument flDoc = new XmlDocument();
                if (File.Exists(fileName))
                {
                    flDoc.Load(fileName);
                    XmlNodeList flNodeList = flDoc.SelectNodes("/Links/Serial");
                    foreach (XmlElement serialNode in flNodeList)
                    {
                        int csId = ConvertHelper.GetInteger(serialNode.GetAttribute("id"));
                        flDic[csId] = serialNode.GetAttribute("file");
                    }
                    CacheManager.InsertCache(cacheKey, flDic, 30);
                }
            }
            return flDic;
        }

        /// <summary>
        /// 获取子品牌的友情链接内容
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public string GetSerialFriendLinkHtml(int serialId)
        {
            string cacheKey = "Get_Serial_FriendLink_Html_" + serialId;
            string flHtml = (string)CacheManager.GetCachedData(cacheKey);
            if (flHtml == null)
            {
                Dictionary<int, string> flDic = GetSerialFriendLinkSetting();
                if (!flDic.ContainsKey(serialId))
                    flHtml = String.Empty;
                else
                {
                    string fileUrl = "http://news.bitauto.com" + flDic[serialId];
                    string incHtml = String.Empty;
                    try
                    {
                        WebClient wc = new WebClient();
                        incHtml = wc.DownloadString(fileUrl);
                    }
                    catch { }

                    if (incHtml.Length == 0)
                        flHtml = String.Empty;
                    else
                    {
                        StringBuilder htmlCode = new StringBuilder();
                        htmlCode.Append("<div class=\"col-con\">");
                        htmlCode.Append("<div class=\"newlink\"><h5>友情链接：</h5>");
                        htmlCode.Append("<ul id=\"dealer_logo\">");
                        htmlCode.Append(incHtml);
                        htmlCode.Append("</ul>");
                        htmlCode.Append("<div class=\"more\"><a href=\"http://www.bitauto.com/yqlj.shtml\" target=\"_blank\">更多&gt;&gt;</a></div>");
                        htmlCode.Append("</div></div>");
                        flHtml = htmlCode.ToString();
                        CacheManager.InsertCache(cacheKey, flHtml, 30);
                    }
                }

            }
            return flHtml;
        }

        /// <summary>
        /// 获取子品牌综述页的
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public XmlDocument GetSerialFocusNews(int serialId)
        {
            string cacheKey = "Serial_FocusNews_" + serialId;
            XmlDocument xmlDoc = (XmlDocument)CacheManager.GetCachedData(cacheKey);
            if (xmlDoc == null)
            {
                string xmlFile = "Serial_FocusNews_" + serialId + ".xml";
                xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\FocusNews\\Xml\\" + xmlFile);
                xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);
                CacheManager.InsertCache(cacheKey, xmlDoc, WebConfig.CachedDuration);
            }
            return xmlDoc;
        }
        /// <summary>
        /// 获取子品牌论坛新闻
        /// </summary>
        /// <param name="serialId">子品牌ID</param>
        /// <returns></returns>
        public XmlDocument GetSerialForumNews(int serialId)
        {
            string cacheKey = "Car_SerialBll_GetSerialForumNews_" + serialId;
            XmlDocument xmlDoc = (XmlDocument)CacheManager.GetCachedData(cacheKey);
            if (xmlDoc == null)
            {
                string xmlFile = serialId + ".xml";
                xmlFile = Path.Combine(WebConfig.DataBlockPath, @"Data\SerialNews\Forum\" + xmlFile);
                xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);
                CacheManager.InsertCache(cacheKey, xmlDoc, WebConfig.CachedDuration);
            }
            return xmlDoc;
        }

        /// <summary>
        /// 获取子品牌的热点新闻
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public XmlDocument GetSerialHotNews(int serialId)
        {
            string cacheKey = "GetSerialHotNews_" + serialId;
            XmlDocument xmlDoc = (XmlDocument)CacheManager.GetCachedData(cacheKey);
            if (xmlDoc == null)
            {
                string xmlFile = "Serial_Hot_News_" + serialId + ".xml";
                xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\SerialHotNews\\" + xmlFile);
                xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);

                CacheManager.InsertCache(cacheKey, xmlDoc, WebConfig.CachedDuration);
            }
            return xmlDoc;
        }

        /// <summary>
        /// 根据子品牌ID及分类获取新闻
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="kind">新闻类型，市场或导购</param>
        /// <returns></returns>
        public DataSet GetNewsListBySerialId(int serialId, string kind)
        {
            return GetNewsListBySerialId(serialId, kind, 0);
        }

        /// <summary>
        /// 根据子品牌ID\年款及分类获取新闻
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="kind">新闻类型，市场或导购</param>
        /// <param name="year">子品牌年款</param>
        /// <returns></returns>
        public DataSet GetNewsListBySerialId(int serialId, string kind, int year)
        {
            string cacheKey = "GetNewsListBySerial_" + serialId + "_" + kind + "_" + year;
            DataSet newsDs = (DataSet)CacheManager.GetCachedData(cacheKey);
            if (newsDs == null)
            {
                if (kind == "pingce")
                    kind = "pingce\\xml";
                newsDs = new DataSet();
                try
                {
                    string xmlFile = "";
                    if (year == 0)
                        xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\" + kind + "\\" + "Serial_All_News_" + serialId + ".xml");
                    else
                        xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\" + kind + "\\" + "Serial_All_News_" + serialId + "_" + year + ".xml");
                    newsDs.ReadXml(xmlFile);
                }
                catch
                { }

                CacheManager.InsertCache(cacheKey, newsDs, WebConfig.CachedDuration);
            }
            return newsDs;
        }

        /// <summary>
        /// 根据子品牌全拼获取子品牌ID
        /// </summary>
        /// <param name="serialSpell"></param>
        /// <returns></returns>
        public int GetSerialIdBySpell(string serialSpell)
        {
            // modified by chengl Nov.20.2009
            XmlDocument xmlDoc = AutoStorageService.GetAllAutoXml();
            // XmlDocument xmlDoc = AutoStorageService.GetAutoXml();
            XmlElement serialNode = (XmlElement)xmlDoc.SelectSingleNode("/Params/MasterBrand/Brand/Serial[@AllSpell=\"" + serialSpell.Trim().ToLower() + "\"]");
            int serialId = 0;
            if (serialNode != null)
                serialId = ConvertHelper.GetInteger(serialNode.GetAttribute("ID"));
            return serialId;
        }

        /// <summary>
        /// 根据子品牌ID获取子品牌的基础信息
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public Car_SerialBaseEntity GetSerialBaseEntity(int serialId)
        {
            string cacheKey = "Serial_BaseEntity_Dic";
            Dictionary<int, Car_SerialBaseEntity> serialBaseDic = (Dictionary<int, Car_SerialBaseEntity>)CacheManager.GetCachedData(cacheKey);

            if (serialBaseDic == null)
            {
                // modify chengl Sep.25.2009 and modify
                // string xmlFile = Path.Combine(WebConfig.WebRootPath, "Data\\autodata.xml");
                // string xmlFile = "http://carser.bitauto.com/forpicmastertoserial/MasterToBrandToSerialAllSale.xml";

                // modified by chengl May.29.2012 开放概念车
                // string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\AllAutoData.xml");
                //string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\MasterToBrandToSerialAllSaleAndLevel.xml");

                serialBaseDic = new Dictionary<int, Car_SerialBaseEntity>();
                XmlDocument xmlDoc = new XmlDocument();
                //modified by sk 2013.04.26 增加文件读取失败，换数据源
                xmlDoc = AutoStorageService.GetAllAutoAndLevelXml();
                //xmlDoc.Load(xmlFile);
                XmlNodeList serialList = xmlDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                foreach (XmlElement sNode in serialList)
                {
                    Car_SerialBaseEntity serialBase = new Car_SerialBaseEntity();

                    serialBase.SerialId = Convert.ToInt32(sNode.GetAttribute("ID"));
                    serialBase.SerialName = sNode.GetAttribute("Name");
                    serialBase.SerialNameSpell = sNode.GetAttribute("AllSpell");
                    serialBase.SerialLevel = sNode.GetAttribute("CsLevel");
                    serialBase.SerialShowName = sNode.GetAttribute("ShowName");
                    serialBase.SerialSeoName = sNode.GetAttribute("SerialSEOName");
                    // modified by chengl May.29.2012 开放概念车
                    //int levelId = 0;
                    //try
                    //{
                    //    levelId = (int)System.Enum.Parse(typeof(EnumCollection.SerialAllLevelEnum), serialBase.SerialLevel);
                    //}
                    //catch (Exception ex)
                    //{ }
                    //serialBase.SerialLevelSpell = ((EnumCollection.SerialAllLevelSpellEnum)levelId).ToString();
                    serialBase.SerialLevelSpell = CarLevelDefine.GetLevelSpellByName(serialBase.SerialLevel);
                    //品牌名
                    XmlElement bNode = (XmlElement)sNode.ParentNode;
                    if (bNode != null)
                    {
                        serialBase.BrandName = bNode.GetAttribute("Name");
                        serialBase.BrandSpell = bNode.GetAttribute("AllSpell").ToLower();
                        XmlElement mNode = (XmlElement)bNode.ParentNode;
                        if (mNode != null)
                        {
                            serialBase.MasterBrandName = mNode.GetAttribute("Name");
                            serialBase.MasterbrandId = ConvertHelper.GetInteger(mNode.GetAttribute("ID"));
                            serialBase.MasterbrandSpell = mNode.GetAttribute("AllSpell");
                        }
                        else
                        {
                            serialBase.MasterBrandName = "";
                            serialBase.MasterbrandSpell = "";
                        }
                    }
                    else
                    {
                        serialBase.BrandName = "";
                        serialBase.BrandSpell = "";
                        serialBase.MasterBrandName = "";
                    }

                    serialBaseDic[serialBase.SerialId] = serialBase;


                }
                CacheManager.InsertCache(cacheKey, serialBaseDic, WebConfig.CachedDuration);
            }

            if (serialBaseDic.ContainsKey(serialId))
                return serialBaseDic[serialId];
            else
                return null;

        }

        /// <summary>
        /// 获取子品牌视频
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public XmlNodeList GetSerialVideo(int serialId)
        {
            string cacheKey = "serialVideoNodeList_" + serialId;
            XmlNodeList videoNodeList = (XmlNodeList)CacheManager.GetCachedData(cacheKey);
            if (videoNodeList == null)
            {
                string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\SerialVideo\\Serial_Video_" + serialId + ".xml");
                XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);
                videoNodeList = xmlDoc.SelectNodes("/NewDataSet/listNews");
            }
            return videoNodeList;
        }

        //public DataTable 

        public int GetSerialVideoNum(int serialId)
        {
            string cacheKey = "Serial_video_num_dic";
            Dictionary<int, int> numDic = (Dictionary<int, int>)CacheManager.GetCachedData(cacheKey);
            if (numDic == null)
            {
                numDic = new Dictionary<int, int>();
                string numFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\newsNum.xml");
                if (File.Exists(numFile))
                {
                    //modified by sk 2013.04.28 统一读取文件方法
                    XmlDocument numDoc = CommonFunction.ReadXmlFromFile(numFile);
                    XmlNodeList serialNodeList = numDoc.SelectNodes("/SerialList/Serial");
                    foreach (XmlElement serialNode in serialNodeList)
                    {
                        int csId = ConvertHelper.GetInteger(serialNode.GetAttribute("id"));
                        int videoNum = ConvertHelper.GetInteger(serialNode.GetAttribute("video"));
                        if (videoNum > 0)
                            numDic[csId] = videoNum;
                    }
                    CacheManager.InsertCache(cacheKey, numDic, 60);
                }
            }
            if (numDic.ContainsKey(serialId))
                return numDic[serialId];
            else
                return 0;
        }

        /// <summary>
        /// 获取子品牌的城市新闻
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="cityId"></param>
        /// <param name="newsType"></param>
        /// <returns></returns>
        public List<News> GetSerialCityNews(int serialId, int cityId, string newsType)
        {
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialCityNews\\CityNews_" + serialId + ".xml");
            List<News> newsList = null;
            List<News> centerNewsList = null;
            Dictionary<int, City> to30CityDic = AutoStorageService.GetCityTo30Dic();            //中心城的对应关系
            int centerCityId = 0;
            if (to30CityDic.ContainsKey(cityId) && to30CityDic[cityId].CityId != cityId)
                centerCityId = to30CityDic[cityId].CityId;
            if (File.Exists(xmlFile))
            {
                XmlReaderSettings xrs = new XmlReaderSettings();
                xrs.CheckCharacters = false;
                using (XmlReader reader = XmlReader.Create(xmlFile, xrs))
                {
                    while (reader.ReadToFollowing("City"))
                    {
                        if (reader.MoveToAttribute("id"))
                        {
                            int cId = Convert.ToInt32(reader.Value);
                            if (cId == cityId)
                            {
                                newsList = ReadDealerCityNews(reader, newsType);
                            }
                            if (cId == centerCityId)
                            {
                                centerNewsList = ReadDealerCityNews(reader, newsType);
                            }

                            //满足以下情况时不再读取
                            if (newsList != null && newsList.Count >= 8)
                                break;
                            if (newsList != null && newsList.Count < 8 && centerNewsList != null)
                                break;
                        }
                    }

                    if (newsList == null)
                        newsList = new List<News>();
                    if (newsList.Count < 8 && centerNewsList != null)
                        newsList.AddRange(centerNewsList);

                }
            }
            return newsList;
        }

        private List<News> ReadDealerCityNews(XmlReader reader, string newsType)
        {
            List<News> newsList = new List<News>();
            if (reader.ReadToFollowing("Kind"))
            {
                if (reader.MoveToAttribute("kind"))
                    if (reader.Value == newsType)
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "City")
                                break;
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Table")
                            {
                                News news = new News();
                                XmlReader inner = reader.ReadSubtree();
                                inner.ReadToDescendant("NewsID");
                                int id = ConvertHelper.GetInteger(inner.ReadInnerXml());
                                news.NewsId = id;
                                while (!inner.EOF)
                                {
                                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "url")
                                        news.PageUrl = inner.ReadInnerXml();
                                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "NewsPubTime")
                                        news.PublishTime = ConvertHelper.GetDateTime(inner.ReadInnerXml());
                                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "NewsTitle")
                                        news.Title = Convert.ToString(inner.ReadInnerXml());
                                    inner.Read();
                                }
                                newsList.Add(news);
                            }
                        }
            }
            return newsList;
        }
        /// <summary>
        /// 根据子品牌和城市ID获取新闻信息
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public List<News> GetHangqingNewsBySerialAndCity(int serialId, string cityName)
        {
            string xmlFile = "SerialHangqing_" + serialId + ".xml";
            string newsPath = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialCityNews\\Hangqing\\" + cityName);
            xmlFile = Path.Combine(newsPath, xmlFile);
            List<News> newsList = new List<News>();
            if (File.Exists(xmlFile))
            {
                try
                {
                    using (XmlReader xmlReader = XmlReader.Create(xmlFile))
                    {
                        while (xmlReader.ReadToFollowing("listNews"))
                        {
                            News news = new News();
                            XmlReader inner = xmlReader.ReadSubtree();
                            inner.ReadToDescendant("newsid");
                            int id = ConvertHelper.GetInteger(inner.ReadInnerXml());
                            news.NewsId = id;
                            while (!inner.EOF)
                            {
                                if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "filepath")
                                    news.PageUrl = Convert.ToString(inner.ReadInnerXml());
                                if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "title")
                                    news.Title = Convert.ToString(inner.ReadInnerXml());
                                if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "publishtime")
                                    news.PublishTime = ConvertHelper.GetDateTime(inner.ReadInnerXml());
                                if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "sourceName")
                                    news.SourceName = Convert.ToString(inner.ReadInnerXml());
                                if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "author")
                                    news.Author = Convert.ToString(inner.ReadInnerXml());
                                if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "CommentNum")
                                    news.CommentNum = ConvertHelper.GetInteger(inner.ReadInnerXml());
                                inner.Read();
                            }
                            newsList.Add(news);
                        }
                    }

                }
                catch { }

            }
            return newsList;
        }

        /// <summary>
        /// 更新城市行情新闻线程
        /// </summary>
        private void UpdateHangqingNewsThread()
        {
            bool isEnter = Monitor.TryEnter(m_updateLock);
            if (!isEnter)
                return;
            string xmlFile = "News_Serial" + m_serialId + "_City" + m_cityId + ".xml";
            string newsPath = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialCityNews\\Hangqing");
            string backupPath = Path.Combine(newsPath, "Backup");
            string backupFile = Path.Combine(backupPath, xmlFile);
            xmlFile = Path.Combine(newsPath, xmlFile);

            //获取新闻
            string xmlUrl = WebConfig.NewsUrl + "?nonewstype=2&brandid=" + m_serialId + "&cityid=" + m_cityId + "&getcount=500";
            XmlDocument newsDoc = new XmlDocument();
            newsDoc.Load(xmlUrl);
            XmlNodeList newsList = newsDoc.SelectNodes("/NewDataSet/listNews");
            foreach (XmlElement tempNode in newsList)
            {
                AppendNewsInfo(tempNode);
            }

            //过滤分类ID
            int[] cateIdList = new int[] { 16, 215, 3 };
            string xmlPath = CommonFunction.GetCategoryXmlPath(cateIdList);
            XmlNodeList cityNewsList = newsDoc.SelectNodes("/NewDataSet/listNews[" + xmlPath + "]");
            List<XmlElement> tempList = new List<XmlElement>();


            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("root");
            root.SetAttribute("time", DateTime.Now.ToString());
            doc.AppendChild(root);
            XmlDeclaration xmlDeclar = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            doc.InsertBefore(xmlDeclar, root);
            foreach (XmlElement tempNode in cityNewsList)
            {
                root.AppendChild(doc.ImportNode(tempNode, true));
                tempList.Add(tempNode);
            }

            //添加新闻评论数
            foreach (XmlElement tempNode in doc.DocumentElement.ChildNodes)
            {
            }
            AutoStorageService.AppendNewsCommentNum(tempList);

            if (!Directory.Exists(backupPath))
                Directory.CreateDirectory(backupPath);
            if (File.Exists(xmlFile))
                File.Copy(xmlFile, backupFile, true);
            doc.Save(xmlFile);
            Monitor.Exit(m_updateLock);
        }

        /// <summary>
        /// 给新闻内容加入根分类ID及分类路径信息
        /// </summary>
        /// <param name="newsNode"></param>
        public void AppendNewsInfo(XmlElement newsNode)
        {
            int cateId = 0;
            try
            {
                Dictionary<int, NewsCategory> newsCategorys = AutoStorageService.GetNewsCategorys();

                cateId = Convert.ToInt32(newsNode.SelectSingleNode("categoryId").InnerText);
                if (newsCategorys.ContainsKey(cateId))
                {
                    //加入根分类ID
                    XmlElement rootIdEle = newsNode.OwnerDocument.CreateElement("RootCategoryId");
                    newsNode.AppendChild(rootIdEle);
                    rootIdEle.InnerText = newsCategorys[cateId].RootCategoryId.ToString();

                    //加入分类路径
                    XmlElement pathEle = newsNode.OwnerDocument.CreateElement("CategoryPath");
                    newsNode.AppendChild(pathEle);
                    pathEle.InnerText = newsCategorys[cateId].CategoryPath;
                }
            }
            catch { }
        }

        ///// <summary>
        ///// 获取UCar二手车信息
        ///// </summary>
        ///// <param name="serialId"></param>
        ///// <returns></returns>
        //public List<UCarInfoEntity> GetUCarInfo(int serialId)
        //{
        //    List<UCarInfoEntity> infoList = new List<UCarInfoEntity>();
        //    string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\UsedCarInfo\\Serial\\Ucar_" + serialId + ".xml");
        //    if (File.Exists(xmlFile))
        //    {
        //        using (XmlReader xmlReader = XmlReader.Create(xmlFile))
        //        {
        //            while (xmlReader.ReadToFollowing("item"))
        //            {
        //                UCarInfoEntity ucarinfo = new UCarInfoEntity();
        //                XmlReader inner = xmlReader.ReadSubtree();
        //                inner.ReadToDescendant("CityName");
        //                ucarinfo.CityName = inner.ReadString();
        //                while (!inner.EOF)
        //                {
        //                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "ProducerName")
        //                    {
        //                        ucarinfo.ProducerName = inner.ReadString();
        //                    }
        //                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "BrandName")
        //                    {
        //                        ucarinfo.BrandName = inner.ReadString();
        //                    }
        //                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "CarName")
        //                    {
        //                        ucarinfo.CarName = inner.ReadString();
        //                    }
        //                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "BuyCarDate")
        //                    {
        //                        ucarinfo.BuyCarDate = inner.ReadString();
        //                    }
        //                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "Color")
        //                    {
        //                        ucarinfo.Color = inner.ReadString();
        //                    }
        //                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "DrivingMileage")
        //                    {
        //                        ucarinfo.DrivingMileage = inner.ReadString();
        //                    }
        //                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "DisplayPrice")
        //                    {
        //                        ucarinfo.DisplayPrice = inner.ReadString();
        //                    }
        //                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "VendorFullName")
        //                    {
        //                        ucarinfo.VendorFullName = inner.ReadString();
        //                    }
        //                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "CarlistUrl")
        //                    {
        //                        ucarinfo.CarlistUrl = inner.ReadString();
        //                    }
        //                    // add by chengl Sep.29.2011
        //                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "VendorUrl")
        //                    {
        //                        ucarinfo.VendorUrl = inner.ReadString();
        //                    }
        //                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "CityUrL")
        //                    {
        //                        ucarinfo.CityUrL = inner.ReadString();
        //                    }
        //                    inner.Read();
        //                }
        //                infoList.Add(ucarinfo);
        //            }
        //        }
        //    }
        //    return infoList;
        //}

        /// <summary>
        /// 根据子品牌ID获取论坛地址
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public string GetForumUrlBySerialId(int serialId)
        {
            string cacheKey = "Serial_Forum_Link_Dictionary";
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\CarBrandToForumUrl.xml");
            string baaUrl = "http://baa.bitauto.com/";
            Dictionary<int, string> urlDic = (Dictionary<int, string>)CacheManager.GetCachedData(cacheKey);
            if (urlDic == null)
            {
                urlDic = new Dictionary<int, string>();
                try
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(xmlFile);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        int sId = ConvertHelper.GetInteger(row["id"]);
                        string url = ConvertHelper.GetString(row["url"]).ToLower();
                        urlDic[sId] = url;
                    }
                }
                catch { }
                CacheManager.InsertCache(cacheKey, urlDic, WebConfig.CachedDuration);
            }
            if (urlDic != null && urlDic.ContainsKey(serialId))
                baaUrl = urlDic[serialId];

            return baaUrl;
        }

        /// <summary>
        /// 获取子品牌的信息卡片
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public EnumCollection.SerialInfoCard GetSerialInfoCard(int serialId)
        {
            return GetSerialInfoCard(serialId, 0);
        }

        /// <summary>
        /// 获取子品牌的信息卡片
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public EnumCollection.SerialInfoCard GetSerialInfoCard(int serialId, int carYear)
        {
            string catchKeyCard = "CsSummaryCsCard_CsID" + serialId + "_year_" + carYear;
            object serialInfoCardByCsID = null;
            CacheManager.GetCachedData(catchKeyCard, out serialInfoCardByCsID);
            EnumCollection.SerialInfoCard sic = new EnumCollection.SerialInfoCard();
            if (serialInfoCardByCsID == null)
            {
                sic = new PageBase().GetSerialInfoCardByCsID(serialId, carYear);
                if (sic.CsSaleState == "停销")
                {
                    // 停销子品牌显示全部颜色
                    sic.ColorList = GetSerialColors(serialId, carYear, true);
                }
                else
                {
                    // modified by chengl May.5.2011
                    // 变更为 年款页取年款下所有车型，子品牌页取在产车型
                    if (carYear > 0)
                    {
                        sic.ColorList = GetSerialColors(serialId, carYear, true);
                    }
                    else
                    {
                        // 子品牌页
                        sic.ColorList = GetSerialColors(serialId, carYear, false);
                    }
                    // old ->非停销子品牌显示非停销车型颜色
                    // sic.ColorList = GetSerialColors(serialId, carYear, false);
                }
                CacheManager.InsertCache(catchKeyCard, sic, 60);
            }
            else
            {
                sic = (EnumCollection.SerialInfoCard)serialInfoCardByCsID;
            }
            return sic;
        }

        /// <summary>
        /// 获取子品牌的信息
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public Car_SerialEntity GetSerialInfoEntity(int serialId)
        {
            string catchKeyEntity = "CsSummaryEntity_CsID" + serialId;
            object serialInfoEntityByCsID = null;
            CacheManager.GetCachedData(catchKeyEntity, out serialInfoEntityByCsID);
            Car_SerialEntity cse = new Car_SerialEntity();
            if (serialInfoEntityByCsID == null)
            {
                cse = (new Car_SerialBll()).Get_Car_SerialByCsID(serialId);
                CacheManager.InsertCache(catchKeyEntity, cse, 60);
            }
            else
            {
                cse = (Car_SerialEntity)serialInfoEntityByCsID;
            }

            return cse;
        }

        /// <summary>
        /// 根据ID获取车展子品牌或主品牌的默认图
        /// </summary>
        /// <param name="id"></param>
        /// <param name="brandType"></param>
        /// <param name="classId">图集分类ID</param>
        /// <returns></returns>
        public string GetCarShowDefaultImage(int id, string brandType, out int classId)
        {
            classId = 0;
            return GetCarShowDefaultImage(id, brandType, 4, out classId);
        }

        /// <summary>
        /// 根据子品牌ID获取车展默认图
        /// </summary>
        /// <param name="id"></param>
        /// <param name="brandType"></param>
        /// <param name="classId">图集分类ID</param>
        /// <returns></returns>
        public string GetCarShowDefaultImage(int id, string brandType, int imgType, out int classId)
        {

            XmlDocument imgDoc = GetCarshowDefaultImageDoc();
            classId = 0;
            string imgUrl = WebConfig.DefaultCarPic;
            if (imgDoc != null)
            {
                string xmlPath = "";
                brandType = brandType.Trim().ToLower();
                if (brandType == "master")
                    xmlPath = "Images/Master[@ID=\"" + id + "\"]";
                else if (brandType == "serial")
                    xmlPath = "Images/Serial[@ID=\"" + id + "\"]";

                XmlElement serialNode = (XmlElement)imgDoc.SelectSingleNode(xmlPath);
                if (serialNode != null)
                {
                    int imgID = ConvertHelper.GetInteger(serialNode.GetAttribute("ImgId"));
                    classId = ConvertHelper.GetInteger(serialNode.GetAttribute("ClassID"));
                    string url = serialNode.GetAttribute("ImgUrl").Trim();
                    if (imgID > 0 && url.Length > 0)
                        imgUrl = new OldPageBase().GetPublishImage(imgType, url, imgID);
                }
            }

            return imgUrl;
        }
        /// <summary>
        /// 通过车主品牌得到车图片的分类ID
        /// </summary>
        /// <param name="id">车主品牌ID</param>
        /// <returns>分类ID</returns>
        public int GetMasterBrandImageClassID(int id)
        {
            int imgClassID = 0;
            XmlDocument imgDoc = GetCarshowDefaultImageDoc();
            if (imgDoc != null)
            {
                string xmlPath = "";
                xmlPath = "Images/Master[@ID=\"" + id + "\"]";

                XmlElement serialNode = (XmlElement)imgDoc.SelectSingleNode(xmlPath);
                if (serialNode != null)
                {
                    imgClassID = ConvertHelper.GetInteger(serialNode.GetAttribute("MasterClassID"));
                }
            }

            return imgClassID;
        }
        /// <summary>
        /// 获取车展主品牌或子品牌的图片数量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="brandType"></param>
        /// <returns></returns>
        public int GetCarshowImageCount(int id, string brandType)
        {
            int imgCount = 0;
            XmlDocument imgDoc = GetCarshowDefaultImageDoc();
            if (imgDoc != null)
            {
                string xmlPath = "";
                brandType = brandType.Trim().ToLower();
                if (brandType == "master")
                    xmlPath = "Images/Master[@ID=\"" + id + "\"]";
                else if (brandType == "serial")
                    xmlPath = "Images/Serial[@ID=\"" + id + "\"]";

                XmlElement serialNode = (XmlElement)imgDoc.SelectSingleNode(xmlPath);
                if (serialNode != null)
                {
                    imgCount = ConvertHelper.GetInteger(serialNode.GetAttribute("ImageCount"));
                }
            }

            return imgCount;
        }

        /// <summary>
        /// 获取子品牌的图片列表
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="classId">图集ID</param>
        /// <returns></returns>
        public List<XmlElement> GetCarshowSerilaImages(int serialId, out int classId, out int imgCount)
        {
            XmlDocument imgDoc = GetCarshowDefaultImageDoc();
            classId = 0;
            imgCount = 0;
            List<XmlElement> imgList = new List<XmlElement>();
            if (imgDoc != null)
            {
                XmlElement sNode = (XmlElement)imgDoc.SelectSingleNode("/Images/Serial[@ID=\"" + serialId + "\"]");
                if (sNode != null)
                {
                    classId = ConvertHelper.GetInteger(sNode.GetAttribute("ClassID"));
                    imgCount = ConvertHelper.GetInteger(sNode.GetAttribute("ImageCount"));
                    XmlNodeList nodeList = sNode.SelectNodes("Image");
                    foreach (XmlElement imgNode in nodeList)
                    {
                        imgList.Add(imgNode);
                    }
                }
            }

            return imgList;
        }
        /// <summary>
        /// 获取车展图片文档
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetCarshowDefaultImageDoc()
        {
            string cacheKey = "car_show_defaultimage";
            XmlDocument imgDoc = (XmlDocument)CacheManager.GetCachedData(cacheKey);
            if (imgDoc == null)
            {
                try
                {
                    string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\CarShow\\DefaultImages.xml");
                    imgDoc = new XmlDocument();
                    imgDoc.Load(xmlFile);
                    CacheManager.InsertCache(cacheKey, imgDoc, 60);
                }
                catch
                {
                    imgDoc = null;
                }
            }
            return imgDoc;
        }

        /// <summary>
        /// 取主品牌车展模特图片
        /// </summary>
        /// <param name="masterId"></param>
        /// <param name="classId">主品牌分类ID</param>
        /// <returns></returns>
        public List<XmlElement> GetCarshowMasterModelImages(int masterId, out int classId, out int imgCount)
        {
            XmlDocument imgDoc = GetCarshowDefaultImageDoc();
            classId = 0;
            imgCount = 0;
            List<XmlElement> imgList = new List<XmlElement>();
            if (imgDoc != null)
            {
                XmlElement mNode = (XmlElement)imgDoc.SelectSingleNode("/Images/Master[@ID=\"" + masterId + "\"]");
                if (mNode != null)
                {
                    classId = ConvertHelper.GetInteger(mNode.GetAttribute("ClassID"));
                    imgCount = ConvertHelper.GetInteger(mNode.GetAttribute("ImageCount"));
                    XmlNodeList nodeList = mNode.SelectNodes("ImageList");
                    foreach (XmlElement imgNode in nodeList)
                    {
                        imgList.Add(imgNode);
                    }
                }
            }

            return imgList;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public XmlDocument GetCarshowSerialDianping(int serialId)
        {
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialDianping\\XML\\Dianping_Serial_" + serialId + ".xml");
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFile);
            }
            catch { }
            return xmlDoc;
        }

        /// <summary>
        /// 直接读取点评的HTML
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public string GetDianpingHtml(int serialId)
        {
            string fileName = Path.Combine(WebConfig.DataBlockPath, String.Format(@"Data\SerialDianping\Html\DianpingHtml_Serial_{0}.html", serialId));
            string dpHtml = String.Empty;
            try
            {
                if (File.Exists(fileName))
                    dpHtml = File.ReadAllText(fileName, Encoding.UTF8);
            }
            catch { }
            return dpHtml;
        }

        /// <summary>
        /// 获取城市的热门新车数据
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public DataSet GetHotNewCar(int cityId, string byNewsType)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("ShowName");
            dt.Columns.Add("AllSpell");
            dt.Columns.Add("Url");
            ds.Tables.Add(dt);
            //指数规则修改 by sk 2013.01.29
            //DateObj dateObj = CommonFunction.GetLastDate(IndexType.UV, DateType.Month);
            //Dictionary<int, List<int>> indexDataDic = UVIndex.GetSerialsCityIndexData(dateObj);
            Dictionary<int, List<int>> indexDataDic = UVIndex.GetSerialCityIndexData(cityId);

            byNewsType = byNewsType.ToLower();

            if (indexDataDic.ContainsKey(cityId))
            {
                //取新车数据
                XmlDocument xmlDoc = AutoStorageService.GetAutoXml();
                XmlNodeList serialNodeList = xmlDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                Dictionary<int, XmlElement> serialNodeDic = new Dictionary<int, XmlElement>();
                foreach (XmlElement serialNode in serialNodeList)
                {
                    int serialId = 0;
                    bool isId = Int32.TryParse(serialNode.GetAttribute("ID"), out serialId);
                    if (!isId)
                        continue;
                    serialNodeDic[serialId] = serialNode;
                }

                // 上市时间排序的子品牌
                List<int> lCsmd = GetAllSerialMarkDayList();
                // 有保养信息的子品牌 按上市时间取前30
                List<int> top30Csmd = new List<int>();
                if (lCsmd != null && lCsmd.Count > 0)
                {
                    foreach (int csid in lCsmd)
                    {
                        if (byNewsType == "maintance" && !IsExitsMaintanceMessage(csid))
                            continue;
                        if (byNewsType == "anquan" && !IsExistedAnquanNews(csid))
                            continue;
                        if (byNewsType == "keji" && !IsExistedKejiNews(csid))
                            continue;

                        if (!top30Csmd.Contains(csid))
                        {
                            top30Csmd.Add(csid);
                            if (top30Csmd.Count >= 35)
                            { break; }
                        }
                    }
                }

                // List<DataRow> otherRowList = new List<DataRow>();   //其他关注度指数高的车

                foreach (int sid in indexDataDic[cityId])
                {
                    if (!serialNodeDic.ContainsKey(sid))
                        continue;

                    if (!top30Csmd.Contains(sid))
                    { continue; }

                    DataRow row = dt.NewRow();
                    row["Id"] = sid;
                    row["Name"] = serialNodeDic[sid].GetAttribute("Name");
                    row["ShowName"] = serialNodeDic[sid].GetAttribute("ShowName");
                    string allSpell = serialNodeDic[sid].GetAttribute("AllSpell").Trim().ToLower();
                    row["AllSpell"] = allSpell;

                    //各类型文章Url
                    switch (byNewsType)
                    {
                        case "maintance":
                            row["Url"] = "http://car.bitauto.com/" + allSpell + "/baoyang/";
                            break;
                        case "keji":
                            row["Url"] = "http://car.bitauto.com/tree_keji/sb_" + sid + "/";
                            break;
                        case "anquan":
                            row["Url"] = "http://car.bitauto.com/tree_anquan/sb_" + sid + "/";
                            break;
                        default:
                            row["Url"] = "http://car.bitauto.com/" + allSpell + "/";
                            break;
                    }

                    dt.Rows.Add(row);
                    if (dt.Rows.Count >= 30)
                        break;
                }
                ds.AcceptChanges();
            }
            return ds;
        }

        /// <summary>
        /// 获取十佳车型的字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetBestCarTop10()
        {
            string cacheKey = "Get_bestcar_top10";
            Dictionary<int, string> bestCarDic = (Dictionary<int, string>)CacheManager.GetCachedData(cacheKey);
            if (bestCarDic == null)
            {
                string dataXmlFile = HttpContext.Current.Server.MapPath("~/App_Data/BestCarTop10.xml");
                bestCarDic = new Dictionary<int, string>();
                if (File.Exists(dataXmlFile))
                {
                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(dataXmlFile);
                        XmlNodeList serialNodeList = xmlDoc.SelectNodes("/SerialList/Serial");
                        foreach (XmlElement serialNode in serialNodeList)
                        {
                            int serialId = ConvertHelper.GetInteger(serialNode.GetAttribute("id"));
                            string serialName = serialNode.GetAttribute("name").Trim();
                            if (serialId > 0 && serialName.Length > 0)
                                bestCarDic[serialId] = serialName;
                        }
                    }
                    catch { }
                    CacheManager.InsertCache(cacheKey, bestCarDic, WebConfig.CachedDuration);
                }
            }
            return bestCarDic;
        }

        /// <summary>
        /// 根据年份 获取十佳车型的字典
        /// </summary>
        /// <param name="year">2012年开始</param>
        /// <returns></returns>
        public static Dictionary<int, string> GetBestCarTop10ByYear(int year)
        {
            string cacheKey = "Get_bestcar_top10_" + year.ToString();
            Dictionary<int, string> bestCarDic = (Dictionary<int, string>)CacheManager.GetCachedData(cacheKey);
            if (bestCarDic == null)
            {
                string dataXmlFile = HttpContext.Current.Server.MapPath("~/App_Data/BestCarTop10_" + year.ToString() + ".xml");
                bestCarDic = new Dictionary<int, string>();
                if (File.Exists(dataXmlFile))
                {
                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(dataXmlFile);
                        XmlNodeList serialNodeList = xmlDoc.SelectNodes("/SerialList/Serial");
                        foreach (XmlElement serialNode in serialNodeList)
                        {
                            int serialId = ConvertHelper.GetInteger(serialNode.GetAttribute("id"));
                            string serialName = serialNode.GetAttribute("name").Trim();
                            if (serialId > 0 && serialName.Length > 0)
                                bestCarDic[serialId] = serialName;
                        }
                    }
                    catch { }
                    CacheManager.InsertCache(cacheKey, bestCarDic, WebConfig.CachedDuration);
                }
            }
            return bestCarDic;
        }

        /// <summary>
        /// 年度10佳车配置
        /// </summary>
        /// <returns></returns>
        public static List<BestTopCar> GetAllBestTopCar()
        {
            List<BestTopCar> listAllBestTopCar = new List<BestTopCar>();
            string cacheKey = "GetAllBestTopCar_New";
            listAllBestTopCar = (List<BestTopCar>)CacheManager.GetCachedData(cacheKey);
            if (listAllBestTopCar == null)
            {
                string dataXmlFile = HttpContext.Current.Server.MapPath("~/App_Data/TopCarAD.xml");
                if (File.Exists(dataXmlFile))
                {
                    try
                    {
                        listAllBestTopCar = new List<BestTopCar>();
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(dataXmlFile);
                        XmlNodeList adNodeList = xmlDoc.SelectNodes("/Root/TopCar");
                        foreach (XmlElement adNode in adNodeList)
                        {
                            BestTopCar btc = new BestTopCar();
                            btc.Title = "";
                            btc.Link = "";
                            btc.ListCsList = new List<int>();
                            btc.Title = adNode.GetAttribute("Title").ToString().Trim();
                            btc.Link = adNode.GetAttribute("Link").ToString().Trim();
                            XmlNodeList csNodeList = adNode.SelectNodes("Serial");
                            foreach (XmlElement csNode in csNodeList)
                            {
                                int csid = ConvertHelper.GetInteger(csNode.GetAttribute("id"));
                                if (!btc.ListCsList.Contains(csid))
                                { btc.ListCsList.Add(csid); }
                            }
                            listAllBestTopCar.Add(btc);
                        }
                    }
                    catch (Exception ex)
                    { CommonFunction.WriteLog(ex.ToString()); }
                    CacheManager.InsertCache(cacheKey, listAllBestTopCar, WebConfig.CachedDuration);
                }
            }
            return listAllBestTopCar;
        }

        /// <summary>
        /// 获取城市的热门新车数据
        /// </summary>
        /// <param name="level">级别</param>
        /// <param name="range">报价区间</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="byNewsType">限定的文章类型，即有该类型的文章才取此子品牌</param>
        /// <returns></returns>
        public DataSet GetHotNewCarByPriceRange(int level, int range, int cityId, string byNewsType)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("ShowName");
            dt.Columns.Add("AllSpell");
            dt.Columns.Add("Url");
            ds.Tables.Add(dt);
            //指数规则修改 by sk 2013.01.29
            //DateObj dateObj = CommonFunction.GetLastDate(IndexType.UV, DateType.Month);
            //Dictionary<int, List<int>> indexDataDic = UVIndex.GetSerialsCityIndexData(dateObj);
            Dictionary<int, List<int>> indexDataDic = UVIndex.GetSerialCityIndexData(cityId);

            // 级别
            string levelName = "";
            if (level > 0)
            {
                //EnumCollection.SerialLevelEnum levelEnum = (EnumCollection.SerialLevelEnum)level;
                //levelName = levelEnum.ToString();
                levelName = CarLevelDefine.GetLevelNameById(level);
            }

            if (indexDataDic.ContainsKey(cityId))
            {
                //取新车数据
                XmlDocument xmlDoc = AutoStorageService.GetAutoXml();
                XmlNodeList serialNodeList = xmlDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                Dictionary<int, XmlElement> serialNodeDic = new Dictionary<int, XmlElement>();
                foreach (XmlElement serialNode in serialNodeList)
                {
                    int serialId = 0;
                    bool isId = Int32.TryParse(serialNode.GetAttribute("ID"), out serialId);
                    if (!isId)
                        continue;

                    if (level > 0)
                    {
                        // 取级别
                        if (levelName != serialNode.Attributes["CsLevel"].Value.ToString())
                        { continue; }
                    }
                    if (range > 0)
                    {
                        // 40万以上区间合并
                        if (range == 7 || range == 8)
                        {
                            // 40万以上区间合并
                            if (serialNode.Attributes["MultiPriceRange"].Value.ToString().IndexOf(",7,") < 0 && serialNode.Attributes["MultiPriceRange"].Value.ToString().IndexOf(",8,") < 0)
                            { continue; }
                        }
                        else
                        {
                            // 是否当前价格区间
                            if (serialNode.Attributes["MultiPriceRange"].Value.ToString().IndexOf("," + range + ",") < 0)
                            { continue; }

                            // 当报价区间时不包含SUV
                            if (serialNode.Attributes["CsLevel"].Value.ToString() == "SUV")
                            { continue; }
                        }
                    }
                    serialNodeDic[serialId] = serialNode;
                }

                List<DataRow> otherRowList = new List<DataRow>();   //其他关注度指数高的车

                foreach (int sid in indexDataDic[cityId])
                {
                    if (!serialNodeDic.ContainsKey(sid))
                        continue;
                    if (byNewsType == "maintance" && !IsExitsMaintanceMessage(sid))
                        continue;
                    if (byNewsType == "anquan" && !IsExistedAnquanNews(sid))
                        continue;
                    if (byNewsType == "keji" && !IsExistedKejiNews(sid))
                        continue;

                    DataRow row = dt.NewRow();
                    row["Id"] = sid;
                    row["Name"] = serialNodeDic[sid].GetAttribute("Name");
                    row["ShowName"] = serialNodeDic[sid].GetAttribute("ShowName");
                    string allSpell = serialNodeDic[sid].GetAttribute("AllSpell").Trim().ToLower();
                    row["AllSpell"] = allSpell;

                    //各类型文章Url
                    switch (byNewsType)
                    {
                        case "maintance":
                            row["Url"] = "http://car.bitauto.com/" + allSpell + "/baoyang/";
                            break;
                        case "keji":
                            row["Url"] = "http://car.bitauto.com/tree_keji/sb_" + sid + "/";
                            break;
                        case "anquan":
                            row["Url"] = "http://car.bitauto.com/tree_anquan/sb_" + sid + "/";
                            break;
                        default:
                            row["Url"] = "http://car.bitauto.com/" + allSpell + "/";
                            break;
                    }

                    dt.Rows.Add(row);
                    if (dt.Rows.Count >= 30)
                        break;
                }

                ////补齐30条数据 (保养不补齐)
                //if (dt.Rows.Count < 30 && !onlyMaintance)
                //{
                //    foreach (DataRow sRow in otherRowList)
                //    {
                //        dt.Rows.Add(sRow);
                //        if (dt.Rows.Count >= 30)
                //            break;
                //    }
                //}
                ds.AcceptChanges();
            }
            return ds;
        }

        /// <summary>
        /// 获取子品牌的概况信息
        /// </summary>
        /// <param name="serialIdList"></param>
        /// <returns></returns>
        public DataSet GetSerialOverview(int[] serialIdList)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("ShowName");
            dt.Columns.Add("Price");
            dt.Columns.Add("EngineExhaust");
            dt.Columns.Add("OfficialFuelCost");
            dt.Columns.Add("GuestFuelCost");
            dt.Columns.Add("TransmissionType");
            dt.Columns.Add("StartSellNews");
            dt.Columns.Add("SallDataChart");
            dt.Columns.Add("MaintainNews");
            dt.Columns.Add("ScienceAndTechnologyNews");
            dt.Columns.Add("SafeNews");
            dt.Columns.Add("ImageCount");
            dt.Columns.Add("PublicPraise");
            dt.Columns.Add("AskCount");
            dt.Columns.Add("TestForBuy");
            dt.Columns.Add("BitautoTest");
            dt.Columns.Add("Merit");
            dt.Columns.Add("Demerit");

            ds.Tables.Add(dt);

            foreach (int serialId in serialIdList)
            {
                try
                {
                    DataRow row = dt.NewRow();
                    EnumCollection.SerialInfoCard sic = GetSerialInfoCard(serialId);
                    Car_SerialEntity cse = GetSerialInfoEntity(serialId);
                    row["Id"] = sic.CsID;
                    row["Name"] = sic.CsName;
                    row["ShowName"] = sic.CsShowName;
                    row["Price"] = sic.CsPriceRange.Replace("万-", "-");
                    row["EngineExhaust"] = sic.CsEngineExhaust;
                    row["OfficialFuelCost"] = sic.CsOfficialFuelCost;
                    row["GuestFuelCost"] = sic.CsGuestFuelCost;
                    row["TransmissionType"] = sic.CsTransmissionType;
                    row["StartSellNews"] = sic.CsNewShangShi;
                    row["SallDataChart"] = sic.CsNewXiaoShouShuJu;
                    row["MaintainNews"] = sic.CsNewWeiXiuBaoYang;
                    row["ScienceAndTechnologyNews"] = sic.CsNewKeJi;
                    row["SafeNews"] = sic.CsNewAnQuan;
                    row["ImageCount"] = sic.CsPicCount;
                    row["PublicPraise"] = sic.CsDianPingCount;
                    row["AskCount"] = sic.CsAskCount;
                    row["TestForBuy"] = sic.CsNewMaiCheCheShi;
                    row["BitautoTest"] = sic.CsNewYiCheCheShi;
                    row["Merit"] = cse.Cs_Virtues;
                    row["Demerit"] = cse.Cs_Defect;
                    dt.Rows.Add(row);
                }
                catch { }
            }

            ds.AcceptChanges();
            return ds;

        }

        /// <summary>
        /// 获取主页的热门车型代码
        /// </summary>
        /// <returns></returns>
        public string GetHomepageHotSerial()
        {
            string cacheKey = "homepage_hotserial";
            string htmlStr = (string)CacheManager.GetCachedData(cacheKey);
            if (htmlStr == null)
            {
                //获取数据xml
                XmlDocument mbDoc = AutoStorageService.GetAutoXml();
                XmlNodeList serialList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                List<XmlElement> serialNodeList = new List<XmlElement>();
                TopPVSerialSelector tpvSelector = new TopPVSerialSelector(10);
                foreach (XmlElement serialNode in serialList)
                {
                    tpvSelector.AddSerial(serialNode);
                }
                htmlStr = GetHomepageHotSerial(tpvSelector.GetTopSerialList());
                CacheManager.InsertCache(cacheKey, htmlStr, 60);
            }
            return htmlStr;
        }
        /// <summary>
        /// 获取热门车型
        /// </summary>
        /// <param name="num">获取数量</param>
        /// <returns></returns>
        public List<XmlElement> GetHotSerial(int num)
        {
            string cacheKey = "cooperation_hotserial" + num;
            List<XmlElement> list = (List<XmlElement>)CacheManager.GetCachedData(cacheKey);
            if (list == null)
            {
                //获取数据xml
                XmlDocument mbDoc = AutoStorageService.GetAutoXml();
                XmlNodeList serialList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                List<XmlElement> serialNodeList = new List<XmlElement>();
                TopPVSerialSelector tpvSelector = new TopPVSerialSelector(num);
                foreach (XmlElement serialNode in serialList)
                {
                    tpvSelector.AddSerial(serialNode);
                }
                list = tpvSelector.GetTopSerialList();
                CacheManager.InsertCache(cacheKey, list, 60);
            }
            return list;
        }

        /// <summary>
        /// 获取主页的热门车型代码
        /// </summary>
        /// <returns></returns>
        public string GetHomepageHotSerial(int num)
        {
            string cacheKey = "homepage_hotserial + top" + num;
            string htmlStr = (string)CacheManager.GetCachedData(cacheKey);
            List<String> htmlList = new List<string>();
            if (htmlStr == null)
            {
                //获取数据xml
                XmlDocument mbDoc = AutoStorageService.GetAutoXml();
                XmlNodeList serialList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                List<XmlElement> serialNodeList = new List<XmlElement>();
                TopPVSerialSelector tpvSelector = new TopPVSerialSelector(num);
                List<SerialListADEntity> listSerialAD = this.GetSerialAD("indexcarhot"); //子品牌广告
                foreach (XmlElement serialNode in serialList)
                {
                    if (listSerialAD != null) //广告排重
                    {
                        int serialId = ConvertHelper.GetInteger(serialNode.Attributes["ID"].Value);
                        SerialListADEntity serialAdEntity = listSerialAD.Find((p) => { return p.SerialId == serialId; });
                        if (serialAdEntity != null)
                        {
                            continue;
                        }
                    }
                    tpvSelector.AddSerial(serialNode);
                }
                List<XmlElement> listTopSerial = tpvSelector.GetTopSerialList();

                //插入子品牌广告
                int removeNum = 0;
                // modified by chengl Mar.11.2013 when listSerialAD is null bug here
                if (listSerialAD != null && listSerialAD.Count > 0)
                {
                    foreach (SerialListADEntity serialAd in listSerialAD)
                    {
                        int index = serialAd.Pos - 1;
                        if (index < 0)
                            index = 0;
                        XmlElement nodeSerial = (XmlElement)mbDoc.SelectSingleNode("//Serial[@ID='" + serialAd.SerialId + "']");
                        if (nodeSerial != null && listTopSerial.Count >= index)
                        {
                            removeNum++;
                            listTopSerial.Insert(index, nodeSerial);
                        }
                    }
                }
                if (removeNum > 0)
                    listTopSerial.RemoveRange(listTopSerial.Count - removeNum, removeNum);

                htmlStr = GetHomepageHotSerial(listTopSerial);
                CacheManager.InsertCache(cacheKey, htmlStr, 60);
            }
            return htmlStr;
        }

        /// <summary>
        /// 获取主页的热门车型代码
        /// </summary>
        /// <returns></returns>
        public string GetHomepageHotSerialV2(int num)
        {
            string cacheKey = "homepage_hotserialv2 + top" + num;
            string htmlStr = (string)CacheManager.GetCachedData(cacheKey);
            List<String> htmlList = new List<string>();
            if (htmlStr == null)
            {
                //获取数据xml
                XmlDocument mbDoc = AutoStorageService.GetAutoXml();
                XmlNodeList serialList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                List<XmlElement> serialNodeList = new List<XmlElement>();
                TopPVSerialSelector tpvSelector = new TopPVSerialSelector(num);
                List<SerialListADEntity> listSerialAD = this.GetSerialAD("indexcarhot"); //子品牌广告
                foreach (XmlElement serialNode in serialList)
                {
                    if (listSerialAD != null) //广告排重
                    {
                        int serialId = ConvertHelper.GetInteger(serialNode.Attributes["ID"].Value);
                        SerialListADEntity serialAdEntity = listSerialAD.Find((p) => { return p.SerialId == serialId; });
                        if (serialAdEntity != null)
                        {
                            continue;
                        }
                    }
                    tpvSelector.AddSerial(serialNode);
                }
                List<XmlElement> listTopSerial = tpvSelector.GetTopSerialList();

                //插入子品牌广告
                int removeNum = 0;
                // modified by chengl Mar.11.2013 when listSerialAD is null bug here
                if (listSerialAD != null && listSerialAD.Count > 0)
                {
                    foreach (SerialListADEntity serialAd in listSerialAD)
                    {
                        int index = serialAd.Pos - 1;
                        if (index < 0)
                            index = 0;
                        XmlElement nodeSerial = (XmlElement)mbDoc.SelectSingleNode("//Serial[@ID='" + serialAd.SerialId + "']");
                        if (nodeSerial != null && listTopSerial.Count >= index)
                        {
                            removeNum++;
                            listTopSerial.Insert(index, nodeSerial);
                        }
                    }
                }
                if (removeNum > 0)
                    listTopSerial.RemoveRange(listTopSerial.Count - removeNum, removeNum);

                htmlStr = GetHomepageHotSerialV2(listTopSerial);
                CacheManager.InsertCache(cacheKey, htmlStr, 60);
            }
            return htmlStr;
        }

        #region 获取子品牌广告
        /// <summary>
        /// 获取页面子品牌广告位
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SerialListADEntity> GetSerialAD(string name)
        {
            Dictionary<string, List<SerialListADEntity>> dictSerialAD = this.GetSerialListADData();
            if (dictSerialAD != null && dictSerialAD.ContainsKey(name))
            {
                List<SerialListADEntity> list = dictSerialAD[name];
                list.Sort((x, y) => x.Pos < y.Pos ? -1 : 0);
                return list;
            }
            return null;
        }
        /// <summary>
        /// 获取子品牌广告位数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<SerialListADEntity>> GetSerialListADData()
        {
            string cachekey = "Car_SerialListAD";
            Dictionary<string, List<SerialListADEntity>> serialAD = (Dictionary<string, List<SerialListADEntity>>)CacheManager.GetCachedData(cachekey);
            if (serialAD == null)
            {
                string path = HttpContext.Current.Server.MapPath("~/App_Data/ad/SerialListAD.xml");
                if (File.Exists(path))
                {
                    XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(path);
                    serialAD = new Dictionary<string, List<SerialListADEntity>>();
                    XmlNodeList nodeList = xmlDoc.SelectNodes("//SerialAD");
                    foreach (XmlNode node in nodeList)
                    {
                        List<SerialListADEntity> list = new List<SerialListADEntity>();
                        foreach (XmlNode serialNode in node.SelectNodes("./item"))
                        {
                            DateTime startTime = ConvertHelper.GetDateTime(serialNode.Attributes["starttime"].Value);
                            DateTime endTime = ConvertHelper.GetDateTime(serialNode.Attributes["endtime"].Value);
                            if (DateTime.Compare(startTime, DateTime.Now) < 0
                                && CommonFunction.DateDiff("d", startTime, DateTime.Now) >= 0
                                && CommonFunction.DateDiff("d", endTime, DateTime.Now) <= 0)
                            {
                                string url = string.Empty;
                                string imageUrl = string.Empty;
                                if (serialNode.Attributes["url"] != null)
                                    url = serialNode.Attributes["url"].Value;
                                if (serialNode.Attributes["imageurl"] != null)
                                    imageUrl = serialNode.Attributes["imageurl"].Value;
                                list.Add(new SerialListADEntity()
                                {
                                    Pos = ConvertHelper.GetInteger(serialNode.Attributes["pos"].Value),
                                    SerialId = ConvertHelper.GetInteger(serialNode.Attributes["serialId"].Value),
                                    Url = url,
                                    ImageUrl = imageUrl,
                                    StartTime = startTime,
                                    EndTime = endTime
                                });
                            }
                        }
                        serialAD.Add(node.Attributes["name"].Value, list);
                    }
                    CacheManager.InsertCache(cachekey, serialAD, new CacheDependency(path), DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            return serialAD;
        }
        /// <summary>
        /// SUV频道 热门suv广告位配置
        /// </summary>
        /// <returns></returns>
        public List<SerialListADEntity> GetSuvChannelAdData()
        {
            string cachekey = "Car_GetSuvChannelAdData";
            List<SerialListADEntity> serialList = (List<SerialListADEntity>)CacheManager.GetCachedData(cachekey);
            if (serialList == null)
            {
                string path = HttpContext.Current.Server.MapPath("~/App_Data/ad/SUVChannelAdConfig.xml");
                if (File.Exists(path))
                {
                    XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(path);
                    serialList = new List<SerialListADEntity>();
                    XmlNodeList nodeList = xmlDoc.SelectNodes("//Item");
                    foreach (XmlNode node in nodeList)
                    {
                        DateTime startTime = ConvertHelper.GetDateTime(node.Attributes["starttime"].Value);
                        DateTime endTime = ConvertHelper.GetDateTime(node.Attributes["endtime"].Value);
                        if (DateTime.Compare(startTime, DateTime.Now) < 0
                            && CommonFunction.DateDiff("d", startTime, DateTime.Now) >= 0
                            && CommonFunction.DateDiff("d", endTime, DateTime.Now) <= 0)
                        {
                            serialList.Add(new SerialListADEntity()
                            {
                                Pos = ConvertHelper.GetInteger(node.Attributes["pos"].Value),
                                SerialId = ConvertHelper.GetInteger(node.Attributes["serialId"].Value),
                                StartTime = startTime,
                                EndTime = endTime
                            });
                        }
                    }
                    if (serialList.Any())
                        CacheManager.InsertCache(cachekey, serialList, new CacheDependency(path), DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            return serialList;
        }

        /// <summary>
        /// 取子品牌地址广告
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<LinkADForCs>> GetLinkAD()
        {
            string cachekey = "Car_SerialBll_GetLinkAD_New";
            Dictionary<string, List<LinkADForCs>> dic = (Dictionary<string, List<LinkADForCs>>)CacheManager.GetCachedData(cachekey);
            if (dic == null)
            {
                string path = HttpContext.Current.Server.MapPath("~/App_Data/ad/LinkADByCsID.xml");
                if (File.Exists(path))
                {
                    XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(path);
                    dic = new Dictionary<string, List<LinkADForCs>>();
                    XmlNodeList nodeList = xmlDoc.SelectNodes("//Root/ADItem");
                    foreach (XmlNode node in nodeList)
                    {
                        string key = (node.SelectNodes("Key") != null && node.SelectNodes("Key").Count > 0)
                            ? node.SelectNodes("Key")[0].InnerText.ToString().Trim() : "";
                        string title = (node.SelectNodes("Title") != null && node.SelectNodes("Title").Count > 0)
                            ? node.SelectNodes("Title")[0].InnerText.ToString().Trim() : "";
                        //string link = (node.SelectNodes("Link") != null && node.SelectNodes("Link").Count > 0)
                        //    ? node.SelectNodes("Link")[0].InnerText.ToString().Trim() : "";
                        if (key != "" && !dic.ContainsKey(key))
                        {
                            List<LinkADForCs> list = new List<LinkADForCs>();
                            foreach (XmlNode serialID in node.SelectNodes("CsIDList"))
                            {
                                string link = (serialID.Attributes["Link"] != null && serialID.Attributes["Link"].Value != "") ?
                                    serialID.Attributes["Link"].Value.ToString() : "";
                                if (link != "")
                                {
                                    LinkADForCs lad = new LinkADForCs();
                                    lad.Key = key;
                                    lad.Title = title;
                                    lad.Link = link;
                                    lad.ListCsID = new List<int>();
                                    foreach (XmlNode csidXN in serialID.SelectNodes("ID"))
                                    {
                                        int csid = 0;
                                        if (int.TryParse(csidXN.InnerText, out csid))
                                        {
                                            if (csid > 0 && !lad.ListCsID.Contains(csid))
                                            { lad.ListCsID.Add(csid); }
                                        }
                                    }
                                    list.Add(lad);
                                }
                            }
                            dic.Add(key, list);
                        }
                    }
                    CacheManager.InsertCache(cachekey, dic, new CacheDependency(path), DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            return dic;
        }

        #endregion

        /// <summary>
        /// 得到导购首页的热门车型
        /// </summary>
        /// <returns></returns>
        public List<XmlElement> GetDaogouTreeHomepageHotSerial(string type)
        {
            string cacheKey = string.Format("daogoutreehomepage_hotserial_{0}", type);
            List<XmlElement> serialList = (List<XmlElement>)CacheManager.GetCachedData(cacheKey);
            TreeData treeData = new TreeFactory().GetTreeDataObject(type);
            if (serialList == null || serialList.Count < 1)
            {
                XmlDocument mbDoc = AutoStorageService.GetAutoXml();
                XmlNodeList tempSerialList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                List<XmlElement> serialNodeList = new List<XmlElement>();
                TopPVSerialSelector tpvSelector = new TopPVSerialSelector(20);
                foreach (XmlElement serialNode in tempSerialList)
                {
                    int serialCount = treeData.GetSerialId(ConvertHelper.GetInteger(serialNode.Attributes["ID"].Value.ToString()));
                    if (serialCount < 1) continue;
                    serialNode.SetAttribute(type, serialCount.ToString());

                    tpvSelector.AddSerial(serialNode);
                }
                serialList = tpvSelector.GetTopSerialList();
            }
            //添加缓存
            if (serialList != null && serialList.Count > 0)
            {
                CacheManager.InsertCache(cacheKey, serialList, 60);
            }
            return serialList;
        }
        /// <summary>
        /// 得到行情首页的热门车型,添加城市维度
        /// </summary>
        /// <returns></returns>
        public List<XmlElement> GetDaogouTreeHomepageHotSerial(string type, int cityId)
        {
            //如果城市为全国
            if (cityId < 1)
                return GetDaogouTreeHomepageHotSerial(type);
            //如果城市已经被选中
            string cacheKey = string.Format("daogoutreehomepage_hotserial_{0}_city_{1}", type, cityId);
            List<XmlElement> serialList = (List<XmlElement>)CacheManager.GetCachedData(cacheKey);
            TreeData treeData = new TreeFactory().GetTreeDataObject(type);
            if (serialList == null || serialList.Count < 1)
            {
                XmlDocument mbDoc = AutoStorageService.GetAutoXml();
                XmlNodeList tempSerialList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                List<XmlElement> serialNodeList = new List<XmlElement>();
                TopPVSerialSelector tpvSelector = new TopPVSerialSelector(20);
                foreach (XmlElement serialNode in tempSerialList)
                {
                    int serialCount = 0;
                    int serialId = ConvertHelper.GetInteger(serialNode.GetAttribute("ID"));
                    Dictionary<int, int> cityList = NewsChannelBll.GetTreeHangQingSerialCityNumber(serialId);
                    if (cityList != null && cityList.Count > 0 && cityList.ContainsKey(cityId))
                    {
                        serialCount = cityList[cityId];
                    }
                    if (serialCount < 1) continue;
                    serialNode.SetAttribute(type, serialCount.ToString());

                    tpvSelector.AddSerial(serialNode);
                }
                serialList = tpvSelector.GetTopSerialList();
            }
            //添加缓存
            if (serialList != null && serialList.Count > 0)
            {
                CacheManager.InsertCache(cacheKey, serialList, 60);
            }
            return serialList;
        }

        /// <summary>
        /// 获取分段报价的热门车型列表
        /// </summary>
        /// <param name="serialList"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetHomepageHotSerial(XmlNodeList serialList, string price)
        {
            string cacheKey = "homepage_hotserial_price_" + price;
            Dictionary<string, string> htmlDic = (Dictionary<string, string>)CacheManager.GetCachedData(cacheKey);
            if (htmlDic == null)
            {
                TopPVSerialSelector tpvSelector = new TopPVSerialSelector(10);
                tpvSelector.SelectNewCar = true;
                foreach (XmlElement serialNode in serialList)
                {
                    tpvSelector.AddSerial(serialNode);
                }

                string hotSerilaHtml = GetHomepageHotSerial(tpvSelector.GetTopSerialList());
                string hotNewCarHtml = GetPriceHotNewCarHtml(tpvSelector.GetNewCarList());

                htmlDic = new Dictionary<string, string>();
                htmlDic["hotSerial"] = hotSerilaHtml;
                htmlDic["hotNewCar"] = hotNewCarHtml;

                CacheManager.InsertCache(cacheKey, htmlDic, 60);
            }
            return htmlDic;
        }

        /// <summary>
        /// 获取热门车型的代码
        /// </summary>
        /// <param name="serialList"></param>
        /// <returns></returns>
        private string GetHomepageHotSerial(List<XmlElement> serialList)
        {
            //生成代码
            List<string> htmlList = new List<string>();
            int counter = 0;
            foreach (XmlElement serialNode in serialList)
            {
                counter++;
                htmlList.Add("<li>");
                //if (counter <= 3)
                //    htmlList.Add("<em class=\"top3\">");
                //else
                //    htmlList.Add("<em>");
                //htmlList.Add(counter.ToString() + "</em>");

                int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));

                string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");

                string serialName = "";
                serialName = serialNode.GetAttribute("ShowName");

                string serialLevel = serialNode.GetAttribute("CsLevel");
                string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();
                //EnumCollection.SerialLevelEnum levelEnum = (EnumCollection.SerialLevelEnum)Enum.Parse(typeof(EnumCollection.SerialLevelEnum), serialLevel);
                string serialUrl = "/" + serialSpell + "/";
                //string levelUrl = "/" + ((EnumCollection.SerialLevelSpellEnum)levelEnum).ToString() + "/";
                string levelUrl = string.Format("/{0}/", CarLevelDefine.GetLevelSpellByName(serialLevel));

                htmlList.Add("<a id=\"hotCsID_" + serialId + "\" href=\"" + serialUrl + "\" target=\"_blank\">");
                htmlList.Add("<img src=\"" + imgUrl + "\" alt=\"" + serialName + "\" /></a>");
                htmlList.Add("<div class=\"title\"><a href=\"" + serialUrl + "\" target=\"_blank\">" + serialName + "</a></div>");

                //if (StringHelper.GetRealLength(serialName + "[" + serialLevel + "]") < 20)
                //    htmlList.Add("<a href=\"" + levelUrl + "\" target=\"_blank\" class=\"classify\">[" + serialLevel + "]</a>");

                string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(serialId));
                if (priceRange.Trim().Length == 0)
                    htmlList.Add("<div class=\"txt\">暂无报价</div>");
                else
                    htmlList.Add("<div class=\"txt\">" + priceRange + "</div>");
                htmlList.Add("</li>");
            }
            return String.Concat(htmlList.ToArray());
        }

        /// <summary>
        /// 获取热门车型的代码
        /// </summary>
        /// <param name="serialList"></param>
        /// <returns></returns>
        private string GetHomepageHotSerialV2(List<XmlElement> serialList)
        {
            //生成代码
            List<string> htmlList = new List<string>();
            foreach (XmlElement serialNode in serialList)
            {
                int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));
                string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_3.");
                string serialName = "";
                serialName = serialNode.GetAttribute("ShowName");
                string serialLevel = serialNode.GetAttribute("CsLevel");
                string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();
                string serialUrl = "/" + serialSpell + "/";
                string levelUrl = string.Format("/{0}/", CarLevelDefine.GetLevelSpellByName(serialLevel));

                string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(serialId));

                htmlList.Add("<div class=\"col-xs-3\">");
                htmlList.Add("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
                htmlList.Add("<div class=\"img\">");
                htmlList.Add("<a href=\"" + serialUrl + "\"  target=\"_blank\"><img src=\"" + imgUrl + "\" alt=\"" + serialName + "\"></a>");
                htmlList.Add("</div>");
                htmlList.Add("<ul class=\"p-list\">");
                htmlList.Add("<li class=\"name\"><a href=\"" + serialUrl + "\" target=\"_blank\">" + serialName + "</a></li>");
                if (priceRange.Trim().Length == 0)
                {
                    htmlList.Add("<li class=\"price\"><a href=\"" + serialUrl + "\" target=\"_blank\">暂无报价</a></li>");
                }
                else
                {
                    htmlList.Add("<li class=\"price\"><a href=\"" + serialUrl + "\" target=\"_blank\">" + priceRange + "</a></li>");
                }
                htmlList.Add("</ul>");
                htmlList.Add("</div>");
                htmlList.Add("</div>");
            }
            return String.Concat(htmlList.ToArray());
        }


        /// <summary>
        /// 获取分段报价页的热门新车
        /// </summary>
        /// <param name="serialList"></param>
        /// <returns></returns>
        private string GetPriceHotNewCarHtml(List<XmlElement> serialList)
        {
            //生成代码
            StringBuilder htmlCode = new StringBuilder();
            //图片Url
            // Dictionary<int, XmlElement> urlDic = AutoStorageService.GetImageUrlDic();
            Dictionary<int, XmlElement> urlDic = CarSerialImgUrlService.GetImageUrlDicNew();

            foreach (XmlElement serialNode in serialList)
            {
                int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));

                string imgUrl = "";
                if (urlDic.ContainsKey(serialId))
                {
                    // modified by chengl Jan.4.2010
                    if (urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim() != "")
                    {
                        // 有新封面
                        imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim(), "5");
                    }
                    else
                    {
                        // 没有新封面
                        if (urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim() != "")
                        {
                            imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim(), "5");
                        }
                        else
                        {
                            imgUrl = WebConfig.DefaultCarPic;
                        }
                    }
                    //int imgId = Convert.ToInt32(urlDic[serialId].GetAttribute("ImageId"));
                    //imgUrl = urlDic[serialId].GetAttribute("ImageUrl");
                    //if (imgId == 0 || imgUrl == "")
                    //    imgUrl = WebConfig.DefaultCarPic;
                    //else
                    //    imgUrl = new OldPageBase().GetPublishImage(5, imgUrl, imgId);
                }
                else
                    imgUrl = WebConfig.DefaultCarPic;

                string serialName = serialNode.GetAttribute("Name");
                string serialShowName = serialNode.GetAttribute("ShowName");
                string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();
                string brandName = "";
                string brandUrl = "";
                try
                {
                    XmlElement brankNode = (XmlElement)serialNode.ParentNode;
                    brandName = brankNode.GetAttribute("Name"); ;
                    string brandSpell = brankNode.GetAttribute("AllSpell").ToString();
                    brandUrl = "http://car.bitauto.com/" + brandSpell + "/";
                }
                catch { }

                string serialUrl = "http://car.bitauto.com/" + serialSpell + "/";

                htmlCode.AppendLine("<dl>");
                htmlCode.AppendLine("<dt><a href=\"" + serialUrl + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" alt=\"" + serialShowName + "\" /></a></dt>");
                htmlCode.AppendLine("<dd><ul>");
                htmlCode.AppendLine("<li class=\"zbrand\"><a href=\"" + serialUrl + "\" target=\"_blank\">" + serialName + "</a></li>");
                htmlCode.AppendLine("<li class=\"brand\"><a href=\"" + brandUrl + "\" target=\"_blank\">" + brandName + "</a></li>");
                string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(serialId));
                if (priceRange.Trim().Length == 0)
                    htmlCode.AppendLine("<li class=\"price\">暂无报价</li>");
                else
                    htmlCode.AppendLine("<li class=\"price\">" + priceRange + "</li>");
                htmlCode.AppendLine("</ul></dd>");
                htmlCode.AppendLine("</dl>");

            }

            return htmlCode.ToString();

        }
        /// <summary>
        /// 得到子品牌在城市中的排名
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public Dictionary<string, int> GetSerialAllCityNumber(int csID, HttpContext httpContext)
        {
            if (csID <= 0)
            {
                return null;
            }
            string cacheKey = "serial_City_Pv_Number_All";

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                //判断缓存中是否存在此列表
                if (httpContext.Cache[cacheKey] != null)
                {
                    xmlDoc = (XmlDocument)httpContext.Cache[cacheKey];
                }
                else
                {
                    string sPath = Path.Combine(WebConfig.DataBlockPath, "Data\\AllSerialCityPV.xml");
                    xmlDoc.Load(sPath);

                    if (xmlDoc != null)
                    {
                        httpContext.Cache.Insert(cacheKey, xmlDoc, new System.Web.Caching.CacheDependency(sPath));
                    }
                }

                if (xmlDoc == null)
                {
                    return null;
                }

                Dictionary<string, int> allCityNumber = new Dictionary<string, int>();
                string sNodePath = "/CitySort/Serial[@ID=\"" + csID + "\"]";
                //得到对应子品牌元素
                XmlElement xmlElem = (XmlElement)xmlDoc.SelectSingleNode(sNodePath);
                if (xmlElem == null || xmlElem.ChildNodes.Count < 1)
                {
                    return null;
                }

                foreach (XmlElement childXmlElem in xmlElem.ChildNodes)
                {
                    if (!allCityNumber.ContainsKey(childXmlElem.GetAttribute("Name").ToString()))
                    {
                        allCityNumber.Add(childXmlElem.GetAttribute("Name").ToString(),
                                          ConvertHelper.GetInteger(childXmlElem.GetAttribute("Sort").ToString()));
                    }
                }
                return allCityNumber;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到子品牌在城市中对比的历史数据
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public Dictionary<string, List<Car_SerialBaseEntity>> GetSerialCityCompareList(int csID, HttpContext httpContext)
        {
            if (csID <= 0)
            {
                return new Dictionary<string, List<Car_SerialBaseEntity>>();
            }
            string cacheKey = "serial_City_Comp_List_" + csID.ToString();

            Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseDic = (Dictionary<string, List<Car_SerialBaseEntity>>)CacheManager.GetCachedData(cacheKey);
            if (carSerialBaseDic == null)
            {
                string filePath = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialCityCompare\\" + csID + "_CityCompare.xml");
                if (File.Exists(filePath))
                {
                    try
                    {
                        carSerialBaseDic = new Dictionary<string, List<Car_SerialBaseEntity>>();
                        using (XmlTextReader reader = new XmlTextReader(filePath))
                        {
                            List<Car_SerialBaseEntity> carSerialBaseList = null;
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    if (reader.Name == "City")
                                    {
                                        reader.MoveToAttribute("Name");
                                        string cityName = reader.Value;
                                        if (!carSerialBaseDic.ContainsKey(cityName))
                                        {
                                            carSerialBaseList = new List<Car_SerialBaseEntity>();
                                            carSerialBaseDic[cityName] = carSerialBaseList;
                                        }
                                    }
                                    else if (reader.Name == "Serial")
                                    {
                                        Car_SerialBaseEntity csEntity = new Car_SerialBaseEntity();
                                        reader.MoveToAttribute("ID");
                                        csEntity.SerialId = ConvertHelper.GetInteger(reader.Value);
                                        reader.MoveToAttribute("PriceRange");
                                        csEntity.SerialPrice = reader.Value;
                                        reader.MoveToAttribute("AllSpell");
                                        csEntity.SerialNameSpell = reader.Value;
                                        reader.MoveToAttribute("ShowName");
                                        csEntity.SerialShowName = reader.Value;
                                        carSerialBaseList.Add(csEntity);
                                    }
                                }
                            }
                            reader.Close();
                        }
                    }
                    catch
                    {

                    }
                    CacheManager.InsertCache(cacheKey, carSerialBaseDic, WebConfig.CachedDuration);
                    // CacheManager.InsertCache(cacheKey, carSerialBaseDic, new System.Web.Caching.CacheDependency(filePath), DateTime.Now.AddDays(1));
                }
            }
            return carSerialBaseDic;
        }
        /// <summary>
        /// 得到没有车型的子品牌列表
        /// </summary>
        /// <returns>子品牌列表</returns>
        public List<int> GetIsNoContainsCarTypeSerialList()
        {
            string cacheKey = "carserial_no_cartype";

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                return (List<int>)HttpContext.Current.Cache[cacheKey];
            }

            List<int> serialList = new List<int>();
            serialList = new Car_SerialDal().GetIsNoContainsCarTypeOfSerialList();

            if (serialList == null || serialList.Count < 1)
            {
                return null;
            }

            HttpContext.Current.Cache.Add(cacheKey, serialList, null, DateTime.Now.AddMinutes(5)
                                            , System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);

            return serialList;
        }

        /// <summary>
        /// 获取某子品牌某年款的车身颜色列表
        /// </summary>
        /// <param name="carYear">如果年款为0，取全部车型的颜色</param>--改为如果年款为0，返回在产、最近年款车身颜色列表
        /// <returns></returns>
        public List<string> GetSerialColors(int serialId, int carYear, bool isAllsale)
        {
            List<string> colorList = new List<string>();
            DataSet ds = new Car_SerialDal().GetCarsColorBySerialId(serialId, isAllsale);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dataTable = ds.Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    DataRow[] rows = null;
                    if (carYear > 0)
                    {
                        rows = dataTable.Select("Car_YearType=" + carYear.ToString());
                    }
                    else
                    {
                        int maxYear = ConvertHelper.GetInteger(dataTable.Compute("max(Car_YearType)", string.Empty));
                        if (maxYear > 0)
                            rows = dataTable.Select("Car_YearType=" + maxYear.ToString());
                        else
                            rows = dataTable.Select(string.Empty);
                    }
                    if (rows != null && rows.Length > 0)
                    {
                        string tempColor;
                        foreach (DataRow row in rows)
                        {
                            string[] colors = row["CarColor"].ToString().Replace("，", ",").Split(',');
                            foreach (string colorStr in colors)
                            {
                                tempColor = colorStr.Trim();
                                if (tempColor.Length > 0 && !colorList.Contains(tempColor))
                                    colorList.Add(tempColor);
                            }
                        }
                    }
                }
            }

            return colorList;
        }
        /// <summary>
        /// 根据颜色名称 获取 颜色（停销 未上市 调用方法）
        /// </summary>
        /// <param name="colorList">颜色名List</param>
        /// <returns></returns>
        public List<SerialColorEntity> GetNoSaleSerialColors(int serialId, List<string> colorList)
        {
            string cacheKey = string.Format("Car_SerialBll_GetNoSaleSerialColors_{0}", serialId);
            object cacheObj = CacheManager.GetCachedData(cacheKey);
            if (cacheObj != null)
                return (List<SerialColorEntity>)cacheObj;

            List<SerialColorEntity> serialColorList = new List<SerialColorEntity>();
            if (!colorList.Any()) return serialColorList;

            DataSet dsAllColor = GetAllSerialColorRGB();//车型子品牌数据
            if (dsAllColor != null && dsAllColor.Tables.Count > 0 && dsAllColor.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsAllColor.Tables[0].Select(" cs_id='" + serialId + "' ");
                if (drs != null && drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        int autoId = ConvertHelper.GetInteger(dr["autoid"]);
                        string colorName = dr["colorName"].ToString().Trim();
                        string colorRGB = dr["colorRGB"].ToString().Trim();
                        if (colorList.Contains(colorName))
                        {
                            serialColorList.Add(new SerialColorEntity()
                            {
                                ColorId = autoId,
                                ColorName = colorName,
                                ColorYear = 0,
                                ColorRGB = colorRGB
                            });
                        }
                    }
                    CacheManager.InsertCache(cacheKey, serialColorList, WebConfig.CachedDuration);
                }
            }
            return serialColorList;
        }
        /// <summary>
        /// 获取在产车型所有颜色
        /// </summary>
        /// <param name="serialId"></param>
        public Dictionary<string, int> GetProduceCarColors(int serialId)
        {
            string cacheKey = "Car_M_GetProduceCarColors_" + serialId;
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                return (Dictionary<string, int>)obj;
            }
            Dictionary<string, int> dictCarsColor = new Dictionary<string, int>();
            DataSet ds = csd.GetProduceCarsColorBySerialId(serialId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string[] colors = row["CarColor"].ToString().Replace("，", ",").Split(',');
                    int year = ConvertHelper.GetInteger(row["car_yeartype"]);
                    foreach (string colorStr in colors)
                    {
                        string colorName = colorStr.Trim();
                        if (!string.IsNullOrEmpty(colorName) && !dictCarsColor.ContainsKey(colorName))
                        {
                            dictCarsColor.Add(colorName, year);
                        }
                    }
                }
            }
            CacheManager.InsertCache(cacheKey, dictCarsColor, WebConfig.CachedDuration);
            return dictCarsColor;
        }
        /// <summary>
        /// 在产子品牌车身颜色
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public List<SerialColorEntity> GetProduceSerialColors(int serialId)
        {
            string cacheKey = string.Format("Car_SerialBll_GetProduceSerialColors_{0}", serialId);
            object cacheObj = CacheManager.GetCachedData(cacheKey);
            if (cacheObj != null)
                return (List<SerialColorEntity>)cacheObj;
            Dictionary<string, int> dictCarColor = GetProduceCarColors(serialId);
            List<SerialColorEntity> serialColorList = new List<SerialColorEntity>();
            DataSet dsAllColor = GetAllSerialColorRGB();//车型子品牌数据
            if (dsAllColor != null && dsAllColor.Tables.Count > 0 && dsAllColor.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsAllColor.Tables[0].Select(" cs_id='" + serialId + "' ");
                if (drs != null && drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        int autoId = ConvertHelper.GetInteger(dr["autoid"]);
                        string colorName = dr["colorName"].ToString().Trim();
                        string colorRGB = dr["colorRGB"].ToString().Trim();
                        if (dictCarColor.ContainsKey(colorName))
                        {
                            serialColorList.Add(new SerialColorEntity()
                            {
                                ColorId = autoId,
                                ColorName = colorName,
                                ColorYear = dictCarColor[colorName],
                                ColorRGB = colorRGB
                            });
                        }
                    }
                    CacheManager.InsertCache(cacheKey, serialColorList, WebConfig.CachedDuration);
                }
            }
            return serialColorList;
        }
        /// <summary>
        /// 获取子品牌的口碑报告 
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public string GetSerialKoubeiReport(int serialId)
        {
            string rptUrl = "";
            List<int> lcsReport = GetAllSerialKouBeiReport();
            if (lcsReport != null && lcsReport.Count > 0)
            {
                if (lcsReport.Contains(serialId))
                {
                    rptUrl = "http://car.bitauto.com/{0}/koubei/baogao/";
                }
            }
            return rptUrl;
            // modified by chengl Aug.17.2010
            //try
            //{
            //    string cacheKey = "Serial_Koubei_Report_Dic";
            //    Dictionary<int, string> reportDic = (Dictionary<int, string>)CacheManager.GetCachedData(cacheKey);
            //    if (reportDic == null)
            //    {
            //        reportDic = new Dictionary<int, string>();
            //        string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialKoubeiReport.xml");
            //        XmlDocument xmlDoc = new XmlDocument();
            //        xmlDoc.Load(xmlFile);
            //        XmlNodeList serialNodeList = xmlDoc.SelectNodes("/KoubeiReports/Serial");
            //        foreach (XmlElement serialNode in serialNodeList)
            //        {
            //            int csId = Convert.ToInt32(serialNode.GetAttribute("ID"));
            //            string reportUrl = serialNode.GetAttribute("ReportUrl");
            //            reportDic[csId] = reportUrl;
            //        }

            //        CacheManager.InsertCache(cacheKey, reportDic, 30);
            //    }
            //    if (reportDic.ContainsKey(serialId))
            //        rptUrl = reportDic[serialId];
            //}
            //catch
            //{ }

        }

        /// <summary>
        /// 取子品牌北京2010车展子品牌top10
        /// </summary>
        /// <param name="top"></param>
        /// <param name="httpContext"></param>
        /// <param name="csList">车展包括的子品牌</param>
        /// <returns></returns>
        public List<Common.Enum.EnumCollection.SerialSortForInterface> GetBeiJing2010SerialTop10(int top, Dictionary<string, string> csList)
        {
            List<Common.Enum.EnumCollection.SerialSortForInterface> lssfi = new List<EnumCollection.SerialSortForInterface>();
            if (top <= 0)
            { return lssfi; }
            DataSet dsCsPV = new PageBase().GetAllSerialNewly7Day();
            if (dsCsPV != null && dsCsPV.Tables.Count > 0 && dsCsPV.Tables[0].Rows.Count > 0)
            {
                int loop = 0;
                for (int i = 0; i < dsCsPV.Tables[0].Rows.Count; i++)
                {
                    if (!csList.ContainsKey(dsCsPV.Tables[0].Rows[i]["cs_ID"].ToString()))
                    { continue; }
                    if (loop >= top)
                    { break; }
                    Common.Enum.EnumCollection.SerialSortForInterface ssfi = new EnumCollection.SerialSortForInterface();
                    ssfi.CsID = int.Parse(dsCsPV.Tables[0].Rows[i]["cs_ID"].ToString());
                    ssfi.CsName = dsCsPV.Tables[0].Rows[i]["cs_name"].ToString().Trim();
                    ssfi.CsShowName = dsCsPV.Tables[0].Rows[i]["cs_showname"].ToString().Trim();
                    ssfi.CsLevel = dsCsPV.Tables[0].Rows[i]["cs_CarLevel"].ToString().Trim();
                    ssfi.CsPV = int.Parse(dsCsPV.Tables[0].Rows[i]["Pv_SumNum"].ToString().Trim());
                    ssfi.CsAllSpell = dsCsPV.Tables[0].Rows[i]["allspell"].ToString().Trim().ToLower();
                    ssfi.CsPriceRange = "";
                    lssfi.Add(ssfi);
                    loop++;
                }
            }
            return lssfi;
        }

        /// <summary>
        /// 取所有子品牌的计划购买人名
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<XmlElement>> GetSerialIntensionDic()
        {
            string cacheKey = "All_serial_intension";
            Dictionary<int, List<XmlElement>> intensionDic = (Dictionary<int, List<XmlElement>>)CacheManager.GetCachedData(cacheKey);
            if (intensionDic == null)
            {
                intensionDic = new Dictionary<int, List<XmlElement>>();
                string fileName = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialIntensionUsers.xml");
                if (File.Exists(fileName))
                {
                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(fileName);
                        XmlNodeList serialNodeList = xmlDoc.SelectNodes("/Serials/Serial");
                        foreach (XmlElement serialNode in serialNodeList)
                        {
                            int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));
                            intensionDic[serialId] = new List<XmlElement>();
                            XmlNodeList userNodeList = serialNode.SelectNodes("user");
                            foreach (XmlElement userNode in userNodeList)
                                intensionDic[serialId].Add(userNode);
                        }
                    }
                    catch
                    {

                    }
                }

                CacheManager.InsertCache(cacheKey, intensionDic, 30);
            }
            return intensionDic;
        }

        /// <summary>
        /// 获取子品牌的视频数量
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public int GetSerialVideoCount(int serialId)
        {
            string cacheKey = "Serial_Videos_count";
            Dictionary<int, int> serialVideoDic = (Dictionary<int, int>)CacheManager.GetCachedData(cacheKey);
            int vCount = 0;
            if (serialVideoDic == null)
            {
                string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\cartree\\treedata\\shipin.xml");
                if (File.Exists(xmlFile))
                {
                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(xmlFile);
                        serialVideoDic = new Dictionary<int, int>();
                        XmlNodeList serialNodeList = xmlDoc.SelectNodes("/data/master/brand/serial");
                        foreach (XmlElement serialNode in serialNodeList)
                        {
                            int sId = Convert.ToInt32(serialNode.GetAttribute("id"));
                            int videoCount = Convert.ToInt32(serialNode.GetAttribute("countnum"));
                            serialVideoDic[sId] = videoCount;
                        }
                        CacheManager.InsertCache(cacheKey, serialVideoDic, 30);
                    }
                    catch { }
                }
            }

            if (serialVideoDic != null && serialVideoDic.ContainsKey(serialId))
                vCount = serialVideoDic[serialId];

            return vCount;
        }
        /// <summary>
        /// 得到品牌下其他子品牌
        /// 请尽量使用下面的：public string GetBrandOtherSerialList(SerialEntity se)
        /// </summary>
        /// <returns></returns>
        public string GetBrandOtherSerialList(Car_SerialEntity cse)
        {
            if (cse == null)
            {
                return "";
            }
            PageBase page = new PageBase();
            List<CarSerialPhotoEntity> carSerialPhotoList = new Car_BrandBll().GetCarSerialPhotoListByCBID(cse.Cb_Id, false);

            carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

            if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
            {
                return "";
            }

            int forLastCount = 0;
            foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
            {
                if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Cs_Id)
                {
                    continue;
                }
                forLastCount++;
            }

            StringBuilder contentBuilder = new StringBuilder(string.Empty);
            string serialUrl = "<a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}</a>";
            int index = 0;
            foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
            {
                bool IsExitsUrl = true;
                if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Cs_Id)
                {
                    continue;
                }
                string priceRang = page.GetSerialIntPriceRangeByID(entity.SerialId);
                if (entity.SaleState == "待销")
                {
                    IsExitsUrl = false;
                    priceRang = "未上市";
                }
                else if (priceRang.Trim().Length == 0)
                {
                    IsExitsUrl = false;
                    priceRang = "暂无报价";
                }
                if (IsExitsUrl)
                {
                    priceRang = string.Format("<a href='http://price.bitauto.com/brand.aspx?newbrandid={0}'>{1}</a>", entity.SerialId, priceRang);
                }
                string tempCsSeoName = string.IsNullOrEmpty(entity.CS_SeoName) ? entity.ShowName : entity.CS_SeoName;
                index++;
                contentBuilder.AppendFormat("<li {2}>{0}<span>{1}</span></li>"
                    , string.Format(serialUrl, entity.CS_AllSpell, tempCsSeoName), priceRang
                    , (index == forLastCount ? "class=\"last\"" : ""));
            }

            StringBuilder brandOtherSerial = new StringBuilder(string.Empty);
            if (contentBuilder.Length > 0)
            {
                string brandUrl = "<a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}</a>";

                brandOtherSerial.Append("<h3>");
                brandOtherSerial.AppendFormat("<span>{0}</span>", string.Format(brandUrl, cse.Cb_AllSpell, cse.Cb_Name + "其他车型"));
                brandOtherSerial.Append("</h3>");
                //brandOtherSerial.AppendFormat("<div class=\"more\">{0}</div>", string.Format(brandUrl, cse.Cb_AllSpell, "更多&gt;&gt;"));
                brandOtherSerial.Append("<ul class=\"list\">");

                brandOtherSerial.Append(contentBuilder.ToString());

                brandOtherSerial.Append("</ul>");
            }

            return brandOtherSerial.ToString();
        }

        /// <summary>
        /// 得到品牌下其他子品牌
        /// </summary>
        /// <returns></returns>
        public string GetBrandOtherSerialList(SerialEntity se)
        {
            if (se == null || se.Id == 0)
            {
                return String.Empty;
            }
            PageBase page = new PageBase();
            List<CarSerialPhotoEntity> carSerialPhotoList = new Car_BrandBll().GetCarSerialPhotoListByCBID(se.BrandId, false);

            carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

            if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
            {
                return "";
            }

            List<string> htmlList = new List<string>();

            foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
            {
                if (entity.SerialLevel == "概念车" || entity.SerialId == se.Id)
                    continue;

                string priceRang = page.GetSerialIntPriceRangeByID(entity.SerialId);
                if (entity.SaleState == "待销")
                    priceRang = "<span class=\"none\">未上市</span>";
                else
                {
                    if (priceRang.Trim().Length == 0)
                        priceRang = "暂无报价";

                    priceRang = "<span>" + priceRang + "</span>";
                }

                string tempCsSeoName = string.IsNullOrEmpty(entity.CS_SeoName) ? entity.ShowName : entity.CS_SeoName;
                htmlList.Add(String.Format("<li><a target=\"_blank\" href=\"/{0}/\">{1}</a>{2}</li>", entity.CS_AllSpell, tempCsSeoName, priceRang));
            }

            List<string> brandHtmlList = new List<string>();
            if (htmlList.Count > 0)
            {
                brandHtmlList.Add(String.Format("<h3><span><a target=\"_blank\" href=\"/{0}/\">{1}其他车型</a></span></h3>", se.Brand.AllSpell, se.Brand.Name));
                brandHtmlList.Add("<ul class=\"list\">");

                brandHtmlList.AddRange(htmlList);

                brandHtmlList.Add("</ul>");
            }

            return String.Concat(brandHtmlList.ToArray());
        }


        /// <summary>
        /// 所有级别(包括概念车)所有销售状态子品牌数据(2010北京车展接口)
        /// </summary>
        /// <returns></returns>
        /// 
        public DataSet GetAllSerialUpLevelInfo()
        {
            return new Car_SerialDal().GetAllSerialUpLevelInfo();
            //string cacheKey = "GetAllSerialUpLevelInfoForExhibition2010BeiJing";
            //DataSet ds = (DataSet)CacheManager.GetCachedData(cacheKey);
            //if (ds == null)
            //{
            //    ds = new Car_SerialDal().GetAllSerialUpLevelInfo();
            //    CacheManager.InsertCache(cacheKey, ds, 5);
            //}
            //return ds;
        }

        /// <summary>
        /// 子品牌12张标准图
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public XmlDocument GetSerial12Photo(int csID)
        {
            XmlDocument doc = new XmlDocument();
            string cacheKey = "Serial12Photo_" + csID.ToString();
            if (CacheManager.GetCachedData(cacheKey) != null)
            {
                doc = (XmlDocument)CacheManager.GetCachedData(cacheKey);
            }
            else
            {
                try
                {
                    //图库接口本地化更改 by sk 2012.12.21
                    string xmlUrl = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialStandardImagePath, csID));
                    doc.Load(xmlUrl);
                    //doc.Load(string.Format(WebConfig.SerialPhoto12ImageInterface, csID.ToString()));
                    CacheManager.InsertCache(cacheKey, doc, 60);
                }
                catch (Exception ex)
                { }
            }
            return doc;
        }

        /// <summary>
        /// 获取子品牌的封面图片
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public static string GetSerialImageUrl(int serialId)
        {
            return GetSerialImageUrl(serialId, "2");
        }

        public static string GetSerialImageUrl(int serialId, string imgType)
        {
            string imgUrl = WebConfig.DefaultCarPic;
            Dictionary<int, XmlElement> urlDic = CarSerialImgUrlService.GetImageUrlDicNew();
            if (urlDic.ContainsKey(serialId))
            {
                if (urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim() != "")
                {
                    // 有新封面
                    imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim(), imgType);
                }
                else
                {
                    // 没有新封面
                    if (urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim() != "")
                    {
                        imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim(), imgType);
                    }
                }
            }

            return imgUrl;
        }
        /// <summary>
        /// 获取子品牌封面图 老图优先
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="imgType">图片尺寸型号</param>
        /// <param name="isUseNew">是否启用优先新地址</param>
        /// <returns></returns>
        public static string GetSerialImageUrl(int serialId, int imgType, bool isUseNew)
        {
            string imgUrl = WebConfig.DefaultCarPic;
            Dictionary<int, XmlElement> urlDic = CarSerialImgUrlService.GetImageUrlDicNew();
            if (urlDic.ContainsKey(serialId))
            {
                if (isUseNew)
                {
                    //新图
                    if (urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim() != "")
                    {
                        imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim(), imgType);
                    }
                    else
                    {
                        if (urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim() != "")
                        {
                            imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim(), imgType);
                        }
                    }
                }
                else
                {
                    //老图
                    if (urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim() != "")
                    {
                        imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim(), imgType);
                    }
                    else
                    {
                        if (urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim() != "")
                        {
                            imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim(), imgType);
                        }
                    }
                }
            }
            return imgUrl;
        }

        /// <summary>
        /// 获取子品牌的白底封面图的散列域名的Url
        /// </summary>
        /// <returns></returns>
        public static string GetSerialCoverHashImgUrl(int serialId)
        {
            string imgUrl = WebConfig.DefaultCarPic;
            Dictionary<int, XmlElement> urlDic = CarSerialImgUrlService.GetImageUrlDicNew();
            if (urlDic.ContainsKey(serialId))
            {
                int imgId = ConvertHelper.GetInteger(urlDic[serialId].GetAttribute("ImageId"));
                string domainName = "img" + (imgId % 4 + 1).ToString() + ".bitautoimg.com";
                imgUrl = urlDic[serialId].GetAttribute("ImageUrl2").Trim();
                if (imgUrl.Length > 0)
                {
                    // 有新封面
                    imgUrl = string.Format(imgUrl, "2").Replace("image.bitautoimg.com", domainName);
                }
                else
                {
                    imgUrl = urlDic[serialId].GetAttribute("ImageUrl").Trim();
                    // 没有新封面
                    if (imgUrl.Length > 0)
                    {
                        imgUrl = string.Format(imgUrl, "2").Replace("image.bitautoimg.com", domainName);
                    }
                    else
                    {
                        imgUrl = WebConfig.DefaultCarPic;
                    }
                }
            }

            return imgUrl;
        }

        /// <summary>
        /// 子品牌名称排序
        /// </summary>
        /// <param name="cspe"></param>
        /// <param name="cspe"></param>
        /// <returns></returns>
        public static int CompareSerialName(CarSerialPhotoEntity cspe, CarSerialPhotoEntity cspe1)
        {
            if (cspe == null
                || cspe1 == null
                || string.IsNullOrEmpty(cspe.CS_Name)
                || string.IsNullOrEmpty(cspe1.CS_Name))
            {
                return -1;
            }
            if (cspe.CS_Name.CompareTo(cspe1.CS_Name) < 0)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 得到所有PV子品牌信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetLefTreePvSerialInfo()
        {
            string cacheKey = "treeIndexSerialAndBrandData";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj == null)
            {
                DataSet ds = csd.GetLefTreePvSerialInfo();
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    return null;
                }
                CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
                return ds;
            }
            return (DataSet)obj;
        }
        /// <summary>
        /// 得到所有销量子品牌信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetLeftTreeSaleSerialInfo()
        {
            string cacheKey = "treeSaleSerialInfoAndBrandData";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj == null)
            {
                DataSet ds = csd.GetLeftTreeSaleSerialInfo();
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    return null;
                }
                CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
                return ds;
            }
            return (DataSet)obj;
        }

        public static Dictionary<int, int> GetSerialPvDic()
        {
            string cacheKey = "allserial_pv_dic";
            Dictionary<int, int> pvDic = (Dictionary<int, int>)CacheManager.GetCachedData(cacheKey);
            if (pvDic == null)
            {
                pvDic = new Dictionary<int, int>();
                XmlDocument xmlDoc = AutoStorageService.GetAllAutoXml();
                XmlNodeList serialNodeList = xmlDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                foreach (XmlElement serialNode in serialNodeList)
                {
                    int serialId = ConvertHelper.GetInteger(serialNode.GetAttribute("ID"));
                    int pvNum = ConvertHelper.GetInteger(serialNode.GetAttribute("CsPV"));
                    pvDic[serialId] = pvNum;
                }
                string xmlPath = Path.Combine(WebConfig.DataBlockPath, "Data\\AllAutoData.xml");
                CacheDependency cacheDependency = new CacheDependency(xmlPath);
                CacheManager.InsertCache(cacheKey, pvDic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
            }
            return pvDic;
        }

        /// <summary>
        /// 获取所有子品牌 UV 字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, int> GetAllSerialUVDict()
        {
            string cacheKey = "Car_AllSerial_UV_Dict";
            var obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (Dictionary<int, int>)obj;
            Dictionary<int, int> dict = new Dictionary<int, int>();
            DataSet ds = csd.GetAllSerialUV();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int serialId = ConvertHelper.GetInteger(dr["cs_id"]);
                    int uvCount = ConvertHelper.GetInteger(dr["UVCount"]);
                    dict.Add(serialId, uvCount);
                }
                CacheManager.InsertCache(cacheKey, dict, WebConfig.CachedDuration);
            }
            return dict;
        }


        /// <summary>
        /// 得到最新的子品牌列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetNewsSerialList()
        {
            string cacheKey = "alserial_NewsCarTypeList";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                return (DataSet)obj;
            }

            DataSet ds = csd.GetNewsSerialList();
            if (ds == null) return null;

            CacheManager.InsertCache(cacheKey, ds, 1440);
            return ds;
        }
        /// <summary>
        /// 得到补贴的子品牌列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetSubsidiesSerialList()
        {
            string cacheKey = "GetSubsidiesSerialList";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (Dictionary<int, int>)obj;

            DataSet ds = csd.GetSubsidiesDataSet();
            if (ds == null) return null;

            Dictionary<int, int> SubsidiesList = new Dictionary<int, int>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int serialId = ConvertHelper.GetInteger(dr["cs_id"].ToString());
                if (serialId > 0 && !SubsidiesList.ContainsKey(serialId))
                {
                    SubsidiesList.Add(serialId, 1);
                }
            }
            CacheManager.InsertCache(cacheKey, SubsidiesList, 60);
            return SubsidiesList;
        }

        #region 口碑报告

        ///// <summary>
        ///// 检查子品牌是否含有口碑报告
        ///// </summary>
        ///// <param name="csID">子品牌ID</param>
        ///// <returns>是否有口碑报告</returns>
        //public bool SerialHasKouBeiReport(int csID)
        //{
        //    bool hasReport = false;
        //    List<int> lcsReport = GetAllSerialKouBeiReport();
        //    if (lcsReport != null && lcsReport.Count > 0)
        //    {
        //        if (lcsReport.Contains(csID))
        //        {
        //            hasReport = true;
        //        }
        //    }
        //    return hasReport;
        //}

        /// <summary>
        /// 取所有有口碑报告的子品牌ID列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllSerialKouBeiReport()
        {
            // 接口不可用
            List<int> lcsReport = new List<int>();

            //string cacheKey = "GetAllSerialKouBeiReport";
            //Object getAllSerialKouBeiReport = CacheManager.GetCachedData(cacheKey);
            //if (getAllSerialKouBeiReport == null)
            //{
            //	try
            //	{
            //		XmlDocument doc = new XmlDocument();
            //		doc.Load(WebConfig.SerialKouReport);
            //		if (doc != null && doc.HasChildNodes)
            //		{
            //			XmlNodeList xnl = doc.SelectNodes("/feed/entry");
            //			if (xnl != null && xnl.Count > 0)
            //			{
            //				foreach (XmlNode xn in xnl)
            //				{
            //					if (xn.Attributes["modelid"] != null)
            //					{
            //						int csid = 0;
            //						if (int.TryParse(xn.Attributes["modelid"].Value.ToString(), out  csid))
            //						{
            //							if (csid > 0 && !lcsReport.Contains(int.Parse(xn.Attributes["modelid"].Value.ToString())))
            //							{
            //								lcsReport.Add(int.Parse(xn.Attributes["modelid"].Value.ToString()));
            //							}
            //						}
            //					}
            //				}
            //			}

            //		}
            //	}
            //	catch
            //	{

            //	}
            //	CacheManager.InsertCache(cacheKey, lcsReport, WebConfig.CachedDuration);
            //}
            //else
            //{
            //	lcsReport = (List<int>)getAllSerialKouBeiReport;
            //}
            return lcsReport;
        }

        #endregion

        #region 子品牌车身颜色RGB
        /// <summary>
        /// 子品牌颜色 在产
        /// modified by sk 颜色年款 取的图片的年款 2015.06.24
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="colorNames"></param>
        public List<SerialColorForSummaryEntity> GetSerialColorRGBByCsID(int serialId, int year, List<SerialColorEntity> serialColorList)
        {
            List<SerialColorForSummaryEntity> list = new List<SerialColorForSummaryEntity>();
            Dictionary<string, XmlNode> dic = GetSerialColorPhotoByCsID(serialId, year);//图库数据
            DataSet dsAllColor = GetAllSerialColorRGB();//车型子品牌数据
            if (dsAllColor != null && dsAllColor.Tables.Count > 0 && dsAllColor.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsAllColor.Tables[0].Select(" cs_id='" + serialId + "' ");
                if (drs != null && drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        string colorName = dr["colorName"].ToString().Trim();
                        string colorRGB = dr["colorRGB"].ToString().Trim();
                        string imageUrl = string.Empty;
                        string link = string.Empty;
                        int yearType = 0;
                        if (dic.ContainsKey(colorName))
                        {
                            if (dic[colorName].Attributes["ImageUrl"] != null
                                       && dic[colorName].Attributes["ImageUrl"].Value != "")
                                imageUrl = dic[colorName].Attributes["ImageUrl"].Value;
                            if (dic[colorName].Attributes["Link"] != null
                                       && dic[colorName].Attributes["Link"].Value != "")
                                link = dic[colorName].Attributes["Link"].Value;
                            if (dic[colorName].Attributes["CarYear"] != null
                                       && dic[colorName].Attributes["CarYear"].Value != "")
                                yearType = ConvertHelper.GetInteger(dic[colorName].Attributes["CarYear"].Value);
                        }
                        if (serialColorList.Find(p => p.ColorName == colorName) != null && !string.IsNullOrEmpty(colorRGB))//是否是在产颜色
                        {
                            list.Add(new SerialColorForSummaryEntity()
                            {
                                ColorName = colorName,
                                ColorRGB = colorRGB,
                                ImageUrl = imageUrl,
                                Link = link,
                                YearType = yearType // serialColorList.Find(p => p.ColorName == colorName).ColorYear
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 根据子品牌ID 获取颜色RGB HTML代码 (老代码等颜色图片上线废弃)
        /// </summary>
        /// <param name="csID"></param>
        /// <param name="colorNames"></param>
        /// <param name="RGBHtml"></param>
        /// <param name="RGBTitle"></param>
        public void GetSerialColorRGBByCsID(int csID, List<string> colorNames, out string RGBHtml, out string RGBTitle)
        {
            List<string> rgb = new List<string>();
            StringBuilder sbHTML = new StringBuilder();
            StringBuilder sbTitle = new StringBuilder();
            RGBHtml = "";
            RGBTitle = "";
            DataSet dsAllColor = GetAllSerialColorRGB();
            if (dsAllColor != null && dsAllColor.Tables.Count > 0 && dsAllColor.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsAllColor.Tables[0].Select(" cs_id='" + csID.ToString() + "' ");
                if (drs != null && drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        if (colorNames.Contains(dr["colorName"].ToString().Trim()))
                        {
                            sbTitle.Append(dr["colorName"].ToString().Trim() + " ");
                            if (!rgb.Contains(dr["colorRGB"].ToString().Trim()))
                            {
                                sbHTML.Append("<em style=\"background:" + dr["colorRGB"].ToString().Trim() + ";\"></em>");
                                rgb.Add(dr["colorRGB"].ToString().Trim());
                            }
                        }
                    }
                }
            }
            if (sbHTML.Length > 0)
            { RGBHtml = sbHTML.ToString(); }
            if (sbTitle.Length > 0)
            { RGBTitle = sbTitle.ToString(); }
            if (RGBHtml == "" && RGBTitle != "")
            {
                RGBHtml = RGBTitle;
            }
        }

        /// <summary>
        /// 根据子品牌ID 获取颜色RGB HTML代码 (新增颜色图片功能)
        /// </summary>
        /// <param name="csID"></param>
        /// <param name="colorNames"></param>
        /// <param name="RGBHtml"></param>
        /// <param name="RGBTitle"></param>
        public void GetSerialColorRGBByCsID(int csID, int year, List<string> colorNames, out string RGBHtml, out string RGBTitle)
        {
            List<string> rgb = new List<string>();
            StringBuilder sbHTML = new StringBuilder();
            StringBuilder sbTitle = new StringBuilder();
            StringBuilder sbOnlyHasColor = new StringBuilder();
            bool isHasMoreThenOne = false;
            Dictionary<string, XmlNode> dic = GetSerialColorPhotoByCsID(csID, year);
            RGBHtml = "";
            RGBTitle = "";
            DataSet dsAllColor = GetAllSerialColorRGB();
            if (dsAllColor != null && dsAllColor.Tables.Count > 0 && dsAllColor.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsAllColor.Tables[0].Select(" cs_id='" + csID.ToString() + "' ");
                if (drs != null && drs.Length > 0)
                {
                    int noPicCount = 1;
                    int hasPicCount = 1;
                    foreach (DataRow dr in drs)
                    {
                        // 色块 子品牌综述页不能多于13个、年款页不能多于18个 
                        if (year > 0)
                        {
                            // 年款
                            if (noPicCount > 18 && hasPicCount > 18)
                            { break; }
                        }
                        else
                        {
                            // 子品牌
                            if (noPicCount > 13 && hasPicCount > 13)
                            { break; }
                        }

                        if (colorNames.Contains(dr["colorName"].ToString().Trim()))
                        {
                            sbTitle.Append(dr["colorName"].ToString().Trim() + " ");
                            if (!rgb.Contains(dr["colorRGB"].ToString().Trim()))
                            {

                                if (year > 0)
                                {
                                    // 年款
                                    if (noPicCount <= 18)
                                    {
                                        sbHTML.Append("<em onmouseout=\"BtHide('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "')\" onmouseover=\"BtShow('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "')\" style=\"background:" + dr["colorRGB"].ToString().Trim() + ";\"  ");
                                    }
                                }
                                else
                                {
                                    // 子品牌
                                    if (noPicCount <= 13)
                                    {
                                        sbHTML.Append("<em onmouseout=\"BtHide('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "')\" onmouseover=\"BtShow('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "')\" style=\"background:" + dr["colorRGB"].ToString().Trim() + ";\"  ");
                                    }
                                }

                                rgb.Add(dr["colorRGB"].ToString().Trim());

                                // 增加颜色图片呈现
                                // 遍历子品牌所有颜色名与当前RGB相同的所有中文名
                                bool isHasColorPic = false;
                                StringBuilder temp = new StringBuilder();
                                foreach (DataRow drSub in drs)
                                {
                                    if (drSub["colorRGB"].ToString().Trim() == dr["colorRGB"].ToString().Trim())
                                    {
                                        if (dic.ContainsKey(drSub["colorName"].ToString().Trim()))
                                        {
                                            // 当有颜色的时候只显示有颜色
                                            if (year > 0)
                                            {
                                                // 年款
                                                if (hasPicCount <= 18)
                                                {
                                                    sbOnlyHasColor.Append("<em onmouseout=\"BtHide('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "')\" onmouseover=\"BtShow('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "')\" style=\"background:" + dr["colorRGB"].ToString().Trim() + ";\"  >");
                                                }
                                            }
                                            else
                                            {
                                                // 子品牌
                                                if (hasPicCount <= 13)
                                                {
                                                    sbOnlyHasColor.Append("<em onmouseout=\"BtHide('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "')\" onmouseover=\"BtShow('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "')\" style=\"background:" + dr["colorRGB"].ToString().Trim() + ";\"  >");
                                                }
                                            }


                                            // 有颜色图片对应
                                            temp.Append("<div class=\"pop_color\" id=\"color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "\" style=\"display:none\">");
                                            temp.Append("<div class=\"carbox\"><p>");
                                            if (dic[drSub["colorName"].ToString().Trim()].Attributes["Link"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() != "" && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"].Value.ToString() != "")
                                            {
                                                temp.Append("<a target=\"_blank\" href=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() + "\">");
                                                temp.Append("<img src=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"].Value.ToString() + "\" alt=\"\" />");
                                                temp.Append("</a>");
                                            }
                                            temp.Append("</p></div>");
                                            if (dic[drSub["colorName"].ToString().Trim()].Attributes["Link"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() != "" && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() != "")
                                            {
                                                temp.Append("<b><a target=\"_blank\" href=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() + "\">" + dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() + "</a></b>");
                                            }
                                            temp.Append("</div>");
                                            // 
                                            if (year > 0)
                                            {
                                                // 年款
                                                if (hasPicCount <= 18)
                                                {
                                                    sbOnlyHasColor.Append(temp.ToString() + "</em>");
                                                    hasPicCount++;
                                                }
                                            }
                                            else
                                            {
                                                // 子品牌
                                                if (hasPicCount <= 13)
                                                {
                                                    sbOnlyHasColor.Append(temp.ToString() + "</em>");
                                                    hasPicCount++;
                                                }
                                            }

                                            isHasColorPic = true;
                                            isHasMoreThenOne = true;
                                            break;
                                        }
                                    }
                                }

                                if (year > 0)
                                {
                                    // 年款
                                    if (noPicCount <= 18)
                                    {
                                        if (!isHasColorPic)
                                        {
                                            sbHTML.Append(" title=\"" + dr["colorName"].ToString().Trim() + "\" >");
                                        }
                                        else
                                        {
                                            sbHTML.Append(" >" + temp.ToString());
                                        }
                                        sbHTML.Append("</em>");
                                        noPicCount++;
                                    }
                                }
                                else
                                {
                                    // 子品牌
                                    if (noPicCount <= 13)
                                    {
                                        if (!isHasColorPic)
                                        {
                                            sbHTML.Append(" title=\"" + dr["colorName"].ToString().Trim() + "\" >");
                                        }
                                        else
                                        {
                                            sbHTML.Append(" >" + temp.ToString());
                                        }
                                        sbHTML.Append("</em>");
                                        noPicCount++;
                                    }
                                }

                            }
                        }
                        //else
                        //{
                        //    sbHTML.AppendLine("<!-- dr[colorName]:|" + dr["colorName"].ToString().Trim() + "| -->");
                        //    foreach (string str in colorNames)
                        //    {
                        //        sbHTML.AppendLine("<!-- |" + str + "| -->");
                        //    }
                        //}
                    }
                }
            }
            if (isHasMoreThenOne)
            {
                if (sbOnlyHasColor.Length > 0)
                {
                    RGBHtml = sbOnlyHasColor.ToString();
                }
            }
            else
            {
                if (sbHTML.Length > 0)
                {
                    RGBHtml = sbHTML.ToString();
                }
            }
            if (sbTitle.Length > 0 && dic.Count <= 0)
            {
                RGBTitle = sbTitle.ToString();
            }
            if (RGBHtml == "" && RGBTitle != "")
            {
                RGBHtml = RGBTitle;
            }
        }
        /// <summary>
        /// 获取颜色RGB 根据 子品牌颜色 和 参数 颜色名称
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="colorNames">颜色名称List</param>
        /// <param name="colorNameList"></param>
        /// <param name="colorRGBList"></param>
        public List<SerialColorEntity> GetColorRGBBySerialId(int serialId, int year, List<string> colorNames)
        {
            List<SerialColorEntity> list = new List<SerialColorEntity>();
            Dictionary<string, XmlNode> dic = this.GetSerialColorPhotoByCsID(serialId, year);
            DataSet dsAllColor = this.GetAllSerialColorRGB();
            if (dsAllColor != null && dsAllColor.Tables.Count > 0 && dsAllColor.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsAllColor.Tables[0].Select(" cs_id='" + serialId + "' ");
                foreach (DataRow dr in drs)
                {
                    var colorName = dr["colorName"].ToString().Trim();
                    var colorRGB = dr["colorRGB"].ToString();
                    if (colorNames.Contains(colorName) && !string.IsNullOrEmpty(colorRGB))
                    {
                        int linkExist = 1;//是否有图片
                        string link = string.Empty;
                        string imageUrl = string.Empty;

                        if (dic.ContainsKey(colorName))
                        {
                            link = dic[colorName].Attributes["Link"].Value;
                            imageUrl = dic[colorName].Attributes["ImageUrl"].Value;
                            linkExist = !string.IsNullOrEmpty(link) ? 0 : 1;
                        }

                        list.Add(new SerialColorEntity()
                        {
                            ColorName = colorName,
                            ColorRGB = colorRGB,
                            ColorYear = year,
                            ColorId = linkExist, //制作排序用
                            ColorLink = link,
                            ColorImageUrl = imageUrl
                        });
                    }
                }
                list.Sort((x, y) => x.ColorId.CompareTo(y.ColorId));
            }
            return list;
        }
        /// <summary>
        /// 新子品牌综述页使用 Apr.26.2011
        /// 根据子品牌ID 获取颜色RGB HTML代码 (新增颜色图片功能)
        /// </summary>
        /// <param name="csID"></param>
        /// <param name="year"></param>
        /// <param name="subfix"></param>
        /// <param name="colorNames"></param>
        /// <param name="RGBHtml"></param>
        /// <param name="RGBTitle"></param>
        /// <param name="colorName"></param>
        /// <param name="colorRGB"></param>
        public void GetSerialColorRGBByCsID(int csID, int year, int subfix, List<string> colorNames
            , out string RGBHtml, out string RGBTitle
            , out List<string> colorName, out List<string> colorRGB)
        {
            colorName = new List<string>();
            colorRGB = new List<string>();
            StringBuilder sbHTML = new StringBuilder();
            StringBuilder sbTitle = new StringBuilder();
            // StringBuilder sbOnlyHasColor = new StringBuilder();
            // bool isHasMoreThenOne = false;
            Dictionary<string, XmlNode> dic = GetSerialColorPhotoByCsID(csID, year);
            RGBHtml = "";
            RGBTitle = "";
            DataSet dsAllColor = GetAllSerialColorRGB();
            if (dsAllColor != null && dsAllColor.Tables.Count > 0 && dsAllColor.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsAllColor.Tables[0].Select(" cs_id='" + csID.ToString() + "' ");
                if (drs != null && drs.Length > 0)
                {
                    // int noPicCount = 1;
                    int hasPicCount = 1;
                    // 年款页最大色块数
                    int maxYearCount = 15;
                    // 子品牌综述页最大色块数
                    int maxSummaryCount = 11;
                    //没有图片的颜色放到后面
                    List<string> notImgList = new List<string>();
                    foreach (DataRow dr in drs)
                    {
                        // 色块 子品牌综述页不能多于11个、年款页不能多于16个 
                        if (year > 0)
                        {
                            // 年款
                            if (hasPicCount > maxYearCount)
                            { break; }
                        }
                        else
                        {
                            // 子品牌
                            if (hasPicCount > maxSummaryCount)
                            { break; }
                        }

                        if (colorNames.Contains(dr["colorName"].ToString().Trim()) && dr["colorRGB"].ToString().Trim() != string.Empty)
                        {
                            // 在产车型颜色名中有此颜色名
                            sbTitle.Append(dr["colorName"].ToString().Trim() + " ");

                            // 增加颜色图片呈现
                            // 遍历子品牌所有颜色名与当前RGB相同的所有中文名
                            bool isHasColorPic = false;
                            StringBuilder temp = new StringBuilder();
                            foreach (DataRow drSub in drs)
                            {
                                if (drSub["colorName"].ToString().Trim() == dr["colorName"].ToString().Trim())
                                {
                                    if (dic.ContainsKey(drSub["colorName"].ToString().Trim()))
                                    {
                                        // 有颜色图片对应
                                        if (dic[drSub["colorName"].ToString().Trim()].Attributes["Link"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() != "" && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() != "")
                                        {
                                            temp.AppendLine("<a target=\"_blank\" href=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() + "\"><span style=\"background:" + dr["colorRGB"].ToString().Trim() + "\">" + dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() + "</span></a>");
                                        }


                                        temp.AppendLine("<div class=\"popColor2\" id=\"color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "_" + System.Web.HttpUtility.HtmlEncode(drSub["colorName"].ToString().Trim()) + "\" style=\"display:none\">");
                                        temp.AppendLine("<div class=\"popColorinner\"><p>");
                                        if (dic[drSub["colorName"].ToString().Trim()].Attributes["Link"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() != "" && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"].Value.ToString() != "")
                                        {
                                            temp.AppendLine("<a target=\"_blank\" href=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() + "\">");
                                            temp.AppendLine("<img src=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"].Value.ToString().Replace("_5.", "_" + subfix.ToString() + ".") + "\" alt=\"\" />");
                                            temp.AppendLine("</a>");
                                        }
                                        temp.AppendLine("</p>");

                                        if (dic[drSub["colorName"].ToString().Trim()].Attributes["Link"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() != "" && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() != "")
                                        {
                                            temp.AppendLine("<a target=\"_blank\" href=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() + "\">" + dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() + "</a>");
                                        }

                                        temp.AppendLine("</div>");
                                        temp.AppendLine("</div>");
                                        if (year > 0)
                                        {
                                            // 年款
                                            if (hasPicCount <= maxYearCount)
                                            {
                                                sbHTML.Append("<em onmouseout=\"BtHide('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "_" + System.Web.HttpUtility.HtmlEncode(drSub["colorName"].ToString().Trim()) + "');this.className=''\" onmouseover=\"BtShow('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "_" + System.Web.HttpUtility.HtmlEncode(drSub["colorName"].ToString().Trim()) + "');this.className='current'\" class=\"\"  >");
                                                sbHTML.Append(temp.ToString() + "</em>");
                                                // 颜色名和颜色RGB
                                                colorName.Add(drSub["colorName"].ToString().Trim());
                                                colorRGB.Add(dr["colorRGB"].ToString().Trim());
                                            }
                                        }
                                        else
                                        {
                                            // 子品牌
                                            if (hasPicCount <= maxSummaryCount)
                                            {
                                                sbHTML.Append("<em onmouseout=\"BtHide('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "_" + System.Web.HttpUtility.HtmlEncode(drSub["colorName"].ToString().Trim()) + "');this.className=''\" onmouseover=\"BtShow('color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "_" + System.Web.HttpUtility.HtmlEncode(drSub["colorName"].ToString().Trim()) + "');this.className='current'\" class=\"\"  >");
                                                sbHTML.Append(temp.ToString() + "</em>");
                                                // 颜色名和颜色RGB
                                                colorName.Add(drSub["colorName"].ToString().Trim());
                                                colorRGB.Add(dr["colorRGB"].ToString().Trim());
                                            }
                                        }
                                        isHasColorPic = true;
                                        hasPicCount++;
                                        break;
                                    }
                                    break;
                                }
                            }

                            if (year > 0)
                            {
                                // 当没有颜色图片时显示色块和title
                                // 年款
                                if (!isHasColorPic && hasPicCount <= maxYearCount)
                                {
                                    notImgList.Add("<em title=\"" + dr["colorName"].ToString().Trim() + "\" ><span style=\"background:" + dr["colorRGB"].ToString().Trim() + "\"></span></em>");
                                    // 颜色名和颜色RGB
                                    colorName.Add(dr["colorName"].ToString().Trim());
                                    colorRGB.Add(dr["colorRGB"].ToString().Trim());
                                }
                            }
                            else
                            {
                                // 子品牌
                                if (!isHasColorPic && hasPicCount <= maxSummaryCount)
                                {
                                    notImgList.Add("<em title=\"" + dr["colorName"].ToString().Trim() + "\" ><span style=\"background:" + dr["colorRGB"].ToString().Trim() + "\"></span></em>");
                                    // 颜色名和颜色RGB
                                    colorName.Add(dr["colorName"].ToString().Trim());
                                    colorRGB.Add(dr["colorRGB"].ToString().Trim());
                                }
                            }
                        }
                    }
                    if (year > 0)//年款
                    {
                        if (notImgList.Count > 0)
                        {
                            for (int i = 0; i < notImgList.Count && hasPicCount <= maxYearCount; i++, hasPicCount++)
                            {
                                sbHTML.Append(notImgList[i]);
                            }
                        }
                    }
                    else//子品牌
                    {
                        if (notImgList.Count > 0)
                        {
                            for (int i = 0; i < notImgList.Count && hasPicCount <= maxSummaryCount; i++, hasPicCount++)
                            {
                                sbHTML.Append(notImgList[i]);
                            }
                        }
                    }
                }
            }

            if (sbHTML.Length > 0)
            {
                RGBHtml = sbHTML.ToString();
            }
            if (sbTitle.Length > 0 && dic.Count <= 0)
            {
                RGBTitle = sbTitle.ToString();
            }
            if (RGBHtml == "" && RGBTitle != "")
            {
                RGBHtml = RGBTitle;
            }
        }

        /// <summary>
        /// 取所有子品牌车身颜色 颜色名、RGB值<子品牌ID,<颜色名,RGB值>>
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Dictionary<string, string>> GetAllSerialColorNameRGB()
        {
            Dictionary<int, Dictionary<string, string>> dic = new Dictionary<int, Dictionary<string, string>>();
            string cacheKey = "Car_SerialBll_GetAllSerialColorNameRGB";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                dic = (Dictionary<int, Dictionary<string, string>>)obj;
            }
            else
            {
                // 取有RGB值的车身颜色
                DataSet ds = GetAllSerialColorRGB();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int csid = int.Parse(dr["cs_id"].ToString());
                        string colorName = dr["colorName"].ToString().Trim();
                        string colorRGB = dr["colorRGB"].ToString().Trim();
                        if (dic.ContainsKey(csid))
                        {
                            if (!dic[csid].ContainsKey(colorName))
                            { dic[csid].Add(colorName, colorRGB); }
                        }
                        else
                        {
                            Dictionary<string, string> dicCs = new Dictionary<string, string>();
                            dicCs.Add(colorName, colorRGB);
                            dic.Add(csid, dicCs);
                        }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
            }
            return dic;
        }

        /// <summary>
        /// 取所有子品牌颜色RGB值
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialColorRGB()
        {
            DataSet ds = new DataSet();
            string cacheKey = "GetAllSerialColorRGB";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                ds = (DataSet)obj;
            }
            else
            {
                ds = new Car_SerialDal().GetAllSerialColorRGB(0);
                CacheManager.InsertCache(cacheKey, ds, 60);
            }
            return ds;
        }

        /// <summary>
        /// 取子品牌颜色图片
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="year">年款(不取年款时传0)</param>
        /// <returns></returns>
        public Dictionary<string, XmlNode> GetSerialColorPhotoByCsID(int csID, int year)
        {
            XmlDocument doc = new XmlDocument();
            Dictionary<string, XmlNode> dic = new Dictionary<string, XmlNode>();
            string cacheKey = "GetSerialColorPhotoByCsID_" + csID.ToString() + "_" + year.ToString();
            object getSerialColorPhotoByCsID = null;
            CacheManager.GetCachedData(cacheKey, out getSerialColorPhotoByCsID);
            if (getSerialColorPhotoByCsID == null)
            {
                //图库接口本地化更改 by sk 2012.12.21
                string interfacePath = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialColorPath);
                //string interfacePath = WebConfig.CarColorPhoto + "?dataname=serialcolorimage&serialid=" + csID.ToString() + "&showfullurl=true&subfix=5";
                if (year > 0)
                {
                    //interfacePath += "&showall=true";
                    interfacePath = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialColorAllPath);
                }
                try
                {
                    doc.Load(string.Format(interfacePath, csID));
                    CacheManager.InsertCache(cacheKey, doc, WebConfig.CachedDuration);
                }
                catch
                { }
            }
            else
            {
                doc = (XmlDocument)getSerialColorPhotoByCsID;
            }

            if (doc != null && doc.HasChildNodes)
            {
                XmlNodeList xnl = null;
                if (year > 0)
                {
                    // 年款
                    xnl = doc.SelectNodes("/ImageData/ImageList/ImageInfo[@CarYear='" + year.ToString() + "']");
                }
                else
                {
                    // 子品牌
                    xnl = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
                }
                if (xnl != null && xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        if (xn.Attributes["ImageName"] != null && xn.Attributes["ImageName"].Value.Trim() != "")
                        {
                            if (!dic.ContainsKey(xn.Attributes["ImageName"].Value.Trim()))
                            {
                                dic.Add(xn.Attributes["ImageName"].Value.Trim(), xn);
                            }
                        }
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 取子品牌颜色图片
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="year">年款(不取年款时传0)</param>
        /// <param name="subfix">图片大小编号</param>
        /// <returns></returns>
        public Dictionary<string, XmlNode> GetSerialColorPhotoByCsID(int csID, int year, int subfix)
        {
            XmlDocument doc = new XmlDocument();
            Dictionary<string, XmlNode> dic = new Dictionary<string, XmlNode>();
            string cacheKey = "GetSerialColorPhotoByCsID_" + csID.ToString() + "_" + year.ToString() + "_" + subfix.ToString();
            object getSerialColorPhotoByCsID = null;
            CacheManager.GetCachedData(cacheKey, out getSerialColorPhotoByCsID);
            if (getSerialColorPhotoByCsID == null)
            {
                //图库接口本地化更改 by sk 2012.12.21
                string interfacePath = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialColorPath);
                //string interfacePath = WebConfig.CarColorPhoto + "?dataname=serialcolorimage&serialid=" + csID.ToString() + "&showfullurl=true&subfix=" + subfix.ToString();
                if (year > 0)
                {
                    interfacePath = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialColorAllPath);
                    //interfacePath += "&showall=true";
                }
                try
                {
                    doc.Load(string.Format(interfacePath, csID));
                    CacheManager.InsertCache(cacheKey, doc, WebConfig.CachedDuration);
                }
                catch
                { }
            }
            else
            {
                doc = (XmlDocument)getSerialColorPhotoByCsID;
            }

            if (doc != null && doc.HasChildNodes)
            {
                XmlNodeList xnl = null;
                if (year > 0)
                {
                    // 年款
                    xnl = doc.SelectNodes("/ImageData/ImageList/ImageInfo[@CarYear='" + year.ToString() + "']");
                }
                else
                {
                    // 子品牌
                    xnl = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
                }
                if (xnl != null && xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        if (xn.Attributes["ImageName"] != null && xn.Attributes["ImageName"].Value.Trim() != "")
                        {
                            if (!dic.ContainsKey(xn.Attributes["ImageName"].Value.Trim()))
                            {
                                dic.Add(xn.Attributes["ImageName"].Value.Trim(), xn);
                            }
                        }
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 新车型综述页使用 Jul.8.2011
        /// 根据车型车身颜色 子品牌ID 获取颜色RGB HTML代码
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="subfix">图片规格</param>
        /// <param name="top">取色块数</param>
        /// <param name="idSubFix">页面元素ID前缀</param>
        /// <param name="colorNames">颜色中午列表</param>
        /// <param name="RGBHtml">生成的颜色HTML</param>
        /// <param name="RGBTitle">颜色的名集合</param>
        public void GetCarColorRGBByCsID(int csID, int year, int subfix, int top, string idSubFix, List<string> colorNames, out string RGBHtml, out string RGBTitle)
        {
            List<string> rgb = new List<string>();
            StringBuilder sbHTML = new StringBuilder();
            StringBuilder sbTitle = new StringBuilder();

            Dictionary<string, XmlNode> dic = GetSerialColorPhotoByCsID(csID, year);
            RGBHtml = "";
            RGBTitle = "";
            DataSet dsAllColor = GetAllSerialColorRGB();
            if (dsAllColor != null && dsAllColor.Tables.Count > 0 && dsAllColor.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsAllColor.Tables[0].Select(" cs_id='" + csID.ToString() + "' ");
                if (drs != null && drs.Length > 0)
                {
                    // int noPicCount = 1;
                    int hasPicCount = 1;
                    // 子品牌综述页最大色块数
                    int maxSummaryCount = top;
                    foreach (DataRow dr in drs)
                    {
                        // 色块 子品牌综述页不能多于11个
                        // 子品牌
                        if (hasPicCount > maxSummaryCount)
                        { break; }
                        if (colorNames.Contains(dr["colorName"].ToString().Trim()))
                        {
                            // 在产车型颜色名中有此颜色名
                            sbTitle.Append(dr["colorName"].ToString().Trim() + " ");
                            if (!rgb.Contains(dr["colorRGB"].ToString().Trim()))
                            {
                                rgb.Add(dr["colorRGB"].ToString().Trim());
                                // 增加颜色图片呈现
                                // 遍历子品牌所有颜色名与当前RGB相同的所有中文名
                                bool isHasColorPic = false;
                                StringBuilder temp = new StringBuilder();
                                foreach (DataRow drSub in drs)
                                {
                                    if (drSub["colorRGB"].ToString().Trim() == dr["colorRGB"].ToString().Trim())
                                    {
                                        if (dic.ContainsKey(drSub["colorName"].ToString().Trim()))
                                        {
                                            // 有颜色图片对应
                                            if (dic[drSub["colorName"].ToString().Trim()].Attributes["Link"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() != "" && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() != "")
                                            {
                                                temp.AppendLine("<a target=\"_blank\" href=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() + "\"><span style=\"background:" + dr["colorRGB"].ToString().Trim() + "\">" + dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() + "</span></a>");
                                            }

                                            temp.AppendLine("<div class=\"popColor2\" id=\"" + idSubFix + "color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "\" style=\"display:none\">");
                                            temp.AppendLine("<div class=\"popColorinner\"><p>");
                                            if (dic[drSub["colorName"].ToString().Trim()].Attributes["Link"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() != "" && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"].Value.ToString() != "")
                                            {
                                                temp.AppendLine("<a target=\"_blank\" href=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() + "\">");
                                                temp.AppendLine("<img src=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"].Value.ToString().Replace("_5.", "_" + subfix.ToString() + ".") + "\" alt=\"\" />");
                                                temp.AppendLine("</a>");
                                            }
                                            temp.AppendLine("</p>");
                                            if (dic[drSub["colorName"].ToString().Trim()].Attributes["Link"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() != "" && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() != "")
                                            {
                                                temp.AppendLine("<a target=\"_blank\" href=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() + "\">" + dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() + "</a>");
                                            }
                                            temp.AppendLine("</div>");
                                            temp.AppendLine("</div>");
                                            // 子品牌
                                            if (hasPicCount <= maxSummaryCount)
                                            {
                                                sbHTML.Append("<em onmouseout=\"BtHide('" + idSubFix + "color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "');this.className=''\" onmouseover=\"BtShow('" + idSubFix + "color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "');this.className='current'\" class=\"\"  >");
                                                sbHTML.Append(temp.ToString() + "</em>");
                                            }
                                            isHasColorPic = true;
                                            break;
                                        }
                                    }
                                }

                                // 子品牌
                                if (!isHasColorPic && hasPicCount <= maxSummaryCount)
                                {
                                    sbHTML.Append("<em title=\"" + dr["colorName"].ToString().Trim() + "\" ><span style=\"background:" + dr["colorRGB"].ToString().Trim() + "\"></span></em>");
                                }

                                hasPicCount++;
                            }
                        }

                    }
                }
            }

            if (sbHTML.Length > 0)
            {
                RGBHtml = sbHTML.ToString();
            }
            if (sbTitle.Length > 0 && dic.Count <= 0)
            {
                RGBTitle = sbTitle.ToString();
            }
            if (RGBHtml == "" && RGBTitle != "")
            {
                RGBHtml = RGBTitle;
            }
        }

        /// <summary>
        /// 车款综述页1200改版用 根据车型车身颜色 子品牌ID 获取颜色RGB HTML代码  
        /// zf
        /// 2016-10-9   
        /// </summary>
        /// <param name="csID"></param>
        /// <param name="year"></param>
        /// <param name="subfix"></param>
        /// <param name="top"></param>
        /// <param name="idSubFix"></param>
        /// <param name="colorNames"></param>
        /// <param name="RGBHtml"></param>
        /// <param name="RGBTitle"></param>
        public void GetCarColorRGBByCsIDFor1200(int csID, int year, int subfix, int top, string idSubFix, List<string> colorNames, out string RGBHtml, out string RGBTitle)
        {
            List<string> rgb = new List<string>();
            StringBuilder sbHTML = new StringBuilder();
            StringBuilder sbTitle = new StringBuilder();

            Dictionary<string, XmlNode> dic = GetSerialColorPhotoByCsID(csID, year);
            RGBHtml = "";
            RGBTitle = "";
            DataSet dsAllColor = GetAllSerialColorRGB();
            if (dsAllColor != null && dsAllColor.Tables.Count > 0 && dsAllColor.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsAllColor.Tables[0].Select(" cs_id='" + csID.ToString() + "' ");
                if (drs != null && drs.Length > 0)
                {
                    // int noPicCount = 1;
                    int hasPicCount = 1;
                    // 子品牌综述页最大色块数
                    int maxSummaryCount = top;
                    foreach (DataRow dr in drs)
                    {
                        // 色块 子品牌综述页不能多于11个
                        // 子品牌
                        if (hasPicCount > maxSummaryCount)
                        { break; }
                        if (colorNames.Contains(dr["colorName"].ToString().Trim()))
                        {
                            // 在产车型颜色名中有此颜色名
                            sbTitle.Append(dr["colorName"].ToString().Trim() + " ");
                            if (!rgb.Contains(dr["colorRGB"].ToString().Trim()))
                            {
                                rgb.Add(dr["colorRGB"].ToString().Trim());
                                // 增加颜色图片呈现
                                // 遍历子品牌所有颜色名与当前RGB相同的所有中文名
                                bool isHasColorPic = false;
                                StringBuilder temp = new StringBuilder();
                                foreach (DataRow drSub in drs)
                                {
                                    if (drSub["colorRGB"].ToString().Trim() == dr["colorRGB"].ToString().Trim())
                                    {
                                        if (dic.ContainsKey(drSub["colorName"].ToString().Trim()))
                                        {
                                            // 有颜色图片对应
                                            if (dic[drSub["colorName"].ToString().Trim()].Attributes["Link"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() != "" && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() != "")
                                            {
                                                temp.AppendLine("<a target=\"_blank\" title=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() + "\" href=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() + "\"><span style=\"background:" + dr["colorRGB"].ToString().Trim() + "\"></span></a>");
                                            }

                                            temp.AppendLine("<div class=\"popColor2\" id=\"" + idSubFix + "color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "\" style=\"display:none\">");
                                            temp.AppendLine("<div class=\"popColorinner\"><p>");
                                            if (dic[drSub["colorName"].ToString().Trim()].Attributes["Link"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() != "" && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"].Value.ToString() != "")
                                            {
                                                temp.AppendLine("<a target=\"_blank\" href=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() + "\">");
                                                temp.AppendLine("<img src=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["ImageUrl"].Value.ToString().Replace("_5.", "_" + subfix.ToString() + ".") + "\" alt=\"\" />");
                                                temp.AppendLine("</a>");
                                            }
                                            temp.AppendLine("</p>");
                                            if (dic[drSub["colorName"].ToString().Trim()].Attributes["Link"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() != "" && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"] != null && dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() != "")
                                            {
                                                temp.AppendLine("<a target=\"_blank\" href=\"" + dic[drSub["colorName"].ToString().Trim()].Attributes["Link"].Value.ToString() + "\">" + dic[drSub["colorName"].ToString().Trim()].Attributes["ImageName"].Value.ToString() + "</a>");
                                            }
                                            temp.AppendLine("</div>");
                                            temp.AppendLine("</div>");
                                            // 子品牌
                                            if (hasPicCount <= maxSummaryCount)
                                            {
                                                //sbHTML.Append("<li onmouseout=\"BtHide('" + idSubFix + "color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "');this.className=''\" onmouseover=\"BtShow('" + idSubFix + "color_" + dr["colorRGB"].ToString().Trim().Replace("#", "") + "');this.className='current'\" class=\"\"  >");
                                                sbHTML.Append("<li>");
                                                sbHTML.Append(temp.ToString() + "</li>");
                                            }
                                            isHasColorPic = true;
                                            break;
                                        }
                                    }
                                }

                                // 子品牌
                                if (!isHasColorPic && hasPicCount <= maxSummaryCount)
                                {
                                    //车款综述页1200改版，车身颜色块由长方形变为圆形   2016-10-9
                                    sbHTML.Append("<li><a href=\"javascript:void(0);\" title=\"" + dr["colorName"].ToString().Trim() + "\"><span style=\"background:" + dr["colorRGB"].ToString().Trim() + "\"></span></a></li>");
                                    //下面一行为原版长方形块,注释掉   2016-10-9
                                    //sbHTML.Append("<em title=\"" + dr["colorName"].ToString().Trim() + "\" ><span style=\"background:" + dr["colorRGB"].ToString().Trim() + "\"></span></em>");
                                }

                                hasPicCount++;
                            }
                        }

                    }
                }
            }

            if (sbHTML.Length > 0)
            {
                RGBHtml = sbHTML.ToString();
            }
            if (sbTitle.Length > 0 && dic.Count <= 0)
            {
                RGBTitle = sbTitle.ToString();
            }
            if (RGBHtml == "" && RGBTitle != "")
            {
                RGBHtml = RGBTitle;
            }
        }
        #endregion

        #region 子品牌口碑和答疑数量
        /// <summary>
        /// 得到子品牌答疑数量
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, XmlElement> GetSerialAskNumber()
        {
            string cacheKey = "SerialAskNumberDictionaryObject";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (Dictionary<int, XmlElement>)obj;

            string filePath = Path.Combine(WebConfig.DataBlockPath, _SerialAskNumber);
            if (!File.Exists(filePath)) return null;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
                XmlNodeList xNodeList = xmlDoc.SelectNodes("data/master/brand/serial");
                if (xNodeList == null || xNodeList.Count < 1) return null;
                Dictionary<int, XmlElement> serialAskList = new Dictionary<int, XmlElement>();

                foreach (XmlElement xElem in xNodeList)
                {
                    int serialId = ConvertHelper.GetInteger(xElem.GetAttribute("id"));
                    serialAskList.Add(serialId, xElem);
                }
                CacheDependency cdep = new CacheDependency(filePath);
                CacheManager.InsertCache(cacheKey, serialAskList, cdep, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                return serialAskList;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到子品牌口碑的数量
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, XmlElement> GetSerialKoubeiNumber()
        {
            string cacheKey = "SerialKouBeiNumberDictionaryObject";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (Dictionary<int, XmlElement>)obj;

            string filePath = Path.Combine(WebConfig.DataBlockPath, _SerialKouBeiNumber);
            if (!File.Exists(filePath)) return null;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
                XmlNodeList xNodeList = xmlDoc.SelectNodes("data/serial");
                if (xNodeList == null || xNodeList.Count < 1) return null;
                Dictionary<int, XmlElement> serialAskList = new Dictionary<int, XmlElement>();

                foreach (XmlElement xElem in xNodeList)
                {
                    int serialId = ConvertHelper.GetInteger(xElem.GetAttribute("id"));
                    serialAskList.Add(serialId, xElem);
                }
                CacheDependency cdep = new CacheDependency(filePath);
                CacheManager.InsertCache(cacheKey, serialAskList, cdep, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                return serialAskList;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到子品牌图片数量
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, XmlElement> GetSerialImageNumber()
        {
            string cacheKey = "SerialImageNumberDictionaryObject";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (Dictionary<int, XmlElement>)obj;

            string filePath = Path.Combine(WebConfig.DataBlockPath, _SerialImageNumber);
            if (!File.Exists(filePath)) return null;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
                XmlNodeList xNodeList = xmlDoc.SelectNodes("Params/MasterBrand/Brand/Serial");
                if (xNodeList == null || xNodeList.Count < 1) return null;
                Dictionary<int, XmlElement> serialAskList = new Dictionary<int, XmlElement>();

                foreach (XmlElement xElem in xNodeList)
                {
                    int serialId = ConvertHelper.GetInteger(xElem.GetAttribute("ID"));
                    serialAskList.Add(serialId, xElem);
                }
                CacheDependency cdep = new CacheDependency(filePath);
                CacheManager.InsertCache(cacheKey, serialAskList, cdep, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                return serialAskList;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region 级联下拉列表
        /// <summary>
        /// 得到符合条件的数据集
        /// </summary>
        /// <param name="type">  * 0:包含在销，待销，停销，待查；旗下是否有车型不限
        /// * 1:包含在销，待销，停销，待查；非概念车；旗下是否有车型不限；
        /// * 2:包含在销，待销；非概念车；旗下是否有车型不限；
        /// * 3:包含在销，待销；非概念车；旗下必须有车型；车型可以是不限销焦状态；
        /// * 4:包含在销，待销；非概念车；旗下必须有车型；车型必须是在销、待销；</param>
        /// * 5:包含：在销，待销，停销，待查，非概念车，旗下必须有车款；
        /// <returns></returns>
        public DataSet GetIsConditionsDataSet(int type)
        {
            int conditions = 0;
            switch (type)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 5:
                case 6:
                case 7:
                case 8:
                    conditions = type;
                    break;
                default:
                    conditions = 4;
                    break;
            }
            string cacheKey = "Car_SerialBll_GetIsConditionsDataSet_" + conditions.ToString();
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (DataSet)obj;

            DataSet ds = csd.GetIsConditionsDataSet(conditions);
            if (ds == null) return ds;
            CacheManager.InsertCache(cacheKey, ds, 60);
            return ds;
        }
        /// <summary>
        /// 得到符合条件厂商到子品牌的数据集
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet GetProduceIsConditionsDataSet(int type)
        {
            int conditions = 0;
            switch (type)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 5:
                    conditions = type;
                    break;
                default:
                    conditions = 4;
                    break;
            }
            string cacheKey = "Car_SerialBll_GetProduceIsConditionsDataSet_" + conditions.ToString();
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (DataSet)obj;

            DataSet ds = csd.GetProduceIsConditionsDataSet(conditions);
            if (ds == null) return ds;
            CacheManager.InsertCache(cacheKey, ds, 60);
            return ds;
        }

        #endregion

        #region 子品牌保养信息
        /// <summary>
        /// 得到保养信息内容
        /// </summary>
        /// <param name="csid">子品牌ID</param>
        /// <returns></returns>
        public string GetMaintanceContent(int csid)
        {
            if (csid < 1) return "";
            string cacheKey = "SerialBll_GetMaintanceContent_" + csid;
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                return (string)obj;
            }
            string filePath = Path.Combine(WebConfig.DataBlockPath, string.Format(_SerialMaintanceMessage, csid));
            try
            {
                if (!File.Exists(filePath)) return "";
                StringBuilder contentString = new StringBuilder();
                using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        contentString.Append(sr.ReadLine());
                    }
                }

                string content = contentString.ToString();
                if (string.IsNullOrEmpty(content)) return "";
                CacheDependency cd = new CacheDependency(filePath);
                CacheManager.InsertCache(cacheKey, content, cd, DateTime.Now.AddMinutes(60));
                return content;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 得到保养信息报价内容
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public string GetMaintancePriceContent(int csid)
        {
            if (csid < 1) return "";
            string cacheKey = "SerialBll_GetMaintancePriceContent_" + csid;
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                return (string)obj;
            }
            string filePath = Path.Combine(WebConfig.DataBlockPath, string.Format(_SerailMaintancePrice, csid));
            try
            {
                if (!File.Exists(filePath)) return "";
                StringBuilder contentString = new StringBuilder();
                using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        contentString.Append(sr.ReadLine());
                    }
                }

                string content = contentString.ToString();
                if (string.IsNullOrEmpty(content)) return "";
                CacheDependency cd = new CacheDependency(filePath);
                CacheManager.InsertCache(cacheKey, content, cd, DateTime.Now.AddMinutes(60));
                return content;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 得到保养新闻列表
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public List<XmlNode> GetMaintanceNewsContent(int csid, int count)
        {
            if (csid < 1 || count < 1) return null;

            string cacheKey = "SerialBll_GetMaintanceNewsContent_" + csid + "_" + count;
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                return (List<XmlNode>)obj;
            }
            string filePath = Path.Combine(WebConfig.DataBlockPath, string.Format(_SerialMaintanceNews, csid));
            if (!File.Exists(filePath)) return null;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
                XmlNodeList xNodeList = xmlDoc.SelectNodes("NewDataSet/listNews");
                if (xNodeList == null || xNodeList.Count < 1) return null;
                List<XmlNode> newsList = new List<XmlNode>();
                for (int i = 0; i < count && i < xNodeList.Count; i++)
                {
                    XmlNode xNode = xNodeList[i].SelectSingleNode("filepath");
                    if (xNode == null) continue;
                    newsList.Add(xNodeList[i]);
                }
                CacheDependency cd = new CacheDependency(filePath);
                CacheManager.InsertCache(cacheKey, newsList, cd, DateTime.Now.AddMinutes(60));
                return newsList;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 判断此子品牌是否存在养护信息
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public bool IsExitsMaintanceMessage(int csid)
        {
            Dictionary<int, int> exitsList = new Dictionary<int, int>();
            string cacheKey = "Car_SerialBll_IsExitsMaintanceMessage";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                exitsList = (Dictionary<int, int>)obj;
                if (exitsList != null && exitsList.ContainsKey(csid)) return true;
                else return false;
            }
            string directoryPath = Path.Combine(WebConfig.DataBlockPath, _SerialMaintanceMassageDirectory);
            if (!Directory.Exists(directoryPath)) return false;

            string[] fileNameList = Directory.GetFileSystemEntries(directoryPath);
            Dictionary<int, int> serialIdList = new Dictionary<int, int>();
            foreach (string filename in fileNameList)
            {
                int startIndex = filename.LastIndexOf('\\') + 1;
                int ilength = filename.LastIndexOf('.');
                string idString = filename.Substring(startIndex, ilength - startIndex);
                int csId = ConvertHelper.GetInteger(idString);
                if (csId < 1) continue;
                if (!serialIdList.ContainsKey(csId))
                {
                    serialIdList.Add(csId, 0);
                }
            }
            CacheManager.InsertCache(cacheKey, serialIdList, 60);
            return serialIdList.ContainsKey(csid);
        }

        /// <summary>
        /// 判断此子品牌是否存在安全文章
        /// </summary>
        /// <param name="csId"></param>
        /// <returns></returns>
        public bool IsExistedAnquanNews(int csId)
        {
            // 			string cacheKey = "Serial_Exist_Anqua_news";
            // 			Dictionary<int, int> aqNewsDic = (Dictionary<int, int>)CacheManager.GetCachedData(cacheKey);
            // 			if(aqNewsDic == null)
            // 			{
            // 				aqNewsDic = new Dictionary<int, int>();
            // 				string numFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\newsNum.xml");
            // 				if(File.Exists(numFile))
            // 				{
            // 					XmlDocument numDoc = new XmlDocument();
            // 					numDoc.Load(numFile);
            // 					XmlNodeList serialNodeList = numDoc.SelectNodes("/SerilaList/Serial");
            // 					foreach(XmlElement serialNode in serialNodeList)
            // 					{
            // 						int serialId = ConvertHelper.GetInteger(serialNode.GetAttribute("id"));
            // 						int aqNum = ConvertHelper.GetInteger(serialNode.GetAttribute("anquan"));
            // 						if (aqNum > 0)
            // 							aqNewsDic[serialId] = aqNum;
            // 					}
            // 
            // 					CacheManager.InsertCache(cacheKey, aqNewsDic, WebConfig.CachedDuration);
            // 				}
            // 			}
            // 
            // 			if (aqNewsDic.ContainsKey(csId))
            // 				return true;
            // 			else
            // 				return false;

            TreeData kejiData = new TreeFactory().GetTreeDataObject("anquan");
            int newsNum = kejiData.GetSerialId(csId);
            return newsNum > 0;
        }

        /// <summary>
        /// 判断此子品牌是否存在科技文章
        /// </summary>
        /// <param name="csId"></param>
        /// <returns></returns>
        public bool IsExistedKejiNews(int csId)
        {
            TreeData kejiData = new TreeFactory().GetTreeDataObject("keji");
            int newsNum = kejiData.GetSerialId(csId);
            return newsNum > 0;

        }
        #endregion

        #region 子品牌上市时间排序

        /// <summary>
        /// 取子品牌上市时间排序列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllSerialMarkDayList()
        {
            List<int> lCsmd = new List<int>();
            DataSet ds = new DataSet();
            string cacheKey = "Car_SerialBll_GetAllSerialMarkDayList";
            object getAllSerialMarkDayList = CacheManager.GetCachedData(cacheKey);
            if (getAllSerialMarkDayList != null)
            {
                ds = (DataSet)getAllSerialMarkDayList;
            }
            else
            {
                ds = new Car_SerialDal().GetAllSerialMarkData();
                CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (!lCsmd.Contains(int.Parse(dr["cs_id"].ToString())))
                    {
                        lCsmd.Add(int.Parse(dr["cs_id"].ToString()));
                    }
                }
            }

            return lCsmd;
        }

        /// <summary>
        /// 取子品牌上市时间
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllSerialMarkDay()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string cacheKey = "Car_SerialBll_GetAllSerialMarkDay";
            object getAllSerialMarkDay = CacheManager.GetCachedData(cacheKey);
            if (getAllSerialMarkDay != null)
            {
                dic = (Dictionary<int, string>)getAllSerialMarkDay;
            }
            else
            {
                // 所有车型的上市时间
                DataSet ds = new Car_SerialDal().GetAllSerialMarkData();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int csid = 0, year = 0, month = 0, day = 0;
                        if (int.TryParse(dr["cs_id"].ToString(), out csid)
                            && int.TryParse(dr["mdyear"].ToString(), out year)
                            && int.TryParse(dr["mdmonth"].ToString(), out month)
                            && int.TryParse(dr["mdday"].ToString(), out day))
                        {
                            if (csid > 0 && year > 0 && month > 0 && day > 0 && !dic.ContainsKey(csid))
                            {
                                dic.Add(csid, year + "-" + month + "-" + day);
                            }
                        }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
            }
            return dic;
        }

        /// <summary>
        /// 取车款上市时间
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllCarMarkDay()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string cacheKey = "Car_SerialBll_GetAllCarMarkDay";
            object getAllCarMarkDay = CacheManager.GetCachedData(cacheKey);
            if (getAllCarMarkDay != null)
            {
                dic = (Dictionary<int, string>)getAllCarMarkDay;
            }
            else
            {
                // 所有车型的上市时间
                DataSet ds = new Car_SerialDal().GetAllSerialMarkData();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int carId = 0, year = 0, month = 0, day = 0;
                        if (int.TryParse(dr["car_id"].ToString(), out carId)
                            && int.TryParse(dr["mdyear"].ToString(), out year)
                            && int.TryParse(dr["mdmonth"].ToString(), out month)
                            && int.TryParse(dr["mdday"].ToString(), out day))
                        {
                            if (carId > 0 && year > 0 && month > 0 && day > 0 && !dic.ContainsKey(carId))
                            {
                                dic.Add(carId, year + "-" + month + "-" + day);
                            }
                        }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
            }
            return dic;
        }

        /// <summary>
        /// 获取所有待销子品牌车型上市时间
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, SeralMarketDay> GetAllWaitSaleSerialMarkDay()
        {
            Dictionary<int, SeralMarketDay> dict = new Dictionary<int, SeralMarketDay>();
            string cacheKey = "Car_SerialBll_GetAllWaitSaleSerialMarkDay";
            object waitsaleSerialMarkDay = CacheManager.GetCachedData(cacheKey);
            if (waitsaleSerialMarkDay != null)
            {
                return (Dictionary<int, SeralMarketDay>)waitsaleSerialMarkDay;
            }

            DataSet ds = csd.GetAllWaitSaleSerialMarkData();
            if (!(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0))
                return dict;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int csid = ConvertHelper.GetInteger(dr["cs_id"]);
                int year = ConvertHelper.GetInteger(dr["mdyear"]);
                int month = ConvertHelper.GetInteger(dr["mdmonth"]);
                int day = ConvertHelper.GetInteger(dr["mdday"]);
                if (csid > 0 && !dict.ContainsKey(csid))
                {
                    if (year > 0 && month > 0 && day > 0)
                        dict.Add(csid, new SeralMarketDay() { Sign = "d", Day = year + "-" + month + "-" + day });
                    else if (year > 0 && month > 0)
                        dict.Add(csid, new SeralMarketDay() { Sign = "m", Day = year + "-" + month });
                    else if (year > 0)
                        dict.Add(csid, new SeralMarketDay() { Sign = "y", Day = year + "-1" });
                }
            }
            CacheManager.InsertCache(cacheKey, dict, WebConfig.CachedDuration);
            return dict;
        }

        /// <summary>
        /// 获取新车上市文本
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="csid"></param>
        /// <returns></returns>
        public string GetNewSerialIntoMarketText(int csId, bool isShowDate = true)
        {
            string cacheKey = "Car_SerialBll_GetNewCarIntoMarketText_" + csId + "_" + isShowDate;
            object cacheValue = CacheManager.GetCachedData(cacheKey);
            if (cacheValue != null)
            {
                return cacheValue.ToString();
            }

            string showText = string.Empty;
            SerialEntity se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csId);
            Dictionary<int, string> carIntoMarketTimeDic = GetAllSerialMarkDay();
            List<CarInfoForSerialSummaryEntity> carList = new Car_BasicBll().GetCarInfoForSerialSummaryBySerialId(csId);
            carList = carList.OrderByDescending(x => x.MarketDateTime).ThenByDescending(x => x.CarID).ToList();
            if (se.SaleState.Trim() == "在销" || se.SaleState.Trim() == "停销")
            {
                var newCarList = carList.Where(x => x.SaleState == "待销");
                if (newCarList.Count() > 0)
                {
                    var newCarMarketDateTimeList = newCarList.Where(a => DateTime.Compare(a.MarketDateTime, DateTime.MinValue) != 0);
                    //存在填写了上市时间的待销车款
                    if (newCarMarketDateTimeList.Count() > 0)
                    {
                        CarInfoForSerialSummaryEntity car = newCarMarketDateTimeList.First();//从已经填写的时间中选择最早的时间
                        //排除未上市车填写了过去的上市时间（这种情况属于数据错误，通过程序筛选控制）
                        if (DateTime.Compare(car.MarketDateTime, DateTime.Now.Date) >= 0)
                        {
                            if (isShowDate)
                            {
                                showText = "将于" + car.MarketDateTime.ToString("yy年MM月dd日") + "上市";
                            }
                            else
                            {
                                showText = "即将上市";
                            }
                        }
                        else
                        {
                            //判断车款是否有实拍图
                            int count = 0;
                            foreach (var item in newCarList)
                            {
                                XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(Path.Combine(PhotoImageConfig.SavePath, string.Format(@"SerialCarReallyPic\{0}.xml", item.CarID)));

                                if (xmlDoc != null && xmlDoc.HasChildNodes)
                                {
                                    XmlNode node = xmlDoc.SelectSingleNode("//Data//Total");
                                    var countStr = node.InnerText;
                                    int.TryParse(countStr, out count);
                                    if (count > 0)
                                    {
                                        showText = "新款即将上市";
                                        break;
                                    }
                                    else
                                    {
                                        //是否有指导价
                                        if (item.ReferPrice != "")
                                        {
                                            showText = "新款即将上市";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var newCarMarketDateTimeList = carList.Where(a => DateTime.Compare(a.MarketDateTime, DateTime.MinValue) != 0);
                    if (newCarMarketDateTimeList.Count() > 0)
                    {
                        var car = newCarMarketDateTimeList.First();//倒叙排列，取第一个即可
                        var carOld = carList.Last();
                        if (car != null)
                        {
                            int days = (DateTime.Now - car.MarketDateTime).Days;
                            int daysOld = -1;
                            if (carOld != null)
                            {
                                daysOld = (DateTime.Now - carOld.MarketDateTime).Days;
                            }
                            if (days >= 0 && days <= 30)
                            {
                                //只有一个年款    ***新车上市***
                                if (carList.GroupBy(i => i.CarYear).Count() == 1 && daysOld >= 0 && daysOld <= 30)
                                {
                                    showText = "新车上市";
                                }
                                //不止一个年款    ***新款上市***
                                else
                                {
                                    showText = "新款上市";
                                }
                            }
                        }
                    }
                }
            }
            //待查 待销(未上市)
            else
            {
                //筛选填写了上市时间的待销车
                var newCarList = carList.Where(a => DateTime.Compare(a.MarketDateTime, DateTime.MinValue) != 0);
                //存在填写了上市时间的待销车
                if (newCarList.Count() > 0)
                {
                    var car = carList.First();//从已经填写的时间中选择最早的时间
                    //排除未上市车填写了过去的上市时间（这种情况属于数据错误，通过程序筛选控制）
                    if (DateTime.Compare(car.MarketDateTime, DateTime.Now.Date) >= 0)
                    {
                        if (isShowDate)
                        {
                            showText = "将于" + car.MarketDateTime.ToString("yy年MM月dd日") + "上市";
                        }
                        else
                        {
                            showText = "即将上市";
                        }
                    }
                }
                //没有上市时间，判断有没有实拍图、指导价
                else
                {
                    //查找实拍图
                    int count = 0;
                    foreach (var item in carList)
                    {
                        XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(Path.Combine(PhotoImageConfig.SavePath, string.Format(@"SerialCarReallyPic\{0}.xml", item.CarID)));
                        if (xmlDoc != null && xmlDoc.HasChildNodes)
                        {
                            XmlNode node = xmlDoc.SelectSingleNode("//Data//Total");
                            var countStr = node.InnerText;
                            int.TryParse(countStr, out count);
                            if (count > 0)
                            {
                                showText = "即将上市";
                                break;
                            }
                            //是否有指导价
                            else
                            {
                                //是否有指导价
                                if (item.ReferPrice != "")
                                {
                                    showText = "即将上市";
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            CacheManager.InsertCache(cacheKey, showText, WebConfig.CachedDuration);
            return showText;
        }

        /// <summary>
        /// 获取新上市车款文本
        /// </summary>
        /// <returns></returns>
        public string GetCarMarketText(int carId)
        {
            if (carId == 0) return string.Empty;
            CarEntity car = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
            if (car == null) return string.Empty;
            Dictionary<int, string> carMarketDic = GetAllCarMarkDay();
            DateTime marketDate = DateTime.MinValue;
            if (carMarketDic.ContainsKey(carId))
            {
                DateTime.TryParse(carMarketDic[carId], out marketDate);
            }
            return GetCarMarketText(carId, car.SaleState, marketDate, car.ReferPrice.ToString());
        }

        /// <summary>
        /// 获取新上市车款文本
        /// </summary>
        /// <param name="carId">车款id</param>
        /// <param name="saleSate">销售状态</param>
        /// <param name="marketDate">上市时间</param>
        /// <param name="referPrice">指导价</param>
        /// <returns></returns>
        public string GetCarMarketText(int carId, string saleSate, DateTime marketDate, string referPrice)
        {
            string marketflag = string.Empty;

            if (carId == 0) return marketflag;

            //int res =DateTime.Compare(entity.MarketDateTime, DateTime.MinValue);
            if (DateTime.Compare(marketDate, DateTime.MinValue) != 0)
            {
                int days = (DateTime.Now.Date - marketDate).Days;// GetDaysAboutCurrentDateTime(entity.MarketDateTime);
                if (days >= 0 && days <= 30)
                {
                    if (saleSate.Trim() == "在销")
                    {
                        marketflag = "新上市";
                    }
                }
                else if (days >= -30 && days < 0)
                {
                    if (saleSate.Trim() == "待销")
                    {
                        marketflag = "即将上市";
                    }
                }
            }
            else
            {
                if (saleSate.Trim() == "待销")
                {
                    var picCount = new Car_BasicBll().GetSerialCarRellyPicCount(carId);
                    if (picCount > 0)
                    {
                        marketflag = "即将上市";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(referPrice))
                        {
                            marketflag = "即将上市";
                        }
                    }
                }
            }
            return marketflag;
        }
        #endregion

        #region 子品牌厂商指导价

        /// <summary>
        /// 根据销售状态取子品牌厂商指导价
        /// </summary>
        /// <param name="csid">子品牌ID</param>
        /// <param name="isAllSale">是否全销售状态</param>
        /// <returns></returns>
        public string GetSerialOfficePriceBySaleState(int csid, bool isAllSale)
        {
            string op = "";
            DataSet SaleData = new DataSet();
            string cacheKey = "GetSerialOfficePriceBySaleState_" + isAllSale.ToString();
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                SaleData = (DataSet)obj;
            }
            else
            {
                SaleData = csd.GetAllSerialOfficePrice(isAllSale);
                CacheManager.InsertCache(cacheKey, SaleData, WebConfig.CachedDuration);
            }

            if (SaleData != null && SaleData.Tables.Count > 0 && SaleData.Tables[0].Rows.Count > 0)
            {
                DataRow[] dr = SaleData.Tables[0].Select("cs_id = '" + csid.ToString() + "'");
                if (dr != null && dr.Length > 0)
                {

                    if (dr[0].IsNull("minprice"))
                    {
                        op = "0";
                    }
                    else if (ConvertHelper.GetDecimal(dr[0]["minprice"]) == ConvertHelper.GetDecimal(dr[0]["maxprice"]))
                    {
                        op = dr[0]["minprice"].ToString() + "万";
                    }
                    else
                    {
                        op = dr[0]["minprice"].ToString() + "万" + "-" + dr[0]["maxprice"].ToString() + "万";
                    }
                }
            }
            return op;
        }

        /// <summary>
        /// 根据销售状态取所有子品牌厂商指导价
        /// </summary>
        /// <param name="isAllSale">是否全销售状态</param>
        /// <returns></returns>
        public Dictionary<int, string> GetAllSerialOfficePriceBySaleState(bool isAllSale)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string cacheKey = "GetAllSerialOfficePriceBySaleState" + isAllSale.ToString();
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                dic = (Dictionary<int, string>)obj;
            }
            else
            {
                DataSet ds = new DataSet();
                ds = csd.GetAllSerialOfficePrice(isAllSale);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int csid = int.Parse(dr["cs_id"].ToString());
                        string op = "";
                        if (dr.IsNull("minprice"))
                        {
                            op = "";
                        }
                        else if (ConvertHelper.GetDecimal(dr["minprice"]) == ConvertHelper.GetDecimal(dr["maxprice"]))
                        {
                            op = dr["minprice"].ToString() + "万";
                        }
                        else
                        {
                            op = dr["minprice"].ToString() + "万" + "-" + dr["maxprice"].ToString() + "万";
                        }
                        if (!dic.ContainsKey(csid))
                        { dic.Add(csid, op); }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
            }
            return dic;
        }

        #endregion

        #region 树形行情子品牌根据城市按照UV排序
        /// <summary>
        /// 得到子品牌UV的排序通过城市ID
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public DataSet GetSerialUVOrderByCityId(int cityId)
        {
            string cache = "car_serialbll_GetSerialUVOrderByCityId_" + cityId.ToString();
            DataSet ds = CacheManager.GetCachedData(cache) as DataSet;
            if (ds != null) return ds;
            ds = csd.GetSerialUVOrderByCityId(cityId);
            if (ds == null)
                ds = new DataSet();
            CacheManager.InsertCache(cache, ds, 10);
            return ds;
        }

        #endregion

        #region 图库子品牌接口

        /// <summary>
        /// 取年款焦点图
        /// </summary>
        /// <param name="csid"></param>
        /// <param name="year"></param>
        /// <param name="subfix"></param>
        /// <returns></returns>
        public XmlDocument GetSerialYearPhoto(int csid, int year, int subfix)
        {
            XmlDocument doc = new XmlDocument();
            //图库接口本地化更改 by sk 2012.12.21
            string xmlFile = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialYearFocusImagePath);
            if (File.Exists(string.Format(xmlFile, csid, year)))
                doc.Load(string.Format(xmlFile, csid, year));
            //doc.Load(WebConfig.PhotoSerialInterface + "?dataname=serialfocusimage&serialid=" + csid.ToString() + "&year=" + year.ToString() + "&showall=false&showfullurl=true&subfix=" + subfix.ToString());
            return doc;
        }

        /// <summary>
        /// 取子品牌年款默认车型字典 key: 子品牌ID_年款
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetSerialYearDefaultCarDictionary()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string cacheKey = "GetSerialYearDefaultCarDictionary";
            object getSerialYearDefaultCarDictionary = CacheManager.GetCachedData(cacheKey);
            if (getSerialYearDefaultCarDictionary != null)
            {
                dic = (Dictionary<string, string>)getSerialYearDefaultCarDictionary;
            }
            else
            {
                XmlDocument doc = GetSerialYearDefaultCar();
                if (doc != null && doc.HasChildNodes)
                {
                    XmlNodeList xnl = doc.SelectNodes("ImageData/CarYearList/CarYear");
                    if (xnl != null && xnl.Count > 0)
                    {
                        foreach (XmlNode xn in xnl)
                        {
                            string key = xn.Attributes["SerialId"].Value.ToString() + "_" + xn.Attributes["Year"].Value.ToString();
                            if (!dic.ContainsKey(key))
                            {
                                dic.Add(key, xn.Attributes["CarId"].Value.ToString());
                            }
                        }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, 60);
            }
            return dic;
        }

        /// <summary>
        /// 取子品牌年款的默认车型
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetSerialYearDefaultCar()
        {
            XmlDocument doc = new XmlDocument();
            //图库接口本地化更改 by sk 2012.12.21
            string xmlFile = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialDefaultCarPath);
            if (File.Exists(xmlFile))
                doc.Load(xmlFile);
            //doc.Load(WebConfig.PhotoSerialInterface + "?dataname=serialyearcar&showall=false&showfullurl=true&subfix=2");
            return doc;
        }
        /// <summary>
        /// 获取子品牌图片页面图库内容
        /// </summary>
        /// <param name="serialId">子品牌ID</param>
        /// <returns></returns>
        public static string GetSerialPhotoHtml(int serialId)
        {
            string html = string.Empty;
            if (serialId <= 0)
                return html;
            string cacheKey = "Car_SerialBll_GetSerialPhotoHtml_" + serialId;
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                html = ConvertHelper.GetString(obj);
            }
            else
            {
                string photoPath = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialPhotoHtmlPath);
                if (File.Exists(string.Format(photoPath, serialId)))
                {
                    html = File.ReadAllText(string.Format(photoPath, serialId));
                    CacheManager.InsertCache(cacheKey, html, WebConfig.CachedDuration);
                }
            }
            return html;
        }
        /// <summary>
        /// 获取子品牌图片页面图库内容 1200改版 lisf 2016-8-18
        /// </summary>
        /// <param name="serialId">子品牌ID</param>
        /// <returns></returns>
        public static string GetSerialPhotoHtmlNew(int serialId)
        {
            string html = string.Empty;
            if (serialId <= 0)
                return html;
            string cacheKey = "Car_SerialBll_GetSerialPhotoHtmlNew_" + serialId;
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                html = ConvertHelper.GetString(obj);
            }
            else
            {
                string photoPath = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialPhotoHtmlPathNew);
                if (File.Exists(string.Format(photoPath, serialId)))
                {
                    html = File.ReadAllText(string.Format(photoPath, serialId));
                    CacheManager.InsertCache(cacheKey, html, WebConfig.CachedDuration);
                }
            }
            return html;
        }
        /// <summary>
        /// 获取车款页面图库内容
        /// </summary>
        /// <param name="carId">车型ID</param>
        /// <returns></returns>
        public static string GetCarPhotoHtml(int carId)
        {
            string html = string.Empty;
            if (carId <= 0)
                return html;
            string cacheKey = "Car_SerialBll_GetCarPhotoHtml_" + carId;
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                html = ConvertHelper.GetString(obj);
            }
            else
            {
                string photoPath = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.CarPhotoHtmlPath);
                if (File.Exists(string.Format(photoPath, carId)))
                {
                    html = File.ReadAllText(string.Format(photoPath, carId));
                    CacheManager.InsertCache(cacheKey, html, WebConfig.CachedDuration);
                }
            }
            return html;
        }

        /// <summary>
        /// 获取车款页面图库内容 1200改版 lisf 2016-8-18
        /// </summary>
        /// <param name="carId">车型ID</param>
        /// <returns></returns>
        public static string GetCarPhotoHtmlNew(int carId)
        {
            string html = string.Empty;
            if (carId <= 0)
                return html;
            string cacheKey = "Car_SerialBll_GetCarPhotoHtmlNew_" + carId;
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                html = ConvertHelper.GetString(obj);
            }
            else
            {
                string photoPath = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.CarPhotoHtmlPathNew);
                if (File.Exists(string.Format(photoPath, carId)))
                {
                    html = File.ReadAllText(string.Format(photoPath, carId));
                    CacheManager.InsertCache(cacheKey, html, WebConfig.CachedDuration);
                }
            }
            return html;
        }
        /// <summary>
        /// 获取子品牌年款图片页内容 
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string GetSerialYearPhotoHtml(int serialId, int year)
        {
            string html = string.Empty;
            if (serialId <= 0)
                return html;
            string cacheKey = string.Format("Car_SerialBll_GetSerialYearPhotoHtml_{0}_{1}", serialId, year);
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                html = ConvertHelper.GetString(obj);
            }
            else
            {
                string photoPath = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialYearPhotoHtmlPath);
                if (File.Exists(string.Format(photoPath, serialId, year)))
                {
                    html = File.ReadAllText(string.Format(photoPath, serialId, year));
                    CacheManager.InsertCache(cacheKey, html, WebConfig.CachedDuration);
                }
            }
            return html;
        }
        /// <summary>
        /// 获取子品牌年款图片页内容 1200改版 lisf 2016-8-18
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string GetSerialYearPhotoHtmlNew(int serialId, int year)
        {
            string html = string.Empty;
            if (serialId <= 0)
                return html;
            string cacheKey = string.Format("Car_SerialBll_GetSerialYearPhotoHtmlNew_{0}_{1}", serialId, year);
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                html = ConvertHelper.GetString(obj);
            }
            else
            {
                string photoPath = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialYearPhotoHtmlPathNew);
                if (File.Exists(string.Format(photoPath, serialId, year)))
                {
                    html = File.ReadAllText(string.Format(photoPath, serialId, year));
                    CacheManager.InsertCache(cacheKey, html, WebConfig.CachedDuration);
                }
            }
            return html;
        }
        /// <summary>
        /// 图库提供图片页HTML
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public string GetPhotoProvideCateHTML(int csid)
        {
            string html = "";
            if (csid > 0)
            {
                string cacheKey = "Car_SerialBll_GetPhotoProvideCateHTML_" + csid.ToString();
                object obj = CacheManager.GetCachedData(cacheKey);
                if (obj != null)
                {
                    html = Convert.ToString(obj);
                }
                else
                {
                    WebClient wc = new WebClient();
                    wc.Encoding = Encoding.UTF8;
                    //图库接口本地化更改 by sk 2012.12.21
                    string photoPath = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialClassPath);
                    if (File.Exists(string.Format(photoPath, csid.ToString())))
                    {
                        html = wc.DownloadString(string.Format(photoPath, csid.ToString()));
                        //html = wc.DownloadString(string.Format(WebConfig.PhotoProvideCateHTML, csid.ToString()));
                        CacheManager.InsertCache(cacheKey, html, WebConfig.CachedDuration);
                    }
                }
            }
            return html;
        }

        /// <summary>
        /// 取子品牌图片颜色数据(来源图库数据xml)
        /// </summary>
        /// <param name="csid">子品牌ID</param>
        /// <returns></returns>
        public List<EnumCollection.SerialColorItem> GetPhotoSerialCarColorByCsID(int csid)
        {
            List<EnumCollection.SerialColorItem> listESCI = new List<EnumCollection.SerialColorItem>();
            string cacheKey = "Car_SerialBll_GetPhotoSerialCarColorByCsID_" + csid.ToString();
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                listESCI = (List<EnumCollection.SerialColorItem>)obj;
            }
            else
            {
                //图库接口本地化更改 by sk 2013.01.05
                string colorPath = Path.Combine(PhotoImageConfig.SavePath,
                    string.Format(PhotoImageConfig.SerialColorCountPath, csid));
                //string colorPath = string.Format(Path.Combine(WebConfig.PhotoNASDataPath, "carcolor\\{0}.xml"), csid);
                if (File.Exists(colorPath))
                {
                    using (XmlReader xmlReader = XmlReader.Create(colorPath))
                    {
                        while (xmlReader.ReadToFollowing("Params"))
                        {
                            XmlReader inner = xmlReader.ReadSubtree();
                            int carid = 0;
                            int colorid = 0;
                            string colorName = "";
                            string colorRGB = "";
                            while (!inner.EOF)
                            {
                                if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "C")
                                {
                                    carid = 0;
                                    // 车型节点
                                    int.TryParse(inner.GetAttribute("ID"), out carid);
                                }
                                if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "O")
                                {
                                    // 颜色节点
                                    int.TryParse(inner.GetAttribute("ID"), out colorid);
                                    colorName = inner.GetAttribute("Name") == null ? "" : inner.GetAttribute("Name");
                                    colorRGB = inner.GetAttribute("RGB") == null ? "" : inner.GetAttribute("RGB");
                                    if (carid > 0 && colorid > 0 && colorName != "" && colorRGB != "")
                                    {
                                        EnumCollection.SerialColorItem sci = new EnumCollection.SerialColorItem();
                                        sci.CarID = carid;
                                        sci.ColorID = colorid;
                                        sci.ColorName = colorName;
                                        sci.ColorRGB = colorRGB;
                                        if (!listESCI.Contains(sci))
                                        { listESCI.Add(sci); }
                                    }
                                    colorid = 0;
                                    colorName = "";
                                    colorRGB = "";
                                }
                                inner.Read();
                            }
                        }
                    }
                }
                if (listESCI.Count > 0)
                { listESCI.Sort(CompareColorByColorRGB); }
                CacheManager.InsertCache(cacheKey, listESCI, WebConfig.CachedDuration);
            }
            return listESCI;
        }

        /// <summary>
        /// 根据颜色RGB值排序
        /// </summary>
        /// <param name="sci1"></param>
        /// <param name="sci2"></param>
        /// <returns></returns>
        private static int CompareColorByColorRGB(EnumCollection.SerialColorItem sci1, EnumCollection.SerialColorItem sci2)
        {
            if (sci2 == null)
            {
                if (sci1 == null)
                { return 0; }
                else
                { return 1; }
            }
            else
            {
                if (sci1 == null)
                { return -1; }
                else
                { return String.Compare(sci1.ColorRGB, sci2.ColorRGB); }
            }
        }

        #endregion

        #region 子品牌综述页新闻块
        /// <summary>
        /// 得到子品牌综述页新闻块
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public Dictionary<string, List<News>> GetSerialSummaryPageNewsSpan(int serialId)
        {
            if (serialId < 1) return null;
            /*string cacheKey = "Car_SerialBll_GetSerialSummaryPageNewsSpan_" + serialId;
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (Dictionary<string, List<News>>)obj;*/

            Dictionary<string, List<News>> newsList = new Dictionary<string, List<News>>();
            List<int> exitsNewsList = new List<int>();
            //得到焦点新闻
            List<News> focusNewsList = GetFocusNewsList(serialId, exitsNewsList);
            if (focusNewsList != null && focusNewsList.Count > 0)
                newsList.Add("focus", focusNewsList);
            //得到买车必看新闻
            List<News> mustNewsList = GetMustWatchNewsList(serialId, exitsNewsList);
            if (mustNewsList != null && mustNewsList.Count > 0)
                newsList.Add("must", mustNewsList);
            //如果新闻列表不为空，则添加缓存
            /* if (newsList != null && newsList.Count > 0)
				 CacheManager.InsertCache(cacheKey, newsList, WebConfig.CachedDuration);*/

            return newsList;
        }
        /// <summary>
        /// 得到焦点区新闻列表
        /// </summary>
        /// <param name="exitsNewsList">存在的新闻列表</param>
        /// <returns>新闻列表</returns>
        public List<News> GetFocusNewsList(int serialId, List<int> exitsNewsList)
        {
            if (serialId < 1) return null;
            //新闻列表
            Dictionary<int, News> foucsOrder = new Dictionary<int, News>();
            List<News> newsList = new List<News>();
            string focusNewsPath = Path.Combine(WebConfig.DataBlockPath, string.Format(_SerialFocusNews, serialId));

            for (int i = 1; i < 7; i++)
            {
                string newsType = "xinwen";
                if (i == 1)//添加最新新闻
                {
                    GetFocusNewsList(exitsNewsList, focusNewsPath, foucsOrder);
                    continue;
                }
                else if (i == 2)//添加新闻
                {
                    newsType = "xinwen";
                }
                else if (i == 3 || i == 4)//添加导购新闻
                {
                    newsType = "daogou";
                }
                else if (i == 5)//添加评测新闻
                {
                    newsType = "pingce";
                }
                else if (i == 6)//添加试驾新闻
                {
                    newsType = "shijia";
                }
                GetXinWenNewsList(exitsNewsList, Path.Combine(WebConfig.DataBlockPath, string.Format(_SerialNews, serialId, newsType)), foucsOrder, i);
            }
            //如果没有符合要求的新闻
            if (foucsOrder == null || foucsOrder.Count < 1)
            {
                return GetFocusNewsListByNumber(6, exitsNewsList, focusNewsPath);
            }
            //添加新闻到新闻列表
            for (int i = 1; i < 7; i++)
            {
                if (foucsOrder.ContainsKey(i))
                    newsList.Add(foucsOrder[i]);
            }
            //空缺新闻数量
            int vacanciesNews = 6 - newsList.Count;
            if (vacanciesNews < 1) return newsList;
            //得到其他分类的新闻
            List<News> vacanNewsList = GetFocusNewsListByNumber(vacanciesNews, exitsNewsList, focusNewsPath);
            if (vacanNewsList != null || vacanNewsList.Count > 0)
            {
                foreach (News entity in vacanNewsList)
                {
                    newsList.Add(entity);
                }
            }

            return newsList;
        }
        /// <summary>
        /// 得到买车必看新闻列表
        /// </summary>
        /// <param name="exitsNewsList">已经存在的新闻列表</param>
        /// <returns>新闻列表</returns>
        public List<News> GetMustWatchNewsList(int serialId, List<int> exitsNewsList)
        {
            if (serialId < 1) return null;
            //新闻列表
            List<News> newsList = new List<News>();
            Dictionary<int, int> sortList = new Dictionary<int, int>();
            Dictionary<int, News> newsSortList = new Dictionary<int, News>();

            string pingJiaPath = Path.Combine(WebConfig.DataBlockPath, string.Format(_SerialNews, serialId, "pingjia"));
            string focusNewsPath = Path.Combine(WebConfig.DataBlockPath, string.Format(_SerialFocusNews, serialId));
            //如果文件不存在
            if (!File.Exists(pingJiaPath))
            {
                return GetFocusNewsListByNumber(6, exitsNewsList, focusNewsPath);
            }
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(pingJiaPath);
                //得到列表排序
                sortList = NewsChannelBll.GetNewsSortDic((XmlElement)xmlDoc.SelectSingleNode("/root/SortList"));
                if (sortList != null && sortList.Count > 0)
                {
                    foreach (KeyValuePair<int, int> entity in sortList)
                    {
                        XmlNode newsNode = xmlDoc.SelectSingleNode(string.Format("/root/listNews[newsid={0}]", entity.Value));
                        if (newsNode == null) continue;

                        News newsObject = GetNewsObjectByXmlNode(newsNode);
                        //如果新闻列表不存在些结点
                        if (!newsSortList.ContainsKey(entity.Key))
                        {
                            newsSortList.Add(entity.Key, newsObject);
                        }
                    }
                }
                //得到新闻的排序结点
                XmlNodeList xNodeList = xmlDoc.SelectNodes("/root/listNews");
                if (xNodeList == null || xNodeList.Count < 1)
                {
                    return GetFocusNewsListByNumber(6, exitsNewsList, focusNewsPath);
                }
                //获取新闻列表
                for (int i = 1; i < 6; i++)
                {
                    //新闻排序列表
                    if (newsSortList.ContainsKey(i))
                    {
                        newsList.Add(newsSortList[i]);
                        continue;
                    }
                    foreach (XmlElement entity in xNodeList)
                    {
                        int newsId = ConvertHelper.GetInteger(entity.SelectSingleNode("newsid").InnerText);
                        if (exitsNewsList.Contains(newsId)) continue;

                        News newsObject = GetNewsObjectByXmlNode(entity);
                        newsList.Add(newsObject);
                        exitsNewsList.Add(newsId);
                        break;
                    }
                }

                //空缺新闻数量
                int vacanciesNews = 5 - newsList.Count;
                if (vacanciesNews < 1) return newsList;
                //得到其他分类的新闻
                List<News> vacanNewsList = GetFocusNewsListByNumber(vacanciesNews, exitsNewsList, focusNewsPath);
                if (vacanNewsList != null || vacanNewsList.Count > 0)
                {
                    foreach (News entity in vacanNewsList)
                    {
                        newsList.Add(entity);
                    }
                }


                return newsList;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到城市行情新闻
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [Obsolete("新闻服务上线后，将由CarNewsBll.GetTopCityNews方法代替。")]
        public List<News> GetCityHangQingNewsList(int serialId, int cityId)
        {
            return GetCityHangQingNewsList(serialId, cityId, -1);
        }
        /// <summary>
        /// 得到城市行情新闻
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="cityId"></param>
        /// <param name="parentCityId"></param>
        /// <returns></returns>
        [Obsolete("新闻服务上线后，将由CarNewsBll.GetTopCityNews方法代替。")]
        public List<News> GetCityHangQingNewsList(int serialId, int cityId, int parentCityId)
        {
            string filePath = Path.Combine(WebConfig.DataBlockPath, string.Format(_SerialNews, serialId.ToString(), "hangqing"));
            if (!File.Exists(filePath))
            {
                return null;
            }
            //新闻列表
            List<News> newsList = new List<News>(2), parentNewsList = new List<News>(2);

            FileStream stream = null;
            XmlReader reader = null;
            try
            {
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                reader = XmlReader.Create(stream);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "listnews")
                    {
                        GetCityHangQingNewsObjectByXmlReader(reader, cityId, parentCityId, newsList, parentNewsList);
                        if (newsList.Count >= 2)
                            break;
                    }
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (stream != null)
                    stream.Dispose();
            }
            if (newsList.Count > 0)
                newsList.Sort(CompareNewsByPublishTime);
            else if (parentNewsList.Count > 0)
                parentNewsList.Sort(CompareNewsByPublishTime);
            return newsList.Count > 0 ? newsList : parentNewsList;
        }
        /// <summary>
        /// 根据城市与xmlreader对象返回news对象
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [Obsolete("新闻服务上线后，不再使用。")]
        private void GetCityHangQingNewsObjectByXmlReader(XmlReader reader, int cityId, int parentCityId, List<News> cityNewsList, List<News> parentCityNewsList)
        {
            if (reader == null || cityId <= 0)
                return;
            using (XmlReader subReader = reader.ReadSubtree())
            {
                int newsId = -1;
                string title = string.Empty, pageUrl = string.Empty;
                string faceTitle = string.Empty;
                DateTime publishTime = DateTime.Now;
                bool isContainsCityId = false, isContainsParentCityId = false;
                while (subReader.Read())
                {
                    if (subReader.NodeType == XmlNodeType.Element)
                    {
                        switch (subReader.Name.ToLower())
                        {
                            case "newsid":
                                newsId = ConvertHelper.GetInteger(subReader.ReadString());
                                break;
                            case "title":
                                title = subReader.ReadString();
                                break;
                            case "facetitle":
                                faceTitle = subReader.ReadString();
                                break;
                            case "filepath":
                                pageUrl = subReader.ReadString();
                                break;
                            case "publishtime":
                                publishTime = ConvertHelper.GetDateTime(subReader.ReadString());
                                break;
                            case "relatedcityid":
                                string relationCity = subReader.ReadString();
                                if (string.IsNullOrEmpty(relationCity))
                                    continue;
                                relationCity = string.Concat(",", relationCity, ",");
                                if (relationCity.IndexOf(string.Concat(",", cityId.ToString(), ",")) >= 0)
                                {
                                    isContainsCityId = true;
                                }
                                else if (relationCity.IndexOf(string.Concat(",", parentCityId.ToString(), ",")) >= 0)
                                {
                                    isContainsParentCityId = true;
                                }
                                break;
                        }
                    }
                }
                if (isContainsCityId)
                {
                    News cityNews = new News();
                    cityNews.NewsId = newsId;
                    cityNews.Title = title;
                    cityNews.FaceTitle = faceTitle;
                    cityNews.PageUrl = pageUrl;
                    cityNews.PublishTime = publishTime;
                    cityNews.CategoryName = "hangqing";
                    cityNewsList.Add(cityNews);
                }
                else if (isContainsParentCityId && cityNewsList.Count < 1 && parentCityNewsList.Count < 2)
                {
                    News parentCityNews = new News();
                    parentCityNews.NewsId = newsId;
                    parentCityNews.Title = title;
                    parentCityNews.FaceTitle = faceTitle;
                    parentCityNews.PageUrl = pageUrl;
                    parentCityNews.PublishTime = publishTime;
                    parentCityNews.CategoryName = "hangqing";
                    parentCityNewsList.Add(parentCityNews);
                }
            }
        }
        /// <summary>
        /// List<News>列表比较方法，按News对象中的PublishTime属性
        /// </summary>
        /// <param name="news1"></param>
        /// <param name="news2"></param>
        /// <returns></returns>
        private static int CompareNewsByPublishTime(News news1, News news2)
        {
            if (news1 == null)
            {
                if (news2 == null)
                    return 0;
                else
                    return 1;
            }
            else
            {
                if (news2 == null)
                    return -1;
                else
                    return DateTime.Compare(news2.PublishTime, news1.PublishTime);
            }
        }
        /// <summary>
        /// 取指定数量的焦点新闻列表
        /// </summary>
        /// <param name="serialId">子品牌ID</param>
        /// <param name="newsCount">新闻数量</param>
        /// <param name="exitsNewsList">已存在新闻列表</param>
        /// <returns></returns>
        public List<News> GetFocusNewsListByNumber(int newsCount, List<int> exitsNewsList, string filePath)
        {
            if (!File.Exists(filePath) || newsCount < 1) return null;

            try
            {
                XmlTextReader xmlReader = new XmlTextReader(filePath);
                xmlReader.WhitespaceHandling = WhitespaceHandling.None;
                int index = 1;
                List<News> newsList = new List<News>();
                StringBuilder tempXml = new StringBuilder();
                //分析XML文档
                while (xmlReader.ReadToFollowing("listNews"))
                {
                    if (index > newsCount) break;
                    //xmlReader.Skip();
                    //string listnewsContent = xmlReader.ReadInnerXml();
                    XmlReader inner = xmlReader.ReadSubtree();
                    inner.ReadToDescendant("newsid");
                    int id = ConvertHelper.GetInteger(inner.ReadInnerXml());
                    //如果已经存在的新闻不包括当前新闻
                    if (exitsNewsList != null && exitsNewsList.Contains(id))
                    {
                        inner.Close();
                        continue;
                    }
                    string title = string.Empty;
                    string facetitle = string.Empty;
                    string pageUrl = string.Empty;
                    string categoryRoot = string.Empty;
                    string publishTime = string.Empty;
                    while (!inner.EOF)
                    {
                        if (inner.NodeType == XmlNodeType.Element && inner.LocalName.ToLower() == "title")
                        {
                            title = inner.ReadInnerXml();
                            title = new CommonFunction().ToXml(title);
                            continue;
                        }
                        if (inner.NodeType == XmlNodeType.Element && inner.LocalName.ToLower() == "facetitle")
                        {
                            facetitle = inner.ReadInnerXml();
                            facetitle = new CommonFunction().ToXml(facetitle);
                            continue;
                        }
                        if (inner.NodeType == XmlNodeType.Element && inner.LocalName.ToLower() == "filepath")
                        {
                            pageUrl = inner.ReadInnerXml();
                            continue;
                        }
                        if (inner.NodeType == XmlNodeType.Element && inner.LocalName.ToLower() == "publishtime")
                        {
                            publishTime = inner.ReadInnerXml();
                            continue;
                        }
                        if (inner.NodeType == XmlNodeType.Element && inner.LocalName.ToLower() == "categorypath")
                        {
                            categoryRoot = inner.ReadInnerXml();
                            continue;
                        }
                        inner.Read();
                    }

                    News newsObject = new News();
                    newsObject.NewsId = id;
                    newsObject.Title = title;
                    newsObject.FaceTitle = facetitle;
                    newsObject.PageUrl = pageUrl;
                    newsObject.PublishTime = Convert.ToDateTime(publishTime);
                    newsObject.CategoryName = Car_SerialBll.GetNewsKind(string.IsNullOrEmpty(categoryRoot) ? "" : categoryRoot);
                    //增加索引指向
                    index++;
                    inner.Close();
                    newsList.Add(newsObject);
                    exitsNewsList.Add(newsObject.NewsId);

                }
                //关闭流
                xmlReader.Close();
                return newsList;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到焦点区新闻
        /// </summary>
        /// <param name="exitsNewsList"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private void GetFocusNewsList(List<int> exitsNewsList, string filePath, Dictionary<int, News> newsList)
        {
            if (!File.Exists(filePath)) return;

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(filePath);
                if (xmlDoc == null) return;
                Dictionary<int, int> sortDic = NewsChannelBll.GetNewsSortDic((XmlElement)xmlDoc.SelectSingleNode("/root/FocusNews/SortList"));
                if (sortDic != null && sortDic.Count > 0)
                {
                    foreach (KeyValuePair<int, int> entity in sortDic)
                    {
                        XmlNode newsNode = xmlDoc.SelectSingleNode(string.Format("/root/FocusNews/listNews[newsid={0}]", entity.Value));
                        if (newsNode == null) continue;

                        News newsObject = GetNewsObjectByXmlNode(newsNode);
                        //如果新闻列表不存在些结点
                        if (!newsList.ContainsKey(entity.Key) && !exitsNewsList.Contains(newsObject.NewsId))
                        {
                            newsList.Add(entity.Key, newsObject);
                            exitsNewsList.Add(newsObject.NewsId);
                        }
                    }
                }
                //如果在顺序1中没有新闻，则添加最新的焦点新闻
                if (newsList == null || newsList.Count < 1 || !newsList.ContainsKey(1))
                {
                    XmlNodeList xNodeList = xmlDoc.SelectNodes("/root/FocusNews/listNews");
                    if (xNodeList == null || xNodeList.Count < 1) return;

                    foreach (XmlNode entity in xNodeList)
                    {
                        int id = ConvertHelper.GetInteger(entity.SelectSingleNode("newsid").InnerText);
                        if (exitsNewsList.Contains(id)) continue;
                        //添加新闻列表
                        News newsObject = GetNewsObjectByXmlNode(xNodeList[0]);
                        newsList.Add(1, newsObject);
                        exitsNewsList.Add(id);
                        break;
                    }
                    return;
                }
                //添加已经存在的新闻列表
                exitsNewsList.Add(newsList[1].NewsId);
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 易车网首页接口提取焦点新闻
        /// </summary>
        /// <param name="newsCount">3条</param>
        /// <param name="filePath">Data\\SerialNews\\FocusNews\\homexml\\{0}.xml</param>
        /// <returns></returns>
        public List<News> GetFocusNewsListForCMS(int newsCount, string filePath)
        {
            if (!File.Exists(filePath) || newsCount < 1) return null;
            try
            {
                XmlTextReader xmlReader = new XmlTextReader(filePath);
                xmlReader.WhitespaceHandling = WhitespaceHandling.None;
                int index = 1;
                List<News> newsList = new List<News>();
                StringBuilder tempXml = new StringBuilder();
                //分析XML文档
                while (xmlReader.ReadToFollowing("listNews"))
                {
                    if (index > newsCount) break;
                    XmlReader inner = xmlReader.ReadSubtree();
                    // inner.ReadToDescendant("newsid");
                    // int id = ConvertHelper.GetInteger(inner.ReadInnerXml());
                    string title = string.Empty;
                    string facetitle = string.Empty;
                    string pageUrl = string.Empty;
                    string categoryRoot = string.Empty;
                    string publishTime = string.Empty;
                    while (!inner.EOF)
                    {
                        if (inner.NodeType == XmlNodeType.Element && inner.LocalName.ToLower() == "title")
                        {
                            title = inner.ReadInnerXml();
                            title = new CommonFunction().ToXml(title);
                            continue;
                        }
                        if (inner.NodeType == XmlNodeType.Element && inner.LocalName.ToLower() == "facetitle")
                        {
                            facetitle = inner.ReadInnerXml();
                            facetitle = new CommonFunction().ToXml(facetitle);
                            continue;
                        }
                        if (inner.NodeType == XmlNodeType.Element && inner.LocalName.ToLower() == "filepath")
                        {
                            pageUrl = inner.ReadInnerXml();
                            continue;
                        }
                        inner.Read();
                    }

                    News newsObject = new News();
                    // newsObject.NewsId = id;
                    newsObject.Title = title;
                    newsObject.FaceTitle = facetitle;
                    newsObject.PageUrl = pageUrl;
                    // newsObject.PublishTime = Convert.ToDateTime(publishTime);
                    // newsObject.CategoryName = Car_SerialBll.GetNewsKind(string.IsNullOrEmpty(categoryRoot) ? "" : categoryRoot);
                    //增加索引指向
                    index++;
                    inner.Close();
                    newsList.Add(newsObject);
                }
                //关闭流
                xmlReader.Close();
                return newsList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 得到新闻内容中的列表
        /// </summary>
        /// <param name="exitsNewsList">已经存在的新闻</param>
        /// <param name="filePath">文件地址</param>
        /// <param name="newsList">新闻列表</param>
        private void GetXinWenNewsList(List<int> exitsNewsList, string filePath, Dictionary<int, News> newsList, int orderId)
        {
            if (newsList != null && newsList.Count > 0 && newsList.ContainsKey(orderId))
            {
                if (!exitsNewsList.Contains(newsList[orderId].NewsId))
                {
                    exitsNewsList.Add(newsList[orderId].NewsId);
                    return;
                }
            }

            List<News> newsObjectList = GetFocusNewsListByNumber(1, exitsNewsList, filePath);
            if (newsObjectList == null || newsObjectList.Count < 1) return;
            //如果新闻中不存在此新闻则添加
            if (!newsList.ContainsKey(orderId))
            {
                newsList.Add(orderId, newsObjectList[0]);
                exitsNewsList.Add(newsObjectList[0].NewsId);
            }
        }
        /// <summary>
        /// 得到新闻对象通过结点
        /// </summary>
        /// <param name="xNode"></param>
        /// <returns></returns>
        private News GetNewsObjectByXmlNode(XmlNode xNode)
        {
            int id = ConvertHelper.GetInteger(xNode.SelectSingleNode("newsid").InnerText);
            string title = xNode.SelectSingleNode("title").InnerText;
            string pageUrl = xNode.SelectSingleNode("filepath").InnerText;
            string categoryRoot = "";
            XmlNode cateRoot = xNode.SelectSingleNode("CategoryPath");
            if (cateRoot != null)
                categoryRoot = Car_SerialBll.GetNewsKind(cateRoot.InnerText);
            string publishTime = xNode.SelectSingleNode("publishtime").InnerText;
            News newsObject = new News();
            newsObject.NewsId = id;
            newsObject.Title = title;
            newsObject.PageUrl = pageUrl;
            newsObject.PublishTime = Convert.ToDateTime(publishTime);
            newsObject.CategoryName = categoryRoot;
            return newsObject;
        }
        /// <summary>
        /// 得到新闻列表
        /// </summary>
        /// <returns></returns>
        private DataRow[] GetNewsDataRowList(DataSet ds, int cityId)
        {
            if (ds == null || ds.Tables.Count < 2 || ds.Tables["listNews"].Rows.Count < 1) return null;
            List<int> newsIdList = new List<int>();
            DataTable newsDataTable = ds.Tables["listNews"].Clone();
            newsDataTable.Columns["publishtime"].DataType = typeof(System.DateTime);
            foreach (DataRow dr in ds.Tables["listNews"].Rows)
            {
                int newsId = ConvertHelper.GetInteger(dr["newsid"]);
                if (newsIdList.Contains(newsId)) continue;
                string relationCity = dr["relatedcityid"].ToString();
                if (string.IsNullOrEmpty(relationCity)) continue;
                bool isContainsCityId = false;
                //得到城市列表
                string[] cityIdString = relationCity.Split(',');
                List<int> cityIdList = new List<int>();

                foreach (string idString in cityIdString)
                {
                    if (string.IsNullOrEmpty(idString)) continue;
                    int tempId = ConvertHelper.GetInteger(idString);
                    if (cityId != tempId)
                    {
                        continue;
                    }
                    isContainsCityId = true;
                    break;
                }
                if (!isContainsCityId) continue;
                //将查询到的数据添加新闻数据中
                DataRow newNewsDataRow = newsDataTable.NewRow();
                newNewsDataRow["title"] = dr["title"];
                newNewsDataRow["newsid"] = dr["newsid"];
                newNewsDataRow["filepath"] = dr["filepath"];
                newNewsDataRow["publishtime"] = Convert.ToDateTime(dr["publishtime"]);
                newNewsDataRow["content"] = dr["content"];
                newNewsDataRow["sourceName"] = dr["sourceName"];
                newNewsDataRow["author"] = dr["author"];
                newsDataTable.Rows.Add(newNewsDataRow);
            }

            //返回新闻列表按时间倒序排列
            return newsDataTable.Select("", "publishtime desc");
        }
        #endregion

        public int GetFirstPingceCarId(int serialId)
        {
            int carId = 0;
            string cacheKey = "Serial_FirstPingceCarId";
            Dictionary<int, int> firstCarIdDic = (Dictionary<int, int>)CacheManager.GetCachedData(cacheKey);
            if (firstCarIdDic == null)
            {
                firstCarIdDic = new Dictionary<int, int>();
                string dataFile = Path.Combine(WebConfig.DataBlockPath, _SerialEditorComment);
                if (File.Exists(dataFile))
                {
                    FileStream fs = null;
                    XmlReader reader = null;
                    try
                    {
                        fs = new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        reader = XmlReader.Create(fs);
                        while (reader.Read())
                        {
                            if (reader.NodeType != XmlNodeType.Element)
                                continue;
                            string readerName = reader.LocalName;
                            if (readerName == "Serial")
                            {
                                reader.MoveToAttribute("id");
                                int csId = ConvertHelper.GetInteger(reader.Value);
                                if (reader.MoveToAttribute("firstCarId"))
                                {
                                    int tmpCarId = ConvertHelper.GetInteger(reader.Value);
                                    if (csId != 0 && tmpCarId != 0)
                                        firstCarIdDic[csId] = tmpCarId;
                                }
                            }
                        }
                    }
                    catch
                    { }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                        if (fs != null)
                            fs.Close();
                    }
                }
                CacheDependency cacheDependency = new CacheDependency(dataFile);
                CacheManager.InsertCache(cacheKey, firstCarIdDic, cacheDependency, DateTime.Now.AddDays(1));
            }
            if (firstCarIdDic.ContainsKey(serialId))
                carId = firstCarIdDic[serialId];
            return carId;
        }

        #region 北京购买--车型团购
        /// <summary>
        /// 得到团购活动的对象通过城市ID和子品牌ID
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="serialId">子品牌ID</param>
        /// <returns></returns>
        public GroupPurchaseEntity GetActiveGroupPurchaseByCityAndSerial(int cityId, int serialId, out GroupPurchaseEntity preGroup)
        {
            string key = @"56004efd-16c7-4eaf-a6a8-745cacc04e87";
            string pass = @"WsF4SdeRc%43d";
            preGroup = new GroupPurchaseEntity();
            try
            {
                GroupService gservice = new GroupService();
                gservice.DasSoapHeaderValue = new DasSoapHeader();
                gservice.DasSoapHeaderValue.AuthorizeCode = pass;
                gservice.DasSoapHeaderValue.TokenKey = new Guid(key);
                GroupPurchaseEntity currentEntity = gservice.GetActiveGroupPurchaseByCityAndSerial(cityId, serialId);

                if (currentEntity != null && currentEntity.GroupID > 0)
                {
                    preGroup = gservice.GetPriorGroupPurchaseInfor(currentEntity.GroupID);
                }
                return currentEntity;
            }
            catch
            {
                return null;
            }
        }

        ///// <summary>
        ///// 二手车信息 
        ///// </summary>
        //public string GetUCarHtml(int serialId)
        //{
        //    string ucarHtml = String.Empty;
        //    string ucarFile = Path.Combine(WebConfig.DataBlockPath, "data\\UsedCarInfo\\Serial_Right\\Html\\" + serialId + ".htm");

        //    if (File.Exists(ucarFile))
        //        ucarHtml = File.ReadAllText(ucarFile);
        //    return ucarHtml;
        //}

        /// <summary>
        /// 获取二手车信息
        /// </summary>
        /// <param name="serialId">子品牌编号</param>
        /// <param name="cityId">城市编号</param>
        /// <param name="countSize">获取数量</param>
        /// <returns></returns>
        public XmlNode GetUCarXml(int serialId, int cityId, int countSize)
        {
            CarSourceForBitAuto webService = GetUsedCarDataWebService();
            return webService.GetCarSourceList(serialId, cityId, countSize);

        }

        private CarSourceForBitAuto _usedCarDataWebService = null;
        private CarSourceForBitAuto GetUsedCarDataWebService()
        {
            if (_usedCarDataWebService == null)
            {
                _usedCarDataWebService = new CarSourceForBitAuto();
            }
            return _usedCarDataWebService;
        }
        #endregion

        /// <summary>
        /// 获取有新车的子品牌字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetHasNewCarSerialList()
        {
            string cacheKey = "HasNewCarSerialIdDictionary";
            Dictionary<int, int> hasNewCarDic = (Dictionary<int, int>)CacheManager.GetCachedData(cacheKey);
            if (hasNewCarDic == null)
            {
                hasNewCarDic = new Dictionary<int, int>();
                using (XmlReader reader = XmlReader.Create(WebConfig.AutoDataFile))
                {
                    while (reader.ReadToFollowing("Serial"))
                    {
                        reader.MoveToAttribute("ID");
                        int serialId = ConvertHelper.GetInteger(reader.Value);
                        if (reader.MoveToAttribute("CsHasNew"))
                        {
                            int hasNew = ConvertHelper.GetInteger(reader.Value);
                            if (hasNew > 0)
                                hasNewCarDic[serialId] = 1;
                        }
                    }
                    reader.Close();
                }
                CacheDependency cacheDependency = new CacheDependency(WebConfig.AutoDataFile);
                CacheManager.InsertCache(cacheKey, hasNewCarDic, cacheDependency, DateTime.Now.AddDays(1));
            }
            return hasNewCarDic;
        }
        /// <summary>
        /// 获取子品牌置换优惠信息
        /// </summary>
        public DataSet GetCarReplacementInfo(List<int> serialIds, int cityId, int cityParentId)
        {
            if (serialIds == null || serialIds.Count < 1 || (cityId < 1 && cityParentId < 1)) return null;

            return csd.GetCarReplacementInfo(serialIds, cityId, cityParentId, -1);
        }
        /// <summary>
        /// 获取子品牌置换优惠信息
        /// </summary>
        public DataSet GetCarReplacementInfo(int serialId, int cityId, int cityParentId)
        {
            if (serialId < 1 || (cityId < 1 && cityParentId < 1)) return null;

            return csd.GetCarReplacementInfo(serialId, cityId, cityParentId);
        }
        /// <summary>
        /// 获取子品牌置换优惠信息
        /// </summary>
        public DataSet GetCarReplacementInfo(List<int> serialIds, int cityId, int cityParentId, int top)
        {
            if (serialIds == null || serialIds.Count < 1 || (cityId < 1 && cityParentId < 1)) return null;

            return csd.GetCarReplacementInfo(serialIds, cityId, cityParentId, top);
        }
        /// <summary>
        /// 获取子品牌置换优惠信息和备注
        /// </summary>
        public DataSet GetCarReplacementInfoAndMemo(List<int> serialIds, int cityId, int cityParentId)
        {
            if (serialIds == null || serialIds.Count < 1 || (cityId < 1 && cityParentId < 1)) return null;

            return csd.GetCarReplacementInfoAndMemo(serialIds, cityId, cityParentId, -1);
        }
        /// <summary>
        /// 获取子品牌置换优惠信息和备注
        /// </summary>
        public DataSet GetCarReplacementInfoAndMemo(List<int> serialIds, int cityId, int cityParentId, int top)
        {
            if (serialIds == null || serialIds.Count < 1 || (cityId < 1 && cityParentId < 1)) return null;

            return csd.GetCarReplacementInfoAndMemo(serialIds, cityId, cityParentId, top);
        }
        /// <summary>
        /// 子品牌是否有置换服务
        /// </summary>
        public bool IsZhiHuanService(int serialId)
        {
            if (serialId < 1) return false;
            List<int> list = GetAllZhiHuanCsID();
            if (list == null || list.Count < 1)
                return false;
            else
                return list.Contains(serialId);
        }

        /// <summary>
        /// 取所有有置换信息的子品牌ID add by chengl Jul.18.2012
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllZhiHuanCsID()
        {
            string cacheKey = "Car_SerialBll_GetAllZhiHuanCsID";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj == null)
            {
                DataSet ds = csd.GetZhiHuanList();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<int> serialIds = new List<int>(ds.Tables[0].Rows.Count);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        serialIds.Add(ConvertHelper.GetInteger(row["serialid"].ToString()));
                    }
                    obj = serialIds;
                }
                else
                {
                    obj = new object();
                }
                CacheManager.InsertCache(cacheKey, obj, 15);
            }
            return obj as List<int>;
        }

        #region memCache 数据源

        /// <summary>
        /// 根据子品牌ID，取子品牌基本信息
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public Dictionary<string, string> GetSerialBaseInfoFromMemCache(int csID)
        {
            Dictionary<string, string> dicCsBaseInfo = new Dictionary<string, string>();
            string key = string.Format("Car_Dic_SerialBaseJsonInfo_{0}", csID);
            object obj = MemCache.GetMemCacheByKey(key);
            if (obj != null && obj is Dictionary<string, string>)
            {
                dicCsBaseInfo = (Dictionary<string, string>)obj;
            }
            else
            {
                SerialEntity se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csID);
                if (se != null && se.Id > 0)
                {
                    Dictionary<int, string> dicPingCe = GetAllSerialRainbowItemByRainbowItemID(60);
                    dicCsBaseInfo.Add("CsID", se.Id.ToString());
                    dicCsBaseInfo.Add("CsName", se.Name);
                    dicCsBaseInfo.Add("CsShowName", se.ShowName);
                    dicCsBaseInfo.Add("CsAllSpell", se.AllSpell);
                    dicCsBaseInfo.Add("CsBBS", se.BaaUrl);
                    dicCsBaseInfo.Add("CsPingCe", dicPingCe.ContainsKey(se.Id) ? dicPingCe[se.Id] : "");

                    MemCache.SetMemCacheByKey(key, dicCsBaseInfo, 60 * 60 * 1000);
                }
            }
            return dicCsBaseInfo;
        }

        #endregion

        /// <summary>
        /// 获取子品牌城市促销新闻数
        /// </summary>
        public int GetCuXiaoNewsCount(int serialId, int cityId)
        {
            Dictionary<int, Dictionary<int, int>> cuxiaoNewsCount = GetCuXiaoNewsCount();
            if (cuxiaoNewsCount == null || cuxiaoNewsCount.Count < 1 || !cuxiaoNewsCount.ContainsKey(serialId))
                return 0;
            Dictionary<int, int> cityCount = cuxiaoNewsCount[serialId];
            if (cuxiaoNewsCount.Count < 1 || !cityCount.ContainsKey(cityId))
                return 0;
            else
                return cityCount[cityId];
        }
        /// <summary>
        /// 获取促销新闻数字典
        /// key=子品牌id
        /// value= <key=城市id，value=新闻数量>
        /// </summary>
        public Dictionary<int, Dictionary<int, int>> GetCuXiaoNewsCount()
        {
            string cacheKey = "BitAuto_CarChannel_BLL_Car_SerialBll_GetCuXiaoNewsCountAll";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj == null)
            {
                string file = Path.Combine(WebConfig.DataBlockPath, "data\\SerialCityNews\\CityNewsCount.xml");
                if (File.Exists(file))
                {
                    try
                    {
                        Dictionary<int, Dictionary<int, int>> count = new Dictionary<int, Dictionary<int, int>>();
                        using (XmlReader reader = XmlReader.Create(file))
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType != XmlNodeType.Element || reader.Name.ToLower() != "cs")
                                    continue;
                                Dictionary<int, int> cityCount = new Dictionary<int, int>();
                                count.Add(ConvertHelper.GetInteger(reader["id"]), cityCount);
                                XmlReader subReader = reader.ReadSubtree();
                                while (subReader.Read())
                                {
                                    if (subReader.NodeType != XmlNodeType.Element || subReader.Name.ToLower() != "city")
                                        continue;
                                    cityCount.Add(ConvertHelper.GetInteger(subReader["id"]),
                                        ConvertHelper.GetInteger(subReader["num"]));
                                }
                            }
                        }
                        obj = count;
                    }
                    catch { }
                }
                if (obj == null)
                    obj = new Dictionary<int, Dictionary<int, int>>();
                CacheManager.InsertCache(cacheKey, obj, 30);
            }
            return obj as Dictionary<int, Dictionary<int, int>>;
        }

        /// <summary>
        /// 获取车型详解首条标题
        /// </summary>
        public News GetSerialPingCeTitleNews(int serialId)
        {
            if (serialId > 0)
            {

                Dictionary<int, News> dic = GetAllSerialPingCeTitleNews();
                if (dic != null && dic.ContainsKey(serialId))
                    return dic[serialId];
            }
            return null;
        }
        /// <summary>
        /// 获取车型详解首条标题
        /// </summary>
        private Dictionary<int, News> GetAllSerialPingCeTitleNews()
        {
            string cacheKey = "Car_SerialBll_GetAllSerialPingCeTitleNews";
            Dictionary<int, News> result = CacheManager.GetCachedData(cacheKey) as Dictionary<int, News>;
            if (result == null)
            {
                string path = Path.Combine(WebConfig.DataBlockPath, @"Data\SerialNews\pingce\allpingce.xml");
                if (File.Exists(path))
                {
                    XmlDocument doc = CommonFunction.ReadXml(path);

                    if (doc != null)
                    {
                        XmlNodeList nodeList = doc.SelectNodes("PingCeList/News");
                        result = new Dictionary<int, News>(nodeList.Count);
                        foreach (XmlNode node in nodeList)
                        {
                            result.Add(ConvertHelper.GetInteger(node.Attributes["CsId"].Value)
                            , new News()
                            {
                                NewsId = ConvertHelper.GetInteger(node.Attributes["NewsId"].Value)
                                ,
                                Title = node.Attributes["Title"].Value
                                ,
                                FaceTitle = node.Attributes["FaceTitle"].Value
                                ,
                                PageUrl = node.Attributes["Url"].Value

                            });
                        }
                        CacheManager.InsertCache(cacheKey, result, 30);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 返回一条口碑报告
        /// </summary>
        public News GetSerialKouBeiReport(int csId)
        {
            string cacheKey = string.Format("Car_SerialBll_GetSerialKouBeiReport_" + csId);
            News news = CacheManager.GetCachedData(cacheKey) as News;
            if (news == null)
            {
                XmlDocument doc = CommonFunction.ReadXmlFromFile(Path.Combine(WebConfig.DataBlockPath, string.Format(@"Data\KouBei\Report\{0}.xml", csId)));
                if (doc != null)
                {
                    XmlNode node = doc.SelectSingleNode("feed/entry[1]");
                    if (node != null)
                    {
                        news = new News()
                        {
                            Title = node.Attributes["title"].Value,
                            PageUrl = node.Attributes["url"].Value
                        };
                        CacheManager.InsertCache(cacheKey, news, 30);
                    }
                }
            }
            return news;
        }
        /// <summary>
        /// 获取 CNCAP 数据 
        /// 优先级：C-NCAP星级（后台ID：649）→E-NCAP星级（后台ID：637）→IIHS总评价（后台ID：957）→NHTSA星级（后台ID：638）
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, CNCAPEntity> GetCNCAPData()
        {
            Dictionary<int, CNCAPEntity> dict = new Dictionary<int, CNCAPEntity>();
            try
            {
                string cacheKey = "Car_SerialBll_GetCNCAPData";
                object obj = CacheManager.GetCachedData(cacheKey);
                if (obj != null)
                    return (Dictionary<int, CNCAPEntity>)obj;

                if (obj == null)
                {
                    var paramsIds = new Dictionary<int, string>() { { 649, "C-NCAP碰撞" }, { 637, "E-NCAP碰撞" }, { 957, "IIHS总评价" }, { 638, "NHTSA碰撞" } };
                    DataSet ds = csd.GetCNCAPData();
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var serialGroup = ds.Tables[0].Rows.Cast<DataRow>().GroupBy(p => new { SerialId = ConvertHelper.GetInteger(p["Cs_Id"]) }, p => p);

                        foreach (var g in serialGroup)
                        {
                            var key = CommonFunction.Cast(g.Key, new { SerialId = 0 });
                            if (!dict.ContainsKey(key.SerialId))
                            {
                                var serialGroupList = g.ToList();
                                CNCAPEntity entity = null;
                                foreach (var paramId in paramsIds.Keys)
                                {
                                    var all = serialGroupList.FindAll(p => ConvertHelper.GetInteger(p["ParamId"]) == paramId);
                                    if (all == null || all.Count <= 0) continue;
                                    var lastRow = all.LastOrDefault();
                                    entity = new CNCAPEntity()
                                    {
                                        ParamId = ConvertHelper.GetInteger(lastRow["ParamId"]),
                                        Name = paramsIds[paramId],
                                        SerialId = key.SerialId,
                                        ParamValue = ConvertHelper.GetString(lastRow["Pvalue"])
                                    };
                                    break;
                                }
                                if (entity != null)
                                    dict.Add(key.SerialId, entity);
                            }
                        }
                        if (dict.Count > 0)
                            CacheManager.InsertCache(cacheKey, dict, WebConfig.CachedDuration);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return dict;
        }

        /// <summary>
        /// 获取 CNCAP和ENCAP 数据 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<CNCAPEntity>> GetCNCAPAndENCAPData()
        {
            Dictionary<int, List<CNCAPEntity>> dict = new Dictionary<int, List<CNCAPEntity>>();
            try
            {
                string cacheKey = "Car_SerialBll_GetCNCAPAndENCAPData";
                object obj = CacheManager.GetCachedData(cacheKey);
                if (obj != null)
                    return (Dictionary<int, List<CNCAPEntity>>)obj;

                if (obj == null)
                {
                    var paramsIds = new Dictionary<int, string>() { { 649, "C-NCAP碰撞" }, { 637, "E-NCAP碰撞" } };
                    DataSet ds = csd.GetCNCAPData();
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var serialGroup = ds.Tables[0].Rows.Cast<DataRow>().GroupBy(p => new { SerialId = ConvertHelper.GetInteger(p["Cs_Id"]) }, p => p);

                        foreach (var g in serialGroup)
                        {
                            var key = CommonFunction.Cast(g.Key, new { SerialId = 0 });
                            if (!dict.ContainsKey(key.SerialId))
                            {
                                var serialGroupList = g.ToList();
                                List<CNCAPEntity> list = new List<CNCAPEntity>();
                                foreach (var paramId in paramsIds.Keys)
                                {
                                    var all = serialGroupList.FindAll(p => ConvertHelper.GetInteger(p["ParamId"]) == paramId);
                                    if (all == null || all.Count <= 0) continue;
                                    var lastRow = all.LastOrDefault();
                                    CNCAPEntity entity = new CNCAPEntity()
                                    {
                                        ParamId = ConvertHelper.GetInteger(lastRow["ParamId"]),
                                        Name = paramsIds[paramId],
                                        SerialId = key.SerialId,
                                        ParamValue = ConvertHelper.GetString(lastRow["Pvalue"])
                                    };
                                    list.Add(entity);
                                }
                                if (list.Count > 0)
                                    dict.Add(key.SerialId, list);
                            }
                        }
                        if (dict.Count > 0)
                            CacheManager.InsertCache(cacheKey, dict, WebConfig.CachedDuration);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return dict;
        }
        /// <summary>
        /// 子品牌二手车价格
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetUCarSerialPrice()
        {
            string cacheKey = "Car_SerialBll_UCarSerialPrice";
            object objUCarPrice = CacheManager.GetCachedData(cacheKey);
            if (objUCarPrice != null)
                return (Dictionary<int, string>)objUCarPrice;
            Dictionary<int, string> dictUCarPrice = new Dictionary<int, string>();
            try
            {
                DataSet dsUCarPrice = this.GetSerialUcarData();
                if (dsUCarPrice != null && dsUCarPrice.Tables.Count > 0 && dsUCarPrice.Tables[0].Rows.Count > 0)
                {
                    var rows = dsUCarPrice.Tables[0].Select("Car_YearType=0");
                    foreach (DataRow dr in rows)
                    {
                        int serialId = ConvertHelper.GetInteger(dr["Cs_Id"]);
                        decimal minPrice = decimal.Round(ConvertHelper.GetDecimal(dr["MinPrice"]), 2);
                        decimal maxPrice = decimal.Round(ConvertHelper.GetDecimal(dr["MaxPrice"]), 2);
                        if (!dictUCarPrice.ContainsKey(serialId))
                        {
                            dictUCarPrice.Add(serialId,
                                              minPrice == maxPrice
                                                  ? string.Format("{0}万", maxPrice)
                                                  : string.Format("{0}-{1}万", minPrice, maxPrice));
                        }
                    }
                    CacheManager.InsertCache(cacheKey, dictUCarPrice, WebConfig.CachedDuration);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return dictUCarPrice;
        }
        /// <summary>
        /// 子品牌二手车最低价
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetUCarSerialLowPrice()
        {
            string cacheKey = "Car_SerialBll_UCarSerialLowPrice";
            object objUCarPrice = CacheManager.GetCachedData(cacheKey);
            if (objUCarPrice != null)
                return (Dictionary<int, string>)objUCarPrice;
            Dictionary<int, string> dictUCarPrice = new Dictionary<int, string>();
            try
            {
                DataSet dsUCarPrice = this.GetSerialUcarData();
                if (dsUCarPrice != null && dsUCarPrice.Tables.Count > 0 && dsUCarPrice.Tables[0].Rows.Count > 0)
                {
                    var rows = dsUCarPrice.Tables[0].Select("Car_YearType=0");
                    foreach (DataRow dr in rows)
                    {
                        int serialId = ConvertHelper.GetInteger(dr["Cs_Id"]);
                        decimal minPrice = decimal.Round(ConvertHelper.GetDecimal(dr["MinPrice"]), 2);
                        if (!dictUCarPrice.ContainsKey(serialId))
                            dictUCarPrice.Add(serialId, string.Format("{0}万起", minPrice));
                    }
                    CacheManager.InsertCache(cacheKey, dictUCarPrice, WebConfig.CachedDuration);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return dictUCarPrice;
        }
        /// <summary>
        /// 获取 子品牌 年款 二手车报价
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetSerialUCarYearPrice(int serialId)
        {
            string cacheKey = "Car_SerialBll_GetSerialUCarYearPrice_" + serialId;
            object objUCarPrice = CacheManager.GetCachedData(cacheKey);
            if (objUCarPrice != null)
                return (Dictionary<int, string>)objUCarPrice;
            Dictionary<int, string> dictUCarPrice = new Dictionary<int, string>();
            try
            {
                DataSet dsUCarPrice = this.GetSerialUcarData();
                if (dsUCarPrice != null && dsUCarPrice.Tables.Count > 0 && dsUCarPrice.Tables[0].Rows.Count > 0)
                {
                    var rows = dsUCarPrice.Tables[0].Select("Cs_Id=" + serialId + " AND Car_YearType<>0");
                    foreach (DataRow dr in rows)
                    {
                        int year = ConvertHelper.GetInteger(dr["Car_YearType"]);
                        decimal minPrice = decimal.Round(ConvertHelper.GetDecimal(dr["MinPrice"]), 2);
                        decimal maxPrice = decimal.Round(ConvertHelper.GetDecimal(dr["MaxPrice"]), 2);
                        if (!dictUCarPrice.ContainsKey(year))
                            dictUCarPrice.Add(year,
                                              minPrice == maxPrice
                                                  ? string.Format("{0}万", maxPrice)
                                                  : string.Format("{0}-{1}万", minPrice, maxPrice));
                    }
                    CacheManager.InsertCache(cacheKey, dictUCarPrice, WebConfig.CachedDuration);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return dictUCarPrice;
        }
        /// <summary>
        /// 获取 二手车子品牌 年款 报价 
        /// </summary>
        /// <returns></returns>
        public DataSet GetSerialUcarData()
        {
            string cacheKey = "Car_SerialBll_UCarSerialPrice_DataSet";
            object objUCarPrice = CacheManager.GetCachedData(cacheKey);
            if (objUCarPrice != null)
                return (DataSet)objUCarPrice;

            DataSet dsUCarPrice = new DataSet();
            string ucarPriceFilePath = Path.Combine(WebConfig.DataBlockPath, @"Data\UsedCarInfo\AllSerialPrice.Xml");
            if (File.Exists(ucarPriceFilePath))
            {
                dsUCarPrice.ReadXml(ucarPriceFilePath);
                CacheManager.InsertCache(cacheKey, dsUCarPrice, WebConfig.CachedDuration);
            }
            return dsUCarPrice;
        }

        /// <summary>
        /// 取所有惠买车地址子品牌ID和对应的惠买车Url
        /// add by chengl Apr.25.2014 于继生
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetEPHuiMaiCheAllCsUrl()
        {
            string cacheKey = "Car_SerialBll_GetEPHuiMaiCheAllCsUrl";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
                return (Dictionary<int, string>)obj;
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string filePath = Path.Combine(WebConfig.DataBlockPath, @"Data\EP\HuiMaiCheAllCsUrl.xml");
            if (File.Exists(filePath))
            {
                XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(filePath);
                XmlNodeList entries = xmlDoc.SelectNodes("/ArrayOfCarInfo/CarInfo");
                if (entries != null && entries.Count > 0)
                {
                    foreach (XmlNode xn in entries)
                    {
                        int csid = 0;
                        if (xn.SelectSingleNode("SerialID") != null &&
                            int.TryParse(xn.SelectSingleNode("SerialID").InnerText, out csid))
                        { }
                        string url = "";
                        if (xn.SelectSingleNode("Url") != null)
                        { url = xn.SelectSingleNode("Url").InnerText; }
                        if (csid > 0 && url != "" && !dic.ContainsKey(csid))
                        {
                            dic.Add(csid, url);
                        }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
            }
            return dic;
        }

        /// <summary>
        /// 获取在销车款所有排量 根据子品牌ID
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public List<string> GetSaleCarExhaustBySerialId(int serialId)
        {
            string cacheKey = "Car_SerialBll_GetSaleCarExhaustBySerialId_" + serialId;
            object objExhaustList = CacheManager.GetCachedData(cacheKey);
            if (objExhaustList != null)
                return (List<string>)objExhaustList;
            List<string> list = new List<string>();
            DataSet ds = csd.GetSaleCarExhaustBySerialId(serialId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var query = ds.Tables[0].Rows.Cast<DataRow>().Select(p => p["Engine_Exhaust"]);
                foreach (string exhaust in query)
                {
                    if (!string.IsNullOrEmpty(exhaust))
                        list.Add(exhaust);
                }
                CacheManager.InsertCache(cacheKey, list, WebConfig.CachedDuration);
            }
            return list;
        }

        /// <summary>
        /// 取所有子品牌近30天按级别分组排名
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialUVRAngeByLevel()
        {
            string cacheKey = "Car_SerialBll_GetAllSerialUVRAngeByLevel";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            { return (DataSet)obj; }

            DataSet ds = csd.GetAllSerialUVRAngeByLevel();
            CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
            return ds;
        }

        /// <summary>
        /// 取某级别的按UV倒序的子品牌数据
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public DataSet GetLevelSerialDataByUV(string level)
        {
            string cacheKey = "Car_SerialBll_GetLevelSerialDataByUV";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            { return (DataSet)obj; }

            DataSet ds = csd.GetLevelSerialDataByUV(level);
            CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
            return ds;
        }

        /// <summary>
        /// 按级别和销售状态取车系数据
        /// </summary>
        /// <param name="level">级别</param>
        /// <param name="saleState">销售状态，如果取全部数据传null</param>
        /// <returns></returns>
        public DataSet GetLevelSerialByUVAndSaleState(string level, List<string> saleState)
        {
            string saleStateStr = saleState == null || saleState.Count == 0 ? "All" : string.Join(",", saleState.ToArray());
            string cacheKey = "Car_SerialBll_GetLevelSerialByUVAndSaleState_" + level + "_" + saleStateStr;
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            { return (DataSet)obj; }

            DataSet ds = csd.GetLevelSerialDataByUVAndSaleState(level, saleState);
            CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
            return ds;
        }

        /// <summary>
        /// 五年旧车保值率
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public List<XmlElement> GetSerialBaoZhiLv(string level)
        {
            Dictionary<string, List<XmlElement>> dic = null;
            Dictionary<int, XmlElement> items = GetSeialBaoZhiLv();
            if (items != null && items.Count > 0)
            {
                dic = new Dictionary<string, List<XmlElement>>();
                foreach (KeyValuePair<int, XmlElement> ele in items)
                {
                    string levelStr = ele.Value.Attributes["Level"].InnerText;
                    if (dic.ContainsKey(levelStr))
                    {
                        dic[levelStr].Add(ele.Value);
                    }
                    else
                    {
                        List<XmlElement> list = new List<XmlElement>();
                        list.Add(ele.Value);
                        dic.Add(levelStr, list);
                    }
                }
            }
            return dic != null && dic.ContainsKey(level) ? dic[level] : null;
        }

        /// <summary>
        /// 返回所有5年保值率
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, XmlElement> GetSeialBaoZhiLv()
        {
            string cacheKey = "Car_SerialBll_GetSerialBaoZhiLv";
            object obj = CacheManager.GetCachedData(cacheKey);
            //List<XmlElement> list = null;
            Dictionary<int, XmlElement> dic = null;
            if (obj != null)
            {
                dic = (Dictionary<int, XmlElement>)obj;
            }
            else
            {
                string filePath = Path.Combine(WebConfig.DataBlockPath, @"Data\SerialSet\BaoZhiLv.xml");
                if (File.Exists(filePath))
                {
                    //list = new List<XmlElement>();
                    dic = new Dictionary<int, XmlElement>();
                    XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(filePath);
                    XmlNodeList items = xmlDoc.SelectNodes("/Root/Serial");
                    if (items != null && items.Count > 0)
                    {
                        foreach (XmlElement ele in items)
                        {
                            //list.Add(ele);
                            int csId = ConvertHelper.GetInteger(ele.Attributes["Id"].InnerText);
                            if (!dic.ContainsKey(csId))
                            {
                                dic.Add(csId, ele);
                            }
                        }
                    }
                    CacheManager.InsertCache(cacheKey, dic, 60);
                }
            }
            return dic;
        }
        /// <summary>
        /// 车系销量排行榜,按级别分组
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public List<XmlElement> GetSeialSellRank(string level)
        {
            Dictionary<string, List<XmlElement>> dic = null;
            Dictionary<int, XmlElement> items = GetSeialSellRank();
            if (items != null && items.Count > 0)
            {
                dic = new Dictionary<string, List<XmlElement>>();
                foreach (KeyValuePair<int, XmlElement> ele in items)
                {
                    string levelStr = ele.Value.Attributes["Level"].InnerText;
                    if (dic.ContainsKey(levelStr))
                    {
                        dic[levelStr].Add(ele.Value);
                    }
                    else
                    {
                        List<XmlElement> list = new List<XmlElement>();
                        list.Add(ele.Value);
                        dic.Add(levelStr, list);
                    }
                }
            }
            return dic != null && dic.ContainsKey(level) ? dic[level] : null;
        }

        /// <summary>
		/// 车系销量排行榜
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, XmlElement> GetSeialSellRank()
        {
            string cacheKey = "Car_SerialBll_GetSeialSellRank";
            object obj = CacheManager.GetCachedData(cacheKey);
            //List<XmlElement> list = null;
            Dictionary<int, XmlElement> dic = null;
            if (obj != null)
            {
                dic = (Dictionary<int, XmlElement>)obj;
            }
            else
            {
                string filePath = Path.Combine(WebConfig.DataBlockPath, @"Data\SerialSet\SerialSaleRank.xml");
                if (File.Exists(filePath))
                {
                    //list = new List<XmlElement>();
                    dic = new Dictionary<int, XmlElement>();
                    XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(filePath);
                    XmlNodeList items = xmlDoc.SelectNodes("/Root/Item");
                    if (items != null && items.Count > 0)
                    {
                        foreach (XmlElement ele in items)
                        {
                            //list.Add(ele);
                            int csId = ConvertHelper.GetInteger(ele.Attributes["CsId"].InnerText);
                            if (!dic.ContainsKey(csId))
                            {
                                dic.Add(csId, ele);
                            }
                        }
                    }
                    CacheManager.InsertCache(cacheKey, dic, 60);
                }
            }
            return dic;
        }
        /// <summary>
        ///  易湃的销量最高的suv车型接口数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Dictionary<int, string[]>> GetEPSUVSalesRank()
        {
            string cacheKey = "Car_SerialBll_GetEPSUVSalesRank";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                return (Dictionary<int, Dictionary<int, string[]>>)obj;
            }
            Dictionary<int, Dictionary<int, string[]>> dic = new Dictionary<int, Dictionary<int, string[]>>();
            string filePath = Path.Combine(WebConfig.DataBlockPath, @"Data\EP\SUVMonthSaleRankTop10.xml");
            if (File.Exists(filePath))
            {
                XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(filePath);
                XmlNode root = xmlDoc.SelectSingleNode("/Root");
                string date = "";
                if (root != null)
                {
                    if (root.Attributes["Date"] != null)
                    {
                        date = root.Attributes["Date"].InnerText;
                        Dictionary<int, string[]> dicTemp = new Dictionary<int, string[]>();
                        string[] arrayA = { date, "" };
                        dicTemp.Add(0, arrayA);
                        dic.Add(-1, dicTemp);
                    }
                }
                XmlNodeList items = xmlDoc.SelectNodes("/Root/Item");
                if (items != null && items.Count > 0)
                {
                    foreach (XmlNode xn in items)
                    {
                        int csid = 0;
                        if (xn.Attributes["Id"] != null &&
                            int.TryParse(xn.Attributes["Id"].InnerText, out csid))
                        { }
                        int count = 0;
                        if (xn.Attributes["Count"] != null &&
                            int.TryParse(xn.Attributes["Count"].InnerText, out count))
                        { }
                        string name = "";
                        if (xn.Attributes["Name"] != null)
                        { name = xn.Attributes["Name"].InnerText; }
                        string spell = "";
                        if (xn.Attributes["AllSpell"] != null)
                        { spell = xn.Attributes["AllSpell"].InnerText; }
                        if (csid > 0 && count > 0 && name != "" && !dic.ContainsKey(csid))
                        {
                            Dictionary<int, string[]> dicTemp = new Dictionary<int, string[]>();
                            string[] array = { spell, name };
                            dicTemp.Add(count, array);
                            dic.Add(csid, dicTemp);
                        }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
            }
            return dic;
        }

        public DataSet GetSerialInfoForPK(int serialId1, int serialId2)
        {
            return csd.GetSerialInfoForPK(serialId1, serialId2);
        }
        /// <summary>
        /// 获取子品牌 团购地址
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetSerialTuanUrl()
        {
            string cacheKey = "Car_SerialBll_GetSerialTurnUrl";
            var obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (Dictionary<int, string>)obj;
            Dictionary<int, string> dict = new Dictionary<int, string>();
            try
            {
                string xmlFile = Path.Combine(WebConfig.DataBlockPath, @"Data\Tuan\SerialTuanUrl.xml");
                XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);
                if (xmlDoc != null)
                {
                    XmlNodeList nodeList = xmlDoc.SelectNodes("//Item");
                    foreach (XmlNode node in nodeList)
                    {
                        var serialId = ConvertHelper.GetInteger(node.SelectSingleNode("./CsId").InnerText);
                        var adLink = node.SelectSingleNode("./Adlink").InnerText;
                        if (!dict.ContainsKey(serialId))
                            dict.Add(serialId, adLink);
                    }
                    CacheManager.InsertCache(cacheKey, dict, WebConfig.CachedDuration);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return dict;
        }

        /// <summary>
        /// 获取看过还看过广告
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Dictionary<string, string>> GetSeeToSeeAD(int serialId, bool isTest = false)
        {
            string key = "serialToSeeADConfig";
            DateTime curDate = DateTime.Now;

            Dictionary<int, Dictionary<int, Dictionary<string, string>>> cacheData = CacheManager.GetCachedData(key) as Dictionary<int, Dictionary<int, Dictionary<string, string>>>;
            if (cacheData == null || isTest)
            {
                string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~"), "App_Data\\SerialToSeeADConfig.xml");
                if (!File.Exists(filePath))
                    return null;
                cacheData = new Dictionary<int, Dictionary<int, Dictionary<string, string>>>();
                CacheManager.InsertCache(key, cacheData, 30);
                Dictionary<int, Dictionary<string, string>> serialData = null;
                FileStream stream = null;
                XmlReader reader = null;
                try
                {
                    stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    reader = XmlReader.Create(stream);
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AD")
                        {
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                reader.MoveToAttribute("ad_serialid");
                                string adserailid = reader.Value;
                                while (subReader.Read())
                                {
                                    if (subReader.NodeType == XmlNodeType.Element && subReader.Name == "Serial")
                                    {
                                        string title = string.Empty, url = string.Empty, imgUrl = string.Empty, priceRange = string.Empty;
                                        int postion = 0, toSerialId = 0;
                                        bool needAd = true;
                                        while (subReader.Read())
                                        {
                                            if (subReader.NodeType == XmlNodeType.EndElement && subReader.Name == "Serial")
                                                break;
                                            if (reader.NodeType == XmlNodeType.Element)
                                            {
                                                if (reader.Name == "Position")
                                                {
                                                    postion = ConvertHelper.GetInteger(reader.ReadString().Trim());
                                                }
                                                else if (reader.Name == "SerialID")
                                                {
                                                    toSerialId = ConvertHelper.GetInteger(reader.ReadString().Trim());
                                                }
                                                else if (reader.Name == "Title")
                                                {
                                                    title = reader.ReadString().Trim();
                                                }
                                                else if (reader.Name == "Url")
                                                {
                                                    url = reader.ReadString().Trim();
                                                }
                                                else if (reader.Name == "ImgUrl")
                                                {
                                                    imgUrl = reader.ReadString().Trim();
                                                }
                                                else if (reader.Name == "StartDate")
                                                {
                                                    DateTime startDate = ConvertHelper.GetDateTime(reader.ReadString().Trim());
                                                    if (startDate > curDate)
                                                    {
                                                        needAd = false;
                                                        break;
                                                    }
                                                }
                                                else if (reader.Name == "EndData")
                                                {
                                                    DateTime endDate = ConvertHelper.GetDateTime(reader.ReadString().Trim());
                                                    if (endDate.AddDays(1) < curDate)
                                                    {
                                                        needAd = false;
                                                        break;
                                                    }
                                                }
                                                else if (reader.Name == "PriceRange")
                                                {
                                                    priceRange = reader.ReadString().Trim();
                                                }
                                            }
                                        }

                                        if (needAd)
                                        {
                                            if (cacheData.ContainsKey(toSerialId))
                                                serialData = cacheData[toSerialId];
                                            else
                                            {
                                                serialData = new Dictionary<int, Dictionary<string, string>>();
                                                cacheData.Add(toSerialId, serialData);
                                            }

                                            serialData[postion] = new Dictionary<string, string>();
                                            serialData[postion]["AD_SerialID"] = adserailid;
                                            serialData[postion]["Title"] = title;
                                            serialData[postion]["Url"] = url;
                                            serialData[postion]["ImgUrl"] = imgUrl;
                                            serialData[postion]["PriceRange"] = priceRange;
                                            //{{ , },{ , },{, },{, }};
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    if (stream != null)
                        stream.Dispose();
                }
            }
            return cacheData.ContainsKey(serialId) ? cacheData[serialId] : null;
        }

        /// <summary>
        ///     取所有有销量数据的子品牌
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllSerialSale()
        {
            string cacheKey = "Car_SerialBll_GetAllSerialSale";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                return (List<int>)obj;
            }

            var listCsID = new List<int>();
            try
            {
                string filePath = Path.Combine(WebConfig.DataBlockPath, @"Data\SaleCsIDList.xml");
                if (File.Exists(filePath))
                {
                    var doc = new XmlDocument();
                    doc.Load(filePath);
                    XmlNodeList xnl = doc.SelectNodes("/Root/Cs");
                    if (xnl != null && xnl.Count > 0)
                    {
                        foreach (XmlNode xn in xnl)
                        {
                            if (!listCsID.Contains(int.Parse(xn.Attributes["ID"].Value)))
                            {
                                listCsID.Add(int.Parse(xn.Attributes["ID"].Value));
                            }
                        }
                        CacheManager.InsertCache(cacheKey, listCsID, 60 * 12);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return listCsID;
        }

        public string GetSerialTotalPV(int csID)
        {
            return csd.GetSerialTotalPV(csID);
        }

        /// <summary>
        /// 根据一级车身级别,返回车系最近30天uv排行;
        /// </summary>
        /// <param name="csId">车系id</param>
        /// <returns></returns>
        public int GetAllSerialUvRank30Days(int csId)
        {
            Dictionary<int, int> uvDic = null;
            string cacheKey = "Car_SerialBll_GetAllSerialUv30Days";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                uvDic = (Dictionary<int, int>)obj;
            }
            else
            {
                uvDic = csd.GetAllSerialUvRank30Days();
                CacheManager.InsertCache(cacheKey, uvDic, WebConfig.CachedDuration);
            }
            if (uvDic.ContainsKey(csId))
            {
                return uvDic[csId];
            }
            return 0;
        }

        /// <summary>
        /// 子品牌同级别排行
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public string GetSerialTotalPVWithCache(int csID)
        {
            Hashtable ht = null;
            string cacheKey = "Car_SerialBll_GetSerialTotalPV";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                ht = (Hashtable)obj;
            }
            else
            {
                ht = csd.GetAllSerialPV();
                CacheManager.InsertCache(cacheKey, ht, WebConfig.CachedDuration);
            }
            if (ht == null || ht.Count == 0)
            {
                return string.Empty;
            }
            if (ht.ContainsKey(csID))
            {
                return ht[csID].ToString();
            }
            return string.Empty;
        }

        public void GetCarParallelAndSell(int serialId, int cityId, out List<int> pingXingImport, out List<int> baoXiao)
        {
            List<int> import = new List<int>();
            List<int> sell = new List<int>();
            DataSet ds = csd.GetCarParallelAndSell(serialId, cityId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int carId = ConvertHelper.GetInteger(dr["CarId"]);
                    int carType = Convert.ToInt32(dr["CarType"].ToString());
                    CarEntity carEntity = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                    string carName = carEntity != null ? carEntity.Name : string.Empty;
                    if (carType == 0)//包销
                    {
                        sell.Add(carId);
                    }
                    if (carType == 1)//平行进口
                    {
                        import.Add(carId);
                    }
                }
            }
            pingXingImport = import;
            baoXiao = sell;
        }

        /// <summary>
        /// 取所有子品牌口碑数、口碑综合评分、细项评分
        /// add by chengl Dec.9.2015
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, CsKoubeiBaseInfo> GetAllCsKoubeiBaseInfo()
        {
            Dictionary<int, CsKoubeiBaseInfo> dic = new Dictionary<int, CsKoubeiBaseInfo>();
            string cacheKey = "Car_SerialBll_GetAllCsKoubeiBaseInfo";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                dic = (Dictionary<int, CsKoubeiBaseInfo>)obj;
            }
            else
            {
                DataSet ds = new DataSet();
                ds = csd.GetAllCsKoubei();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        CsKoubeiBaseInfo ckbi = new CsKoubeiBaseInfo();
                        int CsID = BitAuto.Utils.ConvertHelper.GetInteger(dr["CsID"].ToString());
                        decimal Rating = BitAuto.Utils.ConvertHelper.GetDecimal(dr["Rating"].ToString());
                        decimal LevelRating = BitAuto.Utils.ConvertHelper.GetDecimal(dr["LevelRating"].ToString());
                        decimal RatingVariance = BitAuto.Utils.ConvertHelper.GetDecimal(dr["RatingVariance"].ToString());
                        int TotalCount = BitAuto.Utils.ConvertHelper.GetInteger(dr["TotalCount"].ToString());
                        ckbi.CsID = CsID;
                        ckbi.Rating = Rating;
                        ckbi.LevelRating = LevelRating;
                        ckbi.RatingVariance = RatingVariance;
                        ckbi.TotalCount = TotalCount;
                        ckbi.DicSubKoubei = new Dictionary<string, decimal>();
                        decimal KongJian = BitAuto.Utils.ConvertHelper.GetDecimal(dr["KongJian"].ToString());
                        if (!ckbi.DicSubKoubei.ContainsKey("KongJian"))
                        { ckbi.DicSubKoubei.Add("KongJian", KongJian); }
                        decimal DongLi = BitAuto.Utils.ConvertHelper.GetDecimal(dr["DongLi"].ToString());
                        if (!ckbi.DicSubKoubei.ContainsKey("DongLi"))
                        { ckbi.DicSubKoubei.Add("DongLi", DongLi); }
                        decimal CaoKong = BitAuto.Utils.ConvertHelper.GetDecimal(dr["CaoKong"].ToString());
                        if (!ckbi.DicSubKoubei.ContainsKey("CaoKong"))
                        { ckbi.DicSubKoubei.Add("CaoKong", CaoKong); }
                        decimal PeiZhi = BitAuto.Utils.ConvertHelper.GetDecimal(dr["PeiZhi"].ToString());
                        if (!ckbi.DicSubKoubei.ContainsKey("PeiZhi"))
                        { ckbi.DicSubKoubei.Add("PeiZhi", PeiZhi); }
                        decimal ShuShiDu = BitAuto.Utils.ConvertHelper.GetDecimal(dr["ShuShiDu"].ToString());
                        if (!ckbi.DicSubKoubei.ContainsKey("ShuShiDu"))
                        { ckbi.DicSubKoubei.Add("ShuShiDu", ShuShiDu); }
                        decimal XingJiaBi = BitAuto.Utils.ConvertHelper.GetDecimal(dr["XingJiaBi"].ToString());
                        if (!ckbi.DicSubKoubei.ContainsKey("XingJiaBi"))
                        { ckbi.DicSubKoubei.Add("XingJiaBi", XingJiaBi); }
                        decimal WaiGuan = BitAuto.Utils.ConvertHelper.GetDecimal(dr["WaiGuan"].ToString());
                        if (!ckbi.DicSubKoubei.ContainsKey("WaiGuan"))
                        { ckbi.DicSubKoubei.Add("WaiGuan", WaiGuan); }
                        decimal NeiShi = BitAuto.Utils.ConvertHelper.GetDecimal(dr["NeiShi"].ToString());
                        if (!ckbi.DicSubKoubei.ContainsKey("NeiShi"))
                        { ckbi.DicSubKoubei.Add("NeiShi", NeiShi); }
                        decimal YouHao = BitAuto.Utils.ConvertHelper.GetDecimal(dr["YouHao"].ToString());
                        if (!ckbi.DicSubKoubei.ContainsKey("YouHao"))
                        { ckbi.DicSubKoubei.Add("YouHao", YouHao); }
                        if (!dic.ContainsKey(CsID))
                        { dic.Add(CsID, ckbi); }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, 60);
            }
            return dic;
        }

        /// <summary>
        /// 看了还看json
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="count">数量</param>
        /// <param name="size">图片尺寸</param>
        /// <returns></returns>
        public string GetSerialSeeToSeeJson(int serialId, int count, int size = 5)
        {
            string serialToSeeJson = "var serialToSeeJson=[{0}];";
            StringBuilder htmlCode = new StringBuilder();

            List<EnumCollection.SerialToSerial> lsts = new BitAuto.CarChannel.Common.PageBase().GetSerialToSerialByCsID(serialId, count, size);
            if (lsts != null && lsts.Count > 0)
            {
                StringBuilder sbSerialToSerialJson = new StringBuilder();
                foreach (EnumCollection.SerialToSerial serialToSerial in lsts)
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
                    sbSerialToSerialJson.AppendFormat("{{\"serialId\":\"{4}\",\"showName\":\"{0}\",\"price\":\"{1}\",\"allSpell\":\"{2}\",\"img\":\"{3}\"}},"
                        , serialToSerial.ToCsShowName
                        , saleState
                        , serialToSerial.ToCsAllSpell
                        , serialToSerial.ToCsPic
                        , serialToSerial.ToCsID);
                }
                serialToSeeJson = string.Format(serialToSeeJson, sbSerialToSerialJson.ToString().TrimEnd(','));
            }
            else
            {
                serialToSeeJson = string.Format(serialToSeeJson, string.Empty);
            }
            return serialToSeeJson;
        }

        public DataSet GetHotCarTop10()
        {
            DataSet ds = csd.GetHotCarTop10();
            return ds;
        }

        /// <summary>
        /// 获取车系VR url
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetSerialVRUrl()
        {
            Dictionary<int, string> vrDic = null;
            string cacheKey = "Car_SerialBll_GetSerialVRUrl";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                vrDic = (Dictionary<int, string>)obj;
            }
            else
            {
                try
                {
                    string filePath = HttpContext.Current.Server.MapPath("~/config/VR.xml");
                    vrDic = new Dictionary<int, string>();
                    if (File.Exists(filePath))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(filePath);
                        if (xmlDoc != null)
                        {
                            XmlNodeList itemList = xmlDoc.SelectNodes("/Root/serial");
                            if (itemList != null && itemList.Count > 0)
                            {

                                foreach (XmlNode node in itemList)
                                {
                                    int csId = ConvertHelper.GetInteger(node.Attributes["id"].InnerText);
                                    string url = node.Attributes["url"].InnerText;
                                    if (!vrDic.ContainsKey(csId))
                                    {
                                        vrDic.Add(csId, url);
                                    }
                                }
                            }
                        }
                    }
                    CacheManager.InsertCache(cacheKey, vrDic, WebConfig.CachedDuration);
                }
                catch (Exception ex)
                {
                    CommonFunction.WriteLog("解析VR.xml错误：" + ex.ToString());
                }
            }
            return vrDic;
        }



        /// <summary>
        /// 查询车系选配包信息
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public string GetSerialOptionalPackageJson(int serialId)
        {
            DataSet ds = csd.GetSerialOptionalPackage(serialId);
            StringBuilder json = new StringBuilder("[");
            if (ds != null && ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                DataRowCollection package = ds.Tables[0].Rows;
                foreach (DataRow dr in package)
                {
                    DataRow[] carDrs = ds.Tables[1].Select("SerialPackageId=" + dr["autoid"]);
                    string carIds = string.Join(",", carDrs.Select(x => x["carid"]).ToList());
                    json.AppendFormat("{{\"id\":{0},\"csid\":{1},\"name\":\"{2}\",\"price\":{3},\"desc\":\"{4}\",carid:[{5}]}}{6}"
                        , dr["autoid"]
                        , dr["cs_id"]
                        , dr["packagename"]
                        , ConvertHelper.GetString(dr["packageprice"]).Trim()
                        , ConvertHelper.GetString(dr["packagedescription"]).Trim().Replace("\r\n", "")
                        , carIds
                        , package.IndexOf(dr) == package.Count - 1 ? "" : ",");
                }
            }
            json.Append("]");
            return json.ToString();
        }
        /// <summary>
        /// 获取车型选配包数据
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public List<CarSerialPackageEntity> GetCarSerialPackageEntityListBySerialId(int serialId)
        {
            string cacheKey = string.Format(DataCacheKeys.CarSerialPackageKey, serialId);
            var cacheData = CacheManager.GetCachedData<List<CarSerialPackageEntity>>(cacheKey);
            if (cacheData == null)
            {
                cacheData = csd.GetCarSerialPackageListBySerialId(serialId);
                if (cacheData != null)
                    CacheManager.InsertCache(cacheKey, cacheData, 30);
            }
            return cacheData;
        }
        /// <summary>
        /// 获取子品牌幻灯页列表
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public List<SerialFocusImage> GetSerialSlideImageList(int serialId)
        {
            string cacheKey = "serial_slide_image_List_" + serialId;
            List<SerialFocusImage> imgList = (List<SerialFocusImage>)CacheManager.GetCachedData(cacheKey);
            if (imgList == null)
            {
                imgList = new List<SerialFocusImage>();
                XmlDocument doc = GetSerialFocusSlideForNew(serialId);
                XmlNodeList xnl = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
                foreach (XmlElement imgNode in xnl)
                {
                    SerialFocusImage csImg = new SerialFocusImage();
                    csImg.ImageId = ConvertHelper.GetInteger(imgNode.GetAttribute("ImageId"));
                    csImg.ImageName = imgNode.GetAttribute("ImageName");
                    csImg.ImageUrl = imgNode.GetAttribute("ImageUrl");
                    if (csImg.ImageUrl.ToLower().IndexOf("bitautoimg.com") == -1)
                    {
                        csImg.ImageUrl = CommonFunction.GetPublishHashImageDomain(csImg.ImageId) + csImg.ImageUrl;
                    }
                    csImg.TargetUrl = imgNode.GetAttribute("Link");
                    csImg.GroupName = imgNode.GetAttribute("GroupName");
                    csImg.CarName = imgNode.GetAttribute("CarModelName");
                    imgList.Add(csImg);
                }

                CacheManager.InsertCache(cacheKey, imgList, 30);
            }

            return imgList;
        }
        /// <summary>
		/// 子品牌幻灯页图
		/// </summary>
		/// <param name="serialID"></param>
		/// <returns></returns>
		public XmlDocument GetSerialFocusSlideForNew(int serialID)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                string xmlFile = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialSlidePageImagePath, serialID));
                if (File.Exists(xmlFile))
                {
                    doc.Load(xmlFile);
                }
            }
            catch (Exception ex)
            {

            }
            return doc;
        }

        /// <summary>
        /// 获取车款属性
        /// </summary>
        /// <param name="styleId"></param>
        /// <returns></returns>
        public CarStyleInfoEntity GetStyleInfoById(int styleId)
        {

            var cacheKey = string.Format("ycapp.carstyleinfo_{0}", styleId);

            var result = CacheManager.GetCachedData<CarStyleInfoEntity>(cacheKey);
            if (result == null)
            {
                result = csd.GetStyleInfoById(styleId);
                if (result != null)
                {
                    CacheManager.InsertCache(cacheKey, result, 10);
                }

            }
            return result;
        }


        /// <summary>
        /// 	根据车款ID获取相关信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetCarStylePropertyById(int id)
        {
            return csd.GetCarStylePropertyById(id);
        }

        /// <summary>
        /// 转换销售状态枚举类型
        /// </summary>
        /// <param name="CsSaleState"></param>
        /// <returns></returns>
        private int SwitchSaleStatus(string CsSaleState)
        {
            var stateInt = 2;
            switch (CsSaleState)
            {
                case "停销":
                    stateInt = -1;
                    break;
                case "待销":
                    stateInt = 0;
                    break;
                case "在销":
                    stateInt = 1;
                    break;
                case "待查":
                    stateInt = 2;
                    break;
                default:
                    stateInt = 2;
                    break;
            }
            return stateInt;
        }

        private int SwitchNewSaleStatus(string CsSaleState)
        {
            var stateInt = 2;
            switch (CsSaleState)
            {
                case "停销":
                    stateInt = -1;
                    break;
                case "待销":
                    stateInt = 0;
                    break;
                case "在销":
                    stateInt = 1;
                    break;
                case "待查":
                    stateInt = 2;
                    break;
                case "即将上市":
                    stateInt = 100;
                    break;
                case "新车上市":
                    stateInt = 101;
                    break;
                case "新款上市":
                    stateInt = 102;
                    break;
                default:
                    stateInt = 2;
                    break;
            }
            return stateInt;
        }

        /// <summary>
        /// 根据车型ID获取封面图
        /// </summary>
        /// <param name="serialId">车型ID</param>
        /// <returns></returns>
        private string GetImageUrlBySid(int serialId)
        {
            Dictionary<int, XmlElement> urlDic = CarSerialImgUrlService.GetImageUrlDicNew();
            string imgUrl = "";
            if (urlDic.ContainsKey(serialId))
            {
                // modified by chengl Jan.4.2010
                if (urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim() != "")
                {
                    // 有新封面
                    imgUrl = urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim();
                }
                else
                {
                    // 没有新封面
                    if (urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim() != "")
                    {
                        imgUrl = urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim();
                    }
                    else
                    {
                        imgUrl = WebConfig.DefaultCarPic;
                    }
                }
            }
            else
                imgUrl = WebConfig.DefaultCarPic;
            return imgUrl;
        }

        // -1:停销、0:待销、1:在销、2:待查
        /// <summary>
        /// 根据主品牌id获取品牌和车型
        /// </summary>
        /// <param name="masterBrandId">主品牌id</param>
        /// <param name="allSerial">是否是所有车型(包括停销)</param>
        /// <returns></returns>
        public List<CarBrandEntity> GetCarBrandAndSerial(int masterBrandId, bool allSerial)
        {
            if (masterBrandId <= 0)
            {
                return new List<CarBrandEntity>();
            }
            var cacheKey = string.Format("ycapp.carbrandseriallist_{0}", masterBrandId);
            var list = CacheManager.GetCachedData<List<CarBrandEntity>>(cacheKey);
            if (list == null)
            {
                //获取数据xml
                XmlDocument serialXml = AutoStorageService.GetAllAutoXml();
                if (serialXml != null)
                {
                    XmlNode _BrandNode = serialXml.SelectSingleNode(string.Format("Params/MasterBrand[@ID={0}]", masterBrandId.ToString()));
                    if (_BrandNode != null)
                    {
                        XmlNodeList brands = _BrandNode.SelectNodes("Brand");
                        if (brands.Count > 0)
                        {
                            list = new List<CarBrandEntity>();
                            foreach (XmlNode brand in brands)
                            {
                                var serialList = new List<CarSerialEntity>();
                                var serials = brand.ChildNodes;
                                foreach (XmlNode serial in serials)
                                {
                                    serialList.Add(new CarSerialEntity
                                    {
                                        SerialId = ConvertHelper.GetInteger(serial.Attributes["ID"].Value),
                                        serialName = ConvertHelper.GetString(serial.Attributes["ShowName"].Value),
                                        CoverImageUrl = GetImageUrlBySid(ConvertHelper.GetInteger(serial.Attributes["ID"].Value)),
                                        UV = ConvertHelper.GetInteger(serial.Attributes["CsPV"].Value),
                                        SaleStatus = SwitchSaleStatus(ConvertHelper.GetString(serial.Attributes["CsSaleState"].Value)),
                                        NewSaleStatus = SwitchNewSaleStatus(ConvertHelper.GetString(serial.Attributes["CsSaleState"].Value)),
                                        MinPrice = ConvertHelper.GetDecimal(serial.Attributes["MinP"].Value),
                                        MaxPrice = ConvertHelper.GetDecimal(serial.Attributes["MaxP"].Value),
                                        Spell = ConvertHelper.GetString(serial.Attributes["Spell"].Value),
                                        Weight = ConvertHelper.GetInteger(serial.Attributes["Weight"].Value)
                                    });
                                }
                                list.Add(new CarBrandEntity
                                {
                                    BrandId = ConvertHelper.GetInteger(brand.Attributes["ID"].Value),
                                    BrandName = ConvertHelper.GetString(brand.Attributes["Name"].Value),
                                    Foreign = !ConvertHelper.GetString(brand.Attributes["Country"].Value).Equals("国产"),
                                    Weight = ConvertHelper.GetInteger(brand.Attributes["Weight"].Value),
                                    Spell = ConvertHelper.GetString(brand.Attributes["Spell"].Value),
                                    SerialList = serialList.OrderByDescending(x => x.SaleStatus).ThenBy(x => x.Weight).ThenBy(x => x.Spell).ToList()
                                });
                            }
                            list = list.OrderByDescending(x => x.Weight).ThenBy(x => x.Spell).ToList();
                        }
                    }
                }

                foreach (var brand in list)
                {
                    foreach (var serial in brand.SerialList)
                    {
                        if (serial.SaleStatus == 1)
                        {
                            if (serial.MinPrice == 0 && serial.MaxPrice == 0)
                            {
                                serial.DealerPrice = "暂无报价";
                            }
                            else if (serial.MinPrice == 0)
                            {
                                serial.DealerPrice = string.Format("{0}万", serial.MaxPrice > 1000 ? serial.MaxPrice.ToString("F0") : serial.MaxPrice.ToString("F2"));
                            }
                            else
                            {
                                serial.DealerPrice = string.Format("{0}-{1}万", serial.MinPrice > 1000 ? serial.MinPrice.ToString("F0") : serial.MinPrice.ToString("F2"), serial.MaxPrice > 1000 ? serial.MaxPrice.ToString("F0") : serial.MaxPrice.ToString("F2"));
                            }
                        }
                        else if (serial.SaleStatus == 0)
                        {
                            serial.DealerPrice = "未上市";
                        }
                        else if (serial.SaleStatus == -1)
                        {
                            serial.DealerPrice = "停销";
                        }
                    }
                }
                CacheManager.InsertCache(cacheKey, list, 30);//缓存30分钟
            }
            if (!allSerial)
            {
                list.ForEach(l =>
                {
                    l.SerialList.RemoveAll(s => s.SaleStatus < 0); //移除停销车型
                });
            }
            return list;
        }
        #region 获取图片
        /// <summary>
        /// 取xml Document对象，返回DataSet
        /// </summary>
        /// <param name="cacheName">缓存名</param>
        /// <param name="xmlURL">xml 接口地址</param>
        /// <param name="cacheTimeHour">缓存时间(分钟)</param>
        /// <returns></returns>
        public DataSet GetXMLDocToDataSetByURLForCache(string cacheName, string xmlURL, int cacheTimeMin)
        {
            object obj = CacheManager.GetCachedData(cacheName);
            if (obj != null)
            {
                return (DataSet)obj;
            }
            DataSet ds = new DataSet();
            try
            {
                ds.ReadXml(xmlURL);
                CacheManager.InsertCache(cacheName, ds, cacheTimeMin);
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog("PageBase.GetXMLDocToDataSetByURLForCache:" + ex.Message);
            }
            return ds;
        }

        /// <summary>
        /// 取第一张图解
        /// </summary>
        /// <param name="dsCsPic"></param>
        public XmlNode GetFirstTujieImage(DataSet dsCsPic,int serialId)
        {
            XmlElement element = null;
            XmlDocument xmlDoc = new XmlDocument();
            //取图解第一张
            if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("C") && dsCsPic.Tables["C"].Rows.Count > 0)
            {
                var rows = dsCsPic.Tables["C"].Rows.Cast<DataRow>();
                DataRow row = rows.FirstOrDefault(dr => ConvertHelper.GetInteger(dr["P"]) == 12); //dt.Select("P='" + cateId + "'");
                if (row != null)
                {
                    int imgId = row["I"] == DBNull.Value ? 0 : Convert.ToInt32(row["I"]);
                    string imgUrl = row["U"] == DBNull.Value ? "" : Convert.ToString(row["U"]);
                    if (imgId == 0 || imgUrl.Length == 0)
                        imgUrl = WebConfig.DefaultCarPic;
                    else
                        imgUrl = CommonFunction.GetPublishHashImgUrl(4, imgUrl, imgId);
                    string picUrl = "http://photo.bitauto.com/picture/" + serialId + "/" + imgId + "/";
                    element = xmlDoc.CreateElement("CarImage");
                    element.SetAttribute("ImageId", imgId.ToString());
                    element.SetAttribute("ImageUrl", imgUrl);
                    element.SetAttribute("GroupName", "图解");
                    element.SetAttribute("ImageName", "图解");
                    element.SetAttribute("Link", picUrl);
                }
            }
            return (XmlNode)element;
        }
        #endregion
        /// <summary>
        /// 获取车型国别和主品牌id
        /// </summary>
        /// <param name="styleId"></param>
        /// <returns></returns>
        public SerialCountryEntity GetSerialCountryById(int serialId)
        {
            string cacheKey = string.Format(DataCacheKeys.SerialCountry, serialId);
            var result = CacheManager.GetCachedData<SerialCountryEntity>(cacheKey);
            if (result == null)
            {
                result = csd.GetSerialCountryById(serialId);
                CacheManager.InsertCache(cacheKey, result, 60 * 25);
            }
            return result;
        }
    }
}
