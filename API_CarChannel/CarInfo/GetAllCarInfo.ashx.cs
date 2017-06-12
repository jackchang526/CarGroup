using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;

using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.CarUtils.Define;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetAllCarInfo 的摘要说明
	/// </summary>
	public class GetAllCarInfo : IHttpHandler
	{

		private int pv = 0;
		private int subsidy = 0;
		private string masterToSerial;
		private string carInfo;

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(60);
			context.Response.ContentType = "Text/XML";
			context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			context.Response.Write("<Root>");
			GetParams(context);
			if (pv == 1 || subsidy == 1 || carInfo.Length > 0)
			{
				Dictionary<string, string> SubSidyDic = new CarInfoDal().GetSubsidy();
				CarInfoToXmlFromDs(context, SubSidyDic);
			}
			else if (masterToSerial.Length != 0)
			{
				//需要主品牌到子品牌的相关信息
				GetAllMasterToSerialInfo(context);
			}
			else
			{
				DataSet dsCar = new CarInfoDal().GetAllCarInfoDs();
				GenerateCarInfoXML(context, dsCar);
			}
			context.Response.Write("</Root>");
		}

		private void GetParams(HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request.QueryString["pv"]))
			{
				pv = Convert.ToInt32(context.Request.QueryString["pv"]);
			}
			if (!string.IsNullOrEmpty(context.Request.QueryString["subsidy"]))
			{
				subsidy = Convert.ToInt32(context.Request.QueryString["subsidy"]);
			}

			masterToSerial = context.Request.QueryString["MasterToSerial"];
			if (masterToSerial == null)
				masterToSerial = String.Empty;
			else
				masterToSerial = masterToSerial.ToLower();
			carInfo = context.Request.QueryString["CarInfo"];
			if (carInfo == null)
				carInfo = String.Empty;
			else
				carInfo = carInfo.ToLower();

		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		#region  原始接口



		private void GenerateCarInfoXML(HttpContext context, DataSet dsCar)
		{
			if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsCar.Tables[0].Rows.Count; i++)
				{
					context.Response.Write("<Item CarID=\"" + dsCar.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
					context.Response.Write(" CarName=\"" + System.Security.SecurityElement.Escape(dsCar.Tables[0].Rows[i]["car_name"].ToString()) + "\" ");
					context.Response.Write(" CsID=\"" + dsCar.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					context.Response.Write(" CarYearType=\"" + dsCar.Tables[0].Rows[i]["Car_YearType"].ToString() + "\" ");
					context.Response.Write(" ReferPrice=\"" + dsCar.Tables[0].Rows[i]["car_ReferPrice"].ToString() + "\" ");
					context.Response.Write(" EngineExhaust=\"" + dsCar.Tables[0].Rows[i]["Engine_Exhaust"].ToString() + "\" ");
					context.Response.Write(" EngineExhaustForFloat=\"" + dsCar.Tables[0].Rows[i]["Engine_ExhaustForFloat"].ToString() + "\" ");
					context.Response.Write(" UnderPanTransmissionType=\"" + dsCar.Tables[0].Rows[i]["UnderPan_TransmissionType"].ToString() + "\" />");
				}
			}
		}
		#endregion

		#region 有参数时显示数据
		/// <summary>
		/// 有参数时显示数据
		/// </summary>
		#endregion
		private void CarInfoToXmlFromDs(HttpContext context, Dictionary<string, string> SubSidyDic)
		{
			DataSet ds = new CarInfoDal().GetCarInfoByParams();
			if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					string carLevel = ConvertHelper.GetString(row["cs_carLevel"]);
					if (carLevel == "概念车")
						continue;
					context.Response.Write("<Item CarID=\"" + row["car_Id"].ToString() + "\"");
					if (carInfo.Contains('n'))
						context.Response.Write(" CarName=\"" + System.Security.SecurityElement.Escape(row["car_name"].ToString()) + "\"");
					context.Response.Write(" CsID=\"" + row["cs_id"].ToString() + "\"");
					//context.Response.Write(" CarYearType=\"" + row["Car_YearType"].ToString() + "\" ");
					//context.Response.Write(" ReferPrice=\"" + row["car_ReferPrice"].ToString() + "\" ");
					//context.Response.Write(" EngineExhaust=\"" + row["Engine_Exhaust"].ToString() + "\" ");
					//context.Response.Write(" UnderPanTransmissionType=\"" + row["UnderPan_TransmissionType"].ToString() + "\"");
					if (pv == 1)
						context.Response.Write(" PVSumNum=\"" + (string.IsNullOrEmpty(row["pv_sumnum"].ToString()) ? "0" : row["pv_sumnum"].ToString()) + "\"");
					if (subsidy == 1)
					{
						string subsidystr = "";

						if (SubSidyDic.ContainsKey(row["car_Id"].ToString()))
						{
							subsidystr = SubSidyDic[row["car_id"].ToString()];
							if (subsidystr.IndexOf("元") == -1)
								subsidystr = "";
						}
						context.Response.Write(" Subsidy=\"" + subsidystr + "\"");
					}
					context.Response.Write("/>");
				}
			}
		}

		#region masterToserial region
		/// <summary>
		/// 生成主品牌到子品牌的信息
		/// </summary>
		private void GetAllMasterToSerialInfo(HttpContext context)
		{
			if (masterToSerial.Contains('m'))
			{
				DataSet masterDs = MasterBrandDal.GetMasterBrands();
				if (masterDs != null && masterDs.Tables.Count > 0)
				{
					foreach (DataRow row in masterDs.Tables[0].Rows)
					{
						context.Response.Write("<Masterbrand Id=\"" + ConvertHelper.GetString(row["bs_Id"]) + "\"");
						context.Response.Write(" Name=\"" + ConvertHelper.GetString(row["bs_Name"]) + "\"/>");
					}
				}
			}
			if (masterToSerial.Contains('b'))
			{
				DataSet brandDs = GetAllBrandFromDB();
				if (brandDs != null && brandDs.Tables.Count > 0)
				{
					foreach (DataRow row in brandDs.Tables[0].Rows)
					{
						context.Response.Write("<Brand Id=\"" + ConvertHelper.GetString(row["cb_Id"]) + "\"");
						context.Response.Write(" Name=\"" + ConvertHelper.GetString(row["cb_Name"]) + "\"");
						context.Response.Write(" MasterbrandId=\"" + ConvertHelper.GetString(row["bs_Id"]) + "\"/>");
					}
				}
			}
			if (masterToSerial.Contains("l"))
			{
				int[] enumValues = (int[])Enum.GetValues(typeof(CarLevelDefine.SerialLevelEnum));
				foreach (int levelId in enumValues)
				{
					context.Response.Write("<Level Id=\"" + levelId + "\"");
					//string enumName = ((SerialLevelEnum)levelId).ToString();
					//if (enumName == "紧凑型" || enumName == "中大型")
					//    enumName += "车";
					string enumName = CarLevelDefine.GetLevelNameById(levelId);
					enumName = CarLevelDefine.GetLevelFullName(enumName);
					context.Response.Write(" Name=\"" + enumName + "\"/>");
				}
			}
			if (masterToSerial.Contains('s'))
			{
				DataSet ds = GetAllSerialFromDB();
				if (ds != null && ds.Tables.Count > 0)
				{
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						int levelClassId = ConvertHelper.GetInteger(row["carlevel"]);
						//跳过概念车
						if (levelClassId == 481)
							continue;
						context.Response.Write("<Serial Id=\"" + ConvertHelper.GetString(row["cs_Id"]) + "\"");
						context.Response.Write(" Name=\"" + ConvertHelper.GetString(row["csShowName"]) + "\"");
						context.Response.Write(" BrandId=\"" + ConvertHelper.GetString(row["cb_Id"]) + "\"");
						if (masterToSerial.Contains("l"))
						{
							context.Response.Write(" LevelId=\"" + ConvertLeveId(levelClassId) + "\"");
						}
						context.Response.Write("/>");
					}
				}
			}
		}

		private int ConvertLeveId(int classId)
		{
			int levelId = 0;
			switch (classId)
			{
				case 321:
					levelId = 1;
					break;
				case 338:
					levelId = 2;
					break;
				case 339:
					levelId = 3;
					break;
				case 340:
					levelId = 5;
					break;
				case 341:
					levelId = 4;
					break;
				case 342:
					levelId = 6;
					break;
				case 424:
					levelId = 8;
					break;
				case 425:
					levelId = 7;
					break;
				case 426:
					levelId = 9;
					break;
				case 428:
					levelId = 10;
					break;
				case 482:
					levelId = 11;
					break;
				case 483:
					levelId = 12;
					break;
			}
			return levelId;
		}

		/// <summary>
		/// 取所有子品牌信息，等DAL上线后用 Car_SerialDal.GetAllSerialWithStopSale 方法代替
		/// </summary>
		/// <returns></returns>
		private DataSet GetAllSerialFromDB()
		{
			string sqlsStr = "SELECT cs_Id,cb_Id,csName,csShowName,carlevel FROM Car_Serial WHERE IsState=0";
			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlsStr);
		}

		/// <summary>
		/// 取所有品牌信息，等DAL上线后用 Car_BranDal.GetAllBrandWithStopSale 方法代替
		/// </summary>
		/// <returns></returns>
		private DataSet GetAllBrandFromDB()
		{
			string sqlStr = "select cb.cb_Id,cb.cb_Name,cmr.bs_Id from dbo.Car_Brand cb left join dbo.Car_MasterBrand_Rel cmr ON cb.cb_Id=cmr.cb_Id where cb.isState=0 and cmr.isState=0";
			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
		}



		#endregion


	}
}