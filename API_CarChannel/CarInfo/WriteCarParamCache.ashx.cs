using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// WriteCarParamCache 的摘要说明
    /// </summary>
    public class WriteCarParamCache : IHttpHandler
    {
        private string resultStr = "{{\"result\":\"{0}\",\"msg\":\"{1}\"}}";
        List<int> CarIdList = null;//车款列表
        bool IsOptional = false;//是否包含价格选配项
        string callback = string.Empty;
        Car_BasicBll carBasicBll = new Car_BasicBll();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/x-javascript";
            if (WebConfig.IsUseMemcache)
            {
                GetParams(context);
                WriteCache();
            }
            else
            {
                resultStr = string.Format(resultStr, "failure", "未启用Memcache");
            }
            context.Response.Write(string.IsNullOrEmpty(callback) ? resultStr : (string.Format("{0}({1})", callback, resultStr)));
        }

        /// <summary>
        /// 写缓存
        /// </summary>
        private void WriteCache()
        {
            if (CarIdList != null && CarIdList.Count > 0)
            {
                if (IsOptional)
                {
                    carBasicBll.GetCarCompareDataWithOptionalByCarIDs(CarIdList);
                }
                else
                {
                    carBasicBll.GetCarCompareDataByCarIDs(CarIdList);
                }
                resultStr = string.Format(resultStr, "success", "");
            }
            else
            {
                resultStr = string.Format(resultStr, "failure", "参数错误");
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetParams(HttpContext context)
        {
            string carIds = context.Request["carids"];
            if (!string.IsNullOrEmpty(carIds))
            {
                CarIdList = new List<int>();
                string[] carIdArr = carIds.Split(',');
                foreach (string id in carIdArr)
                {
                    int idInt = ConvertHelper.GetInteger(id);
                    if (idInt > 0)
                    {
                        CarIdList.Add(idInt);
                    }
                }
            }
            int optional = ConvertHelper.GetInteger(context.Request["optional"]);
            if (optional == 1)
            {
                IsOptional = true;
            }
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