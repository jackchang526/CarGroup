using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// GetCarIntoMarketText 的摘要说明
    /// </summary>
    public class GetCarIntoMarketText : PageBase,IHttpHandler
    {
        private string csIds = string.Empty;
        private string carIds = string.Empty;
        private string type = string.Empty;
        private string callback = string.Empty;
        private int isShowDate = 0;
        Car_SerialBll carSerialBll = null;


        public void ProcessRequest(HttpContext context)
        {
            base.SetPageCache(10);
            context.Response.ContentType = "application/x-javascript";
            GetParams(context);
            if (string.IsNullOrEmpty(type))
            {
                RenderResult("[]", context);
                return;
            }
            switch (type.ToLower())
            {
                case "car": RenderCarMarketType(context); break;
                case "serial": RenderSerialMarketText(context); break;
            }
        }

        /// <summary>
        /// 获取车款上市标签
        /// </summary>
        /// <param name="context"></param>
        private void RenderCarMarketType(HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(carIds))
                RenderResult("[]", context);
            carSerialBll = new Car_SerialBll();
            string[] carIdArr = carIds.Split(',');
            List<string> jsonList = new List<string>();
            
            foreach (string id in carIdArr)
            {
                string text = carSerialBll.GetCarMarketText(ConvertHelper.GetInteger(id));
                if (!string.IsNullOrEmpty(text))
                {
                    jsonList.Add(string.Format("{{\"carid\":\"{0}\",\"text\":\"{1}\"}}", id, text));
                }
            }
            RenderResult(string.Format("[{0}]", string.Join(",", jsonList)), context);
        }

        /// <summary>
        /// 获取车系上市标签
        /// </summary>
        /// <param name="context"></param>
        private void RenderSerialMarketText(HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(csIds))
                RenderResult("[]", context);
            carSerialBll = new Car_SerialBll();

            string[] csIdArr = csIds.Split(',');
            List<string> jsonList = new List<string>();

            foreach (string id in csIdArr)
            {
                string text = carSerialBll.GetNewSerialIntoMarketText(ConvertHelper.GetInteger(id), isShowDate == 1);
                if (!string.IsNullOrEmpty(text))
                {
                    jsonList.Add(string.Format("{{\"csid\":\"{0}\",\"text\":\"{1}\"}}", id, text));
                }
            }
            RenderResult(string.Format("[{0}]", string.Join(",", jsonList)), context);
        }

        /// <summary>
        /// 输出结果
        /// </summary>
        /// <param name="result"></param>
        private void RenderResult(string result, HttpContext context)
        {
            context.Response.Write(string.IsNullOrEmpty(callback) ? result : string.Format("{0}({1})", callback, result));
            context.Response.End();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="context"></param>
        private void GetParams(HttpContext context)
        {
            csIds = context.Request["csids"];
            carIds = context.Request["carids"];
            type = context.Request["type"];
            isShowDate = ConvertHelper.GetInteger(context.Request["isshowdate"]);
            callback = context.Request["callback"];
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