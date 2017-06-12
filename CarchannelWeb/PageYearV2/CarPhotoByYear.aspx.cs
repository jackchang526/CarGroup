using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Net;
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

namespace BitAuto.CarChannel.CarchannelWeb.PageYearV2
{
    public partial class CarPhotoByYear : PageBase
    {
        protected string strCS_Name = string.Empty;
        protected string strCs_ShowName = string.Empty;
        protected string strCs_SeoName = string.Empty;
        protected string strCs_MasterName = string.Empty;
        //protected int nPhotoCount = 0;
        protected string CsHeadHTML = string.Empty;
        protected int CSID = 0;
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

        protected string JsTagForYear = string.Empty;
        protected string PhotoProvideCateHTML = string.Empty;
        protected string SerialYearPhotoHtml = string.Empty;

        protected string nextSeePingceHtml;
        //protected string nextSeeXinwenHtml;
        protected string nextSeeDaogouHtml;
        //protected string UCarHtml;
        protected string _serialSpell = "";
        protected string serialToSeeHtml = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(10);
            if (!IsPostBack)
            {
                if (CSID > 0)
                {
                    #region 子品牌名片及基本数据
                    InitData();
                    if (sic.CsID == 0)
                    {
                        Response.Redirect("/car/404error.aspx?info=无子品牌");
                    }
					serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, CSID);
                    // 广告
                    base.MakeSerialTopADCode(sic.CsID);
                    #endregion

                    GetSerialYearPhotoHtml(CSID, CarYear);
                    CsHeadHTML = base.GetCommonNavigation("CsPhotoForYear", CSID).Replace("{0}", CarYear.ToString());
                    JsTagForYear = "if(document.getElementById('carYearList_" + CarYear.ToString() + "')){document.getElementById('carYearList_" + CarYear.ToString() + "').className='current';}changeSerialYearTag(0," + CarYear.ToString() + ",'');";

                    ucSerialToSee.serialId = CSID;

                    CsHotCompareCars = this.GetSerialHotCompareCars();

                    InitNextSee();
                }
            }
        }

        private void InitData()
        {
            string catchKeyCard = "CsSummaryCsCard_CsID" + CSID.ToString();
            object serialInfoCardByCsID = null;
            CacheManager.GetCachedData(catchKeyCard, out serialInfoCardByCsID);
            sic = new EnumCollection.SerialInfoCard();
            if (serialInfoCardByCsID == null)
            {
                sic = base.GetSerialInfoCardByCsID(CSID);
                CacheManager.InsertCache(catchKeyCard, sic, 60);
            }
            else
            {
                sic = (EnumCollection.SerialInfoCard)serialInfoCardByCsID;
            }

            string catchKeyEntity = "CsSummaryEntity_CsID" + CSID.ToString();
            object serialInfoEntityByCsID = null;
            CacheManager.GetCachedData(catchKeyEntity, out serialInfoEntityByCsID);
            cse = new Car_SerialEntity();
            if (serialInfoEntityByCsID == null)
            {
                cse = (new Car_SerialBll()).Get_Car_SerialByCsID(CSID);
                CacheManager.InsertCache(catchKeyEntity, cse, 60);
            }
            else
            {
                cse = (Car_SerialEntity)serialInfoEntityByCsID;
            }

            baaUrl = new Car_SerialBll().GetForumUrlBySerialId(CSID);
            strCS_Name = cse.Cs_Name;
            strCs_SeoName = cse.Cs_SeoName;
            strCs_ShowName = cse.Cs_ShowName;
            if (CSID == 1568)
                strCs_ShowName = "索纳塔八";
            strCs_MasterName = cse.Cb_Name.Trim();
        }
        private void GetSerialYearPhotoHtml(int serialId, int year)
        {
            SerialYearPhotoHtml = Car_SerialBll.GetSerialYearPhotoHtmlNew(serialId, year);
            if (string.IsNullOrEmpty(SerialYearPhotoHtml))
            {
                //车型对应年款图片还没有，取子品牌图片
                SerialYearPhotoHtml = Car_SerialBll.GetSerialPhotoHtmlNew(serialId);
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
                sb.Append(SerialYearPhotoHtml);
                SerialYearPhotoHtml = sb.ToString();
            }
        }

        // 取子品牌图片对比
        private string GetSerialHotCompareCars()
        {
            StringBuilder sb = new StringBuilder();
            List<EnumCollection.SerialHotCompareData> lshcd = base.GetSerialHotCompareByCsID(CSID, 6);

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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            int.TryParse(this.Request.QueryString["CSID"], out CSID);
            int.TryParse(this.Request.QueryString["year"], out _carYear);
        }
        /// <summary>
        /// 得到品牌下的其他子品牌
        /// </summary>
        /// <returns></returns>
        protected string GetBrandOtherSerial()
        {
            return new Car_BrandBll().GetBrandOtherSerial(cse.Cb_Id, cse.Cs_Id);
        }

        #region by liurw
        
        private int _carYear;
        /// <summary>
        /// 当前年款
        /// </summary>
        public int CarYear
        {
            get { return _carYear; }
            set { _carYear = value; }
        }

       
        #endregion


        private void InitNextSee()
        {
            nextSeePingceHtml = String.Empty;
            //nextSeeXinwenHtml = String.Empty;
            nextSeeDaogouHtml = String.Empty;
            string serialSpell = sic.CsAllSpell.Trim().ToLower();
            _serialSpell = serialSpell;
            string serialShowName = sic.CsShowName;
            CarNewsBll newsBll = new CarNewsBll();
            if (newsBll.IsSerialNews(CSID, 0, CarNewsType.pingce))
                nextSeePingceHtml = "<li><div class=\"txt\"><a href=\"/" + serialSpell + "/pingce/\" target=\"_self\">" + serialShowName + "车型详解</a></div></li>";
            //if (newsBll.IsSerialNews(CSID, 0, CarNewsType.xinwen))
            //    nextSeeXinwenHtml = "<li><a href=\"/" + serialSpell + "/xinwen/\" target=\"_self\">" + serialShowName + "新闻</a></li>";
            if (newsBll.IsSerialNews(CSID, 0, CarNewsType.daogou))
                nextSeeDaogouHtml = "<li><div class=\"txt\"><a href=\"/" + serialSpell + "/daogou/\" target=\"_self\">" + serialShowName + "导购</a></div></li>";
        }
    }

}