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
    public partial class SerialForBrandList : PageBase
    {
        Dictionary<string, bool> hasChar = new Dictionary<string, bool>();

        protected string hotSerialHtml;
        protected bool _isNoHeader = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(10);
            if (Request.QueryString["noheader"] == "1")
                _isNoHeader = true;

            lrSerialForBrand.Text = GetRenderedHtml();

            //hotSerialHtml = new Car_SerialBll().GetHomepageHotSerial();
        }

        protected string GetRenderedHtml()
        {
            string cacheKey = "serial-brand-list";
            object objHtml = null;
            CacheManager.GetCachedData(cacheKey, out objHtml);
            if (objHtml == null)
            {
                objHtml = RenderBrandList();
                CacheDependency cacheDepend = new CacheDependency(WebConfig.AutoDataFile);
                CacheManager.InsertCache(cacheKey, objHtml, cacheDepend, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
            }

            return (string)objHtml;
        }

        private string RenderBrandList()
        {
            //获取数据xml
            XmlDocument mbDoc = AutoStorageService.GetAutoXml();

            //Html
            List<string> htmlCode = new List<string>();
            htmlCode.Add("<dl class=\"bybrand_list\">");

            //第一个字母处不加回到顶部
            bool isFirstChar = true;

            //遍历所有主品牌节点
            XmlNodeList mbNodeList = mbDoc.SelectNodes("/Params/MasterBrand");
            for (int i = 0; i < mbNodeList.Count; i++)
            {
                XmlElement mbNode = (XmlElement)mbNodeList[i];
                string masterSpell = mbNode.GetAttribute("AllSpell").ToLower();
                //首字母
                string firstChar = mbNode.GetAttribute("Spell").Substring(0, 1).ToUpper();

                //生成字母头
                if (!hasChar.ContainsKey(firstChar))
                {
                    htmlCode.Add("<dt><label>" + firstChar + "</label><div><span id=\"" + firstChar + "\">&nbsp;</span></div></dt>");
                    if (isFirstChar)
                    {
                        isFirstChar = false;
                    }
                    hasChar[firstChar] = true;
                }
                //生成主品牌图标
                string mbId = mbNode.GetAttribute("ID");
                string mbName = mbNode.GetAttribute("Name");
                htmlCode.Add("<dd class=\"b\">");
                htmlCode.Add("<a href=\"/" + masterSpell + "/\" target=\"_blank\"><div class=\"brand m_" + mbId + "_b\"></div></a>");
                htmlCode.Add("<div class=\"brandname\"><a href=\"/" + masterSpell + "/\" target=\"_blank\">" + mbName + "</a></div>");
                htmlCode.Add("</dd>");
                //生成品牌列表
                RenderBrands(htmlCode, mbNode);

                //一条线
                if (i < mbNodeList.Count - 1)
                    htmlCode.Add("<dd class=\"line\"></dd>");
            }
            htmlCode.Add("</dl>");

            //字母导航
            string charNavHtml = CommonFunction.RenderCharNavForDefaultPage(hasChar);
            return charNavHtml + String.Concat(htmlCode.ToArray());
        }

        /// <summary>
        /// 生成主品牌下各品牌的Html
        /// </summary>
        /// <param name="htmlCode">代码容器</param>
        /// <param name="mbNode">主品牌信息</param>
        private void RenderBrands(List<string> htmlCode, XmlElement mbNode)
        {
            htmlCode.Add("<dd class=\"have\">");
            //获取品牌信息
            List<XmlElement> brandList = new List<XmlElement>();
            foreach (XmlElement ele in mbNode.SelectNodes("Brand"))
            {
                brandList.Add(ele);
            }
            //添加排序条件
            brandList.Sort(NodeCompare.CompareBrandNodeSelfFirst);

            bool isFirstBrand = true;

            foreach (XmlElement brandNode in brandList)
            {
                //生成品牌Html
                string brandId = brandNode.GetAttribute("ID");
                string brandName = brandNode.GetAttribute("Name");
                string brandSpell = brandNode.GetAttribute("AllSpell");
                if (isFirstBrand)
                {
                    htmlCode.Add("<h2><a href=\"/" + brandSpell + "/\" target=\"_blank\">" + brandName + "</a></h2>");
                    isFirstBrand = false;
                }
                else
                    htmlCode.Add("<h2 class=\"border\"><a href=\"/" + brandSpell + "/\" target=\"_blank\">" + brandName + "</a></h2>");

                //加入列表
                XmlNodeList serialNodeList = brandNode.SelectNodes("Serial");
                List<XmlElement> serialList = new List<XmlElement>();
                foreach (XmlElement serialNode in serialNodeList)
                {
                    serialList.Add(serialNode);
                }

                //生成子品牌列表
                new Car_SerialBll().RenderSerialsBySpell(htmlCode, serialList, false);
            }

            htmlCode.Add("</dd>");
        }
    }
}