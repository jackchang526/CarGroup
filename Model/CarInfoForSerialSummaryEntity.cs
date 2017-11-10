using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class CarInfoForSerialSummaryEntity
	{
		/// <summary>
		/// 车型ID
		/// </summary>
		public int CarID;
		/// <summary>
		/// 车型名
		/// </summary>
		public string CarName;
		/// <summary>
		/// 车型热度
		/// </summary>
		public int CarPV;
		/// <summary>
		/// 价格区间
		/// </summary>
		public string CarPriceRange;
		/// <summary>
		/// 变速箱
		/// </summary>
		public string TransmissionType;
		/// <summary>
		/// 排量
		/// </summary>
		public string Engine_Exhaust;
		/// <summary>
		/// 厂商指导价
		/// </summary>
		public string ReferPrice;
		/// <summary>
		/// 档位个数
		/// </summary>
		public string UnderPan_ForwardGearNum;
		/// <summary>
		/// 最大功率—功率值  马力换算：最大功率*1.36 
		/// </summary>
		public int Engine_MaxPower;
		/// <summary>
		/// 电机最大功率 
		/// </summary>
		public int Electric_Peakpower;
		/// <summary>
		/// 进气型式
		/// </summary>
		public string Engine_InhaleType;
		/// <summary>
		/// 增压方式
		/// </summary>
		public string Engine_AddPressType;
		/// <summary>
		/// 燃料类型
		/// </summary>
		public string Oil_FuelType;
		/// <summary>
		/// 车款年款
		/// </summary>
		public string CarYear;
		/// <summary>
		/// 销售状态
		/// </summary>
		public string SaleState;
		/// <summary>
		/// 生产状态
		/// </summary>
		public string ProduceState;
		/// <summary>
		/// 是否平行进口
		/// </summary>
		public int IsImport;
        /// <summary>
        /// 车身形式
        /// </summary>
        public string BodyForm;
        /// <summary>
        /// 上市时间
        /// </summary>
        public DateTime MarketDateTime;
	}
}
