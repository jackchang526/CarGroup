using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.CarChannel.CarchannelWeb
{
	public partial class _404error : System.Web.UI.Page
	{
		protected string errorInfo = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Request.QueryString["info"] != null && this.Request.QueryString["info"].ToString() != "")
			{
				errorInfo = this.Request.QueryString["info"].ToString();
				string refURL = "";
				if (this.Request.ServerVariables["HTTP_REFERER"] != null && this.Request.ServerVariables["HTTP_REFERER"].ToString() != "")
				{
					refURL = this.Request.ServerVariables["HTTP_REFERER"].ToString().Trim();
				}
				WriteOperateLog("\r\n[" + DateTime.Now.ToString() + "] RefURL:" + refURL + "\r\n" + errorInfo + "\r\n");
			}
		}

		private void WriteOperateLog(string logContent)
		{
			string sDir = AppDomain.CurrentDomain.BaseDirectory + "log\\error\\";
			// string sDir = "E:\\wwwroot\\AutoChannel\\log\\";
			try
			{
				if (!System.IO.Directory.Exists(sDir))
				{
					System.IO.Directory.CreateDirectory(sDir);
				}
				using (StreamWriter sw = new StreamWriter(sDir + DateTime.Now.ToShortDateString() + ".txt", true))
				{
					sw.Write(logContent);
					sw.Close();
				}
			}
			catch
			{ }
		}
	}
}