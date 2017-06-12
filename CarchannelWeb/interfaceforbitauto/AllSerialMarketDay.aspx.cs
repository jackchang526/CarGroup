using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Xml;
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
	/// 所有子品牌的上市日期(当前时间前的最新日期)、厂商指导价(最小、最大) (田树风)
	/// </summary>
	public partial class AllSerialMarketDay : InterfacePageBase
	{
		private DataTable dtSerial = new DataTable();
		private Hashtable htCsPV = new Hashtable();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				dtSerial = InitTable();

				//sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				//sb.Append("<SerialList>");
				this.GetAllSerialMarketDay();
				//sb.Append("</SerialList>");
				//Response.Write(sb.ToString());
				DataSet ds = new DataSet();
				if (dtSerial != null && dtSerial.Rows.Count > 0)
				{
					ds.Tables.Add(dtSerial);
				}
				// ds.WriteXml(Response.OutputStream);

				XmlTextWriter writer = new XmlTextWriter(Response.OutputStream, Response.ContentEncoding);
				try
				{
					writer.Formatting = Formatting.Indented;
					writer.Indentation = 4;
					writer.IndentChar = ' ';
					writer.WriteStartDocument();
					ds.WriteXml(writer, XmlWriteMode.WriteSchema);
					writer.Flush();
					Response.End();
					writer.Close();
				}
				catch
				{
					Response.End();
					writer.Close();
				}
			}
		}

		private void GetAllSerialMarketDay()
		{
			DataSet dsCsPV = new DataSet();
			string sqlCsPV = " select CS_Id,Pv_SumNum from dbo.Serial_PvRank ";
			dsCsPV = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCsPV);
			if (dsCsPV != null && dsCsPV.Tables.Count > 0 && dsCsPV.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsCsPV.Tables[0].Rows.Count; i++)
				{
					if (!htCsPV.ContainsKey(dsCsPV.Tables[0].Rows[i]["CS_Id"].ToString().Trim()))
					{
						htCsPV.Add(dsCsPV.Tables[0].Rows[i]["CS_Id"].ToString().Trim(), dsCsPV.Tables[0].Rows[i]["Pv_SumNum"].ToString().Trim());
					}
				}
			}

			DataSet ds = new DataSet();
			if (HttpContext.Current.Cache.Get("interfaceforbitauto_AllSerialMarketDay") != null)
			{
				ds = (System.Data.DataSet)HttpContext.Current.Cache.Get("interfaceforbitauto_AllSerialMarketDay");
			}
			else
			{
				string sql = " select cs.cs_id,cs.csShowName,car.car_id,car.car_name,car.car_ReferPrice,cdb1.Pvalue as marketyear,cdb2.Pvalue as marketmonth,cdb3.Pvalue as marketday ";
				sql += " from dbo.Car_relation car ";
				sql += " left join Car_serial cs on car.cs_id = cs.cs_id ";
				sql += " left join dbo.CarDataBase cdb1 on car.car_id = cdb1.CarId and cdb1.ParamId = 385 ";
				sql += " left join dbo.CarDataBase cdb2 on car.car_id = cdb2.CarId and cdb2.ParamId = 384 ";
				sql += " left join dbo.CarDataBase cdb3 on car.car_id = cdb3.CarId and cdb3.ParamId = 383 ";
				sql += " where car.isState=0 and cs.isState=0 and cs.csSaleState<>'停销' and car.car_SaleState<>96 ";
				sql += " order by cs.cs_id ";
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
				HttpContext.Current.Cache.Insert("interfaceforbitauto_AllSerialMarketDay", ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
			}

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				string minReferPrice = "0";
				string maxReferPrice = "0";
				string marketDay = "";
				string currentCsID = "";
				string currentCsShowName = "";

				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					if (i == 0)
					{
						// 第一列
						currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString().Trim();
						currentCsShowName = ds.Tables[0].Rows[i]["csShowName"].ToString().Trim();
					}
					else
					{
						if (currentCsID != ds.Tables[0].Rows[i]["cs_id"].ToString().Trim())
						{
							// 不同子品牌 写子品牌节点
							if (marketDay != "" && Convert.ToDateTime(marketDay) > DateTime.Now)
							{
								currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString().Trim();
								currentCsShowName = ds.Tables[0].Rows[i]["csShowName"].ToString().Trim().Replace("&", "&amp;");
								minReferPrice = "0";
								maxReferPrice = "0";
								marketDay = "";

								continue;
							}
							DataRow row = dtSerial.NewRow();
							row["ID"] = int.Parse(currentCsID);
							row["CsShowName"] = currentCsShowName;
							if (marketDay != "")
							{
								row["MarketDay"] = marketDay;
							}
							row["MinReferPrice"] = decimal.Parse(minReferPrice);
							row["MaxReferPrice"] = decimal.Parse(maxReferPrice);
							if (htCsPV.ContainsKey(currentCsID))
							{
								row["PV"] = int.Parse(htCsPV[currentCsID].ToString());
							}
							else
							{
								row["PV"] = 0;
							}
							dtSerial.Rows.Add(row);
							//sb.Append("<Serial ");
							//sb.Append(" ID=\"" + currentCsID + "\" ");
							//sb.Append(" CsShowName=\"" + currentCsShowName + "\" ");
							//sb.Append(" MarketDay=\"" + marketDay + "\" ");
							//sb.Append(" MinReferPrice=\"" + minReferPrice + "\" ");
							//sb.Append(" MaxReferPrice=\"" + maxReferPrice + "\" />");

							currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString().Trim();
							currentCsShowName = ds.Tables[0].Rows[i]["csShowName"].ToString().Trim();
							minReferPrice = "0";
							maxReferPrice = "0";
							marketDay = "";
						}
						else
						{
							// 同子品牌
						}
					}
					GetMinAndMaxReferPrice(ds.Tables[0].Rows[i]["car_ReferPrice"].ToString().Trim(), ref minReferPrice, ref maxReferPrice);
					GetMarketDay(ds.Tables[0].Rows[i]["marketyear"].ToString().Trim(), ds.Tables[0].Rows[i]["marketmonth"].ToString().Trim(), ds.Tables[0].Rows[i]["marketday"].ToString().Trim(), ref marketDay);
				}
				// 最后的子品牌
				//sb.Append("<Serial ");
				//sb.Append(" ID=\"" + currentCsID + "\" ");
				//sb.Append(" CsShowName=\"" + currentCsShowName + "\" ");
				//sb.Append(" MarketDay=\"" + marketDay + "\" ");
				//sb.Append(" MinReferPrice=\"" + minReferPrice + "\" ");
				//sb.Append(" MaxReferPrice=\"" + maxReferPrice + "\" />");
				if (marketDay == "" || Convert.ToDateTime(marketDay) <= DateTime.Now)
				{
					DataRow rowLast = dtSerial.NewRow();
					rowLast["ID"] = int.Parse(currentCsID);
					rowLast["CsShowName"] = currentCsShowName;
					if (marketDay != "")
					{
						rowLast["MarketDay"] = marketDay;
					}
					rowLast["MinReferPrice"] = decimal.Parse(minReferPrice);
					rowLast["MaxReferPrice"] = decimal.Parse(maxReferPrice);
					if (htCsPV.ContainsKey(currentCsID))
					{
						rowLast["PV"] = int.Parse(htCsPV[currentCsID].ToString());
					}
					else
					{
						rowLast["PV"] = 0;
					}
					dtSerial.Rows.Add(rowLast);
				}
			}
		}

		/// <summary>
		/// 设置厂商指导价最小最大值
		/// </summary>
		/// <param name="referPrice">厂商指导价</param>
		/// <param name="minReferPrice"></param>
		/// <param name="maxReferPrice"></param>
		private void GetMinAndMaxReferPrice(string referPrice, ref string minReferPrice, ref string maxReferPrice)
		{
			// 最小值
			if (referPrice.Trim() != "")
			{
				if (minReferPrice == "0")
				{
					minReferPrice = referPrice;
				}
				else
				{
					if (decimal.Parse(referPrice) < decimal.Parse(minReferPrice))
					{
						minReferPrice = referPrice;
					}
				}

				// 最大值
				if (maxReferPrice == "0")
				{
					maxReferPrice = referPrice;
				}
				else
				{
					if (decimal.Parse(referPrice) > decimal.Parse(maxReferPrice))
					{
						maxReferPrice = referPrice;
					}
				}
			}
		}

		/// <summary>
		/// 设置子品牌的上市时间
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="day"></param>
		/// <param name="marketDay"></param>
		private void GetMarketDay(string year, string month, string day, ref string marketDay)
		{
			if (year.Trim() != "" && month.Trim() != "" && year.Trim() != "0" && month.Trim() != "0")
			{
				string defaultDay = "1";
				if (day.Trim() != "" && day.Trim() != "0")
				{ defaultDay = day.Trim(); }
				// 
				DateTime md = Convert.ToDateTime(year.Trim() + "-" + month.Trim() + "-" + defaultDay.Trim());
				if (marketDay != "")
				{
					if (md < DateTime.Now && md > Convert.ToDateTime(marketDay))
					{ marketDay = md.ToShortDateString(); }
				}
				else
				{ marketDay = md.ToShortDateString(); }
			}
		}

		/// <summary>
		/// 初始化表
		/// </summary>
		/// <returns></returns>
		private DataTable InitTable()
		{
			DataTable dt = new DataTable();
			DataColumn column = new DataColumn();
			column.DataType = System.Type.GetType("System.Int32");
			column.AllowDBNull = false;
			column.Caption = "ID";
			column.ColumnName = "ID";
			column.DefaultValue = 0;
			dt.Columns.Add(column);

			DataColumn column2 = new DataColumn();
			column2.DataType = System.Type.GetType("System.String");
			column2.AllowDBNull = false;
			column2.Caption = "CsShowName";
			column2.ColumnName = "CsShowName";
			column2.DefaultValue = "";
			dt.Columns.Add(column2);

			DataColumn column3 = new DataColumn();
			column3.DataType = System.Type.GetType("System.DateTime");
			column3.AllowDBNull = true;
			column3.Caption = "MarketDay";
			column3.ColumnName = "MarketDay";
			// column3.DefaultValue = "";
			dt.Columns.Add(column3);

			DataColumn column4 = new DataColumn();
			column4.DataType = System.Type.GetType("System.Decimal");
			column4.AllowDBNull = false;
			column4.Caption = "MinReferPrice";
			column4.ColumnName = "MinReferPrice";
			column4.DefaultValue = 0;
			dt.Columns.Add(column4);

			DataColumn column5 = new DataColumn();
			column5.DataType = System.Type.GetType("System.Decimal");
			column5.AllowDBNull = false;
			column5.Caption = "MaxReferPrice";
			column5.ColumnName = "MaxReferPrice";
			column5.DefaultValue = 0;
			dt.Columns.Add(column5);

			// 子品牌PV
			DataColumn column6 = new DataColumn();
			column6.DataType = System.Type.GetType("System.Int32");
			column6.AllowDBNull = false;
			column6.Caption = "PV";
			column6.ColumnName = "PV";
			column6.DefaultValue = 0;
			dt.Columns.Add(column6);
			return dt;
		}
	}
}