using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using BusinessServiceAPI;

namespace BusinessServiceAPI
{
	public class Global : HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			log4net.Config.XmlConfigurator.Configure();
		}

		void Application_End(object sender, EventArgs e)
		{
			//  在应用程序关闭时运行的代码

		}

		void Application_Error(object sender, EventArgs e)
		{
			// 在出现未处理的错误时运行的代码
			Exception objErr = Server.GetLastError().GetBaseException();
			string err = "\r\nIP: " + BitAuto.Utils.WebUtil.GetClientIP() +
			"\r\nError in: " + Request.Url.ToString() +
			"\r\nRef: " + (Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString()) +
			"\r\nError Message: " + objErr.Message.ToString() +
			"\r\nStack Trace: " + objErr.StackTrace.ToString();
			WriteErrorLog(err);
			// System.Diagnostics.EventLog.WriteEntry("CarChannelLog", err, System.Diagnostics.EventLogEntryType.Error);
			Server.ClearError();
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
	}
}
