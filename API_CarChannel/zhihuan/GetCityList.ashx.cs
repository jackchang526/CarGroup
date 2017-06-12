using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.CarChannel.BLL;

namespace BitAuto.CarChannelAPI.Web.zhihuan
{
	/// <summary>
	/// 获取有置换信息的省份和城市ID列表
	/// </summary>
	public class GetCityList : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(30);
			context.Response.ContentType = "Text/XML";
			context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			context.Response.Write("<Root>");

			List<int> cityIds = new CarNewsBll().GetZhiHuanCityIds();
			if (cityIds != null && cityIds.Count > 0)
			{
				foreach (int cityId in cityIds)
				{
					context.Response.Write(string.Format("<City Id=\"{0}\" />", cityId.ToString()));
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