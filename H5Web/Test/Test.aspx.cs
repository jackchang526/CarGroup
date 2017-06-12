using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace H5Web.Test
{
	public partial class Test : H5PageBase
	{
		#region Params

		protected bool IsExistColor = false;
		protected bool IsExistAppearance = false;
		protected bool IsExistInner = false;
		protected bool IsExistPingCe = false;
		protected bool IsExistPeizhi = false;
		protected bool IsExistKouBei = false;
		protected bool IsExistBaoJia = false;
		protected bool IsExistAttention = false;



		protected int serialId = 0;
		// 经纪人ID
		protected int brokerid = 0;
		// private string brokerinterface = "http://api.bitcar.com/api/GetBrokerInfo?brokerid={0}";
		protected string brokerHtml = string.Empty;
		// 经销商ID
		protected int dealerid = 0;
		/// <summary>
		/// 经销商接口 http://m.h5.qiche4s.cn/fourth/h5info/{method}/{dealerid}/{csid}/
		/// 车款列表 carsprice 
		/// 推荐车型 DealerCarReference 
		/// 经销商新闻 DealerNews
		/// 经销商信息 dealerinfo
		/// </summary>
		// private string dealerinterface = "http://m.h5.qiche4s.cn/fourth/h5info/{0}/{1}/{2}/";

		protected string dealerCarsPriceHtml = string.Empty;
		protected string dealerDealerCarReferenceHtml = string.Empty;
		protected string dealerDealerNewsHtml = string.Empty;
		protected string dealerDealerinfoHtml = string.Empty;

		/// <summary>
		/// 经销商接口类型
		/// </summary>
		public struct DealerInterfaceType
		{
			public static string carsprice = "carsprice";
			public static string dealercarreference = "dealercarreference";
			public static string dealernews = "dealernews";
			public static string dealerinfo = "dealerinfo";
		}

		protected SerialEntity BaseSerialEntity;

		// protected Car_SerialEntity SerialBrandEntity;

		protected List<SerialColorForSummaryEntity> SerialColorList;
		protected string SerialColorsJson;
		protected List<SerialFourthStage> ExteriorList;
		protected List<SerialFourthStage> InteriorList;
		protected List<News> SerialNewsList;
		protected string WriterKoubei;
		protected List<EnumCollection.SerialToSerial> AttentionSerials;
		protected List<CarInfoForSerialSummaryEntity> CarModelList;
		protected int CarModelWithPriceCount = 0;
		protected List<SerialFocusImage> ExteriorImageList;
		protected List<SerialFocusImage> InteriorImageList;
		protected List<SerialSparkle> SerialSparkleList;

		// protected List<News> CommerceNewsList;
		// protected Dictionary<int, string> dictSelectSerial; 
		protected List<int> listAllCsID = new List<int>();
		protected Dictionary<int, string> CarParamDictionary;

		protected string KoubeiHtml = string.Empty;
		protected string PeiZhiHtml = string.Empty;

		/// <summary>
		/// 如果有经销商或者经济人 则优惠不显示
		/// </summary>
		protected bool isShowYouHui = true;

		/// <summary>
		/// 经销商、经纪人接口
		/// </summary>
		private Dictionary<string, string> dicForServiceinterface = null;

		#endregion

		#region Services
		private Car_SerialBll carSerialBll = new Car_SerialBll();

		private SerialFourthStageBll serialFourthStageBll = new SerialFourthStageBll();

		private Car_BasicBll _carBLL = new Car_BasicBll();

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
			GetParamter();
			InitData();
			#region 验证访问权限
			// dictSelectSerial = GetDictSelectSerial();
			listAllCsID = serialFourthStageBll.GetAllSerialInH5();
			if (listAllCsID == null || BaseSerialEntity == null)
			{
				Response.Redirect("http://car.h5.yiche.com/");
			}
			if (!listAllCsID.Contains(serialId) && serialId > 0)
			{
				Response.Redirect("http://car.m.yiche.com/" + BaseSerialEntity.AllSpell);
			}
			#endregion
			GetDealerAllInfo("dealer");
			GetBrokerHtml("broker");
			InitColorList();//初始化颜色列表
			InitExteriorInteriorList();//初始化外观内饰设计数据
			InitNews();//初始化新闻列表
			InitKoubei();//编辑口碑
			InitAttentions();//关注的车型
			InitPrice();//车款报价
			// InitCommerceNews();//商配文章
			InitSerialSparkle();//亮点配置
		}

		private void GetParamter()
		{
			serialId = ConvertHelper.GetInteger(Request.QueryString["csID"]);
			brokerid = ConvertHelper.GetInteger(Request.QueryString["brokerid"]);
			dealerid = ConvertHelper.GetInteger(Request.QueryString["dealerid"]);
			if (brokerid > 0 || dealerid > 0)
			{
				dicForServiceinterface = base.GetH5ServiceDic();
			}
		}

		private void InitData()
		{
			BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
			// del by chengl 2015-6-30
			// SerialBrandEntity = carSerialBll.GetSerialInfoEntity(serialId);
		}

		/// <summary>
		/// 取经销商数据
		/// </summary>
		private void GetDealerAllInfo(string serviceName)
		{
			if (dealerid > 0 && dicForServiceinterface != null && dicForServiceinterface.ContainsKey(serviceName))
			{
				//接口类型_经销商ID_子品牌ID
				string cacheTemp = "Car_H5_Dealer_{0}_{1}_{2}";
				// 经销商车款列表
				dealerCarsPriceHtml = GetStringFromWebInterface(string.Format(dicForServiceinterface[serviceName], DealerInterfaceType.carsprice, dealerid, serialId)
					, string.Format(cacheTemp, DealerInterfaceType.carsprice, dealerid, serialId));
				// 如果有经销商ID 并且
				if (!string.IsNullOrEmpty(dealerCarsPriceHtml))
				{ IsExistBaoJia = true; }

				// 经销商推荐车
				dealerDealerCarReferenceHtml = GetStringFromWebInterface(string.Format(dicForServiceinterface[serviceName], DealerInterfaceType.dealercarreference, dealerid, serialId)
					, string.Format(cacheTemp, DealerInterfaceType.dealercarreference, dealerid, serialId));
				IsExistAttention = true;

				// 经销商新闻
				dealerDealerNewsHtml = GetStringFromWebInterface(string.Format(dicForServiceinterface[serviceName], DealerInterfaceType.dealernews, dealerid, serialId)
					, string.Format(cacheTemp, DealerInterfaceType.dealernews, dealerid, serialId));

				// 经销商信息
				dealerDealerinfoHtml = GetStringFromWebInterface(string.Format(dicForServiceinterface[serviceName], DealerInterfaceType.dealerinfo, dealerid, serialId)
					, string.Format(cacheTemp, DealerInterfaceType.dealerinfo, dealerid, serialId));
				isShowYouHui = false;
			}
		}

		private string GetStringFromWebInterface(string url, string cacheKey)
		{
			string temp = "";
			object obj = MemCache.GetMemCacheByKey(cacheKey);
			if (obj != null)
			{
				temp = obj.ToString();
			}
			else
			{
				try
				{
					WebClient wc = new WebClient();
					// temp = wc.DownloadString(url);
					wc.Encoding = System.Text.Encoding.GetEncoding("GB2312");
					Uri uri = new Uri(string.Format(url));
					temp = System.Text.Encoding.UTF8.GetString(wc.DownloadData(uri));
					MemCache.SetMemCacheByKey(cacheKey, temp, 1000 * 60 * 15);
				}
				catch (Exception ex)
				{
					CommonFunction.WriteLog(string.Format("GetStringFromWebInterface:{0} {1}", url, ex.ToString()));
				}
			}
			return temp;
		}

		/// <summary>
		/// 取经纪人
		/// </summary>
		private void GetBrokerHtml(string serviceName)
		{
			if (brokerid > 0 && dicForServiceinterface != null && dicForServiceinterface.ContainsKey(serviceName))
			{
				string keyCache = "Car_H5_Broker_" + brokerid.ToString();
				object obj = MemCache.GetMemCacheByKey(keyCache);
				if (obj != null)
				{
					brokerHtml = obj.ToString();
				}
				else
				{
					try
					{
						WebClient wc = new WebClient();
						wc.Encoding = System.Text.Encoding.GetEncoding("GB2312");
						Uri uri = new Uri(string.Format(dicForServiceinterface[serviceName], brokerid, serialId));
						brokerHtml = System.Text.Encoding.UTF8.GetString(wc.DownloadData(uri));
						MemCache.SetMemCacheByKey(keyCache, brokerHtml, 1000 * 60 * 15);
					}
					catch (Exception ex)
					{
						CommonFunction.WriteLog(string.Format("GetBrokerHtml:{0} {1}", dicForServiceinterface[serviceName], ex.ToString()));
					}
				}
				isShowYouHui = false;
			}
		}

		private void InitColorList()
		{
			var colorList = serialFourthStageBll.GetSerialColorList(serialId);
			if (colorList != null && colorList.Count > 0)
			{
				IsExistColor = true;
				if (colorList.Count > 12)
					SerialColorList = colorList.GetRange(0, 12);
				else
				{
					SerialColorList = colorList;
				}
			}
		}

		private void InitExteriorInteriorList()
		{
			//外观设计
			ExteriorList = serialFourthStageBll.GetExteriorInteriorList(serialId, 0);
			if (ExteriorList != null && ExteriorList.Count > 0)
				IsExistAppearance = true;
			//获取外观图片列表
			ExteriorImageList = serialFourthStageBll.GetPhtotoAtlas(serialId, 6);
			if (ExteriorImageList != null && ExteriorImageList.Count > 0)
				IsExistAppearance = true;
			//内饰设计
			InteriorList = serialFourthStageBll.GetExteriorInteriorList(serialId, 1);
			if (InteriorList != null && InteriorList.Count > 0)
				IsExistInner = true;
			//获取内饰图片列表
			InteriorImageList = serialFourthStageBll.GetPhtotoAtlas(serialId, 7);
			if (InteriorImageList != null && InteriorImageList.Count > 0)
				IsExistInner = true;
		}

		/// <summary>
		/// 初始化新闻列表
		/// </summary>
		private void InitNews()
		{
			SerialNewsList = serialFourthStageBll.GetSerialNewsWithCreative(serialId, 4);
			if (SerialNewsList != null && SerialNewsList.Count > 0)
				IsExistPingCe = true;
		}
		/// <summary>
		/// 初始化口碑信息
		/// </summary>
		private void InitKoubei()
		{
			WriterKoubei = serialFourthStageBll.MakeEditorCommentHtml(serialId);
			KoubeiHtml = serialFourthStageBll.MakeKoubeiDianpingHtml(serialId);
			if (!string.IsNullOrEmpty(WriterKoubei) || !string.IsNullOrEmpty(KoubeiHtml))
				IsExistKouBei = true;
		}
		/// <summary>
		/// 初始化车型报价信息
		/// </summary>
		private void InitPrice()
		{
			CarModelList = serialFourthStageBll.MakeCarList(serialId);
			var carids = new List<int>();
			CarModelList.ForEach(o => carids.Add(o.CarID));
			CarParamDictionary = _carBLL.GetCarParamValueByCarIds(carids.ToArray(), 724);
			if (CarModelList != null && CarModelList.Count > 0)
			{
				IsExistBaoJia = true;
				CarModelWithPriceCount = CarModelList.Count;
			}
		}
		/// <summary>
		/// 车型报价
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <returns></returns>
		protected List<CarInfoForSerialSummaryEntity> GetCarModelListByPage(int pageIndex)
		{
			int start = (pageIndex - 1) * 4,
				end = pageIndex * 4,
				count = pageIndex == 5 ? 3 : 4;
			count = start + count > CarModelWithPriceCount ? CarModelWithPriceCount - start : count;
			return CarModelList.GetRange(start, count);
		}

		/// <summary>
		/// 关注的车型
		/// </summary>
		private void InitAttentions()
		{
			AttentionSerials = serialFourthStageBll.MakeSerialToSerialHtml(serialId);
			if (AttentionSerials != null && AttentionSerials.Count > 0)
				IsExistAttention = true;
		}

		protected string GetforwardGearNum(int carModelId)
		{
			if (CarParamDictionary != null && CarParamDictionary.ContainsKey(carModelId))
			{
				var paramValue = CarParamDictionary[carModelId];
				return (!string.IsNullOrEmpty(paramValue) && paramValue != "无级" && paramValue != "待查") ? paramValue + "挡" : "";
			}
			return string.Empty;
		}

		///// <summary>
		///// 商配文章
		///// </summary>
		//private void InitCommerceNews()
		//{
		//	CommerceNewsList = serialFourthStageBll.GetCommerceNews(serialId);
		//}

		private void InitSerialSparkle()
		{
			string allPeizhi = string.Format("<span><a href=\"http://car.m.yiche.com/{0}/peizhi/\" ><img src=\"http://image.bitautoimg.com/carchannel/pic/sparkle/0.png\" />全部配置</a></span>"
				, BaseSerialEntity.AllSpell);
			SerialSparkleList = serialFourthStageBll.GetSerialSparkle(serialId);
			if (SerialSparkleList != null && SerialSparkleList.Count > 0)
			{
				IsExistPeizhi = true;
				List<string> tempList = new List<string>();
				int loop = 0;
				foreach (SerialSparkle ss in SerialSparkleList)
				{
					if (loop >= 12)
					{ break; }
					if ((loop % 4) == 0)
					{
						if (loop > 0)
						{ tempList.Add("</li>"); }
						tempList.Add("<li>");
					}
					if (loop == 11)
					{
						// 如果满12个 第12个位置 用默认更多，到移动站参数配置页
						tempList.Add(allPeizhi);
					}
					else
					{
						tempList.Add(string.Format("<span><img src=\"http://image.bitautoimg.com/carchannel/pic/sparkle/{0}.png\" />{1}</span>"
							, ss.H5SId, ss.Name));
					}
					loop++;
				}
				if (SerialSparkleList.Count < 12)
				{
					// 如果不够12个，补更多
					if ((SerialSparkleList.Count % 4) == 0)
					{
						tempList.Add(string.Format("</li><li>{0}", allPeizhi));
					}
					else
					{
						tempList.Add(allPeizhi);
					}
				}
				if (tempList.Count > 0)
				{
					tempList.Add("</li>");
				}
				PeiZhiHtml = string.Join("", tempList.ToArray());
			}
		}
		///// <summary>
		///// 获取H5 选中子品牌字典表
		///// </summary>
		///// <returns></returns>
		//public Dictionary<int, string> GetDictSelectSerial()
		//{
		//	string cacheKey = "H5_Serial_DictSelectSerial";
		//	object obj = CacheManager.GetCachedData(cacheKey);
		//	if (obj != null)
		//		return (Dictionary<int, string>)obj;
		//	Dictionary<int, string> dictSelectSerial = new Dictionary<int, string>();
		//	try
		//	{
		//		string filePath = Server.MapPath("~") + "\\config\\H5SelectSerialList.xml";
		//		if (File.Exists(filePath))
		//		{
		//			XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(filePath);
		//			XmlNodeList entries = xmlDoc.SelectNodes("/Group/Item");
		//			if (entries != null && entries.Count > 0)
		//			{
		//				foreach (XmlNode xn in entries)
		//				{
		//					int csid = 0;
		//					if (xn.Attributes["CsId"] != null
		//						&& xn.Attributes["CsId"].Value != ""
		//						&& int.TryParse(xn.Attributes["CsId"].Value, out csid))
		//					{ }
		//					string name = string.Empty;
		//					if (xn.Attributes["Name"] != null)
		//					{ name = xn.Attributes["Name"].Value; }
		//					if (csid > 0 && !dictSelectSerial.ContainsKey(csid))
		//					{
		//						dictSelectSerial.Add(csid, name);
		//					}
		//				}
		//			}
		//			CacheManager.InsertCache(cacheKey, dictSelectSerial, WebConfig.CachedDuration);
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		CommonFunction.WriteLog(ex.ToString());
		//	}
		//	return dictSelectSerial;
		//}

		/// <summary>
		/// 获取压缩图片地址
		/// </summary>
		/// <param name="imageUrl"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		protected string GetCompressImageUrl(string imageUrl, int width)
		{
			string tempUrl = string.Empty;
			if (imageUrl.StartsWith("http://"))
			{
				tempUrl = imageUrl.Substring(7);
			}
			else
			{
				tempUrl = imageUrl;
			}
			tempUrl = tempUrl.Substring(tempUrl.IndexOf('/'));
			return string.Format("http://image.bitautoimg.com/newsimg-{0}-w0-1-q80{1}", width, tempUrl);
		}

	}
}