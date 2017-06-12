using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.PageSerial
{
	public partial class CsSummaryNew : PageBase
	{
		private CommonHtmlBll _commonhtmlBLL;
		private Dictionary<int, string> dictSerialBlockHtml;//静态块内容

		protected int serialId;
		protected string serialSeoName;
		protected string masterBrandName;
		protected string masterBrandAllSpell;
		protected string serialSpell;
		protected string serialName;
		protected string serialShowName;
		protected string serialEncodeName;
		protected string serialReferPrice;
		protected string serialPrice;
		protected string serialExhaust;
		protected string serialTransmission;
		protected string CsPicJiaodian;			//子品牌焦点图
		protected string CsDetailInfo;			//子品牌概况
		protected string CsMustSeeInfo;			//子品牌必看
		protected string SerialInfoBarHtml;		//顶部的信息条
		protected string UCarHtml;				//二手车的信息
		protected string ImpressionHtml;		//网友印象

		protected string topCarListHtml;
		protected string noYearCarListHtml;
		protected string carListTableHtml;
		//protected string serialPositionImageHtml = string.Empty;//子品牌分类图片

		protected string focusNewsHtml;
		protected string xinwenHtml;
		protected string xinwenFirstHtml;
		protected string hangqingHtml;
		protected string hangqingFirstHtml;
		protected string shijiaHtml;
		protected string shijiaFirstHtml;
		protected string focusALinkStr;
		protected string xinwenALinkStr;
		protected string hangqingALinkStr;
		protected string shijiaALinkStr;

		protected string daogouHtml;
		protected string forumHtml;
		protected string rainbowHtml;
		protected string serialImageHtml;
		protected string videosHtml;
		protected string dianpingHtml;
		protected string hotNewsHtml;
		protected string serialToSeeHtml;
		protected string hotSerialCompareHtml;
		protected string intensionHtml;
		protected string CsHead;
		// add by chengl 综述页脚本
		protected string CsHeadJs;
		protected string UserBlock;
		protected string baaUrl;
		protected string FlashADCode;
		protected string serialAskHtml;
		protected string maintainceHtml;
		// protected string friendLinkHtml;			//友情链接
		protected string serialWhiteImageUrl = string.Empty;//白底封面图

		protected string nextSeePingceHtml;
		protected string nextSeeXinwenHtml;
		protected string nextSeeDaogouHtml;
		protected int[] SpecialSerialIdArray = { 2370, 2608, 3398, 3023, 2388 };//特殊子品牌id 展示
		//焦点图新闻地址
		private string _FocusNewsHtmlPage = "Data\\SerialNews\\FocusNews\\Html\\Serial_FocusNews_{0}.html";
		private string _PingJiaHtmlPage = "Data\\SerialNews\\pingjia\\Html\\Serial_All_News_{0}.html";

		#region 子品牌最热车型
		// 最热车型ID
		private int hotCarID = 0;
		// 最热车型热度
		private int hotCarHotCount = 0;
		#endregion

		#region 图释部分变量
		protected string serialImageCarHtml;//图释
		//图释html路径
		private const string _SerialImageCarPath = "Data\\SerialSet\\SerialColorImage\\SerialColorImage_{0}.html";
		#endregion

		#region 编辑试驾评测
		protected string editorCommentHtml;
		private const string _EditorCommentHtmlPath = "Data\\SerialSet\\EditorCommentHtml\\Serial_EditorComment_cs_{0}.html";
		#endregion

		//#region 车型路径栏 广告
		//private string hasPathADCsList = ",1733,1736,2604,2742,1925,2733,1740,3151,3103,3023,2857,2587,2767,2683,2833,2865,1648,3263,1649,2895,3164,3153,3152,1635,2866,1650,1636,1617,1619,1605,1618,2713,1851,1838,2012,1834,1839,1827,1833,1835,2410,1828,1930,2608,2945,2862,2388,2614,2731,1703,1825,3302,2601,";
		//protected string hasPathADHTML = "<div class=\"top_ad\"><a target=\"_blank\" href=\"http://market.bitauto.com/gmac2010/index.aspx\"><img src=\"/images/cssummarytoppathad.jpg\" alt=\"\" /></a></div>";
		//#endregion

		#region SEO 测试用
		//protected bool isSpecialTitle = false;
		//private string specialTitleIDList = ",2566,2886,2871,2370,1991,2381,1617,1599,1828,2598,1692,2576,2043,2608,1833,2420,1594,2947,2677,1596,";
		//protected string seoTestTitle = ""; 
		#endregion

		protected EnumCollection.SerialInfoCard sic;	//子品牌名片
		protected SerialEntity cse;						//子品牌信息
		private string baseUrl;
		private Dictionary<int, string> dicUcarPrice;	// 二手车报价区间

		private Car_SerialBll _serialBLL;
		private Car_BasicBll basicBll;
		//private SerialGoodsBll _serialGoodsBLL;

		//private List<SerialGoodsCarEntity> serialGoodsCarList;//易车惠 商品 车型列表

		public CsSummaryNew()
		{
			_serialBLL = new Car_SerialBll();
			basicBll = new Car_BasicBll();
			_commonhtmlBLL = new CommonHtmlBll();
			//_serialGoodsBLL = new SerialGoodsBll();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			GetParamter();

			base.MakeSerialTopADCode(serialId);
			MakeFlashADCode();

			//子品牌信息
			sic = new Car_SerialBll().GetSerialInfoCard(serialId);
			if (sic.CsID == 0)
			{
				Response.Redirect("/car/404error.aspx?info=无子品牌");
			}
			cse = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
			serialSpell = cse.AllSpell;
			serialShowName = cse.ShowName;
			if (serialId == 1568)
				serialShowName = "索纳塔八";
			serialEncodeName = HttpUtility.UrlEncode(serialShowName);
			serialSeoName = cse.SeoName;
			serialName = cse.Name;

			//serialPrice = sic.CsPriceRange.Replace("万-", "-");
			serialPrice = sic.CsPriceRange;
			if (serialPrice.Length == 0)
				serialPrice = "暂无报价";

			baseUrl = "/" + serialSpell.ToLower() + "/";
			masterBrandName = cse.Brand.Name;
			masterBrandAllSpell = cse.Brand.AllSpell;
			baaUrl = new Car_SerialBll().GetForumUrlBySerialId(serialId);
			serialWhiteImageUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_3.");
			CsHead = base.GetCommonNavigation("CsSummary", serialId);
			CsHeadJs = base.GetCommonNavigation("CsSummaryJs", serialId);
			// 子品牌信息块来源NAS
			SerialInfoBarHtml = base.GetCommonNavigation("SerialInfoBar", serialId);
			//静态块内容
			dictSerialBlockHtml = _commonhtmlBLL.GetCommonHtml(serialId, CommonHtmlEnum.TypeEnum.Serial, CommonHtmlEnum.TagIdEnum.SerialSummary);
			////易车惠 商品 车型列表
			//serialGoodsCarList = _serialGoodsBLL.GetGoodsCarList(serialId);
			//生成页面
			RenderPage();
		}

		/// <summary>
		/// 获取参数
		/// </summary>
		private void GetParamter()
		{
			serialId = ConvertHelper.GetInteger(Request.QueryString["ID"]);
			if (serialId <= 0)
			{
				Response.Redirect("/404error.aspx");
			}
		}


		/// <summary>
		/// 生成页面代码
		/// </summary>
		private void RenderPage()
		{
			Car_SerialBll serialBll = new Car_SerialBll();
			//MakeSerialList();
			MakeCarListHtmlNew();
			// MakeSerialInfoBar();
			MakeSerialFocus();
			MakeSerialOverview();
			MakeMustSee();
			MakeTopNews();
			// modified by chengl Apr.28.2011
			// MakeRainbowHtml();
			MakeSerialImageCarHTML();
			MakeSerialImages();
			//MakeVideHtml();
			NewMakeVideoBlockHtml();
			//MakeSerialPositionImage();

			//MakeDianpingHtml();
			dianpingHtml = serialBll.GetDianpingHtml(serialId);
			// modified by chengl Apr.27.2011
			// MakeHotNewsHtml();
			MakeSerialToSerialHtml();
			MakeHotSerialCompare();
			//MakeMaintainceHtml();
			// modified by chengl Jun.29.2010
			// MakeSerialIntensionHtml();
			// modified by chengl Apr.27.2011
			// GetUserBlockByCarSerialId();
			MakeEditorCommentHtml();
			MakeSerialAskHtml();
			// friendLinkHtml = new Car_SerialBll().GetSerialFriendLinkHtml(serialId);
			//InitNextSee();
			InitNextSeeNew();
			//网友印象
			GetKoubeiImpression();
			// modified Apr.5.2012 已改成IP定向 Ajax
			////二手车
			//UCarHtml = serialBll.GetUCarHtml(serialId);
		}
		///// <summary>
		///// 子品牌分类图片
		///// </summary>
		//private void MakeSerialPositionImage()
		//{
		//    //add by sk 2013.06.17 车款列表新规则修改，（朗逸：2370 科鲁兹：2608 起亚K2：3398 长城C30: 3023 凯越：2388 ）
		//    int[] specialSerialIdArray = { 2370, 2608, 3398, 3023, 2388, 1765, 2871, 1793, 1905, 1611 };
		//    if (!specialSerialIdArray.Contains(cse.Id)) return;
		//    SerialPositionEntity serialPositionEntity = _serialBLL.GetSerialPositionImage(serialId);
		//    if (serialPositionEntity.ImageCount <= 0) return;
		//    StringBuilder sb = new StringBuilder();
		//    sb.Append("<div class=\"line_box\">");
		//    sb.Append("	<h3>");
		//    sb.AppendFormat("		<span>{0}图片</span>", cse.ShowName);
		//    sb.AppendFormat("		<em><a rel=\"nofollow\" href=\"{0}\" target=\"_blank\">共{1}张图片</a></em>", serialPositionEntity.Url, serialPositionEntity.ImageCount);
		//    sb.Append("	</h3>");
		//    sb.Append("	<div class=\"car-pic\">");
		//    sb.Append("		<div class=\"car-pic-main\">");
		//    if (serialPositionEntity.SerialPositionImageList.Count > 0)
		//    {
		//        sb.AppendFormat("			<div class=\"pic-box\"><a href=\"{0}?ref=tupian\" target=\"_blank\"><img data-original=\"{1}\" title=\"{2}\" alt=\"{2}\"></a></div>",
		//            serialPositionEntity.SerialPositionImageList[0].Url,
		//            string.Format(serialPositionEntity.SerialPositionImageList[0].ImageUrl, 4),
		//            serialPositionEntity.SerialPositionImageList[0].PositionName);
		//    }
		//    else
		//    {
		//        sb.AppendFormat("			<div class=\"pic-box\"><a href=\"javascript:;\"><img data-original=\"{0}\"></a></div>", WebConfig.DefaultCarPic);
		//    }
		//    sb.Append("			<div class=\"pic-detail\">");
		//    List<SerialCategoryEntity> categoryList = serialPositionEntity.SerialCategoryList.FindAll(p => (p.Name == "外观" || p.Name == "内饰" || p.Name == "空间" || p.Name == "图解"));
		//    for (int i = 0; i < categoryList.Count; i++)
		//    {
		//        sb.Append("<p>");
		//        sb.AppendFormat(" <a href=\"{0}\" target=\"_blank\">{2}</a><span>{1}张</span>", categoryList[i].Url, categoryList[i].ImageCount, categoryList[i].Name);
		//        sb.Append("</p>");
		//        if (i < categoryList.Count - 1)
		//            sb.Append("<b></b>");
		//    }
		//    sb.Append("			</div>");
		//    sb.Append("		</div>");
		//    if (serialPositionEntity.SerialPositionImageList.Count > 0)
		//    {
		//        sb.Append("<div class=\"car-pic-sub\">");
		//        sb.Append("<ul>");
		//        for (int i = 1; i <= 6 && i < serialPositionEntity.SerialPositionImageList.Count; i++)
		//        {
		//            sb.AppendFormat("<li><a href=\"{0}?ref=tupian\" target=\"_blank\"><img data-original=\"{1}\" title=\"{2}\" alt=\"{2}\"></a></li>",
		//                serialPositionEntity.SerialPositionImageList[i].Url,
		//                string.Format(serialPositionEntity.SerialPositionImageList[i].ImageUrl, 1),
		//                serialPositionEntity.SerialPositionImageList[i].PositionName);
		//        }
		//        //补图片
		//        if (serialPositionEntity.SerialPositionImageList.Count < 7)
		//        {
		//            for (int i = 6; i > serialPositionEntity.SerialPositionImageList.Count - 1; i--)
		//            {
		//                sb.AppendFormat("<li><a href=\"javascript:;\"><img data-original=\"{0}\"></a></li>", WebConfig.DefaultCarPic);
		//            }
		//        }
		//        sb.Append("</ul>");
		//    }
		//    sb.Append("</div>");
		//    sb.Append("	</div>");
		//    sb.Append("	<div class=\"clear\"></div>");
		//    sb.Append("	<div class=\"more\">");
		//    sb.AppendFormat("<a rel=\"nofollow\" href=\"{0}\" target=\"_blank\">更多&gt;&gt;</a>", serialPositionEntity.Url);
		//    sb.Append("	</div>");
		//    sb.Append("</div>");
		//    serialPositionImageHtml = sb.ToString();
		//}

		//private void MakeSerialInfoBar()
		//{
		//    string[] htmlCode = new string[15];
		//    if (topCarListHtml.Length > 0 || noYearCarListHtml.Length > 0)
		//    {
		//        htmlCode[0] = "<div class=\"line_box zs01\">";
		//        htmlCode[1] = "<ul class=\"s\">";
		//        htmlCode[2] = "<li class=\"s1\"><label>厂家指导价：</label>" + serialReferPrice + "</li>";
		//        htmlCode[3] = "<li class=\"s2\"><label>商家报价：</label><span class=\"important\"><a href=\"/" + serialSpell + "/baojia/\"  target=\"_blank\">" + serialPrice + "</a></span></li>";
		//        // add by chengl Jan.17.2012
		//        Dictionary<int, string> dicHQPrice = new HangQingTree().GetAllSerialHangQingPrice();
		//        if (dicHQPrice != null && dicHQPrice.Count > 0 && dicHQPrice.ContainsKey(serialId))
		//        { htmlCode[4] = "<li class=\"s2\"><label>行情价：</label><span class=\"important\"><a target=\"_blank\" id=\"linkForHQPrice\" href=\"/" + serialSpell + "/hangqing/\">" + dicHQPrice[serialId] + "</a></span></li>"; }
		//        else
		//        { htmlCode[4] = ""; }
		//        htmlCode[5] = "<li class=\"s3\"><label>排量：</label>" + serialExhaust + "</li>";
		//        htmlCode[6] = "<li class=\"s4\"><label>变速箱：</label>" + serialTransmission + "</li>";
		//        htmlCode[7] = "</ul>";
		//        htmlCode[8] = "<div class=\"clear\"></div>";
		//        /*
		//        htmlCode[8] = string.Format(@"<ul class='favorite'><a href='javascript:void(0);' target='_self' id='addFavorites'>添加到收藏夹</a> 
		//                                | <a href='/interfaceforbitauto/SavePageDestop.ashx?id={0}'>保存到桌面</a></ul>", serialId);
		//        */
		//        htmlCode[9] = "</div>";
		//    }
		//    SerialInfoBarHtml = String.Concat(htmlCode);
		//}

		private void GetKoubeiImpression()
		{
			string htmlFile = Path.Combine(WebConfig.DataBlockPath, "data\\SerialDianping\\Impression\\Html\\Impression_" + serialId + ".html");
			if (File.Exists(htmlFile))
				ImpressionHtml = File.ReadAllText(htmlFile);
			else
				ImpressionHtml = String.Empty;
		}

		///// <summary>
		///// 生成在售的车款列表
		///// </summary>
		//private void MakeSerialList()
		//{
		//    //add by sk 2013.06.17 车款列表新规则修改，（朗逸：2370 科鲁兹：2608 起亚K2：3398 长城C30: 3023 凯越：2388 ）
		//    int[] specialSerialIdArray = { 2370, 2608, 3398, 3023, 2388 };
		//    if (specialSerialIdArray.Contains(cse.Id))
		//    {
		//        #region 热门车行ID获取 add by chengl Jul.24.2013
		//        List<EnumCollection.CarInfoForSerialSummary> lsTemp = new List<EnumCollection.CarInfoForSerialSummary>();
		//        if (cse.SaleState == "停销")
		//        {
		//            // 停销子品牌取 停销子品牌最新年款的车型(高总逻辑 Oct.11.2011)
		//            lsTemp = GetAllCarInfoForNoSaleSerialSummaryByCsID(serialId);
		//        }
		//        else
		//        {
		//            // 非停销子品牌取 子品牌的非停销所有年款车型
		//            lsTemp = base.GetAllCarInfoForSerialSummaryByCsID(serialId);
		//        }
		//        foreach (EnumCollection.CarInfoForSerialSummary carInfo in lsTemp)
		//        {
		//            // add by chengl Dec.15.2011
		//            if (carInfo.CarPV >= hotCarHotCount)// && carInfo.CarPriceRange.Trim().Length > 0)
		//            {
		//                // 取有报价车型中最热的车型 "预约试驾" url使用
		//                hotCarID = carInfo.CarID;
		//                hotCarHotCount = carInfo.CarPV;
		//            }
		//        }
		//        #endregion

		//        string fileName = Path.Combine(WebConfig.DataBlockPath, string.Format(@"Data\SerialSummary\{0}.html", cse.Id));
		//        if (File.Exists(fileName))
		//            carListTableHtml = File.ReadAllText(fileName);
		//        return;
		//    }
		//    List<string> htmlList = new List<string>();

		//    // modified by chengl Oct.11.2011
		//    List<EnumCollection.CarInfoForSerialSummary> ls = new List<EnumCollection.CarInfoForSerialSummary>();
		//    List<EnumCollection.CarInfoForSerialSummary> saleCarInfo = new List<EnumCollection.CarInfoForSerialSummary>();
		//    if (cse.SaleState == "停销")
		//    {
		//        // 停销子品牌取 停销子品牌最新年款的车型(高总逻辑 Oct.11.2011)
		//        ls = GetAllCarInfoForNoSaleSerialSummaryByCsID(serialId);
		//    }
		//    else
		//    {
		//        // 非停销子品牌取 子品牌的非停销所有年款车型
		//        ls = base.GetAllCarInfoForSerialSummaryByCsID(serialId);
		//    }
		//    saleCarInfo = base.GetAllCarInfoForSerialSummaryByCsID(serialId, true);
		//    ls.Sort(NodeCompare.CompareCarByExhaust);
		//    //排量列表
		//    List<string> exhaustList = new List<string>();
		//    //变速箱列表
		//    List<string> transList = new List<string>();
		//    //年款列表
		//    List<string> yearList = new List<string>();
		//    //在售年款
		//    List<string> saleYearList = new List<string>();
		//    //停售年款
		//    List<string> nosaleYearList = new List<string>();

		//    Dictionary<string, string> yearHtmlDic = new Dictionary<string, string>();
		//    int maxPv = 0;
		//    double maxPrice = Double.MinValue;
		//    double minPrice = Double.MaxValue;
		//    foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
		//    {
		//        if (carInfo.CarPV > maxPv)
		//            maxPv = carInfo.CarPV;
		//        if (carInfo.CarYear.Length > 0)
		//        {
		//            string yearType = carInfo.CarYear + "款";
		//            if (!yearList.Contains(yearType))
		//                yearList.Add(yearType);
		//        }
		//        double referPrice = 0.0;
		//        bool isDouble = Double.TryParse(carInfo.ReferPrice.Replace("万", ""), out referPrice);
		//        if (isDouble)
		//        {
		//            if (referPrice > maxPrice)
		//                maxPrice = referPrice;
		//            if (referPrice < minPrice)
		//                minPrice = referPrice;
		//        }
		//    }
		//    foreach (EnumCollection.CarInfoForSerialSummary carInfo in saleCarInfo)
		//    {
		//        if (carInfo.CarYear.Length > 0)
		//        {
		//            string yearType = carInfo.CarYear + "款";
		//            if (carInfo.SaleState == "停销")
		//            {
		//                if (!nosaleYearList.Contains(yearType))
		//                    nosaleYearList.Add(yearType);
		//            }
		//            else
		//            {
		//                if (!saleYearList.Contains(yearType))
		//                    saleYearList.Add(yearType);
		//            }
		//        }
		//    }
		//    //排除包含在售年款
		//    foreach (string year in saleYearList)
		//    {
		//        if (nosaleYearList.Contains(year))
		//        {
		//            nosaleYearList.Remove(year);
		//        }
		//    }
		//    yearList.Sort(NodeCompare.CompareStringDesc);
		//    saleYearList.Sort(NodeCompare.CompareStringDesc);
		//    nosaleYearList.Sort(NodeCompare.CompareStringDesc);

		//    if (maxPrice == Double.MinValue && minPrice == Double.MaxValue)
		//        serialReferPrice = "暂无";
		//    else
		//    {
		//        serialReferPrice = minPrice + "万-" + maxPrice + "万";
		//    }

		//    if (cse.SaleState != "停销")
		//    {
		//        htmlList.Add("<h3><span>" + serialSeoName + "-在售车款</span><em class=\"h3_spcar\">");
		//        for (int i = 0; i < saleYearList.Count; i++)
		//        {
		//            //// modified by chengl Jun.24.2010
		//            if (i >= 2)
		//            {
		//                break;
		//            }
		//            string yearStr = saleYearList[i];
		//            if (i > 0)
		//                htmlList.Add("<s>|</s>");
		//            string url = baseUrl + yearStr.Replace("款", "") + "/";
		//            htmlList.Add("<a href=\"" + url + "#car_list\" target=\"_self\">" + yearStr + "</a>");
		//        }
		//    }
		//    else
		//    {
		//        htmlList.Add("<h3><span>" + serialSeoName + "-停售车款</span><em class=\"h3_spcar\">");
		//    }

		//    if (serialId != 1568 && base.CheckSerialHasNoSale(serialId))
		//    {
		//        //if (yearList.Count > 0)
		//        //    htmlList.Add("|");
		//        //htmlList.Add("<a href=\"http://www.cheyisou.com/chexing/" + Server.UrlEncode(serialShowName) + "/1.html?para=os|0|en|utf8\" >停售车款</a>");
		//        if (nosaleYearList.Count > 0)
		//        {
		//            if (saleYearList.Count > 0)
		//            {
		//                htmlList.Add("<s>|</s>");
		//            }
		//            htmlList.Add("<dl id=\"bt_car_spcar_table\"><dt>停售年款<em></em></dt><dd style=\"display:none;\">");
		//            for (int i = 0; i < nosaleYearList.Count; i++)
		//            {
		//                string url = baseUrl + nosaleYearList[i].Replace("款", "") + "/#car_list";
		//                if (i == nosaleYearList.Count - 1)
		//                    htmlList.Add("<a href=\"" + url + "\" target=\"_self\" class=\"last_a\">" + nosaleYearList[i] + "</a>");
		//                else
		//                {
		//                    htmlList.Add("<a href=\"" + url + "\" target=\"_self\">" + nosaleYearList[i] + "</a>");
		//                }
		//            }
		//            htmlList.Add("</dd></dl>");
		//        }
		//    }
		//    htmlList.Add("</em></h3>");

		//    htmlList.Add("<div class=\"comparetable\">");

		//    htmlList.Add("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"compare2\" id=\"compare\">");


		//    List<string> tempList = new List<string>();
		//    string carIDs = string.Empty;
		//    int index = 0;
		//    // 停销子品牌 显示二手车报价
		//    if (cse.SaleState == "停销")
		//    {
		//        dicUcarPrice = new Car_BasicBll().GetAllUcarPrice();
		//    }

		//    foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
		//    {
		//        if (!exhaustList.Contains(carInfo.Engine_Exhaust))
		//        {
		//            if (carIDs != "")
		//            {
		//                htmlList.Add(string.Format(String.Concat(tempList.ToArray()), carIDs));
		//                tempList.Clear();
		//            }
		//            carIDs = "";
		//            //显示排量行
		//            exhaustList.Add(carInfo.Engine_Exhaust);
		//            if (index < 1)
		//            {
		//                tempList.Add("<tr style=\"\"><th width=\"255px\" class=\"firstItem\"><b>" + carInfo.Engine_Exhaust + "排量</b></th>");
		//                tempList.Add("<th width='50px' class=\"pdLeftOne\">热度</th>");
		//                tempList.Add("<th width='70px' class=\"pdLeftOne\">变速箱</th>");
		//                tempList.Add("<th width='59px' class=\"pdRightTwo\">指导价</th>");
		//                if (cse.SaleState == "停销")
		//                {
		//                    tempList.Add("<th width='125px' class=\"pdRightThree\">二手车报价</th>");
		//                }
		//                else
		//                {
		//                    tempList.Add("<th width='125px' class=\"pdRightThree\">商家报价</th>");
		//                }
		//                tempList.Add("<th width='58px'>&nbsp;</th>");
		//                tempList.Add("</tr>");
		//            }
		//            else
		//            {
		//                tempList.Add("<tr style=\"\"><th width=\"255px\" colspan=\"6\" class=\"firstItem\"><b>" + carInfo.Engine_Exhaust + "排量</b></th></tr>");
		//            }
		//            index++;
		//            //tempList.Add("<tr class=\"classify\"><td colspan=\"6\"><b>" + carInfo.Engine_Exhaust + "排量</b>&nbsp;&nbsp;");
		//            //tempList.Add("<a class=\"cGray\" href=\"/car/interfaceforbitauto/ForBitAutoCompare.aspx?isNewID=1&carIDs={0}\">同排量对比&gt;&gt;</a></td></tr>");
		//        }
		//        if (carIDs != "")
		//        { carIDs += "," + carInfo.CarID; }
		//        else
		//        { carIDs += carInfo.CarID; }

		//        string carUrl = "/" + serialSpell + "/m" + carInfo.CarID + "/";

		//        string yearType = carInfo.CarYear.Trim();
		//        if (yearType.Length > 0)
		//            yearType += "款";
		//        else
		//            yearType = "未知年款";

		//        if (!yearHtmlDic.ContainsKey(yearType))
		//            yearHtmlDic[yearType] = "<ul>";


		//        yearHtmlDic[yearType] += "<li><a href=\"" + carUrl + "\" >" + carInfo.CarName + "</a></li>";

		//        string carFullName = "";

		//        // modified by chengl Mar.27.2012
		//        // 客户要求将世嘉三厢更名为新世嘉
		//        if (yearType == "2012款" && serialShowName == "世嘉三厢")
		//        { carFullName = "新世嘉&nbsp;" + carInfo.CarName; }
		//        // 又一狗屁逻辑  对于2013款新车的车型列表中，需要将“新胜达”名称调整为“全新胜达”
		//        // modified by chengl Jan.23.2013
		//        else if (yearType == "2013款" && serialId == 1848)
		//        { carFullName = "全新胜达&nbsp;" + carInfo.CarName; }
		//        // modified by chengl Mar.15.2013 for gaoyan
		//        else if (yearType == "2012款" && serialId == 1785)
		//        { carFullName = "奇瑞QQ3&nbsp;" + carInfo.CarName; }
		//        else
		//        { carFullName = serialShowName + "&nbsp;" + carInfo.CarName; }

		//        if (carInfo.CarName.StartsWith(serialShowName))
		//            carFullName = serialShowName + "&nbsp;" + carInfo.CarName.Substring(serialShowName.Length);
		//        if (yearType != "未知年款")
		//            carFullName = yearType + " " + carFullName;

		//        string stopPrd = "";
		//        if (carInfo.ProduceState == "停产")
		//            stopPrd = " <span class=\"tc\">停产</span>";

		//        // 节能补贴 Sep.2.2010
		//        string hasEnergySubsidy = "";
		//        bool isHasEnergySubsidy = new Car_BasicBll().CarHasParamEx(carInfo.CarID, 853);
		//        if (isHasEnergySubsidy)
		//        {
		//            hasEnergySubsidy = " <span class=\"butie\"><a href=\"http://news.bitauto.com/consumerpolicy/20120704/1805753482.html\" title=\"可获得3000元节能补贴\" >补贴</a></span>";
		//        }
		//        //============2012-04-09 减税============================
		//        Dictionary<int, string> dict = basicBll.GetCarAllParamByCarID(carInfo.CarID);
		//        string strTravelTax = "";
		//        if (dict.ContainsKey(895))
		//        {
		//            strTravelTax = " <span class=\"jianshui\"><a target=\"_blank\" title=\"{0}\" href=\"http://news.bitauto.com/others/20120308/0805618954.html\">减税</a></span>";
		//            if (dict[895] == "减半")
		//                strTravelTax = string.Format(strTravelTax, "减征50%车船使用税");
		//            else if (dict[895] == "免征")
		//                strTravelTax = string.Format(strTravelTax, "免征车船使用税");
		//            else
		//                strTravelTax = "";
		//        }
		//        tempList.Add("<tr><td><a href=\"" + carUrl + "\" >" + carFullName + "</a>" + stopPrd + strTravelTax + hasEnergySubsidy + "</td>");
		//        //计算百分比
		//        int percent = (int)Math.Round((double)carInfo.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero);
		//        tempList.Add("<td><div class=\"w\"><div class=\"p\"  style=\"width:" + percent + "%\"></div></div></td>");

		//        // add by chengl Dec.15.2011
		//        if (carInfo.CarPV >= hotCarHotCount)// && carInfo.CarPriceRange.Trim().Length > 0)
		//        {
		//            // 取有报价车型中最热的车型 "预约试驾" url使用
		//            hotCarID = carInfo.CarID;
		//            hotCarHotCount = carInfo.CarPV;
		//        }

		//        //变速器类型
		//        string tempTransmission = carInfo.TransmissionType;
		//        if (tempTransmission.IndexOf("挡") >= 0)
		//        {
		//            tempTransmission = tempTransmission.Substring(tempTransmission.IndexOf("挡") + 1, tempTransmission.Length - tempTransmission.IndexOf("挡") - 1);
		//        }
		//        tempTransmission = tempTransmission.Replace("变速器", "").Replace("CVT", "");
		//        tempList.Add("<td>" + tempTransmission + "</td>");

		//        if (transList.Count < 2)
		//        {
		//            if (tempTransmission.IndexOf("手动") == -1)
		//                tempTransmission = "自动";
		//            if (!transList.Contains(tempTransmission))
		//                transList.Add(tempTransmission);
		//        }


		//        //指导价
		//        if (carInfo.ReferPrice.Trim().Length == 0)
		//            tempList.Add("<td class=\"noPrice1\" style=\"text-align:right\">暂无</td>");
		//        else
		//            tempList.Add("<td style=\"text-align:right\"><span>" + carInfo.ReferPrice + "万</span><a title=\"购车费用计算\" class=\"icon_cal\" href=\"/gouchejisuanqi/?carid=" + carInfo.CarID + "\"></a></td>");

		//        // 			if (carInfo.PerfFuelCostPer100.Trim().Length == 0)
		//        // 				temp.Append("<td style=\"text-align:right\"></td>");
		//        // 			else
		//        // 				temp.Append("<td style=\"text-align:right\">" + carInfo.PerfFuelCostPer100 + "L</td>");
		//        //报价

		//        if (cse.SaleState != "停销")
		//        {
		//            if (carInfo.CarPriceRange.Trim().Length == 0)
		//                tempList.Add("<td class=\"noPrice2\" style=\"text-align:right\">暂无报价</td>");
		//            else
		//            {
		//                ////tempList.Add("<td style=\"text-align:right\"><span><a href=\"/" + serialSpell + "/m" + carInfo.CarID + "/baojia/\" >" + carInfo.CarPriceRange + "</a></span> <a class=\"cGray\" href=\"/" + serialSpell + "/m" + carInfo.CarID + "/baojia/\" >查看>></a></td>");
		//                //20130412 edit anh
		//                //tempList.Add("<td style=\"text-align:right\"><span><a href=\"/"
		//                //    + serialSpell + "/m" + carInfo.CarID
		//                //    + "/baojia/\" >" + carInfo.CarPriceRange
		//                //    + "</a></span> <a href='/"
		//                //    + serialSpell + "/m" + carInfo.CarID
		//                //    + "/baojia/#V'>询价>></a></td>");
		//                tempList.Add(string.Format("<td style=\"text-align:right\"><span><a href=\"/{0}/m{1}/baojia/\" >{2}</a></span> <a href='http://dealer.bitauto.com/zuidijia/nb{3}/nc{1}/'>询价>></a></td>"
		//                    , serialSpell, carInfo.CarID, carInfo.CarPriceRange, serialId));
		//            }
		//        }
		//        else
		//        {
		//            // 停销子品牌 显示二手车报价
		//            if (dicUcarPrice != null && dicUcarPrice.Count > 0 && dicUcarPrice.ContainsKey(carInfo.CarID))
		//            {
		//                tempList.Add("<td style=\"text-align:right\"><span><a href=\"http://yiche.taoche.com/buycar/b-"
		//                    + serialSpell + "/?page=1&carid=" + carInfo.CarID
		//                    + "\" >" + dicUcarPrice[carInfo.CarID]
		//                    + "</a></span></td>");
		//            }
		//            else
		//            {
		//                tempList.Add("<td class=\"noPrice2\" style=\"text-align:right\">暂无报价</td>");
		//            }

		//        }
		//        tempList.Add("<td id=\"tdForCompareCar_" + carInfo.CarID + "\" class=\"small\">");
		//        tempList.Add("<a class=\"addCompare\" target=\"_self\" href=\"javascript:addCarToCompare('" + carInfo.CarID.ToString() + "','" + carInfo.CarName.ToString() + "');\" >+对比</a></td></tr>");
		//    }
		//    if (carIDs != "")
		//    {
		//        htmlList.Add(string.Format(String.Concat(tempList.ToArray()), carIDs));
		//    }
		//    else
		//        htmlList.Add("<tr><td class=\"noline\" colspan=\"7\">暂无在售车款！</td></tr>");
		//    htmlList.Add("</table>");
		//    //htmlList.Add("<div class=\"more\"><a href=\"http://go.bitauto.com/goumai/?id=" + serialId + "\" class=\"more2new\">计划购买</a><a href=\"http://go.bitauto.com/guanzhu/?id=" + serialId + "\" class=\"more3new\">收藏</a><a class=\"more2new\" href=\"http://ask.bitauto.com/browse/" + serialId + "/\">买前咨询</a></div>");
		//    htmlList.Add("</div>");
		//    htmlList.Add("<div class=\"clear\"></div>");
		//    //20130512 edit anh
		//    //// add by chengl Dec.13.2011
		//    //// 如果有最热 有报价车型
		//    //if (hotCarID > 0)
		//    //{
		//    //    htmlList.Add(string.Format("<div class=\"more\"><a href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">我要预约试驾&gt;&gt;</a></div>", serialId));
		//    //}
		//    //else
		//    //{
		//    htmlList.Add(string.Format("<div class=\"more\"><a href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">我要预约试驾&gt;&gt;</a></div>", serialId));
		//    //}
		//    carListTableHtml = String.Concat(htmlList.ToArray());

		//    //年款下拉列表
		//    StringBuilder listCode = new StringBuilder();
		//    foreach (string yearType in yearList)
		//    {
		//        listCode.Append("<h5><a href=\"" + baseUrl + yearType.Replace("款", "") + "/\" target=\"_self\" >" + yearType + "</a></h5>");
		//        listCode.Append(yearHtmlDic[yearType] + "</ul>");
		//    }
		//    topCarListHtml = listCode.ToString();
		//    if (yearHtmlDic.ContainsKey("未知年款"))
		//        noYearCarListHtml = yearHtmlDic["未知年款"] + "</ul>";
		//    else
		//        noYearCarListHtml = "";
		//    exhaustList.Remove("");
		//    transList.Remove("");

		//    //生成在售的排量与变速箱列表
		//    if (exhaustList.Count > 5)
		//        serialExhaust = String.Join("　", new string[] { exhaustList[0], exhaustList[1], exhaustList[2] + "…" + exhaustList[exhaustList.Count - 1] });
		//    else
		//        serialExhaust = String.Join("　", exhaustList.ToArray());
		//    if (transList.Count > 3)
		//        serialTransmission = transList[0] + "　" + transList[1] + "…" + transList[transList.Count - 1];
		//    else
		//        serialTransmission = String.Join("　", transList.ToArray());

		//}

		#region 新版 车型列表 add by sk 2013.08.05
		/// <summary>
		/// 子品牌 车款列表 html
		/// </summary>
		/// <param name="serialId">子品牌ID</param>
		private void MakeCarListHtmlNew()
		{
			StringBuilder sb = new StringBuilder();
			List<string> carSaleListHtml = new List<string>();
			List<string> carNoSaleListHtml = new List<string>();
			List<string> carWaitSaleListHtml = new List<string>();

			List<CarInfoForSerialSummaryEntity> carinfoList = basicBll.GetCarInfoForSerialSummaryBySerialId(serialId);
			int maxPv = 0;
			List<string> saleYearList = new List<string>();
			List<string> noSaleYearList = new List<string>();
			foreach (CarInfoForSerialSummaryEntity carInfo in carinfoList)
			{
				if (carInfo.CarPV > maxPv)
					maxPv = carInfo.CarPV;
				if (carInfo.CarYear.Length > 0)
				{
					string yearType = carInfo.CarYear + "款";

					if (carInfo.SaleState == "停销")
					{
						if (!noSaleYearList.Contains(yearType))
							noSaleYearList.Add(yearType);
					}
					else
					{
						if (!saleYearList.Contains(yearType))
							saleYearList.Add(yearType);
					}
				}
			}
			//排除包含在售年款
			foreach (string year in saleYearList)
			{
				if (noSaleYearList.Contains(year))
				{
					noSaleYearList.Remove(year);
				}
			}
			List<CarInfoForSerialSummaryEntity> carinfoSaleList = carinfoList
				.FindAll(p => p.SaleState == "在销");
			//List<CarInfoForSerialSummaryEntity> carinfoNoSaleList = carinfoList.FindAll(p => p.SaleState == "停销");
			List<CarInfoForSerialSummaryEntity> carinfoWaitSaleList = carinfoList
				.FindAll(p => p.SaleState == "待销");

			carinfoSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
			//carinfoNoSaleList.Sort(StaticCompare.CompareCarByExhaustAndPowerAndInhaleType);
			carinfoWaitSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);

			noSaleYearList.Sort(NodeCompare.CompareStringDesc);

			//sb.Append("<div class=\"line_box\" id=\"car_list\">");
			sb.Append("    <h3>");
			sb.AppendFormat("        <span>{0}</span>", serialSeoName);
			sb.Append("        <div class=\"h3_tab car-comparetable-tab\">");
			sb.Append("<ul id=\"data_tab_jq5\">");
			bool isWaitSale = false;
			if (carinfoWaitSaleList.Count > 0)
			{
				isWaitSale = true;
				sb.Append("<li class=\"\">预售车款</li>");
			}
			if (carinfoSaleList.Count > 0)
				sb.Append("<li class=\"current\">在售车款</li>");
			
			if (noSaleYearList.Count > 0)
			{
				//sb.Append("<ul id=\"car_nosaleyearlist\">");
				sb.Append("                <li id=\"car_nosaleyearlist\" class=\"last\">停售车款<em></em>");
				sb.Append("                    <dl style=\"display: none;\">");
				for (int i = 0; i < noSaleYearList.Count; i++)
				{
					string url = string.Format("/{0}/{1}/#car_list", serialSpell, noSaleYearList[i].Replace("款", ""));
					if (i == noSaleYearList.Count - 1)
						sb.AppendFormat("<dd class=\"last\"><a href=\"{0}\">{1}</a></dd>", url, noSaleYearList[i]);
					else
						sb.AppendFormat("<dd><a href=\"{0}\">{1}</a></dd>", url, noSaleYearList[i]);
				}
				sb.Append("                    </dl>");
				sb.Append("                </li>");
				//sb.Append("</ul>");
			}
			sb.Append("</ul>");
			// modified by chengl Oct.15.2013
			// sb.Append(" <span class=\"h_text\"><a href=\"http://app.yiche.com/qichehui/\">下载易车客户端，体验信息最全的汽车应用！</a></span> ");
			sb.Append("        </div>");
			sb.Append("    </h3>");
			if (isWaitSale)
			{
				sb.AppendFormat("    <div class=\"car-comparetable\" style=\"display: {0};\" id=\"data_tab_jq5_0\">",
					(carinfoSaleList.Count > 0) ? "none" : "block");
				sb.Append("        <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" id=\"compare_wait\">");
				sb.Append("            <tbody>");
				sb.Append(GetCarListHtml(carinfoWaitSaleList, maxPv));
				sb.Append("            </tbody>");
				sb.Append("        </table>");
				sb.Append("    </div>");
			}
			if (carinfoSaleList.Count > 0)
			{
				sb.AppendFormat("    <div class=\"car-comparetable\" id=\"data_tab_jq5_{0}\" style=\"display: block;\">", isWaitSale ? 1 : 0);
				sb.Append("        <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" id=\"compare_sale\">");
				sb.Append("            <tbody>");
				sb.Append(GetCarListHtml(carinfoSaleList, maxPv));
				sb.Append("            </tbody>");
				sb.Append("        </table>");
				sb.Append("    </div>");
			}
			//sb.Append("    <div class=\"car-comparetable\" style=\"display: none;\" id=\"data_box5_2\">");
			//sb.Append("        <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">");
			//sb.Append("            <tbody>");
			//sb.Append(GetCarListHtml(carinfoNoSaleList, serialInfo, maxPv));
			//sb.Append("            </tbody>");
			//sb.Append("        </table>");
			//sb.Append("    </div>");
			sb.Append("    <div class=\"clear\"></div>");
			sb.Append("    <div class=\"more\">");
			sb.AppendFormat("<a href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">我要预约试驾&gt;&gt;</a>", serialId);
			sb.Append("    </div>");
			//sb.Append("</div>");
			carListTableHtml = sb.ToString();
		}
		/// <summary>
		/// 车型列表html
		/// </summary>
		/// <param name="carList">车款列表 list</param>
		/// <param name="serialInfo">子品牌信息</param>
		/// <param name="maxPv">最大pv</param>
		/// <returns></returns>
		private string GetCarListHtml(List<CarInfoForSerialSummaryEntity> carList, int maxPv)
		{
			List<string> carListHtml = new List<string>();
			//if (carList.Count == 0)
			//    carListHtml.Add("<tr>暂无车款！</tr>");
			var querySale = carList.GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_MaxPower }, p => p);
			int groupIndex = 0;
			foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in querySale)
			{
				var key = CommonFunction.Cast(info.Key, new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_MaxPower = 0 });
				string strMaxPowerAndInhaleType = string.Empty;
				string maxPower = key.Engine_MaxPower == 9999 ? "" : key.Engine_MaxPower + "kW";
				string inhaleType = key.Engine_InhaleType;
				if (!string.IsNullOrEmpty(maxPower) || !string.IsNullOrEmpty(inhaleType))
				{
					strMaxPowerAndInhaleType = string.Format("<b>/</b>{0}{1}", maxPower, " " + inhaleType);
				}

				if (groupIndex == 0)
				{
					carListHtml.Add("<tr class=\"\">");
					carListHtml.Add("    <th width=\"47%\" class=\"first-item\">");
					carListHtml.Add(string.Format("<div class=\"pdL10\"><strong>{0}</strong> {1}</div>",
						key.Engine_Exhaust,
						strMaxPowerAndInhaleType));
					carListHtml.Add("    </th>");
					carListHtml.Add("    <th width=\"8%\" class=\"pd-left-one\">关注度</th>");
					carListHtml.Add("    <th width=\"10%\" class=\"pd-left-one\">变速箱</th>");
					carListHtml.Add("    <th width=\"12%\" class=\"pd-left-two\">指导价</th>");
					carListHtml.Add("    <th width=\"11%\" class=\"pd-left-three\">参考最低价</th>");
					carListHtml.Add("    <th width=\"12%\">&nbsp;</th>");
					carListHtml.Add("</tr>");
				}
				else
				{
					carListHtml.Add("<tr class=\"\">");
					carListHtml.Add("    <th width=\"47%\" class=\"first-item\">");
					carListHtml.Add(string.Format("        <div class=\"pdL10\"><strong>{0}</strong> {1}</div>",
						key.Engine_Exhaust,
						strMaxPowerAndInhaleType));
					carListHtml.Add("    </th>");
					carListHtml.Add("    <th width=\"8%\" class=\"pd-left-one\"></th>");
					carListHtml.Add("    <th width=\"10%\" class=\"pd-left-one\"></th>");
					carListHtml.Add("    <th width=\"12%\" class=\"pd-left-two\"></th>");
					carListHtml.Add("    <th width=\"11%\" class=\"pd-left-three\"></th>");
					carListHtml.Add("    <th width=\"12%\">&nbsp;</th>");
					carListHtml.Add("</tr>");
				}
				groupIndex++;
				List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList<CarInfoForSerialSummaryEntity>();//分组后的集合

				foreach (CarInfoForSerialSummaryEntity entity in carGroupList)
				{
					string yearType = entity.CarYear.Trim();
					if (yearType.Length > 0)
						yearType += "款";
					else
						yearType = "未知年款";
					string stopPrd = "";
					if (entity.ProduceState == "停产")
						stopPrd = "<span class=\"ico-tingchan\">停产</span>";
					Dictionary<int, string> dictCarParams = basicBll.GetCarAllParamByCarID(entity.CarID);
					// 节能补贴 Sep.2.2010
					string hasEnergySubsidy = "";
					//补贴功能临时去掉 modified by chengl Oct.24.2013
					//if (dictCarParams.ContainsKey(853) && dictCarParams[853] == "3000元")
					//{
					//    hasEnergySubsidy = "<a href=\"http://news.bitauto.com/zcxwtt/20130924/1406235633.html\" class=\"ico-butie\" title=\"可获得3000元节能补贴\">补贴</a>";
					//}
					//============2012-04-09 减税============================
					string strTravelTax = "";
					if (dictCarParams.ContainsKey(895))
					{
						strTravelTax = "<a target=\"_blank\" title=\"{0}\" href=\"http://news.bitauto.com/others/20120308/0805618954.html\" class=\"ico-jianshui\">减税</a>";
						if (dictCarParams[895] == "减半")
							strTravelTax = string.Format(strTravelTax, "减征50%车船使用税");
						else if (dictCarParams[895] == "免征")
							strTravelTax = string.Format(strTravelTax, "免征车船使用税");
						else
							strTravelTax = "";
					}
					////易车惠
					//string strYiCheHui = "";
					//var carGoods = serialGoodsCarList.Find(p => p.CarId == entity.CarID);
					//if (carGoods != null)
					//{
					//    var goodsUrl = carGoods.GoodsUrl.Replace("/detail", "/all/detail") + "?WT.mc_id=car2";
					//    strYiCheHui = string.Format("<a target=\"_blank\" title=\"{0}\" href=\"{1}\" class=\"ad-yichehui-list\">易车惠特价&gt;&gt;</a>", "", goodsUrl);
					//}
					//string strBest = "<a href=\"#\" class=\"ico-tuijian\">推荐</a>";
					carListHtml.Add("<tr class=\"\">");
					carListHtml.Add("<td>");
					carListHtml.Add(string.Format("    <div class=\"pdL10\" id=\"carlist_{1}\"><a href=\"/{0}/m{1}/\">{2} {3}</a> {4}</div>",
						serialSpell, entity.CarID, yearType, entity.CarName, strTravelTax + hasEnergySubsidy + stopPrd));
					carListHtml.Add("</td>");
					carListHtml.Add("<td>");
					carListHtml.Add("    <div class=\"w\">");
					//计算百分比
					int percent = (int)Math.Round((double)entity.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero);

					carListHtml.Add(string.Format("        <div class=\"p\" style=\"width: {0}%\"></div>", percent));
					carListHtml.Add("    </div>");
					carListHtml.Add("</td>");
					// 档位个数
					string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" && dictCarParams[724] != "待查") ? dictCarParams[724] + "挡" : "";

					carListHtml.Add(string.Format("<td>{0}</td>", forwardGearNum + entity.TransmissionType));
					carListHtml.Add(string.Format("<td style=\"text-align: right\"><span>{0}</span><a title=\"购车费用计算\" class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid={1}\"></a></td>", string.IsNullOrEmpty(entity.ReferPrice) ? "暂无" : entity.ReferPrice + "万", entity.CarID));
					if (entity.CarPriceRange.Trim().Length == 0)
						carListHtml.Add(string.Format("    <td style=\"text-align: right\"><span>{0}</span></td>", "暂无报价"));
					else
					{
						//取最低报价
						string minPrice = entity.CarPriceRange;
						if (entity.CarPriceRange.IndexOf("-") != -1)
							minPrice = entity.CarPriceRange.Substring(0, entity.CarPriceRange.IndexOf('-'));

						carListHtml.Add(string.Format("<td style=\"text-align: right\"><span><a href=\"/{0}/m{1}/baojia/\">{2}</a></span></td>", serialSpell, entity.CarID, minPrice));
					}
					carListHtml.Add("<td>");
					carListHtml.Add(string.Format("<div class=\"car-summary-btn\"><a href=\"http://dealer.bitauto.com/zuidijia/nb{0}/nc{1}/?T=2\"><s>询价</s></a></div>", serialId, entity.CarID));
					carListHtml.Add(string.Format("<div class=\"car-summary-btn\" id=\"carcompare_btn_{0}\"><a target=\"_self\" href=\"javascript:addCarToCompare('{0}','{1}');\"><s>对比</s></a></div>", entity.CarID, entity.CarName));
					carListHtml.Add("    </td>");
					carListHtml.Add("</tr>");
				}
			}
			return string.Concat(carListHtml.ToArray());
		}
		#endregion

		/// <summary>
		/// 生成子品牌焦点图片处的代码
		/// </summary>
		private void MakeSerialFocus()
		{
			//获取数据
			String[] htmlCode = new String[8];
			Car_SerialBll serialBll = new Car_SerialBll();
			List<SerialFocusImage> imgList = serialBll.GetSerialFocusImageList(serialId);
			string[] bigImageCode = new string[3];
			string[] smallImageCode = new string[3];
			string imageLibUrl = "http://photo.bitauto.com/picture/" + serialId + "/";

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
						bigImageCode[i] = "<div id=\"focusBigImg_" + i + "\" style=\"display: block;\"><a href=\"" + csImg.TargetUrl + "\" target=\"_blank\"><img title=\"" + masterBrandName + serialName + csImg.ImageName + "\" alt=\"" + masterBrandName + serialName + csImg.ImageName + "\" src=\"" + bigImgUrl + "\" ></a> </div>";
						smallImageCode[i] = "<li id=\"focusSmallImg_" + i + "\" class=\"current\"><a rel=\"nofollow\" href=\"" + csImg.TargetUrl + "\" target=\"_blank\"><img title=\"" + masterBrandName + serialName + csImg.ImageName + "\" alt=\"" + masterBrandName + serialName + csImg.ImageName + "\" src=\"" + smallImgUrl + "\"></a></li>";
					}
					else
					{
						bigImageCode[i] = "<div id=\"focusBigImg_" + i + "\" style=\"display: none;\"><a href=\"" + csImg.TargetUrl + "\" target=\"_blank\"><img title=\"" + masterBrandName + serialName + csImg.ImageName + "\" alt=\"" + masterBrandName + serialName + csImg.ImageName + "\" src=\"" + bigImgUrl + "\" ></a> </div>";
						smallImageCode[i] = "<li id=\"focusSmallImg_" + i + "\"><a rel=\"nofollow\" href=\"" + csImg.TargetUrl + "\" target=\"_blank\"><img title=\"" + masterBrandName + serialName + csImg.ImageName + "\" alt=\"" + masterBrandName + serialName + csImg.ImageName + "\" src=\"" + smallImgUrl + "\"></a></li>";
					}
				}
			}
			else
			{
				bigImageCode[0] += "<div id=\"focusBigImg_0\" style=\"display: block;\"><img src=\"" + WebConfig.DefaultCarPic + "\" ></div>";
				smallImageCode[0] += "<li id=\"focusSmallImg_0\" class=\"current\"><img src=\"" + WebConfig.DefaultCarPic + "\"></li>";
			}
			htmlCode[0] = "<div class=\"focus_pics\" >";
			htmlCode[1] = "<!-- baidu-tc begin {\"action\":\"DELETE\"} --><div class=\"lantern_pic\" id=\"lantern_pic\">";
			//三张大图
			htmlCode[2] = String.Concat(bigImageCode);
			htmlCode[3] = "</div><!-- baidu-tc end -->";
			//三张小图
			htmlCode[4] = "<ul id=\"lantern_list\" class=\"lantern_list\">";
			htmlCode[5] = String.Concat(smallImageCode);
			htmlCode[6] = "</ul>";
			htmlCode[7] = "</div>";
			CsPicJiaodian = String.Concat(htmlCode);
			//CsPicJiaodian = new Car_SerialBll().MakeSerialFocusImageNew(serialId, masterBrandName + serialName);
		}

		/// <summary>
		/// 生成子品牌概况Html
		/// </summary>
		private void MakeSerialOverview()
		{
			// modified by chengl Apr.26.2011
			List<string> htmlList = new List<string>();
			htmlList.Add("<div style=\"z-index:2\" class=\"line_box line_box_noneBg zs02\">");
			//if (sic.OfficialSite.Length > 0)
			//{
			//    htmlCode.AppendLine("<div class=\"more\"><a href=\"" + sic.OfficialSite + "\">官方网站&gt;&gt;</a></div>");
			//}
			if (sic.CsSaleState == "停销")
			{
				htmlList.Add("<div class=\"car_sub_tt\"><span>停售</span></div>");
			}
			else
			{
				htmlList.Add("<div class=\"car_sub_tt\">参考成交价：<span><a rel=\"nofollow\" href=\"/" + serialSpell + "/baojia/\" target=\"_blank\">" + serialPrice + "</a></span></div>");
			}
			htmlList.Add("<ul class=\"d\">");
			#region 颜色
			string rgbHTML = "";
			string rgbTitle = "";
			List<string> listColorName = new List<string>();
			List<string> listColorRGB = new List<string>();
			new Car_SerialBll().GetSerialColorRGBByCsID(sic.CsID, 0, 1, sic.ColorList
				, out rgbHTML, out rgbTitle, out listColorName, out listColorRGB);
			htmlList.Add("<!-- baidu-tc begin {\"action\":\"DELETE\"} --><li><label>颜色：</label><span id=\"colorSelect\" class=\"c\" >");
			htmlList.Add(rgbHTML);
			htmlList.Add("</span></li><!-- baidu-tc end -->");
			#endregion
			//彩虹条 三包链接
			string sanbaoLink = string.Empty;
			if (!string.IsNullOrEmpty(sic.CsSanBaoLink))
				sanbaoLink = string.Format("<a href=\"{0}\" target=\"_blank\"><em>(三包)</em></a>&nbsp;&nbsp;", sic.CsSanBaoLink);

			if (StringHelper.GetRealLength(sic.SerialRepairPolicy) > 30)
				htmlList.Add("<li title=\"" + StringHelper.RemoveHtmlTag(sic.SerialRepairPolicy) + "\"><label>养护：</label>" + StringHelper.SubString(sic.SerialRepairPolicy, 30, true) + "&nbsp;&nbsp;" + sanbaoLink + "<a href=\"/" + serialSpell + "/baoyang/\" target=\"_blank\">详情&gt;&gt;</a></li>");
			else
				htmlList.Add("<li><label>养护：</label>" + sic.SerialRepairPolicy + "&nbsp;&nbsp;" + sanbaoLink + "<a rel=\"nofollow\" href=\"/" + serialSpell + "/baoyang/\" target=\"_blank\">详情&gt;&gt;</a></li>");
			htmlList.Add("<li><label>油耗：</label>" + sic.CsSummaryFuelCost + " <span>(综合)</span>&nbsp;&nbsp;" + sic.CsGuestFuelCost + " <span>(网友)</span>&nbsp;&nbsp;<a rel=\"nofollow\" href=\"/" + serialSpell + "/youhao/\" target=\"_blank\">详情&gt;&gt;</a></li>");
			htmlList.Add("<li class=\"ofh\"><label>厂商：</label><a rel=\"nofollow\" class=\"cBlack\" href=\"/" + cse.Brand.AllSpell + "/\" target=\"_blank\">" + cse.Brand.Producer.ShortName + "</a>");
			if (cse.OfficialSite.Length > 0)
			// htmlList.Add(" <a href=\"" + cse.OfficialSite + "\" target=\"_blank\">进入产品官网&gt;&gt;</a></li>");
			{ htmlList.Add(" <a rel=\"nofollow\" href=\"" + cse.OfficialSite + "\" target=\"_blank\">进入产品官网&gt;&gt;</a>"); }
			else
			{ }
			// htmlList.Add("</li>");
			//车型频道非标准广告(特定子品牌车型贷款链接) 高总 2013.02.27 
			//string buycarLink = "http://market.bitauto.com/gmac2010/zxsq.aspx";
			//List<int> listSerialIdsForBuyCar = new List<int> { 1679, 2583, 1698, 2409, 2190, 1699, 1695, 3316, 3753, 2567, 1769, 2022, 2196, 3037, 1922, 2678, 2856, 2070 };
			//int serialid = listSerialIdsForBuyCar.Find(id => id == serialId);
			//if (serialid > 0)
			//{ buycarLink = "http://market.bitauto.com/nissan/index.aspx"; }

			//// 车型频道非标准广告(特定子品牌车型贷款链接) 高总 2013.03.26 
			//listSerialIdsForBuyCar.Clear();
			//listSerialIdsForBuyCar = new List<int> { 2611, 3273, 1811, 2674, 1909, 2381, 1765, 2714, 2875, 2593, 2075, 2573, 1798, 2256, 3221, 1802, 1796, 2776, 2370, 2871, 2413, 2710, 2753 };
			//serialid = 0;
			//serialid = listSerialIdsForBuyCar.Find(id => id == serialId);
			//if (serialid > 0)
			//{ buycarLink = "https://ccclub.cmbchina.com/fincreditweb/Apply/ApplyDetail.aspx?WT.mc_id=C31YCCX051303273"; }

			// modified by chengl Nov.30.2011
			// htmlList.Add(" <a target=\"_blank\"  class=\"cRed\" href=\"" + buycarLink + "\">贷款买车>></a></li>");
			htmlList.Add(" <ins id=\"div_258b7c07-4509-4088-adb1-09402e43cbde\" type=\"ad_play\" adplay_IP=\"\" adplay_AreaName=\"\"  adplay_CityName=\"\"    adplay_BrandID=\""
				+ serialId.ToString() + "\"  adplay_BrandName=\"\"  adplay_BrandType=\"\"  adplay_BlockCode=\"258b7c07-4509-4088-adb1-09402e43cbde\"> </ins></li>");
			htmlList.Add("</ul>");
			/*
			htmlCode.AppendLine("<div class=\"h\">");
			if (sic.CsPicCount == 0)
			{ htmlCode.Append("<a class=\"nolink\">图片<span>(0张)</span></a>"); }
			else
			{ htmlCode.Append("<a href=\"/" + serialSpell + "/tupian/\" >图片<span>(" + sic.CsPicCount + "张)</span></a>"); }
			if (sic.CsDianPingCount == 0)
			{ htmlCode.Append("|<a class=\"nolink\">口碑<span>(0条)</span></a>"); }
			else
			{ htmlCode.Append("|<a href=\"/" + serialSpell + "/koubei/\" >口碑<span>(" + sic.CsDianPingCount + "条)</span></a>"); }
			if (sic.CsAskCount == 0)
			{ htmlCode.Append("|<a>答疑<span>(0条)</span></a>"); }
			else
			{ htmlCode.Append("|<a href=\"http://ask.bitauto.com/" + serialId + "/\" >答疑<span>(" + sic.CsAskCount + "条)</span></a>"); }
			*/

			htmlList.Add("<div class=\"fun\">");
			//// modified by chengl Dec.13.2011
			//// htmlList.Add("<a href=\"http://ask.bitauto.com/browse/" + serialId + "/\" target=\"_blank\">买前咨询</a>");
			//// modified by chengl Dec.15.2011
			//// htmlList.Add("<a href=\"/" + cse.AllSpell + "/baojia/\" target=\"_blank\">预约试驾</a>");
			//// 如果有最热 有报价车型
			//20130412 edit anh
			//if (hotCarID > 0)
			//{ htmlList.Add(string.Format("<a href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">预约试驾</a>", serialId)); }
			//else
			//{ 
			htmlList.Add(string.Format("<a href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">预约试驾</a>", serialId));
			//}

			//是否有置换服务
			// modified by chengl Jul.26.2012 高总需求所有子品牌开放置换
			htmlList.Add("<a rel=\"nofollow\" href=\"/" + serialSpell + "/zhihuan/\" target=\"_blank\">置换</a>");
			//if (new Car_SerialBll().IsZhiHuanService(serialId))
			//{
			//    htmlList.Add("<a href=\"/" + serialSpell + "/zhihuan/\" target=\"_blank\">置换</a>");
			//}
			//// add by chengl Jul.20.2012
			//else
			//{
			//    htmlList.Add("<span class=\"nolink\">置换</span>");
			//}

			// htmlList.Add("<a href=\"http://i.bitauto.com/baaadmin/car/goumai_" + serialId + "/\" target=\"_blank\">计划购买</a>");
			htmlList.Add("<a rel=\"nofollow\" id=\"location_ucar\" name=\"location_ucar\" href=\"http://www.taoche.com/buycar/serial/" + serialSpell + "/\" target=\"_blank\">买二手车</a>");

			// modified by chengl May.25.2012
			htmlList.Add("<a rel=\"nofollow\" id=\"LinkForBaaAttention\" href=\"http://i.bitauto.com/baaadmin/car/guanzhu_" + serialId + "/\" target=\"_blank\">加关注</a>");

			htmlList.Add("</div>");
			//20130412 edit anh
			htmlList.Add(string.Format("<div class=\"more\"><a href=\"http://dealer.bitauto.com/zuidijia/nb{0}/?T=1\" target=\"_blank\">询价&gt;&gt;</a></div>", serialId));
			htmlList.Add("</div>");
			CsDetailInfo = String.Concat(htmlList.ToArray());

		}

		/// <summary>
		/// 获取车型详解信息
		/// </summary>
		private void MakeMustSee()
		{
			string pingceHtmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\pingce\\Html\\Serial_All_News_" + serialId + ".html");
			if (File.Exists(pingceHtmlFile))
			{
				CsMustSeeInfo = File.ReadAllText(pingceHtmlFile);
			}
			/* 因修改为此处由服务生成，所以注掉了
			// modified by chengl Apr.26.2011

			//得到要显示的新闻ID
			DataSet pcDs = new Car_SerialBll().GetNewsListBySerialId(serialId, "pingce");
			int newsId = 0;
			if (pcDs != null && pcDs.Tables.Contains("listNews")
				&& pcDs.Tables["listNews"] != null && pcDs.Tables["listNews"].Rows.Count > 0)
			{
				newsId = ConvertHelper.GetInteger(pcDs.Tables["listNews"].Rows[0]["newsid"]);
			}

			string[] tagList = { "导语", "外观", "内饰", "空间", "视野", "灯光", "动力", "操控", "舒适性", "油耗", "配置与安全", "总结" };
			List<string> titleList = new List<string>();
			if (newsId > 0)
			{
				DataSet pccDs = base.GetPingCeNewByNewID(newsId);
				if (pccDs != null && pccDs.Tables.Count > 0 && pccDs.Tables[0].Rows.Count > 0 && pccDs.Tables[0].Columns.Contains("content"))
				{
					DataRow row = pccDs.Tables[0].Rows[0];
					string newsContent = row["content"].ToString();
					string RegexString = "<div(?:[^<]*)?id=\"bt_pagebreak\"[^>]*>([^<]*)</div>";
					Regex r = new Regex(RegexString);
					string[] newsGroup = r.Split(newsContent);
					Regex rex = new Regex(@"\$\$(?<title>.+)\$\$");
					foreach (string pageStr in newsGroup)
					{
						Match m = rex.Match(pageStr);
						if (m.Success)
						{
							string pageTitle = m.Result("${title}");
							if (pageTitle.Length == 0)
								continue;
							else
								titleList.Add(pageTitle);
						}
					}
				}
			}
			string baseUrl = "/" + cse.Cs_AllSpell.ToLower().Trim() + "/";
			StringBuilder htmlCode = new StringBuilder(1400);
			//// htmlCode.Append("<dl class=\"zs02\">");
			htmlCode.Append("<div style=\"z-index:1\" class=\"line_box line_box_noneBg\">");

			string linkStr = "";
			if (sic.CsNewMaiCheCheShi.Trim().Length > 0)
			{
				linkStr = "<a href=\"" + sic.CsNewMaiCheCheShi + "\" >买车测试</a>";
			}
			string rptUrl = new Car_SerialBll().GetSerialKoubeiReport(serialId);
			if (rptUrl.Length > 0)
			{
				if (linkStr.Length > 0)
					linkStr += " | ";
				linkStr += "<a href=\"" + string.Format(rptUrl, cse.Cs_AllSpell.ToLower()) + "\" >口碑报告</a>";
			}
			if (linkStr.Length > 0)
			{
				htmlCode.AppendLine("<div class=\"more\">" + linkStr + "</div>");
			}

			if (newsId > 0)
			{ htmlCode.Append("<div class=\"car_sub_tt\"><a href=\"" + baseUrl + "pingce/\" >车型详解</a></div>"); }
			else
			{ htmlCode.Append("<div class=\"car_sub_tt\">车型详解</div>"); }

			////htmlCode.Append("<dd class=\"sublink\">");
			////string linkStr = "";

			////if (sic.CsNewMaiCheCheShi.Trim().Length > 0)
			////{
			////    linkStr = "<a href=\"" + sic.CsNewMaiCheCheShi + "\" >买车测试</a>";
			////}
			////string rptUrl = new Car_SerialBll().GetSerialKoubeiReport(serialId);
			////if (rptUrl.Length > 0)
			////{
			////    if (linkStr.Length > 0)
			////        linkStr += " | ";
			////    linkStr += "<a href=\"" + string.Format(rptUrl, cse.Cs_AllSpell.ToLower()) + "\" >口碑报告</a>";
			////}
			////linkStr += "</dd>";
			////htmlCode.Append(linkStr);

			//// htmlCode.Append("<dd>");
			htmlCode.Append("<ul class=\"carDetail\">");
			foreach (string tagName in tagList)
			{
				if (tagName == "导语")
				{
					if (newsId > 0)
						htmlCode.Append("<li><a href=\"" + baseUrl + "pingce/1/\">导语</a></li>");
					else
						htmlCode.Append("<li class=\"noDetail\">导语</li>");
					continue;
				}

				//先匹配关键词
				int pageNum = 0;
				for (int i = 0; i < titleList.Count; i++)
				{
					string tmpTitle = titleList[i];
					if (tmpTitle.IndexOf(tagName) > -1)
					{
						pageNum = i + 2;
						break;
					}
				}

				if (pageNum > 0)
					htmlCode.Append("<li><a href=\"" + baseUrl + "pingce/" + pageNum + "/\">" + tagName + "</a></li>");
				else
					htmlCode.Append("<li class=\"noDetail\">" + tagName + "</li>");
			}
			htmlCode.Append("</ul>");
			htmlCode.Append("<div class=\"clear\"></div>");

			htmlCode.Append("<div class=\"carTxt\">");
			if (sic.CsNewShangShi.Trim().Length == 0)
				htmlCode.Append("<span class=\"noContent\">上市</span>");
			else
				htmlCode.Append("<span><a href=\"" + sic.CsNewShangShi + "\">上市</a></span>");
			if (sic.CsNewGouCheShouChe.Trim().Length == 0)
				htmlCode.Append("<span class=\"noContent\">购车手册</span>");
			else
				htmlCode.Append("<span><a href=\"" + sic.CsNewGouCheShouChe + "\">购车手册</a></span>");
			if (new ProduceAndSellDataBll().HasSerialData(serialId))
				htmlCode.Append("<span><a href=\"" + String.Format(sic.CsNewXiaoShouShuJu.Trim(), serialId) + "\">销量</a></span>");
			else
				htmlCode.Append("<span class=\"noContent\">销量</a></span>");
			if (sic.CsNewKeJi.Trim().Length == 0)
				htmlCode.Append("<span class=\"noContent\">技术</span>");
			else
				htmlCode.Append("<span><a href=\"" + sic.CsNewKeJi.Trim() + "\">技术</a></span>");
			if (sic.CsNewAnQuan.Trim().Length == 0)
				htmlCode.Append("<span class=\"noContent\">安全</span>");
			else
				htmlCode.Append("<span><a href=\"" + sic.CsNewAnQuan.Trim() + "\">安全</a></span>");
			if (new Car_SerialBll().IsExitsMaintanceMessage(sic.CsID))
				htmlCode.Append("<span><a href=\"/" + sic.CsAllSpell.ToLower() + "/baoyang/\">维修养护</a></span>");
			else
				htmlCode.Append("<span class=\"noContent\">维修养护</span>");
			htmlCode.Append("</div>");
			htmlCode.Append("</div>");
			////htmlCode.Append("</dl>");
			CsMustSeeInfo = htmlCode.ToString();
			 * */
		}

		private void MakeTopNews()
		{
			StringBuilder htmlCode = new StringBuilder();
			//获取数据
			// XmlDocument xmlDoc = new Car_SerialBll().GetSerialFocusNews(serialId);
			//焦点新闻
			#region 弃用
			// 		XmlNodeList focusList = xmlDoc.SelectNodes("/root/FocusNews/listNews");
			//         if (focusList.Count > 0)
			//         {
			//             //获取第一条导购或行情新闻
			//             int[] cateIdList = new int[] { 3, 4, 179, 102, 115, 120, 29, 30 };
			//             string xmlPath = CommonFunction.GetCategoryXmlPath(cateIdList);
			//             XmlNode firstNews = xmlDoc.SelectSingleNode("/root/FocusNews/listNews[" + xmlPath + "]");
			//             if (firstNews == null)
			//                 firstNews = focusList[0];
			//             int firstNewsId = Convert.ToInt32(firstNews.SelectSingleNode("newsid").InnerText);
			//             string newsTitle = firstNews.SelectSingleNode("title").InnerText;
			//             string filePath = firstNews.SelectSingleNode("filepath").InnerText;
			//             //过滤Html标签
			//             newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
			//             //string shortNewsTitle = StringHelper.SubString(newsTitle, 36, true);
			// 
			//             // 				if(shortNewsTitle != newsTitle)
			//             // 					htmlCode.Append("<h2><a href=\"" + filePath + "\" title=\"" + newsTitle + "\" target=\"_blank\">" + shortNewsTitle + "</a></h2>");
			//             // 				else
			//             htmlCode.Append("<h2><a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a></h2>");
			//             htmlCode.Append("<ul class=\"list_date\" id=\"topa1\">");
			//             int newsCounter = 0;
			//             foreach (XmlElement newsNode in focusList)
			//             {
			//                 int newsId = Convert.ToInt32(newsNode.SelectSingleNode("newsid").InnerText);
			//                 if (newsId == firstNewsId)
			//                     continue;
			// 
			//                 newsCounter++;
			// 
			//                 newsTitle = newsNode.SelectSingleNode("title").InnerText;
			//                 //过滤Html标签
			//                 newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
			//                 string shortNewsTitle = StringHelper.SubString(newsTitle, 40, true);
			//                 if (shortNewsTitle.StartsWith(newsTitle))
			//                     shortNewsTitle = newsTitle;
			//                 filePath = newsNode.SelectSingleNode("filepath").InnerText;
			//                 //string newsCategory = newsNode.SelectSingleNode("categoryname").InnerText;
			//                 string catePath = newsNode.SelectSingleNode("CategoryPath").InnerText;
			//                 string newsCategory = Car_SerialBll.GetNewsKind(catePath);
			//                 string cateUrl = baseUrl + newsCategory + "/";
			//                 switch (newsCategory)
			//                 {
			//                     case "xinwen":
			//                         newsCategory = "新闻";
			//                         break;
			//                     case "hangqing":
			//                         newsCategory = "行情";
			//                         break;
			//                     case "daogou":
			//                         newsCategory = "导购";
			//                         break;
			//                     case "shipin":
			//                         newsCategory = "视频";
			//                         break;
			//                     case "yongche":
			//                         newsCategory = "用车";
			//                         break;
			//                     case "shijia":
			//                         newsCategory = "试驾";
			//                         break;
			//                     default:
			//                         cateUrl = "#";
			//                         newsCategory = "其它";
			//                         break;
			// 
			//                 }
			//                 string newsDate = Convert.ToDateTime(newsNode.SelectSingleNode("publishtime").InnerText).ToString("MM-dd");
			// 
			//                 htmlCode.Append("<li><a href=\"" + cateUrl + "\" class=\"fl\" target=\"_blank\">[" + newsCategory + "]</a>");
			//                 htmlCode.Append("<a href=\"" + filePath + "\" title=\"" + newsTitle + "\" target=\"_blank\">" + shortNewsTitle + "</a><small>" + newsDate + "</small></li>");
			//             }
			//             htmlCode.Append("</ul>");
			//         }
			//         focusNewsHtml = htmlCode.ToString();
			/*
			Dictionary<string, List<News>> newsObjectList = new Dictionary<string, List<News>>();
			newsObjectList = new Car_SerialBll().GetSerialSummaryPageNewsSpan(serialId);

			focusNewsHtml = MakeFoucsNewsHtml(newsObjectList);//MakeFoucsNewsHtml(xmlDoc);
			xinwenHtml = MakeTypeNews("xinwen", out xinwenFirstHtml);
			hangqingHtml = MakeTypeNews("hangqing", out hangqingFirstHtml);
			shijiaHtml = MakeTypeNews("shijia", out shijiaFirstHtml);

			focusALinkStr = "";
			xinwenALinkStr = " href=\"" + baseUrl + "xinwen/\" ";
			hangqingALinkStr = " href=\"" + baseUrl + "hangqing/\" ";
			shijiaALinkStr = " href=\"" + baseUrl + "shijia/\" ";
			if (focusNewsHtml.Length == 0)
				focusALinkStr = " class=\"nolink\" style=\"display: none;\" ";
			if (xinwenHtml.Length == 0)
				xinwenALinkStr = " class=\"nolink\" style=\"display: none;\" ";
			if (hangqingHtml.Length == 0)
				hangqingALinkStr = " class=\"nolink\" style=\"display: none;\" ";
			if (shijiaHtml.Length == 0)
				shijiaALinkStr = " class=\"nolink\" style=\"display: none;\" ";

			daogouHtml = BuyCarMustWatch(newsObjectList);*/

			#endregion
			//modified by sk 2013-09-18 废除旧焦点新闻 读取方式
			//string focusPage = Path.Combine(WebConfig.DataBlockPath, string.Format(_FocusNewsHtmlPage, serialId));
			//if (File.Exists(focusPage))
			//    focusNewsHtml = File.ReadAllText(focusPage);
			int focusNews = (int)CommonHtmlEnum.BlockIdEnum.FocusNewsOld;
			if (dictSerialBlockHtml.ContainsKey(focusNews))
				focusNewsHtml = dictSerialBlockHtml[focusNews];

			string pingjiaPage = Path.Combine(WebConfig.DataBlockPath, string.Format(_PingJiaHtmlPage, serialId));
			if (File.Exists(pingjiaPage))
				daogouHtml = File.ReadAllText(pingjiaPage);
			//导购新闻
			//XmlNodeList newsList = xmlDoc.SelectNodes("/root/Introduce/listNews");
			//daogouHtml = MakeOtherNews(newsList, "introduce");
			//论坛话题
			// modified by chengl Oct.1.2013 论坛xml文件迁移至 Data\SerialNews\Forum\
			XmlDocument xmlDoc = new Car_SerialBll().GetSerialForumNews(serialId);
			XmlNodeList newsList = xmlDoc.SelectNodes("/root/Forum/ForumSubject");
			forumHtml = MakeOtherNews(newsList, "forum");
		}


		private XmlElement GetFirstNews(XmlDocument xmlDoc)
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

		private void GetNewsNode(XmlDocument xmlDoc, int[] cateIdList, ref XmlElement newsNode, ref XmlElement newsNode2)
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

		private string MakeFoucsNewsHtml(XmlDocument xmlDoc)
		{
			StringBuilder htmlCode = new StringBuilder();
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
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle).Replace("·", "&bull;");
					if (i == 0)
					{
						htmlCode.Append("<h2><a href=\"" + filePath + "\" >" + newsTitle + "</a></h2>");
						htmlCode.Append("<ul class=\"list_date\" id=\"topa1\">");
					}
					else
					{
						newsTitle = Server.HtmlDecode(newsTitle);
						string shortNewsTitle = StringHelper.SubString(newsTitle, 40, true);

						if (shortNewsTitle.StartsWith(newsTitle))
							shortNewsTitle = newsTitle;
						XmlNode cateNode = newsNode.SelectSingleNode("CategoryPath");
						string catePath = "";
						if (cateNode != null)
							catePath = cateNode.InnerText;
						string newsCategory = Car_SerialBll.GetNewsKind(catePath);
						string cateUrl = baseUrl + newsCategory + "/";
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
		/// <summary>
		/// 得到焦点图新闻代码
		/// </summary>
		/// <param name="newsList"></param>
		/// <returns></returns>
		private string MakeFoucsNewsHtml(Dictionary<string, List<News>> newsList)
		{
			if (newsList == null || !newsList.ContainsKey("focus") || newsList["focus"].Count < 1) return "";

			int index = 0;
			StringBuilder htmlCode = new StringBuilder();
			foreach (News entity in newsList["focus"])
			{
				if (index == 0)
				{
					htmlCode.Append("<h2><a href=\"" + entity.PageUrl + "\" >" + entity.Title + "</a></h2>");
					htmlCode.Append("<ul class=\"list_date\" id=\"topa1\">");
					index++;
					continue;
				}
				string cateUrl = baseUrl + entity.CategoryName + "/";
				string newsCategory = "";
				switch (entity.CategoryName)
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

				htmlCode.Append("<li><a href=\"" + cateUrl + "\" class=\"fl\" >[" + newsCategory + "]</a>");
				htmlCode.Append("<a href=\"" + entity.PageUrl + "\" title=\"" + entity.Title + "\">" + entity.Title + "</a><small>" + entity.PublishTime.ToString("MM-dd") + "</small></li>");
			}
			htmlCode.Append("</ul>");
			return htmlCode.ToString();
		}

		private string MakeTypeNews(string newsType, out string firstHTML)
		{
			firstHTML = "";
			StringBuilder htmlCode = new StringBuilder();
			DataSet newsDs = new Car_SerialBll().GetNewsListBySerialId(serialId, newsType);
			if (newsDs != null && newsDs.Tables.Count > 0 && newsDs.Tables[0].Rows.Count > 0 && newsDs.Tables.Contains("listNews"))
			{
				int newsCounter = 0;
				foreach (DataRow row in newsDs.Tables["listNews"].Rows)
				{
					// modified by chengl May.19.2010
					// 大于11行 最后行改成更多
					if (newsCounter == 0)
					{
						// modified by chengl Apr.28.2011
						firstHTML = string.Format("<h2><a href=\"{0}\">{1}</a></h2>", row["filepath"], row["title"]);
						// htmlCode.AppendFormat("<h2><a href=\"{0}\">{1}</a></h2>", row["filepath"], row["title"]);
						newsCounter++;
						continue;
					}

					if (newsCounter == 11)//&& newsType != "shijia")
					{
						htmlCode.Append("<li class=\"topnewsmore\" ><a href=\"/" + serialSpell + "/" + newsType + "/\">查看更多>></a></li>");
						break;
					}
					string cityName = "[全国]";
					if (row["relatedcityname"].ToString().Trim() != "" && row["relatedcityname"].ToString().Trim().IndexOf(",") == -1)
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
					string cityUrl = base.GetCityURLByCityName(cityName.Replace("[", "").Replace("]", ""));
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
			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成导购新闻推荐
		/// </summary>
		/// <param name="htmlCode"></param>
		/// <param name="newsList"></param>
		private string MakeOtherNews(XmlNodeList newsList, string type)
		{
			List<string> htmlList = new List<string>();
			type = type.ToLower();
			string codeTitle = serialSeoName;
			string moreUrl = "";
			string bbsLink = "";
			if (type == "introduce")
			{
				codeTitle += "-导购推荐";
				moreUrl = baseUrl + "daogou/";
			}
			else if (type == "forum")
			{
				moreUrl = baaUrl;
				codeTitle += "-论坛话题";
				DataSet ds = base.GetBBSLinkBySerialId(serialId);
				if (ds != null && ds.Tables[0] != null)
				{
					StringBuilder sb = new StringBuilder();
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						sb.AppendFormat("<a rel=\"nofollow\" href=\"{0}\" target=\"_blank\">{1}</a> | ", dr["url"], dr["title"]);
					}
					bbsLink = sb.ToString();
				}
			}

			htmlList.Add("<h3><span><a href=\"" + moreUrl + "\">" + codeTitle + "</a></span></h3>");
			if (newsList.Count > 0)
				htmlList.Add("<div class=\"more\">" + bbsLink + "<a rel=\"nofollow\" href=\"" + moreUrl + "\">更多&gt;&gt; </a></div>");
			if (type == "forum")
			{
				htmlList.Add("<div class=\"mainlist_box reco reco4\" style=\"height:100px\">");
			}
			else
			{
				htmlList.Add("<div class=\"mainlist_box reco\">");
			}
			int loop = 1;
			if (newsList.Count == 0)
			{
				//子品牌综述页论坛补充贴(当1条论坛数据都没有)
				string includeFilePath = Server.MapPath("~/include/BAA/BBS/00001/201311_loulan_ltht_Manual.shtml");
				if (File.Exists(includeFilePath))
				{
					htmlList.Add("<ul class=\"list_date\">");
					htmlList.Add(File.ReadAllText(includeFilePath));
					htmlList.Add("</ul>");
				}
				else
				{
					htmlList.Add("<div class=\"car_nonedata\">暂无精彩话题</div>");
				}
			}
			else
			{
				htmlList.Add("<ul class=\"list_date\">");
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
						// modified by chengl Jun.15.2012
						if (newsNode.SelectSingleNode("postdatetime") != null)
						{
							pubTime = newsNode.SelectSingleNode("postdatetime").InnerText;
							pubTime = Convert.ToDateTime(pubTime).ToString("MM-dd");
						}
					}
					if (shortNewsTitle != newsTitle)
						htmlList.Add("<li><a href=\"" + filePath + "\" title=\"" + newsTitle + "\" >" + shortNewsTitle + "</a><small>" + pubTime + "</small></li>");
					else
						htmlList.Add("<li><a href=\"" + filePath + "\" >" + newsTitle + "</a><small>" + pubTime + "</small></li>");

					// modified by chengl Jul.22.2010
					loop++;
					if (type == "forum")
					{
						if (loop > 4)
							break;
					}
					else
					{
						if (loop > 5)
							break;
					}
				}
				htmlList.Add("</ul>");
			}
			htmlList.Add("<div class=\"clear\"></div>");
			htmlList.Add("</div>");
			return String.Concat(htmlList.ToArray());
		}
		/// <summary>
		/// 买车必看
		/// </summary>
		/// <returns></returns>
		private string BuyCarMustWatch(Dictionary<string, List<News>> newsList)
		{
			List<string> htmlList = new List<string>();
			string codeTitle = serialSeoName + "-买车必看";
			//string moreUrl = "";

			htmlList.Add("<h3><span>" + codeTitle + "</span></h3>");
			if (newsList.ContainsKey("must") && newsList["must"].Count > 0)
			{
				//htmlCode.Append("<div class=\"more\"><a href=\"" + moreUrl + "\">更多&gt;&gt; </a></div>");
				htmlList.Add("<div class=\"mainlist_box reco\">");
				htmlList.Add("<ul class=\"list_date\">");

				for (int i = 0; i < 5 && i < newsList["must"].Count; i++)
				{
					News entity = newsList["must"][i];
					htmlList.Add("<li><a href=\"" + entity.PageUrl + "\" title=\"" + entity.Title + "\" >" + entity.Title + "</a><small>" + entity.PublishTime.ToString("MM-dd") + "</small></li>");
				}
				htmlList.Add("</ul>");
				htmlList.Add("<div class=\"clear\"></div>");
				htmlList.Add("</div>");
			}
			return String.Concat(htmlList.ToArray());
		}

		/// <summary>
		/// 生成彩虹条
		/// </summary>
		private void MakeRainbowHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			//获取数据
			string rainbowStr = new RainbowListBll().GetRainbowListXML_CSID(serialId);
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
						url = baseUrl + "/koubei/";
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
				htmlCode.Append("<h3><span>" + masterBrandName + serialName + "追踪报道</span></h3>");
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
			rainbowHtml = htmlCode.ToString();
		}
		/// <summary>
		/// 生成图释部分
		/// </summary>
		private void MakeSerialImageCarHTML()
		{
			//SerialSet\\SerialColorImage\\SerialColorImage_{0}.html
			string path = Path.Combine(WebConfig.DataBlockPath, string.Format(_SerialImageCarPath, serialId.ToString()));
			if (File.Exists(path))
			{
				FileStream stream = null;
				StreamReader reader = null;
				try
				{
					stream = new FileStream(path, FileMode.Open, FileAccess.Read);
					reader = new StreamReader(stream, Encoding.UTF8);
					serialImageCarHtml = reader.ReadToEnd();
				}
				catch { }
				finally
				{
					if (reader != null)
						reader.Dispose();
					if (stream != null)
						stream.Dispose();
				}
			}
		}
		/// <summary>
		/// 生成图片部分
		/// </summary>
		private void MakeSerialImages()
		{
			////modify by sk 2013.09.25 车款列表新规则修改，（朗逸：2370 科鲁兹：2608 起亚K2：3398 长城C30: 3023 凯越：2388 ）
			//int[] specialSerialIdArray = { 2370, 2608, 3398, 3023, 2388, 1765, 2871, 1793, 1905, 1611 };
			//if (specialSerialIdArray.Contains(cse.Id)) return;

			List<string> htmlList = new List<string>();
			//获取数据
			// 焦点图&中部图库组图(外观，内饰，空间，图解)
			//图库接口本地化更改 by sk 2012.12.21
			string xmlPicPath = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialPhotoListPath, serialId));
			//string xmlPicPath = string.Format(WebConfig.PhotoService, serialId.ToString());
			// 此 Cache 将通用于图片页和车型综述页
			DataSet dsCsPic = this.GetXMLDocToDataSetByURLForCache("CarChannel_SerialAllPic_" + serialId.ToString(), xmlPicPath, 60);
			if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("A"))
			{
				// 外观 6、图解 12、官方图 11、到店实拍 0 更多link
				//string moreCateLink = "";
				////总数
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
						if (cateId != 6 && cateId != 7 && cateId != 8 && cateId != 12 && cateId != 11)
							continue;
						categoryPicNum[cateId] = cateNum;
					}
				}
				string allPicUrl = baseUrl + "tupian/";

				// modified by chengl Oct.19.2011
				// 新版图片显示测掉 换回老版本
				bool needMakePeceImage = false;
				// bool needMakePeceImage = NeedToMakePieceHtml(serialId, dsCsPic);
				// modified by chengl Sep.28.2011 图片点击统计
				if (needMakePeceImage)
				{
					htmlList.Add("<div id=\"DivPicBlockForStat\" class=\"line_box car_pics\">");
					htmlList.Add("<h3><span><a id=\"101\" href=\"" + allPicUrl + "\">" + serialSeoName + "-图片 </a></span>");
					// htmlList.Add("<label class=\"h3sublink\"><a id=\"102\" href=\"" + allPicUrl + "\">" + picNum + "张</a></label></h3>");
					htmlList.Add("</h3>");
				}
				else
				{
					// htmlList.Add("<div id=\"DivPicBlockForStat\" class=\"line_box zs100412_3\">");
					htmlList.Add("<div id=\"DivPicBlockForStat\" class=\"line_box zs100412_3\">");
					htmlList.Add("<h3><span><a id=\"201\" href=\"" + allPicUrl + "\">" + serialSeoName + "-图片 </a></span>");
					htmlList.Add("<label class=\"h3sublink\"><a id=\"202\" href=\"" + allPicUrl + "\">" + picNum + "张</a></label></h3>");
				}
				//htmlList.Add("<h3><span><a href=\"" + allPicUrl + "\">" + serialSeoName + "-图片 </a></span>");
				//htmlList.Add("<label class=\"h3sublink\"><a href=\"" + allPicUrl + "\">" + picNum + "张</a></label></h3>");
				//htmlList.Add("<div class=\"more\">" + moreCateLink + "<a href=\"http://photo.bitauto.com/serial/" + serialId.ToString() + "/\">更多&gt;&gt;</a></div>");
				if (needMakePeceImage)
					MakePieceImageList(dsCsPic, categoryPicNum, htmlList, true);
				else
					MakeDefaultImageList(dsCsPic, categoryPicNum, htmlList);
				htmlList.Add("<div class=\"clear\"></div>");
				// modified by chengl Dec.27.2011
				htmlList.Add("<!-- baidu-tc begin {\"action\":\"DELETE\"} -->");
				if (needMakePeceImage)
				{
					htmlList.Add("<div class=\"more\">");
					if (dsCsPic.Tables.Contains("A"))
					{
						// 分类
						MakePhotoCateForStat(dsCsPic.Tables["A"], htmlList, 1, true);
					}
					htmlList.Add("<a id=\"115\" href=\"http://photo.bitauto.com/serial/" + serialId.ToString() + "/\">更多&gt;&gt;</a></div>");
				}
				else
				{
					htmlList.Add("<div class=\"more\">");
					if (dsCsPic.Tables.Contains("A"))
					{
						// 分类
						MakePhotoCateForStat(dsCsPic.Tables["A"], htmlList, 2, true);
					}
					htmlList.Add("<a id=\"215\" href=\"http://photo.bitauto.com/serial/" + serialId.ToString() + "/\">更多&gt;&gt;</a></div>");
				}
				htmlList.Add("<!-- baidu-tc end -->");
				// 
				htmlList.Add("</div>");
			}

			serialImageHtml = String.Concat(htmlList.ToArray());
		}

		/// <summary>
		/// 分类的HTML
		/// </summary>
		/// <param name="dt">数据源</param>
		/// <param name="htmlList">分类的HTML</param>
		/// <param name="idPrefix">a标签的ID前缀</param>
		/// <param name="isPushID">是否需要a标签的ID</param>
		private void MakePhotoCateForStat(DataTable dt, List<string> htmlList, int idPrefix, bool isPushID)
		{
			int cateId = 0;		//分类ID
			foreach (DataRow row in dt.Rows)
			{
				cateId = Convert.ToInt32(row["G"]);
				// 分类更多link
				if (cateId == 6)
				{
					if (isPushID)
					{
						htmlList.Add("<a id=\"" + idPrefix + "10\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/6/\">外观</a> | ");
					}
					else
					{
						htmlList.Add("<a href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/6/\">外观</a> | ");
					}
				}
				else if (cateId == 7)
				{
					if (isPushID)
					{
						htmlList.Add("<a id=\"" + idPrefix + "11\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/7/\">内饰</a> | ");
					}
					else
					{
						htmlList.Add("<a href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/7/\">内饰</a> | ");
					}
				}
				else if (cateId == 8)
				{
					if (isPushID)
					{
						htmlList.Add("<a id=\"" + idPrefix + "12\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/8/\">空间</a> | ");
					}
					else
					{
						htmlList.Add("<a href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/8/\">空间</a> | ");
					}
				}
				else if (cateId == 12)
				{
					if (isPushID)
					{
						htmlList.Add("<a id=\"" + idPrefix + "13\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/12/\">图解</a> | ");
					}
					else
					{
						htmlList.Add("<a href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/12/\">图解</a> | ");
					}
				}
				else if (cateId == 11)
				{
					if (isPushID)
					{
						htmlList.Add("<a id=\"" + idPrefix + "14\" href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/11/\">官方图</a> | ");
					}
					else
					{
						htmlList.Add("<a href=\"http://photo.bitauto.com/serialmore/" + serialId.ToString() + "/11/\">官方图</a> | ");
					}
				}
				else
				{ }
			}
		}

		private void MakePieceImageList(DataSet dsCsPic, Dictionary<int, int> categoryPicNum, List<string> htmlList, bool isPushID)
		{
			htmlList.Add("<div class=\"leftPic\">");
			DataTable dt = dsCsPic.Tables["C"];

			//图解
			MakeSinglePiece(dt, categoryPicNum, 12, htmlList, isPushID);
			//空间
			MakeSinglePiece(dt, categoryPicNum, 8, htmlList, isPushID);
			//内饰
			MakeSinglePiece(dt, categoryPicNum, 7, htmlList, isPushID);
			//外观
			MakeSinglePiece(dt, categoryPicNum, 6, htmlList, isPushID);
		}

		/// <summary>
		/// 拼单一图块
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="categoryPicNum"></param>
		/// <param name="cateId"></param>
		/// <param name="htmlList"></param>
		private void MakeSinglePiece(DataTable dt, Dictionary<int, int> categoryPicNum, int cateId, List<string> htmlList, bool isPushID)
		{
			string flagStr = "small";
			string cateName = String.Empty;
			int imgType = 1;

			string aLinkImgID = "";
			string aLinkTextID = "";
			string aLinkCountID = "";
			string aLinkMoreID = "";

			switch (cateId)
			{
				case 6:
					flagStr = "big";
					imgType = 3;
					cateName = "外观";
					aLinkImgID = "160";
					aLinkTextID = "161";
					aLinkCountID = "162";
					aLinkMoreID = "163";
					break;
				case 7:
					flagStr = "middle";
					imgType = 4;
					cateName = "内饰";
					aLinkImgID = "150";
					aLinkTextID = "151";
					aLinkCountID = "152";
					aLinkMoreID = "153";
					break;
				case 8:
					flagStr = "small";
					imgType = 1;
					cateName = "空间";
					aLinkImgID = "140";
					aLinkCountID = "141";
					break;
				case 12:
					flagStr = "small";
					imgType = 1;
					cateName = "图解";
					aLinkImgID = "130";
					aLinkCountID = "131";
					break;
			}
			DataRow[] rows = dt.Select("P='" + cateId + "'");
			if (rows.Length > 0)
			{
				DataRow row = rows[0];
				htmlList.Add("<div class=\"" + flagStr + "Pic\">");
				string catUrl = String.Format("http://photo.bitauto.com/serialmore/" + serialId + "/{0}/", cateId);
				int imgId = row["I"] == DBNull.Value ? 0 : Convert.ToInt32(row["I"]);
				string imgUrl = row["U"] == DBNull.Value ? "" : Convert.ToString(row["U"]);
				if (imgId == 0 || imgUrl.Length == 0)
					imgUrl = WebConfig.DefaultCarPic;
				else
					imgUrl = CommonFunction.GetPublishHashImgUrl(imgType, imgUrl, imgId);

				string picName = Convert.ToString(row["D"]);
				string picUrl = "http://photo.bitauto.com/picture/" + serialId + "/" + imgId + "/";
				htmlList.Add("<img src=\"" + imgUrl + "\" />");
				//if (isPushID)
				//{
				//    htmlList.Add("<a id=\"" + aLinkImgID + "\" href=\"" + picUrl + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" /></a>");
				//}
				//else
				//{
				//    htmlList.Add("<a href=\"" + picUrl + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" /></a>");
				//}
				htmlList.Add("<div class=\"" + flagStr + "TxtBg\"></div>");
				htmlList.Add("<div class=\"" + flagStr + "Txt\">");
				if (cateId == 6 || cateId == 7)
				{
					if (isPushID)
					{
						htmlList.Add("<h5><a id=\"" + aLinkTextID + "\" href=\"" + catUrl + "\" target=\"_blank\">" + cateName + "</a></h5>");
						htmlList.Add("<div class=\"moreTxt\"><span>（" + categoryPicNum[cateId] + "张）</span><a id=\"" + aLinkMoreID + "\"  href=\"" + catUrl + "\"target=\"_blank\">查看更多<s class=\"fSong\">&gt;&gt;</s></a></div>");
					}
					else
					{
						htmlList.Add("<h5><a href=\"" + catUrl + "\" target=\"_blank\">" + cateName + "</a></h5>");
						htmlList.Add("<div class=\"moreTxt\"><span>（" + categoryPicNum[cateId] + "张）</span><a href=\"" + catUrl + "\"target=\"_blank\">查看更多<s class=\"fSong\">&gt;&gt;</s></a></div>");
					}
				}
				else
				{
					if (isPushID)
					{
						htmlList.Add("<a id=\"" + aLinkCountID + "\" href=\"" + catUrl + "\" target=\"_blank\">" + cateName + "</a><span>（" + categoryPicNum[cateId] + "张）</span>");
					}
					else
					{
						htmlList.Add("<a href=\"" + catUrl + "\" target=\"_blank\">" + cateName + "</a><span>（" + categoryPicNum[cateId] + "张）</span>");
					}
				}
				htmlList.Add("</div>");
				htmlList.Add("<a id=\"" + aLinkImgID + "\" class=\"car_pic_float\" href=\"" + picUrl + "\"><span></span></a>");
				htmlList.Add("</div>");

				if (cateId == 7)
					htmlList.Add("</div>");
			}

		}

		private void MakeDefaultImageList(DataSet dsCsPic, Dictionary<int, int> categoryPicNum, List<string> htmlList)
		{

			int count = 0;		//有图片类型计数
			if (categoryPicNum.Count > 0 && dsCsPic.Tables.Contains("C") && dsCsPic.Tables["C"].Rows.Count > 0)
			{
				RenderPicByCategroy(htmlList, 6, "外观", categoryPicNum, dsCsPic.Tables["C"], ref count);
				RenderPicByCategroy(htmlList, 7, "内饰", categoryPicNum, dsCsPic.Tables["C"], ref count);
				RenderPicByCategroy(htmlList, 8, "空间", categoryPicNum, dsCsPic.Tables["C"], ref count);
				RenderPicByCategroy(htmlList, 12, "图解", categoryPicNum, dsCsPic.Tables["C"], ref count);
				RenderPicByCategroy(htmlList, 11, "官方图", categoryPicNum, dsCsPic.Tables["C"], ref count);
			}
			else
			{

				//显示其他图
				if (dsCsPic.Tables.Contains("A"))
				{
					foreach (DataRow row in dsCsPic.Tables["A"].Rows)
					{
						int cateNum = Convert.ToInt32(row["N"]);
						int cateId = Convert.ToInt32(row["G"]);
						categoryPicNum[cateId] = cateNum;
					}

					foreach (DataRow row in dsCsPic.Tables["A"].Rows)
					{
						int cateId = Convert.ToInt32(row["G"]);
						string cateName = Convert.ToString(row["D"]);
						//picNum += categoryPicNum[cateId];
						RenderPicByCategroy(htmlList, cateId, cateName, categoryPicNum, dsCsPic.Tables["C"], ref count);
					}
				}
			}

		}

		/// <summary>
		/// 是否需要生成大图片的块
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		private bool NeedToMakePieceHtml(int serialId, DataSet ds)
		{
			bool need = false;
			if (serialId == 2608 || serialId == 2370 || serialId == 3398)
			{
				if (ds.Tables.Contains("C"))
				{
					DataTable cTable = ds.Tables["C"];
					DataRow[] wgRows = cTable.Select("P='6'");
					DataRow[] nsRows = cTable.Select("P='7'");
					DataRow[] kjRows = cTable.Select("P='8'");
					DataRow[] tjRows = cTable.Select("P='12'");
					if (wgRows.Length > 0 && nsRows.Length > 0 && kjRows.Length > 0 && tjRows.Length > 0)
						need = true;
				}
			}
			return need;
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
		private void RenderPicByCategroy(List<string> htmlList, int cateId, string cateName, Dictionary<int, int> picNumDic, DataTable dt, ref int count)
		{
			// 分类统计 id前缀
			int catePrefix = 0;
			switch (cateId)
			{
				case 6: catePrefix = 2; break;
				case 7: catePrefix = 3; break;
				case 8: catePrefix = 4; break;
				case 12: catePrefix = 5; break;
				case 11: catePrefix = 6; break;
				default: catePrefix = 0; break;
			}

			if (dt == null || dt.Rows.Count < 1)
				return;
			if (!picNumDic.ContainsKey(cateId))
				return;
			count++;
			int picNum = picNumDic[cateId];
			string cateURL = "http://photo.bitauto.com/serialmore/" + serialId + "/" + cateId + "/";
			htmlList.Add("<h4><a id=\"2" + catePrefix + "0\" href=\"" + cateURL + "\" >" + cateName + "<span class=\"a\">(" + picNum + "张)</span></a> <a id=\"2" + catePrefix + "5\" href=\"" + cateURL + "\" class=\"more\">更多照片&gt;&gt;</a></h4>");
			if (count == 1)
				htmlList.Add("<div class=\"pic_album\">");
			htmlList.Add("<ul class=\"list_pic\">");

			int loop = 1;
			foreach (DataRow row in dt.Select("P='" + cateId + "'"))
			{
				int imgId = row["I"] == DBNull.Value ? 0 : Convert.ToInt32(row["I"]);
				string imgUrl = row["U"] == DBNull.Value ? "" : Convert.ToString(row["U"]);
				if (imgId == 0 || imgUrl.Length == 0)
					imgUrl = WebConfig.DefaultCarPic;
				else
					imgUrl = CommonFunction.GetPublishHashImgUrl(1, imgUrl, imgId);

				string picName = Convert.ToString(row["D"]);
				string picUlr = "http://photo.bitauto.com/picture/" + serialId + "/" + imgId + "/";
				// add by chengl Dec.23.2011 for baidu TC
				if (loop >= 2)
				{ htmlList.Add("<!-- baidu-tc begin {\"action\":\"DELETE\"} -->"); }
				htmlList.Add("<li><a id=\"2" + catePrefix + "" + loop + "0\" href=\"" + picUlr + "\"><img data-original=\"" + imgUrl + "\" alt=\"" + masterBrandName + serialName + picName + "\" width=\"165\" height=\"110\"></a><a id=\"2" + catePrefix + "" + loop + "1\" href=\"" + picUlr + "\" >" + picName + "</a></li>");
				if (loop >= 2)
				{ htmlList.Add("<!-- baidu-tc end -->"); }
				// add by chengl Dec.23.2011 
				loop++;
				if (loop > 4)
				{ break; }
			}
			htmlList.Add("</ul>");
			if (count < picNumDic.Count)
				htmlList.Add("<div class=\"line\"></div>");
			htmlList.Add("<div class=\"clear\"></div>  ");
			if (count == picNumDic.Count)
				htmlList.Add("</div>");
		}
		/*modified by sk 2013-09-13 旧版视频废除 
		/// <summary>
		/// 生成视频
		/// </summary>
		private void MakeVideHtml()
		{
			int[] videoSeriaIdArray = { 2370, 2608, 3398, 3023, 2388, 2371, 3635, 1568, 2381, 2593 };
			if (videoSeriaIdArray.Contains(serialId))
			{
				NewMakeVideoBlockHtml();
				return;
			}
			StringBuilder htmlCode = new StringBuilder();
			//取数据
			XmlNodeList videoList = new Car_SerialBll().GetSerialVideo(serialId);
			if (videoList.Count > 0)
			{
				htmlCode.Append("<div class=\"line_box vlist\">");
				htmlCode.Append("<h3>");
				htmlCode.Append("<span><a href=\"" + baseUrl + "shipin/\">");
				htmlCode.Append(serialSeoName + "-视频</a></span></h3>");
				htmlCode.Append("<div class=\"more\">");
				htmlCode.Append("<a rel=\"nofollow\" href=\"" + baseUrl + "shipin/\">更多&gt;&gt;</a></div>");
				htmlCode.Append("<div class=\"pic_album\">");
				htmlCode.Append("<ul class=\"list_pic\">");
				try
				{
					int loop = 1;
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

						htmlCode.Append("<li><a rel=\"nofollow\" href=\"" + filepath + "\"class=\"v_bg\" alt=\"视频播放\"></a><a href=\"" + filepath + "\"><img data-original=\"" + imgUrl + "\" alt=\"" + masterBrandName + serialName + videoTitle + "\" width=\"165\" height=\"110\" /></a>");
						if (shortTitle != videoTitle)
							htmlCode.Append("<div class=\"name\"><a href=\"" + filepath + "\" title=\"" + videoTitle + "\" >" + shortTitle + "</a></div></li>");
						else
							htmlCode.Append("<div class=\"name\"><a href=\"" + filepath + "\">" + videoTitle + "</a></div></li>");
						// modified by chengl Dec.21.2011
						loop++;
						if (loop > 4)
						{ break; }
					}
				}
				catch
				{ }
				htmlCode.Append("</ul>");
				htmlCode.Append("<div class=\"clear\"></div>");
				htmlCode.Append("</div>");
				htmlCode.Append("</div>");
			}

			videosHtml = htmlCode.ToString();
		}
		*/
		/// <summary>
		/// 新视频块html
		/// </summary>
		private void NewMakeVideoBlockHtml()
		{
			#region modified by sk 2013-09-13 废除视频
			/* 
 			//取数据
			XmlNodeList nodeList = new Car_SerialBll().GetSerialVideo(serialId);
			if (nodeList.Count <= 0) return;
			StringBuilder sb = new StringBuilder();
			sb.Append("<div class=\"line_box\" id=\"car-videobox\">");
			sb.Append("	<h3>");
			sb.Append("		<span>");
			sb.AppendFormat("			<a href=\"{0}shipin/\">{1}视频</a>", baseUrl, serialSeoName);
			sb.Append("		</span>");
			sb.Append("	</h3>");
			sb.Append("	<div class=\"car-video20130802\">");
			sb.Append("		<ul>");
			int loop = 0;
			foreach (XmlNode node in nodeList)
			{
				loop++;
				if (loop > 4) break;
				string videoTitle = node.SelectSingleNode("title").InnerText;
				videoTitle = StringHelper.RemoveHtmlTag(videoTitle);

				string shortTitle = node.SelectSingleNode("facetitle").InnerText;
				shortTitle = StringHelper.RemoveHtmlTag(shortTitle);

				string imgUrl = node.SelectSingleNode("picture").InnerText;
				if (imgUrl.Trim().Length == 0)
					imgUrl = WebConfig.DefaultVideoPic;
				//imgUrl = imgUrl.Replace("/bitauto/", "/newsimg-242-w0-1-q70/bitauto/");
				//imgUrl = imgUrl.Replace("/autoalbum/", "/newsimg-242-w0-1-q70/autoalbum/");
				string filepath = node.SelectSingleNode("filepath").InnerText;

				sb.Append("			<li>");
				sb.AppendFormat("				<a href=\"{0}\" target=\"_blank\"><img data-original=\"{1}\" alt=\"{2}\" width=\"170\" height=\"96\"></a>", filepath, imgUrl, masterBrandName + serialName + videoTitle);
				sb.AppendFormat("				<a href=\"{0}\" target=\"_blank\">{1}</a>", filepath, shortTitle != videoTitle ? shortTitle : videoTitle);
				sb.AppendFormat("				<a href=\"{0}\" target=\"_blank\" class=\"btn-play\">播放</a>", filepath);
				sb.Append("			</li>");
			}
			sb.Append("		</ul>");
			sb.Append("		<div class=\"clear\"></div>");
			sb.Append("	</div>");
			sb.Append("	<div class=\"more\">");
			sb.AppendFormat("		<a href=\"{0}shipin/\">更多&gt;&gt;</a>", baseUrl, nodeList.Count);
			sb.Append("	</div>");
			sb.Append("</div>");
			videosHtml = sb.ToString();
			 * */
			#endregion

			int video = (int)CommonHtmlEnum.BlockIdEnum.Video;
			if (dictSerialBlockHtml.ContainsKey(video))
				videosHtml = dictSerialBlockHtml[video];
		}


		private void MakeDianpingHtml()
		{
			XmlDocument dpDoc = new Car_SerialBll().GetCarshowSerialDianping(serialId);
			if (dpDoc == null || !dpDoc.HasChildNodes)
			{ return; }
			StringBuilder htmlCode = new StringBuilder();
			int count = ConvertHelper.GetInteger(dpDoc.DocumentElement.GetAttribute("count"));
			int dianpingCount = count;
			htmlCode.Append("<h3><span>" + serialSeoName + "-点评精选</span><strong><em>" + count + "条</em>|");
			htmlCode.Append("<a href=\"/" + serialSpell + "/koubei/tianjia/\">我要点评</a>");
			htmlCode.Append("|<a href=\"http://i.bitauto.com/FriendMore_c0_s" + serialId + "_p1_sort1_r001.html\">和车主聊聊</a>");
			htmlCode.Append("|<a href=\"http://i.bitauto.com/FriendMore_c0_s" + serialId + "_p1_sort1_r010.html\">和想买的人聊聊</a></strong></h3>");

			StringBuilder tabCode = new StringBuilder();
			StringBuilder conCode = new StringBuilder();
			string moreUrl = baseUrl + "koubei/gengduo/";
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
			// modified by chengl Dec.27.2011
			htmlCode.Append("<!-- baidu-tc begin {\"action\":\"DELETE\"} -->");
			htmlCode.Append("<div class=\"more\">");
			htmlCode.Append("<a href=\"" + moreUrl + "\">更多&gt;&gt;</a></div>");
			htmlCode.Append("<!-- baidu-tc end -->");
			dianpingHtml = htmlCode.ToString();
		}


		/// <summary>
		/// 生成热门文章
		/// </summary>
		/// <param name="htmlCode"></param>
		private void MakeHotNewsHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			//获取数据
			XmlDocument xmlDoc = new Car_SerialBll().GetSerialHotNews(serialId);
			XmlNodeList newsList = xmlDoc.SelectNodes("NewDataSet/NewsCommentTop");
			if (newsList.Count > 0)
			{
				htmlCode.Append("<div class=\"line_box hot_article\">");
				htmlCode.Append("<h3><span>" + serialShowName.Replace("(进口)", "").Replace("（进口）", "") + "热门文章</span></h3>");
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
			hotNewsHtml = htmlCode.ToString();
		}

		/// <summary>
		/// 子品牌还关注
		/// </summary>
		/// <returns></returns>
		private void MakeSerialToSerialHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			int loop = 1;
			List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(serialId, 8);

			Dictionary<int, Dictionary<string, string>> serialAdList = GetToSeeAD();

			if (serialAdList != null && serialAdList.Count > 0)
			{
				for (int i = lsts.Count - 1; i >= 0; i--)
				{
					EnumCollection.SerialToSerial item = lsts[i];
					foreach (Dictionary<string, string> values in serialAdList.Values)
					{
						if (values.ContainsValue(item.ToCsID.ToString()))
						{
							lsts.Remove(item);
							break;
						}
					}
				}
			}

			if (lsts.Count > 0)
			{
				foreach (EnumCollection.SerialToSerial sts in lsts)
				{
					if (serialAdList != null && serialAdList.ContainsKey(loop))
					{
						htmlCode.Append("<li><a rel=\"nofollow\" href=\"").Append(serialAdList[loop]["Url"]).Append("\">");
						htmlCode.Append("<img src=\"").Append(serialAdList[loop]["ImgUrl"]).Append("\" alt=\"").Append(serialAdList[loop]["Title"]).Append("\" width=\"90\" height=\"60\"></a>");
						htmlCode.Append("<a href=\"").Append(serialAdList[loop]["Url"]).Append("\">").Append(serialAdList[loop]["Title"]).Append("</a>");
						if (!string.IsNullOrEmpty(serialAdList[loop]["PriceRange"]))
						{
							htmlCode.Append("<div>").Append(serialAdList[loop]["PriceRange"]).Append("</div>");
						}
						else
						{
							htmlCode.Append("<div>").Append(GetSerialPriceRangeByID(ConvertHelper.GetInteger(serialAdList[loop]["AD_SerialID"]))).Append("</div>");
						}
						htmlCode.Append("</li>");
					}
					else
					{
						string csName = sts.ToCsShowName.ToString();
						htmlCode.Append("<li><a rel=\"nofollow\" href=\"/" + sts.ToCsAllSpell.ToString().ToLower() + "/\">");
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
					if (++loop > 6)
					{
						break;
					}
				}
			}
			serialToSeeHtml = htmlCode.ToString();
		}

		/// <summary>
		/// 获取看过还看过广告
		/// </summary>
		/// <returns></returns>
		private Dictionary<int, Dictionary<string, string>> GetToSeeAD()
		{
			string key = "serialToSeeADConfig";
			DateTime curDate = DateTime.Now;
			bool isTest = false;
			if (!String.IsNullOrEmpty(Request.QueryString["testdate"]))
			{
				curDate = Convert.ToDateTime(Request.QueryString["testdate"]);
				isTest = true;
			}
			Dictionary<int, Dictionary<int, Dictionary<string, string>>> cacheData = CacheManager.GetCachedData(key) as Dictionary<int, Dictionary<int, Dictionary<string, string>>>;
			if (cacheData == null || isTest)
			{
				string filePath = Path.Combine(Server.MapPath("~"), "App_Data\\SerialToSeeADConfig.xml");
				if (!File.Exists(filePath))
					return null;
				cacheData = new Dictionary<int, Dictionary<int, Dictionary<string, string>>>();
				CacheManager.InsertCache(key, cacheData, 30);
				Dictionary<int, Dictionary<string, string>> serialData = null;
				FileStream stream = null;
				XmlReader reader = null;
				try
				{
					stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
					reader = XmlReader.Create(stream);
					while (reader.Read())
					{
						if (reader.NodeType == XmlNodeType.Element && reader.Name == "AD")
						{
							using (XmlReader subReader = reader.ReadSubtree())
							{
								reader.MoveToAttribute("ad_serialid");
								string adserailid = reader.Value;
								while (subReader.Read())
								{
									if (subReader.NodeType == XmlNodeType.Element && subReader.Name == "Serial")
									{
										string title = string.Empty, url = string.Empty, imgUrl = string.Empty, priceRange = string.Empty;
										int postion = 0, toSerialId = 0;
										bool needAd = true;
										while (subReader.Read())
										{
											if (subReader.NodeType == XmlNodeType.EndElement && subReader.Name == "Serial")
												break;
											if (reader.NodeType == XmlNodeType.Element)
											{
												if (reader.Name == "Position")
												{
													postion = ConvertHelper.GetInteger(reader.ReadString().Trim());
												}
												else if (reader.Name == "SerialID")
												{
													toSerialId = ConvertHelper.GetInteger(reader.ReadString().Trim());
												}
												else if (reader.Name == "Title")
												{
													title = reader.ReadString().Trim();
												}
												else if (reader.Name == "Url")
												{
													url = reader.ReadString().Trim();
												}
												else if (reader.Name == "ImgUrl")
												{
													imgUrl = reader.ReadString().Trim();
												}
												else if (reader.Name == "StartDate")
												{
													DateTime startDate = ConvertHelper.GetDateTime(reader.ReadString().Trim());
													if (startDate > curDate)
													{
														needAd = false;
														break;
													}
												}
												else if (reader.Name == "EndData")
												{
													DateTime endDate = ConvertHelper.GetDateTime(reader.ReadString().Trim());
													if (endDate.AddDays(1) < curDate)
													{
														needAd = false;
														break;
													}
												}
												else if (reader.Name == "PriceRange")
												{
													priceRange = reader.ReadString().Trim();
												}
											}
										}

										if (needAd)
										{
											if (cacheData.ContainsKey(toSerialId))
												serialData = cacheData[toSerialId];
											else
											{
												serialData = new Dictionary<int, Dictionary<string, string>>();
												cacheData.Add(toSerialId, serialData);
											}

											serialData[postion] = new Dictionary<string, string>();
											serialData[postion]["AD_SerialID"] = adserailid;
											serialData[postion]["Title"] = title;
											serialData[postion]["Url"] = url;
											serialData[postion]["ImgUrl"] = imgUrl;
											serialData[postion]["PriceRange"] = priceRange;
											//{{ , },{ , },{, },{, }};
										}
									}
								}
							}
						}
					}
				}
				catch { }
				finally
				{
					if (reader != null)
						reader.Close();
					if (stream != null)
						stream.Dispose();
				}
			}
			return cacheData.ContainsKey(serialId) ? cacheData[serialId] : null;
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
			if (carSerialBaseList != null && carSerialBaseList.Count > 0 && carSerialBaseList.ContainsKey("全国"))
			{
				List<Car_SerialBaseEntity> serialCompareList = carSerialBaseList["全国"];
				htmlList.Add("<div class=\"line_box h160 ofh\" id=\"serialHotCompareList\">");
				htmlList.Add(string.Format("<h3><span><a rel=\"nofollow\" href=\"/chexingduibi/?carids={0}&ispk=1\" target=\"_blank\">大家都用他和谁比</a></span></h3>", hotCarID));
				htmlList.Add("<div style=\"display: block;\">");
				htmlList.Add("<div id=\"rank_model_box\" class=\"ranking_list\">");
				htmlList.Add("<ol class=\"carContrast\">");

				//htmlList.Add("<h3><span>网友都用它和谁比</span></h3>");
				//htmlList.Add("<div class=\"more\"><a href=\"/chexingduibi/\">车型对比&gt;&gt;</a></div>");
				//htmlList.Add("<div class=\"ranking_list\" id=\"rank_model_box\">");
				//htmlList.Add("<ol class=\"carContrast\">");

				for (int i = 0; i < serialCompareList.Count; i++)
				{
					Car_SerialBaseEntity carSerial = serialCompareList[i];
					if (i == serialCompareList.Count - 1)
						htmlList.Add("<li class=\"last\">");
					else
						htmlList.Add("<li>");
					htmlList.Add("<em>" + BitAuto.Utils.StringHelper.SubString(serialShowName, 10, false) + " <s>VS</s> ");
					htmlList.Add(carSerial.SerialShowName.Trim() + "</em>");
					htmlList.Add("<span><a href=\"" + compareBaseUrl + carSerial.SerialId + "\" target=\"_blank\">对比&gt;&gt;</a></span></li>");
				}

				htmlList.Add("</ol></div>");
				htmlList.Add("</div>");
				//string adStr = base.GetSerialSummaryRightADByLevel(cse.Level.Name);
				//htmlList.Add("<div style=\"display: none;\" id=\"data_box6_1\">");
				//htmlList.Add(adStr);
				//htmlList.Add("</div>");

				//htmlList.Add("<div class=\"clear\"></div>");
				//htmlList.Add(" <div class=\"h3_tab\">");
				//htmlList.Add("<ul id=\"data_tab6\">");
				//htmlList.Add("<li class=\"current\"><a href=\"/chexingduibi/\" target=\"_blank\">同级别对比</a></li>");
				//htmlList.Add("<li class=\"\"><a href=\"http://top.baidu.com/buzz?b=176\" target=\"_blank\">热点推荐</a></li>");
				//htmlList.Add("</ul>");
				//htmlList.Add("</div>");

				htmlList.Add("</div>");
			}

			hotSerialCompareHtml = String.Concat(htmlList.ToArray());
		}

		private void MakeSerialIntensionHtml()
		{
			intensionHtml = "";
			Dictionary<int, List<XmlElement>> intensionDic = new Car_SerialBll().GetSerialIntensionDic();
			if (!intensionDic.ContainsKey(serialId))
				return;

			StringBuilder htmlCode = new StringBuilder();
			int counter = 0;
			foreach (XmlElement userNode in intensionDic[serialId])
			{
				counter++;
				string userName = userNode.SelectSingleNode("name").InnerText;
				string shortName = userName;
				if (StringHelper.GetRealLength(userName) > 8)
					shortName = StringHelper.SubString(userName, 8, true);
				userName = StringHelper.RemoveHtmlTag(userName);
				shortName = StringHelper.RemoveHtmlTag(shortName);
				string userUrl = userNode.SelectSingleNode("url").InnerText;
				htmlCode.Append("<li><a href=\"" + userUrl + "\" title=\"" + userName + "\">" + shortName + "</a></li>");
				if (counter >= 9)
					break;
			}
			intensionHtml = htmlCode.ToString();
		}
		/// <summary>
		/// 得到品牌下的其他子品牌
		/// </summary>
		/// <returns></returns>
		protected string GetBrandOtherSerial()
		{
			string contentHTML = new Car_SerialBll().GetBrandOtherSerialList(cse);
			if (!string.IsNullOrEmpty(contentHTML))
			{
				contentHTML = string.Concat("<div class=\"line_box review_list autoheight\">", contentHTML, "</div>");
			}
			return contentHTML;
		}

		/// <summary>
		/// 取子品牌相关用户
		/// </summary>
		private void GetUserBlockByCarSerialId()
		{
			StringBuilder sbUserBlock = new StringBuilder();
			// 计划购买
			DataTable dtWant = base.GetUserByCarSerialId(sic.CsID, 2, 3);
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
					sbUserBlock.Append("<p><a class=\"add_friend\" href=\"#\" onclick=\"javascript:AjaxAddFriend.show(" + dtWant.Rows[i]["userId"].ToString() + ", " + sic.CsID.ToString() + ");return false;\">加为好友</a></p></li>");
				}
				sbUserBlock.Append("</ul>");
				sbUserBlock.Append("</div><div class=\"clear\"> </div>");
				sbUserBlock.Append("<div class=\"more\"><a href=\"http://i.bitauto.com/FriendMore_c0_s" + sic.CsID.ToString() + "_p1_sort1_r010.html\">更多&gt;&gt;</a></div>");
				sbUserBlock.Append("</div>");
			}
			// 车主
			DataTable dtOwner = base.GetUserByCarSerialId(sic.CsID, 3, 3);
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
					sbUserBlock.Append("<p><a class=\"add_friend\" href=\"#\" onclick=\"AjaxAddFriend.show(" + dtOwner.Rows[i]["userId"].ToString() + ", " + sic.CsID.ToString() + ",3);return false;\">加为好友</a></p></li>");
				}
				sbUserBlock.Append("</ul>");
				sbUserBlock.Append("</div><div class=\"clear\"> </div>");
				sbUserBlock.Append("<div class=\"more\"><a href=\"http://i.bitauto.com/FriendMore_c0_s" + sic.CsID.ToString() + "_p1_sort1_r001.html\">更多&gt;&gt;</a></div>");
				sbUserBlock.Append("</div>");
			}
			UserBlock = sbUserBlock.ToString();
		}
		/// <summary>
		/// 取试驾评测html
		/// </summary>
		private void MakeEditorCommentHtml()
		{
			this.editorCommentHtml = string.Empty;
			// modified by chengl Oct.15.2013
			//int firstCarId = new CarNewsBll().GetEditorCommentCarId(serialId);
			//if (firstCarId > 0)
			//{
			string htmlFile = Path.Combine(WebConfig.DataBlockPath, string.Format(_EditorCommentHtmlPath, serialId));
			if (File.Exists(htmlFile))
			{
				this.editorCommentHtml = File.ReadAllText(htmlFile);
			}
			//}
		}
		/// <summary>
		/// 取子品牌答疑块
		/// modified by chengl Jun.22.2011
		/// 答疑块改版
		/// </summary>
		private void MakeSerialAskHtml()
		{
			//string askHTMLFile = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\AskForCar\\Serial\\Html\\{0}.htm", serialId));
			//if (File.Exists(askHTMLFile))
			//{ serialAskHtml = File.ReadAllText(askHTMLFile); }

			////获取数据
			//StringBuilder htmlCode = new StringBuilder();
			//XmlDocument xmlDoc = new Car_SerialBll().GetSerialAskEntries(serialId);
			//XmlNamespaceManager xnm = new XmlNamespaceManager(xmlDoc.NameTable);
			//xnm.AddNamespace("atom", "http://www.w3.org/2005/Atom");
			//XmlNodeList entries = xmlDoc.SelectNodes("//atom:entry", xnm);
			//htmlCode.Append("<div class=\"line_box ask_box\">");
			//htmlCode.Append("<h3><span><a href=\"http://ask.bitauto.com/" + serialId.ToString() + "/\">在线答疑</a></span></h3>");
			//htmlCode.Append("<div class=\"more\"><a href=\"http://ask.bitauto.com/ask?q=" + Server.UrlEncode(sic.CsName) + "\">我要提问</a></div>");
			//htmlCode.Append("<div class=\"search\">");
			//htmlCode.Append("<fieldset>");
			//htmlCode.Append("<input id=\"askkeyword\" name=\"\" class=\"input_text\" type=\"text\"> <input name=\"\" class=\"btn_search\" onclick=\"window.open('http://ask.bitauto.com/search?keyword=' + document.getElementById('askkeyword').value)\" value=\"找答案\" type=\"button\">");
			//htmlCode.Append("</fieldset>");
			//htmlCode.Append("</div>");
			//htmlCode.Append("<ul class=\"list\">");
			//foreach (XmlElement entryNode in entries)
			//{
			//    string newsTitle = entryNode.SelectSingleNode("atom:title", xnm).InnerText;
			//    //过滤Html标签
			//    newsTitle = BitAuto.Utils.StringHelper.RemoveHtmlTag(newsTitle);
			//    string shortTitle = BitAuto.Utils.StringHelper.SubString(newsTitle, 30, true);
			//    string askLink = entryNode.SelectSingleNode("atom:link", xnm).Attributes["href"].Value;
			//    if (newsTitle != shortTitle)
			//        htmlCode.Append("<li><a href=\"" + askLink + "\" title=\"" + newsTitle + "\">" + shortTitle + "</a></li>");
			//    else
			//        htmlCode.Append("<li><a href=\"" + askLink + "\">" + newsTitle + "</a></li>");
			//}
			//htmlCode.Append("</ul>");
			//htmlCode.Append("</div>");
			//serialAskHtml = htmlCode.ToString();
		}

		/// <summary>
		/// 生成维修保养信息
		/// </summary>
		private void MakeMaintainceHtml()
		{
			string info = new Car_SerialBll().GetMaintanceContent(serialId);
			if (info.Length > 0)
			{
				string[] htmlList = new string[9];
				htmlList[0] = "<div class=\"line_box\" style=\"margin-bottom:0; border-bottom:0\">";
				htmlList[1] = "<h3><span>" + serialSeoName + "保养周期表</span><strong>保养数据仅供参考，请以汽车生产厂指导为准。</strong></h3>";
				htmlList[2] = "<div class=\"more\"><a href=\"/" + serialSpell + "/baoyang/\">更多&gt;&gt;</a></div>";
				htmlList[3] = "</div>";
				htmlList[4] = "<div class=\"f0803_03 h2table\">";
				htmlList[5] = "<div class=\"mtable2\">";
				htmlList[6] = "<div class=\"data_table\">";
				htmlList[7] = info;
				htmlList[8] = "</div></div></div>";
				maintainceHtml = string.Concat(htmlList);
			}
			else
				maintainceHtml = String.Empty;
		}


		/// <summary>
		/// 生成Flash 广告
		/// </summary>
		private void MakeFlashADCode()
		{
			// modified by chengl Sep.25.2010  Del by lisuo

			//if (serialId == 1991)
			//{
			//    //广丰凯美瑞
			//    FlashADCode = base.GetFlashCode("/flash/EC7-Camry-720-80.swf", 720, 80);
			//}
			//else if (serialId == 1909)
			//{
			//    //一汽迈腾
			//    FlashADCode = base.GetFlashCode("/flash/EC7-Magotan-720-80.swf", 720, 80);
			//}
			//else
			//{
			//    // 其他子品牌默认广告
			//    FlashADCode = "";// "<ins id=\"div_37940534-3acb-4358-8f99-ac9abc6624ca\" type=\"ad_play\" adplay_IP=\"\" adplay_AreaName=\"\"  adplay_CityName=\"\"    adplay_BrandID=\"\"  adplay_BrandName=\"\"  adplay_BrandType=\"\"  adplay_BlockCode=\"37940534-3acb-4358-8f99-ac9abc6624ca\"> </ins>"; 
			//}
		}

		//[Obsolete("新闻服务上线后，将由InitNextSeeNew方法代替。")]
		//private void InitNextSee()
		//{
		//    nextSeePingceHtml = String.Empty;
		//    nextSeeXinwenHtml = String.Empty;
		//    nextSeeDaogouHtml = String.Empty;

		//    DataSet newsDs = new Car_SerialBll().GetNewsListBySerialId(serialId, "pingce");
		//    if (newsDs != null && newsDs.Tables.Contains("listNews") && newsDs.Tables["listNews"].Rows.Count > 0)
		//        nextSeePingceHtml = "<li><a href=\"/" + serialSpell + "/pingce/\" target=\"_self\">" + serialShowName + "<span>车型详解</span></a></li>";
		//    newsDs = new Car_SerialBll().GetNewsListBySerialId(serialId, "xinwen");
		//    if (newsDs != null && newsDs.Tables.Contains("listNews") && newsDs.Tables["listNews"].Rows.Count > 0)
		//        nextSeeXinwenHtml = "<li><a href=\"/" + serialSpell + "/xinwen/\" target=\"_self\">" + serialShowName + "<span>新闻</span></a></li>";
		//    newsDs = new Car_SerialBll().GetNewsListBySerialId(serialId, "daogou");
		//    if (newsDs != null && newsDs.Tables.Contains("listNews") && newsDs.Tables["listNews"].Rows.Count > 0)
		//        nextSeeDaogouHtml = "<li><a href=\"/" + serialSpell + "/daogou/\" target=\"_self\">" + serialShowName + "<span>导购</span></a></li>";

		//}

		private void InitNextSeeNew()
		{
			nextSeePingceHtml = String.Empty;
			nextSeeXinwenHtml = String.Empty;
			nextSeeDaogouHtml = String.Empty;
			CarNewsBll newsBll = new CarNewsBll();
			if (newsBll.IsSerialNews(serialId, 0, CarNewsType.pingce))
				nextSeePingceHtml = "<li><a rel=\"nofollow\" href=\"/" + serialSpell + "/pingce/\" target=\"_self\">" + serialShowName + "<span>车型详解</span></a></li>";
			//未使用-anh 20120326
			//if (newsBll.IsSerialNews(serialId, 0, CarNewsType.xinwen))
			//    nextSeeXinwenHtml = "<li><a href=\"/" + serialSpell + "/xinwen/\" target=\"_self\">" + serialShowName + "<span>新闻</span></a></li>";

			if (newsBll.IsSerialNews(serialId, 0, CarNewsType.daogou))
				nextSeeDaogouHtml = "<li><a rel=\"nofollow\" href=\"/" + serialSpell + "/daogou/\" target=\"_self\">" + serialShowName + "<span>导购</span></a></li>";
		}

		/// <summary>
		/// 核心看点
		/// </summary>
		protected void WriteHexinkandianHTML()
		{
			string path = System.IO.Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\SerialSet\\HeXinLianDianHtml\\Serial_{0}.html", serialId));
			if (System.IO.File.Exists(path))
			{
				FileStream stream = null;
				StreamReader reader = null;
				try
				{
					stream = new FileStream(path, FileMode.Open, FileAccess.Read);
					reader = new StreamReader(stream, Encoding.UTF8);
					Response.Write(reader.ReadToEnd());
				}
				catch { }
				finally
				{
					if (reader != null) reader.Close();
					if (stream != null) stream.Close();
				}
			}
		}
	}
}