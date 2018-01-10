using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;

using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using System.Web;

namespace BitAuto.CarChannel.CarchannelWeb.PageSerialV2
{
    public partial class CsSummary : PageBase
    {
        protected EnumCollection.SerialInfoCard serialInfo;	//子品牌名片
        private Car_SerialBll _serialBLL;
        private Car_BasicBll _carBLL;
        private CommonHtmlBll _commonhtmlBLL;
        protected CarNewsBll _carNewsBLL;
        private DataSet dsCsPic;//图片列表数据源 按分类
        protected string serialShowName = string.Empty;
        private Dictionary<int, string> dictSerialBlockHtml;//静态块内容
        #region 页面变量
        protected int serialId;
        protected string serialSeoName = string.Empty;
        protected string serialSpell = string.Empty;
        protected string serialPrice = string.Empty;
        protected double serialMinPrice = 0;
        protected string baseUrl = string.Empty;
        protected SerialEntity serialEntity;				//子品牌信息
        protected string serialName = string.Empty;
        protected string noSaleLastReferPrice = string.Empty;//最新车款厂商指导价
        protected bool isElectrombile = false;//是否是全系电动车
        protected string serialNoSaleDisplacement = string.Empty;//停销车款 排量
        protected string serialNoSaleDisplacementalt = string.Empty;//停销车款 排量
        protected string serialSaleDisplacement = string.Empty;//在销车款 排量
        protected string serialSaleDisplacementalt = string.Empty;//在销车款 排量描述
        protected string serialTransmission = string.Empty;//变速箱
        //protected bool isExistCarList = true;//是否存在车型
        protected string chargeTimeRange = string.Empty;//充电时间区间
        //protected string fastChargeTimeRange = string.Empty;//快充时间区间
        protected string mileageRange = string.Empty;//续航里程区间
        protected string wirelessSerialUrl = string.Empty; //移动端对应的url
        protected string serialToSeeJson = string.Empty;//看过还看json
        protected string CsHotCompareCars = string.Empty;//综合对比
        protected string focusNewsHtml = string.Empty;//分类新闻
        protected string baaUrl = string.Empty;
        protected bool isHaveBaa = true; //是否有论坛
        protected string serialTotalPV = "暂无";//关注度
        protected string CNCAPAndENCAPStr = "暂无";//CNCAP  ENCAP
        protected string mpSerialYouHuiHtml = string.Empty;//名片区优惠

        protected string serialHeaderHtml = string.Empty;//通用导航
        protected string serialHeaderJs = string.Empty;//导航脚本 
        protected string focusImagesHtml = string.Empty; //焦点图
        protected string carListTableHtml = string.Empty;//车型列表
        protected string carListFilterData = string.Empty;//车型列表 筛选数据
        protected string koubeiDianpingHtml = string.Empty;//网友点评
        protected string videosHtml = string.Empty;//视频
        //protected string competitiveKoubeiHtml = string.Empty;//竞争车型
        protected string bbsNewsHtml = string.Empty;//论坛
        protected string serialSparkleHtml = string.Empty;//亮点配置
        protected string photoListHtml = string.Empty;//图片列表
        protected string koubeiReportHtml = string.Empty;//口碑报告
        protected string awardHtml = string.Empty;//奖项块
        protected string editorCommentHtml = string.Empty;//编辑点评
        protected string hexinReportHtml = string.Empty;//核心报告
        protected string relatedNewsHtml = string.Empty;//相关新闻
        protected string carPingceHtml = string.Empty;//车型详解
        protected string brandOtherSerial = string.Empty;//其他车型

        protected string baoZhiLv = string.Empty;//五年保值率
        protected string VRUrl = string.Empty;//vr url

        #endregion

        public CsSummary()
        {
            _serialBLL = new Car_SerialBll();
            _carBLL = new Car_BasicBll();
            _commonhtmlBLL = new CommonHtmlBll();
            _carNewsBLL = new CarNewsBll();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(10);
            GetParamter();
            InitData();
            base.MakeSerialTopADCode(serialId);

            MakeFocusImages();//焦点图
            MakeCarListHtmlNew();//车款列表
            MakeBlockHtml();//块内容

            MakeSerialToSerialHtml();//看了还看
            GetSerialHotCompareCars();//综合对比
            MakeBrandOtherSerial();//同品牌其他车型
            MakeBBSNewsHtml();//论坛新闻
            MakeSerialSparkleHtml();//亮点配置
            MakePhotoListHtml();//图片列表
            GetCNCAPAndENCAPData();//碰撞数据
            MakeRelatedNewHtml();//相关新闻
            GetBaoZhiLv();//5年保值率
            //GetVrUrl();//Vr url
            //InitNextSeeNew();//接下来要看 评测和导购
            ucNextToSee.serialId = serialId;
        }

        /// <summary>
        /// 获取vr url
        /// </summary>
        private void GetVrUrl()
        {
            Dictionary<int, string> vrDic = _serialBLL.GetSerialVRUrl();
            if (vrDic != null && vrDic.ContainsKey(serialId))
            {
                VRUrl = vrDic[serialId];
            }
        }

        /// <summary>
        /// 五年保值率
        /// </summary>
        protected void GetBaoZhiLv()
        {
            Dictionary<int, XmlElement> dic = _serialBLL.GetSeialBaoZhiLv();
            //string[] baoZhiLvLevel = { "weixingche", "xiaoxingche", "jincouxingche", "zhongxingche", "zhongdaxingche", "haohuaxingche", "mpv", "suv", "paoche", "mianbaoche" };
            if (dic != null && dic.ContainsKey(serialId))
            {
                XmlElement ele = dic[serialId];
                if (ele != null)
                {
                    string levelSpell = BitAuto.CarUtils.Define.CarLevelDefine.GetLevelSpellByName(serialEntity.Level.Name);
                    string ratio = Math.Round(ConvertHelper.GetDouble(ele.Attributes["ResidualRatio5"].InnerText) * 100, 1).ToString();
                    baoZhiLv = string.Format("<li><span class=\"note\">五年保值率: </span><span class=\"data\"><a class=\"lnk-bzl\" href=\"/{0}/baozhilv/\" target=\"_blank\" data-channelid=\"2.21.2032\">{1}% &gt;</a></span></li>"
                            , levelSpell
                            , ratio);
                }
            }
            if (string.IsNullOrEmpty(baoZhiLv))
            {
                baoZhiLv = "<li><span class=\"note\">五年保值率: </span><span class=\"data grey-txt\">暂无</span></li>";
            }
        }

        protected void MakeBlockHtml()
        {
            int carPingce = (int)CommonHtmlEnum.BlockIdEnum.Pingce;//评测
            if (dictSerialBlockHtml.ContainsKey(carPingce))
                carPingceHtml = dictSerialBlockHtml[carPingce];

            int focus = (int)CommonHtmlEnum.BlockIdEnum.FocusNews;//焦点新闻
            if (dictSerialBlockHtml.ContainsKey(focus))
                focusNewsHtml = dictSerialBlockHtml[focus];

            int koubeiRating = (int)CommonHtmlEnum.BlockIdEnum.KoubeiReportNew;//口碑报告
            if (dictSerialBlockHtml.ContainsKey(koubeiRating))
                koubeiReportHtml = dictSerialBlockHtml[koubeiRating];

            int video = (int)CommonHtmlEnum.BlockIdEnum.Video;//视频
            if (dictSerialBlockHtml.ContainsKey(video))
                videosHtml = dictSerialBlockHtml[video];

            int hexin = (int)CommonHtmlEnum.BlockIdEnum.HexinReport;//核心报告
            if (dictSerialBlockHtml.ContainsKey(hexin))
                hexinReportHtml = dictSerialBlockHtml[hexin];

            int carAward = (int)CommonHtmlEnum.BlockIdEnum.CarAward;//获奖经历
            if (dictSerialBlockHtml.ContainsKey(carAward))
                awardHtml = dictSerialBlockHtml[carAward];

            int editor = (int)CommonHtmlEnum.BlockIdEnum.EditorComment;//易车网评测编辑
            if (dictSerialBlockHtml.ContainsKey(editor))
                editorCommentHtml = dictSerialBlockHtml[editor];

            int koubei = (int)CommonHtmlEnum.BlockIdEnum.KoubeiReport; //网友点评
            if (dictSerialBlockHtml.ContainsKey(koubei))
                koubeiDianpingHtml = dictSerialBlockHtml[koubei];

            //int competitive = (int)CommonHtmlEnum.BlockIdEnum.CompetitiveKoubei; //竞争车型
            //if (dictSerialBlockHtml.ContainsKey(competitive))
            //	competitiveKoubeiHtml = dictSerialBlockHtml[competitive];
        }

        /// <summary>
        /// 相关新闻
        /// </summary>
        protected void MakeRelatedNewHtml()
        {
            DataSet ds = _carNewsBLL.GetTopSerialNews(serialId, 10);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"layout-2 relation-article-sidebar\" data-channelid=\"2.21.1536\">");
            sb.Append("<h3 class=\"top-title\">相关文章</h3>");
            sb.Append("<div class=\"list-txt list-txt-s list-txt-style\"><ul>");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.AppendFormat("<li class=\"no-wrap\"><a href=\"{0}\" target=\"_blank\">{1}</a></li>", dr["FilePath"], dr["Title"]);
            }
            sb.Append(" </ul></div></div>");
            relatedNewsHtml = sb.ToString();
        }

        /// <summary>
        /// 图片列表
        /// </summary>
        private void MakePhotoListHtml()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbPhoto = new StringBuilder();
            int picNum = 0;
            List<string> classList = new List<string>();
            XmlNode firstTujieNode = null;
            if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("A"))
            {

                int cateId = 0;     //分类ID
                if (dsCsPic.Tables.Contains("A"))
                {
                    Dictionary<int, string> dict = new Dictionary<int, string> { { 6, "外观" }, { 7, "内饰" }, { 8, "空间" }, { 12, "图解" }, { 11, "官方图" } };
                    List<int> existCategory = new List<int>();
                    foreach (DataRow row in dsCsPic.Tables["A"].Rows)
                    {
                        int cateNum = Convert.ToInt32(row["N"]);
                        picNum += cateNum;
                        cateId = Convert.ToInt32(row["G"]);
                        if (cateId != 6 && cateId != 7 && cateId != 8 && cateId != 12 && cateId != 11)
                            continue;
                        existCategory.Add(cateId);
                    }
                    if (existCategory.IndexOf(11) > -1)
                    {
                        existCategory.Remove(11);
                        existCategory.Add(11);
                    }
                    foreach (int cateTemp in existCategory)
                    {
                        classList.Add(string.Format("<a href=\"http://photo.bitauto.com/serialmore/{0}/{1}/\" target=\"_blank\">{2}</a>",
                            serialId, cateTemp, dict.FirstOrDefault(d => d.Key == cateTemp).Value));
                    }
                    //取图解第一张
                    firstTujieNode = this.GetFirstTujieImage(dsCsPic);
                }
                string allPicUrl = baseUrl + "tupian/";
            }
            if (picNum <= 0) return;
            //11张图片
            List<XmlNode> srcElevenImageList = _serialBLL.GetSerialElevenPositionImage(serialId, 12);
            //add by sk 2013.11.25 有图解 第二张 插入到第六张图位置
            if (firstTujieNode != null)
            {
                //modified by sk 2014.03.11 候姐 图解与11张图排重 重复显示11张图
                var tujieExist = srcElevenImageList.Find(p => p.Attributes["ImageId"].Value == firstTujieNode.Attributes["ImageId"].Value);
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
            var elevenImageList = srcElevenImageList.Take(12);
            var ImgGroupDic = new Dictionary<string, bool>() { { "外观", false }, { "内饰", false }, { "空间", false }, { "图解", false } };

            sbPhoto.Append("<div class=\"section-header header2 mbl\">");
            sbPhoto.Append("    <div class=\"box\" data-channelid=\"2.21.808\">");
            sbPhoto.AppendFormat("<h2><a href=\"http://photo.bitauto.com/serial/{1}/\" target=\"_blank\">{0}图片</a></h2>", serialShowName, serialId);
            sbPhoto.Append("    </div>");
            sbPhoto.Append("    <div class=\"more\" data-channelid=\"2.21.809\">");
            sbPhoto.Append(string.Join("", classList.ToArray()));
            sbPhoto.AppendFormat("<a href=\"http://photo.bitauto.com/serial/{0}/\" target=\"_blank\">全部图片&gt;&gt;</a>", serialId);
            sbPhoto.Append("    </div>");
            sbPhoto.Append("</div>");

            if (srcElevenImageList.Count < 10)
            {
                sbPhoto.Append("<div class=\"row col4-243x1-204x3 type-3\">");
                foreach (XmlNode node in elevenImageList)
                {
                    string categoryName = node.Attributes["GroupName"] != null ? node.Attributes["GroupName"].Value : string.Empty;
                    sbPhoto.Append("<div class=\"figure-box w204-h136 col-auto\">");
                    sbPhoto.AppendFormat("    <a href=\"{0}\" target=\"_blank\" class=\"figure\">", node.Attributes["Link"].Value);
                    if (ImgGroupDic.ContainsKey(categoryName) && !ImgGroupDic[categoryName])
                    {
                        ImgGroupDic[categoryName] = true;
                        sbPhoto.AppendFormat("<span class=\"tag-note1\">{0}</span>", node.Attributes["GroupName"].Value);
                    }
                    sbPhoto.AppendFormat("        <img data-original=\"{0}\">", string.Format(node.Attributes["ImageUrl"].Value, 4));
                    sbPhoto.Append("    </a>");
                    sbPhoto.Append("</div>");
                }
                sbPhoto.Append("<div class=\"figure-box w204-h136 col-auto last\">");
                sbPhoto.Append("    <div class=\"title-box\">");
                sbPhoto.AppendFormat("        <a href=\"http://photo.bitauto.com/serial/{0}/\" target=\"_blank\">", serialId);
                sbPhoto.AppendFormat("            <h5 class=\"title\">共{0}张实拍图</h5>", picNum);
                sbPhoto.Append("        </a>");
                sbPhoto.Append("    </div>");
                sbPhoto.Append("</div>");
                sbPhoto.Append("</div>");
            }
            else
            {
                bool isHaveBig = false;
                int loop = 0;
                sbPhoto.Append("<div class=\"row col4-243x1-204x3\" style=\"height: 410px;\" data-channelid=\"2.21.811\">");
                sbPhoto.Append("<div class=\"col-auto\">");
                foreach (XmlNode node in elevenImageList.Take(3))
                {
                    loop++;
                    string link = node.Attributes["Link"].Value;

                    string alt = serialShowName + node.Attributes["ImageName"].Value;

                    string[] categoryNameArray = { "图解" };
                    string categoryName = node.Attributes["GroupName"] != null ? node.Attributes["GroupName"].Value : string.Empty;
                    if (loop == 2 && categoryNameArray.Contains(categoryName))
                    {
                        string imgUrl = string.Format(node.Attributes["ImageUrl"].Value, 1);
                        sbPhoto.Append(" <div class=\"figure-box w243-h273\">");
                        sbPhoto.AppendFormat("     <a href=\"{0}\" target=\"_blank\" class=\"figure\">", link);
                        if (ImgGroupDic.ContainsKey(categoryName) && !ImgGroupDic[categoryName])
                        {
                            ImgGroupDic[categoryName] = true;
                            sbPhoto.AppendFormat("<span class=\"tag-note1\">{0}</span>", node.Attributes["GroupName"].Value);
                        }
                        sbPhoto.AppendFormat("         <img data-original=\"{0}\" alt=\"{1}\" />", imgUrl, alt);
                        sbPhoto.Append("     </a>");
                        sbPhoto.Append(" </div>");
                        isHaveBig = true;
                        break;
                    }
                    else
                    {
                        string imgUrl = string.Format(node.Attributes["ImageUrl"].Value, 4);
                        sbPhoto.AppendFormat("<div class=\"figure-box w243-h136\"{0}>", (loop == 3 ? "" : " style=\"margin-bottom: 1px;\""));
                        sbPhoto.AppendFormat("<a href=\"{0}\" target=\"_blank\" class=\"figure\">", link);
                        if (ImgGroupDic.ContainsKey(categoryName) && !ImgGroupDic[categoryName])
                        {
                            ImgGroupDic[categoryName] = true;
                            sbPhoto.AppendFormat("<span class=\"tag-note1\">{0}</span>", node.Attributes["GroupName"].Value);
                        }
                        sbPhoto.AppendFormat("<img data-original=\"{0}\" alt=\"{1}\" />", imgUrl, alt);
                        sbPhoto.Append("</a>");
                        sbPhoto.Append("</div>");
                    }
                }
                sbPhoto.Append("</div>");
                loop = 0;
                IEnumerable<XmlNode> nodeList = isHaveBig ? elevenImageList.Skip(2) : elevenImageList.Skip(3);
                sbPhoto.Append("<div class=\"col-auto\" style=\"margin-left: 1px; margin-bottom: 1px\">");
                //Dictionary<int, int> marginLeft = new Dictionary<int, int>() { { 0, 0 }, { 1, 2 }, { 2, 1 } };
                foreach (XmlNode node in nodeList)
                {
                    loop++;

                    string link = node.Attributes["Link"].Value;
                    string imgUrl = string.Format(node.Attributes["ImageUrl"].Value, 4);
                    string alt = serialShowName + node.Attributes["ImageName"].Value;

                    sbPhoto.AppendFormat("<div class=\"figure-box w204-h136 col-auto{1}\"{0}>"
                        , ((loop - 1) % 3 == 0 ? "" : " style=\"margin-left: 1px;\"")
                        , loop == 9 ? " last" : "");
                    sbPhoto.AppendFormat("    <a href=\"{0}\" target=\"_blank\" class=\"figure\">", link);
                    string categoryName = node.Attributes["GroupName"] != null ? node.Attributes["GroupName"].Value : string.Empty;
                    if (ImgGroupDic.ContainsKey(categoryName) && !ImgGroupDic[categoryName])
                    {
                        ImgGroupDic[categoryName] = true;
                        sbPhoto.AppendFormat("<span class=\"tag-note1\">{0}</span>", node.Attributes["GroupName"].Value);
                    }
                    sbPhoto.AppendFormat("        <img data-original=\"{0}\" alt=\"{1}\" />", imgUrl, alt);
                    sbPhoto.Append("    </a>");
                    if (loop == 9)
                    {
                        sbPhoto.Append("<div class=\"title-box\">");
                        sbPhoto.AppendFormat("<a href=\"http://photo.bitauto.com/serial/{0}/\" target=\"_blank\">", serialId);
                        sbPhoto.AppendFormat("<h5 class=\"title\">共{0}张实拍图</h5>", picNum);
                        sbPhoto.Append("</a>");
                        sbPhoto.Append("</div>");
                    }
                    sbPhoto.Append("</div>");
                    if (loop == 9)
                    {
                        //sbPhoto.AppendFormat("</div>"); 
                        break;
                    }
                    if (loop % 3 == 0)
                    {
                        sbPhoto.AppendFormat("</div><div class=\"col-auto\" style=\"margin-left: 1px;{0}\">", (loop / 3 == 2 ? "" : " margin-bottom: 1px"));
                    }

                }
                if (loop < 9)
                {
                    sbPhoto.AppendFormat("<div class=\"figure-box w204-h136 col-auto last\"{0}\">", ((loop - 1) % 3 == 0 ? "" : " style=\"margin-left: 1px;\""));
                    sbPhoto.Append("<div class=\"title-box\">");
                    sbPhoto.AppendFormat("<a href=\"http://photo.bitauto.com/serial/{0}/\" target=\"_blank\">", serialId);
                    sbPhoto.AppendFormat("<h5 class=\"title\">共{0}张实拍图</h5>", picNum);
                    sbPhoto.Append("</a>");
                    sbPhoto.Append("</div>");
                    sbPhoto.Append("</div>");
                }
                sbPhoto.Append("</div></div>");
            }
            //sbPhoto.Append("");

            photoListHtml = sbPhoto.ToString();
        }

        /// <summary>
        /// 车型论坛、论坛新闻  待确认默认数据
        /// </summary>
        private void MakeBBSNewsHtml()
        {
            //获取数据
            XmlDocument xmlDoc = _serialBLL.GetSerialForumNews(serialId);
            XmlNodeList newsList = xmlDoc.SelectNodes("/root/Forum/ForumSubject");

            if (baaUrl == "http://baa.bitauto.com/" || newsList.Count <= 0)
            {
                isHaveBaa = false;
                //子品牌综述页论坛补充贴(当1条论坛数据都没有)
                string includeFilePath = Server.MapPath("~/include/BAA/BBS/00001/20170424_cxzs_luntan_new_Manual.shtml");
                if (File.Exists(includeFilePath))
                {
                    bbsNewsHtml = File.ReadAllText(includeFilePath);
                }
                return;
            }

            //StringBuilder sbBBSNewsFirst = new StringBuilder();
            StringBuilder sbBBSNews = new StringBuilder();
            if (newsList.Count > 0)
            {
                sbBBSNews.Append("<div class=\"list-txt list-txt-m list-txt-default list-txt-style2 type-1\"><ul>");
                int loop = 0;
                foreach (XmlNode newsNode in newsList)
                {
                    string tid = newsNode.SelectSingleNode("tid").InnerText;
                    string filePath = newsNode.SelectSingleNode("url").InnerText;
                    string newsTitle = newsNode.SelectSingleNode("title").InnerText.Trim();
                    //过滤Html标签
                    newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
                    //if (loop++ == 0)
                    //{
                    //	sbBBSNewsFirst.AppendFormat("<h3 class=\"no-wrap\"><a href=\"{0}\" target=\"_blank\" title=\"{1}\">{2}</a></h3>", filePath, newsTitle, newsTitle);
                    //	continue;
                    //}
                    if (++loop > 5) break;
                    int classId = ConvertHelper.GetInteger(newsNode.SelectSingleNode("digest").InnerText);
                    string className = Enum.GetName(typeof(EnumCollection.ForumDigest), classId);
                    string pubTime = string.Empty;

                    if (newsNode.SelectSingleNode("postdatetime") != null)
                    {
                        pubTime = newsNode.SelectSingleNode("postdatetime").InnerText;
                        pubTime = Convert.ToDateTime(pubTime).ToString("MM-dd");
                    }
                    sbBBSNews.Append("<li>");
                    sbBBSNews.Append("<div class=\"txt\">");
                    sbBBSNews.AppendFormat("<strong><a href=\"{0}\" target=\"_blank\">{1}</a>|</strong><a href=\"{2}\" target=\"_blank\" title=\"{3}\">{4}</a>"
                        , baaUrl + "index-" + classId + "-all-1-0.html"
                        , className
                        , filePath
                        , newsTitle
                        , newsTitle.Length > 15 ? (newsTitle.Substring(0, 15) + "...") : newsTitle);
                    sbBBSNews.Append("</div>");
                    sbBBSNews.AppendFormat("<span>{0}</span></li>", pubTime);
                }
                sbBBSNews.Append("</ul></div>");
            }

            bbsNewsHtml = sbBBSNews.ToString();
        }

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
            //foreach (string year in saleYearList)
            //{
            //	if (noSaleYearList.Contains(year))
            //	{
            //		noSaleYearList.Remove(year);
            //	}
            //}
            List<CarInfoForSerialSummaryEntity> carinfoSaleList = carinfoList
                .FindAll(p => p.SaleState == "在销");
            List<CarInfoForSerialSummaryEntity> carinfoWaitSaleList = carinfoList
                .FindAll(p => p.SaleState == "待销");


            List<CarInfoForSerialSummaryEntity> carinfoNoSaleList = carinfoList
                .FindAll(p => p.SaleState == "停销");

            if (carinfoSaleList.Count <= 0 && carinfoWaitSaleList.Count <= 0 && carinfoNoSaleList.Count <= 0)
            {
                //isExistCarList = false;
                return;
            }
            //add by 2014.05.04 在销车款 电动车
            var fuelTypeList = carinfoSaleList.Where(p => p.Oil_FuelType != "")
                                              .GroupBy(p => p.Oil_FuelType)
                                              .Select(g => g.Key).ToList();
            isElectrombile = fuelTypeList.Count == 1 && fuelTypeList[0] == "纯电" ? true : false;
            //add by 2014.03.18 在销车款 排量输出
            var exhaustList = carinfoSaleList.Where(p => p.Engine_Exhaust.EndsWith("L"))
                .Select(p => p.Engine_InhaleType.IndexOf("增压") >= 0 ? p.Engine_Exhaust.Replace("L", "T") : p.Engine_Exhaust)
                                            .GroupBy(p => p)
                                            .Select(group => group.Key).ToList();

            #region add by songcl 2014-11-21 停销车款 排量输出

            var maxYear = carinfoNoSaleList.Max(s => s.CarYear);
            var tempList = carinfoNoSaleList.Where(s => s.CarYear == maxYear).ToList();

            List<string> noSaleExhaustList = tempList.Where(p => p.Engine_Exhaust.EndsWith("L"))
                                                              .Select(
                                                                  p =>
                                                                  p.Engine_InhaleType.IndexOf("增压") >= 0
                                                                      ? p.Engine_Exhaust.Replace("L", "T")
                                                                      : p.Engine_Exhaust)
                                                              .GroupBy(p => p)
                                                              .Select(group => group.Key).ToList();

            List<string> fuelTypeListForNoSeal = tempList.Where(p => p.Oil_FuelType != "")
                                                                  .GroupBy(p => p.Oil_FuelType)
                                                                  .Select(g => g.Key).ToList();

            if (noSaleExhaustList.Count > 0)
            {
                noSaleExhaustList.Sort(NodeCompare.ExhaustCompareNew);
                if (noSaleExhaustList.Count > 3)
                {
                    serialNoSaleDisplacement = string.Concat(noSaleExhaustList[0], " ", noSaleExhaustList[1]
                                                             , "..."
                                                             , noSaleExhaustList[noSaleExhaustList.Count - 1],
                                                             fuelTypeListForNoSeal.Contains("纯电") ? " 电动" : "");
                }
                else
                    serialNoSaleDisplacement = string.Join(" ", noSaleExhaustList.ToArray()) +
                                               (fuelTypeListForNoSeal.Contains("纯电") ? " 电动" : "");
                serialNoSaleDisplacementalt = string.Join(" ", noSaleExhaustList.ToArray()) +
                                              (fuelTypeListForNoSeal.Contains("纯电") ? " 电动" : "");
            }

            #endregion

            if (exhaustList.Count > 0)
            {
                exhaustList.Sort(NodeCompare.ExhaustCompareNew);
                if (exhaustList.Count > 3)
                {
                    serialSaleDisplacement = string.Concat(exhaustList[0], "-", exhaustList[exhaustList.Count - 1]);
                }
                else
                    serialSaleDisplacement = string.Join(" ", exhaustList.ToArray()) + (fuelTypeList.Contains("纯电") ? " 电动" : "");
                serialSaleDisplacementalt = string.Join(" ", exhaustList.ToArray());
            }
            //add by 2014.05.20 车型筛选所用
            var newExhaustList = exhaustList.GetRange(0, exhaustList.Count);
            if (fuelTypeList.Contains("纯电"))
                newExhaustList.Add("电动");

            carinfoSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
            carinfoWaitSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);

            noSaleYearList.Sort(NodeCompare.CompareStringDesc);

            sb.Append("<div class=\"layout-1 cartype-inner-section\"><div class=\"section-header header2 h-default2 mb0\"><div class=\"box\">");
            sb.AppendFormat("<h2>{0}车款</h2>", serialShowName);
            sb.Append("<ul class=\"nav\" id=\"data_tab_jq5\" data-channelid=\"2.21.1515\">");


            if (carinfoSaleList.Count > 0)
            {
                sb.Append("<li class=\"current\"><a href=\"javascript:;\">全部在售</a></li>");
            }
            bool isWaitSale = false;
            if (carinfoWaitSaleList.Count > 0)
            {
                isWaitSale = true;
                sb.AppendFormat("<li class=\"{0}\"><a href=\"javascript:;\">未上市</a></li>",
                    carinfoSaleList.Count <= 0 ? "current" : "");
            }
            if (noSaleYearList.Count > 0)
            {
                sb.Append("<li id=\"car_nosaleyearlist\">");
                sb.Append("<a href=\"javascript:;\" class=\"arrow-down\">停售车款</a>");
                sb.Append("<div id=\"carlist_nosaleyear\" class=\"drop-layer\" style=\"display: none;\">");
                //sb.Append("<ul>");
                for (int i = 0; i < noSaleYearList.Count; i++)
                {
                    sb.AppendFormat("<a href=\"javascript:;\" id=\"{0}\">{1}</a>"
                        , noSaleYearList[i].Replace("款", "")
                        , noSaleYearList[i]);
                }
                // sb.Append("</ul>");
                sb.Append("</div>");
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.AppendFormat("<div class=\"more\"><a href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">预约试驾&gt;&gt;</a></div>", serialId);
            sb.Append("</div>");

            if (carinfoSaleList.Count > 0)
            {
                sb.AppendFormat("<div class=\"list-table\" id=\"data_tab_jq5_0\">");
                sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" id=\"compare_sale\">");
                sb.Append("<colgroup><col width=\"40%\"><col width=\"8%\"><col width=\"14%\"><col width=\"10%\"><col width=\"11%\"><col width=\"17%\"></colgroup>");
                sb.Append("<tbody>");
                sb.Append(GetCarListHtml(carinfoSaleList, maxPv));

                sb.Append("</tbody>");
                sb.Append("</table>");

                sb.Append("<div class=\"special-layout-14\" style=\"left: -75px;\" id=\"carCompareFilter\" data-channelid=\"2.21.807\">");

                sb.Append("</div>");

                sb.Append("</div>");
                InitDataByCarList(carinfoSaleList, newExhaustList);
            }

            if (isWaitSale)
            {
                sb.AppendFormat("    <div class=\"list-table\" style=\"display: {0};\" id=\"data_tab_jq5_{1}\">",
                    (carinfoSaleList.Count > 0) ? "none" : "block", isWaitSale ? 1 : 0);
                sb.Append("        <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" id=\"compare_wait\">");
                sb.Append("<colgroup><col width=\"40%\"><col width=\"8%\"><col width=\"14%\"><col width=\"10%\"><col width=\"11%\"><col width=\"17%\"></colgroup>");
                sb.Append("            <tbody>");
                sb.Append(GetCarListHtml(carinfoWaitSaleList, maxPv));
                sb.Append("            </tbody>");
                sb.Append("        </table>");
                sb.Append("    </div>");
            }
            if (carinfoNoSaleList.Count > 0)
            {
                sb.AppendFormat("    <div class=\"list-table\" id=\"data_tab_jq5_{0}\" style=\"display: none;\">", 2);
                sb.Append("        <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" id=\"compare_nosale\">");
                sb.Append("        </table>");
                sb.Append("    </div>");
            }
            sb.Append("</div>");
            carListTableHtml = sb.ToString();
        }

        /// <summary>
        /// 亮点配置
        /// </summary>
        protected void MakeSerialSparkleHtml()
        {
            SerialFourthStageBll serialFourthStageBll = new SerialFourthStageBll();
            List<SerialSparkle> list = serialFourthStageBll.GetSerialSparkle(serialId);
            if (list != null && list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class=\"special-layout-5\">");
                sb.Append("<h6>亮点配置</h6>");
                sb.Append("<ul class=\"list\">");
                foreach (SerialSparkle serialSparkle in list)
                {
                    int index = list.IndexOf(serialSparkle);
                    if (list.IndexOf(serialSparkle) == 5)
                    {
                        sb.Append("</ul><ul class=\"list\">");
                    }
                    if (index == 9) break;
                    sb.AppendFormat("<li>{0}</li>", serialSparkle.Name);
                }
                if (list.Count == 5)
                {
                    sb.Append("</ul><ul class=\"list\">");
                }
                sb.AppendFormat("<li><a data-channelid=\"2.21.1600\" href=\"/{0}/peizhi/\" class=\"more\" target=\"_blank\">更多参数配置>></a></li>", serialSpell);
                sb.Append("</ul>");
                sb.Append("</div>");
                serialSparkleHtml = sb.ToString();
            }
        }

        /// <summary>
        /// 根据车型列表获取相关数据
        /// </summary>
        /// <param name="carList"></param>
        private void InitDataByCarList(List<CarInfoForSerialSummaryEntity> carList, List<string> newExhaustList)
        {
            var listGroupNew = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
            var listGroupOff = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
            var listGroupImport = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();

            var importGroup = carList.GroupBy(p => new { p.IsImport }, p => p);
            foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in importGroup)
            {
                var key = CommonFunction.Cast(info.Key, new { IsImport = 0 });
                if (key.IsImport == 1)
                {
                    listGroupImport.Add(info);
                }
                else
                {
                    var querySale = info.ToList().GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower }, p => p);
                    foreach (IGrouping<object, CarInfoForSerialSummaryEntity> subInfo in querySale)
                    {
                        var isStopState = subInfo.FirstOrDefault(p => p.ProduceState != "停产");
                        if (isStopState != null)
                            listGroupNew.Add(subInfo);
                        else
                            listGroupOff.Add(subInfo);
                    }
                }
            }
            listGroupNew.AddRange(listGroupOff);
            listGroupNew.AddRange(listGroupImport);
            Dictionary<string, object> dictGroup = new Dictionary<string, object>();
            Dictionary<string, object> dictCarList = new Dictionary<string, object>();
            int groupIndex = 0;
            foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in listGroupNew)
            {
                //var key = CommonFunction.Cast(info.Key, new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0,IsImport=0 });
                List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList<CarInfoForSerialSummaryEntity>();//分组后的集合
                //add by 2014.05.15 组织车型数据 
                var yearTypes = carGroupList.Select(p => p.CarYear)
                    .GroupBy(year => year)
                    .Select(g => g.Key)
                    .ToArray();
                var exhausts = carGroupList
                    .Select(p => p.Engine_InhaleType.IndexOf("增压") >= 0 ? p.Engine_Exhaust.Replace("L", "T") : p.Engine_Exhaust)
                    .GroupBy(year => year)
                    .Select(g => g.Key)
                    .Select(s => s == "电动车" ? "电动" : s)
                    .ToArray();
                var trans = carGroupList.Select(p => p.TransmissionType)
                    .GroupBy(year => year)
                    .Select(g => g.Key)
                    .Select(t => t.IndexOf("手动") != -1 ? "手动" : "自动")
                    .ToArray();
                dictGroup.Add(groupIndex.ToString(), new
                {
                    YearType = yearTypes,
                    Exhaust = exhausts,
                    Transmission = trans	//serialInfo.CsTransmissionType.Split('、')
                });
                groupIndex++;
                foreach (CarInfoForSerialSummaryEntity entity in carGroupList)
                {
                    dictCarList.Add(entity.CarID.ToString(), new
                    {
                        YearType = entity.CarYear,
                        Exhaust = entity.Engine_Exhaust == "电动车" ? "电动" : (entity.Engine_InhaleType.IndexOf("增压") >= 0 ? entity.Engine_Exhaust.Replace("L", "T") : entity.Engine_Exhaust),
                        Transmission = entity.TransmissionType.IndexOf("手动") != -1 ? "手动" : "自动"
                    });
                }
            }
            if (carList.Count > 0)
            {
                var allYearList = carList.Where(p => ConvertHelper.GetInteger(p.CarYear) > 0)
                    .Select(p => p.CarYear)
                    .Distinct()
                    .ToList();
                allYearList.Sort((s1, s2) => string.Compare(s2, s1));
                //var allTransList = carList
                //    .Where(p => !string.IsNullOrEmpty(p.TransmissionType))
                //    .Select(p => p.TransmissionType)
                //    .Distinct()
                //    .ToList();
                //allTransList.Sort(NodeCompare.CompareTransmissionType);
                string[] transArray = string.IsNullOrWhiteSpace(serialInfo.CsTransmissionType) ? new string[] { } : serialInfo.CsTransmissionType.Split('、');
                var obj = new
                {
                    Year = allYearList,
                    Exhaust = newExhaustList,
                    //Trans = serialInfo.CsTransmissionType.Split('、'),
                    Trans = transArray,
                    GroupList = dictGroup,
                    CarList = dictCarList
                };
                JavaScriptSerializer js = new JavaScriptSerializer();
                carListFilterData = js.Serialize(obj);
            }
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
            var listGroupNew = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
            var listGroupOff = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
            var listGroupImport = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();

            var importGroup = carList.GroupBy(p => new { p.IsImport }, p => p);
            foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in importGroup)
            {
                var key = CommonFunction.Cast(info.Key, new { IsImport = 0 });
                if (key.IsImport == 1)
                {
                    listGroupImport.Add(info);

                }
                else
                {
                    var querySale = info.ToList().GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower }, p => p);
                    foreach (IGrouping<object, CarInfoForSerialSummaryEntity> subInfo in querySale)
                    {
                        var isStopState = subInfo.FirstOrDefault(p => p.ProduceState != "停产");
                        if (isStopState != null)
                            listGroupNew.Add(subInfo);
                        else
                            listGroupOff.Add(subInfo);
                    }
                }
            }
            listGroupNew.AddRange(listGroupOff);
            listGroupNew.AddRange(listGroupImport);

            int groupIndex = 0;
            int minChargeTime = 0;
            int maxChargeTime = 0;
            //int minFastChargeTime = 0;
            //int maxFastChargeTime = 0;
            int minMileage = 0;
            int maxMileage = 0;
            Dictionary<string, bool> mpYouHuiDic = new Dictionary<string, bool>() { { "补贴", false }, { "减税", false }, { "免税", false } };

            foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in listGroupNew)
            {
                string strMaxPowerAndInhaleType = string.Empty;
                string maxPower = string.Empty;
                string inhaleType = string.Empty;
                string exhaust = string.Empty;

                if (groupIndex == listGroupNew.Count - 1 && listGroupImport.Any())
                {
                    exhaust = "平行进口车";
                }
                else
                {
                    var key = CommonFunction.Cast(info.Key, new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0, Electric_Peakpower = 0 });

                    maxPower = key.Engine_MaxPower == 9999 ? "" : key.Engine_MaxPower + "kW";
                    inhaleType = key.Engine_InhaleType;
                    exhaust = key.Engine_Exhaust.Replace("L", "升");
                    if (!string.IsNullOrEmpty(maxPower) || !string.IsNullOrEmpty(inhaleType))
                    {
                        if (inhaleType == "增压")
                        {
                            inhaleType = string.IsNullOrEmpty(key.Engine_AddPressType) ? inhaleType : key.Engine_AddPressType;
                        }
                        if (key.Electric_Peakpower > 0)
                        {
                            maxPower = string.Format("发动机：{0}，发电机：{1}", maxPower, key.Electric_Peakpower + "kW");
                        }
                        strMaxPowerAndInhaleType = string.Format("<b>/</b>{0}{1}", maxPower, " " + inhaleType);
                    }
                }

                carListHtml.Add(string.Format("<tr id=\"car_filter_gid_{0}\" class=\"table-tit\">", groupIndex));
                carListHtml.Add("    <th class=\"first-item\">");
                carListHtml.Add(string.Format("<strong>{0}</strong> {1}",
                    exhaust,
                    strMaxPowerAndInhaleType));
                carListHtml.Add("    </th>");
                carListHtml.Add("    <th>关注度</th>");
                carListHtml.Add("    <th>变速箱</th>");
                carListHtml.Add("    <th class=\"txt-right txt-right-padding\">" + (waitStateFlag ? "预售价" : "指导价") + "</th>");
                carListHtml.Add("    <th class=\"txt-right\">参考最低价</th>");
                carListHtml.Add("    <th></th>");
                //carListHtml.Add("    <th><div class=\"doubt\" onmouseover=\"javascript:$(this).children('.prompt-layer').show();return false;\" onmouseout=\"javascript:$(this).children('.prompt-layer').hide();return false;\">");
                //carListHtml.Add("    <div class=\"prompt-layer\"  style=\"display:none;\">全国参考最低价</div></div></th>");
                carListHtml.Add("</tr>");

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
                        stopPrd = " <span class=\"color-block3\">停产</span>";

                    //新车上市 即将上市 状态
                    string marketflag = _serialBLL.GetCarMarketText(entity.CarID, entity.SaleState, entity.MarketDateTime, entity.ReferPrice);//GetMarketFlag(entity);
                    if (!string.IsNullOrEmpty(marketflag))
                    {
                        marketflag = string.Format("<a target=\"_blank\" class=\"color-block\">{0}</a>", marketflag);
                    }
                    Dictionary<int, string> dictCarParams = _carBLL.GetCarAllParamByCarID(entity.CarID);
                    //add by 2014.05.04 获取电动车参数
                    if (isElectrombile)
                    {
                        //普通充电时间
                        if (dictCarParams.ContainsKey(879))
                        {
                            var chargeTime = ConvertHelper.GetInteger(dictCarParams[879]);
                            if (minChargeTime == 0 && chargeTime > 0)
                                minChargeTime = chargeTime;
                            if (chargeTime < minChargeTime)
                                minChargeTime = chargeTime;
                            if (chargeTime > maxChargeTime)
                                maxChargeTime = chargeTime;
                        }
                        ////快速充电时间
                        //if (dictCarParams.ContainsKey(878))
                        //{
                        //    var fastChargeTime = ConvertHelper.GetInteger(dictCarParams[878]);
                        //    if (minFastChargeTime == 0 && fastChargeTime > 0)
                        //        minFastChargeTime = fastChargeTime;
                        //    if (fastChargeTime < minFastChargeTime)
                        //        minFastChargeTime = fastChargeTime;
                        //    if (fastChargeTime > maxFastChargeTime)
                        //        maxFastChargeTime = fastChargeTime;
                        //}
                        //纯电最高续航里程
                        if (dictCarParams.ContainsKey(883))
                        {
                            var mileage = ConvertHelper.GetInteger(dictCarParams[883]);
                            if (minMileage == 0 && mileage > 0)
                                minMileage = mileage;
                            if (mileage < minMileage)
                                minMileage = mileage;
                            if (mileage > maxMileage)
                                maxMileage = mileage;
                        }
                    }
                    // 节能补贴 Sep.2.2010
                    string hasEnergySubsidy = "";
                    //补贴功能临时去掉 modified by chengl Oct.24.2013
                    //modified by sk 2015-01-30 只有第七 、 八批次 补贴
                    if ((dictCarParams.ContainsKey(963) && (dictCarParams[963] == "第10批")) && dictCarParams.ContainsKey(853) && dictCarParams[853] == "3000元")
                    {
                        hasEnergySubsidy = " <a name=\"butie\" href=\"http://news.bitauto.com/sum/20160331/0206583470.html\" class=\"color-block2\" title=\"可获得3000元节能补贴\" target=\"_blank\">补贴</a>";
                        mpYouHuiDic["补贴"] = true;
                    }
                    //============2016-02-26 减税 购置税============================
                    string strTravelTax = "";
                    double dEx = 0.0;
                    Double.TryParse(entity.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
                    if (entity.SaleState == "在销")
                    {
                        if (dictCarParams.ContainsKey(987) && (dictCarParams[987] == "第1批" || dictCarParams[987] == "第2批" || dictCarParams[987] == "第3批" || dictCarParams[987] == "第4批" || dictCarParams[987] == "第5批" || dictCarParams[987] == "第6批") && dictCarParams.ContainsKey(986))
                        {
                            strTravelTax = " <a target=\"_blank\" title=\"{0}\" href=\"http://news.bitauto.com/sum/20170105/1406774416.html\" class=\"color-block2\">{1}</a>";
                            if (dictCarParams[986].ToString() == "减半")
                            {
                                strTravelTax = string.Format(strTravelTax, "购置税减半", "减税", entity.CarID);
                                mpYouHuiDic["减税"] = true;
                            }
                            else if (dictCarParams[986].ToString() == "免征")
                            {
                                strTravelTax = string.Format(strTravelTax, "免征购置税", "免税", entity.CarID);
                                mpYouHuiDic["免税"] = true;
                            }
                        }
                        else if (dEx > 0 && dEx <= 1.6)
                        {
                            strTravelTax = " <a id=\"carlist-tag-tax-" + entity.CarID + "\" target=\"_blank\" title=\"购置税75折\" href=\"http://news.bitauto.com/sum/20170105/1406774416.html\" class=\"color-block2\">减税</a>";
                            mpYouHuiDic["减税"] = true;
                        }
                    }

                    //平行进口车标签
                    string parallelImport = "";

                    //混动车标签
                    string fuelTypeStr = "";
                    if (entity.Oil_FuelType == "油电混合" || entity.Oil_FuelType == "插电混合")
                    {
                        fuelTypeStr = "<span class=\"color-block2\">混动</span>";
                    }
                    carListHtml.Add(string.Format("<tr  id=\"car_filter_id_{0}\">", entity.CarID));
                    carListHtml.Add(string.Format("<td class=\"txt-left\" id=\"carlist_{0}\">", entity.CarID));
                    carListHtml.Add(string.Format("<a href=\"/{0}/m{1}/\" data-channelid=\"2.21.848\" target=\"_blank\" class=\"txt\">{2} {3}</a><a href=\"/{0}/m{1}/\" data-channelid=\"2.21.848\" target=\"_blank\" class=\"abs-a\"></a> {4}",
                        serialSpell, entity.CarID, yearType, entity.CarName, fuelTypeStr + hasEnergySubsidy + strTravelTax + parallelImport + stopPrd + marketflag));
                    carListHtml.Add("</td>");
                    carListHtml.Add("<td>");
                    carListHtml.Add("    <div class=\"w\">");
                    //计算百分比

                    int percent = (int)Math.Round((double)entity.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero);

                    carListHtml.Add(string.Format("        <div class=\"p\" style=\"width: {0}%\"></div>", percent));
                    carListHtml.Add("    </div>");
                    carListHtml.Add("</td>");
                    // 档位个数
                    //string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" && dictCarParams[724] != "待查") ? dictCarParams[724] + "挡" : "";
                    string transmissionType = _carBLL.GetCarTransmissionType(dictCarParams.ContainsKey(724) ? dictCarParams[724] : string.Empty, entity.TransmissionType);

                    carListHtml.Add(string.Format("<td>{0}</td>", transmissionType));
                    carListHtml.Add(string.Format("<td class=\"txt-right overflow-visible\"><span>{0}</span><a carid=\"{1}\" data-channelid=\"2.21.852\" class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid={1}\" target=\"_blank\"{2}></a></td>"
                        , string.IsNullOrEmpty(entity.ReferPrice) ? "暂无" : entity.ReferPrice + "万"
                        , entity.CarID
                        , string.IsNullOrWhiteSpace(entity.ReferPrice) ? " title=\"购车计算器\"" : " data-use=\"compute\""));
                    if (entity.CarPriceRange.Trim().Length == 0)
                        carListHtml.Add("    <td  class=\"txt-right\"><span class=\"grey-txt\">暂无报价</span></td>");
                    else
                    {
                        //取最低报价
                        string minPrice = entity.CarPriceRange;
                        if (entity.CarPriceRange.IndexOf("-") != -1)
                            minPrice = entity.CarPriceRange.Substring(0, entity.CarPriceRange.IndexOf('-'));

                        carListHtml.Add(string.Format("<td class=\"txt-right\"><span><a href=\"/{0}/m{1}/baojia/\" target=\"_blank\">{2}</a></span></td>", serialSpell, entity.CarID, minPrice));
                    }
                    carListHtml.Add("<td class=\"txt-right\">");
                    carListHtml.Add(string.Format("<a class=\"btn btn-primary btn-xs\" data-channelid=\"2.21.849\" href=\"http://dealer.bitauto.com/zuidijia/nb{0}/nc{1}/?T=2&leads_source=p002011\" target=\"_blank\">询底价</a> "
                        , serialId
                        , entity.CarID));
                    //carListHtml.Add(string.Format("<a class=\"btn btn-xs\" data-channelid=\"2.21.1342\" href=\"http://item.huimaiche.com/{0}/{1}/?tracker_u=612_ckzs\" target=\"_blank\">买车</a</div>"
                    //    , serialSpell
                    //    , entity.CarID));
                    carListHtml.Add(string.Format("<a class=\"btn btn-secondary btn-xs\" data-use=\"compare\" data-id=\"{0}\" href=\"javascript:;\" data-channelid=\"2.21.850\"><span>+对比</span></a>", entity.CarID));
                    carListHtml.Add("</td>");
                    carListHtml.Add("</tr>");
                }
            }
            //add by 2014.05.04 电动车 参数
            if (maxChargeTime > 0)
            {
                chargeTimeRange = minChargeTime == maxChargeTime ? string.Format("{0}小时", minChargeTime) : string.Format("{0}-{1}小时", minChargeTime, maxChargeTime);
            }
            //if (maxFastChargeTime > 0)
            //{
            //    fastChargeTimeRange = minFastChargeTime == maxFastChargeTime ? string.Format("{0}分钟", minFastChargeTime) : string.Format("{0}-{1}分钟", minFastChargeTime, maxFastChargeTime);
            //}
            if (maxMileage > 0)
            {
                mileageRange = minMileage == maxMileage ? string.Format("{0}公里", minMileage) : string.Format("{0}-{1}公里", minMileage, maxMileage);
            }
            foreach (KeyValuePair<string, bool> kv in mpYouHuiDic)
            {
                if (kv.Value)
                {
                    mpSerialYouHuiHtml += string.Format("<a target=\"_blank\" href=\"http://news.bitauto.com/sum/20170105/1406774416.html\" class=\"lower-tax\">{0}</a>", kv.Key);
                }
            }
            return string.Concat(carListHtml.ToArray());
        }       

        /// <summary>
        /// 焦点图片块
        /// </summary>
        private void MakeFocusImages()
        {
            StringBuilder sb = new StringBuilder();
            var focusImgId = new Dictionary<int, int>();
            #region 焦点图 大图 颜色
            //子品牌焦点图
            List<SerialFocusImage> imgList = _serialBLL.GetSerialFocusImageList(serialId);
            //子品牌幻灯页
            List<SerialFocusImage> imgSlideList = _serialBLL.GetSerialSlideImageList(serialId);
            List<SerialFocusImage> sourceList = new List<SerialFocusImage>();
            int resultCount = 0;
            List<SerialColorEntity> serialColorList;
            if (serialInfo.CsSaleState == "停销")
            {
                serialColorList = _serialBLL.GetNoSaleSerialColors(serialId, serialInfo.ColorList);
            }
            else
            {
                serialColorList = _serialBLL.GetProduceSerialColors(serialId);
            }
            List<SerialColorForSummaryEntity> colorList = _serialBLL.GetSerialColorRGBByCsID(serialId, 0, serialColorList);
            //排序 有图在前 无图在后 颜色 按色值大小从大到小排序
            colorList.Sort(NodeCompare.SerialColorCompare);

            List<string> smallImages = new List<string>();
            sb.Append("<div class=\"img-box focus-img-pos\" id=\"focus_images\" data-channelid=\"2.21.794\">");

            List<string> bigImage = new List<string>();           
            int picCount = 0;
            if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("A"))
            {
                picCount = dsCsPic.Tables["A"].AsEnumerable().Sum(row => ConvertHelper.GetInteger(row["N"]));
            }
            #region 初始化数据源
            foreach (SerialFocusImage img in imgList)
            {
                sourceList.Add(img);
            }
            //焦点图不足，补幻灯页
            foreach (SerialFocusImage imgS in imgSlideList)
            {    
                if(sourceList.Count >= 4)
                {
                    break;
                }
                //焦点图片排重
                SerialFocusImage focusImage = imgList.Find(p => p.ImageId == imgS.ImageId);
                if (focusImage != null)
                    continue;
                sourceList.Add(imgS);
            }
            //取图解第一张
            XmlNode firstTujieNode = this.GetFirstTujieImage(dsCsPic);
            List<int> categoryIdList = new List<int>() { 30, 100, 101 };
            List<VideoEntity> videoList = VideoBll.GetVideoBySerialIdAndCategoryId(serialId, categoryIdList, 1);
            #endregion
            //大图 默认第一张 焦点图第一张 没有焦点图 幻灯图
            bool showTujie = false;
            bool showShipin = false;
            if (sourceList.Count > 0)
            {
                SerialFocusImage csImg = sourceList[0];
                string firstFocusImage = csImg.ImageUrl;
                if (csImg.ImageId > 0)
                {
                    firstFocusImage = String.Format(firstFocusImage, 4);
                }

                bigImage.Insert(0,
                    string.Format(
                        "{2}<div id=\"focus_image_first\" style=\"display:block\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" alt=\"\"/></a></div>",
                        csImg.TargetUrl,
                    firstFocusImage.Replace("_4.", "_3."),
                    picCount > 0 ? string.Format("<a target=\"_blank\" href=\"{1}\" class=\"img-link\">共{0}张图片&gt;&gt;</a>",
                                    picCount,
                                    "http://photo.bitauto.com/serial/" + serialId)
                                    : string.Empty));
                resultCount++;
                if (!focusImgId.ContainsKey(csImg.ImageId))
                {
                    focusImgId.Add(csImg.ImageId, 0);
                }
            }
            else
            {
                //大图无图片 补图解 无图解 补视频
                if (firstTujieNode != null)
                {
                    bigImage.Insert(0, string.Format("{2}<div id=\"focus_image_first\" style=\"display:block\"><a href=\"{0}\" target =\"_blank\"><img src=\"{1}\" alt=\"\" /></a></div>",
                        firstTujieNode.Attributes["Link"].Value,
                        firstTujieNode.Attributes["ImageUrl"].Value,
                        picCount > 0 ? (string.Format("<a target=\"_blank\" href=\"{1}\" class=\"img-link\">共{0}张图片&gt;&gt;</a>",
                                picCount,
                                "http://photo.bitauto.com/serial/" + serialId))
                                : string.Empty));
                    showTujie = true;
                    resultCount++;
                    if (!focusImgId.ContainsKey(Convert.ToInt32(firstTujieNode.Attributes["ImageId"].Value)))
                    {
                        focusImgId.Add(Convert.ToInt32(firstTujieNode.Attributes["ImageId"].Value), 0);
                    }
                }
                else
                {
                    if (videoList.Count > 0)
                    {
                        string imgUrl = videoList[0].ImageLink;
                        imgUrl = imgUrl.Replace("/bitauto/", "/newsimg-150-w0-1-q50/bitauto/");
                        imgUrl = imgUrl.Replace("/Video/", "/newsimg-150-w0-1-q50/Video/");
                        bigImage.Insert(0, string.Format("{2}<div id=\"focus_image_first\" style=\"display:block\"><a href=\"{0}\" target =\"_blank\"><img src=\"{1}\" alt=\"\" /></a></div>",
                       videoList[0].ShowPlayUrl,
                       imgUrl,
                       picCount > 0 ? (string.Format("<a target=\"_blank\" href=\"{1}\" class=\"img-link\">共{0}张图片&gt;&gt;</a>",
                               picCount,
                               "http://photo.bitauto.com/serial/" + serialId))
                               : string.Empty));
                        showShipin = true;
                        resultCount++;
                    }
                }
            }
            if (colorList.Count > 0)
            {
                var firstFocusImageId = 0;
                if (imgList != null && imgList.Count > 0) firstFocusImageId = imgList[0].ImageId;
                //颜色补充实拍图
                string serialColorPath = Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialColorImagePath, serialId));
                XmlDocument xmlSerialColor = CommonFunction.ReadXmlFromFile(serialColorPath);
                int loop = 0;
                foreach (SerialColorForSummaryEntity color in colorList)
                {
                    string imageUrl = color.ImageUrl.Replace("_5.", "_3.");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        XmlNode colorNode = xmlSerialColor.SelectSingleNode("/CarImageList/CarImage[@ColorName='" + color.ColorName + "']");
                        if (colorNode != null)
                        {
                            imageUrl = string.Format(colorNode.Attributes["ImageUrl"].Value, 3);
                            color.Link = colorNode.Attributes["Link"].Value;
                        }
                    }
                    if (string.IsNullOrEmpty(imageUrl))
                        continue;
                    loop++;
                    bigImage.Add(string.Format("<div id=\"focuscolor_{3}\" style=\"display:{2}\"><a href=\"{0}\" target=\"_blank\"><img data-original=\"{1}\" alt=\"\"/></a></div>",
                        color.Link + "#" + firstFocusImageId,
                        imageUrl,
                         "none", loop));
                    smallImages.Add(string.Format("<li><a href=\"{0}\" title=\"{2}\" target=\"_blank\"><span style=\"background:{1}\"></span></a></li>",
                        color.Link + "#" + firstFocusImageId, color.ColorRGB, color.ColorName));
                }
            }

            sb.Append(string.Concat(bigImage.ToArray()));
            sb.Append("</div>");
            sb.Append("<div class=\"img-cor-box\" data-channelid=\"2.21.798\" id=\"focus_color_box\">");
            sb.Append("<div class=\"focus-color-box\">");
            if (smallImages.Count > 8)
            {
                sb.Append("    <div id=\"focus_color_l\" class=\"l-btn noclick\"></div>");
                sb.Append("    <div id=\"focus_color_r\" class=\"r-btn\"></div>");
            }
            sb.Append("<div class=\"focus-color-warp\">");
            //if (smallImages.Count <= 8)
            //	sb.AppendFormat("<ul id=\"color-listbox\" style=\"width:{0}px;\">", smallImages.Count * 23);
            //else
            sb.Append("<ul id=\"color-listbox\" style=\"top: 0px; left: 0px;position: absolute;\">");
            sb.Append(string.Concat(smallImages.ToArray()));
            sb.Append("    </ul>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            #endregion
            sb.Append("<div class=\"row img-list\">");
           
            #region 焦点图片 下面3张小图 ;第二张有图解展示图解，第三张有视频展示视频
            bool hasTujie = false;
            for (int i = 1; i < 4; i++)
            {
                int channelId = 794 + resultCount;
                //第二张图 有图解显示图解
                if (i == 2 && showTujie == false)
                {                  
                    if (firstTujieNode != null && !focusImgId.ContainsKey(Convert.ToInt32(firstTujieNode.Attributes["ImageId"].Value)))
                    {
                        string groupName = firstTujieNode.Attributes["GroupName"].Value;
                        sb.AppendFormat("<div data-channelid=\"2.21.{4}\" class=\"img-link col-auto\"><a href=\"{0}\" target=\"_blank\">{3}<img src=\"{1}\" title=\"{2}\" alt=\"{2}\" width=\"140\" height=\"93\" /></a></div>",
                            firstTujieNode.Attributes["Link"].Value,
                             firstTujieNode.Attributes["ImageUrl"].Value,
                            firstTujieNode.Attributes["ImageName"].Value,
                            string.IsNullOrEmpty(groupName) ? string.Empty : "<i>" + groupName + ">></i>",
                            channelId);
                        hasTujie = true;
                        resultCount++;
                        if (!focusImgId.ContainsKey(Convert.ToInt32(firstTujieNode.Attributes["ImageId"].Value)))
                        {
                            focusImgId.Add(Convert.ToInt32(firstTujieNode.Attributes["ImageId"].Value), i);
                        }
                        continue;
                    }
                }
                //第三张图 有视频展示视频
                if (i == 3 && videoList.Count > 0 && showShipin == false)
                {
                    string imgUrl = videoList[0].ImageLink;
                    imgUrl = imgUrl.Replace("/bitauto/", "/newsimg-150-w0-1-q50/bitauto/");
                    imgUrl = imgUrl.Replace("/Video/", "/newsimg-150-w0-1-q50/Video/");
                    sb.AppendFormat("<div data-channelid=\"2.21.{3}\" class=\"img-link img-current col-auto\"><a href=\"{0}\" target=\"_blank\"><i>视频>></i><em></em><img src=\"{1}\" title=\"{2}\" alt=\"{2}\" width=\"140\" height=\"93\" /></a></div>",
                        videoList[0].ShowPlayUrl, imgUrl, videoList[0].ShortTitle,channelId);
                    resultCount++;
                    continue;
                }
                if (sourceList.Count > i || (hasTujie && sourceList.Count == 3))
                {

                    SerialFocusImage csImg = hasTujie == true ? sourceList[i - 1] : sourceList[i];
                    string smallImgUrl = csImg.ImageUrl;
                    if (csImg.ImageId > 0)
                    {
                        smallImgUrl = String.Format(smallImgUrl, 4);
                    }
                    sb.AppendFormat("<div data-channelid=\"2.21.{4}\" class=\"img-link col-auto\"><a href=\"{0}\" target=\"_blank\">{3}<img src=\"{1}\" title=\"{2}\" alt=\"{2}\" width=\"140\" height=\"93\" /></a></div>",
                        csImg.TargetUrl,
                        smallImgUrl,
                        csImg.ImageName,
                        string.IsNullOrEmpty(csImg.GroupName) ? string.Empty : "<i>" + csImg.GroupName + ">></i>",
                        channelId);
                    resultCount++;
                    if (!focusImgId.ContainsKey(csImg.ImageId))
                    {
                        focusImgId.Add(csImg.ImageId, i);
                    }
                }
            }
            #endregion
            sb.Append("</div>");
            focusImagesHtml = sb.ToString();
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
            if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("C") && dsCsPic.Tables["C"].Rows.Count > 0)
            {
                var rows = dsCsPic.Tables["C"].Rows.Cast<DataRow>();
                DataRow row = rows.FirstOrDefault(dr => ConvertHelper.GetInteger(dr["P"]) == 12); //dt.Select("P='" + cateId + "'");
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
            return (XmlNode)element;
        }

        /// <summary>
        /// 看了还看
        /// </summary>
        /// <returns></returns>
        private void MakeSerialToSerialHtml()
        {
            serialToSeeJson = _serialBLL.GetSerialSeeToSeeJson(serialId, 6, 3);
        }

        /// <summary>
        /// 综合对比
        /// </summary>
        /// <returns></returns>
        private void GetSerialHotCompareCars()
        {
            StringBuilder sb = new StringBuilder();
            //List<EnumCollection.SerialHotCompareData> lshcd = base.GetSerialHotCompareByCsID(serialId, 6);
            Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = _serialBLL.GetSerialCityCompareList(serialId, HttpContext.Current);
            if (carSerialBaseList != null && carSerialBaseList.ContainsKey("全国"))
            {
                var dicAllCsPic = GetAllSerialPicURL(true);
                List<Car_SerialBaseEntity> serialBaseEntityList = carSerialBaseList["全国"];
                var serialCompareForPkUrl = string.Concat("/duibi/", serialId, "-{0}/");
                sb.Append("<div class=\"compare-sidebar\" data-channelid=\"2.21.833\">");
                sb.Append("            <h3 class=\"top-title\"><a href=\"http://car.bitauto.com/duibi/\" target=\"_blank\">综合对比</a></h3>");
                sb.Append("    <div class=\"col2-140-box clearfix\">");
                foreach (Car_SerialBaseEntity carSerial in serialBaseEntityList)
                {
                    sb.Append("        <div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-14093\">");
                    sb.Append("            <div class=\"img\">");
                    sb.AppendFormat("                <a href=\"{1}\" target=\"_blank\"><img src=\"{0}\"></a>"
                        , dicAllCsPic.ContainsKey(carSerial.SerialId) ? dicAllCsPic[carSerial.SerialId].Replace("_2.jpg", "_3.jpg") : WebConfig.DefaultCarPic
                        , string.Format(serialCompareForPkUrl, carSerial.SerialId));
                    sb.Append("            </div>");
                    sb.Append("            <ul class=\"p-list\">");
                    sb.AppendFormat("                <li class=\"name no-wrap\"><a href=\"{1}\" title=\"{0}\" target=\"_blank\"><span>VS</span> {0}</a></li>"
                        , carSerial.SerialShowName.Trim()
                        , string.Format(serialCompareForPkUrl, carSerial.SerialId));
                    sb.Append("            </ul>");
                    sb.Append("        </div>");
                }
                sb.Append("    </div>");
                sb.Append("</div>");
            }
            CsHotCompareCars = sb.ToString();
        }

        /// <summary>
        /// 得到品牌下的其他子品牌
        /// </summary>
        /// <returns></returns>
        private void MakeBrandOtherSerial()
        {
            brandOtherSerial = new Car_BrandBll().GetBrandOtherSerial(serialEntity.BrandId, serialEntity.Id);
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
            serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
            serialSpell = serialEntity.AllSpell;
            serialSeoName = serialEntity.SeoName;
            serialShowName = serialEntity.ShowName;
            serialName = serialEntity.Name;
            serialPrice = serialInfo.CsPriceRange;
            if (serialPrice.Length == 0)
            {
                serialMinPrice = 0;
                serialPrice = "暂无报价";
            }
            else if (serialPrice.IndexOf('-') > -1)
                serialMinPrice = ConvertHelper.GetDouble(serialPrice.Split('-')[0]);
            else serialMinPrice = ConvertHelper.GetDouble(serialPrice.Replace("万", ""));

            serialTransmission = string.Join("/", serialInfo.CsTransmissionType.Split('、'));

            wirelessSerialUrl = string.Format("http://car.m.yiche.com/{0}/", serialSpell);
            baseUrl = "/" + serialSpell.ToLower() + "/";

            //通用导航头
            serialHeaderHtml = base.GetCommonNavigation("CsSummary", serialId);
            //导航脚本
            serialHeaderJs = base.GetCommonNavigation("CsSummaryJs", serialId);
            //图库接口本地化更改
            string xmlPicPath = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialPhotoListPath, serialId));
            // 此 Cache 将通用于图片页和车型综述页
            dsCsPic = this.GetXMLDocToDataSetByURLForCache("CarChannel_SerialAllPic_" + serialId.ToString(), xmlPicPath, 10);

            //论坛url
            baaUrl = _serialBLL.GetForumUrlBySerialId(serialId);

            //静态块内容
            dictSerialBlockHtml = _commonhtmlBLL.GetCommonHtml(serialId, CommonHtmlEnum.TypeEnum.Serial, CommonHtmlEnum.TagIdEnum.SerialSummaryNew);
            //同级别热度
            int serialUvRank = _serialBLL.GetAllSerialUvRank30Days(serialId);
            if (serialUvRank > 0)
            {
                serialTotalPV = string.Format("第{0}名", serialUvRank);
            }

            var tempCarList = serialEntity.CarList;//车型列表
            if (tempCarList.Any())
            {
                var noSaleCarList = tempCarList.Where(s => s.SaleState == "停销").ToList();
                if (noSaleCarList.Any())
                {
                    var lastYear = noSaleCarList.Select(s => s.CarYear).Max();//从车型列表中获取最新年款
                    var lastList = noSaleCarList.Where(s => s.CarYear == lastYear).ToList();//筛选最新年款数据
                    if (lastList.Any())
                    {
                        var priceList = lastList.Select(s => s.ReferPrice).ToList();//得到最新年款的价格集合

                        if (priceList.Any())
                        {
                            var min = priceList.Min();
                            var max = priceList.Max();

                            if (min == 0 && max == 0)
                            {
                                noSaleLastReferPrice = "暂无";
                            }
                            else
                            {
                                noSaleLastReferPrice = min == max ? string.Format("{0}万", min) : string.Format("{0}-{1}万", min, max);
                            }
                        }
                    }
                }
                else
                {
                    noSaleLastReferPrice = "暂无";
                }
            }
        }

        /// <summary>
        /// 碰撞数据
        /// </summary>
        private void GetCNCAPAndENCAPData()
        {
            Dictionary<int, List<CNCAPEntity>> CNCAPAndENCAPList = _serialBLL.GetCNCAPAndENCAPData();
            StringBuilder sbCNCAPAndENCAP = new StringBuilder();
            if (CNCAPAndENCAPList.ContainsKey(serialId))
            {
                List<CNCAPEntity> cnlist = CNCAPAndENCAPList[serialId];
                CNCAPEntity CNCAPEntity = cnlist.Find(x => x.ParamId == 649);
                if (CNCAPEntity != null) sbCNCAPAndENCAP.Append("C-NCAP ").Append(CastNCAPValue(CNCAPEntity.ParamValue));
                CNCAPEntity ENCAPEntity = cnlist.Find(x => x.ParamId == 637);
                if (ENCAPEntity != null) sbCNCAPAndENCAP.Append(sbCNCAPAndENCAP.Length > 0 ? "," : string.Empty).Append("E-NCAP ").Append(CastNCAPValue(ENCAPEntity.ParamValue));

                if (!string.IsNullOrWhiteSpace(serialEntity.AnQuan))
                {
                    CNCAPAndENCAPStr = string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", serialEntity.AnQuan, sbCNCAPAndENCAP.ToString());
                }
                else
                {
                    CNCAPAndENCAPStr = sbCNCAPAndENCAP.ToString();
                }
            }
        }

        private string CastNCAPValue(string value)
        {
            switch (value.Trim())
            {
                case "五星": return "5星";
                case "四星": return "4星";
                case "三星": return "3星";
                case "二星": return "2星";
                case "一星": return "1星";
            }
            return string.Empty;
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
        /*
        private string GetMarketFlag(CarInfoForSerialSummaryEntity entity)
        {
            string marketflag = "";

            if (entity != null)
            {
                //int res =DateTime.Compare(entity.MarketDateTime, DateTime.MinValue);
                if (DateTime.Compare(entity.MarketDateTime, DateTime.MinValue) != 0)
                {
                    int days = GetDaysAboutCurrentDateTime(entity.MarketDateTime);
                    if (days >= 0 && days <= 30)
                    {
                        if (entity.SaleState.Trim() == "在销")
                        {
                            marketflag = "<a target=\"_blank\" class=\"color-block\">新上市</a>";
                        }                        
                    }
                    else if (days >= -30 && days < 0)
                    {
                        if (entity.SaleState.Trim() == "待销")
                        {
                            marketflag = "<a target=\"_blank\" class=\"color-block\">即将上市</a>";
                        }                            
                    }
                }
                else
                {
                    if (entity.SaleState.Trim() == "待销")
                    {
                        var picCount = _carBLL.GetSerialCarRellyPicCount(entity.CarID);
                        if (picCount > 0)
                        {
                            marketflag = "<a target=\"_blank\" class=\"color-block\">即将上市</a>";
                        }
                        else
                        {
                            if (entity.ReferPrice != "")
                            {
                                marketflag = "<a target=\"_blank\" class=\"color-block\">即将上市</a>";
                            }
                        }
                    }
                }
            }           

            return marketflag;
        }
        
        public int GetDaysAboutCurrentDateTime(DateTime dt)
        {
            DateTime currentDateTime = DateTime.Now.Date;
            int days = (currentDateTime - dt).Days;
            return days;
        }

    */
    }
}