using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.BLL.CarTreeData;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.CarTreeV2
{
	public partial class CX_brand : TreePageBase
	{
		protected int _BrandId;
		protected string _BrandSpell;
		protected string _BrandName = String.Empty;
		protected int _BrandContainsSerialCount = 0;
		protected string _SerialList = String.Empty;
		protected string _GuilderString = String.Empty;
		protected string _EncodeName;
		protected BrandEntity _BrandEntity;
		private string _MasterBrandTopUrl = string.Empty;
		private Dictionary<int, int> _SubsidiesList;
		protected string NavPathHtml = "";
		// 级别 默认显示全部
		private int _Level = 0;
		// 默认全部
		private EnumCollection.FlagsSerialLeve fsl = EnumCollection.FlagsSerialLeve.全部;
		private Dictionary<string, List<DataRow>> dicCs = new Dictionary<string, List<DataRow>>();

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			GetParam();
			_SubsidiesList = new Car_SerialBll().GetSubsidiesSerialList();
			InitData();
			//NavbarHtml = base.GetTreeNavBarHtml("brand", "chexing", _BrandId);
			//搜索地址
			this._SearchUrl = InitSearchUrl("chexing");
			//生成条件Html
			//this.MakeConditionsHtml("按条件选车", false, true);
		}
		/// <summary>
		/// 得到页面的参数
		/// </summary>
		private void GetParam()
		{
			_BrandId = string.IsNullOrEmpty(Request.QueryString["id"])
					? 0 : ConvertHelper.GetInteger(Request.QueryString["id"].ToString());

			_Level = string.IsNullOrEmpty(Request.QueryString["l"])
			? 0 : ConvertHelper.GetInteger(Request.QueryString["l"].ToString());
			fsl = (EnumCollection.FlagsSerialLeve)_Level;
		}
		/// <summary>
		/// 初始化页面数据
		/// </summary>
		private void InitData()
		{
			InitBrand();
			//面包屑
			GetNavbarHtml();
			GetMasterContainsBrandList();
		}

		private void GetNavbarHtml()
		{
			StringBuilder sbNavbar = new StringBuilder();
			//Car_BrandBll brand = new Car_BrandBll();
			string masterSpell = _BrandEntity.MasterBrand.AllSpell;
			string masterName = _BrandEntity.MasterBrand.Name;
			int masterId = _BrandEntity.MasterBrand.Id; //brand.GetMasterbrandByBrand(_BrandId, out masterSpell, out masterName);
			string brandName = (_BrandEntity.Country != "中国" ? "进口" : "") + _BrandEntity.Name;
			sbNavbar.Append("<div class=\"crumbs h-line\"><div class=\"crumbs-txt\">");
			sbNavbar.AppendFormat("<span>当前位置：</span><a href=\"http://www.bitauto.com/\">易车</a> &gt; <a href=\"/\">车型</a> &gt; <a href=\"/tree_chexing/mb_{0}/\">{1}</a> &gt; <strong>{2}</strong>"
				, masterId
				, masterName
				, brandName);
            sbNavbar.Append("</div></div>");
			NavPathHtml = sbNavbar.ToString();
		}
		/// <summary>
		/// 初始化品牌
		/// </summary>
		private void InitBrand()
		{
			_BrandEntity = (BrandEntity)DataManager.GetDataEntity(EntityType.Brand, _BrandId);
			if (_BrandEntity == null || _BrandEntity.Id == 0)
			{
				Response.Redirect("/404error.aspx");
			}
			_BrandName = _BrandEntity.Name;
			_BrandSpell = _BrandEntity.AllSpell;
			_EncodeName = Server.UrlEncode(_BrandEntity.Name);

			if (_BrandEntity.MasterBrand.BrandList.Length == 1
				&& (_BrandEntity.Name == _BrandEntity.MasterBrand.Name
				|| _BrandEntity.Name == "进口" + _BrandEntity.MasterBrand.Name))
			{
				Dictionary<string, TagData> tagDic = TagData.GetTagDataDictionary();

				string masterBrandUrl = tagDic["chexing"].UrlDictionary["masterbrand"].UrlRule.Replace("@objid@", _BrandEntity.MasterBrandId.ToString());
				Response.Redirect(masterBrandUrl);
			}

		}
		/// <summary>
		/// 得到主品牌包含的品牌
		/// </summary>
		/// <returns></returns>
		private void GetMasterContainsBrandList()
		{
			if (_BrandEntity == null || _BrandEntity.Id == 0)
			{
				return;
			}

			// 子品牌报价区间
			//Dictionary<int, string> dicCsPrice = base.GetAllCsPriceRange();

			// 子品牌封面
			Dictionary<int, string> dicCsPhoto = base.GetAllSerialPicURL(true);

			// add by chengl Dec.5.2013
			// 年度十佳车
			List<BestTopCar> listAllBestCar = Car_SerialBll.GetAllBestTopCar();

			// List<CarSerialPhotoEntity> serialList = new Car_BrandBll().GetCarSerialPhotoListByCBID(_BrandEntity.Id, false);
			//十佳车型
			// Dictionary<int, string> bestCarDic = Car_SerialBll.GetBestCarTop10();

			// add by chengl Feb.22.2012
			// 2012十佳车型
			// Dictionary<int, string> bestCarDic2012 = Car_SerialBll.GetBestCarTop10ByYear(2012);

			#region 初始化 子品牌列表
			dicCs.Add("全部", new List<DataRow>());
			dicCs.Add("轿车", new List<DataRow>());
			dicCs.Add("SUV", new List<DataRow>());
			dicCs.Add("跑车", new List<DataRow>());
			dicCs.Add("MPV", new List<DataRow>());
			dicCs.Add("皮卡", new List<DataRow>());
			dicCs.Add("面包车", new List<DataRow>());
			#endregion

			// 主品牌旗下所有子品牌 (非停销)
			DataSet dsCs = new Car_BrandBll().GetCarSerialListByBrandID(_BrandEntity.Id, true);
			if (dsCs != null && dsCs.Tables.Count > 0 && dsCs.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsCs.Tables[0].Rows)
				{
					string carLevel = dr["cslevel"].ToString().Trim();
					if (carLevel == "概念车")
					{ continue; }
                    int serialId = ConvertHelper.GetInteger(dr["cs_id"]);
                    string imgUrl = dicCsPhoto.ContainsKey(serialId) ? dicCsPhoto[serialId].Replace("_2.", "_1.") : WebConfig.DefaultCarPic;
                    string sellState = ConvertHelper.GetString(dr["CsSaleState"]);
                    if (sellState.Trim() == "停销" && imgUrl.IndexOf("150-100.gif") > 0)
                    {
                        continue;
                    }
					if (dicCs.ContainsKey("全部"))
					{ dicCs["全部"].Add(dr); }
					if (dicCs.ContainsKey("轿车") &&
						(carLevel == "微型车" || carLevel == "小型车"
						|| carLevel == "紧凑型车" || carLevel == "中型车"
						|| carLevel == "中大型车" || carLevel == "豪华车"))
					{
						dicCs["轿车"].Add(dr);
					}

					GroupCsByLevel("SUV", carLevel, dr, dicCs);
					GroupCsByLevel("跑车", carLevel, dr, dicCs);
					GroupCsByLevel("MPV", carLevel, dr, dicCs);
					GroupCsByLevel("皮卡", carLevel, dr, dicCs);
					GroupCsByLevel("面包车", carLevel, dr, dicCs);
				}
			}

			StringBuilder sbCsList = new StringBuilder();
			StringBuilder sbTitle = new StringBuilder();
			if (dicCs != null && dicCs.Count > 0)
			{
				int loop = 0;
                sbTitle.AppendLine("<ul id=\"divCsLevelIndex\" class=\"nav\">");
				var levelCount = dicCs.Where(p => p.Value.Count > 0).Count();
				foreach (KeyValuePair<string, List<DataRow>> kvp in dicCs)
				{
					// 循环输出各级别的子品牌
					if (kvp.Value == null || kvp.Value.Count < 1)
					{ continue; }

					#region 当前级别
					bool isCurrent = false;
					if (kvp.Key == "全部"
						&& fsl == EnumCollection.FlagsSerialLeve.全部)
					{ isCurrent = true; }
					if (kvp.Key == "轿车"
						&& (
						(fsl & EnumCollection.FlagsSerialLeve.微型车) == EnumCollection.FlagsSerialLeve.微型车
						|| (fsl & EnumCollection.FlagsSerialLeve.小型车) == EnumCollection.FlagsSerialLeve.小型车
						|| (fsl & EnumCollection.FlagsSerialLeve.紧凑型车) == EnumCollection.FlagsSerialLeve.紧凑型车
						|| (fsl & EnumCollection.FlagsSerialLeve.中大型车) == EnumCollection.FlagsSerialLeve.中大型车
						|| (fsl & EnumCollection.FlagsSerialLeve.中型车) == EnumCollection.FlagsSerialLeve.中型车
						|| (fsl & EnumCollection.FlagsSerialLeve.豪华车) == EnumCollection.FlagsSerialLeve.豪华车)
						)
					{ isCurrent = true; }
					if (kvp.Key == "SUV"
						&& (fsl & EnumCollection.FlagsSerialLeve.SUV) == EnumCollection.FlagsSerialLeve.SUV)
					{ isCurrent = true; }
					if (kvp.Key == "跑车"
						&& (fsl & EnumCollection.FlagsSerialLeve.跑车) == EnumCollection.FlagsSerialLeve.跑车)
					{ isCurrent = true; }
					if (kvp.Key == "MPV"
						&& (fsl & EnumCollection.FlagsSerialLeve.MPV) == EnumCollection.FlagsSerialLeve.MPV)
					{ isCurrent = true; }
					if (kvp.Key == "皮卡"
						&& (fsl & EnumCollection.FlagsSerialLeve.皮卡) == EnumCollection.FlagsSerialLeve.皮卡)
					{ isCurrent = true; }
					if (kvp.Key == "面包车"
						&& (fsl & EnumCollection.FlagsSerialLeve.面包车) == EnumCollection.FlagsSerialLeve.面包车)
					{ isCurrent = true; }
					#endregion

					sbTitle.AppendLine(string.Format("<li class=\"{1}\"><a href=\"javascript:;\">{0}</a></li>", kvp.Key, isCurrent ? "current" : ""));

					sbCsList.AppendLine(string.Format("<div id=\"divCsLevel_{0}\" {1}>"
						, loop, isCurrent ? "" : "style=\"display:none;\""));
                    sbCsList.AppendLine("<div class=\"row block-4col-180\">");
                    StringBuilder sbTempList = new StringBuilder();
					foreach (DataRow dr in kvp.Value)
					{
						#region
						int serialId = ConvertHelper.GetInteger(dr["cs_id"]);
						string csName = ConvertHelper.GetString(dr["cs_name"]);
						string csShowName = ConvertHelper.GetString(dr["cs_ShowName"].ToString().Trim());
						// string shortShowName = csShowName.Replace("(进口)", "");
						string csSpell = ConvertHelper.GetString(dr["csspell"]).Trim().ToLower();
						string imgUrl = dicCsPhoto.ContainsKey(serialId) ? dicCsPhoto[serialId].Replace("_2.", "_3.") : WebConfig.DefaultCarPic;
						// string csLevel = ConvertHelper.GetString(dr["cslevel"]);
						string sellState = ConvertHelper.GetString(dr["CsSaleState"]);
						//string priceRang = dicCsPrice.ContainsKey(serialId) ? dicCsPrice[serialId] : "";
                        //改为指导价
                        string priceRang = base.GetSerialReferPriceByID(serialId);
                        string subsidiesString = "";
						string bestCarStr = "";

						// 新10佳车 add by chengl Dec.5.2013
						if (listAllBestCar != null && listAllBestCar.Count > 0)
						{
							foreach (BestTopCar btc in listAllBestCar)
							{
								if (btc.ListCsList.Contains(serialId))
								{
									bestCarStr = "<a href=\"" + btc.Link + "\" class=\"shijiache\" title=\"" + btc.Title + "\" target=\"_blank\"></a>";
									break;
								}
							}
						}

						//if (bestCarDic.ContainsKey(serialId))
						//    bestCarStr = " <a href=\"http://www.bitauto.com/top10cars/gd_2011/\" target=\"_blank\"><img class=\"ico_shijia\" src=\"http://image.bitautoimg.com/uimg/car/images/ico_shijiache.gif\" alt=\"年度十佳车\" title=\"年度十佳车\" /></a>";
						//// add by chengl Feb.22.2012
						//if (bestCarDic2012.ContainsKey(serialId))
						//{
						//    bestCarStr = " <a href=\"http://www.bitauto.com/top10cars/\" target=\"_blank\"><img class=\"ico_shijia\" src=\"http://image.bitautoimg.com/uimg/car/images/ico_shijiache.gif\" alt=\"年度十佳车\" title=\"年度十佳车\" /></a>";
						//}

						if (_SubsidiesList != null && _SubsidiesList.ContainsKey(serialId))
						{
							subsidiesString = " <span class=\"green\">补贴</span>";
						}
						if (serialId == 1568)
						{
							csShowName = "索纳塔八";
						}
                        if (sellState.Trim() == "待销")
						{
                            priceRang = "未上市";
						}
                        else if (sellState.Trim() == "停销")
                        {
                            priceRang = "停售";
                        }
                        else if (priceRang.Trim().Length == 0 && sellState.Trim() == "在销")
						{
							priceRang = "暂无指导价";
						}
						#endregion

                        if (sellState.Trim() == "停销" && imgUrl.IndexOf("150-100.gif") > 0)
                        {
                            continue;
                        }
						if (kvp.Key == "全部")
						{
							_BrandContainsSerialCount++;
						}
                        if (sellState.Trim() == "停销" || sellState.Trim() == "待销")
                        {
                            sbTempList.AppendLine("<div class=\"col-xs-3\"><div class=\"img-info-layout-vertical img-info-layout-vertical-180120 inverse icon-none no-reduce\">");
                            sbTempList.AppendLine(string.Format(
                                "<div class=\"img\"><a href=\"/{0}/\" title=\"{3}\" target=\"_blank\" id=\"n{1}\"><img src=\"{2}\" alt=\"{3}\"></a></div>"
                                , csSpell, serialId, imgUrl, csShowName));
                            sbTempList.AppendLine(string.Format("<ul class=\"p-list\"><li class=\"name\"><a href=\"/{0}/\" title=\"{1}\" target=\"_blank\">{1}</a></li><li class=\"price\"><a href=\"/{0}/\" class=\"price-reduction\" title=\"{1}\" target=\"_blank\">{2}</a></li></ul>"
                                , csSpell, csShowName, priceRang
                                //subsidiesString, bestCarStr,priceRang == "暂无报价" ? " huizi" : ""
                                ));
                            sbTempList.AppendLine("</div></div>");
                        }
                        else
                        {
                            sbTempList.AppendLine("<div class=\"col-xs-3\"><div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
                            sbTempList.AppendLine(string.Format(
                                "<div class=\"img\"><a href=\"/{0}/\" title=\"{3}\" target=\"_blank\" id=\"n{1}\"><img src=\"{2}\" alt=\"{3}\"></a></div>"
                                , csSpell, serialId, imgUrl, csShowName));
                            sbTempList.AppendLine(string.Format("<ul class=\"p-list\"><li class=\"name\"><a href=\"/{0}/\" title=\"{1}\" target=\"_blank\">{1}</a></li><li class=\"price\"><a href=\"/{0}/\" title=\"{1}\" target=\"_blank\">{2}</a></li></ul>"
                                , csSpell, csShowName, priceRang));
                            sbTempList.AppendLine("</div></div>");
                        }
					}
                    sbCsList.Insert(sbCsList.Length, sbTempList.ToString());
					sbCsList.AppendLine("</div>");
                    sbCsList.AppendLine("</div>");
					loop++;
				}
				sbTitle.AppendLine("</ul>");
			}
			string brandName = (_BrandEntity.Country != "中国" ? "进口" : "") + _BrandEntity.Name;
			List<string> listHTML = new List<string>();
            listHTML.Add("<div class=\"main-inner-section condition-selectcar type-3\">");
            listHTML.Add("<div class=\"section-header header1 h-default\">");
			listHTML.Add("<div class=\"box\">");
			listHTML.Add(string.Format("<h2><a href=\"/{0}/\" target=\"_blank\">{1}</a></h2>", _BrandSpell
				, brandName));
			listHTML.Add(sbTitle.ToString());
            listHTML.Add("</div>");
			listHTML.Add(_BrandEntity.OfficialUrl != "" ? "<div class=\"more\"><a target=\"_blank\" href=\"" + _BrandEntity.OfficialUrl + "\">官方网站>></a></div>" : "");
			listHTML.Add("</div>");
			listHTML.Add(sbCsList.ToString());
			listHTML.Add("</div>");
			_SerialList = string.Concat(listHTML.ToArray());
		}

		private void GroupCsByLevel(string key, string level, DataRow dr, Dictionary<string, List<DataRow>> dic)
		{
			if ((key == level) && dic.ContainsKey(key))
			{
				dic[key].Add(dr);
			}
		}

	}
}