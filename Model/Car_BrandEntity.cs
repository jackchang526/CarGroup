using System;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// Ʒ�Ƽ�����Ϣ���伶����Ϣ
    /// </summary>
    [Serializable]
    public class Car_BrandEntity
    {
        #region ��������
        ///<summary>
        /// Ʒ��ID
        ///</summary>
        private int _cb_Id;
        ///<summary>
        /// Ʒ����������ID
        ///</summary>
        private int _cp_Id;
        ///<summary>
        /// Ʒ��������Ʒ��ID
        ///</summary>
        private int _bs_Id;
        ///<summary>
        /// Ʒ����
        ///</summary>
        private string _cb_Name = String.Empty;
        ///<summary>
        /// Ʒ�Ʊ���
        ///</summary>
        private string _cb_OtherName = String.Empty;
        ///<summary>
        /// Ʒ�ƹ���
        ///</summary>
        private string _cb_Country = String.Empty;
        ///<summary>
        /// Ʒ��Ӣ����
        ///</summary>
        private string _cb_EName = String.Empty;
        ///<summary>
        /// Ʒ�Ƶ绰
        ///</summary>
        private string _cb_Phone = String.Empty;
        ///<summary>
        /// Ʒ��URL
        ///</summary>
        private string _cb_url = String.Empty;
        ///<summary>
        /// Ʒ�Ƽ��
        ///</summary>
        private string _cb_introduction = String.Empty;
        ///<summary>
        /// Ʒ��LOGO
        ///</summary>
        private string _cb_Logo = String.Empty;
        ///<summary>
        /// Ʒ��ƴд
        ///</summary>
        private string _cb_spell = String.Empty;
        ///<summary>
        /// Ʒ�ƴ���ʱ��
        ///</summary>
        private DateTime _cb_createTime;
        ///<summary>
        /// Ʒ��״̬
        ///</summary>
        private int _cb_isState;
        ///<summary>
        /// Ʒ�ƶ�Ӧ�Ͽ�ϵ��ID
        ///</summary>
        private int _cb_oldCs_Id;
        ///<summary>
        /// Ʒ�Ƹ���ʱ��
        ///</summary>
        private DateTime _cb_updateTime;
        ///<summary>
        /// Ʒ��ȫƴ(��ַ��д)
        ///</summary>
        private string _cb_allSpell;
        /// <summary>
        /// Ʒ��SEO����
        /// </summary>
        private string _cb_SEOName;
        /// <summary>
        /// Ʒ��������Ʒ������
        /// </summary>
        private string _bs_Name;
        /// <summary>
        /// Ʒ��������Ʒ������Spell
        /// </summary>
        private string _bs_UrlSpell;
        /// <summary>
        /// Ʒ��������Ʒ�Ƶ�LOGO����
        /// </summary>
        private string _bs_LogoInfo;
        /// <summary>
        /// ���̼��
        /// </summary>
        private string _cp_ShortName;       
        /// <summary>
        /// ����Logo��ַ
        /// </summary>
        private string _cp_LogoUrl;       
        /// <summary>
        /// ��������
        /// </summary>
        private string _cp_Name;
        /// <summary>
        /// �������ӵ�ַ
        /// </summary>
        private string _cp_Url;
        /// <summary>
        /// ���̵绰
        /// </summary>
        private string _cp_Phone;
        /// <summary>
        /// ���̼��
        /// </summary>
        private string _cp_Introduction;
        /// <summary>
        /// ���̵Ĺ���
        /// </summary>
        private string _cp_Country;

        #endregion

        #region ���캯��
        ///<summary>
        ///
        ///</summary>
        public Car_BrandEntity()
        {
        }
        ///<summary>
        ///
        ///</summary>
        public Car_BrandEntity
        (
            int cb_Id,
            int cp_Id,
            int bs_Id,
            string cb_Name,
            string cb_OtherName,
            string cb_Country,
            string cb_EName,
            string cb_Phone,
            string cb_url,
            string cb_introduction,
            string cb_Logo,
            string cb_spell,
            DateTime cb_createTime,
            int cb_isState,
            int cb_oldCs_Id,
            DateTime cb_updateTime,
            string cb_allSpell
        )
        {
            _cb_Id = cb_Id;
            _cp_Id = cp_Id;
            _bs_Id = bs_Id;
            _cb_Name = cb_Name;
            _cb_OtherName = cb_OtherName;
            _cb_Country = cb_Country;
            _cb_EName = cb_EName;
            _cb_Phone = cb_Phone;
            _cb_url = cb_url;
            _cb_introduction = cb_introduction;
            _cb_Logo = cb_Logo;
            _cb_spell = cb_spell;
            _cb_createTime = cb_createTime;
            _cb_isState = cb_isState;
            _cb_oldCs_Id = cb_oldCs_Id;
            _cb_updateTime = cb_updateTime;
            _cb_allSpell = cb_allSpell;
        }
        #endregion

        #region ��������

        ///<summary>
        /// Ʒ��ID
        ///</summary>
        public int Cb_Id
        {
            get { return _cb_Id; }
            set { _cb_Id = value; }
        }

        ///<summary>
        /// ����ID
        ///</summary>
        public int Cp_Id
        {
            get { return _cp_Id; }
            set { _cp_Id = value; }
        }
        ///<summary>
        /// ��Ʒ��ID
        ///</summary>
        public int Bs_Id
        {
            get { return _bs_Id; }
            set { _bs_Id = value; }
        }

        ///<summary>
        /// Ʒ����
        ///</summary>
        public string Cb_Name
        {
            get { return _cb_Name; }
            set { _cb_Name = value; }
        }

        ///<summary>
        /// Ʒ�Ʊ���
        ///</summary>
        public string Cb_OtherName
        {
            get { return _cb_OtherName; }
            set { _cb_OtherName = value; }
        }

        ///<summary>
        /// Ʒ�ƹ���
        ///</summary>
        public string Cb_Country
        {
            get { return _cb_Country; }
            set { _cb_Country = value; }
        }

        ///<summary>
        /// Ʒ��Ӣ����
        ///</summary>
        public string Cb_EName
        {
            get { return _cb_EName; }
            set { _cb_EName = value; }
        }

        ///<summary>
        /// Ʒ�Ƶ绰
        ///</summary>
        public string Cb_Phone
        {
            get { return _cb_Phone; }
            set { _cb_Phone = value; }
        }

        ///<summary>
        /// Ʒ��URL
        ///</summary>
        public string Cb_url
        {
            get { return _cb_url; }
            set { _cb_url = value; }
        }

        ///<summary>
        /// Ʒ�Ƽ��
        ///</summary>
        public string Cb_introduction
        {
            get { return _cb_introduction; }
            set { _cb_introduction = value; }
        }

        ///<summary>
        /// Ʒ��LOGO
        ///</summary>
        public string Cb_Logo
        {
            get { return _cb_Logo; }
            set { _cb_Logo = value; }
        }

        ///<summary>
        /// Ʒ��ƴд
        ///</summary>
        public string Cb_Spell
        {
            get { return _cb_spell; }
            set { _cb_spell = value; }
        }

        ///<summary>
        /// Ʒ�ƴ���ʱ��
        ///</summary>
        public DateTime Cb_CreateTime
        {
            get { return _cb_createTime; }
            set { _cb_createTime = value; }
        }

        ///<summary>
        /// Ʒ��״̬
        ///</summary>
        public int Cb_IsState
        {
            get { return _cb_isState; }
            set { _cb_isState = value; }
        }

        ///<summary>
        /// Ʒ�ƶ�Ӧ�Ͽ�ϵ��ID
        ///</summary>
        public int Cb_OldCs_Id
        {
            get { return _cb_oldCs_Id; }
            set { _cb_oldCs_Id = value; }
        }

        ///<summary>
        /// Ʒ�Ƹ���ʱ��
        ///</summary>
        public DateTime Cb_UpdateTime
        {
            get { return _cb_updateTime; }
            set { _cb_updateTime = value; }
        }

        ///<summary>
        /// Ʒ��ȫƴ
        ///</summary>
        public string Cb_AllSpell
        {
            get { return _cb_allSpell; }
            set { _cb_allSpell = value; }
        }
        /// <summary>
        /// Ʒ��SEO����
        /// </summary>
        public string Cb_SEOName
        {
            get { return _cb_SEOName; }
            set { _cb_SEOName = value; }
        }
        /// <summary>
        /// Ʒ��������Ʒ����
        /// </summary>
        public string Bs_Name
        {
            get { return _bs_Name; }
            set { _bs_Name = value; }
        }
        /// <summary>
        /// Ʒ������������Spell
        /// </summary>
        public string Bs_UrlSpell
        {
            get { return _bs_UrlSpell; }
            set { _bs_UrlSpell = value; }
        }
        /// <summary>
        /// Ʒ��������Ʒ�Ƶ�LOGO����
        /// </summary>
        public string Bs_LogoInfo
        {
            get { return _bs_LogoInfo; }
            set { _bs_LogoInfo = value; }
        } 
        /// <summary>
        /// ���̼��
        /// </summary>
        public string Cp_ShortName
        {
            get { return _cp_ShortName; }
            set { _cp_ShortName = value; }
        }
        /// <summary>
        /// ����Logo��ַ
        /// </summary>
        public string Cp_LogoUrl
        {
            get { return _cp_LogoUrl; }
            set { _cp_LogoUrl = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string Cp_Name
        {
            get { return _cp_Name; }
            set { _cp_Name = value; }
        }
        /// <summary>
        /// �������ӵ�ַ
        /// </summary>
        public string Cp_Url
        {
            get { return _cp_Url; }
            set { _cp_Url = value; }
        }
        /// <summary>
        /// ���̼��
        /// </summary>
        public string Cp_Introduction
        {
            get { return _cp_Introduction; }
            set { _cp_Introduction = value; }
        }
        /// <summary>
        /// ���̵Ĺ���
        /// </summary>
        public string Cp_Country
        {
            get { return _cp_Country; }
            set { _cp_Country = value; }
        }
        /// <summary>
        /// ���̵绰
        /// </summary>
        public string Cp_Phone
        {
            get { return _cp_Phone; }
            set { _cp_Phone = value; }
        }
        #endregion
    }
}
