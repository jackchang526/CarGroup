using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using BitAuto.Utils;
using System.Web.Script.Serialization;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.BLL;
using System.Xml;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// GetHotCarForPhotoCompare 的摘要说明
	/// </summary>
	public class GetHotCarForPhotoCompare : PageBase, IHttpHandler
	{
		HttpRequest request;
		HttpResponse response;

		Car_BasicBll carBLL;
		CommonService commonService;

		private string serialIds = string.Empty;

		public GetHotCarForPhotoCompare()
		{
			carBLL = new Car_BasicBll();
			commonService = new CommonService();
		}

		public void ProcessRequest(HttpContext context)
		{
			OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
			{
				Duration = 60 * 60,
				Location = OutputCacheLocation.Any,
				VaryByParam = "*"
			});
			page.ProcessRequest(HttpContext.Current);

			context.Response.ContentType = "application/x-javascript";
			request = context.Request;
			response = context.Response;

			GetParamsters();

			RenderContent();
		}

		private void GetParamsters()
		{
			serialIds = request.QueryString["serialIDs"];
 		}

		private void RenderContent()
		{

			IEnumerable<int> serialIdArray = null;
 			if (!string.IsNullOrEmpty(serialIds))
			{
				serialIdArray = serialIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Distinct().Select(p => ConvertHelper.GetInteger(p));
			}
			var dict = commonService.GetPhotoCompareSerialAndCarList();
			List<object> hotCarList = new List<object>();

			if (serialIdArray != null && serialIdArray.Count() > 0)
			{
				//根据子品牌 获取 看了还看
				List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(serialIdArray.LastOrDefault(), 10);
				var serialList = lsts.Where(p => !serialIdArray.Contains(p.ToCsID) && dict.ContainsKey(p.ToCsID));
				foreach (EnumCollection.SerialToSerial sts in serialList)
				{
					var carId = carBLL.GetHotCarForPhotoCompareBySerialId(sts.ToCsID);
					if (carId > 0)
						hotCarList.Add(new { SerialId = sts.ToCsID, SerialName = sts.ToCsShowName, SerialImage = sts.ToCsPic, CarId = carId });
				}
			}
			else
			{
				//无子品牌 取 最热门车型
				var hotSerialList = new Car_SerialBll().GetHotSerial(10);

				foreach (XmlNode node in hotSerialList)
				{
					int serialId = ConvertHelper.GetInteger(node.Attributes["ID"].Value);
					string serialName = node.Attributes["ShowName"].Value;
					string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId);

					var carId = new Car_BasicBll().GetHotCarForPhotoCompareBySerialId(serialId);
					if (carId > 0)
						hotCarList.Add(new { SerialId = serialId, SerialName = serialName, SerialImage = imgUrl, CarId = carId });
				}
			}
			var obj = new JavaScriptSerializer();
			response.Write(obj.Serialize(hotCarList));
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