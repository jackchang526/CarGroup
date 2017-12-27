using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppModel
{
    /// <summary>
    /// 易湃各地价格
    /// </summary>
    [Serializable]
    public class SalePriceInfoEntity
    {
        public int CityId { get; set; }
        public int Id { get; set; }
        public float MinReferPrice { get; set; }
        public float MaxReferPrice { get; set; }
        public int ReturnType { get; set; }
    }
}
