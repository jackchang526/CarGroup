using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.Assessment
{ 
    public class CostBaseGroup
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
        /// 厂商指导价
        /// </summary>
        public CostProducerEntity CpEntity { get; set; }
        /// <summary>
        /// 6年用车费用
        /// </summary>
        public CostSixYearEntity CsyEntity { get; set; }
        /// <summary>
        /// 质保期
        /// </summary>
        public CostGuaranPeriodEntity CgpEntity { get; set; }
        /// <summary>
        /// 保值率
        /// </summary>
        public CostGuaranQualityEntity CgqEntity { get; set; }
        /// <summary>
        /// 知识点
        /// </summary>
        public IList<WikiEntity> WikiList { get; set; }
    }
    public class CostGuaranPeriodEntity 
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
        /// 质保期评分
        /// </summary>
        public int GuaranPeriodScore { get; set; }

        /// <summary>
        /// 质保期总评
        /// </summary>
        public string GuaranPeriodCmt { get; set; }

    }
    public class CostGuaranQualityEntity 
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
        /// 保值评分
        /// </summary>
        public int GuaranQualityScore { get; set; }
        /// <summary>
        /// 保值总评
        /// </summary>
        public string GuaranQualityCmt { get; set; }

        /// <summary>
        /// 5年保值率下方备注内容
        /// </summary>
        public string RemarksOf5Years { get; set; }
      
    }
    public class CostProducerEntity 
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
        /// 厂商指导价评分
        /// </summary>
        public int ProducerPriceScore { get; set; }
        /// <summary>
        /// 厂商指导价总评
        /// </summary>
        public string ProducerPriceGeneralCmt { get; set; }
      
        /// <summary>
        /// 厂商指导价下方评论框内容
        /// </summary>
        public string AboutPayCmt { get; set; }
    }
    public class CostSixYearEntity 
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
        /// 燃油费用评分
        /// </summary>
        public int OilPayScore { get; set; }
        /// <summary>
        /// 保险费用评分
        /// </summary>
        public int InsuranceExpenseScore { get; set; }
        /// <summary>
        /// 保养费用评分
        /// </summary>
        public int MaintenanceCostScore { get; set; }
        /// <summary>
        /// 6年用车费用总评
        /// </summary>
        public string SixYearExperCmt { get; set; }
    }
}
