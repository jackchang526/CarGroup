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

        #region ���� edit by wangzd Aug.25.2015

        /// <summary>
        /// �㳵�۸�
        /// </summary>
        public double CarPrice
        {
            get { return m_carPrice; }
        }

        /// <summary>
        /// ����(L)
        /// </summary>
        public double CarExhaustFloat
        {
            get { return m_carExhaust; }
        }

        /// <summary>
        /// ����˰����
        /// </summary>
        public string Traveltaxrelief
        {
            get { return m_Traveltaxrelief; }
        }

        /// <summary>
        /// ȼ������
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

        #region ����
        // �����׸�����
        private double m_LoanPayments = 0.3;
        // ������
        private int m_LoanPaymentYear = 2;
        // �׸�
        private int m_FirstDownPayments;
        // �»���
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
                // ����˰���� add by chengl Mar.15
                m_Traveltaxrelief = new Car_BasicBll().GetCarParamEx(m_carInfo.CarID, 895);
                // ȼ������ add by sk 2018-01-23
                m_fuelType = new Car_BasicBll().GetCarParamEx(m_carInfo.CarID, 578);
            }
        }

        /// <summary>
        /// ����Ԥ���ܼ�
        /// </summary>
        public int CarTotalPrice
        {
            get { return m_carTotalPrice; }
        }

        /// <summary>
        /// ����˰
        /// </summary>
        public int AcquisitionTax
        {
            get { return ComputeAcquisitionTax(); }
        }

        /// <summary>
        /// �����ϳ��Ʒ���
        /// </summary>
        public int Chepai
        {
            get { return ComputeChepai(); }
        }

        /// <summary>
        /// ����ʹ�÷�
        /// </summary>
        public int VehicleTax
        {
            get { return ComputeVehicleTax(); }
        }

        /// <summary>
        /// ��ǿ��
        /// </summary>
        public int Compulsory
        {
            get { return ComputeCompulsory(); }
        }

        /// <summary>
        /// ��ҵ����
        /// </summary>
        public int Insurance
        {
            get { return ComputeInsurance(); }
        }

        #region ����

        /// <summary>
        /// �����׸�����
        /// </summary>
        public double LoanPayments
        {
            get { return m_LoanPayments; }
            set
            {
                // �׸����� �ٷֱ�
                if (value > 0 && value < 1)
                {
                    m_LoanPayments = value;
                }
            }
        }

        /// <summary>
        /// ������
        /// </summary>
        public int LoanPaymentYear
        {
            get { return m_LoanPaymentYear; }
            set
            {
                // �������� 1-5 ��
                if (value >= 1 && value <= 5)
                {
                    m_LoanPaymentYear = value;
                }
            }
        }

        /// <summary>
        /// �����׸�
        /// </summary>
        public int LoanFirstDownPayments
        {
            get { return m_FirstDownPayments; }
        }

        /// <summary>
        /// �����»���
        /// </summary>
        public int LoanMonthPayments
        {
            get { return m_MonthPayments; }
        }

        #endregion

        /// <summary>
        /// ����ʽ��ʾ��Ԥ���ܼ�
        /// </summary>
        public string FormatTotalPrice
        {
            get
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                string price = "";
                double dPrice = ChineseRound(m_carTotalPrice / 100.0) / 100.0;
                if (m_carTotalPrice > 0)
                    price = dPrice.ToString("N", nfi) + "��";
                return price;
            }
        }

        /// <summary>
        /// ����ʽ��ʾ���㳵�۸�
        /// </summary>
        public string FormatCarPrice
        {
            get
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                string price = "";
                double dPrice = ChineseRound(m_carPrice / 100.0) / 100.0;
                if (m_carPrice > 0)
                    price = dPrice.ToString("N", nfi) + "��";
                return price;
            }
        }

        /// <summary>
        /// ����ʽ��ʾ�ı�Ҫ����
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
                    price = dPrice.ToString("N", nfi) + "��";
                return price;
            }
        }

        /// <summary>
        /// ����ʽ��ʾ����ҵ����
        /// </summary>
        public string FormatInsurance
        {
            get
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                string price = "";
                double dPrice = ChineseRound(Insurance / 100.0) / 100.0;
                if (Insurance > 0)
                    price = dPrice.ToString("N", nfi) + "��";
                return price;
            }
        }

        /// <summary>
        /// ���㳵��Ԥ���ܼ�
        /// </summary>
        /// <returns></returns>
        public int ComputeCarPrice()
        {
            int totalPrice = m_carPrice;
            if (m_carPrice != 0)
            {
                //���벿��
                totalPrice += ComputeAcquisitionTax();
                totalPrice += ComputeChepai();
                totalPrice += ComputeVehicleTax();
                totalPrice += ComputeCompulsory();

                //��ҵ����
                totalPrice += ComputeInsurance();
            }
            m_carTotalPrice = totalPrice;
            return totalPrice;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public void ComputeCarAutoLoan()
        {
            // �׸����ܶ�=�׸���+��Ҫ����+��ҵ����
            // ��Ҫ���ѣ���ҵ����ͬȫ�����
            // ����Ҫ�׸�=�׸����ܶ�¹�=�¸���ܼƻ���=�׸����ܶ�+�¸�����������ޡ�12
            // ��ȫ����໨��=�ܼƻ���-�������ü۸�

            if (m_carTotalPrice <= 0 && m_carId > 0)
            {
                // û�е��ü��㹺���ܼ�ʱ�ȵ�����
                ComputeCarPrice();
            }
            if (m_carTotalPrice > 0 && m_carPrice > 0)
            {
                // �׸�=�����۸���׸�����
                m_FirstDownPayments = ChineseRound(Math.Round(m_carPrice * m_LoanPayments));

                // �»���=�����������ʡ���1+�����ʣ�^��������/����1+�����ʣ�^��������-1��
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
        /// ���㹺��˰
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
        /// �����ϳ��Ʒ���
        /// </summary>
        /// <returns></returns>
        private int ComputeChepai()
        {
            return 500;
        }

        /// <summary>
        /// ����ʹ��˰
        /// </summary>
        /// <returns></returns>
        private int ComputeVehicleTax()
        {
            //Modified By XinLu
            //2012.03.15
            if (m_Traveltaxrelief == "����") return 0;

            var tax = CalculateNormalVehicleAndVesselTax(m_carExhaust);
            if (m_Traveltaxrelief == "����")
            {
                tax /= 2;
            }
            //add by sk 2018-01-23
            if (FuelType == "����" || FuelType == "�����")
            {
                return 0;
            }
            return Convert.ToInt32(Math.Ceiling(tax));
        }

        /// <summary>
        /// ���������ĳ���˰
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
        /// ��ǿ��
        /// </summary>
        /// <returns></returns>
        private int ComputeCompulsory()
        {
            int cost = 0;
            // if (m_seatNum <= 6)
            // modified by chengl Nov.29.2011 ����Ҫ��
            if (m_seatNum < 6)
                cost = 950;
            else
                cost = 1100;
            return cost;
        }

        #region ���ղ���

        /// <summary>
        /// ������ҵ����
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
        /// �����������գ���20�򱣶��
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
        /// ������ʧ��
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
        /// ȫ��������
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
        /// ��������������
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
        /// ��ȼ��ʧ��
        /// </summary>
        /// <returns></returns>
        private int ComputeSelfignite()
        {
            return ChineseRound(m_carPrice * 0.0015);
        }

        /// <summary>
        /// //�������ر���ʧ��(������*5%)
        /// </summary>
        /// <returns></returns>
        private int ComputeCarEngineDamage()
        {
            int carDamage = ComputeCarDamage();
            return ChineseRound(carDamage * 0.05);
        }

        /// <summary>
        /// ����������Լ��
        /// </summary>
        /// <returns></returns>
        private int ComputeAbatement()
        {
            int cost = ComputeTPL() + ComputeCarDamage();
            return ChineseRound(cost * 0.2);
        }



        /// <summary>
        /// ˾����λ������
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
        /// �˿���λ������
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
        /// �������գ���5000����
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
        /// ��������һ����ֵ
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
