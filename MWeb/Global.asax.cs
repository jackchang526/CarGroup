using BitAuto.CarChannel.Common;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MWeb
{
	// 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
	// 请访问 http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			//AreaRegistration.RegisterAllAreas();


			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			//去除 mvc header 输出
			MvcHandler.DisableMvcResponseHeader = true;
			//加载基本配置
			WebConfig.LoadConfig();
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			// 在出现未处理的错误时运行的代码
			Exception objErr = Server.GetLastError().GetBaseException();
			Server.ClearError();			
			var httpStatusCode = (objErr is HttpException) ? (objErr as HttpException).GetHttpCode() : 500; //这里仅仅区分两种错误  
			switch (httpStatusCode)
			{
				case 404:
					Response.Redirect("/?refer=" + (Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString()), true);
					break;
				default:
					string err = "\r\nIP: " + BitAuto.Utils.WebUtil.GetClientIP() +
								 "\r\nError in: " + Request.Url.ToString() +
								 "\r\nRef: " + (Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString()) +
								 "\r\nError Message: " + objErr.Message.ToString() +
								 "\r\nStack Trace: " + objErr.StackTrace.ToString();
					WriteErrorLog(err);
					Response.Redirect("/error", true);
					break;
			}
		}

		private static void WriteErrorLog(string logContent)
		{
			string subDir = DateTime.Now.ToString("yyyy-MM-dd");
			string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "-" + DateTime.Now.Hour.ToString();
			string sDir = AppDomain.CurrentDomain.BaseDirectory + "GlobalLog\\" + subDir + "\\";
			try
			{
				if (!System.IO.Directory.Exists(sDir))
				{
					System.IO.Directory.CreateDirectory(sDir);
				}
				using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sDir + "" + fileName + ".txt", true))
				{
					sw.Write("[" + DateTime.Now.ToString() + "]  " + logContent + "\r\n\r\n");
					sw.Close();
				}
			}
			catch
			{ }
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			//没有斜杠 301 添加 斜杠 add by sk 2017.03.07
			string strUrl = Request.RawUrl;
			if (strUrl.IndexOf(".", StringComparison.Ordinal) < 0 && strUrl.IndexOf("?", StringComparison.Ordinal) < 0 && !strUrl.EndsWith("/"))
			{
				Response.RedirectPermanent(strUrl + "/");
			}
		}
	}
}