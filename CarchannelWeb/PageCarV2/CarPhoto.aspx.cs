using System;
using System.Xml;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannel.CarchannelWeb.PageCarV2
{
    public partial class CarPhoto : PageBase
    {
        #region Param
        protected string CarPhotoHeadHTML = string.Empty;
        protected int CarID = 0;
        protected EnumCollection.CarInfoForCarSummary cfcs = new EnumCollection.CarInfoForCarSummary();
        protected Car_BasicEntity cbe = new Car_BasicEntity();
        protected CarEntity carEntity;
        //protected string CarClassString = string.Empty;
        //protected string Car12Pic = string.Empty;
        protected string CarClassAndPic = string.Empty;
        protected int CarPhotoCount = 0;
        protected string Serial12Pic = string.Empty;
        protected string CsClass = string.Empty;
        protected string CarList = string.Empty;
        //protected string SerialToSerial = string.Empty;
        protected int CsPhotoCount = 0;
        protected string CarYear = string.Empty;
        protected string UserBlock;
        protected string PhotoProvideCateHTML;
        protected string SerialHotCompare = string.Empty;

        protected string UCarHtml = string.Empty;
        protected string PhotoProvideColorRGBHTML = string.Empty;
        protected string CarPhotoProvideColorRGBHTML = string.Empty;
        protected string CarPhotoHtml = string.Empty;
        protected string serialToSeeHtml = string.Empty;
        protected string hotCarsHtml = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
            if (!this.IsPostBack)
            {
                // 取参数
                GetParams();
                // 取车型数据
                GetCarData();
                GetCarPhotoHtml(CarID);

                ucSerialToSee.serialId = cbe.Cs_Id;
                //ucSerialToSee.SerialName = cbe.Cs_ShowName;
                GetHotCar();
                // 车型图片导航头
                string subDir = Convert.ToString(CarID / 1000);
                CarPhotoHeadHTML = base.GetCommonNavigation("CarPhoto", CarID);
            }
        }

        #region private Method
        private void GetCarPhotoHtml(int carId)
        {
            CarPhotoHtml = Car_SerialBll.GetCarPhotoHtmlNew(carId);
            if (string.IsNullOrEmpty(CarPhotoHtml))
            {
                //车型没有图片，取对应年款图片
                CarPhotoHtml = Car_SerialBll.GetSerialYearPhotoHtmlNew(cbe.Cs_Id, cbe.Car_YearType);
                if (string.IsNullOrEmpty(CarPhotoHtml))
                {
                    //车型对应年款图片还没有，取子品牌图片
                    CarPhotoHtml = Car_SerialBll.GetSerialPhotoHtmlNew(cbe.Cs_Id);
                }
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
                sb.AppendFormat("                    <a href=\"/{0}/\" target=\"_blank\">返回{1}频道&gt;&gt;</a>", cbe.Cs_AllSpell, cbe.Cs_ShowName);
                sb.Append("                </li>");
                sb.Append("            </ul>");
                sb.Append("        </div>");
                sb.Append("    </div>");
                sb.Append("</div>");
                sb.Append(CarPhotoHtml);
                CarPhotoHtml = sb.ToString();
            }
        }
        /// <summary>
        /// 取车型参数
        /// </summary>
        private void GetParams()
        {
            if (this.Request.QueryString["CarID"] != null && this.Request.QueryString["CarID"].ToString() != "")
            {
                string tempCarID = this.Request.QueryString["CarID"].ToString();
                if (int.TryParse(tempCarID, out CarID))
                { }
            }
        }

        /// <summary>
        /// 取车型数据
        /// </summary>
        private void GetCarData()
        {
            if (CarID > 0)
            {
                cfcs = base.GetCarInfoForCarSummaryByCarID(CarID);

                // modified by chengl Nov.9.2009
                if (cfcs.CarID <= 0)
                {
                    Response.Redirect("/404error.aspx?info=无效车型");
                }
                // 子品牌信息
                cbe = (new Car_BasicBll()).Get_Car_BasicByCarID(CarID);
                if (cbe.Cs_Id <= 0)
                {
                    Response.Redirect("/404error.aspx?info=无效车型所属子品牌");
                }
                carEntity = (CarEntity)DataManager.GetDataEntity(EntityType.Car, CarID);
                // 广告
                base.MakeSerialTopADCode(cbe.Cs_Id);
                CarYear = cbe.Car_YearType > 0 ? cbe.Car_YearType + "款 " : "";
            }
        }

        /// <summary>
        /// 取子品牌热门车型
        /// </summary>
        private void GetHotCar()
        {
            DataSet ds = base.GetHotCarInfoByCsID(cbe.Cs_Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" <div class=\"section-header header3\">");
                sb.Append("    <div class=\"box\">");
                sb.Append("        <h2>");
                sb.AppendFormat("            <a href=\"javascript:;\">{0}热门车型</a></h2>", cbe.Cs_ShowName.Replace("(进口)", "").Replace("（进口）", ""));
                sb.Append("    </div>");
                sb.Append("</div>");
                sb.Append("<div class=\"list-txt list-txt-s list-txt-default list-txt-style5\">");

                sb.Append("<ul>");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (i >= 5)
                    { break; }
                    string carName = cbe.Cs_ShowName + " " + ds.Tables[0].Rows[i]["car_name"].ToString();
                    sb.Append("<li>");
                    sb.Append("    <div class=\"txt\"><a href=\"/" + cbe.Cs_AllSpell + "/m" + ds.Tables[0].Rows[i]["car_id"].ToString() + "/\" target=\"_blank\" alt=\"" + carName + "\">" + carName + "</a></div>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append("</div>");
                hotCarsHtml = sb.ToString();
            }
        }
        /// <summary>
        /// 得到品牌下的其他子品牌
        /// </summary>
        /// <returns></returns>
        protected string GetBrandOtherSerial()
        {
            return new Car_BrandBll().GetBrandOtherSerial(cbe.Cb_id, cbe.Cs_Id);
        }
        #endregion
    }
}