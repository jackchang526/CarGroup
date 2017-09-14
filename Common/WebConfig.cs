using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace BitAuto.CarChannel.Common
{
	public class WebConfig
	{
		public static string DefaultConnectionString;	    //Ĭ�������ַ���
		public static string PvConnectionString;	            //PV�������ַ���
		public static string AutoStorageConnectionString;		//���ͺ�̨���������ַ���
		public static string InsuranceLoanConnectionString;		//���ͺ�̨���������ַ���
		public static string CarDataUpdateConnectionString;     //���ͺ�̨�������ݿ������ַ���
		public static string DataBlockPath;                      // ����Ƶ���ļ���λ��
		public static string AutoDataFile;				        //Ʒ�ƣ���Ʒ�������ļ�
		public static string AutoDataUrl;				        //Ʒ�ƣ���Ʒ������xml��url
		public static string BaseAutoDataUrl;				    //��Ʒ�ơ�Ʒ�ơ���Ʒ�ƣ����������� xml��url  
		public static string BaseAllAutoDataUrl;				//��Ʒ�ơ�Ʒ�ơ���Ʒ�ƣ�����ͣ�������� xml��url   
		public static string BaseAllAutoDataAndLevelUrl;		//��Ʒ�ơ�Ʒ�ơ���Ʒ�ƣ�������������� xml��url	
		public static int UpdateInterval;				        //Ʒ�ƣ���Ʒ�����ݸ��¼��ʱ��
		public static int CachedDuration;				        //Ʒ�ƣ���Ʒ�����ݻ������ʱ��
		public static string WebRootPath;				        //��վ��Ŀ¼������·��
		public static string DefaultCarPic;                     // ����Ĭ��ͼ
		public static string DefaultVideoPic;
		public static string ImageDomain;                       // ͼƬ����
		public static string PhotoService;                       // ͼ��ӿڵ�ַ
		public static string PhotoCompareSerialList;       // ͼƬ�Աȵ���Ʒ��
		public static string PhotoCompareService;           // ͼƬ�ԱȽӿ�
		public static string PriceRangeSerial;                 // ���м۸��������Ʒ��
		public static string SerialToCar;                        // ��Ʒ�������г���
		public static string HeadForSerial;                    // ��Ʒ��ͷ
		public static string HeadForCar;                        // ����ͷ
		// public static string LevelCarCost;						//���ݼ����ȡ�ͺ�����������
		public static string AllCarPriceNoZone;        // ȫ�����ͱ���(���ֵ���)
		public static string AllSerialPriceNoZone;     // ȫ����Ʒ�Ʊ���(���ֵ���)
		public static string AllSerialAskCount;         // ȫ����Ʒ�ƴ�������
		public static string AllSerialDianPingCount;     // ȫ����Ʒ�Ƶ�������
		public static string AllSerialPicCount;             // ȫ����Ʒ��ͼƬ��������Ʒ��Ĭ��ͼ
		public static string SerialDianPingYouHao;      // ��Ʒ�Ƶ����ͺ�
		public static string CarCompareStat;               // ���ͶԱ�ͳ��
		public static string SerialCompareStat;            // ��Ʒ�ƶԱ�ͳ��
		public static string MasterToSerialXMLPath;		//���㹤�����裺Ʒ�ƣ���Ʒ�������ļ�
		public static string SerialKouBeiData;              // ��Ʒ�ƿڱ�����
		public static string AllSpellList;                      // ���Ϳ�ȫƴ����
		public static string SeriaPingCeData;               // ����Ա�
		public static string SerialPingCeDataNew;	        //��������
		public static string AddDianPingYouHao;        // ��ӵ����ͺ�
		public static string NewsUrl;					//��ȡ���ŵĵ�ַ
		public static string BBSUrl;
		public static string SerialKouBeiDataForCsSummary;  // ��Ʒ����ϸ�ڱ�����(��Ʒ������ҳ)
		public static string NDomesticCarRBItemIDs;		//���ڲʺ���
		public static string DomesticCarRBItemIDs;		//�����ʺ���
		public static string SellDataMapUrl;			//�������ݵ�ͼ�ӿڵ�ַ
		public static string SerialYouHaoRangeNew; // ��Ʒ���ͺ�����(�ڱ�)
		public static string AllSerialDefaultPicAndCount; // ��Ʒ����Ĭ��ͼ(2��Ĭ��ͼ)
		public static string CarDataBaseNASPath; // ���ͻ�������NAS(ͨ�õ���ͷ)
		public static string SerialPhoto12ImageInterface; // ��Ʒ��12�ű�׼ͼ
		public static string CarPhoto12ImageInterface; // ����12�ű�׼ͼ
		public static string IndexDataBlockPath;		//ָ���ļ��洢·��
		public static string SerialKouReport;          // �ڱ�����
		public static string CarColorPhoto;             // ������ɫͼƬ
		public static string NewsRequestUrl;
		public static string PhotoProvideCateHTML;   // ͼ���ṩͼƬҳHTML
		public static string PhotoSerialInterface;       // ͼ����Ʒ�ƽӿ�(��)
		public static string PhotoCarInterface;          // ͼ�⳵�ͽӿ�(��)
		public static string NewsEditerMessageUrl;       // ���ű༭��Ϣ��ַ

		public static string SerialOutSetWebPath;       // ͼ���ұ�ͼƬ��ַ
		public static string SerialOutSetDefaultWebPath;       // ͼ���ұ�Ĭ��ͼƬ��ַ

		public static string MongoDBConnString;				//MongoDB�����ַ���
		public static string PhotoNASDataPath;				// ͼ��NAS��DataĿ¼
		public static string BaaCarBrandToForumUrl;	//��Ʒ�ƶ�Ӧ��̳��Ϣ

		/// <summary>
		/// �Ƿ�ʹ��memcache�����ʱ��ʹ�ñ���cache Ĭ��Ϊtrue
		/// </summary>
		public static bool IsUseMemcache = true;	// �Ƿ�ʹ��memcache�����ʱ��ʹ�ñ���cache Ĭ��Ϊtrue

		/// <summary>
		/// MongoDB for Car
		/// </summary>
		public static string MongoDBForCarConnectionString;	// MongoDB for Car

        public static string CarsEvaluationDataConnectionString;
        public static string MongoDBConnectionString;
        public static string MongoDBDefaultDataBase;
        public static string MongoDBDefaultDataTable;


        /// <summary>
        /// ��վ��׼��ַ
        /// </summary>
        public static string WebSiteBaseUrl
		{
			get
			{
				if (HttpContext.Current == null) return string.Empty;

				HttpRequest request = HttpContext.Current.Request;
				//������ַ
				//��80�˿ڼ��϶˿ں�
				string host = request.Url.IsDefaultPort ?
					request.Url.Host : string.Format("{0}:{1}", request.Url.Host, request.Url.Port);
				//��������·��
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
		/// ��̬�ļ���׼��ַ
		/// </summary>
		public static string StaticFileBaseUrl
		{
			get
			{
				if (string.IsNullOrEmpty(_StaticFileBaseUrl))
				{
					var currentContext = HttpContext.Current;
					var baseUrl = ConfigurationManager.AppSettings["StaticFileRootUrl"];

					//����Ǳ��أ����ر���url
					if (currentContext != null && currentContext.Request.IsLocal)
					{
						_StaticFileBaseUrl = WebSiteBaseUrl;
					}
					//���򷵻������ļ��е�url
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
		/// װ����վ����,��OnApplicationStart�е���
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
        }

	}
}
