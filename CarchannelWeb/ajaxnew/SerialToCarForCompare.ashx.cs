using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// SerialToCarForCompare 的摘要说明 车型对比 子品牌下所有车型
	/// </summary>
	public class SerialToCarForCompare : IHttpHandler
	{
		private string pageXML = string.Empty;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/xml";
			CommonService cs = new CommonService();
			pageXML = cs.GetAllSerialToCarForCompare();
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