using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.Assessment
{
    public class GeneralBaseGroup
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
        /// 总结 
        /// </summary>
        public GeneralSummaryEntity GSEntity { get; set; }
        /// <summary>
        /// 知识点
        /// </summary>
        public IList<WikiEntity> WikiList { get; set; }
    }
    public class GeneralSummaryEntity 
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
        /// 总结组上传图片集
        /// </summary>
        public List<string> GeneralPics = new List<string>();
        /// <summary>
        /// 总结组评论区
        /// </summary>
        public string GeneralComment { get; set; }

    }
}
