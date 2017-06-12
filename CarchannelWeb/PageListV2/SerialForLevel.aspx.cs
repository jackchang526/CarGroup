using System;
using System.Xml;
using System.Collections.Generic;
using System.Web.Caching;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL;
using BitAuto.CarUtils.Define;

namespace BitAuto.CarChannel.CarchannelWeb.PageListV2
{
    public partial class SerialForLevel : PageBase
    {
        Dictionary<string, string> levelLabelDic;   //级别字典
        Dictionary<int, string> priceDic;           //报价字典
        bool isFirstPrice;                          //是否是页面上第一个报价
        bool isLastLevel;                           //是否是页面上最后一个级别，为了生成最后的<div class="hideline"></div>
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(10);
            lrContent.Text = GetRenderedHtml();
        }

        private string GetRenderedHtml()
        {
            string cacheKey = "serial-level-list";
            object objHtml = null;
            CacheManager.GetCachedData(cacheKey, out objHtml);
            if (objHtml == null)
            {
                InitData();
                objHtml = RenderByLevel();
                CacheDependency cacheDepend = new CacheDependency(WebConfig.AutoDataFile);
                CacheManager.InsertCache(cacheKey, objHtml, cacheDepend, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
            }

            return (string)objHtml;
        }

        /// <summary>
        /// 按子品牌级别生成Html代码
        /// </summary>
        /// <returns></returns>
        private string RenderByLevel()
        {
            string[] levels = new string[] { "微型车", "小型车", "紧凑型", "中型车", "中大型", "豪华车", "MPV", "SUV", "跑车", "面包车", "皮卡", "其它" };
            //Dictionary<string, string> levelNameDic = Car_LevelBll.LevelNameDic;

            //获取数据xml
            XmlDocument mbDoc = AutoStorageService.GetAutoXml();

            //遍历所有子品牌节点
            XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");

            //将所有子品牌按级别，价格，分类列表
            Dictionary<string, Dictionary<int, List<XmlElement>>> allSerialNodes = new Dictionary<string, Dictionary<int, List<XmlElement>>>();
            foreach (XmlElement serialNode in serialNodeList)
            {
                string level = serialNode.GetAttribute("CsLevel").ToUpper();
                //不在字典中则不显示
                if (!levelLabelDic.ContainsKey(level))
                    continue;
                string[] prices = serialNode.GetAttribute("MultiPriceRange").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //按级别加入列表
                if (!allSerialNodes.ContainsKey(level))
                    allSerialNodes[level] = new Dictionary<int, List<XmlElement>>();

                Dictionary<int, List<XmlElement>> priceElements = allSerialNodes[level];
                //按报价加入
                foreach (string priceId in prices)
                {
                    int pId = Convert.ToInt32(priceId);
                    if (!priceDic.ContainsKey(pId))
                        continue;
                    if (!priceElements.ContainsKey(pId))
                        priceElements[pId] = new List<XmlElement>();

                    priceElements[pId].Add(serialNode);
                }
            }

            //生成Html
            List<string> htmlCode = new List<string>();
            htmlCode.Add("<dl class=\"bybrand_list byprice_list\">");
            bool isFirstLevel = true;
            int levelCounter = 0;
            foreach (string level in levels)
            {
                if (!allSerialNodes.ContainsKey(level))
                    continue;

                //级别个数计数，用以确定最后一个级别
                levelCounter++;
                if (levelCounter == allSerialNodes.Count)
                    isLastLevel = true;
                htmlCode.Add("<dt><label>" +
                                 CarLevelDefine.GetLevelDiscName(level) +
                                 "</label><div><span id=\"" + levelLabelDic[level] + "\">&nbsp;</span></div></dt>");
                if (isFirstLevel)
                {
                    isFirstLevel = false;
                }
                RenderByPrice(htmlCode, allSerialNodes[level]);
            }

            htmlCode.Add("</dl>");
            return String.Concat(htmlCode.ToArray());
        }

        /// <summary>
        /// 按报价生成Html
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <param name="pricesNodes"></param>
        private void RenderByPrice(List<string> htmlCode, Dictionary<int, List<XmlElement>> pricesNodes)
        {
            int priceCounter = 0;

            for (int i = 1; i <= 8; i++)
            {
                if (!pricesNodes.ContainsKey(i))
                    continue;

                priceCounter++;

                htmlCode.Add("<dd class=\"b\"><div class=\"brandname\"  ><span>" + priceDic[i] + "</span></div></dd>");
                isFirstPrice = false;

                //第个子品牌
                htmlCode.Add("<dd class=\"have\">");
                new Car_SerialBll().RenderSerialsByPVNoLevel(htmlCode, pricesNodes[i], true);

                //最后一个级别和最后一个报价才有这个
                if (isLastLevel && priceCounter == pricesNodes.Count)
                {
                    htmlCode.Add("<div class=\"hideline\"></div>");
                }
                htmlCode.Add("</dd>");
                htmlCode.Add("<dd class=\"line\"></dd>");
            }
        }


        /// <summary>
        /// 初始化原始数据
        /// </summary>
        private void InitData()
        {
            levelLabelDic = new Dictionary<string, string>();
            levelLabelDic["微型车"] = "a";
            levelLabelDic["小型车"] = "b";
            levelLabelDic["紧凑型"] = "c";
            levelLabelDic["中型车"] = "d";
            levelLabelDic["中大型"] = "e";
            levelLabelDic["豪华车"] = "f";
            levelLabelDic["MPV"] = "g";
            levelLabelDic["SUV"] = "h";
            levelLabelDic["跑车"] = "i";
            levelLabelDic["其它"] = "j";
            levelLabelDic["面包车"] = "l";
            levelLabelDic["皮卡"] = "m";
            priceDic = new Dictionary<int, string>();
            priceDic[1] = "5万以下";
            priceDic[2] = "5万-8万";
            priceDic[3] = "8万-12万";
            priceDic[4] = "12万-18万";
            priceDic[5] = "18万-25万";
            priceDic[6] = "25万-40万";
            priceDic[7] = "40万-80万";
            priceDic[8] = "80万以上";
            isFirstPrice = true;
            isLastLevel = false;
        }
    }
}