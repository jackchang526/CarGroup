using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BitAuto.CarChannelAPI.Web.Mai
{
	/// <summary>
	/// GetSerialGoodsNew 的摘要说明
	/// 新版易车惠 2014-3 在老版易车惠接口上变更数据来源 modified by chengl Mar.13.2014
	/// </summary>
	public class GetSerialGoodsNew : IHttpHandler
	{

		HttpResponse response;
		HttpRequest request;
		SerialGoodsBll serialGoodsBll;

		// private string mongoDBConnectionString =  "mongodb://192.168.0.128/?safe=true";
		private string mongoDBDatabase = "choosecarNew";
		private string collectionName = "GoodsInfo";

		private int serialId = 0;
		private int cityId = 0;
		private string callback = string.Empty;

		public void ProcessRequest(HttpContext context)
		{
			OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
			{
				// Duration = 60 * 10,
				Duration = 24 * 60 * 60,
				Location = OutputCacheLocation.Any,
				VaryByParam = "*"
			});
			page.ProcessRequest(HttpContext.Current);

			context.Response.ContentType = "application/x-javascript";
			response = context.Response;
			request = context.Request;
			serialGoodsBll = new SerialGoodsBll();
			//获取参数
			GetParameter();
			// RenderContent();
			RenderYiChehuiV3();
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

			List<CarGoodsEntity> serialGoodsList = serialGoodsBll.GetGoodsCarListNew(serialId, cityId);
			sb.Append("{");
			if (serialGoodsList.Count > 0)
			{
				foreach (CarGoodsEntity entity in serialGoodsList)
				{
					resultList.Add(string.Format("{{SerialId:\"{0}\",CarId:\"{1}\",GoodsUrl:\"{2}\",MinMarketPrice:\"{3}\",MinBitautoPrice:\"{4}\"}}",
						entity.SerialId,
						entity.CarId,
						entity.GoodsUrl,
						entity.MinMarketPrice,
						entity.MinBitautoPrice));
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

		/// <summary>
		/// 易车惠3期 2014-3
		/// </summary>
		private void RenderYiChehuiV3()
		{
			StringBuilder sb = new StringBuilder();
			List<string> resultList = new List<string>();
			if (serialId > 0 && cityId > 0 && WebConfig.MongoDBForCarConnectionString != "")
			{
				IMongoQuery query = Query.And(
					// 子品牌ID
					Query.EQ("Cs_Id", serialId)
					// 城市ID
					, Query.In("CityList", cityId)
					// 上架时间小于等于当前时间，mongoDB内是UTC时间
					, Query.LTE("UpTime", DateTime.Now)
					, Query.GTE("DownTime", DateTime.Now)
					);
				IMongoSortBy sortBy = new SortByDocument { { "BitautoPrice", 1 }, { "GoodsUrl", 1 } };
				// 易车惠入口先去掉
				List<GoodsInfo> listTemp = new List<GoodsInfo>();
				//List<GoodsInfo> listTemp = CarUtils.MongoDBHelper.GetAll<GoodsInfo>(
				//    WebConfig.MongoDBForCarConnectionString
				//    , mongoDBDatabase, collectionName, query, null, sortBy);
				sb.Append("{");
				if (listTemp != null && listTemp.Count > 0)
				{
					// 已经存在的车款ID，用于排重
					List<int> hasCarid = new List<int>();
					foreach (GoodsInfo gi in listTemp)
					{
						if (!hasCarid.Contains(gi.Car_Id))
						{
							hasCarid.Add(gi.Car_Id);
							resultList.Add(string.Format("{{SerialId:\"{0}\",CarId:\"{1}\",GoodsUrl:\"{2}\",MinMarketPrice:\"{3}\",MinBitautoPrice:\"{4}\"}}",
							gi.Cs_Id,
							gi.Car_Id,
							gi.GoodsUrl,
							gi.MarketPrice,
							gi.BitautoPrice));
						}
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

	[Serializable]
	public class GoodsInfo
	{
		public string _id { get; set; }
		public string EntityId { get; set; }
		public int Cs_Id { get; set; }
		public int Car_Id { get; set; }
		public int[] CityList { get; set; }
		public double MarketPrice { get; set; }
		public double BitautoPrice { get; set; }
		public string GoodsUrl { get; set; }
		public DateTime UpTime { get; set; }
		public DateTime DownTime { get; set; }
	}

}