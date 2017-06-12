using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Threading;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannelAPI.Web.Test
{
	/// <summary>
	/// TestResponse 的摘要说明
	/// </summary>
	public class TestResponse : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			//var client = new WebClient();
			//client.Encoding = System.Text.Encoding.UTF8;//  //System.Text.Encoding.GetEncoding("gb2312");
			//client.DownloadStringCompleted += (s, evt) =>
			//{
			//	context.Response.Write("document.write('" + CommonFunction.GetUnicodeByString(evt.Result) + "');");
			//};
			//client.DownloadStringAsync(new Uri("http://api.bitcar.com/api/GetBrokerInfo?brokerid=2370&modelid=2370"));
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}