using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;

using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.CarTreeV2
{
	public partial class CX_serial : TreePageBase
	{
		private SerialGoodsBll _serialGoodsBLL;
		private Car_SerialBll _serialBLL;
		private Car_BasicBll _carBLL; 

		private List<SerialGoodsCarEntity> serialGoodsCarList;//易车惠 商品 车型列表
		private Dictionary<int, string> dictUCarPrice;//二手车价格

		protected int _SerialId;
		protected string _SerialSpell = string.Empty;
		protected string _SerialSeoName = string.Empty;
		protected string _BrandName = string.Empty;
		protected string _SerialName = string.Empty;
		protected string _MetaKeyWordArea = string.Empty;
		protected string _GuilderString = string.Empty;
		protected string _EncodeName;
		private string _MasterBrandTopUrl = string.Empty;
		private string _BrandToUrl = string.Empty;
        protected SerialEntity _serialEntity;
		private int maxPv = 0;
		private List<EnumCollection.CarInfoForSerialSummary> ls = new List<EnumCollection.CarInfoForSerialSummary>();

		protected string serialExhaust;		//排量列表
		protected string serialTransmission = string.Empty;	//变速箱列表

		protected string carListHtml;
		protected string serialDescribeHtml;

		protected string NavPathHtml = "";

		//add by 2014.05.06
		protected EnumCollection.SerialInfoCard sic;
		//protected SerialEntity serialEntity;
		protected string serialImageUrl = string.Empty;
		protected string serialReferPrice = string.Empty;		//指导价
		protected string baaUrl = string.Empty;
		protected string serialUCarPrice = string.Empty;
		protected bool isElectrombile = false;

		protected string carListTableHtml = string.Empty;
        protected string serialSaleDisplacement = "暂无";//在销,停销
		protected string serialSaleDisplacementalt = string.Empty; //在销,停销
		protected string chargeTimeRange = string.Empty;
		protected string fastChargeTimeRange = string.Empty;
		protected string mileageRange = string.Empty;
		//protected string shijiaOrHuimaiche = string.Empty;


		public CX_serial()
		{
			_serialBLL = new Car_SerialBll();
			_carBLL = new Car_BasicBll();
			_serialGoodsBLL = new SerialGoodsBll();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			GetParam();
			InitData();
			//NavbarHtml = base.GetTreeNavBarHtml("serial", "chexing", _SerialId);
			//搜索地址
			this._SearchUrl = InitSearchUrl("chexing");

			MakeCarListHtmlNew();
			//carListHtml = GetCarTypeList();
			//serialDescribeHtml = GetSerialDescribe();
			//生成条件Html
			//this.MakeConditionsHtml("按条件选车", false, true);
		}
		/// <summary>
		/// 得到参数页面
		/// </summary>
		private void GetParam()
		{
			_SerialId = string.IsNullOrEmpty(Request.QueryString["id"])
				? 0 : ConvertHelper.GetInteger(Request.QueryString["id"].ToString());
		}
		/// <summary>
		/// 初始化数据
		/// </summary>
		private void InitData()
		{
			//InitSourceUrl();
			InitSerial();
			GetNavbarHtml();
			ls = base.GetAllCarInfoForSerialSummaryByCsID(_SerialId);
			ls.Sort(NodeCompare.CompareCarByExhaust);
			//易车惠 商品 车型列表
			serialGoodsCarList = _serialGoodsBLL.GetGoodsCarList(_SerialId);
			//GetPageGuilder();
		}
		//面包屑
		private void GetNavbarHtml()
		{
			StringBuilder sbNavbar = new StringBuilder();
			Car_BrandBll brand = new Car_BrandBll();
			//string masterSpell = "";
			//string masterName = "";
			string brandHtml = string.Empty;
			//int masterId = brand.GetMasterbrandByBrand(_serialEntity.BrandId, out masterSpell, out masterName);
			MasterBrandEntity master = _serialEntity.Brand.MasterBrand;//(MasterBrandEntity)DataManager.GetDataEntity(EntityType.MasterBrand, masterId);
			string brandName = (_serialEntity.Brand.Country != "中国" ? "进口" : "") + _serialEntity.Brand.Name;
            sbNavbar.Append("<div class=\"crumbs h-line\">");
			if (!string.Equals(master.Name, brandName.Replace("进口", "")) || master.BrandList.Length > 1)//主品牌只有一个品牌 并且 主品牌名称不等于品牌名称
			{
				brandHtml = string.Format(" &gt; <a href=\"/tree_chexing/b_{0}/\">{1}</a>", _serialEntity.BrandId, brandName);
			}
			sbNavbar.AppendFormat("<div class=\"crumbs-txt\"><span>当前位置：</span><a href=\"http://www.bitauto.com/\">易车</a> &gt; <a href=\"/\">车型</a> &gt; <a href=\"/tree_chexing/mb_{0}/\">{1}</a>{2} &gt; <strong>{3}</strong></div>"
				, master.Id
				, master.Name
				, brandHtml
				, _SerialName);
			sbNavbar.Append("</div>");
			NavPathHtml = sbNavbar.ToString();
		}
		/// <summary>
		/// 初始化主品牌页面
		/// </summary>
		private void InitSerial()
		{
			_serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, _SerialId);
			if (_serialEntity == null || _serialEntity.Id == 0)
			{
				// Response.Redirect("/tree_chexing/error.html", true);
				Response.Redirect("/404error.aspx");
			}
			// _SerialName = _serialEntity.Name;
			// modified by chengl Mar.15.2013
			_SerialName = _serialEntity.ShowName;
			if (_SerialId == 1568)
				_SerialName = "索纳塔八";
			_SerialSeoName = _serialEntity.SeoName;
			_SerialSpell = _serialEntity.AllSpell;
			_EncodeName = Server.UrlEncode(_serialEntity.ShowName);

			sic = new Car_SerialBll().GetSerialInfoCard(_SerialId);	//子品牌名片
			//serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, _SerialId);
			serialImageUrl = Car_SerialBll.GetSerialImageUrl(_SerialId).Replace("_2.", "_6.");

			//论坛url
			baaUrl = _serialBLL.GetForumUrlBySerialId(_SerialId);
			//二手车价格区间
			dictUCarPrice = _serialBLL.GetUCarSerialPrice();
			serialUCarPrice = dictUCarPrice.ContainsKey(_SerialId) ? dictUCarPrice[_SerialId] : "暂无";
			//变速箱
			var arrTrans = sic.CsTransmissionType.Split('、');
			serialTransmission = arrTrans.Length > 0 ? string.Join(" ", arrTrans) : "暂无";
			//惠买车 低价
			//Dictionary<int, string> dicHuiMaiChe = _serialBLL.GetEPHuiMaiCheAllCsUrl();
			//if (dicHuiMaiChe != null && dicHuiMaiChe.ContainsKey(_SerialId))
			//{
			//	shijiaOrHuimaiche = string.Format("<a class=\"btn\" href=\"{0}?tracker_u=77_cxsxcx&leads_source=p015002\" data-channelid=\"2.22.109\" target=\"_blank\">买新车</a>", dicHuiMaiChe[_SerialId]);
			//}
		}
		/// <summary>
		/// 得到子品牌描述
		/// </summary>
		/// <returns></returns>
		protected string GetSerialDescribe()
		{

			List<string> overviewHtmlList = new List<string>();
			/*
			 * 参数0:子品牌全拼
			 * 参数1:品牌名,
			 * 参数2:子品牌名
			 * 参数3:子品牌ID
			 */
			overviewHtmlList.Add(string.Format(" <h3><span><a id=\"n{3}\" stattype=\"channel\" target=\"_blank\" href=\"/{0}/\">"
												+ "{1}&nbsp;{2}</a></span></h3>"
									, _SerialSpell
									, _serialEntity.Brand.Name
									, _SerialName
									, _SerialId.ToString()));
			overviewHtmlList.Add("<dl class=\"c0624_06\">");

			/*
			 * 参数0:子品牌全拼
			 * 参数1:子品牌SEO名
			 * 参数2:图片链接
			 * 参数3:子品牌ID
			 */
			//子品牌图片
			overviewHtmlList.Add(string.Format("<dt><a id=\"n{3}\" stattype=\"channel\" target=\"_blank\" href=\"/{0}/\">"
									+ "<img alt=\"{1}\" src=\"{2}\" height=\"80\" width=\"120\"></a></dt>"
									, _serialEntity.AllSpell
									, _serialEntity.SeoName
									, serialImageUrl
									, _SerialId.ToString()));
			//子品牌描述
			overviewHtmlList.Add("<dd class=\"l\"><ul>");
			overviewHtmlList.Add(string.Format("<li><label>参考成交价：</label><strong><a href=\"http://price.bitauto.com/brand.aspx?newbrandId=" + _SerialId + "\" target=\"_blank\">{0}</a></strong></li>"
								  , string.IsNullOrEmpty(sic.CsPriceRange) ? "暂无报价" : sic.CsPriceRange));
			overviewHtmlList.Add(string.Format("<li><label>厂家指导价：</label>{0}</li>", serialReferPrice));
			overviewHtmlList.Add(string.Format("<li><label>排量：</label>{0}</li>", serialExhaust));
			overviewHtmlList.Add(string.Format("<li><label>变速箱：</label>{0}</li>", serialTransmission));

			overviewHtmlList.Add("</ul></dd>");


			//子品牌外链
			/*
			* 参数0:子品牌全拼
			* 参数1:品牌名,
			* 参数2:子品牌名
			* 参数3:子品牌ID
			*/
			overviewHtmlList.Add(string.Format("<dd class=\"b\"><div class=\"go\"><em><a id=\"n{3}\" stattype=\"channel\" target=\"_blank\" href=\"/{0}/\">详情查看 <span>"
								   + "<strong>{1}{2} </strong>频道</span> &gt;&gt;</a></em></div>"
									, _SerialSpell
									, ""
									, _SerialName
									, _SerialId.ToString()));

			// modified by chengl May.25.2012
			overviewHtmlList.Add(string.Format("<div class=\"lnk\"><a class=\"more2new\"  target=\"_blank\" href=\"http://www.taoche.com/buycar/serial/{1}/\">买二手车</a>"
				// + "<a id=\"n{0}\" stattype=\"carsale\" href=\"http://i.bitauto.com/baaadmin/car/goumai_{0}/\" target=\"_blank\">计划购买</a>"
									+ "<a id=\"n{0}\" id=\"LinkForBaaAttention\" stattype=\"carsee\" href=\"http://i.bitauto.com/baaadmin/car/guanzhu_{0}/\" target=\"_blank\">加关注</a>"
				//20130412 edit anh
				//// modified by chengl Dec.13.2011
				//// + "<a href=\"http://ask.bitauto.com/browse/{0}/\" target=\"_blank\">买前咨询</a></div>"
									+ "<a href=\"http://dealer.bitauto.com/shijia/nb{0}/\" target=\"_blank\">预约试驾</a></div>"
									, _SerialId.ToString(), _SerialSpell));
			overviewHtmlList.Add("</dd>");

			overviewHtmlList.Add("</dl>");

			return String.Concat(overviewHtmlList.ToArray());
		}
		/*
		/// <summary>
		/// 得到车型列表
		/// </summary>
		/// <returns></returns>
		protected string GetCarTypeList()
		{
			List<string> carsHtmlList = new List<string>();
			//获取数据
			List<EnumCollection.CarInfoForSerialSummary> ls = base.GetAllCarInfoForSerialSummaryByCsID(_SerialId);
			ls.Sort(NodeCompare.CompareCarByExhaust);
			//排量列表
			List<string> exhaustList = new List<string>();
			//变速箱列表
			List<string> transList = new List<string>();
			//年款列表
			List<string> yearList = new List<string>();
			Dictionary<string, string> yearHtmlDic = new Dictionary<string, string>();
			int maxPv = 0;
			double maxPrice = Double.MinValue;
			double minPrice = Double.MaxValue;
			foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
			{
				if (carInfo.CarPV > maxPv)
					maxPv = carInfo.CarPV;
				if (carInfo.CarYear.Length > 0)
				{
					string yearType = carInfo.CarYear + "款";
					if (!yearList.Contains(yearType))
						yearList.Add(yearType);
				}
				double referPrice = 0.0;
				bool isDouble = Double.TryParse(carInfo.ReferPrice.Replace("万", ""), out referPrice);
				if (isDouble)
				{
					if (referPrice > maxPrice)
						maxPrice = referPrice;
					if (referPrice < minPrice)
						minPrice = referPrice;
				}
			}

			yearList.Sort(NodeCompare.CompareStringDesc);


			carsHtmlList.Add("<h3><span>" + _SerialSeoName + "-在售车款</span><em class=\"h3sublink\">");

			carsHtmlList.Add("</h3>");

			carsHtmlList.Add("<div class=\"comparetable\">");

			carsHtmlList.Add("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"compare2\" id=\"compare\">");

			List<string> exhaustHtmlList = new List<string>();
			string carIDs = string.Empty;

			foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
			{
				if (carInfo.CarPV > maxPv)
					maxPv = carInfo.CarPV;
			}
			int index = 0;
			BitAuto.CarChannel.BLL.Car_BasicBll basicBll = new BitAuto.CarChannel.BLL.Car_BasicBll();

			foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
			{
				if (!exhaustList.Contains(carInfo.Engine_Exhaust))
				{
					if (carIDs != "")
					{
						carsHtmlList.Add(string.Format(String.Concat(exhaustHtmlList.ToArray()), carIDs));
						exhaustHtmlList.Clear();
					}
					exhaustList.Add(carInfo.Engine_Exhaust);
					if (index < 1)
					{
						exhaustHtmlList.Add("<tr style=\"\"><th width=\"255px\" class=\"firstItem\"><b>" + carInfo.Engine_Exhaust + "排量</b></th>");
						exhaustHtmlList.Add("<th width='50px'>热度</th>");
						exhaustHtmlList.Add("<th width='80px'>变速箱</th>");
						exhaustHtmlList.Add("<th width='85px'>厂家指导价</th>");
						exhaustHtmlList.Add("<th width='180px'>参考成交价</th>");
						exhaustHtmlList.Add("</tr>");
					}
					else
					{
						exhaustHtmlList.Add("<tr style=\"\"><th width=\"255px\" colspan=\"5\" class=\"firstItem\"><b>" + carInfo.Engine_Exhaust + "排量</b></th></tr>");
					}
					index++;
				}
				if (carIDs != "") { carIDs += "," + carInfo.CarID; }
				else { carIDs += carInfo.CarID; }

				//车型链接
				string carUrl = string.Format("/{0}/m{1}/"
											, _SerialSpell
											, carInfo.CarID.ToString());
				//年款
				string yearType = carInfo.CarYear.Trim();
				//赋值各参数
				if (yearType.Length > 0)
					yearType += "款";
				else
					yearType = "未知年款";

				//车型全称
				string carFullName = string.Empty;
				//车型列表 2013年款车型  “新胜达”名称调整为“全新胜达” by sk 2013.02.04
				if (yearType == "2013款" && _SerialId == 1848)
				{
					carFullName = "全新胜达&nbsp;" + carInfo.CarName;
				}
				// modified by chengl Mar.15.2013 for gaoyan
				else if (yearType == "2012款" && _SerialId == 1785)
				{
					carFullName = "奇瑞QQ3&nbsp;" + carInfo.CarName;
				}
				else
				{
					carFullName = _SerialName + "&nbsp;" + carInfo.CarName;
				}
				//计算百分比
				int percent = (int)Math.Round((double)carInfo.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero);
				//变速器类型
				string tempTransmission = carInfo.TransmissionType;

				//停产车型样式
				string stopPrd = "";

				// 节能补贴 Sep.2.2010
				string hasEnergySubsidy = "";
				bool isHasEnergySubsidy = new Car_BasicBll().CarHasParamEx(carInfo.CarID, 853);
				if (isHasEnergySubsidy)
				{
					hasEnergySubsidy = " <span class=\"butie\"><a href=\"http://news.bitauto.com/zcxwtt/20130924/1406235633.html\" title=\"可获得3000元节能补贴\" target=\"_blank\">补贴</a></span>";
				}
				//============2012-04-09 减税============================
				Dictionary<int, string> dict = basicBll.GetCarAllParamByCarID(carInfo.CarID);
				string strTravelTax = "";
				if (dict.ContainsKey(895))
				{
					strTravelTax = " <span class=\"jianshui\"><a target=\"_blank\" title=\"{0}\" href=\"http://news.bitauto.com/others/20120308/0805618954.html\">减税</a></span>";
					if (dict[895] == "减半")
						strTravelTax = string.Format(strTravelTax, "减征50%车船使用税");
					else if (dict[895] == "免征")
						strTravelTax = string.Format(strTravelTax, "免征车船使用税");
					else
						strTravelTax = "";
				}
				//易车惠
				string strYiCheHui = "";
				var carGoods = serialGoodsCarList.Find(p => p.CarId == carInfo.CarID);
				if (carGoods != null)
				{
					var goodsUrl = carGoods.GoodsUrl.Replace("/detail", "/all/detail") + "?WT.mc_id=car4";
					strYiCheHui = string.Format("<a target=\"_blank\" title=\"{0}\" href=\"{1}\" class=\"ico-jianshui\">特惠</a>", "", goodsUrl);
				}

				//指导价
				string carReferPrice = "<td style=\"text-align:right\"><a title=\"购车费用计算\" target=\"_blank\" "
									+ "href=\"/gouchejisuanqi/?carid={0}\""
									+ ">{1}万</a><a title=\"购车费用计算\" target=\"_blank\" class=\"icon_cal\" "
									+ "href=\"/gouchejisuanqi/?carid={0}\"></a></td>";
				//20130412 edit anh
				////报价
				string carPriceRange = "<td style=\"text-align:right\"><span><a href=\"/{0}/m{1}/baojia/\" target=\"_blank\">"
									 + "{2}</a></span> <a href=\"http://dealer.bitauto.com/zuidijia/nb{3}/nc{1}/\" target=\"_blank\" class=\"cGray\">询价&gt;&gt;</a></td>";

				if (carInfo.CarName.StartsWith(_serialEntity.ShowName))
				{
					//车型列表 2013年款车型  “新胜达”名称调整为“全新胜达” by sk 2013.02.04
					if (yearType == "2013款" && _SerialId == 1848)
					{
						carFullName = "全新胜达&nbsp;" + carInfo.CarName.Substring(_serialEntity.ShowName.Length);
					}
					else
					{
						carFullName = _serialEntity.ShowName
									+ "&nbsp;" + carInfo.CarName.Substring(_serialEntity.ShowName.Length);
					}
				}
				if (yearType != "未知年款")
					carFullName = yearType + " " + carFullName;

				if (carInfo.ProduceState == "停产") stopPrd = " <span class=\"tc\">停产</span>";
				if (tempTransmission.IndexOf("挡") >= 0)
				{
					tempTransmission = tempTransmission.Substring(tempTransmission.IndexOf("挡") + 1, tempTransmission.Length - tempTransmission.IndexOf("挡") - 1);
				}
				tempTransmission = tempTransmission.Replace("变速器", "");
				carReferPrice = string.Format(carReferPrice, carInfo.CarID.ToString(), carInfo.ReferPrice);
				if (carInfo.ReferPrice.Trim().Length == 0) carReferPrice = "<td style=\"text-align:right\">暂无</td>";
				carPriceRange = string.Format(carPriceRange, _SerialSpell, carInfo.CarID.ToString(), carInfo.CarPriceRange, _SerialId);
				if (carInfo.CarPriceRange.Trim().Length == 0) carPriceRange = "<td style=\"text-align:center\"><span style=\"color:gray\">暂无报价</span></td>";


				exhaustHtmlList.Add(string.Format("<tr><td><div class=\"pdL10\"><a id=\"n{3}\" stattype=\"cartype\" href=\"{0}\" target=\"_blank\">{1}</a>{2}</div></td>", carUrl, carFullName, stopPrd + strTravelTax + hasEnergySubsidy + strYiCheHui, carInfo.CarID.ToString()));
				exhaustHtmlList.Add(string.Format("<td><div class=\"w\"><div class=\"p\"  style=\"width:{0}%\"></div></div></td>", percent.ToString()));
				exhaustHtmlList.Add("<td>" + tempTransmission + "</td>");
				exhaustHtmlList.Add(carReferPrice);
				exhaustHtmlList.Add(carPriceRange);

				if (transList.Count < 2)
				{
					if (tempTransmission.IndexOf("手动") == -1)
						tempTransmission = "自动";
					if (!transList.Contains(tempTransmission))
						transList.Add(tempTransmission);
				}


				//生成在销的排量与变速箱列表
				if (exhaustList.Count > 5)
					serialExhaust = String.Join("　", new string[] { exhaustList[0], exhaustList[1], exhaustList[2] + "…" + exhaustList[exhaustList.Count - 1] });
				else
					serialExhaust = String.Join("　", exhaustList.ToArray());
				if (transList.Count > 3)
					serialTransmission = transList[0] + "　" + transList[1] + "…" + transList[transList.Count - 1];
				else
					serialTransmission = String.Join("　", transList.ToArray());

			}
			if (carIDs != "")
			{
				carsHtmlList.Add(string.Format(String.Concat(exhaustHtmlList.ToArray()), carIDs));
			}
			else
				carsHtmlList.Add("<tr><td class=\"noline\" colspan=\"7\">暂无在销车型！</td></tr>");

			carsHtmlList.Add("</tbody></table>");
			carsHtmlList.Add("</div>");
			return String.Concat(carsHtmlList.ToArray());
		}
*/

		#region 新版 车型列表 add by sk 2013.08.05
		/// <summary>
		/// 子品牌 车款列表 html
		/// </summary>
		/// <param name="serialId">子品牌ID</param>
		private void MakeCarListHtmlNew()
		{
			StringBuilder sb = new StringBuilder();
			List<string> carSaleListHtml = new List<string>();
			List<string> carNoSaleListHtml = new List<string>();
			List<string> carWaitSaleListHtml = new List<string>();

			List<CarInfoForSerialSummaryEntity> carinfoList = _carBLL.GetCarInfoForSerialSummaryBySerialId(_SerialId);
			int maxPv = 0;
			List<string> saleYearList = new List<string>();
			List<string> noSaleYearList = new List<string>();
			foreach (CarInfoForSerialSummaryEntity carInfo in carinfoList)
			{
				if (carInfo.CarPV > maxPv)
					maxPv = carInfo.CarPV;
				if (carInfo.CarYear.Length > 0)
				{
					string yearType = carInfo.CarYear + "款";

					if (carInfo.SaleState == "停销")
					{
						if (!noSaleYearList.Contains(yearType))
							noSaleYearList.Add(yearType);
					}
					else
					{
						if (!saleYearList.Contains(yearType))
							saleYearList.Add(yearType);
					}
				}
			}
			//排除包含在售年款
			foreach (string year in saleYearList)
			{
				if (noSaleYearList.Contains(year))
				{
					noSaleYearList.Remove(year);
				}
			}
			List<CarInfoForSerialSummaryEntity> carinfoSaleList = carinfoList
				.FindAll(p => p.SaleState == "在销");
			List<CarInfoForSerialSummaryEntity> carinfoWaitSaleList = carinfoList
				.FindAll(p => p.SaleState == "待销");
            List<CarInfoForSerialSummaryEntity> carinfoNoSaleList = carinfoList
                .FindAll(p => p.SaleState == "停销");


			//add by 2014.05.04 在销车款 电动车
			var fuelTypeList = carinfoSaleList.Where(p => p.Oil_FuelType != "")
											  .GroupBy(p => p.Oil_FuelType)
											  .Select(g => g.Key).ToList();
			isElectrombile = fuelTypeList.Count == 1 && fuelTypeList[0] == "电力" ? true : false;
			//add by 2014.03.18 在销车款 排量输出
 			var exhaustList = carinfoSaleList.Where(p => p.Engine_Exhaust.EndsWith("L"))
				.Select(p => p.Engine_InhaleType == "增压" ? p.Engine_Exhaust.Replace("L", "T") : p.Engine_Exhaust)
											.GroupBy(p => p)
											.Select(group => group.Key).ToList();
            //停销车款 排量输出
            var maxYear = carinfoNoSaleList.Max(s => s.CarYear);
            var tempList = carinfoNoSaleList.Where(s => s.CarYear == maxYear).ToList();

            List<string> noSaleExhaustList = tempList.Where(p => p.Engine_Exhaust.EndsWith("L"))
                                                              .Select(
                                                                  p =>
																  p.Engine_InhaleType == "增压"
                                                                      ? p.Engine_Exhaust.Replace("L", "T")
                                                                      : p.Engine_Exhaust)
                                                              .GroupBy(p => p)
                                                              .Select(group => group.Key).ToList();

            List<string> fuelTypeListForNoSeal = tempList.Where(p => p.Oil_FuelType != "")
                                                                  .GroupBy(p => p.Oil_FuelType)
                                                                  .Select(g => g.Key).ToList();
            if (_serialEntity.SaleState.Trim() == "停销")
            {
                if (noSaleExhaustList.Count > 0)
                {
                    noSaleExhaustList.Sort(NodeCompare.ExhaustCompareNew);
                    if (noSaleExhaustList.Count > 3)
                    {
                        serialSaleDisplacement = string.Concat(noSaleExhaustList[0], " ", noSaleExhaustList[1]
                                                                 , "..."
                                                                 , noSaleExhaustList[noSaleExhaustList.Count - 1],
                                                                 fuelTypeListForNoSeal.Contains("电力") ? " 电动" : "");
                    }
                    else
                        serialSaleDisplacement = string.Join(" ", noSaleExhaustList.ToArray()) +
                                                   (fuelTypeListForNoSeal.Contains("电力") ? " 电动" : "");
                    serialSaleDisplacementalt = string.Join(" ", noSaleExhaustList.ToArray()) +
                                                  (fuelTypeListForNoSeal.Contains("电力") ? " 电动" : "");
                }
            }
            else
            {
                //在销车款 排量输出
                if (exhaustList.Count > 0)
                {
                    exhaustList.Sort(NodeCompare.ExhaustCompareNew);
                    if (exhaustList.Count > 3)
                    {
                        serialSaleDisplacement = string.Concat(exhaustList[0], " ", exhaustList[1]
                            , "..."
                            , exhaustList[exhaustList.Count - 1], fuelTypeList.Contains("电力") ? " 电动" : "");
                    }
                    else
                        serialSaleDisplacement = string.Join(" ", exhaustList.ToArray()) + (fuelTypeList.Contains("电力") ? " 电动" : "");
                    serialSaleDisplacementalt = string.Join(" ", exhaustList.ToArray());
                }
            }
            if (carinfoSaleList.Count <= 0) return;
			carinfoSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
			carinfoWaitSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);

			noSaleYearList.Sort(NodeCompare.CompareStringDesc);

			sb.Append("<div class=\"onsale-car-box\">");
            sb.Append("<div class=\"section-header header2\">");
			sb.Append("<div class=\"box\">");
            sb.Append("<ul class=\"nav\">");
			sb.AppendFormat("<li class=\"current\"><a href=\"javascript:;\">在售车款</a></li>");
            if (noSaleYearList.Count > 0)
            {
                sb.Append("<li id=\"pop_nosale\"><a href=\"javascript:;\" class=\"arrow-down\">停售车款</a>");
                sb.Append("<div id=\"pop_nosalelist\" class=\"drop-layer\" style=\"display:none;\">");
                for (int i = 0; i < noSaleYearList.Count; i++)
                {
                    string url = string.Format("/{0}/{1}/#car_list", _SerialSpell, noSaleYearList[i].Replace("款", ""));
                    sb.AppendFormat("<a href=\"{0}\">{1}</a>", url, noSaleYearList[i]);  
                }
                sb.Append("</div>");
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("</div>");
			if (carinfoSaleList.Count > 0)
			{
				sb.Append("    <div class=\"list-table\" id=\"data_tab_jq5_0\" style=\"display: block;\">");
				sb.Append("        <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" id=\"compare_sale\">");
                sb.Append("            <colgroup><col width=\"39%\"><col width=\"11%\"><col width=\"11%\"><col width=\"10%\"><col width=\"11%\"><col width=\"18%\"></colgroup>");
				sb.Append("            <tbody>");
				sb.Append(GetCarListHtml(carinfoSaleList, maxPv));
				sb.Append("            </tbody>");
				sb.Append("        </table>");
				sb.Append("    </div>");
			}
			sb.Append("</div>");
			carListTableHtml = sb.ToString();
		}
		/// <summary>
		/// 车型列表html
		/// </summary>
		/// <param name="carList">车款列表 list</param>
		/// <param name="serialInfo">子品牌信息</param>
		/// <param name="maxPv">最大pv</param>
		/// <returns></returns>
		private string GetCarListHtml(List<CarInfoForSerialSummaryEntity> carList, int maxPv)
		{
			List<string> carListHtml = new List<string>();
			//if (carList.Count == 0)
			//    carListHtml.Add("<tr>暂无车款！</tr>");
			var querySale = carList.GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower }, p => p);
			int groupIndex = 0;

			int minChargeTime = 0;
			int maxChargeTime = 0;
			int minFastChargeTime = 0;
			int maxFastChargeTime = 0;
			int minMileage = 0;
			int maxMileage = 0;
			foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in querySale)
			{
				var key = CommonFunction.Cast(info.Key, new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0, Electric_Peakpower = 0 });
				string strMaxPowerAndInhaleType = string.Empty;
				string maxPower = key.Engine_MaxPower == 9999 ? "" : key.Engine_MaxPower + "kW";
				string inhaleType = key.Engine_InhaleType;
				if (!string.IsNullOrEmpty(maxPower) || !string.IsNullOrEmpty(inhaleType))
				{
					if (inhaleType == "增压")
					{
						inhaleType = string.IsNullOrEmpty(key.Engine_AddPressType) ? inhaleType : key.Engine_AddPressType;
					}
					if (key.Electric_Peakpower > 0)
					{
						maxPower = string.Format("发动机：{0}，发电机：{1}", maxPower, key.Electric_Peakpower + "kW");
					}
					strMaxPowerAndInhaleType = string.Format("{0}{1}", maxPower, " " + inhaleType);
				}

				//if (groupIndex == 0)
				//{
				carListHtml.Add("<tr class=\"table-tit\">");
				carListHtml.Add("    <th class=\"first-item\">");
                carListHtml.Add(string.Format("<strong>{0}</strong><b>/</b>{1}",//key.Engine_Exhaust.Replace("L", "升"),
					key.Engine_Exhaust,strMaxPowerAndInhaleType));
				carListHtml.Add("    </th>");
				carListHtml.Add("    <th>关注度</th>");
				carListHtml.Add("    <th>变速箱</th>");
                carListHtml.Add("    <th class=\"txt-right txt-right-padding\">指导价</th>");
                carListHtml.Add("    <th class=\"txt-right\">参考最低价</th>");
                carListHtml.Add("    <th><div class=\"doubt\" onmouseover=\"javascript:$(this).children('.prompt-layer').show();return false;\" onmouseout=\"javascript:$(this).children('.prompt-layer').hide();return false;\">");
                carListHtml.Add("    <div class=\"prompt-layer\" style=\"display:none\">全国参考最低价</div></div></th>");
				carListHtml.Add("</tr>");
				//}
				//else
				//{
				//    carListHtml.Add("<tr class=\"\">");
				//    carListHtml.Add("    <th width=\"44%\" class=\"first-item\">");
				//    carListHtml.Add(string.Format("        <div class=\"pdL10\"><strong>{0}</strong> {1}</div>",
				//        key.Engine_Exhaust,
				//        strMaxPowerAndInhaleType));
				//    carListHtml.Add("    </th>");
				//    carListHtml.Add("    <th width=\"8%\" class=\"pd-left-one\"></th>");
				//    carListHtml.Add("    <th width=\"10%\" class=\"pd-left-one\"></th>");
				//    carListHtml.Add("    <th width=\"10%\" class=\"pd-left-two\"></th>");
				//    carListHtml.Add("    <th width=\"10%\" class=\"pd-left-three\"></th>");
				//    carListHtml.Add("    <th width=\"18%\">&nbsp;</th>");
				//    carListHtml.Add("</tr>");
				//}
				groupIndex++;
				List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList<CarInfoForSerialSummaryEntity>();//分组后的集合

				foreach (CarInfoForSerialSummaryEntity entity in carGroupList)
				{
					string yearType = entity.CarYear.Trim();
					if (yearType.Length > 0)
						yearType += "款";
					else
						yearType = "未知年款";
					string stopPrd = "";
					if (entity.ProduceState == "停产")
                        stopPrd = "<span class=\"color-block3\">停产</span>";
					Dictionary<int, string> dictCarParams = _carBLL.GetCarAllParamByCarID(entity.CarID);
					//add by 2014.05.04 获取电动车参数
					if (isElectrombile)
					{
						//普通充电时间
						if (dictCarParams.ContainsKey(879))
						{
							var chargeTime = ConvertHelper.GetInteger(dictCarParams[879]);
							if (minChargeTime == 0 && chargeTime > 0)
								minChargeTime = chargeTime;
							if (chargeTime < minChargeTime)
								minChargeTime = chargeTime;
							if (chargeTime > maxChargeTime)
								maxChargeTime = chargeTime;
						}
						//快速充电时间
						if (dictCarParams.ContainsKey(878))
						{
							var fastChargeTime = ConvertHelper.GetInteger(dictCarParams[878]);
							if (minFastChargeTime == 0 && fastChargeTime > 0)
								minFastChargeTime = fastChargeTime;
							if (fastChargeTime < minFastChargeTime)
								minFastChargeTime = fastChargeTime;
							if (fastChargeTime > maxFastChargeTime)
								maxFastChargeTime = fastChargeTime;
						}
						//纯电最高续航里程
						if (dictCarParams.ContainsKey(883))
						{
							var mileage = ConvertHelper.GetInteger(dictCarParams[883]);
							if (minMileage == 0 && mileage > 0)
								minMileage = mileage;
							if (mileage < minMileage)
								minMileage = mileage;
							if (mileage > maxMileage)
								maxMileage = mileage;
						}
					}
					// 节能补贴 Sep.2.2010
					string hasEnergySubsidy = "";
					//补贴功能临时去掉 modified by chengl Oct.24.2013
					//if (dictCarParams.ContainsKey(853) && dictCarParams[853] == "3000元")
					//{
					//    hasEnergySubsidy = " <a href=\"http://news.bitauto.com/zcxwtt/20130924/1406235633.html\" class=\"butie\" title=\"可获得3000元节能补贴\" target=\"_blank\">补贴</a>";
					//}

                    //============2016-02-26 减税 购置税============================
                    string strTravelTax = "";
                    double dEx = 0.0;
                    Double.TryParse(entity.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
                    if (entity.SaleState == "在销")
                    {
                        if (dictCarParams.ContainsKey(987) && (dictCarParams[987] == "第1批" || dictCarParams[987] == "第2批" || dictCarParams[987] == "第3批" || dictCarParams[987] == "第4批" || dictCarParams[987] == "第5批" || dictCarParams[987] == "第6批") && dictCarParams.ContainsKey(986))
                        {
                            strTravelTax = " <a target=\"_blank\" title=\"{0}\" href=\"http://news.bitauto.com/sum/20170105/1406774416.html\" class=\"color-block2\">减税</a>";
                            if (dictCarParams[986].ToString() == "减半")
                            {
                                strTravelTax = string.Format(strTravelTax, "购置税减半");
                            }
                            else if (dictCarParams[986].ToString() == "免征")
                            {
                                strTravelTax = string.Format(strTravelTax, "免征购置税");
                            }
                        }
                        else if (dEx > 0 && dEx <= 1.6)
                        {
                            strTravelTax = " <a target=\"_blank\" title=\"购置税75折\" href=\"http://news.bitauto.com/sum/20170105/1406774416.html\" class=\"color-block2\">减税</a>";
                        }
                    }
					////易车惠
					//string strYiCheHui = "";
					//var carGoods = serialGoodsCarList.Find(p => p.CarId == entity.CarID);
					//if (carGoods != null)
					//{
					//    var goodsUrl = carGoods.GoodsUrl.Replace("/detail", "/all/detail") + "?WT.mc_id=car2";
					//    strYiCheHui = string.Format("<a target=\"_blank\" title=\"{0}\" href=\"{1}\" class=\"ad-yichehui-list\">易车惠特价&gt;&gt;</a>", "", goodsUrl);
					//}
					//string strBest = "<a href=\"#\" class=\"ico-tuijian\">推荐</a>";
                    carListHtml.Add(string.Format("<tr id=\"car_filter_id_{0}\">", entity.CarID));
                    carListHtml.Add(string.Format("<td class=\"txt-left\" id=\"carlist_{1}\"><a href=\"/{0}/m{1}/\" target=\"_blank\">{2} {3}</a> {4}</td>",
						_SerialSpell, entity.CarID, yearType, entity.CarName, strTravelTax + hasEnergySubsidy + stopPrd));
					carListHtml.Add("<td>");
					carListHtml.Add("<div class=\"w\">");
					//计算百分比
					int percent = (int)Math.Round((double)entity.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero);
					carListHtml.Add(string.Format("<div class=\"p\" style=\"width: {0}%\"></div>", percent));
					carListHtml.Add("    </div>");
					carListHtml.Add("</td>");
					// 档位个数
					string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" && dictCarParams[724] != "待查") ? dictCarParams[724] + "挡" : "";

					carListHtml.Add(string.Format("<td>{0}</td>", forwardGearNum + entity.TransmissionType));
					carListHtml.Add(string.Format("<td class=\"txt-right\"><span>{0}</span><a title=\"购车费用计算\" class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid={1}\" target=\"_blank\"></a></td>", string.IsNullOrEmpty(entity.ReferPrice) ? "暂无" : entity.ReferPrice + "万", entity.CarID));
					if (entity.CarPriceRange.Trim().Length == 0)
                        carListHtml.Add(string.Format("<td class=\"txt-right\"><span>{0}</span></td>", "暂无报价"));
					else
					{
						//取最低报价
						string minPrice = entity.CarPriceRange;
						if (entity.CarPriceRange.IndexOf("-") != -1)
							minPrice = entity.CarPriceRange.Substring(0, entity.CarPriceRange.IndexOf('-'));

                        carListHtml.Add(string.Format("<td class=\"txt-right\"><span><a href=\"/{0}/m{1}/baojia/\" target=\"_blank\">{2}</a></span></td>", _SerialSpell, entity.CarID, minPrice));
					}
                    carListHtml.Add("<td class=\"txt-right\">");
                    carListHtml.Add(string.Format("<a class=\"btn btn-primary btn-xs\" href=\"http://dealer.bitauto.com/zuidijia/nb{0}/nc{1}/?T=2&leads_source=p015006\" target=\"_blank\">询底价</a>", _SerialId, entity.CarID));
                    carListHtml.Add(string.Format(" <a class=\"btn btn-secondary btn-xs\" target=\"_self\" href=\"javascript:;\"  data-use=\"compare\" data-id=\"{0}\">+对比</a>", entity.CarID));
					carListHtml.Add("    </td>");
					carListHtml.Add("</tr>");
				}
			}
			//add by 2014.05.04 电动车 参数
			if (maxChargeTime > 0)
			{
				chargeTimeRange = minChargeTime == maxChargeTime ? string.Format("{0}分钟", minChargeTime) : string.Format("{0}-{1}分钟", minChargeTime, maxChargeTime);
			}
			else
				chargeTimeRange = "暂无";
			if (maxFastChargeTime > 0)
			{
				fastChargeTimeRange = minFastChargeTime == maxFastChargeTime ? string.Format("{0}分钟", minFastChargeTime) : string.Format("{0}-{1}分钟", minFastChargeTime, maxFastChargeTime);
			}
			else
				fastChargeTimeRange = "暂无";
			if (maxMileage > 0)
			{
				mileageRange = minMileage == maxMileage ? string.Format("{0}公里", minMileage) : string.Format("{0}-{1}公里", minMileage, maxMileage);
			}
			else
				mileageRange = "暂无";
			return string.Concat(carListHtml.ToArray());
		}
		#endregion
	}
}