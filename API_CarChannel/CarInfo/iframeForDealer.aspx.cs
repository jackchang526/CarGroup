using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BitAuto.CarChannel.Common;
using BitAuto.Beyond.Caching.RefreshCache;
using BitAuto.Utils;

using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	public partial class _iframeForDealer : PageBase
	{
		#region Param
		protected int csID = 0;
		protected int cityId = 0;
		private StringBuilder sb = new StringBuilder();
		protected string serialDealer = string.Empty;
		protected string csShowName = string.Empty;
		protected string csSeoName = string.Empty;
		protected string cbID = string.Empty;
		private ArrayList hasDealer = new ArrayList();
		protected string csAllSpell = string.Empty;
		private string strThead = string.Empty;
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);
			Response.ContentType = "application/x-javascript";
			strThead = "<thead><tr>"
				+ "<th width=\"19%\">经销商</th>"
				+ "<th width=\"34%\">电话</th>"
				+ "<th width=\"40%\">促销信息</th>"
				+ "</tr></thead>";

			if (!this.IsPostBack)
			{
				CheckParams();
				if (csID > 0)
					GetUserCookieZone();
			}
		}

		// 检查参数
		private void CheckParams()
		{
			csID = ConvertHelper.GetInteger(Request.QueryString["csId"]);
			cityId = ConvertHelper.GetInteger(Request.QueryString["cityId"]);
		}


		private void GetUserCookieZone()
		{
			DataSet ds = new DataSet();


			RefreshCache rc = new RefreshCache();
			ds = rc.GetCacheData(cityId, csID);



			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				try
				{
					serialDealer = MakeDealerHtml(ds.Tables[0]);
				}
				catch
				{ }
			}
			SerialEntity se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csID);
			csAllSpell = se.AllSpell;
			csShowName = se.ShowName;
			csSeoName = se.SeoName;
			if (csID == 1568)
			{
				csShowName = "索纳塔八";
			}
			//if (serialDealer.Trim().Length == 0)
			//{
				//if (se.SaleState == "待销")
					//serialDealer = "<tr><td class=\"noline\" colspan=\"3\">" + se.SeoName + "尚未上市，我们会在第一时间进行更新，请您时刻保持关注</td></tr>";
				//    else
				//        serialDealer = "<tr><td class=\"noline\" colspan=\"3\">对不起！没有符合条件的经销商！</td></tr>";
			//}
		}



		// 循环拼DataTable字符串
		private string MakeDealerHtml(DataTable dt)
		{
			List<string> htmlList = new List<string>();
			if (dt.Columns.Contains("vendorID") && dt.Columns.Contains("vendorName") && dt.Columns.Contains("vendorFullName"))
			{
				string vendorName = "";
				string shortName = "";
				string nameTitle = "";
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					nameTitle = "";
					string vendorId = dt.Rows[i]["vendorID"].ToString();
					htmlList.Add("<ul>");

					if (dt.Columns.Contains("vendorName"))
					{
						vendorName = dt.Rows[i]["vendorName"].ToString();
						shortName = StringHelper.SubString(vendorName, 16, true);
						if (vendorName != shortName)
							nameTitle = " title=\"" + vendorName + "\" ";
					}
					else
					{
						vendorName = dt.Rows[i]["vendorFullName"].ToString();
						shortName = StringHelper.SubString(vendorName, 16, true);
						if (vendorName != shortName)
							nameTitle = " title=\"" + vendorName + "\" ";
					}
					htmlList.Add("<li><label>4S店：</label><a href=\"http://dealer.bitauto.com/" + vendorId + "/\" class=\"dn\" title=\"" + nameTitle + "\" target=\"_blank\">" + shortName + "</a>");
					// modified by chengl Apr.25.2011
					htmlList.Add("</li>");
					// htmlCode.AppendLine("<a class=\"dealer_com\" target=\"_self\" onclick=\"showDealerInfo(this," + vendorId + ");return false;\" href=\"javascript:void(0);\">经销商名片</a></li>");
					// add 400 for dealer
					string str400 = base.GetDealerFor400(vendorId);
					if (str400 != "")
					{
						htmlList.Add("<li><label>电话：</label><span title=\"易车网认证电话，请放心拨打！\" class=\"official\">" + str400 + "</span></li>");
					}
					else
					{
						htmlList.Add("<li><label>电话：</label>" + dt.Rows[i]["vendorTel"].ToString() + "</li>");
					}
					if (dt.Columns.Contains("vendorSaleAddr"))
					{
						string addr = dt.Rows[i]["vendorSaleAddr"].ToString().Trim();
						string shortAddr = addr;
						if (StringHelper.GetRealLength(addr) > 36)
							shortAddr = StringHelper.SubString(addr, 36, true);
						addr = StringHelper.RemoveHtmlTag(addr);
						shortAddr = StringHelper.RemoveHtmlTag(shortAddr);
						string openMapJs = "window.open('http://dealer.bitauto.com/VendorMap/GoogleMap.aspx?dID=" + vendorId + "&S=S&W=400&H=300&Z=12','mapwindow','height=300,width=400,top=90,left=100,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no');return false;";
						htmlList.Add("<li><label>地址：</label><span class=\"dz\" title=\"" + addr + "\">" + shortAddr + "</span>[<a onclick=\"" + openMapJs + "\" href=\"#\">地图</a>]</li>");
					}
					else
						htmlList.Add("<li><label>地址：</label></li>");
					if (dt.Columns.Contains("newsurl") && dt.Columns.Contains("newsTitle"))
					{
						string newsTitle = dt.Rows[i]["newsTitle"].ToString();
						string shortTitle = newsTitle;
						if (StringHelper.GetRealLength(newsTitle) > 36)
							shortTitle = StringHelper.SubString(newsTitle, 36, true);
						newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
						shortTitle = StringHelper.RemoveHtmlTag(shortTitle);
						htmlList.Add("<li><label>促销：</label><a href=\"http://dealer.bitauto.com" + dt.Rows[i]["newsurl"].ToString() + "\" target=\"_blank\" title=\"" + newsTitle + "\">" + shortTitle + "</a></li>");
					}
					htmlList.Add("</ul>");
				}
			}
			string dealerHtml = String.Concat(htmlList.ToArray());
			if (dealerHtml.Length > 0)
				dealerHtml += "<div class=\"hideline\"></div>";
			return dealerHtml.Replace("'", "\\'");
		}
	}
}