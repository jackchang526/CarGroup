using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.Assessment
{
    /// <summary>
    /// 行驶舒适型
    /// </summary>
    public class RidingComfortGroup
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
        /// 座椅
        /// </summary>
        public ChairEntity ChairEntity { get; set; }
        /// <summary>
        /// 空调
        /// </summary>
        public AirConditionerEntity AirConditionerEntity { get; set; }
        /// <summary>
        /// 悬挂舒适性
        /// </summary>
        public HangComfortEntity HangComfortEntity { get; set; }
        /// <summary>
        /// 车内噪声测量值
        /// </summary>
        public NoiseEntity NoiseEntity { get; set; }
        /// <summary>
        /// 噪声感受
        /// </summary>
        public NoiseFeelEntity NoiseFeelEntity { get; set; }
        /// <summary>
        /// 知识点
        /// </summary>
        public IList<WikiEntity> WikiList { get; set; }
    }

    /// <summary>
    /// 座椅
    /// </summary>
    public class ChairEntity
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
        /// 前排座椅评分
        /// </summary>
        public int FrontChair { get; set; }
        /// <summary>
        /// 后排座椅评分
        /// </summary>
        public int BehindChair { get; set; }
        /// <summary>
        /// 座椅总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 前排座椅图文
        /// </summary>
        public IList<CommonImageVideoEntity> FrontImages { get; set; }
        /// <summary>
        /// 前排座椅描述
        /// </summary>
        public string FrontDescription { get; set; }
        /// <summary>
        /// 前排座椅亮点图
        /// </summary>
        public IList<CommonImageVideoEntity> FrontPointImages { get; set; }
        /// <summary>
        /// 后排座椅图文
        /// </summary>
        public IList<CommonImageVideoEntity> BehindImages { get; set; }
        /// <summary>
        /// 后排座椅描述
        /// </summary>
        public string BehindDescription { get; set; }
        /// <summary>
        /// 后排座椅亮点图
        /// </summary>
        public IList<CommonImageVideoEntity> BehindPointImages { get; set; }
    }

    /// <summary>
    /// 空调
    /// </summary>
    public class AirConditionerEntity
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
        /// 空调感受评分
        /// </summary>
        public int AirConditionerFeel { get; set; }
        /// <summary>
        /// 空调降温评分
        /// </summary>
        public int Refrigeration { get; set; }
        /// <summary>
        /// 空调总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 空调图片
        /// </summary>
        public IList<CommonImageVideoEntity> AirConditionerImages { get; set; }
        /// <summary>
        /// 空调描述
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 悬挂舒适性
    /// </summary>
    public class HangComfortEntity
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
        /// 悬挂舒适性
        /// </summary>
        public int SuspensionComfort { get; set; }
        /// <summary>
        /// 悬挂舒适性总评
        /// </summary>
        public string Viewpoit { get; set; }
        /// <summary>
        /// 悬挂图文
        /// </summary>
        public IList<CommonImageVideoEntity> HangImages { get; set; }
        /// <summary>
        /// 悬挂描述
        /// </summary>
        public string Discription { get; set; }
    }

    /// <summary>
    /// 车内噪声测量值
    /// </summary>
    public class NoiseEntity
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
        /// 车内噪声测量值评分
        /// </summary>
        public int NoiseScore { get; set; }
        /// <summary>
        /// 车内噪声测量值总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 车内噪声测量图文
        /// </summary>
        public IList<CommonImageVideoEntity> NoiseImages { get; set; }
        /// <summary>
        /// 车内噪声测量值描述
        /// </summary>
        public string Discription { get; set; }
    }

    /// <summary>
    /// 噪声感受
    /// </summary>
    public class NoiseFeelEntity
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
        /// 怠速噪声评分
        /// </summary>
        public int IdlingNoise { get; set; }
        /// <summary>
        /// 行驶噪声评分
        /// </summary>
        public int RunNoise { get; set; }
        /// <summary>
        /// 车内噪声印象评分
        /// </summary>
        public int NoiseFeel { get; set; }
        /// <summary>
        /// 噪声总评
        /// </summary>
        public string Viewpoint { get; set; }
    }
}
