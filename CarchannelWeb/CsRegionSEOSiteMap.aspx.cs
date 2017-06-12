using System;
using System.Xml;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannel.CarchannelWeb
{
	/// <summary>
	/// SEO城市站站点地图(squid端重写 http://*.bitauto.com/sitemap.html)
	/// </summary>
	public partial class CsRegionSEOSiteMap : PageBase
	{
		private string interfaceCMS = "http://api.admin.bitauto.com/api/list/newstocar.aspx?clientKey=car&cityid={0}&getcount={1}";
		private string citySpell = string.Empty;
		private int cityId = 0;
		protected string cityName = string.Empty;

		protected string SerialList = string.Empty;
		protected string CityNews = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.GetPageParam();
				if (cityId > 0)
				{
					this.GetCityAllSaleSerial();
					this.GetCityNewByCityID();
				}
			}
		}

		/// <summary>
		/// 取页面参数
		/// </summary>
		private void GetPageParam()
		{
			if (this.Request.QueryString["city"] != null && this.Request.QueryString["city"].ToString() != "")
			{
				citySpell = this.Request.QueryString["city"].ToString().Trim().ToLower();
			}

			Dictionary<string, City> cityDic = AutoStorageService.GetCitySpellDic();
			if (cityDic.ContainsKey(citySpell))
			{
				cityId = cityDic[citySpell].CityId;
				cityName = cityDic[citySpell].CityName;
			}
		}

		/// <summary>
		/// 取在销子品牌 生成城市SiteMap
		/// </summary>
		private void GetCityAllSaleSerial()
		{
			//获取数据xml
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();
			XmlNodeList serialList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");

			if (serialList != null && serialList.Count > 0)
			{
				StringBuilder sb = new StringBuilder();
				foreach (XmlNode xn in serialList)
				{
					sb.AppendLine("<li><a href=\"http://" + citySpell + ".bitauto.com/car/" + xn.Attributes["AllSpell"].Value.ToLower() + "/\">");
					sb.Append(xn.Attributes["ShowName"].Value + "报价及图片</a></li>");
				}
				SerialList = sb.ToString();
			}
		}

		/// <summary>
		/// 根据城市ID取城市新闻
		/// </summary>
		private void GetCityNewByCityID()
		{
			try
			{
				DataSet ds = new DataSet();
				ds.ReadXml(string.Format(interfaceCMS, cityId, 20));
				if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
				{
					if (ds.Tables[1].Columns.Contains("title") && ds.Tables[1].Columns.Contains("facetitle") && ds.Tables[1].Columns.Contains("filepath"))
					{
						StringBuilder sb = new StringBuilder();
						foreach (DataRow dr in ds.Tables[1].Rows)
						{
							sb.AppendLine("<li><a href=\"" + dr["filepath"].ToString().Trim() + "\" target=\"_blank\" title=\"" + dr["title"].ToString().Trim() + "\">");
							sb.Append(dr["facetitle"].ToString().Trim() + "</a></li>");
						}
						CityNews = sb.ToString();
					}
				}
			}
			catch
			{ }
		}
	}
}