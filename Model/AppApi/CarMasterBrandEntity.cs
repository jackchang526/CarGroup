using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppApi
{
    [Serializable]
    public class CarMasterBrandEntity
    {
        /// <summary>
        /// 主品牌ID
        /// </summary>
        [JsonProperty("masterId")]
        public int MasterID { get; set; }
        /// <summary>
        /// 主品牌名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 车标
        /// </summary>
        [JsonProperty("logoUrl")]
        public string LogoUrl { get; set; }
        /// <summary>
        /// 首字母
        /// </summary>
        [JsonProperty("initial")]
        public string Initial { get; set; }
        /// <summary>
        /// UV
        /// </summary>
        [JsonProperty("uv")]
        public int UV { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        [JsonProperty("weight")]
        public int Weight { get; set; }

        /// <summary>
        ///  -1:停销、0:待销、1:在销、2:待查
        /// </summary>
        [JsonProperty("saleStatus")]
        public int SaleStatus { get; set; }
    }

}
