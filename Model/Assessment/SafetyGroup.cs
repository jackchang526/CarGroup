using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.Assessment
{
    /// <summary>
    /// 安全
    /// </summary>
    public class SafetyGroup
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
        /// <summary>
        /// 编辑点评
        /// </summary>
        public EditorCommentEntity EditorCommentEntity { get; set; }
        /// <summary>
        /// 刹车
        /// </summary>
        public BrakeEntity BrakeEntity { get; set; }
        /// <summary>
        /// 主/被动安全
        /// </summary>
        public ActiveSafetyEntity ActiveSafetyEntity { get; set; }
        /// <summary>
        /// 车内视野
        /// </summary>
        public VisualFieldEntity VisualFieldEntity { get; set; }
        /// <summary>
        /// 灯光
        /// </summary>
        public LightEntity LightEntity { get; set; }
        /// <summary>
        /// 知识点
        /// </summary>
        public IList<WikiEntity> WikiList { get; set; }
    }

    /// <summary>
    /// 刹车
    /// </summary>
    public class BrakeEntity
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
        /// 空载刹车测量值
        /// </summary>
        public int EmptyScore { get; set; }

        /// <summary>
        /// 满载刹车测量值
        /// </summary>
        public int FullScore { get; set; }

        /// <summary>
        /// 刹车踏板感受
        /// </summary>
        public int BrakeFeel { get; set; }

        /// <summary>
        /// 刹车总评
        /// </summary>
        public string Viewpoint { get; set; }

        /// <summary>
        /// 空载刹车视频
        /// </summary>
        public string EmptyVideo { get; set; }

        /// <summary>
        /// 空载刹车图片
        /// </summary>
        public IList<CommonImageVideoEntity> EmptyImage { get; set; }

        /// <summary>
        /// 满载刹车视频
        /// </summary>
        public string FullVideo { get; set; }

        /// <summary>
        /// 满载刹车图片
        /// </summary>
        public IList<CommonImageVideoEntity> FullImage { get; set; }

        /// <summary>
        /// 刹车描述
        /// </summary>
        public string Discription { get; set; }
    }

    /// <summary>
    /// 主/被动安全
    /// </summary>
    public class ActiveSafetyEntity
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
        /// 驾驶安全辅助
        /// </summary>
        public int DriveSafety { get; set; }

        /// <summary>
        /// 安全装备
        /// </summary>
        public int SafetyDevice { get; set; }

        /// <summary>
        /// 主/被动安全总评
        /// </summary>
        public string Viewpoint { get; set; }

        /// <summary>
        /// 亮点图
        /// </summary>
        public IList<CommonImageVideoEntity> Images { get; set; }
    }

    /// <summary>
    /// 车内视野
    /// </summary>
    public class VisualFieldEntity
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
        /// 车内视野评分
        /// </summary>
        public int ViewScore { get; set; }
        /// <summary>
        /// 车内视野总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 车内视野图片
        /// </summary>
        public IList<CommonImageVideoEntity> Image { get; set; }
    }

    /// <summary>
    /// 灯光
    /// </summary>
    public class LightEntity
    {/// <summary>
     /// 编号
     /// </summary>
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 灯光评分
        /// </summary>
        public int LightScore { get; set; }
        /// <summary>
        /// 灯光总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 灯光图片
        /// </summary>
        public IList<CommonImageVideoEntity> Image { get; set; }
        /// <summary>
        /// 灯光描述
        /// </summary>
        public string Discription { get; set; }
    }
}
