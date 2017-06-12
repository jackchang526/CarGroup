using System;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// ���ͻ�����Ϣ���伶����Ϣ
    /// </summary>
    [Serializable]
    public class Car_BasicEntity
    {
        #region ��������
        ///<summary>
        /// ����ID
        ///</summary>
        private int _car_Id;

        ///<summary>
        /// ��������״̬
        ///</summary>
        private string _car_ProduceState = String.Empty;

        ///<summary>
        /// ��������״̬
        ///</summary>
        private string _car_SaleState = String.Empty;

        ///<summary>
        /// ����������Ʒ��ID
        ///</summary>
        private int _cs_Id;

        ///<summary>
        /// ������
        ///</summary>
        private string _car_Name = String.Empty;

        ///<summary>
        /// ��������ĸƴд
        ///</summary>
        private string _car_spellFirst = String.Empty;

        ///<summary>
        /// ���ͳ���ָ����
        ///</summary>
        private double _car_ReferPrice;

        ///<summary>
        /// ����״̬
        ///</summary>
        private int _car_isState;

        ///<summary>
        /// �����Ƿ�����
        ///</summary>
        private int _car_isLock;

        ///<summary>
        /// ���Ͷ�Ӧ�Ͽ�ID
        ///</summary>
        private long _car_oLdCar_Id;

        ///<summary>
        /// ���ʹ���ʱ��
        ///</summary>
        private DateTime _car_createTime;

        ///<summary>
        /// ���͸���ʱ��
        ///</summary>
        private DateTime _car_updateTime;

        /// <summary>
        /// �������
        /// </summary>
        private int _car_YearType;

        /// <summary>
        /// ����������Ʒ����
        /// </summary>
        private string _cs_Name = string.Empty;

        /// <summary>
        /// ����������Ʒ����ʾ��
        /// </summary>
        private string _cs_ShowName = string.Empty;

        /// <summary>
        /// ����������Ʒ�Ƶ�ȫƴ(��ַ��д)
        /// </summary>
        private string _cs_AllSpell = string.Empty;

        /// <summary>
        /// ��������Ʒ��ID
        /// </summary>
        private int _cb_id;

        /// <summary>
        /// ��������Ʒ����
        /// </summary>
        private string _cb_Name = string.Empty;

        /// <summary>
        /// ������������ID
        /// </summary>
        private int _cp_id;

        /// <summary>
        /// ��������������
        /// </summary>
        private string _cp_Name = string.Empty;

		/// <summary>
		/// ������Ʒ������
		/// </summary>
		private string _masterName = string.Empty;
        #endregion

        #region ���캯��
        ///<summary>
        ///
        ///</summary>
        public Car_BasicEntity()
        {
        }
        ///<summary>
        ///
        ///</summary>
        public Car_BasicEntity
        (
            int car_Id,
            string car_ProduceState,
            string car_SaleState,
            int cs_Id,
            string car_Name,
            string car_spellFirst,
            double car_ReferPrice,
            int car_isState,
            int car_isLock,
            long car_oLdCar_Id,
            DateTime car_createTime,
            DateTime car_updateTime,
            int car_YearType,
            string cs_Name,
            string cs_ShowName,
            string cs_AllSpell,
            int cb_id,
            string cb_Name,
            int cp_id,
            string cp_Name
        )
        {
            _car_Id = car_Id;
            _car_ProduceState = car_ProduceState;
            _car_SaleState = car_SaleState;
            _cs_Id = cs_Id;
            _car_Name = car_Name;
            _car_spellFirst = car_spellFirst;
            _car_ReferPrice = car_ReferPrice;
            _car_isState = car_isState;
            _car_isLock = car_isLock;
            _car_oLdCar_Id = car_oLdCar_Id;
            _car_createTime = car_createTime;
            _car_updateTime = car_updateTime;
            _car_YearType = car_YearType;
            _cs_Name = cs_Name;
            _cs_ShowName = cs_ShowName;
            _cs_AllSpell = cs_AllSpell;
            _cb_id = cb_id;
            _cb_Name = cb_Name;
            _cp_id = cp_id;
            _cp_Name = cp_Name;
        }
        #endregion

        #region ��������

        ///<summary>
        /// ����ID
        ///</summary>
        public int Car_Id
        {
            get { return _car_Id; }
            set { _car_Id = value; }
        }

        ///<summary>
        /// ��������״̬
        ///</summary>
        public string Car_ProduceState
        {
            get { return _car_ProduceState; }
            set { _car_ProduceState = value; }
        }

        ///<summary>
        /// ��������״̬
        ///</summary>
        public string Car_SaleState
        {
            get { return _car_SaleState; }
            set { _car_SaleState = value; }
        }

        ///<summary>
        /// ����������Ʒ��ID
        ///</summary>
        public int Cs_Id
        {
            get { return _cs_Id; }
            set { _cs_Id = value; }
        }

        ///<summary>
        /// ������
        ///</summary>
        public string Car_Name
        {
            get { return _car_Name; }
            set { _car_Name = value; }
        }

        ///<summary>
        /// ��������ĸ
        ///</summary>
        public string Car_SpellFirst
        {
            get { return _car_spellFirst; }
            set { _car_spellFirst = value; }
        }

        ///<summary>
        /// ���ͳ���ָ����
        ///</summary>
        public double Car_ReferPrice
        {
            get { return _car_ReferPrice; }
            set { _car_ReferPrice = value; }
        }

        ///<summary>
        /// ����״̬
        ///</summary>
        public int Car_IsState
        {
            get { return _car_isState; }
            set { _car_isState = value; }
        }

        ///<summary>
        /// �����Ƿ�����
        ///</summary>
        public int Car_IsLock
        {
            get { return _car_isLock; }
            set { _car_isLock = value; }
        }

        ///<summary>
        /// ���Ͷ�Ӧ�Ͽ�ID
        ///</summary>
        public long Car_OLdCar_Id
        {
            get { return _car_oLdCar_Id; }
            set { _car_oLdCar_Id = value; }
        }

        ///<summary>
        /// ���ʹ���ʱ��
        ///</summary>
        public DateTime Car_CreateTime
        {
            get { return _car_createTime; }
            set { _car_createTime = value; }
        }

        ///<summary>
        /// ���͸���ʱ��
        ///</summary>
        public DateTime Car_UpdateTime
        {
            get { return _car_updateTime; }
            set { _car_updateTime = value; }
        }

        /// <summary>
        /// �������
        /// </summary>
        public int Car_YearType
        {
            get { return _car_YearType; }
            set { _car_YearType = value; }
        }

        /// <summary>
        /// ����������Ʒ����
        /// </summary>
        public string Cs_Name
        {
            get { return _cs_Name; }
            set { _cs_Name = value; }
        }

        /// <summary>
        /// ����������Ʒ����ʾ��
        /// </summary>
        public string Cs_ShowName
        {
            get { return _cs_ShowName; }
            set { _cs_ShowName = value; }
        }

        /// <summary>
        /// ����������Ʒ��ȫƴ(��ַ��д)
        /// </summary>
        public string Cs_AllSpell
        {
            get { return _cs_AllSpell; }
            set { _cs_AllSpell = value; }
        }

        /// <summary>
        /// ��������Ʒ��ID
        /// </summary>
        public int Cb_id
        {
            get { return _cb_id; }
            set { _cb_id = value; }
        }

        /// <summary>
        /// ��������Ʒ����
        /// </summary>
        public string Cb_Name
        {
            get { return _cb_Name; }
            set { _cb_Name = value; }
        }

        /// <summary>
        /// ������������ID
        /// </summary>
        public int Cp_id
        {
            get { return _cp_id; }
            set { _cp_id = value; }
        }

        /// <summary>
        /// ��������������
        /// </summary>
        public string Cp_Name
        {
            get { return _cp_Name; }
            set { _cp_Name = value; }
        }

		/// <summary>
		/// ��Ʒ������
		/// </summary>
		public string MasterName
		{
			get { return _masterName; }
			set { _masterName = value; }
		}
        #endregion
    }
}
