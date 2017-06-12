using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using System.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using System.Text;
using System.Xml;
using BitAuto.CarChannel.Common.Enum;
using System.IO;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.CarchannelWeb.PageSerial
{
    public partial class CsSummaryForWaitSale : PageBase
    {
        private string baseUrl = string.Empty;
        private Car_SerialBll _serialBLL;
        private Car_BasicBll _carBLL;
        private CommonHtmlBll _commonhtmlBLL;
        //private SerialGoodsBll _serialGoodsBLL;
        private Dictionary<int, string> dictSerialBlockHtml; //静态块内容
        private Dictionary<int, string> dictUCarPrice; //二手车价格
        //private List<SerialGoodsCarEntity> serialGoodsCarList;//易车惠 商品 车型列表
        private DataSet dsCsPic; //图片列表数据源 按分类
        private Dictionary<int, SeralMarketDay> dictSerialMarketDay;


        protected int serialId;
        protected string serialSpell = string.Empty;
        protected string serialShowName = string.Empty;
        protected string serialSeoName = string.Empty;
        protected string serialName = string.Empty;
        protected string serialPrice = string.Empty;
        protected string serialTransmission = string.Empty; //变速箱
        //protected string serialDisplacement = string.Empty;//子品牌 全部排量
        //protected string serialDisplacementalt = string.Empty;//排量描述
        protected string serialSaleDisplacement = string.Empty; //在销车款 排量
        protected string serialSaleDisplacementalt = string.Empty; //在销车款 排量描述
        protected string serialUCarPrice = string.Empty; //二手车价格区间
        protected string baaUrl = string.Empty;
        protected string sanbaoLink = string.Empty;
        protected string serialWhiteImageUrl = string.Empty; //白底封面图
        protected string serialMarketDay = string.Empty; //子品牌上市时间
        protected EnumCollection.SerialInfoCard serialInfo; //子品牌名片
        protected SerialEntity serialEntity; //子品牌信息
        protected bool isExistCarList = true; //是否存在车型
        protected List<int> h5SerialList = null; //第四极 子品牌列表
        protected string h5SerialUrl = string.Empty;

        protected string serialHeaderHtml = string.Empty; //通用导航
        protected string serialHeaderJs = string.Empty; //导航脚本
        protected string focusImagesHtml = string.Empty; //焦点图片
        protected string titleAndCNCAPHtml = string.Empty; //cncap
        protected string newsTagsHtml = string.Empty; //焦点区 新闻标签
        protected string focusNewsHtml = string.Empty; //焦点新闻
        protected string carPingceHtml = string.Empty; //车型详解
        protected string bbsNewsHtml = string.Empty; //论坛新闻
        protected string bbsMoreHtml = string.Empty; //论坛更多链接
        //protected string carBBSHtml = string.Empty;//车型论坛信息
        //protected string bssLinkBarHtml = string.Empty;//车型论坛彩虹条添加链接
        protected string photoListHtml = string.Empty; //图片列表
        //protected string reallyColorHtml = string.Empty;//在产车型颜色实拍图片链接
        protected string carListTableHtml = string.Empty; //车型列表
        protected string videosHtml = string.Empty; //视频块
        protected string awardHtml = string.Empty; //奖项块
        protected string koubeiImpressionHtml = string.Empty; //口碑印象
        protected string serialToSeeJson = string.Empty;//看过还看json
        protected string hotSerialCompareHtml = string.Empty; //大家和谁比
        protected string brandOtherSerial = string.Empty; //其他车型
        protected string koubeiDianpingHtml = string.Empty; //口碑点评
        protected string editorCommentHtml = string.Empty; //编辑点评
        protected string hexinReportHtml = string.Empty; //核心看点
        //protected string suvReportHtml = string.Empty;//SUV 参数数据
        protected string askHtml = string.Empty; //答疑
        protected string nextSeePingceHtml = string.Empty;
        protected string nextSeeXinwenHtml = string.Empty;
        protected string nextSeeDaogouHtml = string.Empty;
        protected string waitFocusNewsHtml = string.Empty;
        // 车贷广告地址
        protected string chedaiADLink = string.Empty;

        public CsSummaryForWaitSale()
        {
            _serialBLL = new Car_SerialBll();
            _carBLL = new Car_BasicBll();
            _commonhtmlBLL = new CommonHtmlBll();
            //_serialGoodsBLL = new SerialGoodsBll();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(10);
            GetParamter();
            InitData();

            base.MakeSerialTopADCode(serialId);

            MakeFocusImages(); //焦点图
            GetSerialInfo(); //子品牌信息
            //MakePingceHtml();//车型详解
            //MakeTitleAndCNCAPHtml();//p碰撞已经新闻标签
            //MakeNewsTagsHtml();//焦点区 新闻标签
            //MakeFocusNewsHtml();//焦点新闻
            MakeWaitFocusNewsHtml(); //待销子品牌焦点新闻
            MakeBBSNewsHtml(); //论坛新闻
            MakeCarListHtmlNew(); //车型列表
            MakePhotoListHtml(); //图片列表
            MakeHexinReportHtml(); //关键报告
            //MakeSUVReportHtml();//SUV参数数据
            MakeEditorCommentHtml(); //编辑评论
            MakeKoubeiDianpingHtml(); //口碑点评
            MakeVideoBlockHtml(); //视频
            MakeKoubeiImpressionHtml(); //口碑印象
            MakeAwardHtml();
            MakeSerialToSerialHtml(); //看过还看
            MakeHotSerialCompare(); //大家和谁比
            MakeBrandOtherSerial(); //其他品牌
            MakAskHtml(); //答疑
            InitNextSeeNew();
        }

        private void MakeAwardHtml()
        {
            int carAward = (int) CommonHtmlEnum.BlockIdEnum.CarAward;
            if (dictSerialBlockHtml.ContainsKey(carAward))
                awardHtml = dictSerialBlockHtml[carAward];
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            //子品牌信息
            serialInfo = _serialBLL.GetSerialInfoCard(serialId);
            if (serialInfo.CsID == 0)
            {
                Response.Redirect("/car/404error.aspx?info=无子品牌");
            }
            serialEntity = (SerialEntity) DataManager.GetDataEntity(EntityType.Serial, serialId);
            serialSpell = serialEntity.AllSpell;
            serialShowName = serialEntity.ShowName;
            serialSeoName = serialEntity.SeoName;
            serialName = serialEntity.Name;
            serialPrice = serialInfo.CsPriceRange;
            if (serialPrice.Length == 0)
                serialPrice = "暂无报价";
            baseUrl = "/" + serialSpell.ToLower() + "/";
            dictUCarPrice = _serialBLL.GetUCarSerialPrice();
            //通用导航头
            serialHeaderHtml = base.GetCommonNavigation("CsSummary", serialId);
            //导航脚本
            serialHeaderJs = base.GetCommonNavigation("CsSummaryJs", serialId);
            //图库接口本地化更改 by sk 2012.12.21
            string xmlPicPath = System.IO.Path.Combine(PhotoImageConfig.SavePath,
                string.Format(PhotoImageConfig.SerialPhotoListPath, serialId));
            // 此 Cache 将通用于图片页和车型综述页
            dsCsPic = this.GetXMLDocToDataSetByURLForCache("CarChannel_SerialAllPic_" + serialId.ToString(), xmlPicPath,
                60);
            //论坛url
            baaUrl = _serialBLL.GetForumUrlBySerialId(serialId);
            //白底封面图
            serialWhiteImageUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_3.");
            //静态块内容
            dictSerialBlockHtml = _commonhtmlBLL.GetCommonHtml(serialId, CommonHtmlEnum.TypeEnum.Serial,
                CommonHtmlEnum.TagIdEnum.SerialSummary);
            ////易车惠 商品 车型列表
            //serialGoodsCarList = _serialGoodsBLL.GetGoodsCarList(serialId);
            //彩虹条 三包链接
            if (!string.IsNullOrEmpty(serialInfo.CsSanBaoLink))
                sanbaoLink = string.Format("<a href=\"{0}\" target=\"_blank\"><em>(三包)</em></a>",
                    serialInfo.CsSanBaoLink);
            chedaiADLink = "http://car.bitauto.com/" + serialSpell + "/chedai/sq/?ref=sq8";
            // 子品牌车贷广告
            Dictionary<string, List<LinkADForCs>> dicLinkAD = _serialBLL.GetLinkAD();
            if (dicLinkAD.ContainsKey("AD_CsSummaryForCheDai")
                && dicLinkAD["AD_CsSummaryForCheDai"].Count > 0)
                //	&& dicLinkAD["AD_CsSummaryForCheDai"].ListCsID.Contains(serialId)
                //	&& dicLinkAD["AD_CsSummaryForCheDai"].Link != "")
            {
                foreach (LinkADForCs lfs in dicLinkAD["AD_CsSummaryForCheDai"])
                {
                    if (lfs.ListCsID.Contains(serialId) && lfs.Link != "")
                    {
                        chedaiADLink = lfs.Link;
                        break;
                    }
                }
            }
            //子品牌上市时间
            dictSerialMarketDay = _serialBLL.GetAllWaitSaleSerialMarkDay();
            if (dictSerialMarketDay.ContainsKey(serialId))
            {
                var marketDay = dictSerialMarketDay[serialId];
                if (CommonFunction.DateDiff(marketDay.Sign, DateTime.Now, ConvertHelper.GetDateTime(marketDay.Day)) >= 0)
                {
                    serialMarketDay = marketDay.Day + "  即将上市";
                }
            }
            //第四极 子品牌列表
            // h5SerialList = new SerialFourthStageBll().GetAllSerialInH5();
            // modified by chengl 2015-11-13
            if (new SerialFourthStageBll().HasH5ByCsInfo(serialEntity.Level.Name, serialEntity.SaleState))
            {
                h5SerialUrl = string.Format("http://car.h5.yiche.com/{0}/", serialSpell);
            }
            else
            {
                h5SerialUrl = string.Format("http://car.m.yiche.com/{0}/", serialSpell);
            }
            // h5SerialUrl = (h5SerialList != null && h5SerialList.Contains(serialId)) ? string.Format("http://car.h5.yiche.com/{0}/", serialSpell) : string.Format("http://car.m.yiche.com/{0}/", serialSpell);
        }

        /// <summary>
        /// 获取页面参数
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
        /// 焦点图片块
        /// </summary>
        private void MakeFocusImages()
        {
            StringBuilder sb = new StringBuilder();

            #region 焦点图 大图 颜色

            //子品牌焦点图
            List<SerialFocusImage> imgList = _serialBLL.GetSerialFocusImageList(serialId);
            //modified by sk 停销 未上市 取最新年款下的 子品牌颜色图
            List<SerialColorEntity> serialColorList = _serialBLL.GetNoSaleSerialColors(serialId, serialInfo.ColorList);
            //List<SerialColorEntity> serialColorList = _serialBLL.GetProduceSerialColors(serialId);
            List<SerialColorForSummaryEntity> colorList = _serialBLL.GetSerialColorRGBByCsID(serialId, 0,
                serialColorList);
            //排序 有图在前 无图在后 颜色 按色值大小从大到小排序
            colorList.Sort(NodeCompare.SerialColorCompare);

            List<string> smallImages = new List<string>();
            sb.Append("<div class=\"img-box focus-img-pos\" id=\"focus_images\">");
            //if (serialId == 3288) // 焦点图冠名广告 只有 江铃驭胜 才显示  
            //{
            //    sb.Append("<div class=\"ad_300_30\">");
            //    sb.Append("	<ins id=\"div_1ae7f3d9-c409-4b06-b739-9d18aeed10db\" type=\"ad_play\" adplay_ip=\"\" adplay_areaname=\"\"");
            //    sb.Append("		adplay_cityname=\"\" adplay_brandid=\"" + serialId + "\" adplay_brandname=\"\" adplay_brandtype=\"\"");
            //    sb.Append("		adplay_blockcode=\"1ae7f3d9-c409-4b06-b739-9d18aeed10db\"></ins>");
            //    sb.Append("</div>");
            //}
            //bool IsGuanfangPic = false;//是否有官方颜色图片
            List<string> bigImage = new List<string>();
            //大图 默认第一张 焦点图第一张 没有焦点图 白底图
            int picCount = 0;
            if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("A"))
            {
                picCount = dsCsPic.Tables["A"].AsEnumerable().Sum(row => ConvertHelper.GetInteger(row["N"]));
            }
            if (imgList.Count > 0)
            {
                SerialFocusImage csImg = imgList[0];
                string firstFocusImage = csImg.ImageUrl;
                if (csImg.ImageId > 0)
                {
                    firstFocusImage = String.Format(firstFocusImage, 4);
                }

                bigImage.Insert(0,
                    string.Format(
                        "{3}<a href=\"{0}\" class=\"img-link\">共{2}张图片&gt;&gt;</a><div id=\"focus_image_first\" style=\"display:block\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\"/></a></div>",
                        csImg.TargetUrl,
                        firstFocusImage,
                        picCount,
                        serialMarketDay!=""? string.Format("<span class=\"jss-tip\">{0}</span>", serialMarketDay) :""));
            }
            else
            {
                bigImage.Insert(0,
                    string.Format(
                        "{3}<a href=\"{0}\" class=\"img-link\">共{1}张图片&gt;&gt;</a><div id=\"focus_image_first\" style=\"display:block\"><a href=\"http://photo.bitauto.com/serial/{2}/\" target=\"_blank\"><img src=\"{0}\" width=\"300\" height=\"200\" /></a></div>",
                        serialWhiteImageUrl.Replace("_3.", "_6."),
                        picCount,
                        serialId,
                        serialMarketDay != "" ? string.Format("<span class=\"jss-tip\">{0}</span>", serialMarketDay) : ""));
            }
            if (colorList.Count > 0)
            {
                var firstFocusImageId = 0;
                if (imgList != null && imgList.Count > 0) firstFocusImageId = imgList[0].ImageId;
                //颜色补充实拍图
                string serialColorPath = Path.Combine(PhotoImageConfig.SavePath,
                    string.Format(PhotoImageConfig.SerialColorImagePath, serialId));
                XmlDocument xmlSerialColor = CommonFunction.ReadXmlFromFile(serialColorPath);
                //IsGuanfangPic = colorList.Find(p => !string.IsNullOrEmpty(p.ImageUrl)) != null;
                int loop = 0;
                foreach (SerialColorForSummaryEntity color in colorList)
                {
                    string imageUrl = color.ImageUrl.Replace("_5.", "_4.");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        XmlNode colorNode =
                            xmlSerialColor.SelectSingleNode("/CarImageList/CarImage[@ColorName='" + color.ColorName +
                                                            "']");
                        if (colorNode != null)
                        {
                            imageUrl = string.Format(colorNode.Attributes["ImageUrl"].Value, 4);
                            color.Link = colorNode.Attributes["Link"].Value;
                        }
                    }
                    if (string.IsNullOrEmpty(imageUrl))
                        continue;
                    loop++;
                    bigImage.Add(
                        string.Format(
                            "<div id=\"focuscolor_{3}\" style=\"display:{2}\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"300\" height=\"200\" /></a></div>",
                            color.Link + "#" + firstFocusImageId,
                            imageUrl,
                            "none", loop));
                    smallImages.Add(
                        string.Format(
                            "<li><a href=\"{0}\" title=\"{2}\" target=\"_blank\"><span style=\"background:{1}\"></span></a></li>",
                            color.Link + "#" + firstFocusImageId, color.ColorRGB, color.ColorName));
                }
            }

            sb.Append(string.Concat(bigImage.ToArray()));
            sb.Append("</div>");
            sb.Append("<div class=\"img-cor-box\" id=\"focus_color_box\">");
            sb.Append("<div class=\"focus-color-box\">");
            if (smallImages.Count > 8)
            {
                sb.Append("    <div id=\"focus_color_l\" class=\"l-btn\"></div>");
                sb.Append("    <div id=\"focus_color_r\" class=\"r-btn a_r_hover\"></div>");
            }
            sb.Append("<div class=\"focus-color-warp\">");
            if (smallImages.Count <= 8)
                sb.AppendFormat("<ul id=\"color-listbox\" style=\"width:{0}px;\">", smallImages.Count*23);
            else
                sb.Append("<ul id=\"color-listbox\" style=\"top: 13px; left: 0px;position: absolute;\">");
            sb.Append(string.Concat(smallImages.ToArray()));
            sb.Append("    </ul>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            #endregion

            sb.Append("<div class=\"img-list\">");
            //sb.Append("	<ul>");

            #region 焦点图片 下面 前2张 小图

            for (int i = imgList.Count - 1; i >= 1; i--)
            {
                //第二张图 有图解显示图解
                if (i == 1 && true)
                {
                    //取图解第一张
                    XmlNode firstTujieNode = this.GetFirstTujieImage(dsCsPic);
                    if (firstTujieNode != null)
                    {
                        string groupName = firstTujieNode.Attributes["GroupName"].Value;
                        sb.AppendFormat(
                            "<div class=\"img-link\"><a href=\"{0}\" target=\"_blank\">{3}<img src=\"{1}\" title=\"{2}\" alt=\"{2}\" width=\"90\" height=\"60\" /></a></div>",
                            firstTujieNode.Attributes["Link"].Value,
                            firstTujieNode.Attributes["ImageUrl"].Value.Replace("_4.", "_5."),
                            firstTujieNode.Attributes["ImageName"].Value,
                            string.IsNullOrEmpty(groupName) ? string.Empty : "<i>" + groupName + "</i>");
                        continue;
                    }
                }
                SerialFocusImage csImg = imgList[i];
                string smallImgUrl = csImg.ImageUrl;
                if (csImg.ImageId > 0)
                {
                    smallImgUrl = String.Format(smallImgUrl, 5);
                }
                sb.AppendFormat(
                    "<div class=\"img-link\"><a href=\"{0}\" target=\"_blank\">{3}<img src=\"{1}\" title=\"{2}\" alt=\"{2}\" width=\"90\" height=\"60\" /></a></div>",
                    csImg.TargetUrl,
                    smallImgUrl,
                    csImg.ImageName,
                    string.IsNullOrEmpty(csImg.GroupName) ? string.Empty : "<i>" + csImg.GroupName + "</i>");
            }

            #endregion

            #region 最后一张图 视频 及补图逻辑

            //List<NewsForSerialSummaryEntity> videoList = new CarNewsBll().GetSerialNewsByCategoryId(serialId, 348, 1);
            //modified by sk 2014.01.13 按“易车体验”—->“原创节目-新车解析”-->“新车视频-新车解析”的优先级顺序提取视频
            List<int> categoryIdList = new List<int>() {30, 100, 101};
            List<VideoEntity> videoList = VideoBll.GetVideoBySerialIdAndCategoryId(serialId, categoryIdList, 1);
            if (videoList.Count > 0)
            {
                string imgUrl = videoList[0].ImageLink;
                imgUrl = imgUrl.Replace("/bitauto/", "/newsimg-90-w0-1-q50/bitauto/");
                imgUrl = imgUrl.Replace("/Video/", "/newsimg-90-w0-1-q50/Video/");
                sb.AppendFormat(
                    "<div class=\"img-link img-current\"><a href=\"{0}\" target=\"_blank\"><i>视频</i><em></em><img src=\"{1}\" title=\"{2}\" alt=\"{2}\" width=\"90\" height=\"60\" /></a></div>",
                    videoList[0].ShowPlayUrl, imgUrl, videoList[0].ShortTitle);
            }
            else
            {
                #region 补空间第一张

                if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("A"))
                {
                    if (dsCsPic.Tables.Contains("C") && dsCsPic.Tables["C"].Rows.Count > 0)
                    {
                        //取空间图片第一张图片
                        int speceImageCount = 0;
                        foreach (DataRow row in dsCsPic.Tables["C"].Select("P='8'")) //空间
                        {
                            int imgId = row["I"] == DBNull.Value ? 0 : Convert.ToInt32(row["I"]);
                            string imgUrl = row["U"] == DBNull.Value ? "" : Convert.ToString(row["U"]);
                            if (imgId == 0 || imgUrl.Length == 0)
                                continue;
                            speceImageCount++;
                            imgUrl = CommonFunction.GetPublishHashImgUrl(5, imgUrl, imgId);
                            string picName = Convert.ToString(row["D"]);
                            string picUlr = "http://photo.bitauto.com/picture/" + serialId + "/" + imgId + "/";
                            sb.AppendFormat(
                                "<div class=\"img-link img-current\"><a href=\"{0}\" target=\"_blank\"><i>{2}</i><img src=\"{1}\" title=\"{2}\" alt=\"{2}\" width=\"90\" height=\"60\" /></a></div>",
                                picUlr, imgUrl, "空间");
                            break;
                        }
                        //空间第一张图片没有，用其他图片补一张且与焦点图片不重复
                        if (speceImageCount <= 0)
                        {
                            foreach (DataRow row in dsCsPic.Tables["C"].Rows) //空间
                            {
                                int imgId = row["I"] == DBNull.Value ? 0 : Convert.ToInt32(row["I"]);
                                string imgUrl = row["U"] == DBNull.Value ? "" : Convert.ToString(row["U"]);
                                if (imgId == 0 || imgUrl.Length == 0)
                                    continue;
                                //焦点图片排重
                                SerialFocusImage focusImage = imgList.Find(p => p.ImageId == imgId);
                                if (focusImage != null)
                                    continue;
                                imgUrl = CommonFunction.GetPublishHashImgUrl(5, imgUrl, imgId);

                                string picName = Convert.ToString(row["D"]);
                                string picUlr = "http://photo.bitauto.com/picture/" + serialId + "/" + imgId + "/";
                                sb.AppendFormat(
                                    "<div class=\"img-link img-current\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" title=\"{2}\" alt=\"{2}\" width=\"90\" height=\"60\" /></a></div>",
                                    picUlr, imgUrl, serialShowName + "图片");
                                break;
                            }
                        }
                    }
                }

                #endregion
            }

            #endregion

            //sb.Append("	</ul>");
            sb.Append("</div>");
            focusImagesHtml = sb.ToString();
        }

        /// <summary>
        /// 车型详解
        /// </summary>
        protected void MakePingceHtml()
        {
            int pingce = (int) CommonHtmlEnum.BlockIdEnum.Pingce;
            if (dictSerialBlockHtml.ContainsKey(pingce))
                carPingceHtml = dictSerialBlockHtml[pingce];
        }

        /// <summary>
        /// 碰撞及新闻标签
        /// </summary>
        private void MakeTitleAndCNCAPHtml()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<h3><a href=\"/{1}/wenzhang/\" target=\"_blank\">{0}最新文章</a></h3>", serialSeoName,
                serialSpell);

            var cncap = _serialBLL.GetCNCAPData();
            if (cncap.ContainsKey(serialId))
            {
                string starStr = string.Empty;
                //IIHS总评价 显示不同
                if (cncap[serialId].ParamId == 957)
                {
                    starStr = cncap[serialId].ParamValue;
                }
                else
                {
                    int levelNum = 0;
                    switch (cncap[serialId].ParamValue)
                    {
                        case "一星":
                            levelNum = 1;
                            break;
                        case "二星":
                            levelNum = 2;
                            break;
                        case "三星":
                            levelNum = 3;
                            break;
                        case "四星":
                            levelNum = 4;
                            break;
                        case "五星":
                            levelNum = 5;
                            break;
                    }
                    starStr = string.Format("<strong><i class=\"star_bg star_{0}\">3星</i></strong>", levelNum);
                }
                Dictionary<string, Dictionary<int, int>> newsNumber = AutoStorageService.GetCacheTreeSerialNewsCount();
                if (newsNumber != null && newsNumber.ContainsKey("anquan") && newsNumber["anquan"].ContainsKey(serialId) &&
                    newsNumber["anquan"][serialId] > 0)
                {
                    sb.AppendFormat(
                        " <span class=\"cncap\"><a href=\"/{1}/anquan/\" target=\"_blank\"><b>{2}：</b>{0}</a></span>",
                        starStr, serialSpell, cncap[serialId].Name);
                }
                else
                    sb.AppendFormat(" <span class=\"cncap\"><b>{1}：</b>{0}</span>", starStr, cncap[serialId].Name);
            }
            titleAndCNCAPHtml = sb.ToString();
        }

        /// <summary>
        /// 焦点区 新闻标签
        /// </summary>
        private void MakeNewsTagsHtml()
        {
            List<int> koubeiReportList = _serialBLL.GetAllSerialKouBeiReport();
            StringBuilder sb = new StringBuilder();
            List<string> list = new List<string>();
            sb.AppendFormat("<div class=\"more\">");
            list.Add(string.Format("<a href=\"/{0}/xinwen/\" target=\"_blank\">新闻</a>", serialSpell));
            list.Add(string.Format("<a href=\"/{0}/shijia/\" target=\"_blank\">试驾</a>", serialSpell));
            list.Add(string.Format("<a href=\"/{0}/daogou/\" target=\"_blank\">导购</a>", serialSpell));
            if (koubeiReportList.Contains(serialId))
                list.Add(string.Format("<a href=\"/{0}/koubei/baogao/\" target=\"_blank\">口碑报告</a>", serialSpell));
            list.Add("<a href=\"http://www.bitauto.com/zhuanti/daogou/gsqgl/\" target=\"_blank\">购车流程</a>");
            if (new ProduceAndSellDataBll().HasSerialData(serialId))
                list.Add(string.Format("<a href=\"/{0}/xiaoliang/\" target=\"_blank\">销量</a>", serialSpell));
            sb.Append(string.Join(" | ", list.ToArray()));
            sb.Append("</div>");
            newsTagsHtml = sb.ToString();
        }

        /// <summary>
        /// 焦点新闻
        /// </summary>
        private void MakeFocusNewsHtml()
        {
            int focus = (int) CommonHtmlEnum.BlockIdEnum.FocusNews;
            if (dictSerialBlockHtml.ContainsKey(focus))
                focusNewsHtml = dictSerialBlockHtml[focus];
        }

        /// <summary>
        /// 焦点新闻
        /// </summary>
        private void MakeWaitFocusNewsHtml()
        {
            int focus = (int) CommonHtmlEnum.BlockIdEnum.FocusNewsForWaitSale;
            if (dictSerialBlockHtml.ContainsKey(focus))
                waitFocusNewsHtml = dictSerialBlockHtml[focus];
        }

        /// <summary>
        /// 车型论坛、论坛新闻
        /// </summary>
        private void MakeBBSNewsHtml()
        {
            //获取数据
            XmlDocument xmlDoc = _serialBLL.GetSerialForumNews(serialId);
            XmlNodeList newsList = xmlDoc.SelectNodes("/root/Forum/ForumSubject");

            if (baaUrl == "http://baa.bitauto.com/" || newsList.Count <= 0)
            {
                //子品牌综述页论坛补充贴(当1条论坛数据都没有)
                string includeFilePath = Server.MapPath("~/include/BAA/BBS/00001/201401_xbzsy_baa_Manual.shtml");
                if (File.Exists(includeFilePath))
                {
                    bbsNewsHtml = File.ReadAllText(includeFilePath);
                }
                return;
            }

            StringBuilder sbBBSNews = new StringBuilder();
            StringBuilder sbCarBBSInfo = new StringBuilder();
            StringBuilder sbBBSLinkBar = new StringBuilder();
            StringBuilder sbBBSMoreHtml = new StringBuilder();

            sbBBSNews.Append("<div class=\"news_list1 news_list3\">");
            sbBBSNews.Append("    <ul class=\"list_date\">");
            if (newsList.Count > 0)
            {
                int loop = 0;
                foreach (XmlNode newsNode in newsList)
                {
                    int classId = ConvertHelper.GetInteger(newsNode.SelectSingleNode("digest").InnerText);
                    string className = Enum.GetName(typeof (EnumCollection.ForumDigest), classId);
                    string newsTitle = newsNode.SelectSingleNode("title").InnerText.Trim();
                    //过滤Html标签
                    newsTitle = StringHelper.RemoveHtmlTag(newsTitle);

                    string tid = newsNode.SelectSingleNode("tid").InnerText;
                    string filePath = newsNode.SelectSingleNode("url").InnerText;
                    string pubTime = string.Empty;
                    // modified by chengl Jun.15.2012
                    if (newsNode.SelectSingleNode("postdatetime") != null)
                    {
                        pubTime = newsNode.SelectSingleNode("postdatetime").InnerText;
                        pubTime = Convert.ToDateTime(pubTime).ToString("MM-dd");
                    }

                    loop++;
                    if (loop <= 2)
                    {
                        sbCarBBSInfo.AppendFormat("<div class=\"{0}\">",
                            loop == 2 ? "news_outerbox news_maiche" : "news_outerbox news_innerbox_border news_tiche");
                        sbCarBBSInfo.AppendFormat(
                            "<h2><span><a href=\"{2}\" target=\"_blank\">{3}</a></span><em><a href=\"{0}\" target=\"_blank\">{1}</a></em></h2>",
                            filePath, newsTitle,
                            baaUrl + "index-" + classId + "-all-1-0.html",
                            className);
                        sbCarBBSInfo.Append("</div>");
                        continue;
                    }
                    if (loop > 5) break;
                    sbBBSNews.AppendFormat(
                        "<li><div><span><a  href=\"{0}\" class=\"fl\" target=\"_blank\">{1}</a>| </span><a href=\"{2}\" title=\"{3}\" target=\"_blank\">{4}</a></div><small>{5}</small></li>",
                        baaUrl + "index-" + classId + "-all-1-0.html",
                        className,
                        filePath,
                        newsTitle,
                        StringHelper.GetRealLength(newsTitle) > 30
                            ? StringHelper.SubString(newsTitle, 30, true)
                            : newsTitle,
                        pubTime);
                }
            }
            else
            {
                sbBBSNews.Append("<li><span>暂无内容</span></li>");
            }
            sbBBSNews.Append("    </ul>");
            sbBBSNews.Append("</div>");
            //bbsNewsHtml = sbBBSNews.ToString();
            //carBBSHtml = sbCarBBSInfo.ToString();

            //车型论坛
            DataSet ds = base.GetBBSLinkBySerialId(serialId);

            if (ds != null && ds.Tables[0] != null)
            {
                int index = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    index++;
                    if (index > 2) break;
                    sbBBSLinkBar.AppendFormat("<a href=\"{0}\" target=\"_blank\">{1}</a> |", dr["url"].ToString(),
                        dr["title"].ToString());
                }
            }
            //bssLinkBarHtml = sbBBSLinkBar.ToString();
            StringBuilder sbBBSNewsHtml = new StringBuilder();
            //sbBBSNewsHtml.Append("<div class=\"line_box news_box\">");
            //sbBBSNewsHtml.AppendFormat("<h3><span><b>{0}论坛</b></span></h3>", serialSeoName);
            sbBBSNewsHtml.AppendFormat("<div class=\"col-sub\">{0}</div>", sbCarBBSInfo.ToString());
            sbBBSNewsHtml.AppendFormat("<div class=\"col-main\">{0}</div>", sbBBSNews.ToString());
            //sbBBSNewsHtml.Append("<div class=\"clear\"></div>");


            sbBBSMoreHtml.Append("<div class=\"more\">");
            sbBBSMoreHtml.AppendFormat(
                "<a href=\"{0}index-1-all-1-0.html\" target=\"_blank\">提车作业</a> | <a href=\"{0}index-2-all-1-0.html\" target=\"_blank\">用车养护</a> | <a href=\"{0}index-3-all-1-0.html\" target=\"_blank\">装饰改装</a> | <a href=\"{0}index-0-all-1-0.html\" target=\"_blank\">精华帖子</a> | {1} <a href=\"{0}\" target=\"_blank\">进入论坛&gt;&gt;</a>",
                baaUrl, sbBBSLinkBar.ToString());
            sbBBSMoreHtml.Append("</div>");

            bbsMoreHtml = sbBBSMoreHtml.ToString();
            //sbBBSNewsHtml.Append("</div>");
            bbsNewsHtml = sbBBSNewsHtml.ToString();
        }

        /// <summary>
        /// 图片列表
        /// </summary>
        private void MakePhotoListHtml()
        {
            StringBuilder sb = new StringBuilder();
            int picNum = 0;
            List<string> classList = new List<string>();
            XmlNode firstTujieNode = null;
            if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("A"))
            {
                int cateId = 0; //分类ID
                if (dsCsPic.Tables.Contains("A"))
                {
                    Dictionary<int, string> dict = new Dictionary<int, string>
                    {
                        {6, "外观"},
                        {7, "内饰"},
                        {8, "空间"},
                        {12, "图解"},
                        {11, "官方图"}
                    };

                    foreach (DataRow row in dsCsPic.Tables["A"].Rows)
                    {
                        int cateNum = Convert.ToInt32(row["N"]);
                        picNum += cateNum;
                        cateId = Convert.ToInt32(row["G"]);
                        if (cateId != 6 && cateId != 7 && cateId != 8 && cateId != 12 && cateId != 11)
                            continue;
                        classList.Add(
                            string.Format(
                                "<a href=\"http://photo.bitauto.com/serialmore/{0}/{1}/\" target=\"_blank\">{2}</a>",
                                serialId, cateId, dict.FirstOrDefault(d => d.Key == cateId).Value));
                    }
                    //取图解第一张
                    firstTujieNode = this.GetFirstTujieImage(dsCsPic);
                }
                string allPicUrl = baseUrl + "tupian/";
            }
            if (picNum <= 0) return;
            //11张图片
            List<XmlNode> srcElevenImageList = _serialBLL.GetSerialElevenPositionImage(serialId);
            //add by sk 2013.11.25 有图解 第二张 插入到第六张图位置
            if (firstTujieNode != null)
            {
                //modified by sk 2014.03.11 候姐 图解与11张图排重 重复显示11张图
                var tujieExist =
                    srcElevenImageList.Find(
                        p => p.Attributes["ImageId"].Value == firstTujieNode.Attributes["ImageId"].Value);
                if (tujieExist == null)
                {
                    if (srcElevenImageList.Count >= 2)
                    {
                        var secondImage = srcElevenImageList[1];
                        srcElevenImageList.RemoveAt(1);
                        srcElevenImageList.Insert(1, firstTujieNode);
                        if (srcElevenImageList.Count >= 6)
                        {
                            srcElevenImageList.Insert(5, secondImage);
                        }
                        else
                        {
                            srcElevenImageList.Add(secondImage);
                        }
                    }
                    else
                    {
                        srcElevenImageList.Add(firstTujieNode);
                    }
                }
            }
            var elevenImageList = srcElevenImageList.Take(11);
            sb.Append("<div class=\"line_box\" id=\"car-pic-photolist\">");

            sb.Append("<div class=\"title-box\">");
            sb.AppendFormat("<h3><a href=\"http://photo.bitauto.com/serial/{1}/\" target=\"_blank\">{0}图片</a></h3>",
                serialSeoName, serialId);
            sb.AppendFormat("<span>共有实拍图片{0}张</span>", picNum);
            sb.Append("	<div class=\"more\">");
            sb.AppendFormat("{0} | <a href=\"http://photo.bitauto.com/serial/{1}/\" target=\"_blank\">更多&gt;&gt;</a>",
                string.Join(" | ", classList.ToArray()), serialId);
            sb.Append("</div></div>");

            sb.Append("	<div class=\"car-pic20130802\">");
            if (srcElevenImageList.Count < 10)
            {
                sb.Append("<ul class=\"less-pic\">");
                foreach (XmlNode node in elevenImageList)
                {
                    sb.AppendFormat(
                        "<li><a href=\"{0}\" target=\"_blank\"><img data-original=\"{1}\" title=\"{2}\" alt=\"{2}\"></a></li>",
                        node.Attributes["Link"].Value,
                        string.Format(node.Attributes["ImageUrl"].Value, 1),
                        node.Attributes["ImageName"].Value);
                }
                sb.Append("</ul>");
                sb.Append("<div class=\"clear\"></div>");
            }
            else
            {
                sb.Append("		<div class=\"car-pic-area\">");
                sb.Append("			<div class=\"big-pic\">");
                int topLoop = 0;
                foreach (XmlNode node in elevenImageList.Take(2))
                {
                    topLoop++;
                    string[] categoryNameArray = {"外观", "内饰", "空间", "图解"};
                    string categoryName = node.Attributes["GroupName"] != null
                        ? node.Attributes["GroupName"].Value
                        : string.Empty;
                    sb.AppendFormat(
                        "<a href=\"{0}\" target=\"_blank\" class=\"{4}\"><img data-original=\"{1}\" title=\"{2}\" alt=\"{2}\">{3}<span></span></a>",
                        node.Attributes["Link"].Value,
                        string.Format(node.Attributes["ImageUrl"].Value, 4),
                        node.Attributes["ImageName"].Value,
                        categoryNameArray.Contains(categoryName) ? "<em>" + categoryName + "</em>" : string.Empty,
                        topLoop == 2 ? "last" : "");
                }
                sb.Append("			</div>");
                sb.Append("			<div class=\"small-pic\">");
                int loop = 0;
                foreach (XmlNode node in elevenImageList.Skip(2))
                {
                    loop++;
                    if (elevenImageList.Count() >= 11 && loop == 9)
                    {
                        sb.AppendFormat(
                            "<a href=\"http://photo.bitauto.com/serial/{0}/\" target=\"_blank\"><img data-original=\"{1}\" title=\"{2}\" alt=\"{2}\"><em></em><dl><dt>+{3}</dt><dd>查看更多实拍图&gt;&gt;</dd></dl></a>",
                            serialId,
                            string.Format(node.Attributes["ImageUrl"].Value, 1),
                            node.Attributes["ImageName"].Value,
                            picNum);
                    }
                    else
                        sb.AppendFormat(
                            "<a href=\"{0}\" target=\"_blank\"><img data-original=\"{1}\" title=\"{2}\" alt=\"{2}\"><span></span></a>",
                            node.Attributes["Link"].Value,
                            string.Format(node.Attributes["ImageUrl"].Value, 1),
                            node.Attributes["ImageName"].Value);
                }
                sb.Append("			</div>");
                sb.Append("		</div>");
            }
            sb.Append(GetReallyColorListHtml());
            sb.Append("	</div>");
            sb.Append("	<div class=\"clear\"></div>");

            sb.Append("</div>");
            photoListHtml = sb.ToString();
        }

        /// <summary>
        /// 取第一张图解
        /// </summary>
        /// <param name="dsCsPic"></param>
        private XmlNode GetFirstTujieImage(DataSet dsCsPic)
        {
            XmlElement element = null;
            XmlDocument xmlDoc = new XmlDocument();
            //取图解第一张
            if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("C") &&
                dsCsPic.Tables["C"].Rows.Count > 0)
            {
                var rows = dsCsPic.Tables["C"].Rows.Cast<DataRow>();
                DataRow row = rows.FirstOrDefault(dr => ConvertHelper.GetInteger(dr["P"]) == 12);
                    //dt.Select("P='" + cateId + "'");
                if (row != null)
                {
                    int imgId = row["I"] == DBNull.Value ? 0 : Convert.ToInt32(row["I"]);
                    string imgUrl = row["U"] == DBNull.Value ? "" : Convert.ToString(row["U"]);
                    if (imgId == 0 || imgUrl.Length == 0)
                        imgUrl = WebConfig.DefaultCarPic;
                    else
                        imgUrl = CommonFunction.GetPublishHashImgUrl(4, imgUrl, imgId);
                    string picUrl = "http://photo.bitauto.com/picture/" + serialId + "/" + imgId + "/";
                    element = xmlDoc.CreateElement("CarImage");
                    element.SetAttribute("ImageId", imgId.ToString());
                    element.SetAttribute("ImageUrl", imgUrl);
                    element.SetAttribute("GroupName", "图解");
                    element.SetAttribute("ImageName", "图解");
                    element.SetAttribute("Link", picUrl);
                }
            }
            return (XmlNode) element;
        }

        /// <summary>
        /// 在产车型颜色 实拍 
        /// </summary>
        private string GetReallyColorListHtml()
        {
            StringBuilder sb = new StringBuilder();
            string reallyColorPath = Path.Combine(PhotoImageConfig.SavePath,
                string.Format(PhotoImageConfig.SerialReallyColorImagePath, serialId));
            XmlDocument fillImageXml = CommonFunction.ReadXmlFromFile(reallyColorPath);
            List<SerialColorEntity> serialColorList = _serialBLL.GetProduceSerialColors(serialId);
            if (serialColorList.Count > 0)
            {
                sb.Append("<div class=\"car-pic-color\">");
                sb.Append("	<div class=\"car-pic-color-left\">");
                sb.Append("<strong>看实拍：</strong>");
                sb.Append("<ul>");
                int loop = 0;
                foreach (SerialColorEntity color in serialColorList)
                {
                    string link = string.Empty;
                    if (fillImageXml != null)
                    {
                        XmlNode node = fillImageXml.SelectSingleNode("//Color[@Name='" + color.ColorName + "']");
                        if (node != null)
                            link = node.Attributes["Link"].Value;
                    }
                    if (string.IsNullOrEmpty(link)) continue;
                    loop++;
                    if (loop > 25)
                    {
                        loop++;
                        break;
                    }
                    sb.AppendFormat(
                        "<li><a href=\"{0}\" target=\"_blank\" title=\"{2}\"><span style=\"background:{1}\">{2}</span></a></li>",
                        link, color.ColorRGB, color.ColorName);
                }
                sb.Append("</ul>");
                if (loop > 25)
                    sb.AppendFormat(
                        "<a href=\"http://photo.bitauto.com/serial/{0}/\" target=\"_blank\" class=\"more-color\">更多&gt;&gt;</a>",
                        serialId);
                sb.Append("	</div>");
                sb.Append("</div>");
                if (loop == 0)
                {
                    return string.Empty;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 中间块 子品牌信息
        /// </summary>
        private void GetSerialInfo()
        {
            #region 排量 modified by 2014.03.31 废除子品牌排量 采用在销车款排量

            //if (serialEntity.ExhaustList != null && serialEntity.ExhaustList.Length > 0)
            //{
            //    if (serialEntity.ExhaustList.Length > 3)
            //    {
            //        serialDisplacement = string.Concat(serialEntity.ExhaustList[0]
            //            , "..."
            //            , serialEntity.ExhaustList[serialEntity.ExhaustList.Length - 1]);
            //    }
            //    else
            //    {
            //        serialDisplacement = serialEntity.Exhaust;
            //    }
            //    serialDisplacementalt = string.Join(" ", serialEntity.ExhaustList);
            //}

            #endregion

            //变速箱
            serialTransmission = string.Join("<label>/</label>", serialInfo.CsTransmissionType.Split('、'));

            //二手车价格区间
            if (dictUCarPrice.ContainsKey(serialId))
                serialUCarPrice = dictUCarPrice[serialId];
        }

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

            List<CarInfoForSerialSummaryEntity> carinfoList = _carBLL.GetCarInfoForSerialSummaryBySerialId(serialId);
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
            List<CarInfoForSerialSummaryEntity> carinfoWaitSaleList = carinfoList
                .FindAll(p => p.SaleState == "待销");

            if (carinfoSaleList.Count <= 0 && carinfoWaitSaleList.Count <= 0)
                isExistCarList = false;
            //add by 2014.03.18 在销车款 排量输出
            var exhaustList = carinfoSaleList.Where(p => p.Engine_Exhaust.EndsWith("L"))
                .Select(p => p.Engine_InhaleType == "增压" ? p.Engine_Exhaust.Replace("L", "T") : p.Engine_Exhaust)
                .GroupBy(p => p)
                .Select(group => group.Key).ToList();
            if (exhaustList.Count > 0)
            {
                exhaustList.Sort(NodeCompare.ExhaustCompareNew);
                if (exhaustList.Count > 3)
                {
                    serialSaleDisplacement = string.Concat(exhaustList[0]
                        , "..."
                        , exhaustList[exhaustList.Count - 1]);
                }
                else
                    serialSaleDisplacement = string.Join(" ", exhaustList.ToArray());
                serialSaleDisplacementalt = string.Join(" ", exhaustList.ToArray());
            }

            carinfoSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
            carinfoWaitSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);

            noSaleYearList.Sort(NodeCompare.CompareStringDesc);

            //sb.Append("<div class=\"line_box\" id=\"car_list\" style=\"z-index:999;\">");
            sb.Append("<div class=\"title-con\"><div class=\"title-box\">");
            sb.AppendFormat("<h3>{0}车款</h3>", serialSeoName);
            sb.Append("<ul class=\"title-tab\" id=\"data_tab_jq5\">");
            bool isWaitSale = false;
            if (carinfoWaitSaleList.Count > 0)
            {
                isWaitSale = true;
                sb.AppendFormat("<li class=\"{0}\"><a href=\"javascript:;\">未上市</a>{1}</li>",
                    carinfoSaleList.Count <= 0 ? "current" : "",
                    (carinfoSaleList.Count > 0 || noSaleYearList.Count > 0) ? "<em>|</em>" : "");
            }
            if (carinfoSaleList.Count > 0)
            {
                sb.AppendFormat("<li class=\"current\"><a href=\"javascript:;\">在售</a>{0}</li>",
                    noSaleYearList.Count > 0 ? "<em>|</em>" : "");
            }
            if (noSaleYearList.Count > 0)
            {
                sb.Append("<li class=\"bt-hover\" id=\"car_nosaleyearlist\">");
                sb.Append("<a href=\"javascript:;\" class=\"pop\">停售车款<strong></strong></a>");
                sb.Append(
                    "<div id=\"carlist_nosaleyear\" class=\"title-popbox title-popbox-model\" style=\"display: none;\">");
                sb.Append("<ul>");
                for (int i = 0; i < noSaleYearList.Count; i++)
                {
                    string url = string.Format("/{0}/{1}/#car_list", serialSpell, noSaleYearList[i].Replace("款", ""));
                    sb.AppendFormat("<li><a href=\"{0}\" target=\"_blank\">{1}</a></li>", url, noSaleYearList[i]);
                }
                sb.Append("</ul>");
                sb.Append("</div>");
                sb.Append("</li>");
            }
            // modified by chengl Oct.15.2013
            // sb.Append(" <span class=\"h_text\"><a target=\"_blank\" href=\"http://app.yiche.com/qichehui/\">下载易车客户端，体验信息最全的汽车应用！</a></span>");
            sb.Append("</ul>");
            sb.AppendFormat(
                "<div class=\"more\"><a href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">我要预约试驾&gt;&gt;</a></div>",
                serialId);
            sb.Append("</div></div>");
            if (isWaitSale)
            {
                sb.AppendFormat("    <div class=\"c-list-2014\" style=\"display: {0};\" id=\"data_tab_jq5_0\">",
                    (carinfoSaleList.Count > 0) ? "none" : "block");
                sb.Append(
                    "        <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" id=\"compare_wait\">");
                sb.Append("            <tbody>");
                sb.Append(GetCarListHtml(carinfoWaitSaleList, maxPv));
                sb.Append("            </tbody>");
                sb.Append("        </table>");
                sb.Append("    </div>");
            }
            if (carinfoSaleList.Count > 0)
            {
                sb.AppendFormat("    <div class=\"c-list-2014\" id=\"data_tab_jq5_{0}\" style=\"display: block;\">",
                    isWaitSale ? 1 : 0);
                sb.Append(
                    "        <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" id=\"compare_sale\">");
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
            //sb.Append("    <div class=\"more\">");
            //sb.AppendFormat("<a href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">我要预约试驾&gt;&gt;</a>", serialId);
            //sb.Append("    </div>");
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
            var waitStateFlag = false;
            var waitStateList = carList.Find(p => p.SaleState == "待销");
            if (waitStateList != null)
            {
                waitStateFlag = true;
            }

            List<string> carListHtml = new List<string>();
            //if (carList.Count == 0)
            //    carListHtml.Add("<tr>暂无车款！</tr>");
            var querySale =
                carList.GroupBy(
                    p =>
                        new
                        {
                            p.Engine_Exhaust,
                            p.Engine_InhaleType,
                            p.Engine_AddPressType,
                            p.Engine_MaxPower,
                            p.Electric_Peakpower
                        }, p => p);
            int groupIndex = 0;
            foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in querySale)
            {
                var key = CommonFunction.Cast(info.Key,
                    new
                    {
                        Engine_Exhaust = "",
                        Engine_InhaleType = "",
                        Engine_AddPressType = "",
                        Engine_MaxPower = 0,
                        Electric_Peakpower = 0
                    });
                string strMaxPowerAndInhaleType = string.Empty;
                string maxPower = key.Engine_MaxPower == 9999 ? "" : key.Engine_MaxPower + "kW";
                string inhaleType = key.Engine_InhaleType;
                if (!string.IsNullOrEmpty(maxPower) || !string.IsNullOrEmpty(inhaleType))
                {
                    if (inhaleType == "增压")
                    {
                        inhaleType = string.IsNullOrEmpty(key.Engine_AddPressType)
                            ? inhaleType
                            : key.Engine_AddPressType;
                    }
                    if (key.Electric_Peakpower > 0)
                    {
                        maxPower = string.Format("发动机：{0}，发电机：{1}", maxPower, key.Electric_Peakpower + "kW");
                    }
                    strMaxPowerAndInhaleType = string.Format("<b>/</b>{0}{1}", maxPower, " " + inhaleType);
                }

                if (groupIndex == 0)
                {
                    carListHtml.Add("<tr class=\"\">");
                    carListHtml.Add("    <th width=\"44%\" class=\"first-item\">");
                    carListHtml.Add(string.Format("<div class=\"pdL10\"><strong>{0}</strong> {1}</div>",
                        key.Engine_Exhaust,
                        strMaxPowerAndInhaleType));
                    carListHtml.Add("    </th>");
                    carListHtml.Add("    <th width=\"8%\" class=\"pd-left-one\">关注度</th>");
                    carListHtml.Add("    <th width=\"10%\" class=\"pd-left-one\">变速箱</th>");
                    carListHtml.Add("    <th width=\"10%\" class=\"pd-left-two\">" + (waitStateFlag ? "预售价" : "指导价") +
                                    "</th>");
                    carListHtml.Add("    <th width=\"10%\" class=\"pd-left-three\">参考最低价</th>");
                    carListHtml.Add("    <th width=\"18%\">&nbsp;</th>");
                    carListHtml.Add("</tr>");
                }
                else
                {
                    carListHtml.Add("<tr class=\"\">");
                    carListHtml.Add("    <th width=\"44%\" class=\"first-item\">");
                    carListHtml.Add(string.Format("        <div class=\"pdL10\"><strong>{0}</strong> {1}</div>",
                        key.Engine_Exhaust,
                        strMaxPowerAndInhaleType));
                    carListHtml.Add("    </th>");
                    carListHtml.Add("    <th width=\"8%\" class=\"pd-left-one\"></th>");
                    carListHtml.Add("    <th width=\"10%\" class=\"pd-left-one\"></th>");
                    carListHtml.Add("    <th width=\"10%\" class=\"pd-left-two\"></th>");
                    carListHtml.Add("    <th width=\"10%\" class=\"pd-left-three\"></th>");
                    carListHtml.Add("    <th width=\"18%\">&nbsp;</th>");
                    carListHtml.Add("</tr>");
                }
                groupIndex++;
                List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList<CarInfoForSerialSummaryEntity>();
                    //分组后的集合

                foreach (CarInfoForSerialSummaryEntity entity in carGroupList)
                {
                    string yearType = entity.CarYear.Trim();
                    if (yearType.Length > 0)
                        yearType += "款";
                    else
                        yearType = "未知年款";
                    string stopPrd = "";
                    if (entity.ProduceState == "停产")
                        stopPrd = " <span class=\"tingchan\">停产</span>";
                    Dictionary<int, string> dictCarParams = _carBLL.GetCarAllParamByCarID(entity.CarID);
                    // 节能补贴 Sep.2.2010
                    string hasEnergySubsidy = "";
                    //补贴功能临时去掉 modified by chengl Oct.24.2013
                    //if (dictCarParams.ContainsKey(853) && dictCarParams[853] == "3000元")
                    //{
                    //    hasEnergySubsidy = " <a href=\"http://news.bitauto.com/zcxwtt/20130924/1406235633.html\" class=\"butie\" title=\"可获得3000元节能补贴\" target=\"_blank\">补贴</a>";
                    //}
                    //============2012-04-09 减税============================
                    string strTravelTax = "";
                    //车船税功能去掉 2016/3/9
                    //if (dictCarParams.ContainsKey(895))
                    //{
                    //    strTravelTax = " <a target=\"_blank\" title=\"{0}\" href=\"http://news.bitauto.com/others/20120308/0805618954.html\" class=\"jianshui\">减税</a>";
                    //    if (dictCarParams[895] == "减半")
                    //        strTravelTax = string.Format(strTravelTax, "减征50%车船使用税");
                    //    else if (dictCarParams[895] == "免征")
                    //        strTravelTax = string.Format(strTravelTax, "免征车船使用税");
                    //    else
                    //        strTravelTax = "";
                    //}
                    ////易车惠
                    //string strYiCheHui = "";
                    //var carGoods = serialGoodsCarList.Find(p => p.CarId == entity.CarID);
                    //if (carGoods != null)
                    //{
                    //    var goodsUrl = carGoods.GoodsUrl.Replace("/detail", "/all/detail") + "?WT.mc_id=car2";
                    //    strYiCheHui = string.Format("<a target=\"_blank\" title=\"{0}\" href=\"{1}\" class=\"ad-yichehui-list\">易车惠特价&gt;&gt;</a>", "", goodsUrl);
                    //}
                    //string strBest = "<a href=\"#\" class=\"ico-tuijian\">推荐</a>";
                    carListHtml.Add("<tr id=\"car_filter_id_" + entity.CarID + "\">");
                    carListHtml.Add("<td>");
                    carListHtml.Add(
                        string.Format(
                            "    <div class=\"pdL10\" id=\"carlist_{1}\"><a href=\"/{0}/m{1}/\" target=\"_blank\">{2} {3}</a> {4}</div>",
                            serialSpell, entity.CarID, yearType, entity.CarName,
                            strTravelTax + hasEnergySubsidy + stopPrd));
                    carListHtml.Add("</td>");
                    carListHtml.Add("<td>");
                    carListHtml.Add("    <div class=\"w\">");
                    //计算百分比
                    int percent = (int) Math.Round((double) entity.CarPV/maxPv*100.0, MidpointRounding.AwayFromZero);

                    carListHtml.Add(string.Format("        <div class=\"p\" style=\"width: {0}%\"></div>", percent));
                    carListHtml.Add("    </div>");
                    carListHtml.Add("</td>");
                    // 档位个数
                    string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" &&
                                             dictCarParams[724] != "待查")
                        ? dictCarParams[724] + "挡"
                        : "";

                    carListHtml.Add(string.Format("<td>{0}</td>", forwardGearNum + entity.TransmissionType));
                    carListHtml.Add(
                        string.Format(
                            "<td style=\"text-align: right\"><span>{0}</span><a title=\"购车费用计算\" class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid={1}\" target=\"_blank\"></a></td>",
                            string.IsNullOrEmpty(entity.ReferPrice) ? "暂无" : entity.ReferPrice + "万", entity.CarID));
                    if (entity.CarPriceRange.Trim().Length == 0)
                        carListHtml.Add(string.Format("    <td style=\"text-align: right\"><span>{0}</span></td>",
                            "暂无报价"));
                    else
                    {
                        //取最低报价
                        string minPrice = entity.CarPriceRange;
                        if (entity.CarPriceRange.IndexOf("-") != -1)
                            minPrice = entity.CarPriceRange.Substring(0, entity.CarPriceRange.IndexOf('-'));

                        carListHtml.Add(
                            string.Format(
                                "<td style=\"text-align: right\"><span><a href=\"/{0}/m{1}/baojia/\" target=\"_blank\">{2}</a></span></td>",
                                serialSpell, entity.CarID, minPrice));
                    }
                    carListHtml.Add("<td>");
                    carListHtml.Add(
                        string.Format(
                            "<div class=\"car-summary-btn-xunjia button_gray\"><a rel=\"nofollow\" href=\"http://dealer.bitauto.com/zuidijia/nb{0}/nc{1}/?T=2\" target=\"_blank\">询价</a></div>",
                            serialId, entity.CarID));
                    carListHtml.Add(
                        string.Format(
                            "<div class=\"car-summary-btn-duibi button_gray\" id=\"carcompare_btn_new_{0}\"><a target=\"_self\" href=\"javascript:WaitCompareObj.AddCarToCompare('{0}','{1}');\"><span>对比</span></a></div>",
                            entity.CarID, entity.CarName));
                    carListHtml.Add("    </td>");
                    carListHtml.Add("</tr>");
                }
            }
            return string.Concat(carListHtml.ToArray());
        }

        #endregion

        /// <summary>
        /// 核心关键报告
        /// </summary>
        protected void MakeHexinReportHtml()
        {
            const int hexin = (int) CommonHtmlEnum.BlockIdEnum.HexinReport;
            const int suv = (int) CommonHtmlEnum.BlockIdEnum.SuvReport;
            const int carInnerSpace = (int) CommonHtmlEnum.BlockIdEnum.CarInnerSpace;

            if (dictSerialBlockHtml.ContainsKey(hexin) || dictSerialBlockHtml.ContainsKey(suv) ||
                dictSerialBlockHtml.ContainsKey(carInnerSpace))
            {
                StringBuilder sbHtml = new StringBuilder();
                //sbHtml.AppendFormat("<div class=\"line_box stat_box_line {0}\">",
                //    (!dictSerialBlockHtml.ContainsKey(hexin) && dictSerialBlockHtml.ContainsKey(suv)) ? "only-suv" : "");
                sbHtml.AppendFormat("<div class=\"line_box stat_box_line\">");

                sbHtml.Append("<div class=\"title-box\">");
                sbHtml.AppendFormat("<h3><a href=\"/{1}/pingce/\" target=\"_blank\">{0}关键报告</a></h3>", serialSeoName,
                    serialSpell);
                sbHtml.AppendFormat(
                    "<div class=\"more\"><a href=\"/{0}/pingce/\" target=\"_blank\">详细报告&gt;&gt;</a></div>", serialSpell);
                sbHtml.Append("</div>");

                if (dictSerialBlockHtml.ContainsKey(hexin))
                {
                    sbHtml.Append(dictSerialBlockHtml[hexin]);
                    sbHtml.Append("	<div class=\"clear\"></div>");
                }
                if (dictSerialBlockHtml.ContainsKey(suv))
                {
                    sbHtml.Append(dictSerialBlockHtml[suv]);
                    sbHtml.Append("	<div class=\"clear\"></div>");
                }
                if (dictSerialBlockHtml.ContainsKey(carInnerSpace))
                {
                    sbHtml.Append(dictSerialBlockHtml[carInnerSpace]);
                    sbHtml.Append("	<div class=\"clear\"></div>");
                }
                sbHtml.Append("</div>");
                hexinReportHtml = sbHtml.ToString();
            }
        }

        ///// <summary>
        ///// SUV 参数数据
        ///// </summary>
        //protected void MakeSUVReportHtml()
        //{
        //    int suv = (int)CommonHtmlEnum.BlockIdEnum.SuvReport;
        //    if (dictSerialBlockHtml.ContainsKey(suv))
        //        suvReportHtml = dictSerialBlockHtml[suv];
        //}
        /// <summary>
        /// 口碑点评精选
        /// </summary>
        private void MakeEditorCommentHtml()
        {
            // modified by chengl Oct.15.2013
            // 没有核心关键报告的 不显示编辑点评
            if (hexinReportHtml != "")
            {
                //int firstCarId = new CarNewsBll().GetEditorCommentCarId(serialId);
                //editorCommentHtml = _commonhtmlBLL.GetCommonHtmlByBlockId(firstCarId, CommonHtmlEnum.TypeEnum.Car, CommonHtmlEnum.TagIdEnum.SerialSummary, CommonHtmlEnum.BlockIdEnum.EditorComment);
                int editor = (int) CommonHtmlEnum.BlockIdEnum.EditorComment;
                if (dictSerialBlockHtml.ContainsKey(editor))
                    editorCommentHtml = dictSerialBlockHtml[editor];
            }
        }

        /// <summary>
        /// 口碑点评精选
        /// </summary>
        private void MakeKoubeiDianpingHtml()
        {
            //int koubei = (int)CommonHtmlEnum.BlockIdEnum.KoubeiReport;
            //if (dictSerialBlockHtml.ContainsKey(koubei))
            //    koubeiDianpingHtml = dictSerialBlockHtml[koubei];

            int koubei = (int) CommonHtmlEnum.BlockIdEnum.KoubeiReportNew;
            if (dictSerialBlockHtml.ContainsKey(koubei))
                koubeiDianpingHtml = dictSerialBlockHtml[koubei];
        }

        /// <summary>
        /// 新视频块html
        /// </summary>
        private void MakeVideoBlockHtml()
        {
            ////取数据
            //XmlNodeList nodeList = new Car_SerialBll().GetSerialVideo(serialId);
            //if (nodeList.Count <= 0) return;
            //StringBuilder sb = new StringBuilder();
            //sb.Append("<div class=\"line_box\" id=\"car-videobox\">");
            //sb.Append("	<h3>");
            //sb.Append("		<span>");
            ////sb.AppendFormat("<b><a href=\"{0}shipin/\">{1}视频</a></b>", baseUrl, serialSeoName);
            //sb.AppendFormat("<b>{0}视频</b>", serialSeoName);
            //sb.Append("		</span>");
            //sb.Append("	</h3>");
            //sb.Append("	<div class=\"car-video20130802\">");
            //sb.Append("		<ul>");
            //int loop = 0;
            //foreach (XmlNode node in nodeList)
            //{
            //    loop++;
            //    if (loop > 4) break;
            //    string videoTitle = node.SelectSingleNode("title").InnerText;
            //    videoTitle = StringHelper.RemoveHtmlTag(videoTitle);

            //    string shortTitle = node.SelectSingleNode("facetitle").InnerText;
            //    shortTitle = StringHelper.RemoveHtmlTag(shortTitle);

            //    string imgUrl = node.SelectSingleNode("picture").InnerText;
            //    if (imgUrl.Trim().Length == 0)
            //        imgUrl = WebConfig.DefaultVideoPic;
            //    //imgUrl = imgUrl.Replace("/bitauto/", "/newsimg-242-w0-1-q70/bitauto/");
            //    //imgUrl = imgUrl.Replace("/autoalbum/", "/newsimg-242-w0-1-q70/autoalbum/");
            //    string filepath = node.SelectSingleNode("filepath").InnerText;

            //    sb.Append("			<li>");
            //    sb.AppendFormat("				<a href=\"{0}\" target=\"_blank\"><img data-original=\"{1}\" alt=\"{2}\" width=\"170\" height=\"96\"></a>", filepath, imgUrl, serialEntity.Brand.Name + serialName + videoTitle);
            //    sb.AppendFormat("				<a href=\"{0}\" target=\"_blank\">{1}</a>", filepath, shortTitle != videoTitle ? shortTitle : videoTitle);
            //    sb.AppendFormat("				<a href=\"{0}\" target=\"_blank\" class=\"btn-play\">播放</a>", filepath);
            //    sb.Append("			</li>");
            //}
            //sb.Append("		</ul>");
            //sb.Append("		<div class=\"clear\"></div>");
            //sb.Append("	</div>");
            //sb.Append("	<div class=\"more\">");
            //sb.AppendFormat("		<a href=\"{0}shipin/\">更多&gt;&gt;</a>", baseUrl, nodeList.Count);
            //sb.Append("	</div>");
            //sb.Append("</div>");
            //videosHtml = sb.ToString();
            int video = (int) CommonHtmlEnum.BlockIdEnum.Video;
            if (dictSerialBlockHtml.ContainsKey(video))
                videosHtml = dictSerialBlockHtml[video];
        }

        /// <summary>
        /// 口碑印象
        /// </summary>
        /// <returns></returns>
        private void MakeKoubeiImpressionHtml()
        {
            int koubei = (int) CommonHtmlEnum.BlockIdEnum.KoubeiImpression;
            if (dictSerialBlockHtml.ContainsKey(koubei))
            {
                koubeiImpressionHtml = dictSerialBlockHtml[koubei];
                return;
            }

            string filePath = Path.Combine(WebConfig.DataBlockPath,
                string.Format(@"Data\SerialDianping\ImpressionNew\Xml\Impression_{0}.xml", serialId));
            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(filePath))
                xmlDoc.Load(filePath);
            if (xmlDoc == null) return;

            StringBuilder sb = new StringBuilder();
            string impression = string.Empty;
            string virtues = string.Empty;
            string defect = string.Empty;
            var items = xmlDoc.SelectNodes("/ReportList/Item/Detail/Item");
            if (items != null)
            {
                foreach (XmlNode itemNode in items)
                {
                    var usNameNode = itemNode.SelectSingleNode("UsName");
                    if (usNameNode != null)
                    {
                        var usName = usNameNode.InnerText.Trim();
                        if (usName == "impression")
                        {
                            impression = itemNode.SelectSingleNode("Content").InnerText.Trim();
                        }
                        if (usName == "strengths")
                        {
                            virtues = itemNode.SelectSingleNode("Content").InnerText.Trim();
                        }
                        if (usName == "weaknesses")
                        {
                            defect = itemNode.SelectSingleNode("Content").InnerText.Trim();
                        }
                    }
                }
            }
            if (impression.Length > 0 || virtues.Length > 0 || defect.Length > 0)
            {
                impression = StringHelper.RemoveHtmlTag(StringHelper.SubString(impression, 96, true));
                virtues = StringHelper.RemoveHtmlTag(virtues);
                defect = StringHelper.RemoveHtmlTag(defect);

                string reportUrl = string.Format("/{0}/koubei/", serialSpell);

                sb.Append("	<div class=\"line-box\">");
                sb.Append("<div class=\"side_title\">");
                sb.AppendFormat("<h4><a href=\"{0}\" target=\"_blank\">网友对此车的印象</a></h4>", reportUrl);
                sb.Append("</div>");

                //sb.AppendFormat("<p>{1}<a href=\"{0}\" target=\"_blank\">查看详情&gt;&gt;</a></p>", reportUrl, impression);
                sb.Append("<div class=\"youque_box\"><h6 class=\"fl\">优点：</h6>");
                if (StringHelper.GetRealLength(virtues) > 48)
                    sb.AppendFormat("<p class=\"txt\" title=\"{0}\">{1}</p>", virtues,
                        StringHelper.SubString(virtues, 46, true));
                else
                    sb.AppendFormat("<p class=\"txt\">{0}</p>", virtues);
                sb.Append("</div>");
                sb.Append("<div class=\"youque_box quedian\"><h6 class=\"fl\">缺点：</h6>");
                if (StringHelper.GetRealLength(defect) > 48)
                    sb.AppendFormat("<p class=\"txt\" title=\"{0}\">{1}</p>", defect,
                        StringHelper.SubString(defect, 46, true));
                else
                    sb.AppendFormat("<p class=\"txt\">{0}</p>", defect);
                sb.Append("</div>");
                sb.Append("        <div class=\"btn_box\">");
                sb.Append(
                    "        	<span class=\"button_orange\"><a href=\"http://ask.bitauto.com/tiwen/\" target=\"_blank\" class=\"koubei_b_tiwen\">我要提问</a></span>");
                sb.AppendFormat(
                    "   <span class=\"button_gray\"><a href=\"http://car.bitauto.com/{0}/koubei/#fabu\" target=\"_blank\" class=\"koubei_b_dianping\">我要点评</a></span>",
                    serialSpell);
                sb.Append("        </div>");
                sb.Append("<div class=\"clear\"></div>");
                sb.Append("    </div>");
            }
            koubeiImpressionHtml = sb.ToString();
        }
        /*
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
            Dictionary<int, Dictionary<int, Dictionary<string, string>>> cacheData =
                CacheManager.GetCachedData(key) as Dictionary<int, Dictionary<int, Dictionary<string, string>>>;
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
                                        string title = string.Empty,
                                            url = string.Empty,
                                            imgUrl = string.Empty,
                                            priceRange = string.Empty;
                                        int postion = 0, toSerialId = 0;
                                        bool needAd = true;
                                        while (subReader.Read())
                                        {
                                            if (subReader.NodeType == XmlNodeType.EndElement &&
                                                subReader.Name == "Serial")
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
                                                    DateTime startDate =
                                                        ConvertHelper.GetDateTime(reader.ReadString().Trim());
                                                    if (startDate > curDate)
                                                    {
                                                        needAd = false;
                                                        break;
                                                    }
                                                }
                                                else if (reader.Name == "EndData")
                                                {
                                                    DateTime endDate =
                                                        ConvertHelper.GetDateTime(reader.ReadString().Trim());
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
                catch
                {
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    if (stream != null)
                        stream.Dispose();
                }
            }
            return cacheData.ContainsKey(serialId) ? cacheData[serialId] : null;
        }*/

        /// <summary>
        /// 子品牌还关注
        /// </summary>
        /// <returns></returns>
        private void MakeSerialToSerialHtml()
        {
            serialToSeeJson = _serialBLL.GetSerialSeeToSeeJson(serialId, 8);
        }

        /// <summary>
        /// 取子品牌对比排行数据
        /// </summary>
        /// <returns></returns>
        private void MakeHotSerialCompare()
        {
            int hotCarID = 0;
            Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList =
                new Car_SerialBll().GetSerialCityCompareList(serialId, HttpContext.Current);
            List<string> htmlList = new List<string>();
            string compareBaseUrl = "/interfaceforbitauto/TransferCsIDToCarIDForCompare.aspx?count=3&csids=" + serialId +
                                    ",";
            if (carSerialBaseList != null && carSerialBaseList.Count > 0 && carSerialBaseList.ContainsKey("全国"))
            {
                List<Car_SerialBaseEntity> serialCompareList = carSerialBaseList["全国"];

                htmlList.Add("<div class=\"line-box\" id=\"serialHotCompareList\">");

                htmlList.Add("<div class=\"side_title\">");
                htmlList.Add(
                    string.Format(
                        "<h4><a rel=\"nofollow\" href=\"/chexingduibi/\" target=\"_blank\">大家都用他和谁比</a></h4>", hotCarID));
                htmlList.Add("</div>");


                //htmlList.Add("<div id=\"rank_model_box\" class=\"ranking_list\">");
                htmlList.Add("<ul class=\"text-list\">");

                for (int i = 0; i < serialCompareList.Count; i++)
                {
                    Car_SerialBaseEntity carSerial = serialCompareList[i];

                    htmlList.Add("<li>");
                    htmlList.Add(
                        string.Format(
                            "<a href=\"" + compareBaseUrl + carSerial.SerialId +
                            "\" target=\"_blank\">{0}<em class=\"vs\">VS</em>{1}</a>",
                            BitAuto.Utils.StringHelper.SubString(serialShowName, 10, false),
                            carSerial.SerialShowName.Trim()));
                    htmlList.Add("</li>");
                }

                htmlList.Add("</ul>");
                //htmlList.Add("</div>");
                htmlList.Add("</div>");
            }

            hotSerialCompareHtml = String.Concat(htmlList.ToArray());
        }

        /// <summary>
        /// 得到品牌下的其他子品牌
        /// </summary>
        /// <returns></returns>
        private void MakeBrandOtherSerial()
        {
            if (serialEntity == null || serialEntity.Id == 0)
            {
                return;
            }
            List<CarSerialPhotoEntity> carSerialPhotoList =
                new Car_BrandBll().GetCarSerialPhotoListByCBID(serialEntity.BrandId, false);

            carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

            if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
            {
                return;
            }

            List<string> htmlList = new List<string>();
            int loop = 0;
            foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
            {
                loop++;
                if (entity.SerialLevel == "概念车" || entity.SerialId == serialEntity.Id)
                    continue;

                string priceRang = base.GetSerialPriceRangeByID(entity.SerialId);
                if (entity.SaleState == "待销")
                    priceRang = "未上市";
                else
                {
                    if (priceRang.Trim().Length == 0)
                        priceRang = "暂无报价";
                }

                string tempCsSeoName = string.IsNullOrEmpty(entity.CS_SeoName) ? entity.ShowName : entity.CS_SeoName;
                htmlList.Add(
                    String.Format("<li><a target=\"_blank\" href=\"/{0}/\">{1}</a><span class=\"dao\">{2}</span></li>",
                        entity.CS_AllSpell, tempCsSeoName, priceRang));
            }

            List<string> brandHtmlList = new List<string>();
            brandHtmlList.Add("<div class=\"line-box\">");
            if (htmlList.Count > 0)
            {
                brandHtmlList.Add("<div class=\"side_title\">");
                brandHtmlList.Add(string.Format("<h4><a target=\"_blank\" href=\"/{0}/\">{1}其他车型</a></h4>",
                    serialEntity.Brand.AllSpell, serialEntity.Brand.Name));
                brandHtmlList.Add("</div>");
                brandHtmlList.Add("<ul class=\"text-list\">");
                brandHtmlList.AddRange(htmlList);
                brandHtmlList.Add("</ul>");
                brandHtmlList.Add("<div class=\"clear\"></div>");
            }
            brandHtmlList.Add("</div>");
            brandOtherSerial = String.Concat(brandHtmlList.ToArray());
        }

        /// <summary>
        /// 答疑
        /// </summary>
        private void MakAskHtml()
        {
            int ask = (int) CommonHtmlEnum.BlockIdEnum.Ask;
            if (dictSerialBlockHtml.ContainsKey(ask))
                askHtml = dictSerialBlockHtml[ask];
        }

        private void InitNextSeeNew()
        {
            nextSeePingceHtml = String.Empty;
            nextSeeXinwenHtml = String.Empty;
            nextSeeDaogouHtml = String.Empty;
            CarNewsBll newsBll = new CarNewsBll();
            if (newsBll.IsSerialNews(serialId, 0, CarNewsType.pingce))
                nextSeePingceHtml = "<li><a href=\"/" + serialSpell + "/pingce/\" target=\"_self\">" +
                                    serialShowName + "车型详解</a></li>";
            //未使用-anh 20120326
            //if (newsBll.IsSerialNews(serialId, 0, CarNewsType.xinwen))
            //    nextSeeXinwenHtml = "<li><a href=\"/" + serialSpell + "/xinwen/\" target=\"_self\">" + serialShowName + "<span>新闻</span></a></li>";

            if (newsBll.IsSerialNews(serialId, 0, CarNewsType.daogou))
                nextSeeDaogouHtml = "<li><a href=\"/" + serialSpell + "/daogou/\" target=\"_self\">" +
                                    serialShowName + "导购</a></li>";
        }
    }
}