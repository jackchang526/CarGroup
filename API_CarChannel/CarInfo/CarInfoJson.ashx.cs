using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// CarInfoJson 的摘要说明
	/// add by chengl Jun.10.2015 子品牌还关注jsonp接口 for zhuyx
	/// </summary>
	public class CarInfoJson : PageBase, IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;

		private string callback = string.Empty;

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
			string action = ConvertHelper.GetString(request.QueryString["action"]);
			callback = request.QueryString["callback"];
			switch (action.ToLower())
			{
				case "baseinfo": RenderCarInfo(); break;
				case "getcstocsbyid": RenderCsToCsByID(); break;
				default: break;
			}
		}

		/// <summary>
		/// 子品牌还关注数据
		/// </summary>
		private void RenderCsToCsByID()
		{
			int csid = ConvertHelper.GetInteger(request.QueryString["csid"]);
			if (csid <= 0)
			{
				Jsonp("", callback, HttpContext.Current);
				return;
			}
			SerialEntity csEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csid);
			if (csEntity == null)
			{
				Jsonp("", callback, HttpContext.Current);
				return;
			}

			int top = ConvertHelper.GetInteger(request.QueryString["top"]);
			if (top <= 0 || top > 10)
			{ top = 6; }

			Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();
			List<EnumCollection.SerialToSerial> listSTS = base.GetSerialToSerialByCsID(csid, top);
			string json = "";
			if (listSTS != null && listSTS.Count > 0)
			{
				List<string> listTemp = new List<string>();
				int loop = 0;
				string stringJsonTemp = "{{\"CsID\":{0},\"Name\":\"{1}\",\"ShowName\":\"{2}\",\"Pic\":\"{3}\",\"PriceRange\":\"{4}\",\"AllSpell\":\"{5}\"}}";
				foreach (EnumCollection.SerialToSerial sts in listSTS)
				{
					if (loop >= top)
					{ break; }
					loop++;
					listTemp.Add(string.Format(stringJsonTemp
						, sts.ToCsID
						, CommonFunction.GetUnicodeByString(sts.ToCsName)
						, CommonFunction.GetUnicodeByString(sts.ToCsShowName)
						, (dicPicWhite.ContainsKey(sts.ToCsID) ? dicPicWhite[sts.ToCsID] : WebConfig.DefaultCarPic)
						, sts.ToCsPriceRange
						, sts.ToCsAllSpell));
				}
				if (listTemp.Count > 0)
				{
					json = string.Concat("[", string.Join(",", listTemp.ToArray()), "]");
				}
			}
			Jsonp(json, callback, HttpContext.Current);
		}

		private void RenderCarInfo()
		{
			int carId = ConvertHelper.GetInteger(request.QueryString["carid"]);
			if (carId <= 0)
			{
				Jsonp("", callback, HttpContext.Current);
				return;
			}
			CarEntity carEntity = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
			if (carEntity == null)
			{
				Jsonp("", callback, HttpContext.Current);
				return;
			}

			var carPic = WebConfig.DefaultCarPic;

			// CarImg
			Dictionary<int, string> dicCarPhoto = new Car_BasicBll().GetCarDefaultPhotoDictionary(1);
			if (dicCarPhoto.ContainsKey(carEntity.Id))
			{
				carPic = dicCarPhoto[carEntity.Id];
			}
			else
			{
				carPic = Car_SerialBll.GetSerialImageUrl(carEntity.SerialId, 1, false);
			}

			var serialName = carEntity.Serial == null ? "" : carEntity.Serial.ShowName;
			var carName = carEntity.CarYear + "款 " + carEntity.Name;
			var json = string.Format("{{SerialId:{0},SerialName:\"{1}\",AllSpell:\"{6}\",ImageUrl:\"{2}\",CarId:{3},CarName:\"{4}\",CarPrice:{5}}}",
				carEntity.SerialId,
				serialName,
				carPic,
				carEntity.Id,
			 CommonFunction.GetUnicodeByString(carName),
			 carEntity.ReferPrice,
			 carEntity.Serial.AllSpell);
			Jsonp(json, callback, HttpContext.Current);
		}

		private void Jsonp(string content, string callbackName, HttpContext context)
		{
			if (string.IsNullOrEmpty(callbackName))
				context.Response.Write(content);
			else
				context.Response.Write(string.Format("{1}({0})", content, callbackName));
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