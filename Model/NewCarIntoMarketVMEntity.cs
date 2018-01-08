using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    public class NewCarIntoMarketVMEntity
    {
        public int CsId { get; set; }
        public int Type { get; set; }
        public int YearType { get; set; }
        public string MarketDay { get; set; }
        public string RefPrice { get; set; }
        public string AllSpell { get; set; }
        public string CoverImage { get; set; }
        public string Level { get; set; }
        public string CsName { get; set; }
    }
}
