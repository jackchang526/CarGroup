using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BitAuto.CarChannel.Common;

namespace MWeb.Filters
{
    public class ExceptionFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception == null)
                return;

            //if (!filterContext.Exception.Data.Contains("RequestUrl"))
            //	filterContext.Exception.Data.Add("RequestUrl", filterContext.HttpContext.Request.Url);
            //if (!filterContext.Exception.Data.Contains("UrlReferrer"))
            //	filterContext.Exception.Data.Add("UrlReferrer", filterContext.HttpContext.Request.UrlReferrer);

            string excepitonInfo = filterContext.Exception.ToString();
            CommonFunction.WriteLog(filterContext.Exception.Data.ToString() + excepitonInfo);

            base.OnException(filterContext);
        }
    }
}