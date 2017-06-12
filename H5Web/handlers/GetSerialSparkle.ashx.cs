using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using Newtonsoft.Json;

namespace H5Web.handlers
{
    /// <summary>
    /// GetSerialSparkle 的摘要说明
    /// </summary>
	public class GetSerialSparkle : H5PageBase,IHttpHandler
    {
        private readonly SerialFourthStageBll _serialFourthStageBll = new SerialFourthStageBll();
        public void ProcessRequest(HttpContext context)
        {
			base.SetPageCache(60);

            context.Response.ContentType = "application/json; charset=utf-8";

            if (context.Request.QueryString["csid"] == null && string.IsNullOrEmpty(context.Request.QueryString["csid"]))
            {
                return;
            }

            int serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);

            int topCount = 20;

            if (context.Request.QueryString["top"] != null)
            {
                int.TryParse(context.Request.QueryString["top"], out topCount);
            }

            var keyList = new List<int> { serialId, topCount };

            var cacheKey = string.Format("H5V3_GetSerialSparkle_2015_{0}", string.Join("_", keyList));

            object obj = CacheManager.GetCachedData(cacheKey);

            if (obj != null)
            {
                context.Response.Write(obj);
            }
            else
            {
                List<SerialSparkle> serialSparkleList = _serialFourthStageBll.GetSerialSparkle(serialId);

                List<object> targetList=new List<object>();
                serialSparkleList.ForEach(p =>
                {
                    targetList.Add(new
                    {
                        H5SId=p.H5SId,
                        Name=p.Name
                    });
                });

                var serializeObject = JsonConvert.SerializeObject(targetList.Take(topCount));

                CacheManager.InsertCache(cacheKey, serializeObject, WebConfig.CachedDuration);

                context.Response.Write(serializeObject);
            }
            context.Response.End();
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