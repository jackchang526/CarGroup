using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.CarChannel.BLL;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using System.Xml;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// 根据车型ID 城市ID 取经销商
	/// </summary>
	public partial class CarPriceListByCarIDAndCityID : PageBase
	{

		#region Member
		private int carID = 0;
		private int cityID = 201;
        /// <summary>
        /// 1=json,2=xml
        /// </summary>
        private int returnType = 0;
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				base.SetPageCache(30);
				Response.ContentType = "application/x-javascript";
				GetPageParam();
                if (returnType == 1)
                {
                    string value = GetCarPriceJsonFromMongoDB();
                    Response.Write("var carDealerJson = ");
                    Response.Write(value);
                    Response.Write(";");
                }
                else if (returnType == 2)
                {
                    Response.ContentType = "Text/XML";
                    Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                    Response.Write(GetCarPriceXMLFromMongoDB());
                }
                else
                {
                    string strCarPrice = GetCarPriceListFromMongoDB();
                    if (strCarPrice.Length > 0)
                    {
                        // sb.Insert(0, "<h3><span><a href=\"http://price.bitauto.com/car.aspx?newcarId=" + carID.ToString() + "&citycode=" + cityID.ToString() + "\" target=\"_blank\">北京经销商报价</a></span></h3>");
                        //sb.Insert(0, " var carDealerList = '<!-- " + DateTime.Now.ToString() + " -->");
                        //sb.Append("';");
                        Response.Write(" var carDealerList = \"<!-- " + DateTime.Now.ToString() + " -->" + HttpUtility.UrlEncode(strCarPrice).Replace("+", "%20") + "\";");
                    }
                }
			}
		}

		/// <summary>
		/// 取页面参数
		/// </summary>
		private void GetPageParam()
		{
			if (this.Request.QueryString["CarID"] != null && this.Request.QueryString["CarID"].ToString() != "")
			{
				if (int.TryParse(this.Request.QueryString["CarID"].ToString(), out carID))
				{ }
			}

			if (this.Request.QueryString["CityID"] != null && this.Request.QueryString["CityID"].ToString() != "")
			{
				if (int.TryParse(this.Request.QueryString["CityID"].ToString(), out cityID))
				{ }
			}
            returnType = ConvertHelper.GetInteger(this.Request.QueryString["returnType"]);
		}

		/// <summary>
		/// 取车型报价
		/// </summary>
		private string GetCarPriceList()
		{
			StringBuilder sb = new StringBuilder(1000);
			if (carID > 0 && cityID > 0)
			{
				DataSet ds = new Car_BasicBll().GetCarAllCityPriceDealerByCarID(carID);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					int loop = 0;
					DataRow[] drs = ds.Tables[0].Select("cid = '" + cityID.ToString() + "'", "vendororderweight Desc");
					if (drs != null && drs.Length > 0)
					{
						foreach (DataRow dr in drs)
						{
							if (loop >= 5)
							{ break; }
							loop++;
							string vendorName = dr["vendorName"].ToString().Trim();
							string vendorID = dr["vendorId"].ToString().Trim();
							string vendorBizMo = dr["vendorBizMode"].ToString().Trim();
							string vendorSaleAddr = dr["vendorSaleAddr"].ToString().Trim();
							string dealerUrl = dr["dealerUrl"].ToString().Trim();
							string dealerTel = base.GetDealerFor400(vendorID);
							if (dealerTel == "")
							{ dealerTel = "<span>" + dr["vendorTel"].ToString().Trim().Replace("$", "/") + "</span>"; }
							else
							{ dealerTel = "<span title=\"易车网认证电话，请放心拨打！\"><strong>" + dealerTel + "</strong></span>"; }
							decimal vendorPrice = (dr["vendorPrice"].ToString() != "" ? Math.Round(decimal.Parse(dr["vendorPrice"].ToString()), 2) : 0);
							decimal totalPrice = (dr["totalPrice"].ToString() != "" ? Math.Round(decimal.Parse(dr["totalPrice"].ToString()), 2) : 0);
							if (loop == 5 || loop == drs.Length)
							{
								sb.Append("<div class=\"car_dealer_item car_dealer_item_last\">");
							}
							else
							{
								sb.Append("<div class=\"car_dealer_item\">");
							}
							sb.Append("<dl>");
							sb.Append("<dt><a href=\"http://dealer.bitauto.com/" + vendorID + "/\" target=\"_blank\"><span>" + (vendorBizMo == "2" ? "[4S店]" : "") + "</span> " + vendorName + "</a></dt>");
							sb.Append("<dd><label>电话：</label>" + dealerTel + "</dd>");
							sb.Append("<dd><label>地址：</label><span>" + vendorSaleAddr + " [<a <a onclick=\"window.open('http://dealer.bitauto.com/VendorMap/GoogleMap.aspx?dID=" + vendorID + "&S=S&W=400&H=300&Z=12','mapwindow','height=300,width=400,top=90,left=100,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no');return false;\" href=\"#\">查看地图</a>]</span></dd>");
							sb.Append("</dl>");
							sb.Append("<ul class=\"car_price\">");
							sb.Append("<li><a href=\"" + dealerUrl + "\" target=\"_blank\">" + vendorPrice.ToString("G0") + "万" + "</a></li>");
							sb.Append("<li>预估购车总价：<strong>" + totalPrice.ToString("G0") + "万" + "</strong></li>");
							sb.Append("</ul>");
							sb.Append("<a href=\"http://dealer.bitauto.com/" + vendorID + "/price_detail/" + carID.ToString() + ".html?type=open\" target=\"_blank\" class=\"btn_price\">询价&gt;&gt;</a>");
							// add by chengl Dec.13.2011
							sb.Append("<a href=\"http://dealer.bitauto.com/" + vendorID + "/price_detail/" + carID.ToString() + ".html?type=open&order=1\" target=\"_blank\" class=\"btn_price\">试驾&gt;&gt;</a>");
							sb.Append("</div>");
						}
					}
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// 从mongodb里面获取车型报价
		/// </summary>
		/// <returns></returns>
		private string GetCarPriceListFromMongoDB()
		{
			List<string> strlist = new List<string>();
			if (carID > 0 && cityID > 0)
			{
				List<CarChannel.Model.DealerInfo> dealerlist = new Car_BasicBll().GetCarAllCityDealserFromMongoDB(carID);
				List<CarChannel.Model.DealerInfo> newlist = dealerlist.FindAll(Match);
				newlist.Sort(this.Comparer);
				if (newlist.Count > 0)
				{
					int loop = 0;
					foreach (CarChannel.Model.DealerInfo dealer in newlist)
					{
						if (loop >= 5)
						{ break; }
						loop++;
						string vendorName = dealer.VendorName.ToString().Trim();
						string vendorID = dealer.DealerId.ToString().Trim();
						string vendorBizMo = dealer.DealerType.ToString().Trim();
						string vendorSaleAddr = dealer.Address != null ? dealer.Address.ToString().Trim() : "";
						string dealerUrl = dealer.DealerUrl != null ? dealer.DealerUrl.ToString().Trim() : "";
						string dealerTel = base.GetDealerFor400(vendorID);
						if (dealerTel == "")
						{ dealerTel = "<span>" + dealer.PhoneNumber.ToString().Trim().Replace("$", "/") + "</span>"; }
						else
						{ dealerTel = "<span title=\"易车网认证电话，请放心拨打！\"><strong>" + dealerTel + "</strong></span>"; }
						decimal vendorPrice = (dealer.SalePrice.ToString() != "" ? Math.Round(decimal.Parse(dealer.SalePrice.ToString()), 2) : 0);
						decimal totalPrice = (dealer.EvaluatePrice.ToString() != "" ? Math.Round(decimal.Parse(dealer.EvaluatePrice.ToString()), 2) : 0);
						if (loop == 5 || loop == newlist.Count)
						{
							strlist.Add("<div class=\"car_dealer_item car_dealer_item_last\">");
						}
						else
						{
							strlist.Add("<div class=\"car_dealer_item\">");
						}
						strlist.Add("<dl>");
						strlist.Add("<dt><a href=\"http://dealer.bitauto.com/" + vendorID + "/\" target=\"_blank\"><span>" + (vendorBizMo == "2" ? "[4S店]" : "") + "</span> " + vendorName + "</a></dt>");
						strlist.Add("<dd><label>电话：</label>" + dealerTel + "</dd>");
						strlist.Add("<dd><label>地址：</label><span>" + vendorSaleAddr + " [<a <a onclick=\"window.open('http://dealer.bitauto.com/VendorMap/GoogleMap.aspx?dID=" + vendorID + "&S=S&W=400&H=300&Z=12','mapwindow','height=300,width=400,top=90,left=100,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no');return false;\" href=\"#\">查看地图</a>]</span></dd>");
						strlist.Add("</dl>");
						strlist.Add("<ul class=\"car_price\">");
						strlist.Add("<li><a href=\"" + dealerUrl + "\" target=\"_blank\">" + vendorPrice.ToString("G0") + "万" + "</a></li>");
						strlist.Add("<li>预估购车总价：<strong>" + totalPrice.ToString("G0") + "万" + "</strong></li>");
						strlist.Add("</ul>");
						strlist.Add("<a href=\"http://dealer.bitauto.com/" + vendorID + "/price_detail/" + carID.ToString() + ".html?type=open\" target=\"_blank\" class=\"btn_price\">询价&gt;&gt;</a>");
						// add by chengl Dec.13.2011
						strlist.Add("<a href=\"http://dealer.bitauto.com/" + vendorID + "/price_detail/" + carID.ToString() + ".html?type=open&order=1\" target=\"_blank\" class=\"btn_price\">试驾&gt;&gt;</a>");
						strlist.Add("</div>");
					}

				}
			}
			string dealerHtml = String.Concat(strlist.ToArray());
			return dealerHtml;
		}
        /// <summary>
        /// 从mongodb里面获取车型报价
        /// </summary>
        /// <returns>json格式</returns>
        private string GetCarPriceJsonFromMongoDB()
        {
            List<string> strlist = new List<string>();
            if (carID > 0 && cityID > 0)
            {
                List<CarChannel.Model.DealerInfo> dealerlist = new Car_BasicBll().GetCarAllCityDealserFromMongoDB(carID);
                List<CarChannel.Model.DealerInfo> newlist = dealerlist.FindAll(Match);
                newlist.Sort(Comparer);
                if (newlist.Count > 0)
                {
                    strlist.Add("[");
                    foreach (CarChannel.Model.DealerInfo dealer in newlist)
                    {
                        string vendorName = dealer.VendorName.ToString().Trim();
                        string vendorID = dealer.DealerId.ToString().Trim();
                        string vendorBizMo = dealer.DealerType.ToString().Trim();
                        string vendorSaleAddr = dealer.Address != null ? dealer.Address.ToString().Trim() : "";
                        string dealerUrl = dealer.DealerUrl != null ? dealer.DealerUrl.ToString().Trim() : "";
                        string dealerTel = base.GetDealerFor400(vendorID);
                        if (dealerTel == "")
                        { dealerTel = dealer.PhoneNumber.ToString().Trim().Replace("$", "/"); }
                        decimal vendorPrice = (dealer.SalePrice.ToString() != "" ? Math.Round(decimal.Parse(dealer.SalePrice.ToString()), 2) : 0);
                        decimal totalPrice = (dealer.EvaluatePrice.ToString() != "" ? Math.Round(decimal.Parse(dealer.EvaluatePrice.ToString()), 2) : 0);

                        strlist.Add(
                            string.Concat("{"
                                , string.Format("id:\"{0}\",name:\"{1}\", type:\"{2}\", addr:\"{3}\", url:\"{4}\", tel:\"{5}\", p:\"{6}\", tp:\"{7}\""
                                    ,vendorID, vendorName, vendorBizMo, vendorSaleAddr, dealerUrl, dealerTel, vendorPrice, totalPrice)
                            , "}")
                        );
                        strlist.Add(",");
                    }
                    strlist[strlist.Count - 1] = "]";
                }
            }
            string dealerHtml = String.Concat(strlist.ToArray());
            return dealerHtml;
        }
        /// <summary>
        /// 从mongodb里面获取车型报价
        /// </summary>
        /// <returns>xml</returns>
        private string GetCarPriceXMLFromMongoDB()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("root");
            doc.AppendChild(root);
            if (carID > 0 && cityID > 0)
            {
                List<CarChannel.Model.DealerInfo> dealerlist = new Car_BasicBll().GetCarAllCityDealserFromMongoDB(carID);
                List<CarChannel.Model.DealerInfo> newlist = dealerlist.FindAll(Match);
                newlist.Sort(Comparer);
                if (newlist.Count > 0)
                {
                    foreach (CarChannel.Model.DealerInfo dealer in newlist)
                    {
                        string vendorName = dealer.VendorName.ToString().Trim();
                        string vendorID = dealer.DealerId.ToString().Trim();
                        string vendorBizMo = dealer.DealerType.ToString().Trim();
                        string vendorSaleAddr = dealer.Address != null ? dealer.Address.ToString().Trim() : "";
                        string dealerUrl = dealer.DealerUrl != null ? dealer.DealerUrl.ToString().Trim() : "";
                        string dealerTel = base.GetDealerFor400(vendorID);
                        if (dealerTel == "")
                        { dealerTel = dealer.PhoneNumber.ToString().Trim().Replace("$", "/"); }
                        decimal vendorPrice = (dealer.SalePrice.ToString() != "" ? Math.Round(decimal.Parse(dealer.SalePrice.ToString()), 2) : 0);
                        decimal totalPrice = (dealer.EvaluatePrice.ToString() != "" ? Math.Round(decimal.Parse(dealer.EvaluatePrice.ToString()), 2) : 0);
                        XmlElement dealerEle = doc.CreateElement("Dealer");
                        dealerEle.SetAttribute("id", vendorID);
                        dealerEle.SetAttribute("name", vendorName);
                        dealerEle.SetAttribute("type", vendorBizMo);
                        dealerEle.SetAttribute("addr", vendorSaleAddr);
                        dealerEle.SetAttribute("url", dealerUrl);
                        dealerEle.SetAttribute("tel", dealerTel);
                        dealerEle.SetAttribute("p", vendorPrice.ToString());
                        dealerEle.SetAttribute("tp", totalPrice.ToString());
                        root.AppendChild(dealerEle);
                    }
                }
            }
            return doc.OuterXml;
        }
		/// <summary>
		/// 排序委托方法
		/// </summary>
		/// <param name="dealer1"></param>
		/// <param name="dealer2"></param>
		/// <returns></returns>
		public int Comparer(CarChannel.Model.DealerInfo dealer1, CarChannel.Model.DealerInfo dealer2)
		{
			if (dealer1.OrderWeight < dealer2.OrderWeight)
				return 1;
			else if (dealer1.OrderWeight == dealer2.OrderWeight)
				return 0;
			else if (dealer1.OrderWeight > dealer2.OrderWeight)
				return -1;
			else return 0;
		}
		/// <summary>
		/// 筛选委托方法
		/// </summary>
		/// <param name="dealer"></param>
		/// <returns></returns>
		public bool Match(CarChannel.Model.DealerInfo dealer)
		{
			if (dealer.CityId == cityID)
				return true;
			return false;
		}

	}
}