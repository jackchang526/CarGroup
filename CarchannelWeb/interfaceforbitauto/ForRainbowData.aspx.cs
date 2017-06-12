using System;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class ForRainbowData : InterfacePageBase
	{
		private string csIds = "";
		protected string xmlContent = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{

				this.CheckPageParam();

				if (csIds.Length > 0)
				{
					this.GetSerialRainbowDataByCsIds();
				}
				else
				{
					//Response.Redirect("http://car.bitauto.com/car/data/interFace_rainbow.xml");
					// GetSerialRainbowData();
					string cacheKey = "Interface_Rainbow_xml";
					string xmlStr = (string)CacheManager.GetCachedData(cacheKey);
					if (xmlStr == null)
					{
						xmlStr = "";
						string xmlFileName = Path.Combine(WebConfig.DataBlockPath, "Data\\Interface_Rainbow.xml");
						if (File.Exists(xmlFileName))
						{
							xmlStr = File.ReadAllText(xmlFileName, Encoding.UTF8);
						}
						CacheManager.InsertCache(cacheKey, xmlStr, 10);
					}
					Response.Write(xmlStr);
				}
			}
		}

		private void CheckPageParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				csIds = this.Request.QueryString["csID"].ToString().Trim();
			}
		}

		//     private void GetSerialRainbowDataByCSID()
		//     {
		//         RainbowListBll rb = new RainbowListBll();
		//         xmlContent = rb.GetRainbowListXML_CSID(csID);
		// 
		//         Response.Write(xmlContent);
		//     }

		private void GetSerialRainbowDataByCsIds()
		{
			string[] csIdList = csIds.Split(',');
			RainbowListBll rb = new RainbowListBll();
			XmlDocument xmlDoc = new XmlDocument();
			XmlElement root = xmlDoc.CreateElement("RainbowRoot");
			root.SetAttribute("Time", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
			xmlDoc.AppendChild(root);
			XmlDeclaration xmlDeclar = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
			xmlDoc.InsertBefore(xmlDeclar, root);

			//国产车
			XmlElement selfCar = xmlDoc.CreateElement("Serials");
			selfCar.SetAttribute("Type", "国产车");
			root.AppendChild(selfCar);
			XmlElement importCar = xmlDoc.CreateElement("Serials");
			importCar.SetAttribute("Type", "进口车");
			root.AppendChild(importCar);

			foreach (string csIdStr in csIdList)
			{
				try
				{
					int csId = ConvertHelper.GetInteger(csIdStr);
					if (csId > 0)
					{
						XmlDocument serialDoc = new XmlDocument();
						string xmlStr = rb.GetRainbowListXML_CSID(csId);
						serialDoc.LoadXml(xmlStr);

						XmlElement serialNode = (XmlElement)serialDoc.SelectSingleNode("/RainbowRoot/Serial");
						if (serialNode != null)
						{
							int tempId = ConvertHelper.GetInteger(serialNode.GetAttribute("ID"));
							string country = serialNode.GetAttribute("CarCountry");
							if (tempId > 0)
							{
								if (country == "进口车")
									importCar.AppendChild(xmlDoc.ImportNode(serialNode, true));
								else if (country == "国产车")
									selfCar.AppendChild(xmlDoc.ImportNode(serialNode, true));
							}
						}
					}
				}
				catch (System.Exception ex)
				{

				}

			}
			Response.Write(xmlDoc.OuterXml);
		}

		private void GetSerialRainbowData()
		{
			//RainbowListBll rb = new RainbowListBll();
			//xmlContent = rb.GetRainbowListXML_All();

			//Response.Write(xmlContent);
		}
	}
}