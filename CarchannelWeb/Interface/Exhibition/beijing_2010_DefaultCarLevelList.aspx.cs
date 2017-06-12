using System;
using System.Xml;
using System.Text;
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
	public partial class beijing_2010_DefaultCarLevelList : beijing_2010_PageBase
	{
		private BCB.Exhibition _ExhibibitonBLL = new BitAuto.CarChannel.BLL.Exhibition();
		private BCB.ExhibitionAlbum _AlbumBLL = new BitAuto.CarChannel.BLL.ExhibitionAlbum();

		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		private Dictionary<int, BCM.Pavilion> _PavilionList = new Dictionary<int, BCM.Pavilion>();
		private XmlDocument _AlbumXmlDoc = new XmlDocument();
		private string _AllSpell = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParam();
			PageBindData();
		}
		/// <summary>
		/// 得到页面参数
		/// </summary>
		private void GetParam()
		{
			_AllSpell = string.IsNullOrEmpty(Request.QueryString["allspell"])
							? "" : Request.QueryString["allspell"].ToString();
		}
		/// <summary>
		/// 得到车型数据
		/// </summary>
		private void PageBindData()
		{
			if (string.IsNullOrEmpty(_AllSpell))
			{
				Response.Write("");
				Response.End();
				return;
			}

			string csLevel = GetLevelString();

			if (string.IsNullOrEmpty(csLevel))
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

			string xPath = string.Format("root/MasterBrand/Brand/Serial[@CsLevel='{0}']", csLevel);
			if (_ExhibitionID > 48)
			{
				xPath = string.Format("root/MasterBrand/Brand/Serial[@CsLevel='{0}' and @NC=1]", csLevel);
			}

			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes(xPath);

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}
			List<XmlElement> xElemeList = new List<XmlElement>();

			foreach (XmlElement xEleme in xNodeList)
			{
				XmlElement xNode = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='" + xEleme.GetAttribute("ID") + "']");
				if (xNode == null)
				{
					continue;
				}
				foreach (XmlAttribute entity in xNode.Attributes)
				{
					xEleme.SetAttribute(entity.Name, entity.Value);
				}
				xElemeList.Add(xEleme);
			}

			xElemeList.Sort(BCB.ExhibitionAlbum.OrderXmlElement);
			StringBuilder listString = new StringBuilder();
			int index = 1;
			foreach (XmlElement xEleme in xElemeList)
			{
				if (index > 8)
				{
					continue;
				}
				index++;
				string ImageUrl = "";
				if (string.IsNullOrEmpty(xEleme.GetAttribute("ImageUrl")))
				{
					ImageUrl = BCC.WebConfig.DefaultCarPic;
				}
				else
				{
					ImageUrl = GetImageUrl(xEleme);
				}

				int newCar = ConvertHelper.GetInteger(xEleme.GetAttribute("NC"));
				int serialId = ConvertHelper.GetInteger(xEleme.GetAttribute("ID"));
				//如果是已经上市的车
				if (newCar != 1 && _ExhibitionID > 48)
				{
					int imgCount = 0;
					base.GetSerialPicAndCountByCsID(serialId, out ImageUrl, out imgCount, true);
				}
				string masterSpell = ((XmlElement)xEleme.ParentNode.ParentNode).GetAttribute("AllSpell").ToString().ToLower();
				string serialSpell = xEleme.GetAttribute("AllSpell").ToLower();
				//子品牌链接
				string serialUrl = GetSerialUrl(masterSpell, serialSpell, newCar);
				string astyle = GetAhrefStyle();
				//拼接li样式
				listString.AppendFormat(astyle
										, serialUrl
										, GetSettingImageUrl(ImageUrl)
										, xEleme.GetAttribute("Name"));

			}
			Response.Write(listString.ToString());
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
		/// <summary>
		/// 得到级别名称
		/// </summary>
		/// <returns></returns>
		private string GetLevelString()
		{
			if (_AllSpell == "haohuache")
			{
				return "豪华车";
			}
			return _AllSpell.ToUpper();
		}
	}
}