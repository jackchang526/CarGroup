using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BitAuto.CarChannel.Common.Cache;
using System.Timers;
using System.IO;
using System.Net;
using System.Web.Caching;
using System.Data;
using BitAuto.CarChannel.Common.Enum;

namespace BitAuto.CarChannel.Common.Interface
{
    public class CSBillboardListService
    {
		#region 私有成员

        private static Timer    CSBillboardListTimer;
        private static string backupPath    = string.Empty;
        private static string dataPath      = string.Empty;

        private static object   objLevelLock;				//CS级别文件锁
        private static string   levelFile;
        
        private static object   objPriceLock;				//CS价格文件锁
        private static string   priceFile;

		#endregion

		/// <summary>
		/// 静态构造函数
		/// </summary>
        static CSBillboardListService()
		{

            dataPath = Path.Combine(WebConfig.DataBlockPath, "Data\\CSBillboardList");
            backupPath = Path.Combine(WebConfig.DataBlockPath, "Data\\Last\\CSBillboardList");

            objPriceLock = new object();
            priceFile = "CSBillboardPriceList.xml";

            objLevelLock = new object();
            levelFile = "CSBillboardLevelList.xml";
		}

        public static void Start()
        {
            //8小时更新一次
            CSBillboardListTimer = new Timer(8 * 60 * 60 * 1000);	
            //CSBillboardListTimer = new Timer(1 * 60 * 60 * 1000);	
            CSBillboardListTimer.Elapsed +=new ElapsedEventHandler(CSBillboardListTimer_Elapsed);
        }

        static void CSBillboardListTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateCSBillboardListFiles();
        }

        private static void UpdateCSBillboardListFiles()
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;

            //CSBillboardPriceList.xml
            string xmlUrl       = "http://carser.bitauto.com/forpicmastertoserial/list/PriceForList.xml";
			string xmlStr       = wc.DownloadString(xmlUrl);
			xmlStr              = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n" + xmlStr;

            string backupFile = Path.Combine(backupPath, priceFile);
            string xmlFile = Path.Combine(dataPath, priceFile);

            lock (objPriceLock)
			{
				//备份
				if(File.Exists(xmlFile))
				{
					File.Copy(xmlFile, backupFile);
				}
				File.WriteAllText(xmlFile, xmlStr);
			}

            //CSBillboardLevelList.xml
            xmlUrl = "http://carser.bitauto.com/forpicmastertoserial/list/LevelForList.xml";
            xmlStr = wc.DownloadString(xmlUrl);
            xmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n" + xmlStr;

            backupFile = Path.Combine(backupPath, levelFile);
            xmlFile = Path.Combine(dataPath, levelFile);

            lock (objLevelLock)
            {
                //备份
                if (File.Exists(xmlFile))
                {
                    File.Copy(xmlFile, backupFile);
                }
                File.WriteAllText(xmlFile, xmlStr);
            }

        }
        
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public static string GetCSBillboardListHTML_Price(bool isSelected)
        {
            string strCSBillboardListHTML_Price = string.Empty;

            string cacheKey = "Car_Serial_BillboardListHTML_Price";
            object objCSBillboardListHTML_Price = CacheManager.GetCachedData(cacheKey);
            if (null == objCSBillboardListHTML_Price)
            {
                XmlDocument doc = new XmlDocument();
                string xmlFile = Path.Combine(dataPath, priceFile);
                if (File.Exists(xmlFile))
                {
                    doc.Load(xmlFile);
                    DataSet dsPriceList = new DataSet();

                    XmlNodeList csList = doc.SelectNodes("/Params/CsPrice");
                    foreach (XmlElement csNode in csList)
                    {
                        if ("无价格" != csNode.Attributes["PriceRange"].Value)
                        {
                            DataTable dtPrice = new DataTable(csNode.Attributes["PriceRange"].Value);
                            dtPrice.Columns.Add(new DataColumn("ShowName", typeof(string)));
                            dtPrice.Columns.Add(new DataColumn("CsAllSpell", typeof(string)));
                            dtPrice.Columns.Add(new DataColumn("CsPV",  typeof(int)));

                            foreach (XmlElement csPriceNode in csNode.ChildNodes)
                            {
                                DataRow dr = dtPrice.NewRow();

                                dr["ShowName"]      = csPriceNode.GetAttribute("ShowName");
                                dr["CsAllSpell"]    = csPriceNode.GetAttribute("CsAllSpell");
                                dr["CsPV"]          = Convert.ToInt32(csPriceNode.GetAttribute("CsPV"));

                                dtPrice.Rows.Add(dr);
                            }
                            
                            dsPriceList.Tables.Add(dtPrice);
                        }
                    }

                    strCSBillboardListHTML_Price = GetRenderedHtml_Price(dsPriceList, isSelected);

                    //加入缓存
                    CacheDependency cacheDependency = new CacheDependency(xmlFile);
                    CacheManager.InsertCache(cacheKey, strCSBillboardListHTML_Price, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            else
                strCSBillboardListHTML_Price = (string)objCSBillboardListHTML_Price;

            return strCSBillboardListHTML_Price;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public static string GetCSBillboardListHTML_Level(bool isSelected)
        {
            string strCSBillboardListHTML_Level = string.Empty;

            string cacheKey = "Car_Serial_BillboardListHTML_Level";
            object objCSBillboardListHTML_Level = CacheManager.GetCachedData(cacheKey);
            if (null == objCSBillboardListHTML_Level)
            {
                XmlDocument doc = new XmlDocument();
                string xmlFile = Path.Combine(dataPath, levelFile);
                if (File.Exists(xmlFile))
                {
                    doc.Load(xmlFile);
                    DataSet dsLevelList = new DataSet();

                    XmlNodeList csList = doc.SelectNodes("/Params/CsLevel");
                    foreach (XmlElement csNode in csList)
                    {
                        if ("其它" != csNode.Attributes["LevelName"].Value)
                        {
                            DataTable dtLevel = new DataTable(csNode.Attributes["LevelName"].Value);
                            dtLevel.Columns.Add(new DataColumn("ShowName", typeof(string)));
                            dtLevel.Columns.Add(new DataColumn("CsAllSpell", typeof(string)));
                            dtLevel.Columns.Add(new DataColumn("CsPV", typeof(int)));

                            foreach (XmlElement csLevelNode in csNode.ChildNodes)
                            {
                                DataRow dr = dtLevel.NewRow();

                                dr["ShowName"] = csLevelNode.GetAttribute("ShowName");
                                dr["CsAllSpell"] = csLevelNode.GetAttribute("CsAllSpell");
                                dr["CsPV"] = Convert.ToInt32(csLevelNode.GetAttribute("CsPV"));

                                dtLevel.Rows.Add(dr);
                            }

                            dsLevelList.Tables.Add(dtLevel);
                        }
                    }

                    strCSBillboardListHTML_Level = GetRenderedHtml_Level(dsLevelList, isSelected);

                    //加入缓存
                    CacheDependency cacheDependency = new CacheDependency(xmlFile);
                    CacheManager.InsertCache(cacheKey, strCSBillboardListHTML_Level, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            else
                strCSBillboardListHTML_Level = (string)objCSBillboardListHTML_Level;

            return strCSBillboardListHTML_Level;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsPrice"></param>
        /// <returns></returns>
        private static string GetRenderedHtml_Level(DataSet dsLevel, bool isSelected)
        {
            StringBuilder strRenderedHtml = new StringBuilder();

            for (int l = 0; l < dsLevel.Tables.Count; l++)
            {
                string strTableName = dsLevel.Tables[l].TableName;

                DataRow[] drs = dsLevel.Tables[l].Select("", "CsPV DESC");

                string strDivCss = (l == 3 || l == 8) ? "line_box a4 a4_last" : "line_box a4";
                string strHCss = (l == 8) ? "r8" : "r" + (l + 1);

                string strSubDivCss =  "Div" + l ;
                if (isSelected == true && l == 0)
                    strSubDivCss = "rank_dep_article_box";
                if (l == 8)
                    strSubDivCss = "Div7";

                if ("SUV" == strTableName || "MPV" == strTableName)
                {
                    bool isWriteHead = false;
                    if ("SUV" == strTableName && isWriteHead == false)
                    {
                        strRenderedHtml.AppendLine(string.Format("<div class='{0}'><h3 class='{1}'><label><span id='spanSUV' onMouseOver='setTab(this.id)' class='r71 on'><a href='{2}'>SUV</a></span><span id='spanMPV' onMouseOver='setTab(this.id)' class='r72'><a href='{3}'>MPV</a></span></label></h3>",
                                                                    strDivCss, strHCss, GetLinkByCarType("SUV"), GetLinkByCarType("MPV")));

                        isWriteHead = true;
                    }

                    strRenderedHtml.AppendLine(string.Format("<div id='{0}' style='{1}'><ol class='hot_ranking'>", "div" + strTableName, ("SUV" == strTableName) ? "" : "display:none;"));
                    
                    int nCount = drs.Length > 10 ? 10 : drs.Length;
                    for (int i = 0; i < nCount; i++)
                    {
                        string strCsLink = string.Format("http://car.bitauto.com/{0}/", Convert.ToString(drs[i]["CsAllSpell"]));
                        string strCsName = Convert.ToString(drs[i]["ShowName"]);

                        if (i != 9)
                            strRenderedHtml.AppendLine(string.Format("<li><a target='_blank' href='{0}'>{1}</a></li>", strCsLink, strCsName));
                        else
                            strRenderedHtml.AppendLine(string.Format("<li style='background: none'><a target='_blank' href='{0}'>{1}</a></li>", strCsLink, strCsName));
                    }

                    strRenderedHtml.AppendLine("</ol></div>");

                    bool isWriteFoot = false;
                    if ("MPV" == strTableName && isWriteFoot == false)
                    {
                        strRenderedHtml.AppendLine("</div>");
                        isWriteFoot = true;
                    }
                }
                else
                {
                    strRenderedHtml.AppendLine(string.Format("<div class='{0}'><h3 class='{1}'><span><a href='{2}'>{3}</a></span></h3><div id='{4}'><ol class='hot_ranking'>", 
                                                                strDivCss, strHCss, GetLinkByCarType(strTableName), strTableName, strSubDivCss));

                    int nCount = drs.Length > 10 ? 10 : drs.Length;
                    for (int i = 0; i < nCount; i++)
                    {
                        string strCsLink = string.Format("http://car.bitauto.com/{0}/", Convert.ToString(drs[i]["CsAllSpell"]));
                        string strCsName = Convert.ToString(drs[i]["ShowName"]);

                        if (i != 9)
                            strRenderedHtml.AppendLine(string.Format("<li><a target='_blank' href='{0}'>{1}</a></li>", strCsLink, strCsName));
                        else
                            strRenderedHtml.AppendLine(string.Format("<li style='background: none'><a target='_blank' href='{0}'>{1}</a></li>", strCsLink, strCsName));
                    }

                    strRenderedHtml.AppendLine("</ol></div></div>");
                }
            }

            strRenderedHtml.AppendLine("<script><!--");

            strRenderedHtml.AppendLine("function setTab(spanID){");
            strRenderedHtml.AppendLine("    var spanSUVMenu = document.getElementById('spanSUV');");
            strRenderedHtml.AppendLine("    var spanMPVMenu = document.getElementById('spanMPV');");
            strRenderedHtml.AppendLine("    if(spanSUVMenu && spanMPVMenu){");
            strRenderedHtml.AppendLine("        if('spanSUV'== spanID){");
            strRenderedHtml.AppendLine("            spanSUVMenu.className = 'r71 on'");
            strRenderedHtml.AppendLine("            divSUV.style.display  = ''");
            strRenderedHtml.AppendLine("            spanMPVMenu.className = 'r72'");
            strRenderedHtml.AppendLine("            divMPV.style.display = 'none'");
            strRenderedHtml.AppendLine("        }");
            strRenderedHtml.AppendLine("        if('spanMPV'== spanID){");
            strRenderedHtml.AppendLine("            spanSUVMenu.className = 'r71'");
            strRenderedHtml.AppendLine("            divSUV.style.display  = 'none'");
            strRenderedHtml.AppendLine("            spanMPVMenu.className = 'r72 on'");
            strRenderedHtml.AppendLine("            divMPV.style.display = ''");
            strRenderedHtml.AppendLine("        }");
            strRenderedHtml.AppendLine("    }");
            strRenderedHtml.AppendLine("}");

            strRenderedHtml.AppendLine("//--></script>");

            return strRenderedHtml.ToString();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsPrice"></param>
        /// <returns></returns>
        private static string GetRenderedHtml_Price(DataSet dsPrice, bool isSelected)
        {
            StringBuilder strRenderedHtml = new StringBuilder();

            for (int l = 0, j = 8; l < dsPrice.Tables.Count; l++, j++)
            {
                string strTableName = dsPrice.Tables[l].TableName;

                DataRow[] drs = dsPrice.Tables[l].Select("", "CsPV DESC");

                string strDivCss = ((l + 1) % 4 == 0) ? "line_box a4 a4_last" : "line_box a4";

                strRenderedHtml.AppendLine(string.Format("<div class='{0}'><h3 class='r6'><span><a href='{1}'>{2}</a></span></h3><div id='Div{3}'><ol class='hot_ranking'>", strDivCss, "#", strTableName, j));

                int nCount = drs.Length > 10 ? 10 : drs.Length;
                for (int i = 0; i < nCount; i++)
                {
                    string strCsLink = string.Format("http://car.bitauto.com/{0}/", Convert.ToString(drs[i]["CsAllSpell"]));
                    string strCsName = Convert.ToString(drs[i]["ShowName"]);

                    if (i != 9)
                        strRenderedHtml.AppendLine(string.Format("<li><a target='_blank' href='{0}'>{1}</a></li>", strCsLink, strCsName));
                    else
                        strRenderedHtml.AppendLine(string.Format("<li style='background: none'><a target='_blank' href='{0}'>{1}</a></li>", strCsLink, strCsName));
                }

                strRenderedHtml.AppendLine("</ol></div></div>");
            }

            return strRenderedHtml.ToString();
        }

        private static string GetLinkByCarType(string strCarType)
        {
            string strLink = string.Empty;

            switch(strCarType)
            {
                case "MPV":
                    strLink = "http://car.bitauto.com/mpv/";
                    break;
                case "SUV":
                    strLink = "http://car.bitauto.com/suv/";
                    break;
                case "豪华车":
                    strLink = "http://car.bitauto.com/haohuaxingche/";
                    break;
                case "紧凑型车":
                    strLink = "http://car.bitauto.com/jincouxingche/";
                    break;
                case "跑车":
                    strLink = "http://car.bitauto.com/paoche/";
                    break;
                case "微型车":
                    strLink = "http://car.bitauto.com/weixingche/";
                    break;
                case "小型车":
                    strLink = "http://car.bitauto.com/xiaoxingche/";
                    break;
                case "中大型车":
                    strLink = "http://car.bitauto.com/zhongdaxingche/";
                    break;
                case "中型车":
                    strLink = "http://car.bitauto.com/zhongxingche/";
                    break;
                default:
                    strLink = string.Empty;
                    break;
            }

            return strLink; 
        }
    }
}
