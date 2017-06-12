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
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Exhibition
{
	public partial class beijing_2010_SecondLevel : beijing_2010_PageBase
	{
		private BCB.Exhibition _ExhibitionBLL = new BCB.Exhibition();
		private BCB.ExhibitionAlbum _AlbumBLL = new BCB.ExhibitionAlbum();
		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		private XmlDocument _AlbumXmlDoc = new XmlDocument();
		private int type = 0;
		private int NewCarTypeId = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParam();
			BindMainData();
			PrintPage();
		}
		/// <summary>
		/// 得到页面参数
		/// </summary>
		private void GetParam()
		{
			type = string.IsNullOrEmpty(Request.QueryString["type"])
					? 0 : ConvertHelper.GetInteger(Request.QueryString["type"].ToString());
			NewCarTypeId = string.IsNullOrEmpty(Request.QueryString["attr"])
					 ? 10 : ConvertHelper.GetInteger(Request.QueryString["attr"].ToString());
		}
		/// <summary>
		/// 绑定主数据
		/// </summary>
		private void BindMainData()
		{
			_ExhibitionXmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
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
		}
		/// <summary>
		/// 打印页面
		/// </summary>
		private void PrintPage()
		{
			if (type == 0)
			{
				Response.Write("");
				return;
			}
			string content = "";
			switch (type)
			{
				case 1:
					content = BuildChinaNewCar();
					break;
				case 2:
					content = BuildForeginNewCar();
					break;
				case 3:
					content = BuildSUVNewCar();
					break;
				case 4:
					content = BuildNewOil();
					break;
				case 5:
					content = BuildMostSale();
					break;
				case 6:
					content = BuildThink();
					break;
				default:
					break;
			}
			Response.Write(content);
		}
		/// <summary>
		/// 产生国产新车
		/// </summary>
		/// <returns></returns>
		private string BuildChinaNewCar()
		{

			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@Country='国产']/Attribute[@ID='"
									+ NewCarTypeId.ToString() + "']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				return "";
			}

			return NodeListOrder(xNodeList);
		}
		/// <summary>
		/// 产生进口新车
		/// </summary>
		/// <returns></returns>
		private string BuildForeginNewCar()
		{
			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@Country='进口']/Attribute[@ID='"
									+ NewCarTypeId.ToString() + "']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				return "";
			}

			return NodeListOrder(xNodeList);
		}
		/// <summary>
		/// 产生SUV车
		/// </summary>
		/// <returns></returns>
		private string BuildSUVNewCar()
		{
			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@CsLevel='SUV']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				return "";
			}

			return ListOrder(xNodeList);
		}
		/// <summary>
		/// 产生新能源
		/// </summary>
		/// <returns></returns>
		private string BuildNewOil()
		{
			if (NewCarTypeId == 10)
			{
				NewCarTypeId = 12;
			}
			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial/Attribute[@ID='" + NewCarTypeId + "']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				return "";
			}

			return NodeListOrder(xNodeList);
		}
		/// <summary>
		/// 产生豪华车
		/// </summary>
		/// <returns></returns>
		private string BuildMostSale()
		{
			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@CsLevel='豪华车']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				return "";
			}

			return ListOrder(xNodeList);
		}
		/// <summary>
		/// 产生概念车
		/// </summary>
		/// <returns></returns>
		private string BuildThink()
		{
			if (NewCarTypeId == 10)
			{
				NewCarTypeId = 14;
			}
			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial/Attribute[@ID='" + NewCarTypeId + "']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				return "";
			}

			return NodeListOrder(xNodeList);
		}
		/// <summary>
		/// 父结点排序
		/// </summary>
		/// <param name="xNodeList"></param>
		/// <returns></returns>
		private string ListOrder(XmlNodeList xNodeList)
		{
			List<XmlElement> xElemeList = new List<XmlElement>();

			foreach (XmlElement xEleme in xNodeList)
			{
				XmlElement newXmlElem = xEleme;
				XmlElement albumXmlElem = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='"
											+ newXmlElem.GetAttribute("ID") + "']");
				if (albumXmlElem == null
					|| ConvertHelper.GetInteger(albumXmlElem.GetAttribute("Count")) == 0)
				{
					continue;
				}
				foreach (XmlAttribute entity in albumXmlElem.Attributes)
				{
					newXmlElem.SetAttribute(entity.Name, entity.Value);
				}

				xElemeList.Add(newXmlElem);
			}

			xElemeList.Sort(BCB.ExhibitionAlbum.OrderXmlElement);

			if (_ExhibitionID <= 48)
			{
				return BuildHTMLList(xElemeList, 5);
			}
			return BuildNewHTMLLit(xElemeList, 5);
		}
		/// <summary>
		/// 子结点排序
		/// </summary>
		/// <param name="xNodeList"></param>
		/// <returns></returns>
		private string NodeListOrder(XmlNodeList xNodeList)
		{
			List<XmlElement> xElemeList = new List<XmlElement>();

			foreach (XmlElement xEleme in xNodeList)
			{
				XmlElement newXmlElem = ((XmlElement)xEleme.ParentNode);

				if (newXmlElem == null
					|| string.IsNullOrEmpty(newXmlElem.GetAttribute("Name")))
				{
					continue;
				}

				XmlElement albumXmlElem = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='"
											+ newXmlElem.GetAttribute("ID") + "']");
				if (albumXmlElem == null
					|| ConvertHelper.GetInteger(albumXmlElem.GetAttribute("Count")) == 0)
				{
					continue;
				}
				foreach (XmlAttribute entity in albumXmlElem.Attributes)
				{
					newXmlElem.SetAttribute(entity.Name, entity.Value);
				}

				xElemeList.Add(newXmlElem);
			}

			if (xElemeList.Count < 1)
			{
				return "";
			}

			xElemeList.Sort(BCB.ExhibitionAlbum.OrderXmlElement);
			if (_ExhibitionID <= 48)
			{
				return BuildHTMLList(xElemeList, 5);
			}
			return BuildNewHTMLLit(xElemeList, 5);
		}
		/// <summary>
		/// 生成HTML列表
		/// </summary>
		/// <param name="elemList"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		private string BuildHTMLList(List<XmlElement> elemList, int count)
		{
			if (elemList == null || elemList.Count < 1)
			{
				return "";
			}
			StringBuilder liString = new StringBuilder();
			int index = 1;
			foreach (XmlElement entity in elemList)
			{
				if (index > count)
				{
					break;
				}
				index++;
				//子品牌链接
				string serialUrl = GetSerialUrl(entity);
				liString.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img width=\"150\" height=\"100\" src=\"{1}\" alt=\"{2}\"></a>"
									  , serialUrl
									  , GetImageUrl(entity)
									  , entity.GetAttribute("Name"));
				liString.AppendFormat("<p><a href=\"{0}\" target=\"_blank\">{1}</a></p>"
									  , serialUrl
									  , entity.GetAttribute("Name"));

				if (entity.GetAttribute("CsLevel") == "概念车")
				{
					liString.AppendFormat("<p class=\"other\">车型|<a href=\"{0}\">图库</a>|口碑</p>"
										   , _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
										   + _AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='"
										   + entity.GetAttribute("ID") + "']").Attributes["TargetUrl"].Value.ToString());
				}
				else
				{
					liString.AppendFormat("<p class=\"other\"><a href=\"http://car.bitauto.com/{1}/\">车型</a>|"
										   + "<a href=\"http://photo.bitauto.com/serial/{2}/\">图库</a>|<a href=\"http://car.bitauto.com/{1}/koubei/\">口碑</a></p>"
										   , entity.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
										   , entity.GetAttribute("AllSpell")
										   , entity.GetAttribute("ID"));
				}
				liString.Append("</li>");
			}
			return liString.ToString();
		}
		/// <summary>
		/// 建立新的HTML结构
		/// </summary>
		/// <param name="elemList"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		private string BuildNewHTMLLit(List<XmlElement> elemList, int count)
		{
			if (elemList == null || elemList.Count < 1)
			{
				return "";
			}
			StringBuilder liString = new StringBuilder();
			int index = 1;
			foreach (XmlElement entity in elemList)
			{
				if (index > count)
				{
					break;
				}
				index++;
				//子品牌链接
				string serialUrl = GetSerialUrl(entity);
				liString.AppendFormat("<li class=\"\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" alt=\"{2}\">{2}</a>"
									  , serialUrl
									  , GetImageUrl(entity)
									  , entity.GetAttribute("Name"));

				if (entity.GetAttribute("CsLevel") == "概念车")
				{
					liString.AppendFormat("<p><a href=\"{0}\">图库</a></p>"
										   , _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
										   + _AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='"
										   + entity.GetAttribute("ID") + "']").Attributes["TargetUrl"].Value.ToString());
				}
				else
				{
					liString.AppendFormat("<p><a href=\"http://car.bitauto.com/{1}/\">车型</a><span>|</span>"
										   + "<a href=\"http://photo.bitauto.com/serial/{2}/\">图库</a><span>|</span><a href=\"http://car.bitauto.com/{1}/koubei/\">口碑</a></p>"
										   , entity.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
										   , entity.GetAttribute("AllSpell")
										   , entity.GetAttribute("ID"));
				}
				liString.Append("</li>");
			}
			return liString.ToString();
		}
	}
}