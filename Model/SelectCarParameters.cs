using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class SelectCarParameters
	{
		private int m_minPrice;
		private int m_maxPrice;
		private int m_minReferPrice;
		private int m_maxReferPrice;
		private double m_minDis;
		private double m_maxDis;
		private int m_transmissionType;
		private int m_bodyForm;
		private int m_brandType;
		private int m_level;
		private int m_purpose;
		private int m_country;
		private int m_comfortable;
		private int m_safety;
		private int m_pageIndex;
		private int m_carConfig;

		private string m_conditionStr;		//查询条件说明
		private int m_priceFlag;			//仅有报价查询的标志
        //油耗
        private double m_minFuel;
        private double m_maxFuel;

		/// <summary>
		/// 最小报价值（单位：万元）
		/// </summary>
		public int MinPrice
		{
			get { return m_minPrice; }
			set { m_minPrice = value; }
		}
		/// <summary>
		/// 最大报价值（单位：万元）
		/// </summary>
		public int MaxPrice
		{
			get { return m_maxPrice; }
			set { m_maxPrice = value; }
		}

		/// <summary>
		/// 最小指导价（单位：万元）
		/// </summary>
		public int MinReferPrice
		{
			get { return m_minReferPrice; }
			set { m_minReferPrice = value; }
		}

		/// <summary>
		/// 最大指导价(万元)
		/// </summary>
		public int MaxReferPrice
		{
			get { return m_maxReferPrice; }
			set { m_maxReferPrice = value; }
		}
		/// <summary>
		/// 最小排量值
		/// </summary>
		public double MinDis
		{
			get { return m_minDis; }
			set { m_minDis = value; }
		}
		/// <summary>
		/// 最大排量值
		/// </summary>
		public double MaxDis
		{
			get { return m_maxDis; }
			set { m_maxDis = value; }
		}
		/// <summary>
		/// 变速器类型
		/// </summary>
		public int TransmissionType
		{
			get { return m_transmissionType; }
			set { m_transmissionType = value; }
		}
		/// <summary>
		/// 车身形式
		/// </summary>
		public int BodyForm
		{
			get { return m_bodyForm; }
			set { m_bodyForm = value; }
		}

		/// <summary>
		/// 品牌类型，自主，合资，进口
		/// </summary>
		public int BrandType
		{
			get { return m_brandType; }
			set { m_brandType = value; }
		}
		/// <summary>
		/// 车型级别
		/// </summary>
		public int Level
		{
			get { return m_level; }
			set { m_level = value; }
		}
		/// <summary>
		/// 车型用途
		/// </summary>
		public int Purpose
		{
			get { return m_purpose; }
			set { m_purpose = value; }
		}
		/// <summary>
		/// 产系国家
		/// </summary>
		public int Country
		{
			get { return m_country; }
			set { m_country = value; }
		}
		/// <summary>
		/// 舒适性配置
		/// </summary>
		public int ComfortableConfig
		{
			get { return m_comfortable; }
			set { m_comfortable = value; }
		}
		/// <summary>
		/// 安全性配置
		/// </summary>
		public int SafetyConfig
		{
			get { return m_safety; }
			set { m_safety = value; }
		}
		/// <summary>
		/// 页号
		/// </summary>
		public int PageIndex
		{
			get { return m_pageIndex; }
			set { m_pageIndex = value; }
		}

		/// <summary>
		/// 车型配置
		/// </summary>
		public int CarConfig
		{
			get { return m_carConfig; }
			set { m_carConfig = value; }
		}

		/// <summary>
		/// 查询条件说明
		/// </summary>
		public string ConditionString
		{
			get { return m_conditionStr; }
			set { m_conditionStr = value; }
		}

		/// <summary>
		/// 仅有报价条件的标志
		/// </summary>
		public int PriceFlag
		{
			get { return m_priceFlag; }
			set { m_priceFlag = value; }
		}
		/// <summary>
		/// 驱动方式
		/// </summary>
		public int DriveType { get; set; }
		/// <summary>
		/// 燃料类型
		/// </summary>
		public int FuelType { get; set; }
		/// <summary>
		/// 最小车门数
		/// </summary>
		public int MinBodyDoors { get; set; }
		/// <summary>
		/// 最大车门数
		/// </summary>
		public int MaxBodyDoors { get; set; }
		/// <summary>
		///	最小乘员人数（含司机）
		/// </summary>
		public int MinPerfSeatNum { get; set; }
		/// <summary>
		/// 最大乘员人数（含司机）
		/// </summary>
		public int MaxPerfSeatNum { get; set; }
		/// <summary>
		/// 是否旅行版
		/// </summary>
		public int IsWagon { get; set; }
        /// <summary>
        /// 最小油耗值
        /// </summary>
        public double MinFuel
        {
            get { return m_minFuel; }
            set { m_minFuel = value; }
        }
        /// <summary>
        /// 最大油耗值
        /// </summary>
        public double MaxFuel
        {
            get { return m_maxFuel; }
            set { m_maxFuel = value; }
        }
	}
}


