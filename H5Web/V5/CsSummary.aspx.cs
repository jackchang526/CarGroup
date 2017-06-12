using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace H5Web.V5
{
    public partial class CsSummary : H5PageBase
    {
        #region Services

        private readonly SerialFourthStageBll _serialFourthStageBll = new SerialFourthStageBll();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageCache(30);
            InitCustomizationType();
            GetParamter();
            InitData();

            #region 验证访问权限

            if (BaseSerialEntity == null)
            {
                Response.Redirect("http://car.h5.yiche.com/");
            }

            //概念车去移动站
            if (BaseSerialEntity != null && BaseSerialEntity.Level.Name == "概念车")
            {
                Response.Redirect("http://car.m.yiche.com/" + BaseSerialEntity.AllSpell);
            }

            if (BaseSerialEntity != null)
            {
                if (BaseSerialEntity.SaleState == "在销" || BaseSerialEntity.SaleState == "待销")
                {
                    //第四级 listAllCsID
                    SerialIdInConfigList = _serialFourthStageBll.GetAllSerialInH5();
                    if (!SerialIdInConfigList.Contains(SerialId))
                    {
                        var imageUrlDicNew = CarSerialImgUrlService.GetImageUrlDicNew();
                        if (imageUrlDicNew.ContainsKey(SerialId))
                        {
                            DefaultCarPic = imageUrlDicNew[SerialId].GetAttribute("ImageUrl2").Trim() != ""
                                ? string.Format(imageUrlDicNew[SerialId].GetAttribute("ImageUrl2").Trim(), "6")
                                : "http://image.bitautoimg.com/carchannel/pic/nopic600.jpg";

                            ShareImgUrl = DefaultCarPic.Replace("bitautoimg.com/",
                                "bitautoimg.com/newsimg-100-w0-1-q80/");
                        }
                    }
                    else
                    {
                        DefaultCarPic = string.Format("http://image.bitautoimg.com/carchannel/pic/cspic/{0}.jpg", SerialId);
                        ShareImgUrl = string.Format("http://image.bitautoimg.com/newsimg-100-w0-1-q80/carchannel/pic/cspic/{0}.jpg", SerialId);
                    }
                }
                else
                {
                    Response.Redirect("http://car.m.yiche.com/" + BaseSerialEntity.AllSpell);
                }
            }

            #endregion

            InitColorList(); //初始化颜色列表
            InitUserControl(); //初始化用户控件
            GetReferPrice(); //获取最低价
        }

        private void GetParamter()
        {
            SerialId = ConvertHelper.GetInteger(Request.QueryString["csID"]);

            // 是否是用户版
            if (!string.IsNullOrEmpty(Request.QueryString["IsUserEdition"])
                && Request.QueryString["IsUserEdition"] == "1")
            {
                IsUserEdition = true;
            }
            if (Request.QueryString["returnurl"] != null)
            {
                ChezhukaReturnUrl = Request.QueryString["returnurl"];
            }
            // 用户版、经纪人版需要微信分享
            if (Brokerid > 0 || IsUserEdition)
            {
                IsNeedShare = true;
            }
        }

        /// <summary>
        ///     初始化当前用户类型
        /// </summary>
        private void InitCustomizationType()
        {
            Brokerid = ConvertHelper.GetInteger(Request.QueryString["brokerid"]);
            if (Brokerid > 0)
            {
                CurrentCustomizationType = CustomizationType.Broker;
            }

            Dealerid = ConvertHelper.GetInteger(Request.QueryString["dealerid"]);
            if (Dealerid > 0)
            {
                CurrentCustomizationType = CustomizationType.Dealer;
            }
            Chezhuka = Request.QueryString["chezhukamark"];
            if (Chezhuka == "chezhuka")
            {
                CurrentCustomizationType = CustomizationType.CheZhuKa;
            }

            if (Request.QueryString["dealerpersonid"] != null)
            {
                DealerPersonId = ConvertHelper.GetInteger(Request.QueryString["dealerpersonid"]);
            }
            if (DealerPersonId > 0)
            {
                CurrentCustomizationType = CustomizationType.DealerSale;
            }

            AgentId = ConvertHelper.GetInteger(Request.QueryString["agentid"]);
            if (AgentId > 0)
            {
                CurrentCustomizationType = CustomizationType.HuiMaiCheGuWen;
            }
        }

        private void InitData()
        {
            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);
        }

        private void InitColorList()
        {
            var colorList = _serialFourthStageBll.GetSerialColorList(SerialId);
            if (colorList == null || colorList.Count <= 0) return;
            IsExistColor = true;
            // colorList.Sort((x, y) => x.ColorRGB.CompareTo(y.ColorRGB));
            SerialColorList = colorList.Count > 12 ? colorList.GetRange(0, 12) : colorList;
        }

        private void InitUserControl()
        {
            BrokerTmp.BaseSerialEntity = BaseSerialEntity;
            CommonTmpForCustomization.BaseSerialEntity = BaseSerialEntity;
            UserTmp.BaseSerialEntity = BaseSerialEntity;
        }

        /// <summary>
        ///     获得子品牌下车款的最低厂商指导价
        /// </summary>
        private void GetReferPrice()
        {
            var ds = GetCarIDAndNameForCS(SerialId, WebConfig.CachedDuration);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0) return;
            var carView = ds.Tables[0].DefaultView;
            //LowerReferPriceBySerial = ds.Tables[0].AsEnumerable().Where(x => ConvertHelper.GetDecimal(x["car_ReferPrice"]) > 0).Select(x => x.Field<decimal>("car_ReferPrice")).Min();
            var dr =
                ds.Tables[0].AsEnumerable()
                    .Where(x => ConvertHelper.GetDecimal(x["car_ReferPrice"]) > 0)
                    .OrderBy(x => ConvertHelper.GetDecimal(x["car_ReferPrice"]))
                    .FirstOrDefault();
            if (dr != null)
            {
                CarMinReferPrice = ConvertHelper.GetDecimal(dr["car_ReferPrice"]);
            }
        }

        /// <summary>
        ///     获取枚举描述信息
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        protected string GetEnumDescription(Enum enumValue)
        {
            var str = enumValue.ToString();
            var field = enumValue.GetType().GetField(str);
            var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs.Length == 0) return str;
            var da = (DescriptionAttribute)objs[0];
            return da.Description;
        }

        #region Params

        protected CustomizationType CurrentCustomizationType = CustomizationType.User;

        protected int SerialId;

        protected bool IsExistColor;

        // 经纪人ID
        protected int Brokerid;
        // 经销商ID
        protected int Dealerid;
        //经销商人车店
        protected int DealerPersonId;
        //车主卡
        protected string Chezhuka = string.Empty;
        //惠买车经纪人ID
        protected int AgentId;

        protected string ChezhukaReturnUrl = "http://yanghu.w.yiche.com/CZK/IntentCar"; //默认地址
        protected decimal CarMinReferPrice;
        protected string DefaultCarPic = string.Empty;
        protected string ShareImgUrl = string.Empty;

        protected SerialEntity BaseSerialEntity;
        protected List<SerialColorForSummaryEntity> SerialColorList = new List<SerialColorForSummaryEntity>();

        protected List<int> SerialIdInConfigList = new List<int>();

        /// <summary>
        ///     是否需要设置微信分享 用户普通版、经销商(等对方迁移)、经纪人(等对方迁移)
        /// </summary>
        protected bool IsNeedShare;

        /// <summary>
        ///     是否是用户版
        /// </summary>
        protected bool IsUserEdition;

        #endregion
    }

    /// <summary>
    ///     第四级定制版类型定义
    /// </summary>
    public enum CustomizationType
    {
        [Description("用户版")]
        User = 1,
        [Description("经纪人定制版")]
        Broker = 2,
        [Description("经销商普通定制版")]
        Dealer = 3,
        [Description("经销商人车店定制版")]
        DealerSale = 4,
        [Description("车主卡定制版")]
        CheZhuKa = 5,
        [Description("惠买车定制版")]
        HuiMaiCheGuWen = 6,
    }
}