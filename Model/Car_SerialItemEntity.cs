using System;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// ��Ʒ����չ��Ϣ
    /// </summary>
    [Serializable]
    public class Car_SerialItemEntity
    {
        #region ��������
        ///<summary>
        /// ��Ʒ��ID
        ///</summary>
        private int _cs_Id;
        ///<summary>
        /// ��Ʒ�Ƴ���
        ///</summary>
        private string _body_Doors = String.Empty;
        ///<summary>
        /// ��Ʒ������
        ///</summary>
        private string _engine_Exhaust = String.Empty;
        ///<summary>
        /// ��Ʒ�Ƶ�λ&����������
        ///</summary>
        private string _underPan_Num_Type = String.Empty;
        ///<summary>
        /// ��Ʒ�Ʊ���
        ///</summary>
        private string _prices = String.Empty;
        ///<summary>
        /// ��Ʒ������ʱ��
        ///</summary>
        private string _car_MarketDate = String.Empty;
        ///<summary>
        /// ��Ʒ�Ʊ�������
        ///</summary>
        private string _pricesRange = String.Empty;
        ///<summary>
        /// ��Ʒ�Ƴ���ָ��������
        ///</summary>
        private string _referPriceRange = String.Empty;

        /// <summary>
        /// ���ʱ�����䣨���䣩
        /// </summary>
        private string _normalChargeTime = String.Empty;

        /// <summary>
        /// �����������
        /// </summary>
        private string _batteryLife = String.Empty;

        #endregion

        #region ���캯��
        ///<summary>
        ///
        ///</summary>
        public Car_SerialItemEntity()
        {
        }
        ///<summary>
        ///
        ///</summary>
        public Car_SerialItemEntity
        (
            int cs_Id,
            string body_Doors,
            string engine_Exhaust,
            string underPan_Num_Type,
            string prices,
            string car_MarketDate,
            string pricesRange,
            string referPriceRange
        )
        {
            _cs_Id = cs_Id;
            _body_Doors = body_Doors;
            _engine_Exhaust = engine_Exhaust;
            _underPan_Num_Type = underPan_Num_Type;
            _prices = prices;
            _car_MarketDate = car_MarketDate;
            _pricesRange = pricesRange;
            _referPriceRange = referPriceRange;
        }

        #endregion

        #region ��������

        ///<summary>
        /// ��Ʒ��ID
        ///</summary>
        public int cs_Id
        {
            get { return _cs_Id; }
            set { _cs_Id = value; }
        }

        ///<summary>
        /// ��Ʒ�Ƴ�����ʽ
        ///</summary>
        public string Body_Doors
        {
            get { return _body_Doors; }
            set { _body_Doors = value; }
        }

        ///<summary>
        /// ��Ʒ������
        ///</summary>
        public string Engine_Exhaust
        {
            get { return _engine_Exhaust; }
            set { _engine_Exhaust = value; }
        }

        ///<summary>
        /// ��Ʒ�Ʊ�����
        ///</summary>
        public string UnderPan_Num_Type
        {
            get { return _underPan_Num_Type; }
            set { _underPan_Num_Type = value; }
        }

        ///<summary>
        /// ��Ʒ�Ʊ���
        ///</summary>
        public string Prices
        {
            get { return _prices; }
            set { _prices = value; }
        }

        ///<summary>
        /// ��Ʒ������ʱ��
        ///</summary>
        public string Car_MarketDate
        {
            get { return _car_MarketDate; }
            set { _car_MarketDate = value; }
        }

        private string _car_RepairPolicy = string.Empty;
        public string Car_RepairPolicy
        {
            get { return _car_RepairPolicy; }
            set { _car_RepairPolicy = value; }
        }


        ///<summary>
        /// ��Ʒ�Ʊ�������
        ///</summary>
        public string PricesRange
        {
            get { return _pricesRange; }
            set { _pricesRange = value; }
        }

        ///<summary>
        /// ��Ʒ��ָ��������
        ///</summary>
        public string ReferPriceRange
        {
            get { return _referPriceRange; }
            set { _referPriceRange = value; }
        }

        /// <summary>
        /// ���ʱ�����䣨���䣩
        /// </summary>
        public string NormalChargeTime
        {
            get { return _normalChargeTime; }
            set { _normalChargeTime = value; }
        }

        /// <summary>
        /// �����������
        /// </summary>
        public string BatteryLife
        {
            get { return _batteryLife; }
            set { _batteryLife = value; }
        }

        #endregion
    }
}
