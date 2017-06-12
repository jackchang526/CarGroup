using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannelAPI.Web.AppCode;

namespace BitAuto.CarChannelAPI.Web.zhihuan
{
	/// <summary>
	/// 获取全部品牌下多少子品牌有置换数据
	/// </summary>
	public class GetSerialCount : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(30);
			context.Response.ContentType = "Text/XML";
			context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			context.Response.Write("<Root>");

			Dictionary<int, int> brands = new CarNewsBll().GetZhiHuanSerialCount();
			if (brands != null && brands.Count > 0)
			{
				foreach (KeyValuePair<int,int> brand in brands)
				{
					context.Response.Write(string.Format("<Brand Id=\"{0}\" SerialCount=\"{1}\"/>"
					, brand.Key.ToString(), brand.Value.ToString()));
				}
			}
			context.Response.Write("</Root>");
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