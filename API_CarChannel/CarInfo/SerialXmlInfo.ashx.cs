using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using System.Xml.Linq;
using BitAuto.CarChannel.Model;
using System.IO;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// SerialXmlInfo 的摘要说明
	/// modified by chengl Oct.10.2015 for 李东 接口增加车款数据，增加车系销售状态，去掉所有新闻和降价新闻
	/// </summary>
	public class SerialXmlInfo : IHttpHandler
	{
		private int _SerialId = 0;
		private int _JingPinTop = 2;
		private bool _GetNew = false;
		private HttpRequest _request;
		private HttpResponse _response;

		/// <summary>
		/// 是否第2版 for 李东，去掉新闻、增加车款数据和车系的销售状态
		/// </summary>
		private bool isV2 = false;

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(30);
			_response = context.Response;
			_request = context.Request;
			context.Response.ContentType = "text/xml";

			GetPageParam();
			WriteXml();
		}

		private void WriteXml()
		{
			if (_SerialId < 1)
				return;
			string memCacheKey = "Car_List_SerialXmlInfo_" + _SerialId + "_" + _JingPinTop + (_GetNew ? "_New" : string.Empty);
			if (isV2)
			{
				memCacheKey = "Car_List_SerialXmlInfoV2_" + _SerialId;
			}
			string data = MemCache.GetMemCacheByKey(memCacheKey) as string;
			if (string.IsNullOrWhiteSpace(data))
			{
				data = GetDate();
				if (!string.IsNullOrWhiteSpace(data))
				{
					MemCache.SetMemCacheByKey(memCacheKey, data, 60 * 60 * 1000);
				}
			}
			_response.Write(data);
		}

		//备注：cms 李东那边是使用dataset.readxml方式使用，开发时注意 ,现在调用的固定isV2=1，新闻和口碑相关不再从该接口出数据
		private string GetDate()
		{
			XDocument doc = new XDocument();

			// 无此子品牌基本信息
			SerialEntity se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, _SerialId);
			if (se != null && se.Id > 0)
			{
				PageBase pageBase = new PageBase();
				CarNewsBll newsBll = new CarNewsBll();
				Car_SerialBll serialBll = new Car_SerialBll();

				Dictionary<int, string> dicPicWhite = pageBase.GetAllSerialPicURLWhiteBackground();
				// 非白底
				Dictionary<int, string> dicPicNoneWhite = pageBase.GetAllSerialPicURLNoneWhiteBackground();

				Dictionary<int, string> dicPingCeRainbow = pageBase.GetAllPingCeNewsURLForCsPingCePage();

				#region 子品牌基本信息

				XElement root = new XElement("Serial");
				doc.Add(root);

				root.SetAttributeValue("Id", se.Id);
				root.SetAttributeValue("Name", se.Name);
				root.SetAttributeValue("ShowName", se.ShowName);
				root.SetAttributeValue("AllSpell", se.AllSpell);
				// 子品牌封面
				string imgUrl = string.Empty;
				if (dicPicWhite.ContainsKey(se.Id))
				{ imgUrl = dicPicWhite[se.Id].Replace("_2.", "_3."); }
				else if (dicPicNoneWhite.ContainsKey(se.Id))
				{ imgUrl = dicPicNoneWhite[se.Id].Replace("_2.", "_3.").Replace("/autoalbum/", "/wapimg-210-9999/autoalbum/"); }
				else
				{ imgUrl = WebConfig.DefaultCarPic; }
				root.SetAttributeValue("Img", imgUrl);

				// 报价区间换成指导价区间
				root.SetAttributeValue("RefPrice", se.ReferPrice);
				// 参考成交价
				root.SetAttributeValue("Price", se.Price);

				//排量
				if (se.ExhaustList != null && se.ExhaustList.Length > 0)
				{
					root.SetAttributeValue("Exhaust",
						(se.ExhaustList.Length <= 1 ? se.ExhaustList[0] : (string.Format("{0}-{1}", se.ExhaustList[0], se.ExhaustList[se.ExhaustList.Length - 1]))));
				}
				else
				{
					root.SetAttributeValue("Exhaust", string.Empty);
				}
				// 车系销售状态
				root.SetAttributeValue("SaleState", se.SaleState);

				#endregion

				#region 子品牌新闻数量
				int newsCount = GetNewsCount(se.Id);
				root.SetAttributeValue("NewsCount", GetNewsCount(se.Id));

				SerialJiangJiaNewsSummary serialJiangJiaNewsSummary = newsBll.GetSerialJiangJiaNewsSummary(se.Id, 0);
				if (serialJiangJiaNewsSummary != null)
				{
					root.SetAttributeValue("JiangJiaCount", serialJiangJiaNewsSummary.CarNum);
					root.SetAttributeValue("JiangJiaMaxPrice", serialJiangJiaNewsSummary.MaxFavorablePrice.ToString("0.##"));
				}
				else
				{
					root.SetAttributeValue("JiangJiaCount", 0);
					root.SetAttributeValue("JiangJiaMaxPrice", 0);
				}
				#endregion

				#region 子品牌各个标签link
				XElement linksEle = new XElement("Links");
				root.Add(linksEle);

				// 子品牌各个标签link
				linksEle.Add(
					new XElement("PeiZhi", string.Format("http://car.bitauto.com/{0}/peizhi/", se.AllSpell))
					, new XElement("TuPian", string.Format("http://car.bitauto.com/{0}/tupian/", se.AllSpell))
					, new XElement("BaoJia", string.Format("http://car.bitauto.com/{0}/baojia/", se.AllSpell))
					, new XElement("PingCe", string.Format(
						(dicPingCeRainbow.ContainsKey(se.Id) ? "http://car.bitauto.com/{0}/pingce/" : "http://car.bitauto.com/{0}/shijia/")
						, se.AllSpell))
					, new XElement("YouHao", string.Format("http://car.bitauto.com/{0}/youhao/", se.AllSpell))
					, new XElement("XunJia", string.Format("http://car.bitauto.com/{0}/baojia/", se.AllSpell))
					, new XElement("GouMai", string.Format("http://i.bitauto.com/baaadmin/car/goumai_{0}/", se.Id))
					, new XElement("BBS", (new Car_SerialBll()).GetForumUrlBySerialId(se.Id))
					, new XElement("ZhiHuan", string.Format("http://car.bitauto.com/{0}/zhihuan/", se.AllSpell))
					, new XElement("Ucar", string.Format("http://yiche.taoche.com/similarcar/serial/{0}/paesf0bxc/?from=bitauto", se.AllSpell))
				);
				#endregion

				//备注：cms 李东那边是使用dataset.readxml方式使用，开发时注意
				if (isV2)
				{
					// v2 版本增加车款数据 去掉新闻数据
					DataSet ds = new Car_BasicBll().GetCarBaseDataBySerialId(_SerialId, true);
					if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
					{
						foreach (DataRow dr in ds.Tables[0].Rows)
						{
							XElement carsEle = new XElement("CarList");
							root.Add(carsEle);
							carsEle.Add(
								new XElement("CarID", dr["Car_Id"].ToString()),
								new XElement("CarName", dr["Car_Name"].ToString()),
								new XElement("Year", dr["Car_YearType"].ToString()),
								new XElement("ReferPrice", dr["car_ReferPrice"].ToString()),
								new XElement("SaleState", dr["Car_SaleState"].ToString()),
								new XElement("ProduceState", dr["Car_ProduceState"].ToString())
								);
						}
					}
				}
				else
				{
					#region 新闻、导购、试驾、评测

					#region 新闻

					string focusNewsPathNew = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\SerialNews\\FocusNews\\homexml\\{0}.xml", se.Id));
					List<News> listNews = new Car_SerialBll().GetFocusNewsListForCMS(4, focusNewsPathNew);
					if (listNews != null && listNews.Count > 0)
					{
						foreach (News news in listNews)
						{
							root.Add(
								new XElement("News",
									new XElement("Title", ReplaceLowOrderASCIICharacters(news.Title))
									, new XElement("FaceTitle", ReplaceLowOrderASCIICharacters(news.FaceTitle))
									, new XElement("Url", news.PageUrl)
									, new XElement("NewsType", "news")
								)
							);
						}
					}
					#endregion

					#region 试驾、导购
					Dictionary<CarNewsType, List<News>> newsList = newsBll.GetTopSerialNewsByCarNewsTypes(_SerialId, 1, new List<CarNewsType>() { CarNewsType.daogou, CarNewsType.shijia });
					if (newsList != null && newsList.Count > 0)
					{
						foreach (var newsData in newsList)
						{
							foreach (News news in newsData.Value)
							{
								root.Add(
									new XElement("News",
										new XElement("Title", ReplaceLowOrderASCIICharacters(news.Title))
										, new XElement("FaceTitle", ReplaceLowOrderASCIICharacters(news.FaceTitle))
										, new XElement("Url", news.PageUrl)
										, new XElement("NewsType", newsData.Key.ToString())
									)
								);
							}
						}
					}
					#endregion

					#region 评测
					News pingceNews = serialBll.GetSerialPingCeTitleNews(_SerialId);
					if (pingceNews != null)
					{
						root.Add(
							new XElement("News",
								new XElement("Title", ReplaceLowOrderASCIICharacters(pingceNews.Title))
								, new XElement("FaceTitle", ReplaceLowOrderASCIICharacters(pingceNews.FaceTitle))
								, new XElement("Url", pingceNews.PageUrl)
								, new XElement("NewsType", "pingce")
							)
						);
					}
					#endregion

					#endregion

					#region 口碑报告
					News koubeiReportNews = serialBll.GetSerialKouBeiReport(_SerialId);
					if (koubeiReportNews != null)
					{
						root.Add(new XElement("News",
							new XElement("Title", ReplaceLowOrderASCIICharacters(koubeiReportNews.Title))
							, new XElement("Url", koubeiReportNews.PageUrl)
							, new XElement("NewsType", "koubeireport")
							)
						);
					}
					#endregion

					if (_GetNew)
					{
						#region 当前子品牌降价新闻，每城市_JingPinTop条
						if (_JingPinTop > 0)
						{
							List<News> jiangJiaNewsList = newsBll.GetJiangJiaNewsByEveryCityTop(_SerialId.ToString(), _JingPinTop);
							if (jiangJiaNewsList != null && jiangJiaNewsList.Count > 0)
							{
								foreach (News jiangJiaNews in jiangJiaNewsList)
								{
									root.Add(new XElement("JiangJiaNews",
										new XElement("Title", ReplaceLowOrderASCIICharacters(jiangJiaNews.Title))
										, new XElement("Url", jiangJiaNews.PageUrl)
										, new XElement("CityId", jiangJiaNews.CityId)
										)
									);
								}
							}
						}
						#endregion
					}
					else
					{
						#region 竞品降价新闻，每城市2条
						if (_JingPinTop > 0)
						{
							List<EnumCollection.SerialToSerial> jingpinList = pageBase.GetSerialToSerialByCsID(_SerialId, 10);
							if (jingpinList != null && jingpinList.Count > 0)
							{

								List<News> jiangJiaNewsList = newsBll.GetJiangJiaNewsByEveryCityTop(GetSerialIds(jingpinList), _JingPinTop);
								if (jiangJiaNewsList != null && jiangJiaNewsList.Count > 0)
								{
									foreach (News jiangJiaNews in jiangJiaNewsList)
									{
										root.Add(new XElement("JiangJiaNews",
											new XElement("Title", ReplaceLowOrderASCIICharacters(jiangJiaNews.Title))
											, new XElement("Url", jiangJiaNews.PageUrl)
											, new XElement("CityId", jiangJiaNews.CityId)
											, new XElement("JingPinId", jiangJiaNews.RelatedMainSerialID)
											)
										);
									}
								}
							}
						}
						#endregion
					}
				}
			}

			return doc.ToString();
		}

		private string GetSerialIds(List<EnumCollection.SerialToSerial> jingpinList)
		{
			string[] serialIds = new string[jingpinList.Count];
			for (int i = 0; i < jingpinList.Count; i++)
			{
				serialIds[i] = jingpinList[i].ToCsID.ToString();
			}
			return string.Join(",", serialIds);
		}

		public static string ReplaceLowOrderASCIICharacters(string tmp)
		{
			System.Text.StringBuilder info = new System.Text.StringBuilder();
			foreach (char cc in tmp)
			{
				int ss = (int)cc;
				if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
					info.AppendFormat(" ", ss);//&#x{0:X};
				else info.Append(cc);
			}
			return info.ToString();
		}

		/// <summary>
		/// 获取咨询数
		/// </summary>
		/// <returns></returns>
		private int GetNewsCount(int serialId)
		{
			int newsCount = 0;
			Dictionary<string, Dictionary<int, int>> newsNumber = AutoStorageService.GetCacheTreeSerialNewsCount();
			if (newsNumber != null && newsNumber.Count > 0)
			{
				//易车评测 体验试驾 导购 用车 改装 安全 新闻
				string[] types = new string[]{
							CarNewsType.pingce.ToString(),
							CarNewsType.shijia.ToString(),
							CarNewsType.daogou.ToString(),
							CarNewsType.yongche.ToString(),
							CarNewsType.gaizhuang.ToString(),
							CarNewsType.anquan.ToString(),
							CarNewsType.xinwen.ToString()
						};

				foreach (string type in types)
				{
					if (newsNumber.ContainsKey(type) && newsNumber[type].ContainsKey(serialId))
					{
						newsCount += newsNumber[type][serialId];
					}
				}
			}
			return newsCount;
		}
		private void GetPageParam()
		{
			_SerialId = ConvertHelper.GetInteger(_request.QueryString["csid"]);
			// modified by chengl 默认值2
			_JingPinTop = (ConvertHelper.GetInteger(_request.QueryString["jptop"]) <= 0 ?
				2 : ConvertHelper.GetInteger(_request.QueryString["jptop"]));

			if (!string.IsNullOrWhiteSpace(_request.QueryString["new"])
				&& _request.QueryString["new"] == "1")
				_GetNew = true;

			if (!string.IsNullOrEmpty(_request.QueryString["isV2"])
				&& _request.QueryString["isV2"] == "1")
			{
				isV2 = true;
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}