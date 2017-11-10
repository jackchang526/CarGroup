using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AppApi
{
    public class BaseController : Controller
    {
        protected string ContentType { get { return "application/json"; } }

        protected internal JsonResult JsonNet(object data)
        {
            return this.JsonNet(data, ContentType, null, JsonRequestBehavior.DenyGet);
        }

        protected internal JsonResult JsonNet(object data, string contentType)
        {
            return this.JsonNet(data, contentType, null, JsonRequestBehavior.DenyGet);
        }

        protected internal JsonResult JsonNet(object data, JsonRequestBehavior behavior)
        {
            return this.JsonNet(data, ContentType, null, behavior);
        }

        protected internal virtual JsonResult JsonNet(object data, string contentType, Encoding contentEncoding)
        {
            return this.JsonNet(data, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
        }

        protected internal JsonResult JsonNet(object data, string contentType, JsonRequestBehavior behavior)
        {
            return this.JsonNet(data, contentType, null, behavior);
        }

        protected internal virtual JsonResult JsonNet(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult() { Data = data, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behavior };
        }

        protected internal JsonResult AutoJson(object data)
        {
            return this.AutoJson(data, ContentType, null, JsonRequestBehavior.AllowGet);
        }

        protected internal JsonResult AutoJson(object data, string contentType)
        {
            return this.AutoJson(data, contentType, null, JsonRequestBehavior.DenyGet);
        }

        protected internal JsonResult AutoJson(object data, JsonRequestBehavior behavior)
        {
            return this.AutoJson(data, ContentType, null, behavior);
        }

        protected internal virtual JsonResult AutoJson(object data, string contentType, Encoding contentEncoding)
        {
            return this.AutoJson(data, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
        }

        protected internal JsonResult AutoJson(object data, string contentType, JsonRequestBehavior behavior)
        {
            return this.AutoJson(data, contentType, null, behavior);

        }

        protected internal virtual JsonResult AutoJson(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult(new CamelCasePropertyNamesContractResolver()) { Data = data, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behavior };
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            
            base.OnException(filterContext);
        }
    }
}