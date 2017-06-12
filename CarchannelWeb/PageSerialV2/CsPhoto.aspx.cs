using System;
using System.Net;
using System.Xml;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;


namespace BitAuto.CarChannel.CarchannelWeb.PageSerialV2
{
    public partial class CsPhoto : PageBase
    {
        //protected string strCS_Name = string.Empty;
        protected string strCs_ShowName = string.Empty;
        protected string strCs_SeoName = string.Empty;
        //protected string strCs_MasterName = string.Empty;
        //protected int nPhotoCount = 0;
        protected string CsHeadHTML = string.Empty;
        protected int serialId = 0;
        protected string CsHotCompareCars = string.Empty;
        //protected string CsSerialToSerial = string.Empty;
        //private const string DIC_KEY_CS_NAME            = "CS_NAME";
		//private const string DIC_KEY_CS_PHOTOCOUNT = "CS_PHOTOCOUNT";
		//private const string DIC_KEY_CS_PHOTOCATEGORY = "CS_PHOTOCATEGORY";//车型图片(上)
		//private const string DIC_KEY_CS_PHOTOLIST = "CS_PHOTOLIST";//车型图片(列表)
		//private const string DIC_KEY_CS_PHOTOTYPELIST = "CS_PHOTOTYPELIST";
        private EnumCollection.SerialInfoCard sic;
        protected Car_SerialEntity cse;
		protected SerialEntity serialEntity = null;
        //protected string UserBlock;
        protected string baaUrl;
        //protected string ColorPicList;

        //protected string PhotoProvideCateHTML = string.Empty;

        protected string nextSeePingceHtml;
        protected string nextSeeXinwenHtml;
        protected string nextSeeDaogouHtml;
        protected string SerialPhotoHtml = string.Empty;
        //protected string serialToSeeHtml = string.Empty;
        protected string serialToSeeJson = string.Empty;
        // 子品牌信息栏
       // protected string SerialInfoBarHtml;

        private Car_SerialBll serialBll;
        public CsPhoto()
        {
            serialBll = new Car_SerialBll();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
            if (!IsPostBack)
            {
                GetParams();
                base.MakeSerialTopADCode(serialId);
                if (serialId > 0)
                {
					serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
                    #region 子品牌名片及基本数据
                    string catchKeyCard = "CsSummaryCsCard_CsID" + serialId.ToString();
                    object serialInfoCardByCsID = null;
                    CacheManager.GetCachedData(catchKeyCard, out serialInfoCardByCsID);
                    sic = new EnumCollection.SerialInfoCard();
                    if (serialInfoCardByCsID == null)
                    {
                        sic = base.GetSerialInfoCardByCsID(serialId);
                        CacheManager.InsertCache(catchKeyCard, sic, 60);
                    }
                    else
                    {
                        sic = (EnumCollection.SerialInfoCard)serialInfoCardByCsID;
                    }

                    if (sic.CsID == 0)
                    {
                        Response.Redirect("/car/404error.aspx?info=无子品牌");
                    }

                    // add by chengl May.17.2012 高岩要求开放概念车
                    //if (sic.CsLevel == "概念车")
                    //{
                    //    Response.Redirect("/car/404error.aspx?info=概念车无图片页");
                    //}

                    string catchKeyEntity = "CsSummaryEntity_CsID" + serialId.ToString();
                    object serialInfoEntityByCsID = null;
                    CacheManager.GetCachedData(catchKeyEntity, out serialInfoEntityByCsID);
                    cse = new Car_SerialEntity();

                    if (serialInfoEntityByCsID == null)
                    {
                        cse = serialBll.Get_Car_SerialByCsID(serialId);
                        CacheManager.InsertCache(catchKeyEntity, cse, 60);
                    }
                    else
                    {
                        cse = (Car_SerialEntity)serialInfoEntityByCsID;
                    }
                    #endregion
                    baaUrl = serialBll.GetForumUrlBySerialId(serialId);
                    SerialPhotoHtml = Car_SerialBll.GetSerialPhotoHtml(serialId);
                    //strCS_Name = cse.Cs_Name;
                    strCs_SeoName = cse.Cs_SeoName;
                    strCs_ShowName = cse.Cs_ShowName;
					if (serialId == 1568)
						strCs_ShowName = "索纳塔八";
                    //strCs_MasterName = cse.Cb_Name.Trim();// .cse.MasterName;

                    //bool isSuccess = false;
                    //CsHeadHTML = this.GetRequestString(string.Format(WebConfig.HeadForSerial, CSID.ToString(), "CsPhoto"), 10, out isSuccess);
                    CsHeadHTML = base.GetCommonNavigation("CsPhoto", serialId);
                    //SerialInfoBarHtml = base.GetCommonNavigation("SerialInfoBar", serialId);
                    //图库内容   modified by sk 2013.03.21
                    GetSerialPhotoHtml(serialId);
                    //还看过的
                    MakeSerialToSerialHtml();
                    //ucSerialToSee.SerialId = serialId;
                    //ucSerialToSee.SerialName = strCs_ShowName;

                    CsHotCompareCars = this.GetSerialHotCompareCars();

                    // modified by chengl May.5.2011
                    // GetUserBlockByCarSerialId();
                    //接下来要看的
                    //InitNextSee();
                    InitNextSeeNew();

                    //UCarHtml = serialBll.GetUCarHtml(serialId);
                }
            }
        }
        // 
        private void GetSerialPhotoHtml(int serialId)
        {
            SerialPhotoHtml = Car_SerialBll.GetSerialPhotoHtmlNew(serialId);
            if (string.IsNullOrEmpty(SerialPhotoHtml))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class=\"note-box note-empty type-1\" style=\"margin-top:30px;\">");
                sb.Append("    <div class=\"ico\"></div>");
                sb.Append("    <div class=\"info\">");
                sb.Append("        <h3>很抱歉，该车型暂无图片！</h3>");
                sb.Append("        <p class=\"tip\">我们正在努力更新，请查看其他...</p>");
                sb.Append("        <div class=\"more\">");
                sb.Append("            <span>您还可以：</span>");
                sb.Append("            <ul class=\"list list-gapline sm\">");
                sb.Append("                <li>");
                sb.AppendFormat("                    <a href=\"/{0}/\" target=\"_blank\">返回{1}频道&gt;&gt;</a>", cse.Cs_AllSpell, strCs_ShowName);
                sb.Append("                </li>");
                sb.Append("            </ul>");
                sb.Append("        </div>");
                sb.Append("    </div>");
                sb.Append("</div>");
                SerialPhotoHtml = sb.ToString();
            }
        }

        // 取子品牌图片对比
        private string GetSerialHotCompareCars()
        {
            StringBuilder sb = new StringBuilder();
            List<EnumCollection.SerialHotCompareData> lshcd = base.GetSerialHotCompareByCsID(serialId, 6);

            if (lshcd.Count > 0)
            {
                sb.Append("<div class=\"compare layout-1\">");
                sb.Append("    <div class=\"section-header header3\">");
                sb.Append("        <div class=\"box\">");
                sb.Append("            <h2>车型图片对比</h2>");
                sb.Append("        </div>");
                sb.Append("    </div>");
                sb.Append("    <div class=\"img-list clearfix\">");
                foreach (EnumCollection.SerialHotCompareData shcd in lshcd)
                {
					sb.Append("        <div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-14093\">");
                    sb.Append("            <div class=\"img\">");
                    sb.AppendFormat("                <a href=\"/tupianduibi/?csids={1},{2}\" target=\"_blank\"><img src=\"{0}\"></a>"
                        , shcd.ComapreCsImg.Replace("_2.","_3.")
                        , shcd.CurrentCsID
                        , shcd.CompareCsID);
                    sb.Append("            </div>");
                    sb.Append("            <ul class=\"p-list\">");
                    sb.AppendFormat("                <li class=\"name no-wrap\"><a href=\"/tupianduibi/?csids={1},{2}\" title=\"{0}\" target=\"_blank\"><span>VS</span> {0}</a></li>"
                        , shcd.CompareCsShowName
                        , shcd.CurrentCsID
                        , shcd.CompareCsID);
                    sb.Append("            </ul>");
                    sb.Append("        </div>");
                }
                sb.Append("    </div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }

        private void GetParams()
        {
            // modified by chengl Feb.22.2010
            if (this.Request.QueryString["CSID"] != null && this.Request.QueryString["CSID"].ToString() != "")
            {
                string tempCsID = this.Request.QueryString["CSID"].ToString();
                if (int.TryParse(tempCsID, out serialId))
                { }
            }
        }
        /// <summary>
        /// 得到品牌下的其他子品牌
        /// </summary>
        /// <returns></returns>
        protected string GetBrandOtherSerial()
        {
            return new Car_BrandBll().GetBrandOtherSerial(cse.Cb_Id, cse.Cs_Id);
        }

        /// <summary>
        /// 子品牌还关注
        /// </summary>
        /// <returns></returns>
        private void MakeSerialToSerialHtml()
        {
            serialToSeeJson = serialBll.GetSerialSeeToSeeJson(serialId, 6, 3);
        }


        private void InitNextSeeNew()
        {
            nextSeePingceHtml = String.Empty;
            nextSeeXinwenHtml = String.Empty;
            nextSeeDaogouHtml = String.Empty;
            string serialSpell = sic.CsAllSpell.Trim().ToLower();
            CarNewsBll newsBll = new CarNewsBll();
            if (newsBll.IsSerialNews(serialId, 0, CarNewsType.pingce))
                nextSeePingceHtml = "<li><div class=\"txt\"><a href=\"/" + serialSpell + "/pingce/\" target=\"_self\">" + strCs_ShowName + "车型详解</a></div></li>";

            if (newsBll.IsSerialNews(serialId, 0, CarNewsType.daogou))
                nextSeeDaogouHtml = "<li><div class=\"txt\"><a href=\"/" + serialSpell + "/daogou/\" target=\"_self\">" + strCs_ShowName + "导购</a></div></li>";
        }
    }

}