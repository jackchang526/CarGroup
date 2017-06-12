using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WirelessWeb
{
    public partial class CsSummaryV2 : WirelessPageBase
    {
        private const int pageSize = 7;
        private readonly Car_BasicBll _carBLL;
        private readonly CommonHtmlBll _commonhtmlBLL;
        private readonly Car_SerialBll serialBLL;
        private CarNewsBll _newsBLL;
        protected List<SerialColorEntity> SerialColorList;
        protected List<VideoEntity> VideoList = null;
        protected string nearestYear = string.Empty;//所有停售车款里的最近年份
        /// <summary>
        ///     meta-Description
        /// </summary>
        protected string _Description;
        /// <summary>
        ///     车型详解区域html
        /// </summary>
        protected string _carDetailsViewZoneHtml;
        /// <summary>
        ///     车型列表html
        /// </summary>
        protected string _carList;

        protected string _chanDi;
        protected string _editorCommentHtml = string.Empty;
        protected string _forumHotNewsHtml = string.Empty;
        protected int maxPv = 0;
        /// <summary>
        ///     论坛新闻列表
        /// </summary>
        protected string _forumNewsHtml = string.Empty;

        private bool _isYearEnabled;
        protected bool _wenZhangShowFlag = true;

        /// <summary>
        ///     meta-Keywords
        /// </summary>
        protected string _keyWords;

        protected string _serialAllSpell;

        /// <summary>
        ///     车身+车门数+厢体
        /// </summary>
        protected string _serialBody;

        protected string _serialBrandName;
        private List<CarInfoForSerialSummaryEntity> _serialCarList = new List<CarInfoForSerialSummaryEntity>();

        /// <summary>
        ///     颜色
        /// </summary>
        protected string _serialColor;

        protected string _serialDaogou = string.Empty;

        /// <summary>
        ///     排量
        /// </summary>
        protected string _serialDisplacement;

        protected SerialEntity _serialEntity;
        /// <summary>
        /// 是否是全系电动车
        /// </summary>
        protected bool isElectrombile = false;

        /// <summary>
        /// 车系为电动车的续航里程区间
        /// </summary>
        protected string mileageRange = string.Empty;
        /// <summary>
        ///     焦点区文章
        /// </summary>
        protected string _serialFocusNews;

        /// <summary>
        ///     油耗
        /// </summary>
        protected string _serialFuelCost;

        protected int _serialId;
        protected string _baaUrl = string.Empty;

        /// <summary>
        ///     焦点图
        /// </summary>
        protected string _serialImage;

        protected EnumCollection.SerialInfoCard _serialInfoCard; //子品牌名片
        protected string _serialInfoHtml = string.Empty;

        /// <summary>
        ///     级别
        /// </summary>
        protected string _serialLevel;

        /// <summary>
        ///     养护
        /// </summary>
        protected string _serialMaintenance;

        /// <summary>
        ///     买车必看
        /// </summary>
        protected string _serialMustWatch;

        protected string _serialName;
        protected string _serialNavName;

        /// <summary>
        ///     最新文章-热门
        /// </summary>
        protected string _serialNews;

        //车型详解
        protected string _serialPingce = string.Empty;

        /// <summary>
        ///     商家报价
        /// </summary>
        protected string _serialPrice;

        /// <summary>
        ///     厂商
        /// </summary>
        protected string _serialProducer;

        /// <summary>
        ///     指导价
        /// </summary>
        protected string _serialRefPrice;

        /// <summary>
        ///     人数
        /// </summary>
        protected string _serialSeatNum;

        protected string _serialSeoName;
        protected string _serialShowName;

        /// <summary>
        ///     看了还看
        /// </summary>
        protected string _serialToSee;

        //全系导购

        /// <summary>
        ///     关注度
        /// </summary>
        protected string _serialTotalPV = string.Empty;

        /// <summary>
        ///     变速箱
        /// </summary>
        protected string _serialTransmission;

        protected int _serialYear;

        /// <summary>
        ///     页面title
        /// </summary>
        protected string _title;

        protected int _yearCount;
        protected Dictionary<int, Dictionary<int, string>> dicPicNoneWhite;
        private Dictionary<int, string> dictSerailTuanUrl; //团购链接 字典  
        private Dictionary<int, string> dictSerialBlockHtmlV2; //静态块内容 (新增)口碑+详情视图区域

        private Dictionary<int, string> dictSerialFocusSummaryBlockHtml; //焦点区要闻
        private Dictionary<int, string> dictUCarPrice; //二手车最低价格
        protected string uCarLowestPrice = string.Empty;  //二手车最低具体价格

        private Dictionary<int, string> dictUCarPriceRange; //二手车价格区间
        protected string uCarPriceRange = string.Empty;  //二手车价格区间

        private DataSet dsCsPic;
        public List<string> focusImg = new List<string>();
        private Dictionary<int, XmlNode> dicSerialToSerial; //看了又看比它大，比它省油
        /// <summary>
        ///     口碑印象
        /// </summary>
        protected string koubeiImpressionHtml = string.Empty;
        protected string CsHeadHTML = string.Empty;

        protected int pageCount = 0;

        /// <summary>
        /// 文章-评测 是否大于3条
        /// </summary>
        protected bool pingceLiFlag = false;
        /// <summary>
        /// 文章-导购 是否大于3条
        /// </summary>
        protected bool daogouLiFlag = false;
        public CsSummaryV2()
        {
            serialBLL = new Car_SerialBll();
            _carBLL = new Car_BasicBll();
            _commonhtmlBLL = new CommonHtmlBll();
            _newsBLL = new CarNewsBll();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageCache(30);
            GetParamter();
            if (_serialEntity == null || _serialEntity.Id <= 0)
            {
                Response.Redirect("/404error.aspx");
                return;
            }
            string xmlPicPath = Path.Combine(PhotoImageConfig.SavePath,
                string.Format(PhotoImageConfig.SerialPhotoListPath, _serialId));
            dsCsPic = GetXMLDocToDataSetByURLForCache("CarChannel_SerialAllPic_" + _serialEntity.Id, xmlPicPath, 60);
            dicPicNoneWhite = base.GetAllSerialPicNoneWhiteBackground();
            dictUCarPrice = serialBLL.GetUCarSerialLowPrice();
            uCarLowestPrice = dictUCarPrice.ContainsKey(_serialId) ? dictUCarPrice[_serialId] : "";
            //二手车价格区间
            dictUCarPriceRange = serialBLL.GetUCarSerialPrice();
            if (dictUCarPriceRange.ContainsKey(_serialId))
            {
                uCarPriceRange = dictUCarPriceRange[_serialId];
                if (string.IsNullOrEmpty(uCarPriceRange))
                    uCarPriceRange = "暂无报价";
            }
            dicSerialToSerial = base.GetAllSerialToSerial();
            //dictSerailTuanUrl = serialBLL.GetSerialTuanUrl();
            InitSerialInfo();
            MakeSerialInfoHtml();
            GetCarDetailsViewZoneHtml();
            GetCarList(); //CarList();
            //SerialMustWatch();
            SerialNews();
            SerialFocusNews();
            MakeForumNewsHtml();
            MakeForumHotNewsHtml();
            MakeSerialToSerialHtml();
            SerialSeatNum();
            EditorComment();
            InitTitle();


            GetChanDiName();
            GetSerialColors();
            GetVideo();
            MakeKoubeiImpressionHtml();
            newsPingceHtml = GetCategoryNewsHtml(CarNewsType.pingce, ref pingceLiFlag);
            newsDaogouHtml = GetCategoryNewsHtml(CarNewsType.daogou, ref daogouLiFlag);
            if (string.IsNullOrEmpty(_serialNews) && string.IsNullOrEmpty(newsPingceHtml) && string.IsNullOrEmpty(newsDaogouHtml))
            {
                _wenZhangShowFlag = false;
            }
            //InitDaoGouData();
        }
        protected string newsPingceHtml = string.Empty;
        protected string newsDaogouHtml = string.Empty;
        private string GetCategoryNewsHtml(CarNewsType carNewsType, ref bool ligt3Flag)
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsPingce = _newsBLL.GetTopSerialNewsAllData(_serialId, carNewsType, 6);
            if (dsPingce != null && dsPingce.Tables.Count > 0 && dsPingce.Tables[0].Rows.Count > 0)
            {
                int index = 0;
                foreach (DataRow dr in dsPingce.Tables[0].Rows)
                {
                    int newsId = ConvertHelper.GetInteger(dr["CmsNewsId"]);
                    string url = ConvertHelper.GetString(dr["FilePath"]);

                    DateTime date = ConvertHelper.GetDateTime(dr["PublishTime"]);
                    string editorName = ConvertHelper.GetString(dr["EditorName"]);
                    string faceTitle = ConvertHelper.GetString(dr["FaceTitle"]);
                    //modified by sk 22个文字 2017-03-06
                    faceTitle = StringHelper.GetRealLength(faceTitle) > 44 ? StringHelper.SubString(faceTitle, 44, false) : faceTitle;
                    string picUrl = ConvertHelper.GetString(dr["Picture"]);
                    string imageUrl = (!string.IsNullOrEmpty(picUrl) && picUrl.IndexOf("/not") < 0) ? picUrl : ConvertHelper.GetString(dr["FirstPicUrl"]);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        imageUrl = imageUrl.Replace("/bitauto/", "/newsimg_180x120/bitauto/")
                            .Replace("/autoalbum/", "/newsimg_180x120/autoalbum/");
                    }
                    string newsUrl = Convert.ToString(dr["filepath"]).Replace("news.bitauto.com", "news.m.yiche.com");
                    if (index >= 3)
                    {
                        sb.AppendFormat("<li style=\"display:none;\" class=\"{0}\">", string.IsNullOrEmpty(imageUrl) ? "news-noimg" : "");
                    }
                    else
                    {
                        sb.AppendFormat("<li class=\"{0}\">", string.IsNullOrEmpty(imageUrl) ? "news-noimg" : "");
                    }
                    sb.AppendFormat("    <a href=\"{0}\">", newsUrl);
                    sb.Append(string.IsNullOrEmpty(imageUrl) ? "" : string.Format("<div class=\"img-box\"><span><img src=\"{0}\"></span></div>", imageUrl));
                    sb.AppendFormat("        <div class=\"con-box\"><h4>{0}</h4><em><span>{1}</span><span>{2}</span><i class=\"ico-comment huifu comment_0_{3}\"></i></em>", faceTitle, date.ToString("yyyy-MM-dd"), editorName, newsId);
                    sb.AppendFormat("        </div>");
                    sb.AppendFormat("    </a>");
                    sb.AppendFormat("</li>");
                    index++;
                }
                if (index > 3)
                    ligt3Flag = true;
            }
            return sb.ToString();
        }

        private void MakeSerialInfoHtml()
        {
            #region 焦点图

            var focusImgId = new Dictionary<int, string>();
            focusImg.Clear();
            List<SerialFocusImage> imgList = serialBLL.GetSerialFocusImageList(_serialEntity.Id);
            if (imgList != null && imgList.Count > 0)
            {
                //大图默认显示焦点图第一张，如果没有焦点图，显示封面图
                SerialFocusImage image = imgList[0];
                _serialImage =
                    string.Format(
                        "<a href=\"http://photo.m.yiche.com/picture/{0}/{1}/\" data-channelid=\"27.23.723\" class=\"left-area\"><img alt=\"{4}{5}\" src=\"{2}\">{3}</a>",
                        _serialId, image.ImageId, String.Format(image.ImageUrl, 4),
                        string.IsNullOrEmpty(image.GroupName) ? string.Empty : "<em>" + image.GroupName + "</em>",
                        _serialSeoName,
                        image.GroupName);
                if (!focusImgId.ContainsKey(image.ImageId))
                {
                    focusImgId.Add(image.ImageId, _serialImage);
                    focusImg.Add(_serialImage);
                }

                //第二张图取子品牌焦点图第3张（完整内饰）
                if (imgList.Count >= 3)
                {
                    SerialFocusImage csImg = imgList[2];
                    string smallImgUrl = csImg.ImageUrl;
                    if (csImg.ImageId > 0)
                    {
                        smallImgUrl = String.Format(smallImgUrl, 4);
                    }
                    string secondImg =
                        string.Format(
                            "<a href=\"http://photo.m.yiche.com/picture/{0}/{3}/\" data-channelid=\"27.23.724\" class=\"img-box\"><img alt=\"{4}{5}\" src=\"{1}\">{2}</a>",
                            _serialId, smallImgUrl,
                            string.IsNullOrEmpty(csImg.GroupName) ? string.Empty : "<em>" + csImg.GroupName + "</em>",
                            csImg.ImageId,
                            _serialSeoName,
                            csImg.GroupName);
                    if (!focusImgId.ContainsKey(csImg.ImageId))
                    {
                        focusImgId.Add(csImg.ImageId, secondImg);
                        focusImg.Add(secondImg);
                    }
                }
            }
            else
            {
                if (dicPicNoneWhite.ContainsKey(_serialId))
                {
                    _serialImage =
                        string.Format(
                            "<a href=\"http://photo.m.yiche.com/picture/{0}/{1}/\" data-channelid=\"27.23.723\" class=\"left-area\"><img alt=\"{3}外观\" src=\"{2}\"><em>外观</em></a>",
                            _serialId, dicPicNoneWhite[_serialId].FirstOrDefault().Key,
                            dicPicNoneWhite[_serialId].FirstOrDefault().Value,
                            _serialSeoName);
                    if (!focusImgId.ContainsKey(dicPicNoneWhite[_serialId].FirstOrDefault().Key))
                    {
                        focusImgId.Add(dicPicNoneWhite[_serialId].FirstOrDefault().Key, _serialImage);
                        focusImg.Add(_serialImage);
                    }
                }
            }

            //第三张图取空间分类第1张图
            if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("A"))
            {
                if (dsCsPic.Tables.Contains("C") && dsCsPic.Tables["C"].Rows.Count > 0)
                {
                    //取空间图片第一张图片,与前两张图片不重复
                    int speceImageCount = 0;
                    foreach (DataRow row in dsCsPic.Tables["C"].Select("P='8'")) //空间
                    {
                        int imgId = row["I"] == DBNull.Value ? 0 : Convert.ToInt32(row["I"]);
                        string imgUrl = row["U"] == DBNull.Value ? "" : Convert.ToString(row["U"]);
                        if (imgId == 0 || imgUrl.Length == 0 || focusImgId.ContainsKey(imgId))
                        {
                            continue;
                        }
                        speceImageCount++;
                        imgUrl = CommonFunction.GetPublishHashImgUrl(4, imgUrl, imgId);
                        string picName = Convert.ToString(row["D"]);
                        string picUlr = "http://photo.m.yiche.com/picture/" + _serialId + "/" + imgId + "/";
                        string thirdImg =
                            string.Format("<a href=\"{0}\" data-channelid=\"27.23.725\" class=\"img-box\"><img alt=\"{2}空间\" src=\"{1}\"><em>空间</em></a>", picUlr,
                                imgUrl,
                                _serialSeoName);
                        if (!focusImgId.ContainsKey(imgId))
                        {
                            focusImgId.Add(imgId, thirdImg);
                            focusImg.Add(thirdImg);
                        }
                        break;
                    }
                }
            }

            if (focusImg.Count < 3)
            {
                //如果不够3张,则显示焦点图第2张
                if (imgList.Count >= 2)
                {
                    SerialFocusImage csImg = imgList[1];
                    string smallImgUrl = csImg.ImageUrl;
                    if (csImg.ImageId > 0)
                    {
                        smallImgUrl = String.Format(smallImgUrl, 4);
                    }
                    string secondImg =
                        string.Format(
                            "<a href=\"http://photo.m.yiche.com/picture/{0}/{3}/\" data-channelid=\"27.23.724\" class=\"img-box\"><img alt=\"{4}{5}\" src=\"{1}\">{2}</a>",
                            _serialId, smallImgUrl,
                            string.IsNullOrEmpty(csImg.GroupName) ? string.Empty : "<em>" + csImg.GroupName + "</em>",
                            csImg.ImageId,
                            _serialSeoName,
                            csImg.GroupName);
                    if (!focusImgId.ContainsKey(csImg.ImageId))
                    {
                        focusImgId.Add(csImg.ImageId, secondImg);
                        focusImg.Add(secondImg);
                    }
                }
                else
                {
                    //如果没有焦点图第2张,取图解第一张
                    XmlNode firstTujieNode = GetFirstTujieImage(dsCsPic);
                    if (firstTujieNode != null)
                    {
                        string groupName = firstTujieNode.Attributes["GroupName"].Value;
                        string backupImg = string.Empty;
                        if (focusImg.Count > 1)
                        {
                            backupImg = string.Format("<a href=\"{0}\" data-channelid=\"27.23.724\" class=\"img-box\"><img alt=\"{3}{4}\" src=\"{1}\">{2}</a>",
                                firstTujieNode.Attributes["Link"].Value,
                                firstTujieNode.Attributes["ImageUrl"].Value.Replace("_4.", "_4."),
                                string.IsNullOrEmpty(groupName) ? string.Empty : "<em>" + groupName + "</em>",
                                _serialSeoName,
                                groupName);
                        }
                        else
                        {
                            backupImg = string.Format("<a href=\"{0}\" class=\"left-area\"><img alt=\"{3}{4}\" src=\"{1}\">{2}</a>",
                                firstTujieNode.Attributes["Link"].Value,
                                firstTujieNode.Attributes["ImageUrl"].Value.Replace("_4.", "_4."),
                                string.IsNullOrEmpty(groupName) ? string.Empty : "<em>" + groupName + "</em>",
                                _serialSeoName,
                                groupName);
                        }
                        if (!focusImgId.ContainsKey(Convert.ToInt32(firstTujieNode.Attributes["ImageId"].Value)))
                        {
                            focusImgId.Add(Convert.ToInt32(firstTujieNode.Attributes["ImageId"].Value), backupImg);
                            focusImg.Add(backupImg);
                        }
                    }
                }
            }

            #endregion
        }

        /// <summary>
        ///     论坛话题新闻
        /// </summary>
        private void MakeForumNewsHtml()
        {
            var sb = new StringBuilder();
            //string baaUrl = serialBLL.GetForumUrlBySerialId(_serialEntity.Id);
            // XmlDocument xmlDoc = serialBLL.GetSerialFocusNews(_serialEntity.Id);
            XmlDocument xmlDoc = serialBLL.GetSerialForumNews(_serialEntity.Id);
            if (xmlDoc == null) return;
            XmlNodeList newsList = xmlDoc.SelectNodes("/root/Forum/ForumSubject");
            if (newsList.Count <= 1) return;

            sb.Append("<div class='tt-first' data-channelid=\"27.23.736\">");
            sb.Append("<h3>论坛</h3>");
            sb.AppendFormat("<div class='opt-more'><a href='{0}'>更多</a></div>", _baaUrl);
            sb.Append("</div>");
            sb.Append("<div class='card-news card-news-bbs b-shadow' id='m_hotforum' data-channelid=\"27.23.737\">");
            sb.Append("<ul>");
            int i = 0;
            foreach (XmlNode node in newsList)
            {
                i++;
                if (i > 3) break;
                string newsTitle = node.SelectSingleNode("title").InnerText.Trim();
                //过滤Html标签
                newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
                newsTitle = StringHelper.SubString(newsTitle, 40, true);
                string tid = node.SelectSingleNode("tid").InnerText;
                string filePath = node.SelectSingleNode("url").InnerText;
                string replies = node.SelectSingleNode("replies").InnerText;
                string poster = node.SelectSingleNode("poster").InnerText;
                string pubTime = "";
                // modified by chengl Jun.15.2012
                if (node.SelectSingleNode("postdatetime") != null)
                {
                    pubTime = node.SelectSingleNode("postdatetime").InnerText;
                    pubTime = Convert.ToDateTime(pubTime).ToString("yyyy-MM-dd");
                }
                sb.AppendFormat("<li class='news-img-list'>");
                sb.AppendFormat("<a href='{0}'>", filePath.Replace("baa.bitauto.com", "baa.m.yiche.com"));
                sb.AppendFormat("<h4 class=\"h25\">{0}</h4>", newsTitle);
                XmlNode imglistNode = node.SelectSingleNode("imgList");
                if (imglistNode != null)
                {
                    XmlNodeList xmlNodeList = imglistNode.SelectNodes("img");
                    if (xmlNodeList != null && xmlNodeList.Count > 0)
                    {
                        sb.AppendFormat("<ul>");
                        int j = 0;
                        foreach (XmlNode item in xmlNodeList)
                        {
                            if (j >= 3)
                            {
                                break;
                            }
                            sb.AppendFormat("<li><span><img src='{0}' alt=\"{1}\"/></span></li>",
                                item.InnerText.Replace("_120_80_", "_216_144_"), newsTitle);
                            j++;
                        }
                        sb.AppendFormat("</ul>");
                    }
                }

                sb.AppendFormat("<em><span>{0}</span><span>{1}</span><i class='ico-comment'>{2}</i></em>", poster,
                    pubTime, replies);
                sb.AppendFormat("</a>");
                sb.AppendFormat("</li>");
            }
            sb.Append("<script type=\"text/javascript\">");
            sb.Append("showNewsInsCode('c32bb18e-3133-42cc-b65c-477543dd487a', '130ba246-33ce-4962-be9f-cfb4eee3fad4', '33461266-42fb-41c3-b603-ce69acee87b4', '21bdf009-15e2-4dfb-bea1-4508e6b04755');");
            sb.Append("</script>");
            sb.Append("</ul>");
            sb.Append("</div>");
            _forumNewsHtml = sb.ToString();
        }

        /// <summary>
        ///     论坛热帖
        /// </summary>
        private void MakeForumHotNewsHtml()
        {
            var sb = new StringBuilder();
            string baaUrl = serialBLL.GetForumUrlBySerialId(_serialEntity.Id);
            XmlDocument xmlDoc = serialBLL.GetSerialForumNews(_serialEntity.Id);
            if (xmlDoc == null) return;
            XmlNode node = xmlDoc.SelectSingleNode("/root/Forum/ForumSubject");
            if (node == null) return;

            string newsTitle = node.SelectSingleNode("title").InnerText.Trim();
            //过滤Html标签
            newsTitle = StringHelper.RemoveHtmlTag(newsTitle);

            //modified by zhangll Jan.07.2015 去掉热帖标题字数限制
            //newsTitle = StringHelper.SubString(newsTitle, 40, true);
            string tid = node.SelectSingleNode("tid").InnerText;
            string replies = node.SelectSingleNode("replies").InnerText;
            string poster = node.SelectSingleNode("poster").InnerText;
            string filePath = node.SelectSingleNode("url").InnerText;
            string pubTime = "";
            // modified by chengl Jun.15.2012
            if (node.SelectSingleNode("postdatetime") != null)
            {
                pubTime = node.SelectSingleNode("postdatetime").InnerText;
                pubTime = Convert.ToDateTime(pubTime).ToString("yyyy-MM-dd");
            }
            sb.Append("<li>");
            sb.AppendFormat(
                "<a href=\"{0}?ref=cxyw\" onclick=\"javascript:dcsMultiTrack('DCS.dcsuri', '/car/onclick/retie.onclick','WT.ti', '热帖')\">",
                filePath.Replace("baa.bitauto.com", "baa.m.yiche.com"));
            sb.AppendFormat("<span>{0}</span>", newsTitle);
            sb.Append("<em class=\"blue\">热帖</em>");
            sb.Append("<div class=\"line\"></div>");
            // modified by zhangll Sep.05.2014 去掉热帖评论数等
            //sb.AppendFormat("<span><em>{1}</em><em class=\"ico-comment\">{2}</em><em>{0}</em></span>", poster, pubTime, replies);
            sb.Append("</a>");
            sb.Append("</li>");
            //sb.AppendFormat("<a class=\"more-news\" href=\"{0}\">更多</a>", baaUrl.Replace("baa.bitauto.com", "baa.m.yiche.com"));

            _forumHotNewsHtml = sb.ToString();
            //去掉科鲁兹的焦点区要闻和热帖内容
            if (_serialId == 2608)
            {
                _forumHotNewsHtml = string.Empty;
            }
        }

        /// <summary>
        ///     试驾评测块
        /// </summary>
        private void EditorComment()
        {
            //int firstCarId = new CarNewsBll().GetEditorCommentCarId(_serialId);
            //if (firstCarId > 0)
            //{
            //    string htmlFile = Path.Combine(WebConfig.DataBlockPath,
            //        string.Format("Data\\SerialSet\\WirelessEditorCommentHtml\\Serial_EditorComment_{0}.html", firstCarId));
            //    if (File.Exists(htmlFile))
            //    {
            //        _editorCommentHtml = File.ReadAllText(htmlFile);
            //    }
            //}
            string htmlFile = Path.Combine(WebConfig.DataBlockPath,
                string.Format("Data\\SerialSet\\WirelessEditorCommentHtml\\Serial_EditorComment_cs_{0}.html", _serialId));
            if (File.Exists(htmlFile))
            {
                _editorCommentHtml = File.ReadAllText(htmlFile);
            }
        }

        /// <summary>
        ///     座位数
        /// </summary>
        private void SerialSeatNum()
        {
            int seatNum = 0;
            if (_serialCarList != null && _serialCarList.Count > 0)
            {
                Dictionary<int, string> carParamValues = new Car_BasicBll().GetCarParamExDic(665);
                if (carParamValues != null && carParamValues.Count > 0)
                {
                    foreach (CarInfoForSerialSummaryEntity carInfo in _serialCarList)
                    {
                        if (carParamValues.ContainsKey(carInfo.CarID))
                        {
                            int tempNum = ConvertHelper.GetInteger(carParamValues[carInfo.CarID]);
                            if (tempNum > seatNum)
                                seatNum = tempNum;
                        }
                    }
                }
            }
            if (seatNum > 0)
                _serialSeatNum = seatNum + "座";
        }

        private void InitTitle()
        {
            if (_isYearEnabled)
            {
                string yearStr = _serialYear.ToString();
                _title = string.Format("【{0}{3}款】{0}{3}款最新报价_{0}{3}款图片-手机易车网"
                    , _serialSeoName, _serialBrandName, _serialName, yearStr);
                _keyWords =
                    string.Format(
                        "{0}{3}款,{0}{3}款报价,{0}{3}款价格,{0}{3}款油耗,{0}{3}款图片,手机易车网,car.m.yiche.com"
                        , _serialSeoName, _serialBrandName, _serialName, yearStr);
                _Description =
                    string.Format(
                        "易车提供{0}{3}款最新报价,{0}{3}款图片,{0}{3}款油耗,查{0}{3}最新价格,就上手机易车网"
                        , _serialSeoName, _serialBrandName, _serialName, yearStr);
            }
            else
            {
                _title = string.Format("【{0}】最新{0}报价_参数_图片_{1}{2}论坛-手机易车网"
                    , _serialSeoName, _serialBrandName, _serialName);
                _keyWords = string.Format("{0},{0}报价,{0}价格,{0}参数,{0}论坛,手机易车网,car.m.yiche.com"
                    , _serialSeoName);
                _Description =
                    string.Format(
                        "{1}{2},易车提供全国官方4S店{0}报价,最新{1}{2}降价优惠信息。以及{0}报价,{0}图片,{0}在线询价服务,低价买车尽在手机易车网。"
                        , _serialSeoName, _serialBrandName, _serialName);
            }
        }

        /// <summary>
        ///     获取参数
        /// </summary>
        private void GetParamter()
        {
            _serialId = ConvertHelper.GetInteger(Request.QueryString["ID"]);
            _serialYear = ConvertHelper.GetInteger(Request.QueryString["year"]);

            if (_serialId <= 0)
                return;
            _serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, _serialId);
            if (_serialEntity == null || _serialEntity.Id <= 0)
                return;

            #region 该子品牌没有指定的年款，则跳到404页

            if (_serialYear > 0)
            {
                if (_serialEntity.CarList != null && _serialEntity.CarList.Length > 0)
                {
                    bool iserror = true;
                    foreach (CarEntity car in _serialEntity.CarList)
                    {
                        if (car.CarYear == _serialYear)
                        {
                            iserror = false;
                            break;
                        }
                    }
                    if (iserror)
                    {
                        _serialEntity = null;
                        return;
                    }
                }
            }

            #endregion

            _serialInfoCard = new Car_SerialBll().GetSerialInfoCard(_serialId);
        }

        /// <summary>
        ///     生成子品牌概况Html
        /// </summary>
        private void InitSerialInfo()
        {
            _serialName = _serialEntity.Name;
            _serialAllSpell = _serialEntity.AllSpell;
            _serialShowName = _serialEntity.ShowName;
            _serialSeoName = _serialEntity.SeoName;
            _serialNavName = _serialEntity.Name;

            if (_serialInfoCard.CsSaleState == "停销")
            {
                _serialPrice = "停售";
            }
            else if (_serialInfoCard.CsPriceRange == null || _serialInfoCard.CsPriceRange.Length == 0 || _serialInfoCard.CsPriceRange == "未上市")
                _serialPrice = "暂无";
            else
                _serialPrice = _serialInfoCard.CsPriceRange.Replace("万-", "-");

            _serialRefPrice = _serialEntity.ReferPrice.Replace("万-", "-");
            _serialTransmission = _serialInfoCard.CsTransmissionType;
            _serialBody = _serialInfoCard.CsBodyForm;
            _serialMaintenance = _serialInfoCard.SerialRepairPolicy;

            _serialBrandName = _serialEntity.Brand.MasterBrand.Name; //_serialEntity.Brand.SeoName;
            _serialProducer = _serialEntity.Brand.Producer.ShortName;
            _serialLevel = _serialInfoCard.CsLevel;
            //静态块内容
            //dictSerialBlockHtml = _commonhtmlBLL.GetCommonHtml(_serialId, CommonHtmlEnum.TypeEnum.Serial,
            //    CommonHtmlEnum.TagIdEnum.WirelessSerialSummary);
            dictSerialBlockHtmlV2 = _commonhtmlBLL.GetCommonHtml(_serialId, CommonHtmlEnum.TypeEnum.Serial,
               CommonHtmlEnum.TagIdEnum.WirelessSerialSummaryV2);
            //add by zhangll Aug .18.2014 焦点区要闻
            dictSerialFocusSummaryBlockHtml = _commonhtmlBLL.GetCommonHtml(_serialId, CommonHtmlEnum.TypeEnum.Serial,
                CommonHtmlEnum.TagIdEnum.WirelessSerialFocusSummary);
            //Car_SerialBll serialBll = new Car_SerialBll();

            #region 排量

            List<string> exhaustList = serialBLL.GetSaleCarExhaustBySerialId(_serialId);
            if (exhaustList.Count > 3)
                _serialDisplacement = string.Join(" ", exhaustList.Take(2).ToArray()) + "..." +
                                      exhaustList[exhaustList.Count - 1];
            else
                _serialDisplacement = string.Join(" ", exhaustList.ToArray());
            //if (_serialEntity.ExhaustList != null && _serialEntity.ExhaustList.Length > 0)
            //{
            //    if (_serialEntity.ExhaustList.Length > 3)
            //    {
            //        _serialDisplacement = string.Concat(_serialEntity.ExhaustList[0]
            //            , "..."
            //            , _serialEntity.ExhaustList[_serialEntity.ExhaustList.Length - 1]);
            //    }
            //    else
            //    {
            //        _serialDisplacement = _serialEntity.Exhaust;
            //    }
            //}

            #endregion

            #region 油耗

            string serialSummaryFuelCost = _serialInfoCard.CsSummaryFuelCost;
            string serialGuestFuelCost = _serialInfoCard.CsGuestFuelCost;
            if (!string.IsNullOrEmpty(serialSummaryFuelCost) && serialSummaryFuelCost != "0-0L")
            {
                _serialFuelCost = serialSummaryFuelCost; //+ "(综合)";
            }
            //if (!string.IsNullOrEmpty(serialGuestFuelCost) && serialGuestFuelCost != "0-0L")
            //{
            //    if (string.IsNullOrEmpty(_serialFuelCost))
            //        _serialFuelCost = serialGuestFuelCost + "(网友)";
            //    else
            //        _serialFuelCost = string.Format("{0}&nbsp;{1}(网友)", _serialFuelCost, serialGuestFuelCost);
            //}

            #endregion

            #region 车型详解、全系导购 modified by sk 2013.10.25 注释

            /*
 			int newsId = 0;
			Dictionary<int, EnumCollection.PingCeTag> dictPingceNews = base.GetPingceTagsByCsId(_serialId);
			if (dictPingceNews.ContainsKey(1))//查找“导语”新闻
			{
				string url = dictPingceNews[1].url;
				string[] arrTempURL = url.Split('/');
				string pageName = arrTempURL[arrTempURL.Length - 1];
				if (pageName.Length >= 10)
				{
					if (int.TryParse(pageName.Substring(3, 7), out newsId))
					{ }
				}
			}
			if (newsId > 0)
			{
				DataSet ds = base.GetPingCeNewByNewID(newsId);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("content"))
				{
					string PingCeTitle = string.Empty;
					string PingCeFilePath = string.Empty;
					DataRow row = ds.Tables[0].Rows[0];
					if (ds.Tables[0].Columns.Contains("title"))
						PingCeTitle = row["title"].ToString();
					else
						PingCeTitle = row["facetitle"].ToString();
					PingCeFilePath = row["filepath"].ToString();
					_serialPingce = string.Format("<li class=\"oneline\"><span>车型详解 | </span><a href=\"http://news.m.yiche.com{0}\">{1}</a></li>",
						PingCeFilePath,
						PingCeTitle);
				}
			}
			string daogouUrl = base.GetCsRainbowAndURLInfo(_serialId, 57);
			newsId = 0;
			if (!string.IsNullOrEmpty(daogouUrl))
			{
				string[] arrTempURL = daogouUrl.Split('/');
				string pageName = arrTempURL[arrTempURL.Length - 1];
				if (pageName.Length >= 10)
				{
					if (int.TryParse(pageName.Substring(3, 7), out newsId))
					{ }
				}
			}
			if (newsId > 0)
			{
				DataSet ds = base.GetPingCeNewByNewID(newsId);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("content"))
				{
					string PingCeTitle = string.Empty;
					string PingCeFilePath = string.Empty;
					DataRow row = ds.Tables[0].Rows[0];
					if (ds.Tables[0].Columns.Contains("title"))
						PingCeTitle = row["title"].ToString();
					else
						PingCeTitle = row["facetitle"].ToString();
					PingCeFilePath = row["filepath"].ToString();
					_serialDaogou = string.Format("<li class=\"oneline\"><span>全系导购 | </span><a href=\"http://news.m.yiche.com{0}\">{1}</a></li>",
						PingCeFilePath,
						PingCeTitle);
				}
			}
 			 */

            #endregion

            _serialTotalPV = serialBLL.GetSerialTotalPV(_serialId);

            _serialCarList = _carBLL.GetCarInfoForSerialSummaryBySerialId(_serialId);

            #region add by songcl 2015-07-10 在销车系全系为电动车

            var fuelTypeList = _serialCarList.FindAll(p => p.SaleState == "在销").Where(p => p.Oil_FuelType != "")
                .GroupBy(p => p.Oil_FuelType)
                .Select(g => g.Key).ToList();
            isElectrombile = fuelTypeList.Count == 1 && fuelTypeList[0] == "电力";

            #endregion

            _baaUrl = serialBLL.GetForumUrlBySerialId(_serialEntity.Id).Replace("baa.bitauto.com", "baa.m.yiche.com");
            CsHeadHTML = base.GetCommonNavigation("MCsSummary", _serialId);
        }

        /// <summary>
        ///     最新文章
        /// </summary>
        private void SerialNews()
        {
            const int focusNewsForWireless = (int)CommonHtmlEnum.BlockIdEnum.FocusNewsForWireless;
            if (dictSerialBlockHtmlV2.ContainsKey(focusNewsForWireless))
                _serialNews = dictSerialBlockHtmlV2[focusNewsForWireless];
        }

        /// <summary>
        ///     焦点区文章（要闻）
        /// </summary>
        private void SerialFocusNews()
        {
            var news = (int)CommonHtmlEnum.BlockIdEnum.FocusNews;
            if (dictSerialFocusSummaryBlockHtml.ContainsKey(news))
            {
                _serialFocusNews = dictSerialFocusSummaryBlockHtml[news];
            }
            //去掉科鲁兹的焦点区要闻和热帖内容
            if (_serialId == 2608)
            {
                _serialFocusNews = string.Empty;
            }
        }

        /// <summary>
        ///     子品牌还关注
        /// </summary>
        /// <returns></returns>
        private void MakeSerialToSerialHtml()
        {
            var htmlCode = new StringBuilder();
            var htmlTagCode = new StringBuilder();

            htmlTagCode.Append("<div class='tt-first' id='m-tabs-kankan'>");
            htmlTagCode.Append("<h3>看了还看</h3>");
            htmlTagCode.Append("</div>");
            htmlCode.Append("<div class='swiper-container-kankan swiper-container'><div class='swiper-wrapper'>");
            List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(_serialId, 6);
            bool firstTag = false;
            if (lsts.Count > 0)
            {
                firstTag = true;
                htmlCode.Append(" <div class='swiper-slide'>");
                htmlCode.Append("<div class='car-list3' data-channelid=\"27.23.743\">");
                htmlCode.Append("<ul>");
                for (int i = 0; i < lsts.Count && i < 6; i++)
                {
                    EnumCollection.SerialToSerial sts = lsts[i];
                    string csName = sts.ToCsShowName;
                    htmlCode.Append("<li>");
                    htmlCode.AppendFormat("<a href='/{0}/'>", sts.ToCsAllSpell.ToLower());
                    htmlCode.AppendFormat("<img src='{0}' alt=\"{1}\" />", sts.ToCsPic.ToString(CultureInfo.InvariantCulture).Replace("_5.", "_3."), csName);
                    htmlCode.AppendFormat("<strong>{0}</strong>", csName);
                    htmlCode.AppendFormat("<p>{0}</p>",
                        string.IsNullOrEmpty(sts.ToCsPriceRange) ? "暂无报价" : sts.ToCsPriceRange);
                    htmlCode.Append("</a>");
                    htmlCode.Append("</li>");
                }
                htmlCode.Append("</ul>");
                htmlCode.Append("</div></div>");
            }
            //if (dicSerialToSerial.ContainsKey(_serialId))
            //{
            //    var serialBigList = dicSerialToSerial[_serialId].SelectNodes("Item[@Type=0]");
            //    var serialSaveFuelList = dicSerialToSerial[_serialId].SelectNodes("Item[@Type=1]");
            //    if (serialBigList != null && serialBigList.Count > 0)
            //    {
            //        firstTag = true;
            //        htmlTagCode.Append(string.Format(" <li {0}><a href='#'>比它大</a></li>", firstTag == true ? "" : "class='current'"));
            //        htmlCode.Append("<div class='swiper-slide'>");
            //        htmlCode.Append("<div class='car-list3' data-channelid=\"27.23.954\">");
            //        if (serialBigList.Count == 1 && serialBigList[0].Attributes["Name"].Value == "最大啦")
            //        {
            //            htmlCode.Append("<div class='txt-prompt'> <p>您看的车已经最大啦！</p></div>");
            //        }
            //        else
            //        {
            //            htmlCode.Append("<ul>");
            //            int count = 0;
            //            foreach (XmlNode item in serialBigList)
            //            {
            //                if (count > 5)
            //                {
            //                    break;
            //                }
            //                int csId = ConvertHelper.GetInteger(item.Attributes["SerialId"].Value);
            //                var ser = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csId);
            //                if (ser == null)
            //                {
            //                    continue;
            //                }
            //                string name = ser.ShowName.ToString();
            //                var priceRange = this.GetSerialPriceRangeByID(csId);
            //                string defaultPic = "";
            //                int picCount = 0;
            //                this.GetSerialPicAndCountByCsID(csId, out defaultPic, out picCount, true);
            //                if (defaultPic.Trim().Length == 0)
            //                    defaultPic = WebConfig.DefaultCarPic;

            //                htmlCode.Append("<li>");
            //                htmlCode.AppendFormat("<a href='/{0}/'>", ser.AllSpell.ToLower());
            //                htmlCode.AppendFormat("<img src='{0}' alt=\"{1}\" />", defaultPic.Replace("_2.", "_3."), name);
            //                htmlCode.AppendFormat("<strong>{0}</strong>", name);
            //                htmlCode.AppendFormat("<p>{0}</p>",
            //                    string.IsNullOrEmpty(priceRange) ? "暂无报价" : priceRange);
            //                htmlCode.Append("</a>");
            //                htmlCode.Append("</li>");
            //                count++;

            //            }
            //            htmlCode.Append("</ul>");
            //        }
            //        htmlCode.Append("</div></div>");
            //    }
            //    if (serialSaveFuelList != null && serialSaveFuelList.Count > 0)
            //    {
            //        firstTag = true;
            //        htmlTagCode.Append(string.Format(" <li {0}><a href='#'>比它省油</a></li>", firstTag == true ? "" : "class='current'"));
            //        htmlCode.Append("<div class='swiper-slide'>");
            //        htmlCode.Append("<div class='car-list3' data-channelid=\"27.23.955\">");
            //        if (serialSaveFuelList.Count == 1 && serialSaveFuelList[0].Attributes["Name"].Value == "最低啦")
            //        {
            //            htmlCode.Append("<div class='txt-prompt'> <p>您看的车已经最省油了！</p></div>");
            //        }
            //        else
            //        {
            //            htmlCode.Append("<ul>");
            //            int count = 0;
            //            foreach (XmlNode item in serialSaveFuelList)
            //            {
            //                if (count > 5)
            //                {
            //                    break;
            //                }
            //                int csId = ConvertHelper.GetInteger(item.Attributes["SerialId"].Value);
            //                var ser = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csId);
            //                if (ser == null)
            //                {
            //                    continue;
            //                }
            //                string name = ser.ShowName.ToString();
            //                var priceRange = this.GetSerialPriceRangeByID(csId);
            //                string defaultPic = "";
            //                int picCount = 0;
            //                this.GetSerialPicAndCountByCsID(csId, out defaultPic, out picCount, true);
            //                if (defaultPic.Trim().Length == 0)
            //                    defaultPic = WebConfig.DefaultCarPic;

            //                htmlCode.Append("<li>");
            //                htmlCode.AppendFormat("<a href='/{0}/'>", ser.AllSpell.ToLower());
            //                htmlCode.AppendFormat("<img src='{0}' alt=\"{1}\" />", defaultPic.Replace("_2.", "_3."), name);
            //                htmlCode.AppendFormat("<strong>{0}</strong>", name);
            //                htmlCode.AppendFormat("<p>{0}</p>",
            //                    string.IsNullOrEmpty(priceRange) ? "暂无报价" : priceRange);
            //                htmlCode.Append("</a>");
            //                htmlCode.Append("</li>");
            //                count++;
            //            }
            //            htmlCode.Append("</ul>");
            //        }
            //        htmlCode.Append("</div></div>");
            //    }
            //}
            htmlCode.Append("</div></div>");
            _serialToSee = htmlCode.Insert(0, htmlTagCode).ToString();
        }


        /// <summary>
        ///     取第一张图解
        /// </summary>
        /// <param name="dsCsPic"></param>
        private XmlNode GetFirstTujieImage(DataSet dsCsPic)
        {
            XmlElement element = null;
            var xmlDoc = new XmlDocument();
            //取图解第一张
            if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("C") &&
                dsCsPic.Tables["C"].Rows.Count > 0)
            {
                IEnumerable<DataRow> rows = dsCsPic.Tables["C"].Rows.Cast<DataRow>();
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
                    string picUrl = "http://photo.m.yiche.com/picture/" + _serialId + "/" + imgId + "/";
                    element = xmlDoc.CreateElement("CarImage");
                    element.SetAttribute("ImageId", imgId.ToString());
                    element.SetAttribute("ImageUrl", imgUrl);
                    element.SetAttribute("GroupName", "图解");
                    element.SetAttribute("ImageName", "图解");
                    element.SetAttribute("Link", picUrl);
                }
            }
            return element;
        }

        private void GetChanDiName()
        {
            string masterCountry = _serialEntity.Brand.MasterBrand.Country; //主品牌国别
            string producerCountry = _serialEntity.Brand.Producer.Country; //厂商国别
            if (masterCountry == producerCountry)
            {
                _chanDi = producerCountry;

                if (masterCountry.Contains("中国"))
                {
                    _chanDi = masterCountry + "自主";
                }
                else
                {
                    _chanDi = masterCountry + "进口";
                }
            }
            else
            {
                _chanDi = producerCountry + masterCountry + "合资";
            }
        }

        public void GetSerialColors()
        {
            SerialColorList = serialBLL.GetProduceSerialColors(_serialId);
        }

        public void GetVideo()
        {
            VideoList = VideoBll.GetNewAndHotVideoBySerialIdForWireless(_serialId);
        }

        private void MakeKoubeiImpressionHtml()
        {
            const int koubei = (int)CommonHtmlEnum.BlockIdEnum.KoubeiReportNew;
            //if (dictSerialBlockHtml.ContainsKey(koubei))
            //{
            //    koubeiImpressionHtml = dictSerialBlockHtml[koubei];
            //}
            if (dictSerialBlockHtmlV2.ContainsKey(koubei))
            {
                koubeiImpressionHtml += dictSerialBlockHtmlV2[koubei];
            }
        }

        #region modified by songcl 2015-07-07

        /// <summary>
        ///     获取车型详解区域视图列表html
        ///     zf 2016-06-02
        /// </summary>
        private void GetCarDetailsViewZoneHtml()
        {
            const int hexinReport = (int)CommonHtmlEnum.BlockIdEnum.HexinReport;
            if (dictSerialBlockHtmlV2.ContainsKey(hexinReport))
            {
                StringBuilder sbHtml = new StringBuilder();
                if (dictSerialBlockHtmlV2.ContainsKey(hexinReport))
                {
                    sbHtml.Append(dictSerialBlockHtmlV2[hexinReport]);
                }
                _carDetailsViewZoneHtml = sbHtml.ToString();
            }
        }

        /// <summary>
        ///     获取车款列表
        ///     songcl 2015-07-02
        /// </summary>
        private void GetCarList()
        {
            _serialCarList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
            var yearList = new List<string>();//年款
            if (_serialEntity.SaleState == "待销")
            {
                //待销不列年款
            }
            else if (_serialEntity.SaleState == "停销")
            {
                //取车系为停销状态的停销年款
                yearList =
                    _serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "停销")
                        .Select(p => p.CarYear)
                        .Distinct()
                        .ToList();
            }
            else
            {
                //取车系为在销状态的在销年款
                yearList =
                    _serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "在销")
                        .Select(p => p.CarYear)
                        .Distinct()
                        .ToList();
            }

            yearList.Sort(NodeCompare.CompareStringDesc);
            _yearCount = yearList.Count;
            //在售和停售数据集合
            //List<CarInfoForSerialSummaryEntity> salingAndNoSaleList = _serialCarList.FindAll(p => p.SaleState != "待销");

            //停售数据集合
            List<CarInfoForSerialSummaryEntity> carinfoNoSaleList = _serialCarList.FindAll(p => p.SaleState == "停销");

            //在售数据集合
            List<CarInfoForSerialSummaryEntity> carinfoSaleList = _serialCarList.FindAll(p => p.SaleState == "在销");

            //待售（未上市）数据集合
            List<CarInfoForSerialSummaryEntity> carinfoWaitSaleList = _serialCarList.FindAll(p => p.SaleState == "待销");

            var htmlCode = new StringBuilder();
            htmlCode.Append("<div class=\"second-tags-scroll-box mgt10\">");
            htmlCode.Append("<div class=\"pd15\">");
            htmlCode.Append("<div class='second-tags-scroll mgt12'data-channelid=\"27.23.727\">");
            htmlCode.Append("<ul id='yeartag'>");

            var targetList = new List<string>();//当前车系状态下的所有tab标签

            var htmlCondition = new StringBuilder();
            if (_serialEntity.SaleState == "在销")
            {
                htmlCondition.Append("<div class=\"sort sort3 sort-bg-white sort-pop tags-list\"><ul>");
                if (carinfoSaleList.Count > 0)
                {
                    targetList.Add("全部在售");
                }
                targetList.AddRange(yearList);
                if (carinfoWaitSaleList.Count > 0)
                {
                    targetList.Add("未上市");
                }
                if (carinfoNoSaleList.Count > 0)
                {
                    targetList.Add("停售");
                    if (yearList.Count > 0)
                    {
                        var stopCarYears = new List<string>();
                        stopCarYears = _serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "停销")
                                .Select(p => p.CarYear)
                                .Distinct()
                                .ToList();

                        if (stopCarYears.Count > 0)
                        {
                            stopCarYears.Sort(NodeCompare.CompareStringDesc);
                            nearestYear = stopCarYears[0];
                        }
                        htmlCondition.Append("<li data-action=\"stopyear\" class=\"m-btn current\" style=\"display:none\" data-channelid=\"27.23.510\"><a><span>" + nearestYear + "款</span><i></i></a></li>");
                    }
                }
                htmlCondition.Append("<li data-action=\"level\" class=\"m-btn\" data-channelid=\"27.23.511\"><a><span>排量</span><i></i></a></li>");
                htmlCondition.Append("<li data-action=\"bodyform\" class=\"m-btn\" data-channelid=\"27.23.512\"><a><span>变速箱</span><i></i></a></li>");
                htmlCondition.Append("</ul><div class=\"clear\"></div></div>");
            }
            else if (_serialEntity.SaleState == "待销")
            {
                targetList.Add("未上市");
            }
            else
            {
                targetList.AddRange(yearList);
                htmlCondition.Append("<div class=\"sort sort3 sort-bg-white sort-pop tags-list\"><ul>");
                htmlCondition.Append("<li data-action=\"level\" class=\"m-btn\" data-channelid=\"27.23.511\"><a><span>排量</span><i></i></a></li>");
                htmlCondition.Append("<li data-action=\"bodyform\" class=\"m-btn\" data-channelid=\"27.23.512\"><a><span>变速箱</span><i></i></a></li>");
                htmlCondition.Append("</ul><div class=\"clear\"></div></div>");
            }
            int flag = 0;
            foreach (string curtab in targetList)
            {
                switch (curtab)
                {
                    case "全部在售":
                        htmlCode.Append("<li id='all' class='current'><a><span>" + curtab + "</span></a></li>");
                        break;
                    case "未上市":
                        htmlCode.AppendFormat("<li id='unlisted' {0}><a><span>" + curtab + "</span></a></li>",
                            targetList.Contains("全部在售") || targetList.Count > 2 ? "" : "class='current'");
                        break;
                    case "停售":
                        htmlCode.AppendFormat("<li id='nosalelist'><a><span>" + curtab + "</span></a></li>",
                            targetList.Contains("全部在售") || targetList.Count > 2 ? "" : "class='current'");
                        break;
                    default:
                        htmlCode.AppendFormat("<li id='{0}' {1}><a><span>{0}款</span></a></li>", curtab,
                            targetList.Contains("全部在售") || flag > 0 ? "" : "class='current'");
                        flag++;
                        break;
                }
            }
            htmlCode.Append("</ul>");
            htmlCode.Append("</div>");
            htmlCode.Append("</div>");
            htmlCode.Append("<div class=\"right-mask\"></div>");
            htmlCode.Append("</div>");
            htmlCode.Append(htmlCondition.ToString());

            GetCarHtml(targetList, htmlCode);

            _carList = htmlCode.ToString();
        }
        /* 废除无用方法 modified by sk 2016.01.20 
        /// <summary>
        ///     songcl 2015-07-02
        /// </summary>
        /// <param name="yearList"></param>
        /// <param name="stringBuilder"></param>
        /// <returns></returns>
        private void GetCarHtmlOriginal(IEnumerable<string> yearList, StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat("<div id='carList{0}'>", _serialId);
            int counter = 0;
            int minMileage = 0;
            int maxMileage = 0;
            foreach (string item in yearList)
            {
                #region 数据筛选

                string year = item;
                List<CarInfoForSerialSummaryEntity> currentYearCarList;
                IEnumerable<IGrouping<object, CarInfoForSerialSummaryEntity>> querySale;
                switch (year)
                {
                    case "全部在售":
                        currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "在销");

                        querySale = currentYearCarList.GroupBy(
                            p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower },
                            p => p);
                        break;
                    case "未上市":
                        currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "待销");

                        querySale = currentYearCarList.GroupBy(
                            p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower },
                            p => p);
                        break;
                    case "停售":
                        //停售tab栏默认只显示当前所有停售车款里的最新年份
                        var stopCarYears = new List<string>();
                        stopCarYears =  _serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "停销")
                                .Select(p => p.CarYear)
                                .Distinct()
                                .ToList();
                        if (stopCarYears.Count > 0)
                        {
                            stopCarYears.Sort(NodeCompare.CompareStringDesc);
                            nearestYear = stopCarYears[0];
                        }
                        currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "停销" && p.CarYear == nearestYear);

                        querySale = currentYearCarList.GroupBy(
                            p => new {p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower },
                            p => p);
                        break;
                    default:
                        if (_serialEntity.SaleState == "停销")
                        {
                            currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "停销" && p.CarYear == year);
                            //取车系为停销状态的车款列表
                        }
                        else
                        {
                            currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "在销" && p.CarYear == year);
                            //取车系为在销状态的车款列表
                        }
                        querySale = currentYearCarList.GroupBy(
                            p =>
                                new
                                {
                                    p.Engine_Exhaust,
                                    p.Engine_InhaleType,
                                    p.Engine_AddPressType,
                                    p.Engine_MaxPower
                                },
                            p => p);
                        break;
                }

                int carCount = currentYearCarList.Count;
                querySale = currentYearCarList.Take(pageSize).GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower }, p => p);
                pageCount = carCount / pageSize + (carCount % pageSize == 0 ? 0 : 1);

                //start add by sk 2014-09-03 候姐 整组 停产 把整组移到最底 且保持前 排序规则
                var listGroupNew = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
                var listGroupOff = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
                foreach (var info in querySale)
                {
                    CarInfoForSerialSummaryEntity isStopState = info.FirstOrDefault(p => p.ProduceState != "停产");
                    if (isStopState != null)
                        listGroupNew.Add(info);
                    else
                        listGroupOff.Add(info);
                }
                listGroupNew.AddRange(listGroupOff);

                #endregion


                stringBuilder.AppendFormat("<div id='yearDiv{0}' {1} class='sum-car-type-box' pagecount='{2}'>",
                    counter,
                    counter > 0 ? "style='display:none;'" : "", pageCount);
                int num = 0;
                foreach (var info in listGroupNew)
                {
                    //if (num > 9)
                    //    break;

                    #region 基础信息准备

                    var key = CommonFunction.Cast(info.Key,
                        new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0 });
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
                        strMaxPowerAndInhaleType = string.Format("{0}{1}", maxPower, " " + inhaleType);
                    }

                    #endregion

                    stringBuilder.Append("<div class='tt-small'>");
                    stringBuilder.AppendFormat("<span>{0}{1}{2}</span>", key.Engine_Exhaust.Replace("L","升"),
                        (string.IsNullOrEmpty(key.Engine_Exhaust) || string.IsNullOrEmpty(strMaxPowerAndInhaleType))
                            ? ""
                            : "/", strMaxPowerAndInhaleType);
                    stringBuilder.Append("</div>");

                    stringBuilder.Append("<div class='car-card'>");
                    stringBuilder.Append("<ul>");

                    #region

                    List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList(); //分组后的集合

                    foreach (
                        CarInfoForSerialSummaryEntity carInfo in carGroupList)
                    {
                        //if (num > 9)
                        //    break;

                        string carFullName = "";
                        carFullName = carInfo.CarName;
                        if (carInfo.CarName.StartsWith(_serialShowName))
                        {
                            carFullName = carInfo.CarName.Substring(_serialShowName.Length);
                        }
                        if (year == "全部在售" || year == "未上市")
                        {
                            /////////////////////////////
                        }
                        carFullName = carInfo.CarYear + "款 " + carFullName;
                        string stopPrd = "";
                        if (carInfo.ProduceState == "停产")
                            stopPrd = "<em class='tingchan'>停产" + (_serialEntity.SaleState == "停销" ? "停售" : "在售") + "</em>";
                        if (carInfo.ProduceState == "停产"&&carInfo.SaleState=="停销")
                            stopPrd = "<em class='tingchan'>停售</em>";

                        string carMinPrice;
                        string carPriceRange = carInfo.CarPriceRange.Trim();
                        if (carInfo.SaleState == "待销")//顾晓 确认的逻辑 （待销的车款没有价格，全部显示未上市） 2015-07-09
                        {
                            carMinPrice = "未上市";
                        }
                        else if (carInfo.CarPriceRange.Trim().Length == 0)
                        {
                            carMinPrice = "暂无报价";
                        }
                        else
                        {
                            if (carPriceRange.IndexOf('-') != -1)
                                carMinPrice = carPriceRange.Substring(0, carPriceRange.IndexOf('-')); //+ "万"
                            else
                                carMinPrice = carPriceRange;
                        }

                        Dictionary<int, string> dictCarParams = _carBLL.GetCarAllParamByCarID(carInfo.CarID);

                        #region 纯电动车续航里程

                        if (isElectrombile)
                        {
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
                            if (maxMileage > 0)
                            {
                                mileageRange = minMileage == maxMileage
                                    ? string.Format("{0}公里", minMileage)
                                    : string.Format("{0}-{1}公里", minMileage, maxMileage);
                            }
                        }

                        #endregion


                        // 档位个数
                        string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" &&
                                                 dictCarParams[724] != "待查")
                            ? dictCarParams[724] + "挡"
                            : "";

                        //平行进口车标签
                        string parallelImport = "";
                        if (dictCarParams.ContainsKey(382) && dictCarParams[382] == "平行进口")
                        {
                            parallelImport = "<em>平行进口</em>";
                        }

                        stringBuilder.Append("<li>");

                        stringBuilder.AppendFormat(
                            "<a  id='carlist_" + carInfo.CarID + "' class='car-info' href='{0}'>",
                             "/" + _serialAllSpell + "/m" + carInfo.CarID + "/");

                        stringBuilder.AppendFormat("<h2>{0}</h2>", carFullName);
                        stringBuilder.AppendFormat("<span>{0}</span>{1}{2}", forwardGearNum + carInfo.TransmissionType,
                            parallelImport, stopPrd);
                        stringBuilder.AppendFormat("<dl><dt>{0}</dt><dd>指导价：{1}</dd></dl>", carMinPrice,
                            carInfo.ReferPrice.Trim().Length == 0 ? "暂无" : carInfo.ReferPrice.Trim() + "万");
                        stringBuilder.Append("</a>");

                        stringBuilder.Append("<ul class='car-btn'>");
                        stringBuilder.AppendFormat(
                            "<li><a id=\"car-compare-{0}\" href=\"#compare\" data-action=\"car\" data-id=\"{0}\" data-name=\"{1} {2}\">加入对比</a></li>",
                            carInfo.CarID, _serialName, carInfo.CarName);
                        stringBuilder.AppendFormat(
                            "<li><a id = \"car_filter_id_{0}_{1}\" href='/gouchejisuanqi/?carID={0}'>购车计算</a></li>",
                            carInfo.CarID, counter);

                        if (carInfo.SaleState != "停销")
                        {
                            stringBuilder.AppendFormat(
                            "<li class=\"btn-org\"><a id =\"car_filterzuidi_id_{0}_{1}\" href=\"http://price.m.yiche.com/zuidijia/nc{0}/?leads_source=20018\">询底价</a></li>",
                            carInfo.CarID, counter);
                        }
                        else
                        {
                            stringBuilder.AppendFormat("<li class='btn-org'><a href='http://m.taoche.com/all/?carid={0}&WT.mc_id=yichezswap'>买二手车</a></li>", carInfo.CarID);
                        }
                        stringBuilder.Append("</ul>");

                        stringBuilder.Append("</li>");
                        //num++;
                    }

                    #endregion

                    stringBuilder.Append("</ul>");
                    stringBuilder.Append("</div>");
                }
                //if (carCount > pageSize)
                if (pageCount > 1)
                {
                    //stringBuilder.AppendFormat(
                    //    "<asp:literal id='litLoadMore{0}' runat='server'><a id='btnMoreCar{0}' class='btn-more btn-add-more' href='javascript:void(0);'><i>加载更多</i></a></asp:literal>",
                    //    counter);

                    stringBuilder.AppendFormat("<a id='btnMoreCar{0}' page='2' class='btn-more btn-add-more' href='javascript:void(0);'><i>加载更多</i></a>", counter);
                }
                stringBuilder.Append("</div>");
                counter++;
            }
            stringBuilder.Append("</div>");
        }
         */
        private void GetCarHtml(IEnumerable<string> yearList, StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat("<div id='carList{0}'>", _serialId);
            int counter = 0;
            int minMileage = 0;
            int maxMileage = 0;
            bool flag = true;
            foreach (string item in yearList)
            {
                stringBuilder.AppendFormat("<div id='yearDiv{0}' {1} class='sum-car-type-box' pagecount='{2}'>",
                    counter,
                    counter > 0 ? "style='display:none;'" : "", pageCount);
                if (flag)
                {
                    #region 数据筛选

                    string year = item;
                    List<CarInfoForSerialSummaryEntity> currentYearCarList;
                    IEnumerable<IGrouping<object, CarInfoForSerialSummaryEntity>> querySale;

                    maxPv = _serialCarList.Max(m => m.CarPV);
                    switch (year)
                    {
                        case "全部在售":
                            currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "在销");

                            querySale = currentYearCarList.GroupBy(
                                p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower },
                                p => p);
                            break;
                        case "未上市":
                            currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "待销");

                            querySale = currentYearCarList.GroupBy(
                                p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower },
                                p => p);
                            break;
                        case "停售":
                            //停售tab栏默认只显示当前所有停售车款里的最新年份
                            var stopCarYears = new List<string>();
                            stopCarYears = _serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "停销")
                                    .Select(p => p.CarYear)
                                    .Distinct()
                                    .ToList();
                            if (stopCarYears.Count > 0)
                            {
                                stopCarYears.Sort(NodeCompare.CompareStringDesc);
                                nearestYear = stopCarYears[0];
                            }
                            currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "停销" && p.CarYear == nearestYear);

                            querySale = currentYearCarList.GroupBy(
                                p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower },
                                p => p);
                            break;
                        default:
                            if (_serialEntity.SaleState == "停销")
                            {
                                currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "停销" && p.CarYear == year);
                                //取车系为停销状态的车款列表
                            }
                            else
                            {
                                currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "在销" && p.CarYear == year);
                                //取车系为在销状态的车款列表
                            }
                            querySale = currentYearCarList.GroupBy(
                                p =>
                                    new
                                    {
                                        p.Engine_Exhaust,
                                        p.Engine_InhaleType,
                                        p.Engine_AddPressType,
                                        p.Engine_MaxPower,
                                        p.Electric_Peakpower
                                    },
                                p => p);
                            break;
                    }

                    int carCount = currentYearCarList.Count;
                    querySale = currentYearCarList.Take(pageSize).GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower }, p => p);
                    pageCount = carCount / pageSize + (carCount % pageSize == 0 ? 0 : 1);

                    //start add by sk 2014-09-03 候姐 整组 停产 把整组移到最底 且保持前 排序规则
                    var listGroupNew = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
                    var listGroupOff = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
                    foreach (var info in querySale)
                    {
                        CarInfoForSerialSummaryEntity isStopState = info.FirstOrDefault(p => p.ProduceState != "停产");
                        if (isStopState != null)
                            listGroupNew.Add(info);
                        else
                            listGroupOff.Add(info);
                    }
                    listGroupNew.AddRange(listGroupOff);

                    #endregion

                    foreach (var info in listGroupNew)
                    {
                        #region 基础信息准备

                        var key = CommonFunction.Cast(info.Key,
                            new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0, Electric_Peakpower = 0 });
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
                            strMaxPowerAndInhaleType = string.Format("{0}{1}", maxPower, " " + inhaleType);
                        }

                        #endregion

                        stringBuilder.Append("<div class='tt-small'>");
                        stringBuilder.AppendFormat("<span>{0}{1}{2}</span>", key.Engine_Exhaust.Replace("L", "升"),
                            (string.IsNullOrEmpty(key.Engine_Exhaust) || string.IsNullOrEmpty(strMaxPowerAndInhaleType))
                                ? ""
                                : "/", strMaxPowerAndInhaleType);
                        stringBuilder.Append("</div>");

                        stringBuilder.Append("<div class='car-card'>");
                        stringBuilder.Append("<ul>");

                        #region

                        List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList(); //分组后的集合


                        foreach (CarInfoForSerialSummaryEntity carInfo in carGroupList)
                        {
                            //if (num > 9)
                            //    break;

                            string carFullName = "";
                            carFullName = carInfo.CarName;
                            if (carInfo.CarName.StartsWith(_serialShowName))
                            {
                                carFullName = carInfo.CarName.Substring(_serialShowName.Length);
                            }
                            if (year == "全部在售" || year == "未上市")
                            {
                                /////////////////////////////
                            }
                            carFullName = carInfo.CarYear + "款 " + carFullName;
                            string stopPrd = "";
                            if (carInfo.ProduceState == "停产")
                                stopPrd = "<em class='tingchan'>停产" + (_serialEntity.SaleState == "停销" ? "停售" : "在售") + "</em>";
                            if (carInfo.ProduceState == "停产" && carInfo.SaleState == "停销")
                                stopPrd = "<em class='tingchan'>停售</em>";

                            string carMinPrice;
                            string carPriceRange = carInfo.CarPriceRange.Trim();
                            if (carInfo.SaleState == "待销")//顾晓 确认的逻辑 （待销的车款没有价格，全部显示未上市） 2015-07-09
                            {
                                carMinPrice = "未上市";
                            }
                            else if (carInfo.CarPriceRange.Trim().Length == 0)
                            {
                                carMinPrice = "暂无报价";
                            }
                            else
                            {
                                if (carPriceRange.IndexOf('-') != -1)
                                    carMinPrice = carPriceRange.Substring(0, carPriceRange.IndexOf('-')); //+ "万"
                                else
                                    carMinPrice = carPriceRange;
                            }

                            Dictionary<int, string> dictCarParams = _carBLL.GetCarAllParamByCarID(carInfo.CarID);

                            #region 纯电动车续航里程

                            if (isElectrombile)
                            {
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
                                if (maxMileage > 0)
                                {
                                    mileageRange = minMileage == maxMileage
                                        ? string.Format("{0}公里", minMileage)
                                        : string.Format("{0}-{1}公里", minMileage, maxMileage);
                                }
                            }

                            #endregion


                            // 档位个数
                            string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" &&
                                                     dictCarParams[724] != "待查")
                                ? dictCarParams[724] + "挡"
                                : "";

                            //平行进口车标签
                            string parallelImport = "";
                            if (dictCarParams.ContainsKey(382) && dictCarParams[382] == "平行进口")
                            {
                                parallelImport = "<em>平行进口</em>";
                            }

                            stringBuilder.Append("<li>");

                            stringBuilder.AppendFormat(
                                "<a  id='carlist_" + carInfo.CarID + "' class='car-info' href='{0}' data-channelid=\"27.23.915\">",
                                 "/" + _serialAllSpell + "/m" + carInfo.CarID + "/");

                            stringBuilder.AppendFormat("<h2>{0}</h2>", carFullName);
                            stringBuilder.AppendFormat("<span>{0}</span>{1}{2}", forwardGearNum + carInfo.TransmissionType,
                                parallelImport, stopPrd);
                            stringBuilder.AppendFormat("<dl><dt>{0}</dt><dd>指导价：{1}</dd></dl>", carMinPrice,
                                carInfo.ReferPrice.Trim().Length == 0 ? "暂无" : carInfo.ReferPrice.Trim() + "万");
                            //add date :2016-2-3  添加热度
                            int percent = 0;
                            if (maxPv > 0)
                            { percent = (int)Math.Round((double)carInfo.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero); }
                            //减税 购置税
                            string strTravelTax = "<div class=\"tap-box\"></div>";
                            double dEx = 0.0;
                            Double.TryParse(carInfo.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
                            if (carInfo.SaleState == "在销")
                            {
                                if (dictCarParams.ContainsKey(987) && (dictCarParams[987] == "第1批" || dictCarParams[987] == "第2批" || dictCarParams[987] == "第3批" || dictCarParams[987] == "第4批" || dictCarParams[987] == "第5批" || dictCarParams[987] == "第6批") && dictCarParams.ContainsKey(986))
                                {
                                    if (dictCarParams[986] == "免征")
                                    {
                                        strTravelTax = " <div class=\"tap-box\"><em class=\"bt\">免税</em></div>";
                                    }
                                    else
                                    {
                                        strTravelTax = " <div class=\"tap-box\"><em class=\"bt\">减税</em></div>";
                                    }
                                }
                                else if (dEx > 0 && dEx <= 1.6)
                                {
                                    strTravelTax = " <div class=\"tap-box\"><em class=\"bt\">减税</em></div>";
                                }
                            }

                            stringBuilder.AppendFormat("<div class=\"gzd-box\" style=\"\"><div class=\"tit-box\">关注度</div><span class=\"gz-sty\"><i data-pv=\"{0}\" style=\"width:{0}%\"></i></span>{1}</div>", percent, strTravelTax);
                            stringBuilder.Append("</a>");

                            bool maiBtnFlag = false;
                            if (year != "未上市" && year != "停售" && carInfo.SaleState != "待销" && carInfo.SaleState != "停销")
                            { maiBtnFlag = true; }
                            string ulStyle = "car-btn";
                            if (!maiBtnFlag)
                            {
                                ulStyle = "car-btn car-btn-three";
                            }
                            stringBuilder.Append("<ul class='" + ulStyle + "'>");
                            stringBuilder.AppendFormat(
                                "<li><a id=\"car-compare-{0}\" href=\"#compare\" class=\"btnDuibi\" data-action=\"car\" data-id=\"{0}\" data-name=\"{1} {2}\" data-channelid=\"27.23.910\">加入对比</a></li>",
                                carInfo.CarID, _serialName, carInfo.CarName);
                            stringBuilder.AppendFormat(
                                "<li><a id = \"car_filter_id_{0}_{1}\" href='/gouchejisuanqi/?carID={0}' data-channelid=\"27.23.911\">购车计算</a></li>",
                                carInfo.CarID, counter);
                            if (maiBtnFlag)
                            {
                                stringBuilder.AppendFormat("<li><a data-car=\"{0}\" href='javascript:void(0)' class=\"btn-mmm\"  data-action=\"mmm\" data-channelid=\"27.23.1321\">买买买</a></li>", carInfo.CarID);
                            }
                            if (carInfo.SaleState != "停销")
                            {
                                stringBuilder.AppendFormat(
                                "<li class=\"btn-org\"><a id =\"car_filterzuidi_id_{0}_{1}\" href=\"http://price.m.yiche.com/zuidijia/nc{0}/?leads_source=m002008\" data-channelid=\"27.23.912\">询底价</a></li>",
                                carInfo.CarID, counter);
                            }
                            else
                            {
                                stringBuilder.AppendFormat("<li class='btn-org'><a href='http://m.taoche.com/all/?carid={0}&WT.mc_id=yichezswap&leads_source=m002015' data-channelid=\"27.23.913\">买二手车</a></li>", carInfo.CarID);
                            }
                            stringBuilder.Append("</ul>");

                            stringBuilder.Append("</li>");
                            //num++;
                        }

                        #endregion

                        stringBuilder.Append("</ul>");
                        stringBuilder.Append("</div>");
                    }
                    if (pageCount > 1)
                    {
                        stringBuilder.AppendFormat("<a id='btnLoadNext{0}' page='2'  class='btn-more btn-add-more b-shadow' href='javascript:void(0);'><i>加载更多</i></a>", counter);
                    }
                }
                flag = false;
                stringBuilder.Append("</div>");
                counter++;
            }
            stringBuilder.Append("</div>");
        }

        //protected Dictionary<string, List<int>> DaogouDictionary = new Dictionary<string, List<int>>();
        //private void InitDaoGouData()
        //{
        //    string path = HttpContext.Current.Server.MapPath("~/config/DaoGouData.xml");
        //    if (!File.Exists(path)) return;

        //    XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(path);
        //    XmlNodeList nodeList = xmlDoc.SelectNodes("/Root/Group");
        //    if (nodeList == null || nodeList.Count <= 0) return;

        //    foreach (XmlNode node in nodeList)
        //    {
        //        if (node.Attributes == null) continue;

        //        string currentTopic = node.Attributes["topic"].Value;

        //        XmlNodeList itemNodeList = node.SelectNodes("Item");
        //        if (itemNodeList == null) continue;

        //        List<int> list = new List<int>();

        //        foreach (XmlNode subXmlNode in itemNodeList)
        //        {
        //            if (subXmlNode.Attributes == null) continue;
        //            int serialId = ConvertHelper.GetInteger(subXmlNode.Attributes["csid"].Value);
        //            list.Add(serialId);
        //        }
        //        if (list.Contains(_serialId))
        //        {
        //            DaogouDictionary.Add(currentTopic, list);
        //        }
        //    }
        //}
        #endregion
    }
}