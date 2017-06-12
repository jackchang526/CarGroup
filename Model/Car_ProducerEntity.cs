using System;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 厂商信息
    /// </summary>
    [Serializable]
    public class Car_ProducerEntity
    {
        #region 变量定义
        ///<summary>
        /// 厂商ID
        ///</summary>
        private int _cp_Id;
        ///<summary>
        /// 厂商名
        ///</summary>
        private string _cp_Name = String.Empty;
        ///<summary>
        /// 厂商简称
        ///</summary>
        private string _cp_ShortName = String.Empty;
        ///<summary>
        /// 厂商别名
        ///</summary>
        private string _cp_Byname = String.Empty;
        ///<summary>
        /// 厂商英文名
        ///</summary>
        private string _cp_EName = String.Empty;
        ///<summary>
        /// 厂商国家
        ///</summary>
        private string _cp_Country = String.Empty;
        ///<summary>
        /// 厂商URL
        ///</summary>
        private string _cp_Url = String.Empty;
        ///<summary>
        /// 厂商电话
        ///</summary>
        private string _cp_Phone = String.Empty;
        ///<summary>
        /// 厂商创建时间
        ///</summary>
        private DateTime _cp_createTime;
        ///<summary>
        /// 厂商拼写
        ///</summary>
        private string _cp_spell = String.Empty;
        ///<summary>
        /// 厂商简介
        ///</summary>
        private string _cp_Introduction = String.Empty;
        ///<summary>
        /// 厂商LOGO
        ///</summary>
        private string _cp_LogoUrl = String.Empty;
        ///<summary>
        /// 厂商状态
        ///</summary>
        private int _cp_isState;
        ///<summary>
        /// 厂商更新时间
        ///</summary>
        private DateTime _cp_updateTime;
        ///<summary>
        /// 厂商对应老库厂商ID
        ///</summary>
        private int _cp_oldCp_Id;
        /// <summary>
        /// 厂商的SEONAME
        /// </summary>
        private string _cp_seoName;
        #endregion

        #region 构造函数
        ///<summary>
        ///
        ///</summary>
        public Car_ProducerEntity()
        {
        }
        ///<summary>
        ///
        ///</summary>
        public Car_ProducerEntity
        (
            int cp_Id,
            string cp_Name,
            string cp_ShortName,
            string cp_Byname,
            string cp_EName,
            string cp_Country,
            string cp_Url,
            string cp_Phone,
            DateTime cp_createTime,
            string cp_spell,
            string cp_Introduction,
            string cp_LogoUrl,
            int cp_isState,
            DateTime cp_updateTime,
            int cp_oldCp_Id
        )
        {
            _cp_Id = cp_Id;
            _cp_Name = cp_Name;
            _cp_ShortName = cp_ShortName;
            _cp_Byname = cp_Byname;
            _cp_EName = cp_EName;
            _cp_Country = cp_Country;
            _cp_Url = cp_Url;
            _cp_Phone = cp_Phone;
            _cp_createTime = cp_createTime;
            _cp_spell = cp_spell;
            _cp_Introduction = cp_Introduction;
            _cp_LogoUrl = cp_LogoUrl;
            _cp_isState = cp_isState;
            _cp_updateTime = cp_updateTime;
            _cp_oldCp_Id = cp_oldCp_Id;

        }

        ///<summary>
        ///
        ///</summary>
        public Car_ProducerEntity
        (
            int cp_Id,
            string cp_Name,
            string cp_ShortName,
            string cp_Byname,
            string cp_EName,
            string cp_Country,
            string cp_Url,
            string cp_Phone,
            DateTime cp_createTime,
            string cp_spell,
            string cp_Introduction,
            string cp_LogoUrl,
            int cp_isState,
            DateTime cp_updateTime,
            int cp_oldCp_Id,
            string cp_SeoName
        )
        {
            _cp_Id = cp_Id;
            _cp_Name = cp_Name;
            _cp_ShortName = cp_ShortName;
            _cp_Byname = cp_Byname;
            _cp_EName = cp_EName;
            _cp_Country = cp_Country;
            _cp_Url = cp_Url;
            _cp_Phone = cp_Phone;
            _cp_createTime = cp_createTime;
            _cp_spell = cp_spell;
            _cp_Introduction = cp_Introduction;
            _cp_LogoUrl = cp_LogoUrl;
            _cp_isState = cp_isState;
            _cp_updateTime = cp_updateTime;
            _cp_oldCp_Id = cp_oldCp_Id;
            _cp_seoName = cp_SeoName;
        }
        #endregion

        #region 公共属性

        ///<summary>
        /// 厂商ID
        ///</summary>
        public int Cp_Id
        {
            get { return _cp_Id; }
            set { _cp_Id = value; }
        }

        ///<summary>
        /// 厂商名
        ///</summary>
        public string Cp_Name
        {
            get { return _cp_Name; }
            set { _cp_Name = value; }
        }

        ///<summary>
        /// 厂商简称
        ///</summary>
        public string Cp_ShortName
        {
            get { return _cp_ShortName; }
            set { _cp_ShortName = value; }
        }

        ///<summary>
        /// 厂商别名
        ///</summary>
        public string Cp_Byname
        {
            get { return _cp_Byname; }
            set { _cp_Byname = value; }
        }

        ///<summary>
        /// 厂商英文名
        ///</summary>
        public string Cp_EName
        {
            get { return _cp_EName; }
            set { _cp_EName = value; }
        }

        ///<summary>
        /// 厂商国家
        ///</summary>
        public string Cp_Country
        {
            get { return _cp_Country; }
            set { _cp_Country = value; }
        }

        ///<summary>
        /// 厂商URL
        ///</summary>
        public string Cp_Url
        {
            get { return _cp_Url; }
            set { _cp_Url = value; }
        }

        ///<summary>
        /// 厂商电话
        ///</summary>
        public string Cp_Phone
        {
            get { return _cp_Phone; }
            set { _cp_Phone = value; }
        }

        ///<summary>
        /// 厂商创建时间
        ///</summary>
        public DateTime Cp_CreateTime
        {
            get { return _cp_createTime; }
            set { _cp_createTime = value; }
        }

        ///<summary>
        /// 厂商拼写
        ///</summary>
        public string Cp_Spell
        {
            get { return _cp_spell; }
            set { _cp_spell = value; }
        }

        ///<summary>
        /// 厂商简介
        ///</summary>
        public string Cp_Introduction
        {
            get { return _cp_Introduction; }
            set { _cp_Introduction = value; }
        }

        ///<summary>
        /// 厂商URL
        ///</summary>
        public string Cp_LogoUrl
        {
            get { return _cp_LogoUrl; }
            set { _cp_LogoUrl = value; }
        }

        ///<summary>
        /// 厂商状态
        ///</summary>
        public int Cp_IsState
        {
            get { return _cp_isState; }
            set { _cp_isState = value; }
        }

        ///<summary>
        /// 厂商更新时间
        ///</summary>
        public DateTime Cp_UpdateTime
        {
            get { return _cp_updateTime; }
            set { _cp_updateTime = value; }
        }

        ///<summary>
        /// 厂商对应老库厂商ID
        ///</summary>
        public int Cp_OldCp_Id
        {
            get { return _cp_oldCp_Id; }
            set { _cp_oldCp_Id = value; }
        }
        /// <summary>
        /// 厂商的SEO名称
        /// </summary>
        public string Cp_seoName
        {
            get { return _cp_seoName; }
            set { _cp_seoName = value; }
        }
        #endregion
    }
}
