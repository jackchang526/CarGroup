using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannel.CarchannelWeb.Interface
{
	/// <summary>
	/// 已经改成合作联盟js方式
	/// </summary>
	public partial class GetMasterDealer : PageBase
	{
		private int masterId;
		private string masterName;
		private string masterSpell;
		private Car_MasterBrandEntity mbe = new Car_MasterBrandEntity();

		protected void Page_Load(object sender, EventArgs e)
		{
			// base.SetPageCache(60);
			masterId = 0;
			masterName = String.Empty;
			masterSpell = String.Empty;
			GetParameters();
			string dealerHtml = "";// MakeMasterDealerHtml();
			Response.Write(dealerHtml);
			Response.End();
		}

		protected void GetParameters()
		{
			masterId = ConvertHelper.GetInteger(Request.QueryString["masterId"]);
			mbe = new Car_BrandBll().GetCarMasterBrandEntityByBsId(masterId);
			if (mbe != null)
			{
				masterName = mbe.Bs_Name;
				masterSpell = mbe.Bs_allSpell;
			}
		}

		/// <summary>
		/// 从Cookie中取城市ID
		/// </summary>
		/// <returns></returns>
		protected int GetCityId()
		{
			int cityId = ConvertHelper.GetInteger(Request.QueryString["city"]);
			if (cityId > 0)
				return cityId;
			else
				cityId = 201;
			if (Request.Cookies["bitauto_ipregion"] != null && Request.Cookies["bitauto_ipregion"].Value != "")
			{
				string cookieTemp = Server.UrlDecode(Request.Cookies["bitauto_ipregion"].Value);
				if (cookieTemp != "" && cookieTemp.IndexOf(";") > 0)
				{
					string[] arrCityInfo = cookieTemp.Split(';');
					if (arrCityInfo.Length > 1 && arrCityInfo[1].IndexOf(",") > 0)
					{
						string[] arrCityID = arrCityInfo[1].Split(',');
						if (arrCityID.Length > 2)
						{
							Int32.TryParse(arrCityID[0], out cityId);
						}
					}
				}
			}
			return cityId;
		}


		/// <summary>
		/// 主品牌经销商
		/// </summary>
		/// <returns></returns>
		protected string MakeMasterDealerHtml()
		{
			string returnHTML = "";
			int cityId = GetCityId();
			StringBuilder htmlTitle = new StringBuilder();
			StringBuilder htmlCode = new StringBuilder();
			DataSet brandDs = new Car_BrandBll().GetCarSerialPhotoListByBSID(masterId, true);
			if (brandDs != null && brandDs.Tables.Count > 0)
			{
				string onlyOneBrand = "";
				htmlTitle.AppendLine("<h3><span><a href=\"http://dealer.bitauto.com/" + masterSpell + "/\" target=\"_blank\">" + masterName + "-经销商推荐</a></span>");
				htmlTitle.AppendLine("<ul id=\"car_MasterDealer_tab_ul\" class=\"h_tab\">");
				int loop = 0;
				foreach (DataTable brandTable in brandDs.Tables)
				{
					string brandSpell = ConvertHelper.GetString(brandTable.Rows[0]["cbspell"]).Trim().ToLower();
					string brandName = brandTable.TableName.Trim();
					int brandId = ConvertHelper.GetInteger(brandTable.Rows[0]["cb_id"]);

					if (brandDs.Tables.Count == 1)
					{
						// 只有1个品牌 不显示标签
						onlyOneBrand = "<h3><span><a href=\"http://dealer.bitauto.com/" + brandSpell + "/\" target=\"_blank\">" + brandName + "-经销商推荐</a></span></h3>";
					}

					DataSet dsDealer = new PageBase().GetBrandCityDealerInfoByCbID(brandId, cityId);
					if (dsDealer != null && dsDealer.Tables.Count > 0 && dsDealer.Tables[0].Rows.Count > 0)
					{
						// 品牌有经销商数据
						if (loop == 0)
						{ htmlTitle.AppendLine("<li class=\"current\"><a href=\"http://dealer.bitauto.com/" + brandSpell + "/\" target=\"_blank\">" + brandName + "</a></li>"); }
						else
						{ htmlTitle.AppendLine("<li class=\"\"><a href=\"http://dealer.bitauto.com/" + brandSpell + "/\" target=\"_blank\">" + brandName + "</a></li>"); }

						// htmlCode.AppendLine("<h3><span><a target=\"_blank\" href=\"http://dealer.bitauto.com/" + brandSpell + "/\">" + brandName + "经销商</a></span></h3>");
						htmlCode.AppendLine("<div id=\"data_table_MasterDealer_" + loop.ToString() + "\" class=\"c\" style=\"" + (loop == 0 ? "" : "display: none;") + "\">");
						for (int i = 0; i < dsDealer.Tables[0].Rows.Count; i++)
						{
							if (i >= 6)
							{ break; }
							string vendorId = dsDealer.Tables[0].Rows[i]["vendorID"].ToString();
							string vendorName = "";
							string shortName = "";
							string nameTitle = "";

							vendorName = dsDealer.Tables[0].Rows[i]["vendorName"].ToString();
							shortName = StringHelper.SubString(vendorName, 32, true);
							if (vendorName != shortName)
							{ nameTitle = " title=\"" + vendorName + "\" "; }

							htmlCode.AppendLine("<ul>");
							htmlCode.AppendLine("<li>");
							htmlCode.AppendLine("<label>4S店：</label>");
							htmlCode.AppendLine("<a href=\"http://dealer.bitauto.com/" + vendorId + "/\" class=\"dn\" title=\"" + vendorName + "\" target=\"_blank\">" + shortName + "</a> </li>");
							htmlCode.AppendLine("<li>");
							htmlCode.AppendLine("<label>电话：</label>");

							// 400电话
							// string str400 = new PageBase().GetDealerFor400(vendorId);
							// 400 电话换成 CallCenterNumber 字段
							string str400 = dsDealer.Tables[0].Rows[i]["CallCenterNumber"].ToString();
							if (str400 != "")
							{
								htmlCode.Append("<span title=\"易车网认证电话，请放心拨打！\" class=\"official\">" + str400 + "</span></li>");
							}
							else
							{
								htmlCode.Append("<span>" + dsDealer.Tables[0].Rows[i]["vendorTel"].ToString() + "</span></li>");
							}

							// 经销商地址
							if (dsDealer.Tables[0].Columns.Contains("vendorSaleAddr"))
							{
								string addr = dsDealer.Tables[0].Rows[i]["vendorSaleAddr"].ToString().Trim();
								string shortAddr = addr;
								if (StringHelper.GetRealLength(addr) > 36)
								{ shortAddr = StringHelper.SubString(addr, 36, true); }
								addr = StringHelper.RemoveHtmlTag(addr);
								shortAddr = StringHelper.RemoveHtmlTag(shortAddr);
								string openMapJs = "window.open('http://dealer.bitauto.com/VendorMap/GoogleMap.aspx?dID=" + vendorId + "&S=S&W=400&H=300&Z=12','mapwindow','height=300,width=400,top=90,left=100,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no');return false;";
								htmlCode.AppendLine("<li><label>地址：</label><span class=\"dz\" title=\"" + addr + "\">" + shortAddr + "</span>[<a onclick=\"" + openMapJs + "\" href=\"#\">地图</a>]</li>");
							}
							else
							{ htmlCode.AppendLine("<li><label>地址：</label></li>"); }

							// 促销新闻
							if (dsDealer.Tables[0].Columns.Contains("newsurl") && dsDealer.Tables[0].Columns.Contains("newsTitle"))
							{
								htmlCode.Append("<li><label>促销：</label><a target=\"_blank\" href=\"http://dealer.bitauto.com" + dsDealer.Tables[0].Rows[i]["newsurl"].ToString() + "\">" + dsDealer.Tables[0].Rows[i]["newsTitle"].ToString() + "</a></li>");
							}
							else
							{
								htmlCode.Append("<li><label>促销：</label></li>");
							}

							htmlCode.AppendLine("</ul>");
						}
						htmlCode.AppendLine("<div class=\"hideline\"></div>");
						htmlCode.AppendLine("</div>");

						loop++;
					}
				}
				htmlTitle.AppendLine("</ul>");
				htmlTitle.AppendLine("</h3>");

				htmlCode.AppendLine("<div class=\"clear\"></div>");
				htmlCode.AppendLine("<div class=\"more\"><a target=\"_blank\" href=\"http://dealer.bitauto.com/" + masterSpell + "/\">更多&gt;&gt;</a></div>");

				if (brandDs.Tables.Count == 1)
				{ returnHTML = onlyOneBrand + htmlCode.ToString(); }
				else
				{ returnHTML = htmlTitle.ToString() + htmlCode.ToString(); }
			}
			return returnHTML;

		}
	}
}