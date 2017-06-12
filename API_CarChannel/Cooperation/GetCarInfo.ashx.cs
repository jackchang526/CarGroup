using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.Cooperation
{
	/// <summary>
	/// add by 2016-06-03 For百度百科
	/// </summary>
	public class GetCarInfo : PageBase, IHttpHandler
	{
		HttpRequest request;
		HttpResponse response;

		public void ProcessRequest(HttpContext context)
		{
			request = context.Request;
			response = context.Response;
			string action = request.QueryString["act"];
			switch (action)
			{
				case "serialforbk":
					CacheManager.SetPageCache(60);
					RanderSerialInfoForBaike();
					break;
			}
		}

		private void RanderSerialInfoForBaike()
		{
			response.ContentType = "application/x-javascript";
			int serialId = ConvertHelper.GetInteger(request.QueryString["carSerialId"]);
			if (serialId <= 0) return;
			SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
			if (serialEntity == null) return;
			List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(serialId, 6);

			List<object> list = new List<object>();
			foreach (var sts in lsts)
			{
				list.Add(new { title = sts.ToCsShowName, picUrl = sts.ToCsPic.Replace("_5.", "_3."), carSerialId = sts.ToCsID, price = sts.ToCsPriceRange });
			}
			var data = new { data = new { price = serialEntity.Price, type = serialEntity.Level.Name, fuleConsumption = serialEntity.SummaryFuelCost, relateCars = list } };
			var json = JsonConvert.SerializeObject(data);
			response.Write(json);
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