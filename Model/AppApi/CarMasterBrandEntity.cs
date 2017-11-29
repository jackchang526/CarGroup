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
        public int MasterID { get; set; }
        /// <summary>
        /// 主品牌名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 车标
        /// </summary>
        public string LogoUrl { get; set; }
        /// <summary>
        /// 首字母
        /// </summary>
        public string Initial { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        ///  -1:停销、0:待销、1:在销、2:待查
        /// </summary>
        public int SaleStatus { get; set; }
    }

}
