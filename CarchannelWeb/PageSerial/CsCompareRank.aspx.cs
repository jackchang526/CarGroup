using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.PageSerial
{
	public partial class CsCompareRank : PageBase
	{
		//子品牌对象
		protected Car_SerialEntity m_CSE;
		private Car_SerialBll m_CarSerialBll = new Car_SerialBll();
		protected int serialId;
		private string m_PvString;
		private string m_CompareRank;
		protected string CsHeadHTML = string.Empty;
		protected string csShowName;

		/// <summary>
		/// 关注度字符串
		/// </summary>
		public string PvString
		{
			get
			{
				return m_PvString;
			}
		}
		/// <summary>
		/// 全国对比详细
		/// </summary>
		public string CompareRank
		{
			get
			{
				return m_CompareRank;
			}
		}
		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			GetParams();
			base.MakeSerialTopADCode(serialId);
			InitPageContent();
			PrintCityPv();
			PrintCityCompareList();
		}

		//得到页面的参数
		private void GetParams()
		{
			if (string.IsNullOrEmpty(Request.QueryString["id"].ToString()))
			{
				Response.Redirect("http://car.bitauto.com/");
				return;
			}
			//得到子品牌ID
			serialId = ConvertHelper.GetInteger(Request.QueryString["id"].ToString());
			if (serialId <= 0)
			{
				Response.Redirect("http://car.bitauto.com/");
				return;
			}
		}
		/// <summary>
		/// 初始页面内容
		/// </summary>
		private void InitPageContent()
		{
			m_CSE = m_CarSerialBll.GetSerialInfoEntity(serialId);
			if (m_CSE == null || m_CSE.Cs_Id <= 0)
			{
				Response.Redirect("/404error.aspx?info=无效车型");
			}
			csShowName = m_CSE.Cs_ShowName;
			if (serialId == 1568)
				csShowName = "索纳塔八";
			//bool isSuccess = false;
			//CsHeadHTML = this.GetRequestString(string.Format(WebConfig.HeadForSerial, m_CSE.Cs_Id.ToString(), "CsPaiHang"), 10, out isSuccess);
			CsHeadHTML = base.GetCommonNavigation("CsPaiHang", m_CSE.Cs_Id);
		}
		/// <summary>
		/// 得到城市排列
		/// </summary>
		private void PrintCityPv()
		{

			Dictionary<string, int> cityPv = m_CarSerialBll.GetSerialAllCityNumber(serialId, HttpContext.Current);
			if (cityPv == null || cityPv.Count < 1)
			{
				return;
			}
			StringBuilder pvStrBuilder = new StringBuilder();

			pvStrBuilder.Append("<div class=\"title-con\">");
			pvStrBuilder.Append("<div class=\"title-box title-box2\">");
			pvStrBuilder.AppendFormat("<h4>{0}-{1}关注排行（近30天）</h4>", csShowName, m_CSE.Cs_CarLevel);
			pvStrBuilder.Append("<div class=\"more\"><ins id=\"div_542d453b-b855-4c98-ae2a-34384305dfa3\" type=\"ad_play\" adplay_IP=\"\" adplay_AreaName=\"\"  adplay_CityName=\"\"    adplay_BrandID=\"" +
				serialId.ToString() + "\"  adplay_BrandName=\"\"  adplay_BrandType=\"\"  adplay_BlockCode=\"542d453b-b855-4c98-ae2a-34384305dfa3\"> </ins></div>");

			pvStrBuilder.Append("</div>");
			pvStrBuilder.Append("</div>");


			pvStrBuilder.Append("<div class=\"text-list-box-b text-list-box-b-990\">");
			pvStrBuilder.Append("<div class=\"text-list-box\">");
			pvStrBuilder.Append("<ul class=\"text-list text-list-float text-list-float-990 text-list-float9-990\">");
			foreach (KeyValuePair<string, int> keyValue in cityPv)
			{
				pvStrBuilder.AppendFormat("<li><a href=\"javascript:;\">{0}第<strong>{1}</strong>名</a></li>", keyValue.Key.ToString(), keyValue.Value.ToString());
			}
			pvStrBuilder.Append("</ul>");
			pvStrBuilder.Append("</div>");
			pvStrBuilder.Append("</div>");
			m_PvString = pvStrBuilder.ToString();
		}
		/// <summary>
		/// 得到城市对比列表
		/// </summary>
		private void PrintCityCompareList()
		{
			Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = new Dictionary<string, List<Car_SerialBaseEntity>>();
			carSerialBaseList = m_CarSerialBll.GetSerialCityCompareList(serialId, HttpContext.Current);

			if (carSerialBaseList == null || carSerialBaseList.Count < 1)
			{
				return;
			}
			Dictionary<string, string> IsShowCityList = IsShowCity();
			StringBuilder cityCompareStrBuilder = new StringBuilder();

			cityCompareStrBuilder.Append("<div class=\"title-con\">");
			cityCompareStrBuilder.Append("<div class=\"title-box title-box2\">");
			cityCompareStrBuilder.AppendFormat("<h4>{0}-网友都和谁对比（近30天）</h4>", csShowName);
			cityCompareStrBuilder.Append("<div class=\"more\"><a href=\"http://car.bitauto.com/chexingduibi/\" target=\"_blank\">进入车型对比>></a></div>");
			cityCompareStrBuilder.Append("</div>");
			cityCompareStrBuilder.Append("</div>");

			cityCompareStrBuilder.Append("<div class=\"rank-list-box-bg rank-list-box-bg990_4\">");
			cityCompareStrBuilder.Append("<div class=\"rank-list-box-b\">");

			int loop = 0;
			foreach (KeyValuePair<string, List<Car_SerialBaseEntity>> keyValue in carSerialBaseList)
			{
				if (!IsShowCityList.ContainsKey(keyValue.Key))
				{
					continue;
				}

				cityCompareStrBuilder.Append("<div class=\"rank-list-box\">");
				cityCompareStrBuilder.AppendFormat("<h5><a href=\"#\">{0}</a></h5>", keyValue.Key);
				cityCompareStrBuilder.Append("<ol class=\"rank-list\">");
				foreach (Car_SerialBaseEntity carSerial in keyValue.Value)
				{
					cityCompareStrBuilder.AppendFormat("<li><a href=\"http://car.bitauto.com/{0}/\" target=\"_blank\">{1}</a><span>{2}</span></li>",
						carSerial.SerialNameSpell,
						carSerial.SerialShowName,
						carSerial.SerialPrice);
				}
				cityCompareStrBuilder.Append("</ol>");
				cityCompareStrBuilder.Append("</div>");

				//if (loop % 4 == 0) { cityCompareStrBuilder.Append("<div class=\"h-line\"></div>"); }
				//loop++;
				//string width = "";
				//switch (loop % 4)
				//{
				//    case 1: width = "w230"; break;
				//    case 2: width = "w230"; break;
				//    case 3: width = "w231"; break;
				//    case 0: width = "w231"; break;
				//    default: break;
				//}
				//cityCompareStrBuilder.Append("<div class=\" a4 " + width + "\"><h3><span>" + keyValue.Key + "</span></h3><ol class=\"hot_ranking\">");
				//foreach (Car_SerialBaseEntity carSerial in keyValue.Value)
				//{
				//    /*
				//     * 参数1:子品牌全拼
				//     * 参数2:子品牌显示名
				//     * 参数3:子品牌报价
				//     */
				//    cityCompareStrBuilder.AppendFormat("<li><a href=\"http://car.bitauto.com/{0}/\" target=\"_blank\">{1}</a><span>{2}</span></li>"
				//                                       , carSerial.SerialNameSpell, carSerial.SerialShowName, carSerial.SerialPrice);
				//}

				//cityCompareStrBuilder.Append("</ol></div>");
			}
			cityCompareStrBuilder.Append("<div class=\"clear\"></div>");
			cityCompareStrBuilder.Append("</div>");
			cityCompareStrBuilder.Append("</div>");
			m_CompareRank = cityCompareStrBuilder.ToString();
		}
		/// <summary>
		/// 要显示数据的29个城市
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, string> IsShowCity()
		{
			Dictionary<string, string> IsShowCityList = new Dictionary<string, string>();
			IsShowCityList.Add("全国", "");
			IsShowCityList.Add("北京", "");
			IsShowCityList.Add("呼和浩特", "");
			IsShowCityList.Add("天津", "");
			IsShowCityList.Add("济南", "");
			IsShowCityList.Add("青岛", "");
			IsShowCityList.Add("石家庄", "");
			IsShowCityList.Add("太原", "");
			IsShowCityList.Add("长春", "");
			IsShowCityList.Add("沈阳", "");
			//IsShowCityList.Add("大连", "");
			IsShowCityList.Add("哈尔滨", "");
			IsShowCityList.Add("上海", "");
			IsShowCityList.Add("合肥", "");
			IsShowCityList.Add("南京", "");
			IsShowCityList.Add("苏州", "");
			IsShowCityList.Add("杭州", "");
			IsShowCityList.Add("福州", "");
			IsShowCityList.Add("深圳", "");
			IsShowCityList.Add("广州", "");
			IsShowCityList.Add("武汉", "");
			IsShowCityList.Add("南昌", "");
			IsShowCityList.Add("郑州", "");
			IsShowCityList.Add("长沙", "");
			IsShowCityList.Add("成都", "");
			IsShowCityList.Add("重庆", "");
			IsShowCityList.Add("昆明", "");
			IsShowCityList.Add("西安", "");
			IsShowCityList.Add("兰州", "");

			return IsShowCityList;
		}
	}
}