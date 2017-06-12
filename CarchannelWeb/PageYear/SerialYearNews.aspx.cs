﻿using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.BLL;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannel.CarchannelWeb.PageYear
{
	public partial class SerialYearNews : PageBase
	{
		protected string serialShowName;	//子品牌全名称
		protected string masterBrandName;	//主品牌名称
		protected string serialName;		//子品牌名称
		protected string serialSeoName;     //子品牌SEO名称
		private string serialSpell;			//子品牌全拼
		protected int serialId;				//子品牌ID
		protected int year;					//子品牌年款
		private int pageSize = 10;			//页大小
		private int pageIndex;				//页号
		protected CarNewsType _CarNewsType;
		private string newsType;			//类别，是导购还是市场
		protected string SerialNewHead = string.Empty; // 头

		protected int startIndex;
		protected int endIndex;

		protected string pageTitle = "";
		protected string pageKeywords = "";
		protected string pageDescription = "";

		protected string newsHtml = "";
		protected string newsCategorysHtml = "";
		protected string carCompareHtml = "";
		protected string newsNavHtml = "";

		protected string JsForCommonHead = "";

		protected SerialEntity serialBase;
		protected string UCarHtml = string.Empty;

		protected string serialToSeeHtml = string.Empty;

		private int _CurrentNewsCount = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);

			GetParameter();

			newsHtml = RenderNewsList();

			if (newsType == "hangqing")
				newsNavHtml = RenderXinWenTitle();
			else
				newsNavHtml = RenderNewsNav();

			MakeHotSerialCompare();
			//MakeSerialToSerialHtml();
			ucSerialToSee.SerialId = serialId;
			ucSerialToSee.SerialName = serialShowName;
			//UCarHtml = new Car_SerialBll().GetUCarHtml(serialId);

		}

		private void GetParameter()
		{
			serialId = Convert.ToInt32(Request.QueryString["id"]);
			string yearType = Request.QueryString["year"];
			year = 0;
			bool isYear = Int32.TryParse(yearType, out year);
			if (!isYear)
				Response.Redirect("404error.aspx?info=参数错误！");

			newsType = Request.QueryString["type"];
			if (newsType == null)
				newsType = "xinwen";
			newsType = newsType.ToLower();

			switch (newsType)
			{
				case "wenzhang":
					pageTitle = "【{0}新闻-{0}上市新闻】_{1}{0}-易车网";
					pageKeywords = "{0}新闻,{0}上市新闻,{1}{0}";
					pageDescription = "{0}新闻:易车网车型频道为您提供最权威的{1}{0}新闻资讯、{1}{0}上市新闻、{2}最新行情资讯、{2}优惠、{2}经销商降价信息、网友点评讨论等。";
					_CarNewsType = CarNewsType.wenzhang;
					break;
				case "xinwen":
					pageTitle = "【{0}新闻-{0}上市新闻】_{1}{0}-易车网";
					pageKeywords = "{0}新闻,{0}上市新闻,{1}{0}";
					pageDescription = "{0}新闻:易车网车型频道为您提供最权威的{1}{0}新闻资讯、{1}{0}上市新闻、{2}最新行情资讯、{2}优惠、{2}经销商降价信息、网友点评讨论等。";
					_CarNewsType = CarNewsType.xinwen;
					break;
				case "daogou":
					pageTitle = "【{0}导购-{0}促销信息】_{1}{0}-易车网";
					pageKeywords = "{0}导购,{0}促销,{1}{2}";
					pageDescription = "{0}导购:易车网车型频道为您提供最权威的{1}{0}评测导购资讯、最及时的{1}{0}优惠促销信息、{2}最新报价、{2}价格、{2}经销商降价信息、网友点评讨论等。";
					_CarNewsType = CarNewsType.daogou;
					break;
				case "shijia":
					pageTitle = "【{0}促销-{0}试驾】_{1}{0}-易车网";
					pageKeywords = "{0}试驾,{0}促销,{1}{2}";
					pageDescription = "{0}试驾:易车网车型频道为您提供最权威的{1}{0}试驾信息、最及时的{1}{0}优惠促销信息、{2}最新报价、{2}价格、{2}经销商降价信息、网友点评讨论等。";
					_CarNewsType = CarNewsType.shijia;
					break;
				case "yongche":
					pageTitle = "【{0}用车-{0}用车指南】_{1}{0}-易车网";
					pageKeywords = "{0}用车,{0}用车指南,{1}{2}";
					pageDescription = "{0}用车:易车网车型频道为您提供最权威的{1}{0}用车指南、最及时的{1}{0}优惠促销信息、{2}最新报价、{2}价格、{2}经销商降价信息、网友点评讨论等。";
					_CarNewsType = CarNewsType.yongche;
					break;
				case "hangqing":
					pageTitle = "【{0}行情-{0}行情新闻】_{1}{0}-易车网";
					pageKeywords = "{0}行情,{0}行情新闻,{1}{0}";
					pageDescription = "{0}行情:易车网车型频道为您提供最权威的{0}行情信息、{0}优惠促销信息、{2}最新行情报价、{2}优惠、{2}经销商降价信息、网友点评讨论等。";
					_CarNewsType = CarNewsType.hangqing;
					break;
				case "pingce":
					pageTitle = "【{0}评测-{0}单车评测】_{1}-易车网BitAuto.com";
					pageKeywords = "{0}评测,{0}单车评测,{1}{2}";
					pageDescription = "{0}评测:易车网(BitAuto.com)导购频道为您提供最权威的{0}单车评测、最及时的{0}优惠促销信息、网友点评讨论等。";
					_CarNewsType = CarNewsType.pingce;
					break;
				case "gaizhuang":
					pageTitle = "【{0}改装-{0}单车改装指南】_{1}{0}-易车网";
					pageKeywords = "{0}改装,{0}单车改装,{1}{2}";
					pageDescription = "{0}改装:易车网车型频道为您提供最权威的{1}{0}改装指南、最及时的{1}{0}内饰改装资讯、{2}最新报价、{2}价格、{2}经销商降价信息、网友点评讨论等。";
					_CarNewsType = CarNewsType.gaizhuang;
					break;
				case "anquan":
					pageTitle = "【{0}安全-{0}碰撞安全测试】_{1}{0}-易车网";
					pageKeywords = "{0}安全,{0}碰撞测试,{1}{0}";
					pageDescription = "{0}安全:易车网车型频道为您提供最权威的{1}{0}安全指南、最及时的{1}{0}碰撞安全测试、{2}最新报价、{2}价格、{2}经销商降价信息、网友点评讨论等。";
					_CarNewsType = CarNewsType.anquan;
					break;
				default:
					newsType = "xinwen";
					pageTitle = "【{0}新闻-{0}市场新闻】_{1}-易车网BitAuto.com";
					pageKeywords = "{0}新闻,{0}市场新闻,{1}{2}";
					pageDescription = "{0}新闻:易车网(BitAuto.com)新闻频道为您提供最权威的{0}市场新闻资讯、最及时的{0}优惠促销信息、网友点评讨论等。";
					_CarNewsType = CarNewsType.xinwen;
					break;
			}

			pageIndex = Convert.ToInt32(Request.QueryString["pageIndex"]);
			if (pageIndex == 0)
				pageIndex = 1;

			serialBase = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
			if (serialBase != null)
			{
				serialShowName = serialBase.ShowName;
				if (serialId == 1568)
					serialShowName = "索纳塔八";
				serialSpell = serialBase.AllSpell;
				masterBrandName = serialBase.Brand.MasterBrand.Name;
				serialName = serialBase.Name;

				pageTitle = String.Format(pageTitle, serialBase.SeoName + " " + year.ToString() + "款", serialBase.Brand.MasterBrand.Name);
				pageKeywords = String.Format(pageKeywords, serialBase.SeoName + " " + year.ToString() + "款", serialBase.Brand.MasterBrand.Name, serialShowName + " " + year.ToString() + "款");
				pageDescription = String.Format(pageDescription, serialBase.SeoName + " " + year.ToString() + "款", serialBase.Brand.MasterBrand.SeoName, serialBase.SeoName);

				#region 取头
				string tagName = "Cs" + newsType + "ForYear";
				SerialNewHead = base.GetCommonNavigation(tagName, serialId).Replace("{0}", year.ToString());
				JsForCommonHead = "if(document.getElementById('carYearList_" + year.ToString() + "')){document.getElementById('carYearList_" + year.ToString() + "').className='current';}changeSerialYearTag(0," + year.ToString() + ",'');";
				#endregion
			}
			else
				Response.Redirect("car.bitauto.com");
		}

		/// <summary>
		/// 生成小导航代码
		/// </summary>
		/// <returns></returns>
		private string RenderNewsNav()
		{
			Dictionary<CarNewsType, string> titleTag = new Dictionary<CarNewsType, string>();
			titleTag.Add(CarNewsType.wenzhang, "全部");
			//titleTag.Add(CarNewsType.pingce, "易车评测");
			titleTag.Add(CarNewsType.shijia, "体验试驾");
			//titleTag.Add("xinwen", "新闻");
			//titleTag.Add("hangqing", "行情");
			titleTag.Add(CarNewsType.daogou, "导购");
			titleTag.Add(CarNewsType.yongche, "用车");
			titleTag.Add(CarNewsType.gaizhuang, "改装");
			titleTag.Add(CarNewsType.anquan, "安全");
			titleTag.Add(CarNewsType.xinwen, "新闻");
			CarNewsBll newsBll = new CarNewsBll();
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.Append("<div class=\"title-box title-box2\">");
			htmlCode.Append("<ul class=\"title-tab\">");
			List<string> listTemp = new List<string>();
			string baseUrl = "/" + serialSpell + "/" + year + "/";
			foreach (KeyValuePair<CarNewsType, string> entity in titleTag)
			{
				//所有文章
				if (entity.Key == CarNewsType.wenzhang)
				{
					if (entity.Key == _CarNewsType)
						listTemp.Add(string.Format("<li class=\"current\"><a href=\"{0}{1}/\">{2}</a><em>|</em></li>", baseUrl, entity.Key.ToString(), entity.Value));
					else
						listTemp.Add(string.Format("<li><a href=\"{0}{1}/\">{2}</a><em>|</em></li>", baseUrl, entity.Key.ToString(), entity.Value));
					continue;
				}

				if (entity.Key == _CarNewsType)
				{
					if (_CurrentNewsCount > 0)
						listTemp.Add(string.Format("<li class=\"current\"><a href=\"{1}{2}/\">{0}</a><em>|</em></li>", entity.Value, baseUrl, entity.Key.ToString(), _CurrentNewsCount.ToString()));
					else
						listTemp.Add(string.Format("<li class=\"current\"><a>{0}</a><em>|</em></li>", entity.Value));
					continue;
				}
				int newsCount = newsBll.GetSerialNewsCount(serialId, year, entity.Key);
				if (newsCount > 0)
					listTemp.Add(string.Format("<li><a href=\"{0}{1}/\">{2}</a><em>|</em></li>", baseUrl, entity.Key.ToString(), entity.Value, newsCount.ToString()));
				//else
				//    htmlCode.AppendFormat("<li><a class=\"nolink\">{0}</a></li>", entity.Value);
			}
			if (listTemp.Count > 0) listTemp[listTemp.Count - 1] = listTemp[listTemp.Count - 1].Replace("<em>|</em>", "");
			htmlCode.Append(string.Concat(listTemp.ToArray()));
			htmlCode.Append("</ul>");
			htmlCode.Append("</div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成新闻头
		/// </summary>
		/// <returns></returns>
		private string RenderXinWenTitle()
		{
			string title = "新闻";
			if (newsType == "hangqing") title = "行情";
			return string.Format("<h3><span>{0} {2}款{1}</span></h3>", serialShowName, title, year);
		}

		private string RenderNewsList()
		{
			StringBuilder htmlCode = new StringBuilder();
			int newsCount;
			DataSet newsDs = null;

			if (newsType == "wenzhang")
			{
				List<int> carTypeIdList = new List<int>();
				carTypeIdList.Add((int)CarNewsType.shijia);
				carTypeIdList.Add((int)CarNewsType.daogou);
				carTypeIdList.Add((int)CarNewsType.yongche);
				carTypeIdList.Add((int)CarNewsType.gaizhuang);
				carTypeIdList.Add((int)CarNewsType.anquan);
				carTypeIdList.Add((int)CarNewsType.xinwen);
				carTypeIdList.Add((int)CarNewsType.pingce);
				carTypeIdList.Add((int)CarNewsType.treepingce);

				newsDs = new CarNewsBll().GetSerialYearNewsAllData(serialId, year, carTypeIdList, pageSize, pageIndex, out newsCount);
			}
			else
				newsDs = new CarNewsBll().GetSerialYearNewsAllData(serialId, year, _CarNewsType, pageSize, pageIndex, out newsCount);

			_CurrentNewsCount = newsCount;
			if (newsDs != null && newsDs.Tables.Count > 0 && newsDs.Tables[0].Rows.Count > 0)
			{

				int pageCount = newsCount / pageSize + (newsCount % pageSize == 0 ? 0 : 1);

				if (pageIndex > pageCount)
					pageIndex = pageCount;

				htmlCode.AppendLine(" <div class=\"tuwenlistbox\"><ul>");
				//排重需要
				List<int> list = new List<int>();
				foreach (DataRow row in newsDs.Tables[0].Rows)
				{
					string newsTitle = CommonFunction.NewsTitleDecode(row["title"].ToString());
					int newsId = ConvertHelper.GetInteger(row["cmsnewsid"]);
					string newsUrl = row["filepath"].ToString();
					if (_CarNewsType == CarNewsType.pingce)
						newsUrl = "/" + serialSpell + "/pingce/p" + ConvertHelper.GetInteger(row["cmsnewsid"].ToString()) + "/";
					DateTime publishTime = Convert.ToDateTime(row["publishtime"]);
					//string newsContent = Convert.ToString(row["content"]);
					string from = row["sourceName"].ToString();
					string author = row["author"].ToString();
					int commentNum = ConvertHelper.GetInteger(row["CommentNum"]);
					//string firstPicUrl = string.Empty;
					//if (newsType != "hangqing")
					//	firstPicUrl = ConvertHelper.GetString(row["FirstPicUrl"]);
					string imageUrl = string.Empty;
					if (newsType != "hangqing")
					{
						string picUrl = ConvertHelper.GetString(row["Picture"]);
						imageUrl = (!string.IsNullOrEmpty(picUrl) && picUrl.IndexOf("/not") < 0) ? picUrl : ConvertHelper.GetString(row["FirstPicUrl"]);
					}

					string newsContent = Convert.ToString(row["content"]);

					//排重
					if (list.Contains(newsId))
						continue;
					list.Add(newsId);

					if (string.IsNullOrEmpty(imageUrl))
					{
						htmlCode.Append("<li><div class=\"textcont textcontauto\">");
					}
					else
					{
						imageUrl = imageUrl.Replace("/bitauto/", "/newsimg_210_w0_1/bitauto/")
							.Replace("/autoalbum/", "/newsimg_210_w0_1/autoalbum/");

						htmlCode.AppendFormat("<li> <a href=\"{0}\" class=\"img\" target=\"_blank\"> <img src=\"{1}\" width=\"210\" height=\"140\" /></a>", newsUrl, imageUrl);
						htmlCode.Append("<div class=\"textcont\">");
					}
					htmlCode.AppendFormat("<h3><a href=\"{0}\" target=\"_blank\">{1}</a></h3>", newsUrl, newsTitle);
					htmlCode.AppendFormat("<p class=\"info\"><span>来源：<em>{1}</em></span>{2}<span>{0}</span><em class=\"huifu\">{3}</em></p>",
						publishTime.ToString("yyyy-MM-dd HH:mm"),
						from,
						!string.IsNullOrEmpty(author) ? "<span>作者：<em>" + author + "</em></span>" : "",
						commentNum);
					htmlCode.AppendFormat("<p class=\"text\">{0} <a href=\"{1}\" target=\"_blank\">详细&gt;&gt;</a></p>", newsContent, newsUrl);
					htmlCode.Append("</div>");
					htmlCode.Append("</li>");
				}

				//生成页号导航
				if (pageCount > 1)
				{
					string baseUrl = "/" + serialSpell + "/" + year + "/" + newsType + "/{0}/";

					this.AspNetPager1.UrlRewritePattern = baseUrl;
					this.AspNetPager1.RecordCount = newsCount;
					this.AspNetPager1.PageSize = pageSize;
					this.AspNetPager1.CurrentPageIndex = pageIndex;
					this.AspNetPager1.Visible = true;
					this.AspNetPager1.DotShowLimit = 8;
					this.AspNetPager1.ExternalConfigPattern = BitAuto.Controls.Pager.PagerExternalConfigPattern.Apply;
					this.AspNetPager1.ExternalConfigURL = Server.MapPath("~/config/PagerConfig.xml");
				}
				htmlCode.AppendLine("</ul></div>");
			}
			else
			{
				htmlCode.Append("<div class=\"no-txt-box no-txt-m\">");
				htmlCode.Append("<p class=\"tit\">很抱歉，该车型暂无文章！</p>");
				htmlCode.Append("<p>我们正在努力更新，请查看其他...</p>");
				htmlCode.AppendFormat("<p><a href=\"/{0}/\" target=\"_blank\">返回{1}频道&gt;&gt;</a></p>", serialSpell, serialShowName);
				htmlCode.Append("</div>");
			}
			return htmlCode.ToString();
		}

		///// <summary>
		///// 得到品牌下的其他子品牌
		///// </summary>
		///// <returns></returns>
		//protected string GetBrandOtherSerial()
		//{
		//    Car_SerialEntity cse = new Car_SerialBll().GetSerialInfoEntity(serialId);
		//    return new Car_SerialBll().GetBrandOtherSerialList(cse);
		//}

		/// <summary>
		/// 得到品牌下的其他子品牌
		/// </summary>
		/// <returns></returns>
		protected string GetBrandOtherSerial()
		{
			List<CarSerialPhotoEntity> carSerialPhotoList = new Car_BrandBll().GetCarSerialPhotoListByCBID(serialBase.BrandId, false);

			carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

			if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
			{
				return "";
			}

			int forLastCount = 0;
			foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
			{
				if (entity.SerialLevel == "概念车" || entity.SerialId == serialId)
				{
					continue;
				}
				forLastCount++;
			}

			StringBuilder contentBuilder = new StringBuilder(string.Empty);
			string serialUrl = "<a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}</a>";
			int index = 0;
			foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
			{
				bool IsExitsUrl = true;
				if (entity.SerialLevel == "概念车" || entity.SerialId == serialId)
				{
					continue;
				}
				string priceRang = base.GetSerialPriceRangeByID(entity.SerialId);
				if (entity.SaleState == "待销")
				{
					IsExitsUrl = false;
					priceRang = "未上市";
				}
				else if (priceRang.Trim().Length == 0)
				{
					IsExitsUrl = false;
					priceRang = "暂无报价";
				}
				if (IsExitsUrl)
				{
					priceRang = string.Format("<a href='http://price.bitauto.com/brand.aspx?newbrandid={0}'>{1}</a>", entity.SerialId, priceRang);
				}
				string tempCsSeoName = string.IsNullOrEmpty(entity.CS_SeoName) ? entity.ShowName : entity.CS_SeoName;
				index++;
				contentBuilder.AppendFormat("<li>{0}<span class=\"dao\">{1}</span></li>"
					, string.Format(serialUrl, entity.CS_AllSpell, tempCsSeoName), priceRang
					 );
			}

			StringBuilder brandOtherSerial = new StringBuilder(string.Empty);
			if (contentBuilder.Length > 0)
			{
				brandOtherSerial.Append("<div class=\"side_title\">");
				brandOtherSerial.AppendFormat("<h4><a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}其他车型</a></h4>",
					serialBase.Brand.AllSpell, serialBase.Brand.Name);
				brandOtherSerial.Append("</div>");

				brandOtherSerial.Append("<ul class=\"text-list\">");

				brandOtherSerial.Append(contentBuilder.ToString());

				brandOtherSerial.Append("</ul>");
			}

			return brandOtherSerial.ToString();
		}

		/// <summary>
		/// 取子品牌对比排行数据
		/// </summary>
		/// <returns></returns>
		private void MakeHotSerialCompare()
		{
			Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = new Car_SerialBll().GetSerialCityCompareList(serialId, HttpContext.Current);
			List<string> htmlList = new List<string>();
			string compareBaseUrl = "/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?count=3&csids=" + serialId + ",";
            string serialCompareForPkUrl = "/duibi/" + serialId + "-";
            if (carSerialBaseList != null && carSerialBaseList.ContainsKey("全国"))
			{
				List<Car_SerialBaseEntity> serialCompareList = carSerialBaseList["全国"];
				htmlList.Add("<div class=\"line-box\" id=\"serialHotCompareList\">");

				htmlList.Add("<div class=\"side_title\">");
				htmlList.Add("<h4><a rel=\"nofollow\" href=\"/chexingduibi/\" target=\"_blank\">大家都用他和谁比</a></h4>");
				htmlList.Add("</div>");


				//htmlList.Add("<div id=\"rank_model_box\" class=\"ranking_list\">");
				htmlList.Add("<ul class=\"text-list\">");

				for (int i = 0; i < serialCompareList.Count; i++)
				{
					Car_SerialBaseEntity carSerial = serialCompareList[i];
					htmlList.Add("<li>");
                    htmlList.Add(string.Format("<a href=\"" + serialCompareForPkUrl + carSerial.SerialId + "/\" target=\"_blank\">{0}<em class=\"vs\">VS</em>{1}</a>",
						BitAuto.Utils.StringHelper.SubString(serialShowName, 10, false),
						carSerial.SerialShowName.Trim()));
					htmlList.Add("</li>");
				}

				htmlList.Add("</ul>");
				htmlList.Add("<div class=\"clear\"></div>");
				htmlList.Add("</div>");
			}

			carCompareHtml = String.Concat(htmlList.ToArray());
		}

		/// <summary>
		/// 子品牌还关注
		/// </summary>
		/// <returns></returns>
		private void MakeSerialToSerialHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(serialId, 6);
			if (lsts.Count > 0)
			{
				int loop = 0;
				foreach (EnumCollection.SerialToSerial sts in lsts)
				{
					string csName = sts.ToCsShowName.ToString();
					string shortName = StringHelper.SubString(csName, 12, true);
					if (shortName.StartsWith(csName))
						shortName = csName;

					loop++;
					htmlCode.Append("<li>");
					htmlCode.AppendFormat("<a target=\"_blank\" href=\"/{0}/\"><img src=\"{1}\" width=\"90\" height=\"60\"></a>",
						sts.ToCsAllSpell.ToString().ToLower(),
						 sts.ToCsPic.ToString());
					if (shortName != csName)
						htmlCode.AppendFormat("<p><a target=\"_blank\" href=\"/{0}/\" title=\"{1}\">{2}</a></p>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName, shortName);
					else
						htmlCode.AppendFormat("<p><a target=\"_blank\" href=\"/{0}/\">{1}</a></p>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName);
					htmlCode.AppendFormat("<p><span>{0}</span></p>", StringHelper.SubString(sts.ToCsPriceRange.ToString(), 14, false));
					htmlCode.AppendFormat("</li>");
				}
			}
			serialToSeeHtml = htmlCode.ToString();
		}
	}
}