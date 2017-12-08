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

namespace BitAuto.CarChannel.CarchannelWeb.CarTreeV2
{
    public partial class Cartree_SelectCar : TreePageBase
    {
        private SelectCarParameters selectParas;
        private string displayMode;
        private string sortMode;
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
            base.SetPageCache(30);
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
            this.MakeConditionsHtml2016();

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
            List<SuperSerialInfo2016> adCarList = new List<SuperSerialInfo2016>();
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
                        adCarList.Add(new SuperSerialInfo2016(serialAd.SerialId, serialEntity.ShowName, serialEntity.AllSpell)
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
            //排序
            sortMode = Request.QueryString["s"];

            //页号
            tmpStr = Request.QueryString["page"];
            bool isPage = Int32.TryParse(tmpStr, out pageNum);
            if (!isPage)
                pageNum = 1;
            pageSize = 20;
        }

        /// <summary>
        /// 生成搜过栏的Html
        /// </summary>
        /// <returns></returns>
        private string MakeSelectBarHtml()
        {
            StringBuilder htmlCode = new StringBuilder();
            htmlCode.AppendLine("<div class=\"box\">");
            htmlCode.AppendLine("    <h2>选新车</h2>");
            htmlCode.AppendLine("    <span class=\"header-note1\" id=\"styleTotal\"></span>");
            if (LevelName == "SUV")
            {
                htmlCode.AppendLine("    <a class=\"header-note1 type-1\" target=\"_blank\" href=\"http://baike.bitauto.com/other/20120629/1405744796.html\">什么是SUV?</a> <a class=\"header-note1 type-1\" target=\"_blank\" href=\"/suv/all/\">进入SUV频道>></a>");
            }
            htmlCode.AppendLine("</div>");
            htmlCode.AppendLine("<div class=\"more\" id=\"carsort\">");

            if (sortMode == "1" || string.IsNullOrWhiteSpace(sortMode))
            {
                htmlCode.AppendLine("<a href=\"javascript:GotoPage('s6','anchorcarlist');\" class=\"down-arrow\">按口碑评分</a><a href=\"javascript:GotoPage('s" + (sortMode == "1" ? "0" : "1") + "','anchorcarlist');\" data-channelid=\"2.116.1292\" class=\"" + (sortMode == "1" ? "up-arrow current" : "down-arrow current") + "\">按关注</a><a href=\"javascript:GotoPage('s2','anchorcarlist');\" class=\"up-arrow\" data-channelid=\"2.116.1293\">按价格</a>");
            }
            else if (sortMode == "2" || sortMode == "3")
            {
                htmlCode.AppendLine("<a href=\"javascript:GotoPage('s6','anchorcarlist');\" class=\"down-arrow\">按口碑评分</a><a href=\"javascript:GotoPage('s0','anchorcarlist');\" data-channelid=\"2.116.1292\" class=\"down-arrow\">按关注</a><a href=\"javascript:GotoPage('s" + (sortMode == "2" ? "3" : "2") + "','anchorcarlist');\" class=\"" + (sortMode == "2" ? "up-arrow current" : "down-arrow current") + "\" data-channelid=\"2.116.1293\">按价格</a>");
            }
            else if (sortMode == "5" || sortMode == "6")
            {
                htmlCode.AppendLine("<a href=\"javascript:GotoPage('s" + (sortMode == "5" ? "6" : "5") + "','anchorcarlist');\" class=\"" + (sortMode == "5" ? "up-arrow current" : "down-arrow current") + "\">按口碑评分</a><a href=\"javascript:GotoPage('s0','anchorcarlist');\" data-channelid=\"2.116.1292\" class=\"down-arrow\">按关注</a><a href=\"javascript:GotoPage('s2','anchorcarlist');\" class=\"up-arrow\" data-channelid=\"2.116.1293\">按价格</a>");
            }

            htmlCode.AppendLine("</div>");
            return htmlCode.ToString();
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
                htmlCode.Append("<div class=\"main-inner-section hot-cartype\">");
                htmlCode.Append("<div class=\"section-header header2\">");
                htmlCode.Append("    <div class=\"box\">");
				htmlCode.Append("        <h2><a href=\"http://www.bitauto.com/xinche/\" target=\"_blank\">热门新车</a></h2>");
                htmlCode.Append("    </div>");
                htmlCode.Append("</div>");
                //htmlCode.Append("<div class=\"line-box\">");
                //htmlCode.Append("<div class=\"title-con\">");
                //htmlCode.Append("<div class=\"title-box title-box2\">");
                //htmlCode.Append("<h4>");
                //htmlCode.Append("<a href=\"javascript:;\">热门新车</a></h4>");
                //htmlCode.Append("</div>");
                //htmlCode.Append("</div>");
                htmlCode.AppendLine("<div class=\"row block-4col-180\">");
                //htmlCode.AppendLine("<ul>");

                foreach (XmlElement serialNode in serialList)
                {
                    //htmlCode.AppendLine("<li>");
                    int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));

                    string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_3.");
                    string serialName = serialNode.GetAttribute("ShowName");
                    string shortName = serialName.Replace("(进口)", "");
                    string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();
                    string serialUrl = "http://car.bitauto.com/" + serialSpell + "/";
                    //改为指导价
                    string priceRange = new PageBase().GetSerialReferPriceByID(Convert.ToInt32(serialId));

                    //htmlCode.Append("<a href=\"" + serialUrl + "\" target=\"_blank\">");
                    //htmlCode.AppendLine("<img src=\"" + imgUrl + "\" alt=\"" + serialName + "\" /></a>");
                    //htmlCode.Append("<div class=\"title\"><a href=\"" + serialUrl + "\" target=\"_blank\" title=\"" + serialName + "\">" + shortName + "</a></div>");
                    //string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(serialId));
                    //if (priceRange.Trim().Length == 0)
                    //    htmlCode.AppendLine("<div class=\"txt huizi\">暂无报价</div>");
                    //else
                    //    htmlCode.AppendLine("<div class=\"txt\">" + priceRange + "</div>");
                    //htmlCode.AppendLine("</li>");

                    htmlCode.Append("<div class=\"col-xs-3\">");
                    htmlCode.Append("    <div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
                    htmlCode.Append("        <div class=\"img\">");
                    htmlCode.Append("            <a href=\"" + serialUrl + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" alt=\"" + serialName + "\" /></a>");
                    htmlCode.Append("        </div>");
                    htmlCode.Append("        <ul class=\"p-list\">");
                    htmlCode.Append("            <li class=\"name\"><a href=\"" + serialUrl + "\" target=\"_blank\">" + shortName + "</a></li>");
                    if (priceRange.Trim().Length == 0)
                        htmlCode.Append("            <li class=\"price\"><a href=\"" + serialUrl + "\" target=\"_blank\">暂无指导价</a></li>");
                    else
                        htmlCode.Append("            <li class=\"price\"><a href=\"" + serialUrl + "\" target=\"_blank\">" + priceRange + "</a></li>");
                    htmlCode.Append("        </ul>");
                    htmlCode.Append("    </div>");
                    htmlCode.Append("</div>");

                }
                htmlCode.AppendLine("</div>");
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
    public class SuperSerialInfo2016
    {
        private int m_pvNum;

        public SuperSerialInfo2016(int id, string showName, string spell)
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