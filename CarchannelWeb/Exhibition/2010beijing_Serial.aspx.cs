using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCB = BitAuto.CarChannel.BLL;
using BCM = BitAuto.CarChannel.Model;
using BCC = BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Exhibition
{
	public partial class _2010beijing_Serial : beijing_2010_PageBase
	{
		private string _SerialAllSpell = "";
		private XmlDocument _XmlDoc = new XmlDocument();
		private XmlElement _SerialXmlNode;
		private XmlDocument _AlbumXmlDoc = new XmlDocument();
		private Dictionary<int, BCM.Pavilion> pavilionList = new Dictionary<int, BCM.Pavilion>();
		protected string dianpingCount = "";

		#region Url
		//展会首页
		private string _ExhibitionUrl = "http://chezhan.bitauto.com/beijing";
		private string _MasterBrandUrl = "http://chezhan.bitauto.com/beijing/2010/{0}/";
		private string _PavilionUrl = "http://chezhan.bitauto.com/beijing/zhanguan/";
		private string _AttributeUrl = "http://chezhan.bitauto.com/beijing/xinche/";
		private string _CsLevelUrl = "http://chezhan.bitauto.com/beijing/jibie/";

		#endregion
		/// <summary>
		/// 子品牌结点
		/// </summary>
		protected XmlElement SerialXmlNode
		{
			get { return _SerialXmlNode; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParam();
			if (!ValidatorParam())
			{
				Response.Redirect(_ExhibitionUrl, true);
				return;
			}
		}

		/// <summary>
		/// 得到页面参数
		/// </summary>
		private void GetParam()
		{
			_SerialAllSpell = string.IsNullOrEmpty(Request.QueryString["spell"])
								? "" : Request.QueryString["spell"].ToString();
		}
		/// <summary>
		/// 验证页面参数
		/// </summary>
		/// <returns></returns>
		private bool ValidatorParam()
		{
			if (_ExhibitionID < 1 || string.IsNullOrEmpty(_SerialAllSpell))
			{
				return false;
			}

			_XmlDoc = new BCB.Exhibition().GetExibitionXmlByExhibitionId(_ExhibitionID, 10);

			if (_XmlDoc == null)
			{
				return false;
			}

			_SerialXmlNode = (XmlElement)_XmlDoc.SelectSingleNode("root/MasterBrand/Brand/Serial[@AllSpell='" + _SerialAllSpell + "']");

			if (_SerialXmlNode == null || _SerialXmlNode.ParentNode.ParentNode.Attributes["PavilionId"] == null)
			{
				return false;
			}

			pavilionList = new BCB.Exhibition().GetPavilionListByExhibitionId(_ExhibitionID);

			if (pavilionList == null || pavilionList.Count < 1)
			{
				return false;
			}
			_AlbumXmlDoc = new BCB.ExhibitionAlbum().getBeijing2010AlbumRelationData(_ExhibitionID, 10);
			if (_AlbumXmlDoc == null)
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// 页面导航
		/// </summary>
		/// <returns></returns>
		protected string PageGuilder()
		{
			StringBuilder pageGuilder = new StringBuilder("");
			pageGuilder.Append("<div class=\"tit-cx\">");
			pageGuilder.AppendFormat("<img src=\"{0}\" alt=\"\" /></div>", "http://car.bitauto.com/car/images/tit-cx.jpg");
			pageGuilder.Append("<div class=\"breadcrumbs\">");
			pageGuilder.Append("<p>");
			pageGuilder.Append("<a target=\"_blank\" href=\"http://www.bitauto.com\">易车网</a> <a href=\"http://car.bitauto.com\">车型</a><em>&gt;</em>");
			pageGuilder.Append("<a href=\"http://chezhan.bitauto.com/beijing/\">2010北京车展</a><em>&gt;</em><span>");
			pageGuilder.AppendFormat("<a href=\"{0}/\">{1}馆</a></span><em>&gt;</em><span>"
									, _PavilionUrl + PavilionUrl[pavilionList[ConvertHelper.GetInteger(_SerialXmlNode.ParentNode.ParentNode.Attributes["PavilionId"].Value.ToString())].Name.Trim()]
									, pavilionList[ConvertHelper.GetInteger(_SerialXmlNode.ParentNode.ParentNode.Attributes["PavilionId"].Value.ToString())].Name);
			pageGuilder.AppendFormat("<a href=\"http://chezhan.bitauto.com/beijing/2010/{0}/\">{1}</a></span><em>&gt;</em>"
									, _SerialXmlNode.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
									, _SerialXmlNode.ParentNode.ParentNode.Attributes["Name"].Value.ToString());
			pageGuilder.AppendFormat("<span>{0}</span></p>", _SerialXmlNode.GetAttribute("Name"));
			pageGuilder.Append("</div>");
			return pageGuilder.ToString();
		}
		/// <summary>
		/// 打印互联互通头
		/// </summary>
		protected string PrintRelationTitle()
		{
			return "";
			//if (_SerialXmlNode.GetAttribute("CsLevel") == "概念车")
			//{
			//	return PageTitle();
			//}
			//return PageRelationTitle();
		}
		///// <summary>
		///// 绑定页面标题
		///// </summary>
		//private string PageTitle()
		//{
		//	StringBuilder masterBrandTitle = new StringBuilder();
		//	masterBrandTitle.Append("<div class=\"mbrand\">");
		//	masterBrandTitle.Append("<div id=\"thelogoID\" class=\"l\">");
		//	masterBrandTitle.AppendFormat("<img src=\"{0}\">", "http://img1.bitauto.com/bt/car/default/images/carimage/m_"
		//														+ _SerialXmlNode.ParentNode.ParentNode.Attributes["ID"].Value.ToString() + "_a.png");
		//	masterBrandTitle.Append("</div>");
		//	masterBrandTitle.AppendFormat("<h1>{0}</h1>"
		//			, _SerialXmlNode.ParentNode.ParentNode.Attributes["Name"].Value.ToString() + "-"
		//			+ _SerialXmlNode.GetAttribute("Name"));
		//	masterBrandTitle.Append("</div>");

		//	return masterBrandTitle.ToString();
		//}
		///// <summary>
		///// 互联互通头
		///// </summary>
		///// <returns></returns>
		//protected string PageRelationTitle()
		//{
		//	// Apr.17.2010 modified by chengl
		//	return base.GetCommonNavigation("CsBeiJing2010", int.Parse(_SerialXmlNode.GetAttribute("ID").ToString()));
		//}
		/// <summary>
		/// 焦点图
		/// </summary>
		/// <returns></returns>
		protected string PageFocusChart()
		{
			string cacheKey = "Exhibition_" + _ExhibitionID.ToString() + "Serial_" + _SerialXmlNode.GetAttribute("ID") + "_Focus";
			object objString = BCC.Cache.CacheManager.GetCachedData(cacheKey);
			if (objString != null)
			{
				return Convert.ToString(objString);
			}
			string masterImageUrl = BCC.WebConfig.DefaultCarPic.Replace("150-100", "300-200");//品牌封面图
			string CarTypeImageUrl = "";                                                        //车展图片地址
			string CarTypeImageCount = "0";                                                     //车展图片数量
			string ModuleImageUrl = "";                                                         //车模链接
			string ModuleImageCount = "0";                                                      //车模图片数量

			XmlElement xEleme = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='"
								+ ((XmlElement)_SerialXmlNode).GetAttribute("ID") + "']");
			if (xEleme != null)
			{
				if (!string.IsNullOrEmpty(xEleme.GetAttribute("ImageUrl")))
				{
					masterImageUrl = GetImageUrl(xEleme).Replace("_1.", "_4.");
				}
				CarTypeImageUrl = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
								+ xEleme.GetAttribute("TargetUrl");
				CarTypeImageCount = xEleme.GetAttribute("Count");
			}

			xEleme = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/Model/Master[@Id='"
					+ ((XmlElement)_SerialXmlNode.ParentNode.ParentNode).GetAttribute("ID") + "']");
			if (xEleme != null)
			{
				ModuleImageUrl = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
								+ xEleme.GetAttribute("TargetUrl");
				ModuleImageCount = xEleme.GetAttribute("Count");
			}

			BCB.BrandForum bf = new BCB.Car_BrandBll().GetBrandForm("masterbrand", ConvertHelper.GetInteger(_SerialXmlNode.ParentNode.ParentNode.Attributes["ID"].Value.ToString()));

			//获取大本营地址
			string campUrl = bf.CampForumUrl;

			StringBuilder focusString = new StringBuilder("");
			focusString.Append("<div class=\"col-sub\">");
			focusString.Append("<div class=\"ka_focus\">");
			focusString.AppendFormat("<div class=\"photo\"><a href=\"{1}\" target=\"_blank\"><img style=\"width=300px;height=200px\" src=\"{0}\"></a></div>"
									, masterImageUrl
									, CarTypeImageUrl);
			focusString.Append("<div class=\"text\">");
			focusString.Append("<ul class=\"one\">");


			focusString.AppendFormat(" <li><strong>厂家：</strong><a target=\"_blank\" href=\"http://chezhan.bitauto.com/beijing/2010/{0}/\">{1}</a>"
									, _SerialXmlNode.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
									, _SerialXmlNode.ParentNode.ParentNode.Attributes["Name"].Value.ToString());

			if (!string.IsNullOrEmpty(campUrl))
			{
				focusString.AppendFormat("<a class=\"hui\" target=\"_blank\" href=\"{0}\">进入论坛&gt;&gt;</a></li>", campUrl);
			}
			else
			{
				focusString.Append("</li>");
			}


			focusString.AppendFormat("<li class=\"r\"><strong>展馆：</strong><a target=\"_blank\" href=\"{0}/\">{1}馆</a>"
									, _PavilionUrl + PavilionUrl[pavilionList[ConvertHelper.GetInteger(_SerialXmlNode.ParentNode.ParentNode.Attributes["PavilionId"].Value.ToString())].Name.Trim()]
									, pavilionList[ConvertHelper.GetInteger(_SerialXmlNode.ParentNode.ParentNode.Attributes["PavilionId"].Value.ToString())].Name);
			focusString.AppendFormat("<a class=\"hui\" target=\"_blank\" href=\"{0}/\">进入&gt;&gt;</a></li>"
								   , _PavilionUrl + PavilionUrl[pavilionList[ConvertHelper.GetInteger(_SerialXmlNode.ParentNode.ParentNode.Attributes["PavilionId"].Value.ToString())].Name.Trim()]);
			focusString.Append("</ul>");
			focusString.Append("<ul class=\"two\">");

			focusString.AppendFormat("<li><a class=\"hui\" target=\"_blank\" href=\"{0}\">图片</a><a target=\"_blank\" href=\"{0}\">({1}张)</a></li>", CarTypeImageUrl, CarTypeImageCount);
			focusString.AppendFormat("<li><a class=\"hui\" target=\"_blank\" href=\"{0}\">车模</a><a target=\"_blank\" href=\"{0}\">({1}张)</a></li>", ModuleImageUrl, ModuleImageCount);

			string sPath = WebConfig.DataBlockPath
						   + "Data\\CarShow\\2010Beijing\\MasterBrandVideos\\" + "MasterBrand_Videos_"
						   + _SerialXmlNode.ParentNode.ParentNode.Attributes["ID"].Value.ToString() + ".xml";
			string vedioTotalCount = "0";
			if (File.Exists(sPath))
			{
				XmlDocument videoXmlDoc = new XmlDocument();
				videoXmlDoc.Load(sPath);
				if (videoXmlDoc != null)
				{
					XmlNode xNode = videoXmlDoc.SelectSingleNode("NewDataSet/newsAllCount/allcount");
					if (xNode != null)
					{
						vedioTotalCount = xNode.InnerText.ToString();
					}
				}
			}
			focusString.AppendFormat("<li><a class=\"hui\" target=\"_blank\" href=\"{0}\">视频</a><a target=\"_blank\" href=\"{0}\">({1}个)</a></li>"
									 , "http://v.bitauto.com/car/master/" + _SerialXmlNode.ParentNode.ParentNode.Attributes["ID"].Value.ToString()
									 , vedioTotalCount);

			focusString.Append("</ul>");
			focusString.Append("</div>");
			focusString.Append("</div>");
			focusString.Append("</div>");
			BCC.Cache.CacheManager.InsertCache(cacheKey, focusString.ToString(), 10);
			return focusString.ToString();
		}
		/// <summary>
		/// 热门车型排行
		/// </summary>
		/// <returns></returns>
		protected string HotCarTypeOrder()
		{
			string cacheKey = "Exhibition_" + _ExhibitionID.ToString() + "_HotCarType";
			object orderObj = BCC.Cache.CacheManager.GetCachedData(cacheKey);
			if (orderObj != null)
			{
				return Convert.ToString(orderObj);
			}
			StringBuilder hotCarTypeString = new StringBuilder("");
			hotCarTypeString.Append("<div class=\"col-side\">");
			hotCarTypeString.Append("<div class=\"line_box phd0413_02\">");
			hotCarTypeString.Append("<h3><span>热门车型排行</span></h3>");
			hotCarTypeString.Append("<div class=\"wd_hotrank_bg\">");
			hotCarTypeString.Append("<ol class=\"wd_hotrank\">");

			XmlNodeList xNodeList = _XmlDoc.SelectNodes("root/MasterBrand/Brand/Serial");

			if (xNodeList != null && xNodeList.Count > 0)
			{
				Dictionary<string, string> getExhibitionCarType = new Dictionary<string, string>();
				foreach (XmlElement xElem in xNodeList)
				{
					if (!getExhibitionCarType.ContainsKey(xElem.GetAttribute("ID")))
					{
						getExhibitionCarType.Add(xElem.GetAttribute("ID"), xElem.GetAttribute("ID"));
					}
				}

				List<BCC.Enum.EnumCollection.SerialSortForInterface> eesfiList = new BCB.Car_SerialBll().GetBeiJing2010SerialTop10(10, getExhibitionCarType);
				if (eesfiList != null && eesfiList.Count > 0)
				{
					foreach (BCC.Enum.EnumCollection.SerialSortForInterface entity in eesfiList)
					{
						string Url = "http://chezhan.bitauto.com/beijing/2010/"
									+ _XmlDoc.SelectSingleNode("root/MasterBrand/Brand/Serial[@ID='"
									+ entity.CsID
									+ "']").ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
									+ "/" + entity.CsAllSpell + "/";

						hotCarTypeString.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a><small>{2}</small></li>"
													  , Url
													  , entity.CsShowName
													  , entity.CsPV.ToString());
					}
				}
			}

			hotCarTypeString.Append("</ol> ");
			hotCarTypeString.Append("</div>");
			hotCarTypeString.Append("<div class=\"clear\"></div>");
			hotCarTypeString.Append("</div>	");
			hotCarTypeString.Append("</div>");
			BCC.Cache.CacheManager.InsertCache(cacheKey, hotCarTypeString.ToString(), 30);
			return hotCarTypeString.ToString();
		}
		/// <summary>
		/// 车型图片
		/// </summary>
		/// <returns></returns>
		protected string CarTypeImage()
		{
			//得到更多的地址
			string MoreUrl = "";
			XmlNode xNode = _AlbumXmlDoc == null ? null : _AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='"
							+ _SerialXmlNode.GetAttribute("ID") + "']");

			if (xNode != null && xNode.Attributes["TargetUrl"] != null)
			{
				MoreUrl = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
						+ xNode.Attributes["TargetUrl"].Value.ToString();
			}

			StringBuilder carTypeString = new StringBuilder("");
			carTypeString.Append("<div class=\"line_box phd_01\">");
			carTypeString.AppendFormat("<h3><span><a target=\"_blank\" href=\"{2}\">{0}车展图片</a></span><label class=\"tocx\">| <a target=\"_blank\" href=\"{1}\">车型图片</a></label></h3>"
									   , _SerialXmlNode.GetAttribute("Name")
									   , "http://photo.bitauto.com/serial/"
									   + _SerialXmlNode.GetAttribute("ID")
									   , MoreUrl);
			carTypeString.Append("<div class=\"lh_atlaspiclist\">");
			carTypeString.Append("<ul class=\"lh_atlas\">");

			string sPath = WebConfig.DataBlockPath + "Data\\CarShow\\2010Beijing\\SerialImages.xml";

			string serailName = _SerialXmlNode.GetAttribute("Name");

			if (StringHelper.GetRealLength(serailName) > 18)
			{
				serailName = StringHelper.SubString(serailName, 18, false);
			}

			if (File.Exists(sPath))
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(sPath);

				if (xmlDoc != null)
				{
					XmlNodeList xNodeList = xmlDoc.SelectNodes("Images/SerialImages[@Id='"
											+ _SerialXmlNode.GetAttribute("ID") + "']/Image");

					if (xNodeList != null && xNodeList.Count > 0)
					{
						int index = 1;
						foreach (XmlElement entity in xNodeList)
						{
							if (index > 10)
							{
								break;
							}

							carTypeString.AppendFormat("<li><a target=\"_blank\" href=\"{0}\"><img width=\"150\" height=\"100\" alt=\"{2}\" src=\"{1}\">"
													+ "</a><p><a target=\"_blank\" href=\"{0}\">{2}</a></p></li>"
													, entity.GetAttribute("TargetUrl")
													, entity.GetAttribute("ImageUrl").Replace("{0}", "1")
													, string.IsNullOrEmpty(entity.GetAttribute("ImageName")) ? serailName : entity.GetAttribute("ImageName"));
							index++;
						}
					}
				}
			}


			carTypeString.Append("</ul>");
			carTypeString.Append("</div>");


			carTypeString.AppendFormat("<div class=\"more\"><a target=\"_blank\" href=\"{0}\">更多&gt;&gt;</a></div>"
								 , MoreUrl);
			carTypeString.Append("<div class=\"clear\"></div>");
			carTypeString.Append("</div>");
			return carTypeString.ToString();
		}
		/// <summary>
		/// 同主品牌下其他子品牌
		/// </summary>
		/// <returns></returns>
		protected string OtherCarType()
		{
			if (_SerialXmlNode.ParentNode.ParentNode.SelectNodes("Brand/Serial").Count < 2)
			{
				return "";
			}
			StringBuilder otherString = new StringBuilder("");
			otherString.Append("<div class=\"col-all\">");
			otherString.Append("<div class=\"line_box ka_brand phd0413_01\">");
			otherString.AppendFormat("<h3><span><a target=\"_blank\" href=\"http://chezhan.bitauto.com/beijing/2010/{0}\">{1}车型</a></span></h3>"
									, _SerialXmlNode.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
									, _SerialXmlNode.ParentNode.ParentNode.Attributes["Name"].Value.ToString());
			otherString.Append("<dl>");

			//主品牌主体
			otherString.Append("<dd class=\"have\">");
			otherString.Append("<div class=\"brandcatena\">");

			for (int i = 0; i < _SerialXmlNode.ParentNode.ParentNode.ChildNodes.Count; i++)
			{
				XmlNode xNode = _SerialXmlNode.ParentNode.ParentNode.ChildNodes[i];
				if (xNode.ChildNodes.Count == 1
					&& xNode.ChildNodes[0].Attributes["ID"].Value == _SerialXmlNode.GetAttribute("ID"))
				{
					continue;
				}
				otherString.AppendFormat("<h4>{0}</h4>", ((XmlElement)_SerialXmlNode.ParentNode.ParentNode.ChildNodes[i]).GetAttribute("Name"));
				otherString.Append("<ul>");

				otherString.Append(BindSerialSpan(((XmlElement)_SerialXmlNode.ParentNode.ParentNode.ChildNodes[i]).ChildNodes, _SerialXmlNode, -1));

				otherString.Append("</ul>");
				if (i + 1 < _SerialXmlNode.ParentNode.ParentNode.ChildNodes.Count)
				{
					otherString.Append("<div class=\"line_fg\"></div>");
				}
			}

			otherString.Append("<div class=\"clear\"></div>");
			otherString.Append("</div>");
			otherString.Append("</dd>");

			otherString.Append("</dl>");
			otherString.Append("<div class=\"clear\"></div>");
			otherString.Append("</div>");
			otherString.Append("</div>");
			return otherString.ToString();
		}
		/// <summary>
		/// 打印页面新闻
		/// </summary>
		/// <returns></returns>
		protected string PageNewSpan()
		{
			StringBuilder newString = new StringBuilder("");
			newString.Append("<div class=\"col-main\">");
			newString.Append("<div class=\"line_box\">");
			newString.Append("<div class=\"ka_topnews ka_h278\">");
			int index = 0;
			Dictionary<string, ExhibitionNew> eNewList = GetExhibitionNewsList("serial", ConvertHelper.GetInteger(_SerialXmlNode.GetAttribute("ID")));

			if (eNewList != null && eNewList.Count > 0)
			{
				foreach (KeyValuePair<string, ExhibitionNew> entity in eNewList)
				{
					if (index == 0)
					{
						newString.AppendFormat("<h2><a target=\"_blank\" href=\"{0}\">{1}</a></h2><div class=\"demarcation_li\"></div>"
												   , entity.Value.Url
												   , entity.Value.Title);
						newString.Append("<ul>");
						index++;
						continue;
					}
					newString.AppendFormat("<li><a target=\"_blank\" target=\"_blank\" href=\"{0}\">{1}</a><em>{2}</em></li>"
												, entity.Value.Url
												, entity.Value.Title
												, entity.Value.CreatTime.ToString("M月d日"));
					index++;
				}
			}
			string spath = WebConfig.DataBlockPath + "Data\\CarShow\\2010Beijing\\Serial\\Carshow_Serial_"
						 + _SerialXmlNode.GetAttribute("ID") + ".xml";
			//如果指定列表
			if (index > 0)
			{
				newString.Append(BuildOtherNewsSpan(index, spath, eNewList));
				return newString.ToString();
			}

			newString.Append(BuildNoAcceptNewsSpan(spath));
			return newString.ToString();
		}

		/// <summary>
		/// 产生剩余新闻块
		/// </summary>
		/// <returns></returns>
		private string BuildOtherNewsSpan(int index, string spath, Dictionary<string, ExhibitionNew> eNewList)
		{
			StringBuilder newString = new StringBuilder();

			if (!File.Exists(spath))
			{
				newString.Append("</ul>");
				newString.Append(BuildNewsEnd());
				return newString.ToString();
			}
			//加载文档XML
			XmlDocument newsXmlDoc = new XmlDocument();
			newsXmlDoc.Load(spath);
			//如果指定列表不为空但此品牌没有新闻
			if (newsXmlDoc == null)
			{
				newString.Append("</ul>");
				newString.Append(BuildNewsEnd());
				return newString.ToString();
			}

			XmlNodeList xNodeList = newsXmlDoc.SelectNodes("NewDataSet/listNews");
			//如果指定列表不为空但此品牌没有新闻
			if (xNodeList == null
				|| xNodeList.Count < 1)
			{
				newString.Append("</ul>");
				newString.Append(BuildNewsEnd());
				return newString.ToString();
			}

			int IsPrintNewsCount = 9 - index;

			for (int i = 0; i < IsPrintNewsCount; i++)
			{
				if (i + 1 > xNodeList.Count)
				{
					break;
				}
				if (eNewList.ContainsKey(xNodeList[i].SelectSingleNode("newsid").InnerText.ToString()))
				{
					continue;
				}
				newString.AppendFormat("<li><a target=\"_blank\" target=\"_blank\" href=\"{0}\">{1}</a><em>{2}</em></li>"
									  , xNodeList[i].SelectSingleNode("filepath").InnerText.ToString()
									  , xNodeList[i].SelectSingleNode("title").InnerText.ToString()
									  , Convert.ToDateTime(xNodeList[i].SelectSingleNode("publishtime").InnerText.ToString()).ToString("M月d日"));
			}
			newString.Append("</ul>");
			newString.Append(BuildNewsEnd());
			return newString.ToString();
		}
		/// <summary>
		/// 产生没有指定块的新闻
		/// </summary>
		/// <returns></returns>
		private string BuildNoAcceptNewsSpan(string spath)
		{
			StringBuilder newString = new StringBuilder();

			if (!File.Exists(spath))
			{
				newString.Append(BuildNewsEnd());
				return newString.ToString();
			}
			//加载文档XML
			XmlDocument newsXmlDoc = new XmlDocument();
			newsXmlDoc.Load(spath);
			//如果指定列表不为空但此品牌没有新闻
			if (newsXmlDoc == null)
			{
				newString.Append(BuildNewsEnd());
				return newString.ToString();
			}

			XmlNodeList xNodeList = newsXmlDoc.SelectNodes("NewDataSet/listNews");
			//如果指定列表不为空但此品牌没有新闻
			if (xNodeList == null
				|| xNodeList.Count < 1)
			{
				newString.Append(BuildNewsEnd());
				return newString.ToString();
			}

			int IsPrintNewsCount = 9;

			for (int i = 0; i < IsPrintNewsCount; i++)
			{
				if (i + 1 > xNodeList.Count)
				{
					break;
				}
				if (i == 0)
				{
					newString.AppendFormat("<h2><a target=\"_blank\" href=\"{0}\">{1}</a></h2><div class=\"demarcation_li\"></div>"
												 , xNodeList[i].SelectSingleNode("filepath").InnerText.ToString()
												 , xNodeList[i].SelectSingleNode("title").InnerText.ToString());
					newString.Append("<ul>");
					continue;
				}
				newString.AppendFormat("<li><a target=\"_blank\" target=\"_blank\" href=\"{0}\">{1}</a><em>{2}</em></li>"
									  , xNodeList[i].SelectSingleNode("filepath").InnerText.ToString()
									  , xNodeList[i].SelectSingleNode("title").InnerText.ToString()
									  , Convert.ToDateTime(xNodeList[i].SelectSingleNode("publishtime").InnerText.ToString()).ToString("M月d日"));
			}
			newString.Append("</ul>");
			newString.Append(BuildNewsEnd());
			return newString.ToString();
		}
		/// <summary>
		/// 生成新闻列表结束
		/// </summary>
		/// <returns></returns>
		private string BuildNewsEnd()
		{
			StringBuilder newString = new StringBuilder();
			newString.Append("</div>");
			newString.Append("<div class=\"clear\">");
			newString.Append("</div>");
			newString.Append("</div>");
			newString.Append("</div>");
			return newString.ToString();
		}
		/// <summary>
		/// 车模品牌
		/// </summary>
		/// <returns></returns>
		protected string CarTypeModule()
		{

			string cacheKey = "Exhibition_" + _ExhibitionID.ToString() + "MasterBrand_"
							+ ((XmlElement)_SerialXmlNode.ParentNode.ParentNode).GetAttribute("ID") + "_Model";
			object objString = BCC.Cache.CacheManager.GetCachedData(cacheKey);
			if (objString != null)
			{
				return Convert.ToString(objString);
			}

			XmlElement xNode = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/Model/Master[@Id='"
									+ ((XmlElement)_SerialXmlNode.ParentNode.ParentNode).GetAttribute("ID") + "']");
			StringBuilder ModelString = new StringBuilder();
			ModelString.Append("<div class=\"col-all\">");
			ModelString.Append("<div class=\"line_box\">");
			ModelString.AppendFormat("<h3><span><a target=\"_blank\" href=\"{0}\">{2}车模(共{1}张)</a></span></h3>"
									, _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
									+ xNode.GetAttribute("TargetUrl")
									, xNode.Attributes["Count"].Value.ToString()
									, _SerialXmlNode.ParentNode.ParentNode.Attributes["Name"].Value.ToString());
			ModelString.Append("<div class=\"p_piclist1 p_mo1\">");
			ModelString.Append("<ul>");

			if (xNode.ChildNodes != null || xNode.ChildNodes.Count > 0)
			{
				int index = 1;
				foreach (XmlElement xEleme in xNode.ChildNodes)
				{
					if (index > 7)
					{
						break;
					}
					index++;
					ModelString.AppendFormat("<li><a target=\"_blank\" href=\"{2}\"><img alt=\"{0}\" src=\"{1}\"></a><a href=\"{2}\">{0}</a></li>"
											 , xEleme.GetAttribute("Name")
											 , GetImageUrl(xEleme)
											 , _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
											 + xEleme.GetAttribute("TargetUrl"));
				}
			}

			ModelString.Append("</ul>");
			ModelString.Append("</div>");
			ModelString.Append("<div class=\"clear\"></div>");
			ModelString.AppendFormat("<div class=\"more\"><a target=\"_blank\" href=\"{0}\" target=\"_blank\">更多&gt;&gt;</a></div>"
							, _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
							+ xNode.GetAttribute("TargetUrl"));
			ModelString.Append("</div>");
			ModelString.Append("</div>");
			BCC.Cache.CacheManager.InsertCache(cacheKey, ModelString.ToString(), 10);
			return ModelString.ToString();
		}
		/// <summary>
		/// 相同级别车型
		/// </summary>
		/// <returns></returns>
		protected string SameCsLevelCarType()
		{
			// modified by chengl Apr.21.2010
			if (!CsLevel.ContainsKey(_SerialXmlNode.GetAttribute("CsLevel").Trim()))
			{ return ""; }
			string sMoreUrl = "";

			if (SerialXmlNode.GetAttribute("CsLevel") == "概念车")
			{
				sMoreUrl = _AttributeUrl + AttributeUrl["概念"];
			}
			else
			{
				sMoreUrl = _CsLevelUrl + CsLevel[_SerialXmlNode.GetAttribute("CsLevel").Trim()];
			}
			StringBuilder sameLevelString = new StringBuilder();
			sameLevelString.Append("<div class=\"col-all\">");
			sameLevelString.Append("<div class=\"line_box ka_brand phd0413_01 phd0414_01\">");
			sameLevelString.AppendFormat("<h3><span><a target=\"_blank\" href=\"{0}/\">其他同级别车型</a></span></h3>", sMoreUrl);
			sameLevelString.Append("<dl><dd class=\"have\"><div class=\"brandcatena\">");
			sameLevelString.Append("<ul>");

			XmlNodeList xNodeList = _XmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@CsLevel='" + _SerialXmlNode.GetAttribute("CsLevel") + "']");
			sameLevelString.Append(BindSerialSpan(xNodeList, _SerialXmlNode, 5));

			sameLevelString.Append("</ul>");
			sameLevelString.Append("<div class=\"clear\"></div></div></dd></dl>");
			sameLevelString.Append("<div class=\"clear\"></div>");
			sameLevelString.AppendFormat("<div class=\"more\"><a target=\"_blank\" href=\"{0}/\">更多&gt;&gt;</a></div>", sMoreUrl);
			sameLevelString.Append("</div>");
			sameLevelString.Append("</div>");
			return sameLevelString.ToString();
		}
		/// <summary>
		/// 视频列表
		/// </summary>
		/// <returns></returns>
		protected string VideoList()
		{
			StringBuilder videoString = new StringBuilder("");

			videoString.Append("<div class=\"col-all\">");
			videoString.Append("<div class=\"line_box\">");
			videoString.AppendFormat("<h3><span><a href=\"{1}\" target=\"_blank\">{0}视频</a></span></h3>"
									, _SerialXmlNode.ParentNode.ParentNode.Attributes["Name"].Value.ToString()
									, "http://v.bitauto.com/car/master/"
									+ _SerialXmlNode.ParentNode.ParentNode.Attributes["ID"].Value.ToString());
			videoString.Append("<div class=\"phd_02\">");

			string sPath = WebConfig.DataBlockPath
						   + "Data\\CarShow\\2010Beijing\\MasterBrandVideos\\" + "MasterBrand_Videos_"
						   + _SerialXmlNode.ParentNode.ParentNode.Attributes["ID"].Value.ToString() + ".xml";

			if (File.Exists(sPath))
			{
				XmlDocument videoXmlDoc = new XmlDocument();
				videoXmlDoc.Load(sPath);
				if (videoXmlDoc != null)
				{
					XmlNodeList xNodeList = videoXmlDoc.SelectNodes("NewDataSet/listNews");
					if (xNodeList != null && xNodeList.Count > 1)
					{
						videoString.Append("<ul>");
						foreach (XmlElement xEleme in xNodeList)
						{
							videoString.AppendFormat("<li><a href=\"{0}\" target=\"_blank\">"
													, xEleme.GetElementsByTagName("filepath")[0].InnerText.ToString());
							videoString.AppendFormat("<img border=\"0\" title=\"{0}\" src=\"{1}\"></a>"
													, xEleme.GetElementsByTagName("title")[0].InnerText.ToString()
													, xEleme.GetElementsByTagName("picture")[0].InnerText.ToString());
							videoString.AppendFormat("<p><a href=\"{0}\" target=\"_blank\">{1}</a></p>"
													 , xEleme.GetElementsByTagName("filepath")[0].InnerText.ToString()
													 , xEleme.GetElementsByTagName("title")[0].InnerText.ToString());

							// modified by chengl Jul.22.2010
							XmlNodeList xnl = xEleme.GetElementsByTagName("duration");
							if (xnl.Count > 0)
							{
								videoString.AppendFormat("<p>时长:<span>{0}</span></p>"
													 , xEleme.GetElementsByTagName("duration")[0].InnerText.ToString());
							}
							else
							{
								videoString.AppendFormat("<p>时长:<span>{0}</span></p>"
													 , "");
							}
							//videoString.AppendFormat("<p>时长:<span>{0}</span></p>"
							//                         , xEleme.GetElementsByTagName("duration")[0].InnerText.ToString());
							videoString.AppendFormat("<p>热度:<em>{0}</em></p>", xEleme.GetElementsByTagName("TotalVisit")[0].InnerText.ToString());
							videoString.Append("</li>");
						}
						videoString.Append("</ul>");
					}
				}
			}


			videoString.Append("<div class=\"clear\"></div>");
			videoString.AppendFormat("<div class=\"more\"><a href=\"{0}\" target=\"_blank\">更多&gt;&gt;</a></div>"
									, "http://v.bitauto.com/car/master/" + _SerialXmlNode.ParentNode.ParentNode.Attributes["ID"].Value.ToString());
			videoString.Append("</div>");
			videoString.Append("</div>");
			videoString.Append("</div>");
			return videoString.ToString();
		}
		/// <summary>
		/// 页面点评
		/// </summary>
		/// <returns></returns>
		protected string PageComments()
		{

			string sPath = WebConfig.DataBlockPath
						   + "Data\\CarShow\\2010Beijing\\SerialDianping\\" + "Dianping_Serial_"
						   + _SerialXmlNode.GetAttribute("ID") + ".xml";

			if (!File.Exists(sPath))
			{
				return "";
			}

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(sPath);

			if (xmlDoc == null)
			{
				return "";
			}
			dianpingCount = xmlDoc.SelectSingleNode("SerialDianping").Attributes["count"].Value.ToString();
			StringBuilder commentString = new StringBuilder("");
			commentString.Append("<div class=\"col-sub\">");
			commentString.Append("<div class=\"line_box\" style=\"height: 370px\">");
			commentString.AppendFormat("<h3><span>{0}点评</span><label class=\"sum\">(共<b>{1}</b>条)</label></h3>"
									   , _SerialXmlNode.GetAttribute("Name")
									   , xmlDoc.SelectSingleNode("SerialDianping").Attributes["count"].Value.ToString());

			commentString.Append("<div class=\"p_dptab\"><ul id=\"IDtab2\">");
			commentString.AppendFormat("<li id=\"good\" class=\"current\"><a href=\"\">好评<span>({0}条)</span></a></li>"
									   , xmlDoc.SelectSingleNode("SerialDianping/Dianping[@type='3']").Attributes["count"].Value.ToString());
			commentString.AppendFormat("<li id=\"normal\"><a href=\"\">中评<span>({0}条)</span></a></li>"
									   , xmlDoc.SelectSingleNode("SerialDianping/Dianping[@type='2']").Attributes["count"].Value.ToString());
			commentString.AppendFormat("<li id=\"bad\"><a href=\"\">差评<span>({0}条)</span></a></li>"
									   , xmlDoc.SelectSingleNode("SerialDianping/Dianping[@type='1']").Attributes["count"].Value.ToString());
			commentString.Append("</ul></div>");

			commentString.Append("<div class=\"p_dptabcontent\">");
			commentString.Append("<ul id=\"IDbox2_0\">");
			commentString.Append(BindCommontsList(xmlDoc.SelectSingleNode("SerialDianping/Dianping[@type='3']").ChildNodes));
			commentString.Append("</ul>");
			commentString.Append("<ul id=\"IDbox2_1\" style=\"display:none\">");
			commentString.Append(BindCommontsList(xmlDoc.SelectSingleNode("SerialDianping/Dianping[@type='2']").ChildNodes));
			commentString.Append("</ul>");
			commentString.Append("<ul id=\"IDbox2_2\" style=\"display:none\">");
			commentString.Append(BindCommontsList(xmlDoc.SelectSingleNode("SerialDianping/Dianping[@type='1']").ChildNodes));
			commentString.Append("</ul>");

			commentString.Append("</div>");
			commentString.Append("<div class=\"clear\"></div>");
			commentString.Append("</div>");
			commentString.Append("</div>");
			return commentString.ToString();
		}
		/// <summary>
		/// 绑定子品牌块
		/// </summary>
		/// <param name="xNodeList"></param>
		/// <returns></returns>
		private string BindSerialSpan(XmlNodeList xNodeList, XmlElement xRemoveElement, int Count)
		{
			string CarTypeImage = "";

			if (xNodeList == null || xNodeList.Count < 1)
			{
				return "";
			}
			Dictionary<int, BCM.Attribute> attrList = new Dictionary<int, BCM.Attribute>();
			attrList = new BCB.Exhibition().GetAttributeListByExhibitionId(_ExhibitionID, 10);
			StringBuilder serialSpan = new StringBuilder("");
			int index = 1;
			foreach (XmlElement xElem in xNodeList)
			{
				XmlElement _AlbumNode = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='" + xElem.GetAttribute("ID") + "']");

				if (_AlbumNode == null)
				{
					continue;
				}
				else if (string.IsNullOrEmpty(_AlbumNode.GetAttribute("ImageUrl")))
				{
					CarTypeImage = WebConfig.DefaultCarPic;
				}
				else
				{
					CarTypeImage = GetImageUrl(_AlbumNode);
				}

				if (xRemoveElement != null && xElem.GetAttribute("ID") == xRemoveElement.GetAttribute("ID"))
				{
					continue;
				}
				if (Count != -1 && index > Count)
				{
					continue;
				}
				index++;

				serialSpan.AppendFormat(" <li><a target=\"_blank\" href=\"" + _MasterBrandUrl + "{1}/\"><img src=\"{2}\" alt=\"{3}\"></a>"
										, xElem.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
										, xElem.GetAttribute("AllSpell")
										, CarTypeImage
										, xElem.GetAttribute("Name"));

				if (attrList != null && attrList.Count > 0)
				{
					serialSpan.Append(GetSerialAttributeSpan(xElem, attrList));
				}
				string tempName = xElem.GetAttribute("Name");
				if (StringHelper.GetRealLength(tempName) > 18)
				{
					tempName = StringHelper.SubString(tempName, 18, false);
				}
				serialSpan.AppendFormat("<p class=\"name\"><a target=\"_blank\" href=\"" + _MasterBrandUrl + "{1}/\" title=\"{2}\">{3}</a></p>"
										, xElem.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
										, xElem.GetAttribute("AllSpell")
										, xElem.GetAttribute("Name")
										, tempName);

				if (xElem.GetAttribute("CsLevel") == "概念车")
				{
					serialSpan.AppendFormat("<p class=\"other\">车型|<a target=\"_blank\" href=\"{0}\">图库</a>|口碑</p>"
										   , _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
										   + _AlbumNode.GetAttribute("TargetUrl"));
				}
				else
				{
					serialSpan.AppendFormat("<p class=\"other\"><a target=\"_blank\" href=\"http://car.bitauto.com/{1}/\">车型</a>|"
										   + "<a target=\"_blank\" href=\"http://photo.bitauto.com/serial/{2}\">图库</a>|<a target=\"_blank\" href=\"http://car.bitauto.com/{1}/koubei/\">口碑</a></p>"
										   , xElem.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
										   , xElem.GetAttribute("AllSpell")
										   , xElem.GetAttribute("ID"));
				}
				//生成密钥集
				string content = "BitAuto.Chezhan.2010" + "-" + xElem.GetAttribute("ID") + "-" + xElem.GetAttribute("Name") + "%TGBHU*IK<LP_";
				serialSpan.AppendFormat("<p class=\"vote\"><a href=\"javascript:vote('{0}','{1}','{2}')\">顶</a></p>"
										, xElem.GetAttribute("ID")
										, xElem.GetAttribute("Name")
										, BitAuto.Beyond.Utils.SecurityEncrypt.MD5Encrypt(content, 32));
			}

			return serialSpan.ToString();
		}
		/// <summary>
		/// 得到子品牌属性块
		/// </summary>
		/// <returns></returns>
		private string GetSerialAttributeSpan(XmlElement xElement, Dictionary<int, BCM.Attribute> attrList)
		{
			if (xElement == null || attrList == null || attrList.Count < 1)
			{
				return "<p class=\"btm\"></p>";
			}
			if (xElement.ChildNodes == null || xElement.ChildNodes.Count < 1)
			{
				return "<p class=\"btm\"></p>";
			}
			Dictionary<int, int> IsContainsIndex = new Dictionary<int, int>();
			foreach (XmlElement xElem in xElement.ChildNodes)
			{
				if (!IsContainsIndex.ContainsKey(ConvertHelper.GetInteger(xElem.GetAttribute("ID"))))
				{
					IsContainsIndex.Add(ConvertHelper.GetInteger(xElem.GetAttribute("ID")), 0);
				}

			}
			StringBuilder serialAttributeBuilder = new StringBuilder();
			serialAttributeBuilder.Append("<p class=\"btm\">");
			int index = 0;
			foreach (KeyValuePair<int, BCM.Attribute> entity in attrList)
			{
				if (!IsContainsIndex.ContainsKey(entity.Value.ID))
				{
					continue;
				}

				serialAttributeBuilder.AppendFormat("<a target=\"_blank\" href=\"{0}/\">{1}</a>"
												, _AttributeUrl + AttributeUrl[entity.Value.Name]
												, entity.Value.Name);
			}

			serialAttributeBuilder.Append("</p>");
			return serialAttributeBuilder.ToString();
		}
		/// <summary>
		/// 绑定口碑
		/// </summary>
		/// <param name="xNodeList"></param>
		/// <returns></returns>
		private string BindCommontsList(XmlNodeList xNodeList)
		{
			if (xNodeList == null || xNodeList.Count < 1)
			{
				return "";
			}

			StringBuilder liElement = new StringBuilder();

			foreach (XmlElement xEleme in xNodeList)
			{
				liElement.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a></li>"
									 , xEleme.GetElementsByTagName("url")[0].InnerText.ToString()
									 , xEleme.GetElementsByTagName("title")[0].InnerText.ToString());
			}
			return liElement.ToString();
		}
	}
}