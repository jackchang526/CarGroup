using System;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 子品牌级别信息及其级别信息
    /// </summary>
    [Serializable]
    public class Car_SerialEntity
    {
        #region 变量定义
        ///<summary>
        /// 子品牌ID
        ///</summary>
        private int _cs_Id;
        ///<summary>
        /// 品牌ID
        ///</summary>
        private int _cb_Id;
        ///<summary>
        /// 子品牌名
        ///</summary>
        private string _cs_Name = String.Empty;
        ///<summary>
        /// 子品牌别名
        ///</summary>
        private string _cs_OtherName = String.Empty;
        ///<summary>
        /// 子品牌英文名
        ///</summary>
        private string _cs_EName = String.Empty;
        ///<summary>
        /// 子品牌URL
        ///</summary>
        private string _cs_Url = String.Empty;
        ///<summary>
        /// 子品牌电话
        ///</summary>
        private string _cs_Phone = String.Empty;
        ///<summary>
        /// 子品牌简介
        ///</summary>
        private string _cs_Introduction = String.Empty;
        ///<summary>
        /// 子品牌标签
        ///</summary>
        private string _cs_Tag = String.Empty;
        ///<summary>
        /// 子品牌图片
        ///</summary>
        private string _cs_Photo = String.Empty;
        ///<summary>
        /// 子品牌有点
        ///</summary>
        private string _cs_Virtues = String.Empty;
        ///<summary>
        /// 子品牌缺点
        ///</summary>
        private string _cs_Defect = String.Empty;
        ///<summary>
        /// 子品牌首字母拼写
        ///</summary>
        private string _cs_spell = String.Empty;
        ///<summary>
        /// 子品牌创建时间
        ///</summary>
        private DateTime _cs_createTime;
        ///<summary>
        /// 子品牌总要程度
        ///</summary>
        private string _cs_CarType = String.Empty;
        ///<summary>
        /// 子品牌级别
        ///</summary>
        private string _cs_CarLevel = String.Empty;
        ///<summary>
        /// 子品牌状态
        ///</summary>
        private int _cs_isState;
        ///<summary>
        /// 子品牌对应老库ID
        ///</summary>
        private int _cs_oldCb_Id;
        ///<summary>
        /// 子品牌更新时间
        ///</summary>
        private DateTime _cs_updateTime;
        ///<summary>
        /// 子品牌销售状态
        ///</summary>
        private string _cs_SaleState = String.Empty;
        ///<summary>
        /// 子品牌显示名称
        ///</summary>
        private string _cs_ShowName = String.Empty;
        ///<summary>
        /// 子品牌全拼
        ///</summary>
        private string _cs_allSpell = String.Empty;
        /// <summary>
        /// 子品牌SEOName
        /// </summary>
        private string _cs_SeoName = String.Empty;

        /// <summary>
        /// 子品牌所属品牌名
        /// </summary>
        private string _cb_Name = string.Empty;
        /// <summary>
        /// 品牌AllSpell
        /// </summary>
        private string _cb_AllSpell = string.Empty;

		/// <summary>
		/// 所属主品牌名称
		/// </summary>
		private string _masterName = string.Empty;

        /// <summary>
        /// 所属主品牌URL
        /// </summary>
        private string _masterURL = string.Empty;

        /// <summary>
        /// 子品牌所属厂商ID
        /// </summary>
        private int _cp_id;

        /// <summary>
        /// 子品牌所属厂商名
        /// </summary>
        private string _cp_Name = string.Empty;

		/// <summary>
		/// 子品牌所属厂商短名称
		/// </summary>
		private string _cpShortName = string.Empty;
        #endregion

        #region 构造函数

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

        #region 公共属性

        ///<summary>
        /// 子品牌ID
        ///</summary>
        public int Cs_Id
        {
            get { return _cs_Id; }
            set { _cs_Id = value; }
        }

        ///<summary>
        /// 品牌ID
        ///</summary>
        public int Cb_Id
        {
            get { return _cb_Id; }
            set { _cb_Id = value; }
        }

        ///<summary>
        /// 子品牌名
        ///</summary>
        public string Cs_Name
        {
            get { return _cs_Name; }
            set { _cs_Name = value; }
        }

        ///<summary>
        /// 子品牌别名
        ///</summary>
        public string Cs_OtherName
        {
            get { return _cs_OtherName; }
            set { _cs_OtherName = value; }
        }

        ///<summary>
        /// 子品牌英文名
        ///</summary>
        public string Cs_EName
        {
            get { return _cs_EName; }
            set { _cs_EName = value; }
        }

        ///<summary>
        /// 子品牌URL
        ///</summary>
        public string Cs_Url
        {
            get { return _cs_Url; }
            set { _cs_Url = value; }
        }

        ///<summary>
        /// 子品牌电话
        ///</summary>
        public string Cs_Phone
        {
            get { return _cs_Phone; }
            set { _cs_Phone = value; }
        }

        ///<summary>
        /// 子品牌简介
        ///</summary>
        public string Cs_Introduction
        {
            get { return _cs_Introduction; }
            set { _cs_Introduction = value; }
        }

        ///<summary>
        /// 子品牌标签
        ///</summary>
        public string Cs_Tag
        {
            get { return _cs_Tag; }
            set { _cs_Tag = value; }
        }

        ///<summary>
        /// 子品牌图片
        ///</summary>
        public string Cs_Photo
        {
            get { return _cs_Photo; }
            set { _cs_Photo = value; }
        }

        ///<summary>
        /// 子品牌优点
        ///</summary>
        public string Cs_Virtues
        {
            get { return _cs_Virtues; }
            set { _cs_Virtues = value; }
        }

        ///<summary>
        /// 子品牌缺点
        ///</summary>
        public string Cs_Defect
        {
            get { return _cs_Defect; }
            set { _cs_Defect = value; }
        }

        ///<summary>
        /// 子品牌首字母拼写
        ///</summary>
        public string Cs_Spell
        {
            get { return _cs_spell; }
            set { _cs_spell = value; }
        }

        ///<summary>
        /// 子品牌创建时间
        ///</summary>
        public DateTime Cs_CreateTime
        {
            get { return _cs_createTime; }
            set { _cs_createTime = value; }
        }

        ///<summary>
        /// 子品牌重要程度
        ///</summary>
        public string Cs_CarType
        {
            get { return _cs_CarType; }
            set { _cs_CarType = value; }
        }

        ///<summary>
        /// 子品牌级别
        ///</summary>
        public string Cs_CarLevel
        {
            get { return _cs_CarLevel; }
            set { _cs_CarLevel = value; }
        }

        ///<summary>
        /// 子品牌状态
        ///</summary>
        public int Cs_IsState
        {
            get { return _cs_isState; }
            set { _cs_isState = value; }
        }

        ///<summary>
        /// 子品牌对应老库品牌ID
        ///</summary>
        public int Cs_OldCb_Id
        {
            get { return _cs_oldCb_Id; }
            set { _cs_oldCb_Id = value; }
        }

        ///<summary>
        /// 子品牌更新时间
        ///</summary>
        public DateTime Cs_UpdateTime
        {
            get { return _cs_updateTime; }
            set { _cs_updateTime = value; }
        }

        ///<summary>
        /// 子品牌销售状态
        ///</summary>
        public string Cs_SaleState
        {
            get { return _cs_SaleState; }
            set { _cs_SaleState = value; }
        }

        ///<summary>
        /// 子品牌显示名称
        ///</summary>
        public string Cs_ShowName
        {
            get { return _cs_ShowName; }
            set { _cs_ShowName = value; }
        }

        ///<summary>
        /// 子品牌全拼
        ///</summary>
        public string Cs_AllSpell
        {
            get { return _cs_allSpell; }
            set { _cs_allSpell = value; }
        }
        /// <summary>
        /// 子品牌SEO名
        /// </summary>
        public string Cs_SeoName
        {
            get { return _cs_SeoName; }
            set { _cs_SeoName = value; }
        }
        /// <summary>
        /// 子品牌所属品牌名
        /// </summary>
        public string Cb_Name
        {
            get { return _cb_Name; }
            set { _cb_Name = value; }
        }
        /// <summary>
        /// 品牌的AllSpell
        /// </summary>
        public string Cb_AllSpell
        {
            get { return _cb_AllSpell; }
            set { _cb_AllSpell = value; }
        }
        /// <summary>
        /// 子品牌所属厂商ID
        /// </summary>
        public int Cp_id
        {
            get { return _cp_id; }
            set { _cp_id = value; }
        }

        /// <summary>
        /// 子品牌所属厂商名
        /// </summary>
        public string Cp_Name
        {
            get { return _cp_Name; }
            set { _cp_Name = value; }
        }
		/// <summary>
		/// 所属主品牌名称
		/// </summary>
		public string MasterName
		{
			get { return _masterName; }
			set { _masterName = value; }
		}

        /// <summary>
        /// 所属主品牌的URL
        /// </summary>
        public string MasterURL
        {
            get { return _masterURL; }
            set { _masterURL = value; }
        }

		/// <summary>
		/// 所属厂商的短名称
		/// </summary>
		public string Cp_ShortName
		{
			get { return _cpShortName; }
			set { _cpShortName = value; }
		}

        /// <summary>
        /// 指导价区间
        /// </summary>
        public string ReferPriceRange { get; set; }
        #endregion
    }
}
