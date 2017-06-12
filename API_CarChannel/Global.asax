<%@ Application Language="C#" %>
<%@ Import Namespace="BitAuto.CarChannel.Common" %>
<%@ Import Namespace="BitAuto.CarChannel.Common.Interface" %>
<script RunAt="server">

	public void Application_Start(object sender, EventArgs e)
	{
		// 在应用程序启动时运行的代码
		WebConfig.LoadConfig();
	}

	void Application_End(object sender, EventArgs e)
	{
		//  在应用程序关闭时运行的代码

	}

	void Application_BeginRequest(object sender, EventArgs e)
	{
		// Context.Response.WriteFile("html/header.shtml");
		// Context.Response.Output.ToString().Replace("#TestHeader#", "aaaaaa");
	}

	void Application_EndRequest(object sender, EventArgs e)
	{
		// Context.Response.WriteFile("html/footer.shtml");
		// Context.Response.Output.ToString().Replace("#TestHeader#", "aaaaaa"); 
	}

	public void Application_Error(object sender, EventArgs e)
	{
		// 在出现未处理的错误时运行的代码
		Exception objErr = Server.GetLastError().GetBaseException();
		string err = "\r\nIP: " + BitAuto.Utils.WebUtil.GetClientIP() +
		"\r\nError in: " + Request.Url.ToString() +
		"\r\nRef: " + (Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString()) +
		"\r\nError Message: " + objErr.Message.ToString() +
		"\r\nStack Trace: " + objErr.StackTrace.ToString();
		WriteErrorLog(err);
		// System.Diagnostics.EventLog.WriteEntry("CarChannelAPILog", err, System.Diagnostics.EventLogEntryType.Error);
		Server.ClearError();
		//Response.Redirect("~/error.htm");
	}

	void Session_Start(object sender, EventArgs e)
	{
		// 在新会话启动时运行的代码

	}

	void Session_End(object sender, EventArgs e)
	{
		// 在会话结束时运行的代码。 
		// 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
		// InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
		// 或 SQLServer，则不会引发该事件。

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
       
</script>
