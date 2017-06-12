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
	public partial class Beijing_2010_PavilionAllModelList : beijing_2010_PageBase
	{
		private BCB.Exhibition _ExhibibitonBLL = new BitAuto.CarChannel.BLL.Exhibition();
		private BCB.ExhibitionAlbum _AlbumBLL = new BitAuto.CarChannel.BLL.ExhibitionAlbum();

		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		private Dictionary<int, BCM.Pavilion> _PavilionList = new Dictionary<int, BCM.Pavilion>();
		private XmlDocument _AlbumXmlDoc = new XmlDocument();

		protected void Page_Load(object sender, EventArgs e)
		{
			PrintData();
			BuilderData();
		}
		/// <summary>
		/// 绑定页面数据
		/// </summary>
		private void PrintData()
		{
			_PavilionList = _ExhibibitonBLL.GetPavilionListByExhibitionId(_ExhibitionID);
			if (_PavilionList == null)
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
		}
		/// <summary>
		/// 生成数据
		/// </summary>
		private void BuilderData()
		{
			StringBuilder albumMedole = new StringBuilder();
			albumMedole.Append("<root>");

			foreach (KeyValuePair<int, BCM.Pavilion> pavilionEntity in _PavilionList)
			{
				albumMedole.AppendFormat("<Pavilion ID=\"{0}\">", pavilionEntity.Value.ID.ToString());

				XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand[@PavilionId='"
									   + pavilionEntity.Value.ID.ToString()
									   + "']");

				if (xNodeList == null || xNodeList.Count < 1)
				{
					albumMedole.Append("</Pavilion>");
					continue;
				}

				foreach (XmlElement xEleme in xNodeList)
				{
					XmlNodeList xAlbumNodeList = _AlbumXmlDoc.SelectNodes("Data/Model/Master[@Id='" + xEleme.GetAttribute("ID") + "']");
					albumMedole.Append(BuilderAblumData(xAlbumNodeList));
				}

				albumMedole.Append("</Pavilion>");
			}

			albumMedole.Append("</root>");
			Response.Write(albumMedole.ToString());
		}
		/// <summary>
		/// 生成图集结点
		/// </summary>
		private string BuilderAblumData(XmlNodeList xNodeList)
		{
			string photoDomain = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["ImageDomain"].Value.ToString();
			string tagetDomain = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString();
			if (xNodeList == null || xNodeList.Count < 1)
			{
				return "";
			}
			StringBuilder albumString = new StringBuilder();
			foreach (XmlElement xElem in xNodeList)
			{
				if (xElem.ChildNodes == null || xElem.ChildNodes.Count < 1)
				{
					continue;
				}

				foreach (XmlElement entity in xElem.ChildNodes)
				{
					string imgUrl = "";
					if (string.IsNullOrEmpty(entity.GetAttribute("ImageUrl")))
					{
						imgUrl = BCC.WebConfig.DefaultCarPic;
					}
					else
					{
						imgUrl = GetImageUrl(entity);
					}

					if (_ExhibitionID == 48)
					{
						imgUrl = imgUrl.Replace("_1.", "_8.");
					}
					albumString.AppendFormat("<Model LogoUrl=\"{0}\" Url=\"{1}\" Name=\"{2}\" />"
											 , imgUrl
											 , tagetDomain + entity.GetAttribute("TargetUrl")
											 , entity.GetAttribute("Name"));
				}
			}
			return albumString.ToString();
		}
	}
}