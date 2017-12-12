using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    public class PingCeEntity
    {
        /// <summary>
        /// 评测报告ID
        /// </summary>
        public int EvaluationId { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// 头图
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 加速
        /// </summary>
        public double Acceleration { get; set; }
        /// <summary>
        /// 油耗
        /// </summary>
        public double Fuel { get; set; }
        /// <summary>
        /// 刹车距离
        /// </summary>
        public double BrakingDistance { get; set; }
        /// <summary>
        /// 优势描述
        /// </summary>
        public string GoodDiscription { get; set; }
        /// <summary>
        /// 劣势描述
        /// </summary>
        public string BadDiscription { get; set; }
        /// <summary>
        /// 检查是否完成
        /// </summary>
        public bool Finished { get; set; }

        public string StyleName { get; set; }
        public int Year { get; set; }
        public string ModelDisplayName { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string FuelType { get; set; }

        public Dictionary<string, object> ScoreDic = new Dictionary<string, object>();
    }
}
