using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	/// <summary>
	/// 子品牌降价汇总数据
	/// </summary>
	public class SerialJiangJiaNewsSummary
	{
		/// <summary>
		/// 新闻数
		/// </summary>
		public int NewsCount;
		/// <summary>
		/// 最好降价
		/// </summary>
		public decimal MaxFavorablePrice;
        /// <summary>
        /// 最好降幅
        /// </summary>
        public decimal MaxFavorableRate;
		/// <summary>
		/// 经销商数
		/// </summary>
        public int VendorNum;
        /// <summary>
        /// 车型数，不去重复
        /// </summary>
        public int CarNum;

	}
}
