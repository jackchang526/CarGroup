using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Exhibition
{
	public partial class _2011guangzhou_chezhan_Serial : ExhibitionPageBase
	{
		#region member

		private string _SerialAllSpell = "";
		private int _SerialId = 0;
		private XmlDocument _XmlDoc = new XmlDocument();
		private XmlElement _SerialXmlNode;
		private XmlDocument _AlbumXmlDoc = new XmlDocument();
		private Dictionary<int, BitAuto.CarChannel.Model.Pavilion> pavilionList = new Dictionary<int, BitAuto.CarChannel.Model.Pavilion>();
		private Dictionary<string, ExhibitionPageContent> contentList = new Dictionary<string, ExhibitionPageContent>();

		private string _SaveNewsUrl = "2011guangzhou";

		protected string _TargetBase = string.Empty;
		protected string _SerialName = string.Empty;
		protected string _SerialSeoName = string.Empty;
		protected string _BrandName = string.Empty;

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
		/// 车型图片列表
		/// </summary>
		protected string _CarImageList = string.Empty;

		/// <summary>
		/// 图解列表
		/// </summary>
		protected string _TuJieList = string.Empty;

		/// <summary>
		/// 车模图片列表
		/// </summary>
		protected string _CarModelList = string.Empty;

		/// <summary>
		/// 视频列表
		/// </summary>
		protected string _VideoList = string.Empty;

		/// <summary>
		/// 热门车模列表
		/// </summary>
		protected string _HotModelList = string.Empty;

		/// <summary>
		/// 热门车型列表
		/// </summary>
		protected string _HotCarTypeList = string.Empty;

		/// <summary>
		/// 品牌列表
		/// </summary>
		protected string _BrandNewsList = string.Empty;

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			_ExhibitionID = 71;
			base.IntiExhibitionByID(_ExhibitionID);
			if (!this.IsPostBack)
			{
				GetParam();
				if (!ValidatorParam())
				{
					string url = "http://chezhan.bitauto.com/guangzhou-chezhan/";
					//if (base._DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && _DicExhibitionBaseInfo[_ExhibitionID].UrlFormat != "")
					//{ url = _DicExhibitionBaseInfo[_ExhibitionID].UrlFormat; }
					//else
					//{ url = "http://chezhan.bitauto.com/"; }
					Response.Redirect(url, true);
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

				// 得到页面内容
				GetPageContent();

				// 初始化导航
				InitGuilder();

				//得到子品牌描述
				OuterGuilder();

				// 得到焦点区域内容
				GetFocusContent();

				// 得到车型图片列表
				GetCarImageList();

				// 得到图解列表
				GetTuJieList();

				// 得到车模图片
				GetCarModelList();

				// 得到视频列表
				GetVideoList();

				// 得到热门车模列表
				GetHotModelType();

				// 得到热门车型列表
				GetHotCarType();

				// 得到品牌新闻列表
				GetBrandNewsList();
			}
		}

		#region private Method

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

			_XmlDoc = new BitAuto.CarChannel.BLL.Exhibition().GetExibitionXmlByExhibitionId(_ExhibitionID, 10);

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

			pavilionList = new BitAuto.CarChannel.BLL.Exhibition().GetPavilionListByExhibitionId(_ExhibitionID);

			if (pavilionList == null || pavilionList.Count < 1)
			{
				return false;
			}
			_AlbumXmlDoc = new BitAuto.CarChannel.BLL.ExhibitionAlbum().GetCommonAlbumRelationData(_ExhibitionID);
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
			string pavilionUrl = "";// string.Format("http://chezhan.bitauto.com/guangzhou/zhanguan/n2011/{0}/{1}/", PavilionUrl[pavilionList[pavilionId].Name.Trim()], brandId);
			if (base._DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && base._DicExhibitionBaseInfo[_ExhibitionID].PravilionUrlFormat != "")
			{ pavilionUrl = string.Format(_DicExhibitionBaseInfo[_ExhibitionID].PravilionUrlFormat, PavilionUrl[pavilionList[pavilionId].Name.Trim()]) + brandId.ToString() + "/"; }

			StringBuilder guilderString = new StringBuilder();
			// guilderString.Append("<dd><a href='http://chezhan.bitauto.com/guangzhou-chezhan/'>2011广州车展</a></dd>");
			guilderString.AppendFormat("<a href='{0}'>{2}-{1}</a> &gt; ", pavilionUrl, brandName, pavilionList[pavilionId].Name);
			guilderString.AppendFormat("{0}", serialName.Replace("·", "&bull;"));
			_GuilderString = guilderString.ToString();
		}

		/// <summary>
		/// 初始化其他关联导航
		/// </summary>
		private void OuterGuilder()
		{
			string masterLogo = string.Format("http://img1.bitauto.com/bt/car/default/images/carimage/m_{0}_a.png",
											  _SerialXmlNode.ParentNode.ParentNode.Attributes["ID"].Value);
			string masterName = _SerialXmlNode.ParentNode.ParentNode.Attributes["Name"].Value.Replace("·", "&bull;");
			string brandName = _SerialXmlNode.ParentNode.Attributes["Name"].Value.Replace("·", "&bull;");
			string serialName = _SerialXmlNode.GetAttribute("sName").Replace("·", "&bull;");
			StringBuilder describeString = new StringBuilder();
			describeString.Append("<div class='mxl_car_top'>");
			describeString.AppendFormat("<img src='{0}' alt='{1}'></a>", masterLogo, masterName);
			describeString.AppendFormat(" <h3 class=\"mxl_com_fl\">{0} {1}</h3>", brandName, serialName);
			string baaUrl = new BitAuto.CarChannel.BLL.Car_SerialBll().GetForumUrlBySerialId(_SerialId);
			if (_SerialXmlNode.GetAttribute("CsLevel") != "概念车")
			{
				serialName = _SerialXmlNode.GetAttribute("Name");
				string serialAllSpell = _SerialXmlNode.GetAttribute("AllSpell");
				describeString.Append("<div class='mxl_cartop_r'>");
				describeString.AppendFormat("<a href='http://car.bitauto.com/{0}/' target='_blank'>车型</a><span> | ", serialAllSpell);
				describeString.AppendFormat("</span><a href='http://price.bitauto.com/frame.aspx?newbrandid={0}' target='_blank'>报价</a><span> | ", _SerialId);
				describeString.AppendFormat("</span><a href='http://car.bitauto.com/{0}/koubei/' target='_blank'>口碑</a><span> | ", serialAllSpell);
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
			StringBuilder focusContent = new StringBuilder();
			focusContent.AppendLine("<div class=\"mxl_car_indexbox\">");
			focusContent.AppendLine("<div class=\"car_box\">");
			_TargetBase = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value;
			XmlNode _AlbumNode = _AlbumXmlDoc.SelectSingleNode(string.Format("Data/NewCar/Master/Serial[@Id={0}]", _SerialId));
			if (_AlbumNode != null)
			{
				string targetUrl = ((XmlElement)_AlbumNode).GetAttribute("TargetUrl");
				//得到图片的域名
				string imgUrl = GetImageUrl(((XmlElement)_AlbumNode));

				focusContent.AppendFormat("<div class=\"photos\"><a target='_blank' href='{0}{1}'><img src='{2}'></a></div>", _TargetBase, targetUrl, imgUrl.Replace("_1.", "_4."));
			}
			focusContent.AppendLine("<div class=\"car_text\">");
			// 
			//新闻部分内容
			if (contentList.ContainsKey("news") && contentList["news"]._Count > 0)
			{
				XmlElement newsElem = contentList["news"]._NodeList[0];
				string newsUrl = newsElem.SelectSingleNode("filepath").InnerText;
				// string title = newsElem.SelectSingleNode("title").InnerText;
				string title = newsElem.SelectSingleNode("facetitle").InnerText;
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
				// focusContent.Append("<center></center>");
			}
			else
			{
				focusContent.Append("<h2></h2><p></p>");
			}
			//添加视频内容
			focusContent.Append("<ul class='news_list'>");
			if (contentList.ContainsKey("video") && contentList["video"]._Count > 0)
			{
				for (int i = 0; i < contentList["video"]._NodeList.Count; i++)
				{
					if (i > 1)
					{
						break;
					}
					XmlNode xNode = contentList["video"]._NodeList[i];
					string newsUrl = xNode.SelectSingleNode("filepath").InnerText;
					// string title = xNode.SelectSingleNode("title").InnerText;
					string title = xNode.SelectSingleNode("facetitle").InnerText;
					string newsDate = Convert.ToDateTime(xNode.SelectSingleNode("publishtime").InnerText).ToString("MM.dd");
					focusContent.AppendFormat("<li><a href='{0}' target='_blank' title='{1}'>{1}</a><span class=\"date\">{2}</span></li>", newsUrl, title, newsDate);
				}
			}
			focusContent.Append("</ul>");
			//添加数字统计
			focusContent.AppendLine("<div class='bottom'>");
			if (contentList.ContainsKey("img") && contentList["img"]._Count >= 0)
			{
				focusContent.AppendFormat("<p class=\"\"><b>图片</b> <a href='{0}' target='_blank'>{1}张</a></p>", contentList["img"]._Url, contentList["img"]._Count);
			}
			if (contentList.ContainsKey("model") && contentList["model"]._Count >= 0)
			{
				focusContent.AppendFormat("<p class=\"\"><b>车模</b> <a href='{0}' target='_blank'>{1}张</a></p>", contentList["model"]._Url, contentList["model"]._Count);
			}
			if (contentList.ContainsKey("video") && contentList["video"]._Count >= 0)
			{
				focusContent.AppendFormat("<p class=\"\"><b>视频</b> <a href='{0}' target='_blank'>{1}条</a></p>", contentList["video"]._Url, contentList["video"]._Count);
			}
			//if (contentList.ContainsKey("tujie") && contentList["tujie"]._Count > 0)
			//{
			//    focusContent.AppendFormat("<span><a href='{0}' target='_blank'><strong>图解</strong><em>{1}张</em></a></span>", contentList["tujie"]._Url, contentList["tujie"]._Count);
			//}
			focusContent.Append("</div>");
			// 
			focusContent.AppendLine("</div>");
			focusContent.AppendLine("</div>");
			focusContent.AppendLine("</div>");
			_FocusContent = focusContent.ToString();
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
			htmlContent.Append("<div class=\"clear\"></div>");
			htmlContent.Append("</ul></div>");
			htmlContent.Append("<div class='clear'></div>");
			htmlContent.AppendFormat("<div class='more'><a href='{0}' target='_blank'>更多&gt;&gt;</a></div>", pc._Url);
			htmlContent.Append("</div>");
			_CarImageList = htmlContent.ToString();
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
			htmlContent.AppendFormat("<h3><span><a href='{0}' target='_blank'>{1} {2} 新车图解</a></span><small><em>{3}</em>张</small></h3>"
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
			htmlContent.Append("<div class='car_listimg_zh'><div class=\"car_listimgs_mxl\"><ul>");
			int index = 0;
			foreach (XmlElement elem in pc._NodeList)
			{
				// if (index > 3)
				if (index > 7)
				{
					break;
				}
				htmlContent.AppendLine("<li><div class=\"yy\"></div>");
				htmlContent.AppendFormat("<div class=\"con\"><a href='{0}{1}' target='_blank'><img src='{4}{2}' alt='{3}'></a> <a target='_blank' class=\"mt_name\" href=\"{0}{1}\">{3}</a></div></li>"
				, _TargetBase, elem.GetAttribute("TargetUrl")
				, elem.GetAttribute("ImageUrl").Replace("_1.", "_8."), elem.GetAttribute("Name")
				, BitAuto.CarChannel.Common.CommonFunction.GetPublishHashImageDomain(ConvertHelper.GetInteger(elem.GetAttribute("ImageId"))));
				index++;
			}
			htmlContent.Append("</ul></div></div>");
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
			htmlContent.Append("<div class='car_listimg_zh'><ul>");
			for (int i = 0; i < pc._NodeList.Count; i++)
			{
				if (i > 3) break;
				string imgUrl = pc._NodeList[i].SelectSingleNode("picture").InnerText;
				string newsUrl = pc._NodeList[i].SelectSingleNode("filepath").InnerText;
				// string title = pc._NodeList[i].SelectSingleNode("title").InnerText;
				string title = pc._NodeList[i].SelectSingleNode("facetitle").InnerText;
				htmlContent.Append("<li>");
				htmlContent.AppendFormat("<a class='photo' target='_blank' href='{0}'><img width='150' height='100' border='0' src='{1}' alt='{2}' title='{2}'>"
							, newsUrl, imgUrl, title);
				htmlContent.AppendFormat("{0}</a>", title);
				htmlContent.Append("</li>");
			}
			htmlContent.Append("</ul></div>");
			htmlContent.Append("<div class='clear'></div>");
			htmlContent.AppendFormat("<div class='more'><a href='{0}' target='_blank'>更多&gt;&gt;</a></div>", pc._Url);
			htmlContent.Append("</div>");
			_VideoList = htmlContent.ToString();
		}

		/// <summary>
		/// 得到热门车模列表
		/// </summary>
		private void GetHotModelType()
		{
			string template = "<div class='line_box'><h3><span>热门车模</span></h3>"
				+ "<!-- <div class=\"more\"><a target=\"_blank\" href=\"#\">更多&gt;&gt;</a></div> -->"
							+ "<div class='hotrank_wzh hotrank3_wzh'>{0}</div><div class='clear'></div></div>";

			string cacheKey = string.Format("Exhibition_{0}_Serial_HotModelType", _ExhibitionID);
			object obj = BitAuto.CarChannel.Common.Cache.CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				_HotModelList = (string)obj;
				return;
			}

			//----------------
			int StarndValue = 10000;
			XmlNodeList xNodeList = _AlbumXmlDoc.SelectNodes("Data/Model/Master[@Count!='0']");
			if (xNodeList == null || xNodeList.Count < 1)
			{
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
			string modelUrl = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString() + elem[0].GetAttribute("TargetUrl");
			string imgUrl = BitAuto.CarChannel.Common.CommonFunction.GetPublishHashImageDomain(ConvertHelper.GetInteger(elem[0].GetAttribute("ImageId")))
									+ elem[0].GetAttribute("ImageUrl").Replace("_1.", "_2.");
			// 错误跟踪
			if (modelUrl.Length > 50 && imgUrl.Length > 100)
			{
				string path = Path.Combine(WebConfig.DataBlockPath, "CommonLog\\");
				_AlbumXmlDoc.Save(path + "AlbumXmlDoc" + DateTime.Now.ToShortDateString() + ".xml");
				CommonFunction.WriteLog("TargetUrlBase:" + _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString());
			}
			hotString.AppendLine("<ol class=\"clearfix ol_notop1 mxl_olbg_01\">");
			hotString.Append("<li class=\"act_cm_li\">");
			hotString.AppendFormat("<div class=\"head_img\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\"></a>", modelUrl, imgUrl);
			hotString.Append("<div class=\"top1_img\"></div>");
			hotString.Append("</div>");
			hotString.Append("<h5>" + elem[0].GetAttribute("Name") + "</h5>");
			hotString.AppendFormat("<p class=\"guanzhu_num\">关注度:<em>{0}</em></p>", StarndValue + ConvertHelper.GetInteger(10 - index) * 100 + ConvertHelper.GetInteger(10 - index) * (10 - index) - new Random().Next(2, 10));
			hotString.Append("</li>");

			//hotString.Append("<dl class='clearfix'>");
			//hotString.AppendFormat("<dt><a href='{0}' target=\"_blank\"><span><img src='{1}'></span></a></dt>", modelUrl, imgUrl);
			//hotString.Append("<dd><div class='first'></div>");
			//hotString.AppendFormat("<a href='{0}'target=\"_blank\">{1}</a>", modelUrl, elem[0].GetAttribute("Name"));
			//hotString.AppendFormat("<p>关注度：{0}</p></dd>", StarndValue + ConvertHelper.GetInteger(10 - index) * 100 + ConvertHelper.GetInteger(10 - index) * (10 - index) - new Random().Next(2, 10));
			//hotString.Append("</dl>");
			//hotString.Append("<ol class='clearfix'>");
			foreach (XmlElement xElem in elem)
			{
				if (index < 2)
				{
					index++;
					continue;
				}
				//string liClass = "";
				//if (index < 4)
				//{
				//    liClass = "redcc_wzh";
				//}
				if (index > 10)
				{
					break;
				}
				Name = xElem.GetAttribute("Name");
				Url = _AlbumXmlDoc.SelectSingleNode("Data").Attributes["TargetUrlBase"].Value.ToString() + xElem.GetAttribute("TargetUrl");
				ImageCount = xElem.GetAttribute("Count");
				//<!--需要拼接-->
				hotString.AppendFormat("<li class='{3}'><a target=\"_blank\" href=\"{0}\">{1}</a><small  class=\"cm_num\">{2}</small></li>"
									 , Url
									 , Name
									 , StarndValue + ConvertHelper.GetInteger(10 - index) * 100 + ConvertHelper.GetInteger(10 - index) * (10 - index) - new Random().Next(2, 10)
									 , "");
				// , liClass);
				index++;
			}
			hotString.Append("</ol>");
			//----------------
			//string path = string.Format("http://car.bitauto.com/Interface/Exhibition/beijing_2010_DefaultHotModel.aspx?eid={0}", _ExhibitionID);
			//_HotModelList = BitAuto.CarChannel.Common.CommonFunction.GetContentByUrl(path);

			_HotModelList = hotString.ToString();

			if (string.IsNullOrEmpty(_HotModelList)) return;
			_HotModelList = string.Format(template, _HotModelList);

			BitAuto.CarChannel.Common.Cache.CacheManager.InsertCache(cacheKey, _HotModelList, 60);
		}

		/// <summary>
		/// 得到热门车型列表
		/// </summary>
		private void GetHotCarType()
		{
			string template = "<div class='line_box'><h3><span>热门车型</span></h3>"
				+ "<!-- <div class=\"more\"><a target=\"_blank\" href=\"#\">更多&gt;&gt;</a></div> -->"
							+ "<div class='hotrank_wzh hotrank3_wzh'>{0} </div></div>";

			string cacheKey = string.Format("Exhibition_{0}_Serial_HotCarType", _ExhibitionID);
			object obj = BitAuto.CarChannel.Common.Cache.CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				_HotCarTypeList = (string)obj;
				return;
			}

			//--------------------
			StringBuilder list = new StringBuilder();
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
				List<BitAuto.CarChannel.Common.Enum.EnumCollection.SerialSortForInterface> eesfiList = new BitAuto.CarChannel.BLL.Car_SerialBll().GetBeiJing2010SerialTop10(10, getExhibitionCarType);
				if (eesfiList != null && eesfiList.Count > 0)
				{
					list.Append(" <ol class='clearfix mxl_olbg_02'>");
					int index = 0;
					foreach (BitAuto.CarChannel.Common.Enum.EnumCollection.SerialSortForInterface entity in eesfiList)
					{
						//if (index < 1)
						//{
						//    index++;
						//    continue;
						//}
						//子品牌链接
						XmlElement xNode = (XmlElement)_XmlDoc.SelectSingleNode(string.Format("root/MasterBrand/Brand/Serial[@ID={0}]", entity.CsID));
						if (xNode == null) continue;
						//string liClass = "";
						//if (index < 3)
						//{
						//    liClass = "redcc_wzh";
						//}
						//else if (index == eesfiList.Count - 1)
						//{
						//    liClass = "noline";
						//}
						int isNewsCar = ConvertHelper.GetInteger(xNode.GetAttribute("NC"));
						string serialUrl = GetSerialUrl(xNode.ParentNode.ParentNode.Attributes["AllSpell"].Value.ToString(), entity.CsAllSpell, isNewsCar);

						list.AppendFormat("<li {3}><a href=\"{0}\" target=\"_blank\">{1}</a><small class=\"cm_num\">{2}</small></li>"
										 , serialUrl
										 , entity.CsShowName
										 , entity.CsPV.ToString()
										 , "");
						// , liClass);
						index++;
					}
					list.Append("</ol>");
				}
			}
			//--------------------
			//string path = string.Format("http://car.bitauto.com/Interface/Exhibition/beijing_2010_DefaultHotCarType.aspx?eid={0}&type=1", _ExhibitionID);
			//_HotCarTypeList = BitAuto.CarChannel.Common.CommonFunction.GetContentByUrl(path);
			_HotCarTypeList = list.ToString();

			if (string.IsNullOrEmpty(_HotCarTypeList)) return;
			_HotCarTypeList = string.Format(template, _HotCarTypeList);

			BitAuto.CarChannel.Common.Cache.CacheManager.InsertCache(cacheKey, _HotCarTypeList, 60);
		}

		/// <summary>
		/// 得到品牌新闻列表
		/// </summary>
		private void GetBrandNewsList()
		{
			if (!contentList.ContainsKey("news")) return;
			ExhibitionPageContent pc = contentList["news"];
			if (pc._Count < 2) return;
			StringBuilder htmlContent = new StringBuilder();
			htmlContent.Append("<div class='line_box'>");
			htmlContent.AppendFormat("<h3><span>{1}{2}新闻</span></h3>"
									, pc._Url, _BrandName, _SerialName, pc._Count);
			htmlContent.AppendLine("<!-- <div class=\"more\"><a target=\"_blank\" href=\"#\">更多&gt;&gt;</a></div> -->");
			htmlContent.AppendLine("<div class=\"hotrank_wzh hotrank3_wzh\">");
			htmlContent.Append("<ul class='clearfix h_auto'>");
			int index = 1;
			foreach (XmlElement elem in pc._NodeList)
			{
				if (index > 5)
				{ break; }
				index++;
				//if (index == 1)
				//{
				//    index++;
				//    continue;
				//}
				string newsUrl = elem.SelectSingleNode("filepath").InnerText;
				// string title = elem.SelectSingleNode("title").InnerText;
				string title = elem.SelectSingleNode("facetitle").InnerText;
				htmlContent.AppendFormat("<li><a href=\"{0}\" alt=\"{1}\" title=\"{1}\" target='_blank'>{2}</a></li>", newsUrl, title, BitAuto.Utils.StringHelper.SubString(title, 27, false));
			}
			htmlContent.Append("</ul>");
			htmlContent.AppendLine("</div>");
			// htmlContent.Append("<div class='clear'></div>");
			htmlContent.Append("</div>");

			_BrandNewsList = htmlContent.ToString();
		}

		#endregion

		#region GetPageContent
		/// <summary>
		/// 得到页面内容
		/// </summary>
		private void GetPageContent()
		{
			string cacheKey = string.Format("Exhibition_{0}_GuangZhou_{1}_PageContent", _ExhibitionID, _SerialId);
			object obj = BitAuto.CarChannel.Common.Cache.CacheManager.GetCachedData(cacheKey);
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
			BitAuto.CarChannel.Common.Cache.CacheManager.InsertCache(cacheKey, contentList, 10);
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
			string videoUrl = string.Format("http://car.bitauto.com/{0}/shipin/?pageindex=1&order=_new", _SerialAllSpell.ToLower());
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
										+ "showaccount=true&serialid={0}&rownum=4&groupid=12&orderby=&imagename=chezhan-guangzhou2011", _SerialId);
			// 调整后的 图解地址 例如 
			// http://imgsvr.bitauto.com/Photo/ImageService.aspx?dataname=imageingroup&showimages=true&showfullurl=true&showaccount=true&serialid=2190&rownum=4&groupid=12&orderby=&imagename=chezhan-guangzhou2011
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
			catch (Exception ex)
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
	}
}