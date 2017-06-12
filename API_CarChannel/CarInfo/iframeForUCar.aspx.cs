using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;

using BitAuto.CarChannel.BLL;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	public partial class _iframeForUCar : PageBase
	{
		#region Param
		protected string struCar = string.Empty;
		private int serialId = 0;
		private int cityId = 201;
		#endregion

		#region Load
		protected void Page_Load(object sender, EventArgs e)
		{
			//base.SetPageCache(20);
			//Response.ContentType = "application/x-javascript";
			//if (!IsPostBack)
			//{
			//	//检验参数
			//	CheckParam();
			//	GetUsedCarData();
			//}
		}
		#endregion

		#region Method

		/// <summary>
		/// 设置参数
		/// </summary>
		private void CheckParam()
		{
			serialId = ConvertHelper.GetInteger(Request.QueryString["serialid"]);
			cityId = ConvertHelper.GetInteger(Request.QueryString["cityid"]);
		}

		/// <summary>
		/// 获取二手车信息
		/// </summary>
		private void GetUsedCarData()
		{
			Car_SerialBll serialBll = new Car_SerialBll();
			XmlNode xmlNode = serialBll.GetUCarXml(serialId, cityId, 5);
			if (xmlNode.SelectNodes("./item").Count == 0)
			{
				struCar = string.Empty;
			}
			else
			{
				SerialEntity se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
				string seoName = string.Empty;
				if (serialId == 1568)
					seoName = "索纳塔八";
				else
					seoName = se.SeoName;
				StringBuilder makeUCarHtml = new StringBuilder();
				makeUCarHtml.Append("<div class=\"line_box ucar_box\">");
				makeUCarHtml.Append("<h3><a href=\"http://www.taoche.com/buycar/serial/" + se.AllSpell + "/\" target=\"_blank\">二手" + seoName + "</a></h3>");
				makeUCarHtml.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
				makeUCarHtml.Append("<tr><th width=\"16%\" style=\"text-align:left\">年份</th>");
				makeUCarHtml.Append("<th width=\"30%\">车源信息</th>");
				makeUCarHtml.Append("<th width=\"25%\">地区</th>");
				makeUCarHtml.Append("<th width=\"20%\">价格</th>");
				makeUCarHtml.Append("</tr>");
				XmlNodeList nodelist = xmlNode.SelectNodes("./item");
				foreach (XmlNode node in nodelist)
				{
					makeUCarHtml.Append("<tr>");
					makeUCarHtml.Append("<td style=\"text-align:left\">" + node.SelectSingleNode("BuyCarDate").InnerText + "</td>");
					makeUCarHtml.Append("<td style=\"text-align:left\"><a class=\"car_name\" href=\"" + node.SelectSingleNode("CarlistUrl").InnerText + "\" target=\"_blank\" title=\"" + node.SelectSingleNode("BrandName").InnerText + "\">" + node.SelectSingleNode("BrandName").InnerText + "</a></td>");
					makeUCarHtml.Append("<td class=\"cgray\"><a href=\"" + node.SelectSingleNode("CityUrL").InnerText + "\" target=\"_blank\">" + node.SelectSingleNode("CityName").InnerText + "</a></td>");
					makeUCarHtml.Append("<td class=\"ucar_price\">" + node.SelectSingleNode("DisplayPrice").InnerText + "</td>");
					makeUCarHtml.Append("</tr>");
				}
				makeUCarHtml.Append("</table>");
				makeUCarHtml.Append("</div>");
				struCar = makeUCarHtml.ToString();
			}

		}
		#endregion

	}
}