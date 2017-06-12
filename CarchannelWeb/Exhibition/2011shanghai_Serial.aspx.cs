using System;
using System.Xml;
using System.Text;
using System.IO;
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
	public partial class _2011shanghai_Serial : beijing_2010_PageBase
	{
		private string _SerialAllSpell = "";
		private int _SerialId = 0;
		private XmlDocument _XmlDoc = new XmlDocument();
		private XmlElement _SerialXmlNode;
		private XmlDocument _AlbumXmlDoc = new XmlDocument();
		private Dictionary<int, BCM.Pavilion> pavilionList = new Dictionary<int, BCM.Pavilion>();
		private Dictionary<string, ExhibitionPageContent> contentList = new Dictionary<string, ExhibitionPageContent>();
		protected string dianpingCount = "";
		protected string _SerialName = string.Empty;
		protected string _SerialSeoName = string.Empty;
		protected string _BrandName = string.Empty;
		protected string _TagetUrl = string.Empty;
		protected string _TargetBase = string.Empty;

		#region Url
		//展会首页
		private string _ExhibitionUrl = "http://chezhan.bitauto.com/shanghai/";
		private string _MasterBrandUrl = "http://chezhan.bitauto.com/shanghai/2011/{0}/";
		private string _PavilionUrl = "http://chezhan.bitauto.com/shanghai/zhanguan/{0}/{1}/";
		private string _AttributeUrl = "http://chezhan.bitauto.com/shanghai/xinche/";
		private string _CsLevelUrl = "http://chezhan.bitauto.com/shanghai/jibie/";
		private string _SaveNewsUrl = "2011shanghai";

		#endregion

		/// <summary>
		/// 子品牌结点
		/// </summary>
		protected XmlElement SerialXmlNode
		{
			get { return _SerialXmlNode; }
		}
		/// <summary>
		/// 导航条
		/// </summary>
		protected string _GuilderString = string.Empty;
		/// <summary>
		/// 外链其他导航
		/// </summary>
		protected string _OuterOtherGuilderString = string.Empty;
		/// <summary>
		/// 焦点区域内容
		/// </summary>
		protected string _FocusContent = string.Empty;
		/// <summary>
		/// 热门车模列表
		/// </summary>
		protected string _HotModelList = string.Empty;
		/// <summary>
		/// 热门车型列表
		/// </summary>
		protected string _HotCarTypeList = string.Empty;
		/// <summary>
		/// 车型图片列表
		/// </summary>
		protected string _CarImageList = string.Empty;
		/// <summary>
		/// 车模图片列表
		/// </summary>
		protected string _CarModelList = string.Empty;
		/// <summary>
		/// 视频列表
		/// </summary>
		protected string _VideoList = string.Empty;
		/// <summary>
		/// 图解列表
		/// </summary>
		protected string _TuJieList = string.Empty;
		/// <summary>
		/// 品牌列表
		/// </summary>
		protected string _BrandNewsList = string.Empty;
		/// <summary>
		/// 页面加载事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			_ExhibitionID = 59;
			GetParam();
			BuilderPavilionUrlList();
			if (!ValidatorParam())
			{
				Response.Redirect(_ExhibitionUrl, true);
				return;
			}
			//得到子品牌ID
			_SerialId = ConvertHelper.GetInteger(_SerialXmlNode.GetAttribute("ID"));
			//得到子品牌SEO名
			_SerialSeoName = _SerialXmlNode.GetAttribute("SerialSEOName");
			//子品牌名
			_SerialName = _SerialXmlNode.GetAttribute("sName");
			//品牌名
			_BrandName = _SerialXmlNode.ParentNode.Attributes["Name"].Value;
			//得到页面内容
			GetPageContent();
			//初始化导航
			InitGuilder();
			//得到子品牌描述
			OuterGuilder();
			//焦点区内容
			GetFocusContent();
			//得到热门车型列表
			GetHotCarType();
			//得到热门车模列表
			GetHotModelType();
			//得到图片列表
			GetCarImageList();
			//得到车模列表
			GetCarModelList();
			//得到视频列表
			GetVideoList();
			//得到图解列表
			GetTuJieList();
			//得到新闻列表
			GetLeftNewsList();
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

			if (_SerialXmlNode == null
				|| _SerialXmlNode.ParentNode.Attributes["PavilionId"] == null
				|| ConvertHelper.GetInteger(_SerialXmlNode.GetAttribute("NC")) != 1)
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

			XmlElement imgNode = (XmlElement)_AlbumXmlDoc.SelectSingleNode("Data/NewCar/Master/Serial[@Id='" + _SerialXmlNode.GetAttribute("ID") + "']");
			if (_SerialXmlNode.GetAttribute("CsLevel") == "概念车"
				&& (imgNode == null || ConvertHelper.GetInteger(imgNode.GetAttribute("Count")) == 0))
			{
				return false;
			}

			return true;
		}
		/// <summary>
		/// 初始化导航
		/// </summary>
		private void InitGuilder()
		{
			string serialName = _SerialXmlNode.GetAttribute("Name");
			XmlElement brandNode = (XmlElement)_SerialXmlNode.ParentNode;
			string brandName = brandNode.GetAttribute("Name");
			string brandId = brandNode.GetAttribute("ID");
			int pavilionId = ConvertHelper.GetInteger(brandNode.GetAttribute("PavilionId"));
			string pavilionUrl = string.Format(_PavilionUrl, PavilionUrl[pavilionList[pavilionId].Name.Trim()], brandId);

			StringBuilder guilderString = new StringBuilder();
			guilderString.AppendFormat("<dd><a href='{0}'>2011上海车展</a></dd>", _ExhibitionUrl);
			guilderString.AppendFormat("<dd><a href='{0}'>{2}-{1}</a></dd>", pavilionUrl, brandName, pavilionList[pavilionId].Name);
			guilderString.AppendFormat("<dd class='current'>{0}</a></dd>", serialName);
			_GuilderString = guilderString.ToString();
		}
		/// <summary>
		/// 初始化其他关联导航
		/// </summary>
		private void OuterGuilder()
		{
			string masterLogo = string.Format("http://img1.bitauto.com/bt/car/default/images/carimage/m_{0}_a.png",
											  _SerialXmlNode.ParentNode.ParentNode.Attributes["ID"].Value);
			string masterName = _SerialXmlNode.ParentNode.ParentNode.Attributes["Name"].Value;
			string brandName = _SerialXmlNode.ParentNode.Attributes["Name"].Value;
			string serialName = _SerialXmlNode.GetAttribute("sName");
			StringBuilder describeString = new StringBuilder();
			describeString.Append("<div class='car_infor_zh'>");
			describeString.AppendFormat("<img src='{0}' alt='{1}'></a>", masterLogo, masterName);
			describeString.AppendFormat("<h1>{0} {1}</h1>", brandName, serialName);
			string baaUrl = new BCB.Car_SerialBll().GetForumUrlBySerialId(_SerialId);
			if (_SerialXmlNode.GetAttribute("CsLevel") != "概念车")
			{
				serialName = _SerialXmlNode.GetAttribute("Name");
				string serialAllSpell = _SerialXmlNode.GetAttribute("AllSpell");
				describeString.Append("<div class='car_more'>");
				describeString.AppendFormat("<a href='http://car.bitauto.com/{0}/' target='_blank'>车型</a><span>|", serialAllSpell);
				describeString.AppendFormat("</span><a href='http://price.bitauto.com/frame.aspx?newbrandid={0}' target='_blank'>报价</a><span>|", _SerialId);
				describeString.AppendFormat("</span><a href='http://car.bitauto.com/{0}/koubei/' target='_blank'>口碑</a><span>|", serialAllSpell);
				describeString.AppendFormat("</span><a href='{0}' target='_blank'>论坛</a>", baaUrl);
				describeString.Append("</div>");
			}
			describeString.AppendFormat("</div>");
			_OuterOtherGuilderString = describeString.ToString();
		}
		/// <summary>
		/// 得到焦点区域内容
		/// </summary>
		private void GetFocusContent()
		{
			_TargetBase = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value;
			XmlNode _AlbumNode = _AlbumXmlDoc.SelectSingleNode(string.Format("Data/NewCar/Master/Serial[@Id={0}]", _SerialId));
			if (_AlbumNode == null) return;
			string targetUrl = ((XmlElement)_AlbumNode).GetAttribute("TargetUrl");
			//得到图片的域名
			string imgUrl = GetImageUrl(((XmlElement)_AlbumNode));
			StringBuilder focusContent = new StringBuilder();
			focusContent.Append("<div class='car_indexbox'>");
			focusContent.Append("<div class='car_box'>");
			focusContent.Append("<div class='photos'>");
			focusContent.AppendFormat("<a target='_blank' href='{0}{1}'><img src='{2}'></a></div>", _TargetBase, targetUrl, imgUrl.Replace("_1.", "_4."));
			focusContent.Append("<div class='car_text'>");
			//新闻部分内容
			if (contentList.ContainsKey("news") && contentList["news"]._Count > 0)
			{
				XmlElement newsElem = contentList["news"]._NodeList[0];
				string newsUrl = newsElem.SelectSingleNode("filepath").InnerText;
				string title = newsElem.SelectSingleNode("title").InnerText;
				XmlNode summaryNode = newsElem.SelectSingleNode("summary");
				XmlNode contentNode = newsElem.SelectSingleNode("content");
				string detailString = summaryNode == null ? "" : summaryNode.InnerText;
				if (string.IsNullOrEmpty(detailString.ToString()))
				{
					detailString = contentNode == null ? "" : contentNode.InnerText;
				}

				focusContent.AppendFormat("<h2><a href='{0}' target='_blank' title='{1}'>{1}</a></h2>", newsUrl, title);
				if (!string.IsNullOrEmpty(detailString))
					focusContent.AppendFormat("<p>{0}<a href='{1}' target='_blank'>详细&gt;&gt;</a></p>", StringHelper.SubString(detailString, 78, true), newsUrl);
				focusContent.Append("<center></center>");
			}
			//添加视频内容
			if (contentList.ContainsKey("video") && contentList["video"]._Count > 0)
			{
				focusContent.Append("<ul class='video_li'>");
				for (int i = 0; i < contentList["video"]._NodeList.Count; i++)
				{
					if (i > 1)
					{
						break;
					}
					XmlNode xNode = contentList["video"]._NodeList[i];
					string newsUrl = xNode.SelectSingleNode("filepath").InnerText;
					string title = xNode.SelectSingleNode("title").InnerText;
					focusContent.AppendFormat("<li><a href='{0}' target='_blank' title='{1}'>{1}</a></li>", newsUrl, title);
				}
				focusContent.Append("</ul>");
			}
			//添加数字统计
			focusContent.Append("<div class='tab'>");
			if (contentList.ContainsKey("img") && contentList["img"]._Count > 0)
			{
				focusContent.AppendFormat("<span><a href='{0}' target='_blank'><strong>图片</strong><em>{1}张</em></a></span>", contentList["img"]._Url, contentList["img"]._Count);
			}
			if (contentList.ContainsKey("model") && contentList["model"]._Count > 0)
			{
				focusContent.AppendFormat("<span><a href='{0}' target='_blank'><strong>车模</strong><em>{1}张</em></a></span>", contentList["model"]._Url, contentList["model"]._Count);
			}
			if (contentList.ContainsKey("video") && contentList["video"]._Count > 0)
			{
				focusContent.AppendFormat("<span><a href='{0}' target='_blank'><strong>视频</strong><em>{1}条</em></a></span>", contentList["video"]._Url, contentList["video"]._Count);
			}
			if (contentList.ContainsKey("tujie") && contentList["tujie"]._Count > 0)
			{
				focusContent.AppendFormat("<span><a href='{0}' target='_blank'><strong>图解</strong><em>{1}张</em></a></span>", contentList["tujie"]._Url, contentList["tujie"]._Count);
			}
			focusContent.Append("</div>");
			//添加底
			focusContent.Append("</div>");
			focusContent.Append("</div>");
			focusContent.Append("<div class='car_box_bg'></div>");
			focusContent.Append("</div>");
			_FocusContent = focusContent.ToString();
		}

		#region GetPageContent
		/// <summary>
		/// 得到页面内容
		/// </summary>
		private void GetPageContent()
		{
			string cacheKey = string.Format("Exhibition_{0}_ShangHai_{1}_PageContent", _ExhibitionID, _SerialId);
			object obj = BCC.Cache.CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				contentList = (Dictionary<string, ExhibitionPageContent>)obj;
				return;
			}
			InitAlbumCarXmlDocument();
			InitAlbumModelXmlDocument();
			InitNewsXmlDocument();
			InitVideoXmlDocument();
			InitTuJieXmlDocument();
			if (contentList == null || contentList.Count < 1) return;
			BCC.Cache.CacheManager.InsertCache(cacheKey, contentList, 10);
		}
		/// <summary>
		/// 初始化图片的XmlDocument
		/// </summary>
		private void InitAlbumCarXmlDocument()
		{
			_TargetBase = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value;
			XmlNode _AlbumNode = _AlbumXmlDoc.SelectSingleNode(string.Format("Data/NewCar/Master/Serial[@Id={0}]", _SerialId));
			if (_AlbumNode == null) return;
			int imgCount = ConvertHelper.GetInteger(((XmlElement)_AlbumNode).GetAttribute("Count"));
			string targetUrl = ((XmlElement)_AlbumNode).GetAttribute("TargetUrl");
			//图片地址
			string sPath = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\CarShow\\{0}\\SerialImages.xml", _SaveNewsUrl));
			if (!File.Exists(sPath)) return;

			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				xmlDoc.Load(sPath);
			}
			catch
			{
				xmlDoc = null;
			}

			if (xmlDoc == null) return;
			XmlNodeList xNodeList = xmlDoc.SelectNodes(string.Format("Images/SerialImages[@Id={0}]/Image", _SerialId));

			if (xNodeList == null && xNodeList.Count < 1) return;

			List<XmlElement> imgList = new List<XmlElement>();
			foreach (XmlElement entity in xNodeList)
			{
				imgList.Add(entity);
			}


			AddContentList("img", imgCount, imgList, Path.Combine(_TargetBase, targetUrl));
		}
		/// <summary>
		/// 初始化车模的XmlDocument
		/// </summary>
		private void InitAlbumModelXmlDocument()
		{
			int brandId = ConvertHelper.GetInteger(_SerialXmlNode.ParentNode.Attributes["ID"].Value);
			//得到车模的列表
			XmlNodeList _AlbumNodeList = _AlbumXmlDoc.SelectNodes(string.Format("Data/Model/Master/Album[@BrandId={0}]", brandId));
			if (_AlbumNodeList == null || _AlbumNodeList.Count < 1) return;
			int modelCount = 0;
			int modelClassId = 0;
			List<XmlElement> modelList = new List<XmlElement>();
			foreach (XmlElement entity in _AlbumNodeList)
			{
				modelClassId = ConvertHelper.GetInteger(entity.GetAttribute("ParentId"));
				modelCount += ConvertHelper.GetInteger(entity.GetAttribute("Count"));
				modelList.Add(entity);
			}
			string modelUrl = string.Format("{0}class/{1}/", _TargetBase, modelClassId);
			AddContentList("model", modelCount, modelList, modelUrl);
		}
		/// <summary>
		/// 初始化新闻的XmlDocument
		/// </summary>
		private void InitNewsXmlDocument()
		{
			string sPath = Path.Combine(WebConfig.DataBlockPath
									  , string.Format("Data\\CarShow\\{0}\\Serial\\Carshow_Serial_{1}.xml"
									  , _SaveNewsUrl, _SerialId));

			int newsCount = 0;
			List<XmlElement> newsList = GetNewsList(sPath, out newsCount);

			AddContentList("news", newsCount, newsList);
		}
		/// <summary>
		/// 初始化视频的XmlDocument
		/// </summary>
		private void InitVideoXmlDocument()
		{
			string sPath = Path.Combine(WebConfig.DataBlockPath
									 , string.Format("Data\\CarShow\\{0}\\SerialVideo\\{1}.xml"
									 , _SaveNewsUrl, _SerialId));

			int newsCount = 0;
			string videoUrl = string.Format("http://v.bitauto.com/car/serial/{0}/", _SerialId);
			List<XmlElement> newsList = GetNewsList(sPath, out newsCount);

			AddContentList("video", newsCount, newsList, videoUrl);
		}
		/// <summary>
		/// 初始化图解接口
		/// </summary>
		private void InitTuJieXmlDocument()
		{
			string sPath = string.Format("http://imgsvr.bitauto.com/Photo/ImageService.aspx?"
										+ "dataname=imageingroup&showimages=true&showfullurl=true&"
										+ "showaccount=true&serialid={0}&rownum=4&groupid=12&orderby=&imagename=chezhan-shanghai2011", _SerialId);
			//图片地址
			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				xmlDoc.Load(sPath);
			}
			catch
			{
				xmlDoc = null;
			}
			if (xmlDoc == null) return;

			XmlNode xNode = xmlDoc.SelectSingleNode("ImageData/GroupAccount/Group");
			if (xNode == null) return;
			XmlNodeList imgNodeList = xmlDoc.SelectNodes("ImageData/ImageList/ImageInfo");
			if (imgNodeList == null || imgNodeList.Count < 1) return;
			int tujieCount = 0;
			string tujieUrl = string.Empty;
			List<XmlElement> tujieList = new List<XmlElement>();

			tujieCount = ConvertHelper.GetInteger(xNode.Attributes["ImageCount"].Value);
			tujieUrl = xNode.Attributes["Link"].Value;

			foreach (XmlElement entity in imgNodeList)
			{
				tujieList.Add(entity);
			}

			AddContentList("tujie", tujieCount, tujieList, tujieUrl);
		}
		/// <summary>
		/// 得到新闻列表
		/// </summary>
		/// <param name="path">文件路径</param>
		/// <param name="count">新闻数量</param>
		/// <returns></returns>
		private List<XmlElement> GetNewsList(string path, out int count)
		{
			count = 0;
			if (!File.Exists(path)) return null;
			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				xmlDoc.Load(path);
			}
			catch
			{
				xmlDoc = null;
			}

			if (xmlDoc == null) return null;
			XmlNode xNode = xmlDoc.SelectSingleNode("NewDataSet/newsAllCount/allcount");
			if (xNode == null) return null;
			count = ConvertHelper.GetInteger(xNode.InnerText);
			if (count == 0) return null;

			XmlNodeList xNodeList = xmlDoc.SelectNodes("NewDataSet/listNews");
			if (xNodeList == null || xNodeList.Count < 1) return null;

			List<XmlElement> elemList = new List<XmlElement>();
			foreach (XmlElement entity in xNodeList)
			{
				elemList.Add(entity);
			}
			return elemList;
		}
		/// <summary>
		/// 添加页面内容
		/// </summary>
		/// <param name="key"></param>
		/// <param name="pc"></param>
		private void AddContentList(string key, int count, List<XmlElement> elemList)
		{
			AddContentList(key, count, elemList, "");
		}
		/// <summary>
		/// 添加页面内容
		/// </summary>
		/// <param name="key"></param>
		/// <param name="pc"></param>
		private void AddContentList(string key, int count, List<XmlElement> elemList, string url)
		{
			ExhibitionPageContent pc = new ExhibitionPageContent();
			pc._Count = count;
			pc._NodeList = elemList;
			pc._Url = url;
			if (contentList.ContainsKey(key))
			{
				contentList[key] = pc;
				return;
			}
			contentList.Add(key, pc);
		}
		#endregion
		/// <summary>
		/// 得到热门车型列表
		/// </summary>
		private void GetHotCarType()
		{
			string template = "<div class='line_box rank_news'><h3><span>热门车型</span></h3>"
							+ "<div class='hotrank_wzh hotrank2_wzh rank_car_wzh'>{0} </div><div class='clear'></div></div>";

			string cacheKey = string.Format("Exhibition_{0}_Serial_HotCarType", _ExhibitionID);
			object obj = BCC.Cache.CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				_HotCarTypeList = (string)obj;
				return;
			}
			string path = string.Format("http://car.bitauto.com/Interface/Exhibition/beijing_2010_DefaultHotCarType.aspx?eid={0}&type=1", _ExhibitionID);
			_HotCarTypeList = BCC.CommonFunction.GetContentByUrl(path);

			if (string.IsNullOrEmpty(_HotCarTypeList)) return;
			_HotCarTypeList = string.Format(template, _HotCarTypeList);

			BCC.Cache.CacheManager.InsertCache(cacheKey, _HotCarTypeList, 60);
		}
		/// <summary>
		/// 得到热门车模列表
		/// </summary>
		private void GetHotModelType()
		{
			string template = "<div class='line_box'><h3><span>热门车模</span></h3>"
							+ "<div class='hotrank_wzh hotrank2_wzh rank_meinv'>{0}</div><div class='clear'></div></div>";

			string cacheKey = string.Format("Exhibition_{0}_Serial_HotModelType", _ExhibitionID);
			object obj = BCC.Cache.CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				_HotModelList = (string)obj;
				return;
			}
			string path = string.Format("http://car.bitauto.com/Interface/Exhibition/beijing_2010_DefaultHotModel.aspx?eid={0}", _ExhibitionID);
			_HotModelList = BCC.CommonFunction.GetContentByUrl(path);

			if (string.IsNullOrEmpty(_HotCarTypeList)) return;
			_HotModelList = string.Format(template, _HotModelList);

			BCC.Cache.CacheManager.InsertCache(cacheKey, _HotModelList, 60);
		}
		/// <summary>
		/// 得到车型图片列表
		/// </summary>
		private void GetCarImageList()
		{
			if (!contentList.ContainsKey("img")) return;
			ExhibitionPageContent pc = contentList["img"];
			if (pc._Count < 1) return;
			StringBuilder htmlContent = new StringBuilder();
			htmlContent.Append("<div class='line_box'>");
			htmlContent.AppendFormat("<h3><span><a href='{0}' target='_blank'>{1} {2} 车展图片</a></span><small><em>{3}</em>张</small></h3>"
									, pc._Url, _BrandName, _SerialName, pc._Count);
			htmlContent.Append("<div class='car_listimg_zh'><ul>");
			int index = 0;
			foreach (XmlElement elem in pc._NodeList)
			{
				if (index > 7)
				{
					break;
				}
				htmlContent.AppendFormat("<li><a href='{0}' target='_blank'><img width='120' height='80' border='0' src='{1}' alt='{2}'>{2}</a></li>"
										, elem.GetAttribute("TargetUrl")
										, string.Format(elem.GetAttribute("ImageUrl"), "1")
										, elem.GetAttribute("ImageName"));
				index++;
			}

			htmlContent.Append("</ul></div>");
			htmlContent.Append("<div class='clear'></div>");
			htmlContent.AppendFormat("<div class='more'><a href='{0}' target='_blank'>更多&gt;&gt;</a></div>", pc._Url);
			htmlContent.Append("</div>");
			_CarImageList = htmlContent.ToString();
		}
		/// <summary>
		/// 得到车模图片
		/// </summary>
		private void GetCarModelList()
		{
			if (!contentList.ContainsKey("model")) return;
			ExhibitionPageContent pc = contentList["model"];
			if (pc._Count < 1) return;
			StringBuilder htmlContent = new StringBuilder();
			htmlContent.Append("<div class='line_box'>");
			htmlContent.AppendFormat("<h3><span><a href='{0}' target='_blank'>{1} {2} 车模图片</a></span><small><em>{3}</em>张</small></h3>"
									, pc._Url, _BrandName, _SerialName, pc._Count);
			htmlContent.Append("<div class='car_listimgs_zh'><ul>");
			int index = 0;
			foreach (XmlElement elem in pc._NodeList)
			{
				if (index > 3)
				{
					break;
				}
				htmlContent.AppendFormat("<li><a href='{0}{1}' target='_blank'><img src='{4}{2}' alt='{3}'>{3}</a></li>"
				, _TargetBase, elem.GetAttribute("TargetUrl")
				, elem.GetAttribute("ImageUrl").Replace("_1.", "_8."), elem.GetAttribute("Name")
				, BCC.CommonFunction.GetPublishHashImageDomain(ConvertHelper.GetInteger(elem.GetAttribute("ImageId"))));
				index++;
			}
			htmlContent.Append("</ul></div>");
			htmlContent.Append("<div class='clear'></div>");
			htmlContent.AppendFormat("<div class='more'><a href='{0}' target='_blank'>更多&gt;&gt;</a></div>", pc._Url);
			htmlContent.Append("</div>");
			_CarModelList = htmlContent.ToString();
		}
		/// <summary>
		/// 得到视频列表
		/// </summary>
		private void GetVideoList()
		{
			if (!contentList.ContainsKey("video")) return;
			ExhibitionPageContent pc = contentList["video"];
			if (pc._Count < 1) return;
			StringBuilder htmlContent = new StringBuilder();
			htmlContent.Append("<div class='line_box'>");
			htmlContent.AppendFormat("<h3><span><a href='{0}' target='_blank'>{1} {2} 车展视频</a></span><small><em>{3}</em>条</small></h3>"
									, pc._Url, _BrandName, _SerialName, pc._Count);
			htmlContent.Append("<div class='v_cx_list'><ul class='v_public'>");
			for (int i = 0; i < pc._NodeList.Count; i++)
			{
				if (i > 3) break;
				string imgUrl = pc._NodeList[i].SelectSingleNode("picture").InnerText;
				string newsUrl = pc._NodeList[i].SelectSingleNode("filepath").InnerText;
				string title = pc._NodeList[i].SelectSingleNode("title").InnerText;
				htmlContent.Append("<li>");
				htmlContent.AppendFormat("<a class='photo' target='_blank' href='{0}'><img width='150' height='100' border='0' src='{1}' alt='{2}' title='{2}'></a>"
							, newsUrl, imgUrl, title);
				htmlContent.AppendFormat("<p class='title'><a target='_blank' href='{0}'>{1}</a></p>", newsUrl, title);
				htmlContent.Append("</li>");
			}
			htmlContent.Append("</ul></div>");
			htmlContent.Append("<div class='clear'></div>");
			htmlContent.AppendFormat("<div class='more'><a href='{0}' target='_blank'>更多&gt;&gt;</a></div>", pc._Url);
			htmlContent.Append("</div>");
			_VideoList = htmlContent.ToString();
		}
		/// <summary>
		/// 得到图解列表
		/// </summary>
		private void GetTuJieList()
		{
			if (!contentList.ContainsKey("tujie")) return;
			ExhibitionPageContent pc = contentList["tujie"];
			if (pc._Count < 1) return;
			StringBuilder htmlContent = new StringBuilder();
			htmlContent.Append("<div class='line_box'>");
			htmlContent.AppendFormat("<h3><span><a href='{0}' target='_blank'>{1} {2} 新车图解</a></span><small><em>{3}</em>条</small></h3>"
									, pc._Url, _BrandName, _SerialName, pc._Count);
			htmlContent.Append("<div class='car_listimg_zh'><ul>");
			int index = 0;
			foreach (XmlElement elem in pc._NodeList)
			{
				if (index > 3)
				{
					break;
				}
				string name = elem.GetAttribute("ImageName");
				string url = elem.GetAttribute("Link");
				string imgurl = elem.GetAttribute("ImageUrl").Replace("_2.", "_1.");
				htmlContent.AppendFormat("<li><a href='{0}' target='_blank'><img src='{1}'>{2}</a> </li>", url, imgurl, name);
				index++;
			}
			htmlContent.Append("</ul></div>");
			htmlContent.Append("<div class='clear'></div>");
			htmlContent.AppendFormat("<div class='more'><a href='{0}' target='_blank'>更多&gt;&gt;</a></div>", pc._Url);
			htmlContent.Append("</div>");
			_TuJieList = htmlContent.ToString();
		}
		/// <summary>
		/// 得到左侧新闻列表
		/// </summary>
		private void GetLeftNewsList()
		{
			if (!contentList.ContainsKey("news")) return;
			ExhibitionPageContent pc = contentList["news"];
			if (pc._Count < 2) return;
			StringBuilder htmlContent = new StringBuilder();
			htmlContent.Append("<div class='line_box rank_news'>");
			htmlContent.AppendFormat("<h3><span>{1}{2}新闻</span></h3>"
									, pc._Url, _BrandName, _SerialName, pc._Count);
			htmlContent.Append("<ul class='boxbar'>");
			int index = 1;
			foreach (XmlElement elem in pc._NodeList)
			{
				if (index == 1)
				{
					index++;
					continue;
				}
				string newsUrl = elem.SelectSingleNode("filepath").InnerText;
				string title = elem.SelectSingleNode("title").InnerText;
				htmlContent.AppendFormat("<li><a href=\"{0}\" target='_blank'>{1}</a></li>", newsUrl, title);
			}
			htmlContent.Append("</ul>");
			htmlContent.Append("<div class='clear'></div>");
			htmlContent.Append("</div>");

			_BrandNewsList = htmlContent.ToString();
		}
	}
}