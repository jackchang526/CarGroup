using System;
using System.Collections.Generic;
using System.Web.Caching;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;


namespace BitAuto.CarChannel.CarchannelWeb.PageListV2
{
    public partial class SerialForPriceList : PageBase
    {
        Dictionary<int, string> priceIdDic;     //报价ID字典
        Dictionary<int, string> priceTextDic;   //报价文本字典
        bool isLastPrice;                       //是否是页面上最后一个报价，为了生成最后的<div class="hideline"></div>
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(10);
            lrContent.Text = GetRenderedHtml();
        }

        private string GetRenderedHtml()
        {
            string cacheKey = "serial-price-list";
            object objHtml = null;
            CacheManager.GetCachedData(cacheKey, out objHtml);
            if (objHtml == null)
            {
                InitData();
                objHtml = RenderByPrice();
                CacheDependency cacheDepend = new CacheDependency(WebConfig.AutoDataFile);
                CacheManager.InsertCache(cacheKey, objHtml, cacheDepend, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
            }

            return (string)objHtml;
        }

        /// <summary>
        /// 按子品牌报价生成Html代码
        /// </summary>
        /// <returns></returns>
        private string RenderByPrice()
        {
            //获取数据xml
            XmlDocument mbDoc = AutoStorageService.GetAutoXml();

            //遍历所有子品牌节点
            XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");

            //将所有子品牌按价格,箱式，分类列表
            Dictionary<int, List<XmlElement>> allSerialNodes = new Dictionary<int, List<XmlElement>>();
            foreach (XmlElement serialNode in serialNodeList)
            {
                string[] prices = serialNode.GetAttribute("MultiPriceRange").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                //按报价加入
                foreach (string priceId in prices)
                {
                    int pId = Convert.ToInt32(priceId);
                    //没有报价按未上市处理
                    if (pId == 0)
                        pId = 9;
                    if (!priceIdDic.ContainsKey(pId))
                        continue;

                    if (!allSerialNodes.ContainsKey(pId))
                        allSerialNodes[pId] = new List<XmlElement>();

                    allSerialNodes[pId].Add(serialNode);
                }
            }
            //生成Html
            List<string> htmlCode = new List<string>();
            htmlCode.Add("<dl class=\"bybrand_list byprice_list\">");
            bool isFirstPrice = true;
            int priceCounter = 0;
            for (int i = 1; i <= 9; i++)
            {
                if (!allSerialNodes.ContainsKey(i))
                    continue;

                //报价区间个数计数，用以确定最后一个区间
                priceCounter++;
                if (priceCounter == allSerialNodes.Count)
                    isLastPrice = true;
                if (isFirstPrice)
                {
                    isFirstPrice = false;
                }
                htmlCode.Add("<dt><label>" + priceTextDic[i] + "</label><div><span id=\"" + priceIdDic[i] + "\">&nbsp;</span></div></dt>");
                new Car_SerialBll().RenderForPriceNew(htmlCode, allSerialNodes[i], i, isFirstPrice, isLastPrice, priceTextDic[i], true, priceIdDic[i]);
            }
            htmlCode.Add("</dl>");
            return String.Concat(htmlCode.ToArray());
        }

        /// <summary>
        /// 初始化原始数据
        /// </summary>
        private void InitData()
        {
            priceIdDic = new Dictionary<int, string>();
            priceIdDic[1] = "a";
            priceIdDic[2] = "b";
            priceIdDic[3] = "c";
            priceIdDic[4] = "d";
            priceIdDic[5] = "e";
            priceIdDic[6] = "f";
            priceIdDic[7] = "g";
            priceIdDic[8] = "h";
            priceIdDic[9] = "i";
            priceTextDic = new Dictionary<int, string>();
            priceTextDic[1] = "5万以下";
            priceTextDic[2] = "5万-8万";
            priceTextDic[3] = "8万-12万";
            priceTextDic[4] = "12万-18万";
            priceTextDic[5] = "18万-25万";
            priceTextDic[6] = "25万-40万";
            priceTextDic[7] = "40万-80万";
            priceTextDic[8] = "80万以上";
            priceTextDic[9] = "未上市";
            isLastPrice = false;
        }
    }
}