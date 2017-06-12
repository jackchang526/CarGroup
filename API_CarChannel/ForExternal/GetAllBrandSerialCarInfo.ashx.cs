using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.DAL;
using BitAuto.Utils;
using System.Security;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// 获取所有品牌，子品牌，车型的一些基本信息的接口，此信息不会频繁变化，所以可缓存稍长时间
	/// </summary>
	public class GetAllBrandSerialCarInfo : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(100);
			context.Response.ContentType = "Text/XML";
			context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			context.Response.Write("<AllInfo>");
			ResponseBrandInfo(context.Response);
			ResponseSerialInfo(context.Response);
			ResponseCarInfo(context.Response);
			context.Response.Write("</AllInfo>");
			context.Response.End();
		}

		private void ResponseBrandInfo(HttpResponse response)
		{
			DataSet ds = new Car_BrandDal().GetAllBrand();
			if (ds != null && ds.Tables.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					int brandId = ConvertHelper.GetInteger(row["cb_Id"]);
					string brandName = ConvertHelper.GetString(row["cb_Name"]);
					if (brandId > 0 && !String.IsNullOrEmpty(brandName))
						response.Write("<Brand id=\"" + brandId + "\" name=\"" + SecurityElement.Escape(brandName) + "\" />");
				}
			}
		}
		private void ResponseSerialInfo(HttpResponse response)
		{
			DataSet ds = new Car_SerialDal().GetAllSerial();
			if (ds != null && ds.Tables.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					int serialId = ConvertHelper.GetInteger(row["cs_Id"]);
					int brandId = ConvertHelper.GetInteger(row["cb_Id"]);
					if (serialId == 0 || brandId == 0)
						continue;
					string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId, "1");
					response.Write("<Serial id=\"" + serialId + "\" brandId=\"" + brandId
						+ "\" name=\"" + SecurityElement.Escape(ConvertHelper.GetString(row["csName"]))
						+ "\" showName=\"" + SecurityElement.Escape(ConvertHelper.GetString(row["csShowName"]))
						+ "\" imageUrl=\"" + imgUrl + "\"/>");
				}
			}
		}
		private void ResponseCarInfo(HttpResponse response)
		{
			DataSet ds = new Car_BasicDal().GetAllCar();
			if (ds != null && ds.Tables.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					int carId = ConvertHelper.GetInteger(row["Car_Id"]);
					int serialId = ConvertHelper.GetInteger(row["cs_Id"]);
					if (carId == 0 || serialId == 0)
						continue;
					string carName = ConvertHelper.GetString(row["Car_Name"]);
					string referPrice = row["car_ReferPrice"] == DBNull.Value ? String.Empty : row["car_ReferPrice"].ToString();
					string carYear = row["Car_YearType"] == DBNull.Value ? String.Empty : row["Car_YearType"].ToString();
					string exhaus = row["exhaus"] == DBNull.Value ? String.Empty : row["exhaus"].ToString();
					response.Write("<Car id=\"" + carId + "\" serialId=\"" + serialId + "\" carName=\""
						+ SecurityElement.Escape(carName) + "\" referPrice=\"" + referPrice + "\" year=\"" + carYear + "\" exhaus=\"" + exhaus + "\" />");
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