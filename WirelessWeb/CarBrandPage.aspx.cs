using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using System.Text;

namespace WirelessWeb
{
	/// <summary>
	/// 品牌页
	/// </summary>
	public partial class CarBrandPage : WirelessPageBase
	{
		protected int _brandId;
		protected string _brandName;
		protected string _brandReplaceName;
		protected int _masterId;
		protected string _masterName;
		protected string _masterSpell;
		protected string _brandSpell;
		protected string _brandListHtml;


		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
			Response.ContentType = "text/html";
			GetParameter();
			RenderIntroduce();
			RenderBrandSerials();
		}
		private void GetParameter()
		{
			_brandId = ConvertHelper.GetInteger(Request.QueryString["cbid"]);
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
			StringBuilder htmlCode = new StringBuilder();
			List<CarSerialPhotoEntity> serialList = new Car_BrandBll().GetCarSerialPhotoListByCBID(_brandId, true);

            htmlCode.Append("<div class=\"tt-small\">");
            htmlCode.AppendFormat("<span>{0}</span>",_brandReplaceName);
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

                if (sellState == "在销")
                {
                    priceRange = base.GetSerialPriceRangeByID(serialId);
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