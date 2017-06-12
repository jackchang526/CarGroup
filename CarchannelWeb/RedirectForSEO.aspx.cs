using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb
{
	public partial class RedirectForSEO : System.Web.UI.Page
	{
		private Hashtable hsBrandRedirect = new Hashtable();
		private Hashtable hsSerialRedirect = new Hashtable();
		private Hashtable hsCarRedirect = new Hashtable();
		// private readonly string DBConnectionString = ConfigurationManager.AppSettings["AutoStorageNewConnectionString"].ToString();
		private readonly string DBConnectionString = WebConfig.AutoStorageConnectionString;
		private string targetURL = "http://car.bitauto.com";
		private int redirectType = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Response.Write(Response.Charset);// = "utf-8";

			switch (Request.QueryString["redirectType"].ToString())
			{
				// 品牌页
				case "1": redirectType = 1; break;
				// 子品牌综述页
				case "2": redirectType = 2; break;
				// 子品牌配置参数页
				case "3": redirectType = 3; break;
				// 子品牌图片页
				case "4": redirectType = 4; break;
				// 子品牌文章页
				case "5": redirectType = 5; break;
				// 车型综述页
				case "6": redirectType = 6; break;
				// 车型牌配置参数页
				case "7": redirectType = 7; break;
				default: break;
			}

			// 取数据
			ProvideDataForRedirect();

			if (redirectType == 0)
			{
				targetURL = "http://car.bitauto.com/";
			}
			// 品牌页
			else if (redirectType == 1)
			{
				string cbID = RequestQueryString("cb_id", "");
				if (hsBrandRedirect != null && hsBrandRedirect.Count > 0 && hsBrandRedirect.ContainsKey(cbID))
				{
					targetURL = "http://car.bitauto.com/" + Convert.ToString(hsBrandRedirect[cbID]) + "/";
				}
			}
			// 子品牌综述页
			else if (redirectType == 2)
			{
				string csID = RequestQueryString("csid", "");
				if (hsSerialRedirect != null && hsSerialRedirect.Count > 0 && hsSerialRedirect.ContainsKey(csID))
				{
					targetURL = "http://car.bitauto.com/" + Convert.ToString(hsSerialRedirect[csID]) + "/";
				}
			}
			// 子品牌配置参数页
			else if (redirectType == 3)
			{
				string csID = RequestQueryString("csid", "");
				if (hsSerialRedirect != null && hsSerialRedirect.Count > 0 && hsSerialRedirect.ContainsKey(csID))
				{
					targetURL = "http://car.bitauto.com/" + Convert.ToString(hsSerialRedirect[csID] + "/peizhi/");
				}
			}
			// 子品牌图片页
			else if (redirectType == 4)
			{
				string csID = RequestQueryString("sid", "");
				if (hsSerialRedirect != null && hsSerialRedirect.Count > 0 && hsSerialRedirect.ContainsKey(csID))
				{
					targetURL = "http://car.bitauto.com/" + Convert.ToString(hsSerialRedirect[csID] + "/tupian/");
				}
			}
			// 子品牌文章页
			else if (redirectType == 5)
			{
				string csID = RequestQueryString("sid", "");
				string pid = RequestQueryString("pid", "0");
				string cid = RequestQueryString("cid", "");
				if (hsSerialRedirect != null && hsSerialRedirect.Count > 0 && hsSerialRedirect.ContainsKey(csID))
				{
					if (cid == "")
					{
						if (pid != "0" && pid != "")
						{
							targetURL = "http://car.bitauto.com/" + Convert.ToString(hsSerialRedirect[csID] + "/wenzhang/" + pid + "/");
						}
						else
						{
							targetURL = "http://car.bitauto.com/" + Convert.ToString(hsSerialRedirect[csID] + "/wenzhang/");
						}
					}
					else
					{
						targetURL = "http://car.bitauto.com/" + Convert.ToString(hsSerialRedirect[csID] + "/wenzhang/" + cid + "/" + pid + "/");
					}
				}
			}
			// 车型综述页
			else if (redirectType == 6)
			{
				string carID = RequestQueryString("carid", "");
				if (hsCarRedirect != null && hsCarRedirect.Count > 0 && hsCarRedirect.ContainsKey(carID))
				{
					targetURL = "http://car.bitauto.com/" + Convert.ToString(hsCarRedirect[carID] + "/m" + carID + "/");
				}
			}
			// 车型牌配置参数页
			else if (redirectType == 7)
			{
				string carID = RequestQueryString("carid", "");
				if (hsCarRedirect != null && hsCarRedirect.Count > 0 && hsCarRedirect.ContainsKey(carID))
				{
					targetURL = "http://car.bitauto.com/" + Convert.ToString(hsCarRedirect[carID] + "/m" + carID + "/peizhi/");
				}
			}


			// 跳转
			Response.Status = "301 Moved Permanently";
			Response.AddHeader("Location", targetURL.ToLower());
		}

		#region

		// 取拼写数据
		private void ProvideDataForRedirect()
		{
			// 品牌拼写
			if (HttpContext.Current.Cache.Get("RedirectForBrandAllSpell") != null)
			{
				hsBrandRedirect = (System.Collections.Hashtable)HttpContext.Current.Cache.Get("RedirectForBrandAllSpell");
			}
			else
			{
				string sqlBrand = " select cb_id,allSpell from car_brand where isState=0 order by cb_id ";
				DataSet dsBrand = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(DBConnectionString, CommandType.Text, sqlBrand);
				if (dsBrand != null && dsBrand.Tables.Count > 0 && dsBrand.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < dsBrand.Tables[0].Rows.Count; i++)
					{
						if (!hsBrandRedirect.ContainsKey(dsBrand.Tables[0].Rows[i]["cb_id"].ToString()))
						{
							hsBrandRedirect.Add(dsBrand.Tables[0].Rows[i]["cb_id"].ToString(), dsBrand.Tables[0].Rows[i]["allSpell"].ToString());
						}
					}
					HttpContext.Current.Cache.Insert("RedirectForBrandAllSpell", hsBrandRedirect, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
				}
			}

			// 子品牌拼写
			if (HttpContext.Current.Cache.Get("RedirectForSerialAllSpell") != null)
			{
				hsSerialRedirect = (System.Collections.Hashtable)HttpContext.Current.Cache.Get("RedirectForSerialAllSpell");
			}
			else
			{
				string sqlSerial = " select cs_id,allSpell from car_serial where isState=0 order by cs_id ";
				DataSet dsSerial = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(DBConnectionString, CommandType.Text, sqlSerial);
				if (dsSerial != null && dsSerial.Tables.Count > 0 && dsSerial.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < dsSerial.Tables[0].Rows.Count; i++)
					{
						if (!hsSerialRedirect.ContainsKey(dsSerial.Tables[0].Rows[i]["cs_id"].ToString()))
						{
							hsSerialRedirect.Add(dsSerial.Tables[0].Rows[i]["cs_id"].ToString(), dsSerial.Tables[0].Rows[i]["allSpell"].ToString());
						}
					}
					HttpContext.Current.Cache.Insert("RedirectForSerialAllSpell", hsSerialRedirect, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
				}
			}

			// 车型
			if (HttpContext.Current.Cache.Get("RedirectForCarAllSpell") != null)
			{
				hsCarRedirect = (System.Collections.Hashtable)HttpContext.Current.Cache.Get("RedirectForCarAllSpell");
			}
			else
			{
				string sqlCar = " select car_id,cs.allSpell from dbo.Car_relation cr ";
				sqlCar += " left join car_serial cs on cr.cs_id = cs.cs_id ";
				sqlCar += " where cr.isState=0 and cs.isState=0 order by car_id ";
				DataSet dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(DBConnectionString, CommandType.Text, sqlCar);
				for (int i = 0; i < dsCar.Tables[0].Rows.Count; i++)
				{
					if (!hsCarRedirect.ContainsKey(dsCar.Tables[0].Rows[i]["car_id"].ToString()) && dsCar.Tables[0].Rows[i]["allSpell"].ToString() != "")
					{
						hsCarRedirect.Add(dsCar.Tables[0].Rows[i]["car_id"].ToString(), dsCar.Tables[0].Rows[i]["allSpell"].ToString());
					}
				}
				HttpContext.Current.Cache.Insert("RedirectForCarAllSpell", hsCarRedirect, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
			}
		}

		// 取参数
		private string RequestQueryString(string qs, string defaultStr)
		{
			if (this.Request.QueryString[qs] != null && this.Request.QueryString[qs].ToString() != "")
			{
				return this.Request.QueryString[qs].ToString();
			}
			else
			{
				return "";
			}
		}

		#endregion
	}
}