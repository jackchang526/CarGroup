using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.CarchannelWeb.App_Code
{
	/// <summary>
	///ExhibitionPageBase 的摘要说明
	/// </summary>
	public class ExhibitionPageBase : InterfacePageBase
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

		/// <summary>
		/// 车展基础信息
		/// </summary>
		public struct ExhibitionBaseInfo
		{
			public int ExhibitionID;					// 车展ID
			public string UrlFormat;					// URL地址规则
			public string PravilionUrlFormat;	// 展厅地址规则
			public string VoteCarFormat;					// 投票车型接口地址
			public string VoteModFormat;					// 投票车模接口地址
		}

		#region member

		/// <summary>
		/// 车展ID
		/// 71:2011广州车展
		/// </summary>
		protected int _ExhibitionID = 0;

		// 车展配置节点
		protected Dictionary<int, ExhibitionBaseInfo> _DicExhibitionBaseInfo = new Dictionary<int, ExhibitionBaseInfo>();

		//车展链接地址格式
		// protected Dictionary<int, string> _UrlFormat = new Dictionary<int, string>();
		//展馆页地址
		// protected Dictionary<int, string> _PravilionUrlFormat = new Dictionary<int, string>();
		protected Dictionary<string, string> PavilionUrl = new Dictionary<string, string>();
		//投票的Key
		// protected Dictionary<int, string> _VoteFormat = new Dictionary<int, string>();

		protected Dictionary<string, string> AttributeUrl = new Dictionary<string, string>();
		protected Dictionary<string, string> AttributeUrlReverse = new Dictionary<string, string>();
		// private Dictionary<int, EnumCollection.ExhibitionAttribute> _DicExhibitionAttribute = new Dictionary<int, EnumCollection.ExhibitionAttribute>();

		protected Dictionary<string, string> CsLevel = new Dictionary<string, string>();
		protected Dictionary<string, string> CsLevelReverse = new Dictionary<string, string>();

		// 子品牌解析 int,string 子品牌ID,解析的新闻地址
		protected Dictionary<int, string> _DicSerialJieXi = new Dictionary<int, string>();

		#endregion
		public ExhibitionPageBase()
		{
			//
			//TODO: 在此处添加构造函数逻辑
			//
		}

		protected override void OnLoad(EventArgs e)
		{
			GetExhibitionId();
			if (_ExhibitionID > 0)
			{
				IntiExhibitionByID(_ExhibitionID);
				//// 初始化配置文件
				//IntiExhibitionBaseInfo();
				//InitUrlFormat();
				//BuilderAttributeUrlList();
				//BuilderLevelUrlList();
				//// 新闻解析地址
				//IntiSerialJieXiURL();
			}
			base.OnLoad(e);
		}

		/// <summary>
		/// 初始化根据车展ID
		/// </summary>
		/// <param name="exhibitionId"></param>
		protected void IntiExhibitionByID(int exhibitionId)
		{
			_ExhibitionID = exhibitionId;
			if (_ExhibitionID > 0)
			{
				// 初始化配置文件
				IntiExhibitionBaseInfo();
				InitUrlFormat();
				BuilderAttributeUrlList();
				BuilderLevelUrlList();
				// 新闻解析地址
				IntiSerialJieXiURL();
			}
		}

		/// <summary>
		/// 得到展会ID
		/// </summary>
		private void GetExhibitionId()
		{
			//如果展会ID不等于空
			if (!string.IsNullOrEmpty(Request.QueryString["eid"]))
			{
				int.TryParse(Request.QueryString["eid"], out _ExhibitionID);
			}
		}

		/// <summary>
		/// 初始化配置文件
		/// </summary>
		private void IntiExhibitionBaseInfo()
		{
			string cache = "ExhibitionPageBase_IntiExhibitionBaseInfo";
			object obj = CacheManager.GetCachedData(cache);
			if (obj != null)
			{
				_DicExhibitionBaseInfo = (Dictionary<int, ExhibitionBaseInfo>)obj;
			}
			else
			{
				// 车展配置文件
				string fileConfig = WebConfig.DataBlockPath + "Data\\Exhibition\\ExhibitionConfig.xml";
				if (File.Exists(fileConfig))
				{
					XmlDocument docExhibitionConfig = new XmlDocument();
					docExhibitionConfig.Load(fileConfig);
					if (docExhibitionConfig != null && docExhibitionConfig.HasChildNodes)
					{
						XmlNodeList xnl = docExhibitionConfig.SelectNodes("/Exhibition/item");
						if (xnl != null && xnl.Count > 0)
						{
							foreach (XmlNode xn in xnl)
							{
								int eid = 0;
								if (int.TryParse(xn.Attributes["id"].Value.ToString(), out eid))
								{
									if (eid > 0 && !_DicExhibitionBaseInfo.ContainsKey(eid))
									{
										ExhibitionBaseInfo ebi = new ExhibitionBaseInfo();
										ebi.ExhibitionID = eid;
										XmlNodeList xnlurlFormat = xn.SelectNodes("urlFormat");
										if (xnlurlFormat != null && xnlurlFormat.Count > 0 && xnlurlFormat[0].Attributes["url"] != null)
										{ ebi.UrlFormat = xnlurlFormat[0].Attributes["url"].Value; }

										XmlNodeList xnlpravilionUrlFormat = xn.SelectNodes("pravilionUrlFormat");
										if (xnlpravilionUrlFormat != null && xnlpravilionUrlFormat.Count > 0 && xnlpravilionUrlFormat[0].Attributes["url"] != null)
										{ ebi.PravilionUrlFormat = xnlpravilionUrlFormat[0].Attributes["url"].Value; }

										XmlNodeList xnlvoteCarResultUrl = xn.SelectNodes("voteCarResultUrl");
										if (xnlvoteCarResultUrl != null && xnlvoteCarResultUrl.Count > 0 && xnlvoteCarResultUrl[0].Attributes["url"] != null)
										{ ebi.VoteCarFormat = xnlvoteCarResultUrl[0].Attributes["url"].Value; }

										XmlNodeList xnlvoteModResultUrl = xn.SelectNodes("voteModResultUrl");
										if (xnlvoteModResultUrl != null && xnlvoteModResultUrl.Count > 0 && xnlvoteModResultUrl[0].Attributes["url"] != null)
										{ ebi.VoteModFormat = xnlvoteCarResultUrl[0].Attributes["url"].Value; }

										_DicExhibitionBaseInfo.Add(eid, ebi);
									}
								}
							}
						}
						CacheManager.InsertCache(cache, _DicExhibitionBaseInfo, WebConfig.CachedDuration);
					}
				}
			}
		}

		/// <summary>
		/// 初始化链接
		/// </summary>
		private void InitUrlFormat()
		{

			//_UrlFormat.Add(19, "http://chezhan.bitauto.com/beijing/2010/{0}/{1}/");
			//_UrlFormat.Add(48, "http://chezhan.bitauto.com/guangzhou-chezhan/2010/{0}/{1}/");
			//_UrlFormat.Add(59, "http://chezhan.bitauto.com/shanghai/2011/{0}/{1}/");
			//_UrlFormat.Add(71, "http://chezhan.bitauto.com/guangzhou-chezhan/2011/{0}/{1}/");
			//_PravilionUrlFormat.Add(59, "http://chezhan.bitauto.com/shanghai/zhanguan/{0}/");
			//_PravilionUrlFormat.Add(71, "http://chezhan.bitauto.com/guangzhou-chezhan/n2011/{0}/");

			//_VoteFormat.Add(19, "BitAuto.Chezhan.2010");
			//_VoteFormat.Add(48, "BitAuto.Chezhan.2010.guangzhou");
			//_VoteFormat.Add(59, "BitAuto.Chezhan.2011.shanghai.car");
			//_VoteFormat.Add(71, "BitAuto.Chezhan.2011.guangzhou.car");
			// BitAuto.Chezhan.2011.guangzhou.mod

			PavilionUrl = GetPavilionByExhibitionId();
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

		#region 车展新闻

		/// <summary>
		/// 得到车展新闻列表
		/// </summary>
		/// <param name="brandType"></param>
		/// <param name="brandId"></param>
		/// <returns></returns>
		protected Dictionary<string, ExhibitionNew> GetExhibitionNewsList(string brandType, int brandId, string SavePath)
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
		/// 得到子品牌List
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

		#endregion

		/// <summary>
		/// 初始化子品牌解析地址
		/// 新闻接口 提供:朱永旭 Nov.15.2011
		/// </summary>
		private void IntiSerialJieXiURL()
		{
			string cacheKey = "ExhibitionPageBase_IntiSerialJieXiURL";
			object intiSerialJieXiURL = CacheManager.GetCachedData(cacheKey);
			if (intiSerialJieXiURL == null)
			{
				XmlDocument doc = new XmlDocument();
				doc.Load("http://api.admin.bitauto.com/api/list/NewsToCar.aspx?categoryid=360&tags=71");
				if (doc != null && doc.HasChildNodes)
				{
					XmlNodeList xnl = doc.SelectNodes("/NewDataSet/listNews");
					if (xnl != null && xnl.Count > 0)
					{
						foreach (XmlNode xn in xnl)
						{
							int csid = 0;
							int.TryParse(xn.SelectSingleNode("relatedBrand").InnerText, out csid);
							string url = xn.SelectSingleNode("filepath").InnerText.Trim();
							if (csid > 0 && url != "")
							{
								if (!_DicSerialJieXi.ContainsKey(csid))
								{ _DicSerialJieXi.Add(csid, url); }
							}
						}
					}
				}
				CacheManager.InsertCache(cacheKey, _DicSerialJieXi, WebConfig.CachedDuration);
			}
			else
			{
				_DicSerialJieXi = (Dictionary<int, string>)intiSerialJieXiURL;
			}
		}

		/// <summary>
		/// 根据车展ID取展馆信息
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, string> GetPavilionByExhibitionId()
		{
			Dictionary<string, string> temp = new Dictionary<string, string>();
			string cacheKey = "ExhibitionPageBase_GetPavilionByExhibitionId_" + _ExhibitionID.ToString();
			object getPavilionByExhibitionId = CacheManager.GetCachedData(cacheKey);
			if (getPavilionByExhibitionId == null)
			{
				DataSet ds = BitAuto.CarChannel.BLL.Exhibition.GetAllPravilionByExhibitionId(_ExhibitionID);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						if (!temp.ContainsKey(ds.Tables[0].Rows[i]["name"].ToString().Trim()))
						{ temp.Add(ds.Tables[0].Rows[i]["name"].ToString().Trim(), Convert.ToString(i + 1)); }
					}
				}
				CacheManager.InsertCache(cacheKey, temp, WebConfig.CachedDuration);
			}
			else
			{
				temp = (Dictionary<string, string>)getPavilionByExhibitionId;
			}
			return temp;
		}

		/// <summary>
		/// 根据车展ID取属性
		/// </summary>
		private void BuilderAttributeUrlList()
		{
			string cacheKeyURL = "ExhibitionPageBase_BuilderAttributeUrlList_" + _ExhibitionID.ToString() + "_Url";
			object getAllAttributeByExhibitionIdURL = CacheManager.GetCachedData(cacheKeyURL);
			if (getAllAttributeByExhibitionIdURL == null)
			{
				DataSet ds = BitAuto.CarChannel.BLL.Exhibition.GetAllAttributeByExhibitionId(_ExhibitionID);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						if (!AttributeUrl.ContainsKey(ds.Tables[0].Rows[i]["name"].ToString().Trim()))
						{ AttributeUrl.Add(ds.Tables[0].Rows[i]["name"].ToString().Trim(), Convert.ToString(i + 1)); }
					}
				}
				CacheManager.InsertCache(cacheKeyURL, AttributeUrl, WebConfig.CachedDuration);
			}
			else
			{
				AttributeUrl = (Dictionary<string, string>)getAllAttributeByExhibitionIdURL;
			}

			string cacheKeyUrlReverse = "ExhibitionPageBase_BuilderAttributeUrlList_" + _ExhibitionID.ToString() + "_UrlReverse";
			object getAllAttributeByExhibitionIdUrlReverse = CacheManager.GetCachedData(cacheKeyUrlReverse);
			if (getAllAttributeByExhibitionIdUrlReverse == null)
			{
				DataSet ds = BitAuto.CarChannel.BLL.Exhibition.GetAllAttributeByExhibitionId(_ExhibitionID);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						if (!AttributeUrlReverse.ContainsKey(ds.Tables[0].Rows[i]["ID"].ToString().Trim()))
						{ AttributeUrlReverse.Add(ds.Tables[0].Rows[i]["ID"].ToString().Trim(), Convert.ToString(i + 1)); }
					}
				}
				CacheManager.InsertCache(cacheKeyUrlReverse, AttributeUrlReverse, WebConfig.CachedDuration);
			}
			else
			{
				AttributeUrlReverse = (Dictionary<string, string>)getAllAttributeByExhibitionIdUrlReverse;
			}
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
				if (_DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && _DicExhibitionBaseInfo[_ExhibitionID].UrlFormat != "")
				{
					serialUrl = string.Format(_DicExhibitionBaseInfo[_ExhibitionID].UrlFormat
								, masterSpell
								, serialSpell);
				}
				//serialUrl = string.Format(_UrlFormat[_ExhibitionID]
				//                , masterSpell
				//                , serialSpell);
			}
			else
			{
				serialUrl = string.Format("http://car.bitauto.com/{0}/", serialSpell);
			}
			return serialUrl;
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
		/// 得到级别名称
		/// </summary>
		/// <returns></returns>
		protected string GetLevelString(string allSpell)
		{
			if (allSpell == "haohuache")
			{
				return "豪华车";
			}
			else if (allSpell == "paoche")
			{
				return "跑车";
			}
			return allSpell.ToUpper();
		}

		/// <summary>
		/// 得到级别名称
		/// </summary>
		/// <returns></returns>
		protected List<string> GetLevelStringList(string allSpell)
		{
			List<string> list = new List<string>();
			if (allSpell != "")
			{
				string[] arrTemp = allSpell.Replace("'", "").Split('|');
				foreach (string level in arrTemp)
				{
					string name = level.ToLower();
					if (level == "haohuache")
					{
						name = "豪华车";
					}
					else if (level == "paoche")
					{
						name = "跑车";
					}

					if (name.Trim() != "" && !list.Contains(name.ToUpper()))
					{ list.Add(name.ToUpper()); }
				}
			}
			return list;
		}

		/// <summary>
		/// 得到投票的链接
		/// </summary>
		protected string GetUserVoteUrl()
		{
			string voteResultUrl = "";
			if (_DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && _DicExhibitionBaseInfo[_ExhibitionID].VoteCarFormat != "")
			{
				voteResultUrl = _DicExhibitionBaseInfo[_ExhibitionID].VoteCarFormat;
			}
			return voteResultUrl;
			// string voteResultUrl = "http://www.bitauto.com/vote/getCountList.aspx?akey={0}";
			//if (!_VoteFormat.ContainsKey(_ExhibitionID)) return "";
			//return string.Format(voteResultUrl, _VoteFormat[_ExhibitionID]);
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
		/// 得到所有子品牌官方指导价
		/// </summary>
		/// <returns></returns>
		private Dictionary<int, string> GetAllSerialOfficePrice()
		{
			string cacheKey = "exhibitionOfficePrice";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (Dictionary<int, string>)obj;
			DataSet ds = new Car_SerialDal().GetAllSerialOfficePrice(true);
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

		/// <summary>
		/// 根据车展ID取底
		/// </summary>
		/// <returns></returns>
		protected string GetExhibitionFooterByExhibitionId()
		{
			string cacheKey = "ExhibitionPageBase_GetExhibitionFooterByExhibitionId_" + _ExhibitionID.ToString();
			object getExhibitionFooterByExhibitionId = CacheManager.GetCachedData(cacheKey);
			if (getExhibitionFooterByExhibitionId == null)
			{
				string sPath = WebConfig.DataBlockPath + "Data\\Exhibition\\" + _ExhibitionID.ToString() + "\\Footer.htm";
				if (!File.Exists(sPath))
				{
					return "";
				}
				string tempContent = File.ReadAllText(sPath);
				if (tempContent == "")
				{ return ""; }
				// 展厅 对应品牌
				Dictionary<int, List<XmlNode>> dicBrand = new Dictionary<int, List<System.Xml.XmlNode>>();

				#region 取数据
				XmlDocument _ExhibitionXmlDoc = new BitAuto.CarChannel.BLL.Exhibition().GetExibitionXmlByExhibitionId(_ExhibitionID);
				if (_ExhibitionXmlDoc == null
					|| _ExhibitionXmlDoc.SelectSingleNode("root") == null
					|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
					|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
				{
					return "";
				}

				string brandXPath = "root/MasterBrand/Brand";
				XmlNodeList xnl = _ExhibitionXmlDoc.SelectNodes(brandXPath);
				if (xnl != null && xnl.Count > 0)
				{
					foreach (XmlNode xn in xnl)
					{
						int PavilionId = 0;
						if (xn.Attributes["PavilionId"] != null && int.TryParse(xn.Attributes["PavilionId"].Value, out PavilionId))
						{
							if (PavilionId > 0)
							{
								if (!dicBrand.ContainsKey(PavilionId))
								{
									List<XmlNode> list = new List<System.Xml.XmlNode>();
									list.Add(xn);
									dicBrand.Add(PavilionId, list);
								}
								else
								{
									if (!dicBrand[PavilionId].Contains(xn))
									{ dicBrand[PavilionId].Add(xn); }
								}
							}
						}

					}
				}

				#endregion

				// 展厅
				DataSet ds = BitAuto.CarChannel.BLL.Exhibition.GetAllPravilionByExhibitionId(_ExhibitionID);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					string url = "";
					if (_DicExhibitionBaseInfo.ContainsKey(_ExhibitionID) && _DicExhibitionBaseInfo[_ExhibitionID].PravilionUrlFormat != "")
					{ url = _DicExhibitionBaseInfo[_ExhibitionID].PravilionUrlFormat; }
					// url = _PravilionUrlFormat[_ExhibitionID];
					StringBuilder sb = new StringBuilder();
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						// 循环车展
						if ((i % 2) == 0)
						{
							sb.AppendLine("<tr class=\"linebg\">");
						}
						else
						{
							sb.AppendLine("<tr>");
						}
						string pavilionUrlTemp = string.Format(url, PavilionUrl[ds.Tables[0].Rows[i]["name"].ToString().Trim()]);
						sb.AppendLine("<th> <a target=\"_blank\" href=\"" + pavilionUrlTemp + "\">" + ds.Tables[0].Rows[i]["name"].ToString().Trim() + "馆&nbsp;&gt;</a> </th>");
						int pavilionId = int.Parse(ds.Tables[0].Rows[i]["id"].ToString().Trim());
						if (dicBrand.ContainsKey(pavilionId) && dicBrand[pavilionId].Count > 0)
						{
							sb.Append("<td>");
							int cbCount = 0;
							foreach (XmlNode xn in dicBrand[pavilionId])
							{
								if (cbCount > 0)
								{ sb.Append("|"); }
								sb.Append("<a target=\"_blank\" href=\"" + pavilionUrlTemp + xn.Attributes["ID"].Value.ToString() + "\">" + xn.Attributes["Name"].Value.ToString() + "</a>");
								cbCount++;
							}
							sb.Append("</td>");
						}
						sb.AppendLine("</tr>");
					}
					tempContent = tempContent.Replace("<!--展厅品牌列表-->", sb.ToString());
				}
				CacheManager.InsertCache(cacheKey, tempContent, WebConfig.CachedDuration);
				return tempContent;
			}
			else
			{
				return Convert.ToString(getExhibitionFooterByExhibitionId);
			}
		}

	}
}