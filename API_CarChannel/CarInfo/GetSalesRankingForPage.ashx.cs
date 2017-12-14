using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// GetSalesRankingForPage 的摘要说明
    /// 获取车系销量排行
    /// </summary>
    public class GetSalesRankingForPage : PageBase, IHttpHandler
    {

        private int levelId = 0;
        private string levelName = string.Empty;
        private int pageIndex = 1;
        private int pageSize = 10;
        private string callback = string.Empty;
        private string item = "{{\"csId\":{0},\"showName\":\"{1}\",\"priceRange\":\"{2}\",\"sellNum\":\"{3}\",\"imgUrl\":\"{4}\",\"rank\":{5}}}{6}";
        public void ProcessRequest(HttpContext context)
        {
            base.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            GetParam(context);
            GetSellRank(context);
        }

        private void GetSellRank(HttpContext context)
        {
            if (levelId == 0 || string.IsNullOrEmpty(levelName))
            {
                if (string.IsNullOrEmpty(callback))
                {
                    context.Response.Write(ResultUtil.ErrorResult(-1, "参数错误", ""));
                }
                else
                {
                    context.Response.Write(ResultUtil.CallBackResult(callback, ResultUtil.ErrorResult(-1, "参数错误", "")));
                }
                return;
            }
            int allCcount = 0;
            int startIndex = (pageIndex - 1) * pageSize;
            string month = "";
            var carSerialBll = new Car_SerialBll();
            List<XmlElement> sellRankList = carSerialBll.GetSerialSellRankForPage(levelName, startIndex, pageSize, out allCcount);
            if (sellRankList == null || sellRankList.Count == 0)
            {
                if (string.IsNullOrEmpty(callback))
                {
                    context.Response.Write(ResultUtil.ErrorResult(0, "没有符合要求的数据", ""));
                }
                else
                {
                    context.Response.Write(ResultUtil.CallBackResult(callback, ResultUtil.ErrorResult(0, "没有符合要求的数据", "")));
                } 
                return;
            }
            StringBuilder sb = new StringBuilder();
            int endIndex = pageIndex * pageSize;
            if (endIndex > allCcount)
                endIndex = allCcount;
            int index = startIndex;
            month = carSerialBll.GetSeialSellRankMonth();
            foreach (XmlElement ele in sellRankList)
            {
                index++;
                sb.AppendFormat(item
                    , ele.GetAttribute("CsId")
                    , ele.GetAttribute("ShowName")
                    , ele.GetAttribute("PriceRange")
                    , ele.GetAttribute("SellNum")
                    , ele.GetAttribute("ImgUrl")
                    , index
                    , index == endIndex ? "" : ",");
            }
            if (string.IsNullOrEmpty(callback))
            { 
                context.Response.Write(ResultUtil.SuccessResult(ResultDateFormat(month, ResultUtil.PageFormat(pageIndex, pageSize, allCcount, string.Format("[{0}]", sb.ToString())))));
            }
            else
            {
                context.Response.Write(ResultUtil.CallBackResult(callback, ResultUtil.SuccessResult(ResultDateFormat(month, ResultUtil.PageFormat(pageIndex, pageSize, allCcount, string.Format("[{0}]", sb.ToString()))))));
            }
        }


        public static string ResultDateFormat(string month, string page)
        {
            return string.Format("{{\"month\":\"{0}\",\"page\":{1}}}", month, page);
        }

        private void GetParam(HttpContext context)
        {
            levelId = ConvertHelper.GetInteger(context.Request["level"]);
            levelName = CarLevelDefine.GetLevelNameById(this.levelId);
            this.callback = ConvertHelper.GetString(context.Request["callback"]);
            pageIndex = ConvertHelper.GetInteger(context.Request["pageindex"]);
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            pageSize = ConvertHelper.GetInteger(context.Request["pagesize"]);
            if (pageSize == 0)
            {
                pageSize = 10;
            }
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