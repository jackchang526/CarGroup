using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.iphone
{
	public partial class SerialInfo : InterfacePageBase
	{
		private int csID = 0;
		protected EnumCollection.SerialInfoCard sic;	//子品牌名片
		private Car_SerialEntity cse;				//子品牌信息 
		private StringBuilder sb = new StringBuilder();
		private ArrayList m_CarIDlist = new ArrayList();
		private ArrayList m_CompareParameter = new ArrayList();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				GetSerialInfo();
				Response.Write(sb.ToString());
			}
		}

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string tempCsID = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(tempCsID, out csID))
				{
				}
			}
		}

		/// <summary>
		/// 取子品牌信息
		/// </summary>
		private void GetSerialInfo()
		{
			if (csID > 0)
			{
				cse = new Car_SerialBll().GetSerialInfoEntity(csID);
				sic = new Car_SerialBll().GetSerialInfoCard(csID);
				if (sic.CsID > 0 && cse.Cs_Id > 0)
				{
					sb.Append("{");
					sb.Append("CsID:\"" + cse.Cs_Id.ToString() + "\",");                   // 子品牌ID
					sb.Append("CsName:\"" + cse.Cs_Name.Trim() + "\",");                // 子品牌名
					sb.Append("PriceRange:\"" + sic.CsPriceRange.Trim() + "\",");      // 子品牌价格区间
					sb.Append("Exhaust:\"" + sic.CsEngineExhaust.Trim() + "\",");     // 子品牌排量
					sb.Append("FuelCost:\"" + sic.CsGuestFuelCost.Trim() + "\",");     // 子品牌油耗
					sb.Append("UnderPanType:\"" + sic.CsTransmissionType.Trim() + "\",");     // 子品牌变速器

					// 长宽高
					GetLastMarketDateCar();

					sb.Append("CsPic:\"" + sic.CsDefaultPic.Trim() + "\",");                             // 子品牌默认图
					sb.Append("CsVirtues:\"" + cse.Cs_Virtues.Trim() + "\",");                          // 子品牌优点
					sb.Append("CsDefect:\"" + cse.Cs_Defect.Trim() + "\",");                             // 子品牌缺点

					List<EnumCollection.CarInfoForSerialSummary> ls = base.GetAllCarInfoForSerialSummaryByCsID(sic.CsID);
					ls.Sort(NodeCompare.CompareCarByExhaust);

					sb.Append("OnSaleCars:[");
					int loop = 0;
					foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
					{
						if (loop != 0)
						{ sb.Append(","); }
						loop++;
						sb.Append("{CarID:\"" + carInfo.CarID.ToString() + "\",");
						sb.Append("CarName:\"" + carInfo.CarName.Trim() + "\",");
						sb.Append("CarReferPrice:\"" + carInfo.ReferPrice.Trim() + "\",");
						sb.Append("CarPriceRange:\"" + carInfo.CarPriceRange.Trim() + "\"}");
					}
					sb.Append("]");

					sb.Append("}");
				}
			}
		}

		private void GetLastMarketDateCar()
		{
			string sqlNewCar = " select top 1 cb.car_id from dbo.Car_Basic cb ";
			sqlNewCar += " left join dbo.Car_Extend_Item cei on cb.car_id = cei.car_id ";
			sqlNewCar += " where cb.cs_id = " + cse.Cs_Id.ToString() + " and  cei.Car_MarketDate = ";
			sqlNewCar += " (select Max(Car_MarketDate) from Car_Extend_Item ";
			sqlNewCar += " where Car_Id in (select car_id from dbo.Car_Basic where cs_id = " + cse.Cs_Id.ToString() + ")) ";
			sqlNewCar += " and cb.IsState=1 and cb.car_SaleState='在销' order by cei.car_id desc ";
			DataSet dsNewCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlNewCar);
			if (dsNewCar != null && dsNewCar.Tables.Count > 0 && dsNewCar.Tables[0].Rows.Count > 0)
			{
				int carIDtemp = int.Parse(dsNewCar.Tables[0].Rows[0]["car_id"].ToString());

				string OutSet_Length = new Car_BasicBll().GetCarParamEx(carIDtemp, 588);
				string OutSet_Width = new Car_BasicBll().GetCarParamEx(carIDtemp, 593);
				string OutSet_Height = new Car_BasicBll().GetCarParamEx(carIDtemp, 586);
				string OutSet_WheelBase = new Car_BasicBll().GetCarParamEx(carIDtemp, 592);

				sb.Append("OutSet_Length:\"" + OutSet_Length + "\",");
				sb.Append("OutSet_Width:\"" + OutSet_Width + "\",");
				sb.Append("OutSet_Height:\"" + OutSet_Height + "\",");
				sb.Append("OutSet_WheelBase:\"" + OutSet_WheelBase + "\",");
				// CarStorageAI carStorage = new CarStorageAI();
				// m_CarIDlist.Add(5985);
				//m_CarIDlist.Add(carIDtemp);
				//m_CompareParameter.Add("OutSet_Length");
				//m_CompareParameter.Add("OutSet_Width");
				//m_CompareParameter.Add("OutSet_Height");
				//m_CompareParameter.Add("OutSet_WheelBase");
				//Hashtable hsCar = carStorage.GetCarDataHash(m_CarIDlist, m_CompareParameter);

				//if (hsCar.ContainsKey(carIDtemp) && ((Hashtable)hsCar[carIDtemp]).ContainsKey("OutSet_Length"))
				//{
				//    sb.Append("OutSet_Length:\"" + ((Hashtable)hsCar[carIDtemp])["OutSet_Length"].ToString() + "mm\",");
				//}
				//else
				//{
				//    sb.Append("OutSet_Length:\"\",");
				//}
				//if (hsCar.ContainsKey(carIDtemp) && ((Hashtable)hsCar[carIDtemp]).ContainsKey("OutSet_Width"))
				//{
				//    sb.Append("OutSet_Width:\"" + ((Hashtable)hsCar[carIDtemp])["OutSet_Width"].ToString() + "mm\",");
				//}
				//else
				//{
				//    sb.Append("OutSet_Width:\"\",");
				//}
				//if (hsCar.ContainsKey(carIDtemp) && ((Hashtable)hsCar[carIDtemp]).ContainsKey("OutSet_Height"))
				//{
				//    sb.Append("OutSet_Height:\"" + ((Hashtable)hsCar[carIDtemp])["OutSet_Height"].ToString() + "mm\",");
				//}
				//else
				//{
				//    sb.Append("OutSet_Height:\"\",");
				//}
				//if (hsCar.ContainsKey(carIDtemp) && ((Hashtable)hsCar[carIDtemp]).ContainsKey("OutSet_WheelBase"))
				//{
				//    sb.Append("OutSet_WheelBase:\"" + ((Hashtable)hsCar[carIDtemp])["OutSet_WheelBase"].ToString() + "mm\",");
				//}
				//else
				//{
				//    sb.Append("OutSet_WheelBase:\"\",");
				//}
			}
		}
	}
}