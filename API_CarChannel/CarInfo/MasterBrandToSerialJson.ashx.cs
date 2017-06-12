using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;
using BitAuto.CarChannelAPI.Web.AppCode;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// MasterBrandToSerialJson 的摘要说明
	/// 级联菜单json接口 for yangjie1 add by chengl May.21.2014
	/// </summary>
	public class MasterBrandToSerialJson : PageBase, IHttpHandler
	{
		private HttpRequest _request;
		private HttpResponse _response;
		private int _Conditions = 4;//用户查询数据的条件
		private int _Include = 0;//是否包含上一级的数据
		private int _ParentId = 0;//父级ID是多少
		private string _RequestType = string.Empty;//请求类型
		private string callback = string.Empty;
		private string jsonVarName = string.Empty;
		private List<string> jsonString = new List<string>();

		public void ProcessRequest(HttpContext context)
		{
			//context.Response.ContentType = "text/plain";
			//context.Response.Write("Hello World");
			PageHelper.SetPageCache(60 * 4);
			context.Response.ContentType = "application/x-javascript";

			_response = context.Response;
			_request = context.Request;

			GetParams();
			GetContent();
			ResponseJson();
		}

		/// <summary>
		/// 输出
		/// </summary>
		private void ResponseJson()
		{
			if (jsonString.Count > 0)
			{
				jsonString.Insert(0, "[");
				jsonString.Add("]");

				// JsonP
				if (!string.IsNullOrEmpty(callback))
				{
					jsonString.Insert(0, callback + "(");
					jsonString.Add(")");
				}
				else if (!string.IsNullOrEmpty(jsonVarName))
				{
					// 非jsonP
					jsonString.Insert(0, "var " + jsonVarName + " = ");
					jsonString.Add(";");
				}
				else
				{ }
			}
			else
			{
				// JsonP
				if (!string.IsNullOrEmpty(callback))
				{
					jsonString.Add(callback + "({})");
				}
				else if (!string.IsNullOrEmpty(jsonVarName))
				{
					// 非jsonP
					jsonString.Add("var " + jsonVarName + " = null;");
				}
				else
				{ jsonString.Add("参数错误"); }
			}
			_response.Write(string.Concat(jsonString.ToArray()));
		}

		/// <summary>
		/// 得到当前内容的上下文
		/// </summary>
		/// <param name="context"></param>
		private void GetParams()
		{
			_Conditions = string.IsNullOrEmpty(_request.QueryString["type"]) ? 4 : ConvertHelper.GetInteger(_request.QueryString["type"]);
			_Include = string.IsNullOrEmpty(_request.QueryString["include"]) ? 0 : ConvertHelper.GetInteger(_request.QueryString["include"]);
			_ParentId = string.IsNullOrEmpty(_request.QueryString["pid"]) ? 0 : ConvertHelper.GetInteger(_request.QueryString["pid"]);
			_RequestType = string.IsNullOrEmpty(_request.QueryString["rt"]) ? string.Empty : _request.QueryString["rt"];

			callback = _request.QueryString["callback"];

			jsonVarName = _request.QueryString["jsonVarName"];
		}

		/// <summary>
		/// 得到页面内容
		/// </summary>
		/// <returns></returns>
		private void GetContent()
		{
			_RequestType = _RequestType.ToLower();
			switch (_RequestType)
			{
				case "serial":
					GetSerialContent(); break;
				case "master":
					GetMasterBrandContent(); break;
				case "brand":
					GetBrandContent(); break;
				case "cartype":
					GetCarTypeContent(); break;
				default: break;
			}
		}

		/// <summary>
		/// 得到子品牌的内容
		/// </summary>
		/// <returns></returns>
		private void GetSerialContent()
		{
			if (_ParentId < 1) return;
			string cacheKey = string.Format("leveldropdownlistControlJson_{0}_{1}_{2}_{3}", _Conditions, _Include, _ParentId, _RequestType);
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				jsonString.Add((string)obj);
				return;
			}

			DataSet ds = new Car_SerialBll().GetIsConditionsDataSet(_Conditions);
			if (ds == null) return;
			//根据条件不同，设置不同的查询字符串
			string condition = string.Format("bs_id={0}", _ParentId);
			if (_Include != 1)
			{
				condition = string.Format("cb_id={0}", _ParentId);
			}

			DataRow[] drList = ds.Tables[0].Select(condition);
			if (drList == null || drList.Length < 0) return;

			foreach (DataRow dr in drList)
			{
				int csId = Convert.ToInt32(dr["cs_id"]);
				string name = dr["cs_name"].ToString().Trim();
				if (csId == 1568) name = "索纳塔八";
				if (jsonString.Count > 0)
				{
					jsonString.Add(",");
				}
				jsonString.Add("{");
				jsonString.Add(string.Format("\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"{4}\",\"urlSpell\":\"{2}\",\"showName\":\"{3}\",\"csSale\":\"{5}\" {6}"
									 , csId
									 , name
									 , dr["csallspell"].ToString().ToLower()
									 , dr["cs_ShowName"].ToString().Trim()
									 , _Include == 1 ? dr["bs_id"] : dr["cb_id"]
									 , GetSerialSaleState(dr["csSaleState"].ToString().Trim())
									 , GetIncludeContent(dr, "cb_id", new string[] { "cb_name" })));
				jsonString.Add("}");
			}

			string content = string.Concat(jsonString.ToArray());

			CacheManager.InsertCache(cacheKey, content, WebConfig.CachedDuration);
		}
		/// <summary>
		/// 得到主品牌的内容
		/// </summary>
		/// <returns></returns>
		private void GetMasterBrandContent()
		{
			string cacheKey = string.Format("leveldropdownlistControlJson_{0}_{1}_{2}_{3}", _Conditions, _Include, _ParentId, _RequestType);
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				jsonString.Add((string)obj);
				return;
			}

			DataSet ds = new Car_SerialBll().GetIsConditionsDataSet(_Conditions);
			if (ds == null) return;

			DataTable dt = ds.Tables[0].DefaultView.ToTable(true, "bs_id", "bs_Name", "bsallspell", "bsspell");
			if (dt == null || dt.Rows.Count < 1) return;

			foreach (DataRow dr in dt.Rows)
			{
				if (jsonString.Count > 0)
				{
					jsonString.Add(",");
				}
				jsonString.Add("{");
				jsonString.Add(string.Format("\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"0\",\"urlSpell\":\"{2}\",\"tSpell\":\"{3}\",\"nl\":[{4}]"
									   , dr["bs_id"]
									   , dr["bs_Name"].ToString().Trim()
									   , dr["bsallspell"].ToString().ToLower()
									   , dr["bsspell"]
									   , ""));
				jsonString.Add("}");
			}

			string content = string.Concat(jsonString.ToArray());

			CacheManager.InsertCache(cacheKey, content, WebConfig.CachedDuration);
		}
		/// <summary>
		/// 得到品牌的内容
		/// </summary>
		/// <returns></returns>
		private void GetBrandContent()
		{
			string cacheKey = string.Format("leveldropdownlistControlJson_{0}_{1}_{2}_{3}", _Conditions, _Include, _ParentId, _RequestType);
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				jsonString.Add((string)obj);
				return;
			}

			DataSet ds = new Car_SerialBll().GetIsConditionsDataSet(_Conditions);
			if (ds == null) return;
			DataTable dt = ds.Tables[0].DefaultView.ToTable(true, "cb_id", "cb_name", "cballspell", "bs_id", "bs_Name", "bsallspell", "bsspell");
			if (dt == null || dt.Rows.Count < 1) return;
			DataView dv = dt.DefaultView;
			if (_ParentId > 0)
			{
				dv.RowFilter = "bs_id=" + _ParentId;
			}

			foreach (DataRow dr in dv.ToTable().Rows)
			{
				if (jsonString.Count > 0)
				{
					jsonString.Add(",");
				}
				jsonString.Add("{");
				jsonString.Add(string.Format("\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"{3}\",\"urlSpell\":\"{2}\",\"nl\":[] {4}"
										, dr["cb_id"]
										, dr["cb_Name"].ToString().Trim()
										, dr["cballspell"].ToString().ToLower()
										, dr["bs_id"]
										, GetIncludeContent(dr, "bs_id", new string[] { "bsspell", "bs_Name" })));
				jsonString.Add("}");
			}

			string content = string.Concat(jsonString.ToArray());

			CacheManager.InsertCache(cacheKey, content, WebConfig.CachedDuration);
		}
		/// <summary>
		/// 得到车型的内容
		/// </summary>
		/// <returns></returns>
		private void GetCarTypeContent()
		{
			string cacheKey = string.Format("leveldropdownlistControlJson_{0}_{1}_{2}_{3}", _Conditions, _Include, _ParentId, _RequestType);
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				jsonString.Add((string)obj);
				return;
			}

			if (_ParentId < 1) return;

			DataSet dsCar = new DataSet();
			if (_Conditions == 0)
			{
				dsCar = new CommonService().GetAllCarForAjaxCompareContainsStopSale(10);
			}
			else
			{
				dsCar = new CommonService().GetAllCarForAjaxCompare(10);
			}
			if (dsCar == null || dsCar.Tables.Count < 1 || dsCar.Tables[0].Rows.Count < 1) return;
			StringBuilder cartypeString = new StringBuilder();
			try
			{
				List<int> csidlist = new List<int>();
				csidlist.Add(_ParentId);

				foreach (int id in csidlist)
				{
					DataRow[] drs = dsCar.Tables[0].Select(" cs_id=" + id + " ");
					if (drs == null || drs.Length < 1) continue;
					StringBuilder tempString = new StringBuilder();
					foreach (DataRow dr in drs)
					{
						if (tempString.Length > 0)
						{ tempString.Append(","); }
						tempString.Append("{");
						tempString.AppendFormat("\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"{3}\",\"goid\":\"{2}\",\"goname\":\"{2}\",\"referprice\":\"{4}\",\"tt\":\"{5}\""
											, dr["car_id"]
											, dr["car_name"]
											, string.Equals((string)dr["Car_YearType"], "无") ? "未知年款" : dr["Car_YearType"]
											, id
											, dr["car_ReferPrice"]
											, dr["TT"]);
						tempString.Append("}");
					}
					jsonString.Add(tempString.ToString());
				}
			}
			catch
			{
				return;
			}

			string content = string.Concat(jsonString.ToArray());

			CacheManager.InsertCache(cacheKey, content, WebConfig.CachedDuration);
		}

		/// <summary>
		/// 得到包含关系
		/// </summary>
		/// <param name="dr"></param>
		/// <returns></returns>
		private string GetIncludeContent(DataRow dr, string id, string[] name)
		{
			if (dr == null) return "";

			if (_Include != 1) return "";

			string elementName = string.Empty;
			foreach (string n in name)
			{
				elementName += " " + dr[n];
			}
			elementName = elementName.Trim();
			return string.Format(",\"goid\":\"{0}\",\"goname\":\"{1}\"", dr[id], elementName);
		}
		/// <summary>
		/// 得到子品牌销售状态
		/// </summary>
		/// <param name="state"></param>
		private string GetSerialSaleState(string state)
		{
			switch (state)
			{
				case "在销":
					return "0";
				case "待销":
					return "1";
				case "停销":
					return "2";
				default: return "3";
			}
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