using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Text;
using BitAuto.CarChannel.BLL.Data;
using Newtonsoft.Json;
using System.Data;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;
using System.IO;
using BitAuto.CarUtils.Define;
using BitAuto.CarChannel.BLL.CarTreeData;
using BitAuto.Utils;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Web.UI;

namespace MWeb.Controllers
{
	public class SelectCarController : Controller
	{
		int pageNum = 1;
		int pageSize = 20;
		protected string titleHtml = string.Empty;
		private SelectCarParameters selectParas;
		protected string adCarListData;
		protected string serialListHtml = string.Empty;
		protected int SerialNum = 0;
		protected int CarNum = 0;
		protected int pageCount = 0;
		private string sortMode;
		private string sortModeHtml;
		protected string ClearConsitionHtml = string.Empty;
		protected string CarTabText = string.Empty;
		protected string metaHtml = string.Empty;
		protected string _SearchUrl = string.Empty;
		
		[OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream)]
		public ActionResult Index()
		{
			selectParas = GetSelectCarParas(string.Empty);
			InitData(0);
			return View();
		}

		[OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream)]
		public ActionResult Level(string level)
		{
			int l = (int)(SerialLevelSpellEnum)Enum.Parse(typeof(SerialLevelSpellEnum), level);
			selectParas = GetSelectCarParas(l.ToString());
			InitData(l);
			return View("/Views/SelectCar/Index.cshtml");
		}

		protected void InitData(int level)
		{
			//搜索地址 
			_SearchUrl = InitSearchUrl("chexing");
			string queryString = Request.Url.Query.ToLower();
			//排序
			sortMode = Request.QueryString["s"];
			if (sortMode == "2")
			{
				sortMode = "price_up";
				sortModeHtml = "<li><a href=\"javascript:GotoPage('s1');\">按关注度</a></li><li><a href=\"javascript:GotoPage('s3');\">最贵</a></li><li class=\"current\"><a href=\"javascript:GotoPage('s2');\">最便宜</a></li>";
			}
			else if (sortMode == "3")
			{
				sortMode = "price_down";
				sortModeHtml = "<li><a href=\"javascript:GotoPage('s1');\">按关注度</a></li><li class=\"current\"><a href=\"javascript:GotoPage('s3');\">最贵</a></li><li><a href=\"javascript:GotoPage('s2');\">最便宜</a></li>";
			}
			else
			{
				sortMode = "guanzhu_down";
				sortModeHtml = "<li class=\"current\"><a href=\"javascript:GotoPage('s1');\">按关注度</a></li><li class=\"arrow\"><a href=\"javascript:GotoPage('s3');\">最贵</a></li><li class=\"arrow\"><a href=\"javascript:GotoPage('s2');\">最便宜</a></li>";
			}



			titleHtml = "<title>【选车工具|选车中心_汽车车型大全】-手机易车网</title>";
			titleHtml += "<meta name=\"keywords\" content=\"选车,选车工具,易车网\" />";
			titleHtml += "<meta name=\"description\" content=\"选车工具:易车网车型频道为您提供按条件选车工具,包括按汽车价格、汽车级别、国产进口、变速方式、汽车排量等方式选择适合您的喜欢的汽车品牌……\" />";
			ViewData["titleHtml"] = titleHtml;
			ViewData["_SearchUrl"] = _SearchUrl;
			ViewData["adCarListData"] = adCarListData;

			var searchscript = GenerateSearchInitScript(level);
			ViewData["GenerateSearchInitScript"] = searchscript;

			InitSelectCarAD();
		}


		protected SelectCarParameters GetSelectCarParas(string level)
		{
			SelectCarParameters selectParas = new SelectCarParameters();
			string conditionStr = "";

			string price = Request.QueryString["p"];
			string tempLevel = Request.QueryString["l"];
			if (!string.IsNullOrWhiteSpace(tempLevel))
			{
				level = tempLevel;
			}
			string brandType = Request.QueryString["g"];
			string countryType = Request.QueryString["c"];
			string sweptVolume = Request.QueryString["d"];
			string transmissionType = Request.QueryString["t"];
			string driveType = Request.QueryString["dt"];
			string fuelType = Request.QueryString["f"];
			string isWagon = Request.QueryString["lv"];
			string bodyForm = Request.QueryString["b"];
			string configMore = Request.QueryString["more"];
			conditionStr = makeCondtionStrForAdXml(conditionStr, "p", price);
			conditionStr = makeCondtionStrForAdXml(conditionStr, "l", level);
			conditionStr = makeCondtionStrForAdXml(conditionStr, "g", brandType);
			conditionStr = makeCondtionStrForAdXml(conditionStr, "c", countryType);
			conditionStr = makeCondtionStrForAdXml(conditionStr, "d", sweptVolume);
			conditionStr = makeCondtionStrForAdXml(conditionStr, "t", transmissionType);
			conditionStr = makeCondtionStrForAdXml(conditionStr, "dt", driveType);
			conditionStr = makeCondtionStrForAdXml(conditionStr, "f", fuelType);
			conditionStr = makeCondtionStrForAdXml(conditionStr, "lv", isWagon);
			conditionStr = makeCondtionStrForAdXml(conditionStr, "b", bodyForm);
			conditionStr = makeCondtionStrForAdXml(conditionStr, "more", configMore);
			selectParas.ConditionString = conditionStr;
			return selectParas;
		}
		public string makeCondtionStrForAdXml(string condition, string key, string value)
		{
			string result = string.Empty;
			if (!String.IsNullOrEmpty(value))
			{
				condition += string.Format("#{0}={1}", key, value);
			}
			if (!String.IsNullOrEmpty(condition))
			{
				result = condition;
			}
			return result;
		}
		protected string InitSearchUrl(string tagType)
		{
			_SearchUrl = String.Empty;
			Dictionary<string, TagData> tagDic = TagData.GetTagDataDictionary();
			if (tagDic.ContainsKey(tagType))
				_SearchUrl = tagDic[tagType].UrlDictionary["search"].UrlRule;
			int position = _SearchUrl.IndexOf('?');
			if (position >= 0)
				_SearchUrl = _SearchUrl.Remove(position, _SearchUrl.Length - position);
			return _SearchUrl;
		}


		private void InitSelectCarAD()
		{
			//int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
			List<SuperSerialInfo> adCarList = new List<SuperSerialInfo>();
			List<SerialListADEntity> listSerialAD = new Car_SerialBll().GetSerialAD("selectcar" + selectParas.ConditionString);
			if (listSerialAD != null && listSerialAD.Count > 0)
			{
				foreach (SerialListADEntity serialAd in listSerialAD)
				{
					int index = serialAd.Pos - 1;
					if (index < 0)
						index = 0;

					SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialAd.SerialId);
					if (serialEntity != null)
					{
						adCarList.Add(new SuperSerialInfo(serialAd.SerialId, serialEntity.ShowName, serialEntity.AllSpell)
						{
							Pos = serialAd.Pos,
							//CarIdList = string.Join(",", serialEntity.CarList.Select(p => p.Id)),
							ImageUrl = Car_SerialBll.GetSerialImageUrl(serialAd.SerialId, "1"),
							PriceRange = serialEntity.Price,
							CarNum = serialEntity.CarList.Length
						});
					}
				}
			}

			adCarListData = adCarList.Count > 0 ? JsonConvert.SerializeObject(adCarList) : "[]";
		}

		protected readonly string[] querys = { "p", "l", "d", "g", "c", "t", "dt", "f", "b", "lv", "more" };
		Regex regexQurey = new Regex(@"^(\d+((\.\d+)?(-|_)\d+(\.\d+)?)?)+$", RegexOptions.IgnoreCase);

		/// <summary>
		/// 验证 字符串参数有效性 1.3-1.6 、200、200_300
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		protected bool IsMatchQueryString(string query)
		{
			if (string.IsNullOrEmpty(query)) return false;
			//string regexString = @"^(\d+((\.\d+)?(-|_)\d+(\.\d+)?)?)+$";
			//Regex r = new Regex(regexString, RegexOptions.IgnoreCase);
			return regexQurey.IsMatch(query);
		}
		public string GenerateSearchInitScript(int l)
		{
			StringBuilder scriptCode = new StringBuilder();
			string tmpStr = Request.QueryString["p"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("SelectCarTool.Price='" + tmpStr + "';");
			//int l = ConvertHelper.GetInteger(Request.QueryString["l"]);
			if (l == 0)
			{
				l = ConvertHelper.GetInteger(Request.QueryString["l"]);
			}
			if (l > 0)
				scriptCode.AppendLine("SelectCarTool.Level=" + l + ";");
			tmpStr = Request.QueryString["d"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("SelectCarTool.Displacement='" + tmpStr + "';");
			int t = ConvertHelper.GetInteger(Request.QueryString["t"]);
			if (t > 0)
				scriptCode.AppendLine("SelectCarTool.TransmissionType=" + t + ";");
			tmpStr = Request.QueryString["more"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("SelectCarTool.SetMoreCondition('" + tmpStr + "');");
			int g = ConvertHelper.GetInteger(Request.QueryString["g"]);
			if (g > 0)
				scriptCode.AppendLine("SelectCarTool.Brand=" + g + ";");
			int c = ConvertHelper.GetInteger(Request.QueryString["c"]);
			if (c > 0)
				scriptCode.AppendLine("SelectCarTool.Country=" + c + ";");
			int b = ConvertHelper.GetInteger(Request.QueryString["b"]);
			if (b > 0)
				scriptCode.AppendLine("SelectCarTool.BodyForm=" + b + ";");
			int lv = ConvertHelper.GetInteger(Request.QueryString["lv"]);
			if (lv > 0)
				scriptCode.AppendLine("SelectCarTool.IsWagon=" + lv + ";");
			int dt = ConvertHelper.GetInteger(Request.QueryString["dt"]);
			if (dt > 0)
				scriptCode.AppendLine("SelectCarTool.DriveType=" + dt + ";");
			int f = ConvertHelper.GetInteger(Request.QueryString["f"]);
			if (f > 0)
				scriptCode.AppendLine("SelectCarTool.FuelType=" + f + ";");
			//tmpStr = Request.QueryString["sn"];
			//if (!String.IsNullOrEmpty(tmpStr))
			//    scriptCode.AppendLine("SelectCarTool.PerfSeatNum='" + tmpStr + "';");
			tmpStr = Request.QueryString["v"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("SelectCarTool.View=" + tmpStr + ";");
			int s = ConvertHelper.GetInteger(Request.QueryString["s"]);
			if (s > 0)
				scriptCode.AppendLine("SelectCarTool.Sort=" + s + ";");
			tmpStr = Request.QueryString["e"];
			tmpStr = IsMatchQueryString(tmpStr) ? tmpStr : string.Empty;
			if (!String.IsNullOrEmpty(tmpStr))
				scriptCode.AppendLine("SelectCarTool.Envelope=" + tmpStr + ";");
			return scriptCode.ToString();
		}

	}
	public class SuperSerialInfo
	{
		private int m_pvNum;

		public SuperSerialInfo(int id, string showName, string spell)
		{
			SerialId = id;
			ShowName = showName;
			AllSpell = spell;
			//m_carIdList = ",";
		}

		public int Pos { get; set; }

		public int MasterId { get; set; }
		public int SerialId { get; set; }

		public string ShowName { get; set; }

		public string AllSpell { get; set; }

		public string CarIdList { get; set; }

		//public List<CarInfoForSerialSummaryEntity> CarList { get; set; }
		/// <summary>
		///     符合条件的车的数量
		/// </summary>
		public int CarNum { get; set; }

		public string ImageUrl { get; set; }

		public string PriceRange { get; set; }

	}
}
