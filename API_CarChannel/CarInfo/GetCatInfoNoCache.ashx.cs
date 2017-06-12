using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetCatInfoNoCache 的摘要说明
	/// 
	/// 迁移至车型频道接口
	/// /interfaceforbitauto/CarInfo/CarInfoList.aspx?dept=carcolor&colortype=0&carid=104154
	/// add getcarinfobyid 迁移老接口 for zhuyx http://car.bitauto.com/interfaceforbitauto/GetAllCarInfo.aspx?isNoCache=1
	/// add mb\cb\cs 迁移老接口 for photo http://car.bitauto.com/interfaceforbitauto/Serial/SerialInfo.aspx?dept=photo&type=cs&typeid=1991
	/// </summary>
	public class GetCatInfoNoCache : PageBase, IHttpHandler
	{
		private HttpRequest request;
		private HttpResponse response;
		StringBuilder sbTemp = new StringBuilder();

		public void ProcessRequest(HttpContext context)
		{
			response = context.Response;
			request = context.Request;
			string type = request.QueryString["type"];
			if (!string.IsNullOrEmpty(type))
			{ type = type.ToLower(); }
			else
			{ Echo(""); return; }
			switch (type)
			{
				case "carlistgroupbyyear": GetCarListGroupbyYear(); break;
				case "carcolor": GetCarColor(); break;
				case "getcarinfobyid": GetCarInfoByID(); break;
				case "mb": GetTypeByID("mb"); break;
				case "cb": GetTypeByID("cb"); break;
				case "cs": GetTypeByID("cs"); break;
				default: Echo(""); break;
			}
		}

		#region 业务
		/// <summary>
		/// 取主品牌信息
		/// </summary>
		private void GetTypeByID(string type)
		{
			int typeid = BitAuto.Utils.ConvertHelper.GetInteger(request.QueryString["typeid"]);
			string sql = "";
			switch (type)
			{
				case "mb": sql = @"select 
							bs_id AS bsId,bs_Name,spell as bsSpell
							,isState as bsIsState,urlspell as bsallSpell
							,bs_seoname as bsSeoName,UpdateTime
							from dbo.Car_MasterBrand
							where bs_Id={0}"; break;
				case "cb": sql = @"SELECT  cb.cb_id AS cbId, cb.cb_Name, cb.spell AS cbSpell,
                                cb.allspell AS cbAllSpell, cb.cb_seoname AS cbSeoName,
                                cb.isState AS cbIsState, cb.cb_country AS cbCountry, bs_id AS bsId,
                                cb.UpdateTime
                        FROM    Car_Brand cb
                                LEFT JOIN dbo.Car_MasterBrand_Rel cmbr ON cb.cb_Id = cmbr.cb_Id
                                                                          AND cmbr.IsState = 0
                        WHERE   cb.cb_Id ={0}"; break;
				case "cs": sql = @"select 
							cs.cs_id as csId,cs.csName,cs.spell as csSpell
							,cs.isState as csIsState,cs.csSaleState,cs.csShowName
							,cs.cs_seoname as csSeoName,cs.allSpell as csAllSpell
							,cs.cb_id AS cbId,cl.classvalue as carlevel
							,cs.UpdateTime,cs.showSpell,cs.csEName
							from Car_Serial cs
							left join class cl on cs.carlevel=cl.classid
							where cs_Id={0}"; break;
				default: break;
			}
			if (sql != "" && typeid >= 0)
			{
				DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
					, CommandType.Text, string.Format(sql, typeid));
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						sbTemp.Append("<item>");
						foreach (DataColumn dc in ds.Tables[0].Columns)
						{
							sbTemp.AppendFormat("<{0}>{1}</{0}>"
								, System.Security.SecurityElement.Escape(dc.ColumnName)
								, System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i][dc.ColumnName].ToString().Trim()));
						}
						sbTemp.Append("</item>");
					}
				}
			}
			CommonFunction.EchoXml(response, sbTemp.ToString(), "CarInfo");
		}

		/// <summary>
		/// 取单一车型数据
		/// </summary>
		private void GetCarInfoByID()
		{
			int carid = 0;
			if (request.QueryString["carid"] != null && request.QueryString["carid"].ToString() != "")
			{
				if (int.TryParse(request.QueryString["carid"].ToString(), out carid))
				{ }
			}
			if (carid > 0)
			{
				string sql = @"select cs.cs_id,cs.csname,car.car_id,car.car_name,car.Car_YearType,car.updatetime,
				car.car_ReferPrice,car.car_SaleState,car.car_ProduceState,
				cdb1.pvalue as Engine_Exhaust,cdb2.pvalue as UnderPan_TransmissionType
				,cdb3.pvalue as Engine_ExhaustForFloat,cdb4.pvalue as Engine_InhaleType,c.classvalue as CarSaleState
				from dbo.Car_relation car
				left join dbo.Car_Serial cs on car.cs_id = cs.cs_id
				left join dbo.CarDataBase cdb1 on car.car_id=cdb1.carID and cdb1.paramid=423
				left join dbo.CarDataBase cdb2 on car.car_id=cdb2.carID and cdb2.paramid=712
				left join dbo.CarDataBase cdb3 on car.car_id=cdb3.carID and cdb3.paramid=785
				left join dbo.CarDataBase cdb4 on car.car_id=cdb4.carID and cdb4.paramid=425
				left join dbo.class c on  c.classid=car.car_SaleState
				where car.isState=0 and cs.isState=0 and car.car_id={0}  
				order by car.car_id";

				// 如果有车型ID 则忽略销售状态
				sql = string.Format(sql, carid);
				DataSet dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
				if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
				{
					sbTemp.Append("<Item CarID=\"" + dsCar.Tables[0].Rows[0]["car_id"].ToString() + "\" ");
					sbTemp.Append(" CarName=\"" + System.Security.SecurityElement.Escape(dsCar.Tables[0].Rows[0]["car_name"].ToString()) + "\" ");
					sbTemp.Append(" CsID=\"" + dsCar.Tables[0].Rows[0]["cs_id"].ToString() + "\" ");
					sbTemp.Append(" CarYearType=\"" + dsCar.Tables[0].Rows[0]["Car_YearType"].ToString() + "\" ");
					sbTemp.Append(" CarSaleState=\"" + dsCar.Tables[0].Rows[0]["CarSaleState"].ToString() + "\" ");
					sbTemp.Append(" ReferPrice=\"" + dsCar.Tables[0].Rows[0]["car_ReferPrice"].ToString() + "\" ");
					sbTemp.Append(" EngineExhaust=\"" + dsCar.Tables[0].Rows[0]["Engine_Exhaust"].ToString() + "\" ");
					sbTemp.Append(" EngineExhaustForFloat=\"" + dsCar.Tables[0].Rows[0]["Engine_ExhaustForFloat"].ToString() + "\" ");
					sbTemp.Append(" Engine_InhaleType=\"" + dsCar.Tables[0].Rows[0]["Engine_InhaleType"].ToString() + "\" ");
					sbTemp.Append(" UnderPanTransmissionType=\"" + dsCar.Tables[0].Rows[0]["UnderPan_TransmissionType"].ToString() + "\" ");
					sbTemp.Append(" UpdateTime=\"" + dsCar.Tables[0].Rows[0]["updatetime"].ToString() + "\" />");
				}
			}
			CommonFunction.EchoXml(response, sbTemp.ToString(), "Root");
		}

		/// <summary>
		/// 口碑取车型按年款分组数据
		/// </summary>
		private void GetCarListGroupbyYear()
		{

			string sql = @"select car.car_id,car.car_name,cs.cs_id, 
                    (case when car.Car_YearType is null then '无' else CONVERT(varchar(50),car.Car_YearType)+ ' 款' end) as Car_YearType,cei.Engine_Exhaust 
                     ,(case when cei.UnderPan_TransmissionType like '%手动' then 1 
                     when cei.UnderPan_TransmissionType like '%自动' then 2 
                     when cei.UnderPan_TransmissionType like '%手自一体' then 3  
                     else 4 end) as TransmissionType 
                     ,car.car_ReferPrice,car.Car_YearType as CarYearType
				  from car_basic car 
                     left join car_serial cs on car.cs_id=cs.cs_id 
                     left join dbo.Car_Extend_Item cei on car.car_id = cei.car_id 
                     where car.isState=1 and cs.isState=1 and cs.cs_id=@csid
                     order by cs_id,car.Car_YearType desc,TransmissionType,car.car_ReferPrice";
			string csidParam = request.QueryString["csid"];
			int csid = 0;
			if (!string.IsNullOrEmpty(csidParam) && int.TryParse(csidParam, out csid))
			{
				if (csid > 0)
				{
					SqlParameter[] param = { new SqlParameter("@csid", SqlDbType.Int) };
					param[0].Value = csid;
					DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, param);
					if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
					{
						string currentCS = "";
						string currentYear = "";
						BitAuto.CarChannel.BLL.Car_BasicBll basicBll = new BitAuto.CarChannel.BLL.Car_BasicBll();
						// modified by chengl Apr.11.2012
						Dictionary<int, string> dic423 = basicBll.GetCarParamExDicByCacheInterval(423, 1);
						Dictionary<int, string> dic785 = basicBll.GetCarParamExDicByCacheInterval(785, 1);
						Dictionary<int, string> dic712 = basicBll.GetCarParamExDicByCacheInterval(712, 1);
						Dictionary<int, string> dic436 = basicBll.GetCarParamExDicByCacheInterval(436, 1);
						Dictionary<int, string> dic578 = basicBll.GetCarParamExDicByCacheInterval(578, 1);
						Dictionary<int, string> dic430 = basicBll.GetCarParamExDicByCacheInterval(430, 1);
						Dictionary<int, string> dic655 = basicBll.GetCarParamExDicByCacheInterval(655, 1);
						Dictionary<int, string> dic669 = basicBll.GetCarParamExDicByCacheInterval(669, 1);
						Dictionary<int, string> dic667 = basicBll.GetCarParamExDicByCacheInterval(667, 1);
						Dictionary<int, string> dic425 = basicBll.GetCarParamExDicByCacheInterval(425, 1);
						foreach (DataRow dr in ds.Tables[0].Rows)
						{
							if (currentCS == "")
							{
								// 第1行
								currentCS = dr["cs_id"].ToString().Trim();
								currentYear = dr["Car_YearType"].ToString().Trim();
								// sbTemp.AppendLine("<Serial CsID=\"" + currentCS + "\" >");
								sbTemp.AppendLine("<CarYear Year=\"" + currentYear + "\" >");
							}
							else
							{
								if (currentCS == dr["cs_id"].ToString().Trim())
								{
									// 相同子品牌
									if (currentYear == dr["Car_YearType"].ToString().Trim())
									{
										// 相同年款
									}
									else
									{
										// 不同年款
										sbTemp.AppendLine("</CarYear>");
										currentYear = dr["Car_YearType"].ToString().Trim();
										sbTemp.AppendLine("<CarYear Year=\"" + currentYear + "\" >");
									}
								}
							}
							// modified by chengl Dec.26.2011
							sbTemp.AppendLine("<Car CarID=\"" + dr["car_id"].ToString().Trim() + "\" CarName=\"" +
							System.Security.SecurityElement.Escape(dr["car_name"].ToString()) + "\" ");
							sbTemp.Append(" CarYearType=\"" + dr["CarYearType"].ToString().Trim() + "\" ReferPrice=\"" + dr["car_ReferPrice"].ToString() + "\" ");
							//根据车型ID取参数
							int carId = BitAuto.Utils.ConvertHelper.GetInteger(dr["car_id"]);
							// modified by chengl Apr.11.2012
							sbTemp.Append(string.Format(" EngineExhaust=\"{0}\" ", dic423.ContainsKey(carId) ? dic423[carId] : ""));
							sbTemp.Append(string.Format(" EngineExhaustForFloat=\"{0}\" ", dic785.ContainsKey(carId) ? dic785[carId] : ""));
							sbTemp.Append(string.Format(" UnderPanTransmissionType=\"{0}\" ", dic712.ContainsKey(carId) ? dic712[carId] : ""));
							sbTemp.Append(string.Format(" EngineType=\"{0}\" ", dic436.ContainsKey(carId) ? System.Security.SecurityElement.Escape(dic436[carId]) : ""));
							sbTemp.Append(string.Format(" OilFuelType=\"{0}\" ", dic578.ContainsKey(carId) ? dic578[carId] : ""));
							sbTemp.Append(string.Format(" EngineMaxPower=\"{0}\" ", dic430.ContainsKey(carId) ? dic430[carId] : ""));
							sbTemp.Append(string.Format(" PerfDriveType=\"{0}\" ", dic655.ContainsKey(carId) ? dic655[carId] : ""));
							sbTemp.Append(string.Format(" PerfWeight=\"{0}\" ", dic669.ContainsKey(carId) ? dic669[carId] : ""));
							sbTemp.Append(string.Format(" PerfTonnage=\"{0}\" ", dic667.ContainsKey(carId) ? dic667[carId] : ""));
							string Engine_InhaleType = dic425.ContainsKey(carId) ? dic425[carId] : "";
							sbTemp.Append(" InhaleType=\"" + CommonFunction.GetInhaleType(Engine_InhaleType, "") + "\" ");
							// 车型报价
							sbTemp.Append(" CarPriceRange=\"" + GetCarPriceRangeByID(int.Parse(dr["car_id"].ToString())) + "\" ");
							sbTemp.Append("/>");
						}
						sbTemp.AppendLine("</CarYear>");
						// sbTemp.AppendLine("</Serial>");
					}
				}
			}
			Echo(sbTemp.ToString());
		}

		/// <summary>
		/// 迁移至车型频道接口 /interfaceforbitauto/CarInfo/CarInfoList.aspx?dept=carcolor&colortype=0&carid=104154
		/// 取所有车型颜色
		/// modified by chengl Dec.15.2011
		/// 增加内饰图数据输出
		/// </summary>
		private void GetCarColor()
		{
			int carIDInt = 0;
			int colorType = 0;
			if (request.QueryString["carid"] != null && request.QueryString["carid"].ToString() != "")
			{
				string caridStr = request.QueryString["carid"].ToString();
				if (int.TryParse(caridStr, out carIDInt))
				{ }
			}
			if (request.QueryString["colorType"] != null && request.QueryString["colorType"].ToString() != "")
			{
				int tempColorType = 0;
				if (int.TryParse(request.QueryString["colorType"].ToString(), out tempColorType))
				{
					// 默认为车身颜色，当制定颜色类型时取特定类型颜色(0:车身颜色 1:内饰颜色)
					if (tempColorType > 0)
					{ colorType = tempColorType; }
				}
			}
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

			DataSet dsCsRGB = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
				WebConfig.AutoStorageConnectionString, CommandType.Text, sqlCsRGB);
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


			sbTemp.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sbTemp.AppendLine("<CarColor>");
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

					sbTemp.AppendLine("<Car ID=\"" + carid.ToString() + "\" CsID=\"" + csid.ToString() + "\">");
					string[] arrColor = colors.Split(',');
					if (arrColor.Length > 0)
					{
						foreach (string color in arrColor)
						{
							if (color.Trim() != "")
							{
								sbTemp.Append("<Color Name=\"" + color.Trim() + "\" ");
								if (dicCsColor.ContainsKey(csid) && dicCsColor[csid].ContainsKey(color.Trim()))
								{ sbTemp.Append("RGB=\"" + dicCsColor[csid][color.Trim()].ColorRGB + "\" ID=\"" + dicCsColor[csid][color.Trim()].AutoID + "\"/>"); }
								else
								{ sbTemp.AppendLine("RGB=\"\" ID=\"\"/>"); }
							}
						}
					}
					sbTemp.AppendLine("</Car>");
				}
			}
			sbTemp.AppendLine("</CarColor>");
			Echo(sbTemp.ToString(), false);
		}

		#endregion

		private struct SerialColorStruct
		{
			public int AutoID;
			public int CsID;
			public string ColorName;
			public string ColorRGB;
		}

		/// <summary>
		/// 统一输出XML
		/// </summary>
		/// <param name="str"></param>
		private void Echo(string str)
		{
			HttpContext.Current.Response.ContentType = "text/xml";
			HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
			HttpContext.Current.Response.Clear();
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			sb.AppendLine("<Root>");
			sb.Append(str);
			sb.Append("</Root>");
			response.Write(sb.ToString());
		}

		/// <summary>
		/// 统一输出XML
		/// </summary>
		/// <param name="str"></param>
		/// <param name="isNeedRoot">是否需要声明和根节点</param>
		private void Echo(string str, bool isNeedRoot)
		{
			HttpContext.Current.Response.ContentType = "text/xml";
			HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
			HttpContext.Current.Response.Clear();
			StringBuilder sb = new StringBuilder();
			if (isNeedRoot)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				sb.AppendLine("<Root>");
			}
			sb.Append(str);
			if (isNeedRoot)
			{
				sb.Append("</Root>");
			}
			response.Write(sb.ToString());
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