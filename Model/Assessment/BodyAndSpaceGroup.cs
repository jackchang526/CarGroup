using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.Assessment
{
    [BsonIgnoreExtraElements]
    /// <summary>
    /// 车身及空间
    /// </summary>
    public class BodyAndSpaceGroup
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
        /// 车内空间
        /// </summary>
        public SpaceEntity SpaceEntity { get; set; }
        /// <summary>
        /// 行李厢容积
        /// </summary>
        public TrunkEntity TrunkEntity { get; set; }
        /// <summary>
        /// 空间变化
        /// </summary>
        public StoragespaceEntity StoragespaceEntity { get; set; }
        /// <summary>
        /// 操作便利性
        /// </summary>
        public ConvenienceEntity ConvenienceEntity { get; set; }
        /// <summary>
        /// 制造质量
        /// </summary>
        public QualityEntity QualityEntity { get; set; }
        /// <summary>
        /// 知识点
        /// </summary>
        public IList<WikiEntity> WikiList { get; set; }
    }

    /// <summary>
    /// 车内空间
    /// </summary>
    public class SpaceEntity
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
        /// 车内空间印象分
        /// </summary>
        public int ImpressionScore { get; set; }
        /// <summary>
        /// 车内空间测量值
        /// </summary>
        public int ActualScore { get; set; }
        /// <summary>
        /// 车内空间总评
        /// </summary>
        public string Comment { get; set; }
    }

    /// <summary>
    /// 行李厢容积
    /// </summary>
    public class TrunkEntity
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
        /// 容积
        /// </summary>
        public int Cubage { get; set; }
        /// <summary>
        /// 行李厢容积总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 后备箱图片 
        /// </summary>
        public IList<CommonImageVideoEntity> ImageList { get; set; }
        /// <summary>
        /// 行李厢描述
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 空间变化
    /// </summary>
    public class StoragespaceEntity
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
        /// 空间变化评分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 空间变化总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 空间变化图片
        /// </summary>
        public IList<CommonImageVideoEntity> ImageList { get; set; }
        /// <summary>
        /// 空间变化描述
        /// </summary>
        public string Discription { get; set; }
        /// <summary>
        /// 空间变化亮点图
        /// </summary>
        public IList<CommonImageVideoEntity> ImageSpecailList { get; set; }
    }

    /// <summary>
    /// 操作便利性
    /// </summary>
    public class ConvenienceEntity
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
        /// 设备
        /// </summary>
        public int Equipment { get; set; }
        /// <summary>
        /// 多媒体
        /// </summary>
        public int Multimedia { get; set; }
        /// <summary>
        /// 操作便利性总评
        /// </summary>
        public string Viewpoint { get; set; }
        /// <summary>
        /// 图文描述
        /// </summary>
        public IList<CommonImageVideoEntity> Images { get; set; }

        /// <summary>
        /// 操作便利性描述
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 制造质量
    /// </summary>
    public class QualityEntity
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
        /// 制造质量评分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 制造质量总评
        /// </summary>
        public string Viewpoint { get; set; }

        /// <summary>
        /// 质量亮点
        /// </summary>
        public IList<CommonImageVideoEntity> QualityImages { get; set; }

        /// <summary>
        /// 质量亮点描述
        /// </summary>
        public string QualityDescription { get; set; }

        /// <summary>
        /// 工艺亮点
        /// </summary>
        public IList<CommonImageVideoEntity> TechnologyImages { get; set; }

        /// <summary>
        /// 工艺亮点描述
        /// </summary>
        public string TechnologyDescription { get; set; }
    }
}
