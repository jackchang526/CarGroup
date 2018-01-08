using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{

    /// <summary>
    /// 上市新车表 NewCarIntoMarket
    /// </summary>
    public class NewCarIntoMarketEntity
    {
        public int CsId { get; set; }
        public int Type { get; set; }
        public int YearType { get; set; }
        public DateTime MarketDay { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
