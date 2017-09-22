using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;

namespace MWeb.Controllers
{
    public class BrandController : Controller
    {
        protected int _brandId;
        protected string _brandName;
        protected string _brandReplaceName;
        protected int _masterId;
        protected string _masterName;
        protected string _masterSpell;
        protected string _brandSpell;
        protected string _brandListHtml;

        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int id)
        {
            _brandId = id;
            RenderIntroduce();
            RenderBrandSerials();
            ViewData["_brandName"] = _brandName;
            ViewData["_brandSpell"] = _brandSpell;
            ViewData["_brandListHtml"] = _brandListHtml;
            ViewData["_brandReplaceName"] = _brandReplaceName;
			ViewData["_brandId"] = _brandId;
			ViewData["_masterId"] = _masterId;
			ViewData["_masterSpell"] = _masterSpell;
            return View();
        }
        
        /// <summary>
        /// 生成简介与车标故事
        /// </summary>
        private void RenderIntroduce()
        {
            BrandEntity brandEntity = (BrandEntity)DataManager.GetDataEntity(EntityType.Brand, _brandId);
            if (brandEntity != null && brandEntity.Id > 0)
            {
                _brandName = brandEntity.ShowName;
                _brandReplaceName = _brandName.Replace("·", "&bull;");
                //strBrandSeoName = brandEntity.SeoName;
                _masterId = brandEntity.MasterBrandId;
                _masterName = brandEntity.MasterBrand.Name;
                _masterSpell = brandEntity.MasterBrand.AllSpell;
                //string country = brandEntity.ProducerCountry;
                _brandSpell = brandEntity.AllSpell;
            }
        }
        /// <summary>
        /// 生成品牌的子品牌列表,分在销与停销
        /// </summary>
        private void RenderBrandSerials()
        {
            PageBase pageBase = new PageBase();
            StringBuilder htmlCode = new StringBuilder();
            List<CarSerialPhotoEntity> serialList = new Car_BrandBll().GetCarSerialPhotoListByCBID(_brandId, true);

            htmlCode.Append("<div class=\"tt-small\">");
            htmlCode.AppendFormat("<span>{0}</span>", _brandReplaceName);
            htmlCode.Append("</div>");
            htmlCode.AppendFormat("<div class=\"car-list2 car-list2-w\"><ul>");
            htmlCode.AppendFormat("");
            const string liStr = "<li><a href=\"/{0}/\"><span><img src=\"{1}\" /></span><p>{2}</p><b>{3}</b></a></li>";
            StringBuilder htmlAllwaitSaleHtml = new StringBuilder();
            StringBuilder htmlAllnoPriceHtml = new StringBuilder();
            StringBuilder htmlAllstopSaleHtml = new StringBuilder();

            foreach (CarSerialPhotoEntity serialEntity in serialList)
            {
                #region 不显示的子品牌
                string csLevel = serialEntity.SerialLevel;
                // if (csLevel == "概念车" || csLevel == "皮卡")
                if (csLevel == "概念车")
                    continue;
                string csName = serialEntity.CS_Name;
                if (csName.IndexOf("停用") >= 0)
                { continue; }
                string sellState = serialEntity.SaleState;
                string imgUrl = serialEntity.CS_ImageUrl;
                // 无图片的
                if (sellState.Trim() == "停销" && imgUrl.IndexOf("150-100.gif") > 0)
                { continue; }
                #endregion

                imgUrl = imgUrl.Replace("_1.jpg", "_6.jpg");
                int serialId = serialEntity.SerialId;
                string csShowName = serialEntity.ShowName;
                if (serialId == 1568)
                { csShowName = "索纳塔八"; }
                string csSpell = serialEntity.CS_AllSpell;
                string priceRange = sellState;
                string serialUrl = "/" + csSpell + "/";

                if (sellState == "在销")                {
                    
                    priceRange = pageBase.GetSerialPriceRangeByID(serialId);
                    if (priceRange.Trim().Length == 0)
                    {
                        priceRange = "暂无报价";
                        htmlAllnoPriceHtml.AppendFormat(liStr, csSpell, imgUrl, csShowName, priceRange);
                    }
                    else
                    {
                        htmlCode.AppendFormat(liStr, csSpell, imgUrl, csShowName, priceRange);
                    }
                }
                else if (sellState == "待销")
                {
                    priceRange = "未上市";
                    htmlAllwaitSaleHtml.AppendFormat(liStr, csSpell, imgUrl, csShowName, priceRange);
                }
                else
                {
                    // 停销
                    priceRange = "停售";
                    htmlAllstopSaleHtml.AppendFormat(liStr, csSpell, imgUrl, csShowName, priceRange);
                }
            }
            htmlCode.Append(htmlAllnoPriceHtml.ToString());
            htmlCode.Append(htmlAllwaitSaleHtml.ToString());
            htmlCode.Append(htmlAllstopSaleHtml.ToString());
            htmlCode.AppendFormat("</ul></div>");

            _brandListHtml = htmlCode.ToString();
        }
        

    }
}
