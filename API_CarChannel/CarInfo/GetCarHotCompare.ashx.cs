using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using BitAuto.Utils;
using System.Data;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetCarHotCompare 的摘要说明
	/// </summary>
	public class GetCarHotCompare : IHttpHandler
	{
		private HttpRequest request;
		private HttpResponse response;

		private Car_BasicBll _carBLL;
		private int carId = 0;
		private int top = 0;
		private string callback = string.Empty;

		public GetCarHotCompare()
		{
			_carBLL = new Car_BasicBll();
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
			response = context.Response;
			request = context.Request;

			//获取参数
			GetParameter();
			//内容输出
			RenderContent();
		}

		private void GetParameter()
		{
			carId = ConvertHelper.GetInteger(request.QueryString["carid"]);
			top = ConvertHelper.GetInteger(request.QueryString["top"]);
			callback = request.QueryString["callback"];

			if (top <= 0)
				top = 15;
		}

		private void RenderContent()
		{
			List<string> list = new List<string>();
			string cacheKey = "CarSummary_Car" + carId;
			var carEntity = this.GetLocalCache<Car_BasicEntity>(cacheKey, () => { return (new Car_BasicBll()).Get_Car_BasicByCarID(carId); }, 60);
			int currentCsID = carEntity.Cs_Id;

			DataSet dsCompare = _carBLL.GetCarCompareListByCarID(carId);
			if (dsCompare != null && dsCompare.Tables.Count > 0 && dsCompare.Tables[0].Rows.Count > 0)
			{
				var query = dsCompare.Tables[0].AsEnumerable()
					.Where(row => ConvertHelper.GetInteger(row["cs_id"]) != currentCsID)
					.Take(top);
				foreach (DataRow dr in query)
				{
					int serialId = ConvertHelper.GetInteger(dr["cs_id"]);
					int yearType = ConvertHelper.GetInteger(dr["Car_YearType"]);
					list.Add(string.Format("{{serialId:{0},serialName:\"{1}\",carId:{2},carName:\"{3}\",carYearType:{4}}}",
						serialId,
						dr["cs_showname"],
						dr["cCarID"],
						dr["car_name"],
						yearType));
				}
			}
			else
			{
				//同级别最热门车
				SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, currentCsID);
				if (serialEntity != null)
				{
					string levelName = serialEntity.Level.Name;
					var cacheKeyForHotCar = "Car_GetHotCompare_" + carId;
					var dsHotCar = this.GetLocalCache<DataSet>(cacheKeyForHotCar, () => { return _carBLL.GetHotCarForCompare(levelName, currentCsID); }, 60);
					if (dsHotCar != null && dsHotCar.Tables.Count > 0 && dsHotCar.Tables[0].Rows.Count > 0)
					{
						var query = dsHotCar.Tables[0].AsEnumerable().Take(top);
						foreach (DataRow dr in query)
						{
							int serialId = ConvertHelper.GetInteger(dr["cs_id"]);
							int yearType = ConvertHelper.GetInteger(dr["Car_YearType"]);
							list.Add(string.Format("{{serialId:{0},serialName:\"{1}\",carId:{2},carName:\"{3}\",carYearType:{4}}}",
								serialId,
								dr["cs_showname"].ToString().Trim(),
								dr["car_id"],
								dr["car_name"],
								yearType));
						}
					}
				}
			}
			response.Write(string.Format(!string.IsNullOrEmpty(callback) ? (callback + "([{0}])") : "[{0}]", string.Join(",", list.ToArray())));
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// 简单通用缓存
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="cacheKey"></param>
		/// <param name="func"></param>
		/// <param name="interval"></param>
		/// <returns></returns>
		public TResult GetLocalCache<TResult>(string cacheKey, Func<TResult> func, int interval)
		{
			var objT = (TResult)CacheManager.GetCachedData(cacheKey);
			if (objT == null)
			{
				objT = func();
				CacheManager.InsertCache(cacheKey, objT, interval);
			}
			return objT;
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