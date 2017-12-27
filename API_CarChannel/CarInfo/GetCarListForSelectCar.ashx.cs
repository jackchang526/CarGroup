using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using BitAuto.Utils;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using System.Web.Script.Serialization;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetCarListForSelectCar 的摘要说明
	/// </summary>
	public class GetCarListForSelectCar : IHttpHandler
	{
		private Car_BasicBll _carBLL;

		HttpResponse response;
		HttpRequest request;

		private int serialId = 0;
		private IEnumerable<int> carIdList;
		private string callback = string.Empty;

		public GetCarListForSelectCar()
		{
			_carBLL = new Car_BasicBll();
		}

		public void ProcessRequest(HttpContext context)
		{
			OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
			{
				Duration = 30 * 10,
				Location = OutputCacheLocation.Any,
				VaryByParam = "*"
			});
			page.ProcessRequest(HttpContext.Current);

			context.Response.ContentType = "application/x-javascript";
			response = context.Response;
			request = context.Request;

			//获取参数
			GetParameter();
			RenderContent();
		}

		private void RenderContent()
		{
			List<string> list = new List<string>();
			//string cacheKey = "GetCarListForSelectCar_" + serialId;
			var allCarList = _carBLL.GetCarInfoForSerialSummaryBySerialId(serialId);
			var carList = allCarList.FindAll(p => carIdList.Contains(p.CarID));
			var maxPv = allCarList.Max(p => p.CarPV);
			carList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
			var obj = new { MaxPv = maxPv, CarList = carList };
			JavaScriptSerializer js = new JavaScriptSerializer();
			var s = js.Serialize(obj);
			//var s = JsonHelper.Serialize(carinfoList);
			response.Write(string.Format(!string.IsNullOrEmpty(callback) ? (callback + "({0})") : "{0}", s));
		}

		private void GetParameter()
		{
			serialId = ConvertHelper.GetInteger(request.QueryString["serialid"]);
			var carIds = request.QueryString["carIds"];
			var top = ConvertHelper.GetInteger(request.QueryString["top"]);
			if (top <= 0) top = 40;
			callback = request.QueryString["callback"];

			if (!string.IsNullOrEmpty(carIds))
			{
				carIdList = carIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => ConvertHelper.GetInteger(p)).Take(top);
			}
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