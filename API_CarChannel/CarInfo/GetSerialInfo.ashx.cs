using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.DAL.Data;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using System.Web.Script.Serialization;
using BitAuto.CarChannel.BLL.Data;
using Newtonsoft.Json;
using System.Globalization;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// GetSerialInfo 的摘要说明
    /// dept=getcsuv for wangzt 迁移car域名接口 http://car.bitauto.com/interfaceforbitauto/serial/allserialinfo.aspx?dept=csuv
    /// dept=getcsyear for wangzt 迁移car域名接口 http://car.bitauto.com/car/interfaceforbitauto/CarInfo/CarListGroupByYear.aspx?dept=csyear
    /// dept=getserialyearcolor for wangzt 迁移car域名接口 http://car.bitauto.com/car/interfaceforbitauto/CarInfo/CarInfoList.aspx?dept=serialyearcolor&sid=2573
    /// add dept=gettopnewcar for get new car
    /// add dept=getserialtoserial 取所以子品牌还关注的10个子品牌ID 逗号分隔
    /// add dept=getserialrank 取全国top子品牌 或按城市取top子品牌
    /// add dept=getcscountry 取子品牌自主合资进口德系日系韩系美系欧系其他
    /// add dept=getcswireless 杨立锋移动部门迁移car域名接口 http://car.bitauto.com/car/Interface/iphone/SerialInfoAndCar.aspx?csid=2593
    /// add dept=getrobotcsinfo for liurw 迁移car域名接口 http://car.bitauto.com/interfaceforbitauto/AllSerialInfo.aspx
    /// add dept=getcsforyichehuiv3 for 易车惠V3
    /// add dept=getcspingcenewsurl for 杨立锋 Apr.15.2014 取子品牌详解第1片新闻url，数据来源后台编辑录入
    /// add dept=getcslevelrank for 王志腾 Apr.15..2014 取子品牌全国级别排行
    /// add dept=getallcsinfoforep for 易湃 平台业务部 杨杰 May.19.2014 取子品牌级别信息 
    /// add dept=getserialtoserialdetailbycsid for 车贷迁移
    /// add dept=getcssortbylevelorprice for zhuyx 迁移car域名接口 http://car.bitauto.com/InterfaceForBitAuto/SerialSortByLevelOrPrice.aspx?type=2
    /// add dept=getelectricbycsids for zhuyx 按子品牌ID 取电动车数据
    /// add dept=getcsinfoforcheyisou for liuyk 迁移car域名接口 http://car.bitauto.com/car/interfaceforbitauto/Serial/AllSerialInfo.aspx?dept=bitautocheyisou
    /// add dept=getcssearch for duandl 迁移car域名接口 http://car.bitauto.com/car/interfaceforbitauto/Serial/SerialInfo.aspx?dept=search
    /// add dept=getserialyearinteriorcolor 取内饰颜色，有RGB值的
    /// add dept=getallcsimgandbs for 问答 Nov.6.2015 取所有车型名、白底图、主品牌、logo
    /// add dept=getcskoubeibaseinfo for 前端口碑接口
    /// add dept=getallcsinfoforbaa for 张义刚
    /// add dept=gethotcsbymasterid for 胡贞慧 百度合作 根据主品牌获取热门车系
    /// add dept=getoilelectric for 冯津 油电车数据
    /// add dept=getcsinfoforbbs for 张义刚 迁移car域名接口 http://car.bitauto.com/car/interfaceforbitauto/Serial/AllSerialInfo.aspx?dept=bitautobbs&sale=all
    /// add dept=getpingcebycsid for 王志腾 按车系取评测标签及对应的url
    /// add dept=getallcarspaceinfobycsid for 冯津 车款的关键报告数据接口 按车系id
    /// add dept=getserialbaseinfobyidjson for 照片识别  车系基本信息数据 按车系id
    /// </summary>
    public class GetSerialInfo : PageBase, IHttpHandler
    {
        private HttpRequest request;
        private HttpResponse response;
        private string dept = "";
        private StringBuilder sb = new StringBuilder();
        private Car_SerialBll csb = new Car_SerialBll();

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            response = context.Response;
            request = context.Request;
            if (!string.IsNullOrEmpty(request.QueryString["dept"]))
            { dept = request.QueryString["dept"].ToString().Trim().ToLower(); }

            switch (dept)
            {
                case "getcsuv": RenderCsUVData(); break;
                case "getcscompare": RenderCsCompareData(); break;
                case "getcscitycomparebyid": RenderCsCityCompareData(); break;
                case "getcsyear": RenderCsYearList(); break;
                case "getserialyearcolor": RenderSerialYearColor(); break;
                case "getserialyearinteriorcolor": RenderSerialYearInteriorColor(); break;
                case "gettopnewcar": RenderTopNewCar(); break;
                case "getserialtoserial": RenderSerialToSerial(); break;
                case "getserialtoserialdetailbycsid": RenderSerialToSerialDetailByCsID(); break;
                case "getserialrank": RenderSerialRank(false); break;
                case "getserialrankjson": RenderSerialRank(true); break;
                case "getcscountry": RenderSerialCountry(); break;
                case "getcswireless": RenderSerialWireless(); break;
                case "getrobotcsinfo": RenderRobotCsInfo(); break;
                case "getcsforyichehuiv3": RenderCsInfoForYiCheHui(); break;
                case "getcspingcenewsurl": RenderCsPingceNewsUrl(); break;
                case "getcspingcetag": RenderCsPingceTag(); break;
                case "getcslevelrank": RenderCsLevelRank(); break;
                case "getallcsinfoforep": RenderAllCsInfoForEP(); break;
                case "getcarinfojson": RenderCarInfoJson(); break;
                case "getsamelevelbycsid": RenderSameLevelByCsID(); break;
                case "getcssortbylevelorprice": RenderGetCsSortByLevelorPrice(); break;
                case "getelectricbycsids": RenderGetElectricByCsids(); break;
                case "getoilelectric": RenderGetOilElectric(); break;
                case "getcsinfoforcheyisou": RenderGetCsInfoForCheYiSou(); break;
                case "getcsinfofortaoche": RenderGetCsInfoForCheYiSou(); break;
                case "getcsinfoforbaoma": RenderGetCsInfoForCheYiSou(); break;
                case "getcsinfoforbbs": RenderGetCsInfoForBBS(); break;
                case "getcssearch": RenderGetCsSearch(); break;
                case "getallcsimgandbs": RenderGetAllCsImgAndBs(); break;
                case "getcskoubeibaseinfo": RenderGetCsKouBeiBaseInfo(); break;
                case "getallcsinfoforbaa": RenderAllCsInfoForBAA(); break;
                case "gethotcsbymasterid": RenderHotSerialInfoByMasterId(); break;
                case "getpingcebycsid": RenderCsPingceTagByCsID(); break;
                case "getallcarspaceinfobycsid": RenderAllCarSpaceDataByCsId(); break;
                case "getserialbaseinfobyidjson": RenderSerialBaseInfoByCsId(); break;
                default: CommonFunction.EchoXml(response, "<!-- 缺少参数 -->", ""); ; break;
            }
        }

        /// <summary>
        /// 根据车系id获取车系基本参数
        /// </summary>
        private void RenderSerialBaseInfoByCsId()
        {
            response.ContentType = "application/x-javascript";
            int serialId = ConvertHelper.GetInteger(request.QueryString["csid"]);
            string callback = request.QueryString["callback"];
            string result = "{}";
            if (serialId > 0)
            {
                SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
                if (serialEntity != null)
                {
                    result = JsonConvert.SerializeObject(new
                    {
                        Id = serialId,
                        ShowName = CommonFunction.GetUnicodeByString(serialEntity.ShowName),
                        AllSpell = serialEntity.AllSpell,
                        Price = serialEntity.Price,
                        Image = Car_SerialBll.GetSerialImageUrl(serialId, 6, false)
                    });
                }
            }
            response.Write(string.Format(!string.IsNullOrEmpty(callback) ? (callback + "({0})") : "{0}", result));
        }

        private void RenderCsCityCompareData()
        {
            int serialId = ConvertHelper.GetInteger(request.QueryString["csid"]);
            string cityName = request.QueryString["cityName"];

            if (string.IsNullOrEmpty(cityName))
            {
                cityName = "全国";
            }
            var dict = csb.GetSerialCityCompareList(serialId, HttpContext.Current);
            if (dict.ContainsKey(cityName))
            {
                List<object> list = new List<object>();
                foreach (var item in dict[cityName])
                {
                    list.Add(new { SerialId = item.SerialId, ShowName = item.SerialShowName, AllSpell = item.SerialNameSpell });
                }
                var json = JsonConvert.SerializeObject(list);
                response.Write(json);
            }
            else
            {
                response.Write("{}");
            }
        }
        /// <summary>
        /// 根据主品牌 获取热门车系信息
        /// </summary>
        private void RenderHotSerialInfoByMasterId()
        {
            int bsId = ConvertHelper.GetInteger(request.QueryString["bsid"]);
            int topN = ConvertHelper.GetInteger(request.QueryString["top"]);
            if (topN <= 0) topN = 6;
            if (bsId > 0)
            {
                string sql = @"SELECT TOP (@topN)
									cs.cs_Id, cs.cs_Name, cs.cs_ShowName, cb.allSpell
							FROM    [dbo].[Car_Serial] cs
									LEFT JOIN [dbo].[Car_Serial_30UV] cs30 ON cs.cs_Id = cs30.cs_id
									LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
									LEFT JOIN Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
							WHERE   cs.isState = 1
									AND cb.isState = 1
									AND cmbr.bs_Id = @bsId
							ORDER BY cs30.UVCount DESC";
                SqlParameter[] _params = {
                                     new SqlParameter("@bsId",SqlDbType.Int),
                                     new SqlParameter("@topN",SqlDbType.Int)
                                     };
                _params[0].Value = bsId;
                _params[1].Value = topN;
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                    , CommandType.Text, sql, _params);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sb.Append("<!-- ID:子品牌ID Name:子品牌名 ShowName:子品牌显示名 Pic:子品牌图片 PriceRange:子品牌报价区间 AllSpell:子品牌全拼Url使用 -->");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int csId = ConvertHelper.GetInteger(dr["cs_id"]);
                        sb.AppendLine(string.Format("<Cs ID=\"{0}\" Name=\"{1}\" ShowName=\"{2}\" Pic=\"{3}\" PriceRange=\"{4}\" AllSpell=\"{5}\"/>"
                               , csId
                               , System.Security.SecurityElement.Escape(ConvertHelper.GetString(dr["cs_Name"]))
                               , System.Security.SecurityElement.Escape(ConvertHelper.GetString(dr["cs_ShowName"]))
                               , Car_SerialBll.GetSerialImageUrl(csId).Replace("_2.", "_5.")
                               , base.GetSerialPriceRangeByID(csId)
                               , System.Security.SecurityElement.Escape(ConvertHelper.GetString(dr["allSpell"]))));
                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// BAA 张义刚 取子品牌级别信息
        /// </summary>
        private void RenderAllCsInfoForBAA()
        {
            sb.AppendLine("<!-- Cs:子品牌节点 ID:子品牌ID Name:子品牌名 AllSpell:子品牌全拼(url使用) RP:指导价区间(非停销车型) P:报价区间 Img:白底图 Level:级别 CPCountry:国产or进口 BSCountry:主品牌国别 -->");
            // 子品牌非停销指导价区间
            Dictionary<int, string> dicCsOfficePrice = new Car_SerialBll().GetAllSerialOfficePriceBySaleState(false);
            // 子品牌报价区间
            Dictionary<int, string> dicCsPrice = base.GetAllCsPriceRange();
            // 白底
            Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();

            string sql = @"SELECT  cs.cs_id, cs.cs_name, cs.allspell, cs.cs_showname, cs.cs_CarLevel,
                                    cmb.bs_Country, cb.cb_Country AS Cp_Country
                            FROM    car_serial cs
                                    LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                    LEFT JOIN Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                            WHERE   cs.isState = 1
                            ORDER BY cs.cs_id";

            // DataSet dsCs = new Car_SerialBll().GetAllValidSerial();
            DataSet dsCs = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            if (dsCs != null && dsCs.Tables.Count > 0 && dsCs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsCs.Tables[0].Rows)
                {
                    int csid = int.Parse(dr["cs_id"].ToString());
                    string csName = System.Security.SecurityElement.Escape(dr["cs_name"].ToString().Trim());
                    string csShowName = System.Security.SecurityElement.Escape(dr["cs_showname"].ToString().Trim());
                    string csallSpell = System.Security.SecurityElement.Escape(dr["allspell"].ToString().Trim());
                    string rp = dicCsOfficePrice.ContainsKey(csid) ? dicCsOfficePrice[csid] : "";
                    string p = dicCsPrice.ContainsKey(csid) ? dicCsPrice[csid] : "";
                    string img = dicPicWhite.ContainsKey(csid) ? dicPicWhite[csid] : WebConfig.DefaultCarPic;
                    string level = System.Security.SecurityElement.Escape(dr["cs_CarLevel"].ToString().Trim());
                    string CPCountry = System.Security.SecurityElement.Escape(dr["Cp_Country"].ToString().Trim() == "中国" ? "国产" : "进口");
                    string BSCountry = System.Security.SecurityElement.Escape(dr["bs_Country"].ToString().Trim());
                    sb.AppendLine(string.Format("<Cs ID=\"{0}\" Name=\"{1}\" ShowName=\"{2}\" AllSpell=\"{3}\" RP=\"{4}\" P=\"{5}\" Img=\"{6}\" Level=\"{7}\" CPCountry=\"{8}\" BSCountry=\"{9}\" />"
                        , csid, csName, csShowName, csallSpell, rp, p, img, level, CPCountry, BSCountry));
                }
                CommonFunction.EchoXml(response, sb.ToString(), "Root");
            }
        }

        private void RenderGetCsKouBeiBaseInfo()
        {
            // 如果输出json
            response.ContentType = "application/x-javascript";
            string callback = request.QueryString["callback"];
            List<string> csIDsForJson = new List<string>();
            #region 参数
            List<int> csidList = new List<int>();
            if (request.QueryString["csids"] != null && request.QueryString["csids"].ToString() != "")
            {
                string tempCsids = request.QueryString["csids"].ToString();
                string[] arrayCsids = tempCsids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (arrayCsids.Length > 0)
                {
                    foreach (string strCsid in arrayCsids)
                    {
                        int csid = BitAuto.Utils.ConvertHelper.GetInteger(strCsid);
                        if (csid > 0 && !csidList.Contains(csid) && csidList.Count <= 50)
                        { csidList.Add(csid); }
                    }
                }
            }
            #endregion
            if (csidList.Count > 0)
            {
                Dictionary<int, CsKoubeiBaseInfo> dic = csb.GetAllCsKoubeiBaseInfo();
                foreach (int csid in csidList)
                {
                    if (dic.ContainsKey(csid))
                    {
                        csIDsForJson.Add(string.Format("{0}:{{\"TotalCount\":{1},\"Rating\":{2},\"LevelRating\":{3},\"RatingVariance\":{4},\"Desc\":{{\"KongJian\":{5},\"DongLi\":{6},\"CaoKong\":{7},\"PeiZhi\":{8},\"ShuShiDu\":{9},\"XingJiaBi\":{10},\"WaiGuan\":{11},\"NeiShi\":{12},\"YouHao\":{13}}}}}"
                        , csid, dic[csid].TotalCount
                            , String.Format("{0:F}", dic[csid].Rating)
                            , String.Format("{0:F}", dic[csid].LevelRating)
                            , String.Format("{0:F}", dic[csid].RatingVariance)
                            , (dic[csid].DicSubKoubei.ContainsKey("KongJian") ? String.Format("{0:F}", dic[csid].DicSubKoubei["KongJian"]) : "0")
                            , (dic[csid].DicSubKoubei.ContainsKey("DongLi") ? String.Format("{0:F}", dic[csid].DicSubKoubei["DongLi"]) : "0")
                            , (dic[csid].DicSubKoubei.ContainsKey("CaoKong") ? String.Format("{0:F}", dic[csid].DicSubKoubei["CaoKong"]) : "0")
                            , (dic[csid].DicSubKoubei.ContainsKey("PeiZhi") ? String.Format("{0:F}", dic[csid].DicSubKoubei["PeiZhi"]) : "0")
                            , (dic[csid].DicSubKoubei.ContainsKey("ShuShiDu") ? String.Format("{0:F}", dic[csid].DicSubKoubei["ShuShiDu"]) : "0")
                            , (dic[csid].DicSubKoubei.ContainsKey("XingJiaBi") ? String.Format("{0:F}", dic[csid].DicSubKoubei["XingJiaBi"]) : "0")
                            , (dic[csid].DicSubKoubei.ContainsKey("WaiGuan") ? String.Format("{0:F}", dic[csid].DicSubKoubei["WaiGuan"]) : "0")
                            , (dic[csid].DicSubKoubei.ContainsKey("NeiShi") ? String.Format("{0:F}", dic[csid].DicSubKoubei["NeiShi"]) : "0")
                            , (dic[csid].DicSubKoubei.ContainsKey("YouHao") ? String.Format("{0:F}", dic[csid].DicSubKoubei["YouHao"]) : "0")
                            ));
                    }
                }
            }
            response.Write(string.Format("{0}({{{1}}})", callback, string.Join(",", csIDsForJson.ToArray())));
        }

        /// <summary>
        /// 
        /// </summary>
        private void RenderGetAllCsImgAndBs()
        {
            int csidRequest = 0;
            if (request.QueryString["csID"] != null && request.QueryString["csID"].ToString() != "")
            {
                string csIDstr = request.QueryString["csID"].ToString();
                if (int.TryParse(csIDstr, out csidRequest))
                { }
            }
            sb.AppendLine("<!-- Cs:子品牌节点 ID:子品牌ID Name:子品牌名 ShowName:显示名 AllSpell:子品牌全拼(url使用) RP:指导价区间(非停销车型) P:报价区间 Img:白底图 BsID:主品牌ID BsName:主品牌名 BsUrlSpell:主品牌全拼(url使用) -->");
            if (csidRequest > 0)
            {
                // 子品牌非停销指导价区间
                Dictionary<int, string> dicCsOfficePrice = new Car_SerialBll().GetAllSerialOfficePriceBySaleState(false);
                // 子品牌报价区间
                Dictionary<int, string> dicCsPrice = base.GetAllCsPriceRange();
                // 白底
                Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();

                string sql = @"select cs.cs_id,cs.cs_name,cs.allspell,cs.cs_showname 
							,bs.bs_id,bs.bs_name,bs.urlspell
							from car_serial cs 
							left join car_brand cb 
							on cs.cb_id=cb.cb_id
							left join dbo.Car_MasterBrand_Rel cmbr 
							on cb.cb_id=cmbr.cb_id
							left join car_masterbrand bs
							on cmbr.bs_id=bs.bs_id
							where cs.isState=1 
							and cs.cs_id =@csID";
                SqlParameter[] _param ={
                                      new SqlParameter("@csID",SqlDbType.Int)
                                  };
                _param[0].Value = csidRequest;
                DataSet dsCs = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
                if (dsCs != null && dsCs.Tables.Count > 0 && dsCs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCs.Tables[0].Rows)
                    {
                        int csid = int.Parse(dr["cs_id"].ToString());
                        if (csidRequest > 0 && csidRequest != csid)
                        { continue; }
                        string csName = System.Security.SecurityElement.Escape(dr["cs_name"].ToString().Trim());
                        string csShowName = System.Security.SecurityElement.Escape(dr["cs_showname"].ToString().Trim());
                        string csallSpell = System.Security.SecurityElement.Escape(dr["allspell"].ToString().Trim());
                        string rp = dicCsOfficePrice.ContainsKey(csid) ? dicCsOfficePrice[csid] : "";
                        string p = dicCsPrice.ContainsKey(csid) ? dicCsPrice[csid] : "";
                        int bsid = int.Parse(dr["bs_id"].ToString());
                        string bsName = System.Security.SecurityElement.Escape(dr["bs_name"].ToString().Trim());
                        string bsUrlSpell = System.Security.SecurityElement.Escape(dr["urlspell"].ToString().Trim());
                        string img = dicPicWhite.ContainsKey(csid) ? dicPicWhite[csid] : WebConfig.DefaultCarPic;
                        sb.AppendLine(string.Format("<Cs ID=\"{0}\" Name=\"{1}\" ShowName=\"{2}\" AllSpell=\"{3}\" RP=\"{4}\" P=\"{5}\" Img=\"{6}\" BsID=\"{7}\" BsName=\"{8}\" BsUrlSpell=\"{9}\" />"
                            , csid, csName, csShowName, csallSpell, rp, p, img, bsid, bsName, bsUrlSpell));
                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        private void RenderGetCsSearch()
        {
            string sql = @"SELECT  cs.cs_id, LOWER(cs.csname) AS csname,
                                    LOWER(cs.csOtherName) AS csOtherName, LOWER(cs.csEName) AS csEName,
                                    LOWER(cs.Url) AS Url, LOWER(cs.cs_seoname) AS cs_seoname, cs.virtues,
                                    cs.defect, cs.CsSaleState, LOWER(cs.allSpell) AS allSpell, cb.cb_id,
                                    LOWER(cb.cb_name) AS cb_name, LOWER(cb.cb_seoname) AS cb_seoname,
                                    cmb.bs_id, LOWER(cmb.bs_name) AS bs_name, ISNULL(cp.cp_id, 0) AS cp_id,
                                    LOWER(cp.cp_name) AS cp_name, LOWER(cp.CpShortName) AS CpShortName,
                                    cs3.UVCount, LOWER(cs.csEName) AS csEName,
                                    LOWER(cb.cb_EName) AS cb_EName, LOWER(cb.cb_OtherName) AS cb_OtherName,
                                    LOWER(cmb.bs_EName) AS bs_EName,
                                    LOWER(cmb.bs_OtherName) AS bs_OtherName
                            FROM    car_serial cs
                                    LEFT JOIN dbo.Car_Serial_30UV cs3 ON cs.cs_id = cs3.cs_id
                                    LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN dbo.Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                                                              AND cmbr.isState = 0
                                    LEFT JOIN dbo.Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                                    LEFT JOIN dbo.Car_producer cp ON cb.cp_id = cp.cp_id
                            WHERE   cs.isState = 0
                                    AND cs.carlevel <> 481
                                    AND cb.isState = 0
                            ORDER BY cs3.UVCount DESC, cs.cs_id";
            DataSet ds = new DataSet();
            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.AppendLine("<CS ID=\"" + dr["cs_id"].ToString() + "\"");
                    sb.AppendLine(" Name=\"" + System.Security.SecurityElement.Escape(GetSerialAllGroupName(dr)) + "\" />");
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "SerialInfo");
        }

        #region 生成子品牌的所有组合名

        /// <summary>
        /// 生成子品牌的所有组合名
        /// </summary>
        /// <param name="dr">子品牌数据</param>
        /// <returns></returns>
        private string GetSerialAllGroupName(DataRow dr)
        {
            // string gn = "";
            List<string> listName = new List<string>();

            string csName = dr["csName"].ToString().Trim();
            string cbName = dr["cb_Name"].ToString().Trim();
            string bsName = dr["bs_name"].ToString().Trim();
            string cpName = dr["CpShortName"].ToString().Trim();
            string csSeoName = dr["cs_seoname"].ToString().Trim();
            string csOtherName = dr["csOtherName"].ToString().Trim().Replace("，", ",");

            string csEName = dr["csEName"].ToString().Trim();
            string cb_EName = dr["cb_EName"].ToString().Trim();
            string bs_EName = dr["bs_EName"].ToString().Trim();
            string cb_OtherName = dr["cb_OtherName"].ToString().Trim().Replace("，", ",");
            string bs_OtherName = dr["bs_OtherName"].ToString().Trim().Replace("，", ",");

            #region 子品牌

            // 子品牌名
            GetGroupName(ReplaceSomeCode(csName), ref listName);

            // 子品牌SEO名
            GetGroupName(ReplaceSomeCode(csSeoName), ref listName);

            // 子品牌英文名
            GetGroupName(ReplaceSomeCode(csEName), ref listName);

            // 子品牌别名
            if (csOtherName.Replace("，", ",") != "")
            {
                string[] otherName = csOtherName.Replace("，", ",").Split(',');
                if (otherName.Length > 0)
                {
                    foreach (string name in otherName)
                    {
                        if (name != "")
                        {
                            GetGroupName(ReplaceSomeCode(name), ref listName);

                            // 增加主品牌名
                            if (!CheckNameIsContainOther(ReplaceSomeCode(name), ReplaceSomeCode(bsName)))
                            {
                                GetGroupName(ReplaceSomeCode(bsName) + ReplaceSomeCode(name), ref listName);
                            }

                            // 增加品牌名
                            if (!CheckNameIsContainOther(ReplaceSomeCode(name), ReplaceSomeCode(cbName)))
                            {
                                GetGroupName(ReplaceSomeCode(cbName) + ReplaceSomeCode(name), ref listName);
                            }
                        }
                    }
                }
            }

            #endregion

            #region 品牌
            // 品牌名+子品牌名
            if (cbName != "" && !CheckNameIsContainOther(csName, cbName))
            {
                GetGroupName(ReplaceSomeCode(cbName + csName), ref listName);
            }
            // 品牌英文名+子品牌名
            if (cb_EName != "" && !CheckNameIsContainOther(csName, cb_EName))
            {
                GetGroupName(ReplaceSomeCode(cb_EName + csName), ref listName);
            }
            // 品牌英文名+子品牌英文名
            if (cb_EName != "" && csEName != "" && !CheckNameIsContainOther(csEName, cb_EName))
            {
                GetGroupName(ReplaceSomeCode(cb_EName + csName), ref listName);
            }
            // 品牌别名+子品牌名
            if (cb_OtherName != "")
            {
                string[] otherName = cb_OtherName.Split(',');
                if (otherName.Length > 0)
                {
                    foreach (string name in otherName)
                    {
                        //+子品牌名
                        if (name != "" && !CheckNameIsContainOther(csName, name))
                        {
                            GetGroupName(ReplaceSomeCode(name + csName), ref listName);
                        }
                        // +子品牌英文名
                        if (csEName != "" && name != "" && !CheckNameIsContainOther(csEName, name))
                        {
                            GetGroupName(ReplaceSomeCode(name + csEName), ref listName);
                        }
                    }
                }
            }
            #endregion

            #region 主品牌
            // 主品牌+子品牌名
            if (bsName != "" && !CheckNameIsContainOther(csName, bsName))
            {
                GetGroupName(ReplaceSomeCode(bsName + csName), ref listName);
            }
            // 主品牌英文名+子品牌名
            if (bs_EName != "" && !CheckNameIsContainOther(csName, bs_EName))
            {
                GetGroupName(ReplaceSomeCode(bs_EName + csName), ref listName);
            }
            // 主品牌英文名+子品牌英文名
            if (bs_EName != "" && csEName != "" && !CheckNameIsContainOther(csEName, bs_EName))
            {
                GetGroupName(ReplaceSomeCode(bs_EName + csEName), ref listName);
            }
            // 主品牌别名+子品牌名
            if (bs_OtherName != "")
            {
                string[] otherName = bs_OtherName.Split(',');
                if (otherName.Length > 0)
                {
                    foreach (string name in otherName)
                    {
                        //+子品牌名
                        if (name != "" && !CheckNameIsContainOther(csName, name))
                        {
                            GetGroupName(ReplaceSomeCode(name + csName), ref listName);
                        }
                        // +子品牌英文名
                        if (csEName != "" && name != "" && !CheckNameIsContainOther(csEName, name))
                        {
                            GetGroupName(ReplaceSomeCode(name + csEName), ref listName);
                        }
                    }
                }
            }
            #endregion

            #region 厂商

            if (cpName != "" && !CheckNameIsContainOther(csName, cpName))
            {
                GetGroupName(ReplaceSomeCode(cpName + csName), ref listName);
            }

            #endregion

            //if (listName.Count > 0)
            //{
            //	foreach (string name in listName)
            //	{
            //		if (name != "")
            //		{
            //			if (gn != "")
            //			{ gn += ","; }
            //			gn += name;
            //		}
            //	}
            //}
            return string.Join(",", listName.ToArray());
            // return gn;
        }

        private void GetGroupName(string name, ref List<string> list)
        {
            if (!list.Contains(name))
            {
                list.Add(name);
            }
            if (IsContainSpecialCode(name))
            {
                if (!list.Contains(ReplaceSpecialCode(name)))
                {
                    list.Add(ReplaceSpecialCode(name));
                }
            }
        }

        /// <summary>
        /// 替换一些字符
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string ReplaceSomeCode(string name)
        {
            string replaceName = name.ToLower().Replace("（", "(").Replace("）", ")").Replace("&", "").Replace("\"", "");
            return replaceName;
        }

        /// <summary>
        /// 是否包含特殊字符
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsContainSpecialCode(string name)
        {
            bool isContain = false;
            string RegexString = @"[(|)|·|\-|\.|（|）|！|\!]";
            Regex r = new Regex(RegexString);
            isContain = r.IsMatch(name);
            return isContain;
        }

        /// <summary>
        /// 替换特殊字符
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string ReplaceSpecialCode(string name)
        {
            string replaceName = "";
            string RegexString = @"[(|)|·|\-|\.|（|）|！|\!]";
            Regex r = new Regex(RegexString);
            replaceName = r.Replace(name, "");
            return replaceName;
        }

        /// <summary>
        /// 名字中是否包含另一个名字
        /// </summary>
        /// <param name="name"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private bool CheckNameIsContainOther(string name, string other)
        {
            bool isContain = false;
            if (name.IndexOf(other) >= 0)
            { isContain = true; }
            return isContain;
        }

        #endregion

        /// <summary>
        /// 取子品牌基本信息 for 车易搜 for TaoChe
        /// </summary>
        private void RenderGetCsInfoForCheYiSou()
        {
            string sql = @"select cs.cs_Id,cs.CsSaleState,cs.cs_ShowName,cs.cs_Name,cs.CsRepairPolicy
									,csi.Engine_Exhaust,csi.UnderPan_Num_Type,csi.ReferPriceRange
									from car_serial cs
									left join [Car_Serial_Item] csi on cs.cs_Id=csi.cs_Id
									where cs.IsState=1
									order by cs.cs_id";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                , CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                sb.AppendLine("<!-- ID:子品牌ID CsName:子品牌名 CsShowName:子品牌显示名 ReferPrice:指导价区间 EE:排量 TT:变速器 ZH:综合工况油耗区间 CsRepairPolicy:保修政策-->");
                #region 综合工况油耗
                Dictionary<int, string> dicZHYH = GetAllSerialZHYH();
                #endregion

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int csid = ConvertHelper.GetInteger(dr["cs_Id"].ToString());
                    string csSale = dr["CsSaleState"].ToString().Trim();
                    string csName = dr["cs_Name"].ToString().Trim();
                    string csShowName = dr["cs_ShowName"].ToString().Trim();
                    string EE = dr["Engine_Exhaust"].ToString().Trim();
                    string UT = dr["UnderPan_Num_Type"].ToString().Trim();
                    string RP = dr["ReferPriceRange"].ToString().Trim();
                    string csRepairPolicy = dr["CsRepairPolicy"].ToString().Trim();
                    sb.AppendLine(string.Format("<Cs ID=\"{0}\"  CsName=\"{1}\" CsShowName=\"{2}\" ReferPrice=\"{3}\" EE=\"{4}\" TT=\"{5}\" ZH=\"{6}\" CsRepairPolicy=\"{7}\" />"
                        , csid
                        , System.Security.SecurityElement.Escape(csName)
                        , System.Security.SecurityElement.Escape(csShowName)
                        , (RP != "" ? RP + "万" : "")
                        , EE.Replace("、", ",")
                        , CommonFunction.GetTransbyDataBase(UT, '、', ",")
                        , dicZHYH.ContainsKey(csid) ? dicZHYH[csid] : ""
                        , System.Security.SecurityElement.Escape(csRepairPolicy)
                        ));
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "SerialInfo");
        }

        /// <summary>
        /// 论坛 全部销售状态
        /// </summary>
        private void RenderGetCsInfoForBBS()
        {
            DataSet ds = new DataSet();
            string sql = @"select cs.csname,cs.csshowname,cs.AllSpell,
                            car.car_id,car.cs_id,car.car_ReferPrice,
                            cdb1.pvalue as TT ,cdb2.pvalue as EE
                            from dbo.Car_relation car
                            left join car_serial cs on car.cs_id=cs.cs_id
                            left join dbo.CarDataBase cdb1 on car.car_id=cdb1.carid and cdb1.paramid=712
                            left join dbo.CarDataBase cdb2 on car.car_id=cdb2.carid and cdb2.paramid=785
                            where car.isState=0 and cs.isState=0
                            order by car.cs_id,car.car_ReferPrice";
            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int currentCsid = 0;
                string csName = "";
                string csShowName = "";
                string csAllSpell = "";

                string strRP = "";
                string strMinRP = "";
                string strMaxRP = "";
                string strEE = "";
                string strTT = "";

                SortedList<decimal, decimal> slPrice = new SortedList<decimal, decimal>();
                SortedList<string, string> slTT = new SortedList<string, string>();
                SortedList<string, string> slEE = new SortedList<string, string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int rowCsid = int.Parse(dr["cs_id"].ToString());
                    if (currentCsid != rowCsid)
                    {
                        // 新子品牌
                        if (currentCsid > 0)
                        {

                            #region
                            strRP = "";
                            strMinRP = "";
                            strMaxRP = "";
                            strEE = "";
                            strTT = "";
                            // 指导价
                            if (slPrice.Count > 0)
                            {
                                int loop = 0;
                                foreach (KeyValuePair<decimal, decimal> kvp in slPrice)
                                {
                                    if (loop == 0)
                                    { strMinRP = kvp.Key.ToString(); }
                                    strMaxRP = kvp.Key.ToString();
                                    loop++;
                                }
                                strRP = strMinRP + "万-" + strMaxRP + "万";
                            }
                            // 排量
                            if (slEE.Count > 0)
                            {
                                foreach (KeyValuePair<string, string> kvp in slEE)
                                {
                                    if (strEE != "")
                                    { strEE += ","; }
                                    strEE += kvp.Key + "L";
                                }
                            }
                            // 变速箱
                            if (slTT.Count > 0)
                            {
                                foreach (KeyValuePair<string, string> kvp in slTT)
                                {
                                    if (strTT != "")
                                    { strTT += ","; }
                                    strTT += kvp.Key;
                                }
                            }
                            #endregion

                            // 非第1行
                            sb.AppendLine("<Cs ID=\"" + currentCsid.ToString() + "\" ");
                            sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(csName) + "\" ");
                            sb.Append(" CsShowName=\"" + System.Security.SecurityElement.Escape(csShowName) + "\" ");
                            sb.Append(" ReferPrice=\"" + strRP + "\" ");

                            sb.Append(" PriceRange=\"" + GetSerialPriceRangeByID(currentCsid) + "\" ");
                            sb.Append(" AllSpell=\"" + System.Security.SecurityElement.Escape(csAllSpell) + "\" ");
                            string firstImg = "";
                            List<SerialFocusImage> imgList = csb.GetSerialFocusImageList(currentCsid);
                            if (imgList.Count > 0)
                            {
                                firstImg = String.Format(imgList[0].ImageUrl, 4);
                            }
                            sb.Append(" FirstImg=\"" + firstImg + "\" ");

                            sb.Append(" EE=\"" + strEE + "\" ");
                            sb.Append(" TT=\"" + strTT + "\" />");

                        }

                        currentCsid = rowCsid;
                        csName = System.Security.SecurityElement.Escape(dr["csName"].ToString());
                        csShowName = System.Security.SecurityElement.Escape(dr["csShowName"].ToString());
                        csAllSpell = System.Security.SecurityElement.Escape(dr["AllSpell"].ToString().ToLower());

                        slPrice.Clear();
                        slTT.Clear();
                        slEE.Clear();
                    }
                    decimal rp = 0;
                    if (decimal.TryParse(dr["car_ReferPrice"].ToString(), out rp))
                    {
                        if (rp > 0 && !slPrice.ContainsKey(rp))
                        {
                            slPrice.Add(rp, rp);
                        }
                    }
                    string ee = dr["EE"].ToString();
                    if (ee != "" && !slEE.ContainsKey(ee))
                    { slEE.Add(ee, ee); }
                    string tt = dr["TT"].ToString();
                    if (tt != "" && !slTT.ContainsKey(tt))
                    { slTT.Add(tt, tt); }
                }
                #region
                // 指导价
                strRP = "";
                strMinRP = "";
                strMaxRP = "";
                strEE = "";
                strTT = "";
                if (slPrice.Count > 0)
                {
                    int loop = 0;
                    foreach (KeyValuePair<decimal, decimal> kvp in slPrice)
                    {
                        if (loop == 0)
                        { strMinRP = kvp.Key.ToString(); }
                        strMaxRP = kvp.Key.ToString();
                        loop++;
                    }
                    strRP = strMinRP + "万-" + strMaxRP + "万";
                }
                // 排量
                if (slEE.Count > 0)
                {
                    foreach (KeyValuePair<string, string> kvp in slEE)
                    {
                        if (strEE != "")
                        { strEE += ","; }
                        strEE += kvp.Key + "L";
                    }
                }
                // 变速箱
                if (slTT.Count > 0)
                {
                    foreach (KeyValuePair<string, string> kvp in slTT)
                    {
                        if (strTT != "")
                        { strTT += ","; }
                        strTT += kvp.Key;
                    }
                }
                #endregion
                sb.AppendLine("<Cs ID=\"" + currentCsid.ToString() + "\" ");
                sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(csName) + "\" ");
                sb.Append(" CsShowName=\"" + System.Security.SecurityElement.Escape(csShowName) + "\" ");
                sb.Append(" ReferPrice=\"" + strRP + "\" ");
                {
                    sb.Append(" PriceRange=\"" + GetSerialPriceRangeByID(currentCsid) + "\" ");
                    sb.Append(" AllSpell=\"" + System.Security.SecurityElement.Escape(csAllSpell) + "\" ");
                    string firstImg = "";
                    List<SerialFocusImage> imgList = csb.GetSerialFocusImageList(currentCsid);
                    if (imgList.Count > 0)
                    {
                        firstImg = String.Format(imgList[0].ImageUrl, 4);
                    }
                    sb.Append(" FirstImg=\"" + firstImg + "\" ");
                }
                sb.Append(" EE=\"" + strEE + "\" ");
                sb.Append(" TT=\"" + strTT + "\" />");
            }

            CommonFunction.EchoXml(response, sb.ToString(), "SerialInfo");
        }

        /// <summary>
        /// 取所以子品牌综合油耗
        /// 在销子品牌取其下所有在销车款数据；停销子品牌取最新年款车款；待销取最新年款；待查取空
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, string> GetAllSerialZHYH()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string sql = @"select car.Car_Id,car.Car_YearType,cs.CsSaleState
									,cl.classvalue as carSaleState,cs.cs_Id,cdb.Pvalue
									from Car_relation car
									left join Car_Serial cs 
									on car.Cs_Id=cs.cs_Id
									left join CarDataBase cdb
									on car.Car_Id=cdb.CarId and cdb.ParamId=782
									left join class cl on car.car_SaleState=cl.classid
									where car.IsState=0 and cs.IsState=0 and cdb.Pvalue<>''
									order by cs.cs_Id,car.Car_YearType desc";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
                , CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int lastCsID = 0;
                // 如果是停销 待销 子品牌用最大年款车款
                int maxYear = 0;
                decimal minValue = 0;
                decimal maxValue = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int csid = ConvertHelper.GetInteger(dr["Cs_Id"].ToString());
                    string csSale = dr["CsSaleState"].ToString().Trim();

                    string carSale = dr["carSaleState"].ToString().Trim();
                    string tempZHYH = dr["Pvalue"].ToString().Trim();
                    decimal zhyh = 0;
                    int year = ConvertHelper.GetInteger(dr["Car_YearType"].ToString());

                    if (decimal.TryParse(tempZHYH, out zhyh))
                    { }
                    if (zhyh <= 0)
                    { continue; }
                    if (csSale == "在销")
                    {
                        if (carSale != "在销")
                        { continue; }
                    }
                    else if (csSale == "停销" || csSale == "待销")
                    {
                        if (lastCsID == csid && year < maxYear)
                        { continue; }
                    }
                    else
                    { }
                    if (lastCsID > 0 && lastCsID != csid)
                    {
                        if (maxValue > 0 && minValue > 0)
                        {
                            if (maxValue == minValue)
                            {
                                dic.Add(lastCsID, string.Format("{0}L", maxValue.ToString("F1")));
                            }
                            else
                            {
                                dic.Add(lastCsID, string.Format("{0}-{1}L", minValue.ToString("F1"), maxValue.ToString("F1")));
                            }
                        }
                        else
                        {
                            dic.Add(lastCsID, "");
                        }
                        minValue = 0;
                        maxValue = 0;
                        maxYear = 0;
                    }
                    lastCsID = csid;
                    if (year > maxYear)
                    { maxYear = year; }
                    if (minValue == 0 && maxValue == 0)
                    { minValue = zhyh; maxValue = zhyh; }
                    else
                    {
                        if (zhyh < minValue)
                        { minValue = zhyh; }
                        if (zhyh > maxValue)
                        { maxValue = zhyh; }
                    }
                }
                if (lastCsID > 0)
                {
                    if (maxValue > 0 && minValue > 0)
                    {
                        if (maxValue == minValue)
                        {
                            dic.Add(lastCsID, string.Format("{0}L", maxValue.ToString("F1")));
                        }
                        else
                        {
                            dic.Add(lastCsID, string.Format("{0}-{1}L", minValue.ToString("F1"), maxValue.ToString("F1")));
                        }
                    }
                    else
                    {
                        dic.Add(lastCsID, "");
                    }
                }
            }
            return dic;
        }

        private void RenderGetElectricByCsids()
        {
            // 883 	纯电最高续航里程
            // 876	电池容量
            // 879	普通充电时间
            // 878  快速充电时间
            // 954	充电方式

            List<int> listCsIDs = new List<int>();
            #region 请求的子品牌ID
            if (request.QueryString["csids"] != null && request.QueryString["csids"].ToString() != "")
            {
                string strCsIds = request.QueryString["csids"].ToString();
                string[] arrCsIDs = strCsIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (arrCsIDs.Length > 0)
                {
                    foreach (string strid in arrCsIDs)
                    {
                        int csid = 0;
                        if (int.TryParse(strid, out csid))
                        {
                            if (csid > 0 && !listCsIDs.Contains(csid) && listCsIDs.Count < 20)
                            {
                                listCsIDs.Add(csid);
                            }
                        }
                    }
                }
            }
            #endregion

            sb.AppendLine("<!-- CsID:子品牌ID CsName:子品牌名 CsShowName:子品牌显示名 AllSpell:子品牌全拼地址使用 Price:报价区间 Img:白底图 EM:续航里程 EB:电池容量 EN:充电时间(普通) EF:充电时间(快速) EC:充电方式 -->");

            // 每个子品牌的数据
            string sql = @"select
									cs.cs_id,cs.csname,cs.csshowname,cs.allspell
									,cdb1.pvalue as EM,cdb2.pvalue as EB,cdb3.pvalue as EN
									,cdb4.pvalue as EC,cdb5.pvalue as EF
									from car_relation car
									left join car_serial cs 
									on car.cs_id=cs.cs_id
									left join cardatabase cdb1 on cdb1.carid=car.car_id and cdb1.paramid=883
									left join cardatabase cdb2 on cdb2.carid=car.car_id and cdb2.paramid=876
									left join cardatabase cdb3 on cdb3.carid=car.car_id and cdb3.paramid=879
									left join cardatabase cdb4 on cdb4.carid=car.car_id and cdb4.paramid=954
									left join cardatabase cdb5 on cdb5.carid=car.car_id and cdb5.paramid=878
									where cs.cs_id =@csID and car.car_SaleState in (95,97)
									and car.isstate=0 and cs.isState=0
									order by em desc,car.car_id";
            if (listCsIDs != null && listCsIDs.Count > 0)
            {
                // 白底
                Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();
                foreach (int csid in listCsIDs)
                {
                    SqlParameter[] _param ={
                                      new SqlParameter("@csID",SqlDbType.Int)
                                  };
                    _param[0].Value = csid;
                    DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
                        , CommandType.Text, sql, _param);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string maxEM = "";
                        string minEM = "";
                        string maxEB = "";
                        string minEB = "";
                        string maxEN = "";
                        string minEN = "";
                        string maxEF = "";
                        string minEF = "";
                        string EC = "";
                        string csName = "";
                        string csShowName = "";
                        string img = dicPicWhite.ContainsKey(csid) ? dicPicWhite[csid] : WebConfig.DefaultCarPic;
                        string allSpell = "";
                        string price = base.GetSerialPriceRangeByID(csid);
                        int loop = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (loop == 0)
                            {
                                csName = dr["csname"].ToString().Trim();
                                csShowName = dr["csshowname"].ToString().Trim();
                                allSpell = dr["allspell"].ToString().Trim();
                                EC = dr["EC"].ToString().Trim();
                            }
                            #region EM
                            string tempEM = dr["EM"].ToString().Trim();
                            if (tempEM != "" && BitAuto.Utils.ConvertHelper.GetDecimal(tempEM) > 0)
                            {
                                if (maxEM != "" && minEM != "")
                                {
                                    if (BitAuto.Utils.ConvertHelper.GetDecimal(tempEM) > BitAuto.Utils.ConvertHelper.GetDecimal(maxEM))
                                    { maxEM = tempEM; }
                                    if (BitAuto.Utils.ConvertHelper.GetDecimal(tempEM) < BitAuto.Utils.ConvertHelper.GetDecimal(minEM))
                                    { minEM = tempEM; }
                                }
                                else
                                {
                                    maxEM = tempEM;
                                    minEM = tempEM;
                                }
                            }
                            #endregion

                            #region EB
                            string tempEB = dr["EB"].ToString().Trim();
                            if (tempEB != "" && BitAuto.Utils.ConvertHelper.GetDecimal(tempEB) > 0)
                            {
                                if (maxEB != "" && minEB != "")
                                {
                                    if (BitAuto.Utils.ConvertHelper.GetDecimal(tempEB) > BitAuto.Utils.ConvertHelper.GetDecimal(maxEB))
                                    { maxEB = tempEB; }
                                    if (BitAuto.Utils.ConvertHelper.GetDecimal(tempEB) < BitAuto.Utils.ConvertHelper.GetDecimal(minEB))
                                    { minEB = tempEB; }
                                }
                                else
                                {
                                    maxEB = tempEB;
                                    minEB = tempEB;
                                }
                            }
                            #endregion

                            #region EN
                            string tempEN = dr["EN"].ToString().Trim();
                            if (tempEN != "" && BitAuto.Utils.ConvertHelper.GetDecimal(tempEN) > 0)
                            {
                                if (maxEN != "" && minEN != "")
                                {
                                    if (BitAuto.Utils.ConvertHelper.GetDecimal(tempEN) > BitAuto.Utils.ConvertHelper.GetDecimal(maxEN))
                                    { maxEN = tempEN; }
                                    if (BitAuto.Utils.ConvertHelper.GetDecimal(tempEN) < BitAuto.Utils.ConvertHelper.GetDecimal(minEN))
                                    { minEN = tempEN; }
                                }
                                else
                                {
                                    maxEN = tempEN;
                                    minEN = tempEN;
                                }
                            }
                            #endregion

                            #region EF
                            string tempEF = dr["EF"].ToString().Trim();
                            if (tempEF != "" && BitAuto.Utils.ConvertHelper.GetDecimal(tempEF) > 0)
                            {
                                if (maxEF != "" && minEF != "")
                                {
                                    if (BitAuto.Utils.ConvertHelper.GetDecimal(tempEF) > BitAuto.Utils.ConvertHelper.GetDecimal(maxEF))
                                    { maxEF = tempEF; }
                                    if (BitAuto.Utils.ConvertHelper.GetDecimal(tempEF) < BitAuto.Utils.ConvertHelper.GetDecimal(minEF))
                                    { minEF = tempEF; }
                                }
                                else
                                {
                                    maxEF = tempEF;
                                    minEF = tempEF;
                                }
                            }
                            #endregion

                            loop++;
                        }

                        sb.AppendLine(string.Format("<Item CsID=\"{0}\" CsName=\"{1}\" CsShowName=\"{2}\" AllSpell=\"{3}\" Price=\"{4}\" Img=\"{5}\" EM=\"{6}\" EB=\"{7}\" EN=\"{8}\" EF=\"{9}\" EC=\"{10}\"/>"
                            , csid
                            , System.Security.SecurityElement.Escape(csName)
                            , System.Security.SecurityElement.Escape(csShowName)
                            , System.Security.SecurityElement.Escape(allSpell)
                            , System.Security.SecurityElement.Escape(price)
                            , System.Security.SecurityElement.Escape(img)
                            , System.Security.SecurityElement.Escape((maxEM != "" && minEM != "") ? minEM + "-" + maxEM : "")
                            , System.Security.SecurityElement.Escape((maxEB != "" && minEB != "") ? minEB + "-" + maxEB : "")
                            , System.Security.SecurityElement.Escape((maxEN != "" && minEN != "") ? minEN + "-" + maxEN : "")
                            , System.Security.SecurityElement.Escape((maxEF != "" && minEF != "") ? minEF + "-" + maxEF : "")
                            , System.Security.SecurityElement.Escape(EC)
                            ));

                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        private void RenderGetOilElectric()
        {
            sb.AppendLine("<!-- CsID:子品牌ID CsName:子品牌名 CsShowName:子品牌显示名 AllSpell:子品牌全拼地址使用 Price:报价区间 Img:白底图 EM:最大功率 PZ:综合工况 EP:电机最大功率 NE:新能源类型 -->");

            // 998 新能源类型 
            // 782 综合工况油耗
            // 430 最大功率-功率值
            // 870 电机最大功率

            string sqlGetOilElectricByCarID = @"select paramid,pvalue
			from CarDataBase
			where carid=@carID and paramid in (998,782,430,870)";
            SqlParameter[] _param ={
                                      new SqlParameter("@carID",SqlDbType.Int)
                                  };

            string sqlGetOilElectricCarIDs = @"select car.Car_Id,car.Car_Name,cs.cs_Id,cs.csName,cs.csShowName,cs.allSpell
															from (
															select carid from CarDataBase
															where 
															paramid = 578
															and Pvalue='油电混合动力') t
															left join Car_relation car on t.CarId=car.Car_Id
															left join Car_Serial cs on car.Cs_Id=cs.cs_Id
															left join Car_Serial_30UV cs30 on cs.cs_Id=cs30.cs_id
															where car.IsState=0 and cs.IsState=0 and car.car_SaleState=95
															order by cs30.UVCount desc";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
                , CommandType.Text, sqlGetOilElectricCarIDs);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // 白底
                Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();

                string lastCsName = "";
                string lastCsShowName = "";
                string lastCsAllSpell = "";
                int lastCsID = 0;
                string minEM = "";
                string maxEM = "";
                string minPZ = "";
                string maxPZ = "";
                string minEP = "";
                string maxEP = "";
                //string minNE = "";
                //string maxNE = "";
                List<string> listNE = new List<string>();
                int loopCount = 20;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int carid = BitAuto.Utils.ConvertHelper.GetInteger(dr["Car_Id"].ToString());
                    int currentCsid = BitAuto.Utils.ConvertHelper.GetInteger(dr["cs_Id"].ToString());
                    if (lastCsID > 0 && lastCsID != currentCsid)
                    {
                        sb.AppendLine(string.Format("<Item CsID=\"{0}\" CsName=\"{1}\" CsShowName=\"{2}\" AllSpell=\"{3}\" Price=\"{4}\" Img=\"{5}\" EM=\"{6}\" PZ=\"{7}\" EP=\"{8}\" NE=\"{9}\" />"
                        , lastCsID
                        , System.Security.SecurityElement.Escape(lastCsName)
                        , System.Security.SecurityElement.Escape(lastCsShowName)
                        , System.Security.SecurityElement.Escape(lastCsAllSpell)
                        , System.Security.SecurityElement.Escape(base.GetSerialPriceRangeByID(lastCsID))
                        , System.Security.SecurityElement.Escape(dicPicWhite.ContainsKey(lastCsID) ? dicPicWhite[lastCsID].Replace("_2.", "_6.") : WebConfig.DefaultCarPic)
                        , System.Security.SecurityElement.Escape((maxEM != "" && minEM != "") ? minEM + "-" + maxEM : "")
                        , System.Security.SecurityElement.Escape((maxPZ != "" && minPZ != "") ? minPZ + "-" + maxPZ : "")
                        , System.Security.SecurityElement.Escape((maxEP != "" && minEP != "") ? minEP + "-" + maxEP : "")
                        , System.Security.SecurityElement.Escape((listNE.Count > 0 ? string.Join(",", listNE.ToArray()) : ""))
                        ));
                        loopCount--;
                        if (loopCount <= 0)
                        { break; }
                        minEM = "";
                        maxEM = "";
                        minPZ = "";
                        maxPZ = "";
                        minEP = "";
                        maxEP = "";
                        listNE.Clear();
                    }
                    lastCsName = dr["csName"].ToString().Trim();
                    lastCsShowName = dr["csShowName"].ToString().Trim();
                    lastCsAllSpell = dr["allSpell"].ToString().Trim();
                    lastCsID = currentCsid;
                    _param[0].Value = carid;
                    DataSet dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
                        , CommandType.Text, sqlGetOilElectricByCarID, _param);
                    if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drCar in dsCar.Tables[0].Rows)
                        {
                            switch (drCar["paramid"].ToString())
                            {
                                case "430": GetMinOrMax(drCar["pvalue"].ToString().Trim(), ref minEM, ref maxEM); break;
                                case "782": GetMinOrMax(drCar["pvalue"].ToString().Trim(), ref minPZ, ref maxPZ); break;
                                case "870": GetMinOrMax(drCar["pvalue"].ToString().Trim(), ref minEP, ref maxEP); break;
                                case "998": if (!listNE.Contains(drCar["pvalue"].ToString().Trim())) { listNE.Add(drCar["pvalue"].ToString().Trim()); }; break;
                                default: break;
                            }
                        }
                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        private void GetMinOrMax(string currentValue, ref string minValue, ref string maxValue)
        {
            if (currentValue != "" && BitAuto.Utils.ConvertHelper.GetDecimal(currentValue) > 0)
            {
                if (maxValue != "" && minValue != "")
                {
                    if (BitAuto.Utils.ConvertHelper.GetDecimal(currentValue) > BitAuto.Utils.ConvertHelper.GetDecimal(maxValue))
                    { maxValue = currentValue; }
                    if (BitAuto.Utils.ConvertHelper.GetDecimal(currentValue) < BitAuto.Utils.ConvertHelper.GetDecimal(minValue))
                    { minValue = currentValue; }
                }
                else
                {
                    maxValue = currentValue;
                    minValue = currentValue;
                }
            }
        }

        /// <summary>
        /// 迁移car域名接口
        /// </summary>
        private void RenderGetCsSortByLevelorPrice()
        {
            #region 参数
            int type = 0;
            if (request.QueryString["type"] != null && request.QueryString["type"].ToString() != "")
            {
                string typestr = request.QueryString["type"].ToString();
                if (int.TryParse(typestr, out type))
                {
                    if (type < 1 || type > 6)
                    {
                        type = 1;
                    }
                }
            }

            int cityID = 0;
            if (request.QueryString["cityID"] != null && request.QueryString["cityID"].ToString() != "")
            {
                string cityIDstr = request.QueryString["cityID"].ToString();
                if (int.TryParse(cityIDstr, out cityID))
                {
                    if (cityID < 1)
                    {
                        cityID = 0;
                    }
                }
            }
            #endregion

            if (type == 3)
            {
                // 取全部子品牌前5
                GenerateAllSerialTop5();
            }
            else if (type == 4)
            {
                // 取全部子品牌的各级别及部分级别排行
                GenerateAllSerialSortForLevelAndNoLevel();
            }
            else
            {
                if (cityID > 0)
                {
                    // 取特定城市
                    GetTop10ByCityID(type, cityID);
                }
                else
                {
                    // 取全国
                    GetAllCityFor(type);
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        #region

        private void GenerateAllSerialTop5()
        {
            DataSet ds = new DataSet();
            string sql = " select top 5 t1.*,cs.cs_name,cs.cs_showname,cs.allSpell ";
            sql += " from ( ";
            sql += " select csID,sum(uvcount)as uv from dbo.StatisticSerialPVUVCity ";
            sql += " group by csID )t1 ";
            sql += " left join dbo.car_serial cs on t1.csid=cs.cs_id ";
            sql += " order by uv desc ";
            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sb.Append("<Serial ID=\"" + ds.Tables[0].Rows[i]["csID"].ToString().Trim() + "\" ");
                    sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["cs_name"].ToString().Trim()) + "\" ");
                    sb.Append(" ShowName=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["cs_showname"].ToString().Trim()) + "\" ");
                    sb.Append(" CsUV=\"" + ds.Tables[0].Rows[i]["uv"].ToString().Trim() + "\" ");
                    sb.Append(" CsAllSpell=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["allSpell"].ToString().Trim().ToLower()) + "\" />");
                }
            }

        }

        private void GenerateAllSerialSortForLevelAndNoLevel()
        {
            StringBuilder sbAlllevel = new StringBuilder();
            sbAlllevel.Append("<AllLevel><PVRank SerialList=\"");
            // StringBuilder sbLevel = new StringBuilder();
            List<EnumCollection.SerialSortForInterface> lssfi = base.GetAllSerialNewly30DayToList();
            List<EnumCollection.SerialSortForInterface>[] arrSSfi = new List<EnumCollection.SerialSortForInterface>[10];
            for (int i = 0; i < 10; i++)
            {
                List<EnumCollection.SerialSortForInterface> ll1 = new List<EnumCollection.SerialSortForInterface>();
                arrSSfi[i] = ll1;
            }
            int loop = 0;
            foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
            {
                if (loop == 0)
                {
                    sbAlllevel.Append(ssfi.CsID.ToString());
                }
                else
                {
                    sbAlllevel.Append("," + ssfi.CsID.ToString());
                }
                loop++;

                if (ssfi.CsLevel == "微型车")
                { arrSSfi[0].Add(ssfi); }
                if (ssfi.CsLevel == "小型车")
                { arrSSfi[1].Add(ssfi); }
                if (ssfi.CsLevel == "紧凑型车")
                { arrSSfi[2].Add(ssfi); }
                if (ssfi.CsLevel == "中型车")
                { arrSSfi[3].Add(ssfi); }
                if (ssfi.CsLevel == "中大型车")
                { arrSSfi[4].Add(ssfi); }
                if (ssfi.CsLevel == "豪华车")
                { arrSSfi[5].Add(ssfi); }

                if (ssfi.CsLevel == "MPV")
                { arrSSfi[6].Add(ssfi); }
                if (ssfi.CsLevel == "SUV")
                { arrSSfi[7].Add(ssfi); }

                if (ssfi.CsLevel == "跑车")
                { arrSSfi[8].Add(ssfi); }
                if (ssfi.CsLevel == "面包车")
                { arrSSfi[9].Add(ssfi); }
            }
            sbAlllevel.Append("\" /></AllLevel>");

            for (int i = 0; i < arrSSfi.Length; i++)
            {
                string levelName = GetLevelName(i);
                int levelId = CarLevelDefine.GetLevelIdByName(levelName);
                sb.Append("<Level ID=\"" + levelId + "\" Name=\"" + levelName + "\">\r\n");
                if (arrSSfi[i].Count > 0)
                {
                    sb.Append("<PVRank SerialList=\"");
                    int loopLevel = 0;
                    foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
                    {
                        if (loopLevel == 0)
                        { sb.Append(ss.CsID.ToString()); }
                        else
                        { sb.Append("," + ss.CsID.ToString()); }
                        loopLevel++;
                    }
                    sb.Append("\" />");
                }

                //新车
                List<EnumCollection.NewCarForLevel> lncfl = base.GetLevelNewCarsByLevelID(levelId);
                string newSerials = ",";
                sb.Append("<NewCarRank SerialList=\"");
                int serialNum = 0;
                foreach (EnumCollection.NewCarForLevel ncfl in lncfl)
                {
                    if (newSerials.IndexOf("," + ncfl.CsID.ToString() + ",") >= 0)
                    {
                        continue;
                    }
                    newSerials += ncfl.CsID + ",";
                    if (serialNum == 0)
                        sb.Append(ncfl.CsID.ToString());
                    else
                        sb.Append("," + ncfl.CsID);
                    serialNum++;
                }

                //不足时补充按PV排序的子品牌
                int needNewCarNum = 20;
                if (serialNum < needNewCarNum && arrSSfi[i].Count > 0)
                {
                    foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
                    {
                        if (serialNum == 0)
                        { sb.Append(ss.CsID.ToString()); }
                        else
                        { sb.Append("," + ss.CsID.ToString()); }
                        serialNum++;
                        if (serialNum >= needNewCarNum)
                            break;
                    }
                }
                sb.Append(" \" />");
                sb.Append("</Level>");
            }
            sb.Append(sbAlllevel.ToString());
        }

        private string GetLevelName(int index)
        {
            string name = "";
            switch (index)
            {
                case 0: name = "微型车"; break;
                case 1: name = "小型车"; break;
                case 2: name = "紧凑型车"; break;
                case 3: name = "中型车"; break;
                case 4: name = "中大型车"; break;
                case 5: name = "豪华车"; break;
                case 6: name = "MPV"; break;
                case 7: name = "SUV"; break;
                case 8: name = "跑车"; break;
                case 9: name = "面包车"; break;
                case 10: name = "皮卡"; break;
                case 11: name = "其它"; break;
                default: name = ""; break;
            }
            return name;
        }

        private void GetTop10ByCityID(int type, int cityID)
        {
            XmlDocument doc = new XmlDocument();
            // 按价格
            if (type == 1)
            {
                string cacheName = "interfaceforbitauto_SerialSortByLevelOrPrice_priceRangeDataTop10";
                string dataPath = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialCityPVPriceRangeTop10.xml");
                if (HttpContext.Current.Cache[cacheName] != null)
                {
                    doc = (XmlDocument)HttpContext.Current.Cache[cacheName];
                }
                else
                {
                    try
                    {
                        doc.Load(dataPath);
                        System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency(dataPath);
                        HttpContext.Current.Cache.Insert(cacheName, doc, dep, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                    }
                    catch (Exception ex)
                    { }
                }
                GenerateCityPricerangeSort(doc, cityID);
            }
            // 按级别
            if (type == 2)
            {
                string cacheName = "interfaceforbitauto_SerialSortByLevelOrPrice_levelDataTop10";
                string dataPath = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialCityPVLevelTop10.xml");
                if (HttpContext.Current.Cache[cacheName] != null)
                {
                    doc = (XmlDocument)HttpContext.Current.Cache[cacheName];
                }
                else
                {
                    doc.Load(dataPath);
                    System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency(dataPath);
                    HttpContext.Current.Cache.Insert(cacheName, doc, dep, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                }
                GenerateCityLevelSort(doc, cityID);
            }

        }

        /// <summary>
        /// 生成特定城市级别排行
        /// </summary>
        /// <param name="doc"></param>
        private void GenerateCityLevelSort(XmlDocument doc, int cityID)
        {
            if (doc != null && doc.HasChildNodes)
            {
                XmlNodeList xnl = doc.SelectNodes("/SerialSort/City[@ID='" + cityID.ToString() + "']");

                if (xnl != null && xnl.Count > 0 && xnl[0].ChildNodes.Count > 0)
                {
                    GenerateLevelDataFormXmlNode(xnl, "微型车");
                    GenerateLevelDataFormXmlNode(xnl, "小型车");
                    GenerateLevelDataFormXmlNode(xnl, "紧凑型车");
                    GenerateLevelDataFormXmlNode(xnl, "中型车");
                    GenerateLevelDataFormXmlNode(xnl, "中大型车");
                    GenerateLevelDataFormXmlNode(xnl, "豪华车");
                    GenerateLevelDataFormXmlNode(xnl, "SUV");
                    GenerateLevelDataFormXmlNode(xnl, "MPV");
                    GenerateLevelDataFormXmlNode(xnl, "跑车");
                }
                else
                {
                    // 如果此城市取不到数据 则去北京的数据
                    xnl = doc.SelectNodes("/SerialSort/City[@ID='201']");
                    if (xnl != null && xnl.Count > 0 && xnl[0].ChildNodes.Count > 0)
                    {
                        GenerateLevelDataFormXmlNode(xnl, "微型车");
                        GenerateLevelDataFormXmlNode(xnl, "小型车");
                        GenerateLevelDataFormXmlNode(xnl, "紧凑型车");
                        GenerateLevelDataFormXmlNode(xnl, "中型车");
                        GenerateLevelDataFormXmlNode(xnl, "中大型车");
                        GenerateLevelDataFormXmlNode(xnl, "豪华车");
                        GenerateLevelDataFormXmlNode(xnl, "SUV");
                        GenerateLevelDataFormXmlNode(xnl, "MPV");
                        GenerateLevelDataFormXmlNode(xnl, "跑车");
                    }
                }
            }
        }

        private void GenerateLevelDataFormXmlNode(XmlNodeList xnl, string levelName)
        {
            sb.Append("<Group Name=\"" + levelName + "\">");
            XmlNodeList xnlLevel = xnl[0].SelectNodes("Level[@Name='" + levelName + "']");
            if (xnlLevel != null && xnlLevel.Count > 0 && xnlLevel[0].ChildNodes.Count > 0)
            {
                foreach (XmlNode xn in xnlLevel[0])
                {
                    sb.Append("<Serial ID=\"" + xn.Attributes["ID"].Value.Trim() + "\" ");
                    sb.Append(" CsName=\"" + xn.Attributes["CsName"].Value.Trim() + "\" ");
                    sb.Append(" CsShowName=\"" + xn.Attributes["CsShowName"].Value.Trim() + "\" ");
                    sb.Append(" CsAllSpell=\"" + xn.Attributes["CsAllSpell"].Value.Trim().ToLower() + "\" ");
                    sb.Append(" CsAll=\"" + xn.Attributes["CsAllSpell"].Value.Trim().ToLower() + "\" ");
                    sb.Append(" CsLevel=\"" + xn.Attributes["CsLevel"].Value.Trim() + "\" ");
                    sb.Append(" CsPV=\"" + xn.Attributes["CsPV"].Value.Trim() + "\" />");
                }
            }
            sb.Append("</Group>");
        }

        /// <summary>
        /// 生成特定城市价格区间排行
        /// </summary>
        /// <param name="doc"></param>
        private void GenerateCityPricerangeSort(XmlDocument doc, int cityID)
        {
            if (doc != null && doc.HasChildNodes)
            {
                XmlNodeList xnl = doc.SelectNodes("/SerialSort/City[@ID='" + cityID.ToString() + "']");

                if (xnl != null && xnl.Count > 0 && xnl[0].ChildNodes.Count > 0)
                {
                    GeneratePriceRangeDataFormXmlNode(xnl, "0-5", "5万以内");
                    GeneratePriceRangeDataFormXmlNode(xnl, "5-8", "5-8万");
                    GeneratePriceRangeDataFormXmlNode(xnl, "8-12", "8-12万");
                    GeneratePriceRangeDataFormXmlNode(xnl, "12-18", "12-18万");
                    GeneratePriceRangeDataFormXmlNode(xnl, "18-25", "18-25万");
                    GeneratePriceRangeDataFormXmlNode(xnl, "25-40", "25-40万");
                    GeneratePriceRangeDataFormXmlNode(xnl, "40-80", "40-80万");
                    GeneratePriceRangeDataFormXmlNode(xnl, "80-", "80万以上");
                }
                else
                {
                    // 如果此城市取不到数据 则去北京的数据
                    xnl = doc.SelectNodes("/SerialSort/City[@ID='201']");
                    if (xnl != null && xnl.Count > 0 && xnl[0].ChildNodes.Count > 0)
                    {
                        GeneratePriceRangeDataFormXmlNode(xnl, "0-5", "5万以内");
                        GeneratePriceRangeDataFormXmlNode(xnl, "5-8", "5-8万");
                        GeneratePriceRangeDataFormXmlNode(xnl, "8-12", "8-12万");
                        GeneratePriceRangeDataFormXmlNode(xnl, "12-18", "12-18万");
                        GeneratePriceRangeDataFormXmlNode(xnl, "18-25", "18-25万");
                        GeneratePriceRangeDataFormXmlNode(xnl, "25-40", "25-40万");
                        GeneratePriceRangeDataFormXmlNode(xnl, "40-80", "40-80万");
                        GeneratePriceRangeDataFormXmlNode(xnl, "80-", "80万以上");
                    }
                }
            }
        }

        private void GeneratePriceRangeDataFormXmlNode(XmlNodeList xnl, string priceName, string priceShowName)
        {
            sb.Append("<Group Name=\"" + priceShowName + "\">");
            XmlNodeList xnlPrice = xnl[0].SelectNodes("Seria_disPriceNew[@Name='" + priceName + "']");
            if (xnlPrice != null && xnlPrice.Count > 0 && xnlPrice[0].ChildNodes.Count > 0)
            {
                foreach (XmlNode xn in xnlPrice[0])
                {
                    sb.Append("<Serial ID=\"" + xn.Attributes["ID"].Value.Trim() + "\" ");
                    sb.Append(" CsName=\"" + xn.Attributes["CsName"].Value.Trim() + "\" ");
                    sb.Append(" CsShowName=\"" + xn.Attributes["CsShowName"].Value.Trim() + "\" ");
                    sb.Append(" CsAllSpell=\"" + xn.Attributes["CsAllSpell"].Value.Trim().ToLower() + "\" ");
                    sb.Append(" CsAll=\"" + xn.Attributes["CsAllSpell"].Value.Trim().ToLower() + "\" ");
                    sb.Append(" CsLevel=\"" + xn.Attributes["CsLevel"].Value.Trim() + "\" ");
                    sb.Append(" CsPV=\"" + xn.Attributes["CsPV"].Value.Trim() + "\" />");
                }
            }
            sb.Append("</Group>");
        }

        /// <summary>
        /// 取全国排行
        /// </summary>
        private void GetAllCityFor(int type)
        {

            #region 按价格 type == 1
            // 按价格
            if (type == 1)
            {
                List<EnumCollection.SerialSortForInterface> lssfi = base.GetAllSerialNewly30DayToList(); // base.GetAllSerialNewly7DayToList();
                List<EnumCollection.SerialSortForInterface>[] arrSSfi = new List<EnumCollection.SerialSortForInterface>[8];
                for (int i = 0; i < 8; i++)
                {
                    List<EnumCollection.SerialSortForInterface> ll1 = new List<EnumCollection.SerialSortForInterface>();
                    arrSSfi[i] = ll1;
                }
                foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
                {
                    if (ssfi.CsPriceRange.IndexOf(",1,") >= 0 && arrSSfi[0].Count < 10)
                    { arrSSfi[0].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",2,") >= 0 && arrSSfi[1].Count < 10)
                    { arrSSfi[1].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",3,") >= 0 && arrSSfi[2].Count < 10)
                    { arrSSfi[2].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",4,") >= 0 && arrSSfi[3].Count < 10)
                    { arrSSfi[3].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",5,") >= 0 && arrSSfi[4].Count < 10)
                    { arrSSfi[4].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",6,") >= 0 && arrSSfi[5].Count < 10)
                    { arrSSfi[5].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",7,") >= 0 && arrSSfi[6].Count < 10)
                    { arrSSfi[6].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",8,") >= 0 && arrSSfi[7].Count < 10)
                    { arrSSfi[7].Add(ssfi); }
                }
                for (int i = 0; i < arrSSfi.Length; i++)
                {
                    sb.Append("<Group Name=\"" + GetPriceRangeName(i) + "\" >");
                    if (arrSSfi[i].Count > 0)
                    {
                        foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
                        {
                            sb.Append("<Serial ID=\"" + ss.CsID.ToString() + "\" ");
                            sb.Append(" CsName=\"" + ss.CsName.ToString() + "\" ");
                            sb.Append(" CsShowName=\"" + ss.CsShowName.ToString() + "\" ");
                            sb.Append(" CsAllSpell=\"" + ss.CsAllSpell.ToString() + "\" ");
                            sb.Append(" CsAll=\"" + ss.CsAllSpell.ToString() + "\" ");
                            sb.Append(" CsLevel=\"" + ss.CsLevel.ToString() + "\" ");
                            sb.Append(" CsPV=\"" + ss.CsPV.ToString() + "\" />");
                        }
                    }
                    sb.Append("</Group>");
                }
                // temp = CSBillboardListService.GetCSBillboardListHTML_Price(false);
            }
            #endregion

            #region 按级别 type == 2
            // 按级别
            if (type == 2)
            {
                List<EnumCollection.SerialSortForInterface> lssfi = base.GetAllSerialNewly30DayToList();// GetAllSerialNewly7DayToList();
                // 增加面包车 add by chengl Aug.28.2014
                List<EnumCollection.SerialSortForInterface>[] arrSSfi = new List<EnumCollection.SerialSortForInterface>[10];
                for (int i = 0; i < 10; i++)
                {
                    List<EnumCollection.SerialSortForInterface> ll1 = new List<EnumCollection.SerialSortForInterface>();
                    arrSSfi[i] = ll1;
                }
                foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
                {
                    if (ssfi.CsLevel == "微型车" && arrSSfi[0].Count < 10)
                    { arrSSfi[0].Add(ssfi); }
                    if (ssfi.CsLevel == "小型车" && arrSSfi[1].Count < 10)
                    { arrSSfi[1].Add(ssfi); }
                    if (ssfi.CsLevel == "紧凑型车" && arrSSfi[2].Count < 10)
                    { arrSSfi[2].Add(ssfi); }
                    if (ssfi.CsLevel == "中型车" && arrSSfi[3].Count < 10)
                    { arrSSfi[3].Add(ssfi); }
                    if (ssfi.CsLevel == "中大型车" && arrSSfi[4].Count < 10)
                    { arrSSfi[4].Add(ssfi); }
                    if (ssfi.CsLevel == "豪华车" && arrSSfi[5].Count < 10)
                    { arrSSfi[5].Add(ssfi); }

                    if (ssfi.CsLevel == "MPV" && arrSSfi[6].Count < 10)
                    { arrSSfi[6].Add(ssfi); }
                    if (ssfi.CsLevel == "SUV" && arrSSfi[7].Count < 10)
                    { arrSSfi[7].Add(ssfi); }

                    if (ssfi.CsLevel == "跑车" && arrSSfi[8].Count < 10)
                    { arrSSfi[8].Add(ssfi); }
                    if (ssfi.CsLevel == "面包车" && arrSSfi[9].Count < 10)
                    { arrSSfi[9].Add(ssfi); }
                }
                for (int i = 0; i < arrSSfi.Length; i++)
                {
                    sb.Append("<Group Name=\"" + GetLevelName(i) + "\" >");
                    if (arrSSfi[i].Count > 0)
                    {
                        foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
                        {
                            sb.Append("<Serial ID=\"" + ss.CsID.ToString() + "\" ");
                            sb.Append(" CsName=\"" + ss.CsName.ToString() + "\" ");
                            sb.Append(" CsShowName=\"" + ss.CsShowName.ToString() + "\" ");
                            sb.Append(" CsAllSpell=\"" + ss.CsAllSpell.ToString() + "\" ");
                            sb.Append(" CsAll=\"" + ss.CsAllSpell.ToString() + "\" ");
                            sb.Append(" CsLevel=\"" + ss.CsLevel.ToString() + "\" ");
                            sb.Append(" CsPV=\"" + ss.CsPV.ToString() + "\" />");
                        }
                    }
                    sb.Append("</Group>");
                }
                // temp = CSBillboardListService.GetCSBillboardListHTML_Level(true);
            }
            #endregion

            #region 按全级别级别 type == 5
            // 按全级别级别
            if (type == 5)
            {
                List<EnumCollection.SerialSortForInterface> lssfi = base.GetAllSerialNewly30DayToList();// GetAllSerialNewly7DayToList();
                List<EnumCollection.SerialSortForInterface>[] arrSSfi = new List<EnumCollection.SerialSortForInterface>[12];
                for (int i = 0; i < 12; i++)
                {
                    List<EnumCollection.SerialSortForInterface> ll1 = new List<EnumCollection.SerialSortForInterface>();
                    arrSSfi[i] = ll1;
                }
                foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
                {
                    if (ssfi.CsLevel == "微型车" && arrSSfi[0].Count < 10)
                    { arrSSfi[0].Add(ssfi); }
                    if (ssfi.CsLevel == "小型车" && arrSSfi[1].Count < 10)
                    { arrSSfi[1].Add(ssfi); }
                    if (ssfi.CsLevel == "紧凑型车" && arrSSfi[2].Count < 10)
                    { arrSSfi[2].Add(ssfi); }
                    if (ssfi.CsLevel == "中型车" && arrSSfi[3].Count < 10)
                    { arrSSfi[3].Add(ssfi); }
                    if (ssfi.CsLevel == "中大型车" && arrSSfi[4].Count < 10)
                    { arrSSfi[4].Add(ssfi); }
                    if (ssfi.CsLevel == "豪华车" && arrSSfi[5].Count < 10)
                    { arrSSfi[5].Add(ssfi); }

                    if (ssfi.CsLevel == "MPV" && arrSSfi[6].Count < 10)
                    { arrSSfi[6].Add(ssfi); }
                    if (ssfi.CsLevel == "SUV" && arrSSfi[7].Count < 10)
                    { arrSSfi[7].Add(ssfi); }

                    if (ssfi.CsLevel == "跑车" && arrSSfi[8].Count < 10)
                    { arrSSfi[8].Add(ssfi); }
                    if (ssfi.CsLevel == "面包车" && arrSSfi[9].Count < 10)
                    { arrSSfi[9].Add(ssfi); }
                    if (ssfi.CsLevel == "皮卡" && arrSSfi[10].Count < 10)
                    { arrSSfi[10].Add(ssfi); }
                    if (ssfi.CsLevel == "其它" && arrSSfi[11].Count < 10)
                    { arrSSfi[11].Add(ssfi); }
                }
                for (int i = 0; i < arrSSfi.Length; i++)
                {
                    sb.Append("<Group Name=\"" + GetLevelName(i) + "\" >");
                    if (arrSSfi[i].Count > 0)
                    {
                        foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
                        {
                            sb.Append("<Serial ID=\"" + ss.CsID.ToString() + "\" ");
                            sb.Append(" CsName=\"" + ss.CsName.ToString() + "\" ");
                            sb.Append(" CsShowName=\"" + ss.CsShowName.ToString() + "\" ");
                            sb.Append(" CsAllSpell=\"" + ss.CsAllSpell.ToString() + "\" ");
                            sb.Append(" CsAll=\"" + ss.CsAllSpell.ToString() + "\" ");
                            sb.Append(" CsLevel=\"" + ss.CsLevel.ToString() + "\" ");
                            string priceRange = base.GetSerialPriceRangeByID(ss.CsID);
                            sb.Append(" CsPriceRange=\"" + priceRange + "\" ");
                            sb.Append(" CsPV=\"" + ss.CsPV.ToString() + "\" />");
                        }
                    }
                    sb.Append("</Group>");
                }
            }
            #endregion

            #region 按价格 type == 6 增加了报价
            if (type == 6)
            {
                List<EnumCollection.SerialSortForInterface> lssfi = base.GetAllSerialNewly30DayToList(); // base.GetAllSerialNewly7DayToList();
                List<EnumCollection.SerialSortForInterface>[] arrSSfi = new List<EnumCollection.SerialSortForInterface>[8];
                for (int i = 0; i < 8; i++)
                {
                    List<EnumCollection.SerialSortForInterface> ll1 = new List<EnumCollection.SerialSortForInterface>();
                    arrSSfi[i] = ll1;
                }
                foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
                {
                    if (ssfi.CsPriceRange.IndexOf(",1,") >= 0 && arrSSfi[0].Count < 10)
                    { arrSSfi[0].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",2,") >= 0 && arrSSfi[1].Count < 10)
                    { arrSSfi[1].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",3,") >= 0 && arrSSfi[2].Count < 10)
                    { arrSSfi[2].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",4,") >= 0 && arrSSfi[3].Count < 10)
                    { arrSSfi[3].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",5,") >= 0 && arrSSfi[4].Count < 10)
                    { arrSSfi[4].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",6,") >= 0 && arrSSfi[5].Count < 10)
                    { arrSSfi[5].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",7,") >= 0 && arrSSfi[6].Count < 10)
                    { arrSSfi[6].Add(ssfi); }
                    if (ssfi.CsPriceRange.IndexOf(",8,") >= 0 && arrSSfi[7].Count < 10)
                    { arrSSfi[7].Add(ssfi); }
                }
                for (int i = 0; i < arrSSfi.Length; i++)
                {
                    sb.Append("<Group Name=\"" + GetPriceRangeName(i) + "\" >");
                    if (arrSSfi[i].Count > 0)
                    {
                        foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
                        {
                            sb.Append("<Serial ID=\"" + ss.CsID.ToString() + "\" ");
                            sb.Append(" CsName=\"" + ss.CsName.ToString() + "\" ");
                            sb.Append(" CsShowName=\"" + ss.CsShowName.ToString() + "\" ");
                            sb.Append(" CsAllSpell=\"" + ss.CsAllSpell.ToString() + "\" ");
                            sb.Append(" CsAll=\"" + ss.CsAllSpell.ToString() + "\" ");
                            sb.Append(" CsLevel=\"" + ss.CsLevel.ToString() + "\" ");
                            string priceRange = base.GetSerialPriceRangeByID(ss.CsID);
                            sb.Append(" CsPriceRange=\"" + priceRange + "\" ");
                            sb.Append(" CsPV=\"" + ss.CsPV.ToString() + "\" />");
                        }
                    }
                    sb.Append("</Group>");
                }
            }
            #endregion
        }

        private string GetPriceRangeName(int index)
        {
            string name = "";
            switch (index)
            {
                case 0: name = "5万以内"; break;
                case 1: name = "5-8万"; break;
                case 2: name = "8-12万"; break;
                case 3: name = "12-18万"; break;
                case 4: name = "18-25万"; break;
                case 5: name = "25-40万"; break;
                case 6: name = "40-80万"; break;
                case 7: name = "80万以上"; break;
                default: name = ""; break;
            }
            return name;
        }

        #endregion

        /// <summary>
        /// 根据子品牌ID取子品牌同级别子品牌按UV热门倒序
        /// </summary>
        private void RenderSameLevelByCsID()
        {
            int csid = 0;
            int top = 10;
            if (request.QueryString["top"] != null && request.QueryString["top"].ToString() != "")
            {
                string topCount = request.QueryString["top"].ToString();
                if (int.TryParse(topCount, out top))
                { }
            }
            if (request.QueryString["csID"] != null && request.QueryString["csID"].ToString() != "")
            {
                string csIDstr = request.QueryString["csID"].ToString();
                if (int.TryParse(csIDstr, out csid))
                { }
            }
            if (top < 0 || top > 10)
            { top = 10; }

            if (csid > 0 && top > 0)
            {
                DataSet dsSerialRank = base.GetAllSerialNewly30Day();
                if (dsSerialRank != null && dsSerialRank.Tables.Count > 0 && dsSerialRank.Tables[0].Rows.Count > 0)
                {
                    // 级别、DataRow
                    Dictionary<string, List<DataRow>> dicLevelRow = new Dictionary<string, List<DataRow>>();
                    Dictionary<int, string> dicCsIDLevel = new Dictionary<int, string>();
                    foreach (DataRow dr in dsSerialRank.Tables[0].Rows)
                    {
                        int csidDataRow = int.Parse(dr["cs_ID"].ToString());
                        string level = dr["cs_CarLevel"].ToString().Trim();
                        if (!string.IsNullOrEmpty(level) && csidDataRow > 0)
                        {
                            if (!dicLevelRow.ContainsKey(level))
                            {
                                List<DataRow> list = new List<DataRow>();
                                list.Add(dr);
                                dicLevelRow.Add(level, list);
                            }
                            else
                            {
                                dicLevelRow[level].Add(dr);
                            }
                            if (!dicCsIDLevel.ContainsKey(csidDataRow))
                            { dicCsIDLevel.Add(csidDataRow, level); }
                        }
                    }

                    if (dicCsIDLevel.ContainsKey(csid) && dicLevelRow.ContainsKey(dicCsIDLevel[csid]))
                    {
                        // 白底
                        Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();
                        // 子品牌报价区间
                        Dictionary<int, string> dicCsPrice = base.GetAllCsPriceRange();
                        sb.Append("<!-- ID:同级别子品牌ID Name:子品牌名 ShowName:子品牌显示名 Pic:子品牌图片 PriceRange:子品牌报价区间 AllSpell:子品牌全拼Url使用 -->");
                        int loop = 0;
                        foreach (DataRow dr in dicLevelRow[dicCsIDLevel[csid]])
                        {
                            int sameLevelCsid = int.Parse(dr["cs_ID"].ToString());
                            if (sameLevelCsid == csid)
                            { continue; }
                            if (loop >= top)
                            { break; }
                            loop++;
                            string img = dicPicWhite.ContainsKey(sameLevelCsid) ? dicPicWhite[sameLevelCsid] : WebConfig.DefaultCarPic;
                            sb.AppendLine(string.Format("<Cs ID=\"{0}\" Name=\"{1}\" ShowName=\"{2}\" Pic=\"{3}\" PriceRange=\"{4}\" AllSpell=\"{5}\" />"
                                , sameLevelCsid
                                , System.Security.SecurityElement.Escape(dr["cs_name"].ToString().Trim())
                                , System.Security.SecurityElement.Escape(dr["cs_showname"].ToString().Trim())
                                , img
                                , (dicCsPrice.ContainsKey(sameLevelCsid) ? dicCsPrice[sameLevelCsid] : "")
                                , System.Security.SecurityElement.Escape(dr["allspell"].ToString().Trim())));
                        }
                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 获取车款数据 根据子品牌id
        /// </summary>
        private void RenderCarInfoJson()
        {
            response.ContentType = "application/x-javascript";
            string callback = request.QueryString["callback"];
            var serialId = ConvertHelper.GetInteger(request.QueryString["serialid"]);
            var isAll = request.QueryString["isall"];

            var _carBLL = new Car_BasicBll();
            List<CarInfoForSerialSummaryEntity> lastCarList = new List<CarInfoForSerialSummaryEntity>();
            if (serialId > 0)
            {
                lastCarList = _carBLL.GetCarInfoForSerialSummaryBySerialId(serialId);
                if (lastCarList.Count > 0)
                {
                    if (string.IsNullOrEmpty(isAll))
                        lastCarList = lastCarList.FindAll(p => p.SaleState == "在销");
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            var s = js.Serialize(lastCarList);

            response.Write(string.Format(!string.IsNullOrEmpty(callback) ? (callback + "({0})") : "{0}", s));
        }

        /// <summary>
        /// 易湃 平台业务部 杨杰 取子品牌级别信息
        /// </summary>
        private void RenderAllCsInfoForEP()
        {
            sb.AppendLine("<!-- Cs:子品牌节点 ID:子品牌ID Name:子品牌名 AllSpell:子品牌全拼(url使用) RP:指导价区间(非停销车型) P:报价区间 Img:白底图 -->");
            // 子品牌非停销指导价区间
            Dictionary<int, string> dicCsOfficePrice = new Car_SerialBll().GetAllSerialOfficePriceBySaleState(false);
            // 子品牌报价区间
            Dictionary<int, string> dicCsPrice = base.GetAllCsPriceRange();
            // 白底
            Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();

            DataSet dsCs = new Car_SerialBll().GetAllValidSerial();
            if (dsCs != null && dsCs.Tables.Count > 0 && dsCs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsCs.Tables[0].Rows)
                {
                    int csid = int.Parse(dr["cs_id"].ToString());
                    string csName = System.Security.SecurityElement.Escape(dr["cs_name"].ToString().Trim());
                    string csShowName = System.Security.SecurityElement.Escape(dr["cs_showname"].ToString().Trim());
                    string csallSpell = System.Security.SecurityElement.Escape(dr["allspell"].ToString().Trim());
                    string rp = dicCsOfficePrice.ContainsKey(csid) ? dicCsOfficePrice[csid] : "";
                    string p = dicCsPrice.ContainsKey(csid) ? dicCsPrice[csid] : "";
                    string img = dicPicWhite.ContainsKey(csid) ? dicPicWhite[csid] : WebConfig.DefaultCarPic;
                    sb.AppendLine(string.Format("<Cs ID=\"{0}\" Name=\"{1}\" ShowName=\"{2}\" AllSpell=\"{3}\" RP=\"{4}\" P=\"{5}\" Img=\"{6}\" />"
                        , csid, csName, csShowName, csallSpell, rp, p, img));
                }
                CommonFunction.EchoXml(response, sb.ToString(), "Root");
            }
        }

        /// <summary>
        /// 取子品牌级别排行
        /// </summary>
        private void RenderCsLevelRank()
        {
            DataSet dsCsLevelRank = new Car_SerialBll().GetAllSerialUVRAngeByLevel();
            if (dsCsLevelRank != null && dsCsLevelRank.Tables.Count > 0 && dsCsLevelRank.Tables[0].Rows.Count > 0)
            {
                sb.AppendLine("<!-- ID:子品牌ID、Level:级别、Rank:级别排名 -->");
                foreach (DataRow dr in dsCsLevelRank.Tables[0].Rows)
                {
                    sb.AppendLine(string.Format("<Cs ID=\"{0}\" Level=\"{1}\" Rank=\"{2}\" />"
                        , dr["csID"].ToString()
                        , System.Security.SecurityElement.Escape(dr["cs_CarLevel"].ToString().Trim())
                        , dr["rank"].ToString().Trim()));
                }
                CommonFunction.EchoXml(response, sb.ToString(), "Root");
            }
        }

        /// <summary>
        /// 按车系取评测标签及对应的url
        /// </summary>
        private void RenderCsPingceTagByCsID()
        {
            sb.AppendLine("<!-- 标签对应{导语:1,外观:2,内饰:3,空间:4,视野:5,灯光:6,动力:7,操控:8,舒适性:9,油耗:10,配置:11,总结:12} -->");
            int csid = 0;
            if (request.QueryString["csID"] != null && request.QueryString["csID"].ToString() != "")
            {
                string csIDstr = request.QueryString["csID"].ToString();
                if (int.TryParse(csIDstr, out csid))
                { }
            }
            if (csid > 0)
            {
                List<EnumCollection.PingCeTag> listTag = CommonFunction.IntiPingCeTagListInfo();
                string sqlGetPingCe = "SELECT [csid],[url],[tagid] FROM [CarPingceInfo] WHERE csid=@csid ORDER BY tagid";
                SqlParameter[] _param ={
                                      new SqlParameter("@csID",SqlDbType.Int)
                                  };
                _param[0].Value = csid;
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sqlGetPingCe, _param);
                Dictionary<int, string> dicDBUrl = new Dictionary<int, string>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int tagid = BitAuto.Utils.ConvertHelper.GetInteger(dr["tagid"].ToString());
                        string url = dr["url"].ToString().Trim();
                        if (!dicDBUrl.ContainsKey(tagid))
                        { dicDBUrl.Add(tagid, url); }
                    }
                }
                foreach (EnumCollection.PingCeTag tagInfo in listTag)
                {
                    sb.AppendLine(string.Format("<PingCe TarID=\"{0}\" TagName=\"{1}\" Url=\"{2}\" />"
                                           , tagInfo.tagId
                                           , System.Security.SecurityElement.Escape(tagInfo.tagName)
                                           , System.Security.SecurityElement.Escape(
                                           dicDBUrl.ContainsKey(tagInfo.tagId) ? dicDBUrl[tagInfo.tagId] : ""
                                           )));
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }


        /// <summary>
        /// 取子品牌评测首片新闻地址
        /// </summary>
        private void RenderCsPingceNewsUrl()
        {
            string sqlGetCsPingceNews = @"select [csid],[url]
								from (
								SELECT [csid],[url],[tagid]
								,row_number() over(partition by csid 
								order by tagid)as tagrank
								FROM [CarPingceInfo]
								)t1 
								where tagrank=1 ";
            DataSet dsCsPingce = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
                WebConfig.CarDataUpdateConnectionString, CommandType.Text, sqlGetCsPingceNews);
            if (dsCsPingce != null && dsCsPingce.Tables.Count > 0 && dsCsPingce.Tables[0].Rows.Count > 0)
            {
                sb.AppendLine("<!-- ID:子品牌ID、Url:评测文章地址 -->");
                foreach (DataRow dr in dsCsPingce.Tables[0].Rows)
                {
                    sb.AppendLine(string.Format("<Cs ID=\"{0}\" Url=\"{1}\" />"
                        , dr["csid"].ToString()
                        , System.Security.SecurityElement.Escape(dr["url"].ToString().Trim())));
                }
                CommonFunction.EchoXml(response, sb.ToString(), "Root");
            }
        }
        /// <summary>
        /// 获取 评测tag
        /// </summary>
        private void RenderCsPingceTag()
        {
            string serialAllSpell = string.Empty;
            int serialId = ConvertHelper.GetInteger(request.QueryString["serialId"]);
            SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
            if (serialEntity != null)
            {
                serialAllSpell = serialEntity.AllSpell;
                var pingceTag = base.GetPingceNewsByCsId(serialId);
                sb.AppendLine(string.Format("<Pingce BaseUrl=\"http://car.bitauto.com/{0}/pingce/\">", serialAllSpell));
                if (pingceTag.Any())
                {
                    sb.AppendLine("<!-- Name:评测分类名称、Url:评测地址、NewsUrl:新闻地址 -->");
                    foreach (var kv in pingceTag)
                    {
                        sb.AppendLine(string.Format("<PingceTag Name=\"{0}\" Url=\"http://car.bitauto.com/{2}/pingce/{1}/\" NewsUrl=\"{3}\" />",
                            kv.Value.tagName
                            , kv.Key
                            , serialAllSpell
                            , kv.Value.url));
                    }
                }
                sb.AppendLine("</Pingce>");
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        private void RenderCsInfoForYiCheHui()
        {
            string sqlGetCsInfo = @"select cs.cs_id,cs.csname,cl.classvalue as csLevel
					from dbo.Car_Serial cs
					left join class cl on cs.carlevel=cl.classid
					where cs.isState=0 and cs.CsSaleState='在销'
					order by cs.cs_id";
            DataSet dsCs = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
                WebConfig.AutoStorageConnectionString, CommandType.Text, sqlGetCsInfo);
            if (dsCs != null && dsCs.Tables.Count > 0 && dsCs.Tables[0].Rows.Count > 0)
            {
                sb.AppendLine("<!-- ID:子品牌ID、Name:子品牌名、Level:子品牌级别 -->");
                foreach (DataRow dr in dsCs.Tables[0].Rows)
                {
                    sb.AppendLine(string.Format("<Cs ID=\"{0}\" Name=\"{1}\" Level=\"{2}\" />"
                        , dr["cs_id"].ToString()
                        , System.Security.SecurityElement.Escape(dr["csname"].ToString().Trim())
                        , System.Security.SecurityElement.Escape(dr["csLevel"].ToString().Trim())));
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        #region 机器人取子品牌数据

        /// <summary>
        /// 机器人取子品牌数据
        /// </summary>
        private void RenderRobotCsInfo()
        {
            string sql = @"SELECT  ISNULL(cp.cp_id, 0) AS cp_id, cp.Cp_ShortName, cmb.bs_id, cmb.bs_name,
                                    cmb.urlSpell AS bsAllSpell, cb.cb_id, cb.cb_name,
                                    cb.allspell AS cbAllSpell, cs.cs_id, cs.cs_name,
                                    cs.allspell AS csAllSpell, cs.cs_ShowName, cs.cs_CarLevel,
                                    cs.cs_seoname, cs30.uvcount, cs.CsSaleState
                            FROM    car_serial cs
                                    LEFT JOIN dbo.Car_Serial_30UV cs30 ON cs.cs_id = cs30.cs_id
                                    LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                    LEFT JOIN Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                                    LEFT JOIN Car_Producer cp ON cb.cp_id = cp.cp_id
                            WHERE   cs.isState = 1
                                    AND cb.isState = 1
                                    AND cp.isState = 1
                            ORDER BY cp.cp_id, cmb.bs_id, cb.cb_id, cs.cs_id";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                , CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.AppendLine("<Serial ID=\"" + dr["cs_id"].ToString().Trim() + "\" ");
                    sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(
                        dr["cs_name"].ToString().Trim()) + "\" ");
                    sb.Append(" CsShowName=\"" + System.Security.SecurityElement.Escape(
                        dr["cs_ShowName"].ToString().Trim()) + "\" ");
                    sb.Append(" CsAllSpell=\"" + dr["csAllSpell"].ToString().Trim() + "\" ");
                    sb.Append(" CbID=\"" + dr["cb_id"].ToString().Trim() + "\" ");
                    sb.Append(" CbName=\"" + System.Security.SecurityElement.Escape(
                        dr["cb_name"].ToString().Trim()) + "\" ");
                    sb.Append(" CbAllSpell=\"" + dr["cbAllSpell"].ToString().Trim() + "\" ");
                    sb.Append(" BsID=\"" + dr["bs_id"].ToString().Trim() + "\" ");
                    sb.Append(" BsName=\"" + System.Security.SecurityElement.Escape(
                        dr["bs_name"].ToString().Trim()) + "\" ");
                    sb.Append(" BsAllSpell=\"" + dr["bsAllSpell"].ToString().Trim() + "\" ");
                    sb.Append(" CpID=\"" + dr["cp_id"].ToString().Trim() + "\" ");
                    sb.Append(" CpShortName=\"" + System.Security.SecurityElement.Escape(
                        dr["Cp_ShortName"].ToString().Trim()) + "\" ");
                    sb.Append(" CsPrice=\"" + base.GetSerialPriceRangeByID(int.Parse(dr["cs_id"].ToString().Trim())) + "\" ");
                    sb.Append(" CsBBS=\"" + new Car_SerialBll().GetForumUrlBySerialId(int.Parse(dr["cs_id"].ToString().Trim())) + "\" ");
                    sb.Append(" CsLevel=\"" + dr["cs_CarLevel"].ToString().Trim() + "\" ");
                    sb.Append(" CsLevelURL=\"" + GetCsLevelURLByName(dr["cs_CarLevel"].ToString().Trim()) + "\" ");
                    sb.Append(" CsSEOName=\"" + System.Security.SecurityElement.Escape(
                            dr["cs_seoname"].ToString().Trim()) + "\" ");
                    sb.Append(" CsUV=\"" + dr["uvcount"].ToString().Trim() + "\" ");
                    sb.Append(" CsSaleState=\"" + System.Security.SecurityElement.Escape(
                            dr["CsSaleState"].ToString().Trim()) + "\" ");
                    sb.Append("/>");
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "SerialInfo");
        }

        // 根据级别名去级别页地址
        private string GetCsLevelURLByName(string levelName)
        {
            string url = "";
            switch (levelName)
            {
                case "微型车": url = "weixingche"; break;
                case "小型车": url = "xiaoxingche"; break;
                case "紧凑型车": url = "jincouxingche"; break;
                case "中大型车": url = "zhongdaxingche"; break;
                case "中型车": url = "zhongxingche"; break;
                case "豪华车": url = "haohuaxingche"; break;
                case "MPV": url = "mpv"; break;
                case "SUV": url = "suv"; break;
                case "跑车": url = "paoche"; break;
                case "其它": url = "qita"; break;
                case "面包车": url = "mianbaoche"; break;
                case "皮卡": url = "pika"; break;
                default: url = ""; break;
            }
            return url;
        }

        #endregion

        private void RenderSerialWireless()
        {
            EnumCollection.SerialInfoCard sic = new EnumCollection.SerialInfoCard();	//子品牌名片
            Car_SerialEntity cse = new Car_SerialEntity();				//子品牌信息 
            List<string> listcsEE = new List<string>();
            DataSet dsCar = new DataSet();
            int csID = 0;
            bool isAllSaleCar = false;
            if (request.QueryString["csID"] != null && request.QueryString["csID"].ToString() != "")
            {
                string strCSID = request.QueryString["csID"].ToString().Trim();
                if (int.TryParse(strCSID, out csID))
                { }
            }
            if (request.QueryString["isAllSaleCar"] != null && request.QueryString["isAllSaleCar"].ToString() == "1")
            {
                isAllSaleCar = true;
            }
            // 取子品牌的车型，在销或者包括停销
            dsCar = GetCarIDAndNameForCS(csID, isAllSaleCar);
            if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsCar.Tables[0].Rows)
                {
                    string carEE = dr["Engine_Exhaust"].ToString();
                    if (!listcsEE.Contains(carEE))
                    { listcsEE.Add(carEE); }
                }
            }

            #region 子品牌基本信息

            if (csID > 0)
            {
                sic = csb.GetSerialInfoCard(csID);
                cse = csb.GetSerialInfoEntity(csID);
                if (sic.CsID != 0)
                {
                    sb.AppendLine("<Serial ID=\"" + sic.CsID.ToString() + "\" ");
                    sb.Append(" Name=\"" + System.Security.SecurityElement.Escape(sic.CsName.Trim()) + "\" ");
                    sb.Append(" ShowName=\"" + System.Security.SecurityElement.Escape(sic.CsShowName.Trim()) + "\" ");
                    sb.Append(" CsPic=\"" + (sic.CsDefaultPic == "http://image.bitautoimg.com/autoalbum/V2.1/images/150-100.gif" ? "" : sic.CsDefaultPic) + "\" ");
                    sb.Append(" CsLevel=\"" + System.Security.SecurityElement.Escape(cse.Cs_CarLevel.Trim()) + "\" ");
                    sb.Append(" CbName=\"" + System.Security.SecurityElement.Escape(cse.Cb_Name.Trim()) + "\" ");
                    sb.Append(" AllSpell=\"" + System.Security.SecurityElement.Escape(sic.CsAllSpell.Trim().ToLower()) + "\" ");
                    sb.Append(" CsPriceRange=\"" + sic.CsPriceRange.Trim() + "\" ");
                    sb.Append(" CsTransmissionType=\"" + sic.CsTransmissionType.Trim() + "\" ");
                    // add by chengl 加保修政策
                    sb.Append(" RepairPolicy=\"" + System.Security.SecurityElement.Escape(sic.SerialRepairPolicy.Trim()) + "\" ");
                    // sb.Append(" EngineExhaust=\"" + sic.CsEngineExhaust.Trim() + "\" ");
                    string csEE = "";
                    if (listcsEE.Count > 0)
                    {
                        foreach (string carEE in listcsEE)
                        {
                            if (csEE != "")
                            { csEE += "、"; }
                            csEE += carEE;
                        }
                    }
                    sb.Append(" EngineExhaust=\"" + csEE + "\" ");
                    sb.Append(" CsSummaryFuelCost=\"" + sic.CsSummaryFuelCost.Trim() + "\" ");
                    sb.Append(" CsOfficialFuelCost=\"" + sic.CsOfficialFuelCost.Trim() + "\" ");
                    sb.Append(" CsGuestFuelCost=\"" + sic.CsGuestFuelCost.Trim() + "\" ");
                    sb.Append(" CsKouBeiCount=\"" + sic.CsDianPingCount.ToString() + "\" ");
                    sb.Append(" CsPicCount=\"" + sic.CsPicCount.ToString() + "\" ");

                    sb.Append(" CsReferPrice=\"" + GetSerialReferPriceByCsID(sic.CsID) + "\" ");
                    //  取子品牌 加速时间（0—100km/h）、制动距离（100—0km/h）、油耗 数据 (786\787\788)
                    DataSet dsCsDataInfo = GetCsDataInfo(sic.CsID);
                    if (dsCsDataInfo != null && dsCsDataInfo.Tables.Count > 0 && dsCsDataInfo.Tables[0].Rows.Count > 0)
                    {
                        sb.Append(" CsMA=\"" + System.Security.SecurityElement.Escape(dsCsDataInfo.Tables[0].Rows[0]["p1"].ToString()) + "\" ");
                        sb.Append(" CsBD=\"" + System.Security.SecurityElement.Escape(dsCsDataInfo.Tables[0].Rows[0]["p2"].ToString()) + "\" ");
                        sb.Append(" CsMF=\"" + System.Security.SecurityElement.Escape(dsCsDataInfo.Tables[0].Rows[0]["p3"].ToString()) + "\" ");
                    }
                    else
                    {
                        sb.Append(" CsMA=\"\" ");
                        sb.Append(" CsBD=\"\" ");
                        sb.Append(" CsMF=\"\" ");
                    }
                    sb.Append(" CsVirtues=\"" + cse.Cs_Virtues.Trim() + "\" ");
                    sb.Append(" CsDefect=\"" + cse.Cs_Defect.Trim() + "\" />");
                }
            }

            #endregion

            #region 车型基本数据
            if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
            {
                sb.AppendLine("<CarList>");
                string carYear = "";
                bool hasYear = false;
                for (int i = 0; i < dsCar.Tables[0].Rows.Count; i++)
                {
                    if (dsCar.Tables[0].Rows[i]["Car_YearType"].ToString() == "")
                    {
                        continue;
                    }
                    hasYear = true;
                    if (carYear == "")
                    {
                        // 第一个年款
                        carYear = dsCar.Tables[0].Rows[i]["Car_YearType"].ToString();
                        sb.AppendLine("<CarYear Year=\"" + carYear + "\" >");
                    }
                    else
                    {
                        if (dsCar.Tables[0].Rows[i]["Car_YearType"].ToString() != "")
                        {
                            // 不同年款
                            if (carYear != dsCar.Tables[0].Rows[i]["Car_YearType"].ToString())
                            {
                                carYear = dsCar.Tables[0].Rows[i]["Car_YearType"].ToString();
                                sb.AppendLine("</CarYear>");
                                sb.AppendLine("<CarYear Year=\"" + carYear + "\" >");
                            }
                        }
                    }
                    sb.AppendLine("<Car CarID=\"" + dsCar.Tables[0].Rows[i]["Car_id"].ToString() + "\" ");
                    sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(dsCar.Tables[0].Rows[i]["Car_name"].ToString().Trim()) + "\" ");
                    sb.Append(" ReferPrice=\"" + dsCar.Tables[0].Rows[i]["car_ReferPrice"].ToString().Trim() + "\" ");
                    // if(dsCar.Tables[0].Rows[i][""])
                    // 车型的环保补贴
                    bool isHasEnergySubsidy = new Car_BasicBll().CarHasParamEx(int.Parse(dsCar.Tables[0].Rows[i]["Car_id"].ToString()), 853);
                    if (isHasEnergySubsidy)
                    {
                        sb.Append(" EnergySubsidy=\"可获得3000元节能补贴\" ");
                    }
                    else
                    {
                        sb.Append(" EnergySubsidy=\"无\" ");
                    }
                    sb.Append("/>");
                }
                if (hasYear)
                {
                    sb.AppendLine("</CarYear>");
                }
                sb.AppendLine("</CarList>");
            }
            #endregion

            #region 子品牌颜色
            sb.AppendLine("<SerialColor>");
            DataSet dsColor = new Car_SerialBll().GetAllSerialColorRGB();
            if (dsColor != null && dsColor.Tables.Count > 0 && dsColor.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsColor.Tables[0].Select(" cs_id='" + csID.ToString() + "' ");
                if (drs != null && drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        if (sic.ColorList != null && sic.ColorList.Contains(dr["colorName"].ToString().Trim()))
                        {
                            sb.AppendLine("<Color Name=\"" + System.Security.SecurityElement.Escape(dr["colorName"].ToString().Trim()) + "\" RGB=\""
                                + dr["colorRGB"].ToString().ToUpper().Trim() + "\" />");
                        }
                    }
                }
            }
            sb.AppendLine("</SerialColor>");
            #endregion

            CommonFunction.EchoXml(response, sb.ToString(), "SerialBasicInfo");
        }

        /// <summary>
        /// 取子品牌 加速时间（0—100km/h）、制动距离（100—0km/h）、油耗 数据 (786\787\788)
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        private DataSet GetCsDataInfo(int csid)
        {
            string sql = @"select car.car_id,cdb1.pvalue as p1,cdb2.pvalue as p2,cdb3.pvalue as p3
								from car_relation car
								left join cardatabase cdb1 on car.car_id=cdb1.carid and cdb1.paramid=786
								left join cardatabase cdb2 on car.car_id=cdb2.carid and cdb2.paramid=787
								left join cardatabase cdb3 on car.car_id=cdb3.carid and cdb3.paramid=788
								where car.isstate=0 and car.cs_id = {0} and cdb1.pvalue<>''
								and cdb2.pvalue<>'' and cdb3.pvalue<>''";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString,
                CommandType.Text, string.Format(sql, csid));
            return ds;
        }

        /// <summary>
        /// 子品牌的在销车型
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        private string GetSerialReferPriceByCsID(int csid)
        {
            string serialReferPrice = "";
            List<EnumCollection.CarInfoForSerialSummary> ls = base.GetAllCarInfoForSerialSummaryByCsID(csid);
            double maxPrice = Double.MinValue;
            double minPrice = Double.MaxValue;
            foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
            {
                double referPrice = 0.0;
                bool isDouble = Double.TryParse(carInfo.ReferPrice.Replace("万", ""), out referPrice);
                if (isDouble)
                {
                    if (referPrice > maxPrice)
                        maxPrice = referPrice;
                    if (referPrice < minPrice)
                        minPrice = referPrice;
                }
            }

            if (maxPrice == Double.MinValue && minPrice == Double.MaxValue)
                serialReferPrice = "暂无";
            else
            {
                serialReferPrice = minPrice + "万-" + maxPrice + "万";
            }
            return serialReferPrice;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="isAllSaleCar">是否包含停销车型</param>
        /// <returns></returns>
        private DataSet GetCarIDAndNameForCS(int csID, bool isAllSaleCar)
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
            sql += " where car.isState=1 and cs.isState=1 and cs.cs_id = @csID {0} ";
            sql += " order by car.Car_YearType desc,cei.Engine_Exhaust,TransmissionType,car.car_ReferPrice ";
            SqlParameter[] _param ={
                                      new SqlParameter("@csID",SqlDbType.Int)
                                  };
            _param[0].Value = csID;
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, string.Format(sql, (isAllSaleCar ? "" : " and car.Car_SaleState<>'停销' ")), _param);
            return ds;
        }


        /// <summary>
        /// 子品牌自主合资进口德系日系韩系美系欧系其他
        /// </summary>
        private void RenderSerialCountry()
        {
            string sql = @"SELECT  cs.cs_id, cs.cs_name, cs.cs_showname, cmb.bs_Country,
                                    cb.cb_Country AS Cp_Country
                            FROM    car_serial cs
                                    LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN dbo.Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                    LEFT JOIN dbo.Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                            WHERE   cs.isState = 1
                            ORDER BY cs_id";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString,
                CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int csid = int.Parse(dr["cs_id"].ToString());
                    string csShowName = dr["cs_showname"].ToString().Trim();
                    string bsCountry = dr["bs_Country"].ToString().Trim();
                    string cpCountry = dr["Cp_Country"].ToString().Trim();
                    sb.AppendLine(string.Format("<Cs ID=\"{0}\" Name=\"{1}\"  Import=\"{2}\" Country=\"{3}\" />"
                        , csid, System.Security.SecurityElement.Escape(csShowName)
                        , CommonFunction.GetImport(cpCountry, bsCountry), GetCountry(bsCountry)));
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 德系 日系 韩系 美系 欧系 其他
        /// </summary>
        /// <param name="bsCountry"></param>
        /// <returns></returns>
        private string GetCountry(string bsCountry)
        {
            List<string> countryList = new List<string>();
            switch (bsCountry)
            {
                case "中国":; break;
                case "美国": countryList.Add("美系"); break;
                case "日本": countryList.Add("日系"); break;
                case "韩国": countryList.Add("韩系"); break;
                case "德国": countryList.Add("德系"); countryList.Add("欧系"); break;
                case "英国": countryList.Add("欧系"); break;
                case "法国": countryList.Add("欧系"); break;
                case "意大利": countryList.Add("欧系"); break;
                case "瑞典": countryList.Add("欧系"); break;
                case "捷克": countryList.Add("欧系"); break;
                case "马来西亚": countryList.Add("其他"); break;
                case "印度": countryList.Add("其他"); break;
                case "荷兰": countryList.Add("欧系"); break;
                case "西班牙": countryList.Add("欧系"); break;
                case "澳大利亚": countryList.Add("其他"); break;
                default: countryList.Add("其他"); break;
            }
            string country = "";
            if (countryList.Count > 0)
            { country = string.Join(",", countryList.ToArray()); }
            return country;
        }


        /// <summary>
        /// 取全国或按城市top子品牌
        /// </summary>
        private void RenderSerialRank(bool isJson)
        {
            List<int> csIDsForJson = new List<int>();
            int cityID = 0;
            int top = 10;
            if (request.QueryString["top"] != null && request.QueryString["top"].ToString() != "")
            {
                string topCount = request.QueryString["top"].ToString();
                if (int.TryParse(topCount, out top))
                { }
            }
            if (request.QueryString["cityID"] != null && request.QueryString["cityID"].ToString() != "")
            {
                string city = request.QueryString["cityID"].ToString();
                if (int.TryParse(city, out cityID))
                { }
            }
            if (top < 0 || top > 100)
            { top = 10; }
            DataSet ds = csb.GetSerialUVOrderByCityId(cityID);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int loop = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int csid = int.Parse(dr["csID"].ToString());
                    if (isJson)
                    {
                        if (!csIDsForJson.Contains(csid))
                        { csIDsForJson.Add(csid); }
                    }
                    else
                    {
                        sb.AppendLine(string.Format("<ID>{0}</ID>", csid));
                    }
                    if (loop >= top)
                    { break; }
                    loop++;
                }
            }
            if (isJson)
            {
                // 如果输出json
                response.ContentType = "application/x-javascript";
                string callback = request.QueryString["callback"];
                response.Write(string.Format("{0}([{1}])", callback, string.Join(",", csIDsForJson.ToArray())));
            }
            else
            {
                // 如果输出xml
                CommonFunction.EchoXml(response, sb.ToString(), "Root");
            }
        }

        /// <summary>
        /// 取所以子品牌的还关注10个子品牌
        /// </summary>
        private void RenderSerialToSerial()
        {
            Dictionary<int, List<int>> dic = base.GetAllSerialToSerialDic();
            foreach (KeyValuePair<int, List<int>> kvp in dic)
            {
                sb.AppendLine(string.Format("<Cs ID=\"{0}\" OtherID=\"{1}\"/>", kvp.Key, string.Join(",", kvp.Value.ToArray())));
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 根据子品牌ID取所以子品牌的还关注子品牌
        /// </summary>
        private void RenderSerialToSerialDetailByCsID()
        {
            int csid = 0;
            int top = 10;
            if (request.QueryString["top"] != null && request.QueryString["top"].ToString() != "")
            {
                string topCount = request.QueryString["top"].ToString();
                if (int.TryParse(topCount, out top))
                { }
            }
            if (request.QueryString["csID"] != null && request.QueryString["csID"].ToString() != "")
            {
                string csIDstr = request.QueryString["csID"].ToString();
                if (int.TryParse(csIDstr, out csid))
                { }
            }
            if (top < 0 || top > 10)
            { top = 10; }

            if (csid > 0 && top > 0)
            {
                Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();
                List<EnumCollection.SerialToSerial> listSTS = base.GetSerialToSerialByCsID(csid, top);
                if (listSTS != null && listSTS.Count > 0)
                {
                    int loop = 0;
                    sb.Append("<!-- ID:还关注子品牌ID Name:子品牌名 ShowName:子品牌显示名 Pic:子品牌图片 PriceRange:子品牌报价区间 AllSpell:子品牌全拼Url使用 -->");
                    foreach (EnumCollection.SerialToSerial sts in listSTS)
                    {
                        if (loop >= top)
                        { break; }
                        loop++;
                        sb.AppendLine(string.Format("<Cs ID=\"{0}\" Name=\"{1}\" ShowName=\"{2}\" Pic=\"{3}\" PriceRange=\"{4}\" AllSpell=\"{5}\"/>"
                            , sts.ToCsID
                            , System.Security.SecurityElement.Escape(sts.ToCsName)
                            , System.Security.SecurityElement.Escape(sts.ToCsShowName)
                            , (dicPicWhite.ContainsKey(sts.ToCsID) ? dicPicWhite[sts.ToCsID].Replace("_2.", "_5.") : WebConfig.DefaultCarPic)
                            // , sts.ToCsPic
                            , sts.ToCsPriceRange
                            , System.Security.SecurityElement.Escape(sts.ToCsAllSpell)));
                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }


        /// <summary>
        /// 取前几位新车
        /// </summary>
        private void RenderTopNewCar()
        {
            int showNewCarNum = 10;
            if (request.QueryString["top"] != null && request.QueryString["top"].ToString() != "")
            {
                string topCount = request.QueryString["top"].ToString();
                if (int.TryParse(topCount, out showNewCarNum))
                { }
            }

            Dictionary<int, string> dict = csb.GetAllSerialMarkDay();
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>(dict);
            List<KeyValuePair<int, string>> sublist = new List<KeyValuePair<int, string>>();
            foreach (KeyValuePair<int, string> key in list)
            {
                if (CommonFunction.DateDiff("d", BitAuto.Utils.ConvertHelper.GetDateTime(key.Value), DateTime.Now) >= 0)
                {
                    sublist.Add(key);
                }
            }

            for (int i = 0; i < sublist.Count; i++)
            {
                int serialId = sublist[i].Key;
                if (serialId <= 0) break;
                string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId);
                if (string.Equals(imgUrl, WebConfig.DefaultCarPic))
                { continue; }
                if (i >= showNewCarNum) break;
                string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(serialId));
                Car_SerialEntity cs = csb.Get_Car_SerialByCsID(serialId);
                if (cs == null || cs.Cs_Id <= 0)
                { continue; }
                string levelName = cs.Cs_CarLevel;
                switch (levelName)
                {
                    case "紧凑型车":
                        levelName = "紧凑型";
                        break;
                    case "中大型车":
                        levelName = "中大型";
                        break;
                }
                if (string.IsNullOrEmpty(levelName))
                {
                    levelName = "紧凑型";
                }
                //int levelId = (int)System.Enum.Parse(typeof(EnumCollection.SerialAllLevelEnum), levelName);
                //string levelUrl = "/" + ((EnumCollection.SerialAllLevelSpellEnum)levelId).ToString() + "/";
                string levelUrl = string.Format("/{0}/", CarLevelDefine.GetLevelSpellByName(levelName));
                if (priceRange.Trim().Length == 0)
                    priceRange = "暂无报价";
                sb.AppendLine("<Cs>");
                sb.AppendLine(string.Format("<ID>{0}</ID>", cs.Cs_Id));
                sb.AppendLine(string.Format("<ShowName>{0}</ShowName>", System.Security.SecurityElement.Escape(cs.Cs_ShowName)));
                sb.AppendLine(string.Format("<Img>{0}</Img>", System.Security.SecurityElement.Escape(imgUrl)));
                sb.AppendLine(string.Format("<Price>{0}</Price>", System.Security.SecurityElement.Escape(priceRange)));
                sb.AppendLine(string.Format("<Level>{0}</Level>", System.Security.SecurityElement.Escape(levelName)));
                sb.AppendLine(string.Format("<AllSpell>{0}</AllSpell>", System.Security.SecurityElement.Escape(cs.Cs_AllSpell)));
                sb.AppendLine("</Cs>");
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        //获取子品牌年款颜色
        private void RenderSerialYearColor()
        {
            int sid = BitAuto.Utils.ConvertHelper.GetInteger(request.QueryString["sid"]);
            string sql = @"select car.Car_YearType,cdb.* from car_relation car 
								left join car_serial cs on car.cs_id=cs.cs_id 
								left join cardatabase cdb on car.car_id=cdb.carid 
								where car.isState=0 and cs.isState=0 and cdb.paramid= 598 
								and car.Car_YearType>0  and cs.cs_id=" + sid;
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string key = dr["Car_YearType"].ToString();
                    if (dict.ContainsKey(key))
                    {
                        string str = dr["pvalue"].ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] temp = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            List<string> listColor = new List<string>();
                            foreach (string colorkey in temp)
                            {
                                if (!listColor.Contains(colorkey.Trim()))
                                { listColor.Add(colorkey.Trim()); }
                            }
                            List<string> list1 = dict[key];
                            // List<string> list2 = new List<string>(str.Split(','));
                            for (int j = 0; j < listColor.Count; j++)
                            {
                                if (!list1.Contains(listColor[j])) //除去重复项
                                {
                                    list1.Add(listColor[j]);
                                }
                            }
                            dict[key] = list1;
                        }
                    }
                    else
                    {
                        string str = dr["pvalue"].ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] temp = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            List<string> listColor = new List<string>();
                            foreach (string colorkey in temp)
                            {
                                if (!listColor.Contains(colorkey.Trim()))
                                { listColor.Add(colorkey.Trim()); }
                            }
                            dict.Add(key, listColor);
                        }
                    }
                }
                foreach (KeyValuePair<string, List<string>> kv in dict)
                {
                    sb.AppendFormat("<Year ID=\"{0}\">", kv.Key);
                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        string ssql = "select top 1 * from dbo.Car_SerialColor where type=0 and colorname=@colorname and cs_id=@cs_id";
                        SqlParameter[] _param ={
                                      new SqlParameter("@colorname",SqlDbType.NVarChar,50),
                                      new SqlParameter("@cs_id",SqlDbType.Int)
                                  };
                        _param[0].Value = kv.Value[i];
                        _param[1].Value = sid;
                        DataSet cds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, ssql, _param);
                        if (cds.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = cds.Tables[0];
                            sb.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\" RGB=\"{2}\" />", dt.Rows[0]["autoid"], dt.Rows[0]["colorname"], dt.Rows[0]["colorrgb"]);
                        }
                    }
                    sb.Append("</Year>");
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 取子品牌内饰颜色 有RGB值的内饰颜色
        /// </summary>
        private void RenderSerialYearInteriorColor()
        {
            int sid = BitAuto.Utils.ConvertHelper.GetInteger(request.QueryString["sid"]);
            if (sid <= 0)
            { return; }
            string sql = @"select car.Car_YearType,cdb.carid,cdb.paramid,cdb.pvalue from car_relation car 
								left join car_serial cs on car.cs_id=cs.cs_id 
								left join cardatabase cdb on car.car_id=cdb.carid 
								where car.isState=0 and cs.isState=0 and cdb.paramid= 801 
								and car.Car_YearType>0  and cs.cs_id=@cs_id";
            SqlParameter[] _paramBase = { new SqlParameter("@cs_id", SqlDbType.Int) };
            _paramBase[0].Value = sid;
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _paramBase);
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string key = dr["Car_YearType"].ToString();
                    if (dict.ContainsKey(key))
                    {
                        string str = dr["pvalue"].ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] temp = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            List<string> listColor = new List<string>();
                            foreach (string colorkey in temp)
                            {
                                if (!listColor.Contains(colorkey.Trim()))
                                { listColor.Add(colorkey.Trim()); }
                            }
                            List<string> list1 = dict[key];
                            // List<string> list2 = new List<string>(str.Split(','));
                            for (int j = 0; j < listColor.Count; j++)
                            {
                                if (!list1.Contains(listColor[j])) //除去重复项
                                {
                                    list1.Add(listColor[j]);
                                }
                            }
                            dict[key] = list1;
                        }
                    }
                    else
                    {
                        string str = dr["pvalue"].ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] temp = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            List<string> listColor = new List<string>();
                            foreach (string colorkey in temp)
                            {
                                if (!listColor.Contains(colorkey.Trim()))
                                { listColor.Add(colorkey.Trim()); }
                            }
                            dict.Add(key, listColor);
                        }
                    }
                }

                if (dict.Count > 0)
                {
                    #region 取有RGB值的内饰颜色
                    Dictionary<string, SerialColorStruct> dicNeiShiColor = new Dictionary<string, SerialColorStruct>();
                    string neishiSQL = "select autoid,colorName,colorRGB from dbo.Car_SerialColor where type=1 and cs_id=@cs_id";
                    SqlParameter[] _paramNeiShi = { new SqlParameter("@cs_id", SqlDbType.Int) };
                    _paramNeiShi[0].Value = sid;
                    DataSet cds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, neishiSQL, _paramNeiShi);
                    if (cds != null && cds.Tables.Count > 0 && cds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in cds.Tables[0].Rows)
                        {
                            int autoid = ConvertHelper.GetInteger(dr["autoid"]);
                            string colorName = dr["colorName"].ToString().Trim();
                            string colorRGB = dr["colorRGB"].ToString().Trim();
                            if (colorRGB != "")
                            {
                                SerialColorStruct scs = new SerialColorStruct();
                                scs.AutoID = autoid;
                                scs.ColorName = colorName;
                                scs.ColorRGB = colorRGB;
                                if (!dicNeiShiColor.ContainsKey(colorName))
                                {
                                    dicNeiShiColor.Add(colorName, scs);
                                }
                            }
                        }
                    }
                    #endregion
                    if (dicNeiShiColor.Count > 0)
                    {
                        foreach (KeyValuePair<string, List<string>> kv in dict)
                        {
                            List<string> tempList = new List<string>();
                            for (int i = 0; i < kv.Value.Count; i++)
                            {
                                if (dicNeiShiColor.ContainsKey(kv.Value[i]))
                                {
                                    tempList.Add(string.Format("<Item ID=\"{0}\" Name=\"{1}\" RGB=\"{2}\" />"
                                        , dicNeiShiColor[kv.Value[i]].AutoID, dicNeiShiColor[kv.Value[i]].ColorName, dicNeiShiColor[kv.Value[i]].ColorRGB));
                                }
                            }
                            if (tempList.Count > 0)
                            {
                                sb.Append(string.Format("<Year ID=\"{0}\">{1}</Year>", kv.Key, string.Join("", tempList.ToArray())));
                            }
                        }
                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 输出子品牌年款
        /// </summary>
        private void RenderCsYearList()
        {
            bool isAllSale = true;
            if (request.QueryString["isAllSale"] != null && request.QueryString["isAllSale"].ToString() != "")
            {
                if (request.QueryString["isAllSale"].ToString().ToLower() == "0")
                { isAllSale = false; }
            }
            // 按子品牌请求
            int wangCsid = 0;
            if (request.QueryString["csid"] != null && request.QueryString["csid"].ToString() != "")
            {
                if (int.TryParse(request.QueryString["csid"].ToString(), out wangCsid))
                { }
            }
            string sql = @"select cs_id,Car_YearType
							from dbo.Car_relation
							where cs_id>0 and Car_YearType>0
							and isState=0 
							{0}
							group by cs_id,Car_YearType
							order by cs_id,Car_YearType";
            string isGetOneCs = wangCsid > 0 ? string.Format(" and cs_id={0} ", wangCsid) : "";
            if (!isAllSale)
            { sql = string.Format(sql, " and car_SaleState<>96 " + isGetOneCs); }
            else
            { sql = string.Format(sql, isGetOneCs); }
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Dictionary<int, List<int>> dicCsYear = new Dictionary<int, List<int>>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int csid = int.Parse(dr["cs_id"].ToString());
                    int year = int.Parse(dr["Car_YearType"].ToString());
                    if (!dicCsYear.ContainsKey(csid))
                    {
                        List<int> csYear = new List<int>();
                        csYear.Add(year);
                        dicCsYear.Add(csid, csYear);
                    }
                    else
                    {
                        if (!dicCsYear[csid].Contains(year))
                        { dicCsYear[csid].Add(year); }
                    }
                }
                if (dicCsYear.Count > 0)
                {
                    foreach (KeyValuePair<int, List<int>> kvpCs in dicCsYear)
                    {
                        sb.AppendLine("<Cs ID=\"" + kvpCs.Key.ToString() + "\">");
                        if (kvpCs.Value.Count > 0)
                        {
                            foreach (int year in kvpCs.Value)
                            {
                                sb.AppendLine("<Year ID=\"" + year.ToString() + "\"/>");
                            }
                        }
                        sb.AppendLine("</Cs>");
                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 取子品牌对比数据
        /// </summary>
        private void RenderCsCompareData()
        {
            int csID = 0;
            int topCount = 5;
            if (request.QueryString["csID"] != null && request.QueryString["csID"].ToString() != "")
            {
                string csIDStr = request.QueryString["csID"].ToString();
                if (int.TryParse(csIDStr, out csID))
                { }
            }
            if (request.QueryString["top"] != null && request.QueryString["top"].ToString() != "")
            {
                string top = request.QueryString["top"].ToString();
                if (int.TryParse(top, out topCount))
                {
                    if (topCount < 0 || topCount > 1000)
                    {
                        topCount = 5;
                    }
                }
            }
            Dictionary<int, EnumCollection.CsBaseInfo> dicCs = csb.GetAllCsBaseInfoDic();
            if (dicCs.ContainsKey(csID))
            {
                EnumCollection.CsBaseInfo ssfi = dicCs[csID];
                sb.Append("<Serial ID=\"" + csID.ToString() + "\" Name=\"" + ssfi.CsName + "\" >");
                List<EnumCollection.SerialHotCompareData> lshcd = base.GetSerialHotCompareByCsID(csID, topCount);
                if (lshcd.Count > 0)
                {
                    foreach (EnumCollection.SerialHotCompareData shcd in lshcd)
                    {
                        sb.Append("<Item CsID=\"" + shcd.CompareCsID.ToString() + "\" ");
                        sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(shcd.CompareCsName) + "\" ");
                        sb.Append(" CsShowName=\"" + System.Security.SecurityElement.Escape(shcd.CompareCsShowName) + "\" ");
                        sb.Append(" CsAllSpell=\"" + shcd.CompareCsAllSpell + "\" />");
                    }
                }
                sb.Append("</Serial>");
            }
            else
            {
                // add by chengl Oct.26.2012
                sb.Append("<Serial/>");
            }
            CommonFunction.EchoXml(response, sb.ToString(), "");
        }

        /// <summary>
        /// 子品牌近30天UV排序 for wangzt
        /// </summary>
        private void RenderCsUVData()
        {
            DataSet ds = base.GetAllSerialNewly30Day();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                sb.AppendLine("<!-- ID:子品牌ID Name:子品牌名 SEOName:子品牌SEO名 UV:UV数 ShowName:子品牌显示名 Level:级别 Sale:销售状态 AllSpell:全拼(地址使用) -->");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int csid = int.Parse(dr["cs_ID"].ToString());
                    int uvCount = int.Parse(dr["Pv_SumNum"].ToString());
                    string csName = dr["cs_name"].ToString().Trim();
                    // add by chengl Aug.21.2014
                    sb.AppendLine(string.Format("<Serial ID=\"{0}\" Name=\"{1}\" SEOName=\"{2}\" UV=\"{3}\" ShowName=\"{4}\" Level=\"{5}\" Sale=\"{6}\" AllSpell=\"{7}\" />"
                        , csid
                        , System.Security.SecurityElement.Escape(csName)
                        , System.Security.SecurityElement.Escape(dr["cs_seoname"].ToString().Trim())
                        , uvCount
                        , System.Security.SecurityElement.Escape(dr["cs_showname"].ToString().Trim())
                        , System.Security.SecurityElement.Escape(dr["cs_CarLevel"].ToString().Trim())
                        , System.Security.SecurityElement.Escape(dr["cssaleState"].ToString().Trim())
                        , System.Security.SecurityElement.Escape(dr["allspell"].ToString().Trim())
                        ));

                    //sb.AppendLine("<Serial ID=\"" + csid + "\" Name=\""
                    //	+ System.Security.SecurityElement.Escape(csName)
                    //	+ "\" SEOName=\"" +
                    //	System.Security.SecurityElement.Escape(dr["cs_seoname"].ToString().Trim())
                    //	+ "\" UV=\"" + uvCount + "\" />");
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "SerialSort");
        }

        #region   空间数据for冯津  2016-11-22
        /// <summary>
        /// 根据 车系Id 输出关键报告 空间 车款数据
        /// </summary>
        private void RenderAllCarSpaceDataByCsId()
        {
            response.ContentType = "application/x-javascript";
            Car_BasicBll _carBLL = new Car_BasicBll();
            int serialId = ConvertHelper.GetInteger(request.QueryString["serialid"]);
            List<CarInnerSpaceEntity> list = new List<CarInnerSpaceEntity>();
            if (serialId <= 0)
            {
                response.Write("[]");
                return;
            }
            string sql = @"SELECT  cri.Car_Id, cri.[FirstSeatToTop], cri.[SecondSeatToTop],
                                    cri.[FirstSeatDistance], crmd.ModelName, Height, [Weight], ImageUrl, ParaId, [Type],
                                    cri.[ThirdSeatToTop]
                            FROM    [dbo].[Car_relation_InnerSpace] cri
                                    LEFT JOIN [dbo].[Car_relation_Model_Data] crmd ON crmd.Car_Id = cri.Car_Id
                                                                                      AND crmd.IsState = 0
                                    LEFT JOIN dbo.Car_relation car ON car.Car_Id = cri.Car_Id
                            WHERE   car.Cs_Id = @SerialId
                                    AND cri.IsState = 0
                            ORDER BY car.Car_YearType";
            SqlParameter[] parameters =
            {
                new SqlParameter("@SerialId",SqlDbType.Int),
            };
            parameters[0].Value = serialId;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql,
                parameters);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var query = ds.Tables[0].AsEnumerable();
                var ig = query.GroupBy<DataRow, int>(dr => ConvertHelper.GetInteger(dr["Car_Id"]));
                foreach (IGrouping<int, DataRow> g in ig)
                {
                    int carId = g.Key;

                    Dictionary<int, string> dictCarParams = _carBLL.GetCarAllParamByCarID(carId);

                    List<DataRow> drList = g.ToList();
                    CarInnerSpaceEntity entity = new CarInnerSpaceEntity();
                    entity.CarId = carId;
                    entity.FirstseatToTop = ConvertHelper.GetDouble(drList[0]["FirstSeatToTop"]);
                    entity.FirstSeatDistance = ConvertHelper.GetDouble(drList[0]["FirstSeatDistance"]);
                    entity.SecondSeatToTop = ConvertHelper.GetDouble(drList[0]["SecondSeatToTop"]);
                    entity.ThirdSeatToTop = ConvertHelper.GetDouble(drList[0]["ThirdSeatToTop"]);
                    entity.TrunkCapacity = dictCarParams.ContainsKey(465)
                       ? ConvertHelper.GetInteger(dictCarParams[465])
                       : 0;
                    entity.TrunkCapacityE = dictCarParams.ContainsKey(466)
                        ? ConvertHelper.GetInteger(dictCarParams[466])
                        : 0;
                    entity.FirstSeatToTopLevel = Leval(entity.FirstseatToTop, CommonEnum.CarInnerSpaceType.FirstSeatToTop);
                    entity.FirstSeatDistanceLevel = Leval(entity.FirstseatToTop, CommonEnum.CarInnerSpaceType.FirstSeatDistance);
                    entity.SecondSeatToTopLevel = Leval(entity.SecondSeatToTop, CommonEnum.CarInnerSpaceType.SecondSeatToTop);
                    entity.ThirdSeatToTopLevel = Leval(entity.ThirdSeatToTop, CommonEnum.CarInnerSpaceType.ThirdSeatToTop);
                    entity.TrunkCapacityLevel = Leval(entity.TrunkCapacity, CommonEnum.CarInnerSpaceType.BackBoot);

                    var firstSeatModel = drList.FirstOrDefault(dr => ConvertHelper.GetInteger(dr["type"]) == (int)CommonEnum.CarInnerSpaceType.FirstSeatToTop);
                    if (firstSeatModel != null)
                    {
                        entity.FirstSeatToTopModelHeight = ConvertHelper.GetDouble(firstSeatModel["Height"]);
                        entity.FirstSeatToTopModelWeight = ConvertHelper.GetDouble(firstSeatModel["Weight"]);
                        entity.FirstseatToTopImageUrl = ConvertHelper.GetString(firstSeatModel["ImageUrl"]);
                    }
                    var secondSeatModel = drList.FirstOrDefault(dr => ConvertHelper.GetInteger(dr["type"]) == (int)CommonEnum.CarInnerSpaceType.SecondSeatToTop);
                    if (secondSeatModel != null)
                    {
                        entity.SecondSeatToTopModelHeight = ConvertHelper.GetDouble(secondSeatModel["Height"]);
                        entity.SecondSeatToTopModelWeight = ConvertHelper.GetDouble(secondSeatModel["Weight"]);
                        entity.SecondSeatToTopImageUrl = ConvertHelper.GetString(secondSeatModel["ImageUrl"]);
                    }
                    var firstSeatDistanceModel = drList.FirstOrDefault(dr => ConvertHelper.GetInteger(dr["type"]) == (int)CommonEnum.CarInnerSpaceType.FirstSeatDistance);
                    if (firstSeatDistanceModel != null)
                    {
                        entity.FirstSeatDistanceModelHeight = ConvertHelper.GetDouble(firstSeatDistanceModel["Height"]);
                        entity.FirstSeatDistanceModelWeight = ConvertHelper.GetDouble(firstSeatDistanceModel["Weight"]);
                        entity.FirstSeatDistanceImageUrl = ConvertHelper.GetString(firstSeatDistanceModel["ImageUrl"]);
                    }
                    var thirdSeatModel = drList.FirstOrDefault(dr => ConvertHelper.GetInteger(dr["type"]) == (int)CommonEnum.CarInnerSpaceType.ThirdSeatToTop);
                    if (thirdSeatModel != null)
                    {
                        entity.ThirdSeatToTopModelHeight = ConvertHelper.GetDouble(thirdSeatModel["Height"]);
                        entity.ThirdSeatToTopModelWeight = ConvertHelper.GetDouble(thirdSeatModel["Weight"]);
                        entity.ThirdSeatToTopImageUrl = ConvertHelper.GetString(thirdSeatModel["ImageUrl"]);
                    }
                    var trunkCapacityModel = drList.FirstOrDefault(dr => ConvertHelper.GetInteger(dr["type"]) == (int)CommonEnum.CarInnerSpaceType.BackBoot && ConvertHelper.GetInteger(dr["ParaId"]) == 465);
                    if (trunkCapacityModel != null)
                    {
                        entity.TrunkCapacityImageUrl = ConvertHelper.GetString(trunkCapacityModel["ImageUrl"]);
                    }
                    var trunkCapacityEModel = drList.FirstOrDefault(dr => ConvertHelper.GetInteger(dr["type"]) == (int)CommonEnum.CarInnerSpaceType.BackBoot && ConvertHelper.GetInteger(dr["ParaId"]) == 466);
                    if (trunkCapacityEModel != null)
                    {
                        entity.TrunkCapacityEImageUrl = ConvertHelper.GetString(trunkCapacityEModel["ImageUrl"]);
                    }
                    list.Add(entity);
                }
            }
            string json = JsonConvert.SerializeObject(list);
            response.Write(json);
        }
        public string Leval(double number, CommonEnum.CarInnerSpaceType carInnerSpaceType)
        {
            string msg = string.Empty;
            switch ((int)carInnerSpaceType)
            {
                case (int)CommonEnum.CarInnerSpaceType.FirstSeatToTop:
                    if (number >= 95)
                    {
                        msg = "宽裕";
                    }
                    if (number >= 90 && number < 95)
                    {
                        msg = "适中";
                    }
                    if (number < 90)
                    {
                        msg = "局促";
                    }
                    break;
                case (int)CommonEnum.CarInnerSpaceType.SecondSeatToTop:
                    if (number >= 95)
                    {
                        msg = "宽裕";
                    }
                    if (number >= 90 && number < 95)
                    {
                        msg = "适中";
                    }
                    if (number < 90)
                    {
                        msg = "局促";
                    }
                    break;
                case (int)CommonEnum.CarInnerSpaceType.FirstSeatDistance:
                    if (number >= 75)
                    {
                        msg = "宽裕";
                    }
                    if (number >= 60 && number < 75)
                    {
                        msg = "适中";
                    }
                    if (number < 60)
                    {
                        msg = "局促";
                    }
                    break;
                case (int)CommonEnum.CarInnerSpaceType.ThirdSeatToTop:
                    if (number >= 95)
                    {
                        msg = "宽裕";
                    }
                    if (number >= 90 && number < 95)
                    {
                        msg = "适中";
                    }
                    if (number < 90)
                    {
                        msg = "局促";
                    }
                    break;
                case (int)CommonEnum.CarInnerSpaceType.BackBoot:
                    if (number >= 500)
                    {
                        msg = "宽裕";
                    }
                    if (number >= 300 && number < 500)
                    {
                        msg = "适中";
                    }
                    if (number > 0 && number < 300)
                    {
                        msg = "局促";
                    }
                    if (number < 0)
                    {
                        msg = "";
                    }
                    break;
                default:
                    msg = "";
                    break;
            }
            return msg;
        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}