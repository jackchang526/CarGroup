using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// GetCityList 的摘要说明
	/// </summary>
	public class GetCityList : IHttpHandler
	{
		private string type = string.Empty;
		private int provinceId = 0;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			GetParams(context);
			if (type.ToLower() == "p")
				context.Response.Write(GetProvincesList(context));
			else
				context.Response.Write(GetCityListByID(context));
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 得到参数列表
		/// </summary>
		private void GetParams(HttpContext context)
		{
			type = string.IsNullOrEmpty(context.Request.QueryString["type"]) ? "" : context.Request.QueryString["type"];
			provinceId = string.IsNullOrEmpty(context.Request.QueryString["pId"]) ? 0 : ConvertHelper.GetInteger(context.Request.QueryString["pId"]);
		}
		/// <summary>
		/// 得到省列表
		/// </summary>
		private string GetProvincesList(HttpContext context)
		{
			XmlDocument xmlDoc = GetCityXml(context);
			if (xmlDoc == null) return "var provincelist = {};";
			XmlNodeList xNodeList = xmlDoc.SelectNodes("Root/PVC");
			if (xNodeList == null || xNodeList.Count < 1) return "var provincelist = {};";

			StringBuilder provStrBuilder = new StringBuilder();

			foreach (XmlElement xmlElem in xNodeList)
			{
				provStrBuilder.AppendFormat(",\"p{0}\":\"{1}\"", xmlElem.GetAttribute("pvc_Id"), xmlElem.GetAttribute("pvc_name"));
			}

			return "var provincelist = {" + provStrBuilder.Remove(0, 1).ToString() + "};";
		}
		/// <summary>
		/// 得到城市列表
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private string GetCityListByID(HttpContext context)
		{
			XmlDocument xmlDoc = GetCityXml(context);

			if (xmlDoc == null) return "var citylist = {};";
			XmlNodeList xNodeList = xmlDoc.SelectNodes("Root/PVC[@pvc_Id='" + provinceId + "']/City1");
			if (xNodeList == null || xNodeList.Count < 1) return "var citylist = {};";

			StringBuilder provStrBuilder = new StringBuilder();

			foreach (XmlElement xmlElem in xNodeList)
			{
				provStrBuilder.AppendFormat(",\"c{0}\":\"{1}\"", xmlElem.GetAttribute("cityId"), xmlElem.GetAttribute("cityname"));
			}

			return "var citylist = {" + provStrBuilder.Remove(0, 1).ToString() + "};";
		}
		/// <summary>
		/// 得到城市的XML文件
		/// </summary>
		/// <returns></returns>
		private XmlDocument GetCityXml(HttpContext context)
		{
			string cacheKey = "provinceandcitylist";

			object obj = context.Cache.Get(cacheKey);

			if (obj != null) return (XmlDocument)obj;

			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				xmlDoc.Load("http://baa.bitauto.com/tools/xml/city.xml");
				if (xmlDoc == null) return null;

				context.Cache.Insert(cacheKey, xmlDoc);
				return xmlDoc;
			}
			catch
			{
				return null;
			}
		}

	}
}