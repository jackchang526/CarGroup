using System;
using System.Xml;
using System.IO;
using System.Text;
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
	public partial class _2010guangzhou_chezhan_MasterBrand : beijing_2010_PageBase
	{
		private string _MasterBrandAllSpell = "";
		private XmlDocument _XmlDoc = new XmlDocument();
		private XmlElement _MasterBrandXmlNode;
		private XmlDocument _AlbumXmlDoc = new XmlDocument();
		private Dictionary<int, BCM.Pavilion> pavilionList = new Dictionary<int, BCM.Pavilion>();

		protected XmlElement MasterBrandXmlNode
		{
			get { return _MasterBrandXmlNode; }
		}

		#region Url
		//展会首页
		private string _ExhibitionUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/";
		private string _MasterBrandUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/2010/{0}/";
		private string _PavilionUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/zhanguan/";
		private string _AttributeUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/xinche/";
		private string _CsLevelUrl = "http://chezhan.bitauto.com/guangzhou-chezhan/jibie/";
		private string _SaveNewsUrl = "2010guangzhou";
		protected int _MasterBrandId = 0;
		protected string _MasterBrandName = string.Empty;

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			_ExhibitionID = 48;
			GetParam();
			BuilderPavilionUrlList();
			if (!ValidatorParam())
			{
				Response.Redirect(_ExhibitionUrl, true);
				return;
			}
			_MasterBrandName = _MasterBrandXmlNode.GetAttribute("Name");
		}
		/// <summary>
		/// 得到页面参数
		/// </summary>
		private void GetParam()
		{
			_MasterBrandAllSpell = string.IsNullOrEmpty(Request.QueryString["spell"])
								? "" : Request.QueryString["spell"].ToString();
		}
		/// <summary>
		/// 验证页面参数
		/// </summary>
		/// <returns></returns>
		private bool ValidatorParam()
		{
			if (_ExhibitionID < 1 || string.IsNullOrEmpty(_MasterBrandAllSpell))
			{
				return false;
			}

			_XmlDoc = new BCB.Exhibition().GetExibitionXmlByExhibitionId(_ExhibitionID, 10);

			if (_XmlDoc == null)
			{
				return false;
			}

			_MasterBrandXmlNode = (XmlElement)_XmlDoc.SelectSingleNode("root/MasterBrand[@AllSpell='" + _MasterBrandAllSpell + "']");

			if (_MasterBrandXmlNode == null || _MasterBrandXmlNode.Attributes["PavilionId"] == null)
			{
				return false;
			}

			pavilionList = new BCB.Exhibition().GetPavilionListByExhibitionId(_ExhibitionID);

			if (pavilionList == null || pavilionList.Count < 1)
			{
				return false;
			}
			_AlbumXmlDoc = new BCB.ExhibitionAlbum().getBeijing2010AlbumRelationData(_ExhibitionID, 10);
			if (_AlbumXmlDoc == null
				|| _AlbumXmlDoc.SelectSingleNode("Data/Model/Master[@Id='" + _MasterBrandXmlNode.GetAttribute("ID") + "']") == null)
			{
				return false;
			}
			return true;
		}

		protected string BuilderPageGuilder()
		{
			StringBuilder guilderString = new StringBuilder();
			int pavilionId = ConvertHelper.GetInteger(_MasterBrandXmlNode.GetAttribute("PavilionId"));

			guilderString.AppendFormat("<em>&gt;</em><span><a href=\"{2}{0}/\">{1}</a></span>"
									, PavilionUrl[pavilionList[pavilionId].Name.Trim()]
									, pavilionList[pavilionId].Name
									, _PavilionUrl);
			guilderString.AppendFormat("<em>&gt;</em><span>{0}</span>", _MasterBrandXmlNode.GetAttribute("Name"));

			return guilderString.ToString();
		}
		/// <summary>
		/// 绑定页面标题
		/// </summary>
		protected string PageTitle()
		{
			StringBuilder masterBrandTitle = new StringBuilder();
			masterBrandTitle.Append("<div class=\"mbrand_box\">");
			masterBrandTitle.Append("<div class=\"mbrand_tab1\"><div class=\"mbrand_tab2\">");
			masterBrandTitle.Append("<div id=\"thelogoID\" class=\"l\">");
			masterBrandTitle.AppendFormat("<b ;=\"\" style=\"background: url(&quot;http://img1.bitauto.com/bt/car/default/images/carimage/m_{0}_a.png&quot;) no-repeat scroll 0pt 0pt transparent;\" class=\"cars\">&nbsp;</b>"
				, _MasterBrandXmlNode.GetAttribute("ID"));
			masterBrandTitle.Append("</div>");
			masterBrandTitle.AppendFormat("<h1>{0}</h1>", _MasterBrandXmlNode.GetAttribute("Name"));
			masterBrandTitle.Append("</div></div>");
			masterBrandTitle.Append("<div class=\"mbrand\">");
			masterBrandTitle.Append("<div class=\"column\">");

			masterBrandTitle.AppendFormat(" <span><a target=\"_blank\" href=\"http://photo.bitauto.com/master/{0}.html\">图片</a></span>| <span><a target=\"_blank\" href=\"http://price.bitauto.com/frame.aspx?keyword={1}&mb_id={0}\">报价</a></span>|"
				, _MasterBrandXmlNode.GetAttribute("ID")
				, Server.UrlEncode(_MasterBrandXmlNode.GetAttribute("Name")));
			masterBrandTitle.AppendFormat("<span><a target=\"_blank\" href=\"http://www.cheyisou.com/jingxiaoshang/{1}/\">经销商</a></span>| <span><a target=\"_blank\" href=\"http://v.bitauto.com/car/master/{0}\">视频</a></span>|"
				, _MasterBrandXmlNode.GetAttribute("ID")
				, _MasterBrandXmlNode.GetAttribute("Name"));
			masterBrandTitle.AppendFormat("<span><a target=\"_blank\" href=\"http://car.bitauto.com/{0}/xinwen/\">文章</a></span>|<span><a target=\"_blank\" href=\"http://ask.bitauto.com/so/{1}/\">答疑</a></span>"
				, _MasterBrandXmlNode.GetAttribute("AllSpell")
				, _MasterBrandXmlNode.GetAttribute("Name"));

			masterBrandTitle.Append("</div></div>");
			masterBrandTitle.Append("</div>");

			return masterBrandTitle.ToString();
		}
		/// <summary>
		/// 绑定页面焦点图
		/// </summary>
		/// <returns></returns>
		protected string PageFocusChart()
		{
			string cacheKey = "Exhibition_" + _ExhibitionID.ToString() + "MasterBrand_" + _MasterBrandXmlNode.GetAttribute("ID") + "_Focus";
			object objString = BCC.Cache.CacheManager.GetCachedData(cacheKey);
			if (objString != null)
			{
				return Convert.ToString(objString);
			}

			string masterImageUrl = BCC.WebConfig.DefaultCarPic.Replace("150-100", "300-200");
			string CarTypeImageUrl = "";
			string CarTypeImageCount = "0";
			string ModuleImageUrl = "";
			string ModuleImageCount = "0";
			//得到该主品牌是否在图片的车型数据中接口中存在
			XmlElement xEleme = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master[@Id='"
								+ _MasterBrandXmlNode.GetAttribute("ID") + "']");

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
			//得到该主品牌是否在图片的模特数据中接口中存在
			xEleme = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/Model/Master[@Id='"
					+ _MasterBrandXmlNode.GetAttribute("ID") + "']");
			if (xEleme != null)
			{
				ModuleImageUrl = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
								+ xEleme.GetAttribute("TargetUrl");
				ModuleImageCount = xEleme.GetAttribute("Count");
			}

			BCB.BrandForum bf = new BCB.Car_BrandBll().GetBrandForm("masterbrand", ConvertHelper.GetInteger(_MasterBrandXmlNode.GetAttribute("ID")));

			//获取大本营地址
			string campUrl = bf.CampForumUrl;

			StringBuilder masterBrandFocus = new StringBuilder("");
			masterBrandFocus.Append("<div class=\"col-sub\">");
			masterBrandFocus.Append("<div class=\"ka_focus\">");
			masterBrandFocus.AppendFormat("<div class=\"photo\"><a href=\"{1}\" target=\"_blank\"><img width=\"300px\" height=\"200px\" src=\"{0}\"></a></div>"
										, masterImageUrl
										, CarTypeImageUrl);
			masterBrandFocus.Append("<div class=\"text\">");
			//厂商块
			masterBrandFocus.Append("<ul class=\"one\">");
			if (!string.IsNullOrEmpty(campUrl))
			{
				masterBrandFocus.AppendFormat("<li><strong>厂家：</strong>{1}"
											 + "<a class=\"hui\" target=\"_blank\" href=\"{0}\">进入论坛&gt;&gt;</a></li>", campUrl, _MasterBrandXmlNode.GetAttribute("Name"));
			}
			else
			{
				masterBrandFocus.AppendFormat("<li><strong>厂家：</strong>{0}</li>", _MasterBrandXmlNode.GetAttribute("Name"));
			}
			masterBrandFocus.AppendFormat("<li class=\"r\"><strong>展馆：</strong>"
										+ "<a target=\"_blank\" href=\"{0}/\">{1}</a>"
										+ "<a class=\"hui\" target=\"_blank\" href=\"{0}/\">进入&gt;&gt;</a></li>"
										, _PavilionUrl + PavilionUrl[pavilionList[ConvertHelper.GetInteger(_MasterBrandXmlNode.GetAttribute("PavilionId"))].Name.Trim()]
										, pavilionList[ConvertHelper.GetInteger(_MasterBrandXmlNode.GetAttribute("PavilionId"))].Name);
			masterBrandFocus.Append("</ul>");
			//图片数字块
			masterBrandFocus.Append("<ul class=\"two\">");
			masterBrandFocus.AppendFormat("<li><a class=\"hui\" target=\"_blank\" href=\"{0}\">图片</a><a target=\"_blank\" href=\"{0}\">({1}张)</a></li>"
										 , CarTypeImageUrl
										 , CarTypeImageCount);
			masterBrandFocus.AppendFormat("<li><a class=\"hui\" target=\"_blank\" href=\"{0}\">车模</a><a target=\"_blank\" href=\"{0}\">({1}张)</a></li>"
										 , ModuleImageUrl
										 , ModuleImageCount);
			//得到视频的文件
			string sPath = WebConfig.DataBlockPath
						   + "Data\\CarShow\\" + _SaveNewsUrl + "\\MasterBrandVideos\\" + "MasterBrand_Videos_"
						   + _MasterBrandXmlNode.GetAttribute("ID") + ".xml";
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
			masterBrandFocus.AppendFormat("<li><a class=\"hui\" target=\"_blank\" href=\"{0}\">视频</a><a target=\"_blank\" href=\"{0}\">({1}个)</a></li>"
										 , "http://v.bitauto.com/car/master/" + _MasterBrandXmlNode.GetAttribute("ID")
										 , vedioTotalCount);
			masterBrandFocus.Append("</ul>");

			masterBrandFocus.Append("</div>");
			masterBrandFocus.Append("</div>");
			masterBrandFocus.Append("</div>");


			BCC.Cache.CacheManager.InsertCache(cacheKey, masterBrandFocus, 10);
			return masterBrandFocus.ToString();
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
			Dictionary<string, ExhibitionNew> eNewList = GetExhibitionNewsList("master"
														, ConvertHelper.GetInteger(_MasterBrandXmlNode.GetAttribute("ID"))
														, _SaveNewsUrl);

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
			string spath = WebConfig.DataBlockPath + "Data\\CarShow\\" + _SaveNewsUrl + "\\Masterbrand\\Carshow_Masterbrand_"
					 + _MasterBrandXmlNode.GetAttribute("ID") + ".xml";
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
						string masterBrandAllSpell = _XmlDoc.SelectSingleNode("root/MasterBrand/Brand/Serial[@ID='"
														+ entity.CsID
														+ "']").ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString();
						//得到子品牌地址
						string Url = string.Format(_UrlFormat[_ExhibitionID], masterBrandAllSpell, entity.CsAllSpell);

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
		/// 包含子品牌
		/// </summary>
		/// <returns></returns>
		protected string ContainsSerialList()
		{
			StringBuilder serialString = new StringBuilder("");
			serialString.Append("<div class=\"col-all\">");
			serialString.Append("<div class=\"line_box wd_linebox_4 zh_brand\">");
			serialString.AppendFormat("<h3><span>{0}车型</span></h3>", _MasterBrandXmlNode.GetAttribute("Name"));
			serialString.Append("<dl>");
			serialString.Append("<dd class=\"b\">");
			//主品牌Logo        
			int masterId = ConvertHelper.GetInteger(_MasterBrandXmlNode.GetAttribute("ID"));
			string masterName = _MasterBrandXmlNode.GetAttribute("Name");
			string masterUrl = string.Format("http://car.bitauto.com/tree_chexing/mb_{0}/", masterId);
			string vUrl = string.Format("http://v.bitauto.com/car/master/{0}.html", masterId);
			/*
			 * 参数0:主品牌ID;
			 * 参数1;主品牌名称;
			 * 参数2:主品牌车型频道链接
			 * 参数3;主品牌视频页链接
			 */
			serialString.AppendFormat("<div class=\"brand m_{0}_b\"></div><div class=\"names\">{1}<p><a href=\"{2}\" target=\"_blank\">车型</a>|<a href=\"{3}\" target=\"_blank\">视频</a></p></div>"
										, masterId
										, masterName
										, masterUrl
										, vUrl);
			serialString.Append("</dd>");
			//主品牌主体
			serialString.Append("<dd class=\"have\">");
			serialString.Append("<div class=\"brandcatena\">");

			List<XmlElement> XmlElemList = new List<XmlElement>();

			for (int i = 0; i < _MasterBrandXmlNode.ChildNodes.Count; i++)
			{
				XmlElemList.Add((XmlElement)_MasterBrandXmlNode.ChildNodes[i]);
			}
			//如果元素大于1排序
			if (XmlElemList.Count > 1)
			{
				XmlElemList.Sort(OrderXmlElement);
			}
			for (int i = 0; i < XmlElemList.Count; i++)
			{
				serialString.AppendFormat("<h4>{0}</h4>", XmlElemList[i].GetAttribute("Name"));
				serialString.Append("<ul>");

				serialString.Append(BindSerialSpan(XmlElemList[i].ChildNodes));

				serialString.Append("</ul>");
				if (i + 1 < XmlElemList.Count)
				{
					serialString.Append("<div class=\"line_fg\"></div>");
				}
			}

			serialString.Append("<div class=\"clear\"></div>");
			serialString.Append("</div>");
			serialString.Append("</dd>");

			serialString.Append("</dl>");
			serialString.Append("</div>");
			serialString.Append("</div>");
			return serialString.ToString();
		}
		/// <summary>
		/// 车模品牌
		/// </summary>
		/// <returns></returns>
		protected string CarTypeModule()
		{
			string cacheKey = "Exhibition_" + _ExhibitionID.ToString() + "MasterBrand_" + _MasterBrandXmlNode.GetAttribute("ID") + "_Model";
			object objString = BCC.Cache.CacheManager.GetCachedData(cacheKey);
			if (objString != null)
			{
				return Convert.ToString(objString);
			}

			XmlElement xNode = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/Model/Master[@Id='"
									+ _MasterBrandXmlNode.GetAttribute("ID") + "']");

			if (xNode == null || xNode.ChildNodes.Count < 1) return "";

			StringBuilder ModelString = new StringBuilder();
			ModelString.Append("<div class=\"col-all\">");
			ModelString.Append("<div class=\"line_box wd_linebox_4\">");
			ModelString.AppendFormat("<h3><span><a target=\"_blank\" href=\"{0}\">{2}车模(共{1}张)</a></span></h3>"
									, _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
									+ xNode.GetAttribute("TargetUrl")
									, xNode.GetAttribute("Count")
									, _MasterBrandXmlNode.GetAttribute("Name"));
			ModelString.Append("<div class=\"p_mo1\">");
			ModelString.Append("<ul class=\"md_box\">");

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
					ModelString.AppendFormat("<li class=\"mgL20\"><div><img alt=\"{0}\" src=\"{1}\"><a class=\"bigimg\" target=\"_blank\" href=\"{2}\"></a><p><a target=\"_blank\" href=\"{2}\" >{0}</a></p></div></li>"
											 , xEleme.GetAttribute("Name")
											 , GetImageUrl(xEleme).Replace("_1.", "_8.")
											 , _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
											 + xEleme.GetAttribute("TargetUrl"));
				}
			}

			ModelString.Append("</ul>");
			ModelString.Append("</div>");
			ModelString.Append("<div class=\"clear\"></div>");
			ModelString.AppendFormat("<div class=\"more\"><a href=\"{0}\" target=\"_blank\">更多&gt;&gt;</a></div>"
							, _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString()
							+ xNode.GetAttribute("TargetUrl"));
			ModelString.Append("</div>");
			ModelString.Append("</div>");
			BCC.Cache.CacheManager.InsertCache(cacheKey, ModelString.ToString(), 10);
			return ModelString.ToString();
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
									, _MasterBrandXmlNode.GetAttribute("Name")
									, "http://v.bitauto.com/car/master/" + _MasterBrandXmlNode.GetAttribute("ID"));
			videoString.Append("<div class=\"phd_02\">");

			string sPath = WebConfig.DataBlockPath
						   + "Data\\CarShow\\" + _SaveNewsUrl + "\\MasterBrandVideos\\" + "MasterBrand_Videos_"
						   + _MasterBrandXmlNode.GetAttribute("ID") + ".xml";

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
									, "http://v.bitauto.com/car/master/" + _MasterBrandXmlNode.GetAttribute("ID"));
			videoString.Append("</div>");
			videoString.Append("</div>");
			videoString.Append("</div>");
			return videoString.ToString();
		}
		/// <summary>
		/// 展馆其他主品牌
		/// </summary>
		/// <returns></returns>
		protected string OtherMasterBrand()
		{
			int pavilionId = ConvertHelper.GetInteger(_MasterBrandXmlNode.GetAttribute("PavilionId"));
			Dictionary<int, BCM.Pavilion> pavilionList = new Dictionary<int, BCM.Pavilion>();
			pavilionList = new BCB.Exhibition().GetPavilionListByExhibitionId(_ExhibitionID, 10);

			string pavilionName = pavilionList[pavilionId].Name;
			StringBuilder otherMasterBrand = new StringBuilder("");
			otherMasterBrand.Append("<div class=\"col-all\">");
			otherMasterBrand.Append("<div class=\"line_box\">");
			otherMasterBrand.AppendFormat("<h3><span><a target=\"_blank\" href=\"{1}/\">{0}其他厂家</a></span></h3>"
										, pavilionName
										, _PavilionUrl + PavilionUrl[pavilionName]);

			XmlNodeList xNodeList = _XmlDoc.SelectNodes("root/MasterBrand[@PavilionId='" + pavilionId.ToString() + "']");

			if (xNodeList != null && xNodeList.Count > 0)
			{
				otherMasterBrand.Append("<div class=\"ka_dealer_logo\">");
				otherMasterBrand.Append("<ul>");
				foreach (XmlElement xElem in xNodeList)
				{
					if (xElem.GetAttribute("ID") == _MasterBrandXmlNode.GetAttribute("ID"))
					{
						continue;
					}
					otherMasterBrand.AppendFormat("<li><a target=\"_blank\" href=\"{1}\"><img src=\"{2}\"></a><p><a target=\"_blank\" href=\"{1}\">{0}</a></p></li>"
												 , xElem.GetAttribute("Name")
												 , string.Format(_MasterBrandUrl, xElem.GetAttribute("AllSpell"))
												 , "http://img1.bitauto.com/bt/car/default/images/carimage/m_" + xElem.GetAttribute("ID") + "_b.jpg");
				}
				otherMasterBrand.Append("</ul>");
				otherMasterBrand.Append("<div class=\"clear\"></div><div class=\"more\"></div>");
				otherMasterBrand.Append("</div>");
			}
			otherMasterBrand.Append("</div>");
			otherMasterBrand.Append("</div>");
			return otherMasterBrand.ToString();
		}
		/// <summary>
		/// 绑定子品牌块
		/// </summary>
		/// <param name="xNodeList"></param>
		/// <returns></returns>
		private string BindSerialSpan(XmlNodeList xNodeList)
		{
			string CarTypeImage = "";

			if (xNodeList == null || xNodeList.Count < 1)
			{
				return "";
			}
			Dictionary<int, BCM.Attribute> attrList = new Dictionary<int, BCM.Attribute>();
			attrList = new BCB.Exhibition().GetAttributeListByExhibitionId(_ExhibitionID, 10);
			StringBuilder serialSpan = new StringBuilder("");

			foreach (XmlElement xElem in xNodeList)
			{
				int serialId = ConvertHelper.GetInteger(xElem.GetAttribute("ID"));
				XmlElement _AlbumNode = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='" + xElem.GetAttribute("ID") + "']");
				//如果这个车型没有图片，或者他是概念车没有图片
				if (_AlbumNode == null ||
					(xElem.GetAttribute("CsLevel") == "概念车"
					&& ConvertHelper.GetInteger(_AlbumNode.GetAttribute("Count")) == 0))
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

				serialSpan.AppendFormat(" <li><a target=\"_blank\" href=\"" + _MasterBrandUrl + "{1}/\"><img src=\"{2}\" alt=\"{3}\"></a>"
										, xElem.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
										, xElem.GetAttribute("AllSpell")
										, CarTypeImage
										, xElem.GetAttribute("Name"));

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
										   + "<a target=\"_blank\" href=\"http://photo.bitauto.com/serial/{2}\">图库</a>|<a target=\"_blank\"  class=\"last\" href=\"http://car.bitauto.com/{1}/koubei/\">口碑</a></p>"
										   , xElem.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString()
										   , xElem.GetAttribute("AllSpell")
										   , xElem.GetAttribute("ID"));
				}
				//得到子品牌报价
				string priceString = GetSerialOfficePriceById(serialId);
				if (priceString == "0")
				{
					serialSpan.AppendFormat("<p class=\"price\"><label>价格：</label><a href=\"{0}\" target=\"_blank\">{1}</a></p>", "http://price.bitauto.com/", "暂无报价");
				}
				else
				{
					serialSpan.AppendFormat("<p class=\"price\"><label>价格：</label><a href=\"{0}\" target=\"_blank\">{1}</a></p>"
							, string.Format("http://price.bitauto.com/frame.aspx?newbrandid={0}", serialId)
							, priceString);
				}
				//生成密钥:+ "-" + xElem.GetAttribute("Name")
				string content = _VoteFormat[_ExhibitionID] + "-" + xElem.GetAttribute("ID") + "-" + "%TGBHU*IK<LP_";
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
		/// 从小到大排
		/// </summary>
		/// <param name="pre"></param>
		/// <param name="last"></param>
		/// <returns></returns>
		private int OrderXmlElement(XmlElement pre, XmlElement last)
		{
			int ret = 0;
			string pv1 = string.IsNullOrEmpty(((XmlElement)pre.ChildNodes[0]).GetAttribute("Country"))
							? "" : ((XmlElement)pre.ChildNodes[0]).GetAttribute("Country").Substring(0, 1);
			string pv2 = string.IsNullOrEmpty(((XmlElement)last.ChildNodes[0]).GetAttribute("Country"))
							? "" : ((XmlElement)last.ChildNodes[0]).GetAttribute("Country").Substring(0, 1);
			if (pv1.CompareTo(pv2) > 0)
				ret = 1;
			else if (pv1.CompareTo(pv2) < 0)
				ret = -1;

			return ret;

		}
	}
}