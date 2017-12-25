using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppApi
{
    [Serializable]
    public class CarBrandEntity
    {
        [JsonProperty("brandId")]
        public int BrandId { get; set; }
        [JsonProperty("brandName")]
        public string BrandName { get; set; }
        [JsonProperty("foreign")]
        public bool Foreign { get; set; }
        [JsonProperty("serialList")]
        public List<CarSerialEntity> SerialList { get; set; }
        [JsonProperty("weight")]
        public int Weight { get; set; }
        public string Spell { get; set; }

    }

    [Serializable]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CarSerialEntity
    {
        [JsonProperty("serialId")]
        public int SerialId { get; set; }
        [JsonProperty("serialName")]
        public string serialName { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        /// <summary>
        /// 上市价格
        /// </summary>
        public decimal MarketPrice { get; set; }
        [JsonProperty("Picture")]
        public string CoverImageUrl { get; set; }
        [JsonProperty("uv")]
        public int UV { get; set; }
        /// <summary>
        ///  -1:停销、0:待销、1:在销、2:待查
        /// </summary>
        [JsonProperty("saleStatus")]
        public int SaleStatus { get; set; }
        /// <summary>
        ///  （-1-停销，0-未上市，1-在销,100-即将上市，101-新上市，102-新款上市）
        /// </summary>
        [JsonProperty("newSaleStatus")]
        public int NewSaleStatus { get; set; }


        [JsonProperty("dealerPrice")]
        public string DealerPrice { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }
        public DateTime MaxTimeToMarket { get; set; }
        public DateTime MinTimeToMarket { get; set; }
        public bool IsHaveImage { get; set; }

        public string Spell { get; set; }

    }
}
