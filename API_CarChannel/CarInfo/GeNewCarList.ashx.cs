using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// GeNewCarList 的摘要说明
    /// </summary>
    public class GeNewCarList : PageBase, IHttpHandler
    {
        private int pageIndex = 1;
        private int pageSize = 10;
        private int type = 0; // type=0 即将上市, type=1 已经上市 
        private string callback = string.Empty;
        private string item = "{{\"csId\":{0},\"showName\":\"{1}\",\"allSpell\":\"{7}\",\"priceRange\":\"{2}\",\"sellNum\":\"{3}\",\"imgUrl\":\"{4}\",\"rank\":{5}}}{6}";
        public void ProcessRequest(HttpContext context)
        {
            base.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            GetParam(context);
            DealRequest(context);
        }

        private void DealRequest(HttpContext context)
        {
            NewCarIntoMarketBll bll = new NewCarIntoMarketBll();
            string jsonList = "";
            int allCount = 0;
            var list = bll.GetNewCarIntoMarketList(type == 0 ? false : true);
            allCount = list.Count;
            jsonList = Newtonsoft.Json.JsonConvert.SerializeObject(list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());

            if (string.IsNullOrEmpty(callback))
            {
                context.Response.Write(ResultUtil.SuccessResult(ResultUtil.PageFormat(pageIndex, pageSize, allCount, jsonList)));
            }
            else
            {
                context.Response.Write(ResultUtil.CallBackResult(callback, ResultUtil.SuccessResult(ResultUtil.PageFormat(pageIndex, pageSize, allCount, jsonList))));
            }
        }


        public static string ResultDateFormat(string month, string page)
        {
            return string.Format("{{\"month\":\"{0}\",\"page\":{1}}}", month, page);
        }

        private void GetParam(HttpContext context)
        {
            this.type = ConvertHelper.GetInteger(context.Request["type"]);
            this.callback = ConvertHelper.GetString(context.Request["callback"]);
            pageIndex = ConvertHelper.GetInteger(context.Request["pageindex"]);
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            pageSize = ConvertHelper.GetInteger(context.Request["pagesize"]);
            if (pageSize <= 0)
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