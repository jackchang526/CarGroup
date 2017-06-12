using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// ListForCompare 的摘要说明
	/// </summary>
	public class ListForCompare : IHttpHandler
	{

		private string pageXML = string.Empty;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/xml";
			string keyTemp = "ListForCompare_ashx";
			object listForCompare_ashx = null;
			CacheManager.GetCachedData(keyTemp, out listForCompare_ashx);
			if (listForCompare_ashx == null)
			{
				CommonService cs = new CommonService();
				pageXML = cs.GetAllSerialForCompareList();
				CacheManager.InsertCache(keyTemp, pageXML, 5);
			}
			else
			{
				pageXML = Convert.ToString(listForCompare_ashx);
			}
			context.Response.Write(pageXML);
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