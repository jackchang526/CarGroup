using System;
using System.Net;
using System.Web.Mvc;

namespace AppApi
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class JsonHandleErrorAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }


            var showException = filterContext.HttpContext.Request["exception"] == "true";
            var innerException = filterContext.Exception;

            JsonResult result = null;
            var httpCode = HttpStatusCode.InternalServerError;
            if (showException)
            {
                result = new JsonResult
                {
                    Data = new { success = false, status = (int)httpCode, message = "获取数据失败，请稍后再试。", data = new { code = httpCode, exception = new { message = innerException.Message, stacktrace = innerException.StackTrace } } },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                result = new JsonResult
                {
                    Data = new { success = false, status = (int)httpCode, message = "获取数据失败，请稍后再试。", data = new { httpcode = httpCode, httperror = innerException.Message } },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            filterContext.HttpContext.ClearError();
            filterContext.Result = result;
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 200; //异常接口不返回异常的httpstatus
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}