using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Enum;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// 输出易车网首页 子品牌信息块 add by chengl Feb.1.2012
	/// 输入参数 子品牌ID，城市ID，jsonp callback，js var
	/// </summary>
	public partial class SerialJsonInfo : PageBase
	{

		#region member
		private List<int> listCsID = new List<int>();
		private int cityID = 0;
		private string callback = string.Empty;
		private List<string> jsonString = new List<string>();
		private string jsonVarName = string.Empty;
		private string memCacheTemp = "Car_List_SerialJsonInfo_{0}";
		private string memHangQingCacheTemp = "Car_List_SerialHangQingJsonInfo_{0}_{1}";
		private string memJiangJiaCacheTemp = "Car_List_SerialJiangJiaJsonInfo_{0}_{1}";
		private string memCuXiaoCacheTemp = "Car_List_SerialCuXiaoJsonInfo_{0}_{1}";
		private bool m_isCuXiao = false;
		private Dictionary<int, SerialForum> _SerialForumList;
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			this.Response.ContentType = "application/x-javascript";
			if (!this.IsPostBack)
			{
				GetPageParam();
				GetCsJsonData();
				ResponseJson();
			}
		}

		#region private Method

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{
			string strCsIDs = this.Request.QueryString["csIDs"];
			if (!string.IsNullOrEmpty(strCsIDs))
			{
				string[] csids = strCsIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (csids.Length > 0)
				{
					foreach (string csItem in csids)
					{
						int csid = 0;
						if (int.TryParse(csItem.Trim(), out  csid))
						{
							if (csid > 0 && !listCsID.Contains(csid))
							{ listCsID.Add(csid); }
						}
					}
				}
			}

			string strCityID = this.Request.QueryString["cityID"];
			if (!string.IsNullOrEmpty(strCityID) && int.TryParse(strCityID, out cityID))
			{ }
			if (cityID <= 0)
			{ cityID = 201; }

			callback = this.Request.QueryString["callback"];

			jsonVarName = this.Request.QueryString["jsonVarName"];

			if (!string.IsNullOrEmpty(this.Request.QueryString["cuxiao"]))
				m_isCuXiao = (this.Request.QueryString["cuxiao"].ToLower() == "true");
		}

		/// <summary>
		/// 取车型json
		/// </summary>
		private void GetCsJsonData()
		{
			if (listCsID.Count > 0)
			{
				#region base data
				// Dictionary<int, string> dicPic = base.GetAllSerialPicURL(true);
				// 白底
				Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();
				// 非白底
				Dictionary<int, string> dicPicNoneWhite = base.GetAllSerialPicURLNoneWhiteBackground();

				Dictionary<int, string> dicPingCeRainbow = base.GetAllPingCeNewsURLForCsPingCePage();

				// 所有有置换信息的子品牌ID
				// modified by chengl Jul.26.2012 高总需求所有子品牌开放置换
				// List<int> allZhiHuanCsID = new Car_SerialBll().GetAllZhiHuanCsID();
				#endregion

				// 1次取出所有memcache 的key集合
				IList<string> keyForMemCache = new List<string>();
				// 子品牌基本信息在memcache中的key
				string key = string.Empty;
				// 子品牌城市行情在memcache中的key
				string keyHangQing = string.Empty;
				// 子品牌城市降价在memcache中的key
				string keyJiangJia = string.Empty;
				// 子品牌城市促销在memcache中的key
				string keyCuXiao = string.Empty;

				// 循环生成 memcache key 1次取回
				foreach (int csid in listCsID)
				{
					key = string.Format(memCacheTemp, csid);
					keyHangQing = string.Format(memHangQingCacheTemp, csid, cityID);
					keyJiangJia = string.Format(memJiangJiaCacheTemp, csid, cityID); 

					keyForMemCache.Add(key);
					keyForMemCache.Add(keyHangQing);
					keyForMemCache.Add(keyJiangJia);

					if (m_isCuXiao)
					{
						keyCuXiao = string.Format(memCuXiaoCacheTemp, csid, cityID);
						keyForMemCache.Add(keyCuXiao);
					}
				}

				// 1次从memcache取数据
				IDictionary<string, object> dicMemCache = MemCache.GetMultipleMemCacheByKey(keyForMemCache);

				// 循环取回的缓存
				foreach (int csid in listCsID)
				{
					List<string> listCs = new List<string>(55);
					List<string> listHangQing = new List<string>(10);
					List<string> listJiangJia = new List<string>(10);

					key = string.Format(memCacheTemp, csid);
					keyHangQing = string.Format(memHangQingCacheTemp, csid, cityID);
					keyJiangJia = string.Format(memJiangJiaCacheTemp, csid, cityID); 

					#region 子品牌基本信息
					// 子品牌基本信息
					if (dicMemCache != null
						&& dicMemCache.Count > 0
						&& dicMemCache.ContainsKey(key)
						&& dicMemCache[key] != null)
					{
						// 有此子品牌基本信息
						listCs = dicMemCache[key] as List<string>;
					}
					else
					{
						CarNewsBll newsBll = new CarNewsBll();
						// 无此子品牌基本信息
						SerialEntity se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csid);
						if (se != null && se.Id > 0)
						{
							if (_SerialForumList == null)
								_SerialForumList = BaaCarBrandToForum.GetSerialForumList();

							#region 子品牌基本信息
							listCs.Add("CsID:\"" + se.Id + "\",");
							listCs.Add("CsName:\"" + UrlEncodeUnicode(se.Name) + "\",");
							listCs.Add("CsShowName:\"" + UrlEncodeUnicode(se.ShowName) + "\",");
							listCs.Add("CsAllSpell:\"" + se.AllSpell + "\",");
							// modified by chengl Mar.5.2012
							// 子品牌封面
							if (dicPicWhite.ContainsKey(se.Id))
							{ listCs.Add("CsImg:\"" + dicPicWhite[se.Id].Replace("_2.", "_3.") + "\","); }
							else if (dicPicNoneWhite.ContainsKey(se.Id))
							{ listCs.Add("CsImg:\"" + dicPicNoneWhite[se.Id].Replace("_2.", "_3.").Replace("/autoalbum/", "/wapimg-210-9999/autoalbum/") + "\","); }
							else
							{ listCs.Add("CsImg:\"" + WebConfig.DefaultCarPic + "\","); }

							//if (dicPic.ContainsKey(se.Id))
							//{ listCs.Add("CsImg:\"" + dicPic[se.Id].Replace("_2.", "_3.") + "\","); }
							//else
							//{ listCs.Add("CsImg:\"" + WebConfig.DefaultCarPic + "\","); }

							// 报价区间换成指导价区间
							listCs.Add("CsPrice:\"" + UrlEncodeUnicode(se.ReferPrice) + "\",");
							// listCs.Add("CsPrice:\"" + UrlEncodeUnicode(GetSerialPriceRangeByID(se.Id)) + "\",");
							#endregion

							#region 子品牌新闻数量
							int newsCount = GetNewsCount(se.Id);
							listCs.Add("CsNewsCount:\"" + newsCount.ToString() + "\",");
							listCs.Add("CsForumCount:\"" + (_SerialForumList.ContainsKey(se.Id) ? _SerialForumList[se.Id].TopicCount : 0) + "\",");
							listCs.Add("CsHangQingCount:\"" + newsBll.GetSerialCityNewsCount(se.Id,0).ToString() + "\",");
							SerialJiangJiaNewsSummary serialJiangJiaNewsSummary = newsBll.GetSerialJiangJiaNewsSummary(se.Id, 0);
							if (serialJiangJiaNewsSummary != null)
							{
								listCs.Add("CsJiangJiaCount:\"" + serialJiangJiaNewsSummary.CarNum + "\",");
								listCs.Add("CsJiangJiaMaxPrice:\"" + serialJiangJiaNewsSummary.MaxFavorablePrice.ToString("0.##") + "\",");
							}
							else
							{
								listCs.Add("CsJiangJiaCount:\"0\",");
								listCs.Add("CsJiangJiaMaxPrice:\"0\",");
							}
							#endregion

							#region 子品牌各个标签link
							// 子品牌各个标签link
							listCs.Add("CsLink:{");
							listCs.Add("PeiZhi:\"http://car.bitauto.com/" + se.AllSpell + "/peizhi/\",");
							listCs.Add("TuPian:\"http://car.bitauto.com/" + se.AllSpell + "/tupian/\",");
							listCs.Add("BaoJia:\"http://car.bitauto.com/" + se.AllSpell + "/baojia/\",");
							if (dicPingCeRainbow.ContainsKey(se.Id))
							{ listCs.Add("PingCe:\"http://car.bitauto.com/" + se.AllSpell + "/pingce/\","); }
							else
							{ listCs.Add("PingCe:\"http://car.bitauto.com/" + se.AllSpell + "/shijia/\","); }
							listCs.Add("YouHao:\"http://car.bitauto.com/" + se.AllSpell + "/youhao/\",");
							listCs.Add("XunJia:\"http://car.bitauto.com/" + se.AllSpell + "/baojia/\",");
							listCs.Add("GouMai:\"http://i.bitauto.com/baaadmin/car/goumai_" + se.Id.ToString() + "/\",");
							// add by chengl Mar.14.2012
							listCs.Add("BBS:\"" + (new Car_SerialBll()).GetForumUrlBySerialId(se.Id) + "\",");
							// add by chengl Jul.18.2012
							// modified by chengl Jul.26.2012 高总需求所有子品牌开放置换
							listCs.Add("ZhiHuan:\"http://car.bitauto.com/" + se.AllSpell + "/zhihuan/\","); 
							//if (allZhiHuanCsID.Contains(se.Id))
							//{ listCs.Add("ZhiHuan:\"http://car.bitauto.com/" + se.AllSpell + "/zhihuan/\","); }
							//else
							//{ listCs.Add("ZhiHuan:\"\","); }
							//listCs.Add("Ucar:\"http://yiche.taoche.com/buycar/b-" + se.AllSpell + "/\"");
							listCs.Add("Ucar:\"http://yiche.taoche.com/similarcar/serial/" + se.AllSpell + "/paesf0bxc/?from=bitauto\"");
							listCs.Add("},");
							#endregion

							#region 新闻资讯
							// 新闻资讯
							listCs.Add("CsNewsLink:[");
							List<int> listint = new List<int>();
							// 焦点3条新闻 数据源变更
							// modified by chengl Apr.6.2012
							string focusNewsPathNew = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\SerialNews\\FocusNews\\homexml\\{0}.xml", se.Id));
							// string focusNewsPath = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\SerialNews\\FocusNews\\XML\\Serial_FocusNews_{0}.xml", se.Id));
							List<News> listNews = new Car_SerialBll().GetFocusNewsListForCMS(3, focusNewsPathNew);
							if (listNews != null && listNews.Count > 0)
							{
								int loop = 1;
								foreach (News news in listNews)
								{
									if (loop > 1)
									{ listCs.Add(","); }
									listCs.Add("{");
									listCs.Add("Title:\"" + UrlEncodeUnicode(news.Title.Replace("\"", "'")) + "\",");
									listCs.Add("FaceTitle:\"" + UrlEncodeUnicode(news.FaceTitle.Replace("\"", "'")) + "\",");
									listCs.Add("Url:\"" + news.PageUrl + "\"");
									listCs.Add("}");
									loop++;
									if (loop > 3)
									{ break; }
								}
							}
							listCs.Add("],");
							#endregion

							#region 论坛热贴

							// 论坛热贴
							XmlDocument xmlFocusNews = new Car_SerialBll().GetSerialFocusNews(se.Id);

							// 论坛热贴
							listCs.Add("CsForumLink:[");
							XmlNodeList newsForumList = xmlFocusNews.SelectNodes("/root/Forum/ForumSubject");
							if (newsForumList != null && newsForumList.Count > 0)
							{
								int loop = 1;
								foreach (XmlElement newsForumNode in newsForumList)
								{
									if (loop > 1)
									{ listCs.Add(","); }
									string newsTitle = newsForumNode.SelectSingleNode("title").InnerText.Trim().Replace("\"", "'");
									string filePath = newsForumNode.SelectSingleNode("url").InnerText;
									listCs.Add("{Title:\"" + UrlEncodeUnicode(newsTitle) + "\",");
									listCs.Add("Url:\"" + filePath + "\"}");
									loop++;
									if (loop > 3)
									{ break; }
								}
							}
							listCs.Add("],");
							#endregion

						}
						// 加入memcache
						MemCache.SetMemCacheByKey(key, listCs, 60 * 60 * 1000);
					}

					#endregion

					#region 子品牌城市行情
					// 子品牌基本信息
					if (dicMemCache != null
						&& dicMemCache.Count > 0
						&& dicMemCache.ContainsKey(keyHangQing)
						&& dicMemCache[keyHangQing] != null)
					{
						// 有缓存
						listHangQing = dicMemCache[keyHangQing] as List<string>;
					}
					else
					{
						// 无缓存
						// 行情报价
						SerialEntity se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csid);
						listHangQing.Add("CsHangQingLink:[");
						if (se != null && se.Id > 0)
						{
							List<News> newsHangQingList = null;
							Dictionary<int, int> parentCityList = CommonFunction.GetCityRelationParentDic();
							if (parentCityList.ContainsKey(cityID) && cityID != parentCityList[cityID])
							{
								//newsHangQingList = new Car_SerialBll().GetCityHangQingNewsList(se.Id, cityID, parentCityList[cityID]);
								newsHangQingList = new CarNewsBll().GetTopCityNews2(se.Id, cityID, parentCityList[cityID],2);
							}
							else
							{
								//newsHangQingList = new Car_SerialBll().GetCityHangQingNewsList(se.Id, cityID);
								newsHangQingList = new CarNewsBll().GetTopCityNews2(se.Id, cityID,2);
							}
							if (newsHangQingList != null && newsHangQingList.Count > 0)
							{
								int loop = 1;
								foreach (News entity in newsHangQingList)
								{
									if (loop > 1)
									{ listHangQing.Add(","); }
									listHangQing.Add("{Title:\"" + UrlEncodeUnicode(entity.Title) + "\",");
									listHangQing.Add("FaceTitle:\"" + UrlEncodeUnicode(entity.FaceTitle) + "\",");
									listHangQing.Add("Url:\"" + entity.PageUrl + "\"}");
									loop++;
									if (loop > 2)
									{ break; }
								}
							}
						}
						listHangQing.Add("],");
						// 加入memcache
						MemCache.SetMemCacheByKey(keyHangQing, listHangQing, 60 * 60 * 1000);
					}
					#endregion

					#region 子品牌城市降价
					// 子品牌基本信息
					if (dicMemCache != null
						&& dicMemCache.Count > 0
						&& dicMemCache.ContainsKey(keyJiangJia)
						&& dicMemCache[keyJiangJia] != null)
					{
						// 有缓存
						listJiangJia = dicMemCache[keyJiangJia] as List<string>;
					}
					else
					{
						// 无缓存
						// 降价新闻
						SerialEntity se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csid);
						listJiangJia.Add("CsJiangJiaLink:[");
						if (se != null && se.Id > 0)
						{
							List<News> newsJiangJiaList = null;
							Dictionary<int, int> parentCityList = CommonFunction.GetCityRelationParentDic();
							if (parentCityList.ContainsKey(cityID) && cityID != parentCityList[cityID])
							{
								newsJiangJiaList = new CarNewsBll().GetSerialJiangJiaTopNews(se.Id, cityID, parentCityList[cityID], 2, 3);
							}
							else
							{
								newsJiangJiaList = new CarNewsBll().GetSerialJiangJiaTopNews(se.Id, cityID, 2, 3);
							}
							if (newsJiangJiaList != null && newsJiangJiaList.Count > 0)
							{
								int loop = 1;
								foreach (News entity in newsJiangJiaList)
								{
									if (loop > 1)
									{ listJiangJia.Add(","); }
									listJiangJia.Add("{Title:\"" + UrlEncodeUnicode(entity.Title) + "\",");
									listJiangJia.Add("Url:\"" + entity.PageUrl + "\"}");
									loop++;
									if (loop > 2)
									{ break; }
								}
							}
						}
						listJiangJia.Add("],");
						// 加入memcache
						MemCache.SetMemCacheByKey(keyJiangJia, listJiangJia, 60 * 60 * 1000);
					}
					#endregion

					#region 子品牌城市促销
					if (m_isCuXiao)
					{
						keyCuXiao = string.Format(memCuXiaoCacheTemp, csid, cityID);
						List<string> listCuXiao = new List<string>(10);
						if (dicMemCache != null
							&& dicMemCache.Count > 0
							&& dicMemCache.ContainsKey(keyCuXiao)
							&& dicMemCache[keyCuXiao] != null)
						{
							// 有缓存
							listCuXiao = dicMemCache[keyCuXiao] as List<string>;
						}
						else
						{
							// 无缓存
							// 促销报价
							Car_SerialBll serialBll = new Car_SerialBll();
							SerialEntity se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csid);
							int cuxiaoCount = 0;
							if (se != null && se.Id > 0)
							{
								cuxiaoCount = serialBll.GetCuXiaoNewsCount(se.Id, 0);
							}
							listCuXiao.Add(string.Format("CsCuXiaoCount:\"{0}\",", cuxiaoCount.ToString()));
							listCuXiao.Add("CsCuXiaoLink:[");
							if (se != null && se.Id > 0)
							{
								List<News> newsCuXiaoList = serialBll.GetSerialCityNews(se.Id, cityID, "cuxiao");
								if (newsCuXiaoList != null && newsCuXiaoList.Count > 0)
								{
									int loop = 1;
									foreach (News entity in newsCuXiaoList)
									{
										if (loop > 1)
										{ listCuXiao.Add(","); }
										listCuXiao.Add("{Title:\"" + UrlEncodeUnicode(entity.Title) + "\",");
										listCuXiao.Add("Url:\"" + entity.PageUrl + "\"}");
										loop++;
										if (loop > 2)
										{ break; }
									}
								}
							}
							listCuXiao.Add("],");
							// 加入memcache
							MemCache.SetMemCacheByKey(keyCuXiao, listCuXiao, 60 * 60 * 1000);
						}
						if (listCs.Count > 0)
						{
							listCs.Add(string.Concat(listCuXiao.ToArray()));
						}
					}
					#endregion

					if (listCs.Count > 0)
					{
						listCs.Insert(0, "{");
						listCs.Add(string.Concat(listHangQing.ToArray()));
						listCs.Add(string.Concat(listJiangJia.ToArray()));
						listCs.Add("Other:\"\"}");
						if (jsonString.Count > 0)
						{ jsonString.Add(","); }
						jsonString.Add(string.Concat(listCs.ToArray()));
					}
				}

			}
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

		/// <summary>
		/// 输出
		/// </summary>
		private void ResponseJson()
		{
			if (jsonString.Count > 0)
			{
				jsonString.Insert(0, "[");
				jsonString.Add("]");

				// JsonP
				if (!string.IsNullOrEmpty(callback))
				{
					jsonString.Insert(0, callback + "(");
					jsonString.Add(")");
				}
				else if (!string.IsNullOrEmpty(jsonVarName))
				{
					// 非jsonP
					jsonString.Insert(0, "var " + jsonVarName + " = ");
					jsonString.Add(";");
				}
				else
				{ }
			}
			else
			{
				// JsonP
				if (!string.IsNullOrEmpty(callback))
				{
					jsonString.Add(callback + "({})");
				}
				else if (!string.IsNullOrEmpty(jsonVarName))
				{
					// 非jsonP
					jsonString.Add("var " + jsonVarName + " = null;");
				}
				else
				{ jsonString.Add("参数错误"); }
			}
			Response.Write(string.Concat(jsonString.ToArray()));
		}

		private string UrlEncodeUnicode(string sor)
		{
			if (sor == null)
			{ return ""; }
			string temp = HttpUtility.UrlEncodeUnicode(sor);
			if (temp.IndexOf("+") >= 0)
			{ temp = temp.Replace("+", "%20"); }
			return temp;
		}

		#endregion

	}
}