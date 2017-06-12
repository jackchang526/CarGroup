using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 取车型的购车总价、车型频道URL(UCAR 杨晓波)
	/// </summary>
	public partial class GetCarPriceAndParamLink : InterfacePageBase
	{
		private int carID = 0;
		// private EnumCollection.CarInfoForCarSummary cfcs = new EnumCollection.CarInfoForCarSummary();
		protected Car_BasicEntity cbe = new Car_BasicEntity();
		//private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				Response.Write("<CarInfo>");
				GetPageParam();
				GetCarData();
				Response.Write("</CarInfo>");
				//Response.Write(sb.ToString());
			}
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["carID"] != null && this.Request.QueryString["carID"].ToString() != "")
			{
				string tempCarID = this.Request.QueryString["carID"].ToString();
				if (int.TryParse(tempCarID, out carID))
				{ }
			}
		}

		private void GetCarData()
		{
			if (carID > 0)
			{
				string catchkeyCar = "CarSummary_Car" + carID.ToString();
				object carInfoByCarID = null;
				CacheManager.GetCachedData(catchkeyCar, out carInfoByCarID);
				if (carInfoByCarID == null)
				{
					cbe = (new Car_BasicBll()).Get_Car_BasicByCarID(carID);
					CacheManager.InsertCache(catchkeyCar, cbe, 60);
				}
				else
				{
					cbe = (Car_BasicEntity)carInfoByCarID;
				}

				// 停销的车型返回0 Aug.3.2010
				if (cbe.Car_SaleState.Trim() == "停销")
				{
					Response.Write("<CarTotalPrice>0</CarTotalPrice>");
				}
				else
				{
					CarPriceComputer priceComputer = new CarPriceComputer(carID);
					priceComputer.ComputeCarPrice();
					Response.Write("<CarTotalPrice>" + priceComputer.FormatTotalPrice.Trim().Replace("万", "") + "</CarTotalPrice>");
				}
				Response.Write("<CarParamURL>http://car.bitauto.com/" + cbe.Cs_AllSpell.ToLower().Trim() + "/m" + cbe.Car_Id.ToString().Trim() + "/peizhi/</CarParamURL>");
				Response.Write("<CarSummaryURL>http://car.bitauto.com/" + cbe.Cs_AllSpell.ToLower().Trim() + "/m" + cbe.Car_Id.ToString().Trim() + "/</CarSummaryURL>");
				Response.Write("<CarKoubeiURL>http://car.bitauto.com/" + cbe.Cs_AllSpell.ToLower().Trim() + "/m" + cbe.Car_Id.ToString().Trim() + "/koubei/</CarKoubeiURL>");
				Response.Write("<CarYouhaoURL>http://car.bitauto.com/" + cbe.Cs_AllSpell.ToLower().Trim() + "/m" + cbe.Car_Id.ToString().Trim() + "/youhao/</CarYouhaoURL>");
				if (cbe.Car_SaleState.Trim() == "停销")
				{
					Response.Write("<CarPriceURL></CarPriceURL>");
				}
				else
				{
					Response.Write("<CarPriceURL>http://price.bitauto.com/frame.aspx?newcarid=" + cbe.Car_Id.ToString().Trim() + "</CarPriceURL>");
				}
				Response.Write("<CarRepairPolicy>" + GetCarRepairPolicyByCarID(carID) + "</CarRepairPolicy>");
			}
		}

		/// <summary>
		/// 取保修政策
		/// </summary>
		/// <param name="carID">车型ID</param>
		/// <returns></returns>
		private string GetCarRepairPolicyByCarID(int carID)
		{
			string RepairPolicy = "";
			DataSet ds = new DataSet();
			string catchkey = "Car_BasicBll_GetCarParamEx_398";
			object carRepairPolicy = null;
			CacheManager.GetCachedData(catchkey, out carRepairPolicy);
			if (carRepairPolicy == null)
			{
				ds = new Car_BasicBll().GetCarParamEx(398);
				CacheManager.InsertCache(catchkey, ds, 60);
			}
			else
			{
				ds = (DataSet)carRepairPolicy;
			}
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = ds.Tables[0].Select(" car_id='" + carID.ToString() + "' ");
				if (drs != null && drs.Length > 0)
				{
					RepairPolicy = drs[0]["pvalue"].ToString().Trim().Replace("&", "").Replace("<", "").Replace(">", "");
				}
			}
			return RepairPolicy;
		}

		private void WriteIpHistory()
		{
			string ipStr = WebUtil.GetClientIP();
			string sDir = AppDomain.CurrentDomain.BaseDirectory + "log\\";
			// string sDir = "E:\\wwwroot\\AutoChannel\\log\\";
			try
			{
				if (!System.IO.Directory.Exists(sDir))
				{
					System.IO.Directory.CreateDirectory(sDir);
				}
				using (StreamWriter sw = new StreamWriter(sDir + "GetCarPriceAndParamLink.txt", true))
				{
					sw.Write(ipStr + ":refer:" + Request.UrlReferrer + "\r\n");
					sw.Close();
				}
			}
			catch
			{ }
		}
	}
}