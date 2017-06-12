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
	public partial class beijing_2010_DefaultHotModel : beijing_2010_PageBase
	{
		private BCB.ExhibitionAlbum _AlbumBLL = new BitAuto.CarChannel.BLL.ExhibitionAlbum();
		private int StarndValue = 10000;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (_ExhibitionID <= 48)
			{
				PrintData();
				return;
			}
			PrintNewData();
		}
		/// <summary>
		/// 打印数据
		/// </summary>
		private void PrintData()
		{
			//XmlDocument xmlDoc = _AlbumBLL.getBeijing2010AlbumHotData(10);

			XmlDocument xmlDoc = _AlbumBLL.getBeijing2010AlbumRelationData(_ExhibitionID);

			if (xmlDoc == null)
			{
				Response.Write("");
				Response.End();
				return;
			}

			//XmlNodeList xNodeList = xmlDoc.SelectNodes("Data/Album");
			XmlNodeList xNodeList = xmlDoc.SelectNodes("Data/Model/Master");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}
			StringBuilder hotString = new StringBuilder();
			string ImageCount = "";
			string Url = "";
			string Name = "";
			int index = 1;

			int NodeCount = xNodeList.Count - 1;
			Dictionary<int, int> masterBrand = new Dictionary<int, int>();
			Random mRand = new Random();
			List<XmlElement> elem = new List<XmlElement>();
			XmlNodeList xAlbumNodeList;
			int i = 0;

			while (i < 10)
			{
				index = mRand.Next(0, NodeCount);

				if (!masterBrand.ContainsKey(index))
				{
					xAlbumNodeList = xNodeList[index].ChildNodes;
					if (xAlbumNodeList.Count < 1)
					{
						continue;
					}
					elem.Add((XmlElement)xAlbumNodeList[new Random().Next(0, xAlbumNodeList.Count - 1)]);
					i++;
				}
			}
			index = 1;
			foreach (XmlElement xElem in elem)
			{
				if (index > 10)
				{
					break;
				}
				Name = xElem.GetAttribute("Name");
				Url = xmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString() + xElem.GetAttribute("TargetUrl");
				ImageCount = xElem.GetAttribute("Count");
				//<!--需要拼接-->
				hotString.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a><small>{2}</small></li>"
									 , Url
									 , Name
									 , StarndValue + ConvertHelper.GetInteger(10 - index) * 100 + ConvertHelper.GetInteger(10 - index) * (10 - index) - new Random().Next(2, 10));
				index++;
			}

			Response.Write(hotString.ToString());
		}
		/// <summary>
		/// 打印数据
		/// </summary>
		private void PrintNewData()
		{
			XmlDocument xmlDoc = _AlbumBLL.getBeijing2010AlbumRelationData(_ExhibitionID);

			if (xmlDoc == null)
			{
				Response.Write("");
				Response.End();
				return;
			}
			XmlNodeList xNodeList = xmlDoc.SelectNodes("Data/Model/Master");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}
			StringBuilder hotString = new StringBuilder();
			string ImageCount = "";
			string Url = "";
			string Name = "";
			int index = 1;

			int NodeCount = xNodeList.Count - 1;
			Dictionary<int, int> masterBrand = new Dictionary<int, int>();
			Random mRand = new Random();
			List<XmlElement> elem = new List<XmlElement>();
			XmlNodeList xAlbumNodeList;
			int i = 0;

			while (i < 10)
			{
				index = mRand.Next(0, NodeCount);

				if (!masterBrand.ContainsKey(index))
				{
					xAlbumNodeList = xNodeList[index].ChildNodes;
					if (xAlbumNodeList.Count < 1)
					{
						continue;
					}
					elem.Add((XmlElement)xAlbumNodeList[new Random().Next(0, xAlbumNodeList.Count - 1)]);
					i++;
				}
			}
			index = 1;
			string modelUrl = xmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString() + elem[0].GetAttribute("TargetUrl");
			string imgUrl = BCC.CommonFunction.GetPublishHashImageDomain(ConvertHelper.GetInteger(elem[0].GetAttribute("ImageId")))
									+ elem[0].GetAttribute("ImageUrl").Replace("_1.", "_2.");
			hotString.Append("<dl class='clearfix'>");
			hotString.AppendFormat("<dt><a href='{0}' target=\"_blank\"><span><img src='{1}'></span></a></dt>", modelUrl, imgUrl);
			hotString.Append("<dd><div class='first'></div>");
			hotString.AppendFormat("<a href='{0}'target=\"_blank\">{1}</a>", modelUrl, elem[0].GetAttribute("Name"));
			hotString.AppendFormat("<p>关注度：{0}</p></dd>", StarndValue + ConvertHelper.GetInteger(10 - index) * 100 + ConvertHelper.GetInteger(10 - index) * (10 - index) - new Random().Next(2, 10));
			hotString.Append("</dl>");
			hotString.Append("<ol class='clearfix'>");
			foreach (XmlElement xElem in elem)
			{
				if (index < 2)
				{
					index++;
					continue;
				}
				string liClass = "";
				if (index < 4)
				{
					liClass = "redcc_wzh";
				}
				if (index > 10)
				{
					break;
				}
				Name = xElem.GetAttribute("Name");
				Url = xmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString() + xElem.GetAttribute("TargetUrl");
				ImageCount = xElem.GetAttribute("Count");
				//<!--需要拼接-->
				hotString.AppendFormat("<li class='{3}'><a target=\"_blank\" href=\"{0}\">{1}</a><small>{2}</small></li>"
									 , Url
									 , Name
									 , StarndValue + ConvertHelper.GetInteger(10 - index) * 100 + ConvertHelper.GetInteger(10 - index) * (10 - index) - new Random().Next(2, 10)
									 , liClass);
				index++;
			}
			hotString.Append("</ol>");
			Response.Write(hotString.ToString());
		}
	}
}