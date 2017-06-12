using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;
using BitAuto.CarUtils.Define;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Level
{
	/// <summary>
	/// GetLevelHtmlCode 的摘要说明
	/// </summary>
	public class GetLevelHtmlCode : InterfacePageBase, IHttpHandler
	{
		private int level = 1;
		private string type = string.Empty;
		private string levelName = string.Empty;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			GetParams(context);
			context.Response.Write(GetInnerHTML());
		}

		/// <summary>
		/// 得到页面参数
		/// </summary>
		/// <param name="context"></param>
		private void GetParams(HttpContext context)
		{
			level = string.IsNullOrEmpty(context.Request.QueryString["level"])
				? 1 : ConvertHelper.GetInteger(context.Request.QueryString["level"]);
			type = string.IsNullOrEmpty(context.Request.QueryString["type"])
				? "brandTag" : context.Request.QueryString["type"];

			//levelName = ((EnumCollection.SerialLevelEnum)level).ToString();
			levelName = CarLevelDefine.GetLevelNameById(level);
		}
		/// <summary>
		/// 得到HTML内容
		/// </summary>
		/// <returns></returns>
		private string GetInnerHTML()
		{
			//获取数据xml
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			//遍历所有子品牌节点
			XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial[@CsLevel=\"" + levelName + "\"]");
			List<XmlElement> serialList = new List<XmlElement>();
			foreach (XmlElement serialNode in serialNodeList)
			{
				serialList.Add(serialNode);
			}
			Car_SerialBll csBll = new Car_SerialBll();

			if (type.ToLower() == "brandtag")
			{
				return RenderSerialByBrand();
			}
			else
			{
				return RenderSerialsBySpell(serialList);
			}
		}
		/// <summary>
		/// 按字母排列
		/// </summary>
		/// <param name="htmlCode"></param>
		private string RenderSerialsBySpell(List<XmlElement> serialList)
		{
			List<string> htmlCode = new List<string>();
			Car_SerialBll csBll = new Car_SerialBll();
			Dictionary<string, List<XmlElement>> serialDic = new Dictionary<string, List<XmlElement>>();
			foreach (XmlElement serialNode in serialList)
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

			string charNav = CommonFunction.RenderCharNav(serialDic, "charlist", false, "pre");
			htmlCode.Add(charNav);
			htmlCode.Add("<dl id=\"charContent\" class=\"byletters_list\" style=\"display:none\">");
			//按字母分类
			int charCounter = 0;
			foreach (string fChar in CommonFunction.CharList)
			{
				if (!serialDic.ContainsKey(fChar))
					continue;
				charCounter++;

				if (charCounter == 1)
					htmlCode.Add("<dt><label id=\"" + "pre" + fChar + "\">" + fChar + "</label></dt>");
				else
					htmlCode.Add("<dt><label id=\"" + "pre" + fChar + "\">" + fChar + "</label><a href=\"#pageTop\" class=\"gotop\">返回顶部↑</a></dt>");

				htmlCode.Add("<dd>");
				csBll.RenderSerialsBySpellNoLevel(htmlCode, serialDic[fChar], true);
				if (charCounter == serialDic.Count)
					htmlCode.Add("<div class=\"hideline\"></div>");
				htmlCode.Add("</dd>");

			}
			htmlCode.Add("</dl>");
			return String.Concat(htmlCode.ToArray());
		}

		/// <summary>
		/// 按品牌显示子品牌列表
		/// </summary>
		/// <param name="htmlCode"></param>
		/// <param name="levelName"></param>
		private string RenderSerialByBrand()
		{
			List<string> htmlCode = new List<string>();
			//获取数据xml
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();


			//遍历所有主品牌节点
			XmlNodeList mbNodeList = mbDoc.SelectNodes("/Params/MasterBrand");

			Dictionary<string, List<XmlElement>> mbDic = new Dictionary<string, List<XmlElement>>();
			int mbNodeCount = 0;
			//取出所有带有当前级别的主品牌
			foreach (XmlElement mbNode in mbNodeList)
			{
				//判断是否有当前级别的子品牌
				XmlNodeList serialList = mbNode.SelectNodes("Brand/Serial[@CsLevel=\"" + levelName + "\"]");
				if (serialList.Count == 0)
					continue;
				mbNodeCount++;
				//首字母
				string firstChar = mbNode.GetAttribute("Spell").Substring(0, 1).ToUpper();
				if (!mbDic.ContainsKey(firstChar))
					mbDic[firstChar] = new List<XmlElement>();
				mbDic[firstChar].Add(mbNode);
			}
			string charNav = CommonFunction.RenderCharNav(mbDic, "brandcharlist", false, "");
			htmlCode.Add(charNav);
			//Html
			htmlCode.Add("<dl id=\"brandContent\" class=\"bybrand_list\" style=\"display:none\">");

			int mbCounter = 0;			//主品牌计数器
			int charCounter = 0;		//字母计数器
			foreach (string fChar in CommonFunction.CharList)
			{
				if (!mbDic.ContainsKey(fChar))
					continue;
				charCounter++;

				if (charCounter == 1)
					htmlCode.Add("<dt><label id=\"" + fChar + "\">" + fChar + "</label></dt>");
				else
					htmlCode.Add("<dt><label id=\"" + fChar + "\">" + fChar + "</label><a href=\"#pageTop\" class=\"gotop\">返回顶部↑</a></dt>");
				foreach (XmlElement mbNode in mbDic[fChar])
				{
					mbCounter++;
					//生成主品牌图标与链接
					string mbUrl = "http://car.bitauto.com/" + mbNode.GetAttribute("AllSpell").ToLower() + "/";
					string mbId = mbNode.GetAttribute("ID");
					string mbName = mbNode.GetAttribute("Name");
					htmlCode.Add("<dd class=\"b\">");
					htmlCode.Add("<a href=\"" + mbUrl + "\" target=\"_blank\"><div class=\"brand m_" + mbId + "_b\"></div></a>");
					htmlCode.Add("<div class=\"brandname\"><a href=\"" + mbUrl + "\" target=\"_blank\">" + mbName + "</a></div></dd>");
					//生成品牌列表
					RenderBrands(htmlCode, mbNode);

					//一条线
					if (mbCounter < mbNodeCount)
						htmlCode.Add("<dd class=\"line\"></dd>");
				}
			}
			htmlCode.Add("</dl>");
			return String.Concat(htmlCode.ToArray());
		}
		/// <summary>
		/// 生成主品牌下各品牌的Html
		/// </summary>
		/// <param name="htmlCode">代码容器</param>
		/// <param name="mbNode">主品牌信息</param>
		private void RenderBrands(List<string> htmlCode, XmlElement mbNode)
		{

			htmlCode.Add("<dd class=\"have\">");
			//获取品牌信息
			List<XmlElement> brandList = new List<XmlElement>();
			foreach (XmlElement ele in mbNode.SelectNodes("Brand"))
			{
				//判断是否有当前级别的子品牌
				XmlNodeList serialList = ele.SelectNodes("Serial[@CsLevel=\"" + levelName + "\"]");
				if (serialList.Count == 0)
					continue;
				brandList.Add(ele);
			}

			//添加排序条件
			brandList.Sort(NodeCompare.CompareBrandNodeSelfFirst);

			bool isFirstBrand = true;

			foreach (XmlElement brandNode in brandList)
			{
				//生成品牌Html
				string brandId = brandNode.GetAttribute("ID");
				string brandName = brandNode.GetAttribute("Name");
				string brandSpell = brandNode.GetAttribute("AllSpell");
				if (isFirstBrand)
				{
					htmlCode.Add("<h2><a href=\"http://car.bitauto.com/" + brandSpell + "/\" target=\"_blank\">" + brandName + "</a></h2>");
					isFirstBrand = false;
				}
				else
					htmlCode.Add("<h2 class=\"border\"><a href=\"http://car.bitauto.com/" + brandSpell + "/\" target=\"_blank\">" + brandName + "</a></h2>");

				//加入列表
				XmlNodeList serialNodeList = brandNode.SelectNodes("Serial[@CsLevel=\"" + levelName + "\"]");
				List<XmlElement> serialList = new List<XmlElement>();
				foreach (XmlElement serialNode in serialNodeList)
				{
					serialList.Add(serialNode);
				}

				//生成子品牌列表
				new Car_SerialBll().RenderSerialsBySpellNoLevel(htmlCode, serialList, false);
			}

			htmlCode.Add("</dd>");
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