using System;
using System.IO;
using System.Text;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.Exhibition
{
	/// <summary>
	/// 车展通用接口(朱永旭 dept:bitautocms)
	/// </summary>
	public partial class CarExhibitionInterface : ExhibitionPageBase
	{
		// 业务
		private string dept = "";
		// 操作类型
		private string operateType = "";

		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		private XmlDocument _AlbumXmlDoc = new XmlDocument();
		private BitAuto.CarChannel.BLL.Exhibition _ExhibitionBLL = new BitAuto.CarChannel.BLL.Exhibition();
		private ExhibitionAlbum _AlbumBLL = new ExhibitionAlbum();

		private int type = 0;
		private int NewCarTypeId = 0;

		private int _PavilionId = 0;
		private int _Count = 9999;

		#region 取车型关注排行 op=gethotcarlist
		private int _HotCarListType = 0;
		private int _HotCarListStandardValue = 10000;
		private int _HotCarListQueneId = 0;
		private DataSet _HotCarListQueneDs = new DataSet();
		#endregion

		#region 首页分类8小图 根据属性ID
		private int _EightPicByAttributeID_AttributeId = 0;
		private int _EightPicByAttributeID_ShowRowCount = 0;
		#endregion

		#region 根据标签ID取子品牌列表
		private int _SerialListByAttrID_AttributeId = 0;
		private int _SerialListByAttrID_TotalCount = 8;
		private string _SerialListByAttrID_removeSerialId = "";
		private string _SerialListByAttrID_MostCount = ",11,";
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				if (dept == "bitautocms")
				{
					if (base._ExhibitionID <= 0)
					{
						// 车展ID异常跳过
						return;
					}
					if (operateType == "getfooter")
					{
						// 网上展厅-文字(输出带格式)
						GetExhibitionFooter();
					}
					else if (operateType == "getnewcar")
					{
						// 根据分类取热门车
						GetHotCarByCate();
					}
					else if (operateType == "geteightpic")
					{
						// 首页分类8小图 根据级别
						GetEightPic();
					}
					else if (operateType == "geteightpicbyid")
					{
						// 首页分类8小图 根据属性ID
						GetEightPicByAttributeID();
					}
					else if (operateType == "getpavilionlist")
					{
						// 取展厅小图
						GetPavilionList();
					}
					else if (operateType == "gethotcarlist")
					{
						// 取车型关注排行 (带格式输出)
						GetHotCarList();
					}
					else if (operateType == "getexhibitionbaseinfo")
					{
						// 车展基本信息 展馆页接口&上市新车——价格区间、车型类别
						GetExhibitionBaseInfo();
					}
					else if (operateType == "getcsbyattrid")
					{
						// 根据标签取子品牌列表
						GetSerialListByAttrID();
					}
					else if (operateType == "test")
					{
						// 测试
						GetTest();
					}
					else
					{ }
				}
				// Response.End();
			}
		}

		#region private Method

		private void GetPageParam()
		{
			// 业务类型
			if (this.Request.QueryString["dept"] != null && this.Request.QueryString["dept"].ToString() != "")
			{
				dept = this.Request.QueryString["dept"].ToString().ToLower();
			}
			// 操作类型
			if (this.Request.QueryString["op"] != null && this.Request.QueryString["op"].ToString() != "")
			{
				operateType = this.Request.QueryString["op"].ToString().ToLower();
			}

			type = string.IsNullOrEmpty(Request.QueryString["type"])
					? 0 : ConvertHelper.GetInteger(Request.QueryString["type"].ToString());
			//NewCarTypeId = string.IsNullOrEmpty(Request.QueryString["attr"])
			//         ? 10 : ConvertHelper.GetInteger(Request.QueryString["attr"].ToString());
			NewCarTypeId = string.IsNullOrEmpty(Request.QueryString["attr"])
					? 0 : ConvertHelper.GetInteger(Request.QueryString["attr"].ToString());

		}

		/// <summary>
		/// 得到链接的样式
		/// </summary>
		private string GetAhrefStyle()
		{
			// 2011 广州车展
			if (_ExhibitionID == 71)
			{
				return "<li><a href=\"{0}\" target=\"_blank\">"
							   + "<img height=\"80\" width=\"120\" src=\"{1}\" alt=\"{2}\">{2}</a>{3}</li>";
			}
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
		#endregion

		#region 车展方法

		#region 网上展厅-文字(输出带格式) op=getfooter

		private void GetExhibitionFooter()
		{
			Response.Write(GetExhibitionFooterByExhibitionId());
		}

		#endregion

		#region 热门车数据 op=getnewcar

		private void GetHotCarByCate()
		{

			#region 取数据
			_ExhibitionXmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (_ExhibitionXmlDoc == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root") == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}
			_AlbumXmlDoc = _AlbumBLL.GetCommonAlbumRelationData(_ExhibitionID);
			if (_AlbumXmlDoc == null
				|| _AlbumXmlDoc.SelectSingleNode("Data") == null
				|| _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes == null
				|| _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes.Count < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}
			#endregion

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
				case 7:	// 新增跑车
					content = BuildPaoChe();
					break;
				case 8:	// 新增跑车 豪华车 合并输出
					content = BuildPaoCheAndMostSale();
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
			string content = "";
			XmlNodeList xNodeList = null;
			if (NewCarTypeId > 0)
			{
				xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@Country='国产']/Attribute[@ID='"
											 + NewCarTypeId.ToString() + "']");
				if (xNodeList == null || xNodeList.Count < 1)
				{
					return content;
				}
				content = NodeListOrder(xNodeList, false);
			}
			else
			{
				xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@Country='国产']");
				if (xNodeList == null || xNodeList.Count < 1)
				{
					return content;
				}
				content = NodeListOrder(xNodeList, true);
			}
			return content;
		}

		/// <summary>
		/// 产生进口新车
		/// </summary>
		/// <returns></returns>
		private string BuildForeginNewCar()
		{
			string content = "";
			XmlNodeList xNodeList = null;
			if (NewCarTypeId > 0)
			{
				xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@Country='进口']/Attribute[@ID='"
										 + NewCarTypeId.ToString() + "']");

				if (xNodeList == null || xNodeList.Count < 1)
				{
					return content;
				}
				content = NodeListOrder(xNodeList, false);
			}
			else
			{
				xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@Country='进口']");

				if (xNodeList == null || xNodeList.Count < 1)
				{
					return content;
				}
				content = NodeListOrder(xNodeList, true);
			}
			return content;
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

			return NodeListOrder(xNodeList, false);
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
		/// 产生跑车
		/// </summary>
		/// <returns></returns>
		private string BuildPaoChe()
		{
			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@CsLevel='跑车']");
			if (xNodeList == null || xNodeList.Count < 1)
			{
				return "";
			}
			return ListOrder(xNodeList);
		}

		/// <summary>
		/// 产生豪华车
		/// </summary>
		/// <returns></returns>
		private string BuildPaoCheAndMostSale()
		{
			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@CsLevel='跑车' or @CsLevel='豪华车']");
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

			return NodeListOrder(xNodeList, false);
		}

		/// <summary>
		/// 子结点排序
		/// </summary>
		/// <param name="xNodeList"></param>
		/// <param name="isparentNode">是否已经是父节点</param>
		/// <returns></returns>
		private string NodeListOrder(XmlNodeList xNodeList, bool isparentNode)
		{
			List<XmlElement> xElemeList = new List<XmlElement>();

			foreach (XmlElement xEleme in xNodeList)
			{
				XmlElement newXmlElem = null;
				if (isparentNode)
				{
					newXmlElem = ((XmlElement)xEleme);
				}
				else
				{
					newXmlElem = ((XmlElement)xEleme.ParentNode);
				}

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

			xElemeList.Sort(BitAuto.CarChannel.BLL.ExhibitionAlbum.OrderXmlElement);
			if (_ExhibitionID <= 48)
			{
				return BuildHTMLList(xElemeList, 5);
			}
			return BuildNewHTMLLit(xElemeList, 5);
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

			xElemeList.Sort(BitAuto.CarChannel.BLL.ExhibitionAlbum.OrderXmlElement);

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

		#endregion

		#region 首页分类8小图 根据级别 op=geteightpic

		private void GetEightPic()
		{
			string _AllSpell = string.IsNullOrEmpty(Request.QueryString["allspell"])
							? "" : Request.QueryString["allspell"].ToString();

			if (string.IsNullOrEmpty(_AllSpell))
			{
				Response.Write("");
				// Response.End();
				return;
			}

			// 支持多级别合并
			List<string> listLevel = new List<string>();
			listLevel = GetLevelStringList(_AllSpell);
			if (listLevel.Count < 1)
			{
				Response.Write("");
				return;
			}

			string csLevelWhere = "";
			foreach (string level in listLevel)
			{
				if (csLevelWhere != "")
				{ csLevelWhere += " or "; }
				csLevelWhere += " @CsLevel='" + level + "' ";
			}

			// string csLevel = GetLevelString(_AllSpell);
			//if (string.IsNullOrEmpty(csLevel))
			//{
			//    Response.Write("");
			//    // Response.End();
			//    return;
			//}

			_ExhibitionXmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (_ExhibitionXmlDoc == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root") == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}
			_AlbumXmlDoc = _AlbumBLL.GetCommonAlbumRelationData(_ExhibitionID);
			if (_AlbumXmlDoc == null
			   || _AlbumXmlDoc.SelectSingleNode("Data") == null
			   || _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes == null
			   || _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes.Count < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}

			string xPath = string.Format("root/MasterBrand/Brand/Serial[{0}]", csLevelWhere);
			if (_ExhibitionID > 48)
			{
				xPath = string.Format("root/MasterBrand/Brand/Serial[({0}) and @NC=1]", csLevelWhere);
			}

			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes(xPath);

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				// Response.End();
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

			xElemeList.Sort(BitAuto.CarChannel.BLL.ExhibitionAlbum.OrderXmlElement);
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
					ImageUrl = BitAuto.CarChannel.Common.WebConfig.DefaultCarPic;
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

				if (_ExhibitionID == 71)
				{
					// 2011 广州车展
					string moreLink = "<span><a target=\"_blank\" href=\"http://photo.bitauto.com/serial/" + serialId + "/\">图库</a> | <a target=\"_blank\" href=\"http://car.bitauto.com/" + serialSpell + "/shipin/?pageindex=1&order=_new\">视频</a>";
					if (base._DicSerialJieXi.ContainsKey(serialId))
					{ moreLink += " | <a target=\"_blank\" href=\"" + base._DicSerialJieXi[serialId] + "\">解析</a>"; }
					moreLink += "</span>";
					//拼接li样式
					listString.AppendFormat(astyle
											, serialUrl
											, GetSettingImageUrl(ImageUrl)
											, xEleme.GetAttribute("Name")
											, moreLink);
				}
				else
				{
					//拼接li样式
					listString.AppendFormat(astyle
											, serialUrl
											, GetSettingImageUrl(ImageUrl)
											, xEleme.GetAttribute("Name"));
				}
			}
			Response.Write(listString.ToString());
		}



		#endregion

		#region 首页分类8小图 根据属性ID op=geteightpicbyid

		private void GetEightPicByAttributeID()
		{
			#region 取参数
			_EightPicByAttributeID_AttributeId = string.IsNullOrEmpty(Request.QueryString["ID"])
			   ? 0 : ConvertHelper.GetInteger(Request.QueryString["ID"].ToString());
			_EightPicByAttributeID_ShowRowCount = string.IsNullOrEmpty(Request.QueryString["Count"])
				? 8 : ConvertHelper.GetInteger(Request.QueryString["Count"].ToString());
			#endregion

			#region 取数据
			if (_EightPicByAttributeID_AttributeId < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}

			_ExhibitionXmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (_ExhibitionXmlDoc == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root") == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}
			_AlbumXmlDoc = _AlbumBLL.GetCommonAlbumRelationData(_ExhibitionID);
			if (_AlbumXmlDoc == null
			   || _AlbumXmlDoc.SelectSingleNode("Data") == null
			   || _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes == null
			   || _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes.Count < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}

			if (_EightPicByAttributeID_ShowRowCount < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}

			PrintDataForEightPicByAttributeID();
			#endregion
		}

		/// <summary>
		/// 打印数据
		/// </summary>
		private void PrintDataForEightPicByAttributeID()
		{
			string xPath = string.Format("root/MasterBrand/Brand/Serial/Attribute[@ID={0}]", _EightPicByAttributeID_AttributeId);
			if (_ExhibitionID > 48)
			{
				xPath = string.Format("root/MasterBrand/Brand/Serial[@NC=1]/Attribute[@ID={0}]", _EightPicByAttributeID_AttributeId);
			}

			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes(xPath);

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}

			Dictionary<int, int> orderSerial = new Dictionary<int, int>();
			orderSerial = _ExhibitionBLL.GetOrderSerialTypeByAttributeId(_EightPicByAttributeID_AttributeId, 3);

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

			SurplusCount = _EightPicByAttributeID_ShowRowCount - xElemeList.Count;
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
				imgElemeList.Sort(BitAuto.CarChannel.BLL.ExhibitionAlbum.OrderXmlElement);

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
				// Response.End();
				return;
			}
			StringBuilder orderString = new StringBuilder();
			for (int i = 0; i < _EightPicByAttributeID_ShowRowCount; i++)
			{
				if (i + 1 > xNodeList.Count)
				{
					break;
				}

				string ImageUrl = "";
				if (xNodeList[i].Attributes["ImageUrl"] == null || string.IsNullOrEmpty(xNodeList[i].Attributes["ImageUrl"].Value))
				{
					ImageUrl = BitAuto.CarChannel.Common.WebConfig.DefaultCarPic;
				}
				else
				{
					ImageUrl = GetImageUrl((XmlElement)xNodeList[i]);
				}
				//子品牌链接
				string serialUrl = GetSerialUrl(xNodeList[i]);
				string astyle = GetAhrefStyle();
				//拼接li样式
				if (_ExhibitionID == 71)
				{
					int csid = int.Parse(xNodeList[i].Attributes["ID"].Value.ToString());
					// 2011 广州车展
					string moreLink = "<span><a target=\"_blank\" href=\"http://photo.bitauto.com/serial/" + csid + "/\">图库</a> | <a target=\"_blank\" href=\"http://car.bitauto.com/" + xNodeList[i].Attributes["AllSpell"].Value.ToString() + "/shipin/?pageindex=1&order=_new\">视频</a>";
					if (base._DicSerialJieXi.ContainsKey(csid))
					{ moreLink += " | <a target=\"_blank\" href=\"" + base._DicSerialJieXi[csid] + "\">解析</a>"; }
					moreLink += "</span>";
					//拼接li样式
					orderString.AppendFormat(astyle
											, serialUrl
											, GetSettingImageUrl(ImageUrl)
											, xNodeList[i].Attributes["Name"].Value.ToString()
											, moreLink);
				}
				else
				{
					orderString.AppendFormat(astyle
											, serialUrl
											, GetSettingImageUrl(ImageUrl)
											, xNodeList[i].Attributes["Name"].Value.ToString());
				}
			}

			Response.Write(orderString.ToString());
		}

		/// <summary>
		/// 打印元素值
		/// </summary>
		/// <param name="xNodeList"></param>
		/// <param name="totalCount">最大数量</param>
		/// <param name="picFix">图片规格</param>
		/// <param name="width">图片宽</param>
		/// <param name="height">图片高</param>
		private void PrintNewElementList(List<XmlElement> xNodeList, int totalCount, int picFix, int width, int height)
		{
			// 图片规格
			if (picFix < 1 || picFix > 5)
			{ picFix = 5; }
			StringBuilder orderString = new StringBuilder();
			for (int i = 0; i < totalCount; i++)
			{
				if (i + 1 > xNodeList.Count)
				{
					break;
				}

				string ImageUrl = "";

				if (xNodeList[i].Attributes["ImageUrl"] == null || string.IsNullOrEmpty(xNodeList[i].Attributes["ImageUrl"].Value))
				{
					ImageUrl = BitAuto.CarChannel.Common.WebConfig.DefaultCarPic;
				}
				else
				{
					ImageUrl = GetImageUrl((XmlElement)xNodeList[i]).Replace("_1.", "_" + picFix + ".");
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

		#endregion

		#region 取展厅小图 op=getpavilionlist

		private void GetPavilionList()
		{
			#region 取参数
			_PavilionId = string.IsNullOrEmpty(Request.QueryString["ID"])
							   ? 0 : ConvertHelper.GetInteger(Request.QueryString["ID"].ToString());
			_Count = string.IsNullOrEmpty(Request.QueryString["c"])
				? 9999 : ConvertHelper.GetInteger(Request.QueryString["c"]);
			#endregion

			_ExhibitionXmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (_ExhibitionXmlDoc == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root") == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}
			Dictionary<int, Pavilion> pavilionList = new Dictionary<int, BitAuto.CarChannel.Model.Pavilion>();
			pavilionList = new BitAuto.CarChannel.BLL.Exhibition().GetPavilionListByExhibitionId(_ExhibitionID);

			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand[@PavilionId='"
									+ _PavilionId.ToString() + "']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}

			List<XmlElement> xElemList = new List<XmlElement>();
			foreach (XmlElement entity in xNodeList)
			{
				xElemList.Add(entity);
			}
			xElemList.Sort(BitAuto.CarChannel.BLL.Exhibition.OrderXmlElement);

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
				string url = "";
				if (base._DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && base._DicExhibitionBaseInfo[_ExhibitionID].PravilionUrlFormat != "")
				{
					url = base._DicExhibitionBaseInfo[_ExhibitionID].PravilionUrlFormat;
				}
				//url = _PravilionUrlFormat[_ExhibitionID];
				string brandid = xElem.GetAttribute("ID");
				BrandUrl = string.Format(url, PavilionUrl[pavilionList[ConvertHelper.GetInteger(xElem.GetAttribute("PavilionId"))].Name]) + brandid + "/";
				BrandLogoUrl = string.Format("http://image.bitautoimg.com/carchannel/logo/brand/55png/b_{0}_p55.png", brandid);
				pavilionString.AppendFormat("<li class='{3}'><a target=\"_blank\" href=\"{0}\"><img alt=\"{1}\" src=\"{2}\">{1}</a></li>"
											, BrandUrl
											, xElem.GetAttribute("Name")
											, BrandLogoUrl
											, index == 1 ? "fist" : "");
				index++;
			}

			Response.Write(pavilionString.ToString());
		}

		#endregion

		#region  取车型关注排行 op=gethotcarlist

		private void GetHotCarList()
		{
			#region 取参数
			_HotCarListType = string.IsNullOrEmpty(Request.QueryString["type"])
			   ? 0 : ConvertHelper.GetInteger(Request.QueryString["type"].ToString());
			_HotCarListQueneId = string.IsNullOrEmpty(Request.QueryString["quene"])
				? 0 : ConvertHelper.GetInteger(Request.QueryString["quene"].ToString());
			#endregion

			#region 验证参数

			if (_HotCarListType == 0)
			{
				Response.Write("");
				// Response.End();
				return;
			}
			_ExhibitionXmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (_ExhibitionXmlDoc == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root") == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}
			string QueneData = BitAuto.CarChannel.Common.CommonFunction.GetContentByUrl(GetUserVoteUrl());
			if (string.IsNullOrEmpty(QueneData))
			{
				Response.Write("");
				// Response.End();
				return;
			}

			try
			{
				using (StringReader sr = new StringReader(QueneData))
				{
					_HotCarListQueneDs.ReadXml(sr, XmlReadMode.InferTypedSchema);
				}
				if (_HotCarListQueneDs == null || _HotCarListQueneDs.Tables.Count < 1 || _HotCarListQueneDs.Tables[0].Rows.Count < 1)
				{
					Response.Write("");
					// Response.End();
					return;
				}
			}
			catch
			{
				Response.Write("");
				// Response.End();
				return;
			}

			#endregion

			#region

			switch (_HotCarListType)
			{
				case 1:
					Response.Write(InitCarLevel());
					break;
				case 2:
					Response.Write(InitCarAttribute());
					break;
				case 3:
					Response.Write(InitCarPrice());
					break;
				default:
					Response.Write("");
					break;
			}

			#endregion
		}

		/// <summary>
		/// 打印车的级别
		/// </summary>
		/// <returns></returns>
		private string InitCarLevel()
		{
			if (!CsLevelReverse.ContainsKey(_HotCarListQueneId.ToString()))
			{
				return "";
			}
			StringBuilder htmlString = new StringBuilder();
			int index = 1;
			foreach (DataRow dr in _HotCarListQueneDs.Tables[0].Rows)
			{
				if (index > 10)
				{
					break;
				}
				string selectPath = "root/MasterBrand/Brand/Serial[@ID='"
								   + dr["ukey"].ToString()
								   + "' and @CsLevel='"
								   + CsLevelReverse[_HotCarListQueneId.ToString()] + "']";
				XmlElement xEleme = (XmlElement)_ExhibitionXmlDoc.SelectSingleNode(selectPath);
				if (xEleme != null)
				{
					htmlString.Append(PrintfHTML(xEleme, dr["pv"].ToString(), index));
					index++;
				}
			}
			return htmlString.ToString();
		}

		/// <summary>
		/// 打印车的标签
		/// </summary>
		/// <returns></returns>
		private string InitCarAttribute()
		{
			if (!AttributeUrlReverse.ContainsKey(_HotCarListQueneId.ToString()))
			{
				return "";
			}
			StringBuilder htmlString = new StringBuilder();
			int index = 1;
			foreach (DataRow dr in _HotCarListQueneDs.Tables[0].Rows)
			{
				if (index > 10)
				{
					break;
				}
				string selectPath = "root/MasterBrand/Brand/Serial[@ID='"
								  + dr["ukey"].ToString()
								  + "']/Attribute[@ID='"
								  + AttributeUrlReverse[_HotCarListQueneId.ToString()] + "']";
				XmlElement xEleme = (XmlElement)_ExhibitionXmlDoc.SelectSingleNode(selectPath);
				if (xEleme != null)
				{
					htmlString.Append(PrintfHTML((XmlElement)xEleme.ParentNode, dr["pv"].ToString(), index));
					index++;
				}
			}
			return htmlString.ToString();
		}

		/// <summary>
		/// 打印车的价格
		/// </summary>
		/// <returns></returns>
		private string InitCarPrice()
		{
			if (_HotCarListQueneId < 1 || _HotCarListQueneId > 8)
			{
				return "";
			}
			StringBuilder htmlString = new StringBuilder();
			int index = 1;
			foreach (DataRow dr in _HotCarListQueneDs.Tables[0].Rows)
			{
				if (index > 10)
				{
					break;
				}
				string selectPath = "root/MasterBrand/Brand/Serial[@ID='"
						+ dr["ukey"].ToString()
						+ "' and contains(@MultiPriceRange,',"
						+ _HotCarListQueneId.ToString() + ",')]";
				XmlElement xEleme = (XmlElement)_ExhibitionXmlDoc.SelectSingleNode(selectPath);
				if (xEleme != null)
				{
					htmlString.Append(PrintfHTML(xEleme, dr["pv"].ToString(), index));
					index++;
				}
			}
			return htmlString.ToString();
		}

		/// <summary>
		/// 打印HTML
		/// </summary>
		/// <param name="name">子品牌名</param>
		/// <param name="url">子品牌url</param>
		/// <param name="count">数量</param>
		/// <returns></returns>
		private string PrintfHTML(XmlElement xElem, string count, int index)
		{
			//子品牌链接地址
			string serialUrl = GetSerialUrl(xElem);
			StringBuilder liString = new StringBuilder();
			liString.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a><small>{2}</small></li>"
								  , serialUrl
								  , xElem.GetAttribute("Name")
								  , (_HotCarListStandardValue + ConvertHelper.GetInteger(count) * 100 + ConvertHelper.GetInteger(10 - index) * (10 - index) - new Random().Next(2, 10)));
			return liString.ToString();
		}

		#endregion

		#region  车展基本信息 op=getexhibitionbaseinfo

		private void GetExhibitionBaseInfo()
		{
			Response.ContentType = "text/xml";
			if (_ExhibitionID < 1)
			{
				Response.Write("");
				return;
			}
			_ExhibitionXmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (_ExhibitionXmlDoc == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root") == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				// Response.End();
				return;
			}
			_AlbumXmlDoc = _AlbumBLL.GetCommonAlbumRelationData(_ExhibitionID);
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
				string url = "";
				if (base._DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && base._DicExhibitionBaseInfo[_ExhibitionID].UrlFormat != "")
				{
					url = _DicExhibitionBaseInfo[_ExhibitionID].UrlFormat.Replace("{0}/", "").Replace("{1}/", "");
				}
				// url = _UrlFormat[_ExhibitionID].Replace("{0}/", "").Replace("{1}/", "");
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
					// modified by chengl Dec.14.2011
					XmlElement temp = (XmlElement)model.Clone();
					string modelUrl = targetUrl + model.GetAttribute("TargetUrl");
					// model.SetAttribute("TargetUrl", modelUrl);
					temp.SetAttribute("TargetUrl", modelUrl);
					string focusUrl = BitAuto.CarChannel.Common.CommonFunction.GetPublishHashImageDomain(ConvertHelper.GetInteger(model.GetAttribute("ImageId")))
									+ model.GetAttribute("ImageUrl").Replace("_1.", "_8.");
					// model.SetAttribute("ImageUrl", focusUrl);
					temp.SetAttribute("ImageUrl", focusUrl);
					// xEleme.AppendChild(xEleme.OwnerDocument.ImportNode(model, false));
					xEleme.AppendChild(xEleme.OwnerDocument.ImportNode(temp, false));
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
							serialImageUrl = BitAuto.CarChannel.Common.WebConfig.DefaultCarPic;
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
						serialImageUrl = BitAuto.CarChannel.Common.WebConfig.DefaultCarPic;
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
					if (base._DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && base._DicExhibitionBaseInfo[_ExhibitionID].UrlFormat != "")
					{
						xEleme.SetAttribute("Url", string.Format(_DicExhibitionBaseInfo[_ExhibitionID].UrlFormat
										  , xEleme.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
										  , xEleme.GetAttribute("AllSpell")));
					}
					//xEleme.SetAttribute("Url", string.Format(_UrlFormat[_ExhibitionID]
					//                          , xEleme.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
					//                          , xEleme.GetAttribute("AllSpell")));
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
			Dictionary<int, BitAuto.CarChannel.Model.Attribute> attrList = _ExhibitionBLL.GetAttributeListByExhibitionId(_ExhibitionID);
			if (attrList == null || attrList.Count < 1)
			{
				return "";
			}
			StringBuilder attrString = new StringBuilder();
			attrString.Append("<AttributeList>");
			foreach (KeyValuePair<int, BitAuto.CarChannel.Model.Attribute> entity in attrList)
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
			Dictionary<int, BitAuto.CarChannel.Model.Pavilion> pavilionList = _ExhibitionBLL.GetPavilionListByExhibitionId(_ExhibitionID);
			if (pavilionList == null || pavilionList.Count < 1)
			{
				return "";
			}
			StringBuilder pavilionString = new StringBuilder();
			pavilionString.Append("<PavilionList>");
			foreach (KeyValuePair<int, BitAuto.CarChannel.Model.Pavilion> entity in pavilionList)
			{
				pavilionString.AppendFormat("<Pavilion ID=\"{0}\" Name=\"{1}\"/>", entity.Value.ID.ToString(), entity.Value.Name);
			}
			pavilionString.Append("</PavilionList>");
			return pavilionString.ToString();
		}

		#endregion

		#region 根据数据ID取子品牌列表 op=getcsbyattrid

		private void GetSerialListByAttrID()
		{
			#region 取参数
			_SerialListByAttrID_AttributeId = string.IsNullOrEmpty(Request.QueryString["id"])
				? 0 : ConvertHelper.GetInteger(Request.QueryString["id"].ToString());
			_SerialListByAttrID_TotalCount = string.IsNullOrEmpty(Request.QueryString["num"])
				? 8 : ConvertHelper.GetInteger(Request.QueryString["num"]);
			_SerialListByAttrID_removeSerialId = string.IsNullOrEmpty(Request.QueryString["rs"])
				? "" : Request.QueryString["rs"];
			#endregion

			if (_SerialListByAttrID_AttributeId < 1)
			{
				Response.Write("");
				return;
			}

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
			_AlbumXmlDoc = _AlbumBLL.GetCommonAlbumRelationData(_ExhibitionID);
			if (_AlbumXmlDoc == null
			   || _AlbumXmlDoc.SelectSingleNode("Data") == null
			   || _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes == null
			   || _AlbumXmlDoc.SelectSingleNode("Data").ChildNodes.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}

			PrintDataByAttrID();
		}

		/// <summary>
		/// 打印数据
		/// </summary>
		private void PrintDataByAttrID()
		{
			if (_SerialListByAttrID_MostCount.IndexOf("," + _SerialListByAttrID_AttributeId.ToString() + ",") >= 0 && _ExhibitionID == 19)
			{
				_SerialListByAttrID_TotalCount = 16;
			}

			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial/Attribute[@ID='"
								   + _SerialListByAttrID_AttributeId.ToString() + "']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				Response.Write("");
				return;
			}

			//定义要移除的子品牌数据
			List<int> removeSerailList = new List<int>();
			//如果要移除的列表存在
			if (!string.IsNullOrEmpty(_SerialListByAttrID_removeSerialId))
			{
				foreach (string idstr in _SerialListByAttrID_removeSerialId.Split(','))
				{
					removeSerailList.Add(ConvertHelper.GetInteger(idstr));
				}
			}

			Dictionary<int, int> orderSerial = new Dictionary<int, int>();
			orderSerial = _ExhibitionBLL.GetOrderSerialTypeByAttributeId(_SerialListByAttrID_AttributeId, 2);

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

			SurplusCount = _SerialListByAttrID_TotalCount - SurplusCount - removeSerailList.Count;
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
				imgElemeList.Sort(BitAuto.CarChannel.BLL.ExhibitionAlbum.OrderXmlElement);

				for (int i = 0; i < SurplusCount; i++)
				{
					if (i + 1 > imgElemeList.Count)
					{
						break;
					}
					xElemeList.Add(imgElemeList[i]);
				}
			}
			PrintNewElementList(xElemeList, _SerialListByAttrID_TotalCount, 5, 90, 60);
		}

		#endregion

		#region 测试

		private void GetTest()
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

			string xPath = "root/MasterBrand/Brand/Serial[@NC=1]";
			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes(xPath);
			Response.ContentType = "text/xml";
			Response.Write("<Root>\r\n");
			foreach (XmlNode xn in xNodeList)
			{
				Response.Write(xn.OuterXml + "\r\n");
			}
			Response.Write("<Root>");
		}

		#endregion

		#endregion

	}
}