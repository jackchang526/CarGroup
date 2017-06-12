using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;

namespace WirelessWeb
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			WebConfig.LoadConfig();
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{
			Exception objErr = Server.GetLastError().GetBaseException();
			string err = "\r\nIP: " + BitAuto.Utils.WebUtil.GetClientIP() +
			"\r\nError in: " + Request.Url.ToString() +
			"\r\nError Message: " + objErr.Message.ToString() +
			"\r\nStack Trace: " + objErr.StackTrace.ToString();
			WriteErrorLog(err);
			// Server.ClearError();
			Response.Redirect("~/404error.aspx");
		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

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