using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCCB = BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.PageList
{
	public partial class CarMasterBrandGuideALL : PageBase
	{
		private string mMaoDianList = "";
		private string mMasterBrandLogoList = "";
		private char[] mAlphabetical = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'Y', 'Z' };
		private Dictionary<string, string[]> mCountryName;
		private Dictionary<char, int> mAlphaDic;
		/// <summary>
		/// 毛点列表
		/// </summary>
		public string MaoDianList
		{
			get
			{
				return mMaoDianList;
			}
		}
		/// <summary>
		/// 主品牌LOGO地址
		/// </summary>
		public string MasterBrandLogoList
		{
			get
			{
				return mMasterBrandLogoList;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			#region Alpha
			//初始化字母列表
			//InitAlphaDicList();
			//加载主品牌列表
			//LoadMasterAphlaList();
			//加载毛点列表
			//LoadAphlaList();
			#endregion

			#region Country
			//初始化系别列表
			initCountryList();
			//初始化主品牌列表
			LoadMasterBrandList();
			#endregion
		}

		#region AphlaQuene
		/// <summary>
		/// 初始化字母列表
		/// </summary>
		private void InitAlphaDicList()
		{
			mAlphaDic = new Dictionary<char, int>();
			foreach (char c in mAlphabetical)
			{
				mAlphaDic.Add(c, 0);
			}
		}
		/// <summary>
		/// 加载主品牌列表
		/// </summary>
		public void LoadMasterAphlaList()
		{
			Dictionary<char, Dictionary<int, string[]>> masterBrandDic = new Dictionary<char, Dictionary<int, string[]>>();
			masterBrandDic = new BCCB.Car_BrandBll().GetMasterBrandListByXML();

			if (masterBrandDic == null || masterBrandDic.Count < 1)
			{
				mMasterBrandLogoList = "";
			}
			string sUrl = "";
			string sImageUrl = "";
			StringBuilder htmlBuilder = new StringBuilder("");

			foreach (KeyValuePair<char, Dictionary<int, string[]>> objectMasterBrand in masterBrandDic)
			{
				htmlBuilder.Append("<div class=\"col-all spic\">");
				htmlBuilder.Append("<div class=\"line_box\">");
				htmlBuilder.Append("<h3><span><a name=\"" + objectMasterBrand.Key + "\" id=\"" + objectMasterBrand.Key + "\"></a>" + objectMasterBrand.Key + "</span></h3>");
				htmlBuilder.Append("<div class=\"pp_auto\">");
				htmlBuilder.Append("<ul class=\"list_pic\">");

				foreach (KeyValuePair<int, string[]> objectMaster in objectMasterBrand.Value)
				{
					sUrl = "http://car.bitauto.com/" + objectMaster.Value[1].ToLower() + "/";
					sImageUrl = "http://image.bitautoimg.com/bt/car/default/images/carimage/m_" + objectMaster.Key + "_100.jpg";
					htmlBuilder.AppendFormat("<li><a href=\"{0}\" title=\"一汽轿车\" target=\"_blank\">"
											+ "<img width=\"100px\" height=\"100px\" src=\"{1}\" /></a>"
											+ "<a href=\"{0}\" target=\"_blank\">{2}</a></li>"
											, sUrl, sImageUrl, objectMaster.Value[0]);
				}
				//设置列表包含的主品牌
				if (mAlphaDic.ContainsKey(objectMasterBrand.Key))
				{
					mAlphaDic[objectMasterBrand.Key] = 1;
				}
				htmlBuilder.Append("</ul>");
				htmlBuilder.Append("<div class=\"clear\"></div>");
				htmlBuilder.Append("</div>");
				htmlBuilder.Append("</div>");
				htmlBuilder.Append("</div>");
			}
			mMasterBrandLogoList = htmlBuilder.ToString();

		}
		/// <summary>
		/// 加载主品牌列表
		/// </summary>
		public void LoadAphlaList()
		{
			int index = 1;
			StringBuilder htmlBuilder = new StringBuilder();
			foreach (KeyValuePair<char, int> keyValueObject in mAlphaDic)
			{
				if (keyValueObject.Value == 1 && index == mAlphaDic.Count)
				{
					htmlBuilder.AppendFormat("<li class=\"last\"><a href=\"#{0}\">{0}</a></li>", keyValueObject.Key);
				}
				else if (keyValueObject.Value == 0 && index == mAlphaDic.Count)
				{
					htmlBuilder.AppendFormat("<li class=\"last nolink\">{0}</li>", keyValueObject.Key);
				}
				else if (keyValueObject.Value == 1)
				{
					htmlBuilder.AppendFormat("<li><a href=\"#{0}\">{0}</a></li>", keyValueObject.Key);
				}
				else if (keyValueObject.Value == 0)
				{
					htmlBuilder.AppendFormat("<li class=\"nolink\">{0}</li>", keyValueObject.Key);
				}
				index++;
			}

			mMaoDianList = htmlBuilder.ToString();
		}
		#endregion

		#region CountryQuene
		/// <summary>
		/// 初始化城市列表
		/// </summary>
		private void initCountryList()
		{
			mCountryName = new Dictionary<string, string[]>();
			string[] CountryArray;
			CountryArray = new string[] { "德国" };
			mCountryName.Add("德系品牌", CountryArray);
			CountryArray = new string[] { "日本", "韩国" };
			mCountryName.Add("日韩品牌", CountryArray);
			CountryArray = new string[] { "美国" };
			mCountryName.Add("美系品牌", CountryArray);
			CountryArray = new string[] { "法国", "英国", "意大利", "瑞典", "捷克" };
			mCountryName.Add("欧系其他", CountryArray);
			CountryArray = new string[] { "中国" };
			mCountryName.Add("自主品牌", CountryArray);

			mMaoDianList = "<li><a href=\"#自主品牌\">自主品牌</a></li><li><a href=\"#德系品牌\">德系品牌</a></li>"
						+ "<li><a href=\"#日韩品牌\">日韩品牌</a></li><li><a href=\"#美系品牌\">美系品牌</a></li>"
						+ "<li class=\"last\"><a href=\"#欧系其他\">欧系其他</a></li>";
		}
		/// <summary>
		/// 加载主品牌列表
		/// </summary>
		private void LoadMasterBrandList()
		{
			Dictionary<string, Dictionary<int, string[]>> masterBrandList = new Dictionary<string, Dictionary<int, string[]>>();
			masterBrandList = new BCCB.Car_BrandBll().GetCountryMasterBrandListByXML();
			if (masterBrandList == null || masterBrandList.Count < 1)
			{
				mMasterBrandLogoList = "";
				return;
			}

			string sUrl = "";
			string sImageUrl = "";

			StringBuilder htmlBuilder = new StringBuilder("");
			foreach (KeyValuePair<string, string[]> keyValueCountry in mCountryName)
			{
				htmlBuilder.Append("<div class=\"col-all spic\"><a name=\"" + keyValueCountry.Key + "\" id=\"" + keyValueCountry.Key + "\"></a>");
				htmlBuilder.Append("<div class=\"line_box\">");
				htmlBuilder.Append("<h3><span>" + keyValueCountry.Key + "</span></h3>");
				htmlBuilder.Append("<div class=\"pp_auto\">");
				htmlBuilder.Append("<ul class=\"list_pic\">");

				foreach (string s in keyValueCountry.Value)
				{
					if (!masterBrandList.ContainsKey(s))
					{
						continue;
					}
					foreach (KeyValuePair<int, string[]> masterBrandObject in masterBrandList[s])
					{
						sUrl = "http://car.bitauto.com/" + masterBrandObject.Value[1].ToLower() + "/";
						sImageUrl = "http://image.bitautoimg.com/bt/car/default/images/carimage/m_" + masterBrandObject.Key + "_100.jpg";
						htmlBuilder.AppendFormat("<li><a href=\"{0}\" title=\"{2}\" target=\"_blank\">"
												+ "<img width=\"100px\" height=\"100px\" alt=\"{2}\" src=\"{1}\" /></a>"
												+ "<a href=\"{0}\" target=\"_blank\">{2}</a></li>"
												, sUrl, sImageUrl, masterBrandObject.Value[0]);
					}
				}

				htmlBuilder.Append("</ul>");
				htmlBuilder.Append("<div class=\"clear\"></div>");
				htmlBuilder.Append("</div>");
				htmlBuilder.Append("</div>");
				htmlBuilder.Append("</div>");

			}
			mMasterBrandLogoList = htmlBuilder.ToString();

		}
		#endregion
	}
}