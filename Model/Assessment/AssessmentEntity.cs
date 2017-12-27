using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.Assessment
{
    /// <summary>
    /// 车型评测数据结构
    /// </summary>
    [BsonIgnoreExtraElements]
    public class AssessmentEntity
    {
        //public ObjectId _id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDateTime { get; set; }
        /// <summary>
        /// 车系ID
        /// </summary>
        public int SerialId { get; set; }
        /// <summary>
        /// 车款ID
        /// </summary>
        public int CarId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 评测ID
        /// </summary>
        public int EvaluationId { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string CarName { get; set; }
        /// <summary>
        /// 基本信息类
        /// </summary>
        public CommonInfoGroup CommonInfoGroup { get; set; }
        /// <summary>
        /// 车身及空间
        /// </summary>
        public BodyAndSpaceGroup BodyAndSpaceGroup { get; set; }
        /// <summary>
        /// 安全
        /// </summary>
        public SafetyGroup SafetyGroup { get; set; }
        /// <summary>
        /// 行驶舒适型
        /// </summary>
        public RidingComfortGroup RidingComfortGroup { get; set; }
        /// <summary>
        /// 动力性能
        /// </summary>
        public DynamicPerformanceGroup DynamicPerformanceGroup { get; set; }
        /// <summary>
        /// 驾驶性能
        /// </summary>
        public JsBaseGroup JsBaseGroup { get; set; }
        /// <summary>
        /// 油耗及环保
        /// </summary>
        public YhBaseGroup YhBaseGroup { get; set; }
        /// <summary>
        /// 费用
        /// </summary>
        public CostBaseGroup CostBaseGroup { get; set; }
        /// <summary>
        /// 总结
        /// </summary>
        public GeneralBaseGroup GeneralBaseGroup { get; set; }
    }


}
