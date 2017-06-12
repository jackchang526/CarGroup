using System;
using System.Data;
using System.Xml;
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
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface
{
	public partial class GetSerialOverview1 : InterfacePageBase
	{
		private int serialId;
		private int noSeeAlso;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(20);
			GetParameter();
			Response.ContentType = "Text/XML";
			Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			try
			{
				Response.Write(GetSerialOverviewXml());
			}
			catch (System.Exception ex)
			{
				Response.Write("<SerialOverview />");
			}
			Response.End();
		}

		private void GetParameter()
		{
			Int32.TryParse(Request.QueryString["SerialId"], out serialId);
			Int32.TryParse(Request.QueryString["NoSeeAlso"], out noSeeAlso);
		}

		private string GetSerialOverviewXml()
		{
			XmlDocument xmlDoc = new XmlDocument();
			XmlElement root = xmlDoc.CreateElement("SerialOverview");
			xmlDoc.AppendChild(root);
			// 		XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
			// 		xmlDoc.InsertBefore(xmlDec, root);
			if (serialId > 0)
			{
				EnumCollection.SerialInfoCard sic = new Car_SerialBll().GetSerialInfoCard(serialId);
				root.SetAttribute("SerialId", serialId.ToString());
				root.SetAttribute("AllSpell", sic.CsAllSpell);
				root.SetAttribute("Name", sic.CsName);
				root.SetAttribute("ShowName", sic.CsShowName);
				//报价
				XmlElement priceRangeNode = xmlDoc.CreateElement("PriceRange");
				root.AppendChild(priceRangeNode);
				string serialPrice = sic.CsPriceRange;
				if (serialPrice.Length == 0)
					serialPrice = "暂无报价";
				priceRangeNode.InnerText = serialPrice;

				//指导价
				XmlElement officalPriceNode = xmlDoc.CreateElement("OfficalPrice");
				root.AppendChild(officalPriceNode);
				List<EnumCollection.CarInfoForSerialSummary> ls = base.GetAllCarInfoForSerialSummaryByCsID(serialId);
				double maxPrice = Double.MinValue;
				double minPrice = Double.MaxValue;
				foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
				{
					double referPrice = 0.0;
					bool isDouble = Double.TryParse(carInfo.ReferPrice.Replace("万", ""), out referPrice);
					if (isDouble)
					{
						if (referPrice > maxPrice)
							maxPrice = referPrice;
						if (referPrice < minPrice)
							minPrice = referPrice;
					}
				}
				string serialReferPrice = "";
				if (maxPrice == Double.MinValue && minPrice == Double.MaxValue)
					serialReferPrice = "暂无";
				else
				{
					serialReferPrice = minPrice + "万-" + maxPrice + "万";
				}
				officalPriceNode.InnerText = serialReferPrice;

				//颜色
				XmlElement colorNode = xmlDoc.CreateElement("Color");
				root.AppendChild(colorNode);
				string colorList = "";
				foreach (string colorStr in sic.ColorList)
				{
					if (colorList.Length > 0)
						colorList += ",";
					colorList += colorStr;
				}
				colorNode.InnerText = colorList;

				// modifed by chengl Oct.22.2010
				XmlElement colorRGB = xmlDoc.CreateElement("ColorRGB");
				root.AppendChild(colorRGB);
				string colorRGBstr = "";
				DataSet dsAllColor = new Car_SerialBll().GetAllSerialColorRGB();
				if (dsAllColor != null && dsAllColor.Tables.Count > 0 && dsAllColor.Tables[0].Rows.Count > 0)
				{
					List<string> rgb = new List<string>();
					DataRow[] drs = dsAllColor.Tables[0].Select(" cs_id='" + serialId.ToString() + "' ");
					if (drs != null && drs.Length > 0)
					{
						foreach (DataRow dr in drs)
						{
							if (sic.ColorList.Contains(dr["colorName"].ToString().Trim()))
							{
								if (!rgb.Contains(dr["colorRGB"].ToString().Trim()))
								{
									rgb.Add(dr["colorRGB"].ToString().Trim());
									if (colorRGBstr != "")
									{ colorRGBstr += "," + dr["colorRGB"].ToString().Trim(); }
									else
									{ colorRGBstr = dr["colorRGB"].ToString().Trim(); }
								}
							}
						}
					}
				}
				colorRGB.InnerText = colorRGBstr;

				//保修政策
				XmlElement repairPolicyNode = xmlDoc.CreateElement("RepairPolicy");
				root.AppendChild(repairPolicyNode);
				repairPolicyNode.InnerText = sic.SerialRepairPolicy;

				//排量
				XmlElement exhaustNode = xmlDoc.CreateElement("Exhaust");
				root.AppendChild(exhaustNode);
				// exhaustNode.InnerText = sic.CsEngineExhaust;
				// 排量和变速器 取非停销车型的 modified by chengl Jan.26.2010
				string tempExhaust = "";
				DataRow[] ExhaustArr = new Car_BasicBll().GetCarParamEx(sic.CsID, 785, false, " pvalue Asc");
				List<string> tmpList = new List<string>();
				if (ExhaustArr != null && ExhaustArr.Length > 0)
				{
					foreach (DataRow dr in ExhaustArr)
					{
						string exStr = dr["pvalue"].ToString();


						//去重
						if (tmpList.Contains(exStr))
							continue;
						tmpList.Add(exStr);



						if (tempExhaust != "")
						{
							tempExhaust += "、" + exStr + "L";
						}
						else
						{
							tempExhaust = exStr + "L";
						}
					}
				}
				exhaustNode.InnerText = tempExhaust;

				//变速箱
				XmlElement transNode = xmlDoc.CreateElement("TranmissionType");
				root.AppendChild(transNode);
				// transNode.InnerText = sic.CsTransmissionType;
				string tempTransmission = "";
				DataRow[] TransmissionArr = new Car_BasicBll().GetCarParamEx(sic.CsID, 712, false, "");
				if (TransmissionArr != null && TransmissionArr.Length > 0)
				{
					tmpList.Clear();
					foreach (DataRow dr in TransmissionArr)
					{
						string tranStr = dr["pvalue"].ToString();

						if (tranStr.IndexOf("手动") > -1)
							tranStr = "手动";
						else
							tranStr = "自动";

						//去重
						if (tmpList.Contains(tranStr))
							continue;
						tmpList.Add(tranStr);

						if (tmpList.Count >= 2)
							break;
					}
				}
				transNode.InnerText = String.Join("、", tmpList.ToArray());

				//综合油耗
				XmlElement fuelNode = xmlDoc.CreateElement("Fuel");
				root.AppendChild(fuelNode);
				fuelNode.InnerText = sic.CsSummaryFuelCost;

				//官方油耗
				XmlElement officalFuelNode = xmlDoc.CreateElement("OfficalFuel");
				root.AppendChild(officalFuelNode);
				officalFuelNode.InnerText = sic.CsOfficialFuelCost;

				//网友发布油耗
				XmlElement netFuelNode = xmlDoc.CreateElement("NetFuel");
				root.AppendChild(netFuelNode);
				netFuelNode.InnerText = sic.CsGuestFuelCost;

				//子品牌论坛
				XmlElement forumNode = xmlDoc.CreateElement("ForumUrl");
				root.AppendChild(forumNode);
				forumNode.InnerText = new Car_SerialBll().GetForumUrlBySerialId(serialId);

				//子品牌图片
				XmlElement imageNode = xmlDoc.CreateElement("ImgUrl");
				root.AppendChild(imageNode);
				Dictionary<int, XmlElement> urlDic = AutoStorageService.GetImageUrlDic();
				string realUrl = "";
				if (urlDic.ContainsKey(serialId))
				{
					int imgId = Convert.ToInt32(urlDic[serialId].GetAttribute("ImageId"));
					string imgUrl = urlDic[serialId].GetAttribute("ImageUrl");
					if (imgId == 0 || imgUrl == "")
						realUrl = WebConfig.DefaultCarPic;
					else
						realUrl = new OldPageBase().GetPublishImage(2, imgUrl, imgId);
				}
				else
					realUrl = WebConfig.DefaultCarPic;
				imageNode.InnerText = realUrl;


				//还看过的子品牌
				if (noSeeAlso != 1)
				{
					XmlElement alsoSeeNode = xmlDoc.CreateElement("AlsoSeeSerial");
					root.AppendChild(alsoSeeNode);
					List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(serialId, 6);
					if (lsts.Count > 0)
					{
						foreach (EnumCollection.SerialToSerial sts in lsts)
						{
							XmlElement serialNode = xmlDoc.CreateElement("Serial");
							serialNode.SetAttribute("ID", sts.ToCsID.ToString());
							serialNode.SetAttribute("Name", sts.ToCsName);
							serialNode.SetAttribute("ShowName", sts.ToCsShowName);
							serialNode.SetAttribute("AllSpell", sts.ToCsAllSpell);
							serialNode.SetAttribute("PriceRange", sts.ToCsPriceRange);
							serialNode.SetAttribute("Image", sts.ToCsPic);
							alsoSeeNode.AppendChild(serialNode);
						}
					}
				}
			}
			return xmlDoc.OuterXml;
		}
	}
}