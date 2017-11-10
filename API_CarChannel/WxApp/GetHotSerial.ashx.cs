using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
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
    /// GetHotSerial 的摘要说明
    /// </summary>
    public class GetHotSerial : PageBase,IHttpHandler
    {
        private int num = 0;
        private StringBuilder sbHtml = new StringBuilder();

        public void ProcessRequest(HttpContext context)
        {
            base.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            GetParam(context);
            GetHotSerialJson(num);
            context.Response.Write(sbHtml.ToString());
        }


        private void GetHotSerialJson(int num)
        {
            List<XmlElement> hotEles = new Car_SerialBll().GetHotSerial(num);
            sbHtml.Append("{\"serial\":[");
            if (hotEles.Count > 0)
            {
                foreach (XmlElement ele in hotEles)
                {
                    var serialId = ConvertHelper.GetInteger(ele.GetAttribute("ID"));
                    var showName = ele.GetAttribute("ShowName");
                    var imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");
                    sbHtml.AppendFormat("{{\"SerialId\":\"{0}\",\"ShowName\":\"{1}\",\"ImgUrl\":\"{2}\"}}{3}"
                        , serialId
                        , showName
                        , imgUrl
                        , hotEles.IndexOf(ele) == hotEles.Count - 1 ? string.Empty : ",");
                }

            }
            sbHtml.Append("]}");
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetParam(HttpContext context)
        {
            num = ConvertHelper.GetInteger(context.Request["num"]);
            if (num <= 0)
            {
                num = 3;
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