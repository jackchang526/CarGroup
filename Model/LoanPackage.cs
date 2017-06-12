using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 套餐信息
    /// </summary>
    public class LoanPackage
    {
        /// <summary>
        /// 套餐Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 套餐名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 促销信息标题
        /// </summary>
        public string PromotionTitle { get; set; }

        /// <summary>
        /// 促销信息标题
        /// </summary>
        public string PromotionMessage { get; set; }

        /// <summary>
        /// 首付比例
        /// </summary>
        public decimal DownPaymentRate { get; set; }

        /// <summary>
        /// 贷款期限(单位：月)
        /// </summary>
        public int RepaymentPeriod { get; set; }

        /// <summary>
        /// 尾款比例
        /// </summary>
        public decimal FinalPaymentRate { get; set; }

        /// <summary>
        /// 年利率
        /// </summary>
        public decimal InterestRate { get; set; }

        /// <summary>
        /// 月利率
        /// </summary>
        public decimal MonthlyInterestRate { get; set; }

        /// <summary>
        /// 总利息
        /// </summary>
        public decimal TotalInterest { get; set; }

        /// <summary>
        /// 月供
        /// </summary>
        public decimal MonthlyPayment { get; set; }

       /// <summary>
        /// 还款方式
        /// </summary>
        public string RepaymentWay { get; set; }

        /// <summary>
        /// 审批时长（单位：天）
        /// </summary>
        public int ApprovalDuration { get; set; }

        /// <summary>
        /// 提前还款
        /// </summary>
        public string PrepaymentMessage { get; set; }

        /// <summary>
        /// 申请条件
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// 所需材料
        /// </summary>
        public string Requirement { get; set; }

        /// <summary>
        /// 套餐特点
        /// </summary>
        public List<PackageFeature> Features { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public Company Company { get; set; }

    }

    /// <summary>
    /// 套餐特点
    /// </summary>
    public class PackageFeature
    {
        /// <summary>
        /// 套餐特点Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 套餐特点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 套餐特点描述
        /// </summary>
        public string Description { get; set; }
    }

    public class Company
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string ChineseAbbreviation { get; set; }
    }
}
