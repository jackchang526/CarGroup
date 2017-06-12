using System;
using System.Xml;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannel.CarchannelWeb.PageCar
{
	/// <summary>
	/// 车型图片页(无数据缓存,页面缓存30分钟)
	/// </summary>
	public partial class CarPhoto : PageBase
	{
		#region Param
		protected string CarPhotoHeadHTML = string.Empty;
		protected int CarID = 0;
		protected EnumCollection.CarInfoForCarSummary cfcs = new EnumCollection.CarInfoForCarSummary();
		protected Car_BasicEntity cbe = new Car_BasicEntity();
		protected CarEntity carEntity;
		//protected string CarClassString = string.Empty;
		//protected string Car12Pic = string.Empty;
		protected string CarClassAndPic = string.Empty;
		protected int CarPhotoCount = 0;
		protected string Serial12Pic = string.Empty;
		protected string CsClass = string.Empty;
		protected string CarList = string.Empty;
		//protected string SerialToSerial = string.Empty;
		protected int CsPhotoCount = 0;
		protected string CarYear = string.Empty;
		protected string UserBlock;
		protected string PhotoProvideCateHTML;
		protected string SerialHotCompare = string.Empty;

		protected string UCarHtml = string.Empty;
		protected string PhotoProvideColorRGBHTML = string.Empty;
		protected string CarPhotoProvideColorRGBHTML = string.Empty;
		protected string CarPhotoHtml = string.Empty;
		protected string serialToSeeHtml = string.Empty;
		protected string hotCarsHtml = string.Empty;
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
			if (!this.IsPostBack)
			{
				// 取参数
				GetParams();
				// 取车型数据
				GetCarData();
				GetCarPhotoHtml(CarID);
				#region  弃用之前调用方式 by sk 2013.03.21
				//#region 图库提供分类HTML
				//PhotoProvideCateHTML = new Car_SerialBll().GetPhotoProvideCateHTML(cbe.Cs_Id);

				//// add by chengl Oct.19.2011
				//// 颜色分类
				//List<EnumCollection.SerialColorItem> listESCI = new Car_SerialBll().GetPhotoSerialCarColorByCsID(cbe.Cs_Id);

				//if (listESCI != null && listESCI.Count > 0)
				//{
				//    // 车型颜色块
				//    List<string> listCarColorRGBHTML = new List<string>();
				//    // 子品牌颜色块
				//    List<string> listColorRGBHTML = new List<string>();
				//    listColorRGBHTML.Add("<div class=\"carPic02 carPic02_next\">");
				//    listColorRGBHTML.Add("<dl class=\"color\">");
				//    listColorRGBHTML.Add("<dt>按颜色：</dt>");
				//    listColorRGBHTML.Add("<dd class=\"w35\"><strong>不限</strong></dd>");
				//    listColorRGBHTML.Add("<dd class=\"w550\">");
				//    listColorRGBHTML.Add("<ul>");
				//    // 排重
				//    List<int> listHasColor = new List<int>();
				//    foreach (EnumCollection.SerialColorItem sci in listESCI)
				//    {
				//        if (!listHasColor.Contains(sci.ColorID))
				//        {
				//            listColorRGBHTML.Add("<li><a target=\"_blank\" href=\"http://photo.bitauto.com/serial/" + cbe.Cs_Id + "/c" + sci.ColorID + "/\"><em><span style=\"background:" + sci.ColorRGB + "\">" + sci.ColorName + "</span></em>" + sci.ColorName + "</a></li>");
				//            listHasColor.Add(sci.ColorID);
				//        }
				//        if (sci.CarID == CarID)
				//        {
				//            // 如果是当前车型
				//            listCarColorRGBHTML.Add("<li><a target=\"_blank\" href=\"http://photo.bitauto.com/model/" + CarID + "/c" + sci.ColorID + "/\"><em><span style=\"background:" + sci.ColorRGB + "\">" + sci.ColorName + "</span></em>" + sci.ColorName + "</a></li>");
				//        }
				//    }
				//    listColorRGBHTML.Add("</ul></dd></dl>");
				//    listColorRGBHTML.Add("<div class=\"clear\"></div>");
				//    listColorRGBHTML.Add("</div>");
				//    PhotoProvideColorRGBHTML = string.Concat(listColorRGBHTML.ToArray());

				//    if (listCarColorRGBHTML.Count > 0)
				//    {
				//        listCarColorRGBHTML.Insert(0, "<ul>");
				//        listCarColorRGBHTML.Insert(0, "<dd class=\"w550\">");
				//        listCarColorRGBHTML.Insert(0, "<dd class=\"w35\"><strong>不限</strong></dd>");
				//        listCarColorRGBHTML.Insert(0, "<dt>按颜色：</dt>");
				//        listCarColorRGBHTML.Insert(0, "<dl class=\"color\">");
				//        listCarColorRGBHTML.Insert(0, "<div class=\"carPic02 carPic02_next\">");

				//        listCarColorRGBHTML.Add("</ul></dd></dl>");
				//        listCarColorRGBHTML.Add("<div class=\"clear\"></div>");
				//        listCarColorRGBHTML.Add("</div>");
				//        CarPhotoProvideColorRGBHTML = string.Concat(listCarColorRGBHTML.ToArray());
				//    }
				//}
				//#endregion

				//// 取车型12张标准图
				//GetCar12Photo();
				//// 取子品牌12张标准图
				//GetSerial12Photo();
				//// 取子品牌分类及车型
				//GetSerialPhotoAndClass();
				#endregion
				// 子品牌还关注
				//MakeSerialToSerialHtml();
				ucSerialToSee.SerialId = cbe.Cs_Id;
				ucSerialToSee.SerialName = cbe.Cs_ShowName;
				// 车友和车主
				//GetUserBlockByCarSerialId();

				//GetSerialHotCompareCars();

				GetHotCar();

				// 车型图片导航头
				string subDir = Convert.ToString(CarID / 1000);
				// CarPhotoHeadHTML = base.GetCommonNavigation("CarPhoto\\" + subDir, CarID);
				CarPhotoHeadHTML = base.GetCommonNavigation("CarPhoto", CarID);
				// modified by sk 2013.03.21
				//PhotoProvideCateHTML = new Car_SerialBll().GetPhotoProvideCateHTML(cbe.Cs_Id);

				//UCarHtml = new Car_SerialBll().GetUCarHtml(cbe.Cs_Id);
			}
		}

		#region private Method
		// modified by sk 2013.03.21
		private void GetCarPhotoHtml(int carId)
		{
			CarPhotoHtml = Car_SerialBll.GetCarPhotoHtml(carId);
			if (string.IsNullOrEmpty(CarPhotoHtml))
			{
				//车型没有图片，取对应年款图片
				CarPhotoHtml = Car_SerialBll.GetSerialYearPhotoHtml(cbe.Cs_Id, cbe.Car_YearType);
				if (string.IsNullOrEmpty(CarPhotoHtml))
				{
					//车型对应年款图片还没有，取子品牌图片
					CarPhotoHtml = Car_SerialBll.GetSerialPhotoHtml(cbe.Cs_Id);
				}
				StringBuilder sb = new StringBuilder();
				//sb.Append("<div class=\"line_box\">");
				//sb.Append("				<!--选车结果-->");
				//sb.Append("				<div class=\"error_page\">");
				//sb.Append("					<h1>");
				//sb.Append("						很抱歉，该车型暂无图片！</h1>");
				//sb.Append("					<p>");
				//sb.Append("					</p>");
				//sb.Append("					<h4>");
				//sb.Append("						建议您：</h4>");
				//sb.Append("					<ul>");
				//sb.Append("						<li>我们正在努力更新，请查看其他...</li>");
				//sb.AppendFormat("						<li><a href=\"http://car.bitauto.com/{0}/\" target=\"_blank\">返回{1}车型页&gt;&gt;</a></li>", cbe.Cs_AllSpell, cbe.Cs_ShowName);
				//sb.Append("					</ul>");
				//sb.Append("				</div>");
				//sb.Append("				<div class=\"clear\">");
				//sb.Append("				</div>");
				//sb.Append("				<div class=\"more\" style=\"display: none;\">");
				//sb.Append("					<a href=\"javascript:\" id=\"carExplanation\" onclick=\"document.getElementById('carExplanationPopup').style.display='block';\">");
				//sb.Append("					</a>");
				//sb.Append("				</div>");
				//sb.Append("				<div class=\"popup-explanation\" id=\"carExplanationPopup\" style=\"display: none\">");
				//sb.Append("					<h6>");
				//sb.Append("						<span></span><a href=\"javascript:\" class=\"ico-close\" onclick=\"document.getElementById('carExplanationPopup').style.display='none';\">");
				//sb.Append("							关闭</a>");
				//sb.Append("					</h6>");
				//sb.Append("					<div class=\"popup-explanation-arrow\">");
				//sb.Append("					</div>");
				//sb.Append("					<p>");
				//sb.Append("					</p>");
				//sb.Append("				</div>");
				//sb.Append("			</div>");
				sb.Append("<div class=\"no-txt-box no-txt-m\">");
				sb.Append("");
				sb.Append("<p class=\"tit\">很抱歉，该车型暂无图片！</p>");
				sb.Append("<p>我们正在努力更新，请查看其他...</p>");
				sb.AppendFormat("<p><a href=\"http://car.bitauto.com/{0}/\" target=\"_blank\">返回{1}频道&gt;&gt;</a></p>",
					cbe.Cs_AllSpell, cbe.Cs_ShowName);
				sb.Append("");
				sb.Append("</div>");
				sb.Append(CarPhotoHtml);
				CarPhotoHtml = sb.ToString();
			}
		}
		/// <summary>
		/// 取车型参数
		/// </summary>
		private void GetParams()
		{
			if (this.Request.QueryString["CarID"] != null && this.Request.QueryString["CarID"].ToString() != "")
			{
				string tempCarID = this.Request.QueryString["CarID"].ToString();
				if (int.TryParse(tempCarID, out CarID))
				{ }
			}
		}

		/// <summary>
		/// 取车型数据
		/// </summary>
		private void GetCarData()
		{
			if (CarID > 0)
			{
				cfcs = base.GetCarInfoForCarSummaryByCarID(CarID);

				// modified by chengl Nov.9.2009
				if (cfcs.CarID <= 0)
				{
					Response.Redirect("/404error.aspx?info=无效车型");
				}
				// 子品牌信息
				cbe = (new Car_BasicBll()).Get_Car_BasicByCarID(CarID);
				if (cbe.Cs_Id <= 0)
				{
					Response.Redirect("/404error.aspx?info=无效车型所属子品牌");
				}
				carEntity = (CarEntity)DataManager.GetDataEntity(EntityType.Car, CarID);
				// 广告
				base.MakeSerialTopADCode(cbe.Cs_Id);
				CarYear = cbe.Car_YearType > 0 ? cbe.Car_YearType + "款 " : "";
			}
		}

		/// <summary>
		/// 取子品牌热门车型
		/// </summary>
		private void GetHotCar()
		{
			DataSet ds = base.GetHotCarInfoByCsID(cbe.Cs_Id);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					if (i >= 5)
					{ break; }
					hotCarsHtml += "<li><a title=\"" + cbe.Cs_ShowName + " " + ds.Tables[0].Rows[i]["car_name"].ToString() + "\" alt=\"" + cbe.Cs_ShowName + " " + ds.Tables[0].Rows[i]["car_name"].ToString() + "\" href=\"http://car.bitauto.com/" + cbe.Cs_AllSpell + "/m" + ds.Tables[0].Rows[i]["car_id"].ToString() + "/\" target=\"_blank\">";
					hotCarsHtml += cbe.Cs_ShowName + " " + ds.Tables[0].Rows[i]["car_name"].ToString() + "</a></li>";
				}
			}
		}

		/// <summary>
		/// 得到品牌下的其他子品牌
		/// </summary>
		/// <returns></returns>
		protected string GetBrandOtherSerial()
		{
			List<CarSerialPhotoEntity> carSerialPhotoList = new Car_BrandBll().GetCarSerialPhotoListByCBID(cbe.Cb_id, false);

			carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);

			if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
			{
				return "";
			}

			int forLastCount = 0;
			foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
			{
				if (entity.SerialLevel == "概念车" || entity.SerialId == cbe.Cs_Id)
				{
					continue;
				}
				forLastCount++;
			}

			StringBuilder contentBuilder = new StringBuilder(string.Empty);
			string serialUrl = "<a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}</a>";
			int index = 0;
			foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
			{
				bool IsExitsUrl = true;
				if (entity.SerialLevel == "概念车" || entity.SerialId == cbe.Cs_Id)
				{
					continue;
				}
				string priceRang = base.GetSerialIntPriceRangeByID(entity.SerialId);
				if (entity.SaleState == "待销")
				{
					IsExitsUrl = false;
					priceRang = "未上市";
				}
				else if (priceRang.Trim().Length == 0)
				{
					IsExitsUrl = false;
					priceRang = "暂无报价";
				}
				if (IsExitsUrl)
				{
					priceRang = string.Format("<a href='http://price.bitauto.com/brand.aspx?newbrandid={0}'>{1}</a>", entity.SerialId, priceRang);
				}
				string tempCsSeoName = string.IsNullOrEmpty(entity.CS_SeoName) ? entity.ShowName : entity.CS_SeoName;
				index++;
				contentBuilder.AppendFormat("<li>{0}<span class=\"dao\">{1}</span></li>"
					, string.Format(serialUrl, entity.CS_AllSpell, tempCsSeoName), priceRang
					 );
			}

			StringBuilder brandOtherSerial = new StringBuilder(string.Empty);
			if (contentBuilder.Length > 0)
			{
				brandOtherSerial.Append("<div class=\"side_title\">");
				brandOtherSerial.AppendFormat("<h4><a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}其他车型</a></h4>",
					carEntity.Serial.Brand.AllSpell, cbe.Cb_Name);
				brandOtherSerial.Append("</div>");

				brandOtherSerial.Append("<ul class=\"text-list\">");

				brandOtherSerial.Append(contentBuilder.ToString());

				brandOtherSerial.Append("</ul>");
			}

			return brandOtherSerial.ToString();
		}

		/// <summary>
		/// 取车型的12张标准图
		/// </summary>
		/// <returns></returns>
		private void GetCar12Photo()
		{
			StringBuilder sbCarClassAndPic = new StringBuilder();
			//StringBuilder sbCar12Pic = new StringBuilder();
			XmlDocument docCar12 = new Car_BasicBll().GetCar12Photo(cbe.Cs_Id, cbe.Car_Id);
			if (docCar12 != null && docCar12.HasChildNodes)
			{
				// 车型分类
				XmlNodeList xnlCarClass = docCar12.SelectNodes("/ImageData/CarImageAccount/Group");
				if (xnlCarClass != null && xnlCarClass.Count > 0)
				{
					sbCarClassAndPic.AppendLine("<div class=\"carPic02\"><dl id=\"carPhotoDL\" class=\"theme\"><dt>按主题：</dt><dd class=\"w35\"><strong>不限</strong></dd>");
					foreach (XmlNode xn in xnlCarClass)
					{
						sbCarClassAndPic.AppendLine("<dd><a target=\"_blank\" href=\"" + xn.Attributes["Link"].Value.ToString() + "\">" + xn.Attributes["GroupName"].Value.ToString() + "<span>(" + xn.Attributes["ImageCount"].Value.ToString() + ")</span></a></dd>");
						if (xn.Attributes["ImageCount"].Value.ToString() != "")
						{
							int count = 0;
							if (int.TryParse(xn.Attributes["ImageCount"].Value.ToString(), out count))
							{
								CarPhotoCount += count;
							}
						}
					}
					sbCarClassAndPic.AppendLine("</dl><div class=\"clear\"></div></div>");
					if (!string.IsNullOrEmpty(CarPhotoProvideColorRGBHTML))
					{ sbCarClassAndPic.AppendLine(CarPhotoProvideColorRGBHTML); }
				}
				// 车型12张标准图
				XmlNodeList xnl12Pic = docCar12.SelectNodes("/ImageData/ImageList/ImageInfo");
				if (xnl12Pic != null && xnl12Pic.Count > 0)
				{
					sbCarClassAndPic.AppendLine("<div class=\"c0603_05\">");
					sbCarClassAndPic.AppendLine("<ul>");
					foreach (XmlElement xn in xnl12Pic)
					{
						int imgId = ConvertHelper.GetInteger(xn.GetAttribute("ImageId"));
						int serverNum = imgId % 4 + 1;
						sbCarClassAndPic.AppendLine("<li><a href=\"" + xn.GetAttribute("Link") + "\" target=\"_blank\">");
						sbCarClassAndPic.AppendLine("<img alt=\"" + cbe.Cs_Name.Trim() + " " + CarYear + cbe.Car_Name.Trim() + " " + xn.GetAttribute("ImageName") + "\" src=\"http://img" + serverNum + ".bitautoimg.com/autoalbum/" + String.Format(xn.GetAttribute("ImageUrl"), 1) + "\"></a>");
						sbCarClassAndPic.AppendLine("<a href=\"" + xn.GetAttribute("Link") + "\" target=\"_blank\">" + xn.GetAttribute("ImageName") + "</a></li>");
					}
					sbCarClassAndPic.AppendLine("</ul>");
					sbCarClassAndPic.AppendLine("</div>");
				}
			}
			if (CarPhotoCount > 0)
			{
				// 有车型图片
				CarClassAndPic = sbCarClassAndPic.ToString() + "<div class=\"c0604_01\"><a target=\"_blank\" href=\"http://photo.bitauto.com/model/" + CarID.ToString() + "/\">查看更多&gt;&gt;</a></div>";
			}
			else
			{
				// 无车型图片
				CarClassAndPic = "<div class=\"c0604_03\">该型号无图，请查看其他……</div>";
			}
			if (sbCarClassAndPic.Length > 0)
			{
				sbCarClassAndPic.Remove(0, sbCarClassAndPic.Length);
			}
		}

		/// <summary>
		/// 取子品牌12张标准图
		/// </summary>
		private void GetSerial12Photo()
		{
			StringBuilder sbSerial12Pic = new StringBuilder();
			XmlDocument docSerial12Pic = new Car_SerialBll().GetSerial12Photo(cbe.Cs_Id);
			if (docSerial12Pic != null && docSerial12Pic.HasChildNodes)
			{
				XmlNodeList xmlCs12Pic = docSerial12Pic.SelectNodes("/ImageData/ImageList/ImageInfo");
				if (xmlCs12Pic != null && xmlCs12Pic.Count > 0)
				{
					foreach (XmlElement xn in xmlCs12Pic)
					{
						int imgId = ConvertHelper.GetInteger(xn.GetAttribute("ImageId"));
						int serverNum = imgId % 4 + 1;
						sbSerial12Pic.Append("<li><a href=\"" + xn.GetAttribute("Link") + "\" target=\"_blank\">");
						sbSerial12Pic.Append("<img alt=\"" + xn.GetAttribute("ImageName") + "\" src=\"http://img" + serverNum + ".bitautoimg.com/autoalbum/" + string.Format(xn.GetAttribute("ImageUrl"), 1) + "\"></a>");
						sbSerial12Pic.Append("<a href=\"" + xn.GetAttribute("Link") + "\" target=\"_blank\">" + xn.GetAttribute("ImageName") + "</a></li>");
					}
				}
				Serial12Pic = sbSerial12Pic.ToString();
				if (sbSerial12Pic.Length > 0)
				{
					sbSerial12Pic.Remove(0, sbSerial12Pic.Length);
				}
			}
		}

		/// <summary>
		/// 取子品牌的分类和车型汇总
		/// </summary>
		private void GetSerialPhotoAndClass()
		{
			StringBuilder sbCsClass = new StringBuilder();
			StringBuilder sbCarList = new StringBuilder();
			//图库接口本地化更改 by sk 2012.12.21
			string xmlPicUrl = System.IO.Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialPhotoListPath, cbe.Cs_Id));
			DataSet dsCsPic = this.GetXMLDocToDataSetByURLForCache("CarChannel_SerialAllPic_" + cbe.Cs_Id.ToString(), xmlPicUrl, 60);
			//DataSet dsCsPic = this.GetXMLDocToDataSetByURLForCache("CarChannel_SerialAllPic_" + cbe.Cs_Id.ToString(), string.Format(WebConfig.PhotoService, cbe.Cs_Id.ToString()), 60);
			// 子品牌分类
			if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("A") && dsCsPic.Tables["A"].Rows.Count > 0)
			{
				foreach (DataRow row in dsCsPic.Tables["A"].Rows)
				{
					sbCsClass.AppendLine("<a target=\"_blank\" href=\"http://photo.bitauto.com/serialmore/" + cbe.Cs_Id.ToString() + "/" + row["G"].ToString() + "/1/\">" + row["D"].ToString() + "<span>(" + row["N"].ToString() + "张)</span></a>");
					if (row["N"].ToString() != "")
					{
						int count = 0;
						if (int.TryParse(row["N"].ToString(), out count))
						{
							CsPhotoCount += count;
						}
					}
				}
				CsClass = sbCsClass.ToString();
				if (sbCsClass.Length > 0)
				{
					sbCsClass.Remove(0, sbCsClass.Length);
				}
			}
			// 子品牌下车型图片数
			if (dsCsPic != null && dsCsPic.Tables.Count > 0 && dsCsPic.Tables.Contains("B") && dsCsPic.Tables["B"].Rows.Count > 0)
			{
				DataSet CarInfo = base.GetCarIDAndNameForCSOrderByEYTP(cbe.Cs_Id, 10);
				if (CarInfo != null && CarInfo.Tables.Count > 0 && CarInfo.Tables[0].Rows.Count > 0)
				{
					int loopExhaust = 0;
					string Exhaust = "";
					for (int i = 0; i < CarInfo.Tables[0].Rows.Count; i++)
					{
						// 此车型是否在图库接口中
						DataRow[] drs = dsCsPic.Tables["B"].Select(" C ='" + CarInfo.Tables[0].Rows[i]["car_id"].ToString() + "' ");
						if (drs == null || drs.Length == 0)
						{ continue; }
						if (loopExhaust == 0)
						{
							// 第1个排量
							Exhaust = CarInfo.Tables[0].Rows[i]["Engine_Exhaust"].ToString().Trim();
							sbCarList.AppendLine("<dl>");
							sbCarList.AppendLine("<dt>" + Exhaust + "</dt>");
							sbCarList.AppendLine("<dd>");
							loopExhaust++;
						}
						else
						{
							if (Exhaust != CarInfo.Tables[0].Rows[i]["Engine_Exhaust"].ToString().Trim())
							{
								// 不同的排量
								Exhaust = CarInfo.Tables[0].Rows[i]["Engine_Exhaust"].ToString().Trim();
								sbCarList.AppendLine("</dd>");
								sbCarList.AppendLine("</dl>");
								sbCarList.AppendLine("<dl>");
								sbCarList.AppendLine("<dt>" + Exhaust + "</dt>");
								sbCarList.AppendLine("<dd>");

							}
							else
							{
								// 同排量的更多车型
								sbCarList.AppendLine("<span>|</span>");
							}
						}
						// 车型
						sbCarList.AppendLine("<a href=\"http://photo.bitauto.com/model/" + CarInfo.Tables[0].Rows[i]["car_id"].ToString() + "/\" target=\"_blank\">" + CarInfo.Tables[0].Rows[i]["Car_YearType"].ToString() + "款 " + CarInfo.Tables[0].Rows[i]["car_name"].ToString().Trim() + "<strong>(" + drs[0]["N"].ToString() + "张)</strong></a>");
					}
					if (loopExhaust > 0)
					{
						sbCarList.AppendLine("</dd>");
						sbCarList.AppendLine("</dl>");
					}
					CarList = sbCarList.ToString();
					if (sbCarList.Length > 0)
					{
						sbCarList.Remove(0, sbCarList.Length);
					}
				}
			}
		}

		/// <summary>
		/// 子品牌还关注
		/// </summary>
		/// <returns></returns>
		private void MakeSerialToSerialHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(cbe.Cs_Id, 6);
			if (lsts.Count > 0)
			{
				int loop = 0;
				foreach (EnumCollection.SerialToSerial sts in lsts)
				{
					string csName = sts.ToCsShowName.ToString();
					string shortName = StringHelper.SubString(csName, 12, true);
					if (shortName.StartsWith(csName))
						shortName = csName;

					loop++;
					htmlCode.Append("<li>");
					htmlCode.AppendFormat("<a target=\"_blank\" href=\"/{0}/\"><img src=\"{1}\" width=\"90\" height=\"60\"></a>",
						sts.ToCsAllSpell.ToString().ToLower(),
						 sts.ToCsPic.ToString());
					if (shortName != csName)
						htmlCode.AppendFormat("<p><a target=\"_blank\" href=\"/{0}/\" title=\"{1}\">{2}</a></p>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName, shortName);
					else
						htmlCode.AppendFormat("<p><a target=\"_blank\" href=\"/{0}/\">{1}</a></p>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName);
					htmlCode.AppendFormat("<p><span>{0}</span></p>", StringHelper.SubString(sts.ToCsPriceRange.ToString(), 14, false));
					htmlCode.AppendFormat("</li>");
				}
			}
			serialToSeeHtml = htmlCode.ToString();
		}

		/// <summary>
		/// 取子品牌相关用户
		/// </summary>
		private void GetUserBlockByCarSerialId()
		{
			StringBuilder sbUserBlock = new StringBuilder();
			// 计划购买
			DataTable dtWant = base.GetUserByCarSerialId(cbe.Cs_Id, 2, 3);
			if (dtWant != null && dtWant.Rows.Count > 0)
			{
				sbUserBlock.AppendLine("<div class=\"line_box zh_driver\">");
				sbUserBlock.AppendLine("<h3><span>和想买这款车的人聊聊</span></h3>");
				sbUserBlock.AppendLine("<div class=\"index_friend_r_l\">");
				sbUserBlock.AppendLine("<ul>");
				for (int i = 0; i < dtWant.Rows.Count; i++)
				{
					sbUserBlock.AppendLine("<li><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtWant.Rows[i]["userId"].ToString() + "/\" title=\"\">");
					sbUserBlock.AppendLine("<img height=\"60\" width=\"60\" src=\"" + dtWant.Rows[i]["userAvatar"].ToString() + "\" alt=\"\"></a>");
					sbUserBlock.AppendLine("<strong><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtWant.Rows[i]["userId"].ToString() + "/\">" + dtWant.Rows[i]["username"].ToString() + "</a></strong>");
					sbUserBlock.AppendLine("<p><a class=\"add_friend\" href=\"#\" onclick=\"javascript:AjaxAddFriend.show(" + dtWant.Rows[i]["userId"].ToString() + ", " + cbe.Cs_Id.ToString() + ");return false;\">加为好友</a></p></li>");
				}
				sbUserBlock.AppendLine("</ul>");
				sbUserBlock.AppendLine("</div><div class=\"clear\"> </div>");
				sbUserBlock.AppendLine("<div class=\"more\"><a target=\"_blank\" href=\"http://i.bitauto.com/FriendMore_c0_s" + cbe.Cs_Id.ToString() + "_p1_sort1_r010.html\">更多&gt;&gt;</a></div>");
				sbUserBlock.AppendLine("</div>");
			}
			// 车主
			DataTable dtOwner = base.GetUserByCarSerialId(cbe.Cs_Id, 3, 3);
			if (dtOwner != null && dtOwner.Rows.Count > 0)
			{
				sbUserBlock.AppendLine("<div class=\"line_box zh_driver\">");
				sbUserBlock.AppendLine("<h3><span>和车主聊聊</span></h3>");
				sbUserBlock.AppendLine("<div class=\"index_friend_r_l\">");
				sbUserBlock.AppendLine("<ul>");
				for (int i = 0; i < dtOwner.Rows.Count; i++)
				{
					sbUserBlock.AppendLine("<li><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtOwner.Rows[i]["userId"].ToString() + "/\" title=\"\">");
					sbUserBlock.AppendLine("<img height=\"60\" width=\"60\" src=\"" + dtOwner.Rows[i]["userAvatar"].ToString() + "\" alt=\"\"></a>");
					sbUserBlock.AppendLine("<strong><a target=\"_blank\" href=\"http://i.bitauto.com/u" + dtOwner.Rows[i]["userId"].ToString() + "/\">" + dtOwner.Rows[i]["username"].ToString() + "</a></strong>");
					sbUserBlock.AppendLine("<p><a class=\"add_friend\" href=\"#\" onclick=\"AjaxAddFriend.show(" + dtOwner.Rows[i]["userId"].ToString() + ", " + cbe.Cs_Id.ToString() + ",3);return false;\">加为好友</a></p></li>");
				}
				sbUserBlock.AppendLine("</ul>");
				sbUserBlock.AppendLine("</div><div class=\"clear\"> </div>");
				sbUserBlock.AppendLine("<div class=\"more\"><a target=\"_blank\" href=\"http://i.bitauto.com/FriendMore_c0_s" + cbe.Cs_Id.ToString() + "_p1_sort1_r001.html\">更多&gt;&gt;</a></div>");
				sbUserBlock.AppendLine("</div>");
			}
			UserBlock = sbUserBlock.ToString();
		}
		// 取子品牌图片对比
		private void GetSerialHotCompareCars()
		{
			StringBuilder sb = new StringBuilder();
			List<EnumCollection.SerialHotCompareData> lshcd = base.GetSerialHotCompareByCsID(cbe.Cs_Id, 5);
			if (lshcd.Count > 0)
			{
				sb.Append("<div class=\"line-box line-box_t0\" id=\"\">");
				sb.Append("<div class=\"side_title\">");
				sb.Append("<h4><a rel=\"nofollow\" href=\"/tupianduibi/\" target=\"_blank\">车型图片对比</a></h4>");
				sb.Append("</div>");


				sb.Append("<ul class=\"text-list\">");
				foreach (EnumCollection.SerialHotCompareData shcd in lshcd)
				{
					sb.AppendFormat("<li><a href=\"/tupianduibi/?csids={2},{3}\" target=\"_blank\">{0}<em class=\"vs\">VS</em>{1}</a></li>",
						cbe.Cs_ShowName, shcd.CompareCsShowName, shcd.CurrentCsID, shcd.CompareCsID);
				}
				sb.Append("</ul>");
				sb.Append("<div class=\"clear\"></div>");
				sb.Append("</div>");
			}
			SerialHotCompare = sb.ToString();
		}

		#endregion
	}
}