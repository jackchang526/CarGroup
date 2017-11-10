using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Xml;
using System.IO;

using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.CarTreeData;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.CarchannelWeb.App_Code
{
	/// <summary>
	///TreePageBase 的摘要说明
	/// </summary>
	public class TreePageBase : PageBase
	{
		protected Dictionary<string, TreeTagConfigEntity> _TTCEntityList = new Dictionary<string, TreeTagConfigEntity>();
		protected string _SearchUrl = string.Empty;
		protected string _SourceUrl = string.Empty;
		protected string _MainUrl = string.Empty;
		protected string _CurrentPageUrlTemplate = string.Empty;
		protected string conditionScript = string.Empty;
		protected string _CityCode = string.Empty;
		protected string _CityEncode = string.Empty;
		protected string _ConditionsHtml = string.Empty;//条件查询

		//存在的城市列表
		protected string _IsExitsCityList = string.Empty;

		protected string NavbarHtml;		//各标签导航的HTML

		protected readonly Regex regexQurey = new Regex(@"^(\d+((\.\d+)?(-|_)\d+(\.\d+)?)?)+$", RegexOptions.IgnoreCase);

		protected override void OnLoad(EventArgs e)
		{
			InitConfig();
			//InitCookie();
			base.OnLoad(e);
		}
		/// <summary>
		/// 初始化配置
		/// </summary>
		private void InitConfig()
		{
			_TTCEntityList = new TreeTagConfig().GetTagConfigList();
		}
		/// <summary>
		/// 初始化Cookie值
		/// </summary>
		private void InitCookie()
		{
			string cityCode = Request.Cookies["bitauto_framecity"] == null ? "" : Server.UrlDecode(Request.Cookies["bitauto_framecity"].Value.ToString());

			if (!string.IsNullOrEmpty(cityCode) && cityCode.IndexOf(',') >= 0)
			{
				string[] arrcCityCookies = cityCode.Split(',');
				_CityCode = ConvertHelper.GetInteger(arrcCityCookies[0]) == 0 ? "" : arrcCityCookies[0];
				if (arrcCityCookies.Length >= 2)
				{
					_CityEncode = string.IsNullOrEmpty(arrcCityCookies[1]) ? "" : arrcCityCookies[1];
				}
			}
		}
		/// <summary>
		/// 初始化搜索链接
		/// </summary>
		protected string InitSearchUrl(string tagType)
		{
			_SearchUrl = String.Empty;
			Dictionary<string, TagData> tagDic = TagData.GetTagDataDictionary();
			if (tagDic.ContainsKey(tagType))
				_SearchUrl = tagDic[tagType].UrlDictionary["search"].UrlRule;
			int position = _SearchUrl.IndexOf('?');
			if (position >= 0)
				_SearchUrl = _SearchUrl.Remove(position, _SearchUrl.Length - position);
			return _SearchUrl;
		}
		/// <summary>
		/// 得到标签定义的数组
		/// </summary>
		/// <returns></returns>
		protected string GetTagDefinedArray(string requestType)
		{
			StringBuilder tagString = new StringBuilder();

			foreach (KeyValuePair<string, TreeTagConfigEntity> entity in _TTCEntityList)
			{
				tagString.AppendFormat("parent.TreeTag.tagObject['{0}']=[];", entity.Key);
				tagString.AppendFormat("parent.TreeTag.tagObject['{0}']['{2}']='{1}';"
									   , entity.Key
									   , GetUrlStringByRequestType(requestType, entity.Value)
									   , requestType);
			}

			return tagString.ToString();
		}


		/// <summary>
		/// 通过请求类型得到链接
		/// </summary>
		/// <param name="tagType"></param>
		/// <returns></returns>
		private string GetUrlStringByRequestType(string requestType, TreeTagConfigEntity ttcEntity)
		{
			switch (requestType)
			{
				case "MasterBrand":
					return ttcEntity.MasterBrandUrl["sourceurl"];
				case "Brand":
					return ttcEntity.BrandUrl["sourceurl"];
				case "Serial":
					return ttcEntity.SerialUrl["sourceurl"];
				case "Search":
					return ttcEntity.SearchUrl["sourceurl"];
			}
			return "";
		}

		/// <summary>
		/// 生成指数的子品牌信息概况
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		protected string MakeSerialInfoHtml(int serialId)
		{
			//获取数据
			string serialReferPrice = "";
			List<EnumCollection.CarInfoForSerialSummary> ls = base.GetAllCarInfoForSerialSummaryByCsID(serialId);
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
			EnumCollection.SerialInfoCard sic = new Car_SerialBll().GetSerialInfoCard(serialId);
			// add by chengl Jul.23.2012
			if (sic.CsID <= 0)
			{ return ""; }
			string serialUrl = "http://car.bitauto.com/" + sic.CsAllSpell.Trim().ToLower();
			string serialExhaust = CommonFunction.GetExhaust(sic.CsEngineExhaust);
			string serialTransmission = CommonFunction.GetTransmission(sic.CsTransmissionType);
			string serialShowName = sic.CsShowName;
			if (serialId == 1568)
				serialShowName = "索纳塔八";

			List<string> htmlList = new List<string>();
			htmlList.Add("<div class=\"line_box c0624_01\">");
			htmlList.Add("<h3><span><a href=\"" + serialUrl + "\" target=\"_blank\">" + serialShowName + "</a></span></h3>");
			htmlList.Add("<dl class=\"c0624_06\">");
			string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId);
			htmlList.Add("<dt><a target=\"_blank\" href=\"" + serialUrl + "\"><img alt=\"" + serialShowName + "\" src=\"" + imgUrl + "\" height=\"80\" width=\"120\"></a></dt>");
			htmlList.Add("<dd class=\"l\">");
			htmlList.Add("<ul>");
			string serialPrice = sic.CsPriceRange;
			if (serialPrice.Length == 0)
				serialPrice = "暂无报价";
			htmlList.Add("<li><label>商家报价：</label><strong><a href=\"http://price.bitauto.com/brand.aspx?newbrandId=" + serialId + "\" target=\"_blank\">" + serialPrice + "</a></strong></li>");
			htmlList.Add("<li><label>厂家指导价：</label>" + serialReferPrice + "</li>");
			htmlList.Add("<li><label>排量：</label>" + serialExhaust + "</li>");
			htmlList.Add("<li><label>变速箱：</label>" + serialTransmission + "</li>");
			htmlList.Add("</ul></dd>");
			htmlList.Add("<dd class=\"b\">");
			htmlList.Add("<div class=\"go\"><em><a target=\"_blank\" href=\"" + serialUrl + "\">详情查看 <span><strong>" + serialShowName + " </strong>频道</span> &gt;&gt;</a></em></div>");
			htmlList.Add("<div class=\"lnk\">");
			htmlList.Add("<a href=\"http://ask.bitauto.com/browse/" + serialId + "/\" target=\"_blank\">买前咨询</a>");
			htmlList.Add("<a href=\"http://i.bitauto.com/baaadmin/car/guanzhu_" + serialId + "/\" target=\"_blank\">收藏</a>");
			htmlList.Add("<a href=\"http://i.bitauto.com/baaadmin/car/goumai_" + serialId + "/\" target=\"_blank\">计划购买</a>");
			htmlList.Add("</div></dd></dl>");
			htmlList.Add("<div class=\"clear\"></div>");
			htmlList.Add("</div>");

			return String.Concat(htmlList.ToArray());
		}

		/// <summary>
		/// 生成看过子品牌的还看过的子品牌列表
		/// </summary>
		/// <returns></returns>
		protected string MakeSerialElseSeeHtml(int serialId, string serialName)
		{
			StringBuilder htmlCode = new StringBuilder();
			List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(serialId, 7);
			if (lsts.Count > 0)
			{
				htmlCode.AppendLine("<div class=\"line_box c0708_05\">");
				htmlCode.AppendLine("<h3><span>看过" + serialName + "的还看过</span></h3>");
				htmlCode.AppendLine("<ul>");
				foreach (EnumCollection.SerialToSerial sts in lsts)
				{
					string serialUrl = "http://car.bitauto.com/" + sts.ToCsAllSpell.ToLower() + "/";

					htmlCode.AppendLine("<li><a href=\"" + serialUrl + "\" target=\"_blank\"><img alt=\"" + sts.ToCsShowName + "\" src=\"" + sts.ToCsPic + "\"></a><a target=\"_blank\" href=\"" + serialUrl + "\" class=\"limitText\">" + sts.ToCsShowName + "</a><p>" + sts.ToCsPriceRange + "</p></li>");
				}
				htmlCode.AppendLine("</ul>");
				htmlCode.AppendLine("<div class=\"clear\"></div>");
				htmlCode.AppendLine("</div>");
			}
			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成主品牌的介绍
		/// </summary>
		/// <param name="introduction"></param>
		/// <param name="masterId"></param>
		/// <param name="masterName"></param>
		/// <param name="masterUrl"></param>
		/// <returns></returns>
		protected string MakeMasterbrandIntroduceHtml(string introduction, int masterId, string masterName, string masterUrl)
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<div class=\"line_box\">");
			htmlCode.AppendLine("<h3><span><a href=\"" + masterUrl + "\" target=\"_blank\">" + masterName + "介绍</a></span></h3>");
			htmlCode.AppendLine("<div class=\"c0622_02\">");
			htmlCode.AppendLine("<div class=\"bybrand_list\">");
			htmlCode.AppendLine("<a class=\"brand m_" + masterId + "_b\" target=\"_blank\" href=\"" + masterUrl + "\"></a>");
			introduction = StringHelper.SubString(introduction, 250, true);
			htmlCode.AppendLine("<p>" + introduction + "<a href=\"" + masterUrl + "\" target=\"_blank\">详细&gt;&gt;</a></p>");
			htmlCode.AppendLine("</div></div>");
			htmlCode.AppendLine("<div class=\"clear\"></div>");
			htmlCode.AppendLine("</div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 按条件查询(生成Html)
		/// </summary>
		/// <param name="title">标题</param>
		/// <param name="isShow">是否显示层</param>
		/// <param name="isMoreConditions">是否显示更多条件查询</param>
		protected virtual void MakeConditionsHtml(string title, bool isShow, bool isMoreConditions)
		{
			List<string> htmlList = new List<string>();
			htmlList.Add("<script type=\"text/javascript\" src=\"http://image.bitautoimg.com/carchannel/jsnew/selectcarcondition.js?d=20130204\"></script>");
			MakeConditonsDetailHtml(isMoreConditions, htmlList);
			MakeConditionEndHtml(htmlList);
			this._ConditionsHtml = String.Concat(htmlList.ToArray());
		}

		protected virtual void MakeConditionsHtmlV2(string title, bool isShow, bool isMoreConditions)
		{
			List<string> htmlList = new List<string>();
			htmlList.Add("<script type=\"text/javascript\" src=\"http://image.bitautoimg.com/carchannel/jsnew/selectcartoolv2.js?d=20150521\"></script>");
			MakeConditonsDetailHtml(isMoreConditions, htmlList);
			MakeConditionEndHtml(htmlList);
			this._ConditionsHtml = String.Concat(htmlList.ToArray());
		}
		/// <summary>
		/// 选车条件 by 2014.02.10
		/// </summary>
		/// <param name="title"></param>
		protected virtual void MakeConditionsHtmlNew()
		{
			List<string> htmlList = new List<string>();
			htmlList.Add("<script type=\"text/javascript\" src=\"http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js\"></script>");
			//htmlList.Add("<script type=\"text/javascript\" src=\"/jsnew/commons.js\"></script>");
			//htmlList.Add("<script type=\"text/javascript\" src=\"/jsnew/carcompareforminiV2.js?d=22\"></script>");
			//htmlList.Add("<script type=\"text/javascript\" src=\"/jsnew/carSelectSimple.js\"></script>");
            htmlList.Add("<script type=\"text/javascript\" src=\"http://image.bitautoimg.com/carchannel/jsnew/newselectcartoolv3.js?d=2016022513\"></script>");
            //htmlList.Add("<script type=\"text/javascript\" src=\"/jsnew/newselectcartoolv3.js?d=20150923\"></script>");
			MakeConditonsDetailHtml(htmlList);
			MakeConditionEndHtml(htmlList);
			this._ConditionsHtml = String.Concat(htmlList.ToArray());
		}

		/// <summary>
		/// 生成带城市选择的条件
		/// </summary>
		/// <param name="title"></param>
		protected virtual void MakeConditionsWithCityHtml(string title, string citySpell)
		{
			List<string> sb = new List<string>();

			sb.Add("<script type=\"text/javascript\" src=\"http://image.bitautoimg.com/carchannel/jsnew/selectcarcondition.js?d=20130204\"></script>");
			sb.Add("<div class=\"line_box c0622_01\" id=\"tempNoborder\" style=\"border-bottom:1px solid #DEE3E7\">");
			sb.Add("<h3><span>&nbsp;</span></h3>");
			sb.Add("<div class=\"h3_tab2\" id=\"sub_index\">");
			sb.Add("<ul><li class=\"current\"><a href=\"#\">按城市查看指数</a></li>");
			sb.Add("<li><a href=\"#\">按条件查看指数</a></li></ul>");
			sb.Add("</div>");
			sb.Add("<div class=\"c0621_01\" id=\"showhideCon\">");
			sb.Add("<div class=\"cityrank\" id=\"sub_index_con_0\">");
			sb.Add("<ul class=\"cr\">");

			MakeCityRankHtml(sb, citySpell);
			sb.Add("</ul></div>");
			sb.Add("<div id=\"sub_index_con_1\" style=\"display:none\">");
			MakeConditonsDetailHtml(false, sb);
			sb.Add("</div>");
			MakeConditionEndHtml(sb);
			this._ConditionsHtml = String.Concat(sb.ToArray());
		}

		protected void MakeCityRankHtml(List<string> sb, string citySpell)
		{
			Dictionary<string, City> citySpellDic = AutoStorageService.GetCitySpellDic();
			string[] cityList = new string[] { "beijing", "shanghai", "guangzhou", "shenzhen", "fuzhou", "haikou", "nanning", "nanjing", "suzhou", "hangzhou", "ningbo", "hefei", "zhengzhou", "nanchang", "wuhan", "changsha", "chengdu", "chongqing", "kunming", "guiyang", "xian", "lanzhou", "yinchuan", "wulumuqi", "lasa", "xining", "taiyuan", "shijiazhuang", "jinan", "qingdao", "tianjin", "changchun", "shenyang", "dalian", "haerbin", "huhehaote" };
			if (citySpell == "")
				sb.Add("<li class=\"current\">全国</li>");
			else
				sb.Add("<li><a href=\"/tree_index/\">全国</a></li>");
			foreach (string spell in cityList)
			{
				if (!citySpellDic.ContainsKey(spell))
					continue;
				if (citySpell == spell)
					sb.Add("<li class=\"current\">" + citySpellDic[spell].CityName + "</li>");
				else
					sb.Add("<li><a href=\"/tree_index/" + spell + "/\">" + citySpellDic[spell].CityName + "</a></li>");
			}
		}

		protected void MakeConditionEndHtml(List<string> htmlList)
		{
			htmlList.Add("<script language=\"javascript\" type=\"text/javascript\">");
			htmlList.Add(GenerateSearchInitScript());
			//htmlList.Add("conditionObj.InitPageCondition();");
			htmlList.Add("</script>");
		}

		/// <summary>
		/// 增加选车的条件
		/// </summary>
		/// <param name="isShow"></param>
		/// <param name="sb"></param>
		protected void MakeConditonsDetailHtml(bool isMoreConditions, List<string> htmlList)
		{
			string path = this.GetParamsString();
			htmlList.Add("<div id=\"showhideCon\" class=\"tool-selectcar\">");
			htmlList.Add("<dl>");
			htmlList.Add("<dt>价&#12288;格：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"price0\"><a href=\"" + this.GetSearchQueryString("p", "", path) + "\">不限</a></li>");
			htmlList.Add("<li id=\"price1\"><a href=\"" + this.GetSearchQueryString("p", "0-5", path) + "\" stattype=\"price0-5\">5万以下</a></li>");
			htmlList.Add("<li id=\"price2\"><a href=\"" + this.GetSearchQueryString("p", "5-8", path) + "\" stattype=\"price5-8\">5-8万</a></li>");
			htmlList.Add("<li id=\"price3\"><a href=\"" + this.GetSearchQueryString("p", "8-12", path) + "\" stattype=\"price8-12\">8-12万</a></li>");
			htmlList.Add("<li id=\"price4\"><a href=\"" + this.GetSearchQueryString("p", "12-18", path) + "\" stattype=\"price12-18\">12-18万</a></li>");
			htmlList.Add("<li id=\"price5\"><a href=\"" + this.GetSearchQueryString("p", "18-25", path) + "\" stattype=\"price18-25\">18-25万</a></li>");
			htmlList.Add("<li id=\"price6\"><a href=\"" + this.GetSearchQueryString("p", "25-40", path) + "\" stattype=\"price25-40\">25-40万</a></li>");
			htmlList.Add("<li id=\"price7\"><a href=\"" + this.GetSearchQueryString("p", "40-80", path) + "\" stattype=\"price40-80\">40-80万</a></li>");
			htmlList.Add("<li id=\"price8\"><a href=\"" + this.GetSearchQueryString("p", "80-9999", path) + "\" stattype=\"price80-9999\">80万以上</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			htmlList.Add("<dl>");
			htmlList.Add("<dt>级&#12288;别：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"level0\"><a href=\"" + this.GetSearchQueryString("l", "0", path) + "\">不限</a></li>");
			htmlList.Add("<li id=\"level63\"><a href=\"" + this.GetSearchQueryString("l", "63", path) + "\" stattype=\"jiaoche\">轿车</a></li>");
			htmlList.Add("<li id=\"level1\">[<a href=\"" + this.GetLevelSearchQueryString("l", "1", path) + "\" class=\"cSubItem\" stattype=\"weixingche\">微型车</a></li>");
			htmlList.Add("<li id=\"level2\"><a href=\"" + this.GetLevelSearchQueryString("l", "2", path) + "\" class=\"cSubItem\" stattype=\"xiaoxingche\">小型车</a></li>");
			htmlList.Add("<li id=\"level3\"><a href=\"" + this.GetLevelSearchQueryString("l", "3", path) + "\" class=\"cSubItem\" stattype=\"jincouxingche\">紧凑型车</a></li>");
			htmlList.Add("<li id=\"level5\"><a href=\"" + this.GetLevelSearchQueryString("l", "5", path) + "\" class=\"cSubItem\" stattype=\"zhongxingche\">中型车</a></li>");
			htmlList.Add("<li id=\"level4\"><a href=\"" + this.GetLevelSearchQueryString("l", "4", path) + "\" class=\"cSubItem\" stattype=\"zhongdaxingche\">中大型车</a></li>");
			htmlList.Add("<li id=\"level6\"><a href=\"" + this.GetLevelSearchQueryString("l", "6", path) + "\" class=\"cSubItem\" stattype=\"haohuache\">豪华车</a>]</li>");
			htmlList.Add("<li id=\"level7\"><a href=\"" + this.GetLevelSearchQueryString("l", "7", path) + "\" stattype=\"mpv\">MPV</a></li>");
			htmlList.Add("<li id=\"level8\"><a href=\"" + this.GetLevelSearchQueryString("l", "8", path) + "\" stattype=\"suv\">SUV</a></li>");
			htmlList.Add("<li id=\"level9\"><a href=\"" + this.GetLevelSearchQueryString("l", "9", path) + "\" stattype=\"paoche\">跑车</a></li>");
			htmlList.Add("<li id=\"level11\"><a href=\"" + this.GetLevelSearchQueryString("l", "11", path) + "\" stattype=\"mianbaoche\">面包车</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");

			//轿车
			//htmlList.Add("<div id=\"jiaocheBox\" class=\"car_sub_select\">");
			//htmlList.Add("<div class=\"ico_arrow\"></div>");
			//htmlList.Add("<ul>");
			//htmlList.Add("<li id=\"level63\"><a href=\"" + this.GetSearchQueryString("l", "63", path) + "\"stattype=\"jiaoche\">全部</a></li>");
			//htmlList.Add("<li id=\"level1\"><a href=\"" + this.GetSearchQueryString("l", "1", path) + "\" stattype=\"weixingche\">微型车</a></li>");
			//htmlList.Add("<li id=\"level2\"><a href=\"" + this.GetSearchQueryString("l", "2", path) + "\" stattype=\"xiaoxingche\">小型车</a></li>");
			//htmlList.Add("<li id=\"level3\"><a href=\"" + this.GetSearchQueryString("l", "3", path) + "\" stattype=\"jincouxingche\">紧凑型车</a></li>");
			//htmlList.Add("<li id=\"level5\"><a href=\"" + this.GetSearchQueryString("l", "5", path) + "\" stattype=\"zhongxingche\">中型车</a></li>");
			//htmlList.Add("<li id=\"level4\"><a href=\"" + this.GetSearchQueryString("l", "4", path) + "\" stattype=\"zhongdaxingche\">中大型车</a></li>");
			//htmlList.Add("<li id=\"level6\"><a href=\"" + this.GetSearchQueryString("l", "6", path) + "\" stattype=\"haohuache\">豪华车</a></li>");
			//htmlList.Add("</ul>");
			//htmlList.Add("</div>");

			//国别
			htmlList.Add("<dl  id=\"toolGuoBie\">");
			htmlList.Add("<dt>国&#12288;别：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"brandType0\"><a href=\"" + this.GetSearchQueryString("g", "0", path) + "\">不限</a></li>");
			htmlList.Add("<li id=\"brandType1\"><a href=\"" + this.GetSearchQueryString("g", "1", path) + "\" stattype=\"zizhu\">自主</a></li>");
			htmlList.Add("<li id=\"brandType2\"><a href=\"" + this.GetSearchQueryString("g", "2", path) + "\" stattype=\"hezi\">合资</a></li>");
			htmlList.Add("<li id=\"brandType4\"><a href=\"" + this.GetSearchQueryString("g", "4", path) + "\" stattype=\"jinkou\">进口</a></li>");
			htmlList.Add("<li id=\"brandType8\"><a href=\"" + this.GetSearchQueryString("g", "8", path) + "\" stattype=\"jinkou\">德系</a></li>");
			//htmlList.Add("<li id=\"brandType9\"><a href=\"" + this.GetSearchQueryString("g", "9", path) + "\" stattype=\"jinkou\">日韩</a></li>");
			htmlList.Add("<li id=\"brandType12\"><a href=\"" + this.GetSearchQueryString("g", "12", path) + "\" stattype=\"jinkou\">日系</a></li>");
			htmlList.Add("<li id=\"brandType16\"><a href=\"" + this.GetSearchQueryString("g", "16", path) + "\" stattype=\"jinkou\">韩系</a></li>");
			htmlList.Add("<li id=\"brandType10\"><a href=\"" + this.GetSearchQueryString("g", "10", path) + "\" stattype=\"jinkou\">美系</a></li>");
			htmlList.Add("<li id=\"brandType11\"><a href=\"" + this.GetSearchQueryString("g", "11", path) + "\" stattype=\"jinkou\">欧系</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");

			htmlList.Add("<dl style=\"display:none;\" id=\"toolBianSuXiang\">");
			htmlList.Add("<dt>变速箱：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"trans0\"><a href=\"" + this.GetSearchQueryString("t", "0", path) + "\">不限</a></li>");
			htmlList.Add("<li id=\"trans1\"><a href=\"" + this.GetSearchQueryString("t", "1", path) + "\" stattype=\"trans_mt\">手动</a></li>");
			htmlList.Add("<li id=\"trans62\"><a href=\"" + this.GetSearchQueryString("t", "62", path) + "\" stattype=\"trans_at\">自动</a></li>");
			htmlList.Add("<li id=\"trans32\">[<a href=\"" + this.GetSearchQueryString("t", "32", path) + "\" class=\"cSubItem\" stattype=\"banzidong\">半自动</a></li>");
			htmlList.Add("<li id=\"trans2\"><a href=\"" + this.GetSearchQueryString("t", "2", path) + "\" class=\"cSubItem\" stattype=\"zidong\">自动(AT)</a></li>");
			htmlList.Add("<li id=\"trans4\"><a href=\"" + this.GetSearchQueryString("t", "4", path) + "\" class=\"cSubItem\" stattype=\"shouziyiti\">手自一体(A/MT)</a></li>");
			htmlList.Add("<li id=\"trans8\"><a href=\"" + this.GetSearchQueryString("t", "8", path) + "\" class=\"cSubItem\" stattype=\"wujibiansu\">无级变速(CVT)</a></li>");
			htmlList.Add("<li id=\"trans16\"><a href=\"" + this.GetSearchQueryString("t", "16", path) + "\" class=\"cSubItem\" stattype=\"shuanglihe\">双离合</a>]</li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");

			//htmlList.Add("<div id=\"zidongBox\" class=\"car_sub_select\" style=\"display:none\">");
			//htmlList.Add("<div class=\"ico_arrow\" style=\"left:97px\"></div>");
			//htmlList.Add("<ul>");
			//htmlList.Add("<li id=\"trans62\"><a href=\"" + this.GetSearchQueryString("t", "62", path) + "\"stattype=\"trans_at\">全部</a></li>");
			//htmlList.Add("<li id=\"trans32\"><a href=\"" + this.GetSearchQueryString("t", "32", path) + "\" stattype=\"banzidong\">半自动</a></li>");
			//htmlList.Add("<li id=\"trans2\"><a href=\"" + this.GetSearchQueryString("t", "2", path) + "\" stattype=\"zidong\">自动(AT)</a></li>");
			//htmlList.Add("<li id=\"trans4\"><a href=\"" + this.GetSearchQueryString("t", "4", path) + "\" stattype=\"shouziyiti\">手自一体(A/MT)</a></li>");
			//htmlList.Add("<li id=\"trans8\"><a href=\"" + this.GetSearchQueryString("t", "8", path) + "\" stattype=\"wujibiansu\">无级变速(CVT)</a></li>");
			//htmlList.Add("<li id=\"trans16\"><a href=\"" + this.GetSearchQueryString("t", "16", path) + "\" stattype=\"shuanglihe\">双离合</a></li>");
			//htmlList.Add("</ul>");
			//htmlList.Add("</div>");

			htmlList.Add("<dl style=\"display:none\" id=\"toolPaiLiang\">");
			htmlList.Add("<dt>排&#12288;量：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"dis0\"><a href=\"" + this.GetSearchQueryString("d", "", path) + "\">不限</a></li>");
			htmlList.Add("<li id=\"dis1\"><a href=\"" + this.GetSearchQueryString("d", "0-1.3", path) + "\" stattype=\"dis0-13\">1.3L以下</a></li>");
			htmlList.Add("<li id=\"dis2\"><a href=\"" + this.GetSearchQueryString("d", "1.3-1.6", path) + "\" stattype=\"dis13-16\">1.3-1.6L</a></li>");
			htmlList.Add("<li id=\"dis3\"><a href=\"" + this.GetSearchQueryString("d", "1.7-2.0", path) + "\" stattype=\"dis17-20\">1.7-2.0L</a></li>");
			htmlList.Add("<li id=\"dis4\"><a href=\"" + this.GetSearchQueryString("d", "2.1-3.0", path) + "\" stattype=\"dis21-30\">2.1-3.0L</a></li>");
			htmlList.Add("<li id=\"dis5\"><a href=\"" + this.GetSearchQueryString("d", "3.1-5.0", path) + "\" stattype=\"dis31-50\">3.1-5.0L</a></li>");
			htmlList.Add("<li id=\"dis6\"><a href=\"" + this.GetSearchQueryString("d", "5.0-9", path) + "\" stattype=\"dis50-999\">5.0L以上</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");

			htmlList.Add("<dl class=\"config-box last\" style=\"display:none\" id=\"toolPeiZhi\">");
			htmlList.Add("<dt>其&#12288;它：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul class=\"config-list\">");
			//htmlList.Add("	<li><input id=\"mcCheck0\" type=\"checkbox\" onclick=\"GotoPage('m');\" />涡轮增压</li>");
			//htmlList.Add("	<li><input id=\"mcCheck1\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>四轮驱动</li>");
			//htmlList.Add("	<li><input id=\"mcCheck2\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>四轮碟刹</li>");
			//htmlList.Add("	<li><input id=\"mcCheck3\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>天窗</li>");
			//htmlList.Add("	<li><input id=\"mcCheck4\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>前后电动车窗</li>");
			//htmlList.Add("	<li><input id=\"mcCheck5\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>皮座椅</li>");
			//htmlList.Add("	<li><input id=\"mcCheck6\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>电动座椅</li>");
			//htmlList.Add("	<li><input id=\"mcCheck7\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>座椅加热</li>");
			//htmlList.Add("	<li><input id=\"mcCheck8\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>自动空调</li>");
			//htmlList.Add("	<li><input id=\"mcCheck9\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>电动外后视镜</li>");
			//htmlList.Add("	<li><input id=\"mcCheck10\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>ESP</li>");
			//htmlList.Add("	<li><input id=\"mcCheck11\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>倒车影像</li>");
			//htmlList.Add("	<li><input id=\"mcCheck12\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>倒车雷达</li>");
			//htmlList.Add("	<li><input id=\"mcCheck13\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>GPS导航</li>");
			//htmlList.Add("	<li><input id=\"mcCheck14\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>自动泊车</li>");
			//htmlList.Add("	<li><input id=\"mcCheck15\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>定速巡航</li>");
			//htmlList.Add("	<li><input id=\"mcCheck16\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>无钥匙启动</li>");
			//htmlList.Add("	<li><input id=\"mcCheck17\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>安全带未系提示</li>");
			//htmlList.Add("	<li><input id=\"mcCheck18\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>主动安全头枕</li>");
			//htmlList.Add("	<li><input id=\"mcCheck19\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>儿童锁</li>");
			//htmlList.Add("	<li><input id=\"mcCheck20\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>儿童座椅固定</li>");
			htmlList.Add("	<li><label><input id=\"mcCheck3\" type=\"checkbox\" onclick=\"GotoPage('m');\" />天窗</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck13\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>GPS导航</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck8\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>自动空调</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck5\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>真皮座椅</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck10\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>ESP</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck15\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>定速巡航</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck21\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>氙气大灯</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck14\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>自动泊车</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck22\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>5座位以上</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck12\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>倒车雷达</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck11\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>倒车影像</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck0\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>涡轮增压</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck1\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>四轮驱动</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck2\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>四轮碟刹</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck16\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>无钥匙启动</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck23\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>胎压监测</label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck24\" type=\"checkbox\" onclick=\"GotoPage('m');\"/><strong class=\"ico-new\">新能源</strong></label></li>");
			htmlList.Add("	<li><label><input id=\"mcCheck25\" type=\"checkbox\" onclick=\"GotoPage('m');\"/><strong class=\"ico-new\">空气净化器</strong></label></li>");
			htmlList.Add("	<li><label><input id=\"bodyform_1\" type=\"checkbox\" onclick=\"GotoPage('b1');\"/>两厢</label></li>");
			htmlList.Add("	<li><label><input id=\"bodyform_2\" type=\"checkbox\" onclick=\"GotoPage('b2');\"/>三厢</label></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			htmlList.Add("<dl class=\"config-box last\" id=\"toolSelected\" style=\"display:none\">");
			htmlList.Add("			<dt>所&#12288;选：</dt>");
			htmlList.Add("			<dd id=\"more-selected\">");
			htmlList.Add("			</dd>");
			htmlList.Add("</dl>");
			//if (isMoreConditions)
			//{
			//    htmlList.Add("<dl>");
			//    htmlList.Add("<dt>更　多：</dt>");
			//    htmlList.Add("<dd>");
			//    htmlList.Add("<div id=\"moreCondition\"></div>");
			//    htmlList.Add("<div class=\"set\"><a href=\"javascript:popload('pop_more')\">设置条件&gt;&gt;</a></div>");
			//    htmlList.Add("</dd>");
			//    htmlList.Add("</dl>");
			//}

			htmlList.Add("<div class=\"hideline\"></div>");
			htmlList.Add("</div>");
		}
		/// <summary>
		/// edit by hepw jan.27.2016 add  data-channelid=""  for every condition 
		/// </summary>
		/// <param name="htmlList"></param>
		protected void MakeConditonsDetailHtml(List<string> htmlList)
		{
			string path = this.GetParamsString();
			htmlList.Add("<div id=\"showhideCon\" class=\"tool-selectcar-v2 y2015\">");
			htmlList.Add("<dl>");
			htmlList.Add("<dt>价&#12288;格：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"price0\"><a href=\"" + this.GetSearchQueryString("p", "", path) + "\" data-channelid=\"2.94.548\">不限</a></li>");
			htmlList.Add("<li id=\"price1\"><a href=\"" + this.GetSearchQueryString("p", "0-5", path) + "\" stattype=\"price0-5\" data-channelid=\"2.94.552\">5万以下</a></li>");
			htmlList.Add("<li id=\"price2\"><a href=\"" + this.GetSearchQueryString("p", "5-8", path) + "\" stattype=\"price5-8\" data-channelid=\"2.94.553\">5-8万</a></li>");
			htmlList.Add("<li id=\"price3\"><a href=\"" + this.GetSearchQueryString("p", "8-12", path) + "\" stattype=\"price8-12\" data-channelid=\"2.94.554\">8-12万</a></li>");
			htmlList.Add("<li id=\"price4\"><a href=\"" + this.GetSearchQueryString("p", "12-18", path) + "\" stattype=\"price12-18\" data-channelid=\"2.94.555\">12-18万</a></li>");
			htmlList.Add("<li id=\"price5\"><a href=\"" + this.GetSearchQueryString("p", "18-25", path) + "\" stattype=\"price18-25\" data-channelid=\"2.94.556\">18-25万</a></li>");
			htmlList.Add("<li id=\"price6\"><a href=\"" + this.GetSearchQueryString("p", "25-40", path) + "\" stattype=\"price25-40\" data-channelid=\"2.94.557\">25-40万</a></li>");
			htmlList.Add("<li id=\"price7\"><a href=\"" + this.GetSearchQueryString("p", "40-80", path) + "\" stattype=\"price40-80\" data-channelid=\"2.94.558\">40-80万</a></li>");
			htmlList.Add("<li id=\"price8\"><a href=\"" + this.GetSearchQueryString("p", "80-9999", path) + "\" stattype=\"price80-9999\" data-channelid=\"2.94.559\">80万以上</a></li>");


			htmlList.Add("<li class=\"filter active\" id=\"p_custom_null\">");
			htmlList.Add("<input type=\"text\" class=\"txtinput\" id=\"p_min\" onkeyup=\"value=value.replace(/(\\D|\\d{5})/g,'')\" maxlength=\"4\"> 至 <input type=\"text\" class=\"txtinput\" id=\"p_max\" onkeyup=\"value=value.replace(/(\\D|\\d{5})/g,'')\" maxlength=\"4\"> 万");
			htmlList.Add("<span class=\"button_gray button_59_22\"><a href=\"javascript:;\" id=\"btnPriceSubmit\" data-channelid=\"2.94.560\">确定</a></span>");
			htmlList.Add("<div class=\"tc tc-xunjia y2015\" id=\"p_alert\"></div>");
			htmlList.Add("</li>");

			htmlList.Add("<li id=\"p_custom\" class=\"last\" style=\"display: none;\">");
			htmlList.Add("<a href=\"javascript:;\" id=\"btnPriceCus\">自定义</a>");
			htmlList.Add("</li>");

			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			htmlList.Add("<dl>");
			htmlList.Add("<dt>级&#12288;别：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"level0\"><a href=\"" + this.GetSearchQueryString("l", "0", path) + "\" data-channelid=\"2.94.561\">不限</a></li>");
			htmlList.Add("<li id=\"level63\"><a href=\"" + this.GetSearchQueryString("l", "63", path) + "\" stattype=\"jiaoche\" data-channelid=\"2.161.1668\">轿车</a></li>");
            htmlList.Add("<li id=\"level1\"> ( <a href=\"" + this.GetLevelSearchQueryString("l", "1", path) + "\" class=\"cSubItem\" stattype=\"weixingche\" data-channelid=\"2.161.1669\">微型</a></li>");
            htmlList.Add("<li id=\"level2\"><a href=\"" + this.GetLevelSearchQueryString("l", "2", path) + "\" class=\"cSubItem\" stattype=\"xiaoxingche\" data-channelid=\"2.161.1670\">小型</a></li>");
            htmlList.Add("<li id=\"level3\"><a href=\"" + this.GetLevelSearchQueryString("l", "3", path) + "\" class=\"cSubItem\" stattype=\"jincouxingche\" data-channelid=\"2.161.1671\">紧凑型</a></li>");
            htmlList.Add("<li id=\"level5\"><a href=\"" + this.GetLevelSearchQueryString("l", "5", path) + "\" class=\"cSubItem\" stattype=\"zhongxingche\" data-channelid=\"2.161.1672\">中型</a></li>");
            htmlList.Add("<li id=\"level4\"><a href=\"" + this.GetLevelSearchQueryString("l", "4", path) + "\" class=\"cSubItem\" stattype=\"zhongdaxingche\" data-channelid=\"2.161.1673\">中大型</a></li>");
            htmlList.Add("<li id=\"level6\"><a href=\"" + this.GetLevelSearchQueryString("l", "6", path) + "\" class=\"cSubItem\" stattype=\"haohuache\" data-channelid=\"2.161.1674\">豪华型</a> ) </li>");
            htmlList.Add("<li id=\"level7\"><a href=\"" + this.GetLevelSearchQueryString("l", "7", path) + "\" stattype=\"mpv\" data-channelid=\"2.161.1675\">MPV</a></li>");
            htmlList.Add("<li id=\"level8\" class=\"last\" style=\"z-index: 1;\"><a href=\"" + this.GetLevelSearchQueryString("l", "8", path) + "\" stattype=\"suv\" class=\"ico-arrow\" data-channelid=\"2.161.1676\">SUV</a>");
			htmlList.Add("<div id=\"suv_popup\" class=\"popup-list\" style=\"display: none;\">");
			htmlList.Add("<div class=\"head-item\">");
            htmlList.Add("<a href=\"" + this.GetLevelSearchQueryString("l", "8", path) + "\" data-channelid=\"2.161.1676\">SUV</a></div>");
			htmlList.Add("<p id=\"level13\">");
            htmlList.Add("<a href=\"" + this.GetLevelSearchQueryString("l", "13", path) + "\" data-channelid=\"2.161.1677\">小型SUV</a></p>");
			htmlList.Add("<p id=\"level14\">");
            htmlList.Add("<a href=\"" + this.GetLevelSearchQueryString("l", "14", path) + "\" data-channelid=\"2.161.1678\">紧凑型SUV</a></p>");
			htmlList.Add("<p id=\"level15\">");
            htmlList.Add("<a href=\"" + this.GetLevelSearchQueryString("l", "15", path) + "\" data-channelid=\"2.161.1679\">中型SUV</a></p>");
			htmlList.Add("<p id=\"level16\">");
            htmlList.Add("<a href=\"" + this.GetLevelSearchQueryString("l", "16", path) + "\" data-channelid=\"2.161.1692\">中大型SUV</a></p>");
			htmlList.Add("<p id=\"level17\">");
            htmlList.Add("<a href=\"" + this.GetLevelSearchQueryString("l", "17", path) + "\" data-channelid=\"2.161.1681\">大型SUV</a></p>");
			htmlList.Add("</div>");

			htmlList.Add("</li>");
            htmlList.Add("<li id=\"level9\"><a href=\"" + this.GetLevelSearchQueryString("l", "9", path) + "\" stattype=\"paoche\" data-channelid=\"2.161.1682\">跑车</a></li>");
            htmlList.Add("<li id=\"level11\"><a href=\"" + this.GetLevelSearchQueryString("l", "11", path) + "\" stattype=\"mianbaoche\" data-channelid=\"2.161.1684\">面包车</a></li>");
            htmlList.Add("<li id=\"level12\"><a href=\"" + this.GetLevelSearchQueryString("l", "12", path) + "\" stattype=\"pika\" data-channelid=\"2.161.1683\">皮卡</a></li>");
            htmlList.Add("<li id=\"level18\"><a href=\"" + this.GetLevelSearchQueryString("l", "18", path) + "\" stattype=\"keche\" data-channelid=\"2.161.1685\">客车</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			////电动车
			//htmlList.Add("<dl>");
			//htmlList.Add("<dt>电动车：</dt>");
			//htmlList.Add("<dd>");
			//htmlList.Add("<ul>");
			//htmlList.Add("<li id=\"fueltype_dian_0\"><a href=\"" + this.GetSearchQueryString("f", "0", path) + "\">不限</a></li>");
			//htmlList.Add("<li id=\"fueltype16\"><a href=\"" + this.GetSearchQueryString("f", "16", path) + "\">纯电动</a></li>");
			//htmlList.Add("<li id=\"fueltype2\"><a href=\"" + this.GetSearchQueryString("f", "2", path) + "\">油电混合</a></li>");
			//htmlList.Add("</ul>");
			//htmlList.Add("</dd>");
			//htmlList.Add("</dl>");
			//箱式
			htmlList.Add("<dl class=\"w-short\">");
			htmlList.Add("<dt>车&#12288;身：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"bodyform0\"><a href=\"" + this.GetSearchQueryString("b", "0", path) + "\" data-channelid=\"2.94.580\">不限</a></li>");
			htmlList.Add("<li id=\"bodyform1\"><a href=\"" + this.GetSearchQueryString("b", "1", path) + "\" data-channelid=\"2.94.581\">两厢</a></li>");
			htmlList.Add("<li id=\"bodyform2\"><a href=\"" + this.GetSearchQueryString("b", "2", path) + "\" data-channelid=\"2.94.582\">三厢</a></li>");
			htmlList.Add("<li id=\"wagon1\"><a href=\"" + this.GetSearchQueryString("lv", "1", path) + "\" data-channelid=\"2.94.583\">旅行版</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			//燃料
			htmlList.Add("<dl class=\"w-long\">");
			htmlList.Add("<dt>能&#12288;源：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"fueltype0\"><a href=\"" + this.GetSearchQueryString("f", "0", path) + "\" data-channelid=\"2.94.584\">不限</a></li>");
            htmlList.Add("<li id=\"fueltype7\"><a href=\"" + this.GetSearchQueryString("f", "7", path) + "\" data-channelid=\"2.161.1686\">汽油</a></li>");
            htmlList.Add("<li id=\"fueltype8\"><a href=\"" + this.GetSearchQueryString("f", "8", path) + "\" data-channelid=\"2.161.1687\">柴油</a></li>");
            htmlList.Add("<li id=\"fueltype16\"><a href=\"" + this.GetSearchQueryString("f", "16", path) + "\" data-channelid=\"2.161.1688\">纯电动</a></li>");
            htmlList.Add("<li id=\"fueltype2\"><a href=\"" + this.GetSearchQueryString("f", "2", path) + "\" data-channelid=\"2.161.1689\">油电混合</a></li>");
            htmlList.Add("<li id=\"fueltype4\"><a href=\"" + this.GetSearchQueryString("f", "4", path) + "\" data-channelid=\"2.161.1690\">油气混合</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			//厂商
			htmlList.Add("<dl  id=\"toolGuoBie\" class=\"w-short\">");
			htmlList.Add("<dt>厂&#12288;商：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"brandType0\"><a href=\"" + this.GetSearchQueryString("g", "0", path) + "\" data-channelid=\"2.94.590\">不限</a></li>");
			htmlList.Add("<li id=\"brandType1\"><a href=\"" + this.GetSearchQueryString("g", "1", path) + "\" stattype=\"zizhu\" data-channelid=\"2.94.591\">自主</a></li>");
			htmlList.Add("<li id=\"brandType2\"><a href=\"" + this.GetSearchQueryString("g", "2", path) + "\" stattype=\"hezi\" data-channelid=\"2.94.592\">合资</a></li>");
			htmlList.Add("<li id=\"brandType4\"><a href=\"" + this.GetSearchQueryString("g", "4", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.593\">进口</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			//国别
			htmlList.Add("<dl class=\"w-long\">");
			htmlList.Add("<dt>国&#12288;别：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"country0\"><a href=\"" + this.GetSearchQueryString("c", "0", path) + "\" data-channelid=\"2.94.594\">不限</a></li>");
			htmlList.Add("<li id=\"country4\"><a href=\"" + this.GetSearchQueryString("c", "4", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.595\">德系</a></li>");
			//htmlList.Add("<li id=\"country9\"><a href=\"" + this.GetSearchQueryString("c", "9", path) + "\" stattype=\"jinkou\">日韩</a></li>");
			htmlList.Add("<li id=\"country2\"><a href=\"" + this.GetSearchQueryString("c", "2", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.596\">日系</a></li>");
			htmlList.Add("<li id=\"country16\"><a href=\"" + this.GetSearchQueryString("c", "16", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.597\">韩系</a></li>");
			htmlList.Add("<li id=\"country8\"><a href=\"" + this.GetSearchQueryString("c", "8", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.598\">美系</a></li>");
			htmlList.Add("<li id=\"country484\"><a href=\"" + this.GetSearchQueryString("c", "484", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.599\">欧系</a></li>");
			htmlList.Add("<li id=\"country509\"><a href=\"" + this.GetSearchQueryString("c", "509", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.600\">非日系</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			//变速箱
			htmlList.Add("<dl id=\"toolBianSuXiang\" class=\"w-short\">");
			htmlList.Add("<dt>变速箱：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"trans0\"><a href=\"" + this.GetSearchQueryString("t", "0", path) + "\" data-channelid=\"2.94.601\">不限</a></li>");
			htmlList.Add("<li id=\"trans1\"><a href=\"" + this.GetSearchQueryString("t", "1", path) + "\" stattype=\"trans_mt\" data-channelid=\"2.94.602\">手动</a></li>");
			htmlList.Add("<li id=\"trans126\" class=\"last\" style=\"z-index:2;\"><a href=\"" + this.GetSearchQueryString("t", "126", path) + "\" class=\"ico-arrow\" stattype=\"trans_at\" data-channelid=\"2.94.603\">自动</a>");
			htmlList.Add("<div id=\"trans_popup\" class=\"popup-list\" style=\"display:none;\">");
			htmlList.Add("<div class=\"head-item\"><a href=\"" + this.GetSearchQueryString("t", "126", path) + "\" data-channelid=\"2.94.604\">自动</a></div>");
			htmlList.Add("<p id=\"trans32\"><a href=\"" + this.GetSearchQueryString("t", "32", path) + "\" class=\"cSubItem\" stattype=\"banzidong\" data-channelid=\"2.94.605\">半自动（AMT）</a></p>");
			htmlList.Add("<p id=\"trans2\"><a href=\"" + this.GetSearchQueryString("t", "2", path) + "\" class=\"cSubItem\" stattype=\"zidong\" data-channelid=\"2.94.606\">自动（AT）</a></p>");
			htmlList.Add("<p id=\"trans4\"><a href=\"" + this.GetSearchQueryString("t", "4", path) + "\" class=\"cSubItem\" stattype=\"shouziyiti\" data-channelid=\"2.94.607\">手自一体</a></p>");
			htmlList.Add("<p id=\"trans8\"><a href=\"" + this.GetSearchQueryString("t", "8", path) + "\" class=\"cSubItem\" stattype=\"wujibiansu\" data-channelid=\"2.94.608\">无极变速（CVT）</a></p>");
			htmlList.Add("<p id=\"trans16\"><a href=\"" + this.GetSearchQueryString("t", "16", path) + "\" class=\"cSubItem\" stattype=\"shuanglihe\" data-channelid=\"2.94.609\">双离合（DSG）</a></p>");
			htmlList.Add("</div>");
			htmlList.Add("</li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			//排量
			htmlList.Add("<dl id=\"toolPaiLiang\" class=\"w-long\">");
			htmlList.Add("<dt>排&#12288;量：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"dis0\"><a href=\"" + this.GetSearchQueryString("d", "", path) + "\" data-channelid=\"2.94.610\">不限</a></li>");
			htmlList.Add("<li id=\"dis1\"><a href=\"" + this.GetSearchQueryString("d", "0-1.3", path) + "\" stattype=\"dis0-13\" data-channelid=\"2.94.611\">1.3L以下</a></li>");
			htmlList.Add("<li id=\"dis2\"><a href=\"" + this.GetSearchQueryString("d", "1.3-1.6", path) + "\" stattype=\"dis13-16\" data-channelid=\"2.94.612\">1.3-1.6L</a></li>");
			htmlList.Add("<li id=\"dis3\"><a href=\"" + this.GetSearchQueryString("d", "1.7-2.0", path) + "\" stattype=\"dis17-20\" data-channelid=\"2.94.613\">1.7-2L</a></li>");
			htmlList.Add("<li id=\"dis4\"><a href=\"" + this.GetSearchQueryString("d", "2.1-3.0", path) + "\" stattype=\"dis21-30\" data-channelid=\"2.94.614\">2.1-3L</a></li>");
			htmlList.Add("<li id=\"dis5\"><a href=\"" + this.GetSearchQueryString("d", "3.1-5.0", path) + "\" stattype=\"dis31-50\" data-channelid=\"2.94.615\">3.1-5L</a></li>");
			htmlList.Add("<li id=\"dis6\"><a href=\"" + this.GetSearchQueryString("d", "5.0-9", path) + "\" stattype=\"dis50-999\" data-channelid=\"2.94.616\">5L以上</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			//驱动
			htmlList.Add("<dl class=\"w-short\">");
			htmlList.Add("<dt>驱&#12288;动：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"drivetype0\"><a href=\"" + this.GetSearchQueryString("dt", "0", path) + "\" data-channelid=\"2.94.617\">不限</a></li>");
			htmlList.Add("<li id=\"drivetype1\"><a href=\"" + this.GetSearchQueryString("dt", "1", path) + "\" data-channelid=\"2.94.618\">前驱</a></li>");
			htmlList.Add("<li id=\"drivetype2\"><a href=\"" + this.GetSearchQueryString("dt", "2", path) + "\" data-channelid=\"2.94.619\">后驱</a></li>");
			htmlList.Add("<li id=\"drivetype252\" class=\"last\" style=\"z-index:1;\">");
			htmlList.Add("<a href=\"" + this.GetSearchQueryString("dt", "252", path) + "\" class=\"ico-arrow\" data-channelid=\"2.94.620\">四驱</a>");
			htmlList.Add("<div id=\"drivetype_popup\" class=\"popup-list\" style=\"display:none;\">");
			htmlList.Add("<div class=\"head-item\"><a href=\"" + this.GetSearchQueryString("dt", "252", path) + "\" data-channelid=\"2.94.621\">四驱</a></div>");
			htmlList.Add("<p id=\"drivetype4\"><a href=\"" + this.GetSearchQueryString("dt", "4", path) + "\" data-channelid=\"2.94.622\">全时四驱</a></p>");
			htmlList.Add("<p id=\"drivetype8\"><a href=\"" + this.GetSearchQueryString("dt", "8", path) + "\" data-channelid=\"2.94.623\">分时四驱</a></p>");
			htmlList.Add("<p id=\"drivetype16\"><a href=\"" + this.GetSearchQueryString("dt", "16", path) + "\" data-channelid=\"2.94.624\">适时四驱</a></p>");
			//htmlList.Add("<p id=\"drivetype32\"><a href=\"" + this.GetSearchQueryString("dt", "32", path) + "\">智能四驱</a></p>");
			//htmlList.Add("<p id=\"drivetype64\"><a href=\"" + this.GetSearchQueryString("dt", "64", path) + "\">四轮驱动</a></p>");
			//htmlList.Add("<p id=\"drivetype128\"><a href=\"" + this.GetSearchQueryString("dt", "128", path) + "\">前置四驱</a></p>");
			htmlList.Add("</div>");
			htmlList.Add("</li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			//排放
			htmlList.Add("<dl class=\"w-long\">");
			htmlList.Add("<dt>排&#12288;放：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"more_2\" class=\"current\"><a href=\"javascript:;\">不限</a></li>");
			htmlList.Add("<li id=\"more_126\"><a href=\"javascript:;\">国4</a></li>");
			htmlList.Add("<li id=\"more_125\"><a href=\"javascript:;\">国5</a></li>");
			htmlList.Add("<li id=\"more_127\"><a href=\"javascript:;\">京5</a></li>");
			htmlList.Add("<li id=\"more_123\"><a href=\"javascript:;\">欧4</a></li>");
			htmlList.Add("<li id=\"more_122\"><a href=\"javascript:;\">欧5</a></li>");
			htmlList.Add("<li id=\"more_126.123\"><a href=\"javascript:;\">国4/欧4</a></li>");
			htmlList.Add("<li id=\"more_125.122\"><a href=\"javascript:;\">国5/欧5</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			//车门
			htmlList.Add("<dl class=\"w-short\">");
			htmlList.Add("<dt>车门数：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"more_0\" class=\"current\"><a href=\"javascript:;\" data-channelid=\"2.94.632\">不限</a></li>");
			htmlList.Add("<li id=\"more_268\"><a href=\"javascript:;\" data-channelid=\"2.94.633\">2-3门</a></li>");
			htmlList.Add("<li id=\"more_270\"><a href=\"javascript:;\" data-channelid=\"2.94.634\">4-6门</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			htmlList.Add("<dl class=\"w-long\">");
			//座位
			htmlList.Add("<dt>座位数：</dt>");
			htmlList.Add("<dd>");
			htmlList.Add("<ul>");
			htmlList.Add("<li id=\"more_1\" class=\"current\"><a href=\"javascript:;\" data-channelid=\"2.94.635\">不限</a></li>");
			htmlList.Add("<li id=\"more_262\"><a href=\"javascript:;\" data-channelid=\"2.94.636\">2座</a></li>");
			htmlList.Add("<li id=\"more_263\"><a href=\"javascript:;\" data-channelid=\"2.94.637\">4-5座</a></li>");
			htmlList.Add("<li id=\"more_265\"><a href=\"javascript:;\" data-channelid=\"2.94.638\">6座</a></li>");
			htmlList.Add("<li id=\"more_266\"><a href=\"javascript:;\" data-channelid=\"2.94.639\">7座</a></li>");
			htmlList.Add("<li id=\"more_267\"><a href=\"javascript:;\" data-channelid=\"2.94.640\">7座以上</a></li>");
			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			//其他配置
			htmlList.Add("<dl id=\"toolPeiZhi\">");
			htmlList.Add("<dt>其&#12288;它：</dt>");
			htmlList.Add("<dd class=\"config-list\">");
			htmlList.Add("<ul>");
			htmlList.Add("	<li><label><input id=\"more_204\" type=\"checkbox\" data-channelid=\"2.94.641\" />天窗</label></li>");
			htmlList.Add("	<li><label><input id=\"more_196\" type=\"checkbox\" data-channelid=\"2.94.642\" />GPS导航</label></li>");
			htmlList.Add("	<li><label><input id=\"more_200\" type=\"checkbox\" data-channelid=\"2.94.643\" />倒车影像</label></li>");
			htmlList.Add("	<li><label><input id=\"more_180\" type=\"checkbox\" data-channelid=\"2.94.644\" />儿童锁</label></li>");
			htmlList.Add("	<li><label><input id=\"more_101\" type=\"checkbox\" data-channelid=\"2.94.645\" />涡轮增压</label></li>");
			htmlList.Add("	<li><label><input id=\"more_179\" type=\"checkbox\" data-channelid=\"2.94.646\" />无钥匙启动</label></li>");
			htmlList.Add("	<li><label><input id=\"more_141\" type=\"checkbox\" data-channelid=\"2.94.647\" />四轮碟刹</label></li>");
			htmlList.Add("	<li><label><input id=\"more_250\" type=\"checkbox\" data-channelid=\"2.94.648\" />真皮座椅</label></li>");
			htmlList.Add("	<li><label><input id=\"more_184\" type=\"checkbox\" data-channelid=\"2.94.649\" />ESP</label></li>");
			htmlList.Add("	<li><label><input id=\"more_224\" type=\"checkbox\" data-channelid=\"2.94.650\" />氙气大灯</label></li>");
			htmlList.Add("	<li><label><input id=\"more_194\" type=\"checkbox\" data-channelid=\"2.94.651\" />定速巡航</label></li>");
			htmlList.Add("	<li><label><input id=\"more_274\" type=\"checkbox\" data-channelid=\"2.94.652\" />自动空调</label></li>");
			htmlList.Add("	<li><label><input id=\"more_177\" type=\"checkbox\" data-channelid=\"2.94.653\" />胎压监测</label></li>");
			htmlList.Add("	<li><label><input id=\"more_189\" type=\"checkbox\" data-channelid=\"2.94.654\" />自动泊车</label></li>");
			htmlList.Add("	<li><label><input id=\"more_249\" type=\"checkbox\" data-channelid=\"2.94.655\" />空气净化器</label></li>");
			//htmlList.Add("	<li><label><input id=\"mcCheck26\" type=\"checkbox\"/>电动窗防夹</label></li>");
			htmlList.Add("	<li><label><input id=\"more_163\" type=\"checkbox\" data-channelid=\"2.94.656\" />换挡拨片</label></li>");
			htmlList.Add("	<li><label><input id=\"more_236\" type=\"checkbox\" data-channelid=\"2.94.657\" />电动座椅</label></li>");
			htmlList.Add("	<li><label><input id=\"more_181\" type=\"checkbox\" data-channelid=\"2.94.658\" />儿童座椅接口</label></li>");

			//htmlList.Add("	<li><label><input id=\"mcCheck22\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>5座位以上</label></li>");
			//htmlList.Add("	<li><label><input id=\"mcCheck12\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>倒车雷达</label></li>");
			//htmlList.Add("	<li><label><input id=\"mcCheck1\" type=\"checkbox\" onclick=\"GotoPage('m');\"/>四轮驱动</label></li>");
			//htmlList.Add("	<li><label><input id=\"mcCheck24\" type=\"checkbox\" onclick=\"GotoPage('m');\"/><strong class=\"ico-new\">新能源</strong></label></li>");


			htmlList.Add("</ul>");
			htmlList.Add("</dd>");
			htmlList.Add("</dl>");
			//htmlList.Add("<dl class=\"tool-info\"><span>如果以上条件不能满足您，请</span> <a href=\"http://www.bitauto.com/feedback/?CategoryType=7\" target=\"_blank\">告知我们&gt;&gt;</a></dl>");
			//htmlList.Add("<dl class=\"tool-info\"><span>条件太少？，试试</span><a href=\"/gaojixuanche/\" data-action=\"tool-msg\" target=\"_blank\"> 高级选车工具&gt;&gt;</a></dl>");
			htmlList.Add("<dl class=\"tool-info\">");
			htmlList.Add("<span>条件太少？试试</span>");
			htmlList.Add("<a href=\"/gaojixuanche/\" target=\"_blank\" data-alert=\"alert\"> 高级选车工具&gt;&gt;</a>");
			htmlList.Add("<div id=\"tool-alert\" class=\"alert\" style=\"display:none;\">");
			htmlList.Add("<div class=\"content\">");
			htmlList.Add("<a id=\"tool-close\" href=\"javascript:;\" class=\"close\"></a>");
			htmlList.Add("<span></span>");
			htmlList.Add("<em class=\"star\"></em>");
			htmlList.Add("</div>");
			htmlList.Add("<dl class=\"tool-info\"><span>条件太少？试试</span><a href=\"/gaojixuanche/\" target=\"_blank\"> 高级选车工具&gt;&gt;</a></dl>");
			htmlList.Add("<i class=\"back\"></i>");
			htmlList.Add("</div>");
			htmlList.Add("</dl>");
			htmlList.Add("<div class=\"clear\"></div>");
			htmlList.Add("</div>");
		}
        //protected List<string> listSelectCarParams = new List<string>(new string[] { "p", "d", "t", "m", "g", "b", "dt", "f", "bd", "sn", "lv" });

        #region 选车条件2016.8.2
        /// <summary>
        /// 选车条件2016.8.2
        /// </summary>
        /// <param name="title"></param>
        protected virtual void MakeConditionsHtml2016()
        {
            List<string> htmlList = new List<string>();
			//htmlList.Add("<script type=\"text/javascript\" src=\"http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js\"></script>");
			//htmlList.Add("<script type=\"text/javascript\" src=\"/jsnewv2/newselectcartoolv4.min.js?d=20161228\"></script>");
            MakeConditonsDetailHtml2016(htmlList);
            MakeConditionEndHtml2016(htmlList);
            this._ConditionsHtml = String.Concat(htmlList.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlList"></param>
        protected void MakeConditonsDetailHtml2016(List<string> htmlList)
        {
            string path = this.GetParamsString();
            htmlList.Add("<div id=\"showhideCon\" class=\"tool-selectcar-v2 y2015\">");
            htmlList.Add("<dl>");
            htmlList.Add("<dt>价格：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"price0\"><a href=\"" + this.GetSearchQueryString("p", "", path) + "\" data-channelid=\"2.94.548\">不限</a></li>");
            htmlList.Add("<li id=\"price1\"><a href=\"" + this.GetSearchQueryString("p", "0-5", path) + "\" stattype=\"price0-5\" data-channelid=\"2.94.552\">5万以下</a></li>");
            htmlList.Add("<li id=\"price2\"><a href=\"" + this.GetSearchQueryString("p", "5-8", path) + "\" stattype=\"price5-8\" data-channelid=\"2.94.553\">5-8万</a></li>");
            htmlList.Add("<li id=\"price3\"><a href=\"" + this.GetSearchQueryString("p", "8-12", path) + "\" stattype=\"price8-12\" data-channelid=\"2.94.554\">8-12万</a></li>");
            htmlList.Add("<li id=\"price4\"><a href=\"" + this.GetSearchQueryString("p", "12-18", path) + "\" stattype=\"price12-18\" data-channelid=\"2.94.555\">12-18万</a></li>");
            htmlList.Add("<li id=\"price5\"><a href=\"" + this.GetSearchQueryString("p", "18-25", path) + "\" stattype=\"price18-25\" data-channelid=\"2.94.556\">18-25万</a></li>");
            htmlList.Add("<li id=\"price6\"><a href=\"" + this.GetSearchQueryString("p", "25-40", path) + "\" stattype=\"price25-40\" data-channelid=\"2.94.557\">25-40万</a></li>");
            htmlList.Add("<li id=\"price7\"><a href=\"" + this.GetSearchQueryString("p", "40-80", path) + "\" stattype=\"price40-80\" data-channelid=\"2.94.558\">40-80万</a></li>");
            htmlList.Add("<li id=\"price8\"><a href=\"" + this.GetSearchQueryString("p", "80-9999", path) + "\" stattype=\"price80-9999\" data-channelid=\"2.94.559\">80万以上</a></li>");
            htmlList.Add("<li id=\"p_custom\" style=\"display:none;\"><a href=\"javascript:;\" id=\"btnPriceCus\">自定义</a></li>");
            htmlList.Add("<li class=\"last current\" id=\"p_custom_null\">");
            htmlList.Add("<div class=\"price-input\">");
            htmlList.Add("<input type=\"text\" id=\"p_min\" maxlength=\"4\"> - <input type=\"text\" id=\"p_max\" maxlength=\"4\"> 万");
            htmlList.Add("<div class=\"prompt-layer\" id=\"p_alert\" style=\"display:none;\"></div>");
            htmlList.Add("</div>");
            htmlList.Add("<a href=\"javascript:;\" class=\"btn-md\" id=\"btnPriceSubmit\" data-channelid=\"2.94.560\">确定</a>");
            //htmlList.Add("<div class=\"tc tc-xunjia y2015\" id=\"p_alert\"></div>");
            htmlList.Add("</li>");

            //htmlList.Add("<li id=\"p_custom\" class=\"last\" style=\"display: none;\">");
            //htmlList.Add("<a href=\"javascript:;\" id=\"btnPriceCus\">自定义</a>");
            //htmlList.Add("</li>");

            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            htmlList.Add("<dl class=\"dl-level\">");
            htmlList.Add("<dt>级别：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"level0\"><a href=\"" + this.GetSearchQueryString("l", "0", path) + "\" data-channelid=\"2.94.561\">不限</a></li>");
            htmlList.Add("<li id=\"level63\"><a href=\"" + this.GetSearchQueryString("l", "63", path) + "\" stattype=\"jiaoche\" data-channelid=\"2.161.1668\">轿车</a></li>");
            htmlList.Add("<li id=\"level1\">(<a href=\"" + this.GetLevelSearchQueryString("l", "1", path) + "\" class=\"cSubItem\" stattype=\"weixingche\" data-channelid=\"2.161.1669\">微型</a></li>");
            htmlList.Add("<li id=\"level2\"><a href=\"" + this.GetLevelSearchQueryString("l", "2", path) + "\" class=\"cSubItem\" stattype=\"xiaoxingche\" data-channelid=\"2.161.1670\">小型</a></li>");
            htmlList.Add("<li id=\"level3\"><a href=\"" + this.GetLevelSearchQueryString("l", "3", path) + "\" class=\"cSubItem\" stattype=\"jincouxingche\" data-channelid=\"2.161.1671\">紧凑型</a></li>");
            htmlList.Add("<li id=\"level5\"><a href=\"" + this.GetLevelSearchQueryString("l", "5", path) + "\" class=\"cSubItem\" stattype=\"zhongxingche\" data-channelid=\"2.161.1672\">中型</a></li>");
            htmlList.Add("<li id=\"level4\"><a href=\"" + this.GetLevelSearchQueryString("l", "4", path) + "\" class=\"cSubItem\" stattype=\"zhongdaxingche\" data-channelid=\"2.161.1673\">中大型</a></li>");
            htmlList.Add("<li id=\"level6\"><a href=\"" + this.GetLevelSearchQueryString("l", "6", path) + "\" class=\"cSubItem\" stattype=\"haohuache\" data-channelid=\"2.161.1674\">豪华型</a>)</li>");
            htmlList.Add("<li id=\"level7\"><a href=\"" + this.GetLevelSearchQueryString("l", "7", path) + "\" stattype=\"mpv\" data-channelid=\"2.161.1675\">MPV</a></li>");
            htmlList.Add("<li id=\"level8\" class=\"last\" style=\"z-index: 1;\"><a href=\"" + this.GetLevelSearchQueryString("l", "8", path) + "\" stattype=\"suv\" class=\"ico-arrow\" data-channelid=\"2.161.1676\">SUV</a>");
            htmlList.Add("<div id=\"suv_popup\" class=\"drop-layer\" style=\"display: none;\">");
            htmlList.Add("<a href=\"" + this.GetLevelSearchQueryString("l", "8", path) + "\"  data-channelid=\"2.161.1676\">SUV</a>");
            htmlList.Add("<a id=\"level13\" href=\"" + this.GetLevelSearchQueryString("l", "13", path) + "\" data-channelid=\"2.161.1677\">&nbsp;小型SUV</a>");
            htmlList.Add("<a id=\"level14\" href=\"" + this.GetLevelSearchQueryString("l", "14", path) + "\" data-channelid=\"2.161.1678\">&nbsp;紧凑型SUV</a>");
            htmlList.Add("<a id=\"level15\" href=\"" + this.GetLevelSearchQueryString("l", "15", path) + "\" data-channelid=\"2.161.1679\">&nbsp;中型SUV</a>");
            htmlList.Add("<a id=\"level16\" href=\"" + this.GetLevelSearchQueryString("l", "16", path) + "\" data-channelid=\"2.161.1692\">&nbsp;中大型SUV</a>");
            htmlList.Add("<a id=\"level17\" href=\"" + this.GetLevelSearchQueryString("l", "17", path) + "\" data-channelid=\"2.161.1681\">&nbsp;全尺寸SUV</a>");

            htmlList.Add("</li>");
            htmlList.Add("<li id=\"level9\"><a href=\"" + this.GetLevelSearchQueryString("l", "9", path) + "\" stattype=\"paoche\" data-channelid=\"2.161.1682\">跑车</a></li>");
            htmlList.Add("<li id=\"level11\"><a href=\"" + this.GetLevelSearchQueryString("l", "11", path) + "\" stattype=\"mianbaoche\" data-channelid=\"2.161.1684\">面包车</a></li>");
            htmlList.Add("<li id=\"level12\"><a href=\"" + this.GetLevelSearchQueryString("l", "12", path) + "\" stattype=\"pika\" data-channelid=\"2.161.1683\">皮卡</a></li>");
            htmlList.Add("<li id=\"level18\"><a href=\"" + this.GetLevelSearchQueryString("l", "18", path) + "\" stattype=\"keche\" data-channelid=\"2.161.1685\">客车</a></li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            //箱式
            htmlList.Add("<dl class=\"w-short\">");
            htmlList.Add("<dt>车身：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"bodyform0\"><a href=\"" + this.GetSearchQueryString("b", "0", path) + "\" data-channelid=\"2.94.580\">不限</a></li>");
            htmlList.Add("<li id=\"bodyform1\"><a href=\"" + this.GetSearchQueryString("b", "1", path) + "\" data-channelid=\"2.94.581\">两厢</a></li>");
            htmlList.Add("<li id=\"bodyform2\"><a href=\"" + this.GetSearchQueryString("b", "2", path) + "\" data-channelid=\"2.94.582\">三厢</a></li>");
            htmlList.Add("<li id=\"bodyform8\"><a href=\"" + this.GetSearchQueryString("b", "8", path) + "\" data-channelid=\"2.94.583\">旅行版</a></li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            //燃料
            htmlList.Add("<dl class=\"w-long\">");
            htmlList.Add("<dt>能源：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"fueltype0\"><a href=\"" + this.GetSearchQueryString("f", "0", path) + "\">不限</a></li>");
            htmlList.Add("<li id=\"fueltype7\"><a href=\"" + this.GetSearchQueryString("f", "7", path) + "\" data-channelid=\"2.161.1686\">汽油</a></li>");
            htmlList.Add("<li id=\"fueltype8\"><a href=\"" + this.GetSearchQueryString("f", "8", path) + "\" data-channelid=\"2.161.1687\">柴油</a></li>");
            htmlList.Add("<li id=\"fueltype16\"><a href=\"" + this.GetSearchQueryString("f", "16", path) + "\" data-channelid=\"2.161.1688\">纯电动</a></li>");
            htmlList.Add("<li id=\"fueltype128\"><a href=\"" + this.GetSearchQueryString("f", "128", path) + "\">插电混合</a></li>");
            htmlList.Add("<li id=\"fueltype2\"><a href=\"" + this.GetSearchQueryString("f", "2", path) + "\" data-channelid=\"2.161.1689\">油电混合</a></li>");
            htmlList.Add("<li id=\"fueltype256\"><a href=\"" + this.GetSearchQueryString("f", "256", path) + "\">天然气</a></li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            //厂商
            htmlList.Add("<dl  id=\"toolGuoBie\" class=\"w-short\">");
            htmlList.Add("<dt>厂商：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"brandType0\"><a href=\"" + this.GetSearchQueryString("g", "0", path) + "\" data-channelid=\"2.94.590\">不限</a></li>");
            htmlList.Add("<li id=\"brandType1\"><a href=\"" + this.GetSearchQueryString("g", "1", path) + "\" stattype=\"zizhu\" data-channelid=\"2.94.591\">自主</a></li>");
            htmlList.Add("<li id=\"brandType2\"><a href=\"" + this.GetSearchQueryString("g", "2", path) + "\" stattype=\"hezi\" data-channelid=\"2.94.592\">合资</a></li>");
            htmlList.Add("<li id=\"brandType4\"><a href=\"" + this.GetSearchQueryString("g", "4", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.593\">进口</a></li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            //国别
            htmlList.Add("<dl class=\"w-long\">");
            htmlList.Add("<dt>国别：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"country0\"><a href=\"" + this.GetSearchQueryString("c", "0", path) + "\" data-channelid=\"2.94.594\">不限</a></li>");
            htmlList.Add("<li id=\"country4\"><a href=\"" + this.GetSearchQueryString("c", "4", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.595\">德系</a></li>");
            //htmlList.Add("<li id=\"country9\"><a href=\"" + this.GetSearchQueryString("c", "9", path) + "\" stattype=\"jinkou\">日韩</a></li>");
            htmlList.Add("<li id=\"country2\"><a href=\"" + this.GetSearchQueryString("c", "2", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.596\">日系</a></li>");
            htmlList.Add("<li id=\"country16\"><a href=\"" + this.GetSearchQueryString("c", "16", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.597\">韩系</a></li>");
            htmlList.Add("<li id=\"country8\"><a href=\"" + this.GetSearchQueryString("c", "8", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.598\">美系</a></li>");
            htmlList.Add("<li id=\"country484\"><a href=\"" + this.GetSearchQueryString("c", "484", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.599\">欧系</a></li>");
            htmlList.Add("<li id=\"country509\"><a href=\"" + this.GetSearchQueryString("c", "509", path) + "\" stattype=\"jinkou\" data-channelid=\"2.94.600\">非日系</a></li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            //变速箱
            htmlList.Add("<dl id=\"toolBianSuXiang\" class=\"w-short\">");
            htmlList.Add("<dt>变速箱：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"trans0\"><a href=\"" + this.GetSearchQueryString("t", "0", path) + "\" data-channelid=\"2.94.601\">不限</a></li>");
            htmlList.Add("<li id=\"trans1\"><a href=\"" + this.GetSearchQueryString("t", "1", path) + "\" stattype=\"trans_mt\" data-channelid=\"2.94.602\">手动</a></li>");
            htmlList.Add("<li id=\"trans126\" class=\"last\" style=\"z-index:2;\"><a href=\"" + this.GetSearchQueryString("t", "126", path) + "\" class=\"ico-arrow\" stattype=\"trans_at\" data-channelid=\"2.94.603\">自动</a>");
            htmlList.Add("<div id=\"trans_popup\" class=\"drop-layer\" style=\"display:none;\">");
            htmlList.Add("<a href=\"" + this.GetSearchQueryString("t", "126", path) + "\" data-channelid=\"2.94.604\">自动</a>");
			htmlList.Add("<a id=\"trans32\" href=\"" + this.GetSearchQueryString("t", "32", path) + "\" class=\"cSubItem\" stattype=\"banzidong\" data-channelid=\"2.94.605\">&nbsp;机械自动（AMT）</a>");
			htmlList.Add("<a id=\"trans2\" href=\"" + this.GetSearchQueryString("t", "2", path) + "\" class=\"cSubItem\" stattype=\"zidong\" data-channelid=\"2.94.606\">&nbsp;自动（AT）</a>");
			htmlList.Add("<a id=\"trans4\" href=\"" + this.GetSearchQueryString("t", "4", path) + "\" class=\"cSubItem\" stattype=\"shouziyiti\" data-channelid=\"2.94.607\">&nbsp;手自一体</a>");
			htmlList.Add("<a id=\"trans8\" href=\"" + this.GetSearchQueryString("t", "8", path) + "\" class=\"cSubItem\" stattype=\"wujibiansu\" data-channelid=\"2.94.608\">&nbsp;无极变速（CVT）</a>");
			htmlList.Add("<a id=\"trans16\" href=\"" + this.GetSearchQueryString("t", "16", path) + "\" class=\"cSubItem\" stattype=\"shuanglihe\" data-channelid=\"2.94.609\">&nbsp;双离合（DSG）</a>");
            htmlList.Add("</div>");
            htmlList.Add("</li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            //排量
            htmlList.Add("<dl id=\"toolPaiLiang\" class=\"w-long\">");
            htmlList.Add("<dt>排量：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"dis0\"><a href=\"" + this.GetSearchQueryString("d", "", path) + "\" data-channelid=\"2.94.610\">不限</a></li>");
            htmlList.Add("<li id=\"dis1\"><a href=\"" + this.GetSearchQueryString("d", "0-1.3", path) + "\" stattype=\"dis0-13\" data-channelid=\"2.94.611\">1.3L以下</a></li>");
            htmlList.Add("<li id=\"dis2\"><a href=\"" + this.GetSearchQueryString("d", "1.3-1.6", path) + "\" stattype=\"dis13-16\" data-channelid=\"2.94.612\">1.3-1.6L</a></li>");
            htmlList.Add("<li id=\"dis3\"><a href=\"" + this.GetSearchQueryString("d", "1.7-2.0", path) + "\" stattype=\"dis17-20\" data-channelid=\"2.94.613\">1.7-2L</a></li>");
            htmlList.Add("<li id=\"dis4\"><a href=\"" + this.GetSearchQueryString("d", "2.1-3.0", path) + "\" stattype=\"dis21-30\" data-channelid=\"2.94.614\">2.1-3L</a></li>");
            htmlList.Add("<li id=\"dis5\"><a href=\"" + this.GetSearchQueryString("d", "3.1-5.0", path) + "\" stattype=\"dis31-50\" data-channelid=\"2.94.615\">3.1-5L</a></li>");
            htmlList.Add("<li id=\"dis6\"><a href=\"" + this.GetSearchQueryString("d", "5.0-9", path) + "\" stattype=\"dis50-999\" data-channelid=\"2.94.616\">5L以上</a></li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            //驱动
            htmlList.Add("<dl class=\"w-short\">");
            htmlList.Add("<dt>驱动：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"drivetype0\"><a href=\"" + this.GetSearchQueryString("dt", "0", path) + "\" data-channelid=\"2.94.617\">不限</a></li>");
            htmlList.Add("<li id=\"drivetype1\"><a href=\"" + this.GetSearchQueryString("dt", "1", path) + "\" data-channelid=\"2.94.618\">前驱</a></li>");
            htmlList.Add("<li id=\"drivetype2\"><a href=\"" + this.GetSearchQueryString("dt", "2", path) + "\" data-channelid=\"2.94.619\">后驱</a></li>");
            htmlList.Add("<li id=\"drivetype252\" class=\"last\" style=\"z-index:1;\">");
            htmlList.Add("<a href=\"" + this.GetSearchQueryString("dt", "252", path) + "\" class=\"ico-arrow\" data-channelid=\"2.94.620\">四驱</a>");
            htmlList.Add("<div id=\"drivetype_popup\" class=\"drop-layer\" style=\"display:none;\">");
            htmlList.Add("<a href=\"" + this.GetSearchQueryString("dt", "252", path) + "\" data-channelid=\"2.94.621\">四驱</a>");
            htmlList.Add("<a id=\"drivetype4\" href=\"" + this.GetSearchQueryString("dt", "4", path) + "\" data-channelid=\"2.94.622\">&nbsp;全时四驱</a>");
			htmlList.Add("<a id=\"drivetype8\" href=\"" + this.GetSearchQueryString("dt", "8", path) + "\" data-channelid=\"2.94.623\">&nbsp;分时四驱</a>");
			htmlList.Add("<a id=\"drivetype16\" href=\"" + this.GetSearchQueryString("dt", "16", path) + "\" data-channelid=\"2.94.624\">&nbsp;适时四驱</a>");
            //htmlList.Add("<p id=\"drivetype32\"><a href=\"" + this.GetSearchQueryString("dt", "32", path) + "\">智能四驱</a></p>");
            //htmlList.Add("<p id=\"drivetype64\"><a href=\"" + this.GetSearchQueryString("dt", "64", path) + "\">四轮驱动</a></p>");
            //htmlList.Add("<p id=\"drivetype128\"><a href=\"" + this.GetSearchQueryString("dt", "128", path) + "\">前置四驱</a></p>");
            htmlList.Add("</div>");
            htmlList.Add("</li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            //排放
            htmlList.Add("<dl class=\"w-long\">");
            htmlList.Add("<dt>排放：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"more_2\" class=\"current\"><a href=\"javascript:;\">不限</a></li>");
            htmlList.Add("<li id=\"more_120\"><a href=\"javascript:;\">国5</a></li>");
            htmlList.Add("<li id=\"more_121\"><a href=\"javascript:;\">国4</a></li>");
            htmlList.Add("<li id=\"more_122\"><a href=\"javascript:;\">国3</a></li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            //车门
            /*
            htmlList.Add("<dl class=\"w-short\">");
            htmlList.Add("<dt>车门数：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"more_0\" class=\"current\"><a href=\"javascript:;\" data-channelid=\"2.94.632\">不限</a></li>");
            htmlList.Add("<li id=\"more_268\"><a href=\"javascript:;\" data-channelid=\"2.94.633\">2-3门</a></li>");
            htmlList.Add("<li id=\"more_270\"><a href=\"javascript:;\" data-channelid=\"2.94.634\">4-6门</a></li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            */
            //座位
            htmlList.Add("<dl class=\"dl-level\">");
            htmlList.Add("<dt>座位数：</dt>");
            htmlList.Add("<dd>");
            htmlList.Add("<ul>");
            htmlList.Add("<li id=\"more_1\" class=\"current\"><a href=\"javascript:;\" data-channelid=\"2.94.635\">不限</a></li>");
            htmlList.Add("<li id=\"more_279\"><a href=\"javascript:;\" data-channelid=\"2.94.636\">2座</a></li>");
            htmlList.Add("<li id=\"more_280\"><a href=\"javascript:;\" data-channelid=\"2.94.637\">4座</a></li>");
            htmlList.Add("<li id=\"more_281\"><a href=\"javascript:;\">5座</a></li>");
            htmlList.Add("<li id=\"more_282\"><a href=\"javascript:;\" data-channelid=\"2.94.638\">6座</a></li>");
            htmlList.Add("<li id=\"more_283\"><a href=\"javascript:;\" data-channelid=\"2.94.639\">7座</a></li>");
            htmlList.Add("<li id=\"more_284\"><a href=\"javascript:;\" data-channelid=\"2.94.640\">7座以上</a></li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            //其他配置
            htmlList.Add("<dl id=\"toolPeiZhi\" class=\"toolPeiZhi\" >");
            htmlList.Add("<dt>其它：</dt>");
            htmlList.Add("<dd class=\"config-list\">");
            htmlList.Add("<ul>");
            htmlList.Add("	<li><label><input id=\"more_207\" type=\"checkbox\" data-channelid=\"2.94.641\" />天窗</label></li>");
            htmlList.Add("	<li><label><input id=\"more_197\" type=\"checkbox\" data-channelid=\"2.94.643\" />倒车影像</label></li>");
            htmlList.Add("	<li><label><input id=\"more_101\" type=\"checkbox\" data-channelid=\"2.94.645\" />涡轮增压</label></li>");
            htmlList.Add("	<li><label><input id=\"more_226\" type=\"checkbox\" data-channelid=\"2.94.646\" />无钥匙启动</label></li>");
            htmlList.Add("	<li><label><input id=\"more_192\" type=\"checkbox\" />自动驻车</label></li>");
            htmlList.Add("	<li><label><input id=\"more_244\" type=\"checkbox\" data-channelid=\"2.94.652\" />自动空调</label></li>");
            htmlList.Add("	<li><label><input id=\"more_170\" type=\"checkbox\" data-channelid=\"2.94.649\" />ESP</label></li>");
            htmlList.Add("	<li><label><input id=\"more_242\" type=\"checkbox\" data-channelid=\"2.94.656\" />换挡拨片</label></li>");
            htmlList.Add("	<li><label><input id=\"more_191\" type=\"checkbox\" data-channelid=\"2.94.654\" />自动泊车</label></li>");
            htmlList.Add("	<li><label><input id=\"more_287\" type=\"checkbox\" data-channelid=\"2.94.642\" />GPS导航</label></li>");
            htmlList.Add("	<li><label><input id=\"more_182\" type=\"checkbox\" data-channelid=\"2.94.651\" />定速巡航</label></li>");
            htmlList.Add("	<li><label><input id=\"more_179\" type=\"checkbox\" data-channelid=\"2.94.653\" />胎压监测</label></li>");
            htmlList.Add("	<li><label><input id=\"more_184\" type=\"checkbox\" />主动刹车</label></li>");
            htmlList.Add("	<li><label><input id=\"more_194\" type=\"checkbox\" />上坡辅助</label></li>");
            htmlList.Add("	<li><label><input id=\"more_201\" type=\"checkbox\" />LED日间行车灯</label></li>");
            htmlList.Add("	<li><label><input id=\"more_246\" type=\"checkbox\" />后排空调</label></li>");
            htmlList.Add("	<li><label><input id=\"more_297\" type=\"checkbox\" />发动机启停</label></li>");
            htmlList.Add("	<li><label><input id=\"more_169\" type=\"checkbox\" />牵引力制动</label></li><a id=\"anchorcarlist\"></a></li>");
            htmlList.Add("</ul>");
            htmlList.Add("</dd>");
            htmlList.Add("</dl>");
            htmlList.Add("<dl class=\"tool-info\">");
            htmlList.Add("<p><span>条件太少？试试</span>");
            htmlList.Add("<a href=\"/gaojixuanche/\" target=\"_blank\" data-alert=\"alert\"> 高级选车工具</a></p>");
            htmlList.Add("</dl>");
            htmlList.Add("<div class=\"clear\"></div>");
            htmlList.Add("</div>");
        }

        protected void MakeConditionEndHtml2016(List<string> htmlList)
        {
            htmlList.Add("<script language=\"javascript\" type=\"text/javascript\">");
            htmlList.Add(GenerateSearchInitScript());
            //htmlList.Add("conditionObj.InitPageCondition();");
            htmlList.Add("</script>");
        }
        #endregion
       

		protected string GetLevelSearchQueryString(string query, string value, string param)
		{
			NameValueCollection collection = Request.QueryString;
			string path = this._SearchUrl + "?" + param;
			bool flag = false;
			//只有级别参数有值 才显示级别链接
			foreach (string key in collection.AllKeys)
			{
				if (querys.Contains(key))
				{
					if (key != "l" && !string.IsNullOrEmpty(collection[key]) && collection[key] != "0")
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag && this._SearchUrl.IndexOf("xuanchegongju") > 0)
			{
				switch (value)
				{
					case "1": path = "/weixingche/"; break;
					case "2": path = "/xiaoxingche/"; break;
					case "3": path = "/jincouxingche/"; break;
					case "5": path = "/zhongxingche/"; break;
					case "4": path = "/zhongdaxingche/"; break;
					case "6": path = "/haohuaxingche/"; break;
					case "7": path = "/mpv/"; break;
					case "8": path = "/suv/"; break;
					case "9": path = "/paoche/"; break;
					case "11": path = "/mianbaoche/"; break;
					case "12": path = "/pika/"; break;
					case "13": path = "/suv/x/"; break;
					case "14": path = "/suv/j/"; break;
					case "15": path = "/suv/z/"; break;
					case "16": path = "/suv/zd/"; break;
					case "17": path = "/suv/q/"; break;
                    case "18": path = "/keche/"; break; 
					case "63": path = "/jiaoche/"; break;
				}
				return path;
			}
			return this.GetSearchQueryString(query, value, param);
		}
		/// <summary>
		/// 生成查询条件的query
		/// </summary>
		/// <param name="query">参数key</param>
		/// <param name="value">参数值</param>
		/// <param name="param">参数字符串</param>
		/// <returns>查询地址</returns>
		protected virtual string GetSearchQueryString(string query, string value, string param)
		{
			NameValueCollection collection = Request.QueryString;
			string path = this._SearchUrl + "?";
			var queryString = HttpUtility.ParseQueryString(param);
			//旅行版 箱式 不同参数 同级单选 处理 2014.02.11
			if (query == "b" || query == "lv")
			{
				if (query == "b" && !string.IsNullOrEmpty(collection["lv"]))
				{
					queryString.Remove("lv");
				}
				else if (query == "lv" && !string.IsNullOrEmpty(collection["b"]))
				{
					queryString.Remove("b");
				}
			}
			if (string.IsNullOrEmpty(collection[query]))
			{
				queryString.Add(query, value);
			}
			else
			{
				if (query == "more")
				{
					queryString[query] = queryString[query] + "_" + value;
				}
				else
				{
					queryString[query] = value;
				}
			}

			return path + queryString.ToString() + "#anchorTitle";
		}

		/// <summary>
		/// 获取参数字符串
		/// </summary>
		/// <returns>string</returns>
		protected virtual string GetParamsString()
		{
			StringBuilder sb = new StringBuilder();
			NameValueCollection collection = Request.QueryString;

			foreach (string key in collection.Keys)
			{
				if (string.IsNullOrEmpty(key))
				{
					continue;
				}

				if (!string.IsNullOrEmpty(collection[key]) && this.IsInArray(key) && this.IsMatchQueryString(collection[key]))
				{
					if (sb.Length > 0)
					{
						sb.Append("&");
					}

					sb.Append(key + "=" + collection[key]);
				}
			}

			return sb.ToString();
		}

		protected readonly string[] querys = { "p", "l", "d", "t", "g", "more", "v", "s", "b", "e", "dt", "f", "bd", "sn", "lv", "fc", "c" };

		protected SelectCarParameters GetSelectCarParas()
		{
			SelectCarParameters selectParas = new SelectCarParameters();
			string conditionStr = "";
			//价格
			string tmpStr = Request.QueryString["p"];
			if (!String.IsNullOrEmpty(tmpStr))
			{
				string[] pc = tmpStr.Split('-');
				if (pc.Length == 2)
				{
					selectParas.MinPrice = ConvertHelper.GetInteger(pc[0]);
					selectParas.MaxPrice = ConvertHelper.GetInteger(pc[1]);
					if (selectParas.MaxPrice == 9999)
						selectParas.MaxPrice = 0;
				}

				switch (tmpStr)
				{
					case "0-5":
						selectParas.PriceFlag = 1;
						conditionStr = "5万以下";
						break;
					case "5-8":
						selectParas.PriceFlag = 2;
						conditionStr = "5万-8万";
						break;
					case "8-12":
						selectParas.PriceFlag = 3;
						conditionStr = "8万-12万";
						break;
					case "12-18":
						selectParas.PriceFlag = 4;
						conditionStr = "12万-18万";
						break;
					case "18-25":
						selectParas.PriceFlag = 5;
						conditionStr = "18万-25万";
						break;
					case "25-40":
						selectParas.PriceFlag = 6;
						conditionStr = "25万-40万";
						break;
					case "40-80":
						selectParas.PriceFlag = 7;
						conditionStr = "40万-80万";
						break;
					case "80-9999":
						selectParas.PriceFlag = 8;
						conditionStr = "80万以上";
						break;
				}
			}
			//级别
			selectParas.Level = ConvertHelper.GetInteger(Request.QueryString["l"]);
			if (selectParas.Level > 0)
			{
				string levelName = "";
				//63是轿车的级别集合，
				if (selectParas.Level == 63)
				{
					levelName = "轿车";
				}
				else
				{
					//EnumCollection.SelectCarLevelEnum level = (EnumCollection.SelectCarLevelEnum)selectParas.Level;
					//levelName = Car_LevelBll.LevelNameDic[level.ToString()];
					levelName = CarUtils.Define.CarLevelDefine.GetSelectCarLevelNameById(selectParas.Level);
					selectParas.Level = (int)Math.Pow(2, selectParas.Level - 1);
				}
				if (conditionStr.Length > 0)
					conditionStr += "_";
				conditionStr += levelName;
				selectParas.PriceFlag = 0;
			}

			//自主，合资，进口
			int brandType = ConvertHelper.GetInteger(Request.QueryString["g"]);
			if (brandType > 0)
			{
				if (conditionStr.Length > 0)
					conditionStr += "_";
				switch (brandType)
				{
					case 1:
						selectParas.BrandType = 1;
						conditionStr += "自主";
						break;
					case 2:
						selectParas.BrandType = 2;
						conditionStr += "合资";
						break;
					case 4:
						selectParas.BrandType = 4;
						conditionStr += "进口";
						break;
				}
			}
			//国别
			selectParas.Country = ConvertHelper.GetInteger(Request.QueryString["c"]);
			if (selectParas.Country > 0)
			{
				if (conditionStr.Length > 0)
					conditionStr += "_";
				switch (selectParas.Country)
				{
					case 8:
						selectParas.Country = 8;
						conditionStr += "德系";
						break;
					case 9:
						selectParas.Country = 9;
						conditionStr += "日韩";
						break;
					case 10:
						selectParas.Country = 10;
						conditionStr += "美系";
						break;
					case 11:
						selectParas.Country = 11;
						conditionStr += "欧系";
						break;
					case 12:
						selectParas.Country = 12;
						conditionStr += "日本";
						break;
					case 509:
						selectParas.Country = 509;
						conditionStr += "非日系";
						break;
					default:
						conditionStr += Enum.GetName(typeof(EnumCollection.FlagsCountries), selectParas.Country);
						break;
				}
			}
			//油耗
			tmpStr = Request.QueryString["fc"];
			if (!String.IsNullOrEmpty(tmpStr))
			{
				if (conditionStr.Length > 0)
					conditionStr += "_";
				conditionStr += tmpStr + "L";

				string[] fc = tmpStr.Split('-');
				if (fc.Length == 2)
				{
					selectParas.MinFuel = ConvertHelper.GetDouble(fc[0]);
					selectParas.MaxFuel = ConvertHelper.GetDouble(fc[1]);
					//if (selectParas.MaxDis == 9.0)
					//selectParas.MaxDis = 0.0;
				}
				selectParas.PriceFlag = 0;
			}

			//排量
			tmpStr = Request.QueryString["d"];
			if (!String.IsNullOrEmpty(tmpStr))
			{
				if (conditionStr.Length > 0)
					conditionStr += "_";
				conditionStr += tmpStr + "L";

				string[] dc = tmpStr.Split('-');
				if (dc.Length == 2)
				{
					selectParas.MinDis = ConvertHelper.GetDouble(dc[0]);
					selectParas.MaxDis = ConvertHelper.GetDouble(dc[1]);
					if (selectParas.MaxDis == 9.0)
						selectParas.MaxDis = 0.0;
				}
				selectParas.PriceFlag = 0;
			}

			//变速箱
			selectParas.TransmissionType = ConvertHelper.GetInteger(Request.QueryString["t"]);
			string transType = "";
			if (selectParas.TransmissionType >= 2)
			{
				transType = "自动";
				//selectParas.TransmissionType = 2 + 4 + 8 + 16;		//合并了自动，手自一体，CVT及双离合
			}
			else if (selectParas.TransmissionType == 1)
				transType = "手动";

			if (selectParas.TransmissionType != 0)
			{
				if (conditionStr.Length > 0)
					conditionStr += "_";
				conditionStr += transType;
				selectParas.PriceFlag = 0;
			}

			//驱动
			selectParas.DriveType = ConvertHelper.GetInteger(Request.QueryString["dt"]);
			//燃料
			selectParas.FuelType = ConvertHelper.GetInteger(Request.QueryString["f"]);
			//车门数
			var bodyDoors = Request.QueryString["bd"];
			if (!string.IsNullOrEmpty(bodyDoors))
			{
				string[] doors = bodyDoors.Split('-');
				if (doors.Length == 2)
				{
					selectParas.MinBodyDoors = ConvertHelper.GetInteger(doors[0]);
					selectParas.MaxBodyDoors = ConvertHelper.GetInteger(doors[1]);
				}
				switch (bodyDoors)
				{
					case "2-3": break;
					case "4-5": break;
				}
			}
			//座位数
			var perfSeatNum = Request.QueryString["sn"];
			if (!string.IsNullOrEmpty(perfSeatNum))
			{
				string[] seatArr = perfSeatNum.Split('-');
				if (seatArr.Length == 2)
				{
					selectParas.MinPerfSeatNum = ConvertHelper.GetInteger(seatArr[0]);
					selectParas.MaxPerfSeatNum = ConvertHelper.GetInteger(seatArr[1]);
				}
				else
				{
					selectParas.MinPerfSeatNum = ConvertHelper.GetInteger(seatArr[0]);
				}
				switch (perfSeatNum)
				{
					case "2": break;
					case "4-5": break;
					case "6": break;
					case "7": break;
					case "8-9999": break;
				}
			}
			//是否旅行版
			selectParas.IsWagon = ConvertHelper.GetInteger(Request.QueryString["lv"]);

			//更多条件
			tmpStr = Request.QueryString["m"];
			if (!String.IsNullOrEmpty(tmpStr))
			{
				// modified by chengl 
				int mcLength = (tmpStr.Length > 30 ? 30 : tmpStr.Length);
				for (int i = 0; i < mcLength; i++)
				{
					if (tmpStr[i] == '1')
					{
						selectParas.CarConfig += (int)Math.Pow(2, i);
						selectParas.PriceFlag = 0;
					}
				}
			}

			//箱式
			tmpStr = Request.QueryString["b"];
			int bodyForm = 0;
			Int32.TryParse(tmpStr, out bodyForm);
			selectParas.BodyForm = bodyForm;

			selectParas.ConditionString = conditionStr;

			return selectParas;
		}

		/// <summary>
		/// 参数是否在参数数组中
		/// </summary>
		/// <param name="array">参数</param>
		/// <returns>bool</returns>
		protected virtual bool IsInArray(string array)
		{
			bool isOK = false;

			foreach (string qu in querys)
			{
				if (array.ToLower() == qu)
				{
					isOK = true;

					break;
				}
			}

			return isOK;
		}
		/// <summary>
		/// 验证 字符串参数有效性 1.3-1.6 、200、200_300
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		protected bool IsMatchQueryString(string query)
		{
			if (string.IsNullOrEmpty(query)) return false;
			//string regexString = @"^(\d+((\.\d+)?(-|_)\d+(\.\d+)?)?)+$";
			//Regex r = new Regex(regexString, RegexOptions.IgnoreCase);
			return regexQurey.IsMatch(query);
		}

		/// <summary>
		/// 生成
		/// </summary>
		/// <returns></returns>
		protected string GenerateSearchInitScript()
		{
			StringBuilder scriptCode = new StringBuilder();
			string tmpStr = Request.QueryString["p"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("conditionObj.Price='" + tmpStr + "';");
			int l = ConvertHelper.GetInteger(Request.QueryString["l"]);
			if (l > 0)
				scriptCode.AppendLine("conditionObj.Level=" + l + ";");
			tmpStr = Request.QueryString["d"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("conditionObj.Displacement='" + tmpStr + "';");
			int t = ConvertHelper.GetInteger(Request.QueryString["t"]);
			if (t > 0)
				scriptCode.AppendLine("conditionObj.TransmissionType=" + t + ";");
			tmpStr = Request.QueryString["more"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("conditionObj.SetMoreCondition('" + tmpStr + "');");
			int g = ConvertHelper.GetInteger(Request.QueryString["g"]);
			if (g > 0)
				scriptCode.AppendLine("conditionObj.Brand=" + g + ";");
			int c = ConvertHelper.GetInteger(Request.QueryString["c"]);
			if (c > 0)
				scriptCode.AppendLine("conditionObj.Country=" + c + ";");
			int b = ConvertHelper.GetInteger(Request.QueryString["b"]);
			if (b > 0)
				scriptCode.AppendLine("conditionObj.BodyForm=" + b + ";");
			tmpStr = Request.QueryString["v"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("conditionObj.View=" + tmpStr + ";");
			int s = ConvertHelper.GetInteger(Request.QueryString["s"]);
			if (s > 0)
				scriptCode.AppendLine("conditionObj.Sort=" + s + ";");
			tmpStr = Request.QueryString["e"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("conditionObj.Envelope=" + tmpStr + ";");
			int dt = ConvertHelper.GetInteger(Request.QueryString["dt"]);
			if (dt > 0)
				scriptCode.AppendLine("conditionObj.DriveType=" + dt + ";");
			int f = ConvertHelper.GetInteger(Request.QueryString["f"]);
			if (f > 0)
				scriptCode.AppendLine("conditionObj.FuelType=" + f + ";");
			//tmpStr = Request.QueryString["bd"];
			//if (!String.IsNullOrEmpty(tmpStr))
			//    scriptCode.AppendLine("conditionObj.BodyDoors='" + tmpStr + "';");
			//tmpStr = Request.QueryString["sn"];
			//if (!String.IsNullOrEmpty(tmpStr))
			//    scriptCode.AppendLine("conditionObj.PerfSeatNum='" + tmpStr + "';");
			int lv = ConvertHelper.GetInteger(Request.QueryString["lv"]);
			if (lv > 0)
				scriptCode.AppendLine("conditionObj.IsWagon=" + lv + ";");
			tmpStr = Request.QueryString["fc"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("conditionObj.FuelConsumption='" + tmpStr + "';");
			int page = ConvertHelper.GetInteger(Request.QueryString["page"]);
			if (page > 0)
				scriptCode.AppendLine("conditionObj.Page='" + page + "';");
			return scriptCode.ToString();
		}
		/// <summary>
		/// 初始化城市ID列表
		/// </summary>
		/// <returns></returns>
		protected void ScriptCityIdList(int serialId)
		{
			string cacheKey = "TreePageBase_ScriptCityIdList_" + serialId.ToString();
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				_IsExitsCityList = (string)obj;
				return;
			}
			StringBuilder cityList = new StringBuilder();
			if (serialId == 0)
			{
				Dictionary<int, City> cityNameList = AutoStorageService.GetCityNameIdList();
				foreach (KeyValuePair<int, City> entity in cityNameList)
				{
					cityList.AppendFormat(",\"{0}\":\"1\"", entity.Key);
				}
			}
			else
			{
				Dictionary<int, int> cityNameList = NewsChannelBll.GetTreeHangQingSerialCityNumber(serialId);
				if (cityNameList != null && cityNameList.Count > 0)
				{
					foreach (KeyValuePair<int, int> entity in cityNameList)
					{
						cityList.AppendFormat(",\"{0}\":\"{1}\"", entity.Key, entity.Value);
					}
				}
			}

			if (cityList.Length < 1)
			{
				_IsExitsCityList = "var _CityList = {};";
				return;
			}
			else
			{
				cityList = cityList.Remove(0, 1);
				cityList.Append("};");
				cityList.Insert(0, "{");
				cityList.Insert(0, "var _CityList =");
				_IsExitsCityList = cityList.ToString();
				CacheManager.InsertCache(cacheKey, _IsExitsCityList, 60);
			}
		}
		/// <summary>
		/// 初始化城市列表
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="tagType"></param>
		protected void ScriptCityIdList(int serialId, string tagType)
		{
			if (string.IsNullOrEmpty(tagType)) return;
			string cacheKey = "TreePageBase_ScriptCityIdList_" + serialId.ToString() + "_" + tagType;
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				_IsExitsCityList = (string)obj;
				return;
			}
			StringBuilder cityList = new StringBuilder();
			if (serialId == 0)
			{
				cityList = Get70CityIdList(tagType);

			}
			else
			{

				XmlNodeList xNodeList = NewsChannelBll.GetTreeHangQingSerialCityNumber(serialId, "xml");
				string searchString = "root/Province/City[@CityOrder='{0}']";
				if (xNodeList != null)
				{
					//循环城市
					foreach (XmlElement entity in xNodeList)
					{
						int cityOrder = string.IsNullOrEmpty(entity.GetAttribute("CityOrder"))
							? 0 : ConvertHelper.GetInteger(entity.GetAttribute("CityOrder"));
						int parentcityOrder = string.IsNullOrEmpty(entity.GetAttribute("ParentCityOrder"))
							? 0 : ConvertHelper.GetInteger(entity.GetAttribute("ParentCityOrder"));
						int cityId = ConvertHelper.GetInteger(entity.GetAttribute("ID"));

						string allSpell = "";
						string number = "";

						if (cityOrder > 0)
						{
							allSpell = entity.GetAttribute("EngName").ToLower();
							number = entity.GetAttribute(tagType);
						}
						else
						{
							XmlNode xnode = entity.ParentNode.ParentNode.SelectSingleNode(string.Format(searchString, parentcityOrder));
							if (xnode == null) continue;
							allSpell = ((XmlElement)xnode).GetAttribute("EngName").ToLower();
							number = ((XmlElement)xnode).GetAttribute(tagType);
						}
						cityList.AppendFormat(",\"{0}\":", cityId.ToString());
						cityList.Append("{");
						cityList.AppendFormat("\"allspell\":\"{0}\",\"num\":\"{1}\"", allSpell, number);
						cityList.Append("}");
					}
				}
			}

			if (cityList.Length < 1)
			{
				_IsExitsCityList = "var _CityList = {};";
				return;
			}
			else
			{
				cityList = cityList.Remove(0, 1);
				cityList.Append("};");
				cityList.Insert(0, "{");
				cityList.Insert(0, "var _CityList =");
				_IsExitsCityList = cityList.ToString();
				CacheManager.InsertCache(cacheKey, _IsExitsCityList, 60);
			}
		}
		/// <summary>
		/// 得到70个城市的ID列表
		/// </summary>
		/// <param name="tagType"></param>
		/// <returns></returns>
		private StringBuilder Get70CityIdList(string tagType)
		{
			HangQingTree hqTree = new HangQingTree();
			XmlDocument xmlDoc = hqTree.GetProvinceAndCityRelation();

			if (xmlDoc == null) return new StringBuilder();

			StringBuilder listString = new StringBuilder();

			XmlNodeList xNodeList = xmlDoc.SelectNodes("root/Province/City");
			string searchString = "root/Province/City[@CityOrder='{0}']";
			if (xNodeList == null || xNodeList.Count < 1) return listString;
			//循环城市
			foreach (XmlElement entity in xNodeList)
			{
				int cityOrder = string.IsNullOrEmpty(entity.GetAttribute("CityOrder"))
					? 0 : ConvertHelper.GetInteger(entity.GetAttribute("CityOrder"));
				int parentcityOrder = string.IsNullOrEmpty(entity.GetAttribute("ParentCityOrder"))
					? 0 : ConvertHelper.GetInteger(entity.GetAttribute("ParentCityOrder"));
				int cityId = ConvertHelper.GetInteger(entity.GetAttribute("ID"));

				string allSpell = "";
				string number = "";

				if (cityOrder > 0)
				{
					allSpell = entity.GetAttribute("EngName").ToLower();
					number = entity.GetAttribute(tagType);
				}
				else
				{
					XmlNode xnode = xmlDoc.SelectSingleNode(string.Format(searchString, parentcityOrder));
					if (xnode == null) continue;
					allSpell = ((XmlElement)xnode).GetAttribute("EngName").ToLower();
					number = ((XmlElement)xnode).GetAttribute(tagType);
				}
				listString.AppendFormat(",\"{0}\":", cityId.ToString());
				listString.Append("{");
				listString.AppendFormat("\"allspell\":\"{0}\",\"num\":\"{1}\"", allSpell, number);
				listString.Append("}");
			}

			return listString;
		}
		/// <summary>
		/// 初始化行情城市列表
		/// </summary>
		protected void ScriptCityIdListNew()
		{
			string cacheKey = "TreePageBase_ScriptHangqingCityIdList";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				_IsExitsCityList = (string)obj;
				return;
			}
			StringBuilder cityList = new StringBuilder();

			HangQingTree hqTree = new HangQingTree();
			XmlDocument xmlDoc = hqTree.GetProvinceAndCityRelationXmlDocument();
			if (xmlDoc != null)
			{
				XmlNodeList xNodeList = xmlDoc.SelectNodes("root/Province/City");
				string searchString = "root/Province/City[@CityOrder='{0}']";
				if (xNodeList != null && xNodeList.Count > 0)
				{
					Dictionary<int, int> cityNewsCount = new CarNewsBll().GetProviceCityNewsCount();
					if (cityNewsCount != null && cityNewsCount.Count > 0)
					{
						//循环城市
						foreach (XmlElement entity in xNodeList)
						{
							int cityOrder = string.IsNullOrEmpty(entity.GetAttribute("CityOrder"))
								? 0 : ConvertHelper.GetInteger(entity.GetAttribute("CityOrder"));
							int parentcityOrder = string.IsNullOrEmpty(entity.GetAttribute("ParentCityOrder"))
								? 0 : ConvertHelper.GetInteger(entity.GetAttribute("ParentCityOrder"));
							int cityId = ConvertHelper.GetInteger(entity.GetAttribute("ID"));

							string allSpell = "";
							string number = "";

							if (cityOrder > 0)
							{
								allSpell = entity.GetAttribute("EngName").ToLower();
								number = cityNewsCount.ContainsKey(cityId) ? cityNewsCount[cityId].ToString() : "0";
							}
							else
							{
								XmlNode xnode = xmlDoc.SelectSingleNode(string.Format(searchString, parentcityOrder));
								if (xnode == null) continue;
								allSpell = ((XmlElement)xnode).GetAttribute("EngName").ToLower();
								int pCityId = Convert.ToInt32(((XmlElement)xnode).GetAttribute("ID"));
								number = cityNewsCount.ContainsKey(pCityId) ? cityNewsCount[pCityId].ToString() : "0";
							}
							cityList.AppendFormat(",\"{0}\":", cityId.ToString());
							cityList.Append("{");
							cityList.AppendFormat("\"allspell\":\"{0}\",\"num\":\"{1}\"", allSpell, number);
							cityList.Append("}");
						}
					}
				}
			}

			if (cityList.Length < 1)
			{
				_IsExitsCityList = "var _CityList = {};";
				return;
			}
			else
			{
				cityList = cityList.Remove(0, 1);
				cityList.Append("};");
				cityList.Insert(0, "{");
				cityList.Insert(0, "var _CityList =");
				_IsExitsCityList = cityList.ToString();
				CacheManager.InsertCache(cacheKey, _IsExitsCityList, 60);
			}
		}
		/// <summary>
		/// 获取树形导航头
		/// </summary>
		/// <param name="brandType"></param>
		/// <param name="tagType"></param>
		/// <param name="objId"></param>
		/// <returns></returns>
		protected string GetTreeNavBarHtml(string brandType, string tagType, int objId)
		{
			// add by chengl Oct.12.2013
			// 车型首页新 定制搜索块
			if (brandType != "homenew")
			{ brandType = "home"; }//去掉面包屑 统一只读home
			string basePath = Path.Combine(WebConfig.DataBlockPath, "Data\\CarTree\\NavigationBar\\" + tagType + "\\" + brandType);
			string fileName = "home.htm";
			//brandType = brandType.ToLower();
			//switch (brandType)
			//{
			//    case "home":
			//        fileName = "home.htm";
			//        break;
			//    case "search":
			//        fileName = "search.htm";
			//        break;
			//    default:
			//        fileName = brandType + "_" + objId.ToString() + ".htm";
			//        break;
			//}
			fileName = Path.Combine(basePath, fileName);
			if (File.Exists(fileName))
				return File.ReadAllText(fileName);
			else
				return String.Empty;
		}
	}
}