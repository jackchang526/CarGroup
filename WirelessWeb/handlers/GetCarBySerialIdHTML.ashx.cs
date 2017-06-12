using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common;
using System.Text;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Cache;

namespace WirelessWeb.handlers
{
	/// <summary>
	/// GetCarBySerialIdHTML 的摘要说明
	/// </summary>
	public class GetCarBySerialIdHTML : WirelessPageBase, IHttpHandler
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
			CarList();
			RenderContent();
		}
		/// <summary>
		/// 输出内容
		/// </summary>
		private void RenderContent()
		{
			string cacheKey = "Car_Wireless_GetCarBySerialIdHTML_" + _serialId;
			object serialHtml = CacheManager.GetCachedData(cacheKey);
			if (serialHtml != null)
				response.Write((string)serialHtml);
			else
			{
				StringBuilder sb = new StringBuilder();
                sb.Append("<div class=\"b-return\">");
                sb.Append("<a href=\"javascript:goMasterBrand();\" class=\"btn-return\">返回</a>");
                sb.Append("<span>选择车款</span>");
                sb.Append("</div>");
				sb.Append("<div class=\"mbt-page\">");
				sb.Append(_carList);
				sb.Append("</div>");
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
		private void CarList()
		{
			// modified by chengl Oct.11.2011
			_serialCarList = new List<EnumCollection.CarInfoForSerialSummary>();
			List<EnumCollection.CarInfoForSerialSummary> saleCarInfo = new List<EnumCollection.CarInfoForSerialSummary>();
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
			if (_serialCarList.Count == 0)
			{
				_carList = "暂无车款";
				return;
			}
			saleCarInfo = base.GetAllCarInfoForSerialSummaryByCsID(_serialId, true);
			_serialCarList.Sort(NodeCompare.CompareCarByExhaust);
			//年款列表
			List<string> yearList = new List<string>();
			//在售年款
			List<string> saleYearList = new List<string>();
			//停售年款
			List<string> nosaleYearList = new List<string>();

			foreach (EnumCollection.CarInfoForSerialSummary carInfo in saleCarInfo)
			{
				if (carInfo.CarYear.Length > 0)
				{
					string yearType = carInfo.CarYear + "款";
					if (carInfo.SaleState == "停销")
					{
						if (!nosaleYearList.Contains(yearType))
							nosaleYearList.Add(yearType);
					}
					else
					{
						if (!saleYearList.Contains(yearType))
							saleYearList.Add(yearType);
					}
				}
			}
			//排除包含在售年款
			foreach (string year in saleYearList)
			{
				if (nosaleYearList.Contains(year))
				{
					nosaleYearList.Remove(year);
				}
			}

			saleYearList.Sort(NodeCompare.CompareStringDesc);
			nosaleYearList.Sort(NodeCompare.CompareStringDesc);

			if (_serialEntity.SaleState != "停销")
			{
				yearList = saleYearList;
			}
			else
			{
				yearList = nosaleYearList;
			}
			// del by chengl Sep.29.2012
			//if (yearList.Count < 1)
			//    return;

			StringBuilder htmlCode = new StringBuilder();
			htmlCode.Append("<section class=\"m-line-box\">");
			htmlCode.Append("<div class=\"m-tabs-box\"><ul class=\"m-tabs\" id=\"yeartag\">");
			_yearCount = _serialEntity.SaleState != "停销" ? 3 : 1;
			SetYearEnabled(yearList);
			for (int i = 0; i < yearList.Count && i < _yearCount; i++)
			{
				htmlCode.AppendFormat("<li{0}><span>{1}<s></s></span></li>"
					, IsCarCurrent(yearList[i], i) ? " class=\"current\"" : string.Empty, yearList[i]);
			}
			// 如果只有在销年款 add by chengl Sep.29.2012
			if (yearList.Count == 0 && _serialCarList.Count > 0)
			{
				htmlCode.AppendFormat("<li class=\"current\"><span>未知年款<s></s></span></li>");
			}
			htmlCode.Append("</ul>");
			htmlCode.Append("</div>");
			for (int idx = 0; idx < yearList.Count && idx < _yearCount; idx++)
			{
				htmlCode.Append(GetCarHtml(idx, yearList[idx]));
			}
			// 如果只有在销年款 add by chengl Sep.29.2012
			if (yearList.Count == 0 && _serialCarList.Count > 0)
			{
				htmlCode.Append(GetCarHtml(-1, ""));
			}

			htmlCode.Append("</section>");

			_carList = htmlCode.ToString();
		}

		private string GetCarHtml(int index, string yearType)
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendFormat("<div id=\"yearDiv{0}\"{1}>", index.ToString()
				, IsCarCurrent(yearType, index) ? string.Empty : " style=\"display:none;\"");

			string tempEngine_Exhaust = null;
			foreach (EnumCollection.CarInfoForSerialSummary carInfo in _serialCarList)
			{
				if (index >= 0)
				{
					if (!yearType.StartsWith(carInfo.CarYear))
						continue;
				}
				else
				{
					// 未知年款
					if (carInfo.CarYear != "")
					{ continue; }
				}
				if (carInfo.Engine_Exhaust != tempEngine_Exhaust)
				{
					if (tempEngine_Exhaust != null)
						htmlCode.Append("</ul>");
					tempEngine_Exhaust = carInfo.Engine_Exhaust;
					htmlCode.AppendFormat("<div class=\"m-tabs-box\"><ul class=\"m-tabs m-tabs-small\"><li>{0}</li></ul></div>", tempEngine_Exhaust);
					htmlCode.Append("<ul class=\"m-summary-price m-summary-price-carstyle\">");
				}
 				string carFullName = "";

				// modified by chengl Mar.27.2012
				// 客户要求将世嘉三厢更名为新世嘉
				if (yearType == "2012款" && _serialShowName == "世嘉三厢")
				{ carFullName = "新世嘉&nbsp;" + carInfo.CarName; }
				else
				{ carFullName = _serialShowName + "&nbsp;" + carInfo.CarName; }

				if (carInfo.CarName.StartsWith(_serialShowName))
					carFullName = _serialShowName + "&nbsp;" + carInfo.CarName.Substring(_serialShowName.Length);
				if (yearType != "未知年款")
					carFullName = yearType + " " + carFullName;

				string carMinPrice = string.Empty;
				string carPriceRange = carInfo.CarPriceRange.Trim();
				if (carInfo.CarPriceRange.Trim().Length == 0)
					carMinPrice = "暂无报价";
				else
				{
					if (carPriceRange.IndexOf('-') != -1)
						carMinPrice = carPriceRange.Substring(0, carPriceRange.IndexOf('-'));
					else
						carMinPrice = carPriceRange;
				}
				htmlCode.Append("<li><a href=\"javascript:selectCarId(" + carInfo.CarID.ToString() + ");\"><dl id=\"dl" + carInfo.CarID + "\">");
				//htmlCode.AppendFormat("<dt><a href=\"/" + _serialAllSpell + "/m" + carInfo.CarID.ToString() + "/\">{0}</a></dt>", carFullName);
				htmlCode.AppendFormat("<dt>{0}</dt>", carFullName);
				htmlCode.AppendFormat("<dd><span>参考成交价：</span><em>{0}</em></dd>", carMinPrice);
				//htmlCode.AppendFormat("<dd><span>厂商指导价：</span>{0}</dd>", carInfo.ReferPrice.Trim().Length == 0 ? "暂无" : carInfo.ReferPrice.Trim() + "万");
				//htmlCode.AppendFormat("<dd><span>变&nbsp;&nbsp;&nbsp;&nbsp;速&nbsp;&nbsp;&nbsp;&nbsp;箱：</span>{0}</dd>", carInfo.TransmissionType);

				htmlCode.Append("</dl></a>");
				//htmlCode.AppendFormat("<div class=\"m-btn-xunjia m-btn-gray\"><a href=\"http://price.m.yiche.com/nc{0}/\">询价</a></div>", carInfo.CarID);
				htmlCode.Append("</li>");
			}
			htmlCode.Append("</ul>");
			htmlCode.Append("</div>");
			return htmlCode.ToString();
		}
		private bool IsCarCurrent(string yearStr, int index)
		{
			if (_isYearEnabled)
			{
				return yearStr.StartsWith(_serialYear.ToString());
			}
			else
			{
				// 未知年款 index=-1 add by chengl Sep.29.2012
				return index <= 0;
			}
		}
		private void SetYearEnabled(List<string> yearList)
		{
			string yearStr = _serialYear.ToString();
			for (int i = 0; i < yearList.Count && i < _yearCount; i++)
			{
				if (yearList[i].StartsWith(yearStr))
				{
					_isYearEnabled = true;
					break;
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