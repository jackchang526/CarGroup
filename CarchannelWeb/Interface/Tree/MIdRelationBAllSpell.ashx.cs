using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Tree
{
	/// <summary>
	/// MIdRelationBAllSpell 的摘要说明
	/// </summary>
	public class MIdRelationBAllSpell : InterfacePageBase, IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Write(GetReleation());
		}

		/// <summary>
		/// 得到主品牌ID与品牌AllSpell的对应关系
		/// </summary>
		/// <returns></returns>
		public string GetReleation()
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc = AutoStorageService.GetAllAutoXml();
			if (xmlDoc == null) return "";
			XmlNodeList masterNodeList = xmlDoc.SelectNodes("Params/MasterBrand");
			if (masterNodeList == null || masterNodeList.Count < 1) return "";

			StringBuilder rStrBuilder = new StringBuilder();
			foreach (XmlElement entity in masterNodeList)
			{
				if (entity.ChildNodes == null || entity.ChildNodes.Count < 1) continue;

				rStrBuilder.AppendFormat(",\"{0}\":\"{1}\"", "m" + entity.GetAttribute("ID"), ((XmlElement)entity.ChildNodes[0]).GetAttribute("AllSpell").ToLower());
			}
			return "var mtobspell = {" + rStrBuilder.Remove(0, 1).ToString() + "}";
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