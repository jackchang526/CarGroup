using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;
using System.Data.SqlClient;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.CarInfo
{
	/// <summary>
	/// 包括停销车型按子品牌、年款分组 级联列表(互动产品熊玉辉dept:bitautocheguanjia)
	/// bitautophoto:图库刘荣威 所有车型按年款、排量、变速器、厂商指导价排序
	/// </summary>
	public partial class CarListGroupByYear : InterfacePageBase
	{
		private string dept = "";
		private StringBuilder sb = new StringBuilder();
		private bool isAllSale = true;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			if (!this.IsPostBack)
			{
				GetPageParam();
				if (dept == "bitautocheguanjia")
				{
					GetCarListGroupbyYear();
				}
				else if (dept == "bitautophoto")
				{
					GetCarListOrderby();
				}
				else if (dept == "bitautocarpv")
				{
					GetCarPVListOrderbyEE();
				}
				else if (dept == "csyear")
				{
					// 只取子品牌年款
					GetCsYearList();
				}
				else
				{ }
				Response.Write(sb.ToString());
			}
		}

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{
			if (this.Request.QueryString["dept"] != null && this.Request.QueryString["dept"].ToString() != "")
			{
				dept = this.Request.QueryString["dept"].ToString().ToLower();
			}
			if (this.Request.QueryString["isAllSale"] != null && this.Request.QueryString["isAllSale"].ToString() != "")
			{
				if (this.Request.QueryString["isAllSale"].ToString().ToLower() == "0")
				{ isAllSale = false; }
			}
		}

		//    /// <summary>
		//    /// 取车型列表 按子品牌和年款分组
		//    /// modified by chengl Dec.26.2011 为车型节点增加属性
		//    /// </summary>
		//    private void GetCarListGroupbyYear()
		//    {
		//        sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
		//        sb.Append("<Root>");
		//        string sql = @"select car.car_id,car.car_name,cs.cs_id, 
		//                    (case when car.Car_YearType is null then '无' else CONVERT(varchar(50),car.Car_YearType)+ ' 款' end) as Car_YearType,cei.Engine_Exhaust 
		//                     ,(case when cei.UnderPan_TransmissionType like '%手动' then 1 
		//                     when cei.UnderPan_TransmissionType like '%自动' then 2 
		//                     when cei.UnderPan_TransmissionType like '%手自一体' then 3  
		//                     else 4 end) as TransmissionType 
		//                     ,car.car_ReferPrice,car.Car_YearType as CarYearType
		//				  from car_basic car 
		//                     left join car_serial cs on car.cs_id=cs.cs_id 
		//                     left join dbo.Car_Extend_Item cei on car.car_id = cei.car_id 
		//                     where car.isState=1 and cs.isState=1
		//                     order by cs_id,car.Car_YearType desc,TransmissionType,car.car_ReferPrice";
		//        DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
		//        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
		//        {
		//            string currentCS = "";
		//            string currentYear = "";
		//            BitAuto.CarChannel.BLL.Car_BasicBll basicBll = new BitAuto.CarChannel.BLL.Car_BasicBll();
		//            // modified by chengl Apr.11.2012
		//            Dictionary<int, string> dic423 = basicBll.GetCarParamExDic(423);
		//            Dictionary<int, string> dic785 = basicBll.GetCarParamExDic(785);
		//            Dictionary<int, string> dic712 = basicBll.GetCarParamExDic(712);
		//            Dictionary<int, string> dic436 = basicBll.GetCarParamExDic(436);
		//            Dictionary<int, string> dic578 = basicBll.GetCarParamExDic(578);
		//            Dictionary<int, string> dic430 = basicBll.GetCarParamExDic(430);
		//            Dictionary<int, string> dic655 = basicBll.GetCarParamExDic(655);
		//            Dictionary<int, string> dic669 = basicBll.GetCarParamExDic(669);
		//            Dictionary<int, string> dic667 = basicBll.GetCarParamExDic(667);
		//            Dictionary<int, string> dic425 = basicBll.GetCarParamExDic(425);
		//            foreach (DataRow dr in ds.Tables[0].Rows)
		//            {
		//                if (currentCS == "")
		//                {
		//                    // 第1行
		//                    currentCS = dr["cs_id"].ToString().Trim();
		//                    currentYear = dr["Car_YearType"].ToString().Trim();
		//                    sb.Append("<Serial CsID=\"" + currentCS + "\" >");
		//                    sb.Append("<CarYear Year=\"" + currentYear + "\" >");
		//                }
		//                else
		//                {
		//                    if (currentCS == dr["cs_id"].ToString().Trim())
		//                    {
		//                        // 相同子品牌
		//                        if (currentYear == dr["Car_YearType"].ToString().Trim())
		//                        {
		//                            // 相同年款
		//                        }
		//                        else
		//                        {
		//                            // 不同年款
		//                            sb.Append("</CarYear>");
		//                            currentYear = dr["Car_YearType"].ToString().Trim();
		//                            sb.Append("<CarYear Year=\"" + currentYear + "\" >");
		//                        }
		//                    }
		//                    else
		//                    {
		//                        // 不同子品牌
		//                        sb.Append("</CarYear>");
		//                        sb.Append("</Serial>");
		//                        currentCS = dr["cs_id"].ToString().Trim();
		//                        currentYear = dr["Car_YearType"].ToString().Trim();
		//                        sb.Append("<Serial CsID=\"" + currentCS + "\" >");
		//                        sb.Append("<CarYear Year=\"" + currentYear + "\" >");
		//                    }
		//                }
		//                // modified by chengl Dec.26.2011
		//                // sb.Append("<Car CarID=\"" + dr["car_id"].ToString().Trim() + "\" CarName=\"" + dr["car_name"].ToString().Trim().Replace("&","＆").Replace("\"","'") + "\" />");
		//                sb.Append("<Car CarID=\"" + dr["car_id"].ToString().Trim() + "\" CarName=\"" + dr["car_name"].ToString().Trim().Replace("&", "＆").Replace("\"", "'") + "\" ");
		//                sb.Append(" CarYearType=\"" + dr["CarYearType"].ToString().Trim() + "\" ReferPrice=\"" + dr["car_ReferPrice"].ToString() + "\" ");
		//                //根据车型ID取参数
		//                int carId = ConvertHelper.GetInteger(dr["car_id"]);
		//                // modified by chengl Apr.11.2012
		//                sb.AppendFormat(" EngineExhaust=\"{0}\" ", dic423.ContainsKey(carId) ? dic423[carId] : "");
		//                sb.AppendFormat(" EngineExhaustForFloat=\"{0}\" ", dic785.ContainsKey(carId) ? dic785[carId] : "");
		//                sb.AppendFormat(" UnderPanTransmissionType=\"{0}\" ", dic712.ContainsKey(carId) ? dic712[carId] : "");
		//                sb.AppendFormat(" EngineType=\"{0}\" ", dic436.ContainsKey(carId) ? System.Security.SecurityElement.Escape(dic436[carId]) : "");
		//                sb.AppendFormat(" OilFuelType=\"{0}\" ", dic578.ContainsKey(carId) ? dic578[carId] : "");
		//                sb.AppendFormat(" EngineMaxPower=\"{0}\" ", dic430.ContainsKey(carId) ? dic430[carId] : "");
		//                sb.AppendFormat(" PerfDriveType=\"{0}\" ", dic655.ContainsKey(carId) ? dic655[carId] : "");
		//                sb.AppendFormat(" PerfWeight=\"{0}\" ", dic669.ContainsKey(carId) ? dic669[carId] : "");
		//                sb.AppendFormat(" PerfTonnage=\"{0}\" ", dic667.ContainsKey(carId) ? dic667[carId] : "");
		//                string InhaleType = "";
		//                string Engine_InhaleType = dic425.ContainsKey(carId) ? dic425[carId] : "";
		//                //Dictionary<int, string> dict = basicBll.GetCarAllParamByCarID(carId);
		//                //sb.AppendFormat(" EngineExhaust=\"{0}\" ", dict.ContainsKey(423) ? dict[423] : "");
		//                //sb.AppendFormat(" EngineExhaustForFloat=\"{0}\" ", dict.ContainsKey(785) ? dict[785] : "");
		//                //sb.AppendFormat(" UnderPanTransmissionType=\"{0}\" ", dict.ContainsKey(712) ? dict[712] : "");
		//                //sb.AppendFormat(" EngineType=\"{0}\" ", dict.ContainsKey(436) ? System.Security.SecurityElement.Escape(dict[436]) : "");
		//                //sb.AppendFormat(" OilFuelType=\"{0}\" ", dict.ContainsKey(578) ? dict[578] : "");
		//                //sb.AppendFormat(" EngineMaxPower=\"{0}\" ", dict.ContainsKey(430) ? dict[430] : "");
		//                //sb.AppendFormat(" PerfDriveType=\"{0}\" ", dict.ContainsKey(655) ? dict[655] : "");
		//                //sb.AppendFormat(" PerfWeight=\"{0}\" ", dict.ContainsKey(669) ? dict[669] : "");
		//                //sb.AppendFormat(" PerfTonnage=\"{0}\" ", dict.ContainsKey(667) ? dict[667] : "");
		//                //string InhaleType = "";
		//                //string Engine_InhaleType = dict.ContainsKey(425) ? dict[425] : "";
		//                if (Engine_InhaleType == "增压")
		//                { InhaleType = "T"; }
		//                // 高总的逻辑 除了 T 其他都是L 
		//                // add by chengl Mar.5.2012
		//                //else if (Engine_InhaleType == "待查" || Engine_InhaleType == "")
		//                //{ InhaleType = ""; }
		//                else
		//                { InhaleType = "L"; }
		//                sb.Append(" InhaleType=\"" + InhaleType + "\" ");
		//                // 车型报价
		//                sb.Append(" CarPriceRange=\"" + GetCarPriceRangeByID(int.Parse(dr["car_id"].ToString())) + "\" ");
		//                sb.Append("/>");
		//            }
		//            sb.Append("</CarYear>");
		//            sb.Append("</Serial>");
		//        }
		//        sb.Append("</Root>");
		//    }

		/// <summary>
		/// 取车型列表 按子品牌和年款分组
		/// modified by chengl Dec.26.2011 为车型节点增加属性
		/// </summary>
		private void GetCarListGroupbyYear()
		{
			Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			Response.Write("<Root>");
			int serialId = ConvertHelper.GetInteger(Request.QueryString["csid"]);
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
                     where car.isState=1 and cs.isState=1 {0}
                     order by cs_id,car.Car_YearType desc,TransmissionType,car.car_ReferPrice";
			if (serialId > 0)
				sql = string.Format(sql, "and cs.cs_id=@csid");
			else
				sql = string.Format(sql, "");
			SqlParameter[] param = { new SqlParameter("@csid", SqlDbType.Int) };
			param[0].Value = serialId;
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, param);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				string currentCS = "";
				string currentYear = "";
				BitAuto.CarChannel.BLL.Car_BasicBll basicBll = new BitAuto.CarChannel.BLL.Car_BasicBll();
				// modified by chengl Apr.11.2012
				Dictionary<int, string> dic423 = basicBll.GetCarParamExDic(423);
				Dictionary<int, string> dic785 = basicBll.GetCarParamExDic(785);
				Dictionary<int, string> dic712 = basicBll.GetCarParamExDic(712);
				Dictionary<int, string> dic436 = basicBll.GetCarParamExDic(436);
				Dictionary<int, string> dic578 = basicBll.GetCarParamExDic(578);
				Dictionary<int, string> dic430 = basicBll.GetCarParamExDic(430);
				Dictionary<int, string> dic655 = basicBll.GetCarParamExDic(655);
				Dictionary<int, string> dic669 = basicBll.GetCarParamExDic(669);
				Dictionary<int, string> dic667 = basicBll.GetCarParamExDic(667);
				Dictionary<int, string> dic425 = basicBll.GetCarParamExDic(425);
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					if (currentCS == "")
					{
						// 第1行
						currentCS = dr["cs_id"].ToString().Trim();
						currentYear = dr["Car_YearType"].ToString().Trim();
						Response.Write("<Serial CsID=\"" + currentCS + "\" >");
						Response.Write("<CarYear Year=\"" + currentYear + "\" >");
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
								Response.Write("</CarYear>");
								currentYear = dr["Car_YearType"].ToString().Trim();
								Response.Write("<CarYear Year=\"" + currentYear + "\" >");
							}
						}
						else
						{
							// 不同子品牌
							Response.Write("</CarYear>");
							Response.Write("</Serial>");
							currentCS = dr["cs_id"].ToString().Trim();
							currentYear = dr["Car_YearType"].ToString().Trim();
							Response.Write("<Serial CsID=\"" + currentCS + "\" >");
							Response.Write("<CarYear Year=\"" + currentYear + "\" >");
						}
					}
					// modified by chengl Dec.26.2011
					// sb.Append("<Car CarID=\"" + dr["car_id"].ToString().Trim() + "\" CarName=\"" + dr["car_name"].ToString().Trim().Replace("&","＆").Replace("\"","'") + "\" />");
					Response.Write("<Car CarID=\"" + dr["car_id"].ToString().Trim() + "\" CarName=\"" + dr["car_name"].ToString().Trim().Replace("&", "＆").Replace("\"", "'") + "\" ");
					Response.Write(" CarYearType=\"" + dr["CarYearType"].ToString().Trim() + "\" ReferPrice=\"" + dr["car_ReferPrice"].ToString() + "\" ");
					//根据车型ID取参数
					int carId = ConvertHelper.GetInteger(dr["car_id"]);
					// modified by chengl Apr.11.2012
					Response.Write(string.Format(" EngineExhaust=\"{0}\" ", dic423.ContainsKey(carId) ? dic423[carId] : ""));
					Response.Write(string.Format(" EngineExhaustForFloat=\"{0}\" ", dic785.ContainsKey(carId) ? dic785[carId] : ""));
					Response.Write(string.Format(" UnderPanTransmissionType=\"{0}\" ", dic712.ContainsKey(carId) ? dic712[carId] : ""));
					Response.Write(string.Format(" EngineType=\"{0}\" ", dic436.ContainsKey(carId) ? System.Security.SecurityElement.Escape(dic436[carId]) : ""));
					Response.Write(string.Format(" OilFuelType=\"{0}\" ", dic578.ContainsKey(carId) ? dic578[carId] : ""));
					Response.Write(string.Format(" EngineMaxPower=\"{0}\" ", dic430.ContainsKey(carId) ? dic430[carId] : ""));
					Response.Write(string.Format(" PerfDriveType=\"{0}\" ", dic655.ContainsKey(carId) ? dic655[carId] : ""));
					Response.Write(string.Format(" PerfWeight=\"{0}\" ", dic669.ContainsKey(carId) ? dic669[carId] : ""));
					Response.Write(string.Format(" PerfTonnage=\"{0}\" ", dic667.ContainsKey(carId) ? dic667[carId] : ""));
					string InhaleType = "";
					string Engine_InhaleType = dic425.ContainsKey(carId) ? dic425[carId] : "";
					//Dictionary<int, string> dict = basicBll.GetCarAllParamByCarID(carId);
					//sb.AppendFormat(" EngineExhaust=\"{0}\" ", dict.ContainsKey(423) ? dict[423] : "");
					//sb.AppendFormat(" EngineExhaustForFloat=\"{0}\" ", dict.ContainsKey(785) ? dict[785] : "");
					//sb.AppendFormat(" UnderPanTransmissionType=\"{0}\" ", dict.ContainsKey(712) ? dict[712] : "");
					//sb.AppendFormat(" EngineType=\"{0}\" ", dict.ContainsKey(436) ? System.Security.SecurityElement.Escape(dict[436]) : "");
					//sb.AppendFormat(" OilFuelType=\"{0}\" ", dict.ContainsKey(578) ? dict[578] : "");
					//sb.AppendFormat(" EngineMaxPower=\"{0}\" ", dict.ContainsKey(430) ? dict[430] : "");
					//sb.AppendFormat(" PerfDriveType=\"{0}\" ", dict.ContainsKey(655) ? dict[655] : "");
					//sb.AppendFormat(" PerfWeight=\"{0}\" ", dict.ContainsKey(669) ? dict[669] : "");
					//sb.AppendFormat(" PerfTonnage=\"{0}\" ", dict.ContainsKey(667) ? dict[667] : "");
					//string InhaleType = "";
					//string Engine_InhaleType = dict.ContainsKey(425) ? dict[425] : "";
					if (Engine_InhaleType == "增压")
					{ InhaleType = "T"; }
					// 高总的逻辑 除了 T 其他都是L 
					// add by chengl Mar.5.2012
					//else if (Engine_InhaleType == "待查" || Engine_InhaleType == "")
					//{ InhaleType = ""; }
					else
					{ InhaleType = "L"; }
					Response.Write(" InhaleType=\"" + InhaleType + "\" ");
					// 车型报价
					Response.Write(" CarPriceRange=\"" + GetCarPriceRangeByID(int.Parse(dr["car_id"].ToString())) + "\" ");
					Response.Write("/>");
				}
				Response.Write("</CarYear>");
				Response.Write("</Serial>");
			}
			Response.Write("</Root>");
		}

		/// <summary>
		/// 取车型列表
		/// </summary>
		private void GetCarListOrderby()
		{
			string sql = @"select car.car_id,car.car_name,cs.cs_id, 
                    (case when car.Car_YearType is null then '无' else CONVERT(varchar(50),car.Car_YearType)+ ' 款' end) as Car_YearType,cei.Engine_Exhaust 
                     ,(case when cei.UnderPan_TransmissionType like '%手动' then 1 
                     when cei.UnderPan_TransmissionType like '%自动' then 2 
                     when cei.UnderPan_TransmissionType like '%手自一体' then 3  
                     else 4 end) as TransmissionType 
                     ,car.car_ReferPrice ,cdb1.pvalue as CarEngine_Exhaust,cdb2.pvalue as CarUnderPan_TransmissionType,cdb3.pvalue as CarEngine_ExhaustForFloat
                     from car_basic car 
                     left join car_serial cs on car.cs_id=cs.cs_id 
                     left join dbo.Car_Extend_Item cei on car.car_id = cei.car_id 
                     left join AutoStorageNew.AutoStorageNew.dbo.CarDataBase cdb1 on car.car_id=cdb1.carID and cdb1.paramid=423
                     left join AutoStorageNew.AutoStorageNew.dbo.CarDataBase cdb2 on car.car_id=cdb2.carID and cdb2.paramid=712
                     left join AutoStorageNew.AutoStorageNew.dbo.CarDataBase cdb3 on car.car_id=cdb3.carID and cdb3.paramid=785
                     where car.isState=1 and cs.isState=1
                     order by cs_id,car.Car_YearType desc,cei.Engine_Exhaust,TransmissionType,car.car_ReferPrice";
			DataSet dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);

			if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<Root>");
				for (int i = 0; i < dsCar.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Item CarID=\"" + dsCar.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
					sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(dsCar.Tables[0].Rows[i]["car_name"].ToString()) + "\" ");
					sb.Append(" CsID=\"" + dsCar.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					sb.Append(" CarYearType=\"" + dsCar.Tables[0].Rows[i]["Car_YearType"].ToString() + "\" ");
					sb.Append(" ReferPrice=\"" + dsCar.Tables[0].Rows[i]["car_ReferPrice"].ToString() + "\" ");
					sb.Append(" EngineExhaust=\"" + dsCar.Tables[0].Rows[i]["Engine_Exhaust"].ToString() + "\" ");
					sb.Append(" EngineExhaustForFloat=\"" + dsCar.Tables[0].Rows[i]["CarEngine_ExhaustForFloat"].ToString() + "\" ");
					sb.Append(" UnderPanTransmissionType=\"" + dsCar.Tables[0].Rows[i]["CarUnderPan_TransmissionType"].ToString() + "\" />");
				}
				sb.Append("</Root>");
			}
		}

		/// <sumary>
		/// 取车型列表 PV,年款,排量
		/// </summary>
		private void GetCarPVListOrderbyEE()
		{
			string sql = @"select cs.cs_id,cs.csname,car.car_id,car.car_name,car.Car_YearType,
car.car_ReferPrice,car.car_SaleState,car.car_ProduceState,
cdb2.pvalue as UnderPan_TransmissionType,cdb3.pvalue as Engine_ExhaustForFloat
from dbo.Car_relation car
left join dbo.Car_Serial cs on car.cs_id = cs.cs_id
left join dbo.CarDataBase cdb2 on car.car_id=cdb2.carID and cdb2.paramid=712
left join dbo.CarDataBase cdb3 on car.car_id=cdb3.carID and cdb3.paramid=785
where car.isState=0 and cs.isState=0 and car.Car_SaleState<>96
order by cs.cs_id,Engine_ExhaustForFloat,Car_YearType desc,car_ReferPrice";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				Hashtable ht = GetAllCarPv();

				sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.AppendLine("<Root>");
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					sb.AppendLine("<Car ID=\"" + ds.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
					sb.Append("CsID=\"" + ds.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					sb.Append("Name=\"" + ds.Tables[0].Rows[i]["car_name"].ToString() + "\" ");
					sb.Append("Year=\"" + ds.Tables[0].Rows[i]["Car_YearType"].ToString() + "\" ");
					sb.Append("RP=\"" + ds.Tables[0].Rows[i]["car_ReferPrice"].ToString() + "\" ");
					sb.Append("EEF=\"" + ds.Tables[0].Rows[i]["Engine_ExhaustForFloat"].ToString() + "\" ");
					sb.Append("TT=\"" + ds.Tables[0].Rows[i]["UnderPan_TransmissionType"].ToString() + "\" ");
					sb.Append("PR=\"" + base.GetCarPriceRangeByID(int.Parse(ds.Tables[0].Rows[i]["car_id"].ToString())) + "\" ");
					if (ht != null && ht.Count > 0 && ht.ContainsKey(ds.Tables[0].Rows[i]["car_id"].ToString()))
					{
						sb.Append("PV=\"" + ht[ds.Tables[0].Rows[i]["car_id"].ToString()].ToString() + "\" ");
					}
					else
					{
						sb.Append("PV=\"0\" ");
					}
					sb.Append("/>");
				}
				sb.AppendLine("</Root>");
			}
		}

		/// <summary>
		/// 取车型前天PV
		/// </summary>
		/// <returns></returns>
		private Hashtable GetAllCarPv()
		{
			Hashtable ht = new Hashtable();
			string sql = @"SELECT  CarId AS Car_Id, PVSum AS Pv_SumNum FROM Car_Basic_PV";

			SqlParameter[] _params = { 
										 new SqlParameter("@Date1",SqlDbType.DateTime),
										 new SqlParameter("@Date2",SqlDbType.DateTime)									 
									 };
			_params[0].Value = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
			_params[1].Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					if (!ht.ContainsKey(dr["Car_Id"].ToString()))
					{
						ht.Add(dr["Car_Id"].ToString(), dr["Pv_SumNum"].ToString());
					}
				}
			}
			return ht;
		}

		/// <summary>
		/// 输出子品牌年款
		/// </summary>
		private void GetCsYearList()
		{
			string sql = @"select cs_id,Car_YearType
							from dbo.Car_relation
							where cs_id>0 and Car_YearType>0
							and isState=0 
							{0}
							group by cs_id,Car_YearType
							order by cs_id,Car_YearType";
			if (!isAllSale)
			{ sql = string.Format(sql, " and car_SaleState<>96 "); }
			else
			{ sql = string.Format(sql, ""); }
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<Root>");
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				Dictionary<int, List<int>> dicCsYear = new Dictionary<int, List<int>>();
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int csid = int.Parse(dr["cs_id"].ToString());
					int year = int.Parse(dr["Car_YearType"].ToString());
					if (!dicCsYear.ContainsKey(csid))
					{
						List<int> csYear = new List<int>();
						csYear.Add(year);
						dicCsYear.Add(csid, csYear);
					}
					else
					{
						if (!dicCsYear[csid].Contains(year))
						{ dicCsYear[csid].Add(year); }
					}
				}
				if (dicCsYear.Count > 0)
				{
					foreach (KeyValuePair<int, List<int>> kvpCs in dicCsYear)
					{
						sb.AppendLine("<Cs ID=\"" + kvpCs.Key.ToString() + "\">");
						if (kvpCs.Value.Count > 0)
						{
							foreach (int year in kvpCs.Value)
							{
								sb.AppendLine("<Year ID=\"" + year.ToString() + "\"/>");
							}
						}
						sb.AppendLine("</Cs>");
					}
				}
			}
			sb.AppendLine("</Root>");
		}

	}
}