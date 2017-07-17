using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MWeb.Controllers
{
    public class SelectBrandController : Controller
    {
        //
        // GET: /SelectBrand/
		private Car_BrandBll carBrandBll = null;
		private PageBase pageBase = null;
		private bool isValidMasterBrand = true;

		public SelectBrandController()
		{
			carBrandBll = new Car_BrandBll();
			pageBase = new PageBase();
		}
		/// <summary>
		/// 主品牌列表
		/// </summary>
		/// <returns></returns>
		[OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream)]
		public ActionResult Index(int id)
        {
			RenderMasterBrand(id);
			if (!isValidMasterBrand)
			{
				Response.Redirect("/error", true);
				return new EmptyResult();
			}
			RenderBrandList(id);
            return View();
        }


		/// <summary>
		/// 获取主品牌数据
		/// </summary>
		private void RenderMasterBrand(int masterId)
		{
			string masterName = string.Empty;
			string masterSpell = string.Empty;
			DataRow drInfo = carBrandBll.GetCarMasterBrandInfoByBSID(masterId);
			if (drInfo != null)
			{
				masterName = drInfo["bs_name"].ToString().Trim();
				masterSpell = drInfo["urlspell"].ToString().Trim();
			}
			else
			{
				//Response.Redirect("/error", true);
				isValidMasterBrand = false;
			}
			ViewData["masterName"] = masterName;
			ViewData["masterSpell"] = masterSpell;
			ViewData["masterId"] = masterId;
		}

		/// <summary>
		/// 生成主品牌下各品牌的子品牌列表
		/// </summary>
		private void RenderBrandList(int masterId)
		{
			StringBuilder htmlCode = new StringBuilder();
			DataSet brandDs = carBrandBll.GetCarSerialPhotoListByBSID(masterId, true);
			if (brandDs != null && brandDs.Tables.Count > 0)
			{
				foreach (DataTable brandTable in brandDs.Tables)
				{
					if (brandTable.Rows.Count == 0)
					{
						continue;
					}
					htmlCode.AppendFormat("<div class=\"tt-small\" id=\"{1}\"><span>{0}</span></div>", brandTable.TableName, brandTable.Rows[0]["cbspell"].ToString());
					htmlCode.Append("<div class=\"pic-txt-h pic-txt-9060\">");
					htmlCode.Append(GetSerialHtml(brandTable));
					htmlCode.Append("</div>");
				}
				//_brandListHtml = htmlCode.ToString();
			}
			ViewData["BrandListHtml"] = htmlCode.ToString();
		}

		private string GetSerialHtml(DataTable brandTable)
		{
			StringBuilder serialList = new StringBuilder();
			StringBuilder htmlAllwaitSaleHtml = new StringBuilder();
			StringBuilder htmlAllnoPriceHtml = new StringBuilder();
			StringBuilder htmlAllwaitCheckHtml = new StringBuilder();
			StringBuilder htmlAllstopSaleHtml = new StringBuilder();
			string formatHtml = "<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p><strong>{3}</strong></p></a></li>";
			serialList.Append("<ul>");

			foreach (DataRow row in brandTable.Rows)
			{
				#region 不显示的子品牌
				string csLevel = ConvertHelper.GetString(row["cslevel"]);
				// if (csLevel == "概念车" || csLevel == "皮卡")
				if (csLevel == "概念车")
					continue;
				string csName = ConvertHelper.GetString(row["cs_name"]);
				if (csName.IndexOf("停用") >= 0)
				{ continue; }
				string sellState = ConvertHelper.GetString(row["CsSaleState"]);
				string imgUrl = ConvertHelper.GetString(row["csImageUrl"]).ToLower();
				// 无图片的
				if (sellState.Trim() == "停销" && imgUrl.IndexOf("150-100.gif") > 0)
				{ continue; }
				#endregion

				int serialId = ConvertHelper.GetInteger(row["cs_id"]);
				string csShowName = ConvertHelper.GetString(row["cs_ShowName"]);
				if (serialId == 1568)
				{ csShowName = "索纳塔八"; }
				string csSpell = ConvertHelper.GetString(row["csspell"]).Trim().ToLower();
				string priceRange = sellState;
				string serialUrl = "/" + csSpell + "/";

				if (sellState == "在销")
				{
					//priceRange = pageBase.GetSerialPriceRangeByID(serialId);
					priceRange = row["ReferPriceRange"].ToString();
					if (priceRange.Trim().Length == 0)
					{
						priceRange = "暂无指导价";
						htmlAllnoPriceHtml.AppendFormat(formatHtml, csSpell, imgUrl, csShowName, priceRange);
					}
					else
					{
						priceRange = priceRange + "万";
						serialList.AppendFormat(formatHtml, csSpell, imgUrl, csShowName, priceRange);
					}
				}
				else if (sellState == "待销")
				{
					priceRange = "未上市";
					htmlAllwaitSaleHtml.AppendFormat(formatHtml, csSpell, imgUrl, csShowName, priceRange);
				}
				else if (sellState == "待查")
				{
					priceRange = "暂无指导价";
					htmlAllwaitCheckHtml.AppendFormat(formatHtml, csSpell, imgUrl, csShowName, priceRange);
				}
				else
				{
					// 停销
					priceRange = "停售";
					htmlAllstopSaleHtml.AppendFormat(formatHtml, csSpell, imgUrl, csShowName, priceRange);
				}
			}
			serialList.Append(htmlAllnoPriceHtml.ToString());
			serialList.Append(htmlAllwaitSaleHtml.ToString());
			serialList.Append(htmlAllwaitCheckHtml.ToString());//待查
			serialList.Append(htmlAllstopSaleHtml.ToString());

			serialList.Append("</ul>");
			return serialList.ToString();
		}
    }
}
