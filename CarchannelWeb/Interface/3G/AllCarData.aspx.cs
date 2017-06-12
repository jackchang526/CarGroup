using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;
using System.Security;

namespace BitAuto.CarChannel.CarchannelWeb.Interface._3G
{
	/// <summary>
	/// 3G合作数据 (张冬冬)
	/// </summary>
	public partial class AllCarData : InterfacePageBase
	{
		private DataSet dsCp = new DataSet();
		private DataSet dsBs = new DataSet();
		private DataSet dsCb = new DataSet();
		private DataSet dsCs = new DataSet();
		private DataSet dsCar = new DataSet();
		private StringBuilder sb = new StringBuilder();

		private Hashtable csHasNew = new Hashtable();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.GetAllData();
				this.GenerateXMLContentFromData();
				Response.Write(sb.ToString());
			}
		}

		private void GetAllData()
		{
			// 厂商
			string sqlCp = @"select cp.Cp_Id,cp.Cp_Name,cp.CpShortName, 
(case CpCountry when 90 then '国产' else '进口' end) as CpCountry
from dbo.Car_producer cp
where cp.isState=0";
			dsCp = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlCp);

			// 主品牌
			string sqlBs = "select bs_Id,bs_Name,spell from dbo.Car_MasterBrand where isState=0";
			dsBs = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlBs);

			// 品牌
			string sqlCb = " select cb.cb_Id,cb.cb_Name,cb.cp_Id,cmb.bs_id ";
			sqlCb += " from dbo.Car_Brand cb ";
			sqlCb += " left join dbo.Car_MasterBrand_Rel cmb on cb.cb_id = cmb.cb_id and cmb.isState=0 ";
			sqlCb += " where cb.isState=0 ";
			dsCb = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlCb);

			// 子品牌
			string sqlCs = "select carimpot,cs_id,cb_id,csname,cs_carlevel,minReferPrice,maxReferPrice from dbo.VCar_ForBitAuto ";
			dsCs = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlCs);

			// 车型
			string sqlCar = " select car.car_id,car.car_name,car.cs_id,car.Car_YearType,  ";
			sqlCar += " cdb1.pvalue as EngineExhaust,cdb2.pvalue as MarketYear,cdb3.pvalue as MarketMonth, ";
			sqlCar += " cdb5.pvalue as TransmissionType,cdb6.pvalue as RepairPolicy, ";
			sqlCar += " cl.classvalue as SaleState, car.car_ReferPrice,cdb7.pvalue as ForwardGearNum  ";
			sqlCar += " from dbo.Car_relation car ";
			sqlCar += " left join car_serial cs on car.cs_id = cs.cs_id ";
			// sqlCar += " left join dbo.CarDataBase cdb1 on car.car_id = cdb1.carid and cdb1.paramid=423 ";
			sqlCar += " left join dbo.CarDataBase cdb1 on car.car_id = cdb1.carid and cdb1.paramid=785 ";
			sqlCar += " left join dbo.CarDataBase cdb2 on car.car_id = cdb2.carid and cdb2.paramid=385 ";
			sqlCar += " left join dbo.CarDataBase cdb3 on car.car_id = cdb3.carid and cdb3.paramid=384 ";
			sqlCar += " left join dbo.CarDataBase cdb4 on car.car_id = cdb4.carid and cdb4.paramid=383 ";
			sqlCar += " left join dbo.CarDataBase cdb5 on car.car_id = cdb5.carid and cdb5.paramid=712 ";
			sqlCar += " left join dbo.CarDataBase cdb6 on car.car_id = cdb6.carid and cdb6.paramid=398 ";
			sqlCar += " left join dbo.CarDataBase cdb7 on car.car_id = cdb7.carid and cdb7.paramid=724 ";
			sqlCar += " left join class cl on car.car_SaleState = cl.classid ";
			sqlCar += " where car.isState=0 and cs.isState=0 order by car_id ";
			dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlCar);
		}

		private void GenerateXMLContentFromData()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<Data>");

			// 厂商
			sb.Append("<Producer>");
			if (dsCp != null && dsCp.Tables.Count > 0 && dsCp.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsCp.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Cp CpID=\"" + dsCp.Tables[0].Rows[i]["Cp_Id"].ToString() + "\" ");
					sb.Append(" CpName=\"" + SecurityElement.Escape(dsCp.Tables[0].Rows[i]["Cp_Name"].ToString()) + "\" ");
					sb.Append(" CpCountry=\"" + dsCp.Tables[0].Rows[i]["CpCountry"].ToString() + "\" />");
				}
			}
			sb.Append("</Producer>");

			// 主品牌
			sb.Append("<Master>");
			if (dsBs != null && dsBs.Tables.Count > 0 && dsBs.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsBs.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Bs BsID=\"" + dsBs.Tables[0].Rows[i]["bs_Id"].ToString() + "\" ");
					sb.Append(" BsName=\"" + SecurityElement.Escape(dsBs.Tables[0].Rows[i]["bs_Name"].ToString()) + "\" ");
					sb.Append(" BsSpell=\"" + dsBs.Tables[0].Rows[i]["spell"].ToString().ToLower() + "\" />");
				}
			}
			sb.Append("</Master>");

			// 品牌
			sb.Append("<Brand>");
			if (dsCb != null && dsCb.Tables.Count > 0 && dsCb.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsCb.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Cb CbID=\"" + dsCb.Tables[0].Rows[i]["cb_Id"].ToString() + "\" ");
					sb.Append(" CbName=\"" + SecurityElement.Escape(dsCb.Tables[0].Rows[i]["cb_Name"].ToString()) + "\" ");
					sb.Append(" CpID=\"" + dsCb.Tables[0].Rows[i]["cp_Id"].ToString() + "\" ");
					sb.Append(" BsID=\"" + dsCb.Tables[0].Rows[i]["bs_id"].ToString() + "\" />");
				}
			}
			sb.Append("</Brand>");

			// 子品牌
			GetSerialHasNewCar();
			sb.Append("<Serial>");
			if (dsCs != null && dsCs.Tables.Count > 0 && dsCs.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsCs.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Cs CsID=\"" + dsCs.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					sb.Append(" CsName=\"" + SecurityElement.Escape(dsCs.Tables[0].Rows[i]["csName"].ToString()) + "\" ");
					string csPic = "";
					int count = 0;
					base.GetSerialPicAndCountByCsID(int.Parse(dsCs.Tables[0].Rows[i]["cs_id"].ToString()), out csPic, out count, false);
					sb.Append(" Pic=\"" + csPic.Replace("_2.", "_6.") + "\" ");
					sb.Append(" CbID=\"" + dsCs.Tables[0].Rows[i]["cb_id"].ToString() + "\" ");
					sb.Append(" CsLevel=\"" + dsCs.Tables[0].Rows[i]["cs_carlevel"].ToString() + "\" ");
					sb.Append(" CsPriceRange=\"" + GetSerialPriceRByID(int.Parse(dsCs.Tables[0].Rows[i]["cs_id"].ToString())) + "\" ");
					sb.Append(" CsEngineExhaust=\"" + GetSerialEngineExhaust(int.Parse(dsCs.Tables[0].Rows[i]["cs_id"].ToString())) + "\" ");
					sb.Append(" MultiPriceRange=\"" + GetPriceRankMult(dsCs.Tables[0].Rows[i]["minReferPrice"].ToString(), dsCs.Tables[0].Rows[i]["maxReferPrice"].ToString()) + "\" ");
					sb.Append(" CsCountry=\"" + dsCs.Tables[0].Rows[i]["carimpot"].ToString() + "\" ");
					if (csHasNew != null && csHasNew.Count > 0 && csHasNew.ContainsKey(dsCs.Tables[0].Rows[i]["cs_id"].ToString()))
					{
						sb.Append(" CsHasNew=\"1\" ");
					}
					else
					{
						sb.Append(" CsHasNew=\"0\" ");
					}
					sb.Append("/>");
				}
			}
			sb.Append("</Serial>");

			// 车型
			sb.Append("<Car>");
			if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsCar.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Car CarID=\"" + dsCar.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
					sb.Append(" CarName=\"" + SecurityElement.Escape(dsCar.Tables[0].Rows[i]["car_name"].ToString()) + "\" ");
					sb.Append(" CsID=\"" + dsCar.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					sb.Append(" CarYear=\"" + dsCar.Tables[0].Rows[i]["Car_YearType"].ToString() + "\" ");
					sb.Append(" EngineExhaust=\"" + (dsCar.Tables[0].Rows[i]["EngineExhaust"].ToString().Trim() == "" ? "" : dsCar.Tables[0].Rows[i]["EngineExhaust"].ToString().Trim() + "L") + "\" ");
					sb.Append(" MarketDay=\"" + GetCarMarketYearAndMonth(dsCar.Tables[0].Rows[i]["MarketYear"].ToString(), dsCar.Tables[0].Rows[i]["MarketMonth"].ToString()) + "\" ");
					string forwardGearNum = dsCar.Tables[0].Rows[i]["ForwardGearNum"].ToString().Trim() == "" ? "" : dsCar.Tables[0].Rows[i]["ForwardGearNum"].ToString().Trim() + "档";
					string transmissionType = forwardGearNum + dsCar.Tables[0].Rows[i]["TransmissionType"].ToString().Trim();
					sb.Append(" TransmissionType=\"" + transmissionType + "\" ");
					sb.Append(" RepairPolicy=\"" + dsCar.Tables[0].Rows[i]["RepairPolicy"].ToString() + "\" ");
					sb.Append(" SaleState=\"" + dsCar.Tables[0].Rows[i]["SaleState"].ToString() + "\" ");
					sb.Append(" ReferPrice=\"" + dsCar.Tables[0].Rows[i]["car_ReferPrice"].ToString() + "\" ");
					sb.Append(" Transmission=\"" + dsCar.Tables[0].Rows[i]["TransmissionType"].ToString().Trim() + "\" ");
					sb.Append(" CarPriceRange=\"" + GetCarPriceByCarID(int.Parse(dsCar.Tables[0].Rows[i]["car_id"].ToString())) + "\" ");
					sb.Append("/>");
				}
			}
			sb.Append("</Car>");
			sb.Append("</Data>");
		}

		/// <summary>
		/// 取车型报价区间
		/// </summary>
		/// <param name="carid"></param>
		/// <returns></returns>
		private string GetCarPriceByCarID(int carid)
		{
			string price = "";
			DataSet ds = base.GetAllCarPrice();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = ds.Tables[0].Select(" Id='" + carid.ToString() + "' ");
				if (drs.Length > 0)
				{
					try
					{
						decimal min = Math.Round(decimal.Parse(drs[0]["MinPrice"].ToString()), 2);
						decimal max = Math.Round(decimal.Parse(drs[0]["MaxPrice"].ToString()), 2);
						price = min.ToString() + "万-" + max.ToString() + "万";
					}
					catch
					{ }
				}
			}
			return price;
		}

		/// <summary>
		/// 取子品牌报价区间
		/// </summary>
		/// <param name="csID"></param>
		/// <returns></returns>
		private string GetSerialPriceRByID(int csID)
		{
			string result = string.Empty;
			DataSet ds = base.GetAllSerialPrice();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = ds.Tables[0].Select(" Id=" + csID.ToString() + " ");
				if (drs.Length > 0)
				{
					try
					{
						decimal min = Math.Round(decimal.Parse(drs[0]["MinPrice"].ToString()), 2);
						decimal max = Math.Round(decimal.Parse(drs[0]["MaxPrice"].ToString()), 2);
						result = min.ToString() + "万-" + max.ToString() + "万";
					}
					catch
					{ }
				}
			}
			return result;
		}

		/// <summary>
		/// 取子品牌价格区间
		/// </summary>
		/// <param name="minPrice"></param>
		/// <param name="maxPrice"></param>
		/// <returns></returns>
		private string GetPriceRankMult(string minPrice, string maxPrice)
		{
			string temp = ",";
			try
			{
				double dMin = Convert.ToDouble(minPrice);
				double dMax = Convert.ToDouble(maxPrice);
				int iMin = 0;
				int iMax = 0;
				if (dMin > 0)
				{
					iMin = 1;
				}
				if (dMin > 5)
				{
					iMin = 2;
				}
				if (dMin > 10)
				{
					iMin = 3;
				}
				if (dMin > 15)
				{
					iMin = 4;
				}
				if (dMin > 20)
				{
					iMin = 5;
				}
				if (dMin > 25)
				{
					iMin = 6;
				}
				if (dMin > 40)
				{
					iMin = 7;
				}
				if (dMin > 80)
				{
					iMin = 8;
				}

				if (dMax > 0)
				{
					iMax = 1;
				}
				if (dMax > 5)
				{
					iMax = 2;
				}
				if (dMax > 10)
				{
					iMax = 3;
				}
				if (dMax > 15)
				{
					iMax = 4;
				}
				if (dMax > 20)
				{
					iMax = 5;
				}
				if (dMax > 25)
				{
					iMax = 6;
				}
				if (dMax > 40)
				{
					iMax = 7;
				}
				if (dMax > 80)
				{
					iMax = 8;
				}

				for (int i = iMin; i <= iMax; i++)
				{
					if (i == 0)
					{
						continue;
					}
					if (i != iMax)
					{
						temp += i.ToString() + ",";
					}
					else
					{
						temp += i.ToString();
					}
				}
				temp += ",";
			}
			catch
			{
				temp = ",0,";
			}
			return temp;
		}

		/// <summary>
		/// 取子品牌的聚合排量
		/// </summary>
		/// <param name="csID"></param>
		/// <returns></returns>
		private string GetSerialEngineExhaust(int csID)
		{
			string ee = "";
			if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = dsCar.Tables[0].Select("cs_id = '" + csID.ToString() + "' ");
				if (drs != null && drs.Length > 0)
				{
					List<string> al = new List<string>();
					foreach (DataRow dr in drs)
					{
						if (!al.Contains(dr["EngineExhaust"].ToString().Trim()) && dr["EngineExhaust"].ToString().Trim() != "")
						{
							al.Add(dr["EngineExhaust"].ToString().Trim());
						}
					}
					string[] arrEE = al.ToArray();
					Array.Sort(arrEE);
					if (arrEE.Length > 0)
					{
						for (int i = 0; i < arrEE.Length; i++)
						{
							if (ee == "")
							{
								ee = arrEE[i] + "L";
							}
							else
							{
								ee += "、" + arrEE[i] + "L";
							}
						}
					}
				}
			}

			return ee;
		}

		/// <summary>
		/// 是否有新车子品牌
		/// </summary>
		/// <returns></returns>
		private void GetSerialHasNewCar()
		{
			XmlDocument xmlDoc = AutoStorageService.GetAutoXml();
			if (xmlDoc != null && xmlDoc.HasChildNodes)
			{
				XmlNodeList xnl = xmlDoc.SelectNodes("/Params/MasterBrand/Brand/Serial[@CsHasNew='1']");
				if (xnl != null && xnl.Count > 0)
				{
					foreach (XmlNode xn in xnl)
					{
						if (!csHasNew.ContainsKey(xn.Attributes["ID"].Value.ToString()))
						{
							csHasNew.Add(xn.Attributes["ID"].Value.ToString(), 1);
						}
					}
				}
			}
		}

		/// <summary>
		/// 取车型上市年、月
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <returns></returns>
		private string GetCarMarketYearAndMonth(string year, string month)
		{
			string ym = "";
			int oYear = 0;
			int oMonth = 0;
			if (year != "" && int.TryParse(year, out  oYear))
			{
				if (oYear > 0)
				{
					ym = oYear.ToString();
					if (month != "" && int.TryParse(month, out  oMonth))
					{
						if (oMonth > 0 && oMonth < 13)
						{
							ym = oYear.ToString() + "-" + oMonth.ToString();
						}
					}
				}
			}
			return ym;
		}

		/*
		 * 节点说明:
		 * Data:根节点
		 * Producer:厂商节点(CpID:厂商ID,CpName:厂商名称)
		 * Master:主品牌节点(BsID:主品牌ID,BsName:主品牌名称,BsSpell:主品牌首字母)
		 * Brand:品牌节点(CbID:品牌ID,CbName:品牌名称,CpID:所属厂商ID,BsID:所属主品牌ID)
		 * Serial:子品牌节点(CsID:子品牌ID,CsName:子品牌名称,CbID:子品牌所属品牌ID)
		 * Car:车型节点(CarID:车型ID,CarName:车型名称,CsID:车型所属子品牌ID,CarYear:年款,EngineExhaust:排量,MarketDay:上市时间,TransmissionType:变速箱,RepairPolicy:保修政策,SaleState:销售状态,ReferPrice:厂商指导价格,Pic:默认图片(1024*768))
		 */
	}
}