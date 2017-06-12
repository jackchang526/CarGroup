using System;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// ��Ʒ�Ƽ�����Ϣ���伶����Ϣ
    /// </summary>
    [Serializable]
    public class Car_SerialEntity
    {
        #region ��������
        ///<summary>
        /// ��Ʒ��ID
        ///</summary>
        private int _cs_Id;
        ///<summary>
        /// Ʒ��ID
        ///</summary>
        private int _cb_Id;
        ///<summary>
        /// ��Ʒ����
        ///</summary>
        private string _cs_Name = String.Empty;
        ///<summary>
        /// ��Ʒ�Ʊ���
        ///</summary>
        private string _cs_OtherName = String.Empty;
        ///<summary>
        /// ��Ʒ��Ӣ����
        ///</summary>
        private string _cs_EName = String.Empty;
        ///<summary>
        /// ��Ʒ��URL
        ///</summary>
        private string _cs_Url = String.Empty;
        ///<summary>
        /// ��Ʒ�Ƶ绰
        ///</summary>
        private string _cs_Phone = String.Empty;
        ///<summary>
        /// ��Ʒ�Ƽ��
        ///</summary>
        private string _cs_Introduction = String.Empty;
        ///<summary>
        /// ��Ʒ�Ʊ�ǩ
        ///</summary>
        private string _cs_Tag = String.Empty;
        ///<summary>
        /// ��Ʒ��ͼƬ
        ///</summary>
        private string _cs_Photo = String.Empty;
        ///<summary>
        /// ��Ʒ���е�
        ///</summary>
        private string _cs_Virtues = String.Empty;
        ///<summary>
        /// ��Ʒ��ȱ��
        ///</summary>
        private string _cs_Defect = String.Empty;
        ///<summary>
        /// ��Ʒ������ĸƴд
        ///</summary>
        private string _cs_spell = String.Empty;
        ///<summary>
        /// ��Ʒ�ƴ���ʱ��
        ///</summary>
        private DateTime _cs_createTime;
        ///<summary>
        /// ��Ʒ����Ҫ�̶�
        ///</summary>
        private string _cs_CarType = String.Empty;
        ///<summary>
        /// ��Ʒ�Ƽ���
        ///</summary>
        private string _cs_CarLevel = String.Empty;
        ///<summary>
        /// ��Ʒ��״̬
        ///</summary>
        private int _cs_isState;
        ///<summary>
        /// ��Ʒ�ƶ�Ӧ�Ͽ�ID
        ///</summary>
        private int _cs_oldCb_Id;
        ///<summary>
        /// ��Ʒ�Ƹ���ʱ��
        ///</summary>
        private DateTime _cs_updateTime;
        ///<summary>
        /// ��Ʒ������״̬
        ///</summary>
        private string _cs_SaleState = String.Empty;
        ///<summary>
        /// ��Ʒ����ʾ����
        ///</summary>
        private string _cs_ShowName = String.Empty;
        ///<summary>
        /// ��Ʒ��ȫƴ
        ///</summary>
        private string _cs_allSpell = String.Empty;
        /// <summary>
        /// ��Ʒ��SEOName
        /// </summary>
        private string _cs_SeoName = String.Empty;

        /// <summary>
        /// ��Ʒ������Ʒ����
        /// </summary>
        private string _cb_Name = string.Empty;
        /// <summary>
        /// Ʒ��AllSpell
        /// </summary>
        private string _cb_AllSpell = string.Empty;

		/// <summary>
		/// ������Ʒ������
		/// </summary>
		private string _masterName = string.Empty;

        /// <summary>
        /// ������Ʒ��URL
        /// </summary>
        private string _masterURL = string.Empty;

        /// <summary>
        /// ��Ʒ����������ID
        /// </summary>
        private int _cp_id;

        /// <summary>
        /// ��Ʒ������������
        /// </summary>
        private string _cp_Name = string.Empty;

		/// <summary>
		/// ��Ʒ���������̶�����
		/// </summary>
		private string _cpShortName = string.Empty;
        #endregion

        #region ���캯��

        ///<summary>
        ///
        ///</summary>
        public Car_SerialEntity()
        {
        }
        ///<summary>
        ///
        ///</summary>
        public Car_SerialEntity
        (
            int cs_Id,
            int cb_Id,
            string cs_Name,
            string cs_OtherName,
            string cs_EName,
            string cs_Url,
            string cs_Phone,
            string cs_Introduction,
            string cs_Tag,
            string cs_Photo,
            string cs_Virtues,
            string cs_Defect,
            string cs_spell,
            DateTime cs_createTime,
            string cs_CarType,
            string cs_CarLevel,
            int cs_isState,
            int cs_oldCb_Id,
            DateTime cs_updateTime,
            string cs_SaleState,
            string cs_ShowName,
            string cs_allSpell,
            string cb_Name,
            int cp_id,
            string cp_Name,
            string masterName,
            string masterURL
        )
        {
            _cs_Id = cs_Id;
            _cb_Id = cb_Id;
            _cs_Name = cs_Name;
            _cs_OtherName = cs_OtherName;
            _cs_EName = cs_EName;
            _cs_Url = cs_Url;
            _cs_Phone = cs_Phone;
            _cs_Introduction = cs_Introduction;
            _cs_Tag = cs_Tag;
            _cs_Photo = cs_Photo;
            _cs_Virtues = cs_Virtues;
            _cs_Defect = cs_Defect;
            _cs_spell = cs_spell;
            _cs_createTime = cs_createTime;
            _cs_CarType = cs_CarType;
            _cs_CarLevel = cs_CarLevel;
            _cs_isState = cs_isState;
            _cs_oldCb_Id = cs_oldCb_Id;
            _cs_updateTime = cs_updateTime;
            _cs_SaleState = cs_SaleState;
            _cs_ShowName = cs_ShowName;
            _cs_allSpell = cs_allSpell;
            _cb_Name = cb_Name;
            _cp_id = cp_id;
            _cp_Name = cp_Name;
            _masterName = masterName;
            _masterURL = masterURL;
        }
        
        #endregion

        #region ��������

        ///<summary>
        /// ��Ʒ��ID
        ///</summary>
        public int Cs_Id
        {
            get { return _cs_Id; }
            set { _cs_Id = value; }
        }

        ///<summary>
        /// Ʒ��ID
        ///</summary>
        public int Cb_Id
        {
            get { return _cb_Id; }
            set { _cb_Id = value; }
        }

        ///<summary>
        /// ��Ʒ����
        ///</summary>
        public string Cs_Name
        {
            get { return _cs_Name; }
            set { _cs_Name = value; }
        }

        ///<summary>
        /// ��Ʒ�Ʊ���
        ///</summary>
        public string Cs_OtherName
        {
            get { return _cs_OtherName; }
            set { _cs_OtherName = value; }
        }

        ///<summary>
        /// ��Ʒ��Ӣ����
        ///</summary>
        public string Cs_EName
        {
            get { return _cs_EName; }
            set { _cs_EName = value; }
        }

        ///<summary>
        /// ��Ʒ��URL
        ///</summary>
        public string Cs_Url
        {
            get { return _cs_Url; }
            set { _cs_Url = value; }
        }

        ///<summary>
        /// ��Ʒ�Ƶ绰
        ///</summary>
        public string Cs_Phone
        {
            get { return _cs_Phone; }
            set { _cs_Phone = value; }
        }

        ///<summary>
        /// ��Ʒ�Ƽ��
        ///</summary>
        public string Cs_Introduction
        {
            get { return _cs_Introduction; }
            set { _cs_Introduction = value; }
        }

        ///<summary>
        /// ��Ʒ�Ʊ�ǩ
        ///</summary>
        public string Cs_Tag
        {
            get { return _cs_Tag; }
            set { _cs_Tag = value; }
        }

        ///<summary>
        /// ��Ʒ��ͼƬ
        ///</summary>
        public string Cs_Photo
        {
            get { return _cs_Photo; }
            set { _cs_Photo = value; }
        }

        ///<summary>
        /// ��Ʒ���ŵ�
        ///</summary>
        public string Cs_Virtues
        {
            get { return _cs_Virtues; }
            set { _cs_Virtues = value; }
        }

        ///<summary>
        /// ��Ʒ��ȱ��
        ///</summary>
        public string Cs_Defect
        {
            get { return _cs_Defect; }
            set { _cs_Defect = value; }
        }

        ///<summary>
        /// ��Ʒ������ĸƴд
        ///</summary>
        public string Cs_Spell
        {
            get { return _cs_spell; }
            set { _cs_spell = value; }
        }

        ///<summary>
        /// ��Ʒ�ƴ���ʱ��
        ///</summary>
        public DateTime Cs_CreateTime
        {
            get { return _cs_createTime; }
            set { _cs_createTime = value; }
        }

        ///<summary>
        /// ��Ʒ����Ҫ�̶�
        ///</summary>
        public string Cs_CarType
        {
            get { return _cs_CarType; }
            set { _cs_CarType = value; }
        }

        ///<summary>
        /// ��Ʒ�Ƽ���
        ///</summary>
        public string Cs_CarLevel
        {
            get { return _cs_CarLevel; }
            set { _cs_CarLevel = value; }
        }

        ///<summary>
        /// ��Ʒ��״̬
        ///</summary>
        public int Cs_IsState
        {
            get { return _cs_isState; }
            set { _cs_isState = value; }
        }

        ///<summary>
        /// ��Ʒ�ƶ�Ӧ�Ͽ�Ʒ��ID
        ///</summary>
        public int Cs_OldCb_Id
        {
            get { return _cs_oldCb_Id; }
            set { _cs_oldCb_Id = value; }
        }

        ///<summary>
        /// ��Ʒ�Ƹ���ʱ��
        ///</summary>
        public DateTime Cs_UpdateTime
        {
            get { return _cs_updateTime; }
            set { _cs_updateTime = value; }
        }

        ///<summary>
        /// ��Ʒ������״̬
        ///</summary>
        public string Cs_SaleState
        {
            get { return _cs_SaleState; }
            set { _cs_SaleState = value; }
        }

        ///<summary>
        /// ��Ʒ����ʾ����
        ///</summary>
        public string Cs_ShowName
        {
            get { return _cs_ShowName; }
            set { _cs_ShowName = value; }
        }

        ///<summary>
        /// ��Ʒ��ȫƴ
        ///</summary>
        public string Cs_AllSpell
        {
            get { return _cs_allSpell; }
            set { _cs_allSpell = value; }
        }
        /// <summary>
        /// ��Ʒ��SEO��
        /// </summary>
        public string Cs_SeoName
        {
            get { return _cs_SeoName; }
            set { _cs_SeoName = value; }
        }
        /// <summary>
        /// ��Ʒ������Ʒ����
        /// </summary>
        public string Cb_Name
        {
            get { return _cb_Name; }
            set { _cb_Name = value; }
        }
        /// <summary>
        /// Ʒ�Ƶ�AllSpell
        /// </summary>
        public string Cb_AllSpell
        {
            get { return _cb_AllSpell; }
            set { _cb_AllSpell = value; }
        }
        /// <summary>
        /// ��Ʒ����������ID
        /// </summary>
        public int Cp_id
        {
            get { return _cp_id; }
            set { _cp_id = value; }
        }

        /// <summary>
        /// ��Ʒ������������
        /// </summary>
        public string Cp_Name
        {
            get { return _cp_Name; }
            set { _cp_Name = value; }
        }
		/// <summary>
		/// ������Ʒ������
		/// </summary>
		public string MasterName
		{
			get { return _masterName; }
			set { _masterName = value; }
		}

        /// <summary>
        /// ������Ʒ�Ƶ�URL
        /// </summary>
        public string MasterURL
        {
            get { return _masterURL; }
            set { _masterURL = value; }
        }

		/// <summary>
		/// �������̵Ķ�����
		/// </summary>
		public string Cp_ShortName
		{
			get { return _cpShortName; }
			set { _cpShortName = value; }
		}

        /// <summary>
        /// ָ��������
        /// </summary>
        public string ReferPriceRange { get; set; }
        #endregion
    }
}
