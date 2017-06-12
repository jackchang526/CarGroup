using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.Serial
{
	/// <summary>
	/// 取所有子品牌信息
	/// dept:使用业务类型(bitautoipad:杨立峰ipad版所有子品牌厂商指导价区间)
	/// bitautocheyisou:刘艳昆 所有子品牌非停销车型数据
	/// bitautobbs:王泊 同车易搜 增加报价区间
	/// cig:徐永帅 子品牌还关注
	/// cmshasnewcs:朱永旭 有上市新车的子品牌，按国产、进口区分
	/// </summary>
	public partial class AllSerialInfo : InterfacePageBase
	{
		private string dept = "";
		private string sale = "";
		private StringBuilder sb = new StringBuilder();
		private Car_SerialBll serialBll = new Car_SerialBll();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				GetAllSerialInfoBydDept();
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
			// 销售状态
			if (this.Request.QueryString["sale"] != null && this.Request.QueryString["sale"].ToString() != "")
			{
				sale = this.Request.QueryString["sale"].ToString().ToLower();
			}
		}

		/// <summary>
		/// 根据业务和子品牌ID取子品牌信息
		/// </summary>
		private void GetAllSerialInfoBydDept()
		{
			if (dept == "bitautoipad")
			{
				GenerateAllSerialReferPrice();
			}
			else if (dept == "bitautocheyisou")
			{
				// 车易搜
				GenerateAllSerialForcheyisou(false);
			}
			else if (dept == "bitautobbs")
			{
				// 论坛
				GenerateAllSerialForcheyisou(true);
			}
			else if (dept == "cig")
			{
				// 子品牌还关注
				GenerateSerialToSerial();
			}
			else if (dept == "cmshasnewcs")
			{
				// 有新上市的子品牌
				GenerateHasNewSerialInfo();
			}
			else if (dept == "csuv")
			{
				// add by chengl 子品牌近30天UV排序
				GenerateSerial30DaySort();
			}
			else
			{ }
		}

		#region 业务

		/// <summary>
		/// 取所有子品牌厂商指导价区间
		/// </summary>
		private void GenerateAllSerialReferPrice()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<SerialReferPrice>");
			DataSet ds = base.GetAllSErialInfo();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					sb.AppendLine("<Serial ID=\"" + ds.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					sb.Append(" ReferPrice=\"" + GetSerialReferPriceByCsID(int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString())) + "\" />");
				}
			}
			sb.AppendLine("</SerialReferPrice>");
		}

		/// <summary>
		/// 所有子品牌信息for cheyisou
		/// </summary>
		/// <param name="isForBBS">是否需要论坛需要的字段</param>
		private void GenerateAllSerialForcheyisou(bool isForBBS)
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<SerialInfo>");
			DataSet ds = new DataSet();
			if (sale == "all")
			{
				// 全部销售状态
				ds = GetAllSerialReferPriceEETTBySale(true);
			}
			else
			{
				// 非停销
				ds = GetAllSerialReferPriceEETTBySale(false);
			}
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				int currentCsid = 0;
				string csName = "";
				string csShowName = "";
				string csAllSpell = "";

				string strRP = "";
				string strMinRP = "";
				string strMaxRP = "";
				string strEE = "";
				string strTT = "";

				SortedList<decimal, decimal> slPrice = new SortedList<decimal, decimal>();
				SortedList<string, string> slTT = new SortedList<string, string>();
				SortedList<string, string> slEE = new SortedList<string, string>();
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int rowCsid = int.Parse(dr["cs_id"].ToString());
					if (currentCsid != rowCsid)
					{
						// 新子品牌
						if (currentCsid > 0)
						{

							#region
							strRP = "";
							strMinRP = "";
							strMaxRP = "";
							strEE = "";
							strTT = "";
							// 指导价
							if (slPrice.Count > 0)
							{
								int loop = 0;
								foreach (KeyValuePair<decimal, decimal> kvp in slPrice)
								{
									if (loop == 0)
									{ strMinRP = kvp.Key.ToString(); }
									strMaxRP = kvp.Key.ToString();
									loop++;
								}
								strRP = strMinRP + "万-" + strMaxRP + "万";
							}
							// 排量
							if (slEE.Count > 0)
							{
								foreach (KeyValuePair<string, string> kvp in slEE)
								{
									if (strEE != "")
									{ strEE += ","; }
									strEE += kvp.Key + "L";
								}
							}
							// 变速箱
							if (slTT.Count > 0)
							{
								foreach (KeyValuePair<string, string> kvp in slTT)
								{
									if (strTT != "")
									{ strTT += ","; }
									strTT += kvp.Key;
								}
							}
							#endregion

							// 非第1行
							sb.AppendLine("<Cs ID=\"" + currentCsid.ToString() + "\" ");
							sb.Append(" CsName=\"" + csName + "\" ");
							sb.Append(" CsShowName=\"" + csShowName + "\" ");
							sb.Append(" ReferPrice=\"" + strRP + "\" ");
							if (isForBBS)
							{
								sb.Append(" PriceRange=\"" + GetSerialPriceRangeByID(currentCsid) + "\" ");
								sb.Append(" AllSpell=\"" + csAllSpell + "\" ");
								string firstImg = "";
								List<SerialFocusImage> imgList = serialBll.GetSerialFocusImageList(currentCsid);
								if (imgList.Count > 0)
								{
									firstImg = String.Format(imgList[0].ImageUrl, 4);
								}
								sb.Append(" FirstImg=\"" + firstImg + "\" ");
							}
							sb.Append(" EE=\"" + strEE + "\" ");
							sb.Append(" TT=\"" + strTT + "\" />");

						}

						currentCsid = rowCsid;
						csName = dr["csName"].ToString();
						csShowName = dr["csShowName"].ToString();
						csAllSpell = dr["AllSpell"].ToString().ToLower();

						slPrice.Clear();
						slTT.Clear();
						slEE.Clear();
					}
					decimal rp = 0;
					if (decimal.TryParse(dr["car_ReferPrice"].ToString(), out rp))
					{
						if (rp > 0 && !slPrice.ContainsKey(rp))
						{
							slPrice.Add(rp, rp);
						}
					}
					string ee = dr["EE"].ToString();
					if (ee != "" && !slEE.ContainsKey(ee))
					{ slEE.Add(ee, ee); }
					string tt = dr["TT"].ToString();
					if (tt != "" && !slTT.ContainsKey(tt))
					{ slTT.Add(tt, tt); }
				}
				#region
				// 指导价
				strRP = "";
				strMinRP = "";
				strMaxRP = "";
				strEE = "";
				strTT = "";
				if (slPrice.Count > 0)
				{
					int loop = 0;
					foreach (KeyValuePair<decimal, decimal> kvp in slPrice)
					{
						if (loop == 0)
						{ strMinRP = kvp.Key.ToString(); }
						strMaxRP = kvp.Key.ToString();
						loop++;
					}
					strRP = strMinRP + "万-" + strMaxRP + "万";
				}
				// 排量
				if (slEE.Count > 0)
				{
					foreach (KeyValuePair<string, string> kvp in slEE)
					{
						if (strEE != "")
						{ strEE += ","; }
						strEE += kvp.Key + "L";
					}
				}
				// 变速箱
				if (slTT.Count > 0)
				{
					foreach (KeyValuePair<string, string> kvp in slTT)
					{
						if (strTT != "")
						{ strTT += ","; }
						strTT += kvp.Key;
					}
				}
				#endregion
				sb.AppendLine("<Cs ID=\"" + currentCsid.ToString() + "\" ");
				sb.Append(" CsName=\"" + csName + "\" ");
				sb.Append(" CsShowName=\"" + csShowName + "\" ");
				sb.Append(" ReferPrice=\"" + strRP + "\" ");
				if (isForBBS)
				{
					sb.Append(" PriceRange=\"" + GetSerialPriceRangeByID(currentCsid) + "\" ");
					sb.Append(" AllSpell=\"" + csAllSpell + "\" ");
					string firstImg = "";
					List<SerialFocusImage> imgList = serialBll.GetSerialFocusImageList(currentCsid);
					if (imgList.Count > 0)
					{
						firstImg = String.Format(imgList[0].ImageUrl, 4);
					}
					sb.Append(" FirstImg=\"" + firstImg + "\" ");
				}
				sb.Append(" EE=\"" + strEE + "\" ");
				sb.Append(" TT=\"" + strTT + "\" />");
			}
			sb.AppendLine("</SerialInfo>");
		}

		/// <summary>
		/// 所有子品牌还关注
		/// </summary>
		private void GenerateSerialToSerial()
		{
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<SerialToSerial>");
			DataSet ds = GetSerialToSerial();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int csid = int.Parse(dr["cs_id"].ToString());
					string csName = System.Security.SecurityElement.Escape(dr["csShowName"].ToString().Trim());
					int pcs_id = int.Parse(dr["pcs_id"].ToString());
					string pcsName = System.Security.SecurityElement.Escape(dr["pcsShowName"].ToString().Trim());
					int count = int.Parse(dr["Pv_num"].ToString());

					sb.AppendLine("<Serial ID=\"" + csid + "\" ");
					sb.Append("Name=\"" + csName + "\" ");
					sb.Append("OtherID=\"" + pcs_id + "\" ");
					sb.Append("OtherName=\"" + pcsName + "\" ");
					sb.Append("Count=\"" + count + "\" />");
				}
			}
			sb.AppendLine("</SerialToSerial>");
		}

		/// <summary>
		/// 根据国产或进口 取有上市新车的子品牌信息(近3个月有上市时间的车型)
		/// </summary>
		private void GenerateHasNewSerialInfo()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<SerialInfo>");
			string country = this.Request["country"];
			if (country != null && country != "")
			{
				string countrySearchKey = "";
				List<int> listHasNew = GetAllHasNewSerial();
				DataSet ds = GetSerialOrderbyUV();
				if (country.ToLower() == "guochan")
				{
					// 取国产子品牌
					countrySearchKey = "国产";
				}
				else if (country.ToLower() == "jinkou")
				{
					// 取进口子品牌
					countrySearchKey = "进口";
				}
				else
				{ }
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						string Cp_Country = (dr["Cp_Country"].ToString().Trim() == "中国" ? "国产" : "进口");
						if (Cp_Country != countrySearchKey)
						{ continue; }
						int csid = int.Parse(dr["cs_id"].ToString());
						if (!listHasNew.Contains(csid))
						{ continue; }
						string csName = System.Security.SecurityElement.Escape(dr["cs_name"].ToString().Trim());
						string showName = System.Security.SecurityElement.Escape(dr["cs_showname"].ToString().Trim());
						string allSpell = dr["allSpell"].ToString().ToLower();

						sb.AppendLine("<Serial ID=\"" + csid + "\" ");
						sb.Append(" Name=\"" + csName + "\" ");
						sb.Append(" ShowName=\"" + showName + "\" ");
						sb.Append(" AllSpell=\"" + allSpell + "\" ");
						sb.Append(" PriceRange=\"" + GetSerialPriceRangeByID(csid) + "\" ");
						sb.Append(" ReferPrice=\"" + new Car_SerialBll().GetSerialOfficePriceBySaleState(csid, false) + "\" ");
						sb.Append("/>");
					}
				}
			}
			sb.AppendLine("</SerialInfo>");
		}

		/// <summary>
		/// 子品牌近30天UV排序
		/// </summary>
		private void GenerateSerial30DaySort()
		{
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<SerialSort>");
			DataSet ds = base.GetAllSerialNewly30Day();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int csid = int.Parse(dr["cs_ID"].ToString());
					int uvCount = int.Parse(dr["Pv_SumNum"].ToString());
					string csName = dr["cs_name"].ToString().Trim();
					sb.AppendLine("<Serial ID=\"" + csid + "\" Name=\"" + System.Security.SecurityElement.Escape(csName) + "\" UV=\"" + uvCount + "\" />");
				}
			}
			sb.AppendLine("</SerialSort>");
		}

		#endregion

		#region 取数据

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
				serialReferPrice = Math.Round(minPrice, 2) + "万-" + Math.Round(maxPrice, 2) + "万";
			}
			return serialReferPrice;
		}

		/// <summary>
		/// 取所有子品牌指导价 排量 变速箱
		/// </summary>
		/// <param name="isHasNoSale"></param>
		/// <returns></returns>
		private DataSet GetAllSerialReferPriceEETTBySale(bool isHasNoSale)
		{
			DataSet ds = new DataSet();
			string sql = @"select cs.csname,cs.csshowname,cs.AllSpell,
                            car.car_id,car.cs_id,car.car_ReferPrice,
                            cdb1.pvalue as TT ,cdb2.pvalue as EE
                            from dbo.Car_relation car
                            left join car_serial cs on car.cs_id=cs.cs_id
                            left join dbo.CarDataBase cdb1 on car.car_id=cdb1.carid and cdb1.paramid=712
                            left join dbo.CarDataBase cdb2 on car.car_id=cdb2.carid and cdb2.paramid=785
                            where car.isState=0 and cs.isState=0 {0}
                            order by car.cs_id,car.car_ReferPrice";
			if (isHasNoSale)
			{
				// 包含停销车
				sql = string.Format(sql, "");
			}
			else
			{
				// 不包含停销车
				sql = string.Format(sql, " and car.car_SaleState<>96 ");
			}
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
			return ds;
		}

		/// <summary>
		/// 取子品牌还关注
		/// </summary>
		/// <returns></returns>
		private DataSet GetSerialToSerial()
		{
			string sql = @"select sts.cs_id,rtrim(cs.cs_showname) as csShowName,sts.pcs_id,
							rtrim(cs1.cs_showname) as pcsShowName,sts.Pv_num
							from dbo.Serial_To_Serial sts
							left join car_serial cs on sts.cs_id = cs.cs_id
							left join car_serial cs1 on sts.pcs_id = cs1.cs_id
							order by cs_id,pv_num desc";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString,
				CommandType.Text, sql);
			return ds;
		}

		/// <summary>
		/// 根据子品牌UV取子品牌
		/// </summary>
		/// <returns></returns>
		private DataSet GetSerialOrderbyUV()
		{
			string sql = @"SELECT  cs.cs_id, cs.cb_id, cs.cs_CarLevel, cb.cb_Country AS Cp_Country,
                                    cs.cs_name, cs.cs_showname, cs.allSpell
                            FROM    car_serial cs
                                    LEFT JOIN dbo.Car_Serial_30UV cs3 ON cs.cs_id = cs3.cs_id
                                    LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                            WHERE   cs.isState = 1
                                    AND cs.cs_CarLevel <> '概念车'
                                    AND cb.isState = 1
                            ORDER BY cs3.UVCount DESC, cs.cs_id";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString,
				CommandType.Text, sql);
			return ds;
		}

		/// <summary>
		/// 取所有有上次新车的子品牌
		/// (近3个月的 上市时间)
		/// </summary>
		/// <returns></returns>
		private List<int> GetAllHasNewSerial()
		{
			List<int> list = new List<int>();
			DataSet ds = base.GetAllNewCars();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int csid = int.Parse(dr["cs_id"].ToString());
					if (csid > 0 && !list.Contains(csid))
					{ list.Add(csid); }
				}
			}
			return list;
		}

		#endregion
	}
}