using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.CarChannel.BLL;
using BitAuto.Utils;

namespace BitAuto.CarChannelAPI.Web.zhihuan
{
	/// <summary>
	/// GetBrandList 的摘要说明
	/// </summary>
	public class GetBrandList : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(30);

			context.Response.ContentType = "Text/XML";
			context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			context.Response.Write("<Root>");

			int cityId = ConvertHelper.GetInteger(context.Request.QueryString["cityid"]);
			if (cityId > 0)
			{
				List<int> brandIds = new CarNewsBll().GetZhiHuanBrandIds(cityId);
				if (brandIds != null && brandIds.Count > 0)
				{
					foreach (int brandId in brandIds)
					{
						context.Response.Write(string.Format("<Brand Id=\"{0}\" />", brandId.ToString()));
					}
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