using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Model;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using System.Data;

namespace BitAuto.CarChannel.CarchannelWeb.PageLevelV2
{
    public partial class LevelPaihang : PageBase
    {
        protected int LevelId;
        //protected string CityName;
        //private string _citySpell;
       // private int _cityId;

        protected string LevelName = "";
        protected string LevelFullName = "";
        protected string LevelSpell = "";
        protected string LevelNavBarHtml = string.Empty;
        //protected string CityListHtml = string.Empty;
        protected string SerialsHtml = string.Empty;
        protected string LevelHtml = string.Empty;
        string[] _cityList;
        Dictionary<string, City> _cityDic;
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageCache(10);
            Getparameters();
            //_cityDic = AutoStorageService.GetCitySpellDic();
            //_cityList = new[] { "beijing", "shanghai", "guangzhou", "shenzhen", "fuzhou", "nanjing", "suzhou", "hangzhou", "ningbo", "hefei", "zhengzhou", "nanchang", "wuhan", "changsha", "chengdu", "chongqing", "kunming", "xian", "lanzhou", "taiyuan", "shijiazhuang", "jinan", "qingdao", "tianjin", "changchun", "shenyang", "dalian", "haerbin", "huhehaote" };
            RenderLevel();
            //RenderCityList();
            RenderSerialPaihang();
        }
        private void Getparameters()
        {
            LevelId = ConvertHelper.GetInteger(Request.QueryString["Level"]);
			//_citySpell = Request.QueryString["city"];
			//if (String.IsNullOrEmpty(_citySpell))
			//{
			//	CityName = "全国";
			//	_citySpell = "";
			//}
            LevelName = CarLevelDefine.GetLevelNameById(LevelId);
            LevelFullName = LevelName;
            if (LevelName == "紧凑型" || LevelName == "中大型")
                LevelFullName = LevelName + "车";
            LevelSpell = CarLevelDefine.GetLevelSpellById(LevelId);
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
                    spell, arr[i], spell == LevelSpell ? "active" : ""));
                else
                    list.Add(string.Format("<li class=\"{2}\"><a href=\"/{0}/paihang/\">{1}</a></li>",
                        spell, arr[i], spell == LevelSpell ? "active" : ""));
            }
            LevelHtml = string.Concat(list.ToArray());
        }


		private void RenderSerialPaihang()
		{
			DataSet ds = new Car_SerialBll().GetLevelSerialByUVAndSaleState(LevelFullName, null);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >0)
			{
				int counter = 0;
				StringBuilder htmlCode = new StringBuilder();
				const string baseUrl = "/";
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					counter++;
					string showName = dr["cs_ShowName"].ToString();
					string spell = dr["csAllSpell"].ToString();
					if (counter <= 3)
					{
						htmlCode.AppendFormat("<li><i class=\"hot\">{0}</i><a class=\"car\" href=\"{1}\" target=\"_blank\">{2}</a></li>", counter, baseUrl + spell + "/", showName);
					}
					else
					{
						htmlCode.AppendFormat("<li><i>{0}</i><a class=\"car\" href=\"{1}\" target=\"_blank\">{2}</a></li>", counter, baseUrl + spell + "/", showName);
					}
				}
				SerialsHtml = htmlCode.ToString();
			}
		}
		/*
        private void RenderCityList()
        {
            StringBuilder htmlCode = new StringBuilder();
            string baseUrl = "/" + LevelSpell + "/paihang/";
            if (_citySpell.Length == 0)
            {
                htmlCode.AppendLine("<li class=\"current\"><a href=\"javascript:\">全国</a></li>");
            }
            else
            {
                htmlCode.AppendLine("<li><a href=\"" + baseUrl + "\">全国</a></li>");
            }
            foreach (string spell in _cityList)
            {
                string tmpCityName = _cityDic[spell].CityName;
                if (spell == _citySpell)
                {
                    htmlCode.AppendLine("<li class=\"current\"><a href=\"javascript:\">" + tmpCityName + "</a></li>");
                    _cityId = _cityDic[spell].CityId;
                    CityName = tmpCityName;
                }
                else
                {
                    htmlCode.AppendLine("<li><a href=\"" + baseUrl + spell + "\">" + tmpCityName + "</a></li>");
                }
            }
            CityListHtml = htmlCode.ToString();
        }
		
        private void RenderSerialPaihang()
        {
            StringBuilder htmlCode = new StringBuilder();
            const string baseUrl = "/";
            XmlNodeList serialList = new Car_LevelBll().GetSerialPVSortByLevelAndCity(_cityId, LevelFullName);
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
                        htmlCode.AppendFormat("<li><i class=\"hot\">{0}</i><a class=\"car\" href=\"{1}\" target=\"_blank\">{2}</a></li>", counter, baseUrl + spell + "/", showName);
                    }
                    else
                    {
                        htmlCode.AppendFormat("<li><i>{0}</i><a class=\"car\" href=\"{1}\" target=\"_blank\">{2}</a></li>", counter, baseUrl + spell + "/", showName);
                    }
                }
            }
            SerialsHtml = htmlCode.ToString();
        }
		 * */
	}
}