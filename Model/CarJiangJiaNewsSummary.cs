using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 车型降价汇总信息
    /// </summary>
    public class CarJiangJiaNewsSummary
    {
        /// <summary>
        /// 车型ID
        /// </summary>
        public int CarId;
        /// <summary>
        /// 最高降额 0.000
        /// </summary>
        public decimal MaxFavorablePrice;
        /// <summary>
        /// 最高降幅 0.00
        /// </summary>
        public decimal MaxFavorableRate;
    }
}
