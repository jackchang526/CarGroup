using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL;
using System.Web.UI;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using System.Text;

namespace BitAuto.CarChannelAPI.Web.Mai
{
	/// <summary>
	/// GetSerialCashBacks 的摘要说明
	/// </summary>
	public class GetSerialCashBacks : IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;
		SerialGoodsBll serialCashBacksBll;

		private int serialId = 0;
		private int cityId = 0;
		private string callback = string.Empty;

		public void ProcessRequest(HttpContext context)
		{
			OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
			{
				// Duration = 60 * 10,
				Duration = 60 * 60,
				Location = OutputCacheLocation.Any,
				VaryByParam = "*"
			});
			page.ProcessRequest(HttpContext.Current);

			context.Response.ContentType = "application/x-javascript";
			response = context.Response;
			request = context.Request;
			serialCashBacksBll = new SerialGoodsBll();
			//获取参数
			GetParameter();
			RenderContent();
		}

		private void GetParameter()
		{
			serialId = ConvertHelper.GetInteger(request.QueryString["serialId"]);
			cityId = ConvertHelper.GetInteger(request.QueryString["cityid"]);

			callback = request.QueryString["callback"];
		}

		private void RenderContent()
		{
			StringBuilder sb = new StringBuilder();
			List<string> resultList = new List<string>();

			List<CarCashBack> serialGoodsList = serialCashBacksBll.GetCashBacksCarList(serialId, cityId);
			sb.Append("{");
			if (serialGoodsList.Count > 0)
			{
				foreach (CarCashBack entity in serialGoodsList)
				{
					resultList.Add(string.Format("{{SerialId:\"{0}\",CarId:\"{1}\",BackPrice:\"{2}\",Url:\"{3}\"}}",
						entity.SerialId,
						entity.CarId,
						entity.BackPrice,
						entity.Url));
				}
				sb.AppendFormat("IsExist:true,CarList:[{0}]", string.Join(",", resultList.ToArray()));
			}
			else
			{
				sb.Append("IsExist:false,CarList:[]");
			}
			sb.Append("}");

			if (string.IsNullOrEmpty(callback))
				response.Write(string.Format("{0}", sb.ToString()));
			else
				response.Write(string.Format("{1}({0})", sb.ToString(), callback));
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		private sealed class OutputCachedPage : Page
		{
			private OutputCacheParameters _cacheSettings;

			public OutputCachedPage(OutputCacheParameters cacheSettings)
			{
				ID = Guid.NewGuid().ToString();
				_cacheSettings = cacheSettings;
			}

			protected override void FrameworkInitialize()
			{
				base.FrameworkInitialize();
				InitOutputCache(_cacheSettings);
			}
		}
	}
}