using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppApi
{
    [Serializable]
    public class CarStyleInfoEntity
    {
        /// <summary>
        /// 车款Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 车型Id serialId
        /// </summary>
        public int ModelId { get; set; }

        public int Year { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 销售状态  -1:停销、0:待销、1:在销、2:待查
        /// </summary>
        public int SaleStatus { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime TimeToMarket { get; set; }
        /// <summary>
        /// 厂商指导价
        /// </summary>
        public float NowMsrp { get; set; }
        /// <summary>
        /// 是否旅行版 0:否 1：是
        /// </summary>
        public int IsWagon { get; set; }
        /// <summary>
        /// 车型车身形式
        /// 0：单厢车、1：两厢车、3：皮卡、4：三厢车、5：旅行车、6：掀背车、 7：跨界车、8：敞篷车、9：跑车、10：SUV、11：MPV、12：微面、13：卡车、14：客车、15：房车
        /// </summary>
        public int StyleBodyType { get; set; }
        /// <summary>
        /// 生产状态  -1:停产、0:待产、1:在产、2:待查
        /// </summary>
        public int ProductionStatus { get; set; }
        /// <summary>
        /// 车款显示名称
        /// </summary>
        public string DisplayName()
        {
            return string.Format("{0}款{1}", Year, Name);
        }

        public string CarSerialName { get; set; }
    }
}
