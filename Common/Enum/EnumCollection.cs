using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Common.Enum
{
	public class EnumCollection
	{
		///// <summary>
		///// 子品级别枚举
		///// </summary>
		//public enum SelectCarLevelEnum
		//{
		//    微型车 = 1,
		//    小型车 = 2,
		//    紧凑型 = 3,
		//    中大型 = 4,
		//    中型车 = 5,
		//    豪华车 = 6,
		//    MPV = 7,
		//    SUV = 8,
		//    跑车 = 9,
		//    其它 = 10,
		//    面包车 = 11,
		//    皮卡 = 12,
		//    小型SUV = 13,
		//    紧凑型SUV = 14,
		//    中型SUV = 15,
		//    中大型SUV = 16,
		//    全尺寸SUV = 17
		//}

		///// <summary>
		///// 子品级别枚举
		///// </summary>
		//public enum SerialLevelEnum
		//{
		//    微型车 = 1,
		//    小型车 = 2,
		//    紧凑型 = 3,
		//    中大型 = 4,
		//    中型车 = 5,
		//    豪华车 = 6,
		//    MPV = 7,
		//    SUV = 8,
		//    跑车 = 9,
		//    其它 = 10,
		//    面包车 = 11,
		//    皮卡 = 12,
		//    概念车 = 13,
		//    轻客 = 14,
		//    客车 = 15,
		//    微卡 = 16,
		//    轻卡 = 17,
		//    重卡 = 18
		//}

		/// <summary>
		/// 子品牌细分级别字段 add by chengl Mar.18.2014
		/// 0：其它、1：小型suv、2：紧凑型suv、3：中型suv、4：中大型suv、5：全尺寸suv
		/// </summary>
		public enum ModelLevelSecond
		{
			其它 = 0,
			小型suv = 1,
			紧凑型suv = 2,
			中型suv = 3,
			中大型suv = 4,
			全尺寸suv = 5
		}

		///// <summary>
		///// 子品级别枚举
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
		///// 子品级别枚举 开放概念车
		///// add by chengl May.29.2012
		///// </summary>
		//public enum SerialAllLevelEnum
		//{
		//    微型车 = 1,
		//    小型车 = 2,
		//    紧凑型 = 3,
		//    中大型 = 4,
		//    中型车 = 5,
		//    豪华车 = 6,
		//    MPV = 7,
		//    SUV = 8,
		//    跑车 = 9,
		//    其它 = 10,
		//    面包车 = 11,
		//    皮卡 = 12,
		//    概念车 = 13,
		//    轻客 = 14,
		//    客车 = 15,
		//    微卡 = 16,
		//    轻卡 = 17,
		//    重卡 = 18
		//}

		///// <summary>
		///// 子品级别枚举 开放概念车
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
		/// 子品牌用途
		/// </summary>
		public enum SerialPurpose
		{
			越野 = 1,
			时尚 = 2,
			家用 = 3,
			代步 = 4,
			休闲 = 5,
			运动 = 6,
			商务 = 7,
			cross = 8,
			多功能 = 9
		}

		/// <summary>
		/// 子品牌车身形式
		/// </summary>
		public enum SerialBodyForm
		{
			两厢轿车 = 1,
			三厢轿车 = 2,
			SUV = 3,
			MPV = 4,
			Cross混型车 = 5,
			Wagon旅行车 = 6,
			Coupe双门硬顶跑车 = 7,
			Roadster敞篷跑车 = 8,
			Pickup皮卡 = 9,
			MicroBus厢式车 = 10
		}

		/// <summary>
		/// 子品牌用途
		/// </summary>
		[Flags]
		public enum SerialPurposeForInterface
		{
			未知 = 0,
			越野 = 1,
			时尚 = 2,
			家用 = 4,
			代步 = 8,
			休闲 = 16,
			运动 = 32,
			商务 = 64,
			cross = 128,
			多功能 = 256
		}

		/// <summary>
		/// 变速器类型
		/// </summary>
		[Flags]
		public enum FlagsTransmissionType
		{
			全部 = 0,
			手动 = 1,
			自动 = 2,
			手自一体 = 4,
			CVT无级 = 8,
			双离合 = 16,
			半自动 = 32
		}

		[Flags]
		public enum FlagsSerialBodyType
		{
			全部 = 0,
			两厢轿车 = 1,
			三厢轿车 = 2,
			Cross混型车 = 4,
			Wagon旅行车 = 8,
			Pick_up皮卡 = 16,
			Micro_Bus厢式车 = 32
		}

		/// <summary>
		/// 车的品牌类型
		/// </summary>
		[Flags]
		public enum FlagsBrandType
		{
			全部 = 0,
			自主 = 1,
			合资 = 2,
			进口 = 4
		}
		/// <summary>
		/// 车型级别
		/// </summary>
		[Flags]
		public enum FlagsSerialLeve
		{
			全部 = 0,
			微型车 = 1,
			小型车 = 2,
			紧凑型车 = 4,
			中大型车 = 8,
			中型车 = 16,
			豪华车 = 32,
			MPV = 64,
			SUV = 128,
			跑车 = 256,
			其他 = 512,
			面包车 = 1024,
			皮卡 = 2048
		}

		/// <summary>
		/// 子品牌价格区间
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
		/// 国家
		/// </summary>
		[Flags]
		public enum FlagsCountries
		{
			全部 = 0,
			中国 = 1,
			日本 = 2,
			德国 = 4,
			美国 = 8,
			韩国 = 16,
			法国 = 32,
			英国 = 64,
			意大利 = 128,
			其他 = 256
		}
		/// <summary>
		/// 国家按区域
		/// </summary>
		[Flags]
		public enum AreaCountries
		{
			德系 = 4,
			美系 = 8,
			日韩 = 18,
			欧系 = 484
		}
		/// <summary>
		/// 舒适配置
		/// </summary>
		[Flags]
		public enum FlagsComfortableConfig
		{
			全部 = 0,
			皮座椅 = 1,
			前后电动窗 = 2,
			外后视镜电动调节 = 4,
			外后视镜加热功能 = 8,
			CD = 16,
			DVD = 32,
			定速巡航系统 = 64,
			GPS电子导航 = 128,
			倒车雷达 = 256,
			倒车影像 = 512
		}

		/// <summary>
		/// 安全配置
		/// </summary>
		[Flags]
		public enum FlagsSafetyConfig
		{
			全部 = 0,
			ABS = 1,
			ESP = 2,
			驾驶位安全气囊 = 4,
			副驾驶位安全气囊 = 8,
			前排头部气囊气帘 = 16,
			后排头部气囊气帘 = 32,
			前排侧安全气囊 = 64,
			后排侧安全气囊 = 128,
			后排安全带 = 256,
			后排中间三点式安全带 = 512,
			儿童安全座椅固定装置 = 1024,
			儿童锁 = 2048,
			中控门锁前后排 = 4096,
			铝合金轮辋 = 8192,
			四轮碟刹 = 16384
		}

		/// <summary>
		/// 车型配置
		/// </summary>
		[Flags]
		public enum CarConfig
		{
			全部 = 0,
			涡轮增压 = 1,
			四轮驱动 = 2,
			四轮碟刹 = 4,
			天窗 = 8,
			电动车窗 = 16,
			皮座椅 = 32,
			电动座椅 = 64,
			座椅加热 = 128,
			自动空调 = 256,
			电动外后视镜 = 512,
			ESP = 1024,
			倒车影像 = 2048,
			倒车雷达 = 4096,
			GPS导航 = 8192,
			泊车辅助 = 16384,
			定速巡航 = 32768,
			无钥匙启动 = 65536,
			安全带未系提示 = 131072,
			主动安全头枕 = 262144,
			儿童锁 = 524288,
			儿童座椅固定 = 1048576,
			氙气大灯 = 2097152,
			五座位以上 = 4194304,
			胎压监测装置 = 8388608,
			燃料类型 = 16777216,
			车内空气净化装置 = 33554432,
			电动窗防夹功能 = 67108864,
			换档拨片 = 134217728
		}


		/// <summary>
		/// 子品牌对比图片
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
			public int DefaultCarID;                         // 默认车型ID
			public string DefaultCarName;                    // 车型名
			public string DefaultCarSerialName;              // 子品牌显示名
			public string DefaultCarYear;                    // 年款
			public string CsAllSpell;

			public int MasterId { get; set; }
			public string SerialName { get; set; }
			public string SerialShowName { get; set; }
			public string SerialImageUrl { get; set; }

			public Dictionary<int, string> OtherParam;		// 其他车型参数
			public Dictionary<int, PhotoComparePhotoInfo> DicPhotoComparePhotoInfo; // 每个位置的图片信息
		}

		/// <summary>
		/// 子品牌基本数据结构
		/// </summary>
		public struct CsBaseInfo
		{
			public int CsID;
			public string CsName;
			public string CsShowName;
			public string CsAllSpell;
		}

		/// <summary>
		/// 子品牌对比图片(老版本)
		/// </summary>
		public struct SerialPhotoCompareData
		{
			public int CsID;
			public int DefaultCarID;                         // 默认车型ID
			public string DefaultCarName;                    // 车型名
			public string DefaultCarSerialName;              // 子品牌显示名
			public string DefaultCarYear;                    // 年款
			public string DefaultCarLength;                  // 长
			public string DefaultCarWidth;                   // 宽
			public string DefaultCarHeight;                  // 高
			public string DefaultCarWheelBase;               // 轴距
			public Hashtable CsPhotoData;
			public Hashtable CsPhotoCategoryData;
		}

		/// <summary>
		/// 图片对比图片的属性
		/// </summary>
		public struct PhotoComparePhotoInfo
		{
			public int SiteImageId;	// 图片ID 拼link用
			public string ImageURL;	// 图片地址
		}

		/// <summary>
		/// 图片对比的配置
		/// </summary>
		public class PhotoCompareConfig
		{
			public int ParentCatetroyId { get; set; }
			public string ParentCategoryName { get; set; }
			public int CoverPropertyID;				// 对比图片位置ID
			public string CoverPropertyName;		// 对比图片位置名
			public bool IsHasContent;					// 此位置是否有内容
			public List<CarParamForPhotoCompare> OtherParam;		// 此位置需要显示的参数
		}

		/// <summary>
		/// 图片对比 显示的车型参数
		/// </summary>
		public struct CarParamForPhotoCompare
		{
			public int ParamID;				// 参数ID
			public string ParamName;	// 参数名
			public string ParamUnit;		// 参数单位
		}

		/// <summary>
		/// 子品牌综述页车型信息
		/// </summary>
		public struct CarInfoForSerialSummary
		{
			public int CarID;                          // 车型ID
			public string CarName;                 // 车型名
			public int CarPV;                         // 车型热度
			public string CarPriceRange;        // 价格区间
			public string TransmissionType;    // 变速箱
			public string Engine_Exhaust;       // 排量
			public string PerfFuelCostPer100; // 百公里等速油耗
			public string ReferPrice;              // 厂商指导价
			public string CarYear;
			public string SaleState;				//销售状态
			public string ProduceState;				//生产状态
		}

		/// <summary>
		/// 车型综述页车型信息
		/// </summary>
		public struct CarInfoForCarSummary
		{
			public int CarID;                          // 车型ID
			public string CarName;                 // 车型名
			public string ReferPrice;              // 厂商指导价
			public string CarPriceRange;        // 价格区间
			public string CarTotalPrice;			//预估总价
			public string PerfFuelCostPer100;	//官方油耗
			public string CarSummaryFuelCost;	//综合工况油耗
			public string TransmissionType;    // 变速箱
			public string Engine_Exhaust;       // 排量
			public string CarLevel;                 // 级别
			public string CarBodyType;          // 车身厢式
			public string CarRepairPolicy;      // 质保
			public string CarMarketDate;        // 上市时间     
			public int CarCpID;                      // 厂商ID
			public string CarCpShortName;     // 厂商简称
		}

		/// <summary>
		/// 子品牌名片(子品牌综述页&接口)
		/// </summary>
		public struct SerialInfoCard
		{
			public int CsID;                                  // 子品牌ID
			public string CsName;                         // 子品牌名
			public string CsShowName;                 // 子品牌显示名 
			public string CsAllSpell;                      // 子品牌全拼 
			public string CsDefaultPic;                  // 子品牌默认图
			public string CsPriceRange;                // 子品牌价格区间
			public int CsPicCount;                        // 子品牌图片数
			public int CsDianPingCount;                // 子品牌点评数
			public int CsAskCount;                       // 子品牌答疑数
			public string CsEngine_Exhaust;          // 子品牌排量(Html)
			public string CsEngineExhaust;			// 子品牌排量
			public string CsOfficialFuelCost;          // 子品牌官方油耗
			public string CsSummaryFuelCost;			//子品牌综合工况油耗
			public string CsGuestFuelCost;            // 子品牌网友油耗
			public string CsTransmissionType;       // 子品牌变速箱
			public string OfficialSite;				//子品牌官网
			public string SerialRepairPolicy;		//保修政策
			public List<string> PurposeList;		//用途列表
			public List<string> ColorList;			//颜色列表
			public string CsSaleState;				//销售状态
			public string CsLevel;                     // 子品牌级别

			public string CsNewShangShi;              // 子品牌新闻 上市专题
			public string CsNewGouCheShouChe;    // 子品牌新闻 购车手册
			public string CsNewXiaoShouShuJu;      // 子品牌新闻 销售数据
			public string CsNewWeiXiuBaoYang;     // 子品牌新闻 维修保养
			public string CsNewKeJi;                      // 子品牌新闻 科技
			public string CsNewAnQuan;                     //  子品牌新闻 安全
			public string CsNewYouHao;                     // 子品牌新闻 油耗
			public string CsNewMaiCheCheShi;       // 子品牌新闻 买车测试
			public string CsNewYiCheCheShi;         // 子品牌新闻 易车评测
			public string CsSanBaoLink;	//子品牌 三包

			public string CsBodyForm;//车身形式
		}

		/// <summary>
		/// 还关注子品牌
		/// </summary>
		public struct SerialToSerial
		{
			public int CsID;    // 当前子品牌
			public int ToCsID;    // 还关注子品牌ID
			public string ToCsName; // 还关注子品牌名
			public string ToCsShowName;    // 还关注子品牌显示名
			public string ToCsPic;              // 还关注子品牌默认图
			public string ToCsPriceRange;   // 还关注子品牌价格区间
			public string ToCsAllSpell;        // 还关注子品牌全拼
			public int ToPv_Num;               // 还关注子品牌次数
		    public string ToCsSaleState;        //还关注子品牌销售状态
		}

		/// <summary>
		/// 竞争车型数据
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
		/// 竞争子品牌数据
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
		/// 子品牌排行
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
		/// 级别新车
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
		/// 颜色类型 来自图库xml数据
		/// </summary>
		public class SerialColorItem
		{
			public int CarID;
			public int ColorID;
			public string ColorName;
			public string ColorRGB;
		}

		/// <summary>
		/// 评测的标签 名及匹配规则
		/// </summary>
		public struct PingCeTag
		{
			public string tagName;
			public string tagRegularExpressions;
			public int tagId;
			public string url;
		}

		/// <summary>
		/// 论坛帖子分类枚举 add by chengl Aug.16.2013
		/// </summary>
		public enum ForumDigest
		{
			提车作业 = 1,
			用车养护 = 2,
			装饰改装 = 3,
			聚会报道 = 4,
			自驾游记 = 5,
			试乘试驾 = 6,
			热点话题 = 7,
			精彩图片 = 8,
			新车谍照 = 10,
			靓车 = 11,
			交通事故 = 12,
			路书 = 13,
			绝色美女 = 14
		}

	}
}
