using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppApi
{
    [Serializable]
    public class CarBrandEntity
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public bool Foreign { get; set; }
        public List<CarSerialEntity> SerialList { get; set; }
        public int Weight { get; set; }
        public string Spell { get; set; }

    }

    [Serializable]
    public class CarSerialEntity
    {
        public int SerialId { get; set; }
        public string serialName { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        /// <summary>
        /// 上市价格
        /// </summary>
        public decimal MarketPrice { get; set; }

        public string CoverImageUrl { get; set; }

        public int UV { get; set; }
        /// <summary>
        ///  -1:停销、0:待销、1:在销、2:待查
        /// </summary>
        public int SaleStatus { get; set; }
        /// <summary>
        ///  （-1-停销，0-未上市，1-在销,100-即将上市，101-新上市，102-新款上市）
        /// </summary>
        public int NewSaleStatus { get; set; }


        public string DealerPrice { get; set; }

        public int Weight { get; set; }

        public DateTime MaxTimeToMarket { get; set; }

        public DateTime MinTimeToMarket { get; set; }

        public bool IsHaveImage { get; set; }

        public string Spell { get; set; }

    }
}
