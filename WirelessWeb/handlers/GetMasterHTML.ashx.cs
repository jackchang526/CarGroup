using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;
using BitAuto.CarChannel.Common.Cache;

namespace WirelessWeb.handlers
{
	/// <summary>
	/// GetMasterJson 的摘要说明
	/// </summary>
	public class GetMasterHTML : IHttpHandler
	{
		public static string[] CharList = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q",
			"R","S","T","U","V","W","X","Y","Z"};

		HttpResponse response;
		HttpRequest request;
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/html";
			response = context.Response;
			request = context.Request;
			RenderContent();
		}

		private void RenderContent()
		{
			string cacheKey = "Car_Wireless_GetMasterHTML";
			object masterHtml = CacheManager.GetCachedData(cacheKey);
			if (masterHtml != null)
				response.Write((string)masterHtml);
			else
			{
				StringBuilder sb = new StringBuilder();

				sb.Append("<div class=\"b-return\">");
				sb.Append("<a href=\"javascript:goCarCompare();\" class=\"btn-return\">返回</a>");
                sb.Append("<span>选择车型</span>");
                sb.Append("</div>");
				sb.Append("<div class=\"mbt-page\">");
				sb.Append(MakeMasterBrand());
				sb.Append("</div>");
				CacheManager.InsertCache(cacheKey, sb.ToString(), 60 * 24);
				response.Write(sb.ToString());
			}
		}
		/// <summary>
		/// 生成字母导航
		/// </summary>
		/// <param name="charNav"></param>
		/// <returns></returns>
		private string MakeCharNav(Dictionary<string, bool> charNav)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<div class=\"m-letter\"><ul>");
			foreach (string key in CharList)
			{
				if (charNav.ContainsKey(key))
					sb.AppendFormat("<li id=\"char_{0}\"><a href=\"#char_nav_{0}\">{0}</a></li>", key);
				else
					sb.Append("<li class=\"none\">" + key + "</li>");
			}
			sb.Append("</ul><div class=\"clear\"></div></div>");
			return sb.ToString();
		}
		/// <summary>
		/// 生成主品牌HTML
		/// </summary>
		/// <returns></returns>
		private string MakeMasterBrand()
		{
			StringBuilder sb = new StringBuilder();
			Dictionary<string, bool> charNav = new Dictionary<string, bool>();
			//获取数据xml
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			//遍历所有主品牌节点
			XmlNodeList mbNodeList = mbDoc.SelectNodes("/Params/MasterBrand");
			sb.Append("<div id=\"m-car-logo\" class=\"m-car-logo bybrand_list\">");
			sb.Append("<ul>");
			for (int i = 0; i < mbNodeList.Count; i++)
			{
				XmlElement mbNode = (XmlElement)mbNodeList[i];
				string masterSpell = mbNode.GetAttribute("AllSpell").ToLower();
				//首字母
				string firstChar = mbNode.GetAttribute("Spell").Substring(0, 1).ToUpper();
				//生成主品牌图标
				int mbId = ConvertHelper.GetInteger(mbNode.GetAttribute("ID"));
				string mbName = mbNode.GetAttribute("Name");

				XmlNodeList serialNodeList = mbNode.SelectNodes("./Brand/Serial");
				if (serialNodeList.Count <= 0) continue;

				if (!charNav.ContainsKey(firstChar))
				{
					sb.AppendFormat("<li id=\"char_nav_{0}\" class=\"m-root m-popup-arrow\">", firstChar);
					sb.AppendFormat("<strong>{0}</strong>", firstChar);
					sb.Append("<ul class=\"m-root-item clearfix\">");
					sb.Append("</ul>");
					sb.Append("</li>");
					charNav.Add(firstChar, true);
				}
				sb.Insert(sb.Length - 10, string.Format("<li id=\"mb_{0}\"><a href=\"javascript:;\" class=\"m-brand m_{0}_b\"></a><a href=\"javascript:;\" class=\"m-car-name\">{1}</a></li>",
					mbId,
					mbName
					));
			}
			sb.Append("</ul>");
			sb.Append("		<div class=\"clear\"></div>");
			sb.Append("	</div>");
			sb.Insert(0, MakeCharNav(charNav));
			return sb.ToString();
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