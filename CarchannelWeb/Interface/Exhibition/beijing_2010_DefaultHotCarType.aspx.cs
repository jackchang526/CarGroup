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
	public partial class beijing_2010_DefaultHotCarType : beijing_2010_PageBase
	{
		private BCB.Exhibition _ExhibitionBLL = new BCB.Exhibition();
		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		private BCB.ExhibitionAlbum _AlbumBLL = new BitAuto.CarChannel.BLL.ExhibitionAlbum();
		/// <summary>
		/// type为1表示是左边
		/// </summary>
		private int type = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParam();
			if (_ExhibitionID <= 48)
			{
				BindPageData();
				return;
			}
			//绑定带焦点图的列表
			BindPageDataOfFocusImage();
		}
		/// <summary>
		/// 得到新闻参数
		/// </summary>
		private void GetParam()
		{
			type = string.IsNullOrEmpty(Request.QueryString["type"]) ? 0 : ConvertHelper.GetInteger(Request.QueryString["type"]);
		}
		/// <summary>
		/// 绑定页面数据
		/// </summary>
		private void BindPageData()
		{
			XmlDocument xmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (xmlDoc == null
				|| xmlDoc.SelectSingleNode("root") == null
				|| xmlDoc.SelectSingleNode("root").ChildNodes == null
				|| xmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			XmlNodeList xNodeList = xmlDoc.SelectNodes("root/MasterBrand/Brand/Serial");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			Dictionary<string, string> getExhibitionCarType = new Dictionary<string, string>();

			foreach (XmlElement xElem in xNodeList)
			{
				if (!getExhibitionCarType.ContainsKey(xElem.GetAttribute("ID")))
				{
					getExhibitionCarType.Add(xElem.GetAttribute("ID"), xElem.GetAttribute("ID"));
				}
			}

			List<BCC.Enum.EnumCollection.SerialSortForInterface> eesfiList = new BCB.Car_SerialBll().GetBeiJing2010SerialTop10(10, getExhibitionCarType);

			if (eesfiList == null || eesfiList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			StringBuilder list = new StringBuilder();

			foreach (BCC.Enum.EnumCollection.SerialSortForInterface entity in eesfiList)
			{
				//子品牌链接
				string serialUrl = string.Format(_UrlFormat[_ExhibitionID]
									, (xmlDoc.SelectSingleNode("root/MasterBrand/Brand/Serial[@ID='"
									   + entity.CsID
									   + "']").ParentNode.ParentNode).Attributes["AllSpell"].Value.ToString()
									, entity.CsAllSpell);

				//string Url = "http://chezhan.bitauto.com/beijing/2010/"
				//                   + (xmlDoc.SelectSingleNode("root/MasterBrand/Brand/Serial[@ID='"
				//                   + entity.CsID
				//                   + "']").ParentNode.ParentNode).Attributes["AllSpell"].Value.ToString()
				//                   + "/" + entity.CsAllSpell + "/";

				list.AppendFormat("<li><a href=\"{0}\" target=\"_blank\">{1}</a><small>{2}</small></li>"
								 , serialUrl
								 , entity.CsShowName
								 , entity.CsPV.ToString());
			}

			Response.Write(list.ToString());
		}
		/// <summary>
		/// 绑定页面的焦点图
		/// </summary>
		private void BindPageDataOfFocusImage()
		{
			if (type == 1)
			{
				BindLeftPageDataOfFocusImage();
				return;
			}
			XmlDocument xmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (xmlDoc == null
				|| xmlDoc.SelectSingleNode("root") == null
				|| xmlDoc.SelectSingleNode("root").ChildNodes == null
				|| xmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			XmlNodeList xNodeList = xmlDoc.SelectNodes("root/MasterBrand/Brand/Serial");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			Dictionary<string, string> getExhibitionCarType = new Dictionary<string, string>();

			foreach (XmlElement xElem in xNodeList)
			{
				if (!getExhibitionCarType.ContainsKey(xElem.GetAttribute("ID")))
				{
					getExhibitionCarType.Add(xElem.GetAttribute("ID"), xElem.GetAttribute("ID"));
				}
			}

			List<BCC.Enum.EnumCollection.SerialSortForInterface> eesfiList = new BCB.Car_SerialBll().GetBeiJing2010SerialTop10(10, getExhibitionCarType);

			if (eesfiList == null || eesfiList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}
			StringBuilder list = new StringBuilder();
			XmlElement xfNode = (XmlElement)xmlDoc.SelectSingleNode(string.Format("root/MasterBrand/Brand/Serial[@ID={0}]", eesfiList[0].CsID));
			if (xfNode != null)
			{
				int isNewsCar = ConvertHelper.GetInteger(xfNode.GetAttribute("NC"));
				string serialUrl = GetSerialUrl(xfNode.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString(), eesfiList[0].CsAllSpell, isNewsCar);
				//得到车型的默认图
				XmlDocument albumDoc = _AlbumBLL.getBeijing2010AlbumRelationData(_ExhibitionID);
				string imgUrl = "";
				int imgCount = 0;
				//如果是新车图片，则取车展图库图片，否则取车型图库图片
				if (isNewsCar == 1)
				{
					XmlElement albumNode = albumDoc == null ? null : (XmlElement)albumDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='"
											  + eesfiList[0].CsID + "']");
					if (albumNode == null)
						imgUrl = BCC.WebConfig.DefaultCarPic.Replace("150-100", "300-200");
					else
						imgUrl = BCC.CommonFunction.GetPublishHashImageDomain(ConvertHelper.GetInteger(albumNode.GetAttribute("ImageId")))
									+ albumNode.GetAttribute("ImageUrl").Replace("_1.", "_2.");
				}
				else
				{
					base.GetSerialPicAndCountByCsID(eesfiList[0].CsID, out imgUrl, out imgCount, true);
				}

				list.Append("<dl class='clearfix'>");
				list.AppendFormat("<dt><a href='{0}' target=\"_blank\"><img src='{1}'></a></dt>", serialUrl, imgUrl);
				list.Append("<dd><div class='first'></div>");
				list.AppendFormat("<a href='{0}' target=\"_blank\">{1}</a>", serialUrl, eesfiList[0].CsShowName);
				list.AppendFormat("<p>关注度：{0}</p></dd>", eesfiList[0].CsPV);
				list.Append("</dl>");
			}
			list.Append(" <ol class='clearfix'>");
			int index = 0;
			foreach (BCC.Enum.EnumCollection.SerialSortForInterface entity in eesfiList)
			{
				if (index < 1)
				{
					index++;
					continue;
				}

				string liClass = "";
				if (index < 3)
				{
					liClass = "redcc_wzh";
				}
				//子品牌链接
				XmlElement xNode = (XmlElement)xmlDoc.SelectSingleNode(string.Format("root/MasterBrand/Brand/Serial[@ID={0}]", entity.CsID));
				if (xNode == null) continue;
				int isNewsCar = ConvertHelper.GetInteger(xNode.GetAttribute("NC"));
				string serialUrl = GetSerialUrl(xNode.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString(), entity.CsAllSpell, isNewsCar);

				list.AppendFormat("<li class='{3}'><a href=\"{0}\" target=\"_blank\">{1}</a><small>{2}</small></li>"
								 , serialUrl
								 , entity.CsShowName
								 , entity.CsPV.ToString()
								 , liClass);
				index++;
			}
			list.Append("</ol>");
			Response.Write(list.ToString());
		}
		/// <summary>
		/// 绑定左边的块
		/// </summary>
		private void BindLeftPageDataOfFocusImage()
		{

			XmlDocument xmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (xmlDoc == null
				|| xmlDoc.SelectSingleNode("root") == null
				|| xmlDoc.SelectSingleNode("root").ChildNodes == null
				|| xmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			XmlNodeList xNodeList = xmlDoc.SelectNodes("root/MasterBrand/Brand/Serial");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			Dictionary<string, string> getExhibitionCarType = new Dictionary<string, string>();

			foreach (XmlElement xElem in xNodeList)
			{
				if (!getExhibitionCarType.ContainsKey(xElem.GetAttribute("ID")))
				{
					getExhibitionCarType.Add(xElem.GetAttribute("ID"), xElem.GetAttribute("ID"));
				}
			}

			List<BCC.Enum.EnumCollection.SerialSortForInterface> eesfiList = new BCB.Car_SerialBll().GetBeiJing2010SerialTop10(10, getExhibitionCarType);

			if (eesfiList == null || eesfiList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}
			StringBuilder list = new StringBuilder();
			XmlElement xfNode = (XmlElement)xmlDoc.SelectSingleNode(string.Format("root/MasterBrand/Brand/Serial[@ID={0}]", eesfiList[0].CsID));
			if (xfNode != null)
			{
				int isNewsCar = ConvertHelper.GetInteger(xfNode.GetAttribute("NC"));
				string serialUrl = GetSerialUrl(xfNode.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString(), eesfiList[0].CsAllSpell, isNewsCar);
				//得到车型的默认图
				XmlDocument albumDoc = _AlbumBLL.getBeijing2010AlbumRelationData(_ExhibitionID);
				string imgUrl = "";
				int imgCount = 0;
				//如果是新车图片，则取车展图库图片，否则取车型图库图片
				if (isNewsCar == 1)
				{
					XmlElement albumNode = albumDoc == null ? null : (XmlElement)albumDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='"
											  + eesfiList[0].CsID + "']");
					if (albumNode == null)
						imgUrl = BCC.WebConfig.DefaultCarPic.Replace("150-100", "300-200");
					else
						imgUrl = BCC.CommonFunction.GetPublishHashImageDomain(ConvertHelper.GetInteger(albumNode.GetAttribute("ImageId")))
									+ albumNode.GetAttribute("ImageUrl").Replace("_1.", "_2.");
				}
				else
				{
					base.GetSerialPicAndCountByCsID(eesfiList[0].CsID, out imgUrl, out imgCount, true);
				}

				list.Append("<dl class='clearfix'>");
				list.Append("<dd><div class='first'></div>");
				list.AppendFormat("<a href='{0}' class='rank_one' target=\"_blank\">{1}</a>", serialUrl, eesfiList[0].CsShowName);
				list.AppendFormat("<p>关注度：{0}</p></dd>", eesfiList[0].CsPV);
				list.Append("</dl>");
			}
			list.Append(" <ol class='clearfix'>");
			int index = 0;
			foreach (BCC.Enum.EnumCollection.SerialSortForInterface entity in eesfiList)
			{
				if (index < 1)
				{
					index++;
					continue;
				}
				//子品牌链接
				XmlElement xNode = (XmlElement)xmlDoc.SelectSingleNode(string.Format("root/MasterBrand/Brand/Serial[@ID={0}]", entity.CsID));
				if (xNode == null) continue;
				string liClass = "";
				if (index < 3)
				{
					liClass = "redcc_wzh";
				}
				else if (index == eesfiList.Count - 1)
				{
					liClass = "noline";
				}
				int isNewsCar = ConvertHelper.GetInteger(xNode.GetAttribute("NC"));
				string serialUrl = GetSerialUrl(xNode.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString(), entity.CsAllSpell, isNewsCar);

				list.AppendFormat("<li class='{3}'><a href=\"{0}\" target=\"_blank\">{1}</a><small>{2}</small></li>"
								 , serialUrl
								 , entity.CsShowName
								 , entity.CsPV.ToString()
								 , liClass);
				index++;
			}
			list.Append("</ol>");
			Response.Write(list.ToString());
		}
	}
}