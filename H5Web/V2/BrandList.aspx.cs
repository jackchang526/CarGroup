using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using System.Data;
using System.Text;

namespace H5Web.V2
{
    public partial class BrandList : PageBase
    {
        private SerialFourthStageBll serialFourthStageBll = new SerialFourthStageBll();
        
        protected MasterBrandEntity masterBrandEntity = null;
        protected int MasterId = 0;
        //protected string MasterName = string.Empty;
        protected string HtmlBrandToSerial = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetParam();
            if (MasterId < 1)
            {
                Response.Redirect("http://car.h5.yiche.com/");
            }
            InitData();
            if (masterBrandEntity == null)
            {
                Response.Redirect("http://car.h5.yiche.com/");
            }
            GetBrandToSerialInfo();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            masterBrandEntity = (MasterBrandEntity)DataManager.GetDataEntity(EntityType.MasterBrand, MasterId);
        }

        private void GetParam()
        {
            MasterId = ConvertHelper.GetInteger(Request.QueryString["mID"]);
        }

        private void GetBrandToSerialInfo()
        {
            List<int> listAllCsID = serialFourthStageBll.GetAllSerialInH5();
            DataSet brandDs = new Car_BrandBll().GetCarSerialPhotoListByBSID(MasterId, true);
            if (brandDs != null && brandDs.Tables.Count > 0)
            {
                StringBuilder htmlCode = new StringBuilder();
                foreach (DataTable brandTable in brandDs.Tables)
                {
                    if (brandTable.Rows.Count == 0)
                    {
                        continue;
                    }

                    htmlCode.Append(GetSerialHtml(brandTable, listAllCsID));
                }
                HtmlBrandToSerial = htmlCode.ToString();
            }
            
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
                    htmlAllnoPriceHtml.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p>厂商指导价：{3}</p></a></li>", csSpell, imgUrl, csShowName, priceRange);
                //    if (priceRange.Trim().Length == 0)
                //    {
                //        priceRange = "暂无报价";
                //        htmlAllnoPriceHtml.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p>{3}</p></a></li>", csSpell, imgUrl, csShowName, priceRange);
                //    }
                //    else
                //    {
                //        serialList.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p>报价：{3}</p></a></li>", csSpell, imgUrl, csShowName, priceRange);
                //    }
                //}
                //else if (sellState == "待销")
                //{
                //    priceRange = "未上市";
                //    htmlAllwaitSaleHtml.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p>{3}</p></a></li>", csSpell, imgUrl, csShowName, priceRange);
                }
                else
                {
                    // 停销
                    priceRange = "暂无";
                    htmlAllstopSaleHtml.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p>厂商指导价：{3}</p></a></li>", csSpell, imgUrl, csShowName, priceRange);
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
    }
}