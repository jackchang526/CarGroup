using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace H5Web.handlers
{
    /// <summary>
    /// GetBrandList 的摘要说明
    /// </summary>
    public class GetBrandList : PageBase, IHttpHandler
    {
        private readonly SerialFourthStageBll _serialFourthStageBll = new SerialFourthStageBll();
        protected MasterBrandEntity MasterBrandEntity = null;
        protected int MasterId = 0;
        protected string WtStr = "";
        public void ProcessRequest(HttpContext context)
        {
            base.SetPageCache(60);
            
            context.Response.ContentType = "text/plain";

            MasterId = ConvertHelper.GetInteger(context.Request.QueryString["mID"]);
            WtStr = (!string.IsNullOrEmpty(context.Request["WT_mc_id"])) ? "?WT.mc_id=" + context.Request["WT_mc_id"] : "";
            var list = new List<int>();

            list.Add(MasterId);

            var cacheKey = string.Format("H5V20151023BrandList_{0}", string.Join("_", list));

            var obj = CacheManager.GetCachedData(cacheKey);

            if (obj != null )
            {
                context.Response.Write(obj);
            }
            else
            {
                MasterBrandEntity = (MasterBrandEntity)DataManager.GetDataEntity(EntityType.MasterBrand, MasterId);
                var res = GetBrandToSerialInfo();
                CacheManager.InsertCache(cacheKey, res, WebConfig.CachedDuration);
                context.Response.Write(res);
            }

            context.Response.End();
        }

        private string GetBrandToSerialInfo()
        {
            List<int> listAllCsID = _serialFourthStageBll.GetAllSerialInH5();
            DataSet brandDs = new Car_BrandBll().GetCarSerialPhotoListByBSID(MasterId, true);
            if (brandDs != null && brandDs.Tables.Count > 0)
            {
                StringBuilder htmlCode = new StringBuilder();
                //htmlCode.Append("<div class='tt-list absolute'>");
                htmlCode.Append("<!-- 厂商 start -->");
                htmlCode.Append("<div class='choose-car-name-close bybrand_list'>");
                htmlCode.AppendFormat("    <div class='brand-logo-none-border m_{0}_b'></div>", MasterBrandEntity.Id);
                htmlCode.AppendFormat("    <span class='brand-name'>{0}</span>",MasterBrandEntity.Name);
                htmlCode.Append("    <!-- <a href='#' class='choose-car-btn-close'>关闭</a> -->");
                htmlCode.Append("</div>");
                htmlCode.Append("<div class='clear'></div>");
                htmlCode.Append("<!-- 厂商 end -->");

                foreach (DataTable brandTable in brandDs.Tables)
                {
                    if (brandTable.Rows.Count == 0)
                    {
                        continue;
                    }

                    htmlCode.Append(GetSerialHtml(brandTable, listAllCsID));
                }
                //htmlCode.Append("</div>");
                return htmlCode.ToString();
            }
            return "";
        }

        private string GetSerialHtml(DataTable brandTable, List<int> listAllCsID)
        {
            StringBuilder htmlCode = new StringBuilder();

            StringBuilder serialList = new StringBuilder();
            StringBuilder htmlAllwaitSaleHtml = new StringBuilder();
            StringBuilder htmlAllnoPriceHtml = new StringBuilder();
            StringBuilder htmlAllstopSaleHtml = new StringBuilder();
            serialList.Append("<ul>");

            bool isShowBrand = false;//是否有要显示的子品牌
            foreach (DataRow row in brandTable.Rows)
            {
                int serialId = ConvertHelper.GetInteger(row["cs_id"]);
                if (!listAllCsID.Contains(serialId))
                {
                    continue;
                }

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

                isShowBrand = true;

                string csShowName = ConvertHelper.GetString(row["cs_ShowName"]);
                if (serialId == 1568)
                { csShowName = "索纳塔八"; }
                string csSpell = ConvertHelper.GetString(row["csspell"]).Trim().ToLower();
                //string priceRange = sellState;
                string serialUrl = "/" + csSpell + "/";

                SerialEntity serialBrandEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
                string priceRange = serialBrandEntity.ReferPrice;
                if (sellState == "在销" || sellState == "待销")
                {
                    //priceRange = base.GetSerialPriceRangeByID(serialId);

                    htmlAllnoPriceHtml.AppendFormat("<li>");
                    htmlAllnoPriceHtml.AppendFormat("    <a class='imgbox-2' href='/{0}/{1}'>", csSpell, WtStr);
                    htmlAllnoPriceHtml.AppendFormat("        <div class='img-box'>");
                    htmlAllnoPriceHtml.AppendFormat("            <img src='{0}' />", imgUrl);
                    htmlAllnoPriceHtml.AppendFormat("        </div>");
                    htmlAllnoPriceHtml.AppendFormat("        <div class='c-box'>");
                    htmlAllnoPriceHtml.AppendFormat("            <h4>{0}</h4>",csShowName);
                    htmlAllnoPriceHtml.AppendFormat("            <p>指导价：{0}</p>", priceRange);
                    htmlAllnoPriceHtml.AppendFormat("        </div>");
                    htmlAllnoPriceHtml.AppendFormat("    </a>");
                    htmlAllnoPriceHtml.AppendFormat("</li>");

                    //htmlAllnoPriceHtml.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p>厂商指导价：{3}</p></a></li>", csSpell, imgUrl, csShowName, priceRange);
                    
                }
                else
                {
                    // 停销
                    priceRange = "暂无";

                    htmlAllnoPriceHtml.AppendFormat("<li>");
                    htmlAllnoPriceHtml.AppendFormat("    <a class='imgbox-2' href='/{0}/{1}'>", csSpell, WtStr);
                    htmlAllnoPriceHtml.AppendFormat("        <div class='img-box'>");
                    htmlAllnoPriceHtml.AppendFormat("            <img src='{0}' />", imgUrl);
                    htmlAllnoPriceHtml.AppendFormat("        </div>");
                    htmlAllnoPriceHtml.AppendFormat("        <div class='c-box'>");
                    htmlAllnoPriceHtml.AppendFormat("            <h4>{0}</h4>", csShowName);
                    htmlAllnoPriceHtml.AppendFormat("            <p>报价：{0}</p>", priceRange);
                    htmlAllnoPriceHtml.AppendFormat("        </div>");
                    htmlAllnoPriceHtml.AppendFormat("    </a>");
                    htmlAllnoPriceHtml.AppendFormat("</li>");

                    //htmlAllstopSaleHtml.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p>厂商指导价：{3}</p></a></li>", csSpell, imgUrl, csShowName, priceRange);
                }
            }
            serialList.Append(htmlAllnoPriceHtml.ToString());
            serialList.Append(htmlAllwaitSaleHtml.ToString());
            serialList.Append(htmlAllstopSaleHtml.ToString());

            serialList.Append("</ul>");
            if (isShowBrand)
            {
                htmlCode.AppendFormat("<div class=\"tt-small\"><span>{0}</span></div>", brandTable.TableName);
                htmlCode.Append("<div class=\"pic-txt-h pic-txt-9060\">");
                htmlCode.Append(serialList);
                htmlCode.Append("</div>");
            }
            return htmlCode.ToString();
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}