using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;

namespace WirelessWeb
{
    public partial class CarPhoto : WirelessPageBase
    {
        #region member

        protected int carID = 0;
        protected CarEntity ce;

        protected string exhaustForFloat = string.Empty;
        protected string transmissionType = string.Empty;
        protected string carList = string.Empty;
        protected string carAllPhotoHTML = string.Empty;
        protected string carSummaryAD = string.Empty;
        //参考成交价
        protected string cankaoPrice = string.Empty;
        //贷款首付
        protected string loanDownPay = string.Empty;
        //月付
        protected string monthPay = string.Empty;
        //预估总价
        protected string totalPay = string.Empty;


        private List<EnumCollection.CarInfoForSerialSummary> ls = new List<EnumCollection.CarInfoForSerialSummary>();
        Car_SerialBll carBLL;

        #endregion
        public CarPhoto()
		{
			carBLL = new Car_SerialBll();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				base.SetPageCache(30);
				GetPageParam();
				GetPhotoData();
			}
		}
        #region private Method

        /// <summary>
        /// 取页面参数
        /// </summary>
        private void GetPageParam()
        {
            string strCarID = this.Request.QueryString["CarID"];
            if (!string.IsNullOrEmpty(strCarID)
                && int.TryParse(strCarID, out carID))
            { }
        }

        /// <summary>
        /// 取车型数据
        /// </summary>
        private void GetPhotoData()
        {
            if (carID > 0)
            {
                ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carID);
                if (ce != null && ce.Id > 0)
                {
                    ComputerCarPrice();
                    GetCarListByCsID();
                    GetCarAllPhoto();
                }
            }
        }

        /// <summary>
        /// 取车型外观内饰空间的图片
        /// </summary>
        private void GetCarAllPhoto()
        {
            List<string> listTempClass = new List<string>();
            XmlDocument docPC = new XmlDocument();
            string cache = "CarPhoto_all_" + carID;
            object carPhoto = null;
            CacheManager.GetCachedData(cache, out carPhoto);
            if (carPhoto != null)
            {
                docPC = (XmlDocument)carPhoto;
            }
            else
            {
                var filePath = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\PhotoImage\\CarImagesListInfo\\{0}.xml"
                , carID.ToString()));
                if (File.Exists(filePath))
                {
                    docPC.Load(filePath);
                    CacheManager.InsertCache(cache, docPC, new System.Web.Caching.CacheDependency(filePath), DateTime.Now.AddMinutes(60));
                }
            }
            if (docPC != null && docPC.HasChildNodes)
            {
                XmlNode rootPC = docPC.DocumentElement;
                if (rootPC != null && rootPC.ChildNodes.Count > 0)
                {
                    foreach (XmlNode xng in rootPC.ChildNodes)
                    {
                        if (xng.NodeType != XmlNodeType.Element)
                        { continue; }
                        // 大类外观 内饰 空间
                        if (xng != null && xng.ChildNodes.Count > 0)
                        {
                            listTempClass.Add("<div class=\"tt-small tt-small-dark\"><span>");
                            listTempClass.Add(xng.Attributes["Name"].Value);
                            listTempClass.Add("</span></div>");
                            listTempClass.Add("<div class=\"pic-select-car-3\"><ul> ");
                            foreach (XmlNode xn in xng.ChildNodes)
                            {
                                if (xn.NodeType != XmlNodeType.Element)
                                { continue; }
                                int imgId = 0;
                                if (xn.Attributes["ImageId"] != null
                                    && xn.Attributes["ImageId"].Value != ""
                                    && int.TryParse(xn.Attributes["ImageId"].Value, out imgId))
                                { }
                                if (imgId > 0 && !string.IsNullOrEmpty(xn.Attributes["ImageUrl"].Value))
                                {
                                    listTempClass.Add(string.Format("<li><a href=\"http://photo.m.yiche.com/picture/{1}/{2}\"><img src=\"{0}\"></a></li>", string.Format(xn.Attributes["ImageUrl"].Value,1), ce.SerialId, imgId));
                                }
                            }
                            listTempClass.Add("</ul></div>");

                        }
                    }
                }

                if (listTempClass.Count <= 0)
                {
                    listTempClass.Add("<div class=\"wrap\">");
                    listTempClass.Add("<div class=\"m-no-result2\">");
                    listTempClass.Add("<div class=\"face face-fail\"></div>");
                    listTempClass.Add("<h6>该车款暂无实拍图，请尝试选择其它车款。</h6>");
                    listTempClass.Add("</div></div>");
                }
                carAllPhotoHTML = string.Concat(listTempClass.ToArray());
            }

        }

        /// <summary>
        /// 计算车款贷款信息
        /// </summary>
        private void ComputerCarPrice()
        {
            CarPriceComputer priceComputer = new CarPriceComputer(carID);
            priceComputer.ComputeCarPrice();
            priceComputer.LoanPaymentYear = 3;
            priceComputer.ComputeCarAutoLoan();
            double loan = 0;
            if (priceComputer.LoanFirstDownPayments > 0)
                loan = ((double)(priceComputer.LoanFirstDownPayments + priceComputer.AcquisitionTax + priceComputer.Compulsory + priceComputer.Insurance + priceComputer.VehicleTax + priceComputer.Chepai));

            //totalPay = priceComputer.LoanFirstDownPayments > 0 ? (priceComputer.LoanMonthPayments * 3 * 12 + loan).ToString("#,##") + "元" : "暂无";
            totalPay = string.IsNullOrEmpty(priceComputer.FormatTotalPrice) ? "暂无" : priceComputer.FormatTotalPrice.ToString() + "元";
            loanDownPay = loan > 0 ? (loan / 10000).ToString("F2") + "万元" : "暂无";
            monthPay = priceComputer.LoanMonthPayments > 0 ? priceComputer.LoanMonthPayments + "元" : "暂无";
            cankaoPrice = GetCarPriceByID(carID);
            if (string.IsNullOrEmpty(cankaoPrice))
            {
                cankaoPrice = "暂无";
            }
        }

        /// <summary>
        /// 取同子品牌下其他车型
        /// </summary>
        private void GetCarListByCsID()
        {
            if (ce.Id > 0 && ce.Serial.Id > 0)
            {
                if (ce.SaleState == "停销")
                {
                    // 停销子品牌取 停销子品牌最新年款的车型(高总逻辑 Oct.11.2011)
                    ls = GetAllCarInfoForNoSaleSerialSummaryByCsID(ce.Serial.Id);
                }
                else
                {
                    // 非停销子品牌取 子品牌的非停销所有年款车型
                    ls = base.GetAllCarInfoForSerialSummaryByCsID(ce.Serial.Id);
                }
                if (ls.Count > 0)
                {
                    var year = string.Empty;
                    ls.Sort(NodeCompare.CompareCarByYear);
                    List<string> listCarList = new List<string>(20);
                    foreach (EnumCollection.CarInfoForSerialSummary cifss in ls)
                    {
                        //modified by sk 2013.06.03 修改最低报价
                        string carMinPrice = string.Empty;
                        string carPriceRange = cifss.CarPriceRange.Trim();
                        if (cifss.CarPriceRange.Trim().Length == 0)
                            carMinPrice = "暂无报价";
                        else
                        {
                            if (carPriceRange.IndexOf('-') != -1)
                                carMinPrice = carPriceRange.Substring(0, carPriceRange.IndexOf('-'));
                            else
                                carMinPrice = carPriceRange;
                        }

                        if (!string.IsNullOrEmpty(cifss.CarYear) && cifss.CarYear != year)
                        {
                            year = cifss.CarYear;
                            listCarList.Add("<dt><span>" + year + "款</span></dt>");
                        }

                        if (cifss.CarID == carID)
                        {
                            listCarList.Add("<dd class=\"current\"><a href=\"#\"><p>" + cifss.CarName + "</p><strong>" + carMinPrice + "</strong></a></dd>");
                        }
                        else
                        {
                            listCarList.Add("<dd><a href=\"/" + ce.Serial.AllSpell + "/m" + cifss.CarID + "/tupian\"><p>" + cifss.CarName + "</p><strong>" + carMinPrice + "</strong></a></dd>");
                        }
                    }
                    carList = string.Concat(listCarList.ToArray());
                }
            }
        }
        #endregion
    }
}