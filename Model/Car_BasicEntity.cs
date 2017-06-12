using System;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 车型基本信息及其级别信息
    /// </summary>
    [Serializable]
    public class Car_BasicEntity
    {
        #region 变量定义
        ///<summary>
        /// 车型ID
        ///</summary>
        private int _car_Id;

        ///<summary>
        /// 车型生产状态
        ///</summary>
        private string _car_ProduceState = String.Empty;

        ///<summary>
        /// 车型销售状态
        ///</summary>
        private string _car_SaleState = String.Empty;

        ///<summary>
        /// 车型所属子品牌ID
        ///</summary>
        private int _cs_Id;

        ///<summary>
        /// 车型名
        ///</summary>
        private string _car_Name = String.Empty;

        ///<summary>
        /// 车型首字母拼写
        ///</summary>
        private string _car_spellFirst = String.Empty;

        ///<summary>
        /// 车型厂商指导价
        ///</summary>
        private double _car_ReferPrice;

        ///<summary>
        /// 车型状态
        ///</summary>
        private int _car_isState;

        ///<summary>
        /// 车型是否锁定
        ///</summary>
        private int _car_isLock;

        ///<summary>
        /// 车型对应老库ID
        ///</summary>
        private long _car_oLdCar_Id;

        ///<summary>
        /// 车型创建时间
        ///</summary>
        private DateTime _car_createTime;

        ///<summary>
        /// 车型更新时间
        ///</summary>
        private DateTime _car_updateTime;

        /// <summary>
        /// 车型年款
        /// </summary>
        private int _car_YearType;

        /// <summary>
        /// 车型所属子品牌名
        /// </summary>
        private string _cs_Name = string.Empty;

        /// <summary>
        /// 车型所属子品牌显示名
        /// </summary>
        private string _cs_ShowName = string.Empty;

        /// <summary>
        /// 车型所属子品牌的全拼(地址重写)
        /// </summary>
        private string _cs_AllSpell = string.Empty;

        /// <summary>
        /// 车型所属品牌ID
        /// </summary>
        private int _cb_id;

        /// <summary>
        /// 车型所属品牌名
        /// </summary>
        private string _cb_Name = string.Empty;

        /// <summary>
        /// 车型所属厂商ID
        /// </summary>
        private int _cp_id;

        /// <summary>
        /// 车型所属厂商名
        /// </summary>
        private string _cp_Name = string.Empty;

		/// <summary>
		/// 所属主品牌名称
		/// </summary>
		private string _masterName = string.Empty;
        #endregion

        #region 构造函数
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

        #region 公共属性

        ///<summary>
        /// 车型ID
        ///</summary>
        public int Car_Id
        {
            get { return _car_Id; }
            set { _car_Id = value; }
        }

        ///<summary>
        /// 车型生产状态
        ///</summary>
        public string Car_ProduceState
        {
            get { return _car_ProduceState; }
            set { _car_ProduceState = value; }
        }

        ///<summary>
        /// 车型销售状态
        ///</summary>
        public string Car_SaleState
        {
            get { return _car_SaleState; }
            set { _car_SaleState = value; }
        }

        ///<summary>
        /// 车型所属子品牌ID
        ///</summary>
        public int Cs_Id
        {
            get { return _cs_Id; }
            set { _cs_Id = value; }
        }

        ///<summary>
        /// 车型名
        ///</summary>
        public string Car_Name
        {
            get { return _car_Name; }
            set { _car_Name = value; }
        }

        ///<summary>
        /// 车型首字母
        ///</summary>
        public string Car_SpellFirst
        {
            get { return _car_spellFirst; }
            set { _car_spellFirst = value; }
        }

        ///<summary>
        /// 车型厂商指导价
        ///</summary>
        public double Car_ReferPrice
        {
            get { return _car_ReferPrice; }
            set { _car_ReferPrice = value; }
        }

        ///<summary>
        /// 车型状态
        ///</summary>
        public int Car_IsState
        {
            get { return _car_isState; }
            set { _car_isState = value; }
        }

        ///<summary>
        /// 车型是否被锁定
        ///</summary>
        public int Car_IsLock
        {
            get { return _car_isLock; }
            set { _car_isLock = value; }
        }

        ///<summary>
        /// 车型对应老库ID
        ///</summary>
        public long Car_OLdCar_Id
        {
            get { return _car_oLdCar_Id; }
            set { _car_oLdCar_Id = value; }
        }

        ///<summary>
        /// 车型创建时间
        ///</summary>
        public DateTime Car_CreateTime
        {
            get { return _car_createTime; }
            set { _car_createTime = value; }
        }

        ///<summary>
        /// 车型更新时间
        ///</summary>
        public DateTime Car_UpdateTime
        {
            get { return _car_updateTime; }
            set { _car_updateTime = value; }
        }

        /// <summary>
        /// 车型年款
        /// </summary>
        public int Car_YearType
        {
            get { return _car_YearType; }
            set { _car_YearType = value; }
        }

        /// <summary>
        /// 车型所属子品牌名
        /// </summary>
        public string Cs_Name
        {
            get { return _cs_Name; }
            set { _cs_Name = value; }
        }

        /// <summary>
        /// 车型所属子品牌显示名
        /// </summary>
        public string Cs_ShowName
        {
            get { return _cs_ShowName; }
            set { _cs_ShowName = value; }
        }

        /// <summary>
        /// 车型所属子品牌全拼(地址重写)
        /// </summary>
        public string Cs_AllSpell
        {
            get { return _cs_AllSpell; }
            set { _cs_AllSpell = value; }
        }

        /// <summary>
        /// 车型所属品牌ID
        /// </summary>
        public int Cb_id
        {
            get { return _cb_id; }
            set { _cb_id = value; }
        }

        /// <summary>
        /// 车型所属品牌名
        /// </summary>
        public string Cb_Name
        {
            get { return _cb_Name; }
            set { _cb_Name = value; }
        }

        /// <summary>
        /// 车型所属厂商ID
        /// </summary>
        public int Cp_id
        {
            get { return _cp_id; }
            set { _cp_id = value; }
        }

        /// <summary>
        /// 车型所属厂商名
        /// </summary>
        public string Cp_Name
        {
            get { return _cp_Name; }
            set { _cp_Name = value; }
        }

		/// <summary>
		/// 主品牌名称
		/// </summary>
		public string MasterName
		{
			get { return _masterName; }
			set { _masterName = value; }
		}
        #endregion
    }
}
