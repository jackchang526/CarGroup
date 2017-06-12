using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
using System.Net;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarUtils.Define;

namespace BitAuto.CarChannel.BLL
{
    public class Car_LevelBll
    {
        //public static Dictionary<string, string> LevelNameDic;
        //public static Dictionary<EnumCollection.SerialLevelEnum, int> LevelClassIdDic;
        public static Dictionary<string, int[]> KindCateDic;
        static Car_LevelBll()
        {
            //LevelNameDic = new Dictionary<string, string>(12);
            //LevelNameDic["微型车"] = "微型车";
            //LevelNameDic["小型车"] = "小型车";
            //LevelNameDic["紧凑型"] = "紧凑型车";
            //LevelNameDic["中型车"] = "中型车";
            //LevelNameDic["中大型"] = "中大型车";
            //LevelNameDic["豪华车"] = "豪华车";
            //LevelNameDic["MPV"] = "MPV多用途车";
            //LevelNameDic["SUV"] = "SUV越野车";
            //LevelNameDic["跑车"] = "跑车";
            //LevelNameDic["其它"] = "其它";
            //LevelNameDic["面包车"] = "面包车";
            //LevelNameDic["皮卡"] = "皮卡";

            //LevelNameDic["小型SUV"] = "小型SUV";
            //LevelNameDic["紧凑型SUV"] = "紧凑型SUV";
            //LevelNameDic["中型SUV"] = "中型SUV";
            //LevelNameDic["中大型SUV"] = "中大型SUV";
            //LevelNameDic["全尺寸SUV"] = "全尺寸SUV";

            //LevelClassIdDic = new Dictionary<EnumCollection.SerialLevelEnum, int>(12);
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.微型车] = 321;
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.小型车] = 338;
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.紧凑型] = 339;
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.中型车] = 340;
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.中大型] = 341;
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.豪华车] = 342;
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.MPV] = 425;
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.SUV] = 424;
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.跑车] = 426;
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.其它] = 428;
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.面包车] = 482;
            //LevelClassIdDic[EnumCollection.SerialLevelEnum.皮卡] = 483;


            KindCateDic = new Dictionary<string, int[]>();
            KindCateDic["xinwen"] = new int[] { 150, 152, 34, 148, 146, 144, 147, 83, 151, 153, 198, 145, 149, 123, 127, 2, 13, 210, 98 };
            KindCateDic["hangqing"] = new int[] { 3 };
            KindCateDic["daogou"] = new int[] { 4, 179, 102, 115, 120, 29, 30 };
            KindCateDic["pingce"] = new int[] { 33, 31, 32 };
            KindCateDic["yongche"] = new int[] { 87, 88, 143, 142, 86, 85, 173, 56, 54, 53, 55, 201 };
        }


        /// <summary>
        /// 根据级别全名获取级别的全拼
        /// </summary>
        /// <param name="levelName"></param>
        /// <returns></returns>
        public static string GetLevelSpellByFullName(string levelName)
        {
            //if (levelName.Trim().Length == 0)
            //    return "";
            //switch (levelName)
            //{
            //    case "紧凑型车":
            //        levelName = "紧凑型";
            //        break;
            //    case "中大型车":
            //        levelName = "中大型";
            //        break;
            //}

            //EnumCollection.SerialLevelEnum level = (EnumCollection.SerialLevelEnum)Enum.Parse(typeof(EnumCollection.SerialLevelEnum), levelName);
            //return ((EnumCollection.SerialLevelSpellEnum)(int)level).ToString();
            return CarLevelDefine.GetLevelSpellByName(levelName);
        }

        /// <summary>
        /// 获取导航的Html
        /// </summary>
        /// <param name="level"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public string RenderNavBar(int level, string tagName)
        {
            //string levelName = ((EnumCollection.SerialLevelEnum)level).ToString();
            //string tempLevelName = levelName;
            //if (levelName == "中大型" || levelName == "紧凑型")
            //{
            //    tempLevelName = levelName + "车";
            //}
            //string levelFullName = levelName;
            //if (LevelNameDic.ContainsKey(levelName))
            //    levelFullName = Car_LevelBll.LevelNameDic[levelName];
            //string levelSpell = ((EnumCollection.SerialLevelSpellEnum)level).ToString();
            string levelName = CarLevelDefine.GetLevelNameById(level);
            string levelFullName = CarLevelDefine.GetLevelDiscName(levelName);
            string levelSpell = CarLevelDefine.GetLevelSpellById(level);

            int levelForVideo = ConvertLevelForVideoTag(level);
            string levelForumUrl = "";
            switch (levelSpell)
            {
                case "weixingche":
                    levelForumUrl = "http://baa.bitauto.com/foruminterrelated/brandforumlist_by_jibie.html#a";
                    break;
                case "xiaoxingche":
                    levelForumUrl = "http://baa.bitauto.com/foruminterrelated/brandforumlist_by_jibie.html#b";
                    break;
                case "jincouxingche":
                    levelForumUrl = "http://baa.bitauto.com/foruminterrelated/brandforumlist_by_jibie.html#c";
                    break;
                case "zhongxingche":
                    levelForumUrl = "http://baa.bitauto.com/foruminterrelated/brandforumlist_by_jibie.html#d";
                    break;
                case "zhongdaxingche":
                    levelForumUrl = "http://baa.bitauto.com/foruminterrelated/brandforumlist_by_jibie.html#e";
                    break;
                case "haohuaxingche":
                    levelForumUrl = "http://baa.bitauto.com/foruminterrelated/brandforumlist_by_jibie.html#f";
                    break;
                case "mpv":
                    levelForumUrl = "http://baa.bitauto.com/foruminterrelated/brandforumlist_by_jibie.html#g";
                    break;
                case "suv":
                    levelForumUrl = "http://baa.bitauto.com/foruminterrelated/brandforumlist_by_jibie.html#h";
                    break;
                case "paoche":
                    levelForumUrl = "http://baa.bitauto.com/foruminterrelated/brandforumlist_by_jibie.html";
                    break;
                case "mianbaoche":
                    levelForumUrl = "http://baa.bitauto.com/foruminterrelated/brandforumlist_by_jibie.html";
                    break;
                case "pika":
                    levelForumUrl = "http://baa.bitauto.com/foruminterrelated/brandforumlist_by_jibie.html";
                    break;
            }

            StringBuilder htmlCode = new StringBuilder();
            // modified by chengl Jun.13.2011
            //if (String.IsNullOrEmpty(tagName))
            //    htmlCode.Append("<li class=\"first\"><span class=\"c_" + level + "\"><h1>" + levelFullName + "</h1>");
            //else
            //    htmlCode.Append("<li class=\"first\"><span class=\"c_" + level + "\"><a href=\"/" + levelSpell + "/\" >" + levelFullName + "首页</a>");
            //htmlCode.AppendLine("</span></li>");
            if (String.IsNullOrEmpty(tagName))
            { htmlCode.AppendLine("<li class=\"on\"><a href=\"/" + levelSpell + "/\" >首页</a></li>"); }
            else
            { htmlCode.AppendLine("<li class=\"\"><a href=\"/" + levelSpell + "/\" >首页</a></li>"); }

            if (tagName == "xinwen")
                htmlCode.AppendLine("<li class=\"on\"><a>新闻</a></li>");
            else
                htmlCode.AppendLine("<li><a href=\"/" + levelSpell + "/xinwen/\" >新闻</a></li>");

            if (tagName == "hangqing")
                htmlCode.AppendLine("<li class=\"on\"><a>行情</a></li>");
            else
                htmlCode.AppendLine("<li><a href=\"/tree_hangqing/search/?l=" + level.ToString() + "\" target=\"_blank\">行情</a></li>");

            if (tagName == "daogou")
                htmlCode.AppendLine("<li class=\"on\"><a>导购</a></li>");
            else
                htmlCode.AppendLine("<li><a href=\"/tree_daogou/search/?l=" + level.ToString() + "\" target=\"_blank\">导购</a></li>");

            if (tagName == "pingce")
                htmlCode.AppendLine("<li class=\"on\"><a>评测</a></li>");
            else
                htmlCode.AppendLine("<li><a href=\"/tree_pingce/search/?l=" + level.ToString() + "\" target=\"_blank\">评测</a></li>");

            if (tagName == "yongche")
                htmlCode.AppendLine("<li class=\"on\"><a>养护</a></li>");
            else
                htmlCode.AppendLine("<li><a href=\"/tree_baoyang/brandsearch.aspx?l=" + level + "\" target=\"_blank\">养护</a></li>");



            if (levelName != "其它" && levelName != "皮卡")
            {
                if (tagName == "tupian")
                    htmlCode.AppendLine("<li class=\"on\"><a>图片</a></li>");
                else
                    htmlCode.AppendLine("<li><a href=\"http://photo.bitauto.com/xuanche/?l=" + level.ToString() + "\" target=\"_blank\">图片</a></li>");

                if (tagName == "shipin")
                    htmlCode.AppendLine("<li class=\"on\"><a>视频</a></li>");
                else
                    htmlCode.AppendLine("<li><a href=\"http://v.bitauto.com/car/?l=" + level.ToString() + "\" target=\"_blank\">视频</a></li>");

                if (tagName == "koubei")
                    htmlCode.AppendLine("<li class=\"on\"><a>口碑</a></li>");
                else
                    htmlCode.AppendLine("<li><a href=\"http://koubei.bitauto.com/tree/xuanche/?l=" + level.ToString() + "\" target=\"_blank\">口碑</a></li>");

                htmlCode.AppendLine("<li><a href=\"/tree_ucar/search/?l=" + level + "\" target=\"_blank\">二手车</a></li>");
            }

            if (tagName == "paihang")
                htmlCode.AppendLine("<li class=\"on\"><a>排行榜</a></li>");
            else
                htmlCode.AppendLine("<li><a href=\"/" + levelSpell + "/paihang/\">排行榜</a></li>");


            if (levelName != "其它")
                htmlCode.AppendLine("<li class=\"\"><a href=\"" + levelForumUrl + "\" target=\"_blank\">" + levelFullName + "论坛</a></li>");
            return htmlCode.ToString();
        }

        /// <summary>
        /// 获取一条新闻的类别名称
        /// </summary>
        /// <param name="catePath"></param>
        /// <returns></returns>
        public static string GetNewsKind(string catePath)
        {
            string[] cateIds = catePath.Split(',');
            string kindName = "";
            foreach (string tempCateId in cateIds)
            {
                int cateId = 0;
                bool success = Int32.TryParse(tempCateId, out cateId);
                if (!success)
                    continue;
                bool isKind = false;
                foreach (string kind in KindCateDic.Keys)
                {
                    foreach (int id in KindCateDic[kind])
                    {
                        if (id == cateId)
                        {
                            isKind = true;
                            kindName = kind;
                            break;
                        }
                    }

                    if (isKind)
                        break;
                }

                if (isKind)
                    break;
            }
            return kindName;
        }
        /// <summary>
        /// 获取级别介绍
        /// </summary>
        /// <param name="levelSpell"></param>
        /// <returns></returns>
        public string GetLevelIntroduce(string levelSpell)
        {
            string levelFile = levelSpell + ".html";
            levelFile = Path.Combine(WebConfig.WebRootPath, "html\\level\\" + levelFile);
            if (File.Exists(levelFile))
                return File.ReadAllText(levelFile);
            else
                return "";
        }

        /// <summary>
        /// 获取级别介绍完整版
        /// </summary>
        /// <param name="levelSpell"></param>
        /// <returns></returns>
        public string GetLevelIntroduceAll(string levelSpell)
        {
            string levelFile = levelSpell + "_all.html";
            levelFile = Path.Combine(WebConfig.WebRootPath, "html\\level\\" + levelFile);
            if (File.Exists(levelFile))
                return File.ReadAllText(levelFile);
            else
                return "";
        }

        /// <summary>
        /// 获取级别焦点专题新闻的Xml
        /// </summary>
        /// <param name="level">车型级别</param>
        /// <returns></returns>
        public XmlDocument GetLevelFocusXml(string level)
        {
            XmlDocument focusXml = null;
            if (level == "豪华车")
                level = "豪华型";
            string xmlFile = "Level_" + level + "_FocusNews.xml";
            xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\LevelNews\\FocusNews\\" + xmlFile);
            focusXml = CommonFunction.ReadXmlFromFile(xmlFile);
            return focusXml;
        }

        /// <summary>
        /// 获取级别十条新闻的Xml
        /// </summary>
        /// <param name="level">级别</param>
        /// <returns></returns>
        public XmlDocument GetLevelTopNewsXml(string level)
        {
            XmlDocument levelXml = null;
            if (level == "豪华车")
                level = "豪华型";
            string xmlFile = "Level_" + level + "_News.xml";
            xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\LevelNews\\TopNews\\" + xmlFile);
            levelXml = CommonFunction.ReadXmlFromFile(xmlFile);
            return levelXml;
        }

        /// <summary>
        /// 获取级别分类新闻的Xml
        /// </summary>
        /// <param name="level">级别</param>
        /// <returns></returns>
        public XmlDocument GetLevelCategoryNewsXml(string level)
        {
            XmlDocument levelXml = null;
            if (level == "豪华车")
                level = "豪华型";
            string xmlFile = "Level_" + level + "_CategoryNews.xml";
            xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\LevelNews\\CategoryNews\\" + xmlFile);
            levelXml = CommonFunction.ReadXmlFromFile(xmlFile);
            return levelXml;
        }

        /// <summary>
        /// 根据级别获取新闻
        /// </summary>
        /// <param name="levelName">级别名称</param>
        /// <param name="kind">新闻类型</param>
        /// <returns></returns>
        public DataSet GetNewsListByLevel(string levelName, string kind)
        {
            if (levelName == "豪华车")
                levelName = "豪华型";
            string cacheKey = kind + "_" + levelName;
            DataSet newsDs = (DataSet)CacheManager.GetCachedData(cacheKey);
            if (newsDs == null)
            {
                string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\LevelNews\\" + kind + "\\" + cacheKey + ".xml");
                if (File.Exists(xmlFile))
                {
                    newsDs = new DataSet();
                    newsDs.ReadXml(xmlFile);
                    CacheManager.InsertCache(cacheKey, newsDs, WebConfig.CachedDuration);
                }
            }

            return newsDs;
        }

        /// <summary>
        /// 根据级别ID获取油耗与养车费用 （文件不再更新）
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public DataSet GetLevelCarCost(int levelId, string dataType)
        {
            if (dataType != "fuel")
                dataType = "fee";

            //string levelName = ((EnumCollection.SerialLevelEnum)levelId).ToString();
            string levelName = CarLevelDefine.GetLevelNameById(levelId);
            if (levelName == "豪华车")
                levelName = "豪华型";
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\LevelNews\\CarCost\\Level_" + dataType + "_CarCost_" + levelName + ".xml");

            //油耗
            DataSet ds = new DataSet();
            if (File.Exists(xmlFile))
                ds.ReadXml(xmlFile);
            return ds;
        }

        /// <summary>
        /// 获取级别的视频（文件不再更新）
        /// </summary>
        /// <param name="levelName"></param>
        /// <returns></returns>
        public XmlNodeList GetLevelVideo(string levelName)
        {
            if (levelName == "豪华车")
                levelName = "豪华型";

            string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\LevelNews\\LevelVideo.xml");
            XmlDocument videoDoc = CommonFunction.ReadXmlFromFile(xmlFile);
            XmlNodeList videoList = videoDoc.SelectNodes("/root/Level[@name=\"" + levelName + "\"]/listNews");
            return videoList;
        }

        /// <summary>
        /// 获取级别的焦点专题
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public string GetLevelFocusNews(int level)
        {
            string cacheKey = "Level_FocusNews_" + level;
            string focusHtml = (string)CacheManager.GetCachedData(cacheKey);
            if (focusHtml == null)
            {
                string htmlUrl = "http://www.bitauto.com/";
                switch (level)
                {
                    case 1:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/test_Manual.shtml");
                        break;
                    case 2:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/xxcjdt_Manual.shtml");
                        break;
                    case 3:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/jcxcjdt_Manual.shtml");
                        break;
                    case 4:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/zdxcjdt_Manual.shtml");
                        break;
                    case 5:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/zxcjdt_Manual.shtml");
                        break;
                    case 6:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/hhcjdt_Manual.shtml");
                        break;
                    case 7:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/mpvjdt_Manual.shtml");
                        break;
                    case 8:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/suvjdt_Manual.shtml");
                        break;
                    case 9:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/pcjdt_Manual.shtml");
                        break;
                    case 11:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/mbcjdt_Manual.shtml");
                        break;
                    case 12:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/pkjdt_Manual.shtml");
                        break;
                    default:
                        htmlUrl = Path.Combine(htmlUrl, "include/09gq/carchannel/00001/qtjdt_Manual.shtml");
                        break;
                }
                try
                {
                    WebClient wc = new WebClient();
                    focusHtml = wc.DownloadString(htmlUrl);
                }
                catch { }

                if (!String.IsNullOrEmpty(focusHtml))
                    CacheManager.InsertCache(cacheKey, focusHtml, 30);
            }

            return focusHtml;
        }

        /// <summary>
        /// 转换视频的级别标签
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int ConvertLevelForVideoTag(int level)
        {
            int retLevel = 1;
            switch (level)
            {
                case 1:
                    retLevel = 1;
                    break;
                case 2:
                    retLevel = 2;
                    break;
                case 3:
                    retLevel = 3;
                    break;
                case 4:
                    retLevel = 5;
                    break;
                case 5:
                    retLevel = 4;
                    break;
                case 6:
                    retLevel = 6;
                    break;
                case 7:
                    retLevel = 8;
                    break;
                case 8:
                    retLevel = 7;
                    break;
                case 9:
                    retLevel = 9;
                    break;
            }
            return retLevel;
        }

        /// <summary>
        /// 根据城市ID与级别名称获取子品牌点击排行
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="levelName"></param>
        /// <returns></returns>
        public XmlNodeList GetSerialPVSortByLevelAndCity(int cityId, string levelName)
        {
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialCityLevelPV\\" + cityId + "_CityPV.xml");
            if (!File.Exists(xmlFile))
                return null;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFile);
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }

            XmlNodeList serialList = xmlDoc.SelectNodes("/CityLevelSort/Level[@Name=\"" + levelName + "\"]/Serial");

            return serialList;
        }
        /// <summary>
        /// 根据城市ID与级别名称获取子品牌点击排行
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="levelName"></param>
        /// <returns></returns>
        public XmlNodeList GetSerialPVSortByPriceAndCity(int cityId, string price)
        {
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialCityPricePV\\" + cityId + ".xml");
            if (!File.Exists(xmlFile))
                return null;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFile);
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }

            XmlNodeList serialList = xmlDoc.SelectNodes("/CityPriceSort/Price[@Name=\"" + price + "\"]/Serial");

            return serialList;
        }

        /// <summary>
        /// 获取级别的子品牌列表XML
        /// </summary>
        /// <param name="levelName"></param>
        /// <returns></returns>
        public XmlNodeList GetLevelSerialList(string levelName)
        {
            levelName = levelName.ToUpper();
            if (levelName.IndexOf("SUV") > -1)
                levelName = "SUV";
            if (levelName.IndexOf("MPV") > -1)
                levelName = "MPV";
            string cacheKey = "Level_Serial_Price_GroupList_" + levelName;

            XmlNodeList serialList = (XmlNodeList)CacheManager.GetCachedData(cacheKey);

            if (serialList == null)
            {
                string xmlUrl = "http://carser.bitauto.com/forpicmastertoserial/list/LevelNewForList.xml";
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(xmlUrl);
                    serialList = xmlDoc.SelectNodes("/Params/CsLevel[@LevelName=\"" + levelName + "\"]/LevelTwoGroup");
                    CacheManager.InsertCache(cacheKey, serialList, 30);
                }
                catch
                {

                }
            }

            return serialList;
        }

        /// <summary>
        /// 获取级别的视频排行
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public string GetLevelVideoRank(int levelId)
        {
            string rankHtml = "";
            if (levelId >= 1 && levelId <= 12)
            {
                string cacheKey = "Level_video_rank_" + levelId;
                rankHtml = (string)CacheManager.GetCachedData(cacheKey);
                if (rankHtml == null)
                {
                    string rankUrl = "http://www.bitauto.com/include/debris/ranks/Top10CarLevelNews_" + levelId + ".shtml";
                    WebClient wc = new WebClient();
                    try
                    {
                        rankHtml = wc.DownloadString(rankUrl);
                    }
                    catch
                    {
                        rankHtml = "";
                    }
                    if (rankHtml == null)
                        rankHtml = "";
                    CacheManager.InsertCache(cacheKey, rankHtml, 60);
                }
            }
            return rankHtml;
        }

    }
}
