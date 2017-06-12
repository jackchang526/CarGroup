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
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Exhibition
{
	public partial class beijing_2010_DefaultPavilionList : beijing_2010_PageBase
	{
		private BCB.Exhibition _ExhibibitonBLL = new BitAuto.CarChannel.BLL.Exhibition();

		private int _PavilionId = 0;
		private int _Count = 9999;
		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		Dictionary<int, BCM.Pavilion> pavilionList = new Dictionary<int, BitAuto.CarChannel.Model.Pavilion>();

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParam();
			//绑定展馆属性
			BuilderPavilionUrlList();
			if (_ExhibitionID <= 48)
			{
				PrintPage();
				return;
			}
			PrintNewPage();
		}
		/// <summary>
		/// 得到页面参数
		/// </summary>
		private void GetParam()
		{
			_PavilionId = string.IsNullOrEmpty(Request.QueryString["ID"])
								? 0 : ConvertHelper.GetInteger(Request.QueryString["ID"].ToString());
			_Count = string.IsNullOrEmpty(Request.QueryString["c"])
				? 9999 : ConvertHelper.GetInteger(Request.QueryString["c"]);
		}
		/// <summary>
		/// 打印页面
		/// </summary>
		private void PrintPage()
		{
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

			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand[@PavilionId='"
									+ _PavilionId.ToString() + "']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			List<XmlElement> xElemList = new List<XmlElement>();
			foreach (XmlElement entity in xNodeList)
			{
				xElemList.Add(entity);
			}
			xElemList.Sort(BCB.Exhibition.OrderXmlElement);

			StringBuilder pavilionString = new StringBuilder();
			int index = 1;
			string MasterBrandUrl = "";
			string MasterLogoUrl = "";
			foreach (XmlElement xElem in xElemList)
			{
				if (index > 10)
				{
					break;
				}
				index++;
				string url = _UrlFormat[_ExhibitionID].Replace("{0}/", "").Replace("{1}/", "");
				MasterBrandUrl = url
								+ xElem.GetAttribute("AllSpell") + "/";
				MasterLogoUrl = "http://image.bitautoimg.com/bt/car/default/images/carimage/m_"
								+ xElem.GetAttribute("ID") + "_100.jpg";
				pavilionString.AppendFormat("<li><a target=\"_blank\" href=\"{0}\"><img height=\"100\" width=\"100\" alt=\"{1}\" src=\"{2}\"></a><a target=\"_blank\" href=\"{0}\">{1}</a></li>"
											, MasterBrandUrl
											, xElem.GetAttribute("Name")
											, MasterLogoUrl);
			}

			Response.Write(pavilionString.ToString());
		}
		/// <summary>
		/// 打印新的页面
		/// </summary>
		private void PrintNewPage()
		{
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
			pavilionList = new BCB.Exhibition().GetPavilionListByExhibitionId(_ExhibitionID);

			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand[@PavilionId='"
									+ _PavilionId.ToString() + "']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			List<XmlElement> xElemList = new List<XmlElement>();
			foreach (XmlElement entity in xNodeList)
			{
				xElemList.Add(entity);
			}
			xElemList.Sort(BCB.Exhibition.OrderXmlElement);

			StringBuilder pavilionString = new StringBuilder();
			int index = 1;
			string BrandUrl = "";
			string BrandLogoUrl = "";
			foreach (XmlElement xElem in xElemList)
			{
				if (index > _Count)
				{
					break;
				}
				string url = _PravilionUrlFormat[_ExhibitionID];
				string brandid = xElem.GetAttribute("ID");
				BrandUrl = string.Format(url, PavilionUrl[pavilionList[ConvertHelper.GetInteger(xElem.GetAttribute("PavilionId"))].Name]) + brandid + "/";
				BrandLogoUrl = string.Format("http://image.bitautoimg.com/carchannel/logo/brand/55png/b_{0}_p55.png", brandid);
				pavilionString.AppendFormat("<li class='{3}'><a target=\"_blank\" href=\"{0}\"><img alt=\"{1}\" src=\"{2}\">{1}</li>"
											, BrandUrl
											, xElem.GetAttribute("Name")
											, BrandLogoUrl
											, index == 1 ? "fist" : "");
				index++;
			}

			Response.Write(pavilionString.ToString());
		}
	}
}