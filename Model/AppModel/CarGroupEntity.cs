using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppModel
{
    /// <summary>
    /// 车款分组数据
    /// </summary>
    [Serializable]
    public class CarGroupEntity
    {
        public string Name { get; set; }
        public List<CarInfoEntity> CarList { get; set; }

    }
    /// <summary>
    /// 车款信息
    /// </summary>
    public class CarInfoEntity
    {
        /// <summary>
        /// 车ID
        /// </summary>
        public int CarId { get; set; }
        /// <summary>
        /// 车型名称
        /// </summary>
        public string Name { get; set; }
        public string Year { get; set; }
        /// <summary>
        /// 是否商城在销
        /// </summary>
        public bool IsSupport { get; set; }
        /// <summary>
        /// 最低报价
        /// </summary>
        public string MinPrice { get; set; }
        /// <summary>
        /// 厂商指导价

        /// </summary>
        public string ReferPrice { get; set; }
        /// <summary>
        /// 档位模式 
        /// </summary>
        public string Trans { get; set; }
        public string CarImg { get; set; }
        /// <summary>
        /// 销售状态
        /// </summary>
        public string SaleState { get; set; }
        /// <summary>
        /// 销售状态（-1-停销，0-未上市，1-在销,100-即将上市，101-新上市，102-新款上市）
        /// </summary>
        public string NewSaleStatus { get; set; }
        /// <summary>
        /// 此车款类型 包销:0 、平行:1
        /// </summary>
        public int SupportType { get; set; }
        /// <summary>
        /// 平行进口车
        /// </summary>
        public string ImportType { get; set; }
        
        public string CarLink { get; set; }
        /// <summary>
        /// 车款pv数
        /// </summary>
        public int CarPV { get; set; }
        /// <summary>
        /// 是否减税，true-减税，false-不减税
        /// </summary>
        public bool IsTax { get; set; }
        /// <summary>
        /// 减税描述
        /// </summary>
        public string TaxRelief { get; set; }
        /// <summary>
        /// 上市时间
        /// </summary>
        public string TimeToMarket { get; set; }
        /// <summary>
        /// 是否有实拍图（true-有，false-没有）
        /// </summary>
        public bool IsHaveImage { get; set; }
           
    }
}
