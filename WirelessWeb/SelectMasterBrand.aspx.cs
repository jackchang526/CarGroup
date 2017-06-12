using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BitAuto.CarChannel.BLL.CarTreeData;
using BitAuto.Utils;
using System.Xml;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common;
using System.IO;

namespace WirelessWeb
{
	/// <summary>
	/// 选车主品牌页面
	/// </summary>
	public partial class SelectMasterBrand : WirelessTreeBase
	{
		public static string[] CharList = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q",
			"R","S","T","U","V","W","X","Y","Z"};
		protected string brandListHtml;
		protected string letterListHtml;
		protected string re = string.Empty; //seo 参数 判断 是否直接输出
		protected Dictionary<string, bool> charNav = new Dictionary<string, bool>();
		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
			re = Request.QueryString["re"];
			this._SearchUrl = InitSearchUrl("chexing");
			Response.ContentType = "text/html";
			brandListHtml = MakeMasterBrand();
			letterListHtml = MakeCharNav(charNav);
		}
		/// <summary>
		/// 生成字母导航
		/// </summary>
		/// <param name="charNav"></param>
		/// <returns></returns>
		private string MakeCharNav(Dictionary<string, bool> charNav)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string key in CharList)
			{
				if (charNav.ContainsKey(key))
				{
					sb.AppendFormat("<li id=\"char_{0}\"><a href=\"#char_nav_{0}\">{0}</a></li>", key);
				}
			}
			return sb.ToString();
		}
		/// <summary>
		/// 生成主品牌HTML
		/// </summary>
		/// <returns></returns>
		private string MakeMasterBrand()
		{
			StringBuilder sb = new StringBuilder();
			//获取数据xml
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			//遍历所有主品牌节点
			XmlNodeList mbNodeList = mbDoc.SelectNodes("/Params/MasterBrand");
			for (int i = 0; i < mbNodeList.Count; i++)
			{
				XmlElement mbNode = (XmlElement)mbNodeList[i];
				string masterSpell = mbNode.GetAttribute("AllSpell").ToLower();
				//首字母
				string firstChar = mbNode.GetAttribute("Spell").Substring(0, 1).ToUpper();
				//生成主品牌图标
				int mbId = ConvertHelper.GetInteger(mbNode.GetAttribute("ID"));
				string mbName = mbNode.GetAttribute("Name");

				if (!charNav.ContainsKey(firstChar))
				{
					sb.AppendFormat("<div id=\"char_nav_{0}\" class=\"tt-small phone-title\" data-key=\"{0}\">", firstChar);
					sb.AppendFormat("<span>{0}</span>", firstChar);
					sb.Append("</div>");
					sb.Append("<div class=\"box\">"); ;
					sb.Append("<ul>");
					sb.Append("</ul>");
					sb.Append("</div>");
					charNav.Add(firstChar, true);
				}
				//seo 直接跳转
				if (re == "true")
				{
					sb.Insert(sb.Length - 11, string.Format("<li id=\"char_nav_{2}\"><a href=\"/brandlist/{2}/\"> <span class=\"brand-logo m_{0}_b\"></span><span class=\"brand-name\">{1}</span></a></li>",
						mbId,
						mbName,
						masterSpell));
				}
				else
				{
					sb.Insert(sb.Length - 11, string.Format("<li id=\"char_nav_{2}\"><a data-id=\"{0}\" href=\"javascript:void(0);\" data-action=\"car\"> <span class=\"brand-logo m_{0}_b\"></span><span class=\"brand-name\">{1}</span></a></li>",
					   mbId,
					   mbName,
					   masterSpell));
				}
			}
			return sb.ToString();
		}
	}
}