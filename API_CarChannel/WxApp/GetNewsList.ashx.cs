using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;

namespace BitAuto.CarChannelAPI.Web.WxApp
{
    /// <summary>
    /// GetNewsList 的摘要说明
    /// </summary>
    public class GetNewsList : IHttpHandler
    {
        //访问格式：http://api.car.bitauto.com/WxApp/GetNewsList.ashx?sid=1765&i=1&size=10

        private int _pageIndex;
        private int _pageSize;
        private int _serialId;
        private int _carNewsTypeInt;
        private int _newsCount;
        private HttpResponse response;

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.CarChannel.Common.Cache.CacheManager.SetPageCache(30);
            context.Response.ContentType = "application/x-javascript";
            response = context.Response;
            GetParam(context.Request);

            if (_serialId < 1 || _carNewsTypeInt < 0 )
                return;
            GetJosn();
        }

        private void GetJosn()
        {
            string result = string.Empty;
            try
            {
                string cacheKey = "Car_WxApp_News_" + _serialId + "_" + _pageIndex + "_" + _pageSize;
                var obj = CacheManager.GetCachedData(cacheKey);
                if (obj != null)
                {
                    result = (string)obj;

                }

                DataSet ds = null;
                List<int> carTypeIdList = new List<int>() 
				{ 
                (int)CarNewsType.serialfocus,
				(int)CarNewsType.shijia,
				(int)CarNewsType.daogou,
				(int)CarNewsType.yongche,
				(int)CarNewsType.gaizhuang,
				(int)CarNewsType.anquan,
				(int)CarNewsType.xinwen,
                (int)CarNewsType.pingce
				};
                ds = new CarNewsBll().GetSerialNewsAllData(_serialId, carTypeIdList, _pageSize, _pageIndex, ref _newsCount);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                { result = string.Empty; }
                StringBuilder json = new StringBuilder();
                DataRowCollection rows = ds.Tables[0].Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i];
                    int newsId = ConvertHelper.GetInteger(row["cmsnewsid"]);
                    string newsUrl = Convert.ToString(row["filepath"]).Replace("news.bitauto.com", "news.m.yiche.com");
                    string firstPicUrl = string.Empty;
                    firstPicUrl = ConvertHelper.GetString(row["FirstPicUrl"]);

                    //if (newsType == "pingce")
                    //    newsUrl = "/" + _serialAllSpell + "/pingce/p" + newsId + "/";
                    DateTime publishTime = Convert.ToDateTime(row["publishtime"]);
                    string author = Convert.ToString(row["author"]);
                    int commentNum = row["CommentNum"] == DBNull.Value ? 0 : Convert.ToInt32(row["CommentNum"]);
                    string image = ConvertHelper.GetString(row["Picture"]);
                    if (string.IsNullOrEmpty(image))
                    {
                        if (!string.IsNullOrEmpty(firstPicUrl))
                        {
                            firstPicUrl = firstPicUrl.Replace("/bitauto/", "/newsimg_180_w0_1/bitauto/")
                                .Replace("/autoalbum/", "/newsimg_180_w0_1/autoalbum/");
                            if (firstPicUrl.IndexOf(".bitauto") != -1)
                            {
                                image = firstPicUrl;
                            }
                        }
                    }
                    else
                    {
                        image = image.Replace("/bitauto/", "/newsimg_180_w0_1/bitauto/")
                               .Replace("/autoalbum/", "/newsimg_180_w0_1/autoalbum/");
                    }
                    json.Append("{");
                    json.AppendFormat("\"id\":{0},\"tag\":\"{1}\",\"d\":\"{2}\",\"aut\":\"{3}\",\"url\":\"{4}\",\"com\":\"{5}\",\"image\":\"{6}\""
                        , row["CmsNewsId"].ToString()
                        , CommonFunction.NewsTitleDecode(row["title"].ToString())
                        , publishTime.ToString("yyyy-MM-dd")
                        , StrCut(author, 6)
                        , newsUrl
                        , commentNum.ToString()
                        , image
                        );
                    json.Append("}");
                    if (i < rows.Count - 1)
                        json.Append(",");
                }
                var strNewsJson = json.ToString();
                result = "{\"NewsList\":[" + strNewsJson + "],\"NewsCnt\":" + _newsCount + ",\"PageCnt\":" + CommonFunction.GetTotalPageNumber(_newsCount, _pageSize) + "}";
                response.Write(result);
                CacheManager.InsertCache(cacheKey, result, WebConfig.CachedDuration);
                
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString() + ";StackTrace:" + ex.StackTrace);
            }
        }

        private void GetParam(HttpRequest request)
        {
            _serialId = ConvertHelper.GetInteger(request.QueryString["sid"]);
            if (_serialId < 0)
                return;
            
            _pageIndex = ConvertHelper.GetInteger(request.QueryString["i"]);
            if (_pageIndex < 1)
                _pageIndex = 1;
            _pageSize = ConvertHelper.GetInteger(request.QueryString["size"]);
            if (_pageSize < 1)
                _pageSize = 10;
        }

        /// <summary>
        /// 截取指定长度字符串(按字节算)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected string StrCut(string str, int length)
        {
            int len = 0;
            byte[] b;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                b = Encoding.Default.GetBytes(str.Substring(i, 1));
                if (b.Length > 1)
                    len += 2;
                else
                    len++;

                if (len > length)
                {
                    sb.Append("...");
                    break;
                }

                sb.Append(str[i]);
            }

            return sb.ToString();
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