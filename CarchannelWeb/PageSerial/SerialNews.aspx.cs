﻿using System;
using System.Xml;
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

namespace BitAuto.CarChannel.CarchannelWeb.PageSerial
{
    public partial class SerialNews : PageBase
    {
        protected string levelName;			//级别名称
        protected string levelSpell;		//级别全拼
        protected string serialShowName;	//子品牌全名称
        protected string serialSeoName;     //子品牌SEO名称
        protected string masterBrandName;	//主品牌名称
        protected string serialName;		//子品牌名称
        private string serialSpell;			//子品牌全拼
        protected int serialId;				//子品牌ID
        private int pageSize = 10;			//页大小
        private int pageIndex;				//页号
        protected string newsType;			//类别，是导购还是市场
        protected CarNewsType _CarNewsType;
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
        protected Car_SerialEntity cse;
        protected string baaUrl;
        protected string pingCeAndDaoGouSeo = string.Empty;

        protected string nextSeePingceHtml;
        protected string nextSeeXinwenHtml;
        protected string nextSeeDaogouHtml;

        protected string UCarHtml;

        protected string _InitUrl = string.Empty;
        protected string _ProvinceList = string.Empty;
        protected string _CityList = string.Empty;
        protected string HangQingDealerHTML = string.Empty;
        protected HangQingTree _HangQingTree = new HangQingTree();

        protected int _ProvinceId = -1;
        protected int _CityId = -1;
        protected string _ProvinceName = string.Empty;
        protected string _ProvinceAllSpell = string.Empty;
        protected string _CityName = string.Empty;
        protected string _CityAllSpell = string.Empty;
        protected bool _IsCityOrProvince = false;
        protected string _IsShowCityAndLine = "none";
        // 如果是4个直辖市的话 城市ID
        private int SpecialCityID = -1;

        protected bool isHangQingPage = false;

        // 城市ID 包括4个直辖市 广告使用
        protected string cityIDForAD = string.Empty;

        // 如果当前是行情 的话 可以有城市Spell
        // private string queryStringCitySpell = string.Empty;

        protected string seoCityName = string.Empty;

        private int _CurrentNewsCount = 0;

        protected string serialToSeeHtml = string.Empty;//看了还看

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(10);

            _CityAllSpell = Request.QueryString["city"];
            if (Request.QueryString["type"] == "hangqing" && !string.IsNullOrEmpty(_CityAllSpell))
            {
                InitCity();
            }
            GetParameter();
            if (newsType == "hangqing")
            {
                isHangQingPage = true;
                // 如果是行情 且无城市或全国 则跳转至 全国行情link
                // modified by chengl Nov.2.2011
                if (string.IsNullOrEmpty(_CityAllSpell))
                {
                    string targetURL = "/" + cse.Cs_AllSpell + "/hangqing/quanguo/";
                    Response.Status = "301 Moved Permanently";
                    Response.AddHeader("Location", targetURL.ToLower());
                }
                else
                {
                    //InitCity();
                    //生成省份列表
                    //PrinfProvinceList();
                    PrinfProvinceListNew();
                    HangQingDealerHTML = RenderHangQingDealer();
                    #region cityid for AD
                    // cityid for AD 
                    if (_CityId > 0)
                    {
                        cityIDForAD = _CityId.ToString();
                    }
                    else if (SpecialCityID > 0)
                    {
                        cityIDForAD = SpecialCityID.ToString();
                    }
                    else
                    {
                        cityIDForAD = "";
                    }
                    #endregion
                    //newsHtml = RenderNewsList();
                    newsHtml = RenderNewsListNew();
                    //==================================
                    //2012-04-01 修改无相关文章消息提示。
                    //==================================
                    newsHtml = string.IsNullOrEmpty(newsHtml) ? "暂无行情文章" : newsHtml;
                    //==================================
                    newsNavHtml = RenderXinWenTitle();
                    MakeHotSerialCompare();
                    InitNextSeeNew();
                }
            }
            else
            {

                base.MakeSerialTopADCode(serialId);
                //InitNextSee();
                InitNextSeeNew();
                RenderContent();
            }

        }

        private void GetParameter()
        {
            // modified by chengl Sep.6.2012
            serialId = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["id"])
                && int.TryParse(Request.QueryString["id"].ToString(), out serialId))
            { }
            _CityAllSpell = Request.QueryString["city"];

            string catchKeyEntity = "CsSummaryEntity_CsID" + serialId.ToString();
            object serialInfoEntityByCsID = null;
            CacheManager.GetCachedData(catchKeyEntity, out serialInfoEntityByCsID);
            cse = new Car_SerialEntity();
            if (serialInfoEntityByCsID == null)
            {
                cse = (new Car_SerialBll()).Get_Car_SerialByCsID(serialId);
                CacheManager.InsertCache(catchKeyEntity, cse, 60);
            }
            else
            {
                cse = (Car_SerialEntity)serialInfoEntityByCsID;
            }

            // add by chengl May.17.2012 高岩要求开放概念车
            //if (cse.Cs_CarLevel == "概念车")
            //{
            //    Response.Redirect("/404error.aspx?info=概念车无新闻页");
            //}

            baaUrl = new Car_SerialBll().GetForumUrlBySerialId(serialId);

            //UCarHtml = new Car_SerialBll().GetUCarHtml(serialId);

            newsType = Request.QueryString["type"];
            if (newsType == null)
                newsType = "xinwen";
            newsType = newsType.ToLower();

            switch (newsType)
            {
                //modify by zf May.18.2016  朱江要求，暂时只改车型“文章”下的“全部”类别的tdk
                case "wenzhang":
                    pageTitle = "【{0}新闻】{1}{2}新闻_最新{0}报道-易车网";
                    pageKeywords = "{0}新闻,{1}{2}新闻,{0}上市新闻,{0}导购,易车网,car.bitauto.com";
                    pageDescription = "{1}{2}新闻,易车网提供最权威的{0}新闻资讯、{0}最新行情资讯、{0}优惠、{0}经销商降价信息、网友点评讨论等。";
                    pingCeAndDaoGouSeo = "<a href=\"/" + cse.Cs_AllSpell + "/daogou/\">导购</a>|<a href=\"/" + cse.Cs_AllSpell + "/pingce/\">评测</a>";
                    _CarNewsType = CarNewsType.wenzhang;
                    break;
                case "xinwen":
                    pageTitle = "【{0}新闻】_{0}上市新闻_{0}导购用车新闻-易车网";
                    pageKeywords = "{0}新闻,{0}上市新闻,{0},{0}导购";
                    pageDescription = "易车{0}新闻提供最权威的{0}新闻资讯、{0}上市新闻、{0}最新行情资讯、{0}优惠、{0}经销商降价信息、网友点评讨论等。{0}在线询价、底价买车，尽在易车网。";
                    pingCeAndDaoGouSeo = "<a href=\"/" + cse.Cs_AllSpell + "/daogou/\">导购</a>|<a href=\"/" + cse.Cs_AllSpell + "/pingce/\">评测</a>";
                    _CarNewsType = CarNewsType.xinwen;
                    break;
                case "daogou":
                    pageTitle = "【{0}导购-{0}促销信息】_{1}{0}-易车网";
                    pageKeywords = "{0}导购,{0}促销,{1}{2}";
                    pageDescription = "{0}导购:易车网车型频道为您提供最权威的{1}{0}评测导购资讯、最及时的{1}{0}优惠促销信息、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    pingCeAndDaoGouSeo = "导购 |<a href=\"/" + cse.Cs_AllSpell + "/pingce/\">评测</a>";
                    _CarNewsType = CarNewsType.daogou;
                    break;
                case "shijia":
                    pageTitle = "【{0}促销-{0}试驾】_{1}{0}-易车网";
                    pageKeywords = "{0}试驾,{0}促销,{1}{2}";
                    pageDescription = "{0}试驾:易车网车型频道为您提供最权威的{1}{0}试驾信息、最及时的{1}{0}优惠促销信息、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    pingCeAndDaoGouSeo = "<a href=\"/" + cse.Cs_AllSpell + "/daogou/\">导购</a>|<a href=\"/" + cse.Cs_AllSpell + "/pingce/\">评测</a>";
                    _CarNewsType = CarNewsType.shijia;
                    break;
                case "yongche":
                    pageTitle = "【{0}用车-{0}用车指南】_{1}{0}-易车网";
                    pageKeywords = "{0}用车,{0}用车指南,{1}{2}";
                    pageDescription = "{0}用车:易车网车型频道为您提供最权威的{1}{0}用车指南、最及时的{1}{0}优惠促销信息、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    pingCeAndDaoGouSeo = "<a href=\"/" + cse.Cs_AllSpell + "/daogou/\">导购</a>|<a href=\"/" + cse.Cs_AllSpell + "/pingce/\">评测</a>";
                    _CarNewsType = CarNewsType.yongche;
                    break;
                case "hangqing":
                    pageTitle = "【" + seoCityName + "{0}行情-" + seoCityName + "{0}行情新闻】_{1}{0}-易车网";
                    pageKeywords = "{0}行情,{0}行情新闻,{1}{2}";
                    pageDescription = "" + seoCityName + "{0}行情:易车网车型频道为您提供最权威的" + seoCityName + "{0}行情信息、" + seoCityName + "{0}优惠促销信息、{0}最新行情报价、{0}优惠、{0}经销商降价信息、网友点评讨论等。";
                    pingCeAndDaoGouSeo = "<a href=\"/" + cse.Cs_AllSpell + "/daogou/\">导购</a>|<a href=\"/" + cse.Cs_AllSpell + "/pingce/\">评测</a>";
                    _CarNewsType = CarNewsType.hangqing;
                    break;
                case "pingce":
                    pageTitle = "【{0}评测-{0}单车评测】_{1}-易车网BitAuto.com";
                    pageKeywords = "{0}评测,{0}单车评测,{1}{2}";
                    pageDescription = "{0}评测:易车网(BitAuto.com)导购频道为您提供最权威的{0}单车评测、最及时的{0}优惠促销信息、网友点评讨论等。";
                    pingCeAndDaoGouSeo = "<a href=\"/" + cse.Cs_AllSpell + "/daogou/\">导购</a>| 评测 ";
                    _CarNewsType = CarNewsType.pingce;
                    break;
                case "gaizhuang":
                    pageTitle = "【{0}改装-{0}单车改装指南】_{1}{0}-易车网";
                    pageKeywords = "{0}改装,{0}单车改装,{1}{2}";
                    pageDescription = "{0}改装:易车网车型频道为您提供最权威的{1}{0}改装指南、最及时的{1}{0}内饰改装资讯、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    pingCeAndDaoGouSeo = "<a href=\"/" + cse.Cs_AllSpell + "/daogou/\">导购</a>|<a href=\"/" + cse.Cs_AllSpell + "/pingce/\">评测</a>";
                    _CarNewsType = CarNewsType.gaizhuang;
                    break;
                case "anquan":
                    pageTitle = "【{0}安全-{0}碰撞安全测试】_{1}{0}-易车网";
                    pageKeywords = "{0}安全,{0}碰撞测试,{1}{0}";
                    pageDescription = "{0}安全:易车网车型频道为您提供最权威的{1}{0}安全指南、最及时的{1}{0}碰撞安全测试、{0}最新报价、{0}价格、{0}经销商降价信息、网友点评讨论等。";
                    pingCeAndDaoGouSeo = "<a href=\"/" + cse.Cs_AllSpell + "/daogou/\">导购</a>|<a href=\"/" + cse.Cs_AllSpell + "/pingce/\">评测</a>";
                    _CarNewsType = CarNewsType.anquan;
                    break;
                default:
                    newsType = "xinwen";
                    pageTitle = "【{0}新闻】_{0}上市新闻_{0}导购用车新闻-易车网";
                    pageKeywords = "{0}新闻,{0}上市新闻,{0},{0}导购";
                    pageDescription = "易车{0}新闻提供最权威的{0}新闻资讯、{0}上市新闻、{0}最新行情资讯、{0}优惠、{0}经销商降价信息、网友点评讨论等。{0}在线询价、底价买车，尽在易车网。";
                    pingCeAndDaoGouSeo = "<a href=\"/" + cse.Cs_AllSpell + "/daogou/\">导购</a>|<a href=\"/" + cse.Cs_AllSpell + "/pingce/\">评测</a>";
                    _CarNewsType = CarNewsType.xinwen;
                    break;
            }

            pageIndex = ConvertHelper.GetInteger(Request.QueryString["pageIndex"]);
            if (pageIndex == 0)
                pageIndex = 1;



            Car_SerialBaseEntity serialBase = new Car_SerialBll().GetSerialBaseEntity(serialId);
            if (serialBase != null)
            {
                serialShowName = serialBase.SerialShowName;
                if (serialId == 1568)
                    serialShowName = "索纳塔八";
                levelName = serialBase.SerialLevel;
                levelSpell = serialBase.SerialLevelSpell;
                serialSpell = serialBase.SerialNameSpell;
                masterBrandName = serialBase.MasterBrandName;
                serialName = serialBase.SerialName;

                if (newsType == "daogou")
                {
                    pageTitle = String.Format(pageTitle, serialBase.SerialSeoName, serialBase.MasterBrandName);
                    pageKeywords = String.Format(pageKeywords, serialBase.SerialSeoName, serialBase.MasterBrandName, serialShowName);
                    pageDescription = String.Format(pageDescription, serialBase.SerialSeoName, serialBase.MasterBrandName);
                }
                else
                {
                    pageTitle = String.Format(pageTitle, serialBase.SerialSeoName, serialBase.MasterBrandName, serialName);
                    pageKeywords = String.Format(pageKeywords, serialBase.SerialSeoName, serialBase.MasterBrandName, serialShowName, serialName);
                    pageDescription = String.Format(pageDescription, serialBase.SerialSeoName, serialBase.MasterBrandName, serialName);
                }


                #region 取头
                // bool isSuccess = false;
                string tagName = "Cs" + newsType;
                //if (newsType == "shichang")
                //{ tagName = "CsShiChang"; }
                // SerialNewHead = this.GetRequestString(string.Format(WebConfig.HeadForSerial, serialId.ToString(), tagName), 10, out isSuccess);
                SerialNewHead = base.GetCommonNavigation(tagName, serialId);
                #endregion
            }
            else
                Response.Redirect("car.bitauto.com");
        }

        #region 新行情

        /// <summary>
        /// 初始化城市对象
        /// </summary>
        private void InitCity()
        {
            //如果用户没有请求
            if (string.IsNullOrEmpty(_CityAllSpell)) return;

            string provPath = "root/Province[@EngName='{0}']";
            string cityPath = "root/Province/City[@EngName='{0}']";

            XmlDocument xmlDoc = _HangQingTree.GetProvinceAndCityRelationXmlDocument();
            if (xmlDoc == null)
            {
                _ProvinceId = 0;
                _ProvinceName = "全国";
                _IsCityOrProvince = false;
            }

            XmlNode entityNode = xmlDoc.SelectSingleNode(string.Format(provPath, _CityAllSpell));
            if (entityNode != null)
            {
                _ProvinceId = ConvertHelper.GetInteger(((XmlElement)entityNode).GetAttribute("ID"));
                _ProvinceName = ((XmlElement)entityNode).GetAttribute("Name");
                _ProvinceAllSpell = ((XmlElement)entityNode).GetAttribute("EngName");

                // 如果是4个直辖市的话
                XmlNode entitySpecialNode = xmlDoc.SelectSingleNode(string.Format(cityPath, _CityAllSpell));
                if (entitySpecialNode != null)
                {
                    SpecialCityID = ConvertHelper.GetInteger(((XmlElement)entitySpecialNode).GetAttribute("ID"));
                }
                seoCityName = _ProvinceName;
                return;
            }
            else
            {
                entityNode = xmlDoc.SelectSingleNode(string.Format(cityPath, _CityAllSpell));
                if (entityNode != null)
                {
                    _CityId = ConvertHelper.GetInteger(((XmlElement)entityNode).GetAttribute("ID"));
                    _CityName = ((XmlElement)entityNode).GetAttribute("Name");
                    _CityAllSpell = ((XmlElement)entityNode).GetAttribute("EngName");
                    _ProvinceId = ConvertHelper.GetInteger(((XmlElement)entityNode.ParentNode).GetAttribute("ID"));
                    _ProvinceName = ((XmlElement)entityNode.ParentNode).GetAttribute("Name");
                    _ProvinceAllSpell = ((XmlElement)entityNode.ParentNode).GetAttribute("EngName");
                    _IsCityOrProvince = true;
                    seoCityName = _CityName;
                    return;
                }
            }
            _ProvinceId = 0;
            _ProvinceName = "全国";
            _IsCityOrProvince = false;

        }

        /// <summary>
        /// 打印省份列表
        /// </summary>
        [Obsolete("新闻服务上线后，将由PrinfProvinceListNew方法代替。")]
        private void PrinfProvinceList()
        {
            _InitUrl = "/" + cse.Cs_AllSpell + "/hangqing/";
            List<XmlElement> provinceList = _HangQingTree.GetProvinceNewsNumber();
            XmlElement currentXmlElement = null;
            //如果列表中不存在数据
            if (provinceList == null || provinceList.Count < 1) return;
            int count = 0;
            int index = 0;
            string isShowProvince = string.Empty;
            string isNoSelect = "<li style=\"display:{4}\"><a href=\"{0}{1}/\">{2}</a><em>({3})</em></li>";
            string isCurrentSelect = "<li class=\"current\"><a href=\"{0}{1}/\">{2}</a><em>({3})</em></li>";
            StringBuilder provList = new StringBuilder();
            int topThresholdValue = 13;
            int bottomThresholdValue = 12;
            //循环匹配省份信息
            foreach (XmlElement entity in provinceList)
            {
                int provinceId = ConvertHelper.GetInteger(entity.GetAttribute("ID"));
                string Name = entity.GetAttribute("Name");
                string engName = entity.GetAttribute("EngName");
                int number = _HangQingTree.GetSerialNewsNumberByIDAndCityId(serialId, false, provinceId);
                if (number < 1) continue;
                count++;
                if (count > bottomThresholdValue) isShowProvince = "none";//如果显示省份大于18个，则要把显示设置为不显示
                if (provinceId == _ProvinceId)
                {
                    index = count;
                    currentXmlElement = entity;
                    provList.AppendFormat(isCurrentSelect, _InitUrl, engName.ToLower(), Name, number);
                    continue;
                }

                provList.AppendFormat(isNoSelect, _InitUrl, engName.ToLower(), Name, number, count <= topThresholdValue ? "block" : "@display@");
            }

            if (index > topThresholdValue) isShowProvince = "block";
            provList = provList.Replace("@display@", isShowProvince);
            if (index <= topThresholdValue && count > bottomThresholdValue)
            {
                provList.Append("<li class=\"showAllBtn\" id=\"showAllBtn\" style=\"display: block;\"><a href=\"javascript:void(0)\">展开全部&gt;&gt;</a></li>");
            }

            _ProvinceList = provList.ToString();
            //PrintfCityList(currentXmlElement);
            PrintfCityListNew(currentXmlElement);
        }
        /// <summary>
        /// 打印省份列表
        /// </summary>
        private void PrinfProvinceListNew()
        {
            _InitUrl = "/" + cse.Cs_AllSpell + "/hangqing/";
            XmlDocument relDoc = _HangQingTree.GetProvinceAndCityRelationXmlDocument();
            if (relDoc == null) return;
            Dictionary<int, int> newsCount = new CarNewsBll().GetSerialCityNewsCount(serialId);
            if (newsCount == null || newsCount.Count < 1) return;

            XmlNodeList provinceList = relDoc.SelectNodes("/root/Province");
            //如果列表中不存在数据
            if (provinceList == null || provinceList.Count < 1) return;

            XmlElement currentXmlElement = null;
            int count = 0;
            int index = 0;
            string isShowProvince = string.Empty;
            string isNoSelect = "<li style=\"display:{4}\"><a href=\"{0}{1}/\">{2}</a><em>({3})</em></li>";
            string isCurrentSelect = "<li class=\"current\"><a href=\"{0}{1}/\">{2}</a><em>({3})</em></li>";
            StringBuilder provList = new StringBuilder();
            int topThresholdValue = 13;
            int bottomThresholdValue = 12;
            //循环匹配省份信息
            foreach (XmlElement entity in provinceList)
            {
                int provinceId = ConvertHelper.GetInteger(entity.GetAttribute("ID"));
                if (!newsCount.ContainsKey(provinceId))
                    continue;

                string Name = entity.GetAttribute("Name");
                string engName = entity.GetAttribute("EngName");
                int number = newsCount[provinceId];
                count++;
                if (count > bottomThresholdValue) isShowProvince = "none";//如果显示省份大于18个，则要把显示设置为不显示
                if (provinceId == _ProvinceId)
                {
                    index = count;
                    currentXmlElement = entity;
                    provList.AppendFormat(isCurrentSelect, _InitUrl, engName.ToLower(), Name, number);
                    continue;
                }

                provList.AppendFormat(isNoSelect, _InitUrl, engName.ToLower(), Name, number, count <= topThresholdValue ? "block" : "@display@");
            }

            if (index > topThresholdValue) isShowProvince = "block";
            provList = provList.Replace("@display@", isShowProvince);
            if (index <= topThresholdValue && count > bottomThresholdValue)
            {
                provList.Append("<li class=\"showAllBtn\" id=\"showAllBtn\" style=\"display: block;\"><a href=\"javascript:void(0)\">展开全部&gt;&gt;</a></li>");
            }

            _ProvinceList = provList.ToString();
            PrintfCityListNew(currentXmlElement);
        }
        /// <summary>
        /// 打印省下城市的列表
        /// </summary>
        /// <param name="provinceElement"></param>
        [Obsolete("新闻服务上线后，将由PrintfCityListNew方法代替。")]
        private void PrintfCityList(XmlElement provinceElement)
        {
            if (provinceElement == null) return;
            int provinceId = ConvertHelper.GetInteger(provinceElement.GetAttribute("ID"));
            if (provinceId < 0) return;
            if (provinceElement.ChildNodes.Count < 1) return;
            //开始进行城市的获取
            Dictionary<int, int> _Municipalities = _HangQingTree._Municipalities;
            if (_Municipalities != null && _Municipalities.ContainsKey(provinceId))
            {
                _IsShowCityAndLine = "none";
                return;
            }

            StringBuilder cityStringBuilder = new StringBuilder();
            cityStringBuilder.AppendFormat("<li><b>{0}：</b></li>", provinceElement.GetAttribute("Name"));
            string isNoSelect = "<li><a href=\"{0}{1}/\">{2}</a><em>({3})</em></li>";
            string isSelect = "<li class=\"current\"><a href=\"{0}{1}/\">{2}</a><em>({3})</em></li>";
            int counter = 0;

            List<XmlElement> cityXmlList = new List<XmlElement>();

            foreach (XmlElement entity in provinceElement.ChildNodes)
            {
                cityXmlList.Add(entity);
            }
            //排序列表
            cityXmlList.Sort(NodeCompare.CompareProvinceOrder);
            Dictionary<int, City> dic348 = AutoStorageService.GetCityNameIdList();

            foreach (XmlElement entity in cityXmlList)
            {
                int cityId = ConvertHelper.GetInteger(entity.GetAttribute("ID"));
                if (!dic348.ContainsKey(cityId)) continue;
                string Name = entity.GetAttribute("Name");
                string engName = entity.GetAttribute("EngName");
                int number = _HangQingTree.GetSerialNewsNumberByIDAndCityId(serialId, true, cityId);
                if (number < 1) continue;
                counter++;
                if (_CityId == cityId)
                {
                    cityStringBuilder.AppendFormat(isSelect, _InitUrl, engName.ToLower(), Name, number);
                    continue;
                }

                cityStringBuilder.AppendFormat(isNoSelect, _InitUrl, engName.ToLower(), Name, number);
            }
            if (counter > 0) _IsShowCityAndLine = "block";
            _CityList = cityStringBuilder.ToString();

        }
        /// <summary>
        /// 打印省下城市的列表
        /// </summary>
        private void PrintfCityListNew(XmlElement provinceElement)
        {
            if (provinceElement == null) return;
            int provinceId = ConvertHelper.GetInteger(provinceElement.GetAttribute("ID"));
            if (provinceId < 0) return;
            if (provinceElement.ChildNodes.Count < 1) return;

            Dictionary<int, int> _CityNewsCount = new CarNewsBll().GetSerialCityNewsCount(serialId);
            if (_CityNewsCount == null || _CityNewsCount.Count < 1) return;

            //开始进行城市的获取
            Dictionary<int, int> _Municipalities = _HangQingTree._Municipalities;
            if (_Municipalities != null && _Municipalities.ContainsKey(provinceId))
            {
                _IsShowCityAndLine = "none";
                return;
            }

            StringBuilder cityStringBuilder = new StringBuilder();
            cityStringBuilder.AppendFormat("<li><b>{0}：</b></li>", provinceElement.GetAttribute("Name"));
            string isNoSelect = "<li><a href=\"{0}{1}/\">{2}</a><em>({3})</em></li>";
            string isSelect = "<li class=\"current\"><a href=\"{0}{1}/\">{2}</a><em>({3})</em></li>";
            int counter = 0;

            Dictionary<int, City> dic348 = AutoStorageService.GetCityNameIdList();

            foreach (XmlElement entity in provinceElement.ChildNodes)
            {
                int cityId = ConvertHelper.GetInteger(entity.GetAttribute("ID"));
                if (!dic348.ContainsKey(cityId)) continue;
                if (!_CityNewsCount.ContainsKey(cityId)) continue;
                string Name = entity.GetAttribute("Name");
                string engName = entity.GetAttribute("EngName");
                int number = _CityNewsCount[cityId];
                counter++;
                if (_CityId == cityId)
                {
                    cityStringBuilder.AppendFormat(isSelect, _InitUrl, engName.ToLower(), Name, number);
                    continue;
                }

                cityStringBuilder.AppendFormat(isNoSelect, _InitUrl, engName.ToLower(), Name, number);
            }
            if (counter > 0) _IsShowCityAndLine = "block";
            _CityList = cityStringBuilder.ToString();
        }
        /*
        /// <summary>
        /// 得到城市选择块
        /// </summary>
        /// <returns></returns>
        private string GetCitySelectSpan()
        {
            string objName = _IsCityOrProvince ? _CityName : _ProvinceName;
            StringBuilder cityMessageString = new StringBuilder();
            cityMessageString.AppendLine("<h3 style=\"overflow: visible;z-index:999; position:relative\">");
            cityMessageString.AppendFormat("<span>{0}{1}{2} - {3}</span>", cse.Cb_Name, cse.Cs_Name, "行情", objName);

            cityMessageString.AppendLine("</span>");
            cityMessageString.AppendLine("</h3>");
            return cityMessageString.ToString();
        }
        */

        /// <summary>
        /// 得到省份的新闻列表
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private List<int> GetCityList()
        {
            List<int> idList = new List<int>();
            if (_IsCityOrProvince && _CityId > 0)
            {
                idList.Add(_CityId);
                return idList;
            }
            else if (_IsCityOrProvince)
            {
                return null;
            }

            Dictionary<int, City> dic348 = AutoStorageService.GetCityNameIdList();

            string provPath = "root/Province[@ID={0}]/City";
            XmlDocument xmlDoc = _HangQingTree.GetProvinceAndCityRelation();
            if (xmlDoc == null) return null;

            XmlNodeList xNodeList = xmlDoc.SelectNodes(string.Format(provPath, _ProvinceId));
            if (xNodeList == null || xNodeList.Count < 1) return null;

            foreach (XmlElement entity in xNodeList)
            {
                int tempId = ConvertHelper.GetInteger(entity.GetAttribute("ID"));
                if (idList.Contains(tempId)) continue;
                if (!dic348.ContainsKey(tempId)) continue;
                idList.Add(tempId);
            }

            return idList;
        }

        #endregion

        /// <summary>
        /// 生成Html代码
        /// </summary>
        /// <returns></returns>
        private void RenderContent()
        {
            //newsHtml = RenderNewsList();
            newsHtml = RenderNewsListNew();

            //StringBuilder htmlCode = new StringBuilder();
            if (newsType == "hangqing")
            {
                newsNavHtml = RenderXinWenTitle();
                // 如果是行情 取行情经销商数据 
                // add Oct.10.2011 by chengl
                // HangQingDealerHTML = RenderHangQingDealer();
            }
            else
            {
                //newsNavHtml = RenderNewsNav();
                newsNavHtml = RenderNewsNavNew();
            }
            //==================================
            //2012-04-01 修改无相关文章消息提示。
            //==================================
            string msg = "";
            switch (newsType)
            {
                case "wenzhang": msg = "全部文章"; break;
                case "xinwen": msg = "新闻"; break;
                case "hangqing": msg = "行情"; break;
                case "daogou": msg = "导购"; break;
                case "yongche": msg = "用车"; break;
                case "gaizhuang": msg = "改装"; break;
                case "pingce": msg = "易车评测"; break;
                case "shijia": msg = "体验试驾"; break;
                case "anquan": msg = "安全"; break;
                default: break;
            }
            string noMsg = "暂无{0}文章";
            newsHtml = string.IsNullOrEmpty(newsHtml) ? string.Format(noMsg, msg) : newsHtml;
            //===========================
            MakeHotSerialCompare();
            //MakeSerialToSerialHtml();
            ucSerialToSee.SerialId = serialId;
            ucSerialToSee.SerialName = serialShowName;
        }
        /// <summary>
        /// 生成小导航代码
        /// </summary>
        private string RenderNewsNavNew()
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
            foreach (KeyValuePair<CarNewsType, string> entity in titleTag)
            {
                //所有文章
                if (entity.Key == CarNewsType.wenzhang)
                {
                    if (entity.Key == _CarNewsType)
                        listTemp.Add(string.Format("<li class=\"current\"><a href=\"/{0}/{1}/\">{2}</a><em>|</em></li>", cse.Cs_AllSpell.ToLower(), entity.Key.ToString(), entity.Value));
                    else
                        listTemp.Add(string.Format("<li><a href=\"/{0}/{1}/\">{2}</a><em>|</em></li>", cse.Cs_AllSpell.ToLower(), entity.Key.ToString(), entity.Value));
                    continue;
                }

                if (entity.Key == _CarNewsType)
                {
                    if (_CurrentNewsCount > 0)
                        listTemp.Add(string.Format("<li class=\"current\"><a href=\"/{1}/{2}/\">{0}</a><em>|</em></li>", entity.Value, cse.Cs_AllSpell, entity.Key.ToString(), _CurrentNewsCount.ToString()));
                    else
                        listTemp.Add(string.Format("<li class=\"current\"><a>{0}</a><em>|</em></li>", entity.Value));
                    continue;
                }
                int newsCount = newsBll.GetSerialNewsCount(serialId, 0, entity.Key);
                if (newsCount > 0)
                    listTemp.Add(string.Format("<li><a href=\"/{0}/{1}/\">{2}</a><em>|</em></li>", cse.Cs_AllSpell.ToLower(), entity.Key.ToString(), entity.Value, newsCount.ToString()));
                //else
                //{
                //    htmlCode.AppendFormat("<li><a class=\"nolink\">{0}</a></li>", entity.Value);
                //}
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
            string retStr = "";
            string title = "新闻";
            string baojiaUrl;
            string dealerUrl;
            if (newsType == "hangqing")
            {
                title = "行情";
                baojiaUrl = string.Format("http://car.bitauto.com/{0}/baojia/c{1}/", serialSpell, SpecialCityID >= 0 ? SpecialCityID : (_IsCityOrProvince ? _CityId : _ProvinceId));
                dealerUrl = string.Format("http://dealer.bitauto.com/{0}/{1}/", serialSpell, _IsCityOrProvince ? _CityAllSpell : _ProvinceAllSpell);
            }
            else
            {
                baojiaUrl = string.Format("http://car.bitauto.com/{0}/baojia/", serialSpell);
                dealerUrl = string.Format("http://dealer.bitauto.com/{0}/", serialSpell);
            }
            retStr = string.Format("<h3><span>{0}{1}</span></h3>", cse.Cs_ShowName, title);
            retStr += string.Format("<div class=\"more\" style=\"z-index:1001\"><a href=\"http://car.bitauto.com/{0}/\">车型</a> | <a href=\"{1}\" >报价</a> | <a target=\"_blank\" href=\"{2}\">经销商</a></div>"
                , serialSpell, baojiaUrl, dealerUrl);
            return retStr;
            //return string.Format("<h3><span>{0}{1}</span></h3>", cse.Cs_ShowName, title);
        }
        /// <summary>
        /// 将数据源转 行情数据按程序筛选
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="drs">返回处理后数据集</param>
        /// <param name="newsCount">新闻数量</param>
        private void GetDataRowArrayForNews(DataSet ds, ref DataRow[] drs, ref int newsCount)
        {
            newsCount = 0;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables["listNews"] != null && ds.Tables["listNews"].Rows.Count > 0)
            {
                if (newsType == "hangqing")
                {
                    // 行情新闻
                    //如果是全国
                    if (!_IsCityOrProvince && _ProvinceId == 0)
                    {
                        drs = ds.Tables["listNews"].Select("");
                    }

                    //省份包含的城市列表
                    List<int> cityList = GetCityList();
                    if (cityList == null || cityList.Count < 1)
                    {
                        newsCount = drs.Length;
                        return;
                    }
                    //已经加入的新闻列表
                    List<int> newsIdList = new List<int>();

                    DataTable newsDataTable = ds.Tables["listNews"].Clone();
                    foreach (DataRow dr in ds.Tables["listNews"].Rows)
                    {
                        int newsId = ConvertHelper.GetInteger(dr["newsid"]);
                        if (newsIdList.Contains(newsId)) continue;
                        string relationCity = dr["relatedcityid"].ToString();
                        if (string.IsNullOrEmpty(relationCity)) continue;
                        bool isContainsCityId = false;
                        //得到城市列表
                        string[] cityIdString = relationCity.Split(',');
                        List<int> cityIdList = new List<int>();

                        foreach (string idString in cityIdString)
                        {
                            if (string.IsNullOrEmpty(idString)) continue;
                            int tempId = ConvertHelper.GetInteger(idString);
                            if (tempId != 0 && !cityList.Contains(tempId))
                            {
                                continue;
                            }
                            isContainsCityId = true;
                            break;
                        }
                        if (!isContainsCityId) continue;
                        //将查询到的数据添加新闻数据中
                        DataRow newNewsDataRow = newsDataTable.NewRow();
                        newNewsDataRow["title"] = dr["title"];
                        newNewsDataRow["newsid"] = dr["newsid"];
                        newNewsDataRow["filepath"] = dr["filepath"];
                        newNewsDataRow["publishtime"] = dr["publishtime"];
                        newNewsDataRow["content"] = dr["content"];
                        newNewsDataRow["sourceName"] = dr["sourceName"];
                        newNewsDataRow["author"] = dr["author"];
                        if (ds.Tables["listNews"].Columns.Contains("CommentNum"))
                        {
                            newNewsDataRow["CommentNum"] = dr["CommentNum"];
                        }
                        newsDataTable.Rows.Add(newNewsDataRow);
                    }

                    //返回新闻列表按时间倒序排列
                    drs = newsDataTable.Select("", "");
                    newsCount = drs.Length;
                }
                else
                {
                    // 非行情新闻
                    drs = ds.Tables["listNews"].Select("");
                    if (ds.Tables["newsAllCount"] != null && ds.Tables["newsAllCount"].Rows.Count > 0)
                    {
                        newsCount = Convert.ToInt32(ds.Tables["newsAllCount"].Rows[0][0]);
                    }
                }
            }
        }
        private string RenderNewsListNew()
        {
            StringBuilder htmlCode = new StringBuilder();


            int newsCount = 0;
            DataSet newsDs = null;
            if (newsType == "hangqing")
            {
                // 行情新闻
                if (_IsCityOrProvince && _CityId > 0)
                    newsDs = new CarNewsBll().GetSerialCityNewsAllData(serialId, _CityId, pageSize, pageIndex, ref newsCount);
                else if (!_IsCityOrProvince && _ProvinceId > 0)
                    newsDs = new CarNewsBll().GetSerialProvinceNewsAllData(serialId, _ProvinceId, pageSize, pageIndex, ref newsCount);
                else
                    newsDs = new CarNewsBll().GetSerialNewsAllData(serialId, CarNewsType.hangqing, pageSize, pageIndex, ref newsCount);
            }
            else
            {
                if (newsType == "wenzhang")
                {
                    List<int> carTypeIdList = new List<int>() 
					{ 
                    (int)CarNewsType.serialfocus, //add 2016-10-11 by gux,liub
					(int)CarNewsType.shijia,
					(int)CarNewsType.daogou,
					(int)CarNewsType.yongche,
					(int)CarNewsType.gaizhuang,
					(int)CarNewsType.anquan,
					(int)CarNewsType.xinwen,
					(int)CarNewsType.pingce,
					(int)CarNewsType.treepingce
					};
                    newsDs = new CarNewsBll().GetSerialNewsAllData(serialId, carTypeIdList, pageSize, pageIndex, ref newsCount);
                }
                else
                    newsDs = new CarNewsBll().GetSerialNewsAllData(serialId, _CarNewsType, pageSize, pageIndex, ref newsCount);
            }

            _CurrentNewsCount = newsCount;

            if (newsDs != null && newsDs.Tables.Count > 0 && newsDs.Tables[0].Rows.Count > 0)
            {
                htmlCode.AppendLine(" <div class=\"tuwenlistbox\"><ul>");

                DataRowCollection drsNews = newsDs.Tables[0].Rows;
                int pageCount = newsCount / pageSize + (newsCount % pageSize == 0 ? 0 : 1);

                int newsCounter = 0;
                //排重需要
                List<int> list = new List<int>();
                foreach (DataRow row in drsNews)
                {
                    newsCounter++;

                    string newsTitle = CommonFunction.NewsTitleDecode(row["title"].ToString());
                    int newsId = ConvertHelper.GetInteger(row["cmsnewsid"]);
                    string newsUrl = Convert.ToString(row["filepath"]);
                    //string firstPicUrl = string.Empty;

                    string imageUrl = string.Empty;
                    if (newsType != "hangqing")
                    {
                        string picUrl = ConvertHelper.GetString(row["Picture"]);
                        imageUrl = (!string.IsNullOrEmpty(picUrl) && picUrl.IndexOf("/not") < 0) ? picUrl : ConvertHelper.GetString(row["FirstPicUrl"]);
                    }
                    //if (newsType != "hangqing")
                    //	firstPicUrl = ConvertHelper.GetString(row["FirstPicUrl"]);
                    if (newsType == "pingce")
                        newsUrl = "/" + serialSpell + "/pingce/p" + newsId + "/";
                    DateTime publishTime = Convert.ToDateTime(row["publishtime"]);
                    string newsContent = Convert.ToString(row["content"]);
                    string from = Convert.ToString(row["sourceName"]);
                    string author = Convert.ToString(row["author"]);
                    int commentNum = ConvertHelper.GetInteger(row["CommentNum"]);
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
                    htmlCode.AppendFormat("<p class=\"info\"><span>来源：<em>{1}</em></span>{2}<span>{0}</span><span><em data-vnewsid=\"{4}\" class=\"liulan\">0</em><em data-cnewsid=\"{4}\" class=\"huifu comment_0_{4}\" ></em></span></p>",
                        publishTime.ToString("yyyy-MM-dd HH:mm"),
                        from,
                        !string.IsNullOrEmpty(author) ? "<span>作者：<em>" + author + "</em></span>" : "",
                        commentNum,
                        newsId);
                    htmlCode.AppendFormat("<p class=\"text\">{0} <a href=\"{1}\" target=\"_blank\">详细&gt;&gt;</a></p>", newsContent, newsUrl);
                    htmlCode.Append("</div>");
                    htmlCode.Append("</li>");
                }

                //生成页号导航
                if (pageCount > 1)
                {
                    // modified by chengl Nov.3.2011
                    // 行情分页带当前城市
                    string baseUrl = "/" + serialSpell + "/" + newsType + "/";
                    if (newsType == "hangqing" && _CityAllSpell != "")
                    { baseUrl += _CityAllSpell + "/"; }

                    this.AspNetPager1.UrlRewritePattern = baseUrl + "{0}/";
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
        // 	private string RenderNewsCategory()
        // 	{
        // 		StringBuilder htmlCode = new StringBuilder();
        //         // 分类静态块
        //         htmlCode.AppendLine("<div class=\"line_box\">");
        //         htmlCode.AppendLine("<h3><span>文章分类</span></h3>");
        //         htmlCode.AppendLine("<ul class=\"half\">");
        //         htmlCode.AppendLine("<li><a href=\"/" + serialSpell + "/xinwen/\" >新闻</a></li>");
        //         htmlCode.AppendLine("<li><a href=\"/" + serialSpell + "/daogou/\" >导购</a></li>");
        //         htmlCode.AppendLine("</ul>");
        //         htmlCode.AppendLine("<ul class=\"half\">");
        //         htmlCode.AppendLine("<li><a href=\"/" + serialSpell + "/hangqing/\" >行情</a></li>");
        //         htmlCode.AppendLine("<li><a href=\"/" + serialSpell + "/yongche/\" >用车</a></li>");
        //         htmlCode.AppendLine("</ul>");
        //         htmlCode.AppendLine("<div class=\"clear\"></div>");
        //         htmlCode.AppendLine("</div>");
        // 		return htmlCode.ToString();
        // 	}

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
            List<CarSerialPhotoEntity> carSerialPhotoList = new Car_BrandBll().GetCarSerialPhotoListByCBID(cse.Cb_Id, false);

            carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

            if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
            {
                return "";
            }

            int forLastCount = 0;
            foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
            {
                if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Cs_Id)
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
                if (entity.SerialLevel == "概念车" || entity.SerialId == cse.Cs_Id)
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
                    cse.Cb_AllSpell, cse.Cb_Name);
                brandOtherSerial.Append("</div>");

                brandOtherSerial.Append("<ul class=\"text-list\">");

                brandOtherSerial.Append(contentBuilder.ToString());

                brandOtherSerial.Append("</ul>");
            }

            return brandOtherSerial.ToString();
        }
        //[Obsolete("新闻服务上线后，将由InitNextSeeNew方法代替。")]
        //private void InitNextSee()
        //{
        //    nextSeePingceHtml = String.Empty;
        //    nextSeeXinwenHtml = String.Empty;
        //    nextSeeDaogouHtml = String.Empty;
        //    string serialSpell = cse.Cs_AllSpell.Trim().ToLower();
        //    string serialShowName = cse.Cs_ShowName;
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
            string serialSpell = cse.Cs_AllSpell.Trim().ToLower();
            string serialShowName = cse.Cs_ShowName;

            CarNewsBll newsBll = new CarNewsBll();
            if (newsBll.IsSerialNews(serialId, 0, CarNewsType.pingce))
                nextSeePingceHtml = "<li><a href=\"/" + serialSpell + "/pingce/\" target=\"_self\">" + serialShowName + "车型详解</a></li>";

            //if (newsBll.IsSerialNews(serialId, 0, CarNewsType.pingce))
            //    nextSeeXinwenHtml = "<li><a href=\"/" + serialSpell + "/xinwen/\" target=\"_self\">" + serialShowName + "<span>新闻</span></a></li>";

            if (newsBll.IsSerialNews(serialId, 0, CarNewsType.xinwen))
                nextSeeDaogouHtml = "<li><a href=\"/" + serialSpell + "/daogou/\" target=\"_self\">" + serialShowName + "导购</a></li>";
        }
        /// <summary>
        /// 生成行情行情经销商块
        /// </summary>
        /// <returns></returns>
        private string RenderHangQingDealer()
        {
            int topDealerCount = 4;
            HangQingTree _HangQingTree = new HangQingTree();
            List<string> listTitleHTML = new List<string>();
            List<string> listCarHTML = new List<string>();
            List<XmlElement> listXE = new List<XmlElement>();
            if (_CityId > 0)
            {
                listXE = _HangQingTree.GetSerialHangQingDealer(serialId, _CityId, topDealerCount);
            }
            else if (SpecialCityID > 0)
            {
                listXE = _HangQingTree.GetSerialHangQingDealer(serialId, SpecialCityID, topDealerCount);
            }
            else
            {
                // 全国 和 省份 统一不显示 保持和树形行情统一
                // listXE = _HangQingTree.GetSerialHangQingDealer(serialId, 0, 5);
            }
            // listXE = _HangQingTree.GetSerialHangQingDealer(serialId, 0, 5);
            if (listXE != null && listXE.Count > 0)
            {
                listTitleHTML.Add("<h3><span>最新行情价</span></h3>");
                listTitleHTML.Add("<ul class=\"car_tab_ul right100\" id=\"car_tab_ul3\">");
                int loop = 0;
                foreach (XmlElement xe in listXE)
                {
                    if (loop >= topDealerCount)
                    { break; }
                    int vendorId = int.Parse(xe.SelectSingleNode("VendorId").InnerText);
                    string vendorName = xe.SelectSingleNode("VendorName").InnerText;
                    if (loop == 0)
                    {
                        listTitleHTML.Add("<li class=\"current\">" + vendorName + "店</li>");
                    }
                    else
                    {
                        listTitleHTML.Add("<li>" + vendorName + "店</li>");
                    }
                    DateTime dateOfPriceCollection = Convert.ToDateTime(xe.SelectSingleNode("DateOfPriceCollection").InnerText);
                    // DateTime dtStartTime = Convert.ToDateTime(xe.SelectSingleNode("ValidStartTime").InnerText);
                    // DateTime dtEndTime = Convert.ToDateTime(xe.SelectSingleNode("ValidEndTime").InnerText);
                    int newsID = int.Parse(xe.SelectSingleNode("NewsID").InnerText);
                    string title = xe.SelectSingleNode("Title").InnerText;
                    DateTime pubTime = Convert.ToDateTime(xe.SelectSingleNode("pubTime").InnerText);
                    string url = xe.SelectSingleNode("Url").InnerText;
                    XmlNode xnCarList = xe.SelectSingleNode("items");

                    if (loop == 0)
                    {
                        listCarHTML.Add("<div id=\"data_table3_" + loop.ToString() + "\" class=\"data_table data_table_p\"> ");
                    }
                    else
                    {
                        listCarHTML.Add("<div id=\"data_table3_" + loop.ToString() + "\" class=\"data_table data_table_p\" style=\"display:none\"> ");
                    }
                    listCarHTML.Add("<ul class=\"data_table_head\">");
                    listCarHTML.Add("<li class=\"head\"> <span class=\"p01\">优惠车型</span> <span class=\"p02\">厂商指导价</span> <span class=\"p03\">行情价</span> <span class=\"p04\">优惠政策</span> </li>");
                    if (xnCarList != null && xnCarList.HasChildNodes)
                    {
                        foreach (XmlElement xeCar in xnCarList)
                        {
                            int carId = int.Parse(xeCar.SelectSingleNode("CarId").InnerText);
                            string carName = xeCar.SelectSingleNode("CarName").InnerText;
                            decimal referPrice = Convert.ToDecimal(xeCar.SelectSingleNode("ReferPrice").InnerText);
                            decimal vendorPrice = Convert.ToDecimal(xeCar.SelectSingleNode("VendorPrice").InnerText);
                            string gift = xeCar.SelectSingleNode("Gift").InnerText;
                            listCarHTML.Add("<li><a title=\"" + carName + "\" href=\"" + url + "\" target=\"_blank\"><strong class=\"p01\">" + carName + "</strong> ");
                            listCarHTML.Add("<span class=\"p02\">" + referPrice.ToString("0.00 ") + "万</span> ");
                            listCarHTML.Add("<span class=\"p03\"><em>" + vendorPrice.ToString("0.00 ") + "万</em></span> ");
                            listCarHTML.Add("<span class=\"p04\" title=\"" + gift + "\">" + BitAuto.Utils.StringHelper.SubString(gift, 34, true) + "</span></a></li>");
                        }
                    }
                    listCarHTML.Add("</ul>");
                    listCarHTML.Add("<div class=\"clear\"></div>");
                    listCarHTML.Add("<div class=\"data_table_b data_table_bt\">查看详情：<span><a href=\"" + url + "\" target=\"_blank\">" + title + "</a></span></div>");
                    // listCarHTML.Add("<div class=\"data_table_b\">以上优惠信息来源于行情文章，市场价格变化迅速，请以经销商实际报价为准。活动有效期：" + dtStartTime.ToString("yyyy-MM-dd") + "--" + dtEndTime.ToString("yyyy-MM-dd") + "</div>    ");
                    listCarHTML.Add("<div class=\"data_table_b\">以上优惠信息来源于行情文章，市场价格变化迅速，请以经销商实际报价为准。数据采集时间：" + dateOfPriceCollection.ToString("yyyy-MM-dd") + "</div>    ");
                    listCarHTML.Add("</div>");

                    loop++;
                }
                listTitleHTML.Add("</ul>");
            }
            if (listTitleHTML.Count > 0 && listCarHTML.Count > 0)
            {
                string cityName = Request.QueryString["city"];
                listTitleHTML.Add(string.Format("<div class=\"more\"><a href=\"http://dealer.bitauto.com/{0}{1}/\" target=\"_blank\">更多经销商&gt;&gt;</a></div>", string.IsNullOrEmpty(cityName) ? "" : (cityName + "/"), serialSpell));
                return "<div  style=\"width:100%; overflow:hidden;\"><div class=\"line_box\">" + string.Concat(listTitleHTML.ToArray()) + string.Concat(listCarHTML.ToArray()) + "</div></div>";
            }
            else
            {
                return "";
            }
        }
    }
}