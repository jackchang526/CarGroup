using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using System.Web.UI;
using System.Text;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetCarParameter 的摘要说明
	/// </summary>
	public class GetCarParameter : PageBase, IHttpHandler
	{
		private HttpRequest request;
		private HttpResponse response;

 		private List<int> listCarID = new List<int>();
		private Dictionary<int, Dictionary<string, string>> dicCarParam = new Dictionary<int, Dictionary<string, string>>();
		private bool isParamPage = false;
		private int topCount = 10;
		//private Dictionary<int, string> dicParamIDToName = new Dictionary<int, string>();
 		// 参数模板
		private Dictionary<int, List<string>> dicTemp = null;

		public void ProcessRequest(HttpContext context)
		{
			OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
			{
				Duration = 60 * 15,
				Location = OutputCacheLocation.Any,
				VaryByParam = "*"
			});
			page.ProcessRequest(HttpContext.Current);

			context.Response.ContentType = "application/x-javascript";
 			response = context.Response;
			request = context.Request;

 			GetPageParam();
            StringBuilder sb = new StringBuilder();
            sb.Append(" var carCompareJson = ");
            sb.Append(GetCarParamData());
            //sb.Append("];");

            response.Write(sb.ToString());
		}

		/// <summary>
		/// 取车型ID 
		/// </summary>
		private void GetPageParam()
		{
			// 最多对比车型数量 对比页10个 参数配置页40个
			if (!string.IsNullOrEmpty(request.QueryString["isParamPage"]))
			{
				isParamPage = true;
				topCount = 50;
			}

			string carIDsStr = request.QueryString["carIDs"];
			if (!string.IsNullOrEmpty(carIDsStr))
			{
				string[] carIDArray = carIDsStr.Split(',');
				if (carIDArray != null && carIDArray.Length > 0)
				{
					foreach (string id in carIDArray)
					{
						int carid = 0;
						if (int.TryParse(id.Trim(), out carid))
						{
							if (carid > 0 && !listCarID.Contains(carid))
							{
								listCarID.Add(carid);
								if (listCarID.Count > topCount)
								{ break; }
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 取车型详细参数
		/// </summary>
		private string GetCarParamData()
		{
            if (listCarID.Count > 0)
            {
                return (new Car_BasicBll()).GetValidCarJsObject(listCarID);
            }
            return string.Empty;
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