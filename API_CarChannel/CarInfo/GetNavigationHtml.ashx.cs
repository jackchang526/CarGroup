using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.DAL.Data;
using System.Data;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannelAPI.Web.AppCode;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetNavigationHtml 的摘要说明
	/// car域名接口迁移 取子品牌、车型互联互通导航 无缓存 http://car.bitauto.com/CarService/GetSerialTop.aspx?csID=1660&tagName=csnewsnocrumb
	/// modified by chengl Feb.15.2015 add koubei youhao
	/// modified by chengl Feb.14.2016 add CsVideo
	/// </summary>
	public class GetNavigationHtml : PageBase, IHttpHandler
	{
		private HttpRequest request;
		private HttpResponse response;
		private string dept = "";
		private StringBuilder sb = new StringBuilder();
		string html = " ";

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/html";
			response = context.Response;
			request = context.Request;
			if (!string.IsNullOrEmpty(request.QueryString["dept"]))
			{ dept = request.QueryString["dept"].ToString().Trim().ToLower(); }

			switch (dept)
			{
				case "getserialnavigation": GetSerialNavigation(); break;
				case "getcarnavigation": GetCarNavigation(); break;
				default: ; break;
			}
			response.Write(html);
		}

		/// <summary>
		/// 子品牌导航
		/// </summary>
		private void GetSerialNavigation()
		{
			int csID = 0;
			string tarName = "";
			#region 取参数
			// 子品牌ID
			if (request.QueryString["csID"] != null && request.QueryString["csID"].ToString() != "")
			{
				string csIDStr = request.QueryString["csID"].ToString().Trim();
				if (int.TryParse(csIDStr, out csID))
				{ }
				else
				{
					csID = 0;
				}
			}
			// 标签类型
			if (request.QueryString["tagName"] != null && request.QueryString["tagName"].ToString() != "")
			{
				tarName = request.QueryString["tagName"].ToString().Trim().ToLower();
			}
			#endregion
			if (csID > 0 && tarName != "")
			{
				switch (tarName)
				{
					case "csprice": html = base.GetCommonNavigation("CsPrice", csID); break;
					case "csvideo": html = base.GetCommonNavigation("CsVideo", csID); break;
					case "csnewsnocrumb": html = base.GetCommonNavigation("CsCMSNews", csID); break;
					case "csucar": html = base.GetCommonNavigation("CsUcar", csID); break;
					case "csjiangjia": html = base.GetCommonNavigation("CsJiangJia", csID); break;
					case "cschedai": html = base.GetCommonNavigation("CsCheDai", csID); break;
					case "csbaoyang": html = base.GetCommonNavigation("CsMaintenance", csID); break;
					case "csxiaoliang": html = base.GetCommonNavigation("CsSellData", csID); break;
					case "cssummary": html = base.GetCommonNavigation("CsSummary", csID); break;
					case "cssummaryjs": html = base.GetCommonNavigation("CsSummaryJs", csID); break;
					case "csask": html = base.GetCommonNavigation("CsAsk", csID); break;
					case "cskoubei": html = base.GetCommonNavigation("CsKouBei", csID); break;
					case "csyouhao": html = base.GetCommonNavigation("CsYouHao", csID); break;
					//add by sk 2016-04-08 移动站 导航头
					case "mcsphoto": html = base.GetCommonNavigation("MCsPhoto", csID); break;
					case "mcsprice": html = base.GetCommonNavigation("MCsPrice", csID); break;
					case "mcsbaoyang": html = base.GetCommonNavigation("MCsMaintenance", csID); break;
					case "mcskoubei": html = base.GetCommonNavigation("MCsKouBei", csID); break;
					case "mcsyouhao": html = base.GetCommonNavigation("MCsYouHao", csID); break;
					case "mcsdealer": html = base.GetCommonNavigation("MCsDealer", csID); break;
                    case "mcsvideo": html = base.GetCommonNavigation("MCsVideo", csID); break;
                    default: ; break;
				}
			}
		}

		/// <summary>
		/// 车型导航
		/// </summary>
		private void GetCarNavigation()
		{
			int carID = 0;
			string tarName = "";
			#region 取参数
			// 子品牌ID
			if (request.QueryString["carID"] != null && request.QueryString["carID"].ToString() != "")
			{
				string carIDStr = request.QueryString["carID"].ToString().Trim();
				if (int.TryParse(carIDStr, out carID))
				{ }
				else
				{
					carID = 0;
				}
			}
			// 标签类型
			if (request.QueryString["tagName"] != null && request.QueryString["tagName"].ToString() != "")
			{
				tarName = request.QueryString["tagName"].ToString().Trim().ToLower();
			}
			#endregion
			if (carID > 0 && tarName != "")
			{
				switch (tarName)
				{
					case "carprice": html = base.GetCommonNavigation("CarPrice", carID); break;
					case "carucar": html = base.GetCommonNavigation("CarUcar", carID); break;
					case "carjiangjia": html = base.GetCommonNavigation("CarJiangJia", carID); break;
					case "carchedai": html = base.GetCommonNavigation("CarCheDai", carID); break;
					default: ; break;
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