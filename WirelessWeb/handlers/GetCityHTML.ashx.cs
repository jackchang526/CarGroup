using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using BitAuto.CarChannel.Common.Cache;

namespace WirelessWeb.handlers
{
	/// <summary>
	/// GetCityHTML 的摘要说明
	/// </summary>
	public class GetCityHTML : IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/html";
			response = context.Response;
			request = context.Request;
			RenderContent();
		}

		private void RenderContent()
		{
			string cacheKey = "Car_Wireless_GetCityHTML";
			object cityHtml = CacheManager.GetCachedData(cacheKey);
			if (cityHtml != null)
				response.Write((string)cityHtml);
			else
			{
				string filePath = HttpContext.Current.Server.MapPath("~/html/cityNew.htm");
				if (File.Exists(filePath))
				{
					string content = File.ReadAllText(filePath);
					CacheManager.InsertCache(cacheKey, content, 60 * 24 * 7);
					response.Write(content);
				}
			}
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