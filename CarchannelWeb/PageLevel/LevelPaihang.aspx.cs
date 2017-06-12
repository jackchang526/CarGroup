using System;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.BLL;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarUtils.Define;

namespace BitAuto.CarChannel.CarchannelWeb.PageLevel
{
	public partial class LevelPaihang : PageBase
	{
		protected int levelId;
		protected string cityName;
		private string citySpell;
		private int cityId;

		protected string levelName = "";
		protected string levelFullName = "";
		protected string levelSpell = "";
		protected string levelNavBarHtml = string.Empty;
		protected string cityListHtml = string.Empty;
		protected string serialsHtml = string.Empty;
		protected string levelHtml = string.Empty;
		string[] cityList;
		Dictionary<string, City> cityDic;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			Getparameters();
			cityDic = AutoStorageService.GetCitySpellDic();
			cityList = new string[] { "beijing", "shanghai", "guangzhou", "shenzhen", "fuzhou", "nanjing", "suzhou", "hangzhou", "ningbo", "hefei", "zhengzhou", "nanchang", "wuhan", "changsha", "chengdu", "chongqing", "kunming", "xian", "lanzhou", "taiyuan", "shijiazhuang", "jinan", "qingdao", "tianjin", "changchun", "shenyang", "dalian", "haerbin", "huhehaote" };
			// levelNavBarHtml = new Car_LevelBll().RenderNavBar(levelId, "paihang");
			RenderLevel();
			RenderCityList();
			RenderSerialPaihang();
		}

		private void Getparameters()
		{
			levelId = ConvertHelper.GetInteger(Request.QueryString["Level"]);
			citySpell = Request.QueryString["city"];
			if (String.IsNullOrEmpty(citySpell))
			{
				cityName = "全国";
				citySpell = "";
			}
 			levelName = CarLevelDefine.GetLevelNameById(levelId);
 			levelFullName = levelName;
			if (levelName == "紧凑型" || levelName == "中大型")
				levelFullName = levelName + "车";
			levelSpell = CarLevelDefine.GetLevelSpellById(levelId);
		}

		private void RenderLevel()
		{
			List<string> list = new List<string>();
			string[] arr = { "微型车", "小型车", "紧凑型车", "中型车", "中大型车", "豪华车", "MPV", "SUV", "跑车", "面包车" };
			for (var i = 0; i < arr.Length; i++)
			{
				var spell = CarLevelDefine.GetLevelSpellByName(arr[i]);
				if (i == arr.Length - 1)
					list.Add(string.Format("<li class=\"last {2}\"><a href=\"/{0}/paihang/\">{1}</a></li>",
					spell, arr[i], spell == levelSpell ? "current" : ""));
				else
					list.Add(string.Format("<li class=\"{2}\"><a href=\"/{0}/paihang/\">{1}</a></li>",
						spell, arr[i], spell == levelSpell ? "current" : ""));
			}
			levelHtml = string.Concat(list.ToArray());
		}



		private void RenderCityList()
		{
			StringBuilder htmlCode = new StringBuilder();
			string baseUrl = "/" + levelSpell + "/paihang/";
			if (citySpell.Length == 0)
				htmlCode.AppendLine("<dd class=\"current\">全国</dd>");
			else
				htmlCode.AppendLine("<dd><a href=\"" + baseUrl + "\">全国</a></dd>");
			foreach (string spell in cityList)
			{
				string tmpCityName = cityDic[spell].CityName;
				if (spell == citySpell)
				{
					htmlCode.AppendLine("<dd class=\"current\">" + tmpCityName + "</dd>");
					cityId = cityDic[spell].CityId;
					cityName = tmpCityName;
				}
				else
				{
					htmlCode.AppendLine("<dd><a href=\"" + baseUrl + spell + "/\">" + tmpCityName + "</a></dd>");
				}
			}
			cityListHtml = htmlCode.ToString();
		}

		private void RenderSerialPaihang()
		{
			StringBuilder htmlCode = new StringBuilder();
			string baseUrl = "/";
			XmlNodeList serialList = new Car_LevelBll().GetSerialPVSortByLevelAndCity(cityId, levelFullName);
			if (serialList != null)
			{
				int counter = 0;
				foreach (XmlElement serialNode in serialList)
				{
					counter++;
					string showName = serialNode.GetAttribute("ShowName");
					string spell = serialNode.GetAttribute("AllSpell");
					if (counter <= 3)
					{
						htmlCode.AppendFormat("<li class=\"fist_three\"><em>{0}</em><a href=\"{1}\" target=\"_blank\">{2}</a></li>",
							counter, baseUrl + spell + "/", showName);
					}
					else
					{
						htmlCode.AppendFormat("<li><em>{0}</em><a href=\"{1}\" target=\"_blank\">{2}</a></li>", counter, baseUrl + spell + "/", showName);
					}
				}

			}
			serialsHtml = htmlCode.ToString();

		}
	}
}