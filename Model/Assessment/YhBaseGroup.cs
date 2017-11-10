using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.Assessment
{
    public class YhBaseGroup
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
        public EditorCommentEntity EcEntity { get; set; }
        /// <summary>
        /// 油耗总结
        /// </summary>
        public YhGeneralEntity YgEntity { get; set; }
        /// <summary>
        /// 空气质量测试
        /// </summary>
        public YhAirEntity YaEntity { get; set; }
        /// <summary>
        /// 知识点
        /// </summary>
        public IList<WikiEntity> WikiList { get; set; }

    }
    public class YhAirEntity
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
        /// 空气质量评分
        /// </summary>
        public int AirQualityScore { get; set; }
        /// <summary>
        /// 空气质量总评
        /// </summary>
        public string AirQualityGeneralCmt { get; set; }
        /// <summary>
        /// 图片集（暂停使用该字段）
        /// </summary>
        public List<string> AirQualityPics = new List<string>();
        /// <summary>
        /// 空气质量区 亮点小图集合1
        /// </summary>
        public IList<CommonImageVideoEntity> AirPointSmallImgs1 { get; set; }
        /// <summary>
        /// 空气质量区 亮点小图集合2
        /// </summary>
        public IList<CommonImageVideoEntity> AirPointSmallImgs2 { get; set; }
        /// <summary>
        /// 空气质量区 亮点大图标题
        /// </summary>
        public string AirQualityPointTitle { get; set; }
        /// <summary>
        /// 空气质量区 亮点大图集合
        /// </summary>
        public IList<CommonImageVideoEntity> AirPointBigImgsUrl { get; set; }
        /// <summary>
        /// 空气质量区 备注内容
        /// </summary>
        public string AirQualityRemarks { get; set; }
    }
    public class YhGeneralEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// 油耗及环保组下 “油耗”部分
        /// <summary>
        ///低速油耗评分 
        /// </summary>
        public int LowSpeedYhScore { get; set; }
        /// <summary>
        /// 高速油耗评分
        /// </summary>
        public int HighSpeedYhScore { get; set; }
        /// <summary>
        ///工信部油耗评分（MIIT  工信部简写）
        /// </summary>
        public int MiitYhScore { get; set; }
        /// <summary>
        /// 油耗总评
        /// </summary>
        public string YhGeneralCmt { get; set; }
        /// <summary>
        /// 平均车速1
        /// </summary>
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float AverageSpeed1 { get; set; }
        /// <summary>
        /// 平均油耗1
        /// </summary>
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float AverageYh1 { get; set; }
        /// <summary>
        /// 车辆状态1
        /// </summary>
        public string CarStatus1 { get; set; }
        /// <summary>
        /// 线路1
        /// </summary>
        public string RouteDesc1 { get; set; }
        /// <summary>
        /// 平均车速2
        /// </summary>
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float AverageSpeed2 { get; set; }
        /// <summary>
        /// 平均油耗2
        /// </summary>
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float AverageYh2 { get; set; }
        /// <summary>
        /// 车辆状态2
        /// </summary>
        public string CarStatus2 { get; set; }
        /// <summary>
        /// 线路2
        /// </summary>
        public string RouteDesc2 { get; set; }
        /// <summary>
        /// 油耗备注区
        /// </summary>
        public string YhRemarks { get; set; }
    }
}
