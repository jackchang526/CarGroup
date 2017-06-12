using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Common.Enum
{
	public class EnumCollection
	{
		///// <summary>
		///// ��Ʒ����ö��
		///// </summary>
		//public enum SelectCarLevelEnum
		//{
		//    ΢�ͳ� = 1,
		//    С�ͳ� = 2,
		//    ������ = 3,
		//    �д��� = 4,
		//    ���ͳ� = 5,
		//    ������ = 6,
		//    MPV = 7,
		//    SUV = 8,
		//    �ܳ� = 9,
		//    ���� = 10,
		//    ����� = 11,
		//    Ƥ�� = 12,
		//    С��SUV = 13,
		//    ������SUV = 14,
		//    ����SUV = 15,
		//    �д���SUV = 16,
		//    ȫ�ߴ�SUV = 17
		//}

		///// <summary>
		///// ��Ʒ����ö��
		///// </summary>
		//public enum SerialLevelEnum
		//{
		//    ΢�ͳ� = 1,
		//    С�ͳ� = 2,
		//    ������ = 3,
		//    �д��� = 4,
		//    ���ͳ� = 5,
		//    ������ = 6,
		//    MPV = 7,
		//    SUV = 8,
		//    �ܳ� = 9,
		//    ���� = 10,
		//    ����� = 11,
		//    Ƥ�� = 12,
		//    ��� = 13,
		//    ��� = 14,
		//    �ͳ� = 15,
		//    ΢�� = 16,
		//    �Ῠ = 17,
		//    �ؿ� = 18
		//}

		/// <summary>
		/// ��Ʒ��ϸ�ּ����ֶ� add by chengl Mar.18.2014
		/// 0��������1��С��suv��2��������suv��3������suv��4���д���suv��5��ȫ�ߴ�suv
		/// </summary>
		public enum ModelLevelSecond
		{
			���� = 0,
			С��suv = 1,
			������suv = 2,
			����suv = 3,
			�д���suv = 4,
			ȫ�ߴ�suv = 5
		}

		///// <summary>
		///// ��Ʒ����ö��
		///// </summary>
		//public enum SerialLevelSpellEnum
		//{
		//    weixingche = 1,
		//    xiaoxingche = 2,
		//    jincouxingche = 3,
		//    zhongdaxingche = 4,
		//    zhongxingche = 5,
		//    haohuaxingche = 6,
		//    mpv = 7,
		//    suv = 8,
		//    paoche = 9,
		//    qita = 10,
		//    mianbaoche = 11,
		//    pika = 12,
		//    gainianche = 13,
		//    qingke = 14,
		//    keche = 15,
		//    weika = 16,
		//    qingka = 17,
		//    zhongka = 18
		//}

		///// <summary>
		///// ��Ʒ����ö�� ���Ÿ��
		///// add by chengl May.29.2012
		///// </summary>
		//public enum SerialAllLevelEnum
		//{
		//    ΢�ͳ� = 1,
		//    С�ͳ� = 2,
		//    ������ = 3,
		//    �д��� = 4,
		//    ���ͳ� = 5,
		//    ������ = 6,
		//    MPV = 7,
		//    SUV = 8,
		//    �ܳ� = 9,
		//    ���� = 10,
		//    ����� = 11,
		//    Ƥ�� = 12,
		//    ��� = 13,
		//    ��� = 14,
		//    �ͳ� = 15,
		//    ΢�� = 16,
		//    �Ῠ = 17,
		//    �ؿ� = 18
		//}

		///// <summary>
		///// ��Ʒ����ö�� ���Ÿ��
		///// add by chengl May.29.2012
		///// </summary>
		//public enum SerialAllLevelSpellEnum
		//{
		//    weixingche = 1,
		//    xiaoxingche = 2,
		//    jincouxingche = 3,
		//    zhongdaxingche = 4,
		//    zhongxingche = 5,
		//    haohuaxingche = 6,
		//    mpv = 7,
		//    suv = 8,
		//    paoche = 9,
		//    qita = 10,
		//    mianbaoche = 11,
		//    pika = 12,
		//    gainianche = 13,
		//    qingke = 14,
		//    keche = 15,
		//    weika = 16,
		//    qingka = 17,
		//    zhongka = 18
		//}

		/// <summary>
		/// ��Ʒ����;
		/// </summary>
		public enum SerialPurpose
		{
			ԽҰ = 1,
			ʱ�� = 2,
			���� = 3,
			���� = 4,
			���� = 5,
			�˶� = 6,
			���� = 7,
			cross = 8,
			�๦�� = 9
		}

		/// <summary>
		/// ��Ʒ�Ƴ�����ʽ
		/// </summary>
		public enum SerialBodyForm
		{
			����γ� = 1,
			����γ� = 2,
			SUV = 3,
			MPV = 4,
			Cross���ͳ� = 5,
			Wagon���г� = 6,
			Coupe˫��Ӳ���ܳ� = 7,
			Roadster�����ܳ� = 8,
			PickupƤ�� = 9,
			MicroBus��ʽ�� = 10
		}

		/// <summary>
		/// ��Ʒ����;
		/// </summary>
		[Flags]
		public enum SerialPurposeForInterface
		{
			δ֪ = 0,
			ԽҰ = 1,
			ʱ�� = 2,
			���� = 4,
			���� = 8,
			���� = 16,
			�˶� = 32,
			���� = 64,
			cross = 128,
			�๦�� = 256
		}

		/// <summary>
		/// ����������
		/// </summary>
		[Flags]
		public enum FlagsTransmissionType
		{
			ȫ�� = 0,
			�ֶ� = 1,
			�Զ� = 2,
			����һ�� = 4,
			CVT�޼� = 8,
			˫��� = 16,
			���Զ� = 32
		}

		[Flags]
		public enum FlagsSerialBodyType
		{
			ȫ�� = 0,
			����γ� = 1,
			����γ� = 2,
			Cross���ͳ� = 4,
			Wagon���г� = 8,
			Pick_upƤ�� = 16,
			Micro_Bus��ʽ�� = 32
		}

		/// <summary>
		/// ����Ʒ������
		/// </summary>
		[Flags]
		public enum FlagsBrandType
		{
			ȫ�� = 0,
			���� = 1,
			���� = 2,
			���� = 4
		}
		/// <summary>
		/// ���ͼ���
		/// </summary>
		[Flags]
		public enum FlagsSerialLeve
		{
			ȫ�� = 0,
			΢�ͳ� = 1,
			С�ͳ� = 2,
			�����ͳ� = 4,
			�д��ͳ� = 8,
			���ͳ� = 16,
			������ = 32,
			MPV = 64,
			SUV = 128,
			�ܳ� = 256,
			���� = 512,
			����� = 1024,
			Ƥ�� = 2048
		}

		/// <summary>
		/// ��Ʒ�Ƽ۸�����
		/// </summary>
		[Flags]
		public enum FlagsSerialPrice
		{
			All = 0,
			P5 = 1,
			P5_8 = 2,
			P8_12 = 4,
			P12_18 = 8,
			P18_25 = 16,
			P25_40 = 32,
			P40_80 = 64,
			P80 = 128
		}

		/// <summary>
		/// ����
		/// </summary>
		[Flags]
		public enum FlagsCountries
		{
			ȫ�� = 0,
			�й� = 1,
			�ձ� = 2,
			�¹� = 4,
			���� = 8,
			���� = 16,
			���� = 32,
			Ӣ�� = 64,
			����� = 128,
			���� = 256
		}
		/// <summary>
		/// ���Ұ�����
		/// </summary>
		[Flags]
		public enum AreaCountries
		{
			��ϵ = 4,
			��ϵ = 8,
			�պ� = 18,
			ŷϵ = 484
		}
		/// <summary>
		/// ��������
		/// </summary>
		[Flags]
		public enum FlagsComfortableConfig
		{
			ȫ�� = 0,
			Ƥ���� = 1,
			ǰ��綯�� = 2,
			����Ӿ��綯���� = 4,
			����Ӿ����ȹ��� = 8,
			CD = 16,
			DVD = 32,
			����Ѳ��ϵͳ = 64,
			GPS���ӵ��� = 128,
			�����״� = 256,
			����Ӱ�� = 512
		}

		/// <summary>
		/// ��ȫ����
		/// </summary>
		[Flags]
		public enum FlagsSafetyConfig
		{
			ȫ�� = 0,
			ABS = 1,
			ESP = 2,
			��ʻλ��ȫ���� = 4,
			����ʻλ��ȫ���� = 8,
			ǰ��ͷ���������� = 16,
			����ͷ���������� = 32,
			ǰ�Ųలȫ���� = 64,
			���Ųలȫ���� = 128,
			���Ű�ȫ�� = 256,
			�����м�����ʽ��ȫ�� = 512,
			��ͯ��ȫ���ι̶�װ�� = 1024,
			��ͯ�� = 2048,
			�п�����ǰ���� = 4096,
			���Ͻ����� = 8192,
			���ֵ�ɲ = 16384
		}

		/// <summary>
		/// ��������
		/// </summary>
		[Flags]
		public enum CarConfig
		{
			ȫ�� = 0,
			������ѹ = 1,
			�������� = 2,
			���ֵ�ɲ = 4,
			�촰 = 8,
			�綯���� = 16,
			Ƥ���� = 32,
			�綯���� = 64,
			���μ��� = 128,
			�Զ��յ� = 256,
			�綯����Ӿ� = 512,
			ESP = 1024,
			����Ӱ�� = 2048,
			�����״� = 4096,
			GPS���� = 8192,
			�������� = 16384,
			����Ѳ�� = 32768,
			��Կ������ = 65536,
			��ȫ��δϵ��ʾ = 131072,
			������ȫͷ�� = 262144,
			��ͯ�� = 524288,
			��ͯ���ι̶� = 1048576,
			������ = 2097152,
			����λ���� = 4194304,
			̥ѹ���װ�� = 8388608,
			ȼ������ = 16777216,
			���ڿ�������װ�� = 33554432,
			�綯�����й��� = 67108864,
			������Ƭ = 134217728
		}


		/// <summary>
		/// ��Ʒ�ƶԱ�ͼƬ
		/// </summary>
		public class SerialPhotoCompareDataNew
		{
			public SerialPhotoCompareDataNew()
			{
				CsID = 0;
				DefaultCarID = 0;
				DefaultCarName = "";
				DefaultCarSerialName = "";
				DefaultCarYear = "";
				CsAllSpell = "";
				OtherParam = null;
				DicPhotoComparePhotoInfo = null;
			}

			public int CsID;
			public int DefaultCarID;                         // Ĭ�ϳ���ID
			public string DefaultCarName;                    // ������
			public string DefaultCarSerialName;              // ��Ʒ����ʾ��
			public string DefaultCarYear;                    // ���
			public string CsAllSpell;

			public int MasterId { get; set; }
			public string SerialName { get; set; }
			public string SerialShowName { get; set; }
			public string SerialImageUrl { get; set; }

			public Dictionary<int, string> OtherParam;		// �������Ͳ���
			public Dictionary<int, PhotoComparePhotoInfo> DicPhotoComparePhotoInfo; // ÿ��λ�õ�ͼƬ��Ϣ
		}

		/// <summary>
		/// ��Ʒ�ƻ������ݽṹ
		/// </summary>
		public struct CsBaseInfo
		{
			public int CsID;
			public string CsName;
			public string CsShowName;
			public string CsAllSpell;
		}

		/// <summary>
		/// ��Ʒ�ƶԱ�ͼƬ(�ϰ汾)
		/// </summary>
		public struct SerialPhotoCompareData
		{
			public int CsID;
			public int DefaultCarID;                         // Ĭ�ϳ���ID
			public string DefaultCarName;                    // ������
			public string DefaultCarSerialName;              // ��Ʒ����ʾ��
			public string DefaultCarYear;                    // ���
			public string DefaultCarLength;                  // ��
			public string DefaultCarWidth;                   // ��
			public string DefaultCarHeight;                  // ��
			public string DefaultCarWheelBase;               // ���
			public Hashtable CsPhotoData;
			public Hashtable CsPhotoCategoryData;
		}

		/// <summary>
		/// ͼƬ�Ա�ͼƬ������
		/// </summary>
		public struct PhotoComparePhotoInfo
		{
			public int SiteImageId;	// ͼƬID ƴlink��
			public string ImageURL;	// ͼƬ��ַ
		}

		/// <summary>
		/// ͼƬ�Աȵ�����
		/// </summary>
		public class PhotoCompareConfig
		{
			public int ParentCatetroyId { get; set; }
			public string ParentCategoryName { get; set; }
			public int CoverPropertyID;				// �Ա�ͼƬλ��ID
			public string CoverPropertyName;		// �Ա�ͼƬλ����
			public bool IsHasContent;					// ��λ���Ƿ�������
			public List<CarParamForPhotoCompare> OtherParam;		// ��λ����Ҫ��ʾ�Ĳ���
		}

		/// <summary>
		/// ͼƬ�Ա� ��ʾ�ĳ��Ͳ���
		/// </summary>
		public struct CarParamForPhotoCompare
		{
			public int ParamID;				// ����ID
			public string ParamName;	// ������
			public string ParamUnit;		// ������λ
		}

		/// <summary>
		/// ��Ʒ������ҳ������Ϣ
		/// </summary>
		public struct CarInfoForSerialSummary
		{
			public int CarID;                          // ����ID
			public string CarName;                 // ������
			public int CarPV;                         // �����ȶ�
			public string CarPriceRange;        // �۸�����
			public string TransmissionType;    // ������
			public string Engine_Exhaust;       // ����
			public string PerfFuelCostPer100; // �ٹ�������ͺ�
			public string ReferPrice;              // ����ָ����
			public string CarYear;
			public string SaleState;				//����״̬
			public string ProduceState;				//����״̬
		}

		/// <summary>
		/// ��������ҳ������Ϣ
		/// </summary>
		public struct CarInfoForCarSummary
		{
			public int CarID;                          // ����ID
			public string CarName;                 // ������
			public string ReferPrice;              // ����ָ����
			public string CarPriceRange;        // �۸�����
			public string CarTotalPrice;			//Ԥ���ܼ�
			public string PerfFuelCostPer100;	//�ٷ��ͺ�
			public string CarSummaryFuelCost;	//�ۺϹ����ͺ�
			public string TransmissionType;    // ������
			public string Engine_Exhaust;       // ����
			public string CarLevel;                 // ����
			public string CarBodyType;          // ������ʽ
			public string CarRepairPolicy;      // �ʱ�
			public string CarMarketDate;        // ����ʱ��     
			public int CarCpID;                      // ����ID
			public string CarCpShortName;     // ���̼��
		}

		/// <summary>
		/// ��Ʒ����Ƭ(��Ʒ������ҳ&�ӿ�)
		/// </summary>
		public struct SerialInfoCard
		{
			public int CsID;                                  // ��Ʒ��ID
			public string CsName;                         // ��Ʒ����
			public string CsShowName;                 // ��Ʒ����ʾ�� 
			public string CsAllSpell;                      // ��Ʒ��ȫƴ 
			public string CsDefaultPic;                  // ��Ʒ��Ĭ��ͼ
			public string CsPriceRange;                // ��Ʒ�Ƽ۸�����
			public int CsPicCount;                        // ��Ʒ��ͼƬ��
			public int CsDianPingCount;                // ��Ʒ�Ƶ�����
			public int CsAskCount;                       // ��Ʒ�ƴ�����
			public string CsEngine_Exhaust;          // ��Ʒ������(Html)
			public string CsEngineExhaust;			// ��Ʒ������
			public string CsOfficialFuelCost;          // ��Ʒ�ƹٷ��ͺ�
			public string CsSummaryFuelCost;			//��Ʒ���ۺϹ����ͺ�
			public string CsGuestFuelCost;            // ��Ʒ�������ͺ�
			public string CsTransmissionType;       // ��Ʒ�Ʊ�����
			public string OfficialSite;				//��Ʒ�ƹ���
			public string SerialRepairPolicy;		//��������
			public List<string> PurposeList;		//��;�б�
			public List<string> ColorList;			//��ɫ�б�
			public string CsSaleState;				//����״̬
			public string CsLevel;                     // ��Ʒ�Ƽ���

			public string CsNewShangShi;              // ��Ʒ������ ����ר��
			public string CsNewGouCheShouChe;    // ��Ʒ������ �����ֲ�
			public string CsNewXiaoShouShuJu;      // ��Ʒ������ ��������
			public string CsNewWeiXiuBaoYang;     // ��Ʒ������ ά�ޱ���
			public string CsNewKeJi;                      // ��Ʒ������ �Ƽ�
			public string CsNewAnQuan;                     //  ��Ʒ������ ��ȫ
			public string CsNewYouHao;                     // ��Ʒ������ �ͺ�
			public string CsNewMaiCheCheShi;       // ��Ʒ������ �򳵲���
			public string CsNewYiCheCheShi;         // ��Ʒ������ �׳�����
			public string CsSanBaoLink;	//��Ʒ�� ����

			public string CsBodyForm;//������ʽ
		}

		/// <summary>
		/// ����ע��Ʒ��
		/// </summary>
		public struct SerialToSerial
		{
			public int CsID;    // ��ǰ��Ʒ��
			public int ToCsID;    // ����ע��Ʒ��ID
			public string ToCsName; // ����ע��Ʒ����
			public string ToCsShowName;    // ����ע��Ʒ����ʾ��
			public string ToCsPic;              // ����ע��Ʒ��Ĭ��ͼ
			public string ToCsPriceRange;   // ����ע��Ʒ�Ƽ۸�����
			public string ToCsAllSpell;        // ����ע��Ʒ��ȫƴ
			public int ToPv_Num;               // ����ע��Ʒ�ƴ���
		    public string ToCsSaleState;        //����ע��Ʒ������״̬
		}

		/// <summary>
		/// ������������
		/// </summary>
		public struct CarHotCompareData
		{
			public int CurrentCarID;
			public string CurrentCarName;
			public int CurrentCsID;
			public string CurrentCsName;
			public int CompareCarID;
			public string CompareCarName;
			public int CompareCsID;
			public string CompareCsName;
			public int CompareCount;
		}

		/// <summary>
		/// ������Ʒ������
		/// </summary>
		public struct SerialHotCompareData
		{
			public int CurrentCsID;
			public int CompareCsID;
			public string CompareCsName;
			public string CompareCsShowName;
			public string CompareCsAllSpell;
			public string CompareCsCbName;
			public int CompareCount;
			public string CompareCsPriceRange;
            public string ComapreCsImg;
		}

		/// <summary>
		/// ��Ʒ������
		/// </summary>
		public struct SerialSortForInterface
		{
			public int CsID;
			public string CsName;
			public string CsShowName;
			public string CsAllSpell;
			public string CsLevel;
			public string CsPriceRange;
			public int CsPV;
		}

		/// <summary>
		/// �����³�
		/// </summary>
		public struct NewCarForLevel
		{
			public int CarID;
			public string CarName;
			public int CsID;
			public string CsName;
			public string CsShowName;
			public string CsAllSpell;
		}

		/// <summary>
		/// ��ɫ���� ����ͼ��xml����
		/// </summary>
		public class SerialColorItem
		{
			public int CarID;
			public int ColorID;
			public string ColorName;
			public string ColorRGB;
		}

		/// <summary>
		/// ����ı�ǩ ����ƥ�����
		/// </summary>
		public struct PingCeTag
		{
			public string tagName;
			public string tagRegularExpressions;
			public int tagId;
			public string url;
		}

		/// <summary>
		/// ��̳���ӷ���ö�� add by chengl Aug.16.2013
		/// </summary>
		public enum ForumDigest
		{
			�ᳵ��ҵ = 1,
			�ó����� = 2,
			װ�θ�װ = 3,
			�ۻᱨ�� = 4,
			�Լ��μ� = 5,
			�Գ��Լ� = 6,
			�ȵ㻰�� = 7,
			����ͼƬ = 8,
			�³����� = 10,
			���� = 11,
			��ͨ�¹� = 12,
			·�� = 13,
			��ɫ��Ů = 14
		}

	}
}
