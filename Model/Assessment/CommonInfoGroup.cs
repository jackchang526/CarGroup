using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.Assessment
{
    [BsonIgnoreExtraElements]
    /// <summary>
    /// 基本信息组
    /// </summary>
    public class CommonInfoGroup
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
        /// 基本信息
        /// </summary>
        public CommonInformationEntity CommonInformationEntity { get; set; }
        /// <summary>
        /// 知识点
        /// </summary>
        public IList<WikiEntity> WikiList { get; set; }
    }

    /// <summary>
    /// 基本信息
    /// </summary>
    public class CommonInformationEntity
    {
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 头图
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 主图
        /// </summary>
        public string MainImageUrl { get; set; }
        
        /// <summary>
        /// 导语
        /// </summary>
        public string Preface { get; set; }
        /// <summary>
        /// 介绍视频
        /// </summary>
        public string VideoUrl { get; set; }
        /// <summary>
        /// 直播回放
        /// </summary>
        public string LiveUrl { get; set; }

        /// <summary>
        /// 自定义评测报告标题
        /// </summary>
        public string Title { get; set; }
    }
}
