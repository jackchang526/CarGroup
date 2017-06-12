using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Das;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.Beyond.Caching.RefreshCache;

namespace BitAuto.CarChannel.CarchannelWeb.PageSerial
{
	public partial class CsRegion : PageBase
	{
		protected int serialId;			//子品牌ID
		protected string serialSpell;	//子品牌全拼
		protected string serialName;	//子品牌名称
		protected string serialShowName;//子品牌显示名
		protected string serialSEOName; //子品牌SEO名称
		protected int cityId;				//城市ID
		protected string citySpell;		//城市拼音
		protected string cityName;		//城市名称
		private int cityLevel;			//城市级别
		protected string serialReferPrice = string.Empty;//子品牌官方指导价
		protected string serialExhaust = string.Empty;//子品牌排量
		protected string serialTransmission = string.Empty;//变速箱
		protected string CsHead = string.Empty;		//子品牌综述页头
		protected string errorStr = string.Empty;
		protected string cityLogoStyle = string.Empty;		//城市Logo样式
		protected SerialEntity serialEntity;
		protected Dictionary<string, Dictionary<string, string>> _AreaCity;

		protected string cityNewsHtml = String.Empty;
		protected string dealserNewsHtml = String.Empty;
		protected string compareHtml = String.Empty;
		protected string priceHtml = String.Empty;
		protected string ucarHtml = String.Empty;
		protected bool hasSaleData = false;

		#region 面包削广告

		// private readonly string serialMianBaoADList = ",2601,1825,1703,2731,2614,2388,2862,2945,2608,1930,1828,2410,1835,1833,1827,1839,1834,2012,1838,2712,2874,2764,3167,2612,1574,2334,1569,1575,1660,2780,1661,2676,2589,2288,2057,2859,3269,1636,1650,2866,1635,3152,3153,3164,2895,1649,3263,1648,2865,2833,2683,2767,3023,2587,2857,3023,3103,3151,2713,1618,1605,1619,1617,1740,2733,1925,2742,2604,1736,1733,1751,1851,2773,2867,3129,3210,3209,1755,2749,1654,2708,2251,2585,2692,2949,2420,1594,2618,1595,1596,2834,2971,2335,2836,2907,3022,2929,2748,1684,1683,2283,1678,2719,1681,1682,3086,1622,2625,2369,1787,1607,2194,2950,2711,1785,1789,2747,1784,3250,3251,3252,1667,1666,2703,2417,2761,2381,2932,2714,1863,1905,1909,1765,1871,2848,1879,2701,1977,2407,1873,1914,2566,2944,2938,2677,1918,3046,3261,2964,3368,2957,2759,2963,3360,2693,2182,1969,3186,2709,2366,2403,2425,2741,3348,1978,2906,3061,3304,2633,2846,1633,2336,3026,2576,1692,3301,";
		private readonly string serialMianBaoADList = ",1909,1796,1991,2731,3323,3381,2409,";
		protected string SerialMianBaoAD = "";

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(15);
			GetParameter();
			base.MakeSerialTopADCode(serialId);
			InitData();
			//生成新闻
			RenderSerialCityNews();
			//RenderDealerCityNews();  //生成经销商的新闻，被易湃的合作联盟取代
			CsHead = base.GetCommonNavigation("CsCity", serialId).Replace("#CityID#", cityId.ToString()).Replace("#CityName#", cityName.ToString()).Replace("#CitySpell#", citySpell.ToString());
			RenderRegionHtml();
			// 面包屑广告
			// modified by chengl Dec.1.2011
			// if (cityName.Length < 4)
			if (true)
			{
				// if (serialMianBaoADList.IndexOf("," + serialId.ToString() + ",") >= 0)
				// if (true)
				// modified by chengl Jun.21.2012

				// modified by cheng Jul.10.2012
				SerialMianBaoAD = "<div id=\"divSerialSummaryMianBaoAD\" class=\"top_ad02\"><ins id=\"div_ba10f730-0c13-4dcf-aa81-8b5ccafc9e21\" type=\"ad_play\" adplay_IP=\"\" adplay_AreaName=\"\"  adplay_CityName=\"\"    adplay_BrandID=\"" + serialId.ToString() + "\"  adplay_BrandName=\"\"  adplay_BrandType=\"\"  adplay_BlockCode=\"ba10f730-0c13-4dcf-aa81-8b5ccafc9e21\"> </ins></div>";
				//if(serialMianBaoADList.IndexOf("," + serialId.ToString() + ",") >= 0)
				//{
				//    SerialMianBaoAD = "<div id=\"divSerialSummaryMianBaoAD\" class=\"top_ad02\"><a target=\"_blank\" href=\"http://news.bitauto.com/diaocha/20120621/0905735407.html\"><img alt=\"\" src=\"http://gimg.bitauto.com/ResourceFiles/0/0/146/20120621044141180.gif\"></a></div>";
				//}
				//else
				//{
				//    SerialMianBaoAD = "<div id=\"divSerialSummaryMianBaoAD\" class=\"top_ad02\"><a target=\"_blank\" href=\"http://market.bitauto.com/gmac2010/index.aspx\"><img alt=\"\" src=\"http://gimg.bitauto.com/ResourceFiles/0/0/108/20120213045621321.gif\"></a></div>";
				//}
			}
		}

		private void GetParameter()
		{
			serialSpell = Convert.ToString(Request.QueryString["serial"]);
			if (String.IsNullOrEmpty(serialSpell))
				Response.Redirect("http://car.bitauto.com");
			else
			{ serialSpell = serialSpell.ToLower(); }

			citySpell = Convert.ToString(Request.QueryString["city"]);
			if (String.IsNullOrEmpty(citySpell))
				Response.Redirect("http://car.bitauto.com");
			else
				citySpell = citySpell.ToLower();

			Dictionary<string, int> serialDic = AutoStorageService.GetSerialSpellDic();
			if (!serialDic.ContainsKey(serialSpell))
				Response.Redirect("http://car.bitauto.com");

			serialId = serialDic[serialSpell];
			if (serialId <= 0)
			{ Response.Redirect("http://car.bitauto.com"); }

			//取子品牌信息
			serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);

			// modified by chengl Mar.2.2010
			if (serialEntity == null || serialEntity.Id == 0)
				Response.Redirect("http://car.bitauto.com");
			serialName = serialEntity.Name;
			serialShowName = serialEntity.ShowName;
			serialSEOName = serialEntity.SeoName;

			cityLogoStyle = "bt_logo_loca";
			Dictionary<string, City> cityDic = AutoStorageService.GetCitySpellDic();
			if (cityDic.ContainsKey(citySpell))
			{
				cityId = cityDic[citySpell].CityId;
				cityLevel = cityDic[citySpell].Level;
				cityName = cityDic[citySpell].CityName;
				if (cityName.Length > 2)
					cityLogoStyle += cityName.Length.ToString();
			}
			else
			{
				// 不存在城市 404
				Response.Redirect("http://car.bitauto.com/car/404error.aspx?info=城市不存在");
			}
		}

		/// <summary>
		/// 生成Html代码
		/// </summary>
		/// <returns></returns>
		private void RenderRegionHtml()
		{
			//生成报价块的Html
			RenderPriceHtml();
			// 活动栏目展示(邢松)
			//htmlList.Add(GetActiveSpan());
			//htmlCode.AppendLine("<script type=\"text/javascript\"src=\"http://life.bitauto.com/huodong/recommend/forum_recommend.aspx?serial=" + serialId + "&city=" + cityId + "&count=3&source=goumai\"></script>");
			// htmlCode.AppendLine("<script charset=\"gb2312\" type=\"text/javascript\"src=\"http://life.bitauto.com/huodong/recommend/forum_recommend.aspx?serial=2886&city=201&count=6&source=goumai\"></script>");

			//是否要加子品牌销量曲线
			hasSaleData = new ProduceAndSellDataBll().HasSerialData(serialId);
			//RenderUCarInfo();
		}
		/// <summary>
		/// 生成报价块Html
		/// </summary>
		/// <param name="htmlCode"></param>
		private void RenderPriceHtml()
		{
			List<string> htmlList = new List<string>();

			htmlList.Add("<h3><span><a href=\"http://price.bitauto.com/brand.aspx?newbrandid=" + serialId + "&citycode=" + cityId + "\" target=\"_blank\">" + cityName + serialShowName + "报价</a></span>");
			// htmlCode.Append("<div class=\"city_list\">" + cityName + "</div>");
			// modified by chengl Jun.24.2010
			htmlList.Add("<div class=\"car0623_01\">");
			// modified by chengl May.25.2012
			// htmlList.Add("<a href=\"http://ask.bitauto.com/browse/" + serialId + "/\" target=\"_blank\" class=\"more4\">买前咨询</a>");
            //20130412 edit anh
			//htmlList.Add("<a href=\"http://car.bitauto.com/" + serialSpell + "/baojia/\" target=\"_blank\" class=\"more4\">预约试驾</a>");
            htmlList.Add(string.Format("<a href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\" class=\"more4\">预约试驾</a>", serialId));
			htmlList.Add("<a id=\"LinkForBaaAttention\" href=\"http://i.bitauto.com/baaadmin/car/guanzhu_" + serialId + "/\" target=\"_blank\" class=\"more3\">加关注</a>");
			// htmlList.Add("<a href=\"http://i.bitauto.com/baaadmin/car/goumai_" + serialId + "/\" target=\"_blank\" class=\"more2\">计划购买</a>");
			// htmlCode.AppendLine("<a href=\"http://j.bitauto.com/" + serialSpell + "/tuangou/\" target=\"_blank\" class=\"more0\">参加团购</a>");
			htmlList.Add("</div>");
			// modified end
			htmlList.Add("<div class=\"city_list\"></div>");
			htmlList.Add("</h3>");
			htmlList.Add("<div>");
			htmlList.Add("<table class=\"table_dealer\">");
			htmlList.Add("<thead><tr>");
			htmlList.Add("<th width=\"300\">车型名称</th>");
			htmlList.Add("<th width=\"90\">排量</th>");
			htmlList.Add("<th width=\"90\">变速器</th>");
			htmlList.Add("<th width=\"170\" style=\"text-align:right\">厂家指导价</th>");
			htmlList.Add("<th width=\"130\" style=\"text-align:right\">参考成交价</th>");
			//htmlList.Add("<th width=\"60\">走势区间</th>");
			htmlList.Add("<th width=\"65\" style=\"text-align:center\">报价详情</th>");
			htmlList.Add("</tr></thead>");
			htmlList.Add("<tbody>");

			List<string> exhausList = new List<string>();
			List<string> transList = new List<string>();

			// DataSet ds = new Car_SerialBll().GetCarExtendInfoBySerial(serialId,cityId);
			DataSet ds = new DataSet();
			try
			{
				ds = new Car_SerialBll().GetCarExtendInfoBySerial(serialId, cityId);
			}
			catch (Exception ex)
			{
				errorStr = serialId.ToString() + " " + cityId.ToString() + " " + ex.ToString();
			}
			double maxPrice = Double.MinValue;
			double minPrice = Double.MaxValue;
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				List<DataRow> rowList = new List<DataRow>();
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					rowList.Add(row);
				}
				rowList.Sort(NodeCompare.CompareRegionPrice);
				foreach (DataRow row in rowList)
				{
					string yearType = row["Car_YearType"].ToString().Trim();
					if (yearType.Length > 0)
						yearType += "款 ";
					string carName = yearType + row["cs_ShowName"].ToString().Trim() + row["Car_Name"].ToString().Trim();

					string exhaust = Convert.ToString(row["Engine_Exhaust"]);
					if (!exhausList.Contains(exhaust))
						exhausList.Add(exhaust);

					string transmissionType = Convert.ToString(row["UnderPan_TransmissionType"]);
					if (!transList.Contains(transmissionType))
						transList.Add(transmissionType);

					string referPrice = Convert.ToString(row["car_ReferPrice"]);
					string price = Convert.ToString(row["Price"]);
					string carId = Convert.ToString(row["Car_Id"]);
					string carUrl = Convert.ToString(row["PriceUrl"]);
					string tendChartUrl = Convert.ToString(row["PriceTendUrl"]);

					//计算官方指导价
					double dreferPrice = 0.0;
					bool isDouble = Double.TryParse(referPrice.Replace("万", ""), out dreferPrice);
					if (isDouble)
					{
						if (dreferPrice > maxPrice)
							maxPrice = dreferPrice;
						if (dreferPrice < minPrice)
							minPrice = dreferPrice;
					}

					htmlList.Add("<tr>");
					htmlList.Add("<td><strong><a target=\"_blank\" href=\"" + carUrl + "\">");
					htmlList.Add(carName + "</a></strong></td>");
					htmlList.Add("<td>" + exhaust + "</td>");
					htmlList.Add("<td>" + transmissionType + "</td>");

					// modified by chengl Dec.28.2009 for calculator
					string calculator = "<a title=\"购车费用计算\" class=\"icon_cal\" target=\"_blank\" href=\"http://car.bitauto.com/gouchejisuanqi/?carid=" + carId + "\" /></a>";
					if (referPrice.Trim().Length == 0)
						htmlList.Add("<td class=\"glo\">暂无" + calculator + "</td>");
					else
						htmlList.Add("<td class=\"glo\"><a title=\"购车费用计算\" target=\"_blank\" href=\"http://car.bitauto.com/gouchejisuanqi/?carid=" + carId + "\" />" + referPrice + "万</a>" + calculator + "</td>");



					if (String.IsNullOrEmpty(price))
					{
						htmlList.Add("<td class=\"dealer_price\"><a href=\"" + carUrl + "\" target=\"_blank\">暂无报价</a></td>");
						//htmlList.Add("<td class=\"car0507_01\"><a class=\"nolink\">走势图</a></td>");
					}
					else
					{
						htmlList.Add("<td class=\"dealer_price\"><a href=\"" + carUrl + "\" target=\"_blank\">" + price + "</a></td>");
						// htmlList.Add("<td class=\"car0507_01\"><a href=\"javascript:void(0);\" onclick=\"window.open('" + tendChartUrl + "',null,'height=240,width=400,status=yes,toolbar=no,scrollbars=no');\">走势图</a></td>");
						//htmlList.Add("<td class=\"car0507_01\"><a href=\"javascript:void(0);\" onclick=\"openwindow('" + tendChartUrl + "','',450,280);\">走势图</a></td>");
					}
					htmlList.Add("<td class=\"mid\"><a href=\"" + carUrl + "\" target=\"_blank\">查看</a></td>");
					htmlList.Add("</tr>");
				}
			}

			if (maxPrice == Double.MinValue && minPrice == Double.MaxValue)
				serialReferPrice = "暂无";
			else
			{
				serialReferPrice = minPrice + "万-" + maxPrice + "万";
			}

			htmlList.Add("</tbody>");
			htmlList.Add("</table></div>");
			htmlList.Add("<div class=\"more\"><a href=\"http://price.bitauto.com/brand.aspx?newbrandid=" + serialId + "&citycode=" + cityId + "\" target=\"_blank\">更多>></a></div>");

			//排量与变速箱
			serialExhaust = String.Join(",", exhausList.ToArray());
			serialTransmission = String.Join(",", transList.ToArray());
			priceHtml = String.Concat(htmlList.ToArray());

		}
        #region 展示行情改为展示降价信息/***/
        /*
        /// <summary>
		/// 生成新闻块Html
		/// </summary>
		/// <param name="htmlCode"></param>
		private void RenderSerialCityNews()
		{
			StringBuilder htmlCode = new StringBuilder();
			int newsNum = 8;

			// 行情链接到对应的城市行情页
			htmlCode.AppendLine("<h3><span><a href=\"http://car.bitauto.com/" + serialSpell + "/hangqing/" + citySpell + "/\" target=\"_blank\">" + cityName + serialShowName + "行情</a></span></h3>");
			htmlCode.AppendLine("<ul class=\"list_date\">");


			int newsCounter = 0;

			int titleLength = 34;
			string shortTitle = "";
			Dictionary<int, City> to30CityDic = AutoStorageService.GetCityTo30Dic();			//中心城的对应关系
			List<int> newsIdList = new List<int>();		//补充过的文章可能重复，用此列表排重
			List<News> newsList = null,
				zhihuanList = null;

			#region 置换新闻
			if (to30CityDic.ContainsKey(cityId))
			{
				zhihuanList = new CarNewsBll().GetTopCityZhiHuanNews2(serialId, cityId, to30CityDic[cityId].CityId, 2);
			}
			else
			{
				zhihuanList = new CarNewsBll().GetTopCityZhiHuanNews2(serialId, cityId, 2);
			}
			if (zhihuanList != null && zhihuanList.Count > 0)
			{
				newsNum = newsNum - zhihuanList.Count;
			}
			#endregion

			if (to30CityDic.ContainsKey(cityId))
			{
				newsList = new CarNewsBll().GetTopCityNews2(serialId, cityId, to30CityDic[cityId].CityId, newsNum);
			}
			else
			{
				newsList = new CarNewsBll().GetTopCityNews2(serialId, cityId, newsNum);
			}

			try
			{
				if (newsList != null && newsList.Count > 0)
				{
					foreach (News news in newsList)
					{
						int newsId = ConvertHelper.GetInteger(news.NewsId);
						if (newsIdList.Contains(newsId))
							continue;
						newsCounter++;
						newsIdList.Add(newsId);
						string newsTitle = Convert.ToString(news.Title);
						newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
						shortTitle = StringHelper.SubString(newsTitle, titleLength, true);
						string filePath = Convert.ToString(news.PageUrl);
						if (shortTitle != newsTitle)
							htmlCode.AppendLine("<li><a href=\"" + filePath + "\" title=\"" + newsTitle + "\" target=\"_blank\">" + newsTitle + "</a></li>");
						else
							htmlCode.AppendLine("<li><a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a></li>");

						if (newsCounter == newsNum)
							break;
					}
				}
				#region 置换
				if (zhihuanList != null && zhihuanList.Count > 0)
				{
					string tag = string.Format("<span><a href=\"http://car.bitauto.com/{0}/zhihuan/{1}/\" target=\"_blank\">置换</a> | </span> ", serialSpell, citySpell);
					foreach (News news in zhihuanList)
					{
						string newsTitle = Convert.ToString(news.Title);
						newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
						shortTitle = StringHelper.SubString(newsTitle, titleLength, true);
						string filePath = Convert.ToString(news.PageUrl);

						if (shortTitle != newsTitle)
							htmlCode.AppendLine("<li>" + tag + "<a href=\"" + filePath + "\" title=\"" + newsTitle + "\" target=\"_blank\">" + newsTitle + "</a></li>");
						else
							htmlCode.AppendLine("<li>" + tag + "<a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a></li>");
					}
				}
				#endregion
			}
			catch
			{ }

			htmlCode.AppendLine("</ul>");
			// modified by chengl Nov.3.2011
			// 行情链接到对应的城市行情页
			// htmlCode.AppendLine("<div class=\"more\"><a href=\"http://" + citySpell + ".bitauto.com/car/" + serialSpell + "/hangqing/\" target=\"_blank\">更多>></a></div>");
			htmlCode.AppendLine("<div class=\"more\"><a href=\"http://car.bitauto.com/" + serialSpell + "/hangqing/" + citySpell + "/\" target=\"_blank\">更多>></a></div>");
			cityNewsHtml = htmlCode.ToString();

			htmlCode.Length = 0;

			Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = new Dictionary<string, List<Car_SerialBaseEntity>>();
			carSerialBaseList = new Car_SerialBll().GetSerialCityCompareList(serialId, HttpContext.Current);

			string compareBaseUrl = "http://car.bitauto.com/car/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?count=3&csids=" + serialId + ",";
			if (carSerialBaseList != null && carSerialBaseList.Count > 0 && carSerialBaseList.ContainsKey("全国"))
			{
				List<Car_SerialBaseEntity> serialCompareList = carSerialBaseList["全国"];
				htmlCode.Append("<h3><span>网友都用它和谁比</span></h3>");
				htmlCode.Append("<div class=\"more\"><a target=\"_blank\" href=\"http://car.bitauto.com/chexingduibi/\">车型对比>></a></div>");
				htmlCode.Append("<div id=\"rank_depreciate_box\">");
				htmlCode.Append("<div class=\"this\">" + cityName + " " + serialShowName + " VS</div>");
				htmlCode.Append("<ol class=\"hot_ranking\">");

				for (int i = 0; i < serialCompareList.Count; i++)
				{
					Car_SerialBaseEntity carSerial = serialCompareList[i];
					htmlCode.Append("<li><a target=\"_blank\" href=\"http://car.bitauto.com/" + carSerial.SerialNameSpell.Trim().ToLower() + "/\" >");
					htmlCode.Append(carSerial.SerialShowName.Trim() + "</a>");
					htmlCode.Append("<span><a href=\"" + compareBaseUrl + carSerial.SerialId + "\" target=\"_blank\">对比</a></span></li>");
				}

				htmlCode.Append("</ol></div>");
				htmlCode.Append("<div class=\"l\"><script type=\"text/javascript\" src=\"http://index.bitauto.com/Interface/GetSerialCompareSort.aspx?csid=" + serialId + "&cityid=0&dept=carChannel\"></script></div>");
			}


			compareHtml = htmlCode.ToString();
		}
        	*/
        #endregion

        /// <summary>
        /// 生成新闻块Html
        /// </summary>
        /// <param name="htmlCode"></param>
        private void RenderSerialCityNews()
        {
            StringBuilder htmlCode = new StringBuilder();
            int newsNum = 8;

            // 行情链接到对应的城市降价页
            htmlCode.AppendFormat("<h3><span><a href=\"http://jiangjia.bitauto.com/nb{0}_c{1}/\" target=\"_blank\">{2}{3}降价行情</a></span></h3>", serialId, cityId, cityName, serialShowName);
            htmlCode.AppendLine("<ul class=\"list_date\">");


            int newsCounter = 0;

            int titleLength = 34;
            string shortTitle = string.Empty;
            Dictionary<int, City> to30CityDic = AutoStorageService.GetCityTo30Dic();			//中心城的对应关系
            List<int> newsIdList = new List<int>();		//补充过的文章可能重复，用此列表排重
            List<News> newsList = null,
                zhihuanList = null;
            /*
                    #region 置换新闻
                    if (to30CityDic.ContainsKey(cityId))
                    {
                        zhihuanList = new CarNewsBll().GetTopCityZhiHuanNews2(serialId, cityId, to30CityDic[cityId].CityId, 2);
                    }
                    else
                    {
                        zhihuanList = new CarNewsBll().GetTopCityZhiHuanNews2(serialId, cityId, 2);
                    }
                    if (zhihuanList != null && zhihuanList.Count > 0)
                    {
                        newsNum = newsNum - zhihuanList.Count;
                    }
                    #endregion
            */
            if (to30CityDic.ContainsKey(cityId))
            {
                newsList = new CarNewsBll().GetSerialJiangJiaTopNews(serialId, cityId, to30CityDic[cityId].CityId, newsNum, 3);
            }
            else
            {
                newsList = new CarNewsBll().GetSerialJiangJiaTopNews(serialId, cityId, newsNum, 3);
            }

            try
            {
                if (newsList != null && newsList.Count > 0)
                {
                    foreach (News news in newsList)
                    {
                        int newsId = ConvertHelper.GetInteger(news.NewsId);
                        if (newsIdList.Contains(newsId))
                            continue;
                        newsCounter++;
                        newsIdList.Add(newsId);
                        string newsTitle = Convert.ToString(news.Title);
                        newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
                        shortTitle = StringHelper.SubString(newsTitle, titleLength, true);
                        string filePath = Convert.ToString(news.PageUrl);
                        if (shortTitle != newsTitle)
                            htmlCode.AppendFormat("<li><a href=\"{0}\" title=\"{1}\" target=\"_blank\">{1}</a></li>", filePath, newsTitle);
                        else
                            htmlCode.AppendFormat("<li><a href=\"{0}\" target=\"_blank\">{1}</a></li>", filePath, newsTitle);

                        if (newsCounter == newsNum)
                            break;
                    }
                }
                /*
                #region 置换
                if (zhihuanList != null && zhihuanList.Count > 0)
                {
                    string tag = string.Format("<span><a href=\"http://car.bitauto.com/{0}/zhihuan/{1}/\" target=\"_blank\">置换</a> | </span> ", serialSpell, citySpell);
                    foreach (News news in zhihuanList)
                    {
                        string newsTitle = Convert.ToString(news.Title);
                        newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
                        shortTitle = StringHelper.SubString(newsTitle, titleLength, true);
                        string filePath = Convert.ToString(news.PageUrl);

                        if (shortTitle != newsTitle)
                            htmlCode.AppendLine("<li>" + tag + "<a href=\"" + filePath + "\" title=\"" + newsTitle + "\" target=\"_blank\">" + newsTitle + "</a></li>");
                        else
                            htmlCode.AppendLine("<li>" + tag + "<a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a></li>");
                    }
                }
                #endregion
    */
            }
            catch
            { }

            htmlCode.AppendLine("</ul>");
            // modified by chengl Nov.3.2011
            // 行情链接到对应的城市行情页
            // htmlCode.AppendLine("<div class=\"more\"><a href=\"http://" + citySpell + ".bitauto.com/car/" + serialSpell + "/hangqing/\" target=\"_blank\">更多>></a></div>");
            htmlCode.AppendFormat("<div class=\"more\"><a href=\"http://jiangjia.bitauto.com/nb{0}_c{1}/\" target=\"_blank\">更多>></a></div>", serialId, cityId);
            cityNewsHtml = htmlCode.ToString();

            htmlCode.Length = 0;

            Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = new Dictionary<string, List<Car_SerialBaseEntity>>();
            carSerialBaseList = new Car_SerialBll().GetSerialCityCompareList(serialId, HttpContext.Current);

            string compareBaseUrl = "/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?count=3&csids=" + serialId + ",";
            if (carSerialBaseList != null && carSerialBaseList.Count > 0 && carSerialBaseList.ContainsKey("全国"))
            {
                List<Car_SerialBaseEntity> serialCompareList = carSerialBaseList["全国"];
                htmlCode.Append("<h3><span>网友都用它和谁比</span></h3>");
                htmlCode.Append("<div class=\"more\"><a target=\"_blank\" href=\"http://car.bitauto.com/chexingduibi/\">车型对比>></a></div>");
                htmlCode.Append("<div id=\"rank_depreciate_box\">");
                htmlCode.Append("<div class=\"this\">" + cityName + " " + serialShowName + " VS</div>");
                htmlCode.Append("<ol class=\"hot_ranking\">");

                for (int i = 0; i < serialCompareList.Count; i++)
                {
                    Car_SerialBaseEntity carSerial = serialCompareList[i];
                    htmlCode.Append("<li><a target=\"_blank\" href=\"http://car.bitauto.com/" + carSerial.SerialNameSpell.Trim().ToLower() + "/\" >");
                    htmlCode.Append(carSerial.SerialShowName.Trim() + "</a>");
                    htmlCode.Append("<span><a href=\"" + compareBaseUrl + carSerial.SerialId + "\" target=\"_blank\">对比</a></span></li>");
                }

                htmlCode.Append("</ol></div>");
                htmlCode.Append("<div class=\"l\"><script type=\"text/javascript\" src=\"http://index.bitauto.com/Interface/GetSerialCompareSort.aspx?csid=" + serialId + "&cityid=0&dept=carChannel\"></script></div>");
            }


            compareHtml = htmlCode.ToString();
        }
        
        private void RenderDealerCityNews()
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<ul class=\"list_date\">");
			List<News> newsList = new Car_SerialBll().GetSerialCityNews(serialId, cityId, "cuxiao");
			List<int> newsIdList = new List<int>();		//补充过的文章可能重复，用此列表排重
			////不足要用中心城的补齐
			//if (newsList.Count < newsNum)
			//{
			//    if (to30CityDic.ContainsKey(cityId))
			//    {
			//        List<News> tmpList = serialBll.GetSerialCityNews(serialId, to30CityDic[cityId].CityId, "cuxiao");
			//        newsList.AddRange(tmpList);
			//    }
			//}
			int newsCounter = 0;
			int titleLength = 34;
			int newsNum = 8;
			try
			{
				newsIdList.Clear();
				foreach (News news in newsList)
				{
					int newsId = news.NewsId;
					if (newsIdList.Contains(newsId))
						continue;
					newsIdList.Add(newsId);
					newsCounter++;
					string newsTitle = news.Title;
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
					string shortTitle = StringHelper.SubString(newsTitle, titleLength, true);
					string filePath = news.PageUrl;
					if (shortTitle != newsTitle)
						htmlCode.AppendLine("<li><a href=\"" + filePath + "\" title=\"" + newsTitle + "\" target=\"_blank\">" + shortTitle + "</a></li>");
					else
						htmlCode.AppendLine("<li><a href=\"" + filePath + "\" target=\"_blank\">" + shortTitle + "</a></li>");

					if (newsCounter == newsNum)
						break;
				}
			}
			catch
			{ }
			htmlCode.AppendLine("</ul>");
			dealserNewsHtml = htmlCode.ToString();
		}

		///// <summary>
		///// 生成二手车内容
		///// </summary>
		///// <param name="htmlCode"></param>
		//private void RenderUCarInfo()
		//{
		//    //获取数据
		//    List<UCarInfoEntity> infoList = new Car_SerialBll().GetUCarInfo(serialId);
		//    if (infoList.Count == 0)
		//        return;
		//    List<string> htmlList = new List<string>();
		//    htmlList.Add("<div class=\"line_box\">");
		//    htmlList.Add("<h3><span><a href=\"http://www.taoche.com/buycar/serial/" + serialSpell + "/\" target=\"_blank\">" + serialShowName + "二手车</a></span>");
		//    // htmlCode.AppendLine("<div class=\"city_list\">" + cityName + "</div>");
		//    htmlList.Add("<div class=\"city_list\"></div>");
		//    htmlList.Add("</h3>");
		//    htmlList.Add("<div class=\"table_style\">");
		//    htmlList.Add("<table class=\"table_dealer\">");
		//    htmlList.Add("<thead>");
		//    htmlList.Add("<tr>");
		//    htmlList.Add("<th>车型名称</th>");
		//    htmlList.Add("<th>地区</th>");
		//    htmlList.Add("<th>上牌日期</th>");
		//    htmlList.Add("<th>颜色</th>");
		//    htmlList.Add("<th>行驶里程</th>");
		//    htmlList.Add("<th>价格</th>");
		//    htmlList.Add("<th>经销商</th>");
		//    htmlList.Add("</tr>");
		//    htmlList.Add("</thead>");
		//    htmlList.Add("<tbody>");
		//    //这里生成列表
		//    int counter = 0;
		//    foreach (UCarInfoEntity info in infoList)
		//    {
		//        counter++;
		//        if (counter % 2 == 1)
		//            htmlList.Add("<tr class=\"list\">");
		//        else
		//            htmlList.Add("<tr>");

		//        htmlList.Add("<td><a href=\"" + info.CarlistUrl + "\" target=\"_blank\">" + info.BrandName + " " + info.CarName + "</a></td>");
		//        htmlList.Add("<td><a href=\"" + info.CityUrL + "\" target=\"_blank\">" + info.CityName + "</a></td>");
		//        htmlList.Add("<td>" + info.BuyCarDate + "</td>");
		//        htmlList.Add("<td>" + info.Color + "</td>");
		//        htmlList.Add("<td>" + info.DrivingMileage + "</td>");
		//        htmlList.Add("<td>" + info.DisplayPrice + "</td>");
		//        htmlList.Add("<td><a href=\"" + info.VendorUrl + "\" target=\"_blank\">" + info.VendorFullName + "</a></td>");
		//        htmlList.Add("</tr>");
		//    }
		//    htmlList.Add("</tbody>");
		//    htmlList.Add("</table>");
		//    htmlList.Add("</div>");
		//    htmlList.Add("<div class=\"more\"><a href=\"http://yiche.taoche.com/buycar/carlist.aspx?sbid=" + serialId + "&pvid=-1\" target=\"_blank\">更多>></a></div>");
		//    htmlList.Add("</div>");
		//    ucarHtml = String.Concat(htmlList.ToArray());
		//}
		/// <summary>
		/// 初始化城市
		/// </summary>
		private void InitData()
		{
			string cacheKey_AreaCity = "areacity";
			_AreaCity = (Dictionary<string, Dictionary<string, string>>)CacheManager.GetCachedData(cacheKey_AreaCity);
			if (_AreaCity == null)
			{
				_AreaCity = new Dictionary<string, Dictionary<string, string>>();
				Dictionary<string, string> cityList = new Dictionary<string, string>();
				cityList.Add("beijing", "北京");
				cityList.Add("tianjin", "天津");
				cityList.Add("taiyuan", "太原");
				cityList.Add("shijiazhuang", "石家庄");
				cityList.Add("baoding", "保定");
				cityList.Add("tangshan", "唐山");
				cityList.Add("huhehaote", "呼和浩特");
				_AreaCity.Add("华北地区", cityList);
				cityList = new Dictionary<string, string>();
				cityList.Add("haerbin", "哈尔滨");
				cityList.Add("daqing", "大庆");
				cityList.Add("changchun", "长春");
				cityList.Add("shenyang", "沈阳");
				cityList.Add("dalian", "大连");
				_AreaCity.Add("东北地区", cityList);
				cityList = new Dictionary<string, string>();
				cityList.Add("jinan", "济南");
				cityList.Add("qingdao", "青岛");
				cityList.Add("zibo", "淄博");
				cityList.Add("yantai", "烟台");
				cityList.Add("shanghai", "上海");
				cityList.Add("hangzhou", "杭州");
				cityList.Add("nanjing", "南京");
				cityList.Add("fuzhou", "福州");
				cityList.Add("ningbo", "宁波");
				cityList.Add("jinhua", "金华");
				cityList.Add("suzhou", "苏州");
				cityList.Add("xuzhou", "徐州");
				cityList.Add("wuxi", "无锡");
				cityList.Add("hefei", "合肥");
				cityList.Add("nanchang", "南昌");
				cityList.Add("xiamen", "厦门");
				cityList.Add("quanzhou", "泉州");
				_AreaCity.Add("华东地区", cityList);
				cityList = new Dictionary<string, string>();
				cityList.Add("guangzhou", "广州");
				cityList.Add("shenzhen", "深圳");
				cityList.Add("dongguan", "东莞");
				cityList.Add("foshan", "佛山");
				cityList.Add("nanning", "南宁");
				cityList.Add("haikou", "海口");
				_AreaCity.Add("华南地区", cityList);
				cityList = new Dictionary<string, string>();
				cityList.Add("zhengzhou", "郑州");
				cityList.Add("luoyang", "洛阳");
				cityList.Add("wuhan", "武汉");
				cityList.Add("yichang", "宜昌");
				cityList.Add("changsha", "长沙");
				_AreaCity.Add("华中地区", cityList);
				cityList = new Dictionary<string, string>();
				cityList.Add("xian", "西安");
				cityList.Add("wulumuqi", "乌鲁木齐");
				cityList.Add("lanzhou", "兰州");
				cityList.Add("yinchuan", "银川");
				_AreaCity.Add("西北地区", cityList);
				cityList = new Dictionary<string, string>();
				cityList.Add("chongqing", "重庆");
				cityList.Add("chengdu", "成都");
				cityList.Add("kunming", "昆明");
				cityList.Add("guiyang", "贵阳");
				_AreaCity.Add("西南地区", cityList);
				CacheManager.InsertCache(cacheKey_AreaCity, _AreaCity, 100);
			}
		}
		/// <summary>
		/// 得到城市列表
		/// </summary>
		/// <returns></returns>
		protected string GetOtherCityPriceList()
		{
			string areaLi = "<li>{0}</li>";
			string cityLi = "<li><a href=\"http://{0}.bitauto.com/car/{1}/\">{2}{3}</a></li>";

			StringBuilder cityContent = new StringBuilder();

			foreach (KeyValuePair<string, Dictionary<string, string>> entity in _AreaCity)
			{
				cityContent.AppendLine(string.Format(areaLi, entity.Key));
				foreach (KeyValuePair<string, string> entityNode in entity.Value)
				{
					if (entityNode.Key == citySpell)
					{
						continue;
					}
					cityContent.AppendLine(string.Format(cityLi, entityNode.Key, serialSpell, entityNode.Value, serialSEOName));
				}
			}

			return cityContent.ToString();
		}
		/// <summary>
		/// 得到活动块
		/// </summary>
		/// <returns></returns>
		private string GetActiveSpan()
		{
			GroupPurchaseEntity preEntity = new GroupPurchaseEntity();
			GroupPurchaseEntity gpEntity = new Car_SerialBll().GetActiveGroupPurchaseByCityAndSerial(cityId, serialId, out preEntity);
			if (gpEntity == null || gpEntity.GroupID < 1) return "";
			/*

			// 排量和变速器
			string tempExhaust = "";
			DataRow[] ExhaustArr = new Car_BasicBll().GetCarParamEx(serialId, 785, false, " pvalue Asc");
			if (ExhaustArr != null && ExhaustArr.Length > 0)
			{
				foreach (DataRow dr in ExhaustArr)
				{
					if (tempExhaust != "")
					{
						tempExhaust += "、" + dr["pvalue"].ToString() + "L";
					}
					else
					{
						tempExhaust = dr["pvalue"].ToString() + "L";
					}
				}
			}
			serialExhaust = CommonFunction.GetExhaust(tempExhaust, ",", false);
			//排量
			string tempTransmission = "";
			DataRow[] TransmissionArr = new Car_BasicBll().GetCarParamEx(serialId, 712, false, "");
			if (TransmissionArr != null && TransmissionArr.Length > 0)
			{
				foreach (DataRow dr in TransmissionArr)
				{
					if (tempTransmission != "")
					{
						tempTransmission += "、" + dr["pvalue"].ToString();
					}
					else
					{
						tempTransmission = dr["pvalue"].ToString();
					}
				}
			}
			serialTransmission = CommonFunction.GetTransmission(tempTransmission, ",", false);
			*/
			string ativeUrl = string.Format("http://life.bitauto.com/huodong/tuangou/{0}/{1}/", serialSpell, citySpell);
			List<string> htmlList = new List<string>();
			htmlList.Add("<div class='col-all'>");
			htmlList.Add("<div class='line_box'>");
			htmlList.Add("<h3><span>");
			htmlList.Add(String.Format("<a target='_blank' href='{0}'>{1}</a>", ativeUrl, gpEntity.GroupName));
			htmlList.Add("</span></h3>");
			htmlList.Add("<div class='tuan_buy'>");
			htmlList.Add(String.Format("<div class='img'><img alt='{0}' src='{1}'></div>", gpEntity.GroupName, gpEntity.CarPic));
			htmlList.Add("<div class='tuan_jiage'>");
			htmlList.Add(String.Format("<p>上期价格：<em>{0}-{1}<small>万</small></em></p>", preEntity.MinPrice.ToString("N2"), preEntity.MaxPrice.ToString("N2")));
			htmlList.Add(String.Format("<p><span>上期平均折扣<em>{0}折</em></span><span>上期平均优惠<em>{1}元</em></span></p>", preEntity.AvgDiscount.ToString("N1"), preEntity.CostSaving * 10000));
			htmlList.Add("</div>");
			htmlList.Add("<div class='chaoji_zhidao'>");
			htmlList.Add("<ul>");
			htmlList.Add(String.Format("<li><label>厂家指导价：</label>{0}</li>", serialReferPrice));
			htmlList.Add(String.Format("<li><label>排&nbsp;&nbsp;量：</label>{0}</li>", serialExhaust));
			htmlList.Add(String.Format("<li><label>变速箱：</label>{0}</li>", serialTransmission));
			htmlList.Add(String.Format("<li class='model'><a href='http://car.bitauto.com/{0}/' target='_blank'>车型</a><span>", serialSpell));
			htmlList.Add(String.Format("|</span><a href='http://car.bitauto.com/{0}/tupian/' target='_blank'>图片</a><span>", serialSpell));
			htmlList.Add(String.Format("|</span><a href='http://car.bitauto.com/{0}/baojia/' target='_blank'>报价</a><span>", serialSpell));
			htmlList.Add(String.Format("|</span><a href='http://car.bitauto.com/{0}/koubei/' target='_blank'>口碑</a><span>", serialSpell));
			htmlList.Add(String.Format("|</span><a href='http://ask.bitauto.com/{0}/' target='_blank'>问答</a></li>", serialId));
			htmlList.Add("</ul>");
			htmlList.Add("</div>");
			htmlList.Add("<div class='tuan_can' id='div_tuan'>");
			htmlList.Add(String.Format("<p>截止日期：{0}</p>", gpEntity.StopTime.ToString("yyyy年MM月dd日")));
			if (gpEntity.IsLock == 0)
				htmlList.Add(String.Format("<a href='{0}'class='link_msct' title='马上参团' target='_blank'>马上参团</a>", ativeUrl));
			htmlList.Add("</div>");
			htmlList.Add("</div>");
			htmlList.Add("</div>");
			htmlList.Add("</div>");
			return String.Concat(htmlList.ToArray());
		}
	}
}