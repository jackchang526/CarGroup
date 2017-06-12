using System;
using System.Text;
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
	public partial class beijing_2010_DefaultCarTypeList : beijing_2010_PageBase
	{
		private BCB.Exhibition _ExhibibitonBLL = new BitAuto.CarChannel.BLL.Exhibition();
		private BCB.ExhibitionAlbum _AlbumBLL = new BitAuto.CarChannel.BLL.ExhibitionAlbum();

		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		private Dictionary<int, BCM.Pavilion> _PavilionList = new Dictionary<int, BCM.Pavilion>();
		private XmlDocument _AlbumXmlDoc = new XmlDocument();
		private int _AttributeId = 0;
		private int _ShowRowCount = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParam();
			PageBindData();
		}
		/// <summary>
		/// 得到参数页面
		/// </summary>
		private void GetParam()
		{
			_AttributeId = string.IsNullOrEmpty(Request.QueryString["ID"])
				? 0 : ConvertHelper.GetInteger(Request.QueryString["ID"].ToString());
			_ShowRowCount = string.IsNullOrEmpty(Request.QueryString["Count"])
				? 8 : ConvertHelper.GetInteger(Request.QueryString["Count"].ToString());
		}
		/// <summary>
		/// 得到页面数据
		/// </summary>
		private void PageBindData()
		{
			if (_AttributeId < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			_ExhibitionXmlDoc = _ExhibibitonBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (_ExhibitionXmlDoc == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root") == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}
			_AlbumXmlDoc = _AlbumBLL.getBeijing2010AlbumRelationData(_ExhibitionID);
			if (_AlbumXmlDoc == null
			   || _AlbumXmlDoc.SelectSingleNode("Data") == null
			   || _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes == null
			   || _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			if (_ShowRowCount < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			PrintData();
		}
		/// <summary>
		/// 打印数据
		/// </summary>
		private void PrintData()
		{
			string xPath = string.Format("root/MasterBrand/Brand/Serial/Attribute[@ID={0}]", _AttributeId);
			if (_ExhibitionID > 48)
			{
				xPath = string.Format("root/MasterBrand/Brand/Serial[@NC=1]/Attribute[@ID={0}]", _AttributeId);
			}


			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes(xPath);

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			Dictionary<int, int> orderSerial = new Dictionary<int, int>();
			orderSerial = _ExhibibitonBLL.GetOrderSerialTypeByAttributeId(_AttributeId, 3);

			int SurplusCount = 0;

			List<XmlElement> xElemeList = new List<XmlElement>();
			//判断是否设置了子品牌
			if (orderSerial != null && orderSerial.Count > 0)
			{
				foreach (KeyValuePair<int, int> entity in orderSerial)
				{
					XmlElement xNode = (XmlElement)_ExhibitionXmlDoc.SelectSingleNode("root/MasterBrand/Brand/Serial[@ID='"
						+ entity.Key.ToString() + "']");
					XmlElement albumNode = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='"
										   + entity.Key.ToString() + "']");

					if (xNode == null || albumNode == null)
					{
						continue;
					}

					foreach (XmlAttribute xAttr in albumNode.Attributes)
					{
						xNode.SetAttribute(xAttr.Name, xAttr.Value);
					}

					xElemeList.Add(xNode);
				}
			}

			SurplusCount = _ShowRowCount - xElemeList.Count;
			List<XmlElement> imgElemeList = new List<XmlElement>();
			//判断设置的子品牌是否不够显示
			if (SurplusCount > 0)
			{
				foreach (XmlElement entity in xNodeList)
				{
					if (orderSerial != null
						&& orderSerial.ContainsKey(ConvertHelper.GetInteger(entity.GetAttribute("ID"))))
					{
						continue;
					}

					XmlElement tempXmlEleme = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='"
											+ ((XmlElement)entity.ParentNode).GetAttribute("ID") + "']");

					if (tempXmlEleme == null
						|| ConvertHelper.GetInteger(tempXmlEleme.GetAttribute("Count")) == 0)
					{
						continue;
					}

					foreach (XmlAttribute xAttr in tempXmlEleme.Attributes)
					{
						((XmlElement)entity.ParentNode).SetAttribute(xAttr.Name, xAttr.Value);
					}

					imgElemeList.Add(((XmlElement)entity.ParentNode));
				}
				imgElemeList.Sort(BCB.ExhibitionAlbum.OrderXmlElement);

				for (int i = 0; i < SurplusCount; i++)
				{
					if (i + 1 > imgElemeList.Count)
					{
						break;
					}
					xElemeList.Add(imgElemeList[i]);
				}

			}

			PrintElementList(xElemeList);

		}
		/// <summary>
		/// 打印元素值
		/// </summary>
		/// <param name="xNodeList"></param>
		private void PrintElementList(List<XmlElement> xNodeList)
		{
			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}
			StringBuilder orderString = new StringBuilder();
			for (int i = 0; i < _ShowRowCount; i++)
			{
				if (i + 1 > xNodeList.Count)
				{
					break;
				}

				string ImageUrl = "";
				if (xNodeList[i].Attributes["ImageUrl"] == null || string.IsNullOrEmpty(xNodeList[i].Attributes["ImageUrl"].Value))
				{
					ImageUrl = BCC.WebConfig.DefaultCarPic;
				}
				else
				{
					ImageUrl = GetImageUrl((XmlElement)xNodeList[i]);
				}
				//子品牌链接
				string serialUrl = GetSerialUrl(xNodeList[i]);
				string astyle = GetAhrefStyle();
				//拼接li样式
				orderString.AppendFormat(astyle
										, serialUrl
										, GetSettingImageUrl(ImageUrl)
										, xNodeList[i].Attributes["Name"].Value.ToString());
			}

			Response.Write(orderString.ToString());
		}
		/// <summary>
		/// 得到链接的样式
		/// </summary>
		private string GetAhrefStyle()
		{
			if (_ExhibitionID >= 48)
			{
				return "<li><a href=\"{0}\" target=\"_blank\">"
					+ "<img height=\"80\" width=\"120\" src=\"{1}\" alt=\"{2}\">{2}</a></li>";
			}
			return "<li><a href=\"{0}\" target=\"_blank\">"
				   + "<img height=\"91\" width=\"141\" src=\"{1}\" alt=\"{2}\"></a>"
				   + "<a href=\"{0}\" target=\"_blank\">{2}</a></li>";
		}
		/// <summary>
		/// 得到图片地址
		/// </summary>
		/// <param name="defaultImage"></param>
		/// <returns></returns>
		private string GetSettingImageUrl(string defaultImage)
		{
			if (_ExhibitionID >= 48)
			{
				return defaultImage.Replace("_1.", "_2.");
			}
			return defaultImage;
		}
	}
}