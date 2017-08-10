using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using System;
using System.Collections.Generic;
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
            string strUrl = Request.RawUrl;
            int paramPos = strUrl.IndexOf('?');
            string paramStr = string.Empty;
            string spell = string.Empty;
            string urlParam = string.Empty;
            if (paramPos >= 0)
            {
                paramStr = strUrl.Substring(paramPos + 1);
                spell = strUrl.Substring(0, paramPos).Trim('/');
            }
            if (string.IsNullOrWhiteSpace(spell))
            {
                spell = strUrl.Trim('/');
            }
            int urlPos = spell.IndexOf('/');
            if (urlPos > -1)
            {
                urlParam = spell.Substring(urlPos + 1);
                spell = spell.Substring(0, urlPos).Trim('/');
            }
            Dictionary<string, string> dic = new MVCRouteBll().GetAllChangeUrl();
            if (dic != null && dic.ContainsKey(spell))
            {
                string newUrl = string.Format("{0}://{1}/{2}/{3}{4}",Request.Url.Scheme
                    ,Request.Url.Authority
                    ,dic[spell]
                    ,urlParam.Length > 0 ? (urlParam + "/"): ""
                    ,string.IsNullOrWhiteSpace(paramStr) ? string.Empty : ("?" + paramStr));
                Response.StatusCode = 301;
                Response.AddHeader("Location", newUrl);
                return;
            }

            //没有斜杠 301 添加 斜杠 add by sk 2017.03.07
            //string strUrl = Request.RawUrl;
            if (strUrl.IndexOf(".", StringComparison.Ordinal) < 0 && strUrl.IndexOf("?", StringComparison.Ordinal) < 0 && !strUrl.EndsWith("/"))
			{
				Response.RedirectPermanent(strUrl + "/");
			}
		}
	}
}