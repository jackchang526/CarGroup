using AppApi.App;
using AppApi.Controllers;
using BitAuto.CarChannel.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AppApi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // 在应用程序启动时运行的代码
            WebConfig.LoadConfig();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lasteException = Server.GetLastError().GetBaseException();
            var httpException = lasteException as HttpException;

            //接口404的情况直接返回404页面
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
               CommonFunction.WriteLog(string.Format("info:{0},message:{1},error:{2}", HttpHelper.GetHttpLogMessage(Context, false), lasteException.Message, lasteException.StackTrace));
               Response.Clear();
                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "NotFound");
                IController controller = new ErrorController();
                Context.Response.StatusCode = 404;
                controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                Server.ClearError();
                Response.TrySkipIisCustomErrors = true;
                return;
            }
            CommonFunction.WriteLog(string.Format("info:{0},message:{1},error:{2}", HttpHelper.GetHttpLogMessage(Context, false), lasteException.Message, lasteException.StackTrace));
        }
    }
}
