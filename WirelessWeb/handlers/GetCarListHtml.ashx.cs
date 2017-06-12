using System.Collections.Generic;
using System.Text;
using System.Web;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;

namespace WirelessWeb.handlers
{
	/// <summary>
	/// GetCarListHtml 的摘要说明
	/// </summary>
	public class GetCarListHtml : WirelessPageBase, IHttpHandler
	{
		int _serialId = 0;
		SerialEntity _serialEntity;
		List<EnumCollection.CarInfoForSerialSummary> _serialCarList;
		string _carList;
		int _yearCount;
		string _serialShowName;
		string _serialAllSpell;
		private bool _isYearEnabled;
		int _serialYear;

		HttpResponse response;
		HttpRequest request;
		public void ProcessRequest(HttpContext context)
		{
			base.SetPageCache(10);
			context.Response.ContentType = "text/html";
			response = context.Response;
			request = context.Request;
			GetParamter();
			if (_serialEntity == null)
				return;
			InitSerialInfo();
			GetCarListByCsID();
			RenderContent();
		}
		/// <summary>
		/// 输出内容
		/// </summary>
		private void RenderContent()
		{
			string cacheKey = "Car_Wireless_GetCarListHTML_" + _serialId;
			object serialHtml = CacheManager.GetCachedData(cacheKey);
			if (serialHtml != null)
				response.Write((string)serialHtml);
			else
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(_carList);
				CacheManager.InsertCache(cacheKey, sb.ToString(), 10);
				response.Write(sb.ToString());
			}
		}

		private void GetParamter()
		{
			_serialId = ConvertHelper.GetInteger(request.QueryString["ID"]);
			_serialYear = ConvertHelper.GetInteger(request.QueryString["year"]);

			_serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, _serialId);
		}
		/// <summary>
		/// 生成子品牌概况Html
		/// </summary>
		private void InitSerialInfo()
		{
			_serialAllSpell = _serialEntity.AllSpell;
			_serialShowName = _serialEntity.ShowName;
		}

		/// <summary>
		/// 车型列表
		/// </summary>
		private void GetCarListByCsID()
		{
			if (_serialId > 0 && _serialId > 0)
			{
				if (_serialEntity.SaleState == "停销")
				{
					// 停销子品牌取 停销子品牌最新年款的车型(高总逻辑 Oct.11.2011)
					_serialCarList = GetAllCarInfoForNoSaleSerialSummaryByCsID(_serialId);
				}
				else
				{
					// 非停销子品牌取 子品牌的非停销所有年款车型
					_serialCarList = base.GetAllCarInfoForSerialSummaryByCsID(_serialId);
				}
				if (_serialCarList.Count > 0)
				{
					var year = string.Empty;
					_serialCarList.Sort(NodeCompare.CompareCarByYear);
					List<string> listCarList = new List<string>(20);
					listCarList.Add("<dl class=\"tt-list\">");
					foreach (EnumCollection.CarInfoForSerialSummary cifss in _serialCarList)
					{
						//modified by sk 2013.06.03 修改最低报价
						string carMinPrice = string.Empty;
						string carPriceRange = cifss.CarPriceRange.Trim();
						if (cifss.CarPriceRange.Trim().Length == 0)
							carMinPrice = "暂无报价";
						else
						{
							if (carPriceRange.IndexOf('-') != -1)
								carMinPrice = carPriceRange.Substring(0, carPriceRange.IndexOf('-')) + "万";
							else
								carMinPrice = carPriceRange;
						}

						if (!string.IsNullOrEmpty(cifss.CarYear) && cifss.CarYear != year)
						{
							year = cifss.CarYear;
							listCarList.Add("<dt><span>" + year + "款</span></dt>");
						}
						listCarList.Add("<dd id= \"" + cifss.CarID.ToString() + "\"><a href=\"javascript:selectCarId(" + cifss.CarID.ToString() + ");\"><p>" + cifss.CarName + "</p><strong>" + carMinPrice + "</strong></a></dd>");

					}
					listCarList.Add("</dl>");
					_carList = string.Concat(listCarList.ToArray());
				}
			}
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