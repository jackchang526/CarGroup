using System;
using System.Data;
using System.Globalization;
using System.Collections;
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
	/// 易车网，子品牌昨天排名接口，朱永旭
	/// </summary>
	public partial class SerialYesterdaySort : InterfacePageBase
	{
		private string sortString = string.Empty;
		private string xmlPath = "http://carser.bitauto.com/forpicmastertoserial/xml/";
		private Hashtable hsSerial = new Hashtable();
		private string csID = string.Empty;
		private string oldcbID = string.Empty;
		private Hashtable hsCS = new Hashtable();

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckSerialID();
			Response.Write(sortString);
		}

		private void CheckSerialID()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				try
				{
					int iCsID = int.Parse(this.Request.QueryString["csID"].ToString());
					csID = iCsID.ToString();

					// 检查是否老ID
					if (this.Request.QueryString["isOldID"] != null && this.Request.QueryString["isOldID"].ToString() != "" && this.Request.QueryString["isOldID"].ToString() == "1")
					{
						// 检查新ID
						if (HttpContext.Current.Cache.Get("InterfaceForBitautoSerialYesterdaySortOldCSID") != null)
						{
							hsCS = (System.Collections.Hashtable)HttpContext.Current.Cache.Get("InterfaceForBitautoSerialYesterdaySortOldCSID");
						}
						else
						{
							string sql = " select cs_id,oldcb_id from car_serial where isState>=1 ";
							DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
							if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
							{
								for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
								{
									if (!hsCS.ContainsKey(ds.Tables[0].Rows[i]["oldcb_id"].ToString()))
									{
										hsCS.Add(ds.Tables[0].Rows[i]["oldcb_id"].ToString(), ds.Tables[0].Rows[i]["cs_id"].ToString());
									}
								}
								HttpContext.Current.Cache.Insert("InterfaceForBitautoSerialYesterdaySortOldCSID", hsCS, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
							}
						}
						if (hsCS.Count > 0 && hsCS.ContainsKey(csID))
						{
							// 取新ID
							csID = Convert.ToString(hsCS[csID]);
						}
						else
						{
							csID = "0";
							sortString = "0";
							return;
						}
					}

					this.GetAllSerialYesterdaySort();
				}
				catch
				{
					sortString = "0";
				}
			}
		}

		private void GetAllSerialYesterdaySort()
		{
			// 检查缓存
			if (HttpContext.Current.Cache.Get("InterfaceForBitautoSerialYesterdaySort") != null)
			{
				hsSerial = (System.Collections.Hashtable)HttpContext.Current.Cache.Get("InterfaceForBitautoSerialYesterdaySort");
			}
			else
			{
				CultureInfo myCI = new CultureInfo("zh-CN");
				System.Globalization.Calendar myCal = myCI.Calendar;
				CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
				DayOfWeek myFirstDOW = DayOfWeek.Monday;
				int thisWeek = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);

				try
				{
					DataSet ds = new DataSet();
					ds.ReadXml(xmlPath + "SerialSort-D" + DateTime.Now.ToShortDateString() + "-W" + Convert.ToString(thisWeek - 1) + ".xml");
					if (ds != null && ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)
					{
						for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
						{
							if (!hsSerial.ContainsKey(ds.Tables[1].Rows[i]["ID"].ToString()))
							{
								hsSerial.Add(ds.Tables[1].Rows[i]["ID"].ToString(), ds.Tables[1].Rows[i]["YesterdaySort"].ToString());
							}
						}
					}
				}
				catch
				{ }
				HttpContext.Current.Cache.Insert("InterfaceForBitautoSerialYesterdaySort", hsSerial, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
			}

			// 
			if (hsSerial != null && hsSerial.Count > 0 && hsSerial.ContainsKey(csID))
			{
				sortString = Convert.ToString(hsSerial[csID]);
			}
			else
			{
				sortString = "0";
			}
		}
	}
}