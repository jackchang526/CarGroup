using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using System.Data;
using System.Text;

namespace H5Web.V3
{
	public partial class CarBrandPage : H5PageBase
	{
		private SerialFourthStageBll serialFourthStageBll = new SerialFourthStageBll();

		protected BrandEntity brandEntity = null;
		protected int BrandId = 0;
		protected string HtmlBrandToSerial = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			SetPageCache(30);
			GetParam();
			if (BrandId < 1)
			{
				Response.Redirect("http://car.h5.yiche.com/");
			}
			InitData();
			if (brandEntity == null)
			{
				Response.Redirect("http://car.h5.yiche.com/");
			}
			GetBrandToSerialInfo();
		}

		/// <summary>
		/// 初始化
		/// </summary>
		private void InitData()
		{
			brandEntity = (BrandEntity)DataManager.GetDataEntity(EntityType.Brand, BrandId);
		}

		private void GetParam()
		{
			BrandId = ConvertHelper.GetInteger(Request.QueryString["cbid"]);
		}

		private void GetBrandToSerialInfo()
		{
			List<CarSerialPhotoEntity> brandDs = new Car_BrandBll().GetCarSerialPhotoListByCBID(BrandId, false);
			if (brandDs != null && brandDs.Count > 0)
			{
				StringBuilder htmlCode = new StringBuilder();
				htmlCode.Append(GetSerialHtml(brandDs));
				HtmlBrandToSerial = htmlCode.ToString();
			}
		}


		private string GetSerialHtml(List<CarSerialPhotoEntity> brandTable)
		{
			StringBuilder htmlCode = new StringBuilder();

			StringBuilder serialList = new StringBuilder();
			StringBuilder htmlAllwaitSaleHtml = new StringBuilder();
			StringBuilder htmlAllnoPriceHtml = new StringBuilder();
			StringBuilder htmlAllstopSaleHtml = new StringBuilder();
			serialList.Append("<ul>");

			bool isShowBrand = false;//是否有要显示的子品牌
			foreach (CarSerialPhotoEntity csInfo in brandTable)
			{
				int serialId = csInfo.SerialId;

				#region 不显示的子品牌
				string csLevel = csInfo.SerialLevel;
				// if (csLevel == "概念车" || csLevel == "皮卡")
				if (csLevel == "概念车")
					continue;
				string csName = csInfo.CS_Name;
				if (csName.IndexOf("停用") >= 0)
				{ continue; }
				string sellState = csInfo.SaleState;
				string imgUrl = csInfo.CS_ImageUrl;
				// 无图片的
				if (sellState.Trim() == "停销" && imgUrl.IndexOf("150-100.gif") > 0)
				{ continue; }
				#endregion

				isShowBrand = true;

				string csShowName = csInfo.ShowName;
				string csSpell = csInfo.CS_AllSpell;
				string serialUrl = "/" + csSpell + "/";

				SerialEntity serialBrandEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
				string priceRange = serialBrandEntity.ReferPrice;
				if (sellState == "在销" || sellState == "待销")
				{
					htmlAllnoPriceHtml.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p>厂商指导价：{3}</p></a></li>", csSpell, imgUrl, csShowName, priceRange);
				}
				else
				{
					// 停销
					priceRange = "暂无";
					htmlAllstopSaleHtml.AppendFormat("<li><a href=\"/{0}/\"><img src=\"{1}\"/><h4>{2}</h4><p>厂商指导价：{3}</p></a></li>", csSpell, imgUrl, csShowName, priceRange);
				}
			}
			serialList.Append(htmlAllnoPriceHtml.ToString());
			serialList.Append(htmlAllwaitSaleHtml.ToString());
			serialList.Append(htmlAllstopSaleHtml.ToString());

			serialList.Append("</ul>");
			if (isShowBrand)
			{

				htmlCode.AppendFormat("<div class=\"tt-small\"><span>{0}</span></div>", brandEntity.Name);
				htmlCode.Append("<div class=\"pic-txt-h pic-txt-9060\">");
				htmlCode.Append(serialList);
				htmlCode.Append("</div>");
			}
			return htmlCode.ToString();
		}
	}
}