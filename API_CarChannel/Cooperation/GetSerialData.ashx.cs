using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannelAPI.Web.Cooperation
{
    /// <summary>
    /// serialrankbylevel: 按级别获取热门车型 by 2345 2017-03-23
	/// serialsparklebyid: 车系亮点 by chengl 2017-05-11
	/// serialnewcarbyyear: 新车上市车系 for baidu by sk 2017-08-29 
    /// </summary>
    public class GetSerialData : PageBase, IHttpHandler
    {
        HttpRequest request;
        HttpResponse response;
        public void ProcessRequest(HttpContext context)
        {
            request = context.Request;
            response = context.Response;
            string action = request.QueryString["act"];
            switch (action)
            {
                case "serialrankbylevel": CacheManager.SetPageCache(60); RanderSerialRank(); break;
                case "serialsparklebyid": CacheManager.SetPageCache(60); RanderSerialSparkleByCsID(); break;
                case "serialnewcarbyyear": CacheManager.SetPageCache(60); RanderNewCarByYear(); break;
            }
        }

        private void RanderNewCarByYear()
        {
            response.ContentType = "application/json";
            List<string> jsonString = new List<string>();
            #region 取查询年款
            int year = DateTime.Now.Year;
            string strYear = request.QueryString["year"];
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
                string cacheKey = "GetSerialData_RanderNewCarByYear_" + year.ToString();
                object obj = CacheManager.GetCachedData(cacheKey);
                if (obj != null)
                {
                    jsonString.Add(obj.ToString());
                }
                else
                {
                    #region 查询数据

                    string sql = @"SELECT  mb.bs_Name, mb.bs_Country, cb.cb_Name, cb.cb_country, cs.cs_id,
        cs.csname, cs.csshowname, cl.classvalue AS carLevel, cs.allspell,
        car.car_id, car.car_name, car.car_ReferPrice, cdb1.pvalue AS pyear,
        cdb2.pvalue AS pmonth, cdb3.pvalue AS pday
FROM    car_relation car WITH ( NOLOCK )
        LEFT JOIN car_serial cs WITH ( NOLOCK ) ON car.cs_id = cs.cs_id
        LEFT JOIN dbo.Car_Brand cb WITH ( NOLOCK ) ON cb.cb_Id = cs.cb_Id
        LEFT JOIN dbo.Car_MasterBrand_Rel cmr WITH ( NOLOCK ) ON cmr.cb_Id = cb.cb_Id
        LEFT JOIN dbo.Car_MasterBrand mb WITH ( NOLOCK ) ON mb.bs_Id = cmr.bs_Id
        LEFT JOIN class cl WITH ( NOLOCK ) ON cs.carlevel = cl.classid
        LEFT JOIN cardatabase cdb1 WITH ( NOLOCK ) ON car.car_id = cdb1.carid
                                                      AND cdb1.paramid = 385
        LEFT JOIN cardatabase cdb2 WITH ( NOLOCK ) ON car.car_id = cdb2.carid
                                                      AND cdb2.paramid = 384
        LEFT JOIN cardatabase cdb3 WITH ( NOLOCK ) ON car.car_id = cdb3.carid
                                                      AND cdb3.paramid = 383
WHERE   car.isstate = 0
        AND cs.isstate = 0
        AND cdb1.pvalue = @CarYear
        AND ISNUMERIC(cdb2.pvalue) >= 1
        AND ISNUMERIC(cdb3.pvalue) >= 1
ORDER BY CONVERT(INT, cdb2.pvalue) DESC, CONVERT(INT, cdb3.pvalue) DESC,
        cs.cs_id, car.car_ReferPrice";

                    SqlParameter[] _params = { new SqlParameter("@CarYear", SqlDbType.VarChar) };
                    _params[0].Value = year;
                    DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
                        , CommandType.Text, sql, _params);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        // 白底
                        Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();
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
                            int pyear = int.Parse(dr["pyear"].ToString());
                            int pmonth = int.Parse(dr["pmonth"].ToString());
                            int pday = int.Parse(dr["pday"].ToString());
                            string masterBrandName = dr["bs_Name"].ToString().Trim();
                            string brandName = dr["cb_Name"].ToString().Trim();
                            int masterBrandCountry = ConvertHelper.GetInteger(dr["bs_Country"]);
                            int brandCountry = ConvertHelper.GetInteger(dr["cb_country"]);
                            string producerType = string.Empty;
                            if (brandCountry == 90 && masterBrandCountry == 90)
                            {
                                producerType = "自主";
                            }
                            if (brandCountry == 90 && masterBrandCountry != 90)
                            {
                                producerType = "合资";
                            }
                            if (brandCountry != 90)
                            {
                                producerType = "进口";
                            }
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
                                //无封面图过滤掉
                                if (!dicPicWhite.ContainsKey(csid)) continue;

                                // 如果换子品牌或者 换上市时间 则将之前的数据补齐价格区间
                                if (lastCsID > 0)
                                {
                                    // 已有数据 补价格区间
                                    jsonString.Add(string.Format(",\"Price\":\"{0}\""
                                        ,base.GetSerialPriceRangeByID(lastCsID) //CommonFunction.GetUnicodeByString(GetPriceStr(minP, maxP))
                                        ));
                                    jsonString.Add("},");
                                    minP = 0;
                                    maxP = 0;
                                }
                                jsonString.Add("{");
                                jsonString.Add(string.Format("\"ID\":\"{0}\",\"ShowName\":\"{1}\",\"MakeDay\":\"{2}\",\"Pic\":\"{3}\",\"CarLevel\":\"{4}\",\"MasterBrandName\":\"{5}\",\"BrandName\":\"{6}\",\"ProducerType\":\"{7}\",\"MUrl\":\"{8}\""
                                    , csid
                                    , CommonFunction.GetUnicodeByString(csShowName)
                                    , tempDt.ToString("yyyy-MM-dd")
                                    , dicPicWhite.ContainsKey(csid) ? dicPicWhite[csid].Replace("_2.", "_4.") : WebConfig.DefaultCarPic
                                    , csLevel
                                    , masterBrandName
                                    , brandName
                                    , producerType
                                    , string.Format("http://car.m.yiche.com/{0}/?WT.mc_id=mbdappcr_qt", csAllspell)
                                    ));

                                lastCsID = csid;
                                lastDT = tempDt;
                            }
                            //decimal referPrice = 0;
                            //if (decimal.TryParse(dr["car_ReferPrice"].ToString().Trim(), out referPrice))
                            //{
                            //    if (referPrice > maxP || maxP == 0)
                            //    { maxP = referPrice; }
                            //    if (referPrice < minP || minP == 0)
                            //    { minP = referPrice; }
                            //}
                        }
                        jsonString.Add(string.Format(",\"Price\":\"{0}\""
                            , base.GetSerialPriceRangeByID(lastCsID)//CommonFunction.GetUnicodeByString(GetPriceStr(minP, maxP))
                            ));
                        jsonString.Add("}");
                        jsonString.Add("]");
                    }
                    CacheManager.InsertCache(cacheKey, string.Join("", jsonString.ToArray()), 60);
                    #endregion
                }
            }
            response.Write(string.Join("", jsonString.ToArray()));
        }

        /// <summary>
        /// 车系亮点
        /// </summary>
        private void RanderSerialSparkleByCsID()
        {
            response.ContentType = "application/json";
            string json = "[]";
            int csid = ConvertHelper.GetInteger(request.QueryString["csid"]);
            if (csid > 0)
            {
                List<SerialSparkle> serialSparkleList = new SerialFourthStageBll().GetSerialSparkle(csid);
                List<object> temp = new List<object>();
                if (serialSparkleList != null && serialSparkleList.Count > 0)
                {
                    foreach (SerialSparkle ss in serialSparkleList)
                    {
                        temp.Add(new { id = ss.H5SId, name = ss.Name, url = ss.ImageUrl });
                    }
                }
                json = JsonConvert.SerializeObject(temp);
            }
            response.Write(json);
        }

        private void RanderSerialRank()
        {
            response.ContentType = "application/json";
            int topN = ConvertHelper.GetInteger(request.QueryString["top"]);
            topN = topN <= 0 ? 10 : topN;

            List<object> list = new List<object>();
            list.Add(new { name = "汽车关注排行榜", moreurl = "http://car.bitauto.com/?WT.mc_id=2345nyph", rank = GetHotSerialData(10) });

            var dict = GetHotSerialDataByLevel(10);
            var arr = new string[] {  "微型车", "紧凑型车", "中型车", "SUV",
                                             "MPV"};
            foreach (string ln in arr)
            {
                if (dict.ContainsKey(ln))
                {
                    string name = string.Empty;
                    string moreUrl = string.Empty;
                    if (ln == "微型车")
                    {
                        name = "微型车关注排行榜";
                        moreUrl = "http://car.bitauto.com/weixingche/?WT.mc_id=2345nyph";
                    }
                    else if (ln == "紧凑型车")
                    {
                        name = "紧凑型车关注排行榜";
                        moreUrl = "http://car.bitauto.com/jincouxingche/?WT.mc_id=2345nyph";
                    }
                    else if (ln == "中型车")
                    {
                        name = "中型车关注排行榜";
                        moreUrl = "http://car.bitauto.com/zhongxingche/?WT.mc_id=2345nyph";
                    }
                    else if (ln == "SUV")
                    {
                        name = "SUV关注排行榜";
                        moreUrl = "http://car.bitauto.com/suv/?WT.mc_id=2345nyph";
                    }
                    else if (ln == "MPV")
                    {
                        name = "MPV关注排行榜";
                        moreUrl = "http://car.bitauto.com/mpv/?WT.mc_id=2345nyph";
                    }
                    list.Add(new { name = name, moreurl = moreUrl, rank = dict[ln] });
                }
            }

            string json = JsonConvert.SerializeObject(new { rank = list });
            response.Write(json);
        }

        /// <summary>
        /// 热门车系
        /// </summary>
        /// <param name="topN">前N条</param>
        /// <returns></returns>
        public List<object> GetHotSerialData(int topN)
        {
            List<object> list = new List<object>();
            string sql = @"SELECT TOP (@topN)
                                    cs.cs_Id, cs_Name, cs.allSpell
                            FROM    [dbo].[Car_Serial] cs
                                    LEFT JOIN dbo.Car_Serial_30UV csuv ON cs.cs_Id = csuv.cs_id
                            WHERE   cs.IsState = 1
                            ORDER BY csuv.UVCount DESC";
            SqlParameter[] _param = { new SqlParameter("@topN", SqlDbType.Int) };
            _param[0].Value = topN;
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                , CommandType.Text, sql, _param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int csId = ConvertHelper.GetInteger(dr["cs_Id"]);
                    string allspell = ConvertHelper.GetString(dr["allSpell"]);
                    list.Add(new
                    {
                        name = ConvertHelper.GetString(dr["cs_Name"]),
                        nameurl = string.Format("http://car.bitauto.com/{0}/?WT.mc_id=2345nyph", ConvertHelper.GetString(dr["allSpell"])),
                        price = base.GetSerialPriceRangeByID(csId).Split('-')[0],
                        priceurl = string.Format("http://car.bitauto.com/{0}/baojia/?WT.mc_id=2345nyph", allspell),
                        minprice = string.Format("http://dealer.bitauto.com/zuidijia/nb{0}/?WT.mc_id=2345nyph", csId),
                        image = Car_SerialBll.GetSerialImageUrl(csId, "2")
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// 按指定级别获取热门车系
        /// </summary>
        /// <param name="topN">前N条</param>
        /// <returns></returns>
        public Dictionary<string, List<object>> GetHotSerialDataByLevel(int topN)
        {
            Dictionary<string, List<object>> dict = new Dictionary<string, List<object>>();
            string sql = @"WITH    result
                          AS(SELECT   cs.[cs_Id], [cs_Name], cs_CarLevel,cs.allSpell,
                                        ROW_NUMBER() OVER(PARTITION BY cs_CarLevel ORDER BY csuv.UVCount DESC)
                                        AS rownum
                               FROM[dbo].[Car_Serial] cs
                                        LEFT JOIN dbo.Car_Serial_30UV csuv ON cs.cs_Id = csuv.cs_id
                               WHERE    cs.IsState = 1
                                        AND cs_CarLevel IN('微型车', '紧凑型车', '中型车', 'SUV',
                                                             'MPV')
                             )
                    SELECT *
                    FROM    result
                    WHERE   rownum <= @topN";
            SqlParameter[] _param = { new SqlParameter("@topN", SqlDbType.Int) };
            _param[0].Value = topN;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                , CommandType.Text, sql, _param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int csId = ConvertHelper.GetInteger(dr["cs_Id"]);
                    string level = ConvertHelper.GetString(dr["cs_CarLevel"]);
                    string allspell = ConvertHelper.GetString(dr["allSpell"]);
                    if (dict.ContainsKey(level))
                    {
                        dict[level].Add(new
                        {
                            name = ConvertHelper.GetString(dr["cs_Name"]),
                            nameurl = string.Format("http://car.bitauto.com/{0}/?WT.mc_id=2345nyph", allspell),
                            price = base.GetSerialPriceRangeByID(csId).Split('-')[0],
                            priceurl = string.Format("http://car.bitauto.com/{0}/baojia/?WT.mc_id=2345nyph", allspell),
                            minprice = string.Format("http://dealer.bitauto.com/zuidijia/nb{0}/?WT.mc_id=2345nyph", csId),
                            image = Car_SerialBll.GetSerialImageUrl(csId, "2")
                        });
                    }
                    else
                    {
                        List<object> list = new List<object>();
                        list.Add(new
                        {
                            name = ConvertHelper.GetString(dr["cs_Name"]),
                            nameurl = string.Format("http://car.bitauto.com/{0}/?WT.mc_id=2345nyph", ConvertHelper.GetString(dr["allSpell"])),
                            price = base.GetSerialPriceRangeByID(csId).Split('-')[0],
                            priceurl = string.Format("http://car.bitauto.com/{0}/baojia/?WT.mc_id=2345nyph", allspell),
                            minprice = string.Format("http://dealer.bitauto.com/zuidijia/nb{0}/?WT.mc_id=2345nyph", csId),
                            image = Car_SerialBll.GetSerialImageUrl(csId, "2")
                        });
                        dict.Add(level, list);
                    }
                }
            }
            return dict;
        }

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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}