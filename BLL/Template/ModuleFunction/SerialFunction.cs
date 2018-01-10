using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Web;

using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL.Template.ModuleFunction
{
	/// <summary>
	/// 生成子品牌相关的页面代码
	/// </summary>
	public class SerialFunction
	{
		/// <summary>
		/// 生成子品牌综述页的头
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string GetHeaderForSummary(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			return new PageBase().GetCommonNavigation("CsSummary", se.Id);
		}

		/// <summary>
		/// 获取车型下拉列表的Html
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string GetCarListGroupByYear(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			Dictionary<string, string> yearHtmlDic = new Dictionary<string, string>();
			List<string> yearTypeList = new List<string>();
			string baseUrl = "/" + se.AllSpell + "/";
			foreach (CarEntity ce in se.CarList)
			{
				if (ce.SaleState == "停销")
					continue;

				string carUrl = baseUrl + "m" + ce.Id;
				string yearType = String.Empty;
				if (ce.CarYear == 0)
					yearType = "未知年款";
				else
				{
					yearType = ce.CarYear + "款";
					if (!yearTypeList.Contains(yearType))
						yearTypeList.Add(yearType);
				}


				if (!yearHtmlDic.ContainsKey(yearType))
					yearHtmlDic[yearType] = "<ul>";
				yearHtmlDic[yearType] += "<li><a href=\"" + carUrl + "\" >" + ce.Name + "</a></li>";
			}

			//年款排序
			yearTypeList.Sort(NodeCompare.CompareStringDesc);

			//年款下拉列表
			StringBuilder listCode = new StringBuilder(700);
			foreach (string yearType in yearTypeList)
			{
				listCode.Append("<h5><a href=\"" + baseUrl + yearType.Replace("款", "") + "/\" target=\"_self\" >" + yearType + "</a></h5>");
				listCode.Append(yearHtmlDic[yearType] + "</ul>");
			}
			string topCarListHtml = listCode.ToString();
			if (topCarListHtml.Length == 0)
			{
				if (yearHtmlDic.ContainsKey("未知年款"))
					topCarListHtml = yearHtmlDic["未知年款"] + "</ul>";
				else
					topCarListHtml = "";
			}

			return topCarListHtml;
		}

		/// <summary>
		/// 生成子品牌焦点图Html
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string GetFocusImageHtml(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			//获取数据
			StringBuilder htmlCode = new StringBuilder(2000);
			Car_SerialBll serialBll = new Car_SerialBll();
			List<SerialFocusImage> imgList = serialBll.GetSerialFocusImageList(se.Id);
			string bigImageCode = "";
			string smallImageCode = "";
			string imageLibUrl = "http://photo.bitauto.com/picture/" + se.Id + "/";

			if (imgList.Count > 0)
			{
				for (int i = 0; i < 3 && i < imgList.Count; i++)
				{
					SerialFocusImage csImg = imgList[i];
					string bigImgUrl = csImg.ImageUrl;
					string smallImgUrl = csImg.ImageUrl;
					if (csImg.ImageId > 0)
					{
						bigImgUrl = String.Format(bigImgUrl, 4);
						smallImgUrl = String.Format(smallImgUrl, 5);
					}
					if (i == 0)
					{
						bigImageCode += "<div id=\"focusBigImg_" + i + "\" style=\"display: block;\"><a href=\"" + csImg.TargetUrl + "\" target=\"_blank\"><img alt=\"" + se.Brand.MasterBrand.Name + se.Name + csImg.ImageName + "\" src=\"" + bigImgUrl + "\" width=\"300\" height=\"199\"></a> </div>";
						smallImageCode += "<li id=\"focusSmallImg_" + i + "\" class=\"current\"><a href=\"" + csImg.TargetUrl + "\" target=\"_blank\"><img alt=\"" + se.Brand.MasterBrand.Name + se.Name + csImg.ImageName + "\" src=\"" + smallImgUrl + "\"></a></li>";
					}
					else
					{
						bigImageCode += "<div id=\"focusBigImg_" + i + "\" style=\"display: none;\"><a href=\"" + csImg.TargetUrl + "\" target=\"_blank\"><img alt=\"" + se.Brand.MasterBrand.Name + se.Name + csImg.ImageName + "\" src=\"" + bigImgUrl + "\" width=\"300\" height=\"199\"></a> </div>";
						smallImageCode += "<li id=\"focusSmallImg_" + i + "\"><a href=\"" + csImg.TargetUrl + "\" target=\"_blank\"><img alt=\"" + se.Brand.MasterBrand.Name + se.Name + csImg.ImageName + "\" src=\"" + smallImgUrl + "\"></a></li>";
					}
				}
			}
			else
			{
				bigImageCode += "<div id=\"focusBigImg_0\" style=\"display: block;\"><img src=\"" + WebConfig.DefaultCarPic + "\" width=\"300\" height=\"199\"></div>";
				smallImageCode += "<li id=\"focusSmallImg_0\" class=\"current\"><img src=\"" + WebConfig.DefaultCarPic + "\"></li>";
			}
			//htmlCode.Append("<div class=\"focus_pics\" >");
			htmlCode.Append("<div class=\"lantern_pic\" id=\"lantern_pic\">");
			//三张大图
			htmlCode.Append(bigImageCode);
			htmlCode.Append("</div>");
			//三张小图
			htmlCode.Append("<ul id=\"lantern_list\" class=\"lantern_list\">");
			htmlCode.Append(smallImageCode);
			htmlCode.Append("</ul>");
			//htmlCode.Append("</div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成新车的子品牌概况HTML
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string GetSerialOveriewForNewCar(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.Append("<div class=\"sum_car_card\">");
			htmlCode.Append("<dl>");
			htmlCode.Append("<dt>" + se.Brand.MasterBrand.Name + se.Name + "</dt>");
			if (se.PicCount == 0)
				htmlCode.Append("<dd><a class=\"nolink\">图片(<span>0</span>张)</a></dd>");
			else
				htmlCode.Append("<dd><a href=\"/" + se.AllSpell + "/tupian/\">图片(<span>" + se.PicCount + "</span>张)</a></dd>");

			if (se.VideosCount == 0)
				htmlCode.Append("<dd><a class=\"nolink\">视频(<span>0</span>条)</a></dd>");
			else
				htmlCode.Append("<dd><a href=\"/" + se.AllSpell + "/shipin/\">视频(<span>" + se.VideosCount + "</span>条)</a></dd>");
			htmlCode.Append("</dl>");

			if (se.OfficialSite.Length > 0)
				htmlCode.Append("<a href=\"" + se.OfficialSite + "\" target=\"_blank\" class=\"more\">官方网站</a>");
			htmlCode.Append("</div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成子品牌概况HTML
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string GetSerialOverview(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder(1500);
			htmlCode.Append("<dl class=\"zs02\">");
			htmlCode.Append("<dt>" + se.Brand.MasterBrand.Name + se.Name + "</dt>");
			htmlCode.Append("<dd class=\"sublink\">");
			//// 2566:睿翼
			if (se.Id == 2566)
				htmlCode.Append("<a href=\"http://3d.bitauto.com/mazda/\" >3D展厅</a> ");

			if (se.OfficialSite.Length > 0)
				htmlCode.Append("<a href=\"" + se.OfficialSite + "\" >官方网站</a></dd>");

			htmlCode.Append("<dd>");
			htmlCode.Append("<ul class=\"d\">");
			htmlCode.Append("<li class=\"fl\"><a href=\"/" + se.Level.AllSpell + "/\" >" + se.Level.Name + "</a>");
			foreach (string purName in se.PurposeList)
			{
				string tagName = CommonFunction.GetPurposeTagName(purName);
				string purUrl = "/functionlist.html#" + tagName;
				htmlCode.Append("<a href=\"" + purUrl + "\" >" + purName + "</a>");
			}
			htmlCode.Append("</li>");

			// 停销子品牌显示全部颜色(文字形式)
			if (se.SaleState == "停销")
			{
				string colorHtml = "";
				string allColorHtml = "";
				foreach (string colorStr in se.Colors)
				{
					if (allColorHtml.Length > 0)
						allColorHtml += "　";
					allColorHtml += colorStr;
				}
				if (se.Colors.Length > 3)
				{
					if (se.Colors[0].Length + se.Colors[1].Length + se.Colors[se.Colors.Length - 1].Length > 16)
					{
						colorHtml = se.Colors[0] + "　…　" + se.Colors[se.Colors.Length - 1];
					}
					else
					{
						colorHtml = se.Colors[0] + "　" + se.Colors[1] + "　…　" + se.Colors[se.Colors.Length - 1];
					}
				}
				else
					colorHtml = allColorHtml;

				htmlCode.Append("<li><label>颜色：</label><span class=\"c\" title=\"" + allColorHtml + "\">");
				htmlCode.Append(colorHtml);
				htmlCode.Append("</span></li>");
			}
			else
			{
				// 子品牌颜色RGB
				string rgbHTML = "";
				string rgbTitle = "";
				new Car_SerialBll().GetSerialColorRGBByCsID(se.Id, 0, new List<string>(se.Colors), out rgbHTML, out rgbTitle);
				htmlCode.Append("<li><label>颜色：</label><span class=\"c\" title=\"" + rgbTitle + "\">");
				htmlCode.Append(rgbHTML);
				htmlCode.Append("</span></li>");
			}

			htmlCode.Append("<li><label>保修：</label>" + se.RepairPolicy + "</li>");
			htmlCode.Append("<li><span class=\"wy\">网友发布：<a href=\"/" + se.AllSpell + "/youhao/\" >" + se.GuestFuelCost + "</a></span>");


			if (se.SummaryFuelCost.Length > 0)
				htmlCode.Append("<label>综合工况油耗：</label>" + se.SummaryFuelCost + "</li>");
			else
				htmlCode.Append("<label>官方油耗：</label>" + se.OfficialFuelCost + "</li>");

			htmlCode.Append("</ul>");
			htmlCode.Append("<div class=\"h\">");
			if (se.PicCount == 0)
				htmlCode.Append("<a class=\"nolink\">图片(<span>0</span>张)</a>");
			else
				htmlCode.Append("<a href=\"/" + se.AllSpell + "/tupian/\" >图片(<span>" + se.PicCount + "</span>张)</a>");
			if (se.DianPingCount == 0)
				htmlCode.Append("<a class=\"nolink\">口碑(<span>0</span>条)</a>");
			else
				htmlCode.Append("<a href=\"/" + se.AllSpell + "/koubei/\" >口碑(<span>" + se.DianPingCount + "</span>条)</a>");
			if (se.AskCount == 0)
				htmlCode.Append("<a>问答(<span>0</span>条)</a>");
			else
				htmlCode.Append("<a href=\"http://ask.bitauto.com/" + se.Id + "/\" >问答(<span>" + se.AskCount + "</span>条)</a>");
			htmlCode.Append("</div>");
			htmlCode.Append("</dd>");
			htmlCode.Append("</dl>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 买车必看
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string GetMustSee(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder(1300);
			htmlCode.Append("<dl class=\"zs02\">");
			htmlCode.Append("<dt><a href=\"/" + se.AllSpell + "/koubei/\" >车型评价</a></dt>");
			htmlCode.Append("<dd class=\"sublink\">");
			string linkStr = "";

			string rptUrl = new Car_SerialBll().GetSerialKoubeiReport(se.Id);
			if (rptUrl.Length > 0)
			{
				linkStr += "<a href=\"" + string.Format(rptUrl, se.AllSpell) + "\" >口碑报告</a>";
			}

			if (se.YiCheCeShi.Length > 0)
			{
				if (linkStr.Length > 0)
					linkStr += " | ";
				linkStr += "<a href=\"" + se.YiCheCeShi + "\" >易车评测</a>";
			}
			if (se.MaiCheCeShi.Length > 0)
			{
				if (linkStr.Length > 0)
					linkStr += " | ";
				linkStr += "<a href=\"" + se.MaiCheCeShi + "\" >买车测试</a>";
			}
			linkStr += "</dd>";
			htmlCode.Append(linkStr);

			htmlCode.Append("<dd>");
			htmlCode.Append("<ul class=\"gb\">");
			htmlCode.Append("<li title=\"" + se.Virtues + "\" ><a class=\"good\" href=\"/" + se.AllSpell + "/koubei/gengduo/\">好评</a><p>" + StringHelper.SubString(se.Virtues, 68, false) + "</p></li>");
			htmlCode.Append("<li title=\"" + se.Defect + "\" ><a class=\"bad\" href=\"/" + se.AllSpell + "/koubei/gengduo/\">差评</a><p>" + StringHelper.SubString(se.Defect, 68, false) + "</p></li>");
			htmlCode.Append("</ul>");
			htmlCode.Append("<ul class=\"l\">");
			htmlCode.Append("<li>");
			if (se.ShangShi.Length == 0)
				htmlCode.Append("<a class=\"nolink\">上市报道</a></li>");
			else
				htmlCode.Append("<a href=\"" + se.ShangShi + "\">上市报道</a></li>");
			if (se.GouCheShouCe.Length == 0)
				htmlCode.Append("<li><a class=\"nolink\">购车手册</a></li>");
			else
				htmlCode.Append("<li><a href=\"" + se.GouCheShouCe + "\">购车手册</a></li>");
			if (new ProduceAndSellDataBll().HasSerialData(se.Id))
				htmlCode.Append("<li><a href=\"" + String.Format(se.XiaoShouShuJu, se.Id) + "\">销量</a></li>");
			else
				htmlCode.Append("<li><a class=\"nolink\">销量</a></li>");
			if (se.KeJi.Length == 0)
				htmlCode.Append("<li><a class=\"nolink\">技术</a></li>");
			else
				htmlCode.Append("<li><a href=\"" + se.KeJi + "\">技术</a></li>");
			if (se.AnQuan.Trim().Length == 0)
				htmlCode.Append("<li><a class=\"nolink\">安全</a></li>");
			else
				htmlCode.Append("<li><a href=\"" + se.AnQuan + "\">安全</a></li>");
			if (new Car_SerialBll().IsExitsMaintanceMessage(se.Id))
				htmlCode.Append("<li><a href=\"http://car.bitauto.com/" + se.AllSpell + "/baoyang/\">维修保养</a></li>");
			else
				htmlCode.Append("<li><a class=\"nolink\">维修保养</a></li>");
			htmlCode.Append("</ul>");
			htmlCode.Append("</dd>");
			htmlCode.Append("</dl>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 顶部新闻
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string GetTopNews(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder(12000);
			//获取数据
			XmlDocument xmlDoc = new Car_SerialBll().GetSerialFocusNews(se.Id);
			//焦点新闻
			string focusNewsHtml = MakeFoucsNewsHtml(xmlDoc, se);
			string xinwenHtml = MakeTypeNews("xinwen", se, "toptab_1");
			string hangqingHtml = MakeTypeNews("hangqing", se, "toptab_2");
			string shijiaHtml = MakeTypeNews("shijia", se, "toptab_3");

			string focusALinkStr = "";
			string xinwenALinkStr = " href=\"/" + se.AllSpell + "xinwen/\" ";
			string hangqingALinkStr = " href=\"/" + se.AllSpell + "hangqing/\" ";
			string shijiaALinkStr = " href=\"/" + se.AllSpell + "shijia/\" ";
			if (focusNewsHtml.Length == 0)
				focusALinkStr = " class=\"nolink\"";
			if (xinwenHtml.Length == 0)
				xinwenALinkStr = " class=\"nolink\"";
			if (hangqingHtml.Length == 0)
				hangqingALinkStr = " class=\"nolink\"";
			if (shijiaHtml.Length == 0)
				shijiaALinkStr = " class=\"nolink\"";

			htmlCode.AppendLine("<div class=\"line_box\"><h3></h3><div id=\"topnews\" class=\"h3_tab\">");
			htmlCode.AppendLine(" <ul id=\"toptab\">");
			htmlCode.AppendLine("<li><a " + focusALinkStr + ">最新</a></li>");
			htmlCode.AppendLine("<li><a " + xinwenALinkStr + ">新闻</a></li>");
			htmlCode.AppendLine("<li><a " + hangqingALinkStr + ">行情</a></li>");
			htmlCode.AppendLine("<li><a " + shijiaALinkStr + ">试驾</a></li>");
			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("</div>");
			htmlCode.AppendLine("<div id=\"toptab_0\"><div class=\"mainlist_box topa1\">");
			htmlCode.AppendLine(focusNewsHtml);
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("</div></div>");
			htmlCode.AppendLine(xinwenHtml);
			htmlCode.AppendLine(hangqingHtml);
			htmlCode.AppendLine(shijiaHtml);
			htmlCode.AppendLine("</div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 获取新车子品牌综述页的新闻
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string GetTopNewsHtmlForNewCar(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder(12000);
			//获取数据
			XmlDocument xmlDoc = new Car_SerialBll().GetSerialFocusNews(se.Id);
			//焦点新闻
			string focusNewsHtml = MakeFocusNewsHtmlForNewCar(xmlDoc);
			string xinwenHtml = MakeXinwenForNewCar(se);

			string focusALinkStr = "";
			string xinwenALinkStr = " href=\"/" + se.AllSpell + "xinwen/\" ";
			if (focusNewsHtml.Length == 0)
				focusALinkStr = " class=\"nolink\"";
			if (xinwenHtml.Length == 0)
				xinwenALinkStr = " class=\"nolink\"";

			htmlCode.AppendLine("<div class=\"line_box\"><h3></h3><div id=\"topnews\" class=\"h3_tab\">");
			htmlCode.AppendLine(" <ul id=\"toptab\">");
			htmlCode.AppendLine("<li><a " + focusALinkStr + ">最新</a></li>");
			htmlCode.AppendLine("<li><a " + xinwenALinkStr + ">新闻</a></li>");
			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("</div>");

			htmlCode.AppendLine("<div id=\"toptab_0\"><div class=\"mainlist_box topa1\">");
			htmlCode.AppendLine(focusNewsHtml);
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("</div></div>");
			htmlCode.AppendLine(xinwenHtml);
			htmlCode.AppendLine("</div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 获取导购新闻
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string GetDaogouNews(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder(1000);
			htmlCode.AppendLine("<div class=\"line_box\">");
			//获取数据
			XmlDocument xmlDoc = new Car_SerialBll().GetSerialFocusNews(se.Id);
			//导购新闻
			XmlNodeList newsList = xmlDoc.SelectNodes("/root/Introduce/listNews");
			string daogouHtml = MakeOtherNews(newsList, "introduce", se);
			htmlCode.AppendLine(daogouHtml);
			htmlCode.AppendLine("</div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 获取论坛话题
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string GetForumSubject(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder(1000);
			htmlCode.AppendLine("<div class=\"line_box\">");
			//获取数据
			XmlDocument xmlDoc = new Car_SerialBll().GetSerialFocusNews(se.Id);

			//论坛话题
			XmlNodeList newsList = xmlDoc.SelectNodes("/root/Forum/ForumSubject");
			string forumHtml = MakeOtherNews(newsList, "forum", se);
			htmlCode.AppendLine(forumHtml);
			htmlCode.AppendLine("</div>");
			return htmlCode.ToString();
		}

		private static string MakeFocusNewsHtmlForNewCar(XmlDocument xmlDoc)
		{
			StringBuilder htmlCode = new StringBuilder();
			XmlNodeList focusList = xmlDoc.SelectNodes("/root/FocusNews/listNews");
			Dictionary<int, int> sortDic = NewsChannelBll.GetNewsSortDic((XmlElement)xmlDoc.SelectSingleNode("/root/FocusNews/SortList"));
			if (focusList.Count > 0)
			{
				int counter = 0;
				foreach (XmlElement newsNode in focusList)
				{
					string newsTitle = newsNode.SelectSingleNode("title").InnerText;
					string filePath = newsNode.SelectSingleNode("filepath").InnerText;
					XmlNode contentNode = newsNode.SelectSingleNode("content");
					string content = "";
					if (contentNode != null)
						content = StringHelper.RemoveHtmlTag(contentNode.InnerText);
					content += "...<a href=\"" + filePath + "\">详细&gt;&gt;</a>";
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
					htmlCode.Append("<h2><a href=\"" + filePath + "\">" + HttpUtility.HtmlDecode(newsTitle) + "</a></h2>");
					htmlCode.Append("<p>" + content + "</p>");
					counter++;
					if (counter >= 2)
						break;
				}
			}

			return htmlCode.ToString();
		}

		private static string MakeFoucsNewsHtml(XmlDocument xmlDoc, SerialEntity se)
		{
			StringBuilder htmlCode = new StringBuilder(2000);
			XmlNodeList focusList = xmlDoc.SelectNodes("/root/FocusNews/listNews");
			Dictionary<int, int> sortDic = NewsChannelBll.GetNewsSortDic((XmlElement)xmlDoc.SelectSingleNode("/root/FocusNews/SortList"));
			if (focusList.Count > 0)
			{
				List<int> newsIdList = new List<int>();
				Dictionary<int, XmlElement> newsNodeDic = new Dictionary<int, XmlElement>();
				foreach (XmlElement newsNode in focusList)
				{
					int newsId = Convert.ToInt32(newsNode.SelectSingleNode("newsid").InnerText);
					if (!sortDic.ContainsValue(newsId))
						newsIdList.Add(newsId);
					newsNodeDic[newsId] = newsNode;
				}

				int newsNum = focusList.Count;
				if (newsNum > 10)
					newsNum = 10;
				for (int i = 0; i < newsNum; i++)
				{
					XmlElement newsNode = null;
					//如果该位置指定了新闻
					if (sortDic.ContainsKey(i + 1))
					{
						int newsId = sortDic[i + 1];
						if (newsNodeDic.ContainsKey(newsId))
						{
							newsNode = newsNodeDic[newsId];
							newsIdList.Remove(newsId);
						}
					}
					//该位置没有新闻
					if (newsNode == null)
					{
						if (i == 0)
						{
							//取第一条导购行情新闻
							newsNode = GetFirstNews(xmlDoc);
							if (newsNode != null)
							{
								int firstNewsId = Convert.ToInt32(newsNode.SelectSingleNode("newsid").InnerText);
								newsIdList.Remove(firstNewsId);
							}
						}

						if (newsNode == null)
						{
							//从列表中取一个
							newsNode = newsNodeDic[newsIdList[0]];
							newsIdList.Remove(newsIdList[0]);
						}
					}

					string newsTitle = newsNode.SelectSingleNode("title").InnerText;
					string filePath = newsNode.SelectSingleNode("filepath").InnerText;
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
					if (i == 0)
					{
						htmlCode.Append("<h2><a href=\"" + filePath + "\" >" + newsTitle + "</a></h2>");
						htmlCode.Append("<ul class=\"list_date\" id=\"topa1\">");
					}
					else
					{
						newsTitle = HttpUtility.HtmlDecode(newsTitle);
						string shortNewsTitle = StringHelper.SubString(newsTitle, 40, true);

						if (shortNewsTitle.StartsWith(newsTitle))
							shortNewsTitle = newsTitle;
						XmlNode cateNode = newsNode.SelectSingleNode("CategoryPath");
						string catePath = "";
						if (cateNode != null)
							catePath = cateNode.InnerText;
						string newsCategory = Car_SerialBll.GetNewsKind(catePath);
						string cateUrl = "/" + se.AllSpell + newsCategory + "/";
						switch (newsCategory)
						{
							case "xinwen":
								newsCategory = "新闻";
								break;
							case "hangqing":
								newsCategory = "行情";
								break;
							case "daogou":
								newsCategory = "导购";
								break;
							case "shipin":
								newsCategory = "视频";
								break;
							case "yongche":
								newsCategory = "用车";
								break;
							case "shijia":
								newsCategory = "试驾";
								break;
							case "pingce":
								newsCategory = "评测";
								break;
							default:
								cateUrl = "#";
								newsCategory = "其它";
								break;

						}
						string newsDate = Convert.ToDateTime(newsNode.SelectSingleNode("publishtime").InnerText).ToString("MM-dd");

						htmlCode.Append("<li><a href=\"" + cateUrl + "\" class=\"fl\" >[" + newsCategory + "]</a>");
						htmlCode.Append("<a href=\"" + filePath + "\" title=\"" + newsTitle + "\">" + shortNewsTitle + "</a><small>" + newsDate + "</small></li>");
					}
				}

				htmlCode.Append("</ul>");
			}
			return htmlCode.ToString();
		}

		private static XmlElement GetFirstNews(XmlDocument xmlDoc)
		{
			//取第一条导购行情新闻
			int[] cateIdList = new int[] { 29, 30, 31 };
			XmlElement newsNode = null;			//原创
			XmlElement newsNode2 = null;		//非原创		
			GetNewsNode(xmlDoc, cateIdList, ref newsNode, ref newsNode2);
			if (newsNode == null)
			{
				GetNewsNode(xmlDoc, new int[] { 4, 179, 227, 3 }, ref newsNode, ref newsNode2);
				if (newsNode == null)
				{
					GetNewsNode(xmlDoc, new int[] { 2, 13, 210 }, ref newsNode, ref newsNode2);
				}
			}
			if (newsNode == null)
				return newsNode2;
			else
				return newsNode;

		}

		private static void GetNewsNode(XmlDocument xmlDoc, int[] cateIdList, ref XmlElement newsNode, ref XmlElement newsNode2)
		{
			string xmlPath = CommonFunction.GetCategoryXmlPath(cateIdList);
			XmlNodeList nodeList = xmlDoc.SelectNodes("/root/FocusNews/listNews[" + xmlPath + "]");
			foreach (XmlElement tmpNode in nodeList)
			{
				//时间，30天之内的
				XmlNode dateNode = tmpNode.SelectSingleNode("publishtime");
				if (dateNode != null)
				{
					DateTime newsDate = ConvertHelper.GetDateTime(dateNode.InnerText);
					if (newsDate.AddDays(30) < DateTime.Now)
						break;
				}

				//是否原创
				XmlNode typeNode = tmpNode.SelectSingleNode("CreativeType");
				if (typeNode != null)
				{
					if (typeNode.InnerText == "0")
					{
						newsNode = tmpNode;
						break;
					}
					else if (newsNode2 == null)
						newsNode2 = tmpNode;
				}
			}
		}

		private static string MakeXinwenForNewCar(SerialEntity se)
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.Append("<div style=\"display: none\" id=\"toptab_1\"><div class=\"mainlist_box topa2 car_zs0519_01\">");
			htmlCode.AppendLine("<ul class=\"list_date\">");

			DataSet newsDs = new Car_SerialBll().GetNewsListBySerialId(se.Id, "xinwen");
			if (newsDs != null && newsDs.Tables.Count > 0 && newsDs.Tables[0].Rows.Count > 0 && newsDs.Tables.Contains("listNews"))
			{
				int newsCounter = 0;
				foreach (DataRow row in newsDs.Tables["listNews"].Rows)
				{
					newsCounter++;
					// 大于11行 最后行改成更多
					if (newsCounter == 12)
					{
						htmlCode.Append("<li class=\"topnewsmore\" ><a href=\"/" + se.AllSpell + "/xinwen/\">查看更多>></a></li>");
						break;
					}
					string newsTitle = Convert.ToString(row["title"]);
					int newsId = ConvertHelper.GetInteger(row["newsid"]);
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
					string newsUrl = Convert.ToString(row["filepath"]);
					DateTime publishTime = Convert.ToDateTime(row["publishtime"]);

					if (newsCounter == 1)
					{
						htmlCode.Append("<h2><a href=\"" + newsUrl + "\">" + newsTitle + "</a><h2>");
						continue;
					}
					string cityName = "[全国]";
					string relaCityName = row["relatedcityname"].ToString().Trim();
					if (relaCityName != "" && relaCityName.IndexOf(",") == 0)
					{
						// 如果只关联1个城市
						cityName = "[" + relaCityName + "]";
					}
					else
					{
						// 如果关联多个城市 则取编辑所在城市
						string editorInfo = row["editorName"].ToString().Trim();
						int cityPos = editorInfo.IndexOf("CityName:");
						if (editorInfo != "" && cityPos >= 0)
						{
							cityName = "[" + editorInfo.Substring(cityPos + 9).Trim() + "]";
							if (cityName == "[]")
							{
								cityName = "[全国]";
							}
						}
					}

					string cityUrl = new PageBase().GetCityURLByCityName(cityName.Replace("[", "").Replace("]", ""));
					string cityHasLink = "";
					if (cityName == "[全国]")
					{ cityHasLink = "[全国]"; }
					else
					{ cityHasLink = "<a class=\"fl\" href=\"" + cityUrl + "\" >" + cityName + "</a>"; }

					htmlCode.Append("<li><label>" + cityHasLink + "</label> <a title=\"" + newsTitle + "\" href=\"" + newsUrl + "\">" + newsTitle + "</a><small>" + publishTime.ToString("MM-dd") + "</small></li>");
					if (newsCounter >= 12)
						break;
				}
			}


			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("</div></div>");
			return htmlCode.ToString();
		}

		private static string MakeTypeNews(string newsType, SerialEntity se, string divIdStr)
		{
			StringBuilder htmlCode = new StringBuilder(3000);
			htmlCode.AppendLine("<div id=\"" + divIdStr + "\" style=\"display: none\"><div class=\"mainlist_box topa2 car_zs0519_01\">");
			htmlCode.AppendLine("<ul class=\"list_date\">");
			DataSet newsDs = new Car_SerialBll().GetNewsListBySerialId(se.Id, newsType);
			if (newsDs != null && newsDs.Tables.Count > 0 && newsDs.Tables[0].Rows.Count > 0 && newsDs.Tables.Contains("listNews"))
			{
				int newsCounter = 0;
				foreach (DataRow row in newsDs.Tables["listNews"].Rows)
				{
					// modified by chengl May.19.2010
					// 大于11行 最后行改成更多
					if (newsCounter == 11 && newsType != "shijia")
					{
						htmlCode.Append("<li class=\"topnewsmore\" ><a href=\"/" + se.AllSpell + "/" + newsType + "/\">查看更多>></a></li>");
						break;
					}
					string cityName = "[全国]";
					if (row["relatedcityname"].ToString().Trim() != "" && row["relatedcityname"].ToString().Trim().IndexOf(",") == 0)
					{
						// 如果只关联1个城市
						cityName = "[" + row["relatedcityname"].ToString().Trim() + "]";
					}
					else
					{
						// 如果关联多个城市 则取编辑所在城市
						if (row["editorName"].ToString().Trim() != "" && row["editorName"].ToString().Trim().IndexOf("CityName:") > 0)
						{
							cityName = "[" + row["editorName"].ToString().Substring(row["editorName"].ToString().Trim().IndexOf("CityName:") + 9).Trim() + "]";
							if (cityName == "[]")
							{
								cityName = "[全国]";
							}
						}
					}

					string cityUrl = new PageBase().GetCityURLByCityName(cityName.Replace("[", "").Replace("]", ""));
					string cityHasLink = "";
					if (cityName == "[全国]")
					{ cityHasLink = "[全国]"; }
					else
					{ cityHasLink = "<a class=\"fl\" href=\"" + cityUrl + "\" >" + cityName + "</a>"; }

					// modified end
					newsCounter++;
					string newsTitle = Convert.ToString(row["title"]);
					int newsId = ConvertHelper.GetInteger(row["newsid"]);
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
					string newsUrl = Convert.ToString(row["filepath"]);
					DateTime publishTime = Convert.ToDateTime(row["publishtime"]);
					// htmlCode.Append("<li><a target=\"_blank\" title=\"" + newsTitle + "\" href=\"" + newsUrl + "\">" + newsTitle + "</a><small>" + publishTime.ToString("yy-MM-dd") + "</small></li>");
					if (newsType == "shijia")
					{ htmlCode.Append("<li><a title=\"" + newsTitle + "\" href=\"" + newsUrl + "\">" + newsTitle + "</a><small>" + publishTime.ToString("yy-MM-dd") + "</small></li>"); }
					else
					{
						if (newsType == "hangqing" || newsType == "xinwen")
						{ htmlCode.Append("<li><label>" + cityHasLink + "</label> <a title=\"" + newsTitle + "\" href=\"" + newsUrl + "\">" + newsTitle + "</a><small>" + publishTime.ToString("MM-dd") + "</small></li>"); }
						else
						{ htmlCode.Append("<li><label>" + cityHasLink + "</label> <a title=\"" + newsTitle + "\" href=\"" + newsUrl + "\">" + newsTitle + "</a><small>" + publishTime.ToString("yy-MM-dd") + "</small></li>"); }
					}
					if (newsCounter >= 12)
						break;
				}
			}
			htmlCode.AppendLine("</ul>");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("</div></div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成导购新闻推荐
		/// </summary>
		/// <param name="htmlCode"></param>
		/// <param name="newsList"></param>
		private static string MakeOtherNews(XmlNodeList newsList, string type, SerialEntity se)
		{
			StringBuilder htmlCode = new StringBuilder(1000);
			// string bbsURL = new Car_SerialBll().GetForumUrlBySerialId(serialId);

			type = type.ToLower();
			string codeTitle = se.ShowName;
			string moreUrl = "";
			if (type == "introduce")
			{
				codeTitle += "导购推荐";
				moreUrl = "/" + se.AllSpell + "/daogou/";
			}
			else if (type == "forum")
			{
				moreUrl = "/" + se.AllSpell + "/";
				codeTitle += "论坛话题";
			}

			htmlCode.Append("<h3 class=\"car\"><span><span class=\"caption\"><a href=\"" + moreUrl + "\">" + codeTitle + "</a></span></span></h3>");
			if (newsList.Count > 0)
				htmlCode.Append("<div class=\"more\"><a href=\"" + moreUrl + "\">更多&gt;&gt; </a></div>");
			htmlCode.Append("<div class=\"mainlist_box reco\">");
			htmlCode.Append("<ul class=\"list_date\">");
			int loop = 1;
			foreach (XmlElement newsNode in newsList)
			{
				string newsTitle = newsNode.SelectSingleNode("title").InnerText.Trim();
				//过滤Html标签
				newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
				string shortNewsTitle = newsTitle;
				if (type == "forum")
					shortNewsTitle = StringHelper.SubString(newsTitle, 40, true);
				string filePath = "";
				string pubTime = "";
				if (type == "introduce")
				{
					filePath = newsNode.SelectSingleNode("filepath").InnerText;
					pubTime = newsNode.SelectSingleNode("publishtime").InnerText;
					pubTime = Convert.ToDateTime(pubTime).ToString("MM-dd");
				}
				else if (type == "forum")
				{
					string tid = newsNode.SelectSingleNode("tid").InnerText;
					filePath = newsNode.SelectSingleNode("url").InnerText;
					pubTime = "";
				}
				if (shortNewsTitle != newsTitle)
					htmlCode.Append("<li><a href=\"" + filePath + "\" title=\"" + newsTitle + "\" >" + shortNewsTitle + "</a><small>" + pubTime + "</small></li>");
				else
					htmlCode.Append("<li><a href=\"" + filePath + "\" >" + newsTitle + "</a><small>" + pubTime + "</small></li>");

				// modified by chengl Jul.22.2010
				loop++;
				if (loop > 5)
				{ break; }
			}
			htmlCode.Append("</ul>");
			htmlCode.Append("<div class=\"clear\"></div>");
			htmlCode.Append("</div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成车型列表
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string MakeCarList(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder tableCode = new StringBuilder(8000);
			tableCode.AppendLine("<div class=\"line_box onsale\" id=\"car_list\">");
			//获取数据
			List<string> yearList = new List<string>();
			Dictionary<string, string> yearHtmlDic = new Dictionary<string, string>();
			int maxPv = 0;
			bool hasStopSaleCar = false;
			foreach (CarEntity carInfo in se.CarList)
			{
				if (carInfo.SaleState == "停销")
				{
					hasStopSaleCar = true;
					continue;
				}
				if (carInfo.CarPV > maxPv)
					maxPv = carInfo.CarPV;
				if (carInfo.CarYear > 0)
				{
					string yearType = carInfo.CarYear + "款";
					if (!yearList.Contains(yearType))
						yearList.Add(yearType);
				}
			}

			yearList.Sort(NodeCompare.CompareStringDesc);

			string baseUrl = "/" + se.AllSpell + "/";
			tableCode.Append("<h3 class=\"car\"><span><span class=\"caption\">" + se.Brand.MasterBrand.Name + "&nbsp;" + se.Name + "在售车款</span></span><em class=\"h3sublink\">");
			for (int i = 0; i < yearList.Count; i++)
			{
				if (i >= 2)
				{
					break;
				}
				string yearStr = yearList[i];
				if (i > 0)
					tableCode.Append("|");
				string url = baseUrl + yearStr.Replace("款", "") + "/";
				tableCode.Append("<a href=\"" + url + "#car_list\" target=\"_self\">" + yearStr + "</a>");
			}

			if (hasStopSaleCar)
			{
				if (yearList.Count > 0)
					tableCode.Append("|");
				tableCode.Append("<a href=\"http://www.cheyisou.com/chexing/" + HttpUtility.UrlEncode(se.ShowName) + "/1.html?para=os|0|en|utf8\" >停售车款</a>");
			}
			tableCode.Append("</em></h3>");

			tableCode.Append("<div class=\"comparetable\">");

			tableCode.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"compare2\" id=\"compare\">");
			tableCode.Append("<tr><th width=\"250px\">车款名称</th>");
			tableCode.Append("<th width=\"50px\">热度</th>");
			tableCode.Append("<th width=\"70px\">变速箱</th>");
			tableCode.Append("<th width=\"100px\">厂家指导价</th>");
			tableCode.Append("<th width=\"155px\">商家报价</th>");
			tableCode.Append("<th width=\"73px\"><a href=\"" + baseUrl + "peizhi/\">参数配置</a></th>");
			tableCode.Append("</tr>");

			StringBuilder temp = new StringBuilder(3000);
			List<string> exhaustList = new List<string>();
			string carIDs = string.Empty;
			foreach (CarEntity carInfo in se.CarList)
			{
				if (carInfo.IsState != 1 || carInfo.SaleState == "停销")
					continue;
				if (!exhaustList.Contains(carInfo["Engine_ExhaustForFloat"]))
				{
					if (carIDs != "")
					{
						tableCode.Append(string.Format(temp.ToString(), carIDs));
						temp.Remove(0, temp.Length);
					}
					carIDs = "";
					//显示排量行
					exhaustList.Add(carInfo["Engine_ExhaustForFloat"]);
					temp.Append("<tr class=\"classify\"><td colspan=\"6\">" + carInfo["Engine_ExhaustForFloat"] + "L排量");
					temp.Append("(<a href=\"/car/interfaceforbitauto/ForBitAutoCompare.aspx?isNewID=1&carIDs={0}\">对比</a>)</td></tr>");
				}
				if (carIDs != "")
				{ carIDs += "," + carInfo.Id; }
				else
				{ carIDs += carInfo.Id; }

				string carUrl = baseUrl + "m" + carInfo.Id + "/";

				string yearType = carInfo.CarYear.ToString();
				if (yearType.Length > 0)
					yearType += "款";
				else
					yearType = "未知年款";

				string carFullName = se.ShowName + "&nbsp;" + carInfo.Name;
				if (carInfo.Name.StartsWith(se.ShowName))
					carFullName = se.ShowName + "&nbsp;" + carInfo.Name.Substring(se.ShowName.Length);
				if (yearType != "未知年款")
					carFullName = yearType + " " + carFullName;

				string stopPrd = "";
				if (carInfo.ProduceState == "停产")
					stopPrd = " <span class=\"tc\">停产</span>";

				string hasEnergySubsidy = carInfo["Car_EnergySubsidy"];
				if (hasEnergySubsidy.Length > 0)
				{
					hasEnergySubsidy = " <span class=\"butie\"><a href=\"http://news.bitauto.com/consumerpolicy/20120704/1805753482.html\" title=\"可获得3000元节能补贴\" >补贴</a></span>";
				}

				temp.Append("<tr><td><a href=\"" + carUrl + "\" >" + carFullName + "</a>" + stopPrd + hasEnergySubsidy + "</td>");
				//计算百分比
				int percent = (int)Math.Round((double)carInfo.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero);
				temp.Append("<td><div class=\"w\"><div class=\"p\"  style=\"width:" + percent + "%\"></div></div></td>");

				//变速器类型
				string tempTransmission = carInfo["UnderPan_TransmissionType"];
				if (tempTransmission.IndexOf("挡") >= 0)
				{
					tempTransmission = tempTransmission.Substring(tempTransmission.IndexOf("挡") + 1, tempTransmission.Length - tempTransmission.IndexOf("挡") - 1);
				}
				tempTransmission = tempTransmission.Replace("变速器", "");
				temp.Append("<td>" + tempTransmission + "</td>");

				//指导价
				if (carInfo.ReferPrice == 0)
					temp.Append("<td style=\"text-align:right\">暂无</td>");
				else
					temp.Append("<td style=\"text-align:right\"><a title=\"购车费用计算\" href=\"/gouchejisuanqi/?carid=" + carInfo.Id + "\">" + carInfo.ReferPrice + "万</a><a title=\"购车费用计算\" class=\"icon_cal\" href=\"/gouchejisuanqi/?carid=" + carInfo.Id + "\"></a></td>");

				if (carInfo.PriceRange.Length == 0)
					temp.Append("<td style=\"text-align:right\"><span style=\"color:gray\">暂无报价</span></td>");
				else
					temp.Append("<td style=\"text-align:right\"><span><a href=\"/" + se.AllSpell + "/m" + carInfo.Id + "/baojia/\" >" + carInfo.PriceRange + "</a></span> <a href=\"/" + se.AllSpell + "/m" + carInfo.Id + "/baojia/\" >查看</a></td>");
				temp.Append("<td>");
				temp.Append("&nbsp; <a target=\"_self\" href=\"javascript:addCarToCompare('" + carInfo.Id.ToString() + "','" + carInfo.Name.ToString() + "');\" >加入对比</a></td></tr>");
			}
			if (carIDs != "")
			{
				tableCode.Append(string.Format(temp.ToString(), carIDs));
			}
			else
				tableCode.Append("<tr><td class=\"noline\" colspan=\"7\">暂无在销车型！</td></tr>");
			tableCode.Append("</table>");
			tableCode.Append("<div class=\"more\"><a href=\"http://go.bitauto.com/goumai/?id=" + se.Id + "\" class=\"more2\">计划购买</a><a href=\"http://go.bitauto.com/guanzhu/?id=" + se.Id + "\" class=\"more3\">强烈关注</a></div>");
			tableCode.Append("</div>");
			tableCode.Append("<div class=\"clear\"></div></div>");
			return tableCode.ToString();
		}

		/// <summary>
		/// 生成彩虹条
		/// </summary>
		public static string MakeRainbowHtml(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder();
			//获取数据
			string rainbowStr = new RainbowListBll().GetRainbowListXML_CSID(se.Id);
			XmlDocument rainbowDoc = new XmlDocument();
			rainbowDoc.LoadXml(rainbowStr);
			string headStr = "";
			string conStr = "";
			XmlElement serialNode = (XmlElement)rainbowDoc.SelectSingleNode("/RainbowRoot/Serial");
			bool isShow = Convert.ToBoolean(serialNode.GetAttribute("IsShow"));
			if (isShow)
			{
				XmlNodeList eleList = rainbowDoc.SelectNodes("/RainbowRoot/Serial/Item");
				string importWidth = importWidth = "style=\"width:70px\""; ;
				if (eleList.Count == 6)
					importWidth = "style=\"width:119px\"";

				bool hasCar = false;			//是否已经有车图显示了
				for (int i = eleList.Count - 1; i >= 0; i--)
				{
					XmlElement ele = (XmlElement)eleList[i];
					string name = ele.GetAttribute("Name");
					//国产车型去掉三项
					if (eleList.Count > 6 && (name == "易车评测" || name == "口碑"))
						continue;
					string url = ele.GetAttribute("URL");
					string time = "";
					if (url.Trim().Length > 0)
						time = Convert.ToDateTime(ele.GetAttribute("Time")).ToString("yyyy-MM-dd");

					// modified by chengl Dec.8.2009 if KouBei Tag goto koubei link
					if (name == "口碑")
					{
						url = "/" + se.AllSpell + "/koubei/";
						time = DateTime.Now.ToShortDateString();
					}

					//计算彩虹条
					headStr = "<th scope=\"col\"><div " + importWidth + ">" + name + "</div></th>" + headStr;
					if (url.Length > 0)
					{
						if (hasCar)
							conStr = "<td class=\"rainbow_" + (i + 1) + "\"><a href=\"" + url + "\" >" + time + "</a></td>" + conStr;
						else
						{
							conStr = "<td class=\"rainbow_comp\"><a href=\"" + url + "\" >" + time + "</a></td>" + conStr;
							hasCar = true;
						}
						//showRainbow = true;
					}
					else
						conStr = "<td class=\"rainbow_none\">及时关注</td>" + conStr;
				}
				// string forumUrl = new Car_SerialBll().GetForumUrlBySerialId(serialId);

				htmlCode.Append("<div class=\"line_box rainbow_box\">");
				htmlCode.Append("<h3 class=\"gr\"><span><span class=\"caption\">" + se.Brand.MasterBrand.Name + se.Name + "追踪报道</span></span></h3>");
				if (eleList.Count == 6)
				{
					//进口车
					htmlCode.Append("<table class=\"table_rainbow2 table_rainbow\">");
				}
				else
				{
					htmlCode.Append("<table class=\"table_rainbow\">");
				}
				htmlCode.Append("<tbody><tr>");
				htmlCode.Append(headStr);
				htmlCode.Append("</tr><tr>");
				htmlCode.Append(conStr);
				htmlCode.Append("</tbody></table>");
				htmlCode.Append("<div class=\"more\"></div>");
				htmlCode.Append("</div>");
			}
			return htmlCode.ToString();
		}

		/// <summary>
		/// 新车子品牌综述页上显示的车型图片
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string MakeSerialImagesForNewcar(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			//图库接口本地化更改 by sk 2012.12.21
			string xmlUrl = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialStandardImagePath, se.Id));
			//string xmlUrl = "http://imgsvr.bitauto.com/autochannel/SerialImageService.aspx?dataname=serialstandardimage&serialid=" + se.Id + "&rownum=4";
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<div class=\"line_box zs100412_3\">");
			string tupianUrl = "/" + se.AllSpell + "/tupian/";
			htmlCode.AppendLine("<h3 class=\"car\"><span><span class=\"caption\"><a href=\"" + tupianUrl + "\">" + se.SeoName + "图片 </a></span></span><label class=\"h3sublink\"><a href=\"" + tupianUrl + "\">" + se.PicCount + "张</a></label></h3>");
			htmlCode.AppendLine("<div class=\"more\"><a href=\"" + tupianUrl + "\">更多&gt;&gt;</a></div>");
			htmlCode.AppendLine("<div class=\"pic_album\">");
			htmlCode.AppendLine("<ul class=\"list_pic\">");
			XmlTextReader reader = new XmlTextReader(xmlUrl);
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element && reader.Name == "ImageInfo")
				{
					reader.MoveToAttribute("ImageId");
					int imgId = ConvertHelper.GetInteger(reader.Value);
					reader.MoveToAttribute("ImageName");
					string imgName = reader.Value;
					reader.MoveToAttribute("ImageUrl");
					string imgUrl = reader.Value;
					reader.MoveToAttribute("Link");
					string photoUrl = reader.Value;

					if (imgId == 0 || imgUrl.Length == 0)
						imgUrl = WebConfig.DefaultCarPic;
					else
					{
						imgUrl = String.Format("http://img" + (imgId % 4 + 1).ToString() + ".bitautoimg.com/autoalbum/" + imgUrl, "1");
					}

					htmlCode.AppendLine("<li><a href=\"" + photoUrl + "\"><img width=\"165\" height=\"110\" alt=\"" + se.SeoName + imgName + "\" src=\"" + imgUrl + "\"></a><a href=\"" + photoUrl + "\">" + se.SeoName + imgName + "</a></li>");
				}

			}
			reader.Close();
			htmlCode.AppendLine("</ul><div class=\"clear\"></div>");
			htmlCode.AppendLine("</div></div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成图片部分
		/// </summary>
		public static string MakeSerialImages(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder(7000);
			htmlCode.AppendLine("<div class=\"line_box zs100412_3\">");
			//获取数据
			// 焦点图&中部图库组图(外观，内饰，空间，图解)
			//string xmlPicPath = string.Format(WebConfig.PhotoService, se.Id);
			//图库接口本地化更改 by sk 2012.12.21
			string xmlPicPath = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialPhotoListPath, se.Id));
			// 此 Cache 将通用于图片页和车型综述页
			DataSet dsCsPic = new PageBase().GetXMLDocToDataSetByURLForCache("CarChannel_SerialAllPic_" + se.Id, xmlPicPath, 10);
			if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("A"))
			{
				// 外观 6、图解 12、官方图 11、到店实拍 0 更多link
				string moreCateLink = "";
				//总数
				int picNum = 0;
				int cateId = 0;		//分类ID
				Dictionary<int, int> categoryPicNum = new Dictionary<int, int>();
				if (dsCsPic.Tables.Contains("A"))
				{
					foreach (DataRow row in dsCsPic.Tables["A"].Rows)
					{
						int cateNum = Convert.ToInt32(row["N"]);
						picNum += cateNum;
						cateId = Convert.ToInt32(row["G"]);
						// 分类更多link
						if (cateId == 6)
						{
							moreCateLink += "<a href=\"http://photo.bitauto.com/serialmore/" + se.Id + "/6/\">外观</a> | ";
						}
						else if (cateId == 12)
						{
							moreCateLink += "<a href=\"http://photo.bitauto.com/serialmore/" + se.Id + "/12/\">图解</a> | ";
						}
						else if (cateId == 11)
						{
							moreCateLink += "<a href=\"http://photo.bitauto.com/serialmore/" + se.Id + "/11/\">官方图</a> | ";
						}
						else if (cateId == 0)
						{
							moreCateLink += "<a href=\"http://photo.bitauto.com/serialmore/" + se.Id + "/0/\">到店实拍</a> | ";
						}
						else
						{ }

						if (cateId != 6 && cateId != 7 && cateId != 8 && cateId != 12)
							continue;
						categoryPicNum[cateId] = cateNum;
					}
				}
				string allPicUrl = "/" + se.AllSpell + "/tupian/";
				htmlCode.Append("<h3 class=\"car\"><span><span class=\"caption\"><a href=\"" + allPicUrl + "\">" + se.Brand.MasterBrand.Name + "&nbsp;" + se.Name + "图片 </a></span></span>");
				htmlCode.Append("<label class=\"h3sublink\"><a href=\"" + allPicUrl + "\">" + picNum + "张</a></label></h3>");
				htmlCode.Append("<div class=\"more\">" + moreCateLink + "<a href=\"http://photo.bitauto.com/serial/" + se.Id + "/\">更多&gt;&gt;</a></div>");

				int count = 0;		//有图片类型计数
				if (categoryPicNum.Count > 0 && dsCsPic.Tables.Contains("C") && dsCsPic.Tables["C"].Rows.Count > 0)
				{
					RenderPicByCategroy(se, htmlCode, 6, "外观", categoryPicNum, dsCsPic.Tables["C"], ref count);
					RenderPicByCategroy(se, htmlCode, 7, "内饰", categoryPicNum, dsCsPic.Tables["C"], ref count);
					RenderPicByCategroy(se, htmlCode, 8, "空间", categoryPicNum, dsCsPic.Tables["C"], ref count);
					RenderPicByCategroy(se, htmlCode, 12, "图解", categoryPicNum, dsCsPic.Tables["C"], ref count);
				}
				else
				{

					//显示其他图
					if (dsCsPic.Tables.Contains("A"))
					{
						foreach (DataRow row in dsCsPic.Tables["A"].Rows)
						{
							int cateNum = Convert.ToInt32(row["N"]);
							cateId = Convert.ToInt32(row["G"]);
							categoryPicNum[cateId] = cateNum;
						}

						foreach (DataRow row in dsCsPic.Tables["A"].Rows)
						{
							cateId = Convert.ToInt32(row["G"]);
							string cateName = Convert.ToString(row["D"]);
							picNum += categoryPicNum[cateId];
							RenderPicByCategroy(se, htmlCode, cateId, cateName, categoryPicNum, dsCsPic.Tables["C"], ref count);
						}
					}


				}
				htmlCode.Append("<div class=\"clear\"></div>");
			}
			htmlCode.Append("</div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成每个分类的图片页面
		/// </summary>
		/// <param name="htmlCode"></param>
		/// <param name="cateId"></param>
		/// <param name="cateName"></param>
		/// <param name="picNum"></param>
		/// <param name="dt">图片数据</param>
		/// <param name="isLast">是否是最后一个</param>
		private static void RenderPicByCategroy(SerialEntity se, StringBuilder htmlCode, int cateId, string cateName, Dictionary<int, int> picNumDic, DataTable dt, ref int count)
		{
			if (dt == null || dt.Rows.Count < 1)
				return;
			if (!picNumDic.ContainsKey(cateId))
				return;
			count++;
			int picNum = picNumDic[cateId];

			htmlCode.Append("<h4><a href=\"http://photo.bitauto.com/serialmore/" + se.Id + "/" + cateId + "/\" >" + cateName + "<span class=\"a\">(" + picNum + "张)</span></a></h4>");
			if (count == 1)
				htmlCode.Append("<div class=\"pic_album\">");
			htmlCode.Append("<ul class=\"list_pic\">");
			foreach (DataRow row in dt.Select("P='" + cateId + "'"))
			{
				int imgId = row["I"] == DBNull.Value ? 0 : Convert.ToInt32(row["I"]);
				string imgUrl = row["U"] == DBNull.Value ? "" : Convert.ToString(row["U"]);
				if (imgId == 0 || imgUrl.Length == 0)
					imgUrl = WebConfig.DefaultCarPic;
				else
					imgUrl = CommonFunction.GetPublishHashImgUrl(1, imgUrl, imgId);

				string picName = Convert.ToString(row["D"]);
				string picUlr = "http://photo.bitauto.com/picture/" + se.Id + "/" + imgId + "/";
				htmlCode.Append("<li><a href=\"" + picUlr + "\"><img src=\"" + imgUrl + "\" alt=\"" + se.Brand.MasterBrand.Name + se.Name + picName + "\" width=\"165\" height=\"110\"></a><a href=\"" + picUlr + "\" >" + picName + "</a></li>");
			}
			htmlCode.Append("</ul>");
			if (count < picNumDic.Count)
				htmlCode.Append("<div class=\"line\"></div>");
			htmlCode.Append("<div class=\"clear\"></div>  ");
			if (count == picNumDic.Count)
				htmlCode.Append("</div>");
		}


		/// <summary>
		/// 生成视频
		/// </summary>
		public static string MakeVideHtml(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder(2800);
			//取数据
			XmlNodeList videoList = new Car_SerialBll().GetSerialVideo(se.Id);
			if (videoList.Count > 0)
			{
				htmlCode.Append("<div class=\"line_box vlist\">");
				htmlCode.Append("<h3 class=\"car\">");
				htmlCode.Append("<span><span class=\"caption\"><a href=\"/" + se.AllSpell + "/shipin/\">");
				htmlCode.Append(se.Brand.MasterBrand.Name + "&nbsp;" + se.Name + "视频</a></span></span></h3>");
				htmlCode.Append("<div class=\"more\">");
				htmlCode.Append("<a href=\"/" + se.AllSpell + "/shipin/\">更多&gt;&gt;</a></div>");
				htmlCode.Append("<div class=\"pic_album\">");
				htmlCode.Append("<ul class=\"list_pic\">");
				try
				{
					foreach (XmlElement videoNode in videoList)
					{
						string videoTitle = videoNode.SelectSingleNode("title").InnerText;
						videoTitle = StringHelper.RemoveHtmlTag(videoTitle);

						string shortTitle = videoNode.SelectSingleNode("facetitle").InnerText;
						shortTitle = StringHelper.RemoveHtmlTag(shortTitle);

						string imgUrl = videoNode.SelectSingleNode("picture").InnerText;
						if (imgUrl.Trim().Length == 0)
							imgUrl = WebConfig.DefaultVideoPic;
						string filepath = videoNode.SelectSingleNode("filepath").InnerText;

						htmlCode.Append("<li><a href=\"" + filepath + "\"class=\"v_bg\" alt=\"视频播放\"></a><a href=\"" + filepath + "\"><img src=\"" + imgUrl + "\" alt=\"" + se.Brand.MasterBrand.Name + se.Name + videoTitle + "\" width=\"165\" height=\"110\" /></a>");
						if (shortTitle != videoTitle)
							htmlCode.Append("<div class=\"name\"><a href=\"" + filepath + "\" title=\"" + videoTitle + "\" >" + shortTitle + "</a></div></li>");
						else
							htmlCode.Append("<div class=\"name\"><a href=\"" + filepath + "\">" + videoTitle + "</a></div></li>");
					}
				}
				catch
				{ }
				htmlCode.Append("</ul>");
				htmlCode.Append("<div class=\"clear\"></div>");
				htmlCode.Append("</div>");
				htmlCode.Append("</div>");
			}

			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成子品牌点评
		/// </summary>
		/// <param name="se"></param>
		/// <param name="paraDic"></param>
		/// <returns></returns>
		public static string MakeDianpingHtml(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			XmlDocument dpDoc = new Car_SerialBll().GetCarshowSerialDianping(se.Id);
			if (dpDoc == null || !dpDoc.HasChildNodes)
			{ return ""; }
			StringBuilder htmlCode = new StringBuilder(4000);
			htmlCode.AppendLine("<div class=\"line_box  choice\">");
			int count = ConvertHelper.GetInteger(dpDoc.DocumentElement.GetAttribute("count"));
			int dianpingCount = count;
			htmlCode.Append("<h3 class=\"commu\"><span><span class=\"caption\">" + se.Brand.MasterBrand.Name + "&nbsp;" + se.Name + "点评精选</span></span><strong>已有<em>" + count + "</em>条点评");
			htmlCode.Append("<a href=\"/" + se.AllSpell + "/koubei/gengduo/\">查看全部</a> | <a href=\"http://koubei.bitauto.com/" + se.AllSpell + "/koubei/tianjia/\">我要点评</a></strong></h3>");

			string moreUrl = "/" + se.AllSpell + "/koubei/gengduo/";
			for (int i = 3; i >= 1; i--)
			{
				XmlElement dpNode = (XmlElement)dpDoc.SelectSingleNode("/SerialDianping/Dianping[@type=\"" + i + "\"]");
				if (dpNode == null)
					continue;
				count = ConvertHelper.GetInteger(dpNode.GetAttribute("count"));
				htmlCode.Append("<div class=\"list_li\">");
				switch (i)
				{
					case 1:
						htmlCode.Append("<h4 class=\"cha\">");
						htmlCode.Append("<a href=\"" + moreUrl + "\">差评</a><strong><em>(" + count + "条)</em></strong></h4>");
						break;
					case 2:
						htmlCode.Append("<h4 class=\"zhong\">");
						htmlCode.Append("<a href=\"" + moreUrl + "\">中评</a><strong><em>(" + count + "条)</em></strong></h4>");
						break;
					case 3:
						htmlCode.Append("<h4 class=\"hao\">");
						htmlCode.Append("<a href=\"" + moreUrl + "\">好评</a><strong><em>(" + count + "条)</em></strong></h4>");
						break;
				}

				htmlCode.Append("<ul>");
				int counter = 0;
				foreach (XmlElement ele in dpNode.ChildNodes)
				{
					counter++;
					string title = ele.SelectSingleNode("title").InnerText;
					string url = ele.SelectSingleNode("url").InnerText;
					string shortTitle = title;
					if (StringHelper.GetRealLength(title) > 24)
						shortTitle = StringHelper.SubString(title, 24, false);
					htmlCode.Append("<li><a href=\"" + url + "\" title=\"" + title + "\">" + shortTitle + "</a></li>");
					if (counter >= 7)
						break;
				}
				htmlCode.Append("</ul>");
				htmlCode.Append("<div class=\"more\">");
				htmlCode.Append("<a href=\"" + moreUrl + "\">更多&gt;&gt;</a></div>");
				htmlCode.Append("</div>");

			}
			htmlCode.Append("<div class=\"clear\"></div>");
			htmlCode.Append("<div class=\"more\">");
			htmlCode.Append("<a href=\"" + moreUrl + "\">更多&gt;&gt;</a></div></div>");

			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成热门文章
		/// </summary>
		/// <param name="htmlCode"></param>
		public static string MakeHotNewsHtml(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder(1000);
			//获取数据
			XmlDocument xmlDoc = new Car_SerialBll().GetSerialHotNews(se.Id);
			XmlNodeList newsList = xmlDoc.SelectNodes("NewDataSet/NewsCommentTop");
			if (newsList.Count > 0)
			{
				htmlCode.Append("<div class=\"line_box hot_article\">");
				htmlCode.Append("<h3><span>" + se.ShowName.Replace("(进口)", "").Replace("（进口）", "") + "热门文章</span></h3>");
				htmlCode.Append("<div id=\"rank_newcar_box\">");
				htmlCode.Append("<ol class=\"hot_ranking\">");
				int counter = 0;
				foreach (XmlElement newsNode in newsList)
				{
					counter++;
					string newsTitle = newsNode.SelectSingleNode("NewsTitle").InnerText;
					//过滤Html标签
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
					string shortNewsTitle = StringHelper.SubString(newsTitle, 26, true);
					string filePath = newsNode.SelectSingleNode("NewsUrl").InnerText;
					string pubTime = newsNode.SelectSingleNode("Time").InnerText;
					pubTime = Convert.ToDateTime(pubTime).ToString("MM-dd");
					if (shortNewsTitle != newsTitle)
						htmlCode.Append("<li><a href=\"" + filePath + "\" title=\"" + newsTitle + "\">" + shortNewsTitle + "</a></li>");
					else
						htmlCode.Append("<li><a href=\"" + filePath + "\">" + newsTitle + "</a></li>");
					if (counter >= 5)
						break;
				}
				htmlCode.Append("</ol></div>");
				htmlCode.Append("</div>");
			}
			return htmlCode.ToString();
		}


		/// <summary>
		/// 子品牌还关注
		/// </summary>
		/// <returns></returns>
		public static string MakeSerialToSerialHtml(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder htmlCode = new StringBuilder(1800);
			List<EnumCollection.SerialToSerial> lsts = new PageBase().GetSerialToSerialByCsID(se.Id, 6);
			if (lsts.Count > 0)
			{
				htmlCode.AppendLine("<div class=\"line_box zs100412_1\"><h3><span>看过" + se.ShowName.Replace("(进口)", "").Replace("（进口）", "") + "的还看过</span></h3>");
				htmlCode.AppendLine("<ul class=\"pic_list\">");
				foreach (EnumCollection.SerialToSerial sts in lsts)
				{
					string csName = sts.ToCsShowName.ToString();
					htmlCode.Append("<li><a href=\"/" + sts.ToCsAllSpell.ToString().ToLower() + "/\">");
					htmlCode.Append("<img src=\"" + sts.ToCsPic.ToString() + "\" alt=\"" + csName + "\" width=\"90\" height=\"60\"></a>");
					string shortName = StringHelper.SubString(csName, 12, true);
					if (shortName.StartsWith(csName))
						shortName = csName;
					if (shortName != csName)
						htmlCode.Append("<a href=\"/" + sts.ToCsAllSpell.ToString().ToLower() + "/\" title=\"" + csName + "\">" + shortName + "</a>");
					else
						htmlCode.Append("<a href=\"/" + sts.ToCsAllSpell.ToString().ToLower() + "/\">" + csName + "</a>");
					if (sts.ToCsPriceRange.Trim().Length == 0)
						htmlCode.Append("<div>&nbsp;</div>");
					else
						htmlCode.Append("<div>" + StringHelper.SubString(sts.ToCsPriceRange.ToString(), 14, false) + "</div>");
					htmlCode.Append("</li>");
				}

				htmlCode.AppendLine("</ul>");
				htmlCode.AppendLine("<div class=\"hiedline\"></div>");
				//htmlCode.AppendLine("<div class=\"more\"><a href=\"#\"></a></div>");
				htmlCode.AppendLine("<div class=\"clear\"></div>");
				htmlCode.AppendLine("</div>");
			}
			return htmlCode.ToString();
		}

		/// <summary>
		/// 取子品牌对比排行数据
		/// </summary>
		/// <returns></returns>
		public static string MakeHotSerialCompare(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = new Dictionary<string, List<Car_SerialBaseEntity>>();
			carSerialBaseList = new Car_SerialBll().GetSerialCityCompareList(se.Id, HttpContext.Current);
			StringBuilder htmlCode = new StringBuilder(1500);
			string compareBaseUrl = "/car/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?count=3&csids=" + se.Id + ",";
			if (carSerialBaseList != null && carSerialBaseList.Count > 0 && carSerialBaseList.ContainsKey("全国"))
			{
				List<Car_SerialBaseEntity> serialCompareList = carSerialBaseList["全国"];
				htmlCode.Append("<div class=\"line_box newcarindex\" id=\"serialHotCompareList\">");
				htmlCode.Append("<h3><span>网友都用它和谁比</span></h3>");
				htmlCode.Append("<div class=\"more\"><a href=\"/chexingduibi/\">车型对比>></a></div>");
				htmlCode.Append("<div class=\"ranking_list\" id=\"rank_model_box\">");
				htmlCode.Append("<div class=\"this\">" + se.Brand.MasterBrand + se.Name + " VS</div>");
				htmlCode.Append("<ol class=\"hot_ranking\">");

				for (int i = 0; i < serialCompareList.Count; i++)
				{
					Car_SerialBaseEntity carSerial = serialCompareList[i];
					htmlCode.Append("<li><em><a href=\"/" + carSerial.SerialNameSpell.Trim().ToLower() + "/\" >");
					htmlCode.Append(carSerial.SerialShowName.Trim() + "</a></em>");
					htmlCode.Append("<span><a href=\"" + compareBaseUrl + carSerial.SerialId + "\">对比</a></span></li>");
				}

				htmlCode.Append("</ol></div>");
				htmlCode.Append("</div>");
			}

			return htmlCode.ToString();
		}

		///// <summary>
		///// 取子品牌答疑块
		///// </summary>
		//public static string MakeSerialAskHtml(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		//{
		//	//获取数据
		//	StringBuilder htmlCode = new StringBuilder(1000);
		//	XmlDocument xmlDoc = new Car_SerialBll().GetSerialAskEntries(se.Id);
		//	XmlNamespaceManager xnm = new XmlNamespaceManager(xmlDoc.NameTable);
		//	xnm.AddNamespace("atom", "http://www.w3.org/2005/Atom");
		//	XmlNodeList entries = xmlDoc.SelectNodes("//atom:entry", xnm);
		//	htmlCode.Append("<div class=\"line_box ask_box\">");
		//	htmlCode.Append("<h3><span><a href=\"http://ask.bitauto.com/" + se.Id + "/\">在线问答</a></span></h3>");
		//	htmlCode.Append("<div class=\"more\"><a href=\"http://ask.bitauto.com/ask?q=" + HttpUtility.UrlEncode(se.Name) + "\">我要提问</a></div>");
		//	htmlCode.Append("<div class=\"search\">");
		//	htmlCode.Append("<fieldset>");
		//	htmlCode.Append("<input id=\"askkeyword\" name=\"\" class=\"input_text\" type=\"text\"> <input name=\"\" class=\"btn_search\" onclick=\"window.open('http://ask.bitauto.com/search?keyword=' + document.getElementById('askkeyword').value)\" value=\"找答案\" type=\"button\">");
		//	htmlCode.Append("</fieldset>");
		//	htmlCode.Append("</div>");
		//	htmlCode.Append("<ul class=\"list\">");
		//	foreach (XmlElement entryNode in entries)
		//	{
		//		string newsTitle = entryNode.SelectSingleNode("atom:title", xnm).InnerText;
		//		//过滤Html标签
		//		newsTitle = BitAuto.Utils.StringHelper.RemoveHtmlTag(newsTitle);
		//		string shortTitle = BitAuto.Utils.StringHelper.SubString(newsTitle, 30, true);
		//		string askLink = entryNode.SelectSingleNode("atom:link", xnm).Attributes["href"].Value;
		//		if (newsTitle != shortTitle)
		//			htmlCode.Append("<li><a href=\"" + askLink + "\" title=\"" + newsTitle + "\">" + shortTitle + "</a></li>");
		//		else
		//			htmlCode.Append("<li><a href=\"" + askLink + "\">" + newsTitle + "</a></li>");
		//	}
		//	htmlCode.Append("</ul>");
		//	htmlCode.Append("</div>");
		//	return htmlCode.ToString();
		//}

		/// <summary>
		/// 取子品牌相关用户
		/// </summary>
		public static string GetUserBlockByCarSerialId(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			StringBuilder sbUserBlock = new StringBuilder(2600);
			// 计划购买
			DataTable dtWant = new PageBase().GetUserByCarSerialId(se.Id, 2, 3);
			if (dtWant != null && dtWant.Rows.Count > 0)
			{
				sbUserBlock.Append("<div class=\"line_box zh_driver\">");
				sbUserBlock.Append("<h3><span>和想买这款车的人聊聊</span></h3>");
				sbUserBlock.Append("<div class=\"index_friend_r_l\">");
				sbUserBlock.Append("<ul>");
				for (int i = 0; i < dtWant.Rows.Count; i++)
				{
					sbUserBlock.Append("<li><a href=\"http://i.bitauto.com/u" + dtWant.Rows[i]["userId"].ToString() + "/\">");
					sbUserBlock.Append("<img height=\"60\" width=\"60\" src=\"" + dtWant.Rows[i]["userAvatar"].ToString() + "\"></a>");
					sbUserBlock.Append("<strong><a href=\"http://i.bitauto.com/u" + dtWant.Rows[i]["userId"].ToString() + "/\">" + dtWant.Rows[i]["username"].ToString() + "</a></strong>");
					sbUserBlock.Append("<p><a class=\"add_friend\" href=\"#\" onclick=\"javascript:AjaxAddFriend.show(" + dtWant.Rows[i]["userId"].ToString() + ", " + se.Id.ToString() + ");return false;\">加为好友</a></p></li>");
				}
				sbUserBlock.Append("</ul>");
				sbUserBlock.Append("</div><div class=\"clear\"> </div>");
				sbUserBlock.Append("<div class=\"more\"><a href=\"http://i.bitauto.com/FriendMore_c0_s" + se.Id + "_p1_sort1_r010.html\">更多&gt;&gt;</a></div>");
				sbUserBlock.Append("</div>");
			}
			// 车主
			DataTable dtOwner = new PageBase().GetUserByCarSerialId(se.Id, 3, 3);
			if (dtOwner != null && dtOwner.Rows.Count > 0)
			{
				sbUserBlock.Append("<div class=\"line_box zh_driver\">");
				sbUserBlock.Append("<h3><span>和车主聊聊</span></h3>");
				sbUserBlock.Append("<div class=\"index_friend_r_l\">");
				sbUserBlock.Append("<ul>");
				for (int i = 0; i < dtOwner.Rows.Count; i++)
				{
					sbUserBlock.Append("<li><a href=\"http://i.bitauto.com/u" + dtOwner.Rows[i]["userId"].ToString() + "/\">");
					sbUserBlock.Append("<img height=\"60\" width=\"60\" src=\"" + dtOwner.Rows[i]["userAvatar"].ToString() + "\"></a>");
					sbUserBlock.Append("<strong><a href=\"http://i.bitauto.com/u" + dtOwner.Rows[i]["userId"].ToString() + "/\">" + dtOwner.Rows[i]["username"].ToString() + "</a></strong>");
					sbUserBlock.Append("<p><a class=\"add_friend\" href=\"#\" onclick=\"AjaxAddFriend.show(" + dtOwner.Rows[i]["userId"].ToString() + ", " + se.Id + ",3);return false;\">加为好友</a></p></li>");
				}
				sbUserBlock.Append("</ul>");
				sbUserBlock.Append("</div><div class=\"clear\"> </div>");
				sbUserBlock.Append("<div class=\"more\"><a href=\"http://i.bitauto.com/FriendMore_c0_s" + se.Id + "_p1_sort1_r001.html\">更多&gt;&gt;</a></div>");
				sbUserBlock.Append("</div>");
			}
			return sbUserBlock.ToString();
		}


		/// <summary>
		/// 得到品牌下的其他子品牌
		/// </summary>
		/// <returns></returns>
		public static string GetBrandOtherSerial(SerialEntity se, Dictionary<string, TemplateParameter> paraDic)
		{
			PageBase page = new PageBase();
			List<CarSerialPhotoEntity> carSerialPhotoList = new Car_BrandBll().GetCarSerialPhotoListByCBID(se.Brand.Id, false);

			carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

			if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
			{
				return "";
			}

			string brandUrl = "<a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}</a>";
			string serialUrl = "<a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}</a>";

			StringBuilder brandOtherSerial = new StringBuilder(800);
			brandOtherSerial.AppendLine("<div class=\"line_box review_list autoheight\">");
			brandOtherSerial.Append("<h3>");
			brandOtherSerial.AppendFormat("<span>{0}</span>", string.Format(brandUrl, se.Brand.AllSpell, se.Brand.Name + "其他车型"));
			brandOtherSerial.Append("</h3>");
			//brandOtherSerial.AppendFormat("<div class=\"more\">{0}</div>", string.Format(brandUrl, cse.Cb_AllSpell, "更多&gt;&gt;"));
			brandOtherSerial.Append("<ul class=\"list\">");

			foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
			{
				if (entity.SerialLevel == "概念车" || entity.SerialId == se.Id)
				{
					continue;
				}
				string priceRang = page.GetSerialIntPriceRangeByID(entity.SerialId);
				if (entity.SaleState == "待销")
				{
					priceRang = "未上市";
				}
				else if (priceRang.Trim().Length == 0)
				{
					priceRang = "暂无报价";
				}
				string tempCsSeoName = string.IsNullOrEmpty(entity.CS_SeoName) ? entity.ShowName : entity.CS_SeoName;
				brandOtherSerial.AppendFormat("<li>{0}<span>{1}</span></li>", string.Format(serialUrl, entity.CS_AllSpell, tempCsSeoName), priceRang);
			}

			brandOtherSerial.Append("</ul></div>");

			return brandOtherSerial.ToString();
		}

	}



}
