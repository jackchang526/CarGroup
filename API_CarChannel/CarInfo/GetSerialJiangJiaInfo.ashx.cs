using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using System.Text;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// 获取子品牌降价信息
	/// </summary>
	public class GetSerialJiangJiaInfo : IHttpHandler
	{
		private int _SerialId = 0;

		private HttpRequest _request;
		private HttpResponse _response;

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(30);
			context.Response.ContentType = "application/x-javascript";

			_response = context.Response;
			_request = context.Request;

			GetParams();
			GetContent();
		}

		/// <summary>
		/// 得到页面参数
		/// </summary>
		private void GetParams()
		{
			_SerialId = ConvertHelper.GetInteger(_request.QueryString["id"]);
		}
		/// <summary>
		/// 得到内容
		/// </summary>
		private void GetContent()
		{
			StringBuilder result = new StringBuilder();

			if (_SerialId > 0)
			{
				try
				{
					CarNewsBll newsBll = new CarNewsBll();

					Dictionary<int, SerialJiangJiaNewsSummary> summary = newsBll.GetSerialJiangJiaNewsSummary(_SerialId);

                    SerialJiangJiaNewsSummary serialSummary = null;
					foreach (KeyValuePair<int, SerialJiangJiaNewsSummary> citySummary in summary)
					{
                        serialSummary = citySummary.Value;

                        result.AppendFormat("\"c{0}\"", citySummary.Key).Append(":{");

                        result.AppendFormat("\"favo\":\"{0}\",\"vendorNum\":\"{1}\"", serialSummary.MaxFavorablePrice.ToString("0.##"), serialSummary.VendorNum.ToString());

						result.Append("},");
					}
				}
				catch { }
			}

			if (result.Length > 0)
				result.Remove(result.Length - 1, 1);

			_response.Write("var csJiangJiaInfo = {");
			_response.Write(result.ToString());
			_response.Write("};");
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