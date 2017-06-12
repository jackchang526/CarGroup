using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;

using BitAuto.Beyond.Caching.RefreshCache;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.App_Code
{
	/// <summary>
	/// ServicesFor3DShow 的摘要说明
	/// </summary>
	[WebService(Namespace = "http://car.bitauto.com/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ServicesFor3DShow : System.Web.Services.WebService
	{

		public ServicesFor3DShow()
		{

			//如果使用设计的组件，请取消注释以下行 
			//InitializeComponent(); 
		}

		/// <summary>
		/// 获取城市子品牌的经销商报价
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="cityId"></param>
		/// <returns></returns>
		[WebMethod]
		public DataSet GetCarPrice(int serialId, int cityId)
		{
			return new Car_SerialBll().GetCarExtendInfoBySerial(serialId, cityId);
		}
		/// <summary>
		/// 获取子品牌的城市经销商
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="cityId"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		[WebMethod]
		public DataSet GetSerialCityVendors(int serialId, int cityId, int count)
		{
			RefreshCache rc = new RefreshCache();
			DataSet ds = rc.GetCacheData(cityId, serialId);

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > count)
			{
				for (int i = ds.Tables[0].Rows.Count - 1; i >= count; i--)
				{
					ds.Tables[0].Rows.RemoveAt(i);
				}
				if (!ds.Tables[0].Columns.Contains("vendorSaleAddr"))
				{
					ds.Tables[0].Columns.Add("vendorSaleAddr");
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						row["vendorSaleAddr"] = "";
					}
				}
				//取400电话
				ds.Tables[0].Columns.Add("Is400");
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					string str400 = new PageBase().GetDealerFor400(row["vendorID"].ToString());
					if (str400.Length > 0)
					{
						row["vendorTel"] = str400;
						row["Is400"] = true;
					}
					else
						row["Is400"] = false;
				}
				ds.AcceptChanges();
			}
			return ds;
		}

		/// <summary>
		/// 获取子品牌的城市促销信息
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="cityId"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		[WebMethod]
		public DataSet GetSerialCitySellNews(int serialId, int cityId, int count)
		{
			List<BitAuto.CarChannel.Model.News> newsList = new Car_SerialBll().GetSerialCityNews(serialId, cityId, "cuxiao");
			DataSet ds = new DataSet();
			DataTable dt = new DataTable();
			ds.Tables.Add(dt);
			dt.Columns.Add("news_Id");
			dt.Columns.Add("news_title");
			//dt.Columns.Add("VendorID");
			dt.Columns.Add("news_PubTime");
			dt.Columns.Add("url");

			int counter = 0;
			foreach (BitAuto.CarChannel.Model.News news in newsList)
			{
				DataRow row = dt.NewRow();
				row["news_Id"] = news.NewsId;
				row["news_title"] = news.Title;
				row["news_PubTime"] = news.PublishTime.ToString();
				row["url"] = news.PageUrl;
				dt.Rows.Add(row);
				counter++;
				if (counter == count)
					break;
			}
			ds.AcceptChanges();
			return ds;
		}

	}
}