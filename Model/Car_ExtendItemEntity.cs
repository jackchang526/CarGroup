using System;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 车型扩展信息
    /// </summary>
    [Serializable]
    public class Car_ExtendItemEntity
    {
        #region 变量定义

        /// <summary>
        /// 车型ID
        /// </summary>
        private int _car_Id;

        /// <summary>
        /// 排量
        /// </summary>
        private string _car_Engine_Exhaust = string.Empty;

        /// <summary>
        /// 车门及厢式
        /// </summary>
        private string _car_Body_Type = string.Empty;

        /// <summary>
        /// 档位及变速器
        /// </summary>
        private string _car_UnderPan_TransmissionType = string.Empty;

        /// <summary>
        /// 质保
        /// </summary>
        private string _car_RepairPolicy = string.Empty;

        /// <summary>
        /// 上市时间
        /// </summary>
        private string _car_MarketDate = string.Empty;

        /// <summary>
        /// 车型名
        /// </summary>
        private string _car_Name = string.Empty;

        /// <summary>
        /// 车型PV
        /// </summary>
        private int _car_PV;

        #endregion

        #region 构造函数

        public Car_ExtendItemEntity()
        {
        }

        public Car_ExtendItemEntity
        (
            int car_Id,
            string car_Engine_Exhaust,
            string car_Body_Type,
            string car_UnderPan_TransmissionType,
            string car_RepairPolicy,
            string car_MarketDate,
            string car_Name,
            int car_PV
        )
        {
            _car_Id = car_Id;
            _car_Engine_Exhaust = car_Engine_Exhaust;
            _car_Body_Type = car_Body_Type;
            _car_UnderPan_TransmissionType = car_UnderPan_TransmissionType;
            _car_RepairPolicy = car_RepairPolicy;
            _car_MarketDate = car_MarketDate;
            _car_Name = car_Name;
            _car_PV = car_PV;
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

        /// <summary>
        /// 排量
        /// </summary>
        public string Car_Engine_Exhaust
        {
            get { return _car_Engine_Exhaust; }
            set { _car_Engine_Exhaust = value; }
        }

        /// <summary>
        /// 车门及厢式
        /// </summary>
        public string Car_Body_Type
        {
            get { return _car_Body_Type; }
            set { _car_Body_Type = value; }
        }

        /// <summary>
        /// 档位及变数器
        /// </summary>
        public string Car_UnderPan_TransmissionType
        {
            get { return _car_UnderPan_TransmissionType; }
            set { _car_UnderPan_TransmissionType = value; }
        }

        /// <summary>
        /// 质保
        /// </summary>
        public string Car_RepairPolicy
        {
            get { return _car_RepairPolicy; }
            set { _car_RepairPolicy = value; }
        }

        /// <summary>
        /// 上市时间
        /// </summary>
        public string Car_MarketDate
        {
            get { return _car_MarketDate; }
            set { _car_MarketDate = value; }
        }

        /// <summary>
        /// 车型名
        /// </summary>
        public string Car_Name
        {
            get { return _car_Name; }
            set { _car_Name = value; }
        }

        /// <summary>
        /// 车型PV
        /// </summary>
        public int Car_PV
        {
            get { return _car_PV; }
            set { _car_PV = value; }
        }

        #endregion
    }
}
