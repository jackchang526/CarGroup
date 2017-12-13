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
        private string result = "{{\"list\":[{0}]}}";
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
                context.Response.Write(ResultUtil.ErrorResult(-1, "参数错误", ""));
                return;
            }
            int allCcount = 0;
            int startIndex = (pageIndex - 1) * pageSize;
            List<XmlElement> sellRankList = new Car_SerialBll().GetSerialSellRankForPage(levelName, startIndex, pageSize, out allCcount);
            if (sellRankList == null || sellRankList.Count == 0)
            {
                context.Response.Write(ResultUtil.ErrorResult(0, "没有符合要求的数据", ""));
                return;
            }
            StringBuilder sb = new StringBuilder();
            int endIndex = pageIndex * pageSize;
            if (endIndex > allCcount)
                endIndex = allCcount;
            int index = startIndex;
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
            context.Response.Write(ResultUtil.SuccessResult(ResultUtil.PageFormat(pageIndex, pageSize, allCcount, string.Format("[{0}]", sb.ToString()))));
        }

        private void GetParam(HttpContext context)
        {
            levelId = ConvertHelper.GetInteger(context.Request["level"]);
            levelName = CarLevelDefine.GetLevelNameById(this.levelId);
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