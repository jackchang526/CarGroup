using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils;
using System.Text;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;

namespace BitAuto.CarChannelAPI.Web.Cooperation
{
	/// <summary>
	/// 这2天在车型频道的api项目 Cooperation 目录里加个接口吧，需求如下
    ///	根据子品牌ID，取车型相关数据 json输出 支持jsonp 
	///	接口格式:json
	///	数据内容：
	///	车款名称（与综述页规则组合显示规则相同），车款链接地址，排量，变速器，指导价，车款计算器链接；
	///	数据与子品牌综述页保持一致，排序和划分方式都一致。
	/// </summary>
	public class GetCarList : IHttpHandler
	{
		private string c_ReturnEmpty = string.Empty;

		private string _callback = null;
		private int _serialId;

		private HttpRequest request;
		private HttpResponse response;

		private PageBase _pageBase;
		protected SerialEntity _serialEntity;
		private List<EnumCollection.CarInfoForSerialSummary> _serialCarList;

		public void ProcessRequest(HttpContext context)
		{
			CacheManager.SetPageCache(30);

			request = context.Request;
			response = context.Response;

			GetParam();

			string result = GetCarListJson();
			if (!string.IsNullOrEmpty(_callback))
			{
				result = string.Format("{0}({1});", _callback, result);
			}

			response.ContentType = "application/x-javascript";
			response.Write(result);
			response.End();
		}
		
		private void GetParam()
		{
			_serialId = ConvertHelper.GetInteger(request.QueryString["csid"]);

			if (!string.IsNullOrEmpty(request.QueryString["callback"]) 
				&& request.QueryString["callback"].Trim().Length > 0)
				_callback = request.QueryString["callback"].Trim();
		}
		private string GetCarListJson()
		{
			if (_serialId < 1)
				return c_ReturnEmpty;

			string cacheKey = "Api_Cooperation_GetCarDataJson_" + _serialId.ToString();
			object result = CacheManager.GetCachedData(cacheKey);
			if (result == null)
			{
				string data = GetCarListData();
				if (string.IsNullOrEmpty(data))
					data = c_ReturnEmpty;
				result = data;
				CacheManager.InsertCache(cacheKey, result, 30);
			}
			return (string)result;
		}

		private string GetCarListData()
		{
			_serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, _serialId);
			if (_serialEntity == null || _serialEntity.Id < 1)
				return null;

			if (_serialEntity.SaleState == "停销")//子品牌停销状态返回空
				return null;

			StringBuilder json = new StringBuilder();

			#region json格式
			/*
			{
			    id:2370,
			    allspall:"langyi",
			    cars:[
			        {
			            pl:"1.6L",
			            ls:[
			                {
			                    id:11767,
			                    name:"2011款 朗逸 1.6L 自动品雅版",
			                    refp:"13.48万",
			                    trans:"自动",
			                    cuttax:"减免",
			                    subsidy:true,
			                    pstate:"停产"
			                }
			            ]
			        }
			    ]
			} 
			*/
			 #endregion

			string serialShowName = _serialEntity.ShowName;
			string csSpell = _serialEntity.AllSpell;

			json.Append("{").AppendFormat("id:{0},allspell:\"{1}\",cars:[", _serialId.ToString(), csSpell);

			_pageBase = new PageBase();

			// modified by chengl Oct.11.2011
			List<EnumCollection.CarInfoForSerialSummary> ls = new List<EnumCollection.CarInfoForSerialSummary>();
			List<EnumCollection.CarInfoForSerialSummary> saleCarInfo = new List<EnumCollection.CarInfoForSerialSummary>();
			//if (_serialEntity.SaleState == "停销")
			//{
			//    // 停销子品牌取 停销子品牌最新年款的车型(高总逻辑 Oct.11.2011)
			//    ls = _pageBase.GetAllCarInfoForNoSaleSerialSummaryByCsID(_serialId);
			//}
			//else
			//{
				// 非停销子品牌取 子品牌的非停销所有年款车型
				ls = _pageBase.GetAllCarInfoForSerialSummaryByCsID(_serialId);
			//}
			saleCarInfo = _pageBase.GetAllCarInfoForSerialSummaryByCsID(_serialId, true);
			ls.Sort(NodeCompare.CompareCarByExhaust);
			//排量列表
			List<string> exhaustList = new List<string>();

			StringBuilder pls = new StringBuilder();
			StringBuilder cars = new StringBuilder();
			BitAuto.CarChannel.BLL.Car_BasicBll basicBll = new BitAuto.CarChannel.BLL.Car_BasicBll();
			foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
			{
				if (!exhaustList.Contains(carInfo.Engine_Exhaust))
				{
					if (cars.Length > 0)
						pls.Append(cars.Remove(0, 1).ToString()).Append("]}");
					cars.Length = 0;
					pls.Append(",").Append("{").AppendFormat("pl:\"{0}\",ls:[", carInfo.Engine_Exhaust);
					exhaustList.Add(carInfo.Engine_Exhaust);
				}

				string yearType = carInfo.CarYear.Trim();
				if (yearType.Length > 0)
					yearType += "款";
				else
					yearType = "未知年款";

				string carFullName = string.Empty;

				// modified by chengl Mar.27.2012
				// 客户要求将世嘉三厢更名为新世嘉
				if (yearType == "2012款" && serialShowName == "世嘉三厢")
				{ carFullName = "新世嘉 " + carInfo.CarName; }
				else
				{ carFullName = serialShowName + " " + carInfo.CarName; }

				if (carInfo.CarName.StartsWith(serialShowName))
					carFullName = serialShowName + " " + carInfo.CarName.Substring(serialShowName.Length);
				if (yearType != "未知年款")
					carFullName = yearType + " " + carFullName;

				bool isHasEnergySubsidy = new Car_BasicBll().CarHasParamEx(carInfo.CarID, 853);
				//============2012-04-09 减税============================
				Dictionary<int, string> dict = basicBll.GetCarAllParamByCarID(carInfo.CarID);
				string strTravelTax = string.Empty;
				if (dict.ContainsKey(895))
				{
					if (dict[895] == "减半")
						strTravelTax = "减半";
					else if (dict[895] == "免征")
						strTravelTax = "免征";
				}

				//变速器类型
				string tempTransmission = carInfo.TransmissionType;
				if (tempTransmission.IndexOf("挡") >= 0)
				{
					tempTransmission = tempTransmission.Substring(tempTransmission.IndexOf("挡") + 1, tempTransmission.Length - tempTransmission.IndexOf("挡") - 1);
				}
				tempTransmission = tempTransmission.Replace("变速器", string.Empty).Replace("CVT", string.Empty);

				//指导价
				string referPrice = string.Empty;
				if (carInfo.ReferPrice.Trim().Length == 0)
					referPrice = "未知";
				else
					referPrice = carInfo.ReferPrice + "万";

				string carPriceRange = string.Empty;
				if (carInfo.CarPriceRange.Trim().Length == 0)
					carPriceRange = "暂无报价";
				else
					carPriceRange = carInfo.CarPriceRange;

				cars.Append(",{").AppendFormat("id:{0},name:\"{1}\",refp:\"{2}\",price:\"{3}\",trans:\"{4}\",cuttax:\"{5}\",subsidy:{6},pstate:\"{7}\""
					, carInfo.CarID.ToString()
					, CommonFunction.GetUnicodeByString(carFullName)
					, CommonFunction.GetUnicodeByString(referPrice)
					, CommonFunction.GetUnicodeByString(carPriceRange)
					, CommonFunction.GetUnicodeByString(tempTransmission)
					, CommonFunction.GetUnicodeByString(strTravelTax)
					, isHasEnergySubsidy.ToString().ToLower()
					, CommonFunction.GetUnicodeByString(carInfo.ProduceState)
					).Append("}");
			}
			if (cars.Length > 0)
				pls.Append(cars.Remove(0, 1).ToString()).Append("]}");
			if (pls.Length > 0)
			{
				pls.Remove(0, 1);
			}
			return json.Append(pls.ToString()).Append("]}").ToString();
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