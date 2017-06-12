using System;
using System.Xml;
using System.Collections.Generic;
using System.Web.Caching;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL;

namespace BitAuto.CarChannel.CarchannelWeb.PageList
{
	public partial class SerialForCharList : PageBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            base.SetPageCache(10);
			lrContent.Text = GetRenderedHtml();
		}

		private string GetRenderedHtml()
		{
			string cacheKey = "serial-char-list";
			object objHtml = null;
			CacheManager.GetCachedData(cacheKey, out objHtml);
			if (objHtml == null)
			{
				objHtml = RenderCharList();
				CacheDependency cacheDepend = new CacheDependency(WebConfig.AutoDataFile);
				CacheManager.InsertCache(cacheKey, objHtml, cacheDepend, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
			}

			return (string)objHtml;
		}

		/// <summary>
		/// 生成按字母列表的Html
		/// </summary>
		/// <returns></returns>
		private string RenderCharList()
		{
			//获取数据xml
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			//按字母分类
			Dictionary<string, List<XmlElement>> serialDic = new Dictionary<string, List<XmlElement>>();

			//遍历所有子品牌节点
			XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
			foreach (XmlElement serialNode in serialNodeList)
			{
				//首字母
				string[] firstChars = serialNode.GetAttribute("CsMultiChar").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string firstChar in firstChars)
				{
					string fChar = CommonFunction.ConvertNumToChar(firstChar).ToUpper();
					if (!serialDic.ContainsKey(fChar))
					{
						serialDic[fChar] = new List<XmlElement>();
					}
					if (!serialDic[fChar].Contains(serialNode))
						serialDic[fChar].Add(serialNode);
				}
			}

			string[] charList = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q",
			"R","S","T","U","V","W","X","Y","Z"};

			//生成代码
			List<string> htmlCode = new List<string>();
			htmlCode.Add("<dl class=\"bybrand_list byletters_list\">");
			//已处理字母计数
			int charCounter = 0;
			foreach (string fChar in charList)
			{
				if (!serialDic.ContainsKey(fChar))
					continue;
				charCounter++;
				htmlCode.Add("<dt><label>" + fChar + "</label><div><span id=\"" + fChar + "\">&nbsp;</span></div></dt>");
				htmlCode.Add("<dd>");

				//生成子品牌列表
				new Car_SerialBll().RenderSerialsBySpell(htmlCode, serialDic[fChar], true);

				if (charCounter == serialDic.Count)
					htmlCode.Add("<div class=\"hideline\"></div>");
				htmlCode.Add("</dd>");
                htmlCode.Add("<dd class=\"line\"></dd>");
			}
			htmlCode.Add("</dl>");

			//字母导航
            string navHtml = CommonFunction.RenderCharNavForDefaultPage(serialDic);
            return navHtml + String.Concat(htmlCode.ToArray());
		}
	}
}