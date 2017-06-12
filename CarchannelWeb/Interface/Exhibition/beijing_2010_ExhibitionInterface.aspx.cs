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
	public partial class beijing_2010_ExhibitionInterface : beijing_2010_PageBase
	{
		private BCB.Exhibition exhibitionBLL = new BitAuto.CarChannel.BLL.Exhibition();
		private BCB.ExhibitionAlbum _AlbumBLL = new BitAuto.CarChannel.BLL.ExhibitionAlbum();

		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		private Dictionary<int, BCM.Pavilion> _PavilionList = new Dictionary<int, BCM.Pavilion>();
		private XmlDocument _AlbumXmlDoc = new XmlDocument();

		protected void Page_Load(object sender, EventArgs e)
		{
			PrintfExhibitionXML();
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
			_AlbumXmlDoc = _AlbumBLL.getBeijing2010AlbumRelationData(_ExhibitionID);
			//if (_AlbumXmlDoc == null
			//    || _AlbumXmlDoc.SelectSingleNode("Data") == null
			//    || _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes == null
			//    || _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes.Count < 1)
			//{
			//    Response.Write("");
			//    Response.End();
			//    return;
			//}
			AddMasterBrandAttribute(ref _ExhibitionXmlDoc);
			if (_ExhibitionID > 48) AddBrandAttribue(ref _ExhibitionXmlDoc);
			AddSerialAttribute(ref _ExhibitionXmlDoc);
			string XmlString = _ExhibitionXmlDoc.InnerXml.Replace("</root>", "")
							 + AddAttributeElement()
							 + AddPavilionElement() + "</root>";

			Response.Write(XmlString);
		}
		/// <summary>
		/// 添加主品牌属性
		/// </summary>
		/// <param name="xmlDoc"></param>
		protected void AddMasterBrandAttribute(ref XmlDocument xmlDoc)
		{
			XmlNodeList xNodeList = xmlDoc.SelectNodes("root/MasterBrand");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				return;
			}

			foreach (XmlElement xEleme in xNodeList)
			{
				int masterId = ConvertHelper.GetInteger(xEleme.GetAttribute("ID"));
				xEleme.SetAttribute("LogoUrl", "http://image.bitautoimg.com/bt/car/default/images/carimage/m_" + xEleme.GetAttribute("ID") + "_b.jpg");
				string url = _UrlFormat[_ExhibitionID].Replace("{0}/", "").Replace("{1}/", "");
				if (_ExhibitionID <= 48)
				{
					xEleme.SetAttribute("Url", url + xEleme.GetAttribute("AllSpell") + "/");
				}
				else
				{
					xEleme.SetAttribute("Url", "");
				}
				xEleme.SetAttribute("carUrl", string.Format("http://car.bitauto.com/tree_chexing/mb_{0}/", masterId));
				xEleme.SetAttribute("vUrl", string.Format("http://v.bitauto.com/car/master/{0}.html", masterId));
			}
		}
		/// <summary>
		/// 添加主品牌属性
		/// </summary>
		/// <param name="xmlDoc"></param>
		protected void AddBrandAttribue(ref XmlDocument xmlDoc)
		{
			XmlNodeList xNodeList = xmlDoc.SelectNodes("root/MasterBrand/Brand");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				return;
			}

			foreach (XmlElement xEleme in xNodeList)
			{
				int brandId = ConvertHelper.GetInteger(xEleme.GetAttribute("ID"));
				xEleme.SetAttribute("LogoUrl", string.Format("http://image.bitautoimg.com/carchannel/logo/brand/55png/b_{0}_p55.png", brandId));
				//得到品牌包含的车模数量
				XmlNodeList modelList = _AlbumXmlDoc == null ? null : _AlbumXmlDoc.SelectNodes(string.Format("Data/Model/Master/Album[@BrandId={0}]", brandId));
				if (modelList == null || modelList.Count < 1) continue;
				string targetUrl = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString();
				foreach (XmlElement model in modelList)
				{
					string modelUrl = targetUrl + model.GetAttribute("TargetUrl");
					model.SetAttribute("TargetUrl", modelUrl);
					string focusUrl = BCC.CommonFunction.GetPublishHashImageDomain(ConvertHelper.GetInteger(model.GetAttribute("ImageId")))
									+ model.GetAttribute("ImageUrl").Replace("_1.", "_8.");
					model.SetAttribute("ImageUrl", focusUrl);
					xEleme.AppendChild(xEleme.OwnerDocument.ImportNode(model, false));
				}
			}
		}
		/// <summary>
		/// 添加子品牌属性
		/// </summary>
		/// <param name="xmlDoc"></param>
		protected void AddSerialAttribute(ref XmlDocument xmlDoc)
		{
			XmlNodeList xNodeList = xmlDoc.SelectNodes("root/MasterBrand/Brand/Serial");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				return;
			}

			foreach (XmlElement xEleme in xNodeList)
			{
				int serialId = ConvertHelper.GetInteger(xEleme.GetAttribute("ID"));
				int isNewsCar = ConvertHelper.GetInteger(xEleme.GetAttribute("NC"));
				string serialImageUrl = "";
				string targetUrl = "";
				string classUrl = "";
				string ImageCount = "0";
				if (isNewsCar == 1 && _AlbumXmlDoc != null)
				{
					XmlElement imgNode = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='" + xEleme.GetAttribute("ID") + "']");
					targetUrl = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString();

					if (imgNode != null)
					{
						if (string.IsNullOrEmpty(imgNode.GetAttribute("ImageUrl")))
						{
							serialImageUrl = BCC.WebConfig.DefaultCarPic;
						}
						else
						{
							serialImageUrl = GetImageUrl(imgNode);

						}
						classUrl = imgNode.GetAttribute("TargetUrl");
						ImageCount = imgNode.GetAttribute("Count");
					}
					else
					{
						serialImageUrl = BCC.WebConfig.DefaultCarPic;
					}

				}
				else
				{
					int imgCount = 0;
					base.GetSerialPicAndCountByCsID(serialId, out serialImageUrl, out imgCount, true);
				}
				xEleme.SetAttribute("LogoUrl", serialImageUrl.Replace("_2.", "_1."));
				//如果是2011年上海车展以后的车展，并且车展为非新车，则显示车型综述页
				if (_ExhibitionID > 48 && ConvertHelper.GetInteger(xEleme.GetAttribute("NC")) == 0)
				{
					xEleme.SetAttribute("Url", "http://car.bitauto.com/" + xEleme.GetAttribute("AllSpell") + "/");
				}
				else
				{
					xEleme.SetAttribute("Url", string.Format(_UrlFormat[_ExhibitionID]
											  , xEleme.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
											  , xEleme.GetAttribute("AllSpell")));
				}
				if (xEleme.GetAttribute("CsLevel") == "概念车")
				{
					string picUrl = string.IsNullOrEmpty(targetUrl) && string.IsNullOrEmpty(classUrl) ? "" : targetUrl + classUrl;
					xEleme.SetAttribute("baaUrl", "");
					xEleme.SetAttribute("carUrl", "");
					xEleme.SetAttribute("albumUrl", picUrl);
					xEleme.SetAttribute("imgcount", ImageCount);
				}
				else
				{
					xEleme.SetAttribute("baaUrl", "http://car.bitauto.com/" + xEleme.GetAttribute("AllSpell") + "/koubei/");
					xEleme.SetAttribute("carUrl", "http://car.bitauto.com/" + xEleme.GetAttribute("AllSpell") + "/");
					xEleme.SetAttribute("albumUrl", "http://photo.bitauto.com/serial/" + xEleme.GetAttribute("ID") + "/");
					xEleme.SetAttribute("imgcount", ImageCount);
				}
				//得到子品牌的报价字符串
				string priceString = GetSerialOfficePriceById(serialId);
				if (priceString == "0")
				{
					xEleme.SetAttribute("price", "暂无报价");
					xEleme.SetAttribute("priceUrl", "http://price.bitauto.com/");
				}
				else
				{
					xEleme.SetAttribute("price", priceString);
					xEleme.SetAttribute("priceUrl", string.Format("http://price.bitauto.com/frame.aspx?newbrandid={0}", serialId));
				}
			}
		}
		/// <summary>
		/// 添加属性标签
		/// </summary>
		/// <returns></returns>
		protected string AddAttributeElement()
		{
			Dictionary<int, BCM.Attribute> attrList = exhibitionBLL.GetAttributeListByExhibitionId(_ExhibitionID);
			if (attrList == null || attrList.Count < 1)
			{
				return "";
			}

			StringBuilder attrString = new StringBuilder();
			attrString.Append("<AttributeList>");

			foreach (KeyValuePair<int, BCM.Attribute> entity in attrList)
			{
				attrString.AppendFormat("<Attribute ID=\"{0}\" Name=\"{1}\"/>", entity.Value.ID.ToString(), entity.Value.Name);
			}

			attrString.Append("</AttributeList>");

			return attrString.ToString();
		}
		/// <summary>
		/// 添加展馆标签
		/// </summary>
		/// <returns></returns>
		protected string AddPavilionElement()
		{
			Dictionary<int, BCM.Pavilion> pavilionList = exhibitionBLL.GetPavilionListByExhibitionId(_ExhibitionID);
			if (pavilionList == null || pavilionList.Count < 1)
			{
				return "";
			}

			StringBuilder pavilionString = new StringBuilder();
			pavilionString.Append("<PavilionList>");

			foreach (KeyValuePair<int, BCM.Pavilion> entity in pavilionList)
			{
				pavilionString.AppendFormat("<Pavilion ID=\"{0}\" Name=\"{1}\"/>", entity.Value.ID.ToString(), entity.Value.Name);
			}

			pavilionString.Append("</PavilionList>");

			return pavilionString.ToString();
		}
	}
}