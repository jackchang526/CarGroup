using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace BitAuto.CarChannelAPI.Web.WxApp
{
    /// <summary>
    /// GetSerialSellRank 的摘要说明
    /// </summary>
    public class GetSerialSellRank : PageBase,IHttpHandler
    {
        private int levelId = 0;
        private string levelName = string.Empty;
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
                context.Response.Write(string.Format(result, string.Empty));
                return;
            }
            List<XmlElement> sellRankList = new Car_SerialBll().GetSeialSellRank(levelName);
            if (sellRankList == null || sellRankList.Count == 0)
            {
                context.Response.Write(string.Format(result,string.Empty));
                return;
            }
            StringBuilder sb = new StringBuilder();
            foreach (XmlElement ele in sellRankList)
            {
                int index = sellRankList.IndexOf(ele) + 1;
                sb.AppendFormat(item
                    ,ele.GetAttribute("CsId")
                    ,ele.GetAttribute("ShowName")
                    , ele.GetAttribute("PriceRange")
                    , ele.GetAttribute("SellNum")
                    , ele.GetAttribute("ImgUrl")
                    , index
                    , index == sellRankList.Count ? "":",");
            }
            context.Response.Write(string.Format(result, sb.ToString()));
        }

        private void GetParam(HttpContext context)
        {
            levelId = ConvertHelper.GetInteger(context.Request["level"]);
            levelName = CarLevelDefine.GetLevelNameById(this.levelId);
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