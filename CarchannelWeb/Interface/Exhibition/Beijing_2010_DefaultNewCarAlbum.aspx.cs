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
	public partial class Beijing_2010_DefaultNewCarAlbum : beijing_2010_PageBase
	{
		private BCB.Exhibition _ExhibibitonBLL = new BitAuto.CarChannel.BLL.Exhibition();
		private BCB.ExhibitionAlbum _AlbumBLL = new BitAuto.CarChannel.BLL.ExhibitionAlbum();

		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		private Dictionary<int, BCM.Pavilion> _PavilionList = new Dictionary<int, BCM.Pavilion>();
		private XmlDocument _AlbumXmlDoc = new XmlDocument();
		private int _AttributeId = 0;
		private int _TotalCount = 8;
		private string _MostCount = ",11,";
		private string _removeSerialId = "";//要移除的子品牌ID列表

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
			_TotalCount = string.IsNullOrEmpty(Request.QueryString["num"])
				? 8 : ConvertHelper.GetInteger(Request.QueryString["num"]);
			_removeSerialId = string.IsNullOrEmpty(Request.QueryString["rs"])
				? "" : Request.QueryString["rs"];
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

			PrintData();
		}
		/// <summary>
		/// 打印数据
		/// </summary>
		private void PrintData()
		{

			if (_MostCount.IndexOf("," + _AttributeId.ToString() + ",") >= 0 && _ExhibitionID == 19)
			{
				_TotalCount = 16;
			}

			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial/Attribute[@ID='"
								   + _AttributeId.ToString() + "']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			//定义要移除的子品牌数据
			List<int> removeSerailList = new List<int>();
			//如果要移除的列表存在
			if (!string.IsNullOrEmpty(_removeSerialId))
			{
				foreach (string idstr in _removeSerialId.Split(','))
				{
					removeSerailList.Add(ConvertHelper.GetInteger(idstr));
				}
			}

			Dictionary<int, int> orderSerial = new Dictionary<int, int>();
			orderSerial = _ExhibibitonBLL.GetOrderSerialTypeByAttributeId(_AttributeId, 2);

			int SurplusCount = 0;

			List<XmlElement> xElemeList = new List<XmlElement>();
			//判断是否设置了子品牌
			if (orderSerial != null && orderSerial.Count > 0)
			{
				foreach (KeyValuePair<int, int> entity in orderSerial)
				{
					if (removeSerailList.Contains(entity.Key)) continue;
					SurplusCount++;
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

			SurplusCount = _TotalCount - SurplusCount - removeSerailList.Count;
			List<XmlElement> imgElemeList = new List<XmlElement>();
			//判断设置的子品牌是否不够显示
			if (SurplusCount > 0)
			{
				foreach (XmlElement entity in xNodeList)
				{
					int serialId = ConvertHelper.GetInteger(((XmlElement)entity.ParentNode).GetAttribute("ID"));//得到当前的子品牌ID
					if (orderSerial != null
						&& orderSerial.ContainsKey(serialId))
					{
						continue;
					}

					if (removeSerailList.Contains(serialId)) continue;//如果移除列表中包含此品牌

					XmlElement tempXmlEleme = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='"
											+ serialId + "']");

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
			//如果是大于48，则调用另外接口
			if (_ExhibitionID > 48)
			{
				PrintNewElementList(xNodeList);
				return;
			}
			StringBuilder orderString = new StringBuilder();
			for (int i = 0; i < _TotalCount; i++)
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
					ImageUrl = GetImageUrl((XmlElement)xNodeList[i]).Replace("_1.", "_5.");
				}


				//orderString.AppendFormat("<li><a target=\"_blank\" href=\"{1}\"><img src=\"{0}\" width=\"90\" height=\"60\" /></a><a target=\"_blank\" href=\"{1}\">{2}</a></li>"
				//                         , ImageUrl
				//                         , _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
				//                          + xNodeList[i].Attributes["TargetUrl"].Value
				//                         , xNodeList[i].Attributes["Name"].Value.ToString());

				//子品牌链接
				string serialUrl = GetSerialUrl(xNodeList[i]);

				orderString.AppendFormat("<li><a target=\"_blank\" href=\"{1}\"><img src=\"{0}\" width=\"90\" height=\"60\" /></a><a target=\"_blank\" href=\"{1}\">{2}</a></li>"
									   , ImageUrl
									   , serialUrl
									   , xNodeList[i].Attributes["Name"].Value.ToString());
			}

			Response.Write(orderString.ToString());
		}
		/// <summary>
		/// 打印元素值
		/// </summary>
		/// <param name="xNodeList"></param>
		private void PrintNewElementList(List<XmlElement> xNodeList)
		{
			StringBuilder orderString = new StringBuilder();
			for (int i = 0; i < _TotalCount; i++)
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
					ImageUrl = GetImageUrl((XmlElement)xNodeList[i]).Replace("_1.", "_5.");
				}
				//子品牌链接
				string serialUrl = GetSerialUrl(xNodeList[i]);
				orderString.AppendFormat("<li><a href='{1}' target='_blank'><img width='90' height='60' src='{0}'>{2}</a></li>"
									   , ImageUrl
									   , serialUrl
									   , xNodeList[i].Attributes["Name"].Value.ToString());
			}

			Response.Write(orderString.ToString());
		}
	}
}