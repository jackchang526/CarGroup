using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannelAPI.Web.AppCode;
using System.Text;
using BitAuto.Utils;
using System.Xml;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using System.Web.UI;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannelAPI.Web.Exhibition
{
	/// <summary>
	/// CarExhibitionXml 的摘要说明
	/// </summary>
	public class CarExhibitionXml : ExhibitionPageBase, IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;
		BitAuto.CarChannel.BLL.Exhibition exhibitionBLL = new BitAuto.CarChannel.BLL.Exhibition();
		ExhibitionAlbum _AlbumBLL = new ExhibitionAlbum();
		XmlDocument xmlExhibition;
		private XmlDocument _PavilionMasterXml = new XmlDocument();
		public void ProcessRequest(HttpContext context)
		{
			//base.SetPageCache(10);
			// delete cache by chengl Apr.18.2013
			//OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
			//{
			//    Duration = 60 * 10,
			//    Location = OutputCacheLocation.Any,
			//    VaryByParam = "*"
			//});
			//page.ProcessRequest(HttpContext.Current);
			response = context.Response;
			request = context.Request;
			GetParams();
			InitData();
			string action = request.QueryString["action"];
			switch (action)
			{
				case "getpavilionlist": GetPavilionList(); break;
				case "getpavilionlistv2": GetPavilionListV2(); break;
				case "getnewcar": GetCarByCategory(); break;
				case "getpavilionlistforphoto": ExhibitionCarTypeList(); break;
				default: Echo(""); break;
			}
			response.End();
		}
		/// <summary>
		/// 初始化 参数
		/// </summary>
		private void GetParams()
		{
			_ExhibitionID = ConvertHelper.GetInteger(request.QueryString["eid"]);
			if (_ExhibitionID <= 0)
				Echo("");
		}
		/// <summary>
		/// 初始化数据
		/// </summary>
		private void InitData()
		{
			base.IntiExhibitionByID(_ExhibitionID);
			xmlExhibition = ExhibitionXML;// exhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			_PavilionMasterXml = exhibitionBLL.GetPavilionMasterXmlById(_ExhibitionID);
		}
		public XmlDocument ExhibitionXML
		{
			get
			{
				string key = "Car_api_Exhibition_Xml";
				object obj = CacheManager.GetCachedData(key);
				if (obj != null)
					return (XmlDocument)obj;
				XmlDocument xmlDoc = exhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
				CacheManager.InsertCache(key, xmlDoc, WebConfig.CachedDuration);
				return xmlDoc;
			}
		}
		/// <summary>
		/// 根据分类获取新车数据
		/// </summary>
		private void GetCarByCategory()
		{
			XmlDocument _AlbumXmlDoc = _AlbumBLL.GetCommonAlbumRelationData(_ExhibitionID);
			StringBuilder sb = new StringBuilder();
			string attrId = request.QueryString["attr"];
			string isImport = request.QueryString["isImport"];
			string levelAllSpell = request.QueryString["allspell"];
			string imgSubfix = request.QueryString["subfix"];
			int count = ConvertHelper.GetInteger(request.QueryString["count"]);
			int type = ConvertHelper.GetInteger(request.QueryString["type"]);
			string xpath = "root/MasterBrand/Brand/Serial{0}{1}";
			List<string> listSerialWhere = new List<string>();
			if (!string.IsNullOrEmpty(levelAllSpell))
			{
				List<string> listLevel = base.GetLevelStringList(levelAllSpell);
				foreach (string level in listLevel)
					listSerialWhere.Add("@CsLevel='" + level + "'");
			}
			if (!string.IsNullOrEmpty(isImport))
			{
				if (isImport == "0")
					listSerialWhere.Add("@Country='国产'");
				else
					listSerialWhere.Add("@Country='进口'");
			}

			List<string> listAttrWhere = new List<string>();
			if (!string.IsNullOrEmpty(attrId))
			{
				xpath = string.Format(xpath, "{0}", "/Attribute[{1}]");
				listAttrWhere.Add("@ID='" + attrId + "'");
			}

			xpath = string.Format(xpath,
				listSerialWhere.Count > 0 ? "[(" + string.Join(" or ", listSerialWhere.ToArray()) + ")]" : "",//and @NC=1
				listAttrWhere.Count > 0 ? string.Join(" or ", listAttrWhere.ToArray()) : "");

			XmlNodeList nodeList = xmlExhibition.SelectNodes(xpath); ;

			if (nodeList == null || nodeList.Count < 1)
			{
				Echo("");
				return;
			}
			int i = 0;

			List<SerialNodeStr> newList = new List<SerialNodeStr>();

			foreach (XmlNode tmpNode in nodeList)
			{
				XmlNode node = tmpNode;
				if (!string.IsNullOrEmpty(attrId))
					node = tmpNode.ParentNode;
				XmlElement albumXmlElem = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='" + node.Attributes["ID"].Value + "']");
				if (albumXmlElem == null
					|| ConvertHelper.GetInteger(albumXmlElem.GetAttribute("Count")) == 0)
				{
					continue;
				}
				int imageCount = ConvertHelper.GetInteger(albumXmlElem.GetAttribute("Count"));
				// 子品牌的图库更新时间
				DateTime updateTime = new DateTime(1900, 1, 1);
				if (albumXmlElem != null && albumXmlElem.GetAttribute("UpdateTime") != null)
				{
					updateTime = Convert.ToDateTime(albumXmlElem.GetAttribute("UpdateTime"));
				}
				// 先不退出，取所有子品牌节点内容 按图库更新时间倒序后 按提取数量输出
				// i++;
				//if (count > 0 && i > count) break;
				int serialId = ConvertHelper.GetInteger(node.Attributes["ID"].Value);
				string serialUrl = string.Empty;
				string serialMUrl = string.Empty;
				string serialImageUrl = string.Empty;
				int isNewsCar = 0;
				if (node.Attributes["NC"] != null)
					isNewsCar = ConvertHelper.GetInteger(node.Attributes["NC"].Value);
				//新车
				if (isNewsCar == 1 && _AlbumXmlDoc != null)
				{
					if (string.IsNullOrEmpty(albumXmlElem.GetAttribute("ImageUrl")))
					{
						serialImageUrl = BitAuto.CarChannel.Common.WebConfig.DefaultCarPic;
					}
					else
						serialImageUrl = base.GetImageUrl(albumXmlElem);

					if (base._DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && base._DicExhibitionBaseInfo[_ExhibitionID].UrlFormat != "")
					{
						if (node.Attributes["AllSpell"] == null)
						{ continue; }
						string tempUrl = string.Format("{0}/{1}",
							node.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString(),
							node.Attributes["AllSpell"].Value);
						serialUrl = string.Format(_DicExhibitionBaseInfo[_ExhibitionID].UrlFormat,
							tempUrl);
						serialMUrl = serialMUrl = serialUrl.Replace("chezhan.bitauto.com", "chezhan.m.yiche.com");
					}
				}
				else
				{
					continue;//add by sk 2016.04.25 by liubin
					int imgCount = 0;
					base.GetSerialPicAndCountByCsID(serialId, out serialImageUrl, out imgCount, true);
					serialUrl = string.Format("http://car.bitauto.com/{0}/", node.Attributes["AllSpell"].Value);
					serialMUrl = string.Format("http://car.m.yiche.com/{0}/", node.Attributes["AllSpell"].Value);
				}
				serialImageUrl = serialImageUrl.Replace("_1.", "_4.");
				serialImageUrl = serialImageUrl.Replace("_2.", "_3.");
				// modified by chengl Apr.13.2015 图片后缀自定义
				if (!string.IsNullOrEmpty(imgSubfix))
				{ serialImageUrl = serialImageUrl.Replace("_4.", "_" + imgSubfix + "."); }

				SerialNodeStr sns = new SerialNodeStr();
				StringBuilder _tmpSb = new StringBuilder(4);
				_tmpSb.Append("<Serial>");
				_tmpSb.AppendFormat("<Name>{0}</Name>", node.Attributes["Name"].Value);
				_tmpSb.AppendFormat("<ImgUrl>{0}</ImgUrl>", serialImageUrl);
				_tmpSb.AppendFormat("<ImgCount>{0}</ImgCount>", imageCount);
				_tmpSb.AppendFormat("<TargetUrl>{0}</TargetUrl>", (type == 1 ? serialMUrl : serialUrl));
				_tmpSb.Append("</Serial>");
				sns.SerialString = _tmpSb.ToString();
				sns.SerialPhotoUpdateTime = updateTime;
				newList.Add(sns);
			}

			newList.Sort(CompareXmlNodeUpdateTime);
			foreach (SerialNodeStr sns in newList)
			{
				i++;
				if (count > 0 && i > count) break;
				sb.Append(sns.SerialString);
			}

			Echo(sb.ToString());
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sns1"></param>
		/// <param name="sns2"></param>
		/// <returns></returns>
		public static int CompareXmlNodeUpdateTime(SerialNodeStr sns1, SerialNodeStr sns2)
		{
			int ret = 0;
			if (sns1.SerialPhotoUpdateTime > sns2.SerialPhotoUpdateTime)
				ret = -1;
			else if (sns1.SerialPhotoUpdateTime < sns2.SerialPhotoUpdateTime)
				ret = 1;
			return ret;
		}

		/// <summary>
		/// 子品牌节点内容
		/// </summary>
		public struct SerialNodeStr
		{
			public string SerialString;							// 子品牌节点内容
			public DateTime SerialPhotoUpdateTime;		// 子品牌图库接口更新时间
		}

		/// <summary>
		/// 车展 展厅 下的主品牌 xml
		/// </summary>
		private void GetPavilionList()
		{
			StringBuilder sb = new StringBuilder();
			int pavilionId = ConvertHelper.GetInteger(request.QueryString["id"]);
			int count = ConvertHelper.GetInteger(request.QueryString["count"]);
			//XmlDocument xmlExhibition = exhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (xmlExhibition == null
				|| xmlExhibition.SelectSingleNode("root") == null
				|| xmlExhibition.SelectSingleNode("root").ChildNodes == null
				|| xmlExhibition.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Echo(sb.ToString());
				return;
			}
			Dictionary<int, Pavilion> pavilionList = new Dictionary<int, BitAuto.CarChannel.Model.Pavilion>();
			pavilionList = new BitAuto.CarChannel.BLL.Exhibition().GetPavilionListByExhibitionId(_ExhibitionID);
			foreach (KeyValuePair<int, Pavilion> key in pavilionList)
			{
				if (pavilionId > 0 && key.Key != pavilionId)
					continue;
				XmlNodeList xNodeList = xmlExhibition.SelectNodes("root/MasterBrand/Brand[@PavilionId='" + key.Key + "']");

				//补充一个品牌分布在多个展馆的情况。2015广州车展
				XmlNodeList pavilionMasterNodeList = null;
				if (_PavilionMasterXml != null && _PavilionMasterXml.HasChildNodes)
				{
					var pavilion = _PavilionMasterXml.SelectSingleNode("/root/Pavilion[@ID=" + pavilionId + "]");
					if (pavilion != null && pavilion.HasChildNodes)
					{
						pavilionMasterNodeList = pavilion.SelectNodes("MasterBrand");
					}
				}

				if ((xNodeList == null || xNodeList.Count < 1)
					&& (pavilionMasterNodeList == null || pavilionMasterNodeList.Count < 1))
				{
					continue;
				}
				sb.AppendFormat("<Pavilion>");
				sb.AppendFormat("<PavilionId>{0}</PavilionId>", key.Key);
				sb.AppendFormat("<PavilionName>{0}</PavilionName>", key.Value.Name);
				List<XmlElement> xElemList = new List<XmlElement>();
				foreach (XmlElement entity in xNodeList)
				{
					xElemList.Add(entity);
				}
				xElemList.Sort(BitAuto.CarChannel.BLL.Exhibition.OrderXmlElement);
				int i = 0;
				List<int> tempList = new List<int>();
				foreach (XmlNode node in xElemList)
				{
					XmlNode masterNode = node.ParentNode;
					if (masterNode == null)
						continue;
					int masterId = ConvertHelper.GetInteger(masterNode.Attributes["ID"].Value);
					if (tempList.Contains(masterId)) //排重主品牌
						continue;
					tempList.Add(masterId);
					i++;
					if (count > 0 && i > count) break;
					string url = string.Empty;
					if (base._DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && base._DicExhibitionBaseInfo[_ExhibitionID].UrlFormat != "")
					{
						url = string.Format(_DicExhibitionBaseInfo[_ExhibitionID].UrlFormat,
							masterNode.Attributes["AllSpell"].Value);
					}
					sb.Append("<Master>");
					sb.AppendFormat("<Id>{0}</Id>", masterId);
					sb.AppendFormat("<Name>{0}</Name>", masterNode.Attributes["Name"].Value);
					//sb.AppendFormat("<Logo>{0}</Logo>", "http://img1.bitauto.com/bt/car/default/images/carimage/autoshanghai2013/b_" + masterId + "_55.png");
					sb.AppendFormat("<Logo>{0}</Logo>", "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_" + masterId + "_55.png");
					sb.AppendFormat("<Url>{0}</Url>", url);
					sb.Append("</Master>");
				}
				if (pavilionMasterNodeList != null && pavilionMasterNodeList.Count > 0)
				{
					foreach (XmlNode masterNode in pavilionMasterNodeList)
					{
						int masterId = ConvertHelper.GetInteger(masterNode.Attributes["ID"].Value);
						if (tempList.Contains(masterId)) //排重主品牌
							continue;
						tempList.Add(masterId);
						i++;
						if (count > 0 && i > count) break;
						string url = string.Empty;
						if (base._DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && base._DicExhibitionBaseInfo[_ExhibitionID].UrlFormat != "")
						{
							url = string.Format(_DicExhibitionBaseInfo[_ExhibitionID].UrlFormat,
								masterNode.Attributes["AllSpell"].Value);
						}
						sb.Append("<Master>");
						sb.AppendFormat("<Id>{0}</Id>", masterId);
						sb.AppendFormat("<Name>{0}</Name>", masterNode.Attributes["Name"].Value);
						sb.AppendFormat("<Logo>{0}</Logo>", "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_" + masterId + "_55.png");
						sb.AppendFormat("<Url>{0}</Url>", url);
						sb.Append("</Master>");
					}
				}
				sb.AppendFormat("</Pavilion>");
			}
			Echo(sb.ToString());
		}

		/// <summary>
		/// 车展 展厅 下的主品牌 xml by 旭旭 无法转datatable
		/// </summary>
		private void GetPavilionListV2()
		{
			StringBuilder sb = new StringBuilder();
			int pavilionId = ConvertHelper.GetInteger(request.QueryString["id"]);
			int count = ConvertHelper.GetInteger(request.QueryString["count"]);
			//XmlDocument xmlExhibition = exhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (xmlExhibition == null
				|| xmlExhibition.SelectSingleNode("root") == null
				|| xmlExhibition.SelectSingleNode("root").ChildNodes == null
				|| xmlExhibition.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Echo(sb.ToString());
				return;
			}
			Dictionary<int, Pavilion> pavilionList = new Dictionary<int, BitAuto.CarChannel.Model.Pavilion>();
			pavilionList = new BitAuto.CarChannel.BLL.Exhibition().GetPavilionListByExhibitionId(_ExhibitionID);
			foreach (KeyValuePair<int, Pavilion> key in pavilionList)
			{
				if (pavilionId > 0 && key.Key != pavilionId)
					continue;
				XmlNodeList xNodeList = xmlExhibition.SelectNodes("root/MasterBrand/Brand[@PavilionId='" + key.Key + "']");

				//补充一个品牌分布在多个展馆的情况。2015广州车展
				XmlNodeList pavilionMasterNodeList = null;
				if (_PavilionMasterXml != null && _PavilionMasterXml.HasChildNodes)
				{
					var pavilion = _PavilionMasterXml.SelectSingleNode("/root/Pavilion[@ID=" + pavilionId + "]");
					if (pavilion != null && pavilion.HasChildNodes)
					{
						pavilionMasterNodeList = pavilion.SelectNodes("MasterBrand");
					}
				}

				if ((xNodeList == null || xNodeList.Count < 1)
					&& (pavilionMasterNodeList == null || pavilionMasterNodeList.Count < 1))
				{
					continue;
				}
				//sb.AppendFormat("<Pavilion Name=\"{0}\">", key.Value.Name);
				//sb.AppendFormat("<PavilionId>{0}</PavilionId>", key.Key);
				//sb.AppendFormat("<PavilionName>{0}</PavilionName>", key.Value.Name);
				List<XmlElement> xElemList = new List<XmlElement>();
				foreach (XmlElement entity in xNodeList)
				{
					xElemList.Add(entity);
				}
				xElemList.Sort(BitAuto.CarChannel.BLL.Exhibition.OrderXmlElement);
				int i = 0;
				List<int> tempList = new List<int>();
				foreach (XmlNode node in xElemList)
				{
					XmlNode masterNode = node.ParentNode;
					if (masterNode == null)
						continue;
					int masterId = ConvertHelper.GetInteger(masterNode.Attributes["ID"].Value);
					if (tempList.Contains(masterId)) //排重主品牌
						continue;
					tempList.Add(masterId);
					i++;
					if (count > 0 && i > count) break;
					string url = string.Empty;
					if (base._DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && base._DicExhibitionBaseInfo[_ExhibitionID].UrlFormat != "")
					{
						url = string.Format(_DicExhibitionBaseInfo[_ExhibitionID].UrlFormat,
							masterNode.Attributes["AllSpell"].Value);
					}
					sb.Append("<Master>");
					sb.AppendFormat("<Id>{0}</Id>", masterId);
					sb.AppendFormat("<Name>{0}</Name>", masterNode.Attributes["Name"].Value);
					//sb.AppendFormat("<Logo>{0}</Logo>", "http://img1.bitauto.com/bt/car/default/images/carimage/autoshanghai2013/b_" + masterId + "_55.png");
					sb.AppendFormat("<Logo>{0}</Logo>", "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_" + masterId + "_55.png");
					sb.AppendFormat("<Url>{0}</Url>", url);
					sb.Append("</Master>");
				}
				if (pavilionMasterNodeList != null && pavilionMasterNodeList.Count > 0)
				{
					foreach (XmlNode masterNode in pavilionMasterNodeList)
					{
						int masterId = ConvertHelper.GetInteger(masterNode.Attributes["ID"].Value);
						if (tempList.Contains(masterId)) //排重主品牌
							continue;
						tempList.Add(masterId);
						i++;
						if (count > 0 && i > count) break;
						string url = string.Empty;
						if (base._DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && base._DicExhibitionBaseInfo[_ExhibitionID].UrlFormat != "")
						{
							url = string.Format(_DicExhibitionBaseInfo[_ExhibitionID].UrlFormat,
								masterNode.Attributes["AllSpell"].Value);
						}
						sb.Append("<Master>");
						sb.AppendFormat("<Id>{0}</Id>", masterId);
						sb.AppendFormat("<Name>{0}</Name>", masterNode.Attributes["Name"].Value);
						sb.AppendFormat("<Logo>{0}</Logo>", "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_" + masterId + "_55.png");
						sb.AppendFormat("<Url>{0}</Url>", url);
						sb.Append("</Master>");
					}
				}
				//sb.AppendFormat("</Pavilion>");
			}
			Echo(sb.ToString());
		}

		/// <summary>
		/// 打印展会列表
		/// add by chengl Apr.18.2013
		/// </summary>
		private void ExhibitionCarTypeList()
		{
			HttpContext.Current.Response.ContentType = "text/xml";
			HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
			HttpContext.Current.Response.Clear();

			if (_ExhibitionID < 1)
			{
				response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				return;
			}

			XmlDocument xmlDoc = new XmlDocument();

			// modified by chengl 先读Data下xml文件，没有再读库
			string eidXml = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\Exhibition\\{0}.xml", _ExhibitionID));
			if (File.Exists(eidXml))
			{
				xmlDoc.Load(eidXml);
			}
			else
			{
				xmlDoc = exhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			}

			if (xmlDoc == null || !xmlDoc.HasChildNodes)
			{
				response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				return;
			}
			response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + xmlDoc.InnerXml);
		}

		//统一输出XML
		private void Echo(string str)
		{
			HttpContext.Current.Response.ContentType = "text/xml";
			HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
			HttpContext.Current.Response.Clear();

			StringBuilder sb = new StringBuilder();

			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			sb.Append("<Root>");
			sb.Append(str);
			sb.Append("</Root>");
			response.Write(sb.ToString());
			response.End();
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		private sealed class OutputCachedPage : Page
		{
			private OutputCacheParameters _cacheSettings;

			public OutputCachedPage(OutputCacheParameters cacheSettings)
			{
				ID = Guid.NewGuid().ToString();
				_cacheSettings = cacheSettings;
			}

			protected override void FrameworkInitialize()
			{
				base.FrameworkInitialize();
				InitOutputCache(_cacheSettings);
			}
		}
	}
}