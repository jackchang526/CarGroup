using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using System.Text;

namespace BitAuto.CarChannelAPI.Web.Mai
{
	/// <summary>
	/// GetSerialGoods 的摘要说明
	/// </summary>
	public class GetSerialGoods : IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;
		SerialGoodsBll serialGoodsBll;

		private int serialId = 0;
		private int cityId = 0;
		private string callback = string.Empty;

		public void ProcessRequest(HttpContext context)
		{
			OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
			{
				Duration = 60 * 10,
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
			List<string> resultList = new List<string>();

			List<SerialGoodsEntity> serialGoodsList = serialGoodsBll.GetSerialGoodsByCity(serialId, cityId);

			foreach (SerialGoodsEntity entity in serialGoodsList)
			{
				resultList.Add(string.Format("{{GoodsId:\"{0}\",SerialId:\"{1}\",GoodsUrl:\"{2}\",CoverImageUrl:\"{3}\",PromotTitle:\"{4}\",StartTime:\"{5}\",EndTime:\"{6}\",MinMarketPrice:\"{7}\",MinBitautoPrice:\"{8}\",PromotionList:[{9}]}}",
					entity.GoodsId,
					entity.SerialId,
					entity.GoodsUrl,
					this.GetImage(entity.CoverImageUrl),
					entity.PromotTitle,
					entity.StartTime.ToString("yyyy-MM-dd"),
					entity.EndTime.ToString("yyyy-MM-dd"),
					entity.MinMarketPrice,
					entity.MinBitautoPrice,
					this.GetPromotion(entity.GoodsId)));
			}
			if (string.IsNullOrEmpty(callback))
				response.Write(string.Format("[{0}]", string.Join(",", resultList.ToArray())));
			else
				response.Write(string.Format("{1}([{0}])", string.Join(",", resultList.ToArray()), callback));
		}

		private string GetPromotion(int goodsId)
		{
			List<string> resultList = new List<string>();

			List<GoodsPromotionEntity> list = serialGoodsBll.GetGoodsPromotion(goodsId);

			foreach (GoodsPromotionEntity entity in list)
			{
				resultList.Add(string.Format("{{Name:\"{0}\",Description:\"{1}\"}}", entity.Name, entity.Description));
			}
			return string.Join(",", resultList.ToArray());
		}

		private string GetImage(string imgPath)
		{
			string tempLast = imgPath.Substring(imgPath.LastIndexOf("/"), imgPath.Length - imgPath.LastIndexOf("/"));
			string guid = tempLast.Substring(0, tempLast.LastIndexOf("."));
			int hashCode = Math.Abs(guid.GetHashCode());
			int img = hashCode % 4 + 1;
			imgPath = imgPath.Replace("img1", "img" + img);
			imgPath = imgPath.Replace("/bitautomai/", "/img/V2img1.baa.bitautotech.com/bitautomai/");
			imgPath = imgPath.Replace(guid, guid + "_180_120_jpg");
			return imgPath;
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