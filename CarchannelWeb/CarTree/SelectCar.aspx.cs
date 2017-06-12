using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.CarchannelWeb.App_Code;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Model;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using System.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace BitAuto.CarChannel.CarchannelWeb.CarTree
{
	public partial class Cartree_SelectCar : TreePageBase
	{
		private SelectCarParameters selectParas;
		private string displayMode;
		private string sortMode;
		private string displayModeHtml;
		private string sortModeHtml;
		protected int SerialNum;
		protected int CarNum;
		private int pageNum;
		private int pageSize;
		private string priceTag = String.Empty;

		protected string pageTitle;
		protected string serialListHtml;
		protected string hotSerialHtml;

		protected string ADTopHtml = string.Empty;
		protected string ShowLevelName = "";
		protected string LevelName = "";
		protected string LevelDesc = "";
		protected string levelIsShow = "block";//无数据不显示级别描述
		protected NameValueCollection nvcAd;
		protected string topAd = "";
		protected string fullAd = "";
		protected string bottomAd = "";

		// add by chengl Jun.20.2012
		protected string metaKeywords = "";
		protected string metaDescription = "";

		protected string adCarListData = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60 * 4);
			//NavbarHtml = base.GetTreeNavBarHtml("search", "chexing", 0);

			GetParameters();
			string condition = Request.QueryString["p"];
			//初始化广告
			InitAdCode();
			//选车条件广告
			InitSelectCarAD();
			//级别描述
			InitLevelDesc();

			serialListHtml = MakeSelectBarHtml();
			MakeHotSerialHtml();
			//搜索地址
			this._SearchUrl = InitSearchUrl("chexing");
			//生成条件Html
			this.MakeConditionsHtmlNew();

			//百度分享代码
			InitBaiduShare();
		}

		private void InitAdCode()
		{
			string condition = "";
			string level = Request.QueryString["l"];
			string price = Request.QueryString["p"];
			string g = Request.QueryString["g"];
			string t = Request.QueryString["t"];
			string d = Request.QueryString["d"];
			string m = Request.QueryString["m"];
			string page = Request.QueryString["page"];
			if ((g != null && g != "0") || (t != null && t != "0") || (d != null && d != "") || (m != null && m != "") || (page != null && page != ""))
				return;
			if (level == "0")
				level = "";
			condition = level + price;
			NameValueCollection nvcAdCode = GetAdCode("selectcar", condition);
			topAd = nvcAdCode["top"];
			fullAd = nvcAdCode["fullscreen"];
			bottomAd = nvcAdCode["bottom"];
		}

		private void InitSelectCarAD()
		{
			int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);

			//选车工具广告输出 add by sk 2015.09.22
			List<SuperSerialInfo> adCarList = new List<SuperSerialInfo>();
			List<SerialListADEntity> listSerialAD = new Car_SerialBll().GetSerialAD("selectcar" + selectParas.ConditionString);
			if (pageIndex <= 1 && selectParas.BodyForm == 0 && selectParas.CarConfig == 0 && listSerialAD != null && listSerialAD.Count > 0)
			{
				foreach (SerialListADEntity serialAd in listSerialAD)
				{
					int index = serialAd.Pos - 1;
					if (index < 0)
						index = 0;

					SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialAd.SerialId);
					if (serialEntity != null)
					{
						adCarList.Add(new SuperSerialInfo(serialAd.SerialId, serialEntity.ShowName, serialEntity.AllSpell)
						{
							Pos = serialAd.Pos,
							CarIdList = string.Join(",", serialEntity.CarList.Select(p => p.Id)),
							ImageUrl = Car_SerialBll.GetSerialImageUrl(serialAd.SerialId, "1"),
							PriceRange = serialEntity.Price,
							CarNum = serialEntity.CarList.Length
						});
					}
				}
			}
			adCarListData = adCarList.Any() ? JsonConvert.SerializeObject(adCarList) : "[]";
		}

		private void InitBaiduShare()
		{
			NameValueCollection nvc = new NameValueCollection(Request.QueryString);
			if (Array.IndexOf(nvc.AllKeys, "page") != -1)
				nvc.Remove("page");
			if (Array.IndexOf(nvc.AllKeys, "l") != -1 && nvc.Count == 1 && nvc["l"] != "0")
				litLevelBaiduShare.Text = GetBaiduShareCode();
		}

		private string GetBaiduShareCode()
		{
			StringBuilder sbBaiduShare = new StringBuilder();

			sbBaiduShare.AppendLine("<script type=\"text/javascript\" id=\"bdshare_js\" data=\"type=slide&amp;img=3&amp;pos=left&amp;uid=653519\" ></script>");
			sbBaiduShare.AppendLine("	<script type=\"text/javascript\" id=\"bdshell_js\"></script>");
			sbBaiduShare.AppendLine("	<script type=\"text/javascript\">");
			sbBaiduShare.AppendLine("		var bds_config = { \"bdTop\": 255 };");
			sbBaiduShare.AppendLine("		var bdscript = document.getElementById(\"bdshell_js\");");
			sbBaiduShare.AppendLine("		var bdscriptloaded = 0;");
			sbBaiduShare.AppendLine("		bdscript.onload = bdscript.onreadystatechange = function () {");
			sbBaiduShare.AppendLine("			if (bdscriptloaded) {");
			sbBaiduShare.AppendLine("				return");
			sbBaiduShare.AppendLine("			}");
			sbBaiduShare.AppendLine("			var a = bdscript.readyState;");
			sbBaiduShare.AppendLine("			if (\"undefined\" == typeof a || a == \"loaded\" || a == \"complete\") {");
			sbBaiduShare.AppendLine("				bdscriptloaded = 1;");
			sbBaiduShare.AppendLine("				var inter = setInterval(function () {");
			sbBaiduShare.AppendLine("					var share = document.getElementById(\"bdshare\");");
			sbBaiduShare.AppendLine("					if (share && share.tagName.toUpperCase() == \"DIV\") {");
			sbBaiduShare.AppendLine("						share.style.width = \"24px\";");
			sbBaiduShare.AppendLine("						clearInterval(inter);");
			sbBaiduShare.AppendLine("					}");
			sbBaiduShare.AppendLine("				}, 1000);");
			sbBaiduShare.AppendLine("				bdscript.onload = bdscript.onreadystatechange = null;");
			sbBaiduShare.AppendLine("			}");
			sbBaiduShare.AppendLine("		};");
			sbBaiduShare.AppendLine("		bdscript.src = \"http://bdimg.share.baidu.com/static/js/shell_v2.js?cdnversion=\" + new Date().getHours();");
			sbBaiduShare.AppendLine("");
			sbBaiduShare.AppendLine("	</script>");
			return sbBaiduShare.ToString();
		}

		/// <summary>
		/// 获取参数
		/// </summary>
		private void GetParameters()
		{
			selectParas = base.GetSelectCarParas();
			//价格
			string tmpStr = Request.QueryString["p"];
			if (!String.IsNullOrEmpty(tmpStr))
			{
				string adCode = "";
				switch (tmpStr)
				{
					case "0-5":
						adCode = "3a0979d3-55fd-4e02-8b03-04261331d66e";
						break;
					case "5-8":
						adCode = "3c9428d5-4a66-4498-8495-9d5c31dcbd82";
						break;
					case "8-12":
						adCode = "3d0adf56-ee18-4e25-be4f-3bd9dd44e267";
						break;
					case "12-18":
						adCode = "7ba70bf4-c64f-472c-a721-61917b6f5c9a";
						break;
					case "18-25":
						adCode = "d408c918-77c5-46f2-9a10-7ff223ebebd6";
						break;
					case "25-40":
						adCode = "8325bd58-15b9-4f9f-8b40-6f995e1dec56";
						break;
					case "40-80":
						adCode = "b7092fe8-1ba1-4357-889e-c769c2e3690a";
						break;
					case "80-9999":
						adCode = "a9b6f394-2d87-4bad-a0cc-7acafc6ab046";
						break;
				}
				// 广告
				if (adCode != "" && selectParas.PriceFlag != 0)
				{
					ADTopHtml = "<ins style=\"margin: 0 0; float: left;\" id=\"Ad_Cartree_SelectCar_" + adCode + "\" type=\"ad_play\" adplay_IP=\"\" adplay_AreaName=\"\"  adplay_CityName=\"\"    adplay_BrandID=\"\"  adplay_BrandName=\"\"  adplay_BrandType=\"\"  adplay_BlockCode=\"" + adCode + "\" ></ins>";
				}

			}

			//页面标题
			pageTitle = "【选车工具|选车中心_汽车车型大全";
			if (selectParas.ConditionString.Length > 0)
				pageTitle += ":" + selectParas.ConditionString;
			pageTitle += "】-易车网";

			// add by chengl Jun.20.2012
			metaKeywords = "选车,选车工具,易车网";
			metaDescription = "选车工具:易车网车型频道为您提供按条件选车工具,包括按汽车价格、汽车级别、国产进口、变速方式、汽车排量等方式选择适合您的喜欢的汽车品牌……";
			if (ConvertHelper.GetInteger(Request.QueryString["l"]) > 0
				&& selectParas.BodyForm <= 0 && selectParas.BrandType <= 0
				&& selectParas.CarConfig <= 0 && selectParas.ComfortableConfig <= 0
				&& selectParas.Country <= 0 && selectParas.MaxDis <= 0.0
				&& selectParas.MaxPrice <= 0 && selectParas.MaxReferPrice <= 0
				&& selectParas.MinDis <= 0.0 && selectParas.MinPrice <= 0
				&& selectParas.MinReferPrice <= 0 && selectParas.Purpose <= 0
				&& selectParas.SafetyConfig <= 0 && selectParas.TransmissionType <= 0
				&& selectParas.MinFuel <= 0.0 && selectParas.MaxFuel <= 0.0)
			{
				// 当只选了级别 其他没选的情况 将Title Keywords Description 重新设置到老的级别
				ReSetTitleForLevel();
			}
			// end

			//显示模式 modify by sk 2012-06-12
			displayMode = "BigImage";
			//displayMode = Request.QueryString["v"];
			//if (String.IsNullOrEmpty(displayMode))
			//    displayMode = "0";
			//if (displayMode == "0")
			//{
			//    displayMode = "BigImage";
			//    displayModeHtml = "<a href=\"javascript:GotoPage('v1');\" class=\"lb\">切换到列表</a>";
			//}
			//else
			//{
			//    displayMode = "List";
			//    displayModeHtml = "<a href=\"javascript:GotoPage('v0');\" class=\"dt\">切换到大图</a>";
			//}
			//排序
			sortMode = Request.QueryString["s"];
			if (sortMode == "1" || ConvertHelper.GetInteger(sortMode) > 4)
			{
				//sortMode = "guanzhu_up";
				sortModeHtml = "<a href=\"javascript:GotoPage('s0');\" class=\"" + (ConvertHelper.GetInteger(sortMode) > 4 ? "downarrow" : "uparrow") + "\" data-channelid=\"2.116.1292\">" + (ConvertHelper.GetInteger(sortMode) > 4 ? "按关注" : "<em>按关注</em>") + "</a> | <a href=\"javascript:GotoPage('s2');\" class=\"uparrow\" data-channelid=\"2.116.1293\">按价格</a>";
			}
			else if (sortMode == "2")
			{
				//sortMode = "price_up";
				sortModeHtml = "<a href=\"javascript:GotoPage('s0');\" class=\"downarrow\" data-channelid=\"2.116.1292\">按关注</a> | <a href=\"javascript:GotoPage('s3');\" class=\"uparrow\" data-channelid=\"2.116.1293\"><em>按价格</em></a>";
			}
			else if (sortMode == "3")
			{
				//sortMode = "price_down";
				sortModeHtml = "<a href=\"javascript:GotoPage('s0');\" class=\"downarrow\" data-channelid=\"2.116.1292\">按关注</a> | <a href=\"javascript:GotoPage('s2');\" class=\"downarrow\" data-channelid=\"2.116.1293\"><em>按价格</em></a>";
			}
			else
			{
				//sortMode = "guanzhu_down";
				sortModeHtml = "<a href=\"javascript:GotoPage('s1');\" class=\"downarrow\" data-channelid=\"2.116.1292\"><em>按关注</em></a> | <a href=\"javascript:GotoPage('s2');\" class=\"uparrow\" data-channelid=\"2.116.1293\">按价格</a>";
			}

			//页号
			tmpStr = Request.QueryString["page"];
			bool isPage = Int32.TryParse(tmpStr, out pageNum);
			if (!isPage)
				pageNum = 1;
			pageSize = 20;
		}

		private void SelectCar()
		{
			List<BitAuto.CarChannel.BLL.SerialInfo> tmpList = new SelectCarToolBll().SelectCarByParameters(selectParas);
			//Copy一个复本，以免排序时影响原其他线程
			List<BitAuto.CarChannel.BLL.SerialInfo> serialList = tmpList.GetRange(0, tmpList.Count);

			//排序
			if (sortMode == "guanzhu_down")
			{
				serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByPVDesc);
			}
			else if (sortMode == "guanzhu_up")
				serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerial);
			else if (sortMode == "price_up")
				serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinPrice);
			else if (sortMode == "price_down")
				serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMaxPriceDesc);
			SerialNum = serialList.Count;
			CarNum = 0;
			StringBuilder htmlCode = new StringBuilder();
			if (SerialNum > 0)
			{
				//modified by sk 2013.10.23 广告 
				List<SerialListADEntity> listSerialAD = new Car_SerialBll().GetSerialAD("selectcar" + selectParas.ConditionString);
				if (selectParas.BodyForm == 0 && selectParas.CarConfig == 0 && listSerialAD != null && listSerialAD.Count > 0)
				{
					foreach (SerialListADEntity serialAd in listSerialAD)
					{
						int index = serialAd.Pos - 1;
						if (index < 0)
							index = 0;
						SerialInfo serialInfo = serialList.Find((p) => { return p.SerialId == serialAd.SerialId; });
						if (serialInfo != null)
						{
							serialList.Remove(serialInfo);
							serialInfo.ADSerialUrl = serialAd.Url;
							serialList.Insert(index, serialInfo);
						}
					}
				}
				if (displayMode == "BigImage")
					htmlCode.AppendLine(MakeImageModeHtmlV2(serialList));
				else
					htmlCode.AppendLine(MakeListModeHtml(serialList));
				MakePageNavHtml(SerialNum);
				htmlCode.Insert(0, MakeSelectBarHtml());
				serialListHtml = htmlCode.ToString();
			}
			else
			{
				levelIsShow = "none";
				serialListHtml = "<div class=\"title-con\" style=\"z-index:1\"><div class=\"title-box title-box2\"><h4>车型列表</h4></div></div><div class=\"no-txt-box\"><p class=\"tit\">抱歉，未找到合适的车型</p><p>请修改条件再次查询，或者去 <a href=\"http://www.taoche.com/all/\" target=\"_blank\">易车二手车</a> 看看</p></div>";
			}
		}

		/// <summary>
		/// 生成搜过栏的Html
		/// </summary>
		/// <returns></returns>
		private string MakeSelectBarHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.Append("<div class=\"title-con\">");
			htmlCode.Append("<div class=\"title-box title-box2\">");
			htmlCode.AppendFormat("<h4>选新车</h4><span id= \"styleTotal\"></span>");
			if (!string.IsNullOrEmpty(ShowLevelName))
			{
				htmlCode.Append("<span class=\"tc-whatcar\"  onmouseover=\"document.getElementById('car_leveldesc').style.display='';\" onmouseout=\"document.getElementById('car_leveldesc').style.display='none';\">&nbsp;<a href=\"javascript:;\">" + ShowLevelName + "</a>");
				htmlCode.AppendFormat("<div id=\"car_leveldesc\" class=\"tc tc-shenmeche\" style=\"display:none;\"><div class=\"tc-box\"><i></i><strong>{0}</strong><p>{1}</p></div></div></span>", LevelName, LevelDesc);
			}
			if (LevelName == "SUV")
			{
				htmlCode.AppendFormat("<span class=\"tc-goto\">&nbsp;<a target=\"_blank\" href=\"/suv/all/\">进入SUV频道>></a></span>");
			}
			htmlCode.AppendFormat("<div class=\"more\">");
			htmlCode.AppendFormat("<div class=\"kb-px-box\">");
			string koubeiTitle = string.Empty;
			switch (sortMode)
			{
				case "5":
				case "6": koubeiTitle = "综合"; break;
				case "7":
				case "8": koubeiTitle = "空间"; break;
				case "9":
				case "10": koubeiTitle = "动力"; break;
				case "11":
				case "12": koubeiTitle = "操控"; break;
				case "13":
				case "14": koubeiTitle = "配置"; break;
				case "15":
				case "16": koubeiTitle = "舒适度"; break;
				case "17":
				case "18": koubeiTitle = "性价比"; break;
				case "19":
				case "20": koubeiTitle = "外观"; break;
				case "21":
				case "22": koubeiTitle = "内饰"; break;
				case "23":
				case "24": koubeiTitle = "油耗"; break;
				default: koubeiTitle = "按口碑评分"; break;
			}
			htmlCode.AppendFormat("<a href=\"#\" id=\"c_koubei\" class=\" " + (ConvertHelper.GetInteger(sortMode) > 4 ? (ConvertHelper.GetInteger(sortMode) % 2 == 0 ? "current curt-hover down-currt" : "current curt-hover up-currt") : "") + " hover1\">" + koubeiTitle + "<em></em><strong></strong></a>");
			htmlCode.AppendFormat("<div class=\"more kb-px-b hover0\" style=\"display:none\">");
			htmlCode.AppendFormat("<ul>");
			htmlCode.AppendFormat("<li><a href=\"javascript:" + (sortMode == "6" ? "GotoPage('s5')" : "GotoPage('s6')") + "\"  class=\"" + (sortMode == "6" ? "current-uparrow" : (sortMode == "5" ? "current-downarrow" : "downarrow")) + "\" data-channelid=\"2.116.1304\"><em>综合</em></a></li>");
			htmlCode.AppendFormat("<li><a href=\"javascript:" + (sortMode == "24" ? "GotoPage('s23')" : "GotoPage('s24')") + "\"  class=\"" + (sortMode == "24" ? "current-uparrow" : (sortMode == "23" ? "current-downarrow" : "downarrow")) + "\" data-channelid=\"2.116.1296\"><em>油耗</em></a></li>");
			htmlCode.AppendFormat("<li><a href=\"javascript:" + (sortMode == "10" ? "GotoPage('s9')" : "GotoPage('s10')") + "\"   class=\"" + (sortMode == "10" ? "current-uparrow" : (sortMode == "9" ? "current-downarrow" : "downarrow")) + "\" data-channelid=\"2.116.1295\"><em>动力</em></a></li>");
			htmlCode.AppendFormat("<li><a href=\"javascript:" + (sortMode == "18" ? "GotoPage('s17')" : "GotoPage('s18')") + "\"  class=\"" + (sortMode == "18" ? "current-uparrow" : (sortMode == "17" ? "current-downarrow" : "downarrow")) + "\" data-channelid=\"2.116.1297\"><em>性价比</em></a></li>");
			htmlCode.AppendFormat("<li><a href=\"javascript:" + (sortMode == "14" ? "GotoPage('s13')" : "GotoPage('s14')") + "\"  class=\"" + (sortMode == "14" ? "current-uparrow" : (sortMode == "13" ? "current-downarrow" : "downarrow")) + "\" data-channelid=\"2.116.1298\"><em>配置</em></a></li>");
			htmlCode.AppendFormat("<li><a href=\"javascript:" + (sortMode == "12" ? "GotoPage('s11')" : "GotoPage('s12')") + "\"  class=\"" + (sortMode == "12" ? "current-uparrow" : (sortMode == "11" ? "current-downarrow" : "downarrow")) + "\" data-channelid=\"2.116.1299\"><em>操控</em></a></li>");
			htmlCode.AppendFormat("<li><a href=\"javascript:" + (sortMode == "8" ? "GotoPage('s7')" : "GotoPage('s8')") + "\"  class=\"" + (sortMode == "8" ? "current-uparrow" : (sortMode == "7" ? "current-downarrow" : "downarrow")) + "\" data-channelid=\"2.116.1300\"><em>空间</em></a></li>");
			htmlCode.AppendFormat("<li><a href=\"javascript:" + (sortMode == "20" ? "GotoPage('s19')" : "GotoPage('s20')") + "\"  class=\"" + (sortMode == "20" ? "current-uparrow" : (sortMode == "19" ? "current-downarrow" : "downarrow")) + "\" data-channelid=\"2.116.1301\"><em>外观</em></a></li>");
			htmlCode.AppendFormat("<li><a href=\"javascript:" + (sortMode == "16" ? "GotoPage('s15')" : "GotoPage('s16')") + "\"  class=\"" + (sortMode == "16" ? "current-uparrow" : (sortMode == "15" ? "current-downarrow" : "downarrow")) + "\" data-channelid=\"2.116.1302\"><em>舒适度</em></a></li>");
			htmlCode.AppendFormat("<li><a href=\"javascript:" + (sortMode == "22" ? "GotoPage('s21')" : "GotoPage('s22')") + "\"  class=\"" + (sortMode == "22" ? "current-uparrow" : (sortMode == "21" ? "current-downarrow" : "downarrow")) + "\" data-channelid=\"2.116.1303\"><em>内饰</em></a></li>");
			htmlCode.AppendFormat("</ul>");
			htmlCode.AppendFormat("</div>");
			htmlCode.AppendFormat("</div> | ");
			htmlCode.AppendFormat("{0}", sortModeHtml);
			htmlCode.AppendFormat("</div>");
			htmlCode.Append("</div>");
			htmlCode.Append("</div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 以大图方式显示选车结果
		/// </summary>
		/// <param name="serilaList"></param>
		//private string MakeImageModeHtml(List<BitAuto.CarChannel.BLL.SerialInfo> serilaList)
		//{
		//    int startIndex = (pageNum - 1) * pageSize + 1;
		//    int endIndex = startIndex + pageSize - 1;

		//    StringBuilder htmlCode = new StringBuilder();
		//    htmlCode.AppendLine("<div class=\"c0621_03 c0623_02\">");
		//    htmlCode.AppendLine("<ul>");
		//    int counter = 0;
		//    foreach (BitAuto.CarChannel.BLL.SerialInfo info in serilaList)
		//    {
		//        counter++;
		//        CarNum += info.CarNum;
		//        if (counter < startIndex || counter > endIndex)
		//            continue;
		//        string serialUrl = "http://car.bitauto.com/" + info.AllSpell + "/";
		//        if (!string.IsNullOrEmpty(info.ADSerialUrl))
		//            serialUrl = info.ADSerialUrl;
		//        htmlCode.AppendLine("<li>");
		//        htmlCode.Append("<a target=\"_blank\" href=\"" + serialUrl + "\">");
		//        string shortName = info.ShowName.Replace("(进口)", "");
		//        if (info.SerialId == 1568)
		//        {
		//            shortName = "索纳塔八";
		//        }
		//        htmlCode.Append("<img width=\"120\" height=\"80\" alt=\"" + info.ShowName + "\" src=\"" + info.ImageUrl + "\"></a>");
		//        htmlCode.AppendLine("<a target=\"_blank\" href=\"" + serialUrl + "\" title=\"" + info.ShowName + "\">" + shortName + "</a>");
		//        htmlCode.AppendLine("<br/><span>" + info.PriceRange + "</span>");
		//        //htmlCode.AppendLine("<p><a href=\"\">图片</a> <a href=\"\">口碑</a> <a href=\"\">答疑</a></p>");
		//        htmlCode.AppendLine("</li>");
		//    }
		//    htmlCode.AppendLine("</ul>");
		//    htmlCode.AppendLine("</div>");
		//    return htmlCode.ToString();
		//}
		private string MakeImageModeHtmlV2(List<BitAuto.CarChannel.BLL.SerialInfo> serilaList)
		{
			int startIndex = (pageNum - 1) * pageSize + 1;
			int endIndex = startIndex + pageSize - 1;

			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<div class=\"carpic_list\"><ul>");
			int counter = 0;
			int currentPageCount = 0;
			int currentLineCount = 0;
			foreach (BitAuto.CarChannel.BLL.SerialInfo info in serilaList)
			{
				counter++;
				CarNum += info.CarNum;
				if (counter < startIndex || counter > endIndex)
					continue;
				currentPageCount++;

				string serialUrl = "http://car.bitauto.com/" + info.AllSpell + "/";
				if (!string.IsNullOrEmpty(info.ADSerialUrl))
					serialUrl = info.ADSerialUrl;
				htmlCode.AppendLine("<li>");
				htmlCode.Append("<a target=\"_blank\" href=\"" + serialUrl + "\">");
				string shortName = info.ShowName;
				// string shortName = info.ShowName.Replace("(进口)", "");
				if (info.SerialId == 1568)
				{
					shortName = "索纳塔八";
				}
				htmlCode.Append("<img alt=\"" + info.ShowName + "\" src=\"" + info.ImageUrl.Replace("_2.", "_1.") + "\"></a>");
				htmlCode.AppendLine("<div class=\"title\"><a target=\"_blank\" href=\"" + serialUrl + "\" title=\"" + info.ShowName + "\">" + shortName + "</a></div>");
				htmlCode.AppendFormat("<div class=\"txt {1}\">{0}</div>",
					info.PriceRange != "暂无报价" ? info.PriceRange : (info.MinReferPrice > 0 ? "指导价：" + info.MinReferPrice + "万起" : "暂无报价"),
					info.PriceRange == "暂无报价" ? "huizi" : "");
				htmlCode.AppendFormat("<div class=\"seach_more\" bit-seachmore><a href=\"javascript:;\" bit-serial=\"{1}\" bit-car=\"{2}\" bit-line=\"{3}\" bit-allspell=\"{4}\" class=\"sub-color\">{0}个车款符合条件</a></div>", info.CarNum, info.SerialId, info.CarIdList, currentLineCount, info.AllSpell);
				//htmlCode.AppendFormat("<div class=\"ico-arrow\"></div>");
				htmlCode.AppendLine("</li>");
				if (currentPageCount % 4 == 0 || counter == serilaList.Count)
				{
					currentLineCount++;
					htmlCode.AppendFormat("<li class=\"c-list-2014 c-list-2014-pop\"  bit-line=\"{0}\"></li>", currentLineCount);
				}
			}
			htmlCode.AppendLine("</ul></div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 以列表方式显示选车结果
		/// </summary>
		/// <param name="serilaList"></param>
		private string MakeListModeHtml(List<BitAuto.CarChannel.BLL.SerialInfo> serilaList)
		{
			int startIndex = (pageNum - 1) * pageSize + 1;
			int endIndex = startIndex + pageSize - 1;

			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<div class=\"l\">");
			int counter = 0;
			foreach (BitAuto.CarChannel.BLL.SerialInfo info in serilaList)
			{
				counter++;
				CarNum += info.CarNum;
				if (counter < startIndex || counter > endIndex)
					continue;

				string serialReferPrice = "";		//指导价
				string serialExhaust = "";			//排量
				string serialTransmission = "";		//变速器类型
				List<EnumCollection.CarInfoForSerialSummary> ls = base.GetAllCarInfoForSerialSummaryByCsID(info.SerialId);
				double maxPrice = Double.MinValue;
				double minPrice = Double.MaxValue;
				foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
				{
					double referPrice = 0.0;
					bool isDouble = Double.TryParse(carInfo.ReferPrice.Replace("万", ""), out referPrice);
					if (isDouble)
					{
						if (referPrice > maxPrice)
							maxPrice = referPrice;
						if (referPrice < minPrice)
							minPrice = referPrice;
					}
				}
				if (maxPrice == Double.MinValue && minPrice == Double.MaxValue)
					serialReferPrice = "暂无";
				else
				{
					serialReferPrice = minPrice + "万-" + maxPrice + "万";
				}

				EnumCollection.SerialInfoCard sic = new Car_SerialBll().GetSerialInfoCard(info.SerialId);
				serialExhaust = CommonFunction.GetExhaust(sic.CsEngineExhaust);
				serialTransmission = CommonFunction.GetTransmission(sic.CsTransmissionType);

				string colorHtml = "";
				string allColorHtml = "";
				foreach (string colorStr in sic.ColorList)
				{
					if (allColorHtml.Length > 0)
						allColorHtml += "　";
					allColorHtml += colorStr;
				}
				if (sic.ColorList.Count > 3)
					colorHtml = sic.ColorList[0] + "　" + sic.ColorList[1] + "　…　" + sic.ColorList[sic.ColorList.Count - 1];
				else
					colorHtml = allColorHtml;

				string serialUrl = "http://car.bitauto.com/" + info.AllSpell + "/";
				string showName = info.ShowName;
				if (info.SerialId == 1568)
				{
					showName = "索纳塔八";
				}

				htmlCode.AppendLine("<dl>");
				htmlCode.AppendLine("<dt><span><a href=\"" + serialUrl + "\" target=\"_blank\">" + showName + "</a></span><em><a href=\"" + serialUrl + "\" target=\"_blank\">详情查看 <span>" + showName + "</span> 频道&gt;&gt;</a></em></dt>");
				htmlCode.AppendLine("<dd>");
				htmlCode.AppendLine("<div class=\"pic\"><a target=\"_blank\" href=\"" + serialUrl + "\"><img width=\"120\" height=\"80\" alt=\"" + info.ShowName + "\" src=\"" + info.ImageUrl + "\"></a></div>");
				htmlCode.AppendLine("<ul>");
				htmlCode.AppendLine("<li><label>商家报价：</label><strong>" + info.PriceRange + "</strong></li>");
				htmlCode.AppendLine("<li><label>排量：</label>" + serialExhaust + "</li>");
				htmlCode.AppendLine("<li><label>厂家指导价：</label>" + serialReferPrice + "</li>");
				htmlCode.AppendLine("<li><label>变速箱：</label>" + serialTransmission + "</li>");
				htmlCode.AppendLine("<li><label>颜色：</label>" + colorHtml + "</li>");
				if (sic.CsSummaryFuelCost.Length > 0)
					htmlCode.AppendLine("<li><label>综合工况油耗：</label>" + sic.CsSummaryFuelCost + "</li>");
				else
					htmlCode.AppendLine("<li><label>官方油耗：</label>" + sic.CsOfficialFuelCost + "</li>");

				htmlCode.AppendLine("<li><label>保修：</label>" + sic.SerialRepairPolicy + "</li>");
				htmlCode.AppendLine("<li><label>网友发布：</label><strong>" + sic.CsGuestFuelCost + "</strong></li>");
				htmlCode.AppendLine("</ul>");
				htmlCode.AppendLine("</dd>");
				htmlCode.AppendLine("</dl>");
			}
			htmlCode.AppendLine("</div>");
			return htmlCode.ToString();
		}

		private void MakePageNavHtml(int serialCount)
		{
			int pageCount = serialCount / pageSize + (serialCount % pageSize == 0 ? 0 : 1);

			if (pageNum > pageCount)
				pageNum = pageCount;

			//生成页号导航
			if (pageCount > 1)
			{
				string baseUrl = _SearchUrl;
				string queryString = Request.Url.Query.ToLower();
				int pos = queryString.IndexOf("page=");
				if (pos > -1)
				{
					queryString = queryString.Substring(0, pos);
					queryString = queryString.TrimEnd('&');
				}
				//解决 级别页重写地址重写 by sk 2012.06.14
				if (queryString.IndexOf("l=") != -1)
				{
					NameValueCollection nvc = HttpUtility.ParseQueryString(queryString.TrimStart('?'));
					string rewrite = "";
					if (Request.Headers["X-Rewrite-URL"] != null)
					{
						rewrite = Request.Headers["X-Rewrite-URL"];
					}
					else if (Request.Headers["X-Original-URL"] != null)
					{
						rewrite = Request.Headers["X-Original-URL"];
					}
					//Request.Headers["X-Rewrite-URL"] == null ? Request.Headers["X-Original-URL"] : Request.Headers["X-Rewrite-URL"];
					if (nvc.Count == 1 && Array.IndexOf(nvc.AllKeys, "l") != -1 && nvc["l"] != "63" && rewrite.IndexOf("xuanchegongju") == -1 && rewrite.IndexOf("tree_chexing") == -1)
					{
						nvc.Remove("l");
						queryString = nvc.ToString();
					}
				}
				if (queryString == "?")
					queryString = "";
				if (queryString.Length > 0)
					baseUrl += queryString + "&";
				else
					baseUrl += "?";

				baseUrl += "page={0}";

				//设置分页控件
				//this.AspNetPager1.PageSize = pageSize;
				//this.AspNetPager1.RecordCount = serialCount;
				//this.AspNetPager1.CurrentPageIndex = pageNum;
				//this.AspNetPager1.UrlRewritePattern = baseUrl;
				//this.AspNetPager1.Visible = true;
				//this.AspNetPager1.ExternalConfigPattern = BitAuto.Controls.Pager.PagerExternalConfigPattern.Apply;
				//this.AspNetPager1.ExternalConfigURL = Server.MapPath("~/config/PagerConfig.xml");

			}
		}

		private string MakeFilterHtml()
		{
			List<string> htmlList = new List<string>();
			/*
			htmlList.Add("品牌：<select id=\"selBrandType\" onchange=\"SelectFilterCondition('brandtype');\"><option value=\"0\">不限</option>");
			if (selectParas.BrandType == 1)
				htmlList.Add("<option value=\"1\" selected style=\"color:#c00\">自主</option>");
			else
				htmlList.Add("<option value=\"1\">自主</option>");

			if (selectParas.BrandType == 2)
				htmlList.Add("<option value=\"2\" selected style=\"color:#c00\">合资</option>");
			else
				htmlList.Add("<option value=\"2\">合资</option>");

			if (selectParas.BrandType == 4)
				htmlList.Add("<option value=\"4\" selected style=\"color:#c00\">进口</option>");
			else
				htmlList.Add("<option value=\"4\">进口</option>");
			htmlList.Add("</select>");
			 */
			//当级别选了SUV,MPV，跑车等级别时不显示箱示过滤条件 modify by sk 2012-06-12
			//int paraLevel = ConvertHelper.GetInteger(Request.QueryString["l"]);
			//if (paraLevel < 7 || paraLevel == 63)
			//{
			//    htmlList.Add("　厢体：<select id=\"selBodyForm\" onchange=\"SelectFilterCondition('bodyform');\"><option value=\"0\">不限</option>");
			//    if (selectParas.BodyForm == 1)
			//        htmlList.Add("<option value=\"1\" selected style=\"color:#c00\">两厢及掀背</option>");
			//    else
			//        htmlList.Add("<option value=\"1\">两厢及掀背</option>");

			//    if (selectParas.BodyForm == 2)
			//        htmlList.Add("<option value=\"2\" selected style=\"color:#c00\">三厢</option>");
			//    else
			//        htmlList.Add("<option value=\"2\">三厢</option>");
			//    htmlList.Add("</select>");
			//}
			return String.Concat(htmlList.ToArray());
		}

		private void MakeHotSerialHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			if (selectParas.PriceFlag > 0)
			{
				//获取数据xml
				XmlDocument mbDoc = AutoStorageService.GetAutoXml();

				//遍历所有子品牌节点
				XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial[contains(@MultiPriceRange,\"," + selectParas.PriceFlag + ",\")]");

				TopPVSerialSelector tpvSelector = new TopPVSerialSelector(10);
				tpvSelector.SelectNewCar = true;
				tpvSelector.NewCarNum = 4;
				foreach (XmlElement serialNode in serialNodeList)
				{
					tpvSelector.AddSerial(serialNode);
				}
				List<XmlElement> serialList = tpvSelector.GetNewCarList();
				htmlCode.Append("<div class=\"line-box\">");
				htmlCode.Append("<div class=\"title-con\">");
				htmlCode.Append("<div class=\"title-box title-box2\">");
				htmlCode.Append("<h4>");
				htmlCode.Append("<a href=\"javascript:;\">热门新车</a></h4>");
				htmlCode.Append("</div>");
				htmlCode.Append("</div>");
				htmlCode.AppendLine("<div class=\"carpic_list\">");
				htmlCode.AppendLine("<ul>");

				foreach (XmlElement serialNode in serialList)
				{
					htmlCode.AppendLine("<li>");
					int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));

					string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");
					string serialName = serialNode.GetAttribute("ShowName");
					string shortName = serialName.Replace("(进口)", "");
					string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();
					string serialUrl = "http://car.bitauto.com/" + serialSpell + "/";

					htmlCode.Append("<a href=\"" + serialUrl + "\" target=\"_blank\">");
					htmlCode.AppendLine("<img src=\"" + imgUrl + "\" alt=\"" + serialName + "\" /></a>");
					htmlCode.Append("<div class=\"title\"><a href=\"" + serialUrl + "\" target=\"_blank\" title=\"" + serialName + "\">" + shortName + "</a></div>");
					string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(serialId));
					if (priceRange.Trim().Length == 0)
						htmlCode.AppendLine("<div class=\"txt huizi\">暂无报价</div>");
					else
						htmlCode.AppendLine("<div class=\"txt\">" + priceRange + "</div>");
					htmlCode.AppendLine("</li>");
				}

				htmlCode.AppendLine("</ul>");
				htmlCode.AppendLine("</div>");
				htmlCode.Append("<div class=\"clear\">");
				htmlCode.Append("</div>");
				htmlCode.Append("</div>");

			}
			hotSerialHtml = htmlCode.ToString();
		}

		protected NameValueCollection GetAdCode(string pName, string condition)
		{
			NameValueCollection nvc = new NameValueCollection();
			string cacheLevelKey = "BITA_CAR_AD_" + condition;
			string filePath = string.Format(Path.Combine(Server.MapPath("~"), "App_Data\\ad\\{0}.xml"), pName);
			if (!File.Exists(filePath))
				return nvc;
			try
			{
				XmlDocument cacheData = CacheManager.GetCachedData(cacheLevelKey) as XmlDocument;
				if (cacheData == null)
				{
					cacheData = new XmlDocument();
					cacheData.Load(filePath);
					CacheManager.InsertCache(cacheLevelKey, cacheData, new CacheDependency(filePath), DateTime.Now.AddDays(5));
				}
				XmlNode root = cacheData.DocumentElement;
				//XmlNode node = root.SelectSingleNode("//adcode[@pos='" + adpos + "']");
				XmlNodeList nodeList = root.SelectNodes("//adcode");
				foreach (XmlNode node in nodeList)
				{
					XmlNode nodeCode = node.SelectSingleNode("./code[@condition='" + System.Security.SecurityElement.Escape(condition) + "']");
					if (nodeCode != null)
						nvc.Add(node.Attributes["pos"].Value, nodeCode.InnerText);
				}
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.Message + ex.StackTrace);
			}
			return nvc;
		}

		private void InitLevelDesc()
		{
			int level = ConvertHelper.GetInteger(Request.QueryString["l"]);
			string cacheLevelKey = "BITA_CAR_LEVEL_DESC";
			if (level > 0)
			{
				string filePath = Path.Combine(Server.MapPath("~"), "App_Data\\LevelDesc.xml");
				if (!File.Exists(filePath))
					return;
				try
				{
					XmlDocument cacheData = CacheManager.GetCachedData(cacheLevelKey) as XmlDocument;
					if (cacheData == null)
					{
						cacheData = new XmlDocument();
						cacheData.Load(filePath);
						CacheManager.InsertCache(cacheLevelKey, cacheData, new CacheDependency(filePath), DateTime.Now.AddDays(1));
					}
					XmlNode root = cacheData.DocumentElement;
					XmlNode node = root.SelectSingleNode("//item[@id='" + level + "']");
					if (node != null)
					{
						LevelDesc = node.InnerText;
						LevelName = node.Attributes["name"].Value;
						if (level == 10)
							ShowLevelName = "其他车型";
						else
							ShowLevelName = "什么是" + LevelName + "？";
					}
				}
				catch (Exception ex)
				{
					CommonFunction.WriteLog(ex.ToString());
				}
			}
		}

		/// <summary>
		/// 级别页重新设置 Title Keywords Description
		/// </summary>
		private void ReSetTitleForLevel()
		{
			switch (ConvertHelper.GetInteger(Request.QueryString["l"]))
			{
				case 1:
					pageTitle = "【微型车|微型车销量排行榜_微型车哪款好_最省油微型车】-易车网";
					metaKeywords = "微型车,微型车哪款好,微型车排行榜,最便宜的微型车,最省油的微型车,微型车图片,微型车报价";
					metaDescription = "微型车:易车网微型车频道提供最全面的微型车销量排行榜,最省油的微型车报价,图片,论坛等,微型车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				case 2:
					pageTitle = "【小型车|小型车销量排行榜_小型车哪款好_最省油小型车】-易车网";
					metaKeywords = "小型车,小型车哪款好,小型车排行榜,最便宜的小型车,最省油的小型车,小型车图片,小型车报价";
					metaDescription = "小型车:易车网小型车频道提供最全面的小型车销量排行榜,最省油的小型车报价,图片,论坛等,小型车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				case 3:
					pageTitle = "【紧凑型车|紧凑型车排行榜_紧凑型车哪款好_最省油紧凑型车】-易车网";
					metaKeywords = "紧凑型车,紧凑型车哪款好,紧凑型车排行榜,最便宜的紧凑型车,最省油的紧凑型车,紧凑型车图片,紧凑型车报价";
					metaDescription = "紧凑型车:易车网紧凑型车频道提供最全面的紧凑型车排行榜,最省油的紧凑型车报价,图片,论坛等,紧凑型车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				case 4:
					pageTitle = "【中大型车|中大型车排行榜_中大型车哪款好_最省油中大型车】-易车网";
					metaKeywords = "中大型车,中大型车哪款好,中大型车排行榜,最便宜的中大型车,最省油的中大型车,中大型车图片,中大型车报价";
					metaDescription = "中大型车:易车网中大型车频道提供最全面的中大型车排行榜,最省油的中大型车报价,图片,论坛等,中大型车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				case 5:
					pageTitle = "【中型车|中型车销量排行榜_中型车哪款好_最省油中型车】-易车网";
					metaKeywords = "中型车,中型车哪款好,中型车排行榜,最便宜的中型车,最省油的中型车,中型车图片,中型车报价";
					metaDescription = "中型车:易车网中型车频道提供最全面的中型车销量排行榜,最省油的中型车报价,图片,论坛等,中型车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				case 6:
					pageTitle = "【豪华车|豪华车销量排行榜_豪华车哪款好_豪华车品牌】-易车网";
					metaKeywords = "豪华车,豪华车哪款好,豪华车排行榜,最便宜的豪华车,最省油的豪华车,豪华车图片,豪华车报价";
					metaDescription = "豪华车:易车网豪华车频道提供最全面的豪华车排行榜,豪华车报价,图片,论坛等,豪华车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				case 7:
					pageTitle = "【MPV商务车|MPV报价_MPV排行榜推荐_商务车大全】-易车网";
					metaKeywords = "MPV,MPV大全,MPV报价,商务车,商务车哪款好,商务车排行榜,最便宜的商务车,最省油的商务车,商务车图片,商务车报价";
					metaDescription = "MPV商务车:易车网MPV商务车频道提供最全面的MPV商务车排行榜,MPV商务车报价,图片,论坛等,MPV商务车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				case 8:
					pageTitle = "【SUV越野车|SUV销量排行榜_越野车哪款好_SUV越野车大全】-易车网";
					metaKeywords = "SUV,SUV大全,SUV报价,越野车,越野车哪款好,越野车排行榜,最便宜的越野车,最省油的越野车,越野车图片,越野车报价";
					metaDescription = "SUV越野车:易车网SUV越野车频道提供最全面的SUV越野车排行榜,SUV越野车报价,图片,论坛等,SUV越野车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				case 9:
					pageTitle = "【跑车|跑车报价_跑车图片大全_跑车排行榜推荐】-易车网";
					metaKeywords = "跑车,跑车哪款好,跑车排行榜,最便宜的跑车,最省油的跑车,跑车图片,跑车报价";
					metaDescription = "跑车:易车网跑车频道提供最全面的跑车排行榜,跑车报价,图片,论坛等,跑车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				case 10:
					pageTitle = "【其他车|其他车报价大全_其他车排行榜推荐】-易车网";
					metaKeywords = "其他车,其他车报价,其他车哪款好,其他车排行榜,最便宜的其他车,其他车图片";
					metaDescription = "其他车:易车网其他车频道提供最全面的其他车排行榜,其他车报价,图片,论坛等,其他车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				case 11:
					pageTitle = "【面包车|面包车报价大全_面包车排行榜推荐】-易车网";
					metaKeywords = "面包车,面包车哪款好,面包车排行榜,最便宜的面包车,最省油的面包车,面包车图片,面包车报价";
					metaDescription = "面包车:易车网面包车频道提供最全面的面包车排行榜,面包车报价,图片,论坛等,面包车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				case 12:
					pageTitle = "【皮卡|皮卡车报价大全_皮卡车排行榜推荐】-易车网";
					metaKeywords = "皮卡,皮卡车报价,皮卡车哪款好,皮卡排行榜,最便宜的皮卡,皮卡车图片";
					metaDescription = "皮卡车:易车网皮卡车频道提供最全面的皮卡车排行榜,皮卡车报价,图片,论坛等,皮卡车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。";
					break;
				default: break;
			}
		}
	}
	public class SuperSerialInfo
	{
		private int m_pvNum;

		public SuperSerialInfo(int id, string showName, string spell)
		{
			SerialId = id;
			ShowName = showName;
			AllSpell = spell;
			//m_carIdList = ",";
		}

		public int Pos { get; set; }

		public int MasterId { get; set; }
		public int SerialId { get; set; }

		public string ShowName { get; set; }

		public string AllSpell { get; set; }

		public string CarIdList { get; set; }

		//public List<CarInfoForSerialSummaryEntity> CarList { get; set; }
		/// <summary>
		///     符合条件的车的数量
		/// </summary>
		public int CarNum { get; set; }

		public string ImageUrl { get; set; }

		public string PriceRange { get; set; }

	}
}