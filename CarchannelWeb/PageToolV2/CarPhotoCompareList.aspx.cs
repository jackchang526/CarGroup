using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.PageToolV2
{
    public partial class CarPhotoCompareList : PageBase
    {
        #region new edit by chengl Jan.18.2012
        protected int adSerialId = 0;
        protected string adParameters = string.Empty;

        protected string firstCsPrice = string.Empty;
        protected string validCsIDs = string.Empty;
        private List<int> listValidCsIDs = new List<int>();
        protected string csIDs = string.Empty;
        // 当前有效数量
        protected int validCount = 0;
        protected bool isHasContent = false;
        protected string carIDs = string.Empty;
        protected string titleForSEO = "【汽车图片对比选车_汽车车型大全】";
        private StringBuilder sb = new StringBuilder();
        protected string photoCompare = string.Empty;
        private int loopForSEO = 0;
        private CommonService cs = new CommonService();
        private CommonFunction cf = new CommonFunction();

        protected int categoryID = 6;
        private List<int> listCsID = new List<int>();

        /// <summary>
        /// 页面浮动内容
        /// </summary>
        protected string FloatDivHTML = string.Empty;
        /// <summary>
        /// 子品牌图片对比数据集合
        /// </summary>
        private Dictionary<int, EnumCollection.SerialPhotoCompareDataNew> dicCsInfo = new Dictionary<int, EnumCollection.SerialPhotoCompareDataNew>();
        /// <summary>
        /// 图片对比的 配置 <位置ID,位置配置>
        /// </summary>

        #endregion


        private Car_BasicBll _carBasicBLL;

        private Dictionary<int, EnumCollection.SerialPhotoCompareDataNew> dictCarInfo = new Dictionary<int, EnumCollection.SerialPhotoCompareDataNew>();

        private Dictionary<int, EnumCollection.PhotoCompareConfig> dicPhotoCompareConfig = new Dictionary<int, EnumCollection.PhotoCompareConfig>();

        private Dictionary<int, string> dicCarPhoto;

        protected string photoHeaderHtml = string.Empty;
        protected string photoCompareHtml = string.Empty;
        protected string menuSideHtml = string.Empty;

        protected string carInfoJson = string.Empty;
        protected List<int> paramsSerialIdList = new List<int>();

        public CarPhotoCompareList()
        {
            _carBasicBLL = new Car_BasicBll();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(10);
            GetParamsters();
            RenderContent();
        }

        private void GetParamsters()
        {
            carIDs = ConvertHelper.GetString(Request.QueryString["carids"]);
            csIDs = ConvertHelper.GetString(Request.QueryString["csids"]);
            if (string.IsNullOrEmpty(carIDs))
                GetDefaultCarIds();
            dicCarPhoto = _carBasicBLL.GetCarDefaultPhotoDictionary(2);
        }

        /// <summary>
        /// 获取子品牌 默认车型有图  最新年款 最热门车型 
        /// </summary>
        private void GetDefaultCarIds()
        {
            if (string.IsNullOrEmpty(csIDs)) return;
            var serialIdArray = csIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct().Select(serialId => ConvertHelper.GetInteger(serialId));
            Dictionary<int, int> dict = _carBasicBLL.GetHotCarForPhotoCompareBySerialId(serialIdArray);
            carIDs = string.Join(",", dict.Values.ToArray().Take(3));
        }

        /// <summary>
        /// 输出对比图片内容
        /// </summary>
        private void RenderContent()
        {
            var carIdArray = carIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct().Select(serialId => ConvertHelper.GetInteger(serialId));
            //获取图片对比配置信息
            GetPhotoCompareConfig();
            foreach (int carId in carIdArray)
            {
                //获取车款图片对比数据
                DataSet ds = cs.GetCarPhotoDataForCompare(carId, 60);
                //获取 车款 相关信息
                this.GetPhotoCompareInfoNew(carId, ds);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                { isHasContent = true; }
            }
            //输出头部内容
            this.RenderNavContent();

            StringBuilder sb = new StringBuilder();
            string[] classArray = { "img-compare-left", "img-compare-mid", "img-compare-right" };
            List<object> carInfoJsonList = new List<object>();
            List<string> menuSideHtmlList = new List<string>();
            int loop = 0;
            foreach (KeyValuePair<int, EnumCollection.SerialPhotoCompareDataNew> kvpCar in dictCarInfo)
            {
                //参数传递需要
                if (!paramsSerialIdList.Contains(kvpCar.Value.CsID))
                    paramsSerialIdList.Add(kvpCar.Value.CsID);

                carInfoJsonList.Add(new { MasterId = kvpCar.Value.MasterId, SerialId = kvpCar.Value.CsID, CarId = kvpCar.Value.DefaultCarID, CarName = kvpCar.Value.DefaultCarName});
                sb.AppendFormat("<div class=\"{0}\">", classArray[loop]);
                int prevCateoryId = 0;
                int configLoop = 0;
                foreach (KeyValuePair<int, EnumCollection.PhotoCompareConfig> kvpCover in dicPhotoCompareConfig)
                {
                    // 如果此位置没有图则循环下1个
                    if (!kvpCover.Value.IsHasContent)
                    { continue; }
                    int cateoryId = kvpCover.Value.ParentCatetroyId;

                    sb.AppendFormat("<div class=\"sub-tt\" id=\"{1}\"><span>{0}</span></div>",
                        loop == 0 ? kvpCover.Value.CoverPropertyName : "", (loop == 0 && cateoryId != prevCateoryId) ? "menu-side-" + cateoryId : "");
                    //左侧菜单
                    if (loop == 0 && cateoryId != prevCateoryId)
                    {
                        menuSideHtmlList.Add(string.Format("<li class=\"{0}\"><a data-target=\"{1}\" href=\"javascript:;\">{2}</a></li>",
                            configLoop == 0 ? "current" : "", "menu-side-" + cateoryId, kvpCover.Value.ParentCategoryName));
                    }
                    configLoop++;
                    prevCateoryId = cateoryId;

                    string imgURL = "http://image.bitautoimg.com/carchannel/images/nopic300_200.JPG";
                    string imgLink = "http://photo.bitauto.com/picture/" + kvpCar.Value.CsID.ToString() + "/";
                    sb.Append("<div class=\"img-box\">");
                    if (kvpCar.Value.DicPhotoComparePhotoInfo.ContainsKey(kvpCover.Key)
                            && kvpCar.Value.DicPhotoComparePhotoInfo[kvpCover.Key].ImageURL != "")
                    {
                        imgURL = kvpCar.Value.DicPhotoComparePhotoInfo[kvpCover.Key].ImageURL;
                        imgLink = imgLink + kvpCar.Value.DicPhotoComparePhotoInfo[kvpCover.Key].SiteImageId + "/";
                        sb.AppendLine("<a target=\"_blank\" href=\"" + imgLink + "\"><img alt=\"\" src=\"" + imgURL + "\"></a>");
                    }
                    else
                    {
                        sb.AppendLine("<img class=\"noneCar\" alt=\"\" src=\"" + imgURL + "\">");
                    }
                    if (kvpCover.Value.OtherParam != null && kvpCover.Value.OtherParam.Count > 0)
                    {
                        sb.AppendLine("<p>");
                        // 当此位置需要显示扩展参数时
                        foreach (EnumCollection.CarParamForPhotoCompare cpfpc
                            in kvpCover.Value.OtherParam)
                        {
                            // 如果此车型有此参数值
                            if (kvpCar.Value.OtherParam != null
                                && kvpCar.Value.OtherParam.Count > 0
                                && kvpCar.Value.OtherParam.ContainsKey(cpfpc.ParamID)
                                && kvpCar.Value.OtherParam[cpfpc.ParamID] != ""
                                )
                            {
                                var paramsValue = kvpCar.Value.OtherParam[cpfpc.ParamID];
                                sb.Append(" " + cpfpc.ParamName + ":" + (paramsValue == "待查" ? "" : paramsValue) + "" + cpfpc.ParamUnit);
                            }
                        }
                        sb.AppendLine("</p>");
                    }
                    sb.Append("</div>");
                }
                sb.AppendFormat("</div>");
                loop++;
                if (loop >= 3) break;
            }
            // 当不足3列时候补齐到3列(这里至少会有1个)
            if (loop < 3)
            {
                for (int i = loop; i < 3; i++)
                {
                    sb.AppendFormat("<div class=\"{0}\">", classArray[i]);
                    // 循环每个位置
                    int nocarFirstLoop = 0;
                    foreach (KeyValuePair<int, EnumCollection.PhotoCompareConfig> kvpCover in dicPhotoCompareConfig)
                    {
                        // 如果此位置没有图则循环下1个
                        if (!kvpCover.Value.IsHasContent)
                        { continue; }
                        sb.AppendFormat("<div class=\"sub-tt\"><span>{0}</span></div>",
                            loop == 0 ? kvpCover.Value.CoverPropertyName : "");
                        if (nocarFirstLoop == 0 && i == loop)
                        {
                            sb.Append("<div class=\"recommend-box\" id=\"recommend-box\">");
                            sb.Append("</div>");
                        }
                        else
                        {
                            sb.Append("<div class=\"img-box\">");
                            sb.AppendLine("<img class=\"noneCar\" alt=\"\" src=\"http://image.bitautoimg.com/carchannel/images/nopic300_200.JPG\">");
                            sb.Append("</div>");
                        }
                        nocarFirstLoop++;
                    }
                    sb.Append("</div>");
                }
            }

            var j = new JavaScriptSerializer();
            carInfoJson = j.Serialize(carInfoJsonList);

            photoCompare = sb.ToString();

            if (menuSideHtmlList.Count > 0)
            {
                menuSideHtml = string.Format("<div class=\"left-nav\" id=\"left-nav\" style=\"position: absolute; top: 187px; left: -76px;\" ><ul>{0}</ul></div>", string.Concat(menuSideHtmlList.ToArray()));
            }
        }

        /// <summary>
        /// 输出头内容
        /// </summary>
        private void RenderNavContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"img-compare-header\" id=\"img-compare-header\">");
            List<string> list = new List<string>() { "one", "two", "three" };
            int loop = 0;
            foreach (KeyValuePair<int, EnumCollection.SerialPhotoCompareDataNew> kvpCar in dictCarInfo)
            {

                sb.AppendFormat("<div class=\"{0} has-car\">", list[loop]);
                sb.Append("<div class=\"con-box\">");
                sb.AppendFormat("<img src=\"{0}\">", kvpCar.Value.SerialImageUrl.Replace("_2.", "_1."));
                sb.Append("<dl>");
                sb.AppendFormat("<dt><a target=\"_blank\" href=\"/{0}/\">{1}</a></dt>", kvpCar.Value.CsAllSpell, kvpCar.Value.SerialShowName);
                sb.Append("<dd>");
                sb.AppendFormat("<a target=\"_blank\" href=\"/{0}/m{1}/\">{2} {3}</a></dd>",
                    kvpCar.Value.CsAllSpell, kvpCar.Value.DefaultCarID, kvpCar.Value.DefaultCarYear, kvpCar.Value.DefaultCarName);
                sb.Append("</dl>");
                sb.Append("<div class=\"change-car-area\">");
                sb.AppendFormat("<a id=\"btn-changecar-{0}\" bit-changeindex=\"{0}\" href=\"javascript:;\" class=\"huanche-btn btn-show-car\"><span class=\"huanche\">换辆车</span></a>", loop);
                sb.Append("<div class=\"clear\">");
                sb.Append("</div>");
                sb.AppendFormat("<div class=\"drop-layer huanche-layer\" bit-popupindex=\"{0}\" style=\"display: none;\">", loop);
                sb.Append("<div class=\"db-car-box\">");
                sb.AppendFormat("<div class=\"brand-form  no-second\" id=\"change-master-{0}\" style=\"z-index: 50\"></div>", loop);
                sb.AppendFormat("<div class=\"brand-form  no-second\" id=\"change-serial-{0}\" style=\"z-index: 40\"></div>", loop);
                sb.AppendFormat("<div class=\"brand-form  no-second\" id=\"change-car-{0}\" style=\"z-index: 30\"></div>", loop);
                sb.Append("</div>");
                sb.AppendFormat("<div class=\"db-txt-list\"><div id=\"recom-car-list-{0}\" class=\"list-txt list-txt-s list-txt-default list-txt-style4\"></div></div>", loop);
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.AppendFormat("<a href=\"javascript:closeCarCompare({0});\" class=\"btn-close-car\">关闭</a>", kvpCar.Value.DefaultCarID);
                sb.Append("</div>");
                loop++;
                if (loop >= 3) break;
            }
            // 当不足3列时候补齐到3列(这里至少会有1个)
            if (loop < 3)
            {
                for (int i = loop; i < 3; i++)
                {
                    sb.AppendFormat("<div class=\"{0} {1}\">", list[i], i == loop ? "" : "bg-gray");
                    sb.AppendFormat("{0}", i == 0 ? "<div class=\"select-box\"><div class=\"con-box\">" : "<div class=\"select-box\">");
                    #region
                    if (i == loop)
                    {
                        sb.Append("<div id=\"master4\" class=\"brand-form no-second\"><span class=\"default\">请选择品牌</span><a href=\"#\" class=\"jt\"></a></div>");
                        sb.AppendFormat("<div id=\"serial4\" class=\"brand-form no-second {0}\"><span class=\"default\">请选择车型</span><a href=\"#\" class=\"jt\"></a></div>", i == 0 ? "brand-disabled" : "");
                        sb.AppendFormat("<div id=\"cartype4\" class=\"brand-form no-second {0}\"><span class=\"default\">请选择车款</span><a href=\"#\" class=\"jt\"></a></div>", i == 0 ? "brand-disabled" : "");
                    }
                    else
                    {
                        sb.Append("<div class=\"brand-form no-second brand-disabled\"><span class=\"default\">请选择品牌</span><a href=\"#\" class=\"jt\"></a></div>");
                        sb.Append("<div class=\"brand-form no-second brand-disabled\"><span class=\"default\">请选择车型</span><a href=\"#\" class=\"jt\"></a></div>");
                        sb.Append("<div class=\"brand-form no-second brand-disabled\"><span class=\"default\">请选择车款</span><a href=\"#\" class=\"jt\"></a></div>");    
                    }
                    #endregion

                    sb.AppendFormat("{0}", i == 0 ? "</div></div>" : "</div>");
                    sb.Append("</div>");
                }
            }
            sb.Append("</div>");
            photoHeaderHtml = sb.ToString();
        }

        /// <summary>
        /// 获取对比图片位置配置文件信息
        /// </summary>
        private void GetPhotoCompareConfig()
        {
            #region 先取配置
            XmlDocument docTempConfig = cs.GetPhotoCompareTemplate();
            if (docTempConfig != null && docTempConfig.HasChildNodes)
            {
                XmlNodeList xnl = docTempConfig.SelectNodes("//Item");
                if (xnl != null)
                {
                    // 循环每个图片位子
                    foreach (XmlNode xn in xnl)
                    {
                        int parentCateoryId = ConvertHelper.GetInteger(xn.ParentNode.Attributes["ID"].Value);
                        string parentCategoryName = xn.ParentNode.Attributes["Name"].Value;
                        string cateName = xn.Attributes["Name"].Value.ToString();
                        int cateID = int.Parse(xn.Attributes["ID"].Value.ToString());
                        string otherParam = xn.Attributes["OtherParam"].Value.ToString().Trim();
                        EnumCollection.PhotoCompareConfig pcc = new EnumCollection.PhotoCompareConfig();
                        pcc.ParentCatetroyId = parentCateoryId;
                        pcc.ParentCategoryName = parentCategoryName;
                        pcc.CoverPropertyID = cateID;
                        pcc.CoverPropertyName = cateName;
                        pcc.IsHasContent = false;
                        pcc.OtherParam = null;
                        if (otherParam != "")
                        {
                            string[] arrParam = otherParam.Split('|');
                            if (arrParam.Length > 0)
                            {
                                foreach (string paramInfo in arrParam)
                                {
                                    string[] arrInfo = paramInfo.Trim().Split(',');
                                    if (arrInfo.Length == 3)
                                    {
                                        EnumCollection.CarParamForPhotoCompare cpfpc = new EnumCollection.CarParamForPhotoCompare();
                                        cpfpc.ParamID = int.Parse(arrInfo[1]);
                                        cpfpc.ParamName = arrInfo[0].Trim();
                                        cpfpc.ParamUnit = arrInfo[2].Trim();
                                        if (pcc.OtherParam == null)
                                        {
                                            pcc.OtherParam = new List<EnumCollection.CarParamForPhotoCompare>();
                                        }
                                        if (!pcc.OtherParam.Contains(cpfpc))
                                        { pcc.OtherParam.Add(cpfpc); }
                                    }
                                }
                            }
                        }
                        if (!dicPhotoCompareConfig.ContainsKey(cateID))
                        { dicPhotoCompareConfig.Add(cateID, pcc); }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取对比车型信息
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="ds"></param>
        private void GetPhotoCompareInfoNew(int carId, DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                return;

            CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
            if (ce != null && ce.Id > 0 && ce.Serial.Id > 0)
            {
                EnumCollection.SerialPhotoCompareDataNew spcd = new EnumCollection.SerialPhotoCompareDataNew();
                spcd.MasterId = ce.Serial.Brand.MasterBrandId;
                spcd.CsID = ce.Serial.Id;
                spcd.DefaultCarID = carId;
                spcd.DefaultCarName = ce.Name.Trim();
                spcd.DefaultCarYear = ce.CarYear > 0 ? ce.CarYear.ToString() + "款" : "";
                spcd.SerialImageUrl = dicCarPhoto.ContainsKey(carId) ? dicCarPhoto[carId] : Car_SerialBll.GetSerialImageUrl(ce.Serial.Id, 2, false);
                spcd.SerialName = ce.Serial.Name;
                spcd.SerialShowName = ce.Serial.ShowName;
                spcd.CsAllSpell = ce.Serial.AllSpell;
                // 车型扩展参数
                Dictionary<int, string> dic = new Car_BasicBll().GetCarAllParamByCarID(spcd.DefaultCarID);
                if (dic != null && dic.Count > 0)
                {
                    spcd.OtherParam = dic;
                }
                // 图片按位置检测 取出图片ID和图片地址
                spcd.DicPhotoComparePhotoInfo = new Dictionary<int, EnumCollection.PhotoComparePhotoInfo>();

                string imgURL = string.Empty;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int CoverPropertyId = int.Parse(ds.Tables[0].Rows[i]["PositionID"].ToString());
                    int SiteImageId = int.Parse(ds.Tables[0].Rows[i]["ImageID"].ToString());
                    //imgURL = CommonFunction.GetPublishHashImgUrl(4, ds.Tables[0].Rows[i]["ImageUrl"].ToString(), int.Parse(ds.Tables[0].Rows[i]["ImageUrl"].ToString()));
                    imgURL = ds.Tables[0].Rows[i]["ImageUrl"].ToString();

                    if (!spcd.DicPhotoComparePhotoInfo.ContainsKey(CoverPropertyId))
                    {
                        EnumCollection.PhotoComparePhotoInfo ppi = new EnumCollection.PhotoComparePhotoInfo();
                        ppi.SiteImageId = SiteImageId;
                        ppi.ImageURL = imgURL;
                        spcd.DicPhotoComparePhotoInfo.Add(CoverPropertyId, ppi);

                        // 如果此位置有图片的话
                        if (dicPhotoCompareConfig.ContainsKey(CoverPropertyId))
                        {
                            dicPhotoCompareConfig[CoverPropertyId].IsHasContent =
                                dicPhotoCompareConfig[CoverPropertyId].IsHasContent || true;
                        }
                    }
                }
                if (!dictCarInfo.ContainsKey(carId))
                {
                    dictCarInfo.Add(carId, spcd);
                }
            }
        }
    }
}