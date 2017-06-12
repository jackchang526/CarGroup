using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Common
{
	/// <summary>
	/// 时间类型
	/// </summary>
	public enum DateType
	{
		Season = 1,
		Month = 2,
		Week = 3
	}

	/// <summary>
	/// 指数项数据
	/// </summary>
	public class IndexItem
	{
		public int ID;			//项目ID
		public string ItmeName;	//项目名称
		public string ItemUrl;	//项目地址
		public int Index;		//项目指数
	}

	/// <summary>
	/// 日期数据，用于存储时间，可存储如下结构：年-周、年-季度、年-月。
	/// </summary>
	public class DateObj
	{
		public int Year;			//年
		public int DateNum;			//季度，月，周的数字
	}

	/// <summary>
	/// 地区类型
	/// </summary>
	public enum RegionType
	{
		None = 0,		//全国
		Province = 1,	//省
		City = 2		//城市
	}

	/// <summary>
	/// 指数类型
	/// </summary>
	public enum IndexType
	{
		UV = 1,
		Dealer = 2,
		Sale = 3,
		Media = 4,
		Compare = 5
	}

	/// <summary>
	/// 品牌类型（厂商，主品牌，子品牌，级别）
	/// </summary>
	public enum BrandType
	{
		Producer = 1,
		MasterBrand = 2,
		Serial = 3,
		Level = 4
	}

	/// <summary>
	/// 指数项数据
	/// </summary>
	public class IndexDetailItem
	{
		public int ID;			//项目ID
		public string ItmeName;	//项目名称
		public string ItemUrl;	//项目地址
		public string PriceRange;	//项目报价
		public string Exhaust;		//排量
		public string AllExhaust;	//全部排量
		public string Transmission;	//变速器类型
		public string SettingsUrl;	//配置Url
		public string ImageUrl;		//图库Url
		public string KoubeiUrl;	//口碑Url
		public string AskUrl;		//答疑Url
		public int Index;		//项目指数
		public int ImageNum;	//图片数量
		public int KoubeiNum;	//口碑数量
		public int AskNum;		//答疑数量
	}

	/// <summary>
	/// 图表上显示的数据项
	/// </summary>
	public class ChartDataItem
	{
		public string Name;
		public double Data;
	}

	/// <summary>
	/// 城市排行数据项
	/// </summary>
	public class CitySortItem
	{
		public int CityId;
		public string cityName;
		public int Sort;
		public int IndexNum;
	}

	/// <summary>
	/// 厂商信息
	/// </summary>
	public class ProducerInfo
	{
		public int Id;
		public string Name;
		public string Spell;
	}

	/// <summary>
	/// 主品牌信息
	/// </summary>
	public class MasterBrandInfo
	{
		public int BsID;
		public string BsName;
		public string UrlSpell;
		public string Introduction;
	}

	/// <summary>
	/// 品牌信息
	/// </summary>
	public class BrandInfo
	{
		public int Id;
		public string Name;
		public string Country;
	}

	/// <summary>
	/// 子品级别枚举
	/// </summary>
	public enum SerialLevelSpellEnum
	{
		weixingche = 1,
		xiaoxingche = 2,
		jincouxingche = 3,
		zhongdaxingche = 4,
		zhongxingche = 5,
		haohuaxingche = 6,
		mpv = 7,
		suv = 8,
		paoche = 9,
		qita = 10,
		mianbaoche = 11,
		pika = 12
	}

	/// <summary>
	/// 子品级别枚举
	/// </summary>
	public enum SerialLevelEnum
	{
		微型车 = 1,
		小型车 = 2,
		紧凑型 = 3,
		中大型 = 4,
		中型车 = 5,
		豪华车 = 6,
		MPV = 7,
		SUV = 8,
		跑车 = 9,
		其它 = 10,
		面包车 = 11,
		皮卡 = 12
	}

	/// <summary>
	/// 车型扩展参数
	/// </summary>
	public class CarParam
	{
		public int ParamID;
		public string ParamName;
		public string AliasName;
		public string ModuleDec;
	}

	public struct SerialColorStruct
	{
		public int AutoID;
		public int CsID;
		public string ColorName;
		public string ColorRGB;
	}

	/// <summary>
	/// 年度十佳车 广告
	/// </summary>
	public struct BestTopCar
	{
		public string Title;	// a标签title
		public string Link; //  a标签link
		public List<int> ListCsList; // 年度子品牌列表
	}

	/// <summary>
	/// 子品牌连接广告
	/// </summary>
	public struct LinkADForCs
	{
		public string Key;	// 唯一标识
		public string Title;	// 广告描述
		public string Link;	// 广告地址
		public List<int> ListCsID;	// 投广告的子品牌ID列表
	}
	/// <summary>
	/// 子品牌上市时间
	/// </summary>
	public struct SeralMarketDay
	{
		public string Sign;
		public string Day;
	}

	/// <summary>
	/// 子品牌口碑基本信息
	/// </summary>
	public struct CsKoubeiBaseInfo
	{
		/// <summary>
		/// 子品牌ID
		/// </summary>
		public int CsID;

		/// <summary>
		/// 综合评分
		/// </summary>
		public decimal Rating;

		/// <summary>
		/// 同级别评分
		/// </summary>
		public decimal LevelRating;

		/// <summary>
		/// 评分变化率
		/// </summary>
		public decimal RatingVariance;

		/// <summary>
		/// 口碑数
		/// </summary>
		public int TotalCount;

		/// <summary>
		/// 口碑细则评分数
		/// </summary>
		public Dictionary<string, decimal> DicSubKoubei;
	}

}


