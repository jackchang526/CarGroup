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
            }
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
					foreach(SerialSparkle ss in serialSparkleList)
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}