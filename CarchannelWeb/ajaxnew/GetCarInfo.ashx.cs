using System;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// GetCarInfo 的摘要说明
	/// </summary>
	public class GetCarInfo : IHttpHandler
	{
		HttpRequest request;
		HttpResponse response;
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "application/x-javascript";
			request = context.Request;
			response = context.Response;
			string action = request.QueryString["action"];
			if (action != null)
			{
				switch (action.ToLower())
				{
					case "color": GetSerialColor(); break;
				}
			}
		}

		private void GetSerialColor()
		{
			int csId = ConvertHelper.GetInteger(request.QueryString["csid"]);
			Car_SerialBll serial = new Car_SerialBll();
			Dictionary<string, XmlNode> dict = serial.GetSerialColorPhotoByCsID(csId, 0);
			StringBuilder sb = new StringBuilder();
			sb.Append("{");
			if (dict != null && dict.Count > 0)
			{
				sb.Append("ColorList:[");
				foreach (KeyValuePair<string, XmlNode> key in dict)
				{
					sb.Append("\"" + key.Value.Attributes["ImageName"].Value + "\",");
				}
				if (dict.Count > 0) sb.Remove(sb.Length - 1, 1);
				sb.Append("]");
			}
			else
			{
				sb.Append("ColorList:[\"黑色\",\"银灰色\",\"白色\",\"红色\",\"蓝色\",\"深灰色\",\"香槟色\",\"绿色\",\"黄色\",\"橙色\",\"咖啡色\",\"紫色\",\"多彩色\"]");
			}
			sb.Append("}");
			response.Write(sb.ToString());
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