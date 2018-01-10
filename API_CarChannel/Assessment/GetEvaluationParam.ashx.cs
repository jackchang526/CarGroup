﻿using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BitAuto.CarChannelAPI.Web.Assessment
{
    /// <summary>
    /// GetEvaluationParam 的摘要说明
    /// </summary>
    public class GetEvaluationParam : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            string result = "{}";
            string callback = context.Request.QueryString["callback"];
            int evaluationId = ConvertHelper.GetInteger(context.Request.QueryString["evaluationId"]);
            int carId = ConvertHelper.GetInteger(context.Request.QueryString["carId"]);
            if (evaluationId <= 0 || carId <= 0)
            {
                context.Response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, result) : result);
                return;
            }
            string sql = @"SELECT     [259], [260],[261], [262],[263], [264],[265], [266],[267], [268],[269], [270],[271], [272],
                                      [273], [274],[275], [276],[277], [278],[279], [280],[281], [282],
        [181], [182], [183], [184], [185], [186], [175], [176], [177], [178],
        [179], [180], [169], [170], [171], [172], [173], [174], [166], [167],
        [168], [11], [14], [1], [2], [3], [4], [57], [58], [59], [60], [61],
        [62], [15], [16], [17], [18], [205], [206], [207], [208], [209], [210],
        [202], [203], [63], [64], [65], [66], [67], [68], [69], [70], [71],
        [72], [73], [74], [144], [124], [126], [128], [130], [132], [143],
        [39], [40], [41], [42], [43], [44], [45], [46], [47], [48], [49], [50],
        [51], [52], [53], [54], [55], [56], [22], [21], [23], [27], [28], [29],
        [85], [213], [214], [215], [216], [217], [257],[258],[255],[254],[80]
 FROM   ( SELECT    [PropertyId], [PropertyValue]
          FROM      [dbo].[StylePropertyValue]
          WHERE     EvaluationId = @evaluationId
          UNION ALL
          SELECT    PropertyId, PropertyValue
          FROM      dbo.StyleStandardPropertyValue
          WHERE     StyleId = @carId
        ) AS T1 PIVOT( MAX([PropertyValue]) FOR [PropertyId] IN (  
                                                                [259], [260],[261], [262],[263], [264],[265], [266],[267], [268],[269], [270],[271], [272],
                                                              [273], [274],[275], [276],[277], [278],[279], [280],[281], [282],
                                                              [181], [182],[183], [184],[185], [186],[175], [176],[177], [178],[179], [180],[169], [170],[171], [172],
                                                              [173], [174],[166], [167],[168], [11],[14], [1], [2],[3], [4], [57],[58], [59], [60],[61], [62], [15],[16], [17], [18],
                                                              [205], [206],[207], [208],[209], [210],[202], [203],[63], [64], [65],[66], [67], [68],[69], [70], [71],[72], [73], [74],
                                                              [144], [124],[126], [128],[130], [132],[143], [39],[40], [41], [42],[43], [44], [45],[46], [47], [48],[49], [50], [51],
                                                              [52], [53], [54],[55], [56], [22],[21], [23], [27],[28], [29], [85],[213], [214],[215], [216],
                                                              [217], [257],[258],[255],[254],[80] ) ) AS T2";
            SqlParameter[] _params = {
                                         new SqlParameter("@evaluationId",SqlDbType.Int),
                                         new SqlParameter("@carId",SqlDbType.Int)
                                     };
            _params[0].Value = evaluationId;
            _params[1].Value = carId;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarsEvaluationDataConnectionString, CommandType.Text, sql, _params);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                var espOff = (dr["22"] != null && dr["22"].ToString() != "")?Math.Round(ConvertHelper.GetDouble(dr["22"]),1): dr["22"];
                var espOn = (dr["27"] != null && dr["27"].ToString() != "") ? Math.Round(ConvertHelper.GetDouble(dr["27"]), 1) : dr["27"];                
                var obj = new
                {
                    EvaluationId = evaluationId,
                    //空气质量参数组
                    BenGj = dr["259"],  //笨（国际结果）
                    BenSw = dr["260"],//笨（Iso升温模式结果）
                    BenQd = dr["261"],//笨（Iso启动模式结果）
                    JiaBenGj = dr["262"],
                    JiaBenSw = dr["263"],
                    JiaBenQd = dr["264"],
                    YiBenGj = dr["265"],
                    YiBenSw = dr["266"],
                    YiBenQd = dr["267"],
                    ErJiaBenGj = dr["268"],
                    ErJiaBenSw = dr["269"],
                    ErJiaBenQd = dr["270"],
                    BenYiXiGj = dr["271"],
                    BenYiXiSw = dr["272"],
                    BenYiXiQd = dr["273"],
                    JiaQuanGj = dr["274"],
                    JiaQuanSw = dr["275"],
                    JiaQuanQd = dr["276"],
                    YiQuanGj = dr["277"],
                    YiQuanSw = dr["278"],
                    YiQuanQd = dr["279"],
                    BinXiQuanGj = dr["280"],
                    BinXiQuanSw = dr["281"],
                    BinXiQuanQd = dr["282"],

                    TuiQianXiao = dr["181"],
                    TuiQianDa = dr["182"],
                    TuiZhongXiao = dr["183"],
                    TuiZhongDa = dr["184"],
                    TuiHouXiao = dr["185"],
                    TuiHouDa = dr["186"],

                    SeatQianXiao = dr["175"],
                    SeatQianDa = dr["176"],
                    SeatZhongXiao = dr["177"],
                    SeatZhongDa = dr["178"],
                    SeatHouXiao = dr["179"],
                    SeatHouDa = dr["180"],

                    HightQianXiao = dr["169"],
                    HightQianDa = dr["170"],
                    HightZhongXiao = dr["171"],
                    HightZhongDa = dr["172"],
                    HightHouXiao = dr["173"],
                    HightHouDa = dr["174"],

                    WidthQian = dr["166"],
                    WidthZhong = dr["167"],
                    WidthHou = dr["168"],

                    ShacheKong = dr["11"],
                    ShacheMan = dr["14"],


                    LengShaChe100_1 = dr["1"],
                    LengShaChe100_2 = dr["2"],
                    LengShaChe100_3 = dr["3"],
                    LengShaChe100_4 = dr["4"],

                    GongLiCha57 = dr["57"],
                    GongLiCha58 = dr["58"],
                    GongLiCha59 = dr["59"],
                    GongLiCha60 = dr["60"],
                    GongLiCha61 = dr["61"],
                    GongLiCha62 = dr["62"],

                    ShaChePanWenDu_zq = dr["15"],
                    ShaChePanWenDu_yq = dr["16"],
                    ShaChePanWenDu_zh = dr["17"],
                    ShaChePanWenDu_yh = dr["18"],


                    Azuo = dr["205"],
                    Ayou = dr["206"],
                    Bzuo = dr["207"],
                    Byou = dr["208"],
                    Czuo = dr["209"],
                    Cyou = dr["210"],
                    QianKeShiFanWei = dr["202"],
                    HouKeShiFanWei = dr["203"],

                    TingZhi = dr["63"],
                    Qian50 = dr["64"],
                    Qian80 = dr["65"],
                    Qian100 = dr["66"],
                    Qian120 = dr["67"],
                    Qian130 = dr["68"],
                    Qian140 = dr["69"],
                    Qian160 = dr["70"],
                    Hou50 = dr["71"],
                    Hou80 = dr["72"],
                    Hou100 = dr["73"],
                    Hou120 = dr["74"],

                    JiaSu400mKm = dr["144"],



                    JiaSu0_20 = dr["124"],
                    JiaSu0_40 = dr["126"],
                    JiaSu0_60 = dr["128"],
                    JiaSu0_80 = dr["130"],
                    JiaSu0_100 = dr["132"],
                    JiaSu400m = dr["143"],

                    Dang1Gaosu = dr["39"],
                    Dang1Zhuansu = dr["40"],
                    Dang2Gaosu = dr["41"],
                    Dang2Zhuansu = dr["42"],
                    Dang3Gaosu = dr["43"],
                    Dang3Zhuansu = dr["44"],
                    Dang4Gaosu = dr["45"],
                    Dang4Zhuansu = dr["46"],
                    Dang5Gaosu = dr["47"],
                    Dang5Zhuansu = dr["48"],
                    Dang6Gaosu = dr["49"],
                    Dang6Zhuansu = dr["50"],
                    Dang7Gaosu = dr["51"],
                    Dang7Zhuansu = dr["52"],
                    Dang8Gaosu = dr["53"],
                    Dang8Zhuansu = dr["54"],
                    Dang9Gaosu = dr["55"],
                    Dang9Zhuansu = dr["56"],


                    EspOff = espOff,
                    EspOn1 = dr["21"],
                    ZuiDaG = dr["23"],
                    EspOn = espOn,
                    EspOff1 = dr["28"],
                    ZuiDaG1 = dr["29"],

                    PuSiSaiDao = dr["85"],

                    ZhuanWanZhiJingZuo = dr["213"],
                    ZhuanWanZhiJingYou = dr["214"],

                    FangXiangPanQuanShu = dr["215"],
                    FangXiangPanZhiJingZhong = dr["216"],
                    FangXiangPanZhiJingWai = dr["217"],

                    DzhuZuoZheBiJiaoDu = dr["257"],
                    DzhuYouZheBiJiaoDu = dr["258"],
                    BaoYangFeiYong6Nian = dr["255"],
                    KongTiaoChuFengKouShuLiang = dr["254"],
                    ShiJiCeShiYouHao = dr["80"],
                    LevelRankInfo=GetRankInfo(carId)
                };
                result = JsonConvert.SerializeObject(obj);
            }
            context.Response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, result) : result);
        }

        private object GetRankInfo(int carId)
        {
            List<int> propertyIds = GetPropertyIdList();
            Dictionary<string, RankInfo> dic = new Dictionary<string, RankInfo>();
            foreach (int propertyId in propertyIds)
            {
                string xmlFile = Path.Combine(WebConfig.DataBlockPath, string.Format(@"Data\EvaluationRank\Rank_{0}\{1}.xml", propertyId, carId));                
                try
                {                    
                    if (File.Exists(xmlFile))
                    {
                        RankInfo rankInfo = new RankInfo();
                        XDocument doc = XDocument.Load(xmlFile);
                        var query = from p in doc.Element("Root").Element("EvaluationRankList").Elements("EvaluationRank") select p;
                        XElement ele = query.ToList().First();
                        rankInfo.LevelBestName = ele.Element("ModelName").Value;
                        rankInfo.LevelBestValue = ele.Element("PropertyValue").Value;
                        rankInfo.LevelBestUnit = ele.Element("Unit").Value;
                        rankInfo.LevelTotal = ConvertHelper.GetInteger(doc.Element("Root").Element("LevelTotal").Value);
                        rankInfo.LevelNum = ConvertHelper.GetInteger(doc.Element("Root").Element("LevelNum").Value);
                        rankInfo.LevelName = doc.Element("Root").Element("LevelName").Value;
                        rankInfo.Beat = ConvertHelper.GetDouble((rankInfo.LevelTotal - rankInfo.LevelNum) * 100 / rankInfo.LevelTotal);
                        rankInfo.LevelAvg = doc.Element("Root").Element("LevelAvg").Value;
                        dic.Add("p_" + propertyId, rankInfo);
                    }
                }
                catch (Exception ex)
                {
                    CommonFunction.WriteLog(ex.ToString());
                }
            }
            return dic;
        }      

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public List<int> GetPropertyIdList()
        {
            string xmlFile = System.Web.HttpContext.Current.Server.MapPath(@"~/config/PropertyId.xml");

            string key = "PropertyId_Xml";

            List<int> list = new List<int>();

            var obj = CacheManager.GetCachedData(key);

            try
            {
                if (obj != null)
                {
                    list = (List<int>)obj;
                }
                else
                {
                    if (File.Exists(xmlFile))
                    {
                        XDocument doc = XDocument.Load(xmlFile);
                        var query = from p in doc.Element("Root").Elements("Item") select p;
                        query.ToList().ForEach(item =>
                        {
                            int propertyId = ConvertHelper.GetInteger(item.Element("PropertyId").Value);
                            if (!list.Contains(propertyId))
                            {
                                list.Add(propertyId);
                            }                            
                        });
                        CacheManager.InsertCache(key, list, WebConfig.CachedDuration);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return list;
        }
    }
}