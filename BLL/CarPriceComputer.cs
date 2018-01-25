using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;

namespace BitAuto.CarChannel.BLL
{
    public class CarPriceComputer
    {
        private int m_carId;
        private EnumCollection.CarInfoForCarSummary m_carInfo;
        private double m_carExhaust;
        private bool m_isGuochan;
        private int m_seatNum;
        private int m_carPrice;
        private int m_carTotalPrice;
        private string m_Traveltaxrelief;
        private string m_fuelType;

        #region 属性 edit by wangzd Aug.25.2015

        /// <summary>
        /// 裸车价格
        /// </summary>
        public double CarPrice
        {
            get { return m_carPrice; }
        }

        /// <summary>
        /// 排量(L)
        /// </summary>
        public double CarExhaustFloat
        {
            get { return m_carExhaust; }
        }

        /// <summary>
        /// 车船税减免
        /// </summary>
        public string Traveltaxrelief
        {
            get { return m_Traveltaxrelief; }
        }

        /// <summary>
        /// 燃料类型
        /// </summary>
        public string FuelType
        {
            get
            {
                return m_fuelType;
            }
        }

        public EnumCollection.CarInfoForCarSummary CarEntity
        {
            get { return m_carInfo; }
        }

        #endregion

        #region 贷款
        // 贷款首付比例
        private double m_LoanPayments = 0.3;
        // 贷款年
        private int m_LoanPaymentYear = 2;
        // 首付
        private int m_FirstDownPayments;
        // 月还款
        private int m_MonthPayments;
        #endregion

        public CarPriceComputer(int carId)
        {
            PageBase pBase = new PageBase();
            m_carId = carId;
            m_carInfo = pBase.GetCarInfoForCarSummaryByCarID(carId);
            if (m_carInfo.CarID > 0)
            {
                // modified by chengl Dec.3.2009
                double m_carPriceTemp = 0;
                if (double.TryParse(m_carInfo.ReferPrice, out m_carPriceTemp))
                { }
                m_carPrice = (int)(m_carPriceTemp * 10000);
                // m_carPrice = (int)(Convert.ToDouble(m_carInfo.ReferPrice) * 10000);

                double m_carExhaustTemp = 0;
                if (double.TryParse(m_carInfo.Engine_Exhaust.Replace("L", ""), out m_carExhaustTemp))
                { }
                m_carExhaust = m_carExhaustTemp;
                int carExhaust = 0;
                double refP = 0.0;
                pBase.GetCarCountryEngineAndSeatNumByCarID(carId, out m_isGuochan, out carExhaust, out m_seatNum, out refP);
                // 车船税减免 add by chengl Mar.15
                m_Traveltaxrelief = new Car_BasicBll().GetCarParamEx(m_carInfo.CarID, 895);
                // 燃料类型 add by sk 2018-01-23
                m_fuelType = new Car_BasicBll().GetCarParamEx(m_carInfo.CarID, 578);
            }
        }

        /// <summary>
        /// 车型预估总价
        /// </summary>
        public int CarTotalPrice
        {
            get { return m_carTotalPrice; }
        }

        /// <summary>
        /// 购置税
        /// </summary>
        public int AcquisitionTax
        {
            get { return ComputeAcquisitionTax(); }
        }

        /// <summary>
        /// 计算上车牌费用
        /// </summary>
        public int Chepai
        {
            get { return ComputeChepai(); }
        }

        /// <summary>
        /// 车船使用费
        /// </summary>
        public int VehicleTax
        {
            get { return ComputeVehicleTax(); }
        }

        /// <summary>
        /// 交强险
        /// </summary>
        public int Compulsory
        {
            get { return ComputeCompulsory(); }
        }

        /// <summary>
        /// 商业保险
        /// </summary>
        public int Insurance
        {
            get { return ComputeInsurance(); }
        }

        #region 贷款

        /// <summary>
        /// 贷款首付比例
        /// </summary>
        public double LoanPayments
        {
            get { return m_LoanPayments; }
            set
            {
                // 首付比例 百分比
                if (value > 0 && value < 1)
                {
                    m_LoanPayments = value;
                }
            }
        }

        /// <summary>
        /// 贷款年
        /// </summary>
        public int LoanPaymentYear
        {
            get { return m_LoanPaymentYear; }
            set
            {
                // 还款年在 1-5 年
                if (value >= 1 && value <= 5)
                {
                    m_LoanPaymentYear = value;
                }
            }
        }

        /// <summary>
        /// 贷款首付
        /// </summary>
        public int LoanFirstDownPayments
        {
            get { return m_FirstDownPayments; }
        }

        /// <summary>
        /// 贷款月还款
        /// </summary>
        public int LoanMonthPayments
        {
            get { return m_MonthPayments; }
        }

        #endregion

        /// <summary>
        /// 按格式显示的预估总价
        /// </summary>
        public string FormatTotalPrice
        {
            get
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                string price = "";
                double dPrice = ChineseRound(m_carTotalPrice / 100.0) / 100.0;
                if (m_carTotalPrice > 0)
                    price = dPrice.ToString("N", nfi) + "万";
                return price;
            }
        }

        /// <summary>
        /// 按格式显示的裸车价格
        /// </summary>
        public string FormatCarPrice
        {
            get
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                string price = "";
                double dPrice = ChineseRound(m_carPrice / 100.0) / 100.0;
                if (m_carPrice > 0)
                    price = dPrice.ToString("N", nfi) + "万";
                return price;
            }
        }

        /// <summary>
        /// 按格式显示的必要花费
        /// </summary>
        public string FormatEssentialPrice
        {
            get
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                string price = "";
                int essentialPrice = AcquisitionTax + Compulsory + Chepai + VehicleTax;
                double dPrice = ChineseRound(essentialPrice / 100.0) / 100.0;
                if (essentialPrice > 0)
                    price = dPrice.ToString("N", nfi) + "万";
                return price;
            }
        }

        /// <summary>
        /// 按格式显示的商业保险
        /// </summary>
        public string FormatInsurance
        {
            get
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                string price = "";
                double dPrice = ChineseRound(Insurance / 100.0) / 100.0;
                if (Insurance > 0)
                    price = dPrice.ToString("N", nfi) + "万";
                return price;
            }
        }

        /// <summary>
        /// 计算车型预估总价
        /// </summary>
        /// <returns></returns>
        public int ComputeCarPrice()
        {
            int totalPrice = m_carPrice;
            if (m_carPrice != 0)
            {
                //必须部分
                totalPrice += ComputeAcquisitionTax();
                totalPrice += ComputeChepai();
                totalPrice += ComputeVehicleTax();
                totalPrice += ComputeCompulsory();

                //商业保险
                totalPrice += ComputeInsurance();
            }
            m_carTotalPrice = totalPrice;
            return totalPrice;
        }

        /// <summary>
        /// 计算贷款购车
        /// </summary>
        /// <returns></returns>
        public void ComputeCarAutoLoan()
        {
            // 首付款总额=首付款+必要花费+商业保险
            // 必要花费，商业保险同全款购车。
            // 您需要首付=首付款总额；月供=月付款；总计花费=首付款总额+月付款×还款年限×12
            // 比全款购车多花费=总计花费-车辆购置价格。

            if (m_carTotalPrice <= 0 && m_carId > 0)
            {
                // 没有调用计算购车总价时先调用下
                ComputeCarPrice();
            }
            if (m_carTotalPrice > 0 && m_carPrice > 0)
            {
                // 首付=购车价格×首付比例
                m_FirstDownPayments = ChineseRound(Math.Round(m_carPrice * m_LoanPayments));

                // 月还款=贷款额×月利率×（1+月利率）^还款月数/（（1+月利率）^还款月数-1）
                double monthPercent = 0;
                switch (m_LoanPaymentYear)
                {
                    case 1: monthPercent = 0.0631 / 12; break;
                    case 2:
                    case 3: monthPercent = 0.064 / 12; break;
                    case 4:
                    case 5: monthPercent = 0.065 / 12; break;
                    default: break;
                }
                if (monthPercent > 0)
                {
                    m_MonthPayments = ChineseRound(
                        (m_carPrice - m_FirstDownPayments) * monthPercent
                        * Math.Pow((1 + monthPercent), m_LoanPaymentYear * 12) /
                        (Math.Pow((1 + monthPercent), m_LoanPaymentYear * 12) - 1)
                        );
                }
            }
        }

        /// <summary>
        /// 计算购置税
        /// </summary>
        /// <returns></returns>
        private int ComputeAcquisitionTax()
        {
            double tax = m_carPrice / (1 + 0.17);
            if (m_carExhaust <= 1.6)
            {
                if (m_carExhaust < 0.0001)
                {
                    return ChineseRound(0);
                }
                //else
                //{
                //    DateTime dtBegin = Convert.ToDateTime("2017-01-01 00:00:00");
                //    DateTime dtEnd = Convert.ToDateTime("2017-12-31 23:59:59");
                //    DateTime dtNow = DateTime.Now;
                //    if (DateTime.Compare(dtNow, dtBegin) > 0 && DateTime.Compare(dtNow, dtEnd) < 0)
                //    {
                //        return ChineseRound(tax*0.075);
                //    }    
                //}
            }
            return ChineseRound(tax * 0.1);
        }

        /// <summary>
        /// 计算上车牌费用
        /// </summary>
        /// <returns></returns>
        private int ComputeChepai()
        {
            return 500;
        }

        /// <summary>
        /// 车船使用税
        /// </summary>
        /// <returns></returns>
        private int ComputeVehicleTax()
        {
            //Modified By XinLu
            //2012.03.15
            if (m_Traveltaxrelief == "免征") return 0;

            var tax = CalculateNormalVehicleAndVesselTax(m_carExhaust);
            if (m_Traveltaxrelief == "减半")
            {
                tax /= 2;
            }
            //add by sk 2018-01-23
            if (FuelType == "纯电" || FuelType == "插电混合")
            {
                return 0;
            }
            return Convert.ToInt32(Math.Ceiling(tax));
        }

        /// <summary>
        /// 计算正常的车船税
        /// </summary>
        /// <param name="displacement"></param>
        /// <returns></returns>
        private double CalculateNormalVehicleAndVesselTax(double displacement)
        {
            double tax = 0D;
            if (displacement <= 1D)
            {
                tax = 300D;
            }
            else if (displacement <= 1.6D)
            {
                tax = 420D;
            }
            else if (displacement <= 2.0D)
            {
                tax = 480D;
            }
            else if (displacement <= 2.5D)
            {
                tax = 900D;
            }
            else if (displacement <= 3.0D)
            {
                tax = 1920D;
            }
            else if (displacement <= 4.0D)
            {
                tax = 3480D;
            }
            else
            {
                tax = 5280D;
            }

            return tax;
        }

        /// <summary>
        /// 交强险
        /// </summary>
        /// <returns></returns>
        private int ComputeCompulsory()
        {
            int cost = 0;
            // if (m_seatNum <= 6)
            // modified by chengl Nov.29.2011 高总要求
            if (m_seatNum < 6)
                cost = 950;
            else
                cost = 1100;
            return cost;
        }

        #region 保险部分

        /// <summary>
        /// 计算商业保险
        /// </summary>
        /// <returns></returns>
        private int ComputeInsurance()
        {
            int insurance = 0;
            insurance += ComputeTPL();
            insurance += ComputeCarDamage();
            insurance += ComputeAbatement();
            insurance += ComputeCarTheft();
            insurance += ComputeBreakageOfGlass();
            insurance += ComputeSelfignite();
            insurance += ComputeCarEngineDamage();
            insurance += ComputeCarDamageDW();
            insurance += ComputeLimitofDriver();
            insurance += ComputeLimitofPassenger();
            return insurance;
        }

        /// <summary>
        /// 第三方责任险，按20万保额计
        /// </summary>
        /// <returns></returns>
        private int ComputeTPL()
        {
            int cost = 0;
            // modified by wangzd 2015.8.18
            if (m_seatNum < 6)
                cost = 1270;
            else
                cost = 1131;
            return cost;
        }

        /// <summary>
        /// 车辆损失险
        /// </summary>
        /// <returns></returns>
        private int ComputeCarDamage()
        {
            if (m_seatNum < 6)
            {
                return ChineseRound(285 + (m_carPrice * 0.0095));
            }
            if (m_seatNum >= 6 && m_seatNum < 10)
            {
                return ChineseRound(342 + (m_carPrice * 0.009));
            }
            if (m_seatNum >= 10 && m_seatNum < 20)
            {
                return ChineseRound(342 + (m_carPrice * 0.0095));
            }
            if (m_seatNum >= 20)
            {
                return ChineseRound(357 + (m_carPrice * 0.0095));
            }
            return ChineseRound(285 + (m_carPrice * 0.0095));
        }

        /// <summary>
        /// 全车盗抢险
        /// </summary>
        /// <returns></returns>
        private int ComputeCarTheft()
        {
            if (m_seatNum < 6)
            {
                return ChineseRound(120 + (m_carPrice * 0.0049));
            }
            return ChineseRound(140 + (m_carPrice * 0.0044));
        }

        /// <summary>
        /// 玻璃单独破碎险
        /// </summary>
        /// <returns></returns>
        private int ComputeBreakageOfGlass()
        {
            if (m_isGuochan)
            {
                return ChineseRound(m_carPrice * 0.0019);
            }
            else
            {
                if (m_seatNum < 6)
                {
                    return ChineseRound(m_carPrice * 0.0031);
                }
                return ChineseRound(m_carPrice * 0.003);
            }
        }

        /// <summary>
        /// 自燃损失险
        /// </summary>
        /// <returns></returns>
        private int ComputeSelfignite()
        {
            return ChineseRound(m_carPrice * 0.0015);
        }

        /// <summary>
        /// //发动机特别损失险(车损险*5%)
        /// </summary>
        /// <returns></returns>
        private int ComputeCarEngineDamage()
        {
            int carDamage = ComputeCarDamage();
            return ChineseRound(carDamage * 0.05);
        }

        /// <summary>
        /// 不计免赔特约险
        /// </summary>
        /// <returns></returns>
        private int ComputeAbatement()
        {
            int cost = ComputeTPL() + ComputeCarDamage();
            return ChineseRound(cost * 0.2);
        }



        /// <summary>
        /// 司机座位责任险
        /// </summary>
        /// <returns></returns>
        private int ComputeLimitofDriver()
        {
            if (m_seatNum < 6)
            {
                return ChineseRound(20000 * 0.0042);
            }
            return ChineseRound(20000 * 0.004);
        }

        /// <summary>
        /// 乘客座位责任险
        /// </summary>
        /// <returns></returns>
        private int ComputeLimitofPassenger()
        {
            int calCount = 4;
            if (m_seatNum > 4)
            {
                calCount = m_seatNum - 1;
            }
            if (m_seatNum < 6)
            {
                return ChineseRound(20000 * 0.0027 * calCount);
            }
            else
            {
                return ChineseRound(20000 * 0.0027 * calCount);
            }
        }

        /// <summary>
        /// 车身划痕险，按5000计算
        /// </summary>
        /// <returns></returns>
        private int ComputeCarDamageDW()
        {
            int cost = 0;
            if (m_carPrice < 300000)
                cost = 570;
            else if (m_carPrice > 500000)
                cost = 1100;
            else
                cost = 900;
            return cost;
        }

        #endregion

        /// <summary>
        /// 四舍五入一个数值
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private int ChineseRound(double num)
        {
            int retNum = 0;
            double minNum = Math.Floor(num);
            if ((minNum * 2 + 1) / 2 == num)
                retNum = (int)minNum + 1;
            else
                retNum = (int)Math.Round(num);
            return retNum;
        }

    }
}
