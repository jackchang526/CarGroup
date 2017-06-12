using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using System.Data;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Model;

namespace WirelessWeb
{
    /// <summary>
    /// 选车品牌页面
    /// </summary>
    public partial class SelectBrand : WirelessPageBase
    {
        protected int _masterId = 0;			//主品牌ID
        protected string _masterName = string.Empty;	//主品牌名称
        protected string _masterSpell;
        protected string _brandListHtml;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
            Response.ContentType = "text/html";
            GetParameter();
            RenderMasterBrand();
            RenderBrandList();
        }
        /// <summary>
        /// 获取主品牌ID
        /// </summary>
        private void GetParameter()
        {
            _masterId = ConvertHelper.GetInteger(Request.QueryString["bsid"]);
        }
        /// <summary>
        /// 获取主品牌数据
        /// </summary>
        private void RenderMasterBrand()
        {

            DataRow drInfo = new Car_BrandBll().GetCarMasterBrandInfoByBSID(_masterId);
            if (drInfo != null)
            {
                _masterName = drInfo["bs_name"].ToString().Trim();
                _masterSpell = drInfo["urlspell"].ToString().Trim(); 
            }
        }
        /// <summary>
        /// 生成主品牌下各品牌的子品牌列表
        /// </summary>
        private void RenderBrandList()
        {
            StringBuilder htmlCode = new StringBuilder();
            DataSet brandDs = new Car_BrandBll().GetCarSerialPhotoListByBSID(_masterId, false);
            if (brandDs != null && brandDs.Tables.Count > 0)
            {
                foreach (DataTable brandTable in brandDs.Tables)
                {
                    if (brandTable.Rows.Count == 0)
                    {
                        continue;
                    }
                    htmlCode.AppendFormat("<div class=\"tt-small\" id=\"{1}\"><span>{0}</span></div>", brandTable.TableName,brandTable.Rows[0]["cbspell"].ToString());                   
                    htmlCode.Append("<div class=\"pic-txt-h pic-txt-9060\">");
                    htmlCode.Append(GetSerialHtml(brandTable));
                    htmlCode.Append("</div>");
                }
                _brandListHtml = htmlCode.ToString();
            }
        }

        private string GetSerialHtml(DataTable brandTable)
        {
            StringBuilder serialList = new StringBuilder();
            StringBuilder htmlAllwaitSaleHtml = new StringBuilder();
            StringBuilder htmlAllnoPriceHtml = new StringBuilder();
            StringBuilder htmlAllstopSaleHtml = new StringBuilder();
            serialList.Append("<ul>");

            foreach (DataRow row in brandTable.Rows)
            {
                #region 不显示的子品牌
                string csLevel = ConvertHelper.GetString(row["cslevel"]);
                // if (csLevel == "概念车" || csLevel == "皮卡")
                if (csLevel == "概念车")
                    continue;
                string csName = ConvertHelper.GetString(row["cs_name"]);
                if (csName.IndexOf("停用") >= 0)
                { continue; }
                string sellState = ConvertHelper.GetString(row["CsSaleState"]);
                string imgUrl = ConvertHelper.GetString(row["csImageUrl"]).ToLower();
                // 无图片的
                if (sellState.Trim() == "停销" && imgUrl.IndexOf("150-100.gif") > 0)
                { continue; }
                #endregion

                int serialId = ConvertHelper.GetInteger(row["cs_id"]);
                string csShowName = ConvertHelper.GetString(row["cs_ShowName"]);
                if (serialId == 1568)
                { csShowName = "索纳塔八"; }
                string csSpell = ConvertHelper.GetString(row["csspell"]).Trim().ToLower();
                string priceRange = sellState;
                string serialUrl = "/" + csSpell + "/";

                if (sellState == "在销")
                {
                    priceRange = base.GetSerialPriceRangeByID(serialId);
                    if (priceRange.Trim().Length == 0)
                    {
                        priceRange = "暂无报价";
                        htmlAllnoPriceHtml.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p><strong>{3}</strong></p></a></li>", csSpell, imgUrl, csShowName, priceRange);
                    }
                    else
                    {
                        serialList.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p><strong>{3}</strong></p></a></li>", csSpell, imgUrl, csShowName, priceRange);
                    }
                }
                else if (sellState == "待销")
                {
                    priceRange = "未上市";
                    htmlAllwaitSaleHtml.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p><strong>{3}</strong></p></a></li>", csSpell, imgUrl, csShowName, priceRange);
                }
                else
                {
                    // 停销
                    priceRange = "停产";
                    htmlAllstopSaleHtml.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p><strong>{3}</strong></p></a></li>", csSpell, imgUrl, csShowName, priceRange);
                }
            }
            serialList.Append(htmlAllnoPriceHtml.ToString());
            serialList.Append(htmlAllwaitSaleHtml.ToString());
            serialList.Append(htmlAllstopSaleHtml.ToString());

            serialList.Append("</ul>");
            return serialList.ToString();
        }
    }
}
