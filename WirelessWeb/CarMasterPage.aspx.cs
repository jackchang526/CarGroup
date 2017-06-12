﻿using System;
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
using BitAuto.CarChannel.Common.Enum;

namespace WirelessWeb
{
    /// <summary>
    /// 主品牌页
    /// </summary>
    public partial class CarMasterPage : WirelessPageBase
    {
        protected int _masterId = 0;			//主品牌ID
        protected string _masterName = string.Empty;	//主品牌名称
        protected string _masterUrlSpell = string.Empty; // 主品牌全拼
        protected int _serialCount;
        protected int _serialSaleCount;
        protected string _masterInfo;
        protected string _masterStory;
        protected string _brandListHtml;
        protected string _newsListHtml;
        protected int _PageIndex = 1;
        protected int _PageTotal = 0;
        protected int _NewsCount = 0;
        protected int _PageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
            Response.ContentType = "text/html";
            GetParameter();
            RenderIntroduction();
            RenderBrandList();
            InitNewsListNew();
        }

        private void RenderIntroduction()
        {
            //StringBuilder htmlCode = new StringBuilder();

            DataRow drInfo = new Car_BrandBll().GetCarMasterBrandInfoByBSID(_masterId);
            CommonFunction commonFunction = new CommonFunction();
            if (drInfo != null)
            {
                _masterName = drInfo["bs_name"].ToString().Trim();
                //_strMasterSeoName = drInfo["bs_seoname"].ToString().Trim();
                _masterUrlSpell = drInfo["urlspell"].ToString().ToLower();
                _masterInfo = drInfo["bs_introduction"].ToString().Trim();
                _masterStory = drInfo["bs_logoinfo"].ToString().Trim();
            }
        }
        private void GetParameter()
        {
            _masterId = ConvertHelper.GetInteger(Request.QueryString["bsid"]);
        }
        /// <summary>
        /// 生成主品牌下各品牌的子品牌列表
        /// </summary>
        private void RenderBrandList()
        {
            StringBuilder htmlCode = new StringBuilder();

            DataSet brandDs = new Car_BrandBll().GetCarSerialPhotoListByBSID(_masterId, true);
            if (brandDs != null && brandDs.Tables.Count > 0)
            {
                foreach (DataTable brandTable in brandDs.Tables)
                {
                    if (brandTable.Rows.Count == 0)
                        continue;
                    string brandSpell = ConvertHelper.GetString(brandTable.Rows[0]["cbspell"]).Trim().ToLower();
                    string brandTag = null;
                    if (brandDs.Tables.Count <= 1 && brandTable.TableName == _masterName)
                    {
                        brandTag = string.Format("<span>{0}</span>", brandTable.TableName);
                    }
                    else
                    {
                        brandTag = string.Format("<span><a href=\"/{0}/\">{1}</a></span>", brandSpell, brandTable.TableName);
                    }
                    htmlCode.AppendFormat("<div class=\"tt-small\">{0}</div>", brandTag);
                    //htmlCode.AppendFormat("<section class=\"m-line-box\"><div class=\"m-tabs-box\"><ul class=\"m-tabs\"><li>{0}</li></ul></div>", brandTag);
                    htmlCode.Append(GetSerialHtml(brandTable));
                    //htmlCode.Append("</section>");
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
            serialList.Append("<div class=\"car-list2 car-list2-w\"><ul>");
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

                var cs_IsWhiteCover = ConvertHelper.GetBoolean(row["cs_IsWhiteCover"]);
                if (cs_IsWhiteCover)
                {
                    imgUrl = imgUrl.Replace("_1.jpg", "_6.jpg");
                }
                else
                {
                    imgUrl = imgUrl.Replace("_1.jpg", "_4.jpg");
                }
                int serialId = ConvertHelper.GetInteger(row["cs_id"]);
                string csShowName = ConvertHelper.GetString(row["cs_ShowName"]);
                if (serialId == 1568)
                { csShowName = "索纳塔八"; }
                string csSpell = ConvertHelper.GetString(row["csspell"]).Trim().ToLower();
                string priceRange = sellState;
                string serialUrl = "/" + csSpell + "/";
                const string liStr = "<li><a href=\"/{0}/\"><span><img src=\"{1}\" alt=\"{2}\"/></span><p>{2}</p><b>{3}</b></a></li>";
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
                        serialList.AppendFormat(liStr, csSpell, imgUrl, csShowName, priceRange);
                    }
                    _serialSaleCount++;
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
                _serialCount++;
            }
            serialList.Append(htmlAllnoPriceHtml.ToString());
            serialList.Append(htmlAllwaitSaleHtml.ToString());
            serialList.Append(htmlAllstopSaleHtml.ToString());

            serialList.Append("</ul></div>");
            return serialList.ToString();
        }

        /// <summary>
		/// 初始化新闻列表
		/// </summary>
		private void InitNewsListNew()
        {
            List<int> carTypeIdList = new List<int>()
                    {
                    (int)CarNewsType.daogou,
                    (int)CarNewsType.yongche,
                    (int)CarNewsType.xinwen
                    };
            DataSet ds = new CarNewsBll().GetMasterBrandNews(_masterId,
                        carTypeIdList, _PageSize, _PageIndex, ref _NewsCount);
            _PageTotal = _NewsCount / _PageSize + (_NewsCount % _PageSize == 0 ? 0 : 1);
            if (_PageTotal < 1) return;
            if (_PageIndex > _PageTotal) _PageIndex = _PageTotal;
            GetContentNew(ds, _NewsCount);
        }

        /// <summary>
		/// 得到新闻内容
		/// </summary>
		/// <param name="xmlDoc"></param>
		/// <returns></returns>
		private void GetContentNew(DataSet ds, int rowCount)
        {
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return;
            var contentString = new StringBuilder();
            contentString.Append("<ul id=\"cardNews\">");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string picUrl = ConvertHelper.GetString(row["Picture"]);
                string imageUrl = (!string.IsNullOrEmpty(picUrl) && picUrl.IndexOf("/not") < 0) ? picUrl : ConvertHelper.GetString(row["FirstPicUrl"]);
                DateTime publishTime = Convert.ToDateTime(row["publishtime"]);
                string title = CommonFunction.NewsTitleDecode(row["title"].ToString());
                //modified by sk 22个文字 2017-03-06
                title = StringHelper.GetRealLength(title) > 44 ? StringHelper.SubString(title, 44, false) : title;

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    contentString.Append("<li>");
                    contentString.AppendFormat("<a href=\"{0}\">", row["filepath"].ToString());
                    contentString.AppendFormat("<div class=\"img-box\"><span><img src=\"{0}\"></span></div>", imageUrl);
                    contentString.Append("<div class=\"con-box\">");
                    contentString.AppendFormat("<h4>{0}</h4>", title);
                    contentString.AppendFormat("<em><span>{0}</span><span>{1}</span><i class=\"ico-comment\">{2}</i></em>",
                        publishTime.ToString("yyyy-MM-dd"), row["EditorName"].ToString(), ConvertHelper.GetInteger(row["CommentNum"]));
                    contentString.Append("</div></a></li>");
                }
                else
                {
                    contentString.Append("<li class=\"news-noimg\">");
                    contentString.AppendFormat("<a href=\"{0}\">", row["filepath"].ToString());
                    contentString.AppendFormat("<div class=\"con-box\"><h4>{0}</h4><em><span>{1}</span><span>{2}</span><i class=\"ico-comment\">{3}</i></em></div>",
                        title, publishTime.ToString("yyyy-MM-dd"),
                        row["EditorName"].ToString(), ConvertHelper.GetInteger(row["CommentNum"]));
                    contentString.Append("</div></a></li>");
                }
            }
            contentString.Append("</ul>");
            _newsListHtml = contentString.ToString();
        }
    }
}