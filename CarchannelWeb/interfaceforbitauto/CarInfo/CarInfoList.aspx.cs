using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;

using BitAuto.Utils;
using System.Data.SqlClient;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.CarInfo
{
	/// <summary>
	/// 工信部油耗 车型信息 口碑 熊玉辉 Ucar陈燕婷
	/// carcolor : 车型车身颜色
	/// </summary>
	public partial class CarInfoList : InterfacePageBase
	{
		private string dept = "";
		private StringBuilder sb = new StringBuilder();
		private int carIDInt = 0;
		private string csids = "";
		private string carids = "";
		// 颜色类型0:车身颜色 1:内饰颜色
		private int colorType = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				if (dept == "koubei")
				{
					// 口碑工信部油耗
					GetCarInfoList();
				}
				else if (dept == "carselectinfo")
				{
					// 选车信息
					GetCarSelect();
				}
				else if (dept == "carmarketday")
				{
					// 董博 近3个月上市车(有指导价,上市时间齐全)
					GetCarNearMarketDay();
				}
				else if (dept == "ucarcardata")
				{
					// Ucar陈燕婷 车型数据
					GetCarDataForUcar();
				}
				else if (dept == "carcolor")
				{
					// 车型颜色 及颜色值
					GetCarColor();
				}
				else if (dept == "serialyearcolor")
				{
					GetSerialYearColor();
				}
				else
				{
					sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?><Root><!-- 无效参数 --></Root>");
				}
				Response.Write(sb.ToString());
			}
		}
		//获取子品牌年款颜色
		private void GetSerialYearColor()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?><Root>");
			int sid = ConvertHelper.GetInteger(Request.QueryString["sid"]);
			string sql = "select car.Car_YearType,cdb.* from car_relation car left join car_serial cs on car.cs_id=cs.cs_id left join cardatabase cdb on car.car_id=cdb.carid where car.isState=0 and cs.isState=0 and cdb.paramid= 598 and car.Car_YearType>0  and cs.cs_id=" + sid;
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
			Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
			if (ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					string key = dr["Car_YearType"].ToString();
					if (dict.ContainsKey(key))
					{
						string str = dr["pvalue"].ToString();
						if (!string.IsNullOrEmpty(str))
						{
							string[] temp = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
							List<string> listColor = new List<string>();
							foreach (string colorkey in temp)
							{
								if (!listColor.Contains(colorkey.Trim()))
								{ listColor.Add(colorkey.Trim()); }
							}
							List<string> list1 = dict[key];
							// List<string> list2 = new List<string>(str.Split(','));
							for (int j = 0; j < listColor.Count; j++)
							{
								if (!list1.Contains(listColor[j])) //除去重复项
								{
									list1.Add(listColor[j]);
								}
							}
							dict[key] = list1;
						}
					}
					else
					{
						string str = dr["pvalue"].ToString();
						if (!string.IsNullOrEmpty(str))
						{
							string[] temp = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
							List<string> listColor = new List<string>();
							foreach (string colorkey in temp)
							{
								if (!listColor.Contains(colorkey.Trim()))
								{ listColor.Add(colorkey.Trim()); }
							}
							dict.Add(key, listColor);
						}
					}
				}
				foreach (KeyValuePair<string, List<string>> kv in dict)
				{
					sb.AppendFormat("<Year ID=\"{0}\">", kv.Key);
					for (int i = 0; i < kv.Value.Count; i++)
					{
						string ssql = "select top 1 * from dbo.Car_SerialColor where type=0 and colorname=@colorname and cs_id=@cs_id";
						SqlParameter[] _param ={
                                      new SqlParameter("@colorname",SqlDbType.NVarChar,50),
									  new SqlParameter("@cs_id",SqlDbType.Int)
                                  };
						_param[0].Value = kv.Value[i];
						_param[1].Value = sid;
						DataSet cds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, ssql, _param);
						if (cds.Tables[0].Rows.Count > 0)
						{
							DataTable dt = cds.Tables[0];
							sb.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\" RGB=\"{2}\" />", dt.Rows[0]["autoid"], dt.Rows[0]["colorname"], dt.Rows[0]["colorrgb"]);
						}
					}
					sb.Append("</Year>");
				}
			}
			sb.Append("</Root>");
			Response.Write(sb.ToString());
		}
		private void GetPageParam()
		{
			if (this.Request.QueryString["dept"] != null && this.Request.QueryString["dept"].ToString() != "")
			{
				dept = this.Request.QueryString["dept"].ToString().ToLower();
			}
			if (this.Request.QueryString["csids"] != null && this.Request.QueryString["csids"].ToString() != "")
			{
				csids = this.Request.QueryString["csids"].ToString();
			}
			if (this.Request.QueryString["carids"] != null && this.Request.QueryString["carids"].ToString() != "")
			{
				carids = this.Request.QueryString["carids"].ToString();
			}
			if (this.Request.QueryString["carid"] != null && this.Request.QueryString["carid"].ToString() != "")
			{
				string caridStr = this.Request.QueryString["carid"].ToString();
				if (int.TryParse(caridStr, out carIDInt))
				{ }
			}
			if (this.Request.QueryString["colorType"] != null && this.Request.QueryString["colorType"].ToString() != "")
			{
				int tempColorType = 0;
				if (int.TryParse(this.Request.QueryString["colorType"].ToString(), out tempColorType))
				{
					// 默认为车身颜色，当制定颜色类型时取特定类型颜色(0:车身颜色 1:内饰颜色)
					if (tempColorType > 0)
					{ colorType = tempColorType; }
				}
			}
		}

		/// <summary>
		/// 口碑 取工信部油耗 车型信息
		/// </summary>
		private void GetCarInfoList()
		{
			string sql = @"SELECT  car.car_id, cp.Cp_Name, cl.classvalue AS carLevel,
                                    cdb1.pvalue AS Car_Num, cs.csShowName, cdb2.pvalue AS Engine_Type,
                                    cdb3.pvalue AS Oil_FuelType, cdb4.pvalue AS Engine_Exhaust,
                                    cdb5.pvalue AS Engine_MaxPower,
                                    cdb6.pvalue AS UnderPan_TransmissionType,
                                    cdb7.pvalue AS Perf_DriveType, cdb8.pvalue AS Perf_Weight,
                                    cdb9.pvalue AS Perf_TotalWeight, cdb10.pvalue AS Perf_ShiQuYouHao,
                                    cdb11.pvalue AS Perf_ZongHeYouHao, cdb12.pvalue AS Perf_ShiJiaoYouHao
                            FROM    dbo.Car_relation car
                                    LEFT JOIN car_serial cs ON car.cs_id = cs.cs_id
                                    LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN dbo.Car_producer cp ON cb.cp_id = cp.cp_id
                                    LEFT JOIN class cl ON cs.carlevel = cl.classid
                                    LEFT JOIN dbo.CarDataBase cdb1 ON car.car_id = cdb1.carid
                                                                      AND cdb1.paramid = 387
                                    LEFT JOIN dbo.CarDataBase cdb2 ON car.car_id = cdb2.carid
                                                                      AND cdb2.paramid = 436
                                    LEFT JOIN dbo.CarDataBase cdb3 ON car.car_id = cdb3.carid
                                                                      AND cdb3.paramid = 578
                                    LEFT JOIN dbo.CarDataBase cdb4 ON car.car_id = cdb4.carid
                                                                      AND cdb4.paramid = 423
                                    LEFT JOIN dbo.CarDataBase cdb5 ON car.car_id = cdb5.carid
                                                                      AND cdb5.paramid = 430
                                    LEFT JOIN dbo.CarDataBase cdb6 ON car.car_id = cdb6.carid
                                                                      AND cdb6.paramid = 712
                                    LEFT JOIN dbo.CarDataBase cdb7 ON car.car_id = cdb7.carid
                                                                      AND cdb7.paramid = 655
                                    LEFT JOIN dbo.CarDataBase cdb8 ON car.car_id = cdb8.carid
                                                                      AND cdb8.paramid = 669
                                    LEFT JOIN dbo.CarDataBase cdb9 ON car.car_id = cdb9.carid
                                                                      AND cdb9.paramid = 668
                                    LEFT JOIN dbo.CarDataBase cdb10 ON car.car_id = cdb10.carid
                                                                       AND cdb10.paramid = 783
                                    LEFT JOIN dbo.CarDataBase cdb11 ON car.car_id = cdb11.carid
                                                                       AND cdb11.paramid = 782
                                    LEFT JOIN dbo.CarDataBase cdb12 ON car.car_id = cdb12.carid
                                                                       AND cdb12.paramid = 784
                            WHERE   car.isState = 0
                                    AND cs.isState = 0
                            ORDER BY car.cs_id, car.car_id";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<CarList>");

				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					sb.AppendLine("<Car ID=\"" + dr["car_id"].ToString() + "\" ");
					sb.Append("CP=\"" + dr["Cp_Name"].ToString().Trim().Replace("&", "&amp;") + "\" ");
					sb.Append("LE=\"" + dr["carLevel"].ToString().Trim() + "\" ");
					sb.Append("CN=\"" + dr["Car_Num"].ToString().Trim().Replace("&", "&amp;") + "\" ");
					sb.Append("SN=\"" + dr["csShowName"].ToString().Trim().Replace("&", "&amp;") + "\" ");
					sb.Append("ET=\"" + dr["Engine_Type"].ToString().Trim().Replace("&", "&amp;") + "\" ");
					sb.Append("OFT=\"" + dr["Oil_FuelType"].ToString().Trim() + "\" ");
					sb.Append("EE=\"" + dr["Engine_Exhaust"].ToString().Trim() + "\" ");
					sb.Append("EMP=\"" + dr["Engine_MaxPower"].ToString().Trim().Replace("&", "&amp;") + "\" ");
					sb.Append("UPTT=\"" + dr["UnderPan_TransmissionType"].ToString().Trim() + "\" ");
					sb.Append("PDT=\"" + dr["Perf_DriveType"].ToString().Trim().Replace("&", "&amp;") + "\" ");
					sb.Append("PW=\"" + dr["Perf_Weight"].ToString().Trim() + "\" ");
					sb.Append("PTW=\"" + dr["Perf_TotalWeight"].ToString().Trim() + "\" ");
					sb.Append("SQYH=\"" + dr["Perf_ShiQuYouHao"].ToString().Trim() + "\" ");
					sb.Append("ZHYH=\"" + dr["Perf_ZongHeYouHao"].ToString().Trim() + "\" ");
					sb.Append("SJYH=\"" + dr["Perf_ShiJiaoYouHao"].ToString().Trim() + "\" />");
				}
				sb.Append("</CarList>");
			}
		}

		private void GetCarSelect()
		{
			string sql = " select * from CarInfoForSelecting";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.AppendLine("<CarList>");
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					sb.Append("<Car ID=\"" + dr["carId"].ToString().Trim() + "\"");
					sb.Append(" Name=\"" + System.Security.SecurityElement.Escape(dr["carName"].ToString().Trim()) + "\"");
					sb.Append(" CsID=\"" + dr["csID"].ToString().Trim() + "\"");
					sb.Append(" MinP=\"" + dr["minPrice"].ToString().Trim() + "\"");
					sb.Append(" MaxP=\"" + dr["maxprice"].ToString().Trim() + "\"");
					sb.Append(" E=\"" + dr["Exhaust"].ToString().Trim() + "\"");
					sb.Append(" EL=\"" + dr["ExhaustL"].ToString().Trim() + "\"");
					sb.Append(" TT=\"" + dr["TransmissionType"].ToString().Trim() + "\"");
					sb.Append(" BF=\"" + dr["BodyForm"].ToString().Trim() + "\"");
					sb.Append(" CL=\"" + dr["CarLevel"].ToString().Trim() + "\"");
					sb.Append(" P=\"" + dr["Purpose"].ToString().Trim() + "\"");
					sb.Append(" CC=\"" + dr["CarCountry"].ToString().Trim() + "\"");
					sb.Append(" C=\"" + dr["Comfortable"].ToString().Trim() + "\"");
					sb.Append(" S=\"" + dr["Safety"].ToString().Trim() + "\"");
					sb.Append(" BT=\"" + dr["BrandType"].ToString().Trim() + "\"");
					sb.Append(" CC2=\"" + dr["CarConfig"].ToString().Trim() + "\"");
					sb.Append(" CRP=\"" + dr["CarReferPrice"].ToString().Trim() + "\"");
					sb.AppendLine(" />");
				}
				sb.AppendLine("</CarList>");
			}
		}

		/// <summary>
		/// 董博 近3个月上市车(有指导价,上市时间齐全)
		/// </summary>
		private void GetCarNearMarketDay()
		{
			string sql = @"select cs.cs_id,cs.csname,car.car_id,car.car_name,car.car_ReferPrice,car.Car_YearType
,Convert(datetime,cdb1.pvalue+'-'+cdb2.pvalue+'-'+cdb3.pvalue) as marketDay
from dbo.Car_relation car
left join dbo.Car_Serial cs on car.cs_id=cs.cs_id
left join dbo.CarDataBase cdb1 on car.car_id=cdb1.carid and cdb1.paramid=385
left join dbo.CarDataBase cdb2 on car.car_id=cdb2.carid and cdb2.paramid=384
left join dbo.CarDataBase cdb3 on car.car_id=cdb3.carid and cdb3.paramid=383
where cs.isState=0 and car.isState=0
and cdb1.pvalue>0 and cdb2.pvalue>0 and cdb3.pvalue>0 and car.car_ReferPrice>0
and DATEDIFF(day, Convert(datetime,cdb1.pvalue+'-'+cdb2.pvalue+'-'+cdb3.pvalue), getdate())<=90
and DATEDIFF(day, Convert(datetime,cdb1.pvalue+'-'+cdb2.pvalue+'-'+cdb3.pvalue), getdate())>=0
order by marketDay desc";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);

			//只显示点击过的
			string sqlStr = "SELECT distinct(cid) FROM Carlog where  logType=3 and logtime>dateadd(month,-3,getdate())";
			DataSet clickDS = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
			Dictionary<int, int> clickedDic = new Dictionary<int, int>();
			if (clickDS != null && clickDS.Tables.Count > 0)
			{
				foreach (DataRow row in clickDS.Tables[0].Rows)
				{
					int tmpCarId = ConvertHelper.GetInteger(row["cid"]);
					if (tmpCarId != 0)
						clickedDic[tmpCarId] = 0;
				}
			}

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.AppendLine("<CarList>");

				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int carId = ConvertHelper.GetInteger(dr["car_id"]);
					if (!clickedDic.ContainsKey(carId))
						continue;
					sb.AppendLine("<Car ID=\"" + carId + "\" ");
					sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(dr["car_name"].ToString().Trim().Replace("&", "")) + "\" ");
					sb.Append(" CarYear=\"" + dr["Car_YearType"].ToString() + "\" ");
					sb.Append(" CsID=\"" + dr["cs_id"].ToString() + "\" ");
					sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(dr["csname"].ToString().Trim().Replace("&", "")) + "\" ");
					sb.Append(" ReferPrice=\"" + dr["car_ReferPrice"].ToString() + "\" ");
					sb.Append(" MarketDay=\"" + Convert.ToDateTime(dr["marketDay"].ToString()).ToShortDateString() + "\" />");
				}

				sb.AppendLine("</CarList>");
			}
		}

		/// <summary>
		/// 取二手车导出数据
		/// </summary>
		private void GetCarDataForUcar()
		{
			string sqlCarBase = @"SELECT  car.car_id, cmb.bs_name, car.Car_YearType, cb.cb_name, cs.csname,
                                            car.car_name, cdb1.pvalue AS p1, cdb3.pvalue AS p3, cdb4.pvalue AS p4,
                                            cdb5.pvalue AS p5, cdb7.pvalue AS p7, cdb9.pvalue AS p9,
                                            cl1.classvalue AS c1, cl2.classvalue AS c2
                                    FROM    dbo.Car_relation car
                                            LEFT JOIN car_serial cs ON car.cs_id = cs.cs_id
                                            LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                            LEFT JOIN Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                                                                  AND cmbr.isState = 0
                                            LEFT JOIN Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                                            LEFT JOIN class cl1 ON car.car_ProduceState = cl1.classid
                                            LEFT JOIN class cl2 ON cb.cb_country = cl2.classid
                                            LEFT JOIN dbo.CarDataBase cdb1 ON car.car_id = cdb1.carid
                                                                              AND cdb1.paramid = 385
                                            LEFT JOIN dbo.CarDataBase cdb3 ON car.car_id = cdb3.carid
                                                                              AND cdb3.paramid = 712
                                            LEFT JOIN dbo.CarDataBase cdb4 ON car.car_id = cdb4.carid
                                                                              AND cdb4.paramid = 423
                                            LEFT JOIN dbo.CarDataBase cdb5 ON car.car_id = cdb5.carid
                                                                              AND cdb5.paramid = 578
                                            LEFT JOIN dbo.CarDataBase cdb7 ON car.car_id = cdb7.carid
                                                                              AND cdb7.paramid = 574
                                            LEFT JOIN dbo.CarDataBase cdb9 ON car.car_id = cdb9.carid
                                                                              AND cdb9.paramid = 655
                                    WHERE   car.isState = 0
                                            AND cs.isState = 0
                                            AND car_id ={0}";

			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<CarList>");

			List<int> listCarID = new List<int>();

			#region 取ID
			// 车型ID
			if (carids != "")
			{
				string[] arrCarID = carids.Split(',');
				if (arrCarID.Length > 0)
				{
					foreach (string id in arrCarID)
					{
						int idInt = 0;
						if (int.TryParse(id, out idInt))
						{
							if (idInt > 0 && !listCarID.Contains(idInt))
							{ listCarID.Add(idInt); }
						}
					}
				}
			}
			// 子品牌ID
			if (csids != "")
			{
				string[] arrCsID = csids.Split(',');
				if (arrCsID.Length > 0)
				{
					foreach (string id in arrCsID)
					{
						int idInt = 0;
						if (int.TryParse(id, out idInt))
						{
							if (idInt > 0)
							{
								DataSet dsCarByCs = base.GetCarReferPriceByCsID(idInt, true);
								if (dsCarByCs != null && dsCarByCs.Tables.Count > 0 && dsCarByCs.Tables[0].Rows.Count > 0)
								{
									foreach (DataRow dr in dsCarByCs.Tables[0].Rows)
									{
										if (!listCarID.Contains(int.Parse(dr["car_id"].ToString())))
										{
											listCarID.Add(int.Parse(dr["car_id"].ToString()));
										}
									}
								}
							}
						}
					}
				}
			}
			#endregion

			if (listCarID.Count > 0)
			{
				foreach (int carid in listCarID)
				{
					DataSet dsCarBase = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, string.Format(sqlCarBase, carid));
					if (dsCarBase != null && dsCarBase.Tables.Count > 0 && dsCarBase.Tables[0].Rows.Count > 0)
					{
						sb.AppendLine("<Car ID=\"" + dsCarBase.Tables[0].Rows[0]["car_id"].ToString() + "\" ");
						sb.AppendLine(" bs_name=\"" + dsCarBase.Tables[0].Rows[0]["bs_name"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						sb.AppendLine(" Car_YearType=\"" + dsCarBase.Tables[0].Rows[0]["Car_YearType"].ToString() + "\" ");
						sb.AppendLine(" cb_name=\"" + dsCarBase.Tables[0].Rows[0]["cb_name"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						sb.AppendLine(" csname=\"" + dsCarBase.Tables[0].Rows[0]["csname"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						sb.AppendLine(" car_name=\"" + dsCarBase.Tables[0].Rows[0]["car_name"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						sb.AppendLine(" p1=\"" + dsCarBase.Tables[0].Rows[0]["p1"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						sb.AppendLine(" p3=\"" + dsCarBase.Tables[0].Rows[0]["p3"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						sb.AppendLine(" p4=\"" + dsCarBase.Tables[0].Rows[0]["p4"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						sb.AppendLine(" p5=\"" + dsCarBase.Tables[0].Rows[0]["p5"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						sb.AppendLine(" p7=\"" + dsCarBase.Tables[0].Rows[0]["p7"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						sb.AppendLine(" p9=\"" + dsCarBase.Tables[0].Rows[0]["p9"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						sb.AppendLine(" c1=\"" + dsCarBase.Tables[0].Rows[0]["c1"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						sb.AppendLine(" c2=\"" + dsCarBase.Tables[0].Rows[0]["c2"].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");

						// 安全配置 操控技术->安全配置
						string g1 = GetParamGroup(carid, "600011");
						if (g1 != "")
						{ g1 += ";"; }
						g1 += GetParamGroup(carid, "600016");
						sb.AppendLine(" G1=\"" + g1.Replace("&", "&amp;").Replace("\"", "'") + "\" ");

						// 座椅配置->内部配置
						string g2 = GetParamGroup(carid, "600015");
						sb.AppendLine(" G2=\"" + g2.Replace("&", "&amp;").Replace("\"", "'") + "\" ");

						// 多媒体 行车电脑、定速巡航、倒车雷达->舒适与便利性配置
						string g3 = GetParamGroup(carid, "600013");
						if (g3 != "")
						{ g3 += ";"; }
						g3 += GetParamList(carid, "500,545,702");
						sb.AppendLine(" G3=\"" + g3.Replace("&", "&amp;").Replace("\"", "'") + "\" ");
						// GetParamGroup(carid, "600013", ref sb);

						// 操控技术
						// GetParamGroup(carid, "600016", ref sb);

						// 行车电脑、定速巡航、倒车雷达
						// GetParamList(carid, "List1", "500,545,702", ref sb);

						//sb.AppendLine("");
						//sb.AppendLine("");
						sb.AppendLine("/>");
					}
				}
			}

			sb.AppendLine("</CarList>");
		}

		/// <summary>
		/// 根据分类取车型参数
		/// </summary>
		/// <param name="carid"></param>
		/// <param name="group"></param>
		/// <param name="_sb"></param>
		private string GetParamGroup(int carid, string group)
		{
			string sqlParamGroup = @"select cdb.carid,cdb.paramid,cdb.pvalue,pl.paramname
                            from dbo.Car_relation car
                            left join dbo.CarDataBase cdb 
                            on car.car_id=cdb.carid
                            left join paramlist pl on cdb.paramid=pl.paramid
                            where car.car_id={0} and cdb.pvalue<>'0' and cdb.pvalue<>'无'
                            and cdb.pvalue<>'待查' and cdb.pvalue not like '%参数%' 
                            and cdb.paramid in
                            (
	                            select paramid from paramlist 
	                            where gradenum like '{1}%'
	                            and isState=1 and isParent=0
                            )";

			DataSet dsGroup = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, string.Format(sqlParamGroup, carid, group));
			string param = "";
			if (dsGroup != null && dsGroup.Tables.Count > 0 && dsGroup.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsGroup.Tables[0].Rows)
				{
					if (param != "")
					{ param += ";"; }
					if (dr["pvalue"].ToString().Trim() == "有")
					{
						param += dr["paramname"].ToString().Trim();
					}
					else
					{
						param += dr["paramname"].ToString().Trim() + "(" + dr["pvalue"].ToString().Trim() + ")";
					}
				}
			}
			return param;
		}

		/// <summary>
		/// 根据参数列表取
		/// </summary>
		/// <param name="carid"></param>
		/// <param name="key"></param>
		/// <param name="pList"></param>
		/// <param name="_sb"></param>
		private string GetParamList(int carid, string pList)
		{
			string sqlParamList = @"select cdb.carid,cdb.paramid,cdb.pvalue,pl.paramname
                        from dbo.Car_relation car
                        left join dbo.CarDataBase cdb 
                        on car.car_id=cdb.carid
                        left join paramlist pl on cdb.paramid=pl.paramid
                        where car.car_id={0} and cdb.pvalue<>'0' and cdb.pvalue<>'无'
                        and cdb.pvalue<>'待查' and cdb.pvalue not like '%参数%' 
                        and cdb.paramid in
                        ({1})";
			DataSet dsParam = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, string.Format(sqlParamList, carid, pList));
			string param = "";
			if (dsParam != null && dsParam.Tables.Count > 0 && dsParam.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsParam.Tables[0].Rows)
				{
					if (param != "")
					{ param += ";"; }
					if (dr["pvalue"].ToString().Trim() == "有")
					{
						param += dr["paramname"].ToString().Trim();
					}
					else
					{
						param += dr["paramname"].ToString().Trim() + "(" + dr["pvalue"].ToString().Trim() + ")";
					}
				}
			}
			return param;
		}

		/// <summary>
		/// 取所有车型颜色
		/// modified by chengl Dec.15.2011
		/// 增加内饰图数据输出
		/// </summary>
		private void GetCarColor()
		{
			Dictionary<int, Dictionary<string, SerialColorStruct>> dicCsColor = new Dictionary<int, Dictionary<string, SerialColorStruct>>();
			// modified by chengl Dec.15.2011
			// string sqlCsRGB = "select autoID,cs_id,colorName,colorRGB from Car_SerialColor where [type] = 0 or [type] = 2";
			string sqlCsRGB = "select autoID,cs_id,colorName,colorRGB from Car_SerialColor where {0} ";
			// colorType (0:车身颜色 1:内饰颜色)
			if (colorType == 0)
			{
				// 车身颜色
				sqlCsRGB = string.Format(sqlCsRGB, " [type] = 0 or [type] = 2 ");
			}
			else if (colorType == 1)
			{
				// 内饰颜色
				sqlCsRGB = string.Format(sqlCsRGB, " [type] = 1 ");
			}
			else
			{
				// 不满足条件 返回
				return;
			}

			DataSet dsCsRGB = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlCsRGB);
			if (dsCsRGB != null && dsCsRGB.Tables.Count > 0 && dsCsRGB.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsCsRGB.Tables[0].Rows)
				{
					int csid = int.Parse(dr["cs_id"].ToString());
					string colorName = dr["colorName"].ToString().Trim();
					SerialColorStruct scs = new SerialColorStruct();
					scs.AutoID = int.Parse(dr["autoID"].ToString());
					scs.CsID = csid;
					scs.ColorName = colorName;
					scs.ColorRGB = dr["colorRGB"].ToString().Trim();
					if (dicCsColor.ContainsKey(csid))
					{
						// 包含子品牌
						if (!dicCsColor[csid].ContainsKey(colorName))
						{
							// 不包含颜色名
							dicCsColor[csid].Add(colorName, scs);
						}
					}
					else
					{
						Dictionary<string, SerialColorStruct> dic = new Dictionary<string, SerialColorStruct>();
						dic.Add(colorName, scs);
						dicCsColor.Add(csid, dic);
					}
				}
			}


			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<CarColor>");
			// modified by chengl Dec.15.2011
			//        string sqlAllCarColor = @"select car.cs_id,car.car_id,cdb.pvalue
			//						from car_relation car
			//						left join car_serial cs on car.cs_id=cs.cs_id
			//						left join carDataBase cdb on car.car_id=cdb.carid and cdb.paramid=598
			//						where car.isState=0 and cs.isState=0 and cdb.pvalue <>''
			//						order by car.car_id";
			string sqlAllCarColor = @"select car.cs_id,car.car_id,cdb.pvalue
						from car_relation car
						left join car_serial cs on car.cs_id=cs.cs_id
						left join carDataBase cdb on car.car_id=cdb.carid and cdb.paramid={0}
						where car.isState=0 and cs.isState=0 and cdb.pvalue <>''
						order by car.car_id";
			if (colorType == 0)
			{
				// 车身颜色
				sqlAllCarColor = string.Format(sqlAllCarColor, 598);
			}
			else if (colorType == 1)
			{
				// 内饰颜色
				sqlAllCarColor = string.Format(sqlAllCarColor, 801);
			}
			else
			{
				// 不满足条件 返回
				return;
			}
			DataSet dsAllCarColor = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlAllCarColor);
			if (dsAllCarColor != null && dsAllCarColor.Tables.Count > 0 && dsAllCarColor.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsAllCarColor.Tables[0].Rows)
				{
					int carid = int.Parse(dr["car_id"].ToString());
					int csid = int.Parse(dr["cs_id"].ToString());
					string colors = dr["pvalue"].ToString().Trim().Replace("，", ",");

					if (carIDInt > 0 && carIDInt != carid)
					{ continue; }

					sb.AppendLine("<Car ID=\"" + carid.ToString() + "\" CsID=\"" + csid.ToString() + "\">");
					string[] arrColor = colors.Split(',');
					if (arrColor.Length > 0)
					{
						foreach (string color in arrColor)
						{
							if (color.Trim() != "")
							{
								sb.Append("<Color Name=\"" + color.Trim() + "\" ");
								if (dicCsColor.ContainsKey(csid) && dicCsColor[csid].ContainsKey(color.Trim()))
								{ sb.Append("RGB=\"" + dicCsColor[csid][color.Trim()].ColorRGB + "\" ID=\"" + dicCsColor[csid][color.Trim()].AutoID + "\"/>"); }
								else
								{ sb.AppendLine("RGB=\"\" ID=\"\"/>"); }
							}
						}
					}
					sb.AppendLine("</Car>");
				}
			}
			sb.AppendLine("</CarColor>");
		}

		private struct SerialColorStruct
		{
			public int AutoID;
			public int CsID;
			public string ColorName;
			public string ColorRGB;
		}
	}
}