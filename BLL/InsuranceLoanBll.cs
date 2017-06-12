using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Extensions;
using System.Xml.Linq;
using System.IO;

namespace BitAuto.CarChannel.BLL
{
    public class InsuranceLoanBll
    {
        private static readonly InsuranceLoanDal InsuranceLoanDal = new InsuranceLoanDal();

        /// <summary>
        /// 获取车贷套餐数据
        /// </summary>
        /// <param name="id">套餐ID</param>
        /// <param name="payRate">首付比例</param>
        /// <returns>套餐数据</returns>
        public LoanPackage GetLoanPackage(int id, decimal payRate)
        {
            return InsuranceLoanDal.GetLoanPackage(id, payRate);
        }

        /// <summary>
        /// 文件不再更新
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, SerialLoanInfo> GetAllSerialLoanList()
        {
            string cacheKey = "InsuranceLoanBll.GetAllSerialLoanList";

            if (HttpRuntime.Cache != null && HttpRuntime.Cache[cacheKey] != null)
            {
                return HttpRuntime.Cache[cacheKey] as Dictionary<int, SerialLoanInfo>;
            }

            string uri = Path.Combine(WebConfig.DataBlockPath, @"Data\InsuranceAndLoan\CalculateAllSerialBrands.xml");

            Dictionary<int, SerialLoanInfo> dic = new Dictionary<int, SerialLoanInfo>();
            using (Stream stream = File.OpenRead(uri))
            {
                XDocument xdoc = XDocument.Load(stream);
                var elements = xdoc.Descendants("SerialBrand");
                foreach (var el in elements)
                {
                    int serialId = el.Element("SerialBrandId").Value<int>();
                    decimal lowestPay = el.Element("LowestMonthlyPayment").Value<decimal>();
                    if (serialId > 0 && !dic.ContainsKey(serialId))
                    {
                        dic.Add(serialId, new SerialLoanInfo() { SerialBrandId = serialId, LowestMonthlyPayment = lowestPay });
                    }
                }
            }

            if (HttpRuntime.Cache != null)
            {
                HttpRuntime.Cache.Insert(cacheKey, dic, null, DateTime.Now.AddDays(1), TimeSpan.Zero);
            }

            return dic;
        }
    }
}
