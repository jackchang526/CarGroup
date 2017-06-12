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
	public partial class CarMasterBrandSelect : WirelessTreeBase
	{
		public static string[] CharList = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q",
			"R","S","T","U","V","W","X","Y","Z"};
		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
			this._SearchUrl = InitSearchUrl("chexing");
			Response.ContentType = "text/html";
			//FileWrite("~/include/masterbrand.htm", MakeMasterBrand());
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
		public static void FileWrite(string logFileName, string str)
		{
			string logdir;
			logdir = System.Web.HttpContext.Current.Server.MapPath(logFileName);
			try
			{
				using (FileStream fs = new FileStream(logdir, FileMode.OpenOrCreate, FileAccess.Write))
				{
					using (StreamWriter sw = new System.IO.StreamWriter(fs))
					{
						sw.WriteLine(str);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}