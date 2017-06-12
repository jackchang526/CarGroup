using System;
using System.Collections.Generic;
using System.Web.Caching;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.PageListV2
{
    public partial class SerialForFunctionList : PageBase
    {
        Dictionary<string, string> functionLabelDic;    //级别字典
        Dictionary<int, string> priceDic;               //报价字典
        bool isFirstPrice;                              //是否是页面上第一个报价
        bool isLastFunction;                                //是否是页面上最后一个功能，为了生成最后的<div class="hideline"></div>

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(10);
            lrContent.Text = GetRenderedHtml();
        }

        private string GetRenderedHtml()
        {
            string cacheKey = "serial-function-list";
            object objHtml = null;
            CacheManager.GetCachedData(cacheKey, out objHtml);
            if (objHtml == null)
            {
                InitData();
                objHtml = RenderByFunction();
                CacheDependency cacheDepend = new CacheDependency(WebConfig.AutoDataFile);
                CacheManager.InsertCache(cacheKey, objHtml, cacheDepend, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
            }

            return (string)objHtml;
        }

        /// <summary>
        /// 按子品牌级别生成Html代码
        /// </summary>
        /// <returns></returns>
        private string RenderByFunction()
        {
            string[] funcs = new string[] { "越野", "时尚", "家用", "代步", "休闲", "运动", "商务", "cross", "多功能" };

            //获取数据xml
            XmlDocument mbDoc = AutoStorageService.GetAutoXml();

            //遍历所有子品牌节点
            XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");

            //将所有子品牌按用途，价格，分类列表
            Dictionary<string, Dictionary<int, List<XmlElement>>> allSerialNodes = new Dictionary<string, Dictionary<int, List<XmlElement>>>();
            foreach (XmlElement serialNode in serialNodeList)
            {
                EnumCollection.SerialPurposeForInterface funComplex = (EnumCollection.SerialPurposeForInterface)Convert.ToInt32(serialNode.GetAttribute("CsPurpose"));

                string[] funcHas = funComplex.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] prices = serialNode.GetAttribute("MultiPriceRange").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);



                foreach (string func in funcHas)
                {
                    //不在字典中则不显示
                    if (!functionLabelDic.ContainsKey(func.Trim()))
                        continue;
                    //按用途加入列表
                    if (!allSerialNodes.ContainsKey(func.Trim()))
                        allSerialNodes[func.Trim()] = new Dictionary<int, List<XmlElement>>();

                    Dictionary<int, List<XmlElement>> priceElements = allSerialNodes[func.Trim()];

                    //按报价加入
                    foreach (string priceId in prices)
                    {
                        int pId = Convert.ToInt32(priceId.Trim());
                        if (pId == 0)
                            pId = 9;
                        if (!priceDic.ContainsKey(pId))
                            continue;
                        if (!priceElements.ContainsKey(pId))
                            priceElements[pId] = new List<XmlElement>();

                        priceElements[pId].Add(serialNode);
                    }
                }
            }

            //生成Html
            List<string> htmlCode = new List<string>();
            htmlCode.Add("<dl class=\"bybrand_list byprice_list\">");
            bool isFirstFunc = true;
            int functionCounter = 0;
            foreach (string func in funcs)
            {
                if (!allSerialNodes.ContainsKey(func.Trim()))
                    continue;

                //级别个数计数，用以确定最后一个用途
                functionCounter++;
                if (allSerialNodes[func.Trim()].Count == 0)
                    continue;

                if (functionCounter == allSerialNodes.Count)
                    isLastFunction = true;
                htmlCode.Add("<dt><label>" + func.Trim() + "</label><div><span id=\"" + functionLabelDic[func.Trim()] + "\">&nbsp;</span></div></dt>");
                if (isFirstFunc)
                {
                    isFirstFunc = false;
                }
                RenderByPrice(htmlCode, allSerialNodes[func.Trim()], functionLabelDic[func.Trim()]);
            }
            htmlCode.Add("</dl>");
            return String.Concat(htmlCode.ToArray());
        }

        /// <summary>
        /// 按报价生成Html
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <param name="?"></param>
        private void RenderByPrice(List<string> htmlCode, Dictionary<int, List<XmlElement>> pricesNodes, string labelName)
        {
            int priceCounter = 0;

            for (int i = 1; i <= 9; i++)
            {
                if (!pricesNodes.ContainsKey(i))
                    continue;

                priceCounter++;

                //htmlCode.Add("<dt><label>" + priceDic[i] + "</label><div><span id=\"" + labelName + "\">&nbsp;</span></div></dt>");
                htmlCode.Add("<dd class=\"b\"><div class=\"brandname\"  ><span>" + priceDic[i] + "</span></div></dd>");
                if (isFirstPrice)
                {
                    isFirstPrice = false;
                }
                htmlCode.Add("<dd class=\"have\">");
                new Car_SerialBll().RenderSerialsByPv(htmlCode, pricesNodes[i], true);

                //最后一个级别和最后一个报价才有这个
                if (isLastFunction && priceCounter == pricesNodes.Count)
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
            functionLabelDic = new Dictionary<string, string>();
            functionLabelDic["越野"] = "yy";
            functionLabelDic["时尚"] = "ss";
            functionLabelDic["家用"] = "jy";
            functionLabelDic["代步"] = "db";
            functionLabelDic["休闲"] = "xx";
            functionLabelDic["运动"] = "yd";
            functionLabelDic["商务"] = "sw";
            functionLabelDic["cross"] = "cr";
            functionLabelDic["多功能"] = "dgn";
            priceDic = new Dictionary<int, string>();
            priceDic[1] = "5万以下";
            priceDic[2] = "5万-8万";
            priceDic[3] = "8万-12万";
            priceDic[4] = "12万-18万";
            priceDic[5] = "18万-25万";
            priceDic[6] = "25万-40万";
            priceDic[7] = "40万-80万";
            priceDic[8] = "80万以上";
            priceDic[9] = "未上市";
            isFirstPrice = true;
            isLastFunction = false;
        }
    }
}