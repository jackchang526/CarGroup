using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.iphone
{
	/// <summary>
	/// iphone 子品牌名片基本信息+旗下车型按年款分(杨立锋)
	/// </summary>
	public partial class SerialInfoAndCar : InterfacePageBase
	{
		private int csID = 0;
		private StringBuilder sb = new StringBuilder();
		protected EnumCollection.SerialInfoCard sic;	//子品牌名片
		private Car_SerialEntity cse;				//子品牌信息 
		private bool isAllSaleCar = false;		// 是否包含停销车型
		private DataSet dsCar;
		private List<string> listcsEE = new List<string>();

		/// <summary>
		/// iPhone  数据接口 子品牌及旗下车型 按年款分(杨立峰)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.AppendLine("<SerialBasicInfo>");
				GetPageParam();
				// 取子品牌的车型，在销或者包括停销
				dsCar = GetCarIDAndNameForCS(csID, isAllSaleCar);
				if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
				{
					foreach(DataRow dr in dsCar.Tables[0].Rows)
					{
						string carEE = dr["Engine_Exhaust"].ToString();
						if (!listcsEE.Contains(carEE))
						{ listcsEE.Add(carEE); }
					}
				}

				GetSerialBasicInfo();
				GetSerialCar();
				GetSerialColorList();
				sb.AppendLine("</SerialBasicInfo>");
				Response.Write(sb.ToString());
			}
		}


		private void GetPageParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string strCSID = this.Request.QueryString["csID"].ToString().Trim();
				if (int.TryParse(strCSID, out csID))
				{ }
			}
			if (this.Request.QueryString["isAllSaleCar"] != null && this.Request.QueryString["isAllSaleCar"].ToString() == "1")
			{
				isAllSaleCar = true;
			}
		}

		private void GetSerialBasicInfo()
		{
			if (csID > 0)
			{
				sic = new Car_SerialBll().GetSerialInfoCard(csID);
				cse = new Car_SerialBll().GetSerialInfoEntity(csID);
				if (sic.CsID != 0)
				{
					sb.AppendLine("<Serial ID=\"" + sic.CsID.ToString() + "\" ");
					sb.Append(" Name=\"" + sic.CsName.Trim() + "\" ");
					sb.Append(" ShowName=\"" + sic.CsShowName.Trim() + "\" ");
					sb.Append(" CsPic=\"" + (sic.CsDefaultPic == "http://image.bitautoimg.com/autoalbum/V2.1/images/150-100.gif" ? "" : sic.CsDefaultPic) + "\" ");
					sb.Append(" CsLevel=\"" + cse.Cs_CarLevel.Trim() + "\" ");
					sb.Append(" CbName=\"" + cse.Cb_Name.Trim() + "\" ");
					sb.Append(" AllSpell=\"" + sic.CsAllSpell.Trim().ToLower() + "\" ");
					sb.Append(" CsPriceRange=\"" + sic.CsPriceRange.Trim() + "\" ");
					sb.Append(" CsTransmissionType=\"" + sic.CsTransmissionType.Trim() + "\" ");
					// sb.Append(" EngineExhaust=\"" + sic.CsEngineExhaust.Trim() + "\" ");
					string csEE = "";
					if (listcsEE.Count > 0)
					{
						foreach (string carEE in listcsEE)
						{
							if (csEE != "")
							{ csEE += "、"; }
							csEE += carEE;
						}
					}
					sb.Append(" EngineExhaust=\"" + csEE + "\" ");
					sb.Append(" CsOfficialFuelCost=\"" + sic.CsOfficialFuelCost.Trim() + "\" ");
					sb.Append(" CsGuestFuelCost=\"" + sic.CsGuestFuelCost.Trim() + "\" ");
					sb.Append(" CsKouBeiCount=\"" + sic.CsDianPingCount.ToString() + "\" ");
					sb.Append(" CsPicCount=\"" + sic.CsPicCount.ToString() + "\" ");

					sb.Append(" CsReferPrice=\"" + GetSerialReferPriceByCsID(sic.CsID) + "\" ");
					//  取子品牌 加速时间（0—100km/h）、制动距离（100—0km/h）、油耗 数据 (786\787\788)
					DataSet dsCsDataInfo = GetCsDataInfo(sic.CsID);
					if (dsCsDataInfo != null && dsCsDataInfo.Tables.Count > 0 && dsCsDataInfo.Tables[0].Rows.Count > 0)
					{
						sb.Append(" CsMA=\"" + System.Security.SecurityElement.Escape(dsCsDataInfo.Tables[0].Rows[0]["p1"].ToString()) + "\" ");
						sb.Append(" CsBD=\"" + System.Security.SecurityElement.Escape(dsCsDataInfo.Tables[0].Rows[0]["p2"].ToString()) + "\" ");
						sb.Append(" CsMF=\"" + System.Security.SecurityElement.Escape(dsCsDataInfo.Tables[0].Rows[0]["p3"].ToString()) + "\" ");
					}
					else
					{
						sb.Append(" CsMA=\"\" ");
						sb.Append(" CsBD=\"\" ");
						sb.Append(" CsMF=\"\" ");
					}
					sb.Append(" CsVirtues=\"" + cse.Cs_Virtues.Trim() + "\" ");
					sb.Append(" CsDefect=\"" + cse.Cs_Defect.Trim() + "\" />");
				}
			}
		}

		private void GetSerialCar()
		{
			// DataSet dsCar = base.GetCarIDAndNameForCS(csID, WebConfig.CachedDuration);
			// DataSet dsCar = GetCarIDAndNameForCS(csID, isAllSaleCar);
			if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
			{
				sb.AppendLine("<CarList>");
				string carYear = "";
				bool hasYear = false;
				for (int i = 0; i < dsCar.Tables[0].Rows.Count; i++)
				{
					if (dsCar.Tables[0].Rows[i]["Car_YearType"].ToString() == "")
					{
						continue;
					}
					hasYear = true;
					if (carYear == "")
					{
						// 第一个年款
						carYear = dsCar.Tables[0].Rows[i]["Car_YearType"].ToString();
						sb.AppendLine("<CarYear Year=\"" + carYear + "\" >");
					}
					else
					{
						if (dsCar.Tables[0].Rows[i]["Car_YearType"].ToString() != "")
						{
							// 不同年款
							if (carYear != dsCar.Tables[0].Rows[i]["Car_YearType"].ToString())
							{
								carYear = dsCar.Tables[0].Rows[i]["Car_YearType"].ToString();
								sb.AppendLine("</CarYear>");
								sb.AppendLine("<CarYear Year=\"" + carYear + "\" >");
							}
						}
					}
					sb.AppendLine("<Car CarID=\"" + dsCar.Tables[0].Rows[i]["Car_id"].ToString() + "\" ");
					sb.Append(" CarName=\"" + dsCar.Tables[0].Rows[i]["Car_name"].ToString().Trim().Replace("\"", "“").Replace("&", "＆") + "\" ");
					sb.Append(" ReferPrice=\"" + dsCar.Tables[0].Rows[i]["car_ReferPrice"].ToString().Trim() + "\" ");
					// if(dsCar.Tables[0].Rows[i][""])
					// 车型的环保补贴
					bool isHasEnergySubsidy = new Car_BasicBll().CarHasParamEx(int.Parse(dsCar.Tables[0].Rows[i]["Car_id"].ToString()), 853);
					if (isHasEnergySubsidy)
					{
						sb.Append(" EnergySubsidy=\"可获得3000元节能补贴\" ");
					}
					else
					{
						sb.Append(" EnergySubsidy=\"无\" ");
					}
					sb.Append("/>");
				}
				if (hasYear)
				{
					sb.AppendLine("</CarYear>");
				}
				sb.AppendLine("</CarList>");
			}
		}

		/// <summary>
		/// 子品牌的在销车型
		/// </summary>
		/// <param name="csid"></param>
		/// <returns></returns>
		private string GetSerialReferPriceByCsID(int csid)
		{
			string serialReferPrice = "";
			List<EnumCollection.CarInfoForSerialSummary> ls = base.GetAllCarInfoForSerialSummaryByCsID(csid);
			double maxPrice = Double.MinValue;
			double minPrice = Double.MaxValue;
			foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
			{
				double referPrice = 0.0;
				bool isDouble = Double.TryParse(carInfo.ReferPrice.Replace("万", ""), out referPrice);
				if (isDouble)
				{
					if (referPrice > maxPrice)
						maxPrice = referPrice;
					if (referPrice < minPrice)
						minPrice = referPrice;
				}
			}

			if (maxPrice == Double.MinValue && minPrice == Double.MaxValue)
				serialReferPrice = "暂无";
			else
			{
				serialReferPrice = minPrice + "万-" + maxPrice + "万";
			}
			return serialReferPrice;
		}

		/// <summary>
		/// 取子品牌颜色
		/// </summary>
		private void GetSerialColorList()
		{
			sb.AppendLine("<SerialColor>");
			DataSet dsColor = new Car_SerialBll().GetAllSerialColorRGB();
			if (dsColor != null && dsColor.Tables.Count > 0 && dsColor.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = dsColor.Tables[0].Select(" cs_id='" + csID.ToString() + "' ");
				if (drs != null && drs.Length > 0)
				{
					foreach (DataRow dr in drs)
					{
						if (sic.ColorList.Contains(dr["colorName"].ToString().Trim()))
						{
							sb.AppendLine("<Color Name=\"" + dr["colorName"].ToString().Trim().Replace("&", "＆") + "\" RGB=\"" + dr["colorRGB"].ToString().ToUpper().Trim() + "\" />");
						}
					}
				}
			}
			sb.AppendLine("</SerialColor>");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="csID">子品牌ID</param>
		/// <param name="isAllSaleCar">是否包含停销车型</param>
		/// <returns></returns>
		private DataSet GetCarIDAndNameForCS(int csID, bool isAllSaleCar)
		{
			string sql = " select car.car_id,car.car_name,cs.cs_id,cs_name,car.Car_YearType,car.car_ReferPrice,cei.UnderPan_TransmissionType as TransmissionValue ";
			sql += " ,(case when cei.UnderPan_TransmissionType like '%手动' then 1 ";
			sql += " when cei.UnderPan_TransmissionType like '%自动' then 2 ";
			sql += " when cei.UnderPan_TransmissionType like '%手自一体' then 3 ";
			sql += " when cei.UnderPan_TransmissionType like '%半自动' then 4 ";
			sql += " when cei.UnderPan_TransmissionType like '%CVT无级变速' then 5 ";
			sql += " when cei.UnderPan_TransmissionType like '%双离合' then 6 ";
			sql += " else 9 end) as TransmissionType,cei.Engine_Exhaust ";
			sql += " from dbo.Car_Basic car ";
			sql += " left join dbo.Car_Extend_Item cei on car.car_id = cei.car_id ";
			sql += " left join Car_Serial cs on car.cs_id = cs.cs_id ";
			sql += " where car.isState=1 and cs.isState=1 and cs.cs_id = @csID {0} ";
			sql += " order by car.Car_YearType desc,cei.Engine_Exhaust,TransmissionType,car.car_ReferPrice ";
			SqlParameter[] _param ={
                                      new SqlParameter("@csID",SqlDbType.Int)
                                  };
			_param[0].Value = csID;
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, string.Format(sql, (isAllSaleCar ? "" : " and car.Car_SaleState<>'停销' ")), _param);
			return ds;
		}

		/// <summary>
		/// 取子品牌 加速时间（0—100km/h）、制动距离（100—0km/h）、油耗 数据 (786\787\788)
		/// </summary>
		/// <param name="csid"></param>
		/// <returns></returns>
		private DataSet GetCsDataInfo(int csid)
		{
			string sql = @"select car.car_id,cdb1.pvalue as p1,cdb2.pvalue as p2,cdb3.pvalue as p3
								from car_relation car
								left join cardatabase cdb1 on car.car_id=cdb1.carid and cdb1.paramid=786
								left join cardatabase cdb2 on car.car_id=cdb2.carid and cdb2.paramid=787
								left join cardatabase cdb3 on car.car_id=cdb3.carid and cdb3.paramid=788
								where car.isstate=0 and car.cs_id = {0} and cdb1.pvalue<>''
								and cdb2.pvalue<>'' and cdb3.pvalue<>''";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString,
				CommandType.Text, string.Format(sql, csid));
			return ds;
		}

	}
}