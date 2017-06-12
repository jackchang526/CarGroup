using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Data;

using BitAuto.Utils;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.CarchannelWeb.App_Code
{
	/// <summary>
	///beijing_2010_PageBase 的摘要说明
	/// </summary>
	public class beijing_2010_PageBase : InterfacePageBase
	{
		/// <summary>
		/// 车展新闻
		/// </summary>
		public struct ExhibitionNew
		{
			public int Id;
			public string Title;
			public string Url;
			public DateTime CreatTime;
		}

		protected int _ExhibitionID = 19;

		protected Dictionary<string, string> PavilionUrl = new Dictionary<string, string>();
		protected Dictionary<string, string> AttributeUrl = new Dictionary<string, string>();
		protected Dictionary<string, string> AttributeUrlReverse = new Dictionary<string, string>();
		protected Dictionary<string, string> CsLevel = new Dictionary<string, string>();
		protected Dictionary<string, string> CsLevelReverse = new Dictionary<string, string>();
		protected Dictionary<string, string> CsPrice = new Dictionary<string, string>();
		//车展链接地址格式
		protected Dictionary<int, string> _UrlFormat = new Dictionary<int, string>();
		//展馆页地址
		protected Dictionary<int, string> _PravilionUrlFormat = new Dictionary<int, string>();
		//投票的Key
		protected Dictionary<int, string> _VoteFormat = new Dictionary<int, string>();

		//子品牌数据类
		private Car_SerialDal csd = new Car_SerialDal();

		public beijing_2010_PageBase()
		{
			//
			//TODO: 在此处添加构造函数逻辑
			//
		}

		protected override void OnLoad(EventArgs e)
		{
			GetExhibitionId();
			InitUrlFormat();
			BuilderPavilionUrlList();
			BuilderAttributeUrlList();
			BuilderLevelUrlList();
			base.OnLoad(e);
		}
		/// <summary>
		/// 得到展会ID
		/// </summary>
		private void GetExhibitionId()
		{
			//如果展会ID不等于空
			if (!string.IsNullOrEmpty(Request.QueryString["eid"]))
			{
				_ExhibitionID = ConvertHelper.GetInteger(Request.QueryString["eid"]);
			}
		}
		/// <summary>
		/// 生成展馆URL列表
		/// </summary>
		protected void BuilderPavilionUrlList()
		{
			string UrlFormat = "E1|1,E2|2,E3|3,E4|4,E5|5,E6|6,W1|7,W2|8,W3|9,W4|10";
			if (_ExhibitionID == 48)
			{
				UrlFormat = "1.1馆|1,1.2馆|2,2.1馆|3,2.2馆|4,3.1馆|5,3.2馆|6,4.1馆|7,4.2馆|8,5.1馆|9,5.2馆|10";
			}
			else if (_ExhibitionID == 59)
			{
				UrlFormat = "E1|1,E2|2,E3|3,E4|4,E5|5,E6|6,E7|7,W1|8,W2|9,W3|10,N5|11";
			}

			PavilionUrl = BuilderList(UrlFormat);
		}
		/// <summary>
		/// 生成属性URL列表
		/// </summary>
		private void BuilderAttributeUrlList()
		{
			string UrlFormat = "首发|1,上市|2,热门|3,环保|4,自主|5,概念|6";
			string UrlId = "9|1,10|2,11|3,12|4,13|5,14|6";

			if (_ExhibitionID == 48)
			{
				UrlFormat = "上市|1,首发|2,热门|3,自主|4,新能源|5,豪华车|6,概念车|7";
				UrlId = "15|1,16|2,17|3,18|4,19|5,20|6,21|7";
			}
			else if (_ExhibitionID == 59)
			{
				UrlFormat = "首发|1,上市|2,热门|3,自主|4,概念|5,环保|6";
				UrlId = "22|1,23|2,24|3,25|4,26|5,27|6";
			}
			AttributeUrl = BuilderList(UrlFormat);
			AttributeUrlReverse = ReverseBuilderList(UrlId);
		}
		/// <summary>
		/// 生成级别列表
		/// </summary>
		private void BuilderLevelUrlList()
		{
			string UrlFormat = "微型车|1,小型车|2,紧凑型|3,中型车|4,中大型|5,豪华车|6,SUV|8,MPV|7,跑车|9,面包车|10";
			CsLevel = BuilderList(UrlFormat);
			CsLevelReverse = ReverseBuilderList(UrlFormat);
		}
		/// <summary>
		/// 初始化链接
		/// </summary>
		private void InitUrlFormat()
		{
			_UrlFormat.Add(19, "http://chezhan.bitauto.com/beijing/2010/{0}/{1}/");
			_UrlFormat.Add(48, "http://chezhan.bitauto.com/guangzhou-chezhan/2010/{0}/{1}/");
			_UrlFormat.Add(59, "http://chezhan.bitauto.com/shanghai/2011/{0}/{1}/");
			_PravilionUrlFormat.Add(59, "http://chezhan.bitauto.com/shanghai/zhanguan/{0}/");

			_VoteFormat.Add(19, "BitAuto.Chezhan.2010");
			_VoteFormat.Add(48, "BitAuto.Chezhan.2010.guangzhou");
			_VoteFormat.Add(59, "BitAuto.Chezhan.2011.shanghai.car");
		}

		/// <summary>
		/// 生成列表
		/// </summary>
		/// <param name="UrlFormat"></param>
		/// <returns></returns>
		private Dictionary<string, string> BuilderList(string UrlFormat)
		{
			string[] UrlList = UrlFormat.Split(',');
			Dictionary<string, string> UrlDicitionary = new Dictionary<string, string>();
			foreach (string entity in UrlList)
			{
				if (!UrlDicitionary.ContainsKey(entity.Split('|')[0]))
				{
					UrlDicitionary.Add(entity.Split('|')[0], entity.Split('|')[1]);
				}
			}
			return UrlDicitionary;
		}
		/// <summary>
		/// 生成反转列表
		/// </summary>
		/// <param name="UrlFormat"></param>
		/// <returns></returns>
		private Dictionary<string, string> ReverseBuilderList(string UrlFormat)
		{
			string[] UrlList = UrlFormat.Split(',');
			Dictionary<string, string> UrlDicitionary = new Dictionary<string, string>();
			foreach (string entity in UrlList)
			{
				if (!UrlDicitionary.ContainsKey(entity.Split('|')[1]))
				{
					UrlDicitionary.Add(entity.Split('|')[1], entity.Split('|')[0]);
				}
			}
			return UrlDicitionary;
		}
		/// <summary>
		/// 得到车展新闻列表
		/// </summary>
		/// <param name="brandType"></param>
		/// <param name="brandId"></param>
		/// <returns></returns>
		public Dictionary<string, ExhibitionNew> GetExhibitionNewsList(string brandType, int brandId)
		{
			return GetExhibitionNewsList(brandType, brandId, "beijing2010");
		}
		/// <summary>
		/// 得到车展新闻列表
		/// </summary>
		/// <param name="brandType"></param>
		/// <param name="brandId"></param>
		/// <returns></returns>
		public Dictionary<string, ExhibitionNew> GetExhibitionNewsList(string brandType, int brandId, string SavePath)
		{
			string sPath = WebConfig.DataBlockPath + "Data\\Exhibition\\" + SavePath + "\\ConfigNewsList.xml";
			if (!File.Exists(sPath))
			{
				return null;
			}

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(sPath);

			if (brandType == "master")
			{
				return GetMasterBrandNewsList(xmlDoc, brandId);
			}
			else
			{
				return GetSerialNewsList(xmlDoc, brandId);
			}
		}
		/// <summary>
		/// 得到主品牌List
		/// </summary>
		/// <param name="xmlDoc"></param>
		/// <param name="brandId"></param>
		/// <returns></returns>
		private Dictionary<string, ExhibitionNew> GetMasterBrandNewsList(XmlDocument xmlDoc, int brandId)
		{
			if (xmlDoc == null || brandId < 1)
			{
				return null;
			}

			XmlElement xElem = (XmlElement)xmlDoc.SelectSingleNode("root/MasterBrandList/MasterBrand[@ID='"
							   + brandId.ToString() + "']");

			if (xElem == null
				|| xElem.ChildNodes == null
				|| xElem.ChildNodes.Count < 1)
			{
				return null;
			}
			Dictionary<string, ExhibitionNew> list = new Dictionary<string, ExhibitionNew>();
			foreach (XmlElement entity in xElem)
			{
				ExhibitionNew enews = new ExhibitionNew();
				enews.Id = ConvertHelper.GetInteger(entity.GetAttribute("ID"));
				enews.Title = entity.GetAttribute("Title");
				enews.CreatTime = Convert.ToDateTime(entity.GetAttribute("Time"));
				enews.Url = entity.GetAttribute("Url");

				if (!list.ContainsKey(enews.Id.ToString()))
				{
					list.Add(enews.Id.ToString(), enews);
				}

			}
			return list;
		}
		/// <summary>
		/// 得到主品牌List
		/// </summary>
		/// <param name="xmlDoc"></param>
		/// <param name="serialId"></param>
		/// <returns></returns>
		private Dictionary<string, ExhibitionNew> GetSerialNewsList(XmlDocument xmlDoc, int serialId)
		{
			if (xmlDoc == null || serialId < 1)
			{
				return null;
			}

			XmlElement xElem = (XmlElement)xmlDoc.SelectSingleNode("root/SerialList/Serial[@ID='"
							   + serialId.ToString() + "']");

			if (xElem == null
				|| xElem.ChildNodes == null
				|| xElem.ChildNodes.Count < 1)
			{
				return null;
			}
			Dictionary<string, ExhibitionNew> list = new Dictionary<string, ExhibitionNew>();
			foreach (XmlElement entity in xElem)
			{
				ExhibitionNew enews = new ExhibitionNew();
				enews.Id = ConvertHelper.GetInteger(entity.GetAttribute("ID"));
				enews.Title = entity.GetAttribute("Title");
				enews.CreatTime = Convert.ToDateTime(entity.GetAttribute("Time"));
				enews.Url = entity.GetAttribute("Url");

				if (!list.ContainsKey(enews.Id.ToString()))
				{
					list.Add(enews.Id.ToString(), enews);
				}

			}
			return list;
		}

		/// <summary>
		/// 如果存在此图片
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public bool IsExitsUrl(string url)
		{
			bool IsSuccess = false;
			HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(url);
			httpReq.Method = "head";
			HttpWebResponse res = null;
			try
			{
				res = (HttpWebResponse)httpReq.GetResponse();
				if (res.StatusCode == HttpStatusCode.OK)
				{
					IsSuccess = true;
				}

			}
			catch
			{

			}
			finally
			{
				if (res != null)
				{
					res.Close();
				}
			}

			return IsSuccess;
		}
		/// <summary>
		/// 得到图片地址
		/// </summary>
		/// <param name="xElem"></param>
		/// <returns></returns>
		protected string GetImageUrl(XmlElement xElem)
		{
			string imgUrl = xElem.GetAttribute("ImageUrl");
			string imgId = xElem.GetAttribute("ImageId");
			if (string.IsNullOrEmpty(imgUrl))
			{
				return "";
			}
			else if (!string.IsNullOrEmpty(imgUrl)
				&& string.IsNullOrEmpty(imgId))
			{
				return imgUrl;
			}
			else if (!string.IsNullOrEmpty(imgUrl)
				&& !string.IsNullOrEmpty(imgId))
			{
				string imgDomain = CommonFunction.GetPublishHashImageDomain(ConvertHelper.GetInteger(imgId));
				if (string.IsNullOrEmpty(imgDomain)) return imgUrl;
				return Path.Combine(imgDomain, imgUrl);
			}

			return "";
		}
		/// <summary>
		/// 得到投票的链接
		/// </summary>
		protected string GetUserVoteUrl()
		{
			string voteResultUrl = "http://www.bitauto.com/vote/getCountList.aspx?akey={0}";

			if (!_VoteFormat.ContainsKey(_ExhibitionID)) return "";
			return string.Format(voteResultUrl, _VoteFormat[_ExhibitionID]);
		}
		/// <summary>
		/// 通过子品牌ID得到子品牌报价
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected string GetSerialOfficePriceById(int id)
		{
			Dictionary<int, string> priceList = GetAllSerialOfficePrice();

			if (priceList == null || priceList.Count < 1 || !priceList.ContainsKey(id))
				return "0";

			return priceList[id];
		}
		/// <summary>
		/// 得到子品牌链接
		/// </summary>
		/// <param name="xNode"></param>
		/// <returns></returns>
		protected string GetSerialUrl(XmlNode xNode)
		{
			if (xNode == null || xNode.Name != "Serial") return "";
			XmlElement xEleme = (XmlElement)xNode;
			int newCar = ConvertHelper.GetInteger(xEleme.GetAttribute("NC"));
			string masterSpell = ((XmlElement)xEleme.ParentNode.ParentNode).GetAttribute("AllSpell").ToString().ToLower();
			string serialSpell = xEleme.GetAttribute("AllSpell").ToLower();

			return GetSerialUrl(masterSpell, serialSpell, newCar);
		}
		/// <summary>
		/// 得到子品牌链接
		/// </summary>
		/// <param name="masterSpell"></param>
		/// <param name="serialSpell"></param>
		/// <param name="isNewCar"></param>
		/// <returns></returns>
		protected string GetSerialUrl(string masterSpell, string serialSpell, int isNewCar)
		{
			string serialUrl = "";
			if (isNewCar == 1)
			{
				serialUrl = string.Format(_UrlFormat[_ExhibitionID]
								, masterSpell
								, serialSpell);
			}
			else
			{
				serialUrl = string.Format("http://car.bitauto.com/{0}/", serialSpell);
			}
			return serialUrl;
		}
		/// <summary>
		/// 得到所有子品牌官方指导价
		/// </summary>
		/// <returns></returns>
		private Dictionary<int, string> GetAllSerialOfficePrice()
		{
			string cacheKey = "exhibitionOfficePrice";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (Dictionary<int, string>)obj;

			DataSet ds = csd.GetAllSerialOfficePrice(true);
			if (ds == null) return null;

			Dictionary<int, string> priceList = new Dictionary<int, string>();

			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				int serialId = dr.IsNull("cs_id") ? 0 : ConvertHelper.GetInteger(dr["cs_id"]);
				string priceArrang = string.Empty;
				if (dr.IsNull("minprice"))
				{
					priceArrang = "0";
				}
				else if (ConvertHelper.GetDecimal(dr["minprice"]) == ConvertHelper.GetDecimal(dr["maxprice"]))
				{
					priceArrang = dr["minprice"].ToString() + "万";
				}
				else
				{
					priceArrang = dr["minprice"].ToString() + "万" + "-" + dr["maxprice"].ToString() + "万";
				}
				if (priceList.ContainsKey(serialId)) continue;
				priceList.Add(serialId, priceArrang);
			}

			CacheManager.InsertCache(cacheKey, priceList, WebConfig.CachedDuration);
			return priceList;
		}
	}
	/// <summary>
	/// 页面内容
	/// </summary>
	public class ExhibitionPageContent
	{
		public int _Count = 0;
		public string _Url = string.Empty;
		public List<XmlElement> _NodeList = new List<XmlElement>();
	}
}