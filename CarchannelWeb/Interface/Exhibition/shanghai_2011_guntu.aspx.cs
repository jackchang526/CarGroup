using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.Utils;
using BCB = BitAuto.CarChannel.BLL;
using BCM = BitAuto.CarChannel.Model;
using BCC = BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Exhibition
{
	public partial class shanghai_2011_guntu : beijing_2010_PageBase
	{
		private BCB.Exhibition exhibitionBLL = new BitAuto.CarChannel.BLL.Exhibition();
		private BCB.ExhibitionAlbum _AlbumBLL = new BitAuto.CarChannel.BLL.ExhibitionAlbum();

		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		private Dictionary<int, BCM.Pavilion> _PavilionList = new Dictionary<int, BCM.Pavilion>();
		private XmlDocument _AlbumXmlDoc = new XmlDocument();
		private DateTime _RequestTime;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParams();
			PrintfExhibitionXML();
		}

		private void GetParams()
		{
			_RequestTime = string.IsNullOrEmpty(Request.QueryString["t"])
							? DateTime.Now : ConvertHelper.GetDateTime(Request.QueryString["t"]);
		}
		/// <summary>
		/// 打印展会XML
		/// </summary>
		protected void PrintfExhibitionXML()
		{
			Response.ContentType = "XML";
			if (_ExhibitionID < 1)
			{
				Response.Write("");
				return;
			}

			_ExhibitionXmlDoc = exhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);


			if (_ExhibitionXmlDoc == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root") == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}
			DateTime startTime = new DateTime(_RequestTime.AddDays(-1).Year, _RequestTime.AddDays(-1).Month, _RequestTime.AddDays(-1).Day
											, 21, 0, 0);
			DateTime endTime = new DateTime(_RequestTime.Year, _RequestTime.Month, _RequestTime.Day
										   , 21, 0, 0);
			//得到图集的时间
			_AlbumXmlDoc = _AlbumBLL.getShangHai2010AlbumNewCarGunTuList(_ExhibitionID, startTime, endTime);
			if (_AlbumXmlDoc == null
				|| _AlbumXmlDoc.SelectSingleNode("Data") == null
				|| _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes == null
				|| _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}
			GetElementList();
		}
		/// <summary>
		/// 得到结果XML
		/// </summary>
		private void GetElementList()
		{
			//创建新的XML结点
			XmlDocument xmlDoc = new XmlDocument();
			XmlNode rootNode = xmlDoc.CreateElement("root");
			xmlDoc.AppendChild(rootNode);
			XmlDeclaration xmlDel = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
			xmlDoc.InsertBefore(xmlDel, rootNode);

			XmlNodeList dataNodeList = _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes;

			foreach (XmlElement elem in dataNodeList)
			{
				int serialId = ConvertHelper.GetInteger(elem.GetAttribute("SerialId"));
				if (serialId < 1) continue;
				//得到展会关联的子品牌
				XmlNode xNode = _ExhibitionXmlDoc.SelectSingleNode(string.Format("root/MasterBrand/Brand/Serial[@ID={0}]", serialId));
				if (xNode == null || xNode.Attributes["NC"] == null || ConvertHelper.GetInteger(xNode.Attributes["NC"].Value) != 1) continue;
				if (string.IsNullOrEmpty(elem.GetAttribute("ImageUrl"))) continue;

				string focusImage = string.Format(elem.GetAttribute("ImageUrl"), 2);
				string showName = xNode.Attributes["Name"].Value.ToString();
				string serialUrl = GetSerialUrl(xNode.ParentNode.ParentNode.Attributes["AllSpell"].Value
											   , xNode.Attributes["AllSpell"].Value
											   , ConvertHelper.GetInteger(xNode.Attributes["NC"].Value));

				XmlElement serialElem = xmlDoc.CreateElement("Serial");
				serialElem.SetAttribute("ID", serialId.ToString());
				serialElem.SetAttribute("Name", showName);
				serialElem.SetAttribute("ImageUrl", focusImage);
				serialElem.SetAttribute("Url", serialUrl);
				rootNode.AppendChild(serialElem);
			}
			Response.Write(xmlDoc.InnerXml);
		}
	}
}