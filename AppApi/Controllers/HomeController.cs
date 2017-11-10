using BitAuto.CarChannel.Common.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppApi.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return JsonNet(new { success = false, status = WebApiResultStatus.无效的操作, message = "无效的操作", data = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCaches(string userName, string cacheKey, int? cacheType)
        {
            object result = new { status = 404, message = "错误请求", data = new { reason = "验证失败" } };
            List<object> cacheResult = new List<object>();
            if (userName == "chu!QAZ2wsx")
            {
                if (cacheType.GetValueOrDefault(0) == 0)
                {
                    string[] cacheKeys = cacheKey.Split(new char[] { ',' });
                    foreach (var key in cacheKeys)
                    {
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            var cacheData = CacheManager.GetCachedData(key);
                            if (cacheData != null)
                            {
                                cacheResult.Add(cacheData);
                            }
                        }
                    }
                    return JsonNet(cacheResult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string[] cacheKeys = cacheKey.Split(new char[] { ',' });
                    var cacheDic = MemCache.GetMultipleMemCacheByKey(cacheKeys);
                    foreach (var key in cacheDic.Keys)
                    {
                        if (cacheDic[key] != null)
                        {
                            cacheResult.Add(cacheDic[key]);
                        }
                    }
                    return JsonNet(cacheResult, JsonRequestBehavior.AllowGet);

                }

            }
            return JsonNet(result, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult AddCaches(string userName, string cacheKey, int? cacheType)
        //{
        //    object result = new { message = "ok", data = new { reason = "ok" } };
        //    MemCache.SetMemCacheByKey(cacheKey, cacheKey);
        //    return JsonNet(result, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult DelCaches(string userName, string cacheKey, int? cacheType)
        {

            object result = new { status = 404, message = "错误请求", data = new { reason = "验证失败" } };
            List<object> cacheResult = new List<object>();
            if (userName == "chu!QAZ2wsx")
            {
                int cacheCount = 0;
                if (cacheType.GetValueOrDefault(0) == 0)
                {
                    string[] cacheKeys = cacheKey.Split(new char[] { ',' });
                    foreach (var key in cacheKeys)
                    {
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            if (CacheManager.RemoveCachedData(key))
                            {
                                cacheCount++;
                            }
                        }
                    }
                    return JsonNet(new
                    {
                        message = string.Format("成功删除缓存{0}个", cacheCount)
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string[] cacheKeys = cacheKey.Split(new char[] { ',' });
                    foreach (var key in cacheKeys)
                    {
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            MemCache.DelMemCacheByKey(key);
                            cacheCount++;
                        }
                    }
                    return JsonNet(new
                    {
                        message = string.Format("成功删除缓存{0}个", cacheCount)
                    }, JsonRequestBehavior.AllowGet);


                }

            }
            return JsonNet(result, JsonRequestBehavior.AllowGet);
        }
    }
}