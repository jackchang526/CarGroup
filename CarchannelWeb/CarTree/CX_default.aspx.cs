using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;
using BitAuto.CarChannel.CarchannelWeb.App_Code;
using BitAuto.CarUtils.Define;
using System.Data;

namespace BitAuto.CarChannel.CarchannelWeb.CarTree
{
	public partial class CX_default : TreePageBase
	{
		protected string _BrandUrl = string.Empty;
		private List<EnumCollection.SerialSortForInterface>[] _LevelArrSSfi = new List<EnumCollection.SerialSortForInterface>[9];
		private List<EnumCollection.SerialSortForInterface>[] _PriceRangeArrSSfi = new List<EnumCollection.SerialSortForInterface>[8];
		private Car_SerialBll serialBll = new Car_SerialBll();
		private CommonHtmlBll _commonhtmlBLL = new CommonHtmlBll();
		private Car_MasterBrandBll _masterBrandBll;

		protected string hotMasterBrandHtml = string.Empty;
		public CX_default()
		{
			_masterBrandBll = new Car_MasterBrandBll();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30, true);
			InitData();
		}
		/// <summary>
		/// 绑定页面数据
		/// </summary>
		private void InitData()
		{
			InitLevelAndPriceData();
			// modified by chengl Oct.12.2013 
			// 车型首页搜索定制 by 张晓鹏
			//NavbarHtml = base.GetTreeNavBarHtml("homenew", "chexing", 0);
			//搜索地址
			this._SearchUrl = InitSearchUrl("chexing");
			//生成条件Html
			this.MakeConditionsHtmlV2("按条件选车", true, true);

			MakeHotMasterBrand();
		}

		private void MakeHotMasterBrand()
		{
			StringBuilder sb = new StringBuilder();
			DataSet ds = _masterBrandBll.GetHotMasterBrand(10);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				sb.AppendFormat("<ul class=\"bybrand_list\">");
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					sb.AppendFormat("<li><a href=\"/{0}/\" target=\"_blank\"><img src=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_{1}_55.png\" alt=\"{2}\"/></a><div class=\"title\"><a href=\"/{0}/\" target=\"_blank\">{2}</a></div></li>", dr["urlspell"], dr["bs_Id"], dr["bs_Name"]);
				}
				sb.AppendFormat("</ul>");
			}
			hotMasterBrandHtml = sb.ToString();
		}
		/// <summary>
		/// 初始化页面的URL
		/// </summary>
		private void InitUrl()
		{
			if (_TTCEntityList == null
				|| _TTCEntityList.Count < 1
				|| !_TTCEntityList.ContainsKey("chexing"))
			{
				return;
			}

			Dictionary<string, string> urlList = _TTCEntityList["chexing"].MainUrl;

			if (urlList == null
				|| urlList.Count < 1
				|| !urlList.ContainsKey("sourceurl"))
			{
				return;
			}

			_SourceUrl = urlList["sourceurl"];
		}
		/// <summary>
		/// 初始化级别和报价数据
		/// </summary>
		private void InitLevelAndPriceData()
		{
			string levelcacheKeys = "carlevelpvorderdefault";
			string pricecachekeys = "capricepvorderdefault";

			_LevelArrSSfi = (List<EnumCollection.SerialSortForInterface>[])CacheManager.GetCachedData(levelcacheKeys);
			_PriceRangeArrSSfi = (List<EnumCollection.SerialSortForInterface>[])CacheManager.GetCachedData(pricecachekeys);

			if (_LevelArrSSfi == null
				|| _LevelArrSSfi.Length < 1
				|| _PriceRangeArrSSfi == null
				|| _PriceRangeArrSSfi.Length < 1)
			{
				List<EnumCollection.SerialSortForInterface> lssfi = base.GetAllSerialNewly30DayToList();
				_LevelArrSSfi = new List<EnumCollection.SerialSortForInterface>[9];
				_PriceRangeArrSSfi = new List<EnumCollection.SerialSortForInterface>[8];
				for (int i = 0; i < 9; i++)
				{
					if (i < 8)
					{
						_PriceRangeArrSSfi[i] = new List<EnumCollection.SerialSortForInterface>();
					}
					_LevelArrSSfi[i] = new List<EnumCollection.SerialSortForInterface>();
				}

				#region Delete


				foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
				{
					if (_PriceRangeArrSSfi[0].Count < 10 && ssfi.CsPriceRange.IndexOf(",1,") >= 0)
					{ _PriceRangeArrSSfi[0].Add(ssfi); }
					if (_PriceRangeArrSSfi[1].Count < 10 && ssfi.CsPriceRange.IndexOf(",2,") >= 0)
					{ _PriceRangeArrSSfi[1].Add(ssfi); }
					if (_PriceRangeArrSSfi[2].Count < 10 && ssfi.CsPriceRange.IndexOf(",3,") >= 0)
					{ _PriceRangeArrSSfi[2].Add(ssfi); }
					if (_PriceRangeArrSSfi[3].Count < 10 && ssfi.CsPriceRange.IndexOf(",4,") >= 0)
					{ _PriceRangeArrSSfi[3].Add(ssfi); }
					if (_PriceRangeArrSSfi[4].Count < 10 && ssfi.CsPriceRange.IndexOf(",5,") >= 0)
					{ _PriceRangeArrSSfi[4].Add(ssfi); }
					if (_PriceRangeArrSSfi[5].Count < 10 && ssfi.CsPriceRange.IndexOf(",6,") >= 0)
					{ _PriceRangeArrSSfi[5].Add(ssfi); }
					if (_PriceRangeArrSSfi[6].Count < 10 && ssfi.CsPriceRange.IndexOf(",7,") >= 0)
					{ _PriceRangeArrSSfi[6].Add(ssfi); }
					if (_PriceRangeArrSSfi[7].Count < 10 && ssfi.CsPriceRange.IndexOf(",8,") >= 0)
					{ _PriceRangeArrSSfi[7].Add(ssfi); }
					if (_LevelArrSSfi[0].Count < 10 && ssfi.CsLevel == "微型车")
					{ _LevelArrSSfi[0].Add(ssfi); continue; }
					if (_LevelArrSSfi[1].Count < 10 && ssfi.CsLevel == "小型车")
					{ _LevelArrSSfi[1].Add(ssfi); continue; }
					if (_LevelArrSSfi[2].Count < 10 && ssfi.CsLevel == "紧凑型车")
					{ _LevelArrSSfi[2].Add(ssfi); continue; }
					if (_LevelArrSSfi[3].Count < 10 && ssfi.CsLevel == "中型车")
					{ _LevelArrSSfi[3].Add(ssfi); continue; }
					if (_LevelArrSSfi[4].Count < 10 && ssfi.CsLevel == "中大型车")
					{ _LevelArrSSfi[4].Add(ssfi); continue; }
					if (_LevelArrSSfi[5].Count < 10 && ssfi.CsLevel == "豪华车")
					{ _LevelArrSSfi[5].Add(ssfi); continue; }
					if (_LevelArrSSfi[6].Count < 10 && ssfi.CsLevel == "SUV")
					{ _LevelArrSSfi[6].Add(ssfi); continue; }
					if (_LevelArrSSfi[7].Count < 10 && ssfi.CsLevel == "MPV")
					{ _LevelArrSSfi[7].Add(ssfi); continue; }
					if (_LevelArrSSfi[8].Count < 10 && ssfi.CsLevel == "跑车")
					{ _LevelArrSSfi[8].Add(ssfi); continue; }
				}

				#endregion

				CacheManager.InsertCache(levelcacheKeys, _LevelArrSSfi, WebConfig.CachedDuration);
				CacheManager.InsertCache(pricecachekeys, _PriceRangeArrSSfi, WebConfig.CachedDuration);
			}
		}
		/// <summary>
		/// 得到车型关注排行
		/// </summary>
		/// <returns></returns>
		protected string GetCarAttentionList()
		{
			string titleString = string.Empty;
			StringBuilder carAttentionList = new StringBuilder();
			carAttentionList.AppendLine(titleString);
			//添加价格区间
			carAttentionList.AppendLine(GetPriceRangeList());
			//添加级别区间
			carAttentionList.AppendLine(GetLevelList());

			return carAttentionList.ToString();
		}

		/// <summary>
		/// 首页热门车10个
		/// </summary>
		/// <returns></returns>
		protected string GetHotCarTypeNew()
		{
			StringBuilder hotCarType = new StringBuilder();
			try
			{
				hotCarType.Append("<ul id=\"data_box3_0\">");
				hotCarType.AppendLine(serialBll.GetHomepageHotSerial(8));
				hotCarType.Append("</ul>");
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return hotCarType.ToString();
		}

		/// <summary>
		/// 首页新车10个
		/// </summary>
		/// <returns></returns>
		protected string GetNewCarTypeNew()
		{
			StringBuilder newCarType = new StringBuilder();
			try
			{
				Dictionary<int, string> dict = serialBll.GetAllSerialMarkDay();
				List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>(dict);
				List<KeyValuePair<int, string>> sublist = new List<KeyValuePair<int, string>>();
				foreach (KeyValuePair<int, string> key in list)
				{
					if (CommonFunction.DateDiff("d", ConvertHelper.GetDateTime(key.Value), DateTime.Now) >= 0)
					{
						sublist.Add(key);
					}
				}
				newCarType.Append("<ul id=\"data_box3_1\" style=\"display:none\">");
				int showNewCarNum = 8;
				for (int i = 0; i < sublist.Count; i++)
				{
					int serialId = sublist[i].Key;
					if (serialId <= 0) break;
					string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");
					if (string.Equals(imgUrl, WebConfig.DefaultCarPic)) { showNewCarNum++; continue; }
					if (i >= showNewCarNum) break;
					string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(serialId));
					Car_SerialEntity cs = serialBll.Get_Car_SerialByCsID(serialId);
					string levelName = cs.Cs_CarLevel;
					switch (levelName)
					{
						case "紧凑型车":
							levelName = "紧凑型";
							break;
						case "中大型车":
							levelName = "中大型";
							break;
					}
					if (string.IsNullOrEmpty(levelName))
					{
						levelName = "紧凑型";
					}
					//int levelId = (int)System.Enum.Parse(typeof(EnumCollection.SerialAllLevelEnum), levelName);
					//string levelUrl = "/" + ((EnumCollection.SerialAllLevelSpellEnum)levelId).ToString() + "/";
					string levelUrl = string.Format("/{0}/", CarLevelDefine.GetLevelSpellByName(levelName));

					if (priceRange.Trim().Length == 0)
						priceRange = "暂无报价";
					newCarType.AppendFormat("<li><a id=\"newCsID_" + serialId + "\" href=\"{0}\" target=\"_blank\"><img src=\"{2}\" alt=\"{1}\" /></a><div class=\"title\"><a href=\"{0}\" target=\"_blank\">{1}</a></div>",
						"/" + cs.Cs_AllSpell + "/", cs.Cs_ShowName, imgUrl);
					//if (StringHelper.GetRealLength(cs.Cs_ShowName + "[" + levelName + "]") < 20)
					//    newCarType.AppendFormat("<a href=\"{0}\" target=\"_blank\" class=\"classify\">[{1}]</a>", levelUrl, levelName);
					newCarType.AppendFormat("<div class=\"txt{1}\">{0}</div></li>", priceRange,
						priceRange == "暂无报价" ? " huizi" : "");
				}
				newCarType.Append("</ul>");
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return newCarType.ToString();
		}

		/// <summary>
		/// 车型首页问答
		/// author:songcl date:2014-11-26
		/// </summary>
		/// <returns></returns>
		protected string GetAskHtml()
		{
			return _commonhtmlBLL.GetCommonHtmlByBlockId(1, CommonHtmlEnum.TypeEnum.Other, CommonHtmlEnum.TagIdEnum.CarDefault, CommonHtmlEnum.BlockIdEnum.Ask);
		}

		///// <summary>
		///// 得到热门车型
		///// </summary>
		///// <returns></returns>
		//protected string GetHotCarType()
		//{
		//    StringBuilder hotCarType = new StringBuilder();
		//    try
		//    {
		//        Car_SerialBll serial = new Car_SerialBll();
		//        hotCarType.Append(" <div class=\"line_box car-hotcar\">");
		//        hotCarType.Append("      <h3><span>推荐车型</span></h3>");
		//        hotCarType.Append("      <div class=\"h3_tab\">");
		//        hotCarType.Append("          <ul id=\"data_tab3\">");
		//        hotCarType.Append("            <li class=\"current\">热门</li>");
		//        hotCarType.Append("            <li class=\"\">新车</li>");
		//        hotCarType.Append("          </ul>");
		//        hotCarType.Append("        </div>");
		//        hotCarType.Append("      <div class=\"c0621_03\">");
		//        hotCarType.Append("        <ul id=\"data_box3_0\">");
		//        hotCarType.AppendLine(serial.GetHomepageHotSerial(10));
		//        hotCarType.Append("</ul>");
		//        Dictionary<int, string> dict = serial.GetAllSerialMarkDay();
		//        List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>(dict);
		//        List<KeyValuePair<int, string>> sublist = new List<KeyValuePair<int, string>>();
		//        foreach (KeyValuePair<int, string> key in list)
		//        {
		//            if (CommonFunction.DateDiff("d", ConvertHelper.GetDateTime(key.Value), DateTime.Now) >= 0)
		//            {
		//                sublist.Add(key);
		//            }
		//        }
		//        //List<KeyValuePair<int, string>> sublist = list.FindAll((d) =>
		//        //{
		//        //    return CommonFunction.DateDiff("d", ConvertHelper.GetDateTime(d.Value), DateTime.Now) >= 0;
		//        //});
		//        hotCarType.Append("<ul id=\"data_box3_1\" style=\"display:none\">");
		//        int showNewCarNum = 10;
		//        for (int i = 0; i < sublist.Count; i++)
		//        {
		//            int serialId = sublist[i].Key;
		//            if (serialId <= 0) break;
		//            string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId);
		//            if (string.Equals(imgUrl, WebConfig.DefaultCarPic)) { showNewCarNum++; continue; }
		//            if (i >= showNewCarNum) break;
		//            string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(serialId));
		//            Car_SerialEntity cs = serial.Get_Car_SerialByCsID(serialId);
		//            string levelName = cs.Cs_CarLevel;
		//            switch (levelName)
		//            {
		//                case "紧凑型车":
		//                    levelName = "紧凑型";
		//                    break;
		//                case "中大型车":
		//                    levelName = "中大型";
		//                    break;
		//            }
		//            if (string.IsNullOrEmpty(levelName))
		//            {
		//                levelName = "紧凑型";
		//            }
		//            int levelId = (int)System.Enum.Parse(typeof(EnumCollection.SerialAllLevelEnum), levelName);
		//            string levelUrl = "/" + ((EnumCollection.SerialAllLevelSpellEnum)levelId).ToString() + "/";
		//            if (priceRange.Trim().Length == 0)
		//                priceRange = "暂无报价";
		//            hotCarType.AppendFormat("<li><a id=\"hotCsID_" + serialId + "\" href=\"{0}\" target=\"_blank\"><img src=\"{2}\" width=\"120\" height=\"80\" alt=\"{1}\" /></a><a href=\"{0}\" target=\"_blank\">{1}</a>",
		//                "/" + cs.Cs_AllSpell + "/", cs.Cs_ShowName, imgUrl);
		//            if (StringHelper.GetRealLength(cs.Cs_ShowName + "[" + levelName + "]") < 20)
		//                hotCarType.AppendFormat("<a href=\"{0}\" target=\"_blank\" class=\"classify\">[{1}]</a>", levelUrl, levelName);
		//            hotCarType.AppendFormat("<span>{0}</span></li>", priceRange);
		//        }
		//        hotCarType.Append("</ul>");
		//        hotCarType.Append("      </div>");
		//        hotCarType.Append("      <div class=\"clear\"></div>");
		//        hotCarType.Append("    </div>");
		//        //string headerSpan = " <div class=\"line_box car-hotcar\"><h3>"
		//        //                + "<span>热门车型</span></h3>"
		//        //                + "<div class=\"c0621_03\"><ul>";
		//        //string footerSpan = "</ul></div><div class=\"clear\"></div></div>";

		//        ////string lastSerialHtml = "<li><em>10</em><a href=\"/mazida3/\" target=\"_blank\">"
		//        ////        + "<img src=\"http://gimg.bitauto.com/ResourceFiles/0/0/35/20101111112426287.jpg\" width=\"120\" height=\"80\" alt=\"\" /></a>"
		//        ////        + "<a href=\"/mazida3/\" target=\"_blank\">马自达3</a><a href=\"/jincouxingche/\" target=\"_blank\" class=\"classify\">[紧凑型]</a><span>10.16万-14.98万</span></li>";

		//        //hotCarType.AppendLine(headerSpan);
		//        //hotCarType.AppendLine(serial.GetHomepageHotSerial(10));
		//        ////hotCarType.AppendLine(lastSerialHtml);
		//        //hotCarType.AppendLine(footerSpan);
		//    }
		//    catch (Exception ex)
		//    {
		//        CommonFunction.WriteLog(ex.Message + ex.StackTrace);
		//    }
		//    return hotCarType.ToString();
		//}
		/// <summary>
		/// 得到价格区间的对象列表
		/// </summary>
		/// <returns></returns>
		private string GetPriceRangeList()
		{
			string priceName = "";
			string priceRange = "";

			StringBuilder priceString = new StringBuilder();
			priceString.AppendLine("<div id=\"data_box1_0\" style=\"display: block;\" class=\"rank-list-box-bg\">");
			priceString.Append("<div class=\"rank-list-box-b\">");
			for (int i = 0; i < _PriceRangeArrSSfi.Length; i++)
			{
				GetPriceRangeName(i, out priceName, out priceRange);
				priceString.AppendLine("<div class=\"rank-list-box\">");
				priceString.AppendLine(string.Format("<h5><a href=\"{0}?p={1}\" target=\"_self\">{2}</a></h5>", _SearchUrl, priceRange, priceName));
				priceString.AppendLine("<ol class=\"rank-list\">");
				//赋值车型
				if (_PriceRangeArrSSfi[i].Count > 0)
				{
					for (int j = 0; j < 10; j++)
					{
						if (j + 1 > _PriceRangeArrSSfi[i].Count) break;
						priceString.AppendLine(string.Format("<li><a href=\"/{0}/\" target=\"_blank\">{1}</a><span>{2}</span></li>", _PriceRangeArrSSfi[i][j].CsAllSpell.ToLower(), _PriceRangeArrSSfi[i][j].CsShowName, _PriceRangeArrSSfi[i][j].CsLevel));
					}
				}
				priceString.AppendLine("</ol>");
				priceString.AppendLine("</div>");
			}
			priceString.AppendLine("</div>");
			priceString.AppendLine("</div>");
			return priceString.ToString();
		}
		/// <summary>
		/// 得到级别列表
		/// </summary>
		/// <returns></returns>
		private string GetLevelList()
		{
			string levelName = "";
			string levelRange = "";
			string levelSpell = "";
			StringBuilder priceString = new StringBuilder();
			priceString.AppendLine("<div id=\"data_box1_1\" style=\"display: none;\" class=\"rank-list-box-bg\">");
			priceString.Append("<div class=\"rank-list-box-b\">");
			for (int i = 0; i < _LevelArrSSfi.Length; i++)
			{
				GetLevelName(i, out levelName, out levelRange, out levelSpell);
				priceString.AppendLine("<div class=\"rank-list-box\">");
				priceString.AppendLine(string.Format("<h5><a href=\"/{0}/\" target=\"_blank\">{1}</a></h5>", levelSpell, levelName));
				priceString.AppendLine("<ol class=\"rank-list\">");
				if (_LevelArrSSfi[i].Count > 0)
				{
					for (int j = 0; j < 10; j++)
					{
						if (j + 1 > _LevelArrSSfi[i].Count) break;
						string priceRange = new PageBase().GetSerialPriceRangeByID(_LevelArrSSfi[i][j].CsID);
						var minPrice = priceRange;
						if (string.IsNullOrEmpty(minPrice))
							minPrice = "暂无报价";
						if (minPrice.IndexOf('-') != -1)
							minPrice = minPrice.Substring(0, minPrice.IndexOf('-')) + "万起";
						priceString.AppendLine(string.Format("<li><a href=\"/{0}/\" target=\"_blank\">{1}</a><span>{2}</span></li>", _LevelArrSSfi[i][j].CsAllSpell.ToLower(), _LevelArrSSfi[i][j].CsShowName, minPrice));
					}
				}
				priceString.AppendLine("</ol>");
				priceString.AppendLine("</div>");
			}
			priceString.AppendLine("</div>");
			priceString.AppendLine("</div>");

			return priceString.ToString();
		}
		/// <summary>
		/// 得到价格区间
		/// </summary>
		private void GetPriceRangeName(int index, out string name, out string price)
		{
			switch (index)
			{
				case 0: name = "5万以下"; price = "0-5"; break;
				case 1: name = "5-8万"; price = "5-8"; break;
				case 2: name = "8-12万"; price = "8-12"; break;
				case 3: name = "12-18万"; price = "12-18"; break;
				case 4: name = "18-25万"; price = "18-25"; break;
				case 5: name = "25-40万"; price = "25-40"; break;
				case 6: name = "40-80万"; price = "40-80"; break;
				case 7: name = "80万以上"; price = "80-9999"; break;
				default: name = ""; price = ""; break;
			}
		}
		/// <summary>
		/// 得到级别
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private void GetLevelName(int index, out string name, out string param, out string spell)
		{
			switch (index)
			{
				case 0: name = "微型车"; param = "1"; spell = "weixingche"; break;
				case 1: name = "小型车"; param = "2"; spell = "xiaoxingche"; break;
				case 2: name = "紧凑型车"; param = "3"; spell = "jincouxingche"; break;
				case 3: name = "中型车"; param = "4"; spell = "zhongxingche"; break;
				case 4: name = "中大型车"; param = "5"; spell = "zhongdaxingche"; break;
				case 5: name = "豪华车"; param = "6"; spell = "haohuaxingche"; break;
				case 6: name = "SUV"; param = "7"; spell = "suv"; break;
				case 7: name = "MPV"; param = "8"; spell = "mpv"; break;
				case 8: name = "跑车"; param = "9"; spell = "paoche"; break;
				default: name = ""; param = ""; spell = ""; break;
			}
		}
	}
}