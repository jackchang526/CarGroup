using System;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class ForCarCompareListByCsID : InterfacePageBase
	{
		private int csID = 0;
		private int topCount = 5;
		private string temp = string.Empty;
		private string tempCurrent = string.Empty;
		private string xmlPath = "http://carser.bitauto.com/forpicmastertoserial/CarCompareStat/{0}.xml";
		private StringBuilder sb = new StringBuilder();
		private List<CarData> lcd = new List<CarData>();
		private List<int> lcount = new List<int>();
		// private int currentCsID = 0;
		private bool isContainSameSerial = false;

		protected void Page_Load(object sender, EventArgs e)
		{
			temp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
			if (!this.IsPostBack)
			{
				this.CheckPageParam();
				if (csID > 0 && topCount > 0)
				{
					temp += "<Serial ID=\"" + csID.ToString() + "\" {0} >";
					GetCarCompareDataByCsID();
					temp += "{1}";
					temp += "</Serial>";
				}
			}
			Response.Write(string.Format(temp, tempCurrent, sb.ToString()));
		}

		private void CheckPageParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string csIDStr = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(csIDStr, out csID))
				{ }
				else
				{
					csID = 0;
				}
			}
			if (this.Request.QueryString["top"] != null && this.Request.QueryString["top"].ToString() != "")
			{
				string top = this.Request.QueryString["top"].ToString();
				if (int.TryParse(top, out topCount))
				{
					if (topCount < 0 || topCount > 1000)
					{
						topCount = 5;
					}
				}
				else
				{
					topCount = 0;
				}
			}
			if (this.Request.QueryString["isContainSameSerial"] != null && this.Request.QueryString["isContainSameSerial"].ToString() != "")
			{
				if (this.Request.QueryString["isContainSameSerial"].ToString().Trim() == "1")
				{
					isContainSameSerial = true;
				}
				else
				{
					isContainSameSerial = false;
				}
			}
		}

		private void GetCarCompareDataByCsID()
		{
			XmlDocument doc = new XmlDocument();
			try
			{
				string sql = " select car.car_id,car.car_name,cs.cs_id,cs.cs_name ";
				sql += " from car_basic car ";
				sql += " left join car_serial cs on car.cs_id = cs.cs_id ";
				sql += " where car.isState=1 and cs.isState=1 ";
				DataSet ds = new DataSet();
				if (HttpContext.Current.Cache.Get("ForCarCompareList_AllData") != null)
				{
					ds = (DataSet)HttpContext.Current.Cache.Get("ForCarCompareList_AllData");
				}
				else
				{
					ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
					HttpContext.Current.Cache.Insert("ForCarCompareList_AllData", ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
				}

				// DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(base.szConnString, CommandType.Text, sql);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					DataRow[] dr = ds.Tables[0].Select(" cs_id = " + csID.ToString());

					if (dr.Length > 0)
					{
						tempCurrent = "CsName=\"" + dr[0]["cs_name"] + "\"";
						foreach (DataRow drCar in dr)
						{
							int carID = int.Parse(drCar["car_id"].ToString());
							try
							{
								if (HttpContext.Current.Cache.Get("ForCarCompareList_XML" + carID.ToString()) != null)
								{
									doc = (XmlDocument)HttpContext.Current.Cache.Get("ForCarCompareList_XML" + carID.ToString());
								}
								else
								{
									doc.Load(string.Format(xmlPath, carID.ToString()));
									HttpContext.Current.Cache.Insert("ForCarCompareList_XML" + carID.ToString(), doc, null, DateTime.Now.AddHours(6), TimeSpan.Zero);
								}

								if (doc != null && doc.HasChildNodes)
								{
									XmlNodeList xnl = doc.SelectNodes("/Car/Item");
									if (xnl != null && xnl.Count > 0)
									{
										for (int i = 0; i < xnl.Count; i++)
										{
											int iCarId = int.Parse(xnl[i].Attributes["ID"].Value.ToString());
											int iCompareCount = int.Parse(xnl[i].Attributes["Count"].Value.ToString());

											DataRow[] drCompare = ds.Tables[0].Select(" car_id = " + iCarId.ToString());
											if (drCompare.Length > 0)
											{
												CarData cd = new CarData();
												this.GetCarStructDataByID(iCarId, iCompareCount, drCompare, drCar, ref cd);
												lcd.Add(cd);
												lcount.Add(iCompareCount);
											}
										}
									}
								}

							}
							catch
							{ }
						}
					}
				}
				else
				{ return; }

				CarData[] arrCarData = lcd.ToArray();
				int[] arrCount = lcount.ToArray();
				if (arrCarData.Length == arrCount.Length)
				{
					int loop = 1;
					Array.Sort(arrCount, arrCarData);
					for (int i = arrCarData.Length - 1; i >= 0; i--)
					{
						if (loop > topCount)
						{
							break;
						}

						if (isContainSameSerial)
						{
							// 可以取同一子品牌车型
							sb.Append("<Item CarID=\"" + arrCarData[i].CurrentCarID.ToString() + "\" ");
							sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(arrCarData[i].CurrentCarName.ToString()) + "\" ");

							sb.Append(" CompareCarID=\"" + arrCarData[i].CarID.ToString() + "\" ");
							sb.Append(" CompareCarName=\"" + System.Security.SecurityElement.Escape(arrCarData[i].CarName.ToString()) + "\" ");

							sb.Append(" CompareCsID=\"" + arrCarData[i].CsID.ToString() + "\" ");
							sb.Append(" CompareCsName=\"" + System.Security.SecurityElement.Escape(arrCarData[i].CsName) + "\" />");
							loop++;
						}
						else
						{
							// 不取同一子品牌车型
							if (arrCarData[i].CsID == csID)
							{
								continue;
							}
							sb.Append("<Item CarID=\"" + arrCarData[i].CurrentCarID.ToString() + "\" ");
							sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(arrCarData[i].CurrentCarName.ToString()) + "\" ");

							sb.Append(" CompareCarID=\"" + arrCarData[i].CarID.ToString() + "\" ");
							sb.Append(" CompareCarName=\"" + System.Security.SecurityElement.Escape(arrCarData[i].CarName.ToString()) + "\" ");

							sb.Append(" CompareCsID=\"" + arrCarData[i].CsID.ToString() + "\" ");
							sb.Append(" CompareCsName=\"" + System.Security.SecurityElement.Escape(arrCarData[i].CsName) + "\" />");
							loop++;
						}
					}
				}

			}
			catch
			{ }
		}

		private void GetCarStructDataByID(int carID, int compareCount, DataRow[] drCompare, DataRow drCurrent, ref CarData carData)
		{
			// CarData carData = new CarData();
			carData.CurrentCarID = int.Parse(drCurrent["car_id"].ToString());
			carData.CurrentCarName = drCurrent["car_Name"].ToString();
			carData.CarID = carID;
			carData.CompareCount = compareCount;
			if (drCompare.Length > 0)
			{
				carData.CarName = drCompare[0]["car_name"].ToString();
				carData.CsID = int.Parse(drCompare[0]["cs_id"].ToString());
				carData.CsName = drCompare[0]["cs_name"].ToString();
			}
		}

		private struct CarData
		{
			public int CurrentCarID;
			public string CurrentCarName;
			public int CarID;
			public string CarName;
			public int CsID;
			public string CsName;
			public int CompareCount;
		}
	}
}