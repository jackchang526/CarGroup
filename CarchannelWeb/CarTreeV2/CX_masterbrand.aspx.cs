using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Model;

using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.CarTreeV2
{
	public partial class CX_masterbrand : TreePageBase
	{
		protected int _MasterBrandId;
		protected string _MasterBrandSpell;
		protected string _EncodeName;
		protected string _MasterBrandName;
		protected string _ContainsBrandName;
		protected int _SerialCount = 0;
		protected string _SerialListShow = string.Empty;
		protected string _MasterIntroduce = string.Empty;
		protected string _GuildString = string.Empty;
		private MasterBrandEntity _MasterBrandEntity;
		private Dictionary<int, int> _SubsidiesList;
		protected string NavPathHtml = "";
		// 级别 默认显示全部
		private int _Level = 0;
		// 默认全部
		private EnumCollection.FlagsSerialLeve fsl = EnumCollection.FlagsSerialLeve.全部;
		private Dictionary<string, List<DataRow>> dicCs = new Dictionary<string, List<DataRow>>();
        Car_SerialBll carSerialBll = new Car_SerialBll();


        protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			GetParam();
			_SubsidiesList = carSerialBll.GetSubsidiesSerialList();
			InitData();
			//NavbarHtml = base.GetTreeNavBarHtml("masterbrand", "chexing", _MasterBrandId);
			//搜索地址
			this._SearchUrl = InitSearchUrl("chexing");
			//生成条件Html
			//this.MakeConditionsHtml("按条件选车", false, true);
		}
		/// <summary>
		/// 得到页面参数
		/// </summary>
		private void GetParam()
		{
			_MasterBrandId = string.IsNullOrEmpty(Request.QueryString["id"])
				? 0 : ConvertHelper.GetInteger(Request.QueryString["id"].ToString());

			_Level = string.IsNullOrEmpty(Request.QueryString["l"])
				? 0 : ConvertHelper.GetInteger(Request.QueryString["l"].ToString());
			fsl = (EnumCollection.FlagsSerialLeve)_Level;
		}
		/// <summary>
		/// 初始化数据
		/// </summary>
		private void InitData()
		{
			//InitSourceUrl();
			InitMasterBrand();
			//面包屑
			GetNavbarHtml();
			//GetPageGuilder();
			//主品牌介绍
			GetMasterBrandIntroduce();
			//品牌列表显示
			GetMasterContainsBrandList();
		}

		private void GetNavbarHtml()
		{
			StringBuilder sbNavbar = new StringBuilder();
			sbNavbar.Append("<div class=\"crumbs h-line\">");
			sbNavbar.AppendFormat("<div class=\"crumbs-txt\"><span>当前位置：</span><a href=\"http://www.bitauto.com/\">易车</a> &gt; <a href=\"/\">车型</a> &gt; <strong>{0}</strong></div>", _MasterBrandName);
			sbNavbar.Append("</div>");
			NavPathHtml = sbNavbar.ToString();
		}
		/// <summary>
		/// 初始化主品牌数据
		/// </summary>
		private void InitMasterBrand()
		{
			_MasterBrandEntity = (MasterBrandEntity)DataManager.GetDataEntity(EntityType.MasterBrand, _MasterBrandId);
			if (_MasterBrandEntity == null || _MasterBrandEntity.Id == 0)
			{
				Response.Redirect("/404error.aspx");
			}
			_MasterBrandSpell = _MasterBrandEntity.AllSpell;
			_MasterBrandName = _MasterBrandEntity.Name;
			_EncodeName = Server.UrlEncode(_MasterBrandEntity.Name);
		}

		/// <summary>
		/// 得到主品牌的介绍
		/// </summary>
		/// <returns></returns>
		private void GetMasterBrandIntroduce()
		{
			List<string> introduceHtmlList = new List<string>();
			introduceHtmlList.Add("<div class=\"line-box\">");
			introduceHtmlList.Add("<h3>");
			introduceHtmlList.Add(String.Format("<span><a href=\"/{1}/\" target=\"_blank\">{0}介绍</a></span></h3>"
										, _MasterBrandEntity.Name
										, _MasterBrandEntity.AllSpell));
			introduceHtmlList.Add("<div class=\"c0622_02\"><div class=\"bybrand_list\">");
			introduceHtmlList.Add(String.Format("<a class=\"brand m_{0}_b\" target=\"_blank\" href=\"/{1}/\"></a>"
										, _MasterBrandEntity.Id
										, _MasterBrandEntity.AllSpell));
			introduceHtmlList.Add(String.Format("<p>{0}<a href=\"/{1}/\" target=\"_blank\">详细&gt;&gt;</a></p>"
										, StringHelper.SubString(_MasterBrandEntity.Introduction, 250, true).Replace("\r\n", "<br/>")
										, _MasterBrandEntity.AllSpell));
			introduceHtmlList.Add("</div></div><div class=\"clear\"></div></div>");
			_MasterIntroduce = String.Concat(introduceHtmlList.ToArray());
		}
		/// <summary>
		/// 得到主品牌包含的品牌
		/// </summary>
		/// <returns></returns>
		private void GetMasterContainsBrandList()
		{
			if (_MasterBrandEntity == null || _MasterBrandEntity.Id == 0)
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

			////十佳车型
			//Dictionary<int, string> bestCarDic = Car_SerialBll.GetBestCarTop10();

			//// add by chengl Feb.22.2012
			//// 2012十佳车型
			//Dictionary<int, string> bestCarDic2012 = Car_SerialBll.GetBestCarTop10ByYear(2012);

			#region 初始化 子品牌列表
			dicCs.Add("全部", new List<DataRow>());
			dicCs.Add("轿车", new List<DataRow>());
			dicCs.Add("SUV", new List<DataRow>());
			dicCs.Add("跑车", new List<DataRow>());
			dicCs.Add("MPV", new List<DataRow>());
			dicCs.Add("皮卡", new List<DataRow>());
			dicCs.Add("面包车", new List<DataRow>());
			#endregion
			// 主品牌旗下所有子品牌 (包含停销)
			DataSet dsCs = new Car_BrandBll().GetCarSerialListByBSID(_MasterBrandId, true);
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

					var query = kvp.Value.GroupBy(p => new { BrandName = p["cb_name"].ToString(), BrandId = ConvertHelper.GetInteger(p["cb_id"]), Country = ConvertHelper.GetString(p["cp_Country"]) }, p => p);
                    sbCsList.AppendLine(string.Format("<div id=\"divCsLevel_{0}\" {1}>"
                        , loop, isCurrent ? "" : "style=\"display:none;\""));
					foreach (var group in query)
					{
						var key = CommonFunction.Cast(group.Key, new { BrandName = "", BrandId = 0, Country = "" });
						var groupList = group.ToList<DataRow>();//分组后的集合
						var brandName = _MasterBrandEntity.BrandList.Length > 1 && key.Country != "中国" ? "进口" + key.BrandName : key.BrandName;
						if (_MasterBrandEntity.BrandList.Length > 1)
							sbCsList.AppendFormat("<h5 class=\"h5-sep\"><a href=\"/tree_chexing/b_{0}/\">{1}&gt;&gt;</a></h5>",
									key.BrandId, brandName);
						sbCsList.AppendLine("<div class=\"row block-4col-180\">");
                        StringBuilder sbTempList = new StringBuilder();
						foreach (DataRow dr in groupList)
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
                            //改为指导价
                            string priceRang = base.GetSerialReferPriceByID(serialId);
							string subsidiesString = "";
							string bestCarStr = "";
                            string newCarIntoMarcket = carSerialBll.GetNewSerialIntoMarketText(serialId, false);
                            if (!string.IsNullOrWhiteSpace(newCarIntoMarcket))
                            {
                                newCarIntoMarcket = string.Format("<span class=\"spl-label type{1}\">{0}</span>"
                                    ,newCarIntoMarcket
                                    ,newCarIntoMarcket == "即将上市" ? "2":"1");
                            }

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
								_SerialCount++;
							}
                            if (sellState.Trim() == "停销" || sellState.Trim() == "待销"||priceRang == "暂无指导价")
                            {
                                sbTempList.AppendLine("<div class=\"col-xs-3\"><div class=\"img-info-layout-vertical img-info-layout-vertical-180120 inverse icon-none no-reduce\">");
                                sbTempList.AppendLine(string.Format(
                                   "<div class=\"img\"><a href=\"/{0}/\" title=\"{3}\" target=\"_blank\" id=\"n{1}\">{4}<img src=\"{2}\" alt=\"{3}\"></a></div>"
                                   , csSpell, serialId, imgUrl, csShowName, newCarIntoMarcket));
                                sbTempList.AppendLine(string.Format("<ul class=\"p-list\"><li class=\"name\"><a href=\"/{0}/\" title=\"{1}\" target=\"_blank\" id=\"m{2}\">{1}</a></li><li class=\"price\"><a href=\"/{0}/\" class=\"price-reduction\" title=\"{1}\" target=\"_blank\">{3}</a></li></ul>"
                                    , csSpell, csShowName, serialId, priceRang, subsidiesString, bestCarStr,
                                   priceRang == "暂无指导价" ? " huizi" : ""));
                                sbTempList.AppendLine("</div></div>");
                            }
                            else
                            {
                                sbCsList.AppendLine("<div class=\"col-xs-3\"><div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
                                sbCsList.AppendLine(string.Format(
                                    "<div class=\"img\"><a href=\"/{0}/\" title=\"{3}\" target=\"_blank\" id=\"n{1}\">{4}<img src=\"{2}\" alt=\"{3}\"></a></div>"
                                    , csSpell, serialId, imgUrl, csShowName, newCarIntoMarcket));
                                sbCsList.AppendLine(string.Format("<ul class=\"p-list\"><li class=\"name\"><a href=\"/{0}/\" title=\"{1}\" target=\"_blank\" id=\"m{2}\">{1}</a><li><li class=\"price\"><a href=\"/{0}/\" title=\"{1}\" target=\"_blank\" id=\"m{2}\">{3}</a></li></ul>"
                                    , csSpell, csShowName, serialId, priceRang, subsidiesString, bestCarStr,
                                   priceRang == "暂无指导价" ? " huizi" : ""));
                                sbCsList.AppendLine("</li>");
                                sbCsList.AppendLine("</div></div>");
                            }
						}
                        sbCsList.Insert(sbCsList.Length, sbTempList.ToString());
                        sbCsList.AppendLine("</div>");
					}
                    sbCsList.AppendLine("</div>");
					loop++;
				}
				sbTitle.AppendLine("</ul>");
			}

			List<string> listHTML = new List<string>();
            listHTML.Add("<div class=\"main-inner-section condition-selectcar type-3\">");
            listHTML.Add("<div class=\"section-header header1 h-default\">");
			listHTML.Add("<div class=\"box\">");
			listHTML.Add(string.Format("<h2><a href=\"/{0}/\" target=\"_blank\">{1}</a></h2>", _MasterBrandEntity.AllSpell
				, _MasterBrandEntity.Name.Length >= 4 ? _MasterBrandEntity.Name : _MasterBrandEntity.Name));
			listHTML.Add(sbTitle.ToString());
            listHTML.Add("</div>");
            listHTML.Add(string.Format("<div class=\"more\"><a href=\"/{0}/\" target=\"_blank\">频道</a><a href=\"http://www.taoche.com/{0}/?leads_source=p012001\" target=\"_blank\">二手车</a></div>", _MasterBrandEntity.AllSpell));
			listHTML.Add("</div>");

			listHTML.Add(sbCsList.ToString());
			listHTML.Add("</div>");
			_SerialListShow = string.Concat(listHTML.ToArray());

			/* old modified by chengl Sep.24.2013
			DataSet brandDs = new Car_BrandBll().GetCarSerialPhotoListByBSID(_MasterBrandId, false);
			if (brandDs == null || brandDs.Tables.Count < 1)
			{
				return;
			}

			List<string> serialHtmlList = new List<string>();
			string brandTitle = "<div class=\"line_box\" listtype=\"cartype\"><h3>"
							   + "<span><a href=\"/{1}/\" target=\"_blank\">{0}</a></span></h3>"
							   + "<div class=\"c0621_03 c0621_02\">";
			string brandFooter = "</div><div class=\"clear\"></div></div>";
			
			 * 参数0:子品牌urlspell
			 * 参数1:子品牌名称
			 * 参数2:子品牌图片地址
			 * 参数3:子品牌短名称
			 * 参数4:子品牌报价
			 * 参数5:子品牌ID
			 * 参数6:子品牌补贴
			 
			string serialFormater = "<li><a id=\"n{5}\" stattype=\"car\" target=\"_blank\" href=\"/{0}/\">"
								+ "<img width=\"120\" height=\"80\" alt=\"{1}\" src=\"{2}\"></a>"
								+ "<a id=\"m{5}\" stattype=\"car\" target=\"_blank\" title=\"{1}\" href=\"/{0}/\">{3}</a>{6}{7}<br/><span>{4}</span>"
								+ "</li>";

			//十佳车型
			Dictionary<int, string> bestCarDic = Car_SerialBll.GetBestCarTop10();

			// add by chengl Feb.22.2012
			// 2012十佳车型
			Dictionary<int, string> bestCarDic2012 = Car_SerialBll.GetBestCarTop10ByYear(2012);

			List<string> csInBrandList = new List<string>();
			foreach (DataTable brandTable in brandDs.Tables)
			{
				if (brandTable.Rows.Count < 1) continue;
				//拼接主品牌包含的品牌名
				if (brandDs.Tables.Count > 1
				|| (brandDs.Tables[0].TableName != _MasterBrandName
				&& brandDs.Tables[0].TableName != "进口" + _MasterBrandName))
				{
					_ContainsBrandName += "," + brandTable.TableName;
				}
				if (brandTable.Rows.Count == 1 && ConvertHelper.GetInteger(brandTable.Rows[0]["cs_id"]) < 1)
				{
					//serialHtmlList.Add("<div style=\"text-align:center;font:13px/3 Verdana;color:#C00;\">该品牌下暂无在销车型！</div>");
					//serialHtmlList.Add(brandFooter);
					continue;
				}
				string brandSpell = ConvertHelper.GetString(brandTable.Rows[0]["cbspell"]).Trim().ToLower();
				DataRow[] drList = brandTable.Select("", "CsSaleState desc,spell asc");
				csInBrandList.Clear();
				foreach (DataRow serialRow in drList)
				{
					int serialId = ConvertHelper.GetInteger(serialRow["cs_id"]);
					string csName = ConvertHelper.GetString(serialRow["cs_name"]);
					string csShowName = ConvertHelper.GetString(serialRow["cs_ShowName"]);
					string shortShowName = csShowName.Replace("(进口)", "");
					string csSpell = ConvertHelper.GetString(serialRow["csspell"]).Trim().ToLower();
					string imgUrl = ConvertHelper.GetString(serialRow["csImageUrl"]).ToLower();
					string csLevel = ConvertHelper.GetString(serialRow["cslevel"]);
					string sellState = ConvertHelper.GetString(serialRow["CsSaleState"]);
					string subsidiesString = "";
					string bestCarStr = "";

					if (serialId == 1568)
					{
						shortShowName = "索纳塔八";
					}

					if (bestCarDic.ContainsKey(serialId))
						bestCarStr = " <a href=\"http://www.bitauto.com/top10cars/gd_2011/\" target=\"_blank\"><img class=\"ico_shijia\" src=\"http://image.bitautoimg.com/uimg/car/images/ico_shijiache.gif\" alt=\"年度十佳车\" title=\"年度十佳车\" /></a>";
					// add by chengl Feb.22.2012
					if (bestCarDic2012.ContainsKey(serialId))
					{
						bestCarStr = " <a href=\"http://www.bitauto.com/top10cars/\" target=\"_blank\"><img class=\"ico_shijia\" src=\"http://image.bitautoimg.com/uimg/car/images/ico_shijiache.gif\" alt=\"年度十佳车\" title=\"年度十佳车\" /></a>";
					}

					if (csLevel == "概念车") continue;
					if (csName.IndexOf("停用") >= 0) continue;
					if (sellState == "停销") continue;

					_SerialCount++;
					string priceRang = base.GetSerialPriceRangeByID(serialId);
					string levelSpell = Car_LevelBll.GetLevelSpellByFullName(csLevel);
					imgUrl = imgUrl.Replace("_1.jpg", "_2.jpg");
					if (sellState == "待销")
					{
						priceRang = "未上市";
					}
					else if (priceRang.Trim().Length == 0)
					{
						priceRang = "暂无报价";
					}
					if (_SubsidiesList != null && _SubsidiesList.ContainsKey(serialId))
					{
						subsidiesString = " <span class=\"green\">补贴</span>";
					}
					string serialItem = string.Format(serialFormater, csSpell.ToLower(), csShowName, imgUrl, shortShowName, priceRang, serialId.ToString(), subsidiesString, bestCarStr);
					csInBrandList.Add(serialItem);
				}

				if (csInBrandList.Count > 0)
				{
					string brandName = brandTable.TableName;
					if (_MasterBrandEntity.BrandList.Length <= 1 && brandTable.TableName.IndexOf("进口") != -1)
					{
						brandName = brandTable.TableName.Replace("进口", "");
					}
					serialHtmlList.Add(string.Format(brandTitle, brandName
						, ((brandDs.Tables.Count == 1 && brandName.ToLower() == _MasterBrandName.ToLower()) ? _MasterBrandSpell : brandSpell)));
					serialHtmlList.Add("<ul>");
					serialHtmlList.AddRange(csInBrandList);
					serialHtmlList.Add("</ul>");
					serialHtmlList.Add(brandFooter);
				}
			}
			_SerialListShow = String.Concat(serialHtmlList.ToArray());
			*/
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