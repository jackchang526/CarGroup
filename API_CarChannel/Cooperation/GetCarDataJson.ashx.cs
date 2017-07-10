using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using System.Text;
using System.Data.SqlClient;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using System.Xml;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using System.Data;
using Newtonsoft.Json;

namespace BitAuto.CarChannelAPI.Web.Cooperation
{
    /// <summary>
    /// 热门车360运营\114la
    /// modified by chengl Apr.5.2012 增加魔图 pagechoice 业务支持 根据子品牌ID取子品牌json数据
    /// </summary>
    public class GetCarDataJson : PageBase, IHttpHandler
    {
        // 魔图 pagechoice 业务支持 memcache
        private string coopPageChoiceCacheTemp = "Car_List_SerialPageChoice_{0}";

        HttpRequest request;
        HttpResponse response;
        private StringBuilder sb = new StringBuilder();
        // 白底图
        private Dictionary<int, string> dicPicWhite;
        // 报价区间
        private Dictionary<int, string> dicCsPrice;
        // 合作业务名 保持 GetCarData一致
        private string name = "";

        public new void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            request = context.Request;
            response = context.Response;
            response.ContentType = "application/x-javascript";
            // -- set cache for 
            response.Cache.SetNoServerCaching();
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetMaxAge(TimeSpan.FromMinutes(60));
            string dept = string.IsNullOrEmpty(request.QueryString["dept"]) ? "" : request.QueryString["dept"].ToString().Trim().ToLower();
            string type = string.IsNullOrEmpty(request.QueryString["type"]) ? "" : request.QueryString["type"].ToString().Trim().ToLower();
            name = string.IsNullOrEmpty(request.QueryString["name"]) ? "" : request.QueryString["name"].ToString().Trim().ToLower();
            // add by chengl Oct.26.2012 add 114la
            if (dept == "360" || dept == "114la")
            {
                switch (type)
                {
                    case "hot": CoopGetCarPriceRange(); break;
                    case "car": CoopGetCarData(); break;
                    case "master": CoopGetMasterData(); break;
                    case "brandorserial": CoopGetBrandOrSerial(); break;
                }
            }
            // add by chengl Apr.5.2012
            else if (dept == "pagechoice")
            {
                GetCarInfoForPageChoice();
            }
            // add by chengl Jun.29.2012
            else if (dept == "5w")
            {
                // 高总新加 同360接口
                switch (type)
                {
                    case "hot": CoopGetCarPriceRange(); break;
                    case "car": CoopGetCarData(); break;
                    case "master": CoopGetMasterData(); break;
                    case "brandorserial": CoopGetBrandOrSerial(); break;
                }
            }
            // add by chengl Jun.24.2013 搜狗
            else if (dept == "sougou")
            {
                switch (type)
                {
                    case "hot": SouGouGetCarPriceRange(); break;
                    default: break;
                }
            }
            else if (dept == "hao123")
            {
                switch (type)
                {
                    case "leveltop": Hao123GetLevelTop(); break;
                    default: break;
                }
            }

            //百度  add by wangzheng 2015-06-04
            else if (dept == "baidu")
            {
                switch (type)
                {
                    case "getcsimg":
                        GetCsImgForBaidu();
                        break;
                    case "getcswenzhang":
                        GetCsWenzhangForBaidu();
                        break;
                    case "getcsinfo":
                        GetSerialData();
                        break;

                    default: break;
                }
            }
            else
            { }

            List<string> listCoopName = LoadCoopNameXml();
            // 好贷&易车购车贷款合作
            // add by chengl Dec.12.2014 木仓接口 车一百
            // add by chengl Dec.23.2014 搜狗号码通
            // add by chengl Jul.29.2015 易点时空
            // add by chengl Jan.14.2016 百度图片
            // add by chengl Feb.22.2016 微车
            // add by chengl Mar.24.2016 新美大
            // add by chengl Apr.5.2016 UC
            // add by chengl May.30.2016 智选车
            //if (name == "haodai" || name == "caronehundred" || name == "mucang"
            //	|| name == "sougouphone" || name == "rong360" || name == "easypoint"
            //	|| name == "baidupic" || name == "weicar" || name == "xinmeida" || name == "uccar"
            //	|| name == "selecar")
            if (listCoopName.Contains(name))
            {
                switch (type)
                {
                    case "car": HaoDaiGetCarByID(); break;
                    case "serial": HaoDaiGetSerialByID(); break;
                    case "brand": HaoDaiGetBrandByID(); break;
                    case "master": HaoDaiGetMasterByID(); break;
                    default: break;
                }
            }

            context.Response.End();
        }

        #region 百度

        private void GetCsImgForBaidu()
        {
            int csId = 0;
            string csidStr = request.QueryString["csId"];
            if (!string.IsNullOrEmpty(csidStr) && int.TryParse(csidStr, out csId))
            { }
            if (csId > 0)
            {
                var CsPhotoCount = 0;
                string xmlPicUrl = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialPhotoListPath, csId));
                DataSet dsCsPic = this.GetXMLDocToDataSetByURLForCache("CarChannel_SerialAllPic_" + csId, xmlPicUrl, 60);
                // 子品牌分类
                if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("A") &&
                    dsCsPic.Tables["A"].Rows.Count > 0)
                {
                    //图片总数
                    foreach (DataRow row in dsCsPic.Tables["A"].Rows)
                    {
                        if (row["N"].ToString() != "")
                        {
                            int count = 0;
                            if (int.TryParse(row["N"].ToString(), out count))
                            {
                                CsPhotoCount += count;
                            }
                        }
                    }
                }
                //取分类照片
                if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("C") &&
                    dsCsPic.Tables["C"].Rows.Count > 0)
                {
                    var imgTypeDic = new Dictionary<int, string[]>
                    {
                        {6, new []{"waiguanURL","外观"}},
                        {7,  new []{"neishiURL","内饰"}},
                        {8,  new []{"kongjianURL","空间"}},
                        {11,  new []{"guanfangURL","官方"}},
                        {12, new []{ "tujieURL","图解"}},
                        {17,  new []{"chezhanURL","车展"}}
                    };
                    var index = 1;
                    var dataJsonStr = "{{\"img_url\": \"{0}\",\"type\": \"{1}\",\"clickLink\": \"{2}\",\"image_index\": \"{3}\"}}";
                    var dataJsonStrList = new List<string>();
                    var tujiUrlStr = new StringBuilder();
                    foreach (var dic in imgTypeDic)
                    {
                        tujiUrlStr.AppendFormat(",\"{0}\":\"{1}\"", dic.Value[0], string.Format("http://photo.bitauto.com/serialmore/{0}/{1}/?WT.mc_id=bdqcsp", csId, dic.Key));

                        if (dic.Key == 12) continue;  //不取图解的图片
                        DataRow[] drs = dsCsPic.Tables["C"].Select(" P ='" + dic.Key + "' ");
                        if (drs == null || drs.Length == 0)
                        { continue; }
                        var imgId = Convert.ToInt32(drs[0]["I"].ToString());
                        var imgU = drs[0]["U"].ToString();
                        var imgName = drs[0]["D"].ToString();
                        var imgUrl = CommonFunction.GetPublishHashImgUrl(4, imgU, imgId);
                        var clickUrl = string.Format("http://photo.bitauto.com/picture/{0}/{1}/?WT.mc_id=bdqcsp", csId, imgId);
                        dataJsonStrList.Add(string.Format(dataJsonStr
                            , imgUrl
                            , dic.Value[1]
                            , clickUrl
                            , index));
                        index++;
                    }
                    if (index != 6)  //图片不够，用外观图补
                    {
                        DataRow[] drs = dsCsPic.Tables["C"].Select(" P ='6' ");
                        if (drs.Count() > 1 && (drs.Count() - 1) >= ((imgTypeDic.Count - 1) - (index - 1)))
                        {
                            for (int i = 0; i < ((imgTypeDic.Count - 1) - (index - 1)); i++)
                            {
                                var imgId = Convert.ToInt32(drs[i + 1]["I"].ToString());
                                var imgU = drs[i + 1]["U"].ToString();
                                var imgUrl = CommonFunction.GetPublishHashImgUrl(4, imgU, imgId);
                                var clickUrl = string.Format("http://photo.bitauto.com/picture/{0}/{1}/", csId, imgId);
                                dataJsonStrList.Add(string.Format(dataJsonStr
                                    , imgUrl
                                    , imgTypeDic[6]
                                    , clickUrl
                                    , index));
                            }
                        }
                    }
                    if (dataJsonStrList.Count == 5)
                    {
                        sb.AppendFormat("{{ \"seriesId\": \"{0}\",\"detail\": [{{\"category\": \"精美图片\",\"image_amount\": \"{1}\",{2},\"data\": [{3}]}}]}}"
                            , csId
                            , CsPhotoCount
                            , tujiUrlStr.ToString().Substring(1)
                            , string.Join(",", dataJsonStrList)
                            );
                    }
                }
            }
            response.Write(sb.ToString());
        }


        private string baiduWenzhangStr = "{{ \"seriesId\": \"{0}\",\"detail\": [{{\"category\": \"最新文章\",\"moreInfo\": \"{1}\",\"data\": [{2}]}}]}}";
        private void GetCsWenzhangForBaidu()
        {
            int csId = 0;
            string csidStr = request.QueryString["csId"];
            if (!string.IsNullOrEmpty(csidStr) && int.TryParse(csidStr, out csId))
            { }
            if (csId > 0)
            {
                var serialInfo = new Car_SerialBll().GetSerialBaseInfoFromMemCache(csId);
                var csAllSpell = serialInfo["CsAllSpell"];
                var url = string.Format("http://car.bitauto.com/{0}/wenzhang/?WT.mc_id=bdqcsp", csAllSpell);
                var carTypeIdList = new List<int>()
                    {
                    (int)CarNewsType.shijia,
                    (int)CarNewsType.daogou,
                    (int)CarNewsType.yongche,
                    (int)CarNewsType.gaizhuang,
                    (int)CarNewsType.anquan,
                    (int)CarNewsType.xinwen,
                    (int)CarNewsType.pingce,
                    (int)CarNewsType.treepingce
                    };
                var newsCount = 0;
                var newsDs = new CarNewsBll().GetSerialNewsAllData(csId, carTypeIdList, 10, 1, ref newsCount);
                var dataStrLists = new List<string>();
                if (newsDs != null && newsDs.Tables[0].Rows.Count > 0)
                {
                    string dataStr = "{{\"type\": \"{0}\",\"title\": \"{1}\",\"infoLink\": \"{2}\",\"resourceTime\": \"{3}\",\"intro\": \"{4}\"}}";
                    foreach (DataRow dr in newsDs.Tables[0].Rows)
                    {
                        var type = 0;
                        Int32.TryParse(dr["CarNewsTypeId"].ToString(), out type);
                        var tempstr = string.Format(dataStr
                            , CarNewsTypeName.GetCarNewsTypeName(type)
                            , CommonFunction.GetUnicodeByString(dr["Title"].ToString())
                            , string.IsNullOrEmpty(dr["FilePath"].ToString()) ? "" : dr["FilePath"] + "?WT.mc_id=bdqcsp"
                            , Convert.ToDateTime(dr["PublishTime"]).ToString("yyyy-MM-dd HH:mm")
                            , CommonFunction.GetUnicodeByString(Convert.ToString(dr["Content"]).Trim())
                            );
                        dataStrLists.Add(tempstr);
                    }
                    if (dataStrLists.Count > 0)
                    {
                        sb.AppendFormat(baiduWenzhangStr
                            , csId
                            , url
                            , string.Join(",", dataStrLists)
                            );
                    }
                }
            }
            response.Write(sb.ToString());
        }

        private void GetSerialData()
        {
            int sku_id = ConvertHelper.GetInteger(request.QueryString["sku_id"]);
            string from = request.QueryString["from"];
            if (string.IsNullOrEmpty(from))
            {
                from = "WT.mc_id=mbdfdbjh";
            }
            object data = null;
            if (sku_id > 0)
            {
                SerialEntity ce = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, sku_id);
                if (ce != null)
                {
                    int price = 0;
                    if (ce.Price.IndexOf('-') != -1)
                    {
                        price = ConvertHelper.GetInteger(ConvertHelper.GetDouble(ce.Price.Split('-')[0]) * 1000000);
                    }
                    else
                    {
                        price = ConvertHelper.GetInteger(ConvertHelper.GetDouble(ce.Price.Replace("万", "")) * 1000000);
                    }
                    data = new
                    {
                        sku_id = sku_id,
                        tp_src = "yiche",
                        //from = from,
                        source = "易车",
                        is_on_sale = ce.SaleState == "在销" ? 1 : 0,
                        name = ce.ShowName,
                        desc = ce.ShowName,
                        img = Car_SerialBll.GetSerialImageUrl(sku_id, "6"),
                        price = price,
                        mprice = "",
                        promotion_url = string.Format("http://car.h5.yiche.com/{0}/?order=page0,page1,page7,page2,page5,page3,page4,page6,page9&ad=0&WT.mc_id=mbdfdbjh", ce.AllSpell)
                    };
                }
            }

            var json = new { errmsg = "成功", errno = "0", data = data };
            string result = JsonConvert.SerializeObject(json);
            response.Write(result);
        }

        #endregion

        #region 好贷&易车购车贷款合作

        private void HaoDaiGetCarByID()
        {
            string callback = request.QueryString["callback"];
            int carid = 0;
            string caridStr = request.QueryString["id"];
            if (!string.IsNullOrEmpty(caridStr) && int.TryParse(caridStr, out carid))
            { }
            if (carid > 0)
            {
                Car_BasicBll carBll = new Car_BasicBll();
                DataSet dsCarBasic = carBll.GetCarDetailById(carid);
                if (dsCarBasic != null && dsCarBasic.Tables.Count > 0 && dsCarBasic.Tables[0].Rows.Count > 0)
                {
                    // modified by chengl Jan.12.2015
                    // 排量
                    Dictionary<int, string> dic785 = carBll.GetCarParamExDic(785);
                    // 最大马力
                    Dictionary<int, string> dic791 = carBll.GetCarParamExDic(791);
                    // 档位个数
                    Dictionary<int, string> dic724 = carBll.GetCarParamExDic(724);
                    // 变速箱
                    Dictionary<int, string> dic712 = carBll.GetCarParamExDic(712);

                    // 车型焦点图第一张
                    string imgURL = "";
                    int csid = BitAuto.Utils.ConvertHelper.GetInteger(dsCarBasic.Tables[0].Rows[0]["cs_id"].ToString());
                    int year = BitAuto.Utils.ConvertHelper.GetInteger(dsCarBasic.Tables[0].Rows[0]["Car_yearType"].ToString());
                    XmlDocument doc = carBll.GetCarDefaultPhoto(csid, carid, year);
                    XmlNodeList xnl = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
                    if (xnl != null && xnl.Count > 0)
                    { imgURL = (xnl[0].Attributes["ImageUrl"] != null ? xnl[0].Attributes["ImageUrl"].Value.ToString().Replace("_2.", "_4.") : ""); }
                    if (imgURL == "")
                    {
                        // 补子品牌
                        List<SerialFocusImage> imgList = new Car_SerialBll().GetSerialFocusImageList(csid);
                        if (imgList.Count > 0)
                        { imgURL = String.Format(imgList[0].ImageUrl, 4); }
                    }

                    sb.Append(string.Format("{{\"CarID\":{0},\"CarName\":\"{1}\",\"CarYear\":\"{2}\",\"CsID\":{3},\"Displacement\":\"{4}\",\"CarReferPrice\":\"{5}\",\"Power\":\"{6}\",\"Gearbox\":\"{7}\",\"CoverImage\":\"{8}\",\"CarSaleState\":\"{9}\",\"CarProduceState\":\"{10}\"}}"
                        , carid
                        , CommonFunction.GetUnicodeByString(dsCarBasic.Tables[0].Rows[0]["Car_name"].ToString().Trim())
                        , dsCarBasic.Tables[0].Rows[0]["Car_yearType"].ToString()
                        , dsCarBasic.Tables[0].Rows[0]["Cs_ID"].ToString()
                        , (dic785.ContainsKey(carid) ? CommonFunction.GetUnicodeByString(dic785[carid] + "L") : "")
                        , (dsCarBasic.Tables[0].Rows[0]["car_ReferPrice"].ToString() != "" ? CommonFunction.GetUnicodeByString(dsCarBasic.Tables[0].Rows[0]["car_ReferPrice"].ToString() + "万") : "")
                        , (dic791.ContainsKey(carid) ? CommonFunction.GetUnicodeByString(dic791[carid] + "马力") : "")
                        , ((dic724.ContainsKey(carid) ? CommonFunction.GetUnicodeByString(dic724[carid] + "挡") : "")
                            + (dic712.ContainsKey(carid) ? CommonFunction.GetUnicodeByString(dic712[carid]) : ""))
                        , imgURL
                        , dsCarBasic.Tables[0].Rows[0]["Car_SaleState"].ToString()
                        , dsCarBasic.Tables[0].Rows[0]["Car_ProduceState"]
                        ));
                }
            }
            if (!string.IsNullOrEmpty(callback))
            {
                sb.Insert(0, callback + "(");
                sb.Append(")");
            }
            response.Write(sb.ToString());
        }

        private void HaoDaiGetSerialByID()
        {
            string callback = request.QueryString["callback"];
            int csid = 0;
            string csidStr = request.QueryString["id"];
            if (!string.IsNullOrEmpty(csidStr) && int.TryParse(csidStr, out csid))
            { }
            if (csid > 0)
            {
                Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();

                Dictionary<int, CsKoubeiBaseInfo> dictKoubei = new Car_SerialBll().GetAllCsKoubeiBaseInfo();

                string sql = @"SELECT  cs.cs_Id, cs.cs_Name, cs.cs_ShowName, cb.cb_id, cs.allSpell,
                                        cb.cb_Name, cmb.bs_Id, cmb.bs_Name, cmb.bs_Country,
                                        cb.cb_Country AS Cp_Country, cs.cs_CarLevel, csi.Engine_Exhaust,
                                        cs.CsSaleState
                                FROM    car_serial cs
                                        LEFT JOIN dbo.Car_Serial_Item csi ON cs.cs_Id = csi.cs_Id
                                        LEFT JOIN Car_Brand cb ON cs.cb_Id = cb.cb_Id
                                        LEFT JOIN dbo.Car_MasterBrand_Rel cmr ON cb.cb_Id = cmr.cb_Id
                                        LEFT JOIN dbo.Car_MasterBrand cmb ON cmr.bs_Id = cmb.bs_Id
                                WHERE   cs.IsState = 1
                                        AND cs.cs_Id = @csid";
                SqlParameter[] _param = { new SqlParameter("@csid", SqlDbType.Int) };
                _param[0].Value = csid;
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                    , CommandType.Text, sql, _param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sb.Append(string.Format("{{\"CsID\":{0},\"CsShowName\":\"{1}\",\"CBName\":\"{2}\",\"MBName\":\"{3}\",\"Logo\":\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_{4}_100.png\",\"CbID\":{5},\"BsID\":{4},\"CsPic\":\"{6}\",\"Country\":\"{7}\",\"Type\":\"{8}\",\"Displacement\":\"{9}\",\"Level\":\"{10}\",\"Url\":\"{11}\",\"CsPriceRange\":\"{12}\",\"KoubeiRating\":{13},\"CsSaleState\":\"{14}\"}}"
                        , csid
                        , CommonFunction.GetUnicodeByString(ds.Tables[0].Rows[0]["cs_ShowName"].ToString().Trim())
                        , CommonFunction.GetUnicodeByString(ds.Tables[0].Rows[0]["cb_Name"].ToString().Trim())
                        , CommonFunction.GetUnicodeByString(ds.Tables[0].Rows[0]["bs_Name"].ToString().Trim())
                        , ds.Tables[0].Rows[0]["bs_Id"].ToString()
                        , ds.Tables[0].Rows[0]["cb_Id"].ToString()
                        , (dicPicWhite.ContainsKey(csid) ? dicPicWhite[csid].Replace("_2.", "_6.") : WebConfig.DefaultCarPic)
                        , CommonFunction.GetUnicodeByString(ds.Tables[0].Rows[0]["bs_Country"].ToString().Trim())
                        , CommonFunction.GetUnicodeByString(
                            CommonFunction.GetImport(ds.Tables[0].Rows[0]["Cp_Country"].ToString().Trim(), ds.Tables[0].Rows[0]["bs_Country"].ToString().Trim()))
                        , CommonFunction.GetUnicodeByString(GetTop3EngineExhaust(ds.Tables[0].Rows[0]["Engine_Exhaust"].ToString().Trim()))
                        , CommonFunction.GetUnicodeByString(ds.Tables[0].Rows[0]["cs_CarLevel"].ToString().Trim())
                        , CommonFunction.GetUnicodeByString(ds.Tables[0].Rows[0]["allSpell"].ToString().Trim())
                        , CommonFunction.GetUnicodeByString(base.GetSerialPriceRangeByID(csid))
                        , dictKoubei.ContainsKey(csid) ? Math.Round(dictKoubei[csid].Rating, 2) : 0
                        , CommonFunction.GetUnicodeByString(ds.Tables[0].Rows[0]["CsSaleState"].ToString().Trim())
                        ));
                }
            }
            if (!string.IsNullOrEmpty(callback))
            {
                sb.Insert(0, callback + "(");
                sb.Append(")");
            }
            response.Write(sb.ToString());
        }

        /// <summary>
        /// 根据Car_Serial_Item 表排量字段取前3排量
        /// </summary>
        /// <param name="carSerialItem_Engine_Exhaust"></param>
        /// <returns></returns>
        private string GetTop3EngineExhaust(string carSerialItem_Engine_Exhaust)
        {
            string ee = "";
            if (!string.IsNullOrEmpty(carSerialItem_Engine_Exhaust))
            {
                string[] arrayEE = carSerialItem_Engine_Exhaust.Split(new char[] { '、' }, StringSplitOptions.RemoveEmptyEntries);
                if (arrayEE.Length > 3)
                {
                    List<string> listEE = new List<string>();
                    foreach (string arrItem in arrayEE)
                    {
                        listEE.Add(arrItem);
                        if (listEE.Count >= 3)
                        { break; }
                    }
                    ee = string.Join("、", listEE.ToArray());
                }
                else
                { ee = carSerialItem_Engine_Exhaust; }
            }
            return ee;
        }

        // 根据品牌ID取品牌数据
        private void HaoDaiGetBrandByID()
        {
            string callback = request.QueryString["callback"];
            int cbid = 0;
            string cbidStr = request.QueryString["id"];
            if (!string.IsNullOrEmpty(cbidStr) && int.TryParse(cbidStr, out cbid))
            { }
            if (cbid > 0)
            {
                string sql = @"select cb.cb_Id,cb.cb_Name,cmb.bs_Id
										from Car_Brand cb
										left join dbo.Car_MasterBrand_Rel cmb
										on cb.cb_Id=cmb.cb_Id
										where cb.IsState=1 and cb.cb_Id=@cbid";
                SqlParameter[] _param = { new SqlParameter("@cbid", SqlDbType.Int) };
                _param[0].Value = cbid;
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                    , CommandType.Text, sql, _param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sb.Append(string.Format("{{\"CbID\":{0},\"CbName\":\"{1}\",\"BsID\":{2}}}"
                        , cbid
                        , CommonFunction.GetUnicodeByString(ds.Tables[0].Rows[0]["cb_Name"].ToString().Trim())
                        , ds.Tables[0].Rows[0]["bs_Id"].ToString()));
                }
            }
            if (!string.IsNullOrEmpty(callback))
            {
                sb.Insert(0, callback + "(");
                sb.Append(")");
            }
            response.Write(sb.ToString());
        }

        private void HaoDaiGetMasterByID()
        {
            string callback = request.QueryString["callback"];
            int bsid = 0;
            string bsidStr = request.QueryString["id"];
            if (!string.IsNullOrEmpty(bsidStr) && int.TryParse(bsidStr, out bsid))
            { }
            if (bsid > 0)
            {
                string sql = @"select bs_Id,bs_Name,spell
										from dbo.Car_MasterBrand
										where IsState=1 and bs_Id=@bsid";
                SqlParameter[] _param = { new SqlParameter("@bsid", SqlDbType.Int) };
                _param[0].Value = bsid;
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                    , CommandType.Text, sql, _param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sb.Append(string.Format("{{\"BsID\":{0},\"BsName\":\"{1}\",\"Spell\":\"{2}\",\"Logo\":\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_{0}_100.png\"}}"
                        , bsid
                        , CommonFunction.GetUnicodeByString(ds.Tables[0].Rows[0]["bs_Name"].ToString().Trim())
                        , (ds.Tables[0].Rows[0]["spell"].ToString().Trim().Length > 0 ? ds.Tables[0].Rows[0]["spell"].ToString().Trim().Substring(0, 1) : "")
                        ));
                }
            }
            if (!string.IsNullOrEmpty(callback))
            {
                sb.Insert(0, callback + "(");
                sb.Append(")");
            }
            response.Write(sb.ToString());
        }

        #endregion

        #region hao123

        /// <summary>
        /// hao123 级别数据合作
        /// </summary>
        private void Hao123GetLevelTop()
        {
            string callback = request.QueryString["callback"];
            string key = string.Format("Cooperation_GetCarDataJson_Hao123_LevelTop");
            // 1次从memcache取数据
            object objMemCache = MemCache.GetMemCacheByKey(key);
            if (objMemCache != null)
            {
                sb.Append((string)objMemCache);
            }
            else
            {
                Dictionary<string, List<string>> dicLevel = new Dictionary<string, List<string>>();
                dicLevel.Add("jincou", new List<string>());
                dicLevel.Add("SUV", new List<string>());
                dicLevel.Add("zhongxing", new List<string>());
                dicLevel.Add("xiaoxing", new List<string>());
                DataSet dsCsLevelRank = new Car_SerialBll().GetAllSerialUVRAngeByLevel();
                if (dsCsLevelRank != null && dsCsLevelRank.Tables.Count > 0 && dsCsLevelRank.Tables[0].Rows.Count > 0)
                {
                    Dictionary<int, string> dicPrice = base.GetAllCsPriceRange();
                    foreach (DataRow dr in dsCsLevelRank.Tables[0].Rows)
                    {
                        string level = dr["cs_CarLevel"].ToString().Trim();
                        string csAllSpell = dr["allSpell"].ToString().Trim();
                        string csSeoName = dr["cs_seoname"].ToString().Trim();
                        int csid = int.Parse(dr["csID"].ToString());
                        string levelKey = "";
                        switch (level)
                        {
                            case "紧凑型车": levelKey = "jincou"; break;
                            case "SUV": levelKey = "SUV"; break;
                            case "中型车": levelKey = "zhongxing"; break;
                            case "小型车": levelKey = "xiaoxing"; break;
                        }
                        // 4个级别取前10
                        if (levelKey != "" && dicLevel.ContainsKey(levelKey) && dicLevel[levelKey].Count < 10)
                        {
                            string minP = "暂无";
                            string maxP = "暂无";
                            if (dicPrice.ContainsKey(csid))
                            {
                                string[] priceArray = dicPrice[csid].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                                if (priceArray.Length > 1)
                                {
                                    minP = priceArray[0].Replace("万", "");
                                    maxP = priceArray[1].Replace("万", "");
                                }
                            }
                            dicLevel[levelKey].Add(
                                string.Format("{{\"url\":\"http://car.bitauto.com/{0}/?WT.mc_id=h123nph\",\"model_name\":\"{1}\",\"price_min\":\"{2}\",\"price_max\":\"{3}\"}}"
                                , csAllSpell
                                , CommonFunction.GetUnicodeByString(csSeoName)
                                , minP
                                , maxP));
                        }
                    }
                }

                foreach (KeyValuePair<string, List<string>> kvp in dicLevel)
                {
                    if (kvp.Value.Count > 0)
                    {
                        if (sb.Length > 0)
                        { sb.Append(","); }
                        sb.Append(string.Format("\"{0}\":[", kvp.Key));
                        sb.Append(string.Join(",", kvp.Value.ToArray()));
                        sb.Append(string.Format("]"));
                    }
                }
                sb.Insert(0, "{");
                sb.Append("}");
                // 加入memcache
                MemCache.SetMemCacheByKey(key, sb.ToString(), 60 * 60 * 1000);
            }
            if (!string.IsNullOrEmpty(callback))
            {
                sb.Insert(0, callback + "(");
                sb.Append(")");
            }
            response.Write(sb.ToString());
        }

        #endregion

        #region 搜狗

        /// <summary>
        /// 搜狗合作数据接口
        /// </summary>
        private void SouGouGetCarPriceRange()
        {
            string callback = request.QueryString["callback"];
            string key = string.Format("Cooperation_GetCarDataJson_SouGou_hot");
            // 1次从memcache取数据
            object objMemCache = MemCache.GetMemCacheByKey(key);
            if (objMemCache != null)
            {
                sb.Append((string)objMemCache);
            }
            else
            {
                List<EnumCollection.SerialSortForInterface>[] listPriceRange = base.GetAllSerialNewly30DayToPriceRangeList();
                if (listPriceRange != null && listPriceRange.Length >= 8)
                {
                    // 白底图
                    dicPicWhite = base.GetAllSerialPicURLWhiteBackground();
                    // 子品牌报价区间
                    dicCsPrice = base.GetAllCsPriceRange();
                    // 5-8万
                    GetSerialPriceRangeListToString(listPriceRange[1], 4);
                    // 8-12
                    GetSerialPriceRangeListToString(listPriceRange[2], 4);
                    // 12-18
                    GetSerialPriceRangeListToString(listPriceRange[3], 4);

                    sb.Insert(0, "[");
                    sb.Append("]");
                }
                // 加入memcache
                MemCache.SetMemCacheByKey(key, sb.ToString(), 60 * 60 * 1000);
            }
            if (!string.IsNullOrEmpty(callback))
            {
                sb.Insert(0, callback + "(");
                sb.Append(")");
            }
            response.Write(sb.ToString());
        }

        private void GetSerialPriceRangeListToString(List<EnumCollection.SerialSortForInterface> list, int top)
        {
            if (list != null && list.Count > 0)
            {
                int loop = 0;
                foreach (EnumCollection.SerialSortForInterface ssi in list)
                {
                    if (sb.Length > 0)
                    { sb.Append(","); }
                    sb.Append("{");
                    sb.AppendFormat("\"imgUrl\":\"{0}\",\"link\":\"{1}\",\"title\":\"{2}\" ,\"price\":\"{3}\""
                        , dicPicWhite.ContainsKey(ssi.CsID) ? HttpUtility.UrlPathEncode(CommonFunction.GetPicOtherSize(dicPicWhite[ssi.CsID].Replace("_2.", "_3."), 125)) : ""
                        , HttpUtility.UrlPathEncode("http://car.bitauto.com/" + ssi.CsAllSpell + "/?WT.mc_id=sg")
                        , HttpUtility.UrlPathEncode(ssi.CsShowName)
                        , HttpUtility.UrlPathEncode(dicCsPrice.ContainsKey(ssi.CsID) ? dicCsPrice[ssi.CsID] : "暂无"));
                    sb.Append("}");
                    loop++;
                    if (loop >= top)
                    { break; }
                }
            }
        }

        #endregion

        #region dept 360

        //获取简化版主品牌数据
        private void CoopGetMasterData()
        {
            string callback = request.QueryString["callback"];
            Car_SerialBll carBll = new Car_SerialBll();
            DataSet ds = carBll.GetIsConditionsDataSet(5);
            DataTable dtMaster = ds.Tables[0].DefaultView.ToTable(true, "bs_id", "bs_Name", "bsallspell", "bsspell");
            sb.Append("{\"master\":[");
            if (dtMaster.Rows.Count > 0)
            {
                foreach (DataRow dr in dtMaster.Rows)
                {
                    sb.Append("{");
                    sb.AppendFormat("\"bsid\":\"{0}\",\"bsname\":\"{1}\",\"bsspell\":\"{2}\"",
                        dr["bs_id"],
                        dr["bs_Name"],
                         dr["bsspell"]);
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]}");
            string result = string.IsNullOrEmpty(callback) ? sb.ToString() : callback + "(" + sb.ToString() + ")";
            response.Write(result);
        }
        //获取品牌、子品牌数据
        private void CoopGetBrandOrSerial()
        {
            string callback = request.QueryString["callback"];
            int id = ConvertHelper.GetInteger(request.QueryString["id"]);
            if (id > 0)
            {
                Car_SerialBll carBll = new Car_SerialBll();
                DataSet ds = carBll.GetIsConditionsDataSet(5);
                //品牌
                DataTable dtBrand = ds.Tables[0].DefaultView.ToTable(true, "cb_id", "cb_name", "cballspell", "bs_id", "bs_Name", "bsallspell", "bsspell");
                sb.Append("{\"brand\":[");
                if (dtBrand.Rows.Count > 0)
                {
                    DataView dvBrand = dtBrand.DefaultView;
                    dvBrand.RowFilter = "bs_id=" + id;
                    foreach (DataRowView drv in dvBrand)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"cbname\":\"{0}\",\"serial\":[", drv["cb_Name"]);
                        //子品牌
                        DataRow[] drSerialArr = ds.Tables[0].Select("cb_id=" + drv["cb_id"]);
                        foreach (DataRow drSerial in drSerialArr)
                        {
                            sb.Append("{");
                            sb.AppendFormat("\"csname\":\"{0}\",\"cslink\":\"{1}\"",
                                drSerial["cs_name"],
                                "http://car.bitauto.com/" + ConvertHelper.GetString(drSerial["csallspell"]).ToLower() + "/");
                            sb.Append("},");

                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("]},");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]}");
            }
            string result = string.IsNullOrEmpty(callback) ? sb.ToString() : callback + "(" + sb.ToString() + ")";
            response.Write(result);
        }
        //获取主品牌、品牌、子品牌数据
        private void CoopGetCarData()
        {
            string callback = request.QueryString["callback"];
            Car_SerialBll carBll = new Car_SerialBll();
            DataSet ds = carBll.GetIsConditionsDataSet(5);
            DataTable dtMaster = ds.Tables[0].DefaultView.ToTable(true, "bs_id", "bs_Name", "bsallspell", "bsspell");
            sb.Append("{\"master\":[");
            foreach (DataRow dr in dtMaster.Rows)
            {
                sb.Append("{");
                sb.AppendFormat("\"bsname\":\"{0}\",\"logo\":\"{1}\",\"bslink\":\"{2}\",\"bsspell\":\"{3}\",\"brand\":[",
                    dr["bs_Name"],
                    "http://img1.bitauto.com/bt/car/default/images/carimage/m_" + dr["bs_id"] + "_b.jpg",
                    "http://car.bitauto.com/" + dr["bsallspell"] + "/",
                    dr["bsspell"]);
                //品牌
                DataTable dtBrand = ds.Tables[0].DefaultView.ToTable(true, "cb_id", "cb_name", "cballspell", "bs_id", "bs_Name", "bsallspell", "bsspell");
                if (dtBrand.Rows.Count > 0)
                {
                    DataView dvBrand = dtBrand.DefaultView;
                    dvBrand.RowFilter = "bs_id=" + dr["bs_id"];
                    foreach (DataRowView drv in dvBrand)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"cbname\":\"{0}\",\"brandlink\":\"{1}\",\"serial\":[",
                            drv["cb_Name"],
                            "http://car.bitauto.com/" + drv["cballspell"] + "/");
                        //子品牌
                        DataRow[] drSerialArr = ds.Tables[0].Select("cb_id=" + drv["cb_id"]);
                        foreach (DataRow drSerial in drSerialArr)
                        {
                            sb.Append("{");
                            sb.AppendFormat("\"csname\":\"{0}\",\"csshowname\":\"{1}\",\"cslink\":\"{2}\",\"cssalestate\":\"{3}\",\"baojia\":\"{4}\",\"photourl\":\"{5}\",\"bbsurl\":\"{6}\"",
                                drSerial["cs_name"],
                                ConvertHelper.GetString(drSerial["cs_ShowName"]).Trim(),
                                "http://car.bitauto.com/" + ConvertHelper.GetString(drSerial["csallspell"]).ToLower() + "/",
                                ConvertHelper.GetString(drSerial["csSaleState"]).Trim(),
                                "http://car.bitauto.com/" + drSerial["csallspell"] + "/baojia/",
                                "http://photo.bitauto.com/serial/" + drSerial["cs_id"] + "/",
                                //"http://car.bitauto.com/" + drSerial["csallspell"] + "/tupian/?WT.mc_id=360ny",
                                carBll.GetForumUrlBySerialId(ConvertHelper.GetInteger(drSerial["cs_id"])));
                            sb.Append("},");

                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("]},");
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]}");
            string result = string.IsNullOrEmpty(callback) ? sb.ToString() : callback + "(" + sb.ToString() + ")";
            response.Write(result);
        }
        //车型关注排行榜,按价位
        private void CoopGetCarPriceRange()
        {
            string callback = request.QueryString["callback"];
            int num = 24;
            string cachePricePVOrderKey = "cooperation_cachepricepvorderkey";
            try
            {

                Car_SerialBll serialBll = new Car_SerialBll();
                List<XmlElement> list = serialBll.GetHotSerial(num); //获取热门车型
                sb.Append("{\"hotcar\":[");
                foreach (XmlElement serialNode in list)
                {
                    int serialId = ConvertHelper.GetInteger(serialNode.GetAttribute("ID"));
                    string serialName = serialNode.GetAttribute("ShowName");
                    string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();
                    string priceRange = base.GetSerialPriceRangeByID(serialId);
                    sb.Append("{");
                    //sb.AppendFormat("\"id\":\"{0}\",", serialId);
                    sb.AppendFormat("\"name\":\"{0}\",", serialName);
                    sb.AppendFormat("\"spell\":\"{0}\",", serialSpell);
                    sb.AppendFormat("\"range\":\"{0}\",", priceRange);
                    sb.AppendFormat("\"imgurl\":\"{0}\",", "");
                    sb.AppendFormat("\"bbsurl\":\"{0}\"", serialBll.GetForumUrlBySerialId(serialId));
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("],");
                //各价格区间 子品牌信息
                // modified by chengl Jun.24.2013
                List<EnumCollection.SerialSortForInterface>[] listPriceRange = base.GetAllSerialNewly30DayToPriceRangeList();
                //List<EnumCollection.SerialSortForInterface>[] listPriceRange = (List<EnumCollection.SerialSortForInterface>[])CacheManager.GetCachedData(cachePricePVOrderKey);
                //if (listPriceRange == null)
                //{
                //    //获取价格区间子品牌信息
                //    List<EnumCollection.SerialSortForInterface> listSerial = base.GetAllSerialNewly30DayToList();
                //    listPriceRange = new List<EnumCollection.SerialSortForInterface>[8];
                //    for (int i = 0; i < 8; i++)
                //    {
                //        listPriceRange[i] = new List<EnumCollection.SerialSortForInterface>();
                //    }
                //    foreach (EnumCollection.SerialSortForInterface ssfi in listSerial)
                //    {
                //        if (listPriceRange[0].Count < num && ssfi.CsPriceRange.IndexOf(",1,") >= 0)
                //        { listPriceRange[0].Add(ssfi); }
                //        if (listPriceRange[1].Count < num && ssfi.CsPriceRange.IndexOf(",2,") >= 0)
                //        { listPriceRange[1].Add(ssfi); }
                //        if (listPriceRange[2].Count < num && ssfi.CsPriceRange.IndexOf(",3,") >= 0)
                //        { listPriceRange[2].Add(ssfi); }
                //        if (listPriceRange[3].Count < num && ssfi.CsPriceRange.IndexOf(",4,") >= 0)
                //        { listPriceRange[3].Add(ssfi); }
                //        if (listPriceRange[4].Count < num && ssfi.CsPriceRange.IndexOf(",5,") >= 0)
                //        { listPriceRange[4].Add(ssfi); }
                //        if (listPriceRange[5].Count < num && ssfi.CsPriceRange.IndexOf(",6,") >= 0)
                //        { listPriceRange[5].Add(ssfi); }
                //        if (listPriceRange[6].Count < num && ssfi.CsPriceRange.IndexOf(",7,") >= 0)
                //        { listPriceRange[6].Add(ssfi); }
                //        if (listPriceRange[7].Count < num && ssfi.CsPriceRange.IndexOf(",8,") >= 0)
                //        { listPriceRange[7].Add(ssfi); }
                //    }
                //    CacheManager.InsertCache(cachePricePVOrderKey, listPriceRange, WebConfig.CachedDuration);
                //}
                string priceName = "";
                string coopPriceRange = "";
                for (int i = 0; i < listPriceRange.Length; i++)
                {
                    GetPriceRangeName(i, out priceName, out coopPriceRange);
                    sb.AppendFormat("\"{0}\":[", priceName);
                    for (int j = 0; j < num; j++)
                    {
                        if (j + 1 > listPriceRange[i].Count) break;
                        sb.Append("{");
                        sb.AppendFormat("\"name\":\"{0}\",", listPriceRange[i][j].CsShowName);
                        sb.AppendFormat("\"spell\":\"{0}\",", listPriceRange[i][j].CsAllSpell.ToLower());
                        sb.AppendFormat("\"range\":\"{0}\",", base.GetSerialPriceRangeByID(listPriceRange[i][j].CsID));
                        sb.AppendFormat("\"imgurl\":\"{0}\",", "");
                        sb.AppendFormat("\"bbsurl\":\"{0}\"", serialBll.GetForumUrlBySerialId(listPriceRange[i][j].CsID));
                        sb.Append("},");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("],");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("}");
            }
            catch (Exception ex)
            {
                sb.Remove(0, sb.Length);
                sb.Append("{\"msg\":\"异常:" + ex.Message + "\"}");
            }
            string result = string.IsNullOrEmpty(callback) ? sb.ToString() : callback + "(" + sb.ToString() + ")";
            response.Write(result);
        }

        private void GetPriceRangeName(int index, out string name, out string price)
        {
            switch (index)
            {
                case 0: name = "5万以下"; price = "0-5"; break;
                case 1: name = "5-8万"; price = "5-8"; break;
                case 2: name = "8-12万"; price = "8-12"; break;
                case 3: name = "12-18万"; price = "12-18"; break;
                case 4: name = "18-25万"; price = "18-25"; break;
                case 5: name = "25-40万"; price = "25-40"; break;
                case 6: name = "40-80万"; price = "40-80"; break;
                case 7: name = "80万以上"; price = "80-9999"; break;
                default: name = ""; price = ""; break;
            }
        }

        #endregion

        #region detp pagechoice 魔图

        /// <summary>
        /// 魔图的json数据
        /// </summary>
        private void GetCarInfoForPageChoice()
        {
            // 此 request.QueryString["car_id"] 为子品牌ID，参数名不能改
            int csid = 0;
            if (!string.IsNullOrEmpty(request.QueryString["car_id"]))
            {
                if (int.TryParse(request.QueryString["car_id"].ToString(), out csid))
                { }
            }

            string jsoncallback = "";
            if (!string.IsNullOrEmpty(request.QueryString["jsoncallback"]))
            {
                jsoncallback = request.QueryString["jsoncallback"].ToString().Trim();
            }

            if (csid > 0)
            {
                // 根据车型ID取车型信息
                List<string> listJson = new List<string>();
                string key = string.Format(coopPageChoiceCacheTemp, csid);
                // 1次从memcache取数据
                object objMemCache = MemCache.GetMemCacheByKey(key);
                if (objMemCache != null)
                {
                    listJson = (List<string>)objMemCache;
                }
                else
                {
                    listJson.Add("\"car_id\":" + csid);
                    SerialEntity ce = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csid);
                    if (ce != null && ce.Id > 0)
                    {
                        Dictionary<int, string> dicMarkDay = new Car_SerialBll().GetAllSerialMarkDay();
                        string md = dicMarkDay.ContainsKey(ce.Id) ? dicMarkDay[ce.Id] : "";
                        // 有效子品牌
                        listJson.Add(",\"name\":\"" + CommonFunction.GetUnicodeByString(ce.Name) + "\"");
                        listJson.Add(",\"price\":\"" + CommonFunction.GetUnicodeByString(ce.Price) + "\"");
                        listJson.Add(",\"factory\":\"" + CommonFunction.GetUnicodeByString(ce.Brand.Producer != null ? ce.Brand.Producer.ShortName : "") + "\"");
                        listJson.Add(",\"type\":\"" + CommonFunction.GetUnicodeByString(ce.Level.Name) + "\"");
                        listJson.Add(",\"emissions\":\"" + CommonFunction.GetUnicodeByString(ce.Exhaust) + "\"");
                        listJson.Add(",\"marketTime\":\"" + CommonFunction.GetUnicodeByString(md) + "\"");
                        listJson.Add(",\"website\":\"" + CommonFunction.GetUnicodeByString(ce.OfficialSite) + "\"");
                    }
                    else
                    {
                        // 无效数据
                        listJson.Add(",\"name\":\"\",\"price\":\"\",\"factory\":\"\",\"type\":\"\",\"emissions\":\"\",\"marketTime\":\"\",\"website\":\"\"");
                    }
                    // 加入memcache
                    MemCache.SetMemCacheByKey(key, listJson, 60 * 60 * 1000);
                }
                listJson.Insert(0, jsoncallback + "({");
                listJson.Add("});");
                response.Write(string.Concat(listJson.ToArray()));
            }
        }

        #endregion

        //加载XML
        private List<string> LoadCoopNameXml()
        {
            List<string> coopNameList = new List<string>();
            string physicsPath = System.Web.HttpContext.Current.Server.MapPath(@"~/config/CooperationConfig.xml");
            XmlDocument xmlDoc = null;
            string cacheName = "BITAUTO_Cooperation_Coop_Name_List";
            object objCache = CacheManager.GetCachedData(cacheName);
            try
            {
                if (objCache != null)
                {
                    coopNameList = (List<string>)objCache;
                }
                else
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.Load(physicsPath);
                    XmlElement root = xmlDoc.DocumentElement;

                    XmlNodeList cNameList = root.SelectNodes(@"//Cooperation");
                    foreach (XmlNode node in cNameList)
                    {
                        if (node.Attributes["Name"] != null)
                        {
                            string coopNameNode = node.Attributes["Name"].Value.ToLower();
                            if (coopNameNode != "" && !coopNameList.Contains(coopNameNode))
                            { coopNameList.Add(coopNameNode); }
                        }
                    }
                    CacheManager.InsertCache(cacheName, coopNameList, new System.Web.Caching.CacheDependency(physicsPath)
    , DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return coopNameList;
        }

        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}