using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 子品牌的年款范围 根据上市时间 (胡利)
	/// </summary>
	public partial class SerialYearInfo : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();
		private string MakeTime = string.Empty;
		private DataSet dsMake = new DataSet();
		private DataSet dsYearInfo = new DataSet();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				// GetParam();
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<SerialYear>");
				//if (MakeTime != "")
				//{
				//    // 取子品牌年款的上市时间
				//    GetAllSerialYearMakeTime();
				//}
				//else
				//{
				// 取子品牌的年款
				GetAllSerialYearInfo();
				GetAllMasterToSerial();
				//}
				sb.Append("</SerialYear>");
				Response.Write(sb.ToString());
			}
		}

		private void GetParam()
		{
			if (this.Request.QueryString["MakeTime"] != null && this.Request.QueryString["MakeTime"].ToString() != "")
			{
				MakeTime = this.Request.QueryString["MakeTime"].ToString().ToLower();
			}
		}

		// 左右子品牌年款上市时间
		private void GetAllSerialYearMakeTime()
		{
			string sqlGetAll = @"select car.car_id,car.cs_id,car.Car_YearType,cdb1.pvalue as makeYear,cdb2.pvalue as makemonth
from dbo.Car_relation car
left join car_serial cs on car.cs_id=cs.cs_id
left join dbo.CarDataBase cdb1 on car.car_id=cdb1.carid and cdb1.paramid=385
left join dbo.CarDataBase cdb2 on car.car_id=cdb2.carid and cdb2.paramid=384
where car.isState=0 and cs.isState=0 and car.Car_YearType>0
order by car.cs_id,car.Car_YearType desc";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlGetAll);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				string currentCsID = "";
				string currentYear = "";
				string currentTime = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString();
				string minMakeTime = "";
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					if (currentCsID != ds.Tables[0].Rows[i]["cs_id"].ToString())
					{
						// 不同子品牌
						if (currentCsID == "")
						{
							// 第1行
						}
						else
						{
							// 非第一行
							if (currentYear != ds.Tables[0].Rows[i]["Car_YearType"].ToString())
							{
								// 不同年款
								// sb("<Year ID=\"\" />");
								sb.Append("</Serial>");
								sb.Append("<Serial ID=\"" + currentCsID + "\" >");
								currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString();
							}
							else
							{
								// 相同年款
							}
						}
					}
					else
					{
						// 相同子品牌
					}
					GetMinMakeTime(ds.Tables[0].Rows[i]["Car_YearType"].ToString(), ds.Tables[0].Rows[i]["makeYear"].ToString(), ds.Tables[0].Rows[i]["makemonth"].ToString(), ref minMakeTime);

				}
			}
		}

		// 所有子品牌年款信息
		private void GetAllSerialYearInfo()
		{

			#region 取子品牌年款上市时间

			string sqlGetAll = @"select car.car_id,car.cs_id,car.Car_YearType,cdb1.pvalue as makeYear,cdb2.pvalue as makemonth
from dbo.Car_relation car
left join car_serial cs on car.cs_id=cs.cs_id
left join dbo.CarDataBase cdb1 on car.car_id=cdb1.carid and cdb1.paramid=385
left join dbo.CarDataBase cdb2 on car.car_id=cdb2.carid and cdb2.paramid=384
where car.isState=0 and cs.isState=0 and car.Car_YearType>0
order by car.cs_id,car.Car_YearType desc";
			dsMake = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlGetAll);

			#endregion

			string sqlYearInfo = @"select csy.cs_id,csy.carYear
from dbo.Car_SerialYear csy
left join car_serial cs on csy.cs_id = cs.cs_id
where cs.isState=0 and csy.isState=0 and csy.carYear>0 
order by csy.cs_id,csy.carYear desc";
			dsYearInfo = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlYearInfo);
			//if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			//{
			//    string currentCsID = "";
			//    string lastMake = DateTime.Now.AddYears(10).Year.ToString() + "-" + DateTime.Now.Month.ToString();
			//    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			//    {
			//        if (currentCsID != ds.Tables[0].Rows[i]["cs_id"].ToString().Trim())
			//        {
			//            if (currentCsID == "")
			//            {
			//                // 第一行
			//            }
			//            else
			//            {
			//                // 新子品牌
			//                sb.Append("</Serial>");
			//                lastMake = DateTime.Now.AddYears(10).Year.ToString() + "-" + DateTime.Now.Month.ToString();
			//            }
			//            currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString().Trim();
			//            sb.Append("<Serial CsID=\"" + currentCsID + "\" >");
			//        }
			//        string minMakeTime = GetMakeTime(currentCsID, ds.Tables[0].Rows[i]["carYear"].ToString().Trim());
			//        bool isRight = isMaxAndMinMakeTime(minMakeTime, lastMake);
			//        if (isRight)
			//        {
			//            sb.Append("<Year ID=\"" + ds.Tables[0].Rows[i]["carYear"].ToString().Trim() + "\" MinMakeTime=\"" + minMakeTime + "\" MaxMakeTime=\"" + lastMake + "\" />");
			//        }
			//        else
			//        {
			//            sb.Append("<Year ID=\"" + ds.Tables[0].Rows[i]["carYear"].ToString().Trim() + "\" MinMakeTime=\"" + minMakeTime + "\" MaxMakeTime=\"" + lastMake + "\" MakeTimeRight=\"false\" />");
			//        }
			//            string[] arrMin = minMakeTime.Split('-');
			//        if (arrMin[1] == "1")
			//        {
			//            lastMake = Convert.ToString(int.Parse(arrMin[0]) - 1) + "-12";
			//        }
			//        else
			//        {
			//            lastMake = arrMin[0] + "-" + Convert.ToString(int.Parse(arrMin[1]) - 1);
			//        }
			//    }
			//}
			//sb.Append("</Serial>");

		}

		private void GetAllMasterToSerial()
		{
			string sqlMasterToSerial = @"select cmb.bs_id,cmb.bs_name,cmb.urlSpell as bsAllSpell,cb.cb_id,
cb.cb_name,cb.allSpell as cbAllSpell,cs.cs_id,
cs.csname,cs.allSpell as csAllspell,cs.csShowName
from car_serial cs 
left join dbo.Car_Brand cb on cs.cb_id=cb.cb_id
left join dbo.Car_MasterBrand_Rel cmbr on cb.cb_id=cmbr.cb_id and cmbr.isState=0
left join dbo.Car_MasterBrand cmb on cmbr.bs_id = cmb.bs_id
where cs.isState=0 and cs.isState=0 and cmb.isState=0
order by cmb.spell,cmb.bs_id,cb.spell,cb.cb_id,cs.spell";
			DataSet dsMasterToSerial = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlMasterToSerial);

			#region make MasterToSerial
			if (dsMasterToSerial != null && dsMasterToSerial.Tables.Count > 0 && dsMasterToSerial.Tables[0].Rows.Count > 0)
			{
				string currentBsID = "";
				string currentCbID = "";
				string currentCsID = "";
				for (int i = 0; i < dsMasterToSerial.Tables[0].Rows.Count; i++)
				{
					if (i == 0)
					{
						currentBsID = dsMasterToSerial.Tables[0].Rows[i]["bs_id"].ToString();
						currentCbID = dsMasterToSerial.Tables[0].Rows[i]["cb_id"].ToString();
						currentCsID = dsMasterToSerial.Tables[0].Rows[i]["cs_id"].ToString();
						sb.Append("<MasterBrand ID=\"" + currentBsID + "\" ");
						sb.Append(" Name=\"" + dsMasterToSerial.Tables[0].Rows[i]["bs_name"].ToString() + "\" ");
						sb.Append(" AllSpell=\"" + dsMasterToSerial.Tables[0].Rows[i]["bsAllSpell"].ToString().ToLower() + "\" >");

						sb.Append("<Brand ID=\"" + currentCbID + "\" ");
						sb.Append(" Name=\"" + dsMasterToSerial.Tables[0].Rows[i]["cb_name"].ToString() + "\" ");
						sb.Append(" AllSpell=\"" + dsMasterToSerial.Tables[0].Rows[i]["cbAllSpell"].ToString().ToLower() + "\" >");

						sb.Append("<Serial ID=\"" + currentCsID + "\" ");
						sb.Append(" Name=\"" + dsMasterToSerial.Tables[0].Rows[i]["csname"].ToString() + "\" ");
						sb.Append(" ShowName=\"" + dsMasterToSerial.Tables[0].Rows[i]["csShowName"].ToString() + "\" ");
						sb.Append(" AllSpell=\"" + dsMasterToSerial.Tables[0].Rows[i]["csAllspell"].ToString().ToLower() + "\" >");

						GetSerialYearInfoByCsID(currentCsID, ref sb);
					}
					else
					{
						// 不同主品牌
						if (currentBsID != dsMasterToSerial.Tables[0].Rows[i]["bs_id"].ToString())
						{
							sb.Append("</Serial>");
							sb.Append("</Brand>");
							sb.Append("</MasterBrand>");
							currentBsID = dsMasterToSerial.Tables[0].Rows[i]["bs_id"].ToString();
							currentCbID = dsMasterToSerial.Tables[0].Rows[i]["cb_id"].ToString();
							currentCsID = dsMasterToSerial.Tables[0].Rows[i]["cs_id"].ToString();

							sb.Append("<MasterBrand ID=\"" + currentBsID + "\" ");
							sb.Append(" Name=\"" + dsMasterToSerial.Tables[0].Rows[i]["bs_name"].ToString() + "\" ");
							sb.Append(" AllSpell=\"" + dsMasterToSerial.Tables[0].Rows[i]["bsAllSpell"].ToString().ToLower() + "\" >");

							sb.Append("<Brand ID=\"" + currentCbID + "\" ");
							sb.Append(" Name=\"" + dsMasterToSerial.Tables[0].Rows[i]["cb_name"].ToString() + "\" ");
							sb.Append(" AllSpell=\"" + dsMasterToSerial.Tables[0].Rows[i]["cbAllSpell"].ToString().ToLower() + "\" >");

							sb.Append("<Serial ID=\"" + currentCsID + "\" ");
							sb.Append(" Name=\"" + dsMasterToSerial.Tables[0].Rows[i]["csname"].ToString() + "\" ");
							sb.Append(" ShowName=\"" + dsMasterToSerial.Tables[0].Rows[i]["csShowName"].ToString() + "\" ");
							sb.Append(" AllSpell=\"" + dsMasterToSerial.Tables[0].Rows[i]["csAllspell"].ToString().ToLower() + "\" >");

							GetSerialYearInfoByCsID(currentCsID, ref sb);
						}
						else
						{
							// 不同品牌
							if (currentCbID != dsMasterToSerial.Tables[0].Rows[i]["cb_id"].ToString())
							{
								sb.Append("</Serial>");
								sb.Append("</Brand>");
								currentCbID = dsMasterToSerial.Tables[0].Rows[i]["cb_id"].ToString();
								currentCsID = dsMasterToSerial.Tables[0].Rows[i]["cs_id"].ToString();

								sb.Append("<Brand ID=\"" + currentCbID + "\" ");
								sb.Append(" Name=\"" + dsMasterToSerial.Tables[0].Rows[i]["cb_name"].ToString() + "\" ");
								sb.Append(" AllSpell=\"" + dsMasterToSerial.Tables[0].Rows[i]["cbAllSpell"].ToString().ToLower() + "\" >");

								sb.Append("<Serial ID=\"" + currentCsID + "\" ");
								sb.Append(" Name=\"" + dsMasterToSerial.Tables[0].Rows[i]["csname"].ToString() + "\" ");
								sb.Append(" ShowName=\"" + dsMasterToSerial.Tables[0].Rows[i]["csShowName"].ToString() + "\" ");
								sb.Append(" AllSpell=\"" + dsMasterToSerial.Tables[0].Rows[i]["csAllspell"].ToString().ToLower() + "\" >");

								GetSerialYearInfoByCsID(currentCsID, ref sb);
							}
							else
							{
								// 不同子品牌
								if (currentCsID != dsMasterToSerial.Tables[0].Rows[i]["cs_id"].ToString())
								{
									sb.Append("</Serial>");
									currentCsID = dsMasterToSerial.Tables[0].Rows[i]["cs_id"].ToString();

									sb.Append("<Serial ID=\"" + currentCsID + "\" ");
									sb.Append(" Name=\"" + dsMasterToSerial.Tables[0].Rows[i]["csname"].ToString() + "\" ");
									sb.Append(" ShowName=\"" + dsMasterToSerial.Tables[0].Rows[i]["csShowName"].ToString() + "\" ");
									sb.Append(" AllSpell=\"" + dsMasterToSerial.Tables[0].Rows[i]["csAllspell"].ToString().ToLower() + "\" >");

									GetSerialYearInfoByCsID(currentCsID, ref sb);
								}
								else
								{
									GetSerialYearInfoByCsID(currentCsID, ref sb);
								}
							}
						}

						// 
					}
				}
				sb.Append("</Serial>");
				sb.Append("</Brand>");
				sb.Append("</MasterBrand>");
			}
			#endregion
		}

		/// <summary>
		/// 根据子品牌ID取年款
		/// </summary>
		/// <param name="csID"></param>
		/// <param name="_sb"></param>
		private void GetSerialYearInfoByCsID(string csID, ref StringBuilder _sb)
		{
			if (dsYearInfo != null && dsYearInfo.Tables.Count > 0 && dsYearInfo.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = dsYearInfo.Tables[0].Select(" cs_id='" + csID + "' ");
				if (drs != null && drs.Length > 0)
				{
					string lastMake = DateTime.Now.AddYears(10).Year.ToString() + "-" + DateTime.Now.Month.ToString();
					foreach (DataRow dr in drs)
					{
						string minMakeTime = GetMakeTime(csID, dr["carYear"].ToString().Trim());
						bool isRight = isMaxAndMinMakeTime(minMakeTime, lastMake);
						if (isRight)
						{
							_sb.Append("<Year ID=\"" + dr["carYear"].ToString().Trim() + "\" MinMakeTime=\"" + minMakeTime + "\" MaxMakeTime=\"" + lastMake + "\" />");
						}
						else
						{
							sb.Append("<Year ID=\"" + dr["carYear"].ToString().Trim() + "\" MinMakeTime=\"" + minMakeTime + "\" MaxMakeTime=\"" + lastMake + "\" MakeTimeRight=\"false\" />");
						}
						string[] arrMin = minMakeTime.Split('-');
						if (arrMin[1] == "1")
						{
							lastMake = Convert.ToString(int.Parse(arrMin[0]) - 1) + "-12";
						}
						else
						{
							lastMake = arrMin[0] + "-" + Convert.ToString(int.Parse(arrMin[1]) - 1);
						}
					}
				}
			}
		}

		private string GetMakeTime(string csID, string year)
		{
			string makeTime = "";
			if (dsMake != null && dsMake.Tables.Count > 0 && dsMake.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = dsMake.Tables[0].Select(" cs_id ='" + csID + "' and Car_YearType='" + year.ToString() + "'");
				if (drs != null && drs.Length > 0)
				{
					foreach (DataRow dr in drs)
					{
						GetMinMakeTime(year, dr["makeYear"].ToString(), dr["makemonth"].ToString(), ref makeTime);
					}
				}
				else
				{
					makeTime = year + "-3";
				}
			}
			return makeTime;
		}

		private void GetMinMakeTime(string year, string makeYear, string makeMonth, ref string minMakeTime)
		{
			if (makeYear == "" || makeYear == "0")
			{
				// 没填上市年
				makeYear = year;
				makeMonth = "3";
			}
			if (makeMonth == "" || makeMonth == "0")
			{
				// 没填上市月
				if (int.Parse(year) <= int.Parse(makeYear))
				{
					// 年款小于上市时间年
					makeMonth = "3";
				}
				else
				{
					// 年款大于上市时间年
					makeMonth = "9";
				}
			}

			if (minMakeTime == "")
			{
				// 没有最早上市时间
				minMakeTime = makeYear + "-" + makeMonth;
			}
			else
			{
				// 有最早上市时间
				string[] arrMakeTime = minMakeTime.Split('-');
				if (int.Parse(arrMakeTime[0]) > int.Parse(makeYear))
				{
					// 已有年大于当前年
					minMakeTime = makeYear + "-" + makeMonth;
				}
				else if (int.Parse(arrMakeTime[0]) == int.Parse(makeYear) && int.Parse(arrMakeTime[1]) > int.Parse(makeMonth))
				{
					// 已有月大于当前月
					minMakeTime = makeYear + "-" + makeMonth;
				}
			}
		}

		private bool isMaxAndMinMakeTime(string minTime, string maxTime)
		{
			bool isRight = false;
			if (Convert.ToDateTime(minTime + "-1") < Convert.ToDateTime(maxTime + "-1"))
			{ isRight = true; }
			return isRight;
		}
	}
}