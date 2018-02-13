using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace BitAuto.CarChannel.Common
{
	public class WebConfig
	{
		public static string DefaultConnectionString;	    //默认连接字符串
		public static string PvConnectionString;	            //PV库连接字符串
		public static string AutoStorageConnectionString;		//车型后台数据连接字符串
		public static string InsuranceLoanConnectionString;		//车型后台数据连接字符串
		public static string CarDataUpdateConnectionString;     //车型后台服务数据库连接字符串
		public static string DataBlockPath;                      // 车型频道文件块位置
		public static string AutoDataFile;				        //品牌，子品牌数据文件
		public static string AutoDataUrl;				        //品牌，子品牌数据xml的url
		public static string BaseAutoDataUrl;				    //主品牌、品牌、子品牌（在销）数据 xml的url  
		public static string BaseAllAutoDataUrl;				//主品牌、品牌、子品牌（包括停销）数据 xml的url   
		public static string BaseAllAutoDataAndLevelUrl;		//主品牌、品牌、子品牌（包括概念车）数据 xml的url	
		public static int UpdateInterval;				        //品牌，子品牌数据更新检查时间
		public static int CachedDuration;				        //品牌，子品牌数据缓存持续时间
		public static string WebRootPath;				        //网站根目录的物理路径
		public static string DefaultCarPic;                     // 车型默认图
		public static string DefaultVideoPic;
		public static string ImageDomain;                       // 图片域名
		public static string PhotoService;                       // 图库接口地址
		public static string PhotoCompareSerialList;       // 图片对比的子品牌
		public static string PhotoCompareService;           // 图片对比接口
		public static string PriceRangeSerial;                 // 所有价格区间的子品牌
		public static string SerialToCar;                        // 子品牌下所有车型
		public static string HeadForSerial;                    // 子品牌头
		public static string HeadForCar;                        // 车型头
		// public static string LevelCarCost;						//根据级别获取油耗与养车费用
		public static string AllCarPriceNoZone;        // 全部车型报价(部分地区)
		public static string AllSerialPriceNoZone;     // 全部子品牌报价(部分地区)
		public static string AllSerialAskCount;         // 全部子品牌答疑总数
		public static string AllSerialDianPingCount;     // 全部子品牌点评总数
		public static string AllSerialPicCount;             // 全部子品牌图片总数及子品牌默认图
		public static string SerialDianPingYouHao;      // 子品牌点评油耗
		public static string CarCompareStat;               // 车型对比统计
		public static string SerialCompareStat;            // 子品牌对比统计
		public static string MasterToSerialXMLPath;		//计算工具所需：品牌，子品牌数据文件
		public static string SerialKouBeiData;              // 子品牌口碑数据
		public static string AllSpellList;                      // 车型库全拼对照
		public static string SeriaPingCeData;               // 评测对比
		public static string SerialPingCeDataNew;	        //评测新闻
		public static string AddDianPingYouHao;        // 添加点评油耗
		public static string NewsUrl;					//获取新闻的地址
		public static string BBSUrl;
		public static string SerialKouBeiDataForCsSummary;  // 子品牌详细口碑数据(子品牌综述页)
		public static string NDomesticCarRBItemIDs;		//进口彩虹条
		public static string DomesticCarRBItemIDs;		//国产彩虹条
		public static string SellDataMapUrl;			//销量数据地图接口地址
		public static string SerialYouHaoRangeNew; // 子品牌油耗区间(口碑)
		public static string AllSerialDefaultPicAndCount; // 子品牌新默认图(2种默认图)
		public static string CarDataBaseNASPath; // 车型基础数据NAS(通用导航头)
		public static string SerialPhoto12ImageInterface; // 子品牌12张标准图
		public static string CarPhoto12ImageInterface; // 车型12张标准图
		public static string IndexDataBlockPath;		//指数文件存储路径
		public static string SerialKouReport;          // 口碑报告
		public static string CarColorPhoto;             // 车型颜色图片
		public static string NewsRequestUrl;
		public static string PhotoProvideCateHTML;   // 图库提供图片页HTML
		public static string PhotoSerialInterface;       // 图库子品牌接口(泛)
		public static string PhotoCarInterface;          // 图库车型接口(泛)
		public static string NewsEditerMessageUrl;       // 新闻编辑信息地址

		public static string SerialOutSetWebPath;       // 图释右边图片地址
		public static string SerialOutSetDefaultWebPath;       // 图释右边默认图片地址

		public static string MongoDBConnString;				//MongoDB连接字符串
		public static string PhotoNASDataPath;				// 图库NAS上Data目录
		public static string BaaCarBrandToForumUrl;	//子品牌对应论坛信息

		/// <summary>
		/// 是否使用memcache，否的时候使用本地cache 默认为true
		/// </summary>
		public static bool IsUseMemcache = true;	// 是否使用memcache，否的时候使用本地cache 默认为true

		/// <summary>
		/// MongoDB for Car
		/// </summary>
		public static string MongoDBForCarConnectionString;	// MongoDB for Car

        public static string CarsEvaluationDataConnectionString;
        public static string MongoDBConnectionString;
        public static string MongoDBDefaultDataBase;
        public static string MongoDBDefaultDataTable;
        public static string SelectCarUrl;


        /// <summary>
        /// 网站基准网址
        /// </summary>
        public static string WebSiteBaseUrl
		{
			get
			{
				if (HttpContext.Current == null) return string.Empty;

				HttpRequest request = HttpContext.Current.Request;
				//主机地址
				//非80端口加上端口号
				string host = request.Url.IsDefaultPort ?
					request.Url.Host : string.Format("{0}:{1}", request.Url.Host, request.Url.Port);
				//加上虚拟路径
				string virtualPath = request.ApplicationPath;
				if (!virtualPath.EndsWith("/"))
				{
					virtualPath = virtualPath + "/";
				}
				return string.Format("http://{0}{1}", host, virtualPath);
			}
		}

		private static string _StaticFileBaseUrl = null;
		/// <summary>
		/// 静态文件基准网址
		/// </summary>
		public static string StaticFileBaseUrl
		{
			get
			{
				if (string.IsNullOrEmpty(_StaticFileBaseUrl))
				{
					var currentContext = HttpContext.Current;
					var baseUrl = ConfigurationManager.AppSettings["StaticFileRootUrl"];

					//如果是本地，返回本地url
					if (currentContext != null && currentContext.Request.IsLocal)
					{
						_StaticFileBaseUrl = WebSiteBaseUrl;
					}
					//否则返回配置文件中的url
					else if (!string.IsNullOrEmpty(baseUrl))
					{
						_StaticFileBaseUrl = baseUrl;
					}
					else
					{
						_StaticFileBaseUrl = string.Empty;
					}
				}

				return _StaticFileBaseUrl;
			}
		}

		/// <summary>
		/// 装载网站配置,在OnApplicationStart中调用
		/// </summary>
		public static void LoadConfig()
		{
			WebRootPath = AppDomain.CurrentDomain.BaseDirectory;
			DataBlockPath = ConfigurationManager.AppSettings["DataBlockPath"];
			DefaultConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
			PvConnectionString = ConfigurationManager.ConnectionStrings["PvConnectionString"].ConnectionString;
			InsuranceLoanConnectionString = ConfigurationManager.ConnectionStrings["InsuranceLoanConnectionString"].ConnectionString;
			AutoDataUrl = ConfigurationManager.AppSettings["AutoDataUrl"];
			BaseAutoDataUrl = ConfigurationManager.AppSettings["BaseAutoDataUrl"];
			BaseAllAutoDataUrl = ConfigurationManager.AppSettings["BaseAllAutoDataUrl"];
			BaseAllAutoDataAndLevelUrl = ConfigurationManager.AppSettings["BaseAllAutoDataAndLevelUrl"];
			// AutoDataFile = Path.Combine(WebRootPath, ConfigurationManager.AppSettings["AutoData"]);
			AutoDataFile = Path.Combine(DataBlockPath, ConfigurationManager.AppSettings["AutoData"]);
			UpdateInterval = Convert.ToInt32(ConfigurationManager.AppSettings["UpdateInterval"]);
			CachedDuration = Convert.ToInt32(ConfigurationManager.AppSettings["CachedDuration"]);
			DefaultCarPic = ConfigurationManager.AppSettings["DefaultCarPic"];
			DefaultVideoPic = ConfigurationManager.AppSettings["DefaultVideoPic"];
			ImageDomain = ConfigurationManager.AppSettings["ImageDomain"];
			PhotoService = ConfigurationManager.AppSettings["PhotoService"];
			PhotoCompareSerialList = ConfigurationManager.AppSettings["PhotoCompareSerialList"];
			PhotoCompareService = ConfigurationManager.AppSettings["PhotoCompareService"];
			PriceRangeSerial = ConfigurationManager.AppSettings["PriceRangeSerial"];
			SerialToCar = ConfigurationManager.AppSettings["SerialToCar"];
			HeadForSerial = ConfigurationManager.AppSettings["HeadForSerial"];
			HeadForCar = ConfigurationManager.AppSettings["HeadForCar"];
			// MasterToSerialXMLPath = Path.Combine(WebRootPath, ConfigurationManager.AppSettings["MasterToSerialXMLPath"]);
			MasterToSerialXMLPath = ConfigurationManager.AppSettings["MasterToSerialXMLPath"];
			AutoStorageConnectionString = ConfigurationManager.ConnectionStrings["AutoStorageConnectionString"].ConnectionString;
			// LevelCarCost = ConfigurationManager.AppSettings["LevelCarCost"];
			AllCarPriceNoZone = ConfigurationManager.AppSettings["AllCarPriceNoZone"];
			AllSerialPriceNoZone = ConfigurationManager.AppSettings["AllSerialPriceNoZone"];
			AllSerialAskCount = ConfigurationManager.AppSettings["AllSerialAskCount"];
			AllSerialDianPingCount = ConfigurationManager.AppSettings["AllSerialDianPingCount"];
			AllSerialPicCount = ConfigurationManager.AppSettings["AllSerialPicCount"];
			SerialDianPingYouHao = ConfigurationManager.AppSettings["SerialDianPingYouHao"];
			CarCompareStat = ConfigurationManager.AppSettings["CarCompareStat"];
			SerialCompareStat = ConfigurationManager.AppSettings["SerialCompareStat"];
			SerialKouBeiData = ConfigurationManager.AppSettings["SerialKouBeiData"];
			AllSpellList = ConfigurationManager.AppSettings["AllSpellList"];
			SeriaPingCeData = ConfigurationManager.AppSettings["SeriaPingCeData"];
			SerialPingCeDataNew = ConfigurationManager.AppSettings["SerialPingCeDataNew"];
			AddDianPingYouHao = ConfigurationManager.AppSettings["AddDianPingYouHao"];
			SerialKouBeiDataForCsSummary = ConfigurationManager.AppSettings["SerialKouBeiDataForCsSummary"];
			NDomesticCarRBItemIDs = ConfigurationManager.AppSettings["NDomesticCarRBItemIDs"];
			DomesticCarRBItemIDs = ConfigurationManager.AppSettings["DomesticCarRBItemIDs"];
			NewsUrl = ConfigurationManager.AppSettings["NewsUrl"];
			BBSUrl = ConfigurationManager.AppSettings["BBSUrl"];
			SellDataMapUrl = ConfigurationManager.AppSettings["SellDataMapUrl"];
			SerialYouHaoRangeNew = ConfigurationManager.AppSettings["SerialYouHaoRangeNew"];
			AllSerialDefaultPicAndCount = ConfigurationManager.AppSettings["AllSerialDefaultPicAndCount"];
			CarDataBaseNASPath = ConfigurationManager.AppSettings["CarDataBaseNASPath"];
			SerialPhoto12ImageInterface = ConfigurationManager.AppSettings["SerialPhoto12ImageInterface"];
			CarPhoto12ImageInterface = ConfigurationManager.AppSettings["CarPhoto12ImageInterface"];
			IndexDataBlockPath = ConfigurationManager.AppSettings["IndexDataBlockPath"];
			SerialKouReport = ConfigurationManager.AppSettings["SerialKouReport"];
			CarColorPhoto = ConfigurationManager.AppSettings["CarColorPhoto"];
			NewsRequestUrl = ConfigurationManager.AppSettings["NewsRequestUrl"];
			PhotoProvideCateHTML = ConfigurationManager.AppSettings["PhotoProvideCateHTML"];
			PhotoSerialInterface = ConfigurationManager.AppSettings["PhotoSerialInterface"];
			PhotoCarInterface = ConfigurationManager.AppSettings["PhotoCarInterface"];
			NewsEditerMessageUrl = ConfigurationManager.AppSettings["NewsEditerMessageUrl"];
			SerialOutSetWebPath = ConfigurationManager.AppSettings["SerialOutSetWebPath"];
			SerialOutSetDefaultWebPath = ConfigurationManager.AppSettings["SerialOutSetDefaultWebPath"];
			MongoDBConnString = ConfigurationManager.AppSettings["MongoDBConnectionString"];
			PhotoNASDataPath = ConfigurationManager.AppSettings["PhotoNASDataPath"];
			CarDataUpdateConnectionString = ConfigurationManager.ConnectionStrings["CarDataUpdateConnectionString"] != null ? ConfigurationManager.ConnectionStrings["CarDataUpdateConnectionString"].ConnectionString : string.Empty;

			BaaCarBrandToForumUrl = ConfigurationManager.AppSettings["BaaCarBrandToForumUrl"];
			if (ConfigurationManager.AppSettings["IsUseMemcache"] != null
				&& bool.TryParse(ConfigurationManager.AppSettings["IsUseMemcache"], out IsUseMemcache))
			{ }
			MongoDBForCarConnectionString = ConfigurationManager.AppSettings["MongoDBForCarConnectionString"];


            CarsEvaluationDataConnectionString=ConfigurationManager.ConnectionStrings["CarsEvaluationData"] != null ? ConfigurationManager.ConnectionStrings["CarsEvaluationData"].ConnectionString : string.Empty;
            MongoDBConnectionString = ConfigurationManager.AppSettings["MongoDBConnectionString"];
            MongoDBDefaultDataBase = ConfigurationManager.AppSettings["MongoDBDefaultDataBase"];
            MongoDBDefaultDataTable = ConfigurationManager.AppSettings["MongoDBDefaultDataTable"];

            SelectCarUrl = ConfigurationManager.AppSettings["SelectCarUrl"];
        }

	}
}
