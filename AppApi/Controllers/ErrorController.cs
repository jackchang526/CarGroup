using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace AppApi.Controllers
{
    public class ErrorController : BaseController
    {
        static object NotFountJsonData = new { status = 404, message = "网络错误", data = new { reason = "您正在访问一个不存在的接口地址！" } };
        public ActionResult NoAccess()
        {
            return JsonNet(NotFountJsonData, JsonRequestBehavior.AllowGet);
        }

    }
}
