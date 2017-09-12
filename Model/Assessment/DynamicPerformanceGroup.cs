using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.Assessment
{
    /// <summary>
    /// 动力性能
    /// </summary>
    public class DynamicPerformanceGroup
    {
        /// <summary>
        /// 组ID
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 排序编号
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public double Score { get; set; }
        public EditorCommentEntity EditorCommentEntity { get; set; }
        /// <summary>
        /// 加速
        /// </summary>
        public AccelerateEntity AccelerateEntity { get; set; }
        /// <summary>
        /// 发动机表现
        /// </summary>
        public EngineEntity EngineEntity { get; set; }
        /// <summary>
        /// 运动性
        /// </summary>
        public MotilityEntity MotilityEntity { get; set; }
        /// <summary>
        /// 发动机平顺性
        /// </summary>
        public EngineSmoothnessEntity EngineSmoothnessEntity { get; set; }
        /// <summary>
        /// 换挡及变速箱
        /// </summary>
        public GearboxEntity GearboxEntity { get; set; }
        /// <summary>
        /// 知识点
        /// </summary>
        public IList<WikiEntity> WikiList { get; set; }
    }

    /// <summary>
    /// 加速
    /// </summary>
    public class AccelerateEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 加速评分
        /// </summary>
        public int AccelerateScroe { get; set; }
        /// <summary>
        /// 加速总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 加速视频图文
        /// </summary>
        public IList<CommonImageVideoEntity> Videos { get; set; }
        /// <summary>
        /// 加速图片图文
        /// </summary>
        public IList<CommonImageVideoEntity> Images { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Discription { get; set; }
    }

    /// <summary>
    /// 发动机表现
    /// </summary>
    public class EngineEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 扭矩表现评分
        /// </summary>
        public int Torque { get; set; }
        /// <summary>
        /// 牵引力
        /// </summary>
        public int TractiveForce { get; set; }
        /// <summary>
        /// 功率
        /// </summary>
        public int Power { get; set; }
        /// <summary>
        /// 发动机表现总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 发动机频图文
        /// </summary>
        public IList<CommonImageVideoEntity> Videos { get; set; }
        /// <summary>
        /// 发动机图片图文
        /// </summary>
        public IList<CommonImageVideoEntity> Images { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Discription { get; set; }
    }

    /// <summary>
    /// 运动性
    /// </summary>
    public class MotilityEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 运动性评分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 运动性总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 运动性视频及文案
        /// </summary>
        public IList<CommonImageVideoEntity> Videos { get; set; }
        public List<CommonImageVideoEntity> Images { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Discription { get; set; }
    }

    /// <summary>
    /// 发动机平顺性
    /// </summary>
    public class EngineSmoothnessEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 发动机平顺性评分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 发动机平顺性总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 发动机平顺性图片
        /// </summary>
        public IList<CommonImageVideoEntity> Images { get; set; }
        /// <summary>
        /// 发动机平顺性视频
        /// </summary>
        public IList<CommonImageVideoEntity> Videos { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Discription { get; set; }
    }

    /// <summary>
    /// 换挡及变速箱
    /// </summary>
    public class GearboxEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 换挡及变速箱评分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 换挡及变速箱总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 换挡及变速箱视频
        /// </summary>
        public List<string> Videos { get; set; }
        /// <summary>
        /// 换挡及变速箱图片及文案
        /// </summary>
        public IList<CommonImageVideoEntity> Images { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Discription { get; set; }
    }
}
