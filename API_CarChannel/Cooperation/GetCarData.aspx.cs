using System;
using System.Data;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;
using System.Web.Caching;
using System.Collections.Specialized;

namespace BitAuto.CarChannelAPI.Web.Cooperation
{
	/// <summary>
	/// 合作站 接口数据(不同合作站 通过此接口取车型各个级别数据)
	/// 需要IP限制
	/// add by chengl Mar.13.2012
	/// </summary>
	public partial class GetCarData : PageBase
	{
		private string action, type;
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			action = Request.QueryString["name"];
			type = Request.QueryString["type"];
			Verify();
			switch (type.ToLower())
			{
				case "mb": GetMasterBrand(); break;
				case "cb": GetBrand(); break;
				case "cs": GetSerial(); break;
				case "car": GetCar(); break;
				case "getcaridlist": GetCarIDList(); break;
				case "getcsidlist": GetCsIDList(); break;
				case "getcbidlist": GetCbIDList(); break;
				case "getbsidlist": GetBsIDList(); break;
				default:
					Echo("<info>类型无匹配。</info>");
					break;
			}
		}
		//验证访问限制
		private void Verify()
		{
			if (string.IsNullOrEmpty(action))
			{
				Echo("<info>请输入标识。</info>");
			}
			if (string.IsNullOrEmpty(type))
			{
				Echo("<info>请输入类型。</info>");
			}
			List<NameValueCollection> list = GetXmlData();
			bool flag = false;
			foreach (NameValueCollection nvc in list)
			{
				if (string.Equals(action, nvc["cname"], StringComparison.OrdinalIgnoreCase))
				{
					string mip = nvc["iprange"];
					string[] arr = mip.Split(';');
					string clientIp = BitAuto.Utils.WebUtil.GetClientIP();

					for (int i = 0; i < arr.Length; i++)
					{
						if (Regex.IsMatch(clientIp, arr[i]))
						{ flag = true; break; }
					}
					if (!flag)
					{
						CommonFunction.WriteLog("合作接口 IP:" + clientIp + " Url:" + this.Request.Url.ToString());
						Echo("<info>对不起，您的IP不能访问。</info>");
					}
					break;
				}
			}
			if (!flag) Echo("<info>输入标识不正确。</info>");
		}
		//获取主品牌
		private void GetMasterBrand()
		{
			int id = ConvertHelper.GetInteger(Request.QueryString["id"]);
			BitAuto.CarChannel.DAL.Data.TMasterBrandDAL mb = new CarChannel.DAL.Data.TMasterBrandDAL();
			if (id > 0)
			{
				System.Data.DataSet ds = mb.GetMasterBrandInfoById(id);
				sb.Append("<!-- id:主品牌ID,name:主品牌名,logo:主品牌Logo,introduction:简介,country:主品牌国别 -->");
				if (ds.Tables[0].Rows.Count > 0)
				{
					foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
					{
						sb.Append("<item>");
						sb.Append("<id><![CDATA[" + dr["bs_id"] + "]]></id>");
						sb.Append("<name><![CDATA[" + dr["bs_name"] + "]]></name>");
						sb.Append("<logo><![CDATA[http://img1.bitauto.com/bt/car/default/images/carimage/m_" + dr["bs_id"] + "_b.jpg]]></logo>");
						sb.Append("<introduction><![CDATA[" + dr["bs_introduction"] + "]]></introduction>");
						sb.Append("<country><![CDATA[" + dr["classvalue"] + "]]></country>");
						sb.Append("</item>");
					}
				}
			}
			Echo(sb.ToString());
		}
		//获取品牌
		private void GetBrand()
		{
			int id = ConvertHelper.GetInteger(Request.QueryString["id"]);
			System.Data.DataSet ds;
			BitAuto.CarChannel.DAL.Data.TBrandDAL cb = new CarChannel.DAL.Data.TBrandDAL();
			if (id > 0)
			{
				ds = cb.GetBrandInfoById(id);
				sb.Append("<!-- id:品牌ID,name:品牌名,masterid:主品牌ID,country:品牌国别 -->");
				if (ds.Tables[0].Rows.Count > 0)
				{
					foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
					{
						sb.Append("<item>");
						sb.Append("<id><![CDATA[" + dr["cb_id"] + "]]></id>");
						sb.Append("<name><![CDATA[" + dr["cb_name"] + "]]></name>");
						sb.Append("<masterid><![CDATA[" + dr["bs_id"] + "]]></masterid>");
						sb.Append("<country><![CDATA[" + dr["classvalue"] + "]]></country>");
						sb.Append("</item>");
					}
				}
			}
			Echo(sb.ToString());
		}
		//获取子品牌
		private void GetSerial()
		{
			int id = ConvertHelper.GetInteger(Request.QueryString["id"]);
			System.Data.DataSet ds;
			BitAuto.CarChannel.DAL.Data.TSerialDAL cb = new CarChannel.DAL.Data.TSerialDAL();
			if (id > 0)
			{
				// ds = cb.GetSerailInfoById(id);
				// modified by chengl Dec.11.2015
				ds = cb.GetSerialDataById(id);
				sb.Append("<!-- id:子品牌ID,name:子品牌名,showname:子品牌显示名,brandid:品牌ID,level:级别,cspurpose:用途,bodyform:车身形式,salestate:销售状态,cspic:封面,referpricerange:指导价区间 -->");
				if (ds.Tables[0].Rows.Count > 0)
				{
					foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
					{
						sb.Append("<item>");
						sb.Append("<id><![CDATA[" + dr["cs_id"] + "]]></id>");
						sb.Append("<name><![CDATA[" + dr["cs_name"] + "]]></name>");
						sb.Append("<showname><![CDATA[" + dr["cs_showname"] + "]]></showname>");
						sb.Append("<brandid><![CDATA[" + dr["cb_id"] + "]]></brandid>");
						sb.Append("<level><![CDATA[" + dr["cs_carlevel"] + "]]></level>");
						string purpose = ConvertHelper.GetString(dr["cspurpose"]);
						purpose = string.IsNullOrEmpty(purpose) ? "0" : purpose.Trim(',');
						//string[] arr = purpose.Split(',');
						//int[] arrInt = Array.ConvertAll(arr, new Converter<string, int>((i) => { return ConvertHelper.GetInteger(i); }));
						//获取用途
						System.Data.DataSet dsClass = new BitAuto.CarChannel.DAL.Car_BasicDal().GetClassValueById(purpose);
						List<string> list = new List<string>();
						foreach (System.Data.DataRow drc in dsClass.Tables[0].Rows)
						{
							list.Add(ConvertHelper.GetString(drc["classvalue"]));
						}
						sb.Append("<purpose><![CDATA[" + string.Join(",", list.ToArray()) + "]]></purpose>");
						sb.Append("<bodyform><![CDATA[" + dr["csbodyform"] + "]]></bodyform>");
						sb.Append("<salestate><![CDATA[" + dr["cssalestate"] + "]]></salestate>");
						string picUrl = "";
						int picCount = 0;
						base.GetSerialPicAndCountByCsID(ConvertHelper.GetInteger(dr["cs_id"]), out picUrl, out picCount, true);
						sb.Append("<cspic><![CDATA[" + picUrl + "]]></cspic>");
						// 加指导价区间 Dec.11.2015
						sb.Append("<referpricerange><![CDATA[" + (dr["ReferPriceRange"].ToString()!="" ? dr["ReferPriceRange"].ToString() + "万" : "") + "]]></referpricerange>");
						sb.Append("</item>");
					}
				}
			}
			Echo(sb.ToString());
		}
		//获取车型
		private void GetCar()
		{
			int id = ConvertHelper.GetInteger(Request.QueryString["id"]);
			System.Data.DataSet ds;
			BitAuto.CarChannel.DAL.Data.TCarDAL car = new CarChannel.DAL.Data.TCarDAL();
			if (id > 0)
			{
				ds = car.GetCarInfoById(id);
				sb.Append("<!-- id:车款ID,name:车款名,serialid:子品牌ID,producestate:生产状态,salestate:销售状态,year:年款,referprice:指导价,params:参配,params/item/paramid:参数ID,params/item/paramname:参数名,params/item/pvalue:参数值 -->");
				if (ds.Tables[0].Rows.Count > 0)
				{
					foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
					{
						sb.Append("<item>");
						sb.Append("<id><![CDATA[" + dr["car_id"] + "]]></id>");
						sb.Append("<name><![CDATA[" + dr["car_name"] + "]]></name>");
						sb.Append("<serialid><![CDATA[" + dr["cs_id"] + "]]></serialid>");
						sb.Append("<producestate><![CDATA[" + dr["producestate"] + "]]></producestate>");
						sb.Append("<salestate><![CDATA[" + dr["salestate"] + "]]></salestate>");
						sb.Append("<year><![CDATA[" + dr["car_yeartype"] + "]]></year>");
						sb.Append("<referprice><![CDATA[" + dr["car_referprice"] + "]]></referprice>");
						//根据车型取参数
						System.Data.DataSet dsParamList = new BitAuto.CarChannel.DAL.Car_BasicDal().GetParamListByCarId(ConvertHelper.GetInteger(dr["car_id"]));
						if (dsParamList.Tables[0].Rows.Count > 0)
						{
							List<NameValueCollection> list = GetXmlData();
							foreach (NameValueCollection nvc in list)
							{
								if (string.Equals(action, nvc["cname"], StringComparison.OrdinalIgnoreCase))
								{
									string limitParamList = nvc["paramlist"];
									string[] arr = limitParamList.Split(',');
									sb.Append("<params>");
									foreach (System.Data.DataRow pdr in dsParamList.Tables[0].Rows)
									{
										if (Array.IndexOf<string>(arr, ConvertHelper.GetString(pdr["paramid"])) != -1)
										{
											sb.Append("<item>");
											sb.Append("<paramid><![CDATA[" + pdr["paramid"] + "]]></paramid>");
											sb.Append("<paramname><![CDATA[" + pdr["paramname"] + "]]></paramname>");
											sb.Append("<pvalue><![CDATA[" + pdr["pvalue"] + "]]></pvalue>");
											sb.Append("</item>");
										}
									}
									sb.Append("</params>");
								}
							}
						}
						sb.Append("</item>");
					}
				}
			}
			Echo(sb.ToString());
		}

		// 取车型ID
		private void GetCarIDList()
		{
			string isGetAllSale = Request.QueryString["allSale"];
			string sql = @"SELECT car.Car_Id,car.Cs_Id     
										FROM Car_Basic car
										left join car_serial cs 
										on car.Cs_Id=cs.cs_Id
										where car.IsState=1 and cs.IsState=1 {0}";
			string whereStr = "";
			if (!string.IsNullOrEmpty(isGetAllSale) && isGetAllSale == "1")
			{ }
			else
			{ whereStr = "and car.Car_SaleState='在销'"; }
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
				, CommandType.Text, string.Format(sql, whereStr));
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				sb.AppendLine("<!-- ID:车款ID -->");
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					sb.AppendLine(string.Format("<Item ID=\"{0}\"/>", dr["Car_Id"].ToString()));
				}
			}
			CommonFunction.EchoXml(this.Response, sb.ToString(), "root");
		}

		// 取子品牌ID
		private void GetCsIDList()
		{
			string sql = @"select cs.cs_id
										from car_serial cs
										left join Car_Serial_30UV cs30 on cs.cs_id=cs30.cs_id
										where cs.IsState=1 {0} order by cs30.uvcount desc";
			string isGetAllSale = Request.QueryString["allSale"];
			string whereStr = "";
			if (!string.IsNullOrEmpty(isGetAllSale) && isGetAllSale == "1")
			{ }
			else
			{ whereStr = "and cs.CsSaleState='在销'"; }
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
				, CommandType.Text, string.Format(sql, whereStr));
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				sb.AppendLine("<!-- ID:车系ID -->");
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					sb.AppendLine(string.Format("<Item ID=\"{0}\"/>", dr["cs_id"].ToString()));
				}
			}
			CommonFunction.EchoXml(this.Response, sb.ToString(), "root");
		}

		// 取品牌ID
		private void GetCbIDList()
		{
			string sql = @"select cb_id
										from car_brand
										where IsState=1";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
				, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				sb.AppendLine("<!-- ID:品牌ID -->");
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					sb.AppendLine(string.Format("<Item ID=\"{0}\"/>", dr["cb_id"].ToString()));
				}
			}
			CommonFunction.EchoXml(this.Response, sb.ToString(), "root");
		}

		// 取主品牌ID
		private void GetBsIDList()
		{
			string sql = @"select bs_id
										from car_masterbrand
										where IsState=1";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
				, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				sb.AppendLine("<!-- ID:主品牌ID -->");
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					sb.AppendLine(string.Format("<Item ID=\"{0}\"/>", dr["bs_id"].ToString()));
				}
			}
			CommonFunction.EchoXml(this.Response, sb.ToString(), "root");
		}

		//统一输出XML
		private void Echo(string str)
		{
			HttpContext.Current.Response.ContentType = "text/xml";
			HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
			HttpContext.Current.Response.Clear();

			StringBuilder sb = new StringBuilder();

			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			sb.Append("<root>");
			sb.Append(str);
			sb.Append("</root>");
			Response.Write(sb.ToString());
			Response.End();
		}
		//加载XML
		private XmlDocument LoadXml()
		{
			string physicsPath = System.Web.HttpContext.Current.Server.MapPath(@"~/config/CooperationConfig.xml");
			XmlDocument xmlDoc = null;
			string cacheName = "BITAUTO_Cooperation_Car_Photo_API";
			Cache cache = System.Web.HttpContext.Current.Cache;
			try
			{
				if (cache[cacheName] != null)
				{
					xmlDoc = (XmlDocument)cache[cacheName];
				}
				else
				{
					xmlDoc = new XmlDocument();
					xmlDoc.Load(physicsPath);
					XmlElement root = xmlDoc.DocumentElement;
					cache.Insert(cacheName, xmlDoc, new CacheDependency(physicsPath));
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.StackTrace);
			}
			return xmlDoc;
		}
		//获取XML数据
		private List<NameValueCollection> GetXmlData()
		{
			List<NameValueCollection> list = new List<NameValueCollection>();
			try
			{
				XmlDocument xmlDoc = LoadXml();
				XmlElement root = xmlDoc.DocumentElement;

				XmlNodeList cNameList = root.SelectNodes(@"//Cooperation");
				foreach (XmlNode node in cNameList)
				{
					NameValueCollection nvc = new NameValueCollection();
					nvc.Add("cname", node.Attributes["Name"].Value);
					XmlNode ipRange = node.SelectSingleNode(@"./IPRange");
					if (ipRange != null)
					{
						nvc.Add("iprange", ipRange.InnerText);
					}
					XmlNode paramList = node.SelectSingleNode(@"./CarParamList");
					if (paramList != null)
					{
						nvc.Add("paramlist", paramList.InnerText);
					}
					list.Add(nvc);
				}
			}
			catch (Exception ex) { throw new Exception(ex.StackTrace); }
			return list;
		}
	}
}