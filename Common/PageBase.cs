using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml;
using System.Web;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using BitAuto.Utils;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using System.Web.Caching;
using System.Web.UI;
using System.Collections.Specialized;
using BitAuto.Utils.Data;
using BitAuto.CarUtils.Define;

namespace BitAuto.CarChannel.Common
{
    public class PageBase : System.Web.UI.Page
    {
        #region Private Member
        private const string sContentType = "application/x-www-form-urlencoded";
        private const string sRequestEncoding = "UTF-8";
        private const string sResponseEncoding = "UTF-8";
        private int imageRootId = 1;
        #endregion

        // 		(已删除)北斗星		1599
        // 		(已删除)福克斯三厢	1611  X
        // 		(已删除)骊威		1679
        // 		(已删除)骐达		1698
        // 		(已删除)轩逸		1699
        // 		(已删除)新速腾		1765 X
        // 		(已删除)卡罗拉		1879 X
        // 		(已删除)迈腾		1909 X
        // 		(已删除)凯美瑞		1991 X
        // 		(已删除)马自达三厢	2930 X
        // 		(已删除)五菱荣光	2576
        // 		(已删除)悦翔三厢	2633

        // 		众泰5008	2577    9-10  2010全月
        // 		北斗星		1599    9-10  2010全月
        //        五菱荣光 2576    9-10     2010全月
        //        骊威     1679    9       2010全月

        #region 子品牌综述页 百度广告

        /// <summary>
        /// 子品牌右侧百度广告
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [Obsolete("百度热榜合作已经下线")]
        public string GetSerialSummaryRightADByLevel(string level)
        {
            if (string.IsNullOrEmpty(level))
            { return ""; }
            // 默认广告
            string adStr = "<iframe vspace=\"0\" hspace=\"0\" scrolling=\"no\" frameborder=\"0\" id=\"clip\" name=\"clip\" width=\"218\" height=\"156\" src=\" http://top.baidu.com/none/clip_176.html?hd_h_info=1&p_name=%E4%BB%8A%E6%97%A5%E7%83%AD%E6%90%9C%E6%B1%BD%E8%BD%A6%E6%8E%92%E8%A1%8C%E6%A6%9C&width=218&line=4&hd_h=1&hd_trend=0&hd_border=1&lh=30&t3_color=%23CE0907&other_color=%23174785&key_color=%23999999&bc_bg=F7F7F7\" ></iframe>";
            Dictionary<string, string> dicAD = new Dictionary<string, string>();
            string cacheName = "GetSerialSummaryRightADByLevel";
            if (HttpContext.Current.Cache[cacheName] != null)
            {
                dicAD = (Dictionary<string, string>)HttpContext.Current.Cache[cacheName];
            }
            else
            {
                string adFile = HttpContext.Current.Request.MapPath("~/App_Data/ad/CsSummaryRightBaiduAD.xml");
                if (File.Exists(adFile))
                {
                    FileStream stream = null;
                    XmlReader reader = null;
                    try
                    {
                        stream = new FileStream(adFile, FileMode.Open, FileAccess.Read);
                        reader = XmlReader.Create(stream);
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "LevelAD")
                            {
                                string levelName = string.Empty;
                                string ad = string.Empty;
                                while (reader.Read())
                                {
                                    if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "LevelAD")
                                        break;
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        if (reader.Name == "Level")
                                        {
                                            levelName = reader.ReadString().Trim();
                                        }
                                        else if (reader.Name == "AD")
                                        {
                                            ad = reader.ReadString().Trim();
                                            if (!dicAD.ContainsKey(levelName))
                                            { dicAD.Add(levelName, ad); }
                                        }
                                        else { }
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
                System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency(adFile);
                HttpContext.Current.Cache.Insert(cacheName, dicAD, dep, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
            }
            if (dicAD.ContainsKey(level))
            { adStr = dicAD[level]; }
            else
            {
                // 默认
                if (dicAD.ContainsKey("默认"))
                { adStr = dicAD["默认"]; }
            }
            return adStr;
        }

        #endregion

        protected List<int> sideADSerialIdList;
        protected string serialTopAdCode;

        protected override void OnLoad(EventArgs e)
        {
            sideADSerialIdList = new List<int>();
            sideADSerialIdList.AddRange(new int[] { 1599, 2577, 2576, 1679 });
            base.OnLoad(e);
        }

        /// <summary>
        /// 生成子品牌顶部的广告页
        /// </summary>
        /// <param name="serialId"></param>
        protected void MakeSerialTopADCode(int serialId)
        {
            StringBuilder htmlCode = new StringBuilder();
            //// 2010年9-10月广告
            //bool isNeedAD = false;
            //if (serialId == 1599 || serialId == 2577 || serialId == 2576)
            //{
            //    // 众泰5008 北斗星 五菱荣光 9-10月
            //    if (DateTime.Now.Year == 2010 && (DateTime.Now.Month == 9 || DateTime.Now.Month == 10))
            //    {
            //        isNeedAD = true;
            //    }
            //}
            //if (serialId == 1679)
            //{
            //    // 骊威 9月
            //    if (DateTime.Now.Year == 2010 && DateTime.Now.Month == 9)
            //    {
            //        isNeedAD = true;
            //    }
            //}

            //if (sideADSerialIdList.Contains(serialId) && isNeedAD)
            //{
            //    htmlCode.Append("<div class=\"bt_ad_main\">");
            //    htmlCode.Append("<ins id=\"topADLeftFromCar\" type=\"ad_play\" adplay_ip=\"\" adplay_areaname=\"\" adplay_cityname=\"\"");
            //    htmlCode.Append("adplay_brandid=\"" + serialId + "\" adplay_brandname=\"\" adplay_brandtype=\"\" adplay_blockcode=\"80c10c31-34ab-4a36-bde5-549e292c5327\">");
            //    htmlCode.Append("</ins>");
            //    htmlCode.Append("</div>");
            //    htmlCode.Append("<div class=\"bt_ad_side\">");
            //    htmlCode.Append(GetBlockContent(Server.MapPath("~") + "\\ADFile\\" + serialId.ToString() + ".htm"));
            //    //htmlCode.AppendLine("<ins id=\"topADRightFromCar\" type=\"ad_play\" adplay_ip=\"\" adplay_areaname=\"\" adplay_cityname=\"\"");
            //    //htmlCode.AppendLine("adplay_brandid=\"" + serialId + "\" adplay_brandname=\"\" adplay_brandtype=\"\" adplay_blockcode=\"d0c1b90e-7d2a-4fe1-bfc7-693c6ec98486\">");
            //    //htmlCode.AppendLine("</ins>");
            //    htmlCode.Append("</div>");
            //}
            //else
            //{
            htmlCode.Append("<ins id=\"topADLeftFromCar\" type=\"ad_play\" adplay_ip=\"\" adplay_areaname=\"\" adplay_cityname=\"\"");
            htmlCode.Append("adplay_brandid=\"" + serialId + "\" adplay_brandname=\"\" adplay_brandtype=\"\" adplay_blockcode=\"80c10c31-34ab-4a36-bde5-549e292c5327\">");
            htmlCode.Append("</ins>");
            // }
            serialTopAdCode = htmlCode.ToString();
        }

        #region 取级别下新车

        /// <summary>
        /// 取级别下新车
        /// </summary>
        /// <param name="levelID"></param>
        /// <returns></returns>
        public List<EnumCollection.NewCarForLevel> GetLevelNewCarsByLevelID(int levelID)
        {
            List<EnumCollection.NewCarForLevel> lncfl = new List<EnumCollection.NewCarForLevel>();
            //string levelName = Convert.ToString((EnumCollection.SerialLevelEnum)levelID);
            string levelName = CarLevelDefine.GetLevelNameById(levelID);
            // modified by chengl Oct.22.2009
            //if (levelName == "紧凑型")
            //{
            //    levelName = "紧凑型车";
            //}
            //if (levelName == "中大型")
            //{
            //    levelName = "中大型车";
            //}
            //if (levelName == "豪华型")
            //{
            //    levelName = "豪华车";
            //}
            levelName = CarLevelDefine.GetLevelFullName(levelName);
            // modified end
            DataSet ds = this.GetAllNewCars();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" csLevel = '" + levelName + "' ");
                if (drs != null && drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        EnumCollection.NewCarForLevel ncfl = new EnumCollection.NewCarForLevel();
                        ncfl.CarID = int.Parse(dr["car_id"].ToString());
                        ncfl.CarName = dr["car_name"].ToString();
                        ncfl.CsID = int.Parse(dr["cs_id"].ToString());
                        ncfl.CsName = dr["csname"].ToString();
                        ncfl.CsShowName = dr["csshowname"].ToString();
                        ncfl.CsAllSpell = dr["allspell"].ToString().ToLower();

                        lncfl.Add(ncfl);
                    }

                }
            }
            return lncfl;
        }

        #endregion

        #region 取子品牌数据

        /// <summary>
        /// 取访问过的子品牌
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<DataRow> GetVisitSerialByIDs(string ids)
        {
            List<DataRow> dr = new List<DataRow>();
            DataSet ds = GetAllSErialInfo();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ids.IndexOf("|") > 0 || ids.Length > 0)
                {
                    string[] arrIDs = ids.Split('|');
                    if (arrIDs.Length > 0)
                    {
                        int csid = 0;
                        for (int i = 0; i < arrIDs.Length; i++)
                        {
                            if (int.TryParse(arrIDs[i].Replace("id", ""), out csid))
                            {
                                if (csid > 0)
                                {
                                    DataRow[] drs = ds.Tables[0].Select(" cs_id = " + csid.ToString() + " ");
                                    if (drs != null && drs.Length > 0)
                                    {
                                        dr.Add(drs[0]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dr;
        }

        /// <summary>
        /// 取子品牌名片
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public EnumCollection.SerialInfoCard GetSerialInfoCardByCsID(int csid)
        {
            return GetSerialInfoCardByCsID(csid, 0);
        }

        /// <summary>
        /// 取子品牌名片
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public EnumCollection.SerialInfoCard GetSerialInfoCardByCsID(int csid, int carYear)
        {
            DataSet ds = GetAllSErialInfo();
            EnumCollection.SerialInfoCard sic = new EnumCollection.SerialInfoCard();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" cs_id = " + csid.ToString() + " ");
                if (drs != null && drs.Length > 0)
                {
                    // 子品牌基本信息
                    sic.CsID = int.Parse(drs[0]["cs_id"].ToString());
                    sic.CsName = drs[0]["cs_name"].ToString().Trim();
                    sic.CsShowName = drs[0]["cs_ShowName"].ToString().Trim();
                    sic.CsAllSpell = drs[0]["allSpell"].ToString().Trim().ToLower();
                    sic.CsSaleState = drs[0]["CsSaleState"].ToString().Trim();
                    sic.CsLevel = drs[0]["cs_CarLevel"].ToString().Trim();
                    sic.CsBodyForm = drs[0]["CsBodyForm"].ToString().Trim();
                    if (sic.CsSaleState == "停销")
                        sic.CsPriceRange = "停售";
                    else if (sic.CsSaleState == "待销")
                        sic.CsPriceRange = "未上市";
                    else
                        sic.CsPriceRange = this.GetSerialPriceRangeByID(sic.CsID);
                    sic.CsEngine_Exhaust = drs[0]["Engine_Exhaust"].ToString().Trim();
                    sic.OfficialSite = drs[0]["cs_Url"].ToString().Trim();
                    sic.SerialRepairPolicy = drs[0]["CsRepairPolicy"].ToString().Trim();
                    string purposeStr = drs[0]["CsPurpose"].ToString().Trim();
                    sic.CsEngineExhaust = sic.CsEngine_Exhaust;
                    string[] exhaustList = sic.CsEngine_Exhaust.Split(new char[] { '、' }, StringSplitOptions.RemoveEmptyEntries);
                    int counter = 0;
                    Array.Sort(exhaustList);
                    sic.CsEngine_Exhaust = "";
                    foreach (string exhaust in exhaustList)
                    {
                        counter++;
                        sic.CsEngine_Exhaust += "<span>" + exhaust + "</span>";
                        if (counter == 6)
                            break;
                    }
                    if (exhaustList.Length > 6)
                        sic.CsEngine_Exhaust += "<span>" + exhaustList[exhaustList.Length - 1] + "</span>";

                    List<string> transList = new List<string>();
                    string[] arrTra = drs[0]["UnderPan_Num_Type"].ToString().Trim().Split(new char[] { '、' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arrTra.Length > 0)
                    {
                        for (int i = 0; i < arrTra.Length; i++)
                        {
                            string tmpTrans = "手动";
                            if (arrTra[i].IndexOf("手动") == -1)
                                tmpTrans = "自动";
                            if (!transList.Contains(tmpTrans))
                                transList.Add(tmpTrans);
                            if (transList.Count >= 2)
                                break;
                        }
                    }
                    sic.CsTransmissionType = String.Join("、", transList.ToArray());// drs[0]["UnderPan_Num_Type"].ToString().Trim();
                                                                                   //用途
                    string[] purposes = purposeStr.Split(',');
                    sic.PurposeList = new List<string>();
                    foreach (string purIdStr in purposes)
                    {
                        int purId = 0;
                        bool isId = Int32.TryParse(purIdStr, out purId);
                        if (isId)
                        {
                            string purName = CommonFunction.GetPurposeById(purId);
                            if (purName.Length > 0)
                                sic.PurposeList.Add(purName);
                        }
                    }

                    // 子品油耗
                    sic.CsOfficialFuelCost = this.GetSerialPerfFuelCostPer100(sic.CsID, carYear);
                    if (sic.CsSaleState == "停销")
                    {
                        sic.CsSummaryFuelCost = this.GetSerialSummaryFuelLast(sic.CsID);//songcl 2014-11-14
                    }
                    else
                    {
                        sic.CsSummaryFuelCost = this.GetSerialSummaryFuel(sic.CsID, carYear);
                    }
                    sic.CsGuestFuelCost = this.GetSerialDianPingYouHaoByCsID(sic.CsID);
                    // 子品牌业务数量统计
                    sic.CsAskCount = 0;// this.GetSerialAskCountByCsID(sic.CsID);
                    sic.CsDianPingCount = this.GetSerialDianPingCountByCsID(sic.CsID);
                    string defaultPic = "";
                    int picCount = 0;
                    // this.GetSerialPicAndCountByCsID(sic.CsID, out defaultPic, out picCount, false);
                    this.GetSerialPicAndCountByCsID(sic.CsID, out defaultPic, out picCount, true);
                    sic.CsPicCount = picCount;
                    sic.CsDefaultPic = defaultPic;
                    // 子品牌文章link
                    sic.CsNewGouCheShouChe = this.GetCsRainbowAndURLInfo(sic.CsID, 42);
                    sic.CsNewKeJi = this.GetCsRainbowAndURLInfo(sic.CsID, 41);
                    sic.CsNewMaiCheCheShi = drs[0]["bitautoTestURL"].ToString().Trim(); //this.GetCsRainbowAndURLInfo(sic.CsID, 39);
                    sic.CsNewShangShi = this.GetCsRainbowAndURLInfo(sic.CsID, 37);
                    sic.CsNewWeiXiuBaoYang = this.GetCsRainbowAndURLInfo(sic.CsID, 40);
                    sic.CsNewXiaoShouShuJu = "http://car.bitauto.com/" + sic.CsAllSpell + "/xiaoliang/";
                    sic.CsNewYiCheCheShi = this.GetCsRainbowAndURLInfo(sic.CsID, 43);
                    sic.CsNewAnQuan = this.GetCsRainbowAndURLInfo(sic.CsID, 44);
                    sic.CsNewYouHao = "";
                    sic.CsSanBaoLink = this.GetCsRainbowAndURLInfo(sic.CsID, 65);
                }
            }
            return sic;
        }

        /// <summary>
        /// 取主品牌-子品牌(图片&评测下拉列表用)
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialbyMasterForCompareList()
        {
            DataSet ds = new DataSet();
            string catchkey = "GetAllSerialbyMasterForCompareList";
            object getAllSerialbyMasterForCompareList = null;
            CacheManager.GetCachedData(catchkey, out getAllSerialbyMasterForCompareList);
            if (getAllSerialbyMasterForCompareList == null)
            {
                string sql = " select cmb.bs_id,left(cmb.spell,1) + ' ' + cmb.bs_name as bsname,cs.cs_id,cs.cs_name,cs.cs_showname ";
                sql += " from car_serial cs ";
                sql += " left join car_brand cb on cs.cb_id = cb.cb_id ";
                sql += " left join dbo.Car_MasterBrand_Rel cmbr on cb.cb_id = cmbr.cb_id ";
                sql += " left join dbo.Car_MasterBrand cmb on cmbr.bs_id = cmb.bs_id ";
                sql += " where cs.isState=1 and cb.isState=1 and cmb.isState=1 and cs.CsSaleState<>'停销' ";
                sql += " order by cmb.spell,cmb.bs_id,cs.cs_showname ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)getAllSerialbyMasterForCompareList;
            }
            return ds;
        }

        public DataSet GetAllSErialInfo()
        {
            string catchkey = "AllSErialInfo";
            object allSErialInfo = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allSErialInfo);
            if (allSErialInfo == null)
            {
				string sql = " select  cs.cs_id,cs.cs_name,cs.cs_ShowName,cs.allSpell,cs.cs_Virtues,cs.cs_Defect,cs.CsSaleState,cs.cs_CarLevel,cs.CsBodyForm,cs.cs_Url,";
				sql += " cs.CsPurpose,bat.bitautoTestURL,cb.cb_name,csi.Body_Doors,csi.Engine_Exhaust,csi.UnderPan_Num_Type,csi.Car_RepairPolicy CsRepairPolicy ";
                sql += " from dbo.Car_Serial cs ";
                sql += " left join dbo.Car_Serial_Item csi on cs.cs_id = csi.cs_id ";
                sql += " left join dbo.Car_Brand cb on cs.cb_id = cb.cb_id ";
                sql += " left join dbo.BitAutoTest bat on cs.cs_id = bat.cs_id";
                sql += " where cs.isState=1 ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)allSErialInfo;
            }
            return ds;
        }

        #endregion

        #region 取子品牌彩虹条相关信息

        public string GetCsRainbowAndURLInfo(int csID, int rainbowEditID)
        {
            string url = "";
            DataSet ds = GetAllSerialRainbowAndURLInfo();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" csID='" + csID.ToString() + "' and RainbowitemID='" + rainbowEditID.ToString() + "' ");
                if (drs != null && drs.Length > 0)
                {
                    url = drs[0]["url"].ToString();
                }
            }
            return url;
        }

        /// <summary>
        /// 取所有子品牌彩虹条相关信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialRainbowAndURLInfo()
        {
            string catchkey = "GetAllSerialRainbowAndURLInfo";
            object getAllSerialRainbowAndURLInfo = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out getAllSerialRainbowAndURLInfo);
            if (getAllSerialRainbowAndURLInfo == null)
            {
                string sql = " select csID,RainbowitemID,url from dbo.RainbowEdit ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)getAllSerialRainbowAndURLInfo;
            }
            return ds;
        }
        /// <summary>
        /// 获取子品牌论坛链接地址，根据子品牌
        /// </summary>
        /// <param name="serialId">子品牌ID</param>
        /// <returns></returns>
        public DataSet GetBBSLinkBySerialId(int serialId)
        {
            string catchkey = string.Format("GetSerialBBSLink_{0}", serialId);
            object getSerialBBSLink = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out getSerialBBSLink);
            if (getSerialBBSLink == null)
            {
                string sql = "SELECT [RainbowItemID], [csid], [title], [url] FROM [RainbowEdit] WHERE csid=@csid AND RainbowItemID IN (61,62,63,64) ORDER BY RainbowItemID";
                SqlParameter[] parameters = {
                                            new SqlParameter("@csid",SqlDbType.Int)
                                        };
                parameters[0].Value = serialId;
                ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, parameters);
                CacheManager.InsertCache(catchkey, ds, 10);
            }
            else
            {
                ds = (DataSet)getSerialBBSLink;
            }
            return ds;
        }
        #endregion

        #region 根据子品牌取车型数据

        /// <summary>
        /// 取子品牌下车型
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public DataSet GetCarByCsID(int csID)
        {
            DataSet ds = new DataSet();
            string sql = " select car_id,car_Name from dbo.Car_Basic where isState=1 and Car_SaleState<>'停销' and cs_id = @cs_id ";
            SqlParameter[] _param ={
                                      new SqlParameter("@cs_id",SqlDbType.Int)
                                  };
            _param[0].Value = csID;
            try
            {
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// 取子品牌下车型及厂商指导价
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public DataSet GetCarReferPriceByCsID(int csID)
        {
            DataSet ds = new DataSet();
            string sql = " select car_id,car_Name,Car_YearType,car_ReferPrice from dbo.Car_Basic where isState=1 and Car_SaleState<>'停销' and cs_id = @cs_id ORDER BY Car_YearType desc ,car_Name";
            SqlParameter[] _param ={
                                      new SqlParameter("@cs_id",SqlDbType.Int)
                                  };
            _param[0].Value = csID;
            try
            {
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// 取子品牌下车型及厂商指导价(所有或非停销子品牌)
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="isAllSale">是否所有销售状态</param>
        /// <returns></returns>
        public DataSet GetCarReferPriceByCsID(int csID, bool isAllSale)
        {
            DataSet ds = new DataSet();
            string sql = " select car_id,car_Name,Car_YearType,car_ReferPrice from dbo.Car_Basic where isState=1 ";
            if (!isAllSale)
            {
                sql += " and Car_SaleState<>'停销' ";
            }
            sql += " and cs_id = @cs_id ORDER BY Car_YearType desc ,car_Name";
            SqlParameter[] _param ={
                                      new SqlParameter("@cs_id",SqlDbType.Int)
                                  };
            _param[0].Value = csID;
            try
            {
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// 取子品牌热门车型
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public DataSet GetHotCarInfoByCsID(int csID)
        {
            string catchkey = "HotCarInfoByCsID_" + csID.ToString();
            object hotCarInfoByCsID = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out hotCarInfoByCsID);
            if (hotCarInfoByCsID == null)
            {
                string sql = @" SELECT car.car_id, car.car_name, car.car_ReferPrice,car.Car_YearType, ccp.PVSum AS Pv_SumNum
								 FROM   dbo.Car_Basic car WITH ( NOLOCK )
										LEFT JOIN Car_serial cs WITH ( NOLOCK ) ON car.cs_id = cs.cs_id
										LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON car.Car_Id = ccp.CarId
								 WHERE  car.isState = 1
										AND cs.isState = 1
										AND car.Car_SaleState <> '停销'
										AND car.cs_id = @csID
								 ORDER BY Pv_SumNum DESC ";
                SqlParameter[] _param ={
                                      new SqlParameter("@csID",SqlDbType.Int)
                                  };
                _param[0].Value = csID;
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)hotCarInfoByCsID;
            }

            return ds;
        }

        /// <summary>
        /// 取车型信息(子品牌综述页)
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public List<EnumCollection.CarInfoForSerialSummary> GetAllCarInfoForSerialSummaryByCsID(int csID)
        {
            return GetAllCarInfoForSerialSummaryByCsID(csID, false);
        }
        /// <summary>
        /// 取车型全部参数项
        /// </summary>
        /// <param name="carID">车型ID</param>
        /// <returns></returns>
        public Dictionary<int, string> GetCarAllParamByCarID(int carID)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string sql = "select carid,paramid,pvalue from dbo.CarDataBase where carid=@carID";
            SqlParameter[] _param ={
                                      new SqlParameter("@carID",SqlDbType.Int)
                                  };
            _param[0].Value = carID;
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int paramid = 0;
                    if (int.TryParse(dr["paramid"].ToString(), out paramid))
                    {
                        if (paramid > 0 && dr["pvalue"].ToString().Trim() != "" && !dic.ContainsKey(paramid))
                        {
                            dic.Add(paramid, dr["pvalue"].ToString().Trim());
                        }
                    }
                }
            }
            return dic;
        }
        /// <summary>
        /// 取车型信息(子品牌综述页)，add by zhangll Aug.22.2014 按照PC的排序规则取数据
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public List<CarInfoForSerialSummaryEntity> GetAllCarInfoForSerialNewSummaryByCsID(int csID, bool includeStopSale)
        {
            List<CarInfoForSerialSummaryEntity> lcfss = new List<CarInfoForSerialSummaryEntity>();
            DataSet ds = GetAllCarInfoForSerialSummary(includeStopSale);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" cs_id = " + csID.ToString() + " ");
                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        CarInfoForSerialSummaryEntity cfss = new CarInfoForSerialSummaryEntity();
                        cfss.CarID = int.Parse(dr["car_id"].ToString());
                        cfss.CarName = dr["car_name"].ToString();
                        cfss.SaleState = dr["Car_SaleState"].ToString().Trim();
                        if (cfss.SaleState == "停销")
                            cfss.CarPriceRange = "停售";
                        else
                            cfss.CarPriceRange = this.GetCarPriceRangeByID(cfss.CarID);
                        cfss.CarPV = dr["Pv_SumNum"].ToString() == "" ? 0 : int.Parse(dr["Pv_SumNum"].ToString());
                        //cfss.PerfFuelCostPer100 = this.GetCarPerfFuelCostPer100(cfss.CarID);
                        cfss.ReferPrice = dr["car_ReferPrice"].ToString();
                        cfss.TransmissionType = dr["UnderPan_TransmissionType"].ToString();
                        cfss.Engine_Exhaust = dr["Engine_Exhaust"].ToString();
                        cfss.CarYear = dr["Car_YearType"] == DBNull.Value ? "" : dr["Car_YearType"].ToString();
                        cfss.ProduceState = dr["Car_ProduceState"].ToString();

                        #region add by zhangll Aug.20.2014
                        Dictionary<int, string> dictParams = GetCarAllParamByCarID(cfss.CarID);
                        //modified by sk 2013.08.07 进气形式为null/待查/自然吸气的，视为一种分类
                        string inhaleType = string.Empty;
                        if (dictParams.ContainsKey(425))
                        {
                            if (dictParams[425] == "" || dictParams[425] == "待查" || dictParams[425] == "自然吸气") { }
                            else
                                inhaleType = dictParams[425];
                        }
                        //add by sk 2014.3.31 增压方式
                        string addPressType = string.Empty;
                        //if (dictParams.ContainsKey(425))
                        //{
                        //    if (dictParams[425] == "" || dictParams[425] == "待查" || dictParams[425] == "无") { }
                        //    else
                        //        addPressType = dictParams[425];
                        //}
                        int kw = 0;
                        if (dictParams.ContainsKey(430))
                        {
                            double tempkW;
                            double.TryParse(dictParams[430], out tempkW);
                            kw = (int)Math.Round(tempkW);
                        }
                        kw = kw == 0 ? 9999 : kw;

                        #endregion
                        cfss.UnderPan_ForwardGearNum = dictParams.ContainsKey(724) ? dictParams[724] : "";
                        cfss.Engine_MaxPower = kw;
                        cfss.Engine_InhaleType = inhaleType;
                        cfss.Engine_AddPressType = addPressType;
                        cfss.Oil_FuelType = dictParams.ContainsKey(578) ? dictParams[578] : "";//燃料类型
                        lcfss.Add(cfss);
                    }
                }
            }

            return lcfss;
        }

        /// <summary>
        /// 取停销车型信息(停销子品牌综述页)，add by zhangll Aug.22.2014 按照PC的排序规则取数据
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public List<CarInfoForSerialSummaryEntity> GetAllCarInfoForNoSaleSerialNewSummaryByCsID(int csID)
        {
            List<CarInfoForSerialSummaryEntity> lcfss = new List<CarInfoForSerialSummaryEntity>();
            DataSet ds = GetAllCarInfoForNoSaleSerialSummary(csID);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // DataRow[] drs = ds.Tables[0].Select(" cs_id = " + csID.ToString() + " ");
                // if (drs.Length > 0)
                //{
                int newYear = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                // foreach (DataRow dr in drs)
                {
                    int currentYear = 0;
                    if (int.TryParse(dr["Car_YearType"].ToString(), out currentYear))
                    { }
                    if (currentYear < 1)
                    { break; }
                    if (newYear > 0 && currentYear > 0 && newYear != currentYear)
                    { break; }
                    newYear = currentYear;
                    CarInfoForSerialSummaryEntity cfss = new CarInfoForSerialSummaryEntity();
                    cfss.CarID = int.Parse(dr["car_id"].ToString());
                    cfss.CarName = dr["car_name"].ToString();
                    cfss.SaleState = dr["Car_SaleState"].ToString().Trim();
                    cfss.CarPriceRange = "停售";
                    cfss.CarPV = dr["Pv_SumNum"].ToString() == "" ? 0 : int.Parse(dr["Pv_SumNum"].ToString());
                    //cfss.PerfFuelCostPer100 = this.GetCarPerfFuelCostPer100(cfss.CarID);
                    cfss.ReferPrice = dr["car_ReferPrice"].ToString();
                    cfss.TransmissionType = dr["UnderPan_TransmissionType"].ToString();
                    cfss.Engine_Exhaust = dr["Engine_Exhaust"].ToString();
                    cfss.CarYear = dr["Car_YearType"] == DBNull.Value ? "" : dr["Car_YearType"].ToString();
                    cfss.ProduceState = dr["Car_ProduceState"].ToString();

                    #region add by zhangll Aug.20.2014
                    Dictionary<int, string> dictParams = GetCarAllParamByCarID(cfss.CarID);
                    //modified by sk 2013.08.07 进气形式为null/待查/自然吸气的，视为一种分类
                    string inhaleType = string.Empty;
                    if (dictParams.ContainsKey(425))
                    {
                        if (dictParams[425] == "" || dictParams[425] == "待查" || dictParams[425] == "自然吸气") { }
                        else
                            inhaleType = dictParams[425];
                    }
                    //add by sk 2014.3.31 增压方式
                    string addPressType = string.Empty;
                    //if (dictParams.ContainsKey(425))
                    //{
                    //    if (dictParams[425] == "" || dictParams[425] == "待查" || dictParams[425] == "无") { }
                    //    else
                    //        addPressType = dictParams[425];
                    //}
                    int kw = 0;
                    if (dictParams.ContainsKey(430))
                    {
                        double tempkW;
                        double.TryParse(dictParams[430], out tempkW);
                        kw = (int)Math.Round(tempkW);
                    }
                    kw = kw == 0 ? 9999 : kw;

                    #endregion
                    cfss.UnderPan_ForwardGearNum = dictParams.ContainsKey(724) ? dictParams[724] : "";
                    cfss.Engine_MaxPower = kw;
                    cfss.Engine_InhaleType = inhaleType;
                    cfss.Engine_AddPressType = addPressType;
                    cfss.Oil_FuelType = dictParams.ContainsKey(578) ? dictParams[578] : "";//燃料类型
                    lcfss.Add(cfss);
                }
                // }
            }

            return lcfss;
        }
        /// <summary>
        /// 取车型信息(子品牌综述页)
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public List<EnumCollection.CarInfoForSerialSummary> GetAllCarInfoForSerialSummaryByCsID(int csID, bool includeStopSale)
        {
            //string cacheKey = string.Format("PageBase_{0}_{1}", csID, includeStopSale);
            List<EnumCollection.CarInfoForSerialSummary> lcfss = new List<EnumCollection.CarInfoForSerialSummary>();
            DataSet ds = GetAllCarInfoForSerialSummary(includeStopSale);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" cs_id = " + csID.ToString() + " ");
                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        EnumCollection.CarInfoForSerialSummary cfss = new EnumCollection.CarInfoForSerialSummary();
                        cfss.CarID = int.Parse(dr["car_id"].ToString());
                        cfss.CarName = dr["car_name"].ToString();
                        cfss.SaleState = dr["Car_SaleState"].ToString().Trim();
                        if (cfss.SaleState == "停销")
                            cfss.CarPriceRange = "停售";
                        else
                            cfss.CarPriceRange = this.GetCarPriceRangeByID(cfss.CarID);
                        cfss.CarPV = dr["Pv_SumNum"].ToString() == "" ? 0 : int.Parse(dr["Pv_SumNum"].ToString());
                        cfss.PerfFuelCostPer100 = this.GetCarPerfFuelCostPer100(cfss.CarID);
                        cfss.ReferPrice = dr["car_ReferPrice"].ToString();
                        cfss.TransmissionType = dr["UnderPan_TransmissionType"].ToString();
                        cfss.Engine_Exhaust = dr["Engine_Exhaust"].ToString();
                        cfss.CarYear = dr["Car_YearType"] == DBNull.Value ? "" : dr["Car_YearType"].ToString();
                        cfss.ProduceState = dr["Car_ProduceState"].ToString();
                        lcfss.Add(cfss);
                    }
                }
            }

            return lcfss;
        }

        /// <summary>
        /// 取停销车型信息(停销子品牌综述页)
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public List<EnumCollection.CarInfoForSerialSummary> GetAllCarInfoForNoSaleSerialSummaryByCsID(int csID)
        {
            List<EnumCollection.CarInfoForSerialSummary> lcfss = new List<EnumCollection.CarInfoForSerialSummary>();
            DataSet ds = GetAllCarInfoForNoSaleSerialSummary(csID);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // DataRow[] drs = ds.Tables[0].Select(" cs_id = " + csID.ToString() + " ");
                // if (drs.Length > 0)
                //{
                int newYear = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                // foreach (DataRow dr in drs)
                {
                    int currentYear = 0;
                    if (int.TryParse(dr["Car_YearType"].ToString(), out currentYear))
                    { }
                    if (currentYear < 1)
                    { break; }
                    if (newYear > 0 && currentYear > 0 && newYear != currentYear)
                    { break; }
                    newYear = currentYear;
                    EnumCollection.CarInfoForSerialSummary cfss = new EnumCollection.CarInfoForSerialSummary();
                    cfss.CarID = int.Parse(dr["car_id"].ToString());
                    cfss.CarName = dr["car_name"].ToString();
                    cfss.SaleState = dr["Car_SaleState"].ToString().Trim();
                    cfss.CarPriceRange = "停售";
                    cfss.CarPV = dr["Pv_SumNum"].ToString() == "" ? 0 : int.Parse(dr["Pv_SumNum"].ToString());
                    cfss.PerfFuelCostPer100 = this.GetCarPerfFuelCostPer100(cfss.CarID);
                    cfss.ReferPrice = dr["car_ReferPrice"].ToString();
                    cfss.TransmissionType = dr["UnderPan_TransmissionType"].ToString();
                    cfss.Engine_Exhaust = dr["Engine_Exhaust"].ToString();
                    cfss.CarYear = dr["Car_YearType"] == DBNull.Value ? "" : dr["Car_YearType"].ToString();
                    cfss.ProduceState = dr["Car_ProduceState"].ToString();
                    lcfss.Add(cfss);
                }
                // }
            }

            return lcfss;
        }

        /// <summary>
        /// 取所有车型扩展数据(子品牌综述页)
        /// </summary>
        /// <returns></returns>
        private DataSet GetAllCarInfoForSerialSummary()
        {
            return GetAllCarInfoForSerialSummary(false);
        }

        /// <summary>
        /// 取所有车型扩展数据(子品牌综述页)
        /// </summary>
        /// <returns></returns>
        private DataSet GetAllCarInfoForSerialSummary(bool includeStopSale)
        {
            string catchkey = "AllCarInfoForSerialSummary_" + includeStopSale.ToString();
            object allCarInfoForSerialSummary = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allCarInfoForSerialSummary);
            if (allCarInfoForSerialSummary == null)
            {
                string sql = @"SELECT    car.car_id, car.car_name, car.car_ReferPrice, car.Car_YearType,
										car.Car_ProduceState, car.Car_SaleState, cs.cs_id,
										cei.Engine_Exhaust, cei.UnderPan_TransmissionType,
										ccp.PVSum AS Pv_SumNum
							  FROM      dbo.Car_Basic car WITH ( NOLOCK )
										LEFT JOIN dbo.Car_Extend_Item cei WITH ( NOLOCK ) ON car.car_id = cei.car_id
										LEFT JOIN Car_serial cs WITH ( NOLOCK ) ON car.cs_id = cs.cs_id
										LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON car.Car_Id = ccp.CarId
							  WHERE     car.isState = 1
										AND cs.isState = 1 ";
                if (!includeStopSale)
                    sql += " AND car.Car_SaleState <> '停销' ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, WebConfig.CachedDuration);
            }
            else
            {
                ds = (DataSet)allCarInfoForSerialSummary;
            }

            return ds;
        }

        /// <summary>
        /// 取所有停销车型扩展数据(停销子品牌综述页)
        /// </summary>
        /// <returns></returns>
        private DataSet GetAllCarInfoForNoSaleSerialSummary(int csID)
        {
            string catchkey = "AllCarInfoForNoSaleSerialSummary_" + csID.ToString();
            object allCarInfoForNoSaleSerialSummary = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allCarInfoForNoSaleSerialSummary);
            if (allCarInfoForNoSaleSerialSummary == null)
            {
                string sql = @"SELECT  car.car_id, car.car_name, car.car_ReferPrice, car.Car_YearType,
										car.Car_ProduceState, car.Car_SaleState, cs.cs_id, cei.Engine_Exhaust,
										cei.UnderPan_TransmissionType, ccp.PVSum AS Pv_SumNum
								FROM    dbo.Car_Basic car WITH ( NOLOCK )
										LEFT JOIN dbo.Car_Extend_Item cei WITH ( NOLOCK ) ON car.car_id = cei.car_id
										LEFT JOIN Car_serial cs WITH ( NOLOCK ) ON car.cs_id = cs.cs_id
										LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON car.Car_Id = ccp.CarId
								WHERE   car.isState = 1
										AND cs.isState = 1
										AND car.cs_id = @csID
										AND car.Car_SaleState = '停销'
										AND car.Car_YearType > 0
								ORDER BY cs.cs_id, car.Car_YearType DESC";
                SqlParameter[] _params ={
                                      new SqlParameter("@csID",SqlDbType.Int)
                                  };
                _params[0].Value = csID;
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
                CacheManager.InsertCache(catchkey, ds, WebConfig.CachedDuration);
            }
            else
            {
                ds = (DataSet)allCarInfoForNoSaleSerialSummary;
            }
            return ds;
        }


        /// <summary>
        /// 取车型数据(图片对比页)
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarInfoForPhotoCompare()
        {
            string catchkey = "AllCarInfoForPhotoCompare";
            object allCarInfoForPhotoCompare = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allCarInfoForPhotoCompare);
            if (allCarInfoForPhotoCompare == null)
            {
                string sql = " select cr.car_id,cr.car_name,cr.Car_YearType,cs.cs_id,cs.csName,cs.csShowName,cs.cs_seoname ";
                sql += " ,cdb1.pvalue as WheelBase,cdb2.pvalue as Length ";
                sql += " ,cdb3.pvalue as Width,cdb4.pvalue as Height ";
                sql += " from Car_relation cr ";
                sql += " left join Car_Serial cs on cr.cs_id = cs.cs_id ";
                sql += " left join CarDataBase cdb1 on cr.car_id = cdb1.carid and cdb1.paramid = 592 ";
                sql += " left join CarDataBase cdb2 on cr.car_id = cdb2.carid and cdb2.paramid = 588 ";
                sql += " left join CarDataBase cdb3 on cr.car_id = cdb3.carid and cdb3.paramid = 593 ";
                sql += " left join CarDataBase cdb4 on cr.car_id = cdb4.carid and cdb4.paramid = 586 ";
                sql += " where cr.isState=0 and cs.isState=0 ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, WebConfig.CachedDuration);
            }
            else
            {
                ds = (DataSet)allCarInfoForPhotoCompare;
            }

            return ds;
        }

        /// <summary>
        /// 取所有车型ID(参数配置页)
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarIDAndNameForCS()
        {
            string catchkey = "GetAllCarIDAndNameForCS";
            object allCarIDAndNameForCS = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allCarIDAndNameForCS);
            if (allCarIDAndNameForCS == null)
            {
                string sql = " select car.car_id,car.car_name,cs.cs_id ";
                sql += " from dbo.Car_Basic car ";
                sql += " left join Car_Serial cs on car.cs_id = cs.cs_id ";
                sql += " where car.isState=1 and cs.isState=1 ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, WebConfig.CachedDuration);
            }
            else
            {
                ds = (DataSet)allCarIDAndNameForCS;
            }
            return ds;
        }

        /// <summary>
        /// 取子品牌下车型ID(参数配置页)
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="catchMin"></param>
        /// <returns></returns>
        public DataSet GetCarIDAndNameForCS(int csID, int catchMin)
        {
            string catchkey = "GetCarIDAndNameForCS_" + csID.ToString();
            object getCarIDAndNameForCS = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out getCarIDAndNameForCS);
            if (getCarIDAndNameForCS == null)
            {
                //string sql = " select car.car_id,car.car_name,cs.cs_id ";
                //sql += " from dbo.Car_Basic car ";
                //sql += " left join Car_Serial cs on car.cs_id = cs.cs_id ";
                //sql += " where car.isState=1 and cs.isState=1 and cs.cs_id = @csID and car.Car_SaleState<>'停销' order by car.car_ReferPrice desc ";
                // sql += " where car.isState=1 and cs.isState=1 and cs.cs_id = @csID and car.Car_SaleState<>'停销' order by car.car_id desc ";
                string sql = @"SELECT    car.car_id, car.car_name, cs.cs_id, cs_name, car.Car_YearType,
										car.car_ReferPrice,
										cei.UnderPan_TransmissionType AS TransmissionValue,
										( CASE WHEN cei.UnderPan_TransmissionType LIKE '%手动' THEN 1
											   WHEN cei.UnderPan_TransmissionType LIKE '%自动' THEN 2
											   WHEN cei.UnderPan_TransmissionType LIKE '%手自一体' THEN 3
											   WHEN cei.UnderPan_TransmissionType LIKE '%半自动' THEN 4
											   WHEN cei.UnderPan_TransmissionType LIKE '%CVT无级变速'
											   THEN 5
											   WHEN cei.UnderPan_TransmissionType LIKE '%双离合' THEN 6
											   WHEN cei.UnderPan_TransmissionType = '电动车单速变速箱' THEN 7
											   ELSE 9
										  END ) AS TransmissionType, cei.Engine_Exhaust
							  FROM      dbo.Car_Basic car
										LEFT JOIN dbo.Car_Extend_Item cei ON car.car_id = cei.car_id
										LEFT JOIN Car_Serial cs ON car.cs_id = cs.cs_id
							  WHERE     car.isState = 1
										AND cs.isState = 1
										AND cs.cs_id = @csID
										AND car.Car_SaleState <> '停销'
							  ORDER BY  car.Car_YearType DESC, cei.Engine_Exhaust, TransmissionType,
										car.car_ReferPrice";
                SqlParameter[] _param ={
                                      new SqlParameter("@csID",SqlDbType.Int)
                                  };
                _param[0].Value = csID;
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
                CacheManager.InsertCache(catchkey, ds, catchMin);
            }
            else
            {
                ds = (DataSet)getCarIDAndNameForCS;
            }
            return ds;
        }

        /// <summary>
        /// 取停销子品牌下最新年款车型(子品牌参数配置页) 
        /// 高总逻辑 add by chengl May.15.2012
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="catchMin"></param>
        /// <returns></returns>
        public DataSet GetCarIDAndNameForNoSaleCS(int csID, int catchMin)
        {
            string catchkey = "GetCarIDAndNameForNoSaleCS_" + csID.ToString();
            object getCarIDAndNameForNoSaleCS = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out getCarIDAndNameForNoSaleCS);
            if (getCarIDAndNameForNoSaleCS == null)
            {
                string sql = @" SELECT car.car_id, car.car_name, cs.cs_id, cs_name, car.Car_YearType,
										car.car_ReferPrice, cei.UnderPan_TransmissionType AS TransmissionValue,
										( CASE WHEN cei.UnderPan_TransmissionType LIKE '%手动' THEN 1
											   WHEN cei.UnderPan_TransmissionType LIKE '%自动' THEN 2
											   WHEN cei.UnderPan_TransmissionType LIKE '%手自一体' THEN 3
											   WHEN cei.UnderPan_TransmissionType LIKE '%半自动' THEN 4
											   WHEN cei.UnderPan_TransmissionType LIKE '%CVT无级变速' THEN 5
											   WHEN cei.UnderPan_TransmissionType LIKE '%双离合' THEN 6
											   WHEN cei.UnderPan_TransmissionType LIKE '%电动车单速变速箱' THEN 7
											   ELSE 9
										  END ) AS TransmissionType, cei.Engine_Exhaust
								 FROM   dbo.Car_Basic car
										LEFT JOIN dbo.Car_Extend_Item cei ON car.car_id = cei.car_id
										LEFT JOIN Car_Serial cs ON car.cs_id = cs.cs_id
								 WHERE  car.isState = 1
										AND cs.isState = 1
										AND cs.cs_id = @csID
										AND car.Car_YearType > 0
										AND Car_YearType = ( SELECT MAX(Car_YearType)
															 FROM   Car_Basic
															 WHERE  cs_id = @csID
																	AND isState = 1
														   )
								 ORDER BY car.Car_YearType DESC, cei.Engine_Exhaust, TransmissionType,
										car.car_ReferPrice";
                SqlParameter[] _param ={
                                      new SqlParameter("@csID",SqlDbType.Int)
                                  };
                _param[0].Value = csID;
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
                CacheManager.InsertCache(catchkey, ds, catchMin);
            }
            else
            {
                ds = (DataSet)getCarIDAndNameForNoSaleCS;
            }
            return ds;
        }

        /// <summary>
        /// 取子品牌下车型ID(车型图片页 排序:排量、年款、变速器、指导价)
        /// </summary>
        /// <param name="csID"></param>
        /// <param name="catchMin"></param>
        /// <returns></returns>
        public DataSet GetCarIDAndNameForCSOrderByEYTP(int csID, int catchMin)
        {
            string catchkey = "GetCarIDAndNameForCSOrderByEYTP_" + csID.ToString();
            object getCarIDAndNameForCS = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out getCarIDAndNameForCS);
            if (getCarIDAndNameForCS == null)
            {
                string sql = " select car.car_id,car.car_name,cs.cs_id,cs_name,car.Car_YearType,car.car_ReferPrice,cei.UnderPan_TransmissionType as TransmissionValue ";
                sql += " ,(case when cei.UnderPan_TransmissionType like '%手动' then 1 ";
                sql += " when cei.UnderPan_TransmissionType like '%自动' then 2 ";
                sql += " when cei.UnderPan_TransmissionType like '%手自一体' then 3 ";
                sql += " when cei.UnderPan_TransmissionType like '%半自动' then 4 ";
                sql += " when cei.UnderPan_TransmissionType like '%CVT无级变速' then 5 ";
                sql += " when cei.UnderPan_TransmissionType like '%双离合' then 6 ";
                sql += " else 9 end) as TransmissionType,cei.Engine_Exhaust ";
                sql += " from dbo.Car_Basic car ";
                sql += " left join dbo.Car_Extend_Item cei on car.car_id = cei.car_id ";
                sql += " left join Car_Serial cs on car.cs_id = cs.cs_id ";
                sql += " where car.isState=1 and cs.isState=1 and cs.cs_id = @csID ";
                sql += " order by cei.Engine_Exhaust,car.Car_YearType desc,TransmissionType,car.car_ReferPrice ";
                SqlParameter[] _param ={
                                      new SqlParameter("@csID",SqlDbType.Int)
                                  };
                _param[0].Value = csID;
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
                CacheManager.InsertCache(catchkey, ds, catchMin);
            }
            else
            {
                ds = (DataSet)getCarIDAndNameForCS;
            }
            return ds;
        }

        /// <summary>
        /// 取子品牌下车型ID(参数配置页)
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="catchMin"></param>
        /// <returns></returns>
        public DataSet GetCarIDAndNameForCSYear(int csID, int year, int catchMin)
        {
            string catchkey = "GetCarIDAndNameForCS_" + csID.ToString() + "_year_" + year.ToString();
            object getCarIDAndNameForCS = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out getCarIDAndNameForCS);
            if (getCarIDAndNameForCS == null)
            {
                //string sql = " select car.car_id,car.car_name,cs.cs_id ";
                //sql += " from dbo.Car_Basic car ";
                //sql += " left join Car_Serial cs on car.cs_id = cs.cs_id ";
                //sql += " where car.isState=1 and cs.isState=1 and cs.cs_id = @csID and car.Car_SaleState<>'停销' order by car.car_ReferPrice desc ";
                // sql += " where car.isState=1 and cs.isState=1 and cs.cs_id = @csID and car.Car_SaleState<>'停销' order by car.car_id desc ";
                string sql = @"SELECT    car.car_id, car.car_name, cs.cs_id, cs_name, car.Car_YearType,
										car.car_ReferPrice,
										cei.UnderPan_TransmissionType AS TransmissionValue,
										( CASE WHEN cei.UnderPan_TransmissionType LIKE '%手动' THEN 1
											   WHEN cei.UnderPan_TransmissionType LIKE '%自动' THEN 2
											   WHEN cei.UnderPan_TransmissionType LIKE '%手自一体' THEN 3
											   WHEN cei.UnderPan_TransmissionType LIKE '%半自动' THEN 4
											   WHEN cei.UnderPan_TransmissionType LIKE '%CVT无级变速' THEN 5
											   WHEN cei.UnderPan_TransmissionType LIKE '%双离合' THEN 6
											   WHEN cei.UnderPan_TransmissionType LIKE '%电动车单速变速箱' THEN 7
											   ELSE 9
										  END ) AS TransmissionType, cei.Engine_Exhaust
							  FROM      dbo.Car_Basic car
										LEFT JOIN dbo.Car_Extend_Item cei ON car.car_id = cei.car_id
										LEFT JOIN Car_Serial cs ON car.cs_id = cs.cs_id
							  WHERE     car.isState = 1
										AND cs.isState = 1
										AND cs.cs_id = @csID
										AND car.Car_YearType = @year
							  ORDER BY  car.Car_YearType DESC, cei.Engine_Exhaust, TransmissionType,
										car.car_ReferPrice  ";
                SqlParameter[] _param ={
                                      new SqlParameter("@csID",SqlDbType.Int),
                                      new SqlParameter("@year",SqlDbType.Int)
                                  };
                _param[0].Value = csID;
                _param[1].Value = year;
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
                CacheManager.InsertCache(catchkey, ds, catchMin);
            }
            else
            {
                ds = (DataSet)getCarIDAndNameForCS;
            }
            return ds;
        }

        #endregion

        #region 取子品牌近7天PV

        /// <summary>
        /// 取子品牌7天内排名
        /// </summary>
        /// <returns></returns>
        public List<EnumCollection.SerialSortForInterface> GetAllSerialNewly7DayToList()
        {
            List<EnumCollection.SerialSortForInterface> lssfi = new List<EnumCollection.SerialSortForInterface>();
            DataSet ds = GetAllSerialNewly7Day();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    EnumCollection.SerialSortForInterface ssfi = new EnumCollection.SerialSortForInterface();
                    ssfi.CsID = int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString());
                    ssfi.CsName = ds.Tables[0].Rows[i]["cs_name"].ToString();
                    ssfi.CsShowName = ds.Tables[0].Rows[i]["cs_showname"].ToString();
                    ssfi.CsAllSpell = ds.Tables[0].Rows[i]["allspell"].ToString();
                    ssfi.CsLevel = ds.Tables[0].Rows[i]["cs_CarLevel"].ToString();
                    ssfi.CsPV = int.Parse(ds.Tables[0].Rows[i]["Pv_SumNum"].ToString());
                    ssfi.CsPriceRange = GetSerialPriceRange(ssfi.CsID);
                    lssfi.Add(ssfi);
                }
            }
            return lssfi;
        }

        /// <summary>
        /// 子品牌按价格区间取前几个
        /// </summary>
        /// <returns>返回价格区间数组{5万以下,5-8万,8-12万,12-18万,18-25万,25-40万,40-80万,80万以上}</returns>
        public List<EnumCollection.SerialSortForInterface>[] GetAllSerialNewly30DayToPriceRangeList()
        {
            List<EnumCollection.SerialSortForInterface>[] listPriceRange;
            string cacheKey = "GetAllSerialNewly30DayToPriceRangeArrayList";
            object obj = MemCache.GetMemCacheByKey(cacheKey);
            if (obj != null)
            {
                listPriceRange = (List<EnumCollection.SerialSortForInterface>[])obj;
            }
            else
            {
                listPriceRange = new List<EnumCollection.SerialSortForInterface>[8];
                List<EnumCollection.SerialSortForInterface> listSerial = GetAllSerialNewly30DayToList();
                for (int i = 0; i < 8; i++)
                {
                    listPriceRange[i] = new List<EnumCollection.SerialSortForInterface>();
                }
                foreach (EnumCollection.SerialSortForInterface ssfi in listSerial)
                {
                    // 5万以下
                    if (ssfi.CsPriceRange.IndexOf(",1,") >= 0)
                    { listPriceRange[0].Add(ssfi); }
                    // 5-8万
                    if (ssfi.CsPriceRange.IndexOf(",2,") >= 0)
                    { listPriceRange[1].Add(ssfi); }
                    // 8-12万
                    if (ssfi.CsPriceRange.IndexOf(",3,") >= 0)
                    { listPriceRange[2].Add(ssfi); }
                    // 12-18万
                    if (ssfi.CsPriceRange.IndexOf(",4,") >= 0)
                    { listPriceRange[3].Add(ssfi); }
                    // 18-25万
                    if (ssfi.CsPriceRange.IndexOf(",5,") >= 0)
                    { listPriceRange[4].Add(ssfi); }
                    // 25-40万
                    if (ssfi.CsPriceRange.IndexOf(",6,") >= 0)
                    { listPriceRange[5].Add(ssfi); }
                    // 40-80万
                    if (ssfi.CsPriceRange.IndexOf(",7,") >= 0)
                    { listPriceRange[6].Add(ssfi); }
                    // 80万以上
                    if (ssfi.CsPriceRange.IndexOf(",8,") >= 0)
                    { listPriceRange[7].Add(ssfi); }
                }
                // 缓存1天
                MemCache.SetMemCacheByKey(cacheKey, listPriceRange, 1000 * 60 * 60 * 24);
            }
            return listPriceRange;
        }

        /// <summary>
        /// 取子品牌30天内排名
        /// </summary>
        /// <returns></returns>
        public List<EnumCollection.SerialSortForInterface> GetAllSerialNewly30DayToList()
        {
            string cacheKey = "PageBase_GetAllSerialNewly30DayToList";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj == null)
            {
                List<EnumCollection.SerialSortForInterface> lssfi = new List<EnumCollection.SerialSortForInterface>();
                DataSet ds = GetAllSerialNewly30Day();
                // DataSet serialDs = GetAllSErialInfo();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        EnumCollection.SerialSortForInterface ssfi = new EnumCollection.SerialSortForInterface();
                        ssfi.CsID = int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString());
                        //DataRow[] rows = serialDs.Tables[0].Select("cs_id=" + ssfi.CsID);
                        //if (rows.Length > 0)
                        //{
                        //    ssfi.CsName = rows[0]["cs_name"].ToString().Trim();
                        //    ssfi.CsShowName = rows[0]["cs_showname"].ToString().Trim();
                        //    ssfi.CsAllSpell = rows[0]["allspell"].ToString().Trim();
                        //    ssfi.CsLevel = rows[0]["cs_CarLevel"].ToString().Trim();
                        //}
                        //else
                        //    continue;

                        ssfi.CsName = ds.Tables[0].Rows[i]["cs_name"].ToString().Trim();
                        ssfi.CsShowName = ds.Tables[0].Rows[i]["cs_showname"].ToString().Trim();
                        ssfi.CsAllSpell = ds.Tables[0].Rows[i]["allspell"].ToString().Trim();
                        ssfi.CsLevel = ds.Tables[0].Rows[i]["cs_CarLevel"].ToString().Trim();
                        ssfi.CsPV = int.Parse(ds.Tables[0].Rows[i]["Pv_SumNum"].ToString());
                        ssfi.CsPriceRange = GetSerialMultPriceRangeSplitByComma(ssfi.CsID);

                        lssfi.Add(ssfi);
                    }
                    obj = lssfi;
                }
                if (obj == null)
                {
                    obj = new List<EnumCollection.SerialSortForInterface>();
                }

                CacheManager.InsertCache(cacheKey, obj, 60);

            }
            return obj as List<EnumCollection.SerialSortForInterface>;
        }


        public string GetSerialPriceRange(int csID)
        {
            string priceRange = "0";
            string catchkey = "GetSerialPriceRange";
            object getSerialPriceRange = null;
            XmlDocument xmlDoc = new XmlDocument();
            CacheManager.GetCachedData(catchkey, out getSerialPriceRange);
            if (getSerialPriceRange == null)
            {
                xmlDoc.Load(WebConfig.PriceRangeSerial);
                CacheManager.InsertCache(catchkey, xmlDoc, 60);
            }
            else
            {
                xmlDoc = (XmlDocument)getSerialPriceRange;
            }
            if (xmlDoc != null && xmlDoc.HasChildNodes)
            {
                if (xmlDoc.ChildNodes[1] != null && xmlDoc.ChildNodes[1].ChildNodes.Count > 0)
                {
                    for (int i = 0; i < xmlDoc.ChildNodes[1].ChildNodes.Count; i++)
                    {
                        XmlNodeList xnlChild = xmlDoc.ChildNodes[1].ChildNodes[i].ChildNodes;
                        foreach (XmlNode xn in xnlChild)
                        {
                            if (xn.Attributes["ID"].Value == csID.ToString())
                            {
                                priceRange = i.ToString();
                                break;
                            }
                        }
                    }
                }
            }
            return priceRange;
        }

        /// <summary>
        /// 取子品牌的多个价格区间 逗号分隔
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public string GetSerialMultPriceRangeSplitByComma(int csID)
        {
            string multPriceRange = "";
            string cacheKey = "SerialMultPriceRangeSplitByComma";
            Dictionary<int, string> prDic = (Dictionary<int, string>)CacheManager.GetCachedData(cacheKey);
            if (prDic == null)
            {
                prDic = new Dictionary<int, string>();
                XmlDocument mbDoc = AutoStorageService.GetAutoXml();
                if (mbDoc != null && mbDoc.HasChildNodes)
                {
                    XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
                    foreach (XmlElement serialNode in serialNodeList)
                    {
                        int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));
                        prDic[serialId] = serialNode.GetAttribute("MultiPriceRange");
                    }
                }
                CacheDependency cacheDependency = new CacheDependency(WebConfig.AutoDataFile);
                CacheManager.InsertCache(cacheKey, prDic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
            }
            if (prDic.ContainsKey(csID))
                multPriceRange = prDic[csID];
            return multPriceRange;
        }

        public DataSet GetAllSerialNewly7Day()
        {
            string catchkey = "AllSerialNewly7Day";
            object allSerialNewly7Day = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allSerialNewly7Day);
            if (allSerialNewly7Day == null)
            {
                string sql = " select spr.cs_id,spr.Pv_SumNum,cs.cs_CarLevel ";
                sql += " ,cs.cs_name,cs.cs_showname,cs.allspell ";
                sql += " from dbo.Serial_PvRank spr ";
                sql += " left join Car_Serial cs on spr.cs_id = cs.cs_id ";
                sql += " where cs.isState = 1 ";
                sql += " order by spr.Pv_SumNum desc ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)allSerialNewly7Day;
            }
            return ds;
        }

        //获取最近30天的所有子品牌的PV排名
        public DataSet GetAllSerialNewly30Day()
        {
            string catchkey = "AllSerialNewly30Day";
            object allSerialNewly30Day = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allSerialNewly30Day);
            if (allSerialNewly30Day == null)
            {
                string sql = " select t1.cs_ID as cs_ID,t1.uvCount as Pv_SumNum ,cs.cs_CarLevel,cs.allspell,cs.cs_name,cs.cs_showname,cs.cssaleState,cs.cs_seoname from "
                    + " dbo.Car_Serial_30UV t1 "
                    + " left join dbo.car_serial cs on t1.cs_ID=cs.cs_id where cs.isState=1 "
                    + " order by t1.uvCount desc";

                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)allSerialNewly30Day;
            }
            return ds;
        }

        /// <summary>
        /// 取所有包含停销车型的子品牌
        /// </summary>
        /// <returns></returns>
        [Obsolete("Do not call this method.")]
        public bool CheckSerialHasNoSale(int csID)
        {
            bool hasSale = false;
            string catchkey = "GetAllHasNoSaleSerial";
            object getAllHasNoSaleSerial = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out getAllHasNoSaleSerial);
            if (getAllHasNoSaleSerial == null)
            {
                string sql = " select car.cs_id from dbo.Car_Basic car ";
                sql += " left join Car_Serial cs on car.cs_id = cs.cs_id ";
                sql += " left join Car_Brand cb on cs.cb_id = cb.cb_id ";
                sql += " where car.isState=1 and cs.isState=1 and cb.isState=1 and car.Car_SaleState='停销' group by car.cs_id";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)getAllHasNoSaleSerial;
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" cs_id = '" + csID.ToString() + "' ");
                if (drs != null && drs.Length > 0)
                {
                    hasSale = true;
                }
            }
            return hasSale;
        }

        #endregion

        #region 取厂商数据(CMS接口)

        /// <summary>
        /// 取所有厂商简称，LOGO
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllProducerInfoForCMS()
        {
            string catchkey = "GetAllProducerInfoForCMS";
            object getAllProducerInfoForCMS = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out getAllProducerInfoForCMS);
            if (getAllProducerInfoForCMS == null)
            {
                string sql = " select cp.cp_id,cp.Cp_ShortName,(case cp.Cp_Country when '中国' then '国产' else '进口' end) as Country ";
                sql += " from dbo.Car_Producer cp ";
                sql += " where cp.isState=1 ";
                sql += " order by Country,cp.Cp_ShortName ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)getAllProducerInfoForCMS;
            }
            return ds;
        }

        #endregion

        #region 取品牌数据(CMS接口)

        /// <summary>
        /// 取所有品牌名，LOGO
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllBrandInfoForCMS()
        {
            string catchkey = "GetAllBrandInfoForCMS";
            object getAllBrandInfoForCMS = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out getAllBrandInfoForCMS);
            if (getAllBrandInfoForCMS == null)
            {
                string sql = @"SELECT  cb.cb_id, cb.cb_name, allSpell AS cbAllSpell, ( CASE cb.cb_Country
                                                                                          WHEN '中国' THEN '国产'
                                                                                          ELSE '进口'
                                                                                        END ) AS Country
                                FROM dbo.Car_Brand cb
                                WHERE cb.isState = 1
                                ORDER BY Country, cb_name";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)getAllBrandInfoForCMS;
            }
            return ds;
        }

        #endregion

        #region 取车型数据

        public EnumCollection.CarInfoForCarSummary GetCarInfoForCarSummaryByCarID(int carID)
        {
            EnumCollection.CarInfoForCarSummary cfcs = new EnumCollection.CarInfoForCarSummary();
            //DataSet ds = GetCarInfoByCarID(carID, 10);
            DataSet ds = GetAllCarInfo();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select("car_id = " + carID);
                if (drs == null || drs.Length == 0)
                {
                    return cfcs;
                }
                if (drs[0]["cp_id"].ToString() != "")
                {
                    cfcs.CarID = int.Parse(drs[0]["car_id"].ToString());
                    cfcs.CarName = drs[0]["car_name"].ToString();
                    cfcs.CarBodyType = drs[0]["Body_Type"].ToString();
                    cfcs.CarMarketDate = drs[0]["Car_MarketDate"].ToString();
                    cfcs.CarRepairPolicy = drs[0]["Car_RepairPolicy"].ToString();
                    cfcs.ReferPrice = drs[0]["car_ReferPrice"].ToString();
                    cfcs.PerfFuelCostPer100 = this.GetCarPerfFuelCostPer100(cfcs.CarID);
                    cfcs.CarPriceRange = this.GetCarPriceRangeByID(cfcs.CarID);
                    cfcs.TransmissionType = drs[0]["UnderPan_TransmissionType"].ToString();
                    cfcs.CarCpID = int.Parse(drs[0]["cp_id"].ToString());
                    cfcs.CarCpShortName = drs[0]["Cp_ShortName"].ToString();
                    cfcs.CarLevel = drs[0]["cs_CarLevel"].ToString();
                    cfcs.Engine_Exhaust = drs[0]["Engine_Exhaust"].ToString();
                    cfcs.CarSummaryFuelCost = this.GetCarSummaryFuelCost(cfcs.CarID);

                    if (cfcs.PerfFuelCostPer100.Trim().Length > 0)
                        cfcs.PerfFuelCostPer100 += "L";
                    else
                        cfcs.PerfFuelCostPer100 = "无";
                }
            }
            return cfcs;
        }

        /// <summary>
        /// 取所有车型信息(车型综述页)
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarInfo()
        {
            string catchkey = "AllCarInfoForCarSummary";
            object allCarInfoForCarSummary = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allCarInfoForCarSummary);
            if (allCarInfoForCarSummary == null)
            {
                string sql = @"SELECT  car.*, cs.cs_id, cs.cs_name, cs.cs_showname, cs.OldCb_Id, cs.allspell,
                                        cs.cs_seoname, cei.*, ISNULL(cp.cp_id, 0) AS cp_id, cp.Cp_Name,
                                        cp.Cp_ShortName, cs.cs_CarLevel, cb.cb_Country AS Cp_Country
                                FROM    dbo.Car_Basic car
                                        LEFT JOIN dbo.Car_Extend_Item cei ON car.car_id = cei.car_id
                                        LEFT JOIN Car_serial cs ON car.cs_id = cs.cs_id
                                        LEFT JOIN Car_Brand cb ON cs.cb_id = cb.cb_id
                                        LEFT JOIN dbo.Car_Producer cp ON cb.cp_id = cp.cp_id
                                WHERE car.isState = 1
                                        AND cs.isState = 1";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, WebConfig.CachedDuration);
            }
            else
            {
                ds = (DataSet)allCarInfoForCarSummary;
            }
            return ds;
        }

        ///// <summary>
        ///// 取车型数据
        ///// </summary>
        ///// <param name="carID"></param>
        ///// <param name="catchMin"></param>
        ///// <returns></returns>
        //public DataSet GetCarInfoByCarID(int carID, int catchMin)
        //{
        //	string catchkey = "GetCarInfoByCarID_" + carID.ToString();
        //	object getCarInfoByCarID = null;
        //	DataSet ds = new DataSet();
        //	CacheManager.GetCachedData(catchkey, out getCarInfoByCarID);
        //	if (getCarInfoByCarID == null)
        //	{
        //		StringBuilder sql = new StringBuilder();
        //		//string sql = " select car.*,cs.cs_id,cs.cs_name,cs.cs_showname,cs.allspell,cei.*,cp.cp_id,cp.Cp_Name,cp.Cp_ShortName,cs.cs_CarLevel,cp.Cp_Country  ";
        //		sql.Append(" select car.car_id,car.Car_Name,cei.Body_Type,cei.Car_MarketDate,cei.Car_RepairPolicy,car.car_ReferPrice,cei.UnderPan_TransmissionType,cp.cp_id,cp.Cp_ShortName,cs.cs_CarLevel,cei.Engine_Exhaust");
        //		sql.Append(" from dbo.Car_Basic car  ");
        //		sql.Append(" left join dbo.Car_Extend_Item cei on car.car_id = cei.car_id ");
        //		sql.Append(" left join Car_serial cs on car.cs_id = cs.cs_id ");
        //		sql.Append(" left join Car_Brand cb on cs.cb_id = cb.cb_id ");
        //		sql.Append(" left join dbo.Car_Producer cp on cb.cp_id=cp.cp_id ");
        //		sql.Append(" where car.isState=1 and cs.isState=1 and car.car_id = @carID ");
        //		SqlParameter[] _param ={
        //                                    new SqlParameter("@carID",SqlDbType.Int)
        //                                };
        //		_param[0].Value = carID;
        //		ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql.ToString(), _param);
        //		CacheManager.InsertCache(catchkey, ds, catchMin);
        //	}
        //	else
        //	{
        //		ds = (DataSet)getCarInfoByCarID;
        //	}
        //	return ds;
        //}

        /// <summary>
        /// 取所有近新车
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllNewCars()
        {
            string catchkey = "GetAllNewCars";
            object getAllNewCars = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out getAllNewCars);
            if (getAllNewCars == null)
            {
                string sql = " select cr.cs_id,cr.car_id,cr.car_name,cdb1.Pvalue,cdb2.Pvalue,cdb3.Pvalue  ";
                sql += " ,cs.cs_id,cs.csName,cs.csshowname,cs.allspell,cl1.classvalue as csLevel  ";
                sql += " from dbo.Car_relation cr ";
                sql += " left join dbo.CarDataBase cdb1 on cdb1.carid = cr.car_id and cdb1.ParamId=385 ";
                sql += " left join dbo.CarDataBase cdb2 on cdb2.carid = cr.car_id and cdb2.ParamId=384 ";
                sql += " left join dbo.CarDataBase cdb3 on cdb3.carid = cr.car_id and cdb3.ParamId=383 ";
                sql += " left join dbo.Car_Serial cs on cr.cs_id = cs.cs_id ";
                sql += " left join class cl1 on cs.carlevel = cl1.classid ";
                sql += " where cr.IsState=0 and cs.IsState=0 and ";
                sql += " DATEDIFF(day, Convert(datetime,(case cdb1.Pvalue when null then '2000' else ";
                sql += " cdb1.Pvalue end)+'-'+(case cdb2.Pvalue when null then '1' when '0' then '1' else ";
                sql += " cdb2.Pvalue end)+'-'+(case cdb3.Pvalue when null then '1' when '0' then '1' else ";
                sql += " cdb3.Pvalue end)), getdate())<=90 and cdb1.Pvalue <>''  ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, WebConfig.CachedDuration);
            }
            else
            {
                ds = (DataSet)getAllNewCars;
            }
            return ds;
        }

        /// <summary>
        /// 取同子品牌下车型对比(车型参数配置)
        /// </summary>
        /// <param name="carID"></param>
        /// <returns></returns>
        public Hashtable GetCarsTheSameSerialByCarID(int carID, int catchMin)
        {
            Hashtable htTheSame = new Hashtable();
            DataSet ds = GetAllCarInfo();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string csID = "";
                DataRow[] drsCar = ds.Tables[0].Select(" car_id = " + carID.ToString() + " ");
                if (drsCar != null && drsCar.Length > 0)
                {
                    csID = drsCar[0]["cs_id"].ToString();
                    if (!htTheSame.ContainsKey(carID.ToString()))
                    {
                        htTheSame.Add(carID.ToString(), drsCar[0]["car_name"].ToString());
                    }
                }
                if (csID != "")
                {
                    DataRow[] drsTheSame = ds.Tables[0].Select(" cs_id = " + csID + " and Car_SaleState<>'停销' ", "car_ReferPrice DESC");
                    if (drsTheSame != null && drsTheSame.Length > 0)
                    {
                        foreach (DataRow dr in drsTheSame)
                        {
                            if (!htTheSame.ContainsKey(dr["car_id"].ToString()))
                            {
                                htTheSame.Add(dr["car_id"].ToString(), dr["car_name"].ToString());
                            }
                        }
                    }
                }
            }
            return htTheSame;

            #region old for test
            //Hashtable htTheSame = new Hashtable();
            //DataSet ds = GetCarIDAndNameForCS(carID, catchMin);
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    string csID = ds.Tables[0].Rows[0]["cs_id"].ToString();
            //    if (!htTheSame.ContainsKey(carID.ToString()))
            //    {
            //        htTheSame.Add(carID.ToString(), ds.Tables[0].Rows[0]["car_name"].ToString());
            //    }
            //    if (csID != "")
            //    {
            //        DataSet dsOther = GetCarIDAndNameForCS(int.Parse(csID), catchMin);
            //        if (dsOther != null && dsOther.Tables.Count > 0 && dsOther.Tables[0].Rows.Count > 0)
            //        {
            //            for (int i = 0; i < dsOther.Tables[0].Rows.Count; i++)
            //            {
            //                if (dsOther.Tables[0].Rows[i]["car_id"].ToString() == carID.ToString())
            //                {
            //                    continue;
            //                }
            //                if (!htTheSame.ContainsKey(dsOther.Tables[0].Rows[i]["car_id"].ToString()))
            //                {
            //                    htTheSame.Add(dsOther.Tables[0].Rows[i]["car_id"].ToString(), dsOther.Tables[0].Rows[i]["car_name"].ToString());
            //                }
            //            }
            //        }
            //    }
            //}
            //return htTheSame;
            #endregion
        }

        // 取车型参数配置
        public string GetConfigurationNew(int carID, int showType, int operate)
        {
            string result = "";
            StringBuilder sbParameter = new StringBuilder();
            StringBuilder sbConfiguration = new StringBuilder();
            StringBuilder sbForPage = new StringBuilder();

            // 车型XML
            XmlDocument docCar = new XmlDocument();
            CommonFunction cf = new CommonFunction();
            try
            {
                docCar.LoadXml(cf.GetCarInfoForCompare(carID));
            }
            catch { }

            // 车型数据
            if (docCar != null && docCar.HasChildNodes)
            {
                // 参数配置
                if (File.Exists(HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfiguration.config"))
                {
                    XmlDocument docPC = new XmlDocument();
                    docPC.Load(HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfiguration.config");

                    XmlNode rootPC = docPC.DocumentElement;
                    // 显示 参数
                    if (docPC.ChildNodes.Count > 1 && showType != 2)
                    {
                        sbParameter.Append("<div class=\"line_box car_datalist\">");
                        sbParameter.Append("<h3><span>参 数</span><i style=\"font-size:12px; font-weight:normal; font-style:normal\">注：●标配 ○选配 -无</i></h3>");
                        sbParameter.Append("<div class=\"more\"><a target=\"_blank\" href=\"/car/Ajaxnew/ForConfiguration.aspx?carID=" + carID + "&showType=1&operate=0\" class=\"save\">保存</a>");
                        sbParameter.Append("<a target=\"_blank\" href=\"/car/Ajaxnew/ForConfiguration.aspx?carID=" + carID + "&showType=1&operate=1\" class=\"print\">打印</a></div>");
                        sbParameter.Append("</div>");
                        sbParameter.Append("<table width=\"720\" border=\"0\" cellspacing=\"1\" cellpadding=\"0\" class=\"car_datalist_table\">");
                        XmlNode parameter = rootPC.ChildNodes[0];
                        foreach (XmlNode parameterList in parameter)
                        {
                            if (parameterList.NodeType == XmlNodeType.Element)
                            {
                                string block = "";
                                if (parameterList.HasChildNodes && parameterList.Attributes.GetNamedItem("Type").Value.ToString() == "2")
                                {
                                    block += "<tr><td colspan=\"4\"><h4>" + parameterList.Attributes.GetNamedItem("Name").Value + "</h4></td></tr>";
                                    bool isHasChild = false;
                                    int loopCount = 0;
                                    XmlNodeList xmlNode = parameterList.ChildNodes;
                                    foreach (XmlNode item in xmlNode)
                                    {
                                        if (item.NodeType != XmlNodeType.Element)
                                        { continue; }
                                        if (docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value) != null)
                                        {
                                            isHasChild = true || isHasChild;
                                            if (loopCount % 2 == 0)
                                            {
                                                if (loopCount != 0)
                                                {
                                                    block += "</tr>";
                                                }
                                                block += "<tr>";
                                            }
                                            string pvalue = docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value).Attributes.GetNamedItem("PValue").Value + item.Attributes.GetNamedItem("Unit").Value;
                                            if (pvalue.IndexOf("有") == 0)
                                            { pvalue = "●"; }
                                            if (pvalue.IndexOf("选配") == 0)
                                            { pvalue = "○"; }
                                            if (pvalue.IndexOf("无") == 0)
                                            { pvalue = "-"; }
                                            block += "<th width=\"156\">" + item.Attributes.GetNamedItem("Name").Value + "</th>";
                                            block += "<td width=\"201\">" + pvalue + "</td>";
                                            loopCount++;
                                        }
                                    }
                                    if (loopCount % 2 == 1)
                                    {
                                        block += "<th width=\"156\"></th>";
                                        block += "<td width=\"201\"></td>";
                                    }
                                    // 如果有子项
                                    if (isHasChild)
                                    {
                                        sbParameter.Append(block + "</tr>");
                                    }
                                }
                                block = "";
                            }
                        }
                        sbParameter.Append("</table>");
                    }

                    // Response.Write(sbParameter.ToString() + "<br/>-----------------------------------------------------------------");
                    // 显示配置
                    if (docPC.ChildNodes.Count > 1 && showType != 1)
                    {
                        sbConfiguration.Append("<div class=\"line_box car_datalist\">");
                        sbConfiguration.Append("<h3><span>配 置</span><i style=\"font-size:12px; font-weight:normal; font-style:normal\">注：●标配 ○选配 -无</i></h3>");
                        sbConfiguration.Append("<div class=\"more\"><a target=\"_blank\" href=\"/car/Ajaxnew/ForConfiguration.aspx?carID=" + carID + "&showType=2&operate=0\" class=\"save\">保存</a>");
                        sbConfiguration.Append("<a target=\"_blank\" href=\"/car/Ajaxnew/ForConfiguration.aspx?carID=" + carID + "&showType=2&operate=1\" class=\"print\">打印</a></div>");
                        sbConfiguration.Append("</div>");
                        sbConfiguration.Append("<table width=\"720\" border=\"0\" cellspacing=\"1\" cellpadding=\"0\" class=\"car_datalist_table\">");
                        XmlNode parameter = rootPC.ChildNodes[1];
                        foreach (XmlNode parameterList in parameter)
                        {
                            if (parameterList.NodeType == XmlNodeType.Element)
                            {
                                string block = "";
                                // Response.Write(" " + parameterList.Attributes.GetNamedItem("Name").Value + " " + parameterList.Attributes.GetNamedItem("Type").Value + ";<br/>");
                                if (parameterList.HasChildNodes && parameterList.Attributes.GetNamedItem("Type").Value.ToString() == "2")
                                {
                                    block += "<tr><td colspan=\"4\"><h4>" + parameterList.Attributes.GetNamedItem("Name").Value + "</h4></td></tr>";
                                    bool isHasChild = false;
                                    int loopCount = 0;
                                    XmlNodeList xmlNode = parameterList.ChildNodes;
                                    foreach (XmlNode item in xmlNode)
                                    {
                                        if (item.NodeType != XmlNodeType.Element)
                                        { continue; }
                                        if (docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value) != null)
                                        {
                                            isHasChild = true || isHasChild;
                                            if (loopCount % 2 == 0)
                                            {
                                                if (loopCount != 0)
                                                {
                                                    block += "</tr>";
                                                }
                                                block += "<tr>";
                                            }

                                            string pvalue = docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value).Attributes.GetNamedItem("PValue").Value + item.Attributes.GetNamedItem("Unit").Value;
                                            if (pvalue.IndexOf("有") == 0)
                                            { pvalue = "●"; }
                                            if (pvalue.IndexOf("选配") == 0)
                                            { pvalue = "○"; }
                                            if (pvalue.IndexOf("无") == 0)
                                            { pvalue = "-"; }

                                            block += "<th width=\"156\">" + item.Attributes.GetNamedItem("Name").Value + "</th>";
                                            block += "<td width=\"201\">" + pvalue + "</td>";
                                            loopCount++;
                                            //block += "<li><strong>" + item.Attributes.GetNamedItem("Name").Value + "    </strong>";
                                            //block += docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value).Attributes.GetNamedItem("PValue").Value + "</li>";
                                            // Response.Write(docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value).Attributes.GetNamedItem("PValue").Value);
                                        }
                                        // Response.Write(" " + item.Attributes.GetNamedItem("Value").Value + " " + item.Attributes.GetNamedItem("Type").Value + ";<br/>");
                                    }
                                    if (loopCount % 2 == 1)
                                    {
                                        block += "<th width=\"156\"></th>";
                                        block += "<td width=\"201\"></td>";
                                    }
                                    // 如果有子项
                                    if (isHasChild)
                                    {
                                        sbConfiguration.Append(block + "</tr>");
                                    }
                                }
                                block = "";
                            }
                        }
                        sbConfiguration.Append("</table>");
                    }
                    if (operate >= 0)
                    {
                        sbForPage.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                        sbForPage.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
                        sbForPage.Append("<head>");
                        sbForPage.Append("<title>保存参数</title>");
                        sbForPage.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"http://www.bitauto.com/themes/2009/common/css/common.css\" />");
                        sbForPage.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"http://image.bitautoimg.com/uimg/car/css/car2010_0409_1.css\" />");
                        //sbForPage.Append("<link href=\"http://192.168.0.10:8080/ued_file/css/blocking_test.css\" rel=\"stylesheet\" type=\"text/css\" />");
                        //sbForPage.Append("<link href=\"http://192.168.0.10:8080/ued_file/css/reset.css\" rel=\"stylesheet\" type=\"text/css\" />");
                        //sbForPage.Append("<link href=\"http://192.168.0.10:8080/ued_file/_car/style/car2009.css\" rel=\"stylesheet\" type=\"text/css\" />");
                        sbForPage.Append("</head>");
                        sbForPage.Append("<body>");
                        sbForPage.Append("<div id=\"tabadcontent_10\">");
                        sbForPage.Append("{0}");
                        sbForPage.Append("</div>");
                        sbForPage.Append("</body></html>");

                        string jsString = "";
                        if (operate == 0)
                        {
                            jsString = "<script>document.execCommand('Saveas',false,'配置参数.html');</script>";
                        }
                        if (operate == 1)
                        {
                            jsString = "<script>window.print();</script>";
                        }
                        if (showType == 1)
                        {
                            result = (string.Format(sbForPage.ToString(), sbParameter.ToString()) + jsString);
                            // Response.Write(string.Format(sbForPage.ToString(), sbParameter.ToString()) + jsString);
                        }
                        if (showType == 2)
                        {
                            result = (string.Format(sbForPage.ToString(), sbConfiguration.ToString()) + jsString);
                            // Response.Write(string.Format(sbForPage.ToString(), sbConfiguration.ToString()) + jsString);
                        }
                        if (showType == 3)
                        {
                            result = (string.Format(sbForPage.ToString(), sbParameter.ToString() + sbConfiguration.ToString()) + jsString);
                            // Response.Write(string.Format(sbForPage.ToString(), sbParameter.ToString() + sbConfiguration.ToString()) + jsString);
                        }
                    }
                    if (showType == 1 && operate < 0)
                    {
                        result = sbParameter.ToString();
                        // Response.Write(sbParameter.ToString());
                    }
                    if (showType == 2 && operate < 0)
                    {
                        result = sbConfiguration.ToString();
                        // Response.Write(sbConfiguration.ToString());
                    }
                    if (showType == 3 && operate < 0)
                    {
                        result = sbParameter.ToString() + sbConfiguration.ToString();
                        // base.WriteHtmlForConfiguration(sbParameter.ToString() + "" + sbConfiguration.ToString() + m_Bottom, carID);
                        // Response.Write("_");
                        // Response.Write(sbParameter.ToString() + "$" + sbConfiguration.ToString());
                    }
                    return result;
                }
                else
                {
                    return "";
                }
                // Response.Write(sbParameter.ToString() + "<br/>-----------------------------------------------------------------");
                // Response.Write(sbConfiguration.ToString() + "<br/>-----------------------------------------------------------------");
            }
            else
            {
                return "";
            }
        }

        // 取车型综述页参数配置
        public string GetCarConfigurationForCarSummaey(int carID, string name, string allSpell)
        {
            string result = "";
            StringBuilder sbParameter = new StringBuilder(5000);
            StringBuilder sbConfiguration = new StringBuilder(5000);
            StringBuilder sbTemp = new StringBuilder(500);
            // StringBuilder sbForPage = new StringBuilder();
            // 车型XML
            XmlDocument docCar = new XmlDocument();
            CommonFunction cf = new CommonFunction();
            string carContent = cf.GetCarInfoForCompare(carID);
            if (carContent.Trim() == "")
            { return ""; }
            docCar.LoadXml(carContent);

            // 车型数据
            if (docCar != null && docCar.HasChildNodes)
            {
                XmlDocument docPC = new XmlDocument();
                string cache = "CarSummary_ParameterConfiguration";
                object parameterConfiguration = null;
                CacheManager.GetCachedData(cache, out parameterConfiguration);
                if (parameterConfiguration != null)
                {
                    docPC = (XmlDocument)parameterConfiguration;
                }
                else
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfiguration.config"))
                    {
                        docPC.Load(HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfiguration.config");
                        CacheManager.InsertCache(cache, docPC, 60);
                    }
                }

                // 参数配置
                if (docPC != null && docPC.HasChildNodes)
                {
                    // XmlDocument docPC = new XmlDocument();
                    // docPC.Load(HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfiguration.config");

                    XmlNode rootPC = docPC.DocumentElement;
                    // 显示 参数
                    if (docPC.ChildNodes.Count > 1)
                    {
                        sbParameter.AppendLine("<div id=\"DicCarParameter\" class=\"line_box car_config\">");
                        sbParameter.AppendLine("<h3><span>" + name + " 参数</span><!--<i>注：●标配 ○选配 -无</i>--></h3>");
                        sbParameter.AppendLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                        XmlNode parameter = rootPC.ChildNodes[0];
                        foreach (XmlNode parameterList in parameter)
                        {
                            if (parameterList.NodeType == XmlNodeType.Element)
                            {
                                if (parameterList.HasChildNodes && parameterList.Attributes.GetNamedItem("Type").Value.ToString() == "2")
                                {
                                    sbTemp.AppendLine("<thead><tr><th colspan=\"4\">" + parameterList.Attributes.GetNamedItem("Name").Value + "</th></tr></thead><tbody>");
                                    bool isHasChild = false;
                                    int loopCount = 0;
                                    XmlNodeList xmlNode = parameterList.ChildNodes;
                                    foreach (XmlNode item in xmlNode)
                                    {
                                        if (item.NodeType != XmlNodeType.Element)
                                        { continue; }
                                        if (docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value) != null
                                            && docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value).Attributes.GetNamedItem("PValue").Value != "待查")
                                        {
                                            isHasChild = true || isHasChild;
                                            if (loopCount % 2 == 0)
                                            {
                                                if (loopCount != 0)
                                                {
                                                    sbTemp.AppendLine("</tr>");
                                                }
                                                sbTemp.AppendLine("<tr>");
                                            }
                                            string pvalue = docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value).Attributes.GetNamedItem("PValue").Value + item.Attributes.GetNamedItem("Unit").Value;
                                            // 牛B的逻辑不硬编码都不行
                                            // 燃料类型 汽油的话同时显示 燃油标号
                                            string pvalueOther;
                                            if (item.Attributes.GetNamedItem("ParamID").Value == "578"
                                                && pvalue == "汽油")
                                            {
                                                if (docCar.SelectSingleNode("CarParams/Oil_FuelTab") != null
                                            && docCar.SelectSingleNode("CarParams/Oil_FuelTab").Attributes.GetNamedItem("PValue").Value != "待查")
                                                {
                                                    pvalueOther = docCar.SelectSingleNode("CarParams/Oil_FuelTab").Attributes.GetNamedItem("PValue").Value;
                                                    switch (pvalueOther)
                                                    {
                                                        case "90号": pvalueOther = pvalueOther + "(北京89号)"; break;
                                                        case "93号": pvalueOther = pvalueOther + "(北京92号)"; break;
                                                        case "97号": pvalueOther = pvalueOther + "(北京95号)"; break;
                                                        default: break;
                                                    }
                                                    pvalue = pvalue + " " + pvalueOther;
                                                }
                                            }
                                            // 进气型式 如果自然吸气直接显示，如果是增压则显示增压方式
                                            if (item.Attributes.GetNamedItem("ParamID").Value == "425"
                                                && pvalue == "增压")
                                            {
                                                if (docCar.SelectSingleNode("CarParams/Engine_AddPressType") != null
                                            && docCar.SelectSingleNode("CarParams/Engine_AddPressType").Attributes.GetNamedItem("PValue").Value != "待查")
                                                {
                                                    pvalueOther = docCar.SelectSingleNode("CarParams/Engine_AddPressType").Attributes.GetNamedItem("PValue").Value;
                                                    pvalue = pvalue + " " + pvalueOther;
                                                }
                                            }

                                            if (pvalue.IndexOf("有") == 0)
                                            { pvalue = "●"; }
                                            if (pvalue.IndexOf("选配") == 0)
                                            { pvalue = "○"; }
                                            if (pvalue.IndexOf("无") == 0)
                                            { pvalue = "-"; }
                                            sbTemp.AppendLine("<th>" + item.Attributes.GetNamedItem("Name").Value + "</th>");
                                            sbTemp.AppendLine("<td>" + pvalue + "</td>");
                                            loopCount++;
                                        }
                                    }
                                    if (loopCount % 2 == 1)
                                    {
                                        sbTemp.AppendLine("<th></th>");
                                        sbTemp.AppendLine("<td></td>");
                                    }
                                    // 如果有子项
                                    if (isHasChild)
                                    {
                                        sbParameter.AppendLine(sbTemp.ToString() + "</tr></tbody>");
                                    }
                                    if (sbTemp.Length > 0)
                                    { sbTemp.Remove(0, sbTemp.Length); }
                                }
                            }
                        }
                        sbParameter.AppendLine("</table>");
                        sbParameter.AppendLine("<div class=\"more\"><a href=\"/" + allSpell + "/m" + carID.ToString() + "/peizhi/\" target=\"_blank\">对比查看&gt;&gt;</a></div>");
                        sbParameter.AppendLine("</div>");
                    }

                    // 显示配置
                    if (docPC.ChildNodes.Count > 1)
                    {
                        sbConfiguration.Append("<div class=\"line_box car_config\">");
                        sbConfiguration.Append("<h3><span>" + name + " 配置</span><i>注：●标配 ○选配 -无</i></h3>");
                        sbConfiguration.Append("<table cellspacing=\"0\" cellpadding=\"0\">");
                        XmlNode parameter = rootPC.ChildNodes[1];
                        foreach (XmlNode parameterList in parameter)
                        {
                            if (parameterList.NodeType == XmlNodeType.Element)
                            {
                                // string block = "";
                                if (parameterList.HasChildNodes && parameterList.Attributes.GetNamedItem("Type").Value.ToString() == "2")
                                {
                                    sbTemp.AppendLine("<thead><tr><th colspan=\"4\">" + parameterList.Attributes.GetNamedItem("Name").Value + "</th></tr></thead><tbody>");
                                    bool isHasChild = false;
                                    int loopCount = 0;
                                    XmlNodeList xmlNode = parameterList.ChildNodes;
                                    foreach (XmlNode item in xmlNode)
                                    {
                                        if (item.NodeType != XmlNodeType.Element)
                                        { continue; }
                                        if (docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value) != null
                                            && docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value).Attributes.GetNamedItem("PValue").Value != "待查")
                                        {
                                            isHasChild = true || isHasChild;
                                            if (loopCount % 2 == 0)
                                            {
                                                if (loopCount != 0)
                                                {
                                                    sbTemp.AppendLine("</tr>");
                                                }
                                                sbTemp.AppendLine("<tr>");
                                            }

                                            string pvalue = docCar.SelectSingleNode("CarParams/" + item.Attributes.GetNamedItem("Value").Value).Attributes.GetNamedItem("PValue").Value + item.Attributes.GetNamedItem("Unit").Value;
                                            if (pvalue.IndexOf("有") == 0)
                                            { pvalue = "●"; }
                                            if (pvalue.IndexOf("选配") == 0)
                                            { pvalue = "○"; }
                                            if (pvalue.IndexOf("无") == 0)
                                            { pvalue = "-"; }

                                            sbTemp.AppendLine("<th>" + item.Attributes.GetNamedItem("Name").Value + "</th>");
                                            // 车身颜色呈现特殊化
                                            if (item.Attributes.GetNamedItem("Name").Value == "车身颜色")
                                            {
                                                sbTemp.AppendLine("<td colspan=\"3\"><span class=\"c w530\"><!--车身颜色--></span></td>");
                                                loopCount++;
                                            }
                                            else
                                            {
                                                sbTemp.AppendLine("<td>" + pvalue + "</td>");
                                            }
                                            loopCount++;
                                        }
                                    }
                                    if (loopCount % 2 == 1)
                                    {
                                        sbTemp.AppendLine("<th></th>");
                                        sbTemp.AppendLine("<td></td>");
                                    }
                                    // 如果有子项
                                    if (isHasChild)
                                    {
                                        sbConfiguration.AppendLine(sbTemp.ToString() + "</tr></tbody>");
                                    }
                                    if (sbTemp.Length > 0)
                                    { sbTemp.Remove(0, sbTemp.Length); }
                                }
                            }
                        }
                        sbConfiguration.AppendLine("</table>");
                        sbConfiguration.AppendLine("<div class=\"more\"><a href=\"/" + allSpell + "/m" + carID.ToString() + "/peizhi/\" target=\"_blank\">对比查看&gt;&gt;</a></div>");
                        sbConfiguration.AppendLine("</div>");
                    }

                    result = sbParameter.ToString() + sbConfiguration.ToString();
                }
            }
            return result;
        }

        #endregion

        #region 取车型国别、排量、乘员人数(计算工具)

        /// <summary>
        /// 取车型国别、排量、乘员人数
        /// </summary>
        /// <param name="carID"></param>
        /// <param name="isguoChan"></param>
        /// <param name="Engine"></param>
        /// <param name="SeatNum"></param>
        public void GetCarCountryEngineAndSeatNumByCarID(int carID, out bool isguoChan, out int Engine, out int SeatNum, out double referPrice)
        {
            isguoChan = true;
            Engine = 0;
            SeatNum = 0;
            referPrice = 0.0;
            // 国产还是进口
            DataSet dsCarInfo = this.GetAllCarInfo();
            if (dsCarInfo != null && dsCarInfo.Tables.Count > 0 && dsCarInfo.Tables[0].Rows.Count > 0)
            {
                DataRow[] drsCarInfo = dsCarInfo.Tables[0].Select(" car_id = " + carID.ToString() + " ");
                if (drsCarInfo != null && drsCarInfo.Length > 0)
                {
                    if (drsCarInfo[0]["Cp_Country"].ToString() == "中国")
                    { isguoChan = true; }
                    else
                    { isguoChan = false; }

                    //指导价
                    referPrice = ConvertHelper.GetDouble(drsCarInfo[0]["car_ReferPrice"]);
                }
            }

            // 排量
            DataSet dsEngine = this.GetAllCarCountryEngineAndSeatNum(423);
            if (dsEngine != null && dsEngine.Tables.Count > 0 && dsEngine.Tables[0].Rows.Count > 0)
            {
                DataRow[] drsEngine = dsEngine.Tables[0].Select(" carid = " + carID.ToString() + " ");
                if (drsEngine != null && drsEngine.Length > 0)
                {
                    if (int.TryParse(drsEngine[0]["Pvalue"].ToString(), out Engine))
                    {
                    }
                }
            }
            // 座位数
            DataSet dsSeatNum = this.GetAllCarCountryEngineAndSeatNum(665);
            if (dsSeatNum != null && dsSeatNum.Tables.Count > 0 && dsSeatNum.Tables[0].Rows.Count > 0)
            {
                DataRow[] drsSeatNum = dsSeatNum.Tables[0].Select(" carid = " + carID.ToString() + " ");
                if (drsSeatNum != null && drsSeatNum.Length > 0)
                {
                    if (int.TryParse(drsSeatNum[0]["Pvalue"].ToString(), out SeatNum))
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 取所有车型排量、乘员人数
        /// </summary>
        /// <param name="paramID"></param>
        /// <returns></returns>
        private DataSet GetAllCarCountryEngineAndSeatNum(int paramID)
        {
            string catchkey = "AllCarCountryEngineAndSeatNum" + paramID.ToString();
            object allCarCountryEngineAndSeatNum = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allCarCountryEngineAndSeatNum);
            if (allCarCountryEngineAndSeatNum == null)
            {
                // string sql = " select cdb.carid,cdb.ParamId,cdb.Pvalue,cp.CpCountry ";
                string sql = " select cdb.carid,cdb.ParamId,cdb.Pvalue ";
                sql += " from CarDataBase cdb ";
                //sql += " left join dbo.Car_relation car on cdb.carId=car.car_id ";
                //sql += " left join dbo.Car_Serial cs on car.cs_id = cs.cs_id ";
                //sql += " left join dbo.Car_Brand cb on cs.cb_id = cb.cb_id ";
                //sql += " left join dbo.Car_producer cp on cb.cp_id = cp.cp_id ";
                //sql += " where car.isState = 0 and cs.isState = 0  ";
                //sql += " and cdb.ParamId = @ParamId";
                sql += " where cdb.ParamId = @ParamId";
                SqlParameter[] _param ={
                                      new SqlParameter("@ParamId",SqlDbType.Int)
                                  };
                _param[0].Value = paramID;
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)allCarCountryEngineAndSeatNum;
            }
            return ds;
        }

        #endregion

        #region 取油耗

        /// <summary>
        /// 取子品牌官方油耗
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="splitString">返回字符串的分割符</param>
        /// <returns></returns>
        public string GetSerialPerfFuelCostPer100(int csID)
        {
            return GetSerialPerfFuelCostPer100(csID, 0);
        }

        /// <summary>
        /// 取子品牌官方油耗
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="splitString">返回字符串的分割符</param>
        /// <returns></returns>
        public string GetSerialPerfFuelCostPer100(int csID, int carYear)
        {
            DataSet ds = GetAllCarPerfFuelCostPer100();
            return GetSerialFuel(csID, carYear, ds);
        }

        /// <summary>
        /// 获取子品牌的综合工况油耗
        /// </summary>
        /// <param name="csId"></param>
        /// <returns></returns>
        public string GetSerialSummaryFuel(int csId)
        {
            return GetSerialSummaryFuel(csId, 0);
        }

        /// <summary>
        /// 获取子品牌的综合工况油耗
        /// </summary>
        /// <param name="csId"></param>
        /// <param name="carYear"></param>
        /// <returns></returns>
        public string GetSerialSummaryFuel(int csId, int carYear)
        {
            DataSet ds = GetAllCarSummaryFuelCost();
            return GetSerialFuel(csId, carYear, ds);
        }

        /// <summary>
        /// 获取子品牌的油耗信息
        /// </summary>
        /// <param name="csID"></param>
        /// <param name="carYear"></param>
        /// <param name="fuelDs"></param>
        /// <returns></returns>
        private string GetSerialFuel(int csID, int carYear, DataSet fuelDs)
        {
            string result = string.Empty;
            double min = (double)0;
            double max = (double)0;

            if (fuelDs != null && fuelDs.Tables.Count > 0 && fuelDs.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = fuelDs.Tables[0].Select(" cs_id=" + csID.ToString() + " ");
                foreach (DataRow dr in drs)
                {
                    int year = ConvertHelper.GetInteger(dr["Car_YearType"].ToString());
                    if (carYear != 0 && carYear != year)
                        continue;
                    string sPerfFuel = dr["pvalue"].ToString();
                    double iPerfFuel = 0;

                    if (double.TryParse(sPerfFuel, out iPerfFuel))
                    {
                        if (iPerfFuel > 0)
                        {
                            if (min == 0)
                            {
                                min = iPerfFuel;
                            }
                            if (max == 0)
                            {
                                max = iPerfFuel;
                            }
                            if (iPerfFuel < min)
                            {
                                min = iPerfFuel;
                            }
                            if (iPerfFuel > max)
                            {
                                max = iPerfFuel;
                            }
                        }
                    }
                }
                if (min > 0)
                {
                    result = min.ToString(CultureInfo.InvariantCulture) + "";
                }
                if (max > 0)
                {
                    result += "-" + max.ToString(CultureInfo.InvariantCulture) + "L";
                }

                if (min > 0 && max > 0 && min == max)
                {
                    result = max + "L";
                }
            }
            return result;
        }

        /// <summary>
        /// 获取停售车辆年款的综合工况油耗
        /// Author：songcl Date：2014-11-14
        /// </summary>
        /// <param name="id">车款ID</param>
        /// <returns></returns>
        public string GetSerialSummaryFuelLast(int id)
        {
            DataSet ds = GetAllNoSaleSummaryFuelCost();
            return GetFuelLast(id, ds);
        }

        /// <summary>
        /// 获取最新年款的综合工况油耗
        /// Author：songcl Date：2014-11-13
        /// </summary>
        /// <param name="id">车款ID</param>
        /// <param name="ds">所有车型的综合工况油耗</param>
        /// <returns></returns>
        private static string GetFuelLast(int id, DataSet ds)
        {
            var result = string.Empty;
            var rows = ds.Tables[0].Select(" cs_id=" + id);
            var maxYear = rows.Select(row => ConvertHelper.GetInteger(row["Car_YearType"].ToString())).Concat(new[] { 0 }).Max();
            var lastRows = ds.Tables[0].Select(" cs_id=" + id + " and Car_YearType=" + maxYear);
            var min = 0.0;
            var max = 0.0;
            var list = new List<double>();
            foreach (var row in lastRows)
            {
                var current = row["pvalue"].ToString();
                var temp = 0.0;
                if (!double.TryParse(current, out temp)) continue;
                list.Add(temp);
            }


            max = list.Count() > 0 ? list.Max() : max;
            min = list.Count() > 0 ? list.Min() : min;

            if (min > 0 && max > 0 && (min != max))
            {
                result = min + "-" + max + "L";
            }
            if (min > 0 && max > 0 && (min == max))
            {
                result = max + "L";
            }

            return result;
        }

        /// <summary>
        /// 获取车型的综合工况油耗
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        private string GetCarSummaryFuelCost(int carId)
        {
            string fuelStr = "";
            DataSet fuelDs = GetAllCarSummaryFuelCost();
            if (fuelDs != null && fuelDs.Tables.Count > 0 && fuelDs.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = fuelDs.Tables[0].Select(" carid=" + carId + " ");
                if (drs.Length > 0)
                {
                    fuelStr = drs[0]["pvalue"].ToString() + "L";
                }
            }
            return fuelStr;
        }

        /// <summary>
        /// 取车型的百公里油耗
        /// </summary>
        /// <param name="carID">车型ID</param>
        /// <returns></returns>
        public string GetCarPerfFuelCostPer100(int carID)
        {
            string result = string.Empty;
            DataSet ds = GetAllCarPerfFuelCostPer100();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" carid=" + carID.ToString() + " ");
                if (drs.Length > 0)
                {
                    result = drs[0]["pvalue"].ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// 取所有车型的油耗
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarPerfFuelCostPer100()
        {
            string catchkey = "AllCarPerfFuelCostPer100";
            object allCarPerfFuelCostPer100 = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allCarPerfFuelCostPer100);
            if (allCarPerfFuelCostPer100 == null)
            {
                string sql = " select cdb.carid,cdb.paramid,cdb.pvalue,cr.cs_id,cr.Car_YearType ";
                sql += " from dbo.CarDataBase cdb ";
                sql += " left join dbo.Car_relation cr on cdb.carid=cr.car_id ";
                sql += " left join Car_Serial cs on cr.cs_id=cs.cs_id ";
                sql += " where cdb.paramid=658 and cr.isState=0 and cs.isState=0 and cr.car_SaleState<>96";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)allCarPerfFuelCostPer100;
            }

            return ds;
        }

        /// <summary>
        /// 获取所有车型的综合工况油耗
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarSummaryFuelCost()
        {
            string catchkey = "AllCar_Summary_Fuel";
            object allCarPerfFuelCostPer100 = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allCarPerfFuelCostPer100);
            if (allCarPerfFuelCostPer100 == null)
            {
                string sql = " select cdb.carid,cdb.paramid,cdb.pvalue,cr.cs_id,cr.Car_YearType ";
                sql += " from dbo.CarDataBase cdb ";
                sql += " left join dbo.Car_relation cr on cdb.carid=cr.car_id ";
                sql += " left join Car_Serial cs on cr.cs_id=cs.cs_id ";
                sql += " where cdb.paramid=782 and cr.isState=0 and cs.isState=0 and cr.car_SaleState<>96";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)allCarPerfFuelCostPer100;
            }

            return ds;
        }

        /// <summary>
        /// 停售车辆综合油耗
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllNoSaleSummaryFuelCost()
        {
            string catchKey = "NoSale_AllCar_Summary_Fuel";
            object noSalePerfFuelCostPer100 = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchKey, out noSalePerfFuelCostPer100);
            if (noSalePerfFuelCostPer100 == null)
            {
                string sql = " select cdb.carid,cdb.paramid,cdb.pvalue,cr.cs_id,cr.Car_YearType ";
                sql += " from dbo.CarDataBase cdb ";
                sql += " left join dbo.Car_relation cr on cdb.carid=cr.car_id ";
                sql += " left join Car_Serial cs on cr.cs_id=cs.cs_id ";
                sql += " where cdb.paramid=782 and cr.isState=0 and cs.isState=0 and cr.car_SaleState=96";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchKey, ds, 60);
            }
            else
            {
                ds = (DataSet)noSalePerfFuelCostPer100;
            }

            return ds;
        }

        /// <summary>
        /// 获取子品牌的油耗 
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public string GetSerialDianPingYouHaoByCsID(int csID)
        {
            /* old code
			string result = string.Empty;
			double min = (double)0;
			double max = (double)0;
			DataSet ds = GetSerialDianPingYouHao(csID);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				this.GetTableMaxAndMinValue(ds.Tables[0], "value", out min, out max);
			}
			result = min.ToString() + "-" + max.ToString() + "L";
			return result;			 * */

            string fuelStr = "无";
            string cacheKey = "all_serial_netfriend_fuel";
            Dictionary<int, string> fuelDic = (Dictionary<int, string>)CacheManager.GetCachedData(cacheKey);
            if (fuelDic == null)
            {
                string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\AllSerialFuel.xml");
                fuelDic = new Dictionary<int, string>();
                XmlDocument fuelDoc = new XmlDocument();
                //bool netGood = false;
                //try
                //{
                //    fuelDoc.Load(WebConfig.SerialYouHaoRangeNew);
                //    netGood = true;
                //    fuelDoc.Save(xmlFile);
                //}
                //catch { }
                //if (!netGood)
                //{
                //    try
                //    {
                //        if (File.Exists(xmlFile))
                //            fuelDoc.Load(xmlFile);
                //    }
                //    catch { }
                //}
                //modified  by sk 2013.05.07 只从文件中取数据（计划任务维护）
                fuelDoc = CommonFunction.ReadXmlFromFile(xmlFile);
                XmlNodeList serialNodeList = fuelDoc.SelectNodes("/root/model");
                foreach (XmlElement serialNode in serialNodeList)
                {
                    int serialId = ConvertHelper.GetInteger(serialNode.GetAttribute("id"));
                    string tmpFuelStr = serialNode.GetAttribute("fuelvalue");
                    fuelDic[serialId] = tmpFuelStr;
                }
                if (serialNodeList.Count > 0)
                    CacheManager.InsertCache(cacheKey, fuelDic, 30);
            }
            if (fuelDic != null && fuelDic.ContainsKey(csID))
                fuelStr = fuelDic[csID];
            return fuelStr;
        }

        /// <summary>
        /// 取子品牌点评油耗
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public DataSet GetSerialDianPingYouHao(int csID)
        {
            string catchkey = "SerialDianPingYouHao" + csID;
            object serialDianPingYouHao = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out serialDianPingYouHao);
            if (serialDianPingYouHao == null)
            {
                try
                {
                    ds.ReadXml(string.Format(WebConfig.SerialDianPingYouHao, csID.ToString()));
                    CacheManager.InsertCache(catchkey, ds, 60);
                }
                catch { }

            }
            else
            {
                ds = (DataSet)serialDianPingYouHao;
            }

            return ds;
        }

        #endregion

        #region 取子品牌答疑总数

        /// <summary>
        /// 根据子品牌ID取子品牌答疑总数
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public int GetSerialAskCountByCsID(int csID)
        {
            int result = 0;
            DataSet ds = GetAllSerialAskCount();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id"))
            {
                DataRow[] drs = ds.Tables[0].Select(" id=" + csID.ToString() + " ");
                if (drs.Length > 0)
                {
                    string count = drs[0]["count"].ToString();
                    if (int.TryParse(count, out result))
                    { }
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        private DataSet GetAllSerialAskCount()
        {
            string catchkey = "AllSerialAskCount";
            object allSerialAskCount = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allSerialAskCount);
            try
            {
                if (allSerialAskCount == null)
                {
                    try
                    {
                        ds.ReadXml(WebConfig.AllSerialAskCount);
                        CacheManager.InsertCache(catchkey, ds, 60);
                    }
                    catch
                    { }
                }
                else
                {
                    ds = (DataSet)allSerialAskCount;
                }
            }
            catch
            { }
            return ds;
        }

        #endregion

        #region 取子品牌点评数据

        /// <summary>
        /// 取所有子品牌点评总数
        /// </summary>
        /// <returns></returns>
        public DataSet GetSerialDianPingByCsID(int csID)
        {
            string catchkey = "GetSerialDianPingByCsID" + csID.ToString();
            object serialDianPingByCsID = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out serialDianPingByCsID);
            if (serialDianPingByCsID == null)
            {
                try
                {
                    ds.ReadXml(string.Format(WebConfig.SerialKouBeiData, csID.ToString(), 10));
                    CacheManager.InsertCache(catchkey, ds, 120);
                }
                catch
                { }
            }
            else
            {
                ds = (DataSet)serialDianPingByCsID;
            }
            return ds;
        }

        /// <summary>
        /// 取所有子品牌点评总数
        /// </summary>
        /// <returns></returns>
        public string GetSerialDetailDianPingByCsID(int csID, int page, int size)
        {
            string catchkey = "GetSerialDetailDianPingByCsID" + csID.ToString() + "_" + page.ToString() + "_" + size.ToString();
            object GetSerialDetailDianPingByCsID = null;
            string tempDianPing = string.Empty;
            bool isSuccess = false;
            CacheManager.GetCachedData(catchkey, out GetSerialDetailDianPingByCsID);
            if (GetSerialDetailDianPingByCsID == null)
            {
                try
                {
                    tempDianPing = GetRequestString(string.Format(WebConfig.SerialKouBeiDataForCsSummary, csID.ToString(), page, size), 10, out isSuccess);
                    // ds.ReadXml(string.Format(WebConfig.SerialKouBeiDataForCsSummary, csID.ToString(), page, size));
                    if (isSuccess)
                    {
                        CacheManager.InsertCache(catchkey, tempDianPing, 10);
                    }
                }
                catch
                { }
            }
            else
            {
                tempDianPing = Convert.ToString(GetSerialDetailDianPingByCsID);
            }
            return tempDianPing;
        }

        #endregion

        #region 取子品牌点评总数

        /// <summary>
        /// 取子品牌点评总数
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public int GetSerialDianPingCountByCsID(int csID)
        {
            int result = 0;
            DataSet ds = GetAllSerialDianPingCount();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" id=" + csID.ToString() + " ");
                if (drs.Length > 0)
                {
                    string count = drs[0]["topics_count"].ToString();
                    if (int.TryParse(count, out result))
                    { }
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        /// <summary>
        /// 取所有子品牌点评总数
        /// </summary>
        /// <returns></returns>
        private DataSet GetAllSerialDianPingCount()
        {
            string catchkey = "AllSerialDianPingCount";
            object allSerialDianPingCount = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allSerialDianPingCount);
            if (allSerialDianPingCount == null)
            {
                try
                {
                    string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\AllSerialDianpingCount.xml");
                    ds.ReadXml(xmlFile);
                    CacheManager.InsertCache(catchkey, ds, 60);
                }
                catch
                { }
            }
            else
            {
                ds = (DataSet)allSerialDianPingCount;
            }
            return ds;
        }

        #endregion

        #region 取报价(不分地区)

        /// <summary>
        /// 取所有车型报价区间字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllCarPriceRange()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string catchkey = "PageBase_GetAllCarPriceRange";
            object getAllCarPriceRange = null;
            CacheManager.GetCachedData(catchkey, out getAllCarPriceRange);
            if (getAllCarPriceRange == null)
            {
                DataSet ds = GetAllCarPrice();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            int Id = int.Parse(dr["Id"].ToString());
                            decimal min = Math.Round(decimal.Parse(dr["MinPrice"].ToString()), 2);
                            decimal max = Math.Round(decimal.Parse(dr["MaxPrice"].ToString()), 2);
                            if (!dic.ContainsKey(Id))
                            { dic.Add(Id, min.ToString() + "万-" + max.ToString() + "万"); }
                        }
                        catch
                        { }
                    }
                }
                CacheManager.InsertCache(catchkey, dic, WebConfig.CachedDuration);
            }
            else
            {
                dic = (Dictionary<int, string>)getAllCarPriceRange;
            }
            return dic;
        }

        /// <summary>
        /// 取所有子品牌报价区间字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllCsPriceRange()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string catchkey = "PageBase_GetAllCsPriceRange";
            object getAllCsPriceRange = null;
            CacheManager.GetCachedData(catchkey, out getAllCsPriceRange);
            if (getAllCsPriceRange == null)
            {
                DataSet ds = GetAllSerialPrice();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            int Id = int.Parse(dr["Id"].ToString());
                            decimal min = Math.Round(decimal.Parse(dr["MinPrice"].ToString()), 2);
                            decimal max = Math.Round(decimal.Parse(dr["MaxPrice"].ToString()), 2);
                            if (!dic.ContainsKey(Id))
                            {
                                // 100万以上保留整数
                                if (min >= 100)
                                { min = Math.Round(min, 0); }
                                // 100万以上保留整数
                                if (max >= 100)
                                { max = Math.Round(max, 0); }
                                dic.Add(Id, (min == max) ? min + "万" : string.Format("{0}-{1}万", min, max));
                            }
                        }
                        catch
                        { }
                    }
                }
                CacheManager.InsertCache(catchkey, dic, WebConfig.CachedDuration);
            }
            else
            {
                dic = (Dictionary<int, string>)getAllCsPriceRange;
            }
            return dic;
        }

        /// <summary>
        /// 取车型报价区间(不分地区)
        /// </summary>
        /// <param name="carID">车型ID</param>
        /// <returns></returns>
        public string GetCarPriceRangeByID(int carID)
        {
            string result = string.Empty;
            DataSet ds = GetAllCarPrice();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" Id='" + carID.ToString() + "' ");
                if (drs.Length > 0)
                {
                    try
                    {
                        decimal min = Math.Round(decimal.Parse(drs[0]["MinPrice"].ToString()), 2);
                        // 100万以上保留整数
                        if (min >= 100)
                        { min = Math.Round(min, 0); }
                        decimal max = Math.Round(decimal.Parse(drs[0]["MaxPrice"].ToString()), 2);
                        // 100万以上保留整数
                        if (max >= 100)
                        { max = Math.Round(max, 0); }
                        result = (min == max) ? min + "万" : string.Format("{0}-{1}万", min, max);
                    }
                    catch
                    { }
                }
            }
            return result;
        }
        /// <summary>
        /// 取车型参考成交价（非区间）
        /// </summary>
        /// <param name="carID">车型ID</param>
        /// <returns></returns>
        public string GetCarPriceByID(int carID)
        {
            string result = string.Empty;
            DataSet ds = GetAllCarPrice();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" Id='" + carID.ToString() + "' ");
                if (drs.Length > 0)
                {
                    try
                    {
                        decimal price = Math.Round(decimal.Parse(drs[0]["MinPrice"].ToString()), 2);
                        // 100万以上保留整数
                        if (price >= 100)
                        { price = Math.Round(price, 0); }
                        result = price.ToString() + "万";
                    }
                    catch
                    { }
                }
            }
            return result;
        }
        /// <summary>
        /// 取子品牌报价区间(不分地区)
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public string GetSerialPriceRangeByID(int csID)
        {
            string result = string.Empty;
            DataSet ds = GetAllSerialPrice();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" Id=" + csID.ToString() + " ");
                if (drs.Length > 0)
                {
                    try
                    {
                        decimal min = Math.Round(decimal.Parse(drs[0]["MinPrice"].ToString()), 2);
                        // 100万以上保留整数
                        if (min >= 100)
                        { min = Math.Round(min, 0); }
                        decimal max = Math.Round(decimal.Parse(drs[0]["MaxPrice"].ToString()), 2);
                        // 100万以上保留整数
                        if (max >= 100)
                        { max = Math.Round(max, 0); }

                        if (min == max)
                        {
                            result = min + "万";
                        }
                        else
                        {
                            result = string.Format("{0}-{1}万", min, max);
                        }
                    }
                    catch
                    { }
                }
            }
            return result;
        }
        /// <summary>
        /// 取子品牌报价区间(不分地区)
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public string GetSerialIntPriceRangeByID(int csID)
        {
            string result = string.Empty;
            DataSet ds = GetAllSerialPrice();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" Id=" + csID.ToString() + " ");
                if (drs.Length > 0)
                {
                    try
                    {
                        int min = (int)Math.Round(decimal.Parse(drs[0]["MinPrice"].ToString()), 0);
                        int max = (int)Math.Round(decimal.Parse(drs[0]["MaxPrice"].ToString()), 0);
                        result = min.ToString() + "万-" + max.ToString() + "万";
                    }
                    catch
                    { }
                }
            }
            return result;
        }

        /// <summary>
        /// 取所有车型的报价(不分地区)
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarPrice()
        {
            DataSet ds = new DataSet();
            try
            {
                string catchkey = "AllCarPriceNoZone";
                object allCarPriceNoZone = null;
                CacheManager.GetCachedData(catchkey, out allCarPriceNoZone);
                if (allCarPriceNoZone == null)
                {
                    //ds.ReadXml(WebConfig.AllCarPriceNoZone);
                    XmlDocument xmlDoc = CommonFunction.ReadXmlFromUrl(WebConfig.AllCarPriceNoZone, 60000);
                    if (xmlDoc != null && xmlDoc.HasChildNodes)
                    {
                        ds.ReadXml(new StringReader(xmlDoc.InnerXml));
                        CacheManager.InsertCache(catchkey, ds, 60);
                    }
                    else
                    {
                        CommonFunction.WriteLog("车款报价区间接口 读取或数据异常");
                        ds = GetLocalCarPriceRange();
                    }
                }
                else
                {
                    ds = allCarPriceNoZone as DataSet;
                    if (ds == null)
                    {
                        CommonFunction.WriteLog("车款报价区间接口 缓存数据 不是 DataSet");
                        ds = GetLocalCarPriceRange();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
                ds = GetLocalCarPriceRange();
            }
            return ds;
        }

        /// <summary>
        /// 取所有子品牌的报价(不分地区)
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialPrice()
        {
            DataSet ds = new DataSet();
            try
            {
                string catchkey = "AllSerialPriceNoZone";
                object allSerialPriceNoZone = null;
                CacheManager.GetCachedData(catchkey, out allSerialPriceNoZone);
                if (allSerialPriceNoZone == null)
                {
                    //ds.ReadXml(WebConfig.AllSerialPriceNoZone);
                    XmlDocument xmlDoc = CommonFunction.ReadXmlFromUrl(WebConfig.AllSerialPriceNoZone, 60000);
                    if (xmlDoc != null && xmlDoc.HasChildNodes)
                    {
                        ds.ReadXml(new StringReader(xmlDoc.InnerXml));
                        CacheManager.InsertCache(catchkey, ds, 60);
                    }
                    else
                    {
                        CommonFunction.WriteLog("子品牌报价区间接口 读取或数据异常");
                        ds = GetLocalSerialPriceRange();
                    }
                }
                else
                {
                    ds = allSerialPriceNoZone as DataSet;
                    if (ds == null)
                    {
                        CommonFunction.WriteLog("子品牌报价区间接口 缓存数据 不是 DataSet");
                        ds = GetLocalSerialPriceRange();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
                ds = GetLocalSerialPriceRange();
            }
            return ds;
        }
        /// <summary>
        /// 从本地获取子品牌报价区间（易湃）
        /// </summary>
        /// <returns></returns>
        public DataSet GetLocalSerialPriceRange()
        {
            DataSet ds = new DataSet();
            try
            {
                string fileName = Path.Combine(WebConfig.DataBlockPath, @"Data\EP\cspricescope.xml");
                ds.ReadXml(fileName);
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return ds;
        }
        /// <summary>
        /// 从本地获取车款报价区间（易湃）
        /// </summary>
        /// <returns></returns>
        public DataSet GetLocalCarPriceRange()
        {
            DataSet ds = new DataSet();
            try
            {
                string fileName = Path.Combine(WebConfig.DataBlockPath, @"Data\EP\carpricescope.xml");
                ds.ReadXml(fileName);
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return ds;
        }
        #endregion

        #region 子品牌图库数据
        /*0引用，删除2017-04-25
		/// <summary>
		/// 获取子品牌的图片数量
		/// </summary>
		/// <param name="csId"></param>
		/// <returns></returns>
		public int GetSerialPicCount(int csId)
		{
			DataSet ds = GetAllSerialPicAndCount();
			int csCount = 0;
			if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
			{
				DataRow[] drs = ds.Tables[1].Select(" SerialId = '" + csId + "' ");
				if (drs != null && drs.Length > 0)
				{
					csCount = Int32.Parse(drs[0]["ImageCount"].ToString());
				}
			}
			return csCount;
		}
		*/
        /// <summary>
        /// 取所有子品牌封面字典 图片规格定位2 需要其他规格另行替换
        /// </summary>
        /// <param name="isUseNew">新白底图 或者 老非白底图</param>
        /// <returns></returns>
        public Dictionary<int, string> GetAllSerialPicURL(bool isUseNew)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string catchkey = "PageBase_GetAllSerialPicURL_IsNew" + isUseNew.ToString();
            object getAllSerialPicURL = null;
            CacheManager.GetCachedData(catchkey, out getAllSerialPicURL);
            if (getAllSerialPicURL == null)
            {
                XmlDocument xmlDoc = this.GetAllSerialConverImgAndCount();
                if (xmlDoc != null)
                {
                    XmlNodeList serialNodeList = xmlDoc.SelectNodes("/SerialList/Serial");
                    if (serialNodeList != null && serialNodeList.Count > 0)
                    {
                        foreach (XmlNode serialNode in serialNodeList)
                        {
                            int csid = ConvertHelper.GetInteger(serialNode.Attributes["SerialId"].Value);
                            string csNewPic = ConvertHelper.GetString(serialNode.Attributes["ImageUrl2"].Value);
                            string csOldPic = ConvertHelper.GetString(serialNode.Attributes["ImageUrl"].Value);
                            csNewPic = string.IsNullOrEmpty(csNewPic) ? string.Empty : string.Format(csNewPic, 2);
                            csOldPic = string.IsNullOrEmpty(csOldPic) ? string.Empty : string.Format(csOldPic, 2);
                            if (isUseNew)
                            {
                                // 新图的
                                if (!string.IsNullOrEmpty(csNewPic) && !dic.ContainsKey(csid))
                                { dic.Add(csid, csNewPic); }
                                else if (!string.IsNullOrEmpty(csOldPic) && !dic.ContainsKey(csid))
                                { dic.Add(csid, csOldPic); }
                                else if (!dic.ContainsKey(csid))
                                { dic.Add(csid, WebConfig.DefaultCarPic); }
                            }
                            else
                            {
                                // 老图
                                if (!string.IsNullOrEmpty(csOldPic) && !dic.ContainsKey(csid))
                                { dic.Add(csid, csOldPic); }
                                else if (!dic.ContainsKey(csid))
                                { dic.Add(csid, WebConfig.DefaultCarPic); }
                            }
                        }
                    }
                }
                #region 老的接口
                /*
				DataSet ds = GetAllSerialPicAndCount();
				if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[1].Rows)
					{
						int csid = int.Parse(dr["SerialId"].ToString());
						string csNewPic = dr["ImageUrl2"].ToString() != "" ? string.Format(dr["ImageUrl2"].ToString().Trim(), "2") : "";
						string csOldPic = dr["ImageUrl"].ToString() != "" ? string.Format(dr["ImageUrl"].ToString().Trim(), "2") : "";
						if (isUseNew)
						{
							// 新图的
							if (csNewPic != "" && !dic.ContainsKey(csid))
							{ dic.Add(csid, csNewPic); }
							else if (csOldPic != "" && !dic.ContainsKey(csid))
							{ dic.Add(csid, csOldPic); }
							else if (!dic.ContainsKey(csid))
							{ dic.Add(csid, WebConfig.DefaultCarPic); }
						}
						else
						{
							// 老图
							if (csOldPic != "" && !dic.ContainsKey(csid))
							{ dic.Add(csid, csOldPic); }
							else if (!dic.ContainsKey(csid))
							{ dic.Add(csid, WebConfig.DefaultCarPic); }
						}
					}
				}
				 * */
                #endregion

                CacheManager.InsertCache(catchkey, dic, WebConfig.CachedDuration);
            }
            else
            {
                dic = (Dictionary<int, string>)getAllSerialPicURL;
            }
            return dic;
        }

        /// <summary>
        /// 取子品牌默认图及图片总数
        /// </summary>
        /// <param name="csID">车系id</param>
        /// <param name="csPic">图片地址</param>
        /// <param name="csCount">图片数量</param>
        public void GetSerialPicAndCountByCsID(int csID, out string csPic, out int csCount, bool isUseNew)
        {
            csPic = string.Empty;
            csCount = 0;
            XmlDocument imgDoc = this.GetAllSerialConverImgAndCount();
            if (imgDoc != null)
            {
                XmlNode imgNode = imgDoc.SelectSingleNode("/SerialList/Serial[@SerialId='" + csID + "']");
                if (imgNode != null)
                {
                    csCount = ConvertHelper.GetInteger(imgNode.Attributes["ImageCount"].Value);
                    if (isUseNew)
                    {
                        csPic = imgNode.Attributes["ImageUrl2"].Value;
                    }
                    if (string.IsNullOrEmpty(csPic))
                    {
                        csPic = imgNode.Attributes["ImageUrl"].Value;
                    }
                    if (string.IsNullOrEmpty(csPic))
                    {
                        csPic = WebConfig.DefaultCarPic;
                    }
                    else
                    {
                        csPic = string.Format(csPic, 2);
                    }
                }
            }
            #region 老接口
            /*
			DataSet ds = GetAllSerialPicAndCount();
			if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
			{
				DataRow[] drs = ds.Tables[1].Select(" SerialId = '" + csID.ToString() + "' ");
				if (drs != null && drs.Length > 0)
				{
					//CommonFunction cf = new CommonFunction();
					//csPic = cf.GetPublishImage(2, drs[0]["ImageUrl"].ToString(), int.Parse(drs[0]["ImageId"].ToString()));
					if (isUseNew)
					{
						// 用新图
						// 检查新默认图
						if (drs[0]["ImageUrl2"].ToString() != "")
						{
							csPic = string.Format(drs[0]["ImageUrl2"].ToString().Trim(), "2");
						}
						else
						{
							// 老默认图
							if (drs[0]["ImageUrl"].ToString() != "")
							{
								csPic = string.Format(drs[0]["ImageUrl"].ToString().Trim(), "2");
							}
							else
							{
								// 老图新图都没有
								csPic = WebConfig.DefaultCarPic;
							}
						}
					}
					else
					{
						// 用老图
						if (drs[0]["ImageUrl"].ToString() != "")
						{
							csPic = string.Format(drs[0]["ImageUrl"].ToString().Trim(), "2");
						}
						else
						{
							// 老图新图都没有
							csPic = WebConfig.DefaultCarPic;
						}
					}
					csCount = int.Parse(drs[0]["ImageCount"].ToString());
				}
			}
			 * */
            #endregion

        }

        /// <summary>
        /// 取所有子品牌封面字典 白底图
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllSerialPicURLWhiteBackground()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string catchkey = "PageBase_GetAllSerialPicURLWhiteBackground";
            object getAllSerialPicURLWhiteBackground = null;
            CacheManager.GetCachedData(catchkey, out getAllSerialPicURLWhiteBackground);
            if (getAllSerialPicURLWhiteBackground == null)
            {
                XmlDocument imgDoc = this.GetAllSerialConverImgAndCount();
                if (imgDoc != null)
                {
                    XmlNodeList serialNodeList = imgDoc.SelectNodes("/SerialList/Serial");
                    if (serialNodeList != null && serialNodeList.Count > 0)
                    {
                        foreach (XmlNode serialNode in serialNodeList)
                        {
                            int csid = ConvertHelper.GetInteger(serialNode.Attributes["SerialId"].Value);
                            string csNewPic = ConvertHelper.GetString(serialNode.Attributes["ImageUrl2"].Value);
                            csNewPic = string.IsNullOrEmpty(csNewPic) ? string.Empty : string.Format(csNewPic, 2);
                            // 新图的
                            if (!string.IsNullOrEmpty(csNewPic) && !dic.ContainsKey(csid))
                            { dic.Add(csid, csNewPic); }
                        }
                    }
                }
                #region 老接口
                /*
				DataSet ds = GetAllSerialPicAndCount();
				if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[1].Rows)
					{
						int csid = int.Parse(dr["SerialId"].ToString());
						string csNewPic = dr["ImageUrl2"].ToString() != "" ? string.Format(dr["ImageUrl2"].ToString().Trim(), "2") : "";
						string csOldPic = dr["ImageUrl"].ToString() != "" ? string.Format(dr["ImageUrl"].ToString().Trim(), "2") : "";
						// 新图的
						if (csNewPic != "" && !dic.ContainsKey(csid))
						{ dic.Add(csid, csNewPic); }
					}
				}
				 * */
                #endregion

                CacheManager.InsertCache(catchkey, dic, WebConfig.CachedDuration);
            }
            else
            {
                dic = (Dictionary<int, string>)getAllSerialPicURLWhiteBackground;
            }
            return dic;
        }

        /// <summary>
        /// 取所有子品牌封面字典 非白底图
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllSerialPicURLNoneWhiteBackground()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string catchkey = "PageBase_GetAllSerialPicURLNoneWhiteBackground";
            object getAllSerialPicURLNoneWhiteBackground = null;
            CacheManager.GetCachedData(catchkey, out getAllSerialPicURLNoneWhiteBackground);
            if (getAllSerialPicURLNoneWhiteBackground == null)
            {
                XmlDocument xmlDoc = this.GetAllSerialConverImgAndCount();
                if (xmlDoc != null)
                {
                    XmlNodeList serialNodeList = xmlDoc.SelectNodes("/SerialList/Serial");
                    if (serialNodeList != null && serialNodeList.Count > 0)
                    {
                        foreach (XmlNode serialNode in serialNodeList)
                        {
                            int csid = ConvertHelper.GetInteger(serialNode.Attributes["SerialId"].Value);
                            string csOldPic = ConvertHelper.GetString(serialNode.Attributes["ImageUrl"].Value);
                            csOldPic = string.IsNullOrEmpty(csOldPic) ? string.Empty : string.Format(csOldPic, 2);

                            // 老图
                            if (!string.IsNullOrEmpty(csOldPic) && !dic.ContainsKey(csid))
                            { dic.Add(csid, csOldPic); }
                        }
                    }
                }
                #region 老接口
                /*
				DataSet ds = GetAllSerialPicAndCount();
				if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[1].Rows)
					{
						int csid = int.Parse(dr["SerialId"].ToString());
						string csNewPic = dr["ImageUrl2"].ToString() != "" ? string.Format(dr["ImageUrl2"].ToString().Trim(), "2") : "";
						string csOldPic = dr["ImageUrl"].ToString() != "" ? string.Format(dr["ImageUrl"].ToString().Trim(), "2") : "";

						// 老图
						if (csOldPic != "" && !dic.ContainsKey(csid))
						{ dic.Add(csid, csOldPic); }

					}
				}
				 * */
                #endregion
                CacheManager.InsertCache(catchkey, dic, WebConfig.CachedDuration);
            }
            else
            {
                dic = (Dictionary<int, string>)getAllSerialPicURLNoneWhiteBackground;
            }
            return dic;
        }
        /// <summary>
        /// 取所有子品牌封面字典 非白底图(带ImageId)
        /// </summary>
        /// <param name="imgSize">图片尺寸</param>
        /// <returns></returns>
        public Dictionary<int, Dictionary<int, string>> GetAllSerialPicNoneWhiteBackground(int imgSize = 6)
        {
            Dictionary<int, Dictionary<int, string>> dic = new Dictionary<int, Dictionary<int, string>>();
            string catchkey = "PageBase_GetAllSerialPicNoneWhiteBackground";
            object getAllSerialPicNoneWhiteBackground = null;
            CacheManager.GetCachedData(catchkey, out getAllSerialPicNoneWhiteBackground);
            if (getAllSerialPicNoneWhiteBackground == null)
            {
                XmlDocument xmlDoc = this.GetAllSerialConverImgAndCount();
                if (xmlDoc != null)
                {
                    XmlNodeList serialNodeList = xmlDoc.SelectNodes("/SerialList/Serial");
                    if (serialNodeList != null && serialNodeList.Count > 0)
                    {
                        foreach (XmlNode serialNode in serialNodeList)
                        {
                            int csid = ConvertHelper.GetInteger(serialNode.Attributes["SerialId"].Value);
                            int imgid = ConvertHelper.GetInteger(serialNode.Attributes["ImageId"].Value);
                            string csOldPic = ConvertHelper.GetString(serialNode.Attributes["ImageUrl"].Value);
                            csOldPic = string.IsNullOrEmpty(csOldPic) ? string.Empty : string.Format(csOldPic, imgSize);
                            // 老图
                            if (!string.IsNullOrEmpty(csOldPic) && !dic.ContainsKey(csid))
                            {
                                var subDic = new Dictionary<int, string>();
                                subDic.Add(imgid, csOldPic);
                                dic.Add(csid, subDic);
                            }
                        }
                    }
                }
                #region 老接口
                /*
				DataSet ds = GetAllSerialPicAndCount();
				if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[1].Rows)
					{
						int csid = int.Parse(dr["SerialId"].ToString());
						int imgid = int.Parse(dr["ImageId"].ToString());
						string csNewPic = dr["ImageUrl2"].ToString() != "" ? string.Format(dr["ImageUrl2"].ToString().Trim(), "4") : "";
						string csOldPic = dr["ImageUrl"].ToString() != "" ? string.Format(dr["ImageUrl"].ToString().Trim(), "4") : "";

						// 老图
						if (csOldPic != "" && !dic.ContainsKey(csid))
						{
							var subDic = new Dictionary<int, string>();
							subDic.Add(imgid, csOldPic);
							dic.Add(csid, subDic);
						}

					}
				}
				 * */
                #endregion

                CacheManager.InsertCache(catchkey, dic, WebConfig.CachedDuration);
            }
            else
            {
                dic = (Dictionary<int, Dictionary<int, string>>)getAllSerialPicNoneWhiteBackground;
            }
            return dic;
        }

        /* 车系封面图及图片数量接口不在使用，有GetAllSerialConverImgAndCount方法代替
		/// <summary>
		/// 子品牌默认图及图片总数
		/// </summary>
		/// <returns></returns>
		public DataSet GetAllSerialPicAndCount()
		{
			string catchkey = "AllSerialPicAndCount";
			object allSerialPicAndCount = null;
			DataSet ds = new DataSet();
			CacheManager.GetCachedData(catchkey, out allSerialPicAndCount);
			if (allSerialPicAndCount == null)
			{
				try
				{
					// ds.ReadXml(WebConfig.AllSerialPicCount);
					//图库接口本地化更改 by sk 2012.12.21
					string photoDataPath = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialCoverWithoutPath);
					ds.ReadXml(photoDataPath);
					//ds.ReadXml(WebConfig.AllSerialDefaultPicAndCount);
					CacheManager.InsertCache(catchkey, ds, 60);
				}
				catch (Exception ex)
				{
					CommonFunction.WriteLog(ex.ToString());
				}
			}
			else
			{
				ds = (DataSet)allSerialPicAndCount;
			}
			return ds;
		}
		*/
        /// <summary>
        /// 车系封面图及图片数量
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetAllSerialConverImgAndCount()
        {
            string cacheKey = "Car_GetAllSerialConverImgAndCount";
            object allSerialPicAndCount = null;
            CacheManager.GetCachedData(cacheKey, out allSerialPicAndCount);
            if (allSerialPicAndCount == null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                string photoDataPath = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialCoverImageAndCountPath);
                xmlDoc.Load(photoDataPath);
                CacheManager.InsertCache(cacheKey, xmlDoc, 60);
                return xmlDoc;
            }
            else
            {
                return allSerialPicAndCount as XmlDocument;
            }
        }

        #endregion

        #region 取竞争车型对比

        /// <summary>
        /// 取级别的竞争车型
        /// </summary>
        /// <param name="levelID">级别ID(EnumCollection.SerialLevelEnum 枚举值)</param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public List<EnumCollection.CarHotCompareData> GetLevelHotCompareCars(int levelID, int topCount)
        {
            List<EnumCollection.CarHotCompareData> lresult = new List<EnumCollection.CarHotCompareData>();
            //string levelName = Convert.ToString((EnumCollection.SerialLevelEnum)levelID);
            string levelName = CarLevelDefine.GetLevelNameById(levelID);
            List<EnumCollection.CarHotCompareData> lchcd = new List<EnumCollection.CarHotCompareData>();
            List<int> lcount = new List<int>();
            DataSet dsAllSerial = this.GetAllSErialInfo();
            if (dsAllSerial != null && dsAllSerial.Tables.Count > 0 && dsAllSerial.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsAllSerial.Tables[0].Select(" cs_CarLevel = '" + levelName + "' ");
                if (drs != null && drs.Length > 0)
                {
                    for (int i = 0; i < drs.Length; i++)
                    {
                        List<EnumCollection.CarHotCompareData> temp = this.GetSerialHotCompareByCsID(int.Parse(drs[i]["cs_ID"].ToString()), 1, false);
                        if (temp.Count > 0)
                        {
                            lchcd.Add(temp[0]);
                            lcount.Add(temp[0].CompareCount);
                        }
                    }
                }
            }

            EnumCollection.CarHotCompareData[] arrTemp = lchcd.ToArray();
            int[] arrIntTemp = lcount.ToArray();
            if (arrIntTemp.Length == arrTemp.Length)
            {
                Array.Sort(arrIntTemp, arrTemp);
                for (int i = arrIntTemp.Length - 1; i >= 0; i--)
                {
                    if (arrIntTemp.Length - i > topCount)
                    { break; }
                    lresult.Add(arrTemp[i]);
                }
            }
            return lresult;
        }

        /// <summary>
        /// 取子品牌对子品牌的竞争数据
        /// </summary>
        /// <param name="csID"></param>
        /// <param name="topCount"></param>
        /// <param name="isContainSameSerial"></param>
        /// <returns></returns>
        public List<EnumCollection.SerialHotCompareData> GetSerialToSerialHotCompareByCsID(int csID, int topCount, bool isContainSameSerial)
        {
            List<EnumCollection.SerialHotCompareData> lreturn = new List<EnumCollection.SerialHotCompareData>();
            List<EnumCollection.SerialHotCompareData> lshcd = new List<EnumCollection.SerialHotCompareData>();
            List<int> lcount = new List<int>();
            this.GetSerialHotToSerialCompareByCsID(csID, ref lshcd, ref lcount);

            EnumCollection.SerialHotCompareData[] arrCsData = lshcd.ToArray();
            int[] arrCount = lcount.ToArray();
            if (arrCsData.Length == arrCount.Length)
            {
                int loop = 1;
                Array.Sort(arrCount, arrCsData);
                for (int i = arrCsData.Length - 1; i >= 0; i--)
                {
                    if (loop > topCount)
                    {
                        break;
                    }

                    if (isContainSameSerial)
                    {
                        lreturn.Add(arrCsData[i]);
                        loop++;
                    }
                    else
                    {
                        // 不取同一子品牌车型
                        if (arrCsData[i].CompareCsID == csID)
                        {
                            continue;
                        }
                        lreturn.Add(arrCsData[i]);
                        loop++;
                    }
                }
            }
            return lreturn;
        }

        /// <summary>
        /// 取子品牌竞争车型
        /// </summary>
        /// <param name="csID"></param>
        /// <param name="topCount"></param>
        /// <param name="isContainSameSerial"></param>
        /// <returns></returns>
        public List<EnumCollection.CarHotCompareData> GetSerialHotCompareByCsID(int csID, int topCount, bool isContainSameSerial)
        {
            List<EnumCollection.CarHotCompareData> lreturn = new List<EnumCollection.CarHotCompareData>();
            List<EnumCollection.CarHotCompareData> lchcd = new List<EnumCollection.CarHotCompareData>();
            List<int> lcount = new List<int>();
            this.GetSerialHotCompareByCsID(csID, ref lchcd, ref lcount);

            EnumCollection.CarHotCompareData[] arrCarData = lchcd.ToArray();
            int[] arrCount = lcount.ToArray();
            if (arrCarData.Length == arrCount.Length)
            {
                int loop = 1;
                Array.Sort(arrCount, arrCarData);
                for (int i = arrCarData.Length - 1; i >= 0; i--)
                {
                    if (loop > topCount)
                    {
                        break;
                    }

                    if (isContainSameSerial)
                    {
                        lreturn.Add(arrCarData[i]);
                        loop++;
                    }
                    else
                    {
                        // 不取同一子品牌车型
                        if (arrCarData[i].CompareCsID == csID)
                        {
                            continue;
                        }
                        lreturn.Add(arrCarData[i]);
                        loop++;
                    }
                }
            }
            return lreturn;
        }

        /// <summary>
        /// 取车型竞争车型
        /// </summary>
        /// <param name="carID"></param>
        /// <param name="topCount"></param>
        /// <param name="isContainSameSerial"></param>
        /// <returns></returns>
        public List<EnumCollection.CarHotCompareData> GetCarHotCompareByCarID(int csID, int carID, int topCount, bool isContainSameSerial)
        {
            List<EnumCollection.CarHotCompareData> lreturn = new List<EnumCollection.CarHotCompareData>();
            List<EnumCollection.CarHotCompareData> lchcd = new List<EnumCollection.CarHotCompareData>();
            List<int> lcount = new List<int>();
            this.GetCarHotCompareByCarID(carID, ref lchcd, ref lcount);

            EnumCollection.CarHotCompareData[] arrCarData = lchcd.ToArray();
            int[] arrCount = lcount.ToArray();
            if (arrCarData.Length == arrCount.Length)
            {
                int loop = 1;
                Array.Sort(arrCount, arrCarData);
                for (int i = arrCarData.Length - 1; i >= 0; i--)
                {
                    if (loop > topCount)
                    {
                        break;
                    }

                    if (isContainSameSerial)
                    {
                        lreturn.Add(arrCarData[i]);
                        loop++;
                    }
                    else
                    {
                        // 不取同一子品牌车型
                        if (arrCarData[i].CompareCsID == csID)
                        {
                            continue;
                        }
                        lreturn.Add(arrCarData[i]);
                        loop++;
                    }
                }
            }
            return lreturn;
        }

        /// <summary>
        /// 取子品牌下竞争车型
        /// </summary>
        /// <param name="csID"></param>
        /// <param name="lchcd"></param>
        /// <param name="lcount"></param>
        public void GetSerialHotCompareByCsID(int csID, ref List<EnumCollection.CarHotCompareData> lchcd, ref List<int> lcount)
        {
            XmlDocument doc = new XmlDocument();
            DataSet ds = this.GetAllCarInfo();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] dr = ds.Tables[0].Select(" cs_id = " + csID.ToString());
                if (dr.Length > 0)
                {
                    foreach (DataRow drCar in dr)
                    {
                        int carID = int.Parse(drCar["car_id"].ToString());
                        try
                        {
                            if (HttpContext.Current.Cache.Get("ForCarCompareList_XML" + carID.ToString()) != null)
                            {
                                doc = (XmlDocument)HttpContext.Current.Cache.Get("ForCarCompareList_XML" + carID.ToString());
                            }
                            else
                            {
                                doc.Load(string.Format(WebConfig.CarCompareStat, carID.ToString()));
                                HttpContext.Current.Cache.Insert("ForCarCompareList_XML" + carID.ToString(), doc, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                            }

                            if (doc != null && doc.HasChildNodes)
                            {
                                XmlNodeList xnl = doc.SelectNodes("/Car/Item");
                                if (xnl != null && xnl.Count > 0)
                                {
                                    for (int i = 0; i < xnl.Count; i++)
                                    {
                                        int iCarId = int.Parse(xnl[i].Attributes["ID"].Value.ToString());
                                        int iCompareCount = int.Parse(xnl[i].Attributes["Count"].Value.ToString());

                                        DataRow[] drCompare = ds.Tables[0].Select(" car_id = " + iCarId.ToString());
                                        if (drCompare.Length > 0)
                                        {
                                            EnumCollection.CarHotCompareData cd = new EnumCollection.CarHotCompareData();
                                            this.GetCarStructDataByID(iCarId, iCompareCount, drCompare, drCar, ref cd);
                                            lchcd.Add(cd);
                                            lcount.Add(iCompareCount);
                                        }
                                    }
                                }
                            }

                        }
                        catch
                        { }
                    }
                }
            }
        }

        /// <summary>
        /// 取车型的竞争车型
        /// </summary>
        /// <param name="carID"></param>
        /// <param name="lchcd"></param>
        /// <param name="lcount"></param>
        public void GetCarHotCompareByCarID(int carID, ref List<EnumCollection.CarHotCompareData> lchcd, ref List<int> lcount)
        {
            XmlDocument doc = new XmlDocument();
            DataSet ds = this.GetAllCarInfo();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] dr = ds.Tables[0].Select(" car_id = " + carID.ToString());
                if (dr.Length > 0)
                {
                    // int csID = int.Parse(dr[0]["cs_id"].ToString());
                    try
                    {
                        if (HttpContext.Current.Cache.Get("ForCarCompareList_XML" + carID.ToString()) != null)
                        {
                            doc = (XmlDocument)HttpContext.Current.Cache.Get("ForCarCompareList_XML" + carID.ToString());
                        }
                        else
                        {
                            doc.Load(string.Format(WebConfig.CarCompareStat, carID.ToString()));
                            HttpContext.Current.Cache.Insert("ForCarCompareList_XML" + carID.ToString(), doc, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                        }

                        if (doc != null && doc.HasChildNodes)
                        {
                            XmlNodeList xnl = doc.SelectNodes("/Car/Item");
                            if (xnl != null && xnl.Count > 0)
                            {
                                for (int i = 0; i < xnl.Count; i++)
                                {
                                    int iCarId = int.Parse(xnl[i].Attributes["ID"].Value.ToString());
                                    int iCompareCount = int.Parse(xnl[i].Attributes["Count"].Value.ToString());

                                    DataRow[] drCompare = ds.Tables[0].Select(" car_id = " + iCarId.ToString());
                                    if (drCompare.Length > 0)
                                    {
                                        EnumCollection.CarHotCompareData cd = new EnumCollection.CarHotCompareData();
                                        this.GetCarStructDataByID(iCarId, iCompareCount, drCompare, dr[0], ref cd);
                                        lchcd.Add(cd);
                                        lcount.Add(iCompareCount);
                                    }
                                }
                            }
                        }

                    }
                    catch
                    { }

                }
            }
        }

        /// <summary>
        /// 取子品牌对子品牌对比统计
        /// </summary>
        /// <param name="csID"></param>
        /// <param name="lchcd"></param>
        /// <param name="lcount"></param>
        public void GetSerialHotToSerialCompareByCsID(int csID, ref List<EnumCollection.SerialHotCompareData> lchcd, ref List<int> lcount)
        {
            XmlDocument doc = new XmlDocument();
            DataSet ds = this.GetAllSErialInfo();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    if (HttpContext.Current.Cache.Get("ForSerialCompareList_XML" + csID.ToString()) != null)
                    {
                        doc = (XmlDocument)HttpContext.Current.Cache.Get("ForSerialCompareList_XML" + csID.ToString());
                    }
                    else
                    {
                        doc.Load(string.Format(WebConfig.SerialCompareStat, csID.ToString()));
                        HttpContext.Current.Cache.Insert("ForSerialCompareList_XML" + csID.ToString(), doc, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                    }
                    DataRow[] dr = ds.Tables[0].Select(" cs_id = " + csID.ToString());
                    if (dr.Length > 0)
                    {
                        string currentCsName = dr[0]["cs_name"].ToString();
                        if (doc != null && doc.HasChildNodes)
                        {
                            XmlNodeList xnl = doc.SelectNodes("/Serial/Item");
                            if (xnl != null && xnl.Count > 0)
                            {
                                for (int i = 0; i < xnl.Count; i++)
                                {
                                    int iCsId = int.Parse(xnl[i].Attributes["ID"].Value.ToString());
                                    int iCompareCount = int.Parse(xnl[i].Attributes["Count"].Value.ToString());

                                    DataRow[] drCompare = ds.Tables[0].Select(" cs_id = " + iCsId.ToString());

                                    if (drCompare.Length > 0)
                                    {
                                        EnumCollection.SerialHotCompareData shcd = new EnumCollection.SerialHotCompareData();
                                        this.GetCsStructDataByID(csID, iCompareCount, drCompare, ref shcd);
                                        lchcd.Add(shcd);
                                        lcount.Add(iCompareCount);
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                { }
            }

        }

        private void GetCsStructDataByID(int csID, int compareCount, DataRow[] dr, ref EnumCollection.SerialHotCompareData csData)
        {
            csData.CurrentCsID = csID;
            csData.CompareCount = compareCount;
            if (dr.Length > 0)
            {
                Dictionary<int, string> whileImgDic = this.GetAllSerialPicURLWhiteBackground();
                csData.CompareCsID = int.Parse(dr[0]["cs_id"].ToString());
                csData.CompareCsName = dr[0]["cs_name"].ToString().Trim();
                csData.CompareCsShowName = dr[0]["cs_showname"].ToString().Trim();
                csData.CompareCsAllSpell = dr[0]["allspell"].ToString().Trim().ToLower();
                csData.CompareCsCbName = dr[0]["cb_name"].ToString().Trim();
                csData.CompareCsPriceRange = this.GetSerialPriceRangeByID(csData.CompareCsID);
                csData.ComapreCsImg = whileImgDic.ContainsKey(csData.CompareCsID) ? whileImgDic[csData.CompareCsID] : WebConfig.DefaultCarPic;
            }
        }

        private void GetCarStructDataByID(int carID, int compareCount, DataRow[] drCompare, DataRow drCurrent, ref EnumCollection.CarHotCompareData carData)
        {
            carData.CurrentCarID = int.Parse(drCurrent["car_id"].ToString());
            carData.CurrentCarName = drCurrent["car_Name"].ToString();
            carData.CurrentCsID = int.Parse(drCurrent["cs_id"].ToString());
            carData.CurrentCsName = drCurrent["cs_Name"].ToString();
            carData.CompareCarID = carID;
            carData.CompareCount = compareCount;
            if (drCompare.Length > 0)
            {
                carData.CompareCarName = drCompare[0]["car_name"].ToString();
                carData.CompareCsID = int.Parse(drCompare[0]["cs_id"].ToString());
                carData.CompareCsName = drCompare[0]["cs_name"].ToString();
            }
        }

        /// <summary>
        /// 取子品牌热门对比子品牌
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="top">取前几条</param>
        public List<EnumCollection.SerialHotCompareData> GetSerialHotCompareByCsID(int csID, int top)
        {
            List<EnumCollection.SerialHotCompareData> lchcd = new List<EnumCollection.SerialHotCompareData>();
            if (top < 0 || top > 20)
            { top = 5; }
            string allSerialCompareTop20Path = WebConfig.DataBlockPath + "Data\\AllSerialCompareTop20.xml";
            string cachekey = "AllSerialCompareTop20";
            object allSerialCompareTop20 = null;

            DataSet ds = this.GetAllSErialInfo();
            XmlDocument doc = new XmlDocument();
            CacheManager.GetCachedData(cachekey, out allSerialCompareTop20);
            if (allSerialCompareTop20 == null)
            {
                if (File.Exists(allSerialCompareTop20Path))
                {
                    doc.Load(allSerialCompareTop20Path);
                    CacheManager.InsertCache(cachekey, doc, WebConfig.CachedDuration);
                }
            }
            else
            {
                doc = (XmlDocument)allSerialCompareTop20;
            }

            if (doc != null && doc.HasChildNodes)
            {
                XmlNodeList xnl = doc.SelectNodes("/AllSerialCompare/CS[@ID='" + csID + "']");
                if (xnl != null && xnl.Count > 0)
                {
                    int loop = 0;
                    if (xnl[0].ChildNodes.Count > 0)
                    {

                        foreach (XmlNode xn in xnl[0])
                        {
                            DataRow[] drCompare = ds.Tables[0].Select(" cs_id = " + xn.Attributes["ID"].Value.ToString());

                            if (drCompare.Length > 0)
                            {
                                EnumCollection.SerialHotCompareData shcd = new EnumCollection.SerialHotCompareData();
                                this.GetCsStructDataByID(csID, 0, drCompare, ref shcd);
                                lchcd.Add(shcd);

                                loop++;
                                if (loop >= top)
                                { break; }
                            }
                        }
                    }

                }
            }
            return lchcd;
        }

        #endregion

        #region 取评测对比文章
        /// <summary>
        /// 改新闻接口已经不再维护，调用新闻接口改到CarNewsBll.GetSerialPingCeNews方法
        /// </summary>
        /// <param name="newID"></param>
        /// <returns></returns>
        public DataSet GetPingCeNewByNewID(int newID)
        {
            string catchkey = "pingCeNewByNewID_" + newID.ToString();
            object pingCeNewByNewID = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out pingCeNewByNewID);
            if (pingCeNewByNewID == null)
            {
                try
                {
                    ds.ReadXml(string.Format(WebConfig.SeriaPingCeData, newID.ToString()));
                    CacheManager.InsertCache(catchkey, ds, 60);
                }
                catch
                { }
            }
            else
            {
                ds = (DataSet)pingCeNewByNewID;
            }
            return ds;
        }

        public int GetPingCeNewIDByCsID(int csID)
        {
            int newID = 0;
            DataSet ds = this.GetAllPingCeNewsURL();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" csID = " + csID.ToString() + " ");
                if (drs != null && drs.Length > 0)
                {
                    string tempURL = drs[0]["url"].ToString();
                    if (tempURL.IndexOf("/") > 0)
                    {
                        string[] arrTempURL = tempURL.Split('/');
                        try
                        {
                            if (int.TryParse(arrTempURL[arrTempURL.Length - 1].ToString().Substring(3, 7), out newID))
                            { }
                        }
                        catch
                        { }
                    }
                }
            }
            return newID;
        }

        /// <summary>
        /// 取评测新闻 返回新闻URL
        /// </summary>
        /// <param name="csID"></param>
        /// <param name="newURL"></param>
        /// <returns></returns>
        public int GetPingCeNewIDByCsID(int csID, out string newURL)
        {
            newURL = "";
            int newID = 0;
            DataSet ds = this.GetAllPingCeNewsURL();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" csID = " + csID.ToString() + " ");
                if (drs != null && drs.Length > 0)
                {
                    string tempURL = drs[0]["url"].ToString();
                    newURL = drs[0]["url"].ToString();
                    if (tempURL.IndexOf("/") > 0)
                    {
                        string[] arrTempURL = tempURL.Split('/');
                        try
                        {
                            if (int.TryParse(arrTempURL[arrTempURL.Length - 1].ToString().Substring(3, 7), out newID))
                            { }
                        }
                        catch
                        { }
                    }
                }
            }
            return newID;
        }

        private DataSet GetAllPingCeNewsURL()
        {
            string catchkey = "AllPingCeNewsURL";
            object allPingCeNewsURL = null;
            DataSet ds = new DataSet();
            // 
            CacheManager.GetCachedData(catchkey, out allPingCeNewsURL);
            if (allPingCeNewsURL == null)
            {
                string sql = " select csid,url from dbo.RainbowEdit where RainbowItemID = 43 ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)allPingCeNewsURL;
            }
            return ds;
        }

        /// <summary>
        /// 评测页、子品牌 "车型详解" 使用彩虹条 60 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllPingCeNewsURLForCsPingCePage()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string catchkey = "PageBase_GetAllPingCeNewsURLForCsPingCePage";
            object allPingCeNewsURLForCsPingCePage = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allPingCeNewsURLForCsPingCePage);
            if (allPingCeNewsURLForCsPingCePage == null)
            {
                string sql = " select csid,url from dbo.RainbowEdit where RainbowItemID = 60 ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int csid = int.Parse(dr["csid"].ToString());
                        string url = dr["url"].ToString().Trim();
                        if (!dic.ContainsKey(csid))
                        { dic.Add(csid, url); }
                    }
                }
                CacheManager.InsertCache(catchkey, dic, 60);
            }
            else
            {
                dic = (Dictionary<int, string>)allPingCeNewsURLForCsPingCePage;
            }
            return dic;
        }
        /// <summary>
        /// 获取子品牌评测信息
        /// </summary>
        /// <param name="csId">子品牌ID</param>
        /// <returns></returns>
        public Dictionary<int, EnumCollection.PingCeTag> GetPingceNewsByCsId(int csId)
        {
            Dictionary<int, EnumCollection.PingCeTag> dicAllTagInfo = CommonFunction.IntiPingCeTagInfo();
            Dictionary<int, EnumCollection.PingCeTag> dic = new Dictionary<int, EnumCollection.PingCeTag>();
            string sql = "SELECT [csid],[url],[tagid] FROM [CarPingceInfo] WHERE csid=@csid ORDER BY tagid";
            SqlParameter[] parameters = { new SqlParameter("@csid", SqlDbType.Int) };
            parameters[0].Value = csId;
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, parameters);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int pageNum = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    pageNum++;
                    int tagId = ConvertHelper.GetInteger(dr["tagid"]);
                    string url = dr["url"].ToString();
                    EnumCollection.PingCeTag pingce = new EnumCollection.PingCeTag();
                    pingce.tagId = tagId;
                    pingce.tagName = dicAllTagInfo[tagId].tagName;
                    pingce.url = url;
                    if (!dic.ContainsKey(pageNum))
                    {
                        dic.Add(pageNum, pingce);
                    }
                }
            }
            return dic;
        }
        /// <summary>
        /// 获取子品牌评测信息
        /// </summary>
        /// <param name="csId">子品牌ID</param>
        /// <returns></returns>
        public Dictionary<int, EnumCollection.PingCeTag> GetPingceTagsByCsId(int csId)
        {
            Dictionary<int, EnumCollection.PingCeTag> dicAllTagInfo = CommonFunction.IntiPingCeTagInfo();
            Dictionary<int, EnumCollection.PingCeTag> dic = new Dictionary<int, EnumCollection.PingCeTag>();
            string cacheKey = string.Format("Car_Dict_PingTag_{0}", csId);
            object cacheObj = CacheManager.GetCachedData(cacheKey);
            if (cacheObj != null)
            {
                return (Dictionary<int, EnumCollection.PingCeTag>)cacheObj;
            }
            string sql = "SELECT [csid],[url],[tagid] FROM [CarPingceInfo] WHERE csid=@csid ORDER BY tagid";
            SqlParameter[] parameters = { new SqlParameter("@csid", SqlDbType.Int) };
            parameters[0].Value = csId;
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, parameters);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int tagId = ConvertHelper.GetInteger(dr["tagid"]);
                    string url = dr["url"].ToString();
                    EnumCollection.PingCeTag pingce = new EnumCollection.PingCeTag();
                    pingce.tagId = tagId;
                    pingce.tagName = dicAllTagInfo[tagId].tagName;
                    pingce.url = url;
                    if (!dic.ContainsKey(tagId))
                    {
                        dic.Add(tagId, pingce);
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, 60 * 12);
            }
            return dic;
        }
        #endregion

        #region 取子品牌还关注

        /// <summary>
        /// 取子品牌的还关注
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="top">取条数</param>
        public List<EnumCollection.SerialToSerial> GetSerialToSerialByCsID(int csID, int top)
        {
            List<EnumCollection.SerialToSerial> lsts = new List<EnumCollection.SerialToSerial>();
            DataSet ds = this.GetAllSerialToSerial();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" cs_id = " + csID.ToString() + " ");
                if (drs != null && drs.Length > 0)
                {
                    int loopCount = 0;
                    foreach (DataRow dr in drs)
                    {
                        if (loopCount >= top)
                        { break; }
                        EnumCollection.SerialToSerial sts = new EnumCollection.SerialToSerial();
                        sts.CsID = int.Parse(dr["cs_id"].ToString());
                        sts.ToCsID = int.Parse(dr["PCs_Id"].ToString());
                        sts.ToPv_Num = int.Parse(dr["Pv_Num"].ToString());
                        sts.ToCsName = dr["cs_name"].ToString().Trim();
                        sts.ToCsShowName = dr["cs_showname"].ToString().Trim();
                        sts.ToCsAllSpell = dr["allspell"].ToString().Trim();
                        sts.ToCsSaleState = dr["CsSaleState"].ToString().Trim();
                        string defaultPic = "";
                        int picCount = 0;
                        this.GetSerialPicAndCountByCsID(sts.ToCsID, out defaultPic, out picCount, true);
                        if (defaultPic.Trim().Length == 0)
                            defaultPic = WebConfig.DefaultCarPic;
                        sts.ToCsPic = defaultPic.Replace("_2.", "_5.");
                        sts.ToCsPriceRange = this.GetSerialPriceRangeByID(sts.ToCsID);
                        lsts.Add(sts);
                        loopCount++;
                    }
                }
            }
            return lsts;
        }

        /// <summary>
        /// 取子品牌的还关注
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="top">取条数</param>
        public List<EnumCollection.SerialToSerial> GetSerialToSerialByCsID(int csID, int top, int size)
        {
            List<EnumCollection.SerialToSerial> lsts = new List<EnumCollection.SerialToSerial>();
            DataSet ds = this.GetAllSerialToSerial();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select(" cs_id = " + csID.ToString() + " ");
                if (drs != null && drs.Length > 0)
                {
                    int loopCount = 0;
                    foreach (DataRow dr in drs)
                    {
                        if (loopCount >= top)
                        { break; }
                        EnumCollection.SerialToSerial sts = new EnumCollection.SerialToSerial();
                        sts.CsID = int.Parse(dr["cs_id"].ToString());
                        sts.ToCsID = int.Parse(dr["PCs_Id"].ToString());
                        sts.ToPv_Num = int.Parse(dr["Pv_Num"].ToString());
                        sts.ToCsName = dr["cs_name"].ToString().Trim();
                        sts.ToCsShowName = dr["cs_showname"].ToString().Trim();
                        sts.ToCsAllSpell = dr["allspell"].ToString().Trim();
                        sts.ToCsSaleState = dr["CsSaleState"].ToString().Trim();
                        string defaultPic = "";
                        int picCount = 0;
                        this.GetSerialPicAndCountByCsID(sts.ToCsID, out defaultPic, out picCount, true);
                        if (defaultPic.Trim().Length == 0)
                            defaultPic = WebConfig.DefaultCarPic;
                        sts.ToCsPic = defaultPic.Replace("_2.", string.Format("_{0}.", size));
                        sts.ToCsPriceRange = this.GetSerialPriceRangeByID(sts.ToCsID);
                        lsts.Add(sts);
                        loopCount++;
                    }
                }
            }
            return lsts;
        }


        /// <summary>
        /// 取子品牌还关注的10个子品牌ID
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<int>> GetAllSerialToSerialDic()
        {
            Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();
            DataSet ds = GetAllSerialToSerial();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int csid = int.Parse(dr["CS_Id"].ToString());
                    int ocsid = int.Parse(dr["PCs_Id"].ToString());
                    if (dic.ContainsKey(csid))
                    {
                        if (!dic[csid].Contains(ocsid))
                        { dic[csid].Add(ocsid); }
                    }
                    else
                    {
                        List<int> list = new List<int>();
                        list.Add(ocsid);
                        dic.Add(csid, list);
                    }
                }
            }
            return dic;
        }

        /// <summary>
        /// 取所有子品牌还关注数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialToSerial()
        {
            string catchkey = "AllSerialToSerial";
            object allSerialToSerial = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allSerialToSerial);
            if (allSerialToSerial == null)
            {
                string sql = " select sts.CS_Id,sts.PCs_Id,sts.Pv_Num,cs.cs_name,cs.cs_showname,cs.allspell,cs.CsSaleState ";
                sql += " from dbo.Serial_To_Serial sts ";
                sql += " left join Car_Serial cs on sts.PCs_Id = cs.cs_id ";
                sql += " order by sts.CS_Id,sts.Pv_Num desc ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)allSerialToSerial;
            }
            return ds;
        }

        #endregion

        #region 对比中转接口

        public ArrayList GetTheSamePriceRangeCarsByCarID(int carID, int top, int balance)
        {
            ArrayList alCars = new ArrayList();
            DataSet dsCarInfo = this.GetAllCarInfo();
            if (dsCarInfo != null && dsCarInfo.Tables.Count > 0 && dsCarInfo.Tables[0].Rows.Count > 0)
            {
                DataSet ds = GetAllCarPrice();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] drs = ds.Tables[0].Select(" Id='" + carID.ToString() + "' ");
                    DataRow[] drCarinfo = dsCarInfo.Tables[0].Select(" car_id=" + carID.ToString() + " ");
                    string currentCsID = "0";
                    if (drs.Length > 0 && drCarinfo.Length > 0)
                    {
                        currentCsID = drCarinfo[0]["cs_id"].ToString();
                        decimal avg = 0;
                        try
                        {
                            decimal min = Math.Round(decimal.Parse(drs[0]["MinPrice"].ToString()), 2);
                            decimal max = Math.Round(decimal.Parse(drs[0]["MaxPrice"].ToString()), 2);
                            avg = Math.Round(((min + max) / 2), 2);
                        }
                        catch
                        { }
                        int loop = 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow[] drOtherCarinfo = dsCarInfo.Tables[0].Select(" car_id=" + ds.Tables[0].Rows[i]["Id"].ToString() + " ");
                            if (drOtherCarinfo.Length > 0)
                            {
                                if (drOtherCarinfo[0]["cs_id"].ToString() == currentCsID)
                                {
                                    continue;
                                }
                                if (loop > top)
                                {
                                    break;
                                }
                                decimal minOther = Math.Round(decimal.Parse(ds.Tables[0].Rows[i]["MinPrice"].ToString()), 2);
                                decimal maxOther = Math.Round(decimal.Parse(ds.Tables[0].Rows[i]["MaxPrice"].ToString()), 2);
                                decimal avgOther = Math.Round(((minOther + maxOther) / 2), 2);
                                if (Math.Abs(avg - avgOther) <= balance)
                                {
                                    alCars.Add(ds.Tables[0].Rows[i]["Id"].ToString());
                                    loop++;
                                }
                            }
                        }
                    }
                }
            }
            return alCars;
        }

        /// <summary>
        /// 易车网老对比跳转页
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarIDForCompare()
        {
            string catchkey = "AllCarIDForCompare";
            object allCarIDForCompare = null;
            DataSet ds = new DataSet();
            CacheManager.GetCachedData(catchkey, out allCarIDForCompare);
            if (allCarIDForCompare == null)
            {
                string sql = " select cb.car_id,cb.car_name,cb.oldcar_id,cs.cs_id,cs.cs_Name ";
                sql += " from dbo.Car_Basic cb ";
                sql += " left join dbo.Car_Serial cs on cb.cs_id = cs.cs_id ";
                sql += " where cb.isState >= 1 and cs.isState >= 1 ";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)allCarIDForCompare;
            }
            return ds;
        }

        #endregion

        #region 取子品牌论坛
        protected string GetBBSURLByCsID(int csID)
        {
            string bbsURL = "http://baa.bitauto.com/";
            Dictionary<int, SerialForum> _SerialForumList = BaaCarBrandToForum.GetSerialForumList();
            if (_SerialForumList.ContainsKey(csID))
            {
                bbsURL = _SerialForumList[csID].Url;
            }
            return bbsURL;

            //string cacheName = "GetBBSURLByCsID_" + csID.ToString();

            //if (HttpContext.Current.Cache[cacheName] != null)
            //{
            //    bbsURL = Convert.ToString(HttpContext.Current.Cache[cacheName]);
            //}
            //else
            //{
            //    WebRequest webRequest = WebRequest.Create(string.Format(WebConfig.BBSUrl, csID.ToString()));
            //    HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
            //    if (httpWebRequest == null)
            //    {
            //        // data = "000"; //无效的URL;
            //    }
            //    httpWebRequest.ContentType = sContentType;
            //    httpWebRequest.Method = "GET";
            //    httpWebRequest.Timeout = 1000 * 1;
            //    Stream responseStream = null;
            //    try
            //    {
            //        responseStream = httpWebRequest.GetResponse().GetResponseStream();
            //        using (StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8))
            //        {
            //            bbsURL = responseReader.ReadToEnd();
            //        }
            //        responseStream.Close();
            //        Cache.Insert(cacheName + csID.ToString(), bbsURL, null, DateTime.Now.AddMinutes(120), new TimeSpan(0));
            //        // result = true;
            //    }
            //    catch (Exception exp)
            //    {
            //        // result = false;
            //        // data = exp.Message;
            //    }
            //}
            //return bbsURL.Replace("\r\n", "");
        }
        #endregion

        #region 其他方法

        /// <summary>
        /// 取xml Document对象
        /// </summary>
        /// <param name="cacheName">缓存名</param>
        /// <param name="xmlURL">xml 接口地址</param>
        /// <param name="cacheTimeHour">缓存时间(分钟)</param>
        /// <returns></returns>
        protected XmlDocument GetXMLDocByURLForCache(string cacheName, string xmlURL, int cacheTimeMin)
        {
            XmlDocument doc = new XmlDocument();
            if (HttpContext.Current.Cache[cacheName] != null)
            {
                doc = (XmlDocument)HttpContext.Current.Cache[cacheName];
            }
            else
            {
                try
                {
                    doc.Load(xmlURL);
                    this.Cache.Insert(cacheName, doc, null, DateTime.Now.AddMinutes(cacheTimeMin), new TimeSpan(0));
                }
                catch
                { }
            }
            return doc;
        }

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

        public string GetRequestString(string url, int timeout, out bool result)
        {
            string data = string.Empty;

            WebRequest webRequest = WebRequest.Create(url);

            HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;

            if (httpWebRequest == null)
            {
                data = "000"; //无效的URL;
                result = false;
            }

            httpWebRequest.ContentType = sContentType;
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 1000 * timeout;

            Stream responseStream = null;
            try
            {
                responseStream = httpWebRequest.GetResponse().GetResponseStream();
                using (StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    data = responseReader.ReadToEnd();
                }
                responseStream.Close();
                result = true;
            }
            catch (Exception exp)
            {
                result = false;
                data = exp.Message;
            }

            return data;
        }

        /// <summary>
        /// 取通用导航头
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCommonNavigation(string dirName, int id)
        {
            // modified by chengl Feb.16.2013 缓存取memcache
            // string keyTemp = "Car_CommonHead_{0}_{1}";
            string commonHead = "";
            string CarCommonHeadProcedureName = "dbo.Car_CommonHead_Get";
            SqlParameter[] parameters = {
                                            new SqlParameter("@ID", SqlDbType.Int) ,
                                            new SqlParameter("@TagName", SqlDbType.VarChar)
                                        };
            parameters[0].Value = id;
            parameters[1].Value = dirName;
            DataSet dsHead = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
                WebConfig.DefaultConnectionString, CommandType.StoredProcedure
                , CarCommonHeadProcedureName, parameters);
            //object obj = MemCache.GetMemCacheByKey(string.Format(keyTemp, dirName, id));
            //if (obj != null)
            //{
            //    commonHead = string.Concat("<!--memcache-->", Convert.ToString(obj));
            //}
            if (dsHead != null && dsHead.Tables.Count > 0 && dsHead.Tables[0].Rows.Count > 0)
            {
                commonHead = string.Concat("<!--db-->", dsHead.Tables[0].Rows[0]["CommonHeadContent"].ToString());
            }
            else
            {
                // 先取memcache 没有的话都文件(NAS或本地)

                //string cacheName = "CommonNavigation_" + dirName + "_" + id.ToString();
                //if (HttpContext.Current.Cache[cacheName] != null)
                //{
                //    commonHead = Convert.ToString(HttpContext.Current.Cache[cacheName]);
                //}
                //else
                //{
                string subDir = "";
                if (dirName.StartsWith("Car"))
                {
                    // 如果是车型 子目录名
                    subDir = "\\" + Convert.ToString(id / 1000);
                }
                string navigationFile = Path.Combine(WebConfig.CarDataBaseNASPath
                    + @"CarChannel\CommonHead\" + dirName + subDir, id + ".shtml");
                if (File.Exists(navigationFile))
                {
                    FileStream stream = null;
                    StreamReader sr = null;
                    try
                    {
                        stream = new FileStream(navigationFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        sr = new StreamReader(stream);
                        commonHead = sr.ReadToEnd();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        if (sr != null)
                            sr.Close();
                        if (stream != null)
                            stream.Close();
                    }
                    //        System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency(navigationFile);
                    //        HttpContext.Current.Cache.Insert(cacheName, commonHead, dep, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                    //    }
                }
            }
            return commonHead;
        }

        /// <summary>
        /// 得到优化文件的站点路径
        /// </summary>
        /// <returns></returns>
        public string GetImageRoot()
        {
            imageRootId++;
            if (imageRootId > 3) imageRootId = 1;
            return "http://img" + imageRootId.ToString() + ".bitauto.com/bt/car/default/";

        }

        /// <summary>
        /// 取表最大最小值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void GetTableMaxAndMinValue(DataTable dt, string columnName, out double min, out double max)
        {
            min = (double)0;
            max = (double)0;
            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains(columnName))
            {
                DataRow[] drs = dt.Select("", " " + columnName + " asc ");
                if (drs != null && drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        string valueStr = dr[columnName].ToString();
                        double valueD = (double)0;
                        if (double.TryParse(valueStr, out valueD))
                        {
                            if (valueD > 0)
                            {
                                if (min == 0)
                                {
                                    min = valueD;
                                }
                                if (max == 0)
                                {
                                    max = valueD;
                                }
                                if (valueD < min)
                                {
                                    min = valueD;
                                }
                                if (valueD > max)
                                {
                                    max = valueD;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据名取cookie值
        /// </summary>
        /// <param name="name">cookie名</param>
        /// <returns></returns>
        public string GetCookieByName(string name)
        {
            if (HttpContext.Current.Request.Cookies[name] != null && HttpContext.Current.Request.Cookies[name].Value != "")
            {
                return Server.UrlDecode(HttpContext.Current.Request.Cookies[name].Value);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 正则表达式取值
        /// </summary>
        /// <param name="HtmlCode">源码</param>
        /// <param name="RegexString">正则表达式</param>
        /// <param name="GroupKey">正则表达式分组关键字</param>
        /// <param name="RightToLeft">是否从右到左</param>
        /// <returns></returns>
        public string[] GetRegValue(string HtmlCode, string RegexString, string GroupKey, bool RightToLeft)
        {
            MatchCollection m;
            Regex r;
            if (RightToLeft == true)
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.RightToLeft);
            }
            else
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
            m = r.Matches(HtmlCode);
            string[] MatchValue = new string[m.Count];
            for (int i = 0; i < m.Count; i++)
            {
                MatchValue[i] = m[i].Groups[GroupKey].Value;
            }
            return MatchValue;
        }

        /// <summary>
        /// 根据类型获取发布的路径
        /// </summary>
        /// <param name="publishType">发布类型</param>
        /// <param name="imgUrl">图片路径</param>
        /// <returns></returns>
        public string GetPublishImage(int publishType, string imgUrl, int imgId)
        {
            return new CommonFunction().GetPublishImage(publishType, imgUrl, imgId);
        }

        /// <summary>
        /// 获取log图片
        /// </summary>
        /// <param name="_id">id</param>
        /// <param name="_os">类型 p厂商，b品牌</param>
        /// <param name="_ot">图片类型 b大 ，s小</param>
        /// <returns></returns>
        public string GetLogImage(string _id, string _os, string _ot)
        {
            return "http://image.bitautoimg.com/bt/car/default/CarImage/" + _os + "_" + _id + "_" + _ot + ".jpg";
        }

        #endregion

        #region 接口数据
        /// <summary>
        /// 取所有子品牌(interfaceforbitauto/AllSerialName)
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCSForDLXXInterface()
        {
            string sqlCS = @"SELECT  cs.cs_id, cs.cb_id, cs.cs_name, cs.cs_OtherName, cs.cs_EName,
                                    cs.cs_ShowName, cs.allSpell, cs.OldCb_Id, cs.spell,
                                    (CASE cb.cb_Country
                                       WHEN '中国' THEN '国产'
                                        ELSE '进口'
                                      END ) AS CpCountry, cs.cs_seoname
                            FROM    Car_Serial cs
                                    LEFT JOIN dbo.CarImage ci ON cs.cs_id = ci.SerialId
                                                                 AND ci.IsDefaltImage = 1
                                    LEFT JOIN Car_brand cb ON cs.cb_id = cb.cb_id
                            WHERE cs.IsState >= 1
                                    AND cb.IsState >= 1 ";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCS);
            return _ds;
        }

        /// <summary>
        ///  取国产和进口排行
        /// </summary>
        /// <param name="isImport">是否是进口</param>
        /// <returns></returns>
        public DataSet GetAllSerialSort(int top, bool isImport)
        {
            //string sql = "select top " + top.ToString() + " spr.FeignedPV,cs.cs_id,cs.cs_name,cs.allspell,cs.oldcb_id  ";
            //sql += " from dbo.Serial_PvRank spr ";
            //sql += " left join car_serial cs on spr.cs_id = cs.cs_id";
            //sql += " left join car_brand cb on cs.cb_id = cb.cb_id";
            //sql += " left join Car_Producer cp on cb.cp_id = cp.cp_id";
            //// sql += " left join dbo.VCar_SerialFullInfo cs on spr.cs_id = cs.cs_id ";
            //// sql += " left join dbo.Car_Serial_Item csi on cs.cs_id = csi.cs_id ";
            //if (isImport)
            //{ sql += " where cp.Cp_Country <> '中国' and cs.isState = 1 "; }
            //else
            //{ sql += " where cp.Cp_Country = '中国' and cs.isState = 1 "; }
            //sql += " order by FeignedPV desc ";

            string sql = string.Format(@"SELECT TOP(@TopN) spr.FeignedPV, cs.cs_id, cs.cs_name, cs.allspell, cs.oldcb_id
                                            FROM    dbo.Serial_PvRank spr
                                                    LEFT JOIN car_serial cs ON spr.cs_id = cs.cs_id
                                                    LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                                    LEFT JOIN Car_Producer cp ON cb.cp_id = cp.cp_id
                                            WHERE   {0}  AND cs.isState = 1
                                            ORDER BY FeignedPV DESC", isImport ? "cp.Cp_Country <> '中国'" : " cp.Cp_Country = '中国");
            SqlParameter[] parameters = { new SqlParameter("@TopN", SqlDbType.Int) };
            parameters[0].Value = top;

            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, parameters);
            return ds;
        }

        /// <summary>
        /// 取子品牌信息(interfaceforbitauto/SerialInfo)
        /// </summary>
        /// <param name="isNewID">是新库子品牌ID还是老库品牌ID</param>
        /// <param name="csID">ID(新库子品牌ID或者老库品牌ID)</param>
        /// <returns></returns>
        public DataSet GetSerialInfoByIDForInterface(bool isNewID, int csID)
        {
            //string sql = " select cb.cb_id,cb.cb_name,cb.oldcs_id,cp.cp_id,cp.cp_name,cp.oldcp_id ";//ci.SiteImageId,ci.SiteImageUrl,ci.CommonClassId
            //sql += " ,cs.cs_id,cs.cs_Name,cs.oldcb_id,cs.cs_Virtues,cs.cs_Defect,csi.Prices,csi.ReferPriceRange,csi.Engine_Exhaust,csi.UnderPan_Num_Type,csi.Body_Doors,cmbr.bs_id,cs.cs_CarLevel,bat.bitautoTestURL,cs.allSpell as CSAllSpell,cb.allSpell as CBAllSpell";
            //sql += " from dbo.Car_Serial cs ";
            //sql += " left join dbo.Car_Serial_Item csi on cs.cs_id = csi.cs_id ";
            //sql += " inner join dbo.Car_Brand cb on cs.cb_id = cb.cb_id ";
            //sql += " inner join dbo.Car_MasterBrand_Rel cmbr on cmbr.cb_id = cb.cb_id ";
            //sql += " inner join dbo.Car_Producer cp on cb.cp_id = cp.cp_id ";
            //sql += " left join dbo.BitAutoTest bat on cs.cs_id = bat.cs_id ";
            //// sql += " left join CarImage ci on cs.cs_id = ci.SerialId and ci.IsDefaltImage=1 where cs.IsState>=0 and cb.IsState>=0 and cp.IsState>=0 ";
            //sql += " where cs.IsState>=1 and cb.IsState>=1 and cp.IsState>=1 ";
            //if (isNewID)
            //{
            //    sql = sql + " and cs.cs_id = " + csID.ToString();
            //}
            //else
            //{
            //    sql = sql + " and cs.oldcb_id = " + csID.ToString();
            //}
            string sql = string.Format(@"SELECT  cb.cb_id, cb.cb_name, cb.oldcs_id, ISNULL(cp.cp_id, 0) AS cp_id,
                                                cp.cp_name, ISNULL(cp.oldcp_id, 0) AS oldcp_id, cs.cs_id, cs.cs_Name,
                                                cs.oldcb_id, cs.cs_Virtues, cs.cs_Defect, csi.Prices,
                                                csi.ReferPriceRange, csi.Engine_Exhaust, csi.UnderPan_Num_Type,
                                                csi.Body_Doors, cmbr.bs_id, cs.cs_CarLevel, bat.bitautoTestURL,
                                                cs.allSpell AS CSAllSpell, cb.allSpell AS CBAllSpell
                                        FROM    dbo.Car_Serial cs
                                                LEFT JOIN dbo.Car_Serial_Item csi ON cs.cs_id = csi.cs_id
                                                INNER JOIN dbo.Car_Brand cb ON cs.cb_id = cb.cb_id
                                                INNER JOIN dbo.Car_MasterBrand_Rel cmbr ON cmbr.cb_id = cb.cb_id
                                                INNER JOIN dbo.Car_Producer cp ON cb.cp_id = cp.cp_id
                                                LEFT JOIN dbo.BitAutoTest bat ON cs.cs_id = bat.cs_id
                                        WHERE   cs.IsState >= 1
                                                AND cb.IsState >= 1
                                                AND cp.IsState >= 1 {0} ", isNewID ? "and cs.cs_id =@CsId" : "and cs.oldcb_id = @CsId");
            SqlParameter[] parameters = { new SqlParameter("@CsId", SqlDbType.Int) };
            parameters[0].Value = csID;
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, parameters);
            return ds;
        }

        /// <summary>
        /// 取所有子品牌(interfaceforbitauto/SerialInfo)
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCSForInterface()
        {
            string sqlCS = @"SELECT  a.cs_id, a.cb_id, OldCb_Id, cs_name, a.Spell,
                                        ISNULL(d.cp_id, 0) AS cp_id, bat.bitautoTestURL
                                FROM    Car_Serial a
                                        INNER JOIN dbo.Car_Brand c ON c.cb_id = a.cb_id
                                        INNER JOIN dbo.Car_Producer d ON c.cp_id = d.cp_id
                                        LEFT JOIN dbo.BitAutoTest bat ON a.cs_id = bat.cs_id
                                WHERE   a.IsState >= 1";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCS);
            return _ds;
        }

        /// <summary>
        /// 取所有厂商
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCPForInterface()
        {
            string sqlCP = "select cp_id,OldCp_Id,cp_name,cp_shortname,Cp_Country,Spell from Car_Producer where IsState>=1";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCP);
            return _ds;
        }

        /// <summary>
        /// 取所有品牌
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCBForInterface()
        {
            string sqlCB = "select cb_id,cp_id,OldCs_Id,cb_name,Spell from Car_Brand where IsState>=1";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCB);
            return _ds;
        }

        //        /// <summary>
        //        /// 取所有子品牌
        //        /// </summary>
        //        /// <returns></returns>
        //        public DataSet GetAllCSForInterface()
        //        {
        //            string sqlCS = @"select a.cs_id,a.cb_id,OldCb_Id,CommonClassId,cs_name,a.Spell,d.cp_id,bat.bitautoTestURL from Car_Serial a 
        //                    inner join  dbo.Car_Brand c on c.cb_id = a.cb_id
        //					inner join  dbo.Car_Producer d on c.cp_id = d.cp_id 
        //                    LEFT OUTER JOIN CarImage b on a.cs_id=b.SerialId AND IsDefaltImage = 1 
        //                    left join dbo.BitAutoTest bat on a.cs_id = bat.cs_id
        //                    where a.IsState>=1 ";
        //            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCS);
        //            return _ds;
        //        }

        /// <summary>
        /// 分别去子品牌外观,内饰,默认图(10:外观,11:内饰)
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public DataSet GetSerialImgByIDForInterface(int csID)
        {
            string sql = " select * from dbo.CarImage ";
            sql += " where serialid = " + csID.ToString() + " and CHARINDEX(',11,',ImageProperties) > 0 ";
            sql += " order by SiteImageOrder desc ";
            sql += " select * from dbo.CarImage ";
            sql += " where serialid = " + csID.ToString() + " and CHARINDEX(',10,',ImageProperties) > 0 ";
            sql += " order by SiteImageOrder desc ";
            sql += " select * from dbo.CarImage where serialid = " + csID.ToString() + " and IsDefaltImage=1 ";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 取所有车型
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarForInterface()
        {
            string sqlCar = @" SELECT car_id, a.cs_id, OldCar_Id, car_name, c.cb_id,
                                        ISNULL(d.cp_id, 0) AS cp_id, a.Car_SaleState, a.Car_YearType
                                 FROM   Car_Basic a
                                        INNER JOIN Car_Serial b ON a.cs_id = b.cs_id
                                        INNER JOIN dbo.Car_Brand c ON c.cb_id = b.cb_id
                                        INNER JOIN dbo.Car_Producer d ON c.cp_id = d.cp_id
                                 WHERE  a.IsState >= 1
                                        AND b.IsState >= 1";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCar);
            return _ds;
        }


        /// <summary>
        /// 根据子品牌ID获取车型基本信息
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public DataRow[] GetCarListBySerialId(int serialId)
        {
            string cacheKey = "All_Car_Base_For_SerialId";
            DataSet ds = (DataSet)CacheManager.GetCachedData(cacheKey);
            if (ds == null)
            {
                string sqlStr = "SELECT Car_id,Car_name,cs_id FROM Car_Basic WHERE Car_SaleState<>'停销' AND isState=1";
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
                if (ds != null)
                    CacheManager.InsertCache(cacheKey, ds, 30);
            }
            DataRow[] carRows = null;
            if (ds != null)
                carRows = ds.Tables[0].Select("cs_id=" + serialId);

            return carRows;
        }

        /// <summary>
        /// 取子品牌时间段内特定级别排行(level为空时取所有排行)
        /// </summary>
        /// <param name="level">级别</param>
        /// <param name="currentStartTime">开始时间</param>
        /// <param name="currentEndTime">结束时间</param>
        /// <returns></returns>
        public DataSet GetSerialSortListByTimeAndCarLevel(string level, DateTime currentStartTime, DateTime currentEndTime)
        {
            string sql = " select csp.cs_id,Sum(Pv_SumNum) as Total,Rank() OVER(ORDER  BY Sum(Pv_SumNum) desc) AS Rank ";
            sql += " from  Chart_Serial_Pv csp ";
            if (level != "")
            {
                sql += "inner join Car_Serial cs on csp.cs_id = cs.cs_id and cs.IsState>=1 and cs.cs_carlevel ='" + level + "'";
            }
            sql += "where csp.createDateStr>=@Date1 and csp.createDateStr<@Date2 group by csp.cs_id";

            SqlParameter[] _params = {
                                         new SqlParameter("@Date1",SqlDbType.DateTime),
                                         new SqlParameter("@Date2",SqlDbType.DateTime)
                                     };
            _params[0].Value = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
            _params[1].Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
            return ds;
        }

        /// <summary>
        /// 取子品牌关注(Pv倒序)
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public DataSet GetSerialForSerialByID(int csid)
        {

            string sql = "select a.* from dbo.VCar_SerialDA a inner join (";
            sql += "select cs_id,PCs_Id,Pv_Num from dbo.Serial_To_Serial where cs_id=" + csid.ToString();
            sql += ") b on a.cs_id= b.pcs_id order by Pv_Num desc";

            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 取经销商400
        /// </summary>
        /// <param name="vendorID">经销商ID</param>
        /// <returns>返回400，无则返回空</returns>
        public string GetDealerFor400(string vendorID)
        {
            string dealer400 = "";
            Hashtable hsDealer400 = new Hashtable();
            if (HttpContext.Current.Cache.Get("DealerFor400") != null)
            {
                hsDealer400 = (System.Collections.Hashtable)HttpContext.Current.Cache.Get("DealerFor400");
            }
            else
            {
                try
                {
                    com.bitauto.das.Das dasNf = new BitAuto.CarChannel.Common.com.bitauto.das.Das();
                    DataSet ds = dasNf.GetAllNumber();
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (!hsDealer400.ContainsKey(ds.Tables[0].Rows[i]["VendorID"].ToString()))
                            {
                                hsDealer400.Add(ds.Tables[0].Rows[i]["VendorID"].ToString(), ds.Tables[0].Rows[i]["User400Number"].ToString());
                            }
                        }
                        HttpContext.Current.Cache.Insert("DealerFor400", hsDealer400, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
                    }
                }
                catch
                { }
            }
            if (hsDealer400 != null && hsDealer400.Count > 0)
            {
                if (hsDealer400.ContainsKey(vendorID))
                {
                    dealer400 = Convert.ToString(hsDealer400[vendorID]);
                }
            }
            return dealer400;
        }

        #endregion

        #region 计算工具 根据CarID取主品牌ID，子品牌ID
        public void GetIDsByCarID(int nCarID, out int nBsID, out int nCsID)
        {
            string catchkey = "GetIDsByCarID_" + nCarID;

            object objIDs = null;

            DataSet ds = null;
            CacheManager.GetCachedData(catchkey, out objIDs);
            if (objIDs == null)
            {
                string sql = @"select bs.bs_id, cs.cs_id, c.car_id from Car_MasterBrand bs left join Car_MasterBrand_Rel bsr on bs.bs_id = bsr.bs_id left join Car_Brand cb on bsr.cb_id = cb.cb_id left join 
	                            Car_Serial cs on cs.cb_id = cb.cb_id left join Car_Basic c on c.cs_id = cs.cs_id
                                where c.isState = 1 and cs.isState = 1 and cb.isState = 1 and bs.isState = 1 and c.car_id = " + nCarID.ToString();

                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)objIDs;
            }

            nBsID = -1;
            nCsID = -1;

            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                int nBackCarID = (ds.Tables[0].Rows[0]["car_id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(ds.Tables[0].Rows[0]["car_id"].ToString());
                if (nCarID == nBackCarID)
                {
                    nCarID = nBackCarID;
                    nBsID = (ds.Tables[0].Rows[0]["bs_id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(ds.Tables[0].Rows[0]["bs_id"].ToString());
                    nCsID = (ds.Tables[0].Rows[0]["cs_id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(ds.Tables[0].Rows[0]["cs_id"].ToString());
                }
            }
        }
        #endregion

        #region 取品牌经销商信息

        public DataSet GetBrandDealerInfoByCbID(int cbID)
        {
            string catchkey = "GetBrandDealerInfoByCbID_" + cbID.ToString();
            object GetBrandDealerInfoByCbID = null;
            DataSet dsDealer = new DataSet();
            CacheManager.GetCachedData(catchkey, out GetBrandDealerInfoByCbID);
            if (GetBrandDealerInfoByCbID == null)
            {
                com.bitauto.dealer_VendorInfor.VendorInfor vi = new BitAuto.CarChannel.Common.com.bitauto.dealer_VendorInfor.VendorInfor();
                dsDealer = vi.GetVendorNewsListByBrandId(cbID, -1, -1, "");
                CacheManager.InsertCache(catchkey, dsDealer, 60);
            }
            else
            {
                dsDealer = (DataSet)GetBrandDealerInfoByCbID;
            }
            return dsDealer;
        }

        /// <summary>
        /// 取品牌的城市经销商
        /// </summary>
        /// <param name="cbID">品牌ID</param>
        /// <param name="city">城市ID</param>
        /// <returns></returns>
        public DataSet GetBrandCityDealerInfoByCbID(int cbID, int city)
        {
            string catchkey = "GetBrandDealerInfoByCbID_" + cbID.ToString() + "_" + city.ToString();
            object GetBrandDealerInfoByCbID = null;
            DataSet dsDealer = new DataSet();
            CacheManager.GetCachedData(catchkey, out GetBrandDealerInfoByCbID);
            if (GetBrandDealerInfoByCbID == null)
            {
                try
                {
                    com.bitauto.dealer_VendorInfor.VendorInfor vi = new BitAuto.CarChannel.Common.com.bitauto.dealer_VendorInfor.VendorInfor();
                    dsDealer = vi.GetVendorNewsListByBrandId(cbID, -1, city, "");
                    CacheManager.InsertCache(catchkey, dsDealer, 60);
                }
                catch
                { }
            }
            else
            {
                dsDealer = (DataSet)GetBrandDealerInfoByCbID;
            }
            return dsDealer;
        }

        #endregion

        #region 取品牌、主品牌 论坛

        /// <summary>
        /// 易车会友 论坛接口(王泊)
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="relationID">车与人的关系（ 2：计划购买的 3：拥有的）</param>
        /// <param name="count">需要获取用户数</param>
        /// <returns>返回结果：DataTable(userId:用户ID,username：用户名,userAvatar：用户头像)</returns>
        public DataTable GetUserByCarSerialId(int csID, int relationID, int count)
        {
            DataTable dt = new DataTable();
            string cachekey = "GetUserByCarSerialId_" + csID.ToString() + "_" + relationID.ToString();
            object GetUserByCarSerialId = null;
            CacheManager.GetCachedData(cachekey, out GetUserByCarSerialId);
            if (GetUserByCarSerialId == null)
            {
                try
                {
                    cn.com.baa.ibt.api.UserManagerWebService umws = new BitAuto.CarChannel.Common.cn.com.baa.ibt.api.UserManagerWebService();
                    dt = umws.GetUserByCarSerialId(csID, relationID, count);
                    CacheManager.InsertCache(cachekey, dt, 60);
                }
                catch (Exception ex)
                { }
            }
            else
            {
                dt = (DataTable)GetUserByCarSerialId;
            }
            return dt;
        }

        /*
		/// <summary>
		/// 取主品牌论坛
		/// </summary>
		/// <param name="bsID"></param>
		/// <param name="top"></param>
		/// <returns></returns>
		public DataTable GetMasterBrandForumByBsID(int bsID, int top, ref DataTable dtLink)
		{
			DataTable dt = new DataTable();
			string catchkey = "GetMasterBrandForumByBsID_" + bsID.ToString() + "_" + top.ToString();
			object GetMasterBrandForumByBsID = null;

			string catchkeyRef = "GetMasterBrandForumByBsIDRef_" + bsID.ToString() + "_" + top.ToString();
			object GetMasterBrandForumByBsIDRef = null;

			CacheManager.GetCachedData(catchkey, out GetMasterBrandForumByBsID);
			CacheManager.GetCachedData(catchkeyRef, out GetMasterBrandForumByBsIDRef);

			if (GetMasterBrandForumByBsID == null || GetMasterBrandForumByBsIDRef == null)
			{
				cn.com.baa.api.ForumService fs = new BitAuto.CarChannel.Common.cn.com.baa.api.ForumService();
				dt = fs.GetDegestTopicListBy_BsId("bitauto", bsID, top, 1, ref dtLink);
				CacheManager.InsertCache(catchkey, dt, 60);
				CacheManager.InsertCache(catchkeyRef, dtLink, 60);
			}
			else
			{
				dt = (DataTable)GetMasterBrandForumByBsID;
				dtLink = (DataTable)GetMasterBrandForumByBsIDRef;
			}
            
			return dt;
		}


		/// <summary>
		/// 取品牌论坛
		/// </summary>
		/// <param name="cbID"></param>
		/// <param name="top"></param>
		/// <returns></returns>
		public DataTable GetBrandForumByCbID(int cbID, int top, ref DataTable dtLink)
		{
			DataTable dt = new DataTable();
			string catchkey = "GetBrandForumByCbID_" + cbID.ToString() + "_" + top.ToString();
			object GetBrandForumByCbID = null;

			string catchkeyRef = "GetBrandForumByCbIDRef_" + cbID.ToString() + "_" + top.ToString();
			object GetBrandForumByCbIDRef = null;

			CacheManager.GetCachedData(catchkey, out GetBrandForumByCbID);
			CacheManager.GetCachedData(catchkeyRef, out GetBrandForumByCbIDRef);

			if (GetBrandForumByCbID == null || GetBrandForumByCbIDRef == null)
			{
				cn.com.baa.api.ForumService fs = new BitAuto.CarChannel.Common.cn.com.baa.api.ForumService();
				dt = fs.GetDegestTopicListBy_CbId("bitauto", cbID, top, 1, ref dtLink);
				CacheManager.InsertCache(catchkey, dt, 60);
				CacheManager.InsertCache(catchkeyRef, dtLink, 60);
			}
			else
			{
				dt = (DataTable)GetBrandForumByCbID;
				dtLink = (DataTable)GetBrandForumByCbIDRef;
			}

			return dt;

			//DataTable dt = new DataTable();
			//cn.com.baa.api.ForumService fs = new BitAuto.CarChannel.Common.cn.com.baa.api.ForumService();
			//dt = fs.GetDegestTopicListBy_CbId("bitauto", cbID, top, 1, ref dtLink);
			//return dt;
		}

		/// <summary>
		/// 获取主品牌的大本营论坛地址
		/// </summary>
		/// <param name="masterId"></param>
		/// <returns></returns>
		public string GetMasterbrandCampForumUrl(int masterId)
		{
			string campUrl = "";
			string cacheKey = "MasterBrandCampForumUrlDic";
			Dictionary<int, string> urlDic = (Dictionary<int, string>)CacheManager.GetCachedData(cacheKey);
			if(urlDic == null || !urlDic.ContainsKey(masterId))
			{
				try
				{
					cn.com.baa.api.ForumService fs = new BitAuto.CarChannel.Common.cn.com.baa.api.ForumService();
					DataTable dt = fs.GetCampLinkBy_bs_Id("bitauto", 1, masterId);
					if(dt != null && dt.Rows.Count > 0)
					{
						campUrl = ConvertHelper.GetString(dt.Rows[0]["url"]);
					}
				}
				catch (System.Exception ex)
				{
					
				}

				if (urlDic == null)
				{
					urlDic = new Dictionary<int, string>();
					CacheManager.InsertCache(cacheKey, urlDic, 60);
				}
				urlDic[masterId] = campUrl;
			}
			else
			{
				campUrl = urlDic[masterId];
			}

			return campUrl;
		}


		/// <summary>
		/// 获取品牌的大本营论坛
		/// </summary>
		/// <param name="brandId"></param>
		/// <returns></returns>
		public string GetBrandCampForumUrl(int brandId)
		{
			string campUrl = "";
			string cacheKey = "BrandCampForumUrlDic";
			Dictionary<int, string> urlDic = (Dictionary<int, string>)CacheManager.GetCachedData(cacheKey);
			if (urlDic == null || !urlDic.ContainsKey(brandId))
			{
				try
				{
					cn.com.baa.api.ForumService fs = new BitAuto.CarChannel.Common.cn.com.baa.api.ForumService();
					DataTable dt = fs.GetCampLinkBy_cb_Id("bitauto", 1, brandId);
					if (dt != null && dt.Rows.Count > 0)
					{
						campUrl = ConvertHelper.GetString(dt.Rows[0]["url"]);
					}
				}
				catch (System.Exception ex)
				{

				}

				if (urlDic == null)
				{
					urlDic = new Dictionary<int, string>();
					CacheManager.InsertCache(cacheKey, urlDic, 60);
				}
				urlDic[brandId] = campUrl;
			}
			else
			{
				campUrl = urlDic[brandId];
			}

			return campUrl;
		}
		*/
        #endregion

        #region 取本站静态文件(目前广告使用)

        /// <summary>
        /// 取本站静态文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetBlockContent(string filePath)
        {
            string content = "";
            string cacheKey = "ADManual_" + filePath.ToLower();
            object getAllLevelCityInfo = CacheManager.GetCachedData(cacheKey);
            if (getAllLevelCityInfo != null)
            {
                content = getAllLevelCityInfo.ToString();
            }
            else
            {
                if (File.Exists(filePath))
                {
                    FileStream stream = null;
                    StreamReader sr = null;
                    try
                    {
                        stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        sr = new StreamReader(stream);
                        content = sr.ReadToEnd();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        if (sr != null)
                            sr.Close();
                        if (stream != null)
                            stream.Close();
                    }
                    System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency(filePath);
                    Cache.Insert(cacheKey, content, dep, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                }
            }
            return content;
        }

        #endregion

        /// <summary>
        /// 生成Flash的嵌入代码
        /// </summary>
        /// <param name="swfUrl"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        protected string GetFlashCode(string swfUrl, int width, int height)
        {
            StringBuilder htmlCode = new StringBuilder();
            htmlCode.AppendLine("<div class=\"bt_ad720\"><object height=\"" + height + "\" width=\"" + width + "\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0\" style=\"float: none;\" classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\">");
            htmlCode.AppendLine("<param value=\"" + swfUrl + "\" name=\"movie\">");
            htmlCode.AppendLine("<param value=\"high\" name=\"quality\">");
            htmlCode.AppendLine("<param value=\"opaque\" name=\"wmode\">");
            htmlCode.AppendLine("<embed height=\"" + height + "\" align=\"\" width=\"" + width + "\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" name=\"myMovieName\" quality=\"high\" wmode=\"opaque\" src=\"" + swfUrl + "\">");
            htmlCode.AppendLine("</object></div>");
            return htmlCode.ToString();
        }

        /// <summary>
        /// 设置页面缓存的头输出
        /// </summary>
        /// <param name="interval">单位：分钟</param>
        protected void SetPageCache(int interval)
        {
            if (HttpContext.Current != null && HttpContext.Current.Response.Cache != null)
            {
                HttpContext.Current.Response.Cache.SetNoServerCaching();
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
                HttpContext.Current.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(interval));
            }
        }
        /// <summary>
        /// 设置页面缓存的头输出
        /// </summary>
        /// <param name="interval">单位：分钟</param>
        /// <param name="ignoreParams">是否随参数变化</param>
        protected void SetPageCache(int interval, bool ignoreParams)
        {
            if (HttpContext.Current != null && HttpContext.Current.Response.Cache != null)
            {
                HttpContext.Current.Response.Cache.SetNoServerCaching();
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
                HttpContext.Current.Response.Cache.VaryByParams.IgnoreParams = ignoreParams;
                HttpContext.Current.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(interval));
            }
        }

        #region 城市相关

        public string GetCityURLByCityName(string cityName)
        {
            string url = "http://www.bitauto.com/";
            Hashtable ht = GetAllLevelCityInfoByName();
            if (ht != null && ht.Count > 0)
            {
                if (ht.ContainsKey(cityName.Trim()))
                {
                    City city = (City)ht[cityName.Trim()];
                    if (city.Level <= 2)
                    {
                        url = "http://" + city.CitySpell + ".bitauto.com/";
                    }
                    else
                    {
                        url = "http://www.bitauto.com/" + city.CitySpell + "/";
                    }
                }
            }
            return url;
        }

        /// <summary>
        /// 取所有城市 Hashtable 以城市名为键
        /// </summary>
        /// <returns></returns>
        private Hashtable GetAllLevelCityInfoByName()
        {
            Hashtable ht = new Hashtable();
            string cacheKey = "GetAllLevelCityInfo";
            object getAllLevelCityInfo = CacheManager.GetCachedData(cacheKey);
            if (getAllLevelCityInfo != null)
            {
                ht = (Hashtable)getAllLevelCityInfo;
            }
            else
            {
                string fileName = Server.MapPath("~") + "\\App_Data\\AllLevelCity.xml";
                if (File.Exists(fileName))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(fileName);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            City ci = new City();
                            ci.CityId = Convert.ToInt32(row["CityId"].ToString());
                            ci.CityName = row["CityName"].ToString().Trim();
                            ci.CitySpell = row["EngName"].ToString().Trim().ToLower();
                            ci.Level = Convert.ToInt32(row["Level"].ToString());
                            if (!ht.ContainsKey(ci.CityName))
                            {
                                ht.Add(ci.CityName, ci);
                            }
                        }
                    }

                }
                // 依赖文件缓存
                CacheDependency cacheDependency = new CacheDependency(fileName);
                CacheManager.InsertCache(cacheKey, ht, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
            }
            return ht;
        }

        #endregion

        #region SEO

        /// <summary>
        /// 取子品牌综述页title
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public string GetSerialSummaryTitleByID(int csID)
        {
            string title = "";
            Hashtable ht = GetAllSummaryTitle();
            if (ht.ContainsKey(csID.ToString()))
            {
                title = ht[csID.ToString()].ToString();
            }
            return title;
        }

        /// <summary>
        /// 取子品牌综述页title
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="defaultTitle">匹配不上时的默认title</param>
        /// <returns></returns>
        public string GetSerialSummaryTitleByID(int csID, string defaultTitle)
        {
            string title = defaultTitle;
            Hashtable ht = GetAllSummaryTitle();
            if (ht.ContainsKey(csID.ToString()))
            {
                title = ht[csID.ToString()].ToString();
            }
            return title;
        }

        /// <summary>
        /// 取所有子品牌综述页title
        /// </summary>
        /// <returns></returns>
        private Hashtable GetAllSummaryTitle()
        {
            Hashtable ht = new Hashtable();
            string cacheKey = "PageBase_GetAllSummaryTitle";
            object getAllSummaryTitle = CacheManager.GetCachedData(cacheKey);
            if (getAllSummaryTitle != null)
            {
                ht = (Hashtable)getAllSummaryTitle;
            }
            else
            {
                string fileName = Server.MapPath("~") + "\\App_Data\\SEOTitleForSummary.xml";
                if (File.Exists(fileName))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(fileName);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (!ht.ContainsKey(row["CsID"].ToString()))
                            {
                                ht.Add(row["CsID"].ToString(), row["Title"].ToString());
                            }
                        }
                    }
                    // 依赖文件缓存
                    CacheDependency cacheDependency = new CacheDependency(fileName);
                    CacheManager.InsertCache(cacheKey, ht, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            return ht;
        }

        #endregion

        #region 参数配置页优化

        /// <summary>
        /// 车型参数模板
        /// </summary>
        /// <returns></returns>
        protected Dictionary<int, List<string>> GetCarParameterJsonConfig()
        {
            Dictionary<int, List<string>> dic = new Dictionary<int, List<string>>();
            string cacheKey = "PageBase_GetCarParameterJsonConfig";
            object getCarParameterJsonConfig = CacheManager.GetCachedData(cacheKey);
            if (getCarParameterJsonConfig != null)
            {
                dic = (Dictionary<int, List<string>>)getCarParameterJsonConfig;
            }
            else
            {
                string fileName = Server.MapPath("~") + "\\config\\ParameterForJson.xml";
                if (File.Exists(fileName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileName);
                    if (doc != null && doc.HasChildNodes)
                    {
                        XmlNodeList xnl = doc.SelectNodes("/Param/Group");
                        if (xnl != null && xnl.Count > 0)
                        {
                            int i = 0;
                            foreach (XmlNode xnCate in xnl)
                            {
                                // 大分类
                                if (xnCate.ChildNodes.Count > 0)
                                {
                                    List<string> lp = new List<string>();
                                    // 分类内项
                                    foreach (XmlNode xn in xnCate.ChildNodes)
                                    {
                                        if (xn.NodeType == XmlNodeType.Element
                                            && !lp.Contains(xn.Attributes["Value"].Value.ToString()))
                                        {
                                            lp.Add(xn.Attributes["Value"].Value.ToString());
                                        }
                                    }
                                    if (!dic.ContainsKey(i))
                                    {
                                        dic.Add(i, lp);
                                    }
                                }
                                i++;
                            }
                        }
                    }
                    CacheDependency cacheDependency = new CacheDependency(fileName);
                    CacheManager.InsertCache(cacheKey, dic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            return dic;
        }

        /// <summary>
        /// 车型参数模板
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<string>> GetCarParameterJsonConfigNew()
        {
            Dictionary<int, List<string>> dic = new Dictionary<int, List<string>>();
            string cacheKey = "PageBase_GetCarParameterJsonConfigNew";
            object getCarParameterJsonConfig = CacheManager.GetCachedData(cacheKey);
            if (getCarParameterJsonConfig != null)
            {
                dic = (Dictionary<int, List<string>>)getCarParameterJsonConfig;
            }
            else
            {
                string fileName = Server.MapPath("~") + "\\config\\ParameterForJsonNew.xml";
                if (File.Exists(fileName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileName);
                    if (doc != null && doc.HasChildNodes)
                    {
                        XmlNodeList xnl = doc.SelectNodes("/Param/Group");
                        if (xnl != null && xnl.Count > 0)
                        {
                            int i = 0;
                            foreach (XmlNode xnCate in xnl)
                            {
                                // 大分类
                                if (xnCate.ChildNodes.Count > 0)
                                {
                                    List<string> lp = new List<string>();
                                    // 分类内项
                                    foreach (XmlNode xn in xnCate.ChildNodes)
                                    {
                                        if (xn.NodeType == XmlNodeType.Element
                                            && !lp.Contains(xn.Attributes["Value"].Value.ToString()))
                                        {
                                            lp.Add(xn.Attributes["Value"].Value.ToString());
                                        }
                                    }
                                    if (!dic.ContainsKey(i))
                                    {
                                        dic.Add(i, lp);
                                    }
                                }
                                i++;
                            }
                        }
                    }
                    CacheDependency cacheDependency = new CacheDependency(fileName);
                    CacheManager.InsertCache(cacheKey, dic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            return dic;
        }

        /// <summary>
        /// 车型参数模板
        /// </summary>
        /// <returns></returns>
        protected Dictionary<int, Dictionary<string, CarParam>> GetCarParameterJsonConfigDictionary()
        {
            Dictionary<int, Dictionary<string, CarParam>> dic = new Dictionary<int, Dictionary<string, CarParam>>();
            string cacheKey = "PageBase_GetCarParameterJsonConfigDictionary";
            object getCarParameterJsonConfigDictionary = CacheManager.GetCachedData(cacheKey);
            if (getCarParameterJsonConfigDictionary != null)
            {
                dic = (Dictionary<int, Dictionary<string, CarParam>>)getCarParameterJsonConfigDictionary;
            }
            else
            {
                string fileName = Server.MapPath("~") + "\\config\\ParameterForJson.xml";
                if (File.Exists(fileName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileName);
                    if (doc != null && doc.HasChildNodes)
                    {
                        XmlNodeList xnl = doc.SelectNodes("/Param/Group");
                        if (xnl != null && xnl.Count > 0)
                        {
                            int i = 0;
                            foreach (XmlNode xnCate in xnl)
                            {
                                // 大分类
                                if (xnCate.ChildNodes.Count > 0)
                                {
                                    Dictionary<string, CarParam> dicCp = new Dictionary<string, CarParam>();
                                    // 分类内项
                                    foreach (XmlNode xn in xnCate.ChildNodes)
                                    {
                                        if (xn.NodeType == XmlNodeType.Element
                                        && !dicCp.ContainsKey(xn.Attributes["Value"].Value.ToString()))
                                        {
                                            CarParam cp = new CarParam();
                                            cp.ParamID = int.Parse(xn.Attributes["ParamID"].Value.ToString());
                                            cp.ParamName = xn.Attributes["Desc"].Value.ToString();
                                            cp.AliasName = xn.Attributes["Value"].Value.ToString();
                                            cp.ModuleDec = xn.Attributes["Unit"].Value.ToString();
                                            dicCp.Add(xn.Attributes["Value"].Value.ToString(), cp);
                                        }
                                    }
                                    if (!dic.ContainsKey(i))
                                    {
                                        dic.Add(i, dicCp);
                                    }
                                }
                                i++;
                            }
                        }
                    }
                    CacheDependency cacheDependency = new CacheDependency(fileName);
                    CacheManager.InsertCache(cacheKey, dic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            return dic;
        }
        /// <summary>
        /// 车型参数模板
        /// </summary>
        /// <returns></returns>
        protected Dictionary<int, Dictionary<string, CarParam>> GetCarParameterJsonConfigDictionaryNew()
        {
            Dictionary<int, Dictionary<string, CarParam>> dic = new Dictionary<int, Dictionary<string, CarParam>>();
            string cacheKey = "PageBase_GetCarParameterJsonConfigDictionaryNew";
            object getCarParameterJsonConfigDictionary = CacheManager.GetCachedData(cacheKey);
            if (getCarParameterJsonConfigDictionary != null)
            {
                dic = (Dictionary<int, Dictionary<string, CarParam>>)getCarParameterJsonConfigDictionary;
            }
            else
            {
                string fileName = Server.MapPath("~") + "\\config\\ParameterForJsonNew.xml";
                if (File.Exists(fileName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileName);
                    if (doc != null && doc.HasChildNodes)
                    {
                        XmlNodeList xnl = doc.SelectNodes("/Param/Group");
                        if (xnl != null && xnl.Count > 0)
                        {
                            int i = 0;
                            foreach (XmlNode xnCate in xnl)
                            {
                                // 大分类
                                if (xnCate.ChildNodes.Count > 0)
                                {
                                    Dictionary<string, CarParam> dicCp = new Dictionary<string, CarParam>();
                                    // 分类内项
                                    foreach (XmlNode xn in xnCate.ChildNodes)
                                    {
                                        if (xn.NodeType == XmlNodeType.Element
                                        && !dicCp.ContainsKey(xn.Attributes["Value"].Value.ToString()))
                                        {
                                            CarParam cp = new CarParam();
                                            cp.ParamID = int.Parse(xn.Attributes["ParamID"].Value.ToString());
                                            cp.ParamName = xn.Attributes["Desc"].Value.ToString();
                                            cp.AliasName = xn.Attributes["Value"].Value.ToString();
                                            cp.ModuleDec = xn.Attributes["Unit"].Value.ToString();
                                            dicCp.Add(xn.Attributes["Value"].Value.ToString(), cp);
                                        }
                                    }
                                    if (!dic.ContainsKey(i))
                                    {
                                        dic.Add(i, dicCp);
                                    }
                                }
                                i++;
                            }
                        }
                    }
                    CacheDependency cacheDependency = new CacheDependency(fileName);
                    CacheManager.InsertCache(cacheKey, dic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            return dic;
        }
        #endregion

        /// <summary>
        /// 查找第一条指定分类的新闻
        /// </summary>
        public DataRow GetTopNewsFirstRow(DataTable newsTable, int[] cateIdList)
        {
            Dictionary<int, NewsCategory> categorys = AutoStorageService.GetNewsCategorys();
            if (categorys != null)
            {
                foreach (int cateId in cateIdList)
                {
                    if (categorys.ContainsKey(cateId))
                    {
                        foreach (DataRow row in newsTable.Rows)
                        {
                            if (categorys[cateId].CategoryPath.IndexOf(string.Format(",{0},", row["categoryid"].ToString())) >= 0)
                            {
                                return row;
                            }
                        }
                    }
                }
            }
            return null;
        }

        #region 设置页面分页控件
        public string GetAspNetPager(string urlFormat, int recordCount, int pageSize, int pageIndex)
        {
            StringBuilder newsList = new StringBuilder();
            HtmlTextWriter writer = null;
            StringWriter stringWriter = null;
            try
            {
                stringWriter = new StringWriter(newsList);
                writer = new HtmlTextWriter(stringWriter);
                BitAuto.Controls.Pager pager = new BitAuto.Controls.Pager();

                pager.UrlRewritePattern = urlFormat;
                pager.RecordCount = recordCount;
                pager.DotShowLimit = 8;
                pager.PageSize = pageSize;
                pager.CurrentPageIndex = pageIndex;
                pager.RenderControl(writer);
                return newsList.ToString();
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (stringWriter != null) stringWriter.Close();
                if (writer != null) writer.Close();
            }
        }
        #endregion

        /// <summary>
        /// 获取口碑对比参数
        /// </summary>
        /// <returns></returns>
        protected Dictionary<int, Dictionary<string, string>> GetAllKouBeiCompareParams()
        {
            Dictionary<int, Dictionary<string, string>> carDic = null;
            string cacheKey = "PageBase_GetAllKouBeiCompareParams";
            var obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                carDic = (Dictionary<int, Dictionary<string, string>>)obj;
            }
            else
            {
                string fileName = Path.Combine(WebConfig.DataBlockPath, "Data/Compare/CarComparePrice.xml");
                if (!File.Exists(fileName))
                {
                    CommonFunction.WriteLog("车型对比参数文件不存在，路径：" + fileName);
                    return null;
                }
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(fileName);
                    carDic = new Dictionary<int, Dictionary<string, string>>();
                    XmlNodeList carNodeList = xmlDoc.SelectNodes("root/car");
                    foreach (XmlNode node in carNodeList)
                    {
                        Dictionary<string, string> paramDic = new Dictionary<string, string>();
                        //paramDic.Add("ReferPrice", node.Attributes["referPrice"].Value);
                        paramDic.Add("GouZhiShui", node.Attributes["gouZhiShui"].Value);
                        paramDic.Add("CheChuanShui", node.Attributes["cheChuanShui"].Value);
                        paramDic.Add("BaoXian", node.Attributes["baoXian"].Value);
                        paramDic.Add("ChePai", node.Attributes["chePai"].Value);
                        paramDic.Add("Koubei", node.Attributes["koubei"].Value);
                        paramDic.Add("Price3", node.Attributes["price3"].Value);
                        carDic.Add(ConvertHelper.GetInteger(node.Attributes["id"].Value), paramDic);
                    }
                    CacheManager.InsertCache(cacheKey, carDic, 60 * 24);
                }
                catch (Exception ex)
                {
                    CommonFunction.WriteLog(ex.ToString());
                }
            }
            return carDic;
        }

        /// <summary>
        /// 获取h5车款对比参数
        /// </summary>
        /// <param name="carIds"></param>
        /// <returns></returns>
        protected Dictionary<int, Dictionary<string, string>> GetKouBeiCompareParams(params int[] carIds)
        {
            string cacheKey = "PageBase_GetKouBeiCompareParams_" + string.Join("_", carIds);
            Dictionary<int, Dictionary<string, string>> dic = new Dictionary<int, Dictionary<string, string>>();
            var obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                dic = (Dictionary<int, Dictionary<string, string>>)obj;
            }
            else
            {
                Dictionary<int, Dictionary<string, string>> allParamDic = GetAllKouBeiCompareParams();
                if (allParamDic == null) return dic;
                foreach (int carId in carIds)
                {
                    if (!allParamDic.ContainsKey(carId)) continue;
                    if (!dic.ContainsKey(carId))
                    {
                        dic.Add(carId, allParamDic[carId]);
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
            }
            return dic;
        }

    }
}
