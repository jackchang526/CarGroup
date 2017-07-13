using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetCarListWithTax 的摘要说明
	/// </summary>
	public class GetCarListWithTax : PageBase,IHttpHandler
	{
		private int serialId = 0;
		private int saleState = 0;//车款销售状态：0：所有，1：在销
		private string callback = string.Empty;
		private string retStr = "{{\"result\":\"{0}\",\"msg\":\"{1}\",\"serial\":{3},\"carList\":[{2}]}}";
		private string csStr = "{{\"csId\":\"{0}\",\"csName\":\"{1}\",\"price\":\"{2}\"}}";
		private string carStr = "{{\"carId\":\"{0}\",\"carName\":\"{1}\",\"yearType\":\"{2}\",\"price\":\"{3}\",\"referPrice\":\"{4}\",\"tax\":\"{5}\"}}";
		private Car_BasicBll carBasicBll = new Car_BasicBll();
		private Car_SerialBll _serialBLL = new Car_SerialBll();

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(60);
			context.Response.ContentType = "application/json; charset=utf-8";
			GetParam(context);
			RenderCarList(context);
		}

		private void RenderCarList(HttpContext context)
		{
			if (serialId <= 0)
			{
				context.Response.Write(string.Format(retStr, "failure", "参数错误", string.Empty, string.Empty));
				context.Response.End();
			}

			SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
			if (serialEntity == null || serialEntity.Id == 0)
			{
				context.Response.Write(string.Format(retStr, "failure", "参数错误", string.Empty, string.Empty));
				context.Response.End();
			}
			EnumCollection.SerialInfoCard serialInfo = _serialBLL.GetSerialInfoCard(serialId);
			List<CarInfoForSerialSummaryEntity> serialCarList = carBasicBll.GetCarInfoForSerialSummaryBySerialId(serialId);
			if (serialCarList == null || serialCarList.Count ==0)
			{
				context.Response.Write(string.Format(retStr, "failure", "车款列表为空", string.Empty, string.Empty));
				context.Response.End();
			}
			List<CarInfoForSerialSummaryEntity> carinfoSaleList;
			if (saleState == 1)
			{
				carinfoSaleList = serialCarList.FindAll(p => p.SaleState == "在销");
			}
			else
			{
				carinfoSaleList = serialCarList;
			}
			carinfoSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);

			var carids = carinfoSaleList.ToArray().Select(p => p.CarID);
			Dictionary<int, string> subTaxDic = carBasicBll.GetSubTaxByCarIds(carids.ToList());

			List<string> retList = new List<string>();
			foreach (CarInfoForSerialSummaryEntity carInfo in carinfoSaleList)
			{
				string carMinPrice = string.Empty;
				string carPriceRange = carInfo.CarPriceRange;
				string tax = subTaxDic != null && subTaxDic.ContainsKey(carInfo.CarID) ? subTaxDic[carInfo.CarID] : string.Empty;
				if (carInfo.SaleState == "待销")//顾晓 确认的逻辑 （待销的车款没有价格，全部显示未上市） 2015-07-09
				{
					carMinPrice = "未上市";
				}
				else if (carInfo.CarPriceRange.Trim().Length == 0)
				{
					carMinPrice = "暂无报价";
				}
				else
				{
					if (carPriceRange.IndexOf('-') != -1)
						carMinPrice = carPriceRange.Substring(0, carPriceRange.IndexOf('-')); //+ "万"
					else
						carMinPrice = carPriceRange;
				}
				retList.Add(string.Format(carStr, carInfo.CarID, carInfo.CarName, carInfo.CarYear, carMinPrice, carInfo.ReferPrice, tax));
			}
			retStr = string.Format(retStr, "success", string.Empty, string.Join(",", retList.ToArray()),
				string.Format(csStr, serialId, serialEntity.ShowName, serialInfo.CsPriceRange.Length == 0 ? "暂无报价" : serialInfo.CsPriceRange));
			if (!string.IsNullOrWhiteSpace(callback))
			{
				context.Response.Write(string.Format("{0}({1})", callback, retStr));
			}
			else
			{
				context.Response.Write(retStr);
			}
			context.Response.End();
		}

		private void GetParam(HttpContext context)
		{
			serialId = ConvertHelper.GetInteger(context.Request["csid"]);
			saleState = ConvertHelper.GetInteger(context.Request["salestate"]);
			callback = context.Request["callback"];
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