using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.CarChannel.Model;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common.Extensions;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.DAL
{
    public class InsuranceLoanDal
    {
        /// <summary>
        /// 获取车贷套餐数据
        /// </summary>
        /// <param name="id">套餐ID</param>
        /// <param name="payRate">首付比例</param>
        /// <returns>套餐数据</returns>
        public static LoanPackage GetLoanPackage(int id, decimal payRate)
        {
            string commandText =
@"SELECT  a.Id, a.Name, StartDate, ExpiryDate, RepaymentPeriod, FinalPaymentType, FinalPaymentRate, Fee, ManagementFee, PromotionTitle, PromotionMessage,
        ApprovalDuration, PrepaymentMessage, RepaymentWay, OrderNumber, Condition, Requirement,b.ChineseName [CompanyName],b.ChineseAbbreviation, c.InterestRate
FROM    LoanPackage a INNER JOIN dbo.CompanyInfo b ON a.CompanyId=b.CompanyId
LEFT JOIN [LoanDownPayment] c ON a.Id=c.PackageId AND c.DownPaymentRate=@payrate
WHERE   a.Id = @id
SELECT a.Id,a.Name,b.Description FROM dbo.LoanPackageFeature a INNER JOIN dbo.LoanPackageFeatureDescription b ON a.Id=b.FeatureId WHERE b.PackageId=@id";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(WebConfig.InsuranceLoanConnectionString, CommandType.Text, commandText, new SqlParameter[] { new SqlParameter("@Id", id), new SqlParameter("@payrate", payRate) { SqlDbType = SqlDbType.Decimal, Precision = 10, Scale = 10 } }))
            {
                LoanPackage model = null;

                if (reader.HasRows)
                {
                    reader.Read();

                    HashSet<string> columns = reader.GetColums();
                    model = new LoanPackage()
                    {
                        Id = reader.GetValueOrDefault<int>("Id", columns),
                        Name = reader.GetValueOrDefault<string>("Name", columns),
                        Company = new Company()
                        {
                            CompanyId = reader.GetValueOrDefault<int>("CompanyId", columns),
                            CompanyName = reader.GetValueOrDefault<string>("CompanyName", columns),
                            ChineseAbbreviation = reader.GetValueOrDefault<string>("ChineseAbbreviation", columns),
                        },
                        DownPaymentRate = payRate,
                        InterestRate = reader.GetValueOrDefault<decimal>("InterestRate", columns),
                        MonthlyInterestRate = reader.GetValueOrDefault<decimal>("InterestRate", columns) / 12,
                        FinalPaymentRate = reader.GetValueOrDefault<decimal>("FinalPaymentRate", columns),
                        PromotionTitle = reader.GetValueOrDefault<string>("PromotionTitle", columns),
                        PromotionMessage = reader.GetValueOrDefault<string>("PromotionMessage", columns),
                        RepaymentPeriod = reader.GetValueOrDefault<int>("RepaymentPeriod", columns),
                        RepaymentWay = reader.GetValueOrDefault<string>("RepaymentWay", columns),
                        ApprovalDuration = reader.GetValueOrDefault<int>("ApprovalDuration", columns),
                        PrepaymentMessage = reader.GetValueOrDefault<string>("PrepaymentMessage", columns),
                        Condition = reader.GetValueOrDefault<string>("Condition", columns),
                        Requirement = reader.GetValueOrDefault<string>("Requirement", columns),
                    };

                    if (reader.NextResult() && reader.HasRows)
                    {
                        columns = reader.GetColums();

                        model.Features = new List<PackageFeature>();
                        while (reader.Read())
                        {
                            model.Features.Add(new PackageFeature()
                            {
                                Id = reader.GetValueOrDefault<int>("Id", columns),
                                Name = reader.GetValueOrDefault<string>("Name", columns),
                                Description = reader.GetValueOrDefault<string>("Description", columns),
                            });
                        }
                    }
                }

                reader.Close();

                return model;
            }
        }
    }
}
