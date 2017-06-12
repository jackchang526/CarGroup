using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using BitAuto.Utils;

namespace WirelessWeb.UserControls
{
	public partial class SelectCarTool : System.Web.UI.UserControl
	{
		readonly Regex regexQurey = new Regex(@"^(\d+((\.\d+)?(-|_)\d+(\.\d+)?)?)+$", RegexOptions.IgnoreCase);
		public string SearchUrl { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected virtual string GetSearchQueryString(string query, string value)
		{
			string param = GetParamsString();
			NameValueCollection collection = Request.QueryString;
			string path = this.SearchUrl + "?";
			var queryString = HttpUtility.ParseQueryString(param);
			if (query == "b" || query == "lv")
			{
				if (query == "b" && !string.IsNullOrEmpty(collection["lv"]))
				{
					queryString.Remove("lv");
				}
				else if (query == "lv" && !string.IsNullOrEmpty(collection["b"]))
				{
					queryString.Remove("b");
				}
			}
			if (query == "g" || query == "c")
			{
				if (query == "g" && !string.IsNullOrEmpty(collection["c"]))
				{
					queryString.Remove("c");
				}
				else if (query == "c" && !string.IsNullOrEmpty(collection["g"]))
				{
					queryString.Remove("g");
				}
			}
			if (string.IsNullOrEmpty(collection[query]))
			{
				queryString.Add(query, value);
			}
			else
			{
				queryString[query] = value;
			}

			return path + queryString.ToString();
		}
		protected virtual string GetParamsString()
		{
			StringBuilder sb = new StringBuilder();
			NameValueCollection collection = Request.QueryString;

			foreach (string key in collection.Keys)
			{
				if (string.IsNullOrEmpty(key))
				{
					continue;
				}

				if (!string.IsNullOrEmpty(collection[key]) && this.IsInArray(key) && this.IsMatchQueryString(collection[key]))
				{
					if (sb.Length > 0)
					{
						sb.Append("&");
					}

					sb.Append(key + "=" + collection[key]);
				}
			}

			return sb.ToString();
		}
		protected readonly string[] querys = { "p", "l", "d", "g", "c", "t", "dt", "f", "b", "lv", "more" };
		protected virtual bool IsInArray(string array)
		{
			bool isOK = false;

			foreach (string qu in querys)
			{
				if (array.ToLower() == qu)
				{
					isOK = true;

					break;
				}
			}

			return isOK;
		}
		/// <summary>
		/// 验证 字符串参数有效性 1.3-1.6 、200、200_300
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		protected bool IsMatchQueryString(string query)
		{
			if (string.IsNullOrEmpty(query)) return false;
			//string regexString = @"^(\d+((\.\d+)?(-|_)\d+(\.\d+)?)?)+$";
			//Regex r = new Regex(regexString, RegexOptions.IgnoreCase);
			return regexQurey.IsMatch(query);
		}
		protected string GenerateSearchInitScript()
		{
			StringBuilder scriptCode = new StringBuilder();
			string tmpStr = Request.QueryString["p"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("SelectCarTool.Price='" + tmpStr + "';");
			int l = ConvertHelper.GetInteger(Request.QueryString["l"]);
			if (l > 0)
				scriptCode.AppendLine("SelectCarTool.Level=" + l + ";");
			tmpStr = Request.QueryString["d"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("SelectCarTool.Displacement='" + tmpStr + "';");
			int t = ConvertHelper.GetInteger(Request.QueryString["t"]);
			if (t > 0)
				scriptCode.AppendLine("SelectCarTool.TransmissionType=" + t + ";");
			tmpStr = Request.QueryString["more"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("SelectCarTool.SetMoreCondition('" + tmpStr + "');");
			int g = ConvertHelper.GetInteger(Request.QueryString["g"]);
			if (g > 0)
				scriptCode.AppendLine("SelectCarTool.Brand=" + g + ";");
			int c = ConvertHelper.GetInteger(Request.QueryString["c"]);
			if (c > 0)
				scriptCode.AppendLine("SelectCarTool.Country=" + c + ";");
			int b = ConvertHelper.GetInteger(Request.QueryString["b"]);
			if (b > 0)
				scriptCode.AppendLine("SelectCarTool.BodyForm=" + b + ";");
			int lv = ConvertHelper.GetInteger(Request.QueryString["lv"]);
			if (lv > 0)
				scriptCode.AppendLine("SelectCarTool.IsWagon=" + lv + ";");
			int dt = ConvertHelper.GetInteger(Request.QueryString["dt"]);
			if (dt > 0)
				scriptCode.AppendLine("SelectCarTool.DriveType=" + dt + ";");
			int f = ConvertHelper.GetInteger(Request.QueryString["f"]);
			if (f > 0)
				scriptCode.AppendLine("SelectCarTool.FuelType=" + f + ";");
			//tmpStr = Request.QueryString["sn"];
			//if (!String.IsNullOrEmpty(tmpStr))
			//    scriptCode.AppendLine("SelectCarTool.PerfSeatNum='" + tmpStr + "';");
			tmpStr = Request.QueryString["v"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("SelectCarTool.View=" + tmpStr + ";");
			int s = ConvertHelper.GetInteger(Request.QueryString["s"]);
			if (s > 0)
				scriptCode.AppendLine("SelectCarTool.Sort=" + s + ";");
			tmpStr = Request.QueryString["e"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("SelectCarTool.Envelope=" + tmpStr + ";");
			return scriptCode.ToString();
		}
	}
}